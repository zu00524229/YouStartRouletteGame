using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using YSPFrom.Core.RTP; // 這裡用來取 WheelModel, WheelCell, MultiplierSampler
using YSPFrom.Engine.ExtraPay;
using YSPFrom.Core.Logging;
using static YSPFrom.Core.Logging.LogManager;
using YSPFrom.Models; // 如果 SpinOutcome, BetData 在 Models
 //如果 BetData 還在 Core，就改成 using YSPFrom;

namespace YSPFrom.Engine.Outcome
{
    /// 控獎抽樣器：負責根據 RTP 與下注資料決定落點與倍率
    public static class OutcomeSelector
    {
        private static BoostRegulator BoostCtl = new BoostRegulator(50f);
        private static readonly Random rng = new Random();

        // 主要接口：傳入下注，回傳一次結果
        public static LotteryResult Select(BetData data)
        {
            #region ) 開局：增加局數、檢查重置
            RTPManager.IncrementSpinCount();

            if (data != null && data.totalBet > 0)
            {
                LogManager.LotteryLog(LotteryLogType.NetProfitCheck, RTPManager.GetNetProfit());    // 統一Log 管理
            }
            #endregion

            #region ) 三段池 + 偏壓（RTP 控制）
            List<WheelCell> lows, mids, highs;
            WheelModel.SplitPools(out lows, out mids, out highs);
            float alpha = RTPManager.GetBiasAlpha(); // -0.20 ~ +0.20
            #endregion

            #region ) 大獎 Gate：依淨利儲備判斷是否允許進入抽樣池
            Outcome_JackpotGate.Apply(ref lows, data);
            Outcome_JackpotGate.Apply(ref mids, data);
            Outcome_JackpotGate.Apply(ref highs, data);
            #endregion

            #region ) 嘗試觸發 ExtraPay（只要一次，全域共用）
            var targetRtp = RTPManager.targetRTP;
            var currentRtp = RTPManager.GetCurrentRTP();
            var extraPayInfo = RewardSelector.TryTriggerExtraPay(data, currentRtp, targetRtp);
            //Console.WriteLine("[Debug] ExtraPay 檢查開始");
            //Console.WriteLine($"當前 RTP: {currentRtp}, 目標 RTP: {targetRtp}");
            LogManager.LotteryLog(LotteryLogType.ExtraPayCheck, currentRtp, targetRtp);     // 統一Log 管理

            #endregion

            #region ) 分池偏壓上限
            var (boostHigh, boostMid, boostLow) = Outcome_PoolBias.GetBiasValues(alpha);
            #endregion

            #region ) 下注關聯：有押加權 / 無押折扣
            bool hasJackpotBet = Outcome_BetWeight.HasJackpotBet(data);
            float NO_BET_JACKPOT_BOOST = BoostCtl.GetNoBetJackpotBoost(hasJackpotBet);
            HashSet<string> noBetJackpots = Outcome_BetWeight.GetNoBetJackpots(data);
            #endregion

            #region ) 計算每格最終權重 (不套 BOOST)
            var weighted = new List<Tuple<WheelCell, double>>();
          
            Outtcome_WeightCalculator.AddRange(weighted, lows, boostLow, data, extraPayInfo);
            Outtcome_WeightCalculator.AddRange(weighted, mids, boostMid, data, extraPayInfo);
            Outtcome_WeightCalculator.AddRange(weighted, highs, boostHigh, data, extraPayInfo);

            Console.WriteLine(
                 $"[偏壓調整] α(控獎強度)={alpha:0.000}, " +
                 $"低倍加成={boostLow:0.000}, " +
                 $"中倍加成={boostMid:0.000}, " +
                 $"高倍加成={boostHigh:0.000}"
             );
            //Program.MainForm?.LogRTP(
            //    $"[偏壓調整] α(控獎強度)={alpha:0.000}, " +
            //    $"低倍加成={boostLow:0.000}, " +
            //    $"中倍加成={boostMid:0.000}, " +
            //    $"高倍加成={boostHigh:0.000}"
            //);
            LogManager.LotteryLog(LotteryLogType.PoolBiasAdjustment, alpha, boostLow, boostMid, boostHigh); // 統一Log 管理


            #endregion

            #region ) 大獎占比封頂 （封頂時不考慮 BOOST） // 差0.005 RTP 就差了7% 我的天(目前0.02，五萬局是 RTP 90%上下)
            Outcome_JackpotLimiter.ApplyLimit(weighted, hasJackpotBet);
            #endregion

            #region ) 封頂後套用 BOOST（只套在無下注大獎）
            for (int i = 0; i < weighted.Count; i++)
            {
                if (noBetJackpots.Contains(weighted[i].Item1.RewardName))
                {
                    weighted[i] = Tuple.Create(
                        weighted[i].Item1,
                        weighted[i].Item2 * NO_BET_JACKPOT_BOOST
                    );
                }
            }
            #endregion

            #region ) 安全防護：全為 0 時回退為等權
            if (weighted.Count == 0)
            {
                for (int i = 0; i < WheelModel.Cells.Count; i++)
                {
                    WheelCell c = WheelModel.Cells[i];
                    weighted.Add(new Tuple<WheelCell, double>(c, Math.Max(0.0001, (double)c.BaseWeight)));
                }
            }
            #endregion

            #region ) 依權重抽一格（決定落點 index）
            double totalW = 0d;
            for (int i = 0; i < weighted.Count; i++) totalW += weighted[i].Item2;

            double r = rng.NextDouble() * totalW;
            double acc = 0d;
            WheelCell hit = weighted[0].Item1;
            for (int i = 0; i < weighted.Count; i++)
            {
                acc += weighted[i].Item2;
                if (r <= acc) { hit = weighted[i].Item1; break; }
            }
            #endregion

            #region ) 決定倍率（大獎）
            int betOnHit = 0;
            data?.betAmounts?.TryGetValue(hit.RewardName, out betOnHit);
            int multiplier = MultiplierResolver.Resolve(hit.RewardName, hit.Min, hit.Max, betOnHit);
            #endregion

            #region ) 計算派彩
            int payout = betOnHit * multiplier;
            //Console.WriteLine($"[DEBUG] hit={hit.RewardName}, betOnHit={betOnHit}, multiplier={multiplier}, payout={payout}");
            LogManager.LotteryLog(LotteryLogType.DebugHitResult, hit.RewardName, betOnHit, multiplier, payout);

            #endregion

            #region ) 寫帳 & 更新滾動 RTP
            int roundBet = data?.totalBet ?? 0;
            RTPManager.AddBet(roundBet);
            RTPManager.AddPayout(payout);
            RTPManager.UpdateRollingRTP(roundBet, payout);
            #endregion

            // 更新 BoostRegulator 狀態
            BoostCtl.OnSpinEnd(
                data.totalBet,
                payout,
                hit.RewardName == "PRIZE_PICK" ||
                hit.RewardName == "GOLD_MANIA" ||
                hit.RewardName == "GOLDEN_TREASURE"
            );

            #region ) 組裝結果
            LotteryResult so = new LotteryResult();
            so.rewardName = hit.RewardName;
            so.rewardIndex = hit.Index;
            so.multiplier = multiplier;
            so.payout = payout;
            so.isJackpot = hit.IsJackpot;
            so.extraPay = extraPayInfo;
            return so;
            #endregion


        }

        // ============ 理論 RTP（無控情境）供表校準 ============
        public static double ComputeTheoreticalRTP(Dictionary<string, int> unitBet)
        {
            if (unitBet == null || unitBet.Count == 0) return 0d;

            double totalBase = 0d;
            for (int i = 0; i < WheelModel.Cells.Count; i++) totalBase += WheelModel.Cells[i].BaseWeight;
            if (totalBase <= 0d) return 0d;

            double ePayout = 0d;
            for (int i = 0; i < WheelModel.Cells.Count; i++)
            {
                WheelCell c = WheelModel.Cells[i];
                double p = c.BaseWeight / totalBase;
                double eMul = WheelModel.ExpectedMultiplier(c.Min, c.Max);

                int b = 0;
                unitBet.TryGetValue(c.RewardName, out b);
                ePayout += p * eMul * (double)b;
            }

            double tBet = unitBet.Values.Sum();
            return tBet <= 0d ? 0d : (ePayout / tBet);
        }
    }
}
