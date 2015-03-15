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
