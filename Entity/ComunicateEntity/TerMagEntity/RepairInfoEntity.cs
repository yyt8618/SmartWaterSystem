using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class RepairInfoEntity
    {
        /// <summary>
        /// Id TerManagerInfo表ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public int DevId { get; set; }

        /// <summary>
        /// 故障类型ID
        /// </summary>
        public int BreakdownId { get; set; }

        /// <summary>
        /// 故障描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 照片路径
        /// </summary>
        public List<string> PicsPath { get; set; }

        /// <summary>
        /// 维修人员ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 维修人员名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 维修时间
        /// </summary>
        public string RepairTime { get; set; }
    }
}
