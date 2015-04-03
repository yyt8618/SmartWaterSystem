using System;
using System.Collections.Generic;
using Entity;
using DAL;
using System.Data;

namespace BLL
{
    public class TerminalDataBLL
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("TerminalDataBLL");
        TerminalDataDAL dal = new TerminalDataDAL();

        public DataTable GetTerInfo(TerType type)
        {
            try
            {
                return dal.GetTerInfo(type);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetTerInfo", ex);
                return null;
            }
        }

        #region GPRS数据操作
        public int InsertGPRSPreData(Queue<GPRSPreFrameDataEntity> datas, out string msg)
        {
            msg = "";
            try
            {
                if ((datas == null) || (datas.Count == 0))
                    return 0;
                return dal.InsertGPRSPreData(datas);
            }
            catch (Exception ex)
            {
                logger.ErrorException("InsertGPRSPreData", ex);
                msg = "保存至数据库发生异常";
                return -1;
            }
        }

        public int InsertGPRSFlowData(Queue<GPRSFlowFrameDataEntity> datas, out string msg)
        {
            msg = "";
            try
            {
                if (datas.Count == 0)
                    return 0;
                return dal.InsertGPRSFlowData(datas);
            }
            catch (Exception ex)
            {
                logger.ErrorException("InsertGPRSFlowData", ex);
                msg = "保存至数据库发生异常";
                return -1;
            }
        }

        public int InsertGPRSUniversalData(Queue<GPRSUniversalFrameDataEntity> datas, out string msg)
        {
            msg = "";
            try
            {
                if (datas.Count == 0)
                    return 0;
                return dal.InsertGPRSUniversalData(datas);
            }
            catch (Exception ex)
            {
                logger.ErrorException("InsertGPRSUniversalData", ex);
                msg = "保存至数据库发生异常";
                return -1;
            }
        }

        public List<GPRSCmdEntity> GetGPRSParm()
        {
            try
            {
                return dal.GetGPRSParm();
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetGPRSParm()", ex);
                return null;
            }
        }

        public int UpdateGPRSParmFlag(List<GPRSCmdFlag> ids)
        {
            try
            {
                return dal.UpdateGPRSParmFlag(ids);
            }
            catch (Exception ex)
            {
                logger.ErrorException("UpdateGPRSParmFlag()", ex);
                return -1;
            }
        }
        #endregion

        /// <summary>
        /// 保存通用终端配置 -1:保存异常,1:保存成功
        /// </summary>
        public int SaveUniversalTerConfig(int terminalid, string name, string addr, string remark, List<UniversalWayTypeConfigEntity> lstPointID)
        {
            try
            {
                return dal.SaveUniversalTerConfig(terminalid, name, addr, remark, lstPointID);
            }
            catch (Exception ex)
            {
                logger.ErrorException("SaveUniversalTerConfig", ex);
                return -1;
            }
        }

        public int DeleteUniversalWayTypeConfig(int PointID)
        {
            try
            {
                dal.DeleteUniversalWayTypeConfig(PointID);
                return 1;
            }
            catch (Exception ex)
            {
                logger.ErrorException("DeleteUniversalWayTypeConfig", ex);
                return -1;
            }
        }

        public int DeleteUniversalWayTypeConfig_TerID(int TerminalID)
        {
            try
            {
                dal.DeleteUniversalWayTypeConfig_TerID(TerminalID);
                return 1;
            }
            catch (Exception ex)
            {
                logger.ErrorException("DeleteUniversalWayTypeConfig_TerID", ex);
                return -1;
            }
        }

        /// <summary>
        /// 获取对应终端的采集配置
        /// </summary>
        /// <param name="TerminalID"></param>
        /// <returns></returns>
        public List<UniversalWayTypeConfigEntity> GetUniversalWayTypeConfig(int TerminalID)
        {
            try
            {
                return dal.GetUniversalWayTypeConfig(TerminalID);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetUniversalWayTypeConfig", ex);
                return null;
            }
        }

        public int DeleteTer(TerType type, int TerminalID)
        {
            try
            {
                dal.DeleteTer(type, TerminalID);
                return 1;
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetUniversalWayTypeConfig", ex);
                return -1;
            }
        }

        public DataTable GetUniversalDataConfig()
        {
            try
            {
                return dal.GetUniversalDataConfig();
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetUniversalDataConfig()", ex);
                return null;
            }
        }

    }
}
