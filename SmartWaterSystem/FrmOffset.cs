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
using Entity;
using System.Text.RegularExpressions;

namespace SmartWaterSystem
{
    public partial class FrmOffset : DevExpress.XtraEditors.XtraForm
    {
        DataTable dt = new DataTable("OffsetTable");

        public FrmOffset()
        {
            InitializeComponent();
        }

        private void FrmOffset_Load(object sender, EventArgs e)
        {
            dt.Columns.Add("Id");
            dt.Columns.Add("TerType");
            dt.Columns.Add("FunCode");
            dt.Columns.Add("OffsetValue");

            try {
                
                gridControl1.DataSource = dt;

                InitGridView();
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show("初始化失败!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitGridView()
        {
            cb_tertype.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            EnumHelper enumhelp = new EnumHelper();
            foreach(int dev in Enum.GetValues(typeof(ConstValue.DEV_TYPE)))
            {
                cb_tertype.Items.Add(enumhelp.GetEnumDescription((ConstValue.DEV_TYPE)dev));
            }
            txt_id.KeyPress += new KeyPressEventHandler(txt_threebyte_KeyPress);
            txt_funcode.KeyPress += new KeyPressEventHandler(txt_onebyte_KeyPress);
        }

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
                    text = text.Remove(txtbox.SelectionStart, txtbox.SelectionLength);

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
        /// 限制最大3byte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txt_threebyte_KeyPress(object sender, KeyPressEventArgs e)
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
                if ((Regex.IsMatch(text, @"^\d{1,8}$") && Convert.ToInt32(text) <= 16777215)
                || Regex.IsMatch(text, @"^(0x|0x[0-9a-fA-F]{1,6})$"))
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

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try {
                DataTable dt = gridControl1.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0)
                {
                    XtraMessageBox.Show("请输入至少一条数据!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                XtraMessageBox.Show("设置成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show("保存时发生异常!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                int hRowHandle = this.gridView1.FocusedRowHandle;
                if (hRowHandle < 0)
                    return;

                if (DialogResult.Yes == XtraMessageBox.Show("是否删除?", GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                {
                    gridView1.DeleteRow(hRowHandle);
                    e.Handled = true;
                }
            }
        }
    }
}
