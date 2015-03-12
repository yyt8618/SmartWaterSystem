using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Common
{
    public partial class IPControl : UserControl
    {
        public string IP
        {
            get { return GetIpAddress(); }
        }
        public string Text
        {
            get { return GetIpAddress(); }
        }

        public IPControl()
        {
            InitializeComponent();
        }

        private void IPControl_Load(object sender, EventArgs e)
        {
            //txtNum1.Focus();
        }

        private string GetIpAddress()
        {
            if ((!string.IsNullOrEmpty(txtNum1.Text)) && (!string.IsNullOrEmpty(txtNum2.Text)) &&
                (!string.IsNullOrEmpty(txtNum3.Text)) && (!string.IsNullOrEmpty(txtNum4.Text)))
            {
                return txtNum1.Text + "." + txtNum2.Text + "." + txtNum3.Text + "." + txtNum4.Text;
            }
            else
                return "";
        }

        private void txtNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txtbox = (TextBox)sender;
            int index = Convert.ToInt32(txtbox.Tag);
            if (e.KeyChar == '\b') //backspace
            {
                if (txtbox.TextLength == 0)
                {
                    if (2 == index)
                    {
                        txtNum1.Focus();
                        txtNum1.SelectionStart = txtNum1.TextLength;
                    }
                    else if (3 == index)
                    {
                        txtNum2.Focus();
                        txtNum2.SelectionStart = txtNum2.TextLength;
                    }
                    else if (4 == index)
                    {
                        txtNum3.Focus();
                        txtNum3.SelectionStart = txtNum3.TextLength;
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
                if (txtbox.TextLength == 3)
                {
                    if (1 == index)
                    {
                        txtNum2.Text = e.KeyChar.ToString();
                        txtNum2.Focus();
                        txtNum2.SelectionStart = txtNum2.TextLength;
                    }
                    else if (2 == index)
                    {
                        txtNum3.Text = e.KeyChar.ToString();
                        txtNum3.Focus();
                        txtNum3.SelectionStart = txtNum3.TextLength;
                    }
                    else if (3 == index)
                    {
                        txtNum4.Text = e.KeyChar.ToString();
                        txtNum4.Focus();
                        txtNum4.SelectionStart = txtNum4.TextLength;
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

        
    }
}
