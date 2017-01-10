namespace SmartWaterSystem
{
    partial class CancleAlarmForm
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
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtCancleAlarmLen = new DevExpress.XtraEditors.TextEdit();
            this.btnCancle = new DevExpress.XtraEditors.SimpleButton();
            this.rgEnableAlarm = new DevExpress.XtraEditors.RadioGroup();
            this.ceCancleAlarmLen = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCancleAlarmLen.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgEnableAlarm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceCancleAlarmLen.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl2.Location = new System.Drawing.Point(32, 12);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(108, 14);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "停用将关闭实时报警";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(100, 131);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "确定";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(319, 91);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(24, 14);
            this.labelControl1.TabIndex = 5;
            this.labelControl1.Text = "小时";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(32, 58);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(100, 14);
            this.labelControl4.TabIndex = 1;
            this.labelControl4.Text = "是否启用实时报警:";
            // 
            // txtCancleAlarmLen
            // 
            this.txtCancleAlarmLen.Enabled = false;
            this.txtCancleAlarmLen.Location = new System.Drawing.Point(137, 88);
            this.txtCancleAlarmLen.Name = "txtCancleAlarmLen";
            this.txtCancleAlarmLen.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtCancleAlarmLen.Properties.MaxLength = 4;
            this.txtCancleAlarmLen.Size = new System.Drawing.Size(176, 20);
            this.txtCancleAlarmLen.TabIndex = 4;
            this.txtCancleAlarmLen.EditValueChanged += new System.EventHandler(this.txtCancleAlarmLen_EditValueChanged);
            // 
            // btnCancle
            // 
            this.btnCancle.Location = new System.Drawing.Point(191, 131);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(75, 23);
            this.btnCancle.TabIndex = 7;
            this.btnCancle.Text = "取消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // rgEnableAlarm
            // 
            this.rgEnableAlarm.Location = new System.Drawing.Point(137, 53);
            this.rgEnableAlarm.Name = "rgEnableAlarm";
            this.rgEnableAlarm.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.rgEnableAlarm.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "启用实时报警"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "停用实时报警")});
            this.rgEnableAlarm.Properties.ItemsLayout = DevExpress.XtraEditors.RadioGroupItemsLayout.Flow;
            this.rgEnableAlarm.Size = new System.Drawing.Size(206, 29);
            this.rgEnableAlarm.TabIndex = 2;
            this.rgEnableAlarm.SelectedIndexChanged += new System.EventHandler(this.rgEnableAlarm_SelectedIndexChanged);
            // 
            // ceCancleAlarmLen
            // 
            this.ceCancleAlarmLen.Enabled = false;
            this.ceCancleAlarmLen.Location = new System.Drawing.Point(12, 86);
            this.ceCancleAlarmLen.Name = "ceCancleAlarmLen";
            this.ceCancleAlarmLen.Properties.Caption = "设置停用报警时长:";
            this.ceCancleAlarmLen.Size = new System.Drawing.Size(119, 19);
            this.ceCancleAlarmLen.TabIndex = 3;
            // 
            // CancleAlarmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 170);
            this.Controls.Add(this.ceCancleAlarmLen);
            this.Controls.Add(this.rgEnableAlarm);
            this.Controls.Add(this.txtCancleAlarmLen);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CancleAlarmForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "设置报警";
            this.Load += new System.EventHandler(this.CancleAlarmForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtCancleAlarmLen.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgEnableAlarm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceCancleAlarmLen.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txtCancleAlarmLen;
        private DevExpress.XtraEditors.SimpleButton btnCancle;
        private DevExpress.XtraEditors.RadioGroup rgEnableAlarm;
        private DevExpress.XtraEditors.CheckEdit ceCancleAlarmLen;
    }
}