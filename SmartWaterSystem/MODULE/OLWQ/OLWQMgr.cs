using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using Entity;
using BLL;
using DevExpress.XtraEditors;
using System.Text.RegularExpressions;

namespace SmartWaterSystem
{
    public partial class OLWQMgr : BaseView, IOLWQMgr
    {
        UniversalWayTypeBLL WayTypebll = new UniversalWayTypeBLL();
        TerminalDataBLL Terbll = new TerminalDataBLL();
        TreeListNode currentNode;  //当前操作的Node
        List<UniversalWayTypeEntity> lstComboboxdata = null;  //ComboboxEdit 控件数据源

        public OLWQMgr()
        {
            InitializeComponent();
        }

        private void OLWQMgr_Load(object sender, EventArgs e)
        {
            BindCombobox();
            LoadTreeList();
            ClearCollectCtrl();
            LoadTerminalData();

            if (gridTer.RowCount > 0)
                gridTer_RowClick(null, new DevExpress.XtraGrid.Views.Grid.RowClickEventArgs(new DevExpress.Utils.DXMouseEventArgs(System.Windows.Forms.MouseButtons.Left, 0, 0, 0, 0), 0));
        }

        private void ClearComboboxValue()
        {
            //simulate
            cbSimulate1.Properties.Items.Clear();
            cbSimulate2.Properties.Items.Clear();
            cbSimulate3.Properties.Items.Clear();

            //RS485
            cbRS485_1.Properties.Items.Clear();
            cbRS485_2.Properties.Items.Clear();
            cbRS485_3.Properties.Items.Clear();
            cbRS485_4.Properties.Items.Clear();
            cbRS485_5.Properties.Items.Clear();
            cbRS485_6.Properties.Items.Clear();
            cbRS485_7.Properties.Items.Clear();
            cbRS485_8.Properties.Items.Clear();
        }

        private void LoadTerminalData()
        {
            gridControls.DataSource = null;
            DataTable dt_ter = Terbll.GetTerInfo(TerType.OLWQTer);
            if (dt_ter != null && dt_ter.Rows.Count > 0)
            {
                gridTer.BeginDataUpdate();
                gridControls.DataSource = dt_ter;
                gridTer.EndDataUpdate();
            }
        }

        private void BindCombobox()
        {
            ClearComboboxValue();

            lstComboboxdata = WayTypebll.Select("WHERE Level = '1' AND SyncState!=-1 AND TerminalType='" + ((int)TerType.OLWQTer).ToString() + "' ORDER BY WayType,Name");
            if (lstComboboxdata != null && lstComboboxdata.Count > 0)
            {
                foreach (UniversalWayTypeEntity entity in lstComboboxdata)
                {
                    if (entity.WayType == UniversalCollectType.Simulate)
                    {
                        //simulate
                        cbSimulate1.Properties.Items.Add(entity.Name); cbSimulate1.Tag = (cbSimulate1.Tag != null ? cbSimulate1.Tag.ToString() + "," + entity.ID : entity.ID.ToString());
                        cbSimulate2.Properties.Items.Add(entity.Name); cbSimulate2.Tag = (cbSimulate2.Tag != null ? cbSimulate2.Tag.ToString() + "," + entity.ID : entity.ID.ToString());
                        cbSimulate3.Properties.Items.Add(entity.Name); cbSimulate3.Tag = (cbSimulate3.Tag != null ? cbSimulate3.Tag.ToString() + "," + entity.ID : entity.ID.ToString());
                    }
                    else
                    {
                        //RS485
                        cbRS485_1.Properties.Items.Add(entity.Name); cbRS485_1.Tag = (cbRS485_1.Tag != null ? cbRS485_1.Tag.ToString() + "," + entity.ID : entity.ID.ToString());
                        cbRS485_2.Properties.Items.Add(entity.Name); cbRS485_2.Tag = (cbRS485_2.Tag != null ? cbRS485_2.Tag.ToString() + "," + entity.ID : entity.ID.ToString());
                        cbRS485_3.Properties.Items.Add(entity.Name); cbRS485_3.Tag = (cbRS485_3.Tag != null ? cbRS485_3.Tag.ToString() + "," + entity.ID : entity.ID.ToString());
                        cbRS485_4.Properties.Items.Add(entity.Name); cbRS485_4.Tag = (cbRS485_4.Tag != null ? cbRS485_4.Tag.ToString() + "," + entity.ID : entity.ID.ToString());
                        cbRS485_5.Properties.Items.Add(entity.Name); cbRS485_5.Tag = (cbRS485_5.Tag != null ? cbRS485_5.Tag.ToString() + "," + entity.ID : entity.ID.ToString());
                        cbRS485_6.Properties.Items.Add(entity.Name); cbRS485_6.Tag = (cbRS485_6.Tag != null ? cbRS485_6.Tag.ToString() + "," + entity.ID : entity.ID.ToString());
                        cbRS485_7.Properties.Items.Add(entity.Name); cbRS485_7.Tag = (cbRS485_7.Tag != null ? cbRS485_7.Tag.ToString() + "," + entity.ID : entity.ID.ToString());
                        cbRS485_8.Properties.Items.Add(entity.Name); cbRS485_8.Tag = (cbRS485_8.Tag != null ? cbRS485_8.Tag.ToString() + "," + entity.ID : entity.ID.ToString());
                    }
                }
            }
        }

