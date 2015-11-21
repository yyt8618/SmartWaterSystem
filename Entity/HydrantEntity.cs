using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class HydrantEntity
    {
        private string _hydrantId;
        /// <summary>
        /// 消防栓编号
        /// </summary>
        public string HydrantID
        {
            get { return _hydrantId; }
            set { _hydrantId = value; }
        }

        private string _addr;
        /// <summary>
        /// 消防栓地址
        /// </summary>
        public string Addr
        {
            get { return _addr; }
            set { _addr = value; }
        }

        private string _longtitude;
        /// <summary>
        /// 经度
        /// </summary>
        public string Longtitude
        {
            get { return _longtitude; }
            set { _longtitude = value; }
        }

        private string _latitude;
        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }

        private HydrantOptType _OptType= HydrantOptType.Close;
        /// <summary>
        /// 消防栓操作类型
        /// </summary>
        public HydrantOptType OptType
        {
            get { return _OptType; }
            set { _OptType = value; }
        }

        private float _PressValue = 0f;
        /// <summary>
        /// 压力值
        /// </summary>
        public float PressValue
        {
            get { return _PressValue; }
            set { _PressValue = value; }
        }

        private int _OpenAngle = 0;
        /// <summary>
        /// 开度
        /// </summary>
        public int OpenAngle
        {
            get { return _OpenAngle; }
            set { _OpenAngle = value; }
        }

        private bool _IsAlarm = true; 
        /// <summary>
        /// 是否报警
        /// </summary>
        public bool IsAlarm
        {
            get { return _IsAlarm; }
            set { _IsAlarm = value; }
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
