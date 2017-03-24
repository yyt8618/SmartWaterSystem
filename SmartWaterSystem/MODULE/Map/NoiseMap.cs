using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraEditors;
using ChartDirector;

namespace SmartWaterSystem
{
    public partial class NoiseMap : BaseView, INoiseMap
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("NoiseMap");
        public NoiseMap()
        {
            InitializeComponent();

            try
            {
                Uri uri = new Uri(Path.Combine(Application.StartupPath, ".\\NoiseMap\\NoiseBaiduMap.htm"));
                webBrowser1.Url = uri;
                MapForScripting mapscripting = new MapForScripting();
                mapscripting.webBrow = this.webBrowser1;
                webBrowser1.ObjectForScripting = mapscripting;
            }
            catch (Exception ex)
            {
                logger.ErrorException("NoiseMap_Load", ex);
                XtraMessageBox.Show("加载地图出现异常!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override void OnLoad()
        {
        }

        MapFullScreen frm = null;
        private void btnFullScreen_Click(object sender, EventArgs e)
        {
            frm = new MapFullScreen();
            AddEventKeyUp(this.webBrowser1);
            this.Controls.Clear();
            frm.Controls.Add(this.webBrowser1);
            frm.ShowDialog();
        }

        private void AddEventKeyUp(Control control)
        {
            try
            {
                if (control != null)
                {
                    control.KeyUp -= new KeyEventHandler(control_KeyUp);
                    control.KeyUp += new KeyEventHandler(control_KeyUp);
                    foreach (Control c in control.Controls)
                    {
                        AddEventKeyUp(c);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void control_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                try
                {
                    if (this.webBrowser1 != null)
                    {
                        if (frm != null)
                        {
                            frm.Controls.Clear();
                            this.Controls.Add(this.webBrowser1);

                            frm.Close();
                            frm = null;
                        }
                    }
                    this.Controls.Add(this.btnFullScreen);
                    this.btnFullScreen.Click -= new EventHandler(btnFullScreen_Click);
                    this.btnFullScreen.Click += new EventHandler(btnFullScreen_Click);
                    webBrowser1.SendToBack();
                    btnFullScreen.BringToFront();
                    btnFullScreen.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

    }
}
