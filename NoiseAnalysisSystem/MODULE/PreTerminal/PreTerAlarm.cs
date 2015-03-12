using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SmartWaterSystem
{
    public partial class PreTerAlarm : BaseView,IPreTerAlarm
    {
        public PreTerAlarm(FrmSystem parentform)
        {
            InitializeComponent();
        }
    }
}
