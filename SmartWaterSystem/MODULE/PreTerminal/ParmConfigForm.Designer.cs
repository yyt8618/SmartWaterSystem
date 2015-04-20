namespace SmartWaterSystem
{
    partial class ParmConfigForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbSendInterval = new System.Windows.Forms.ComboBox();
            this.cbCollectInterval = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDeviceId = new System.Windows.Forms.TextBox();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.btnSet = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "发送时间间隔";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(52, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 14);
            this.label2.TabIndex = 0;
            this.label2.Text = "采集时间间隔";
            // 
            // cbSendInterval
            // 
            this.cbSendInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSendInterval.FormattingEnabled = true;
            this.cbSendInterval.Items.AddRange(new object[] {
            "5",
            "15",
            "30",
            "60",
            "120",
            "240",
            "480",
            "720",
            "1440"});
            this.cbSendInterval.Location = new System.Drawing.Point(149, 37);
            this.cbSendInterval.Name = "cbSendInterval";
            this.cbSendInterval.Size = new System.Drawing.Size(140, 22);
            this.cbSendInterval.TabIndex = 1;
            // 
            // cbCollectInterval
            // 
            this.cbCollectInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCollectInterval.FormattingEnabled = true;
            this.cbCollectInterval.Items.AddRange(new object[] {
            "1",
            "5",
            "15",
            "30",
            "60",
            "120"});
            this.cbCollectInterval.Location = new System.Drawing.Point(149, 67);
            this.cbCollectInterval.Name = "cbCollectInterval";
            this.cbCollectInterval.Size = new System.Drawing.Size(140, 22);
            this.cbCollectInterval.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(94, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 14);
            this.label3.TabIndex = 3;
            this.label3.Text = "设备ID";
            // 
            // txtDeviceId
            // 
            this.txtDeviceId.Enabled = false;
            this.txtDeviceId.Location = new System.Drawing.Point(149, 5);
            this.txtDeviceId.Name = "txtDeviceId";
            this.txtDeviceId.Size = new System.Drawing.Size(140, 22);
            this.txtDeviceId.TabIndex = 0;
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(133, 95);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(75, 23);
            this.btnSet.TabIndex = 3;
            this.btnSet.Text = "设置";
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // ParmConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 126);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.txtDeviceId);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbCollectInterval);
            this.Controls.Add(this.cbSendInterval);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ParmConfigForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "压力参数设置";
            this.Load += new System.EventHandler(this.ParmConfigForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbSendInterval;
        private System.Windows.Forms.ComboBox cbCollectInterval;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDeviceId;
        private DevExpress.XtraEditors.SimpleButton btnSet;
    }
}

