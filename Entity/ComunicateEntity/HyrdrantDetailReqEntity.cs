using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class HyrdrantDetailReqEntity
    {
        public string action = "";
        /// <summary>
        /// 消防栓Id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 消防栓状态类型,-1为获取全部,其他同HydrantOptType
        /// </summary>
        public int opt { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        public string mintime { get; set; }
        /// <summary>
        /// 结束时间,为空时，使用服务器当前时间
        /// </summary>
        public string maxtime { get; set; }
        /// <summary>
        /// 上传间隔
        /// </summary>
        public int interval { get; set; }
    }
}
