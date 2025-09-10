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
using YSPFrom.Hubs.BetHub;

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


            var (response, result) = BetManager.StartLottery(player, data, roundId);

            if (result == null) // 如果 result == null，代表下注失敗（超過上限 / 餘額不足 / 未登入 ...）
            {
                Clients.Caller.lotteryResult(response); // 直接回傳錯誤給前端，不要繼續走轉盤動畫
                return;
            }

            #region //// 邏輯移到BetManager 管理
            
            #endregion

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

            var (balance, betAmounts, message) = BetManager.PlaceBet(player, areaName, amount);

            if (!string.IsNullOrEmpty(message))
            {
                // 如果餘額不足 / 超過上限
                Clients.Caller.broadcastMessage("LotteryBalanceUpdate", new
                {
                    balance,
                    betAmounts,
                    message
                });
                return; // return 不要往下走
            }

            // ✅ 成功下注
            Clients.Caller.broadcastMessage("LotteryBalanceUpdate", new
            {
                balance,
                betAmounts,
            });

            //// 餘額檢查 邏輯移到BetManager 管理

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
