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

namespace SmartWaterSystem
{
    public partial class FrmSocketSet : DevExpress.XtraEditors.XtraForm
    {
        string configfilepath = Application.StartupPath + @"\SocketAddr.txt";
        DataTable dt = new DataTable("SocketAddr");

        public FrmSocketSet()
        {
            InitializeComponent();
        }

        private void FrmMSMQSet_Load(object sender, EventArgs e)
        {
            dt.Columns.Add("Selected", typeof(Boolean));
            dt.Columns.Add("Name");
            dt.Columns.Add("IP");
            dt.Columns.Add("Port");

            try {
                if (File.Exists(configfilepath))
                {
                    string selectedrow = Settings.Instance.GetString(SettingKeys.GPRS_IP) + "\t" + Settings.Instance.GetString(SettingKeys.GPRS_PORT);
                    using (StreamReader reader = new StreamReader(configfilepath, Encoding.UTF8))
                    {
                        do
                        {
                            string strrow = reader.ReadLine();
                            if (!string.IsNullOrEmpty(strrow))
                            {
                                string[] strcols = strrow.Split('\t');
                                if (strcols != null && strcols.Length == 3)
                                {
                                    DataRow dr = dt.NewRow();
                                    dr["Name"] = strcols[0];
                                    dr["IP"] = strcols[1];
                                    dr["Port"] = strcols[2];
                                    if (strrow.Contains(selectedrow))
                                        dr["Selected"] = true;
                                    dt.Rows.Add(dr);
                                }
                            }

                        } while (!reader.EndOfStream);
                    }
                }
                gridControl1.DataSource = dt;
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show("初始化失败!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try {
                DataTable dt = gridControl1.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0)
                {
                    XtraMessageBox.Show("请输入至少一条数据!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int selectedcount = 0;
                string checkedip = "", checkedport = "";  //获取选择的地址
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Selected"].ToString() == "True")
                    {
                        selectedcount++;
                        checkedip = dr["IP"].ToString();
                        checkedport = dr["Port"].ToString();
                    }
                }
                if (selectedcount==0)
                {
                    XtraMessageBox.Show("请至少选择一条数据!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (selectedcount>1)
                {
                    XtraMessageBox.Show("请选择一条数据!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (StreamWriter writer = new StreamWriter(configfilepath, false, Encoding.UTF8))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if(dr["Name"]!=DBNull.Value && !string.IsNullOrEmpty(dr["Name"].ToString()))
                            writer.WriteLine(dr["Name"].ToString().Replace('\t', ' ') + "\t" + dr["IP"].ToString() + "\t" + dr["Port"].ToString());
                    }
                    writer.Flush();
                }

                Settings.Instance.SetValue(SettingKeys.GPRS_IP, checkedip);
                Settings.Instance.SetValue(SettingKeys.GPRS_PORT, checkedport);

                XtraMessageBox.Show("设置成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show("保存时发生异常!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
