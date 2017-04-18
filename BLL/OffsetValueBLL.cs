using DAL;
using System;
using System.Data;

namespace BLL
{
    public class OffsetValueBLL
    {
        OffsetValueDAL dal = new OffsetValueDAL();
        NLog.Logger logger = NLog.LogManager.GetLogger("OffsetValueBLL");

        /// <summary>
        /// 获取全部的偏移值
        /// </summary>
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

        /// <summary>
        /// 保存所有的偏移量
        /// </summary>
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
