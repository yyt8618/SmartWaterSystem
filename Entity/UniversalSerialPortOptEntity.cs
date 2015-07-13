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

        /// <summary>
        /// 是否读取/设置modbus协议标识
        /// </summary>
        //public bool IsOptmodbusExeFlag = false;
        private bool _modbusExeFlag;
        /// <summary>
        /// 485采集执行modbus协议标识
        /// </summary>
        public bool ModbusExeFlag
        {
            get { return _modbusExeFlag; }
            set { _modbusExeFlag = value; }
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

    }
}
