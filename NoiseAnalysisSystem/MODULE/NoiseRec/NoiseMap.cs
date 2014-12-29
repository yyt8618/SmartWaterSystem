using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraEditors;
using ChartDirector;

namespace NoiseAnalysisSystem
{
    public partial class NoiseMap : DevExpress.XtraEditors.XtraUserControl
    {
        private FrmSystem main;
        public NoiseMap(FrmSystem frm)
        {
            InitializeComponent();
            this.main = frm;
        }

        private void TestChart_Load(object sender, EventArgs e)
        {
            try
            {
                //DevExpress.XtraMap
            }
            catch { }
        }


    }
}
