using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Common;
using BLL;
using Entity;

namespace GCGPRSService
{
    public enum SQLType : uint
    {
        None,
        GetSendParm,
        GetUniversalConfig,
        UpdateSendParmFlag,
        InsertPreValue,

        InsertFlowValue,
        InsertUniversalValue,
        InsertOLWQValue,
        InsertHydrantValue,
        InsertPrectrlValue,

        InsertNoiseValue,
        InsertWaterworkerValue,
    }

    public class SQLNotifyEventArgs : EventArgs
    {
        private SQLType sqlType;
        /// <summary>
        /// 数据库操作类型
        /// </summary>
        public SQLType SQLType
        {
            get { return sqlType; }
        }

        private int _result=-1;
        /// <summary>
        /// 结果 (-1:执行失败;1:执行成功;0:无执行返回)
        /// </summary>
        public int Result
        {
            get { return _result; }
            set { _result = value; }
        }

        private string _msg = "";
        /// <summary>
        /// 执行结果
        /// </summary>
        public string Msg
        {
            get { return _msg; }
            set { _msg = value; }
        }

        public SQLNotifyEventArgs(SQLType type, int result, string msg)
        {
            this.sqlType = type;
            this._result = result;
            this._msg = msg;
        }
    }

    public delegate void SQLHandle(object sender,SQLNotifyEventArgs e);

    public class SocketSQLManager
    {
        private NLog.Logger logger = NLog.LogManager.GetLogger("SocketSQLThread");
        TerminalDataBLL dataBll = new TerminalDataBLL();

        private const int eventcount = 12;
        private IntPtr[] hEvent = new IntPtr[eventcount];
        public event SQLHandle SQLEvent;

        public SocketSQLManager()
        {
            for (int i = 0; i < eventcount; i++)
            {
                hEvent[i] = Win32.CreateEvent(IntPtr.Zero, false, false, null);
            }
        }

        ~SocketSQLManager()
        {
            for (int i = 0; i < eventcount; i++)
            {
                Win32.CloseHandle(hEvent[i]);
            }
        }

        public virtual void OnSQLEvent(SQLNotifyEventArgs e)
        {
            if (SQLEvent != null)
                SQLEvent(this, e);
        }

        public void Start()
        {
            Thread t = new Thread(new ThreadStart(SocketSQLThread));
            t.IsBackground = true;
            t.Start();
        }

        public void Stop()
        {
            Win32.SetEvent(hEvent[0]);
        }

        public void Send(SQLType type)
        {
            Win32.SetEvent(hEvent[(int)type]);
        }

        private void SocketSQLThread()
        {
            while (true)
            {
                uint evt=Win32.WaitForMultipleObjects(eventcount,hEvent,false,Win32.INFINITE);
                int result = -1;  //-1:执行失败;1:执行成功;0:无执行返回
                string msg = "";

                switch (evt)
                {
                    case (uint)SQLType.None:
                        Thread.CurrentThread.Abort();
                        break;
                    case (uint)SQLType.GetSendParm:
                        {
                            #region 获取GPRS下送帧
                            lock (GlobalValue.Instance.lstGprsCmdLock)
                            {
                                GlobalValue.Instance.lstGprsCmd = dataBll.GetGPRSParm();
                            }
                            #endregion
                        }
                        break;
                    case (uint)SQLType.UpdateSendParmFlag:
                        {
                            #region 更新GPRS下送帧标志
                            result = dataBll.UpdateGPRSParmFlag(GlobalValue.Instance.lstSendedCmdId);
                            if (result == 1)
                            {
                                List<GPRSCmdEntity> lstTmp = new List<GPRSCmdEntity>();
                                for (int i = 0; i < GlobalValue.Instance.lstGprsCmd.Count; i++)
                                {
                                    bool exist = false;
                                    foreach(GPRSCmdFlag flag in GlobalValue.Instance.lstSendedCmdId)
                                    {
                                        if(flag.TableId == GlobalValue.Instance.lstGprsCmd[i].TableId) 
                                        {
                                            exist = true;
                                            break;
                                        }
                                    }
                                    if (!exist)
                                    {
                                        lstTmp.Add(GlobalValue.Instance.lstGprsCmd[i]);
                                    }
                                }
                                GlobalValue.Instance.lstSendedCmdId.Clear();
                                lock (GlobalValue.Instance.lstGprsCmdLock)
                                {
                                    GlobalValue.Instance.lstGprsCmd = lstTmp;
                                }
                            }
                            #endregion
                        }
                        break;
                    case (uint)SQLType.InsertPreValue:
                            #region 保存压力数据帧至数据库
                            result = dataBll.InsertGPRSPreData(GlobalValue.Instance.GPRS_PreFrameData, out msg);
                            #endregion
                                break;
                    case (uint)SQLType.InsertFlowValue:
                        result = dataBll.InsertGPRSFlowData(GlobalValue.Instance.GPRS_FlowFrameData, out msg);
                        break;
                    case (uint)SQLType.InsertPrectrlValue:
                        result = dataBll.InsertGPRSPrectrlData(GlobalValue.Instance.GPRS_PrectrlFrameData, out msg);
                        break;
                    case (uint)SQLType.InsertUniversalValue:
                        {
                            result = dataBll.InsertGPRSUniversalData(GlobalValue.Instance.GPRS_UniversalFrameData, out msg);
                        }
                        break;
                    case (uint)SQLType.GetUniversalConfig:
                        {
                            #region 获取解析帧的配置数据
                            GlobalValue.Instance.UniversalDataConfig = dataBll.GetUniversalDataConfig(TerType.UniversalTer);
                            #endregion
                        }
                        break;
                    case (uint)SQLType.InsertOLWQValue:
                        {
                            #region 保存水质终端数据帧至数据库
                            result = dataBll.InsertGPRSOLWQData(GlobalValue.Instance.GPRS_OLWQFrameData, out msg);
                            #endregion
                        }
                        break;
                    case (uint)SQLType.InsertHydrantValue:
                        {
                            #region 保存水质终端数据帧至数据库
                            result = dataBll.InsertGPRSHydrantData(GlobalValue.Instance.GPRS_HydrantFrameData, out msg);
                            #endregion
                        }
                        break;
                    case (uint)SQLType.InsertNoiseValue:
                        {
                            #region 保存噪声数据帧至数据库
                            result = dataBll.InsertGPRSNoiseData(GlobalValue.Instance.GPRS_NoiseFrameData, out msg);
                            #endregion
                        }
                        break;
                    case (uint)SQLType.InsertWaterworkerValue:
                        {
                            #region 保存水厂数据帧至数据库
                            result = dataBll.InsertWaterworkerData(GlobalValue.Instance.GPRS_WaterworkerFrameData, out msg);
                            #endregion
                        }
                        break;
                }
                OnSQLEvent(new SQLNotifyEventArgs((SQLType)evt,result,msg));
            }
        }
    }
}
