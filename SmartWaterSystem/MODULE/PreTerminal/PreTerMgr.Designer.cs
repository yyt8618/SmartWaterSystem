namespace SmartWaterSystem
{
    partial class PreTerMgr
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
            this.groupControl8 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.cboxSlopeAlarm = new System.Windows.Forms.CheckBox();
            this.cboxPreAlarm = new System.Windows.Forms.CheckBox();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnAddModify = new System.Windows.Forms.Button();
            this.txtSlopeUpLimit = new System.Windows.Forms.TextBox();
            this.txtPreUpLimite = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSlopeLowLimit = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPreLowLimit = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTerminalName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTerminalID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lstTerminalConfigView = new System.Windows.Forms.ListView();
            this.colTerminalID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTerminalName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPreLowLimit = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPreUpLimit = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPreSlopeLowLimit = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPreSlopeUpLimit = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colRemark = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEnablePreAlarm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEnableSlopeAlarm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.colorPickSlopeUpLimit = new DevExpress.XtraEditors.ColorPickEdit();
            this.colorPickPreLowLimit = new DevExpress.XtraEditors.ColorPickEdit();
            this.label11 = new System.Windows.Forms.Label();
            this.colorPickSlopeLowLimit = new DevExpress.XtraEditors.ColorPickEdit();
            this.label9 = new System.Windows.Forms.Label();
            this.colorPickPreUpLimit = new DevExpress.XtraEditors.ColorPickEdit();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl8)).BeginInit();
            this.groupControl8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.colorPickSlopeUpLimit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorPickPreLowLimit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorPickSlopeLowLimit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorPickPreUpLimit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl8
            // 
            this.groupControl8.Controls.Add(this.groupControl3);
            this.groupControl8.Controls.Add(this.groupControl2);
            this.groupControl8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl8.Location = new System.Drawing.Point(0, 0);
            this.groupControl8.Name = "groupControl8";
            this.groupControl8.Size = new System.Drawing.Size(797, 494);
            this.groupControl8.TabIndex = 4;
            this.groupControl8.Text = "压力终端配置与管理";
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.cboxSlopeAlarm);
            this.groupControl3.Controls.Add(this.cboxPreAlarm);
            this.groupControl3.Controls.Add(this.btnDel);
            this.groupControl3.Controls.Add(this.btnAddModify);
            this.groupControl3.Controls.Add(this.txtSlopeUpLimit);
            this.groupControl3.Controls.Add(this.txtPreUpLimite);
            this.groupControl3.Controls.Add(this.label6);
            this.groupControl3.Controls.Add(this.label4);
            this.groupControl3.Controls.Add(this.txtSlopeLowLimit);
            this.groupControl3.Controls.Add(this.label5);
            this.groupControl3.Controls.Add(this.txtPreLowLimit);
            this.groupControl3.Controls.Add(this.label3);
            this.groupControl3.Controls.Add(this.txtAddress);
            this.groupControl3.Controls.Add(this.txtRemark);
            this.groupControl3.Controls.Add(this.label8);
            this.groupControl3.Controls.Add(this.label7);
            this.groupControl3.Controls.Add(this.txtTerminalName);
            this.groupControl3.Controls.Add(this.label2);
            this.groupControl3.Controls.Add(this.txtTerminalID);
            this.groupControl3.Controls.Add(this.label1);
            this.groupControl3.Controls.Add(this.lstTerminalConfigView);
            this.groupControl3.Location = new System.Drawing.Point(3, 21);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(789, 289);
            this.groupControl3.TabIndex = 0;
            this.groupControl3.Text = "终端设置";
            // 
            // cboxSlopeAlarm
            // 
            this.cboxSlopeAlarm.AutoSize = true;
            this.cboxSlopeAlarm.Location = new System.Drawing.Point(200, 56);
            this.cboxSlopeAlarm.Name = "cboxSlopeAlarm";
            this.cboxSlopeAlarm.Size = new System.Drawing.Size(98, 18);
            this.cboxSlopeAlarm.TabIndex = 5;
            this.cboxSlopeAlarm.Text = "启用斜率报警";
            this.cboxSlopeAlarm.UseVisualStyleBackColor = true;
            this.cboxSlopeAlarm.CheckedChanged += new System.EventHandler(this.cboxSlopeAlarm_CheckedChanged);
            // 
            // cboxPreAlarm
            // 
            this.cboxPreAlarm.AutoSize = true;
            this.cboxPreAlarm.Location = new System.Drawing.Point(200, 28);
            this.cboxPreAlarm.Name = "cboxPreAlarm";
            this.cboxPreAlarm.Size = new System.Drawing.Size(98, 18);
            this.cboxPreAlarm.TabIndex = 1;
            this.cboxPreAlarm.Text = "启用压力报警";
            this.cboxPreAlarm.UseVisualStyleBackColor = true;
            this.cboxPreAlarm.CheckedChanged += new System.EventHandler(this.cboxPreAlarm_CheckedChanged);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(587, 109);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 11;
            this.btnDel.Text = "删除";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnAddModify
            // 
            this.btnAddModify.Location = new System.Drawing.Point(587, 81);
            this.btnAddModify.Name = "btnAddModify";
            this.btnAddModify.Size = new System.Drawing.Size(75, 23);
            this.btnAddModify.TabIndex = 10;
            this.btnAddModify.Text = "添加/修改";
            this.btnAddModify.UseVisualStyleBackColor = true;
            this.btnAddModify.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtSlopeUpLimit
            // 
            this.txtSlopeUpLimit.Location = new System.Drawing.Point(562, 54);
            this.txtSlopeUpLimit.MaxLength = 8;
            this.txtSlopeUpLimit.Name = "txtSlopeUpLimit";
            this.txtSlopeUpLimit.Size = new System.Drawing.Size(100, 22);
            this.txtSlopeUpLimit.TabIndex = 7;
            // 
            // txtPreUpLimite
            // 
            this.txtPreUpLimite.Location = new System.Drawing.Point(562, 26);
            this.txtPreUpLimite.MaxLength = 8;
            this.txtPreUpLimite.Name = "txtPreUpLimite";
            this.txtPreUpLimite.Size = new System.Drawing.Size(100, 22);
            this.txtPreUpLimite.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(479, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 14);
            this.label6.TabIndex = 15;
            this.label6.Text = "压力斜率上限:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(503, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 14);
            this.label4.TabIndex = 18;
            this.label4.Text = "压力上限:";
            // 
            // txtSlopeLowLimit
            // 
            this.txtSlopeLowLimit.Location = new System.Drawing.Point(378, 54);
            this.txtSlopeLowLimit.MaxLength = 8;
            this.txtSlopeLowLimit.Name = "txtSlopeLowLimit";
            this.txtSlopeLowLimit.Size = new System.Drawing.Size(100, 22);
            this.txtSlopeLowLimit.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(295, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 14);
            this.label5.TabIndex = 20;
            this.label5.Text = "压力斜率下限:";
            // 
            // txtPreLowLimit
            // 
            this.txtPreLowLimit.Location = new System.Drawing.Point(378, 27);
            this.txtPreLowLimit.MaxLength = 8;
            this.txtPreLowLimit.Name = "txtPreLowLimit";
            this.txtPreLowLimit.Size = new System.Drawing.Size(100, 22);
            this.txtPreLowLimit.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(319, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 14);
            this.label3.TabIndex = 14;
            this.label3.Text = "压力下限:";
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(94, 110);
            this.txtRemark.MaxLength = 300;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(468, 22);
            this.txtRemark.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(39, 114);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 14);
            this.label8.TabIndex = 16;
            this.label8.Text = "备    注:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(39, 86);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 14);
            this.label7.TabIndex = 17;
            this.label7.Text = "地    址:";
            // 
            // txtTerminalName
            // 
            this.txtTerminalName.Location = new System.Drawing.Point(94, 54);
            this.txtTerminalName.MaxLength = 200;
            this.txtTerminalName.Name = "txtTerminalName";
            this.txtTerminalName.Size = new System.Drawing.Size(100, 22);
            this.txtTerminalName.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 14);
            this.label2.TabIndex = 22;
            this.label2.Text = "终端名称:";
            // 
            // txtTerminalID
            // 
            this.txtTerminalID.Location = new System.Drawing.Point(94, 26);
            this.txtTerminalID.MaxLength = 5;
            this.txtTerminalID.Name = "txtTerminalID";
            this.txtTerminalID.Size = new System.Drawing.Size(100, 22);
            this.txtTerminalID.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 14);
            this.label1.TabIndex = 19;
            this.label1.Text = "终端ID:";
            // 
            // lstTerminalConfigView
            // 
            this.lstTerminalConfigView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTerminalID,
            this.colTerminalName,
            this.colPreLowLimit,
            this.colPreUpLimit,
            this.colPreSlopeLowLimit,
            this.colPreSlopeUpLimit,
            this.colAddress,
            this.colRemark,
            this.colEnablePreAlarm,
            this.colEnableSlopeAlarm});
            this.lstTerminalConfigView.FullRowSelect = true;
            this.lstTerminalConfigView.Location = new System.Drawing.Point(38, 138);
            this.lstTerminalConfigView.MultiSelect = false;
            this.lstTerminalConfigView.Name = "lstTerminalConfigView";
            this.lstTerminalConfigView.Size = new System.Drawing.Size(624, 139);
            this.lstTerminalConfigView.TabIndex = 12;
            this.lstTerminalConfigView.TabStop = false;
            this.lstTerminalConfigView.UseCompatibleStateImageBehavior = false;
            this.lstTerminalConfigView.View = System.Windows.Forms.View.Details;
            this.lstTerminalConfigView.SelectedIndexChanged += new System.EventHandler(this.lstTerminalConfigView_SelectedIndexChanged);
            // 
            // colTerminalID
            // 
            this.colTerminalID.Text = "终端ID";
            // 
            // colTerminalName
            // 
            this.colTerminalName.Text = "终端名称";
            this.colTerminalName.Width = 61;
            // 
            // colPreLowLimit
            // 
            this.colPreLowLimit.Text = "压力下限";
            this.colPreLowLimit.Width = 80;
            // 
            // colPreUpLimit
            // 
            this.colPreUpLimit.Text = "压力上限";
            this.colPreUpLimit.Width = 87;
            // 
            // colPreSlopeLowLimit
            // 
            this.colPreSlopeLowLimit.Text = "压力斜率下限";
            this.colPreSlopeLowLimit.Width = 90;
            // 
            // colPreSlopeUpLimit
            // 
            this.colPreSlopeUpLimit.Text = "压力斜率上限";
            this.colPreSlopeUpLimit.Width = 94;
            // 
            // colAddress
            // 
            this.colAddress.Text = "地址";
            this.colAddress.Width = 137;
            // 
            // colRemark
            // 
            this.colRemark.Text = "备注";
            this.colRemark.Width = 118;
            // 
            // colEnablePreAlarm
            // 
            this.colEnablePreAlarm.Text = "压力报警";
            this.colEnablePreAlarm.Width = 0;
            // 
            // colEnableSlopeAlarm
            // 
            this.colEnableSlopeAlarm.Text = "斜率报警";
            this.colEnableSlopeAlarm.Width = 0;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.colorPickSlopeUpLimit);
            this.groupControl2.Controls.Add(this.colorPickPreLowLimit);
            this.groupControl2.Controls.Add(this.label11);
            this.groupControl2.Controls.Add(this.colorPickSlopeLowLimit);
            this.groupControl2.Controls.Add(this.label9);
            this.groupControl2.Controls.Add(this.colorPickPreUpLimit);
            this.groupControl2.Controls.Add(this.label10);
            this.groupControl2.Controls.Add(this.label12);
            this.groupControl2.Location = new System.Drawing.Point(3, 316);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(789, 87);
            this.groupControl2.TabIndex = 1;
            this.groupControl2.Text = "颜色配置";
            // 
            // colorPickSlopeUpLimit
            // 
            this.colorPickSlopeUpLimit.EditValue = System.Drawing.Color.Empty;
            this.colorPickSlopeUpLimit.Location = new System.Drawing.Point(409, 55);
            this.colorPickSlopeUpLimit.Name = "colorPickSlopeUpLimit";
            this.colorPickSlopeUpLimit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.colorPickSlopeUpLimit.Size = new System.Drawing.Size(100, 20);
            this.colorPickSlopeUpLimit.TabIndex = 3;
            this.colorPickSlopeUpLimit.EditValueChanged += new System.EventHandler(this.colorPickSlopeUpLimit_EditValueChanged);
            // 
            // colorPickPreLowLimit
            // 
            this.colorPickPreLowLimit.EditValue = System.Drawing.Color.Empty;
            this.colorPickPreLowLimit.Location = new System.Drawing.Point(119, 30);
            this.colorPickPreLowLimit.Name = "colorPickPreLowLimit";
            this.colorPickPreLowLimit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.colorPickPreLowLimit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.colorPickPreLowLimit.Properties.ColorDialogOptions.ShowArrows = DevExpress.XtraEditors.ShowArrows.False;
            this.colorPickPreLowLimit.Size = new System.Drawing.Size(100, 20);
            this.colorPickPreLowLimit.TabIndex = 0;
            this.colorPickPreLowLimit.EditValueChanged += new System.EventHandler(this.colorPickPreLowLimit_EditValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(354, 34);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(59, 14);
            this.label11.TabIndex = 1;
            this.label11.Text = "压力上限:";
            // 
            // colorPickSlopeLowLimit
            // 
            this.colorPickSlopeLowLimit.EditValue = System.Drawing.Color.Empty;
            this.colorPickSlopeLowLimit.Location = new System.Drawing.Point(119, 55);
            this.colorPickSlopeLowLimit.Name = "colorPickSlopeLowLimit";
            this.colorPickSlopeLowLimit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.colorPickSlopeLowLimit.Size = new System.Drawing.Size(100, 20);
            this.colorPickSlopeLowLimit.TabIndex = 2;
            this.colorPickSlopeLowLimit.EditValueChanged += new System.EventHandler(this.colorPickSlopeLowLimit_EditValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(63, 34);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 14);
            this.label9.TabIndex = 1;
            this.label9.Text = "压力下限:";
            // 
            // colorPickPreUpLimit
            // 
            this.colorPickPreUpLimit.EditValue = System.Drawing.Color.Empty;
            this.colorPickPreUpLimit.Location = new System.Drawing.Point(409, 30);
            this.colorPickPreUpLimit.Name = "colorPickPreUpLimit";
            this.colorPickPreUpLimit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.colorPickPreUpLimit.Size = new System.Drawing.Size(100, 20);
            this.colorPickPreUpLimit.TabIndex = 1;
            this.colorPickPreUpLimit.EditValueChanged += new System.EventHandler(this.colorPickPreUpLimit_EditValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(40, 59);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(83, 14);
            this.label10.TabIndex = 1;
            this.label10.Text = "压力斜率下限:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(330, 59);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(83, 14);
            this.label12.TabIndex = 1;
            this.label12.Text = "压力斜率上限:";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(94, 81);
            this.txtAddress.MaxLength = 300;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(468, 22);
            this.txtAddress.TabIndex = 8;
            // 
            // PreTerMgr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl8);
            this.Name = "PreTerMgr";
            this.Size = new System.Drawing.Size(797, 494);
            this.Load += new System.EventHandler(this.PreTerMgr_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl8)).EndInit();
            this.groupControl8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            this.groupControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.colorPickSlopeUpLimit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorPickPreLowLimit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorPickSlopeLowLimit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorPickPreUpLimit.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl8;
        private DevExpress.XtraEditors.ColorPickEdit colorPickSlopeUpLimit;
        private DevExpress.XtraEditors.ColorPickEdit colorPickPreLowLimit;
        private DevExpress.XtraEditors.ColorPickEdit colorPickSlopeLowLimit;
        private DevExpress.XtraEditors.ColorPickEdit colorPickPreUpLimit;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private System.Windows.Forms.CheckBox cboxSlopeAlarm;
        private System.Windows.Forms.CheckBox cboxPreAlarm;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnAddModify;
        private System.Windows.Forms.TextBox txtSlopeUpLimit;
        private System.Windows.Forms.TextBox txtPreUpLimite;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSlopeLowLimit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPreLowLimit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtTerminalName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTerminalID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lstTerminalConfigView;
        private System.Windows.Forms.ColumnHeader colTerminalID;
        private System.Windows.Forms.ColumnHeader colTerminalName;
        private System.Windows.Forms.ColumnHeader colPreLowLimit;
        private System.Windows.Forms.ColumnHeader colPreUpLimit;
        private System.Windows.Forms.ColumnHeader colPreSlopeLowLimit;
        private System.Windows.Forms.ColumnHeader colPreSlopeUpLimit;
        private System.Windows.Forms.ColumnHeader colAddress;
        private System.Windows.Forms.ColumnHeader colRemark;
        private System.Windows.Forms.ColumnHeader colEnablePreAlarm;
        private System.Windows.Forms.ColumnHeader colEnableSlopeAlarm;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private System.Windows.Forms.TextBox txtAddress;

    }
}