        #region TreeList
        private void LoadTreeList()
        {
            List<UniversalWayTypeEntity> lstNodedata = WayTypebll.Select("WHERE SyncState !=-1 AND TerminalType='" + ((int)TerType.OLWQTer).ToString() + "' ORDER BY WayType,Sequence");
            if (lstNodedata != null && lstNodedata.Count > 0)
            {
                treeCollectType.BeginUnboundLoad();
                TreeListNode tmpnode = null;
                foreach (UniversalWayTypeEntity data in lstNodedata)
                {
                    if (data.Level == 1)
                    {
                        tmpnode = treeCollectType.AppendNode(new object[] { ConstValue.GetUniversalCollectTypeName(data.WayType) + "  " + data.Name }, -1, data);
                    }
                }
                foreach (UniversalWayTypeEntity data in lstNodedata)
                {
                    if (data.Level == 2)
                    {
                        tmpnode = treeCollectType.AppendNode(new object[] { data.Name }, findFather(data.ParentID), data);
                    }
                }

                treeCollectType.ExpandAll();
                treeCollectType.EndUnboundLoad();
            }
        }

        //tree id与数据库id编码不同,不能通用
        private int findFather(int tag_parentid)
        {
            if (treeCollectType.Nodes != null && treeCollectType.Nodes.Count > 0)
            {
                foreach (TreeListNode node in treeCollectType.Nodes)
                {
                    if (node.Level == 0)
                    {
                        if (node.Tag != null && ((UniversalWayTypeEntity)node.Tag).ID == tag_parentid)
                        {
                            return node.Id;
                        }
                    }
                }
            }
            return -1;
        }

        private void treeCollectType_MouseDown(object sender, MouseEventArgs e)
        {
            ClearCollectCtrl();
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                treeCollectType.ContextMenuStrip = null;
                TreeListHitInfo hInfo = treeCollectType.CalcHitInfo(new Point(e.X, e.Y));
                TreeListNode node = hInfo.Node;
                treeCollectType.FocusedNode = node;
                if (node != null)
                {
                    if (node.Level == 0)
                    {
                        currentNode = node;
                        menuTree.Items[0].Visible = true;
                        menuTree.Items[1].Visible = true;
                        treeCollectType.ContextMenuStrip = menuTree;
                    }
                    else
                    {
                        currentNode = node;
                        menuTree.Items[0].Visible = false;
                        menuTree.Items[1].Visible = true;
                        treeCollectType.ContextMenuStrip = menuTree;
                    }
                }
                else
                {
                    currentNode = null;
                    menuTree.Items[0].Visible = true;
                    menuTree.Items[1].Visible = false;
                    treeCollectType.ContextMenuStrip = menuTree;
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left)  //左键选定
            {
                TreeListHitInfo hInfo = treeCollectType.CalcHitInfo(new Point(e.X, e.Y));
                TreeListNode node = hInfo.Node;
                if (node != null && node.Tag != null)
                {
                    UniversalWayTypeEntity entity = (UniversalWayTypeEntity)node.Tag;
                    if (entity != null)
                    {
                        txtCollectType.Text = ConstValue.GetUniversalCollectTypeName(entity.WayType);
                        txtTypeName.Text = entity.Name;
                        if (node.HasChildren)
                        {
                            txtFrameWidth.Text = "------";
                            txtMaxMeasureR.Text = "------";
                            txtMaxMeasureRFlag.Text = "------";
                            txtPrecision.Text = "------";
                            txtUnit.Text = "------";
                        }
                        else
                        {
                            txtFrameWidth.Text = entity.FrameWidth.ToString();
                            txtMaxMeasureR.Text = entity.MaxMeasureRange.ToString();
                            txtMaxMeasureRFlag.Text = entity.ManMeasureRangeFlag.ToString();
                            txtPrecision.Text = entity.Precision.ToString();
                            txtUnit.Text = entity.Unit;
                            if (entity.WayType == UniversalCollectType.Simulate)//模拟
                            {
                                lblMaxMeasureR.Text = "最大测量范围:";
                            }
                            else if (entity.WayType == UniversalCollectType.Pluse) //脉冲
                            {
                                lblMaxMeasureR.Text = "单位脉冲大小:";
                                txtMaxMeasureRFlag.Text = "------";
                            }
                            else if (entity.WayType == UniversalCollectType.RS485) //RS485
                            {
                                lblMaxMeasureR.Text = "最大测量范围:";
                            }
                        }
                        
                    }
                }
            }
        }

