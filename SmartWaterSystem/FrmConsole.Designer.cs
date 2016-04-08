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
            this.txtControl = new System.Windows.Forms.TextBox();
            this.cbShowSocket = new System.Windows.Forms.CheckBox();
            this.cbHTTP = new System.Windows.Forms.CheckBox();
            this.cbSerialPort = new System.Windows.Forms.CheckBox();
            this.cbErrs = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRefurbish = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
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
            this.btnClear.Location = new System.Drawing.Point(711, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(52, 23);
            this.btnClear.TabIndex = 0;
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
            this.txtControl.Multiline = true;
            this.txtControl.Name = "txtControl";
            this.txtControl.ReadOnly = true;
            this.txtControl.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtControl.Size = new System.Drawing.Size(766, 393);
            this.txtControl.TabIndex = 3;
            this.txtControl.TabStop = false;
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
            this.cbShowSocket.TabIndex = 1;
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
            this.cbHTTP.TabIndex = 2;
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
            this.cbSerialPort.TabIndex = 3;
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
            this.cbErrs.TabIndex = 4;
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
            this.panel1.Controls.Add(this.btnRefurbish);
            this.panel1.Controls.Add(this.cbErrs);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.cbSerialPort);
            this.panel1.Controls.Add(this.cbShowSocket);
            this.panel1.Controls.Add(this.cbHTTP);
            this.panel1.Location = new System.Drawing.Point(0, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(766, 32);
            this.panel1.TabIndex = 5;
            // 
            // btnRefurbish
            // 
            this.btnRefurbish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefurbish.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
            this.btnRefurbish.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(226)))), ((int)(((byte)(254)))));
            this.btnRefurbish.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
            this.btnRefurbish.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
            this.btnRefurbish.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(226)))), ((int)(((byte)(254)))));
            this.btnRefurbish.Location = new System.Drawing.Point(656, 3);
            this.btnRefurbish.Name = "btnRefurbish";
            this.btnRefurbish.Size = new System.Drawing.Size(52, 23);
            this.btnRefurbish.TabIndex = 0;
            this.btnRefurbish.TabStop = false;
            this.btnRefurbish.Text = "刷新";
            this.btnRefurbish.UseVisualStyleBackColor = false;
            this.btnRefurbish.Visible = false;
            this.btnRefurbish.Click += new System.EventHandler(this.btnRefurbish_Click);
            // 
            // FrmConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(766, 426);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtControl);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerCtrl;
        private Button btnClear;
        private TextBox txtControl;
        private CheckBox cbShowSocket;
        private CheckBox cbHTTP;
        private CheckBox cbSerialPort;
        private CheckBox cbErrs;
        private Panel panel1;
        private Button btnRefurbish;
    }
}