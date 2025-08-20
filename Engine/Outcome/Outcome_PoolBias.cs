using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSPFrom.Engine.Outcome
{
    public static class Outcome_PoolBias
    {
        // 🎯 分池加成的上限值（避免權重偏移過大）
        private const float HIGH_CAP = 0.20f; // 高倍池最大加成
        private const float LOW_CAP = 0.15f; // 低倍池最大加成
        private const float MID_CAP = 0.10f; // 中倍池最大加成

        public static (float boostHigh, float boostMid, float boostLow) GetBiasValues(float alpha)
        {
            float boostHigh, boostLow, boostMid;
            if (alpha >= 0f)
            {
                // 正值 → 偏壓高倍池與中倍池，低倍池不加成
                boostHigh = Math.Min(HIGH_CAP, alpha);
                boostMid = Math.Min(MID_CAP, alpha * 0.5f);
                boostLow = 0f;
            }
            else
            {
                // 負值 → 偏壓低倍池與中倍池，高倍池不加成
                float a = -alpha;
                boostHigh = 0f;
                boostMid = -Math.Min(MID_CAP, a * 0.5f);
                boostLow = Math.Min(LOW_CAP, a);
            }
            return (boostHigh, boostMid, boostLow);
        }
    }
}
