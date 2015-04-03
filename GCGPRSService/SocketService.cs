using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private string _msg = "";
        public string Msg
        {
            get { return _msg; }
            set { _msg = value; }
        }

        public SocketEventArgs(string msg)
        {
            if (!msg.EndsWith("\r\n"))
                _msg = msg + "\r\n";
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
    }

    public delegate void cmdEventHandle(object sender, SocketEventArgs e);

    public class SocketManager
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("SocketService");
        public ManualResetEvent allDone = new ManualResetEvent(false);
        public event cmdEventHandle cmdEvent;
        Thread t_socket;
        Thread t_send;
        Socket listener;
        bool isRunning = false;

        public virtual void OnSendMsg(SocketEventArgs e)
        {
            if (cmdEvent != null)
                cmdEvent(this, e);
        }


        public void T_Listening()
        {
            OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + " 开启Socket监听线程!"));
            isRunning = true;
            t_socket = new Thread(new ThreadStart(StartListening));
            t_socket.Start();

            OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + " 开启数据库线程!"));
            GlobalValue.Instance.SocketSQLMag.SQLEvent += new SQLHandle(sqlmanager_SQLEvent);
            GlobalValue.Instance.SocketSQLMag.Start();

            t_send = new Thread(new ThreadStart(Send_Thread));
            t_send.Start();
        }

        private void Send_Thread()
        {
            while (true)
            {
                Thread.Sleep(180 * 1000);
                GlobalValue.Instance.SocketSQLMag.Send(SQLType.GetSendParm); //获得上传参数
                GlobalValue.Instance.SocketSQLMag.Send(SQLType.GetUniversalConfig); //获取解析帧的配置数据
            }
        }

        void sqlmanager_SQLEvent(object sender, SQLNotifyEventArgs e)
        {
            if (e.SQLType == SQLType.InsertPreValue)
            {
                if (1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs("保存成功"));
                }
                else if(-1 == e.Result)
                {
                    OnSendMsg(new SocketEventArgs("保存失败:"+e.Msg));
                }
            }
        }

        public void Close()
        {
            try
            {
                isRunning = false;
                if (listener != null && listener.Connected)
                {
                    listener.Shutdown(SocketShutdown.Both);
                    System.Threading.Thread.Sleep(10);
                    listener.Close();
                    listener.Dispose();
                }

                if (t_socket != null && t_socket.IsAlive)
                    t_socket.Abort();

                if (t_send != null && t_send.IsAlive)
                    t_send.Abort();
            }
            catch(Exception e)
            {
                logger.ErrorException("Socket Close", e);
            }
        }

        private void StartListening()
        {
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
                                    //getReadResponse = true;
                                }
                                else if (pack.CommandType == CTRL_COMMAND_TYPE.REQUEST_BY_SLAVE)//接收到的数据帧
                                {
                                    string str_frame = ByteToString(arr, bytesRead);
#if debug
                                    OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + " 收到帧数据:" + str_frame));
#else
                                    OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + " 收到帧数据"));
