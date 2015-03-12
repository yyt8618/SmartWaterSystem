using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Common;

namespace NoiseAnalysisSystem
{
    public partial class NoiseParmSetting : BaseView, INoiseParmSetting
    {
        public NoiseParmSetting()
        {
            InitializeComponent();

            cbArith.Items.Add("直接平均法");
            cbArith.Items.Add("去最值平均法");
        }

        public override void OnLoad()
        {
            #region 计算参数
            txtMax1.Text = AppConfigHelper.GetAppSettingValue("Max1");
            txtMax2.Text = AppConfigHelper.GetAppSettingValue("Max2");
            txtMin1.Text = AppConfigHelper.GetAppSettingValue("Min1");
            txtMin2.Text = AppConfigHelper.GetAppSettingValue("Min2");
            txtLeakHZ.Text = AppConfigHelper.GetAppSettingValue("LeakHZ_Template");
            txtStandardAMP.Text = AppConfigHelper.GetAppSettingValue("StandardAMP");
            txtDCCompLen.Text = AppConfigHelper.GetAppSettingValue("DCComponentLen");

            if (AppConfigHelper.GetAppSettingValue("Calc") == (1).ToString())
                cbArith.SelectedIndex = 0;
            else if (AppConfigHelper.GetAppSettingValue("Calc") == (2).ToString())
                cbArith.SelectedIndex = 1;
            #endregion

            #region 模板参数
            txtComTime_T.Text = AppConfigHelper.GetAppSettingValue("ComTime_Template");
            txtRecTime_T.Text = AppConfigHelper.GetAppSettingValue("RecTime_Template");
            nUpDownSamSpan_T.Value = Convert.ToInt32(AppConfigHelper.GetAppSettingValue("Span_Template"));
            txtRecNum_T.Text = (GlobalValue.Time * 60 / Convert.ToInt32(AppConfigHelper.GetAppSettingValue("Span_Template"))).ToString();
            txtLeakValue_T.Text = AppConfigHelper.GetAppSettingValue("LeakValue_Template");
            int power = Convert.ToInt32(AppConfigHelper.GetAppSettingValue("Power_Template"));
            comboBoxEditPower.SelectedIndex = power;
            int conPower = Convert.ToInt32(AppConfigHelper.GetAppSettingValue("ControlPower_Template"));
            comboBoxEditDist.SelectedIndex = power;

            txtConPort_T.Text = AppConfigHelper.GetAppSettingValue("Port_Template");
            txtConAdree_T.Text = AppConfigHelper.GetAppSettingValue("Adress_Template");
            #endregion
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            #region controls validation
            if (string.IsNullOrEmpty(txtMax1.Text) || Convert.ToInt32(txtMax1.Text) <= 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("低频段傅里叶数据区间上限不能为空或小于零!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMax1.Focus();
                txtMax1.SelectAll();
                return;
            }
            if (string.IsNullOrEmpty(txtMin1.Text) || Convert.ToInt32(txtMin1.Text) <= 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("低频段傅里叶数据区间下限不能为空或小于零!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMin1.Focus();
                txtMin1.SelectAll();
                return;
            }

            if (Convert.ToInt32(txtMax1.Text) <= Convert.ToInt32(txtMin1.Text))
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("低频段傅里叶数据区间下限不能大于或等于上限!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMax1.Focus();
                txtMax1.SelectAll();
                return;
            }

            if (string.IsNullOrEmpty(txtMax2.Text) || Convert.ToInt32(txtMax2.Text) <= 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("高频段傅里叶数据区间上限不能为空或小于零!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMax2.Focus();
                txtMax2.SelectAll();
                return;
            }
            if (string.IsNullOrEmpty(txtMin2.Text) || Convert.ToInt32(txtMin2.Text) <= 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("高频段傅里叶数据区间下限不能为空或小于零!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMin2.Focus();
                txtMin2.SelectAll();
                return;
            }

            if (Convert.ToInt32(txtMax2.Text) <= Convert.ToInt32(txtMin2.Text))
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("高频段傅里叶数据区间下限不能大于或等于上限!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMax2.Focus();
                txtMax2.SelectAll();
                return;
            }

            if (string.IsNullOrEmpty(txtStandardAMP.Text) || Convert.ToDecimal(txtStandardAMP.Text) <= 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("请输入有效的静态漏水标准幅度值!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtStandardAMP.Focus();
                txtStandardAMP.SelectAll();
                return;
            }
            if (string.IsNullOrEmpty(txtDCCompLen.Text) || Convert.ToDecimal(txtDCCompLen.Text) <= 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("请输入有效的直流分量值!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDCCompLen.Focus();
                txtDCCompLen.SelectAll();
                return;
            }

            #endregion

            AppConfigHelper.SetAppSettingValue("Max1", txtMax1.Text);
            AppConfigHelper.SetAppSettingValue("Max2", txtMax2.Text);
            AppConfigHelper.SetAppSettingValue("Min1", txtMin1.Text);
            AppConfigHelper.SetAppSettingValue("Min2", txtMin2.Text);
            AppConfigHelper.SetAppSettingValue("LeakHZ_Template", txtLeakHZ.Text);
            AppConfigHelper.SetAppSettingValue("StandardAMP", txtStandardAMP.Text);
            AppConfigHelper.SetAppSettingValue("DCComponentLen", txtDCCompLen.Text);

            if (cbArith.SelectedIndex == 0)
                AppConfigHelper.SetAppSettingValue("Calc", (1).ToString());
            else if (cbArith.SelectedIndex == 1)
                AppConfigHelper.SetAppSettingValue("Calc", (2).ToString());

            XtraMessageBox.Show("保存成功！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 验证记录仪系统设置选项卡输入是否正确
        /// </summary>
        private bool ValidateSysSetInput(out string msg)
        {
            bool ok;
            msg = string.Empty;

            ok = MetarnetRegex.IsTime(txtComTime_T.Text);
            if (!ok)
            {
                txtComTime_T.Focus();
                txtComTime_T.SelectAll();
                msg = "通讯时间设置错误！";
                return ok;
            }
            ok = MetarnetRegex.IsTime(txtRecTime_T.Text);
            if (!ok)
            {
                txtRecTime_T.Focus();
                txtRecTime_T.SelectAll();
                msg = "记录时间设置错误！";
                return ok;
            }
            ok = MetarnetRegex.IsUint(txtLeakValue_T.Text);
            if (!ok)
            {
                txtLeakValue_T.Focus();
                txtLeakValue_T.SelectAll();
                msg = "报漏幅度值设置错误！";
                return ok;
            }

            ok = MetarnetRegex.IsUint(txtConPort_T.Text);
            if (!ok)
            {
                txtConPort_T.Focus();
                txtConPort_T.SelectAll();
                msg = "远传端口设置错误！";
                return ok;
            }
            ok = MetarnetRegex.IsIPv4(txtConAdree_T.Text);
            if (!ok)
            {
                txtConAdree_T.Focus();
                txtConAdree_T.SelectAll();
                msg = "远传地址设置错误！";
                return ok;
            }

            // 通讯时间与记录时间不能重叠
            int comTime = Convert.ToInt32(txtComTime_T.Text);
            int recTime1 = Convert.ToInt32(txtRecTime_T.Text);
            int recTime2 = Convert.ToInt32(txtRecTime1_T.Text);

            if (comTime == recTime1 || comTime == recTime2 || (comTime > recTime1 && comTime < recTime2))
            {
                txtComTime_T.Focus();
                txtComTime_T.SelectAll();
                msg = "通讯时间/记录时间设置重叠！";
                return false;
            }

            return ok;
        }

        private void btnSaveTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = string.Empty;
                if (!ValidateSysSetInput(out msg))
                {
                    throw new Exception(msg);
                }

                AppConfigHelper.SetAppSettingValue("ComTime_Template", txtComTime_T.Text);
                AppConfigHelper.SetAppSettingValue("RecTime_Template", txtRecTime_T.Text);
                AppConfigHelper.SetAppSettingValue("Span_Template", nUpDownSamSpan_T.Value.ToString());
                AppConfigHelper.SetAppSettingValue("LeakValue_Template", txtLeakValue_T.Text);
                AppConfigHelper.SetAppSettingValue("Power_Template", comboBoxEditPower.SelectedIndex.ToString());
                AppConfigHelper.SetAppSettingValue("ControlPower_Template", comboBoxEditDist.SelectedIndex.ToString());
                AppConfigHelper.SetAppSettingValue("Port_Template", txtConPort_T.Text);
                AppConfigHelper.SetAppSettingValue("Adress_Template", txtConAdree_T.Text);

                XtraMessageBox.Show("保存成功！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("保存失败：" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void nUpDownSamSpan_T_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown obj = sender as NumericUpDown;
            switch (obj.Name)
            {
                case "nUpDownSamSpan_T":
                    txtRecNum_T.Text = (GlobalValue.Time * 60 / obj.Value).ToString();
                    break;
            }
        }

        private void txtRecTime_T_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraEditors.TextEdit obj = sender as DevExpress.XtraEditors.TextEdit;
                int t = 0;
                int t1 = 0;
                switch (obj.Name)
                {
                    case "txtRecTime_T":
                        t = Convert.ToInt32(obj.Text);
                        t1 = t + GlobalValue.Time;
                        if (t1 > 24)
                            txtRecTime1_T.Text = (t1 - 24).ToString();
                        else
                            txtRecTime1_T.Text = (t + GlobalValue.Time).ToString();
                        break;
                }
            }
            catch (Exception)
            {
            }
        }

        private void txtRecTime_T_TextChanged(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.TextBox obj = sender as System.Windows.Forms.TextBox;
                int t = 0;
                int t1 = 0;
                switch (obj.Name)
                {
                    case "txtRecTime_T":
                        t = Convert.ToInt32(obj.Text);
                        t1 = t + GlobalValue.Time;
                        if (t1 > 24)
                            txtRecTime1_T.Text = (t1 - 24).ToString();
                        else
                            txtRecTime1_T.Text = (t + GlobalValue.Time).ToString();
                        break;
                }
            }
            catch (Exception)
            {
            }
        }



    }
}
