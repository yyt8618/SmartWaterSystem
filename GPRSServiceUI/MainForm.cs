using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Messaging;
using System.ServiceProcess;
using Microsoft.Data.ConnectionUI;
using System.Text.RegularExpressions;
using System.IO;
using Common;
using Entity;

namespace GPRSServiceUI
{
    public partial class MainForm : Form
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("MainForm");
        private const int MaxLine = 1001;

        Thread t1;
        string QueuePath = ".\\private$\\GcGPRS";

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            MainForm.CheckForIllegalCrossThreadCalls = false;

            InitConfig();

            ServiceController serviceController1 = ServiceManager.GetService(ConstValue.MSMQServiceName);
            SetButtonStatus(serviceController1);

            timerCtrl.Interval = 100;
            timerCtrl.Tick += new EventHandler(timerCtrl_Tick);
            timerCtrl.Enabled = true;

            t1 = new Thread(new ThreadStart(GetServiceStatus));
            t1.IsBackground = true;
            t1.Start();

            ShowCtrlMsg();
        }

        private void InitConfig()
        {
            if (string.IsNullOrEmpty(Settings.Instance.GetString(SettingKeys.DBString)))
            {
                SetCtrlMsg(DateTime.Now.ToString() + " 未配置数据库连接\r\n");
                MessageBox.Show("请点击“数据库连接”按钮设置数据库连接!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            InitGPRS();
        }

        private void InitGPRS()
        {
            try
            {
                txtGPRSIP.Text = Settings.Instance.GetString(SettingKeys.GPRS_IP);
                txtGPRSPort.Text = Settings.Instance.GetString(SettingKeys.GPRS_PORT);
            }
            catch (Exception ex)
            {
                MessageBox.Show("初始化GPRS失败!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.ErrorException("InitGPRS", ex);
            }
        }

        private void SetCtrlMsg(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                lstCtrlMsg.Add(msg);
                ctrlMsgChange = true;
            }
        }

        void timerCtrl_Tick(object sender, EventArgs e)
        {
            if (ctrlMsgChange && txtControl.Visible)
            {
                //去除超过MaxLine行数部分
                int outliencount = lstCtrlMsg.Count - MaxLine;
                if (outliencount > 0)
                {
                    lstCtrlMsg.RemoveRange(0, outliencount);
                }

                ShowCtrlMsg();
                ctrlMsgChange = false;
            }
        }
        private delegate void AddMsgHandle(string msg);
        List<string> lstCtrlMsg = new List<string>();
        bool ctrlMsgChange = false;  //是否有更新消息
        public void ShowCtrlMsg()
        {
            try
            {
                string ctrlmsg = "";
                for (int i = 0; i < lstCtrlMsg.Count; i++)
                {
                    ctrlmsg += lstCtrlMsg[i];
                }
                txtControl.Clear();
                txtControl.AppendText(ctrlmsg);
                txtControl.DeselectAll();
                txtControl.ScrollToCaret();
            }
            catch (Exception ex)
            {
                logger.ErrorException("ShowCtrlMsg", ex);
                MessageBox.Show("输出消息时发生异常", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GetServiceStatus()
        {
            if (MessageQueue.Exists(QueuePath))
            {
                MessageQueue.Delete(QueuePath);
            }

            while (true)
            {
                t1.Join(1000);

                ServiceController serviceController1 = ServiceManager.GetService(ConstValue.MSMQServiceName);

                SetButtonStatus(serviceController1);

                if (serviceController1 != null)
                {
                    serviceController1.Refresh();

                    //if (serviceController1.Status != ServiceControllerStatus.Running)
                    //{
                    //    if (MessageQueue.Exists(QueuePath))
                    //    {
                    //        MessageQueue.Delete(QueuePath);
                    //    }

                    //    continue;
                    //}

                    MessageQueue MQueue;

                    if (MessageQueue.Exists(QueuePath))
                    {
                        MQueue = new MessageQueue(QueuePath);
                    }
                    else
                    {
                        MQueue = MessageQueue.Create(QueuePath);
                        MQueue.SetPermissions("Administrators", MessageQueueAccessRights.FullControl);
                        MQueue.Label = "GCGprsMSMQ";
                    }

                    //一次读取全部消息,但是不去除读过的消息
                    System.Messaging.Message[] Msg = MQueue.GetAllMessages();
                    //删除所有消息
                    MQueue.Purge();

                    foreach (System.Messaging.Message m in Msg)
                    {
                        m.Formatter = new BinaryMessageFormatter();
                        SetCtrlMsg(m.Body.ToString()+"\r\n");
                    }
                }
            }
        }

        private void SetButtonStatus(ServiceController servicestate)
        {
            if (servicestate == null)
            {
                btnInstall.Enabled = true;
                btnUninstall.Enabled = false;
                btnStart.Enabled = false;
                btnStop.Enabled = false;
            }
            else
            {
                if (servicestate.Status == ServiceControllerStatus.Paused)
                {
                    btnInstall.Enabled = false;
                    btnUninstall.Enabled = true;
                    btnStart.Enabled = true;
                    btnStop.Enabled = false;
                }
                else if (servicestate.Status == ServiceControllerStatus.Running)
                {
                    btnInstall.Enabled = false;
                    btnUninstall.Enabled = true;
                    btnStart.Enabled = false;
                    btnStop.Enabled = true;
                }
                else if (servicestate.Status == ServiceControllerStatus.Stopped)
                {
                    btnInstall.Enabled = false;
                    btnUninstall.Enabled = true;
                    btnStart.Enabled = true;
                    btnStop.Enabled = false;
                }
            }
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.No == MessageBox.Show("是否安装敢创上位机远传服务?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                {
                    return;
                }
                string servicePath = Application.StartupPath + "\\GCGPRSService.exe";
                if (!File.Exists(servicePath))
                    SetCtrlMsg("服务程序不存在，请重新安装\r\n");
                if (ServiceManager.InstallServie(servicePath))
                {
                    SetCtrlMsg(DateTime.Now.ToString() + " " + ServiceManager.GetServiceInfo(ConstValue.MSMQServiceName));
                    MessageBox.Show("服务安装成功");
                }
                else
                {
                    MessageBox.Show("服务安装失败");
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("btnInstall_Click", ex);
                MessageBox.Show("服务安装失败");
            }
            finally
            {
                ServiceController serviceController1 = ServiceManager.GetService(ConstValue.MSMQServiceName);
                SetButtonStatus(serviceController1);
            }
        }

        private void btnUninstall_Click(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.No == MessageBox.Show("是否卸载敢创上位机远传服务?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                {
                    return;
                }
                string servicePath = Application.StartupPath + "\\GCGPRSService.exe";
                if (!File.Exists(servicePath))
                    SetCtrlMsg("服务程序不存在，请重新安装\r\n");
                if (ServiceManager.UninstallService(servicePath))
                {
                    MessageBox.Show("服务卸载成功");
                }
                else
                {
                    MessageBox.Show("服务卸载失败");
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("btnUninstall_Click", ex);
                MessageBox.Show("服务卸载失败");
            }
            finally
            {
                ServiceController serviceController1 = ServiceManager.GetService(ConstValue.MSMQServiceName);
                SetButtonStatus(serviceController1);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (ServiceManager.StartService(ConstValue.MSMQServiceName))
                {
                    SetCtrlMsg(DateTime.Now.ToString() + " 服务启动成功\r\n");
                }
                else
                {
                    SetCtrlMsg(DateTime.Now.ToString() + " 服务启动失败\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("btnStart_Click", ex);
                MessageBox.Show("服务启动失败");
            }
            finally
            {
                ServiceController serviceController1 = ServiceManager.GetService(ConstValue.MSMQServiceName);
                SetButtonStatus(serviceController1);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (ServiceManager.StopService(ConstValue.MSMQServiceName))
                {
                    SetCtrlMsg(DateTime.Now.ToString() + " 服务成功停止\r\n");
                }
                else
                {
                    SetCtrlMsg(DateTime.Now.ToString() + " 服务停止失败\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("btnStop_Click", ex);
                MessageBox.Show("服务停止失败");
            }
            finally
            {
                ServiceController serviceController1 = ServiceManager.GetService(ConstValue.MSMQServiceName);
                SetButtonStatus(serviceController1);
            }
        }

        private void btnDBConnect_Click(object sender, EventArgs e)
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

                ServiceController serviceController1 = ServiceManager.GetService(ConstValue.MSMQServiceName);
                if ((serviceController1 != null) && (serviceController1.Status == ServiceControllerStatus.Running))
                {
                    btnStop_Click(null, null);
                    btnStart_Click(null, null);
                }
                else
                {
                    MessageBox.Show("设置成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnGPRSConfirm_Click(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(txtGPRSIP.Text, @"^(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])$"))
            {
                MessageBox.Show("请填写合法的IP值,如:'192.168.1.2'", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtGPRSIP.SelectAll();
                txtGPRSIP.Focus();
                return;
            }

            if (!Regex.IsMatch(txtGPRSPort.Text, "^\\d{1,5}$"))
            {
                MessageBox.Show("请填写合法的端口值[0-65535]", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtGPRSPort.SelectAll();
                txtGPRSPort.Focus();
                return;
            }
            if (Convert.ToInt32(txtGPRSPort.Text) > 65535)
            {
                MessageBox.Show("请填写合法的端口值[0-65535]", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtGPRSPort.SelectAll();
                txtGPRSPort.Focus();
                return;
            }

            Settings.Instance.SetValue(SettingKeys.GPRS_IP, txtGPRSIP.Text);
            Settings.Instance.SetValue(SettingKeys.GPRS_PORT, txtGPRSPort.Text);

            ServiceController serviceController1 = ServiceManager.GetService(ConstValue.MSMQServiceName);
            if ((serviceController1 != null) && (serviceController1.Status == ServiceControllerStatus.Running))
            {
                btnStop_Click(null, null);
                btnStart_Click(null, null);
            }
            else
            {
                MessageBox.Show("设置IP与端口成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (t1 != null)
                    t1.Abort();
            }
            catch { }
        }
    }
}
