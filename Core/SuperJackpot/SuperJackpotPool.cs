using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSPFrom.Core.SuperJackpot
{
    /// <summary>
    /// 超級大獎池（每局提撥、派彩扣除）
    /// </summary>
    public static class SuperJackpotPool
    {
        public static long PoolBalance { get; private set; }
        public static double PoolRate = 0.05; // 提撥比例 (5%)

        /// <summary>
        /// 本局提撥
        /// </summary>
        public static long AddContribution(long totalBet)
        {
            long contribution = 0;
            if (totalBet > 0)
            {
                contribution = (long)(totalBet * PoolRate); // 用 long 儲存
                PoolBalance += contribution;
                Console.WriteLine($"[SJP] 本局提撥 {contribution}, 大獎池餘額={PoolBalance}");
            }
            return contribution;
        }

        /// <summary>
        /// 扣除派彩
        /// </summary>
        public static void Deduct(long payout)
        {
            if (payout > 0)
            {
                PoolBalance -= payout;
                if (PoolBalance < 0) PoolBalance = 0;
                Console.WriteLine($"[SJP] 大獎派彩 {payout}, 扣除後餘額={PoolBalance}");
            }
        }

        /// <summary>
        /// 手動加錢（活動/營運調整）
        /// </summary>
        public static void Inject(long amount)
        {
            if (amount > 0)
            {
                PoolBalance += amount;
                Console.WriteLine($"[SJP] 手動注入 {amount}, 新餘額={PoolBalance}");
            }
        }
    }

}
