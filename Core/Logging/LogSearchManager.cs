using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSPFrom.Core.Logging
{
    public class LogSearchManager
    {
        // 儲存所有 log 的列表
        public static List<string> allLogs = new List<string>();


        #region 儲存所有 Batlog 的列表
        public static List<string> BatLogs = new List<string>();

        // 設定 BatLogs
        public static void SetBatLogs(List<string> logs)
        {
            BatLogs = logs;
        }
        #endregion

        // 搜尋方法
        public static List<string> SearchLogs(List<string> logs, string keyword)
        {
            return logs.Where(log => log.Contains(keyword)).ToList();
        }
    }
}
