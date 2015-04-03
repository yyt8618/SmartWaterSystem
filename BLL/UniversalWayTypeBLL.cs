using System;
using System.Collections.Generic;
using Entity;
using System.Data;

namespace BLL
{
    public class UniversalWayTypeBLL
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("UniversalWayTypeBLL");
        DAL.UniversalWayTypeDAL dal = new DAL.UniversalWayTypeDAL();

        //获取最大ID
        public int GetMaxId()
        {
            try
            {
                return dal.GetMaxId();
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetMaxId", ex);
                return -1;
            }
        }

        //获取最大Sequence
        public int GetMaxSequence(int parentId)
        {
            try
            {
                return dal.GetMaxSequence(parentId);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetMaxSequence", ex);
                return -1;
            }
        }

        public int TypeExist(UniversalCollectType type, string name)
        {
            try
            {
                return dal.TypeExist(type, name);
            }
            catch (Exception ex)
            {
                logger.ErrorException("TypeExist", ex);
                return -1;
            }
        }

        public int Insert(UniversalWayTypeEntity entity)
        {
            try
            {
                return dal.Insert(entity);
            }
            catch (Exception ex)
            {
                logger.ErrorException("Insert", ex);
                return -1;
            }
        }

        /// <summary>
        /// 根据ID删除一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(int id)
        {
            try
            {
                return dal.Delete(id);
            }
            catch (Exception ex)
            {
                logger.ErrorException("Delete", ex);
                return -1;
            }
        }

        public int UpdateFlag(int id, int state)
        {
            try
            {
                return dal.UpdateFlag(id, state);
            }
            catch (Exception ex)
            {
                logger.ErrorException("UpdateFlag", ex);
                return -1;
            }
        }

        public List<UniversalWayTypeEntity> Select(string where)
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

        /// <summary>
        /// 获得配置的全部PointID(不区分ID)
        /// </summary>
        /// <returns></returns>
        public List<UniversalWayTypeEntity> GetAllConfigPointID()
        {
            try
            {
                return dal.GetAllConfigPointID();
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetAllConfigPointID", ex);
                return null;
            }
        }

        public DataTable GetTerminalID_Configed()
        {
            try
            {
                return dal.GetTerminalID_Configed();
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetTerminalID_Configed",ex);
                return null;
            }
        }

        /// <summary>
        /// 获取指定终端的数据,并按指定顺序排列
        /// </summary>
        /// <returns></returns>
        public DataTable GetTerminalDataToShow(List<string> lstTerID,List<int> lstTypeID)
        {
            try
            {
                return dal.GetTerminalDataToShow(lstTerID,lstTypeID);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetTerminalDataToShow", ex);
                return null;
            }
        }

    }
}
