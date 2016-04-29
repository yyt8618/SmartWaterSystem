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
    public partial class PrectrlChartForm : DevExpress.XtraEditors.XtraForm
    {
        TerminalDataBLL dataBll = new TerminalDataBLL();
        List<PrectrlDetailDataEntity> lstData = null;
        public static string TerminalID = "";

        public PrectrlChartForm()
        {
            InitializeComponent();
        }

        private void PrectrlChartForm_Load(object sender, EventArgs e)
        {
            dtpStart.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dtpEnd.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);

            groupBoxList.Visible = true;
            groupBoxChart.Visible = false;

            cbInterval.SelectedIndex = 0;
            //cbDataType.SelectedIndex = 0;
            //btnAnalysis_Click(null, null);
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
                int maxvalue_index_enpre = 0;
                int minvalue_index_enpre = 0;
                decimal tmpmin_enpre, tmpmax_enpre, tmpaverage_enpre = 0;

                int maxvalue_index_outletpre = 0;
                int minvalue_index_outletpre = 0;
                decimal tmpmin_outletpre, tmpmax_outletpre, tmpaverage_outletpre = 0;

                int maxvalue_index_forflow = 0;
                int minvalue_index_forflow = 0;
                decimal tmpmin_forflow, tmpmax_forflow, tmpaverage_forflow = 0;

                int maxvalue_index_revflow = 0;
                int minvalue_index_revflow = 0;
                decimal tmpmin_revflow, tmpmax_revflow, tmpaverage_revflow = 0;

                int maxvalue_index_insflow = 0;
                int minvalue_index_insflow = 0;
                decimal tmpmin_insflow, tmpmax_insflow, tmpaverage_insflow = 0;

                tmpmin_enpre = tmpmax_enpre = lstData[0].EntrancePreData;
                tmpmin_outletpre = tmpmax_outletpre = lstData[0].OutletPreData;
                tmpmin_forflow = tmpmax_forflow = lstData[0].ForwardFlow;
                tmpmin_revflow = tmpmax_revflow = lstData[0].ReverseFlow;
                tmpmin_insflow = tmpmax_insflow = lstData[0].InstantFlow;
                for (int i = 0; i < lstData.Count; i++)
                {
                    if (tmpmin_enpre > lstData[i].EntrancePreData)
                    {
                        minvalue_index_enpre = i;
                        tmpmin_enpre = lstData[i].EntrancePreData;
                    }
                    if (tmpmax_enpre < lstData[i].EntrancePreData)
                    {
                        maxvalue_index_enpre = i;
                        tmpmax_enpre = lstData[i].EntrancePreData;
                    }
                    tmpaverage_enpre += lstData[i].EntrancePreData;

                    if (tmpmin_outletpre > lstData[i].OutletPreData)
                    {
                        minvalue_index_outletpre = i;
                        tmpmin_outletpre = lstData[i].OutletPreData;
                    }
                    if (tmpmax_outletpre < lstData[i].OutletPreData)
                    {
                        maxvalue_index_outletpre = i;
                        tmpmax_outletpre = lstData[i].OutletPreData;
                    }
                    tmpaverage_outletpre += lstData[i].OutletPreData;

                    if (tmpmin_forflow > lstData[i].ForwardFlow)
                    {
                        minvalue_index_forflow = i;
                        tmpmin_forflow = lstData[i].ForwardFlow;
                    }
                    if (tmpmax_forflow < lstData[i].ForwardFlow)
                    {
                        maxvalue_index_forflow = i;
                        tmpmax_forflow = lstData[i].ForwardFlow;
                    }
                    tmpaverage_forflow += lstData[i].ForwardFlow;

                    if (tmpmin_revflow > lstData[i].ReverseFlow)
                    {
                        minvalue_index_revflow = i;
                        tmpmin_revflow = lstData[i].ReverseFlow;
                    }
                    if (tmpmax_revflow < lstData[i].ReverseFlow)
                    {
                        maxvalue_index_revflow = i;
                        tmpmax_revflow = lstData[i].ReverseFlow;
                    }
                    tmpaverage_revflow += lstData[i].ReverseFlow;

                    if (tmpmin_insflow > lstData[i].InstantFlow)
                    {
                        minvalue_index_insflow = i;
                        tmpmin_insflow = lstData[i].InstantFlow;
                    }
                    if (tmpmax_insflow < lstData[i].InstantFlow)
                    {
                        maxvalue_index_insflow = i;
                        tmpmax_insflow = lstData[i].InstantFlow;
                    }
                    tmpaverage_insflow += lstData[i].InstantFlow;
                }
                tmpaverage_enpre = tmpaverage_enpre / lstData.Count;  //计算平均值
                tmpaverage_outletpre = tmpaverage_outletpre / lstData.Count;  //计算平均值
                tmpaverage_forflow = tmpaverage_forflow / lstData.Count;  //计算平均值
                tmpaverage_revflow = tmpaverage_revflow / lstData.Count;  //计算平均值
                tmpaverage_insflow = tmpaverage_insflow / lstData.Count;  //计算平均值

                DataTable dt = new DataTable();
                dt.Columns.Add("Time");
                dt.Columns.Add("EnPreValue");
                dt.Columns.Add("OutletPreValue");
                dt.Columns.Add("ForFlow");
                dt.Columns.Add("RevFlow");
                dt.Columns.Add("InsFlow");

                DataRow row_maxvalue = dt.Rows.Add();
                row_maxvalue["Time"] = "最高";
                row_maxvalue["EnPreValue"]= lstData[maxvalue_index_enpre].EntrancePreData.ToString();
                row_maxvalue["OutletPreValue"]= lstData[maxvalue_index_outletpre].OutletPreData.ToString();
                row_maxvalue["ForFlow"]= lstData[maxvalue_index_forflow].ForwardFlow.ToString();
                row_maxvalue["RevFlow"]= lstData[maxvalue_index_revflow].ReverseFlow.ToString();
                row_maxvalue["InsFlow"]= lstData[maxvalue_index_insflow].InstantFlow.ToString();

                DataRow row_maxtime = dt.Rows.Add();
                row_maxtime["Time"] = "出现时间";
                row_maxtime["EnPreValue"] = lstData[maxvalue_index_enpre].CollTime.ToString("yyyy-MM-dd HH:mm");
                row_maxtime["OutletPreValue"] = lstData[maxvalue_index_outletpre].CollTime.ToString("yyyy-MM-dd HH:mm");
                row_maxtime["ForFlow"] = lstData[maxvalue_index_forflow].CollTime.ToString("yyyy-MM-dd HH:mm");
                row_maxtime["RevFlow"] = lstData[maxvalue_index_revflow].CollTime.ToString("yyyy-MM-dd HH:mm");
                row_maxtime["InsFlow"] = lstData[maxvalue_index_insflow].CollTime.ToString("yyyy-MM-dd HH:mm");

                DataRow row_minvalue = dt.Rows.Add();
                row_minvalue["Time"] = "最低";
                row_minvalue["EnPreValue"] = lstData[minvalue_index_enpre].EntrancePreData.ToString();
                row_minvalue["OutletPreValue"] = lstData[minvalue_index_outletpre].OutletPreData.ToString();
                row_minvalue["ForFlow"] = lstData[minvalue_index_forflow].ForwardFlow.ToString();
                row_minvalue["RevFlow"] = lstData[minvalue_index_revflow].ReverseFlow.ToString();
                row_minvalue["InsFlow"] = lstData[minvalue_index_insflow].InstantFlow.ToString();

                DataRow row_mintime = dt.Rows.Add();
                row_mintime["Time"] = "出现时间";
                row_mintime["EnPreValue"] = lstData[minvalue_index_enpre].CollTime.ToString("yyyy-MM-dd HH:mm");
                row_mintime["OutletPreValue"] = lstData[minvalue_index_outletpre].CollTime.ToString("yyyy-MM-dd HH:mm");
                row_mintime["ForFlow"] = lstData[minvalue_index_forflow].CollTime.ToString("yyyy-MM-dd HH:mm");
                row_mintime["RevFlow"] = lstData[minvalue_index_revflow].CollTime.ToString("yyyy-MM-dd HH:mm");
                row_mintime["InsFlow"] = lstData[minvalue_index_insflow].CollTime.ToString("yyyy-MM-dd HH:mm");

                DataRow row_averagexvalue = dt.Rows.Add();
                row_averagexvalue["Time"] = "平均";
                row_averagexvalue["EnPreValue"] = tmpaverage_enpre.ToString("f4");
                row_averagexvalue["OutletPreValue"] = tmpaverage_outletpre.ToString("f4");
                row_averagexvalue["ForFlow"] = tmpaverage_forflow.ToString("f4");
                row_averagexvalue["RevFlow"] = tmpaverage_revflow.ToString("f4");
                row_averagexvalue["InsFlow"] = tmpaverage_insflow.ToString("f4");

                //dt.Rows.Add(row_maxvalue);
                //dt.Rows.Add(row_maxtime);
                //dt.Rows.Add(row_minvalue);
                //dt.Rows.Add(row_mintime);
                //dt.Rows.Add(row_averagexvalue);

                for (int i = 0; i < lstData.Count; i++)
                {
                    DataRow row = dt.Rows.Add();
                    row["Time"] = lstData[i].CollTime.ToString("yyyy-MM-dd HH:mm");
                    row["EnPreValue"] = lstData[i].EntrancePreData.ToString();
                    row["OutletPreValue"] = lstData[i].OutletPreData.ToString();
                    row["ForFlow"] = lstData[i].ForwardFlow.ToString();
                    row["RevFlow"] = lstData[i].ReverseFlow.ToString();
                    row["InsFlow"] = lstData[i].InstantFlow.ToString();

                    //dt.Rows.Add(row);
                }

                gridControl1.DataSource = dt;
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
            gridControl1.DataSource = null;
            int interval = Convert.ToInt32(cbInterval.Text);
            lstData = dataBll.GetPrectrlDetail(TerminalID, dtpStart.Value, dtpEnd.Value, interval);

            if (lstData != null && lstData.Count > 0)
            {
                return true;
            }
            else
            {
                XtraMessageBox.Show("没有数据,请修改条件!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = gridControl1.DataSource as DataTable;
            if (dt != null && dt.Rows.Count > 0)
            {
                ;
            }
            else
            {
                XtraMessageBox.Show("请先点击分析按钮查询!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtpStart.Focus();
                return ;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "导出Excel";
            saveFileDialog.Filter = "Excel文件(*.xls)|*.xls";
            DialogResult dialogResult = saveFileDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                //gridControl1.ExportToXls(saveFileDialog.FileName, options);  
                gridControl1.ExportToExcelOld(saveFileDialog.FileName);
                DevExpress.XtraEditors.XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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
                    flowTable.Columns.Add("EnPreValue");
                    flowTable.Columns.Add("OutletPreValue");
                    flowTable.Columns.Add("ForFlow");
                    flowTable.Columns.Add("RevFlow");
                    flowTable.Columns.Add("InsFlow");
                    flowTable.Columns.Add("CollTime");

                    for (int i = 1; i < lstData.Count; i++)
                    {
                        DataRow row = flowTable.Rows.Add();
                        row["EnPreValue"] = lstData[i].EntrancePreData;
                        row["OutletPreValue"] = lstData[i].OutletPreData;
                        row["ForFlow"] = lstData[i].ForwardFlow;
                        row["RevFlow"] = lstData[i].ReverseFlow;
                        row["InsFlow"] = lstData[i].InstantFlow;
                        row["CollTime"] = lstData[i].CollTime;
                    }

                    chart.DataSource = flowTable;
                    chart.ResetAutoValues();

                    chart.Series[0].XValueMember = "CollTime";
                    chart.Series[0].YValueMembers = "EnPreValue";
                    chart.Legends[0].Enabled = true;
                    chart.Series[0].MarkerStyle = MarkerStyle.Circle;
                    chart.Series[0].MarkerSize = 3;

                    chart.Series[1].XValueMember = "CollTime";
                    chart.Series[1].YValueMembers = "OutletPreValue";
                    chart.Series[1].MarkerStyle = MarkerStyle.Circle;
                    chart.Series[1].MarkerSize = 3;

                    chart.Series[2].XValueMember = "CollTime";
                    chart.Series[2].YValueMembers = "ForFlow";
                    chart.Series[2].MarkerStyle = MarkerStyle.Circle;
                    chart.Series[2].MarkerSize = 3;

                    chart.Series[3].XValueMember = "CollTime";
                    chart.Series[3].YValueMembers = "RevFlow";
                    chart.Series[3].MarkerStyle = MarkerStyle.Circle;
                    chart.Series[3].MarkerSize = 3;

                    chart.Series[4].XValueMember = "CollTime";
                    chart.Series[4].YValueMembers = "InsFlow";
                    chart.Series[4].MarkerStyle = MarkerStyle.Circle;
                    chart.Series[4].MarkerSize = 3;
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

        //private void cbDataType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    lstDetailView.Columns[1].Text = cbDataType.Text;
        //}
    }
}
