namespace SmartWaterSystem
{
    partial class TreeNodeInfoForm
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
            this.cbPrecision = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.txtUnit = new DevExpress.XtraEditors.TextEdit();
            this.txtMaxMeasureRFlag = new DevExpress.XtraEditors.TextEdit();
            this.txtMaxMeasureR = new DevExpress.XtraEditors.TextEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.txtName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtFrameWidth = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.cbPrecision.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUnit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxMeasureRFlag.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxMeasureR.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFrameWidth.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // cbPrecision
            // 
            this.cbPrecision.Location = new System.Drawing.Point(104, 135);
            this.cbPrecision.Name = "cbPrecision";
            this.cbPrecision.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbPrecision.Properties.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4"});
            this.cbPrecision.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbPrecision.Size = new System.Drawing.Size(86, 20);
            this.cbPrecision.TabIndex = 5;
            // 
            // cbType
            // 
            this.cbType.Location = new System.Drawing.Point(104, 3);
            this.cbType.Name = "cbType";
            this.cbType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbType.Properties.Items.AddRange(new object[] {
            "模拟",
            "脉冲",
            "RS485"});
            this.cbType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbType.Size = new System.Drawing.Size(86, 20);
            this.cbType.TabIndex = 0;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(74, 32);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(28, 14);
            this.labelControl2.TabIndex = 130;
            this.labelControl2.Text = "名称:";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(50, 6);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(52, 14);
            this.labelControl6.TabIndex = 131;
            this.labelControl6.Text = "采集类型:";
            // 
            // txtUnit
            // 
            this.txtUnit.EditValue = "";
            this.txtUnit.Location = new System.Drawing.Point(104, 161);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Properties.MaxLength = 10;
            this.txtUnit.Size = new System.Drawing.Size(86, 20);
            this.txtUnit.TabIndex = 6;
            // 
            // txtMaxMeasureRFlag
            // 
            this.txtMaxMeasureRFlag.Location = new System.Drawing.Point(104, 109);
            this.txtMaxMeasureRFlag.Name = "txtMaxMeasureRFlag";
            this.txtMaxMeasureRFlag.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtMaxMeasureRFlag.Properties.MaxLength = 5;
            this.txtMaxMeasureRFlag.Size = new System.Drawing.Size(86, 20);
            this.txtMaxMeasureRFlag.TabIndex = 4;
            // 
            // txtMaxMeasureR
            // 
            this.txtMaxMeasureR.Location = new System.Drawing.Point(104, 83);
            this.txtMaxMeasureR.Name = "txtMaxMeasureR";
            this.txtMaxMeasureR.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtMaxMeasureR.Properties.MaxLength = 5;
            this.txtMaxMeasureR.Size = new System.Drawing.Size(86, 20);
            this.txtMaxMeasureR.TabIndex = 3;
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(2, 112);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(100, 14);
            this.labelControl9.TabIndex = 128;
            this.labelControl9.Text = "仪表最大测量范围:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(104, 29);
            this.txtName.Name = "txtName";
            this.txtName.Properties.MaxLength = 15;
            this.txtName.Size = new System.Drawing.Size(86, 20);
            this.txtName.TabIndex = 1;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(26, 86);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(76, 14);
            this.labelControl7.TabIndex = 127;
            this.labelControl7.Text = "最大测量范围:";
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(74, 164);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(28, 14);
            this.labelControl10.TabIndex = 129;
            this.labelControl10.Text = "单位:";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(74, 138);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(28, 14);
            this.labelControl8.TabIndex = 132;
            this.labelControl8.Text = "精度:";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(82, 187);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(192, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 14);
            this.label1.TabIndex = 134;
            this.label1.Text = "如:20m";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(192, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 14);
            this.label2.TabIndex = 134;
            this.label2.Text = "如:FFF";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(50, 58);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(52, 14);
            this.labelControl1.TabIndex = 127;
            this.labelControl1.Text = "数据宽度:";
            // 
            // txtFrameWidth
            // 
            this.txtFrameWidth.Location = new System.Drawing.Point(104, 55);
            this.txtFrameWidth.Name = "txtFrameWidth";
            this.txtFrameWidth.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtFrameWidth.Properties.MaxLength = 2;
            this.txtFrameWidth.Size = new System.Drawing.Size(86, 20);
            this.txtFrameWidth.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(192, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 14);
            this.label3.TabIndex = 134;
            this.label3.Text = "字节";
            // 
            // TreeNodeInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(238, 211);
            this.Controls.Add(this.cbPrecision);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.txtUnit);
            this.Controls.Add(this.txtMaxMeasureRFlag);
            this.Controls.Add(this.txtFrameWidth);
            this.Controls.Add(this.txtMaxMeasureR);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl9);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.labelControl10);
            this.Controls.Add(this.labelControl8);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TreeNodeInfoForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.TreeNodeInfoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cbPrecision.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUnit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxMeasureRFlag.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxMeasureR.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFrameWidth.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ComboBoxEdit cbPrecision;
        private DevExpress.XtraEditors.ComboBoxEdit cbType;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.TextEdit txtUnit;
        private DevExpress.XtraEditors.TextEdit txtMaxMeasureRFlag;
        private DevExpress.XtraEditors.TextEdit txtMaxMeasureR;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.TextEdit txtName;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtFrameWidth;
        private System.Windows.Forms.Label label3;
    }
}