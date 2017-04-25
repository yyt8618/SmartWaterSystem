using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class TerMagInfoResqEntity
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
        /// 终端安装列表
        /// </summary>
        public List<TerMagInfoEntity> lstTer = new List<TerMagInfoEntity>();
    }
}
