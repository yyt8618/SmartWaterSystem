using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using Common;
using Entity;
using System.Data;

namespace GCGPRSService
{
    public class SocketEventArgs : EventArgs
    {
        private MSMQEntity _jsonmsg;// = "";
        public MSMQEntity JsonMsg
        {
            get { return _jsonmsg; }
            set { _jsonmsg = value; }
        }

        public SocketEventArgs(Entity.ConstValue.MSMQTYPE MsgType, string msg)
        {
            MSMQEntity msmqEntity = new MSMQEntity();
            msmqEntity.MsgType = MsgType;
            msmqEntity.Msg = msg;
            _jsonmsg = msmqEntity; // JsonConvert.SerializeObject(msmqEntity);
        }

        public SocketEventArgs(Entity.ConstValue.MSMQTYPE MsgType, MSMQEntity msg)
        {
            msg.MsgType = MsgType;
            _jsonmsg = msg;
        }

        public SocketEventArgs(string msg)
        {
            MSMQEntity msmqEntity = new MSMQEntity();
            msmqEntity.MsgType = Entity.ConstValue.MSMQTYPE.Message;
            msmqEntity.Msg = msg;
            _jsonmsg = msmqEntity;// JsonConvert.SerializeObject(msmqEntity);
        }

        public SocketEventArgs(MSMQEntity msg)
        {
            _jsonmsg = msg;
        }
    }

    public class StateObject
    {
        // Client socket.     
        public Socket workSocket = null;
        // Size of receive buffer.     
        public const int BufferSize = 1152;
        // Receive buffer.     
        public byte[] buffer = new byte[BufferSize];
        // Received data string.     
        public List<Package> lstBuffer = new List<Package>();

        public void AppendBuffer(Package pack)
        {
            lstBuffer.Add(pack);
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

    public delegate void cmdEventHandle(object sender, SocketEventArgs e);

    public class SocketManager
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("SocketService");
        public ManualResetEvent allDone = new ManualResetEvent(false);
        public event cmdEventHandle cmdEvent;
        Thread t_socket;
        Thread t_Interval;
        Socket listener;
        bool isRunning = false;
        List<CallSocketEntity> lstClient = new List<CallSocketEntity>();  //在线客户端列表
        int SQL_Interval = 3 * 63;  //数据更新时间间隔(second)
        DateTime SQLSync_Time = DateTime.Now.AddHours(-1);  //数据库同步时间,-1:才开启，马上同步一次数据

        int OnLineState_Interval = 5 * 60; //终端在线状态更新时间间隔(second)
        int CheckThread_Interval = 2 * 60; //检查线程状态间隔(second)

        public virtual void OnSendMsg(SocketEventArgs e)
        {
            if (cmdEvent != null)
                cmdEvent(this, e);
        }

        public void T_Listening()
        {
            OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + " 开启数据库线程!"));
            GlobalValue.Instance.SocketSQLMag.SQLEvent += new SQLHandle(sqlmanager_SQLEvent);
            GlobalValue.Instance.SocketSQLMag.Start();

            t_Interval = new Thread(new ThreadStart(Interval_Thread));
            t_Interval.Start();

            CheckThread_Interval = 1;  //开启socket线程

            MSMQEntity msmqEntity = new MSMQEntity();
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
                    MSMQEntity msmqEntity = new MSMQEntity();
                    msmqEntity.lstOnLine = new List<OnLineTerEntity>();
                    //List<CallSocketEntity> lstOnLine = new List<CallSocketEntity>();
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
                        if (client.ClientSocket != null && client.ClientSocket.Connected)
                        {
                            try
                            {
                                if (client.ClientSocket.Poll(1, SelectMode.SelectRead))
                                {
                                    //byte[] temp = new byte[1024];
                                    //int nRead = client.ClientSocket.Receive(temp);
                                    //if (nRead == 0)
                                    //{

                                    //}
                                    //else
                                    //    add = true;
                                }
                                else
                                    add = true;
                            }
                            catch
                            {
                                add = false;
                            }
                        }

