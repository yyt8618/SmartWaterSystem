using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using System.Reflection;
using DevExpress.XtraBars.Helpers;
using System.Collections.Generic;
using DevExpress.LookAndFeel;
using DevExpress.XtraSplashScreen;
using System.Threading;
using Microsoft.Data.ConnectionUI;
using Common;
using Entity;

namespace SmartWaterSystem
{
    public partial class FrmSystem : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        List<Type> lstType = new List<Type>();
        DevExpress.XtraEditors.XtraUserControl currentView = null;
        NLog.Logger logger = NLog.LogManager.GetLogger("FrmSystem");

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmSystem()
        {
            InitializeComponent();
        }

        private void FrmSystem_Load(object sender, EventArgs e)
        {
            this.Text = "自来水管道分析系统" + "(" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + ")";
            // 读取数据库 初始化界面
            try
            {
                SkinHelper.InitSkinGallery(this.ribbonGalleryBarItem1);

                SplashScreenManager.ShowForm(typeof(WelcomSplash));
                if (SplashScreenManager.Default.IsSplashFormVisible)
                {
                    SplashScreenManager.Default.SendCommand(null,"正在加载皮肤...");
                }
                //Set Skin
                string skin = Settings.Instance.GetString(SettingKeys.Skin);
                if (string.IsNullOrEmpty(skin))
                    skin = "SevenClassic";
                UserLookAndFeel.Default.SetSkinStyle(skin);

                //HideNavigateBar
                foreach (DevExpress.XtraNavBar.NavBarGroup group in this.navBarControl1.Groups)
                {
                    foreach (DevExpress.XtraNavBar.NavBarItemLink itemlink in group.ItemLinks)
                    {
                        itemlink.Item.Visible = false;
                    }
                    group.Visible = false;
                }

                if (SplashScreenManager.Default.IsSplashFormVisible)
                {
                    SplashScreenManager.Default.SendCommand(null, "正在加载控件...");
                }
                #region 加载控件
                var files = Directory.GetFiles(Application.StartupPath, "I*");
                for (int i = 0; i < files.Length; i++)
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(files[i], @".dll$"))
                        LoadAddin(files[i]);
                }

                if (lstType != null && lstType.Count > 0)
                {
                    InitNavigate();
                }
                else
                {
                }
                #endregion

                #region 数据库操作
                SQLiteDbManager dbMgr = new SQLiteDbManager();
                #region 创建数据库
                //如果数据库文件不存在创建
                if (!(dbMgr.Exists))
                {
                    if (SplashScreenManager.Default.IsSplashFormVisible)
                    {
                        SplashScreenManager.Default.SendCommand(null, "正在创建数据库...");
                    }
                    if (!dbMgr.ResetDatabase())
                    {
                        //error.ErrorCode = -1;
                        logger.Error("ResetDatabase","创建数据库失败，请联系系统管理员");
                    }
                }
                #endregion
                #region 升级数据库
                DBVersion versionBLL = new DBVersion();
                string dbVersion = versionBLL.GetVersion(VersionType.DataBase.ToString());
                if (dbVersion != dbMgr.LastestDBVersion)
                {
                    if (SplashScreenManager.Default.IsSplashFormVisible)
                    {
                        SplashScreenManager.Default.SendCommand(null, "正在更新数据库...");
                    }
                    if (!dbMgr.UpgradeDB())
                    {
                        //error.ErrorCode = 0;
                        //error.ErrorMessage = "      自动升级数据库失败，请联系系统管理员";
                    }
                    else
                    {
                        if (SplashScreenManager.Default.IsSplashFormVisible)
                        {
                            SplashScreenManager.Default.SendCommand(null, "正在更新数据库版本...");
                        }
                        if (!versionBLL.UpdateVersion(VersionType.DataBase.ToString(), dbMgr.LastestDBVersion))
                        {
                            //error.ErrorCode = 0;
                            //error.ErrorMessage = "      自动升级数据库失败，请联系系统管理员";
                        }
                    }
                }
                #endregion
                #endregion

