using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSPFrom.Models
{
    // ==================== 下注資料：用「獎項名」對應金額 ====================

    public class BetData    // 定義前端送來的Json資料結構
    {
        public Dictionary<string, int> betAmounts { get; set; }     // 下注區域與金額
        public bool isAutoMode { get; set; }                        // 是否為自動模式
        //public int totalBet { get; set; }                           // 總下注金額
        public int totalBet => betAmounts?.Values.Sum() ?? 0;       // 總下注金額( 及時計算 )
    }
}
