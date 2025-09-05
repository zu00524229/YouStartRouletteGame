using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSPFrom.Models
{
    public class RoundContext
    {
        public long RoundId { get; set; }
        public string UserId { get; set; }

        // 下注與派彩
        public long BetAmount { get; set; }
        public long Contribution { get; set; }   // 提撥獎池
        public long Payout { get; set; }
        public long NetChange => Payout - BetAmount;

        // 玩家資訊
        public long BalanceBefore { get; set; }
        public long BalanceAfter { get; set; }

        // 中獎資訊
        public string RewardName { get; set; }
        public int Multiplier { get; set; }
        public bool IsJackpot { get; set; }
        public ExtraPayInfo ExtraPay { get; set; } // 如果有加倍

        // 獎池 / RTP
        public double PoolBalance { get; set; }
        public double CurrentRTP { get; set; }

        // 狀態標記
        public bool InsufficientBalance { get; set; }
        public string Message { get; set; }
    }
}

