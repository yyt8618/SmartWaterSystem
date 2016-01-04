using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Common;
using Entity;

namespace SmartWaterSystem
{
    public partial class NoiseGroupMgr : BaseView, INoiseGroupMgr
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("NoiseGroupMgr");
        List<NoiseRecorder> OptRecList = null;
        int currentOptRecIndex = -1;

        public NoiseGroupMgr()
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            SerialPortEvent(GlobalValue.portUtil.IsOpen);

            this.timer1.Interval = 1000;   //1s
            this.timer1.Tick += new EventHandler(timer1_Tick);
            this.timer1.Enabled = true;
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                DateTime datenow = this.dateTimePicker.Value;
                if (datenow.CompareTo(DateTime.Now.AddDays(-1)) > -1)
                    this.dateTimePicker.Value = datenow.AddSeconds(1);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 绑定树形列表
        /// </summary>
        public void BindTree()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("KeyFieldName");
                dt.Columns.Add("ParentFieldName");
                dt.Columns.Add("ID");
                dt.Columns.Add("Remark");
                dt.Columns.Add("Name");

                int pFlag = 0;
                int cFlag = 0;
                int tFlag = 0;
                for (int i = 0; i < GlobalValue.groupList.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["KeyFieldName"] = tFlag;
                    dr["ParentFieldName"] = DBNull.Value;
                    dr["ID"] = GlobalValue.groupList[i].Name;  //注意这里与Name字段是相反的,主要是显示的时候用名称,子节点用终端号
                    dr["Remark"] = GlobalValue.groupList[i].Remark;
                    dr["Name"] = GlobalValue.groupList[i].ID;
                    cFlag = tFlag + 1;
                    pFlag = tFlag;
                    dt.Rows.Add(dr);
                    for (int j = 0; j < GlobalValue.groupList[i].RecorderList.Count; j++)
                    {
                        DataRow dr1 = dt.NewRow();
                        dr1["KeyFieldName"] = cFlag;
                        dr1["ParentFieldName"] = pFlag;
                        dr1["ID"] = GlobalValue.groupList[i].RecorderList[j].ID;
                        dr1["Remark"] = GlobalValue.groupList[i].RecorderList[j].Remark;
                        dt.Rows.Add(dr1);
                        cFlag++;
                        tFlag++;
                    }
                    pFlag++;
                    tFlag++;
                }

                treeList1.DataSource = dt;
                treeList1.ParentFieldName = "ParentFieldName";
                treeList1.KeyFieldName = "KeyFieldName";

