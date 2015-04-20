using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Media;
using C1.Win.C1Chart;
using DevExpress.XtraEditors;
using Entity;
using Common;
using System.Threading;

namespace SmartWaterSystem
{
    public partial class FrmDataAnalysis : DevExpress.XtraEditors.XtraForm
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("FrmDataAnalysis");
        NoiseData data;
		internal NoiseRecorder Recorder { get; set; }
        Color[] colors = new Color[3];  //颜色条使用渐变颜色
        Image[] images = new Image[7];
        int index_image = 0;
        Thread t_animation = null;
        
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

            images[0] = Properties.Resources.leak1;
            images[1] = Properties.Resources.leak2;
            images[2] = Properties.Resources.leak3;
            images[3] = Properties.Resources.leak4;
            images[4] = Properties.Resources.leak5;
            images[5] = Properties.Resources.leak6;
            images[6] = Properties.Resources.leak7;

            data = Recorder.Data;
            DataBinding();
            InitChart();
        }

        private void InitChart()
        {
            try
            {
                //this.colorPanel1.ColorBarValue = colors;
                //Random r = new Random();
                int seriescount = data.Amplitude.Length;

                ChartDataSeriesCollection coll = c1Chart1.ChartGroups[0].ChartData.SeriesList;
                List<double> lsttmp = new List<double>();
                #region testdata
                //int i = 0;
                //for (i = 0; i < seriescount; i++) 
                //{
                //    lstdata.Add(r.Next(300, 800));
                //}
                string str_DCLen = Settings.Instance.GetString(SettingKeys.DCComponentLen);  //获取设定的直流分量长度
                int DCComponentLen = 6;
                if (!string.IsNullOrEmpty(str_DCLen))
                    DCComponentLen = Convert.ToInt32(str_DCLen);

                float min = DCComponentLen * 2000 / 256;
                float max = 1000;
                float interval = (max-min)/4;
                for (int i = 0; i < seriescount; i++)
                {
                    lsttmp = new List<double>();
                    lsttmp.Add(data.Amplitude[i]);
                    lsttmp.Add(1);
                    c1Chart1.ChartGroups[0].ChartData.SeriesList[i].Y.CopyDataIn(lsttmp.ToArray());
                    c1Chart1.ChartGroups[0].ChartData.SeriesList[i].FillStyle.Color1 = GetColor(min, max, data.Frequency[i]);
                }
                #endregion
                while (coll.Count > seriescount)
                {
                    c1Chart1.ChartGroups[0].ChartData.SeriesList.RemoveAt(seriescount);  //将多余的Series移除，总计35个
                }

                colorPanel_static1.SetColorPanel((int)Math.Ceiling(min), (int)Math.Ceiling(min + interval), (int)Math.Ceiling(min + interval * 2),
                    (int)Math.Ceiling(min + interval * 3), (int)Math.Ceiling(max));  //set colorpanel mark
                C1.Win.C1Chart.Axis axisX = (C1.Win.C1Chart.Axis)c1Chart1.ChartArea.AxisX;
                if (seriescount > 1)
                    axisX.Max = 0.1 * (30 - seriescount) + 1.52;
                else
                    axisX.Max = 15;

                c1Chart1.ChartArea.AxisY.ValueLabels.Add(Recorder.LeakValue, "");
                c1Chart1.ChartArea.AxisY.ValueLabels[0].Appearance = ValueLabelAppearanceEnum.TriangleMarker;
                c1Chart1.ChartArea.AxisY.ValueLabels[0].GridLine = false;
                c1Chart1.ChartArea.AxisY.ValueLabels[0].Color = Color.Red;
                c1Chart1.ChartArea.AxisY.ValueLabels[0].Moveable = false;
                c1Chart1.ChartArea.AxisY.AnnoMethod = AnnotationMethodEnum.Mixed;

                c1Chart1.ChartArea.AxisY.Max = 100;
                c1Chart1.ChartArea.AxisY.AutoMax = false;
                
            }
            catch (Exception ex)
            {
                logger.ErrorException("InitChart", ex);
                XtraMessageBox.Show("加载图表发生错误!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

            double max_amp, max_frq, min_amp, min_frq, leak_amp, leak_frq;
            NoiseDataHandler.IsLeak3(data.Amplitude, data.Frequency, Recorder.LeakValue, out max_amp, out max_frq, out min_amp, out min_frq, out leak_amp, out leak_frq);

            //double maxAmp = data.Amplitude.ToList().Max();
            //double minAmp = data.Amplitude.ToList().Min();
            //double maxHz = data.Frequency.ToList().Max();
            //double minHz = data.Frequency.ToList().Min();

            txtMaxNoise.Text = max_amp.ToString();
            txtMinNoise.Text = min_amp.ToString();
            txtMaxHz.Text = max_frq.ToString();
            txtMinHz.Text = min_frq.ToString();
			txtNum.Text = data.UploadFlag.ToString();
            txtEnergyValue.Text = Recorder.Result.EnergyValue.ToString("f2");

            txtLeakNoise.Text = leak_amp.ToString();// Recorder.Result.LeakAmplitude.ToString();
            txtLeakHz.Text = leak_frq.ToString(); // Recorder.Result.LeakFrequency.ToString();

            if (Recorder.Result.IsLeak == 1)
            {
                //errorProvider.SetError(txtLeakNoise, "漏水！");
                //errorProvider.BlinkStyle = ErrorBlinkStyle.AlwaysBlink;

                PicBox.Visible = true;
                if (Settings.Instance.GetString(SettingKeys.LeakVoice) != string.Empty)
                {
                    SoundPlayer player = new SoundPlayer();
                    player.PlayLooping();
                }

                t_animation = new Thread(new ThreadStart(EnableLeak_animation));
                t_animation.Start();
            }
            else
                PicBox.Visible = false;
		}

        private void EnableLeak_animation()
        {
            while (true)
            {
                if (index_image == 7)
                    index_image = 0;
                PicBox.Image = images[index_image];
                index_image++;

                Thread.Sleep(120);
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
                    txtCurSeriesValue.Text = ((grp.ChartData[s].Y[p])).ToString();
                    return;
                }
            }

            AddBarValueLabel(-1, -1);
        }

        private int X, Y;
        private Pen pen = new Pen(Color.LawnGreen);
        Pen pen_mark = new Pen(Color.Red);
        private void c1Chart1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawLine(pen, X, c1Chart1.ChartArea.PlotArea.Location.Y, X, c1Chart1.ChartArea.PlotArea.Location.Y + c1Chart1.ChartArea.PlotArea.Size.Height);

            //paint leakvalue marker
            Rectangle rect_mark = c1Chart1.ChartArea.AxisY.ValueLabels[0].MarkerRectangle;
            g.DrawLine(pen_mark, c1Chart1.ChartArea.PlotArea.Location.X, rect_mark.Y + rect_mark.Height / 2, c1Chart1.ChartArea.PlotArea.Location.X + c1Chart1.ChartArea.PlotArea.Size.Width, rect_mark.Y + rect_mark.Height / 2);
        }

        private Color GetColor(double min, double max, double Value)
        {
            try
            {
                //目前算法只支持  蓝->红->黄顺序
                if (colors == null || colors.Length == 0)
                    return Color.AliceBlue;

                if (Value >= max)
                    return colors[0];
                if (Value <= min)
                    return colors[2];

                if (Value < ((max - min) / 2))  //colors[2] ~ colors[1]
                {
                    double per = Value / ((max - min) / 2);  //计算区间百分比
                    return Color.FromArgb((int)Math.Ceiling(per * 255), 0, 255 - (int)Math.Ceiling(per * 255));
                }
                else  //colors[1] ~ colors[0]
                {
                    double per = (Value - (max - min) / 2) / ((max - min) / 2); 
                    return Color.FromArgb(255, (int)Math.Ceiling(per * 255), 0);
                }
            }
            catch (Exception ex)
            {
                return colors[0];
            }
        }

        private void FrmDataAnalysis_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (t_animation != null)
                    t_animation.Abort();
            }
            catch { }
        }

    }
}