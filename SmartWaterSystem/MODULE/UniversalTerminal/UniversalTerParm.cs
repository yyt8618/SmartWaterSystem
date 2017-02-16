using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Text.RegularExpressions;
using DevExpress.XtraGrid.Views.Grid;
using Entity;
using Common;
using DevExpress.XtraTreeList.Nodes;
using System.Collections.Generic;

namespace SmartWaterSystem
{
    public partial class UniversalTerParm : BaseView,IUniversalTerParm
    {
        private ConstValue.DEV_TYPE _DevType = ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
        public ConstValue.DEV_TYPE DevType
        {
            set
            {
                _DevType = value;
                SetDevTypeEnable();
            }
            get { return _DevType; }
        }
        public UniversalTerParm()
        {
            InitializeComponent();
            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;

            timer_GetWaitCmd.Tick += new EventHandler(timer_GetWaitCmd_Tick);

            #region Init Simulate GridView
            cb_sim_starttime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            for (int i = 0; i < 24; i++)
                cb_sim_starttime.Items.Add(i);

            //cb_sim_coltime1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_sim_coltime1.Items.Add(20);
            cb_sim_coltime1.Items.Add(30);
            cb_sim_coltime1.Items.Add(60);
            cb_sim_coltime1.Items.Add(120);
            cb_sim_coltime1.Items.Add(240);
            cb_sim_coltime1.Items.Add(300);
            cb_sim_coltime1.Items.Add(900);

            //cb_sim_coltime2.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_sim_coltime2.Items.Add(20);
            cb_sim_coltime2.Items.Add(30);
            cb_sim_coltime2.Items.Add(60);
            cb_sim_coltime2.Items.Add(120);
            cb_sim_coltime2.Items.Add(240);
            cb_sim_coltime2.Items.Add(300);
            cb_sim_coltime2.Items.Add(900);

            cb_sim_sendtime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_sim_sendtime.Items.Add(2);
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
            
            //cb_pluse_coltime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_pluse_coltime.Items.Add(20);
            cb_pluse_coltime.Items.Add(30);
            cb_pluse_coltime.Items.Add(60);
            cb_pluse_coltime.Items.Add(120);
            cb_pluse_coltime.Items.Add(240);
            cb_pluse_coltime.Items.Add(300);
            cb_pluse_coltime.Items.Add(900);

            //cb_pre_coltime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_pre_coltime.Items.Add(20);
            cb_pre_coltime.Items.Add(30);
            cb_pre_coltime.Items.Add(60);
            cb_pre_coltime.Items.Add(120);
            cb_pre_coltime.Items.Add(240);
            cb_pre_coltime.Items.Add(300);
            cb_pre_coltime.Items.Add(900);

            cb_pluse_sendtime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_pluse_sendtime.Items.Add(2);
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

            //cb_RS485_coltime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_RS485_coltime.Items.Add(20);
            cb_RS485_coltime.Items.Add(30);
            cb_RS485_coltime.Items.Add(60);
            cb_RS485_coltime.Items.Add(120);
            cb_RS485_coltime.Items.Add(240);
            cb_RS485_coltime.Items.Add(300);
            cb_RS485_coltime.Items.Add(900);

            cb_RS485_sendtime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cb_RS485_sendtime.Items.Add(2);
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

            Inittree();
        }

        private void UniversalTerParm_Load(object sender, EventArgs e)
        {
            InitGridView();
            InitControls();

            cbComType.SelectedIndex = 1;
            cbPreFlag.SelectedIndex = 0;
        }

        void timer_GetWaitCmd_Tick(object sender, EventArgs e)
        {
            GlobalValue.SocketMgr.SendMessage(new SocketEntity(ConstValue.MSMQTYPE.Get_P68_WaitSendCmd, ""));      //获取待发送命令
        }

        private void cbBaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {
        }


        private void InitGridView()
        {
            gridControl_Simulate.DataSource = null;
            gridControl_Pluse.DataSource = null;
            gridControl_RS485.DataSource = null;
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

            ceCollectModbus.Checked = false;
            gridControl_485protocol.Enabled = false;

        }

        #region treeSocketType
        private void Inittree()
        {
            DataTable dttree = new DataTable();
            dttree.Columns.Add("ID", typeof(int));
            dttree.Columns.Add("Name");
            dttree.Columns.Add("ParentID", typeof(int));

            int tmpparent = -1;
            DataRow dr = dttree.NewRow();
            dr["ID"] = -99;
            dr["Name"] = "全部";
            dr["ParentID"] = -999;
            dttree.Rows.Add(dr);
            
            dr = dttree.NewRow();
            dr["ID"] = 1;
            dr["Name"] = "压力";
            dr["ParentID"] = -99;
            dttree.Rows.Add(dr);
            dr = dttree.NewRow();
            dr["ID"] = 2;
            dr["Name"] = "脉冲量";
            dr["ParentID"] = -99;
            dttree.Rows.Add(dr);

            dr = dttree.NewRow();
            dr["ID"] = 3;
            tmpparent = 3;
            dr["Name"] = "模拟量";
            dr["ParentID"] = -99;
            dttree.Rows.Add(dr);
            dr = dttree.NewRow();
            dr["ID"] = 31;
            dr["Name"] = "第1路模拟量";
            dr["ParentID"] = tmpparent;
            dttree.Rows.Add(dr);
            dr = dttree.NewRow();
            dr["ID"] = 32;
            dr["Name"] = "第2路模拟量";
            dr["ParentID"] = tmpparent;
            dttree.Rows.Add(dr);

            dr = dttree.NewRow();
            dr["ID"] = 4;
            tmpparent = 4;
            dr["Name"] = "485";
            dr["ParentID"] = -99;
            dttree.Rows.Add(dr);
            dr = dttree.NewRow();
            dr["ID"] = 41;
            dr["Name"] = "第1路485";
            dr["ParentID"] = tmpparent;
            dttree.Rows.Add(dr);
            dr = dttree.NewRow();
            dr["ID"] = 42;
            dr["Name"] = "第2路485";
            dr["ParentID"] = tmpparent;
            dttree.Rows.Add(dr);
            dr = dttree.NewRow();
            dr["ID"] = 43;
            dr["Name"] = "第3路485";
            dr["ParentID"] = tmpparent;
            dttree.Rows.Add(dr);
            dr = dttree.NewRow();
            dr["ID"] = 44;
            dr["Name"] = "第4路485";
            dr["ParentID"] = tmpparent;
            dttree.Rows.Add(dr);


            treeSocketType.Properties.DataSource = dttree;
            treeSocketType.Properties.ValueMember = "ID";
            treeSocketType.Properties.DisplayMember = "Name";

            treeSocketType.Properties.TreeList.ParentFieldName = "ParentID";
            treeSocketType.Properties.TreeList.KeyFieldName = "ID";

            if (treeSocketType.Properties.TreeList.Nodes != null)
            {
                foreach (TreeListNode node in treeSocketType.Properties.TreeList.GetNodeList())
                {
                    node.Checked = true;
                }
            }

            treeSocketType.Properties.TreeList.AfterCheckNode += (s, a) =>
            {
                a.Node.Selected = true;
                UpdateParentNodesCheckstate(a.Node, a.Node.Checked);
                UpdateChildsNodesCheckstate(a.Node, a.Node.Checked);
            };
        }