                treeList1.ExpandAll();
            }
            catch (Exception ex)
            {
                logger.ErrorException("BindTree", ex);
                XtraMessageBox.Show("初始化页面出现异常！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 绑定可用记录仪列表
        /// </summary>
        public void BindListBox()
        {
            listBoxRec.Items.Clear();
            for (int i = 0; i < GlobalValue.recorderList.Count; i++)
            {
                if (GlobalValue.recorderList[i].GroupState == 0)
                    listBoxRec.Items.Add(GlobalValue.recorderList[i].ID.ToString());
            }
            listBoxRec.UnSelectAll();
        }

        private void treeList1_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            if (e.Node == null)
                return;

            if (e.Node.Level == 0)
            {
                btnDelRecFromGroup.Enabled = false;

                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode item in e.Node.Nodes)
                {
                    item.CheckState = e.Node.CheckState;
                }

                if (treeList1.GetAllCheckedNodes().Count > 0)
                {
                    btnDeleteGroup.Enabled = true;
                    btnDelRecFromGroup.Enabled = true;
                }
                else
                {
                    btnDeleteGroup.Enabled = false;
                    btnDelRecFromGroup.Enabled = false;
                }
            }
            else if (e.Node.Level == 1)
            {
                if (treeList1.GetAllCheckedNodes().Count > 0)
                {
                    btnDelRecFromGroup.Enabled = true;
                }
                else
                {
                    btnDelRecFromGroup.Enabled = false;
                }
            }
        }

        private void simpleButtonSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DevExpress.XtraTreeList.Nodes.TreeListNode item in treeList1.Nodes)
            {
                item.CheckState = CheckState.Checked;

                DevExpress.XtraTreeList.NodeEventArgs arg = new DevExpress.XtraTreeList.NodeEventArgs(item);
                treeList1_AfterCheckNode(treeList1, arg);
            }
        }

        private void simpleButtonUnSelect_Click(object sender, EventArgs e)
        {
            foreach (DevExpress.XtraTreeList.Nodes.TreeListNode item in treeList1.Nodes)
            {
                if (item.Checked)
                    item.CheckState = CheckState.Unchecked;
                else
                    item.CheckState = CheckState.Checked;

                DevExpress.XtraTreeList.NodeEventArgs arg = new DevExpress.XtraTreeList.NodeEventArgs(item);
                treeList1_AfterCheckNode(treeList1, arg);
            }
        }

        private void treeList1_AfterFocusNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                DataRowView drv = treeList1.GetDataRecordByNode(e.Node) as DataRowView;
                txtGroupID.Text = drv["Name"].ToString();
                txtGroupName.Text = drv["ID"].ToString();
                txtGroupNote.Text = drv["Remark"].ToString();

                btnAlterGroupSet.Enabled = true;

                if (listBoxRec.SelectedItems!=null && listBoxRec.SelectedItems.Count>0)
                    btnImportRec.Enabled = true;
                else
                    btnImportRec.Enabled = false;

                if (e.Node.Nodes.Count > 0)
                    groupControl3.Enabled = true;
                else
                    groupControl3.Enabled = false;
            }
            else
            {
                groupControl3.Enabled = false;
                btnAlterGroupSet.Enabled = false;
                btnImportRec.Enabled = false;
            }
        }

        private void btnAlterGroupSet_Click(object sender, EventArgs e)
        {
            try
            {
                NoiseRecorderGroup alterGrp = new NoiseRecorderGroup();
                alterGrp.ID = Convert.ToInt32(txtGroupID.Text);
                alterGrp.Name = txtGroupName.Text;
                alterGrp.Remark = txtGroupNote.Text;
                int query = NoiseDataBaseHelper.UpdateGroup(alterGrp);

                if (query != -1)
                {
                    GlobalValue.groupList = NoiseDataBaseHelper.GetGroups();
                    XtraMessageBox.Show("更新成功！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //LoadGroupList();
                    BindTree();
                }
                else
                    throw new Exception("数据入库发生错误。");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("更新失败：" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtGroupName.Text) || string.IsNullOrEmpty(txtGroupNote.Text))
                {
                    XtraMessageBox.Show("分组名称/分组备注未输入！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                NoiseRecorderGroup newGrp = new NoiseRecorderGroup();
                newGrp.ID = NoiseDataBaseHelper.GetGroupID();
                newGrp.Name = txtGroupName.Text;
                newGrp.Remark = txtGroupNote.Text;
                int query = NoiseDataBaseHelper.AddGroup(newGrp);

                if (query != -1)
                {
                    GlobalValue.groupList = NoiseDataBaseHelper.GetGroups();
                    BindTree();
                }
                else
                    throw new Exception("数据入库发生错误。");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("添加失败：" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteGroup_Click(object sender, EventArgs e)
        {
            List<DevExpress.XtraTreeList.Nodes.TreeListNode> nodes = treeList1.GetAllCheckedNodes();
            if (nodes.Count == 0)
                return;

            DialogResult dr = XtraMessageBox.Show("确定删除所选分组？", GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i].Level == 0)
                    {
                        DataRowView drv = treeList1.GetDataRecordByNode(nodes[i]) as DataRowView;
                        int query = NoiseDataBaseHelper.DeleteGroup(Convert.ToInt32(drv["ID"]));
                        if (query == -1)
                            throw new Exception("数据入库发生错误。");
                    }
                }
                GlobalValue.ClearText(groupControl2);
                GlobalValue.ClearText(groupControl3);
                GlobalValue.groupList = NoiseDataBaseHelper.GetGroups();
                GlobalValue.recorderList = NoiseDataBaseHelper.GetRecorders();
                BindTree();
                GlobalValue.reReadIdList.Clear();
            }
        }

        private void txtRecTime_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraEditors.TextEdit obj = sender as DevExpress.XtraEditors.TextEdit;
                int t = 0;
                int t1 = 0;
                switch (obj.Name)
                {
                    case "txtRecTime":
                        t = Convert.ToInt32(obj.Text);
                        t1 = t + GlobalValue.Time;
                        if (t1 > 24)
                            txtRecTime1.Text = (t1 - 24).ToString();
                        else
                            txtRecTime1.Text = (t + GlobalValue.Time).ToString();
                        break;
                }
            }
            catch (Exception)
            {

            }
        }

        private void nUpDownSamSpan_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown obj = sender as NumericUpDown;
            switch (obj.Name)
            {
                case "nUpDownSamSpan":
                    txtRecNum.Text = (GlobalValue.Time * 60 / obj.Value).ToString();
                    break;
                default:
                    break;
            }
        }

        private void btnNow_Click(object sender, EventArgs e)
        {
            dateTimePicker.Value = DateTime.Now;
        }

        // 分配记录仪
        private void btnImportRec_Click(object sender, EventArgs e)
        {
            if (treeList1.Selection != null)
            {
                if (treeList1.Selection.Count == 0)
                    return;
                if (treeList1.Selection[0].Level == 1)
                    return;

                if (listBoxRec.Items.Count == 0)
                {
                    XtraMessageBox.Show("当前无记录仪可分配！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (listBoxRec.SelectedItems.Count == 0)
                {
                    XtraMessageBox.Show("请选择需要分配的记录仪！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                NoiseRecorderGroup gp = (from temp in GlobalValue.groupList
                                         where temp.ID == Convert.ToInt32(treeList1.Selection[0].GetValue("Name"))
                                         select temp).ToList()[0];
                if (gp == null)
                    return;

                for (int i = 0; i < listBoxRec.SelectedItems.Count; i++)
                {
                    NoiseRecorder tmp = (from item in GlobalValue.recorderList.AsEnumerable()
                                         where item.ID.ToString() == listBoxRec.SelectedItems[i].ToString()
                                         select item).ToList()[0];

                    tmp.GroupState = 1;
                    NoiseDataBaseHelper.AddRecorderGroupRelation(tmp.ID, gp.ID);
                    NoiseDataBaseHelper.UpdateRecorder(tmp);

                }

                gp.RecorderList = NoiseDataBaseHelper.GetRecordersByGroupId(gp.ID);
                GlobalValue.recorderList = NoiseDataBaseHelper.GetRecorders();
                BindTree();
                BindListBox();
            }
        }

        // 移除记录仪
        private void btnDelRecFromGroup_Click(object sender, EventArgs e)
        {
            try
            {
                //DialogResult dr = XtraMessageBox.Show("确定移除该记录仪？", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                //if (dr == System.Windows.Forms.DialogResult.Yes)
                //{

                List<DevExpress.XtraTreeList.Nodes.TreeListNode> nodes = treeList1.GetAllCheckedNodes();
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode item in nodes)
                {
                    if (item.Level == 1)
                    {
                        int recID = Convert.ToInt32(item.GetValue("ID"));
                        int gID = Convert.ToInt32(item.ParentNode.GetValue("Name"));

                        for (int i = 0; i < GlobalValue.recorderList.Count; i++)
                        {
                            if (GlobalValue.recorderList[i].ID == recID)
                            {
                                GlobalValue.recorderList[i].GroupState = 0;
                                NoiseDataBaseHelper.UpdateRecorder(GlobalValue.recorderList[i]);
                                break;
                            }
                        }

                        int query = NoiseDataBaseHelper.DeleteOneRelation(recID, gID);
                        if (query != -1)
                        {

                        }
                    }
                }

                btnDelRecFromGroup.Enabled = false;
                //NoiseRecorderGroup gp = (from temp in GlobalValue.groupList
                //                         where temp.ID == gID
                //                         select temp).ToList()[0];
                //gp.RecorderList = NoiseDataBaseHelper.GetRecordersByGroupId(gp.ID);
                GlobalValue.recorderList = NoiseDataBaseHelper.GetRecorders();
                GlobalValue.groupList = NoiseDataBaseHelper.GetGroups();
                BindTree();
                BindListBox();
                GlobalValue.reReadIdList.Clear();
                //}
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("移除失败：" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImportApply_Click(object sender, EventArgs e)
        {
            NoiseRecorderGroup gp = (from temp in GlobalValue.groupList
                                     where temp.ID == Convert.ToInt32(btnImportRec.Tag)
                                     select temp).ToList()[0];

            for (int i = 0; i < listBoxRec.SelectedItems.Count; i++)
            {
                NoiseRecorder tmp = (from item in GlobalValue.recorderList.AsEnumerable()
                                     where item.ID.ToString() == listBoxRec.SelectedItems[i].ToString()
                                     select item).ToList()[0];

                tmp.GroupState = 1;
                NoiseDataBaseHelper.AddRecorderGroupRelation(tmp.ID, gp.ID);
                NoiseDataBaseHelper.UpdateRecorder(tmp);

            }

            gp.RecorderList = NoiseDataBaseHelper.GetRecordersByGroupId(gp.ID);
            GlobalValue.recorderList = NoiseDataBaseHelper.GetRecorders();
            BindTree();
            listBoxRec.Items.Clear();
        }

        private void btnReadTemplate_Click(object sender, EventArgs e)
        {
            txtComTime.Text = Settings.Instance.GetString(SettingKeys.ComTime_Template);
            txtRecTime.Text = Settings.Instance.GetString(SettingKeys.RecTime_Template);
            nUpDownSamSpan.Value = Settings.Instance.GetInt(SettingKeys.Span_Template);
            txtRecNum.Text = (GlobalValue.Time * 60 / Settings.Instance.GetInt(SettingKeys.Span_Template)).ToString();
            txtLeakValue.Text = (new BLL.NoiseParmBLL()).GetParm(ConstValue.LeakValue_Template);

            int power = Settings.Instance.GetInt(SettingKeys.Power_Template);
            comboBoxEditPower.SelectedIndex = power;

            int conPower = Settings.Instance.GetInt(SettingKeys.ControlPower_Template);
            comboBoxDist.SelectedIndex = conPower;
        }

        private void btnSaveGroupSet_Click(object sender, EventArgs e)
        {
            //new Action(() =>
            //{
                Control cl = sender as Control;
                try
                {
                    string msg = string.Empty;
                    if (!ValidateRecorderManageInput(out msg))
                    {
                        throw new Exception(msg);
                    }

                    if (string.IsNullOrEmpty(txtGroupID.Text))
                    {
                        XtraMessageBox.Show("请选择需要操作的分组", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    NoiseRecorderGroup CurrentGroup = (from tmp in GlobalValue.groupList
                                                       where tmp.ID == Convert.ToInt32(this.txtGroupID.Text)
                                                       select tmp).ToList()[0];
                    if (CurrentGroup != null && CurrentGroup.RecorderList != null && CurrentGroup.RecorderList.Count > 0)
                    {
                        this.Enabled = false;
                        DisableRibbonBar();
                        DisableNavigateBar();
                        ShowWaitForm("", "正在进行批量设置...");
                        Application.DoEvents();
                        SetStaticItem("正在进行批量设置...");

                        //this.Enabled = false;
                        //cl.Enabled = false;
                        //short id = 0;
                        OptRecList = CurrentGroup.RecorderList;
                        
                        //foreach (NoiseRecorder alterRec in CurrentGroup.RecorderList)
                        //{
                            //NoiseRecorder alterRec = OptRecList[currentOptRecIndex];
                            //id = Convert.ToInt16(alterRec.ID);

                            GlobalValue.NoiseSerialPortOptData = new NoiseSerialPortOptEntity();
                            //GlobalValue.NoiseSerialPortOptData.ID = id;
                            // 设置记录仪时间
                            GlobalValue.NoiseSerialPortOptData.dt = this.dateTimePicker.Value;
                            // 设置远传通讯时间
                            GlobalValue.NoiseSerialPortOptData.ComTime = Convert.ToInt32(txtComTime.Text);
                            // 设置记录时间段
                            GlobalValue.NoiseSerialPortOptData.colstarttime = Convert.ToInt32(txtRecTime.Text);
                            GlobalValue.NoiseSerialPortOptData.colendtime = Convert.ToInt32(txtRecTime1.Text);
                            // 设置采集间隔
                            GlobalValue.NoiseSerialPortOptData.Interval = (int)nUpDownSamSpan.Value;
                            
                            // 设置远传功能
                            if (comboBoxDist.SelectedIndex == 1)
                            {
                                GlobalValue.NoiseSerialPortOptData.RemoteSwitch =true;
                            }
                            else
                            {
                                GlobalValue.NoiseSerialPortOptData.RemoteSwitch = false;
                            }

                            foreach (NoiseRecorder alterRec in OptRecList)
                            {
                                // 设置开关
                                if (comboBoxEditPower.SelectedIndex == 1)
                                {
                                    alterRec.Power = 1;
                                }
                                else if (comboBoxEditPower.SelectedIndex == 0)
                                {
                                    alterRec.Power = 0;
                                }

                                alterRec.CommunicationTime = GlobalValue.NoiseSerialPortOptData.ComTime;
                                alterRec.RecordTime = Convert.ToInt32(txtRecTime.Text);
                                alterRec.PickSpan = GlobalValue.NoiseSerialPortOptData.Interval;
                                if (comboBoxDist.SelectedIndex == 1)
                                {
                                    alterRec.ControlerPower = 1;
                                }
                                else
                                {
                                    alterRec.ControlerPower = 0;
                                }
                            }
                            GlobalValue.NoiseSerialPortOptData.ID = Convert.ToInt16(OptRecList[0].ID);
                            currentOptRecIndex = 0;
                            BeginSerialPortDelegate();
                            GlobalValue.SerialPortMgr.SerialPortScheduleEvent -= new SerialPortScheduleHandle(SerialPortParm_SerialPortScheduleEvent);
                            GlobalValue.SerialPortMgr.SerialPortScheduleEvent += new SerialPortScheduleHandle(SerialPortParm_SerialPortScheduleEvent);
                            GlobalValue.SerialPortMgr.Send(SerialPortType.NoiseBatchWrite);

                            //// 设置记录时间段
                            //GlobalValue.Noiselog.WriteStartEndTime(id, Convert.ToInt32(txtRecTime.Text), Convert.ToInt32(txtRecTime1.Text));
                            //alterRec.RecordTime = Convert.ToInt32(txtRecTime.Text);

                            //// 设置采集间隔
                            //GlobalValue.Noiselog.WriteInterval(id, (int)nUpDownSamSpan.Value);
                            //alterRec.PickSpan = Convert.ToInt32(nUpDownSamSpan.Value);

                            //// 设置远传通讯时间
                            //GlobalValue.Noiselog.WriteRemoteSendTime(id, Convert.ToInt32(txtComTime.Text));
                            //alterRec.CommunicationTime = Convert.ToInt32(txtComTime.Text);

                            //// 设置远传功能
                            //if (comboBoxDist.SelectedIndex == 1)
                            //{
                            //    GlobalValue.Noiselog.WriteRemoteSwitch(id, true);

                            //    alterRec.ControlerPower = 1;
                            //    //DistanceController alterCtrl = new DistanceController();
                            //    //alterCtrl.ID = Convert.ToInt32(txtConId.Text);
                            //    //alterCtrl.RecordID = alterRec.ID;
                            //    //alterCtrl.Port = Convert.ToInt32(txtConPort.Text);
                            //    //alterCtrl.IPAdress = txtConAdress.Text;

                            //    //NoiseDataBaseHelper.UpdateControler(alterCtrl);
                            //}
                            //else
                            //{
                            //    GlobalValue.Noiselog.WriteRemoteSwitch(id, false);
                            //    alterRec.ControlerPower = 0;
                            //}
                            //short[] origitydata = null;
                            //// 设置开关
                            //if (comboBoxEditPower.SelectedIndex == 1)
                            //{
                            //    GlobalValue.Noiselog.CtrlStartOrStop(id, true, out origitydata);
                            //    alterRec.Power = 1;
                            //}
                            //else if (comboBoxEditPower.SelectedIndex == 0)
                            //{
                            //    GlobalValue.Noiselog.CtrlStartOrStop(id, false, out origitydata);
                            //    alterRec.Power = 0;
                            //}

                            //// 设置记录仪时间
                            //GlobalValue.Noiselog.WriteTime(id, this.dateTimePicker.Value);

                            //// 更新设置入库
                            //int query = NoiseDataBaseHelper.UpdateRecorder(alterRec);
                            //if (query != -1)
                            //{
                            //    ShowDialog("设置成功！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //    GlobalValue.recorderList = NoiseDataBaseHelper.GetRecorders();
                            //    GlobalValue.groupList = NoiseDataBaseHelper.GetGroups();
                            //}
                            //else
                            //    throw new Exception("数据入库发生错误。");
                        //}
                    }
                    else
                    {
                        ShowDialog("请选择需要操作的分组", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    //short id = Convert.ToInt16(txtRecID.Text);
                    //NoiseRecorder alterRec = (from item in GlobalValue.recorderList
                    //                          where item.ID == id
                    //                          select item).ToList()[0];

                    //alterRec.ID = Convert.ToInt32(txtRecID.Text);
                    //alterRec.LeakValue = Convert.ToInt32(txtLeakValue.Text);
                    //alterRec.Remark = txtRecNote.Text;

                    //SetStaticItem("当前设置已批量应用到该组设备");
                }
                catch (Exception ex)
                {
                    ShowDialog("设置失败：" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SetStaticItem("设置失败");
                }
                finally
                {
                    //EnableRibbonBar();
                    //EnableNavigateBar();
                    //HideWaitForm();
                    //cl.Enabled = true;
                }
            //}).BeginInvoke(null, null);
        }

        public override void OnSerialPortNotify(object sender, SerialPortEventArgs e)
        {
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.NoiseBatchWrite)
            {
                
                if (e.TransactStatus == TransStatus.Success)
                {
                    if (OptRecList != null && OptRecList.Count > currentOptRecIndex)
                    {
                        // 更新设置入库
                        int query = NoiseDataBaseHelper.UpdateRecorder(OptRecList[currentOptRecIndex]);
                        if (query != -1)
                        {
                            //XtraMessageBox.Show("噪声记录仪[" + currentRec.ID + "]设置成功！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            GlobalValue.recorderList = NoiseDataBaseHelper.GetRecorders();
                            GlobalValue.groupList = NoiseDataBaseHelper.GetGroups();
                        }
                        else
                            XtraMessageBox.Show("噪声记录仪[" + OptRecList[currentOptRecIndex].ID + "]数据入库发生错误！", GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                        if (++currentOptRecIndex < OptRecList.Count)
                        {
                            GlobalValue.NoiseSerialPortOptData.ID = Convert.ToInt16(OptRecList[currentOptRecIndex].ID);
                            GlobalValue.SerialPortMgr.Send(SerialPortType.NoiseBatchWrite);
                        }
                        else
                        {
                            this.Enabled = true;
                            GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);
                            EnableRibbonBar();
                            EnableNavigateBar();
                            HideWaitForm();
                            XtraMessageBox.Show("批量设置噪声记录仪参数成功！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    if (OptRecList != null && OptRecList.Count > currentOptRecIndex)
                    {
                        HideWaitForm();
                        if (DialogResult.Yes == XtraMessageBox.Show("噪声记录仪[" + OptRecList[currentOptRecIndex].ID + "]设置参数失败,是否重试?" + e.Msg, GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                        {
                            GlobalValue.SerialPortMgr.Send(SerialPortType.NoiseBatchWrite);
                        }
                        else
                        {
                            if (++currentOptRecIndex < OptRecList.Count)
                            {
                                GlobalValue.NoiseSerialPortOptData.ID = Convert.ToInt16(OptRecList[currentOptRecIndex].ID);
                                GlobalValue.SerialPortMgr.Send(SerialPortType.NoiseBatchWrite);
                            }
                            else
                            {
                                this.Enabled = true;
                                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);
                                EnableRibbonBar();
                                EnableNavigateBar();
                                HideWaitForm();
                            }
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("噪声记录仪设置参数失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Enabled = true;
                        GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);
                        EnableRibbonBar();
                        EnableNavigateBar();
                        HideWaitForm();
                    }
                }
            }
        }

        void SerialPortParm_SerialPortScheduleEvent(object sender, SerialPortScheduleEventArgs e)
        {
            if (e.OptType == SerialPortType.NoiseBatchWrite && !string.IsNullOrEmpty(e.Msg))
            {
                ShowWaitForm("", e.Msg);
                SetStaticItem(e.Msg);
            }
        }

        #region 输入验证
        /// <summary>
        /// 验证记录仪管理选项卡输入是否正确
        /// </summary>
        private bool ValidateRecorderManageInput(out string msg)
        {
            bool ok;
            msg = string.Empty;
            ok = MetarnetRegex.IsTime(txtComTime.Text);
            if (!ok)
            {
                txtComTime.Focus();
                txtComTime.SelectAll();
                msg = "通讯时间设置错误！";
                return ok;
            }
            ok = MetarnetRegex.IsTime(txtRecTime.Text);
            if (!ok)
            {
                txtRecTime.Focus();
                txtRecTime.SelectAll();
                msg = "记录时间设置错误！";
                return ok;
            }
            ok = MetarnetRegex.IsUint(txtLeakValue.Text);
            if (!ok)
            {
                txtLeakValue.Focus();
                txtLeakValue.SelectAll();
                msg = "报漏幅度值设置错误！";
                return ok;
            }

            //if (cbConStart.Checked)
            //{
            //    ok = MetarnetRegex.IsUint(txtConId.Text);
            //    if (!ok)
            //    {
            //        txtConId.Focus();
            //        txtConId.SelectAll();
            //        msg = "控制器编号设置错误！";
            //        return ok;
            //    }
            //    ok = MetarnetRegex.IsUint(txtConPort.Text);
            //    if (!ok)
            //    {
            //        txtConPort.Focus();
            //        txtConPort.SelectAll();
            //        msg = "远传端口设置错误！";
            //        return ok;
            //    }
            //    ok = MetarnetRegex.IsIPv4(txtConAdress.Text);
            //    if (!ok)
            //    {
            //        txtConAdress.Focus();
            //        txtConAdress.SelectAll();
            //        msg = "远传地址设置错误！";
            //        return ok;
            //    }
            //}

            // 通讯时间与记录时间不能重叠
            int comTime = Convert.ToInt32(txtComTime.Text);
            int recTime1 = Convert.ToInt32(txtRecTime.Text);
            int recTime2 = Convert.ToInt32(txtRecTime1.Text);

            if (comTime == recTime1 || comTime == recTime2 || (comTime > recTime1 && comTime < recTime2))
            {
                txtComTime.Focus();
                txtComTime.SelectAll();
                msg = "通讯时间/记录时间设置重叠！";
                return false;
            }

            return ok;
        }
        #endregion

        public override void SerialPortEvent(bool Enabled)
        {
            btnSaveGroupSet.Enabled = Enabled;
            btnReadTemplate.Enabled = Enabled;
        }

        private void listBoxRec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (treeList1.Selection != null && treeList1.Selection.Count > 0)
            {
                if (listBoxRec.SelectedItems != null && listBoxRec.SelectedItems.Count > 0 && (treeList1.Selection[0].Level == 0))
                    btnImportRec.Enabled = true;
                else
                    btnImportRec.Enabled = false;
            }
        }
    }
}
