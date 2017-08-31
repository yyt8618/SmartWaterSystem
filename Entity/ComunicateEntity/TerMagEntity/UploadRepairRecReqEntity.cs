using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class UploadRepairRecReqEntity
    {
        public string action = "";

        /// <summary>
        /// 维修记录
        /// </summary>
        public RepairInfoEntity repairdata { get; set; }
    }
}
