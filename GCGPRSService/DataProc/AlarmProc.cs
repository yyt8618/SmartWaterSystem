using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCGPRSService
{
    public class AlarmProc
    {
        /// <summary>
        /// 通过传入的警告标志，返回对应的警告类型（源于AlarmType）名称用于显示
        /// </summary>
        /// <param name="AlarmFlag"></param>
        /// <returns></returns>
        public List<string> GetAlarmName(byte TerminalType,byte FunCode,byte[] AlarmFlag)
        {
            List<string> lstAlarmName = new List<string>();
            if (GlobalValue.Instance.lstAlarmType != null && GlobalValue.Instance.lstAlarmType.Count > 0)
            {

            }

            return lstAlarmName;
        }
    }
}
