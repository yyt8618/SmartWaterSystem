using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using Common;
using System.Text;
using System.Diagnostics;
using System.Data;
using DevExpress.XtraTreeList.Nodes;
using System.Drawing;
using Entity;
using System.Collections;

namespace SmartWaterSystem
{
    public partial class FrmConsole : DevExpress.XtraEditors.XtraForm
    {
        class ColorMsgInfo
        {
            public string Msg = "";
            public Color color = Color.Lime;   //默认为Lime色

            public ColorMsgInfo(string msg)
            {
                this.Msg = msg;
            }
            public ColorMsgInfo(string msg,Color color)
            {
                this.Msg = msg;
                this.color = color;
            }
        }

        NLog.Logger logger = NLog.LogManager.GetLogger("FrmGPRSConsole");
        private const int MaxLine = 2000;
        private const int BufLine = 200;    //行缓冲数量
        private bool showHttpMsg = true;  //是否显示HTTP消息
        private bool showSocketMsg = true;   //是否显示Socket消息
        private bool showErrMsg = true;     //是否显示错误信息

        Hashtable ht_color = null;          //保存配置的颜色信息

        public FrmConsole()
        {
            InitializeComponent();

            timerCtrl.Interval = 150;
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
            UpdateColorConfig();
            UpdateFont();
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
            if (!string.IsNullOrEmpty(msg))
                SetCtrlMsg(msg, ColorType.SerialPort);
        }

        void MSMQMgr_MSMQEvent(object sender, SocketEventArgs e)
        {
            if (e.msmqEntity != null  && e.msmqEntity.MsgType == Entity.ConstValue.MSMQTYPE.Msg_Socket)
            {
                SetCtrlMsg(e.msmqEntity.Msg, e.msmqEntity.ShowType);
            }
        }

