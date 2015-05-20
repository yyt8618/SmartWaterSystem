using System;

namespace Entity
{
    public class NoiseSerialPortOptEntity
    {
        private short _id;
        /// <summary>
        /// 设备ID
        /// </summary>
        public short ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private DateTime _dt;
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime dt
        {
            get { return _dt; }
            set { _dt = value; }
        }

        private int _comtime;
        /// <summary>
        /// 远传通讯时间
        /// </summary>
        public int ComTime
        {
            get { return _comtime; }
            set { _comtime = value; }
        }

        private int _colstarttime;
        /// <summary>
        /// 采集开始时间
        /// </summary>
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
        public int Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

        private bool _RemoteSwitch;
        /// <summary>
        /// 远传启动停止
        /// </summary>
        public bool RemoteSwitch
        {
            get { return _RemoteSwitch; }
            set { _RemoteSwitch = value; }
        }

        private string _ip;
        /// <summary>
        /// IP
        /// </summary>
        public string IP
        {
            get { return _ip; }
            set { _ip = value; }
        }

        private int _port = 80;
        /// <summary>
        /// 端口号
        /// </summary>
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        private bool _Enable = false;
        /// <summary>
        /// 是否启动
        /// </summary>
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
            int colendtime, int interval, bool remoteswitch, string ip, int port)
        {
            this._id = id;
            this._dt = dt;
            this._comtime = comtime;
            this._colstarttime = colstarttime;
            this._colendtime = colendtime;
            this._interval = interval;
            this._RemoteSwitch = remoteswitch;
            this._ip = ip;
            this._port = port;
        }
    }
}
