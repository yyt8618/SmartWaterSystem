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
            this.txtStandValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStandardAMP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtStandardAverage = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.winChartViewer1 = new ChartDirector.WinChartViewer();
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(564, 336);
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
            // txtStandValue
            // 
            this.txtStandValue.Location = new System.Drawing.Point(307, 336);
            this.txtStandValue.Margin = new System.Windows.Forms.Padding(2);
            this.txtStandValue.Name = "txtStandValue";
            this.txtStandValue.ReadOnly = true;
            this.txtStandValue.Size = new System.Drawing.Size(68, 22);
            this.txtStandValue.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(246, 340);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 14);
            this.label1.TabIndex = 22;
            this.label1.Text = "能量强度:";
            // 
            // txtStandardAMP
            // 
            this.txtStandardAMP.Location = new System.Drawing.Point(148, 336);
            this.txtStandardAMP.Margin = new System.Windows.Forms.Padding(2);
            this.txtStandardAMP.Name = "txtStandardAMP";
            this.txtStandardAMP.ReadOnly = true;
            this.txtStandardAMP.Size = new System.Drawing.Size(68, 22);
            this.txtStandardAMP.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 340);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 14);
            this.label2.TabIndex = 22;
            this.label2.Text = "静态漏水标准幅度值:";
            // 
            // txtStandardAverage
            // 
            this.txtStandardAverage.Location = new System.Drawing.Point(478, 336);
            this.txtStandardAverage.Margin = new System.Windows.Forms.Padding(2);
            this.txtStandardAverage.Name = "txtStandardAverage";
            this.txtStandardAverage.ReadOnly = true;
            this.txtStandardAverage.Size = new System.Drawing.Size(68, 22);
            this.txtStandardAverage.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(404, 340);
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
            this.winChartViewer1.Size = new System.Drawing.Size(857, 332);
            this.winChartViewer1.TabIndex = 2;
            this.winChartViewer1.TabStop = false;
            this.winChartViewer1.ZoomDirection = ChartDirector.WinChartDirection.HorizontalVertical;
            this.winChartViewer1.ViewPortChanged += new ChartDirector.WinViewPortEventHandler(this.winChartViewer1_ViewPortChanged);
            // 
            // NoiseEnergyAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(855, 364);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtStandardAMP);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtStandardAverage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtStandValue);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.winChartViewer1);
            this.Name = "NoiseEnergyAnalysis";
            this.Text = "漏水能量变化分析";
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ChartDirector.WinChartViewer winChartViewer1;
        private DevExpress.XtraEditors.SimpleButton btnSelect;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TextBox txtStandValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtStandardAMP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtStandardAverage;
        private System.Windows.Forms.Label label3;

    }
}