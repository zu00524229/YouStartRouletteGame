using System;
using System.Linq;
using YourNamespace.Core.Utils;
using YSPFrom.Core.Logging;
using YSPFrom.Core.RTP;
using YSPFrom.Core.SuperJackpot;
using YSPFrom.Engine.ExtraPay;
using YSPFrom.Engine.Outcome;
using YSPFrom.Models;
using static YSPFrom.Core.Logging.LogManager;

namespace YSPFrom
{
    public class LotteryService
    {
        public void ShowRTP()
        {
            string msg = RTPManager.GetRTPStatusMessage();
            Console.WriteLine(msg);
        }

        // ===== 🎰 主邏輯：新版平滑控獎抽獎 =====
        public static LotteryResult CalculateLotteryResult(Player player, BetData data, bool affectBalance = true)
        {
            int balanceBefore = player?.Balance ?? 0;

            // 扣下注金額
            if (affectBalance && player != null)
                player.Balance -= data.totalBet;

            // 印下注資訊
            LogManager.LotteryLog(LotteryLogType.BetAmounts, data.betAmounts);

            //  OutcomeSelector 決定結果（會自動做 AddBet/AddPayout/EWMA）
            LotteryResult outcome = OutcomeSelector.Select(data);
            ExtraPayInfo extraPayInfo = outcome.extraPay;

            int finalMultiplier = outcome.multiplier;
            int winAmount = outcome.payout;

            // 處理 ExtraPay（若有的話）
            if (extraPayInfo != null && extraPayInfo.rewardName == outcome.rewardName)
            {
                finalMultiplier *= extraPayInfo.extraMultiplier;
                winAmount = checked((data.betAmounts.ContainsKey(outcome.rewardName) ? data.betAmounts[outcome.rewardName] : 0) * finalMultiplier);
                //RTPManager.AddPayout(winAmount);
            }

            // 當局下注提撥到超級大獎池
            double contribution = SuperJackpotPool.AddContribution(data.totalBet);
            LogManager.LotteryLog(LotteryLogType.JackpotContribution, contribution, SuperJackpotPool.PoolBalance);  // 統一Log 管理

            // 📝 加回派彩
            if (affectBalance && player != null)
            {
                player.Balance += winAmount;
                RTPManager.AddPayout(winAmount);
            }


            // 每 100 局印一次歷史統計 (1)
            if (RTPManager.lifetimeSpinCount % 1 == 0)
            {
                //RTPManager.LogLifetimeStats();
                LogManager.LotteryLog(LotteryLogType.RTPHistoryStats);  // 統一管理 Log
            }

            #region 舊版log
            //Console.WriteLine($"[大獎後重製RTP] 結束時間={DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            //Program.MainForm?.LogJackpot($"[大獎後重製RTP] 結束時間={DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            //Console.WriteLine($"[大獎後重製RTP] 結束時間={DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            //Program.MainForm?.LogJackpot($"[大獎後重製RTP] 結束時間={DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            //// RTP 當前區間統計（放 RTP 數值分頁）
            //string intervalLog = string.Format("累計下注={0}, 累計派彩={1}, 轉盤次數={2}, RTP={3:0.0000}",
            //    RTPManager.totalBets, RTPManager.totalPayouts, RTPManager.spinCount, RTPManager.GetCurrentRTP());
            //Console.WriteLine(intervalLog);
            //Program.MainForm?.LogRTP(intervalLog); // 改成 LogRTP

            //// 大獎詳細紀錄（保留在 BigResult 分頁）
            //long roundId = RoundIdGenerator.NextId();
            //string logMsg = $"局號={roundId} | 獎項={outcome.rewardName}, 倍率={finalMultiplier}, 派彩金額={winAmount}, 當時RTP={RTPManager.GetCurrentRTP():0.0000}";
            //Program.MainForm?.LogBigResult(logMsg);
            #endregion
            // === 建立 RoundContext ===
            var roundCtx = new RoundContext
            {
                RoundId = RoundIdGenerator.NextId(),
                UserId = player?.UserId ?? "SIM",  // 模擬模式給個預設值
                BetAmount = data.totalBet,
                Contribution = (int)contribution,
                Payout = winAmount,
                RewardName = outcome.rewardName,
                Multiplier = finalMultiplier,
                IsJackpot = outcome.isJackpot,
                ExtraPay = outcome.extraPay,
                BalanceBefore = balanceBefore,
                BalanceAfter = player.Balance,
                PoolBalance = SuperJackpotPool.PoolBalance,
                CurrentRTP = RTPManager.GetCurrentRTP()
            };

            LogManager.LotteryLog(
                 LotteryLogType.RoundWinSummary, roundCtx);  // 統一管理log

            // === 命中大獎 ===
            if ((outcome.rewardName == "PRIZE_PICK" ||
                 outcome.rewardName == "GOLD_MANIA" ||
                 outcome.rewardName == "GOLDEN_TREASURE") &&
                data.betAmounts.ContainsKey(outcome.rewardName) &&
                data.betAmounts[outcome.rewardName] > 0)
            {
                LogManager.LotteryLog(
                    LotteryLogType.Jackpot,
                    outcome.rewardName,
                    finalMultiplier,
                    winAmount,
                    RTPManager.GetCurrentRTP());
            }

            // 若中大獎類 → 儲存紀錄並延遲重置
            if (outcome.isJackpot)
            {
                // 只有 GOLDEN_TREASURE 且有下注才扣超大獎池
                if (outcome.rewardName == "GOLDEN_TREASURE" &&
                    data.betAmounts.TryGetValue(outcome.rewardName, out int betAmt) &&
                    betAmt > 0)
                {
                    SuperJackpotPool.Deduct(winAmount);
                }

                RTPManager.MarkForReset();
            }

            // 列印中獎結果
            if (winAmount > 0)
            {
                LogManager.LotteryLog(LotteryLogType.WinResult, outcome.rewardName, finalMultiplier, winAmount);
            }

            // 顯示當前 RTP 狀態
            LogManager.LotteryLog(LotteryLogType.RTPStatus);    // 統一管理 Log

            // 回傳給前端
            return new LotteryResult
            {
                rewardName = outcome.rewardName,            // 中獎名稱（前端 UI 文字 / 動畫判斷用）
                rewardIndex = outcome.rewardIndex,          // 中獎落點 index（轉盤停在哪格）
                multiplier = winAmount > 0 ? finalMultiplier : 0, // 中獎倍率（不中獎則為 0，前端顯示用）
                payout = winAmount,                         // 實際派彩金額（前端直接顯示）
                isJackpot = outcome.isJackpot,              // 是否為大獎（前端觸發特殊動畫 / 音效）
                extraPay = extraPayInfo                     // 加倍資訊（觸發加倍時才有，否則為 null）

            };
        }
    }
}
