using DAL;
using System;
using System.Data;

namespace BLL
{
    public class RectifyValueBLL
    {
        RectifyValueDAL dal = new RectifyValueDAL();
        NLog.Logger logger = NLog.LogManager.GetLogger("RectifyValueBLL");

        /// <summary>
        /// 获取全部的偏移值
        /// </summary>
        public DataTable GetAllRectifyValue()
        {
            try
            {
                return dal.GetAllRectifyValue();
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetAllRectifyValue", ex);
                return null;
            }
        }

        /// <summary>
        /// 保存所有的偏移量
        /// </summary>
        public bool SaveRectifyValue(DataTable dt)
        {
            try
            {
                return dal.SaveRectifyValue(dt);
            }
            catch (Exception ex)
            {
                logger.ErrorException("SaveRectifyValue", ex);
                return false;
            }
        }
    }
}
