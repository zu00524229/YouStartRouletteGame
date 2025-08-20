using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// =================== 前端輪盤 對應表 ======================
namespace YSPFrom.Configs
{
    public static class RewardTable
    {
        // 對應每個獎項的設定（名稱、對應index、最小倍數、最大倍數）
        public static readonly Dictionary<string, (List<int> indices, int min, int max)> Table =
            new Dictionary<string, (List<int>, int, int)>
        {
        { "GOLDEN_TREASURE", (new List<int> { 48 }, 100, 3000) },
        { "GOLD_MANIA",      (new List<int> { 14, 31 }, 25, 500) },
        { "PRIZE_PICK",      (new List<int> { 6, 23, 40 }, 15, 100) },
        { "10X",             (new List<int> { 10, 18, 36, 44 }, 10, 10) },
        { "6X",              (new List<int> { 3, 16, 22, 27, 33, 42, 49 }, 6, 6) },
        { "4X",              (new List<int> { 1, 5, 9, 12, 20, 25, 29, 34, 38, 41, 46 }, 4, 4) },
        { "2X",              (new List<int> { 0, 2, 4, 8, 11, 13, 15, 17, 19, 21, 24, 26, 28, 30, 32, 35, 37, 39, 43, 45, 47 }, 2, 2) }
        };
    }

}
