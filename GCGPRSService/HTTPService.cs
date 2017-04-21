using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using Common;
using Entity;
using BLL;
using SmartWaterSystem;

namespace GCGPRSService
{
    public delegate void HTTPReceiveMessage(HTTPMsgEventArgs e);
    public class HTTPMsgEventArgs : EventArgs
    {
        private string _msg = "";
        public string Msg
        {
            get { return _msg; }
            set { _msg = value; }
        }

        public HTTPMsgEventArgs()
        {
        }

        public HTTPMsgEventArgs(string msg)
        {
            this._msg = msg;
        }
    }

    public class HTTPService
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("HTTPService");
        //private bool isrun = false;
        HttpListener listener = new HttpListener();
        HttpNoiseBLL noisebll = new HttpNoiseBLL();
        HttpTerMagBLL termagbll = new HttpTerMagBLL();

        //本地临时图片存放文件夹
        private string PicLocalTmpDir = Settings.Instance.GetString(SettingKeys.PicLocalTmpDir);
        //本地正式图片存放文件夹
        private string PicLocalDir = Settings.Instance.GetString(SettingKeys.PicLocalDir);
        //网络临时图片头
        private string PicNetTmpDirHead = Settings.Instance.GetString(SettingKeys.PicNetTmpDirHead);
        //网络正式图片头
        private string PicNetDirHead = Settings.Instance.GetString(SettingKeys.PicNetDirHead);

        public event HTTPReceiveMessage HTTPMessageEvent;

        public void Start()
        {
            if (string.IsNullOrEmpty(Settings.Instance.GetString(SettingKeys.HTTPServiceURL)))
            {
                GlobalValue.Instance.lstStartRecord.Add(DateTime.Now.ToString() + " 启动HTTP服务失败,URL为空");
                return;
            }
            if (string.IsNullOrEmpty(Settings.Instance.GetString(SettingKeys.HTTPMD5Key)))
            {
                GlobalValue.Instance.lstStartRecord.Add(DateTime.Now.ToString() + " 启动HTTP服务失败,MD5 Key为空");
                return;
            }
            listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            listener.Prefixes.Add(Settings.Instance.GetString(SettingKeys.HTTPServiceURL));
            GlobalValue.Instance.lstStartRecord.Add(DateTime.Now.ToString() + " 启动HTTP服务,URL:" + Settings.Instance.GetString(SettingKeys.HTTPServiceURL));
            //isrun = true;
            //Thread t = new Thread(new ThreadStart(ServiceThread));
            listener.Start();
            listener.BeginGetContext(new AsyncCallback(GetContextCallback), listener);
            //t.Start();
            GlobalValue.Instance.lstStartRecord.Add(DateTime.Now.ToString() + " 开启HTTP接收服务完成!");
        }

        public void Stop()
        {
            //isrun = false;
            listener.Close();

            GlobalValue.Instance.lstStartRecord.Add("停止HTTP服务...");
        }

        public virtual void OnReceiveMsg(string str)
        {
            if (HTTPMessageEvent != null)
                HTTPMessageEvent(new HTTPMsgEventArgs(str));
        }

