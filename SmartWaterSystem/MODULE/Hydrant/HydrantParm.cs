using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Text.RegularExpressions;
using DevExpress.XtraGrid.Views.Grid;
using Entity;
using System.Collections;
using BLL;
using Common;
using System.Collections.Generic;

namespace SmartWaterSystem
{
    public partial class HydrantParm : BaseView, IHydrantParm
    {

        DataTable HistoryData = new DataTable("HistoryTable");
        public HydrantParm()
        {
            InitializeComponent();

            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
            //Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void HydrantParm_Load(object sender, EventArgs e)
        {
            GlobalValue.Hydrantlog.serialPortUtil.ReadHisData += new Protocol.ReadHistoryDataHandle(serialPortUtil_ReadHisData);
            InitGridView();
        }

        void serialPortUtil_ReadHisData(Protocol.HistoryValueArgs e)
        {
            try
            {
                if (e != null)
                {
                    DataRow dr = HistoryData.NewRow();
                    dr["CollectTime"] = e.CollectTime.ToString();
                    dr["PreValue"] = e.PreValue.ToString();
                    dr["OpenAngle"] = e.OpenAngle.ToString();
                    HistoryData.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("接受历史数据发生错误" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitGridView()
        {
            HistoryData.Columns.Add("CollectTime");  //采集时间
            HistoryData.Columns.Add("PreValue");   //压力值
            HistoryData.Columns.Add("OpenAngle");   //开度
            gridControl_History.DataSource = HistoryData;
        }
        
        #region txt Event
        /// <summary>
        /// 限制最大1byte (<=255)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void onebyte_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextEdit txtbox = (TextEdit)sender;
            if (e.KeyChar == '\b') //backspace
            {
                e.Handled = false;
            }
            else if (e.KeyChar == ' ' || e.KeyChar == '\r' || e.KeyChar == '.')
            {
                e.Handled = false;
            }
            else if ((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar >= 65 && e.KeyChar <= 70) || (e.KeyChar >= 97 && e.KeyChar <= 102) || e.KeyChar == 120)
            {
                string text = txtbox.Text;
                if (txtbox.SelectedText.Length > 0)
                {
                    text =text.Remove(txtbox.SelectionStart, txtbox.SelectionLength);
                    
                }
                text = text.Insert(txtbox.SelectionStart, e.KeyChar.ToString());

                if (Regex.IsMatch(text, @"^(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])$")
            || Regex.IsMatch(text, @"^(0x|0x[0-9a-fA-F]{1,2})$"))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            else
                e.Handled = true;
        }

        /// <summary>
        /// 限制最大2byte (<=65535)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void twobyte_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextEdit txtbox = (TextEdit)sender;
            if (e.KeyChar == '\b') //backspace
            {
                e.Handled = false;
            }
            else if (e.KeyChar == ' ' || e.KeyChar == '\r' || e.KeyChar == '.')
            {
                e.Handled = false;
            }
            else if ((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar >= 65 && e.KeyChar <= 70) || (e.KeyChar >= 97 && e.KeyChar <= 102) || e.KeyChar==120)
            {
                string text = txtbox.Text;
                if (txtbox.SelectedText.Length > 0)
                {
                    text = text.Remove(txtbox.SelectionStart, txtbox.SelectionLength);
                    text = text.Insert(txtbox.SelectionStart, e.KeyChar.ToString());
                }
                text = text.Insert(txtbox.SelectionStart, e.KeyChar.ToString());
                if ((Regex.IsMatch(text, @"^\d{1,5}$") && Convert.ToInt32(text) <= 65535)
                || Regex.IsMatch(text, @"^(0x|0x[0-9a-fA-F]{1,4})$"))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            else
                e.Handled = true;
        }
        #endregion

        private void gridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator)
            {
                e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        #region button Events
        private void btnReset_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                XtraMessageBox.Show("请先填写终端ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return;
            }
            if (DialogResult.No == XtraMessageBox.Show("确定复位?", GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
            {
                return;
            }
            GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity();
            GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

            EnableControls(false);
            DisableRibbonBar();
            DisableNavigateBar();
            ShowWaitForm("", "正在复位消防栓...");
            BeginSerialPortDelegate();
            Application.DoEvents();
            SetStaticItem("正在复位消防栓...");
            GlobalValue.SerialPortMgr.Send(SerialPortType.HydrantReset);
        }

        private void btnCheckingTime_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                XtraMessageBox.Show("请先填写消防栓ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return;
            }
            GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity();
            GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

            EnableControls(false);
            DisableRibbonBar();
            DisableNavigateBar();
            ShowWaitForm("", "正在设置时间...");
            BeginSerialPortDelegate();
            Application.DoEvents();
            SetStaticItem("正在设置时间...");
            GlobalValue.SerialPortMgr.Send(SerialPortType.HydrantSetTime);
        }

        private void btnReadParm_Click(object sender, EventArgs e)
        {
            GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity();
            bool haveread = false;
            if (ceID.Checked)
            {
                GlobalValue.UniSerialPortOptData.IsOptID = ceID.Checked;
                haveread = true;
            }
            else
            {
                if (!Regex.IsMatch(txtID.Text, @"^\d{1,5}$"))
                {
                    XtraMessageBox.Show("请输入消防栓ID", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtID.Focus();
                    return;
                }
                GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

                if (ceTime.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptDT = ceTime.Checked;
                    haveread = true;
                }
                if (ceNumofturns.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_Numofturns = ceNumofturns.Checked;
                    haveread = true;
                }
                if(cePreRealTime.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_RealTimeData = cePreRealTime.Checked;
                    haveread = true;
                }
                if(ceComTime.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_ComTime = ceComTime.Checked;
                    haveread = true;
                }
                if (ceIP.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptIP = ceIP.Checked;
                    haveread = true;
                }
                if (cePort.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptPort = cePort.Checked;
                    haveread = true;
                }
                if (ceEnable.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_HydrantEnable = ceEnable.Checked;
                    haveread = true;
                }
                if(cePreRange.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_Range = cePreRange.Checked;
                    haveread = true;
                }
                if(ceOffset.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_PreOffset = ceOffset.Checked;
                    haveread = true;
                }
                if (cePreConfig.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_PreConfig = cePreConfig.Checked;
                    haveread = true;
                }
            }

            if (!haveread)
            {
                XtraMessageBox.Show("请选择需要读取的参数!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            EnableControls(false);
            DisableRibbonBar();
            DisableNavigateBar();
            ShowWaitForm("", "正在读取...");
            BeginSerialPortDelegate();
            GlobalValue.SerialPortMgr.SerialPortScheduleEvent -= new SerialPortScheduleHandle(SerialPortParm_SerialPortScheduleEvent);
            GlobalValue.SerialPortMgr.SerialPortScheduleEvent += new SerialPortScheduleHandle(SerialPortParm_SerialPortScheduleEvent);
            Application.DoEvents();
            SetStaticItem("正在读取...");
            GlobalValue.SerialPortMgr.Send(SerialPortType.HydrantReadBasicInfo);
        }

        void SerialPortParm_SerialPortScheduleEvent(object sender, SerialPortScheduleEventArgs e)
        {
            if ((e.OptType == SerialPortType.UniversalReadBasicInfo || e.OptType == SerialPortType.UniversalSetBasicInfo) && !string.IsNullOrEmpty(e.Msg))
            {
                ShowWaitForm("", e.Msg);
                SetStaticItem(e.Msg);
            }
        }

        private void btnSetParm_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity();
                bool haveset = false;
                if (ceID.Checked)
                {
                    if (DialogResult.No == XtraMessageBox.Show("设置消防栓编号会初始化设备参数,是否继续?", GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                    {
                        return;
                    }
                    GlobalValue.UniSerialPortOptData.IsOptID = ceID.Checked;
                    GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);
                    haveset = true;
                }
                else
                {
                    GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);
                    if(ceComTime.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOpt_ComTime = ceComTime.Checked;
                        GlobalValue.UniSerialPortOptData.ComTime = (int)(seComTime.Value);
                        haveset = true;
                    }
                    if (ceIP.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptIP = ceIP.Checked;
                        GlobalValue.UniSerialPortOptData.IP = new int[4];
                        string[] ips = txtIP.Text.Split('.');
                        if (ips != null && ips.Length == 4)
                        {
                            GlobalValue.UniSerialPortOptData.IP[0] = Convert.ToInt32(ips[0]);
                            GlobalValue.UniSerialPortOptData.IP[1] = Convert.ToInt32(ips[1]);
                            GlobalValue.UniSerialPortOptData.IP[2] = Convert.ToInt32(ips[2]);
                            GlobalValue.UniSerialPortOptData.IP[3] = Convert.ToInt32(ips[3]);
                        }
                        haveset = true;
                    }
                    if (cePort.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptPort = cePort.Checked;
                        GlobalValue.UniSerialPortOptData.Port = Convert.ToInt32(txtPort.Text);
                        haveset = true;
                    }
                    if (ceEnable.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOpt_HydrantEnable = ceEnable.Checked;
                        GlobalValue.UniSerialPortOptData.HydrantEnable = SwitchEnable.IsOn;
                        haveset = true;
                    }
                    if(cePreRange.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOpt_Range = cePreRange.Checked;
                        GlobalValue.UniSerialPortOptData.Range = Convert.ToDouble(txtPreRange.Text);
                        haveset = true;
                    }
                    if(ceOffset.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOpt_PreOffset = ceOffset.Checked;
                        GlobalValue.UniSerialPortOptData.PreOffset = Convert.ToDouble(txtOffset.Text);
                        haveset = true;
                    }
                    if (cePreConfig.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOpt_PreConfig = cePreConfig.Checked;
                        GlobalValue.UniSerialPortOptData.PreConfig = cbPreConfig.SelectedIndex == 0 ? false : true;  
                        haveset = true;
                    }
                    
                }

                if (!haveset)
                {
                    XtraMessageBox.Show("请选择需要设置的参数!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                EnableControls(false);
                DisableRibbonBar();
                DisableNavigateBar();
                ShowWaitForm("", "正在设置...");
                BeginSerialPortDelegate();
                GlobalValue.SerialPortMgr.SerialPortScheduleEvent -= new SerialPortScheduleHandle(SerialPortParm_SerialPortScheduleEvent);
                GlobalValue.SerialPortMgr.SerialPortScheduleEvent += new SerialPortScheduleHandle(SerialPortParm_SerialPortScheduleEvent);
                Application.DoEvents();
                SetStaticItem("正在设置...");
                GlobalValue.SerialPortMgr.Send(SerialPortType.HydrantSetBasicInfo);
            }
        }

        private void btnReadHistory_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity();
                if (ceID.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptID = ceID.Checked;
                    GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);
                }
                else
                {
                    GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);
                    HistoryData.Clear();
                    gridView_History.Columns["PreValue"].Visible = false;
                    gridView_History.Columns["OpenAngle"].Visible = false;
                    if (rgOpt.SelectedIndex == 0)
                    {
                        GlobalValue.UniSerialPortOptData.HydrantHistoryOpt = HydrantOptType.Open;
                        gridView_History.Columns["PreValue"].Visible = true;
                        gridView_History.Columns["OpenAngle"].Visible = true;
                    }
                    else if (rgOpt.SelectedIndex == 1)
                        GlobalValue.UniSerialPortOptData.HydrantHistoryOpt = HydrantOptType.Close;
                    else if (rgOpt.SelectedIndex == 2)
                    {
                        GlobalValue.UniSerialPortOptData.HydrantHistoryOpt = HydrantOptType.OpenAngle;
                        gridView_History.Columns["OpenAngle"].Visible = true;
                    }
                    else if (rgOpt.SelectedIndex == 3)
                        GlobalValue.UniSerialPortOptData.HydrantHistoryOpt = HydrantOptType.Impact;
                    else if (rgOpt.SelectedIndex == 4)
                        GlobalValue.UniSerialPortOptData.HydrantHistoryOpt = HydrantOptType.KnockOver;
                }

                EnableControls(false);
                DisableRibbonBar();
                DisableNavigateBar();
                ShowWaitForm("", "正在读取历史数据...");
                BeginSerialPortDelegate();
                GlobalValue.SerialPortMgr.SerialPortScheduleEvent -= new SerialPortScheduleHandle(SerialPortParm_SerialPortScheduleEvent);
                GlobalValue.SerialPortMgr.SerialPortScheduleEvent += new SerialPortScheduleHandle(SerialPortParm_SerialPortScheduleEvent);
                Application.DoEvents();
                SetStaticItem("正在读取历史数据...");
                GlobalValue.SerialPortMgr.Send(SerialPortType.HydrantReadHistory);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
                saveFileDialog.DefaultExt = "xls";
                saveFileDialog.Filter = "Excel文件|*.xls";
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    gridView_History.ExportToXls(saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("导出数据发生异常!" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnEnableCollect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                XtraMessageBox.Show("请先填写终端ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return;
            }
            GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity();
            GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

            EnableControls(false);
            DisableRibbonBar();
            DisableNavigateBar();
            ShowWaitForm("", "正在启动采集...");
            BeginSerialPortDelegate();
            Application.DoEvents();
            SetStaticItem("正在启动采集...");
            GlobalValue.SerialPortMgr.Send(SerialPortType.HydrantSetEnableCollect);
        }

        public override void OnSerialPortNotify(object sender, SerialPortEventArgs e)
        {
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.HydrantReset)
            {
                this.Enabled = true;
                HideWaitForm();

                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);
                if (e.TransactStatus == TransStatus.Success)
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();

                    XtraMessageBox.Show("复位成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    XtraMessageBox.Show("复位失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.HydrantSetTime)
            {
                this.Enabled = true;
                HideWaitForm();

                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);
                if (e.TransactStatus == TransStatus.Success)
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();

                    XtraMessageBox.Show("设置时间成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    XtraMessageBox.Show("设置时间失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.HydrantReadBasicInfo)
            {
                this.Enabled = true;
                HideWaitForm();

                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);
                if (e.TransactStatus == TransStatus.Success)
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();

                    if (GlobalValue.UniSerialPortOptData.IsOptID)
                    {
                        txtID.Text = GlobalValue.UniSerialPortOptData.ID.ToString();
                    }
                    if (GlobalValue.UniSerialPortOptData.IsOptDT)
                    {
                        txtTime.Text = GlobalValue.UniSerialPortOptData.DT.ToString();
                    }
                    if (GlobalValue.UniSerialPortOptData.IsOpt_Numofturns)
                    {
                        txtNumofturns.Text = GlobalValue.UniSerialPortOptData.Numofturns.ToString("f0");
                    }
                    if(GlobalValue.UniSerialPortOptData.IsOpt_RealTimeData)
                    {
                        txtPreRealTime.Text = GlobalValue.UniSerialPortOptData.RealTimeData.ToString();
                    }
                    if(GlobalValue.UniSerialPortOptData.IsOpt_ComTime)
                    {
                        seComTime.Value = GlobalValue.UniSerialPortOptData.ComTime;
                    }
                    if (GlobalValue.UniSerialPortOptData.IsOptIP)
                    {
                        txtIP.Text = GlobalValue.UniSerialPortOptData.IP[0].ToString() + "." + GlobalValue.UniSerialPortOptData.IP[1].ToString() + "." +
                            GlobalValue.UniSerialPortOptData.IP[2].ToString() + "." + GlobalValue.UniSerialPortOptData.IP[3].ToString();
                    }
                    if (GlobalValue.UniSerialPortOptData.IsOptPort)
                    {
                        txtPort.Text = GlobalValue.UniSerialPortOptData.Port.ToString();
                    }
                    if (GlobalValue.UniSerialPortOptData.IsOpt_HydrantEnable)
                    {
                        SwitchEnable.IsOn = GlobalValue.UniSerialPortOptData.HydrantEnable;
                    }
                    if(GlobalValue.UniSerialPortOptData.IsOpt_Range)
                    {
                        txtPreRange.Text = GlobalValue.UniSerialPortOptData.Range.ToString();
                    }
                    if(GlobalValue.UniSerialPortOptData.IsOpt_PreOffset)
                    {
                        txtOffset.Text = GlobalValue.UniSerialPortOptData.PreOffset.ToString();
                    }
                    if (GlobalValue.UniSerialPortOptData.IsOpt_PreConfig)
                    {
                        if (GlobalValue.UniSerialPortOptData.PreConfig)
                            cbPreConfig.SelectedIndex = 1;
                        else
                            cbPreConfig.SelectedIndex = 0;
                    }
                    
                    
                    if (GlobalValue.UniSerialPortOptData.IsOpt_SimualteInterval)
                    {
                        gridControl_History.DataSource=GlobalValue.UniSerialPortOptData.Simulate_Interval;
                    }
                    

                    XtraMessageBox.Show("读取消防栓参数成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    XtraMessageBox.Show("读取消防栓参数失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.HydrantSetBasicInfo)
            {
                this.Enabled = true;
                HideWaitForm();

                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);
                if (e.TransactStatus == TransStatus.Success)
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();

                    XtraMessageBox.Show("设置消防栓参数成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    XtraMessageBox.Show("设置消防栓参数失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.HydrantReadHistory)
            {
                this.Enabled = true;
                HideWaitForm();

                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);
                if (e.TransactStatus == TransStatus.Success)
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();

                    XtraMessageBox.Show("读取消防栓历史数据成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    XtraMessageBox.Show("读取消防栓历史数据失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }

        private void EnableControls(bool enable)
        {
            btnReset.Enabled = enable;
            btnCheckingTime.Enabled = enable;
            btnReadParm.Enabled = enable;
            btnSetParm.Enabled = enable;
            btnReadHistory.Enabled = enable;
        }

        private bool Validate()
        {
            if (!Regex.IsMatch(txtID.Text,@"^\d{1,5}$"))
            {
                XtraMessageBox.Show("请输入设备ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return false;
            }

            if (cePreConfig.Checked && !(cbPreConfig.SelectedIndex>-1))
            {
                XtraMessageBox.Show("请选择压力配置!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbPreConfig.Focus();
                return false;
            }

            if (ceIP.Checked)
            {
                if (string.IsNullOrEmpty(txtIP.Text))
                {
                    XtraMessageBox.Show("请填写完整的IP地址!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtIP.Focus();
                    return false;
                }
            }

            if (cePort.Checked && !Regex.IsMatch(txtPort.Text,@"^\d{3,4}$"))
            {
                XtraMessageBox.Show("请输入端口号!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPort.Focus();
                return false;
            }

            return true;
        }
        #endregion

        public override void SerialPortEvent(bool Enabled)
        {
            btnReset.Enabled = Enabled;
            btnCheckingTime.Enabled = Enabled;
            btnReadParm.Enabled = Enabled;
            btnSetParm.Enabled = Enabled;
            btnReadHistory.Enabled = Enabled;
        }

        private void SetSerialPortCtrlStatus()
        {
            btnReset.Enabled = GlobalValue.portUtil.IsOpen;
            btnCheckingTime.Enabled = GlobalValue.portUtil.IsOpen;
            btnReadParm.Enabled = GlobalValue.portUtil.IsOpen;
            btnSetParm.Enabled = GlobalValue.portUtil.IsOpen;
            btnReadHistory.Enabled = GlobalValue.portUtil.IsOpen;

            ceTime.Enabled = true;
            txtTime.Enabled = true;
            txtTime.Text = "";
            txtNumofturns.Enabled = true;
            txtNumofturns.Text = "";
            cbPreConfig.Enabled = true;
            cbPreConfig.SelectedIndex = -1;
            ceIP.Enabled = true;
            txtIP.Enabled = true;
            txtIP.Text = "";
            cePort.Enabled = true;
            txtPort.Enabled = true;
            txtPort.Text = "";
        }

        private void ceIP_CheckedChanged(object sender, EventArgs e)
        {
            if (!ceIP.Checked)
            {
                txtIP.Text = "";
            }
        }

        private void cePort_CheckedChanged(object sender, EventArgs e)
        {
            if (!cePort.Checked)
            {
                txtPort.Text = "";
            }
        }



    }
}
