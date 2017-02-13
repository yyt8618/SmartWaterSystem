using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AboutTime
{
    public partial class HostForm : Form
    {
        public static string name = "";
        public static string addr = "";
        public static string port = "";

        public HostForm()
        {
            InitializeComponent();
        }

        private void HostForm_Load(object sender, EventArgs e)
        {
            txtName.Text = name;
            txtAddr.Text = addr;
            txtPort.Text = port;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("请输入主机名称", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return;
            }
            if(string.IsNullOrEmpty(txtAddr.Text))
            {
                MessageBox.Show("请输入地址", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAddr.Focus();
                return;
            }
            if(!Regex.IsMatch(txtPort.Text,"^\\d{1,6}$"))
            {
                MessageBox.Show("请输入端口号", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPort.Focus();
                return;
            }
            name = txtName.Text.Trim();
            addr = txtAddr.Text.Trim();
            port = txtPort.Text.Trim();
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
