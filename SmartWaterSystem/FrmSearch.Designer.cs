namespace SmartWaterSystem
{
    partial class FrmSearch
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnSearchNext = new DevExpress.XtraEditors.SimpleButton();
            this.rdReverse = new DevExpress.XtraEditors.RadioGroup();
            this.ceCase = new DevExpress.XtraEditors.CheckEdit();
            this.txtContent = new DevExpress.XtraEditors.TextEdit();
            this.btnCancle = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.rdReverse.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceCase.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtContent.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(22, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(52, 14);
            this.labelControl1.TabIndex = 99;
            this.labelControl1.Text = "查找内容:";
            // 
            // btnSearchNext
            // 
            this.btnSearchNext.Location = new System.Drawing.Point(280, 6);
            this.btnSearchNext.Name = "btnSearchNext";
            this.btnSearchNext.Size = new System.Drawing.Size(75, 23);
            this.btnSearchNext.TabIndex = 3;
            this.btnSearchNext.Text = "查找下一个";
            this.btnSearchNext.Click += new System.EventHandler(this.btnSearchNext_Click);
            // 
            // rdReverse
            // 
            this.rdReverse.Location = new System.Drawing.Point(123, 39);
            this.rdReverse.Name = "rdReverse";
            this.rdReverse.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "向上"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "向下")});
            this.rdReverse.Properties.ItemsLayout = DevExpress.XtraEditors.RadioGroupItemsLayout.Flow;
            this.rdReverse.Size = new System.Drawing.Size(111, 26);
            this.rdReverse.TabIndex = 2;
            // 
            // ceCase
            // 
            this.ceCase.Location = new System.Drawing.Point(22, 43);
            this.ceCase.Name = "ceCase";
            this.ceCase.Properties.Caption = "区分大小写";
            this.ceCase.Size = new System.Drawing.Size(86, 19);
            this.ceCase.TabIndex = 1;
            // 
            // txtContent
            // 
            this.txtContent.Location = new System.Drawing.Point(100, 8);
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(174, 20);
            this.txtContent.TabIndex = 0;
            this.txtContent.EditValueChanged += new System.EventHandler(this.txtContent_EditValueChanged);
            // 
            // btnCancle
            // 
            this.btnCancle.Location = new System.Drawing.Point(280, 41);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(75, 23);
            this.btnCancle.TabIndex = 4;
            this.btnCancle.Text = "取消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // FrmSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 77);
            this.ControlBox = false;
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.ceCase);
            this.Controls.Add(this.rdReverse);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnSearchNext);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FrmSearch";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "查找";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmSearch_FormClosed);
            this.Load += new System.EventHandler(this.FrmSearch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rdReverse.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceCase.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtContent.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnSearchNext;
        private DevExpress.XtraEditors.RadioGroup rdReverse;
        private DevExpress.XtraEditors.CheckEdit ceCase;
        private DevExpress.XtraEditors.TextEdit txtContent;
        private DevExpress.XtraEditors.SimpleButton btnCancle;
    }
}