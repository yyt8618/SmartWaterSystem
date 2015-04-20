using Common;
namespace SmartWaterSystem
{
    partial class UniversalCallData
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
            this.btnOnLine = new DevExpress.XtraEditors.SimpleButton();
            this.btnCallData = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl_data = new DevExpress.XtraGrid.GridControl();
            this.advBandedGridView1 = new DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView();
            this.groupControl6 = new DevExpress.XtraEditors.GroupControl();
            this.gridControlTer = new DevExpress.XtraGrid.GridControl();
            this.gridViewTer = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemPictureEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.repositoryItemImageEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemImageEdit();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_data)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advBandedGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).BeginInit();
            this.groupControl6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.btnOnLine);
            this.groupControl3.Controls.Add(this.btnCallData);
            this.groupControl3.Controls.Add(this.gridControl_data);
            this.groupControl3.Location = new System.Drawing.Point(103, 5);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(694, 483);
            this.groupControl3.TabIndex = 4;
            this.groupControl3.Text = "数据列表";
            // 
            // btnOnLine
            // 
            this.btnOnLine.Location = new System.Drawing.Point(509, -2);
            this.btnOnLine.Name = "btnOnLine";
            this.btnOnLine.Size = new System.Drawing.Size(75, 23);
            this.btnOnLine.TabIndex = 5;
            this.btnOnLine.Text = "实时在线";
            this.btnOnLine.Click += new System.EventHandler(this.btnOnLine_Click);
            // 
            // btnCallData
            // 
            this.btnCallData.Location = new System.Drawing.Point(428, -2);
            this.btnCallData.Name = "btnCallData";
            this.btnCallData.Size = new System.Drawing.Size(75, 23);
            this.btnCallData.TabIndex = 5;
            this.btnCallData.Text = "实时招测";
            this.btnCallData.Click += new System.EventHandler(this.btnCallData_Click);
            // 
            // gridControl_data
            // 
            this.gridControl_data.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_data.Location = new System.Drawing.Point(2, 22);
            this.gridControl_data.MainView = this.advBandedGridView1;
            this.gridControl_data.Name = "gridControl_data";
            this.gridControl_data.Size = new System.Drawing.Size(690, 459);
            this.gridControl_data.TabIndex = 1;
            this.gridControl_data.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.advBandedGridView1});
            // 
            // advBandedGridView1
            // 
            this.advBandedGridView1.GridControl = this.gridControl_data;
            this.advBandedGridView1.Name = "advBandedGridView1";
            this.advBandedGridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.advBandedGridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.advBandedGridView1.OptionsBehavior.ReadOnly = true;
            this.advBandedGridView1.OptionsView.ShowGroupPanel = false;
            this.advBandedGridView1.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.advBandedGridView1_CustomDrawRowIndicator);
            // 
            // groupControl6
            // 
            this.groupControl6.Controls.Add(this.gridControlTer);
            this.groupControl6.Location = new System.Drawing.Point(4, 5);
            this.groupControl6.Name = "groupControl6";
            this.groupControl6.Size = new System.Drawing.Size(95, 483);
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
            this.repositoryItemImageEdit1,
            this.repositoryItemPictureEdit1});
            this.gridControlTer.Size = new System.Drawing.Size(91, 459);
            this.gridControlTer.TabIndex = 0;
            this.gridControlTer.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewTer});
            // 
            // gridViewTer
            // 
            this.gridViewTer.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3});
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
            this.gridViewTer.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.gridViewTer_RowCellClick);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "状态";
            this.gridColumn1.ColumnEdit = this.repositoryItemPictureEdit1;
            this.gridColumn1.FieldName = "img";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsColumn.AllowFocus = false;
            this.gridColumn1.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn1.OptionsColumn.AllowMove = false;
            this.gridColumn1.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn1.OptionsColumn.TabStop = false;
            this.gridColumn1.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn1.OptionsFilter.AllowFilter = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 32;
            // 
            // repositoryItemPictureEdit1
            // 
            this.repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
            this.repositoryItemPictureEdit1.ReadOnly = true;
            this.repositoryItemPictureEdit1.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "终端编号";
            this.gridColumn2.FieldName = "TerminalID";
            this.gridColumn2.ImageAlignment = System.Drawing.StringAlignment.Center;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsColumn.AllowMove = false;
            this.gridColumn2.OptionsColumn.ReadOnly = true;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 57;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "IsOnLine";
            this.gridColumn3.FieldName = "IsOnLine";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsColumn.AllowFocus = false;
            this.gridColumn3.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn3.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn3.OptionsColumn.AllowMove = false;
            this.gridColumn3.OptionsColumn.AllowShowHide = false;
            this.gridColumn3.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn3.OptionsColumn.ReadOnly = true;
            this.gridColumn3.OptionsColumn.ShowCaption = false;
            this.gridColumn3.OptionsColumn.TabStop = false;
            this.gridColumn3.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn3.OptionsFilter.AllowFilter = false;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // repositoryItemImageEdit1
            // 
            this.repositoryItemImageEdit1.Name = "repositoryItemImageEdit1";
            // 
            // UniversalCallData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl6);
            this.Controls.Add(this.groupControl3);
            this.Name = "UniversalCallData";
            this.Size = new System.Drawing.Size(797, 494);
            this.Load += new System.EventHandler(this.UniversalTerParm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_data)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advBandedGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).EndInit();
            this.groupControl6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraGrid.GridControl gridControl_data;
        private DevExpress.XtraEditors.GroupControl groupControl6;
        private DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView advBandedGridView1;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraGrid.GridControl gridControlTer;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewTer;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.SimpleButton btnCallData;
        private DevExpress.XtraEditors.SimpleButton btnOnLine;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageEdit repositoryItemImageEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit repositoryItemPictureEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;

    }
}