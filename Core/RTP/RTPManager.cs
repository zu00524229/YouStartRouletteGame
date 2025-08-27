using System;
using System.Collections.Generic;
using System.Linq;
using YSPFrom.Configs;
using YSPFrom.Core.SuperJackpot;

namespace YSPFrom.Core.RTP
{
    // ==================== RTP 管理（含 EWMA 平滑） ====================
    internal static class RTPManager
    {
        // 當期累計（可隨大獎重置）
        public static float totalBets = 0f;
        public static float totalPayouts = 0f;
        public static int spinCount = 0;

        // 目標 RTP（控獎對齊目標）：0.95 = 95%
        public static float targetRTP = 0.95f;

        // 歷史累計（不重置）
        public static float lifetimeBets = 0f;
        public static float lifetimePayouts = 0f;
        public static int lifetimeSpinCount = 0;

        // EWMA：指數加權移動平均（滾動 RTP）
        private static bool ewmaInit = false;
        private static float ewmaRTP = 0f;
        private const float ewmaAlpha = 0.10f; // 0.05~0.15 常用，越大反應越快

        private static bool isResetPending = false; // 標記下局是否重製 ( 通常大獎會觸發重製 )

        // === 下注累計 ===
        public static void AddBet(float bet)
        {
            totalBets += bet;       // 當期累積下注
            lifetimeBets += bet;    // 歷史累積下注
        }
        // === 派彩累計 ===
        public static void AddPayout(float payout)
        {
            totalPayouts += payout;     // 當期累積派彩
            lifetimePayouts += payout;  // 歷史累積派彩
        }

        // === 遞增轉盤次數 ===
        public static void IncrementSpinCount()
        {
            // 先檢查是否需要重置當期統計
            CheckAndResetIfPending();
            spinCount++;            // 大獎前累積次數
            lifetimeSpinCount++;    // 歷史累積次數
        }

        // === 當前 RTP (及時計算) ===
        public static float GetCurrentRTP()
        {
            return totalBets == 0f ? 0f : totalPayouts / totalBets;
        }

        // === 更新滾動RTP (EWMA 平滑處理) ===
        public static void UpdateRollingRTP(float roundBet, float roundPayout)
        {
            float roundRTP = roundBet <= 0f ? 0f : (roundPayout / roundBet);
            if (!ewmaInit)
            {
                ewmaRTP = roundRTP; // 第一次初始化
                ewmaInit = true;
            }
            else
            {
                ewmaRTP = ewmaAlpha * roundRTP + (1f - ewmaAlpha) * ewmaRTP;
            }
        }

        // === 取得滾動 RTP (優先用 EWMA 計算)
        public static float GetRollingRTP()
        {
            return ewmaInit ? ewmaRTP : GetCurrentRTP();
        }


        // 由 gap 轉成偏壓係數：正值→放水(偏高倍)，負值→收水(偏低倍)
        // === 偏壓計算 ( 控獎核心 )
        // α > 0 → 放水（高倍加權），α < 0 → 收水（低倍加權）
        public static float GetBiasAlpha(float k = 1.2f, float alphaMax = 0.20f)
        {
            // 範圍限制在 - 0.20 ~+0.20
            float gap = targetRTP - GetRollingRTP(); // 目標 - 現況
            float a = k * gap;
            if (a > alphaMax) a = alphaMax;
            //α > 0 → 放水（提高高倍命中率，RTP 拉高）
            if (a < -alphaMax) a = -alphaMax;
            //α < 0 → 收水（提高低倍命中率，RTP 降低）
            return a;
        }

        // === 淨利計算 ===
        public static float GetNetProfit()
        {
            return (lifetimeBets - lifetimePayouts) - (float)SuperJackpotPool.PoolBalance;
        }

