using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSPFrom.Configs;

namespace YSPFrom.Core.RTP
{
    // ==================== 每一格的模型（由 RewardTable 展開） ====================
    // RewardTable 是「獎項設定表」，而這裡的 WheelCell 是將其展開成「每一格」的資料物件
    // 例如 GOLDEN_TREASURE 在 RewardTable 中只佔 1 筆（indices = [48]），
    // 但在這裡會生成 1 個 WheelCell 物件，方便後續分池、加權、抽獎等操作。
    public class WheelCell
    {
        public string RewardName;   // 獎項名稱
        public int Index;           // 轉盤格子 index
        public int Min;             // 該格的最小倍率
        public int Max;             // 該格的最大倍率
        public bool IsJackpot;      // 是否為大獎（min != max）
        public float BaseWeight;    // 該格的基礎權重（可用於調整特定格的機率）
    }

    internal static class WheelModel
    {
        // 轉盤清單
        public static readonly List<WheelCell> Cells = BuildCells();

        // 從 RewardTable 展開所有獎格
        private static List<WheelCell> BuildCells()
        {
            var list = new List<WheelCell>();
            // 來自你的 RewardTable.Table（獎項名 → (indices, min, max)）
            foreach (var kv in RewardTable.Table)
            {
                string name = kv.Key;
                List<int> indices = kv.Value.indices;   // 該獎項對應的轉盤編號
                int min = kv.Value.min;
                int max = kv.Value.max; 
                bool isJackpot = (min != max);      // 若倍率範圍有差異 → 視為大獎

                for (int i = 0; i < indices.Count; i++)
                {
                    list.Add(new WheelCell
                    {
                        RewardName = name,
                        Index = indices[i],
                        Min = min,
                        Max = max,
                        IsJackpot = isJackpot,
                        BaseWeight = 1f     // 預設基礎權重為 1
                    });
                }
            }
            return list;
        }

        // 計算期望倍率（用於判斷低/中/高倍池）
        public static double ExpectedMultiplier(int min, int max)
        {
            if (min == max) return min;     // 固定倍率直接回傳

            // 對數型權重的期望：偏小～中倍，保留長尾
            double mi = Math.Max(1.0, (double)min);
            double ma = Math.Max(mi + 1e-6, (double)max);
            double exp = (ma - mi) / Math.Log(ma / mi);

            // 確保期望值不超出範圍
            if (exp < mi) exp = mi;
            if (exp > ma) exp = ma;
            return exp;
        }

        // 依期望倍率分池：低倍 / 中倍 / 高倍）
        public static void SplitPools(out List<WheelCell> lows, out List<WheelCell> mids, out List<WheelCell> highs)
        {
            const int LowCap = 6;   // <=6X 視為低倍
            const int HighCap = 20;  // >=20X 視為高倍（含 PRIZE_PICK 以上）
            lows = new List<WheelCell>();
            mids = new List<WheelCell>();
            highs = new List<WheelCell>();

            // 遍歷所有格子，依期望倍率分入對應的池子
            foreach (var c in Cells)
            {
                double eMul = ExpectedMultiplier(c.Min, c.Max);
                if (eMul <= LowCap) lows.Add(c);
                else if (eMul >= HighCap) highs.Add(c);
                else mids.Add(c);
            }
        }
    }
}

