using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class AlarmTypeEntity
    {
        /// <summary>
        /// 终端类型
        /// </summary>
        public byte TerminalType { get; set; }

        /// <summary>
        /// 功能码
        /// </summary>
        public byte FunCode { get; set; }

        /// <summary>
        /// 报警标志(2byte)
        /// </summary>
        public byte[] AlarmFlag { get; set; }

        /// <summary>
        /// 报警表ID(int) (TerminalType+FunCode+AlarmFlag(2byte)组成)
        /// </summary>
        public int AlarmId { get; set; }

        /// <summary>
        /// 报警名称
        /// </summary>
        public string AlarmName { get; set; }
        
    }
}
