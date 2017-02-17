using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class GPRSPrectrlFrameDataEntity
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

        private List<GPRSPrectrlDataEntity> _lstPrectrlData = new List<GPRSPrectrlDataEntity>();
        public List<GPRSPrectrlDataEntity> lstPrectrlData
        {
            get { return _lstPrectrlData; }
            set { _lstPrectrlData = value; }
        }
    }
    public class GPRSPrectrlDataEntity
    {
        private DateTime _colTime;
        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime ColTime
        {
            get { return _colTime; }
            set
            {
                if ((value - DateTime.Now).TotalHours > 1)  //如果超过当前时间1小时，使用当前时间
                    _colTime = DateTime.Now;
                else
                    _colTime = value;
            }
        }

        private double _entrance_preValue = 0f;
        /// <summary>
        /// 进口压力
        /// </summary>
        public double Entrance_preValue
        {
            get { return _entrance_preValue; }
            set { _entrance_preValue = value; }
        }

        private double _outlet_preValue = 0f;
        /// <summary>
        /// 出口压力
        /// </summary>
        public double Outlet_preValue
        {
            get { return _outlet_preValue; }
            set { _outlet_preValue = value; }
        }

        private double _forward_flowValue = 0f;
        /// <summary>
        /// 正向流量
        /// </summary>
        public double Forward_FlowValue
        {
            get { return _forward_flowValue; }
            set { _forward_flowValue = value; }
        }

        private double _reverse_flowValue = 0f;
        /// <summary>
        /// 反向流量
        /// </summary>
        public double Reverse_FlowValue
        {
            get { return _reverse_flowValue; }
            set { _reverse_flowValue = value; }
        }

        private double _instant_flowValue = 0f;
        /// <summary>
        /// 瞬时流量
        /// </summary>
        public double Instant_FlowValue
        {
            get { return _instant_flowValue; }
            set { _instant_flowValue = value; }
        }

        private uint _alarmcode;
        /// <summary>
        /// 报警编号
        /// </summary>
        public uint AlarmCode
        {
            get { return _alarmcode; }
            set { _alarmcode = value; }
        }

        private string _alarmdesc;
        /// <summary>
        /// 报警描述
        /// </summary>
        public string AlarmDesc
        {
            get { return _alarmdesc; }
            set { _alarmdesc = value; }
        }

        private float _Voltage = 0f;
        /// <summary>
        /// 电压值
        /// </summary>
        public float Voltage
        {
            get { return _Voltage; }
            set { _Voltage = value; }
        }
    }
}
