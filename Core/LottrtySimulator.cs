using System;
using System.Collections.Generic;
using System.Linq;
using YourNamespace.Core.Utils;
using YSPFrom.Core.RTP;
using YSPFrom.Core.SuperJackpot;
using YSPFrom.Engine.Outcome;
using YSPFrom.Models;
using static YSPFrom.Core.Logging.LogManager;

namespace YSPFrom
{
    public static class LotterySimulator
    {
        /// <summary>
        /// 模擬多次抽獎並統計結果（已包含 ExtraPay 的派彩與次數統計）
        /// </summary>
        public static SimulationResult RunSimulation(BetData data, int simulateCount)
        {
            int totalBet = 0;
            int totalPayout = 0;

            var hitCounts = new Dictionary<string, int>();
            var extraPayHits = new Dictionary<string, int>(); // ExtraPay 統計

            for (int i = 0; i < simulateCount; i++)
            {
                var result = LotteryService.CalculateLotteryResult(data);

                // 下注金額：如果 totalBet 是唯讀計算屬性，就由 betAmounts 自行算；否則照你現在的寫法
                totalBet += data.totalBet;

                // 派彩（CalculateLotteryResult 內已把 ExtraPay 加到 payout）
                totalPayout += result.payout;

                // 命中次數
                if (!hitCounts.ContainsKey(result.rewardName))
                    hitCounts[result.rewardName] = 0;
                hitCounts[result.rewardName]++;

                // ExtraPay 次數
                if (result.extraPay != null)
                {
                    var key = result.extraPay.rewardName;
                    if (!extraPayHits.ContainsKey(key))
                        extraPayHits[key] = 0;
                    extraPayHits[key]++;
                }
            }

            float rtp = totalBet > 0 ? (float)totalPayout / totalBet : 0f;

            return new SimulationResult
            {
                TotalBets = totalBet,
                TotalPayouts = totalPayout,
                RTP = rtp,
                HitCounts = hitCounts,
                ExtraPayHits = extraPayHits
            };
        }

        /// <summary>
        /// 後端快速測試模式（寫死下注資料）；可傳入 logger 委派把結果丟到 UI。
        /// </summary>
        #region 固定下注法
        //    public static void RunTestMode(Action<string> logger = null, int times = 1000)
        //    {
        //        if (logger == null) logger = Console.WriteLine;

        //        // 固定下注組合
        //        var betDataTemplate = new Dictionary<string, int>
        //        {
        //            { "2X", 100 },
        //            { "4X", 100 },
        //            { "6X", 200 },
        //            { "10X", 200 },
        //            { "PRIZE_PICK", 200 },
        //            { "GOLD_MANIA", 300 },
        //            { "GOLDEN_TREASURE", 500 }
        //        };

        //        // 累計統計
        //        int totalBets = 0;
        //        int totalPayouts = 0;
        //        var hitCounts = new Dictionary<string, int>();
        //        var extraPayHits = new Dictionary<string, int>();

        //        // 🆕 大獎命中統計（需有下注才計數）
        //        var jackpotHitCounts = new Dictionary<string, int>
        //        {
        //            { "PRIZE_PICK", 0 },
        //            { "GOLD_MANIA", 0 },
        //            { "GOLDEN_TREASURE", 0 }
        //        };

        //        int balance = 5_000_000; // 初始餘額
        //        long _currentRoundId = 0;
        //        int actualRounds = 0;    // 🆕 實際跑了幾局

        //        for (int i = 1; i <= times; i++)
        //        {
        //            var betData = new BetData { betAmounts = new Dictionary<string, int>(betDataTemplate) };
        //            int betTotal = betData.totalBet;

        //            // 🛑 餘額不足 → 停止模擬
        //            if (balance < betTotal)
        //            {
        //                logger($"⚠️ [第 {i} 局] 餘額不足，停止模擬。當前餘額={balance}, 需要={betTotal}");
        //                break;
        //            }

        //            actualRounds++; // 記錄成功跑過的局數

        //            // === BalanceBeforeBet ===
        //            long roundId = RoundIdGenerator.NextId();
        //            _currentRoundId = roundId;
        //            LotteryLog(LotteryLogType.BalanceBeforeBet, balance, betTotal);

        //            // === BalanceAfterBet ===
        //            balance -= betTotal;
        //            LotteryLog(LotteryLogType.BalanceAfterBet, balance);

