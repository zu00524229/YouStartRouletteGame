using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSPFrom.Core.Logging;
using YSPFrom.Core.RTP;
using YSPFrom.Core.SuperJackpot;
using YSPFrom.Models;
using static YSPFrom.Core.Logging.LogManager;

namespace YSPFrom
{
    public class ChatHub : Hub
    {
        public void Send(string user, string message)   // 這是給 client 呼叫的
        {
            // 廣播訊息給所有 client
            Clients.All.broadcastMessage(user, message);
        }

        public override Task OnConnected()
        {
            Console.WriteLine("Hub 名稱: " + this.Context.ConnectionId);
            Console.WriteLine("Hub 類型: " + this.GetType().Name);

            // 這裡只記錄 ConnId，不要傳 userId
            string msg = $"⚡ 有新連線進來 ConnId={Context.ConnectionId}";
            Console.WriteLine(msg);
            Program.MainForm?.LogPlayerStatus(msg);

            return base.OnConnected();
        }

        // ✅ 當玩家斷線，自動清除 ConnectionId
        public override Task OnDisconnected(bool stopCalled)
        {
            var connId = Context.ConnectionId;

            // 找出斷線的玩家
            var player = playersDb.Values.FirstOrDefault(p => p.ConnectionId == connId);
            if (player != null)
            {
                Console.WriteLine($"玩家 {player.UserId} 已斷線，清除連線ID (ConnId={connId})");
                LogManager.LotteryLog(LogManager.LotteryLogType.ClientDisconnected, player.UserId);
                LogManager.LotteryLog(LogManager.LotteryLogType.PlayerLogoutBalance, player.UserId, player.Balance);

                player.ConnectionId = null; // 清掉斷線
            }

            return base.OnDisconnected(stopCalled);
        }

        #region // 從 data 取得下注資料(舊StartLottery)
        //呼叫 form?.LogBet() 方法來列印下注資料
        //public void StartLottery(BetData data)
        //{
        //    //Console.WriteLine(" 收到下注資料：");
        //    //Console.WriteLine($"自動模式: {data.isAutoMode}");

        //    //var form = (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
        //    //form?.LogBet("收到下注資料：");
        //    //form?.LogBet($"總下注: {data.totalBet}");
        //    //form?.LogBet($"自動模式: {data.isAutoMode}");

        //    //form?.LogBase("有前端連進來!");

        //    //form?.LogRTP($"RTP: {LotteryService.GetCurrentRTP()}");


        //    LogManager.LotteryLog(LogManager.LotteryLogType.BetDataReceived, data.totalBet, data.isAutoMode);       // 統一管理 Log
        //    //var date = new YSPFrom.Core.BetData();
        //    foreach (var entry in data.betAmounts)
        //    {
        //        LogManager.LotteryLog(LogManager.LotteryLogType.BetAreaReceived, entry.Key, entry.Value);       // 統一管理 Log
        //    }
        //    var result = LotteryService.CalculateLotteryResult(data);

        //    Clients.Caller.broadcastLotteryResult(result);

        //    // TODO: 你可以在這裡回傳中獎結果
        //    //var form = (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
        //    //form?.LogResult($"🎰 中獎結果: {selectedReward} x{multiplier} → 派彩 {winAmount}");
        //}
        #endregion


        #region // 玩家

        // 玩家資料庫（假資料）
        private static readonly Dictionary<string, Player> playersDb = new Dictionary<string, Player>
    {
        { "ethan",  new Player { UserId = "ethan",  Passworld = "zxc123", Balance = 5000000 } },
        { "player", new Player { UserId = "player", Passworld = "zxc123", Balance = 10000000 } },
        { "player2", new Player { UserId = "player2", Passworld = "zxc123", Balance = 10000000 } },
        { "player3", new Player { UserId = "player3", Passworld = "zxc123", Balance = 10000000 } },
        { "player4", new Player { UserId = "player4", Passworld = "zxc123", Balance = 10000000 } },
        { "player5", new Player { UserId = "player5", Passworld = "zxc123", Balance = 10000000 } },
        { "player6", new Player { UserId = "player6", Passworld = "zxc123", Balance = 10000000 } },
    };

        // 登入並綁定 ConnectionId
        public object Login(dynamic loginData)
        {
            string username = loginData.username;
            string password = loginData.password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return new { success = false, message = "帳號或密碼不得為空" };
            }

            if (!playersDb.ContainsKey(username) || playersDb[username].Passworld != password)
            {
                return new { success = false, message = "帳號或密碼錯誤" };
            }

            var player = playersDb[username];
            

            // 🔑 檢查是否已經有人登入
            if (!string.IsNullOrEmpty(player.ConnectionId))
            {
                // 如果你要拒絕新登入：
                return new { success = false, message = "此帳號已在其他地方登入" };

                // 如果你要踢掉舊連線（只允許新連線）：
                // Clients.Client(player.ConnectionId).SendAsync("ForceLogout", "帳號已在別處登入");
                // Console.WriteLine($"玩家 {username} 被新連線擠下線 (舊ConnId={player.ConnectionId}, 新ConnId={Context.ConnectionId})");
            }

            player.ConnectionId = Context.ConnectionId; // 綁定連線ID
            Console.WriteLine($"玩家 {username} 登入成功，餘額：{player.Balance} (ConnId: {player.ConnectionId})");

            // 真正記錄玩家登入
            LogManager.LotteryLog(LogManager.LotteryLogType.ClientConnected, player.UserId);

            return new
            {
                success = true,
                message = "登入成功",
                username = player.UserId,   // 帳號
                balance = player.Balance
            };
        }



        // ✅ 下注流程：根據 ConnectionId 找玩家
        public void StartLottery(BetData data)
        {
            // 找玩家用 ConnectionId 比較安全)
            var player = playersDb.Values.FirstOrDefault(p => p.ConnectionId == Context.ConnectionId);
            if (player == null)
            {
                Clients.Caller.lotteryResult(new LotteryResponse
                {
                    insufficientBalance = true,
                    message = "未登入"
                });
                return;
            }

            // 檢查餘額是否足夠
            if  (player.Balance < data.totalBet)
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

            // 扣除下注金額
            var before = player.Balance;        // 抽獎前餘額
            LotteryLog(LotteryLogType.BalanceBeforeBet, player.UserId, player.Balance, data.totalBet);

            player.Balance -= data.totalBet;    // 扣住
            var afterDebit = player.Balance;    // 扣住後餘額( 還未派彩 )
            LotteryLog(LotteryLogType.BalanceAfterBet, player.Balance);     // 統一管理log 

            // 下注資料
            LogManager.LotteryLog(LogManager.LotteryLogType.BetDataReceived, data.totalBet, data.isAutoMode);       // 統一管理 Log
            LogManager.LotteryLog(LogManager.LotteryLogType.BetAreaReceived, data.betAmounts);


            // 計算抽獎結果( 不改動餘額)
            var result = LotteryService.CalculateLotteryResult(player, data);

            // 派彩加回餘額( 使用 LotteryResponse)
            player.Balance += result.payout;
            var afterCredit = player.Balance;  // 派彩後餘額( 最終 )
            LotteryLog(LotteryLogType.BalanceAfterPayout, result.payout, afterCredit);  // 統一管理log 

            // 回傳完整封包
            var response = new LotteryResponse
            {
                //result = result,            // 抽獎結果資料
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

    }
}
