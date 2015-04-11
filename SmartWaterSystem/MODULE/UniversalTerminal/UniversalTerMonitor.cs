using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraEditors;
using BLL;
using Entity;
using System.Data;

namespace SmartWaterSystem
{
    public partial class UniversalTerMonitor : BaseView, IUniversalTerMonitor
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("UniversalTerMonitor");
        UniversalWayTypeBLL typeBll = new UniversalWayTypeBLL();
        List<int> lst_datacolumnIdIndex = new List<int>();  //数据列id索引
        public UniversalTerMonitor()
        {
            InitializeComponent();
        }

        private void UniversalTerParm_Load(object sender, EventArgs e)
        {
            timer1.Interval = 20 * 1000;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Enabled = true;
            //repositoryItemCheckEdit3
            InitGrid();
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
            ////表头折行设置
            //advBandedGridView1.ColumnPanelRowHeight = 40;
            //advBandedGridView1.OptionsView.AllowHtmlDrawHeaders = true;
            //advBandedGridView1.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
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

            List<UniversalWayTypeEntity> lst_TypeWay_Parent = typeBll.GetAllConfigPointID();
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
                //binding data
                InitGridData();
            }
            catch (Exception ex)
            {
                logger.ErrorException("InitGridData", ex);
                XtraMessageBox.Show("初始化列表发生错误!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void InitGridData()
        {
            ShowGridTerData();
            //ShowTerData();
        }

        private void ShowGridTerData()
        {
            gridControlTer.DataSource = null;
            DataTable dt = typeBll.GetTerminalID_Configed();

            DataTable dt_bind = new DataTable("BindTable");
            DataColumn col = dt_bind.Columns.Add("checked");
            col.DataType = System.Type.GetType("System.Boolean");
            dt_bind.Columns.Add("TerminalID");

            foreach (DataRow dr in dt.Rows)
            {
                dt_bind.Rows.Add(new object[] { false, dr["TerminalID"].ToString().Trim() });
            }
            gridControlTer.DataSource = dt_bind;
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
            DataColumn col = dt_bind.Columns.Add("checked");
            col.DataType = System.Type.GetType("System.Boolean");
            dt_bind.Columns.Add("TerminalID");

            foreach (DataRow dr in dt_config.Rows)
            {
                bool select = false;
                foreach (string terid in lst_selectIds)
                {
                    if (terid == dr["TerminalID"].ToString().Trim())
                    {
                        select = true;
                    }
                }
                dt_bind.Rows.Add(new object[] { select, dr["TerminalID"].ToString().Trim() });
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

        private void gridViewTer_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            ShowTerData();
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

    }
}
