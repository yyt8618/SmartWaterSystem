using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ChartDirector;
using System.Collections;
using DevExpress.XtraEditors;
using Entity;

namespace SmartWaterSystem
{
    public partial class NoiseDataCompare : BaseView, INoiseDataCompare
    {
        private NoiseRecorder sRecorder;
        private NoiseRecorder eRecorder;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public NoiseDataCompare()
        {
            InitializeComponent();
            InitComBox();
        }

        /// <summary>
        /// 初始化下拉列表
        /// </summary>
        private void InitComBox()
        {
            for (int i = 0; i < GlobalValue.recorderList.Count; i++)
            {
                if (GlobalValue.recorderList[i].Data != null)
                {
                    comboBoxEdit1.Properties.Items.Add(GlobalValue.recorderList[i].ID);
                    comboBoxEdit2.Properties.Items.Add(GlobalValue.recorderList[i].ID);
                }
            }
            //comboBoxEdit1.SelectedIndex = 0;
            //comboBoxEdit2.SelectedIndex = 1;
        }

        /// <summary>
        /// 绘制噪声数据比较分析图
        /// </summary>
        private void CreateChart(int line)
        {
            if (sRecorder == null)
                return;

            XYChart c = new XYChart(700, 360);
            c.setBackground(c.linearGradientColor(0, 0, 0, 100, 0x99ccff, 0xffffff), 0x888888);

            ChartDirector.TextBox title = c.addTitle("噪声数据比较分析图", "Arial Bold", 13);
            title.setPos(0, 20);

            c.setPlotArea(80, 80, 580, 230, 0xffffff, -1, -1, c.dashLineColor(
                0xaaaaaa, Chart.DotLine), -1);

            LegendBox legendBox = c.addLegend(350, 80, false, "Arial", 8);
            legendBox.setAlignment(Chart.BottomCenter);
            legendBox.setBackground(Chart.Transparent, Chart.Transparent);
            legendBox.setLineStyleKey();
            legendBox.setFontSize(8);

            c.xAxis().setIndent(true);
            c.xAxis().setTitle("噪声频率(Hz)");


            c.yAxis().setTitle("噪声幅度(%)");


            LineLayer layer1;
            ChartDirector.DataSet ds;
            double[] dataSet;
            double[] da;

            dataSet = sRecorder.Data.Amplitude.Skip(4).ToArray();
            da = sRecorder.Data.Frequency.Skip(4).ToArray();
            switch (line)
            {
                case 0:
                    layer1 = c.addLineLayer();
                    ds = layer1.addDataSet(dataSet, GetRandomColor(0), "记录仪" + sRecorder.ID);
                    layer1.setLineWidth(2);
                    layer1.setXData(da);
                    break;
                case 1:
                    layer1 = c.addSplineLayer();
                    ds = layer1.addDataSet(dataSet, GetRandomColor(0), "记录仪" + sRecorder.ID);
                    layer1.setLineWidth(2);
                    layer1.setXData(da);
                    break;
            }

            if (eRecorder != null && sRecorder.ID != eRecorder.ID)
            {

                dataSet = eRecorder.Data.Amplitude.Skip(4).ToArray();
                da = eRecorder.Data.Frequency.Skip(4).ToArray();
                switch (line)
                {
                    case 0:
                        layer1 = c.addLineLayer();
                        ds = layer1.addDataSet(dataSet, GetRandomColor(1), "记录仪" + eRecorder.ID);
                        layer1.setLineWidth(2);
                        layer1.setXData(da);
                        break;
                    case 1:
                        layer1 = c.addSplineLayer();
                        ds = layer1.addDataSet(dataSet, GetRandomColor(1), "记录仪" + eRecorder.ID);
                        layer1.setLineWidth(2);
                        layer1.setXData(da);
                        break;
                }
            }
            
            c.xAxis().setLabelStep(15);
            c.yAxis().setDateScale(0, 120);

            winChartViewer1.Chart = c;
            winChartViewer1.ImageMap = c.getHTMLImageMap("clickable", "",
                "title='噪声频率: {x}Hz, \n{dataSetName}: {value}%'");
        }

        /// <summary>
        /// 获取随机颜色
        /// </summary>
        /// <returns></returns>
        private int GetRandomColor(int i)
        {
            switch (i)
            { 
                case 0:
                    return Chart.CColor(Color.Red);
                case 1:
                    return Chart.CColor(Color.Green);
                case 2:
                    return Chart.CColor(Color.Blue);
                case 3:
                    return Chart.CColor(Color.Orange);
                case 4:
                    return Chart.CColor(Color.Black);
                case 5:
                    return Chart.CColor(Color.Yellow);
                case 6:
                    return Chart.CColor(Color.Purple);
                default:
                    return 65535;
            }
        }

        private void winChartViewer1_MouseMovePlotArea(object sender, MouseEventArgs e)
        {
            WinChartViewer viewer = (WinChartViewer)sender;
            //trackLineAxis((XYChart)viewer.Chart, viewer.PlotAreaMouseX);
            viewer.updateDisplay();

            // Hide the track cursor when the mouse leaves the plot area
            viewer.removeDynamicLayer("MouseLeavePlotArea");

        }

		private void winChartViewer1_ClickHotSpot(object sender, WinHotSpotEventArgs e)
		{
			Hashtable ht = e.GetAttrValues();
			ht.Remove("噪声幅度");
			ht.Remove("噪声频率");

			ht.Add("噪声幅度", e.AttrValues["value"]);
			ht.Add("噪声频率", e.AttrValues["xLabel"]);

			new FrmParamViewer().Display(sender, e);
		}

		private void FrmDataAnalysis_Load(object sender, EventArgs e)
		{
			CreateChart(0);
		}

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit combox = sender as ComboBoxEdit;
            string id = combox.SelectedItem.ToString();
            NoiseRecorder rec = (from item in GlobalValue.recorderList
                                 where item.ID.ToString() == id
                                 select item).ToList()[0];
            
            sRecorder = rec;

            CreateChart(0);
        }

        private void comboBoxEdit2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit combox = sender as ComboBoxEdit;
            string id = combox.SelectedItem.ToString();
            NoiseRecorder rec = (from item in GlobalValue.recorderList
                                 where item.ID.ToString() == id
                                 select item).ToList()[0];

            eRecorder = rec;
            CreateChart(0);
        }
    }
}