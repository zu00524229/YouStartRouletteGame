using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSPFrom.Core.Logging;
using YSPFrom.Models;

namespace YSPFrom.Hubs.PlayerHub
{
    public static class ClearConnection
    {
        public static void Clear(Player player, string reason)
        {
            if (player == null) return;

            string msg = $"❌ 玩家 {player.UserId} {reason} (ConnId={player.ConnectionId}), 餘額={player.Balance}";
            Console.WriteLine(msg);
            Program.MainForm?.LogPlayerStatus(msg);

            // LogManager：斷線或被踢掉都當作 ClientDisconnected
            LogManager.LotteryLog(LogManager.LotteryLogType.ClientDisconnected, player.UserId, player.Balance); // 玩家登出 log


            PlayerManager.RemoveByConnectionId(player.ConnectionId); // 呼叫方法
            player.ConnectionId = null;

        }
    }
}
