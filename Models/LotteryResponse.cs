using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSPFrom.Models
{
    // ✨ 回傳給前端的封包（包含抽獎結果 + 結算後的餘額資訊）
    public class LotteryResponse
    {
        // 當前局號
        public string roundId { get; set; }    
        //public LotteryResult result { get; set; }
        // 抽獎結果資料（名稱、倍率、派彩金額、ExtraPay 等）

        public long balanceBefore { get; set; }
        // 抽獎前玩家餘額（扣下注金額前的數值）

        public long balanceAfter { get; set; }
        // 抽獎結算後玩家餘額（派彩加回後的最終數值）

        public long totalBet { get; set; }
        // 本輪總下注金額（來自 BetData.totalBet）


        //public int netChange => (result?.payout ?? 0) - totalBet;
        public long netChange => balanceAfter - balanceBefore;
        // 本輪淨變化金額（派彩金額 - 下注金額）
        // 正數代表贏錢，負數代表輸錢，0 代表打平

        public bool insufficientBalance { get; set; }
        // 若為 true 表示下注前餘額不足，這輪未成功扣款/開獎

        public string message { get; set; }
        // 給前端顯示的提示訊息（例如 "OK"、"餘額不足"、"未登入"）
    }

}
