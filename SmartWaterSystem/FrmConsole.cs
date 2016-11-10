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

namespace SmartWaterSystem
{
    public partial class FrmConsole : DevExpress.XtraEditors.XtraForm
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("FrmGPRSConsole");
        private const int MaxLine = 3500;
        private bool showHttpMsg = true;  //是否显示HTTP消息
        private bool showSocketMsg = true;   //是否显示Socket消息
        private bool showErrMsg = true;     //是否显示错误信息

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

            Inittree();
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
        List<string> lstCtrlMsg = new List<string>();
        bool ctrlMsgChange = false;  //是否有更新消息
        object lockMsgChange = new object();
        string lastline = "";
        public void ShowCtrlMsg()
        {
            try
            {
                if (btnPause.Text == "暂停")
                {
                    for (int i = 0; i < lstCtrlMsg.Count; i++)
                    {
                        txtControl.AppendText(lstCtrlMsg[i]);
                    }
                    if (txtControl.Lines.Length > MaxLine+200)  //200行作为缓存,这样的话就不需要每次都执行下面的耗时操作
                    {
                        int moreLines = txtControl.Lines.Length - MaxLine;
                        string[] lines = txtControl.Lines;
                        Array.Copy(lines, moreLines, lines, 0, MaxLine);
                        lastline = lines[MaxLine - 1]+ "\r\n";
                        Array.Resize(ref lines, MaxLine-1);
                        txtControl.Lines = lines;
                        txtControl.AppendText(lastline);  //前一行的直接复制导致ScrollToCaret不能滚动到最后一行显示，需要将最后一行用AppendText方法再调用ScrollToCaret才能滚动到最后一行
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
                SetCtrlMsg(DateTime.Now.ToString() + " 记录日志发生错误,ex:" + ex.Message);
            }
            finally
            {
                sw.Close(); 
            }
        }

        #endregion

        #region treeSocketType
        private void Inittree()
        {
            DataTable dttree = new DataTable();
            dttree.Columns.Add("ID",typeof(int));
            dttree.Columns.Add("Name");
            dttree.Columns.Add("ParentID", typeof(int));

            int i = 1,tmpparent=-1;
            DataRow dr = dttree.NewRow();
            dr["ID"] = -99;
            dr["Name"] = "全部";
            dr["ParentID"] = -999;
            dttree.Rows.Add(dr);

            dr = dttree.NewRow();
            dr["ID"] = -1;
            dr["Name"] = "远传";
            dr["ParentID"] = -99;
            dttree.Rows.Add(dr);
            dr = dttree.NewRow();
            dr["ID"] = -2;
            dr["Name"] = "手机";
            dr["ParentID"] = -99;
            dttree.Rows.Add(dr);
            dr = dttree.NewRow();
            dr["ID"] = -3;
            dr["Name"] = "串口";
            dr["ParentID"] = -99;
            dttree.Rows.Add(dr);
            dr = dttree.NewRow();
            dr["ID"] = -4;
            dr["Name"] = "错误";
            dr["ParentID"] = -99;
            dttree.Rows.Add(dr);

            dr = dttree.NewRow();
            dr["ID"] = i;
            dr["Name"] = "公共";
            dr["ParentID"] = -1;
            dttree.Rows.Add(dr);
            dr = dttree.NewRow();
            dr["ID"] = ++i;
            dr["Name"] = "错误";
            dr["ParentID"] = -1;
            dttree.Rows.Add(dr);
            dr = dttree.NewRow();
            dr["ID"] = ++i;
            dr["Name"] = "解析失败";
            dr["ParentID"] = -1;
            dttree.Rows.Add(dr);
            dr = dttree.NewRow();
            dr["ID"] = ++i;
            dr["Name"] = "数据帧";
            dr["ParentID"] = -1;
            dttree.Rows.Add(dr);
            
            dr = dttree.NewRow();
            dr["ID"] = ++i;
            tmpparent = i;
            dr["Name"] = "终端";
            dr["ParentID"] = -1;
            dttree.Rows.Add(dr);
            dr = dttree.NewRow();
            dr["ID"] = ++i;
            dr["Name"] = "噪声终端";
            dr["ParentID"] = tmpparent;
            dttree.Rows.Add(dr);
            dr = dttree.NewRow();
            dr["ID"] = ++i;
            dr["Name"] = "压力流量终端";
            dr["ParentID"] = tmpparent;
            dttree.Rows.Add(dr);
            dr = dttree.NewRow();
            dr["ID"] = ++i;
            dr["Name"] = "通用终端";
            dr["ParentID"] = tmpparent;
            dttree.Rows.Add(dr);
            dr = dttree.NewRow();
            dr["ID"] = ++i;
            dr["Name"] = "阀门开度控制器";
            dr["ParentID"] = tmpparent;
            dttree.Rows.Add(dr);
            dr = dttree.NewRow();
            dr["ID"] = ++i;
            dr["Name"] = "在线水质终端";
            dr["ParentID"] = tmpparent;
            dttree.Rows.Add(dr);
            dr = dttree.NewRow();
            dr["ID"] = ++i;
            dr["Name"] = "消防栓";
            dr["ParentID"] = tmpparent;
            dttree.Rows.Add(dr);
            dr = dttree.NewRow();
            dr["ID"] = ++i;
            dr["Name"] = "水厂数据";
            dr["ParentID"] = tmpparent;
            dttree.Rows.Add(dr);
            
            treeSocketType.Properties.DataSource = dttree;
            treeSocketType.Properties.ValueMember = "ID";
            treeSocketType.Properties.DisplayMember = "Name";

            treeSocketType.Properties.TreeList.ParentFieldName = "ParentID";
            treeSocketType.Properties.TreeList.KeyFieldName = "ID";

            if(treeSocketType.Properties.TreeList.Nodes!= null)
            {
                foreach (TreeListNode node in treeSocketType.Properties.TreeList.GetNodeList())
                {
                    node.Checked = true;
                }
            }

            treeSocketType.Properties.TreeList.AfterCheckNode += (s, a) =>
            {
                a.Node.Selected = true;
                //DataRowView drv = tlOffice.Properties.TreeList.GetDataRecordByNode(node) as DataRowView;//关键代码，就是不知道是这样获取数据而纠结了很久(可以转换为DataRowView啊)
                UpdateParentNodesCheckstate(a.Node, a.Node.Checked);
                UpdateChildsNodesCheckstate(a.Node, a.Node.Checked);
            };
        }

        /// <summary>
        /// 更新父节点的选中状态
        /// </summary>
        /// <param name="node"></param>
        /// <param name="Checked"></param>
        private void UpdateParentNodesCheckstate(TreeListNode node,bool Checked)
        {
            if(node.ParentNode!=null)
            {
                
                bool childcheck=false, childuncheck = false;    //除去自身是否有选择或未选中的孩子节点
                foreach (TreeListNode child in node.ParentNode.Nodes)
                {
                    if (child.CheckState != CheckState.Unchecked)
                        childcheck = true;
                    else
                        childuncheck = true;
                }
                if (Checked)   //如果当前节点选中,则父节点需要改变为中间态或者选中状态
                {
                    if (childuncheck)
                        node.ParentNode.CheckState = CheckState.Indeterminate;
                    else
                        node.ParentNode.CheckState = CheckState.Checked;
                }
                else
                {
                    if (childcheck)
                        node.ParentNode.CheckState = CheckState.Indeterminate;
                    else
                        node.ParentNode.CheckState = CheckState.Unchecked;
                }
                UpdateParentNodesCheckstate(node.ParentNode, Checked);
            }
        }

        /// <summary>
        /// 更新孩子节点的选中状态
        /// </summary>
        /// <param name="node"></param>
        /// <param name="Checked"></param>
        private void UpdateChildsNodesCheckstate(TreeListNode node, bool Checked)
        {
            if (node.Nodes != null && node.Nodes.Count > 0)
            {
                foreach (TreeListNode child in node.Nodes)
                {
                    if (Checked)
                        child.CheckState = CheckState.Checked;
                    else
                        child.CheckState = CheckState.Unchecked;

                    UpdateChildsNodesCheckstate(child, Checked);
                }
                if (Checked)
                    node.CheckState = CheckState.Checked;
                else
                    node.CheckState = CheckState.Unchecked;
            }
        }
        #endregion
    }
}
