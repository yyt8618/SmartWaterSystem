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
        private UcDataMgr dataMgr;
        private UcRecMgr recMgr;
        private UcGroupMgr gpMgr;
        private UcPreTerMgr pretelMgr;

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
            dataMgr = new UcDataMgr(this);
            recMgr = new UcRecMgr(this);
            gpMgr = new UcGroupMgr(this);
            pretelMgr = new UcPreTerMgr(this);
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

        private void navBarPreTelMgr_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControlMain.Controls.Clear();
            panelControlMain.Controls.Add(pretelMgr);
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

    }
}