namespace NoiseAnalysisSystem
{
    partial class FrmSystem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSystem));
            this.splashScreenmanager = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::NoiseAnalysisSystem.WaitForm1), true, true);
            this.panelControlMain = new DevExpress.XtraEditors.PanelControl();
            this.barStaticItemWait = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barBtnFFT = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnSetSerial = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnSetTemplate = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnSetVoice = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnSetParam = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnSetClose = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnSerialOpen = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnSerialClose = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnSetAbout = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnCompare = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageSys = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup5 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.ControlribbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.navBarControl1 = new DevExpress.XtraNavBar.NavBarControl();
            this.navBarGroup1 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarItem1 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarSeparatorItem1 = new DevExpress.XtraNavBar.NavBarSeparatorItem();
            this.navBarItem2 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarSeparatorItem2 = new DevExpress.XtraNavBar.NavBarSeparatorItem();
            this.navBarItem3 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarSeparatorItem3 = new DevExpress.XtraNavBar.NavBarSeparatorItem();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControlMain
            // 
            this.panelControlMain.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControlMain.Location = new System.Drawing.Point(193, 149);
            this.panelControlMain.Name = "panelControlMain";
            this.panelControlMain.Size = new System.Drawing.Size(804, 492);
            this.panelControlMain.TabIndex = 1;
            // 
            // barStaticItemWait
            // 
            this.barStaticItemWait.Caption = "欢迎使用本系统";
            this.barStaticItemWait.Id = 0;
            this.barStaticItemWait.Name = "barStaticItemWait";
            this.barStaticItemWait.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticItem2
            // 
            this.barStaticItem2.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barStaticItem2.Caption = "上海敢创水业科技有限公司";
            this.barStaticItem2.Id = 1;
            this.barStaticItem2.Name = "barStaticItem2";
            this.barStaticItem2.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.barStaticItemWait,
            this.barStaticItem2,
            this.barBtnFFT,
            this.barBtnSetSerial,
            this.barBtnSetTemplate,
            this.barBtnSetVoice,
            this.barBtnSetParam,
            this.barBtnSetClose,
            this.barBtnSerialOpen,
            this.barBtnSerialClose,
            this.barBtnSetAbout,
            this.barBtnCompare});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 27;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPageSys});
            this.ribbonControl1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2010;
            this.ribbonControl1.Size = new System.Drawing.Size(999, 149);
            this.ribbonControl1.StatusBar = this.ribbonStatusBar1;
            // 
            // barBtnFFT
            // 
            this.barBtnFFT.Caption = "傅里叶分析";
            this.barBtnFFT.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.barBtnFFT.Glyph = ((System.Drawing.Image)(resources.GetObject("barBtnFFT.Glyph")));
            this.barBtnFFT.Id = 5;
            this.barBtnFFT.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barBtnFFT.LargeGlyph")));
            this.barBtnFFT.Name = "barBtnFFT";
            this.barBtnFFT.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnFFT_ItemClick);
            // 
            // barBtnSetSerial
            // 
            this.barBtnSetSerial.Caption = "串口通讯";
            this.barBtnSetSerial.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.barBtnSetSerial.Glyph = ((System.Drawing.Image)(resources.GetObject("barBtnSetSerial.Glyph")));
            this.barBtnSetSerial.Id = 6;
            this.barBtnSetSerial.LargeGlyph = global::NoiseAnalysisSystem.Properties.Resources.SerialPortConfig1;
            this.barBtnSetSerial.Name = "barBtnSetSerial";
            this.barBtnSetSerial.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnSetSerial_ItemClick);
            // 
            // barBtnSetTemplate
            // 
            this.barBtnSetTemplate.Caption = "模板参数";
            this.barBtnSetTemplate.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.barBtnSetTemplate.Glyph = ((System.Drawing.Image)(resources.GetObject("barBtnSetTemplate.Glyph")));
            this.barBtnSetTemplate.Id = 7;
            this.barBtnSetTemplate.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barBtnSetTemplate.LargeGlyph")));
            this.barBtnSetTemplate.Name = "barBtnSetTemplate";
            this.barBtnSetTemplate.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnSetTemplate_ItemClick);
            // 
            // barBtnSetVoice
            // 
            this.barBtnSetVoice.Caption = "系统声音";
            this.barBtnSetVoice.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.barBtnSetVoice.Glyph = ((System.Drawing.Image)(resources.GetObject("barBtnSetVoice.Glyph")));
            this.barBtnSetVoice.Id = 8;
            this.barBtnSetVoice.LargeGlyph = global::NoiseAnalysisSystem.Properties.Resources.voice3;
            this.barBtnSetVoice.Name = "barBtnSetVoice";
            this.barBtnSetVoice.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnSetVoice_ItemClick);
            // 
            // barBtnSetParam
            // 
            this.barBtnSetParam.Caption = "计算参数";
            this.barBtnSetParam.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.barBtnSetParam.Glyph = ((System.Drawing.Image)(resources.GetObject("barBtnSetParam.Glyph")));
            this.barBtnSetParam.Id = 9;
            this.barBtnSetParam.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barBtnSetParam.LargeGlyph")));
            this.barBtnSetParam.Name = "barBtnSetParam";
            this.barBtnSetParam.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnSetParam_ItemClick);
            // 
            // barBtnSetClose
            // 
            this.barBtnSetClose.Caption = "退出";
            this.barBtnSetClose.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.barBtnSetClose.Glyph = ((System.Drawing.Image)(resources.GetObject("barBtnSetClose.Glyph")));
            this.barBtnSetClose.Id = 10;
            this.barBtnSetClose.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barBtnSetClose.LargeGlyph")));
            this.barBtnSetClose.Name = "barBtnSetClose";
            this.barBtnSetClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnSetClose_ItemClick);
            // 
            // barBtnSerialOpen
            // 
            this.barBtnSerialOpen.Caption = "打开";
            this.barBtnSerialOpen.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.barBtnSerialOpen.Glyph = ((System.Drawing.Image)(resources.GetObject("barBtnSerialOpen.Glyph")));
            this.barBtnSerialOpen.Id = 11;
            this.barBtnSerialOpen.LargeGlyph = global::NoiseAnalysisSystem.Properties.Resources.SerialPort;
            this.barBtnSerialOpen.Name = "barBtnSerialOpen";
            this.barBtnSerialOpen.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnSerialOpen_ItemClick);
            // 
            // barBtnSerialClose
            // 
            this.barBtnSerialClose.Caption = "关闭";
            this.barBtnSerialClose.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.barBtnSerialClose.Enabled = false;
            this.barBtnSerialClose.Glyph = ((System.Drawing.Image)(resources.GetObject("barBtnSerialClose.Glyph")));
            this.barBtnSerialClose.Id = 12;
            this.barBtnSerialClose.LargeGlyph = global::NoiseAnalysisSystem.Properties.Resources.DisableSerialPort;
            this.barBtnSerialClose.Name = "barBtnSerialClose";
            this.barBtnSerialClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnSerialClose_ItemClick);
            // 
            // barBtnSetAbout
            // 
            this.barBtnSetAbout.Caption = "关于";
            this.barBtnSetAbout.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.barBtnSetAbout.Glyph = ((System.Drawing.Image)(resources.GetObject("barBtnSetAbout.Glyph")));
            this.barBtnSetAbout.Id = 13;
            this.barBtnSetAbout.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barBtnSetAbout.LargeGlyph")));
            this.barBtnSetAbout.Name = "barBtnSetAbout";
            this.barBtnSetAbout.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnSetAbout_ItemClick);
            // 
            // barBtnCompare
            // 
            this.barBtnCompare.Caption = "数据比较";
            this.barBtnCompare.Glyph = ((System.Drawing.Image)(resources.GetObject("barBtnCompare.Glyph")));
            this.barBtnCompare.Id = 26;
            this.barBtnCompare.LargeGlyph = global::NoiseAnalysisSystem.Properties.Resources.DataCompare1;
            this.barBtnCompare.Name = "barBtnCompare";
            this.barBtnCompare.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnCompare_ItemClick);
            // 
            // ribbonPageSys
            // 
            this.ribbonPageSys.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup4,
            this.ribbonPageGroup1,
            this.ribbonPageGroup2,
            this.ribbonPageGroup3,
            this.ribbonPageGroup5});
            this.ribbonPageSys.Name = "ribbonPageSys";
            this.ribbonPageSys.Text = "系统";
            // 
            // ribbonPageGroup4
            // 
            this.ribbonPageGroup4.ItemLinks.Add(this.barBtnSerialOpen);
            this.ribbonPageGroup4.ItemLinks.Add(this.barBtnSerialClose);
            this.ribbonPageGroup4.Name = "ribbonPageGroup4";
            this.ribbonPageGroup4.ShowCaptionButton = false;
            this.ribbonPageGroup4.Text = "串口";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barBtnFFT);
            this.ribbonPageGroup1.ItemLinks.Add(this.barBtnCompare);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.ShowCaptionButton = false;
            this.ribbonPageGroup1.Text = "工具";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.barBtnSetSerial);
            this.ribbonPageGroup2.ItemLinks.Add(this.barBtnSetTemplate);
            this.ribbonPageGroup2.ItemLinks.Add(this.barBtnSetVoice);
            this.ribbonPageGroup2.ItemLinks.Add(this.barBtnSetParam);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.ShowCaptionButton = false;
            this.ribbonPageGroup2.Text = "设置";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.barBtnSetClose);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.ShowCaptionButton = false;
            this.ribbonPageGroup3.Text = "退出";
            // 
            // ribbonPageGroup5
            // 
            this.ribbonPageGroup5.Glyph = ((System.Drawing.Image)(resources.GetObject("ribbonPageGroup5.Glyph")));
            this.ribbonPageGroup5.ItemLinks.Add(this.barBtnSetAbout);
            this.ribbonPageGroup5.Name = "ribbonPageGroup5";
            this.ribbonPageGroup5.ShowCaptionButton = false;
            this.ribbonPageGroup5.Text = "帮助";
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.ItemLinks.Add(this.barStaticItemWait);
            this.ribbonStatusBar1.ItemLinks.Add(this.barStaticItem2);
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 649);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.ribbonControl1;
            this.ribbonStatusBar1.ShowSizeGrip = false;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(999, 23);
            // 
            // ControlribbonPageGroup
            // 
            this.ControlribbonPageGroup.Name = "ControlribbonPageGroup";
            this.ControlribbonPageGroup.ShowCaptionButton = false;
            this.ControlribbonPageGroup.Text = "串口";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.navBarControl1);
            this.panelControl1.Location = new System.Drawing.Point(0, 147);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(189, 494);
            this.panelControl1.TabIndex = 4;
            // 
            // navBarControl1
            // 
            this.navBarControl1.ActiveGroup = this.navBarGroup1;
            this.navBarControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navBarControl1.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.navBarGroup1});
            this.navBarControl1.Items.AddRange(new DevExpress.XtraNavBar.NavBarItem[] {
            this.navBarItem1,
            this.navBarSeparatorItem1,
            this.navBarItem2,
            this.navBarSeparatorItem2,
            this.navBarItem3,
            this.navBarSeparatorItem3});
            this.navBarControl1.Location = new System.Drawing.Point(2, 2);
            this.navBarControl1.Name = "navBarControl1";
            this.navBarControl1.OptionsNavPane.ExpandedWidth = 185;
            this.navBarControl1.Size = new System.Drawing.Size(185, 490);
            this.navBarControl1.StoreDefaultPaintStyleName = true;
            this.navBarControl1.TabIndex = 0;
            this.navBarControl1.Text = "navBarControl1";
            // 
            // navBarGroup1
            // 
            this.navBarGroup1.Caption = "功能菜单";
            this.navBarGroup1.Expanded = true;
            this.navBarGroup1.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.LargeIconsText;
            this.navBarGroup1.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem1),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarSeparatorItem1),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem2),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarSeparatorItem2),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem3),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarSeparatorItem3)});
            this.navBarGroup1.Name = "navBarGroup1";
            // 
            // navBarItem1
            // 
            this.navBarItem1.Caption = "噪声数据管理";
            this.navBarItem1.LargeImage = ((System.Drawing.Image)(resources.GetObject("navBarItem1.LargeImage")));
            this.navBarItem1.Name = "navBarItem1";
            this.navBarItem1.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navBarItem1_LinkClicked);
            // 
            // navBarSeparatorItem1
            // 
            this.navBarSeparatorItem1.CanDrag = false;
            this.navBarSeparatorItem1.Enabled = false;
            this.navBarSeparatorItem1.Hint = null;
            this.navBarSeparatorItem1.LargeImageIndex = 0;
            this.navBarSeparatorItem1.LargeImageSize = new System.Drawing.Size(0, 0);
            this.navBarSeparatorItem1.Name = "navBarSeparatorItem1";
            this.navBarSeparatorItem1.SmallImageIndex = 0;
            this.navBarSeparatorItem1.SmallImageSize = new System.Drawing.Size(0, 0);
            // 
            // navBarItem2
            // 
            this.navBarItem2.Caption = "噪声记录仪管理";
            this.navBarItem2.LargeImage = ((System.Drawing.Image)(resources.GetObject("navBarItem2.LargeImage")));
            this.navBarItem2.Name = "navBarItem2";
            this.navBarItem2.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navBarItem2_LinkClicked);
            // 
            // navBarSeparatorItem2
            // 
            this.navBarSeparatorItem2.CanDrag = false;
            this.navBarSeparatorItem2.Enabled = false;
            this.navBarSeparatorItem2.Hint = null;
            this.navBarSeparatorItem2.LargeImageIndex = 0;
            this.navBarSeparatorItem2.LargeImageSize = new System.Drawing.Size(0, 0);
            this.navBarSeparatorItem2.Name = "navBarSeparatorItem2";
            this.navBarSeparatorItem2.SmallImageIndex = 0;
            this.navBarSeparatorItem2.SmallImageSize = new System.Drawing.Size(0, 0);
            // 
            // navBarItem3
            // 
            this.navBarItem3.Caption = "记录仪分组管理";
            this.navBarItem3.LargeImage = ((System.Drawing.Image)(resources.GetObject("navBarItem3.LargeImage")));
            this.navBarItem3.Name = "navBarItem3";
            this.navBarItem3.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navBarItem3_LinkClicked);
            // 
            // navBarSeparatorItem3
            // 
            this.navBarSeparatorItem3.CanDrag = false;
            this.navBarSeparatorItem3.Enabled = false;
            this.navBarSeparatorItem3.Hint = null;
            this.navBarSeparatorItem3.LargeImageIndex = 0;
            this.navBarSeparatorItem3.LargeImageSize = new System.Drawing.Size(0, 0);
            this.navBarSeparatorItem3.Name = "navBarSeparatorItem3";
            this.navBarSeparatorItem3.SmallImageIndex = 0;
            this.navBarSeparatorItem3.SmallImageSize = new System.Drawing.Size(0, 0);
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Office 2007 Silver";
            this.defaultLookAndFeel1.LookAndFeel.TouchUIMode = DevExpress.LookAndFeel.TouchUIMode.False;
            // 
            // FrmSystem
            // 
            this.AllowFormGlass = DevExpress.Utils.DefaultBoolean.True;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(999, 672);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControlMain);
            this.Controls.Add(this.ribbonStatusBar1);
            this.Controls.Add(this.ribbonControl1);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmSystem";
            this.Ribbon = this.ribbonControl1;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.StatusBar = this.ribbonStatusBar1;
            this.Text = "自来水管道噪声分析系统";
            this.Load += new System.EventHandler(this.FrmSystem_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControlMain;
        private DevExpress.XtraBars.BarStaticItem barStaticItem2;
        public DevExpress.XtraBars.BarStaticItem barStaticItemWait;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPageSys;
        private DevExpress.XtraBars.BarButtonItem barBtnFFT;
        private DevExpress.XtraBars.BarButtonItem barBtnSetSerial;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.BarButtonItem barBtnSetTemplate;
        private DevExpress.XtraBars.BarButtonItem barBtnSetVoice;
        private DevExpress.XtraBars.BarButtonItem barBtnSetParam;
        private DevExpress.XtraBars.BarButtonItem barBtnSetClose;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ControlribbonPageGroup;
        private DevExpress.XtraBars.BarButtonItem barBtnSerialOpen;
        private DevExpress.XtraBars.BarButtonItem barBtnSerialClose;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;
        private DevExpress.XtraBars.BarButtonItem barBtnSetAbout;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup5;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraNavBar.NavBarControl navBarControl1;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup1;
        private DevExpress.XtraNavBar.NavBarItem navBarItem1;
        private DevExpress.XtraNavBar.NavBarSeparatorItem navBarSeparatorItem1;
        private DevExpress.XtraNavBar.NavBarItem navBarItem2;
        private DevExpress.XtraNavBar.NavBarSeparatorItem navBarSeparatorItem2;
        private DevExpress.XtraNavBar.NavBarItem navBarItem3;
        private DevExpress.XtraNavBar.NavBarSeparatorItem navBarSeparatorItem3;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraBars.BarButtonItem barBtnCompare;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenmanager;

    }
}