using System;
using System.Collections.Generic;
using Entity;
using DAL;

namespace BLL
{
    public class TerminalConfigBLL
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("TerminalConfigBLL");
        TerminalConfigDAL dal = new TerminalConfigDAL();

        /// <summary>
        /// 获取所有压力终端配置数据
        /// </summary>
        /// <returns></returns>
        public List<TerminalConfigEntity> GetAllPreTerminals()
        {
            try
            {
                return dal.GetAllPreTerminals();
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetAllPreTerminals", ex);
                return null;
            }
        }

        /// <summary>
        /// 指定终端编号是否存在
        /// </summary>
        /// <param name="TerminalID"></param>
        /// <returns></returns>
        public bool IsExist(string TerminalID)
        {
            try
            {
                return dal.IsExist(TerminalID);
            }
            catch (Exception ex)
            {
                logger.ErrorException("IsExist", ex);
                return false;
            }
        }

        /// <summary>
        /// 插入一条记录
        /// </summary>
        public bool Insert(TerminalConfigEntity entity)
        {
            try
            {
                return dal.Insert(entity);
            }
            catch (Exception ex)
            {
                logger.ErrorException("Insert", ex);
                return false;
            }
        }

        /// <summary>
        /// 修改一条记录(根据终端编号)
        /// </summary>
        public bool Modify(TerminalConfigEntity entity)
        {
            try
            {
                return dal.Modify(entity);
            }
            catch (Exception ex)
            {
                logger.ErrorException("Modify", ex);
                return false;
            }
        }

        /// <summary>
        /// 根据终端编号删除一条记录
        /// </summary>
        public bool DeletePreTer(string TerminalID)
        {
            try
            {
                return dal.DeletePreTer(TerminalID);
            }
            catch (Exception ex)
            {
                logger.ErrorException("Delete", ex);
                return false;
            }
        }

        public bool ExistUniversalConfig(string terId)
        {
            try
            {
                return dal.ExistUniversalConfig(terId);
            }
            catch (Exception ex)
            {
                logger.ErrorException("ExistUniversalConfig", ex);
                return false;
            }
        }

    }
}
