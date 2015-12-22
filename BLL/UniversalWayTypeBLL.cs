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
        public int GetMaxSequence(int parentId, TerType terType)
        {
            try
            {
                return dal.GetMaxSequence(parentId,terType);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetMaxSequence", ex);
                return -1;
            }
        }

        public int TypeExist(UniversalCollectType type, string name, TerType terType)
        {
            try
            {
                return dal.TypeExist(type, name,terType);
            }
            catch (Exception ex)
            {
                logger.ErrorException("TypeExist", ex);
                return -1;
            }
        }

        public int Insert(UniversalWayTypeEntity entity, TerType terType)
        {
            try
            {
                return dal.Insert(entity,terType);
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
        public List<UniversalWayTypeEntity> GetConfigPointID(string id, TerType terType)
        {
            try
            {
                return dal.GetConfigPointID(id,terType);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetAllConfigPointID", ex);
                return null;
            }
        }

        public DataTable GetTerminalID_Configed(TerType terType)
        {
            try
            {
                return dal.GetTerminalID_Configed(terType);
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

        /// <summary>
        /// 获得从UniversalTerWayConfig表中的第?路数据,-1为无效数据
        /// </summary>
        public int GetCofingSequence(string Terid, string pointid, TerType terType)
        {
            try
            {
                return dal.GetCofingSequence(Terid, pointid, terType);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetCofingSequence", ex);
                return -1;
            }
        }

    }
}