        //            // === 投注明細 ===
        //            LotteryLog(LotteryLogType.BetAmounts, betData.betAmounts);

        //            // === 獎池提撥 ===
        //            SuperJackpotPool.AddContribution(betTotal);
        //            LotteryLog(LotteryLogType.JackpotContribution, betTotal * 0.05, SuperJackpotPool.PoolBalance);

        //            // === 抽獎 ===
        //            var result = OutcomeSelector.Select(betData);

        //            // === BalanceAfterPayout ===
        //            balance += result.payout;
        //            LotteryLog(LotteryLogType.BalanceAfterPayout, result.payout, balance);

        //            // === 中獎結果 ===
        //            LotteryLog(LotteryLogType.WinResult, result.rewardName, result.multiplier, result.payout);

        //            // === RoundSummary（下半部左）===
        //            LotteryLog(LotteryLogType.RoundSummary,
        //                result.rewardName,
        //                betTotal,
        //                result.multiplier,
        //                result.payout);

        //            // === OtherInfo（下半部右）===
        //            LotteryLog(LotteryLogType.OtherInfo, i, RTPManager.GetCurrentRTP(), totalBets, totalPayouts, balance, SuperJackpotPool.PoolBalance);

        //            // === 命中大獎 ===
        //            if ((result.rewardName == "PRIZE_PICK" ||
        //                 result.rewardName == "GOLD_MANIA" ||
        //                 result.rewardName == "GOLDEN_TREASURE") &&
        //                betData.betAmounts.ContainsKey(result.rewardName) &&
        //                betData.betAmounts[result.rewardName] > 0)
        //            {
        //                jackpotHitCounts[result.rewardName]++;
        //                LotteryLog(LotteryLogType.Jackpot, result.rewardName, result.multiplier, result.payout, RTPManager.GetCurrentRTP());
        //            }

        //            // === ExtraPay ===
        //            if (result.extraPay != null)
        //            {
        //                LotteryLog(LotteryLogType.ExtraPayTriggered,
        //                    result.extraPay.rewardName,
        //                    betData.betAmounts[result.extraPay.rewardName],
        //                    result.extraPay.extraMultiplier);

        //                string key = result.extraPay.rewardName;
        //                if (!extraPayHits.ContainsKey(key)) extraPayHits[key] = 0;
        //                extraPayHits[key]++;
        //            }

        //            totalBets += betTotal;
        //            totalPayouts += result.payout;

        //            // 中獎統計
        //            if (!hitCounts.ContainsKey(result.rewardName)) hitCounts[result.rewardName] = 0;
        //            hitCounts[result.rewardName]++;


        //            // 每 1000 局輸出一次
        //            if (i % 1000 == 0)
        //            {
        //                double rtpNow = totalBets > 0 ? (double)totalPayouts / totalBets : 0;
        //                logger($"[第 {i} 局統計] 總下注={totalBets}, 總派彩={totalPayouts}, RTP={rtpNow:P2}");

        //                foreach (var kv in hitCounts)
        //                {
        //                    double rate = (double)kv.Value / i * 100;
        //                    logger($"{kv.Key}: {kv.Value} 次 ({rate:F2}%)");
        //                }

        //                // 🆕 大獎命中統計（有下注才算）
        //                logger("=== 大獎命中統計（有下注才算） ===");
        //                foreach (var jackpotName in jackpotHitCounts.Keys)
        //                {
        //                    double hitRate = (double)jackpotHitCounts[jackpotName] / i * 100;
        //                    logger($"{jackpotName}: {jackpotHitCounts[jackpotName]} 次 ({hitRate:F2}%)");
        //                }

        //                logger("=== ExtraPay 命中統計 ===");
        //                if (extraPayHits.Count == 0) logger("（目前未觸發）");
        //                foreach (var kv in extraPayHits)
        //                    logger($"{kv.Key}: {kv.Value} 次");

        //                logger("--------------------------");
        //            }
        //        }

        //        // === 最終統計 ===
        //        double rtpFinal = totalBets > 0 ? (double)totalPayouts / totalBets : 0;
        //        logger($"[最終統計] 實際跑了 {actualRounds} 局 (計畫 {times} 局)，總下注={totalBets}, 總派彩={totalPayouts}, RTP={rtpFinal:P2}");

