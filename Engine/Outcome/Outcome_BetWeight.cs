using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSPFrom.Models;      // BetData
using YSPFrom.Core.RTP;
using System.Security.Cryptography.X509Certificates;    // WheelModel

namespace YSPFrom.Engine.Outcome
{
    public class Outcome_BetWeight
    {
        const float BET_HIT_BONUS = 1.5f;   // 有下注該區時, 權重調降原本的0.8倍
        const float NO_BET_FACTOR = 0.8f;   // 無下注時,權重提升原本1.2倍

        public static float GetFactor(string rewardName, BetData data)
        {
            // 若下注資料為 null，預設視為「無下注」→ 採用 NO_BET_FACTOR
            if (data?.betAmounts == null) return NO_BET_FACTOR;

            // 有下注該獎項 → 使用 BET_HIT_BONUS；無下注 → 使用 NO_BET_FACTOR
            return (data.betAmounts.TryGetValue(rewardName, out int amt) && amt > 0)
                ? BET_HIT_BONUS : NO_BET_FACTOR;
        }

        public static HashSet<string> GetNoBetJackpots(BetData data)
        {
            var noBetJackpots = new HashSet<string>();

            foreach (var c in WheelModel.Cells.Where(x => x.IsJackpot))
            {
                if (data?.betAmounts == null ||
                    !data.betAmounts.TryGetValue(c.RewardName, out int amt) ||
                    amt <= 0)
                {
                    noBetJackpots.Add(c.RewardName);
                }
            }

            return noBetJackpots;
        }

        public static bool HasJackpotBet(BetData data)
        {
            return data.betAmounts.Any(kv =>
                WheelModel.Cells.Any(c =>
                    c.IsJackpot && c.RewardName == kv.Key && kv.Value > 0));
        }
    }
}
