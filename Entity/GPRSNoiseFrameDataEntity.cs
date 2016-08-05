using System;
using System.Collections.Generic;

namespace Entity
{
    public class GPRSNoiseFrameDataEntity
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

        private UpLoadNoiseDataEntity _NoiseData = null;
        public UpLoadNoiseDataEntity NoiseData
        {
            get { return _NoiseData; }
            set { _NoiseData = value; }
        }
    }
}