                        if (add)
                        {
                            msmqEntity.lstOnLine.Add(new OnLineTerEntity(client.DevType, client.TerId));
                            //lstOnLine.Add(client);
                        }
                    }
                    //lstClient = lstOnLine;  //将意外断开连接的的去除
                    OnSendMsg(new SocketEventArgs(ConstValue.MSMQTYPE.Data_OnLineState, msmqEntity));
                }
                if (CheckThread_Interval-- == 0)
                {
                    CheckThread_Interval = 2 * 60;
                    if (t_socket == null || (t_socket != null && !t_socket.IsAlive))
                    {
                        OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + " 开启Socket监听线程!"));
                        isRunning = true;
                        t_socket = new Thread(new ThreadStart(StartListening));
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
        }

        public void Close()
        {
            try
            {
                isRunning = false;
                if (listener != null )
                {
                    listener.Shutdown(SocketShutdown.Both);
                    System.Threading.Thread.Sleep(10);
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
                OnSendMsg(new SocketEventArgs("GPRS远传服务停止,请设置IP与端口号!"));
                t_socket.Abort();
                return;
            }
            if(string.IsNullOrEmpty(Settings.Instance.GetString(SettingKeys.GPRS_PORT)))
            {
                OnSendMsg(new SocketEventArgs("GPRS远传服务停止,请设置IP与端口号!"));
                t_socket.Abort();
                return;
            }
            //Thread.Sleep(10 * 1000);
            int port =Settings.Instance.GetInt(SettingKeys.GPRS_PORT);
            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
            // Create a TCP/IP socket.     
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.ReceiveTimeout = 60 * 1000;  //设置超时
            listener.SendTimeout = 60 * 1000;
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
                OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + " GPRS远传服务发生异常，将停止! Exp:"+sockEx.Message));
                logger.ErrorException("StartListening", sockEx);
                if (t_socket != null && t_socket.IsAlive)
                    t_socket.Abort();
            }
            catch (Exception e)
            {
                OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + " GPRS远传服务发生异常，将停止!"));
                logger.ErrorException("StartListening", e);
                if (t_socket != null && t_socket.IsAlive)
                    t_socket.Abort();
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            Socket handler = null;
            try
            {   
                allDone.Set(); 
                Socket listener = (Socket)ar.AsyncState;
                handler = listener.EndAccept(ar);
                StateObject state = new StateObject();
                //OnSendMsg(new SocketEventArgs("AcceptCallback线程ID为:  " + Thread.CurrentThread.ManagedThreadId.ToString()));
                state.workSocket = handler;
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                //listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
            }
            catch (Exception ex)
            {
                OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + " 接收错误:" + ex.Message));
                logger.ErrorException("AcceptCallback",ex);
                try
                {
                    if (handler != null)
                    {
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                    }
                }
                catch (Exception ex1)
                {
                    OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + " 接收错误(关闭异常):" + ex1.Message));
                    logger.ErrorException("AcceptCallback",ex1);
                }
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
                if (!handler.Connected)
                    return;
                bytesRead = handler.EndReceive(ar);
                if (bytesRead > 0)
                {
                    Queue<byte> ByteQueue = new Queue<byte>();
                    List<byte> packageBytes = new List<byte>();

                    for (int i = 0; i < bytesRead; i++)
                    {
                        ByteQueue.Enqueue(state.buffer[i]);   //入队列
                    }
                    while (ByteQueue.Count > 0)
                    {
                        byte byteItem = ByteQueue.Dequeue();    //出队列
                        packageBytes.Add(byteItem);
                        if (byteItem == PackageDefine.EndByte && packageBytes.Count >= PackageDefine.MinLenth)
                        {
                            byte[] arr = packageBytes.ToArray();
                            int len = BitConverter.ToInt16(new byte[] { arr[9], arr[8] }, 0);  //数据域长度
                            Package pack;
                            if ((PackageDefine.MinLenth + len == arr.Length) && Package.TryParse(arr, out pack))//找到结束字符并且是完整一帧
                            {
                                if (pack.CommandType == CTRL_COMMAND_TYPE.RESPONSE_BY_SLAVE)  //接受到应答,判断是否D11是否为1,如果为0,表示没有数据需要读
                                {
                                    foreach (CallSocketEntity callentity in lstClient)
                                    {
                                        if (callentity.DevType == pack.DevType && callentity.TerId == pack.DevID && callentity.lstWaitSendCmd!=null)
                                        {
                                            List<SendPackageEntity> lst_save_pack = new List<SendPackageEntity>();
                                            for (int i = 0; i < callentity.lstWaitSendCmd.Count; i++)  //收到响应帧
                                            {
                                                /* CommandPack.DevID = pack.DevID;
                                                    CommandPack.DevType = pack.DevType;
                                                    CommandPack.C0 = Convert.ToByte(GlobalValue.Instance.lstGprsCmd[i].CtrlCode);
                                                    CommandPack.C1 = Convert.ToByte(GlobalValue.Instance.lstGprsCmd[i].FunCode);
                                                    CommandPack.Data = ConvertHelper.StringToByte(GlobalValue.Instance.lstGprsCmd[i].Data);
                                                    CommandPack.DataLength = CommandPack.Data.Length;*/
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
                                }
                                else if (pack.CommandType == CTRL_COMMAND_TYPE.REQUEST_BY_SLAVE)//接收到的数据帧
                                {
                                    string str_frame = ConvertHelper.ByteToString(arr, arr.Length);
#if debug
                                    OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + " 收到帧数据:" + str_frame));
#else
                                    OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + " 收到帧数据"));
#endif
                                    bool bNeedCheckTime = false;  //是否需要校时
                                    #region 解析数据
                                    if (pack.ID3 == (byte)Entity.ConstValue.DEV_TYPE.Data_CTRL)
                                    {
                                        #region 压力终端
                                        if (pack.C1 == (byte)GPRS_READ.READ_PREDATA)  //从站向主站发送压力采集数据
                                        {
                                            int dataindex = (pack.DataLength - 2 - 1) % 8;
                                            if (dataindex != 0)
                                            {
                                                throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(2+1+8*n)规则");
                                            }
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
                                            framedata.TerId = pack.DevID.ToString();
                                            framedata.ModifyTime = DateTime.Now;
                                            framedata.Frame = str_frame;

                                            int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                                            float pressuevalue = 0;
                                            for (int i = 0; i < dataindex; i++)
                                            {
                                                year = 2000 + Convert.ToInt16(pack.Data[i * 8 + 3]);
                                                month = Convert.ToInt16(pack.Data[i * 8 + 4]);
                                                day = Convert.ToInt16(pack.Data[i * 8 + 5]);
                                                hour = Convert.ToInt16(pack.Data[i * 8 + 6]);
                                                minute = Convert.ToInt16(pack.Data[i * 8 + 7]);
                                                sec = Convert.ToInt16(pack.Data[i * 8 + 8]);

                                                pressuevalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[i * 8 + 10], pack.Data[i * 8 + 9] }, 0)) / 1000;

                                                OnSendMsg(new SocketEventArgs(string.Format("index({0})|压力终端[{1}]|报警({2})|压力标志({3})|采集时间({4})|压力值:{5}MPa",
                                                    dataindex, pack.DevID, alarm, preFlag, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, pressuevalue)));

                                                GPRSPreDataEntity data = new GPRSPreDataEntity();
                                                data.PreValue = pressuevalue;
                                                data.ColTime = new DateTime(year, month, day, hour, minute, sec);
                                                bNeedCheckTime = NeedCheckTime(data.ColTime);
                                                framedata.lstPreData.Add(data);
                                            }

                                            GlobalValue.Instance.GPRS_PreFrameData.Enqueue(framedata);  //通知存储线程处理
                                            GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertPreValue);
                                        }
                                        else if (pack.C1 == (byte)GPRS_READ.READ_FLOWDATA)  //从站向主站发送流量采集数据
                                        {
                                            int dataindex = (pack.DataLength - 2 - 1) % 18;
                                            if (dataindex != 0)
                                            {
                                                throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(2+1+18*n)规则");
                                            }
                                            dataindex = (pack.DataLength - 2 - 1) / 18;

                                            int alarmflag = 0;
                                            int flowFlag = 0;


                                            //报警标志
                                            alarmflag = BitConverter.ToInt16(new byte[] { pack.Data[0], pack.Data[1] }, 0);
                                            flowFlag = Convert.ToInt16(pack.Data[2]);

                                            GPRSFlowFrameDataEntity framedata = new GPRSFlowFrameDataEntity();
                                            framedata.TerId = pack.DevID.ToString();
                                            framedata.ModifyTime = DateTime.Now;
                                            framedata.Frame = str_frame;

                                            int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                                            float forward_flowvalue = 0, reverse_flowvalue, instant_flowvalue = 0;
                                            for (int i = 0; i < dataindex; i++)
                                            {
                                                year = 2000 + Convert.ToInt16(pack.Data[i * 18 + 3]);
                                                month = Convert.ToInt16(pack.Data[i * 18 + 4]);
                                                day = Convert.ToInt16(pack.Data[i * 18 + 5]);
                                                hour = Convert.ToInt16(pack.Data[i * 18 + 6]);
                                                minute = Convert.ToInt16(pack.Data[i * 18 + 7]);
                                                sec = Convert.ToInt16(pack.Data[i * 18 + 8]);

                                                //前向流量
                                                forward_flowvalue = BitConverter.ToSingle(new byte[] { pack.Data[i * 18 + 12], pack.Data[i * 18 + 11], pack.Data[i * 18 + 10], pack.Data[i * 18 + 9] }, 0);
                                                //反向流量
                                                reverse_flowvalue = BitConverter.ToSingle(new byte[] { pack.Data[i * 18 + 16], pack.Data[i * 18 + 15], pack.Data[i * 18 + 14], pack.Data[i * 18 + 13] }, 0);
                                                //瞬时流量
                                                instant_flowvalue = BitConverter.ToSingle(new byte[] { pack.Data[i * 18 + 20], pack.Data[i * 18 + 19], pack.Data[i * 18 + 18], pack.Data[i * 18 + 17] }, 0);

                                                OnSendMsg(new SocketEventArgs(string.Format("index({0})|流量终端[{1}]|报警标志({2})|流量标志({3})|采集时间({4})|正向流量值:{5}|反向流量值:{6}|瞬时流量值:{7}",
                                                    dataindex, pack.DevID, alarmflag, flowFlag, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, forward_flowvalue, reverse_flowvalue, instant_flowvalue)));

                                                GPRSFlowDataEntity data = new GPRSFlowDataEntity();
                                                data.Forward_FlowValue = forward_flowvalue;
                                                data.Reverse_FlowValue = reverse_flowvalue;
                                                data.Instant_FlowValue = instant_flowvalue;
                                                data.ColTime = new DateTime(year, month, day, hour, minute, sec);
                                                bNeedCheckTime = NeedCheckTime(data.ColTime);
                                                framedata.lstFlowData.Add(data);
                                            }
                                            GlobalValue.Instance.GPRS_FlowFrameData.Enqueue(framedata);  //通知存储线程处理
                                            GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertFlowValue);
                                        }
                                        else if (pack.C1 == (byte)GPRS_READ.READ_ALARMINFO)  //从站向主站发送设备报警信息
                                        {
                                            if (pack.DataLength != 7)
                                            {
                                                throw new ArgumentException(DateTime.Now.ToString() + " " + "帧数据长度[" + pack.DataLength + "]不符合(2+1+18*n)规则");
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

                                            bNeedCheckTime = NeedCheckTime(new DateTime(year, month, day, hour, minute, sec));
                                            OnSendMsg(new SocketEventArgs(string.Format("压力终端[{0}]{1}|时间({2})",
                                                 pack.DevID, alarm, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec)));
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
                                            int calibration = BitConverter.ToInt16(new byte[] { pack.Data[1], pack.Data[0] }, 0);
                                            GPRSUniversalFrameDataEntity framedata = new GPRSUniversalFrameDataEntity();
                                            framedata.TerId = pack.DevID.ToString();
                                            framedata.ModifyTime = DateTime.Now;
                                            framedata.Frame = str_frame;

                                            int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                                            float datavalue = 0;

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
                                                        datavalue = Convert.ToSingle(datavalue.ToString("F" + precision));  //精度调整
                                                        if (datavalue < 0)
                                                            datavalue = 0;
                                                        OnSendMsg(new SocketEventArgs(string.Format("index({0})|通用终端[{1}]模拟{2}路|校准值({3})|采集时间({4})|{5}:{6}{7}",
                                                            i, pack.DevID, name, calibration, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, dr_TerminalDataConfig[0]["Name"].ToString().Trim(), datavalue, dr_TerminalDataConfig[0]["Unit"].ToString())));

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
                                            framedata.TerId = pack.DevID.ToString();
                                            framedata.ModifyTime = DateTime.Now;
                                            framedata.Frame = str_frame;

                                            int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                                            float datavalue = 0;

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
                                                            datavalue = Convert.ToSingle(datavalue.ToString("F" + Precisions[j]));  //精度调整
                                                            OnSendMsg(new SocketEventArgs(string.Format("index({0})|通用终端[{1}]脉冲{2}路|采集时间({3})|{4}:{5}{6}",
                                                                i, pack.DevID, j + 1, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, Names[j], datavalue, Units[j])));

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
                                            framedata.TerId = pack.DevID.ToString();
                                            framedata.ModifyTime = DateTime.Now;
                                            framedata.Frame = str_frame;

                                            int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                                            float datavalue = 0;

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
                                                            datavalue = Convert.ToSingle(datavalue.ToString("F" + Precisions[j]));  //精度调整
                                                            OnSendMsg(new SocketEventArgs(string.Format("index({0})|通用终端[{1}]RS485 {2}路|采集时间({3})|{4}:{5}{6}",
                                                                i, pack.DevID, name, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, Names[j], datavalue, Units[j])));

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
                                        if ((pack.C1 == (byte)GPRS_READ.READ_TURBIDITY) || (pack.C1 == (byte)GPRS_READ.READ_RESIDUALCL) ||
                                            (pack.C1 == (byte)GPRS_READ.READ_PH))  //从站向主站发送水质采集数据
                                        {
                                            int dataindex = (pack.DataLength - 2 - 1) % 8;
                                            if (dataindex != 0)
                                            {
                                                throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(2+1+8*n)规则");
                                            }
                                            dataindex = (pack.DataLength - 2 - 1) / 8;

                                            GPRSOLWQFrameDataEntity framedata = new GPRSOLWQFrameDataEntity();
                                            framedata.TerId = pack.DevID.ToString();
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
                                                    data.ResidualCl = value/1000;
                                                }
                                                else if (pack.C1 == (byte)GPRS_READ.READ_PH)
                                                {
                                                    data.PH = value/100;
                                                }

                                                OnSendMsg(new SocketEventArgs(string.Format("index({0})|水质终端[{1}]|采集时间({2})|{3}值:{4}{5}",
                                                    dataindex, pack.DevID, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, name, value,unit)));

                                                data.ColTime = new DateTime(year, month, day, hour, minute, sec);
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
                                                throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(2+1+10*n)规则");
                                            }
                                            dataindex = (pack.DataLength - 2 - 1) / 12;

                                            GPRSOLWQFrameDataEntity framedata = new GPRSOLWQFrameDataEntity();
                                            framedata.TerId = pack.DevID.ToString();
                                            framedata.ModifyTime = DateTime.Now;
                                            framedata.Frame = str_frame;

                                            int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                                            float Condvalue = 0;
                                            float Tempvalue = 0;

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

                                                OnSendMsg(new SocketEventArgs(string.Format("index({0})|水质终端[{1}]|采集时间({2})|电导率值:{3}us/cm,温度:{4}℃",
                                                    dataindex, pack.DevID, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, Condvalue.ToString("f2"), Tempvalue.ToString("f1"))));

                                                GPRSOLWQDataEntity data = new GPRSOLWQDataEntity();
                                                data.Conductivity = Condvalue;
                                                data.Temperature = Tempvalue;
                                                data.ValueColumnName = "Conductivity";
                                                data.ColTime = new DateTime(year, month, day, hour, minute, sec);
                                                bNeedCheckTime = NeedCheckTime(data.ColTime);
                                                framedata.lstOLWQData.Add(data);
                                            }

                                            GlobalValue.Instance.GPRS_OLWQFrameData.Enqueue(framedata);  //通知存储线程处理
                                            GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertOLWQValue);
                                        }
                                        #endregion
                                    }
                                    else if (pack.ID3 == (byte)Entity.ConstValue.DEV_TYPE.HYDRANT_CTRL)
                                    {
                                        #region 消防栓
                                        GPRSHydrantFrameDataEntity framedata = new GPRSHydrantFrameDataEntity();
                                        framedata.TerId = pack.DevID.ToString();
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
                                        data.ColTime = new DateTime(year, month, day, hour, minute, sec);
                                        bNeedCheckTime = NeedCheckTime(data.ColTime);

                                        if (pack.C1 == (byte)GPRS_READ.READ_HYDRANT_OPEN)
                                        {
                                            int openangle = Convert.ToInt16(pack.Data[6]);
                                            float prevalue = (float)BitConverter.ToInt16(new byte[] { pack.Data[8], pack.Data[7] }, 0)/3;
                                            OnSendMsg(new SocketEventArgs(string.Format("消防栓[{0}]被打开|时间({1})|开度:{2},压力:{3}",
                                                    pack.DevID, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, openangle, prevalue.ToString("f3"))));
                                            data.Operate = HydrantOptType.Open;
                                            data.PreValue = prevalue;
                                            data.OpenAngle = openangle;
                                        }
                                        else if (pack.C1 == (byte)GPRS_READ.READ_HYDRANT_CLOSE)
                                        {
                                            OnSendMsg(new SocketEventArgs(string.Format("消防栓[{0}]被关闭|时间({1})",
                                                       pack.DevID, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec)));
                                            data.Operate = HydrantOptType.Close;
                                        }
                                        else if (pack.C1 == (byte)GPRS_READ.READ_HYDRANT_OPENANGLE)
                                        {
                                            int openangle = Convert.ToInt16(pack.Data[6]);
                                            OnSendMsg(new SocketEventArgs(string.Format("消防栓[{0}]开度|时间({1})|开度:{2}",
                                                    pack.DevID, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, openangle)));
                                            data.OpenAngle = openangle;
                                            data.Operate = HydrantOptType.OpenAngle;
                                        }
                                        else if (pack.C1 == (byte)GPRS_READ.READ_HYDRANT_IMPACT)
                                        {
                                            OnSendMsg(new SocketEventArgs(string.Format("消防栓[{0}]被撞击|时间({1})",
                                                       pack.DevID, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec)));
                                            data.Operate = HydrantOptType.Impact;
                                        }
                                        else if (pack.C1 == (byte)GPRS_READ.READ_HYDRANT_KNOCKOVER)
                                        {
                                            OnSendMsg(new SocketEventArgs(string.Format("消防栓[{0}]被撞倒|时间({1})",
                                                       pack.DevID, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec)));
                                            data.Operate = HydrantOptType.KnockOver;
                                        }

                                        framedata.lstHydrantData.Add(data);
                                        GlobalValue.Instance.GPRS_HydrantFrameData.Enqueue(framedata);  //通知存储线程处理
                                        GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertHydrantValue);
                                        #endregion
                                    }

                                    #endregion

                                    Package response = new Package();
                                    response.DevID = pack.DevID;
                                    List<Package> lstCommandPack = new List<Package>();
                                    CallSocketEntity currentSocketEntity = null;
                                    foreach (CallSocketEntity callentity in lstClient)
                                    {
                                        if (callentity.DevType == pack.DevType && callentity.TerId == pack.DevID)
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
                                            MSMQEntity msmqEntity = new MSMQEntity();
                                            msmqEntity.lstOnLine = new List<OnLineTerEntity>();
                                            msmqEntity.lstOnLine.Add(new OnLineTerEntity(callentity.DevType, callentity.TerId));
                                            OnSendMsg(new SocketEventArgs(ConstValue.MSMQTYPE.Data_OnLineState, msmqEntity));
                                        }
                                    }

                                    #region 发送后续命令帧
                                    if (((GlobalValue.Instance.lstGprsCmd != null && GlobalValue.Instance.lstGprsCmd.Count > 0)
                                        || bNeedCheckTime || lstCommandPack.Count > 0) && pack.IsFinal)
                                    {
                                        #region 回复响应帧
                                        response.DevType = pack.DevType;
                                        response.C0 = 0x48;  //主站发出的应答帧，有后续命令
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
                                        sendObj.IsFinal = false;
                                        sendObj.DevType = pack.DevType;
                                        sendObj.DevID = pack.DevID;

                                        handler.BeginSend(bsenddata, 0, bsenddata.Length, 0, new AsyncCallback(SendCallback), sendObj);
                                        #endregion

                                        if (GlobalValue.Instance.lstGprsCmd != null && GlobalValue.Instance.lstGprsCmd.Count > 0)
                                        {
                                            for (int i = 0; i < GlobalValue.Instance.lstGprsCmd.Count; i++)
                                            {
                                                if (GlobalValue.Instance.lstGprsCmd[i].DeviceId == (int)pack.DevID && GlobalValue.Instance.lstGprsCmd[i].DevTypeId == (int)pack.DevType)
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
                                                }
                                            }
                                        }

                                        if (bNeedCheckTime)
                                        {
                                            Package pack_time = new Package();
                                            pack_time.DevType = pack.DevType;
                                            pack_time.DevID = pack.DevID;
                                            pack_time.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
                                            if (pack.DevType == Entity.ConstValue.DEV_TYPE.PRESS_CTRL)
                                                pack_time.C1 = (byte)NOISE_LOG_COMMAND.WRITE_TIME;
                                            else if (pack.DevType == Entity.ConstValue.DEV_TYPE.Data_CTRL)
                                                pack_time.C1 = (byte)PreTer_WRITE.WRITE_TIME;
                                            else if (pack.DevType == Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL)
                                                pack_time.C1 = (byte)UNIVERSAL_COMMAND.SET_TIME;
                                            else if (pack.DevType == Entity.ConstValue.DEV_TYPE.OLWQ_CTRL)
                                                pack_time.C1 = (byte)OLWQ_COMMAND.SET_TIME;
                                            else if (pack.DevType == Entity.ConstValue.DEV_TYPE.HYDRANT_CTRL)
                                                pack_time.C1 = (byte)HYDRANT_COMMAND.SET_TIME;
                                            byte[] data = new byte[6];
                                            data[0] = (byte)(DateTime.Now.Year - 2000);
                                            data[1] = (byte)DateTime.Now.Month;
                                            data[2] = (byte)DateTime.Now.Day;
                                            data[3] = (byte)DateTime.Now.Hour;
                                            data[4] = (byte)DateTime.Now.Minute;
                                            data[5] = (byte)DateTime.Now.Second;
                                            pack_time.DataLength = data.Length;
                                            pack_time.Data = data;
                                            //pack_time.CS = pack_time.CreateCS();

                                            lstCommandPack.Add(pack_time);
                                        }

                                        for (int i = 0; i < lstCommandPack.Count; i++)
                                        {
                                            Package commandpack = lstCommandPack[i];
                                            if (i != (lstCommandPack.Count - 1))
                                                commandpack.C0 = ((byte)(commandpack.C0 | 0x08));  //不是最后一个帧
                                            commandpack.CS = commandpack.CreateCS();
                                            bsenddata = commandpack.ToArray();
#if debug
                                            OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  发送命令帧:" + ConvertHelper.ByteToString(bsenddata, bsenddata.Length)));
#else
                                        OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  发送响应帧"));
#endif
                                            sendObj = new SendObject();
                                            sendObj.workSocket = handler;
                                            sendObj.IsFinal = (i == (lstCommandPack.Count - 1)) ? true : false;
                                            sendObj.DevType = pack.DevType;
                                            sendObj.DevID = pack.DevID;
                                            handler.BeginSend(bsenddata, 0, bsenddata.Length, 0, new AsyncCallback(SendCallback), sendObj);

                                            if (GlobalValue.Instance.lstGprsCmd != null && GlobalValue.Instance.lstGprsCmd.Count > 0)
                                            {
                                                for (int j = 0; j < GlobalValue.Instance.lstGprsCmd.Count; j++)
                                                {
                                                    if (GlobalValue.Instance.lstGprsCmd[j].DevTypeId == (int)commandpack.DevType && GlobalValue.Instance.lstGprsCmd[j].FunCode == commandpack.C1
                                                        && GlobalValue.Instance.lstGprsCmd[j].DeviceId == commandpack.DevID)
                                                    {
                                                        GPRSCmdFlag flag = new GPRSCmdFlag();
                                                        flag.Index = i;
                                                        flag.TableId = GlobalValue.Instance.lstGprsCmd[j].TableId;
                                                        GlobalValue.Instance.lstSendedCmdId.Add(flag);
                                                    }
                                                }
                                            }
                                            GlobalValue.Instance.SocketSQLMag.Send(SQLType.UpdateSendParmFlag);
                                        }
                                    }
                                    else
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
                                        handler.BeginSend(bsenddata, 0, bsenddata.Length, 0, new AsyncCallback(SendCallback), sendObj);
                                    }
                                    #endregion

                                    //if (currentSocketEntity != null)   //如果有需要发送的帧数据，在最后认为已经发送完，清除
                                    //{
                                    //    currentSocketEntity.lstWaitSendCmd = new List<Package>();
                                    //}
                                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                                }
                            }
                        }
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
                    handler.Close();
                }
                catch { }
            }
            catch (SocketException sockex)
            {
                foreach(CallSocketEntity callsocket in lstClient)
                {
                    if (callsocket.ClientSocket.Equals(handler))
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
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
                catch { }
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = ((SendObject)ar.AsyncState).workSocket;
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
                    Thread.Sleep(10 * 1000);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception e)
            {
                logger.ErrorException(DateTime.Now.ToString() + " SendCallback", e);
                OnSendMsg(new SocketEventArgs(e.ToString()));
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
                if ((lstClient[i].DevType == DevType) && (lstClient[i].TerId == DevId))
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
                    if ((lstClient[i].DevType == DevType) && (lstClient[i].TerId == DevId))
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

    }

}
