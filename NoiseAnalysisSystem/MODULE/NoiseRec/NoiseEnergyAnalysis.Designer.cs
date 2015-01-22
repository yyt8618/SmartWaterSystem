namespace NoiseAnalysisSystem
{
    partial class NoiseEnergyAnalysis
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
            this.btnSelect = new DevExpress.XtraEditors.SimpleButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.txtEnergyValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStandardAMP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtStandardAverage = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.winChartViewer1 = new ChartDirector.WinChartViewer();
            this.cbRecorders = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbRecorders.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(531, 462);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(67, 23);
            this.btnSelect.TabIndex = 17;
            this.btnSelect.Text = "展示";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "*.txt";
            this.openFileDialog.Filter = "txt文件|*.txt";
            // 
            // txtEnergyValue
            // 
            this.txtEnergyValue.Location = new System.Drawing.Point(290, 462);
            this.txtEnergyValue.Margin = new System.Windows.Forms.Padding(2);
            this.txtEnergyValue.Name = "txtEnergyValue";
            this.txtEnergyValue.ReadOnly = true;
            this.txtEnergyValue.Size = new System.Drawing.Size(68, 22);
            this.txtEnergyValue.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(229, 466);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 14);
            this.label1.TabIndex = 22;
            this.label1.Text = "能量强度:";
            // 
            // txtStandardAMP
            // 
            this.txtStandardAMP.Location = new System.Drawing.Point(151, 462);
            this.txtStandardAMP.Margin = new System.Windows.Forms.Padding(2);
            this.txtStandardAMP.Name = "txtStandardAMP";
            this.txtStandardAMP.ReadOnly = true;
            this.txtStandardAMP.Size = new System.Drawing.Size(68, 22);
            this.txtStandardAMP.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 466);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 14);
            this.label2.TabIndex = 22;
            this.label2.Text = "静态漏水标准幅度值:";
            // 
            // txtStandardAverage
            // 
            this.txtStandardAverage.Location = new System.Drawing.Point(445, 462);
            this.txtStandardAverage.Margin = new System.Windows.Forms.Padding(2);
            this.txtStandardAverage.Name = "txtStandardAverage";
            this.txtStandardAverage.ReadOnly = true;
            this.txtStandardAverage.Size = new System.Drawing.Size(68, 22);
            this.txtStandardAverage.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(371, 466);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 14);
            this.label3.TabIndex = 22;
            this.label3.Text = "标准平均值:";
            // 
            // winChartViewer1
            // 
            this.winChartViewer1.Location = new System.Drawing.Point(0, 0);
            this.winChartViewer1.Margin = new System.Windows.Forms.Padding(2);
            this.winChartViewer1.Name = "winChartViewer1";
            this.winChartViewer1.Size = new System.Drawing.Size(795, 455);
            this.winChartViewer1.TabIndex = 2;
            this.winChartViewer1.TabStop = false;
            this.winChartViewer1.ZoomDirection = ChartDirector.WinChartDirection.HorizontalVertical;
            this.winChartViewer1.ViewPortChanged += new ChartDirector.WinViewPortEventHandler(this.winChartViewer1_ViewPortChanged);
            // 
            // cbRecorders
            // 
            this.cbRecorders.Location = new System.Drawing.Point(625, 463);
            this.cbRecorders.Name = "cbRecorders";
            this.cbRecorders.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbRecorders.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbRecorders.Size = new System.Drawing.Size(144, 20);
            this.cbRecorders.TabIndex = 23;
            this.cbRecorders.SelectedIndexChanged += new System.EventHandler(this.cbRecorders_SelectedIndexChanged);
            // 
            // NoiseEnergyAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbRecorders);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtStandardAMP);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtStandardAverage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtEnergyValue);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.winChartViewer1);
            this.Name = "NoiseEnergyAnalysis";
            this.Size = new System.Drawing.Size(797, 494);
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbRecorders.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ChartDirector.WinChartViewer winChartViewer1;
        private DevExpress.XtraEditors.SimpleButton btnSelect;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TextBox txtEnergyValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtStandardAMP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtStandardAverage;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.ComboBoxEdit cbRecorders;

    }
}