        private void ClearCollectCtrl()
        {
            txtCollectType.Text = "";
            txtTypeName.Text = "";
            txtMaxMeasureR.Text = "";
            txtMaxMeasureRFlag.Text = "";
            txtPrecision.Text = "0";
            txtUnit.Text = "";
        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool formtypeEnable = true;
            int selectedindex = 0;
            if (currentNode != null)
            {
                selectedindex = (int)((UniversalWayTypeEntity)currentNode.Tag).WayType;
                if (currentNode.Level == 0)
                    formtypeEnable = false;
            }
            OLWQTreeNodeInfoForm dialogform = new OLWQTreeNodeInfoForm(formtypeEnable, selectedindex);
            if (DialogResult.OK != dialogform.ShowDialog())
            {
                return;
            }

            UniversalWayTypeEntity nodeentity = dialogform.GetTypeEntity();
            nodeentity.SyncState = 0;

            treeCollectType.BeginUnboundLoad();
            if (currentNode == null)
            {
                nodeentity.Level = 1;
                nodeentity.ParentID = -1;
                currentNode = treeCollectType.AppendNode(new object[] { ConstValue.GetUniversalCollectTypeName(nodeentity.WayType) +"  "+ nodeentity.Name }, -1);
                currentNode.Tag = nodeentity;
            }
            else
            {
                nodeentity.Level = 2;
                nodeentity.ParentID = ((UniversalWayTypeEntity)currentNode.Tag).ID;
                currentNode = treeCollectType.AppendNode(new object[] { nodeentity.Name}, currentNode.Id);
                currentNode.Tag = nodeentity;
            }

            //save to sqlite
            if (nodeentity.ParentID == -1)
                nodeentity.Sequence = 0;
            else
            {
                int sequence =WayTypebll.GetMaxSequence(nodeentity.ParentID,TerType.OLWQTer);
                if (sequence == -1)
                {
                    XtraMessageBox.Show("读取数据库最大Sequence失败,请联系管理员!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    treeCollectType.DeleteNode(currentNode);
                    currentNode = null;
                    treeCollectType.EndUnboundLoad();
                    return;
                }
                nodeentity.Sequence = sequence + 1;
            }
            nodeentity.SyncState = 1;
            int saveresult = WayTypebll.Insert(nodeentity,TerType.OLWQTer);
            if (-1 == saveresult)
            {
                XtraMessageBox.Show("保存数据发生异常,请联系管理员", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                treeCollectType.DeleteNode(currentNode);
                currentNode = null;
            }
            treeCollectType.ExpandAll();
            treeCollectType.EndUnboundLoad();

            BindCombobox();

            GlobalValue.SQLSyncMgr.Send(SqlSyncType.SyncUpdate_UniversalTerWayType);
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == XtraMessageBox.Show("确定删除当前类型?(通用终端该类型配置都将删除)", GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information))
            {
                return;
            }
            if (currentNode != null)
            {
                while (currentNode.HasChildren)
                {
                    TreeListNodes childs = currentNode.Nodes;
                    if (childs != null && childs.Count > 0)
                    {
                        if ((WayTypebll.UpdateFlag(((UniversalWayTypeEntity)childs[0].Tag).ID,-1) == -1) || (Terbll.DeleteUniversalWayTypeConfig(((UniversalWayTypeEntity)childs[0].Tag).ID) == -1))
                        {
                            XtraMessageBox.Show("删除数据发生异常,请联系管理员", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            treeCollectType.DeleteNode(childs[0]);
                        }
                    }
                }
                if ((WayTypebll.UpdateFlag(((UniversalWayTypeEntity)currentNode.Tag).ID, -1) == -1) || (Terbll.DeleteUniversalWayTypeConfig(((UniversalWayTypeEntity)currentNode.Tag).ID) == -1))
                {
                    XtraMessageBox.Show("删除数据发生异常,请联系管理员", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    treeCollectType.DeleteNode(currentNode);
                }
                currentNode = null;

                WayTypeControlsDefault();
                BindCombobox();

                GlobalValue.SQLSyncMgr.Send(SqlSyncType.SyncUpdate_UniversalTerWayType);
                GlobalValue.SQLSyncMgr.Send(SqlSyncType.SyncUpdate_UniversalTerWayConfig);
            }
        }
        #endregion

        #region Simulate Visiable
        private void ceSimulate1_CheckedChanged(object sender, EventArgs e)
        {
            if (!ceSimulate1.Checked)
                cbSimulate1.SelectedIndex = -1;
        }

        private void ceSimulate2_CheckedChanged(object sender, EventArgs e)
        {
            if (!ceSimulate2.Checked)
                cbSimulate2.SelectedIndex = -1;
        }

        private void ceSimulate3_CheckedChanged(object sender, EventArgs e)
        {
            if (!ceSimulate3.Checked)
                cbSimulate3.SelectedIndex = -1;
        }
        #endregion

        #region RS485 Visiable
        private void ceRS485_1_CheckedChanged(object sender, EventArgs e)
        {
            if (ceRS485_1.Checked)
            {
                ceRS485_2.Visible = true;
                cbRS485_2.Visible = true;
                ceRS485_2.Checked = false;
                cbRS485_2.SelectedIndex = -1;
            }
            else
            {
                cbRS485_1.SelectedIndex = -1;

                ceRS485_2.Visible = false;
                cbRS485_2.Visible = false;

                ceRS485_3.Visible = false;
                cbRS485_3.Visible = false;

                ceRS485_4.Visible = false;
                cbRS485_4.Visible = false;

                ceRS485_5.Visible = false;
                cbRS485_5.Visible = false;

                ceRS485_6.Visible = false;
                cbRS485_6.Visible = false;

                ceRS485_7.Visible = false;
                cbRS485_7.Visible = false;

                ceRS485_8.Visible = false;
                cbRS485_8.Visible = false;
            }
        }

        private void ceRS485_2_CheckedChanged(object sender, EventArgs e)
        {
            if (ceRS485_2.Checked)
            {
                ceRS485_3.Visible = true;
                cbRS485_3.Visible = true;
                ceRS485_3.Checked = false;
                cbRS485_3.SelectedIndex = -1;
            }
            else
            {
                cbRS485_2.SelectedIndex = -1;

                ceRS485_3.Visible = false;
                cbRS485_3.Visible = false;

                ceRS485_4.Visible = false;
                cbRS485_4.Visible = false;

                ceRS485_5.Visible = false;
                cbRS485_5.Visible = false;

                ceRS485_6.Visible = false;
                cbRS485_6.Visible = false;

                ceRS485_7.Visible = false;
                cbRS485_7.Visible = false;

                ceRS485_8.Visible = false;
                cbRS485_8.Visible = false;
            }
        }

        private void ceRS485_3_CheckedChanged(object sender, EventArgs e)
        {
            if (ceRS485_3.Checked)
            {
                ceRS485_4.Visible = true;
                cbRS485_4.Visible = true;
                ceRS485_4.Checked = false;
                cbRS485_4.SelectedIndex = -1;
            }
            else
            {
                cbRS485_3.SelectedIndex = -1;

                ceRS485_4.Visible = false;
                cbRS485_4.Visible = false;

                ceRS485_5.Visible = false;
                cbRS485_5.Visible = false;

                ceRS485_6.Visible = false;
                cbRS485_6.Visible = false;

                ceRS485_7.Visible = false;
                cbRS485_7.Visible = false;

                ceRS485_8.Visible = false;
                cbRS485_8.Visible = false;
            }
        }

        private void ceRS485_4_CheckedChanged(object sender, EventArgs e)
        {
            if (ceRS485_4.Checked)
            {
                ceRS485_5.Visible = true;
                cbRS485_5.Visible = true;
                ceRS485_5.Checked = false;
                cbRS485_5.SelectedIndex = -1;
            }
            else
            {
                cbRS485_4.SelectedIndex = -1;

                ceRS485_5.Visible = false;
                cbRS485_5.Visible = false;

                ceRS485_6.Visible = false;
                cbRS485_6.Visible = false;

                ceRS485_7.Visible = false;
                cbRS485_7.Visible = false;

                ceRS485_8.Visible = false;
                cbRS485_8.Visible = false;
            }
        }

        private void ceRS485_5_CheckedChanged(object sender, EventArgs e)
        {
            if (ceRS485_5.Checked)
            {
                ceRS485_6.Visible = true;
                cbRS485_6.Visible = true;
                ceRS485_6.Checked = false;
                cbRS485_6.SelectedIndex = -1;
            }
            else
            {
                cbRS485_5.SelectedIndex = -1;

                ceRS485_6.Visible = false;
                cbRS485_6.Visible = false;

                ceRS485_7.Visible = false;
                cbRS485_7.Visible = false;

                ceRS485_8.Visible = false;
                cbRS485_8.Visible = false;
            }
        }

        private void ceRS485_6_CheckedChanged(object sender, EventArgs e)
        {
            if (ceRS485_6.Checked)
            {
                ceRS485_7.Visible = true;
                cbRS485_7.Visible = true;
                ceRS485_7.Checked = false;
                cbRS485_7.SelectedIndex = -1;
            }
            else
            {
                cbRS485_6.SelectedIndex = -1;

                ceRS485_7.Visible = false;
                cbRS485_7.Visible = false;

                ceRS485_8.Visible = false;
                cbRS485_8.Visible = false;
            }
        }

        private void ceRS485_7_CheckedChanged(object sender, EventArgs e)
        {
            if (ceRS485_7.Checked)
            {
                ceRS485_8.Visible = true;
                cbRS485_8.Visible = true;
                ceRS485_8.Checked = false;
                cbRS485_8.SelectedIndex = -1;
            }
            else
            {
                cbRS485_7.SelectedIndex = -1;
                ceRS485_8.Visible = false;
                cbRS485_8.Visible = false;
            }
        }

        private void ceRS485_8_CheckedChanged(object sender, EventArgs e)
        {
            if(!ceRS485_8.Checked)
            {
                cbRS485_8.SelectedIndex = -1;
            }
        }
        #endregion

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(txtID.Text,@"^\d{1,5}$"))
            {
                XtraMessageBox.Show("请选择需要删除的终端", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (DialogResult.Yes == XtraMessageBox.Show("确定要删除[ID:" + txtID.Text + "]终端信息?", GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information))
            {
                if ((Terbll.DeleteTer(TerType.UniversalTer, txtID.Text) == 1) && (Terbll.DeleteUniversalWayTypeConfig_TerID(Convert.ToInt32(txtID.Text),TerType.OLWQTer) == 1))
                {
                    XtraMessageBox.Show("删除成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GlobalValue.SQLSyncMgr.Send(SqlSyncType.SyncTerminal);
                    GlobalValue.SQLSyncMgr.Send(SqlSyncType.SyncUpdate_UniversalTerWayConfig);
                }
                else
                {
                    XtraMessageBox.Show("删除发生异常，请联系管理员", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                WayTypeControlsDefault();
                ClearTerControls();
                LoadTerminalData();

                if (gridTer.RowCount > 0)
                    gridTer_RowClick(null, new DevExpress.XtraGrid.Views.Grid.RowClickEventArgs(new DevExpress.Utils.DXMouseEventArgs(System.Windows.Forms.MouseButtons.Left, 0, 0, 0, 0), 0));

                UpdateMonitroUI();
            }
        }

        public int GetWayTypeId(string WayTypeName)
        {
            if (lstComboboxdata != null && lstComboboxdata.Count > 0)
            {
                foreach (UniversalWayTypeEntity entity in lstComboboxdata)
                {
                    if (entity.Name == WayTypeName)
                    {
                        return entity.ID;
                    }
                }
            }
            return -1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            #region 校验
            if (string.IsNullOrEmpty(txtID.Text))
            {
                XtraMessageBox.Show("请输入终端编号!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return;
            }
            if (!Regex.IsMatch(txtID.Text, @"^\d{1,5}$"))
            {
                XtraMessageBox.Show("请输入合法终端编号!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtName.Text))
            {
                XtraMessageBox.Show("请输入终端名称!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Focus();
                return;
            }

            List<UniversalWayTypeConfigEntity> lstPointID = new List<UniversalWayTypeConfigEntity>();
            if (ceSimulate1.Checked )
            {
                if (cbSimulate1.SelectedIndex < 0)
                {
                    XtraMessageBox.Show("请选择模拟1路采集类型!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbSimulate1.Focus();
                    return;
                }
                else
                {
                    int pointid = GetWayTypeId(cbSimulate1.Text);
                    lstPointID.Add(new UniversalWayTypeConfigEntity(1, pointid));
                }
            }

            if (ceSimulate2.Checked)
            {
                if (cbSimulate2.SelectedIndex < 0)
                {
                    XtraMessageBox.Show("请选择模拟2路采集类型!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbSimulate2.Focus();
                    return;
                }
                else
                {
                    int pointid = GetWayTypeId(cbSimulate2.Text);
                    if (null == lstPointID.Find(a => a.PointID == (pointid)))
                    {
                        lstPointID.Add(new UniversalWayTypeConfigEntity(2, pointid));
                    }
                    else
                    {
                        XtraMessageBox.Show("请选择模拟2路采集类型!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cbSimulate2.Focus();
                        return;
                    }
                }
            }

            if (ceSimulate3.Checked)
            {
                if (cbSimulate3.SelectedIndex < 0)
                {
                    XtraMessageBox.Show("请选择模拟3路采集类型!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbSimulate3.Focus();
                    return;
                }
                else
                {
                    int pointid = GetWayTypeId(cbSimulate3.Text);
                    if (null == lstPointID.Find(a => a.PointID == (pointid)))
                    {
                        lstPointID.Add(new UniversalWayTypeConfigEntity(3, pointid));
                    }
                    else
                    {
                        XtraMessageBox.Show("请选择模拟3路采集类型!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cbSimulate3.Focus();
                        return;
                    }
                }
            }

            if (ceRS485_1.Checked)
            {
                if (cbRS485_1.SelectedIndex < 0)
                {
                    XtraMessageBox.Show("请选择RS485 1路采集类型!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbRS485_1.Focus();
                    return;
                }
                else
                {
                    int pointid = GetWayTypeId(cbRS485_1.Text);
                    if (null == lstPointID.Find(a => a.PointID == (pointid)))
                    {
                        lstPointID.Add(new UniversalWayTypeConfigEntity(9, pointid));
                    }
                    else
                    {
                        XtraMessageBox.Show("请选择RS485 1路采集类型!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cbRS485_1.Focus();
                        return;
                    }
                }
            }

            if (ceRS485_2.Checked)
            {
                if (cbRS485_2.SelectedIndex < 0)
                {
                    XtraMessageBox.Show("请输入RS485 2路采集类型!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbRS485_2.Focus();
                    return;
                }
                else
                {
                    int pointid = GetWayTypeId(cbRS485_2.Text);
                    if (null == lstPointID.Find(a => a.PointID == (pointid)))
                    {
                        lstPointID.Add(new UniversalWayTypeConfigEntity(10, pointid));
                    }
                    else
                    {
                        XtraMessageBox.Show("请选择RS485 2路采集类型!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cbRS485_2.Focus();
                        return;
                    }
                }
            }

            if (ceRS485_3.Checked)
            {
                if (cbRS485_3.SelectedIndex < 0)
                {
                    XtraMessageBox.Show("请输入RS485 3路采集类型!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbRS485_3.Focus();
                    return;
                }
                else
                {
                    int pointid = GetWayTypeId(cbRS485_3.Text);
                    if (null == lstPointID.Find(a => a.PointID == (pointid)))
                    {
                        lstPointID.Add(new UniversalWayTypeConfigEntity(11, pointid));
                    }
                    else
                    {
                        XtraMessageBox.Show("请选择RS485 3路采集类型!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cbRS485_3.Focus();
                        return;
                    }
                }
            }

            if (ceRS485_4.Checked)
            {
                if (cbRS485_4.SelectedIndex < 0)
                {
                    XtraMessageBox.Show("请输入RS485 4路采集类型!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbRS485_4.Focus();
                    return;
                }
                else
                {
                    int pointid = GetWayTypeId(cbRS485_4.Text);
                    if (null == lstPointID.Find(a => a.PointID == (pointid)))
                    {
                        lstPointID.Add(new UniversalWayTypeConfigEntity(12, pointid));
                    }
                    else
                    {
                        XtraMessageBox.Show("请选择RS485 4路采集类型!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cbRS485_4.Focus();
                        return;
                    }
                }
            }

            if (ceRS485_5.Checked)
            {
                if (cbRS485_5.SelectedIndex < 0)
                {
                    XtraMessageBox.Show("请输入RS485 5路采集类型!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbRS485_5.Focus();
                    return;
                }
                else
                {
                    int pointid = GetWayTypeId(cbRS485_5.Text);
                    if (null == lstPointID.Find(a => a.PointID == (pointid)))
                    {
                        lstPointID.Add(new UniversalWayTypeConfigEntity(13, pointid));
                    }
                    else
                    {
                        XtraMessageBox.Show("请选择RS485 5路采集类型!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cbRS485_5.Focus();
                        return;
                    }
                }
            }

            if (ceRS485_6.Checked)
            {
                if (cbRS485_6.SelectedIndex < 0)
                {
                    XtraMessageBox.Show("请输入RS485 6路采集类型!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbRS485_6.Focus();
                    return;
                }
                else
                {
                    int pointid = GetWayTypeId(cbRS485_6.Text);
                    if (null == lstPointID.Find(a => a.PointID == (pointid)))
                    {
                        lstPointID.Add(new UniversalWayTypeConfigEntity(14, pointid));
                    }
                    else
                    {
                        XtraMessageBox.Show("请选择RS485 6路采集类型!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cbRS485_6.Focus();
                        return;
                    }
                }
            }

            if (ceRS485_7.Checked)
            {
                if (cbRS485_7.SelectedIndex < 0)
                {
                    XtraMessageBox.Show("请输入RS485 7路采集类型!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbRS485_7.Focus();
                    return;
                }
                else
                {
                    int pointid = GetWayTypeId(cbRS485_7.Text);
                    if (null == lstPointID.Find(a => a.PointID == (pointid)))
                    {
                        lstPointID.Add(new UniversalWayTypeConfigEntity(15, pointid));
                    }
                    else
                    {
                        XtraMessageBox.Show("请选择RS485 7路采集类型!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cbRS485_7.Focus();
                        return;
                    }
                }
            }

            if (ceRS485_8.Checked)
            {
                if (cbRS485_8.SelectedIndex < 0)
                {
                    XtraMessageBox.Show("请输入RS485 8路采集类型!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbRS485_8.Focus();
                    return;
                }
                else
                {
                    int pointid = GetWayTypeId(cbRS485_8.Text);
                    if (null == lstPointID.Find(a => a.PointID == (pointid)))
                    {
                        lstPointID.Add(new UniversalWayTypeConfigEntity(16, pointid));
                    }
                    else
                    {
                        XtraMessageBox.Show("请选择RS485 8路采集类型!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cbRS485_8.Focus();
                        return;
                    }
                }
            }
            #endregion

            int saveresult=Terbll.SaveTerInfo(Convert.ToInt32(txtID.Text), txtName.Text, txtAddr.Text, txtRemark.Text,TerType.OLWQTer, lstPointID);
            if (saveresult == 1)
            {
                XtraMessageBox.Show("保存成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTerminalData();
                GlobalValue.SQLSyncMgr.Send(SqlSyncType.SyncTerminal);
                GlobalValue.SQLSyncMgr.Send(SqlSyncType.SyncUpdate_UniversalTerWayConfig);
            }
            else
            {
                XtraMessageBox.Show("保存发生异常，请联系管理员!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            UpdateMonitroUI();
        }

        private void gridTer_CustomDrawEmptyForeground(object sender, DevExpress.XtraGrid.Views.Base.CustomDrawEventArgs e)
        {
            DataTable dt = this.gridTer.DataSource as DataTable;
            if (dt==null || dt.Rows.Count == 0)
            {
                string str = "没有终端信息，请配置!";
                Font f = new Font("宋体", 10, FontStyle.Bold);
                Rectangle r = new Rectangle(e.Bounds.Top-5, e.Bounds.Left + 200, e.Bounds.Right - 3, e.Bounds.Height - 3);
                e.Graphics.DrawString(str, f, Brushes.Black, r);
            }
        }

        private void WayTypeControlsDefault()
        {
            ceSimulate1.Checked = false;
            ceSimulate2.Checked = false;
            ceSimulate3.Checked = false;
            ceRS485_1.Checked = false;
            ceRS485_2.Checked = false;
            ceRS485_3.Checked = false;
            ceRS485_4.Checked = false;
            ceRS485_5.Checked = false;
            ceRS485_6.Checked = false;
            ceRS485_7.Checked = false;
            ceRS485_8.Checked = false;

            cbSimulate1.SelectedIndex = -1;
            cbSimulate2.SelectedIndex = -1;
            cbSimulate3.SelectedIndex = -1;
            cbRS485_1.SelectedIndex = -1;
            cbRS485_2.SelectedIndex = -1;
            cbRS485_3.SelectedIndex = -1;
            cbRS485_4.SelectedIndex = -1;
            cbRS485_5.SelectedIndex = -1;
            cbRS485_6.SelectedIndex = -1;
            cbRS485_7.SelectedIndex = -1;
            cbRS485_8.SelectedIndex = -1;

        }

        private void gridTer_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                ClearTerControls();

                txtID.Text = gridTer.GetRowCellValue(e.RowHandle, "TerminalID").ToString().Trim();
                txtName.Text = gridTer.GetRowCellValue(e.RowHandle, "TerminalName").ToString().Trim();
                txtAddr.Text = gridTer.GetRowCellValue(e.RowHandle, "Address").ToString().Trim();
                txtRemark.Text = gridTer.GetRowCellValue(e.RowHandle, "Remark").ToString().Trim();
                
                WayTypeControlsDefault();

                List<UniversalWayTypeConfigEntity> lstWayTypeConfig = Terbll.GetUniversalWayTypeConfig(Convert.ToInt32(txtID.Text),TerType.OLWQTer);
                if (lstWayTypeConfig != null && lstWayTypeConfig.Count > 0)
                {
                    foreach (UniversalWayTypeConfigEntity entity in lstWayTypeConfig)
                    {
                        switch (entity.Sequence)
                        {
                            case 1:
                                ceSimulate1.Checked = true;
                                FindWayTypeConfig(cbSimulate1, entity.PointID);
                                break;
                            case 2:
                                ceSimulate2.Checked = true;
                                FindWayTypeConfig(cbSimulate2, entity.PointID);
                                break;
                            case 3:
                                ceSimulate3.Checked = true;
                                FindWayTypeConfig(cbSimulate3, entity.PointID);
                                break;
                            case 9:
                                ceRS485_1.Checked = true;
                                ceRS485_1.Visible = true;
                                FindWayTypeConfig(cbRS485_1, entity.PointID);
                                break;
                            case 10:
                                ceRS485_2.Checked = true;
                                ceRS485_2.Visible = true;
                                FindWayTypeConfig(cbRS485_2, entity.PointID);
                                break;
                            case 11:
                                ceRS485_3.Checked = true;
                                ceRS485_3.Visible = true;
                                FindWayTypeConfig(cbRS485_3, entity.PointID);
                                break;
                            case 12:
                                ceRS485_4.Checked = true;
                                ceRS485_4.Visible = true;
                                FindWayTypeConfig(cbRS485_4, entity.PointID);
                                break;
                            case 13:
                                ceRS485_5.Checked = true;
                                ceRS485_5.Visible = true;
                                FindWayTypeConfig(cbRS485_5, entity.PointID);
                                break;
                            case 14:
                                ceRS485_6.Checked = true;
                                ceRS485_6.Visible = true;
                                FindWayTypeConfig(cbRS485_6, entity.PointID);
                                break;
                            case 15:
                                ceRS485_7.Checked = true;
                                ceRS485_7.Visible = true;
                                FindWayTypeConfig(cbRS485_7, entity.PointID);
                                break;
                            case 16:
                                ceRS485_8.Checked = true;
                                ceRS485_8.Visible = true;
                                FindWayTypeConfig(cbRS485_8, entity.PointID);
                                break;
                        }
                    }
                }
            }
        }

        private void FindWayTypeConfig(ComboBoxEdit control,int PointID)
        {
            string[] ids = null;
            if (control.Tag != null)
                ids = control.Tag.ToString().Split(',');
            if (ids != null && ids.Length > 0)
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    if (ids[i].Trim() == PointID.ToString())
                    {
                        control.Visible = true;
                        control.SelectedIndex = i;
                        break;
                    }
                }
            }
            /*
            for (int i = 0; i < lstComboboxdata.Count; i++)
            {
                if (lstComboboxdata[i].ID == PointID && (i < control.Properties.Items.Count))
                {
                    control.Visible = true;
                    control.SelectedIndex = i;
                    break;
                }
            }*/
        }

        private void ClearTerControls()
        {
            txtID.Text = "";
            txtName.Text = "";
            txtAddr.Text = "";
            txtRemark.Text = "";

            WayTypeControlsDefault();
        }
        private void UpdateMonitroUI()
        {
            UniversalTerMonitor monitorView = (UniversalTerMonitor)GlobalValue.MainForm.GetView(typeof(UniversalTerMonitor));
            if (monitorView != null)
                monitorView.UpdateView();
        }

        
    }
}
