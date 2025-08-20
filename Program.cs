using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YSPFrom
{
    internal static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        public static Form1 MainForm { get; private set; }
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();   // 跑出視窗UI
            Application.SetCompatibleTextRenderingDefault(false);   // 設定文字呈現方式
            //Application.Run(new Form1());       // 執行主表單Form1,啟動整個視窗應用程式
            MainForm = new Form1();
            Application.Run(MainForm);
        }


    }
}