        private void SetCtrlMsg(string msg, ColorType showType)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                lock(lockMsgChange)
                {
                    if (!msg.EndsWith("\r\n"))
                        msg += "\r\n";

                    ColorMsgInfo msginfo = null;
                    if (ht_color != null && ht_color[(int)showType] != null)
                        msginfo = new ColorMsgInfo(msg, Color.FromArgb(Convert.ToInt32(ht_color[(int)showType])));
                    else
                        msginfo = new ColorMsgInfo(msg);
                    
                    lstCtrlMsg.Add(msginfo);
                    ctrlMsgChange = true;
                    if(lstCtrlMsg.Count  > MaxLine+BufLine)
                    {
                        lstCtrlMsg.RemoveRange(0, lstCtrlMsg.Count - MaxLine);
                    }
                }
            }
        }

        void timerCtrl_Tick(object sender, EventArgs e)
        {
            if (ctrlMsgChange && this.WindowState != FormWindowState.Minimized)  //txtControl.Visible
            {
                lock (lockMsgChange)
                {
                    if(picBoxLog.Tag.ToString()=="1")
                    {
                        WriteLog();
                    }
                    ShowCtrlMsg();
                    ctrlMsgChange = false;
                }
            }
        }

        private delegate void AddMsgHandle(string msg);
        List<ColorMsgInfo> lstCtrlMsg = new List<ColorMsgInfo>();
        bool ctrlMsgChange = false;  //是否有更新消息
        object lockMsgChange = new object();
        //string lastline = "";
        public void ShowCtrlMsg()
        {
            try
            {
                if (btnPause.Text == "暂停")
                {
                    for (int i = 0; i < lstCtrlMsg.Count; i++)
                    {
                        //txtControl.SelectionColor = lstCtrlMsg[i].color;
                        //txtControl.AppendText(lstCtrlMsg[i].Msg);


                        txtControl.SelectionStart = txtControl.TextLength;
                        txtControl.SelectionLength = 0;
                        txtControl.SelectionColor = lstCtrlMsg[i].color;
                        txtControl.AppendText(lstCtrlMsg[i].Msg);
                    }
                    if (txtControl.Lines.Length > MaxLine+ BufLine)  //BufLine行作为缓存,这样的话就不需要每次都执行下面的耗时操作
                    {
                        int moreLines = txtControl.Lines.Length - MaxLine;
                        string str_rtf=txtControl.Rtf;
                        int index = 0;
                        int headindex = 0;
                        for(int i=0;i<moreLines;i++)
                        {
                            index = str_rtf.IndexOf('\r', index);
                            index+=2;
                            if (i == 1) //保存前2行
                                headindex = index;
                        }
                        //str_rtf = str_rtf.Substring(headindex, index - headindex);
                        string head_rtf = str_rtf.Substring(0, headindex);
                        str_rtf = str_rtf.Substring(index);
                        txtControl.Rtf = head_rtf + str_rtf;

                        head_rtf = null;
                        str_rtf = null;
                        //string[] lines = txtControl.Lines;
                        //Array.Copy(lines, moreLines, lines, 0, MaxLine);
                        //lastline = lines[MaxLine - 1] + "\r\n";
                        //Array.Resize(ref lines, MaxLine - 1);
                        //txtControl.Lines = lines;
                        //txtControl.AppendText(lastline);  //前一行的直接复制导致ScrollToCaret不能滚动到最后一行显示，需要将最后一行用AppendText方法再调用ScrollToCaret才能滚动到最后一行
                    }

                    txtControl.ScrollToCaret();
                    lstCtrlMsg.Clear();
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
                                    //comboSocketServer.Properties.Items.Add(strcols[0]);
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

        /// <summary>
        /// 获取颜色配置信息更新ht_color变量
        /// </summary>
        public void UpdateColorConfig()
        {
            ht_color=new MsgColorHelper().GetColorConfig(GlobalValue.ColorConfigFilePath);
            SetBackColor();
        }

        private void SetBackColor()
        {
            if (ht_color != null && ht_color[(int)ColorType.BackColor] != null)
            {
                Color backcolor = Color.FromArgb(Convert.ToInt32(ht_color[(int)ColorType.BackColor]));
                txtControl.BackColor = backcolor;
            }
        }

        public void UpdateFont()
        {
            try
            {
                txtControl.Font = new Font(Settings.Instance.GetString(SettingKeys.ConsoleFont), Convert.ToSingle(Settings.Instance.GetString(SettingKeys.ConsoleFontSize)));
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("设置窗体时发生异常,ex" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            {
                btnPause.Text = "继续";
                this.btnPause.BackColor = System.Drawing.Color.Red;
                this.btnPause.ForeColor = System.Drawing.Color.Black;
            }
            else {
                btnPause.Text = "暂停";
                this.btnPause.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(30)))));
                this.btnPause.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(226)))), ((int)(((byte)(254)))));
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
                case "C#超级通信调试工具":
                    OpenTool(Application.StartupPath + "\\ToolBox\\C#超级通信调试工具.exe");
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
                this.btnPause.BackColor = System.Drawing.Color.Red;
                this.btnPause.ForeColor = System.Drawing.Color.Black;
                if (txtControl.SelectedText.Length > 0 && txtControl.SelectedText.Length < 50)
                    frmsearch.KeyText = txtControl.SelectedText;

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

        #region 记录Console内容
        StreamWriter sw = null;
        private void picBoxLog_Click(object sender, EventArgs e)
        {
            string logpath = Settings.Instance.GetString(SettingKeys.ConsoleLogPath);
            if (((System.Windows.Forms.MouseEventArgs)e).Button == MouseButtons.Left)
            {
                if (string.IsNullOrEmpty(logpath))
                {
                    XtraMessageBox.Show("请先设置文件保存位置!");
                    return;
                }
                if (picBoxLog.Tag != null)
                {
                    if (Convert.ToInt32(picBoxLog.Tag) == 0)//不记录日志到文件
                    {
                        picBoxLog.Tag = 1;      //开始记录
                        picBoxLog.Image = global::SmartWaterSystem.Properties.Resources.WriteLog1;
                    }
                    else    //停止记录
                    {
                        picBoxLog.Tag = 0;      //不记录
                        picBoxLog.Image = global::SmartWaterSystem.Properties.Resources.WriteLog;
                    }
                }
                else
                    picBoxLog.Tag = 0;
            }
            else if (((System.Windows.Forms.MouseEventArgs)e).Button == MouseButtons.Right)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.ShowNewFolderButton = true;
                fbd.SelectedPath = logpath;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string localFilePath = fbd.SelectedPath; //获得文件路径
                    Settings.Instance.SetValue(SettingKeys.ConsoleLogPath, localFilePath);
                }
            }
        }
        
        private void WriteLog()
        {
            try
            {
                string logpath = Settings.Instance.GetString(SettingKeys.ConsoleLogPath);
                if (string.IsNullOrEmpty(logpath))
                    return;
                if (!Directory.Exists(logpath))
                {
                    Directory.CreateDirectory(logpath);
                }
                string filepath = Path.Combine(logpath, DateTime.Now.ToString("yyyy-MM-dd") + "_Console.txt");
                if (!File.Exists(filepath))
                {
                    FileStream fs=File.Create(filepath);
                    fs.Close();
                }
                if (File.Exists(filepath))
                {
                    sw = File.AppendText(filepath);
                    if (lstCtrlMsg != null && lstCtrlMsg.Count > 0)
                    {
                        for (int i = 0; i < lstCtrlMsg.Count; i++)
                        {
                            sw.Write(lstCtrlMsg[i]);
                        }
                    }
                    sw.Flush();
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("WriteLog", ex);
                SetCtrlMsg(DateTime.Now.ToString() + " 记录日志发生错误,ex:" + ex.Message, ColorType.Error);
            }
            finally
            {
                sw.Close(); 
            }
        }

        #endregion

        private void btnDmp_Click(object sender, EventArgs e)
        {
            SocketEntity socketmsg = new SocketEntity();
            socketmsg.MsgType = Entity.ConstValue.MSMQTYPE.MiniDump;
            GlobalValue.SocketMgr.SendMessage(socketmsg);
        }

        private void pbColor_Click(object sender, EventArgs e)
        {
            FrmColorSet colorFrm = new FrmColorSet();
            colorFrm.ShowDialog();
        }
    }
}
