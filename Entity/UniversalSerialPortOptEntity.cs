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
        public bool IsOptmodbusExeFlag = false;
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
        /// 是否读取/设置波特率
        /// </summary>
        public bool IsOptBaud = false;
        private int _baud;
        /// <summary>
        /// 波特率
        /// </summary>
        public int Baud
        {
            get { return _baud; }
            set { _baud = value; }
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

        /// <summary>
        /// 是否读取/设置采集配置
        /// </summary>
        public bool IsOpt_CollectConfig = false;


        private bool _collect_simulate;
        /// <summary>
        /// 是否采集模拟量
        /// </summary>
        public bool Collect_Simulate
        {
            get { return _collect_simulate; }
            set { _collect_simulate = value; }
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

    }
}
