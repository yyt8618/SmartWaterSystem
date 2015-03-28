using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Common;

namespace SmartWaterSystem
{
    public partial class FrmSerialComSet : DevExpress.XtraEditors.XtraForm
    {
        public FrmSerialComSet()
        {
            InitializeComponent();

            #region Init cbBaudRate
            cbBaudRate.Properties.Items.AddRange(new int[]{300,600,1200,2400,4800,9600,14400,19200,38400,56000,57600,115200,128000,256000});
            cbBaudRate.Properties.Items.Add("customiz.");
            #endregion

            #region Init cbParity
            cbParity.Properties.Items.AddRange(new string[] {"NONE","ODD","EVEN","MARK","SPACE" });
            #endregion

            #region Init cbDataPos
            cbDataPos.Properties.Items.AddRange(new[] { 5, 6, 7, 8 });
            #endregion
            #region Init cbStopPos
            cbStopPos.Properties.Items.AddRange(new string[] { "NONE", "ONE", "TWO", "OnePointFive" });
            #endregion
        }

        private void FrmSerialComSet_Load(object sender, EventArgs e)
        {
            #region Init cbSerialPort
            cbSerialPort.Properties.DataSource = null;
            cbSerialPort.Properties.Columns.Clear();
            cbSerialPort.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cbSerialPort.Properties.ShowHeader = false;
            cbSerialPort.Properties.ShowFooter = false;

            List<Protocol.SerialInfoEntity> lstSerial = Protocol.SerialPortUtil.GetInstance().MulGetHardwareInfo(Protocol.SerialPortUtil.HardwareEnum.Win32_PnPEntity);
            //List<Protocol.SerialInfoEntity> lstpnp = Protocol.SerialPortUtil.GetInstance().MulGetHardwareInfo(Protocol.SerialPortUtil.HardwareEnum.Win32_PnPEntity);
            //if (lstpnp != null && lstpnp.Count > 0)
            //{
            //    lstSerial.AddRange(lstpnp);
            //}
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
            #endregion

            //cbBaudRate.SelectedItem = Settings.Instance.GetString(SettingKeys.BaudRate");
            SetComboboxIndex(cbBaudRate, Settings.Instance.GetString(SettingKeys.BaudRate));
            SetComboboxIndex(cbParity, Settings.Instance.GetString(SettingKeys.Parity));
            SetComboboxIndex(cbDataPos, Settings.Instance.GetString(SettingKeys.DataBits));
            SetComboboxIndex(cbStopPos, Settings.Instance.GetString(SettingKeys.StopBits));
            txtTime.Text = Settings.Instance.GetString(SettingKeys.TimeOut);
        }

        private void SetComboboxIndex(DevExpress.XtraEditors.ComboBoxEdit control, string selectvalue)
        {
            if (control.Properties.Items != null && control.Properties.Items.Count > 0)
            {
                DevExpress.XtraEditors.Controls.ComboBoxItemCollection cbSelectItems = control.Properties.Items;
                for (int i = 0; i < cbSelectItems.Count; i++ )
                {
                    if (cbSelectItems[i].ToString() == selectvalue)
                        control.SelectedIndex = i;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if((!Regex.IsMatch(txtTime.Text,@"^\d+$"))  || Convert.ToInt32(txtTime.Text) < 2 || Convert.ToInt32(txtTime.Text)>61)
                {

                    DevExpress.XtraEditors.XtraMessageBox.Show("请设置超时时间(3-60seconds)", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtTime.SelectAll();
                    txtTime.Focus();
                    return;
                }
                if (cbSerialPort.EditValue == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("请选择串口", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbSerialPort.Focus();
                    return;
                }
                if (!Regex.IsMatch(cbBaudRate.SelectedItem.ToString(), @"^\d+$"))
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("请设置串口波特率", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbBaudRate.Focus();
                    return;
                }

                if (cbParity.SelectedItem == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("请设置校验位", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbParity.Focus();
                    return;
                }

                if (cbDataPos.SelectedItem == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("请设置数据位", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbDataPos.Focus();
                    return;
                }
                if(cbStopPos.SelectedItem == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("请设置停止位",GlobalValue.Text,MessageBoxButtons.OK,MessageBoxIcon.Information);
                    cbStopPos.Focus();
                    return;
                }

                Settings.Instance.SetValue(SettingKeys.SerialPort, cbSerialPort.EditValue.ToString());
                Settings.Instance.SetValue(SettingKeys.BaudRate, cbBaudRate.SelectedItem.ToString());
                Settings.Instance.SetValue(SettingKeys.Parity, cbParity.SelectedItem.ToString());
                Settings.Instance.SetValue(SettingKeys.DataBits, cbDataPos.SelectedItem.ToString());
                Settings.Instance.SetValue(SettingKeys.StopBits, cbStopPos.SelectedItem.ToString());
                Settings.Instance.SetValue(SettingKeys.TimeOut, txtTime.Text);
                DevExpress.XtraEditors.XtraMessageBox.Show("保存成功，请重新打开串口！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("保存数据失败!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbBaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbBaudRate.Text.Trim() == "customiz.")
            {
                cbBaudRate.Text = "";
            }
        }

        
    }
}
