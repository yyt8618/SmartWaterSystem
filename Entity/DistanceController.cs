using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 远传控制器
    /// </summary>
    public class DistanceController
    {
        /// <summary>
        /// 控制器编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 记录仪编号
        /// </summary>
        public int RecordID { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 远传地址
        /// </summary>
        public string IPAdress { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public int SendTime { get; set; }
    }
}
