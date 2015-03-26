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

            #region Init Simulate GridView
            cb_sim_starttime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            for (int i = 0; i < 24; i++)
                cb_sim_starttime.Items.Add(i);

            cb_sim_coltime1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_sim_coltime1.Items.Add(1);
            cb_sim_coltime1.Items.Add(5);
            cb_sim_coltime1.Items.Add(15);
            cb_sim_coltime1.Items.Add(30);
            cb_sim_coltime1.Items.Add(60);
            cb_sim_coltime1.Items.Add(120);

            cb_sim_coltime2.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_sim_coltime2.Items.Add(1);
            cb_sim_coltime2.Items.Add(5);
            cb_sim_coltime2.Items.Add(15);
            cb_sim_coltime2.Items.Add(30);
            cb_sim_coltime2.Items.Add(60);
            cb_sim_coltime2.Items.Add(120);

            cb_sim_sendtime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_sim_sendtime.Items.Add(5);
            cb_sim_sendtime.Items.Add(15);
            cb_sim_sendtime.Items.Add(30);
            cb_sim_sendtime.Items.Add(60);
            cb_sim_sendtime.Items.Add(120);
            cb_sim_sendtime.Items.Add(240);
            cb_sim_sendtime.Items.Add(480);
            cb_sim_sendtime.Items.Add(720);
            cb_sim_sendtime.Items.Add(1440);
            #endregion

            #region Init Pluse GridView
            cb_pluse_starttime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            for (int i = 0; i < 24; i++)
                cb_pluse_starttime.Items.Add(i);
            
            cb_pluse_coltime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_pluse_coltime.Items.Add(1);
            cb_pluse_coltime.Items.Add(5);
            cb_pluse_coltime.Items.Add(15);
            cb_pluse_coltime.Items.Add(30);
            cb_pluse_coltime.Items.Add(60);
            cb_pluse_coltime.Items.Add(120);

            cb_pluse_sendtime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_pluse_sendtime.Items.Add(5);
            cb_pluse_sendtime.Items.Add(15);
            cb_pluse_sendtime.Items.Add(30);
            cb_pluse_sendtime.Items.Add(60);
            cb_pluse_sendtime.Items.Add(120);
            cb_pluse_sendtime.Items.Add(240);
            cb_pluse_sendtime.Items.Add(480);
            cb_pluse_sendtime.Items.Add(720);
            cb_pluse_sendtime.Items.Add(1440);
            #endregion

            #region Init RS485 GridView
            cb_RS485_starttime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            for (int i = 0; i < 24; i++)
                cb_RS485_starttime.Items.Add(i);

            cb_RS485_coltime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_RS485_coltime.Items.Add(1);
            cb_RS485_coltime.Items.Add(5);
            cb_RS485_coltime.Items.Add(15);
            cb_RS485_coltime.Items.Add(30);
            cb_RS485_coltime.Items.Add(60);
            cb_RS485_coltime.Items.Add(120);

            cb_RS485_sendtime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_RS485_sendtime.Items.Add(5);
            cb_RS485_sendtime.Items.Add(15);
            cb_RS485_sendtime.Items.Add(30);
            cb_RS485_sendtime.Items.Add(60);
            cb_RS485_sendtime.Items.Add(120);
            cb_RS485_sendtime.Items.Add(240);
            cb_RS485_sendtime.Items.Add(480);
            cb_RS485_sendtime.Items.Add(720);
            cb_RS485_sendtime.Items.Add(1440);
            #endregion

            #region Init RS485 Protocol GridView
            cb_485protocol_baud.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_485protocol_baud.Items.AddRange(new int[] { 1200, 2400, 4800, 9600, 14400, 19200, 38400, 56000, 57600, 115200});
            #endregion
        }
        //public UniversalTerParm(FrmSystem parentform)
        //{
        //    InitializeComponent();
        //}

        private void UniversalTerParm_Load(object sender, EventArgs e)
        {
            InitGridView();
            InitControls();
        }

        private void cbBaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbBaudRate.Text.Trim() == "customiz.")
            {
                cbBaudRate.Text = "";
            }
        }

        private void InitGridView()
        {
            gridControl_Simulate.DataSource = null;
            DataTable dt_simulate = new DataTable();
            dt_simulate.Columns.Add("starttime");
            dt_simulate.Columns.Add("collecttime1");
            dt_simulate.Columns.Add("collecttime2");
            dt_simulate.Columns.Add("sendtime");
            gridControl_Simulate.DataSource = dt_simulate;
            gridView_Simulate.AddNewRow();

            gridControl_Pluse.DataSource = null;
            DataTable dt_pluse = new DataTable();
            dt_pluse.Columns.Add("starttime");
            dt_pluse.Columns.Add("collecttime");
            dt_pluse.Columns.Add("sendtime");
            gridControl_Pluse.DataSource = dt_pluse;
            gridView_Pluse.AddNewRow();

            gridControl_RS485.DataSource = null;
            DataTable dt_485 = new DataTable();
            dt_485.Columns.Add("starttime");
            dt_485.Columns.Add("collecttime");
            dt_485.Columns.Add("sendtime");
            gridControl_RS485.DataSource = dt_485;
            gridView_RS485.AddNewRow();

            gridControl_485protocol.DataSource = null;
            DataTable dt_485protocol = new DataTable();
            dt_485protocol.Columns.Add("baud");
            dt_485protocol.Columns.Add("ID");
            dt_485protocol.Columns.Add("funcode");
            dt_485protocol.Columns.Add("regbeginaddr");
            dt_485protocol.Columns.Add("regcount");
            gridControl_485protocol.DataSource = dt_485protocol;
            gridView_485protocol.AddNewRow();
            gridView_485protocol.AddNewRow();
        }

        private void InitControls()
        {
            ceColConfig.Checked = false;
            ceCollectSimulate.Checked = false;
            ceCollectPluse.Checked = false;
            ceCollectRS485.Checked = false;
            gridControl_Simulate.Enabled = false;
            gridControl_Pluse.Enabled = false;
            gridControl_RS485.Enabled = false;

            ceModbusExeFlag.Checked = false;
            gridControl_485protocol.Enabled = false;
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

        private void gridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator)
            {
                e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        #region Simulate GridView
        int simulaterowindex = -1;
        private void gridView_Simulate_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
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

        private void gridView_Simulate_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                GridView gridview = sender as GridView;
                simulaterowindex = e.RowHandle;
                cb_sim_starttime.Items.Clear();
                if ((gridview.RowCount > simulaterowindex + 1) && (simulaterowindex > -1))
                {
                    int starttime = 0;
                    if ((simulaterowindex - 1) > -1)
                        starttime = Convert.ToInt32(gridview.GetRowCellValue(simulaterowindex - 1, "starttime")) + 1;
                    if (starttime < 0)
                        starttime = 0;
                    int endtime = 23;
                    object obj_endtime = gridview.GetRowCellValue(simulaterowindex + 1, "starttime");
                    if (obj_endtime != DBNull.Value && obj_endtime != null)
                        endtime = Convert.ToInt32(obj_endtime);
                    for (int i = starttime; i < endtime; i++)
                    {
                        cb_sim_starttime.Items.Add(i);
                    }
                }
                else
                {
                    int i = 0;
                    if (simulaterowindex > 0)
                    {
                        if (gridview.GetRowCellValue(simulaterowindex - 1, "starttime") != DBNull.Value && gridview.GetRowCellValue(simulaterowindex - 1, "starttime") != null)
                            i = Convert.ToInt32(gridview.GetRowCellValue(simulaterowindex - 1, "starttime")) + 1;
                    }
                    for (; i < 24; i++)
                    {
                        cb_sim_starttime.Items.Add(i);
                    }
                }
            }
        }
        #endregion

        #region Pluse GridView
        int pluserowindex = -1;
        private void gridView_Pluse_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
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

        private void gridView_Pluse_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                GridView gridview = sender as GridView;
                pluserowindex = e.RowHandle;
                cb_pluse_starttime.Items.Clear();
                if ((gridview.RowCount > pluserowindex + 1) && (pluserowindex > -1))
                {
                    int starttime = 0;
                    if ((pluserowindex - 1) > -1)
                        starttime = Convert.ToInt32(gridview.GetRowCellValue(pluserowindex - 1, "starttime")) + 1;
                    if (starttime < 0)
                        starttime = 0;
                    int endtime = 23;
                    object obj_endtime = gridview.GetRowCellValue(pluserowindex + 1, "starttime");
                    if (obj_endtime != DBNull.Value && obj_endtime != null)
                        endtime = Convert.ToInt32(obj_endtime);
                    for (int i = starttime; i < endtime; i++)
                    {
                        cb_pluse_starttime.Items.Add(i);
                    }
                }
                else
                {
                    int i = 0;
                    if (pluserowindex > 0)
                    {
                        if (gridview.GetRowCellValue(pluserowindex - 1, "starttime") != DBNull.Value && gridview.GetRowCellValue(pluserowindex - 1, "starttime") != null)
                            i = Convert.ToInt32(gridview.GetRowCellValue(pluserowindex - 1, "starttime")) + 1;
                    }
                    for (; i < 24; i++)
                    {
                        cb_pluse_starttime.Items.Add(i);
                    }
                }
            }
        }
        #endregion

        #region RS485 GridView
        int rs485rowindex = -1;
        private void gridView_RS485_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
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

        private void gridView_RS485_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                GridView gridview = sender as GridView;
                rs485rowindex = e.RowHandle;
                cb_RS485_starttime.Items.Clear();
                if ((gridview.RowCount > rs485rowindex + 1) && (rs485rowindex > -1))
                {
                    int starttime = 0;
                    if ((rs485rowindex - 1) > -1)
                        starttime = Convert.ToInt32(gridview.GetRowCellValue(rs485rowindex - 1, "starttime")) + 1;
                    if (starttime < 0)
                        starttime = 0;
                    int endtime = 23;
                    object obj_endtime = gridview.GetRowCellValue(rs485rowindex + 1, "starttime");
                    if (obj_endtime != DBNull.Value && obj_endtime != null)
                        endtime = Convert.ToInt32(obj_endtime);
                    for (int i = starttime; i < endtime; i++)
                    {
                        cb_RS485_starttime.Items.Add(i);
                    }
                }
                else
                {
                    int i = 0;
                    if (rs485rowindex > 0)
                    {
                        if (gridview.GetRowCellValue(rs485rowindex - 1, "starttime") != DBNull.Value && gridview.GetRowCellValue(rs485rowindex - 1, "starttime") != null)
                            i = Convert.ToInt32(gridview.GetRowCellValue(rs485rowindex - 1, "starttime")) + 1;
                    }
                    for (; i < 24; i++)
                    {
                        cb_RS485_starttime.Items.Add(i);
                    }
                }
            }
        }
        #endregion

        #region RS485 Protocol
        private void gridView_485protocol_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                GridView gridview = sender as GridView;
                int rowindex = e.RowHandle;
                if ((!string.IsNullOrEmpty(gridview.GetRowCellValue(rowindex, "baud").ToString())) &&
                    (gridview.RowCount < 8))
                {
                    if ((gridview.RowCount == (rowindex + 1)) && (rowindex > -1))
                        gridview.AddNewRow();
                }
            }
        }
        #endregion

        private void ceColConfig_CheckedChanged(object sender, EventArgs e)
        {
            gridControl_Simulate.Enabled = ceColConfig.Checked & ceCollectSimulate.Checked;
            gridControl_Pluse.Enabled = ceColConfig.Checked & ceCollectPluse.Checked;
            gridControl_RS485.Enabled = ceColConfig.Checked & ceCollectRS485.Checked;
        }

        private void ceCollectSimulate_CheckedChanged(object sender, EventArgs e)
        {
            gridControl_Simulate.Enabled = ceColConfig.Checked & ceCollectSimulate.Checked;
        }

        private void ceCollectPluse_CheckedChanged(object sender, EventArgs e)
        {
            gridControl_Pluse.Enabled = ceColConfig.Checked & ceCollectPluse.Checked ;
        }

        private void ceCollectRS485_CheckedChanged(object sender, EventArgs e)
        {
            gridControl_RS485.Enabled = ceColConfig.Checked & ceCollectRS485.Checked;
        }

        private void ceModbusExeFlag_CheckedChanged(object sender, EventArgs e)
        {
            gridControl_485protocol.Enabled = ceModbusExeFlag.Checked;
        }

        #region button Events
        private void btnReset_Click(object sender, EventArgs e)
        {
        }

        private void btnCheckingTime_Click(object sender, EventArgs e)
        {
        }

        private void btnEnableCollect_Click(object sender, EventArgs e)
        {

        }

        private void btnReadParm_Click(object sender, EventArgs e)
        {

        }

        private void btnSetParm_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}
