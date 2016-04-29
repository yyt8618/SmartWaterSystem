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

        public int InsertGPRSPrectrlData(Queue<GPRSPrectrlFrameDataEntity> datas ,out string msg)
        {
            msg = "";
            try
            {
                if (datas.Count == 0)
                    return 0;
                return dal.InsertGPRSPrectrlData(datas);
            }
            catch (Exception ex)
            {
                logger.ErrorException("InsertGPRSPrectrlData", ex);
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

        public int InsertGPRSOLWQData(Queue<GPRSOLWQFrameDataEntity> datas, out string msg)
        {
            msg = "";
            try
            {
                if (datas.Count == 0)
                    return 0;
                return dal.InsertGPRSOLWQData(datas);
            }
            catch (Exception ex)
            {
                logger.ErrorException("InsertGPRSOLWQData", ex);
                msg = "保存至数据库发生异常";
                return -1;
            }
        }

        public int InsertGPRSHydrantData(Queue<GPRSHydrantFrameDataEntity> datas, out string msg)
        {
            msg = "";
            try
            {
                if (datas.Count == 0)
                    return 0;
                return dal.InsertGPRSHydrantData(datas);
            }
            catch (Exception ex)
            {
                logger.ErrorException("InsertGPRSHydrantData", ex);
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
        public int SaveTerInfo(int terminalid, string name, string addr, string remark, TerType terType, List<UniversalWayTypeConfigEntity> lstPointID)
        {
            try
            {
                return dal.SaveTerInfo(terminalid, name, addr, remark,terType, lstPointID);
            }
            catch (Exception ex)
            {
                logger.ErrorException("SaveTerInfo", ex);
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

        public int DeleteUniversalWayTypeConfig_TerID(int TerminalID,TerType terType)
        {
            try
            {
                dal.DeleteUniversalWayTypeConfig_TerID(TerminalID, terType);
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
        public List<UniversalWayTypeConfigEntity> GetUniversalWayTypeConfig(int TerminalID, TerType terType)
        {
            try
            {
                return dal.GetUniversalWayTypeConfig(TerminalID, terType);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetUniversalWayTypeConfig", ex);
                return null;
            }
        }

        public int DeleteTer(TerType type, string TerminalID)
        {
            try
            {
                dal.DeleteTer(type, TerminalID);
                return 1;
            }
            catch (Exception ex)
            {
                logger.ErrorException("DeleteTer", ex);
                return -1;
            }
        }

        public DataTable GetUniversalDataConfig(TerType terType)
        {
            try
            {
                return dal.GetUniversalDataConfig(terType);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetUniversalDataConfig()", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取指定时间范围的终端压力数据
        /// </summary>
        public List<PreDetailDataEntity> GetPreDetail(string TerminalID, DateTime minTime, DateTime maxTime, int interval)
        {
            try
            {
                return dal.GetPreDetail(TerminalID, minTime, maxTime, interval);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetPreDetail", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取指定时间范围的终端流量数据
        /// </summary>
        public List<PreDetailDataEntity> GetFlowDetail(string TerminalID, DateTime minTime, DateTime maxTime, int interval, int datatype)
        {
            try
            {
                return dal.GetFlowDetail(TerminalID, minTime, maxTime, interval, datatype);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetFlowDetail", ex);
                return null;
            }
        }

        public List<PrectrlDetailDataEntity> GetPrectrlDetail(string TerminalID, DateTime minTime, DateTime maxTime, int interval)
        {
            try
            {
                return dal.GetPrectrlDetail(TerminalID, minTime, maxTime, interval);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetPrectrlDetail", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取指定时间范围的终端压力数据
        /// </summary>
        public List<UniversalDetailDataEntity> GetUniversalDetail(string TerminalID, int typeId, DateTime minTime, DateTime maxTime, int interval)
        {
            try
            {
                return dal.GetUniversalDetail(TerminalID, typeId, minTime, maxTime, interval);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetUniversalDetail", ex);
                return null;
            }
        }

        public List<OLWQDetailDataEntity> GetOLWQDetail(string TerminalID, DateTime minTime, DateTime maxTime, int interval, int datatype)
        {
            try
            {
                return dal.GetOLWQDetail(TerminalID, minTime, maxTime, interval, datatype);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetOLWQDetail", ex);
                return null;
            }
        }

        public string GetTerminalName(string TerminalID, TerType tertype)
        {
            try
            {
                return dal.GetTerminalName(TerminalID, tertype);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetTerminalName", ex);
                return "";
            }
        }

        public bool InsertDevGPRSParm(int devId, int DevTypeId, int ctrlCode, int Funcode, string DataValue)
        {
            try
            {
                dal.InsertDevGPRSParm(devId, DevTypeId, ctrlCode, Funcode, DataValue);
                return true;
            }
            catch (Exception ex)
            {
                logger.ErrorException("InsertDevGPRSParm", ex);
                return false;
            }
        }

        /// <summary>
        /// 获取压力数据，包括最新和次新数据
        /// </summary>
        public List<PreDataEntity> GetPreDataTop2(List<int> terminalids)
        {
            try
            {
                return dal.GetPreDataTop2(terminalids);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetPreDataTop2", ex);
                return null;
            }
        }

        public List<FlowDataEntity> GetFlowDataTop2(List<int> terminalids)
        {
            try
            {
                return dal.GetFlowDataTop2(terminalids);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetFlowDataTop2", ex);
                return null;
            }
        }

        public List<PrectrlDataEntity> GetPrectrlData(List<int> terminalids)
        {
            try
            {
                return dal.GetPrectrlData(terminalids);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetPrectrlData", ex);
                return null;
            }
        }

        public DataTable GetOLWQData(List<string> terminalids)
        {
            try
            {
                return dal.GetOLWQData(terminalids);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetOLWQData", ex);
                return null;
            }
        }

    }
}
