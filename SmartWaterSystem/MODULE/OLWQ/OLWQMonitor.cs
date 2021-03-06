﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraEditors;
using BLL;
using Entity;
using System.Data;
using Common;
using DevExpress.XtraGrid.Views.BandedGrid.ViewInfo;
using DevExpress.XtraGrid.Columns;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Text.RegularExpressions;

namespace SmartWaterSystem
{
    public partial class OLWQMonitor : BaseView, IOLWQMonitor
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("OLWQMonitor");
        TerminalDataBLL dataBll = new TerminalDataBLL();
        bool calldataEnable = false;  //是否开启招测
        string currentTerid = "";       //当前操作的终端编号
        List<OnLineTerEntity> OnLineTers = null;  //在线终端记录
        Color cLowLimit, cUpLimit, cDefault;  //报警颜色变量
        List<OLWQConfigEntity> lst_config;    //配置数据
        bool update_config = false;            //是否在更新配置

        public OLWQMonitor()
        {
            InitializeComponent();
            GlobalValue.SocketMgr.SockMsgEvent += new SocketHandler(MSMQMgr_MSMQEvent);
            timer1.Interval = 20 * 1000;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Enabled = true;
        }

        private void OLWQMonitor_Load(object sender, EventArgs e)
        {
            //是否启用招测
            calldataEnable=Settings.Instance.GetBool(SettingKeys.CallDataEnable);
            GridColumn calldataCol = gridViewTer.Columns["OnLinePic"];
            calldataCol.Visible = calldataEnable;
            
            InitGrid();

            if (calldataEnable)
            {
                //获取终端在线状态
                SocketEntity msmqEntity = new SocketEntity();
                msmqEntity.MsgType = ConstValue.MSMQTYPE.Get_OnLineState;
                GlobalValue.SocketMgr.SendMessage(msmqEntity);
            }

            UpdateColorsConfig();
        }

        public void UpdateColorsConfig()
        {
            cLowLimit = Color.FromArgb(Settings.Instance.GetInt(SettingKeys.OLWQLowLimitColor));
            cUpLimit = Color.FromArgb(Settings.Instance.GetInt(SettingKeys.OLWQUpLimitColor));

            btnClrLowLimit.BackColor = cLowLimit;
            btnClrUpLimit.BackColor = cUpLimit;
        }

        void MSMQMgr_MSMQEvent(object sender, SocketEventArgs e)
        {
            if (e.msmqEntity != null && e.msmqEntity.MsgType == ConstValue.MSMQTYPE.Data_OnLineState)
            {
                InvokeShowGridTerData(e.msmqEntity.lstOnLine);
                OnLineTers = e.msmqEntity.lstOnLine;
            }
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            ShowTerData();
        }

        private void InitGrid()
        {
            try
            {
                InitTerGrid();
            }
            catch (Exception ex)
            {
                logger.ErrorException("InitGridData", ex);
                XtraMessageBox.Show("初始化列表发生错误!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void InitTerGrid()
        {
            ShowGridTerData(null);
        }

        private delegate void ShowGridTerDataHandler(List<OnLineTerEntity> lstOnLine);
        private void InvokeShowGridTerData(List<OnLineTerEntity> lstOnLine)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((ShowGridTerDataHandler)delegate(List<OnLineTerEntity> onlinedata)
                {
                    ShowGridTerData(onlinedata);
                }, lstOnLine);
            }
            else
            {
                ShowGridTerData(lstOnLine);
            }

        }

        private void ShowGridTerData(List<OnLineTerEntity> lstOnLine)
        {
            gridControlTer.DataSource = null;
            DataTable dt = dataBll.GetTerInfo(TerType.OLWQTer);

            DataTable dt_bind = new DataTable("BindTable");
            DataColumn col_check1 = dt_bind.Columns.Add("checked");
            col_check1.DataType = System.Type.GetType("System.Boolean");
            dt_bind.Columns.Add("TerminalID");
            DataColumn col_check2=dt_bind.Columns.Add("OnLinePic");  //是否在线状态图片
            //col_check2.DataType = System.Type.GetType("System.Int32");
            col_check2.DataType = System.Type.GetType("System.Byte[]");
            DataColumn col_check3 = dt_bind.Columns.Add("IsOnLine");  //是否在线状态标志
            col_check3.DataType = System.Type.GetType("System.Int32");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string terid = dt.Rows[i]["TerminalID"].ToString().Trim();
                bool online = false;
                if (lstOnLine != null)
                {
                    foreach (OnLineTerEntity terentity in lstOnLine)
                    {
                        if (terentity.DevType == ConstValue.DEV_TYPE.OLWQ_CTRL && terentity.DevId.ToString() == terid)
                        {
                            online = true;
                            break;
                        }
                    }
                }
                if (online)
                    dt_bind.Rows.Add(new object[] { false,terid,Bitmap2Byte(SmartWaterSystem.Properties.Resources.亮灯16x25),1 });
                else
                    dt_bind.Rows.Add(new object[] { false,terid, Bitmap2Byte(SmartWaterSystem.Properties.Resources.灭灯16x25), 0 });

            }
            gridControlTer.DataSource = dt_bind;
            gridControlTer.RefreshDataSource();
        }

