using System;
using DAL;
using Entity;
using System.Collections.Generic;

namespace BLL
{
    public class OLWQConfigBLL
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("OLWQConfigBLL");
        private OLWQConfigDAL dal = new OLWQConfigDAL();

        public bool Insert(OLWQConfigEntity configData)
        {
            try
            {
                return dal.Insert(configData);
            }
            catch (Exception ex)
            {
                logger.ErrorException("Insert", ex);
                return false;
            }
        }

        public bool Delete(string TerId)
        {
            try
            {
                dal.Delete(TerId);
                return true;
            }
            catch (Exception ex)
            {
                logger.ErrorException("Delete", ex);
                return false;
            }
        }

        public List<OLWQConfigEntity> Select(string where)
        {
            try
            {
                return dal.Select(where);
            }
            catch (Exception ex)
            {
                logger.ErrorException("Select", ex);
                return null;
            }
        }

    }
}
