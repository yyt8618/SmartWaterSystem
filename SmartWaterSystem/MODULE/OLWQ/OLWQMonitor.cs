using System;
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
        UniversalWayTypeBLL typeBll = new UniversalWayTypeBLL();
        List<int> lst_datacolumnIdIndex = new List<int>();  //数据列id索引
        bool calldataEnable = false;  //是否开启招测
        string currentTerid = "";       //当前操作的终端编号
        List<OnLineTerEntity> OnLineTers = null;  //在线终端记录
        public OLWQMonitor()
        {
            InitializeComponent();
            GlobalValue.MSMQMgr.MSMQEvent += new MSMQHandler(MSMQMgr_MSMQEvent);
            timer1.Interval = 20 * 1000;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Enabled = true;
        }

        private void UniversalTerParm_Load(object sender, EventArgs e)
        {
            //是否启用招测
            calldataEnable=Settings.Instance.GetBool(SettingKeys.CallDataEnable);
            GridColumn calldataCol = gridViewTer.Columns["OnLinePic"];
            calldataCol.Visible = calldataEnable;
            
            InitGrid();

            if (calldataEnable)
            {
                //获取终端在线状态
                MSMQEntity msmqEntity = new MSMQEntity();
                msmqEntity.MsgType = ConstValue.MSMQTYPE.Get_OnLineState;
                GlobalValue.MSMQMgr.SendMessage(msmqEntity);
            }

            #region 
            /*
            ceCallData.CheckedChanged += new EventHandler(ceCallData_CheckedChanged);
            ImageList imageList = new ImageList();
            imageList.ImageSize = new System.Drawing.Size(12, 12);
            imageList.ColorDepth = ColorDepth.Depth32Bit;
            imageList.Images.Add(SmartWaterSystem.Properties.Resources.灭灯16x25);
            imageList.Images.Add(SmartWaterSystem.Properties.Resources.亮灯16x25);
            string[] s = new string[] { "关","开"};
            for (int i = 0; i < s.Length; i++)
                ImageComboBox_Online.Items.Add(new ImageComboBoxItem(s[i], i, i));

            this.ImageComboBox_Online.SmallImages = imageList;
             * */
            #endregion
        }

        void MSMQMgr_MSMQEvent(object sender, MSMQEventArgs e)
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

        private void SetGridDataProperties()
        {
            lst_datacolumnIdIndex = new List<int>();

            advBandedGridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            advBandedGridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            advBandedGridView1.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Click;
            advBandedGridView1.OptionsBehavior.Editable = false;
            advBandedGridView1.OptionsMenu.EnableColumnMenu = false;
            advBandedGridView1.OptionsCustomization.AllowFilter = false;
            advBandedGridView1.OptionsView.ShowColumnHeaders = false;
            advBandedGridView1.OptionsView.ColumnAutoWidth = true;
            advBandedGridView1.IndicatorWidth = 27;
            advBandedGridView1.OptionsView.ShowFooter = true;
            ////表头及行内容居中显示
            advBandedGridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            advBandedGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            BandedGridView view = advBandedGridView1 as BandedGridView;
            view.BeginUpdate();
            view.BeginDataUpdate();
            view.Bands.Clear();

            BandedGridColumn columnId = new BandedGridColumn();
            columnId.Caption = "ID";
            columnId.Name = "gridColumn0";
            columnId.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            columnId.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            columnId.OptionsColumn.AllowMove = false;
            columnId.OptionsEditForm.StartNewRow = true;
            columnId.OptionsFilter.AllowAutoFilter = false;
            columnId.OptionsFilter.AllowFilter = false;
            columnId.Visible = true;
            columnId.VisibleIndex = 0;
            columnId.Width = 40;
            columnId.FieldName = "TerminalID";

            GridBand bandID = view.Bands.AddBand("ID"); 
            bandID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            bandID.Columns.Add(columnId);

            List<UniversalWayTypeEntity> lst_TypeWay_Parent = typeBll.GetConfigPointID("");
            if (lst_TypeWay_Parent != null)
            {
                int index = 0;
                foreach (UniversalWayTypeEntity ParentNode in lst_TypeWay_Parent)
                {
                    GridBand bandParent = view.Bands.AddBand(ParentNode.Name);
                    bandParent.Tag = ParentNode.ID;
                    bandParent.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    if (ParentNode.HaveChild)  //have child
                    {
                        List<UniversalWayTypeEntity> lst_TypeWay_Child = typeBll.Select("WHERE ParentID='" + ParentNode.ID + "' ORDER BY Sequence");
                        if (lst_TypeWay_Child != null)
                        {
                            foreach (UniversalWayTypeEntity ChildNode in lst_TypeWay_Child)
                            {
                                index++;
                                GridBand bandChild = bandParent.Children.AddBand(ChildNode.Name);
                                bandChild.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                BandedGridColumn column_child = new BandedGridColumn();
                                column_child.Caption = ParentNode.Name;
                                column_child.Name = "gridColumn" + index;
                                column_child.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
                                column_child.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                                column_child.OptionsColumn.AllowMove = false;
                                column_child.OptionsEditForm.StartNewRow = true;
                                column_child.OptionsFilter.AllowAutoFilter = false;
                                column_child.OptionsFilter.AllowFilter = false;
                                column_child.Visible = true;
                                column_child.VisibleIndex = index - 1;
                                column_child.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                column_child.FieldName = "column" + index;
                                column_child.Tag = ChildNode.ID;
                                bandChild.Columns.Add(column_child);
                                GridBand bandUnit=bandChild.Children.AddBand(ChildNode.Unit);
                                bandUnit.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                if (!lst_datacolumnIdIndex.Contains(ChildNode.ID))
                                    lst_datacolumnIdIndex.Add(ChildNode.ID);
                            }
                        }
                    }
                    else  //alone
                    {
                        index++;
                        BandedGridColumn column_parent = new BandedGridColumn();
                        column_parent.Caption = ParentNode.Name;
                        column_parent.Name = "gridColumn" + index;
                        column_parent.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
                        column_parent.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                        column_parent.OptionsColumn.AllowMove = false;
                        column_parent.OptionsEditForm.StartNewRow = true;
                        column_parent.OptionsFilter.AllowAutoFilter = false;
                        column_parent.OptionsFilter.AllowFilter = false;
                        column_parent.Visible = true;
                        column_parent.VisibleIndex = index - 1;
                        column_parent.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        column_parent.FieldName = "column" + index;
                        column_parent.Tag = ParentNode.ID;
                        bandParent.Columns.Add(column_parent);
                        GridBand bandUnit = bandParent.Children.AddBand(ParentNode.Unit);
                        bandUnit.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        if (!lst_datacolumnIdIndex.Contains(ParentNode.ID))
                            lst_datacolumnIdIndex.Add(ParentNode.ID);
                    }
                }
            }

            view.EndDataUpdate();//结束数据的编辑
            view.EndUpdate();   //结束视图的编辑
        }

        private void InitGrid()
        {
            try
            {
                SetGridDataProperties();
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
            //ShowTerData();
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
            DataTable dt = typeBll.GetTerminalID_Configed();

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
                        if (terentity.DevType == ConstValue.DEV_TYPE.UNIVERSAL_CTRL && terentity.DevId.ToString() == terid)
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
            SetGridDataProperties();
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
            DataTable dt_config = typeBll.GetTerminalID_Configed();

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
                        if (terentity.DevType == ConstValue.DEV_TYPE.UNIVERSAL_CTRL && terentity.DevId.ToString() == terid)
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

            if (lst_datacolumnIdIndex.Count > 0 && lst_datacolumnIdIndex != null && lst_terid.Count > 0)
            {
                DataTable dt = typeBll.GetTerminalDataToShow(lst_terid, lst_datacolumnIdIndex);
                gridControl_data.DataSource = dt;
                gridControl_data.RefreshDataSource();
            }
            else
            {
                gridControl_data.DataSource = null;
                gridControl_data.RefreshDataSource();
            }
        }

        private void advBandedGridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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

        private void advBandedGridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.RowHandle > -1 && e.Column.Tag != null && e.CellValue!=null)
            {
                string terId = advBandedGridView1.GetRowCellValue(e.RowHandle, "TerminalID").ToString().Trim();
                UniversalChartForm.TerminalID = terId;
                UniversalChartForm.TypeId = Convert.ToInt32(e.Column.Tag);
                UniversalChartForm.ColumnName = e.Column.Caption.Trim();
                UniversalChartForm detailForm = new UniversalChartForm();
                detailForm.ShowDialog();
            }
        }

        private void advBandedGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            System.Drawing.Point pt = new System.Drawing.Point(e.X, e.Y);
            BandedGridHitInfo hit = advBandedGridView1.CalcHitInfo(e.Location);

            if (!hit.InBandPanel || !calldataEnable)
            {
                return;
            }

            string terminalid = "";
            for (int i = 0; i < advBandedGridView1.RowCount; i++)
            {
                if (advBandedGridView1.IsRowSelected(i))
                {
                    terminalid = advBandedGridView1.GetRowCellValue(i, "TerminalID").ToString().Trim();
                    break;
                }
            }

            if (string.IsNullOrEmpty(terminalid))
            {
                XtraMessageBox.Show("请选中一条记录操作!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            GridBand gb1 = hit.Band;
            if (DialogResult.No == XtraMessageBox.Show("是否确定发送招测" + gb1.Caption.Trim() + "命令？", GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
            {
                return;
            }
            if (gb1.Tag != null)
            {
                if (calldataEnable)
                {
                    MSMQEntity msmqEntity = new MSMQEntity();
                    msmqEntity.MsgType = ConstValue.MSMQTYPE.Cmd_CallData;
                    msmqEntity.DevId = Convert.ToInt16(currentTerid);
                    msmqEntity.DevType = ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
                    msmqEntity.CallDataType = new CallDataTypeEntity();
                    int config_Seq = typeBll.GetCofingSequence(currentTerid.Trim(), gb1.Tag.ToString().Trim());
                    if (config_Seq == -1)
                    {
                        return;
                    }
                    else if (config_Seq == 1)
                        msmqEntity.CallDataType.GetSim1 = true;
                    else if (config_Seq == 2)
                        msmqEntity.CallDataType.GetSim2 = true;
                    else if (config_Seq == 3)
                        msmqEntity.CallDataType.GetSim3 = true;
                    else if (config_Seq >= 4 && config_Seq <= 8)
                        msmqEntity.CallDataType.GetPluse = true;
                    else if (config_Seq == 9)
                        msmqEntity.CallDataType.GetRS4851 = true;
                    else if (config_Seq == 10)
                        msmqEntity.CallDataType.GetRS4852 = true;
                    else if (config_Seq == 11)
                        msmqEntity.CallDataType.GetRS4853 = true;
                    else if (config_Seq == 12)
                        msmqEntity.CallDataType.GetRS4854 = true;
                    else if (config_Seq == 13)
                        msmqEntity.CallDataType.GetRS4855 = true;
                    else if (config_Seq == 14)
                        msmqEntity.CallDataType.GetRS4856 = true;
                    else if (config_Seq == 15)
                        msmqEntity.CallDataType.GetRS4857 = true;
                    else if (config_Seq == 16)
                        msmqEntity.CallDataType.GetRS4858 = true;
                    
                    GlobalValue.MSMQMgr.SendMessage(msmqEntity);
                }
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
                MSMQEntity msmqEntity = new MSMQEntity();
                msmqEntity.MsgType = ConstValue.MSMQTYPE.Cmd_Online;
                msmqEntity.DevId = Convert.ToInt16(currentTerid);
                msmqEntity.DevType = ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
                msmqEntity.AllowOnline = true;
                GlobalValue.MSMQMgr.SendMessage(msmqEntity);
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
                MSMQEntity msmqEntity = new MSMQEntity();
                msmqEntity.MsgType = ConstValue.MSMQTYPE.Cmd_Online;
                msmqEntity.DevId = Convert.ToInt16(currentTerid);
                msmqEntity.DevType = ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
                msmqEntity.AllowOnline = false;
                GlobalValue.MSMQMgr.SendMessage(msmqEntity);
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
                    //if (isOnline.Trim() == "1")
                    //{
                    //    MenuOnLine.Items[0].Visible = false;
                    //    MenuOnLine.Items[1].Visible = true;
                    //    gridControlTer.ContextMenuStrip = MenuOnLine;
                    //}
                    //else
                    //{
                    //    MenuOnLine.Items[0].Visible = true;
                    //    MenuOnLine.Items[1].Visible = false;
                    //    gridControlTer.ContextMenuStrip = MenuOnLine;
                    //}
                    gridControlTer.ContextMenuStrip = MenuOnLine;
                }
            }
        }

    }
}
