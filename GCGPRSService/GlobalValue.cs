using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Entity;

namespace GCGPRSService
{
    public class GlobalValue
    {
        private static GlobalValue _instance;
        public static GlobalValue Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GlobalValue();

                return _instance;
            }
        }

        /// <summary>
        /// GPRS远传数据操作管理线程
        /// </summary>
        public SocketSQLManager SocketSQLMag = new SocketSQLManager();
        /// <summary>
        /// GPRS远传服务线程
        /// </summary>
        public SocketManager SocketMag = new SocketManager();

        private Queue<GPRSPreFrameDataEntity> _GPRS_PreFrameData = new Queue<GPRSPreFrameDataEntity>();
        /// <summary>
        /// GPRS压力帧队列
        /// </summary>
        public Queue<GPRSPreFrameDataEntity> GPRS_PreFrameData
        {
            get { return _GPRS_PreFrameData; }
            set { _GPRS_PreFrameData = value; }
        }

        private Queue<GPRSFlowFrameDataEntity> _GPRS_FlowFrameData = new Queue<GPRSFlowFrameDataEntity>();
        /// <summary>
        /// GPRS流量帧数据
        /// </summary>
        public Queue<GPRSFlowFrameDataEntity> GPRS_FlowFrameData
        {
            get { return _GPRS_FlowFrameData; }
            set { _GPRS_FlowFrameData = value; }
        }

        private List<GPRSCmdEntity> _lstGprsCmd = new List<GPRSCmdEntity>();
        /// <summary>
        /// GPRS下送命令
        /// </summary>
        public List<GPRSCmdEntity> lstGprsCmd
        {
            get { return _lstGprsCmd; }
            set { _lstGprsCmd = value; }
        }

        private List<GPRSCmdFlag> _lstSendedCmdId = new List<GPRSCmdFlag>();
        /// <summary>
        /// GPRS下发命令后对应的数据库表,用于更新数据库
        /// </summary>
        public List<GPRSCmdFlag> lstSendedCmdId
        {
            get { return _lstSendedCmdId; }
            set { _lstSendedCmdId = value; }
        }

        
    }
}
