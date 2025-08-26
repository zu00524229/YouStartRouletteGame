using System;
using System.Collections.Generic;
using System.IO;
using YourNamespace.Core.Utils;
using YSPFrom;
using YSPFrom.Core.RTP;

namespace YSPFrom.Core.Logging
{
    public static class LogManager
    {
        private static readonly object _lock = new object();
        private static readonly string LogBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        // class 裡面加一個靜態變數，記錄當前局號
        private static long _currentRoundId = 0;

        static LogManager()
        {
            Directory.CreateDirectory(LogBasePath);
        }

        public static void Log(string category, string message)
        {
            lock (_lock)
            {
                string filePath = Path.Combine(LogBasePath, $"{category}_{DateTime.Now:yyyyMMdd}.log");
                string logLine = $"[{DateTime.Now:HH:mm:ss}] {message}";
                File.AppendAllText(filePath, logLine + Environment.NewLine);

                // UI 顯示（可選）
                //Program.MainForm?.LogBase($"[{category}] {message}");
            }
        }

        // 局號 -> 玩家ID 對應
        private static readonly Dictionary<long, string> _roundUserMap = new Dictionary<long, string>();

        // 玩家成就累積：userId -> (大獎名稱 -> 次數)
        private static readonly Dictionary<string, Dictionary<string, int>> _playerAchievements
            = new Dictionary<string, Dictionary<string, int>>();


        #region 統一管理型別
        public enum LotteryLogType
        {
            // ChatHub
            PlayerLogoutBalance,
            ClientConnected,    // 前端連線
            ClientDisconnected, // 前端斷線
            BetDataReceived,    // 收到下注資料
            BetAreaReceived,     // 收到下注區金額

            // 金流事件
            BalanceBeforeBet,   // 抽獎前餘額
            BalanceAfterBet,    // 扣注後餘額
            BalanceAfterPayout,  // 派彩後餘額
            RoundSummary,       // 「局號、獎項、下注額、倍率、派彩」
            OtherInfo,       // 額外補充資訊


            // LotterySerivcr
            Jackpot,            // 大獎後重製RTP + 詳細紀錄
            WinResult,          // 中獎結果
            BetAmounts,         // 下注區金額
            JackpotContribution, // 獎池提撥
            RTPStatus,          // 顯示 RTP 狀態
            RTPHistoryStats,     // 當前歷史統計

            // ExtraPayManager
            ExtraPayTriggered, // ExtraPay 觸發紀錄

            // OutcomeSelector 
            NetProfitCheck,       // 安全機制：當前淨利
            PoolBiasAdjustment,   // 偏壓調整
            ExtraPayCheck,        // ExtraPay 檢查
            DebugHitResult,        // [DEBUG] 抽中結果
            // Outcome_JackpotGate 
            JackpotPoolInsufficient, // 獎池不足移除
            JackpotThresholdOK,      // 大獎達標
            JackpotThresholdFail,     // 淨利不足
            // Outcome_JackpotLimiter
            JackpotLimiterBefore // 封頂前紀錄
        }
        #endregion

        #region Switch

