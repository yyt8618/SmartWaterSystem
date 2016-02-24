using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class UploadFlowDataReqEntity
    {
        public string action = "uploadflowdata";
        public List<UpLoadFlowDataEntity> TerData = new List<UpLoadFlowDataEntity>();
    }
    public class UpLoadFlowDataEntity
    {
        /// <summary>
        /// 终端编号
        /// </summary>
        public string terid = "";
        /// <summary>
        /// 正向累积流量
        /// </summary>
        public string flowvalue = "";
        /// <summary>
        /// 反向累积流量
        /// </summary>
        public string flowinverted = "";
        /// <summary>
        /// 瞬时流量
        /// </summary>
        public string flowinstant = "";
        /// <summary>
        /// 采集时间
        /// </summary>
        public string collTime = "";
    }
}
