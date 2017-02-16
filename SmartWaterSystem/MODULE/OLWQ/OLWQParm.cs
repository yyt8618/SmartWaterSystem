using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Text.RegularExpressions;
using DevExpress.XtraGrid.Views.Grid;
using Entity;
using System.Collections;
using BLL;
using Common;
using System.Collections.Generic;

namespace SmartWaterSystem
{
    public partial class OLWQParm : BaseView, IOLWQParm
    {
        public OLWQParm()
        {
            InitializeComponent();

            #region Init ResidualCl GridView
            cb_ResidualCl_starttime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            for (int i = 0; i < 24; i++)
                cb_ResidualCl_starttime.Items.Add(i);

            cb_ResidualCl_coltime1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_ResidualCl_coltime1.Items.Add(1);
            cb_ResidualCl_coltime1.Items.Add(5);
            cb_ResidualCl_coltime1.Items.Add(15);
            cb_ResidualCl_coltime1.Items.Add(30);
            cb_ResidualCl_coltime1.Items.Add(60);
            cb_ResidualCl_coltime1.Items.Add(120);

            cb_ResidualCl_sendtime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_ResidualCl_sendtime.Items.Add(5);
            cb_ResidualCl_sendtime.Items.Add(15);
            cb_ResidualCl_sendtime.Items.Add(30);
            cb_ResidualCl_sendtime.Items.Add(60);
            cb_ResidualCl_sendtime.Items.Add(120);
            cb_ResidualCl_sendtime.Items.Add(240);
            cb_ResidualCl_sendtime.Items.Add(480);
            cb_ResidualCl_sendtime.Items.Add(720);
            cb_ResidualCl_sendtime.Items.Add(1440);
            #endregion

            #region Init Turbidity GridView
            cb_Turbidity_starttime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            for (int i = 0; i < 24; i++)
                cb_Turbidity_starttime.Items.Add(i);

            cb_Turbidity_coltime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_Turbidity_coltime.Items.Add(1);
            cb_Turbidity_coltime.Items.Add(5);
            cb_Turbidity_coltime.Items.Add(15);
            cb_Turbidity_coltime.Items.Add(30);
            cb_Turbidity_coltime.Items.Add(60);
            cb_Turbidity_coltime.Items.Add(120);

            cb_Turbidity_sendtime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_Turbidity_sendtime.Items.Add(5);
            cb_Turbidity_sendtime.Items.Add(15);
            cb_Turbidity_sendtime.Items.Add(30);
            cb_Turbidity_sendtime.Items.Add(60);
            cb_Turbidity_sendtime.Items.Add(120);
            cb_Turbidity_sendtime.Items.Add(240);
            cb_Turbidity_sendtime.Items.Add(480);
            cb_Turbidity_sendtime.Items.Add(720);
            cb_Turbidity_sendtime.Items.Add(1440);
            #endregion

            #region Init PH GridView
            cb_PH_starttime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            for (int i = 0; i < 24; i++)
                cb_PH_starttime.Items.Add(i);
            
            cb_PH_coltime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_PH_coltime.Items.Add(1);
            cb_PH_coltime.Items.Add(5);
            cb_PH_coltime.Items.Add(15);
            cb_PH_coltime.Items.Add(30);
            cb_PH_coltime.Items.Add(60);
            cb_PH_coltime.Items.Add(120);

            cb_PH_sendtime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_PH_sendtime.Items.Add(5);
            cb_PH_sendtime.Items.Add(15);
            cb_PH_sendtime.Items.Add(30);
            cb_PH_sendtime.Items.Add(60);
            cb_PH_sendtime.Items.Add(120);
            cb_PH_sendtime.Items.Add(240);
            cb_PH_sendtime.Items.Add(480);
            cb_PH_sendtime.Items.Add(720);
            cb_PH_sendtime.Items.Add(1440);
            #endregion

            #region Init Conductivity GridView
            cb_Conductivity_starttime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            for (int i = 0; i < 24; i++)
                cb_Conductivity_starttime.Items.Add(i);

            cb_Conductivity_coltime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_Conductivity_coltime.Items.Add(1);
            cb_Conductivity_coltime.Items.Add(5);
            cb_Conductivity_coltime.Items.Add(15);
            cb_Conductivity_coltime.Items.Add(30);
            cb_Conductivity_coltime.Items.Add(60);
            cb_Conductivity_coltime.Items.Add(120);

            cb_Conductivity_sendtime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_Conductivity_sendtime.Items.Add(5);
            cb_Conductivity_sendtime.Items.Add(15);
            cb_Conductivity_sendtime.Items.Add(30);
            cb_Conductivity_sendtime.Items.Add(60);
            cb_Conductivity_sendtime.Items.Add(120);
            cb_Conductivity_sendtime.Items.Add(240);
            cb_Conductivity_sendtime.Items.Add(480);
            cb_Conductivity_sendtime.Items.Add(720);
            cb_Conductivity_sendtime.Items.Add(1440);
            #endregion
        }

        private void OLWQParm_Load(object sender, EventArgs e)
        {
            InitGridView();
            InitControls();

            cbPowersupplyType.SelectedIndex = 0;
        }


        private void InitGridView()
        {
            gridControl_ResidualCl.DataSource = null;
            gridControl_Turbidity.DataSource = null;
            gridControl_PH.DataSource = null;
            gridControl_Conductivity.DataSource = null;
        }

        private void InitControls()
        {
            ceColConfig.Checked = false;

            ceResidualClInterval.Checked = false;
            ceTurbidityInterval.Checked = false;
            cePHInterval.Checked = false;
            ceConductivityInterval.Checked = false;
            gridControl_ResidualCl.Enabled = false;
            gridControl_Turbidity.Enabled = false;
            gridControl_PH.Enabled = false;
            gridControl_Conductivity.Enabled = false;
        }

        #region IP
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
        #endregion

