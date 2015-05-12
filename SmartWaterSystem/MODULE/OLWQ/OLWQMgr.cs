using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Entity;
using BLL;
using DevExpress.XtraEditors;
using System.Text.RegularExpressions;

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
                    XtraMessageBox.Show("删除成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GlobalValue.SQLSyncMgr.Send(SqlSyncType.SyncTerminal);
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
            #endregion

            int saveresult=Terbll.SaveTerInfo(Convert.ToInt32(txtID.Text), txtName.Text, txtAddr.Text, txtRemark.Text,TerType.OLWQTer,null);
            if (saveresult == 1)
            {
                XtraMessageBox.Show("保存成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTerminalData();
                GlobalValue.SQLSyncMgr.Send(SqlSyncType.SyncTerminal);
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
        }
        private void UpdateMonitroUI()
        {
            OLWQMonitor monitorView = (OLWQMonitor)GlobalValue.MainForm.GetView(typeof(OLWQMonitor));
            if (monitorView != null)
                monitorView.UpdateView();
        }

        
    }
}
