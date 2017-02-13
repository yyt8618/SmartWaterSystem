namespace AboutTime
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnExist = new System.Windows.Forms.Button();
            this.btnHide = new System.Windows.Forms.Button();
            this.btnSetTime = new System.Windows.Forms.Button();
            this.lstLog = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnDownHost = new System.Windows.Forms.Button();
            this.btnUpHost = new System.Windows.Forms.Button();
            this.btnEditHost = new System.Windows.Forms.Button();
            this.btnDelHost = new System.Windows.Forms.Button();
            this.btnAddHost = new System.Windows.Forms.Button();
            this.lstHosts = new System.Windows.Forms.ListBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.cbService = new System.Windows.Forms.CheckBox();
            this.cbAutoStart = new System.Windows.Forms.CheckBox();
            this.cbAutoHide = new System.Windows.Forms.CheckBox();
            this.cbAllHosts = new System.Windows.Forms.CheckBox();
            this.cbAutoTiming = new System.Windows.Forms.CheckBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.timer_interval = new System.Windows.Forms.Timer(this.components);
            this.ledClock1 = new Common.LEDClock();
            this.txtServiceIP = new System.Windows.Forms.TextBox();
            this.txtServicePort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ledClock1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(0, 40);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(419, 194);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnExist);
            this.tabPage1.Controls.Add(this.btnHide);
            this.tabPage1.Controls.Add(this.btnSetTime);
            this.tabPage1.Controls.Add(this.lstLog);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(411, 168);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "设置时间";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnExist
            // 
            this.btnExist.Location = new System.Drawing.Point(262, 136);
            this.btnExist.Name = "btnExist";
            this.btnExist.Size = new System.Drawing.Size(75, 23);
            this.btnExist.TabIndex = 3;
            this.btnExist.Text = "退出";
            this.btnExist.UseVisualStyleBackColor = true;
            this.btnExist.Click += new System.EventHandler(this.btnExist_Click);
            // 
            // btnHide
            // 
            this.btnHide.Location = new System.Drawing.Point(152, 136);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(75, 23);
            this.btnHide.TabIndex = 2;
            this.btnHide.Text = "隐藏";
            this.btnHide.UseVisualStyleBackColor = true;
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // btnSetTime
            // 
            this.btnSetTime.Location = new System.Drawing.Point(42, 136);
            this.btnSetTime.Name = "btnSetTime";
            this.btnSetTime.Size = new System.Drawing.Size(75, 23);
            this.btnSetTime.TabIndex = 1;
            this.btnSetTime.Text = "设置时间";
            this.btnSetTime.UseVisualStyleBackColor = true;
            this.btnSetTime.Click += new System.EventHandler(this.btnSetTime_Click);
            // 
            // lstLog
            // 
            this.lstLog.FormattingEnabled = true;
            this.lstLog.ItemHeight = 12;
            this.lstLog.Location = new System.Drawing.Point(0, 4);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(411, 124);
            this.lstLog.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnDownHost);
            this.tabPage2.Controls.Add(this.btnUpHost);
            this.tabPage2.Controls.Add(this.btnEditHost);
            this.tabPage2.Controls.Add(this.btnDelHost);
            this.tabPage2.Controls.Add(this.btnAddHost);
            this.tabPage2.Controls.Add(this.lstHosts);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(411, 168);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "服务主机";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnDownHost
            // 
            this.btnDownHost.Location = new System.Drawing.Point(305, 133);
            this.btnDownHost.Name = "btnDownHost";
            this.btnDownHost.Size = new System.Drawing.Size(75, 23);
            this.btnDownHost.TabIndex = 5;
            this.btnDownHost.Text = "下移";
            this.btnDownHost.UseVisualStyleBackColor = true;
            this.btnDownHost.Click += new System.EventHandler(this.btnDownHost_Click);
            // 
            // btnUpHost
            // 
            this.btnUpHost.Location = new System.Drawing.Point(305, 104);
            this.btnUpHost.Name = "btnUpHost";
            this.btnUpHost.Size = new System.Drawing.Size(75, 23);
            this.btnUpHost.TabIndex = 4;
            this.btnUpHost.Text = "上移";
            this.btnUpHost.UseVisualStyleBackColor = true;
            this.btnUpHost.Click += new System.EventHandler(this.btnUpHost_Click);
            // 
            // btnEditHost
            // 
            this.btnEditHost.Location = new System.Drawing.Point(305, 75);
            this.btnEditHost.Name = "btnEditHost";
            this.btnEditHost.Size = new System.Drawing.Size(75, 23);
            this.btnEditHost.TabIndex = 3;
            this.btnEditHost.Text = "编辑";
            this.btnEditHost.UseVisualStyleBackColor = true;
            this.btnEditHost.Click += new System.EventHandler(this.btnEditHost_Click);
            // 
            // btnDelHost
            // 
            this.btnDelHost.Location = new System.Drawing.Point(305, 46);
            this.btnDelHost.Name = "btnDelHost";
            this.btnDelHost.Size = new System.Drawing.Size(75, 23);
            this.btnDelHost.TabIndex = 2;
            this.btnDelHost.Text = "删除";
            this.btnDelHost.UseVisualStyleBackColor = true;
            this.btnDelHost.Click += new System.EventHandler(this.btnDelHost_Click);
            // 
            // btnAddHost
            // 
            this.btnAddHost.Location = new System.Drawing.Point(305, 17);
            this.btnAddHost.Name = "btnAddHost";
            this.btnAddHost.Size = new System.Drawing.Size(75, 23);
            this.btnAddHost.TabIndex = 1;
            this.btnAddHost.Text = "添加";
            this.btnAddHost.UseVisualStyleBackColor = true;
            this.btnAddHost.Click += new System.EventHandler(this.btnAddHost_Click);
            // 
            // lstHosts
            // 
            this.lstHosts.FormattingEnabled = true;
            this.lstHosts.ItemHeight = 12;
            this.lstHosts.Location = new System.Drawing.Point(4, 4);
            this.lstHosts.Name = "lstHosts";
            this.lstHosts.Size = new System.Drawing.Size(282, 160);
            this.lstHosts.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.txtServicePort);
            this.tabPage3.Controls.Add(this.txtServiceIP);
            this.tabPage3.Controls.Add(this.txtInterval);
            this.tabPage3.Controls.Add(this.cbService);
            this.tabPage3.Controls.Add(this.cbAutoStart);
            this.tabPage3.Controls.Add(this.cbAutoHide);
            this.tabPage3.Controls.Add(this.cbAllHosts);
            this.tabPage3.Controls.Add(this.cbAutoTiming);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(411, 168);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "选项";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(125, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "分钟";
            // 
            // txtInterval
            // 
            this.txtInterval.Location = new System.Drawing.Point(76, 3);
            this.txtInterval.MaxLength = 4;
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(41, 21);
            this.txtInterval.TabIndex = 1;
            this.txtInterval.Text = "60";
            this.txtInterval.TextChanged += new System.EventHandler(this.txtInterval_TextChanged);
            // 
            // cbService
            // 
            this.cbService.AutoSize = true;
            this.cbService.Location = new System.Drawing.Point(8, 94);
            this.cbService.Name = "cbService";
            this.cbService.Size = new System.Drawing.Size(72, 16);
            this.cbService.TabIndex = 6;
            this.cbService.Text = "提供服务";
            this.cbService.UseVisualStyleBackColor = true;
            this.cbService.CheckedChanged += new System.EventHandler(this.cbService_CheckedChanged);
            // 
            // cbAutoStart
            // 
            this.cbAutoStart.AutoSize = true;
            this.cbAutoStart.Location = new System.Drawing.Point(8, 72);
            this.cbAutoStart.Name = "cbAutoStart";
            this.cbAutoStart.Size = new System.Drawing.Size(84, 16);
            this.cbAutoStart.TabIndex = 5;
            this.cbAutoStart.Text = "随系统启动";
            this.cbAutoStart.UseVisualStyleBackColor = true;
            this.cbAutoStart.CheckedChanged += new System.EventHandler(this.cbAutoStart_CheckedChanged);
            // 
            // cbAutoHide
            // 
            this.cbAutoHide.AutoSize = true;
            this.cbAutoHide.Location = new System.Drawing.Point(8, 50);
            this.cbAutoHide.Name = "cbAutoHide";
            this.cbAutoHide.Size = new System.Drawing.Size(84, 16);
            this.cbAutoHide.TabIndex = 4;
            this.cbAutoHide.Text = "启动后隐藏";
            this.cbAutoHide.UseVisualStyleBackColor = true;
            this.cbAutoHide.CheckedChanged += new System.EventHandler(this.cbAutoHide_CheckedChanged);
            // 
            // cbAllHosts
            // 
            this.cbAllHosts.AutoSize = true;
            this.cbAllHosts.Location = new System.Drawing.Point(8, 28);
            this.cbAllHosts.Name = "cbAllHosts";
            this.cbAllHosts.Size = new System.Drawing.Size(96, 16);
            this.cbAllHosts.TabIndex = 3;
            this.cbAllHosts.Text = "尝试所有主机";
            this.cbAllHosts.UseVisualStyleBackColor = true;
            this.cbAllHosts.CheckedChanged += new System.EventHandler(this.cbAllHosts_CheckedChanged);
            // 
            // cbAutoTiming
            // 
            this.cbAutoTiming.AutoSize = true;
            this.cbAutoTiming.Location = new System.Drawing.Point(8, 6);
            this.cbAutoTiming.Name = "cbAutoTiming";
            this.cbAutoTiming.Size = new System.Drawing.Size(72, 16);
            this.cbAutoTiming.TabIndex = 0;
            this.cbAutoTiming.Text = "自动校时";
            this.cbAutoTiming.UseVisualStyleBackColor = true;
            this.cbAutoTiming.CheckedChanged += new System.EventHandler(this.cbAutoTiming_CheckedChanged);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "恢复校时界面";
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // timer_interval
            // 
            this.timer_interval.Interval = 300000;
            this.timer_interval.Tick += new System.EventHandler(this.timer_interval_Tick);
            // 
            // ledClock1
            // 
            this.ledClock1.BackColor = System.Drawing.Color.Black;
            this.ledClock1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ledClock1.BackgroundImage")));
            this.ledClock1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
            this.ledClock1.BrightColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(1)))));
            this.ledClock1.DarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(0)))));
            this.ledClock1.ErrorImage = ((System.Drawing.Image)(resources.GetObject("ledClock1.ErrorImage")));
            this.ledClock1.InitialImage = ((System.Drawing.Image)(resources.GetObject("ledClock1.InitialImage")));
            this.ledClock1.Interval = 1000;
            this.ledClock1.Location = new System.Drawing.Point(0, -1);
            this.ledClock1.Name = "ledClock1";
            this.ledClock1.Size = new System.Drawing.Size(419, 39);
            this.ledClock1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal;
            this.ledClock1.TabIndex = 0;
            this.ledClock1.TabStop = false;
            this.ledClock1.Type = Common.TimeType.年月日时分秒;
            // 
            // txtServiceIP
            // 
            this.txtServiceIP.Location = new System.Drawing.Point(83, 92);
            this.txtServiceIP.MaxLength = 16;
            this.txtServiceIP.Name = "txtServiceIP";
            this.txtServiceIP.Size = new System.Drawing.Size(100, 21);
            this.txtServiceIP.TabIndex = 7;
            // 
            // txtServicePort
            // 
            this.txtServicePort.Location = new System.Drawing.Point(194, 92);
            this.txtServicePort.MaxLength = 5;
            this.txtServicePort.Name = "txtServicePort";
            this.txtServicePort.Size = new System.Drawing.Size(41, 21);
            this.txtServicePort.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(184, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = ":";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 230);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.ledClock1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AboutTime";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ledClock1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Common.LEDClock ledClock1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnHide;
        private System.Windows.Forms.Button btnSetTime;
        private System.Windows.Forms.ListBox lstLog;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnExist;
        private System.Windows.Forms.Button btnDownHost;
        private System.Windows.Forms.Button btnUpHost;
        private System.Windows.Forms.Button btnEditHost;
        private System.Windows.Forms.Button btnDelHost;
        private System.Windows.Forms.Button btnAddHost;
        private System.Windows.Forms.ListBox lstHosts;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtInterval;
        private System.Windows.Forms.CheckBox cbAutoStart;
        private System.Windows.Forms.CheckBox cbAutoHide;
        private System.Windows.Forms.CheckBox cbAllHosts;
        private System.Windows.Forms.CheckBox cbAutoTiming;
        private System.Windows.Forms.CheckBox cbService;
        private System.Windows.Forms.Timer timer_interval;
        private System.Windows.Forms.TextBox txtServicePort;
        private System.Windows.Forms.TextBox txtServiceIP;
        private System.Windows.Forms.Label label2;
    }
}

