using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections;
using System.Media;
using C1.Win.C1Chart;
using DevExpress.XtraEditors;
using Entity;
using Common;

namespace SmartWaterSystem
{
    public partial class FrmDataAnalysis : DevExpress.XtraEditors.XtraForm
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("FrmDataAnalysis");
        NoiseData data;
		internal NoiseRecorder Recorder { get; set; }
        Color[] colors = new Color[3];  //颜色条使用渐变颜色
                

        /// <summary>
        /// 构造函数
        /// </summary>
        internal FrmDataAnalysis()
        {
            InitializeComponent();
        }

        private void FrmDataAnalysis_Load(object sender, EventArgs e)
        {
            colors[0] = Color.Yellow;
            colors[1] = Color.Red;
            colors[2] = Color.Blue;

            data = Recorder.Data;
            //cbLineType.SelectedIndex = 1;
            //cbPoint.Checked = false;
            DataBinding();
            InitChart();
        }

        private void InitChart()
        {
            try
            {
                this.colorPanel1.ColorBarValue = colors;
                Random r = new Random();
                int seriescount = 20;

                ChartDataSeriesCollection coll = c1Chart1.ChartGroups[0].ChartData.SeriesList;
                List<float> lsttmp = new List<float>();
                List<float> lstdata = new List<float>();
                #region testdata
                int i = 0;
                for (i = 0; i < seriescount; i++) 
                {
                    lstdata.Add(r.Next(300, 800));
                }
                float min = lstdata.Min();
                float max = lstdata.Max();
                float interval = (max-min)/4;
                for (i = 0; i < seriescount; i++)
                {
                    lsttmp = new List<float>();
                    lsttmp.Add(lstdata[i]);
                    lsttmp.Add(1);
                    c1Chart1.ChartGroups[0].ChartData.SeriesList[i].Y.CopyDataIn(lsttmp.ToArray());
                    c1Chart1.ChartGroups[0].ChartData.SeriesList[i].FillStyle.Color1 = GetColor(min, max, lsttmp[0]);
                }
                #endregion
                while (coll.Count > seriescount)
                {
                    c1Chart1.ChartGroups[0].ChartData.SeriesList.RemoveAt(seriescount);  //将多余的Series移除，总计35个
                }

                this.colorPanel1.SetColorPanel((int)Math.Ceiling(min), (int)Math.Ceiling(min + interval), (int)Math.Ceiling(min + interval * 2),
                    (int)Math.Ceiling(min + interval * 3), (int)Math.Ceiling(max));  //set colorpanel mark
                C1.Win.C1Chart.Axis axisX = (C1.Win.C1Chart.Axis)c1Chart1.ChartArea.AxisX;
                axisX.Max = 0.1 * (30 - seriescount) + 1.52;
            }
            catch (Exception ex)
            {
                logger.ErrorException("InitChart", ex);
                XtraMessageBox.Show("加载图表发生错误!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        ///// <summary>
        ///// 绘制噪声数据统计图
        ///// </summary>
        //private void CreateChart()
        //{
        //    int hzColor = Chart.CColor(Color.MediumSeaGreen);
        //    int ampColor = Chart.CColor(Color.DodgerBlue);

        //    XYChart c = new XYChart(700, 360);
        //    c.setBackground(c.linearGradientColor(0, 0, 0, 100, 0x99ccff, 0xffffff), 0x888888);
        //    //c.setRoundedFrame();
        //    //c.setDropShadow();
        //    //c.setClipping();

        //    ChartDirector.TextBox title = c.addTitle("噪声数据分析图", "Arial Bold", 15);
        //    title.setPos(0, 20);

        //    c.setPlotArea(120, 80, 490, 230, 0xffffff, -1, -1, c.dashLineColor(
        //        0xaaaaaa, Chart.DotLine), -1);

        //    LegendBox legendBox = c.addLegend(350, 80, false, "Arial", 8);
        //    legendBox.setAlignment(Chart.BottomCenter);
        //    legendBox.setBackground(Chart.Transparent, Chart.Transparent);
        //    legendBox.setLineStyleKey();
        //    legendBox.setFontSize(8);

        //    c.xAxis().setIndent(true);
        //    c.xAxis().setTitle("噪声频率(Hz)");
        //    c.yAxis().setTitle("噪声幅度(%)").setAlignment(Chart.TopLeft2);

        //    LineLayer layer1;
        //    ChartDirector.DataSet ds;
        //    double[] da = data.Frequency.ToArray();
        //    double[] dataSet = new double[data.Amplitude.Length];
        //    data.Amplitude.CopyTo(dataSet, 0);

        //    List<double> tmp = dataSet.ToList();
        //    tmp.RemoveRange(0, 4);
        //    dataSet = tmp.ToArray();
			 
        //    switch (cbLineType.SelectedItem.ToString())
        //    {
        //        case "折线":
        //            layer1 = c.addLineLayer();
        //            ds = layer1.addDataSet(dataSet, ampColor, "噪声幅度");
        //            layer1.setLineWidth(2);
                    
        //            layer1.setXData(da);
        //            if (cbPoint.Checked)
        //                ds.setDataSymbol(Chart.CircleSymbol, 4);
        //            break;
        //        case "曲线":
        //            layer1 = c.addSplineLayer();
        //            ds = layer1.addDataSet(dataSet, ampColor, "噪声幅度");
        //            layer1.setLineWidth(2);

        //            layer1.setXData(da);
        //            if (cbPoint.Checked)
        //                ds.setDataSymbol(Chart.CircleSymbol, 5, ampColor, ampColor);
        //            break;
        //    }

        //    Mark mark = c.yAxis().addMark(Recorder.LeakValue, Chart.CColor(Color.Crimson), "警戒值(" + Recorder.LeakValue + "%)", "Arial Bold");
        //    mark.setLineWidth(2);
        //    mark.setPos(-15, 0);

        //    c.xAxis().setLabelStep(15);
        //    c.yAxis().setDateScale(0, 120);

        //    winChartViewer1.Chart = c;
        //    winChartViewer1.ImageMap = c.getHTMLImageMap("clickable", "",
        //        "title='噪声频率: {x}Hz, \n{dataSetName}: {value}%'");
        //}
        
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
            txtEnergyValue.Text = Recorder.Result.EnergyValue.ToString("f2");

			txtLeakNoise.Text = Recorder.Result.LeakAmplitude.ToString();
			txtLeakHz.Text = Recorder.Result.LeakFrequency.ToString();

			if (Recorder.Result.IsLeak == 1)
			{
				errorProvider.SetError(txtLeakNoise, "漏水！");
				errorProvider.BlinkStyle = ErrorBlinkStyle.AlwaysBlink;

				if (Settings.Instance.GetString(SettingKeys.LeakVoice) != string.Empty)
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

        private bool inMouseMove = false;
        private void c1Chart1_MouseMove(object sender, MouseEventArgs e)
        {
            X = e.X;
            Y = e.X;
            Invalidate(true);

            if (inMouseMove)
                return;

            inMouseMove = true;

            ChartRegionEnum region = c1Chart1.ChartRegionFromCoord(e.X, e.Y);
            switch (region)
            {
                case ChartRegionEnum.ChartArea:
                case ChartRegionEnum.ChartLabel:
                case ChartRegionEnum.PlotArea:
                    AddBarValueLabel(e.X, e.Y);
                    break;

                default:
                    AddBarValueLabel(-1, -1);
                    break;
            }

            inMouseMove = false;
        }
        private int oldSeries = -1, oldPoint = -1;

        private void AddBarValueLabel(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                oldSeries = -1;
                oldPoint = -1;
                return;
            }

            int s = -1, p = -1, d = -1;
            ChartGroup grp = c1Chart1.ChartGroups[0];
            if (grp.CoordToDataIndex(x, y, CoordinateFocusEnum.XCoord, ref s, ref p, ref d))
            {
                if (s >= 0 && p >= 0 && d == 0)
                {
                    if (s == oldSeries && p == oldPoint)
                        return;
                    txtCurSeriesValue.Text = ((float)(grp.ChartData[s].Y[p])).ToString("0.##");
                    return;
                }
            }

            AddBarValueLabel(-1, -1);
        }

        private int X, Y;
        private Pen pen = new Pen(Color.LawnGreen);
        private void c1Chart1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawLine(pen, X, c1Chart1.ChartArea.PlotArea.Location.Y, X, c1Chart1.ChartArea.PlotArea.Location.Y + c1Chart1.ChartArea.PlotArea.Size.Height);
        }

        private Color GetColor(float min,float max,float Value)
        {  //目前算法只支持  蓝->红->黄顺序
            if (colors == null || colors.Length == 0)
                return Color.AliceBlue ;

            if (Value >= max)
                return colors[0];
            if (Value <= min)
                return colors[2];

            float per = (Value-min)/(max - min);  //计算区间百分比
            if (per < 0.5)  //colors[2] ~ colors[1]
            {
                return Color.FromArgb(0, 0,255-(int)Math.Ceiling(per * 255));
            }
            else   //colors[1] ~ colors[0]
            {
                return Color.FromArgb(255, (int)Math.Ceiling(per * 255), 0);
            }
        }

    }
}