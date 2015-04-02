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
        private float _sim1 = 0f;
        public float Sim1
        {
            get { return _sim1; }
            set { _sim1 = value; }
        }

        private float _sim2 = 0f;
        public float Sim2
        {
            get { return _sim2; }
            set { _sim2 = value; }
        }

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

        private float _pluse1 = 0f;
        public float Pluse1
        {
            get { return _pluse1; }
            set { _pluse1 = value; }
        }

        private float _pluse2 = 0f;
        public float Pluse2
        {
            get { return _pluse2; }
            set { _pluse2 = value; }
        }

        private float _pluse3 = 0f;
        public float Pluse3
        {
            get { return _pluse3; }
            set { _pluse3 = value; }
        }

        private float _pluse4 = 0f;
        public float Pluse4
        {
            get { return _pluse4; }
            set { _pluse4 = value; }
        }

        private float _pluse5 = 0f;
        public float Pluse5
        {
            get { return _pluse1; }
            set { _pluse1 = value; }
        }

        private float _rs4851 = 0f;
        public float RS4851
        {
            get { return _rs4851; }
            set { _rs4851 = value; }
        }

        private float _rs4852 = 0f;
        public float RS4852
        {
            get { return _rs4852; }
            set { _rs4852 = value; }
        }

        private float _rs4853 = 0f;
        public float RS4853
        {
            get { return _rs4853; }
            set { _rs4853 = value; }
        }

        private float _rs4854 = 0f;
        public float RS4854
        {
            get { return _rs4854; }
            set { _rs4854 = value; }
        }

        private float _rs4855 = 0f;
        public float RS4855
        {
            get { return _rs4855; }
            set { _rs4855 = value; }
        }

        private float _rs4856 = 0f;
        public float RS4856
        {
            get { return _rs4856; }
            set { _rs4856 = value; }
        }

        private float _rs4857 = 0f;
        public float RS4857
        {
            get { return _rs4857; }
            set { _rs4857 = value; }
        }

        private float _rs4858 = 0f;
        public float RS4858
        {
            get { return _rs4858; }
            set { _rs4858 = value; }
        }

        private DateTime _colTime;
        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime ColTime
        {
            get { return _colTime; }
            set { _colTime = value; }
        }

        private float _DataValue = 0f;
        /// <summary>
        /// 采集值
        /// </summary>
        public float DataValue
        {
            get { return _DataValue; }
            set { _DataValue = value; }
        }

    }
}
