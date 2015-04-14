using System;
using Common;
using System.Threading;
using Protocol;

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
        UniversalReset, //通用终端复位
        UniversalSetTime,   //设置通用终端时间
        UniversalSetEnableCollect, //设置通用终端启用采集

        UniversalSetCollectConfig,  //设置通用终端采集配置功能
        UniversalSetSim_Interval,  //设置通用终端模拟量时间间隔
        UniversalSetPluse_Interval, //设置通用终端脉冲量时间间隔
        UnviersalSet485_Interval,  //设置通用终端RS485时间间隔
        UnviversalSetModbusProtocol,   //设置通用终端Modbus协议

        UniversalSetModbusExeFlag,     //设置通用终端Modbus执行标志
        UniversalSetBasicInfo,      //通用终端设置基本信息,包括手机号、通信方式、波特率、ip、端口号
        UniversalReadBaicInfo,       //通用终端读取基本信息，包括手机号、通信方式、波特率、ip、端口号
        UniversalCalibrationSimualte1, //通用终端校准第一路模拟量
        UniversalCalibrationSimualte2,  //通用终端校准第二路模拟量
        
        UniversalPluseBasic,    //通用终端设置脉冲基准数
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
        private const int eventcount = 21;
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
            if (SerialPortEvent != null)
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
                                NoiseCtrl ctrl = new NoiseCtrl();
                                result = ctrl.Write_IP(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.IP);
                                if (!result)
                                    break;
                                result = ctrl.WritePortName(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.Port.ToString());
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
                    case (uint)SerialPortType.UniversalReset:
                        #region 通用终端复位
                        {
                            try
                            {
                                result = GlobalValue.Universallog.Reset(GlobalValue.UniversalSerialPortOptData.ID);
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
                                result = GlobalValue.Universallog.SetTime(GlobalValue.UniversalSerialPortOptData.ID, DateTime.Now);
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
                                result = GlobalValue.Universallog.EnableCollect(GlobalValue.UniversalSerialPortOptData.ID);
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
                                if (GlobalValue.UniversalSerialPortOptData.IsOptID)  //没有ID时，只读写ID
                                {
                                    GlobalValue.UniversalSerialPortOptData.ID = GlobalValue.Universallog.ReadId();
                                }
                                else
                                {
                                    //if (GlobalValue.UniversalSerialPortOptData.ID > 0)
                                    //{
                                        if (GlobalValue.UniversalSerialPortOptData.IsOptDT)
                                        {
                                            OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo,"正在读取设备时间..."));
                                            GlobalValue.UniversalSerialPortOptData.DT = GlobalValue.Universallog.ReadTime(GlobalValue.UniversalSerialPortOptData.ID);
                                        }
                                        if (GlobalValue.UniversalSerialPortOptData.IsOptCellPhone)
                                        {
                                            OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在读取报警手机号码..."));
                                            GlobalValue.UniversalSerialPortOptData.CellPhone = GlobalValue.Universallog.ReadCellPhone(GlobalValue.UniversalSerialPortOptData.ID);
                                        }
                                        //if (GlobalValue.UniversalSerialPortOptData.IsOptmodbusExeFlag)
                                        //{
                                        //    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在读取Modbus执行标识..."));
                                        //    GlobalValue.UniversalSerialPortOptData.ModbusExeFlag = GlobalValue.Universallog.ReadModbusExeFlag(GlobalValue.UniversalSerialPortOptData.ID);
                                        //}
                                        if (GlobalValue.UniversalSerialPortOptData.IsOptComType)
                                        {
                                            OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在读取终端通信方式..."));
                                            GlobalValue.UniversalSerialPortOptData.ComType = GlobalValue.Universallog.ReadComType(GlobalValue.UniversalSerialPortOptData.ID);
                                        }
                                        if (GlobalValue.UniversalSerialPortOptData.IsOptIP)
                                        {
                                            OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在读取终端通信IP地址..."));
                                            GlobalValue.UniversalSerialPortOptData.IP = GlobalValue.Universallog.ReadIP(GlobalValue.UniversalSerialPortOptData.ID);
                                        }
                                        if (GlobalValue.UniversalSerialPortOptData.IsOptPort)
                                        {
                                            OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在读取终端通信端口号..."));
                                            GlobalValue.UniversalSerialPortOptData.Port = GlobalValue.Universallog.ReadPort(GlobalValue.UniversalSerialPortOptData.ID);
                                        }
                                        if (GlobalValue.UniversalSerialPortOptData.IsOpt_CollectConfig)
                                        {
                                            OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在读取终端采集配置..."));
                                            byte data = GlobalValue.Universallog.ReadCollectConfig(GlobalValue.UniversalSerialPortOptData.ID);
                                            if ((data & 0x02) == 0x02)
                                                GlobalValue.UniversalSerialPortOptData.Collect_Pluse = true;
                                            if ((data & 0x04) == 0x04)
                                                GlobalValue.UniversalSerialPortOptData.Collect_Simulate1 = true;
                                            if ((data & 0x08) == 0x08)
                                                GlobalValue.UniversalSerialPortOptData.Collect_Simulate2 = true;
                                            if ((data & 0x10) == 0x10)
                                                GlobalValue.UniversalSerialPortOptData.Collect_RS485 = true;

                                            OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在读取modbus执行标识..."));
                                            GlobalValue.UniversalSerialPortOptData.ModbusExeFlag= GlobalValue.Universallog.ReadModbusExeFlag(GlobalValue.UniversalSerialPortOptData.ID);
                                        }
                                        if (GlobalValue.UniversalSerialPortOptData.IsOpt_SimualteInterval)
                                        {
                                            OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在读取终端模拟量时间间隔..."));
                                            GlobalValue.UniversalSerialPortOptData.Simulate_Interval = GlobalValue.Universallog.ReadSimualteInterval(GlobalValue.UniversalSerialPortOptData.ID);
                                        }
                                        if (GlobalValue.UniversalSerialPortOptData.IsOpt_PluseInterval)
                                        {
                                            OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在读取终端脉冲量时间间隔..."));
                                            GlobalValue.UniversalSerialPortOptData.Pluse_Interval = GlobalValue.Universallog.ReadPluseInterval(GlobalValue.UniversalSerialPortOptData.ID);
                                        }
                                        if (GlobalValue.UniversalSerialPortOptData.IsOpt_RS485Interval)
                                        {
                                            OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在读取终端RS485时间间隔..."));
                                            GlobalValue.UniversalSerialPortOptData.RS485_Interval = GlobalValue.Universallog.ReadRS485Interval(GlobalValue.UniversalSerialPortOptData.ID);
                                        }
                                        if (GlobalValue.UniversalSerialPortOptData.IsOpt_RS485Protocol)
                                        {
                                            OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在读取终端modbus协议..."));
                                            GlobalValue.UniversalSerialPortOptData.RS485Protocol = GlobalValue.Universallog.ReadModbusProtocol(GlobalValue.UniversalSerialPortOptData.ID);
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
                                if (GlobalValue.UniversalSerialPortOptData.IsOptID)  //没有ID时，只读写ID
                                {
                                    result = GlobalValue.Universallog.SetID(GlobalValue.UniversalSerialPortOptData.ID);
                                }
                                else
                                {
                                    if (GlobalValue.UniversalSerialPortOptData.IsOptCellPhone)  //暂时不使用
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置报警手机号码..."));
                                        result = GlobalValue.Universallog.SetCellPhone(GlobalValue.UniversalSerialPortOptData.ID,GlobalValue.UniversalSerialPortOptData.CellPhone);
                                    }
                                    //if (GlobalValue.UniversalSerialPortOptData.IsOptmodbusExeFlag)
                                    //{
                                    //    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置Modbus执行标识..."));
                                    //    result = GlobalValue.Universallog.SetModbusExeFlag(GlobalValue.UniversalSerialPortOptData.ID,GlobalValue.UniversalSerialPortOptData.ModbusExeFlag);
                                    //}
                                    if (GlobalValue.UniversalSerialPortOptData.IsOptComType)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置终端通信方式..."));
                                        result = GlobalValue.Universallog.SetComType(GlobalValue.UniversalSerialPortOptData.ID, GlobalValue.UniversalSerialPortOptData.ComType);
                                    }
                                    if (GlobalValue.UniversalSerialPortOptData.IsOptIP)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置终端通信IP地址..."));
                                        result = GlobalValue.Universallog.SetIP(GlobalValue.UniversalSerialPortOptData.ID, GlobalValue.UniversalSerialPortOptData.IP);
                                    }
                                    if (GlobalValue.UniversalSerialPortOptData.IsOptPort)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置终端通信端口号..."));
                                        result = GlobalValue.Universallog.SetPort(GlobalValue.UniversalSerialPortOptData.ID,GlobalValue.UniversalSerialPortOptData.Port);
                                    }
                                    if (GlobalValue.UniversalSerialPortOptData.IsOpt_CollectConfig)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置终端采集配置..."));
                                        Int16 data = 0;
                                        if (GlobalValue.UniversalSerialPortOptData.Collect_Pluse)
                                            data |= 0x02;
                                        if (GlobalValue.UniversalSerialPortOptData.Collect_Simulate1)
                                            data |= 0x04;
                                        if (GlobalValue.UniversalSerialPortOptData.Collect_Simulate2)
                                            data |= 0x08;
                                        if (GlobalValue.UniversalSerialPortOptData.Collect_RS485)
                                            data |= 0x10;
                                        result=GlobalValue.Universallog.SetCollectConfig(GlobalValue.UniversalSerialPortOptData.ID,(byte)data);

                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置Modbus执行标识..."));
                                        result = GlobalValue.Universallog.SetModbusExeFlag(GlobalValue.UniversalSerialPortOptData.ID, GlobalValue.UniversalSerialPortOptData.ModbusExeFlag);
                                    }
                                    if (GlobalValue.UniversalSerialPortOptData.IsOpt_SimualteInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置终端模拟量时间间隔..."));
                                        result = GlobalValue.Universallog.SetSimulateInterval(GlobalValue.UniversalSerialPortOptData.ID,GlobalValue.UniversalSerialPortOptData.Simulate_Interval);
                                    }
                                    if (GlobalValue.UniversalSerialPortOptData.IsOpt_PluseInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置终端脉冲量时间间隔..."));
                                        result = GlobalValue.Universallog.SetPluseInterval(GlobalValue.UniversalSerialPortOptData.ID, GlobalValue.UniversalSerialPortOptData.Pluse_Interval);
                                    }
                                    if (GlobalValue.UniversalSerialPortOptData.IsOpt_RS485Interval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置终端RS485时间间隔..."));
                                        result = GlobalValue.Universallog.SetRS485Interval(GlobalValue.UniversalSerialPortOptData.ID, GlobalValue.UniversalSerialPortOptData.RS485_Interval);
                                    }
                                    if (GlobalValue.UniversalSerialPortOptData.IsOpt_RS485Protocol)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置终端modbus协议..."));
                                        result = GlobalValue.Universallog.SetModbusProtocol(GlobalValue.UniversalSerialPortOptData.ID, GlobalValue.UniversalSerialPortOptData.RS485Protocol);
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
                                result = GlobalValue.Universallog.CalibartionSimulate1(GlobalValue.UniversalSerialPortOptData.ID);
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
                                result = GlobalValue.Universallog.CalibartionSimulate2(GlobalValue.UniversalSerialPortOptData.ID);
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
                                if (GlobalValue.UniversalSerialPortOptData.SetPluseBasic1)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置脉冲一路基准..."));
                                    result = GlobalValue.Universallog.SetPluseBasic(GlobalValue.UniversalSerialPortOptData.ID, GlobalValue.UniversalSerialPortOptData.PluseBasic1,1);
                                }
                                if (GlobalValue.UniversalSerialPortOptData.SetPluseBasic2)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置脉冲二路基准..."));
                                    result = GlobalValue.Universallog.SetPluseBasic(GlobalValue.UniversalSerialPortOptData.ID, GlobalValue.UniversalSerialPortOptData.PluseBasic2, 2);
                                }
                                if (GlobalValue.UniversalSerialPortOptData.SetPluseBasic3)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置脉冲三路基准..."));
                                    result = GlobalValue.Universallog.SetPluseBasic(GlobalValue.UniversalSerialPortOptData.ID, GlobalValue.UniversalSerialPortOptData.PluseBasic3, 3);
                                }
                                if (GlobalValue.UniversalSerialPortOptData.SetPluseBasic4)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBaicInfo, "正在设置脉冲四路基准..."));
                                    result = GlobalValue.Universallog.SetPluseBasic(GlobalValue.UniversalSerialPortOptData.ID, GlobalValue.UniversalSerialPortOptData.PluseBasic4, 4);
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
                }
                OnSerialPortEvent(new SerialPortEventArgs((SerialPortType)evt, result ? TransStatus.Success : TransStatus.Fail, msg, obj));
            }
        }
    }
}
