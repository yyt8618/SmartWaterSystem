using System;
using System.Collections.Generic;

namespace Entity
{
    public class GPRSWaterWorkerFrameDataEntity
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

        private double _activenerge1 = 0;
        /// <summary>
        /// 1#有功电量
        /// </summary>
        public double Activenerge1
        {
            get { return _activenerge1; }
            set { _activenerge1 = value; }
        }

        private double _reactivenerge1 = 0;
        /// <summary>
        /// 1#无功电量
        /// </summary>
        public double Rectivenerge1
        {
            get { return _reactivenerge1; }
            set { _reactivenerge1 = value; }
        }

        private double _activenerge2 = 0;
        /// <summary>
        /// 2#有功电量
        /// </summary>
        public double Activenerge2
        {
            get { return _activenerge2; }
            set { _activenerge2 = value; }
        }

        private double _reactivenerge2 = 0;
        /// <summary>
        /// 2#无功电量
        /// </summary>
        public double Rectivenerge2
        {
            get { return _reactivenerge2; }
            set { _reactivenerge2 = value; }
        }

        private double _activenerge3 = 0;
        /// <summary>
        /// 3#有功电量
        /// </summary>
        public double Activenerge3
        {
            get { return _activenerge3; }
            set { _activenerge3 = value; }
        }

        private double _reactivenerge3 = 0;
        /// <summary>
        /// 3#无功电量
        /// </summary>
        public double Rectivenerge3
        {
            get { return _reactivenerge3; }
            set { _reactivenerge3 = value; }
        }

        private double _activenerge4 = 0;
        /// <summary>
        /// 4#有功电量
        /// </summary>
        public double Activenerge4
        {
            get { return _activenerge4; }
            set { _activenerge4 = value; }
        }

        private double _reactivenerge4 = 0;
        /// <summary>
        /// 4#无功电量
        /// </summary>
        public double Rectivenerge4
        {
            get { return _reactivenerge4; }
            set { _reactivenerge4 = value; }
        }

        private double _pressure = 0;
        /// <summary>
        /// 压力值
        /// </summary>
        public double Pressure
        {
            get { return _pressure; }
            set { _pressure = value; }
        }

        private double _liquidlevel = 0;
        /// <summary>
        /// 液位
        /// </summary>
        public double LiquidLevel
        {
            get { return _liquidlevel; }
            set { _liquidlevel = value; }
        }

        private double _flow1 = 0;
        /// <summary>
        /// 流量1
        /// </summary>
        public double Flow1
        {
            get { return _flow1; }
            set { _flow1 = value; }
        }

        private double _flow2 = 0;
        /// <summary>
        /// 流量2
        /// </summary>
        public double Flow2
        {
            get { return _flow2; }
            set { _flow2 = value; }
        }

        private bool _switch1 = false;
        /// <summary>
        /// 开关状态1
        /// </summary>
        public bool Switch1
        {
            get { return _switch1; }
            set { _switch1 = value; }
        }

        private bool _switch2 = false;
        /// <summary>
        /// 开关状态2
        /// </summary>
        public bool Switch2
        {
            get { return _switch2; }
            set { _switch2 = value; }
        }

        private bool _switch3 = false;
        /// <summary>
        /// 开关状态3
        /// </summary>
        public bool Switch3
        {
            get { return _switch3; }
            set { _switch3 = value; }
        }

        private bool _switch4 = false;
        /// <summary>
        /// 开关状态4
        /// </summary>
        public bool Switch4
        {
            get { return _switch4; }
            set { _switch4 = value; }
        }

        private float _Current1 = -1;
        /// <summary>
        /// 1#电流
        /// </summary>
        public float Current1
        {
            get { return _Current1; }
            set { _Current1 = value; }
        }

        private float _Current2 = -1;
        /// <summary>
        /// 2#电流
        /// </summary>
        public float Current2
        {
            get { return _Current2; }
            set { _Current2 = value; }
        }

        private float _Current3 = -1;
        /// <summary>
        /// 3#电流
        /// </summary>
        public float Current3
        {
            get { return _Current3; }
            set { _Current3 = value; }
        }

        private float _Current4 = -1;
        /// <summary>
        /// 4#电流
        /// </summary>
        public float Current4
        {
            get { return _Current4; }
            set { _Current4 = value; }
        }

        private float _Freq1 = -1;
        /// <summary>
        /// 1#频率
        /// </summary>
        public float Freq1
        {
            get { return _Freq1; }
            set { _Freq1 = value; }
        }

        private float _Freq2 = -1;
        /// <summary>
        /// 2#频率
        /// </summary>
        public float Freq2
        {
            get { return _Freq2; }
            set { _Freq2 = value; }
        }

        private float _Freq3 = -1;
        /// <summary>
        /// 3#频率
        /// </summary>
        public float Freq3
        {
            get { return _Freq3; }
            set { _Freq3 = value; }
        }

        private float _Freq4 = -1;
        /// <summary>
        /// 4#频率
        /// </summary>
        public float Freq4
        {
            get { return _Freq4; }
            set { _Freq4 = value; }
        }


    }
}
