using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using DevExpress.XtraCharts;
using System.IO;
using DevExpress.XtraEditors;
using ChartDirector;

namespace NoiseAnalysisSystem
{
    public partial class TestChart : DevExpress.XtraEditors.XtraForm
    {
        private List<double> lst_data =new List<double>();    
        private List<double> lst_stdev = new List<double>();  //标准差数组
        private double Standard_average = 0;    //标准平均值
        private double StandardAMP = 0;
        public TestChart()
        {
            InitializeComponent();
        }

        private void TestChart_Load(object sender, EventArgs e)
        {
            try
            {
                txtStandValue.Text = "";
                StandardAMP = Convert.ToDouble(AppConfigHelper.GetAppSettingValue("StandardAMP"));
            }
            catch { }
            winChartViewer1.ViewPortHeight = 1;
            winChartViewer1.ViewPortWidth = 1;
        }

        private void winChartViewer1_ViewPortChanged(object sender, ChartDirector.WinViewPortEventArgs e)
        {
            if (e.NeedUpdateChart)
                createChart();
        }

        private void createChart()
        {
            string[] xLabels = null;
            if (lst_data.Count != 0)
            {
                xLabels = new string[lst_data.Count];

                for (int i = 0; i < xLabels.Length; i++)
                {
                    xLabels[i] = (i + 1).ToString();
                }
            }

            XYChart chart = new XYChart(701, 314);
            chart.setBackground(chart.linearGradientColor(0, 0, 0, 100, 0x99ccff, 0xffffff), 0x888888);

            ChartDirector.TextBox title1_txt = chart.addTitle("能量强度变化分析", "Arial Bold", 13);
            title1_txt.setPos(0, 20);

            chart.setPlotArea(60, 60, 600, 200, 0xffffff, -1, -1, chart.dashLineColor(Chart.CColor(Color.AliceBlue), Chart.DotLine), -1);
            chart.xAxis().setLabels(xLabels);
            chart.xAxis().setLabelStep(1);
            chart.xAxis().setIndent(true);
            chart.xAxis().setTitle("点数");

            chart.yAxis().setTitle("噪声强度");

            LineLayer layer;
            layer=chart.addLineLayer();
            layer.addDataSet(lst_data.ToArray(), Chart.CColor(Color.Green), "噪声强度");
            layer.setLineWidth(2);
            layer.setFastLineMode(true);

            layer = chart.addLineLayer();
            layer.addDataSet(lst_stdev.ToArray(), Chart.CColor(Color.DarkViolet), "绝对差");
            layer.setLineWidth(2);
            layer.setFastLineMode(true);

            Mark standAMPmark=chart.yAxis().addMark(StandardAMP, Chart.CColor(Color.Crimson), "", "Arial Bold");
            standAMPmark.setLineWidth(2);
            standAMPmark.setPos(-15, 10);
            standAMPmark.setFontAngle(90);

            //标准平均值
            Mark standAVmark = chart.yAxis().addMark(StandardAMP, Chart.CColor(Color.Cyan), "", "Arial Bold");
            standAVmark.setLineWidth(2);
            standAVmark.setPos(-15, 10);
            standAVmark.setFontAngle(90);

            LegendBox legendbox=chart.addLegend(550, 25);
            legendbox.addKey("静态漏水标准值", Chart.CColor(Color.Crimson));
            legendbox.addKey("标准平均值", Chart.CColor(Color.Cyan));
            //legendbox.addKey("噪声强度", Chart.CColor(Color.Green));
            //legendbox.addKey("绝对差", Chart.CColor(Color.DarkViolet));

            chart.layout();

            winChartViewer1.Chart = chart;
            winChartViewer1.ImageMap =
                winChartViewer1.Chart.getHTMLImageMap("clickable", "", "title='点数: {xLabel},强度: {value}'");
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            txtStandValue.Text = "";
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
                    Standard_average = d[0];
                    lst_data.AddRange(d.Skip(1).ToArray());

                    for (int i = 0; i < lst_data.Count; i++)
                    {
                        lst_stdev.Add(Math.Abs(Standard_average - lst_data[i]));
                    }

                    double standvalue=GetAverage(lst_data.ToArray());
                    standvalue = Math.Abs(Standard_average - standvalue);

                    txtStandValue.Text = standvalue.ToString("f2");

                    winChartViewer1.updateViewPort(true, false);
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

        private double GetAverage(double[] datas)
        {
            double average = 0;
            if (datas != null && datas.Length > 0)
            {
                for (int i = 0; i < datas.Length; i++)
                {
                    average += datas[i];
                }
                average /= datas.Length;
            }
            return average;
        }

    }
}
