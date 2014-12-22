using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Protocol;

namespace NoiseAnalysisSystem
{
    public partial class UcGroupMgr : DevExpress.XtraEditors.XtraUserControl
    {
        private FrmSystem main;
        public UcGroupMgr(FrmSystem frm)
        {
            InitializeComponent();
            this.main = frm;
        }


        private void UcGroupMgr_Load(object sender, EventArgs e)
        {
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
                MessageBox.Show(ex.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 绑定树形列表
        /// </summary>
        public void BindTree()
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
                dr["ID"] = GlobalValue.groupList[i].ID;
                dr["Remark"] = GlobalValue.groupList[i].Remark;
                dr["Name"] = GlobalValue.groupList[i].Name;
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
                txtGroupID.Text = drv["ID"].ToString();
                txtGroupName.Text = drv["Name"].ToString();
                txtGroupNote.Text = drv["Remark"].ToString();

                btnAlterGroupSet.Enabled = true;

                if (listBoxRec.Items.Count == 0)
                    btnImportRec.Enabled = false;
                else
                    btnImportRec.Enabled = true;

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
                }

                NoiseRecorderGroup newGrp = new NoiseRecorderGroup();
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
                                         where temp.ID == Convert.ToInt32(treeList1.Selection[0].GetValue("ID"))
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
                BindListBox();
            }
        }

        // 移除记录仪
        private void btnDelRecFromGroup_Click(object sender, EventArgs e)
        {
            try
            {
                //DialogResult dr = MessageBox.Show("确定移除该记录仪？", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                //if (dr == System.Windows.Forms.DialogResult.Yes)
                //{

                List<DevExpress.XtraTreeList.Nodes.TreeListNode> nodes = treeList1.GetAllCheckedNodes();
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode item in nodes)
                {
                    if (item.Level == 1)
                    {
                        int recID = Convert.ToInt32(item.GetValue("ID"));
                        int gID = Convert.ToInt32(item.ParentNode.GetValue("ID"));

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
                MessageBox.Show("移除失败：" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txtComTime.Text = AppConfigHelper.GetAppSettingValue("ComTime_Template");
            txtRecTime.Text = AppConfigHelper.GetAppSettingValue("RecTime_Template");
            nUpDownSamSpan.Value = Convert.ToInt32(AppConfigHelper.GetAppSettingValue("Span_Template"));
            txtRecNum.Text = (GlobalValue.Time * 60 / Convert.ToInt32(AppConfigHelper.GetAppSettingValue("Span_Template"))).ToString();
            txtLeakValue.Text = AppConfigHelper.GetAppSettingValue("LeakValue_Template");

            int power = Convert.ToInt32(AppConfigHelper.GetAppSettingValue("Power_Template"));
            comboBoxEditPower.SelectedIndex = power;

            int conPower = Convert.ToInt32(AppConfigHelper.GetAppSettingValue("ControlPower_Template"));
            comboBoxDist.SelectedIndex = conPower;
        }

        private void btnSaveGroupSet_Click(object sender, EventArgs e)
        {
            new Action(() =>
            {
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
                        DevExpress.XtraEditors.XtraMessageBox.Show("请选择需要操作的分组", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    NoiseRecorderGroup CurrentGroup = (from tmp in GlobalValue.groupList
                                                       where tmp.ID == Convert.ToInt32(this.txtGroupID.Text)
                                                       select tmp).ToList()[0];
                    if (CurrentGroup != null && CurrentGroup.RecorderList != null && CurrentGroup.RecorderList.Count > 0)
                    {
                        main.DisableRibbonBar();
                        main.DisableNavigateBar();
                        main.ShowWaitForm("", "正在进行批量设置...");
                        main.barStaticItemWait.Caption = "正在进行批量设置...";

                        cl.Enabled = false;
                        short id = 0;
                        foreach (NoiseRecorder alterRec in CurrentGroup.RecorderList)
                        {
                            id = Convert.ToInt16(alterRec.ID);
                            // 设置记录时间段
                            GlobalValue.log.WriteStartEndTime(id, Convert.ToInt32(txtRecTime.Text), Convert.ToInt32(txtRecTime1.Text));
                            alterRec.RecordTime = Convert.ToInt32(txtRecTime.Text);

                            // 设置采集间隔
                            GlobalValue.log.WriteInterval(id, (int)nUpDownSamSpan.Value);
                            alterRec.PickSpan = Convert.ToInt32(nUpDownSamSpan.Value);

                            // 设置远传通讯时间
                            GlobalValue.log.WriteRemoteSendTime(id, Convert.ToInt32(txtComTime.Text));
                            alterRec.CommunicationTime = Convert.ToInt32(txtComTime.Text);

                            // 设置远传功能
                            if (comboBoxDist.SelectedIndex == 1)
                            {
                                GlobalValue.log.WriteRemoteSwitch(id, true);

                                alterRec.ControlerPower = 1;
                                //DistanceController alterCtrl = new DistanceController();
                                //alterCtrl.ID = Convert.ToInt32(txtConId.Text);
                                //alterCtrl.RecordID = alterRec.ID;
                                //alterCtrl.Port = Convert.ToInt32(txtConPort.Text);
                                //alterCtrl.IPAdress = txtConAdress.Text;

                                //NoiseDataBaseHelper.UpdateControler(alterCtrl);
                            }
                            else
                            {
                                GlobalValue.log.WriteRemoteSwitch(id, false);
                                alterRec.ControlerPower = 0;
                            }
                            short[] origitydata = null;
                            // 设置开关
                            if (comboBoxEditPower.SelectedIndex == 1)
                            {
                                GlobalValue.log.CtrlStartOrStop(id, true, out origitydata);
                                alterRec.Power = 1;
                            }
                            else if (comboBoxEditPower.SelectedIndex == 0)
                            {
                                GlobalValue.log.CtrlStartOrStop(id, false, out origitydata);
                                alterRec.Power = 0;
                            }

                            // 设置记录仪时间
                            GlobalValue.log.WriteTime(id, this.dateTimePicker.Value);

                            // 更新设置入库
                            int query = NoiseDataBaseHelper.UpdateRecorder(alterRec);
                            if (query != -1)
                            {
                                XtraMessageBox.Show("设置成功！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                GlobalValue.recorderList = NoiseDataBaseHelper.GetRecorders();
                                GlobalValue.groupList = NoiseDataBaseHelper.GetGroups();
                            }
                            else
                                throw new Exception("数据入库发生错误。");
                        }
                    }
                    else
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("请选择需要操作的分组", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    //short id = Convert.ToInt16(txtRecID.Text);
                    //NoiseRecorder alterRec = (from item in GlobalValue.recorderList
                    //                          where item.ID == id
                    //                          select item).ToList()[0];

                    //alterRec.ID = Convert.ToInt32(txtRecID.Text);
                    //alterRec.LeakValue = Convert.ToInt32(txtLeakValue.Text);
                    //alterRec.Remark = txtRecNote.Text;
                    
                    main.barStaticItemWait.Caption = "当前设置已批量应用到该组设备";

                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("设置失败：" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    main.barStaticItemWait.Caption = "设置失败";
                }
                finally
                {
                    main.EnableRibbonBar();
                    main.EnableNavigateBar();
                    main.HideWaitForm();
                    cl.Enabled = true;
                }
            }).BeginInvoke(null, null);
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
    }
}
