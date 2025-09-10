using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Contexts;
using System.Text;
using YSPFrom.Hubs.PlayerHub;
using System.Threading.Tasks;
using YSPFrom.Core.Logging;
using YSPFrom.Core.RTP;
using YSPFrom.Core.SuperJackpot;
using YSPFrom.Hubs;
using YSPFrom.Models;
using static YSPFrom.Core.Logging.LogManager;

namespace YSPFrom
{
    public class ChatHub : Hub
    {
        //public void Send(string user, string message)   // 這是給 client 呼叫的
        //{
        //    // 廣播訊息給所有 client
        //    Clients.All.broadcastMessage(user, message);
        //}

        // =========================== Hub =====================================
        public override Task OnConnected()
        {
            string msg = $"⚡有新連線進來 ConnId={Context.ConnectionId}";
            Console.WriteLine(msg);
            Program.MainForm?.LogConnectionCheck(msg);
            return base.OnConnected();
        }

        #region 玩家斷線時自動清除 ConnectionId
        public override Task OnDisconnected(bool stopCalled)
        {
            var connId = Context.ConnectionId;

            // 找出斷線的玩家
            var player = PlayerManager.GetByConnectionId(connId);
            if (player != null)
            {
                //ClearConnection(player, "斷線");
                ClearConnection.Clear(player, "正常斷線");    // 讀 ClearConnection.cs 的 Clear 方法
            }
            else
            {
                Console.WriteLine($"⚠️ OnDisconnected：未知連線 ConnId={connId}");
            }

            return base.OnDisconnected(stopCalled);
        }
        #endregion

        // ====================== 玩家登入/管理 ======================
        #region 玩家登入，檢查帳號/密碼，綁定 ConnectionId
        private static readonly object _loginLock = new object();
        public object Login(dynamic loginData)
        {
            string username = loginData.username;
            string password = loginData.password;

            var (ok, msg, player) = PlayerManager.Login(username, password, Context.ConnectionId, Clients);

            if (!ok)
            {
                return new { succes = false, message = msg };
            }

            // 真正記錄玩家登入
            Console.WriteLine($"玩家 {username} 登入成功，餘額：{player.Balance} (ConnId: {player.ConnectionId})");
            LogManager.LotteryLog(LogManager.LotteryLogType.ClientConnected, player.UserId, player.Balance);

            return new { success = true, message = "登入成功", username = player.UserId, balance = player.Balance };
        }
        #endregion

