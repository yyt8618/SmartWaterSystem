using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class GetHydrantDetailRespEntity
    {
        /// <summary>
        /// 调用结果编码
        /// </summary>
        public HttpRespCode code = HttpRespCode.Fail;
        /// <summary>
        /// 调用结果编码为-1时，返回详细信息
        /// </summary>
        public string msg = "";
        /// <summary>
        /// 消防栓数据明细
        /// </summary>
        public List<HydrantDataDetailEntity> lstData = new List<HydrantDataDetailEntity>();
    }
}
