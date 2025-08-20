using System;
using System.Drawing;
using System.Linq;
using YSPFrom.Core.Logging;
using YSPFrom.Models;

namespace YSPFrom.Engine.ExtraPay
{
    public class ExtraPayManager
    {
        public static ExtraPayInfo TryTriggerExtraPay(BetData data)
        {
            var eligible = data.betAmounts
                .Where(kv => new[] { "2X", "4X", "6X", "10X" }.Contains(kv.Key) && kv.Value > 0)
                .Select(kv => kv.Key)
                .ToList();
            Random random = new Random();

            if (eligible.Count > 0 && random.NextDouble() < 0.35)  // 35%機率觸發 ExtarPay
            {
                string chosen = eligible[random.Next(eligible.Count)];


                // 取該區下注金額（方便回測）
                int betAmount = data.betAmounts.ContainsKey(chosen) ? data.betAmounts[chosen] : 0;

                //string triggerLog = $"[ExtraPay觸發] 區域={chosen} x2，下注={betAmount}";

                // 後台輸出
                //Console.WriteLine(triggerLog);

                // 顯示於中獎結果分頁
                //Program.MainForm?.LogResult(triggerLog);

                LogManager.LotteryLog(LogManager.LotteryLogType.ExtraPayTriggered, chosen, betAmount, 2);       // 統一管理Log

                return new ExtraPayInfo
                {
                    rewardName = chosen,
                    extraMultiplier = 2
                };
            }

            return null;
        }

        // 
        public static float GetExtraPayWeight(string rewardKey, ExtraPayInfo info, float currentWeight = 0f, float floor = 40f)
        {
            return 0f;
        }
    }
}
