using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using Utility;
using System.Threading;
using System.Management;
using Common;
using Entity;


namespace Protocol
{
    /// <summary>
    /// 串口开发辅助类
    /// </summary>
    public class SerialPortUtil
    {
        public readonly SerialPort serialPort = null;
        public bool ReceiveEventFlag = false;  //接收事件是否有效 false表示有效
        private NLog.Logger logger = NLog.LogManager.GetLogger("SerialPortUtil");

        private static readonly SerialPortUtil instance = new SerialPortUtil();

        public static SerialPortUtil GetInstance()
        {
            return instance;
        }

        #region 事件
        /// <summary>
        /// 完整协议的记录处理事件
        /// </summary>
        public event PackageReceivedEventHandler PackageReceived;
        /// <summary>
        /// 串口错误事件
        /// </summary>
        public event SerialErrorReceivedEventHandler Error;
        /// <summary>
        /// 状态监控日志
        /// </summary>
        public event AppendBufLogEventHandler AppendBufLog;

        /// <summary>
        /// 读取记录仪数据完成
        /// </summary>
        public event DataCompletedEventHandler DataCompleted;


        // 定义一个事件来提示界面工作的进度
        public event ReadDataChangedEventHandler ValueChanged;

        /// <summary>
        /// 读取历史数据
        /// </summary>
        public event ReadHistoryDataHandle ReadHisData;

        public event ShowMsgHandle ShowMsgEvent;

        #endregion
        /// <summary>
        /// 监控日志
        /// </summary>
        private StringBuilder strLogBuf = new StringBuilder();

        #region 串口状态
        private bool isComRecving = false;
        /// <summary>
        /// 正在读取串口
        /// </summary>
        public bool IsComRecving
        {
            get { return isComRecving; }
            set { IsComRecving = value; }
        }
        private bool isComSending = false;
        /// <summary>
        /// 正在发送串口
        /// </summary>
        public bool IsComSending
        {
            get { return isComSending; }
        }
        private bool isComClosing = false;
        /// <summary>
        /// 正在关闭串口
        /// </summary>
        public bool IsComClosing
        {
            get { return isComClosing; }
        }

        /// <summary>
        /// 串口是否打开
        /// </summary>
        /// <returns></returns>
        public bool IsOpen
        {
            get
            {
                return serialPort.IsOpen;
            }
        }
        #endregion

        #region 监控日志
        public void AppendBufLine(string log)
        {
            strLogBuf.Clear();
            strLogBuf.Append(DateTime.Now.ToString("[hh:mm:ss]"));
            strLogBuf.Append(log);
            strLogBuf.Append("\r\n");
            if (AppendBufLog != null)
            {
                AppendBufLog(new AppendBufLogEventArgs(strLogBuf));
            }
            OnShowMsg(log);
            //logger.Info(log);
        }

        private void AppendBufLine(string log, params object[] args)
        {
            if (args == null)
                AppendBufLine(log);
            else
                AppendBufLine(string.Format(log, args));
        }

        private void AppendBufLine(string log, Package package,Package651 package651)
        {
            if (AppendBufLog != null)
            {
                if (package != null)
                    AppendBufLine(log, ConvertHelper.ByteArrayToHexString(package.ToArray()));
                if (package651 != null)
                    AppendBufLine(log, ConvertHelper.ByteArrayToHexString(package651.ToResponseArray()));
            }
        }

        private void AppendBufLine(string log, Package651 package)
        {
            if (AppendBufLog != null)
            {
                AppendBufLine(log, ConvertHelper.ByteArrayToHexString(package.ToResponseArray()));
            }
        }

        private void OnShowMsg(string msg)
        {
            if (ShowMsgEvent != null)
                ShowMsgEvent(msg);
        }

        #endregion

        #region 构造函数
        /// <summary>
        /// 默认构造函数
        /// </summary>
        private SerialPortUtil()
        {
            serialPort = new SerialPort();
            serialPort.ErrorReceived += new SerialErrorReceivedEventHandler(serialPort_ErrorReceived);
            LoadSerialPort();
        }

