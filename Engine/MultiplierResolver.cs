using System;
using System.Collections.Generic;
using System.Linq;
using YSPFrom.Core.RTP;     // 引入 RTPManager 安全公式

namespace YSPFrom.Engine
{
    // ======================== 🎯 處理「大獎類型」的倍率隨機邏輯 ========================
    public static class MultiplierResolver
    {
        private static readonly Random rng = new Random();

        // 大獎類型權重表
        private static readonly Dictionary<string, Dictionary<int, float>> jackpotWeights =
            new Dictionary<string, Dictionary<int, float>>
        {
            { "PRIZE_PICK", new Dictionary<int, float> {
                { 15, 0.5f }, { 60, 0.3f }, { 73, 0.1f }, { 91, 0.07f }, { 100, 0.03f }
            }},
            { "GOLD_MANIA", new Dictionary<int, float> {
                { 25, 0.5f }, { 78, 0.25f }, { 135, 0.15f }, { 300, 0.08f }, { 500, 0.02f }
            }},
            { "GOLDEN_TREASURE", new Dictionary<int, float> {
                { 100, 50f }, { 800, 30f }, { 1700, 15f }, { 3000, 5f }
            }}
        };

        // 大獎倍率決定（含安全機制過濾）
        public static int Resolve(string rewardName, int min, int max, int betOnReward = 0)
        {
            // 有定義權重表 → 用權重抽
            if (jackpotWeights.ContainsKey(rewardName))
            {
                var weightMap = jackpotWeights[rewardName];

                if (betOnReward > 0)
                {
                    double net = RTPManager.GetNetProfit();
                    Console.WriteLine($"[倍率檢查] {rewardName} | 淨利={net:0} | 下注額={betOnReward}");
                    // 過濾掉超過安全機制允許的倍率
                    var safeWrights = weightMap.Where(kv =>
                    {
                        double worstPayout = betOnReward * kv.Key * 1.25;   // 安全係數
                        return worstPayout <= net;

                    })
                    .ToDictionary(kv => kv.Key, kv => kv.Value);

                    if (safeWrights.Count > 0) 
                        return GetWeighted(safeWrights);

                    Console.WriteLine("[倍率檢查] 全部倍率超過安全上限 → 回退最小倍率");
                    
                    return weightMap.Keys.Min();    // 如果全被剔除, 回退最小倍率
                }
                return GetWeighted(weightMap);
            }

            // 沒定義 → 用對數分佈抽（一般獎）
            return SampleLog(min, max);
        }

        // 權重抽取 /// 🎯 私有方法：給定一組倍率權重，回傳一個隨機倍率（依機率分布）
        private static int GetWeighted(Dictionary<int, float> weightMap)
        {
            float totalWeight = weightMap.Values.Sum();
            float roll = (float)rng.NextDouble() * totalWeight;
            float cumulative = 0f;

            foreach (var kv in weightMap)
            {
                cumulative += kv.Value;
                if (roll <= cumulative)
                    return kv.Key;
            }
            
            return weightMap.Keys.Min(); // ⚠️ 極小機率：若沒選中任何項目，就選最小的倍率
        }

        // 對數分佈抽樣
        private static int SampleLog(int min, int max)
        {
            if (min >= max) return min;

            double mi = Math.Max(1.0, min);
            double ma = Math.Max(mi + 1e-6, max);
            double u = rng.NextDouble();
            double x = mi * Math.Pow(ma / mi, u);
            int k = (int)Math.Round(x);

            if (k < min) k = min;
            if (k > max) k = max;
            return k;
        }
    }
}
