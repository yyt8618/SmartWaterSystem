using System;
using Common;
using System.Threading;
using Protocol;
using System.Data;
using Entity;

namespace SmartWaterSystem
{
    /// <summary>
    /// TransStatus
    /// </summary>
    public enum TransStatus
    {
        Start,
        Success,
        Fail
    }

    public enum SerialPortType : uint
    {
        None = 0,
        NoiseWriteTime,  //设置时间
        NoiseWriteRemoteSendTime,//设置远传通信时间
        NoiseWriteStartEndTime,  //设置记录时间段
        NoiseWriteInterval,    //设置采集间隔

        NoiseCtrlStartOrStop,  //设置开关
        NoiseWriteRemoteSwitch, //设置远传功能
        NoiseStart,             //设置启动
        NoiseStop,              //设置停止
        NoiseClearData,         //设置清除数据

        NoiseReadParm,          //读取参数数据
        NoiseReadData,          //读取数据
        NoiseBatchWrite,        //批量设置
        UniversalReset, //通用终端复位
        UniversalSetTime,   //设置通用终端时间
        UniversalSetEnableCollect, //设置通用终端启用采集
        UniversalSetCollectConfig,  //设置通用终端采集配置功能

        UniversalSetSim_Interval,  //设置通用终端模拟量时间间隔
        UniversalSetPluse_Interval, //设置通用终端脉冲量时间间隔
        UniversalSet485_Interval,  //设置通用终端RS485时间间隔
        UnviversalSetModbusProtocol,   //设置通用终端Modbus协议
        UniversalSetModbusExeFlag,     //设置通用终端Modbus执行标志

        UniversalSetBasicInfo,      //通用终端设置基本信息,包括手机号、通信方式、波特率、ip、端口号
        UniversalReadBaicInfo,       //通用终端读取基本信息，包括手机号、通信方式、波特率、ip、端口号
        UniversalCalibrationSimualte1, //通用终端校准第一路模拟量
        UniversalCalibrationSimualte2,  //通用终端校准第二路模拟量
        UniversalPluseBasic,    //通用终端设置脉冲基准数

        UniversalCallData,      //通用终端招测数据(串口)
        OLWQReset, //水质终端复位
        OLWQSetTime,   //设置水质终端时间
        OLWQSetEnableCollect, //设置水质终端启用采集

        //OLWQSetCollectConfig,  //设置水质终端采集配置功能
        //OLWQSetResidualCl_Interval, //设置水质终端余氯时间间隔
        //OLWQSetTurbidity_Interval,  //设置水质终端浊度时间间隔
        //OLWQSetPH_Interval,         //设置水质终端PH时间间隔
        //OLWQSetConductivity_Interval,//设置水质终端电导率时间间隔

        OLWQSetBasicInfo,      //水质终端设置基本信息,包括通信方式、波特率、ip、端口号
        OLWQReadBaicInfo,       //水质终端读取基本信息，包括通信方式、波特率、ip、端口号
        
    }

    public class SerialPortEventArgs : EventArgs
    {
        private SerialPortType _type = SerialPortType.None;
        public SerialPortType OptType
        {
            get { return _type; }
        }

        private TransStatus _transStatus;
        /// <summary>
        /// 传输状态
        /// </summary>
        public TransStatus TransactStatus
        {
            get { return _transStatus; }
        }

        private string _msg = "";
        public string Msg
        {
            get { return _msg; }
        }

        private object _tag = null;
        public object Tag
        {
            get { return _tag; }
        }

        public SerialPortEventArgs(SerialPortType Type, TransStatus status, string msg, object obj)
        {
            this._type = Type;
            this._transStatus = status;
            this._msg = msg;
            this._tag = obj;
        }
    }

    public class SerialPortScheduleEventArgs : EventArgs
    {
        public SerialPortScheduleEventArgs(SerialPortType Type)
        {
            this._type = Type;
        }

        public SerialPortScheduleEventArgs(SerialPortType Type,string msg)
        {
            this._type = Type;
            this._msg = msg;
        }

        private SerialPortType _type = SerialPortType.None;
        public SerialPortType OptType
        {
            get { return _type; }
        }

        private string _msg = "";
        public string Msg
        {
            get { return _msg; }
            set { _msg = value; }
        }
    }

