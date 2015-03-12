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
        }

        public override void OnLoad()
        {
            try
            {
                Uri uri = new Uri(Path.Combine(Application.StartupPath, "BaiduMap.htm"));
                webBrowser1.Url = uri;
            }
            catch (Exception ex)
            {
                logger.ErrorException("NoiseMap_Load", ex);
                XtraMessageBox.Show("加载地图出现异常!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
