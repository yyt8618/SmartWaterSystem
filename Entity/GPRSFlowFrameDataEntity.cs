using System;
using System.Collections.Generic;

namespace Entity
{
    public class GPRSFlowFrameDataEntity
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

        private List<GPRSFlowDataEntity> _lstFlowData = new List<GPRSFlowDataEntity>();
        public List<GPRSFlowDataEntity> lstFlowData
        {
            get { return _lstFlowData; }
            set { _lstFlowData = value; }
        }
    }
    public class GPRSFlowDataEntity
    {
        private DateTime _colTime;
        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime ColTime
        {
            get { return _colTime; }
            set { _colTime = value; }
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
