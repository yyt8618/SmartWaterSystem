using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoiseAnalysisSystem
{
    /// <summary>
    /// 噪声数据
    /// </summary>
    internal class NoiseData
    {
        /// <summary>
        /// 分组编号
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// 记录仪编号
        /// </summary>
        public int RecorderID { get; set; }

        /// <summary>
        /// 幅度值（%）
        /// </summary>
        public double[] Amplitude { get; set; }

        /// <summary>
        /// 频率值（Hz）
        /// </summary>
        public double[] Frequency { get; set; }

        /// <summary>
        /// 读取时间
        /// </summary>
        public DateTime ReadTime { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UploadTime { get; set; }

        /// <summary>
        /// 上传标志，存放已采集的数目
        /// </summary>
        public int UploadFlag { get; set; }

        /// <summary>
        /// 原始采集数据
        /// </summary>
        public short[] OriginalData { get; set; }
    }
}
