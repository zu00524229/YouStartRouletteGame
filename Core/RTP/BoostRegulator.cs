using System;


namespace YSPFrom.Core.RTP
{

    /// <summary>
    /// 動態 BOOST 控制器：
    /// 目標：把觀察到的RTP控制在 TARGET_RTP ± DEADBAND（總幅度約10%內），
    /// 並避免「沒押就開大獎」的可疑行為。
    /// </summary>
    public class BoostRegulator
    {
        // === 外部可調參數 ===
        public float TARGET_RTP = 0.68f;      // 你的目標RTP（例：68%）
        public float DEADBAND = 0.05f;      // ±5%（總10%）
        public float BOOST_MIN = 30f;
        public float BOOST_MAX = 90f;
        public float BOOST_MID = 50f;

        public float Kp = 180f;               // 比例增益：RTP誤差 * Kp -> 調整量
        public float Ki = 12f;                // 積分增益：累積誤差消除長期偏差

        public float MAX_STEP_PER_SPIN = 6f;  // 每轉BOOST最大變動幅度
        public float JITTER_RANGE = 2f;       // 抖動雜訊 ±2

        // 大獎後冷卻（避免連環爆）
        public int JACKPOT_COOLDOWN_SPINS = 25;
        public float COOLDOWN_CAP = 42f;      // 冷卻期間BOOST上限

        // 無押大獎遮罩（該轉無押大獎時，降低實際生效的BOOST，避免顯著關聯）
        public float NO_BET_MASK = 0.55f;     // 只對該轉生效（不改內部狀態）

        // RTP 平滑（EWMA）
        public float EWMA_ALPHA = 0.15f;      // 0.1~0.2 之間都可

        // === 內部狀態 ===
        private float _boost;                 // 內部基準BOOST（作為 NO_BET_JACKPOT_BOOST 的來源）
        private float _rtpEwma = 0.68f;       // 平滑後RTP初始化為目標附近
        private float _integral;
        private int _cooldownLeft = 0;
        private Random _rng = new Random();

        public BoostRegulator(float? initBoost = null)
        {
            _boost = initBoost ?? 50f;        // 初始抓中位數
        }

        /// <summary>
        /// 每轉結束後呼叫：用實際「該轉RTP」更新平滑RTP與內部BOOST
        /// </summary>
        /// <param name="spinBet">該轉下注總額</param>
        /// <param name="spinPayout">該轉派彩</param>
        /// <param name="justHitJackpot">是否剛中大獎（含 PRIZE_PICK、GOLD_MANIA、GOLDEN_TREASURE）</param>
        public void OnSpinEnd(int spinBet, int spinPayout, bool justHitJackpot)
        {
            if (spinBet > 0)
            {
                float spinRtp = (float)spinPayout / spinBet; // 該轉RTP
                // EWMA 平滑
                _rtpEwma = EWMA_ALPHA * spinRtp + (1 - EWMA_ALPHA) * _rtpEwma;
            }

            if (justHitJackpot)
            {
                // 重置積分避免積分風暴；開啟冷卻
                _integral = Clamp(_integral, -0.02f, 0.02f);
                _cooldownLeft = JACKPOT_COOLDOWN_SPINS;
            }

            // 控制律：PI + 死區 + 每轉最大步長
            float err = TARGET_RTP - _rtpEwma; // 目標-實際（正值=拉高RTP→提升大獎傾向）

            float delta = 0f;
            float halfBand = DEADBAND; // 我們把「±5%」當死區半寬

            if (Math.Abs(err) <= halfBand)
            {
                // 在死區內：緩慢回歸 BOOST_MID，保持刺激但不漂移
                float toMid = BOOST_MID - _boost;
                delta = Math.Sign(toMid) * Math.Min(Math.Abs(toMid), 1.5f); // 緩慢靠攏
            }
            else
            {
                // PI 控制
                _integral += err;
                _integral = Clamp(_integral, -0.20f, 0.20f); // 反風up

                float pTerm = Kp * err;
                float iTerm = Ki * _integral;

                delta = pTerm + iTerm;

                // 將「RTP誤差」映射到合理尺度（避免一次跳太多）
                delta = Clamp(delta, -MAX_STEP_PER_SPIN, MAX_STEP_PER_SPIN);
            }

            // 懲罰/保護：大獎冷卻期間不允許BOOST超過COOLDOWN_CAP
            float newBoost = _boost + delta;
            if (_cooldownLeft > 0)
            {
                newBoost = Math.Min(newBoost, COOLDOWN_CAP);
                _cooldownLeft--;
            }

            // 加入細微抖動，避免被抓到規律（不影響整體趨勢）
            float jitter = (float)(_rng.NextDouble() * 2 * JITTER_RANGE - JITTER_RANGE);
            newBoost += jitter * 0.5f; // 抖動縮半用以溫和

            _boost = Clamp(newBoost, BOOST_MIN, BOOST_MAX);
            Console.WriteLine($"[BoostRegulator] EWMA_RTP={_rtpEwma:F3}, BOOST={_boost:F1}");

        }

        /// <summary>
        /// 取得本轉實際生效的 NO_BET_JACKPOT_BOOST。
        /// 若該轉大獎無下注，套「遮罩」只對這一轉降低，避免沒押就強開。
        /// </summary>
        public float GetNoBetJackpotBoost(bool hasJackpotBet)
        {
            if (hasJackpotBet) return _boost;

            // 無押大獎 → 套遮罩，不修改內部_boost，只在該轉壓低
            float masked = _boost * NO_BET_MASK;

            // 加一點微噪，避免固定比例被看穿
            float eps = (float)(_rng.NextDouble() * 0.06 - 0.03); // ±3%
            masked *= (1f + eps);

            // 冷卻期間再多壓一點（更保守）
            if (_cooldownLeft > 0) masked = Math.Min(masked, COOLDOWN_CAP * NO_BET_MASK);

            return Math.Max(masked, BOOST_MIN * 0.5f);
        }

        /// <summary>
        /// 直接取當前內部基準BOOST（用於監控/Log）
        /// </summary>
        public float CurrentBaselineBoost => _boost;

        /// <summary>
        /// 取當前EWMA RTP（用於監控/Log）
        /// </summary>
        public float CurrentSmoothedRTP => _rtpEwma;

        private static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}
