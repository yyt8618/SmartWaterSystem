using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoiseAnalysisSystem
{
    /// <summary>
    /// 噪声分析结果
    /// </summary>
    public class NoiseResult
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
        /// 漏水幅度值（%）
        /// </summary>
        public double LeakAmplitude { get; set; }

        /// <summary>
        /// 漏水频率值（Hz）
        /// </summary>
        public double LeakFrequency { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UploadTime { get; set; }

        /// <summary>
        /// 读取时间
        /// </summary>
        public DateTime ReadTime { get; set; }

        /// <summary>
        /// 上传标志
        /// </summary>
        public int UploadFlag { get; set; }

        /// <summary>
        /// 是否漏水（0：不漏水，1：漏水）
        /// </summary>
        public int IsLeak { get; set; }

        /// <summary>
        /// ESA
        /// </summary>
        public int ESA { get; set; }

        /// <summary>
        /// 能量值
        /// </summary>
        public double EnergyValue { get; set; }
    }
}
