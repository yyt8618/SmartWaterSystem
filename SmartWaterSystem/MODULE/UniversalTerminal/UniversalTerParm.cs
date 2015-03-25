using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Text.RegularExpressions;
using DevExpress.XtraGrid.Views.Grid;

namespace SmartWaterSystem
{
    public partial class UniversalTerParm : BaseView,IUniversalTerParm
    {
        public UniversalTerParm()
        {
            InitializeComponent();

            #region Init cbBaudRate
            cbBaudRate.Properties.Items.AddRange(new int[] { 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 56000, 57600, 115200, 128000, 256000 });
            cbBaudRate.Properties.Items.Add("customiz.");
            #endregion
        }

        //public UniversalTerParm(FrmSystem parentform)
        //{
        //    InitializeComponent();
        //}

        private void UniversalTerParm_Load(object sender, EventArgs e)
        {
            ceColConfig.Checked = false;

            Init();
        }

        private void cbBaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbBaudRate.Text.Trim() == "customiz.")
            {
                cbBaudRate.Text = "";
            }
        }

        private void Init()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextEdit txtbox = (TextEdit)sender;
            int index = Convert.ToInt32(txtbox.Tag);
            if (e.KeyChar == '\b') //backspace
            {
                if (txtbox.Text.Length == 0)
                {
                    if (2 == index)
                    {
                        txtNum1.Focus();
                        txtNum1.SelectionStart = txtNum1.Text.Length;
                    }
                    else if (3 == index)
                    {
                        txtNum2.Focus();
                        txtNum2.SelectionStart = txtNum2.Text.Length;
                    }
                    else if (4 == index)
                    {
                        txtNum3.Focus();
                        txtNum3.SelectionStart = txtNum3.Text.Length;
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
                if (txtbox.Text.Length == 3)
                {
                    if (1 == index)
                    {
                        txtNum2.Text = e.KeyChar.ToString();
                        txtNum2.Focus();
                        txtNum2.SelectionStart = txtNum2.Text.Length;
                    }
                    else if (2 == index)
                    {
                        txtNum3.Text = e.KeyChar.ToString();
                        txtNum3.Focus();
                        txtNum3.SelectionStart = txtNum3.Text.Length;
                    }
                    else if (3 == index)
                    {
                        txtNum4.Text = e.KeyChar.ToString();
                        txtNum4.Focus();
                        txtNum4.SelectionStart = txtNum4.Text.Length;
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

        int rowindex = -1;
        private void gridView3_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            rowindex = e.RowHandle;
            if ((!string.IsNullOrEmpty(gridView3.GetRowCellValue(rowindex, "starttime").ToString())) &&
                (!string.IsNullOrEmpty(gridView3.GetRowCellValue(rowindex, "collecttime").ToString())) &&
                (!string.IsNullOrEmpty(gridView3.GetRowCellValue(rowindex, "sendtime").ToString())) &&
                (gridView3.RowCount < 6))
            {
                cbStartTime.Items.Clear();
                if ((gridView3.RowCount > rowindex + 1) && (rowindex > -1))
                {
                    int starttime = 0;
                    if ((rowindex - 1) > -1)
                        starttime = Convert.ToInt32(gridView3.GetRowCellValue(rowindex - 1, "starttime"));
                    if (starttime < 0)
                        starttime = 0;
                    int endtime = Convert.ToInt32(gridView3.GetRowCellValue(rowindex+1, "starttime"));
                    for (int i=starttime; i < endtime; i++)
                    {
                        cbStartTime.Items.Add(i);
                    }
                }
                else
                {
                    int i = Convert.ToInt32(gridView3.GetRowCellValue(rowindex, "starttime")) + 1;
                    for (; i < 24; i++)
                    {
                        cbStartTime.Items.Add(i);
                    }
                }
                gridView3.AddNewRow();
            }
        }

        private void gridView3_RowCellClick(object sender, RowCellClickEventArgs e)
        {
        }

        private void gridView3_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                rowindex = e.RowHandle;
            }
        }
    }
}