    public delegate void SerialPortHandle(object sender,SerialPortEventArgs e);
    public delegate void SerialPortScheduleHandle(object sender,SerialPortScheduleEventArgs e);
    public class SerialPortManager:SerialPortRW
    {
        private NLog.Logger logger = NLog.LogManager.GetLogger("SerialPortMgr");
        private const int eventcount = 33;
        public event SerialPortHandle SerialPortEvent;
        /// <summary>
        /// 用于通知UI多个通信动作是的进度(读写)
        /// </summary>
        public event SerialPortScheduleHandle SerialPortScheduleEvent;
        private IntPtr[] hEvent = new IntPtr[eventcount];
        public SerialPortManager()
        {
            for (int i = 0; i < eventcount; i++)
            {
                hEvent[i]=Win32.CreateEvent(IntPtr.Zero, false, false, "");
            }
        }

        ~SerialPortManager()
        {
            for (int i = 0; i < eventcount; i++)
            {
                Win32.CloseHandle(hEvent[i]);
            }
        }

        public virtual void OnSerialPortEvent(SerialPortEventArgs e)
        {
            if (SerialPortEvent != null)
                SerialPortEvent(this, e);
        }

        public virtual void OnSerialPortScheduleEvent(SerialPortScheduleEventArgs e)
        {
            if (SerialPortScheduleEvent != null)
                SerialPortScheduleEvent(this, e);
        }

        public void Start()
        {
            Thread t = new Thread(new ThreadStart(SerialPortThread));
            t.IsBackground = true;
            t.Start();
        }

        public void Stop()
        {
            Win32.SetEvent(hEvent[0]);
        }

        public void Send(SerialPortType type)
        {
            Win32.SetEvent(hEvent[(int)type]);
        }

