using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Entity
{
    public class UniversalSerialPortOptEntity
    {
        /// <summary>
        /// 是否读取/设置设备ID
        /// </summary>
        public bool IsOptID = false;
        private short _id;
        /// <summary>
        /// 设备编号
        /// </summary>
        public short ID
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 是否读取设备时间
        /// </summary>
        public bool IsOptDT = false;
        private DateTime _dt;
        /// <summary>
        /// 设备时间
        /// </summary>
        public DateTime DT
        {
            get { return _dt; }
            set { _dt = value; }
        }

        /// <summary>
        /// 是否读取/设置手机号码
        /// </summary>
        public bool IsOptCellPhone = false;
        private string _cellphone;
        /// <summary>
        /// 手机号码
        /// </summary>
        public string CellPhone
        {
            get { return _cellphone; }
            set { _cellphone = value; }
        }
        
        private bool _Collect_DigitPre;
        /// <summary>
        /// 数字量压力
        /// </summary>
        public bool Collect_DigitPre
        {
            get { return _Collect_DigitPre; }
            set { _Collect_DigitPre = value; }
        }

        /// <summary>
        /// 是否读取/设置modbus执行标识
        /// </summary>
        public bool IsOptModbusExeFlag = false;
        private bool _ModbusExeFlag;
        /// <summary>
        /// modbus执行标识
        /// </summary>
        public bool ModbusExeFlag
        {
            get { return _ModbusExeFlag; }
            set { _ModbusExeFlag = value; }
        }

        /// <summary>
        /// 是否读取/设置通讯方式
        /// </summary>
        public bool IsOptComType = false;
        private int _comType;
        /// <summary>
        /// 通信方式(1:GSM,2:GPRS)
        /// </summary>
        public int ComType
        {
            get { return _comType; }
            set { _comType = value; }
        }

        /// <summary>
        /// 是否读取/设置IP
        /// </summary>
        public bool IsOptIP = false;
        private int[] _ip;
        /// <summary>
        /// IP地址
        /// </summary>
        public int[] IP
        {
            get { return _ip; }
            set { _ip = value; }
        }

        /// <summary>
        /// 是否读取/设置端口号
        /// </summary>
        public bool IsOptPort = false;
        private int _port;
        /// <summary>
        /// 端口号
        /// </summary>
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        public bool IsOptResidualClLowLimit = false;
        private ushort _ResidualClLowLimit = 0;
        /// <summary>
        /// 余氯下限
        /// </summary>
        public ushort ResidualClLowLimit
        {
            get { return _ResidualClLowLimit; }
            set { _ResidualClLowLimit = value; }
        }

        public bool IsOptResidualClZero = false;
        private ushort _ResidualClZero = 0;
        /// <summary>
        /// 余氯零点值
        /// </summary>
        public ushort ResidualClZero
        {
            get { return _ResidualClZero; }
            set { _ResidualClZero = value; }
        }

        public bool IsOptResidualClStandValue = false;
        private ushort _ResidualClStandValue = 0;
        /// <summary>
        /// 余氯标准值
        /// </summary>
        public ushort ResidualClStandValue
        {
            get { return _ResidualClStandValue; }
            set { _ResidualClStandValue = value; }
        }

        public bool IsOptResidualClSensitivity = false;
        private ushort _ResidualClSensitivity = 0;
        /// <summary>
        /// 余氯灵敏度
        /// </summary>
        public ushort ResidualClSensitivity
        {
            get { return _ResidualClSensitivity; }
            set { _ResidualClSensitivity = value; }
        }

        public bool IsOptTerAddr = false;
        private byte[] _TerAddr = new byte[5];
        /// <summary>
        /// 终端地址
        /// </summary>
        public byte[] TerAddr
        {
            get { return _TerAddr; }
            set { _TerAddr = value; }
        }

        public bool IsOptCenterAddr = false;
        private byte _CenterAddr;
        /// <summary>
        /// 中心站地址
        /// </summary>
        public byte CenterAddr
        {
            get { return _CenterAddr; }
            set { _CenterAddr = value; }
        }

        public bool IsOptPwd = false;
        private byte[] _Pwd = new byte[2];
        /// <summary>
        /// 密码
        /// </summary>
        public byte[] Pwd
        {
            get { return _Pwd; }
            set { _Pwd = value; }
        }

        public bool IsOptWorkType = false;
        private int _WorkType = 0;
        /// <summary>
        /// 工作方式
        /// </summary>
        public int WorkType
        {
            get { return _WorkType; }
            set { _WorkType = value; }
        }

        public bool IsOptGprsSwitch = false;
        private bool _GprsSwitch = false;
        /// <summary>
        /// GPRS开关
        /// </summary>
        public bool GprsSwitch
        {
            get { return _GprsSwitch; }
            set { _GprsSwitch = value; }
        }

        public bool IsOptClearInterval = false;
        private ushort _ClearInterval = 0;
        /// <summary>
        /// 清洗间隔
        /// </summary>
        public ushort ClearInterval
        {
            get { return _ClearInterval; }
            set { _ClearInterval = value; }
        }

        public bool IsOptDataInterval = false;
        private ushort _DataInterval = 0;
        /// <summary>
        /// 数据加报时间间隔
        /// </summary>
        public ushort DataInterval
        {
            get { return _DataInterval; }
            set { _DataInterval = value; }
        }

        public bool IsOptTempUpLimit = false;
        private ushort _TempUpLimit = 0;
        /// <summary>
        /// 温度上限
        /// </summary>
        public ushort TempUpLimit
        {
            get { return _TempUpLimit; }
            set { _TempUpLimit = value; }
        }

        public bool IsOptTempLowLimit = false;
        private ushort _TempLowLimit = 0;
        /// <summary>
        /// 温度下限
        /// </summary>
        public ushort TempLowLimit
        {
            get { return _TempLowLimit; }
            set { _TempLowLimit = value; }
        }

        public bool IsOptTempAddtion = false;
        private ushort _TempAddtion = 0;
        /// <summary>
        /// 温度加报阀值
        /// </summary>
        public ushort TempAddtion
        {
            get { return _TempAddtion; }
            set { _TempAddtion = value; }
        }

        public bool IsOptPHUpLimit = false;
        private ushort _PHUpLimit = 0;
        /// <summary>
        /// PH上限
        /// </summary>
        public ushort PHUpLimit
        {
            get { return _PHUpLimit; }
            set { _PHUpLimit = value; }
        }

        public bool IsOptPHLowLimit = false;
        private ushort _PHLowLimit = 0;
        /// <summary>
        /// PH下限
        /// </summary>
        public ushort PHLowLimit
        {
            get { return _PHLowLimit; }
            set { _PHLowLimit = value; }
        }

        public bool IsOptConductivityUpLimit = false;
        private ushort _ConductivityUpLimit = 0;
        /// <summary>
        /// 电导率上限
        /// </summary>
        public ushort ConductivityUpLimit
        {
            get { return _ConductivityUpLimit; }
            set { _ConductivityUpLimit = value; }
        }

        public bool IsOptConductivityLowLimit = false;
        private ushort _ConductivityLowLimit = 0;
        /// <summary>
        /// 电导率下限
        /// </summary>
        public ushort ConductivityLowLimit
        {
            get { return _ConductivityLowLimit; }
            set { _ConductivityLowLimit = value; }
        }

        public bool IsOptTurbidityUpLimit = false;
        private ushort _TurbidityUpLimit = 0;
        /// <summary>
        /// 浊度上限
        /// </summary>
        public ushort TurbidityUpLimit
        {
            get { return _TurbidityUpLimit; }
            set { _TurbidityUpLimit = value; }
        }

        public bool IsOptTurbidityLowLimit = false;
        private ushort _TurbidityLowLimit = 0;
        /// <summary>
        /// 浊度下限
        /// </summary>
        public ushort TurbidityLowLimit
        {
            get { return _TurbidityLowLimit; }
            set { _TurbidityLowLimit = value; }
        }

        public bool IsOptPowerSupplyType = false;
        private ushort _PowerSupplyType = 0;
        /// <summary>
        /// 供电方式
        /// </summary>
        public ushort PowerSupplyType
        {
            get { return _PowerSupplyType; }
            set { _PowerSupplyType = value; }
        }

        /// <summary>
        /// 是否读取/设置采集配置
        /// </summary>
        public bool IsOpt_CollectConfig = false;

        private bool _collect_ResidualC1;
        /// <summary>
        /// 是否采集余氯
        /// </summary>
        public bool Collect_ResidualC1
        {
            get { return _collect_ResidualC1; }
            set { _collect_ResidualC1 = value; }
        }

        private bool _collect_Turbidity;
        /// <summary>
        /// 是否采集浊度
        /// </summary>
        public bool Collect_Turbidity
        {
            get { return _collect_Turbidity; }
            set { _collect_Turbidity = value; }
        }

        private bool _collect_PH;
        /// <summary>
        /// 是否采集PH值
        /// </summary>
        public bool Collect_PH
        {
            get { return _collect_PH; }
            set { _collect_PH = value; }
        }

        private bool _collect_Conductivity;
        /// <summary>
        /// 是否采集电导率
        /// </summary>
        public bool Collect_Conductivity
        {
            get { return _collect_Conductivity; }
            set { _collect_Conductivity = value; }
        }

        private bool _collect_simulate1;
        /// <summary>
        /// 是否采集模拟量1路
        /// </summary>
        public bool Collect_Simulate1
        {
            get { return _collect_simulate1; }
            set { _collect_simulate1 = value; }
        }

        private bool _collect_simulate2;
        /// <summary>
        /// 是否采集模拟量2路
        /// </summary>
        public bool Collect_Simulate2
        {
            get { return _collect_simulate2; }
            set { _collect_simulate2 = value; }
        }

        private bool _collect_pluse;
        /// <summary>
        /// 是否采集脉冲量
        /// </summary>
        public bool Collect_Pluse
        {
            get { return _collect_pluse; }
            set { _collect_pluse = value; }
        }

        private bool _collect_RS485;
        /// <summary>
        /// 是否采集RS485
        /// </summary>
        public bool Collect_RS485
        {
            get { return _collect_RS485; }
            set { _collect_RS485 = value; }
        }

        /// <summary>
        /// 是否读取/设置模拟量时间间隔
        /// </summary>
        public bool IsOpt_SimualteInterval = false;
        private DataTable _Simulate_Interval;
        /// <summary>
        /// 模拟量时间间隔
        /// </summary>
        public DataTable Simulate_Interval
        {
            get { return _Simulate_Interval; }
            set { _Simulate_Interval = value; }
        }

        /// <summary>
        /// 是否读取/设置脉冲量时间间隔
        /// </summary>
        public bool IsOpt_PluseInterval = false;
        private DataTable _Pluse_Interval;
        /// <summary>
        /// 脉冲量时间间隔
        /// </summary>
        public DataTable Pluse_Interval
        {
            get { return _Pluse_Interval; }
            set { _Pluse_Interval = value; }
        }

        /// <summary>
        /// 是否读取/设置RS485时间间隔
        /// </summary>
        public bool IsOpt_RS485Interval = false;
        private DataTable _RS485_Interval;
        /// <summary>
        /// RS485时间间隔
        /// </summary>
        public DataTable RS485_Interval
        {
            get { return _RS485_Interval; }
            set { _RS485_Interval = value; }
        }

        /// <summary>
        /// 是否读取/设置modbus协议
        /// </summary>
        public bool IsOpt_RS485Protocol = false;
        private DataTable _RS485Protocol;
        /// <summary>
        /// RS485采集MODBUS协议配置
        /// </summary>
        public DataTable RS485Protocol
        {
            get { return _RS485Protocol; }
            set { _RS485Protocol = value; }
        }

        /// <summary>
        /// 是否读取/设置余氯时间间隔
        /// </summary>
        public bool IsOpt_ResidualClInterval = false;
        private DataTable _ResidualCl_Interval;
        /// <summary>
        /// 余氯时间间隔
        /// </summary>
        public DataTable ResidualCl_Interval
        {
            get { return _ResidualCl_Interval; }
            set { _ResidualCl_Interval = value; }
        }

        /// <summary>
        /// 是否读取/设置浊度时间间隔
        /// </summary>
        public bool IsOpt_TurbidityInterval = false;
        private DataTable _Turbidity_Interval;
        /// <summary>
        /// 浊度时间间隔
        /// </summary>
        public DataTable Turbidity_Interval
        {
            get { return _Turbidity_Interval; }
            set { _Turbidity_Interval = value; }
        }

        /// <summary>
        /// 是否读取/设置PH时间间隔
        /// </summary>
        public bool IsOpt_PHInterval = false;
        private DataTable _PH_Interval;
        /// <summary>
        /// PH时间间隔
        /// </summary>
        public DataTable PH_Interval
        {
            get { return _PH_Interval; }
            set { _PH_Interval = value; }
        }

        /// <summary>
        /// 是否读取/设置电导率时间间隔
        /// </summary>
        public bool IsOpt_ConductivityInterval = false;
        private DataTable _Conductivity_Interval;
        /// <summary>
        /// 电导率时间间隔
        /// </summary>
        public DataTable Conductivity_Interval
        {
            get { return _Conductivity_Interval; }
            set { _Conductivity_Interval = value; }
        }

        private bool _SetPluseBasic1 = false;
        /// <summary>
        /// 是否设置第一路脉冲基准数
        /// </summary>
        public bool SetPluseBasic1
        {
            get { return _SetPluseBasic1; }
            set { _SetPluseBasic1 = value; }
        }
        private bool _SetPluseBasic2 = false;
        /// <summary>
        /// 是否设置第二路脉冲基准数
        /// </summary>
        public bool SetPluseBasic2
        {
            get { return _SetPluseBasic2; }
            set { _SetPluseBasic2 = value; }
        }
        private bool _SetPluseBasic3 = false;
        /// <summary>
        /// 是否设置第三路脉冲基准数
        /// </summary>
        public bool SetPluseBasic3
        {
            get { return _SetPluseBasic3; }
            set { _SetPluseBasic3 = value; }
        }
        private bool _SetPluseBasic4 = false;
        /// <summary>
        /// 是否设置第四路脉冲基准数
        /// </summary>
        public bool SetPluseBasic4
        {
            get { return _SetPluseBasic4; }
            set { _SetPluseBasic4 = value; }
        }
        private UInt32 _PluseBasic1 = 0;
        /// <summary>
        /// 第一路脉冲基准数
        /// </summary>
        public UInt32 PluseBasic1
        {
            get { return _PluseBasic1; }
            set { _PluseBasic1 = value; }
        }
        private UInt32 _PluseBasic2 = 0;
        /// <summary>
        /// 第二路脉冲基准数
        /// </summary>
        public UInt32 PluseBasic2
        {
            get { return _PluseBasic2; }
            set { _PluseBasic2 = value; }
        }
        private UInt32 _PluseBasic3 = 0;
        /// <summary>
        /// 第三路脉冲基准数
        /// </summary>
        public UInt32 PluseBasic3
        {
            get { return _PluseBasic3; }
            set { _PluseBasic3 = value; }
        }
        private UInt32 _PluseBasic4 = 0;
        /// <summary>
        /// 第四路脉冲基准数
        /// </summary>
        public UInt32 PluseBasic4
        {
            get { return _PluseBasic4; }
            set { _PluseBasic4 = value; }
        }

        private HydrantOptType _HydrantHistoryOpt;
        /// <summary>
        /// 消防栓指定操作类型历史数据
        /// </summary>
        public HydrantOptType HydrantHistoryOpt
        {
            get { return _HydrantHistoryOpt; }
            set { _HydrantHistoryOpt = value; }
        }

        public bool IsOpt_PreConfig = false;
        private bool _PreConfig = false;
        /// <summary>
        /// 压力配置
        /// </summary>
        public bool PreConfig
        {
            get { return _PreConfig; }
            set { _PreConfig = value; }
        }

        public bool IsOpt_Numofturns = false;
        private int _Numofturns = 0;
        /// <summary>
        /// 开度
        /// </summary>
        public int Numofturns
        {
            get { return _Numofturns; }
            set { _Numofturns = value; }
        }

        public bool IsOpt_HydrantEnable = false;

        private bool _HydrantEnable = false;
        /// <summary>
        /// 消防栓开关
        /// </summary>
        public bool HydrantEnable
        {
            get { return _HydrantEnable; }
            set { _HydrantEnable = value; }
        }

        public bool IsOpt_ComTime = false;
        private int _ComTime = 0;
        /// <summary>
        /// 远传通讯时间(每天)
        /// </summary>
        public int ComTime
        {
            get { return _ComTime; }
            set { _ComTime = value; }
        }

        public bool IsOpt_Range = false;
        private double _Range = 0;
        /// <summary>
        /// 压力、模拟量量程
        /// </summary>
        public double Range
        {
            get { return _Range; }
            set { _Range = value; }
        }

        public bool IsOpt_PreOffset = false;
        private double _PreOffset = 0;
        /// <summary>
        /// 压力值量程
        /// </summary>
        public double PreOffset
        {
            get { return _PreOffset; }
            set { _PreOffset = value; }
        }

        public bool IsOpt_RealTimeData = false;
        private double _RealTimeData = 0;
        /// <summary>
        /// 实时数据
        /// </summary>
        public double RealTimeData
        {
            get { return _RealTimeData; }
            set { _RealTimeData = value; }
        }

        /// <summary>
        /// 是否读取/设置心跳间隔
        /// </summary>
        public bool IsOpt_HeartInterval = false;

        private int _HeartInterval = 0;
        /// <summary>
        /// 心跳间隔
        /// </summary>
        public int HeartInterval
        {
            get { return _HeartInterval; }
            set { _HeartInterval = value; }
        }

        /// <summary>
        /// 是否读取/设置电压时间间隔
        /// </summary>
        public bool IsOpt_VolInterval = false;
        private int _VolInterval = 0;
        /// <summary>
        /// 电压时间间隔
        /// </summary>
        public int VolInterval
        {
            get { return _VolInterval; }
            set { _VolInterval = value; }
        }

        /// <summary>
        /// 是否读取/设置电压报警下限
        /// </summary>
        public bool IsOpt_VolLower = false;
        private short _VolLower = 0;
        /// <summary>
        /// 电压报警下限
        /// </summary>
        public short VolLower
        {
            get { return _VolLower; }
            set { _VolLower = value; }
        }

        /// <summary>
        /// 是否读取/设置短信发送间隔
        /// </summary>
        public bool IsOpt_SMSInterval = false;
        private short _SMSInterval;
        /// <summary>
        /// 短信发送间隔
        /// </summary>
        public short SMSInterval
        {
            get { return _SMSInterval; }
            set { _SMSInterval = value; }
        }

        /// <summary>
        /// 是否读取/设置脉冲计数单位
        /// </summary>
        public bool IsOpt_PluseUnit = false;
        /// <summary>
        /// 脉冲计数单位
        /// </summary>
        private int _PluseUnit;
        public int PluseUnit
        {
            get { return _PluseUnit; }
            set { _PluseUnit = value; }
        }

        /// <summary>
        /// 是否读取/设置485波特率
        /// </summary>
        public bool IsOpt_Baud485 = false;
        private int _Baud485 = 0;
        /// <summary>
        /// 485波特率 0～3分别表示波特率1200、2400、4800、9600
        /// </summary>
        public int Baud485
        {
            get { return _Baud485; }
            set { _Baud485 = value; }
        }

        /// <summary>
        /// 是否读取/设置联网模式
        /// </summary>
        public bool IsOpt_NetWorkType = false;
        private int _NetWorkType = 0;
        /// <summary>
        /// 联网模式 0-不连网，1-低功耗模式 2-实时在线模式
        /// </summary>
        public int NetWorkType
        {
            get { return _NetWorkType; }
            set { _NetWorkType = value; }
        }

        /// <summary>
        /// 是否读取/设置上限值
        /// </summary>
        public bool IsOpt_UpLimit = false;
        private double _UpLimit = 0;
        /// <summary>
        /// 上限值
        /// </summary>
        public double UpLimit
        {
            get { return _UpLimit; }
            set { _UpLimit = value; }
        }

        /// <summary>
        /// 是否读取/设置上限投退
        /// </summary>
        public bool IsOpt_UpLimitEnable = false;
        private bool _UpLimitEnable = false;
        /// <summary>
        /// 报警上限投退
        /// </summary>
        public bool UpLimitEnable
        {
            get { return _UpLimitEnable; }
            set { _UpLimitEnable = value; }
        }

        /// <summary>
        /// 是否读取/设置下限值
        /// </summary>
        public bool IsOpt_LowLimit = false;
        private double _LowLimit = 0;
        /// <summary>
        /// 下限值
        /// </summary>
        public double LowLimit
        {
            get { return _LowLimit; }
            set { _LowLimit = value; }
        }

        /// <summary>
        /// 是否读取/设置下限投退
        /// </summary>
        public bool IsOpt_LowLimitEnable = false;
        private bool _LowLimitEnable = false;
        /// <summary>
        /// 报警下限投退
        /// </summary>
        public bool LowLimitEnable
        {
            get { return _LowLimitEnable; }
            set { _LowLimitEnable = value; }
        }

        /// <summary>
        /// 是否读取/设置斜率上限值
        /// </summary>
        public bool IsOpt_SlopUpLimit = false;
        private double _SlopUpLimit = 0;
        /// <summary>
        /// 斜率上限值
        /// </summary>
        public double SlopUpLimit
        {
            get { return _SlopUpLimit; }
            set { _SlopUpLimit = value; }
        }

        /// <summary>
        /// 是否读取/设置斜率上限投退
        /// </summary>
        public bool IsOpt_SlopUpLimitEnable = false;
        private bool _SlopUpLimitEnable = false;
        /// <summary>
        /// 斜率报警上限投退
        /// </summary>
        public bool SlopUpLimitEnable
        {
            get { return _SlopUpLimitEnable; }
            set { _SlopUpLimitEnable = value; }
        }

        /// <summary>
        /// 是否读取/设置斜率下限值
        /// </summary>
        public bool IsOpt_SlopLowLimit = false;
        private double _SlopLowLimit = 0;
        /// <summary>
        /// 斜率下限值
        /// </summary>
        public double SlopLowLimit
        {
            get { return _SlopLowLimit; }
            set { _SlopLowLimit = value; }
        }

        /// <summary>
        /// 是否读取/设置斜率下限投退
        /// </summary>
        public bool IsOpt_SlopLowLimitEnable = false;
        private bool _SlopLowLimitEnable = false;
        /// <summary>
        /// 斜率报警下限投退
        /// </summary>
        public bool SlopLowLimitEnable
        {
            get { return _SlopLowLimitEnable; }
            set { _SlopLowLimitEnable = value; }
        }

        private string _Ver = "";
        /// <summary>
        /// 版本号
        /// </summary>
        public string Ver
        {
            get { return _Ver; }
            set { _Ver = value; }
        }

        private string _FieldStrength = "";
        /// <summary>
        /// 场强\电压
        /// </summary>
        public string FieldStrength
        {
            get { return _FieldStrength; }
            set { _FieldStrength = value; }
        }

        private UniversalFlagType _FlagType;
        /// <summary>
        /// 报警标识
        /// </summary>
        public UniversalFlagType FlagType
        {
            get { return _FlagType; }
            set { _FlagType = value; }
        }
        
    }
}
