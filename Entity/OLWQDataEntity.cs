using System;

namespace Entity
{
    public class OLWQDataEntity
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

        private string _TerminalName = "";
        /// <summary>
        /// 终端名称
        /// </summary>
        public string TerminalName
        {
            get { return _TerminalName; }
            set { _TerminalName = value; }
        }

        private decimal _TurbidityValue;
        /// <summary>
        /// 浊度值
        /// </summary>
        public decimal TurbidityValue
        {
            get { return _TurbidityValue; }
            set { _TurbidityValue = value; }
        }

        private decimal _ResidualClValue;
        /// <summary>
        /// 余氯值
        /// </summary>
        public decimal ResidualClValue
        {
            get { return _ResidualClValue; }
            set { _ResidualClValue = value; }
        }

        private decimal _PHValue;
        /// <summary>
        /// PH值
        /// </summary>
        public decimal PHValue
        {
            get { return _PHValue; }
            set { _PHValue = value; }
        }

        private decimal _ConductivityValue;
        /// <summary>
        /// 电导率
        /// </summary>
        public decimal ConductivityValue
        {
            get { return _ConductivityValue; }
            set { _ConductivityValue = value; }
        }

        private decimal _Temperature;
        /// <summary>
        /// 温度
        /// </summary>
        public decimal Temperature
        {
            get { return _Temperature; }
            set { _Temperature = value; }
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
