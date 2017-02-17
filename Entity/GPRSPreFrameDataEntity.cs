using System;
using System.Collections.Generic;

namespace Entity
{
    public class GPRSPreFrameDataEntity
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

        private List<GPRSPreDataEntity> _lstPreData = new List<GPRSPreDataEntity>();
        public List<GPRSPreDataEntity> lstPreData
        {
            get { return _lstPreData; }
            set { _lstPreData = value; }
        }
    }
    public class GPRSPreDataEntity
    {
        private DateTime _colTime;
        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime ColTime
        {
            get { return _colTime; }
            set {
                if ((value - DateTime.Now).TotalHours > 1)  //如果超过当前时间1小时，使用当前时间
                    _colTime = DateTime.Now;
                else
                    _colTime = value;
            }
        }

        private float _PreValue = 0f;
        /// <summary>
        /// 压力值
        /// </summary>
        public float PreValue
        {
            get { return _PreValue; }
            set { _PreValue = value; }
        }

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
