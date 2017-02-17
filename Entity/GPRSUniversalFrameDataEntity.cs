using System;
using System.Collections.Generic;

namespace Entity
{
    public class GPRSUniversalFrameDataEntity
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

        private List<GPRSUniversalDataEntity> _lstData = new List<GPRSUniversalDataEntity>();
        public List<GPRSUniversalDataEntity> lstData
        {
            get { return _lstData; }
            set { _lstData = value; }
        }
    }
    public class GPRSUniversalDataEntity
    {
        private float _sim1zero = 0f;
        public float Sim1Zero
        {
            get { return _sim1zero; }
            set { _sim1zero = value; }
        }

        private float _sim2zero = 0f;
        public float Sim2Zero
        {
            get { return _sim2zero; }
            set { _sim2zero = value; }
        }

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

        private double _DataValue = 0f;
        /// <summary>
        /// 采集值
        /// </summary>
        public double DataValue
        {
            get { return _DataValue; }
            set { _DataValue = value; }
        }

        private int _TypeTableID;
        /// <summary>
        /// UniversalTerWayType表对应ID(用于显示)
        /// </summary>
        public int TypeTableID
        {
            get { return _TypeTableID; }
            set { _TypeTableID = value; }
        }

    }
}
