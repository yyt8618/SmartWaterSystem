using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.IO;
using Common;
using DevExpress.XtraEditors;

namespace SmartWaterSystem
{
    public partial class FrmMSMQSet : DevExpress.XtraEditors.XtraForm
    {
        string[] str;
        public FrmMSMQSet()
        {
            InitializeComponent();
        }

        private void FrmMSMQSet_Load(object sender, EventArgs e)
        {
            txtIPAddr.Text = Settings.Instance.GetString(SettingKeys.MSMQIpAddr);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            System.Net.IPAddress ipaddr ;
            if (System.Net.IPAddress.TryParse(txtIPAddr.Text, out ipaddr))
            {
                Settings.Instance.SetValue(SettingKeys.MSMQIpAddr, ipaddr.ToString());
                XtraMessageBox.Show("IP设置成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                XtraMessageBox.Show("IP地址输入错误!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtIPAddr.Focus();
            }
        }


    }
}
