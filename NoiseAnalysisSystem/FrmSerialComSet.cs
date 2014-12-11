using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NoiseAnalysisSystem
{
    public partial class FrmSerialComSet : DevExpress.XtraEditors.XtraForm
    {
        public FrmSerialComSet()
        {
            InitializeComponent();

            cbSerialPort.Properties.DataSource = null;
            cbSerialPort.Properties.Columns.Clear();
            cbSerialPort.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cbSerialPort.Properties.ShowHeader = false;
            cbSerialPort.Properties.ShowFooter = false;
            
            List<Protocol.SerialInfoEntity> lstSerial=Protocol.SerialPortUtil.GetInstance().MulGetHardwareInfo(Protocol.SerialPortUtil.HardwareEnum.Win32_SerialPort);
            if (lstSerial != null && lstSerial.Count > 0)
            {
                cbSerialPort.Properties.NullText = "--请选择--";

                cbSerialPort.Properties.DataSource = lstSerial;
                cbSerialPort.Properties.DisplayMember = "SerialFullName";
                cbSerialPort.Properties.ValueMember = "SerialName";

                cbSerialPort.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("SerialFullName"));
            }
            else
            {
                cbSerialPort.Properties.DataSource = null;
                cbSerialPort.Properties.NullText = "--没有数据--";
            }

            cbSerialPort.Properties.BestFit();
            //cbSerialPort.SelectedItem = AppConfigHelper.GetAppSettingValue("SerialPort");
            cbBaudRate.Text = AppConfigHelper.GetAppSettingValue("BaudRate");
            txtTime.Text = AppConfigHelper.GetAppSettingValue("TimeOut");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cbSerialPort.EditValue == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("请选择串口", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbSerialPort.Focus();
                return;
            }
            AppConfigHelper.SetAppSettingValue("SerialPort", cbSerialPort.EditValue.ToString());
            AppConfigHelper.SetAppSettingValue("BaudRate", cbBaudRate.SelectedItem.ToString());
            AppConfigHelper.SetAppSettingValue("TimeOut", txtTime.Text);
            MessageBox.Show("保存成功，请重新打开串口！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
			this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
