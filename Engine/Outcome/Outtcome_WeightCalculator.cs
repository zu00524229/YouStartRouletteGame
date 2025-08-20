using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSPFrom.Core.RTP;
using YSPFrom.Engine.ExtraPay;
using YSPFrom.Models;

namespace YSPFrom.Engine.Outcome
{
    internal class Outtcome_WeightCalculator
    {
        public static void AddRange(
            List<Tuple<WheelCell, double>> weighted,
            List<WheelCell> list,
            float poolBoost,
            BetData data,
            ExtraPayInfo extraInfo)
        {
            foreach (var c in list)
            {
                float poolFactor = 1f + poolBoost;
                float betFactor = Outcome_BetWeight.GetFactor(c.RewardName, data);
                //float betFactor = BetFactor(c.RewardName);
                double w = Math.Max(0.0001, c.BaseWeight) * poolFactor * betFactor;

                //if (noBetJackpots.Contains(c.RewardName))
                //{
                //    w *= NO_BET_JACKPOT_BOOST;
                //}

                if (extraInfo != null)
                {
                    w += ExtraPayManager.GetExtraPayWeight(c.RewardName, extraInfo, (float)w);
                }

                weighted.Add(Tuple.Create(c, w));
            }
        }
    }

}
