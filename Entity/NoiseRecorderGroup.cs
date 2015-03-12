using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Entity
{
    /// <summary>
    /// 噪声记录仪分组
    /// </summary>
    public class NoiseRecorderGroup
    {
        public NoiseRecorderGroup()
        {
            RecorderList = new List<NoiseRecorder>();
        }

        /// <summary>
        /// 分组编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 记录仪列表
        /// </summary>
        public List<NoiseRecorder> RecorderList { get; set; }
    }
}
