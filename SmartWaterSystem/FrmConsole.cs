using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using Common;
using System.Text;
using System.Diagnostics;

namespace SmartWaterSystem
{
    public partial class FrmConsole : DevExpress.XtraEditors.XtraForm
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("FrmGPRSConsole");
        private const int MaxLine = 1200;
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
        
        private void FrmConsole_Load(object sender, EventArgs e)
        {
            FrmConsole.CheckForIllegalCrossThreadCalls = false;
            ShowCtrlMsg();

            txtControl.SelectionStart = txtControl.Text.Length;
            txtControl.ScrollToCaret();
            txtControl.SelectedText = "";

            UpdateSocketList();
        }

        private void SocketMgr_SocketConnEvent(object sender, SocketStatusEventArgs e)
        {
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
                if (btnPause.Text == "暂停")
                {
                    string ctrlmsg = "";
                    for (int i = 0; i < lstCtrlMsg.Count; i++)
                    {
                        ctrlmsg += lstCtrlMsg[i];
                    }
                    txtControl.ResetText();
                    txtControl.AppendText(ctrlmsg);
                }
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

        //复制消息至剪贴板
        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(txtControl.Text, true);
        }

        private void FrmConsole_VisibleChanged(object sender, EventArgs e)
        {
            
        }

        public void UpdateSocketList()
        {
            try
            {
                this.comboSocketServer.SelectedIndexChanged -= new System.EventHandler(this.comboSocketServer_SelectedIndexChanged);
                if (File.Exists(GlobalValue.SocketConfigFilePath))
                {
                    using (StreamReader reader = new StreamReader(GlobalValue.SocketConfigFilePath, Encoding.UTF8))
                    {
                        string selectedrow = Settings.Instance.GetString(SettingKeys.GPRS_IP) + "\t" + Settings.Instance.GetString(SettingKeys.GPRS_PORT);
                        string selectname = "";
                        comboSocketServer.SelectedIndex = -1;
                        do
                        {
                            string strrow = reader.ReadLine();
                            if (!string.IsNullOrEmpty(strrow))
                            {
                                string[] strcols = strrow.Split('\t');
                                if (strcols != null && strcols.Length == 3)
                                {
                                    comboSocketServer.Properties.Items.Add(strcols[0]);
                                    bool bcontain = false;  //标记是否列表中已包含
                                    for (int i = 0; i < comboSocketServer.Properties.Items.Count; i++)
                                    {
                                        if (strcols[0] == comboSocketServer.Properties.Items[i].ToString())
                                        {
                                            bcontain = true;
                                        }
                                    }

                                    if (!bcontain) //未包含的情况下添加进列表
                                    {
                                        comboSocketServer.Properties.Items.Add(strcols[0]);
                                    }

                                    if (strrow.Contains(selectedrow))
                                        selectname = strcols[0];
                                }
                            }

                        } while (!reader.EndOfStream);
                        //设置选中的数据
                        if (!string.IsNullOrEmpty(selectname))
                        {
                            for (int i = 0; i < comboSocketServer.Properties.Items.Count; i++)
                            {
                                if (selectname == comboSocketServer.Properties.Items[i].ToString())
                                {
                                    comboSocketServer.SelectedIndex = i;
                                    break;
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("初始化Socket列表失败!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.comboSocketServer.SelectedIndexChanged += new System.EventHandler(this.comboSocketServer_SelectedIndexChanged);
            }
        }

        private void comboSocketServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboSocketServer.SelectedIndex>-1)
            {
                try
                {
                    if (File.Exists(GlobalValue.SocketConfigFilePath))
                    {
                        using (StreamReader reader = new StreamReader(GlobalValue.SocketConfigFilePath, Encoding.UTF8))
                        {
                            do
                            {
                                string strrow = reader.ReadLine();
                                if (!string.IsNullOrEmpty(strrow))
                                {
                                    string[] strcols = strrow.Split('\t');
                                    if (strcols != null && strcols.Length == 3)
                                    {
                                        if(strcols[0] == comboSocketServer.SelectedItem.ToString())
                                        {
                                            Settings.Instance.SetValue(SettingKeys.GPRS_IP, strcols[1]);
                                            Settings.Instance.SetValue(SettingKeys.GPRS_PORT, strcols[2]);
                                        }
                                    }
                                }

                            } while (!reader.EndOfStream);
                        }
                    }
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("设置Socket失败!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (btnPause.Text == "暂停")
                btnPause.Text = "继续";
            else {
                btnPause.Text = "暂停";
                ShowCtrlMsg();
            }
        }

        #region 工具
        private void comboToolBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboToolBox.SelectedItem.ToString())
            {
                case "计算器":
                    OpenTool("calc");
                    break;
                case "串口调试助手":
                    OpenTool(Application.StartupPath + "\\ToolBox\\UartAssist.exe");
                    break;
                case "网络调试助手":
                    OpenTool(Application.StartupPath + "\\ToolBox\\NetAssist.exe");
                    break;
                case "串口通讯调试器":
                    OpenTool(Application.StartupPath + "\\ToolBox\\串口通讯调试器.exe");
                    break;
            }
            this.comboToolBox.SelectedIndexChanged -= new System.EventHandler(this.comboToolBox_SelectedIndexChanged);
            comboToolBox.SelectedIndex = -1;
            comboToolBox.Text = "工具箱";
            this.comboToolBox.SelectedIndexChanged += new System.EventHandler(this.comboToolBox_SelectedIndexChanged);
        }
        private void OpenTool(string toolname)
        {
            try
            {
                Process.Start(toolname);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region 查找
        FrmSearch frmsearch = new FrmSearch();
        private void txtControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 70)  //Ctrl+F
            {
                frmsearch.frmCtrl = this;
                btnPause.Text = "继续";
                frmsearch.Show();
            }
        }

        public string FindText(string content, RichTextBoxFinds options, bool MatchCase)
        {
            int startIndex;
            int endIndex;

            if ((options & RichTextBoxFinds.Reverse) == RichTextBoxFinds.Reverse)
            {
                startIndex = 0;
                endIndex = txtControl.SelectionStart;
            }
            else
            {
                startIndex = txtControl.SelectionStart + txtControl.SelectionLength;
                endIndex = txtControl.Text.Length;
            }
            int index = -1;
            if (MatchCase)
                index = txtControl.Find(content, startIndex, endIndex, RichTextBoxFinds.MatchCase | options);
            else
                index = txtControl.Find(content, startIndex, endIndex, options);

            if (index >= 0) //如果找到
                ShowSelection(txtControl, index, content.Length);
            else
                return "找不到\"" + content + "\"";
            return "";
        }

        //查找第一个
        public void FindFirst(RichTextBox rich, string content)
        {
            int index = rich.Find(content, 0);
            if (index >= 0) ShowSelection(rich, index, content.Length);
        }

        //查找最后一个
        public void FindLast(RichTextBox rich, string content)
        {
            int index = rich.Find(content, rich.Text.Length, RichTextBoxFinds.Reverse);
            if (index >= 0) ShowSelection(rich, index, content.Length);
        }

        //选择搜索到的文本
        private void ShowSelection(RichTextBox rich, int index, int length)
        {
            rich.SelectionStart = index;
            rich.SelectionLength = length;
            //rich.SelectionColor = Color.Red;
            rich.Focus();
        }
        #endregion

        
    }
}
