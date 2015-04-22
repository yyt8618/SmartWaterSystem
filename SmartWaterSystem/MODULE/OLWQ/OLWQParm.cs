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
            cb_485protocol_baud.Items.AddRange(new int[] { 1200, 2400, 4800, 9600});//, 14400, 19200, 38400, 56000, 57600, 115200});
            txt_485protocol_ID.KeyPress += new KeyPressEventHandler(txt_485protocol_onebyte_KeyPress);
            txt_485protocol_funcode.KeyPress += new KeyPressEventHandler(txt_485protocol_onebyte_KeyPress);
            txt_485protocol_regbeginaddr.KeyPress += new KeyPressEventHandler(txt_485protocol_twobyte_KeyPress);
            txt_485protocol_regcount.KeyPress += new KeyPressEventHandler(txt_485protocol_twobyte_KeyPress);
            #endregion
        }

        private void OLWQParm_Load(object sender, EventArgs e)
        {
            InitGridView();
            InitControls();

            cbComType.SelectedIndex = 1;
        }


        private void InitGridView()
        {
            gridControl_Simulate.DataSource = null;

            gridControl_RS485.DataSource = null;
        }

        private void InitControls()
        {
            ceColConfig.Checked = false;
            ceCollectSimulate.Checked = false;
            ceCollectRS485.Checked = false;
            gridControl_Simulate.Enabled = false;
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

        #region 485Protocol txt Event
        /// <summary>
        /// 限制最大1byte (<=255)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txt_485protocol_onebyte_KeyPress(object sender, KeyPressEventArgs e)
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
        void txt_485protocol_twobyte_KeyPress(object sender, KeyPressEventArgs e)
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

        #region Simulate GridView
        int simulaterowindex = -1;
        private void gridView_Simulate_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                GridView gridview = sender as GridView;
                if ((!string.IsNullOrEmpty(gridview.GetRowCellValue(e.RowHandle, "starttime").ToString())) &&
                    (!string.IsNullOrEmpty(gridview.GetRowCellValue(e.RowHandle, "collecttime1").ToString())) &&
                    (!string.IsNullOrEmpty(gridview.GetRowCellValue(e.RowHandle, "collecttime2").ToString())) &&
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
            cemodbusprotocolstatus.Enabled = ceColConfig.Checked;
            ceRS485State.Enabled = ceColConfig.Checked;
            ceSimulate1State.Enabled = ceColConfig.Checked;
            ceSimulate2State.Enabled = ceColConfig.Checked;

            if (!ceColConfig.Checked)
            {
                cemodbusprotocolstatus.Checked = ceColConfig.Checked;
                ceRS485State.Checked = ceColConfig.Checked;
                ceSimulate1State.Checked = ceColConfig.Checked;
                ceSimulate2State.Checked = ceColConfig.Checked;
            }
        }

        private void ceCollectSimulate_CheckedChanged(object sender, EventArgs e)
        {
            gridControl_Simulate.Enabled = ceCollectSimulate.Checked;
            if (!ceCollectSimulate.Checked)
                gridControl_Simulate.DataSource = null;
            else
            {
                DataTable dt_simulate = new DataTable();
                dt_simulate.Columns.Add("starttime");
                dt_simulate.Columns.Add("collecttime1");
                dt_simulate.Columns.Add("collecttime2");
                dt_simulate.Columns.Add("sendtime");
                gridControl_Simulate.DataSource = dt_simulate;
                gridView_Simulate.AddNewRow();
            }
        }

        private void ceCollectRS485_CheckedChanged(object sender, EventArgs e)
        {
            gridControl_RS485.Enabled = ceCollectRS485.Checked;
            if (!ceCollectRS485.Checked)
                gridControl_RS485.DataSource = null;
            else
            {
                DataTable dt_485 = new DataTable();
                dt_485.Columns.Add("starttime");
                dt_485.Columns.Add("collecttime");
                dt_485.Columns.Add("sendtime");
                gridControl_RS485.DataSource = dt_485;
                gridView_RS485.AddNewRow();
            }
        }

        private void ceModbusExeFlag_CheckedChanged(object sender, EventArgs e)
        {
            gridControl_485protocol.Enabled = ceModbusExeFlag.Checked;
            if (!ceModbusExeFlag.Checked)
                gridControl_485protocol.DataSource = null;
            else
            {
                DataTable dt_485protocol = new DataTable();
                dt_485protocol.Columns.Add("baud");
                dt_485protocol.Columns.Add("ID");
                dt_485protocol.Columns.Add("funcode");
                dt_485protocol.Columns.Add("regbeginaddr");
                dt_485protocol.Columns.Add("regcount");
                gridControl_485protocol.DataSource = dt_485protocol;
                gridView_485protocol.AddNewRow();
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
            if (SwitchComunication.IsOn)
            {
                GlobalValue.UniversalSerialPortOptData = new UniversalSerialPortOptEntity();
                GlobalValue.UniversalSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

                EnableControls(false);
                DisableRibbonBar();
                DisableNavigateBar();
                ShowWaitForm("", "正在复位终端...");
                BeginSerialPortDelegate();
                Application.DoEvents();
                SetStaticItem("正在复位终端...");
                GlobalValue.SerialPortMgr.Send(SerialPortType.UniversalReset);
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
                GlobalValue.UniversalSerialPortOptData = new UniversalSerialPortOptEntity();
                GlobalValue.UniversalSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

                EnableControls(false);
                DisableRibbonBar();
                DisableNavigateBar();
                ShowWaitForm("", "正在设置时间...");
                BeginSerialPortDelegate();
                Application.DoEvents();
                SetStaticItem("正在设置时间...");
                GlobalValue.SerialPortMgr.Send(SerialPortType.UniversalSetTime);
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
                GlobalValue.UniversalSerialPortOptData = new UniversalSerialPortOptEntity();
                GlobalValue.UniversalSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

                EnableControls(false);
                DisableRibbonBar();
                DisableNavigateBar();
                ShowWaitForm("", "正在启动采集...");
                BeginSerialPortDelegate();
                Application.DoEvents();
                SetStaticItem("正在启动采集...");
                GlobalValue.SerialPortMgr.Send(SerialPortType.UniversalSetEnableCollect);
            }
            else
            {
            }
        }

        private void btnReadParm_Click(object sender, EventArgs e)
        {
            if (SwitchComunication.IsOn)
            {
                GlobalValue.UniversalSerialPortOptData = new UniversalSerialPortOptEntity();
                bool haveread = false;
                if (ceID.Checked)
                {
                    GlobalValue.UniversalSerialPortOptData.IsOptID = ceID.Checked;
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
                    GlobalValue.UniversalSerialPortOptData.ID = Convert.ToInt16(txtID.Text);
                    
                    if (ceTime.Checked)
                    {
                        GlobalValue.UniversalSerialPortOptData.IsOptDT = ceTime.Checked;
                        haveread = true;
                    }
                    if (ceCellPhone.Checked)
                    {
                        GlobalValue.UniversalSerialPortOptData.IsOptCellPhone = ceCellPhone.Checked;
                        haveread = true;
                    }
                    //if (ceModbusExeFlag.Checked)
                    //{
                    //    GlobalValue.UniversalSerialPortOptData.IsOptmodbusExeFlag = ceModbusExeFlag.Checked;
                    //    haveread = true;
                    //}
                    //if (ceBaud.Checked)
                    //    GlobalValue.UniversalSerialPortOptData.IsOptBaud = ceBaud.Checked;
                    if (ceComType.Checked)
                    {
                        GlobalValue.UniversalSerialPortOptData.IsOptComType = ceComType.Checked;
                        haveread = true;
                    }
                    if (ceIP.Checked)
                    {
                        GlobalValue.UniversalSerialPortOptData.IsOptIP = ceIP.Checked;
                        haveread = true;
                    }
                    if (cePort.Checked)
                    {
                        GlobalValue.UniversalSerialPortOptData.IsOptPort = cePort.Checked;
                        haveread = true;
                    }
                    if (ceColConfig.Checked){
                        GlobalValue.UniversalSerialPortOptData.IsOpt_CollectConfig = ceColConfig.Checked;
                        haveread = true;

                        ceSimulate1State.Checked =false;
                        ceSimulate2State.Checked = false;
                        ceRS485State.Checked = false;
                        cemodbusprotocolstatus.Checked = false;
                    }
                    if (ceCollectSimulate.Checked)
                    {
                        GlobalValue.UniversalSerialPortOptData.IsOpt_SimualteInterval = ceCollectSimulate.Checked;
                        haveread = true;
                    }
                    if (ceCollectRS485.Checked)
                    {
                        GlobalValue.UniversalSerialPortOptData.IsOpt_RS485Interval = ceCollectRS485.Checked;
                        haveread = true;
                    }
                    if (ceModbusExeFlag.Checked)
                    {
                        GlobalValue.UniversalSerialPortOptData.IsOpt_RS485Protocol = ceModbusExeFlag.Checked;
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
                GlobalValue.SerialPortMgr.Send(SerialPortType.UniversalReadBaicInfo);
            }
            else
            {
            }
        }

        void SerialPortMgr_SerialPortScheduleEvent(object sender, SerialPortScheduleEventArgs e)
        {
            if ((e.OptType == SerialPortType.UniversalReadBaicInfo || e.OptType == SerialPortType.UniversalSetBasicInfo) && !string.IsNullOrEmpty(e.Msg))
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
                    GlobalValue.UniversalSerialPortOptData = new UniversalSerialPortOptEntity();
                    bool haveset = false;
                    if (ceID.Checked)
                    {
                        GlobalValue.UniversalSerialPortOptData.IsOptID = ceID.Checked;
                        GlobalValue.UniversalSerialPortOptData.ID = Convert.ToInt16(txtID.Text);
                        haveset = true;
                    }
                    else
                    {
                        GlobalValue.UniversalSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

                        if (ceCellPhone.Checked)
                        {
                            GlobalValue.UniversalSerialPortOptData.IsOptCellPhone = ceCellPhone.Checked;
                            GlobalValue.UniversalSerialPortOptData.CellPhone = txtCellPhone.Text;
                            haveset = true;
                        }
                        //if (ceColConfig.Checked)
                        //{
                        //    GlobalValue.UniversalSerialPortOptData.IsOptmodbusExeFlag = ceColConfig.Checked;
                        //    GlobalValue.UniversalSerialPortOptData.ModbusExeFlag = cemodbusprotocolstatus.Checked;
                        //    haveset = true;
                        //}
                        if (ceComType.Checked)
                        {
                            GlobalValue.UniversalSerialPortOptData.IsOptComType = ceComType.Checked;
                            GlobalValue.UniversalSerialPortOptData.ComType = cbComType.SelectedIndex;
                            haveset = true;
                        }
                        if (ceIP.Checked)
                        {
                            GlobalValue.UniversalSerialPortOptData.IsOptIP = ceIP.Checked;
                            GlobalValue.UniversalSerialPortOptData.IP = new int[4];
                            GlobalValue.UniversalSerialPortOptData.IP[0] = Convert.ToInt32(txtNum1.Text);
                            GlobalValue.UniversalSerialPortOptData.IP[1] = Convert.ToInt32(txtNum2.Text);
                            GlobalValue.UniversalSerialPortOptData.IP[2] = Convert.ToInt32(txtNum3.Text);
                            GlobalValue.UniversalSerialPortOptData.IP[3] = Convert.ToInt32(txtNum4.Text);
                            haveset = true;
                        }
                        if (cePort.Checked)
                        {
                            GlobalValue.UniversalSerialPortOptData.IsOptPort = cePort.Checked;
                            GlobalValue.UniversalSerialPortOptData.Port = Convert.ToInt32(txtPort.Text);
                            haveset = true;
                        }
                        if (ceColConfig.Checked)
                        {
                            GlobalValue.UniversalSerialPortOptData.IsOpt_CollectConfig = ceColConfig.Checked;
                            if (ceSimulate1State.Checked)
                                GlobalValue.UniversalSerialPortOptData.Collect_Simulate1 = true;
                            if (ceSimulate2State.Checked)
                                GlobalValue.UniversalSerialPortOptData.Collect_Simulate2 = true;
                            if (ceRS485State.Checked)
                                GlobalValue.UniversalSerialPortOptData.Collect_RS485 = true;

                            //GlobalValue.UniversalSerialPortOptData.IsOptmodbusExeFlag = ceColConfig.Checked;
                            GlobalValue.UniversalSerialPortOptData.ModbusExeFlag = cemodbusprotocolstatus.Checked;
                            haveset = true;
                        }
                        if (ceCollectSimulate.Checked)
                        {
                            GlobalValue.UniversalSerialPortOptData.IsOpt_SimualteInterval = ceCollectSimulate.Checked;
                            GlobalValue.UniversalSerialPortOptData.Simulate_Interval = gridControl_Simulate.DataSource as DataTable;
                            haveset = true;
                        }
                        if (ceCollectRS485.Checked)
                        {
                            GlobalValue.UniversalSerialPortOptData.IsOpt_RS485Interval = ceCollectRS485.Checked;
                            GlobalValue.UniversalSerialPortOptData.RS485_Interval =gridControl_RS485.DataSource as DataTable;
                            haveset = true;
                        }
                        if (ceModbusExeFlag.Checked)
                        {
                            GlobalValue.UniversalSerialPortOptData.IsOpt_RS485Protocol = ceModbusExeFlag.Checked;
                            GlobalValue.UniversalSerialPortOptData.RS485Protocol =gridControl_485protocol.DataSource as DataTable;
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
                    GlobalValue.SerialPortMgr.Send(SerialPortType.UniversalSetBasicInfo);
                }
            }
            else
            {
                #region GPRS校验
                if (!Regex.IsMatch(txtID.Text, @"^\d{1,5}$"))
                {
                    XtraMessageBox.Show("请输入设备ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtID.Focus();
                    return ;
                }
                bool haveset = false;
                if (ceCollectSimulate.Checked)
                {
                    DataTable dt = gridControl_Simulate.DataSource as DataTable;
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        XtraMessageBox.Show("请填写模拟量时间间隔!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        gridView_Simulate.Focus();
                        return ;
                    }
                    haveset = true;
                }

                if (ceCollectRS485.Checked)
                {
                    DataTable dt = gridControl_RS485.DataSource as DataTable;
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        XtraMessageBox.Show("请填写RS485时间间隔表!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        gridView_RS485.Focus();
                        return ;
                    }
                    haveset = true;
                }
                if (!haveset)
                {
                    XtraMessageBox.Show("请选择需要设置的参数!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                #endregion

                TerminalDataBLL terBll = new TerminalDataBLL();
                List<Package> lstPack = new List<Package>();
                if (ceCollectSimulate.Checked)
                {
                    DataTable dt = gridControl_Simulate.DataSource as DataTable;
                    Package pack = GlobalValue.Universallog.GetSimulateIntervalPackage(Convert.ToInt16(txtID.Text), dt);
                    lstPack.Add(pack);
                }
                if (ceCollectRS485.Checked)
                {
                    DataTable dt = gridControl_RS485.DataSource as DataTable;
                    Package pack = GlobalValue.Universallog.GetRS485IntervalPackage(Convert.ToInt16(txtID.Text), dt);
                    lstPack.Add(pack);
                }

                foreach (Package pack in lstPack)
                {
                    if (!terBll.InsertDevGPRSParm(pack.DevID, Convert.ToInt32(pack.DevType), Convert.ToInt32(pack.C0), Convert.ToInt32(pack.C1), ConvertHelper.ByteToString(pack.Data, pack.DataLength)))
                    {
                        XtraMessageBox.Show("参数保存失败!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                ceCollectSimulate.Checked = false; ceCollectRS485.Checked = false;
                XtraMessageBox.Show("参数保存成功，等待传输!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void btnCalibrationSimualte1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                XtraMessageBox.Show("请先填写终端ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return;
            }
            if (SwitchComunication.IsOn)
            {
                GlobalValue.UniversalSerialPortOptData = new UniversalSerialPortOptEntity();
                GlobalValue.UniversalSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

                EnableControls(false);
                DisableRibbonBar();
                DisableNavigateBar();
                ShowWaitForm("", "正在校准第一路模拟量...");
                BeginSerialPortDelegate();
                Application.DoEvents();
                SetStaticItem("正在校准第一路模拟量...");
                GlobalValue.SerialPortMgr.Send(SerialPortType.UniversalCalibrationSimualte1);
            }
            else
            {
            }
        }

        private void btnCalibrationSimualte2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                XtraMessageBox.Show("请先填写终端ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return;
            }
            if (SwitchComunication.IsOn)
            {
                GlobalValue.UniversalSerialPortOptData = new UniversalSerialPortOptEntity();
                GlobalValue.UniversalSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

                EnableControls(false);
                DisableRibbonBar();
                DisableNavigateBar();
                ShowWaitForm("", "正在校准第二路模拟量...");
                BeginSerialPortDelegate();
                Application.DoEvents();
                SetStaticItem("正在校准第二路模拟量...");
                GlobalValue.SerialPortMgr.Send(SerialPortType.UniversalCalibrationSimualte2);
            }
            else
            {
            }
        }

        public override void OnSerialPortNotify(object sender, SerialPortEventArgs e)
        {
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.UniversalReset)
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
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.UniversalSetTime)
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
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.UniversalSetEnableCollect)
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
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.UniversalReadBaicInfo)
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

                    if (GlobalValue.UniversalSerialPortOptData.IsOptID)
                    {
                        txtID.Text = GlobalValue.UniversalSerialPortOptData.ID.ToString();
                    }
                    if (GlobalValue.UniversalSerialPortOptData.IsOptDT)
                    {
                        txtTime.Text = GlobalValue.UniversalSerialPortOptData.DT.ToString();
                    }
                    if (GlobalValue.UniversalSerialPortOptData.IsOptCellPhone)
                    {
                        txtCellPhone.Text = GlobalValue.UniversalSerialPortOptData.CellPhone;
                    }
                    //if (GlobalValue.UniversalSerialPortOptData.IsOptmodbusExeFlag)
                    //{
                    //    ceModbusExeFlag.Checked = GlobalValue.UniversalSerialPortOptData.ModbusExeFlag;
                    //}
                    //if (GlobalValue.UniversalSerialPortOptData.IsOptBaud)
                    //{
                    //    cbBaudRate.Text = GlobalValue.UniversalSerialPortOptData.Baud.ToString();
                    //}
                    if (GlobalValue.UniversalSerialPortOptData.IsOptComType)
                    {
                        cbComType.SelectedIndex = GlobalValue.UniversalSerialPortOptData.ComType;
                    }
                    if (GlobalValue.UniversalSerialPortOptData.IsOptIP)
                    {
                        txtNum1.Text = GlobalValue.UniversalSerialPortOptData.IP[0].ToString();
                        txtNum2.Text = GlobalValue.UniversalSerialPortOptData.IP[1].ToString();
                        txtNum3.Text = GlobalValue.UniversalSerialPortOptData.IP[2].ToString();
                        txtNum4.Text = GlobalValue.UniversalSerialPortOptData.IP[3].ToString();
                    }
                    if (GlobalValue.UniversalSerialPortOptData.IsOptPort)
                    {
                        txtPort.Text = GlobalValue.UniversalSerialPortOptData.Port.ToString();
                    }
                    if (GlobalValue.UniversalSerialPortOptData.IsOpt_CollectConfig)
                    {
                        ceSimulate1State.Checked =GlobalValue.UniversalSerialPortOptData.Collect_Simulate1;
                        ceSimulate2State.Checked = GlobalValue.UniversalSerialPortOptData.Collect_Simulate2;
                        ceRS485State.Checked = GlobalValue.UniversalSerialPortOptData.Collect_RS485;
                        cemodbusprotocolstatus.Checked = GlobalValue.UniversalSerialPortOptData.ModbusExeFlag;
                    }
                    if (GlobalValue.UniversalSerialPortOptData.IsOpt_SimualteInterval)
                    {
                        gridControl_Simulate.DataSource=GlobalValue.UniversalSerialPortOptData.Simulate_Interval;
                    }
                    if (GlobalValue.UniversalSerialPortOptData.IsOpt_RS485Interval)
                    {
                        gridControl_RS485.DataSource = GlobalValue.UniversalSerialPortOptData.RS485_Interval;
                    }
                    if (GlobalValue.UniversalSerialPortOptData.IsOpt_RS485Protocol)
                    {
                        gridControl_485protocol.DataSource = GlobalValue.UniversalSerialPortOptData.RS485Protocol;
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
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.UniversalSetBasicInfo)
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
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.UniversalCalibrationSimualte1)
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

                    XtraMessageBox.Show("校准第一路模拟量成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    XtraMessageBox.Show("校准第一路模拟量失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.UniversalCalibrationSimualte2)
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

                    XtraMessageBox.Show("校准第二路模拟量成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    XtraMessageBox.Show("校准第二路模拟量失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            btnCalibrationSimualte1.Enabled = enable;
            btnCalibrationSimualte2.Enabled = enable;
            SwitchComunication.Enabled = enable;
        }

        private bool Validate()
        {
            if (!Regex.IsMatch(txtID.Text,@"^\d{1,5}$"))
            {
                XtraMessageBox.Show("请输入设备ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return false;
            }

            if (ceCellPhone.Checked && !Regex.IsMatch(txtCellPhone.Text, @"^1\d{10}$"))
            {
                XtraMessageBox.Show("请输入手机号码[1XX XXXX XXXX]!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCellPhone.Focus();
                return false;
            }

            if (ceModbusExeFlag.Checked)
            {
                DataTable dt = gridControl_485protocol.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0)
                {
                    XtraMessageBox.Show("请填写RS485采集modbus协议配置表!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    gridView_485protocol.Focus();
                    return false;
                }
            }

            //if (ceBaud.Checked && !Regex.IsMatch(cbBaudRate.Text,@"^$"))
            //{
            //    XtraMessageBox.Show("请选择波特率!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    cbBaudRate.Focus();
            //    return false;
            //}

            if (ceComType.Checked && string.IsNullOrEmpty(cbComType.Text))
            {
                XtraMessageBox.Show("请选择通信方式!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbComType.Focus();
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

            if (cePort.Checked && !Regex.IsMatch(txtPort.Text,@"^\d{3,5}$"))
            {
                XtraMessageBox.Show("请输入端口号!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPort.Focus();
                return false;
            }

            if (ceCollectSimulate.Checked)
            {
                DataTable dt = gridControl_Simulate.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0)
                {
                    XtraMessageBox.Show("请填写模拟量时间间隔!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    gridView_Simulate.Focus();
                    return false;
                }
            }

            if (ceCollectRS485.Checked)
            {
                DataTable dt = gridControl_RS485.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0)
                {
                    XtraMessageBox.Show("请填写RS485时间间隔表!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    gridView_RS485.Focus();
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
                btnCalibrationSimualte1.Enabled = Enabled;
                btnCalibrationSimualte2.Enabled = Enabled;
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
            btnCalibrationSimualte1.Enabled = GlobalValue.portUtil.IsOpen;
            btnCalibrationSimualte2.Enabled = GlobalValue.portUtil.IsOpen;

            ceTime.Enabled = true;
            txtTime.Enabled = true;
            txtTime.Text = "";
            //ceCellPhone.Enabled = true;
            txtCellPhone.Enabled = true;
            txtCellPhone.Text = "";
            ceModbusExeFlag.Enabled = true;
            ceCollectRS485.Enabled = true;
            //ceBaud.Enabled = true;
            //cbBaudRate.Enabled = true;
            //cbBaudRate.SelectedIndex = -1;

            //ceComType.Enabled = true;
            cbComType.Enabled = true;
            cbComType.SelectedIndex = -1;
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

            ceColConfig.Enabled = true;
            ceSimulate1State.Enabled = true;
            ceSimulate1State.Checked = false;
            ceSimulate2State.Enabled = true;
            ceSimulate2State.Checked = false;
            ceRS485State.Enabled = true;
            ceRS485State.Checked = false;
            cemodbusprotocolstatus.Enabled = true;
            cemodbusprotocolstatus.Checked = false;
        }

        private void SetGprsCtrlStatus()
        {
            btnReset.Enabled = false;
            btnCheckingTime.Enabled = false;
            btnEnableCollect.Enabled = false;
            btnReadParm.Enabled = false;
            btnSetParm.Enabled = true;
            btnCalibrationSimualte1.Enabled = false;
            btnCalibrationSimualte2.Enabled = false;

            ceTime.Enabled = false;
            ceTime.Checked = false;
            txtTime.Enabled = false;
            txtTime.Text = "";
            ceCellPhone.Enabled = false;
            ceCellPhone.Checked = false;
            txtCellPhone.Enabled = false;
            txtCellPhone.Text = "";
            ceModbusExeFlag.Enabled = false;
            ceModbusExeFlag.Checked = false;
            //ceCollectRS485.Enabled = false;
            //ceCollectRS485.Checked = false;
            //ceBaud.Enabled = false;
            //ceBaud.Checked = false;
            //cbBaudRate.Enabled = false;
            //cbBaudRate.SelectedIndex = -1;
            ceComType.Enabled = false;
            ceComType.Checked = false;
            cbComType.Enabled = false;
            cbComType.SelectedIndex = -1;
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

            ceColConfig.Enabled = false;
            ceColConfig.Checked = false;
            ceSimulate1State.Enabled = false;
            ceSimulate1State.Checked = false;
            ceSimulate2State.Enabled = false;
            ceSimulate2State.Checked = false;
            ceRS485State.Enabled = false;
            ceRS485State.Checked = false;
            cemodbusprotocolstatus.Enabled = false;
            cemodbusprotocolstatus.Checked = false;
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
        

    }
}
