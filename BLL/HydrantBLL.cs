using DAL;
using System;
using System.Collections.Generic;
using Entity;

namespace BLL
{
    public class HydrantBLL
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("HydrantBLL");
        HydrantDAL dal = new HydrantDAL();

        public List<HydrantEntity> SelectAll()
        {
            try
            {
                return dal.SelectAll();
            }
            catch (Exception ex)
            {
                logger.ErrorException("SelectAll", ex);
                return null;
            }
        }
        public bool Insert(string id, string addr, string longitude, string latitude)
        {
            try
            {
                dal.Insert(id, addr, longitude, latitude);
                return true;
            }
            catch (Exception ex)
            {
                logger.ErrorException("Insert", ex);
                return false;
            }
        }

        public bool Delete(string id)
        {
            try
            {
                dal.Delete(id);
                return true;
            }
            catch(Exception ex)
            {
                logger.ErrorException("Delete", ex);
                return false;
            }
        }

        public bool UnAlarm(string id)
        {
            try
            {
                dal.UnAlarm(id);
                return true;
            }
            catch (Exception ex)
            {
                logger.ErrorException("UnAlarm", ex);
                return false;
            }
        }

        public bool modifyCoordinate(string id, string longitude, string latitude)
        {
            try
            {
                dal.modifyCoordinate(id, longitude, latitude);
                return true;
            }
            catch (Exception ex)
            {
                logger.ErrorException("modifyCoordinate", ex);
                return false;
            }
        }

        public List<HydrantEntity> GetHydrantDetail(string HydrantID, int opt, DateTime minTime, DateTime maxTime, int interval)
        {
            try
            {
                return dal.GetHydrantDetail(HydrantID, opt, minTime, maxTime, interval);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetHydrantDetail", ex);
                return null;
            }
        }
    }
}
