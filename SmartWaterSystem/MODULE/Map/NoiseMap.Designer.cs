namespace SmartWaterSystem
{
    partial class NoiseMap
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
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.btnFullScreen = new DevExpress.XtraEditors.SimpleButton();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumnName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumnID = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.dockPanel1.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(175, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(622, 494);
            this.webBrowser1.TabIndex = 0;
            // 
            // btnFullScreen
            // 
            this.btnFullScreen.Image = global::SmartWaterSystem.Properties.Resources.fullscreen;
            this.btnFullScreen.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnFullScreen.Location = new System.Drawing.Point(709, 0);
            this.btnFullScreen.LookAndFeel.SkinName = "Visual Studio 2013 Light";
            this.btnFullScreen.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnFullScreen.Name = "btnFullScreen";
            this.btnFullScreen.Size = new System.Drawing.Size(64, 64);
            this.btnFullScreen.TabIndex = 3;
            this.btnFullScreen.Click += new System.EventHandler(this.btnFullScreen_Click);
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanel1});
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl"});
            // 
            // dockPanel1
            // 
            this.dockPanel1.Controls.Add(this.dockPanel1_Container);
            this.dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.dockPanel1.ID = new System.Guid("23e8ea95-0a6d-47f2-8fec-5542ecf8d8b8");
            this.dockPanel1.Location = new System.Drawing.Point(0, 0);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.OriginalSize = new System.Drawing.Size(175, 200);
            this.dockPanel1.Size = new System.Drawing.Size(175, 494);
            this.dockPanel1.Text = "未定位终端列表";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.treeList1);
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(167, 467);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // treeList1
            // 
            this.treeList1.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumnName,
            this.treeListColumnID,
            this.treeListColumn1});
            this.treeList1.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList1.Location = new System.Drawing.Point(0, 0);
            this.treeList1.Name = "treeList1";
            this.treeList1.Size = new System.Drawing.Size(167, 467);
            this.treeList1.TabIndex = 5;
            this.treeList1.Click += new System.EventHandler(this.treeList1_Click);
            // 
            // treeListColumnName
            // 
            this.treeListColumnName.Caption = "名称";
            this.treeListColumnName.FieldName = "Name";
            this.treeListColumnName.MinWidth = 32;
            this.treeListColumnName.Name = "treeListColumnName";
            this.treeListColumnName.OptionsColumn.AllowEdit = false;
            this.treeListColumnName.OptionsColumn.AllowMove = false;
            this.treeListColumnName.OptionsColumn.AllowSort = false;
            this.treeListColumnName.OptionsFilter.AllowAutoFilter = false;
            this.treeListColumnName.OptionsFilter.AllowFilter = false;
            this.treeListColumnName.Width = 104;
            // 
            // treeListColumnID
            // 
            this.treeListColumnID.Caption = "名称";
            this.treeListColumnID.FieldName = "ID";
            this.treeListColumnID.MinWidth = 34;
            this.treeListColumnID.Name = "treeListColumnID";
            this.treeListColumnID.OptionsColumn.AllowEdit = false;
            this.treeListColumnID.OptionsColumn.ReadOnly = true;
            this.treeListColumnID.Visible = true;
            this.treeListColumnID.VisibleIndex = 0;
            this.treeListColumnID.Width = 95;
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "备注";
            this.treeListColumn1.FieldName = "Remark";
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.OptionsColumn.AllowEdit = false;
            this.treeListColumn1.OptionsColumn.ReadOnly = true;
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 1;
            this.treeListColumn1.Width = 109;
            // 
            // NoiseMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnFullScreen);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.dockPanel1);
            this.Name = "NoiseMap";
            this.Size = new System.Drawing.Size(797, 494);
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.dockPanel1.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private DevExpress.XtraEditors.SimpleButton btnFullScreen;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumnID;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumnName;
        private DevExpress.XtraTreeList.TreeList treeList1;
    }
}