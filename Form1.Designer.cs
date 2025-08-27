using System;
using System.Drawing;
using YSPFrom.Core.Logging;

namespace YSPFrom
{
    partial class Form1
    {
        // 當用戶點擊輸入框時清除預設提示文字
        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "輸入玩家ID或局號")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;  // 改變顏色為黑色
            }
        }

        // 當用戶離開輸入框並且沒有輸入文字時，恢復預設提示文字
        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "輸入玩家ID或局號";
                txtSearch.ForeColor = Color.Gray;  // 恢復灰色提示文字顏色
            }
        }
       
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.TabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.logPlayerBox = new System.Windows.Forms.TextBox();
            this.splitContainer8 = new System.Windows.Forms.SplitContainer();
            this.logPlayerBalance = new System.Windows.Forms.TextBox();
            this.logPlayereffort = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer9 = new System.Windows.Forms.SplitContainer();
            this.logTextBox_Bet = new System.Windows.Forms.TextBox();
            this.panelSearchBar = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.logSearch = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.logTextBox2_Result = new System.Windows.Forms.TextBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.logTextBox_Result = new System.Windows.Forms.TextBox();
            this.logTextBox_Jackpot = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.logTextBox_RTP = new System.Windows.Forms.TextBox();
            this.logTextBox_Base_Right = new System.Windows.Forms.TextBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.txtBalanceLeft = new System.Windows.Forms.TextBox();
            this.txtBalanceRight = new System.Windows.Forms.TextBox();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.txtRoundSummary = new System.Windows.Forms.TextBox();
            this.txtOtherInfo = new System.Windows.Forms.TextBox();
            this.TabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).BeginInit();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer8)).BeginInit();
            this.splitContainer8.Panel1.SuspendLayout();
            this.splitContainer8.Panel2.SuspendLayout();
            this.splitContainer8.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer9)).BeginInit();
            this.splitContainer9.Panel1.SuspendLayout();
            this.splitContainer9.Panel2.SuspendLayout();
            this.splitContainer9.SuspendLayout();
            this.panelSearchBar.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.tabPage1);
            this.TabControl.Controls.Add(this.tabPage2);
            this.TabControl.Controls.Add(this.tabPage3);
            this.TabControl.Controls.Add(this.tabPage4);
            this.TabControl.Controls.Add(this.tabPage5);
            this.TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl.Font = new System.Drawing.Font("新細明體", 10F);
            this.TabControl.Location = new System.Drawing.Point(0, 0);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(983, 450);
            this.TabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer7);
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(975, 423);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "玩家資訊";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.Location = new System.Drawing.Point(3, 3);
            this.splitContainer7.Name = "splitContainer7";
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.logPlayerBox);
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.splitContainer8);
            this.splitContainer7.Size = new System.Drawing.Size(969, 417);
            this.splitContainer7.SplitterDistance = 322;
            this.splitContainer7.TabIndex = 1;
            // 
            // logPlayerBox
            // 
            this.logPlayerBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logPlayerBox.Location = new System.Drawing.Point(0, 0);
            this.logPlayerBox.Multiline = true;
            this.logPlayerBox.Name = "logPlayerBox";
            this.logPlayerBox.ReadOnly = true;
            this.logPlayerBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logPlayerBox.Size = new System.Drawing.Size(322, 417);
            this.logPlayerBox.TabIndex = 1;
            // 
            // splitContainer8
            // 
            this.splitContainer8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer8.Location = new System.Drawing.Point(0, 0);
            this.splitContainer8.Name = "splitContainer8";
            this.splitContainer8.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer8.Panel1
            // 
            this.splitContainer8.Panel1.Controls.Add(this.logPlayerBalance);
            // 
            // splitContainer8.Panel2
            // 
            this.splitContainer8.Panel2.Controls.Add(this.logPlayereffort);
            this.splitContainer8.Size = new System.Drawing.Size(643, 417);
            this.splitContainer8.SplitterDistance = 173;
            this.splitContainer8.TabIndex = 0;
            // 
            // logPlayerBalance
            // 
            this.logPlayerBalance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logPlayerBalance.Location = new System.Drawing.Point(0, 0);
            this.logPlayerBalance.Multiline = true;
            this.logPlayerBalance.Name = "logPlayerBalance";
            this.logPlayerBalance.ReadOnly = true;
            this.logPlayerBalance.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logPlayerBalance.Size = new System.Drawing.Size(643, 173);
            this.logPlayerBalance.TabIndex = 0;
            this.logPlayerBalance.TextChanged += new System.EventHandler(this.logTextBox_Base_TextChanged);
            // 
            // logPlayereffort
            // 
            this.logPlayereffort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logPlayereffort.Location = new System.Drawing.Point(0, 0);
            this.logPlayereffort.Multiline = true;
            this.logPlayereffort.Name = "logPlayereffort";
            this.logPlayereffort.ReadOnly = true;
            this.logPlayereffort.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logPlayereffort.Size = new System.Drawing.Size(643, 240);
            this.logPlayereffort.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer9);
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(975, 423);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "下注紀錄";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer9
            // 
            this.splitContainer9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer9.Location = new System.Drawing.Point(3, 3);
            this.splitContainer9.Name = "splitContainer9";
            // 
            // splitContainer9.Panel1
            // 
            this.splitContainer9.Panel1.Controls.Add(this.logTextBox_Bet);
            // 
            // splitContainer9.Panel2
            // 
            this.splitContainer9.Panel2.Controls.Add(this.panelSearchBar);
            this.splitContainer9.Panel2.Controls.Add(this.logSearch);
            this.splitContainer9.Size = new System.Drawing.Size(969, 417);
            this.splitContainer9.SplitterDistance = 506;
            this.splitContainer9.TabIndex = 4;
            // 
            // logTextBox_Bet
            // 
            this.logTextBox_Bet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox_Bet.Location = new System.Drawing.Point(0, 0);
            this.logTextBox_Bet.Multiline = true;
            this.logTextBox_Bet.Name = "logTextBox_Bet";
            this.logTextBox_Bet.ReadOnly = true;
            this.logTextBox_Bet.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox_Bet.Size = new System.Drawing.Size(506, 417);
            this.logTextBox_Bet.TabIndex = 0;
            // 
            // panelSearchBar
            // 
            this.panelSearchBar.Controls.Add(this.btnSearch);
            this.panelSearchBar.Controls.Add(this.btnClear);
            this.panelSearchBar.Controls.Add(this.txtSearch);
            this.panelSearchBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearchBar.Location = new System.Drawing.Point(0, 0);
            this.panelSearchBar.Name = "panelSearchBar";
            this.panelSearchBar.Size = new System.Drawing.Size(459, 23);
            this.panelSearchBar.TabIndex = 4;
            // 
            // btnSearch
            // 
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Location = new System.Drawing.Point(309, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "搜尋";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClear.Location = new System.Drawing.Point(384, 0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "取消";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.ForeColor = System.Drawing.Color.Gray;
            this.txtSearch.Location = new System.Drawing.Point(0, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(459, 23);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.Text = "輸入玩家ID或局號";
            this.txtSearch.Enter += new System.EventHandler(this.txtSearch_Enter);
            this.txtSearch.Leave += new System.EventHandler(this.txtSearch_Leave);
            // 
            // logSearch
            // 
            this.logSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logSearch.Location = new System.Drawing.Point(0, 0);
            this.logSearch.Multiline = true;
            this.logSearch.Name = "logSearch";
            this.logSearch.ReadOnly = true;
            this.logSearch.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logSearch.Size = new System.Drawing.Size(459, 417);
            this.logSearch.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.splitContainer2);
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(975, 423);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "中獎紀錄";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.logTextBox2_Result);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(975, 423);
            this.splitContainer2.SplitterDistance = 205;
            this.splitContainer2.TabIndex = 1;
            // 
            // logTextBox2_Result
            // 
            this.logTextBox2_Result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox2_Result.Font = new System.Drawing.Font("新細明體", 10F);
            this.logTextBox2_Result.Location = new System.Drawing.Point(0, 0);
            this.logTextBox2_Result.Multiline = true;
            this.logTextBox2_Result.Name = "logTextBox2_Result";
            this.logTextBox2_Result.ReadOnly = true;
            this.logTextBox2_Result.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox2_Result.Size = new System.Drawing.Size(975, 205);
            this.logTextBox2_Result.TabIndex = 1;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.logTextBox_Result);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.logTextBox_Jackpot);
            this.splitContainer3.Size = new System.Drawing.Size(975, 214);
            this.splitContainer3.SplitterDistance = 474;
            this.splitContainer3.TabIndex = 2;
            // 
            // logTextBox_Result
            // 
            this.logTextBox_Result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox_Result.Font = new System.Drawing.Font("新細明體", 10F);
            this.logTextBox_Result.Location = new System.Drawing.Point(0, 0);
            this.logTextBox_Result.Multiline = true;
            this.logTextBox_Result.Name = "logTextBox_Result";
            this.logTextBox_Result.ReadOnly = true;
            this.logTextBox_Result.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox_Result.Size = new System.Drawing.Size(474, 214);
            this.logTextBox_Result.TabIndex = 0;
            // 
            // logTextBox_Jackpot
            // 
            this.logTextBox_Jackpot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox_Jackpot.Font = new System.Drawing.Font("新細明體", 10F);
            this.logTextBox_Jackpot.Location = new System.Drawing.Point(0, 0);
            this.logTextBox_Jackpot.Multiline = true;
            this.logTextBox_Jackpot.Name = "logTextBox_Jackpot";
            this.logTextBox_Jackpot.ReadOnly = true;
            this.logTextBox_Jackpot.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox_Jackpot.Size = new System.Drawing.Size(497, 214);
            this.logTextBox_Jackpot.TabIndex = 1;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.splitContainer1);
            this.tabPage4.Location = new System.Drawing.Point(4, 23);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(975, 423);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "RTP狀態";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.logTextBox_RTP);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.logTextBox_Base_Right);
            this.splitContainer1.Size = new System.Drawing.Size(975, 423);
            this.splitContainer1.SplitterDistance = 466;
            this.splitContainer1.TabIndex = 1;
            // 
            // logTextBox_RTP
            // 
            this.logTextBox_RTP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox_RTP.Font = new System.Drawing.Font("新細明體", 10F);
            this.logTextBox_RTP.Location = new System.Drawing.Point(0, 0);
            this.logTextBox_RTP.Multiline = true;
            this.logTextBox_RTP.Name = "logTextBox_RTP";
            this.logTextBox_RTP.ReadOnly = true;
            this.logTextBox_RTP.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox_RTP.Size = new System.Drawing.Size(466, 423);
            this.logTextBox_RTP.TabIndex = 0;
            // 
            // logTextBox_Base_Right
            // 
            this.logTextBox_Base_Right.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox_Base_Right.Font = new System.Drawing.Font("新細明體", 10F);
            this.logTextBox_Base_Right.Location = new System.Drawing.Point(0, 0);
            this.logTextBox_Base_Right.Multiline = true;
            this.logTextBox_Base_Right.Name = "logTextBox_Base_Right";
            this.logTextBox_Base_Right.ReadOnly = true;
            this.logTextBox_Base_Right.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox_Base_Right.Size = new System.Drawing.Size(505, 423);
            this.logTextBox_Base_Right.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.splitContainer4);
            this.tabPage5.Font = new System.Drawing.Font("新細明體", 12F);
            this.tabPage5.Location = new System.Drawing.Point(4, 23);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(975, 423);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "金流紀錄";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.splitContainer6);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.splitContainer5);
            this.splitContainer4.Size = new System.Drawing.Size(975, 423);
            this.splitContainer4.SplitterDistance = 157;
            this.splitContainer4.TabIndex = 0;
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.Location = new System.Drawing.Point(0, 0);
            this.splitContainer6.Name = "splitContainer6";
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.txtBalanceLeft);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.txtBalanceRight);
            this.splitContainer6.Size = new System.Drawing.Size(975, 157);
            this.splitContainer6.SplitterDistance = 460;
            this.splitContainer6.TabIndex = 0;
            // 
            // txtBalanceLeft
            // 
            this.txtBalanceLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBalanceLeft.Font = new System.Drawing.Font("新細明體", 11F);
            this.txtBalanceLeft.Location = new System.Drawing.Point(0, 0);
            this.txtBalanceLeft.Multiline = true;
            this.txtBalanceLeft.Name = "txtBalanceLeft";
            this.txtBalanceLeft.ReadOnly = true;
            this.txtBalanceLeft.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBalanceLeft.Size = new System.Drawing.Size(460, 157);
            this.txtBalanceLeft.TabIndex = 1;
            // 
            // txtBalanceRight
            // 
            this.txtBalanceRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBalanceRight.Font = new System.Drawing.Font("新細明體", 11F);
            this.txtBalanceRight.Location = new System.Drawing.Point(0, 0);
            this.txtBalanceRight.Multiline = true;
            this.txtBalanceRight.Name = "txtBalanceRight";
            this.txtBalanceRight.ReadOnly = true;
            this.txtBalanceRight.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBalanceRight.Size = new System.Drawing.Size(511, 157);
            this.txtBalanceRight.TabIndex = 1;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.txtRoundSummary);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.txtOtherInfo);
            this.splitContainer5.Size = new System.Drawing.Size(975, 262);
            this.splitContainer5.SplitterDistance = 109;
            this.splitContainer5.TabIndex = 0;
            // 
            // txtRoundSummary
            // 
            this.txtRoundSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRoundSummary.Font = new System.Drawing.Font("新細明體", 11F);
            this.txtRoundSummary.Location = new System.Drawing.Point(0, 0);
            this.txtRoundSummary.Multiline = true;
            this.txtRoundSummary.Name = "txtRoundSummary";
            this.txtRoundSummary.ReadOnly = true;
            this.txtRoundSummary.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRoundSummary.Size = new System.Drawing.Size(975, 109);
            this.txtRoundSummary.TabIndex = 1;
            // 
            // txtOtherInfo
            // 
            this.txtOtherInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOtherInfo.Font = new System.Drawing.Font("新細明體", 11F);
            this.txtOtherInfo.Location = new System.Drawing.Point(0, 0);
            this.txtOtherInfo.Multiline = true;
            this.txtOtherInfo.Name = "txtOtherInfo";
            this.txtOtherInfo.ReadOnly = true;
            this.txtOtherInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOtherInfo.Size = new System.Drawing.Size(975, 149);
            this.txtOtherInfo.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(983, 450);
            this.Controls.Add(this.TabControl);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.TabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel1.PerformLayout();
            this.splitContainer7.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).EndInit();
            this.splitContainer7.ResumeLayout(false);
            this.splitContainer8.Panel1.ResumeLayout(false);
            this.splitContainer8.Panel1.PerformLayout();
            this.splitContainer8.Panel2.ResumeLayout(false);
            this.splitContainer8.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer8)).EndInit();
            this.splitContainer8.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer9.Panel1.ResumeLayout(false);
            this.splitContainer9.Panel1.PerformLayout();
            this.splitContainer9.Panel2.ResumeLayout(false);
            this.splitContainer9.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer9)).EndInit();
            this.splitContainer9.ResumeLayout(false);
            this.panelSearchBar.ResumeLayout(false);
            this.panelSearchBar.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel1.PerformLayout();
            this.splitContainer6.Panel2.ResumeLayout(false);
            this.splitContainer6.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel1.PerformLayout();
            this.splitContainer5.Panel2.ResumeLayout(false);
            this.splitContainer5.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox logPlayerBalance;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox logTextBox_Bet;
        private System.Windows.Forms.TextBox logTextBox_Result;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox logTextBox_RTP;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox logTextBox_Base_Right;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox logTextBox2_Result;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private System.Windows.Forms.TextBox txtBalanceLeft;
        private System.Windows.Forms.TextBox txtBalanceRight;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.TextBox txtRoundSummary;
        private System.Windows.Forms.TextBox txtOtherInfo;
        private System.Windows.Forms.SplitContainer splitContainer7;
        private System.Windows.Forms.SplitContainer splitContainer8;
        private System.Windows.Forms.TextBox logPlayerBox;
        private System.Windows.Forms.TextBox logPlayereffort;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.SplitContainer splitContainer9;
        private System.Windows.Forms.TextBox logSearch;
        private System.Windows.Forms.Panel panelSearchBar;
        private System.Windows.Forms.TextBox logTextBox_Jackpot;
    }
}