        public static void LotteryLog(LotteryLogType type, params object[] args)
        {
            switch (type)
            {
                #region ChatHub Log

                case LotteryLogType.PlayerLogoutBalance:
                    {
                        string userId = (string)args[0];
                        int balance = (int)args[1];

                        string msg = $"❌ 玩家登出：[ {userId} ] 登出時餘額={balance}";
                        Console.WriteLine(msg);
                        Program.MainForm?.LogPlayerRoundBalance(msg); // 玩家分頁
                        Program.MainForm?.LogPlayerStatus(msg);       // 狀態分頁
                    }
                    break;

                case LotteryLogType.ClientConnected:
                    {
                        string userId = (string)args[0];
                        string msg = $"✅ 玩家登入成功：{userId}";
                        Console.WriteLine(msg);
                        Program.MainForm?.LogPlayerStatus(msg);
                    }
                    break;

                case LotteryLogType.ClientDisconnected:
                    {
                        string userId = (string)args[0];
                        string msg = $"❌ 玩家已斷線：{userId}";
                        Console.WriteLine(msg);
                        Program.MainForm?.LogPlayerStatus(msg);
                    }
                    break;

                case LotteryLogType.BetDataReceived:
                    {
                        int totalBet = (int)args[0];
                        bool isAutoMode = (bool)args[1];
                        Console.WriteLine(" 收到下注資料：");
                        Console.WriteLine($"自動模式: {isAutoMode}");

                        Program.MainForm?.LogBet("收到下注資料：");
                        Program.MainForm?.LogBet($"總下注: {totalBet}");
                        Program.MainForm?.LogBet($"自動模式: {isAutoMode}");
                    }
                    break;

                case LotteryLogType.BetAreaReceived:
                    {
                        long roundId = _currentRoundId;
                        string userId = _roundUserMap.ContainsKey(roundId) ? _roundUserMap[roundId] : "UNKNOWN";


                        string areaName = (string)args[0];
                        int betAmount = (int)args[1];
                        string msg = $"[下注][局號={roundId}][{userId}] 區域 {areaName} 金額={betAmount}"; Console.WriteLine(msg);
                        Program.MainForm?.LogBet(msg);
                    }
                    break;
                #endregion

                #region 管理 LotterySerivcr Log
                case LotteryLogType.Jackpot:    // 命中大獎
                    {
                        long roundId = _currentRoundId;
                        string userId = _roundUserMap.ContainsKey(roundId) ? _roundUserMap[roundId] : "UNKNOWN";

                        string rewardName = (string)args[0];
                        int finalMultiplier = (int)args[1];
                        int winAmount = (int)args[2];
                        double currentRTP = (float)args[3];

                        if (winAmount > 0)
                        {
                            // 🆕 累積玩家成就
                            if (!_playerAchievements.ContainsKey(userId))
                                _playerAchievements[userId] = new Dictionary<string, int>();
                            if (!_playerAchievements[userId].ContainsKey(rewardName))
                                _playerAchievements[userId][rewardName] = 0;

                            _playerAchievements[userId][rewardName]++;

                            int count = _playerAchievements[userId][rewardName];

                            // 🖨️ 印出成就 Log
                            string msg = $"[局號={roundId}][{userId}] 🏆 大獎 {rewardName} 倍率={finalMultiplier} → 派彩={winAmount} | 累積 {count} 次";
                            Console.WriteLine(msg);
                            Program.MainForm?.LogBigResult(msg);

                            // 額外記錄 RTP 狀態
                            string rtpMsg = $"大獎後RTP={currentRTP:0.0000} | 累計下注={RTPManager.totalBets}, 累計派彩={RTPManager.totalPayouts}, 轉盤次數={RTPManager.spinCount}";
                            Program.MainForm?.LogRTP(rtpMsg);

                            // 🆕 也可以印到成就分頁
                            Program.MainForm?.LogPlayereffort($"🎖️ 成就：{userId} 第 {count} 次中 {rewardName}");
                        }
                    }
                        break;

                case LotteryLogType.WinResult:
                    {
                        long roundId = _currentRoundId;
                        string userId = _roundUserMap.ContainsKey(roundId) ? _roundUserMap[roundId] : "UNKNOWN";

                        string rewardName = (string)args[0];
                        int finalMultiplier = (int)args[1];
                        int winAmount = (int)args[2];

                        if (winAmount > 0)
                        {
                            string msg = $"[局號={_currentRoundId}][{userId}] 🎯 中獎結果: {rewardName} x{finalMultiplier} → 派彩={winAmount}";
                            Console.WriteLine(msg);
                            Program.MainForm?.LogResult(msg);
                        }
                    }
                    break;

                case LotteryLogType.BetAmounts:
                    {
                        var betAmounts = (Dictionary<string, int>)args[0];
   
                    }
                    break;

                case LotteryLogType.JackpotContribution:
                    {
                        double contribution = (double)args[0];
                        double poolBalance = (double)args[1];

                        string msg = $"[獎池] 提撥 {contribution} 到超級大獎池，剩餘 {poolBalance:0}";
                        Program.MainForm?.LogJackpot(msg);
                    }
                    break;

                case LotteryLogType.RTPStatus:
                    {
                        string rtpMsg = RTPManager.GetRTPStatusMessage();
                        Console.WriteLine(rtpMsg);
                        Program.MainForm?.LogRTP(rtpMsg);
                    }
                    break;

                case LotteryLogType.RTPHistoryStats:
                    {
                        RTPManager.LogLifetimeStats();
                    }
                    break;
                #endregion

                #region Outcome Log
                case LotteryLogType.NetProfitCheck:
                    {
                        double net = RTPManager.GetNetProfit();
                        string msg = $"[安全機制] 當前淨利 = {net:0}";
                        Console.WriteLine(msg);
                        Program.MainForm?.LogRTP(msg);  // 即時狀態 → LogRTP
                    }
                    break;

                case LotteryLogType.PoolBiasAdjustment:
                    {
                        float alpha = (float)args[0];
                        float boostLow = (float)args[1];
                        float boostMid = (float)args[2];
                        float boostHigh = (float)args[3];
                        string msg = $"[偏壓調整] α(控獎強度)={alpha:0.000}, 低倍加成={boostLow:0.000}, 中倍加成={boostMid:0.000}, 高倍加成={boostHigh:0.000}";
                        Console.WriteLine(msg);
                        Program.MainForm?.LogRTP(msg);
                    }
                    break;

                case LotteryLogType.ExtraPayCheck:
                    {
                        double currentRtp = Convert.ToDouble(args[0]);
                        double targetRtp = Convert.ToDouble(args[1]);
                        Console.WriteLine("[Debug] ExtraPay 檢查開始");
                        Console.WriteLine($"當前 RTP: {currentRtp}, 目標 RTP: {targetRtp}");
                    }
                    break;

                case LotteryLogType.DebugHitResult:
                    {
                        string rewardName = (string)args[0];
                        int betOnHit = (int)args[1];
                        int multiplier = (int)args[2];
                        int payout = (int)args[3];
                        Console.WriteLine($"[DEBUG] hit={rewardName}, betOnHit={betOnHit}, multiplier={multiplier}, payout={payout}");
                    }
                    break;


                #endregion

                #region Outcome_JackpotGate Log
                case LotteryLogType.JackpotPoolInsufficient:
                    {
                        string rewardName = (string)args[0];
                        double needPool = (double)args[1];
                        double poolBalance = (double)args[2];
                        string msg = $"[獎池機制] 獎池不足 → 移除 {rewardName} | 需求={needPool:0}, 目前={poolBalance:0}";
                        Console.WriteLine(msg);
                        Program.MainForm?.LogJackpot(msg);
                    }
                    break;

                case LotteryLogType.JackpotThresholdOK:
                    {
                        string rewardName = (string)args[0];
                        double net = (double)args[1];
                        double probability = (double)args[2];
                        string msg = $"[門檻] {rewardName} 達標 → 目前淨利={net:0} | 中獎機率={probability:P4}";
                        Console.WriteLine(msg);
                        Program.MainForm?.LogRTP(msg);
                        // ★ 超大獎，額外記錄到大獎分頁
                        if (rewardName == "GOLDEN_TREASURE")
                            Program.MainForm?.LogJackpot(msg);
                    }
                    break;

                case LotteryLogType.JackpotThresholdFail:
                    {
                        string rewardName = (string)args[0];
                        double need = (double)args[1];
                        double net = (double)args[2];
                        // ❌ 安全機制觸發 → 歷史分頁
                        string msg = $"[安全機制] 淨利不足 → 移除 {rewardName} | 需求={need:0}, 淨利={net:0}";
                        Console.WriteLine(msg);
                        Program.MainForm?.LogRTPhistory(msg);

                        // ★ 超大獎，額外記錄到大獎分頁
                        if (rewardName == "GOLDEN_TREASURE")
                            Program.MainForm?.LogJackpot(msg);
                    }
                    break;
                #endregion

                #region Outcome_JackpotLimiter Log
                case LotteryLogType.JackpotLimiterBefore:
                    {
                        double jackSum = Convert.ToDouble(args[0]);
                        double nonJackSum = Convert.ToDouble(args[1]);
                        double jackpotCap = Convert.ToDouble(args[2]);

                        string msg = $"[封頂前] 大獎權重總和={jackSum}, 非大獎權重總和={nonJackSum}, 大獎上限={jackpotCap}";
                        Console.WriteLine(msg);
                        Program.MainForm?.LogRTP(msg);
                    }
                    break;
                #endregion

                #region ExtraPayManager Log
                case LotteryLogType.ExtraPayTriggered:
                    {
                        long roundId = _currentRoundId;
                        string userId = _roundUserMap.ContainsKey(roundId) ? _roundUserMap[roundId] : "UNKNOWN";

                        string rewardName = (string)args[0];   // ✅ 區域名稱
                        int betAmount = (int)args[1];          // ✅ 下注金額
                        int extraMultiplier = (int)args[2];    // ✅ 加倍倍數

                        if (betAmount > 0)
                        {
                            string msg = $"[局號={_currentRoundId}][{userId}] 🎉 觸發 ExtraPay：{rewardName} 區域，加倍 x{extraMultiplier}";
                            Console.WriteLine(msg);
                            Program.MainForm?.LogResult(msg);
                            //Program.MainForm?.LogPlayereffort($"ExtraPay {rewardName} x{extraMultiplier}");
                        }
                    }
                    break;
                #endregion

                #region ChatHub.StartLottery 金流事件
                case LotteryLogType.BalanceBeforeBet:
                    {
                        Console.WriteLine($"[DEBUG] BalanceBeforeBet args.Length={args.Length}");

                        // 每一局開始時，產生一個新的局號
                        long roundId = RoundIdGenerator.NextId();
                        _currentRoundId = roundId;

                        string userId = (string)args[0];

                        // ✅ 綁定局號 -> 玩家ID
                        _roundUserMap[roundId] = userId;

                        int balanceBefore = Convert.ToInt32(args[1]);
                        int totalBet = Convert.ToInt32(args[2]);

                        string msg = $"[局號={roundId}][{userId}] 餘額流動：{balanceBefore} → (下注 {totalBet})";
                        Console.WriteLine(msg);
                        Program.MainForm?.LogBalanceLeft(msg);  // ① 玩家餘額變化
                        Program.MainForm?.LogPlayerRoundBalance(msg); // 玩家分頁：局號/餘額
                    }
                    break;
                case LotteryLogType.BalanceAfterBet:
                    {
                        Console.WriteLine($"[DEBUG] BalanceBeforeBet args.Length={args.Length}");

                        long roundId = _currentRoundId;
                        string userId = _roundUserMap.ContainsKey(roundId) ? _roundUserMap[roundId] : "UNKNOWN";

                        int balanceAfterBet = Convert.ToInt32(args[0]);

                        string msg = $"[局號={roundId}][{userId}] 扣注後餘額={balanceAfterBet}";
                        Program.MainForm?.LogBalanceLeft(msg);  // ① 玩家餘額變化
                        Program.MainForm?.LogPlayerRoundBalance(msg); // 玩家分頁

                    }
                break;

                case LotteryLogType.BalanceAfterPayout:
                    {
                        long roundId = _currentRoundId;
                        string userId = _roundUserMap.ContainsKey(roundId) ? _roundUserMap[roundId] : "UNKNOWN";

                        int payout = Convert.ToInt32(args[0]);
                        int balanceAfterPayout = Convert.ToInt32(args[1]);

                        string msg = $"[局號={roundId}][{userId}] 派彩={payout}, 結算後餘額={balanceAfterPayout}";
                        Console.WriteLine(msg);
                        Program.MainForm?.LogBalanceRight(msg);  // ① 玩家餘額變化(派彩後)
                        Program.MainForm?.LogPlayerRoundBalance(msg); // 玩家分頁

                    }
                    break;

                case LotteryLogType.RoundSummary:
                    {
                        long roundId = _currentRoundId;
                        string userId = _roundUserMap.ContainsKey(roundId) ? _roundUserMap[roundId] : "UNKNOWN";

                        string rewardName = Convert.ToString(args[0]);
                        int betAmount = Convert.ToInt32(args[1]);
                        int multiplier = Convert.ToInt32(args[2]);
                        int netChange = Convert.ToInt32(args[3]);

                        string msg = $"[局號={roundId}][{userId}] 下注={betAmount}, 獎項={rewardName}, 倍率={multiplier}, 淨變化={(netChange >= 0 ? "+" : "")}{netChange}";
                        Console.WriteLine(msg);
                        Program.MainForm?.LogRoundSummary(msg);
                    }
                    break;

                case LotteryLogType.OtherInfo:
                    {
                        long roundId = _currentRoundId;
                        string userId = _roundUserMap.ContainsKey(roundId) ? _roundUserMap[roundId] : "UNKNOWN";

                        double rtp = Convert.ToDouble(args[1]);
                        int bets = Convert.ToInt32(args[2]);
                        int payouts = Convert.ToInt32(args[3]);
                        int bal = Convert.ToInt32(args[4]);
                        double jackpot = Convert.ToDouble(args[5]);

                        string msg = $"[System][局號={roundId}][{userId}] RTP={rtp:0.0000}, 總下注={bets}, 總派彩={payouts}, 餘額={bal}, 超大獎池={jackpot:0}";
                        Console.WriteLine(msg);
                        Program.MainForm?.LogOtherInfo(msg);   // ④ 系統/除錯
                    }
                    break;
                    #endregion

            }
        }

        #endregion

        #region 舊版

        #endregion


        // 小工具：安全取得 int / long
        static int GetInt(object[] a, int idx, int def = 0)
        {
            if (a == null || idx < 0 || idx >= a.Length || a[idx] == null) return def;
            try { return Convert.ToInt32(a[idx]); } catch { }
            int v; return int.TryParse(a[idx].ToString(), out v) ? v : def;
        }

        static long GetLong(object[] a, int idx, long def = 0)
        {
            if (a == null || idx < 0 || idx >= a.Length || a[idx] == null) return def;
            try { return Convert.ToInt64(a[idx]); } catch { }
            long v; return long.TryParse(a[idx].ToString(), out v) ? v : def;
        }
    }
}