        // ====================== 遊戲下注流程 ======================
        #region 下注流程：根據 ConnectionId 找玩家
        public void StartLottery(BetData data)
        {

            // 找玩家用 ConnectionId 比較安全)
            string roundId = Core.Utils.RoundIdGenerator.NextIdString();

            //var player = playersDb.Values.FirstOrDefault(p => p.ConnectionId == Context.ConnectionId);
            //Player player = playersDb.Values.FirstOrDefault(p => p.ConnectionId == Context.ConnectionId);
            var player = PlayerManager.GetByConnectionId(Context.ConnectionId);


            if (player == null)
            {
                Clients.Caller.lotteryResult(new LotteryResponse
                {
                    insufficientBalance = true,
                    message = "未登入"
                });
                return;
            }
            Console.WriteLine($"[StartLottery] round={roundId}, player={player.UserId}, totalBet={data.totalBet}, balanceBefore={player.Balance}");

            // === 0) 基本防呆：檢查 data 是否有效(防止 Heisenbug 時序敏感) ===
            if (data == null || data.totalBet <= 0)
            {
                Clients.Caller.lotteryResult(new LotteryResponse
                {
                    message = "下注金額錯誤",
                    balanceBefore = player?.Balance ?? 0,
                    balanceAfter = player?.Balance ?? 0,
                    totalBet = data?.totalBet ?? 0
                });

                Console.WriteLine($"[StartLottery] round={roundId}, totalBet 無效={data?.totalBet}");
                return;
            }

            // 檢查餘額是否足夠
            if (player.Balance < data.totalBet)
            {
                Clients.Caller.lotteryResult(new LotteryResponse
                {
                    insufficientBalance = true,
                    balanceBefore = player.Balance,
                    balanceAfter = player.Balance,
                    totalBet = data.totalBet,
                    message = "餘額不足"
                });
                return;
            }


            // ================= 進入金流關鍵路徑 ================
            // 扣除下注金額
            var before = player.Balance;        // 抽獎前餘額
            LotteryLog(LotteryLogType.BalanceBeforeBet, player.UserId, player.Balance, data.totalBet);

            // ) 扣款（唯一扣點）F
            player.Balance -= data.totalBet;    // 扣住
            //var afterDebit = player.Balance;    // 扣住後餘額( 還未派彩 )
            LotteryLog(LotteryLogType.BalanceAfterBet, player.Balance);     // 統一管理log 

            // ) 記錄下注資料（確保不會是上局殘留）
            LogManager.LotteryLog(LogManager.LotteryLogType.BetDataReceived, data.totalBet, data.isAutoMode);       // 統一管理 Log
            LogManager.LotteryLog(LogManager.LotteryLogType.BetAreaReceived, data.betAmounts);

            // 7) 開獎（務必確定不改 player.Balance）
            var result = LotteryService.CalculateLotteryResult(player, data);

            // 派彩加回餘額( 使用 LotteryResponse)
            player.Balance += result.payout;
            var afterCredit = player.Balance;  // 派彩後餘額( 最終 )
            //LotteryLog(LotteryLogType.BalanceAfterPayout, result.payout, afterCredit);  // 統一管理log 

            // 9) 應得關係式（防呆核對，若不相等就寫 Log 便於抓 bug）
            long expectedAfter = before - data.totalBet + result.payout;
            if (afterCredit != expectedAfter)
            {

                // 可視需要在此強制修正：
                player.Balance = expectedAfter;
                afterCredit = expectedAfter;
            }
            LotteryLog(LotteryLogType.BalanceAfterPayout, result.payout, player.Balance); // 統一管理log 

            // 回傳完整封包
            var response = new LotteryResponse
            {
                //result = result,            // 抽獎結果資料
                roundId = roundId,
                balanceBefore = before,     // 抽獎前餘額
                balanceAfter = afterCredit, // 最終餘額
                totalBet = data.totalBet,   // 餘額充足, 成功扣款開獎
                message = "OK",             // 成功訊息
                // netChange 會自動算，不用另外賦值
            };

            // === RoundSummary（每局必記）===
            LotteryLog(LotteryLogType.RoundSummary,
                result.rewardName,
                response.totalBet,
                result.multiplier,
                response.netChange);

            // 事件：只丟轉盤結果，給前端動畫用
            Clients.Caller.broadcastLotteryResult(result);

            // 回傳給當事人
            Clients.Caller.lotteryResult(response);

            // === 系統資訊 ===
            LotteryLog(LotteryLogType.OtherInfo,
                "OK",  // args[0] 目前沒用，可以放 "OK"
                RTPManager.GetCurrentRTP(),
                RTPManager.totalBets,
                RTPManager.totalPayouts,
                response.balanceAfter,      // 當前玩家餘額 or 機台餘額，看你要印哪個
                SuperJackpotPool.PoolBalance);
        }
        #endregion

        #region 單區下注(即時下注更新)
        public void PlaceBet(string areaName, int amount)
        {
            var player = PlayerManager.GetByConnectionId(Context.ConnectionId);
            if (player == null) return;

            // 餘額檢查
            if (player.Balance >= amount)
            {
                player.Balance -= amount;

                if (!player.CurrentRoundBets.ContainsKey(areaName))
                    player.CurrentRoundBets[areaName] = 0;

                player.CurrentRoundBets[areaName] += amount;

                // ✅ 傳回餘額與下注紀錄
                Clients.Caller.broadcastMessage("LotteryBalanceUpdate", new
                {
                    balance = player.Balance,
                    betAmounts = player.CurrentRoundBets
                });
            }
            else
            {
                Clients.Caller.broadcastMessage("LotteryBalanceUpdate", new
                {
                    balance = player.Balance,
                    betAmounts = player.CurrentRoundBets,
                    message = "餘額不足"
                });
            }
        }
        #endregion

        // ====================== 心跳維護 ======================
        #region Ping (心跳)
        public void Ping()
        {
            var player = PlayerManager.GetByConnectionId(Context.ConnectionId);
            if (player == null)
            {
                // 尚未登入的裸連線，直接忽略
                string msg = $"⚠️ 收到未知連線的 Ping (ConnId={Context.ConnectionId})";
                Console.WriteLine(msg);
                Program.MainForm?.LogConnectionCheck(msg);
                return;
            }
            // 有玩家 → 更新心跳
            HeartbeatManager.UpdateHeartbeat(player.UserId);
            //string okmsg = $"✅ Ping: {player.UserId}";
            string okmsg = $"✅ Ping: 帳號={player.UserId}, 連線ID={player.ConnectionId}";
            Console.WriteLine(okmsg);
            Program.MainForm?.LogConnectionCheck(okmsg);  // 顯示右視窗

            // 可選：Console.WriteLine($"收到 Ping: {player.UserId}");
        }
        #endregion
    }
}
