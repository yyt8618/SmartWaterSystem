
namespace Entity
{
    public class TerminalConfigEntity
    {
        private int _TerminalID;
        /// <summary>
        /// 终端ID
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

        private string _TerminalAddr = "";
        /// <summary>
        /// 终端地址
        /// </summary>
        public string TerminalAddr
        {
            get { return _TerminalAddr; }
            set { _TerminalAddr = value; }
        }

        private string _Remark = "";
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
        }

        private decimal _PreLowLimit = 0;
        /// <summary>
        /// 压力下限值
        /// </summary>
        public decimal PreLowLimit
        {
            get { return _PreLowLimit; }
            set { _PreLowLimit = value; }
        }

        private decimal _PreUpLimit = 0;
        /// <summary>
        /// 压力上限值
        /// </summary>
        public decimal PreUpLimit
        {
            get { return _PreUpLimit; }
            set { _PreUpLimit = value; }
        }

        private decimal _PreSlopeLowLimit = 0;
        /// <summary>
        /// 压力终端斜率下限值
        /// </summary>
        public decimal PreSlopeLowLimit
        {
            get { return _PreSlopeLowLimit; }
            set { _PreSlopeLowLimit = value; }
        }

        private decimal _PreSlopeUpLimit = 0;
        /// <summary>
        /// 压力终端斜率上限值
        /// </summary>
        public decimal PreSlopeUpLimit
        {
            get { return _PreSlopeUpLimit; }
            set { _PreSlopeUpLimit = value; }
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
    }
}
