using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSPFrom.Core.Logging;
using YSPFrom.Core.RTP;   // WheelCell, RTPManager
using YSPFrom.Core.SuperJackpot;
using YSPFrom.Models;     // BetData

namespace YSPFrom.Engine.Outcome
{
    public class Outcome_JackpotGate
    {
        public static void Apply(ref List<WheelCell> list, BetData data)
        {
            /// <summary>
            /// 控制大獎是否允許進入抽獎池：
            /// - 若「三大獎中任一有下注」，則移除其他沒下注的大獎（防止沒押還頻繁開）。
            /// - 若該大獎有下注，則依據淨利儲備檢查是否允許開出。
            /// - 若沒有任何大獎下注，則允許沒下注的大獎存在（可用於誘導下注邏輯）。
            /// </summary>
            /// <param name="list">候選獎項列表（會直接修改）</param>
            /// <param name="data">玩家下注資料</param>
            /// 

            var localList = list;
            var logged = new HashSet<string>();

            list.RemoveAll(c =>
            {
                if (!c.IsJackpot) return false;     // 非大獎 > 不處理
                   
                // 已經檢查過該獎項 > 跳過
                if (logged.Contains(c.RewardName))  
                    return false;

                // 取該大獎下注金額
                double betAmt = 0;
                if (data?.betAmounts != null &&
                    data.betAmounts.TryGetValue(c.RewardName, out int amt) &&
                    amt > 0)
                {
                    betAmt = amt;
                }

                if (betAmt <= 0) return false;

                // 如果是 GOLDEN_TREASURE，額外判斷獎池
                if (c.RewardName == "GOLDEN_TREASURE")
                {
                    // 取得該大獎的最大可能倍率（4檔固定）
                    int[] gtMultipliers = { 100, 800, 1700, 3000 };
                    int maxMul = gtMultipliers.Max();

                    // 計算獎池需求（CoverK 可調，例如 1.0 = 必須完全覆蓋成本）
                    double coverK = 1.0;
                    double needPool = betAmt * maxMul * coverK;

                    if (SuperJackpotPool.PoolBalance < needPool)
                    {
                        Console.WriteLine($"[獎池機制] 獎池不足 → 移除 GOLDEN_TREASURE | 需求={needPool:0}, 目前={SuperJackpotPool.PoolBalance:0}");
                        //Program.MainForm?.LogJackpot($"[獎池機制] 獎池不足 → 移除 GOLDEN_TREASURE | 需求={needPool:0}");
                        LogManager.LotteryLog(LogManager.LotteryLogType.JackpotPoolInsufficient, c.RewardName, needPool, SuperJackpotPool.PoolBalance); // 統一Log 管理
                        return true; // 移除
                    }
                }

                // 有下注該大獎 → 檢查淨利儲備
                bool ok = RTPManager.CanOpenJackpot(c.RewardName, betAmt, out double need, out double net);

                if (ok)
                {
                    double jackpotWeight = localList.Where(x => x.RewardName == c.RewardName).Sum(x => x.BaseWeight);
                    double totalWeight = localList.Sum(x => x.BaseWeight);
                    double probability = totalWeight > 0 ? jackpotWeight / totalWeight : 0;

                    LogManager.LotteryLog(LogManager.LotteryLogType.JackpotThresholdOK, c.RewardName, net, probability);     // 統一管理 Log
                }
                else
                {
                    LogManager.LotteryLog(LogManager.LotteryLogType.JackpotThresholdFail, c.RewardName, need, net);     // 統一管理 Log
                }

                logged.Add(c.RewardName);
                return !ok;
            });
        }
    }
}
