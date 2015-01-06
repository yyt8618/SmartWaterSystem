using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using System.Threading;
using System.Reflection;
using DevExpress.XtraBars.Helpers;
using System.Collections.Generic;

namespace NoiseAnalysisSystem
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
            // 读取数据库 初始化界面
            try
            {
                GlobalValue.groupList = NoiseDataBaseHelper.GetGroups();
                GlobalValue.recorderList = NoiseDataBaseHelper.GetRecorders();
                GlobalValue.controllerList = NoiseDataBaseHelper.GetController();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("获取数据库数据异常", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            try
            {
                //VisiableNavigateBar(false);

                //HideNavigateBar
                foreach (DevExpress.XtraNavBar.NavBarGroup group in this.navBarControl1.Groups)
                {
                    foreach (DevExpress.XtraNavBar.NavBarItemLink itemlink in group.ItemLinks)
                    {
                        itemlink.Item.Visible = false;
                    }
                    group.Visible = false;
                }

                #region 加载控件
                var files = Directory.GetFiles(Application.StartupPath + "\\plugins", "*.dll");
                for (int i = 0; i < files.Length; i++)
                {
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
            }
            catch (Exception ex)
            {
                logger.ErrorException("加载插件异常", ex);
                XtraMessageBox.Show("加载插件异常", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void FrmSystem_Load(object sender, EventArgs e)
        {
            this.Text = "自来水管道噪声分析系统" + "(" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + ")";
            SkinHelper.InitSkinGallery(this.ribbonGalleryBarItem1);
            ClearLogAndDb();
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
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
            groupMgrView.BindTree();
            groupMgrView.BindListBox();
        }

        private void navBarNoiseMap_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            LoadView(typeof(NoiseMap));
        }

        private void navBarPreTerParm_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            LoadView(typeof(PreTerParm));
        }


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
        public void ShowWaitForm(string title, string prompt)
        {
            if (string.IsNullOrEmpty(title))
                title = "请稍候...";
            this.BeginInvoke(new Action(() =>
                {
                    if (!splashScreenmanager.IsSplashFormVisible)
                    {
                        splashScreenmanager.ShowWaitForm();
                        splashScreenmanager.SetWaitFormCaption(title);
                        splashScreenmanager.SetWaitFormDescription(prompt);
                    }
                }));
        }

        public void HideWaitForm()
        {
            this.BeginInvoke(new Action(() =>
            {
                try
                {
                    if (splashScreenmanager.IsSplashFormVisible)
                        splashScreenmanager.CloseWaitForm();
                }
                catch { }
            }));
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

        private void barBtnNoiseEnergyAny_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadView(typeof(NoiseEnergyAnalysis));
        }

        private void FrmSystem_KeyDown(object sender, KeyEventArgs e)
        {
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

    }
}