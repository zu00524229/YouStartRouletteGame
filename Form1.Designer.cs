namespace YSPFrom
{
    partial class Form1
    {
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
            this.logTextBox_Base = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.logTextBox_Bet = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.logTextBox_Result = new System.Windows.Forms.TextBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.logTextBox2_Result = new System.Windows.Forms.TextBox();
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
            this.tabPage2.SuspendLayout();
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
            this.TabControl.Size = new System.Drawing.Size(800, 450);
            this.TabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.logTextBox_Base);
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 423);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本Log";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // logTextBox_Base
            // 
            this.logTextBox_Base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox_Base.Location = new System.Drawing.Point(3, 3);
            this.logTextBox_Base.Multiline = true;
            this.logTextBox_Base.Name = "logTextBox_Base";
            this.logTextBox_Base.ReadOnly = true;
            this.logTextBox_Base.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox_Base.Size = new System.Drawing.Size(786, 417);
            this.logTextBox_Base.TabIndex = 0;
            this.logTextBox_Base.TextChanged += new System.EventHandler(this.logTextBox_Base_TextChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.logTextBox_Bet);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(792, 424);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "下注資料";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // logTextBox_Bet
            // 
            this.logTextBox_Bet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox_Bet.Location = new System.Drawing.Point(3, 3);
            this.logTextBox_Bet.Multiline = true;
            this.logTextBox_Bet.Name = "logTextBox_Bet";
            this.logTextBox_Bet.ReadOnly = true;
            this.logTextBox_Bet.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox_Bet.Size = new System.Drawing.Size(786, 418);
            this.logTextBox_Bet.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.splitContainer2);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(792, 424);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "中獎結果";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.logTextBox_Result);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(792, 424);
            this.splitContainer2.SplitterDistance = 264;
            this.splitContainer2.TabIndex = 1;
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
            this.logTextBox_Result.Size = new System.Drawing.Size(264, 424);
            this.logTextBox_Result.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.logTextBox2_Result);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.logTextBox_Jackpot);
            this.splitContainer3.Size = new System.Drawing.Size(524, 424);
            this.splitContainer3.SplitterDistance = 174;
            this.splitContainer3.TabIndex = 2;
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
            this.logTextBox2_Result.Size = new System.Drawing.Size(174, 424);
            this.logTextBox2_Result.TabIndex = 1;
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
            this.logTextBox_Jackpot.Size = new System.Drawing.Size(346, 424);
            this.logTextBox_Jackpot.TabIndex = 2;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.splitContainer1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(792, 424);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "RTP機率狀態";
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
            this.splitContainer1.Size = new System.Drawing.Size(792, 424);
            this.splitContainer1.SplitterDistance = 380;
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
            this.logTextBox_RTP.Size = new System.Drawing.Size(380, 424);
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
            this.logTextBox_Base_Right.Size = new System.Drawing.Size(408, 424);
            this.logTextBox_Base_Right.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.splitContainer4);
            this.tabPage5.Font = new System.Drawing.Font("新細明體", 12F);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(792, 424);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "計算餘額(Balance)";
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
            this.splitContainer4.Size = new System.Drawing.Size(792, 424);
            this.splitContainer4.SplitterDistance = 158;
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
            this.splitContainer6.Size = new System.Drawing.Size(792, 158);
            this.splitContainer6.SplitterDistance = 264;
            this.splitContainer6.TabIndex = 0;
            // 
            // txtBalanceLeft
            // 
            this.txtBalanceLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBalanceLeft.Location = new System.Drawing.Point(0, 0);
            this.txtBalanceLeft.Multiline = true;
            this.txtBalanceLeft.Name = "txtBalanceLeft";
            this.txtBalanceLeft.ReadOnly = true;
            this.txtBalanceLeft.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBalanceLeft.Size = new System.Drawing.Size(264, 158);
            this.txtBalanceLeft.TabIndex = 1;
            // 
            // txtBalanceRight
            // 
            this.txtBalanceRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBalanceRight.Location = new System.Drawing.Point(0, 0);
            this.txtBalanceRight.Multiline = true;
            this.txtBalanceRight.Name = "txtBalanceRight";
            this.txtBalanceRight.ReadOnly = true;
            this.txtBalanceRight.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBalanceRight.Size = new System.Drawing.Size(524, 158);
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
            this.splitContainer5.Size = new System.Drawing.Size(792, 262);
            this.splitContainer5.SplitterDistance = 109;
            this.splitContainer5.TabIndex = 0;
            // 
            // txtRoundSummary
            // 
            this.txtRoundSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRoundSummary.Location = new System.Drawing.Point(0, 0);
            this.txtRoundSummary.Multiline = true;
            this.txtRoundSummary.Name = "txtRoundSummary";
            this.txtRoundSummary.ReadOnly = true;
            this.txtRoundSummary.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRoundSummary.Size = new System.Drawing.Size(792, 109);
            this.txtRoundSummary.TabIndex = 1;
            // 
            // txtOtherInfo
            // 
            this.txtOtherInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOtherInfo.Location = new System.Drawing.Point(0, 0);
            this.txtOtherInfo.Multiline = true;
            this.txtOtherInfo.Name = "txtOtherInfo";
            this.txtOtherInfo.ReadOnly = true;
            this.txtOtherInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOtherInfo.Size = new System.Drawing.Size(792, 149);
            this.txtOtherInfo.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TabControl);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.TabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
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
        private System.Windows.Forms.TextBox logTextBox_Base;
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
        private System.Windows.Forms.TextBox logTextBox_Jackpot;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private System.Windows.Forms.TextBox txtBalanceLeft;
        private System.Windows.Forms.TextBox txtBalanceRight;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.TextBox txtRoundSummary;
        private System.Windows.Forms.TextBox txtOtherInfo;
    }
}

