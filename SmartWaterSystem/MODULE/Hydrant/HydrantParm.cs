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
        }

        private void HydrantParm_Load(object sender, EventArgs e)
        {
            GlobalValue.Hydrantlog.serialPortUtil.ReadHisData += new Protocol.ReadHistoryDataHandle(serialPortUtil_ReadHisData);
            InitGridView();

            cbComType.SelectedIndex = 1;
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

        #region IP
        private void txtNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextEdit txtbox = (TextEdit)sender;
            int index = Convert.ToInt32(txtbox.Tag);
            if (e.KeyChar == '\b') //backspace
            {
                if (txtbox.Text.Length == 0)
                {
                    if (2 == index)
                    {
                        txtNum1.Focus();
                        txtNum1.SelectionStart = txtNum1.Text.Length;
                    }
                    else if (3 == index)
                    {
                        txtNum2.Focus();
                        txtNum2.SelectionStart = txtNum2.Text.Length;
                    }
                    else if (4 == index)
                    {
                        txtNum3.Focus();
                        txtNum3.SelectionStart = txtNum3.Text.Length;
                    }
                }
            }
            else if (e.KeyChar == ' ' || e.KeyChar == '\r' || e.KeyChar == '.')
            {
                if (1 == index)
                {
                    txtNum2.SelectAll();
                    txtNum2.Focus();
                }
                else if (2 == index)
                {
                    txtNum3.SelectAll();
                    txtNum3.Focus();
                }
                else if (3 == index)
                {
                    txtNum4.SelectAll();
                    txtNum4.Focus();
                }
                e.Handled = true;
            }
            else if (e.KeyChar >= 48 && e.KeyChar <= 57)
            {
                if (txtbox.Text.Length == 3)
                {
                    if (1 == index)
                    {
                        txtNum2.Text = e.KeyChar.ToString();
                        txtNum2.Focus();
                        txtNum2.SelectionStart = txtNum2.Text.Length;
                    }
                    else if (2 == index)
                    {
                        txtNum3.Text = e.KeyChar.ToString();
                        txtNum3.Focus();
                        txtNum3.SelectionStart = txtNum3.Text.Length;
                    }
                    else if (3 == index)
                    {
                        txtNum4.Text = e.KeyChar.ToString();
                        txtNum4.Focus();
                        txtNum4.SelectionStart = txtNum4.Text.Length;
                    }
                }
                else
                {
                    if (!Regex.IsMatch(txtbox.Text + e.KeyChar.ToString(), @"^(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])$"))
                    {
                        e.Handled = true;
                    }
                }
            }
            else
                e.Handled = true;
        }
        #endregion

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
            GlobalValue.SerialPortOptData = new UniversalSerialPortOptEntity();
            GlobalValue.SerialPortOptData.ID = Convert.ToInt16(txtID.Text);

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
            GlobalValue.SerialPortOptData = new UniversalSerialPortOptEntity();
            GlobalValue.SerialPortOptData.ID = Convert.ToInt16(txtID.Text);

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
            GlobalValue.SerialPortOptData = new UniversalSerialPortOptEntity();
            bool haveread = false;
            if (ceID.Checked)
            {
                GlobalValue.SerialPortOptData.IsOptID = ceID.Checked;
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
                GlobalValue.SerialPortOptData.ID = Convert.ToInt16(txtID.Text);

                if (ceTime.Checked)
                {
                    GlobalValue.SerialPortOptData.IsOptDT = ceTime.Checked;
                    haveread = true;
                }
                if (ceCellPhone.Checked)
                {
                    GlobalValue.SerialPortOptData.IsOptCellPhone = ceCellPhone.Checked;
                    haveread = true;
                }
                if (ceComType.Checked)
                {
                    GlobalValue.SerialPortOptData.IsOptComType = ceComType.Checked;
                    haveread = true;
                }
                if (ceIP.Checked)
                {
                    GlobalValue.SerialPortOptData.IsOptIP = ceIP.Checked;
                    haveread = true;
                }
                if (cePort.Checked)
                {
                    GlobalValue.SerialPortOptData.IsOptPort = cePort.Checked;
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
            if ((e.OptType == SerialPortType.UniversalReadBaicInfo || e.OptType == SerialPortType.UniversalSetBasicInfo) && !string.IsNullOrEmpty(e.Msg))
            {
                ShowWaitForm("", e.Msg);
                SetStaticItem(e.Msg);
            }
        }

        private void btnSetParm_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                GlobalValue.SerialPortOptData = new UniversalSerialPortOptEntity();
                bool haveset = false;
                if (ceID.Checked)
                {
                    if (DialogResult.No == XtraMessageBox.Show("设置消防栓编号会初始化设备参数,是否继续?", GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                    {
                        return;
                    }
                    GlobalValue.SerialPortOptData.IsOptID = ceID.Checked;
                    GlobalValue.SerialPortOptData.ID = Convert.ToInt16(txtID.Text);
                    haveset = true;
                }
                else
                {
                    GlobalValue.SerialPortOptData.ID = Convert.ToInt16(txtID.Text);

                    if (ceCellPhone.Checked)
                    {
                        GlobalValue.SerialPortOptData.IsOptCellPhone = ceCellPhone.Checked;
                        GlobalValue.SerialPortOptData.CellPhone = txtCellPhone.Text;
                        haveset = true;
                    }
                    if (ceComType.Checked)
                    {
                        GlobalValue.SerialPortOptData.IsOptComType = ceComType.Checked;
                        GlobalValue.SerialPortOptData.ComType = cbComType.SelectedIndex;
                        haveset = true;
                    }
                    if (ceIP.Checked)
                    {
                        GlobalValue.SerialPortOptData.IsOptIP = ceIP.Checked;
                        GlobalValue.SerialPortOptData.IP = new int[4];
                        GlobalValue.SerialPortOptData.IP[0] = Convert.ToInt32(txtNum1.Text);
                        GlobalValue.SerialPortOptData.IP[1] = Convert.ToInt32(txtNum2.Text);
                        GlobalValue.SerialPortOptData.IP[2] = Convert.ToInt32(txtNum3.Text);
                        GlobalValue.SerialPortOptData.IP[3] = Convert.ToInt32(txtNum4.Text);
                        haveset = true;
                    }
                    if (cePort.Checked)
                    {
                        GlobalValue.SerialPortOptData.IsOptPort = cePort.Checked;
                        GlobalValue.SerialPortOptData.Port = Convert.ToInt32(txtPort.Text);
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
                GlobalValue.SerialPortOptData = new UniversalSerialPortOptEntity();
                if (ceID.Checked)
                {
                    GlobalValue.SerialPortOptData.IsOptID = ceID.Checked;
                    GlobalValue.SerialPortOptData.ID = Convert.ToInt16(txtID.Text);
                }
                else
                {
                    GlobalValue.SerialPortOptData.ID = Convert.ToInt16(txtID.Text);
                    if (rgOpt.SelectedIndex == 0)
                        GlobalValue.SerialPortOptData.HydrantHistoryOpt = HydrantOptType.Open;
                    else if (rgOpt.SelectedIndex == 1)
                        GlobalValue.SerialPortOptData.HydrantHistoryOpt = HydrantOptType.Close;
                    else if (rgOpt.SelectedIndex == 2)
                        GlobalValue.SerialPortOptData.HydrantHistoryOpt = HydrantOptType.OpenAngle;
                    else if (rgOpt.SelectedIndex == 3)
                        GlobalValue.SerialPortOptData.HydrantHistoryOpt = HydrantOptType.Impact;
                    else if (rgOpt.SelectedIndex == 4)
                        GlobalValue.SerialPortOptData.HydrantHistoryOpt = HydrantOptType.KnockOver;
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

                    if (GlobalValue.SerialPortOptData.IsOptID)
                    {
                        txtID.Text = GlobalValue.SerialPortOptData.ID.ToString();
                    }
                    if (GlobalValue.SerialPortOptData.IsOptDT)
                    {
                        txtTime.Text = GlobalValue.SerialPortOptData.DT.ToString();
                    }
                    if (GlobalValue.SerialPortOptData.IsOptCellPhone)
                    {
                        txtCellPhone.Text = GlobalValue.SerialPortOptData.CellPhone;
                    }
                    if (GlobalValue.SerialPortOptData.IsOptComType)
                    {
                        cbComType.SelectedIndex = GlobalValue.SerialPortOptData.ComType;
                    }
                    if (GlobalValue.SerialPortOptData.IsOptIP)
                    {
                        txtNum1.Text = GlobalValue.SerialPortOptData.IP[0].ToString();
                        txtNum2.Text = GlobalValue.SerialPortOptData.IP[1].ToString();
                        txtNum3.Text = GlobalValue.SerialPortOptData.IP[2].ToString();
                        txtNum4.Text = GlobalValue.SerialPortOptData.IP[3].ToString();
                    }
                    if (GlobalValue.SerialPortOptData.IsOptPort)
                    {
                        txtPort.Text = GlobalValue.SerialPortOptData.Port.ToString();
                    }
                    if (GlobalValue.SerialPortOptData.IsOpt_SimualteInterval)
                    {
                        gridControl_History.DataSource=GlobalValue.SerialPortOptData.Simulate_Interval;
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

            if (ceCellPhone.Checked && !Regex.IsMatch(txtCellPhone.Text, @"^1\d{10}$"))
            {
                XtraMessageBox.Show("请输入手机号码[1XX XXXX XXXX]!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCellPhone.Focus();
                return false;
            }

            if (ceComType.Checked && string.IsNullOrEmpty(cbComType.Text))
            {
                XtraMessageBox.Show("请选择通信方式!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbComType.Focus();
                return false;
            }

            if (ceIP.Checked)
            {
                if (string.IsNullOrEmpty(txtNum1.Text) || string.IsNullOrEmpty(txtNum2.Text) ||
                    string.IsNullOrEmpty(txtNum3.Text) || string.IsNullOrEmpty(txtNum4.Text))
                {
                    XtraMessageBox.Show("请填写完整的IP地址!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtNum1.Focus();
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
            txtCellPhone.Enabled = true;
            txtCellPhone.Text = "";
            cbComType.Enabled = true;
            cbComType.SelectedIndex = -1;
            ceIP.Enabled = true;
            txtNum1.Enabled = true;
            txtNum1.Text = "";
            txtNum2.Enabled = true;
            txtNum2.Text = "";
            txtNum3.Enabled = true;
            txtNum3.Text = "";
            txtNum4.Enabled = true;
            txtNum4.Text = "";
            cePort.Enabled = true;
            txtPort.Enabled = true;
            txtPort.Text = "";
        }

        private void SetGprsCtrlStatus()
        {
            btnReset.Enabled = false;
            btnCheckingTime.Enabled = false;
            btnReadParm.Enabled = false;
            btnSetParm.Enabled = true;
            btnReadHistory.Enabled = false;

            ceTime.Enabled = false;
            ceTime.Checked = false;
            txtTime.Enabled = false;
            txtTime.Text = "";
            ceCellPhone.Enabled = false;
            ceCellPhone.Checked = false;
            txtCellPhone.Enabled = false;
            txtCellPhone.Text = "";
            ceComType.Enabled = false;
            ceComType.Checked = false;
            cbComType.Enabled = false;
            cbComType.SelectedIndex = -1;
            ceIP.Enabled = false;
            ceIP.Checked = false;
            txtNum1.Enabled = false;
            txtNum1.Text = "";
            txtNum2.Enabled = false;
            txtNum2.Text = "";
            txtNum3.Enabled = false;
            txtNum3.Text = "";
            txtNum4.Enabled = false;
            txtNum4.Text = "";
            cePort.Enabled = false;
            cePort.Checked = false;
            txtPort.Enabled = false;
            txtPort.Text = "";
        }

        private void ceIP_CheckedChanged(object sender, EventArgs e)
        {
            if (!ceIP.Checked)
            {
                txtNum1.Text = "";
                txtNum2.Text = "";
                txtNum3.Text = "";
                txtNum4.Text = "";
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
