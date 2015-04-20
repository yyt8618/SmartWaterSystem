using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Text.RegularExpressions;

namespace SmartWaterSystem
{
    public partial class PluseBasicInputForm : DevExpress.XtraEditors.XtraForm
    {

        public PluseBasicInputForm()
        {
            InitializeComponent();
        }

        private void PluseBasicInputForm_Load(object sender, EventArgs e)
        {
            txtPluseBasic1.Focus();
        }

        public bool SetPluseBasic1 = false;
        public bool SetPluseBasic2 = false;
        public bool SetPluseBasic3 = false;
        public bool SetPluseBasic4 = false;

        public UInt32 PluseBasic1 = 0;
        public UInt32 PluseBasic2 = 0;
        public UInt32 PluseBasic3 = 0;
        public UInt32 PluseBasic4 = 0;

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPluseBasic1.Text)&&string.IsNullOrEmpty(txtPluseBasic2.Text)&&
                string.IsNullOrEmpty(txtPluseBasic3.Text)&&string.IsNullOrEmpty(txtPluseBasic4.Text) )
            {
                XtraMessageBox.Show("请输入基准数!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPluseBasic1.Focus();
                return;
            }

            if (!string.IsNullOrEmpty(txtPluseBasic1.Text) && Convert.ToUInt32(txtPluseBasic1.Text) >= 0)
            {
                SetPluseBasic1 = true;
                PluseBasic1 = Convert.ToUInt32(txtPluseBasic1.Text);
            }
            if (!string.IsNullOrEmpty(txtPluseBasic2.Text) && Convert.ToUInt32(txtPluseBasic2.Text) >= 0)
            {
                SetPluseBasic2 = true;
                PluseBasic2 = Convert.ToUInt32(txtPluseBasic2.Text);
            }
            if (!string.IsNullOrEmpty(txtPluseBasic3.Text) && Convert.ToUInt32(txtPluseBasic3.Text) >= 0)
            {
                SetPluseBasic3 = true;
                PluseBasic3 = Convert.ToUInt32(txtPluseBasic3.Text);
            }
            if (!string.IsNullOrEmpty(txtPluseBasic4.Text) && Convert.ToUInt32(txtPluseBasic4.Text) >= 0)
            {
                SetPluseBasic4 = true;
                PluseBasic4 = Convert.ToUInt32(txtPluseBasic4.Text);
            }

            if (!SetPluseBasic1 && !SetPluseBasic2 && !SetPluseBasic3 && !SetPluseBasic4)
            {
                XtraMessageBox.Show("请输入至少一个基准数!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPluseBasic1.Focus();
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 限制最大8byte (<=4294967294)
        /// </summary>
        void txt_fourbyte_KeyPress(object sender, KeyPressEventArgs e)
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
                    text = text.Remove(txtbox.SelectionStart, txtbox.SelectionLength);
                    text = text.Insert(txtbox.SelectionStart, e.KeyChar.ToString());
                }
                text = text.Insert(txtbox.SelectionStart, e.KeyChar.ToString());
                if ((Regex.IsMatch(text, @"^\d{1,10}$") && Convert.ToUInt32(text) <= 4294967294)
                    || Regex.IsMatch(text, @"^(0x|0x[0-9a-fA-F]{1,10})$"))
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

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
