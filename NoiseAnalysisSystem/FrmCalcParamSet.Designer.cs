namespace NoiseAnalysisSystem
{
	partial class FrmCalcParamSet
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
            this.groupBox1 = new DevExpress.XtraEditors.GroupControl();
            this.txtMin1 = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMax1 = new DevExpress.XtraEditors.TextEdit();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox2 = new DevExpress.XtraEditors.GroupControl();
            this.txtMin2 = new DevExpress.XtraEditors.TextEdit();
            this.txtMax2 = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLeakHZ = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new DevExpress.XtraEditors.GroupControl();
            this.cbArith = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtDCCompLen = new DevExpress.XtraEditors.TextEdit();
            this.txtStandardAMP = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMin1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMax1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox2)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMin2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMax2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLeakHZ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox3)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDCCompLen.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStandardAMP.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtMin1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtMax1);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Location = new System.Drawing.Point(15, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(217, 92);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.Text = "低频段傅里叶数据区间";
            // 
            // txtMin1
            // 
            this.txtMin1.Location = new System.Drawing.Point(76, 64);
            this.txtMin1.Name = "txtMin1";
            this.txtMin1.Properties.Mask.EditMask = "f0";
            this.txtMin1.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtMin1.Size = new System.Drawing.Size(113, 20);
            this.txtMin1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 14);
            this.label2.TabIndex = 57;
            this.label2.Text = "下限";
            // 
            // txtMax1
            // 
            this.txtMax1.Location = new System.Drawing.Point(76, 33);
            this.txtMax1.Name = "txtMax1";
            this.txtMax1.Properties.Mask.EditMask = "f0";
            this.txtMax1.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtMax1.Size = new System.Drawing.Size(113, 20);
            this.txtMax1.TabIndex = 0;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(20, 36);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(31, 14);
            this.label12.TabIndex = 55;
            this.label12.Text = "上限";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtMin2);
            this.groupBox2.Controls.Add(this.txtMax2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(14, 121);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(218, 91);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.Text = "高频段傅里叶数据区间";
            // 
            // txtMin2
            // 
            this.txtMin2.Location = new System.Drawing.Point(77, 63);
            this.txtMin2.Name = "txtMin2";
            this.txtMin2.Properties.Mask.EditMask = "f0";
            this.txtMin2.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtMin2.Size = new System.Drawing.Size(113, 20);
            this.txtMin2.TabIndex = 1;
            // 
            // txtMax2
            // 
            this.txtMax2.Location = new System.Drawing.Point(77, 32);
            this.txtMax2.Name = "txtMax2";
            this.txtMax2.Properties.Mask.EditMask = "f0";
            this.txtMax2.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtMax2.Size = new System.Drawing.Size(113, 20);
            this.txtMax2.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 14);
            this.label3.TabIndex = 59;
            this.label3.Text = "下限";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 14);
            this.label1.TabIndex = 57;
            this.label1.Text = "上限";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(14, 377);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(66, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "确定";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(194, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 14);
            this.label4.TabIndex = 84;
            this.label4.Text = "Hz";
            // 
            // txtLeakHZ
            // 
            this.txtLeakHZ.Location = new System.Drawing.Point(122, 27);
            this.txtLeakHZ.Name = "txtLeakHZ";
            this.txtLeakHZ.Properties.Mask.EditMask = "f0";
            this.txtLeakHZ.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtLeakHZ.Size = new System.Drawing.Size(66, 20);
            this.txtLeakHZ.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 14);
            this.label5.TabIndex = 82;
            this.label5.Text = "频率分界值";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbArith);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.txtDCCompLen);
            this.groupBox3.Controls.Add(this.txtStandardAMP);
            this.groupBox3.Controls.Add(this.txtLeakHZ);
            this.groupBox3.Location = new System.Drawing.Point(16, 231);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(217, 140);
            this.groupBox3.TabIndex = 85;
            this.groupBox3.Text = "计算参数";
            // 
            // cbArith
            // 
            this.cbArith.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbArith.Location = new System.Drawing.Point(122, 54);
            this.cbArith.Name = "cbArith";
            this.cbArith.Size = new System.Drawing.Size(92, 22);
            this.cbArith.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(19, 114);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(79, 14);
            this.label9.TabIndex = 85;
            this.label9.Text = "直流分量长度";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 86);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(115, 14);
            this.label7.TabIndex = 85;
            this.label7.Text = "静态漏水标准幅度值";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 14);
            this.label6.TabIndex = 85;
            this.label6.Text = "分析算法";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(195, 86);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(19, 14);
            this.label8.TabIndex = 84;
            this.label8.Text = "%";
            // 
            // txtDCCompLen
            // 
            this.txtDCCompLen.Location = new System.Drawing.Point(122, 111);
            this.txtDCCompLen.Name = "txtDCCompLen";
            this.txtDCCompLen.Properties.Mask.EditMask = "f0";
            this.txtDCCompLen.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtDCCompLen.Size = new System.Drawing.Size(66, 20);
            this.txtDCCompLen.TabIndex = 3;
            // 
            // txtStandardAMP
            // 
            this.txtStandardAMP.Location = new System.Drawing.Point(122, 83);
            this.txtStandardAMP.Name = "txtStandardAMP";
            this.txtStandardAMP.Properties.Mask.EditMask = "f0";
            this.txtStandardAMP.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtStandardAMP.Size = new System.Drawing.Size(66, 20);
            this.txtStandardAMP.TabIndex = 2;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(164, 377);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(66, 23);
            this.simpleButton1.TabIndex = 1;
            this.simpleButton1.Text = "取消";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // FrmCalcParamSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 409);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FrmCalcParamSet";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "计算参数设置";
            this.Load += new System.EventHandler(this.FrmCalcParamSet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMin1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMax1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox2)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMin2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMax2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLeakHZ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox3)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDCCompLen.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStandardAMP.Properties)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.GroupControl groupBox1;
		private DevExpress.XtraEditors.GroupControl groupBox2;
		private DevExpress.XtraEditors.TextEdit txtMax1;
		private System.Windows.Forms.Label label12;
		private DevExpress.XtraEditors.TextEdit txtMin1;
		private System.Windows.Forms.Label label2;
		private DevExpress.XtraEditors.TextEdit txtMin2;
		private DevExpress.XtraEditors.TextEdit txtMax2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private DevExpress.XtraEditors.SimpleButton btnSave;
		private System.Windows.Forms.Label label4;
		private DevExpress.XtraEditors.TextEdit txtLeakHZ;
		private System.Windows.Forms.Label label5;
		private DevExpress.XtraEditors.GroupControl groupBox3;
		private System.Windows.Forms.ComboBox cbArith;
		private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.TextEdit txtStandardAMP;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.TextEdit txtDCCompLen;
	}
}