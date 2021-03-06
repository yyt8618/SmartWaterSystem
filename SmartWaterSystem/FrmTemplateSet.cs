﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Common;

namespace SmartWaterSystem
{
    public partial class FrmTemplateSet : DevExpress.XtraEditors.XtraForm
    {
        public FrmTemplateSet()
        {
            InitializeComponent();

            txtComTime_T.Text = Settings.Instance.GetString(SettingKeys.ComTime_Template);
            txtRecTime_T.Text = Settings.Instance.GetString(SettingKeys.RecTime_Template);
            nUpDownSamSpan_T.Value = Settings.Instance.GetInt(SettingKeys.Span_Template);
            txtRecNum_T.Text = (GlobalValue.Time * 60 / Settings.Instance.GetInt(SettingKeys.Span_Template)).ToString();
            txtLeakValue_T.Text = (new BLL.NoiseParmBLL()).GetParm(Entity.ConstValue.LeakValue_Template);
            int power = Settings.Instance.GetInt(SettingKeys.Power_Template);
            comboBoxEditPower.SelectedIndex = power;
            int conPower = Settings.Instance.GetInt(SettingKeys.ControlPower_Template);
            comboBoxEditDist.SelectedIndex = power;

            txtConPort_T.Text = Settings.Instance.GetString(SettingKeys.Port_Template);
            txtConAdree_T.Text = Settings.Instance.GetString(SettingKeys.Adress_Template);
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

                Settings.Instance.SetValue(SettingKeys.ComTime_Template, txtComTime_T.Text);
                Settings.Instance.SetValue(SettingKeys.RecTime_Template, txtRecTime_T.Text);
                Settings.Instance.SetValue(SettingKeys.Span_Template, nUpDownSamSpan_T.Value.ToString());
                (new BLL.NoiseParmBLL()).SetParm(Entity.ConstValue.LeakValue_Template, txtLeakValue_T.Text);
                Settings.Instance.SetValue(SettingKeys.Power_Template, comboBoxEditPower.SelectedIndex.ToString());
                Settings.Instance.SetValue(SettingKeys.ControlPower_Template, comboBoxEditDist.SelectedIndex.ToString());
                Settings.Instance.SetValue(SettingKeys.Port_Template, txtConPort_T.Text);
                Settings.Instance.SetValue(SettingKeys.Adress_Template, txtConAdree_T.Text);

                XtraMessageBox.Show("保存成功！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
				this.Close();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("保存失败：" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 自动计算采集数量
        private void nUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown obj = sender as NumericUpDown;
            switch (obj.Name)
            {
                case "nUpDownSamSpan_T":
                    txtRecNum_T.Text = (GlobalValue.Time * 60 / obj.Value).ToString();
                    break;
            }
        }

        // 自动计算记录时间
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox obj = sender as TextBox;
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

        private void simpleButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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
    }
}