        #region 1-2byte unshort txt Event
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
                    text =text.Remove(txtbox.SelectionStart, txtbox.SelectionLength);
                    
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
        /// 限制最大2byte (<=65535)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txt_twobyte_KeyPress(object sender, KeyPressEventArgs e)
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
            else if ((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar >= 65 && e.KeyChar <= 70) || (e.KeyChar >= 97 && e.KeyChar <= 102) || e.KeyChar==120)
            {
                string text = txtbox.Text;
                if (txtbox.SelectedText.Length > 0)
                {
                    text = text.Remove(txtbox.SelectionStart, txtbox.SelectionLength);
                    text = text.Insert(txtbox.SelectionStart, e.KeyChar.ToString());
                }
                text = text.Insert(txtbox.SelectionStart, e.KeyChar.ToString());
                if ((Regex.IsMatch(text, @"^\d{1,5}$") && Convert.ToInt32(text) <= 65535)
                || Regex.IsMatch(text, @"^(0x|0x[0-9a-fA-F]{1,4})$"))
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
        #endregion

        private void gridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator)
            {
                e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        #region ResidualCl GridView
        int ResidualClrowindex = -1;
        private void gridView_ResidualCl_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                GridView gridview = sender as GridView;
                if ((!string.IsNullOrEmpty(gridview.GetRowCellValue(e.RowHandle, "starttime").ToString())) &&
                    (!string.IsNullOrEmpty(gridview.GetRowCellValue(e.RowHandle, "collecttime").ToString())) &&
                    (!string.IsNullOrEmpty(gridview.GetRowCellValue(e.RowHandle, "sendtime").ToString())) &&
                    (gridview.RowCount < 6) && (gridview.RowCount == e.RowHandle + 1))
                {
                    gridview.AddNewRow();
                }
            }
        }

