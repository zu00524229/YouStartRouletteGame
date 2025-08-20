using YSPFrom.Models;

namespace YSPFrom.Engine
{
    public static class RewardSelector
    {
        /// 由 OutcomeSelector 決定何時呼叫；若未觸發就回 null
        public static ExtraPayInfo TryTriggerExtraPay(BetData data, float currentRtp, float targetRtp)
        {
            // 建議條件：RTP 低於目標且本局有下注才嘗試
            if (data == null || data.totalBet <= 0) return null;
            if (currentRtp >= (targetRtp <= 0 ? 1.0f : targetRtp)) return null;

            return ExtraPay.ExtraPayManager.TryTriggerExtraPay(data);
        }

        /// 回傳「額外權重 delta」；OutcomeSelector 用 baseWeight 算完後再 +delta
        /// eff2xFloor 由 OutcomeSelector 依自己偏壓/缺口邏輯算出來
        public static float GetExtraWeightDelta(
            string rewardKey,
            ExtraPayInfo extraPayInfo,
            float baseWeight,
            float eff2xFloor)
        {
            if (extraPayInfo == null) return 0f;

            switch (rewardKey)
            {
                case "10X":
                case "6X":
                case "4X":
                    // 針對 4~10X：用 ExtraPayManager 既有邏輯計算加成
                    return ExtraPay.ExtraPayManager.GetExtraPayWeight(
                        rewardKey, extraPayInfo, baseWeight, eff2xFloor);

                case "2X":
                    return ExtraPay.ExtraPayManager.GetExtraPayWeight(
                        rewardKey, extraPayInfo, baseWeight, baseWeight);

                default:
                    // 三大獎與其他獎項不加成（交由 OutcomeSelector 的 gate/偏壓處理）
                    return 0f;
            }
        }
    }
}
