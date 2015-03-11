using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NoiseAnalysisSystem
{
    public partial class UniversalTerParm : BaseView,IUniversalTerParm
    {
        public UniversalTerParm()
        {
            InitializeComponent();
        }

        //public UniversalTerParm(FrmSystem parentform)
        //{
        //    InitializeComponent();
        //}

        private void UniversalTerParm_Load(object sender, EventArgs e)
        {
            ceColConfig.Checked = false;
            ceDigitalPreOne.Enabled = ceColConfig.Checked;
            cePulse.Enabled = ceColConfig.Checked;
            ceExternalAO.Enabled = ceColConfig.Checked;
            ceExternalAOSec.Enabled = ceColConfig.Checked;
            ceExternal485.Enabled = ceColConfig.Checked;

            ceColConfig_CheckedChanged(null, null);
            ceDigitalPreOne_CheckedChanged(null, null);
        }

        private void ceColConfig_CheckedChanged(object sender, EventArgs e)
        {
            ceDigitalPreOne.Enabled = ceColConfig.Checked;
            cePulse.Enabled = ceColConfig.Checked;
            ceExternalAO.Enabled = ceColConfig.Checked;
            ceExternalAOSec.Enabled = ceColConfig.Checked;
            ceExternal485.Enabled = ceColConfig.Checked;
        }

        private void btnNow_Click(object sender, EventArgs e)
        {
            dateTimePicker.Value = DateTime.Now;
        }

        private void ceDigitalPreOne_CheckedChanged(object sender, EventArgs e)
        {
            cePreUpLimit.Enabled = ceDigitalPreOne.Checked;
            txtPreUpLimit.Enabled = ceDigitalPreOne.Checked;

            cePreUpEnable.Enabled = ceDigitalPreOne.Checked;
            cbPreUpEnable.Enabled = ceDigitalPreOne.Checked;

            cePreLowLimit.Enabled = ceDigitalPreOne.Checked;
            txtPreLowLimit.Enabled = ceDigitalPreOne.Checked;

            cePreLowEnable.Enabled = ceDigitalPreOne.Checked;
            cbPreLowEnable.Enabled = ceDigitalPreOne.Checked;

            ceHammerInterval.Enabled = ceDigitalPreOne.Checked;
            txtHammerInterval.Enabled = ceDigitalPreOne.Checked;

            ceHammerEnable.Enabled = ceDigitalPreOne.Checked;
            cbHammerEnable.Enabled = ceDigitalPreOne.Checked;
        }
    }
}
