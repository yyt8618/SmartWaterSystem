namespace SmartWaterSystem
{
    partial class PluseBasicInputForm
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
            this.txtPluseBasic1 = new DevExpress.XtraEditors.TextEdit();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtPluseBasic2 = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtPluseBasic3 = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtPluseBasic4 = new DevExpress.XtraEditors.TextEdit();
            this.btnCancle = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtPluseBasic1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPluseBasic2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPluseBasic3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPluseBasic4.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 14);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(100, 14);
            this.labelControl2.TabIndex = 130;
            this.labelControl2.Text = "第一路脉冲基准数:";
            // 
            // txtPluseBasic1
            // 
            this.txtPluseBasic1.Location = new System.Drawing.Point(118, 11);
            this.txtPluseBasic1.Name = "txtPluseBasic1";
            this.txtPluseBasic1.Properties.MaxLength = 10;
            this.txtPluseBasic1.Size = new System.Drawing.Size(108, 20);
            this.txtPluseBasic1.TabIndex = 0;
            this.txtPluseBasic1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_fourbyte_KeyPress);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(36, 142);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "确定";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 47);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(100, 14);
            this.labelControl1.TabIndex = 130;
            this.labelControl1.Text = "第二路脉冲基准数:";
            // 
            // txtPluseBasic2
            // 
            this.txtPluseBasic2.Location = new System.Drawing.Point(118, 44);
            this.txtPluseBasic2.Name = "txtPluseBasic2";
            this.txtPluseBasic2.Properties.MaxLength = 10;
            this.txtPluseBasic2.Size = new System.Drawing.Size(108, 20);
            this.txtPluseBasic2.TabIndex = 1;
            this.txtPluseBasic2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_fourbyte_KeyPress);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(12, 80);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(100, 14);
            this.labelControl3.TabIndex = 130;
            this.labelControl3.Text = "第三路脉冲基准数:";
            // 
            // txtPluseBasic3
            // 
            this.txtPluseBasic3.Location = new System.Drawing.Point(118, 77);
            this.txtPluseBasic3.Name = "txtPluseBasic3";
            this.txtPluseBasic3.Properties.MaxLength = 10;
            this.txtPluseBasic3.Size = new System.Drawing.Size(108, 20);
            this.txtPluseBasic3.TabIndex = 2;
            this.txtPluseBasic3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_fourbyte_KeyPress);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(12, 113);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(100, 14);
            this.labelControl4.TabIndex = 130;
            this.labelControl4.Text = "第四路脉冲基准数:";
            // 
            // txtPluseBasic4
            // 
            this.txtPluseBasic4.Location = new System.Drawing.Point(118, 110);
            this.txtPluseBasic4.Name = "txtPluseBasic4";
            this.txtPluseBasic4.Properties.MaxLength = 10;
            this.txtPluseBasic4.Size = new System.Drawing.Size(108, 20);
            this.txtPluseBasic4.TabIndex = 3;
            this.txtPluseBasic4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_fourbyte_KeyPress);
            // 
            // btnCancle
            // 
            this.btnCancle.Location = new System.Drawing.Point(127, 142);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(75, 23);
            this.btnCancle.TabIndex = 5;
            this.btnCancle.Text = "取消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // PluseBasicInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(238, 177);
            this.Controls.Add(this.txtPluseBasic4);
            this.Controls.Add(this.txtPluseBasic3);
            this.Controls.Add(this.txtPluseBasic2);
            this.Controls.Add(this.txtPluseBasic1);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PluseBasicInputForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "设置脉冲基准数";
            this.Load += new System.EventHandler(this.PluseBasicInputForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtPluseBasic1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPluseBasic2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPluseBasic3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPluseBasic4.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtPluseBasic1;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtPluseBasic2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txtPluseBasic3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txtPluseBasic4;
        private DevExpress.XtraEditors.SimpleButton btnCancle;
    }
}