        /// <summary>
        /// 加载参数
        /// </summary>
        public void LoadSerialPort()
        {
            try
            {
                serialPort.PortName = PubConstant.PortNameString;
                serialPort.BaudRate = Convert.ToInt32(PubConstant.BaudRateString);
                serialPort.Parity = PubConstant.Parity;
                serialPort.DataBits = Convert.ToInt32(PubConstant.DataBitsString);
                serialPort.StopBits = PubConstant.StopBits;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 接收数据
        void serialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            if (Error != null)
            {
                Error(sender, e);
            }
        }
        #endregion

        #region 串口操作集合

        /// <summary>
        /// 释放串口资源
        /// </summary>
        ~SerialPortUtil()
        {
            Close();
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        public void Open()
        {
            try
            {
                AppendBufLine("正在打开串口\"{0}\"...", serialPort.PortName);
                if (!serialPort.IsOpen)
                {
                    LoadSerialPort();
                    serialPort.Open();
                    AppendBufLine("串口\"{0}\"打开成功！", serialPort.PortName);
                    isComClosing = false;
                }
            }
            catch (Exception ex)
            {
                AppendBufLine("串口打开失败！——", ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        public void Close()
        {
            if (serialPort.IsOpen)
            {
                AppendBufLine("正在关闭串口\"{0}\"...", serialPort.PortName);
                isComClosing = true;
                if (isComSending)
                {
                    isComClosing = false;
                    throw new Exception("当前正在发送指令，无法关闭");
                }
                if (isComRecving)
                {
                    isComClosing = false;
                    throw new Exception("当前接收指令，无法关闭");
                }
                serialPort.Close();
                AppendBufLine("串口\"{0}\"已关闭！", serialPort.PortName);
                isComClosing = false;
            }
        }

        /// <summary>
        /// 数据发送
        /// </summary>
        /// <param name="data">要发送的数据字节</param>
        ///<param name="DiscardInBuffer">是否丢弃缓存区的数据默认是</param>
        public void SendData(byte[] data, bool DiscardInBuffer = true)
        {
            try
            {
                if (!IsOpen)
                {
                    Open();
                }
                isComSending = true;
                AppendBufLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff") + " 发送:{0}", ConvertHelper.ByteArrayToHexString(data));
                if (DiscardInBuffer)
                {
                    serialPort.DiscardInBuffer();
                }
                serialPort.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                isComSending = false;
            }
        }
        List<Package> lstResult = new List<Package>();  //返回结果集(支持多包)
        public List<Package651> lstResult651 = new List<Package651>();  //返回结果集651(支持多包)
        public string receiveErr = "";  //接收错误消息
        public void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            List<byte> packageBytes = new List<byte>();
            try
            {
                isComRecving = true;//正在读取串口数据
                nLastRecTime = Environment.TickCount;
                nStartTime = Environment.TickCount;
                receiveErr = "";
                
                int len = serialPort.BytesToRead;
                byte[] buf = new byte[len];
                bytesRead = serialPort.Read(buf, 0, len);
                if (bytesRead > 0)
                {
                    AppendBufLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff") + " 收到原始数据:{0}", ConvertHelper.ByteArrayToHexString(buf));
                    for (int i = 0; i < bytesRead; i++)
                    {
                        ReceiveBytes.Add(buf[i]);
                    }
                }

                int readCount = 0;//计数

                int index = 0;
                
                while (index < ReceiveBytes.Count)
                {
                    packageBytes.Add(ReceiveBytes[index]);
                    packageBytes = FormatHelper.CheckHead(packageBytes);  //检查数据头
                    if (packageBytes.Count >= PackageDefine.MinLenth && ReceiveBytes[index] == PackageDefine.EndByte)
                    {
                        if (packageBytes[0] != PackageDefine.BeginByte)
                        {
                            bool first_BeginByte = false;
                            for (int i = (packageBytes.Count - 6); i > 0; i--)
                            {
                                if (packageBytes[i] == PackageDefine.BeginByte)
                                {
                                    if (!first_BeginByte)
                                        first_BeginByte = true;
                                    else
                                    {
                                        packageBytes.RemoveRange(0, i);  //倒过去找，找到第二个开始字节
                                        break;
                                    }
                                }
                            }
                        }

                        byte[] arr = packageBytes.ToArray();
                        len = BitConverter.ToInt16(new byte[] { arr[9], arr[8] }, 0);//数据域长度
                        Package pack;
                        if (PackageDefine.MinLenth + len == arr.Length && Package.TryParse(arr, out pack))//找到结束字符并且是完整一帧
                        {
                            int total = pack.IsFinal ? pack.DataLength : pack.AllDataLength;
                            readCount += pack.IsFinal ? pack.DataLength : pack.DataLength - 3;
                            OnValueChanged(new ValueEventArgs() { DevID = pack.DevID, CurrentStep = readCount, TotalStep = total });

                            AppendBufLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff")+" 收到:{0}", ConvertHelper.ByteArrayToHexString(arr));
                            packageBytes.Clear();
                            ReceiveBytes.RemoveRange(0, index + 1);
                            index = -1;
                            lstResult.Add(pack);
                        }
                    }
                    if (packageBytes.Count >= PackageDefine.MinLenth651 && 
                        (packageBytes[packageBytes.Count - 1 - 2] == PackageDefine.EndByte651 || packageBytes[packageBytes.Count - 1 - 2] == PackageDefine.EndByte_Continue
                        || packageBytes[packageBytes.Count - 1 - 2] == PackageDefine.ENQ || packageBytes[packageBytes.Count - 1 - 2] == PackageDefine.EOT
                        || packageBytes[packageBytes.Count - 1 - 2] == PackageDefine.ESC))
                    {
                        bool need_response = true;    //是否回复相应帧(连续几个帧，只回最后一个帧)
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
                            Package651 pack651;
                            bool havesubsequent = false;  //是否有后续包
                            string subsequentmsg = "";  //如果是包且有后续，提示当前包数
                            if ((PackageDefine.MinLenth651 <= arr.Length) & Package651.TryParse(arr, out pack651, out havesubsequent, out subsequentmsg))//找到结束字符并且是完整一帧
                            {
                                //AppendBufLine("收到{0}:{1}", subsequentmsg, ConvertHelper.ByteArrayToHexString(arr));
                                packageBytes.Clear();
                                ReceiveBytes.RemoveRange(0, index + 1);
                                index = -1;
                                lstResult651.Add(pack651);
                            }
                        }
                        else if (index > 0 && (index + PackageDefine.MinLenth651 < ReceiveBytes.Count) && 
                            (ReceiveBytes[index + 1] == PackageDefine.BeginByte651[0] && ReceiveBytes[index + 2] == PackageDefine.BeginByte651[1]))
                        {
                            //如果有错误数据在前面，将其去掉
                            AppendBufLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff") + " 忽略错误数据:{0}", ConvertHelper.ByteArrayToHexString(packageBytes.ToArray()));
                            packageBytes.Clear();
                            ReceiveBytes.RemoveRange(0, index + 1);
                            index = -1;
                        }
                        #endregion
                    }

                    index++;
                }
            }
            catch (Exception ex)
            {
                AppendBufLine("接收错误:{0}", ex.Message);
                AppendBufLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff") + " 错误数据:{0}", ConvertHelper.ByteArrayToHexString(packageBytes.ToArray()));
                receiveErr = ex.Message;
            }
            finally
            {
                //Thread.Sleep(20);
                isComRecving = false;
            }
        }
        int bytesRead = 0;
        List<byte> ReceiveBytes = new List<byte>();
        int nLastRecTime = Environment.TickCount;   //单次接收超时时间
        int nStartTime = Environment.TickCount;     //总接收超时时间
        public List<T> SendCommand651<T>(byte[] sendData, int timeout = 5, bool needresp = true) where T : struct
        {
            try
            {
                SendData(sendData,false);

                if (!needresp)
                {
                    List<T> lstT = new List<T>();
                    lstT.Add(default(T));
                    return lstT;        //不需要获取下位机返回数据,如SL651确认帧
                }
                lstResult.Clear();
                lstResult651.Clear();
                serialPort.DiscardInBuffer();   //清空接收缓冲区     
                ReceiveBytes.Clear();

                nStartTime = Environment.TickCount;
                isComRecving = true;//正在读取串口数据
                while (true)
                {
                    Thread.Sleep(50);

                    if (Environment.TickCount - nStartTime > timeout * 1000)    //超时
                    {
                        throw new TimeoutException("等待超时...");
                    }
                    if (IsComClosing)//关闭窗口
                    {
                        AppendBufLine("获取数据中途串口关闭！", null);
                        throw new Exception("关闭串口，停止获取数据。");
                    }

                    //get result
                    if (!isComRecving)
                    {
                        if ((lstResult651.Count > 0 && lstResult651[0].SumPackCount == lstResult651[lstResult651.Count - 1].CurPackCount) || Environment.TickCount - nLastRecTime > 2 * 1000)    //超时1s
                        {
                            if (!string.IsNullOrEmpty(receiveErr))
                                throw new Exception(receiveErr);

                            if (typeof(T) == typeof(Package))
                            {
                                //serialPort.DataReceived -= SerialPort_DataReceived;
                                return (List<T>)((object)lstResult);
                            }
                            if (typeof(T) == typeof(Package651))
                            {
                                //serialPort.DataReceived -= SerialPort_DataReceived;
                                return (List<T>)((object)lstResult651);
                            }
                        }
                    }
                }



            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                isComRecving = false;
            }
        }
        
        public T SendCommand<T>(byte[] sendData, int timeout = 5, bool needresp = true, bool readnextpack = false) where T : struct
        {
            try
            {
                if (!readnextpack)  //如果readnextpack为true时，表示收到的是多包数据，还有未收完的包，接着接收,不需要发送数据
                {
                    //serialPort.DiscardInBuffer();   //清空接收缓冲区     
                    SendData(sendData,false);
                    if (!needresp)
                        return default(T);   //不需要获取下位机返回数据,如SL651确认帧
                    Thread.Sleep(50);                       //延时，等待下位机回数据
                }

                List<byte> packageBytes = new List<byte>();
                int readCount = 0;//计数

                int nStartTime = Environment.TickCount;
                while (true)
                {
                    if (Environment.TickCount - nStartTime > timeout * 1000)    //超时
                    {
                        throw new TimeoutException("等待超时...");
                    }
                    if (IsComClosing)//关闭窗口
                    {
                        AppendBufLine("获取数据中途串口关闭！",null);
                        throw new Exception("关闭串口，停止获取数据。");
                    }
                    isComRecving = true;//正在读取串口数据
                    int bytesRead = 0;
                    List<byte> ReceiveBytes = new List<byte>();
                    while (serialPort.BytesToRead > 0)
                    {
                        int len = serialPort.BytesToRead;
                        byte[] buf = new byte[len];
                        bytesRead = serialPort.Read(buf, 0, len);
                        //foreach (var item in buf)
                        //{
                        //    ByteQueue.Enqueue(item);
                        //}
                        for (int i = 0; i < bytesRead; i++)
                        {
                            ReceiveBytes.Add(buf[i]);
                        }
                    }
                    int index = 0;
                    while (index < bytesRead)
                    {
                        //byte byteItem = ByteQueue.Dequeue();
                        packageBytes.Add(ReceiveBytes[index]);
                        if (ReceiveBytes[index] == PackageDefine.EndByte && packageBytes.Count >= PackageDefine.MinLenth)
                        {
                            //if (bytes[0] == (byte)0x00)
                            //    bytes.RemoveAt(0);

                            if (packageBytes[0] != PackageDefine.BeginByte)
                            {
                                bool first_BeginByte = false;
                                for (int i = (packageBytes.Count - 6); i > 0; i--)
                                {
                                    if (packageBytes[i] == PackageDefine.BeginByte)
                                    {
                                        if (!first_BeginByte)
                                            first_BeginByte = true;
                                        else
                                        {
                                            packageBytes.RemoveRange(0, i);  //倒过去找，找到第二个开始字节
                                            break;
                                        }
                                    }
                                }
                            }

                            byte[] arr = packageBytes.ToArray();
                            int len = BitConverter.ToInt16(new byte[] { arr[9], arr[8] }, 0);//数据域长度

                            Package pack;
                            if (PackageDefine.MinLenth + len == arr.Length && Package.TryParse(arr, out pack))//找到结束字符并且是完整一帧
                            {
                                int total = pack.IsFinal ? pack.DataLength : pack.AllDataLength;
                                readCount += pack.IsFinal ? pack.DataLength : pack.DataLength - 3;
                                OnValueChanged(new ValueEventArgs() { DevID = pack.DevID, CurrentStep = readCount, TotalStep = total });

                                AppendBufLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff")+" 收到:{0}", ConvertHelper.ByteArrayToHexString(arr));
                                //OnReadPackege(new PackageReceivedEventArgs(pack));
                                packageBytes.Clear();
                                return (T)((object)pack);
                            }
                        }
                        else if (packageBytes.Count >= PackageDefine.MinLenth651 && (packageBytes[packageBytes.Count - 1 - 2] == PackageDefine.EndByte651 || packageBytes[packageBytes.Count - 1 - 2] == PackageDefine.EndByte_Continue))
                        {
                            bool need_response = true;    //是否回复相应帧(连续几个帧，只回最后一个帧)
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
                                    AppendBufLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff") + " 收到{0}:{1}",subsequentmsg, ConvertHelper.ByteArrayToHexString(arr));
                                    packageBytes.Clear();
                                    return (T)((object)pack);
                                }
                            }
                            #endregion
                        }

                        index++;
                    }
                    System.Threading.Thread.Sleep(20);
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                isComRecving = false;
            }
        }

        public Package SendPackage(Package package, int timeout = 3, int times = 2)
        {
            try
            {
                Thread.Sleep(100);
                Package result = SendCommand<Package>(package.ToArray(), timeout);
                return result;
            }
            catch (Exception ex)
            {
                times--;
                if (times > 0)
                {
                    AppendBufLine("重试:剩余{0}次尝试", times);
                    return SendPackage(package, timeout, times);
                }
                else
                {
                    AppendBufLine("错误:{0}", "等待超时...");
                    throw;
                }
            }
        }


        public List<Package651> SendPackage(Package651 package, int timeout = 3, int times = 2, bool needresp = true, bool readnextpack = false)
        {
            try
            {
                List<Package651> result = SendCommand651<Package651>(package.ToResponseArray(), timeout, needresp);
                return result;
            }
            catch (Exception ex)
            {
                times--;
                if (times > 0)
                {
                    AppendBufLine("重试:剩余{0}次尝试", times);
                    return SendPackage(package, timeout, times);
                }
                else
                {
                    AppendBufLine("错误:{0}", "等待超时...");
                    throw;
                }
            }
        }

        public void Send(Package package)
        {
            try
            {
                byte[] sendData = package.ToArray();
                SendData(sendData);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public short[] ReadData(short id, int timeout = 30)
        {
            try
            {
                serialPort.DiscardInBuffer();   //清空接收缓冲区    

                Package package = new Package();
                package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_LOG;
                package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
                package.DevID = id;

                package.C1 = (byte)NOISE_LOG_COMMAND.CTRL_START_READ;
                package.DataLength = 0;
                package.CS = package.CreateCS();

                AppendBufLine("开始获取设备{0}数据...", id);
                Send(package);

                Queue<byte> ByteQueue = new Queue<byte>();
                List<Package> packageCache = new List<Package>();
                List<byte> packageBytes = new List<byte>();
                int readCount = 0;//计数

                int nStartTime = Environment.TickCount;
                bool getReadResponse = false;  //是否响应了读取命令
                while (true)
                {
                    if (Environment.TickCount - nStartTime > timeout * 1000)    //超时
                    {
                        AppendBufLine("获取设备{0}数据等待超时...[当前设置为{1}秒超时]", id, timeout);
                        if (getReadResponse)
                            throw new ArgumentNullException("没有读取到数据或数据不完整");
                        else
                            throw new TimeoutException("等待超时...");
                    }
                    if (IsComClosing)//关闭窗口
                    {
                        AppendBufLine("获取设备{0}数据中途串口被关闭！", id);
                        throw new Exception("关闭串口，停止获取数据。");
                    }
                    isComRecving = true;//正在读取串口数据
                    while (serialPort.BytesToRead > 0)
                    {
                        int len = serialPort.BytesToRead;
                        byte[] buf = new byte[len];
                        serialPort.Read(buf, 0, len);
                        foreach (var item in buf)
                        {
                            ByteQueue.Enqueue(item);
                        }
                    }

                    while (ByteQueue.Count > 0)
                    {
                        byte byteItem = ByteQueue.Dequeue();
                        packageBytes.Add(byteItem);
                        //byteStack.Push(ByteQueue.Dequeue());
                        if (byteItem == PackageDefine.EndByte && packageBytes.Count >= PackageDefine.MinLenth)
                        {
                            byte[] arr = packageBytes.ToArray();
                            int len = BitConverter.ToInt16(new byte[] { arr[9], arr[8] }, 0);//数据域长度

                            Package pack;
                            if (PackageDefine.MinLenth + len == arr.Length && Package.TryParse(arr, out pack))//找到结束字符并且是完整一帧
                            {
                                nStartTime = Environment.TickCount;
                                if (pack.CommandType == CTRL_COMMAND_TYPE.RESPONSE_BY_SLAVE)  //接受到应答,判断是否D11是否为1,如果为0,表示没有数据需要读
                                {
                                    //if (((arr[6] >> 3) & 0x01) == 0)   //  ==1表示D11为1,有后续命令
                                    //{
                                    //    throw new ArgumentNullException("没有读取到数据");
                                    //}
                                    getReadResponse = true;
                                }
                                else if (pack.CommandType == CTRL_COMMAND_TYPE.REQUEST_BY_SLAVE)//接收到的数据帧
                                {
                                    int total = pack.IsFinal ? pack.DataLength : pack.AllDataLength;
                                    readCount += pack.IsFinal ? pack.DataLength : pack.DataLength - 3;
                                    OnValueChanged(new ValueEventArgs() { DevID = pack.DevID, CurrentStep = readCount, TotalStep = total });

                                    OnReadPackege(new PackageReceivedEventArgs(pack));//触发事件
                                    packageCache.Add(pack);
                                    AppendBufLine("已收到第{0}帧", packageCache.Count);
                                    //AppendBufLine("第{0}帧:{1}", pack.DataNum, pack);

                                    Package response = new Package();
                                    response.DevType = Entity.ConstValue.DEV_TYPE.NOISE_LOG;
                                    response.DevID = pack.DevID;
                                    response.CommandType = CTRL_COMMAND_TYPE.RESPONSE_BY_SLAVE;
                                    response.C1 = (byte)NOISE_LOG_COMMAND.SEND_RESPONSE_DATA;
                                    response.DataLength = 0;
                                    response.Data = null;
                                    response.CS = response.CreateCS();
                                    AppendBufLine("发送回应...", null);
                                    SendData(response.ToArray(), false);

                                }
                                packageBytes.Clear();
                            }
                        }
                    }

                    if (packageCache.Exists(obj => obj.IsFinal))
                    {
                        Package final = packageCache.Find(obj => obj.IsFinal);
                        List<Package> tmp = packageCache.FindAll(obj => obj.DevID == final.DevID).Distinct().ToList();
                        List<byte> result = new List<byte>();

                        var q = from p in tmp
                                orderby p.DataNum
                                select p;

                        foreach (var item in q)
                        {
                            for (int i = 3; i < item.DataLength; i++)
                            {
                                result.Add(item.Data[i]);
                            }
                        }

                        List<Int16> output = new List<Int16>();

                        byte[] t = new byte[2];
                        for (int i = 0; i + 1 < result.Count; i = i + 2)
                        {
                            t[0] = result[i];
                            t[1] = result[i + 1];
                            output.Add(BitConverter.ToInt16(t, 0));
                        }

                        OnReadData(new ReadDataEventArgs(final.DevID, output.ToArray()));
                        packageCache.Clear();
                        AppendBufLine("获取完毕!", null);

                        packageCache.Clear();

                        //clear data
                        //Package clear = new Package();
                        //clear.DevType = DEV_TYPE.NOISE_LOG;
                        //clear.DevID = id;
                        //clear.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
                        //clear.C1 = (byte)NOISE_LOG_COMMAND.CTRL_CLEAR_FLASH;
                        //clear.DataLength = 0;
                        //clear.Data = null;
                        //clear.CS = clear.CreateCS();
                        //AppendBufLine("读取数据后清除");
                        //SendData(clear.ToArray(), false);

                        return output.ToArray();
                    }
                    System.Threading.Thread.Sleep(10);
                }
                
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                isComRecving = false;//正在读取串口数据
            }

        }

        /// <summary>
        /// 启动并获取静态标准值
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public short[] ReadOrigityData(Package package, int timeout = 30)
        {
            try
            {
                serialPort.DiscardInBuffer();   //清空接收缓冲区    

                AppendBufLine("开始获取设备{0}数据...", package.DevID);
                Send(package);

                Queue<byte> ByteQueue = new Queue<byte>();
                List<Package> packageCache = new List<Package>();
                List<byte> packageBytes = new List<byte>();
                int readCount = 0;//计数

                int nStartTime = Environment.TickCount;
                bool getReadResponse = false;  //是否响应了读取命令
                while (true)
                {
                    if (Environment.TickCount - nStartTime > timeout * 1000)    //超时
                    {
                        AppendBufLine("获取设备{0}数据等待超时...[当前设置为{1}秒超时]", package.DevID, timeout);
                        if (getReadResponse)
                            throw new Exception("没有读取到数据!");
                        else
                            throw new TimeoutException("等待超时...");
                    }
                    if (IsComClosing)//关闭窗口
                    {
                        AppendBufLine("获取设备{0}数据中途串口被关闭！", package.DevID);
                        throw new Exception("关闭串口，停止获取数据。");
                    }
                    isComRecving = true;//正在读取串口数据
                    while (serialPort.BytesToRead > 0)
                    {
                        int len = serialPort.BytesToRead;
                        byte[] buf = new byte[len];
                        serialPort.Read(buf, 0, len);
                        foreach (var item in buf)
                        {
                            ByteQueue.Enqueue(item);
                        }
                    }

                    while (ByteQueue.Count > 0)
                    {
                        byte byteItem = ByteQueue.Dequeue();
                        packageBytes.Add(byteItem);
                        if (byteItem == PackageDefine.EndByte && packageBytes.Count >= PackageDefine.MinLenth)
                        {
                            byte[] arr = packageBytes.ToArray();
                            int len = BitConverter.ToInt16(new byte[] { arr[9], arr[8] }, 0);//数据域长度

                            Package pack;
                            if (PackageDefine.MinLenth + len == arr.Length && Package.TryParse(arr, out pack))//找到结束字符并且是完整一帧
                            {
                                if (pack.CommandType == CTRL_COMMAND_TYPE.RESPONSE_BY_SLAVE)
                                {
                                    getReadResponse = true;
                                }
                                else if (pack.CommandType == CTRL_COMMAND_TYPE.REQUEST_BY_SLAVE)//接收到的数据帧
                                {
                                    int total = pack.IsFinal ? pack.DataLength : pack.AllDataLength;
                                    readCount += pack.IsFinal ? pack.DataLength : pack.DataLength - 3;
                                    OnValueChanged(new ValueEventArgs() { DevID = pack.DevID, CurrentStep = readCount, TotalStep = total });

                                    OnReadPackege(new PackageReceivedEventArgs(pack));//触发事件
                                    packageCache.Add(pack);
                                    //AppendBufLine("已收到第{0}帧", packageCache.Count);
                                    AppendBufLine("第{0}帧:{1}", pack.DataNum, pack);

                                    Package response = new Package();
                                    response.DevType = Entity.ConstValue.DEV_TYPE.NOISE_LOG;
                                    response.DevID = pack.DevID;
                                    response.CommandType = CTRL_COMMAND_TYPE.RESPONSE_BY_SLAVE;
                                    response.C1 = (byte)NOISE_LOG_COMMAND.SEND_RESPONSE_DATA_ORIGITY;
                                    response.DataLength = 0;
                                    response.Data = null;
                                    response.CS = response.CreateCS();
                                    AppendBufLine("发送回应...", null);
                                    SendData(response.ToArray(), false);

                                }
                                packageBytes.Clear();
                            }
                        }
                    }

                    if (packageCache.Exists(obj => obj.IsFinal))
                    {
                        Package final = packageCache.Find(obj => obj.IsFinal);
                        List<Package> tmp = packageCache.FindAll(obj => obj.DevID == final.DevID).Distinct().ToList();
                        List<byte> result = new List<byte>();

                        var q = from p in tmp
                                orderby p.DataNum
                                select p;

                        foreach (var item in q)
                        {
                            for (int i = 0; i < item.DataLength; i++)
                            {
                                result.Add(item.Data[i]);
                            }
                        }

                        List<Int16> output = new List<Int16>();

                        byte[] t = new byte[2];
                        for (int i = 0; i < result.Count; i = i + 2)
                        {
                            t[0] = result[i];
                            t[1] = result[i + 1];
                            output.Add(BitConverter.ToInt16(t, 0));
                        }

                        //OnReadData(new ReadDataEventArgs(final.DevID, output.ToArray()));
                        packageCache.Clear();
                        AppendBufLine("获取完毕!", null);

                        packageCache.Clear();

                        return output.ToArray();
                    }
                    System.Threading.Thread.Sleep(20);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                isComRecving = false;//正在读取串口数据
            }
        }

        /// <summary>
        /// 获取历史数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public void ReadHistoryData(Package package,HydrantOptType opt, int timeout = 15)
        {
            try
            {
                serialPort.DiscardInBuffer();   //清空接收缓冲区    

                AppendBufLine("开始获取设备{0}历史数据...", package.DevID);
                Send(package);

                Queue<byte> ByteQueue = new Queue<byte>();
                List<Package> packageCache = new List<Package>();
                List<byte> packageBytes = new List<byte>();
                int readCount = 0;//计数

                int nStartTime = Environment.TickCount;
                bool getReadResponse = false;  //是否响应了读取命令
                while (true)
                {
                    if (Environment.TickCount - nStartTime > timeout * 1000)    //超时
                    {
                        AppendBufLine("获取设备{0}数据等待超时...[当前设置为{1}秒超时]", package.DevID, timeout);
                        if (getReadResponse)
                            throw new ArgumentNullException("读取完成");
                        else
                            throw new TimeoutException("等待超时...");
                    }
                    if (IsComClosing)//关闭窗口
                    {
                        AppendBufLine("获取设备{0}数据中途串口被关闭！", package.DevID);
                        throw new Exception("关闭串口，停止获取数据。");
                    }
                    isComRecving = true;//正在读取串口数据
                    while (serialPort.BytesToRead > 0)
                    {
                        int len = serialPort.BytesToRead;
                        byte[] buf = new byte[len];
                        serialPort.Read(buf, 0, len);
                        foreach (var item in buf)
                        {
                            ByteQueue.Enqueue(item);
                        }
                    }

                    while (ByteQueue.Count > 0)
                    {
                        byte byteItem = ByteQueue.Dequeue();
                        packageBytes.Add(byteItem);
                        if (byteItem == PackageDefine.EndByte && packageBytes.Count >= PackageDefine.MinLenth)
                        {
                            byte[] arr = packageBytes.ToArray();
                            int len = BitConverter.ToInt16(new byte[] { arr[9], arr[8] }, 0);//数据域长度

                            Package pack;
                            if (PackageDefine.MinLenth + len == arr.Length && Package.TryParse(arr, out pack))//找到结束字符并且是完整一帧
                            {
                                nStartTime = Environment.TickCount;
                                OnReadPackege(new PackageReceivedEventArgs(pack));//触发事件
                                packageCache.Add(pack);
                                getReadResponse = true;
                                if (pack.DataLength == 0)
                                    throw new ArgumentNullException("读取完成,无数据");

                                int datacheck = -1;
                                int dataindex = -1;
                                int partdatalen = 6;
                                if (opt == HydrantOptType.Open)
                                    partdatalen = 9;
                                else if (opt == HydrantOptType.Close)
                                    partdatalen = 6;
                                else if (opt == HydrantOptType.OpenAngle)
                                    partdatalen = 7;
                                else if (opt == HydrantOptType.Impact)
                                    partdatalen = 6;
                                else if (opt == HydrantOptType.KnockOver)
                                    partdatalen = 6;

                                datacheck = pack.DataLength % partdatalen;
                                dataindex = pack.DataLength / partdatalen;

                                if (datacheck != 0)
                                {
                                    AppendBufLine(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(2+1+8*n)规则", null);
                                }

                                int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                                float pressuevalue = 0, openangle = 0;
                                for (int i = 0; i < dataindex; i++)
                                {
                                    year = 2000 + Convert.ToInt16(pack.Data[i * partdatalen]);
                                    month = Convert.ToInt16(pack.Data[i * partdatalen + 1]);
                                    day = Convert.ToInt16(pack.Data[i * partdatalen + 2]);
                                    hour = Convert.ToInt16(pack.Data[i * partdatalen + 3]);
                                    minute = Convert.ToInt16(pack.Data[i * partdatalen + 4]);
                                    sec = Convert.ToInt16(pack.Data[i * partdatalen + 5]);

                                    if (opt == HydrantOptType.Open || opt == HydrantOptType.OpenAngle)
                                        openangle = Convert.ToSingle(pack.Data[i * partdatalen + 6]);
                                    if (opt == HydrantOptType.Open)
                                        pressuevalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[i * partdatalen + 8], pack.Data[i * partdatalen + 7] }, 0)) / 1000;

                                    HistoryValueArgs hisvalueArg = new HistoryValueArgs();
                                    hisvalueArg.CollectTime = new DateTime(year, month, day, hour, minute, sec);
                                    hisvalueArg.PreValue = pressuevalue;
                                    hisvalueArg.OpenAngle = openangle;
                                    if (ReadHisData != null)
                                        ReadHisData(hisvalueArg);
                                }

                                int total = pack.IsFinal ? pack.DataLength : pack.AllDataLength;
                                readCount += pack.IsFinal ? pack.DataLength : pack.DataLength - 3;
                                OnValueChanged(new ValueEventArgs() { DevID = pack.DevID, CurrentStep = readCount, TotalStep = total });

                                packageBytes.Clear();
                            }
                        }
                    }

                    if (packageCache.Exists(obj => obj.IsFinal))
                    {
                        Package final = packageCache.Find(obj => obj.IsFinal);
                        List<Package> tmp = packageCache.FindAll(obj => obj.DevID == final.DevID).Distinct().ToList();
                        List<byte> result = new List<byte>();

                        var q = from p in tmp
                                orderby p.DataNum
                                select p;

                        foreach (var item in q)
                        {
                            for (int i = 0; i < item.DataLength; i++)
                            {
                                result.Add(item.Data[i]);
                            }
                        }

                        packageCache.Clear();
                    }
                    System.Threading.Thread.Sleep(20);
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                isComRecving = false;//正在读取串口数据
            }
        }

        #region 触发外部监听事件
        /// <summary>
        /// 触发读取数据事件的方法
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnReadData(ReadDataEventArgs e)
        {
            if (this.DataCompleted != null)
            {
                DataCompleted(e);
            }
        }

        protected virtual void OnReadPackege(PackageReceivedEventArgs e)
        {
            if (e.packtype == typeof(Package))
                AppendBufLine("收到:{0}", e.Package);
            else
                AppendBufLine("收到:{0}", e.Package651);
            if (this.PackageReceived != null)
            {
                PackageReceived(e);
            }
        }

        // 触发事件的方法
        protected virtual void OnValueChanged(ValueEventArgs e)
        {
            if (this.ValueChanged != null)
            {
                this.ValueChanged(this, e);
            }
        }

        #endregion

        #endregion

        #region 串口信息

        /// <summary>
        /// 获得当前所有串口
        /// </summary>
        /// <returns></returns>
        public static string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }

        /// <summary>
        /// 检查端口名称是否存在
        /// </summary>
        /// <param name="port_name"></param>
        /// <returns></returns>
        public static bool Exists(string port_name)
        {
            foreach (string port in SerialPort.GetPortNames()) if (port == port_name) return true;
            return false;
        }

        /// <summary>
        /// 格式化端口相关属性
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static string Format(SerialPort port)
        {
            return String.Format("{0} ({1},{2},{3},{4},{5})",
                port.PortName, port.BaudRate, port.DataBits, port.StopBits, port.Parity, port.Handshake);
        }

        //获取串口信息
        /// <summary>
        /// 枚举win32 api
        /// </summary>
        public enum HardwareEnum
        {
            // 硬件
            Win32_Processor, // CPU 处理器
            Win32_PhysicalMemory, // 物理内存条
            Win32_Keyboard, // 键盘
            Win32_PointingDevice, // 点输入设备，包括鼠标。
            Win32_FloppyDrive, // 软盘驱动器
            Win32_DiskDrive, // 硬盘驱动器
            Win32_CDROMDrive, // 光盘驱动器
            Win32_BaseBoard, // 主板
            Win32_BIOS, // BIOS 芯片
            Win32_ParallelPort, // 并口
            Win32_SerialPort, // 串口
            Win32_SerialPortConfiguration, // 串口配置
            Win32_SoundDevice, // 多媒体设置，一般指声卡。
            Win32_SystemSlot, // 主板插槽 (ISA & PCI & AGP)
            Win32_USBController, // USB 控制器
            Win32_NetworkAdapter, // 网络适配器
            Win32_NetworkAdapterConfiguration, // 网络适配器设置
            Win32_Printer, // 打印机
            Win32_PrinterConfiguration, // 打印机设置
            Win32_PrintJob, // 打印机任务
            Win32_TCPIPPrinterPort, // 打印机端口
            Win32_POTSModem, // MODEM
            Win32_POTSModemToSerialPort, // MODEM 端口
            Win32_DesktopMonitor, // 显示器
            Win32_DisplayConfiguration, // 显卡
            Win32_DisplayControllerConfiguration, // 显卡设置
            Win32_VideoController, // 显卡细节。
            Win32_VideoSettings, // 显卡支持的显示模式。

            // 操作系统
            Win32_TimeZone, // 时区
            Win32_SystemDriver, // 驱动程序
            Win32_DiskPartition, // 磁盘分区
            Win32_LogicalDisk, // 逻辑磁盘
            Win32_LogicalDiskToPartition, // 逻辑磁盘所在分区及始末位置。
            Win32_LogicalMemoryConfiguration, // 逻辑内存配置
            Win32_PageFile, // 系统页文件信息
            Win32_PageFileSetting, // 页文件设置
            Win32_BootConfiguration, // 系统启动配置
            Win32_ComputerSystem, // 计算机信息简要
            Win32_OperatingSystem, // 操作系统信息
            Win32_StartupCommand, // 系统自动启动程序
            Win32_Service, // 系统安装的服务
            Win32_Group, // 系统管理组
            Win32_GroupUser, // 系统组帐号
            Win32_UserAccount, // 用户帐号
            Win32_Process, // 系统进程
            Win32_Thread, // 系统线程
            Win32_Share, // 共享
            Win32_NetworkClient, // 已安装的网络客户端
            Win32_NetworkProtocol, // 已安装的网络协议
            Win32_PnPEntity,//all device
        }
        /// <summary>
        /// WMI取硬件信息
        /// </summary>
        /// <param name="hardType"></param>
        /// <param name="propKey"></param>
        /// <returns></returns>
        public List<SerialInfoEntity> MulGetHardwareInfo(HardwareEnum hardType)
        {

            List<SerialInfoEntity> lstSerials = new List<SerialInfoEntity>();
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + hardType))
                {
                    var hardInfos = searcher.Get();
                    foreach (var hardInfo in hardInfos)
                    {
                        if (hardInfo.Properties["Name"].Value.ToString().Contains("COM"))
                        {
                            SerialInfoEntity entity = new SerialInfoEntity();
                            entity.SerialFullName = hardInfo.Properties["Name"].Value.ToString();
                            int index = entity.SerialFullName.IndexOf("COM");
                            if (index > -1)
                            {
                                entity.SerialName = entity.SerialFullName.Substring(index, entity.SerialFullName.Length - index-1);
                            }
                            //entity.SerialName = hardInfo.Properties["DeviceID"].Value.ToString();
                            lstSerials.Add(entity);
                        }
                    }
                    searcher.Dispose();
                }
                return lstSerials;
            }
            catch
            {
                return null;
            }
            finally
            { lstSerials = null; }
        }
        #endregion
    }

