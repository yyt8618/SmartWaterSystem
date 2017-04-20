using System;
using System.Collections.Generic;

namespace Entity
{
    public class UploadNoiseDataReqEntity
    {
        public string action = "uploadnoisedata";
        public List<UpLoadNoiseDataEntity> TerData = new List<UpLoadNoiseDataEntity>();
    }
    public class UpLoadNoiseDataEntity
    {
        /// <summary>
        /// 所属组ID
        /// </summary>
        public string GroupId = "";
        /// <summary>
        /// 终端编号
        /// </summary>
        public string TerId = "";
        /// <summary>
        /// 启动值(标准值)
        /// </summary>
        public int cali;
        /// <summary>
        /// 采集的数据(使用','分隔)
        /// </summary>
        public string Data = "";
        /// <summary>
        /// 采集时间
        /// </summary>
        public string ColTime = "";
    }
}
