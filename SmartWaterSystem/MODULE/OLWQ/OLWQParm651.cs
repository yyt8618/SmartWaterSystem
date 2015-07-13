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
    public partial class OLWQParm651 : BaseView, IOLWQParm651
    {
        public OLWQParm651()
        {
            InitializeComponent();

            #region Init Turbidity GridView
            cb_Turbidity_starttime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            for (int i = 0; i < 24; i++)
                cb_Turbidity_starttime.Items.Add(i);

            cb_Turbidity_coltime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_Turbidity_coltime.Items.Add(1);
            cb_Turbidity_coltime.Items.Add(5);
            cb_Turbidity_coltime.Items.Add(15);
            cb_Turbidity_coltime.Items.Add(30);
            cb_Turbidity_coltime.Items.Add(60);
            cb_Turbidity_coltime.Items.Add(120);

            cb_Turbidity_sendtime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_Turbidity_sendtime.Items.Add(5);
            cb_Turbidity_sendtime.Items.Add(15);
            cb_Turbidity_sendtime.Items.Add(30);
            cb_Turbidity_sendtime.Items.Add(60);
            cb_Turbidity_sendtime.Items.Add(120);
            cb_Turbidity_sendtime.Items.Add(240);
            cb_Turbidity_sendtime.Items.Add(480);
            cb_Turbidity_sendtime.Items.Add(720);
            cb_Turbidity_sendtime.Items.Add(1440);
            #endregion
        }

        private void OLWQParm651_Load(object sender, EventArgs e)
        {
            InitGridView();
            InitControls();

            cbPowersupplyType.SelectedIndex = 0;
        }


        private void InitGridView()
        {
            gridControl_Turbidity.DataSource = null;
        }

        private void InitControls()
        {
            ceColConfig.Checked = false;

            ceTurbidityInterval.Checked = false;
            gridControl_Turbidity.Enabled = false;
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

        #region 1-2byte unshort txt Event
        /// <summary>
        /// 限制最大1byte (<=255)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txt_onebyte_KeyPress(object sender, KeyPressEventArgs e)
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
        void txt_twobyte_KeyPress(object sender, KeyPressEventArgs e)
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

        #region Turbidity GridView
        int Turbidityrowindex = -1;
        private void gridView_Turbidity_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                GridView gridview = sender as GridView;
                if ((!string.IsNullOrEmpty(gridview.GetRowCellValue(e.RowHandle, "starttime").ToString())) &&
                    (!string.IsNullOrEmpty(gridview.GetRowCellValue(e.RowHandle, "collecttime").ToString())) &&
                    (!string.IsNullOrEmpty(gridview.GetRowCellValue(e.RowHandle, "sendtime").ToString())) &&
                    (gridview.RowCount < 6) && (gridview.RowCount == e.RowHandle + 1))
                {
                    gridview.AddNewRow();
                }
            }
        }

        private void gridView_Turbidity_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                GridView gridview = sender as GridView;
                Turbidityrowindex = e.RowHandle;
                cb_Turbidity_starttime.Items.Clear();
                if ((gridview.RowCount > Turbidityrowindex + 1) && (Turbidityrowindex > -1))
                {
                    int starttime = 0;
                    if ((Turbidityrowindex - 1) > -1)
                        starttime = Convert.ToInt32(gridview.GetRowCellValue(Turbidityrowindex - 1, "starttime")) + 1;
                    if (starttime < 0)
                        starttime = 0;
                    int endtime = 23;
                    object obj_endtime = gridview.GetRowCellValue(Turbidityrowindex + 1, "starttime");
                    if (obj_endtime != DBNull.Value && obj_endtime != null)
                        endtime = Convert.ToInt32(obj_endtime);
                    for (int i = starttime; i < endtime; i++)
                    {
                        cb_Turbidity_starttime.Items.Add(i);
                    }
                }
                else
                {
                    int i = 0;
                    if (Turbidityrowindex > 0)
                    {
                        if (gridview.GetRowCellValue(Turbidityrowindex - 1, "starttime") != DBNull.Value && gridview.GetRowCellValue(Turbidityrowindex - 1, "starttime") != null)
                            i = Convert.ToInt32(gridview.GetRowCellValue(Turbidityrowindex - 1, "starttime")) + 1;
                    }
                    for (; i < 24; i++)
                    {
                        cb_Turbidity_starttime.Items.Add(i);
                    }
                }
            }
        }
        #endregion

        private void ceColConfig_CheckedChanged(object sender, EventArgs e)
        {
            ceTurbidityState.Enabled = ceColConfig.Checked;
            cePHState.Enabled = ceColConfig.Checked;
            ceConductivityState.Enabled = ceColConfig.Checked;

            if (!ceColConfig.Checked)
            {
                ceTurbidityState.Checked = ceColConfig.Checked;
                cePHState.Checked = ceColConfig.Checked;
                ceConductivityState.Checked = ceColConfig.Checked;
            }
        }

        private void ceTurbidityInterval_CheckedChanged(object sender, EventArgs e)
        {
            gridControl_Turbidity.Enabled = ceTurbidityInterval.Checked;
            if (!ceTurbidityInterval.Checked)
                gridControl_Turbidity.DataSource = null;
            else
            {
                DataTable dt_485 = new DataTable();
                dt_485.Columns.Add("starttime");
                dt_485.Columns.Add("collecttime");
                dt_485.Columns.Add("sendtime");
                gridControl_Turbidity.DataSource = dt_485;
                gridView_Turbidity.AddNewRow();
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
            ShowWaitForm("", "正在复位终端...");
            BeginSerialPortDelegate();
            Application.DoEvents();
            SetStaticItem("正在复位终端...");
            GlobalValue.SerialPortMgr.Send(SerialPortType.OLWQReset);
        }

        private void btnCheckingTime_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                XtraMessageBox.Show("请先填写终端ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            GlobalValue.SerialPortMgr.Send(SerialPortType.OLWQSetTime);
        }

        private void btnEnableCollect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                XtraMessageBox.Show("请先填写终端ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return;
            }
            GlobalValue.SerialPortOptData = new UniversalSerialPortOptEntity();
            GlobalValue.SerialPortOptData.ID = Convert.ToInt16(txtID.Text);

            EnableControls(false);
            DisableRibbonBar();
            DisableNavigateBar();
            ShowWaitForm("", "正在启动采集...");
            BeginSerialPortDelegate();
            Application.DoEvents();
            SetStaticItem("正在启动采集...");
            GlobalValue.SerialPortMgr.Send(SerialPortType.OLWQSetEnableCollect);
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
                    XtraMessageBox.Show("请输入设备ID", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtID.Focus();
                    return;
                }
                GlobalValue.SerialPortOptData.ID = Convert.ToInt16(txtID.Text);

                if (ceTime.Checked)
                {
                    GlobalValue.SerialPortOptData.IsOptDT = ceTime.Checked;
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
                if (ceClearInterval.Checked)
                {
                    GlobalValue.SerialPortOptData.IsOptClearInterval = ceClearInterval.Checked;
                    haveread = true;
                }
                if (ceTurbidityUpLimit.Checked)
                {
                    GlobalValue.SerialPortOptData.IsOptTurbidityUpLimit = ceTurbidityUpLimit.Checked;
                    haveread = true;
                }
                if (cePowersupplyType.Checked)
                {
                    GlobalValue.SerialPortOptData.IsOptPowerSupplyType = cePowersupplyType.Checked;
                    haveread = true;
                }

                if (ceColConfig.Checked)
                {
                    GlobalValue.SerialPortOptData.IsOpt_CollectConfig = ceColConfig.Checked;
                    haveread = true;

                    cePHState.Checked = false;
                    ceConductivityState.Checked = false;
                    ceTurbidityState.Checked = false;
                }

                if (ceTerAddr.Checked)
                {
                    GlobalValue.SerialPortOptData.IsOptTerAddr = ceTerAddr.Checked;
                    haveread = true;
                    txtA5.Text = "";
                    txtA4.Text = "";
                    txtA3.Text = "";
                    txtA2.Text = "";
                    txtA1.Text = "";
                }

                if (ceCenterAddr.Checked)
                {
                    GlobalValue.SerialPortOptData.IsOptCenterAddr = ceCenterAddr.Checked;
                    haveread = true;
                }

                if (cePwd.Checked)
                {
                    GlobalValue.SerialPortOptData.IsOptPwd = cePwd.Checked;
                    haveread = true;
                }

                if (ceWorkType.Checked)
                {
                    GlobalValue.SerialPortOptData.IsOptWorkType = ceWorkType.Checked;
                    haveread = true;
                }

                if (ceGprsSwitch.Checked)
                {
                    GlobalValue.SerialPortOptData.IsOptGprsSwitch = ceGprsSwitch.Checked;
                    haveread = true;
                }

                if (ceTempUpLimit.Checked)
                {
                    GlobalValue.SerialPortOptData.IsOptTempUpLimit = ceTempUpLimit.Checked;
                    haveread = true;
                }

                if (ceTempLowLimit.Checked)
                {
                    GlobalValue.SerialPortOptData.IsOptTempLowLimit = ceTempLowLimit.Checked;
                    haveread = true;
                }

                if (cePHUpLimit.Checked)
                {
                    GlobalValue.SerialPortOptData.IsOptPHUpLimit = cePHUpLimit.Checked;
                    haveread = true;
                }

                if (cePHLowLimit.Checked)
                {
                    GlobalValue.SerialPortOptData.IsOptPHLowLimit = cePHLowLimit.Checked;
                    haveread = true;
                }

                if (ceConductivityUpLimit.Checked)
                {
                    GlobalValue.SerialPortOptData.IsOptConductivityUpLimit = ceConductivityUpLimit.Checked;
                    haveread = true;
                }

                if (ceConductivityLowLimit.Checked)
                {
                    GlobalValue.SerialPortOptData.IsOptConductivityLowLimit = ceConductivityLowLimit.Checked;
                    haveread = true;
                }

                if (ceTurbidityInterval.Checked)
                {
                    GlobalValue.SerialPortOptData.IsOpt_TurbidityInterval = ceTurbidityInterval.Checked;
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
            GlobalValue.SerialPortMgr.SerialPortScheduleEvent -= new SerialPortScheduleHandle(SerialPortMgr_SerialPortScheduleEvent);
            GlobalValue.SerialPortMgr.SerialPortScheduleEvent += new SerialPortScheduleHandle(SerialPortMgr_SerialPortScheduleEvent);
            Application.DoEvents();
            SetStaticItem("正在读取...");
            GlobalValue.SerialPortMgr.Send(SerialPortType.OLWQReadBaicInfo);
        }

        void SerialPortMgr_SerialPortScheduleEvent(object sender, SerialPortScheduleEventArgs e)
        {
            if ((e.OptType == SerialPortType.OLWQReadBaicInfo || e.OptType == SerialPortType.OLWQSetBasicInfo) && !string.IsNullOrEmpty(e.Msg))
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
                    if (DialogResult.No == XtraMessageBox.Show("设置设备编号会初始化设备参数,是否继续?", GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
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
                    if (ceTerAddr.Checked)
                    {
                        GlobalValue.SerialPortOptData.IsOptTerAddr = ceTerAddr.Checked;
                        GlobalValue.SerialPortOptData.TerAddr = new byte[] { Convert.ToByte(txtA5.Text, 16), Convert.ToByte(txtA4.Text, 16), Convert.ToByte(txtA3.Text, 16), Convert.ToByte(txtA2.Text, 16), Convert.ToByte(txtA1.Text, 16) };
                        haveset = true;
                    }
                    if (ceCenterAddr.Checked)
                    {
                        GlobalValue.SerialPortOptData.IsOptCenterAddr = ceCenterAddr.Checked;
                        GlobalValue.SerialPortOptData.CenterAddr = Convert.ToByte(txtCenterAddr.Text);
                        haveset = true;
                    }
                    if (cePwd.Checked)
                    {
                        GlobalValue.SerialPortOptData.IsOptPwd = cePwd.Checked;
                        GlobalValue.SerialPortOptData.Pwd = new byte[] { Convert.ToByte(txtPwd1.Text, 16), Convert.ToByte(txtPwd0.Text, 16) };
                        haveset = true;
                    }
                    if (ceWorkType.Checked)
                    {
                        GlobalValue.SerialPortOptData.IsOptWorkType = ceWorkType.Checked;
                        GlobalValue.SerialPortOptData.WorkType = Convert.ToByte(cbWorkType.SelectedIndex + 1);
                        haveset=true;
                    }
                    if (ceGprsSwitch.Checked)
                    {
                        GlobalValue.SerialPortOptData.IsOptGprsSwitch = ceGprsSwitch.Checked;
                        GlobalValue.SerialPortOptData.GprsSwitch = toggleGprsSwitch.IsOn;
                        haveset = true;
                    }
                    if (ceClearInterval.Checked)
                    {
                        GlobalValue.SerialPortOptData.IsOptClearInterval = ceClearInterval.Checked;
                        GlobalValue.SerialPortOptData.ClearInterval = Convert.ToUInt16(txtClearInterval.Text);
                        haveset = true;
                    }
                    if (ceTempUpLimit.Checked)
                    {
                        GlobalValue.SerialPortOptData.IsOptTempUpLimit = ceTempUpLimit.Checked;
                        GlobalValue.SerialPortOptData.TempUpLimit = Convert.ToUInt16(txtTempUpLimit.Text);
                        haveset = true;
                    }
                    if (ceTempLowLimit.Checked)
                    {
                        GlobalValue.SerialPortOptData.IsOptTempLowLimit = ceTempLowLimit.Checked;
                        GlobalValue.SerialPortOptData.TempLowLimit = Convert.ToUInt16(txtTempLowLimit.Text);
                        haveset = true;
                    }
                    if (cePHUpLimit.Checked)
                    {
                        GlobalValue.SerialPortOptData.IsOptPHUpLimit = cePHUpLimit.Checked;
                        GlobalValue.SerialPortOptData.PHUpLimit = Convert.ToUInt16(txtTempUpLimit.Text);
                        haveset = true;
                    }
                    if (cePHLowLimit.Checked)
                    {
                        GlobalValue.SerialPortOptData.IsOptPHLowLimit = cePHLowLimit.Checked;
                        GlobalValue.SerialPortOptData.PHLowLimit = Convert.ToUInt16(txtTempLowLimit.Text);
                        haveset = true;
                    }
                    if (ceConductivityUpLimit.Checked)
                    {
                        GlobalValue.SerialPortOptData.IsOptConductivityUpLimit = ceConductivityUpLimit.Checked;
                        GlobalValue.SerialPortOptData.ConductivityUpLimit = Convert.ToUInt16(txtConductivityUpLimit.Text);
                        haveset = true;
                    }
                    if (ceConductivityLowLimit.Checked)
                    {
                        GlobalValue.SerialPortOptData.IsOptConductivityLowLimit = ceConductivityLowLimit.Checked;
                        GlobalValue.SerialPortOptData.ConductivityLowLimit = Convert.ToUInt16(txtConductivityLowLimit.Text);
                        haveset = true;
                    }
                    if (ceTurbidityUpLimit.Checked)
                    {
                        GlobalValue.SerialPortOptData.IsOptTurbidityUpLimit = ceTurbidityUpLimit.Checked;
                        GlobalValue.SerialPortOptData.TurbidityUpLimit = Convert.ToUInt16(txtTurbidityUpLimit.Text);
                        haveset = true;
                    }
                    if (ceTurbidityLowLimit.Checked)
                    {
                        GlobalValue.SerialPortOptData.IsOptTurbidityLowLimit = ceTurbidityLowLimit.Checked;
                        GlobalValue.SerialPortOptData.TurbidityLowLimit = Convert.ToUInt16(txtTurbidityLowLimit.Text);
                        haveset = true;
                    }
                    if (cePowersupplyType.Checked)
                    {
                        GlobalValue.SerialPortOptData.IsOptPowerSupplyType = cePowersupplyType.Checked;
                        GlobalValue.SerialPortOptData.PowerSupplyType = (ushort)(cbPowersupplyType.SelectedIndex + 1);
                        haveset = true;
                    }
                    if (ceColConfig.Checked)
                    {
                        GlobalValue.SerialPortOptData.IsOpt_CollectConfig = ceColConfig.Checked;
                        if (ceTurbidityState.Checked)
                            GlobalValue.SerialPortOptData.Collect_Turbidity = true;
                        if (cePHState.Checked)
                            GlobalValue.SerialPortOptData.Collect_PH = true;
                        if (ceConductivityState.Checked)
                            GlobalValue.SerialPortOptData.Collect_Conductivity = true;

                        haveset = true;
                    }
                    if (ceTurbidityInterval.Checked)
                    {
                        GlobalValue.SerialPortOptData.IsOpt_TurbidityInterval = ceTurbidityInterval.Checked;
                        GlobalValue.SerialPortOptData.Turbidity_Interval = gridControl_Turbidity.DataSource as DataTable;
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
                GlobalValue.SerialPortMgr.SerialPortScheduleEvent -= new SerialPortScheduleHandle(SerialPortMgr_SerialPortScheduleEvent);
                GlobalValue.SerialPortMgr.SerialPortScheduleEvent += new SerialPortScheduleHandle(SerialPortMgr_SerialPortScheduleEvent);
                Application.DoEvents();
                SetStaticItem("正在设置...");
                GlobalValue.SerialPortMgr.Send(SerialPortType.OLWQSetBasicInfo);
            }
        }

        public override void OnSerialPortNotify(object sender, SerialPortEventArgs e)
        {
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.OLWQReset)
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
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.OLWQSetTime)
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
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.OLWQSetEnableCollect)
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

                    XtraMessageBox.Show("设置启动采集成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    XtraMessageBox.Show("设置启动采集失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.OLWQReadBaicInfo)
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
                        txtID.Text = GlobalValue.SerialPortOptData.ID.ToString();
                    if (GlobalValue.SerialPortOptData.IsOptDT)
                        txtTime.Text = GlobalValue.SerialPortOptData.DT.ToString();
                    if (GlobalValue.SerialPortOptData.IsOptIP)
                    {
                        txtNum1.Text = GlobalValue.SerialPortOptData.IP[0].ToString();
                        txtNum2.Text = GlobalValue.SerialPortOptData.IP[1].ToString();
                        txtNum3.Text = GlobalValue.SerialPortOptData.IP[2].ToString();
                        txtNum4.Text = GlobalValue.SerialPortOptData.IP[3].ToString();
                    }
                    if (GlobalValue.SerialPortOptData.IsOptPort)
                        txtPort.Text = GlobalValue.SerialPortOptData.Port.ToString();
                    if (GlobalValue.SerialPortOptData.IsOptTerAddr)
                    {
                        txtA5.Text = string.Format("{0:X2}", GlobalValue.SerialPortOptData.TerAddr[4]);
                        txtA4.Text = string.Format("{0:X2}", GlobalValue.SerialPortOptData.TerAddr[3]);
                        txtA3.Text = string.Format("{0:X2}", GlobalValue.SerialPortOptData.TerAddr[2]);
                        txtA2.Text = string.Format("{0:X2}", GlobalValue.SerialPortOptData.TerAddr[1]);
                        txtA1.Text = string.Format("{0:X2}", GlobalValue.SerialPortOptData.TerAddr[0]);
                    }
                    if (GlobalValue.SerialPortOptData.IsOptTerAddr)
                        txtCenterAddr.Text = string.Format("{0:X2}", GlobalValue.SerialPortOptData.CenterAddr);
                    if (GlobalValue.SerialPortOptData.IsOptPwd)
                    {
                        txtPwd1.Text = string.Format("{0:X2}", GlobalValue.SerialPortOptData.Pwd[1]);
                        txtPwd0.Text = string.Format("{0:X2}", GlobalValue.SerialPortOptData.Pwd[0]);
                    }
                    if (GlobalValue.SerialPortOptData.IsOptWorkType)
                        cbWorkType.SelectedIndex = GlobalValue.SerialPortOptData.WorkType-1;
                    if (GlobalValue.SerialPortOptData.IsOptGprsSwitch)
                        toggleGprsSwitch.IsOn = GlobalValue.SerialPortOptData.GprsSwitch;
                    if (GlobalValue.SerialPortOptData.IsOptClearInterval)
                        txtClearInterval.Text = GlobalValue.SerialPortOptData.ClearInterval.ToString();
                    if (GlobalValue.SerialPortOptData.IsOptTempUpLimit)
                        txtTempUpLimit.Text = GlobalValue.SerialPortOptData.TempUpLimit.ToString();
                    if (GlobalValue.SerialPortOptData.IsOptTempLowLimit)
                        txtTempLowLimit.Text = GlobalValue.SerialPortOptData.TempLowLimit.ToString();
                    if (GlobalValue.SerialPortOptData.IsOptPHUpLimit)
                        txtPHUpLimit.Text = GlobalValue.SerialPortOptData.PHUpLimit.ToString();
                    if (GlobalValue.SerialPortOptData.IsOptPHLowLimit)
                        txtPHLowLimit.Text = GlobalValue.SerialPortOptData.PHLowLimit.ToString();
                    if (GlobalValue.SerialPortOptData.IsOptConductivityUpLimit)
                        txtConductivityUpLimit.Text = GlobalValue.SerialPortOptData.ConductivityUpLimit.ToString();
                    if (GlobalValue.SerialPortOptData.IsOptConductivityLowLimit)
                        txtConductivityLowLimit.Text = GlobalValue.SerialPortOptData.IsOptConductivityLowLimit.ToString();
                    if (GlobalValue.SerialPortOptData.IsOptTurbidityUpLimit)
                        txtTurbidityUpLimit.Text = GlobalValue.SerialPortOptData.TurbidityUpLimit.ToString();
                    if (GlobalValue.SerialPortOptData.IsOptTurbidityLowLimit)
                        txtTurbidityLowLimit.Text = GlobalValue.SerialPortOptData.TurbidityLowLimit.ToString();
                    if (GlobalValue.SerialPortOptData.IsOptPowerSupplyType)
                        cbPowersupplyType.SelectedIndex = GlobalValue.SerialPortOptData.PowerSupplyType-1;

                    if (GlobalValue.SerialPortOptData.IsOpt_CollectConfig)
                    {
                        cePHState.Checked =GlobalValue.SerialPortOptData.Collect_PH;
                        ceConductivityState.Checked = GlobalValue.SerialPortOptData.Collect_Conductivity;
                        ceTurbidityState.Checked = GlobalValue.SerialPortOptData.Collect_Turbidity;
                    }
                    if (GlobalValue.SerialPortOptData.IsOpt_TurbidityInterval)
                        gridControl_Turbidity.DataSource = GlobalValue.SerialPortOptData.Turbidity_Interval;

                    XtraMessageBox.Show("读取设备参数成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    XtraMessageBox.Show("读取设备参数失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.OLWQSetBasicInfo)
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

                    XtraMessageBox.Show("设置设备参数成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    XtraMessageBox.Show("设置设备参数失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.OLWQCallData)
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

                    if (e.Tag != null)
                        gridControl_CallData.DataSource = e.Tag as DataTable;

                    XtraMessageBox.Show("招测成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    XtraMessageBox.Show("招测失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void EnableControls(bool enable)
        {
            btnReset.Enabled = enable;
            btnCheckingTime.Enabled = enable;
            btnEnableCollect.Enabled = enable;
            btnReadParm.Enabled = enable;
            btnSetParm.Enabled = enable;
        }

        private bool Validate()
        {
            if (!Regex.IsMatch(txtID.Text,@"^\d{1,5}$"))
            {
                XtraMessageBox.Show("请输入设备ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
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

            if (cePort.Checked && !Regex.IsMatch(txtPort.Text, @"^\d{3,5}$"))
            {
                XtraMessageBox.Show("请输入端口号!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPort.Focus();
                return false;
            }

            if (ceClearInterval.Checked && string.IsNullOrEmpty(txtClearInterval.Text))
            {
                XtraMessageBox.Show("请输入清洗间隔!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtClearInterval.Focus();
                return false;
            }

            if (ceTurbidityUpLimit.Checked && string.IsNullOrEmpty(txtTurbidityUpLimit.Text))
            {
                XtraMessageBox.Show("请输入合法的浊度上限值!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTurbidityUpLimit.Focus();
                return false;
            }

            if (cePowersupplyType.Checked &&string.IsNullOrEmpty(cbPowersupplyType.Text))
            {
                XtraMessageBox.Show("请选择的供电方式!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbPowersupplyType.Focus();
                return false;
            }

            if (ceTurbidityInterval.Checked)
            {
                DataTable dt = gridControl_Turbidity.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0)
                {
                    XtraMessageBox.Show("请填写浊度采集时间间隔表!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    gridView_Turbidity.Focus();
                    return false;
                }
            }


            return true;
        }
        #endregion

        public override void SerialPortEvent(bool Enabled)
        {
            btnReset.Enabled = Enabled;
            btnCheckingTime.Enabled = Enabled;
            btnEnableCollect.Enabled = Enabled;
            btnReadParm.Enabled = Enabled;
            btnSetParm.Enabled = Enabled;
        }

        private void SetSerialPortCtrlStatus()
        {
            btnReset.Enabled = GlobalValue.portUtil.IsOpen;
            btnCheckingTime.Enabled = GlobalValue.portUtil.IsOpen;
            btnEnableCollect.Enabled = GlobalValue.portUtil.IsOpen;
            btnReadParm.Enabled = GlobalValue.portUtil.IsOpen;
            btnSetParm.Enabled = GlobalValue.portUtil.IsOpen;

            ceTime.Enabled = true;
            txtTime.Enabled = true;
            txtTime.Text = "";
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
            cePowersupplyType.Enabled = true;
            cePowersupplyType.Checked = false;
            cbPowersupplyType.Enabled = true;
            cbPowersupplyType.SelectedIndex = -1;

            ceColConfig.Enabled = true;
            cePHState.Enabled = false;
            cePHState.Checked = false;
            ceConductivityState.Enabled = false;
            ceConductivityState.Checked = false;
            ceTurbidityState.Enabled = false;
            ceTurbidityState.Checked = false;

            ceClearInterval.Enabled = true;
            ceClearInterval.Checked = false;
            txtClearInterval.Enabled = true;
            txtClearInterval.Text = "";
            ceTurbidityUpLimit.Enabled = true;
            ceTurbidityUpLimit.Checked = false;
            txtTurbidityUpLimit.Enabled = true;
            txtTurbidityUpLimit.Text = "";
            
            ceTurbidityInterval.Enabled = true;
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

        private void ceClearInterval_CheckedChanged(object sender, EventArgs e)
        {
            if (!ceClearInterval.Checked)
            {
                txtClearInterval.Text = "";
            }
        }

        private void ceTurbidityUpLimit_CheckedChanged(object sender, EventArgs e)
        {
            if (!ceTurbidityUpLimit.Checked)
            {
                txtTurbidityUpLimit.Text = "";
            }
        }

        private void cePowersupplyType_CheckedChanged(object sender, EventArgs e)
        {
            if (!cePowersupplyType.Checked)
            {
                cbPowersupplyType.SelectedIndex = -1;
            }
        }

        private void btnCallData_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                XtraMessageBox.Show("请输入终端编号!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return;
            }
            if (!Regex.IsMatch(txtID.Text, @"^\d{1,5}$"))
            {
                XtraMessageBox.Show("请输入合法终端编号!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return;
            }

            if (GlobalValue.SerialPortOptData == null)
                GlobalValue.SerialPortOptData = new UniversalSerialPortOptEntity();
            GlobalValue.SerialPortOptData.ID = Convert.ToInt16(txtID.Text);

            gridControl_CallData.DataSource = null;
            EnableControls(false);
            DisableRibbonBar();
            DisableNavigateBar();
            ShowWaitForm("", "正在招测...");
            BeginSerialPortDelegate();
            GlobalValue.SerialPortMgr.SerialPortScheduleEvent -= new SerialPortScheduleHandle(SerialPortMgr_SerialPortScheduleEvent);
            GlobalValue.SerialPortMgr.SerialPortScheduleEvent += new SerialPortScheduleHandle(SerialPortMgr_SerialPortScheduleEvent);
            Application.DoEvents();
            SetStaticItem("正在招测...");
            GlobalValue.SerialPortMgr.Send(SerialPortType.OLWQCallData);
        }


    }
}
