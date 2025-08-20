using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSPFrom.Core.Logging;
using YSPFrom.Core.RTP;

namespace YSPFrom.Engine.Outcome
{
    public static class Outcome_JackpotLimiter
    {
        public static void ApplyLimit(
            List<Tuple<WheelCell, double>> weighted,
            bool hasJackpotBet)
        {
            double jackpotCap = hasJackpotBet ? 0.02 : 0.05;      // 差0.005 RTP 就差了7% 我的天(目前0.02，五萬局是 RTP 90%上下)

            double jackSum = weighted.Where(w => w.Item1.IsJackpot).Sum(w => w.Item2);      // 所有大獎格字的加權總合
            double nonJackSum = weighted.Where(w => !w.Item1.IsJackpot).Sum(w => w.Item2);  // 所有非大獎格子加權總合
            //Console.WriteLine($"[封頂前] jackSum={jackSum}, nonJackSum={nonJackSum}, jackpotCap={jackpotCap}");
            LogManager.LotteryLog(LogManager.LotteryLogType.JackpotLimiterBefore, jackSum, jackpotCap, nonJackSum);     // 統一管理Log

            // 只有在大獎與非大獎權重都大於 0 時才作封頂
            if (jackSum > 0d && nonJackSum > 0d)
            {
                double targetJackSum = (jackpotCap / (1.0 - jackpotCap)) * nonJackSum;
                if (jackSum > targetJackSum)
                {
                    double scale = targetJackSum / jackSum; // 計算縮放比例

                    // 遍歷 weighted，僅縮放大獎項目的權重
                    for (int i = 0; i < weighted.Count; i++)
                    {
                        if (!weighted[i].Item1.IsJackpot) continue;

                        // 按比例縮放
                        double w = weighted[i].Item2 * scale;

                        // 防止權重被縮到 0（避免抽不到的情況）
                        if (w < 0.0001) w = 0.0001;

                        weighted[i] = Tuple.Create(weighted[i].Item1, w);
                    }
                }
            }
        }
    }
}
