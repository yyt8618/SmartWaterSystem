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
    public partial class BaseView : DevExpress.XtraEditors.XtraUserControl
    {

        public BaseView()
        {
            InitializeComponent();
        }

        public void BeginSerialPortDelegate()
        {
            GlobalValue.SerialPortMgr.SerialPortEvent += new SerialPortHandle(SerialPortNotify);
        }

        protected void SerialPortNotify(object sender, SerialPortEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new SerialPortHandle(OnSerialPortNotify), new object[2] { sender, e });
            }
            else
            {
                OnSerialPortNotify(sender, e);
            }
        }

        public virtual void OnSerialPortNotify(object sender, SerialPortEventArgs e)
        {
            if (e.TransactStatus != TransStatus.Start)
            {
                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);
                HideWaitForm();
                this.Enabled = true;
            }
        }

        #region 公共方法
        protected void DisableRibbonBar()
        {
            GlobalValue.MainForm.DisableRibbonBar();
        }

        protected void EnableRibbonBar()
        {
            GlobalValue.MainForm.EnableRibbonBar();
        }

        protected void DisableNavigateBar()
        {
            GlobalValue.MainForm.DisableNavigateBar();
        }

        protected void EnableNavigateBar()
        {
            GlobalValue.MainForm.EnableNavigateBar();
        }

        protected void ShowWaitForm(string title, string prompt)
        {
            GlobalValue.MainForm.ShowWaitForm(title, prompt);
        }

        protected void HideWaitForm()
        {
            GlobalValue.MainForm.HideWaitForm();
        }

        protected void ShowDialog(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            GlobalValue.MainForm.ShowDialog(text, caption, buttons, icon);
        }

        /// <summary>
        /// 设置状态栏信息
        /// </summary>
        protected void SetStaticItem(string msg)
        {
            GlobalValue.MainForm.barStaticItemWait.Caption = msg;
        }
        #endregion

        #region 虚方法
        public virtual void OnLoad() { SerialPortEvent(GlobalValue.portUtil.IsOpen); }
        public virtual void SerialPortEvent(bool Enabled) { }
        #endregion

    }
}
