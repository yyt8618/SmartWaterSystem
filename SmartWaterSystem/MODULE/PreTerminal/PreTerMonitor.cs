using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using BLL;
using Common;
using DevExpress.XtraEditors;
using Entity;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors.Drawing;
using DevExpress.Utils;

namespace SmartWaterSystem
{
    public partial class PreTerMonitor : BaseView,IPreTerMonitor
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("PreTerMonitor");
        TerminalDataBLL dataBll = new TerminalDataBLL();

        Color cPreLowLimit, cPreUpLimit, cPreSlopeLowLimit, cPreSlopeUpLimit, cDefault;  //报警颜色变量
        bool IsFlashColor;  //是否刷新颜色(循环显示颜色),1秒刷新一次
        int SecsCount = 60;  //计时器秒数,每60秒刷新一次列表数据
        Color[] Colors = new Color[4];
        List<string> lst_checkedTerId = new List<string>();  //记录已经选中的终端编号

        public PreTerMonitor()
        {
            InitializeComponent();
        }

        private void PreTerMonitor_Load(object sender, EventArgs e)
        {
            cDefault = Color.White;

            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Enabled = true;

            IsFlashColor = false;
            InitView();
            IsFlashColor = true;
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            if (SecsCount-- == 0)  //one minute
            {
                SecsCount = 60;
                ShowTerData();
            }
        }

        private void InitView()
        {
            ShowTerList();
            UpdateColorsConfig();
        }

        public void ShowTerList(bool allchecked = false,bool prechecked = false)
        {
            if (prechecked)
            {
                lst_checkedTerId = new List<string>();
                DataTable preDt = gridControlTer.DataSource as DataTable;
                if (preDt != null && preDt.Rows.Count > 0)
                {
                    for (int i = 0; i < preDt.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(preDt.Rows[i]["checked"]))
                        {
                            string str_terid = preDt.Rows[i]["TerminalID"].ToString().Trim();
                            if (!lst_checkedTerId.Contains(str_terid))
                            {
                                lst_checkedTerId.Add(str_terid);
                            }
                        }
                    }
                }
            }
            
            gridControlTer.DataSource = null;
            DataTable dt = dataBll.GetTerInfo(Entity.TerType.PreTer);

