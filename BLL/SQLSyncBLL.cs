using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class SQLSyncBLL
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("SQLSyncBLL");
        DAL.SQLSyncDAL dal = new DAL.SQLSyncDAL();

        //同步通用终端采集方式表
        public bool Update_UniversalTerWayType()
        {
            try
            {
                dal.Update_UniversalTerWayType();
                return true;
            }
            catch (Exception ex)
            {
                logger.ErrorException("Update_UniversalTerWayType", ex);
                return false;
            }
        }

        public bool UpdateSQL_UniversalTerWayConfig()
        {
            try
            {
                dal.UpdateSQL_UniversalTerWayConfig();
                return true;
            }
            catch (Exception ex)
            {
                logger.ErrorException("UpdateSQL_UniversalTerWayConfig", ex);
                return false;
            }
        }

        public bool UpdateSQL_Terminal()
        {
            try
            {
                dal.UpdateSQL_Terminal();
                return true;
            }
            catch (Exception ex)
            {
                logger.ErrorException("UpdateSQL_Terminal", ex);
                return false;
            }
        }

        public bool UpdateSQL_PreTerConfig()
        {
            try
            {
                dal.UpdateSQL_PreTerConfig();
                return true;
            }
            catch (Exception ex)
            {
                logger.ErrorException("UpdateSQL_PreTerConfig", ex);
                return false;
            }
        }
    }
}
