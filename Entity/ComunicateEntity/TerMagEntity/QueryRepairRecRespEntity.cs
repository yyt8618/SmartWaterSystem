using System.Collections.Generic;

namespace Entity
{
    public class QueryRepairRecRespEntity
    {
        /// <summary>
        /// 调用结果编码,-1:失败,1:成功
        /// </summary>
        public int code = -1;
        /// <summary>
        /// 调用结果编码为-1时，返回详细信息
        /// </summary>
        public string msg = "";
        /// <summary>
        /// 终端维修记录
        /// </summary>
        public List<RepairInfoEntity> lstRec = new List<RepairInfoEntity>();
    }
}
