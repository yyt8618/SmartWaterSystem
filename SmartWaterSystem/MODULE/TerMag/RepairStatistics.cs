using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraEditors;
using BLL;
using Entity;
using System.Data;
using Common;
using DevExpress.XtraGrid.Views.BandedGrid.ViewInfo;
using DevExpress.XtraGrid.Columns;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Text.RegularExpressions;

namespace SmartWaterSystem
{
    public partial class RepairStatistics : BaseView, IUniversalTerMonitor
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("UniversalTerMonitor");
        public RepairStatistics()
        {
            InitializeComponent();
        }

        private void RepairStatistics_Load(object sender, EventArgs e)
        {
        }
        
    }
}
