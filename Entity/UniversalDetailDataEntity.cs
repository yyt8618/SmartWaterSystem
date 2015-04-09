using System;

namespace Entity
{
    public class UniversalDetailDataEntity
    {
        private decimal _Data;
        //数据
        public decimal Data
        {
            get { return _Data; }
            set { _Data = value; }
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
