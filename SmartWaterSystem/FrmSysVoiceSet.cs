using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.IO;
using Common;

namespace SmartWaterSystem
{
    public partial class FrmSysVoiceSet : DevExpress.XtraEditors.XtraForm
    {
        string[] str;
        public FrmSysVoiceSet()
        {
            InitializeComponent();

            string s1 = Application.StartupPath + @"\Voice\";
            str = Directory.GetFiles(s1, "*.wav");
            foreach (string s in str)
            {
                int i = listBoxVoice.Items.Add(s.Split('\\').Last());
            }

            lblLeakVoice.Text = Settings.Instance.GetString(SettingKeys.LeakVoice);
            lblRecordVoice.Text = Settings.Instance.GetString(SettingKeys.RecorderVoice);
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (listBoxVoice.SelectedIndex != -1)
            {
                SoundPlayer player = new SoundPlayer(str[listBoxVoice.SelectedIndex]);
                player.Play();
            }
        }

        private void btnSelectVoice_Click(object sender, EventArgs e)
        {
            if (listBoxVoice.SelectedItem != null)
            {
                lblLeakVoice.Text = listBoxVoice.SelectedItem.ToString();
                Settings.Instance.SetValue(SettingKeys.LeakVoice, lblLeakVoice.Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBoxVoice.SelectedItem != null)
            {
                lblRecordVoice.Text = listBoxVoice.SelectedItem.ToString();
                Settings.Instance.SetValue(SettingKeys.RecorderVoice, lblRecordVoice.Text);
            }
        }
    }
}
