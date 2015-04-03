using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Messaging;
using System.ServiceProcess;
using Common;

namespace SmartWaterSystem
{
    public partial class FrmGPRSConsole : DevExpress.XtraEditors.XtraForm
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("FrmGPRSConsole");
        private const int MaxLine = 1001;

        string QueuePath = ".\\private$\\GcGPRS";
        string ServiceName = "GCGPRSService";
        Thread t1;

        public FrmGPRSConsole()
        {
            InitializeComponent();
        }

        private void FrmGPRSConsole_Load(object sender, EventArgs e)
        {
            FrmGPRSConsole.CheckForIllegalCrossThreadCalls = false;

            timerCtrl.Interval = 100;
            timerCtrl.Tick += new EventHandler(timerCtrl_Tick);
            timerCtrl.Enabled = true;

            t1 = new Thread(new ThreadStart(GetServiceStatus));
            t1.IsBackground = true;
            t1.Start();

            ShowCtrlMsg();

            txtControl.SelectionStart = txtControl.Text.Length;
            txtControl.ScrollToCaret();
            txtControl.SelectedText = "";
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
                txtControl.ResetText();
                txtControl.Text+=ctrlmsg;
                txtControl.SelectionStart = txtControl.Text.Length;
                txtControl.ScrollToCaret();
                txtControl.SelectedText = "";
            }
            catch (Exception ex)
            {
                logger.ErrorException("ShowCtrlMsg", ex);
                MessageBox.Show("输出消息时发生异常", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        bool showstopmsg = false;
        private void GetServiceStatus()
        {
            try
            {
                if (MessageQueue.Exists(QueuePath))
                {
                    MessageQueue.Delete(QueuePath);
                }

                while (true)
                {
                    t1.Join(1000);

                    ServiceController serviceController1 = ServiceManager.GetService(ServiceName);


                    if (serviceController1 != null)
                    {
                        serviceController1.Refresh();

                        if (serviceController1.Status == ServiceControllerStatus.Stopped)
                        {
                            if (!showstopmsg)
                                SetCtrlMsg("服务已停止" + "\r\n");
                            showstopmsg = true;
                        }
                        else
                            showstopmsg = false;

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
                            SetCtrlMsg(m.Body.ToString() + "\r\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetServiceStatus", ex);
                SetCtrlMsg(ex.Message+"\r\n");
            }   
        }

        private void FrmGPRSConsole_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }


    }
}
