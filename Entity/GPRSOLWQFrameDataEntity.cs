using System;
using System.Collections.Generic;

namespace Entity
{
    public class GPRSOLWQFrameDataEntity
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

        private List<GPRSOLWQDataEntity> _lstOLWQData = new List<GPRSOLWQDataEntity>();
        public List<GPRSOLWQDataEntity> lstOLWQData
        {
            get { return _lstOLWQData; }
            set { _lstOLWQData = value; }
        }
    }
    public class GPRSOLWQDataEntity
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

        private float _Turbidity = -1f;
        /// <summary>
        /// 浊度值
        /// </summary>
        public float Turbidity
        {
            get { return _Turbidity; }
            set { _Turbidity = value; }
        }

        private float _ResidualCl = -1f;
        /// <summary>
        /// 余氯值
        /// </summary>
        public float ResidualCl
        {
            get { return _ResidualCl; }
            set { _ResidualCl = value; }
        }

        private float _PH = -1f;
        /// <summary>
        /// PH值
        /// </summary>
        public float PH
        {
            get { return _PH; }
            set { _PH = value; }
        }

        private float _Conductivity = -1f;
        /// <summary>
        /// 电导率值
        /// </summary>
        public float Conductivity
        {
            get { return _Conductivity; }
            set { _Conductivity = value; }
        }

        private float _temperature = -1f;
        /// <summary>
        /// 温度
        /// </summary>
        public float Temperature
        {
            get { return _temperature; }
            set { _temperature = value; }
        }
    }
}
