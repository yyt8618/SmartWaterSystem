using System;
using System.Threading;
using Common;
using BLL;
using Entity;

namespace SmartWaterSystem
{
    public enum SqlSyncType:uint
    {
        None = 0,
        SyncTerminal,
        SyncPreTerConfig,
        SyncUpdate_UniversalTerWayType,
        SyncUpdate_UniversalTerWayConfig
        //Sync
    }

    public class SQLSyncEventArgs : EventArgs
    {
        private SqlSyncType _type;
        /// <summary>
        /// 操作类型
        /// </summary>
        public SqlSyncType Type
        {
            get { return _type; }
        }

        private object _obj = null;
        public object obj
        {
            get { return _obj; }
        }

        private string _msg = "";
        public string Msg
        {
            get { return _msg; }
        }

        private int _result = -1;
        public int Result
        {
            get { return _result; }
        }

        private SQLSyncEventArgs()
        {
        }

        public SQLSyncEventArgs(SqlSyncType type,int result, string msg, object obj)
        {
            this._result = result;
            this._msg = msg;
            this._obj = obj;
            this._type = type;
        }
            
    }

    public delegate void SQLSyncEventHandler(object sender, SQLSyncEventArgs e);
    public class SQLSyncManager
    {
        private const uint eventcount = 5;

        public event SQLSyncEventHandler SQLSyncEvent;
        private IntPtr[] hEvent = new IntPtr[eventcount];

        Thread t = null;
        public SQLSyncManager()
        {
            for (int i = 0; i < eventcount; i++)
            {
                hEvent[i]=Win32.CreateEvent(IntPtr.Zero, false, false, null);
            }
        }

        ~SQLSyncManager()
        {
            Stop();
            for (int i = 0; i < eventcount; i++)
            {
                Win32.CloseHandle(hEvent[i]);
            }
        }

        private void OnSQLSyncEvent(SQLSyncEventArgs e)
        {
            if (SQLSyncEvent != null)
                SQLSyncEvent(this, e);
        }

        public bool Start()
        {
            t = new Thread(new ThreadStart(SQLSyncThread));
            t.Start();
            return true;
        }

        public void Stop()
        {
            Win32.SetEvent(hEvent[0]);
        }

        int result = -1;  //-1:执行失败;1:执行成功;0:无执行返回
        object obj = null;
        string msg = string.Empty;
        SQLSyncBLL syncBll = new SQLSyncBLL();

        MSMQEntity msmqEntity = new MSMQEntity();
        
        private void SQLSyncThread()
        {
            while (true)
            {
                uint evt = Win32.WaitForMultipleObjects(eventcount, hEvent, false, Win32.INFINITE);
                result = -1;  //-1:执行失败;1:执行成功;0:无执行返回
                obj = null;
                msg = string.Empty;
                if (evt != (uint)SqlSyncType.None)
                {
                    msmqEntity.MsgType = Entity.ConstValue.MSMQTYPE.SQL_Syncing;
                    GlobalValue.MSMQMgr.SendMessage(msmqEntity);
                }
                switch (evt)
                {
                    case (uint)SqlSyncType.None:
                        Thread.CurrentThread.Abort();
                        break;
                    case (uint)SqlSyncType.SyncTerminal:
                        {
                            if (SQLHelper.TryConn(SQLHelper.ConnectionString))
                            {
                                bool res = syncBll.UpdateSQL_Terminal();
                                if (res)
                                    result = 1;
                                else
                                    result = -1;
                            }
                            else
                            {
                                result = 1;
                            }
                        }
                        break;
                    case (uint)SqlSyncType.SyncPreTerConfig:
                        {
                            if (SQLHelper.TryConn(SQLHelper.ConnectionString))
                            {
                                bool res = syncBll.UpdateSQL_PreTerConfig();
                                if (res)
                                    result = 1;
                                else
                                    result = -1;
                            }
                            else
                            {
                                result = 1;
                            }
                        }
                        break;
                    case (uint)SqlSyncType.SyncUpdate_UniversalTerWayType:
                        {
                            if (SQLHelper.TryConn(SQLHelper.ConnectionString))
                            {
                                bool res = syncBll.Update_UniversalTerWayType();
                                if (res)
                                    result = 1;
                                else
                                    result = -1;
                            }
                            else
                            {
                                result = 1;
                            }
                        }
                        break;
                    case (uint)SqlSyncType.SyncUpdate_UniversalTerWayConfig:
                        {
                            if (SQLHelper.TryConn(SQLHelper.ConnectionString))
                            {
                                bool res = syncBll.UpdateSQL_UniversalTerWayConfig();
                                if (res)
                                    result = 1;
                                else
                                    result = -1;
                            }
                            else
                            {
                                result = 1;
                            }
                        }
                        break;
                }
                
                OnSQLSyncEvent(new SQLSyncEventArgs((SqlSyncType)evt, result, msg, obj));
            }
        }

        public void Send(SqlSyncType type)
        {
            Win32.SetEvent(hEvent[(int)type]);
        }

    }
}
