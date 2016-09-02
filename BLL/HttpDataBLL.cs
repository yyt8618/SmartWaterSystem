using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using Entity;

namespace BLL
{
    public class HttpDataBLL
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("HttpDataBLL");
        HttpDataDAL dal = new HttpDataDAL();

        public GetGroupsRespEntity GetGroupsInfo()
        {
            GetGroupsRespEntity resp = new GetGroupsRespEntity();
            resp.code = 1;
            resp.msg = "";
            try
            {
                dal.GetGroupsInfo(out resp.groupsdata, out resp.tersdata);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetGroupsInfo", ex);
                resp.code = -1;
                resp.msg = "服务器异常";
                resp.groupsdata = null;
                resp.tersdata = null;
            }
            return resp;
        }

        public HTTPRespEntity UploadGroups(List<UpLoadNoiseDataEntity> lstNoiseData)
        {
            HTTPRespEntity resp = new HTTPRespEntity();
            resp.code = 1;
            resp.msg = "";
            try
            {
                if (lstNoiseData != null && lstNoiseData.Count > 0)
                {
                    Dictionary<short, short[]> result = new Dictionary<short, short[]>();
                    List<NoiseRecorder> recorderSelect = new List<NoiseRecorder>();
                    List<NoiseRecorder> lstRecorder = new List<NoiseRecorder>();
                    foreach (UpLoadNoiseDataEntity dataentity in lstNoiseData)
                    {
                        List<short> lstdata = new List<short>();
                        string[] strdatas = dataentity.Data.Split(',');
                        if (strdatas != null && strdatas.Length > 0)
                        {
                            foreach (string strdata in strdatas)
                            {
                                short shdata  =0;
                                if (short.TryParse(strdata, out shdata) && shdata>0)
                                {
                                    lstdata.Add(shdata);
                                }
                            }
                        }


                        short terid=Convert.ToInt16(dataentity.TerId.Trim());
                        if (string.IsNullOrEmpty(dataentity.GroupId))  //如果组ID为空，则补全(GPRS远传的数据没有组ID信息)
                        {
                            dataentity.GroupId = NoiseDataBaseHelper.GetGroupIdByRec(dataentity.TerId).ToString();
                        }
                        if (lstdata.Count > 0 && !string.IsNullOrEmpty(dataentity.GroupId))
                        {
                            NoiseDataBaseHelper.SaveStandData(dataentity.GroupId, dataentity.TerId, dataentity.cali);

                            result.Add(terid, lstdata.ToArray());

                            NoiseRecorder recorder = new NoiseRecorder();
                            recorder.ID = terid;
                            if (!string.IsNullOrEmpty(dataentity.GroupId))
                                recorder.GroupID = Convert.ToInt32(dataentity.GroupId);
                            recorderSelect.Add(recorder);

                            NoiseRecorder recorder1 = new NoiseRecorder();
                            recorder1.ID = terid;
                            if (!string.IsNullOrEmpty(dataentity.GroupId))
                                recorder1.GroupID = Convert.ToInt32(dataentity.GroupId);
                            lstRecorder.Add(recorder1);

                        }
                    }
                    
                    if (recorderSelect != null && recorderSelect.Count > 0)
                    {
                        string errmsg = NoiseDataHandler.CallbackReaded(result, recorderSelect, "", ref lstRecorder);
                        if (string.IsNullOrEmpty(errmsg))
                        {
                            foreach (NoiseRecorder rec in lstRecorder)
                            {
                                NoiseDataBaseHelper.AddNoiseData(rec.Data);
                                NoiseDataBaseHelper.AddNoiseResult(rec.Result);
                            }
                        }
                        else
                        {
                            resp.code = -1;
                            resp.msg = errmsg;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetGroupsInfo", ex);
                resp.code = -1;
                resp.msg = "服务器异常";
            }
            return resp;
        }
    }
}