#endif

                                    #region 解析数据
                                    if (pack.ID3 == (byte)DEV_TYPE.Data_CTRL)
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
                                                framedata.lstFlowData.Add(data);
                                            }
                                            GlobalValue.Instance.GPRS_FlowFrameData.Enqueue(framedata);  //通知存储线程处理
                                            GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertPreValue);
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

                                            OnSendMsg(new SocketEventArgs(string.Format("压力终端[{0}]{1}|时间({2})",
                                                 pack.DevID, alarm, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec)));
                                        }
                                        #endregion
                                    }
                                    else if (pack.ID3 == (byte)DEV_TYPE.UNIVERSAL_CTRL)
                                    {
                                        #region 通用终端
                                        if (pack.C1 == (byte)GPRS_READ.READ_UNIVERSAL_SIM1)  //接受通用终端发送的模拟1路数据
                                        {
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
                                                dr_TerminalDataConfig = GlobalValue.Instance.UniversalDataConfig.Select("TerminalID='" + framedata.TerId + "' AND Sequence='1'"); //WayType
                                            }
                                            if (dr_TerminalDataConfig != null && dr_TerminalDataConfig.Length > 0)
                                            {
                                                float MaxMeasureRange = dr_TerminalDataConfig[0]["MaxMeasureRange"] != DBNull.Value ? Convert.ToSingle(dr_TerminalDataConfig[0]["MaxMeasureRange"]) : 0;
                                                float MaxMeasureRangeFlag = dr_TerminalDataConfig[0]["MaxMeasureRangeFlag"] != DBNull.Value ? Convert.ToSingle(dr_TerminalDataConfig[0]["MaxMeasureRangeFlag"]) : 0;
                                                int datawidth = dr_TerminalDataConfig[0]["FrameWidth"] != DBNull.Value ? Convert.ToInt16(dr_TerminalDataConfig[0]["FrameWidth"]) : 0;
                                                int precision = dr_TerminalDataConfig[0]["precision"] != DBNull.Value ? Convert.ToInt32(dr_TerminalDataConfig[0]["precision"]) : 0;
                                                if (MaxMeasureRangeFlag > 0 && datawidth > 0 && (datawidth % 2 == 0))
                                                {
                                                    int loopdatalen = 6 + datawidth;  //循环部分数据宽度 = 时间(6)+配置长度
                                                    int dataindex = (pack.DataLength - 2 - 1) % loopdatalen;
                                                    if (dataindex != 0)
                                                        throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(2+1+"+loopdatalen+"*n)规则");
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
                                                        OnSendMsg(new SocketEventArgs(string.Format("index({0})|通用终端[{1}]一路|校准值({2})|采集时间({3})|{4}:{5}{6}",
                                                            i, pack.DevID, calibration, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, dr_TerminalDataConfig[0]["Name"].ToString().Trim(), datavalue, dr_TerminalDataConfig[0]["Unit"].ToString())));

                                                        GPRSUniversalDataEntity data = new GPRSUniversalDataEntity();
                                                        data.Sim1 = datavalue;
                                                        data.TypeTableID = Convert.ToInt32(dr_TerminalDataConfig[0]["ID"]);
                                                        data.TableColumnName = "Simulate1";
                                                        data.ColTime = new DateTime(year, month, day, hour, minute, sec);
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
                                        }
                                        #endregion
                                    }
                                    #endregion

                                    Package response = new Package();
                                    response.DevID = pack.DevID;
                                    if (GlobalValue.Instance.lstGprsCmd != null && GlobalValue.Instance.lstGprsCmd.Count > 0  && pack.IsFinal)
                                    {
                                        for (int i = 0; i < GlobalValue.Instance.lstGprsCmd.Count; i++)
                                        {
                                            if (GlobalValue.Instance.lstGprsCmd[i].DeviceId == (int)pack.DevID)
                                            {
                                                response.DevType = pack.DevType;
                                                response.C0 = 0x48;  //主站发出的应答帧，有后续命令
                                                response.C1 = (byte)pack.C1;
                                                response.DataLength = 0;
                                                response.Data = null;
                                                response.CS = response.CreateCS();

                                                byte[] bsenddata = response.ToArray();
#if debug
                                                OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  发送响应帧:" + ByteToString(bsenddata, bsenddata.Length)));
#else
                                                OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  发送响应帧"));
#endif
                                                SendObject sendObj = new SendObject();
                                                sendObj.workSocket = handler;
                                                sendObj.IsFinal = false;
                                                handler.BeginSend(bsenddata, 0, bsenddata.Length, 0, new AsyncCallback(SendCallback), sendObj);

                                                response.C0 = Convert.ToByte(GlobalValue.Instance.lstGprsCmd[i].CtrlCode);
                                                response.C1 = Convert.ToByte(GlobalValue.Instance.lstGprsCmd[i].FunCode);
                                                response.Data = StringToByte(GlobalValue.Instance.lstGprsCmd[i].Data);
                                                response.DataLength = response.Data.Length;
                                                byte[] bsenddata_cmd = response.ToArray();
#if debug
                                                OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  发送命令帧:" + ByteToString(bsenddata_cmd, bsenddata_cmd.Length)));
#else
                                                OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  发送命令帧"));
#endif
                                                sendObj = new SendObject();
                                                sendObj.workSocket = handler;
                                                sendObj.IsFinal = true;
                                                handler.BeginSend(bsenddata_cmd, 0, bsenddata_cmd.Length, 0, new AsyncCallback(SendCallback), sendObj);

                                                GPRSCmdFlag flag = new GPRSCmdFlag();
                                                flag.Index = i;
                                                flag.TableId = GlobalValue.Instance.lstGprsCmd[i].TableId;
                                                GlobalValue.Instance.lstSendedCmdId.Add(flag);
                                                GlobalValue.Instance.SocketSQLMag.Send(SQLType.UpdateSendParmFlag);
                                                break;
                                            }
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
                                        OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  发送响应帧:" + ByteToString(bsenddata, bsenddata.Length)));
#else
                                        OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() + "  发送响应帧"));
#endif
                                        SendObject sendObj = new SendObject();
                                        sendObj.workSocket = handler;
                                        sendObj.IsFinal = pack.IsFinal;
                                        handler.BeginSend(bsenddata, 0, bsenddata.Length, 0, new AsyncCallback(SendCallback), sendObj);
                                    }

                                    if (!pack.IsFinal)
                                    {
                                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                                    }
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
                    string str_buffer = ByteToString(state.buffer, bytesRead);
#if debug
                    OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() +" "+ argex.Message + ",错误数据:" + str_buffer));
#else
                    OnSendMsg(new SocketEventArgs(DateTime.Now.ToString() +" "+ argex.Message));
#endif
                }
                logger.ErrorException("ReadCallback",argex);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception ex)
            {
                logger.ErrorException("ReadCallback", ex);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        }

        private string ByteToString(byte[] lstByte, int len)
        {
            string StringOut = "";
            for (int i = 0; i < len; i++)
            {
                StringOut = StringOut + String.Format("{0:X2} ", lstByte[i]);
            }
            return StringOut;
        }

        public byte[] StringToByte(string InString)
        {
            List<Byte> lstBt = new List<byte>();
            for (int i = 0; i <= InString.Length - 1; i += 2)
            {
                lstBt.Add(Convert.ToByte(("0x" + InString[i] + InString[i + 1]), 16));
            }
            return lstBt.ToArray();
        }   

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                //OnSendMsg(new SocketEventArgs("SendCallback线程ID为:   " + Thread.CurrentThread.ManagedThreadId.ToString()));
                Socket handler = ((SendObject)ar.AsyncState).workSocket;
                int bytesSent = handler.EndSend(ar);
                if (((SendObject)ar.AsyncState).IsFinal)  //如果是最后一帧，则关闭socket
                {
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception e)
            {
                logger.ErrorException(DateTime.Now.ToString() + " SendCallback", e); ;
                OnSendMsg(new SocketEventArgs(e.ToString()));
            }
        }
    }

}
