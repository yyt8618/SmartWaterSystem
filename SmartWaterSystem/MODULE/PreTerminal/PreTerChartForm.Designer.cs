namespace SmartWaterSystem
{
    partial class PreTerChartForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.lstDetailView = new System.Windows.Forms.ListView();
            this.colTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBoxList = new System.Windows.Forms.GroupBox();
            this.groupBoxChart = new System.Windows.Forms.GroupBox();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.cbInterval = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbDataType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnAnalysis = new DevExpress.XtraEditors.SimpleButton();
            this.btnGraph = new DevExpress.XtraEditors.SimpleButton();
            this.groupBoxList.SuspendLayout();
            this.groupBoxChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbInterval.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbDataType.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(218, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 14);
            this.label3.TabIndex = 21;
            this.label3.Text = "-";
            // 
            // dtpEnd
            // 
            this.dtpEnd.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEnd.Location = new System.Drawing.Point(231, 8);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(140, 22);
            this.dtpEnd.TabIndex = 1;
            this.dtpEnd.Value = new System.DateTime(2015, 1, 1, 0, 0, 0, 0);
            // 
            // dtpStart
            // 
            this.dtpStart.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStart.Location = new System.Drawing.Point(76, 8);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(140, 22);
            this.dtpStart.TabIndex = 0;
            this.dtpStart.Value = new System.DateTime(2015, 1, 1, 0, 0, 0, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 14);
            this.label2.TabIndex = 18;
            this.label2.Text = "选择日期：";
            // 
            // lstDetailView
            // 
            this.lstDetailView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTime,
            this.colname});
            this.lstDetailView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstDetailView.Font = new System.Drawing.Font("宋体", 11F);
            this.lstDetailView.FullRowSelect = true;
            this.lstDetailView.GridLines = true;
            this.lstDetailView.Location = new System.Drawing.Point(3, 18);
            this.lstDetailView.MultiSelect = false;
            this.lstDetailView.Name = "lstDetailView";
            this.lstDetailView.ShowGroups = false;
            this.lstDetailView.Size = new System.Drawing.Size(804, 456);
            this.lstDetailView.TabIndex = 5;
            this.lstDetailView.TabStop = false;
            this.lstDetailView.UseCompatibleStateImageBehavior = false;
            this.lstDetailView.View = System.Windows.Forms.View.Details;
            // 
            // colTime
            // 
            this.colTime.Text = "时间";
            this.colTime.Width = 326;
            // 
            // colname
            // 
            this.colname.Text = "Name";
            this.colname.Width = 344;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(376, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 14);
            this.label1.TabIndex = 18;
            this.label1.Text = "时间间隔：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(511, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 14);
            this.label4.TabIndex = 18;
            this.label4.Text = "分钟";
            // 
            // groupBoxList
            // 
            this.groupBoxList.Controls.Add(this.lstDetailView);
            this.groupBoxList.Location = new System.Drawing.Point(1, 41);
            this.groupBoxList.Name = "groupBoxList";
            this.groupBoxList.Size = new System.Drawing.Size(810, 477);
            this.groupBoxList.TabIndex = 22;
            this.groupBoxList.TabStop = false;
            this.groupBoxList.Text = "列表";
            // 
            // groupBoxChart
            // 
            this.groupBoxChart.Controls.Add(this.chart);
            this.groupBoxChart.Location = new System.Drawing.Point(1, 41);
            this.groupBoxChart.Name = "groupBoxChart";
            this.groupBoxChart.Size = new System.Drawing.Size(810, 477);
            this.groupBoxChart.TabIndex = 22;
            this.groupBoxChart.TabStop = false;
            this.groupBoxChart.Text = "图表";
            // 
            // chart
            // 
            this.chart.BackColor = System.Drawing.SystemColors.Control;
            this.chart.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            this.chart.BackSecondaryColor = System.Drawing.Color.White;
            this.chart.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            this.chart.BorderlineWidth = 2;
            chartArea1.Area3DStyle.IsClustered = true;
            chartArea1.Area3DStyle.IsRightAngleAxes = false;
            chartArea1.Area3DStyle.LightStyle = System.Windows.Forms.DataVisualization.Charting.LightStyle.Realistic;
            chartArea1.Area3DStyle.Rotation = 25;
            chartArea1.Area3DStyle.WallWidth = 3;
            chartArea1.AxisX.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Lines;
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisX.Title = "时间";
            chartArea1.AxisY.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Lines;
            chartArea1.AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.AxisY.IsLabelAutoFit = false;
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisY.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            chartArea1.AxisY.Title = "数据";
            chartArea1.BackColor = System.Drawing.Color.SeaShell;
            chartArea1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            chartArea1.BackSecondaryColor = System.Drawing.Color.White;
            chartArea1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.CursorX.IsUserEnabled = true;
            chartArea1.CursorX.IsUserSelectionEnabled = true;
            chartArea1.InnerPlotPosition.Auto = false;
            chartArea1.InnerPlotPosition.Height = 82.55958F;
            chartArea1.InnerPlotPosition.Width = 84.2484F;
            chartArea1.InnerPlotPosition.X = 12.7516F;
            chartArea1.InnerPlotPosition.Y = 10.04953F;
            chartArea1.Name = "Default";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 86.07874F;
            chartArea1.Position.Width = 90.63375F;
            chartArea1.Position.X = 4.346499F;
            chartArea1.Position.Y = 7.968504F;
            chartArea1.ShadowColor = System.Drawing.Color.Transparent;
            this.chart.ChartAreas.Add(chartArea1);
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.BackColor = System.Drawing.Color.Transparent;
            legend1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            legend1.IsTextAutoFit = false;
            legend1.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Row;
            legend1.Name = "Legend1";
            legend1.Position.Auto = false;
            legend1.Position.Height = 5.542725F;
            legend1.Position.Width = 50.86185F;
            legend1.Position.X = 25F;
            legend1.Position.Y = 8F;
            this.chart.Legends.Add(legend1);
            this.chart.Location = new System.Drawing.Point(3, 18);
            this.chart.Name = "chart";
            series1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
            series1.BorderWidth = 2;
            series1.ChartArea = "Default";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = System.Drawing.Color.Red;
            series1.Legend = "Legend1";
            series1.Name = "数据1";
            series1.ShadowColor = System.Drawing.Color.Black;
            series1.ShadowOffset = 1;
            series2.BorderWidth = 2;
            series2.ChartArea = "Default";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = System.Drawing.Color.DeepSkyBlue;
            series2.Legend = "Legend1";
            series2.Name = "数据2";
            series2.ShadowOffset = 1;
            this.chart.Series.Add(series1);
            this.chart.Series.Add(series2);
            this.chart.Size = new System.Drawing.Size(804, 456);
            this.chart.TabIndex = 10;
            title1.BackColor = System.Drawing.Color.Transparent;
            title1.Font = new System.Drawing.Font("宋体", 15F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            title1.Name = "Default";
            title1.Position.Auto = false;
            title1.Position.Width = 90.63375F;
            title1.Position.X = 4.346499F;
            title1.Position.Y = 4.968504F;
            title1.Text = "图表展示";
            this.chart.Titles.Add(title1);
            this.chart.GetToolTipText += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs>(this.chart_GetToolTipText);
            // 
            // cbInterval
            // 
            this.cbInterval.Location = new System.Drawing.Point(443, 10);
            this.cbInterval.Name = "cbInterval";
            this.cbInterval.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbInterval.Properties.Items.AddRange(new object[] {
            "1",
            "5",
            "15",
            "30",
            "60",
            "120"});
            this.cbInterval.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbInterval.Size = new System.Drawing.Size(65, 20);
            this.cbInterval.TabIndex = 2;
            // 
            // cbDataType
            // 
            this.cbDataType.Location = new System.Drawing.Point(543, 9);
            this.cbDataType.Name = "cbDataType";
            this.cbDataType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbDataType.Properties.Items.AddRange(new object[] {
            "压力",
            "正向流量",
            "反向流量",
            "瞬时流量"});
            this.cbDataType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbDataType.Size = new System.Drawing.Size(87, 20);
            this.cbDataType.TabIndex = 3;
            this.cbDataType.SelectedIndexChanged += new System.EventHandler(this.cbDataType_SelectedIndexChanged);
            // 
            // btnAnalysis
            // 
            this.btnAnalysis.Location = new System.Drawing.Point(632, 7);
            this.btnAnalysis.Name = "btnAnalysis";
            this.btnAnalysis.Size = new System.Drawing.Size(87, 27);
            this.btnAnalysis.TabIndex = 4;
            this.btnAnalysis.Text = "分析";
            this.btnAnalysis.Click += new System.EventHandler(this.btnAnalysis_Click);
            // 
            // btnGraph
            // 
            this.btnGraph.Location = new System.Drawing.Point(721, 7);
            this.btnGraph.Name = "btnGraph";
            this.btnGraph.Size = new System.Drawing.Size(87, 27);
            this.btnGraph.TabIndex = 5;
            this.btnGraph.Text = "图表";
            this.btnGraph.Click += new System.EventHandler(this.btnGraph_Click);
            // 
            // PreTerChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 518);
            this.Controls.Add(this.btnGraph);
            this.Controls.Add(this.btnAnalysis);
            this.Controls.Add(this.cbDataType);
            this.Controls.Add(this.cbInterval);
            this.Controls.Add(this.groupBoxChart);
            this.Controls.Add(this.groupBoxList);
            this.Controls.Add(this.dtpEnd);
            this.Controls.Add(this.dtpStart);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PreTerChartForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "数据详情";
            this.Load += new System.EventHandler(this.PreTerChartForm_Load);
            this.groupBoxList.ResumeLayout(false);
            this.groupBoxChart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbInterval.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbDataType.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView lstDetailView;
        private System.Windows.Forms.ColumnHeader colTime;
        private System.Windows.Forms.ColumnHeader colname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBoxList;
        private System.Windows.Forms.GroupBox groupBoxChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private DevExpress.XtraEditors.ComboBoxEdit cbInterval;
        private DevExpress.XtraEditors.ComboBoxEdit cbDataType;
        private DevExpress.XtraEditors.SimpleButton btnAnalysis;
        private DevExpress.XtraEditors.SimpleButton btnGraph;
    }
}