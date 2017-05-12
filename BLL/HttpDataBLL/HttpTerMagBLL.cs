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
            resp.code = 1;
            resp.msg = "";
            try
            {
                resp.lstTer = dal.GetAllTer(netpathhead);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetAllTer", ex);
                resp.code = -1;
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
            resp.code = 1;
            resp.msg = "";
            try
            {
                dal.DelTer(Id);
            }
            catch (Exception ex)
            {
                logger.ErrorException("DelTer", ex);
                resp.code = -1;
                resp.msg = "服务器异常";
            }
            return resp;
        }
        
        public HTTPRespEntity AddTerMagInfo(TerMagInfoEntity terinfo,string localtmppath,string localhead)
        {
            HTTPRespEntity resp = new HTTPRespEntity();
            resp.code = 1;
            resp.msg = "";
            try
            {
                if (terinfo.DevId <= 0)
                {
                    resp.code = -1;
                    resp.msg = "终端编号不合法！";
                    return resp;
                }
                //检查终端是否已经存在
                TerMagInfoEntity checkTerMagInfo =dal.QueryTerMagInfo(terinfo.DevType, terinfo.DevId, "");
                if (checkTerMagInfo != null)
                {
                    resp.code = -1;
                    resp.msg = "添加的终端已存在！";
                    return resp;
                }
                if (terinfo.PicId == null || terinfo.PicId.Count == 0)
                {
                    resp.code = -1;
                    resp.msg = "请先上传图片！";
                    return resp;
                }
                //检查临时图片文件是否存在
                foreach (string picname in terinfo.PicId)
                {
                    string picfullpath=Path.Combine(localtmppath, picname);
                    if(!File.Exists(picfullpath))
                    {
                        resp.code = -1;
                        resp.msg = "临时图片不存在["+picname+"]";
                        return resp;
                    }
                }

                //检查图片是否已经存在
                if (dal.CheckPicExist(terinfo.PicId))
                {
                    resp.code = -1;
                    resp.msg = "上传的图片已经存在！";
                    return resp;
                }
                //保存数据到数据库
                if (!dal.InsertTerMagInfo(terinfo, localtmppath, localhead))
                {
                    resp.code = -1;
                    return resp;
                }
                
            }
            catch (Exception ex)
            {
                logger.ErrorException("AddTerMagInfo", ex);
                resp.code = -1;
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


    }
}
