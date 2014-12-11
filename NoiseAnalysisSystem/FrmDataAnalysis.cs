using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChartDirector;
using System.Collections;
using System.Media;

namespace NoiseAnalysisSystem
{
    public partial class FrmDataAnalysis : DevExpress.XtraEditors.XtraForm
    {
        NoiseData data;
		internal NoiseRecorder Recorder { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        internal FrmDataAnalysis()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 绘制噪声数据统计图
        /// </summary>
		private void CreateChart()
		{
			int hzColor = Chart.CColor(Color.MediumSeaGreen);
			int ampColor = Chart.CColor(Color.DodgerBlue);

			XYChart c = new XYChart(700, 360);
			c.setBackground(c.linearGradientColor(0, 0, 0, 100, 0x99ccff, 0xffffff), 0x888888);
			//c.setRoundedFrame();
            //c.setDropShadow();
            //c.setClipping();

			ChartDirector.TextBox title = c.addTitle("噪声数据分析图", "Arial Bold", 15);
			title.setPos(0, 20);

			c.setPlotArea(120, 80, 490, 230, 0xffffff, -1, -1, c.dashLineColor(
				0xaaaaaa, Chart.DotLine), -1);

			LegendBox legendBox = c.addLegend(350, 80, false, "Arial", 8);
			legendBox.setAlignment(Chart.BottomCenter);
			legendBox.setBackground(Chart.Transparent, Chart.Transparent);
			legendBox.setLineStyleKey();
			legendBox.setFontSize(8);

			c.xAxis().setIndent(true);
			c.xAxis().setTitle("噪声频率(Hz)");
			c.yAxis().setTitle("噪声幅度(%)").setAlignment(Chart.TopLeft2);

			LineLayer layer1;
			ChartDirector.DataSet ds;
            double[] da = data.Frequency.Skip(4).ToArray();
			double[] dataSet = new double[data.Amplitude.Length];
			data.Amplitude.CopyTo(dataSet, 0);

			List<double> tmp = dataSet.ToList();
			tmp.RemoveRange(0, 4);
			dataSet = tmp.ToArray();
			 
			switch (cbLineType.SelectedItem.ToString())
			{
				case "折线":
					layer1 = c.addLineLayer();
					ds = layer1.addDataSet(dataSet, ampColor, "噪声幅度");
					layer1.setLineWidth(2);
                    
                    layer1.setXData(da);
					if (cbPoint.Checked)
						ds.setDataSymbol(Chart.CircleSymbol, 4);
					break;
				case "曲线":
					layer1 = c.addSplineLayer();
					ds = layer1.addDataSet(dataSet, ampColor, "噪声幅度");
					layer1.setLineWidth(2);

                    layer1.setXData(da);
					if (cbPoint.Checked)
						ds.setDataSymbol(Chart.CircleSymbol, 5, ampColor, ampColor);
					break;
			}

			Mark mark = c.yAxis().addMark(Recorder.LeakValue, Chart.CColor(Color.Crimson), "警戒值(" + Recorder.LeakValue + "%)", "Arial Bold");
			mark.setLineWidth(2);
			mark.setPos(-15, 0);

			c.xAxis().setLabelStep(15);
			c.yAxis().setDateScale(0, 120);

			winChartViewer1.Chart = c;
			winChartViewer1.ImageMap = c.getHTMLImageMap("clickable", "",
				"title='噪声频率: {x}Hz, \n{dataSetName}: {value}%'");
		}
        
        /// <summary>
        /// 数据绑定
        /// </summary>
		private void DataBinding()
		{
			txtRecID.Text = Recorder.ID.ToString();
			txtComTime.Text = Recorder.CommunicationTime.ToString();
			txtRecNum.Text = Recorder.RecordNum.ToString();
			txtRecTime.Text = Recorder.RecordTime.ToString();
			txtRecTime1.Text = (Recorder.RecordTime + (Recorder.RecordNum * Recorder.PickSpan / 60)).ToString();
			txtPickSpan.Text = Recorder.PickSpan.ToString();
			txtRemark.Text = Recorder.Remark;

			double maxAmp = data.Amplitude.ToList().Max();
			double minAmp = data.Amplitude.ToList().Min();
			double maxHz = data.Frequency.ToList().Max();
			double minHz = data.Frequency.ToList().Min();

			txtMaxNoise.Text = maxAmp.ToString();
			txtMinNoise.Text = minAmp.ToString();
			txtMaxHz.Text = maxHz.ToString();
			txtMinHz.Text = minHz.ToString();
			txtNum.Text = data.UploadFlag.ToString();

			txtLeakNoise.Text = Recorder.Result.LeakAmplitude.ToString();
			txtLeakHz.Text = Recorder.Result.LeakFrequency.ToString();

			if (Recorder.Result.IsLeak == 1)
			{
				errorProvider.SetError(txtLeakNoise, "漏水！");
				errorProvider.BlinkStyle = ErrorBlinkStyle.AlwaysBlink;

				if (AppConfigHelper.GetAppSettingValue("LeakVoice") != string.Empty)
				{
					SoundPlayer player = new SoundPlayer();
					player.PlayLooping();
				}
			}
		}

        /// <summary>
        /// 返回出现次数最多的数字
        /// </summary>
        /// <param name="numbers">要统计的数组</param>
        /// <param name="count">统计最多次数</param>
        private double GetMaxCounts(double[] numbers, out int count)
        {
            Hashtable _hash = new Hashtable();

            int max = 0;    //出现次数
            double num = 0; //数字
            foreach (double i in numbers)
            {
                if (_hash.ContainsKey(i))
                {
                    int v = (int)_hash[i];
                    _hash[i] = v + 1;
                }
                else
                    _hash.Add(i, 1);

                int tmp = (int)_hash[i];

                if (tmp > max)
                {
                    max = tmp;
                    num = i;
                }
            }
            count = (int)_hash[num];  //统计次数

            return num;               //返回出现最多次数的数
        }

        private void CheckedChanged(object sender, EventArgs e)
        {
            CreateChart();
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
			ht.Add("噪声频率", e.AttrValues["x"]);

			new FrmParamViewer().Display(sender, e);
		}

		private void FrmDataAnalysis_Load(object sender, EventArgs e)
		{
			data = Recorder.Data;
			cbLineType.SelectedIndex = 1;
			cbPoint.Checked = false;
			CreateChart();
			DataBinding();
		}
    }
}