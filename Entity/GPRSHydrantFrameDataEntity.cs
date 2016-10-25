using System;
using System.Collections.Generic;

namespace Entity
{
    public class GPRSHydrantFrameDataEntity
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

        private List<GPRSHydrantDataEntity> _lstHydrantData = new List<GPRSHydrantDataEntity>();
        public List<GPRSHydrantDataEntity> lstHydrantData
        {
            get { return _lstHydrantData; }
            set { _lstHydrantData = value; }
        }
    }
    public class GPRSHydrantDataEntity
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

        private HydrantOptType _Operate;
        /// <summary>
        /// 操作
        /// </summary>
        public HydrantOptType Operate
        {
            get { return _Operate; }
            set { _Operate = value; }
        }

        private float _preValue = -1f;
        /// <summary>
        /// 压力
        /// </summary>
        public float PreValue
        {
            get { return _preValue; }
            set { _preValue = value; }
        }

        private float _OpenAngle = 0f;
        /// <summary>
        /// 开度
        /// </summary>
        public float OpenAngle
        {
            get { return _OpenAngle; }
            set { _OpenAngle = value; }
        }
    }
}
