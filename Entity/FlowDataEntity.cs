using System;

namespace Entity
{
    public class FlowDataEntity
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

        private decimal _ForwardFlowValue;
        /// <summary>
        /// 正向流量值
        /// </summary>
        public decimal ForwardFlowValue
        {
            get { return _ForwardFlowValue; }
            set { _ForwardFlowValue = value; }
        }

        private decimal _ReverseFlowValue;
        /// <summary>
        /// 反向流量值
        /// </summary>
        public decimal ReverseFlowValue
        {
            get { return _ReverseFlowValue; }
            set { _ReverseFlowValue = value; }
        }

        private decimal _InstantFlowValue;
        /// <summary>
        /// 瞬时流量值
        /// </summary>
        public decimal InstantFlowValue
        {
            get { return _InstantFlowValue; }
            set { _InstantFlowValue = value; }
        }

        private decimal _NewestForwardFlowValue;
        /// <summary>
        /// 最新正向流量值
        /// </summary>
        public decimal NewestForwardFlowValue
        {
            get { return _NewestForwardFlowValue; }
            set { _NewestForwardFlowValue = value; }
        }

        private decimal _NewestReverseFlowValue;
        /// <summary>
        /// 最新反向流量值
        /// </summary>
        public decimal NewestReverseFlowValue
        {
            get { return _NewestReverseFlowValue; }
            set { _NewestReverseFlowValue = value; }
        }

        private decimal _NewestInstantFlowValue;
        /// <summary>
        /// 最新瞬时流量值
        /// </summary>
        public decimal NewestInstantFlowValue
        {
            get { return _NewestInstantFlowValue; }
            set { _NewestInstantFlowValue = value; }
        }

        private decimal _ForwardFlowLastbutone;
        /// <summary>
        /// 倒数第二条正向流量值(相对最新正向流量值;用于计算斜率边界)
        /// </summary>
        public decimal ForwardFlowLastbutone
        {
            get { return _ForwardFlowLastbutone; }
            set { _ForwardFlowLastbutone = value; }
        }

        private decimal _ReverseFlowLastbutone;
        /// <summary>
        /// 倒数第二条反向流量值(相对最新反向流量值;用于计算斜率边界)
        /// </summary>
        public decimal ReverseFlowLastbutone
        {
            get { return _ReverseFlowLastbutone; }
            set { _ReverseFlowLastbutone = value; }
        }

        private decimal _InstantFlowLastbutone;
        /// <summary>
        /// 倒数第二条瞬时流量值(相对最新正向流量值;用于计算斜率边界)
        /// </summary>
        public decimal InstantFlowLastbutone
        {
            get { return _InstantFlowLastbutone; }
            set { _InstantFlowLastbutone = value; }
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
