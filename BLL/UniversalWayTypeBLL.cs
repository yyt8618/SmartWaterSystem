using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;

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
    }
}
