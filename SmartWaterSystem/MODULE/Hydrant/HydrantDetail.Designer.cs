namespace SmartWaterSystem
{
    partial class HydrantDetail
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
            this.label3 = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.lstDetailView = new System.Windows.Forms.ListView();
            this.colTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colOpt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPressValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colOpenAngle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBoxList = new System.Windows.Forms.GroupBox();
            this.cbInterval = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnAnalysis = new DevExpress.XtraEditors.SimpleButton();
            this.cbOperate = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBoxList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbInterval.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbOperate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(223, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 14);
            this.label3.TabIndex = 2;
            this.label3.Text = "-";
            // 
            // dtpEnd
            // 
            this.dtpEnd.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEnd.Location = new System.Drawing.Point(236, 8);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(140, 22);
            this.dtpEnd.TabIndex = 3;
            this.dtpEnd.Value = new System.DateTime(2015, 1, 1, 0, 0, 0, 0);
            // 
            // dtpStart
            // 
            this.dtpStart.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStart.Location = new System.Drawing.Point(80, 8);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(140, 22);
            this.dtpStart.TabIndex = 1;
            this.dtpStart.Value = new System.DateTime(2015, 1, 1, 0, 0, 0, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 14);
            this.label2.TabIndex = 0;
            this.label2.Text = "选择日期：";
            // 
            // lstDetailView
            // 
            this.lstDetailView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTime,
            this.colOpt,
            this.colPressValue,
            this.colOpenAngle});
            this.lstDetailView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstDetailView.Font = new System.Drawing.Font("宋体", 11F);
            this.lstDetailView.FullRowSelect = true;
            this.lstDetailView.GridLines = true;
            this.lstDetailView.Location = new System.Drawing.Point(3, 18);
            this.lstDetailView.MultiSelect = false;
            this.lstDetailView.Name = "lstDetailView";
            this.lstDetailView.ShowGroups = false;
            this.lstDetailView.Size = new System.Drawing.Size(804, 456);
            this.lstDetailView.TabIndex = 0;
            this.lstDetailView.TabStop = false;
            this.lstDetailView.UseCompatibleStateImageBehavior = false;
            this.lstDetailView.View = System.Windows.Forms.View.Details;
            // 
            // colTime
            // 
            this.colTime.Text = "时间";
            this.colTime.Width = 146;
            // 
            // colOpt
            // 
            this.colOpt.Text = "动作";
            this.colOpt.Width = 128;
            // 
            // colPressValue
            // 
            this.colPressValue.Text = "压力(Mpa)";
            this.colPressValue.Width = 248;
            // 
            // colOpenAngle
            // 
            this.colOpenAngle.Text = "开度";
            this.colOpenAngle.Width = 235;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(383, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 14);
            this.label1.TabIndex = 4;
            this.label1.Text = "时间间隔：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(521, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 14);
            this.label4.TabIndex = 6;
            this.label4.Text = "分钟";
            // 
            // groupBoxList
            // 
            this.groupBoxList.Controls.Add(this.lstDetailView);
            this.groupBoxList.Location = new System.Drawing.Point(1, 41);
            this.groupBoxList.Name = "groupBoxList";
            this.groupBoxList.Size = new System.Drawing.Size(810, 477);
            this.groupBoxList.TabIndex = 22;
            this.groupBoxList.TabStop = false;
            this.groupBoxList.Text = "列表";
            // 
            // cbInterval
            // 
            this.cbInterval.Location = new System.Drawing.Point(450, 10);
            this.cbInterval.Name = "cbInterval";
            this.cbInterval.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbInterval.Properties.Items.AddRange(new object[] {
            "1",
            "5",
            "15",
            "30",
            "60",
            "120"});
            this.cbInterval.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbInterval.Size = new System.Drawing.Size(65, 20);
            this.cbInterval.TabIndex = 5;
            // 
            // btnAnalysis
            // 
            this.btnAnalysis.Location = new System.Drawing.Point(696, 6);
            this.btnAnalysis.Name = "btnAnalysis";
            this.btnAnalysis.Size = new System.Drawing.Size(87, 27);
            this.btnAnalysis.TabIndex = 9;
            this.btnAnalysis.Text = "分析";
            this.btnAnalysis.Click += new System.EventHandler(this.btnAnalysis_Click);
            // 
            // cbOperate
            // 
            this.cbOperate.Location = new System.Drawing.Point(611, 10);
            this.cbOperate.Name = "cbOperate";
            this.cbOperate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbOperate.Properties.Items.AddRange(new object[] {
            "--全部--",
            "打开",
            "关闭",
            "开度",
            "撞击",
            "撞倒"});
            this.cbOperate.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbOperate.Size = new System.Drawing.Size(65, 20);
            this.cbOperate.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(564, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 14);
            this.label5.TabIndex = 7;
            this.label5.Text = "动作：";
            // 
            // HydrantDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 518);
            this.Controls.Add(this.cbOperate);
            this.Controls.Add(this.cbInterval);
            this.Controls.Add(this.btnAnalysis);
            this.Controls.Add(this.groupBoxList);
            this.Controls.Add(this.dtpEnd);
            this.Controls.Add(this.dtpStart);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HydrantDetail";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "数据详情";
            this.Load += new System.EventHandler(this.HydrantDetail_Load);
            this.groupBoxList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cbInterval.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbOperate.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView lstDetailView;
        private System.Windows.Forms.ColumnHeader colTime;
        private System.Windows.Forms.ColumnHeader colOpt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBoxList;
        private DevExpress.XtraEditors.ComboBoxEdit cbInterval;
        private DevExpress.XtraEditors.SimpleButton btnAnalysis;
        private System.Windows.Forms.ColumnHeader colPressValue;
        private System.Windows.Forms.ColumnHeader colOpenAngle;
        private DevExpress.XtraEditors.ComboBoxEdit cbOperate;
        private System.Windows.Forms.Label label5;
    }
}