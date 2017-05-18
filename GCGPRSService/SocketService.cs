using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using Common;
using Entity;
using System.Text;
using SmartWaterSystem;
using System.Diagnostics;
using BLL;

namespace GCGPRSService
{
    public class SocketEventArgs : EventArgs
    {
        private SocketEntity _jsonmsg;
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
            _jsonmsg = msmqEntity;
        }

        public SocketEventArgs(Entity.ConstValue.MSMQTYPE MsgType, SocketEntity msg)
        {
            msg.MsgType = MsgType;
            _jsonmsg = msg;
        }
        
        public SocketEventArgs(ColorType Showtype,string msg)
        {
            SocketEntity msmqEntity = new SocketEntity();
            msmqEntity.MsgType = ConstValue.MSMQTYPE.Msg_Socket;
            msmqEntity.ShowType = Showtype;
            msmqEntity.Msg = msg;
            _jsonmsg = msmqEntity;
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
    public class SocketManager
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("SocketService");
        public ManualResetEvent allDone = new ManualResetEvent(false);
        Thread t_socket;
        Thread t_Interval;
        Socket listener;
        bool isRunning = false;
        int maxSmartClient = 8;   //SmartWaterSystem.exe 的最大socket连接数
        object obj_smartsocket = new object();      //SmartWaterSystem.exe的socket连接列表锁
        List<SmartSocketEntity> lstSmartClient = new List<SmartSocketEntity>(); //SmartWaterSystem.exe 的socket连接列表

        int SQL_Interval = 1 * 63;  //数据更新时间间隔(second)
        DateTime SQLSync_Time = DateTime.Now.AddHours(-1);  //数据库同步时间,-1:才开启，马上同步一次数据

        int OnLineState_Interval = 5 * 60; //终端在线状态更新时间间隔(second)
        int CheckThread_Interval = 2 * 60; //检查线程状态间隔(second)
        int ClearHistoryData_Interval = 1*60;  //清除历史数据时间间隔,程序启动后1分钟开始清理一次
        int CheckSend_Interval = 5;        //检查发送状态,WaitForMultipleObjects问题

        bool SL651AllowOnLine = false;  //SL651协议终端是否在线,默认不在线

        Stack<SocketAsyncEventArgs> s_lst = new Stack<SocketAsyncEventArgs>();

        public Service1.DumpThreadCallback Dumpcallback;

        StringBuilder newmsg = new StringBuilder(1024);
        public void OnSendMsg(SocketEventArgs e)
        {
            lock (obj_smartsocket)
            {
                string msg = JSONSerialize.JsonSerialize_Newtonsoft(e.JsonMsg);
                newmsg.Clear();
                newmsg.Append(SocketHelper.SocketMsgSplit);
                newmsg.Append(msg);
                newmsg.Append(SocketHelper.SocketMsgSplit);
                byte[] bs = Encoding.UTF8.GetBytes(newmsg.ToString());
                
                foreach (SmartSocketEntity smartsock in lstSmartClient)
                {
                    if (!SocketSend(smartsock.ClientSocket, bs))
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
                if(CheckSend_Interval-- == 0)
                {
                    CheckSend_Interval = 5;

                    //System.Diagnostics.Debug.WriteLine("GlobalValue.Instance.GPRS_PreFrameData.Count:" + GlobalValue.Instance.GPRS_PreFrameData.Count);
                    
                    if (GlobalValue.Instance.GPRS_AlarmFrameData.Count > 0)
                        GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertAlarm);
                    if (GlobalValue.Instance.GPRS_PreFrameData.Count > 0)
                        GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertPreValue);
                    if (GlobalValue.Instance.GPRS_FlowFrameData.Count > 0)
                        GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertFlowValue);
                    if (GlobalValue.Instance.GPRS_PrectrlFrameData.Count > 0)
                        GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertPrectrlValue);
                    if (GlobalValue.Instance.GPRS_UniversalFrameData.Count > 0)
                        GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertUniversalValue);
                    if (GlobalValue.Instance.GPRS_OLWQFrameData.Count > 0)
                        GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertOLWQValue);
                    if (GlobalValue.Instance.GPRS_HydrantFrameData.Count > 0)
                        GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertHydrantValue);
                    if (GlobalValue.Instance.GPRS_NoiseFrameData.Count > 0)
                        GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertNoiseValue);
                    if (GlobalValue.Instance.GPRS_WaterworkerFrameData.Count > 0)
                        GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertWaterworkerValue);
                }
                if (SQL_Interval-- == 0)
                {
                    TimeSpan ts = DateTime.Now - SQLSync_Time;
                    if (Math.Abs(ts.TotalSeconds) > 15)
                    {
                        SQL_Interval = 2 * 63;
                        GlobalValue.Instance.SocketSQLMag.Send(SQLType.GetSendParm); //获得上传参数
                        GlobalValue.Instance.SocketSQLMag.Send(SQLType.GetUniversalConfig); //获取解析帧的配置数据
                        GlobalValue.Instance.SocketSQLMag.Send(SQLType.GetRectifyValue); //获取偏移值
                    }
                    else
                    {
                        SQL_Interval = 20;
                    }
                }
                if (OnLineState_Interval-- == 0)
                {
                    OnLineState_Interval = 5 * 60;
                    SocketEntity msmqEntity = new SocketEntity();
                    msmqEntity.lstOnLine = new List<OnLineTerEntity>();
                    lock (GlobalValue.Instance.lstClientLock)
                    {
                        for (int i =GlobalValue.Instance.lstClient.Count-1;i>=0; i--)
                        {
                            if ((DateTime.Now - GlobalValue.Instance.lstClient[i].ModifyTime).TotalHours > 36)  //超过36小时未使用对象自动销毁
                            {
                                GlobalValue.Instance.lstClient.RemoveAt(i);
                            }
                            else
                            {
                                //如果重试次数大于0并且发送时间与当前时间相差不超过一天，保留
                                if (GlobalValue.Instance.lstClient[i].lstWaitSendCmd != null && GlobalValue.Instance.lstClient[i].lstWaitSendCmd.Count > 0)
                                {
                                    //List<SendPackageEntity> lst_save_Pack = new List<SendPackageEntity>();
                                    for (int j =GlobalValue.Instance.lstClient[i].lstWaitSendCmd.Count-1;j>=0; j--)
                                    {
                                        TimeSpan ts = DateTime.Now - GlobalValue.Instance.lstClient[i].lstWaitSendCmd[j].SendTime;
                                        if ((Math.Abs(ts.TotalHours) > 24))
                                        {

                                            GlobalValue.Instance.lstClient[i].lstWaitSendCmd.RemoveAt(j);
                                        }
                                    }
                                }
                                //在线检查
                                bool add = false;
                                add = SocketHelper.IsSocketConnected_Poll(GlobalValue.Instance.lstClient[i].ClientSocket);

                                if (add && GlobalValue.Instance.lstClient[i].TerId != -1)
                                {
                                    msmqEntity.lstOnLine.Add(new OnLineTerEntity(GlobalValue.Instance.lstClient[i].DevType, GlobalValue.Instance.lstClient[i].TerId));
                                }
                            }
                        }
                    }
                    OnSendMsg(new SocketEventArgs(ConstValue.MSMQTYPE.Data_OnLineState, msmqEntity));

                    //检查lstSmartClient保留在线的socket客户端
                    lock (obj_smartsocket)
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
                        lsttmp = null;
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
                if(ClearHistoryData_Interval -- == 0)
                {
                    ClearHistoryData_Interval = 3* 24 * 60 * 60;  //3天清理一次
                    TerminalDataBLL dataBll = new TerminalDataBLL();
                    int clearcount = dataBll.ClearHistoryData(DateTime.Now.AddYears(-1));  //清除一年之前的数据
                    OnSendMsg(new SocketEventArgs(ColorType.Public, DateTime.Now.ToString() + " 清除历史数据完成,清理数量:" + clearcount));
                }
            }
        }


        public void Close()
        {
            try
            {
                isRunning = false;
                if (listener != null)
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
            catch (Exception e)
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
            if (string.IsNullOrEmpty(Settings.Instance.GetString(SettingKeys.GPRS_PORT)))
            {
                GlobalValue.Instance.lstStartRecord.Add("GPRS远传服务停止,请设置IP与端口号!");
                t_socket.Abort();
                return;
            }
            //Thread.Sleep(10 * 1000);
            int port = Settings.Instance.GetInt(SettingKeys.GPRS_PORT);
            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
            // Create a TCP/IP socket.     
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.ReceiveTimeout = 30 * 1000;  //设置超时
            listener.SendTimeout = 30 * 1000;
            try
            {
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

                OnSendMsg(new SocketEventArgs(ColorType.Other,DateTime.Now.ToString() + " 客户端" + handler.RemoteEndPoint.ToString() + "上线"));

                state.workSocket = handler;
            }
            catch (Exception ex)
            {
                OnSendMsg(new SocketEventArgs(ColorType.Error,DateTime.Now.ToString() + " 接收错误:" + ex.Message));
                logger.ErrorException("AcceptCallback", ex);
            }
            finally
            {
                try {
                    if (handler != null)
                    {
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                    }

                }
                catch { }
            }
        }

        private void ReadCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            int bytesRead = 0;
            List<byte> packageBytes = null;
            List<byte> ReceiveBytes = null;
            try
            {
                // Read data from the client socket. 
                bytesRead = handler.EndReceive(ar);
                if (bytesRead > 0)
                {
                    if (!SocketHelper.IsSocketConnected_Poll(handler))
                        return;
                    //判断是否是SmartWater的连接
                    bool isclientconn = false;
                    if (state.buffer[0] == PackageDefine.BeginByte && state.buffer[bytesRead - 1] == PackageDefine.EndByte)  //增加帧头尾判断，防止数据中有'@@@'的数据导致判断错误
                        isclientconn = false;
                    else {
                        for (int i = 0; i < bytesRead - SocketHelper.SocketByteSplit.Length; i++)
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
                                        if (smartindex > -1) //心跳包
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
                                                                if (!SocketSend(handler, str))   //发送缓存的消息
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
                                                            mEntity.MsgType = ConstValue.MSMQTYPE.Msg_Socket;
                                                            mEntity.ShowType = ColorType.Error;
                                                            mEntity.Msg = "已达到最大连接数,拒接连接";
                                                            SocketSend(handler, JSONSerialize.JsonSerialize_Newtonsoft(mEntity));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lstSmartClient.Add(new SmartSocketEntity(handler, strip, SmartID));
                                                        SocketEntity mEntity = new SocketEntity();
                                                        foreach (string startrec in GlobalValue.Instance.lstStartRecord)  //将启动的信息发送给新上线的客户端
                                                        {
                                                            mEntity.MsgType = ConstValue.MSMQTYPE.Msg_Socket;
                                                            mEntity.ShowType = ColorType.Public;
                                                            mEntity.Msg = startrec;
                                                            SocketSend(handler, JSONSerialize.JsonSerialize_Newtonsoft(mEntity));
                                                        }
                                                        using (var process = System.Diagnostics.Process.GetCurrentProcess())
                                                        using (var p1 = new PerformanceCounter("Process", "Working Set - Private", process.ProcessName))
                                                        {
                                                            mEntity.MsgType = ConstValue.MSMQTYPE.Msg_Socket;
                                                            mEntity.ShowType = ColorType.Public;
                                                            mEntity.Msg = DateTime.Now.ToString() + " 服务当前专用工作集(" + p1.NextValue() / 1024 + "K),工作设置内存(" + (process.WorkingSet64 / 1024 + "K),提交大小(" + process.PrivateMemorySize64 / 1024) + "K),GC内存(" + GC.GetTotalMemory(true) / 1024 + "K)";
                                                            SocketSend(handler, JSONSerialize.JsonSerialize_Newtonsoft(mEntity));
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

                    packageBytes = new List<byte>();
                    ReceiveBytes = new List<byte>();

                    for (int i = 0; i < bytesRead; i++)
                    {
                        ReceiveBytes.Add(state.buffer[i]);
                    }

                    string str_source = ConvertHelper.ByteToString(state.buffer, bytesRead);
                    OnSendMsg(new SocketEventArgs(ColorType.OriginalFrame,DateTime.Now.ToString() + " 接收客户端" + handler.RemoteEndPoint.ToString() + ", 收到(原始)数据:" + str_source));

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

                                int lstClientIndex = GlobalValue.Instance.lstClientAdd(pack.DevID, pack.DevType);    //找到终端所在位置,不存在就新建
                                GlobalValue.Instance.lstClient[lstClientIndex].ClientSocket = handler;                //保存socket连接对象
                                if (pack.CommandType == CTRL_COMMAND_TYPE.RESPONSE_BY_SLAVE)  //接受到应答,判断是否D11是否为1,如果为0,表示没有数据需要读
                                {
                                    #region 处理响应帧
                                    OnSendMsg(new SocketEventArgs(ColorType.DataFrame, DateTime.Now.ToString() + " 收到响应帧:" + str_source + "  " + (new GPRSCmdMSg()).GetPackageDesc(pack)));
                                    //OnSendMsg(new SocketEventArgs(ColorType.DataFrame, DateTime.Now.ToString() + "  " + (new GPRSCmdMSg()).GetPackageDesc(pack)));

                                    #region 招测
                                    if (pack.C1 == (byte)UNIVERSAL_COMMAND.CallData_Pre1 || pack.C1 == (byte)UNIVERSAL_COMMAND.CallData_Sim1
                                        || pack.C1 == (byte)UNIVERSAL_COMMAND.CallData_Sim2 || pack.C1 == (byte)UNIVERSAL_COMMAND.CallData_Pluse
                                        || pack.C1 == (byte)UNIVERSAL_COMMAND.CallData_RS4851 || pack.C1 == (byte)UNIVERSAL_COMMAND.CallData_RS4852
                                        || pack.C1 == (byte)UNIVERSAL_COMMAND.CallData_RS4853 || pack.C1 == (byte)UNIVERSAL_COMMAND.CallData_RS4854)
                                    {

                                        new Protol68().UniversalTerCallData(pack);
                                    }
                                    #endregion

                                    lock (GlobalValue.Instance.lstClientLock)
                                    {
                                        if (GlobalValue.Instance.lstClient[lstClientIndex].lstWaitSendCmd != null)
                                        {
                                            for (int j = GlobalValue.Instance.lstClient[lstClientIndex].lstWaitSendCmd.Count-1;j>=0; j--)  //收到响应帧
                                            {
                                                if (GlobalValue.Instance.lstClient[lstClientIndex].lstWaitSendCmd[j].SendPackage.C1 == pack.C1 &&
                                                    GlobalValue.Instance.lstClient[lstClientIndex].lstWaitSendCmd[j].SendPackage.Data == pack.Data)
                                                {
                                                    GlobalValue.Instance.lstClient[lstClientIndex].lstWaitSendCmd.RemoveAt(j);      //移除收到的帧
                                                }
                                            }
                                        }


                                        for (int z =GlobalValue.Instance.lstClient[lstClientIndex].lstWaitSendCmd.Count-1;z>=0; z--)
                                        {
                                            if (GlobalValue.Instance.lstClient[lstClientIndex].lstWaitSendCmd[z].SendPackage.C1 == pack.C1)
                                            {
                                                if (GlobalValue.Instance.lstClient[lstClientIndex].lstWaitSendCmd[z].TableId == -1 || GlobalValue.Instance.lstClient[lstClientIndex].lstWaitSendCmd[z].TableId == -2)  //-1用在生成的自动校时,-2用在下送的命令帧
                                                {
                                                    GlobalValue.Instance.lstClient[lstClientIndex].lstWaitSendCmd.RemoveAt(z);   //生成的自动校时直接删除
                                                    break;      //处理完后直接退出循环,防止如噪声远传的话，记录仪1和2一起发送相同功能码，收到响应帧区分不出
                                                }
                                                else
                                                {
                                                    GPRSCmdFlag flag = new GPRSCmdFlag();
                                                    flag.TableId = GlobalValue.Instance.lstClient[lstClientIndex].lstWaitSendCmd[z].TableId;
                                                    GlobalValue.Instance.lstClient[lstClientIndex].lstWaitSendCmd[z].SendedFlag = 1;  //标记为已发送，防止后边重发
                                                    flag.SendCount = 0;  //0:成功发送,收到响应帧
                                                    GlobalValue.Instance.lstSendedCmdId.Add(flag);
                                                    break;      //处理完后直接退出循环,防止如噪声远传的话，记录仪1和2一起发送相同功能码，收到响应帧区分不出
                                                }
                                            }
                                        }

                                        for (int j = 0; j < GlobalValue.Instance.lstClient.Count; j++)
                                        {
                                            for (int z = GlobalValue.Instance.lstClient[j].lstWaitSendCmd.Count - 1; z >= 0; z--)
                                            {
                                                //失败次数多于3次,则删除
                                                if (GlobalValue.Instance.lstClient[j].lstWaitSendCmd[z].SendCount > 3)
                                                {
                                                    if (GlobalValue.Instance.lstClient[j].lstWaitSendCmd[z].TableId == -1)  //-1用在生成的自动校时
                                                    {
                                                        GlobalValue.Instance.lstClient[j].lstWaitSendCmd.RemoveAt(z);   //生成的自动校时直接删除
                                                    }
                                                    else if (GlobalValue.Instance.lstClient[j].lstWaitSendCmd[z].TableId == -2)   //-2用在下送的命令帧
                                                    {
                                                        GlobalValue.Instance.lstClient[j].lstWaitSendCmd.RemoveAt(z);   //下送的命令帧直接删除
                                                    }
                                                    else
                                                    {
                                                        GPRSCmdFlag flag = new GPRSCmdFlag();
                                                        flag.TableId = GlobalValue.Instance.lstClient[j].lstWaitSendCmd[z].TableId;
                                                        flag.SendCount = GlobalValue.Instance.lstClient[j].lstWaitSendCmd[z].SendCount;  //未收到响应帧,发送次数超过限制
                                                        GlobalValue.Instance.lstSendedCmdId.Add(flag);
                                                    }
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
                                    OnSendMsg(new SocketEventArgs(ColorType.DataFrame, DateTime.Now.ToString() + " 收到帧数据:" + str_frame));
#else
                                    OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + " 收到帧数据"));
#endif
                                    #region 解析数据
                                    new Protol68().ProcData(state, pack, str_frame, out bNeedCheckTime);
                                    #endregion

                                    response.DevID = pack.DevID;
                                    //将在线信息发送给UI更新
                                    SocketEntity msmqEntity = new SocketEntity();
                                    msmqEntity.lstOnLine = new List<OnLineTerEntity>();
                                    msmqEntity.lstOnLine.Add(new OnLineTerEntity(GlobalValue.Instance.lstClient[lstClientIndex].DevType, GlobalValue.Instance.lstClient[lstClientIndex].TerId));
                                    OnSendMsg(new SocketEventArgs(ConstValue.MSMQTYPE.Data_OnLineState, msmqEntity));

                                    //Thread.Sleep(500);
                                }

                                #region 发送后续命令帧
                                if (((GlobalValue.Instance.lstClient[lstClientIndex].lstWaitSendCmd.Count > 0)
                                    || bNeedCheckTime || lstCommandPack.Count > 0) && pack.IsFinal)
                                {
                                    bool isupdate = false;
                                    for (int j = 0; j < GlobalValue.Instance.lstClient[lstClientIndex].lstWaitSendCmd.Count; j++)
                                    {
                                        if (GlobalValue.Instance.lstClient[lstClientIndex].lstWaitSendCmd[j].SendedFlag == 0)
                                        {
                                            Package CommandPack = new Package();
                                            CommandPack.DevID = pack.DevID;
                                            CommandPack.DevType = pack.DevType;
                                            CommandPack.C0 = GlobalValue.Instance.lstClient[lstClientIndex].lstWaitSendCmd[j].SendPackage.C0;
                                            CommandPack.C1 = GlobalValue.Instance.lstClient[lstClientIndex].lstWaitSendCmd[j].SendPackage.C1;
                                            CommandPack.Data = GlobalValue.Instance.lstClient[lstClientIndex].lstWaitSendCmd[j].SendPackage.Data;
                                            CommandPack.DataLength = GlobalValue.Instance.lstClient[lstClientIndex].lstWaitSendCmd[j].SendPackage.DataLength;
                                            //CommandPack.CS = CommandPack.CreateCS();
                                            lstCommandPack.Add(CommandPack);
                                            if (!isupdate)  //处理完后直接退出循环,防止如噪声远传的话，记录仪1和2一起发送相同功能码，区分不出
                                            {
                                                GlobalValue.Instance.lstClient[lstClientIndex].lstWaitSendCmd[j].SendCount++;  //记录发送次数
                                                isupdate = true;
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
                                            GlobalValue.Instance.lstClientAdd(pack_time.DevID, pack_time.DevType, (int)PackFromType.P68CheckTime, pack_time, true);
                                        }

                                        lstCommandPack.Add(pack_time);  //校时命令添加进来,下边只发送第一条命令,加入校时命令，用于后面判断是不是最后一个帧
                                    }
                                    #endregion
                                    byte[] bsenddata;
                                    SendObject sendObj = new SendObject();
                                    if (pack.CommandType == CTRL_COMMAND_TYPE.REQUEST_BY_SLAVE)//接收到的数据帧则需要应答响应帧
                                    {
                                        #region 回复响应帧
                                        response.DevType = pack.DevType;
                                        response.C0 = 0x40;
                                        if (lstCommandPack.Count > 0)
                                            response.C0 = ((byte)(response.C0 | 0x08));  //不是最后一个帧,主站发出的应答帧，有后续命令
                                        response.C1 = (byte)pack.C1;
                                        response.DataLength = 0;
                                        response.Data = null;
                                        response.CS = response.CreateCS();

                                        bsenddata = response.ToArray();
                                        Thread.Sleep(300);  //帧之间间隔
#if debug
                                        OnSendMsg(new SocketEventArgs(ColorType.DataFrame, DateTime.Now.ToString() + "  发送响应帧:" + ConvertHelper.ByteToString(bsenddata, bsenddata.Length)));
#else
                                                OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  发送响应帧"));
#endif
                                        sendObj.workSocket = handler;
                                        sendObj.IsFinal = false;
                                        sendObj.DevType = pack.DevType;
                                        sendObj.DevID = pack.DevID;

                                        SocketSend(handler, bsenddata);
                                        //handler.BeginSend(bsenddata, 0, bsenddata.Length, 0, new AsyncCallback(SendCallback), sendObj);
                                        #endregion
                                    }

                                    if (lstCommandPack.Count > 0)
                                    {
                                        Package commandpack = lstCommandPack[0];  //i
                                        if (0 != (lstCommandPack.Count - 1))  //i
                                            commandpack.C0 = ((byte)(commandpack.C0 | 0x08));  //不是最后一个帧
                                        commandpack.CS = commandpack.CreateCS();
                                        bsenddata = commandpack.ToArray();

                                        Thread.Sleep(800);  //帧之间间隔
                                        //OnSendMsg(new SocketEventArgs(ColorType.DataFrame,DateTime.Now.ToString() + "  "+(new GPRSCmdMSg()).GetPackageDesc(commandpack)));
#if debug
                                        OnSendMsg(new SocketEventArgs(ColorType.DataFrame, DateTime.Now.ToString() + "  发送命令帧:" + ConvertHelper.ByteToString(bsenddata, bsenddata.Length) + "  " + (new GPRSCmdMSg()).GetPackageDesc(commandpack)));
#else
                                        OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  发送响应帧"));
#endif
                                        sendObj = new SendObject();
                                        sendObj.workSocket = handler;
                                        sendObj.IsFinal = (0 == (lstCommandPack.Count - 1)) ? true : false;  //i
                                        sendObj.DevType = pack.DevType;
                                        sendObj.DevID = pack.DevID;
                                        SocketSend(handler, bsenddata);
                                        //handler.BeginSend(bsenddata, 0, bsenddata.Length, 0, new AsyncCallback(SendCallback), sendObj);
                                    }
                                }
                                else if (pack.CommandType == CTRL_COMMAND_TYPE.REQUEST_BY_SLAVE) //接收到的数据帧则需要应答响应帧
                                {
                                    response.CommandType = CTRL_COMMAND_TYPE.RESPONSE_BY_MASTER;
                                    response.DevType = pack.DevType;
                                    response.C1 = (byte)pack.C1;
                                    response.DataLength = 0;
                                    response.Data = null;
                                    response.CS = response.CreateCS();

                                    byte[] bsenddata = response.ToArray();

                                    Thread.Sleep(300);  //帧之间间隔
#if debug
                                    OnSendMsg(new SocketEventArgs(ColorType.DataFrame, DateTime.Now.ToString() + "  发送响应帧:" + ConvertHelper.ByteToString(bsenddata, bsenddata.Length)));
#else
                                        OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  发送响应帧"));
#endif
                                    SendObject sendObj = new SendObject();
                                    sendObj.workSocket = handler;
                                    sendObj.IsFinal = pack.IsFinal;
                                    sendObj.DevType = pack.DevType;
                                    sendObj.DevID = pack.DevID;
                                    if (handler != null && SocketHelper.IsSocketConnected_Poll(handler))
                                        SocketSend(handler, bsenddata);
                                    //handler.BeginSend(bsenddata, 0, bsenddata.Length, 0, new AsyncCallback(SendCallback), sendObj);
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
                                    OnSendMsg(new SocketEventArgs(ColorType.DataFrame,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 收到帧数据:" + ConvertHelper.ByteToString(arr, arr.Length)));
#else
                                    OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  收到帧数据"));
#endif
                                    int clientindex = -1;  //查找或添加
                                    for (int i = 0; i < GlobalValue.Instance.lstClient.Count; i++)
                                    {
                                        if (GlobalValue.Instance.lstClient[i].A1 == pack.A1 && GlobalValue.Instance.lstClient[i].A2 == pack.A2 && GlobalValue.Instance.lstClient[i].A3 == pack.A3 && GlobalValue.Instance.lstClient[i].A4 == pack.A4 && GlobalValue.Instance.lstClient[i].A5 == pack.A5)
                                        {
                                            clientindex = i;
                                            GlobalValue.Instance.lstClient[i].ClientSocket = handler;
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
                                        GlobalValue.Instance.lstClient.Add(sockpack);
                                        clientindex = GlobalValue.Instance.lstClient.Count - 1;
                                    }

                                    #region 解析数据
                                    if (havesubsequent)  //多包时保存帧正文数据以便下边解帧时组合
                                    {
                                        if (pack.CurPackCount == 1)  //第一帧时,初始化组帧变量
                                            GlobalValue.Instance.lstClient[clientindex].multiData = new List<byte>();
                                        GlobalValue.Instance.lstClient[clientindex].multiData.AddRange(pack.Data);
                                    }
                                    Universal651SerialPortEntity spEntity = null;  //不使用
                                    string analyseStr = "";
                                    byte[] analyseData = pack.Data;
                                    if (havesubsequent) //如果是多包的情况,且是最后一帧
                                    {
                                        if (pack.CurPackCount > 1 && pack.SumPackCount == pack.CurPackCount)
                                        {
                                            if (GlobalValue.Instance.lstClient[clientindex].multiData != null && GlobalValue.Instance.lstClient[clientindex].multiData.Count > 0)
                                                analyseData = GlobalValue.Instance.lstClient[clientindex].multiData.ToArray();
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

                                        OnSendMsg(new SocketEventArgs(ColorType.UniversalTer,string.Format("中心站地址:{0},遥测站地址:A1-A5[{1},{2},{3},{4},{5}],密码:{6},功能码:{7}({8}),上/下行:{9},",
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
                                        OnSendMsg(new SocketEventArgs(ColorType.UniversalTer,string.Format("中心站地址:{0},遥测站地址:A1-A5[{1},{2},{3},{4},{5}],密码:{6},功能码:{7}({8}),上/下行:{9},",
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
                                        if (GlobalValue.Instance.lstClient != null && GlobalValue.Instance.lstClient.Count > 0)
                                        {
                                            foreach (CallSocketEntity callentity in GlobalValue.Instance.lstClient)
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
                                                OnSendMsg(new SocketEventArgs(ColorType.DataFrame,DateTime.Now.ToString() + "  发送响应帧:" + ConvertHelper.ByteToString(bsenddata, bsenddata.Length)));
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
                                                OnSendMsg(new SocketEventArgs(ColorType.DataFrame,DateTime.Now.ToString() + "  发送命令帧:" + ConvertHelper.ByteToString(bsenddata, bsenddata.Length)));

                                                Thread.Sleep(1500);  //2
                                                if (Send651(handler, bsenddata))  //发送成功则清除
                                                {
                                                    if (GlobalValue.Instance.lstClient != null && GlobalValue.Instance.lstClient.Count > 0)
                                                    {
                                                        foreach (CallSocketEntity callentity in GlobalValue.Instance.lstClient)
                                                        {
                                                            if ((callentity.A5 == pack.A5) && (callentity.A4 == pack.A4) && (callentity.A3 == pack.A3) && (callentity.A2 == pack.A2) && (callentity.A1 == pack.A1))
                                                            {
                                                                lock (GlobalValue.Instance.lstClientLock)
                                                                {
                                                                    if (callentity.lstWaitSendCmd != null && callentity.lstWaitSendCmd.Count > 0)
                                                                    {
                                                                        for (int j =callentity.lstWaitSendCmd.Count-1;j>=0; j--)
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
                                                OnSendMsg(new SocketEventArgs(ColorType.DataFrame,DateTime.Now.ToString() + "  发送响应帧:" + ConvertHelper.ByteToString(bsenddata, bsenddata.Length)));
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
                                    OnSendMsg(new SocketEventArgs(ColorType.Error, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 收到错误帧数据:" + ConvertHelper.ByteToString(arr, arr.Length)));
                                }
                            }
                            #endregion
                        }
                        #endregion

                        index++;
                    }
                }
                else
                {
                    OnSendMsg(new SocketEventArgs(ColorType.Other,DateTime.Now.ToString() + " 客户端" + handler.RemoteEndPoint.ToString() + "下线"));
                    try
                    {
                        handler.Disconnect(false);
                    }
                    catch { }
                }
            }
            catch (ArgumentException argex)
            {
                if (bytesRead > 0)
                {
                    string str_buffer = ConvertHelper.ByteToString(state.buffer, bytesRead);
#if debug
                    OnSendMsg(new SocketEventArgs(ColorType.Error,DateTime.Now.ToString() + " " + argex.Message + ",错误数据:" + str_buffer));
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
                try {
                    foreach (CallSocketEntity callsocket in GlobalValue.Instance.lstClient)
                    {
                        if (callsocket.ClientSocket != null && callsocket.ClientSocket.Equals(handler) && callsocket.TerId != -1)
                        {
                            OnSendMsg(new SocketEventArgs(ColorType.Error,DateTime.Now.ToString()));
                            callsocket.ClientSocket = null;

                            OnLineState_Interval = 1;  //发送下线消息
                            break;
                        }
                    }
                } catch (Exception ex)
                {

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
                packageBytes = null;
                ReceiveBytes = null;
                try
                {
                    if (handler != null && bytesRead > 0 && SocketHelper.IsSocketConnected_Poll(handler))
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
                catch (Exception ex)
                { };
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            Socket handler = null; 
            try
            {
                handler = ((SendObject)ar.AsyncState).workSocket;
                if (handler != null && SocketHelper.IsSocketConnected_Poll(handler))
                {
                    int bytesSent = handler.EndSend(ar);

                    bool AllowOnLine = false;
                    foreach (CallSocketEntity callentity in GlobalValue.Instance.lstClient)
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
            finally
            {
            }
        }

        /// <summary>
        /// 是否需要校时判断
        /// </summary>
        /// <returns></returns>
        public bool NeedCheckTime(DateTime devTime)
        {
            TimeSpan ts = DateTime.Now - (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));   //0点校时
            if (Math.Abs(ts.TotalMinutes) <4)
                return true;

            ts = DateTime.Now - devTime;   //设备时间与服务器时间相差3min校时
            if (Math.Abs(ts.TotalMinutes) > 3)
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
            if (GlobalValue.Instance.lstClient == null)
            {
                GlobalValue.Instance.lstClient = new List<CallSocketEntity>();
            }

            int index = -1;
            for (int i = 0; i < GlobalValue.Instance.lstClient.Count; i++)
            {
                if ((GlobalValue.Instance.lstClient[i].DevType == DevType) && (GlobalValue.Instance.lstClient[i].TerId != -1) && (GlobalValue.Instance.lstClient[i].TerId == DevId))
                {
                    index = i;
                    break;
                }
            }
            if (AllowOnline)
            {
                CallSocketEntity client = null;
                if (index != -1)  //已存在不添加
                    client = GlobalValue.Instance.lstClient[index];
                else
                {
                    client = new CallSocketEntity();
                    client.DevType = DevType;
                    client.TerId = DevId;
                    GlobalValue.Instance.lstClient.Add(client);
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
                    client.lstWaitSendCmd.Add(new SendPackageEntity(PackFromType.P68SendCmd,package));
                }
            }
            else
            {
                if (index > -1)  //已存在移除
                {
                    if (GlobalValue.Instance.lstClient[index].ClientSocket != null)
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

                            GlobalValue.Instance.lstClient[index].AllowOnLine = false;

                            if (GlobalValue.Instance.lstClient[index].ClientSocket != null)  //已连接，马上发送下线命令
                            {
                                try
                                {
                                    SendPackage(GlobalValue.Instance.lstClient[index].ClientSocket, package, "离线命令");
                                    Thread.Sleep(10);
                                    GlobalValue.Instance.lstClient[index].ClientSocket.Shutdown(SocketShutdown.Both);
                                    GlobalValue.Instance.lstClient[index].ClientSocket.Close();
                                    GlobalValue.Instance.lstClient[index].ClientSocket = null;
                                }
                                catch
                                {
                                    if (GlobalValue.Instance.lstClient[index].lstWaitSendCmd == null)
                                        GlobalValue.Instance.lstClient[index].lstWaitSendCmd = new List<SendPackageEntity>();
                                    GlobalValue.Instance.lstClient[index].lstWaitSendCmd.Add(new SendPackageEntity(PackFromType.P68SendCmd,package));
                                }
                            }
                            else
                            {
                                if (GlobalValue.Instance.lstClient[index].lstWaitSendCmd == null)
                                    GlobalValue.Instance.lstClient[index].lstWaitSendCmd = new List<SendPackageEntity>();
                                GlobalValue.Instance.lstClient[index].lstWaitSendCmd.Add(new SendPackageEntity(PackFromType.P68SendCmd,package));
                            }

                        }
                        catch { }
                        finally
                        {
                            GlobalValue.Instance.lstClient[index].ClientSocket = null;
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
            if (GlobalValue.Instance.lstClient != null)
            {
                int index = -1;
                for (int i = 0; i < GlobalValue.Instance.lstClient.Count; i++)
                {
                    if ((GlobalValue.Instance.lstClient[i].DevType == DevType) && (GlobalValue.Instance.lstClient[i].TerId != -1) && (GlobalValue.Instance.lstClient[i].TerId == DevId))
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
                    #endregion

                    foreach (Package package in lstpack)
                    {
                        if (GlobalValue.Instance.lstClient[index].ClientSocket != null)
                        {
                            try
                            {
                                SendPackage(GlobalValue.Instance.lstClient[index].ClientSocket, package, "招测数据命令");
                            }
                            catch
                            {
                                if (GlobalValue.Instance.lstClient[index].lstWaitSendCmd == null)
                                    GlobalValue.Instance.lstClient[index].lstWaitSendCmd = new List<SendPackageEntity>();
                                GlobalValue.Instance.lstClient[index].lstWaitSendCmd.Add(new SendPackageEntity(PackFromType.P68SendCmd,package));
                            }
                        }
                        else
                        {
                            if (GlobalValue.Instance.lstClient[index].lstWaitSendCmd == null)
                                GlobalValue.Instance.lstClient[index].lstWaitSendCmd = new List<SendPackageEntity>();
                            GlobalValue.Instance.lstClient[index].lstWaitSendCmd.Add(new SendPackageEntity(PackFromType.P68SendCmd, package));
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
                OnSendMsg(new SocketEventArgs(ColorType.Error,e.ToString()));
            }
        }

        public void Send651Cmd(Package651 pack)
        {
            if (GlobalValue.Instance.lstClient == null)
                GlobalValue.Instance.lstClient = new List<CallSocketEntity>();

            bool isExist = false;  //是否存在
            foreach (CallSocketEntity sock in GlobalValue.Instance.lstClient)
            {
                if (pack.A1 == sock.A1 && pack.A2 == sock.A2 && pack.A3 == sock.A3 && pack.A4 == sock.A4 && pack.A5 == sock.A5)
                {
                    sock.ActiveTime();
                    isExist = true;
                    if (sock.lstWaitSendCmd != null && sock.lstWaitSendCmd.Count > 0)
                    {
                        for (int i = 0; i < sock.lstWaitSendCmd.Count; i++)
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
                            OnSendMsg(new SocketEventArgs(ColorType.DataFrame, DateTime.Now.ToString() + "  发送命令帧:" + ConvertHelper.ByteToString(bsenddata, bsenddata.Length)));
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
                GlobalValue.Instance.lstClient.Add(newSocket);
            }
        }

        public void GetSL651WaitSendCmd()
        {
            List<Package651> lstWaitSendCmd = new List<Package651>();
            if (GlobalValue.Instance.lstClient != null)
            {
                foreach (CallSocketEntity sockeentity in GlobalValue.Instance.lstClient)
                {
                    if (sockeentity.lstWaitSendCmd != null)
                        foreach (SendPackageEntity packentity in sockeentity.lstWaitSendCmd)
                        {
                            if (packentity.SendPackage651 != null)
                                lstWaitSendCmd.Add(packentity.SendPackage651);
                        }
                }
            }
            SocketEntity msmqEnt = new SocketEntity();
            msmqEnt.MsgType = ConstValue.MSMQTYPE.Get_SL651_WaitSendCmd;
            msmqEnt.Msg = JSONSerialize.JsonSerialize<List<Package651>>(lstWaitSendCmd);
            SocketEventArgs socketargs = new SocketEventArgs(ConstValue.MSMQTYPE.Get_SL651_WaitSendCmd, msmqEnt);
            OnSendMsg(socketargs);
        }

        public void DelSL651WaitSendCmd(byte A1, byte A2, byte A3, byte A4, byte A5, byte funcode)
        {
            if (GlobalValue.Instance.lstClient != null)
            {
                lock (GlobalValue.Instance.lstClientLock)
                {
                    for (int i = 0; i < GlobalValue.Instance.lstClient.Count; i++)
                    {
                        if (GlobalValue.Instance.lstClient[i].lstWaitSendCmd != null)
                            for (int j =GlobalValue.Instance.lstClient[i].lstWaitSendCmd.Count-1;j>=0; j--)
                            {
                                if (GlobalValue.Instance.lstClient[i].lstWaitSendCmd[j].SendPackage651 != null)
                                    if (GlobalValue.Instance.lstClient[i].lstWaitSendCmd[j].SendPackage651.A1 == A1 && GlobalValue.Instance.lstClient[i].lstWaitSendCmd[j].SendPackage651.A2 == A2 && GlobalValue.Instance.lstClient[i].lstWaitSendCmd[j].SendPackage651.A3 == A3 &&
                                        GlobalValue.Instance.lstClient[i].lstWaitSendCmd[j].SendPackage651.A4 == A4 && GlobalValue.Instance.lstClient[i].lstWaitSendCmd[j].SendPackage651.A5 == A5 && GlobalValue.Instance.lstClient[i].lstWaitSendCmd[j].SendPackage651.FUNCODE == funcode)
                                    {
                                        GlobalValue.Instance.lstClient[i].lstWaitSendCmd.RemoveAt(j);
                                    }
                            }
                    }
                }
            }
        }

        private bool Send651(Socket socket, byte[] bsenddata)
        {
            try
            {
                //SendObject sendObj = new SendObject();
                //sendObj.workSocket = socket;
                SocketSend(socket, bsenddata);
                //socket.BeginSend(bsenddata, 0, bsenddata.Length, 0, new AsyncCallback(SendCallback651), sendObj);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region 68协议方法
        public void SendCmd(Package[] packs)
        {
            List<int> lstClientIndex = new List<int>();
            int ClientIndex = -1;
            for (int i = 0; i < packs.Length; i++)
            {
                ClientIndex = GlobalValue.Instance.lstClientAdd(packs[i].DevID, packs[i].DevType, (int)PackFromType.P68SendCmd, packs[i], true);
                if (!lstClientIndex.Contains(ClientIndex))
                    lstClientIndex.Add(ClientIndex);

                GlobalValue.Instance.lstClient[ClientIndex].ActiveTime();
            }
            for(int i = 0; i <lstClientIndex.Count;i++) //每一个终端每次只发送一个命令,不能一次全部发送
            {
                CallSocketEntity sock = GlobalValue.Instance.lstClient[lstClientIndex[i]];
                sock.ActiveTime();
                bool isOnline = false;
                isOnline = SocketHelper.IsSocketConnected_Poll(sock.ClientSocket);
                if (isOnline)
                {
                    try
                    {
                        //packs[i].CS = packs[i].CreateCS();
                        //byte[] bsenddata = packs[i].ToArray();
                        sock.lstWaitSendCmd[0].SendPackage.CS = sock.lstWaitSendCmd[0].SendPackage.CreateCS();
                        byte[] bsenddata = sock.lstWaitSendCmd[0].SendPackage.ToArray();
                        SendObject sendObj = new SendObject();
                        sendObj.workSocket = sock.ClientSocket;
                        SocketSend(sock.ClientSocket, bsenddata);
                        //sock.ClientSocket.BeginSend(bsenddata, 0, bsenddata.Length, 0, new AsyncCallback(SendCallback), sendObj);
                        OnSendMsg(new SocketEventArgs(ColorType.DataFrame, DateTime.Now.ToString() + "  发送命令帧:" + ConvertHelper.ByteToString(bsenddata, bsenddata.Length) + "  " + (new GPRSCmdMSg()).GetPackageDesc(sock.lstWaitSendCmd[0].SendPackage)));
                        //OnSendMsg(new SocketEventArgs(ColorType.DataFrame, DateTime.Now.ToString() + "  " + (new GPRSCmdMSg()).GetPackageDesc(sock.lstWaitSendCmd[0].SendPackage)));
                    }
                    catch
                    {
                        ;
                    }
                }
            }
        }

        public void GetWaitSendCmd()
        {
            List<Package> lstWaitSendCmd = new List<Package>();
            foreach(CallSocketEntity sock in GlobalValue.Instance.lstClient)
            {
                foreach(SendPackageEntity sendpack in sock.lstWaitSendCmd)
                {
                    if (sendpack.SendPackage != null)
                        lstWaitSendCmd.Add(sendpack.SendPackage);
                }
            }
            
            SocketEntity msmqEnt = new SocketEntity();
            msmqEnt.MsgType = ConstValue.MSMQTYPE.Get_P68_WaitSendCmd;
            msmqEnt.Msg = JSONSerialize.JsonSerialize<List<Package>>(lstWaitSendCmd);
            SocketEventArgs socketargs = new SocketEventArgs(ConstValue.MSMQTYPE.Get_P68_WaitSendCmd, msmqEnt);
            OnSendMsg(socketargs);
        }
        public void DelWaitSendCmd(short devid, ConstValue.DEV_TYPE devtype, byte funcode)
        {
            int index = GlobalValue.Instance.GetlstClientIndex(devid, devtype);
            if (index > -1 && GlobalValue.Instance.lstClient[index].lstWaitSendCmd.Count > 0)
            {
                lock (GlobalValue.Instance.lstClientLock)
                {
                    for (int i = GlobalValue.Instance.lstClient[index].lstWaitSendCmd.Count-1;i>=0; i--)
                    {
                        if (GlobalValue.Instance.lstClient[index].lstWaitSendCmd[i].SendPackage.C1 == funcode)
                        {
                            if (GlobalValue.Instance.lstClient[index].lstWaitSendCmd[i].TableId > -1)
                            {
                                //从数据库删除
                                GPRSCmdFlag flag = new GPRSCmdFlag();
                                flag.TableId = GlobalValue.Instance.lstClient[index].lstWaitSendCmd[i].TableId;
                                GlobalValue.Instance.lstClient[index].lstWaitSendCmd[i].SendedFlag = 1;  //标记为已发送，防止后边重发
                                flag.SendCount = 0;  //0:成功发送,收到响应帧
                                GlobalValue.Instance.lstSendedCmdId.Add(flag);
                            }
                            else
                            {

                                GlobalValue.Instance.lstClient[index].lstWaitSendCmd.RemoveAt(i);
                            }
                        }
                    }

                }
            }
            GlobalValue.Instance.SocketSQLMag.Send(SQLType.UpdateSendParmFlag);
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
            OnSendMsg(new SocketEventArgs(ColorType.DataFrame,DateTime.Now.ToString() + "  发送" + prompt + "帧:" + ConvertHelper.ByteToString(bsenddata, bsenddata.Length)));
#else
                                        OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  发送" + prompt + "帧"));
#endif
            SendObject sendObj = new SendObject();
            sendObj.workSocket = handler;
            sendObj.IsFinal = pack.IsFinal;
            sendObj.DevType = pack.DevType;
            sendObj.DevID = pack.DevID;
            SocketSend(handler, bsenddata);
            //handler.BeginSend(bsenddata, 0, bsenddata.Length, 0, new AsyncCallback(SendCallback), sendObj);
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
                else if (Msg.MsgType == ConstValue.MSMQTYPE.MiniDump)
                {
                    //if (IsCreateDumpFile)
                    //{
                    //    OnSendMsg(new SocketEventArgs(ColorType.Other,DateTime.Now.ToString() + " 正在创建Dump文件,请稍候..."));
                    //    return;
                    //}
                    //try
                    //{
                        //IsCreateDumpFile = true;
                        Dumpcallback?.Invoke();
                    //}
                    //catch (Exception ex)
                    //{
                    //    OnSendMsg(new SocketEventArgs(ColorType.Other,DateTime.Now.ToString() + " 生成dmp文件失败,ex:" + ex.Message));
                    //}
                    //finally
                    //{
                    //    IsCreateDumpFile = false;
                    //}
                }
                else if (Msg.MsgType == ConstValue.MSMQTYPE.P68_Cmd)
                {
                    if (Msg.Packs != null)
                        GlobalValue.Instance.SocketMag.SendCmd(Msg.Packs);
                    GlobalValue.Instance.SocketMag.GetWaitSendCmd();  //发送完毕获取待发送命令列表
                }
                else if (Msg.MsgType == ConstValue.MSMQTYPE.Get_P68_WaitSendCmd)
                {
                    GlobalValue.Instance.SocketMag.GetWaitSendCmd();
                }
                else if (Msg.MsgType == ConstValue.MSMQTYPE.Del_P68_WaitSendCmd)
                {
                    GlobalValue.Instance.SocketMag.DelWaitSendCmd(Msg.DevId, Msg.DevType, Msg.P68Funcode);
                    GlobalValue.Instance.SocketMag.GetWaitSendCmd();
                }
                else if(Msg.MsgType == ConstValue.MSMQTYPE.GC)
                {
                    GC.Collect();
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
        public bool SocketSend(Socket sock, string entitymsg)
        {
            //try
            //{
            //    if (string.IsNullOrEmpty(entitymsg))
            //        return true;
            //    if (!SocketHelper.IsSocketConnected_Poll(sock))
            //    {
            //        sock = null;    //释放
            //        return false;
            //    }
            //    StringBuilder buildmsg = new StringBuilder(entitymsg.Length+2* SocketHelper.SocketMsgSplit.Length);
            //    buildmsg.Append(SocketHelper.SocketMsgSplit);
            //    buildmsg.Append(entitymsg);
            //    buildmsg.Append(SocketHelper.SocketMsgSplit);
            //    byte[] bs = Encoding.UTF8.GetBytes(buildmsg.ToString());

            //    SendObject sendObj = new SendObject();
            //    sendObj.workSocket = sock;
            //    sock.BeginSend(bs, 0, bs.Length, 0, new AsyncCallback(SmartSendCallback), sendObj);
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
            StringBuilder buildmsg = new StringBuilder(entitymsg.Length + 2 * SocketHelper.SocketMsgSplit.Length);
            buildmsg.Append(SocketHelper.SocketMsgSplit);
            buildmsg.Append(entitymsg);
            buildmsg.Append(SocketHelper.SocketMsgSplit);
            byte[] bs = Encoding.UTF8.GetBytes(buildmsg.ToString());
            return SocketSendAsync(sock, bs);
        }

        public bool SocketSend(Socket sock, byte[] bs)
        {
            return SocketSendAsync(sock, bs);
            //try
            //{
            //    if (!SocketHelper.IsSocketConnected_Poll(sock))
            //    {
            //        sock = null;    //释放
            //        return false;
            //    }

            //    SendObject sendObj = new SendObject();
            //    sendObj.workSocket = sock;
            //    sock.BeginSend(bs, 0, bs.Length, 0, new AsyncCallback(SmartSendCallback), sendObj);
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }

        public bool SocketSendAsync(Socket sock, byte[] bs)
        {
            SocketAsyncEventArgs e = null;
            lock (s_lst)
            {
                if (s_lst.Count > 0)
                    e = s_lst.Pop();
            }
            if (e == null)
            {
                e = new SocketAsyncEventArgs();
                e.Completed += (object sender, SocketAsyncEventArgs _e) =>
                 {
                     lock (s_lst)
                         s_lst.Push(e);
                 };
            }
            try
            {
                e.SetBuffer(bs, 0, bs.Length);
            }
            catch (Exception ex)
            {
                lock (s_lst)
                    s_lst.Push(e);
                return false;
            }
            try
            {
                if (sock.SendAsync(e))
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            lock (s_lst)
                s_lst.Push(e);

            return true;
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

        void sqlmanager_SQLEvent(object sender, SQLNotifyEventArgs e)
        {
            if (e.SQLType == SQLType.InsertPreValue)
            {
                if (1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs(ColorType.DataBase,"压力数据保存成功"));
                }
                else if (-1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs(ColorType.Error, "压力数据保存失败:" + e.Msg));
                }
            }
            else if (e.SQLType == SQLType.InsertFlowValue)
            {
                if (1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs(ColorType.DataBase, "流量数据保存成功"));
                }
                else if (-1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs(ColorType.Error, "流量数据保存失败:" + e.Msg));
                }
            }
            else if (e.SQLType == SQLType.InsertPrectrlValue)
            {
                if (1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs(ColorType.DataBase, "压力控制器数据保存成功"));
                }
                else if (-1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs(ColorType.Error, "压力控制器数据保存失败:" + e.Msg));
                }
            }
            else if (e.SQLType == SQLType.InsertUniversalValue)
            {
                if (1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs(ColorType.DataBase, "通用终端数据保存成功"));
                }
                else if (-1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs(ColorType.Error, "通用终端数据保存失败:" + e.Msg));
                }
            }
            else if (e.SQLType == SQLType.InsertOLWQValue)
            {
                if (1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs(ColorType.DataBase, "水质终端数据保存成功"));
                }
                else if (-1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs(ColorType.Error, "水质终端数据保存失败:" + e.Msg));
                }
            }
            else if (e.SQLType == SQLType.InsertNoiseValue)
            {
                if (1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs(ColorType.DataBase, "噪声数据保存成功"));
                }
                else if (-1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs(ColorType.Error, "噪声数据保存失败:" + e.Msg));
                }
            }
            else if (e.SQLType == SQLType.InsertWaterworkerValue)
            {
                if (1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs(ColorType.DataBase, "水厂数据保存成功"));
                }
                else if (-1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs(ColorType.Error, "水厂数据保存失败:" + e.Msg));
                }
            }
            else if (e.SQLType == SQLType.InsertHydrantValue)
            {
                if (1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs(ColorType.DataBase, "消防栓数据保存成功"));
                }
                else if (-1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs(ColorType.Error, "消防栓数据保存失败:" + e.Msg));
                }
            }
            else if (e.SQLType == SQLType.InsertAlarm)
            {
                if (1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs(ColorType.DataBase, "报警数据保存成功"));
                }
                else if (-1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs(ColorType.Error, "报警数据保存失败:" + e.Msg));
                }
            }
        }
    }

}
