namespace NoiseAnalysisSystem
{
    partial class FrmSerialComSet
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
            this.label2 = new System.Windows.Forms.Label();
            this.txtTime = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.label42 = new System.Windows.Forms.Label();
            this.txtStopPos = new DevExpress.XtraEditors.TextEdit();
            this.txtDataPos = new DevExpress.XtraEditors.TextEdit();
            this.txtTestPos = new DevExpress.XtraEditors.TextEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.cbBaudRate = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.chartControl1 = new DevExpress.XtraCharts.ChartControl();
            this.cbSerialPort = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStopPos.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataPos.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTestPos.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbBaudRate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbSerialPort.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(234, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 14);
            this.label2.TabIndex = 39;
            this.label2.Text = "s";
            // 
            // txtTime
            // 
            this.txtTime.Location = new System.Drawing.Point(94, 21);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(141, 20);
            this.txtTime.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 14);
            this.label1.TabIndex = 37;
            this.label1.Text = "读取超时时长";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(30, 225);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(66, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "确定";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(215, 88);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(31, 14);
            this.label42.TabIndex = 35;
            this.label42.Text = "bit/s";
            // 
            // txtStopPos
            // 
            this.txtStopPos.EditValue = "1";
            this.txtStopPos.Location = new System.Drawing.Point(94, 181);
            this.txtStopPos.Name = "txtStopPos";
            this.txtStopPos.Size = new System.Drawing.Size(141, 20);
            this.txtStopPos.TabIndex = 5;
            // 
            // txtDataPos
            // 
            this.txtDataPos.EditValue = "8";
            this.txtDataPos.Location = new System.Drawing.Point(94, 149);
            this.txtDataPos.Name = "txtDataPos";
            this.txtDataPos.Size = new System.Drawing.Size(141, 20);
            this.txtDataPos.TabIndex = 4;
            // 
            // txtTestPos
            // 
            this.txtTestPos.EditValue = "0";
            this.txtTestPos.Location = new System.Drawing.Point(94, 117);
            this.txtTestPos.Name = "txtTestPos";
            this.txtTestPos.Size = new System.Drawing.Size(141, 20);
            this.txtTestPos.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 184);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 14);
            this.label4.TabIndex = 30;
            this.label4.Text = "停止位";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 152);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(43, 14);
            this.label12.TabIndex = 28;
            this.label12.Text = "数据位";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(9, 120);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(43, 14);
            this.label23.TabIndex = 26;
            this.label23.Text = "校验位";
            // 
            // cbBaudRate
            // 
            this.cbBaudRate.Location = new System.Drawing.Point(94, 85);
            this.cbBaudRate.Name = "cbBaudRate";
            this.cbBaudRate.Size = new System.Drawing.Size(121, 20);
            this.cbBaudRate.TabIndex = 2;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(9, 88);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(43, 14);
            this.label24.TabIndex = 24;
            this.label24.Text = "波特率";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(9, 56);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(43, 14);
            this.label25.TabIndex = 22;
            this.label25.Text = "串口号";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(152, 225);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(66, 23);
            this.simpleButton1.TabIndex = 7;
            this.simpleButton1.Text = "取消";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // chartControl1
            // 
            this.chartControl1.Location = new System.Drawing.Point(0, 0);
            this.chartControl1.Name = "chartControl1";
            this.chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.chartControl1.Size = new System.Drawing.Size(300, 200);
            this.chartControl1.TabIndex = 0;
            // 
            // cbSerialPort
            // 
            this.cbSerialPort.Location = new System.Drawing.Point(94, 53);
            this.cbSerialPort.Name = "cbSerialPort";
            this.cbSerialPort.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbSerialPort.Size = new System.Drawing.Size(141, 20);
            this.cbSerialPort.TabIndex = 1;
            // 
            // FrmSerialComSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(250, 260);
            this.Controls.Add(this.cbBaudRate);
            this.Controls.Add(this.txtTime);
            this.Controls.Add(this.cbSerialPort);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label42);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.txtStopPos);
            this.Controls.Add(this.txtDataPos);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.txtTestPos);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.Name = "FrmSerialComSet";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "串口通讯设置";
            ((System.ComponentModel.ISupportInitialize)(this.txtTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStopPos.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataPos.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTestPos.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbBaudRate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbSerialPort.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnSave;
        private System.Windows.Forms.Label label42;
        private DevExpress.XtraEditors.TextEdit txtStopPos;
        private DevExpress.XtraEditors.TextEdit txtDataPos;
        private DevExpress.XtraEditors.TextEdit txtTestPos;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label23;
        private DevExpress.XtraEditors.ComboBoxEdit cbBaudRate;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.TextEdit txtTime;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraCharts.ChartControl chartControl1;
        private DevExpress.XtraEditors.LookUpEdit cbSerialPort;
    }
}