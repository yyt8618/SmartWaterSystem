using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
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
        public List<TerminalConfigEntity> GetAllTerminals()
        {
            try
            {
                return dal.GetAllTerminals();
            }
            catch (Exception ex)
            {
                return null;
                logger.ErrorException("GetAllTerminals", ex);
            }
        }

        public TerminalConfigEntity GetTerminal(string TerminalID)
        {
            try
            {
                return dal.GetTerminal(TerminalID);
            }
            catch (Exception ex)
            {
                return null;
                logger.ErrorException("GetTerminal", ex);
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
                return false;
                logger.ErrorException("IsExist", ex);
            }
        }

        public Hashtable GetTerID_PointID()
        {
            try
            {
                return dal.GetTerID_PointID();
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetTerID_PointID()", ex);
                return null;
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
                return false;
                logger.ErrorException("Insert", ex);
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
                return false;
                logger.ErrorException("Modify", ex);
            }
        }

        /// <summary>
        /// 根据终端编号删除一条记录
        /// </summary>
        public bool Delete(string TerminalID)
        {
            try
            {
                return dal.Delete(TerminalID);
            }
            catch (Exception ex)
            {
                return false;
                logger.ErrorException("Delete", ex);
            }
        }

        /// <summary>
        /// 获取全部的地址(去重)
        /// </summary>
        public List<string> GetAddresses()
        {
            try
            {
                return dal.GetAddresses();
            }
            catch (Exception ex)
            {
                return null;
                logger.ErrorException("GetAddresses", ex);
            }
        }

    }
}
