using DAL;
using System;
using System.Data;

namespace BLL
{
    public class OffsetValueBLL
    {
        OffsetValueDAL dal = new OffsetValueDAL();
        NLog.Logger logger = NLog.LogManager.GetLogger("OffsetValueBLL");

        public DataTable GetAllOffsetValue()
        {
            try
            {
                return dal.GetAllOffsetValue();
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetAllOffsetValue", ex);
                return null;
            }
        }

        public bool SaveOffsetValue(DataTable dt)
        {
            try
            {
                return dal.SaveOffsetValue(dt);
            }
            catch (Exception ex)
            {
                logger.ErrorException("SaveOffsetValue", ex);
                return false;
            }
        }
    }
}
