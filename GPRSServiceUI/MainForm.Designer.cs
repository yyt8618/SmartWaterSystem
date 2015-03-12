namespace GPRSServiceUI
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnInstall = new System.Windows.Forms.Button();
            this.btnUninstall = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.txtControl = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.timerCtrl = new System.Windows.Forms.Timer(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDBConnect = new System.Windows.Forms.Button();
            this.txtGPRSPort = new System.Windows.Forms.TextBox();
            this.txtGPRSIP = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.btnGPRSConfirm = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnInstall
            // 
            this.btnInstall.Location = new System.Drawing.Point(68, 20);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(83, 31);
            this.btnInstall.TabIndex = 0;
            this.btnInstall.Text = "安装";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // btnUninstall
            // 
            this.btnUninstall.Location = new System.Drawing.Point(215, 20);
            this.btnUninstall.Name = "btnUninstall";
            this.btnUninstall.Size = new System.Drawing.Size(83, 31);
            this.btnUninstall.TabIndex = 1;
            this.btnUninstall.Text = "卸载";
            this.btnUninstall.UseVisualStyleBackColor = true;
            this.btnUninstall.Click += new System.EventHandler(this.btnUninstall_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(362, 20);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(83, 31);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "启动";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(509, 20);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(83, 31);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // txtControl
            // 
            this.txtControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.txtControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtControl.ForeColor = System.Drawing.Color.Lime;
            this.txtControl.Location = new System.Drawing.Point(3, 17);
            this.txtControl.Multiline = true;
            this.txtControl.Name = "txtControl";
            this.txtControl.ReadOnly = true;
            this.txtControl.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtControl.Size = new System.Drawing.Size(654, 301);
            this.txtControl.TabIndex = 0;
            this.txtControl.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtControl);
            this.groupBox1.Location = new System.Drawing.Point(2, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(660, 321);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "服务信息";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDBConnect);
            this.groupBox2.Controls.Add(this.txtGPRSPort);
            this.groupBox2.Controls.Add(this.txtGPRSIP);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.btnGPRSConfirm);
            this.groupBox2.Location = new System.Drawing.Point(2, 402);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(660, 53);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "远程连接";
            // 
            // btnDBConnect
            // 
            this.btnDBConnect.Location = new System.Drawing.Point(510, 15);
            this.btnDBConnect.Name = "btnDBConnect";
            this.btnDBConnect.Size = new System.Drawing.Size(83, 31);
            this.btnDBConnect.TabIndex = 3;
            this.btnDBConnect.Text = "数据库连接";
            this.btnDBConnect.UseVisualStyleBackColor = true;
            this.btnDBConnect.Click += new System.EventHandler(this.btnDBConnect_Click);
            // 
            // txtGPRSPort
            // 
            this.txtGPRSPort.Location = new System.Drawing.Point(189, 20);
            this.txtGPRSPort.MaxLength = 5;
            this.txtGPRSPort.Name = "txtGPRSPort";
            this.txtGPRSPort.Size = new System.Drawing.Size(100, 21);
            this.txtGPRSPort.TabIndex = 1;
            // 
            // txtGPRSIP
            // 
            this.txtGPRSIP.Location = new System.Drawing.Point(29, 20);
            this.txtGPRSIP.MaxLength = 15;
            this.txtGPRSIP.Name = "txtGPRSIP";
            this.txtGPRSIP.Size = new System.Drawing.Size(100, 21);
            this.txtGPRSIP.TabIndex = 0;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(139, 24);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(47, 12);
            this.label14.TabIndex = 2;
            this.label14.Text = "端口号:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 24);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(23, 12);
            this.label13.TabIndex = 2;
            this.label13.Text = "IP:";
            // 
            // btnGPRSConfirm
            // 
            this.btnGPRSConfirm.Location = new System.Drawing.Point(295, 19);
            this.btnGPRSConfirm.Name = "btnGPRSConfirm";
            this.btnGPRSConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnGPRSConfirm.TabIndex = 2;
            this.btnGPRSConfirm.Text = "保存";
            this.btnGPRSConfirm.UseVisualStyleBackColor = true;
            this.btnGPRSConfirm.Click += new System.EventHandler(this.btnGPRSConfirm_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnInstall);
            this.groupBox3.Controls.Add(this.btnStart);
            this.groupBox3.Controls.Add(this.btnUninstall);
            this.groupBox3.Controls.Add(this.btnStop);
            this.groupBox3.Location = new System.Drawing.Point(2, 331);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(660, 69);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "服务控制";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 458);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "上海敢创远传服务";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.Button btnUninstall;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TextBox txtControl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Timer timerCtrl;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnDBConnect;
        private System.Windows.Forms.TextBox txtGPRSPort;
        private System.Windows.Forms.TextBox txtGPRSIP;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnGPRSConfirm;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}

