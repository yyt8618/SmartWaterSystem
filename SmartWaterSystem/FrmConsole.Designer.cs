using System.Windows.Forms;
namespace SmartWaterSystem
{
    partial class FrmConsole
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmConsole));
            this.timerCtrl = new System.Windows.Forms.Timer(this.components);
            this.btnClear = new System.Windows.Forms.Button();
            this.txtControl = new System.Windows.Forms.RichTextBox();
            this.cbShowSocket = new System.Windows.Forms.CheckBox();
            this.cbHTTP = new System.Windows.Forms.CheckBox();
            this.cbSerialPort = new System.Windows.Forms.CheckBox();
            this.cbErrs = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkedComboBoxEdit1 = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.comboToolBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.comboSocketServer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.picBoxLog = new System.Windows.Forms.PictureBox();
            this.picSockConnect = new System.Windows.Forms.PictureBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnSocketConnect = new System.Windows.Forms.Button();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkedComboBoxEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboToolBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboSocketServer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSockConnect)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
            this.btnClear.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(226)))), ((int)(((byte)(254)))));
            this.btnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
            this.btnClear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(226)))), ((int)(((byte)(254)))));
            this.btnClear.Location = new System.Drawing.Point(823, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(52, 23);
            this.btnClear.TabIndex = 9;
            this.btnClear.TabStop = false;
            this.btnClear.Text = "清除";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtControl
            // 
            this.txtControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.txtControl.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtControl.ForeColor = System.Drawing.Color.Lime;
            this.txtControl.Location = new System.Drawing.Point(0, 33);
            this.txtControl.Name = "txtControl";
            this.txtControl.ReadOnly = true;
            this.txtControl.Size = new System.Drawing.Size(878, 459);
            this.txtControl.TabIndex = 1;
            this.txtControl.TabStop = false;
            this.txtControl.Text = "";
            this.txtControl.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtControl_KeyUp);
            // 
            // cbShowSocket
            // 
            this.cbShowSocket.AutoSize = true;
            this.cbShowSocket.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
            this.cbShowSocket.Checked = true;
            this.cbShowSocket.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowSocket.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbShowSocket.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(226)))), ((int)(((byte)(254)))));
            this.cbShowSocket.Location = new System.Drawing.Point(12, 5);
            this.cbShowSocket.Name = "cbShowSocket";
            this.cbShowSocket.Size = new System.Drawing.Size(47, 18);
            this.cbShowSocket.TabIndex = 0;
            this.cbShowSocket.TabStop = false;
            this.cbShowSocket.Text = "远传";
            this.cbShowSocket.UseVisualStyleBackColor = false;
            this.cbShowSocket.CheckedChanged += new System.EventHandler(this.cbShowSocket_CheckedChanged);
            // 
            // cbHTTP
            // 
            this.cbHTTP.AutoSize = true;
            this.cbHTTP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
            this.cbHTTP.Checked = true;
            this.cbHTTP.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHTTP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbHTTP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(226)))), ((int)(((byte)(254)))));
            this.cbHTTP.Location = new System.Drawing.Point(65, 5);
            this.cbHTTP.Name = "cbHTTP";
            this.cbHTTP.Size = new System.Drawing.Size(47, 18);
            this.cbHTTP.TabIndex = 1;
            this.cbHTTP.TabStop = false;
            this.cbHTTP.Text = "手机";
            this.cbHTTP.UseVisualStyleBackColor = false;
            this.cbHTTP.CheckedChanged += new System.EventHandler(this.cbHTTP_CheckedChanged);
            // 
            // cbSerialPort
            // 
            this.cbSerialPort.AutoSize = true;
            this.cbSerialPort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
            this.cbSerialPort.Checked = true;
            this.cbSerialPort.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSerialPort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSerialPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(226)))), ((int)(((byte)(254)))));
            this.cbSerialPort.Location = new System.Drawing.Point(118, 5);
            this.cbSerialPort.Name = "cbSerialPort";
            this.cbSerialPort.Size = new System.Drawing.Size(47, 18);
            this.cbSerialPort.TabIndex = 2;
            this.cbSerialPort.TabStop = false;
            this.cbSerialPort.Text = "串口";
            this.cbSerialPort.UseVisualStyleBackColor = false;
            this.cbSerialPort.CheckedChanged += new System.EventHandler(this.cbSerialPort_CheckedChanged);
            // 
            // cbErrs
            // 
            this.cbErrs.AutoSize = true;
            this.cbErrs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
            this.cbErrs.Checked = true;
            this.cbErrs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbErrs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbErrs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(226)))), ((int)(((byte)(254)))));
            this.cbErrs.Location = new System.Drawing.Point(171, 5);
            this.cbErrs.Name = "cbErrs";
            this.cbErrs.Size = new System.Drawing.Size(47, 18);
            this.cbErrs.TabIndex = 3;
            this.cbErrs.TabStop = false;
            this.cbErrs.Text = "错误";
            this.cbErrs.UseVisualStyleBackColor = false;
            this.cbErrs.CheckedChanged += new System.EventHandler(this.cbErrs_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.panel1.Controls.Add(this.checkedComboBoxEdit1);
            this.panel1.Controls.Add(this.comboToolBox);
            this.panel1.Controls.Add(this.comboSocketServer);
            this.panel1.Controls.Add(this.picBoxLog);
            this.panel1.Controls.Add(this.picSockConnect);
            this.panel1.Controls.Add(this.btnDisconnect);
            this.panel1.Controls.Add(this.btnCopy);
            this.panel1.Controls.Add(this.btnPause);
            this.panel1.Controls.Add(this.btnSocketConnect);
            this.panel1.Controls.Add(this.cbErrs);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.cbSerialPort);
            this.panel1.Controls.Add(this.cbShowSocket);
            this.panel1.Controls.Add(this.cbHTTP);
            this.panel1.Location = new System.Drawing.Point(0, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(878, 32);
            this.panel1.TabIndex = 0;
            // 
            // checkedComboBoxEdit1
            // 
            this.checkedComboBoxEdit1.EditValue = "";
            this.checkedComboBoxEdit1.Location = new System.Drawing.Point(224, 6);
            this.checkedComboBoxEdit1.Name = "checkedComboBoxEdit1";
            this.checkedComboBoxEdit1.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
            this.checkedComboBoxEdit1.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(226)))), ((int)(((byte)(254)))));
            this.checkedComboBoxEdit1.Properties.Appearance.Options.UseBackColor = true;
            this.checkedComboBoxEdit1.Properties.Appearance.Options.UseForeColor = true;
            this.checkedComboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.checkedComboBoxEdit1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "数据帧(socket)"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "压力流量终端(socket)"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "压力控制器(socket)"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "通用终端(socket)"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "水质终端(socket)"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "消防栓(socket)"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "噪声远传(socket)"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "水厂(socket)"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "手机"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "串口"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "错误")});
            this.checkedComboBoxEdit1.Size = new System.Drawing.Size(42, 20);
            this.checkedComboBoxEdit1.TabIndex = 10;
            this.checkedComboBoxEdit1.Visible = false;
            // 
            // comboToolBox
            // 
            this.comboToolBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboToolBox.EditValue = "工具箱";
            this.comboToolBox.Location = new System.Drawing.Point(328, 4);
            this.comboToolBox.Name = "comboToolBox";
            this.comboToolBox.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.comboToolBox.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(226)))), ((int)(((byte)(254)))));
            this.comboToolBox.Properties.Appearance.Options.UseBackColor = true;
            this.comboToolBox.Properties.Appearance.Options.UseForeColor = true;
            this.comboToolBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboToolBox.Properties.Items.AddRange(new object[] {
            "C#超级通信调试工具",
            "串口调试助手",
            "网络调试助手",
            "计算器"});
            this.comboToolBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboToolBox.Size = new System.Drawing.Size(94, 20);
            this.comboToolBox.TabIndex = 4;
            this.comboToolBox.SelectedIndexChanged += new System.EventHandler(this.comboToolBox_SelectedIndexChanged);
            // 
            // comboSocketServer
            // 
            this.comboSocketServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboSocketServer.Location = new System.Drawing.Point(423, 4);
            this.comboSocketServer.Name = "comboSocketServer";
            this.comboSocketServer.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.comboSocketServer.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(226)))), ((int)(((byte)(254)))));
            this.comboSocketServer.Properties.Appearance.Options.UseBackColor = true;
            this.comboSocketServer.Properties.Appearance.Options.UseForeColor = true;
            this.comboSocketServer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboSocketServer.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboSocketServer.Size = new System.Drawing.Size(123, 20);
            this.comboSocketServer.TabIndex = 4;
            this.comboSocketServer.SelectedIndexChanged += new System.EventHandler(this.comboSocketServer_SelectedIndexChanged);
            // 
            // picBoxLog
            // 
            this.picBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picBoxLog.Image = ((System.Drawing.Image)(resources.GetObject("picBoxLog.Image")));
            this.picBoxLog.Location = new System.Drawing.Point(549, -2);
            this.picBoxLog.Name = "picBoxLog";
            this.picBoxLog.Size = new System.Drawing.Size(23, 26);
            this.picBoxLog.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxLog.TabIndex = 5;
            this.picBoxLog.TabStop = false;
            this.picBoxLog.Tag = "0";
            this.picBoxLog.Click += new System.EventHandler(this.picBoxLog_Click);
            // 
            // picSockConnect
            // 
            this.picSockConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picSockConnect.Image = global::SmartWaterSystem.Properties.Resources.SockNotConnect;
            this.picSockConnect.InitialImage = global::SmartWaterSystem.Properties.Resources.SockNotConnect;
            this.picSockConnect.Location = new System.Drawing.Point(574, 6);
            this.picSockConnect.Name = "picSockConnect";
            this.picSockConnect.Size = new System.Drawing.Size(41, 20);
            this.picSockConnect.TabIndex = 5;
            this.picSockConnect.TabStop = false;
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisconnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
            this.btnDisconnect.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(226)))), ((int)(((byte)(254)))));
            this.btnDisconnect.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
            this.btnDisconnect.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
            this.btnDisconnect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(226)))), ((int)(((byte)(254)))));
            this.btnDisconnect.Location = new System.Drawing.Point(667, 3);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(52, 23);
            this.btnDisconnect.TabIndex = 6;
            this.btnDisconnect.TabStop = false;
            this.btnDisconnect.Text = "断开";
            this.btnDisconnect.UseVisualStyleBackColor = false;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
            this.btnCopy.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(226)))), ((int)(((byte)(254)))));
            this.btnCopy.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
            this.btnCopy.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
            this.btnCopy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(226)))), ((int)(((byte)(254)))));
            this.btnCopy.Location = new System.Drawing.Point(771, 3);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(52, 23);
            this.btnCopy.TabIndex = 8;
            this.btnCopy.TabStop = false;
            this.btnCopy.Text = "复制";
            this.btnCopy.UseVisualStyleBackColor = false;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnPause
            // 
            this.btnPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPause.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
            this.btnPause.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(226)))), ((int)(((byte)(254)))));
            this.btnPause.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
            this.btnPause.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
            this.btnPause.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(226)))), ((int)(((byte)(254)))));
            this.btnPause.Location = new System.Drawing.Point(719, 3);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(52, 23);
            this.btnPause.TabIndex = 7;
            this.btnPause.TabStop = false;
            this.btnPause.Text = "暂停";
            this.btnPause.UseVisualStyleBackColor = false;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnSocketConnect
            // 
            this.btnSocketConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSocketConnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
            this.btnSocketConnect.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(226)))), ((int)(((byte)(254)))));
            this.btnSocketConnect.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
            this.btnSocketConnect.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
            this.btnSocketConnect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(226)))), ((int)(((byte)(254)))));
            this.btnSocketConnect.Location = new System.Drawing.Point(615, 3);
            this.btnSocketConnect.Name = "btnSocketConnect";
            this.btnSocketConnect.Size = new System.Drawing.Size(52, 23);
            this.btnSocketConnect.TabIndex = 5;
            this.btnSocketConnect.TabStop = false;
            this.btnSocketConnect.Text = "连接";
            this.btnSocketConnect.UseVisualStyleBackColor = false;
            this.btnSocketConnect.Click += new System.EventHandler(this.btnSocketConnect_Click);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(878, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 492);
            this.barDockControlBottom.Size = new System.Drawing.Size(878, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 492);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(878, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 492);
            // 
            // FrmConsole
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
            this.Appearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
            this.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(226)))), ((int)(((byte)(254)))));
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(878, 492);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtControl);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmConsole";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "监控";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmGPRSConsole_FormClosing);
            this.Load += new System.EventHandler(this.FrmConsole_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkedComboBoxEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboToolBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboSocketServer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSockConnect)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerCtrl;
        private Button btnClear;
        private RichTextBox txtControl;
        private CheckBox cbShowSocket;
        private CheckBox cbHTTP;
        private CheckBox cbSerialPort;
        private CheckBox cbErrs;
        private Panel panel1;
        private Button btnSocketConnect;
        private PictureBox picSockConnect;
        private Button btnDisconnect;
        private Button btnCopy;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private Button btnPause;
        private DevExpress.XtraEditors.ComboBoxEdit comboSocketServer;
        private DevExpress.XtraEditors.CheckedComboBoxEdit checkedComboBoxEdit1;
        private DevExpress.XtraEditors.ComboBoxEdit comboToolBox;
        private PictureBox picBoxLog;
    }
}