using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using YSPFrom.Core.Logging;
using YSPFrom.Hubs.PlayerHub;
using YSPFrom.Models;

namespace YSPFrom
{
    public static class PlayerManager
    {
        // ================================= 基本暫存 =================================
        #region 玩家連線管理
        // key: ConnectionId
        // 使用 ConcurrentDictionary 來保存「目前在線玩家」
        private static readonly ConcurrentDictionary<string, Player> _players = new ConcurrentDictionary<string, Player>();

        /// <summary>
        /// 新增或更新一個玩家的連線 (依照 ConnectionId 存)
        /// </summary>
        public static void Add(Player player)
        {
            if (string.IsNullOrEmpty(player.ConnectionId)) return;
            _players[player.ConnectionId] = player;
        }

        /// <summary>
        /// 依照 ConnectionId 取得玩家 (用於 SignalR 的連線識別)
        /// </summary>
        public static Player GetByConnectionId(string connectionId)
        {
            _players.TryGetValue(connectionId, out var player);
            return player;
        }

        /// <summary>
        /// 依照 UserId 取得玩家 (用於遊戲內帳號查詢)
        /// </summary>
        public static Player GetByUserId(string userId)
        {
            return _players.Values.FirstOrDefault(p => p.UserId == userId);
        }

        /// <summary>
        /// 移除一個玩家的連線 (通常在斷線時呼叫)
        /// </summary>
        public static void RemoveByConnectionId(string connectionId)    // 清除連線
        {
            _players.TryRemove(connectionId, out _);
        }

        /// <summary>
        /// 取得所有目前在線的玩家
        /// (⚠️ 注意：只包含已成功綁定 ConnectionId 的玩家，不含假資料庫)
        /// </summary>
        public static IEnumerable<Player> GetAll()
        {
            return _players.Values;
        }
        #endregion

        // ================================= 假資料 / 驗證 =================================
        #region 假資料庫
        private static readonly Dictionary<string, Player> playersDb = new Dictionary<string, Player>
        {
            { "ethan",  new Player { UserId = "ethan",  Passworld = "zxc123", Balance = 10000000 } },
            { "ed",  new Player { UserId = "ed",  Passworld = "zxc123", Balance = 10000000 } },
            { "book",  new Player { UserId = "book",  Passworld = "zxc123", Balance = 10000000000 } },
            { "player", new Player { UserId = "player", Passworld = "zxc123", Balance = 10000000 } },
            { "player2", new Player { UserId = "player2", Passworld = "zxc123", Balance = 10000000 } },
            { "player3", new Player { UserId = "player3", Passworld = "zxc123", Balance = 10000000 } },
            { "player4", new Player { UserId = "player4", Passworld = "zxc123", Balance = 10000000 } },
            { "player5", new Player { UserId = "player5", Passworld = "zxc123", Balance = 10000000 } },
            { "player6", new Player { UserId = "player6", Passworld = "zxc123", Balance = 10000000 } },
        };

        /// <summary>
        /// 取得假資料庫中所有玩家 (不代表在線)
        /// </summary>
        public static IEnumerable<Player> GetAllPlayers() => playersDb.Values;
        #endregion

        #region 登入邏輯
        private static readonly object _loginLock = new object();
        public static (bool success, string message, Player player) Login(string username, string password, string connId, dynamic clients)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return (false, "帳號或密碼不得為空", null);

            if (!playersDb.ContainsKey(username) || playersDb[username].Passworld != password)
                return (false, "帳號或密碼錯誤", null);
            var player = playersDb[username];   // 讀資料庫

            lock (_loginLock)  // 🔒 關鍵：避免多視窗同時操作
            {
                if (!string.IsNullOrEmpty(player.ConnectionId))
                {
                    // 踢掉舊的
                    //clients.Client(player.ConnectionId).broadcastMessage("ForceLogout", "帳號已在別處登入");
                    //ClearConnection(player, "斷線");
                    // 🔧 改用獨立事件名稱 forceLogout
                    clients.Client(player.ConnectionId).forceLogout(new
                    {
                        message = "帳號已在別處登入"
                    });

                    // 清掉舊的連線資料
                    ClearConnection.Clear(player, "強制登出");    // 讀 ClearConnection.cs 的 Clear 方法

                }

                // 綁定新連線
                player.ConnectionId = connId;
                //PlayerManager.Add(player);  // 確保這時候才加入
                Add(player);
            }

            return (true, "登入成功", player);
        }
        #endregion
    }
}
