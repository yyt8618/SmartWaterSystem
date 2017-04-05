using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraEditors;
using ChartDirector;
using System.Data;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList;
using System.Text.RegularExpressions;

namespace SmartWaterSystem
{
    public partial class NoiseMap : BaseView, INoiseMap
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("NoiseMap");
        MapForScripting mapscripting = new MapForScripting();
        public NoiseMap()
        {
            InitializeComponent();

            try
            {
                Uri uri = new Uri(Path.Combine(Application.StartupPath, ".\\NoiseMap\\NoiseBaiduMap.htm"));
                webBrowser1.Url = uri;
                mapscripting.webBrow = this.webBrowser1;
                webBrowser1.ObjectForScripting = mapscripting;
            }
            catch (Exception ex)
            {
                logger.ErrorException("NoiseMap_Load", ex);
                XtraMessageBox.Show("加载地图出现异常!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override void OnLoad()
        {
            BindTree();
            mapscripting.UpdateBindEvent += new MapForScripting.UpdateBindHandle(UpdateBind);
        }

        public void UpdateBind()
        {
            BindTree();
        }

        MapFullScreen frm = null;
        private void btnFullScreen_Click(object sender, EventArgs e)
        {
            frm = new MapFullScreen();
            AddEventKeyUp(this.webBrowser1);
            this.Controls.Remove(this.webBrowser1);
            this.Controls.Remove(this.btnFullScreen);
            //this.Controls.Clear();
            frm.Controls.Add(this.webBrowser1);
            frm.ShowDialog();
        }

        private void AddEventKeyUp(Control control)
        {
            try
            {
                if (control != null)
                {
                    control.KeyUp -= new KeyEventHandler(control_KeyUp);
                    control.KeyUp += new KeyEventHandler(control_KeyUp);
                    foreach (Control c in control.Controls)
                    {
                        AddEventKeyUp(c);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void control_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                try
                {
                    if (this.webBrowser1 != null)
                    {
                        if (frm != null)
                        {
                            frm.Controls.Clear();
                            this.Controls.Add(this.webBrowser1);
                            //this.Controls.Add(this.dockPanel1);
                            frm.Close();
                            frm = null;
                        }
                    }
                    this.Controls.Add(this.btnFullScreen);
                    this.btnFullScreen.Click -= new EventHandler(btnFullScreen_Click);
                    this.btnFullScreen.Click += new EventHandler(btnFullScreen_Click);
                    webBrowser1.SendToBack();
                    btnFullScreen.BringToFront();
                    btnFullScreen.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// 绑定树形列表
        /// </summary>
        public void BindTree()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("KeyFieldName");
                dt.Columns.Add("ParentFieldName");
                dt.Columns.Add("ID");
                dt.Columns.Add("Remark");
                dt.Columns.Add("Name");

                int pFlag = 0;
                int cFlag = 0;
                int tFlag = 0;
                for (int i = 0; i < GlobalValue.groupList.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["KeyFieldName"] = tFlag;
                    dr["ParentFieldName"] = DBNull.Value;
                    dr["ID"] = GlobalValue.groupList[i].Name;  //注意这里与Name字段是相反的,主要是显示的时候用名称,子节点用终端号
                    dr["Remark"] = GlobalValue.groupList[i].Remark;
                    dr["Name"] = GlobalValue.groupList[i].ID;
                    cFlag = tFlag + 1;
                    pFlag = tFlag;
                    dt.Rows.Add(dr);
                    for (int j = 0; j < GlobalValue.groupList[i].RecorderList.Count; j++)
                    {
                        if (string.IsNullOrEmpty(GlobalValue.groupList[i].RecorderList[j].Longtitude) || Convert.ToSingle(GlobalValue.groupList[i].RecorderList[j].Longtitude) <= 0)
                        {
                            DataRow dr1 = dt.NewRow();
                            dr1["KeyFieldName"] = cFlag;
                            dr1["ParentFieldName"] = pFlag;
                            dr1["ID"] = GlobalValue.groupList[i].RecorderList[j].ID;
                            dr1["Remark"] = GlobalValue.groupList[i].RecorderList[j].Remark;
                            dt.Rows.Add(dr1);
                            cFlag++;
                            tFlag++;
                        }
                    }
                    pFlag++;
                    tFlag++;
                }

                treeList1.DataSource = dt;
                treeList1.ParentFieldName = "ParentFieldName";
                treeList1.KeyFieldName = "KeyFieldName";

                treeList1.ExpandAll();
            }
            catch (Exception ex)
            {
                logger.ErrorException("BindTree", ex);
                XtraMessageBox.Show("初始化页面出现异常！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void treeList1_Click(object sender, EventArgs e)
        {
            MouseEventArgs mousee = (MouseEventArgs)e;
            TreeList list = (TreeList)sender;
            TreeListHitInfo info= list.CalcHitInfo(new Point(mousee.X, mousee.Y));
            if (info.Node == null) return;
            object obj_id =info.Node.GetValue(1);
            if (Regex.IsMatch(obj_id.ToString(), "^\\d{1,3}$"))
                mapscripting.setcusoricon(Convert.ToInt32(obj_id), true);
        }

    }
}
