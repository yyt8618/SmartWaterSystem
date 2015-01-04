using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ChartDirector;
using System.Collections;
using System.IO;
using DevExpress.XtraEditors;

namespace NoiseAnalysisSystem
{
    public partial class NoiseFFT : DevExpress.XtraEditors.XtraForm
    {
        List<double> data;

        bool hasFinishedInitialization;

		double minX = 0;
		double minY = 0;
		double maxX = 0;
		double maxY = 0;

        //private FrmSystem main;

        /// <summary>
        /// 构造函数
        /// </summary>
        public NoiseFFT()
        {
            InitializeComponent();

            data = new List<double>();
            // Initialize the WinChartViewer
            initChartViewer(winChartViewer1);

            // Some Visual Studio versions do not expose the MouseWheel event in the Property window, so we
            // need to set up the event handler with our own code.
			//winChartViewer1.MouseWheel += new MouseEventHandler(winChartViewer1_MouseWheel);

            // Can handle events now
            hasFinishedInitialization = true;

            // Trigger the ViewPortChanged event to draw the chart
            //winChartViewer1.updateViewPort(true, true);
            winChartViewer1.Visible = false;
        }

		private void updateImageMap(WinChartViewer viewer)
		{
			// Include tool tip for the chart
			if (viewer.ImageMap == null)
			{
				viewer.ImageMap = viewer.Chart.getHTMLImageMap("clickable", "",
					"title='[{dataSetName}] Index = {x}, Value = {value}'");
			}
		}

		public void createChart()
		{
			string[] xLabels = null;
			if (data.Count != 0)
			{
				xLabels = new string[data.Count];

				for (int i = 0; i < xLabels.Length; i++)
				{
					xLabels[i] = (i + 1).ToString();
				}
			}

            XYChart c = new XYChart(701, 314);
			c.setBackground(c.linearGradientColor(0, 0, 0, 100, 0x99ccff, 0xffffff), 0x888888);

            //LegendBox legendBox = c.addLegend(450, 80, false, "Arial", 8);
            //legendBox.setAlignment(Chart.BottomCenter);
            //legendBox.setBackground(Chart.Transparent, Chart.Transparent);
            //legendBox.setLineStyleKey();
            //legendBox.setFontSize(8);

			ChartDirector.TextBox title = c.addTitle("傅里叶数据分析", "Arial Bold", 13);
			title.setPos(0, 20);

			c.setPlotArea(60, 60, 610, 210, 0xffffff, -1, -1, c.dashLineColor(
				0xaaaaaa, Chart.DotLine), -1);

			c.xAxis().setLabels(xLabels);
			c.xAxis().setLabelStep(9);
			c.xAxis().setIndent(true);
			c.xAxis().setTitle("数据序号");
			c.yAxis().setTitle("数据值").setAlignment(Chart.TopLeft2);

			LineLayer layer;
            int hzColor = Chart.CColor(Color.DeepPink);
			layer = c.addLineLayer2();
			layer.addDataSet(data.ToArray(), hzColor, "数据值");//.setDataSymbol(Chart.CircleSymbol, 3); ;
			layer.setLineWidth(2);
			layer.setFastLineMode();

			if (minX == maxX)
			{
				c.layout();
				minX = c.xAxis().getMinValue();
				minY = c.yAxis().getMinValue();
				maxX = c.xAxis().getMaxValue();
				maxY = c.yAxis().getMaxValue();
			}
			
			//SetXYChartScale(minX, minY, maxX, maxY, winChartViewer1, c);			
			//winChartViewer1.syncLinearAxisWithViewPort("x", c.xAxis());
			//winChartViewer1.syncLinearAxisWithViewPort("y", c.yAxis());

			winChartViewer1.Chart = c;
			winChartViewer1.ImageMap =
				winChartViewer1.Chart.getHTMLImageMap("clickable", "", "title='数据序号: {xLabel}, {dataSetName}: {value}'");
		}
        
        private void initChartViewer(WinChartViewer viewer)
        {
            // Set the full x range to be the duration of the data
            //viewer.setFullRange("x", timeStamps[0], timeStamps[timeStamps.Length - 1]);

            // Initialize the view port to show the latest 20% of the time range
            winChartViewer1.ViewPortWidth = 1;
            winChartViewer1.ViewPortHeight = 1;

            // Set the maximum zoom to 10 points
            //viewer.ZoomInWidthLimit = 10.0 / timeStamps.Length;

            // Initially set the mouse usage to "Pointer" mode (Drag to Scroll mode)
            //pointerPB.Checked = true;
        }