        // === 大獎最大倍率表（最壞情況用） ===F
        private static readonly Dictionary<string, int> JackpotMaxMultiplier = new Dictionary<string, int>
        {
            { "PRIZE_PICK", 20 },        // 15/60/73/91/100 → 取最大100
            { "GOLD_MANIA", 135 },        // 25/78/135/300/500 → 取最大500
            { "GOLDEN_TREASURE", 1700 },  // 超大獎自行設定
        };

        // 用 double 做金額/賠付計算，避免 float 精度問題
        private const double RESERVE_FACTOR = 1.25; // 安全係數

        // === 計算大獎所需淨利儲備 ===
        public static double GetJackpotRequiredReserve(string rewardName, double betOnReward)
        {
            if (betOnReward <= 0) return 0;
            int maxMul = JackpotMaxMultiplier.TryGetValue(rewardName, out var m) ? m : 1000;
            // 最壞情境：下注金額 × 最大倍率 × 安全係數
            double worstPayout = betOnReward * maxMul * RESERVE_FACTOR; // 已含安全係數，直接回傳最終需要的淨利儲備
            return worstPayout;
        }

        // === 檢查淨利是否足夠開放大獎 ===
        public static bool CanOpenJackpot(string rewardName, double betOnReward, out double need, out double net)
        {
            net = GetNetProfit();
            need = GetJackpotRequiredReserve(rewardName, betOnReward);
            return net >= need;
        }




        // 標記下局須重製
        public static void MarkForReset()
        {
            isResetPending = true;
        }

        // 檢查是否需要重製
        public static void CheckAndResetIfPending()
        {
            if (isResetPending) Reset();
        }

        // 重置當期數據
        public static void Reset()
        {
            totalBets = 0f;
            totalPayouts = 0f;
            spinCount = 0;
            isResetPending = false;

            // EWMA 重置（也可改成衰減而非清零）
            ewmaInit = false;
            ewmaRTP = 0f;
        }

        // RTP 狀態訊息(除錯用)
        public static string GetRTPStatusMessage()
        {
            double bets = Convert.ToDouble(totalBets);
            double payouts = Convert.ToDouble(totalPayouts);
            double net = bets - payouts - SuperJackpotPool.PoolBalance; // 真正淨利（已扣獎池）

            return string.Format(
                "【RTP 狀態】當前RTP={0:0.0000}, 滾動RTP={1:0.0000}, 總下注={2:0}, 總派彩={3:0}, 淨利={4:0}, 大獎池={5:0}, 局數={6}",
                GetCurrentRTP(),                     // {0} → 當前 RTP
                GetRollingRTP(),                     // {1} → 滾動 RTP
                bets,                                // {2} → 總下注
                payouts,                             // {3} → 總派彩
                net,                                 // {4} → 真正淨利
                SuperJackpotPool.PoolBalance,        // {5} → 大獎池餘額
                spinCount                            // {6} → 總局數
            );
        }

        // 輸出歷史累積數據 (方便後臺分析)
        public static void LogLifetimeStats()
        {
            // 計算實際淨利（扣掉大獎池）
            double net = Convert.ToDouble(lifetimeBets) - Convert.ToDouble(lifetimePayouts) - SuperJackpotPool.PoolBalance;

            // 1) 基本統計
            string msg = string.Format(
                "累計轉盤次數={1}, 總派彩={2}, 總下注={3}, 淨利={4}, 大獎池={5}",
                DateTime.Now,
                lifetimeSpinCount,
                lifetimePayouts,
                lifetimeBets,
                net,
                SuperJackpotPool.PoolBalance
            );
            Console.WriteLine(msg);
            Program.MainForm?.LogRTPhistory(msg);

            // 2) 歷史 RTP
            float lifetimeRTP = lifetimeBets == 0f ? 0f : lifetimePayouts / lifetimeBets;
            string hisRTP = string.Format(
                "歷史RTP={1:0.0000}, 判斷是否符合收益",
                DateTime.Now,
                lifetimeRTP
            );
            Console.WriteLine(hisRTP);
            Program.MainForm?.LogRTPhistory(hisRTP);
        }

    }

}
