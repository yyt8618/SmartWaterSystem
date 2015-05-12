using Common;
namespace SmartWaterSystem
{
    partial class OLWQMonitor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.gridControl_data = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupControl6 = new DevExpress.XtraEditors.GroupControl();
            this.gridControlTer = new DevExpress.XtraGrid.GridControl();
            this.gridViewTer = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_OnLineState = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemPictureEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.gridColumn_OnLine = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ceCallData = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.ImageComboBox_Online = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.MenuOnLine = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItem_Online = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Offline = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_data)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).BeginInit();
            this.groupControl6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceCallData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageComboBox_Online)).BeginInit();
            this.MenuOnLine.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.gridControl_data);
            this.groupControl3.Location = new System.Drawing.Point(119, 5);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(678, 483);
            this.groupControl3.TabIndex = 4;
            this.groupControl3.Text = "数据列表";
            // 
            // gridControl_data
            // 
            this.gridControl_data.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_data.Location = new System.Drawing.Point(2, 22);
            this.gridControl_data.MainView = this.gridView1;
            this.gridControl_data.Name = "gridControl_data";
            this.gridControl_data.Size = new System.Drawing.Size(674, 459);
            this.gridControl_data.TabIndex = 1;
            this.gridControl_data.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9});
            this.gridView1.GridControl = this.gridControl_data;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsBehavior.ReadOnly = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            this.gridView1.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.gridView1_RowCellClick);
            this.gridView1.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView1_CustomDrawRowIndicator);
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "终端编号";
            this.gridColumn3.FieldName = "TerminalID";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsColumn.AllowMove = false;
            this.gridColumn3.OptionsColumn.ReadOnly = true;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 0;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "终端名称";
            this.gridColumn4.FieldName = "TerminalName";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.OptionsColumn.AllowMove = false;
            this.gridColumn4.OptionsColumn.ReadOnly = true;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 1;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "浊度";
            this.gridColumn5.FieldName = "Turbidity";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.OptionsColumn.AllowMove = false;
            this.gridColumn5.OptionsColumn.ReadOnly = true;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 2;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "余氯";
            this.gridColumn6.FieldName = "ResidualCl";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowEdit = false;
            this.gridColumn6.OptionsColumn.AllowMove = false;
            this.gridColumn6.OptionsColumn.ReadOnly = true;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 3;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "PH";
            this.gridColumn7.FieldName = "PH";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowEdit = false;
            this.gridColumn7.OptionsColumn.AllowMove = false;
            this.gridColumn7.OptionsColumn.ReadOnly = true;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 4;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "电导率";
            this.gridColumn8.FieldName = "Conductivity";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsColumn.AllowEdit = false;
            this.gridColumn8.OptionsColumn.AllowMove = false;
            this.gridColumn8.OptionsColumn.ReadOnly = true;
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 5;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "温度";
            this.gridColumn9.FieldName = "Temperature";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsColumn.AllowEdit = false;
            this.gridColumn9.OptionsColumn.AllowMove = false;
            this.gridColumn9.OptionsColumn.ReadOnly = true;
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 6;
            // 
            // groupControl6
            // 
            this.groupControl6.Controls.Add(this.gridControlTer);
            this.groupControl6.Location = new System.Drawing.Point(4, 5);
            this.groupControl6.Name = "groupControl6";
            this.groupControl6.Size = new System.Drawing.Size(111, 483);
            this.groupControl6.TabIndex = 4;
            this.groupControl6.Text = "列表";
            // 
            // gridControlTer
            // 
            this.gridControlTer.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControlTer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlTer.Location = new System.Drawing.Point(2, 22);
            this.gridControlTer.MainView = this.gridViewTer;
            this.gridControlTer.Name = "gridControlTer";
            this.gridControlTer.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1,
            this.ceCallData,
            this.ImageComboBox_Online,
            this.repositoryItemPictureEdit1});
            this.gridControlTer.Size = new System.Drawing.Size(107, 459);
            this.gridControlTer.TabIndex = 0;
            this.gridControlTer.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewTer});
            // 
            // gridViewTer
            // 
            this.gridViewTer.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn_OnLineState,
            this.gridColumn_OnLine});
            this.gridViewTer.GridControl = this.gridControlTer;
            this.gridViewTer.Name = "gridViewTer";
            this.gridViewTer.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewTer.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewTer.OptionsCustomization.AllowFilter = false;
            this.gridViewTer.OptionsCustomization.AllowSort = false;
            this.gridViewTer.OptionsFilter.AllowFilterEditor = false;
            this.gridViewTer.OptionsHint.ShowFooterHints = false;
            this.gridViewTer.OptionsMenu.EnableColumnMenu = false;
            this.gridViewTer.OptionsMenu.EnableFooterMenu = false;
            this.gridViewTer.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewTer.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gridViewTer.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridViewTer.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewTer.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gridViewTer.OptionsView.ShowGroupPanel = false;
            this.gridViewTer.OptionsView.ShowIndicator = false;
            this.gridViewTer.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridViewTer_CellValueChanged);
            this.gridViewTer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridViewTer_MouseDown);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "选择";
            this.gridColumn1.ColumnEdit = this.repositoryItemCheckEdit1;
            this.gridColumn1.FieldName = "checked";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.ShowCaption = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 22;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "终端号";
            this.gridColumn2.FieldName = "TerminalID";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsColumn.AllowMove = false;
            this.gridColumn2.OptionsColumn.ReadOnly = true;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 50;
            // 
            // gridColumn_OnLineState
            // 
            this.gridColumn_OnLineState.Caption = "在线";
            this.gridColumn_OnLineState.ColumnEdit = this.repositoryItemPictureEdit1;
            this.gridColumn_OnLineState.FieldName = "OnLinePic";
            this.gridColumn_OnLineState.Name = "gridColumn_OnLineState";
            this.gridColumn_OnLineState.OptionsColumn.AllowEdit = false;
            this.gridColumn_OnLineState.OptionsColumn.AllowFocus = false;
            this.gridColumn_OnLineState.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn_OnLineState.OptionsColumn.AllowMove = false;
            this.gridColumn_OnLineState.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_OnLineState.OptionsColumn.TabStop = false;
            this.gridColumn_OnLineState.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn_OnLineState.OptionsFilter.AllowFilter = false;
            this.gridColumn_OnLineState.Visible = true;
            this.gridColumn_OnLineState.VisibleIndex = 2;
            this.gridColumn_OnLineState.Width = 33;
            // 
            // repositoryItemPictureEdit1
            // 
            this.repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
            this.repositoryItemPictureEdit1.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            // 
            // gridColumn_OnLine
            // 
            this.gridColumn_OnLine.Caption = "gridColumn3";
            this.gridColumn_OnLine.FieldName = "IsOnLine";
            this.gridColumn_OnLine.Name = "gridColumn_OnLine";
            this.gridColumn_OnLine.OptionsColumn.AllowEdit = false;
            this.gridColumn_OnLine.OptionsColumn.AllowFocus = false;
            this.gridColumn_OnLine.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_OnLine.OptionsColumn.AllowMove = false;
            this.gridColumn_OnLine.OptionsColumn.AllowSize = false;
            this.gridColumn_OnLine.OptionsColumn.ReadOnly = true;
            this.gridColumn_OnLine.OptionsColumn.TabStop = false;
            this.gridColumn_OnLine.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn_OnLine.OptionsFilter.AllowFilter = false;
            // 
            // ceCallData
            // 
            this.ceCallData.AutoHeight = false;
            this.ceCallData.Name = "ceCallData";
            this.ceCallData.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // ImageComboBox_Online
            // 
            this.ImageComboBox_Online.AutoHeight = false;
            this.ImageComboBox_Online.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ImageComboBox_Online.Name = "ImageComboBox_Online";
            // 
            // MenuOnLine
            // 
            this.MenuOnLine.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_Online,
            this.MenuItem_Offline});
            this.MenuOnLine.Name = "MenuOnLine";
            this.MenuOnLine.ShowImageMargin = false;
            this.MenuOnLine.Size = new System.Drawing.Size(74, 48);
            // 
            // MenuItem_Online
            // 
            this.MenuItem_Online.Name = "MenuItem_Online";
            this.MenuItem_Online.Size = new System.Drawing.Size(127, 22);
            this.MenuItem_Online.Text = "在线";
            this.MenuItem_Online.Click += new System.EventHandler(this.MenuItem_Online_Click);
            // 
            // MenuItem_Offline
            // 
            this.MenuItem_Offline.Name = "MenuItem_Offline";
            this.MenuItem_Offline.Size = new System.Drawing.Size(127, 22);
            this.MenuItem_Offline.Text = "下线";
            this.MenuItem_Offline.Click += new System.EventHandler(this.MenuItem_Offline_Click);
            // 
            // OLWQMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl6);
            this.Controls.Add(this.groupControl3);
            this.Name = "OLWQMonitor";
            this.Size = new System.Drawing.Size(797, 494);
            this.Load += new System.EventHandler(this.OLWQMonitor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_data)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).EndInit();
            this.groupControl6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceCallData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageComboBox_Online)).EndInit();
            this.MenuOnLine.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraGrid.GridControl gridControl_data;
        private DevExpress.XtraEditors.GroupControl groupControl6;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraGrid.GridControl gridControlTer;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewTer;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_OnLineState;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit ceCallData;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox ImageComboBox_Online;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit repositoryItemPictureEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_OnLine;
        private System.Windows.Forms.ContextMenuStrip MenuOnLine;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Online;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Offline;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;

    }
}