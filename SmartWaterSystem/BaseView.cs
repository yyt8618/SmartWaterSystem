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
        #region 变量
        private FrmSystem _MDIView;
        public FrmSystem MDIView
        {
            get { return _MDIView; }
            set { _MDIView = value; }
        }
        #endregion

        public BaseView()
        {
            InitializeComponent();
        }

        #region 公共方法
        protected void DisableRibbonBar()
        {
            this.MDIView.DisableRibbonBar();
        }

        protected void EnableRibbonBar()
        {
            this.MDIView.EnableRibbonBar();
        }

        protected void DisableNavigateBar()
        {
            this.MDIView.DisableNavigateBar();
        }

        protected void EnableNavigateBar()
        {
            this.MDIView.EnableNavigateBar();
        }

        protected void ShowWaitForm(string title, string prompt)
        {
            this.MDIView.ShowWaitForm(title, prompt);
        }

        protected void HideWaitForm()
        {
            this.MDIView.HideWaitForm();
        }

        //protected void ShowWaitForm_Test(string title, string prompt)
        //{
        //    this.MDIView.ShowWaitForm_Test(title, prompt);
        //}

        //protected void HideWaitForm_Test()
        //{
        //    this.MDIView.HideWaitForm_Test();
        //}

        protected void ShowDialog(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            this.MDIView.ShowDialog(text, caption, buttons, icon);
        }

        /// <summary>
        /// 设置状态栏信息
        /// </summary>
        protected void SetStaticItem(string msg)
        {
            this.MDIView.barStaticItemWait.Caption = msg;
        }
        #endregion

        #region 虚方法
        public virtual void OnLoad() { SerialPortEvent(GlobalValue.portUtil.IsOpen); }
        public virtual void SerialPortEvent(bool Enabled) { }
        #endregion
    }
}
