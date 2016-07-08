using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Entity;
using BLL;
using DevExpress.XtraEditors;
using System.Text.RegularExpressions;
using Common;
using System.Collections.Generic;

namespace SmartWaterSystem
{
    public partial class OLWQMgr : BaseView, IOLWQMgr
    {

        TerminalDataBLL Terbll = new TerminalDataBLL();

        public OLWQMgr()
        {
            InitializeComponent();
        }

        private void OLWQMgr_Load(object sender, EventArgs e)
        {
            InitColor();
            LoadTerminalData();

            if (gridTer.RowCount > 0)
                gridTer_RowClick(null, new DevExpress.XtraGrid.Views.Grid.RowClickEventArgs(new DevExpress.Utils.DXMouseEventArgs(System.Windows.Forms.MouseButtons.Left, 0, 0, 0, 0), 0));
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

        private void InitColor()
        {
            try
            {
                Color c = Color.FromArgb(Settings.Instance.GetInt(SettingKeys.OLWQLowLimitColor));
                colorPickLowLimit.Color = c;

                c = Color.FromArgb(Settings.Instance.GetInt(SettingKeys.OLWQUpLimitColor));
                colorPickUpLimit.Color = c;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("初始化拾色器失败!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(txtID.Text,@"^\d{1,5}$"))
            {
                XtraMessageBox.Show("请选择需要删除的终端", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (DialogResult.Yes == XtraMessageBox.Show("确定要删除[ID:" + txtID.Text + "]终端信息?", GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information))
            {
                if (Terbll.DeleteTer(TerType.OLWQTer, txtID.Text) == 1)
                {
                    if ((new OLWQConfigBLL()).Delete(txtID.Text.Trim()))
                    {
                        XtraMessageBox.Show("删除成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        XtraMessageBox.Show("删除发生异常，请联系管理员", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    XtraMessageBox.Show("删除发生异常，请联系管理员", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                ClearTerControls();
                LoadTerminalData();

                if (gridTer.RowCount > 0)
                    gridTer_RowClick(null, new DevExpress.XtraGrid.Views.Grid.RowClickEventArgs(new DevExpress.Utils.DXMouseEventArgs(System.Windows.Forms.MouseButtons.Left, 0, 0, 0, 0), 0));

                UpdateMonitroUI();
            }
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
            if (cboxTurbidityAlarm.Checked)
            {
                if (!Regex.IsMatch(txtTurbidityLowLimit.Text, @"^\d{1,4}(.\d{1,4})?$"))
                {
                    XtraMessageBox.Show("请填写合法的终端浊度下限值", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtTurbidityLowLimit.SelectAll();
                    txtTurbidityLowLimit.Focus();
                    return ;
                }
            }

            if (cboxTurbidityAlarm.Checked)
            {
                if (!Regex.IsMatch(txtTurbidityUpLimit.Text, @"^\d{1,4}(.\d{1,4})?$"))
                {
                    XtraMessageBox.Show("请填写合法的终端浊度上限值", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtTurbidityUpLimit.SelectAll();
                    txtTurbidityUpLimit.Focus();
                    return ;
                }
            }

            if (cboxResidualClAlarm.Checked)
            {
                if (!Regex.IsMatch(txtResidualClLowLimit.Text, @"^\d{1,4}(.\d{1,4})?$"))
                {
                    XtraMessageBox.Show("请填写合法的终端余氯下限值", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtResidualClLowLimit.SelectAll();
                    txtResidualClLowLimit.Focus();
                    return;
                }
            }

            if (cboxResidualClAlarm.Checked)
            {
                if (!Regex.IsMatch(txtResidualClUpLimit.Text, @"^\d{1,4}(.\d{1,4})?$"))
                {
                    XtraMessageBox.Show("请填写合法的终端余氯上限值", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtResidualClUpLimit.SelectAll();
                    txtResidualClUpLimit.Focus();
                    return;
                }
            }

            if (cboxPHAlarm.Checked)
            {
                if (!Regex.IsMatch(txtPHLowLimit.Text, @"^\d{1,4}(.\d{1,4})?$"))
                {
                    XtraMessageBox.Show("请填写合法的终端PH下限值", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPHLowLimit.SelectAll();
                    txtPHLowLimit.Focus();
                    return;
                }
            }

            if (cboxPHAlarm.Checked)
            {
                if (!Regex.IsMatch(txtPHUpLimit.Text, @"^\d{1,4}(.\d{1,4})?$"))
                {
                    XtraMessageBox.Show("请填写合法的终端PH上限值", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPHUpLimit.SelectAll();
                    txtPHUpLimit.Focus();
                    return;
                }
            }

            if (cboxConductivityAlarm.Checked)
            {
                if (!Regex.IsMatch(txtConductivityLowLimit.Text, @"^\d{1,4}(.\d{1,4})?$"))
                {
                    XtraMessageBox.Show("请填写合法的终端电导率下限值", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtConductivityLowLimit.SelectAll();
                    txtConductivityLowLimit.Focus();
                    return;
                }
            }

            if (cboxConductivityAlarm.Checked)
            {
                if (!Regex.IsMatch(txtConductivityUpLimit.Text, @"^\d{1,4}(.\d{1,4})?$"))
                {
                    XtraMessageBox.Show("请填写合法的终端电导率上限值", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtConductivityUpLimit.SelectAll();
                    txtConductivityUpLimit.Focus();
                    return;
                }
            }
            #endregion

            int saveresult=Terbll.SaveTerInfo(Convert.ToInt32(txtID.Text), txtName.Text, txtAddr.Text, txtRemark.Text,TerType.OLWQTer,null);
            if (saveresult == 1)
            {
                OLWQConfigEntity configEntity =new OLWQConfigEntity();
                configEntity.TerId = txtID.Text.Trim();
                configEntity.enableTurbidityAlarm = cboxTurbidityAlarm.Checked;
                if (configEntity.enableTurbidityAlarm)
                {
                    configEntity.TurbidityLowLimit = Convert.ToSingle(txtTurbidityLowLimit.Text);
                    configEntity.TurbidityUpLimit = Convert.ToSingle(txtTurbidityUpLimit.Text);
                }
                configEntity.enableResidualClAlarm = cboxResidualClAlarm.Checked;
                if (configEntity.enableResidualClAlarm)
                {
                    configEntity.ResidualClLowLimit = Convert.ToSingle(txtResidualClLowLimit.Text);
                    configEntity.ResidualClUpLimit = Convert.ToSingle(txtResidualClUpLimit.Text);
                }
                configEntity.enablePHAlarm = cboxPHAlarm.Checked;
                if (configEntity.enablePHAlarm)
                {
                    configEntity.PHLowLimit = Convert.ToSingle(txtPHLowLimit.Text);
                    configEntity.PHUpLimit = Convert.ToSingle(txtPHUpLimit.Text);
                }
                configEntity.enableConductivityAlarm = cboxConductivityAlarm.Checked;
                if (configEntity.enableConductivityAlarm)
                {
                    configEntity.ConductivityLowLimit = Convert.ToSingle(txtConductivityLowLimit.Text);
                    configEntity.ConductivityUpLimit = Convert.ToSingle(txtConductivityUpLimit.Text);
                }
                if ((new OLWQConfigBLL()).Insert(configEntity))
                {
                    XtraMessageBox.Show("保存成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadTerminalData();
                }
                else
                    XtraMessageBox.Show("保存发生异常，请联系管理员!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void gridTer_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                ClearTerControls();

                txtID.Text = gridTer.GetRowCellValue(e.RowHandle, "TerminalID").ToString().Trim();
                txtName.Text = gridTer.GetRowCellValue(e.RowHandle, "TerminalName").ToString().Trim();
                txtAddr.Text = gridTer.GetRowCellValue(e.RowHandle, "Address").ToString().Trim();
                txtRemark.Text = gridTer.GetRowCellValue(e.RowHandle, "Remark").ToString().Trim();

                List<OLWQConfigEntity> lst_Config=(new OLWQConfigBLL()).Select("TerminalID = '" + txtID.Text.Trim() + "'");
                if (lst_Config != null && lst_Config.Count > 0)
                {
                    cboxTurbidityAlarm.Checked = lst_Config[0].enableTurbidityAlarm;
                    cboxResidualClAlarm.Checked = lst_Config[0].enableResidualClAlarm;
                    cboxPHAlarm.Checked = lst_Config[0].enablePHAlarm;
                    cboxConductivityAlarm.Checked = lst_Config[0].enableConductivityAlarm;

                    txtTurbidityLowLimit.Text = lst_Config[0].TurbidityLowLimit.ToString();
                    txtTurbidityUpLimit.Text = lst_Config[0].TurbidityUpLimit.ToString();
                    txtResidualClLowLimit.Text = lst_Config[0].ResidualClLowLimit.ToString();
                    txtResidualClUpLimit.Text = lst_Config[0].ResidualClUpLimit.ToString();
                    txtPHLowLimit.Text = lst_Config[0].PHLowLimit.ToString();
                    txtPHUpLimit.Text = lst_Config[0].PHUpLimit.ToString();
                    txtConductivityLowLimit.Text = lst_Config[0].ConductivityLowLimit.ToString();
                    txtConductivityUpLimit.Text = lst_Config[0].ConductivityUpLimit.ToString();
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
        }

        private void ClearTerControls()
        {
            txtID.Text = "";
            txtName.Text = "";
            txtAddr.Text = "";
            txtRemark.Text = "";

            cboxTurbidityAlarm.Checked = false;
            cboxResidualClAlarm.Checked = false;
            cboxPHAlarm.Checked = false;
            cboxConductivityAlarm.Checked = false;

            txtTurbidityLowLimit.Text = "";
            txtTurbidityUpLimit.Text = "";
            txtResidualClLowLimit.Text = "";
            txtResidualClUpLimit.Text = "";
            txtPHLowLimit.Text = "";
            txtPHUpLimit.Text = "";
            txtConductivityLowLimit.Text = "";
            txtConductivityUpLimit.Text = "";
        }

        private void UpdateMonitroUI()
        {
            OLWQMonitor monitorView = (OLWQMonitor)GlobalValue.MainForm.GetView(typeof(OLWQMonitor));
            if (monitorView != null)
                monitorView.UpdateView();
        }

        private void colorPickLowLimit_EditValueChanged(object sender, EventArgs e)
        {
            Settings.Instance.SetValue(SettingKeys.OLWQLowLimitColor, colorPickLowLimit.Color.ToArgb());
            //MonitorView = (PreTerMonitor)GlobalValue.MainForm.GetView(typeof(PreTerMonitor));
            //if (MonitorView != null)
            //    MonitorView.UpdateColorsConfig();
        }

        private void colorPickUpLimit_EditValueChanged(object sender, EventArgs e)
        {
            Settings.Instance.SetValue(SettingKeys.OLWQUpLimitColor, colorPickUpLimit.Color.ToArgb());
            //MonitorView = (PreTerMonitor)GlobalValue.MainForm.GetView(typeof(PreTerMonitor));
            //if (MonitorView != null)
            //    MonitorView.UpdateColorsConfig();
        }
    }
}
