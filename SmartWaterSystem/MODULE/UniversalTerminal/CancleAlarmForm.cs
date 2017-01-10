using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Text.RegularExpressions;

namespace SmartWaterSystem
{
    public partial class CancleAlarmForm : DevExpress.XtraEditors.XtraForm
    {
        public bool IsEnable = false;   //是否启用报警
        public int CancleHours = 0; //取消时长

        public CancleAlarmForm()
        {
            InitializeComponent();
        }

        private void CancleAlarmForm_Load(object sender, EventArgs e)
        {
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ceCancleAlarmLen.Enabled && ceCancleAlarmLen.Checked)
            {
                if (!Regex.IsMatch(txtCancleAlarmLen.Text, @"^[1-9](\d{1,3})?$"))
                {
                    XtraMessageBox.Show("请填写取消报警时长!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    txtCancleAlarmLen.Focus();
                    return;
                }
                CancleHours = Convert.ToInt32(txtCancleAlarmLen.Text);
            }
            else
                CancleHours = -1;  //未设置

            IsEnable = rgEnableAlarm.SelectedIndex == 0 ? true : false;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        
        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rgEnableAlarm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(rgEnableAlarm.SelectedIndex == 0)
            {
                ceCancleAlarmLen.Enabled = false;
                txtCancleAlarmLen.Enabled = false;
            }
            else if(rgEnableAlarm.SelectedIndex == 1)
            {
                ceCancleAlarmLen.Enabled = true;
                txtCancleAlarmLen.Enabled = true;
            }
        }

        private void txtCancleAlarmLen_EditValueChanged(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtCancleAlarmLen.Text))
            {
                ceCancleAlarmLen.Checked = false;
            }
            else
            {
                ceCancleAlarmLen.Checked = true;
            }
        }
    }
}