                if (SplashScreenManager.Default.IsSplashFormVisible)
                {
                    SplashScreenManager.Default.SendCommand(null, "正在加载数据...");
                }
                GlobalValue.groupList = NoiseDataBaseHelper.GetGroups();
                GlobalValue.recorderList = NoiseDataBaseHelper.GetRecorders();
                GlobalValue.controllerList = NoiseDataBaseHelper.GetController();

                if (SplashScreenManager.Default.IsSplashFormVisible)
                {
                    SplashScreenManager.Default.SendCommand(null, "正在初始化参数...");
                }

                SQLHelper.ConnectionString = Settings.Instance.GetString(SettingKeys.DBString);
                if (!string.IsNullOrEmpty(SQLHelper.ConnectionString))
                {
                    bool sqlconnect = SQLHelper.TryConn(SQLHelper.ConnectionString);
                    if (!sqlconnect)
                    {
                        if (DialogResult.No == XtraMessageBox.Show("连接SQL数据库失败，是否继续？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Error))
                        {
                            logger.Info("TryConn func:SQL数据库连接失败，主动退出");
                            Application.Exit();
                        }
                    }
                    else
                    {
                        GlobalValue.SQLSyncMgr.SQLSyncEvent += new SQLSyncEventHandler(SQLSyncMgr_SQLSyncEvent);
                        GlobalValue.SQLSyncMgr.Start();


                        SQLSynctimer.Interval =60 * 1000;
                        SQLSynctimer.Tick += new EventHandler(SQLSynctimer_Tick);
                        SQLSynctimer.Enabled = true;
                    }
                }

                GlobalValue.SerialPortMgr.SerialPortEvent += new SerialPortHandle(SerialPortMgr_SerialPortEvent);
                GlobalValue.SerialPortMgr.Start();

                SplashScreenManager.CloseForm();

                ClearLogAndDb();

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("初始化异常", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.ErrorException("FrmSystem_Load", ex);
                Application.Exit();
            }
        }

        void SerialPortMgr_SerialPortEvent(object sender, SerialPortEventArgs e)
        {
            
        }

        void SQLSynctimer_Tick(object sender, EventArgs e)
        {
            GlobalValue.SQLSyncMgr.Send(SqlSyncType.SyncTerInfo);
            GlobalValue.SQLSyncMgr.Send(SqlSyncType.SyncUpdate_UniversalTerWayType);
        }

        void SQLSyncMgr_SQLSyncEvent(object sender, SQLSyncEventArgs e)
        {
            
        }

        private void InitNavigate()
        {
            foreach (Type t in lstType)
            {
                if(t.Name=="INoiseDataMgr")  //噪声数据管理
                {
                    NBG_Noise.Visible = true; 
                    navBarNoiseDataManager.Visible=true;
                }
                else if (t.Name == "INoiseRecMgr")        //噪声记录仪管理
                {
                    NBG_Noise.Visible = true;
                    navBarNoiseRecorderManager.Visible=true;
                }
                else if (t.Name == "INoiseGroupMgr")  //噪声分组管理
                {
                    NBG_Noise.Visible = true;
                    navBarNoiseGroupManager.Visible=true;
                }
                else if (t.Name == "INoiseMap")    //噪声地图
                {
                    NBG_Noise.Visible = true;
                    navBarNoiseMap.Visible=true;
                }
                else if (t.Name == "INoiseParmSetting") //噪声记录仪参数设置
                {
                    NBG_Noise.Visible = true;
                    navBarNoiseParmSet.Visible=true;
                }
                else if (t.Name=="INoiseFFT")    //噪声记录仪傅里叶分析
                {
                    NBG_Noise.Visible = true;
                    navBarNoiseFFT.Visible=true;
                }
                else if (t.Name == "INoiseDataCompare")  //噪声记录仪数据比较
                {
                    NBG_Noise.Visible = true;
                    navBarNoiseCompare.Visible=true;
                }
                else if (t.Name=="INoiseEnergyAnalysis") //噪声记录仪能量分析
                {
                    NBG_Noise.Visible = true;
                    navBarNoiseEnergy.Visible=true;
                }
                else if (t.Name=="IPreTerParm")  //压力终端参数配置和读取
                {
                    NBG_PreT.Visible = true;
                    navBarPreTerParm.Visible = true;
                }
                else if (t.Name=="IPreTerMgr")  //压力终端配置和管理
                {
                    NBG_PreT.Visible = true;
                    navBarPreTerMgr.Visible = true;
                }
                else if (t.Name=="IPreTerMonitor")  //压力终端实时列表监控、趋势图
                {
                    NBG_PreT.Visible = true;
                    navBarPreTerMonitor.Visible = true;
                }
                else if (t.Name=="IPreTerReportHistory")  //压力终端报表、历史数据查询
                {
                    NBG_PreT.Visible = true;
                    navBarPreTerReport.Visible = true;
                }
                else if (t.Name=="IPreTerAlarm")  //压力终端报警统计分析
                {
                    NBG_PreT.Visible = true;
                    navBarPreTerAlarm.Visible = true;
                }
                else if (t.Name=="IPreTerStoppage")  //压力终端故障统计分析
                {
                    NBG_PreT.Visible = true;
                    navBarPreTerStoppage.Visible = true;
                }
                else if (t.Name == "IUniversalTerParm")  //通用终端参数设置和读取
                {
                    NBG_GSMT.Visible = true;
                    navBarGSMParm.Visible = true;
                }
                else if (t.Name == "IUniversalTerMgr")   //通用终端管理
                {
                    NBG_GSMT.Visible = true;
                    navBarGSMMgr.Visible = true;
                }
            }
        }

        private void VisiableNavigateBar(bool isVisiable)
        {
            //Noise Group
            NBG_Noise.Visible = isVisiable;
            //噪声数据管理
            navBarNoiseDataManager.Visible = isVisiable;
            //噪声记录仪管理
            navBarNoiseRecorderManager.Visible = isVisiable;
            //噪声分组管理
            navBarNoiseGroupManager.Visible = isVisiable;
            //噪声地图
            navBarNoiseMap.Visible = isVisiable;
            //噪声记录仪参数设置
            navBarNoiseParmSet.Visible = isVisiable;
            //噪声记录仪傅里叶分析
            navBarNoiseFFT.Visible = isVisiable;
            //噪声记录仪数据比较
            navBarNoiseCompare.Visible = isVisiable;
            //噪声记录仪能量分析
            navBarNoiseEnergy.Visible = isVisiable;

            //压力终端 Group
            NBG_PreT.Visible = isVisiable;
            //压力终端参数配置和读取
            navBarPreTerParm.Visible = isVisiable;
            //压力终端配置和管理
            navBarPreTerMgr.Visible = isVisiable;
            //压力终端实时列表监控、趋势图
            navBarPreTerMonitor.Visible = isVisiable;
            //压力终端报表、历史数据查询
            navBarPreTerReport.Visible = isVisiable;
            //压力终端报警统计分析
            navBarPreTerAlarm.Visible = isVisiable;
            //压力终端故障统计分析
            navBarPreTerStoppage.Visible = isVisiable;

            //通用终端
            NBG_GSMT.Visible = isVisiable;
            //通用终端参数配置和读取
            navBarGSMParm.Visible = isVisiable;
            //通用终端管理
            navBarGSMMgr.Visible = isVisiable;
            //通用终端列表监控、趋势图
            navBarGSMMonitor.Visible = isVisiable;
            //通用终端报表、历史数据查询
            navBarGSMReport.Visible = isVisiable;
            //通用终端故障统计分析
            navBarGSMStoppage.Visible = isVisiable;


        }

        // 打开串口
        private void barBtnSerialOpen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (!GlobalValue.portUtil.IsOpen)
                {
                    GlobalValue.portUtil.Open();
                    barStaticItemWait.Caption = "串口已打开";
                    barBtnSerialOpen.Enabled = false;
                    barBtnSerialClose.Enabled = true;
                    if (GlobalValue.recorderList.Count > 0)
                    {
                        NoiseRecMgr noiserecMgr=(NoiseRecMgr)GetView(typeof(NoiseRecMgr));
                        if (noiserecMgr != null)
                            noiserecMgr.SerialPortEvent(GlobalValue.portUtil.IsOpen);

                        NoiseDataMgr noisedataMgr = (NoiseDataMgr)GetView(typeof(NoiseDataMgr));
                        if (noisedataMgr != null)
                            noisedataMgr.SerialPortEvent(GlobalValue.portUtil.IsOpen);

                        NoiseGroupMgr noisegrpMgr = (NoiseGroupMgr)GetView(typeof(NoiseGroupMgr));
                        if (noisegrpMgr != null)
                            noisegrpMgr.SerialPortEvent(GlobalValue.portUtil.IsOpen);

                        UniversalTerParm universalterMgr = (UniversalTerParm)GetView(typeof(UniversalTerParm));
                        if (universalterMgr != null)
                            universalterMgr.SerialPortEvent(GlobalValue.portUtil.IsOpen);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 关闭串口
        private void barBtnSerialClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (GlobalValue.portUtil.IsOpen)
                {
                    GlobalValue.portUtil.Close();
                    barStaticItemWait.Caption = "串口已关闭";
                    barBtnSerialOpen.Enabled = true;
                    barBtnSerialClose.Enabled = false;
                    if (GlobalValue.recorderList.Count > 0)
                    {
                        NoiseRecMgr noiserecMgr = (NoiseRecMgr)GetView(typeof(NoiseRecMgr));
                        if (noiserecMgr != null)
                            noiserecMgr.SerialPortEvent(GlobalValue.portUtil.IsOpen);

                        NoiseDataMgr noisedataMgr = (NoiseDataMgr)GetView(typeof(NoiseDataMgr));
                        if (noisedataMgr != null)
                            noisedataMgr.SerialPortEvent(GlobalValue.portUtil.IsOpen);

                        NoiseGroupMgr noisegrpMgr = (NoiseGroupMgr)GetView(typeof(NoiseGroupMgr));
                        if (noisegrpMgr != null)
                            noisegrpMgr.SerialPortEvent(GlobalValue.portUtil.IsOpen);

                        UniversalTerParm universalterMgr = (UniversalTerParm)GetView(typeof(UniversalTerParm));
                        if (universalterMgr != null)
                            universalterMgr.SerialPortEvent(GlobalValue.portUtil.IsOpen);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region NavigatorBar Click
        // 数据管理
        private void navBarNoiseDataManager_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            NoiseDataMgr DataMgrview = (NoiseDataMgr)LoadView(typeof(NoiseDataMgr));
            DataMgrview.BindGroup();
        }

        // 记录仪管理
        private void navBarNoiseRecorderManager_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            NoiseRecMgr RecMgrview = (NoiseRecMgr)LoadView(typeof(NoiseRecMgr));
            RecMgrview.BindRecord();
        }

        // 分组管理
        private void navBarNoiseGroupManager_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            NoiseGroupMgr groupMgrView = (NoiseGroupMgr)this.LoadView(typeof(NoiseGroupMgr));
            groupMgrView.BindListBox();
            groupMgrView.BindTree();
        }

        //噪声地图
        private void navBarNoiseMap_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            LoadView(typeof(NoiseMap));
        }

        // 压力终端参数
        private void navBarPreTerParm_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            LoadView(typeof(PreTerParm));
        }

