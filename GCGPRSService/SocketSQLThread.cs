using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Common;
using BLL;

namespace GCGPRSService
{
    public enum SQLType : uint
    {
        None,
        GetSendParm,
        UpdateSendParmFlag,
        InsertPreValue,
        InsertFlowValue,

        InsertUniversalValue
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

        private const int eventcount = 6;
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
                            GlobalValue.Instance.lstGprsCmd = dataBll.GetGPRSParm();
                            #endregion
                        }
                        break;
                    case (uint)SQLType.UpdateSendParmFlag:
                        {
                            #region 更新GPRS下送帧标志
                            result = dataBll.UpdateGPRSParmFlag(GlobalValue.Instance.lstSendedCmdId);
                            if (result == 1)
                            {
                                for (int i = 0; i < GlobalValue.Instance.lstSendedCmdId.Count; i++)
                                {
                                    GlobalValue.Instance.lstGprsCmd.RemoveAt(GlobalValue.Instance.lstSendedCmdId[i].Index);
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
                    case (uint)SQLType.InsertUniversalValue:
                        {
                            result = dataBll.InsertGPRSUniversalData(GlobalValue.Instance.GPRS_UniversalFrameData, out msg);
                        }
                        break;
                        
                }
                OnSQLEvent(new SQLNotifyEventArgs((SQLType)evt,result,msg));
            }
        }
    }
}
