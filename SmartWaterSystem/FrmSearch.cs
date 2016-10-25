using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SmartWaterSystem
{
    public partial class FrmSearch : DevExpress.XtraEditors.XtraForm
    {
        public FrmConsole frmCtrl = null;
        public FrmSearch()
        {
            InitializeComponent();
        }

        public string KeyText
        {
            set {
                string strvalue = value;
                strvalue= strvalue.Trim(new char[] { '\r', '\n' });
                txtContent.Text = strvalue;
            }
        }

        private void FrmSearch_Load(object sender, EventArgs e)
        {
            btnSearchNext.Enabled = !string.IsNullOrEmpty(txtContent.Text);
        }

        private void btnSearchNext_Click(object sender, EventArgs e)
        {
            if(frmCtrl!=null)
            {
                string errmsg= frmCtrl.FindText(txtContent.Text, rdReverse.SelectedIndex == 0 ? RichTextBoxFinds.Reverse: RichTextBoxFinds.None, ceCase.Checked);
                if (!string.IsNullOrEmpty(errmsg))
                {
                    XtraMessageBox.Show(errmsg, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void txtContent_EditValueChanged(object sender, EventArgs e)
        {
            btnSearchNext.Enabled = !string.IsNullOrEmpty(txtContent.Text);
        }

        private void FrmSearch_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
