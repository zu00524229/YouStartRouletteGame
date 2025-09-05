using System;

namespace YSPFrom.Core.Utils
{
    public static class RoundIdGenerator
    {
        private static readonly object _lock = new object();
        private static long _lastTimestamp = -1L;
        private static long _sequence = 0L;
        private static readonly long MachineId = 1; // 改成每台伺服器的唯一 ID
        private const long Twepoch = 1609459200000L; // 2021-01-01 00:00:00 UTC

        public static string NextIdString()
        {
            return NextId().ToString();
        }

        public static long NextId()
        {
            lock (_lock)
            {
                long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                if (timestamp == _lastTimestamp)
                {
                    _sequence = (_sequence + 1) & 4095; // 12 bits 序列號
                    if (_sequence == 0)
                    {
                        while (timestamp <= _lastTimestamp)
                        {
                            timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        }
                    }
                }
                else
                {
                    _sequence = 0;
                }

                _lastTimestamp = timestamp;

                // 41 bits 時間 + 10 bits 機器 ID + 12 bits 序列號
                return ((timestamp - Twepoch) << 22) | (MachineId << 12) | _sequence;
            }
        }
    }
}
