using DAL;
using Entity;
using SmartWaterSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BLL
{
    public class HttpTerMagBLL
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("HttpTerMagBLL");
        HttpTerMagDAL dal = new HttpTerMagDAL();

        /// <summary>
        /// 获取所有的安装终端信息
        /// </summary>
        /// <param name="netpathhead">网络地址头</param>
        /// <returns></returns>
        public TerMagInfoResqEntity GetAllTer(string netpathhead)
        {
            TerMagInfoResqEntity resp = new TerMagInfoResqEntity();
            resp.code = HttpRespCode.Success;
            resp.msg = "";
            try
            {
                resp.lstTer = dal.GetAllTer(netpathhead);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetAllTer", ex);
                resp.code = HttpRespCode.Excp;
                resp.msg = "服务器异常";
            }
            return resp;
        }

        /// <summary>
        /// 安装表Id删除终端
        /// </summary>
        /// <param name="Id"></param>
        public HTTPRespEntity DelTer(int Id)
        {
            HTTPRespEntity resp = new HTTPRespEntity();
            resp.code = HttpRespCode.Success;
            resp.msg = "";
            try
            {
                dal.DelTer(Id);
            }
            catch (Exception ex)
            {
                logger.ErrorException("DelTer", ex);
                resp.code = HttpRespCode.Excp;
                resp.msg = "服务器异常";
            }
            return resp;
        }

        public HTTPRespEntity AddTerMagInfo(TerMagInfoEntity TerInfo, string PicLocalTmpDir, string PicLocalDir)
        {
            HTTPRespEntity resp = new HTTPRespEntity();
            resp.code = HttpRespCode.Success;
            resp.msg = "";
            try
            {
                if (TerInfo.DevId <= 0)
                {
                    resp.code = HttpRespCode.Fail;
                    resp.msg = "终端编号不合法！";
                    return resp;
                }
                //检查终端是否已经存在
                TerMagInfoEntity checkTerMagInfo = dal.QueryTerMagInfo(TerInfo.DevType, TerInfo.DevId, "");
                if (checkTerMagInfo != null)
                {
                    resp.code = HttpRespCode.Fail;
                    resp.msg = "添加的终端已存在！";
                    return resp;
                }
                if (TerInfo.PicId == null || TerInfo.PicId.Count == 0)
                {
                    //resp.code = HttpRespCode.Fail;
                    //resp.msg = "请先上传图片！";
                    //return resp;
                }
                else
                {
                    PicBLL picbll = new PicBLL();
                    //检查临时图片文件是否存在
                    string piccheckres = picbll.CheckTmpPicExist(PicLocalTmpDir, TerInfo.PicId);
                    if (!string.IsNullOrEmpty(piccheckres))
                    {
                        resp.code = HttpRespCode.Fail;
                        resp.msg = piccheckres;
                        return resp;
                    }

                    //检查图片数据库表中是否已经存在
                    if (picbll.CheckPicExist(TerInfo.PicId))
                    {
                        resp.code = HttpRespCode.Fail;
                        resp.msg = "上传的图片已经存在！";
                        return resp;
                    }
                }
                //保存数据到数据库
                if (!dal.InsertTerMagInfo(TerInfo, PicLocalTmpDir, PicLocalDir))
                {
                    resp.code = HttpRespCode.Fail;
                    return resp;
                }

            }
            catch (Exception ex)
            {
                logger.ErrorException("AddTerMagInfo", ex);
                resp.code = HttpRespCode.Excp;
                resp.msg = "服务器异常";
            }
            return resp;
        }

        /// <summary>
        /// 修改终端信息
        /// </summary>
        /// <param name="terinfo"></param>
        /// <returns></returns>
        public HTTPRespEntity UpdateTerMagInfo(TerMagInfoEntity terinfo)
        {
            return null;
        }

        /// <summary>
        /// 上传维修记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public HTTPRespEntity UploadRepairRec(RepairInfoEntity repairRec, string PicLocalTmpDir, string PicLocalDir)
        {
            HTTPRespEntity resp = new HTTPRespEntity();
            resp.code = HttpRespCode.Success;
            resp.msg = "";
            try
            {
                if (repairRec.Id <= 0)
                {
                    resp.code = HttpRespCode.Fail;
                    resp.msg = "终端编号不合法！";
                    return resp;
                }
                if (!dal.IsExistID(repairRec.Id))
                {
                    resp.code = HttpRespCode.Fail;
                    resp.msg = "终端不存在！";
                    return resp;
                }
                //检查图片
                if (repairRec.PicsPath != null && repairRec.PicsPath.Count > 0)
                {
                    PicBLL picbll = new PicBLL();
                    //检查临时图片文件是否存在
                    string piccheckres = picbll.CheckTmpPicExist(PicLocalTmpDir, repairRec.PicsPath);
                    if (!string.IsNullOrEmpty(piccheckres))
                    {
                        resp.code = HttpRespCode.Fail;
                        resp.msg = piccheckres;
                        return resp;
                    }

                    //检查图片数据库表中是否已经存在
                    if (picbll.CheckPicExist(repairRec.PicsPath))
                    {
                        resp.code = HttpRespCode.Fail;
                        resp.msg = "上传的图片已经存在！";
                        return resp;
                    }
                }

                dal.UploadRepairRec(repairRec, PicLocalTmpDir, PicLocalDir);
                resp.msg = "上传成功";
            }
            catch (Exception ex)
            {
                logger.ErrorException("UploadRepairRec", ex);
                resp.code = HttpRespCode.Excp;
                resp.msg = "服务器异常";
            }
            return resp;
        }

        /// <summary>
        /// 通过id获取维修记录
        /// </summary>
        /// <param name="TerMagId"></param>
        /// <param name="netpathhead">网络地址头</param>
        /// <returns></returns>
        public QueryRepairRecRespEntity QueryRepairRec(long TerMagId, string netpathhead)
        {
            QueryRepairRecRespEntity resp = new QueryRepairRecRespEntity();
            resp.code = HttpRespCode.Success;
            resp.msg = "";
            try
            {
                resp.lstRec = dal.QueryRepairRec(TerMagId, netpathhead);
            }
            catch (Exception ex)
            {
                logger.ErrorException("QueryRepairRec", ex);
                resp.code = HttpRespCode.Excp;
                resp.msg = "服务器异常";
            }
            return resp;
        }

        /// <summary>
        /// 获取所有的终端类型信息
        /// </summary>
        public TerTypeInfoResqEntity GetAllTerTypeInfo()
        {
            TerTypeInfoResqEntity resp = new TerTypeInfoResqEntity();
            resp.code = HttpRespCode.Success;
            resp.msg = "";
            try
            {
                resp.lsttype = dal.GetAllTerTypeInfo();
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetAllTerTypeInfo", ex);
                resp.code = HttpRespCode.Excp;
                resp.msg = "服务器异常";
            }
            return resp;
        }


    }
}
