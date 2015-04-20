using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BLL;
using Common;
using DevExpress.XtraEditors;
using Entity;

namespace SmartWaterSystem
{
    public partial class PreTerMonitor : BaseView,IPreTerMonitor
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("PreTerMonitor");
        TerminalDataBLL dataBll = new TerminalDataBLL();

        Color cPreLowLimit, cPreUpLimit, cPreSlopeLowLimit, cPreSlopeUpLimit, cDefault;  //报警颜色变量
        bool IsFlashColor;  //是否刷新颜色(循环显示颜色),1秒刷新一次
        int SecsCount = 60;  //计时器秒数,每60秒刷新一次列表数据

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
            //UpdateListViewColor();
        }

        private void InitView()
        {
            ShowTerList();

            UpdateColorsConfig();
        }


        public void ShowTerList(bool bchecked = false)
        {
            gridControlTer.DataSource = null;
            DataTable dt = dataBll.GetTerInfo(Entity.TerType.PreTer);

            DataTable dt_bind = new DataTable("BindTable");
            DataColumn col_check1 = dt_bind.Columns.Add("checked");
            col_check1.DataType = System.Type.GetType("System.Boolean");
            dt_bind.Columns.Add("TerminalID");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string terid = dt.Rows[i]["TerminalID"].ToString().Trim();
                dt_bind.Rows.Add(new object[] { bchecked, terid });
            }
            gridControlTer.DataSource = dt_bind;
            gridControlTer.RefreshDataSource();
        }

        private void ShowTerData()
        {
            try
            {
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
                //int index = -1;
                //foreach (int terid in lstTerId)
                //{
                //    if (isExistDataList(terid, out index) == 1)
                //    {
                //        lstTerId.Remove(terid);
                //    }
                //}

                List<PreDataEntity> lstPreData = dataBll.GetPreDataTop2(lstTerId);
                List<FlowDataEntity> lstFlowData = dataBll.GetFlowDataTop2(lstTerId);
                DataTable dt = new DataTable("valueTable");
                dt.Columns.Add("TerminalID");
                dt.Columns.Add("TerminalName");
                dt.Columns.Add("PreValue");
                dt.Columns.Add("ForwardFlowValue");
                dt.Columns.Add("ReverseFlowValue");
                dt.Columns.Add("InstantFlowValue");

                if (lstPreData != null && lstPreData.Count > 0)
                {
                    foreach (PreDataEntity data in lstPreData)
                    {
                        dt.Rows.Add(new object[] { 
                            data.TerminalID.ToString(),
                            data.TerminalName,
                            data.NewestPressueValue.ToString("f3"),
                            "无","无","无"
                        });
                        //ItemTagEntity tagEntity = new ItemTagEntity();
                        //List<Color> lstColor = GetAlarmColor(data, data.EnablePreAlarm, data.EnableSlopeAlarm);
                        //tagEntity.lstColor = lstColor;
                        //tagEntity.Addr = data.Addr;
                        //item.Tag = tagEntity;
                        //判读是否发出警报
                        //if (lstColor != null && lstColor.Count > 0)
                        //{
                        //    item.BackColor = lstColor[0];
                        //}
                        //else
                        //{
                        //    //set default backcolor
                        //    item.BackColor = cDefault;
                        //}

                        //lstDataView.Items.Add(item);
                        //lstItems.Add(item);
                        //lstTerId.Remove(data.TerminalID);
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
                            data.NewestInstantFlowValue.ToString("f3")
                            });
                            //ItemTagEntity tagEntity = new ItemTagEntity();
                            //tagEntity.lstColor = null;
                            //tagEntity.Addr = data.Addr;
                            //item.Tag = tagEntity;
                            ////set default backcolor
                            //item.BackColor = cDefault;
                            //lstItems.Add(item);
                            //lstTerId.Remove(data.TerminalID);
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
                        "无"
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
        }

        /// <summary>
        /// 刷新每行颜色
        /// </summary>
        //private void UpdateListViewColor()
        //{
        //    if (IsFlashColor)
        //    {
        //        lstDataView.BeginUpdate();
        //        foreach (ListViewItem item in lstDataView.Items)
        //        {
        //            if (item.Tag != null)
        //            {
        //                ItemTagEntity tagEntity = (ItemTagEntity)item.Tag;
        //                List<Color> lstColor = tagEntity.lstColor;
        //                if (lstColor != null && lstColor.Count > 1)
        //                {
        //                    for (int i = 0; i < lstColor.Count; i++)
        //                    {
        //                        if (lstColor[i] == item.BackColor)
        //                        {
        //                            if ((i + 1) < lstColor.Count)
        //                            {
        //                                item.BackColor = lstColor[i + 1];
        //                                break;
        //                            }
        //                            else
        //                            {
        //                                item.BackColor = lstColor[0];
        //                                break;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        lstDataView.EndUpdate();
        //    }
        //}

        private List<Color> GetAlarmColor(PreDataEntity PreData, bool enablePreAlarm, bool enableSlopeAlarm)
        {
            try
            {
                List<Color> lstColor = new List<Color>();
                if (enablePreAlarm)
                {
                    if (PreData.PreLowLimit > PreData.NewestPressueValue)
                    {
                        lstColor.Add(cPreLowLimit);
                    }
                    if (PreData.PreUpLimit < PreData.NewestPressueValue)
                    {
                        lstColor.Add(cPreUpLimit);
                    }
                }
                if (enableSlopeAlarm)
                {
                    decimal preabs = Math.Abs(PreData.NewestPressueValue - PreData.PreValueLastbutone);
                    if (PreData.PreSlopeLowLimit > preabs)
                    {
                        lstColor.Add(cPreSlopeLowLimit);
                    }
                    if (PreData.PreSlopeUpLimit < preabs)
                    {
                        lstColor.Add(cPreSlopeUpLimit);
                    }
                }
                return lstColor;
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetAlarmColor", ex);
                XtraMessageBox.Show("显示颜色错误,请联系管理员", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        //private void lstDataView_ItemActivate(object sender, EventArgs e)
        //{
        //    if (lstDataView.FocusedItem.SubItems[2].Text.Trim() == "无压力数据" && lstDataView.FocusedItem.SubItems[3].Text.Trim() == "无压力数据" &&
        //        lstDataView.FocusedItem.SubItems[4].Text.Trim() == "无压力数据" && lstDataView.FocusedItem.SubItems[5].Text.Trim() == "无压力数据")
        //    {
        //        XtraMessageBox.Show(string.Format("终端[{0}]无数据!", lstDataView.FocusedItem.SubItems[0].Text), "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return;
        //    }
        //    DetailForm.TerminalID = lstDataView.FocusedItem.SubItems[0].Text;
        //    DetailForm.TerminalName = lstDataView.FocusedItem.SubItems[1].Text;
        //    DetailForm detailForm = new DetailForm();
        //    detailForm.ShowDialog();
        //}

        private void btnConfig_Click(object sender, EventArgs e)
        {
            //ConfigForm.main = this;
            //ConfigForm configform = new ConfigForm();
            //configform.ShowDialog();
        }

        private void btnParmConfig_Click(object sender, EventArgs e)
        {
            //ListView.SelectedIndexCollection selectCol = lstDataView.SelectedIndices;
            //if (selectCol != null && selectCol.Count > 0)
            //{
            //    ParmConfigForm.TerminalID = lstDataView.Items[selectCol[0]].SubItems[0].Text;
            //    ParmConfigForm parmconfigform = new ParmConfigForm();
            //    parmconfigform.ShowDialog();
            //}
            //else
            //{
            //    XtraMessageBox.Show("请选择一个终端操作!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
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
                e.Appearance.BackColor = Color.DodgerBlue;
            }
        }


        
    }
}
