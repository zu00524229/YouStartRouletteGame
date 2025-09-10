using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using YSPFrom.Hubs.PlayerHub;
using System.Threading.Tasks;
using YSPFrom.Models;

namespace YSPFrom.Hubs
{
    internal class HeartbeatManager
    {
        // 紀錄玩家最後一次心跳時間
        private static readonly Dictionary<string, DateTime> _lastHeartbeat = new Dictionary<string, DateTime>();

        public static void UpdateHeartbeat(string userId)
        {
            _lastHeartbeat[userId] = DateTime.UtcNow;
        }

        // 定期掃描，清理超時玩家
        public static void StartMonitor(Func<IEnumerable<Player>> getPlayers)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    var now = DateTime.UtcNow;
                    foreach (var kv in _lastHeartbeat.ToList())
                    {
                        if ((now - kv.Value).TotalSeconds > 15)
                        {
                            var player = getPlayers().FirstOrDefault(p => p.UserId == kv.Key);
                            if (player != null)
                            {
                                string msg = $"⚠️ 玩家 {player.UserId} 心跳逾時，判定斷線";
                                Console.WriteLine(msg);
                                Program.MainForm?.LogConnectionState(msg);  // ← 顯示到左邊 logState

                                ClearConnection.Clear(player, "心跳逾時斷線");

                                //player.ConnectionId = null;


                            }
                            _lastHeartbeat.Remove(kv.Key);
                        }
                    }
                    await Task.Delay(5000);
                }
            });
        }
    }
}
