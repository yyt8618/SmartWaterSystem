using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraWaitForm;

namespace SmartWaterSystem
{
    public partial class WaitForm1 : WaitForm
    {
        public WaitForm1()
        {
            InitializeComponent();
            this.progressPanel1.AutoHeight = true;
        }

        #region Overrides

        public override void SetCaption(string caption)
        {
            base.SetCaption(caption);
            this.progressPanel1.Caption = caption;
        }
        public override void SetDescription(string description)
        {
            base.SetDescription(description);
            this.progressPanel1.Description = description;
        }
        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        public void ShowProgress(string caption,string description)
        {
            this.progressPanel1.Caption = caption;
            this.progressPanel1.Description = description;

            if (!this.Visible)
            {
                this.Show();
            }
        }

        public void HideProgress()
        {
            this.Hide();
        }
        #endregion

        public enum WaitFormCommand
        {
        }

        private void WaitForm1_Deactivate(object sender, EventArgs e)
        {
            this.Activate();
        }
    }
}