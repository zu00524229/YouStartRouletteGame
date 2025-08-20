using System.Collections.Concurrent;
using System.Collections.Generic;
using YSPFrom.Models;

namespace YSPFrom
{
    public static class PlayerManager
    {
        // key: ConnectionId
        private static readonly ConcurrentDictionary<string, Player> _players = new ConcurrentDictionary<string, Player>();

        //public static void Add(Player player)
        //{
        //    _players[player.ConnectionId] = player;
        //}

        public static Player GetByConnectionId(string connectionId)
        {
            _players.TryGetValue(connectionId, out var player);
            return player;
        }

        public static void RemoveByConnectionId(string connectionId)
        {
            _players.TryRemove(connectionId, out _);
        }

        public static IEnumerable<Player> GetAll()
        {
            return _players.Values;
        }
    }
}
