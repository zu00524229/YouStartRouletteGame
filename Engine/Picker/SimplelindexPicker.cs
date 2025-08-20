using System;
using YSPFrom.Configs;

namespace YSPFrom.Helpers.Utilities
{
    public class SimpleIndexPicker : IIndexPicker
    {
        private readonly Random _random = new Random();

        public int PickIndex(string rewardName, int finalMultiplier)
        {
            if (!RewardTable.Table.TryGetValue(rewardName, out var info) ||
                info.indices == null || info.indices.Count == 0)
                return 0;

            return info.indices[_random.Next(info.indices.Count)];
        }
    }
}
