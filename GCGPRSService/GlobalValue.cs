using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Entity;
using System.Data;
using SmartWaterSystem;

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
        /// 更新lstGprsCmd锁
        /// </summary>
        public object lstGprsCmdLock = new object();

        /// <summary>
        /// //启动记录,用于smartsocket连接过来的时候将启动记录发送过去
        /// </summary>
        public List<string> lstStartRecord = new List<string>();
        public string SmartWaterHeartBeatName = "heartbeat";

        /// <summary>
        /// HTTP 服务
        /// </summary>
        public HTTPService HttpService = new HTTPService();

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

        private Queue<GPRSUniversalFrameDataEntity> _GPRS_UniversalFrameData = new Queue<GPRSUniversalFrameDataEntity>();
        /// <summary>
        /// GPRS通用终端帧数据
        /// </summary>
        public Queue<GPRSUniversalFrameDataEntity> GPRS_UniversalFrameData
        {
            get { return _GPRS_UniversalFrameData; }
            set { _GPRS_UniversalFrameData = value; }
        }

        private Queue<GPRSOLWQFrameDataEntity> _GPRS_OLWQFrameData = new Queue<GPRSOLWQFrameDataEntity>();
        /// <summary>
        /// GPRS水质终端帧队列
        /// </summary>
        public Queue<GPRSOLWQFrameDataEntity> GPRS_OLWQFrameData
        {
            get { return _GPRS_OLWQFrameData; }
            set { _GPRS_OLWQFrameData = value; }
        }

        private Queue<GPRSHydrantFrameDataEntity> _GPRS_HydrantFrameData = new Queue<GPRSHydrantFrameDataEntity>();
        /// <summary>
        /// 消防栓帧队列
        /// </summary>
        public Queue<GPRSHydrantFrameDataEntity> GPRS_HydrantFrameData
        {
            get { return _GPRS_HydrantFrameData; }
            set { _GPRS_HydrantFrameData = value; }
        }

        private Queue<GPRSPrectrlFrameDataEntity> _GPRS_PrectrlFrameData = new Queue<GPRSPrectrlFrameDataEntity>();
        /// <summary>
        /// 压力控制器帧队列
        /// </summary>
        public Queue<GPRSPrectrlFrameDataEntity> GPRS_PrectrlFrameData
        {
            get { return _GPRS_PrectrlFrameData; }
            set { _GPRS_PrectrlFrameData = value; }
        }

        private Queue<GPRSNoiseFrameDataEntity> _GPRS_NoiseFrameData = new Queue<GPRSNoiseFrameDataEntity>();
        /// <summary>
        /// 噪声远传控制器帧队列
        /// </summary>
        public Queue<GPRSNoiseFrameDataEntity> GPRS_NoiseFrameData
        {
            get { return _GPRS_NoiseFrameData; }
            set { _GPRS_NoiseFrameData = value; }
        }

        private Queue<GPRSWaterWorkerFrameDataEntity> _GPRS_WaterworkerFrameData = new Queue<GPRSWaterWorkerFrameDataEntity>();
        /// <summary>
        /// GPRS水厂帧队列
        /// </summary>
        public Queue<GPRSWaterWorkerFrameDataEntity> GPRS_WaterworkerFrameData
        {
            get { return _GPRS_WaterworkerFrameData; }
            set { _GPRS_WaterworkerFrameData = value; }
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

        /// <summary>
        /// 通用终端配置，用于帧数据解析
        /// </summary>
        public DataTable UniversalDataConfig = null;

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
