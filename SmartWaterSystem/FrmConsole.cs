using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SmartWaterSystem
{
    public partial class FrmConsole : DevExpress.XtraEditors.XtraForm
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("FrmGPRSConsole");
        private const int MaxLine = 1000;
        private bool showHttpMsg = true;  //是否显示HTTP消息
        private bool showSocketMsg = true;   //是否显示Socket消息
        private bool showErrMsg = true;     //是否显示错误信息

        public FrmConsole()
        {
            InitializeComponent();

            timerCtrl.Interval = 100;
            timerCtrl.Tick += new EventHandler(timerCtrl_Tick);
            timerCtrl.Enabled = true;

            GlobalValue.SocketMgr.SockMsgEvent += new SocketHandler(MSMQMgr_MSMQEvent);
            GlobalValue.SerialPortMgr.serialPortUtil.ShowMsgEvent += new Protocol.ShowMsgHandle(serialPortUtil_ShowMsgEvent);
            GlobalValue.SocketMgr.SocketConnEvent += SocketMgr_SocketConnEvent;
        }

        private void SocketMgr_SocketConnEvent(object sender, SocketStatusEventArgs e)
        {
            //try
            //{
            //    if (e.Connect)
            //    {
            //        picSockConnect.Image = global::SmartWaterSystem.Properties.Resources.SockConnect;
            //        //btnSocketConnect.Enabled = false;
            //    }
            //    else
            //    {
            //        picSockConnect.Image = global::SmartWaterSystem.Properties.Resources.SockNotConnect;
            //        //btnSocketConnect.Enabled = true;
            //    }
            //}
            //catch { }
            SocketStatusChange(e.Connect);
        }

        private delegate void SocketStatusHandle(bool IsConnect);
        private void SocketStatusChange(bool isconnect)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SocketStatusHandle(SocketStatusChange), isconnect);
            }
            else
                SetSocketStatus(isconnect);
        }

        private void SetSocketStatus(bool isconnect)
        {
            try
            {
                if (isconnect)
                {
                    picSockConnect.Image = global::SmartWaterSystem.Properties.Resources.SockConnect;
                }
                else
                {
                    picSockConnect.Image = global::SmartWaterSystem.Properties.Resources.SockNotConnect;
                }
            }
            catch { }
        }

        void serialPortUtil_ShowMsgEvent(string msg)
        {
            if(!string.IsNullOrEmpty(msg))
                SetCtrlMsg(msg);
        }

        private void FrmConsole_Load(object sender, EventArgs e)
        {
            FrmConsole.CheckForIllegalCrossThreadCalls = false;
            ShowCtrlMsg();

            txtControl.SelectionStart = txtControl.Text.Length;
            txtControl.ScrollToCaret();
            txtControl.SelectedText = "";
        }

        void MSMQMgr_MSMQEvent(object sender, SocketEventArgs e)
        {
            if (e.msmqEntity != null)
            {
                if (e.msmqEntity.MsgType == Entity.ConstValue.MSMQTYPE.Msg_Public)
                {
                    SetCtrlMsg(e.msmqEntity.Msg);
                }
                else if (e.msmqEntity.MsgType == Entity.ConstValue.MSMQTYPE.Msg_Socket && showSocketMsg)
                {
                    SetCtrlMsg(e.msmqEntity.Msg);
                }
                else if (e.msmqEntity.MsgType == Entity.ConstValue.MSMQTYPE.Msg_HTTP && showHttpMsg)
                {
                    SetCtrlMsg(e.msmqEntity.Msg);
                }
                else if (e.msmqEntity.MsgType == Entity.ConstValue.MSMQTYPE.Msg_Err && showErrMsg)
                {
                    SetCtrlMsg(e.msmqEntity.Msg);
                }
            }
        }

        private void SetCtrlMsg(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                lock(lockMsgChange)
                {
                    if (!msg.EndsWith("\r\n"))
                        msg += "\r\n";
                    lstCtrlMsg.Add(msg);
                    ctrlMsgChange = true;
                }
            }
        }

        void timerCtrl_Tick(object sender, EventArgs e)
        {
            if (ctrlMsgChange && txtControl.Visible)
            {
                lock (lockMsgChange)
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
        }

        private delegate void AddMsgHandle(string msg);
        List<string> lstCtrlMsg = new List<string>();
        bool ctrlMsgChange = false;  //是否有更新消息
        object lockMsgChange = new object();
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
                txtControl.AppendText(ctrlmsg);
            }
            catch (Exception ex)
            {
                logger.ErrorException("ShowCtrlMsg", ex);
                XtraMessageBox.Show("输出消息时发生异常", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmGPRSConsole_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtControl.Clear();
            lstCtrlMsg.Clear();
        }

        private void cbShowSocket_CheckedChanged(object sender, EventArgs e)
        {
            showSocketMsg = cbShowSocket.Checked;
        }

        private void cbHTTP_CheckedChanged(object sender, EventArgs e)
        {
            showHttpMsg = cbHTTP.Checked;
        }

        private void cbSerialPort_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSerialPort.Checked)
            {
                GlobalValue.SerialPortMgr.serialPortUtil.ShowMsgEvent -= new Protocol.ShowMsgHandle(serialPortUtil_ShowMsgEvent);
                GlobalValue.SerialPortMgr.serialPortUtil.ShowMsgEvent += new Protocol.ShowMsgHandle(serialPortUtil_ShowMsgEvent);
            }
            else
                GlobalValue.SerialPortMgr.serialPortUtil.ShowMsgEvent -= new Protocol.ShowMsgHandle(serialPortUtil_ShowMsgEvent);
        }

        private void cbErrs_CheckedChanged(object sender, EventArgs e)
        {
            showErrMsg = cbErrs.Checked;
        }

        private void btnSocketConnect_Click(object sender, EventArgs e)
        {
            GlobalValue.SocketMgr.ReConnect();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            GlobalValue.SocketMgr.DisConnect();
        }
    }
}
