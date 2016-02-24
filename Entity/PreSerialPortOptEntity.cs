using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Entity
{
    public class PreSerialPortOptEntity
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
        /// 是否读取电池电压采集间隔
        /// </summary>
        public bool IsOptVoltageInterval = false;
        private string _VoltageInterval;
        /// <summary>
        /// 电池电压采集间隔
        /// </summary>
        public string VoltageInterval
        {
            get { return _VoltageInterval; }
            set { _VoltageInterval = value; }
        }

        /// <summary>
        /// 是否读取电池电压报警下限
        /// </summary>
        public bool IsOptVoltageLowLimit = false;
        private string _VoltageLowLimit;
        /// <summary>
        /// 电池电压报警下限
        /// </summary>
        public string VoltageLowLimit
        {
            get { return _VoltageLowLimit; }
            set { _VoltageLowLimit = value; }
        }

        /// <summary>
        /// 读取采集功能配置
        /// </summary>
        public bool IsOptCollectConfig = false;
        private string _CollectConfig;
        /// <summary>
        /// 采集功能配置
        /// </summary>
        public string CollectConfig
        {
            get { return _CollectConfig; }
            set { _CollectConfig = value; }
        }

        public bool IsOptConnectType = false;
        private int _ConnectType;
        /// <summary>
        /// 通讯方式
        /// </summary>
        public int ConnectType
        {
            get { return _ConnectType; }
            set { _ConnectType = value; }
        }

        public bool IsOptBaudrate = false;
        private int _Baudrate;
        /// <summary>
        /// 波特率
        /// </summary>
        public int Baudrate
        {
            get { return _Baudrate; }
            set { _Baudrate = value; }
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

        public bool IsOptHeartInterval = false;
        private int _HeartInterval;
        /// <summary>
        /// 心跳间隔
        /// </summary>
        public int HeartInterval
        {
            get { return _HeartInterval; }
            set { _HeartInterval = value; }
        }

        public bool IsOptPreInterval = false;
        private DataTable _PreInterval;
        /// <summary>
        /// 压力时间间隔
        /// </summary>
        public DataTable PreInterval
        {
            get { return _PreInterval; }
            set { _PreInterval = value; }
        }

        public bool IsOptPreUpLimit = false;
        private int _PreUpLimit;
        /// <summary>
        /// 压力上限值
        /// </summary>
        public int PreUpLimit
        {
            get { return _PreUpLimit; }
            set { _PreUpLimit = value; }
        }

        public bool IsOptEnablePreUpLimit = false;
        private bool _EnablePreUpLimit;
        /// <summary>
        /// 压力上限投退
        /// </summary>
        public bool EnablePreUpLimit
        {
            get { return _EnablePreUpLimit; }
            set { _EnablePreUpLimit = value; }
        }

        public bool IsOptPreLowLimit = false;
        private int _PreLowLimit;
        /// <summary>
        /// 压力下限值
        /// </summary>
        public int PreLowLimit
        {
            get { return _PreLowLimit; }
            set { _PreLowLimit = value; }
        }

        public bool IsOptEnablePreLowLimit = false;
        private bool _EnablePreLowLimit;
        /// <summary>
        /// 压力下限投退
        /// </summary>
        public bool EnablePreLowLimit
        {
            get { return _EnablePreLowLimit; }
            set { _EnablePreLowLimit = value; }
        }

        public bool IsOptSlopUpLimit = false;
        private int _SlopUpLimit;
        /// <summary>
        /// 压力斜率上限值
        /// </summary>
        public int SlopUpLimit
        {
            get { return _SlopUpLimit; }
            set { _SlopUpLimit = value; }
        }

        public bool IsOptEnableSlopUpLimit = false;
        private bool _EnableSlopUpLimit;
        /// <summary>
        /// 压力斜率上限投退
        /// </summary>
        public bool EnableSlopUpLimit
        {
            get { return _EnableSlopUpLimit; }
            set { _EnableSlopUpLimit = value; }
        }

        public bool IsOptSlopLowLimit = false;
        private int _SlopLowLimit;
        /// <summary>
        /// 压力斜率下限值
        /// </summary>
        public int SlopLowLimit
        {
            get { return _SlopLowLimit; }
            set { _SlopLowLimit = value; }
        }

        public bool IsOptEnableSlopLowLimit = false;
        private bool _EnableSlopLowLimit;
        /// <summary>
        /// 压力斜率下限投退
        /// </summary>
        public bool EnableSlopLowLimit
        {
            get { return _EnableSlopLowLimit; }
            set { _EnableSlopLowLimit = value; }
        }

        public bool IsOptPreRange = false;
        private int _PreRange;
        /// <summary>
        /// 量程
        /// </summary>
        public int PreRange
        {
            get { return _PreRange; }
            set { _PreRange = value; }
        }

        public bool IsOptOffset = false;
        private int _Offset;
        /// <summary>
        /// 偏移量
        /// </summary>
        public int Offset
        {
            get { return _Offset; }
            set { _Offset = value; }
        }
    }
}
