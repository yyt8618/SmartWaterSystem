using System;

namespace Entity
{
    public class PrectrlDataEntity
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

        private string _TerminalName = "";
        /// <summary>
        /// 终端名称
        /// </summary>
        public string TerminalName
        {
            get { return _TerminalName; }
            set { _TerminalName = value; }
        }

        private decimal _entrancePreValue;
        /// <summary>
        /// 进口压力
        /// </summary>
        public decimal EntrancePreValue
        {
            get { return _entrancePreValue; }
            set { _entrancePreValue = value; }
        }

        private decimal _outletPreValue;
        /// <summary>
        /// 出口压力
        /// </summary>
        public decimal OutletPreValue
        {
            get { return _outletPreValue; }
            set { _outletPreValue = value; }
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

        private byte _alarmCode;
        /// <summary>
        /// 报警值,高四位参数报警标志,低四位设备报警标志
        /// </summary>
        public byte AlarmCode
        {
            get { return _alarmCode; }
            set { _alarmCode = value; }
        }

        private string _alarmDesc;
        /// <summary>
        /// 报警描述
        /// </summary>
        public string AlarmDesc
        {
            get { return _alarmDesc; }
            set { _alarmDesc = value; }
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
