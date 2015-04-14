using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraEditors;
using BLL;
using Entity;
using System.Data;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace SmartWaterSystem
{
    public partial class UniversalCallData : BaseView, IUniversalCallData
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("UniversalCallData");
        UniversalWayTypeBLL typeBll = new UniversalWayTypeBLL();
        List<int> lst_datacolumnIdIndex = new List<int>();  //数据列id索引
        string currentTerId = "";       //当前选中的终端编号
        public UniversalCallData()
        {
            InitializeComponent();
        }

        private void UniversalTerParm_Load(object sender, EventArgs e)
        {
            timer1.Interval = 20 * 1000;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Enabled = true;

            GlobalValue.MSMQMgr.MSMQEvent += new MSMQHandler(MSMQMgr_MSMQEvent);
            ShowGridTerData(null);

            //获取终端在线状态
            MSMQEntity msmqEntity = new MSMQEntity();
            msmqEntity.MsgType = ConstValue.MSMQTYPE.Get_OnLineState;
            GlobalValue.MSMQMgr.SendMessage(msmqEntity);
        }

        void MSMQMgr_MSMQEvent(object sender, MSMQEventArgs e)
        {
            if (e.msmqEntity != null && e.msmqEntity.MsgType == ConstValue.MSMQTYPE.Data_OnLineState)
            {
                InvokeShowGridTerData(e.msmqEntity.lstOnLine);
            }
        }

        void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void SetGridDataProperties(string id)
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

            //BandedGridColumn columnId = new BandedGridColumn();
            //columnId.Caption = "ID";
            //columnId.Name = "gridColumn0";
            //columnId.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            //columnId.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            //columnId.OptionsColumn.AllowMove = false;
            //columnId.OptionsEditForm.StartNewRow = true;
            //columnId.OptionsFilter.AllowAutoFilter = false;
            //columnId.OptionsFilter.AllowFilter = false;
            //columnId.Visible = true;
            //columnId.VisibleIndex = 0;
            //columnId.Width = 40;
            //columnId.FieldName = "TerminalID";

            //GridBand bandID = view.Bands.AddBand("ID"); 
            //bandID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //bandID.Columns.Add(columnId);

            List<UniversalWayTypeEntity> lst_TypeWay_Parent = typeBll.GetConfigPointID(id.ToString());
            if (lst_TypeWay_Parent != null)
            {
                int index = 0;
                foreach (UniversalWayTypeEntity ParentNode in lst_TypeWay_Parent)
                {
                    GridBand bandParent = view.Bands.AddBand(ParentNode.Name);
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
                                //column_child.Tag = ChildNode.ID;
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
                        //column_parent.Tag = ParentNode.ID;
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

        private void ShowTerData(string id)
        {
            if (lst_datacolumnIdIndex.Count > 0 && lst_datacolumnIdIndex != null && !string.IsNullOrEmpty(id))
            {
                List<string> lst_terid = new List<string>();
                lst_terid.Add(id);
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

        private delegate void ShowGridTerDataHandler(List<OnLineTerEntity> lstOnLine);
        private void InvokeShowGridTerData(List<OnLineTerEntity> lstOnLine)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((ShowGridTerDataHandler)delegate(List<OnLineTerEntity>  onlinedata)
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
            DataColumn col = dt_bind.Columns.Add("img");
            col.DataType = System.Type.GetType("System.Byte[]");
            dt_bind.Columns.Add("TerminalID");
            dt_bind.Columns.Add("IsOnLine");  //是否在线状态标志 1:在线,0:不在线
            int selectIndex = -1;
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
                    dt_bind.Rows.Add(new object[] { Bitmap2Byte(SmartWaterSystem.Properties.Resources.亮灯16x25), terid,1 });
                else
                    dt_bind.Rows.Add(new object[] { Bitmap2Byte(SmartWaterSystem.Properties.Resources.灭灯16x25), terid,0 });

                if (currentTerId == terid)
                {
                    selectIndex = i;
                }
            }
            gridControlTer.DataSource = dt_bind;
            gridControlTer.RefreshDataSource();

            if (selectIndex!= -1)
            {
                gridViewTer.SelectRow(selectIndex);
            }
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

        private void gridViewTer_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.RowHandle > -1 && e.Column.Caption == "终端编号")
            {
                currentTerId = e.CellValue.ToString().Trim();
                if (!string.IsNullOrEmpty(currentTerId))
                {
                    SetGridDataProperties(currentTerId);
                    ShowTerData(currentTerId);
                }
            }
        }

        private void btnOnLine_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentTerId))
            {
                XtraMessageBox.Show("请选择操作的终端!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MSMQEntity msmqEntity = new MSMQEntity();
            msmqEntity.MsgType = ConstValue.MSMQTYPE.Cmd_Online;
            msmqEntity.DevId = Convert.ToInt16(currentTerId);
            msmqEntity.DevType = ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
            msmqEntity.AllowOnline = true;
            GlobalValue.MSMQMgr.SendMessage(msmqEntity);
        }

        private void btnCallData_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentTerId))
            {
                XtraMessageBox.Show("请选择操作的终端!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

    }
}
