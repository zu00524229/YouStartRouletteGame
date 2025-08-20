using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSPFrom.Models
{
    public class ExtraPayInfo   // ExtraPay 的資料結構
    {
        public string rewardName { get; set; }     // 哪個下注區加倍
        public int extraMultiplier { get; set; }   // 加倍倍數
    }
    public class LotteryResult  // 抽獎結果的資料結構
    {
        public string rewardName { get; set; }  //  中獎名稱
        public int rewardIndex { get; set; }    //  中獎 index (動畫用)
        public int multiplier { get; set; }     //  倍率 (獎金倍率)
        public int payout { get; set; }         // 實際派彩
        public bool isJackpot { get; set; }     // 是否為大獎類
        public ExtraPayInfo extraPay { get; set; }  // 可以為 null，只有觸發加倍時才回傳
    }
}