        private void updateControls(WinChartViewer viewer)
        {
            // In this demo, we need to update the scroll bar to reflect the view port position and
            // width of the view port.
			//hScrollBar1.Enabled = winChartViewer1.ViewPortWidth < 1;
			//hScrollBar1.LargeChange = (int)Math.Ceiling(winChartViewer1.ViewPortWidth *
			//    (hScrollBar1.Maximum - hScrollBar1.Minimum));
			//hScrollBar1.SmallChange = (int)Math.Ceiling(hScrollBar1.LargeChange * 0.1);
			//hScrollBar1.Value = (int)Math.Round(winChartViewer1.ViewPortLeft *
			//    (hScrollBar1.Maximum - hScrollBar1.Minimum)) + hScrollBar1.Minimum;

            // We zoom in or out by 10% depending on the mouse wheel direction.
            double rx = 0; ;

            if (winChartViewer1.MouseUsage == WinChartMouseUsage.ZoomIn)
                rx = 0.9;
            else if (winChartViewer1.MouseUsage == WinChartMouseUsage.ZoomOut)
                rx = 1 / 0.9;
            else return;

            double ry = rx;

            // We do not zoom in beyond the zoom in width or height limit.
            rx = Math.Max(rx, winChartViewer1.ZoomInWidthLimit / winChartViewer1.ViewPortWidth);
            ry = Math.Max(ry, winChartViewer1.ZoomInWidthLimit / winChartViewer1.ViewPortHeight);
            if ((rx == 1) && (ry == 1))
                return;

            XYChart c = (XYChart)winChartViewer1.Chart;
            if (c != null)
            {
                //
                // Set the view port position and size so that it is zoom in/out around the mouse by the 
                // desired ratio.
                //

                double mouseOffset = (winChartViewer1.ChartMouseX - c.getPlotArea().getLeftX()) /
                    (double)c.getPlotArea().getWidth();
                winChartViewer1.ViewPortLeft += mouseOffset * (1 - rx) * winChartViewer1.ViewPortWidth;
                winChartViewer1.ViewPortWidth *= rx;

                double mouseOffsetY = (winChartViewer1.ChartMouseY - c.getPlotArea().getTopY()) /
                    (double)c.getPlotArea().getHeight();
                winChartViewer1.ViewPortTop += mouseOffsetY * (1 - ry) * winChartViewer1.ViewPortHeight;
                winChartViewer1.ViewPortHeight *= ry;

                // Trigger a view port changed event to update the chart
                winChartViewer1.updateViewPort(true, false);
            }
        }
        
        private void winChartViewer1_MouseMovePlotArea(object sender, MouseEventArgs e)
        {
            WinChartViewer viewer = (WinChartViewer)sender;
            //trackLineLegend((XYChart)viewer.Chart, viewer.PlotAreaMouseX);
            //viewer.updateDisplay(false);
			viewer.updateViewPort(true, true);
            // Hide the track cursor when the mouse leaves the plot area
            viewer.removeDynamicLayer("MouseLeavePlotArea");
        }

        private void winChartViewer1_ViewPortChanged(object sender, WinViewPortEventArgs e)
        {
            // In addition to updating the chart, we may also need to update other controls that
            // changes based on the view port.
            updateControls(winChartViewer1);

            // Update the chart if necessary
            if (e.NeedUpdateChart)
                createChart();

			if (e.NeedUpdateImageMap)
				updateImageMap(winChartViewer1);

            // We need to update the track line too. If the mouse is moving on the chart (eg. if 
            // the user drags the mouse on the chart to scroll it), the track line will be updated
            // in the MouseMovePlotArea event. Otherwise, we need to update the track line here.
			//if (!winChartViewer1.IsInMouseMoveEvent)
			//{
			//    //trackLineLegend((XYChart)winChartViewer1.Chart, winChartViewer1.PlotAreaMouseX);
			//    winChartViewer1.updateDisplay();
			//}
        }

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            // When the view port is changed (user drags on the chart to scroll), the scroll bar will get
            // updated. When the scroll bar changes (eg. user drags on the scroll bar), the view port will
            // get updated. This creates an infinite loop. To avoid this, the scroll bar can update the 
            // view port only if the view port is not updating the scroll bar.
            if (hasFinishedInitialization && !winChartViewer1.IsInViewPortChangedEvent)
            {
                // Set the view port based on the scroll bar
				//winChartViewer1.ViewPortLeft = ((double)(hScrollBar1.Value - hScrollBar1.Minimum))
				//    / (hScrollBar1.Maximum - hScrollBar1.Minimum);

                // Trigger a view port changed event to update the chart
                winChartViewer1.updateViewPort(true, false);
            }
        }

        private void winChartViewer1_MouseWheel(object sender, MouseEventArgs e)
        {
            // We zoom in or out by 10% depending on the mouse wheel direction.
            double rx = e.Delta > 0 ? 0.9 : 1 / 0.9;
            double ry = rx;

            // We do not zoom in beyond the zoom in width or height limit.
            rx = Math.Max(rx, winChartViewer1.ZoomInWidthLimit / winChartViewer1.ViewPortWidth);
            ry = Math.Max(ry, winChartViewer1.ZoomInWidthLimit / winChartViewer1.ViewPortHeight);
            if ((rx == 1) && (ry == 1))
                return;

			XYChart c = (XYChart)winChartViewer1.Chart;

            //
            // Set the view port position and size so that it is zoom in/out around the mouse by the 
            // desired ratio.
            //

            double mouseOffset = (winChartViewer1.ChartMouseX - c.getPlotArea().getLeftX()) /
                (double)c.getPlotArea().getWidth();
            winChartViewer1.ViewPortLeft += mouseOffset * (1 - rx) * winChartViewer1.ViewPortWidth;
            winChartViewer1.ViewPortWidth *= rx;

            double mouseOffsetY = (winChartViewer1.ChartMouseY - c.getPlotArea().getTopY()) /
                (double)c.getPlotArea().getHeight();
            winChartViewer1.ViewPortTop += mouseOffsetY * (1 - ry) * winChartViewer1.ViewPortHeight;
            winChartViewer1.ViewPortHeight *= ry;

            // Trigger a view port changed event to update the chart
            winChartViewer1.updateViewPort(true, false);
        }

