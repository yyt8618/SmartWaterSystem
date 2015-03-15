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

        //同步模拟类型信息
        public bool SyncSimulateType()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //同步RS485类型信息
        public bool SyncRS485Type()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //同步脉冲类型信息
        public bool SyncPluseType()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