        private void GetContextCallback(IAsyncResult ar)
        {
            try
            {
                listener = ar.AsyncState as HttpListener;
                HttpListenerContext context = listener.EndGetContext(ar);
                listener.BeginGetContext(new AsyncCallback(GetContextCallback), listener);

                context.Response.StatusCode = 200;

                string str_resp_err = "";
                string str_resp = "";
                byte[] buffer = new byte[1024];

                #region test
                //UploadFlowDataReqEntity testentity = new UploadFlowDataReqEntity();
                //testentity.action = "uploadflowdata";
                //testentity.TerData = new List<UpLoadFlowDataEntity>();
                //UpLoadFlowDataEntity testdata1 = new UpLoadFlowDataEntity();
                //testdata1.terid = "1";
                //testdata1.flowvalue = "100.123";
                //testdata1.flowinverted = "2344.0";
                //testdata1.flowinstant = "233.23";
                //testdata1.collTime = DateTime.Now.ToString();
                //testentity.TerData.Add(testdata1);
                //string strttt = SmartWaterSystem.JSONSerialize.JsonSerialize<UploadFlowDataReqEntity>(testentity);
                //long timestamp = 0;
                //TimeSpan tsp = (TimeZone.CurrentTimeZone.ToLocalTime(DateTime.Now) - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)));
                //timestamp = (long)tsp.TotalMilliseconds;
                //string md51 = MD5Encrypt.MD5(System.Web.HttpUtility.UrlEncode(strttt + timestamp + Settings.Instance.GetString(SettingKeys.HTTPMD5Key)).ToLower());
                //HTTPEntity ttpentity = new HTTPEntity();
                //ttpentity.timestamp = timestamp.ToString();
                //ttpentity.Params = strttt;
                //ttpentity.digest = md51;
                //string reqtemp = SmartWaterSystem.JSONSerialize.JsonSerialize<HTTPEntity>(ttpentity);
                //string urltemp = System.Web.HttpUtility.UrlEncode(reqtemp, Encoding.UTF8);

                #endregion

                if (context.Request.HttpMethod.ToLower().Equals("get"))
                {
                    //GET请求处理
                    str_resp_err = "不支持GET方法";
                }
                else if (context.Request.HttpMethod.ToLower().Equals("post"))
                {
                    //这是在POST请求时必须传参的判断默认注释掉
                    if (!context.Request.HasEntityBody)
                    {
                        str_resp_err = "请传入参数";
                    }
                    else
                    {
                        //POST请求处理
                        Stream SourceStream = context.Request.InputStream;
                        int readcount = -1;

                        List<byte> lstbytes = new List<byte>();
                        while ((readcount = SourceStream.Read(buffer, 0, 1024)) > 0)
                        {
                            for (int i = 0; i < readcount; i++)
                            {
                                lstbytes.Add(buffer[i]);
                            }
                        }
                        string strrequest = Encoding.UTF8.GetString(lstbytes.ToArray());
                        try
                        {
                            strrequest = System.Web.HttpUtility.UrlDecode(strrequest, Encoding.UTF8); //UrlDecode
                            OnReceiveMsg("接收到请求[" + DateTime.Now.ToString() + "]:" + strrequest);
                            HTTPEntity httpentity = JSONSerialize.JsonDeserialize_Newtonsoft<HTTPEntity>(strrequest);  //jsondeSerialize

                            if (httpentity == null)
                            {
                                str_resp_err = "无效数据,解析失败!";
                                goto err;
                            }
                            if (string.IsNullOrEmpty(httpentity.Params))
                            {
                                str_resp_err = "无效params,解析失败!";
                                goto err;
                            }
                            if (string.IsNullOrEmpty(httpentity.timestamp))
                            {
                                str_resp_err = "时间戳不能为空!";
                                goto err;
                            }
                            else
                            {
                                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                                DateTime dtime = startTime.AddMilliseconds(Convert.ToDouble(httpentity.timestamp));
                                TimeSpan ts = DateTime.Now - dtime;
                                if (Math.Abs(ts.TotalMinutes) > Settings.Instance.GetInt(SettingKeys.HTTPReqSuviceTime))
                                {
                                    str_resp_err = "该请求已失效!";
                                    goto err;
                                }
                            }

                            //MD5(body +时间戳+Key)
                            string md5 = MD5Encrypt.MD5(System.Web.HttpUtility.UrlEncode(httpentity.Params + httpentity.timestamp + Settings.Instance.GetString(SettingKeys.HTTPMD5Key)).ToLower());
                            if (md5 != httpentity.digest)
                            {
                                str_resp_err = "MD5校验失败!";
                                goto err;
                            }

                            str_resp_err = "无效action";
                            string action = "";

                            foreach (Match m in Regex.Matches(httpentity.Params, "\"action\" ?: ?\"(?<title>.*?)\"", RegexOptions.IgnoreCase))
                            {
                                if (m.Success)
                                    action = m.Groups["title"].Value;
                            }

                            try
                            {
                                str_resp_err = "";
                                switch (action)
                                {
                                    case "getgroups":
                                        GetGroupsRespEntity getgrouprespentity = noisebll.GetGroupsInfo();
                                        str_resp = JSONSerialize.JsonSerialize<GetGroupsRespEntity>(getgrouprespentity);
                                        break;
                                    case "uploadnoisedata":  //上传噪声数据
                                        UploadNoiseDataReqEntity parmentity = JSONSerialize.JsonDeserialize_Newtonsoft<UploadNoiseDataReqEntity>(httpentity.Params);
                                        HTTPRespEntity uploadrespentity = noisebll.UploadGroups(parmentity.TerData);
                                        str_resp = JSONSerialize.JsonSerialize<HTTPRespEntity>(uploadrespentity);
                                        break;
                                    case "uploadflowdata":  //未使用
                                        UploadFlowDataReqEntity parmflowentity = JSONSerialize.JsonDeserialize_Newtonsoft<UploadFlowDataReqEntity>(httpentity.Params);

                                        if (parmflowentity != null && parmflowentity.TerData != null)
                                        {
                                            foreach (UpLoadFlowDataEntity flowentity in parmflowentity.TerData)
                                            {
                                                GPRSFlowFrameDataEntity framedata = new GPRSFlowFrameDataEntity();
                                                framedata.TerId = flowentity.terid;
                                                framedata.ModifyTime = DateTime.Now;
                                                framedata.Frame = "";
                                                GPRSFlowDataEntity data = new GPRSFlowDataEntity();
                                                data.Forward_FlowValue = Convert.ToDouble(flowentity.flowvalue);
                                                data.Reverse_FlowValue = Convert.ToDouble(flowentity.flowinverted);
                                                data.Instant_FlowValue = Convert.ToDouble(flowentity.flowinstant);
                                                data.ColTime = Convert.ToDateTime(flowentity.collTime);
                                                framedata.lstFlowData.Add(data);
                                                GlobalValue.Instance.GPRS_FlowFrameData.Enqueue(framedata);  //通知存储线程处理
                                            }
                                        }
                                        GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertFlowValue);

                                        uploadrespentity = new HTTPRespEntity();
                                        uploadrespentity.code = 1;
                                        str_resp = SmartWaterSystem.JSONSerialize.JsonSerialize<HTTPRespEntity>(uploadrespentity);
                                        break;
                                    #region 消防栓
                                    case "gethydrants":         //获取所有消防栓
                                        GetHydrantRespEntity gethydrantrespentity = noisebll.GetHydrants();
                                        str_resp = SmartWaterSystem.JSONSerialize.JsonSerialize<GetHydrantRespEntity>(gethydrantrespentity);
                                        break;
                                    case "savehydrant":          //新增/修改消防栓
                                        SaveHydrantReqEntity savehydrantentity = JSONSerialize.JsonDeserialize_Newtonsoft<SaveHydrantReqEntity>(httpentity.Params);
                                        uploadrespentity = noisebll.SaveHydrantInfo(savehydrantentity);
                                        str_resp = SmartWaterSystem.JSONSerialize.JsonSerialize<HTTPRespEntity>(uploadrespentity);
                                        break;
                                    case "delhydrant":          //删除消防栓
                                        DelHydrantReqEntity delhydrantentity = JSONSerialize.JsonDeserialize_Newtonsoft<DelHydrantReqEntity>(httpentity.Params);
                                        uploadrespentity = noisebll.DelHydrant(delhydrantentity.HydrantID);
                                        str_resp = SmartWaterSystem.JSONSerialize.JsonSerialize<HTTPRespEntity>(uploadrespentity);
                                        break;
                                    case "modifyhydrantcoord":     //修改坐标
                                        ModifyHyCoordReqEntity modifyhydrantentity = JSONSerialize.JsonDeserialize_Newtonsoft<ModifyHyCoordReqEntity>(httpentity.Params);
                                        uploadrespentity = noisebll.ModifyHydrantCoordinate(modifyhydrantentity);
                                        str_resp = SmartWaterSystem.JSONSerialize.JsonSerialize<HTTPRespEntity>(uploadrespentity);
                                        break;
                                    case "gethydrantdetail":       //获取消防栓明细
                                        HyrdrantDetailReqEntity hydetailntity = JSONSerialize.JsonDeserialize_Newtonsoft<HyrdrantDetailReqEntity>(httpentity.Params);
                                        GetHydrantDetailRespEntity HydrantDetailResp = noisebll.GetHydrantDetail(hydetailntity);
                                        str_resp = SmartWaterSystem.JSONSerialize.JsonSerialize<GetHydrantDetailRespEntity>(HydrantDetailResp);
                                        break;
                                    #endregion

                                    #region 定位维修APP
                                    case "getalltermaginfo":       //获取所有的安装终端信息
                                        TerMagInfoResqEntity gettermaginforespentity = termagbll.GetAllTer(PicNetDirHead);
                                        str_resp = JSONSerialize.JsonSerialize<TerMagInfoResqEntity>(gettermaginforespentity);
                                        break;
                                    case "addtermaginfo":        //新增安装终端
                                        AddTerMagInfoReqEntity addtermaginfo = JSONSerialize.JsonDeserialize_Newtonsoft<AddTerMagInfoReqEntity>(httpentity.Params);

                                        //uploadrespentity = noisebll.UploadGroups(parmentity.TerData);
                                        //str_resp = JSONSerialize.JsonSerialize<HTTPRespEntity>(uploadrespentity);
                                        /*
                                        //手持照
                                        string handsourceFile = "", handdestFile = "", handnewnetpath = "";
                                        PicHelper.GetNetPic2LocalFileName(data.UserId, data.PichandAddr, nettmppathhead, netpathhead,localtmphead, localhead, out handsourceFile, out handdestFile, out handnewnetpath);
                                        data.PichandAddr = handnewnetpath;

                                        string msg = "";
                                        int result = dal.IndividualIdentifyMaterial(data, out msg);
                                        if (result != 1)
                                        {
                                            resp.ErrCode = RespResult.ParameteErr;
                                            resp.ErrMsg = msg;
                                        }
                                        else
                                        {
                                            resp.ErrCode = RespResult.Success;
                                            resp.ErrMsg = "";
                                            (new PicHelper()).MoveFile(frontsourceFile, frontdestFile);  //将图片文件从临时文件夹移动到正式目录
                                        */
                                        break;
                                    case "deltermaginfo":        //删除安装终端
                                        DelTerMagInfoReqEntity deltermagparmentity = JSONSerialize.JsonDeserialize_Newtonsoft<DelTerMagInfoReqEntity>(httpentity.Params);
                                        HTTPRespEntity delrespentity = termagbll.DelTer(deltermagparmentity.id);
                                        str_resp = JSONSerialize.JsonSerialize<HTTPRespEntity>(delrespentity);
                                        break;
                                    case "modifytermaginfo":    //修改安装终端信息
                                        break;
                                    case "gettermagpic":        //获取图片
                                                                //if(string.IsNullOrEmpty(Settings.Instance.GetString(SettingKeys.PicPath)))
                                                                //{
                                                                //    str_resp_err = "未找到存放图片路径,请设置服务程序图片路径参数!";
                                                                //    break;
                                                                //}
                                        break;
                                    case "uploadtermagpic":     //上传图片
                                        if (string.IsNullOrEmpty(PicLocalTmpDir))
                                        {
                                            str_resp_err = "未找到存放图片路径,请设置服务程序图片路径参数!";
                                            break;
                                        }
                                        if (string.IsNullOrEmpty(PicNetTmpDirHead))
                                        {
                                            str_resp_err = "未找到存放图片路径,请设置服务程序图片路径参数!";
                                            break;
                                        }
                                        UploadPicReqEntity picreqentity = JSONSerialize.JsonDeserialize_Newtonsoft<UploadPicReqEntity>(httpentity.Params);
                                        HTTPRespEntity uploadpicrespentity = new PicBLL().UpLoadPic(picreqentity.picdata, PicLocalTmpDir, PicNetTmpDirHead, "jpg");
                                        str_resp = JSONSerialize.JsonSerialize<HTTPRespEntity>(uploadpicrespentity);
                                        break;
                                    case "":                    //查看维修历史
                                        break;
                                        #endregion
                                }
                            }
                            catch (Exception ex)
                            {
                                str_resp_err = "解析异常";
                            }
                        }
                        catch (Exception ex)
                        {
                            str_resp_err = "无效参数类型";
                        }
                    }
                }

                err:
                if (!string.IsNullOrEmpty(str_resp_err) || string.IsNullOrEmpty(str_resp))
                {
                    HTTPRespEntity respent = new HTTPRespEntity();
                    respent.code = -1;
                    if (!string.IsNullOrEmpty(str_resp_err))
                        respent.msg = str_resp_err;
                    respent.data = "";

                    str_resp = SmartWaterSystem.JSONSerialize.JsonSerialize<HTTPRespEntity>(respent);
                }
                OnReceiveMsg(DateTime.Now.ToString() + " 响应内容:" + str_resp);
                str_resp = System.Web.HttpUtility.UrlEncode(str_resp);
                byte[] buffer_resp = Encoding.UTF8.GetBytes(str_resp);
                context.Response.OutputStream.Write(buffer_resp, 0, buffer_resp.Length);
                context.Response.OutputStream.Flush();
                context.Response.OutputStream.Close();

                context.Response.Close();
            }
            catch (Exception ex)
            {
                logger.ErrorException("ServiceThread", ex);
            }
        }

    }
}
