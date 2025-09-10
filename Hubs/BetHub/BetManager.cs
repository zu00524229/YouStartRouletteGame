using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using YSPFrom.Core.Logging;
using YSPFrom.Core.RTP;
using YSPFrom.Core.SuperJackpot;
using YSPFrom.Models;
using static YSPFrom.Core.Logging.LogManager;

namespace YSPFrom.Hubs.BetHub
{
    public static class BetManager
    {
        // ====================== 遊戲下注流程 ======================
        #region 下注流程：根據 ConnectionId 找玩家
        public static (LotteryResponse response, LotteryResult result) StartLottery(Player player, BetData data, string roundId)
        {
            var before = player.Balance;    // 抽獎前餘額

            // 防呆檢查
            if (data == null || data.totalBet <= 0)
            {
                var failResponse = new LotteryResponse
                {
                    message = "下注金額錯誤",
                    balanceBefore = before,
                    balanceAfter = before,
                    totalBet = 0
                };
                return (failResponse, null);   // ⬅️ 回傳 Tuple，result 用 null
            }

            if (player.Balance < data.totalBet)
            {
                var failPesponse =  new LotteryResponse
                {
                    insufficientBalance = true,
                    message = "餘額不足",
                    balanceBefore = before,
                    balanceAfter = before,
                    totalBet = data.totalBet
                };
                return (failPesponse, null);    // 回傳 Tuple
            }

            // ✅ 單區下注上限檢查
            const int MaxBetPerArea = 100000; // 每區上限
            foreach (var kv in data.betAmounts)
            {
                if (kv.Value > MaxBetPerArea)
                {
                    var failResponse = new LotteryResponse
                    {
                        insufficientBalance = true,
                        message = $"單區超過上限 {MaxBetPerArea}",
                        balanceBefore = before,
                        balanceAfter = before,
                        totalBet = data.totalBet
                    };
                    return (failResponse, null);
                }
            }

            // ================= 進入金流關鍵路徑 ================
            // 扣除下注金額
            //var before = player.Balance;        // 抽獎前餘額
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

            // 派彩
            player.Balance += result.payout;
            long expectedAfter = before - data.totalBet + result.payout;
            if (player.Balance != expectedAfter)
                player.Balance = expectedAfter;

            LotteryLog(LotteryLogType.BalanceAfterPayout, result.payout, player.Balance);



            // 回傳完整封包
            var response = new LotteryResponse
            {
                //result = result,            // 抽獎結果資料
                roundId = roundId,
                balanceBefore = before,     // 抽獎前餘額
                balanceAfter = player.Balance, // 最終餘額
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

            // === 系統資訊 ===
            LotteryLog(LotteryLogType.OtherInfo,
                "OK",  // args[0] 目前沒用，可以放 "OK"
                RTPManager.GetCurrentRTP(),
                RTPManager.totalBets,
                RTPManager.totalPayouts,
                response.balanceAfter,      // 當前玩家餘額 or 機台餘額，看你要印哪個
                SuperJackpotPool.PoolBalance);

            return (response, result);      // 正確回傳Tuple
        }
        #endregion

        #region 單區下注(即時下注更新)
        public static (long balance, Dictionary<string, int> betAmounts, string message) PlaceBet(Player player, string areaName, int amount)
        {
            const int MaxBetPerArea = 100000; // 每區下注上限
                
            if (!player.CurrentRoundBets.ContainsKey(areaName))
                player.CurrentRoundBets[areaName] = 0;

            long newTotal = player.CurrentRoundBets[areaName] + amount;

            if (newTotal > MaxBetPerArea)
            {
                return (player.Balance, player.CurrentRoundBets, $"超過單區上限 {MaxBetPerArea}"); 
            }    

            if (player.Balance >= amount)
            {
                player.Balance -= amount;

                if (!player.CurrentRoundBets.ContainsKey(areaName))
                    player.CurrentRoundBets[areaName] = 0;

                player.CurrentRoundBets[areaName] += amount;

                //return new { balance = player.Balance, betAmounts = player.CurrentRoundBets };
                return (player.Balance, player.CurrentRoundBets, null); // 成功, message = null
            }
            else
            {
                //return new { balance = player.Balance, betAmounts = player.CurrentRoundBets, message = "餘額不足" };
                return (player.Balance, player.CurrentRoundBets, "餘額不足");   
            }
        }
        #endregion
    }
}
