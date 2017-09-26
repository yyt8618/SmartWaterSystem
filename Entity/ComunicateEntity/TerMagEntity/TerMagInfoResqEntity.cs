using System.Collections.Generic;

namespace Entity
{
    public class TerMagInfoResqEntity
    {
        /// <summary>
        /// 调用结果编码
        /// </summary>
        public HttpRespCode code =  HttpRespCode.Fail;
        /// <summary>
        /// 调用结果编码为-1时，返回详细信息
        /// </summary>
        public string msg = "";
        /// <summary>
        /// 终端安装列表
        /// </summary>
        public List<TerMagInfoEntity> lstTer = new List<TerMagInfoEntity>();
    }
}
