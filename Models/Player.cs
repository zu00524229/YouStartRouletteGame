using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 管理玩家資料連線與餘額
namespace YSPFrom.Models
{
    public class Player
    {
        public string UserId { get; set; }      // 帳號
        public string Passworld { get; set; }   // 密碼
        public int Balance { get; set; }    // 餘額
        public string ConnectionId { get; set; }    // 當前連線ID (登入後綁定)
        //public bool IsAuto { get; set; }    // 是否為自動狀態
        // 新增：紀錄本回合下注（key=區域名, value=金額）
        public Dictionary<string, int> CurrentRoundBets { get; set; } = new Dictionary<string, int>();
    }
}
