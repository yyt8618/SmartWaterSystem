using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using Common;
using Entity;
using System.Data;
using System.Text;
using SmartWaterSystem;

namespace GCGPRSService
{
    public class SocketEventArgs : EventArgs
    {
        private SocketEntity _jsonmsg;// = "";
        public SocketEntity JsonMsg
        {
            get { return _jsonmsg; }
            set { _jsonmsg = value; }
        }

        public SocketEventArgs(Entity.ConstValue.MSMQTYPE MsgType, string msg)
        {
            SocketEntity msmqEntity = new SocketEntity();
            msmqEntity.MsgType = MsgType;
            msmqEntity.Msg = msg;
            _jsonmsg = msmqEntity; // JsonConvert.SerializeObject(msmqEntity);
        }

        public SocketEventArgs(Entity.ConstValue.MSMQTYPE MsgType, SocketEntity msg)
        {
            msg.MsgType = MsgType;
            _jsonmsg = msg;
        }

        public SocketEventArgs(string msg)
        {
            SocketEntity msmqEntity = new SocketEntity();
            msmqEntity.MsgType = Entity.ConstValue.MSMQTYPE.Msg_Socket;
            msmqEntity.Msg = msg;
            _jsonmsg = msmqEntity;// JsonConvert.SerializeObject(msmqEntity);
        }

        public SocketEventArgs(string msg,Entity.ConstValue.MSMQTYPE type)
        {
            SocketEntity msmqEntity = new SocketEntity();
            msmqEntity.MsgType = type;
            msmqEntity.Msg = msg;
            _jsonmsg = msmqEntity;// JsonConvert.SerializeObject(msmqEntity);
        }

        public SocketEventArgs(SocketEntity msg)
        {
            _jsonmsg = msg;
        }
    }

    public class SendObject
    {
        // Client socket.     
        public Socket workSocket = null;
        //Is Last Frame
        public bool IsFinal = true;
        public Entity.ConstValue.DEV_TYPE DevType;
        public short DevID;
    }

    //public delegate void cmdEventHandle(object sender, SocketEventArgs e);

    public class SocketManager
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("SocketService");
        public ManualResetEvent allDone = new ManualResetEvent(false);
        //public event cmdEventHandle cmdEvent;
        Thread t_socket;
        Thread t_Interval;
        Socket listener;
        bool isRunning = false;
        List<CallSocketEntity> lstClient = new List<CallSocketEntity>();  //在线客户端列表
        int maxSmartClient = 8;   //SmartWaterSystem.exe 的最大socket连接数
        object obj_smartsocket = new object();      //SmartWaterSystem.exe的socket连接列表锁
        List<SmartSocketEntity> lstSmartClient = new List<SmartSocketEntity>(); //SmartWaterSystem.exe 的socket连接列表

        int SQL_Interval = 3 * 63;  //数据更新时间间隔(second)
        DateTime SQLSync_Time = DateTime.Now.AddHours(-1);  //数据库同步时间,-1:才开启，马上同步一次数据

        int OnLineState_Interval = 5 * 60; //终端在线状态更新时间间隔(second)
        int CheckThread_Interval = 2 * 60; //检查线程状态间隔(second)

        bool SL651AllowOnLine = false;  //SL651协议终端是否在线,默认不在线

        public void OnSendMsg(SocketEventArgs e)
        {
            lock (obj_smartsocket)
            {
                foreach (SmartSocketEntity smartsock in lstSmartClient)
                {
                    string msg = JSONSerialize.JsonSerialize_Newtonsoft(e.JsonMsg);
                    if (!SocketSend(smartsock.ClientSocket, msg, false))
                    {
                        smartsock.MsgBuff.Add(msg);  //缓存发送失败的消息,在下次心跳到来的时候重发
                    }
                }
            }
        }

        public void T_Listening()
        {
            GlobalValue.Instance.lstStartRecord.Add(DateTime.Now.ToString() + " 开启数据库线程!");
            GlobalValue.Instance.SocketSQLMag.SQLEvent += new SQLHandle(sqlmanager_SQLEvent);
            GlobalValue.Instance.SocketSQLMag.Start();

            t_Interval = new Thread(new ThreadStart(Interval_Thread));
            t_Interval.IsBackground = true;
            t_Interval.Start();

            CheckThread_Interval = 1;  //开启socket线程

            SocketEntity msmqEntity = new SocketEntity();
            msmqEntity.lstOnLine = new List<OnLineTerEntity>();
            OnSendMsg(new SocketEventArgs(ConstValue.MSMQTYPE.Data_OnLineState, msmqEntity));
        }

        private void Interval_Thread()
        {
            while (true)
            {
                Thread.Sleep(1000);
                if (SQL_Interval-- == 0)
                {
                    TimeSpan ts = DateTime.Now - SQLSync_Time;
                    if (Math.Abs(ts.TotalSeconds) > 15)
                    {
                        SQL_Interval = 3 * 63;
                        GlobalValue.Instance.SocketSQLMag.Send(SQLType.GetSendParm); //获得上传参数
                        GlobalValue.Instance.SocketSQLMag.Send(SQLType.GetUniversalConfig); //获取解析帧的配置数据
                    }
                    else
                    {
                        SQL_Interval = 20;
                    }
                }
                if (OnLineState_Interval-- == 0)
                {
                    OnLineState_Interval = 5*60;
                    SocketEntity msmqEntity = new SocketEntity();
                    msmqEntity.lstOnLine = new List<OnLineTerEntity>();
                    foreach (CallSocketEntity client in lstClient)
                    {
                        //如果重试次数大于0并且发送时间与当前时间相差不超过一天，保留
                        if (client.lstWaitSendCmd != null && client.lstWaitSendCmd.Count > 0)
                        {
                            List<SendPackageEntity> lst_save_Pack = new List<SendPackageEntity>();
                            for (int i = 0; i < client.lstWaitSendCmd.Count; i++)
                            {
                                TimeSpan ts = DateTime.Now - client.lstWaitSendCmd[i].SendTime;
                                if ((client.lstWaitSendCmd[i].SendCount > -1) && (Math.Abs(ts.TotalHours) < 24))
                                    lst_save_Pack.Add(client.lstWaitSendCmd[i]);
                            }
                            client.lstWaitSendCmd = lst_save_Pack;
                        }
                        //在线检查
                        bool add = false;
                        add = SocketHelper.IsSocketConnected_Poll(client.ClientSocket);

                        if (add && client.TerId!=-1)
                        {
                            msmqEntity.lstOnLine.Add(new OnLineTerEntity(client.DevType, client.TerId));
                        }
                    }
                    OnSendMsg(new SocketEventArgs(ConstValue.MSMQTYPE.Data_OnLineState, msmqEntity));

                    //检查lstSmartClient保留在线的socket客户端
                    lock(obj_smartsocket)
                    {
                        List<SmartSocketEntity> lsttmp = new List<SmartSocketEntity>();
                        foreach (SmartSocketEntity smartsock in lstSmartClient)
                        {
                            if (SocketHelper.IsSocketConnected_Poll(smartsock.ClientSocket))
                            {
                                lsttmp.Add(smartsock);
                            }
                        }
                        lstSmartClient = lsttmp;
                    }
                }
                if (CheckThread_Interval-- == 0)
                {
                    CheckThread_Interval = 2 * 60;
                    if (t_socket == null || (t_socket != null && !t_socket.IsAlive))
                    {
                        GlobalValue.Instance.lstStartRecord.Add(DateTime.Now.ToString() + " 开启Socket监听线程!");
                        isRunning = true;
                        t_socket = new Thread(new ThreadStart(StartListening));
                        t_socket.IsBackground = true;
                        t_socket.Start();
                    }
                }
            }
        }

