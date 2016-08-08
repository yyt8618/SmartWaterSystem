using System;

namespace Entity
{
    public class NoiseSerialPortOptEntity
    {
        private short _id;
        /// <summary>
        /// 设备ID
        /// </summary>
        public bool IsOptID = false;
        public short ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private short _setid;
        /// <summary>
        /// 远传控制器待设置ID
        /// </summary>
        public short SetID
        {
            get { return _setid; }
            set { _setid = value; }
        }

        private DateTime _dt;
        /// <summary>
        /// 时间
        /// </summary>
        public bool IsOptDT = false;
        public DateTime dt
        {
            get { return _dt; }
            set { _dt = value; }
        }

        private int _comtime;
        /// <summary>
        /// 远传通讯时间
        /// </summary>
        public bool IsOptComTime = false;
        public int ComTime
        {
            get { return _comtime; }
            set { _comtime = value; }
        }

        private int _colstarttime;
        /// <summary>
        /// 采集开始时间
        /// </summary>
        public bool IsOptColTime = false;  //采集时间
        public int colstarttime
        {
            get { return _colstarttime; }
            set { _colstarttime = value; }
        }
        private int _colendtime;
        /// <summary>
        /// 采集结束时间
        /// </summary>
        public int colendtime
        {
            get { return _colendtime; }
            set { _colendtime = value; }
        }

        private int _interval;
        /// <summary>
        /// 采集时间间隔
        /// </summary>
        public bool IsOptInterval = false;
        public int Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

        private bool _RemoteSwitch;
        /// <summary>
        /// 远传启动停止
        /// </summary>
        public bool IsOptRemoteSwitch = false;
        public bool RemoteSwitch
        {
            get { return _RemoteSwitch; }
            set { _RemoteSwitch = value; }
        }

        private int _remoteId;
        /// <summary>
        /// 读取远传控制器设备与记录仪设备对应的ID号
        /// </summary>
        public bool IsOptRemoteId = false;
        public int RemoteId
        {
            get { return _remoteId; }
            set { _remoteId = value; }
        }

        private string _ip;
        /// <summary>
        /// IP
        /// </summary>
        public bool IsOptIP = false;
        public string IP
        {
            get { return _ip; }
            set { _ip = value; }
        }

        private int _port = 80;
        /// <summary>
        /// 端口号
        /// </summary>
        public bool IsOptPort = false;
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        private double _frequency=433.920;
        /// <summary>
        /// 收发频率
        /// </summary>
        public double Frequency
        {
            get { return _frequency; }
            set { _frequency = value; }
        }

        private int _rate=4;
        /// <summary>
        /// 无线速率
        /// </summary>
        public int Rate
        {
            get { return _rate; }
            set { _rate = value; }
        }

        private int _power;
        /// <summary>
        /// 发射功率
        /// </summary>
        public int Power
        {
            get { return _power; }
            set { _power = value; }
        }

        private int _baud=3;
        /// <summary>
        /// 串口波特率
        /// </summary>
        public int Baud
        {
            get { return _baud; }
            set { _baud = value; }
        }

        private int _gprsbaud=3;
        /// <summary>
        /// GPRS波特率
        /// </summary>
        public int GprsBaud
        {
            get { return _gprsbaud; }
            set { _gprsbaud = value; }
        }

        private int _waketime=5;
        /// <summary>
        /// 唤醒时间
        /// </summary>
        public int WakeTime
        {
            get { return _waketime; }
            set { _waketime = value; }
        }

        private bool _Enable = false;
        /// <summary>
        /// 是否启动
        /// </summary>
        public bool IsOptEnable = false;
        public bool Enable
        {
            get { return _Enable; }
            set { _Enable = value; }
        }


        public NoiseSerialPortOptEntity()
        {
        }

        public NoiseSerialPortOptEntity(short id, DateTime dt)
        {
            this._id = id;
            this._dt = dt;
        }

        public NoiseSerialPortOptEntity(short id, DateTime dt, int comtime, int colstarttime,
            int colendtime, int interval, bool remoteswitch,int remoteid, string ip, int port,
            double fre,int rate,int power,int baud,int gprsbaud,int waketime)
        {
            this._id = id;
            this._dt = dt;
            this._comtime = comtime;
            this._colstarttime = colstarttime;
            this._colendtime = colendtime;
            this._interval = interval;
            this._RemoteSwitch = remoteswitch;
            this._remoteId = remoteid;
            this._ip = ip;
            this._port = port;
            this._frequency = fre;
            this._rate = rate;
            this._power = power;
            this._baud = baud;
            this._gprsbaud = gprsbaud;
            this._waketime = waketime;
        }
    }
}
