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
            this.comboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox3)).BeginInit();
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
            this.groupBox1.Size = new System.Drawing.Size(217, 117);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.Text = "低频段傅里叶数据区间";
            // 
            // txtMin1
            // 
            this.txtMin1.Location = new System.Drawing.Point(76, 66);
            this.txtMin1.Name = "txtMin1";
            this.txtMin1.Size = new System.Drawing.Size(113, 20);
            this.txtMin1.TabIndex = 58;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 14);
            this.label2.TabIndex = 57;
            this.label2.Text = "下限";
            // 
            // txtMax1
            // 
            this.txtMax1.Location = new System.Drawing.Point(76, 28);
            this.txtMax1.Name = "txtMax1";
            this.txtMax1.Size = new System.Drawing.Size(113, 20);
            this.txtMax1.TabIndex = 56;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(20, 35);
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
            this.groupBox2.Location = new System.Drawing.Point(14, 139);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(218, 106);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.Text = "高频段傅里叶数据区间";
            // 
            // txtMin2
            // 
            this.txtMin2.Location = new System.Drawing.Point(77, 63);
            this.txtMin2.Name = "txtMin2";
            this.txtMin2.Size = new System.Drawing.Size(113, 20);
            this.txtMin2.TabIndex = 60;
            // 
            // txtMax2
            // 
            this.txtMax2.Location = new System.Drawing.Point(77, 27);
            this.txtMax2.Name = "txtMax2";
            this.txtMax2.Size = new System.Drawing.Size(113, 20);
            this.txtMax2.TabIndex = 58;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 14);
            this.label3.TabIndex = 59;
            this.label3.Text = "下限";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 14);
            this.label1.TabIndex = 57;
            this.label1.Text = "上限";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(17, 365);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(66, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "确定";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(176, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 14);
            this.label4.TabIndex = 84;
            this.label4.Text = "Hz";
            // 
            // txtLeakHZ
            // 
            this.txtLeakHZ.Location = new System.Drawing.Point(104, 27);
            this.txtLeakHZ.Name = "txtLeakHZ";
            this.txtLeakHZ.Size = new System.Drawing.Size(65, 20);
            this.txtLeakHZ.TabIndex = 83;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 14);
            this.label5.TabIndex = 82;
            this.label5.Text = "频率分界值";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBox);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.txtLeakHZ);
            this.groupBox3.Location = new System.Drawing.Point(16, 252);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(217, 107);
            this.groupBox3.TabIndex = 85;
            this.groupBox3.Text = "计算参数";
            // 
            // comboBox
            // 
            this.comboBox.Location = new System.Drawing.Point(90, 65);
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new System.Drawing.Size(106, 20);
            this.comboBox.TabIndex = 86;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 14);
            this.label6.TabIndex = 85;
            this.label6.Text = "分析算法";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(167, 365);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(66, 23);
            this.simpleButton1.TabIndex = 86;
            this.simpleButton1.Text = "取消";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // FrmCalcParamSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 399);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FrmCalcParamSet";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "计算参数设置";
            this.Load += new System.EventHandler(this.FrmCalcParamSet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox3)).EndInit();
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
		private DevExpress.XtraEditors.ComboBoxEdit comboBox;
		private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
	}
}