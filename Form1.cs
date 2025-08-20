using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            string url = "http://localhost:5000";
            _server = WebApp.Start<Startup>(url);
            this.Text = $"🎯 SignalR Server running at {url}";
        }
        #region 基本
        public void LogBase(string message) // 列印 基本Log(後台接收)
        {
            AppendTextSafe(logTextBox_Base, message);
        }
        #endregion

        #region 下注資料
        public void LogBet(string message)   // 列印下注資料(後台接收)
        {
            AppendTextSafe(logTextBox_Bet, message);
        }
        #endregion

        #region 中獎結果
        public void LogResult(string message)   // 列印中獎結果(左分頁)
        {
            AppendTextSafe(logTextBox_Result, message);
        }

        public void LogBigResult(string msg)    // 列印重製前中獎結果與RTP(中分頁)
        {
            AppendTextSafe(logTextBox2_Result, msg);
        }

        public void LogJackpot(string message)  // 列印大獎事件(右分頁)
        {
            AppendTextSafe(logTextBox_Jackpot, message);
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

        #region 金流 Log
       
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
            AppendTextSafe(txtRoundSummary, msg);
        }

        // 下半部：額外訊息
        public void LogOtherInfo(string msg)
        {
            AppendTextSafe(txtOtherInfo, msg);
        }
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
            //                this.Invoke((Action)(() => LogBase(logs)));
            //            }


            //        }, times: 50000);

            //    // 模擬完成後輸出剩餘的 log
            //    if (buffer.Length > 0)
            //    {
            //        this.Invoke((Action)(() => LogBase(buffer.ToString())));
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
