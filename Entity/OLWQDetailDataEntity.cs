using System;

namespace Entity
{
    public class OLWQDetailDataEntity
    {
        private decimal _Value;
        /// <summary>
        /// 值
        /// </summary>
        public decimal Value
        {
            get { return _Value; }
            set { _Value = value; }
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
