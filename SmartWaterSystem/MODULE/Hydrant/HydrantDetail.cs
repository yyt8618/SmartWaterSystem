using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Entity;
using BLL;
using DevExpress.XtraEditors;

namespace SmartWaterSystem
{
    public partial class HydrantDetail : DevExpress.XtraEditors.XtraForm
    {
        HydrantBLL dataBll = new HydrantBLL();
        List<HydrantEntity> lstData = null;
        public static string HydrantID = "";

        public HydrantDetail()
        {
            InitializeComponent();
        }

        private void HydrantDetail_Load(object sender, EventArgs e)
        {
            this.Text = "消防栓[编号:" + HydrantID + "]数据详情";
            dtpStart.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dtpEnd.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);

            cbInterval.SelectedIndex = 0;
            cbOperate.SelectedIndex = 0;
        }

        private void btnAnalysis_Click(object sender, EventArgs e)
        {
            if (!GetDat())
            {
                return;
            }
            if(lstData!=null && lstData.Count>0)
            {
                lstDetailView.BeginUpdate();
                string str_opt = "";
                string pressvalue = "";
                string openangle = "";
                for (int i = 0; i < lstData.Count; i++)
                {
                    if (lstData[i].PressValue >= 0)
                        pressvalue = lstData[i].PressValue.ToString();
                    else
                        pressvalue = "---";
                    openangle ="---";
                    switch (lstData[i].OptType)
                    {
                        case HydrantOptType.Open:
                            str_opt = "打开";
                            openangle = lstData[i].OpenAngle.ToString();
                            break;
                        case HydrantOptType.Close:
                            str_opt = "关闭";
                            break;
                        case HydrantOptType.OpenAngle:
                            str_opt = "开度";
                            openangle = lstData[i].OpenAngle.ToString("f0");
                            break;
                        case HydrantOptType.Impact:
                            str_opt = "撞击";
                            break;
                        case HydrantOptType.KnockOver:
                            str_opt = "撞倒";
                            break;
                    }
                    ListViewItem item = new ListViewItem(new string[]{
                        lstData[i].CollTime,
                        str_opt,
                        pressvalue,
                        openangle
                    });
                    lstDetailView.Items.Add(item);
                }
                lstDetailView.EndUpdate();
            }
        }

        private bool GetDat()
        {
            if (dtpEnd.Value < dtpStart.Value)
            {
                XtraMessageBox.Show("结束数据不能大于起始时间!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtpEnd.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(cbInterval.Text))
            {
                XtraMessageBox.Show("间隔时间不能为空!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbInterval.Focus();
                return false;
            }
            int opt = -1;
            if (cbOperate.Text == "打开")
                opt = (int)HydrantOptType.Open;
            else if (cbOperate.Text == "关闭")
                opt = (int)HydrantOptType.Close;
            else if (cbOperate.Text == "开度")
                opt = (int)HydrantOptType.OpenAngle;
            else if (cbOperate.Text == "撞击")
                opt = (int)HydrantOptType.Impact;
            else if (cbOperate.Text == "撞倒")
                opt = (int)HydrantOptType.KnockOver;

            lstDetailView.Items.Clear();
            int interval = Convert.ToInt32(cbInterval.Text);
            lstData = dataBll.GetHydrantDetail(HydrantID, opt, dtpStart.Value, dtpEnd.Value, interval);

            if (lstData != null && lstData.Count > 0)
            {
                return true;
            }
            else
            {
                XtraMessageBox.Show("没有数据,请修改条件!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtpStart.Focus();
                return false;
            }
        }
    }
}