        //        foreach (var kv in hitCounts)
        //        {
        //            double rate = (double)kv.Value / (actualRounds == 0 ? 1 : actualRounds) * 100;
        //            logger($"{kv.Key}: {kv.Value} 次 ({rate:F2}%)");
        //        }

        //        logger("=== 大獎命中統計（有下注才算） ===");
        //        foreach (var jackpotName in jackpotHitCounts.Keys)
        //        {
        //            double hitRate = (double)jackpotHitCounts[jackpotName] / (actualRounds == 0 ? 1 : actualRounds) * 100;
        //            logger($"{jackpotName}: {jackpotHitCounts[jackpotName]} 次 ({hitRate:F2}%)");
        //        }

        //        logger("=== ExtraPay 命中統計 ===");
        //        if (extraPayHits.Count == 0) logger("（本次未觸發）");
        //        foreach (var kv in extraPayHits)
        //            logger($"{kv.Key}: {kv.Value} 次");
        //    }
        //}
        #endregion










        #region 隨機4區隨機下注(RTP 會相對低)
        //隨機下注四區狀框(RTP 會相對低)
        public static void RunTestMode(Action<string> logger = null, int times = 1000)
        {
            var allAreas = new List<string>
            {
                "2X", "4X", "6X", "10X",
                "PRIZE_PICK", "GOLD_MANIA", "GOLDEN_TREASURE"
            };

            var betAmountsPool = new[] { 100, 200, 500, 1000, 10000 }; // 金額隨機池
            var rnd = new Random();

            if (logger == null) logger = Console.WriteLine;

            //累計統計
                    int totalBets = 0;
            int totalPayouts = 0;
            var hitCounts = new Dictionary<string, int>();
            var extraPayHits = new Dictionary<string, int>();

            // 🆕 大獎命中統計（需有下注才計數）
            var jackpotHitCounts = new Dictionary<string, int>
                    {
                        { "PRIZE_PICK", 0 },
                        { "GOLD_MANIA", 0 },
                        { "GOLDEN_TREASURE", 0 }
                    };

            int balance = 5_000_000; // 初始餘額
            long _currentRoundId = 0;
            int actualRounds = 0;    // 🆕 實際跑了幾局

            for (int i = 1; i <= times; i++)
            {
                // === 生成隨機下注資料 ===
                var betAmounts = new Dictionary<string, int>();
                foreach (var area in allAreas)
                {
                    // 隨機決定要不要下注 (50% 機率)
                    if (rnd.NextDouble() < 0.5)
                    {
                        int bet = betAmountsPool[rnd.Next(betAmountsPool.Length)];
                        betAmounts[area] = bet;
                    }
                }

                // 至少要有一個下注，避免 totalBet=0
                if (betAmounts.Count == 0)
                {
                    var area = allAreas[rnd.Next(allAreas.Count)];
                    int bet = betAmountsPool[rnd.Next(betAmountsPool.Length)];
                    betAmounts[area] = bet;
                }

                var betData = new BetData { betAmounts = betAmounts };
                int betTotal = betData.totalBet;

                // 🛑 餘額不足 → 停止模擬
                if (balance < betTotal)
                {
                    logger($"⚠️ [第 {i} 局] 餘額不足，停止模擬。當前餘額={balance}, 需要={betTotal}");
                    break;
                }

                actualRounds++; // 記錄成功跑過的局數

                // === BalanceBeforeBet ===
                long roundId = RoundIdGenerator.NextId();
                _currentRoundId = roundId;
                LotteryLog(LotteryLogType.BalanceBeforeBet, balance, betTotal);

                // === BalanceAfterBet ===
                balance -= betTotal;
                LotteryLog(LotteryLogType.BalanceAfterBet, balance);

                // === 投注明細 ===
                LotteryLog(LotteryLogType.BetAmounts, betData.betAmounts);

                // === 獎池提撥 ===
                SuperJackpotPool.AddContribution(betTotal);
                LotteryLog(LotteryLogType.JackpotContribution, betTotal * 0.05, SuperJackpotPool.PoolBalance);

                // === 抽獎 ===
                var result = OutcomeSelector.Select(betData);

                // === BalanceAfterPayout ===
                balance += result.payout;
                LotteryLog(LotteryLogType.BalanceAfterPayout, result.payout, balance);

                // === 中獎結果 ===
                LotteryLog(LotteryLogType.WinResult, result.rewardName, result.multiplier, result.payout);

                // === RoundSummary（下半部左）===
                LotteryLog(LotteryLogType.RoundSummary,
                    result.rewardName,
                    betTotal,
                    result.multiplier,
                    result.payout);

                // === OtherInfo（下半部右）===
                LotteryLog(LotteryLogType.OtherInfo, i, RTPManager.GetCurrentRTP(), totalBets, totalPayouts, balance, SuperJackpotPool.PoolBalance);

                // === 命中大獎 ===
                if ((result.rewardName == "PRIZE_PICK" ||
                     result.rewardName == "GOLD_MANIA" ||
                     result.rewardName == "GOLDEN_TREASURE") &&
                    betData.betAmounts.ContainsKey(result.rewardName) &&
                    betData.betAmounts[result.rewardName] > 0)
                {
                    jackpotHitCounts[result.rewardName]++;
                    LotteryLog(LotteryLogType.Jackpot, result.rewardName, result.multiplier, result.payout, RTPManager.GetCurrentRTP());
                }

                // === ExtraPay ===
                if (result.extraPay != null)
                {
                    LotteryLog(LotteryLogType.ExtraPayTriggered,
                        result.extraPay.rewardName,
                        betData.betAmounts[result.extraPay.rewardName],
                        result.extraPay.extraMultiplier);

                    string key = result.extraPay.rewardName;
                    if (!extraPayHits.ContainsKey(key)) extraPayHits[key] = 0;
                    extraPayHits[key]++;
                }

                totalBets += betTotal;
                totalPayouts += result.payout;

                // 中獎統計
                if (!hitCounts.ContainsKey(result.rewardName)) hitCounts[result.rewardName] = 0;
                hitCounts[result.rewardName]++;


                // 每 1000 局輸出一次
                if (i % 1000 == 0)
                {
                    double rtpNow = totalBets > 0 ? (double)totalPayouts / totalBets : 0;
                    logger($"[第 {i} 局統計] 總下注={totalBets}, 總派彩={totalPayouts}, RTP={rtpNow:P2}");

                    foreach (var kv in hitCounts)
                    {
                        double rate = (double)kv.Value / i * 100;
                        logger($"{kv.Key}: {kv.Value} 次 ({rate:F2}%)");
                    }

                    // 🆕 大獎命中統計（有下注才算）
                    logger("=== 大獎命中統計（有下注才算） ===");
                    foreach (var jackpotName in jackpotHitCounts.Keys)
                    {
                        double hitRate = (double)jackpotHitCounts[jackpotName] / i * 100;
                        logger($"{jackpotName}: {jackpotHitCounts[jackpotName]} 次 ({hitRate:F2}%)");
                    }

                    logger("=== ExtraPay 命中統計 ===");
                    if (extraPayHits.Count == 0) logger("（目前未觸發）");
                    foreach (var kv in extraPayHits)
                        logger($"{kv.Key}: {kv.Value} 次");

                    logger("--------------------------");
                }
            }

            // === 最終統計 ===
            double rtpFinal = totalBets > 0 ? (double)totalPayouts / totalBets : 0;
            logger($"[最終統計] 實際跑了 {actualRounds} 局 (計畫 {times} 局)，總下注={totalBets}, 總派彩={totalPayouts}, RTP={rtpFinal:P2}");

            foreach (var kv in hitCounts)
            {
                double rate = (double)kv.Value / (actualRounds == 0 ? 1 : actualRounds) * 100;
                logger($"{kv.Key}: {kv.Value} 次 ({rate:F2}%)");
            }

            logger("=== 大獎命中統計（有下注才算） ===");
            foreach (var jackpotName in jackpotHitCounts.Keys)
            {
                double hitRate = (double)jackpotHitCounts[jackpotName] / (actualRounds == 0 ? 1 : actualRounds) * 100;
                logger($"{jackpotName}: {jackpotHitCounts[jackpotName]} 次 ({hitRate:F2}%)");
            }

            logger("=== ExtraPay 命中統計 ===");
            if (extraPayHits.Count == 0) logger("（本次未觸發）");
            foreach (var kv in extraPayHits)
                logger($"{kv.Key}: {kv.Value} 次");
        }
    }
        #endregion




    public class SimulationResult
    {
        public int TotalBets { get; set; }
        public int TotalPayouts { get; set; }
        public float RTP { get; set; }
        public Dictionary<string, int> HitCounts { get; set; }
        public Dictionary<string, int> ExtraPayHits { get; set; }
    }

}
