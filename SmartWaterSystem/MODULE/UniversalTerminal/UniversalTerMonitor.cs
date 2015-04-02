using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraEditors;
using BLL;
using System.Collections;
using DevExpress.Utils.Helpers;
using Entity;

namespace SmartWaterSystem
{
    public partial class UniversalTerMonitor : BaseView, IUniversalTerMonitor
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("UniversalTerMonitor");

        public UniversalTerMonitor()
        {
            InitializeComponent();
        }

        //public UniversalTerParm(FrmSystem parentform)
        //{
        //    InitializeComponent();
        //}

        private void UniversalTerParm_Load(object sender, EventArgs e)
        {
            InitGridData();
        }

        private void SetGridDataProperties()
        {
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
            //advBandedGridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //advBandedGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

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
            //columnId.FieldName = "ID";

            GridBand bandID = view.Bands.AddBand("ID");
            bandID.Columns.Add(columnId);

            UniversalWayTypeBLL typeBll = new UniversalWayTypeBLL();
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
                                bandChild.Columns.Add(column_child);
                                GridBand bandGrandChild=bandChild.Children.AddBand(ChildNode.Unit);
                                bandGrandChild.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
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
                        //column_one.FieldName = "ID";

                        bandParent.Columns.Add(column_parent);
                    }
                }
            }

            view.EndDataUpdate();//结束数据的编辑
            view.EndUpdate();   //结束视图的编辑
        }

        private void InitGridData()
        {
            try
            {
                SetGridDataProperties();
                //binding data
                gridControl_data.DataSource = null;
            }
            catch (Exception ex)
            {
                logger.ErrorException("InitGridData", ex);
                XtraMessageBox.Show("初始化列表发生错误!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
