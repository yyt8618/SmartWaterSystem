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
    }
}
