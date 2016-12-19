using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.IO;
using Common;
using DevExpress.XtraEditors;
using Entity;
using System.Collections;

namespace SmartWaterSystem
{
    public partial class FrmColorSet : DevExpress.XtraEditors.XtraForm
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("FrmColorSet");

        public FrmColorSet()
        {
            InitializeComponent();
        }

        private void FrmColorSet_Load(object sender, EventArgs e)
        {
            BindTree();
        }
        
        private void BindTree()
        {
            try
            {
                //32, 31, 53
                //获取颜色配置
                Hashtable ht = new MsgColorHelper().GetColorConfig(GlobalValue.ColorConfigFilePath);
                //KeyField小于0都是带有孩子的
                DataTable dt = new DataTable();
                dt.Columns.Add("KeyField");
                dt.Columns.Add("ParentField");
                dt.Columns.Add("Type");
                dt.Columns.Add("Color");

                DataRow dr = dt.NewRow();
                dr["KeyField"] = -99;
                dr["Type"] = "全部";
                dr["Color"] = DBNull.Value;
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["KeyField"] = -1;
                dr["ParentField"] = -99;
                dr["Type"] = "远传";
                dr["Color"] = DBNull.Value;
                dt.Rows.Add(dr);

                EnumHelper enumhelper = new EnumHelper();
                foreach (var value in Enum.GetValues(typeof(ColorType)))
                {
                    int ival = Convert.ToInt32(value);
                    int argbcolor = 0;
                    int backargbcolor = Color.FromArgb(32, 31, 53).ToArgb();
                    //ht[ival];
                    if (ht[ival] != null)
                    {
                        argbcolor = Convert.ToInt32(ht[ival]);
                    }
                    else
                        argbcolor = Color.Lime.ToArgb();            //默认颜色
                    if (ival < 40)
                    {
                        dr = dt.NewRow();
                        dr["KeyField"] = ival;
                        dr["ParentField"] = -99;
                        dr["Type"] = enumhelper.GetEnumDescription((ColorType)value);
                        if ((ColorType)value == ColorType.BackColor)
                            dr["Color"] = backargbcolor;
                        else
                            dr["Color"] = argbcolor;
                        dt.Rows.Add(dr);
                    }
                    else if (ival >= 40)
                    {
                        dr = dt.NewRow();
                        dr["KeyField"] = ival;
                        dr["ParentField"] = -1;
                        dr["Type"] = enumhelper.GetEnumDescription((ColorType)value);
                        dr["Color"] = argbcolor;
                        dt.Rows.Add(dr);
                    }
                }

                treeColor.DataSource = dt;

                treeColor.ParentFieldName = "ParentField";
                treeColor.KeyFieldName = "KeyField";
                treeColor.ExpandAll();
            }
            catch (Exception ex)
            {
                logger.ErrorException("BindTree", ex);
                XtraMessageBox.Show("初始化页面出现异常！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = treeColor.DataSource as DataTable;
                using (StreamWriter writer = new StreamWriter(GlobalValue.ColorConfigFilePath, false, Encoding.UTF8))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToInt32(dr["KeyField"]) > 0)
                        {
                            writer.WriteLine(dr["KeyField"].ToString().Replace('\t', ' ') + "\t" + dr["Color"].ToString());
                        }
                    }
                    writer.Flush();
                }
                if (GlobalValue.MainForm.Gprsconsole != null)
                    GlobalValue.MainForm.Gprsconsole.UpdateColorConfig();  //更新FrmConsole界面上的颜色配置

                XtraMessageBox.Show("设置成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("保存时发生异常!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void treeColor_CustomUnboundColumnData(object sender, DevExpress.XtraTreeList.TreeListCustomColumnDataEventArgs e)
        {
            if (e.Column == treeListColumn3)
            {
                if (e.IsSetData)
                {
                    var color = (Color)e.Value;
                    (e.Row as DataRowView)["Color"] = color.ToArgb().ToString();

                    int key = Convert.ToInt32((e.Row as DataRowView)["KeyField"]);
                    if (key < 0)
                    {
                        ChangeChildColor(key, color);
                    }
                }
                if (e.IsGetData)
                {
                    var aVal = (e.Row as DataRowView)["Color"].ToString();
                    int i;
                    if (int.TryParse(aVal, out i))
                    {
                        var bVal = (e.Row as DataRowView)["Color"];
                        var color = Color.FromArgb(Convert.ToInt32(bVal));
                        e.Value = color;
                    }
                }
            }
        }

        /// <summary>
        /// 修改父节点后，将该父节点下的所有子节点都修改成父节点颜色
        /// </summary>
        private void ChangeChildColor(int Parentkey,Color color)
        {
            if (Parentkey < 0)      //小于0 表示还有孩子节点
            {
                DataTable dt = treeColor.DataSource as DataTable;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["ParentField"].ToString() == Parentkey.ToString())
                    {
                        dr["Color"] = color.ToArgb().ToString();
                        if (Convert.ToInt32(dr["KeyField"]) < 0)
                            ChangeChildColor(Convert.ToInt32(dr["KeyField"]), color);
                    }
                }
            }
        }

    }
}
