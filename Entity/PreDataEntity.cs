using System;

namespace Entity
{
    public class PreDataEntity
    {
        private int _TerminalID;
        /// <summary>
        /// 终端编号
        /// </summary>
        public int TerminalID
        {
            get { return _TerminalID; }
            set { _TerminalID = value; }
        }

        private string _ConfigName;
        /// <summary>
        /// 配置名称
        /// </summary>
        public string ConfigName
        {
            get { return _ConfigName; }
            set { _ConfigName = value; }
        }

        private string _TerminalName="";
        /// <summary>
        /// 终端名称
        /// </summary>
        public string TerminalName
        {
            get { return _TerminalName; }
            set { _TerminalName = value; }
        }

        private decimal _PressueValue;
        /// <summary>
        /// 压力值
        /// </summary>
        public decimal PressueValue
        {
            get { return _PressueValue; }
            set { _PressueValue = value; }
        }

        private decimal _NewestPressueValue;
        /// <summary>
        /// 最新压力值
        /// </summary>
        public decimal NewestPressueValue
        {
            get { return _NewestPressueValue; }
            set { _NewestPressueValue = value; }
        }

        private decimal _PreValueLastbutone;
        /// <summary>
        /// 倒数第二条压力值(相对最新压力值;用于计算斜率边界)
        /// </summary>
        public decimal PreValueLastbutone
        {
            get { return _PreValueLastbutone; }
            set { _PreValueLastbutone = value; }
        }

        private DateTime _CollTime = ConstValue.MinDateTime;
        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollTime
        {
            get { return _CollTime; }
            set { _CollTime = value; }
        }

        private DateTime _NewestCollTime = ConstValue.MinDateTime;
        /// <summary>
        /// 最新采集时间
        /// </summary>
        public DateTime NewestCollTime
        {
            get { return _NewestCollTime; }
            set { _NewestCollTime = value; }
        }

        private DateTime _CollTimeLastbutone = ConstValue.MinDateTime;
        /// <summary>
        /// 倒数第二条采集时间
        /// </summary>
        public DateTime CollTimeLastbutone
        {
            get { return _CollTimeLastbutone; }
            set { _CollTimeLastbutone = value; }
        }

        private bool _EnablePreAlarm = true;
        /// <summary>
        /// 是否启用压力报警
        /// </summary>
        public bool EnablePreAlarm
        {
            get { return _EnablePreAlarm; }
            set { _EnablePreAlarm = value; }
        }

        private bool _EnableSlopeAlarm = true;
        /// <summary>
        /// 是否启用斜率报警
        /// </summary>
        public bool EnableSlopeAlarm
        {
            get { return _EnableSlopeAlarm; }
            set { _EnableSlopeAlarm = value; }
        }

        private decimal _PreLowLimit;
        /// <summary>
        /// 压力下限值
        /// </summary>
        public decimal PreLowLimit
        {
            get { return _PreLowLimit; }
            set { _PreLowLimit = value; }
        }

        private decimal _PreUpLimit;
        /// <summary>
        /// 压力上限值
        /// </summary>
        public decimal PreUpLimit
        {
            get { return _PreUpLimit; }
            set { _PreUpLimit = value; }
        }

        private decimal _PreSlopeLowLimit;
        /// <summary>
        /// 斜率下限值
        /// </summary>
        public decimal PreSlopeLowLimit
        {
            get { return _PreSlopeLowLimit; }
            set { _PreSlopeLowLimit = value; }
        }

        private decimal _PreSlopeUpLimit;
        /// <summary>
        /// 斜率上限值
        /// </summary>
        public decimal PreSlopeUpLimit
        {
            get { return _PreSlopeUpLimit; }
            set { _PreSlopeUpLimit = value; }
        }

        private DateTime _UploadTime;
        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UploadTime
        {
            get { return _UploadTime; }
            set { _UploadTime = value; }
        }

        private string _Addr = "";
        /// <summary>
        /// 地址
        /// </summary>
        public string Addr
        {
            get { return _Addr; }
            set { _Addr = value; }
        }
    }
}
