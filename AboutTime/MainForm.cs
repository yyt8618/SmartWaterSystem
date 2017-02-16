using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace AboutTime
{
    public partial class MainForm : Form
    {
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private string HostsFilePath = Application.StartupPath + @"\Hosts.txt";

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text = "AboutTime " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            InitView();
        }

        private void InitView()
        {
            #region 初始化主机列表
            InitHosts();
            #endregion

            #region 初始化选项
            InitOptions();
            #endregion
        }

        private void InitOptions()
        {
            try
            {
                this.cbAutoTiming.CheckedChanged -= new System.EventHandler(this.cbAutoTiming_CheckedChanged);
                this.cbAllHosts.CheckedChanged -= new System.EventHandler(this.cbAllHosts_CheckedChanged);
                this.cbAutoHide.CheckedChanged -= new System.EventHandler(this.cbAutoHide_CheckedChanged);
                this.cbAutoStart.CheckedChanged -= new System.EventHandler(this.cbAutoStart_CheckedChanged);
                this.cbService.CheckedChanged -= new System.EventHandler(this.cbService_CheckedChanged);
                this.txtServicePort.TextChanged -= new System.EventHandler(this.txtInterval_TextChanged);

                cbAutoTiming.Checked = Convert.ToBoolean(config.AppSettings.Settings["AutoTimingEnable"].Value);
                txtInterval.Text = config.AppSettings.Settings["AutoTimingInterval"].Value;
                cbAllHosts.Checked = Convert.ToBoolean(config.AppSettings.Settings["TryAllHosts"].Value);
                cbAutoHide.Checked = Convert.ToBoolean(config.AppSettings.Settings["AutoHide"].Value);
                cbAutoStart.Checked= Convert.ToBoolean(config.AppSettings.Settings["AutoStart"].Value) ;
                cbService.Checked = Convert.ToBoolean(config.AppSettings.Settings["Service"].Value);
                txtServiceIP.Text = config.AppSettings.Settings["ServiceIP"].Value;
                txtServicePort.Text = config.AppSettings.Settings["ServicePort"].Value;
                if (cbAutoStart.Checked) //设置开机自启动  
                {
                    string path = Application.ExecutablePath;
                    RegistryKey rk = Registry.LocalMachine;
                    RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                    rk2.SetValue("AboutTime", path);
                    rk2.Close();
                    rk.Close();

                    config.AppSettings.Settings["AutoStart"].Value = cbAutoStart.Checked.ToString();
                }

                if (cbAutoTiming.Checked)
                {
                    timer_interval.Interval = Convert.ToInt32(txtInterval.Text) * 60 * 1000;
                    timer_interval.Enabled = true;
                }
                else
                {
                    timer_interval.Enabled = false;
                }

                if(cbAutoHide.Checked)
                {
                    btnHide_Click(null, null);
                }

                if (cbService.Checked)
                    TimeService();

                this.cbAutoTiming.CheckedChanged += new System.EventHandler(this.cbAutoTiming_CheckedChanged);
                this.cbAllHosts.CheckedChanged += new System.EventHandler(this.cbAllHosts_CheckedChanged);
                this.cbAutoHide.CheckedChanged += new System.EventHandler(this.cbAutoHide_CheckedChanged);
                this.cbAutoStart.CheckedChanged += new System.EventHandler(this.cbAutoStart_CheckedChanged);
                this.cbService.CheckedChanged += new System.EventHandler(this.cbService_CheckedChanged);
                this.txtServicePort.TextChanged += new System.EventHandler(this.txtInterval_TextChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show("初始化失败!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.notifyIcon1.Visible = true;
            }
            else
            {
                this.notifyIcon1.Visible = false;
            }
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.Visible = false;
            this.notifyIcon1.Visible = true;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.notifyIcon1.Visible = false;
        }


        private void btnExist_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnAddHost_Click(object sender, EventArgs e)
        {
            using (HostForm hostForm = new HostForm())
            {
                HostForm.name = "";
                HostForm.addr = "";
                HostForm.port = "";
                HostForm.ntp = false;
                if (hostForm.ShowDialog() == DialogResult.OK)
                {
                    InsertHost(HostForm.name, HostForm.addr, HostForm.port, HostForm.ntp);
                }
            }
        }

        private void btnDelHost_Click(object sender, EventArgs e)
        {
            if (lstHosts.SelectedIndex > -1)
            {
                DelHost((lstHosts.SelectedIndex + 1).ToString());
            }
            lstHosts.SelectedIndex = -1;
        }

        private void btnEditHost_Click(object sender, EventArgs e)
        {
            if (lstHosts.SelectedIndex > -1)
            {
                int selected = lstHosts.SelectedIndex;
                using (HostForm hostForm = new HostForm())
                {
                    List<HostEntity> lsthosts = GetHosts();

                    HostForm.name = lsthosts[selected].name;
                    HostForm.addr = lsthosts[selected].addr;
                    HostForm.port = lsthosts[selected].port;
                    HostForm.ntp = lsthosts[selected].isntp;
                    if (hostForm.ShowDialog() == DialogResult.OK)
                    {
                        EditHost((selected + 1).ToString(), HostForm.name, HostForm.addr, HostForm.port, HostForm.ntp);
                    }
                }
                lstHosts.SelectedIndex = selected;
            }
        }

        private void btnUpHost_Click(object sender, EventArgs e)
        {
            if (lstHosts.SelectedIndex > 0) //第一条不能上移
            {
                int selected = lstHosts.SelectedIndex;
                List<HostEntity> lsthosts = GetHosts();
                HostEntity tmp= lsthosts[selected - 1];
                lsthosts[selected - 1] = lsthosts[selected];
                lsthosts[selected] = tmp;
                
                SaveHosts(lsthosts);

                InitHosts();
                lstHosts.SelectedIndex = selected-1;
            }
        }

        private void btnDownHost_Click(object sender, EventArgs e)
        {
            if (lstHosts.SelectedIndex>-1 && (lstHosts.SelectedIndex < lstHosts.Items.Count -1)) //最后一条不能下移
            {
                int selected = lstHosts.SelectedIndex;
                List<HostEntity> lsthosts = GetHosts();
                HostEntity tmp = lsthosts[selected + 1];
                lsthosts[selected + 1] = lsthosts[selected];
                lsthosts[selected] = tmp;

                SaveHosts(lsthosts);

                InitHosts();
                lstHosts.SelectedIndex = selected +1;
            }
        }
        
        #region 主机操作
        private void InitHosts()
        {
            lstHosts.Items.Clear();
            List<HostEntity> lsthosts = GetHosts();

            try {
                lstHosts.Items.Clear();
                if(lsthosts!=null)
                {
                    foreach(HostEntity host in lsthosts)
                    {
                        lstHosts.Items.Add(host.hostid + " " + host.name);
                    }
                }
            }
            catch(Exception ex)
            {
                ;
            }
        }
        private List<HostEntity> GetHosts()
        {
            List<HostEntity> lstHosts = new List<HostEntity>();
            try
            {
                if (File.Exists(HostsFilePath))
                {
                    using (StreamReader reader = new StreamReader(HostsFilePath, Encoding.UTF8))
                    {
                        do
                        {
                            //名称    地址  端口号
                            string strrow = reader.ReadLine();
                            if (!string.IsNullOrEmpty(strrow))
                            {
                                string[] strcols = strrow.Split('\t');
                                if (strcols != null && strcols.Length == 5)
                                {
                                    HostEntity host = new HostEntity();
                                    host.hostid = strcols[0];
                                    host.name = strcols[1];
                                    host.addr = strcols[2];
                                    host.port = strcols[3];
                                    host.isntp = strcols[4]=="NTP"? true :false;
                                    lstHosts.Add(host);
                                }
                            }
                        } while (!reader.EndOfStream);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("初始化失败!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return lstHosts;
        }

        private void SaveHosts(List<HostEntity> lsthosts)
        {
            try
            {
                if (lsthosts == null)
                    return;
                using (StreamWriter writer = new StreamWriter(HostsFilePath, false, Encoding.UTF8))
                {
                    for(int i=0; i < lsthosts.Count;i++)
                    {
                        writer.WriteLine((i + 1) + "\t" + lsthosts[i].name + "\t" + lsthosts[i].addr + "\t" + lsthosts[i].port + "\t" + (lsthosts[i].isntp ? "NTP" : "UDP"));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("初始化失败!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertHost(string name,string addr,string port,bool ntp)
        {
            try
            {
                List<HostEntity> lsthosts = GetHosts();
                HostEntity host = new HostEntity();
                host.name = name;
                host.addr = addr;
                host.port = port;
                host.isntp = ntp;
                lsthosts.Insert(0, host);
                SaveHosts(lsthosts);
            }
            catch(Exception ex)
            {
                MessageBox.Show("初始化失败!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            InitHosts();
        }

        private void DelHost(string id)
        {
            try
            {
                List<HostEntity> lsthosts = GetHosts();
                for(int i=0;i<lsthosts.Count;i++)
                {
                    if(lsthosts[i].hostid == id)
                    {
                        lsthosts.RemoveAt(i);
                    }
                }
                SaveHosts(lsthosts);
            }
            catch (Exception ex)
            {
                MessageBox.Show("初始化失败!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            InitHosts();
        }

        private void EditHost(string id, string name, string addr, string port,bool ntp)
        {
            try
            {
                List<HostEntity> lsthosts = GetHosts();
                for (int i = 0; i < lsthosts.Count; i++)
                {
                    if (lsthosts[i].hostid == id)
                    {
                        lsthosts[i].name = name;
                        lsthosts[i].addr = addr;
                        lsthosts[i].port = port;
                        lsthosts[i].isntp = ntp;
                    }
                }
                SaveHosts(lsthosts);
            }
            catch (Exception ex)
            {
                MessageBox.Show("初始化失败!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            InitHosts();
        }

        #endregion

        #region 选项
        private void cbAutoTiming_CheckedChanged(object sender, EventArgs e)
        {
            if(!Regex.IsMatch(txtInterval.Text,@"^\d{1,4}$"))
            {
                MessageBox.Show("请输入正确时间间隔!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtInterval.Focus();
                return;
            }
            
            config.AppSettings.Settings["AutoTimingEnable"].Value = cbAutoTiming.Checked.ToString();
            config.AppSettings.Settings["AutoTimingInterval"].Value = txtInterval.Text;

            //save to apply changes
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
            
            if(cbAutoTiming.Checked)
            {
                timer_interval.Interval = Convert.ToInt32(txtInterval.Text) * 60 * 1000;
                timer_interval.Enabled = true;
            }
            else
            {
                timer_interval.Enabled = false;
            }
            //enable timer
        }

        private void txtInterval_TextChanged(object sender, EventArgs e)
        {
            if (cbAutoTiming.Checked && Regex.IsMatch(txtInterval.Text, @"^\d{1,4}$"))
            {
                timer_interval.Interval = Convert.ToInt32(txtInterval.Text) * 60 * 1000;
                timer_interval.Enabled = true;
            }
            else
            {
                timer_interval.Enabled = false;
            }
        }

        private void cbAllHosts_CheckedChanged(object sender, EventArgs e)
        {
            config.AppSettings.Settings["TryAllHosts"].Value = cbAllHosts.Checked.ToString();

            //save to apply changes
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void cbAutoHide_CheckedChanged(object sender, EventArgs e)
        {
            config.AppSettings.Settings["AutoHide"].Value = cbAutoHide.Checked.ToString();

            //save to apply changes
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void cbAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAutoStart.Checked) //设置开机自启动  
            {
                MessageBox.Show("设置开机自启动，需要修改注册表", "系统提示");
                string path = Application.ExecutablePath;
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                rk2.SetValue("AboutTime", path);
                rk2.Close();
                rk.Close();

                config.AppSettings.Settings["AutoStart"].Value = cbAutoStart.Checked.ToString();
            }
            else //取消开机自启动  
            {
                MessageBox.Show("取消开机自启动，需要修改注册表", "系统提示");
                string path = Application.ExecutablePath;
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                rk2.DeleteValue("AboutTime", false);
                rk2.Close();
                rk.Close();

                config.AppSettings.Settings["AutoStart"].Value = cbAutoStart.Checked.ToString();
            }

            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void cbService_CheckedChanged(object sender, EventArgs e)
        {
            if(cbService.Checked)
            {
                if (!Regex.IsMatch(txtServiceIP.Text, @"^(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])$"))
                {
                    MessageBox.Show("请输入正确服务IP!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtServiceIP.Focus();
                    this.cbService.CheckedChanged -= new System.EventHandler(this.cbService_CheckedChanged);
                    cbService.Checked = false;
                    this.cbService.CheckedChanged += new System.EventHandler(this.cbService_CheckedChanged);
                    return;
                }
                if (!Regex.IsMatch(txtServicePort.Text, @"^\d{1,5}$"))
                {
                    MessageBox.Show("请输入正确服务端口号!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtServicePort.Focus();
                    this.cbService.CheckedChanged -= new System.EventHandler(this.cbService_CheckedChanged);
                    cbService.Checked = false;
                    this.cbService.CheckedChanged += new System.EventHandler(this.cbService_CheckedChanged);
                    return;
                }
                config.AppSettings.Settings["ServiceIP"].Value = txtServiceIP.Text;
                config.AppSettings.Settings["ServicePort"].Value = txtServicePort.Text;
            }
            config.AppSettings.Settings["Service"].Value = cbService.Checked.ToString();
            //save to apply changes
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");

            MessageBox.Show("重启程序后生效!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region 设置时间
        [StructLayoutAttribute(LayoutKind.Sequential)]
        private struct SYSTEMTIME
        {
            public short year;
            public short month;
            public short dayOfWeek;
            public short day;
            public short hour;
            public short minute;
            public short second;
            public short milliseconds;
        }

        [DllImport("kernel32.dll")]
        static extern bool SetLocalTime(ref SYSTEMTIME time);

        // Set system time according to transmit timestamp
        private void SetTime(DateTime dt)
        {
            SYSTEMTIME st;

            DateTime trts = dt;
            st.year = (short)trts.Year;
            st.month = (short)trts.Month;
            st.dayOfWeek = (short)trts.DayOfWeek;
            st.day = (short)trts.Day;
            st.hour = (short)trts.Hour;
            st.minute = (short)trts.Minute;
            st.second = (short)trts.Second;
            st.milliseconds = (short)trts.Millisecond;

            SetLocalTime(ref st);
        }
        

        private bool CorrectTime(string addr,int port,bool isntp)
        {
            TimeUDPClient client = new TimeUDPClient();
            try
            {
                SetLog("地址:" + addr);
                IPAddress ipAddr;
                //如果地址符合IP地址规范，当做IP地址使用
                if (Regex.IsMatch(addr, @"^(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])$"))
                {
                    IPAddress.TryParse(addr, out ipAddr);
                }
                else
                {
                    IPHostEntry hostadd = Dns.GetHostEntry(addr);
                    ipAddr = hostadd.AddressList[0];
                }

                SetLog("IP:" + ipAddr + ",Port:" + port);
                IPEndPoint EPhost = new IPEndPoint(ipAddr, port);

                client.NetWork = new UdpClient();
                client.NetWork.Client.SendTimeout = 1500;
                client.NetWork.Client.ReceiveTimeout = 1500;
                client.NetWork.Connect(EPhost);

                Common.NTP ntp = new Common.NTP();
                //if (isntp)
                //{
                    ntp.Initialize();
                    client.NetWork.Send(ntp.NTPData, ntp.NTPData.Length);
                //}
                IPEndPoint ep = null;
                byte[] recdata = client.NetWork.Receive(ref ep);
                int reclen = recdata.Length;
                if (reclen > 0)
                {
                    SetLog("收到数据,长度:" + recdata.Length);
                    if (isntp)
                    {
                        ntp.NTPData = recdata;
                        if (!ntp.IsResponseValid())
                        {
                            SetLog("无效响应");
                            return false;
                        }
                        ntp.ReceptionTimestamp = DateTime.Now;
                        if (Math.Abs((ntp.TransmitTimestamp - DateTime.Now).TotalDays) >= 3)
                        {
                            SetLog("系统时间与服务获取时间相差超过3天,停止校时!");
                            return false;
                        }
                        else {
                            SetLog("设置时间:" + ntp.TransmitTimestamp.ToString());
                            SetTime(ntp.TransmitTimestamp);
                        }
                        return true;
                    }
                    else
                    {
                        long ticks = BitConverter.ToInt64(recdata, 0);
                        DateTime dt = new DateTime(ticks);
                        //TimeSpan offspan = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
                        //dt = dt + offspan;
                        if (Math.Abs((dt - DateTime.Now).TotalDays) >= 3)
                        {
                            SetLog("系统时间与服务获取时间相差超过3天,停止校时!");
                            return false;
                        }
                        else {
                            SetLog("设置时间:" + dt.ToString());
                            SetTime(dt);
                        }
                        return true;
                    }
                }
                else
                {
                    SetLog("获取时间失败,未获得数据");
                    return false;
                }
            }
            catch(Exception ex)
            {
                SetLog("校时失败,ex:" + ex.Message);
                client.DisConnect();
                return false;
            }
        }
        
        private void btnSetTime_Click(object sender, EventArgs e)
        {
            lstLog.Items.Clear();
            Thread t_time = new Thread(new ThreadStart(CorrectTimeThread));
            t_time.IsBackground = true;
            t_time.Start();
        }

        private void CorrectTimeThread()
        {
            List<HostEntity> lsthosts = GetHosts();
            if (lsthosts == null || lsthosts.Count == 0)
            {
                SetLog("未设置主机");
                return;
            }
            if (Convert.ToBoolean(config.AppSettings.Settings["TryAllHosts"].Value)) //是否测试所有的主机
            {
                int i = 0;
                bool Iscorrect = false;
                do
                {
                    SetLog(DateTime.Now.ToString() + " " + "连接到" + lsthosts[i].name + (lsthosts[i].isntp ? " NTP" : " UDP"));
                    Iscorrect = CorrectTime(lsthosts[i].addr, Convert.ToInt32(lsthosts[i].port),lsthosts[i].isntp);
                    i++;
                }
                while (!Iscorrect && i <= lsthosts.Count - 1);
            }
            else
            {
                SetLog(DateTime.Now.ToString() + " " + "连接到" + lsthosts[0].name + (lsthosts[0].isntp ? " NTP" : " UDP"));
                CorrectTime(lsthosts[0].addr, Convert.ToInt32(lsthosts[0].port), lsthosts[0].isntp);
            }
        }

        private delegate void LogHandle(string msg);
        private void SetLog(string msg)
        {
            if (this.lstLog.InvokeRequired)
            {
                this.lstLog.Invoke(new LogHandle(OnSetLog),msg);
            }
            else
                OnSetLog(msg);
        }

        private void OnSetLog(string msg)
        {
            this.lstLog.Items.Add(msg);
        }

        #endregion

        private void timer_interval_Tick(object sender, EventArgs e)
        {
            lstLog.Items.Clear();
            Thread t_time = new Thread(new ThreadStart(CorrectTimeThread));
            t_time.IsBackground = true;
            t_time.Start();
        }

        private void TimeService()
        {
            Thread t_service = new Thread(new ThreadStart(TimeServiceThread));
            t_service.IsBackground = true;
            t_service.Start();
        }
        private void TimeServiceThread()
        {
            try
            {
                IPAddress ipAddr;
                IPAddress.TryParse(config.AppSettings.Settings["ServiceIP"].Value, out ipAddr);
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, Convert.ToInt32(config.AppSettings.Settings["ServicePort"].Value));
                UdpClient listener = new UdpClient(ipEndPoint);

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                while (true)
                {
                    listener.Receive(ref RemoteIpEndPoint);
                    byte[] datas = BitConverter.GetBytes(DateTime.Now.Ticks);

                    //byte[] datas = Encoding.ASCII.GetBytes(DateTime.UtcNow.ToString("yy-MM-dd HH:mm:ss") + " UTC");
                    listener.Send(datas, datas.Length, RemoteIpEndPoint);
                    //using (NetworkStream clientStream = client.GetStream())
                    //{
                    //    byte[] datas = Encoding.ASCII.GetBytes(DateTime.UtcNow.ToString("yy-MM-dd HH:mm:ss")+ " UTC");
                    //    clientStream.Write(datas, 0, datas.Length);
                    //    clientStream.Flush();
                    //}
                }
            }
            catch (Exception ex)
            {
                SetLog("时间服务错误,ex:" + ex.Message);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(DialogResult.No == MessageBox.Show("是否退出?","系统提示",MessageBoxButtons.YesNo,MessageBoxIcon.Asterisk))
            {
                e.Cancel=true;
            }
        }
    }
}
