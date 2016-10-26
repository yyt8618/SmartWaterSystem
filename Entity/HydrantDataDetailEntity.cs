using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class HydrantDataDetailEntity
    {
        private string _Id;
        /// <summary>
        /// 消防栓编号
        /// </summary>
        public string ID
        {
            get { return _Id; }
            set { _Id = value; }
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

        private string _CollTime;
        /// <summary>
        /// 采集时间
        /// </summary>
        public string CollTime
        {
            get { return _CollTime; }
            set { _CollTime = value; }
        }
        

    }
}
