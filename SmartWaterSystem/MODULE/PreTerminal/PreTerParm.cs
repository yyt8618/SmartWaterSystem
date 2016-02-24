using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Common;
using System.Text.RegularExpressions;

namespace SmartWaterSystem
{
    public partial class PreTerParm : BaseView,IPreTerParm
    {
        public PreTerParm()
        {
            InitializeComponent();
        }

        private void PreTerParm_Load(object sender, EventArgs e)
        {
            SetSerialPortCtrlStatus();
        }

        public override void SerialPortEvent(bool Enabled)
        {
            if (SwitchComunication.IsOn)  //串口
            {
                ButtonEnabled(Enabled);
            }
        }

        private void ButtonEnabled(bool Enabled)
        {
            btnGetRealtimeData.Enabled = Enabled;
            btnHisotryData.Enabled = Enabled;
            btnReset.Enabled = Enabled;
            btnEnableCollect.Enabled = Enabled;
            btnCheckingTime.Enabled = Enabled;
            btnReadParm.Enabled = Enabled;
            btnSetParm.Enabled = Enabled;
        }

        private void SetGprsCtrlStatus()
        {
            ButtonEnabled(true);
        }

        private void SetSerialPortCtrlStatus()
        {
            ButtonEnabled(GlobalValue.portUtil.IsOpen);
        }

        private void EnableControls(bool enable)
        {
            btnGetRealtimeData.Enabled = Enabled;
            btnHisotryData.Enabled = Enabled;
            btnReset.Enabled = enable;
            btnCheckingTime.Enabled = enable;
            btnEnableCollect.Enabled = enable;
            btnReadParm.Enabled = enable;
            btnSetParm.Enabled = enable;
            SwitchComunication.Enabled = enable;
        }

        void SerialPortMgr_SerialPortScheduleEvent(object sender, SerialPortScheduleEventArgs e)
        {
            if ((e.OptType == SerialPortType.PreReadInfo || e.OptType == SerialPortType.PreReadInfo) && !string.IsNullOrEmpty(e.Msg))
            {
                ShowWaitForm("", e.Msg);
                SetStaticItem(e.Msg);
            }
        }

        private void btnReadParm_Click(object sender, EventArgs e)
        {
            if (SwitchComunication.IsOn)
            {
                GlobalValue.PreSerialPortOptData = new Entity.PreSerialPortOptEntity();
                bool haveread = false;
                if (cbID.Checked)
                {
                    GlobalValue.PreSerialPortOptData.IsOptID = cbID.Checked;
                    haveread = true;
                }
                else
                {
                    if (!Regex.IsMatch(txtID.Text, @"^\d{1,5}$"))
                    {
                        XtraMessageBox.Show("请填写设备编号!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtID.Focus();
                        return;
                    }
                    GlobalValue.PreSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

                    if (cbTime.Checked)
                    {
                        GlobalValue.PreSerialPortOptData.IsOptDT = true;
                        haveread = true;
                    }
                    if (cbVoltageInterval.Checked)
                    {
                        GlobalValue.PreSerialPortOptData.IsOptVoltageInterval = true;
                        haveread = true;
                    }
                    if (cbVoltageAlarmLowLimit.Checked)
                    {
                        GlobalValue.PreSerialPortOptData.IsOptVoltageLowLimit = true;
                        haveread = true;
                    }
                    if (cbCollectConfig.Checked)
                    {
                        GlobalValue.PreSerialPortOptData.IsOptCollectConfig = true;
                        haveread = true;
                    }
                    if (cbConnectType.Checked)
                    {
                        GlobalValue.PreSerialPortOptData.IsOptConnectType = true;
                        haveread = true;
                    }

                    if (cbIP.Checked)
                    {
                        GlobalValue.PreSerialPortOptData.IsOptIP = true;
                        haveread = true;
                    }
                    if (cbPort.Checked)
                    {
                        GlobalValue.PreSerialPortOptData.IsOptPort = true;
                        haveread = true;
                    }

                    if (cbPreInterval.Checked)
                    {
                        GlobalValue.PreSerialPortOptData.IsOptPreInterval = true;
                        haveread = true;
                    }

                    if (comboPreFlag.SelectedIndex > -1)
                    {
                        if (cbPreUpLimit.Checked)
                        {
                            GlobalValue.PreSerialPortOptData.IsOptPreUpLimit = true;
                            haveread = true;
                        }
                        if (cbEnablePreUpLimit.Checked)
                        {
                            GlobalValue.PreSerialPortOptData.IsOptEnablePreUpLimit = true;
                            haveread = true;
                        }
                        if (cbPreLowLimit.Checked)
                        {
                            GlobalValue.PreSerialPortOptData.IsOptPreLowLimit = true;
                            haveread = true;
                        }
                        if (cbEnablePreLowLimit.Checked)
                        {
                            GlobalValue.PreSerialPortOptData.IsOptEnablePreLowLimit = true;
                            haveread = true;
                        }
                        if (cbSlopUpLimit.Checked)
                        {
                            GlobalValue.PreSerialPortOptData.IsOptSlopUpLimit = true;
                            haveread = true;
                        }
                        if (cbEnableSlopUpLimit.Checked)
                        {
                            GlobalValue.PreSerialPortOptData.IsOptEnableSlopUpLimit = true;
                            haveread = true;
                        }
                        if (cbSlopLowLimit.Checked)
                        {
                            GlobalValue.PreSerialPortOptData.IsOptSlopLowLimit = true;
                            haveread = true;
                        }
                        if (cbEnableSlopLowLimit.Checked)
                        {
                            GlobalValue.PreSerialPortOptData.IsOptEnableSlopLowLimit = true;
                            haveread = true;
                        }
                        if (cbOffset.Checked)
                        {
                            GlobalValue.PreSerialPortOptData.IsOptOffset = true;
                            haveread = true;
                        }
                        if (cbPreRange.Checked)
                        {
                            GlobalValue.PreSerialPortOptData.IsOptPreRange = true;
                            haveread = true;
                        }
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
                GlobalValue.SerialPortMgr.Send(SerialPortType.PreReadInfo);
            }
        }

        private void btnSetParm_Click(object sender, EventArgs e)
        {

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == XtraMessageBox.Show("是否复位设备?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                return;
        }

        private void btnEnableCollect_Click(object sender, EventArgs e)
        {

        }

        private void btnCheckingTime_Click(object sender, EventArgs e)
        {

        }

        


    }
}
