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
    public partial class NoiseMap : BaseView, INoiseMap
    {
        public NoiseMap()
        {
            InitializeComponent();
        }

    }
}