        private Control activeControl = null;

        private void winChartViewer1_MouseEnter(object sender, EventArgs e)
        {
            // Save the original active control and set the WinChartViewer to be the active control, so that
            // the WinChartViewer can receive the mouse wheel event.
            activeControl = winChartViewer1.FindForm().ActiveControl;
            winChartViewer1.FindForm().ActiveControl = winChartViewer1;

        }

        private void winChartViewer1_MouseLeave(object sender, EventArgs e)
        {
            // Restore the original active control.
            winChartViewer1.FindForm().ActiveControl = activeControl;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
			StreamReader sr = null;
            openFileDialog.FileName = "选择需要展示的文件";
            DialogResult dr = openFileDialog.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
				try
				{
					List<double> d = new List<double>();
					sr = new StreamReader(openFileDialog.FileName);
					while (!sr.EndOfStream)
					{
						string str = sr.ReadLine();
						if (str != string.Empty)
						{
							d.Add(Convert.ToDouble(str));
						}
					}
					sr.Close();
					data = d;

					winChartViewer1.updateViewPort(true, false);
					if (!winChartViewer1.Visible)
						winChartViewer1.Visible = true;
				}
				catch (Exception)
				{
                    XtraMessageBox.Show("文件格式不正确!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				finally
				{
					if (sr != null)
						sr.Close();
				}
            }
        }

        private void btnSelect1_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = "选择需要分析的文件";
            DialogResult dr = openFileDialog.ShowDialog();
            StreamReader sr = null;
            StreamWriter sw = null;
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
				try
				{
					List<double> s = new List<double>();
					double[] db = null;
					sr = new StreamReader(openFileDialog.FileName);
					sw = new StreamWriter(string.Format(GlobalValue.TestPath + @"转换前数据{0}.txt", 99 + 1));
					//string[] str = sr.ReadLine().Split('\t');
					//foreach (string item in str)
					//{
					//    double a = Convert.ToDouble(item);
					//    s.Add(a);
					//    sw.WriteLine(a);
					//}
					//sw.Flush();
					while (!sr.EndOfStream)
					{
						string str = sr.ReadLine();
						double a = Convert.ToDouble(str);
						s.Add(a);
						sw.WriteLine(a);
					}
					sw.Flush();
					sw.Close();

					NoiseDataHandler.num = Convert.ToInt32(txtSize.Text);
					NoiseDataHandler.AmpCalc(99, s.ToArray(), ref db);

					List<double> d = new List<double>();
					sr = new StreamReader(string.Format(GlobalValue.TestPath + "转换后数据{0}.txt", 99 + 1));
					while (!sr.EndOfStream)
					{
						string str1 = sr.ReadLine();
						if (str1 != string.Empty)
						{
							d.Add(Convert.ToDouble(str1));
						}
					}
					sr.Close();
					data = d;
					winChartViewer1.updateViewPort(true, false);
					if (!winChartViewer1.Visible)
						winChartViewer1.Visible = true;
				}
				catch (Exception)
				{
                    XtraMessageBox.Show("文件格式不正确!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
                finally
                {
                    if (sr != null)
                        sr.Close();
                    if (sw != null)
                        sw.Close();
                }
            }
        }

		private void winChartViewer1_ClickHotSpot(object sender, WinHotSpotEventArgs e)
		{
			Hashtable ht = e.GetAttrValues();
			ht.Add("数据值", e.AttrValues["value"]);
			ht.Add("数据序号", e.AttrValues["xLabel"]);

			new FrmParamViewer().Display(sender, e);
		}

		private void SetXYChartScale(double minX, double minY, double maxX, double maxY, WinChartViewer chartViewer, XYChart c)
		{
			double xScaleMin = minX + (maxX - minX) * chartViewer.ViewPortLeft;
			double xScaleMax = minX + (maxX - minX) * (chartViewer.ViewPortLeft + chartViewer.ViewPortWidth);
			double yScaleMin = maxY - (maxY - minY) * (chartViewer.ViewPortTop + chartViewer.ViewPortHeight);
			double yScaleMax = maxY - (maxY - minY) * chartViewer.ViewPortTop;

			c.xAxis().setLinearScale(xScaleMin, xScaleMax);
			c.xAxis().setRounding(false, false);
			c.yAxis().setLinearScale(yScaleMin, yScaleMax);
			c.yAxis().setRounding(false, false);
		}

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}