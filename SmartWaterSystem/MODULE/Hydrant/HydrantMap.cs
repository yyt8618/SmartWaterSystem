using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraEditors;
using ChartDirector;
using System.Security.Permissions;
using BLL;

namespace SmartWaterSystem
{
    //[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    //[System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class HydrantMap : BaseView, IHydrantMap
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("HydrantMap");
        public HydrantMap()
        {
            InitializeComponent();

            try
            {
                Uri uri = new Uri(Path.Combine(Application.StartupPath, ".\\HydrantMap\\BaiduMap.htm"));
                webBrowser1.Url = uri;
                MapForScripting mapscripting=new MapForScripting();
                mapscripting.webBrow = this.webBrowser1;
                webBrowser1.ObjectForScripting = mapscripting;
            }
            catch (Exception ex)
            {
                logger.ErrorException("HydrantMap_Load", ex);
                XtraMessageBox.Show("加载地图出现异常!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

        public override void OnLoad()
        {
            
        }

        
    }
}