        //压力终端管理
        private void navBarPreTerMgr_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            LoadView(typeof(PreTerMgr));
        }

        private void navBarPreTerMonitor_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            LoadView(typeof(PreTerMonitor));
        }

        private void navBarPreTerReport_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            LoadView(typeof(PreTerReportHistory));
        }

        private void navBarPreTerAlarm_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            LoadView(typeof(PreTerAlarm));
        }

        private void navBarPreTerStoppage_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            LoadView(typeof(PreTerStoppage));
        }
        private void navBarNoiseParmSet_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            LoadView(typeof(NoiseParmSetting));
        }

        private void navBarNoiseFFT_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            LoadView(typeof(NoiseFFT));
        }

        private void navBarNoiseCompare_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (GlobalValue.recorderList.Count > 0)
            {
                LoadView(typeof(NoiseDataCompare));
            }
            else
            {
                XtraMessageBox.Show("当前不存在任何记录仪！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void navBarNoiseEnergy_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            LoadView(typeof(NoiseEnergyAnalysis));
        }

        private void navBarGSMParm_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            LoadView(typeof(UniversalTerParm));
        }

        private void navBarGSMMgr_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            LoadView(typeof(UniversalTerMgr));
        }

        private void navBarGSMMonitor_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            LoadView(typeof(UniversalTerMonitor));
        }
        #endregion

        // 退出
        private void barBtnSetClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult dr = DevExpress.XtraEditors.XtraMessageBox.Show("确定退出系统？", "请确定", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                if (GlobalValue.portUtil != null && GlobalValue.portUtil.IsOpen)
                    GlobalValue.portUtil.Close();

                Application.Exit();
            }
        }

        private void barBtnSetSerial_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmSerialComSet fsc = new FrmSerialComSet();
            fsc.ShowDialog();
        }

        private void barBtnSetVoice_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmSysVoiceSet fsv = new FrmSysVoiceSet();
            fsv.ShowDialog();
        }

        private void barBtnSetAbout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AboutBox about = new AboutBox();
            about.ShowDialog();
        }

        private void barBtnSetDBConnect_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                DataConnectionDialog dialog = new DataConnectionDialog();
                //添加数据源列表，可以向窗口中添加自己程序所需要的数据源类型
                dialog.DataSources.Add(DataSource.SqlDataSource);

                dialog.SelectedDataSource = DataSource.SqlDataSource;
                dialog.SelectedDataProvider = DataProvider.SqlDataProvider;


                //只能够通过DataConnectionDialog类的静态方法Show出对话框
                //不同使用dialog.Show()或dialog.ShowDialog()来呈现对话框
                if (DataConnectionDialog.Show(dialog, this) == DialogResult.OK)
                {
                    string dbconnect = dialog.ConnectionString;
                    Settings.Instance.SetValue(SettingKeys.DBString, dbconnect);
                    XtraMessageBox.Show("设置成功,请重启程序生效!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("barBtnSetDBConnect_ItemClick", ex);
                XtraMessageBox.Show("打开设置窗体失败,请联系管理员!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ClearLogAndDb()
        {
            try
            {
                // 清理7天前的日志
                string[] fileNames = Directory.GetFiles(Path.GetDirectoryName(Application.ExecutablePath) + "\\log\\");
                DateTime deleteDate = DateTime.Today.AddDays(-7);
                foreach (string fileName in fileNames)
                {
                    if (DateTime.Parse(Path.GetFileName(fileName).Substring(0, 10)) < deleteDate)
                        File.Delete(fileName);
                }
            }
            catch (System.Exception ex)
            {
                logger.ErrorException("ClearLogAndDb()", ex);
            }
        }

        /// <summary>
        /// 显示等待窗口
        /// </summary>
        //public void ShowWaitForm(string title, string prompt)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(title))
        //            title = "请稍候...";
        //        this.BeginInvoke(new Action(() =>
        //            {
        //                if (!splashScreenmanager.IsSplashFormVisible)
        //                {
        //                    splashScreenmanager.ShowWaitForm();
        //                    splashScreenmanager.SetWaitFormCaption(title);
        //                    splashScreenmanager.SetWaitFormDescription(prompt);
        //                }
        //                else
        //                {
        //                    splashScreenmanager.SetWaitFormCaption(title);
        //                    splashScreenmanager.SetWaitFormDescription(prompt);
        //                }
        //            }));
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.ErrorException("ShowWaitForm", ex);
        //    }
        //}

        //public void HideWaitForm()
        //{
        //    try
        //    {
        //        this.BeginInvoke(new Action(() =>
        //        {
        //            try
        //            {
        //                if (splashScreenmanager.IsSplashFormVisible)
        //                    splashScreenmanager.CloseWaitForm();
        //            }
        //            catch { }
        //        }));
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.ErrorException("HideWaitForm", ex);
        //    }
        //}

        private delegate void showwaitformHandle(string title,string prompt);
        private WaitForm1 progressDlg = new WaitForm1();
        public void ShowWaitForm(string title, string prompt)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new showwaitformHandle(ShowWaitForm), title, prompt);
            }
            else
            {
                progressDlg.ShowProgress(title, prompt);
            }
        }

        public void HideWaitForm()
        {
            progressDlg.HideProgress();
        }

        public void DisableRibbonBar()
        {
            this.ribbonControl1.Enabled = false;
        }

        public void EnableRibbonBar()
        {
            this.ribbonControl1.Enabled = true;
        }

        public void DisableNavigateBar()
        {
            this.navBarControl1.Enabled = false;
        }

        public void EnableNavigateBar()
        {
            this.navBarControl1.Enabled = true;
        }

        public void ShowDialog(string text,string caption,MessageBoxButtons buttons,MessageBoxIcon icon)
        {
            EnableRibbonBar();
            EnableNavigateBar();
            HideWaitForm();
            Application.DoEvents();
            XtraMessageBox.Show(text, caption, buttons, icon);
        }

        private void FrmSystem_KeyDown(object sender, KeyEventArgs e)
        {
        }

        #region 加载插件
        private void LoadAddin(string path)
        {
            //通过反射，获取DLL中的类型
            Assembly assembly = Assembly.LoadFrom(path);
            Type[] types = assembly.GetTypes();
            lstType.AddRange(types);
            //foreach (var t in types)
            //{
            //    var obj = assembly.CreateInstance(t.ToString());
            //    if (obj is IViewContent)
            //    {
            //        AddView((AbstractViewContent)obj);
            //    }

            //    if (t.Name == "INoiseGroupMgr")
            //    {
            //        try
            //        {
            //            IMyFunction pluginclass = Activator.CreateInstance(t) as IMyFunction;
            //            pluginclass.doSomething();
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show(ex.ToString());
            //        }
            //    }
            //}
        }

        private bool isExist(Type type)
        {
            if(lstType!=null && lstType.Count>0)
            {
                foreach(Type t in lstType)
                {
                    if(t.Equals(type))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 加载界面
        /// </summary>
        public BaseView LoadView(Type targetType)
        {
            try
            {
                if (currentView != null)
                    currentView.Visible = false;

                BaseView newView = null;
                foreach (Control control in this.panelControlMain.Controls)
                {
                    if (control.GetType() == targetType)
                    {
                        newView = (BaseView)control;
                        break;
                    }
                }
                if (newView == null)
                {
                    newView = (BaseView)Activator.CreateInstance(targetType);
                    panelControlMain.Controls.Add(newView);
                    newView.MDIView = this;
                }
                currentView = newView;
                newView.Visible = true;
                newView.Focus();
                newView.OnLoad();

                //this.lblTitle.Text = newView.Title;
                return newView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        //获取操作页面对象
        public BaseView GetView(Type targetType)
        {
            try
            {
                BaseView newView = null;
                foreach (Control control in this.panelControlMain.Controls)
                {
                    if (control.GetType() == targetType)
                    {
                        newView = (BaseView)control;
                        break;
                    }
                }
                return newView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        #endregion

        private void ribbonGalleryBarItem1_GalleryItemClick(object sender, DevExpress.XtraBars.Ribbon.GalleryItemClickEventArgs e)
        {
            string skincaption=ribbonGalleryBarItem1.Gallery.GetCheckedItems()[0].Caption;
            Settings.Instance.SetValue(SettingKeys.Skin, skincaption);
        }

        private void FrmSystem_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                GlobalValue.SQLSyncMgr.Stop();
                GlobalValue.SQLSyncMgr.SQLSyncEvent -= new SQLSyncEventHandler(SQLSyncMgr_SQLSyncEvent);
                Thread.Sleep(100);

                GlobalValue.SerialPortMgr.Stop();
                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortMgr_SerialPortEvent);
                Thread.Sleep(100);
            }
            catch { }
        }
        


    }
}