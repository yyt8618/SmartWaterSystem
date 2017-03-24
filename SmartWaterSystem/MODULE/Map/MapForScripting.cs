using System.Security.Permissions;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BLL;
using Entity;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using Common;

namespace SmartWaterSystem
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public class MapForScripting
    {
        public WebBrowser webBrow;
        #region 消防栓
        public void getHydrantMarkers()
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

        public void addHydrantMarker(string id,string addr,string lng,string lat)
        {
            HydrantBLL bll = new HydrantBLL();
            bool result=bll.Insert(id, addr, lng, lat);
            if (!result)
            {
                XtraMessageBox.Show("添加消防栓出现异常!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                webBrow.Document.InvokeScript("reload", null);
            }
        }

        public void delHydrantMarker(string id)
        {
            HydrantBLL bll = new HydrantBLL();
            bool result = bll.Delete(id);
            if (!result)
            {
                XtraMessageBox.Show("删除消防栓出现异常!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                webBrow.Document.InvokeScript("reload", null);
            }
        }

        public void HydrantunAlarm(string id)
        {
            HydrantBLL bll = new HydrantBLL();
            bool result = bll.UnAlarm(id);
            if (!result)
            {
                XtraMessageBox.Show("消防栓取消报警出现异常!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                webBrow.Document.InvokeScript("reload", null);
            }
        }

        public void modifyHydrantCoordinate(string id, string lng, string lat)
        {
            HydrantBLL bll = new HydrantBLL();
            bool result = bll.modifyCoordinate(id,lng,lat);
            if (!result)
            {
                XtraMessageBox.Show("移动消防栓出现异常!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                webBrow.Document.InvokeScript("reload", null);
            }
        }

        public void showHydrantDetail(string id)
        {
            HydrantDetail.HydrantID = id.Trim();
            HydrantDetail detailform = new HydrantDetail();
            detailform.ShowDialog();
        }
        #endregion

        #region 噪声
        public void getNoiseMarkers()
        {
            string str = "";
            if (GlobalValue.recorderList != null && GlobalValue.recorderList.Count > 0)
            {
                //拼凑JSON字符串
                str = "[";
                for (int i = 0; i < GlobalValue.recorderList.Count; i++)
                {
                    if (!string.IsNullOrEmpty(GlobalValue.recorderList[i].Longtitude) && Convert.ToSingle(GlobalValue.recorderList[i].Longtitude) > 0)
                    {
                        string pers = "none"; //"none"不显示  百分比
                        string state = "none";
                        if (GlobalValue.recorderList[i].Result != null)
                        {
                            if (GlobalValue.recorderList[i].Result.IsLeak == 0)
                                state = "normal";
                            else if (GlobalValue.recorderList[i].Result.IsLeak == 1)
                                state = "leak";
                            pers = (GlobalValue.recorderList[i].Result.LeakProbability * 100).ToString("f0") + "%";
                        }
                        str += "{\"id\":\"" + GlobalValue.recorderList[i].ID + "\",\"remark\":\"" + GlobalValue.recorderList[i].Remark +
                            "\",\"lng\":\"" + GlobalValue.recorderList[i].Longtitude + "\",\"lat\":\"" + GlobalValue.recorderList[i].Latitude +
                            "\",\"state\":\"" + state + "\",\"pers\":\"" + pers + "\"}";
                        str += ",";
                    }
                }
                if (str.EndsWith(","))
                    str = str.Substring(0, str.Length - 1);
                str += "]";
            }
            webBrow.Document.InvokeScript("loadMarkers", new object[] { str });
        }

        public void addNoiseMarker(string id, string leakvalue,string standvalue,string remark, string lng, string lat)
        {
            try
            {
                string msg = string.Empty;
                if (!string.IsNullOrEmpty(standvalue))
                {
                    if (!Regex.IsMatch(standvalue, @"^[4-9]\d{2}$"))
                    {
                        XtraMessageBox.Show("请输入有效的启动值[400-999]!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                NoiseRecorder newRec = new NoiseRecorder();
                newRec.ID = Convert.ToInt32(id);
                newRec.AddDate = DateTime.Now;
                newRec.LeakValue = Convert.ToInt32(leakvalue);
                newRec.Remark = remark;
                newRec.Longtitude = lng;
                newRec.Latitude = lat;
                
                newRec.PickSpan = Settings.Instance.GetInt(SettingKeys.Span_Template);
                newRec.RecordTime = Settings.Instance.GetInt(SettingKeys.RecTime_Template);

                bool exist = false;
                for(int i = 0; i < GlobalValue.recorderList.Count;i++)
                {
                    if(GlobalValue.recorderList[i].ID.ToString() == id )
                    {
                        exist = true;
                        break;
                    }
                }
                int query = -1;
                if (exist)
                    query =NoiseDataBaseHelper.UpdateRecorder(newRec);
                else
                    query = NoiseDataBaseHelper.AddRecorder(newRec);
                if (query != -1)
                {
                    GlobalValue.recorderList = NoiseDataBaseHelper.GetRecorders();
                }
                else
                    throw new Exception("数据入库时发生错误。");

                if (!string.IsNullOrEmpty(standvalue))
                {
                    int singledata = Convert.ToInt32(standvalue);
                    if (NoiseDataBaseHelper.SaveStandData(newRec.ID, singledata) < 0)
                    {
                        XtraMessageBox.Show("保存记录仪数据失败!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("添加失败：" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void delNoiseMarker(string id)
        {
            try
            {
                int RecId = Convert.ToInt32(id);
                NoiseDataBaseHelper.DeleteRecorder(RecId);
                if (NoiseDataBaseHelper.DeleteStandData(RecId) < 0)
                {
                    XtraMessageBox.Show("删除记录仪数据失败!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                GlobalValue.recorderList = NoiseDataBaseHelper.GetRecorders();
                GlobalValue.groupList = NoiseDataBaseHelper.GetGroups();
                GlobalValue.reReadIdList.Clear();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("删除失败：" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void NoiseunAlarm(string id)
        {
        }

        public void modifyNoiseCoordinate(string id, string lng, string lat)
        {
        }

        public void showNoiseDetail(string id)
        {
        }
        #endregion

    }
}
