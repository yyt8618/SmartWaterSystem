using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace NoiseAnalysisSystem
{
	public partial class FrmCalcParamSet : DevExpress.XtraEditors.XtraForm
	{
		public FrmCalcParamSet()
		{
			InitializeComponent();
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			AppConfigHelper.SetAppSettingValue("Max1", txtMax1.Text);
			AppConfigHelper.SetAppSettingValue("Max2", txtMax2.Text);
			AppConfigHelper.SetAppSettingValue("Min1", txtMin1.Text);
			AppConfigHelper.SetAppSettingValue("Min2", txtMin2.Text);
			AppConfigHelper.SetAppSettingValue("LeakHZ_Template", txtLeakHZ.Text);

			if(comboBox.SelectedIndex==0)
				AppConfigHelper.SetAppSettingValue("Calc", (1).ToString());
			else if (comboBox.SelectedIndex == 1)
				AppConfigHelper.SetAppSettingValue("Calc", (2).ToString());
            else if (comboBox.SelectedIndex == 2)
                AppConfigHelper.SetAppSettingValue("Calc", (3).ToString());

            XtraMessageBox.Show("保存成功！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
			this.Close();
		}

		private void FrmCalcParamSet_Load(object sender, EventArgs e)
		{
			txtMax1.Text = AppConfigHelper.GetAppSettingValue("Max1");
			txtMax2.Text = AppConfigHelper.GetAppSettingValue("Max2");
			txtMin1.Text = AppConfigHelper.GetAppSettingValue("Min1");
			txtMin2.Text = AppConfigHelper.GetAppSettingValue("Min2");
			txtLeakHZ.Text = AppConfigHelper.GetAppSettingValue("LeakHZ_Template");

			if (AppConfigHelper.GetAppSettingValue("Calc") == (1).ToString())
				comboBox.SelectedIndex = 0;
			else if (AppConfigHelper.GetAppSettingValue("Calc") == (2).ToString())
				comboBox.SelectedIndex = 1;
		}

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
	}
}