            DataTable dt_bind = new DataTable("BindTable");
            DataColumn col_check1 = dt_bind.Columns.Add("checked");
            col_check1.DataType = System.Type.GetType("System.Boolean");
            dt_bind.Columns.Add("TerminalID");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string terid = dt.Rows[i]["TerminalID"].ToString().Trim();
                if (prechecked)
                {
                    if (lst_checkedTerId.Contains(terid))
                    {
                        dt_bind.Rows.Add(new object[] { true, terid });
                    }
                    else
                    {
                        dt_bind.Rows.Add(new object[] { false, terid });
                    }
                }
                else
                {
                    dt_bind.Rows.Add(new object[] { allchecked, terid });
                }
            }
            gridControlTer.DataSource = dt_bind;
            gridControlTer.RefreshDataSource();
        }

        public void ShowTerData()
        {
            try
            {
                gridControl_Data.DataSource = null;
                List<int> lstTerId = new List<int>();
                for (int i = 0; i < lstTerminalListView.RowCount; i++)
                {
                    object obj = lstTerminalListView.GetRowCellValue(i, "checked");
                    if (obj != null && Convert.ToBoolean(obj))
                    {
                        string str_terid = lstTerminalListView.GetRowCellValue(i, "TerminalID").ToString().Trim();
                        if (!string.IsNullOrEmpty(str_terid))
                            lstTerId.Add(Convert.ToInt32(str_terid));
                    }
                }

                IsFlashColor = false;

                List<PreDataEntity> lstPreData = dataBll.GetPreDataTop2(lstTerId);
                List<FlowDataEntity> lstFlowData = dataBll.GetFlowDataTop2(lstTerId);
                DataTable dt = new DataTable("valueTable");
                dt.Columns.Add("TerminalID");
                dt.Columns.Add("TerminalName");
                dt.Columns.Add("PreValue");
                dt.Columns.Add("ForwardFlowValue");
                dt.Columns.Add("ReverseFlowValue");
                dt.Columns.Add("InstantFlowValue");
                dt.Columns.Add("Colors");
                dt.Columns.Add("CurColor");

                if (lstPreData != null && lstPreData.Count > 0)
                {
                    foreach (PreDataEntity data in lstPreData)
                    {
                        string str_colors = GetAlarmColor(data, data.EnablePreAlarm, data.EnableSlopeAlarm);
                        if (!string.IsNullOrEmpty(str_colors))
                        {
                            dt.Rows.Add(new object[] { 
                            data.TerminalID.ToString(),
                            data.TerminalName,
                            data.NewestPressueValue.ToString("f3"),
                            "无","无","无",
                            str_colors,""
                            });
                        }
                        else
                        {
                            dt.Rows.Add(new object[] { 
                            data.TerminalID.ToString(),
                            data.TerminalName,
                            data.NewestPressueValue.ToString("f3"),
                            "无","无","无",
                            "",""
                            });
                        }
                        lstTerId.Remove(data.TerminalID);
                    }
                }
                if (lstFlowData != null && lstFlowData.Count > 0)
                {
                    foreach (FlowDataEntity data in lstFlowData)
                    {
                        int itemindex = -1;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["TerminalID"].ToString().Trim()== data.TerminalID.ToString())
                            {
                                itemindex = i;
                                break;
                            }
                        }
                        if (itemindex == -1)
                        {
                            dt.Rows.Add(new object[] { 
                            data.TerminalID.ToString(),
                            data.TerminalName,
                            "无",
                            data.NewestForwardFlowValue.ToString("f3"),
                            data.NewestReverseFlowValue.ToString("f3"),
                            data.NewestInstantFlowValue.ToString("f3"),
                            "",""
                            });

                            lstTerId.Remove(data.TerminalID);
                        }
                        else
                        {
                            dt.Rows[itemindex]["ForwardFlowValue"] = data.NewestForwardFlowValue.ToString("f3");
                            dt.Rows[itemindex]["ReverseFlowValue"] = data.NewestReverseFlowValue.ToString("f3");
                            dt.Rows[itemindex]["InstantFlowValue"] = data.NewestInstantFlowValue.ToString("f3");
                        }
                    }
                }
                if (lstTerId != null && lstTerId.Count > 0)
                {
                    foreach (int terid in lstTerId)
                    {
                        string terName = dataBll.GetTerminalName(terid.ToString(), TerType.PreTer);
                        dt.Rows.Add(new object[] { 
                        terid.ToString(),
                        terName,
                        "无",
                        "无",
                        "无",
                        "无",
                        "",""
                        });
                    }
                }
                gridControl_Data.DataSource = dt;
                gridControl_Data.RefreshDataSource();
                IsFlashColor = true;
            }
            catch (Exception ex)
            {
                logger.ErrorException("AddItem", ex);
                IsFlashColor = true;
            }
        }

        public void UpdateColorsConfig()
        {
            cPreLowLimit = Color.FromArgb(Settings.Instance.GetInt(SettingKeys.PreLowLimitColor));
            cPreUpLimit = Color.FromArgb(Settings.Instance.GetInt(SettingKeys.PreUpLimtColor));
            cPreSlopeLowLimit = Color.FromArgb(Settings.Instance.GetInt(SettingKeys.PreSlopeLowLimitColor));
            cPreSlopeUpLimit = Color.FromArgb(Settings.Instance.GetInt(SettingKeys.PreSlopeUpLimitColor));

            btnClrPreLowLimit.BackColor = cPreLowLimit;
            btnClrPreUpLimit.BackColor = cPreUpLimit;
            btnClrSlopeLowLimit.BackColor = cPreSlopeLowLimit;
            btnClrSlopeUpLimit.BackColor = cPreSlopeUpLimit;

            Colors[0] = cPreLowLimit;
            Colors[1] = cPreUpLimit;
            Colors[2] = cPreSlopeLowLimit;
            Colors[3] = cPreSlopeUpLimit;
        }

        private string GetAlarmColor(PreDataEntity PreData, bool enablePreAlarm, bool enableSlopeAlarm)
        {
            try
            {
                string strColor = "";
                if (enablePreAlarm)
                {
                    if (PreData.PreLowLimit > PreData.NewestPressueValue)
                    {
                        strColor = "0,";
                    }
                    if (PreData.PreUpLimit < PreData.NewestPressueValue)
                    {
                        strColor += "1,";
                    }
                }
                if (enableSlopeAlarm)
                {
                    decimal preabs = Math.Abs(PreData.NewestPressueValue - PreData.PreValueLastbutone);
                    if (PreData.PreSlopeLowLimit > preabs)
                    {
                        strColor += "2,";
                    }
                    if (PreData.PreSlopeUpLimit < preabs)
                    {
                        strColor += "3,";
                    }
                }
                return strColor;
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetAlarmColor", ex);
                XtraMessageBox.Show("显示颜色错误,请联系管理员", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            GlobalValue.MainForm.LoadView(typeof(PreTerMgr));
        }

        private void btnParmConfig_Click(object sender, EventArgs e)
        {
            DataRow dr=lstDataView.GetFocusedDataRow();
            if (dr != null && dr["TerminalID"] !=DBNull.Value)
            {
                PreParmConfigForm.TerminalID = dr["TerminalID"].ToString().Trim();
                PreParmConfigForm parmconfigform = new PreParmConfigForm();
                parmconfigform.ShowDialog();
            }
            else
            {
                XtraMessageBox.Show("请选择一个终端操作!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lstTerminalListView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.Caption == "选择")
            {
                ShowTerData();
            }
        }

        private void lstDataView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.Caption == "压力值")
            {
                int rowhandle = e.RowHandle;
                if (lstDataView.GetRow(rowhandle)!=null)
                {
                    string strcolors = lstDataView.GetRowCellValue(rowhandle, "Colors").ToString();
                    if (!string.IsNullOrEmpty(strcolors))
                    {
                        string[] cols = strcolors.Split(',');
                        List<int> lstcols = new List<int>();
                        if (cols != null && cols.Length > 0)
                        {
                            for (int i = 0; i < cols.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(cols[i].Trim()))
                                    lstcols.Add(Convert.ToInt32(cols[i]));
                            }
                        }
                        
                        if(lstcols.Count>0)
                        {
                            e.Handled = true;
                            int width = (int)(e.Bounds.Width / lstcols.Count);

                            for (int i = 0; i < lstcols.Count; i++)
                            {
                                Rectangle rect = new Rectangle(e.Bounds.X + width * i, e.Bounds.Y, width, e.Bounds.Height);
                                Brush b = new SolidBrush(Colors[lstcols[i]]);
                                e.Graphics.FillRectangle(b, rect);
                            }

                            GridCellInfo cell = e.Cell as GridCellInfo;
                            Point offset = cell.CellValueRect.Location;
                            BaseEditPainter pb = cell.ViewInfo.Painter as BaseEditPainter;
                            if (!offset.IsEmpty)
                                cell.ViewInfo.Offset(offset.X, offset.Y);
                            try
                            {
                                pb.Draw(new ControlGraphicsInfoArgs(cell.ViewInfo, e.Cache, cell.Bounds));
                            }
                            finally
                            {
                                if (!offset.IsEmpty)
                                    cell.ViewInfo.Offset(-offset.X, -offset.Y);
                            }
                        }
                        else
                        {
                            e.Appearance.BackColor = cDefault;
                        }
                    }
                    else
                    {
                        e.Appearance.BackColor = cDefault;
                    }
                }
            }
        }

        private void btnSelectAllTer_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstTerminalListView.RowCount; i++)
            {
                if (lstTerminalListView.GetRowCellValue(i, "checked") != null)
                {
                    lstTerminalListView.SetRowCellValue(i, "checked", true);
                }
            }
        }

        private void btnUnSelectAllTer_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstTerminalListView.RowCount; i++)
            {
                if (lstTerminalListView.GetRowCellValue(i, "checked") != null)
                {
                    if ((bool)lstTerminalListView.GetRowCellValue(i, "checked"))
                    {
                        lstTerminalListView.SetRowCellValue(i, "checked", false);
                    }
                    else
                    {
                        lstTerminalListView.SetRowCellValue(i, "checked", true);
                    }
                }
            }
        }

        private void lstDataView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.RowHandle > -1 && e.CellValue != null)
            {
                string terId = lstDataView.GetRowCellValue(e.RowHandle, "TerminalID").ToString().Trim();
                PreTerChartForm.TerminalID = terId;
                PreTerChartForm detailForm = new PreTerChartForm();
                detailForm.ShowDialog();
            }
        }

        
    }
}
