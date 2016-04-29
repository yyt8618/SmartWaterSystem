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
using PrectrlMonitor;

namespace SmartWaterSystem
{
    public partial class PrectrlMonitor : BaseView, IPrectrlMonitor
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("PrectrlMonitor");
        TerminalDataBLL dataBll = new TerminalDataBLL();

        Color cPreLowLimit, cPreUpLimit, cTimeout, cBattery, cEntranceSensor, cOutletSensor, cFlowMeter, cDefault;  //报警颜色变量
        bool IsFlashColor;  //是否刷新颜色(循环显示颜色),1秒刷新一次
        int SecsCount = 60;  //计时器秒数,每60秒刷新一次列表数据
        Color[] Colors = new Color[7];
        List<string> lst_checkedTerId = new List<string>();  //记录已经选中的终端编号

        public PrectrlMonitor()
        {
            InitializeComponent();
        }

        private void PrectrlMonitor_Load(object sender, EventArgs e)
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
            DataTable dt = dataBll.GetTerInfo(Entity.TerType.PressureCtrl);

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
                
                List<PrectrlDataEntity> lstPrectrlData = dataBll.GetPrectrlData(lstTerId);
                DataTable dt = new DataTable("valueTable");
                dt.Columns.Add("TerminalID");
                dt.Columns.Add("TerminalName");
                dt.Columns.Add("EnPreValue");
                dt.Columns.Add("OutletPreValue");
                dt.Columns.Add("ForwardFlowValue");
                dt.Columns.Add("ReverseFlowValue");
                dt.Columns.Add("InstantFlowValue");
                dt.Columns.Add("AlarmDesc");
                dt.Columns.Add("Colors");
                dt.Columns.Add("CurColor");

                if (lstPrectrlData != null && lstPrectrlData.Count > 0)
                {
                    foreach (PrectrlDataEntity data in lstPrectrlData)
                    {
                        string str_colors = GetAlarmColor(data.AlarmCode);
                        if (!string.IsNullOrEmpty(str_colors))
                        {
                            dt.Rows.Add(new object[] { 
                            data.TerminalID.ToString(),
                            data.TerminalName,
                            data.EntrancePreValue.ToString("f3"),
                            data.OutletPreValue.ToString("f3"),
                            data.ForwardFlowValue.ToString("f3"),
                            data.ReverseFlowValue.ToString("f3"),
                            data.InstantFlowValue.ToString("f3"),
                            data.AlarmDesc,
                            str_colors,""
                            });
                        }
                        else
                        {
                            dt.Rows.Add(new object[] { 
                            data.TerminalID.ToString(),
                            data.TerminalName,
                            data.EntrancePreValue.ToString("f3"),
                            data.OutletPreValue.ToString("f3"),
                            data.ForwardFlowValue.ToString("f3"),
                            data.ReverseFlowValue.ToString("f3"),
                            data.InstantFlowValue.ToString("f3"),
                            data.AlarmDesc,
                            "",""
                            });
                        }
                        lstTerId.Remove(data.TerminalID);
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
            cPreLowLimit = Color.Purple;
            cPreUpLimit = Color.LimeGreen;
            cTimeout = Color.Aqua;
            cBattery = Color.Aquamarine;
            cEntranceSensor = Color.IndianRed;
            cOutletSensor = Color.BurlyWood;
            cFlowMeter = Color.Orange;

            btnClrPreLowLimit.BackColor = cPreLowLimit;
            btnClrPreUpLimit.BackColor = cPreUpLimit;
            btnClrTimeout.BackColor = cTimeout;
            btnClrBattery.BackColor = cBattery;
            btnClrEntranceSensor.BackColor = cEntranceSensor;
            btnClrOutletSensor.BackColor = cOutletSensor;
            btnClrFlowmeter.BackColor = cFlowMeter;

            Colors[0] = cPreLowLimit;
            Colors[1] = cPreUpLimit;
            Colors[2] = cTimeout;
            Colors[3] = cBattery;
            Colors[4] = cEntranceSensor;
            Colors[5] = cOutletSensor;
            Colors[6] = cFlowMeter;
        }

        private string GetAlarmColor(byte alarmcode)
        {
            try
            {
                string strColor = "";
                if (((alarmcode & 0x10) >> 4) == 1)  //进口压力上限报警
                    strColor += "0,";
                else if (((alarmcode & 0x20) >> 5) == 1)   //进口压力下限报警
                    strColor += "1,";
                else if (((alarmcode & 0x40) >> 6) == 1)   //压力调整超时报警
                    strColor += "2,";

                if ((alarmcode & 0x01) == 1)  //电池报警
                    strColor += "3,";
                else if (((alarmcode & 0x02) >> 1) == 1)   //进口压力传感器报警
                    strColor += "4,";
                else if (((alarmcode & 0x04) >> 2) == 1)  //出口压力传感器报警
                    strColor += "5,";
                else if (((alarmcode & 0x08) >> 3) == 1)  //流量器报警
                    strColor += "6,";

                return strColor;
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetAlarmColor", ex);
                XtraMessageBox.Show("显示颜色错误,请联系管理员", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
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
            if (e.Column.Caption=="报警")
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
                PrectrlChartForm.TerminalID = terId;
                PrectrlChartForm detailForm = new PrectrlChartForm();
                detailForm.ShowDialog();
            }
        }

        
    }
}
