using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using NoiseAnalysisSystem;
using System.Threading.Tasks;

namespace Utility
{
    /// <summary>
    /// 提供对串口的统一访问
    /// </summary>
    public sealed class SerialPortHelper
    {
        #region 事件和字段定义
        public event PortDataReceivedEventHandle Received;
        public SerialPort serialPort = null;
        public bool ReceiveEventFlag = false;  //接收事件是否有效 false表示有效


        #region 串口状态

        private bool isComRecved = false;
        /// <summary>
        /// 串口数据返回完毕
        /// </summary>
        public bool IsComRecved
        {
            get { return isComRecved; }
        }

        private bool isComRecving = false;
        /// <summary>
        /// 正在读取串口
        /// </summary>
        public bool IsComRecving
        {
            get { return isComRecving; }
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

        private static readonly SerialPortHelper instance = new SerialPortHelper();

        #endregion

        #region 属性定义
        private string protName;
        public string PortName
        {
            get { return serialPort.PortName; }
            set
            {
                serialPort.PortName = value;
                protName = value;
            }
        }
        #endregion

        #region 构造函数
        private SerialPortHelper()
        {
            serialPort = new SerialPort();
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
            serialPort.DataReceived += new SerialDataReceivedEventHandler(Command_DataReceived);
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
                serialPort.Parity = Parity.None;
                serialPort.DataBits = Convert.ToInt32(PubConstant.DataBitsString);
                serialPort.StopBits = StopBits.One;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        void Command_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            isComRecved = true;
        }


        #endregion

        #region 串口操作集合
        /// <summary>
        /// 返回串口对象的单个实例
        /// </summary>
        /// <returns></returns>
        public static SerialPortHelper GetSerialPortDao()
        {
            return instance;
        }

        /// <summary>
        /// 释放串口资源
        /// </summary>
        ~SerialPortHelper()
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
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                    isComClosing = false;
                }
            }
            catch (Exception ex)
            {
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

                isComClosing = false;

            }
        }


        /// <summary>
        /// 数据发送
        /// </summary>
        /// <param name="data">要发送的数据字节</param>
        public void SendData(byte[] data)
        {
            isComSending = true;
            try
            {
                serialPort.DiscardInBuffer();
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

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="SendData">发送数据</param>
        /// <param name="ReceiveData">接收数据</param>
        /// <param name="Overtime">超时时间</param>
        /// <returns></returns>
        public int SendCommand(byte[] SendData, ref byte[] ReceiveData, int Overtime)
        {
            if (serialPort.IsOpen)
            {
                isComSending = true;
                try
                {
                    ReceiveEventFlag = true;        //关闭接收事件
                    serialPort.DiscardInBuffer();   //清空接收缓冲区                 
                    serialPort.Write(SendData, 0, SendData.Length);
                    System.Threading.Thread.Sleep(50);
                    int num = 0, ret = 0;
                    while (num++ < Overtime)
                    {
                        if (serialPort.BytesToRead >= ReceiveData.Length)
                        {
                            break;
                        }
                        System.Threading.Thread.Sleep(50);
                    }
                    if (serialPort.BytesToRead >= ReceiveData.Length)
                    {
                        ret = serialPort.Read(ReceiveData, 0, ReceiveData.Length);
                    }
                    else
                    {
                        ret = serialPort.Read(ReceiveData, 0, serialPort.BytesToRead);
                    }
                    ReceiveEventFlag = false;       //打开事件
                    return ret;
                }
                catch (Exception ex)
                {
                    ReceiveEventFlag = false;
                    throw ex;
                }
                finally
                {
                    isComSending = false;
                }
            }
            return -1;
        }

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="SendData">发送数据</param>
        /// <param name="ReceiveData">接收数据</param>
        /// <param name="Overtime">超时时间(单位秒)</param>
        /// <param name="times">失败后重复次数</param>
        /// <returns></returns>
        public int SendCommand(byte[] SendData, out byte[] ReceiveData, int Overtime, int times)
        {
            if (serialPort.IsOpen)
            {

                isComSending = true;
                try
                {

                    //ReceiveEventFlag = true;        //关闭接收事件
                    //serialPort.DiscardInBuffer();   //清空接收缓冲区                 
                    //serialPort.Write(SendData, 0, SendData.Length);

                    //System.Threading.Thread.Sleep(100000);

                    //var waitTsk = Task.Factory.StartNew(() =>
                    //{
                    //    System.Threading.Thread.Sleep(50);
                    //    int num = 0;

                    //    while (isComRecved == false)
                    //    {
                    //        if (num > Overtime * 20)//超时
                    //        {
                    //            break;
                    //        }
                    //        System.Threading.Thread.Sleep(50);
                    //        num++;
                    //    }
                    //});

                    //Task.WaitAll(waitTsk);



                    ReceiveEventFlag = true;        //关闭接收事件
                    serialPort.DiscardInBuffer();   //清空接收缓冲区                 
                    serialPort.Write(SendData, 0, SendData.Length);


                    int num = 0;

                    while (isComRecved == false)
                    {
                        if (num > Overtime * 1000)//超时
                        {
                            break;
                        }
                        System.Threading.Thread.Sleep(1);
                        num++;
                    }


                    #region 根据结束字节来判断是否全部获取完成
                    List<byte> _byteData = new List<byte>();
                    bool found = false;//是否检测到结束符号
                    if (isComRecved)
                    {
                        while (serialPort.BytesToRead > 0 || !found)
                        {
                            byte[] readBuffer = new byte[serialPort.ReadBufferSize + 1];
                            int count = serialPort.Read(readBuffer, 0, serialPort.ReadBufferSize);
                            for (int i = 0; i < count; i++)
                            {
                                _byteData.Add(readBuffer[i]);

                                if (readBuffer[i] == PackageDefine.EndByte && _byteData.Count >= PackageDefine.MinLenth)//可能是结束字符
                                {
                                    int len = BitConverter.ToInt16(new byte[] { _byteData[9], _byteData[8] }, 0);//数据域长度

                                    if (PackageDefine.MinLenth + len == _byteData.Count)//找到结束字符
                                    {
                                        found = true;
                                        goto ac;
                                    }
                                }
                            }

                        }
                    }
                ac:
                    ReceiveData = _byteData.ToArray();

                    #endregion


                    if (ReceiveData.Count() == 0 && times > 0)
                    {
                        times--;
                        return SendCommand(SendData, out ReceiveData, Overtime, times);
                    }
                    return ReceiveData.Count();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    ReceiveEventFlag = false; //打开事件
                    isComSending = false;
                }
            }
            ReceiveData = null;
            return -1;
        }

        public byte[] SendCommand(byte[] SendData, int timeout, int length, ref string error)
        {
            try
            {

                ReceiveEventFlag = true;        //关闭接收事件
                serialPort.DiscardInBuffer();   //清空接收缓冲区                 
                serialPort.Write(SendData, 0, SendData.Length);

                for (int i = 0; i < timeout * 1000; i++)
                {
                    if (serialPort.BytesToRead == length)
                    {
                        byte[] buf = new byte[length];
                        serialPort.Read(buf, 0, length);
                        return buf;
                    }
                    System.Threading.Thread.Sleep(1);
                }
                error = "数据响应超时";
                return null;
            }
            catch (Exception e)
            {
                error = e.Message;
                return null;
            }
        }


        public byte[] SendCommand(byte[] SendData, int timeout, ref string error)
        {
            try
            {
                isComSending = true;
                ReceiveEventFlag = true;        //关闭接收事件
                serialPort.DiscardInBuffer();   //清空接收缓冲区                 
                serialPort.Write(SendData, 0, SendData.Length);

                List<byte> _byteData = new List<byte>();

                for (int i = 0; i < timeout * 1000; i++)
                {
                    while (serialPort.BytesToRead > 0)
                    {
                        int len = serialPort.BytesToRead;
                        byte[] buf = new byte[len];
                        serialPort.Read(buf, 0, len);
                        foreach (var item in buf)
                        {
                            _byteData.Add(item);
                            if (item == PackageDefine.EndByte & _byteData.Count >= PackageDefine.MinLenth)
                            {
                                int datalen = BitConverter.ToInt16(new byte[] { _byteData[9], _byteData[8] }, 0);//数据域长度
                                if (PackageDefine.MinLenth + datalen == _byteData.Count)//找到结束字符
                                {
                                    return _byteData.ToArray();
                                }
                            }
                        }
                    }
                    System.Threading.Thread.Sleep(1);
                }
                error = "数据响应超时";
                return null;
            }
            catch (Exception e)
            {
                error = e.Message;
                return null;
            }
            finally
            {
                isComSending = false;
                ReceiveEventFlag = false;        //关闭接收事件
            }
        }

        ///<summary>
        ///数据发送
        ///</summary>
        ///<param name="data">要发送的数据字符串</param>
        public void SendData(string data)
        {
            //禁止接收事件时直接退出
            if (ReceiveEventFlag || isComClosing)
            {
                return;
            }
            if (serialPort.IsOpen)
            {
                try
                {
                    serialPort.Write(data);
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
        }

        ///<summary>
        ///将指定数量的字节写入输出缓冲区中的指定偏移量处。
        ///</summary>
        ///<param name="data">发送的字节数据</param>
        ///<param name="offset">写入偏移量</param>
        ///<param name="count">写入的字节数</param>
        public void SendData(byte[] data, int offset, int count)
        {
            //禁止接收事件时直接退出
            if (ReceiveEventFlag || isComClosing)
            {
                return;
            }
            if (serialPort.IsOpen)
            {
                try
                {
                    serialPort.Write(data, offset, count);
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
        }


        /// <summary>
        /// 数据接收
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //禁止接收事件时直接退出
            if (ReceiveEventFlag || isComClosing)
            {
                return;
            }

            isComRecving = true;
            try
            {
                #region 根据结束字节来判断是否全部获取完成
                List<byte> _byteData = new List<byte>();
                bool found = false;//是否检测到结束符号
                while (serialPort.BytesToRead > 0 || !found)
                {
                    byte[] readBuffer = new byte[serialPort.ReadBufferSize + 1];
                    int count = serialPort.Read(readBuffer, 0, serialPort.ReadBufferSize);
                    for (int i = 0; i < count; i++)
                    {
                        _byteData.Add(readBuffer[i]);

                        if (readBuffer[i] == PackageDefine.EndByte && _byteData.Count >= PackageDefine.MinLenth)//可能是结束字符
                        {
                            int len = BitConverter.ToInt16(new byte[] { _byteData[9], _byteData[8] }, 0);//数据域长度

                            if (PackageDefine.MinLenth + len == _byteData.Count)//找到结束字符
                            {
                                found = true;
                                goto ac;
                            }
                        }
                    }
                }
                #endregion

            ac:

                if (found && Received != null)
                {
                    Received(sender, new PortDataReciveEventArgs(_byteData.ToArray()));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                isComRecving = false;
            }

        }
        #endregion
    }
}