        byte[] Bitmap2Byte(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                byte[] data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                return data;
            }
        }

        public void UpdateView()
        {
            List<string> lst_selectIds = new List<string>();
            DataTable dt = gridControlTer.DataSource as DataTable;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["checked"] != DBNull.Value)
                    {
                        if (Convert.ToBoolean(dr["checked"]))
                        {
                            lst_selectIds.Add(dr["TerminalID"].ToString().Trim());
                        }
                    }
                }
            }

            gridControlTer.DataSource = null;
            DataTable dt_config = dataBll.GetTerInfo(TerType.OLWQTer);

            DataTable dt_bind = new DataTable("BindTable");
            DataColumn col_check1 = dt_bind.Columns.Add("checked");
            col_check1.DataType = System.Type.GetType("System.Boolean");
            dt_bind.Columns.Add("TerminalID");
            DataColumn col_check2 = dt_bind.Columns.Add("OnLinePic");  //是否在线状态图片
            //col_check2.DataType = System.Type.GetType("System.Int32");
            col_check2.DataType = System.Type.GetType("System.Byte[]");
            DataColumn col_check3 = dt_bind.Columns.Add("IsOnLine");  //是否在线状态标志
            col_check3.DataType = System.Type.GetType("System.Int32");

            for (int i = 0; i < dt_config.Rows.Count; i++)
            {
                string terid = dt_config.Rows[i]["TerminalID"].ToString().Trim();
                bool online = false;
                if (OnLineTers != null)
                {
                    foreach (OnLineTerEntity terentity in OnLineTers)
                    {
                        if (terentity.DevType == ConstValue.DEV_TYPE.OLWQ_CTRL && terentity.DevId.ToString() == terid)
                        {
                            online = true;
                            break;
                        }
                    }
                }

                bool select = false;
                foreach (string teridtmp in lst_selectIds)
                {
                    if (terid == teridtmp.Trim())
                    {
                        select = true;
                    }
                }

                if (online)
                    dt_bind.Rows.Add(new object[] { select, terid, Bitmap2Byte(SmartWaterSystem.Properties.Resources.亮灯16x25), 1 });
                else
                    dt_bind.Rows.Add(new object[] { select, terid, Bitmap2Byte(SmartWaterSystem.Properties.Resources.灭灯16x25), 0 });

            }
            gridControlTer.DataSource = dt_bind;
            gridControlTer.RefreshDataSource();
            ShowTerData();
        }

        private void ShowTerData()
        {
            List<string> lst_terid = new List<string>();
            for (int i = 0; i < gridViewTer.RowCount; i++)
            {
                object obj = gridViewTer.GetRowCellValue(i, "checked");
                if (obj != null && Convert.ToBoolean(obj))
                    lst_terid.Add(gridViewTer.GetRowCellValue(i, "TerminalID").ToString().Trim());
            }

            if (lst_terid.Count > 0)
            {
                update_config = true;
                string ter_where = "";
                foreach (string terid in lst_terid)
                {
                    ter_where += "'" + terid + "',";
                }
                ter_where = ter_where.Substring(0, ter_where.Length - 1);
                lst_config = (new OLWQConfigBLL()).Select("TerminalID IN("+ter_where+") ORDER BY TerminalID");
                update_config = false;

                DataTable dt = dataBll.GetOLWQData(lst_terid);
                gridControl_data.DataSource = dt;
                gridControl_data.RefreshDataSource();
            }
            else
            {
                gridControl_data.DataSource = null;
                gridControl_data.RefreshDataSource();
            }
            update_config = false;
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            if (e.Info.IsRowIndicator)
            {
                if (e.RowHandle >= 0)
                {
                    e.Info.DisplayText = (e.RowHandle + 1).ToString();
                }
                else if (e.RowHandle < 0 && e.RowHandle > -1000)
                {
                    e.Info.Appearance.BackColor = System.Drawing.Color.AntiqueWhite;
                    e.Info.DisplayText = "G" + e.RowHandle.ToString();
                }
            }
        }

        private void gridViewTer_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.Caption == "选择")
            {
                ShowTerData();
            }
        }

        private void MenuItem_Online_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentTerid) && Regex.IsMatch(currentTerid,@"^\d{1,3}$"))
            {
                if (DialogResult.No == XtraMessageBox.Show("是否使终端[" + currentTerid + "]上线?", GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                {
                    return;
                }
                SocketEntity msmqEntity = new SocketEntity();
                msmqEntity.MsgType = ConstValue.MSMQTYPE.Cmd_Online;
                msmqEntity.DevId = Convert.ToInt16(currentTerid);
                msmqEntity.DevType = ConstValue.DEV_TYPE.OLWQ_CTRL;
                msmqEntity.AllowOnline = true;
                GlobalValue.SocketMgr.SendMessage(msmqEntity);
            }
        }

        private void MenuItem_Offline_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentTerid) && Regex.IsMatch(currentTerid, @"^\d{1,3}$"))
            {
                if (DialogResult.No == XtraMessageBox.Show("是否使终端[" + currentTerid + "下线?", GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                {
                    return;
                }
                SocketEntity msmqEntity = new SocketEntity();
                msmqEntity.MsgType = ConstValue.MSMQTYPE.Cmd_Online;
                msmqEntity.DevId = Convert.ToInt16(currentTerid);
                msmqEntity.DevType = ConstValue.DEV_TYPE.OLWQ_CTRL;
                msmqEntity.AllowOnline = false;
                GlobalValue.SocketMgr.SendMessage(msmqEntity);
            }
        }

        private void gridViewTer_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                gridControlTer.ContextMenuStrip = null;
                GridHitInfo hInfo = gridViewTer.CalcHitInfo(new Point(e.X, e.Y));
                if (!hInfo.InDataRow || !calldataEnable)
                    return;
                gridViewTer.FocusedRowHandle = hInfo.RowHandle;
                if (hInfo.RowHandle > -1)
                {
                    string isOnline = gridViewTer.GetRowCellValue(hInfo.RowHandle, "IsOnLine").ToString();
                    currentTerid = gridViewTer.GetRowCellValue(hInfo.RowHandle, "TerminalID").ToString();
                    gridControlTer.ContextMenuStrip = MenuOnLine;
                }
            }
        }

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.RowHandle > -1 && e.CellValue != null)
            {
                string terId = gridView_data.GetRowCellValue(e.RowHandle, "TerminalID").ToString().Trim();
                OLWQChartForm.TerminalID = terId;
                OLWQChartForm detailForm = new OLWQChartForm();
                detailForm.ShowDialog();
            }
        }

        private void gridView_data_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.Caption == "浊度(NTU)" || e.Column.Caption == "余氯(PPM)" || e.Column.Caption == "PH(ph)" || e.Column.Caption == "电导率(us/cm)")
            {
                if (update_config)
                    return;
                int rowhandle = e.RowHandle;
                if (gridView_data.GetRow(rowhandle) != null)
                {
                    if (lst_config != null && lst_config.Count > 0)
                    {
                        int config_index = -1;
                        for(int i = 0; i<lst_config.Count;i++)
                        {
                            if (lst_config[i].TerId.Trim() == gridView_data.GetRowCellValue(rowhandle, "TerminalID").ToString().Trim())
                            {
                                config_index = i;
                                break;
                            }
                        }
                        if (config_index == -1)
                            return;

                        if (e.Column.Caption == "浊度(NTU)" && lst_config[config_index].enableTurbidityAlarm)
                        {
                            float turbidity = Convert.ToSingle(gridView_data.GetRowCellValue(rowhandle, "Turbidity"));
                            if (turbidity < lst_config[config_index].TurbidityLowLimit)
                                e.Appearance.BackColor = cLowLimit;
                            else if (turbidity > lst_config[config_index].TurbidityUpLimit)
                                e.Appearance.BackColor = cUpLimit;
                        }
                        else if (e.Column.Caption == "余氯(PPM)" && lst_config[config_index].enableResidualClAlarm)
                        {
                            float residualCl = Convert.ToSingle(gridView_data.GetRowCellValue(rowhandle, "ResidualCl"));
                            if (residualCl < lst_config[config_index].TurbidityLowLimit)
                                e.Appearance.BackColor = cLowLimit;
                            else if (residualCl > lst_config[config_index].TurbidityUpLimit)
                                e.Appearance.BackColor = cUpLimit;
                        }
                        else if (e.Column.Caption == "PH(ph)" && lst_config[config_index].enablePHAlarm)
                        {
                            float ph = Convert.ToSingle(gridView_data.GetRowCellValue(rowhandle, "PH"));
                            if (ph < lst_config[config_index].TurbidityLowLimit)
                                e.Appearance.BackColor = cLowLimit;
                            else if (ph > lst_config[config_index].TurbidityUpLimit)
                                e.Appearance.BackColor = cUpLimit;
                        }
                        else if (e.Column.Caption == "电导率(us/cm)" && lst_config[config_index].enableConductivityAlarm)
                        {
                            float conductivity = Convert.ToSingle(gridView_data.GetRowCellValue(rowhandle, "Conductivity"));
                            if (conductivity < lst_config[config_index].TurbidityLowLimit)
                                e.Appearance.BackColor = cLowLimit;
                            else if (conductivity > lst_config[config_index].TurbidityUpLimit)
                                e.Appearance.BackColor = cUpLimit;
                        }
                    }
                }
            }
        }

    }
}