        void sqlmanager_SQLEvent(object sender, SQLNotifyEventArgs e)
        {
            if (e.SQLType == SQLType.InsertPreValue)
            {
                if (1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs("压力数据保存成功"));
                }
                else if(-1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs("压力数据保存失败:"+e.Msg));
                }
            }
            else if (e.SQLType == SQLType.InsertFlowValue)
            {
                if (1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs("流量数据保存成功"));
                }
                else if (-1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs("流量数据保存失败:" + e.Msg));
                }
            }
            else if (e.SQLType == SQLType.InsertPrectrlValue)
            {
                if (1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs("压力控制器数据保存成功"));
                }
                else if (-1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs("压力控制器数据保存失败:" + e.Msg));
                }
            }
            else if (e.SQLType == SQLType.InsertUniversalValue)
            {
                if (1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs("通用终端数据保存成功"));
                }
                else if (-1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs("通用终端数据保存失败:" + e.Msg));
                }
            }
            else if (e.SQLType == SQLType.InsertOLWQValue)
            {
                if (1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs("水质终端数据保存成功"));
                }
                else if (-1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs("水质终端数据保存失败:" + e.Msg));
                }
            }
            else if (e.SQLType == SQLType.InsertNoiseValue)
            {
                if (1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs("噪声数据保存成功"));
                }
                else if (-1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs("噪声数据保存失败:" + e.Msg));
                }
            }
            else if (e.SQLType == SQLType.InsertWaterworkerValue)
            {
                if (1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs("水厂数据保存成功"));
                }
                else if (-1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs("水厂数据保存失败:" + e.Msg));
                }
            }
        }

        public void Close()
        {
            try
            {
                isRunning = false;
                if (listener != null )
                {
                    listener.Shutdown(SocketShutdown.Both);
                    Thread.Sleep(10);
                    listener.Close();
                    listener.Dispose();
                }

                if (t_socket != null && t_socket.IsAlive)
                    t_socket.Abort();

                if (t_Interval != null && t_Interval.IsAlive)
                    t_Interval.Abort();
            }
            catch(Exception e)
            {
                logger.ErrorException("Socket Close", e);
            }
        }

        private void StartListening()
        {
            Settings.Instance.Read();
            string ip = Settings.Instance.GetString(SettingKeys.GPRS_IP);
            if (string.IsNullOrEmpty(ip))
            {
                GlobalValue.Instance.lstStartRecord.Add("GPRS远传服务停止,请设置IP与端口号!");
                t_socket.Abort();
                return;
            }
            if(string.IsNullOrEmpty(Settings.Instance.GetString(SettingKeys.GPRS_PORT)))
            {
                GlobalValue.Instance.lstStartRecord.Add("GPRS远传服务停止,请设置IP与端口号!");
                t_socket.Abort();
                return;
            }
            //Thread.Sleep(10 * 1000);
            int port =Settings.Instance.GetInt(SettingKeys.GPRS_PORT);
            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
            // Create a TCP/IP socket.     
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.ReceiveTimeout = 30 * 1000;  //设置超时
            listener.SendTimeout = 30 * 1000;
            try
            {
                //OnSendMsg(new SocketEventArgs("监听时的线程:" + Thread.CurrentThread.ManagedThreadId.ToString()));
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (isRunning)
                {
                    // Set the event to nonsignaled state.     
                    allDone.Reset();
                    // Start an asynchronous socket to listen for connections.
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                    // Wait until a connection is made before continuing.     
                    allDone.WaitOne();
                }
            }
            catch (SocketException sockEx)
            {
                GlobalValue.Instance.lstStartRecord.Add(DateTime.Now.ToString() + " GPRS远传服务发生异常，将停止! Exp:" + sockEx.Message);
                logger.ErrorException("StartListening", sockEx);
                if (t_socket != null && t_socket.IsAlive)
                    t_socket.Abort();
            }
            catch (Exception e)
            {
                GlobalValue.Instance.lstStartRecord.Add(DateTime.Now.ToString() + " GPRS远传服务发生异常，将停止!");
                logger.ErrorException("StartListening", e);
                if (t_socket != null && t_socket.IsAlive)
                    t_socket.Abort();
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            StateObject state = new StateObject();
            Socket handler = null;
            try
            {   
                allDone.Set();
                Socket listener = (Socket)ar.AsyncState;
                handler = listener.EndAccept(ar);

                OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + " 客户端" + handler.RemoteEndPoint.ToString() + "上线"));

                state.workSocket = handler;
            }
            catch (Exception ex)
            {
                OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + " 接收错误:" + ex.Message));
                logger.ErrorException("AcceptCallback",ex);
            }
            finally
            {
                try {
                    if(handler!=null)
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                    
                }
                catch { }
            }
        }

        private void ReadCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            int bytesRead = 0;
            try
            {
                // Read data from the client socket. 
                if (!SocketHelper.IsSocketConnected_Poll(handler))
                    return;
                bytesRead = handler.EndReceive(ar);
                if (bytesRead > 0)
                {
                    //判断是否是SmartWater的连接
                    bool isclientconn = false;
                    for (int i = 0; i < bytesRead -SocketHelper.SocketByteSplit.Length; i++)
                    {
                        bool btmp = true;
                        for (int j = 0; j < SocketHelper.SocketByteSplit.Length; j++)  //判断是否包含SmartWaterHead标记,如果包含则认为是SmartWater的连接
                        {
                            if (state.buffer[i + j] != SocketHelper.SocketByteSplit[j])
                            {
                                btmp = false;
                                break;
                            }
                        }
                        if (btmp)
                        {
                            isclientconn = true;
                            break;
                        }
                    }
                    #region SmartSocket
                    if (isclientconn)   //如果是SmartWaterSystem程序的socket连接,则保存socket连接对象,并解析数据
                    {
                        try
                        {
                            for (int i = 0; i < bytesRead; i++)
                            {
                                state.lstBuffer.Add(state.buffer[i]);
                            }
                            List<byte> lstBuffercopy = new List<byte>(state.lstBuffer);
                            string[] strmsgs = SocketHelper.SplitSocketMsg(ref state.lstBuffer, ref state.msgpart);
                            int smartindex = -1;
                            if (strmsgs != null && strmsgs.Length > 0)
                            {
                                foreach (string strtmp in strmsgs)
                                {
                                    if (!string.IsNullOrEmpty(strtmp))
                                    {
                                        smartindex = strtmp.IndexOf(GlobalValue.Instance.SmartWaterHeartBeatName);
                                        if (smartindex>-1) //心跳包
                                        {
                                            #region 心跳包
                                            IPAddress ip = ((System.Net.IPEndPoint)handler.RemoteEndPoint).Address;
                                            string strip = ip.ToString();
                                            string SmartID = strtmp.Substring(GlobalValue.Instance.SmartWaterHeartBeatName.Length);  //获取ID
                                            bool existsmartsocket = false;
                                            for (int i = 0; i < lstSmartClient.Count; i++)  //是否存在已连接的对象
                                            {
                                                if (lstSmartClient[i].IP == strip && lstSmartClient[i].ID == SmartID)
                                                {
                                                    existsmartsocket = true;
                                                    lstSmartClient[i].ClientSocket = handler;
                                                    if (lstSmartClient[i].MsgBuff.Count > 0)  //是否有缓存的消息,如果有,将消息发送出去
                                                    {
                                                        List<string> lsttmpmsg = new List<string>();   //将发送失败的消息存至该变量,以便于重发
                                                        foreach (string str in lstSmartClient[i].MsgBuff)
                                                        {
                                                            if (!string.IsNullOrEmpty(str))
                                                            {
                                                                if (!SocketSend(handler, str, false))   //发送缓存的消息
                                                                {
                                                                    lsttmpmsg.Add(str);
                                                                }
                                                            }
                                                        }
                                                        lstSmartClient[i].MsgBuff = lsttmpmsg;  //将发送失败消息保存,以便重发
                                                    }
                                                    break;
                                                }
                                            }

                                            if (!existsmartsocket)
                                            {
                                                lock (obj_smartsocket)
                                                {
                                                    if (lstSmartClient.Count > maxSmartClient) //不能超过最大连接数,如果大于最大连接数的话，则找一个失效的连接
                                                    {
                                                        bool bsave = false;
                                                        for (int i = 0; i < lstSmartClient.Count; i++)
                                                        {
                                                            if (!SocketHelper.IsSocketConnected_Poll(lstSmartClient[i].ClientSocket))
                                                            {
                                                                lstSmartClient[i].ClientSocket = handler;
                                                                lstSmartClient[i].IP = strip;
                                                                lstSmartClient[i].ID = SmartID;
                                                                lstSmartClient[i].MsgBuff.Clear();
                                                                bsave = true;
                                                            }
                                                        }
                                                        if (!bsave)
                                                        {
                                                            //达到最大连接数提示
                                                            SocketEntity mEntity = new SocketEntity();
                                                            mEntity.MsgType = ConstValue.MSMQTYPE.Msg_Public;
                                                            mEntity.Msg = "已达到最大连接数,拒接连接";
                                                            SocketSend(handler, JSONSerialize.JsonSerialize_Newtonsoft(mEntity), false);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lstSmartClient.Add(new SmartSocketEntity(handler, strip, SmartID));
                                                        foreach (string startrec in GlobalValue.Instance.lstStartRecord)  //将启动的信息发送给新上线的客户端
                                                        {
                                                            SocketEntity mEntity = new SocketEntity();
                                                            mEntity.MsgType = ConstValue.MSMQTYPE.Msg_Public;
                                                            mEntity.Msg = startrec;
                                                            SocketSend(handler, JSONSerialize.JsonSerialize_Newtonsoft(mEntity), false);
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            try
                                            {
                                                //处理命令消息
                                                SocketEntity sockMsg = JSONSerialize.JsonDeserialize_Newtonsoft<SocketEntity>(strtmp);
                                                ReceiveMsg(sockMsg);
                                            }
                                            catch (Exception ex)
                                            {
                                                logger.ErrorException("处理命令消息", ex);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ;
                        }
                        try
                        {
                            if (handler != null && SocketHelper.IsSocketConnected_Poll(handler))
                                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                        }
                        catch { };
                        return;
                    }
                    #endregion

                    List<byte> packageBytes = new List<byte>();
                    List<byte> ReceiveBytes = new List<byte>();

                    for (int i = 0; i < bytesRead; i++)
                    {
                        ReceiveBytes.Add(state.buffer[i]);
                    }

                    string str_source = ConvertHelper.ByteToString(state.buffer, bytesRead);
                    OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + " 接收客户端" + handler.RemoteEndPoint.ToString() + ", 收到(原始)数据:" + str_source));

                    int index = 0;
                    while (index < bytesRead)
                    {
                        packageBytes.Add(ReceiveBytes[index]);
                        packageBytes = FormatHelper.CheckHead(packageBytes);  //检查数据头
                        #region 68开头协议
                        if (ReceiveBytes[index] == PackageDefine.EndByte && packageBytes.Count >= PackageDefine.MinLenth)
                        {
                            byte[] arr = packageBytes.ToArray();
                            int len = BitConverter.ToInt16(new byte[] { arr[9], arr[8] }, 0);  //数据域长度
                            Package pack;
                            
                            if ((PackageDefine.MinLenth + len == arr.Length) && Package.TryParse(arr, out pack))//找到结束字符并且是完整一帧
                            {
                                bool bNeedCheckTime = false;  //是否需要校时
                                List<Package> lstCommandPack = new List<Package>();
                                Package response = new Package();

                                if (pack.CommandType == CTRL_COMMAND_TYPE.RESPONSE_BY_SLAVE)  //接受到应答,判断是否D11是否为1,如果为0,表示没有数据需要读
                                {
                                    #region 处理响应帧
                                    OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + " 收到响应帧:" + str_source));
                                    foreach (CallSocketEntity callentity in lstClient)
                                    {
                                        if (callentity.DevType == pack.DevType && callentity.TerId != -1 && callentity.TerId == pack.DevID && callentity.lstWaitSendCmd != null)
                                        {
                                            List<SendPackageEntity> lst_save_pack = new List<SendPackageEntity>();
                                            for (int i = 0; i < callentity.lstWaitSendCmd.Count; i++)  //收到响应帧
                                            {
                                                if (callentity.lstWaitSendCmd[i].SendPackage.C0 == pack.C0 &&
                                                    callentity.lstWaitSendCmd[i].SendPackage.C1 == pack.C1 &&
                                                    callentity.lstWaitSendCmd[i].SendPackage.Data == pack.Data)
                                                {
                                                    ;
                                                }
                                                else
                                                    lst_save_pack.Add(callentity.lstWaitSendCmd[i]);
                                            }
                                            callentity.lstWaitSendCmd = lst_save_pack;
                                        }
                                    }

                                    if (GlobalValue.Instance.lstGprsCmd != null && GlobalValue.Instance.lstGprsCmd.Count > 0)
                                    {
                                        for (int j = 0; j < GlobalValue.Instance.lstGprsCmd.Count; j++)
                                        {
                                            if (GlobalValue.Instance.lstGprsCmd[j].DeviceId == pack.DevID && (byte)GlobalValue.Instance.lstGprsCmd[j].DevTypeId == (byte)pack.DevType && (byte)GlobalValue.Instance.lstGprsCmd[j].FunCode == pack.C1)
                                            {
                                                if (GlobalValue.Instance.lstGprsCmd[j].TableId == -1)  //-1用在生成的自动校时
                                                {
                                                    lock (GlobalValue.Instance.lstGprsCmdLock)
                                                        {   GlobalValue.Instance.lstGprsCmd.RemoveAt(j); }  //生成的自动校时直接删除
                                                }
                                                else
                                                {
                                                    GPRSCmdFlag flag = new GPRSCmdFlag();
                                                    flag.TableId = GlobalValue.Instance.lstGprsCmd[j].TableId;
                                                    GlobalValue.Instance.lstGprsCmd[j].SendedFlag = 1;  //标记为已发送，防止后边重发
                                                    flag.SendCount = 0;  //0:成功发送,收到响应帧
                                                    GlobalValue.Instance.lstSendedCmdId.Add(flag);
                                                }
                                            }
                                            //失败次数多于3次,则删除
                                            if (GlobalValue.Instance.lstGprsCmd[j].SendedCount > 3)
                                            {
                                                if (GlobalValue.Instance.lstGprsCmd[j].TableId == -1)  //-1用在生成的自动校时
                                                {
                                                    lock (GlobalValue.Instance.lstGprsCmdLock)
                                                    { GlobalValue.Instance.lstGprsCmd.RemoveAt(j); }  //生成的自动校时直接删除
                                                }
                                                else
                                                {
                                                    GPRSCmdFlag flag = new GPRSCmdFlag();
                                                    flag.TableId = GlobalValue.Instance.lstGprsCmd[j].TableId;
                                                    flag.SendCount = GlobalValue.Instance.lstGprsCmd[j].SendedCount;  //未收到响应帧,发送次数超过限制
                                                    GlobalValue.Instance.lstSendedCmdId.Add(flag);
                                                }
                                            }
                                        }

                                    }
                                    GlobalValue.Instance.SocketSQLMag.Send(SQLType.UpdateSendParmFlag);
                                    #endregion
                                }
                                else if (pack.CommandType == CTRL_COMMAND_TYPE.REQUEST_BY_SLAVE)//接收到的数据帧
                                {
                                    string str_frame = ConvertHelper.ByteToString(arr, arr.Length);
#if debug
                                    OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + " 收到帧数据:" + str_frame));
#else
                                    OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + " 收到帧数据"));
#endif
                                    #region 解析数据
                                    if (pack.ID3 == (byte)Entity.ConstValue.DEV_TYPE.Data_CTRL)
                                    {
                                        #region 压力终端
                                        bool addtion_voldata = false;   //是否在数据段最后增加了两个字节的电压数据
                                        if (pack.C1 == (byte)GPRS_READ.READ_PREDATA)  //从站向主站发送压力采集数据
                                        {
                                            int dataindex = (pack.DataLength - 2 - 1) % 8;
                                            if (dataindex != 0)
                                            {
                                                if (dataindex == 2)
                                                {
                                                    dataindex = (pack.DataLength - 2 - 1 - 2) / 8;
                                                    addtion_voldata = true;
                                                }
                                                else
                                                {
                                                    throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(2+1+8*n)或(2+1+8*n+2)规则");  //GPRS远程压力终端在数据段最后增加两个字节的电压数据
                                                }
                                            }
                                            else
                                                dataindex = (pack.DataLength - 2 - 1) / 8;

                                            string alarm = "";
                                            int preFlag = 0;

                                            //报警
                                            /*
                                             * A0—压力1上限报警。
                                             * A1—压力1下限报警。
                                             * A2—压力2上限报警。
                                             * A3—压力2下限报警。
                                             * A4—压力1斜率上限报警。
                                             * A5—压力1斜率下限报警。
                                             * A6—压力2斜率上限报警。
                                             * A7—压力2斜率下限报警。
                                             * A8～A15—备用
                                             */

                                            if ((pack.Data[1] & 0x01) == 1)  //压力1上限报警
                                                alarm += "压力1上限报警";
                                            else if (((pack.Data[1] & 0x02) >> 1) == 1)   //压力1下限报警
                                                alarm += "压力1下限报警";
                                            else if (((pack.Data[1] & 0x04) >> 2) == 1)   //压力2上限报警
                                                alarm += "压力2上限报警";
                                            else if (((pack.Data[1] & 0x08) >> 3) == 1)  //压力2下限报警
                                                alarm += "压力2下限报警";
                                            else if (((pack.Data[1] & 0x10) >> 4) == 1)   //压力1斜率上限报警
                                                alarm += "压力1斜率上限报警";
                                            else if (((pack.Data[1] & 0x20) >> 5) == 1)  //压力1斜率下限报警
                                                alarm += "压力1斜率下限报警";
                                            else if (((pack.Data[1] & 0x40) >> 6) == 1)  //压力2斜率上限报警
                                                alarm += "压力2斜率上限报警";
                                            else if (((pack.Data[1] & 0x80) >> 7) == 1)  //压力2斜率下限报警
                                                alarm += "压力2斜率下限报警";

                                            preFlag = Convert.ToInt16(pack.Data[2]);

                                            GPRSPreFrameDataEntity framedata = new GPRSPreFrameDataEntity();
                                            framedata.TerId = pack.ID.ToString();
                                            framedata.ModifyTime = DateTime.Now;
                                            framedata.Frame = str_frame;

                                            int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                                            float pressuevalue = 0;
                                            float volvalue = -1;  //电压,如果是没有这个电压值的,赋值为-1，保存至数据库时根据-1保存空
                                            if (addtion_voldata)
                                                volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 1], pack.Data[pack.DataLength - 2] }, 0)) / 1000;
                                            for (int i = 0; i < dataindex; i++)
                                            {
                                                year = 2000 + Convert.ToInt16(pack.Data[i * 8 + 3]);
                                                month = Convert.ToInt16(pack.Data[i * 8 + 4]);
                                                day = Convert.ToInt16(pack.Data[i * 8 + 5]);
                                                hour = Convert.ToInt16(pack.Data[i * 8 + 6]);
                                                minute = Convert.ToInt16(pack.Data[i * 8 + 7]);
                                                sec = Convert.ToInt16(pack.Data[i * 8 + 8]);

                                                pressuevalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[i * 8 + 10], pack.Data[i * 8 + 9] }, 0)) / 1000;
                                                OnSendMsg(new SocketEventArgs(string.Format("index({0})|压力终端[{1}]|报警({2})|压力标志({3})|采集时间({4})|压力值:{5}MPa|电压值:{6}V",
                                                    i, pack.ID, alarm, preFlag, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, pressuevalue, volvalue)));

                                                GPRSPreDataEntity data = new GPRSPreDataEntity();
                                                data.PreValue = pressuevalue;
                                                data.Voltage = volvalue;
                                                try
                                                {
                                                    data.ColTime = new DateTime(year, month, day, hour, minute, sec);
                                                }
                                                catch { data.ColTime = ConstValue.MinDateTime; }
                                                bNeedCheckTime = NeedCheckTime(data.ColTime);
                                                framedata.lstPreData.Add(data);
                                            }

                                            GlobalValue.Instance.GPRS_PreFrameData.Enqueue(framedata);  //通知存储线程处理
                                            GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertPreValue);
                                        }
                                        else if (pack.C1 == (byte)GPRS_READ.READ_FLOWDATA)  //从站向主站发送流量采集数据
                                        {
                                            bool isBCD = false;  //数据是否为BCD码
                                            int dataindex = (pack.DataLength - 2 - 1) % 18;
                                            if (dataindex != 0)
                                            {
                                                if (dataindex == 2)   //有带电压值
                                                {
                                                    addtion_voldata = true;
                                                    dataindex = 0;
                                                }
                                                else
                                                {
                                                    dataindex = (pack.DataLength - 2 - 1) % 10;
                                                    isBCD = true;
                                                    if (dataindex == 2)     //有带电压值
                                                    {
                                                        addtion_voldata = true;
                                                        dataindex = 0;
                                                    }
                                                }

                                            }
                                            if (dataindex != 0)
                                            {
                                                throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(2+1+(18/10)*n)或(2+1+(18/10)*n+2)规则");
                                            }

                                            if (isBCD)
                                            {
                                                if (addtion_voldata)
                                                    dataindex = (pack.DataLength - 2 - 1 - 2) / 10;
                                                else
                                                    dataindex = (pack.DataLength - 2 - 1) / 10;
                                            }
                                            else
                                            {
                                                if (addtion_voldata)
                                                    dataindex = (pack.DataLength - 2 - 1 - 2) / 18;
                                                else
                                                    dataindex = (pack.DataLength - 2 - 1) / 18;
                                            }

                                            int alarmflag = 0;
                                            int flowFlag = 0;

                                            //报警标志
                                            alarmflag = BitConverter.ToInt16(new byte[] { pack.Data[0], pack.Data[1] }, 0);
                                            flowFlag = Convert.ToInt16(pack.Data[2]);

                                            float volvalue = -1;  //电压,如果是没有这个电压值的,赋值为-1，保存至数据库时根据-1保存空
                                            if (addtion_voldata)
                                                volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 1], pack.Data[pack.DataLength - 2] }, 0)) / 1000;

                                            GPRSFlowFrameDataEntity framedata = new GPRSFlowFrameDataEntity();
                                            framedata.TerId = pack.ID.ToString();
                                            framedata.ModifyTime = DateTime.Now;
                                            framedata.Frame = str_frame;

                                            int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                                            double forward_flowvalue = 0, reverse_flowvalue = 0, instant_flowvalue = 0;
                                            for (int i = 0; i < dataindex; i++)
                                            {
                                                year = 2000 + Convert.ToInt16(pack.Data[i * 18 + 3]);
                                                month = Convert.ToInt16(pack.Data[i * 18 + 4]);
                                                day = Convert.ToInt16(pack.Data[i * 18 + 5]);
                                                hour = Convert.ToInt16(pack.Data[i * 18 + 6]);
                                                minute = Convert.ToInt16(pack.Data[i * 18 + 7]);
                                                sec = Convert.ToInt16(pack.Data[i * 18 + 8]);

                                                if (!isBCD)
                                                {
                                                    //前向流量
                                                    forward_flowvalue = BitConverter.ToSingle(new byte[] { pack.Data[i * 18 + 12], pack.Data[i * 18 + 11], pack.Data[i * 18 + 10], pack.Data[i * 18 + 9] }, 0);
                                                    //反向流量
                                                    reverse_flowvalue = BitConverter.ToSingle(new byte[] { pack.Data[i * 18 + 16], pack.Data[i * 18 + 15], pack.Data[i * 18 + 14], pack.Data[i * 18 + 13] }, 0);
                                                    //瞬时流量
                                                    instant_flowvalue = BitConverter.ToSingle(new byte[] { pack.Data[i * 18 + 20], pack.Data[i * 18 + 19], pack.Data[i * 18 + 18], pack.Data[i * 18 + 17] }, 0);

                                                    OnSendMsg(new SocketEventArgs(string.Format("index({0})|流量终端[{1}]|报警标志({2})|流量标志({3})|采集时间({4})|正向流量值:{5}|反向流量值:{6}|瞬时流量值:{7}|电压值:{8}V",
                                                        i, pack.ID, alarmflag, flowFlag, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, forward_flowvalue, reverse_flowvalue, instant_flowvalue, volvalue)));
                                                }
                                                else
                                                {
                                                    string flowvalue = String.Format("{0:X2}", pack.Data[i * 18 + 12]) + String.Format("{0:X2}", pack.Data[i * 18 + 11]) + String.Format("{0:X2}", pack.Data[i * 18 + 10]) + String.Format("{0:X2}", pack.Data[i * 18 + 9]);
                                                    forward_flowvalue = Convert.ToDouble(flowvalue) / 100;
                                                    OnSendMsg(new SocketEventArgs(string.Format("index({0})|流量终端[{1}]|报警标志({2})|流量标志({3})|采集时间({4})|日累计流量值:{5}|电压值:{6}V",
                                                        i, pack.ID, alarmflag, flowFlag, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, forward_flowvalue, volvalue)));
                                                }

                                                GPRSFlowDataEntity data = new GPRSFlowDataEntity();
                                                data.Forward_FlowValue = forward_flowvalue;
                                                data.Reverse_FlowValue = reverse_flowvalue;
                                                data.Instant_FlowValue = instant_flowvalue;
                                                data.Voltage = volvalue;
                                                try
                                                {
                                                    data.ColTime = new DateTime(year, month, day, hour, minute, sec);
                                                }
                                                catch { data.ColTime = ConstValue.MinDateTime; }
                                                bNeedCheckTime = NeedCheckTime(data.ColTime);
                                                framedata.lstFlowData.Add(data);
                                            }
                                            GlobalValue.Instance.GPRS_FlowFrameData.Enqueue(framedata);  //通知存储线程处理
                                            GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertFlowValue);
                                        }
                                        else if (pack.C1 == (byte)GPRS_READ.READ_ALARMINFO)  //从站向主站发送设备报警信息
                                        {
                                            if (pack.DataLength != 7 && pack.DataLength != 9)   //pack.DataLength == 9 带电压值
                                            {
                                                throw new ArgumentException(DateTime.Now.ToString() + " " + "帧数据长度[" + pack.DataLength + "]不符合(2+1+18*n)或(2+1+18*n+2)规则");
                                            }

                                            string alarm = "";
                                            //报警
                                            /*
                                             * A0—电池低压报警。
                                             * A1—压力传感器1损坏报警。
                                             * A2—压力传感器2损坏报警。
                                             * A3—485流量传感器损坏报警。
                                             * A4～A7—备用
                                             */

                                            if ((pack.Data[0] & 0x01) == 1)  //电池低压报警
                                                alarm += "电池低压报警";
                                            else if (((pack.Data[0] & 0x02) >> 1) == 1)   //压力传感器1损坏报警
                                                alarm += "压力传感器1损坏报警";
                                            else if (((pack.Data[0] & 0x04) >> 2) == 1)   //压力传感器2损坏报警
                                                alarm += "压力传感器2损坏报警";
                                            else if (((pack.Data[0] & 0x08) >> 3) == 1)  //485流量传感器损坏报警
                                                alarm += "485流量传感器损坏报警";

                                            int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                                            year = 2000 + Convert.ToInt16(pack.Data[1]);
                                            month = Convert.ToInt16(pack.Data[2]);
                                            day = Convert.ToInt16(pack.Data[3]);
                                            hour = Convert.ToInt16(pack.Data[4]);
                                            minute = Convert.ToInt16(pack.Data[5]);
                                            sec = Convert.ToInt16(pack.Data[6]);

                                            float volvalue = -1;  //电压,如果是没有这个电压值的,赋值为-1，保存至数据库时根据-1保存空
                                            if (pack.DataLength == 9)   //pack.DataLength == 9 带电压值
                                            {
                                                volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 1], pack.Data[pack.DataLength - 2] }, 0)) / 1000;
                                            }
                                            if (month == 0)
                                                month = 1;
                                            if (day == 0)
                                                day = 1;
                                            bNeedCheckTime = NeedCheckTime(new DateTime(year, month, day, hour, minute, sec));
                                            OnSendMsg(new SocketEventArgs(string.Format("压力终端[{0}]{1}|时间({2})|电压值:{3}V",
                                                 pack.ID, alarm, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, volvalue)));
                                        }
                                        #endregion
                                    }
                                    else if (pack.ID3 == (byte)Entity.ConstValue.DEV_TYPE.PRESS_CTRL)
                                    {
                                        #region 压力控制器
                                        if (pack.C1 == (byte)PRECTRL_COMMAND.READ_DATA)  //从站向主站发送采集数据
                                        {
                                            int dataindex = (pack.DataLength) % 24;
                                            if (dataindex != 0)
                                                throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(24*n)规则");  //GPRS远程压力终端在数据段最后增加两个字节的电压数据
                                            else
                                                dataindex = (pack.DataLength) / 24;

                                            string parmalarm = "", alarm = "";

                                            //报警
                                            /*
                                             参数报警标识说明：相应的位为“1”，则表示有相应报警，如下所示，
                                            A0-进口压力上限报警
                                            A1-进口压力下限报警
                                            A2-压力调整超时报警
                                            A3～A7-备用
                                              设备报警标识说明：相应的位为“1”，则表示有相应报警，如下所示，
                                            A0-电池报警
                                            A1-进口压力传感器报警
                                            A2-出口压力传感器报警
                                            A3-流量器报警
                                            A4～A7-备用
                                             */

                                            GPRSPrectrlFrameDataEntity framedata = new GPRSPrectrlFrameDataEntity();
                                            framedata.TerId = pack.DevID.ToString();
                                            framedata.ModifyTime = DateTime.Now;
                                            framedata.Frame = str_frame;

                                            for (int i = 0; i < dataindex; i++)
                                            {
                                                parmalarm = "";
                                                alarm = "";
                                                if ((pack.Data[i * 24 + 22] & 0x01) == 1)  //进口压力上限报警
                                                    parmalarm += "进口压力上限报警";
                                                else if (((pack.Data[i * 24 + 22] & 0x02) >> 1) == 1)   //进口压力下限报警
                                                    parmalarm += "进口压力下限报警";
                                                else if (((pack.Data[i * 24 + 22] & 0x04) >> 2) == 1)   //压力调整超时报警
                                                    parmalarm += "压力调整超时报警";

                                                if ((pack.Data[i * 24 + 23] & 0x01) == 1)  //电池报警
                                                    alarm += "电池报警";
                                                else if (((pack.Data[i * 24 + 23] & 0x02) >> 1) == 1)   //进口压力传感器报警
                                                    alarm += "进口压力传感器报警";
                                                else if (((pack.Data[i * 24 + 23] & 0x04) >> 2) == 1)  //出口压力传感器报警
                                                    alarm += "出口压力传感器报警";
                                                else if (((pack.Data[i * 24 + 23] & 0x08) >> 3) == 1)  //流量器报警
                                                    alarm += "流量器报警";

                                                //发送时间(6bytes)+进口压力(2bytes)+出口压力(2bytes)+正向累积流量 (4bytes)+反向累积流量(4bytes)+瞬时流量(4bytes)+参数报警标识(1byte)+设备报警标识(1byte)
                                                int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                                                year = 2000 + Convert.ToInt16(pack.Data[i * 24]);
                                                month = Convert.ToInt16(pack.Data[i * 24 + 1]);
                                                day = Convert.ToInt16(pack.Data[i * 24 + 2]);
                                                hour = Convert.ToInt16(pack.Data[i * 24 + 3]);
                                                minute = Convert.ToInt16(pack.Data[i * 24 + 4]);
                                                sec = Convert.ToInt16(pack.Data[i * 24 + 5]);

                                                //进口压力
                                                float entrance_prevalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[i * 24 + 7], pack.Data[i * 24 + 6] }, 0)) / 1000;
                                                //出口压力
                                                float outlet_prevalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[i * 24 + 9], pack.Data[i * 24 + 8] }, 0)) / 1000;
                                                //前向流量
                                                float forward_flowvalue = BitConverter.ToSingle(new byte[] { pack.Data[i * 24 + 13], pack.Data[i * 24 + 12], pack.Data[i * 24 + 11], pack.Data[i * 24 + 10] }, 0);
                                                //反向流量
                                                float reverse_flowvalue = BitConverter.ToSingle(new byte[] { pack.Data[i * 24 + 17], pack.Data[i * 24 + 16], pack.Data[i * 24 + 15], pack.Data[i * 24 + 14] }, 0);
                                                //瞬时流量
                                                float instant_flowvalue = BitConverter.ToSingle(new byte[] { pack.Data[i * 24 + 21], pack.Data[i * 24 + 20], pack.Data[i * 24 + 19], pack.Data[i * 24 + 18] }, 0);

                                                OnSendMsg(new SocketEventArgs(string.Format("index({0})|压力控制器[{1}]|参数报警({2})|设备报警({3})|采集时间({4})|进口压力:{5}MPa|出口压力:{6}MPa|正向流量值:{7}|反向流量值:{8}|瞬时流量值:{9}",
                                                        i, pack.DevID, parmalarm, alarm, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, entrance_prevalue, outlet_prevalue, forward_flowvalue, reverse_flowvalue, instant_flowvalue)));

                                                GPRSPrectrlDataEntity data = new GPRSPrectrlDataEntity();
                                                data.Entrance_preValue = entrance_prevalue;
                                                data.Outlet_preValue = outlet_prevalue;
                                                data.Forward_FlowValue = forward_flowvalue;
                                                data.Reverse_FlowValue = reverse_flowvalue;
                                                data.Instant_FlowValue = instant_flowvalue;
                                                byte balarm = (byte)((pack.Data[i * 24 + 22]) << 4);  //balarm存放pack.Data[22]的低四位到高四位位置,存放pack.Data[23]的低四位到低四位位置
                                                balarm |= 0x0f;
                                                balarm = (byte)(balarm & (pack.Data[i * 24 + 23] | 0xF0));

                                                data.AlarmCode = balarm;
                                                data.AlarmDesc = parmalarm;
                                                if (!string.IsNullOrEmpty(data.AlarmDesc))
                                                    data.AlarmDesc += ",";
                                                data.AlarmDesc += alarm;   //存放两个报警信息
                                                try
                                                {
                                                    data.ColTime = new DateTime(year, month, day, hour, minute, sec);
                                                }
                                                catch { data.ColTime = ConstValue.MinDateTime; }
                                                bNeedCheckTime = NeedCheckTime(data.ColTime);
                                                framedata.lstPrectrlData.Add(data);
                                            }

                                            GlobalValue.Instance.GPRS_PrectrlFrameData.Enqueue(framedata);  //通知存储线程处理
                                            GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertPrectrlValue);
                                        }
                                        #endregion
                                    }
                                    else if (pack.ID3 == (byte)Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL)
                                    {
                                        #region 通用终端
                                        if ((pack.C1 == (byte)GPRS_READ.READ_UNIVERSAL_SIM1) || (pack.C1 == (byte)GPRS_READ.READ_UNIVERSAL_SIM2) ||
                                            (pack.C1 == (byte)UNIVERSAL_COMMAND.CalibartionSimualte1) || (pack.C1 == (byte)UNIVERSAL_COMMAND.CalibartionSimualte2))  //接受通用终端发送的模拟数据(包含招测数据)
                                        {
                                            #region 通用终端模拟数据
                                            string name = "";
                                            string sequence = "";
                                            if (pack.C1 == (byte)GPRS_READ.READ_UNIVERSAL_SIM1)
                                            {
                                                name = "1";
                                                sequence = "1";
                                            }
                                            else if (pack.C1 == (byte)UNIVERSAL_COMMAND.CalibartionSimualte1)
                                            {
                                                name = "招测1";
                                                sequence = "1";
                                            }
                                            else if (pack.C1 == (byte)GPRS_READ.READ_UNIVERSAL_SIM2)
                                            {
                                                name = "2";
                                                sequence = "2";
                                            }
                                            else if (pack.C1 == (byte)UNIVERSAL_COMMAND.CalibartionSimualte2)
                                            {
                                                name = "招测2";
                                                sequence = "2";
                                            }
                                            int calibration = 819;// BitConverter.ToInt16(new byte[] { pack.Data[1], pack.Data[0] }, 0);
                                            GPRSUniversalFrameDataEntity framedata = new GPRSUniversalFrameDataEntity();
                                            framedata.TerId = pack.ID.ToString();
                                            framedata.ModifyTime = DateTime.Now;
                                            framedata.Frame = str_frame;

                                            int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                                            double datavalue = 0;

                                            DataRow[] dr_TerminalDataConfig = null;
                                            if (GlobalValue.Instance.UniversalDataConfig != null && GlobalValue.Instance.UniversalDataConfig.Rows.Count > 0)
                                            {
                                                dr_TerminalDataConfig = GlobalValue.Instance.UniversalDataConfig.Select("TerminalID='" + framedata.TerId + "' AND Sequence='" + sequence + "'"); //WayType
                                            }
                                            if (dr_TerminalDataConfig != null && dr_TerminalDataConfig.Length > 0)
                                            {
                                                float MaxMeasureRange = dr_TerminalDataConfig[0]["MaxMeasureRange"] != DBNull.Value ? Convert.ToSingle(dr_TerminalDataConfig[0]["MaxMeasureRange"]) : 0;
                                                float MaxMeasureRangeFlag = dr_TerminalDataConfig[0]["MaxMeasureRangeFlag"] != DBNull.Value ? Convert.ToSingle(dr_TerminalDataConfig[0]["MaxMeasureRangeFlag"]) : 0;
                                                int datawidth = dr_TerminalDataConfig[0]["FrameWidth"] != DBNull.Value ? Convert.ToInt16(dr_TerminalDataConfig[0]["FrameWidth"]) : 0;
                                                int precision = dr_TerminalDataConfig[0]["precision"] != DBNull.Value ? Convert.ToInt32(dr_TerminalDataConfig[0]["precision"]) : 0;
                                                if (MaxMeasureRangeFlag > 0 && datawidth > 0)
                                                {
                                                    int loopdatalen = 6 + datawidth;  //循环部分数据宽度 = 时间(6)+配置长度
                                                    int dataindex = (pack.DataLength - 2 - 1) % loopdatalen;
                                                    if (dataindex != 0)
                                                        throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(2+1+" + loopdatalen + "*n)规则");
                                                    dataindex = (pack.DataLength - 2 - 1) / loopdatalen;
                                                    for (int i = 0; i < dataindex; i++)
                                                    {
                                                        year = 2000 + Convert.ToInt16(pack.Data[i * 8 + 3]);
                                                        month = Convert.ToInt16(pack.Data[i * 8 + 4]);
                                                        day = Convert.ToInt16(pack.Data[i * 8 + 5]);
                                                        hour = Convert.ToInt16(pack.Data[i * 8 + 6]);
                                                        minute = Convert.ToInt16(pack.Data[i * 8 + 7]);
                                                        sec = Convert.ToInt16(pack.Data[i * 8 + 8]);

                                                        if (datawidth == 2)
                                                            datavalue = BitConverter.ToInt16(new byte[] { pack.Data[i * 8 + 10], pack.Data[i * 8 + 9] }, 0);
                                                        else if (datawidth == 4)
                                                            datavalue = BitConverter.ToSingle(new byte[] { pack.Data[i * 8 + 12], pack.Data[i * 8 + 11], pack.Data[i * 8 + 10], pack.Data[i * 8 + 9] }, 0);

                                                        datavalue = (MaxMeasureRange / MaxMeasureRangeFlag) * (datavalue - calibration);  //根据设置和校准值计算
                                                        datavalue = Convert.ToDouble(datavalue.ToString("F" + precision));  //精度调整
                                                        if (datavalue < 0)
                                                            datavalue = 0;
                                                        OnSendMsg(new SocketEventArgs(string.Format("index({0})|通用终端[{1}]模拟{2}路|校准值({3})|采集时间({4})|{5}:{6}{7}",
                                                            i, pack.ID, name, calibration, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, dr_TerminalDataConfig[0]["Name"].ToString().Trim(), datavalue, dr_TerminalDataConfig[0]["Unit"].ToString())));

                                                        GPRSUniversalDataEntity data = new GPRSUniversalDataEntity();
                                                        data.DataValue = datavalue;
                                                        data.Sim1Zero = calibration;
                                                        data.TypeTableID = Convert.ToInt32(dr_TerminalDataConfig[0]["ID"]);
                                                        try
                                                        {
                                                            data.ColTime = new DateTime(year, month, day, hour, minute, sec);
                                                        }
                                                        catch { data.ColTime = ConstValue.MinDateTime; }
                                                        bNeedCheckTime = NeedCheckTime(data.ColTime);
                                                        framedata.lstData.Add(data);
                                                    }
                                                    GlobalValue.Instance.GPRS_UniversalFrameData.Enqueue(framedata);  //通知存储线程处理
                                                    GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertUniversalValue);
                                                }
                                                else
                                                {
                                                    OnSendMsg(new SocketEventArgs("通用终端[" + framedata.TerId + "]数据帧解析规则配置错误,数据未能解析！"));
                                                }
                                            }
                                            else
                                            {
                                                OnSendMsg(new SocketEventArgs("通用终端[" + framedata.TerId + "]未配置数据帧解析规则,数据未能解析！"));
                                            }
                                            #endregion
                                        }
                                        else if ((pack.C1 == (byte)GPRS_READ.READ_UNIVERSAL_PLUSE))  //接受水质终端发送的脉冲数据
                                        {
                                            #region 通用终端脉冲
                                            GPRSUniversalFrameDataEntity framedata = new GPRSUniversalFrameDataEntity();
                                            framedata.TerId = pack.ID.ToString();
                                            framedata.ModifyTime = DateTime.Now;
                                            framedata.Frame = str_frame;

                                            int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                                            double datavalue = 0;

                                            DataRow[] dr_TerminalDataConfig = null;
                                            if (GlobalValue.Instance.UniversalDataConfig != null && GlobalValue.Instance.UniversalDataConfig.Rows.Count > 0)
                                            {
                                                dr_TerminalDataConfig = GlobalValue.Instance.UniversalDataConfig.Select("TerminalID='" + framedata.TerId + "' AND Sequence IN ('4','5','6','7','8')", "Sequence"); //WayType
                                            }
                                            if (dr_TerminalDataConfig != null && dr_TerminalDataConfig.Length > 0)
                                            {
                                                int waycount = dr_TerminalDataConfig.Length;
                                                float[] PluseUnits = new float[waycount];
                                                int[] DataWidths = new int[waycount];
                                                int[] Precisions = new int[waycount];
                                                string[] Names = new string[waycount];
                                                string[] Units = new string[waycount];
                                                int[] config_ids = new int[waycount];

                                                int topdatawidth = 0;
                                                for (int i = 0; i < waycount; i++)
                                                {
                                                    PluseUnits[i] = dr_TerminalDataConfig[i]["MaxMeasureRange"] != DBNull.Value ? Convert.ToSingle(dr_TerminalDataConfig[i]["MaxMeasureRange"]) : 0;  //每个脉冲对应的单位采集量
                                                    DataWidths[i] = dr_TerminalDataConfig[i]["FrameWidth"] != DBNull.Value ? Convert.ToInt16(dr_TerminalDataConfig[i]["FrameWidth"]) : 0;
                                                    Precisions[i] = dr_TerminalDataConfig[i]["precision"] != DBNull.Value ? Convert.ToInt32(dr_TerminalDataConfig[i]["precision"]) : 0;
                                                    Names[i] = dr_TerminalDataConfig[i]["Name"] != DBNull.Value ? dr_TerminalDataConfig[i]["Name"].ToString().Trim() : "";
                                                    Units[i] = dr_TerminalDataConfig[i]["Unit"] != DBNull.Value ? dr_TerminalDataConfig[i]["Unit"].ToString().Trim() : "";
                                                    config_ids[i] = dr_TerminalDataConfig[i]["ID"] != DBNull.Value ? Convert.ToInt32(dr_TerminalDataConfig[i]["ID"]) : 0;
                                                    topdatawidth += DataWidths[i];
                                                }

                                                if (topdatawidth > 0)
                                                {
                                                    int loopdatalen = 6 + topdatawidth + (4 - waycount) * 4;  //循环部分数据宽度 = 时间(6)+固定4路*(每路长度)
                                                    int dataindex = (pack.DataLength - 2 - 1) % loopdatalen;
                                                    if (dataindex != 0)
                                                        throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(2+1+" + loopdatalen + "*n)规则");
                                                    dataindex = (pack.DataLength - 2 - 1) / loopdatalen;
                                                    for (int i = 0; i < dataindex; i++)
                                                    {
                                                        year = 2000 + Convert.ToInt16(pack.Data[i * loopdatalen + 3]);
                                                        month = Convert.ToInt16(pack.Data[i * loopdatalen + 4]);
                                                        day = Convert.ToInt16(pack.Data[i * loopdatalen + 5]);
                                                        hour = Convert.ToInt16(pack.Data[i * loopdatalen + 6]);
                                                        minute = Convert.ToInt16(pack.Data[i * loopdatalen + 7]);
                                                        sec = Convert.ToInt16(pack.Data[i * loopdatalen + 8]);

                                                        int freindex = 0;
                                                        for (int j = 0; j < waycount; j++)
                                                        {
                                                            if (DataWidths[j] == 2)
                                                            {
                                                                datavalue = BitConverter.ToInt16(new byte[] { pack.Data[i * loopdatalen + 10 + freindex], pack.Data[i * loopdatalen + 9 + freindex] }, 0);
                                                                freindex += 2;
                                                            }
                                                            else if (DataWidths[j] == 4)
                                                            {
                                                                datavalue = BitConverter.ToInt32(new byte[] { pack.Data[i * loopdatalen + 12 + freindex], pack.Data[i * loopdatalen + 11 + freindex], pack.Data[i * loopdatalen + 10 + freindex], pack.Data[i * loopdatalen + 9 + freindex] }, 0);
                                                                freindex += 4;
                                                            }

                                                            datavalue = PluseUnits[j] * datavalue;  //脉冲计数*单位脉冲值
                                                            datavalue = Convert.ToDouble(datavalue.ToString("F" + Precisions[j]));  //精度调整
                                                            OnSendMsg(new SocketEventArgs(string.Format("index({0})|通用终端[{1}]脉冲{2}路|采集时间({3})|{4}:{5}{6}",
                                                                i, pack.ID, j + 1, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, Names[j], datavalue, Units[j])));

                                                            GPRSUniversalDataEntity data = new GPRSUniversalDataEntity();
                                                            data.DataValue = datavalue;
                                                            data.TypeTableID = Convert.ToInt32(config_ids[j]);
                                                            try
                                                            {
                                                                data.ColTime = new DateTime(year, month, day, hour, minute, sec);
                                                            }
                                                            catch { data.ColTime = ConstValue.MinDateTime; }
                                                            bNeedCheckTime = NeedCheckTime(data.ColTime);
                                                            framedata.lstData.Add(data);
                                                        }
                                                    }
                                                    GlobalValue.Instance.GPRS_UniversalFrameData.Enqueue(framedata);  //通知存储线程处理
                                                    GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertUniversalValue);
                                                }
                                                else
                                                {
                                                    OnSendMsg(new SocketEventArgs("通用终端[" + framedata.TerId + "]数据帧解析规则配置错误,数据未能解析！"));
                                                }
                                            }
                                            else
                                            {
                                                OnSendMsg(new SocketEventArgs("通用终端[" + framedata.TerId + "]未配置数据帧解析规则,数据未能解析！"));
                                            }
                                            #endregion
                                        }
                                        else if ((pack.C1 == (byte)GPRS_READ.READ_UNVERSAL_RS4851) || (pack.C1 == (byte)GPRS_READ.READ_UNVERSAL_RS4852)
                                            || (pack.C1 == (byte)GPRS_READ.READ_UNVERSAL_RS4853) || (pack.C1 == (byte)GPRS_READ.READ_UNVERSAL_RS4854)
                                            || (pack.C1 == (byte)GPRS_READ.READ_UNVERSAL_RS4855) || (pack.C1 == (byte)GPRS_READ.READ_UNVERSAL_RS4856)
                                            || (pack.C1 == (byte)GPRS_READ.READ_UNVERSAL_RS4857) || (pack.C1 == (byte)GPRS_READ.READ_UNVERSAL_RS4858))//接受通用终端发送的RS485 数据
                                        {
                                            #region 通用终端RS485 ?路数据
                                            string name = "";
                                            string sequence = "";
                                            if (pack.C1 == (byte)GPRS_READ.READ_UNVERSAL_RS4851)
                                            {
                                                name = "1";
                                                sequence = "9";
                                            }
                                            else if (pack.C1 == (byte)GPRS_READ.READ_UNVERSAL_RS4852)
                                            {
                                                name = "2";
                                                sequence = "10";
                                            }
                                            else if (pack.C1 == (byte)GPRS_READ.READ_UNVERSAL_RS4853)
                                            {
                                                name = "3";
                                                sequence = "11";
                                            }
                                            else if (pack.C1 == (byte)GPRS_READ.READ_UNVERSAL_RS4854)
                                            {
                                                name = "4";
                                                sequence = "12";
                                            }
                                            else if (pack.C1 == (byte)GPRS_READ.READ_UNVERSAL_RS4855)
                                            {
                                                name = "5";
                                                sequence = "13";
                                            }
                                            else if (pack.C1 == (byte)GPRS_READ.READ_UNVERSAL_RS4856)
                                            {
                                                name = "6";
                                                sequence = "14";
                                            }
                                            else if (pack.C1 == (byte)GPRS_READ.READ_UNVERSAL_RS4857)
                                            {
                                                name = "7";
                                                sequence = "15";
                                            }
                                            else if (pack.C1 == (byte)GPRS_READ.READ_UNVERSAL_RS4858)
                                            {
                                                name = "8";
                                                sequence = "16";
                                            }
                                            int calibration = BitConverter.ToInt16(new byte[] { pack.Data[1], pack.Data[0] }, 0);
                                            GPRSUniversalFrameDataEntity framedata = new GPRSUniversalFrameDataEntity();
                                            framedata.TerId = pack.ID.ToString();
                                            framedata.ModifyTime = DateTime.Now;
                                            framedata.Frame = str_frame;

                                            int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                                            double datavalue = 0;

                                            DataRow[] dr_TerminalDataConfig = null;
                                            DataRow[] dr_DataConfig_Child = null;
                                            bool ConfigHaveChild = false;
                                            if (GlobalValue.Instance.UniversalDataConfig != null && GlobalValue.Instance.UniversalDataConfig.Rows.Count > 0)
                                            {
                                                dr_TerminalDataConfig = GlobalValue.Instance.UniversalDataConfig.Select("TerminalID='" + framedata.TerId + "' AND Sequence='" + sequence + "'"); //WayType
                                                if (dr_TerminalDataConfig != null && dr_TerminalDataConfig.Length > 0)
                                                {
                                                    dr_DataConfig_Child = GlobalValue.Instance.UniversalDataConfig.Select("ParentID='" + dr_TerminalDataConfig[0]["ID"].ToString().Trim() + "'", "Sequence");
                                                    if (dr_DataConfig_Child != null && dr_DataConfig_Child.Length > 0)
                                                    {
                                                        ConfigHaveChild = true;
                                                        dr_TerminalDataConfig = dr_DataConfig_Child;  //有子节点配置时，使用子节点配置
                                                    }
                                                }
                                            }
                                            if (dr_TerminalDataConfig != null && dr_TerminalDataConfig.Length > 0)
                                            {
                                                int waycount = dr_TerminalDataConfig.Length;
                                                float[] MaxMeasureRanges = new float[waycount];
                                                float[] MaxMeasureRangeFlags = new float[waycount];
                                                int[] DataWidths = new int[waycount];
                                                int[] Precisions = new int[waycount];
                                                string[] Names = new string[waycount];
                                                string[] Units = new string[waycount];
                                                int[] config_ids = new int[waycount];

                                                int topdatawidth = 0;
                                                for (int i = 0; i < waycount; i++)
                                                {
                                                    MaxMeasureRanges[i] = dr_TerminalDataConfig[i]["MaxMeasureRange"] != DBNull.Value ? Convert.ToSingle(dr_TerminalDataConfig[i]["MaxMeasureRange"]) : 0;
                                                    MaxMeasureRangeFlags[i] = dr_TerminalDataConfig[0]["MaxMeasureRangeFlag"] != DBNull.Value ? Convert.ToSingle(dr_TerminalDataConfig[0]["MaxMeasureRangeFlag"]) : 0;
                                                    DataWidths[i] = dr_TerminalDataConfig[i]["FrameWidth"] != DBNull.Value ? Convert.ToInt16(dr_TerminalDataConfig[i]["FrameWidth"]) : 0;
                                                    Precisions[i] = dr_TerminalDataConfig[i]["precision"] != DBNull.Value ? Convert.ToInt32(dr_TerminalDataConfig[i]["precision"]) : 0;
                                                    Names[i] = dr_TerminalDataConfig[i]["Name"] != DBNull.Value ? dr_TerminalDataConfig[i]["Name"].ToString().Trim() : "";
                                                    Units[i] = dr_TerminalDataConfig[i]["Unit"] != DBNull.Value ? dr_TerminalDataConfig[i]["Unit"].ToString().Trim() : "";
                                                    config_ids[i] = dr_TerminalDataConfig[i]["ID"] != DBNull.Value ? Convert.ToInt32(dr_TerminalDataConfig[i]["ID"]) : 0;
                                                    topdatawidth += DataWidths[i];
                                                }

                                                if (topdatawidth > 0)
                                                {
                                                    int loopdatalen = 6 + topdatawidth;  //循环部分数据宽度
                                                    int dataindex = (pack.DataLength - 2 - 1) % loopdatalen;
                                                    if (dataindex != 0)
                                                        throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(2+1+" + loopdatalen + "*n)规则");
                                                    dataindex = (pack.DataLength - 2 - 1) / loopdatalen;
                                                    for (int i = 0; i < dataindex; i++)
                                                    {
                                                        year = 2000 + Convert.ToInt16(pack.Data[i * loopdatalen + 3]);
                                                        month = Convert.ToInt16(pack.Data[i * loopdatalen + 4]);
                                                        day = Convert.ToInt16(pack.Data[i * loopdatalen + 5]);
                                                        hour = Convert.ToInt16(pack.Data[i * loopdatalen + 6]);
                                                        minute = Convert.ToInt16(pack.Data[i * loopdatalen + 7]);
                                                        sec = Convert.ToInt16(pack.Data[i * loopdatalen + 8]);

                                                        int freindex = 0;
                                                        for (int j = 0; j < waycount; j++)
                                                        {
                                                            if (DataWidths[j] == 2)
                                                            {
                                                                datavalue = BitConverter.ToInt16(new byte[] { pack.Data[i * loopdatalen + 10 + freindex], pack.Data[i * loopdatalen + 9 + freindex] }, 0);
                                                                freindex += 2;
                                                            }
                                                            else if (DataWidths[j] == 4)
                                                            {
                                                                datavalue = BitConverter.ToInt32(new byte[] { pack.Data[i * loopdatalen + 12 + freindex], pack.Data[i * loopdatalen + 11 + freindex], pack.Data[i * loopdatalen + 10 + freindex], pack.Data[i * loopdatalen + 9 + freindex] }, 0);
                                                                freindex += 4;
                                                            }

                                                            datavalue = MaxMeasureRanges[j] * datavalue;  //系数
                                                            datavalue = Convert.ToDouble(datavalue.ToString("F" + Precisions[j]));  //精度调整
                                                            OnSendMsg(new SocketEventArgs(string.Format("index({0})|通用终端[{1}]RS485 {2}路|采集时间({3})|{4}:{5}{6}",
                                                                i, pack.ID, name, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, Names[j], datavalue, Units[j])));

                                                            GPRSUniversalDataEntity data = new GPRSUniversalDataEntity();
                                                            data.DataValue = datavalue;
                                                            data.TypeTableID = Convert.ToInt32(config_ids[j]);
                                                            try
                                                            {
                                                                data.ColTime = new DateTime(year, month, day, hour, minute, sec);
                                                            }
                                                            catch { data.ColTime = ConstValue.MinDateTime; }
                                                            bNeedCheckTime = NeedCheckTime(data.ColTime);
                                                            framedata.lstData.Add(data);
                                                        }
                                                    }
                                                    GlobalValue.Instance.GPRS_UniversalFrameData.Enqueue(framedata);  //通知存储线程处理
                                                    GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertUniversalValue);
                                                }
                                                else
                                                {
                                                    OnSendMsg(new SocketEventArgs("通用终端[" + framedata.TerId + "]数据帧解析规则配置错误,数据未能解析！"));
                                                }
                                            }
                                            else
                                            {
                                                OnSendMsg(new SocketEventArgs("通用终端[" + framedata.TerId + "]未配置数据帧解析规则,数据未能解析！"));
                                            }
                                            #endregion
                                        }
                                        #endregion
                                    }
                                    else if (pack.ID3 == (byte)Entity.ConstValue.DEV_TYPE.OLWQ_CTRL)
                                    {
                                        #region 水质终端
                                        bool addtion_voldata = false;   //是否在数据段最后增加了两个字节的电压数据
                                        if ((pack.C1 == (byte)GPRS_READ.READ_TURBIDITY) || (pack.C1 == (byte)GPRS_READ.READ_RESIDUALCL) ||
                                            (pack.C1 == (byte)GPRS_READ.READ_PH) || (pack.C1 == (byte)GPRS_READ.READ_TEMP))  //从站向主站发送水质采集数据
                                        {
                                            int dataindex = (pack.DataLength - 2 - 1) % 8;
                                            if (dataindex != 0)
                                            {
                                                if (dataindex == 2)
                                                {
                                                    dataindex = (pack.DataLength - 2 - 1 - 2) / 8;
                                                    addtion_voldata = true;
                                                }
                                                else
                                                {
                                                    throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(2+1+8*n)或(2+1+8*n+2)规则");  //最后增加两个字节的电压数据
                                                }
                                            }
                                            else
                                                dataindex = (pack.DataLength - 2 - 1) / 8;

                                            GPRSOLWQFrameDataEntity framedata = new GPRSOLWQFrameDataEntity();
                                            framedata.TerId = pack.ID.ToString();
                                            framedata.ModifyTime = DateTime.Now;
                                            framedata.Frame = str_frame;

                                            int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                                            float value = 0;
                                            string name = "";
                                            string unit = "";
                                            string valuecolumnname = "";
                                            if (pack.C1 == (byte)GPRS_READ.READ_TURBIDITY)
                                            {
                                                name = "浊度";
                                                unit = "NTU";
                                                valuecolumnname = "Turbidity";
                                            }
                                            else if (pack.C1 == (byte)GPRS_READ.READ_RESIDUALCL)
                                            {
                                                name = "余氯";
                                                unit = "PPM";
                                                valuecolumnname = "ResidualCl";
                                            }
                                            else if (pack.C1 == (byte)GPRS_READ.READ_PH)
                                            {
                                                name = "PH";
                                                unit = "ph";
                                                valuecolumnname = "PH";
                                            }
                                            else if (pack.C1 == (byte)GPRS_READ.READ_TEMP)
                                            {
                                                name = "温度";
                                                unit = "℃";
                                                valuecolumnname = "Temperature";
                                            }

                                            float volvalue = -1;  //电压,如果是没有这个电压值的,赋值为-1，保存至数据库时根据-1保存空
                                            if (addtion_voldata)  //电压
                                                volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 1], pack.Data[pack.DataLength - 2] }, 0)) / 1000;

                                            for (int i = 0; i < dataindex; i++)
                                            {
                                                year = 2000 + Convert.ToInt16(pack.Data[i * 8 + 3]);
                                                month = Convert.ToInt16(pack.Data[i * 8 + 4]);
                                                day = Convert.ToInt16(pack.Data[i * 8 + 5]);
                                                hour = Convert.ToInt16(pack.Data[i * 8 + 6]);
                                                minute = Convert.ToInt16(pack.Data[i * 8 + 7]);
                                                sec = Convert.ToInt16(pack.Data[i * 8 + 8]);

                                                value = (float)BitConverter.ToInt16(new byte[] { pack.Data[i * 8 + 10], pack.Data[i * 8 + 9] }, 0);
                                                GPRSOLWQDataEntity data = new GPRSOLWQDataEntity();
                                                data.ValueColumnName = valuecolumnname;
                                                if (pack.C1 == (byte)GPRS_READ.READ_TURBIDITY)
                                                {
                                                    value = value / 100;
                                                    data.Turbidity = value;
                                                }
                                                else if (pack.C1 == (byte)GPRS_READ.READ_RESIDUALCL)
                                                {
                                                    data.ResidualCl = value / 1000;
                                                }
                                                else if (pack.C1 == (byte)GPRS_READ.READ_PH)
                                                {
                                                    data.PH = value / 100;
                                                }
                                                else if (pack.C1 == (byte)GPRS_READ.READ_TEMP)
                                                {
                                                    data.Temperature = value;
                                                }

                                                OnSendMsg(new SocketEventArgs(string.Format("index({0})|水质终端[{1}]|采集时间({2})|{3}值:{4}{5}|电压值:{6}V",
                                                    dataindex, pack.ID, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, name, value, unit, volvalue)));
                                                data.Voltage = volvalue;
                                                try
                                                {
                                                    data.ColTime = new DateTime(year, month, day, hour, minute, sec);
                                                }
                                                catch { data.ColTime = ConstValue.MinDateTime; }
                                                bNeedCheckTime = NeedCheckTime(data.ColTime);
                                                framedata.lstOLWQData.Add(data);
                                            }

                                            GlobalValue.Instance.GPRS_OLWQFrameData.Enqueue(framedata);  //通知存储线程处理
                                            GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertOLWQValue);
                                        }
                                        else if (pack.C1 == (byte)GPRS_READ.READ_CONDUCTIVITY)  //从站向主站发送电导率采集数据
                                        {
                                            int dataindex = (pack.DataLength - 2 - 1) % 12;
                                            if (dataindex != 0)
                                            {
                                                if (dataindex == 2)
                                                {
                                                    dataindex = (pack.DataLength - 2 - 1 - 2) / 12;
                                                    addtion_voldata = true;
                                                }
                                                else
                                                {
                                                    throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(2+1+10*n)或(2+1+10*n+2)规则");  //最后增加两个字节的电压数据
                                                }
                                            }
                                            else
                                                dataindex = (pack.DataLength - 2 - 1) / 12;

                                            GPRSOLWQFrameDataEntity framedata = new GPRSOLWQFrameDataEntity();
                                            framedata.TerId = pack.ID.ToString();
                                            framedata.ModifyTime = DateTime.Now;
                                            framedata.Frame = str_frame;

                                            int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                                            float Condvalue = 0;
                                            float Tempvalue = 0;

                                            float volvalue = -1;  //电压,如果是没有这个电压值的,赋值为-1，保存至数据库时根据-1保存空
                                            if (addtion_voldata)  //电压
                                                volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 1], pack.Data[pack.DataLength - 2] }, 0)) / 1000;

                                            for (int i = 0; i < dataindex; i++)
                                            {
                                                year = 2000 + Convert.ToInt16(pack.Data[i * 12 + 3]);
                                                month = Convert.ToInt16(pack.Data[i * 12 + 4]);
                                                day = Convert.ToInt16(pack.Data[i * 12 + 5]);
                                                hour = Convert.ToInt16(pack.Data[i * 12 + 6]);
                                                minute = Convert.ToInt16(pack.Data[i * 12 + 7]);
                                                sec = Convert.ToInt16(pack.Data[i * 12 + 8]);

                                                Condvalue = ((float)BitConverter.ToInt32(new byte[] { pack.Data[i * 12 + 12], pack.Data[i * 12 + 11], pack.Data[i * 12 + 10], pack.Data[i * 12 + 9] }, 0)) / 100;
                                                Tempvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[i * 12 + 14], pack.Data[i * 12 + 13] }, 0)) / 10;

                                                OnSendMsg(new SocketEventArgs(string.Format("index({0})|水质终端[{1}]|采集时间({2})|电导率值:{3}us/cm,温度:{4}℃|电压值:{5}V",
                                                    i, pack.ID, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, Condvalue.ToString("f2"), Tempvalue.ToString("f1"), volvalue)));

                                                GPRSOLWQDataEntity data = new GPRSOLWQDataEntity();
                                                data.Conductivity = Condvalue;
                                                data.Temperature = Tempvalue;
                                                data.ValueColumnName = "Conductivity";
                                                data.Voltage = volvalue;
                                                try
                                                {
                                                    data.ColTime = new DateTime(year, month, day, hour, minute, sec);
                                                }
                                                catch { data.ColTime = ConstValue.MinDateTime; }
                                                bNeedCheckTime = NeedCheckTime(data.ColTime);
                                                framedata.lstOLWQData.Add(data);
                                            }

                                            GlobalValue.Instance.GPRS_OLWQFrameData.Enqueue(framedata);  //通知存储线程处理
                                            GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertOLWQValue);
                                        }
                                        else if (pack.C1 == (byte)GPRS_READ.READ_OLWQFLOW) //从站向主站发送流量采集数据(水质终端)
                                        {
                                            #region 上海肯特(KENT)水表
                                            if (pack.Data[2] == 0x02)   //上海肯特(KENT)水表
                                            {
                                                int dataindex = (pack.DataLength - 2 - 2 - 1) % (6 + 36);  //两字节报警,1字节厂家类型,
                                                if (dataindex != 0)
                                                {
                                                    throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(2+1+(6+36)*n)规则");
                                                }
                                                dataindex = (pack.DataLength - 2 - 2 - 1) / (6 + 36);

                                                int alarmflag = 0;
                                                //报警标志
                                                alarmflag = BitConverter.ToInt16(new byte[] { pack.Data[0], pack.Data[1] }, 0);

                                                float volvalue = -1;  //电压,如果是没有这个电压值的,赋值为-1，保存至数据库时根据-1保存空
                                                if (pack.DataLength - (6 + 36) * dataindex - 3 == 2)  //最后余两个字节则认为是电压值
                                                    volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 1], pack.Data[pack.DataLength - 2] }, 0)) / 1000;

                                                GPRSFlowFrameDataEntity framedata = new GPRSFlowFrameDataEntity();
                                                framedata.TerId = pack.ID.ToString();
                                                framedata.ModifyTime = DateTime.Now;
                                                framedata.Frame = str_frame;

                                                int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                                                double forward_flowvalue = 0, reverse_flowvalue = 0, instant_flowvalue = 0;
                                                for (int i = 0; i < dataindex; i++)
                                                {
                                                    year = 2000 + Convert.ToInt16(pack.Data[i * 42 + 3]);
                                                    month = Convert.ToInt16(pack.Data[i * 42 + 4]);
                                                    day = Convert.ToInt16(pack.Data[i * 42 + 5]);
                                                    hour = Convert.ToInt16(pack.Data[i * 42 + 6]);
                                                    minute = Convert.ToInt16(pack.Data[i * 42 + 7]);
                                                    sec = Convert.ToInt16(pack.Data[i * 42 + 8]);

                                                    byte balarm = 0x00;  //水表报警
                                                    string errmsg = "";
                                                    if (KERTFlow.ProcessFrameData(pack.Data, i * 42 + 3 + 6, 36, out forward_flowvalue, out reverse_flowvalue, out instant_flowvalue, out balarm, out errmsg))
                                                    {
                                                        string str_alarm = "空";
                                                        if ((balarm & 0x10) == 0x10)
                                                        { str_alarm = "励磁报警"; }
                                                        else if ((balarm & 0x08) == 0x08)
                                                        { str_alarm = "空管报警"; }
                                                        else if ((balarm & 0x04) == 0x04)
                                                        { str_alarm = "流浪反向报警"; }
                                                        else if ((balarm & 0x10) == 0x10)
                                                        { str_alarm = "流量上限报警"; }
                                                        else if ((balarm & 0x10) == 0x10)
                                                        { str_alarm = "流量下限报警"; }
                                                        OnSendMsg(new SocketEventArgs(string.Format("index({0})|水质终端[{1}]|报警标志({2})|采集时间({3})|正向流量值:{4}|反向流量值:{5}|瞬时流量值:{6}|报警:{7}|电压值:{8}V",
                                                           i, pack.ID, alarmflag, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, forward_flowvalue.ToString("f4"), reverse_flowvalue.ToString("f4"), instant_flowvalue.ToString("f4"), str_alarm, volvalue)));

                                                        GPRSFlowDataEntity data = new GPRSFlowDataEntity();
                                                        data.Forward_FlowValue = forward_flowvalue;
                                                        data.Reverse_FlowValue = reverse_flowvalue;
                                                        data.Instant_FlowValue = instant_flowvalue;
                                                        data.Voltage = volvalue;
                                                        try
                                                        {
                                                            data.ColTime = new DateTime(year, month, day, hour, minute, sec);
                                                        }
                                                        catch { data.ColTime = ConstValue.MinDateTime; }
                                                        bNeedCheckTime = NeedCheckTime(data.ColTime);
                                                        framedata.lstFlowData.Add(data);


                                                    }
                                                    else
                                                    {
                                                        OnSendMsg(new SocketEventArgs(string.Format("水质终端[{0}]|报警标志({1})|采集时间({2})|错误:{3}",
                                                            pack.ID, alarmflag, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, errmsg)));
                                                    }
                                                }
                                                GlobalValue.Instance.GPRS_FlowFrameData.Enqueue(framedata);  //通知存储线程处理
                                                GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertFlowValue);
                                            }
                                            else
                                            {
                                                OnSendMsg(new SocketEventArgs("水质终端[" + pack.ID + "]错误未知水表类型!"));
                                            }
                                            #endregion
                                        }
                                        else if (pack.C1 == (byte)GPRS_READ.READ_OLWQALARM)  //从站向主站发送设备报警信息(水质终端)
                                        {
                                            if (pack.DataLength != 7 && pack.DataLength != 9)   //pack.DataLength == 9 带电压值
                                            {
                                                throw new ArgumentException(DateTime.Now.ToString() + " " + "帧数据长度[" + pack.DataLength + "]不符合(2+1+18*n)或(2+1+18*n+2)规则");
                                            }

                                            string alarm = "";
                                            //报警
                                            /*
                                             * A0—电池低压报警。
                                             * A1—浊度报警。
                                             * A2—余氯报警。
                                             * A3—流量报警。
                                             * A4—PH报警。
                                             * A5—电导率报警。
                                             * A6～A7—备用
                                             */

                                            if ((pack.Data[0] & 0x01) == 1)  //电池低压报警
                                                alarm += "电池低压报警";
                                            else if (((pack.Data[0] & 0x02) >> 1) == 1)   //浊度报警
                                                alarm += "浊度报警";
                                            else if (((pack.Data[0] & 0x04) >> 2) == 1)   //余氯报警
                                                alarm += "余氯报警";
                                            else if (((pack.Data[0] & 0x08) >> 3) == 1)  //流量报警
                                                alarm += "流量报警";
                                            else if (((pack.Data[0] & 0x10) >> 4) == 1)  //PH报警
                                                alarm += "PH报警";
                                            else if (((pack.Data[0] & 0x20) >> 5) == 1)  //电导率报警
                                                alarm += "电导率报警";

                                            int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                                            year = 2000 + Convert.ToInt16(pack.Data[1]);
                                            month = Convert.ToInt16(pack.Data[2]);
                                            day = Convert.ToInt16(pack.Data[3]);
                                            hour = Convert.ToInt16(pack.Data[4]);
                                            minute = Convert.ToInt16(pack.Data[5]);
                                            sec = Convert.ToInt16(pack.Data[6]);

                                            float volvalue = -1;  //电压,如果是没有这个电压值的,赋值为-1，保存至数据库时根据-1保存空
                                            if (pack.DataLength == 9)   //pack.DataLength == 9 带电压值
                                            {
                                                volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 1], pack.Data[pack.DataLength - 2] }, 0)) / 1000;
                                            }
                                            if (month == 0)
                                                month = 1;
                                            if (day == 0)
                                                day = 1;
                                            bNeedCheckTime = NeedCheckTime(new DateTime(year, month, day, hour, minute, sec));
                                            OnSendMsg(new SocketEventArgs(string.Format("水质终端[{0}]{1}|时间({2})|电压值:{3}V",
                                                 pack.ID, alarm, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, volvalue)));
                                        }
                                        #endregion
                                    }
                                    else if (pack.ID3 == (byte)Entity.ConstValue.DEV_TYPE.HYDRANT_CTRL)
                                    {
                                        #region 消防栓
                                        GPRSHydrantFrameDataEntity framedata = new GPRSHydrantFrameDataEntity();
                                        framedata.TerId = pack.ID.ToString();
                                        framedata.ModifyTime = DateTime.Now;
                                        framedata.Frame = str_frame;

                                        int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                                        year = 2000 + Convert.ToInt16(pack.Data[0]);
                                        month = Convert.ToInt16(pack.Data[1]);
                                        day = Convert.ToInt16(pack.Data[2]);
                                        hour = Convert.ToInt16(pack.Data[3]);
                                        minute = Convert.ToInt16(pack.Data[4]);
                                        sec = Convert.ToInt16(pack.Data[5]);

                                        GPRSHydrantDataEntity data = new GPRSHydrantDataEntity();
                                        try
                                        {
                                            data.ColTime = new DateTime(year, month, day, hour, minute, sec);
                                        }
                                        catch { data.ColTime = ConstValue.MinDateTime; }
                                        bNeedCheckTime = NeedCheckTime(data.ColTime);

                                        if (pack.C1 == (byte)GPRS_READ.READ_HYDRANT_OPEN)
                                        {
                                            int openangle = Convert.ToInt16(pack.Data[6]);
                                            float prevalue = (float)BitConverter.ToInt16(new byte[] { pack.Data[8], pack.Data[7] }, 0) / 3;
                                            OnSendMsg(new SocketEventArgs(string.Format("消防栓[{0}]被打开|时间({1})|开度:{2},压力:{3}",
                                                    pack.ID, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, openangle, prevalue.ToString("f3"))));
                                            data.Operate = HydrantOptType.Open;
                                            data.PreValue = prevalue;
                                            data.OpenAngle = openangle;
                                        }
                                        else if (pack.C1 == (byte)GPRS_READ.READ_HYDRANT_CLOSE)
                                        {
                                            OnSendMsg(new SocketEventArgs(string.Format("消防栓[{0}]被关闭|时间({1})",
                                                       pack.ID, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec)));
                                            data.Operate = HydrantOptType.Close;
                                        }
                                        else if (pack.C1 == (byte)GPRS_READ.READ_HYDRANT_OPENANGLE)
                                        {
                                            int openangle = Convert.ToInt16(pack.Data[6]);
                                            OnSendMsg(new SocketEventArgs(string.Format("消防栓[{0}]开度|时间({1})|开度:{2}",
                                                    pack.ID, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, openangle)));
                                            data.OpenAngle = openangle;
                                            data.Operate = HydrantOptType.OpenAngle;
                                        }
                                        else if (pack.C1 == (byte)GPRS_READ.READ_HYDRANT_IMPACT)
                                        {
                                            OnSendMsg(new SocketEventArgs(string.Format("消防栓[{0}]被撞击|时间({1})",
                                                       pack.ID, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec)));
                                            data.Operate = HydrantOptType.Impact;
                                        }
                                        else if (pack.C1 == (byte)GPRS_READ.READ_HYDRANT_KNOCKOVER)
                                        {
                                            OnSendMsg(new SocketEventArgs(string.Format("消防栓[{0}]被撞倒|时间({1})",
                                                       pack.ID, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec)));
                                            data.Operate = HydrantOptType.KnockOver;
                                        }

                                        framedata.lstHydrantData.Add(data);
                                        GlobalValue.Instance.GPRS_HydrantFrameData.Enqueue(framedata);  //通知存储线程处理
                                        GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertHydrantValue);
                                        #endregion
                                    }
                                    else if (pack.ID3 == (byte)Entity.ConstValue.DEV_TYPE.NOISE_CTRL)
                                    {
                                        #region 噪声数据远传控制器
                                        if (pack.C1 == (byte)GPRS_READ.READ_NOISEDATA)  //从站向主站发送噪声采集数据
                                        {
                                            int dataindex = (pack.DataLength) % 2;
                                            if (dataindex != 0)
                                                throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(2*n)规则");  //GPRS远程压力终端在数据段最后增加两个字节的电压数据
                                            else
                                                dataindex = (pack.DataLength) / 2;
                                            GPRSNoiseFrameDataEntity framedata = new GPRSNoiseFrameDataEntity();
                                            framedata.TerId = pack.ID.ToString();
                                            framedata.ModifyTime = DateTime.Now;
                                            framedata.Frame = str_frame;

                                            GPRSHydrantDataEntity data = new GPRSHydrantDataEntity();
                                            bNeedCheckTime = false;
                                            float volvalue = -1;  //电压,如果是没有这个电压值的,赋值为-1，保存至数据库时根据-1保存空
                                            volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 1], pack.Data[pack.DataLength - 2] }, 0)) / 1000;

                                            //记录仪ID（4byte）＋启动值（2byte）＋总帧数（1byte）＋帧号（1byte）＋ 数据（128byte）＋ 电压（2byte）
                                            int logId = BitConverter.ToInt32(new byte[] { pack.Data[3], pack.Data[2], pack.Data[1], 0x00 }, 0);  //记录仪ID
                                            int standvalue = BitConverter.ToInt16(new byte[] { pack.Data[5], pack.Data[4] }, 0);      //启动值
                                            int sumpackcount = Convert.ToInt32(pack.Data[6]);
                                            int curpackindex = Convert.ToInt32(pack.Data[7]);

                                            if (curpackindex == 1)
                                                state.lstBuffer.Clear();   //收到第一包清空缓存
                                            bool needprocess = true;  //是否处理当前包
                                            if (curpackindex > 1 && (state.NoisePackIndex == curpackindex)) //如果当前包和上一包序号一样则不处理
                                            {
                                                needprocess = false;
                                            }
                                            if (needprocess)
                                            {
                                                state.NoisePackIndex = curpackindex;   //记录当前收到包的序号
                                                if (sumpackcount != curpackindex && !pack.IsFinal)
                                                {
                                                    for (int i = 8; i < pack.DataLength - 2; i++)  //多包时，当前不是最后一包时缓存数据至state.lstBuffer中
                                                    {
                                                        state.lstBuffer.Add(pack.Data[i]);
                                                    }
                                                }
                                                else
                                                {
                                                    List<byte> lstbytes = new List<byte>();
                                                    if (curpackindex > 1)  //多包时获取缓存的数据拼接成完整的数据
                                                    {
                                                        lstbytes.AddRange(state.lstBuffer);
                                                        state.lstBuffer.Clear();
                                                    }
                                                    for (int i = 8; i < pack.DataLength - 2; i++)  //添加当前包数据
                                                    {
                                                        lstbytes.Add(pack.Data[i]);
                                                    }
                                                    UpLoadNoiseDataEntity noisedataentity = new UpLoadNoiseDataEntity();
                                                    noisedataentity.TerId = logId.ToString();  // pack.DevID.ToString();
                                                    noisedataentity.GroupId = "";
                                                    //启动值
                                                    noisedataentity.cali = standvalue;
                                                    noisedataentity.ColTime = DateTime.Now.ToString();
                                                    for (int i = 2; i + 1 < lstbytes.Count; i += 2)
                                                    {
                                                        noisedataentity.Data += BitConverter.ToInt16(new byte[] { lstbytes[i + 1], lstbytes[i] }, 0) + ",";
                                                    }
                                                    if (noisedataentity.Data.EndsWith(","))
                                                        noisedataentity.Data = noisedataentity.Data.Substring(0, noisedataentity.Data.Length - 1);
                                                    framedata.NoiseData = noisedataentity;

                                                    bNeedCheckTime = true;  //每天传一次,一天校时一次,不适用NeedCheckTime方法校时
                                                }
                                                string strcurnoisedata = "";  //当前包的数据,用于显示
                                                for (int i = 8; i + 1 < pack.DataLength - 2; i += 2)
                                                {
                                                    strcurnoisedata += BitConverter.ToInt16(new byte[] { pack.Data[i + 1], pack.Data[i] }, 0) + ",";
                                                }
                                                if (strcurnoisedata.EndsWith(","))
                                                    strcurnoisedata = strcurnoisedata.Substring(0, strcurnoisedata.Length - 1);
                                                OnSendMsg(new SocketEventArgs(string.Format("噪声远传控制器[{0}]|记录仪[{1}]|启动值[{2}]|总包数:{3}、当前第{4}包|噪声数据:{5}|电压值:{6}V",
                                                       pack.DevID, logId, standvalue, sumpackcount, curpackindex, strcurnoisedata, volvalue)));

                                                GlobalValue.Instance.GPRS_NoiseFrameData.Enqueue(framedata);  //通知存储线程处理
                                                GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertNoiseValue);
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (pack.ID3 == (byte)Entity.ConstValue.DEV_TYPE.WATER_WORKS)
                                    {
                                        #region 水厂数据
                                        if (pack.C1 == (byte)GPRS_READ.READ_WATERWORKSDATA)  //水厂PLC采集数据
                                        {
                                            if (pack.DataLength != 50)
                                            {
                                                throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合固定长度(50byte)规则");
                                            }

                                            GPRSWaterWorkerFrameDataEntity framedata = new GPRSWaterWorkerFrameDataEntity();
                                            framedata.TerId = pack.DevID.ToString();
                                            framedata.ModifyTime = DateTime.Now;
                                            framedata.Frame = str_frame;

                                            int i = 0;
                                            string stractivenerge1 = String.Format("{0:X2}", pack.Data[i + 0]) + String.Format("{0:X2}", pack.Data[i + 1]) + String.Format("{0:X2}", pack.Data[i + 2]) + String.Format("{0:X2}", pack.Data[i + 3]);
                                            double activenerge1 = Convert.ToSingle(stractivenerge1) / 100;         //1#有功电量
                                            string strreactivenerge1 = String.Format("{0:X2}", pack.Data[i + 4]) + String.Format("{0:X2}", pack.Data[i + 5]) + String.Format("{0:X2}", pack.Data[i + 6]) + String.Format("{0:X2}", pack.Data[i + 7]);
                                            double reactivenerge1 = Convert.ToSingle(strreactivenerge1) / 100;         //1#无功电量
                                            string stractivenerge2 = String.Format("{0:X2}", pack.Data[i + 8]) + String.Format("{0:X2}", pack.Data[i + 9]) + String.Format("{0:X2}", pack.Data[i + 10]) + String.Format("{0:X2}", pack.Data[i + 11]);
                                            double activenerge2 = Convert.ToSingle(stractivenerge2) / 100;         //2#有功电量
                                            string strreactivenerge2 = String.Format("{0:X2}", pack.Data[i + 12]) + String.Format("{0:X2}", pack.Data[i + 13]) + String.Format("{0:X2}", pack.Data[i + 14]) + String.Format("{0:X2}", pack.Data[i + 15]);
                                            double reactivenerge2 = Convert.ToSingle(strreactivenerge2) / 100;         //2#无功电量
                                            string stractivenerge3 = String.Format("{0:X2}", pack.Data[i + 16]) + String.Format("{0:X2}", pack.Data[i + 17]) + String.Format("{0:X2}", pack.Data[i + 18]) + String.Format("{0:X2}", pack.Data[i + 19]);
                                            double activenerge3 = Convert.ToSingle(stractivenerge3) / 100;         //3#有功电量
                                            string strreactivenerge3 = String.Format("{0:X2}", pack.Data[i + 20]) + String.Format("{0:X2}", pack.Data[i + 21]) + String.Format("{0:X2}", pack.Data[i + 22]) + String.Format("{0:X2}", pack.Data[i + 23]);
                                            double reactivenerge3 = Convert.ToSingle(strreactivenerge3) / 100;         //3#无功电量
                                            string stractivenerge4 = String.Format("{0:X2}", pack.Data[i + 24]) + String.Format("{0:X2}", pack.Data[i + 25]) + String.Format("{0:X2}", pack.Data[i + 26]) + String.Format("{0:X2}", pack.Data[i + 27]);
                                            double activenerge4 = Convert.ToSingle(stractivenerge4) / 100;         //4#有功电量
                                            string strreactivenerge4 = String.Format("{0:X2}", pack.Data[i + 28]) + String.Format("{0:X2}", pack.Data[i + 29]) + String.Format("{0:X2}", pack.Data[i + 30]) + String.Format("{0:X2}", pack.Data[i + 31]);
                                            double reactivenerge4 = Convert.ToSingle(strreactivenerge4) / 100;         //4#无功电量

                                            double pressure = (double)BitConverter.ToInt16(new byte[] { pack.Data[i + 33], pack.Data[i + 32] }, 0) / 1000;         //出口压力
                                            double liquidlevel = BitConverter.ToSingle(new byte[] { pack.Data[i + 37], pack.Data[i + 36], pack.Data[i + 35], pack.Data[i + 34] }, 0);         //液位
                                            double flow1 = BitConverter.ToInt32(new byte[] { pack.Data[i + 41], pack.Data[i + 40], pack.Data[i + 39], pack.Data[i + 38] }, 0);         //流量1
                                            double flow2 = BitConverter.ToInt32(new byte[] { pack.Data[i + 45], pack.Data[i + 44], pack.Data[i + 43], pack.Data[i + 42] }, 0);         //流量2
                                            //2个字节表示一个开关状态,第一个字节没有用  0x00 0x01表示开  0x00 0x00表示关
                                            bool switch1 = pack.Data[i + 46] > 0 ? true : false;            //开关状态1
                                            bool switch2 = pack.Data[i + 47] > 0 ? true : false;            //开关状态2
                                            bool switch3 = pack.Data[i + 48] > 0 ? true : false;            //开关状态3
                                            bool switch4 = pack.Data[i + 49] > 0 ? true : false;            //开关状态4

                                            OnSendMsg(new SocketEventArgs(string.Format("水厂数据[{0}]|1#有功电量:{1}|1#无功电量:{2}|2#有功电量:{3}|2#无功电量:{4}|3#有功电量:{5}|3#无功电量:{6}|4#有功电量:{7}|4#无功电量:{8}|出口压力:{9}|液位:{10}|流量1:{11}|流量2:{12}|开关状态1:{13}|开关状态2:{14}|开关状态3:{15}|开关状态4:{16}",
                                                pack.DevID, activenerge1, reactivenerge1, activenerge2, reactivenerge2, activenerge3, reactivenerge3, activenerge4, reactivenerge4,
                                                pressure, liquidlevel, flow1, flow2, switch1 ? "开" : "关", switch2 ? "开" : "关", switch3 ? "开" : "关", switch4 ? "开" : "关")));

                                            framedata.Activenerge1 = activenerge1;
                                            framedata.Rectivenerge1 = reactivenerge1;
                                            framedata.Activenerge2 = activenerge2;
                                            framedata.Rectivenerge2 = reactivenerge2;
                                            framedata.Activenerge3 = activenerge3;
                                            framedata.Rectivenerge3 = reactivenerge3;
                                            framedata.Activenerge4 = activenerge4;
                                            framedata.Rectivenerge4 = reactivenerge4;
                                            framedata.Pressure = pressure;
                                            framedata.LiquidLevel = liquidlevel;
                                            framedata.Flow1 = flow1;
                                            framedata.Flow2 = flow2;
                                            framedata.Switch1 = switch1;
                                            framedata.Switch2 = switch2;
                                            framedata.Switch3 = switch3;
                                            framedata.Switch4 = switch4;

                                            //bNeedCheckTime = NeedCheckTime(framedata.ColTime);

                                            GlobalValue.Instance.GPRS_WaterworkerFrameData.Enqueue(framedata);  //通知存储线程处理
                                            GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertWaterworkerValue);
                                        }
                                        #endregion
                                    }
                                    #endregion

                                    response.DevID = pack.DevID;
                                    CallSocketEntity currentSocketEntity = null;
                                    foreach (CallSocketEntity callentity in lstClient)
                                    {
                                        if (callentity.DevType == pack.DevType && callentity.TerId != -1 && callentity.TerId == pack.DevID)
                                        {
                                            currentSocketEntity = callentity;
                                            callentity.ClientSocket = handler;
                                            if (callentity.lstWaitSendCmd != null && callentity.lstWaitSendCmd.Count > 0)
                                            {
                                                for (int i = 0; i < callentity.lstWaitSendCmd.Count; i++)
                                                {
                                                    lstCommandPack.Add(callentity.lstWaitSendCmd[i].SendPackage);
                                                    callentity.lstWaitSendCmd[i].SendCount--;
                                                    callentity.lstWaitSendCmd[i].SendTime = DateTime.Now;
                                                }
                                            }
                                            //将在线信息发送给UI更新
                                            SocketEntity msmqEntity = new SocketEntity();
                                            msmqEntity.lstOnLine = new List<OnLineTerEntity>();
                                            msmqEntity.lstOnLine.Add(new OnLineTerEntity(callentity.DevType, callentity.TerId));
                                            OnSendMsg(new SocketEventArgs(ConstValue.MSMQTYPE.Data_OnLineState, msmqEntity));
                                        }
                                    }

                                    //Thread.Sleep(500);
                                }

                                #region 发送后续命令帧
                                if (((GlobalValue.Instance.lstGprsCmd != null && GlobalValue.Instance.lstGprsCmd.Count > 0)
                                    || bNeedCheckTime || lstCommandPack.Count > 0) && pack.IsFinal)
                                {
                                    byte[] bsenddata;
                                    SendObject sendObj = new SendObject();
                                    if (pack.CommandType == CTRL_COMMAND_TYPE.REQUEST_BY_SLAVE)//接收到的数据帧则需要应答响应帧
                                    {
                                        #region 回复响应帧
                                        response.DevType = pack.DevType;
                                        response.C0 = 0x48;  //主站发出的应答帧，有后续命令
                                        response.C1 = (byte)pack.C1;
                                        response.DataLength = 0;
                                        response.Data = null;
                                        response.CS = response.CreateCS();

                                        bsenddata = response.ToArray();

#if debug
                                        OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  发送响应帧:" + ConvertHelper.ByteToString(bsenddata, bsenddata.Length)));
#else
                                                OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  发送响应帧"));
#endif
                                        sendObj.workSocket = handler;
                                        sendObj.IsFinal = false;
                                        sendObj.DevType = pack.DevType;
                                        sendObj.DevID = pack.DevID;

                                        handler.BeginSend(bsenddata, 0, bsenddata.Length, 0, new AsyncCallback(SendCallback), sendObj);
                                        #endregion
                                    }

                                    if (GlobalValue.Instance.lstGprsCmd != null && GlobalValue.Instance.lstGprsCmd.Count > 0)
                                    {
                                        for (int i = 0; i < GlobalValue.Instance.lstGprsCmd.Count; i++)
                                        {
                                            if (GlobalValue.Instance.lstGprsCmd[i].SendedFlag == 0 && GlobalValue.Instance.lstGprsCmd[i].DeviceId == (int)pack.DevID && GlobalValue.Instance.lstGprsCmd[i].DevTypeId == (int)pack.DevType)
                                            {
                                                Package CommandPack = new Package();
                                                CommandPack.DevID = pack.DevID;
                                                CommandPack.DevType = pack.DevType;
                                                CommandPack.C0 = Convert.ToByte(GlobalValue.Instance.lstGprsCmd[i].CtrlCode);
                                                CommandPack.C1 = Convert.ToByte(GlobalValue.Instance.lstGprsCmd[i].FunCode);
                                                CommandPack.Data = ConvertHelper.StringToByte(GlobalValue.Instance.lstGprsCmd[i].Data);
                                                CommandPack.DataLength = CommandPack.Data.Length;
                                                //CommandPack.CS = CommandPack.CreateCS();
                                                lstCommandPack.Add(CommandPack);
                                                GlobalValue.Instance.lstGprsCmd[i].SendedCount++;  //记录发送次数

                                                break;  //只添加一条,每次都发送一条等待回应帧再发送下一条
                                            }
                                        }
                                    }

                                    #region 校时
                                    if (bNeedCheckTime)
                                    {
                                        Package pack_time = new Package();
                                        pack_time.DevType = pack.DevType;
                                        pack_time.DevID = pack.DevID;
                                        pack_time.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
                                        if (pack.DevType == Entity.ConstValue.DEV_TYPE.PRESS_CTRL)
                                            pack_time.C1 = (byte)PRECTRL_COMMAND.SET_TIME;
                                        else if (pack.DevType == Entity.ConstValue.DEV_TYPE.Data_CTRL)
                                            pack_time.C1 = (byte)PreTer_COMMAND.SET_TIME;
                                        else if (pack.DevType == Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL)
                                            pack_time.C1 = (byte)UNIVERSAL_COMMAND.SET_TIME;
                                        else if (pack.DevType == Entity.ConstValue.DEV_TYPE.OLWQ_CTRL)
                                            pack_time.C1 = (byte)OLWQ_COMMAND.SET_TIME;
                                        else if (pack.DevType == Entity.ConstValue.DEV_TYPE.HYDRANT_CTRL)
                                            pack_time.C1 = (byte)HYDRANT_COMMAND.SET_TIME;
                                        else if (pack.DevType == ConstValue.DEV_TYPE.NOISE_CTRL)
                                            pack_time.C1 = (byte)NOISE_CTRL_COMMAND.SET_NOISE_GPRSTIME;

                                        byte[] data = null;
                                        if (pack.DevType == Entity.ConstValue.DEV_TYPE.PRESS_CTRL)  //压力控制器多三个字节,1byte(密码权限=1)+2bytes(管理密码)+6byte(时间数据)，暂时写死
                                        {
                                            data = new byte[9];
                                            data[0] = 0x01;
                                            data[1] = 0x00;
                                            data[2] = 0x00;
                                            data[3] = (byte)(DateTime.Now.Year - 2000);
                                            data[4] = (byte)DateTime.Now.Month;
                                            data[5] = (byte)DateTime.Now.Day;
                                            data[6] = (byte)DateTime.Now.Hour;
                                            data[7] = (byte)DateTime.Now.Minute;
                                            data[8] = (byte)DateTime.Now.Second;
                                            pack_time.DataLength = data.Length;
                                            pack_time.Data = data;
                                        }
                                        else
                                        {
                                            data = new byte[6];
                                            data[0] = (byte)(DateTime.Now.Year - 2000);
                                            data[1] = (byte)DateTime.Now.Month;
                                            data[2] = (byte)DateTime.Now.Day;
                                            data[3] = (byte)DateTime.Now.Hour;
                                            data[4] = (byte)DateTime.Now.Minute;
                                            data[5] = (byte)DateTime.Now.Second;
                                            pack_time.DataLength = data.Length;
                                            pack_time.Data = data;
                                        }
                                        //pack_time.CS = pack_time.CreateCS();
                                        if (lstCommandPack.Count > 0)  //已经有需要下送的命令,由于一次只下送一条命令，校时命令需要放在所有命令之后,将校时命令放入命令缓存
                                        {
                                            GPRSCmdEntity timeCmd = new GPRSCmdEntity();
                                            timeCmd.DeviceId = pack_time.DevID;
                                            timeCmd.CtrlCode = pack_time.C0;
                                            timeCmd.FunCode = pack_time.C1;
                                            timeCmd.DevTypeId = Convert.ToInt32(pack_time.DevType);
                                            timeCmd.Data=ConvertHelper.ByteToString(pack_time.Data, pack_time.DataLength);
                                            timeCmd.DataLen = timeCmd.Data.Length;
                                            timeCmd.TableId = -1;
                                            lock (GlobalValue.Instance.lstGprsCmdLock)
                                            { GlobalValue.Instance.lstGprsCmd.Add(timeCmd); }
                                        }

                                        lstCommandPack.Add(pack_time);  //校时命令添加进来,下边只发送第一条命令,加入校时命令，用于后面判断是不是最后一个帧
                                    }
                                    #endregion

                                    if(lstCommandPack.Count>0)//for (int i = 0; i < lstCommandPack.Count; i++)
                                    {
                                        Package commandpack = lstCommandPack[0];  //i
                                        if (0 != (lstCommandPack.Count - 1))  //i
                                            commandpack.C0 = ((byte)(commandpack.C0 | 0x08));  //不是最后一个帧
                                        commandpack.CS = commandpack.CreateCS();
                                        bsenddata = commandpack.ToArray();

                                        Thread.Sleep(200);  //帧之间间隔
#if debug
                                        OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  发送命令帧:" + ConvertHelper.ByteToString(bsenddata, bsenddata.Length)));
#else
                                        OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  发送响应帧"));
#endif
                                        sendObj = new SendObject();
                                        sendObj.workSocket = handler;
                                        sendObj.IsFinal = (0 == (lstCommandPack.Count - 1)) ? true : false;  //i
                                        sendObj.DevType = pack.DevType;
                                        sendObj.DevID = pack.DevID;
                                        handler.BeginSend(bsenddata, 0, bsenddata.Length, 0, new AsyncCallback(SendCallback), sendObj);
                                    }
                                }
                                else if(pack.CommandType == CTRL_COMMAND_TYPE.REQUEST_BY_SLAVE) //接收到的数据帧则需要应答响应帧
                                {
                                    response.CommandType = CTRL_COMMAND_TYPE.RESPONSE_BY_MASTER;
                                    response.DevType = pack.DevType;
                                    response.C1 = (byte)pack.C1;
                                    response.DataLength = 0;
                                    response.Data = null;
                                    response.CS = response.CreateCS();

                                    byte[] bsenddata = response.ToArray();
#if debug
                                    OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  发送响应帧:" + ConvertHelper.ByteToString(bsenddata, bsenddata.Length)));
#else
                                        OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  发送响应帧"));
#endif
                                    SendObject sendObj = new SendObject();
                                    sendObj.workSocket = handler;
                                    sendObj.IsFinal = pack.IsFinal;
                                    sendObj.DevType = pack.DevType;
                                    sendObj.DevID = pack.DevID;
                                    if (handler != null && SocketHelper.IsSocketConnected_Poll(handler))
                                        handler.BeginSend(bsenddata, 0, bsenddata.Length, 0, new AsyncCallback(SendCallback), sendObj);
                                }
                                #endregion

                            }
                        }
                        #endregion

                        #region SL651协议
                        if (packageBytes.Count >= PackageDefine.MinLenth651 && (packageBytes[packageBytes.Count - 1 - 2] == PackageDefine.EndByte651 || packageBytes[packageBytes.Count - 1 - 2] == PackageDefine.EndByte_Continue))
                        {
                            bool need_response = true;    //是否回复回应帧(连续几个帧，只回最后一个帧)
                            bool deal = false;
                            byte[] crcbs = packageBytes.ToArray();
                            byte[] crctest = Package651.crc16(crcbs, crcbs.Length - 2);
                            if (crctest != null && crctest[0] == crcbs[crcbs.Length - 2] && crctest[1] == crcbs[crcbs.Length - 1])
                            {
                                if (index + 2 < ReceiveBytes.Count) //多个帧在一起时，做检查（当前位置之后两个字节是否是下一帧开始，且剩下长度大于最小帧长度）
                                {
                                    if (ReceiveBytes[index + 1] == 0x7E && ReceiveBytes[index + 2] == 0x7E && ReceiveBytes.Count >= PackageDefine.MinLenth * 2)
                                    {
                                        deal = true;
                                        need_response = true;  //false -> true 每个都回
                                    }
                                }
                                else
                                {
                                    deal = true;
                                    need_response = true;
                                }
                            }

                            #region 651 deal
                            if (deal)
                            {
                                byte[] arr = packageBytes.ToArray();
                                Package651 pack;
                                bool havesubsequent = false;  //是否有后续包
                                string subsequentmsg = "";  //如果是包且有后续，提示当前包数
                                if ((PackageDefine.MinLenth651 <= arr.Length) & Package651.TryParse(arr, out pack, out havesubsequent, out subsequentmsg))//找到结束字符并且是完整一帧
                                {
#if debug
                                    OnSendMsg(new SocketEventArgs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 收到帧数据:" + ConvertHelper.ByteToString(arr, arr.Length)));
#else
                                    OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  收到帧数据"));
#endif
                                    int clientindex = -1;  //查找或添加
                                    for (int i = 0; i < lstClient.Count; i++)
                                    {
                                        if (lstClient[i].A1 == pack.A1 && lstClient[i].A2 == pack.A2 && lstClient[i].A3 == pack.A3 && lstClient[i].A4 == pack.A4 && lstClient[i].A5 == pack.A5)
                                        {
                                            clientindex = i;
                                            lstClient[i].ClientSocket = handler;
                                            break;
                                        }
                                    }
                                    if (clientindex == -1)
                                    {
                                        CallSocketEntity sockpack = new CallSocketEntity();
                                        sockpack.ClientSocket = handler;
                                        sockpack.A1 = pack.A1;
                                        sockpack.A2 = pack.A2;
                                        sockpack.A3 = pack.A3;
                                        sockpack.A4 = pack.A4;
                                        sockpack.A5 = pack.A5;
                                        lstClient.Add(sockpack);
                                        clientindex = lstClient.Count - 1;
                                    }

                                    #region 解析数据
                                    if (havesubsequent)  //多包时保存帧正文数据以便下边解帧时组合
                                    {
                                        if (pack.CurPackCount == 1)  //第一帧时,初始化组帧变量
                                            lstClient[clientindex].multiData = new List<byte>();
                                        lstClient[clientindex].multiData.AddRange(pack.Data);
                                    }
                                    Universal651SerialPortEntity spEntity = null;  //不使用
                                    string analyseStr = "";
                                    byte[] analyseData = pack.Data;
                                    if (havesubsequent) //如果是多包的情况,且是最后一帧
                                    {
                                        if (pack.CurPackCount > 1 && pack.SumPackCount == pack.CurPackCount)
                                        {
                                            if (lstClient[clientindex].multiData != null && lstClient[clientindex].multiData.Count > 0)
                                                analyseData = lstClient[clientindex].multiData.ToArray();
                                            analyseStr = SmartWaterSystem.SL651AnalyseElement.AnalyseElement(pack.FUNCODE, analyseData, pack.dt, ref spEntity);

                                        }
                                    }
                                    else if (analyseData != null)
                                    {
                                        analyseStr = SmartWaterSystem.SL651AnalyseElement.AnalyseElement(pack.FUNCODE, analyseData, pack.dt, ref spEntity);
                                    }

                                    if (!havesubsequent || (havesubsequent && pack.CurPackCount == 1))  //没有后续帧或者有后续帧且是第一帧
                                    {

                                        string str_senddt = "";
                                        if (pack.dt != null && pack.dt.Length == 6)
                                        {
                                            str_senddt = string.Format("{0}-{1}-{2} {3}:{4}:{5}", String.Format("{0:X2}", pack.dt[0]), String.Format("{0:X2}", pack.dt[1])
                                                , String.Format("{0:X2}", pack.dt[2]), String.Format("{0:X2}", pack.dt[3]), String.Format("{0:X2}", pack.dt[4]), String.Format("{0:X2}", pack.dt[5]));
                                        }

                                        OnSendMsg(new SocketEventArgs(string.Format("中心站地址:{0},遥测站地址:A1-A5[{1},{2},{3},{4},{5}],密码:{6},功能码:{7}({8}),上/下行:{9},",
                                                Convert.ToInt16(pack.CenterAddr), String.Format("{0:X2}", pack.A1), String.Format("{0:X2}", pack.A2), String.Format("{0:X2}", pack.A3), String.Format("{0:X2}", pack.A4), String.Format("{0:X2}", pack.A5),
                                                "0x" + String.Format("{0:X2}", pack.PWD[0]) + string.Format("{0:X2}", pack.PWD[1]), "0x" + String.Format("{0:X2}", pack.FUNCODE), SmartWaterSystem.SL651AnalyseElement.GetFuncodeName(pack.FUNCODE), pack.IsUpload ? "上行" : "下行") +
                                                string.Format("报文长度:{0},报文起始符:{1},{2},发报时间:{3},{4}校验码:{5}",
                                                pack.DataLength, "0x" + string.Format("{0:X2}", pack.CStart),
                                                string.IsNullOrEmpty(subsequentmsg) ? "流水号:" + BitConverter.ToInt16(pack.SNum, 0) : subsequentmsg, str_senddt,
                                                analyseStr, ConvertHelper.ByteToString(pack.CS, pack.CS.Length))
                                                ));
                                    }
                                    else if (havesubsequent) // && (pack.CurPackCount > 1 && pack.SumPackCount == pack.CurPackCount))  //有后续帧且是最后一帧
                                    {
                                        OnSendMsg(new SocketEventArgs(string.Format("中心站地址:{0},遥测站地址:A1-A5[{1},{2},{3},{4},{5}],密码:{6},功能码:{7}({8}),上/下行:{9},",
                                            Convert.ToInt16(pack.CenterAddr), Convert.ToInt16(pack.A1), Convert.ToInt16(pack.A2), Convert.ToInt16(pack.A3), Convert.ToInt16(pack.A4), Convert.ToInt16(pack.A5),
                                            "0x" + String.Format("{0:X2}", pack.PWD[0]) + string.Format("{0:X2}", pack.PWD[1]), "0x" + String.Format("{0:X2}", pack.FUNCODE), SmartWaterSystem.SL651AnalyseElement.GetFuncodeName(pack.FUNCODE), pack.IsUpload ? "上行" : "下行") +
                                            string.Format("报文长度:{0},报文起始符:{1},{2},{3}校验码:{4}",
                                            pack.DataLength, "0x" + string.Format("{0:X2}", pack.CStart),
                                            string.IsNullOrEmpty(subsequentmsg) ? "" : subsequentmsg,
                                            analyseStr, ConvertHelper.ByteToString(pack.CS, pack.CS.Length))
                                            ));
                                    }
                                    #endregion

                                    if (!havesubsequent || (havesubsequent && (pack.CurPackCount > 1 && pack.SumPackCount == pack.CurPackCount)))  //不是多包或者多包时最后一包时，可以发送命令帧和响应帧
                                    {
                                        #region 发送帧
                                        Package651 response = new Package651();  //响应帧
                                        List<Package651> pack_cmd = new List<Package651>(); //命令帧(列表)
                                        if (lstClient != null && lstClient.Count > 0)
                                        {
                                            foreach (CallSocketEntity callentity in lstClient)
                                            {
                                                if ((callentity.A5 == pack.A5) && (callentity.A4 == pack.A4) && (callentity.A3 == pack.A3) && (callentity.A2 == pack.A2) && (callentity.A1 == pack.A1))
                                                {
                                                    callentity.ClientSocket = handler;
                                                    if (callentity.lstWaitSendCmd != null && callentity.lstWaitSendCmd.Count > 0)
                                                    {
                                                        for (int i = 0; i < callentity.lstWaitSendCmd.Count; i++)
                                                        {
                                                            pack_cmd.Add(callentity.lstWaitSendCmd[i].SendPackage651);
                                                            callentity.lstWaitSendCmd[i].SendCount--;
                                                            callentity.lstWaitSendCmd[i].SendTime = DateTime.Now;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        need_response = Package651.NeedResp(pack.FUNCODE);

                                        response.dt = new byte[6];
                                        response.dt[0] = ConvertHelper.StringToByte((DateTime.Now.Year - 2000).ToString())[0];
                                        response.dt[1] = ConvertHelper.StringToByte(DateTime.Now.Month.ToString().PadLeft(2, '0'))[0];
                                        response.dt[2] = ConvertHelper.StringToByte(DateTime.Now.Day.ToString().PadLeft(2, '0'))[0];
                                        response.dt[3] = ConvertHelper.StringToByte(DateTime.Now.Hour.ToString().PadLeft(2, '0'))[0];
                                        response.dt[4] = ConvertHelper.StringToByte(DateTime.Now.Minute.ToString().PadLeft(2, '0'))[0];
                                        response.dt[5] = ConvertHelper.StringToByte(DateTime.Now.Second.ToString().PadLeft(2, '0'))[0];

                                        if (pack_cmd != null && pack_cmd.Count > 0)  //有后续帧
                                        {
                                            byte[] bsenddata = null;
                                            if (need_response)
                                            {
                                                response.A1 = pack.A1;
                                                response.A2 = pack.A2;
                                                response.A3 = pack.A3;
                                                response.A4 = pack.A4;
                                                response.A5 = pack.A5;
                                                response.CenterAddr = pack.CenterAddr;
                                                //response.PWD[0] = Convert.ToByte(txtPwd0.Text);
                                                //response.PWD[1] = Convert.ToByte(txtPwd1.Text);
                                                response.PWD = new byte[2];
                                                response.PWD[0] = pack.PWD[1];
                                                response.PWD[1] = pack.PWD[0];
                                                byte[] lens = BitConverter.GetBytes((ushort)8);
                                                response.L0 = lens[0];
                                                response.L1 = lens[1];
                                                response.FUNCODE = pack.FUNCODE;
                                                response.IsUpload = false;
                                                response.CStart = PackageDefine.CStart;
                                                response.SNum = pack.SNum;
                                                response.AddrFlag = pack.AddrFlag;
                                                response.End = PackageDefine.ESC;  //保持在线，以便发送后续帧
                                                bsenddata = response.ToResponseArray(true);
                                                response.CS = Package651.crc16(bsenddata, bsenddata.Length);

                                                bsenddata = response.ToResponseArray();
#if debug
                                                OnSendMsg(new SocketEventArgs((DateTime.Now.ToString() + "  发送响应帧:" + ConvertHelper.ByteToString(bsenddata, bsenddata.Length))));
#else
                                                OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  发送响应帧"));
#endif
                                                //Thread.Sleep(500);  //1
                                                Send651(handler, bsenddata);
                                            }

                                            //发送命令帧
                                            for (int i = 0; i < pack_cmd.Count; i++)
                                            {
                                                if (pack_cmd[i].End != PackageDefine.ENQ)  //如果本来就是询问命令，则还是ENQ(05)发送下去，不管在不在线，因为总是要回应
                                                {
                                                    if (i + 1 == pack_cmd.Count && !SL651AllowOnLine)  //最后一条
                                                    {
                                                        Package651 p = pack_cmd[i];
                                                        p.End = PackageDefine.EOT;
                                                        pack_cmd[i] = p;
                                                    }
                                                    else
                                                    {
                                                        Package651 p = pack_cmd[i];
                                                        p.End = PackageDefine.ESC;
                                                        pack_cmd[i] = p;
                                                    }
                                                }
                                                Package651 pack651Cmd = pack_cmd[i];
                                                bsenddata = pack651Cmd.ToResponseArray(true);
                                                pack651Cmd.CS = Package651.crc16(bsenddata, bsenddata.Length);

                                                bsenddata = pack651Cmd.ToResponseArray();
                                                OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  发送命令帧:" + ConvertHelper.ByteToString(bsenddata, bsenddata.Length)));

                                                Thread.Sleep(1500);  //2
                                                if (Send651(handler, bsenddata))  //发送成功则清除
                                                {
                                                    if (lstClient != null && lstClient.Count > 0)
                                                    {
                                                        foreach (CallSocketEntity callentity in lstClient)
                                                        {
                                                            if ((callentity.A5 == pack.A5) && (callentity.A4 == pack.A4) && (callentity.A3 == pack.A3) && (callentity.A2 == pack.A2) && (callentity.A1 == pack.A1))
                                                            {
                                                                if (callentity.lstWaitSendCmd != null && callentity.lstWaitSendCmd.Count > 0)
                                                                {
                                                                    for (int j = 0; j < callentity.lstWaitSendCmd.Count; j++)
                                                                    {
                                                                        if (callentity.lstWaitSendCmd[j].SendPackage651 != null && callentity.lstWaitSendCmd[j].SendPackage651.Equals(pack651Cmd))
                                                                        {
                                                                            callentity.lstWaitSendCmd.RemoveAt(j);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (need_response)
                                            {
                                                //Package response = new Package();
                                                response.A1 = pack.A1;
                                                response.A2 = pack.A2;
                                                response.A3 = pack.A3;
                                                response.A4 = pack.A4;
                                                response.A5 = pack.A5;
                                                response.CenterAddr = pack.CenterAddr;
                                                //response.PWD[0] = Convert.ToByte(txtPwd0.Text);
                                                //response.PWD[1] = Convert.ToByte(txtPwd1.Text);
                                                response.PWD = new byte[2];
                                                response.PWD[0] = pack.PWD[1];
                                                response.PWD[1] = pack.PWD[0];
                                                byte[] lens = new byte[2];
                                                if (havesubsequent)  //如果是多包，且是最后一帧
                                                {
                                                    lens = BitConverter.GetBytes((ushort)11);
                                                    response.CStart = PackageDefine.CStart_Pack;
                                                    response.CurPackCount = response.SumPackCount = pack.SumPackCount;
                                                }
                                                else
                                                {
                                                    lens = BitConverter.GetBytes((ushort)8);
                                                    response.CStart = PackageDefine.CStart;
                                                }
                                                response.CStart = PackageDefine.CStart;
                                                response.L0 = lens[0];
                                                response.L1 = lens[1];
                                                response.FUNCODE = pack.FUNCODE;
                                                response.IsUpload = false;

                                                response.SNum = pack.SNum;
                                                response.AddrFlag = pack.AddrFlag;
                                                if (SL651AllowOnLine)
                                                    response.End = PackageDefine.ESC;
                                                else
                                                    response.End = PackageDefine.EOT;
                                                byte[] bsenddata = response.ToResponseArray(true);
                                                response.CS = Package651.crc16(bsenddata, bsenddata.Length);

                                                bsenddata = response.ToResponseArray();
#if debug
                                                OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  发送响应帧:" + ConvertHelper.ByteToString(bsenddata, bsenddata.Length)));
#else
                                                OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  发送响应帧"));
#endif
                                                //Thread.Sleep(500); //3
                                                Send651(handler, bsenddata);
                                            }
                                        }
                                        #endregion
                                    }

                                    try
                                    {
                                        if (handler != null && SocketHelper.IsSocketConnected_Poll(handler))
                                            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                                    }
                                    catch { };

                                    packageBytes.Clear();
                                }
                            }
                            else
                            {
                                if (index == bytesRead)
                                {
                                    byte[] arr = packageBytes.ToArray();
                                    OnSendMsg(new SocketEventArgs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 收到错误帧数据:" + ConvertHelper.ByteToString(arr, arr.Length)));

                                }
                            }
                            #endregion
                        }
                        #endregion

                        index++;
                    }
                }
            }
            catch (ArgumentException argex)
            {
                if (bytesRead > 0)
                {
                    string str_buffer = ConvertHelper.ByteToString(state.buffer, bytesRead);
#if debug
                    OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + " " + argex.Message + ",错误数据:" + str_buffer));
#else
                    OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() +" "+ argex.Message));
#endif
                }
                logger.ErrorException("ReadCallback", argex);
                try
                {
                    handler.Shutdown(SocketShutdown.Both);
                    Thread.Sleep(5);
                    handler.Close();
                }
                catch { }
            }
            catch (SocketException sockex)
            {
                foreach (CallSocketEntity callsocket in lstClient)
                {
                    if (callsocket.ClientSocket.Equals(handler) && callsocket.TerId != -1)
                    {
                        string str_devtype = "";
                        if (callsocket.DevType == ConstValue.DEV_TYPE.Data_CTRL)
                        {
                            str_devtype = "终端";
                        }
                        else if (callsocket.DevType == ConstValue.DEV_TYPE.UNIVERSAL_CTRL)
                        {
                            str_devtype = "通用终端";
                        }
                        OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + " " + str_devtype + "[" + callsocket.TerId + "]下线!"));
                        callsocket.ClientSocket = null;

                        OnLineState_Interval = 1;  //发送下线消息
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("ReadCallback", ex);
                try
                {
                    //handler.Shutdown(SocketShutdown.Both);
                    //Thread.Sleep(5);
                    //handler.Close();
                }
                catch { }
            }
            finally
            {
                try
                {
                    if (handler != null && SocketHelper.IsSocketConnected_Poll(handler))
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
                catch(Exception ex)
                { };
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = ((SendObject)ar.AsyncState).workSocket;
                if (handler != null && SocketHelper.IsSocketConnected_Poll(handler))
                {
                    int bytesSent = handler.EndSend(ar);

                    bool AllowOnLine = false;
                    foreach (CallSocketEntity callentity in lstClient)
                    {
                        if (callentity.DevType == ((SendObject)ar.AsyncState).DevType && callentity.TerId == ((SendObject)ar.AsyncState).DevID)
                        {
                            AllowOnLine = callentity.AllowOnLine;
                            break;
                        }
                    }
                    if ((!AllowOnLine) && ((SendObject)ar.AsyncState).IsFinal)
                    {
                        //不主动下线
                        //handler.Shutdown(SocketShutdown.Both);
                        //OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + " 客户端" + handler.RemoteEndPoint.ToString() + "下线"));
                        //Thread.Sleep(5);
                        //handler.Close();
                    }
                }
            }
            catch (Exception e)
            {
                //logger.ErrorException(DateTime.Now.ToString() + " SendCallback", e);
                //OnSendMsg(new SocketEventArgs(e.ToString()));
            }
        }

        /// <summary>
        /// 是否需要校时判断
        /// </summary>
        /// <returns></returns>
        private bool NeedCheckTime(DateTime devTime)
        {
            TimeSpan ts = DateTime.Now - (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));   //0点校时
            if (Math.Abs(ts.TotalMinutes) < 10)
                return true;

            ts = DateTime.Now - devTime;   //设备时间与服务器时间相差两小时校时
            if (Math.Abs(ts.TotalMinutes) > 120)
                return true;

            return false;
        }

        public void GetTerminalOnLineState()
        {
            OnLineState_Interval = 1;  //利用定时器，直接修改时间间隔
        }

        /// <summary>
        /// 终端在线，下线
        /// </summary>
        /// <param name="DevType"></param>
        /// <param name="DevId"></param>
        /// <param name="AllowOnline"></param>
        public void SetOnLineClient(ConstValue.DEV_TYPE DevType, short DevId, bool AllowOnline)
        {
            if (lstClient == null)
            {
                lstClient = new List<CallSocketEntity>();
            }

            int index = -1;
            for (int i = 0; i < lstClient.Count; i++)
            {
                if ((lstClient[i].DevType == DevType) && (lstClient[i].TerId!=-1) && (lstClient[i].TerId == DevId))
                {
                    index = i;
                    break;
                }
            }
            if (AllowOnline)
            {
                CallSocketEntity client = null;
                if (index != -1)  //已存在不添加
                    client = lstClient[index];
                else
                {
                    client = new CallSocketEntity();
                    client.DevType = DevType;
                    client.TerId = DevId;
                    lstClient.Add(client);
                }
                Package package = new Package();
                package.DevType = DevType;
                package.DevID = DevId;
                package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
                package.C1 = (byte)UNIVERSAL_COMMAND.OnLine;
                package.DataLength = 1;
                byte[] data = new byte[package.DataLength];
                data[0] = (byte)0x01;
                package.Data = data;
                package.CS = package.CreateCS();

                client.AllowOnLine = true;
                if (client.ClientSocket != null)  //已连接，不发送，未连接马上发送
                {
                    ;
                }
                else
                {
                    if (client.lstWaitSendCmd == null)
                        client.lstWaitSendCmd = new List<SendPackageEntity>();
                    client.lstWaitSendCmd.Add(new SendPackageEntity(package));
                }
            }
            else
            {
                if (index > -1)  //已存在移除
                {
                    if (lstClient[index].ClientSocket != null)
                    {
                        try
                        {
                            Package package = new Package();
                            package.DevType = DevType;
                            package.DevID = DevId;
                            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
                            package.C1 = (byte)UNIVERSAL_COMMAND.OnLine;
                            package.DataLength = 1;
                            byte[] data = new byte[package.DataLength];
                            data[0] = (byte)0x01;
                            package.Data = data;
                            package.CS = package.CreateCS();

                            lstClient[index].AllowOnLine = false;

                            if (lstClient[index].ClientSocket != null)  //已连接，马上发送下线命令
                            {
                                try
                                {
                                    SendPackage(lstClient[index].ClientSocket, package, "离线命令");
                                    Thread.Sleep(10);
                                    lstClient[index].ClientSocket.Shutdown(SocketShutdown.Both);
                                    lstClient[index].ClientSocket.Close();
                                    lstClient[index].ClientSocket = null;
                                }
                                catch
                                {
                                    if (lstClient[index].lstWaitSendCmd == null)
                                        lstClient[index].lstWaitSendCmd = new List<SendPackageEntity>();
                                    lstClient[index].lstWaitSendCmd.Add(new SendPackageEntity(package));
                                }
                            }
                            else
                            {
                                if (lstClient[index].lstWaitSendCmd == null)
                                    lstClient[index].lstWaitSendCmd = new List<SendPackageEntity>();
                                lstClient[index].lstWaitSendCmd.Add(new SendPackageEntity(package));
                            }
                            
                        }
                        catch { }
                        finally
                        {
                            lstClient[index].ClientSocket = null;
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 终端招测数据
        /// </summary>
        /// <param name="DevType"></param>
        /// <param name="DevId"></param>
        /// <param name="?"></param>
        public void ClientCallData(ConstValue.DEV_TYPE DevType, short DevId, CallDataTypeEntity calldataType)
        {
            if (lstClient != null)
            {
                int index = -1;
                for (int i = 0; i < lstClient.Count; i++)
                {
                    if ((lstClient[i].DevType == DevType) && (lstClient[i].TerId!=-1) && (lstClient[i].TerId == DevId))
                    {
                        index = i;
                        break;
                    }
                }

                if (index > -1)
                {
                    List<Package> lstpack = new List<Package>();
                    #region lstpack.Add
                    if (calldataType.GetSim1)
                    {
                        Package package = new Package();
                        package.DevType = DevType;
                        package.DevID = DevId;
                        package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
                        package.C1 = (byte)UNIVERSAL_COMMAND.CallData_Sim1;
                        package.DataLength = 0;
                        byte[] data = new byte[package.DataLength];
                        package.Data = data;
                        package.CS = package.CreateCS();
                        lstpack.Add(package);
                    }
                    if (calldataType.GetSim2)
                    {
                        Package package = new Package();
                        package.DevType = DevType;
                        package.DevID = DevId;
                        package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
                        package.C1 = (byte)UNIVERSAL_COMMAND.CallData_Sim2;
                        package.DataLength = 0;
                        byte[] data = new byte[package.DataLength];
                        package.Data = data;
                        package.CS = package.CreateCS();
                        lstpack.Add(package);
                    }
                    if (calldataType.GetPluse)
                    {
                        Package package = new Package();
                        package.DevType = DevType;
                        package.DevID = DevId;
                        package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
                        package.C1 = (byte)UNIVERSAL_COMMAND.CallData_Pluse;
                        package.DataLength = 0;
                        byte[] data = new byte[package.DataLength];
                        package.Data = data;
                        package.CS = package.CreateCS();
                        lstpack.Add(package);
                    }
                    if (calldataType.GetRS4851)
                    {
                        Package package = new Package();
                        package.DevType = DevType;
                        package.DevID = DevId;
                        package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
                        package.C1 = (byte)UNIVERSAL_COMMAND.CallData_RS4851;
                        package.DataLength = 0;
                        byte[] data = new byte[package.DataLength];
                        package.Data = data;
                        package.CS = package.CreateCS();
                        lstpack.Add(package);
                    }
                    if (calldataType.GetRS4852)
                    {
                        Package package = new Package();
                        package.DevType = DevType;
                        package.DevID = DevId;
                        package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
                        package.C1 = (byte)UNIVERSAL_COMMAND.CallData_RS4852;
                        package.DataLength = 0;
                        byte[] data = new byte[package.DataLength];
                        package.Data = data;
                        package.CS = package.CreateCS();
                        lstpack.Add(package);
                    }
                    if (calldataType.GetRS4853)
                    {
                        Package package = new Package();
                        package.DevType = DevType;
                        package.DevID = DevId;
                        package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
                        package.C1 = (byte)UNIVERSAL_COMMAND.CallData_RS4853;
                        package.DataLength = 0;
                        byte[] data = new byte[package.DataLength];
                        package.Data = data;
                        package.CS = package.CreateCS();
                        lstpack.Add(package);
                    }
                    if (calldataType.GetRS4854)
                    {
                        Package package = new Package();
                        package.DevType = DevType;
                        package.DevID = DevId;
                        package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
                        package.C1 = (byte)UNIVERSAL_COMMAND.CallData_RS4854;
                        package.DataLength = 0;
                        byte[] data = new byte[package.DataLength];
                        package.Data = data;
                        package.CS = package.CreateCS();
                        lstpack.Add(package);
                    }
                    if (calldataType.GetRS4855)
                    {
                        Package package = new Package();
                        package.DevType = DevType;
                        package.DevID = DevId;
                        package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
                        package.C1 = (byte)UNIVERSAL_COMMAND.CallData_RS4855;
                        package.DataLength = 0;
                        byte[] data = new byte[package.DataLength];
                        package.Data = data;
                        package.CS = package.CreateCS();
                        lstpack.Add(package);
                    }
                    if (calldataType.GetRS4856)
                    {
                        Package package = new Package();
                        package.DevType = DevType;
                        package.DevID = DevId;
                        package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
                        package.C1 = (byte)UNIVERSAL_COMMAND.CallData_RS4856;
                        package.DataLength = 0;
                        byte[] data = new byte[package.DataLength];
                        package.Data = data;
                        package.CS = package.CreateCS();
                        lstpack.Add(package);
                    }
                    if (calldataType.GetRS4857)
                    {
                        Package package = new Package();
                        package.DevType = DevType;
                        package.DevID = DevId;
                        package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
                        package.C1 = (byte)UNIVERSAL_COMMAND.CallData_RS4857;
                        package.DataLength = 0;
                        byte[] data = new byte[package.DataLength];
                        package.Data = data;
                        package.CS = package.CreateCS();
                        lstpack.Add(package);
                    }
                    if (calldataType.GetRS4858)
                    {
                        Package package = new Package();
                        package.DevType = DevType;
                        package.DevID = DevId;
                        package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
                        package.C1 = (byte)UNIVERSAL_COMMAND.CallData_RS4858;
                        package.DataLength = 0;
                        byte[] data = new byte[package.DataLength];
                        package.Data = data;
                        package.CS = package.CreateCS();
                        lstpack.Add(package);
                    }
                    #endregion

                    foreach (Package package in lstpack)
                    {
                        if (lstClient[index].ClientSocket != null)
                        {
                            try
                            {
                                SendPackage(lstClient[index].ClientSocket, package, "招测数据命令");
                            }
                            catch
                            {
                                if (lstClient[index].lstWaitSendCmd == null)
                                    lstClient[index].lstWaitSendCmd = new List<SendPackageEntity>();
                                lstClient[index].lstWaitSendCmd.Add(new SendPackageEntity(package));
                            }
                        }
                        else
                        {
                            if (lstClient[index].lstWaitSendCmd == null)
                                lstClient[index].lstWaitSendCmd = new List<SendPackageEntity>();
                            lstClient[index].lstWaitSendCmd.Add(new SendPackageEntity(package));
                        }
                    }
                }
            }
        }

        #region SL651方法
        
        private void SendCallback651(IAsyncResult ar)
        {
            try
            {
                Socket handler = ((SendObject)ar.AsyncState).workSocket;
                int bytesSent = handler.EndSend(ar);
                //if (((SendObject)ar.AsyncState).IsFinal)  //如果是最后一帧，则关闭socket
                //{
                //    handler.Shutdown(SocketShutdown.Both);
                //    handler.Close();
                //}
            }
            catch (Exception e)
            {
                logger.ErrorException(DateTime.Now.ToString() + " SendCallback651", e);
                OnSendMsg(new SocketEventArgs(e.ToString()));
            }
        }

        public void Send651Cmd(Package651 pack)
        {
            if (lstClient == null)
                lstClient = new List<CallSocketEntity>();

            bool isExist = false;  //是否存在
            if (lstClient.Count > 0)
            {
                foreach (CallSocketEntity sock in lstClient)
                {
                    if (pack.A1 == sock.A1 && pack.A2 == sock.A2 && pack.A3 == sock.A3 && pack.A4 == sock.A4 && pack.A5 == sock.A5)
                    {
                        isExist = true;
                        if (sock.lstWaitSendCmd != null && sock.lstWaitSendCmd.Count > 0)
                        {
                            for(int i = 0; i < sock.lstWaitSendCmd.Count; i++)
                            {
                                //命令已经存在,直接返回
                                if (sock.lstWaitSendCmd[i].SendPackage651 != null && sock.lstWaitSendCmd[i].SendPackage651.Equals(pack))
                                {
                                    return;
                                }
                            }
                        }
                        bool isOnline = false;
                        isOnline = SocketHelper.IsSocketConnected_Poll(sock.ClientSocket);
                        bool issend = false;  //是否已发送
                        if (isOnline)
                        {
                            try
                            {
                                if (pack.End != PackageDefine.ENQ)  //如果本来就是询问命令，则还是ENQ(05)发送下去，不管在不在线，因为总是要回应
                                {
                                    if (SL651AllowOnLine)
                                        pack.End = PackageDefine.ESC;
                                    else
                                        pack.End = PackageDefine.EOT;
                                }
                                byte[] bsenddata = pack.ToResponseArray(true);
                                pack.CS = Package651.crc16(bsenddata, bsenddata.Length);
                                bsenddata = pack.ToResponseArray();
                                SendObject sendObj = new SendObject();
                                sendObj.workSocket = sock.ClientSocket;
                                sock.ClientSocket.BeginSend(bsenddata, 0, bsenddata.Length, 0, new AsyncCallback(SendCallback651), sendObj);
                                OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  发送命令帧:" + ConvertHelper.ByteToString(bsenddata, bsenddata.Length), ConstValue.MSMQTYPE.Msg_Socket));
                                issend = true;
                            }
                            catch
                            {
                                ;
                            }
                        }
                        if (!issend)  //发送失败,添加到待发送列表中
                        {
                            SendPackageEntity sendpack = new SendPackageEntity();
                            sendpack.SendPackage651 = pack;
                            sock.lstWaitSendCmd.Add(sendpack);
                        }
                    
                        break;
                    }
                }
            }

            if (!isExist)  //不存在新增一个
            {
                CallSocketEntity newSocket = new CallSocketEntity();
                newSocket.A1 = pack.A1;
                newSocket.A2 = pack.A2;
                newSocket.A3 = pack.A3;
                newSocket.A4 = pack.A4;
                newSocket.A5 = pack.A5;
                SendPackageEntity sendpack = new SendPackageEntity();
                sendpack.SendPackage651 = pack;
                newSocket.lstWaitSendCmd.Add(sendpack);
                lstClient.Add(newSocket);
            }
        }

        public void GetSL651WaitSendCmd()
        {
            List<Package651> lstWaitSendCmd = new List<Package651>();
            if (lstClient != null)
            {
                foreach (CallSocketEntity sockeentity in lstClient)
                {
                    if(sockeentity.lstWaitSendCmd!=null)
                        foreach (SendPackageEntity packentity in sockeentity.lstWaitSendCmd)
                        {
                            if (packentity.SendPackage651 != null)
                                lstWaitSendCmd.Add(packentity.SendPackage651);
                        }
                }
            }
            SocketEntity msmqEnt = new SocketEntity();
            msmqEnt.MsgType= ConstValue.MSMQTYPE.Get_SL651_WaitSendCmd;
            msmqEnt.Msg = SmartWaterSystem.JSONSerialize.JsonSerialize<List<Package651>>(lstWaitSendCmd);
            SocketEventArgs socketargs = new SocketEventArgs(ConstValue.MSMQTYPE.Get_SL651_WaitSendCmd, msmqEnt);
            OnSendMsg(socketargs);
        }

        public void DelSL651WaitSendCmd(byte A1,byte A2,byte A3,byte A4,byte A5,byte funcode)
        {
            if (lstClient != null)
            {
                for (int i = 0; i < lstClient.Count; i++)
                {
                    if(lstClient[i].lstWaitSendCmd!=null)
                        for (int j = 0; j < lstClient[i].lstWaitSendCmd.Count; j++)
                        {
                            if(lstClient[i].lstWaitSendCmd[j].SendPackage651!=null)
                                if (lstClient[i].lstWaitSendCmd[j].SendPackage651.A1 == A1 && lstClient[i].lstWaitSendCmd[j].SendPackage651.A2 == A2 && lstClient[i].lstWaitSendCmd[j].SendPackage651.A3 == A3 &&
                                    lstClient[i].lstWaitSendCmd[j].SendPackage651.A4 == A4 && lstClient[i].lstWaitSendCmd[j].SendPackage651.A5 == A5 && lstClient[i].lstWaitSendCmd[j].SendPackage651.FUNCODE == funcode)
                                {
                                    lstClient[i].lstWaitSendCmd.RemoveAt(j);
                                }
                        }
                }
            }
        }

        private bool Send651(Socket socket, byte[] bsenddata)
        {
            try
            {
                SendObject sendObj = new SendObject();
                sendObj.workSocket = socket;
                socket.BeginSend(bsenddata, 0, bsenddata.Length, 0, new AsyncCallback(SendCallback651), sendObj);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 更新数据库同步时间
        /// </summary>
        public void SetSQL_SyncingStatus()
        {
            SQLSync_Time = DateTime.Now;
        }

        private void SendPackage(Socket handler,Package pack,string prompt)
        {
            byte[] bsenddata = pack.ToArray();
#if debug
            OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  发送" + prompt + "帧:" + ConvertHelper.ByteToString(bsenddata, bsenddata.Length)));
#else
                                        OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  发送" + prompt + "帧"));
#endif
            SendObject sendObj = new SendObject();
            sendObj.workSocket = handler;
            sendObj.IsFinal = pack.IsFinal;
            sendObj.DevType = pack.DevType;
            sendObj.DevID = pack.DevID;
            handler.BeginSend(bsenddata, 0, bsenddata.Length, 0, new AsyncCallback(SendCallback), sendObj);
        }

        public void GetSL651AllowOnLineFlag()
        {
            OnSendMsg(new SocketEventArgs(ConstValue.MSMQTYPE.Get_SL651_AllowOnlineFlag, SL651AllowOnLine.ToString()));
        }

        public void SetSL651AllowOnLineFlag(bool allowOnline)
        {
            SL651AllowOnLine = allowOnline;
        }

        /// <summary>
        /// 接收socket发送过来的消息
        /// </summary>
        /// <param name="Msg"></param>
        private void ReceiveMsg(SocketEntity Msg)
        {
            if (Msg != null)
            {
                if (Msg.MsgType == ConstValue.MSMQTYPE.Cmd_Online)
                    GlobalValue.Instance.SocketMag.SetOnLineClient(Msg.DevType, Msg.DevId, Msg.AllowOnline);
                else if (Msg.MsgType == ConstValue.MSMQTYPE.Get_OnLineState)
                    GlobalValue.Instance.SocketMag.GetTerminalOnLineState();
                else if (Msg.MsgType == ConstValue.MSMQTYPE.Cmd_CallData && Msg.CallDataType != null)
                    GlobalValue.Instance.SocketMag.ClientCallData(Msg.DevType, Msg.DevId, Msg.CallDataType);
                else if (Msg.MsgType == ConstValue.MSMQTYPE.SQL_Syncing)
                    GlobalValue.Instance.SocketMag.SetSQL_SyncingStatus();
                else if (Msg.MsgType == ConstValue.MSMQTYPE.SL651_Cmd)
                {
                    if (Msg.Pack651 != null)
                        GlobalValue.Instance.SocketMag.Send651Cmd(Msg.Pack651);
                    GlobalValue.Instance.SocketMag.GetSL651WaitSendCmd();  //发送完毕获取待发送SL651命令列表
                }
                else if (Msg.MsgType == ConstValue.MSMQTYPE.Set_SL651_AllowOnlineFlag)
                {
                    GlobalValue.Instance.SocketMag.SetSL651AllowOnLineFlag(Msg.Msg.ToLower() == "true");
                    GlobalValue.Instance.SocketMag.GetSL651AllowOnLineFlag();
                }
                else if (Msg.MsgType == ConstValue.MSMQTYPE.Get_SL651_AllowOnlineFlag)
                    GlobalValue.Instance.SocketMag.GetSL651AllowOnLineFlag();
                else if (Msg.MsgType == ConstValue.MSMQTYPE.Get_SL651_WaitSendCmd)
                {
                    GlobalValue.Instance.SocketMag.GetSL651WaitSendCmd();
                }
                else if (Msg.MsgType == ConstValue.MSMQTYPE.Del_SL651_WaitSendCmd)
                {
                    //del
                    GlobalValue.Instance.SocketMag.DelSL651WaitSendCmd(Msg.A1, Msg.A2, Msg.A3, Msg.A4, Msg.A5, Msg.SL651Funcode);
                    GlobalValue.Instance.SocketMag.GetSL651WaitSendCmd();
                }

                //Other
            }
        }

        /// <summary>
        /// socket发送消息
        /// </summary>
        /// <param name="sock"></param>
        /// <param name="entitymsg"></param>
        /// <param name="needreply">是否需要重试</param>
        /// <returns></returns>
        public bool SocketSend(Socket sock, string entitymsg,bool needreply)
        {
            try {
                if (string.IsNullOrEmpty(entitymsg))
                    return true;
                if(!SocketHelper.IsSocketConnected_Poll(sock))
                {
                    return false;
                }
                entitymsg = SocketHelper.SocketMsgSplit + entitymsg + SocketHelper.SocketMsgSplit;
                byte[] bs = Encoding.UTF8.GetBytes(entitymsg);

                SendObject sendObj = new SendObject();
                sendObj.workSocket = sock;
                sock.BeginSend(bs, 0, bs.Length, 0, new AsyncCallback(SmartSendCallback), sendObj);
                return true;
            }
            catch {
                return false;
            }
        }

        private void SmartSendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = ((SendObject)ar.AsyncState).workSocket;
                int bytesSent = handler.EndSend(ar);
            }
            catch (Exception e)
            {
            }
        }

    }

}
