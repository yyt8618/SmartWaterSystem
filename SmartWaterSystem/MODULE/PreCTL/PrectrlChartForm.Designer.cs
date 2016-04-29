namespace SmartWaterSystem
{
    partial class PrectrlChartForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series11 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series12 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series13 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series14 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series15 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title3 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.lstDetailView = new System.Windows.Forms.ListView();
            this.colTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEnPre = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colOutletPre = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colForFlow = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colRevFlow = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colInstantFlow = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBoxList = new System.Windows.Forms.GroupBox();
            this.groupBoxChart = new System.Windows.Forms.GroupBox();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.cbInterval = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnAnalysis = new DevExpress.XtraEditors.SimpleButton();
            this.btnGraph = new DevExpress.XtraEditors.SimpleButton();
            this.groupBoxList.SuspendLayout();
            this.groupBoxChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbInterval.Properties)).BeginInit();
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
            this.colEnPre,
            this.colOutletPre,
            this.colForFlow,
            this.colRevFlow,
            this.colInstantFlow});
            this.lstDetailView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstDetailView.Font = new System.Drawing.Font("宋体", 11F);
            this.lstDetailView.FullRowSelect = true;
            this.lstDetailView.GridLines = true;
            this.lstDetailView.Location = new System.Drawing.Point(3, 18);
            this.lstDetailView.MultiSelect = false;
            this.lstDetailView.Name = "lstDetailView";
            this.lstDetailView.ShowGroups = false;
            this.lstDetailView.Size = new System.Drawing.Size(876, 509);
            this.lstDetailView.TabIndex = 5;
            this.lstDetailView.TabStop = false;
            this.lstDetailView.UseCompatibleStateImageBehavior = false;
            this.lstDetailView.View = System.Windows.Forms.View.Details;
            // 
            // colTime
            // 
            this.colTime.Text = "时间";
            this.colTime.Width = 140;
            // 
            // colEnPre
            // 
            this.colEnPre.Text = "进口压力";
            this.colEnPre.Width = 140;
            // 
            // colOutletPre
            // 
            this.colOutletPre.Text = "出口压力";
            this.colOutletPre.Width = 140;
            // 
            // colForFlow
            // 
            this.colForFlow.Text = "正向流量";
            this.colForFlow.Width = 140;
            // 
            // colRevFlow
            // 
            this.colRevFlow.Text = "反向流量";
            this.colRevFlow.Width = 140;
            // 
            // colInstantFlow
            // 
            this.colInstantFlow.Text = "瞬时流量";
            this.colInstantFlow.Width = 140;
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
            this.label4.Location = new System.Drawing.Point(544, 13);
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
            this.groupBoxList.Size = new System.Drawing.Size(882, 530);
            this.groupBoxList.TabIndex = 22;
            this.groupBoxList.TabStop = false;
            this.groupBoxList.Text = "列表";
            // 
            // groupBoxChart
            // 
            this.groupBoxChart.Controls.Add(this.chart);
            this.groupBoxChart.Location = new System.Drawing.Point(1, 40);
            this.groupBoxChart.Name = "groupBoxChart";
            this.groupBoxChart.Size = new System.Drawing.Size(883, 530);
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
            chartArea3.Area3DStyle.IsClustered = true;
            chartArea3.Area3DStyle.IsRightAngleAxes = false;
            chartArea3.Area3DStyle.LightStyle = System.Windows.Forms.DataVisualization.Charting.LightStyle.Realistic;
            chartArea3.Area3DStyle.Rotation = 25;
            chartArea3.Area3DStyle.WallWidth = 3;
            chartArea3.AxisX.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Lines;
            chartArea3.AxisX.IsLabelAutoFit = false;
            chartArea3.AxisX.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            chartArea3.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea3.AxisX.Title = "时间";
            chartArea3.AxisY.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Lines;
            chartArea3.AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea3.AxisY.IsLabelAutoFit = false;
            chartArea3.AxisY.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            chartArea3.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea3.AxisY.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            chartArea3.AxisY.Title = "数据";
            chartArea3.BackColor = System.Drawing.Color.SeaShell;
            chartArea3.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            chartArea3.BackSecondaryColor = System.Drawing.Color.White;
            chartArea3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea3.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea3.CursorX.IsUserEnabled = true;
            chartArea3.CursorX.IsUserSelectionEnabled = true;
            chartArea3.InnerPlotPosition.Auto = false;
            chartArea3.InnerPlotPosition.Height = 82.55958F;
            chartArea3.InnerPlotPosition.Width = 84.2484F;
            chartArea3.InnerPlotPosition.X = 12.7516F;
            chartArea3.InnerPlotPosition.Y = 10.04953F;
            chartArea3.Name = "Default";
            chartArea3.Position.Auto = false;
            chartArea3.Position.Height = 86.07874F;
            chartArea3.Position.Width = 90.63375F;
            chartArea3.Position.X = 4.346499F;
            chartArea3.Position.Y = 7.968504F;
            chartArea3.ShadowColor = System.Drawing.Color.Transparent;
            this.chart.ChartAreas.Add(chartArea3);
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.BackColor = System.Drawing.Color.Transparent;
            legend3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            legend3.IsTextAutoFit = false;
            legend3.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Row;
            legend3.Name = "Legend1";
            legend3.Position.Auto = false;
            legend3.Position.Height = 5.542725F;
            legend3.Position.Width = 50.86185F;
            legend3.Position.X = 25F;
            legend3.Position.Y = 8F;
            this.chart.Legends.Add(legend3);
            this.chart.Location = new System.Drawing.Point(3, 18);
            this.chart.Name = "chart";
            series11.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
            series11.BorderWidth = 2;
            series11.ChartArea = "Default";
            series11.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series11.Color = System.Drawing.Color.Red;
            series11.Legend = "Legend1";
            series11.Name = "进口压力";
            series11.ShadowColor = System.Drawing.Color.Black;
            series11.ShadowOffset = 1;
            series12.BorderWidth = 2;
            series12.ChartArea = "Default";
            series12.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series12.Color = System.Drawing.Color.DeepSkyBlue;
            series12.Legend = "Legend1";
            series12.Name = "出口压力";
            series12.ShadowOffset = 1;
            series13.ChartArea = "Default";
            series13.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series13.Legend = "Legend1";
            series13.Name = "正向流量";
            series14.ChartArea = "Default";
            series14.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series14.Legend = "Legend1";
            series14.Name = "反向流量";
            series15.ChartArea = "Default";
            series15.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series15.Legend = "Legend1";
            series15.Name = "瞬时流量";
            this.chart.Series.Add(series11);
            this.chart.Series.Add(series12);
            this.chart.Series.Add(series13);
            this.chart.Series.Add(series14);
            this.chart.Series.Add(series15);
            this.chart.Size = new System.Drawing.Size(877, 509);
            this.chart.TabIndex = 10;
            title3.BackColor = System.Drawing.Color.Transparent;
            title3.Font = new System.Drawing.Font("宋体", 15F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            title3.Name = "Default";
            title3.Position.Auto = false;
            title3.Position.Width = 90.63375F;
            title3.Position.X = 4.346499F;
            title3.Position.Y = 4.968504F;
            title3.Text = "图表展示";
            this.chart.Titles.Add(title3);
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
            this.cbInterval.Size = new System.Drawing.Size(95, 20);
            this.cbInterval.TabIndex = 2;
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
            // PrectrlChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 570);
            this.Controls.Add(this.btnGraph);
            this.Controls.Add(this.btnAnalysis);
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
            this.Name = "PrectrlChartForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "数据详情";
            this.Load += new System.EventHandler(this.PrectrlChartForm_Load);
            this.groupBoxList.ResumeLayout(false);
            this.groupBoxChart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbInterval.Properties)).EndInit();
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
        private System.Windows.Forms.ColumnHeader colEnPre;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBoxList;
        private System.Windows.Forms.GroupBox groupBoxChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private DevExpress.XtraEditors.ComboBoxEdit cbInterval;
        private DevExpress.XtraEditors.SimpleButton btnAnalysis;
        private DevExpress.XtraEditors.SimpleButton btnGraph;
        private System.Windows.Forms.ColumnHeader colOutletPre;
        private System.Windows.Forms.ColumnHeader colForFlow;
        private System.Windows.Forms.ColumnHeader colRevFlow;
        private System.Windows.Forms.ColumnHeader colInstantFlow;
    }
}