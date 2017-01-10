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
        GetAlarmType,
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
        InsertAlarm,
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

        private const int eventcount = 14;
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
                            List<SendPackageEntity> lstDbCmd = dataBll.GetGPRSParm();
                            lock (GlobalValue.Instance.lstClientLock)
                            {
                                for(int i =0; i < GlobalValue.Instance.lstClient.Count;i++)//先移除已有的从数据库添加的数据，再重新添加
                                {
                                    for (int j = GlobalValue.Instance.lstClient[i].lstWaitSendCmd.Count-1;j>=0; j--)
                                    {
                                        if (GlobalValue.Instance.lstClient[i].lstWaitSendCmd[j].TableId > 0)
                                        {
                                             GlobalValue.Instance.lstClient[i].lstWaitSendCmd.RemoveAt(j);
                                        }
                                    }
                                }
                            }
                            foreach(SendPackageEntity sendPack in lstDbCmd)
                            {
                                GlobalValue.Instance.lstClientAdd(sendPack);
                            }
                            
                            #endregion
                        }
                        break;
                    case (uint)SQLType.GetAlarmType:
                        {
                            #region 获取报警类型
                            GlobalValue.Instance.lstAlarmType = dataBll.GetAlarmType();
                            #endregion
                        }
                        break;
                    case (uint)SQLType.InsertAlarm:
                        {
                            #region 保存报警数据帧至数据库
                            result = dataBll.InsertAlarmData(GlobalValue.Instance.GPRS_AlarmFrameData, out msg);
                            #endregion
                        }
                        break;
                    case (uint)SQLType.UpdateSendParmFlag:
                        {
                            #region 更新GPRS下送帧标志
                            result = dataBll.UpdateGPRSParmFlag(GlobalValue.Instance.lstSendedCmdId);
                            if (result == 1)
                            {
                                lock(GlobalValue.Instance.lstClientLock)
                                {
                                    foreach (GPRSCmdFlag flag in GlobalValue.Instance.lstSendedCmdId)
                                    {
                                        for (int i = 0; i < GlobalValue.Instance.lstClient.Count; i++)
                                        {
                                            for (int j = GlobalValue.Instance.lstClient[i].lstWaitSendCmd.Count - 1; j >= 0; j--)
                                            {
                                                if (flag.TableId == GlobalValue.Instance.lstClient[i].lstWaitSendCmd[j].TableId)
                                                {
                                                    GlobalValue.Instance.lstClient[i].lstWaitSendCmd.RemoveAt(j);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                GlobalValue.Instance.lstSendedCmdId.Clear();
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
                        {
                            #region 保存流量数据
                            result = dataBll.InsertGPRSFlowData(GlobalValue.Instance.GPRS_FlowFrameData, out msg);
                            #endregion
                        }
                        break;
                    case (uint)SQLType.InsertPrectrlValue:
                        {
                            #region 保存压力控制器数据
                            result = dataBll.InsertGPRSPrectrlData(GlobalValue.Instance.GPRS_PrectrlFrameData, out msg);
                            #endregion
                        }
                        break;
                    case (uint)SQLType.InsertUniversalValue:
                        {
                            #region 保存通用终端数据
                            result = dataBll.InsertGPRSUniversalData(GlobalValue.Instance.GPRS_UniversalFrameData, out msg);
                            #endregion
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
                            #region 保存消防栓数据帧至数据库
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
