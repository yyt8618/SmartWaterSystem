using System;

namespace Entity
{
    public class PreDetailDataEntity
    {
        private decimal _PreData;
        //压力数据
        public decimal PreData
        {
            get { return _PreData; }
            set { _PreData = value; }
        }

        private DateTime _CollTime;
        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollTime
        {
            get { return _CollTime; }
            set { _CollTime = value; }
        }
    }
}
