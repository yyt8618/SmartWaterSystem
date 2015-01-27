using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;

namespace NoiseAnalysisSystem
{
    public partial class WelcomSplash : SplashScreen
    {
        public WelcomSplash()
        {
            InitializeComponent();
        }

        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            lblPrompt.Text = arg.ToString();
        }

        #endregion

        
    }
}