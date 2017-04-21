using DAL;
using Entity;
using System;
using System.Collections.Generic;
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
    }
}