        /// <summary>
        /// 更新父节点的选中状态
        /// </summary>
        /// <param name="node"></param>
        /// <param name="Checked"></param>
        private void UpdateParentNodesCheckstate(TreeListNode node, bool Checked)
        {
            if (node.ParentNode != null)
            {

                bool childcheck = false, childuncheck = false;    //除去自身是否有选择或未选中的孩子节点
                foreach (TreeListNode child in node.ParentNode.Nodes)
                {
                    if (child.CheckState != CheckState.Unchecked)
                        childcheck = true;
                    else
                        childuncheck = true;
                }
                if (Checked)   //如果当前节点选中,则父节点需要改变为中间态或者选中状态
                {
                    if (childuncheck)
                        node.ParentNode.CheckState = CheckState.Indeterminate;
                    else
                        node.ParentNode.CheckState = CheckState.Checked;
                }
                else
                {
                    if (childcheck)
                        node.ParentNode.CheckState = CheckState.Indeterminate;
                    else
                        node.ParentNode.CheckState = CheckState.Unchecked;
                }
                UpdateParentNodesCheckstate(node.ParentNode, Checked);
            }
        }

        /// <summary>
        /// 更新孩子节点的选中状态
        /// </summary>
        /// <param name="node"></param>
        /// <param name="Checked"></param>
        private void UpdateChildsNodesCheckstate(TreeListNode node, bool Checked)
        {
            if (node.Nodes != null && node.Nodes.Count > 0)
            {
                foreach (TreeListNode child in node.Nodes)
                {
                    if (Checked)
                        child.CheckState = CheckState.Checked;
                    else
                        child.CheckState = CheckState.Unchecked;

                    UpdateChildsNodesCheckstate(child, Checked);
                }
                if (Checked)
                    node.CheckState = CheckState.Checked;
                else
                    node.CheckState = CheckState.Unchecked;
            }
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

        #region Pluse GridView
        int pluserowindex = -1;
        private void gridView_Pluse_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                GridView gridview = sender as GridView;
                if ((!string.IsNullOrEmpty(gridview.GetRowCellValue(e.RowHandle, "starttime").ToString())) &&
                    (!string.IsNullOrEmpty(gridview.GetRowCellValue(e.RowHandle, "precollecttime").ToString())) &&
                    (!string.IsNullOrEmpty(gridview.GetRowCellValue(e.RowHandle, "plusecollecttime").ToString())) &&
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
        private void gridView_485protocol_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                GridView gridview = sender as GridView;
                int rowindex = e.RowHandle;
                if ((!string.IsNullOrEmpty(gridview.GetRowCellValue(rowindex, "baud").ToString())) &&
                    (gridview.RowCount < 4))
                {
                    if ((gridview.RowCount == (rowindex + 1)) && (rowindex > -1))
                        gridview.AddNewRow();
                }
            }
        }
        #endregion

