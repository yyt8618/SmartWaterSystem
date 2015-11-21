using System.Security.Permissions;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BLL;
using Entity;
using System.Collections.Generic;

namespace SmartWaterSystem
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public class MapForScripting
    {
        public WebBrowser webBrow;
        public void getMarkers()
        {
            HydrantBLL bll = new HydrantBLL();
            List<HydrantEntity> lstHydrant = bll.SelectAll();
            string str = "";
            if (lstHydrant != null && lstHydrant.Count > 0)
            {
                //拼凑JSON字符串
                str = "[";
                for (int i = 0; i < lstHydrant.Count; i++)
                {
                    string pressvalue = "none"; //"none"不显示
                    string openangle = "none";
                    if (lstHydrant[i].OptType == HydrantOptType.Open)
                    {
                        pressvalue = lstHydrant[i].PressValue.ToString();
                        openangle = lstHydrant[i].OpenAngle.ToString();
                    }
                    if (lstHydrant[i].OptType == HydrantOptType.OpenAngle)
                    {
                        openangle = lstHydrant[i].OpenAngle.ToString();
                    }
                    str += "{\"id\":\"" + lstHydrant[i].HydrantID + "\",\"addr\":\"" + lstHydrant[i].Addr +
                        "\",\"lng\":\"" + lstHydrant[i].Longtitude + "\",\"lat\":\"" + lstHydrant[i].Latitude +
                        "\",\"state\":\"" + (lstHydrant[i].IsAlarm ? lstHydrant[i].OptType.ToString().ToLower() : HydrantOptType.Close.ToString().ToLower()) +
                        "\",\"pressvalue\":\"" + pressvalue + "\",\"openangle\":\"" + openangle + "\"}";
                    if ((i + 1) < lstHydrant.Count)
                        str += ",";
                }
                str += "]";
            }
            webBrow.Document.InvokeScript("loadMarkers", new object[] { str });
        }

        public void addMarker(string id,string addr,string lng,string lat)
        {
            HydrantBLL bll = new HydrantBLL();
            bool result=bll.Insert(id, addr, lng, lat);
            if (!result)
            {
                XtraMessageBox.Show("添加消防栓出现异常!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                webBrow.Document.InvokeScript("reload", null);
            }
        }

        public void delMarker(string id)
        {
            HydrantBLL bll = new HydrantBLL();
            bool result = bll.Delete(id);
            if (!result)
            {
                XtraMessageBox.Show("删除消防栓出现异常!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                webBrow.Document.InvokeScript("reload", null);
            }
        }

        public void unAlarm(string id)
        {
            HydrantBLL bll = new HydrantBLL();
            bool result = bll.UnAlarm(id);
            if (!result)
            {
                XtraMessageBox.Show("消防栓取消报警出现异常!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                webBrow.Document.InvokeScript("reload", null);
            }
        }

        public void modifyCoordinate(string id, string lng, string lat)
        {
            HydrantBLL bll = new HydrantBLL();
            bool result = bll.modifyCoordinate(id,lng,lat);
            if (!result)
            {
                XtraMessageBox.Show("移动消防栓出现异常!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                webBrow.Document.InvokeScript("reload", null);
            }
        }

        public void showDetail(string id)
        {
            HydrantDetail.HydrantID = id.Trim();
            HydrantDetail detailform = new HydrantDetail();
            detailform.ShowDialog();
        }
    }
}
