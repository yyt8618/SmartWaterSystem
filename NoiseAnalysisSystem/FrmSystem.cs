using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;

namespace NoiseAnalysisSystem
{
    public partial class FrmSystem : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private UcDataMgr dataMgr;
        private UcRecMgr recMgr;
        private UcGroupMgr gpMgr;

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
        }


        private void FrmSystem_Load(object sender, EventArgs e)
        {
            ClearLogAndDb();
        }

        // 打开串口
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (!GlobalValue.portUtil.IsOpen)
                {
                    GlobalValue.portUtil.Open();
                    barStaticItemWait.Caption = "串口已打开";
                    barButtonItem9.Enabled = false;
                    barButtonItem10.Enabled = true;
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
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 关闭串口
        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (GlobalValue.portUtil.IsOpen)
                {
                    GlobalValue.portUtil.Close();
                    barStaticItemWait.Caption = "串口已关闭";
                    barButtonItem9.Enabled = true;
                    barButtonItem10.Enabled = false;
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
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 傅里叶分析
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmTest ft= new FrmTest();
            ft.ShowDialog();
        }

        // 数据管理
        private void navBarItem1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControlMain.Controls.Clear();
            panelControlMain.Controls.Add(dataMgr);
            dataMgr.BindGroup();
        }

        // 记录仪管理
        private void navBarItem2_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControlMain.Controls.Clear();
            panelControlMain.Controls.Add(recMgr);
            recMgr.BindRecord();
        }

        // 分组管理
        private void navBarItem3_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControlMain.Controls.Clear();
            panelControlMain.Controls.Add(gpMgr);
            gpMgr.BindTree();
            gpMgr.BindListBox();
        }

        // 退出
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult dr = DevExpress.XtraEditors.XtraMessageBox.Show("确定退出系统？", "请确定", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                if (GlobalValue.portUtil != null && GlobalValue.portUtil.IsOpen)
                    GlobalValue.portUtil.Close();

                Application.Exit();
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmTemplateSet fts = new FrmTemplateSet();
            fts.ShowDialog();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmSerialComSet fsc = new FrmSerialComSet();
            fsc.ShowDialog();
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmSysVoiceSet fsv = new FrmSysVoiceSet();
            fsv.ShowDialog();
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmCalcParamSet fcp = new FrmCalcParamSet();
            fcp.ShowDialog();
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AboutBox about = new AboutBox();
            about.ShowDialog();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
    }
}