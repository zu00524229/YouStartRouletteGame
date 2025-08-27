using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YSPFrom.Core.Logging;
using YSPFrom.Core.RTP;
using YSPFrom.Models;


namespace YSPFrom
{
    public partial class Form1 : Form
    {
        private IDisposable _server;

        public Form1()
        {
            InitializeComponent();
            StartSignalRServer();
        }

        private void StartSignalRServer()
        {
            string url = "http://172.16.5.21:5000";     // 區網連線 IP
            _server = WebApp.Start<Startup>(url);
            this.Text = $"🎯 SignalR Server running at {url}";
        }
        #region 玩家資訊
        public void LogPlayerStatus(string message) // 登入/登出/連線
        {
            AppendTextSafe(logPlayerBox, message);
        }

        public void LogPlayerRoundBalance(string message) // 每局 / 登出餘額 都可以丟這裡
        {
            AppendTextSafe(logPlayerBalance, message);
        }
        public void LogPlayereffort(string message) // 列印 成就系統
        {
            AppendTextSafe(logPlayereffort, message);
        }
        #endregion

        #region 金流紀錄
       
        public void LogBalanceLeft(string msg) // 上半部左：抽獎前 & 扣注後
        {
            AppendTextSafe(txtBalanceLeft, msg);
        }

        
        public void LogBalanceRight(string msg) // 上半部右：派彩後
        {
            AppendTextSafe(txtBalanceRight, msg);
        }

        // 中間：局號、獎項、下注額、倍率、派彩
        public void LogRoundSummary(string msg)
        {
            Console.WriteLine($"[DEBUG] LogRoundSummary 呼叫成功, msg={msg}");
            AppendTextSafe(txtRoundSummary, msg);
        }


        // 下半部：額外訊息
        public void LogOtherInfo(string msg)
        {
            AppendTextSafe(txtOtherInfo, msg);
        }
        #endregion

        #region 中獎結果
        public void LogSummary(string msg)    // 列印中獎結果(上分頁)
        {
            AppendTextSafe(logTextBox2_Result, msg);
        }

        public void LogWin(string message)   // 列印中獎結果(左分頁)
        {
            AppendTextSafe(logTextBox_Result, message);
        }

        public void LogJackpotHit(string msg) // 右下大獎命中
        {
            AppendTextSafe(logTextBox_Jackpot, "[命中] " + msg);
        }

        public void LogJackpotPool(string msg) // 右下獎池狀態
        {
            AppendTextSafe(logTextBox_Jackpot, "[獎池] " + msg);
        }
        #endregion

        #region RTP狀態
        public void LogRTP(string msg)   // 列印 RTP 資訊(左分頁)
        {
            AppendTextSafe(logTextBox_RTP, msg);
        }

        public void LogRTPhistory(string message)    // // 列印歷史紀錄與總 RTP 資訊(右分頁)
        {
            AppendTextSafe(logTextBox_Base_Right, message);
        }
        #endregion

        #region 下注紀錄
        public void LogBet(string message)   // 下注資料
        {
            // 判斷是否在搜尋模式下
            if (isSearching)
            {
                // 如果正在搜尋且資料包含搜尋關鍵字
                if (message.Contains(currentSearchKeyword))
                {
                    AppendTextSafe(logTextBox_Bet, message);
                    LogSearchManager.BatLogs.Add(message); // 儲存到 BatLogs 列表
                    LogSearchManager.allLogs.Add(message); // 同時儲存到 allLogs 列表
                }
            } else
            {
                // 如果不是搜尋模式，顯示所有資料
                AppendTextSafe(logTextBox_Bet, message);
                LogSearchManager.BatLogs.Add(message); // 儲存到 BatLogs 列表
                LogSearchManager.allLogs.Add(message); // 同時儲存到 allLogs 列表
            }
        }

        #region  Batlog 搜尋功能
        private bool isSearching = false;  // 是否正在搜尋
        private string currentSearchKeyword = "";  // 當前搜尋的關鍵字

        private void btnSearch_Click(object sender, EventArgs e)    // 搜尋按鈕點擊事件
        {
            string keyword = txtSearch.Text.Trim();  // 取得搜尋的關鍵字
            if (string.IsNullOrEmpty(keyword))
            {
                // 如果關鍵字是空的，顯示所有資料
                logTextBox_Bet.Lines = LogSearchManager.BatLogs.ToArray();
                logSearch.Clear();  // 清空搜尋結果顯示框
                isSearching = false;
                currentSearchKeyword = "";
            }
            else
            {
                // 設置搜尋模式，並且只顯示與關鍵字匹配的資料
                currentSearchKeyword = keyword;
                isSearching = true;

                // 搜尋 BatLogs，並顯示結果
                var results = LogSearchManager.SearchLogs(LogSearchManager.BatLogs, keyword);
                if (results.Count == 0)
                {
                    //txtSearch.Text = "查無資料"; // 顯示查無資料
                    logSearch.Lines = new string[] { "查無資料" };  // 如果沒找到，顯示查無資料
                }
                else
                {
                    //txtSearch.Text = ""; // 清空搜尋結果區域
                    logSearch.Lines = results.ToArray();  // 顯示符合條件的結果
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)  // 取消搜尋按鈕點擊事件
        {
            txtSearch.Text = "";  // 清空搜尋框
            logSearch.Clear();  // 清空搜尋結果顯示框
            // 恢復顯示所有資料
            logTextBox_Bet.Lines = LogSearchManager.BatLogs.ToArray();
            // 重製搜尋狀態
            isSearching = false;
            currentSearchKeyword = "";
        }
        #endregion
        #endregion

        private void AppendTextSafe(TextBox box, string message)
        {
            if (box.InvokeRequired)
            {
                box.Invoke(new Action(() =>
                {
                    box.AppendText($"{DateTime.Now:HH:mm:ss} {message}\r\n");
                }));
            }
            else
            {
                box.AppendText($"{DateTime.Now:HH:mm:ss} {message}\r\n");
            }
        }

        private void logTextBox_Base_TextChanged(object sender, EventArgs e)
        {
            // 可留空或移除
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // 你的初始化邏輯

            // 啟動後直接模擬 1000 次（測試用）
            //LotterySimulator.RunTestMode(msg => LogBase(msg), times: 5000);
            #region 背景作業
            //Task.Run(() =>
            //{
            //    var buffer = new StringBuilder();

            //    LotterySimulator.RunTestMode(msg =>
            //        {
            //            buffer.AppendLine(msg);

            //            if (RTPManager.lifetimeSpinCount % 1000 == 0)
            //            {
            //                string logs = buffer.ToString();
            //                buffer.Clear();
            //                this.Invoke((Action)(() => LogPlayereffort(logs)));
            //            }


            //        }, times: 50000);

            //    // 模擬完成後輸出剩餘的 log
            //    if (buffer.Length > 0)
            //    {
            //        this.Invoke((Action)(() => LogPlayereffort(buffer.ToString())));
            //    }
            //});
            #endregion
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer4_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
