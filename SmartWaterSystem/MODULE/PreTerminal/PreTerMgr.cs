using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Common;
using Entity;
using System.Text.RegularExpressions;
using BLL;
using DevExpress.XtraEditors;

namespace SmartWaterSystem
{
    public partial class PreTerMgr : BaseView,IPreTerMgr
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("PreTerMgr");
        TerminalConfigBLL configBLL = new TerminalConfigBLL();

        public PreTerMonitor MonitorView = null;

        public PreTerMgr()
        {
            InitializeComponent();
        }

        private void PreTerMgr_Load(object sender, EventArgs e)
        {
            MonitorView = (PreTerMonitor)GlobalValue.MainForm.GetView(typeof(PreTerMonitor));
            InitView();
        }

        private void InitView()
        {
            ClearControls();
            GetConfigFromDB();
            InitColor();
        }

        private void InitColor()
        {
            try
            {
                Color c = Color.FromArgb(Settings.Instance.GetInt(SettingKeys.PreLowLimitColor));
                colorPickPreLowLimit.Color = c;

                c = Color.FromArgb(Settings.Instance.GetInt(SettingKeys.PreUpLimtColor));
                colorPickPreUpLimit.Color = c;

                c = Color.FromArgb(Settings.Instance.GetInt(SettingKeys.PreSlopeLowLimitColor));
                colorPickSlopeLowLimit.Color = c;

                c = Color.FromArgb(Settings.Instance.GetInt(SettingKeys.PreSlopeUpLimitColor));
                colorPickSlopeUpLimit.Color = c;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("初始化拾色器失败!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.ErrorException("InitColor", ex);

            }
        }

        //清除界面控件数据
        private void ClearControls()
        {
            try
            {
                ClearConfigControls();

                lstTerminalConfigView.Items.Clear();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("清除界面数据发生异常,ex:" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.ErrorException("ClearControls", ex);
            }
        }

        private void ClearConfigControls()
        {
            txtTerminalID.Text = "";
            txtTerminalName.Text = "";
            txtAddress.Text = "";
            txtRemark.Text = "";

            txtPreLowLimit.Text = "";
            txtPreUpLimite.Text = "";

            txtSlopeLowLimit.Text = "";
            txtSlopeUpLimit.Text = "";

            cboxPreAlarm.Checked = true;
            cboxSlopeAlarm.Checked = true;
        }

        private void GetConfigFromDB()
        {
            List<TerminalConfigEntity> lstConfigs = configBLL.GetAllPreTerminals();
            lstTerminalConfigView.Items.Clear();
            if (lstConfigs != null && lstConfigs.Count > 0)
            {
                string str_prelowlimit = "";
                string str_preuplimit = "";
                string str_slopelowlimit = "";
                string str_slopeuplimit = "";
                foreach (TerminalConfigEntity entity in lstConfigs)
                {
                    if (entity.EnablePreAlarm)
                    {
                        str_prelowlimit = entity.PreLowLimit.ToString("f3");
                        str_preuplimit = entity.PreUpLimit.ToString("f3");
                    }
                    else
                    {
                        str_prelowlimit = "--";
                        str_preuplimit = "--";
                    }
                    if (entity.EnableSlopeAlarm)
                    {
                        str_slopelowlimit = entity.PreSlopeLowLimit.ToString("f3");
                        str_slopeuplimit = entity.PreSlopeUpLimit.ToString("f3");
                    }
                    else
                    {
                        str_slopelowlimit = "--";
                        str_slopeuplimit = "--";
                    }
                    ListViewItem item = new ListViewItem(new string[]{
                    entity.TerminalID.ToString(),
                    entity.TerminalName,

                    str_prelowlimit,
                    str_preuplimit,
                    str_slopelowlimit,
                    str_slopeuplimit,

                    entity.TerminalAddr,
                    entity.Remark,

                    entity.EnablePreAlarm.ToString(),
                    entity.EnableSlopeAlarm.ToString()
                    });

                    lstTerminalConfigView.Items.Add(item);
                }
            }
        }

        private void lstTerminalConfigView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection selectCol = lstTerminalConfigView.SelectedIndices;
            if (selectCol != null && selectCol.Count > 0)
            {
                if (selectCol[0] > -1)
                {
                    if (!String.IsNullOrEmpty(lstTerminalConfigView.Items[selectCol[0]].SubItems[0].Text))
                    {
                        txtTerminalID.Text = lstTerminalConfigView.Items[selectCol[0]].SubItems[0].Text;
                        txtTerminalName.Text = lstTerminalConfigView.Items[selectCol[0]].SubItems[1].Text.Trim();
                        txtPreLowLimit.Text = lstTerminalConfigView.Items[selectCol[0]].SubItems[2].Text;
                        txtPreUpLimite.Text = lstTerminalConfigView.Items[selectCol[0]].SubItems[3].Text;
                        txtSlopeLowLimit.Text = lstTerminalConfigView.Items[selectCol[0]].SubItems[4].Text;
                        txtSlopeUpLimit.Text = lstTerminalConfigView.Items[selectCol[0]].SubItems[5].Text;
                        txtAddress.Text = lstTerminalConfigView.Items[selectCol[0]].SubItems[6].Text.Trim();
                        txtRemark.Text = lstTerminalConfigView.Items[selectCol[0]].SubItems[7].Text.Trim();
                        cboxPreAlarm.Checked = lstTerminalConfigView.Items[selectCol[0]].SubItems[8].Text.ToLower() == "true" ? true : false;
                        cboxSlopeAlarm.Checked = lstTerminalConfigView.Items[selectCol[0]].SubItems[9].Text.ToLower() == "true" ? true : false;
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                TerminalConfigEntity entity = null;
                if (configBLL.IsExist(txtTerminalID.Text.Trim()))
                {
                    if (DialogResult.Yes == XtraMessageBox.Show("终端编号[" + txtTerminalID.Text + "]已经存在,是否更新?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                    {
                        entity = GetEntity();
                        if (configBLL.Modify(entity))
                        {
                            XtraMessageBox.Show("更新成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            MonitorView = (PreTerMonitor)GlobalValue.MainForm.GetView(typeof(PreTerMonitor));
                            if (MonitorView != null)
                            {
                                MonitorView.ShowTerList(false, true);
                                MonitorView.ShowTerData();
                            }

                            ClearConfigControls();
                            GetConfigFromDB();
                            
                        }
                        else
                        {
                            XtraMessageBox.Show("更新失败!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                        return;
                }
                else
                {
                    if (DialogResult.Yes == XtraMessageBox.Show("是否新增终端编号[" + txtTerminalID.Text + "]?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                    {
                        entity = GetEntity();
                        if (configBLL.Insert(entity))
                        {
                            XtraMessageBox.Show("新增成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            MonitorView = (PreTerMonitor)GlobalValue.MainForm.GetView(typeof(PreTerMonitor));
                            if (MonitorView != null)
                            {
                                MonitorView.ShowTerList(false,true);
                                MonitorView.ShowTerData();
                            }

                            ClearConfigControls();
                            GetConfigFromDB();
                        }
                        else
                        {
                            XtraMessageBox.Show("新增失败!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                        return;
                }
            }
        }


        private void btnDel_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection selectCol = lstTerminalConfigView.SelectedIndices;
            if (selectCol != null && selectCol.Count > 0)
            {
                if (DialogResult.Yes == XtraMessageBox.Show("确定要删除该数据?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                {
                    if (configBLL.DeletePreTer(lstTerminalConfigView.Items[selectCol[0]].SubItems[0].Text))
                    {
                        XtraMessageBox.Show("删除成功", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MonitorView = (PreTerMonitor)GlobalValue.MainForm.GetView(typeof(PreTerMonitor));
                        if (MonitorView != null)
                        {
                            MonitorView.ShowTerList(false,true);
                            MonitorView.ShowTerData();
                        }

                        ClearConfigControls();
                        GetConfigFromDB();
                    }
                    else
                    {
                        XtraMessageBox.Show("删除失败", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
                XtraMessageBox.Show("请选择一条数据操作", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool Validate()
        {
            if (!Regex.IsMatch(txtTerminalID.Text, @"^\d+$"))
            {
                XtraMessageBox.Show("请填写合法的终端编号", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTerminalID.SelectAll();
                txtTerminalID.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtTerminalName.Text))
            {
                XtraMessageBox.Show("请填写合法的终端名称", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTerminalName.SelectAll();
                txtTerminalName.Focus();
                return false;
            }

            if (cboxPreAlarm.Checked)
            {
                if (!Regex.IsMatch(txtPreLowLimit.Text, @"^\d{1,4}(.\d{1,4})?$"))
                {
                    XtraMessageBox.Show("请填写合法的终端压力下限值", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPreLowLimit.SelectAll();
                    txtPreLowLimit.Focus();
                    return false;
                }
            }

            if (cboxPreAlarm.Checked)
            {
                if (!Regex.IsMatch(txtPreUpLimite.Text, @"^\d{1,4}(.\d{1,4})?$"))
                {
                    XtraMessageBox.Show("请填写合法的终端压力上限值", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPreUpLimite.SelectAll();
                    txtPreUpLimite.Focus();
                    return false;
                }
            }

            if (cboxSlopeAlarm.Checked)
            {
                if (!Regex.IsMatch(txtSlopeLowLimit.Text, @"^\d{1,4}(.\d{1,4})?$"))
                {
                    XtraMessageBox.Show("请填写合法的斜率下限值", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtSlopeLowLimit.SelectAll();
                    txtSlopeLowLimit.Focus();
                    return false;
                }
            }

            if (cboxSlopeAlarm.Checked)
            {
                if (!Regex.IsMatch(txtSlopeUpLimit.Text, @"^\d{1,4}(.\d{1,4})?$"))
                {
                    XtraMessageBox.Show("请填写合法的斜率上限值", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtSlopeUpLimit.SelectAll();
                    txtSlopeUpLimit.Focus();
                    return false;
                }
            }

            return true;
        }


        private TerminalConfigEntity GetEntity()
        {
            TerminalConfigEntity entity = new TerminalConfigEntity();
            entity.TerminalID = Convert.ToInt32(txtTerminalID.Text.Trim());
            entity.TerminalName = txtTerminalName.Text.Trim();
            entity.TerminalAddr = txtAddress.Text.Trim();
            entity.Remark = txtRemark.Text;
            entity.EnablePreAlarm = cboxPreAlarm.Checked;
            entity.EnableSlopeAlarm = cboxSlopeAlarm.Checked;

            if (cboxPreAlarm.Checked)
            {
                entity.PreLowLimit = Convert.ToDecimal(txtPreLowLimit.Text);
                entity.PreUpLimit = Convert.ToDecimal(txtPreUpLimite.Text);
            }
            else
            {
                entity.PreLowLimit = 0;
                entity.PreUpLimit = 0;
            }

            if (cboxSlopeAlarm.Checked)
            {
                entity.PreSlopeLowLimit = Convert.ToDecimal(txtSlopeLowLimit.Text);
                entity.PreSlopeUpLimit = Convert.ToDecimal(txtSlopeUpLimit.Text);
            }
            else
            {
                entity.PreSlopeLowLimit = 0;
                entity.PreSlopeUpLimit = 0;
            }

            return entity;
        }

        private void colorPickPreLowLimit_EditValueChanged(object sender, EventArgs e)
        {
            Settings.Instance.SetValue(SettingKeys.PreLowLimitColor, colorPickPreLowLimit.Color.ToArgb());
            MonitorView = (PreTerMonitor)GlobalValue.MainForm.GetView(typeof(PreTerMonitor));
            if (MonitorView != null)
                MonitorView.UpdateColorsConfig();
        }

        private void colorPickSlopeLowLimit_EditValueChanged(object sender, EventArgs e)
        {
            Settings.Instance.SetValue(SettingKeys.PreSlopeLowLimitColor, colorPickSlopeLowLimit.Color.ToArgb());
            MonitorView = (PreTerMonitor)GlobalValue.MainForm.GetView(typeof(PreTerMonitor));
            if (MonitorView != null)
                MonitorView.UpdateColorsConfig();
        }

        private void colorPickPreUpLimit_EditValueChanged(object sender, EventArgs e)
        {
            Settings.Instance.SetValue(SettingKeys.PreUpLimtColor, colorPickPreUpLimit.Color.ToArgb());
            MonitorView = (PreTerMonitor)GlobalValue.MainForm.GetView(typeof(PreTerMonitor));
            if (MonitorView != null)
                MonitorView.UpdateColorsConfig();
        }


        private void colorPickSlopeUpLimit_EditValueChanged(object sender, EventArgs e)
        {
            Settings.Instance.SetValue(SettingKeys.PreSlopeUpLimitColor, colorPickSlopeUpLimit.Color.ToArgb());
            MonitorView = (PreTerMonitor)GlobalValue.MainForm.GetView(typeof(PreTerMonitor));
            if (MonitorView != null)
                MonitorView.UpdateColorsConfig();
        }

        private void cboxPreAlarm_CheckedChanged(object sender, EventArgs e)
        {
            txtPreLowLimit.Enabled = cboxPreAlarm.Checked;
            txtPreUpLimite.Enabled = cboxPreAlarm.Checked;
        }

        private void cboxSlopeAlarm_CheckedChanged(object sender, EventArgs e)
        {
            txtSlopeLowLimit.Enabled = cboxSlopeAlarm.Checked;
            txtSlopeUpLimit.Enabled = cboxSlopeAlarm.Checked;
        }

    }
}