    /// <summary>
    /// 数据返回事件参数
    /// </summary>
    public class PackageReceivedEventArgs : EventArgs
    {
        public Package Package;
        public Package651 Package651;
        public string DataReceived;
        public Type packtype;
        public PackageReceivedEventArgs(string m_DataReceived)
        {
            this.DataReceived = m_DataReceived;
        }
        public PackageReceivedEventArgs(Package package)
        {
            this.Package = package;
            packtype = typeof(Package);
        }

        public PackageReceivedEventArgs(Package651 package)
        {
            this.Package651 = package;
            packtype = typeof(Package651);
        }
    }

    /// <summary>
    /// 监控日志事件参数
    /// </summary>
    public class AppendBufLogEventArgs : EventArgs
    {
        /// <summary>
        /// 最新日志内容
        /// </summary>
        public StringBuilder StrLogBuf { get; set; }
        public AppendBufLogEventArgs(StringBuilder strLogBuf)
        {
            this.StrLogBuf = strLogBuf;
        }
    }

    /// <summary>
    /// 数据返回事件参数
    /// </summary>
    public class ReadDataEventArgs : EventArgs
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public short id;
        /// <summary>
        /// 返回的数据
        /// </summary>
        public short[] Data;
        public ReadDataEventArgs(short devID, short[] data)
        {
            this.id = devID;
            this.Data = data;
        }
    }

    // 定义事件的参数类
    public class ValueEventArgs : EventArgs
    {
        public int TotalStep { set; get; }
        public int CurrentStep { set; get; }
        public short DevID { get; set; }
    }

    public class HistoryValueArgs : EventArgs
    {
        public DateTime CollectTime { set; get; }
        public float PreValue { set; get; }
        public float OpenAngle { get; set; }
    }

    public delegate void AppendBufLogEventHandler(AppendBufLogEventArgs e);

    // 定义事件使用的委托
    public delegate void ReadDataChangedEventHandler(object sender, ValueEventArgs e);

    public delegate void PackageReceivedEventHandler(PackageReceivedEventArgs e);


    public delegate void DataCompletedEventHandler(ReadDataEventArgs e);


    public delegate void ReadHistoryDataHandle(HistoryValueArgs e);

    public delegate void ShowMsgHandle(string msg);
}
