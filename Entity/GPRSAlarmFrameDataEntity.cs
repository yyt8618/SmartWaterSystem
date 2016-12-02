using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class GPRSAlarmFrameDataEntity
    {
        private string _frame = "";
        /// <summary>
        /// 数据帧
        /// </summary>
        public string Frame
        {
            get { return _frame; }
            set { _frame = value; }
        }

        private string _TerId = "";
        /// <summary>
        /// 终端编号
        /// </summary>
        public string TerId
        {
            get { return _TerId; }
            set { _TerId = value; }
        }

        private DateTime _ModifyTime = DateTime.Now;
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get { return _ModifyTime; }
            set { _ModifyTime = value; }
        }

        /// <summary>
        /// 终端类型
        /// </summary>
        public TerType TerminalType { get; set; }

        /// <summary>
        /// 报警表ID(int) (TerminalType+FunCode+AlarmFlag(2byte)组成)
        /// </summary>
        public List<int> AlarmId { get; set; }

        private float _Voltage = -1f;
        /// <summary>
        /// 电压值
        /// </summary>
        public float Voltage
        {
            get { return _Voltage; }
            set { _Voltage = value; }
        }

        private Int16 _FieldStrength = -1;
        /// <summary>
        /// 信号强度
        /// </summary>
        public Int16 FieldStrength
        {
            get { return _FieldStrength; }
            set { _FieldStrength = value; }
        }
    }

}
