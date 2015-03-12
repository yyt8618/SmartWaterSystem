using System;
using System.Collections.Generic;
using Entity;
using DAL;

namespace BLL
{
    public class TerminalDataBLL
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("TerminalDataBLL");
        TerminalDataDAL dal = new TerminalDataDAL();

        /// <summary>
        /// 获取压力终端数据
        /// </summary>
        public List<PreDataEntity> GetPreData(List<string> terminalIds)
        {
            try
            {
                return dal.GetPreData(terminalIds);
            }
            catch (Exception ex)
            {
                return null;
                logger.ErrorException("terminalIds", ex);
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

        /// <summary>
        /// 获取指定时间范围的终端压力数据
        /// </summary>
        public List<PreDetailDataEntity> GetDetail(string TerminalID, DateTime minTime, DateTime maxTime, int interval)
        {
            try
            {
                return dal.GetDetail(TerminalID, minTime, maxTime, interval);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetDetail", ex);
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
    }
}
