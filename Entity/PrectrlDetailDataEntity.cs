using System;

namespace Entity
{
    public class PrectrlDetailDataEntity
    {
        private decimal _EntrancePreData;
        //进口压力
        public decimal EntrancePreData
        {
            get { return _EntrancePreData; }
            set { _EntrancePreData = value; }
        }

        private decimal _OutletPreData;
        /// <summary>
        /// 出口压力
        /// </summary>
        public decimal OutletPreData
        {
            get { return _OutletPreData; }
            set { _OutletPreData = value; }
        }

        private decimal _forwardFlow;
        /// <summary>
        /// 正向流量
        /// </summary>
        public decimal ForwardFlow
        {
            get { return _forwardFlow; }
            set { _forwardFlow = value; }
        }

        private decimal _ReverseFlow;
        /// <summary>
        /// 反向流量
        /// </summary>
        public decimal ReverseFlow
        {
            get { return _ReverseFlow; }
            set { _ReverseFlow = value; }
        }

        private decimal _InstantFlow;
        /// <summary>
        /// 瞬时流量
        /// </summary>
        public decimal InstantFlow
        {
            get { return _InstantFlow; }
            set { _InstantFlow = value; }
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
