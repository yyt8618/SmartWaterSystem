namespace NoiseAnalysisSystem
{
    partial class TestChart
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
            this.winChartViewer1 = new ChartDirector.WinChartViewer();
            this.btnSelect = new DevExpress.XtraEditors.SimpleButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.txtStandValue = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer1)).BeginInit();
            this.SuspendLayout();
            // 
            // winChartViewer1
            // 
            this.winChartViewer1.Location = new System.Drawing.Point(0, 0);
            this.winChartViewer1.Margin = new System.Windows.Forms.Padding(2);
            this.winChartViewer1.Name = "winChartViewer1";
            this.winChartViewer1.Size = new System.Drawing.Size(697, 326);
            this.winChartViewer1.TabIndex = 2;
            this.winChartViewer1.TabStop = false;
            this.winChartViewer1.ZoomDirection = ChartDirector.WinChartDirection.HorizontalVertical;
            this.winChartViewer1.ViewPortChanged += new ChartDirector.WinViewPortEventHandler(this.winChartViewer1_ViewPortChanged);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(552, 330);
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
            this.txtStandValue.Location = new System.Drawing.Point(320, 330);
            this.txtStandValue.Margin = new System.Windows.Forms.Padding(2);
            this.txtStandValue.Name = "txtStandValue";
            this.txtStandValue.ReadOnly = true;
            this.txtStandValue.Size = new System.Drawing.Size(68, 22);
            this.txtStandValue.TabIndex = 21;
            // 
            // TestChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 364);
            this.Controls.Add(this.txtStandValue);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.winChartViewer1);
            this.Name = "TestChart";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TestChart";
            this.Load += new System.EventHandler(this.TestChart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ChartDirector.WinChartViewer winChartViewer1;
        private DevExpress.XtraEditors.SimpleButton btnSelect;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TextBox txtStandValue;

    }
}