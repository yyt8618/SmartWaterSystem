using System.Collections.Generic;

namespace Entity
{
    public class TerBreakdownInfoResqEntity
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
        /// 故障类型列表
        /// </summary>
        public List<TerBreakdownInfoEntity> lsttype = new List<TerBreakdownInfoEntity>();
    }
}