        private void gridView_ResidualCl_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                GridView gridview = sender as GridView;
                ResidualClrowindex = e.RowHandle;
                cb_ResidualCl_starttime.Items.Clear();
                if ((gridview.RowCount > ResidualClrowindex + 1) && (ResidualClrowindex > -1))
                {
                    int starttime = 0;
                    if ((ResidualClrowindex - 1) > -1)
                        starttime = Convert.ToInt32(gridview.GetRowCellValue(ResidualClrowindex - 1, "starttime")) + 1;
                    if (starttime < 0)
                        starttime = 0;
                    int endtime = 23;
                    object obj_endtime = gridview.GetRowCellValue(ResidualClrowindex + 1, "starttime");
                    if (obj_endtime != DBNull.Value && obj_endtime != null)
                        endtime = Convert.ToInt32(obj_endtime);
                    for (int i = starttime; i < endtime; i++)
                    {
                        cb_ResidualCl_starttime.Items.Add(i);
                    }
                }
                else
                {
                    int i = 0;
                    if (ResidualClrowindex > 0)
                    {
                        if (gridview.GetRowCellValue(ResidualClrowindex - 1, "starttime") != DBNull.Value && gridview.GetRowCellValue(ResidualClrowindex - 1, "starttime") != null)
                            i = Convert.ToInt32(gridview.GetRowCellValue(ResidualClrowindex - 1, "starttime")) + 1;
                    }
                    for (; i < 24; i++)
                    {
                        cb_ResidualCl_starttime.Items.Add(i);
                    }
                }
            }
        }
        #endregion

        #region Turbidity GridView
        int Turbidityrowindex = -1;
        private void gridView_Turbidity_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                GridView gridview = sender as GridView;
                if ((!string.IsNullOrEmpty(gridview.GetRowCellValue(e.RowHandle, "starttime").ToString())) &&
                    (!string.IsNullOrEmpty(gridview.GetRowCellValue(e.RowHandle, "collecttime").ToString())) &&
                    (!string.IsNullOrEmpty(gridview.GetRowCellValue(e.RowHandle, "sendtime").ToString())) &&
                    (gridview.RowCount < 6) && (gridview.RowCount == e.RowHandle + 1))
                {
                    gridview.AddNewRow();
                }
            }
        }

        private void gridView_Turbidity_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                GridView gridview = sender as GridView;
                Turbidityrowindex = e.RowHandle;
                cb_Turbidity_starttime.Items.Clear();
                if ((gridview.RowCount > Turbidityrowindex + 1) && (Turbidityrowindex > -1))
                {
                    int starttime = 0;
                    if ((Turbidityrowindex - 1) > -1)
                        starttime = Convert.ToInt32(gridview.GetRowCellValue(Turbidityrowindex - 1, "starttime")) + 1;
                    if (starttime < 0)
                        starttime = 0;
                    int endtime = 23;
                    object obj_endtime = gridview.GetRowCellValue(Turbidityrowindex + 1, "starttime");
                    if (obj_endtime != DBNull.Value && obj_endtime != null)
                        endtime = Convert.ToInt32(obj_endtime);
                    for (int i = starttime; i < endtime; i++)
                    {
                        cb_Turbidity_starttime.Items.Add(i);
                    }
                }
                else
                {
                    int i = 0;
                    if (Turbidityrowindex > 0)
                    {
                        if (gridview.GetRowCellValue(Turbidityrowindex - 1, "starttime") != DBNull.Value && gridview.GetRowCellValue(Turbidityrowindex - 1, "starttime") != null)
                            i = Convert.ToInt32(gridview.GetRowCellValue(Turbidityrowindex - 1, "starttime")) + 1;
                    }
                    for (; i < 24; i++)
                    {
                        cb_Turbidity_starttime.Items.Add(i);
                    }
                }
            }
        }
        #endregion

        #region PH GridView
        int PHrowindex = -1;
        private void gridView_PH_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                GridView gridview = sender as GridView;
                if ((!string.IsNullOrEmpty(gridview.GetRowCellValue(e.RowHandle, "starttime").ToString())) &&
                    (!string.IsNullOrEmpty(gridview.GetRowCellValue(e.RowHandle, "collecttime").ToString())) &&
                    (!string.IsNullOrEmpty(gridview.GetRowCellValue(e.RowHandle, "sendtime").ToString())) &&
                    (gridview.RowCount < 6) && (gridview.RowCount == e.RowHandle + 1))
                {
                    gridview.AddNewRow();
                }
            }
        }

        private void gridView_PH_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                GridView gridview = sender as GridView;
                PHrowindex = e.RowHandle;
                cb_PH_starttime.Items.Clear();
                if ((gridview.RowCount > PHrowindex + 1) && (PHrowindex > -1))
                {
                    int starttime = 0;
                    if ((PHrowindex - 1) > -1)
                        starttime = Convert.ToInt32(gridview.GetRowCellValue(PHrowindex - 1, "starttime")) + 1;
                    if (starttime < 0)
                        starttime = 0;
                    int endtime = 23;
                    object obj_endtime = gridview.GetRowCellValue(PHrowindex + 1, "starttime");
                    if (obj_endtime != DBNull.Value && obj_endtime != null)
                        endtime = Convert.ToInt32(obj_endtime);
                    for (int i = starttime; i < endtime; i++)
                    {
                        cb_PH_starttime.Items.Add(i);
                    }
                }
                else
                {
                    int i = 0;
                    if (PHrowindex > 0)
                    {
                        if (gridview.GetRowCellValue(PHrowindex - 1, "starttime") != DBNull.Value && gridview.GetRowCellValue(PHrowindex - 1, "starttime") != null)
                            i = Convert.ToInt32(gridview.GetRowCellValue(PHrowindex - 1, "starttime")) + 1;
                    }
                    for (; i < 24; i++)
                    {
                        cb_PH_starttime.Items.Add(i);
                    }
                }
            }
        }
        #endregion

        #region Conductivity GridView
        int Conductivityrowindex = -1;
        private void gridView_Conductivity_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                GridView gridview = sender as GridView;
                if ((!string.IsNullOrEmpty(gridview.GetRowCellValue(e.RowHandle, "starttime").ToString())) &&
                    (!string.IsNullOrEmpty(gridview.GetRowCellValue(e.RowHandle, "collecttime").ToString())) &&
                    (!string.IsNullOrEmpty(gridview.GetRowCellValue(e.RowHandle, "sendtime").ToString())) &&
                    (gridview.RowCount < 6) && (gridview.RowCount == e.RowHandle + 1))
                {
                    gridview.AddNewRow();
                }
            }
        }

        private void gridView_Conductivity_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                GridView gridview = sender as GridView;
                Conductivityrowindex = e.RowHandle;
                cb_Conductivity_starttime.Items.Clear();
                if ((gridview.RowCount > Conductivityrowindex + 1) && (Conductivityrowindex > -1))
                {
                    int starttime = 0;
                    if ((Conductivityrowindex - 1) > -1)
                        starttime = Convert.ToInt32(gridview.GetRowCellValue(Conductivityrowindex - 1, "starttime")) + 1;
                    if (starttime < 0)
                        starttime = 0;
                    int endtime = 23;
                    object obj_endtime = gridview.GetRowCellValue(Conductivityrowindex + 1, "starttime");
                    if (obj_endtime != DBNull.Value && obj_endtime != null)
                        endtime = Convert.ToInt32(obj_endtime);
                    for (int i = starttime; i < endtime; i++)
                    {
                        cb_Conductivity_starttime.Items.Add(i);
                    }
                }
                else
                {
                    int i = 0;
                    if (Conductivityrowindex > 0)
                    {
                        if (gridview.GetRowCellValue(Conductivityrowindex - 1, "starttime") != DBNull.Value && gridview.GetRowCellValue(Conductivityrowindex - 1, "starttime") != null)
                            i = Convert.ToInt32(gridview.GetRowCellValue(Conductivityrowindex - 1, "starttime")) + 1;
                    }
                    for (; i < 24; i++)
                    {
                        cb_Conductivity_starttime.Items.Add(i);
                    }
                }
            }
        }
        #endregion

        private void ceColConfig_CheckedChanged(object sender, EventArgs e)
        {
            ceTurbidityState.Enabled = ceColConfig.Checked;
            ceResidualClState.Enabled = ceColConfig.Checked;
            cePHState.Enabled = ceColConfig.Checked;
            ceConductivityState.Enabled = ceColConfig.Checked;

            if (!ceColConfig.Checked)
            {
                ceTurbidityState.Checked = ceColConfig.Checked;
                ceResidualClState.Checked = ceColConfig.Checked;
                cePHState.Checked = ceColConfig.Checked;
                ceConductivityState.Checked = ceColConfig.Checked;
            }
        }

        private void ceResidualCl_CheckedChanged(object sender, EventArgs e)
        {
            gridControl_ResidualCl.Enabled = ceResidualClInterval.Checked;
            if (!ceResidualClInterval.Checked)
                gridControl_ResidualCl.DataSource = null;
            else
            {
                DataTable dt_ResidualCl = new DataTable();
                dt_ResidualCl.Columns.Add("starttime");
                dt_ResidualCl.Columns.Add("collecttime");
                dt_ResidualCl.Columns.Add("sendtime");
                gridControl_ResidualCl.DataSource = dt_ResidualCl;
                gridView_ResidualCl.AddNewRow();
            }
        }

        private void ceTurbidityInterval_CheckedChanged(object sender, EventArgs e)
        {
            gridControl_Turbidity.Enabled = ceTurbidityInterval.Checked;
            if (!ceTurbidityInterval.Checked)
                gridControl_Turbidity.DataSource = null;
            else
            {
                DataTable dt_485 = new DataTable();
                dt_485.Columns.Add("starttime");
                dt_485.Columns.Add("collecttime");
                dt_485.Columns.Add("sendtime");
                gridControl_Turbidity.DataSource = dt_485;
                gridView_Turbidity.AddNewRow();
            }
        }

        private void cePHInterval_CheckedChanged(object sender, EventArgs e)
        {
            gridControl_PH.Enabled = cePHInterval.Checked;
            if (!cePHInterval.Checked)
                gridControl_PH.DataSource = null;
            else
            {
                DataTable dt_PH = new DataTable();
                dt_PH.Columns.Add("starttime");
                dt_PH.Columns.Add("collecttime");
                dt_PH.Columns.Add("sendtime");
                gridControl_PH.DataSource = dt_PH;
                gridView_PH.AddNewRow();
            }
        }

        private void ceConductivityInterval_CheckedChanged(object sender, EventArgs e)
        {
            gridControl_Conductivity.Enabled = ceConductivityInterval.Checked;
            if (!ceConductivityInterval.Checked)
                gridControl_Conductivity.DataSource = null;
            else
            {
                DataTable dt_Conductivity = new DataTable();
                dt_Conductivity.Columns.Add("starttime");
                dt_Conductivity.Columns.Add("collecttime");
                dt_Conductivity.Columns.Add("sendtime");
                gridControl_Conductivity.DataSource = dt_Conductivity;
                gridView_Conductivity.AddNewRow();
            }
        }

        #region button Events
        private void btnReset_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                XtraMessageBox.Show("请先填写终端ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return;
            }
            if (DialogResult.No == XtraMessageBox.Show("确定复位?", GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
            {
                return;
            }
            if (SwitchComunication.IsOn)
            {
                GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity();
                GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

                EnableControls(false);
                DisableRibbonBar();
                DisableNavigateBar();
                ShowWaitForm("", "正在复位终端...");
                BeginSerialPortDelegate();
                Application.DoEvents();
                SetStaticItem("正在复位终端...");
                GlobalValue.SerialPortMgr.Send(SerialPortType.OLWQReset);
            }
            else
            {

            }
        }

        private void btnCheckingTime_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                XtraMessageBox.Show("请先填写终端ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return;
            }
            if (SwitchComunication.IsOn)
            {
                GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity();
                GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

                EnableControls(false);
                DisableRibbonBar();
                DisableNavigateBar();
                ShowWaitForm("", "正在设置时间...");
                BeginSerialPortDelegate();
                Application.DoEvents();
                SetStaticItem("正在设置时间...");
                GlobalValue.SerialPortMgr.Send(SerialPortType.OLWQSetTime);
            }
            else
            {
            }
        }

        private void btnEnableCollect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                XtraMessageBox.Show("请先填写终端ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return;
            } 
            if (SwitchComunication.IsOn)
            {
                GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity();
                GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

                EnableControls(false);
                DisableRibbonBar();
                DisableNavigateBar();
                ShowWaitForm("", "正在启动采集...");
                BeginSerialPortDelegate();
                Application.DoEvents();
                SetStaticItem("正在启动采集...");
                GlobalValue.SerialPortMgr.Send(SerialPortType.OLWQSetEnableCollect);
            }
            else
            {
            }
        }

        private void btnReadParm_Click(object sender, EventArgs e)
        {
            if (SwitchComunication.IsOn)
            {
                GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity();
                bool haveread = false;
                if (ceID.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptID = ceID.Checked;
                    haveread = true;
                }
                else
                {
                    if (!Regex.IsMatch(txtID.Text, @"^\d{1,5}$"))
                    {
                        XtraMessageBox.Show("请输入设备ID", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtID.Focus();
                        return;
                    }
                    GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);
                    
                    if (ceTime.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptDT = ceTime.Checked;
                        haveread = true;
                    }
                    if (ceIP.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptIP = ceIP.Checked;
                        haveread = true;
                    }
                    if (cePort.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptPort = cePort.Checked;
                        haveread = true;
                    }
                    if (ceResidualClLowLimit.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptResidualClLowLimit = ceResidualClLowLimit.Checked;
                        haveread = true;
                    }
                    if (ceResidualClZero.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptResidualClZero = ceResidualClZero.Checked;
                        haveread = true;
                    }
                    if (ceResidualClStandValue.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptResidualClStandValue = ceResidualClStandValue.Checked;
                        haveread = true;
                    }
                    if (ceResidualClSensitivity.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptResidualClSensitivity = ceResidualClSensitivity.Checked;
                        haveread = true;
                    }
                    if (ceClearInterval.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptClearInterval = ceClearInterval.Checked;
                        haveread = true;
                    }
                    if (ceTurbidityUpLimit.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptTurbidityUpLimit = ceTurbidityUpLimit.Checked;
                        haveread = true;
                    }
                    if (cePowersupplyType.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOptPowerSupplyType = cePowersupplyType.Checked;
                        haveread = true;
                    }

                    if (ceColConfig.Checked){
                        GlobalValue.UniSerialPortOptData.IsOpt_CollectConfig = ceColConfig.Checked;
                        haveread = true;

                        cePHState.Checked =false;
                        ceConductivityState.Checked = false;
                        ceResidualClState.Checked = false;
                        ceTurbidityState.Checked = false;
                    }
                    if (ceResidualClInterval.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOpt_ResidualClInterval = ceResidualClInterval.Checked;
                        haveread = true;
                    }
                    if (ceTurbidityInterval.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOpt_TurbidityInterval = ceTurbidityInterval.Checked;
                        haveread = true;
                    }
                    if (cePHInterval.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOpt_PHInterval = cePHInterval.Checked;
                        haveread = true;
                    }
                    if (ceConductivityInterval.Checked)
                    {
                        GlobalValue.UniSerialPortOptData.IsOpt_ConductivityInterval = ceConductivityInterval.Checked;
                        haveread = true;
                    }
                }

                if (!haveread)
                {
                    XtraMessageBox.Show("请选择需要读取的参数!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                EnableControls(false);
                DisableRibbonBar();
                DisableNavigateBar();
                ShowWaitForm("", "正在读取...");
                BeginSerialPortDelegate();
                GlobalValue.SerialPortMgr.SerialPortScheduleEvent -= new SerialPortScheduleHandle(SerialPortMgr_SerialPortScheduleEvent);
                GlobalValue.SerialPortMgr.SerialPortScheduleEvent += new SerialPortScheduleHandle(SerialPortMgr_SerialPortScheduleEvent);
                Application.DoEvents();
                SetStaticItem("正在读取...");
                GlobalValue.SerialPortMgr.Send(SerialPortType.OLWQReadBaicInfo);
            }
            else
            {
            }
        }

        void SerialPortMgr_SerialPortScheduleEvent(object sender, SerialPortScheduleEventArgs e)
        {
            if ((e.OptType == SerialPortType.OLWQReadBaicInfo || e.OptType == SerialPortType.OLWQSetBasicInfo) && !string.IsNullOrEmpty(e.Msg))
            {
                ShowWaitForm("", e.Msg);
                SetStaticItem(e.Msg);
            }
        }

        private void btnSetParm_Click(object sender, EventArgs e)
        {
            if (SwitchComunication.IsOn)
            {
                if (Validate())
                {
                    GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity();
                    bool haveset = false;
                    if (ceID.Checked)
                    {
                        if (DialogResult.No == XtraMessageBox.Show("设置设备编号会初始化设备参数,是否继续?", GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                        {
                            return;
                        }
                        GlobalValue.UniSerialPortOptData.IsOptID = ceID.Checked;
                        GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);
                        haveset = true;
                    }
                    else
                    {
                        GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

                        if (ceIP.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOptIP = ceIP.Checked;
                            GlobalValue.UniSerialPortOptData.IP = new int[4];
                            GlobalValue.UniSerialPortOptData.IP[0] = Convert.ToInt32(txtNum1.Text);
                            GlobalValue.UniSerialPortOptData.IP[1] = Convert.ToInt32(txtNum2.Text);
                            GlobalValue.UniSerialPortOptData.IP[2] = Convert.ToInt32(txtNum3.Text);
                            GlobalValue.UniSerialPortOptData.IP[3] = Convert.ToInt32(txtNum4.Text);
                            haveset = true;
                        }
                        if (cePort.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOptPort = cePort.Checked;
                            GlobalValue.UniSerialPortOptData.Port = Convert.ToInt32(txtPort.Text);
                            haveset = true;
                        }
                        if (ceResidualClLowLimit.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOptResidualClLowLimit = ceResidualClLowLimit.Checked;
                            GlobalValue.UniSerialPortOptData.ResidualClLowLimit = Convert.ToUInt16(txtResiduaClLowLimit.Text);
                            haveset = true;
                        }
                        if (ceResidualClZero.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOptResidualClZero = ceResidualClZero.Checked;
                            GlobalValue.UniSerialPortOptData.ResidualClZero = Convert.ToUInt16(txtResidualClZero.Text);
                            haveset = true;
                        }
                        if (ceResidualClStandValue.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOptResidualClStandValue = ceResidualClStandValue.Checked;
                            GlobalValue.UniSerialPortOptData.ResidualClStandValue = Convert.ToUInt16(txtResidualClStandValue.Text);
                            haveset = true;
                        }
                        if (ceResidualClSensitivity.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOptResidualClSensitivity = ceResidualClSensitivity.Checked;
                            GlobalValue.UniSerialPortOptData.ResidualClSensitivity = Convert.ToUInt16(txtResidualClSensitivity.Text);
                            haveset = true;
                        }
                        if (ceClearInterval.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOptClearInterval = ceClearInterval.Checked;
                            GlobalValue.UniSerialPortOptData.ClearInterval = Convert.ToUInt16(txtClearInterval.Text);
                            haveset = true;
                        }
                        if (ceTurbidityUpLimit.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOptTurbidityUpLimit = ceTurbidityUpLimit.Checked;
                            GlobalValue.UniSerialPortOptData.TurbidityUpLimit = Convert.ToUInt16(txtTurbidityUpLimit.Text);
                            haveset = true;
                        }
                        if (cePowersupplyType.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOptPowerSupplyType = cePowersupplyType.Checked;
                            GlobalValue.UniSerialPortOptData.PowerSupplyType = (ushort)(cbPowersupplyType.SelectedIndex+1);
                            haveset = true;
                        }
                        if (ceColConfig.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_CollectConfig = ceColConfig.Checked;
                            if (ceTurbidityState.Checked)
                                GlobalValue.UniSerialPortOptData.Collect_Turbidity = true;
                            if (ceResidualClState.Checked)
                                GlobalValue.UniSerialPortOptData.Collect_ResidualC1 = true;
                            if (cePHState.Checked)
                                GlobalValue.UniSerialPortOptData.Collect_PH = true;
                            if (ceConductivityState.Checked)
                                GlobalValue.UniSerialPortOptData.Collect_Conductivity = true;

                            haveset = true;
                        }
                        if (ceResidualClInterval.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_ResidualClInterval = ceResidualClInterval.Checked;
                            GlobalValue.UniSerialPortOptData.ResidualCl_Interval = gridControl_ResidualCl.DataSource as DataTable;
                            haveset = true;
                        }
                        if (cePHInterval.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_PHInterval = cePHInterval.Checked;
                            GlobalValue.UniSerialPortOptData.PH_Interval = gridControl_PH.DataSource as DataTable;
                            haveset = true;
                        }
                        if (ceTurbidityInterval.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_TurbidityInterval = ceTurbidityInterval.Checked;
                            GlobalValue.UniSerialPortOptData.Turbidity_Interval =gridControl_Turbidity.DataSource as DataTable;
                            haveset = true;
                        }
                        if (ceConductivityInterval.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_ConductivityInterval = ceConductivityInterval.Checked;
                            GlobalValue.UniSerialPortOptData.Conductivity_Interval = gridControl_Conductivity.DataSource as DataTable;
                            haveset = true;
                        }
                    }

                    if (!haveset)
                    {
                        XtraMessageBox.Show("请选择需要设置的参数!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    EnableControls(false);
                    DisableRibbonBar();
                    DisableNavigateBar();
                    ShowWaitForm("", "正在设置...");
                    BeginSerialPortDelegate();
                    GlobalValue.SerialPortMgr.SerialPortScheduleEvent -= new SerialPortScheduleHandle(SerialPortMgr_SerialPortScheduleEvent);
                    GlobalValue.SerialPortMgr.SerialPortScheduleEvent += new SerialPortScheduleHandle(SerialPortMgr_SerialPortScheduleEvent);
                    Application.DoEvents();
                    SetStaticItem("正在设置...");
                    GlobalValue.SerialPortMgr.Send(SerialPortType.OLWQSetBasicInfo);
                }
            }
            else
            {
            }
        }

        public override void OnSerialPortNotify(object sender, SerialPortEventArgs e)
        {
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.OLWQReset)
            {
                this.Enabled = true;
                HideWaitForm();

                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);
                if (e.TransactStatus == TransStatus.Success)
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();

                    XtraMessageBox.Show("复位成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    XtraMessageBox.Show("复位失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.OLWQSetTime)
            {
                this.Enabled = true;
                HideWaitForm();

                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);
                if (e.TransactStatus == TransStatus.Success)
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();

                    XtraMessageBox.Show("设置时间成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    XtraMessageBox.Show("设置时间失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.OLWQSetEnableCollect)
            {
                this.Enabled = true;
                HideWaitForm();

                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);
                if (e.TransactStatus == TransStatus.Success)
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();

                    XtraMessageBox.Show("设置启动采集成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    XtraMessageBox.Show("设置启动采集失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.OLWQReadBaicInfo)
            {
                this.Enabled = true;
                HideWaitForm();

                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);
                if (e.TransactStatus == TransStatus.Success)
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();

                    if (GlobalValue.UniSerialPortOptData.IsOptID)
                    {
                        txtID.Text = GlobalValue.UniSerialPortOptData.ID.ToString();
                    }
                    if (GlobalValue.UniSerialPortOptData.IsOptDT)
                    {
                        txtTime.Text = GlobalValue.UniSerialPortOptData.DT.ToString();
                    }
                    if (GlobalValue.UniSerialPortOptData.IsOptIP)
                    {
                        txtNum1.Text = GlobalValue.UniSerialPortOptData.IP[0].ToString();
                        txtNum2.Text = GlobalValue.UniSerialPortOptData.IP[1].ToString();
                        txtNum3.Text = GlobalValue.UniSerialPortOptData.IP[2].ToString();
                        txtNum4.Text = GlobalValue.UniSerialPortOptData.IP[3].ToString();
                    }
                    if (GlobalValue.UniSerialPortOptData.IsOptPort)
                    {
                        txtPort.Text = GlobalValue.UniSerialPortOptData.Port.ToString();
                    }
                    if (GlobalValue.UniSerialPortOptData.IsOptResidualClLowLimit)
                    {
                        txtResiduaClLowLimit.Text = GlobalValue.UniSerialPortOptData.ResidualClLowLimit.ToString();
                    }
                    if (GlobalValue.UniSerialPortOptData.IsOptResidualClZero)
                    {
                        txtResidualClZero.Text = GlobalValue.UniSerialPortOptData.ResidualClZero.ToString();
                    }
                    if (GlobalValue.UniSerialPortOptData.IsOptResidualClStandValue)
                    {
                        txtResidualClStandValue.Text = GlobalValue.UniSerialPortOptData.ResidualClStandValue.ToString();
                    }
                    if (GlobalValue.UniSerialPortOptData.IsOptResidualClSensitivity)
                    {
                        txtResidualClSensitivity.Text = GlobalValue.UniSerialPortOptData.ResidualClSensitivity.ToString();
                    }
                    if (GlobalValue.UniSerialPortOptData.IsOptClearInterval)
                    {
                        txtClearInterval.Text = GlobalValue.UniSerialPortOptData.ClearInterval.ToString();
                    }
                    if (GlobalValue.UniSerialPortOptData.IsOptTurbidityUpLimit)
                    {
                        txtTurbidityUpLimit.Text = GlobalValue.UniSerialPortOptData.TurbidityUpLimit.ToString();
                    }
                    if (GlobalValue.UniSerialPortOptData.IsOptPowerSupplyType)
                    {
                        cbPowersupplyType.SelectedIndex = GlobalValue.UniSerialPortOptData.PowerSupplyType-1;
                    }

                    if (GlobalValue.UniSerialPortOptData.IsOpt_CollectConfig)
                    {
                        cePHState.Checked =GlobalValue.UniSerialPortOptData.Collect_PH;
                        ceConductivityState.Checked = GlobalValue.UniSerialPortOptData.Collect_Conductivity;
                        ceResidualClState.Checked = GlobalValue.UniSerialPortOptData.Collect_ResidualC1;
                        ceTurbidityState.Checked = GlobalValue.UniSerialPortOptData.Collect_Turbidity;
                    }
                    if (GlobalValue.UniSerialPortOptData.IsOpt_ResidualClInterval)
                    {
                        gridControl_ResidualCl.DataSource=GlobalValue.UniSerialPortOptData.ResidualCl_Interval;
                    }
                    if (GlobalValue.UniSerialPortOptData.IsOpt_TurbidityInterval)
                    {
                        gridControl_Turbidity.DataSource = GlobalValue.UniSerialPortOptData.Turbidity_Interval;
                    }
                    if (GlobalValue.UniSerialPortOptData.IsOpt_PHInterval)
                    {
                        gridControl_PH.DataSource = GlobalValue.UniSerialPortOptData.PH_Interval;
                    }
                    if (GlobalValue.UniSerialPortOptData.IsOpt_ConductivityInterval)
                    {
                        gridControl_Conductivity.DataSource = GlobalValue.UniSerialPortOptData.Conductivity_Interval;
                    }

                    XtraMessageBox.Show("读取设备参数成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    XtraMessageBox.Show("读取设备参数失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.OLWQSetBasicInfo)
            {
                this.Enabled = true;
                HideWaitForm();

                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);
                if (e.TransactStatus == TransStatus.Success)
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();

                    XtraMessageBox.Show("设置设备参数成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    XtraMessageBox.Show("设置设备参数失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void EnableControls(bool enable)
        {
            btnReset.Enabled = enable;
            btnCheckingTime.Enabled = enable;
            btnEnableCollect.Enabled = enable;
            btnReadParm.Enabled = enable;
            btnSetParm.Enabled = enable;
            SwitchComunication.Enabled = enable;
        }

        private new bool Validate()
        {
            if (!Regex.IsMatch(txtID.Text,@"^\d{1,5}$"))
            {
                XtraMessageBox.Show("请输入设备ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return false;
            }
            if (ceIP.Checked)
            {
                if (string.IsNullOrEmpty(txtNum1.Text) || string.IsNullOrEmpty(txtNum2.Text) ||
                    string.IsNullOrEmpty(txtNum3.Text) || string.IsNullOrEmpty(txtNum4.Text))
                {
                    XtraMessageBox.Show("请填写完整的IP地址!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtNum1.Focus();
                    return false;
                }
            }

            if (cePort.Checked && !Regex.IsMatch(txtPort.Text, @"^\d{3,5}$"))
            {
                XtraMessageBox.Show("请输入端口号!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPort.Focus();
                return false;
            }

            if (ceResidualClLowLimit.Checked && string.IsNullOrEmpty(txtResiduaClLowLimit.Text))
            {
                XtraMessageBox.Show("请输入余氯下限值!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtResiduaClLowLimit.Focus();
                return false;
            }

            if (ceResidualClZero.Checked && string.IsNullOrEmpty(txtResidualClZero.Text))
            {
                XtraMessageBox.Show("请输入余氯零点值!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtResidualClZero.Focus();
                return false;
            }

            if (ceResidualClStandValue.Checked && string.IsNullOrEmpty(txtResidualClStandValue.Text))
            {
                XtraMessageBox.Show("请输入余氯标准值!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtResidualClStandValue.Focus();
                return false;
            }

            if (ceResidualClSensitivity.Checked && string.IsNullOrEmpty(txtResidualClSensitivity.Text))
            {
                XtraMessageBox.Show("请输入余氯灵敏度!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtResidualClSensitivity.Focus();
                return false;
            }

            if (ceClearInterval.Checked && string.IsNullOrEmpty(txtClearInterval.Text))
            {
                XtraMessageBox.Show("请输入清洗间隔!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtClearInterval.Focus();
                return false;
            }

            if (ceTurbidityUpLimit.Checked && string.IsNullOrEmpty(txtTurbidityUpLimit.Text))
            {
                XtraMessageBox.Show("请输入合法的浊度上限值!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTurbidityUpLimit.Focus();
                return false;
            }

            if (cePowersupplyType.Checked &&string.IsNullOrEmpty(cbPowersupplyType.Text))
            {
                XtraMessageBox.Show("请选择的供电方式!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbPowersupplyType.Focus();
                return false;
            }

            if (ceResidualClInterval.Checked)
            {
                DataTable dt = gridControl_ResidualCl.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0)
                {
                    XtraMessageBox.Show("请填写余氯采集时间间隔!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    gridView_ResidualCl.Focus();
                    return false;
                }
            }

            if (ceTurbidityInterval.Checked)
            {
                DataTable dt = gridControl_Turbidity.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0)
                {
                    XtraMessageBox.Show("请填写浊度采集时间间隔表!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    gridView_Turbidity.Focus();
                    return false;
                }
            }

            if (cePHInterval.Checked)
            {
                DataTable dt = gridControl_PH.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0)
                {
                    XtraMessageBox.Show("请填写PH采集时间间隔表!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    gridView_PH.Focus();
                    return false;
                }
            }

            if (ceConductivityInterval.Checked)
            {
                DataTable dt = gridControl_Conductivity.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0)
                {
                    XtraMessageBox.Show("请填写电导率采集时间间隔表!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    gridView_Conductivity.Focus();
                    return false;
                }
            }

            return true;
        }
        #endregion

        public override void SerialPortEvent(bool Enabled)
        {
            if (SwitchComunication.IsOn)  //串口
            {
                btnReset.Enabled = Enabled;
                btnCheckingTime.Enabled = Enabled;
                btnEnableCollect.Enabled = Enabled;
                btnReadParm.Enabled = Enabled;
                btnSetParm.Enabled = Enabled;
            }
        }

        private void SwitchComunication_Click(object sender, EventArgs e)
        {
            if (SwitchComunication.IsOn)  //Grps
            {
                SetGprsCtrlStatus(); 
            }
            else   //串口
            {
                SetSerialPortCtrlStatus();
            }
        }

        private void SetSerialPortCtrlStatus()
        {
            btnReset.Enabled = GlobalValue.portUtil.IsOpen;
            btnCheckingTime.Enabled = GlobalValue.portUtil.IsOpen;
            btnEnableCollect.Enabled = GlobalValue.portUtil.IsOpen;
            btnReadParm.Enabled = GlobalValue.portUtil.IsOpen;
            btnSetParm.Enabled = GlobalValue.portUtil.IsOpen;

            ceTime.Enabled = true;
            txtTime.Enabled = true;
            txtTime.Text = "";
            ceIP.Enabled = true;
            txtNum1.Enabled = true;
            txtNum1.Text = "";
            txtNum2.Enabled = true;
            txtNum2.Text = "";
            txtNum3.Enabled = true;
            txtNum3.Text = "";
            txtNum4.Enabled = true;
            txtNum4.Text = "";
            cePort.Enabled = true;
            txtPort.Enabled = true;
            txtPort.Text = "";
            cePowersupplyType.Enabled = true;
            cePowersupplyType.Checked = false;
            cbPowersupplyType.Enabled = true;
            cbPowersupplyType.SelectedIndex = -1;

            ceColConfig.Enabled = true;
            cePHState.Enabled = false;
            cePHState.Checked = false;
            ceConductivityState.Enabled = false;
            ceConductivityState.Checked = false;
            ceResidualClState.Enabled = false;
            ceResidualClState.Checked = false;
            ceTurbidityState.Enabled = false;
            ceTurbidityState.Checked = false;

            ceClearInterval.Enabled = true;
            ceClearInterval.Checked = false;
            txtClearInterval.Enabled = true;
            txtClearInterval.Text = "";
            ceTurbidityUpLimit.Enabled = true;
            ceTurbidityUpLimit.Checked = false;
            txtTurbidityUpLimit.Enabled = true;
            txtTurbidityUpLimit.Text = "";

            ceResidualClLowLimit.Enabled = true;
            ceResidualClLowLimit.Checked = false;
            txtResiduaClLowLimit.Enabled = true;
            txtResiduaClLowLimit.Text = "";
            ceResidualClZero.Enabled = true;
            ceResidualClZero.Checked = false;
            txtResidualClZero.Enabled = true;
            txtResidualClZero.Text = "";
            ceResidualClStandValue.Enabled = true;
            ceResidualClStandValue.Checked = false;
            txtResidualClStandValue.Enabled = true;
            txtResidualClStandValue.Text = "";
            ceResidualClSensitivity.Enabled = true;
            ceResidualClSensitivity.Checked = false;
            txtResidualClSensitivity.Enabled = true;
            txtResidualClSensitivity.Text = "";
            
            ceTurbidityInterval.Enabled = true;
            cePHInterval.Enabled = true;
            ceResidualClInterval.Enabled = true;
            ceConductivityInterval.Enabled = true;
        }

        private void SetGprsCtrlStatus()
        {
            btnReset.Enabled = false;
            btnCheckingTime.Enabled = false;
            btnEnableCollect.Enabled = false;
            btnReadParm.Enabled = false;
            btnSetParm.Enabled = true;

            ceTime.Enabled = false;
            ceTime.Checked = false;
            txtTime.Enabled = false;
            txtTime.Text = "";
            ceIP.Enabled = false;
            ceIP.Checked = false;
            txtNum1.Enabled = false;
            txtNum1.Text = "";
            txtNum2.Enabled = false;
            txtNum2.Text = "";
            txtNum3.Enabled = false;
            txtNum3.Text = "";
            txtNum4.Enabled = false;
            txtNum4.Text = "";
            cePort.Enabled = false;
            cePort.Checked = false;
            txtPort.Enabled = false;
            txtPort.Text = "";
            cePowersupplyType.Enabled = false;
            cePowersupplyType.Checked = false;
            cbPowersupplyType.Enabled = false;
            cbPowersupplyType.SelectedIndex = -1;

            ceColConfig.Enabled = false;
            ceColConfig.Checked = false;

            ceClearInterval.Enabled = false;
            ceClearInterval.Checked = false;
            ceTurbidityUpLimit.Enabled = false;
            ceTurbidityUpLimit.Checked = false;

            ceResidualClLowLimit.Enabled = false;
            ceResidualClLowLimit.Checked = false;
            ceResidualClZero.Enabled = false;
            ceResidualClZero.Checked = false;
            ceResidualClStandValue.Enabled = false;
            ceResidualClStandValue.Checked = false;
            ceResidualClSensitivity.Enabled = false;
            ceResidualClSensitivity.Checked = false;
        }

        private void ceIP_CheckedChanged(object sender, EventArgs e)
        {
            if (!ceIP.Checked)
            {
                txtNum1.Text = "";
                txtNum2.Text = "";
                txtNum3.Text = "";
                txtNum4.Text = "";
            }
        }

        private void cePort_CheckedChanged(object sender, EventArgs e)
        {
            if (!cePort.Checked)
            {
                txtPort.Text = "";
            }
        }

        private void ceResidualClLowLimit_CheckedChanged(object sender, EventArgs e)
        {
            if (!ceResidualClLowLimit.Checked)
            {
                txtResiduaClLowLimit.Text = "";
            }
        }

        private void ceResidualClZero_CheckedChanged(object sender, EventArgs e)
        {
            if (!ceResidualClZero.Checked)
            {
                txtResidualClZero.Text = "";
            }
        }

        private void ceResidualClStandValue_CheckedChanged(object sender, EventArgs e)
        {
            if (!ceResidualClStandValue.Checked)
            {
                txtResidualClStandValue.Text = "";
            }
        }

        private void ceResidualClSensitivity_CheckedChanged(object sender, EventArgs e)
        {
            if (!ceResidualClSensitivity.Checked)
            {
                txtResidualClSensitivity.Text = "";
            }
        }

        private void ceClearInterval_CheckedChanged(object sender, EventArgs e)
        {
            if (!ceClearInterval.Checked)
            {
                txtClearInterval.Text = "";
            }
        }

        private void ceTurbidityUpLimit_CheckedChanged(object sender, EventArgs e)
        {
            if (!ceTurbidityUpLimit.Checked)
            {
                txtTurbidityUpLimit.Text = "";
            }
        }

        private void cePowersupplyType_CheckedChanged(object sender, EventArgs e)
        {
            if (!cePowersupplyType.Checked)
            {
                cbPowersupplyType.SelectedIndex = -1;
            }
        }
        

    }
}