        private void SerialPortThread()
        {
            while (true)
            {
                uint evt = Win32.WaitForMultipleObjects(eventcount, hEvent, false, Win32.INFINITE);
                bool result = false;  //-1:执行失败;1:执行成功;0:无执行返回
                string msg = "";
                object obj = null;

                OnSerialPortEvent(new SerialPortEventArgs((SerialPortType)evt, TransStatus.Start, "", null));
                switch (evt)
                {
                    case (uint)SerialPortType.None:
                        Thread.CurrentThread.Abort();
                        break;
                    case (uint)SerialPortType.NoiseWriteTime:
                        {
                            msg = ""; obj = null;
                            result = GlobalValue.Noiselog.WriteTime(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.dt);
                        }
                        break;
                    case (uint)SerialPortType.NoiseWriteRemoteSendTime:
                        result = GlobalValue.Noiselog.WriteRemoteSendTime(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.ComTime);
                        break;
                    case (uint)SerialPortType.NoiseWriteStartEndTime:
                        result = GlobalValue.Noiselog.WriteStartEndTime(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.colstarttime, GlobalValue.NoiseSerialPortOptData.colendtime);
                        break;
                    case (uint)SerialPortType.NoiseWriteInterval:
                        result = GlobalValue.Noiselog.WriteInterval(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.Interval);
                        break;
                    case (uint)SerialPortType.NoiseWriteRemoteSwitch:
                        #region 噪声记录仪远传设置(开关、ip、port)
                        {
                            try
                            {
                                result = GlobalValue.Noiselog.WriteRemoteSwitch(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.RemoteSwitch);
                                if (!result)
                                    break;
                                if (GlobalValue.NoiseSerialPortOptData.RemoteSwitch)
                                {
                                    NoiseCtrl ctrl = new NoiseCtrl();
                                    result = ctrl.Write_IP(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.IP);
                                    if (!result)
                                        break;
                                    result = ctrl.WritePortName(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.Port.ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                logger.ErrorException("WriteRemoteSwitch", ex);
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.NoiseStart:
                        #region 噪声记录仪开启
                        {
                            try
                            {
                                short[] Originaldata = null;
                                result = GlobalValue.Noiselog.CtrlStartOrStop(GlobalValue.NoiseSerialPortOptData.ID, true, out Originaldata);
                                if (Originaldata == null || (Originaldata != null && (NoiseDataHandler.GetAverage(Originaldata) < 450)))  //没有读到标准值，重试2次
                                {
                                    string startvalue = "";
                                    if (Originaldata != null)
                                        foreach (double d in Originaldata)
                                        {
                                            startvalue += d.ToString() + " ";
                                        }
                                    msg = "启动失败,请重试[" + startvalue + "]!";
                                    result = false;
                                }
                                obj = Originaldata;
                            }
                            catch(Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.NoiseStop:
                        #region 噪声记录仪停止
                        {
                            try
                            {
                                short[] Originaldata = null;
                                result = GlobalValue.Noiselog.CtrlStartOrStop(GlobalValue.NoiseSerialPortOptData.ID, false, out Originaldata);
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.NoiseClearData:
                        #region 噪声记录仪清除数据
                        {
                            try
                            {
                                result = GlobalValue.Noiselog.ClearData(GlobalValue.NoiseSerialPortOptData.ID);
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.NoiseReadParm:
                        #region 噪声记录仪读取参数
                        {
                            try
                            {
                                NoiseSerialPortOptEntity result_data = new NoiseSerialPortOptEntity();
                                result_data.ID = GlobalValue.NoiseSerialPortOptData.ID;
                                // 读取时间
                                OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取时间..."));
                                byte[] tt1 = GlobalValue.Noiselog.ReadTime(GlobalValue.NoiseSerialPortOptData.ID);
                                result_data.dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, tt1[0], tt1[1], tt1[2]);
                                // 读取远传通讯时间
                                OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取远传通讯时间..."));
                                result_data.ComTime = Convert.ToInt32(GlobalValue.Noiselog.ReadRemoteSendTime(GlobalValue.NoiseSerialPortOptData.ID));
                                // 读取记录时间段
                                OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取记录时间段..."));
                                byte[] tt = GlobalValue.Noiselog.ReadStartEndTime(GlobalValue.NoiseSerialPortOptData.ID);
                                result_data.colstarttime = Convert.ToInt32(tt[0]);
                                result_data.colendtime = Convert.ToInt32(tt[1]);

                                // 读取采集间隔
                                OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取采集时间间隔..."));
                                result_data.Interval = GlobalValue.Noiselog.ReadInterval(GlobalValue.NoiseSerialPortOptData.ID);
                                // 读取远传功能
                                OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取远传开关状态..."));
                                result_data.RemoteSwitch = GlobalValue.Noiselog.ReadRemote(GlobalValue.NoiseSerialPortOptData.ID);
                                if (result_data.RemoteSwitch)
                                {
                                    NoiseCtrl ctrl = new NoiseCtrl();

                                    // 读取远传端口
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取远传端口..."));
                                    result_data.Port = Convert.ToInt32(ctrl.ReadPort(GlobalValue.NoiseSerialPortOptData.ID));

                                    // 读取远传地址
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取远传地址..."));
                                    result_data.IP = ctrl.ReadIP(GlobalValue.NoiseSerialPortOptData.ID);
                                }
                                result = true;
                                obj = result_data;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.NoiseReadData:
                        #region 读取噪声记录仪数据
                        {
                            try
                            {
                                short[] arr = GlobalValue.Noiselog.Read(GlobalValue.NoiseSerialPortOptData.ID);
                                result = true;
                                obj = arr;
                            }
                            catch (ArgumentNullException)
                            {
                                result = false;
                                msg = "记录仪" + GlobalValue.NoiseSerialPortOptData.ID + "数据为空！";
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.NoiseBatchWrite:
                        #region 噪声记录仪批量设置参数
                        {
                            try
                            {
                                // 设置时间
                                OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseBatchWrite, "正在设置记录仪[" + GlobalValue.NoiseSerialPortOptData.ID + "]时间..."));
                                result=GlobalValue.Noiselog.WriteTime(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.dt);
                                if (result)
                                {
                                    // 设置远传通讯时间
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseBatchWrite, "正在设置记录仪[" + GlobalValue.NoiseSerialPortOptData.ID + "]远传通讯时间..."));
                                    result = GlobalValue.Noiselog.WriteRemoteSendTime(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.ComTime);
                                }
                                if (result)
                                {
                                    // 设置记录时间段
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseBatchWrite, "正在设置记录仪[" + GlobalValue.NoiseSerialPortOptData.ID + "]记录时间段..."));
                                    result = GlobalValue.Noiselog.WriteStartEndTime(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.colstarttime, GlobalValue.NoiseSerialPortOptData.colendtime);
                                }
                                if (result)
                                {
                                    // 设置采集间隔
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseBatchWrite, "正在设置记录仪[" + GlobalValue.NoiseSerialPortOptData.ID + "]采集间隔..."));
                                    result = GlobalValue.Noiselog.WriteInterval(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.Interval);
                                }
                                if (result)
                                {
                                    // 设置远传功能
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseBatchWrite, "正在设置记录仪[" + GlobalValue.NoiseSerialPortOptData.ID + "]远传功能..."));
                                    result = GlobalValue.Noiselog.WriteRemoteSwitch(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.RemoteSwitch);
                                }
                                if (result)
                                {
                                    // 设置开关
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseBatchWrite, "正在设置记录仪[" + GlobalValue.NoiseSerialPortOptData.ID + "]开关..."));
                                    short[] origitydata = null;
                                    result = GlobalValue.Noiselog.CtrlStartOrStop(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.Enable, out origitydata);
                                }
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.UniversalReset:
                        #region 通用终端复位
                        {
                            try
                            {
                                result = GlobalValue.Universallog.Reset(GlobalValue.SerialPortOptData.ID);
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.UniversalSetTime:
                        #region 设置通用终端时间
                        {
                            try
                            {
                                result = GlobalValue.Universallog.SetTime(GlobalValue.SerialPortOptData.ID, DateTime.Now);
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.UniversalSetEnableCollect:
                        #region 通用终端启用采集
                        {
                            try
                            {
                                result = GlobalValue.Universallog.EnableCollect(GlobalValue.SerialPortOptData.ID);
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.UniversalReadBaicInfo:
                        #region 通用终端读取基础参数
                        {
                            try
                            {
                                if (GlobalValue.SerialPortOptData.IsOptID)  //没有ID时，只读写ID
                                {
                                    GlobalValue.SerialPortOptData.ID = GlobalValue.Universallog.ReadId();
                                }
                                else
                                {
                                    //if (GlobalValue.UniversalSerialPortOptData.ID > 0)
                                    //{
                                        if (GlobalValue.SerialPortOptData.IsOptDT)
                                        {
                                            OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo,"正在读取设备时间..."));
                                            GlobalValue.SerialPortOptData.DT = GlobalValue.Universallog.ReadTime(GlobalValue.SerialPortOptData.ID);
                                        }
                                        if (GlobalValue.SerialPortOptData.IsOptCellPhone)
                                        {
                                            OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在读取报警手机号码..."));
                                            GlobalValue.SerialPortOptData.CellPhone = GlobalValue.Universallog.ReadCellPhone(GlobalValue.SerialPortOptData.ID);
                                        }
                                        //if (GlobalValue.UniversalSerialPortOptData.IsOptmodbusExeFlag)
                                        //{
                                        //    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在读取Modbus执行标识..."));
                                        //    GlobalValue.UniversalSerialPortOptData.ModbusExeFlag = GlobalValue.Universallog.ReadModbusExeFlag(GlobalValue.UniversalSerialPortOptData.ID);
                                        //}
                                        if (GlobalValue.SerialPortOptData.IsOptComType)
                                        {
                                            OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在读取终端通信方式..."));
                                            GlobalValue.SerialPortOptData.ComType = GlobalValue.Universallog.ReadComType(GlobalValue.SerialPortOptData.ID);
                                        }
                                        if (GlobalValue.SerialPortOptData.IsOptIP)
                                        {
                                            OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在读取终端通信IP地址..."));
                                            GlobalValue.SerialPortOptData.IP = GlobalValue.Universallog.ReadIP(GlobalValue.SerialPortOptData.ID);
                                        }
                                        if (GlobalValue.SerialPortOptData.IsOptPort)
                                        {
                                            OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在读取终端通信端口号..."));
                                            GlobalValue.SerialPortOptData.Port = GlobalValue.Universallog.ReadPort(GlobalValue.SerialPortOptData.ID);
                                        }
                                        if (GlobalValue.SerialPortOptData.IsOpt_CollectConfig)
                                        {
                                            OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在读取终端采集配置..."));
                                            byte data = GlobalValue.Universallog.ReadCollectConfig(GlobalValue.SerialPortOptData.ID);
                                            if ((data & 0x02) == 0x02)
                                                GlobalValue.SerialPortOptData.Collect_Pluse = true;
                                            if ((data & 0x04) == 0x04)
                                                GlobalValue.SerialPortOptData.Collect_Simulate1 = true;
                                            if ((data & 0x08) == 0x08)
                                                GlobalValue.SerialPortOptData.Collect_Simulate2 = true;
                                            if ((data & 0x10) == 0x10)
                                                GlobalValue.SerialPortOptData.Collect_RS485 = true;

                                            OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在读取modbus执行标识..."));
                                            GlobalValue.SerialPortOptData.ModbusExeFlag= GlobalValue.Universallog.ReadModbusExeFlag(GlobalValue.SerialPortOptData.ID);
                                        }
                                        if (GlobalValue.SerialPortOptData.IsOpt_SimualteInterval)
                                        {
                                            OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在读取终端模拟量时间间隔..."));
                                            GlobalValue.SerialPortOptData.Simulate_Interval = GlobalValue.Universallog.ReadSimualteInterval(GlobalValue.SerialPortOptData.ID);
                                        }
                                        if (GlobalValue.SerialPortOptData.IsOpt_PluseInterval)
                                        {
                                            OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在读取终端脉冲量时间间隔..."));
                                            GlobalValue.SerialPortOptData.Pluse_Interval = GlobalValue.Universallog.ReadPluseInterval(GlobalValue.SerialPortOptData.ID);
                                        }
                                        if (GlobalValue.SerialPortOptData.IsOpt_RS485Interval)
                                        {
                                            OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在读取终端RS485时间间隔..."));
                                            GlobalValue.SerialPortOptData.RS485_Interval = GlobalValue.Universallog.ReadRS485Interval(GlobalValue.SerialPortOptData.ID);
                                        }
                                        if (GlobalValue.SerialPortOptData.IsOpt_RS485Protocol)
                                        {
                                            OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在读取终端modbus协议..."));
                                            GlobalValue.SerialPortOptData.RS485Protocol = GlobalValue.Universallog.ReadModbusProtocol(GlobalValue.SerialPortOptData.ID);
                                        }
                                    //}
                                }
                                result = true;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.UniversalSetBasicInfo:
                        #region 设置通用终端基础参数
                        {
                            try
                            {
                                if (GlobalValue.SerialPortOptData.IsOptID)  //没有ID时，只读写ID
                                {
                                    result = GlobalValue.Universallog.SetID(GlobalValue.SerialPortOptData.ID);
                                }
                                else
                                {
                                    if (GlobalValue.SerialPortOptData.IsOptCellPhone)  //暂时不使用
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置报警手机号码..."));
                                        result = GlobalValue.Universallog.SetCellPhone(GlobalValue.SerialPortOptData.ID,GlobalValue.SerialPortOptData.CellPhone);
                                    }
                                    //if (GlobalValue.UniversalSerialPortOptData.IsOptmodbusExeFlag)
                                    //{
                                    //    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置Modbus执行标识..."));
                                    //    result = GlobalValue.Universallog.SetModbusExeFlag(GlobalValue.UniversalSerialPortOptData.ID,GlobalValue.UniversalSerialPortOptData.ModbusExeFlag);
                                    //}
                                    if (GlobalValue.SerialPortOptData.IsOptComType)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置终端通信方式..."));
                                        result = GlobalValue.Universallog.SetComType(GlobalValue.SerialPortOptData.ID, GlobalValue.SerialPortOptData.ComType);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOptIP)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置终端通信IP地址..."));
                                        result = GlobalValue.Universallog.SetIP(GlobalValue.SerialPortOptData.ID, GlobalValue.SerialPortOptData.IP);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOptPort)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置终端通信端口号..."));
                                        result = GlobalValue.Universallog.SetPort(GlobalValue.SerialPortOptData.ID,GlobalValue.SerialPortOptData.Port);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOpt_CollectConfig)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置终端采集配置..."));
                                        Int16 data = 0;
                                        if (GlobalValue.SerialPortOptData.Collect_Pluse)
                                            data |= 0x02;
                                        if (GlobalValue.SerialPortOptData.Collect_Simulate1)
                                            data |= 0x04;
                                        if (GlobalValue.SerialPortOptData.Collect_Simulate2)
                                            data |= 0x08;
                                        if (GlobalValue.SerialPortOptData.Collect_RS485)
                                            data |= 0x10;
                                        result=GlobalValue.Universallog.SetCollectConfig(GlobalValue.SerialPortOptData.ID,(byte)data);

                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置Modbus执行标识..."));
                                        result = GlobalValue.Universallog.SetModbusExeFlag(GlobalValue.SerialPortOptData.ID, GlobalValue.SerialPortOptData.ModbusExeFlag);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOpt_SimualteInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置终端模拟量时间间隔..."));
                                        result = GlobalValue.Universallog.SetSimulateInterval(GlobalValue.SerialPortOptData.ID,GlobalValue.SerialPortOptData.Simulate_Interval);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOpt_PluseInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置终端脉冲量时间间隔..."));
                                        result = GlobalValue.Universallog.SetPluseInterval(GlobalValue.SerialPortOptData.ID, GlobalValue.SerialPortOptData.Pluse_Interval);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOpt_RS485Interval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置终端RS485时间间隔..."));
                                        result = GlobalValue.Universallog.SetRS485Interval(GlobalValue.SerialPortOptData.ID, GlobalValue.SerialPortOptData.RS485_Interval);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOpt_RS485Protocol)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置终端modbus协议..."));
                                        result = GlobalValue.Universallog.SetModbusProtocol(GlobalValue.SerialPortOptData.ID, GlobalValue.SerialPortOptData.RS485Protocol);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.UniversalCalibrationSimualte1:
                        #region 校准通用终端第1路模拟量
                        {
                            try
                            {
                                result = GlobalValue.Universallog.CalibartionSimulate1(GlobalValue.SerialPortOptData.ID);
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.UniversalCalibrationSimualte2:
                        #region 校准通用终端第2路模拟量
                        {
                            try
                            {
                                result = GlobalValue.Universallog.CalibartionSimulate2(GlobalValue.SerialPortOptData.ID);
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.UniversalPluseBasic:
                        #region 设置通用终端脉冲基准数
                        {
                            try
                            {
                                if (GlobalValue.SerialPortOptData.SetPluseBasic1)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置脉冲一路基准..."));
                                    result = GlobalValue.Universallog.SetPluseBasic(GlobalValue.SerialPortOptData.ID, GlobalValue.SerialPortOptData.PluseBasic1,1);
                                }
                                if (GlobalValue.SerialPortOptData.SetPluseBasic2)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置脉冲二路基准..."));
                                    result = GlobalValue.Universallog.SetPluseBasic(GlobalValue.SerialPortOptData.ID, GlobalValue.SerialPortOptData.PluseBasic2, 2);
                                }
                                if (GlobalValue.SerialPortOptData.SetPluseBasic3)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置脉冲三路基准..."));
                                    result = GlobalValue.Universallog.SetPluseBasic(GlobalValue.SerialPortOptData.ID, GlobalValue.SerialPortOptData.PluseBasic3, 3);
                                }
                                if (GlobalValue.SerialPortOptData.SetPluseBasic4)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置脉冲四路基准..."));
                                    result = GlobalValue.Universallog.SetPluseBasic(GlobalValue.SerialPortOptData.ID, GlobalValue.SerialPortOptData.PluseBasic4, 4);
                                }
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        break;
                        #endregion
                    case (uint)SerialPortType.UniversalCallData:
                        #region 读取通用终端招测数据
                        {
                            try
                            {
                                if (GlobalValue.SerialPortOptData == null || GlobalValue.SerialPortCallDataType == null)
                                {
                                    result = false;
                                    msg = "终端没有配置采集类型,请先配置!";
                                }
                                else
                                {
                                    DataTable dt_config = (new BLL.TerminalDataBLL()).GetUniversalDataConfig(Entity.TerType.UniversalTer);
                                    if (dt_config != null && dt_config.Rows.Count > 0)
                                    {
                                        obj = GlobalValue.Universallog.ReadCallData(GlobalValue.SerialPortOptData.ID, dt_config, GlobalValue.SerialPortCallDataType);
                                        result = true;
                                    }
                                    else
                                    {
                                        result = false;
                                        msg = "终端没有配置采集参数";
                                    }
                                }
                                
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        break;
                        #endregion
                    case (uint)SerialPortType.OLWQReset:
                        #region 水质终端复位
                        {
                            try
                            {
                                result = GlobalValue.OLWQlog.Reset(GlobalValue.SerialPortOptData.ID);
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.OLWQSetTime:
                        #region 设置水质终端时间
                        {
                            try
                            {
                                result = GlobalValue.OLWQlog.SetTime(GlobalValue.SerialPortOptData.ID, DateTime.Now);
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.OLWQSetEnableCollect:
                        #region 水质终端启用采集
                        {
                            try
                            {
                                result = GlobalValue.OLWQlog.EnableCollect(GlobalValue.SerialPortOptData.ID);
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.OLWQReadBaicInfo:
                        #region 水质终端读取基础参数
                        {
                            try
                            {
                                if (GlobalValue.SerialPortOptData.IsOptID)  //没有ID时，只读写ID
                                {
                                    GlobalValue.SerialPortOptData.ID = GlobalValue.OLWQlog.ReadId();
                                }
                                else
                                {
                                    if (GlobalValue.SerialPortOptData.IsOptDT)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取设备时间..."));
                                        GlobalValue.SerialPortOptData.DT = GlobalValue.OLWQlog.ReadTime(GlobalValue.SerialPortOptData.ID);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOptIP)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端通信IP地址..."));
                                        GlobalValue.SerialPortOptData.IP = GlobalValue.OLWQlog.ReadIP(GlobalValue.SerialPortOptData.ID);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOptPort)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端通信端口号..."));
                                        GlobalValue.SerialPortOptData.Port = GlobalValue.OLWQlog.ReadPort(GlobalValue.SerialPortOptData.ID);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOptResidualClLowLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端余氯下限值..."));
                                        GlobalValue.SerialPortOptData.ResidualClLowLimit = GlobalValue.OLWQlog.ReadResidualClLowLimit(GlobalValue.SerialPortOptData.ID);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOptResidualClZero)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端余氯零点..."));
                                        GlobalValue.SerialPortOptData.ResidualClZero = GlobalValue.OLWQlog.ReadResidualClZero(GlobalValue.SerialPortOptData.ID);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOptResidualClStandValue)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端余氯校准值..."));
                                        GlobalValue.SerialPortOptData.ResidualClStandValue = GlobalValue.OLWQlog.ReadResidualClStandValue(GlobalValue.SerialPortOptData.ID);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOptResidualClSensitivity)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端余氯灵敏度..."));
                                        GlobalValue.SerialPortOptData.ResidualClSensitivity = GlobalValue.OLWQlog.ReadResidualClSensitivity(GlobalValue.SerialPortOptData.ID);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOptClearInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端清洗间隔..."));
                                        GlobalValue.SerialPortOptData.ClearInterval = GlobalValue.OLWQlog.ReadClearInterval(GlobalValue.SerialPortOptData.ID);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOptPowerSupplyType)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端供电方式..."));
                                        GlobalValue.SerialPortOptData.PowerSupplyType = GlobalValue.OLWQlog.ReadPowerSupplyType(GlobalValue.SerialPortOptData.ID);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOpt_CollectConfig)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端采集配置..."));
                                        byte data = GlobalValue.OLWQlog.ReadCollectConfig(GlobalValue.SerialPortOptData.ID);
                                        if ((data & 0x01) == 0x01)
                                            GlobalValue.SerialPortOptData.Collect_Turbidity = true;
                                        if ((data & 0x02) == 0x02)
                                            GlobalValue.SerialPortOptData.Collect_ResidualC1 = true;
                                        if ((data & 0x04) == 0x04)
                                            GlobalValue.SerialPortOptData.Collect_PH = true;
                                        if ((data & 0x08) == 0x08)
                                            GlobalValue.SerialPortOptData.Collect_Conductivity = true;
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOpt_ResidualClInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端余氯时间间隔..."));
                                        GlobalValue.SerialPortOptData.ResidualCl_Interval = GlobalValue.OLWQlog.ReadResidualClInterval(GlobalValue.SerialPortOptData.ID);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOpt_TurbidityInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端浊度时间间隔..."));
                                        GlobalValue.SerialPortOptData.Turbidity_Interval = GlobalValue.OLWQlog.ReadTurbidityInterval(GlobalValue.SerialPortOptData.ID);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOpt_PHInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端PH时间间隔..."));
                                        GlobalValue.SerialPortOptData.PH_Interval = GlobalValue.OLWQlog.ReadPHInterval(GlobalValue.SerialPortOptData.ID);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOpt_ConductivityInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端电导率时间间隔..."));
                                        GlobalValue.SerialPortOptData.Conductivity_Interval = GlobalValue.OLWQlog.ReadConductivityInterval(GlobalValue.SerialPortOptData.ID);
                                    }
                                }
                                result = true;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.OLWQSetBasicInfo:
                        #region 设置水质终端基础参数
                        {
                            try
                            {
                                if (GlobalValue.SerialPortOptData.IsOptID)  //没有ID时，只读写ID
                                {
                                    result = GlobalValue.OLWQlog.SetID(GlobalValue.SerialPortOptData.ID);
                                }
                                else
                                {
                                    if (GlobalValue.SerialPortOptData.IsOptIP)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端通信IP地址..."));
                                        result = GlobalValue.OLWQlog.SetIP(GlobalValue.SerialPortOptData.ID, GlobalValue.SerialPortOptData.IP);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOptPort)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端通信端口号..."));
                                        result = GlobalValue.OLWQlog.SetPort(GlobalValue.SerialPortOptData.ID, GlobalValue.SerialPortOptData.Port);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOptResidualClLowLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置余氯下限值..."));
                                        result = GlobalValue.OLWQlog.SetResidualClLowLimit(GlobalValue.SerialPortOptData.ID, GlobalValue.SerialPortOptData.ResidualClLowLimit);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOptResidualClZero)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端零点值..."));
                                        result = GlobalValue.OLWQlog.SetResidualClZero(GlobalValue.SerialPortOptData.ID, GlobalValue.SerialPortOptData.ResidualClZero);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOptResidualClStandValue)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端校准值..."));
                                        result = GlobalValue.OLWQlog.SetResidualClStandValue(GlobalValue.SerialPortOptData.ID, GlobalValue.SerialPortOptData.ResidualClStandValue);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOptResidualClSensitivity)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端灵敏度..."));
                                        result = GlobalValue.OLWQlog.SetResidualClSensitivity(GlobalValue.SerialPortOptData.ID, GlobalValue.SerialPortOptData.ResidualClSensitivity);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOptClearInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端清洗间隔..."));
                                        result = GlobalValue.OLWQlog.SetClearInterval(GlobalValue.SerialPortOptData.ID, GlobalValue.SerialPortOptData.ClearInterval);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOptTurbidityUpLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端浊度上限..."));
                                        result = GlobalValue.OLWQlog.SetTurbidityUpLimit(GlobalValue.SerialPortOptData.ID, GlobalValue.SerialPortOptData.TurbidityUpLimit);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOptPowerSupplyType)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端供电方式..."));
                                        result = GlobalValue.OLWQlog.SetPowerSupplyType(GlobalValue.SerialPortOptData.ID, GlobalValue.SerialPortOptData.PowerSupplyType);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOpt_CollectConfig)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端采集配置..."));
                                        Int16 data = 0;
                                        if (GlobalValue.SerialPortOptData.Collect_Turbidity)
                                            data |= 0x01;
                                        if (GlobalValue.SerialPortOptData.Collect_ResidualC1)
                                            data |= 0x02;
                                        if (GlobalValue.SerialPortOptData.Collect_PH)
                                            data |= 0x04;
                                        if (GlobalValue.SerialPortOptData.Collect_Conductivity)
                                            data |= 0x08;
                                        result = GlobalValue.OLWQlog.SetCollectConfig(GlobalValue.SerialPortOptData.ID, (byte)data);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOpt_ResidualClInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端余氯采集时间间隔..."));
                                        result = GlobalValue.OLWQlog.SetResidualClInterval(GlobalValue.SerialPortOptData.ID, GlobalValue.SerialPortOptData.ResidualCl_Interval);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOpt_TurbidityInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端浊度采集时间间隔..."));
                                        result = GlobalValue.OLWQlog.SetTurbidityInterval(GlobalValue.SerialPortOptData.ID, GlobalValue.SerialPortOptData.Turbidity_Interval);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOpt_PHInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端PH采集时间间隔..."));
                                        result = GlobalValue.OLWQlog.SetPHInterval(GlobalValue.SerialPortOptData.ID, GlobalValue.SerialPortOptData.PH_Interval);
                                    }
                                    if (GlobalValue.SerialPortOptData.IsOpt_ConductivityInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端电导率采集时间间隔..."));
                                        result = GlobalValue.OLWQlog.SetConductivityInterval(GlobalValue.SerialPortOptData.ID, GlobalValue.SerialPortOptData.Conductivity_Interval);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                }
                OnSerialPortEvent(new SerialPortEventArgs((SerialPortType)evt, result ? TransStatus.Success : TransStatus.Fail, msg, obj));
            }
        }
    }
}