        private void ceColConfig_CheckedChanged(object sender, EventArgs e)
        {
            //gridControl_Simulate.Enabled = ceColConfig.Checked & ceCollectSimulate.Checked;
            //gridControl_Pluse.Enabled = ceColConfig.Checked & ceCollectPluse.Checked;
            //gridControl_RS485.Enabled = ceColConfig.Checked & ceCollectRS485.Checked;
            ceDigitPreStatus.Enabled = ceColConfig.Checked;
            cePluseState.Enabled = ceColConfig.Checked;
            ceRS485State.Enabled = ceColConfig.Checked;
            ceSimulate1State.Enabled = ceColConfig.Checked;
            ceSimulate2State.Enabled = ceColConfig.Checked;

            if (!ceColConfig.Checked)
            {
                ceDigitPreStatus.Checked = ceColConfig.Checked;
                cePluseState.Checked = ceColConfig.Checked;
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

        private void ceCollectPluse_CheckedChanged(object sender, EventArgs e)
        {
            gridControl_Pluse.Enabled = ceCollectPluse.Checked ;
            if (!ceCollectPluse.Checked)
                gridControl_Pluse.DataSource = null;
            else
            {
                DataTable dt_pluse = new DataTable();
                dt_pluse.Columns.Add("starttime");
                dt_pluse.Columns.Add("precollecttime");
                dt_pluse.Columns.Add("plusecollecttime");
                dt_pluse.Columns.Add("sendtime");
                gridControl_Pluse.DataSource = dt_pluse;
                gridView_Pluse.AddNewRow();
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

        private void ceCollectModbus_CheckedChanged(object sender, EventArgs e)
        {
            gridControl_485protocol.Enabled = ceCollectModbus.Checked;
            if (!ceCollectModbus.Checked)
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

        private void btnReset_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
            GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity(this.DevType);
            GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

            //GlobalValue.UniSerialPortOptData.ResetType = 
            if (e.Item.Caption == "恢复所有出厂设置")
                GlobalValue.UniSerialPortOptData.ResetType = 0x01;
            else if (e.Item.Caption == "恢复除IP端口外设置")
                GlobalValue.UniSerialPortOptData.ResetType = 0x02;
            else  //系统复位
                GlobalValue.UniSerialPortOptData.ResetType = 0x03;

            EnableControls(false);
            DisableRibbonBar();
            DisableNavigateBar();
            ShowWaitForm("", "正在复位终端...");
            BeginSerialPortDelegate();
            Application.DoEvents();
            SetStaticItem("正在复位终端...");
            Send(SerialPortType.UniversalReset);
        }


        private void btnCheckingTime_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                XtraMessageBox.Show("请先填写终端ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return;
            }
            GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity(this.DevType);
            GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

            EnableControls(false);
            DisableRibbonBar();
            DisableNavigateBar();
            ShowWaitForm("", "正在设置时间...");
            BeginSerialPortDelegate();
            Application.DoEvents();
            SetStaticItem("正在设置时间...");
            Send(SerialPortType.UniversalSetTime);
        }

        private void btnEnableCollect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                XtraMessageBox.Show("请先填写终端ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return;
            }
            GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity(this.DevType);
            GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

            EnableControls(false);
            DisableRibbonBar();
            DisableNavigateBar();
            ShowWaitForm("", "正在启动采集...");
            BeginSerialPortDelegate();
            Application.DoEvents();
            SetStaticItem("正在启动采集...");
            Send(SerialPortType.UniversalSetEnableCollect);
        }

        private void btnReadParm_Click(object sender, EventArgs e)
        {
            GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity(this.DevType);

            bool haveread = false;
            if (ceID.Checked)
            {
                GlobalValue.UniSerialPortOptData.IsOptID = ceID.Checked;
                haveread = true;
            }
            else
            {
                GlobalValue.UniSerialPortOptData.FlagType = (UniversalFlagType)cbPreFlag.SelectedIndex;
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
                if (ceCellPhone.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptCellPhone = ceCellPhone.Checked;
                    haveread = true;
                }

                if (ceComType.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptComType = ceComType.Checked;
                    haveread = true;
                }
                if (ceNetWorkType.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_NetWorkType = ceNetWorkType.Checked;
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
                if (ceHeart.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_HeartInterval = ceHeart.Checked;
                    haveread = true;
                }
                if (ce485Baud.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_Baud485 = ce485Baud.Checked;
                    haveread = true;
                }
                if (ceModbusExeFlag.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOptModbusExeFlag = ceModbusExeFlag.Checked;
                    haveread = true;
                }
                if (ceVolInterval.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_VolInterval = ceVolInterval.Checked;
                    haveread = true;
                }
                if (ceVolLower.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_VolLower = ceVolLower.Checked;
                    haveread = true;
                }
                if (ceSMSInterval.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_SMSInterval = ceSMSInterval.Checked;
                    haveread = true;
                }
                if (cePluseUnit.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_PluseUnit = cePluseUnit.Checked;
                    haveread = true;
                }
                if(ceAlarmLen.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_AlarmLen = ceAlarmLen.Checked;
                    haveread = true;
                }
                if (cePreRange.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_Range = cePreRange.Checked;
                    haveread = true;
                }
                if (ceOffset.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_PreOffset = ceOffset.Checked;
                    haveread = true;
                }
                if (cePreUpLimit.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_UpLimit = cePreUpLimit.Checked;
                    haveread = true;
                }
                if (cePreUpLimitEnable.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_UpLimitEnable = cePreUpLimitEnable.Checked;
                    haveread = true;
                }
                if (cePreLowLimit.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_LowLimit = cePreLowLimit.Checked;
                    haveread = true;
                }
                if (cePreLowLimitEnable.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_LowLimitEnable = cePreLowLimitEnable.Checked;
                    haveread = true;
                }
                if (ceSlopUpLimit.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_SlopUpLimit = ceSlopUpLimit.Checked;
                    haveread = true;
                }
                if (ceSlopUpLimitEnable.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_SlopUpLimitEnable = ceSlopUpLimitEnable.Checked;
                    haveread = true;
                }
                if (ceSlopLowLimit.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_SlopLowLimit = ceSlopLowLimit.Checked;
                    haveread = true;
                }
                if (ceSlopLowLimitEnable.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_SlopLowLimitEnable = ceSlopLowLimitEnable.Checked;
                    haveread = true;
                }
                
                if (ceColConfig.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_CollectConfig = ceColConfig.Checked;

                    ceSimulate1State.Checked = false;
                    ceSimulate2State.Checked = false;
                    cePluseState.Checked = false;
                    ceRS485State.Checked = false;
                    ceDigitPreStatus.Checked = false;
                    haveread = true;
                }
                if (ceCollectSimulate.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_SimualteInterval = ceCollectSimulate.Checked;
                    haveread = true;
                }
                if (ceCollectPluse.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_PluseInterval = ceCollectPluse.Checked;
                    haveread = true;
                }
                if (ceCollectRS485.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_RS485Interval = ceCollectRS485.Checked;
                    haveread = true;
                }
                if (ceCollectModbus.Checked)
                {
                    GlobalValue.UniSerialPortOptData.IsOpt_RS485Protocol = ceCollectModbus.Checked;
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
            GlobalValue.SerialPortMgr.SerialPortScheduleEvent -= new SerialPortScheduleHandle(SerialPortParm_SerialPortScheduleEvent);
            GlobalValue.SerialPortMgr.SerialPortScheduleEvent += new SerialPortScheduleHandle(SerialPortParm_SerialPortScheduleEvent);
            Application.DoEvents();
            SetStaticItem("正在读取...");
            Send(SerialPortType.UniversalReadBasicInfo);
        }

        void SerialPortParm_SerialPortScheduleEvent(object sender, SerialPortScheduleEventArgs e)
        {
            if ((e.OptType == SerialPortType.UniversalReadBasicInfo || e.OptType == SerialPortType.UniversalSetBasicInfo) && !string.IsNullOrEmpty(e.Msg))
            {
                ShowWaitForm("", e.Msg);
                SetStaticItem(e.Msg);
            }

        }

        private void btnSetParm_Click(object sender, EventArgs e)
        {
            //if (SwitchComunication.IsOn)
            //{
                if (Validate())
                {
                    GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity(this.DevType);
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
                        GlobalValue.UniSerialPortOptData.FlagType = (UniversalFlagType)cbPreFlag.SelectedIndex;

                        if (ceCellPhone.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOptCellPhone = ceCellPhone.Checked;
                            GlobalValue.UniSerialPortOptData.CellPhone = txtCellPhone.Text;
                            haveset = true;
                        }
                        if (ceComType.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOptComType = ceComType.Checked;
                            GlobalValue.UniSerialPortOptData.ComType = cbComType.SelectedIndex;
                            haveset = true;
                        }
                        if (ceNetWorkType.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_NetWorkType = ceNetWorkType.Checked;
                            GlobalValue.UniSerialPortOptData.NetWorkType = cbNetworkType.SelectedIndex;
                            haveset = true;
                        }
                        if (ceIP.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOptIP = ceIP.Checked;
                            GlobalValue.UniSerialPortOptData.IP = new int[4];
                            string[] ips = txtIP.Text.Split('.');
                            if (ips != null && ips.Length == 4)
                            {
                                GlobalValue.UniSerialPortOptData.IP[0] = Convert.ToInt32(ips[0]);
                                GlobalValue.UniSerialPortOptData.IP[1] = Convert.ToInt32(ips[1]);
                                GlobalValue.UniSerialPortOptData.IP[2] = Convert.ToInt32(ips[2]);
                                GlobalValue.UniSerialPortOptData.IP[3] = Convert.ToInt32(ips[3]);
                            }
                            haveset = true;
                        }
                        if (cePort.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOptPort = cePort.Checked;
                            GlobalValue.UniSerialPortOptData.Port = Convert.ToInt32(txtPort.Text);
                            haveset = true;
                        }
                        if (ceHeart.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_HeartInterval = ceHeart.Checked;
                            GlobalValue.UniSerialPortOptData.HeartInterval = Convert.ToInt32(txtHeart.Text);
                            haveset = true;
                        }
                        if (ce485Baud.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_Baud485 = ce485Baud.Checked;
                            GlobalValue.UniSerialPortOptData.Baud485 = cb485BaudRate.SelectedIndex;
                            haveset = true;
                        }
                        if(ceModbusExeFlag.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOptModbusExeFlag = ceModbusExeFlag.Checked;
                            GlobalValue.UniSerialPortOptData.ModbusExeFlag = cbModbusExeFlag.SelectedIndex == 1 ? true : false;
                            haveset = true;
                        }
                        if (ceVolInterval.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_VolInterval = ceVolInterval.Checked;
                            GlobalValue.UniSerialPortOptData.VolInterval = Convert.ToInt32(txtVolInterval.Text);
                            haveset = true;
                        }
                        if (ceVolLower.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_VolLower = ceVolLower.Checked;
                            GlobalValue.UniSerialPortOptData.VolLower = Convert.ToInt16(txtVolLower.Text);
                            haveset = true;
                        }
                        if(ceSMSInterval.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_SMSInterval = ceSMSInterval.Checked;
                            GlobalValue.UniSerialPortOptData.SMSInterval = Convert.ToInt16(txtSMSInterval.Text);
                            haveset = true;
                        }
                        if(cePluseUnit.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_PluseUnit = cePluseUnit.Checked;
                            GlobalValue.UniSerialPortOptData.PluseUnit = cbPluseUnit.SelectedIndex;
                            haveset = true;
                        }
                        if(ceAlarmLen.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_AlarmLen = ceAlarmLen.Checked;
                            GlobalValue.UniSerialPortOptData.AlarmLen = Convert.ToInt16(txtAlarmLen.Text);
                            haveset = true;
                        }
                        if(cePreUpLimit.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_UpLimit = cePreUpLimit.Checked;
                            GlobalValue.UniSerialPortOptData.UpLimit = Convert.ToDouble(txtPreUpLimit.Text);
                            haveset = true;
                        }
                        if(cePreUpLimitEnable.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_UpLimitEnable = cePreUpLimitEnable.Checked;
                            GlobalValue.UniSerialPortOptData.UpLimitEnable = cbPreUpLimitEnable.SelectedIndex == 0 ? false : true;
                            haveset = true;
                        }

                        if (cePreLowLimit.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_LowLimit = cePreLowLimit.Checked;
                            GlobalValue.UniSerialPortOptData.LowLimit = Convert.ToDouble(txtPreLowLimit.Text);
                            haveset = true;
                        }
                        if (cePreLowLimitEnable.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_LowLimitEnable = cePreLowLimitEnable.Checked;
                            GlobalValue.UniSerialPortOptData.LowLimitEnable = cbPreLowLimitEnable.SelectedIndex == 0 ? false : true;
                            haveset = true;
                        }

                        if (ceSlopUpLimit.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_SlopUpLimit = ceSlopUpLimit.Checked;
                            GlobalValue.UniSerialPortOptData.SlopUpLimit = Convert.ToDouble(txtSlopUpLimit.Text);
                            haveset = true;
                        }
                        if (ceSlopUpLimitEnable.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_SlopUpLimitEnable = ceSlopUpLimitEnable.Checked;
                            GlobalValue.UniSerialPortOptData.SlopUpLimitEnable = cbSlopUpLimitEnable.SelectedIndex == 0 ? false : true;
                            haveset = true;
                        }

                        if (ceSlopLowLimit.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_SlopLowLimit = ceSlopLowLimit.Checked;
                            GlobalValue.UniSerialPortOptData.SlopLowLimit = Convert.ToDouble(txtSlopLowLimit.Text);
                            haveset = true;
                        }
                        if (ceSlopLowLimitEnable.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_SlopLowLimitEnable = ceSlopLowLimitEnable.Checked;
                            GlobalValue.UniSerialPortOptData.SlopLowLimitEnable = cbSlopLowLimitEnable.SelectedIndex == 0 ? false : true;
                            haveset = true;
                        }

                        if(cePreRange.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_Range = cePreRange.Checked;
                            GlobalValue.UniSerialPortOptData.Range = Convert.ToDouble(txtPreRange.Text);
                            haveset = true;
                        }

                        if(ceOffset.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_PreOffset = ceOffset.Checked;
                            GlobalValue.UniSerialPortOptData.PreOffset = Convert.ToDouble(txtOffset.Text);
                            haveset = true;
                        }

                        if (ceColConfig.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_CollectConfig = ceColConfig.Checked;
                            if (ceDigitPreStatus.Checked)
                                GlobalValue.UniSerialPortOptData.Collect_DigitPre = true;
                            if (cePluseState.Checked)
                                GlobalValue.UniSerialPortOptData.Collect_Pluse = true;
                            if (ceSimulate1State.Checked)
                                GlobalValue.UniSerialPortOptData.Collect_Simulate1 = true;
                            if (ceSimulate2State.Checked)
                                GlobalValue.UniSerialPortOptData.Collect_Simulate2 = true;
                            if (ceRS485State.Checked)
                                GlobalValue.UniSerialPortOptData.Collect_RS485 = true;

                            haveset = true;
                        }

                        if (ceCollectSimulate.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_SimualteInterval = ceCollectSimulate.Checked;
                            GlobalValue.UniSerialPortOptData.Simulate_Interval = gridControl_Simulate.DataSource as DataTable;
                            haveset = true;
                        }
                        if (ceCollectPluse.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_PluseInterval = ceCollectPluse.Checked;
                            GlobalValue.UniSerialPortOptData.Pluse_Interval = gridControl_Pluse.DataSource as DataTable;
                            haveset = true;
                        }
                        if (ceCollectRS485.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_RS485Interval = ceCollectRS485.Checked;
                            GlobalValue.UniSerialPortOptData.RS485_Interval =gridControl_RS485.DataSource as DataTable;
                            haveset = true;
                        }
                        if (ceCollectModbus.Checked)
                        {
                            GlobalValue.UniSerialPortOptData.IsOpt_RS485Protocol = ceCollectModbus.Checked;
                            GlobalValue.UniSerialPortOptData.RS485Protocol =gridControl_485protocol.DataSource as DataTable;
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
                    GlobalValue.SerialPortMgr.SerialPortScheduleEvent -= new SerialPortScheduleHandle(SerialPortParm_SerialPortScheduleEvent);
                    GlobalValue.SerialPortMgr.SerialPortScheduleEvent += new SerialPortScheduleHandle(SerialPortParm_SerialPortScheduleEvent);
                    Application.DoEvents();
                    SetStaticItem("正在设置...");
                    Send(SerialPortType.UniversalSetBasicInfo);
                }
        }

        private void barbtnCalibrationSimualte1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (DialogResult.No == XtraMessageBox.Show("终端出厂已校准，是否继续校准?", GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
            {
                return;
            }
            if (string.IsNullOrEmpty(txtID.Text))
            {
                XtraMessageBox.Show("请先填写终端ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return;
            }
            GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity(this.DevType);
            GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

            EnableControls(false);
            DisableRibbonBar();
            DisableNavigateBar();
            ShowWaitForm("", "正在校准第一路模拟量...");
            BeginSerialPortDelegate();
            Application.DoEvents();
            SetStaticItem("正在校准第一路模拟量...");
            Send(SerialPortType.UniversalCalibrationSimualte1);
        }

        private void barbtnCalibrationSimualte2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (DialogResult.No == XtraMessageBox.Show("终端出厂已校准，是否继续校准?", GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
            {
                return;
            }
            if (string.IsNullOrEmpty(txtID.Text))
            {
                XtraMessageBox.Show("请先填写终端ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return;
            }
            GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity(this.DevType);
            GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

            EnableControls(false);
            DisableRibbonBar();
            DisableNavigateBar();
            ShowWaitForm("", "正在校准第二路模拟量...");
            BeginSerialPortDelegate();
            Application.DoEvents();
            SetStaticItem("正在校准第二路模拟量...");
            Send(SerialPortType.UniversalCalibrationSimualte2);
        }

        /// <summary>
        /// 设置脉冲基准数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetPluseBasic_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                XtraMessageBox.Show("请先填写终端ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return;
            }
            GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity(this.DevType);
            PluseBasicInputForm PluseBasicForm = new PluseBasicInputForm();
            if (DialogResult.OK != PluseBasicForm.ShowDialog())
            {
                return;
            }
            GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);
            GlobalValue.UniSerialPortOptData.SetPluseBasic1 = PluseBasicForm.SetPluseBasic1;
            GlobalValue.UniSerialPortOptData.SetPluseBasic2 = PluseBasicForm.SetPluseBasic2;
            GlobalValue.UniSerialPortOptData.SetPluseBasic3 = PluseBasicForm.SetPluseBasic3;
            GlobalValue.UniSerialPortOptData.SetPluseBasic4 = PluseBasicForm.SetPluseBasic4;
            GlobalValue.UniSerialPortOptData.PluseBasic1 = PluseBasicForm.PluseBasic1;
            GlobalValue.UniSerialPortOptData.PluseBasic2 = PluseBasicForm.PluseBasic2;
            GlobalValue.UniSerialPortOptData.PluseBasic3 = PluseBasicForm.PluseBasic3;
            GlobalValue.UniSerialPortOptData.PluseBasic4 = PluseBasicForm.PluseBasic4;

            EnableControls(false);
            DisableRibbonBar();
            DisableNavigateBar();
            ShowWaitForm("", "正在设置脉冲基准...");
            BeginSerialPortDelegate();
            Application.DoEvents();
            SetStaticItem("正在设置脉冲基准...");
            Send(SerialPortType.UniversalPluseBasic);
        }
        
        private void btnEnableAlarm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                XtraMessageBox.Show("请先填写终端ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return;
            }

            CancleAlarmForm cancleAlarmForm = new CancleAlarmForm();
            if (DialogResult.OK != cancleAlarmForm.ShowDialog())
            {
                return;
            }
            GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity(this.DevType);
            GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

            EnableControls(false);
            DisableRibbonBar();
            DisableNavigateBar();

            if (cancleAlarmForm.CancleHours == -1)
                GlobalValue.UniSerialPortOptData.IsOpt_AlarmLen = false;
            else
            {
                GlobalValue.UniSerialPortOptData.IsOpt_AlarmLen = true;
                GlobalValue.UniSerialPortOptData.AlarmLen = cancleAlarmForm.CancleHours;
            }
            GlobalValue.UniSerialPortOptData.EnableAlarm = cancleAlarmForm.IsEnable;
            ShowWaitForm("", "正在启用报警...");
            BeginSerialPortDelegate();
            Application.DoEvents();
            SetStaticItem("正在启用报警...");
            Send(SerialPortType.UniversalEnableAlarm);
        }

        public override void OnSerialPortNotify(object sender, SerialPortEventArgs e)
        {
            //当选择的是GPRS时，后边串口的判断不需要执行
            if(e.TransactStatus != TransStatus.Start && GlobalValue.Universallog.RWType == RWFunType.GPRS)
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

                    //获取未发送列表
                    XtraMessageBox.Show("成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    XtraMessageBox.Show("失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
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
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.UniversalReadVer)
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

                    XtraMessageBox.Show("版本号:"+ GlobalValue.UniSerialPortOptData.Ver, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    XtraMessageBox.Show("读取版本号失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.UniversalReadFiledStrength)
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

                    XtraMessageBox.Show(GlobalValue.UniSerialPortOptData.FieldStrength, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    XtraMessageBox.Show("读取场强\\电压失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.UniversalReadBasicInfo)
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

                    try
                    {
                        if (GlobalValue.UniSerialPortOptData.IsOptID)
                            txtID.Text = GlobalValue.UniSerialPortOptData.ID.ToString();
                        if (GlobalValue.UniSerialPortOptData.IsOptDT)
                            txtTime.Text = GlobalValue.UniSerialPortOptData.DT.ToString();
                        if (GlobalValue.UniSerialPortOptData.IsOptCellPhone)
                            txtCellPhone.Text = GlobalValue.UniSerialPortOptData.CellPhone;
                        if (GlobalValue.UniSerialPortOptData.IsOptComType)
                            cbComType.SelectedIndex = GlobalValue.UniSerialPortOptData.ComType;
                        if (GlobalValue.UniSerialPortOptData.IsOpt_NetWorkType)
                            cbNetworkType.SelectedIndex = GlobalValue.UniSerialPortOptData.NetWorkType;
                        if (GlobalValue.UniSerialPortOptData.IsOptIP)
                        {
                            txtIP.Text = GlobalValue.UniSerialPortOptData.IP[0].ToString() + "." + GlobalValue.UniSerialPortOptData.IP[1].ToString() + "." +
                                GlobalValue.UniSerialPortOptData.IP[2].ToString() + "." + GlobalValue.UniSerialPortOptData.IP[3].ToString();
                        }
                        if (GlobalValue.UniSerialPortOptData.IsOptPort)
                            txtPort.Text = GlobalValue.UniSerialPortOptData.Port.ToString();

                        if (GlobalValue.UniSerialPortOptData.IsOpt_HeartInterval)
                            txtHeart.Text = GlobalValue.UniSerialPortOptData.HeartInterval.ToString();
                        if (GlobalValue.UniSerialPortOptData.IsOpt_Baud485)
                            cb485BaudRate.SelectedIndex = GlobalValue.UniSerialPortOptData.Baud485;
                        if (GlobalValue.UniSerialPortOptData.IsOptModbusExeFlag)
                            cbModbusExeFlag.SelectedIndex = GlobalValue.UniSerialPortOptData.ModbusExeFlag ? 1 : 0;
                        if (GlobalValue.UniSerialPortOptData.IsOpt_VolInterval)
                            txtVolInterval.Text = GlobalValue.UniSerialPortOptData.VolInterval.ToString();
                        if (GlobalValue.UniSerialPortOptData.IsOpt_VolLower)
                            txtVolLower.Text = GlobalValue.UniSerialPortOptData.VolLower.ToString();
                        if (GlobalValue.UniSerialPortOptData.IsOpt_SMSInterval)
                            txtSMSInterval.Text = GlobalValue.UniSerialPortOptData.SMSInterval.ToString();
                        if (GlobalValue.UniSerialPortOptData.IsOpt_PluseUnit)
                            cbPluseUnit.SelectedIndex = GlobalValue.UniSerialPortOptData.PluseUnit;
                        if (GlobalValue.UniSerialPortOptData.IsOpt_AlarmLen)
                            txtAlarmLen.Text = GlobalValue.UniSerialPortOptData.AlarmLen.ToString();

                        if (GlobalValue.UniSerialPortOptData.IsOpt_UpLimit)
                            txtPreUpLimit.Text = GlobalValue.UniSerialPortOptData.UpLimit.ToString("f3");
                        if (GlobalValue.UniSerialPortOptData.IsOpt_UpLimitEnable)
                            cbPreUpLimitEnable.SelectedIndex = GlobalValue.UniSerialPortOptData.UpLimitEnable ? 1 : 0;
                        if (GlobalValue.UniSerialPortOptData.IsOpt_LowLimit)
                            txtPreLowLimit.Text = GlobalValue.UniSerialPortOptData.LowLimit.ToString("f3");
                        if (GlobalValue.UniSerialPortOptData.IsOpt_LowLimitEnable)
                            cbPreLowLimitEnable.SelectedIndex = GlobalValue.UniSerialPortOptData.LowLimitEnable ? 1 : 0;
                        if (GlobalValue.UniSerialPortOptData.IsOpt_SlopUpLimit)
                            txtSlopUpLimit.Text = GlobalValue.UniSerialPortOptData.SlopUpLimit.ToString("f3");
                        if (GlobalValue.UniSerialPortOptData.IsOpt_SlopUpLimitEnable)
                            cbSlopUpLimitEnable.SelectedIndex = GlobalValue.UniSerialPortOptData.SlopUpLimitEnable ? 1 : 0;
                        if (GlobalValue.UniSerialPortOptData.IsOpt_SlopLowLimit)
                            txtSlopLowLimit.Text = GlobalValue.UniSerialPortOptData.SlopLowLimit.ToString("f3");
                        if (GlobalValue.UniSerialPortOptData.IsOpt_SlopLowLimitEnable)
                            cbSlopLowLimitEnable.SelectedIndex = GlobalValue.UniSerialPortOptData.SlopLowLimitEnable ? 1 : 0;
                        if (GlobalValue.UniSerialPortOptData.IsOpt_Range)
                            txtPreRange.Text = GlobalValue.UniSerialPortOptData.Range.ToString("f3");
                        if (GlobalValue.UniSerialPortOptData.IsOpt_PreOffset)
                            txtOffset.Text = GlobalValue.UniSerialPortOptData.PreOffset.ToString("f3");

                        if (GlobalValue.UniSerialPortOptData.IsOpt_CollectConfig)
                        {
                            ceDigitPreStatus.Checked = GlobalValue.UniSerialPortOptData.Collect_DigitPre;
                            cePluseState.Checked = GlobalValue.UniSerialPortOptData.Collect_Pluse;
                            ceSimulate1State.Checked = GlobalValue.UniSerialPortOptData.Collect_Simulate1;
                            ceSimulate2State.Checked = GlobalValue.UniSerialPortOptData.Collect_Simulate2;
                            ceRS485State.Checked = GlobalValue.UniSerialPortOptData.Collect_RS485;
                        }

                        if (GlobalValue.UniSerialPortOptData.IsOpt_SimualteInterval)
                        {
                            gridControl_Simulate.DataSource = GlobalValue.UniSerialPortOptData.Simulate_Interval;
                        }
                        if (GlobalValue.UniSerialPortOptData.IsOpt_PluseInterval)
                        {
                            gridControl_Pluse.DataSource = GlobalValue.UniSerialPortOptData.Pluse_Interval;
                        }
                        if (GlobalValue.UniSerialPortOptData.IsOpt_RS485Interval)
                        {
                            gridControl_RS485.DataSource = GlobalValue.UniSerialPortOptData.RS485_Interval;
                        }
                        if (GlobalValue.UniSerialPortOptData.IsOpt_RS485Protocol)
                        {
                            gridControl_485protocol.DataSource = GlobalValue.UniSerialPortOptData.RS485Protocol;
                        }

                        XtraMessageBox.Show("读取设备参数成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show("读取设备参数失败!" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.UniversalPluseBasic)
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

                    XtraMessageBox.Show("设置脉冲基准成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    XtraMessageBox.Show("设置脉冲基准失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.UniversalCallEnable)
            {
                this.Enabled = true;
                HideWaitForm();
                EnableControls(true);
                EnableRibbonBar();
                EnableNavigateBar();
                HideWaitForm();
                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);
                if (e.TransactStatus == TransStatus.Success)
                {
                    XtraMessageBox.Show("设置成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show("设置失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.UniversalCallData)
            {
                this.Enabled = true;
                HideWaitForm();
                EnableControls(true);
                EnableRibbonBar();
                EnableNavigateBar();
                HideWaitForm();
                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);
                if (e.TransactStatus == TransStatus.Success)
                {
                    XtraMessageBox.Show("招测成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show("招测失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (e.TransactStatus != TransStatus.Start && (e.OptType == SerialPortType.UniversalEnableAlarm))
            {
                this.Enabled = true;
                HideWaitForm();
                EnableControls(true);
                EnableRibbonBar();
                EnableNavigateBar();
                HideWaitForm();
                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);
                if (e.TransactStatus == TransStatus.Success)
                {
                    XtraMessageBox.Show("设置成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show("设置失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void EnableControls(bool enable)
        {
            btnSetPluseBasic.Enabled = enable;
            dropDownButtonReset.Enabled = enable;
            btnCheckingTime.Enabled = enable;
            btnEnableAlarm.Enabled = enable;
            btnEnableCollect.Enabled = enable;
            btnReadParm.Enabled = enable;
            btnSetParm.Enabled = enable;
            dropbtnCalibrationSimualte.Enabled = enable;
            SwitchComunication.Enabled = enable;
            btnFieldStrength.Enabled = enable;
            btnVer.Enabled = enable;
        }

        private new bool Validate()
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
            if (ceComType.Checked && string.IsNullOrEmpty(cbComType.Text))
            {
                XtraMessageBox.Show("请选择通信方式!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbComType.Focus();
                return false;
            }
            if (ceIP.Checked)
            {
                if (string.IsNullOrEmpty(txtIP.Text))
                {
                    XtraMessageBox.Show("请填写完整的IP地址!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtIP.Focus();
                    return false;
                }
            }

            if (cePort.Checked && !Regex.IsMatch(txtPort.Text, @"^\d{3,5}$"))
            {
                XtraMessageBox.Show("请输入端口号!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPort.Focus();
                return false;
            }
            if(ceHeart.Checked && !Regex.IsMatch(txtHeart.Text,@"^\d{1,5}$"))
            {
                XtraMessageBox.Show("请输入心跳间隔!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtHeart.Focus();
                return false;
            }
            if (ce485Baud.Checked && cb485BaudRate.SelectedIndex <0)
            {
                XtraMessageBox.Show("请选择波特率!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cb485BaudRate.Focus();
                return false;
            }
            if (ceModbusExeFlag.Checked && cbModbusExeFlag.SelectedIndex < 0)
            {
                XtraMessageBox.Show("请选择modbus执行标识!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbModbusExeFlag.Focus();
                return false;
            }
            if(ceVolInterval.Checked && !Regex.IsMatch(txtVolInterval.Text, @"^\d{1,5}$"))
            {
                XtraMessageBox.Show("请输入电压时间间隔!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtVolInterval.Focus();
                return false;
            }
            if (ceVolLower.Checked && !Regex.IsMatch(txtVolLower.Text, @"^\d{1,2}$"))
            {
                XtraMessageBox.Show("请输入电压报警下限!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtVolLower.Focus();
                return false;
            }
            if (ceSMSInterval.Checked && !Regex.IsMatch(txtSMSInterval.Text, @"^\d{1,4}$"))
            {
                XtraMessageBox.Show("请输入短信发送间隔!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSMSInterval.Focus();
                return false;
            }
            if(cePluseUnit.Checked &&　cbPluseUnit.SelectedIndex<0)
            {
                XtraMessageBox.Show("请选择脉冲计数单位!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbPluseUnit.Focus();
                return false;
            }
            if (ceAlarmLen.Checked && !Regex.IsMatch(txtAlarmLen.Text, @"^\d{1,4}$"))
            {
                XtraMessageBox.Show("请输入取消报警时间长度!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAlarmLen.Focus();
                return false;
            }

            if (cePreRange.Checked && !Regex.IsMatch(txtPreRange.Text, @"^[0-9]{1,5}(\.[0-9]{1,3})?$"))
            {
                XtraMessageBox.Show("请输入量程!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPreRange.Focus();
                return false;
            }
            if (ceOffset.Checked && !Regex.IsMatch(txtOffset.Text, @"^[0-9]{1,3}(\.[0-9]{1,3})?$"))
            {
                XtraMessageBox.Show("请输入偏移量!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtOffset.Focus();
                return false;
            }
            if (cePreUpLimit.Checked && !Regex.IsMatch(txtPreUpLimit.Text, @"^[0-9]{1,5}(\.[0-9]{1,3})?$"))
            {
                XtraMessageBox.Show("请输入上限值!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPreUpLimit.Focus();
                return false;
            }
            if (cePreUpLimitEnable.Checked && cbPreUpLimitEnable.SelectedIndex < 0)
            {
                XtraMessageBox.Show("请选择报警上限投退状态!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbPreUpLimitEnable.Focus();
                return false;
            }
            if (cePreLowLimit.Checked && !Regex.IsMatch(txtPreLowLimit.Text, @"^[0-9]{1,5}(\.[0-9]{1,3})?$"))
            {
                XtraMessageBox.Show("请输入下限值!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPreLowLimit.Focus();
                return false;
            }
            if (cePreLowLimitEnable.Checked && cbPreLowLimitEnable.SelectedIndex < 0)
            {
                XtraMessageBox.Show("请选择报警下限投退状态!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbPreLowLimitEnable.Focus();
                return false;
            }
            if (ceSlopUpLimit.Checked && !Regex.IsMatch(txtSlopUpLimit.Text, @"^[0-9]{1,5}(\.[0-9]{1,3})?$"))
            {
                XtraMessageBox.Show("请输入斜率上限值!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSlopUpLimit.Focus();
                return false;
            }
            if (ceSlopUpLimitEnable.Checked && cbSlopUpLimitEnable.SelectedIndex < 0)
            {
                XtraMessageBox.Show("请选择斜率报警上限投退状态!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbSlopUpLimitEnable.Focus();
                return false;
            }
            if (ceSlopLowLimit.Checked && !Regex.IsMatch(txtSlopLowLimit.Text, @"^[0-9]{1,5}(\.[0-9]{1,3})?$"))
            {
                XtraMessageBox.Show("请输入斜率下限值!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSlopLowLimit.Focus();
                return false;
            }
            if (ceSlopLowLimitEnable.Checked && cbSlopLowLimitEnable.SelectedIndex < 0)
            {
                XtraMessageBox.Show("请选择斜率报警下限投退状态!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbSlopLowLimitEnable.Focus();
                return false;
            }
            

            if (ceCollectModbus.Checked)
            {
                DataTable dt = gridControl_485protocol.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0)
                {
                    XtraMessageBox.Show("请填写RS485采集modbus协议配置表!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    gridView_485protocol.Focus();
                    return false;
                }
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

            if (ceCollectPluse.Checked)
            {
                DataTable dt = gridControl_Pluse.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0)
                {
                    XtraMessageBox.Show("请填写脉冲量时间间隔表!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    gridView_Pluse.Focus();
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

        private void btnVer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                XtraMessageBox.Show("请先填写终端ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return;
            }
            GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity(this.DevType);
            GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

            EnableControls(false);
            DisableRibbonBar();
            DisableNavigateBar();
            ShowWaitForm("", "正在读取版本号...");
            BeginSerialPortDelegate();
            Application.DoEvents();
            SetStaticItem("正在读取版本号...");
            Send(SerialPortType.UniversalReadVer);
        }

        private void btnFieldStrength_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                XtraMessageBox.Show("请先填写终端ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return;
            }
            GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity(this.DevType);
            GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

            EnableControls(false);
            DisableRibbonBar();
            DisableNavigateBar();
            ShowWaitForm("", "正在读取场强\\电压...");
            BeginSerialPortDelegate();
            Application.DoEvents();
            SetStaticItem("正在读取场强\\电压...");
            Send(SerialPortType.UniversalReadFiledStrength);
        }
        private void btnDel_Click(object sender, EventArgs e)
        {
            int[] selectrows = gridView_WaitCmd.GetSelectedRows();
            if (selectrows != null && selectrows.Length > 0)
            {
                string DevType = gridView_WaitCmd.GetRowCellValue(selectrows[0], "DevType").ToString();
                string TerId = gridView_WaitCmd.GetRowCellValue(selectrows[0], "ID").ToString();
                string Funcode = gridView_WaitCmd.GetRowCellValue(selectrows[0], "Funcode").ToString();

                SocketEntity msmqentity = new SocketEntity();
                msmqentity.DevType = (ConstValue.DEV_TYPE)Convert.ToInt32(DevType);
                msmqentity.DevId = Convert.ToInt16(TerId);
                msmqentity.P68Funcode = Convert.ToByte(Funcode.Replace("0x", ""), 16);
                msmqentity.MsgType = ConstValue.MSMQTYPE.Del_P68_WaitSendCmd;
                GlobalValue.SocketMgr.SendMessage(msmqentity);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GlobalValue.SocketMgr.SendMessage(new SocketEntity(ConstValue.MSMQTYPE.Get_P68_WaitSendCmd, ""));
        }
        #endregion

        public override void SerialPortEvent(bool Enabled)
        {
            if (SwitchComunication.IsOn)  //串口
            {
                SetSerialPortCtrlStatus();
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
            btnSetPluseBasic.Enabled = GlobalValue.portUtil.IsOpen;
            dropDownButtonReset.Enabled = GlobalValue.portUtil.IsOpen;
            btnCheckingTime.Enabled = GlobalValue.portUtil.IsOpen;
            btnEnableCollect.Enabled = GlobalValue.portUtil.IsOpen;
            btnReadParm.Enabled = GlobalValue.portUtil.IsOpen;
            btnSetParm.Enabled = GlobalValue.portUtil.IsOpen;
            dropbtnCalibrationSimualte.Enabled = GlobalValue.portUtil.IsOpen;
            btnFieldStrength.Enabled = GlobalValue.portUtil.IsOpen;
            btnVer.Enabled = GlobalValue.portUtil.IsOpen;
            btnEnableAlarm.Enabled = GlobalValue.portUtil.IsOpen;

            timer_GetWaitCmd.Enabled = false;  //停用查询GPRS待发送命令定时器 10s
            GlobalValue.SocketMgr.SockMsgEvent -= new SocketHandler(MSMQMgr_MSMQEvent);

            ceID.Enabled = true;
            ceID.Checked = false;
            ceComType.Enabled = true;
            ceComType.Checked = false;
            ceIP.Enabled = GlobalValue.portUtil.IsOpen;
            ceIP.Checked = false;
            txtIP.Enabled = GlobalValue.portUtil.IsOpen;
            txtIP.Text = "";
            cePort.Enabled = GlobalValue.portUtil.IsOpen;
            cePort.Checked = false;
            txtPort.Enabled = GlobalValue.portUtil.IsOpen;
            txtPort.Text = "";
            gridControl_WaitCmd.Enabled = false;
            btnDel.Enabled = false;
            btnRefresh.Enabled = false;

            btnCallOpen.Enabled = GlobalValue.portUtil.IsOpen;
            btnCallClose.Enabled = GlobalValue.portUtil.IsOpen;
            btnCallData.Enabled = GlobalValue.portUtil.IsOpen;

            SetDevTypeEnable();
        }

        private void SetGprsCtrlStatus()
        {
            btnSetPluseBasic.Enabled = true;
            dropDownButtonReset.Enabled = true;
            btnCheckingTime.Enabled = true;
            btnEnableCollect.Enabled = true;
            btnReadParm.Enabled = true;
            btnSetParm.Enabled = true;
            dropbtnCalibrationSimualte.Enabled = true;
            btnFieldStrength.Enabled = false;
            btnVer.Enabled = true;
            btnEnableAlarm.Enabled = true;

            timer_GetWaitCmd.Enabled = true;  //启用查询GPRS待发送命令定时器 10s
            GlobalValue.SocketMgr.SendMessage(new SocketEntity(ConstValue.MSMQTYPE.Get_P68_WaitSendCmd, ""));
            GlobalValue.SocketMgr.SockMsgEvent -= new SocketHandler(MSMQMgr_MSMQEvent);
            GlobalValue.SocketMgr.SockMsgEvent += new SocketHandler(MSMQMgr_MSMQEvent);

            ceID.Enabled = false;
            ceID.Checked = false;
            ceComType.Enabled = false;
            ceComType.Checked = false;
            ceIP.Enabled = false;
            ceIP.Checked = false;
            txtIP.Enabled = false;
            txtIP.Text = "";
            cePort.Enabled = false;
            cePort.Checked = false;
            txtPort.Enabled = false;
            txtPort.Text = "";
            gridControl_WaitCmd.Enabled = true;
            btnDel.Enabled = true;
            btnRefresh.Enabled = true;

            btnCallOpen.Enabled = true;
            btnCallClose.Enabled = true;
            btnCallData.Enabled = true;

            SetDevTypeEnable();
        }

        private void SetDevTypeEnable()
        {
            if (this.DevType == ConstValue.DEV_TYPE.Data_CTRL)
            {
                ceModbusExeFlag.Enabled = false;
                ceModbusExeFlag.Checked = false;
                ceSMSInterval.Enabled = false;
                ceSMSInterval.Checked = false;
                cePluseUnit.Enabled = false;
                cePluseUnit.Checked = false;
                ceCollectSimulate.Enabled = false;
                ceCollectSimulate.Checked = false;
                ceCollectModbus.Enabled = false;
                ceCollectModbus.Checked = false;
                btnFieldStrength.Enabled = false;
            }
            else if (this.DevType == ConstValue.DEV_TYPE.UNIVERSAL_CTRL)
            {
                ceModbusExeFlag.Enabled = true;
                ceSMSInterval.Enabled = true;
                cePluseUnit.Enabled = true;
                ceCollectSimulate.Enabled = true;
                ceCollectModbus.Enabled = true;
            }
        }

        private void cePort_CheckedChanged(object sender, EventArgs e)
        {
            if (!cePort.Checked)
            {
                txtPort.Text = "";
            }
        }
        
        private void cbPreFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            压力1-01，压力2-02，模拟量1-03，模拟量2-04，流量-05
            */
            if(cbPreFlag.SelectedIndex == 0)   //量程只有压力和模拟量有；偏移量只有压力有
            {
                cePreRange.Enabled = true;
                txtPreRange.Enabled = true;
                ceOffset.Enabled = true;
                txtOffset.Enabled = true;
            }
            else if (cbPreFlag.SelectedIndex == 1)
            {
                cePreRange.Enabled = true;
                txtPreRange.Enabled = true;
                ceOffset.Enabled = true;
                txtOffset.Enabled = true;
            }
            else if (cbPreFlag.SelectedIndex == 2)
            {
                cePreRange.Enabled = true;
                txtPreRange.Enabled = true;
                ceOffset.Enabled = false;
                ceOffset.Checked = false;
                txtOffset.Enabled = false;
            }
            else if (cbPreFlag.SelectedIndex == 3)
            {
                cePreRange.Enabled = true;
                txtPreRange.Enabled = true;
                ceOffset.Enabled = false;
                ceOffset.Checked = false;
                txtOffset.Enabled = false;
            }
            else if (cbPreFlag.SelectedIndex == 4)
            {
                cePreRange.Enabled = false;
                cePreRange.Checked = false;
                txtPreRange.Enabled = false;
                ceOffset.Enabled = false;
                ceOffset.Checked = false;
                txtOffset.Enabled = false;
            }
        }

        private void btnCallData_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                XtraMessageBox.Show("请先填写终端ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return;
            }

            bool haveset = false;
            object objselected = treeSocketType.GetSelectedDataRow();
            if (treeSocketType.Properties.TreeList.Nodes != null)
            {
                GlobalValue.SerialPortCallDataType = new CallDataTypeEntity();
                foreach (TreeListNode node in treeSocketType.Properties.TreeList.GetAllCheckedNodes())
                {
                    if (!node.HasChildren)
                    {
                        DataRowView drdata = treeSocketType.Properties.TreeList.GetDataRecordByNode(node) as DataRowView;
                        if (drdata != null)
                        {
                            string name = drdata.Row["Name"].ToString();
                            switch (name)
                            {
                                case "压力":
                                    GlobalValue.SerialPortCallDataType.GetPre = true;
                                    haveset = true;
                                    break;
                                case "脉冲量":
                                    GlobalValue.SerialPortCallDataType.GetPluse = true;
                                    haveset = true;
                                    break;
                                case "第1路模拟量":
                                    GlobalValue.SerialPortCallDataType.GetSim1 = true;
                                    haveset = true;
                                    break;
                                case "第2路模拟量":
                                    GlobalValue.SerialPortCallDataType.GetSim2 = true;
                                    haveset = true;
                                    break;
                                case "第1路485":
                                    GlobalValue.SerialPortCallDataType.GetRS4851 = true;
                                    haveset = true;
                                    break;
                                case "第2路485":
                                    GlobalValue.SerialPortCallDataType.GetRS4852 = true;
                                    haveset = true;
                                    break;
                                case "第3路485":
                                    GlobalValue.SerialPortCallDataType.GetRS4853 = true;
                                    haveset = true;
                                    break;
                                case "第4路485":
                                    GlobalValue.SerialPortCallDataType.GetRS4854 = true;
                                    haveset = true;
                                    break;
                            }

                        }
                    }
                }
            }
            if (!haveset)
            {
                XtraMessageBox.Show("请先选择招测数据类型!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                treeSocketType.ShowPopup();
                return;
            }
            GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity(this.DevType);
            GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);

            EnableControls(false);
            DisableRibbonBar();
            DisableNavigateBar();
            ShowWaitForm("", "正在招测数据...");
            BeginSerialPortDelegate();
            Application.DoEvents();
            SetStaticItem("正在招测数据...");
            Send(SerialPortType.UniversalCallData);
        }

        public void Send(SerialPortType type)
        {
            GlobalValue.Universallog.RWType = SwitchComunication.IsOn ? RWFunType.SerialPort : RWFunType.GPRS;
            GlobalValue.Universallog.ClearCmdPack();
            GlobalValue.SerialPortMgr.Send(type);
        }

        private void btnCallOpen_Click(object sender, EventArgs e)
        {
            CallEnable(true);
        }

        private void btnCallClose_Click(object sender, EventArgs e)
        {
            CallEnable(false);
        }

        private void CallEnable(bool Enable)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                XtraMessageBox.Show("请先填写终端ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return;
            }
            GlobalValue.UniSerialPortOptData = new UniversalSerialPortOptEntity(this.DevType);
            GlobalValue.UniSerialPortOptData.ID = Convert.ToInt16(txtID.Text);
            GlobalValue.UniSerialPortOptData.CallEnable = Enable;

            EnableControls(false);
            DisableRibbonBar();
            DisableNavigateBar();
            ShowWaitForm("", "正在设置招测开关...");
            BeginSerialPortDelegate();
            Application.DoEvents();
            SetStaticItem("正在设置招测开关...");
            Send(SerialPortType.UniversalCallEnable);
        }

        void MSMQMgr_MSMQEvent(object sender, SocketEventArgs e)
        {
            if (e.msmqEntity != null && e.msmqEntity.MsgType == ConstValue.MSMQTYPE.Get_P68_WaitSendCmd)
            {
                if (!string.IsNullOrEmpty(e.msmqEntity.Msg))
                {
                    try
                    {
                        List<Package> lstPack = JSONSerialize.JsonDeserialize<List<Package>>(e.msmqEntity.Msg);
                        DataTable dt = dt = new DataTable("waitsendTable");
                        dt.Columns.Add("StrDevType");
                        dt.Columns.Add("ID");
                        dt.Columns.Add("Funcode");
                        dt.Columns.Add("Data");
                        dt.Columns.Add("DevType");

                        if (lstPack != null)
                        {
                            foreach (Package pack in lstPack)   //如果在lstPack中存在,而在绑定的表中不存在,则添加
                            {
                                DataRow dr = dt.NewRow();
                                dr["StrDevType"] = new EnumHelper().GetEnumDescription(pack.DevType);
                                dr["ID"] = pack.DevID;
                                dr["Funcode"] = "0x" + string.Format("{0:X2}", pack.C1);
                                dr["Data"] = ConvertHelper.ByteToString(pack.Data, pack.DataLength);
                                dr["DevType"] = (int)pack.DevType;
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            dt.Rows.Clear();
                        }
                        SetWaitListView_Cmd(dt);
                    }
                    catch
                    {
                        ;
                    }
                }
            }
        }

        private delegate void SetWaitListViewHandle(DataTable lstPack);
        private void SetWaitListView_Cmd(DataTable lstPack)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((SetWaitListViewHandle)delegate (DataTable lstparm)
                {
                    gridControl_WaitCmd.DataSource = lstparm;
                }, lstPack);
            }
            else
            {
                gridControl_WaitCmd.DataSource = lstPack;
            }
        }

    }
}
