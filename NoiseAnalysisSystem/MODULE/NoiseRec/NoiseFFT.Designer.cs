namespace NoiseAnalysisSystem
{
    partial class NoiseFFT
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
            this.components = new System.ComponentModel.Container();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnSelect = new DevExpress.XtraEditors.SimpleButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnSelect1 = new DevExpress.XtraEditors.SimpleButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.txtDis = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.winChartViewer1 = new ChartDirector.WinChartViewer();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer1)).BeginInit();
            this.SuspendLayout();
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(546, 333);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(67, 23);
            this.btnSelect.TabIndex = 16;
            this.btnSelect.Text = "展示";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "*.txt";
            this.openFileDialog.Filter = "txt文件|*.txt";
            // 
            // btnSelect1
            // 
            this.btnSelect1.Location = new System.Drawing.Point(458, 333);
            this.btnSelect1.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelect1.Name = "btnSelect1";
            this.btnSelect1.Size = new System.Drawing.Size(67, 23);
            this.btnSelect1.TabIndex = 17;
            this.btnSelect1.Text = "分析";
            this.btnSelect1.Click += new System.EventHandler(this.btnSelect1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(201, 336);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 14);
            this.label1.TabIndex = 18;
            this.label1.Text = "傅里叶数组大小:";
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(310, 334);
            this.txtSize.Margin = new System.Windows.Forms.Padding(2);
            this.txtSize.Name = "txtSize";
            this.txtSize.ReadOnly = true;
            this.txtSize.Size = new System.Drawing.Size(68, 22);
            this.txtSize.TabIndex = 19;
            this.txtSize.Text = "256";
            // 
            // txtDis
            // 
            this.txtDis.Location = new System.Drawing.Point(98, 334);
            this.txtDis.Margin = new System.Windows.Forms.Padding(2);
            this.txtDis.Name = "txtDis";
            this.txtDis.ReadOnly = true;
            this.txtDis.Size = new System.Drawing.Size(68, 22);
            this.txtDis.TabIndex = 22;
            this.txtDis.Text = "7.8125";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 337);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 14);
            this.label2.TabIndex = 21;
            this.label2.Text = "频率分辨率:";
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.panelControl1.Controls.Add(this.winChartViewer1);
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(701, 316);
            this.panelControl1.TabIndex = 24;
            // 
            // winChartViewer1
            // 
            this.winChartViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.winChartViewer1.Location = new System.Drawing.Point(2, 2);
            this.winChartViewer1.Margin = new System.Windows.Forms.Padding(2);
            this.winChartViewer1.Name = "winChartViewer1";
            this.winChartViewer1.Size = new System.Drawing.Size(697, 312);
            this.winChartViewer1.TabIndex = 1;
            this.winChartViewer1.TabStop = false;
            this.winChartViewer1.ZoomDirection = ChartDirector.WinChartDirection.HorizontalVertical;
            this.winChartViewer1.ClickHotSpot += new ChartDirector.WinHotSpotEventHandler(this.winChartViewer1_ClickHotSpot);
            this.winChartViewer1.ViewPortChanged += new ChartDirector.WinViewPortEventHandler(this.winChartViewer1_ViewPortChanged);
            // 
            // NoiseFFT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.txtDis);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSize);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSelect1);
            this.Controls.Add(this.btnSelect);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "NoiseFFT";
            this.Size = new System.Drawing.Size(704, 362);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.ErrorProvider errorProvider;
        private DevExpress.XtraEditors.SimpleButton btnSelect;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private DevExpress.XtraEditors.SimpleButton btnSelect1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.TextBox txtDis;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private ChartDirector.WinChartViewer winChartViewer1;



    }
}