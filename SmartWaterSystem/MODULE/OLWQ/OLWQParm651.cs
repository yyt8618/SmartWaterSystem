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

        #region A5-A1
        private bool ValidateA5To1()
        {
            if (string.IsNullOrEmpty(txtA5.Text) || !Regex.IsMatch(txtA5.Text, "^[a-zA-Z0-9]{1,2}$"))
            {
                XtraMessageBox.Show("请填写A5", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtA5.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtA4.Text) || !Regex.IsMatch(txtA4.Text, "^[a-zA-Z0-9]{1,2}$"))
            {
                XtraMessageBox.Show("请填写A4", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtA4.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtA3.Text) || !Regex.IsMatch(txtA3.Text, "^[a-zA-Z0-9]{1,2}$"))
            {
                XtraMessageBox.Show("请填写A3", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtA3.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtA2.Text) || !Regex.IsMatch(txtA2.Text, "^[a-zA-Z0-9]{1,2}$"))
            {
                XtraMessageBox.Show("请填写A2", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtA2.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtA1.Text) || !Regex.IsMatch(txtA1.Text, "^[a-zA-Z0-9]{1,2}$"))
            {
                XtraMessageBox.Show("请填写A1", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtA1.Focus();
                return false;
            }


            return true;
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
            else if (e.KeyChar == ' ' || e.KeyChar == '\r')// || e.KeyChar == '.')
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
            GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity();
            GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

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
            GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity();
            GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

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
            GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity();
            GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

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
                    XtraMessageBox.Show("请输入设备ID", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtID.Focus();
                    return;
                }
                GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

                if (ceTime.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptDT = ceTime.Checked;
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
                if (ceClearInterval.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptClearInterval = ceClearInterval.Checked;
                    haveread = true;
                }
                if (ceDataInterval.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptDataInterval = ceDataInterval.Checked;
                    haveread = true;
                }
                if (ceTurbidityUpLimit.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptTurbidityUpLimit = ceTurbidityUpLimit.Checked;
                    haveread = true;
                }
                if (ceTurbidityLowLimit.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptTurbidityLowLimit = ceTurbidityLowLimit.Checked;
                    haveread = true;
                }
                if (cePowersupplyType.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptPowerSupplyType = cePowersupplyType.Checked;
                    haveread = true;
                }

                if (ceColConfig.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_CollectConfig = ceColConfig.Checked;
                    haveread = true;

                    cePHState.Checked = false;
                    ceConductivityState.Checked = false;
                    ceTurbidityState.Checked = false;
                }

                if (ceTerAddr.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptTerAddr = ceTerAddr.Checked;
                    haveread = true;
                    txtA5.Text = "";
                    txtA4.Text = "";
                    txtA3.Text = "";
                    txtA2.Text = "";
                    txtA1.Text = "";
                }

                if (ceCenterAddr.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptCenterAddr = ceCenterAddr.Checked;
                    haveread = true;
                }

                if (cePwd.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptPwd = cePwd.Checked;
                    haveread = true;
                }

                if (ceWorkType.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptWorkType = ceWorkType.Checked;
                    haveread = true;
                }

                if (ceGprsSwitch.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptGprsSwitch = ceGprsSwitch.Checked;
                    haveread = true;
                }

                if (ceTempUpLimit.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptTempUpLimit = ceTempUpLimit.Checked;
                    haveread = true;
                }

                if (ceTempLowLimit.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptTempLowLimit = ceTempLowLimit.Checked;
                    haveread = true;
                }

                if (ceTempAddtion.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptTempAddtion = ceTempAddtion.Checked;
                    haveread = true;
                }

                if (cePHUpLimit.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptPHUpLimit = cePHUpLimit.Checked;
                    haveread = true;
                }

                if (cePHLowLimit.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptPHLowLimit = cePHLowLimit.Checked;
                    haveread = true;
                }

                if (ceConductivityUpLimit.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptConductivityUpLimit = ceConductivityUpLimit.Checked;
                    haveread = true;
                }

                if (ceConductivityLowLimit.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptConductivityLowLimit = ceConductivityLowLimit.Checked;
                    haveread = true;
                }

                if (ceTurbidityInterval.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_TurbidityInterval = ceTurbidityInterval.Checked;
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
                GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity();
                bool haveset = false;
                if (ceID.Checked)
                {
                    if (DialogResult.No == XtraMessageBox.Show("设置设备编号会初始化设备参数,是否继续?", GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
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

                    if (ceIP.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptIP = ceIP.Checked;
                        GlobalValue.UniSerialPortOptData.IP = new int[4];
                        GlobalValue.UniSerialPortOptData.IP[0] = Convert.ToInt32(txtNum1.Text);
                        GlobalValue.UniSerialPortOptData.IP[1] = Convert.ToInt32(txtNum2.Text);
                        GlobalValue.UniSerialPortOptData.IP[2] = Convert.ToInt32(txtNum3.Text);
                        GlobalValue.UniSerialPortOptData.IP[3] = Convert.ToInt32(txtNum4.Text);
                        haveset = true;
                    }
                    if (cePort.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptPort = cePort.Checked;
                        GlobalValue.UniSerialPortOptData.Port = Convert.ToInt32(txtPort.Text);
                        haveset = true;
                    }
                    if (ceTerAddr.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptTerAddr = ceTerAddr.Checked;
                        GlobalValue.UniSerialPortOptData.TerAddr = new byte[] { Convert.ToByte(txtA5.Text, 16), Convert.ToByte(txtA4.Text, 16), Convert.ToByte(txtA3.Text, 16), Convert.ToByte(txtA2.Text, 16), Convert.ToByte(txtA1.Text, 16) };
                        haveset = true;
                    }
                    if (ceCenterAddr.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptCenterAddr = ceCenterAddr.Checked;
                        GlobalValue.UniSerialPortOptData.CenterAddr = Convert.ToByte(txtCenterAddr.Text,16);
                        haveset = true;
                    }
                    if (cePwd.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptPwd = cePwd.Checked;
                        GlobalValue.UniSerialPortOptData.Pwd = new byte[] { Convert.ToByte(txtPwd1.Text, 16), Convert.ToByte(txtPwd0.Text, 16) };
                        haveset = true;
                    }
                    if (ceWorkType.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptWorkType = ceWorkType.Checked;
                        GlobalValue.UniSerialPortOptData.WorkType = Convert.ToByte(cbWorkType.SelectedIndex + 1);
                        haveset=true;
                    }
                    if (ceGprsSwitch.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptGprsSwitch = ceGprsSwitch.Checked;
                        GlobalValue.UniSerialPortOptData.GprsSwitch = toggleGprsSwitch.IsOn;
                        haveset = true;
                    }
                    if (ceClearInterval.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptClearInterval = ceClearInterval.Checked;
                        GlobalValue.UniSerialPortOptData.ClearInterval = Convert.ToUInt16(txtClearInterval.Text);
                        haveset = true;
                    }
                    if (ceDataInterval.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptDataInterval = ceDataInterval.Checked;
                        GlobalValue.UniSerialPortOptData.DataInterval = Convert.ToUInt16(txtDataInterval.Text);
                        haveset = true;
                    }
                    if (ceTempUpLimit.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptTempUpLimit = ceTempUpLimit.Checked;
                        GlobalValue.UniSerialPortOptData.TempUpLimit = (ushort)(Convert.ToDouble(txtTempUpLimit.Text) * 10);
                        haveset = true;
                    }
                    if (ceTempLowLimit.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptTempLowLimit = ceTempLowLimit.Checked;
                        GlobalValue.UniSerialPortOptData.TempLowLimit = (ushort)(Convert.ToDouble(txtTempLowLimit.Text) * 10);
                        haveset = true;
                    }
                    if (ceTempAddtion.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptTempAddtion = ceTempAddtion.Checked;
                        GlobalValue.UniSerialPortOptData.TempAddtion = Convert.ToUInt16(txtTempAddtion.Text);
                        haveset = true;
                    }
                    if (cePHUpLimit.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptPHUpLimit = cePHUpLimit.Checked;
                        GlobalValue.UniSerialPortOptData.PHUpLimit = (ushort)(Convert.ToDouble(txtPHUpLimit.Text) * 10);
                        haveset = true;
                    }
                    if (cePHLowLimit.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptPHLowLimit = cePHLowLimit.Checked;
                        GlobalValue.UniSerialPortOptData.PHLowLimit = (ushort)(Convert.ToDouble(txtPHLowLimit.Text) * 10);
                        haveset = true;
                    }
                    if (ceConductivityUpLimit.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptConductivityUpLimit = ceConductivityUpLimit.Checked;
                        GlobalValue.UniSerialPortOptData.ConductivityUpLimit = Convert.ToUInt16(txtConductivityUpLimit.Text);
                        haveset = true;
                    }
                    if (ceConductivityLowLimit.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptConductivityLowLimit = ceConductivityLowLimit.Checked;
                        GlobalValue.UniSerialPortOptData.ConductivityLowLimit = Convert.ToUInt16(txtConductivityLowLimit.Text);
                        haveset = true;
                    }
                    if (ceTurbidityUpLimit.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptTurbidityUpLimit = ceTurbidityUpLimit.Checked;
                        GlobalValue.UniSerialPortOptData.TurbidityUpLimit = Convert.ToUInt16(txtTurbidityUpLimit.Text);
                        haveset = true;
                    }
                    if (ceTurbidityLowLimit.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptTurbidityLowLimit = ceTurbidityLowLimit.Checked;
                        GlobalValue.UniSerialPortOptData.TurbidityLowLimit = Convert.ToUInt16(txtTurbidityLowLimit.Text);
                        haveset = true;
                    }
                    if (cePowersupplyType.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptPowerSupplyType = cePowersupplyType.Checked;
                        GlobalValue.UniSerialPortOptData.PowerSupplyType = (ushort)(cbPowersupplyType.SelectedIndex + 1);
                        haveset = true;
                    }
                    if (ceColConfig.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOpt_CollectConfig = ceColConfig.Checked;
                        if (ceTurbidityState.Checked)
                            GlobalValue.UniSerialPortOptData.Collect_Turbidity = true;
                        if (cePHState.Checked)
                            GlobalValue.UniSerialPortOptData.Collect_PH = true;
                        if (ceConductivityState.Checked)
                            GlobalValue.UniSerialPortOptData.Collect_Conductivity = true;

                        haveset = true;
                    }
                    if (ceTurbidityInterval.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOpt_TurbidityInterval = ceTurbidityInterval.Checked;
                        GlobalValue.UniSerialPortOptData.Turbidity_Interval = gridControl_Turbidity.DataSource as DataTable;
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

                    if (GlobalValue.UniSerialPortOptData.IsOptID)
                        txtID.Text = GlobalValue.UniSerialPortOptData.ID.ToString();
                    if (GlobalValue.UniSerialPortOptData.IsOptDT)
                        txtTime.Text = GlobalValue.UniSerialPortOptData.DT.ToString();
                    if (GlobalValue.UniSerialPortOptData.IsOptIP)
                    {
                        txtNum1.Text = GlobalValue.UniSerialPortOptData.IP[0].ToString();
                        txtNum2.Text = GlobalValue.UniSerialPortOptData.IP[1].ToString();
                        txtNum3.Text = GlobalValue.UniSerialPortOptData.IP[2].ToString();
                        txtNum4.Text = GlobalValue.UniSerialPortOptData.IP[3].ToString();
                    }
                    if (GlobalValue.UniSerialPortOptData.IsOptPort)
                        txtPort.Text = GlobalValue.UniSerialPortOptData.Port.ToString();
                    if (GlobalValue.UniSerialPortOptData.IsOptTerAddr)
                    {
                        txtA5.Text = string.Format("{0:X2}", GlobalValue.UniSerialPortOptData.TerAddr[4]);
                        txtA4.Text = string.Format("{0:X2}", GlobalValue.UniSerialPortOptData.TerAddr[3]);
                        txtA3.Text = string.Format("{0:X2}", GlobalValue.UniSerialPortOptData.TerAddr[2]);
                        txtA2.Text = string.Format("{0:X2}", GlobalValue.UniSerialPortOptData.TerAddr[1]);
                        txtA1.Text = string.Format("{0:X2}", GlobalValue.UniSerialPortOptData.TerAddr[0]);
                    }
                    if (GlobalValue.UniSerialPortOptData.IsOptCenterAddr)
                        txtCenterAddr.Text = string.Format("{0:X2}", GlobalValue.UniSerialPortOptData.CenterAddr);
                    if (GlobalValue.UniSerialPortOptData.IsOptPwd)
                    {
                        txtPwd1.Text = string.Format("{0:X2}", GlobalValue.UniSerialPortOptData.Pwd[1]);
                        txtPwd0.Text = string.Format("{0:X2}", GlobalValue.UniSerialPortOptData.Pwd[0]);
                    }
                    if (GlobalValue.UniSerialPortOptData.IsOptWorkType)
                        cbWorkType.SelectedIndex = GlobalValue.UniSerialPortOptData.WorkType-1;
                    if (GlobalValue.UniSerialPortOptData.IsOptGprsSwitch)
                        toggleGprsSwitch.IsOn = GlobalValue.UniSerialPortOptData.GprsSwitch;
                    if (GlobalValue.UniSerialPortOptData.IsOptClearInterval)
                        txtClearInterval.Text = GlobalValue.UniSerialPortOptData.ClearInterval.ToString();
                    if (GlobalValue.UniSerialPortOptData.IsOptDataInterval)
                        txtDataInterval.Text = GlobalValue.UniSerialPortOptData.DataInterval.ToString();
                    if (GlobalValue.UniSerialPortOptData.IsOptTempUpLimit)
                        txtTempUpLimit.Text = (Convert.ToDouble(GlobalValue.UniSerialPortOptData.TempUpLimit) / 10).ToString("f1");
                    if (GlobalValue.UniSerialPortOptData.IsOptTempLowLimit)
                        txtTempLowLimit.Text = (Convert.ToDouble(GlobalValue.UniSerialPortOptData.TempLowLimit) / 10).ToString("f1");
                    if (GlobalValue.UniSerialPortOptData.IsOptTempAddtion)
                        txtTempAddtion.Text = GlobalValue.UniSerialPortOptData.TempAddtion.ToString();
                    if (GlobalValue.UniSerialPortOptData.IsOptPHUpLimit)
                        txtPHUpLimit.Text = (Convert.ToDouble(GlobalValue.UniSerialPortOptData.PHUpLimit) / 10).ToString("f1");
                    if (GlobalValue.UniSerialPortOptData.IsOptPHLowLimit)
                        txtPHLowLimit.Text = (Convert.ToDouble(GlobalValue.UniSerialPortOptData.PHLowLimit) / 10).ToString("f1");
                    if (GlobalValue.UniSerialPortOptData.IsOptConductivityUpLimit)
                        txtConductivityUpLimit.Text = GlobalValue.UniSerialPortOptData.ConductivityUpLimit.ToString();
                    if (GlobalValue.UniSerialPortOptData.IsOptConductivityLowLimit)
                        txtConductivityLowLimit.Text = GlobalValue.UniSerialPortOptData.ConductivityLowLimit.ToString();
                    if (GlobalValue.UniSerialPortOptData.IsOptTurbidityUpLimit)
                        txtTurbidityUpLimit.Text = GlobalValue.UniSerialPortOptData.TurbidityUpLimit.ToString();
                    if (GlobalValue.UniSerialPortOptData.IsOptTurbidityLowLimit)
                        txtTurbidityLowLimit.Text = GlobalValue.UniSerialPortOptData.TurbidityLowLimit.ToString();
                    if (GlobalValue.UniSerialPortOptData.IsOptPowerSupplyType)
                        cbPowersupplyType.SelectedIndex = GlobalValue.UniSerialPortOptData.PowerSupplyType-1;

                    if (GlobalValue.UniSerialPortOptData.IsOpt_CollectConfig)
                    {
                        cePHState.Checked =GlobalValue.UniSerialPortOptData.Collect_PH;
                        ceConductivityState.Checked = GlobalValue.UniSerialPortOptData.Collect_Conductivity;
                        ceTurbidityState.Checked = GlobalValue.UniSerialPortOptData.Collect_Turbidity;
                    }
                    if (GlobalValue.UniSerialPortOptData.IsOpt_TurbidityInterval)
                        gridControl_Turbidity.DataSource = GlobalValue.UniSerialPortOptData.Turbidity_Interval;

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

            if (ceTerAddr.Checked && !ValidateA5To1())
            {
                XtraMessageBox.Show("请输入正确终端地址!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtA5.Focus();
                return false;
            }

            if (ceCenterAddr.Checked && !Regex.IsMatch(txtCenterAddr.Text, "^[a-zA-Z0-9]{1,2}$"))
            {
                XtraMessageBox.Show("请填写中心站地址!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCenterAddr.Focus();
                return false;
            }

            if (cePwd.Checked && !Regex.IsMatch(txtPwd1.Text, "^[a-zA-Z0-9]{1,2}$") && !Regex.IsMatch(txtPwd1.Text, "^[a-zA-Z0-9]{1,2}$"))
            {
                XtraMessageBox.Show("请输入正确密码!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPwd1.Focus();
                return false;
            }

            if (ceWorkType.Checked && cbWorkType.SelectedIndex < 0)
            {
                XtraMessageBox.Show("请选择工作方式!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbWorkType.Focus();
                return false;
            }

            if (ceClearInterval.Checked && string.IsNullOrEmpty(txtClearInterval.Text))
            {
                XtraMessageBox.Show("请输入清洗间隔!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtClearInterval.Focus();
                return false;
            }

            if (ceDataInterval.Checked && (string.IsNullOrEmpty(txtDataInterval.Text) || Convert.ToUInt16(txtDataInterval.Text) <1 || Convert.ToUInt16(txtDataInterval.Text)>59))
            {
                XtraMessageBox.Show("请输入加报时间间隔!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDataInterval.Focus();
                return false;
            }

            if (ceTempUpLimit.Checked && (string.IsNullOrEmpty(txtTempUpLimit.Text) || Convert.ToSingle(txtTempUpLimit.Text) < 0 || Convert.ToSingle(txtTempUpLimit.Text) > 1000))
            {
                XtraMessageBox.Show("请输入合法的温度上限值!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTempUpLimit.Focus();
                return false;
            }

            if (ceTempLowLimit.Checked && (string.IsNullOrEmpty(txtTempLowLimit.Text) || Convert.ToSingle(txtTempLowLimit.Text) < 0 || Convert.ToSingle(txtTempLowLimit.Text) > 1000))
            {
                XtraMessageBox.Show("请输入合法的温度下限值!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTempLowLimit.Focus();
                return false;
            }

            if (ceTempAddtion.Checked && string.IsNullOrEmpty(txtTempAddtion.Text))
            {
                XtraMessageBox.Show("请输入合法的温度加报阀值!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTempAddtion.Focus();
                return false;
            }

            if (cePHUpLimit.Checked && (string.IsNullOrEmpty(txtPHUpLimit.Text) || Convert.ToSingle(txtPHUpLimit.Text) < 0 || Convert.ToSingle(txtPHUpLimit.Text) > 14))
            {
                XtraMessageBox.Show("请输入合法的PH上限值(<=14)!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPHUpLimit.Focus();
                return false;
            }

            if (cePHLowLimit.Checked && (string.IsNullOrEmpty(txtPHLowLimit.Text) || Convert.ToSingle(txtPHLowLimit.Text) < 0 || Convert.ToSingle(txtPHLowLimit.Text) > 14))
            {
                XtraMessageBox.Show("请输入合法的PH下限值(<=14)!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPHLowLimit.Focus();
                return false;
            }

            if (ceConductivityUpLimit.Checked && string.IsNullOrEmpty(txtConductivityUpLimit.Text))
            {
                XtraMessageBox.Show("请输入合法的电导率上限值!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtConductivityUpLimit.Focus();
                return false;
            }

            if (ceConductivityLowLimit.Checked && string.IsNullOrEmpty(txtConductivityLowLimit.Text))
            {
                XtraMessageBox.Show("请输入合法的电导率下限值!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtConductivityLowLimit.Focus();
                return false;
            }

            if (ceTurbidityUpLimit.Checked && string.IsNullOrEmpty(txtTurbidityUpLimit.Text))
            {
                XtraMessageBox.Show("请输入合法的浊度上限值!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTurbidityUpLimit.Focus();
                return false;
            }

            if (ceTurbidityLowLimit.Checked && string.IsNullOrEmpty(txtTurbidityLowLimit.Text))
            {
                XtraMessageBox.Show("请输入合法的浊度下限值!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTurbidityLowLimit.Focus();
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

            if (GlobalValue.UniSerialPortOptData == null)
                GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity();
            GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

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
