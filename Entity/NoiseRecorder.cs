using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 噪声记录仪
    /// </summary>
    public class NoiseRecorder
    {
        public NoiseRecorder()
		{
			LeakValue = 70;
			Power = -1;
		}

        /// <summary>
        /// 记录仪编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 分组编号
        /// </summary>
        public int GroupID { get; set; }

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

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { get; set; }

        /// <summary>
        /// 通讯时间(点)
        /// </summary>
        //public int CommunicationTime { get; set; }

        /// <summary>
        /// 记录时间(点)
        /// </summary>
        public int RecordTime { get; set; }

        /// <summary>
        /// 采集间隔(min)
        /// </summary>
        public int PickSpan { get; set; }

        /// <summary>
        /// 记录数量
        /// </summary>
        public int RecordNum
        {
            get
            {
                return 2 * 60 / PickSpan;
            }
        }

        /// <summary>
        /// 报漏幅度值
        /// </summary>
        public int LeakValue { get; set; }

        /// <summary>
        /// 运行状态（0：关 1：开 -1：未知）
        /// </summary>
        public int Power { get; set; }

        /// <summary>
        /// 远传开关（0：关 1：开）
        /// </summary>
        public int ControlerPower { get; set; }

        /// <summary>
        /// 分组状态（0：未分组 1：已分组）
        /// </summary>
        public int GroupState { get; set; }

        /// <summary>
        /// 噪声数据
        /// </summary>
        public NoiseData Data { get; set; }

        /// <summary>
        /// 噪声数据结果
        /// </summary>
        public NoiseResult Result{ get; set; }
    }
}
