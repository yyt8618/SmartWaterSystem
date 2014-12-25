using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using System.Threading;
using System.Reflection;

namespace NoiseAnalysisSystem
{
    public partial class FrmSystem : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private NoiseDataMgr dataMgr;   //噪声记录仪数据读取、分析
        private NoiseRecMgr recMgr;     //噪声记录仪管理
        private NoiseGroupMgr gpMgr;    //噪声记录仪分组管理
        private PreTerParm pretelParm;  //压力终端参数读取设置
        private PreTerMgr pretelMgr;    //压力终端配置、管理
        private PreTerMonitor pretelMonitor;  //压力终端监控
        private PreTerReportHistory pretelReport; //压力终端报表与历史数据查询
        private PreTerAlarm pretelAlarm;    //压力终端报警
        private PreTerStoppage pretelStoppage;  //压力终端故障统计分析

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
            dataMgr = new NoiseDataMgr(this);
            recMgr = new NoiseRecMgr(this);
            gpMgr = new NoiseGroupMgr(this);
            pretelParm = new PreTerParm(this);
            pretelMgr = new PreTerMgr(this);
            pretelMonitor = new PreTerMonitor(this);
            pretelReport = new PreTerReportHistory(this);
            pretelAlarm = new PreTerAlarm(this);
            pretelStoppage = new PreTerStoppage(this);
        }

        private void FrmSystem_Load(object sender, EventArgs e)
        {
            this.Text = "自来水管道噪声分析系统" + "(" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + ")";
            ClearLogAndDb();
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
                        recMgr.btnApplySet.Enabled = true;
                        recMgr.btnReadSet.Enabled = true;
                        recMgr.btnGetRecID.Enabled = true;
                        recMgr.btnGetConID.Enabled = true;
                        recMgr.btnNow.Enabled = true;
                        recMgr.btnStart.Enabled = true;
                        recMgr.btnStop.Enabled = true;
                        recMgr.btnCleanFlash.Enabled = true;
                        dataMgr.simpleButtonRead.Enabled = true;
                        gpMgr.btnSaveGroupSet.Enabled = true;
                        gpMgr.btnReadTemplate.Enabled = true;
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
                        recMgr.btnApplySet.Enabled = false;
                        recMgr.btnReadSet.Enabled = false;
                        recMgr.btnGetRecID.Enabled = false;
                        recMgr.btnGetConID.Enabled = false;
                        recMgr.btnNow.Enabled = false;
                        recMgr.btnStart.Enabled = false;
                        recMgr.btnStop.Enabled = false;
                        recMgr.btnCleanFlash.Enabled = false;
                        dataMgr.simpleButtonRead.Enabled = false;
                        gpMgr.btnSaveGroupSet.Enabled = false;
                        gpMgr.btnReadTemplate.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 傅里叶分析
        private void barBtnFFT_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmTest ft= new FrmTest();
            ft.ShowDialog();
        }

        // 数据管理
        private void navBarNoiseDataManager_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControlMain.Controls.Clear();
            panelControlMain.Controls.Add(dataMgr);
            dataMgr.BindGroup();
        }

        // 记录仪管理
        private void navBarNoiseRecorderManager_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControlMain.Controls.Clear();
            panelControlMain.Controls.Add(recMgr);
            recMgr.BindRecord();
        }

        // 分组管理
        private void navBarNoiseGroupManager_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControlMain.Controls.Clear();
            panelControlMain.Controls.Add(gpMgr);
            gpMgr.BindTree();
            gpMgr.BindListBox();
        }

        private void navBarNoiseMap_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show("功能未实现，请耐心等候! ^_^", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void navBarPreTelParm_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControlMain.Controls.Clear();
            panelControlMain.Controls.Add(pretelParm);
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

        private void barBtnSetTemplate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmTemplateSet fts = new FrmTemplateSet();
            fts.ShowDialog();
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

        private void barBtnSetParam_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmCalcParamSet fcp = new FrmCalcParamSet();
            fcp.ShowDialog();
        }

        private void barBtnSetAbout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AboutBox about = new AboutBox();
            about.ShowDialog();
        }

        private void barBtnCompare_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalValue.recorderList.Count > 0)
            {
                FrmDataCompare fdc = new FrmDataCompare();
                fdc.ShowDialog();
            }
            else
            {
                XtraMessageBox.Show("当前不存在任何记录仪！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        public void ShowWaitForm(string title, string prompt)
        {
            if (string.IsNullOrEmpty(title))
                title = "请稍候...";
            this.BeginInvoke(new Action(() =>
                {
                    splashScreenmanager.ShowWaitForm();
                    splashScreenmanager.SetWaitFormCaption(title);
                    splashScreenmanager.SetWaitFormDescription(prompt);
                }));
        }

        public void HideWaitForm()
        {
            this.BeginInvoke(new Action(() =>
            {
                if (splashScreenmanager.IsSplashFormVisible)
                    splashScreenmanager.CloseWaitForm();
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

        private void navBarPreTelMgr_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControlMain.Controls.Clear();
            panelControlMain.Controls.Add(pretelMgr);
        }

        private void navBarPreTelMonitor_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControlMain.Controls.Clear();
            panelControlMain.Controls.Add(pretelMonitor);
        }

        private void navBarPreTelReport_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControlMain.Controls.Clear();
            panelControlMain.Controls.Add(pretelReport);
        }

        private void navBarPreTelAlarm_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControlMain.Controls.Clear();
            panelControlMain.Controls.Add(pretelAlarm);
        }

        private void navBarPreTelStoppage_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControlMain.Controls.Clear();
            panelControlMain.Controls.Add(pretelStoppage);
        }

    }
}