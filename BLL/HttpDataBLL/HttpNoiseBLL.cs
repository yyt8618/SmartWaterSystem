using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using Entity;
using System.Text.RegularExpressions;

namespace BLL
{
    public class HttpNoiseBLL
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("HttpDataNoiseBLL");
        HttpNoiseDAL dal = new HttpNoiseDAL();
        HydrantBLL Hybll = new HydrantBLL();

        public GetGroupsRespEntity GetGroupsInfo()
        {
            GetGroupsRespEntity resp = new GetGroupsRespEntity();
            resp.code = HttpRespCode.Success;
            resp.msg = "";
            try
            {
                dal.GetGroupsInfo(out resp.groupsdata, out resp.tersdata);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetGroupsInfo", ex);
                resp.code = HttpRespCode.Excp;
                resp.msg = "服务器异常";
                resp.groupsdata = null;
                resp.tersdata = null;
            }
            return resp;
        }

        public HTTPRespEntity UploadGroups(List<UpLoadNoiseDataEntity> lstNoiseData)
        {
            HTTPRespEntity resp = new HTTPRespEntity();
            resp.code = HttpRespCode.Success;
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
                                short shdata = 0;
                                if (short.TryParse(strdata, out shdata) && shdata > 0)
                                {
                                    lstdata.Add(shdata);
                                }
                            }
                        }

                        short terid = Convert.ToInt16(dataentity.TerId.Trim());
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
                            resp.code = HttpRespCode.Fail;
                            resp.msg = errmsg;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("UploadGroups", ex);
                resp.code = HttpRespCode.Excp;
                resp.msg = "服务器异常";
            }
            return resp;
        }

        #region 消防栓
        public GetHydrantRespEntity GetHydrants()
        {
            GetHydrantRespEntity resp = new GetHydrantRespEntity();
            resp.code = HttpRespCode.Success;
            resp.msg = "";
            try
            {
                resp.lstHydrant=Hybll.SelectAll();
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetHydrants", ex);
                resp.code = HttpRespCode.Excp;
                resp.msg = "服务器异常";
                resp.lstHydrant = null;
            }
            return resp;
        }

        public HTTPRespEntity SaveHydrantInfo(SaveHydrantReqEntity data)
        {
            HTTPRespEntity resp = new HTTPRespEntity();
            resp.code = HttpRespCode.Success;
            resp.msg = "";
            try
            {
                if(string.IsNullOrEmpty(data.HydrantID))
                {
                    resp.code = HttpRespCode.Fail;
                    resp.msg = "终端ID不能为空";
                }
                if (Regex.IsMatch(data.HydrantID,"^\\d{1,8$"))
                {
                    resp.code = HttpRespCode.Fail;
                    resp.msg = "终端ID不合法";
                }
                if (Hybll.HydrantExist(data.HydrantID))  //update
                {
                     if(!Hybll.Update(data.HydrantID, data.Addr, data.Longtitude, data.Latitude, data.Remark))
                    {
                        resp.code = HttpRespCode.Fail;
                        resp.msg = "保存发生异常!";
                    }
                }
                else
                {
                    if(!Hybll.Insert(data.HydrantID, data.Addr, data.Longtitude, data.Latitude, data.Remark))
                    {
                        resp.code = HttpRespCode.Fail;
                        resp.msg = "保存发生异常!";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("SaveHydrantInfo", ex);
                resp.code = HttpRespCode.Excp;
                resp.msg = "服务器异常";
            }
            return resp;
        }

        public HTTPRespEntity DelHydrant(string id)
        {
            HTTPRespEntity resp = new HTTPRespEntity();
            resp.code = HttpRespCode.Success;
            resp.msg = "";
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    resp.code = HttpRespCode.Fail;
                    resp.msg = "终端ID不能为空";
                }
                if (Regex.IsMatch(id, "^\\d{1,8$"))
                {
                    resp.code = HttpRespCode.Fail;
                    resp.msg = "终端ID不合法";
                }
                if (!Hybll.Delete(id))
                {
                    resp.code = HttpRespCode.Fail;
                    resp.msg = "删除失败,服务器异常";
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("SaveHydrantInfo", ex);
                resp.code = HttpRespCode.Excp;
                resp.msg = "服务器异常";
            }
            return resp;
        }

        public HTTPRespEntity ModifyHydrantCoordinate(ModifyHyCoordReqEntity data)
        {
            HTTPRespEntity resp = new HTTPRespEntity();
            resp.code = HttpRespCode.Success;
            resp.msg = "";
            try
            {
                if (string.IsNullOrEmpty(data.HydrantID))
                {
                    resp.code = HttpRespCode.Fail;
                    resp.msg = "终端ID不能为空";
                }
                if (Regex.IsMatch(data.HydrantID, "^\\d{1,8$"))
                {
                    resp.code = HttpRespCode.Fail;
                    resp.msg = "终端ID不合法";
                }
                if (!Hybll.modifyCoordinate(data.HydrantID,data.Longtitude,data.Latitude))
                {
                    resp.code = HttpRespCode.Fail;
                    resp.msg = "修改失败,服务器异常";
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("ModifyHydrantCoordinate", ex);
                resp.code = HttpRespCode.Excp;
                resp.msg = "服务器异常";
            }
            return resp;
        }

        public GetHydrantDetailRespEntity GetHydrantDetail(HyrdrantDetailReqEntity data)
        {
            GetHydrantDetailRespEntity resp = new GetHydrantDetailRespEntity();
            resp.code = HttpRespCode.Success;
            resp.msg = "";
            try
            {
                if (string.IsNullOrEmpty(data.id))
                {
                    resp.code = HttpRespCode.Fail;
                    resp.msg = "终端ID不能为空";
                }
                if (Regex.IsMatch(data.id, "^\\d{1,8$"))
                {
                    resp.code = HttpRespCode.Fail;
                    resp.msg = "终端ID不合法";
                }
                if(string.IsNullOrEmpty(data.mintime))
                {
                    resp.code = HttpRespCode.Fail;
                    resp.msg = "请输入起始时间";
                }
                DateTime minTime = Convert.ToDateTime(data.mintime);
                DateTime maxTime = DateTime.Now;
                if(!string.IsNullOrEmpty(data.maxtime))
                {
                    maxTime=Convert.ToDateTime(data.maxtime);
                }
                if(((maxTime - minTime)).TotalDays >30)
                {
                    resp.code = HttpRespCode.Fail;
                    resp.msg = "查询区间不能超过30天";
                }
                resp.lstData= Hybll.GetHydrantDetail(data.id, data.opt, minTime, maxTime, data.interval);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetHydrantDetail", ex);
                resp.code = HttpRespCode.Excp;
                resp.msg = "服务器异常";
            }
            return resp;
        }
        #endregion
    }
}
