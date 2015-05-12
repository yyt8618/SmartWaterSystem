using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Entity;
using BLL;
using DevExpress.XtraEditors;

namespace SmartWaterSystem
{
    public partial class OLWQChartForm : DevExpress.XtraEditors.XtraForm
    {
        TerminalDataBLL dataBll = new TerminalDataBLL();
        List<OLWQDetailDataEntity> lstData = null;
        public static string TerminalID = "";

        public OLWQChartForm()
        {
            InitializeComponent();
        }

        private void OLWQChartForm_Load(object sender, EventArgs e)
        {
            dtpStart.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dtpEnd.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);

            groupBoxList.Visible = true;
            groupBoxChart.Visible = false;

            cbInterval.SelectedIndex = 0;
            cbDataType.SelectedIndex = 0;
        }

        private void btnAnalysis_Click(object sender, EventArgs e)
        {
            groupBoxList.Visible = true;
            groupBoxChart.Visible = false;
            if (!GetDat())
            {
                return;
            }
            if (lstData != null && lstData.Count > 0)
            {
                int maxvalue_index = 0;
                int minvalue_index = 0;

                decimal tmpmin, tmpmax, tmpaverage = 0;
                tmpmin = tmpmax = lstData[0].Value;
                for (int i = 0; i < lstData.Count; i++)
                {
                    if (tmpmin > lstData[i].Value)
                    {
                        minvalue_index = i;
                        tmpmin = lstData[i].Value;
                    }
                    if (tmpmax < lstData[i].Value)
                    {
                        maxvalue_index = i;
                        tmpmax = lstData[i].Value;
                    }
                    tmpaverage += lstData[i].Value;
                }
                tmpaverage = tmpaverage / lstData.Count;  //计算平均值

                ListViewItem maxvalueItem = new ListViewItem(new string[]{
                    "最高",
                    lstData[maxvalue_index].Value.ToString()
                });
                ListViewItem maxtimeItem = new ListViewItem(new string[]{
                    "出现时间",
                    lstData[maxvalue_index].CollTime.ToString("yyyy-MM-dd HH:mm")
                });

                ListViewItem minxvalueItem = new ListViewItem(new string[]{
                    "最低",
                    lstData[minvalue_index].Value.ToString()
                });
                ListViewItem mintimeItem = new ListViewItem(new string[]{
                    "出现时间",
                    lstData[minvalue_index].CollTime.ToString("yyyy-MM-dd HH:mm")
                });

                ListViewItem averagexvalueItem = new ListViewItem(new string[]{
                    "平均",
                    tmpaverage.ToString("f4")
                });

                lstDetailView.BeginUpdate();
                lstDetailView.Items.Add(maxvalueItem);
                lstDetailView.Items.Add(maxtimeItem);
                lstDetailView.Items.Add(minxvalueItem);
                lstDetailView.Items.Add(mintimeItem);
                lstDetailView.Items.Add(averagexvalueItem);

                for (int i = 0; i < lstData.Count; i++)
                {
                    ListViewItem item = new ListViewItem(new string[]{
                        lstData[i].CollTime.ToString("yyyy-MM-dd HH:mm"),
                        lstData[i].Value.ToString()
                    });
                    lstDetailView.Items.Add(item);
                }
                lstDetailView.EndUpdate();
            }
        }

        private bool GetDat()
        {
            if (dtpEnd.Value < dtpStart.Value)
            {
                XtraMessageBox.Show("结束数据不能大于起始时间!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtpEnd.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(cbInterval.Text))
            {
                XtraMessageBox.Show("间隔时间不能为空!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbInterval.Focus();
                return false;
            }
            lstDetailView.Items.Clear();
            int interval = Convert.ToInt32(cbInterval.Text);
            lstData = dataBll.GetOLWQDetail(TerminalID, dtpStart.Value, dtpEnd.Value, interval, cbDataType.SelectedIndex);

            if (lstData != null && lstData.Count > 0)
            {
                return true;
            }
            else
            {
                XtraMessageBox.Show("没有"+cbDataType.Text+"数据,请修改条件!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtpStart.Focus();
                return false;
            }
        }

        #region 图表
        private void btnGraph_Click(object sender, EventArgs e)
        {
            groupBoxList.Visible = false;
            groupBoxChart.Visible = true;
            if (!GetDat())
            {
                return;
            }
            DrawChart();
        }


        /// <summary>
        /// 绘制图表
        /// </summary>
        /// <param name="dt">数据表</param>
        public void DrawChart()
        {
            if (lstData != null && lstData.Count > 0)
            {
                try
                {
                    DataTable flowTable = new DataTable();
                    flowTable.Columns.Add("PressValue");
                    flowTable.Columns.Add("CollTime");

                    for (int i = 1; i < lstData.Count; i++)
                    {
                        DataRow row = flowTable.Rows.Add();
                        row["PressValue"] = lstData[i].Value;
                        row["CollTime"] = lstData[i].CollTime;
                    }

                    chart.DataSource = flowTable;
                    if (cbDataType.SelectedIndex == 0)
                        chart.Series[0].Name = "压力";
                    else if (cbDataType.SelectedIndex == 1)
                        chart.Series[0].Name = "正向流量";
                    else if (cbDataType.SelectedIndex == 2)
                        chart.Series[0].Name = "反向流量";
                    else if (cbDataType.SelectedIndex == 3)
                        chart.Series[0].Name = "瞬时流量";

                    chart.Series[0].XValueMember = "CollTime";
                    chart.Series[0].YValueMembers = "PressValue";
                    chart.Series[1].Enabled = false;
                    chart.ResetAutoValues();
                    chart.Legends[0].Enabled = true;
                    chart.Series[0].MarkerStyle = MarkerStyle.Circle;
                    chart.Series[0].MarkerSize = 3;
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message);
                }
            }
        }

        private void chart_GetToolTipText(object sender, System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs e)
        {
            switch (e.HitTestResult.ChartElementType)
            {
                case ChartElementType.DataPoint:
                    DataPoint tmp = e.HitTestResult.Object as DataPoint;
                    e.Text = "时间:" + tmp.AxisLabel + "\n值:" + Math.Round(tmp.YValues[0], 3);
                    break;
            }
        }
        #endregion

        private void cbDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstDetailView.Columns[1].Text = cbDataType.Text;
        }
    }
}
