using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class AlarmProc
    {
        /// <summary>
        /// 通过传入的警告标志，返回对应的警告类型（源于AlarmType）名称用于显示
        /// </summary>
        /// <param name="AlarmFlag"></param>
        /// <returns></returns>
        public static Dictionary<int,string> GetAlarmName(Dictionary<int, string> lstAlarmType, byte TerminalType,byte FunCode,byte AlarmFlaglow, byte AlarmFlaghigh)
        {
            Dictionary<int, string> lstAlarmName = new Dictionary<int, string>();
            if (lstAlarmType != null && lstAlarmType.Count > 0)
            {
                int alarmid = 0;
                string name = "";

                try
                {
                    if ((AlarmFlaglow & 0x01) == 1)
                    {
                        alarmid = BitConverter.ToInt32(new byte[] { 0x01, 0x00, FunCode, TerminalType }, 0);
                        name = lstAlarmType[alarmid];
                        if (!string.IsNullOrEmpty(name))
                            lstAlarmName.Add(alarmid, name);
                    }
                    if (((AlarmFlaglow & 0x02) >> 1) == 1)
                    {
                        alarmid = BitConverter.ToInt32(new byte[] { 0x02, 0x00, FunCode, TerminalType }, 0);
                        name = lstAlarmType[alarmid];
                        if (!string.IsNullOrEmpty(name))
                            lstAlarmName.Add(alarmid, name);
                    }
                    if (((AlarmFlaglow & 0x04) >> 2) == 1)
                    {
                        alarmid = BitConverter.ToInt32(new byte[] { 0x04, 0x00, FunCode, TerminalType }, 0);
                        name = lstAlarmType[alarmid];
                        if (!string.IsNullOrEmpty(name))
                            lstAlarmName.Add(alarmid, name);
                    }
                    if (((AlarmFlaglow & 0x08) >> 3) == 1)
                    {
                        alarmid = BitConverter.ToInt32(new byte[] { 0x08, 0x00, FunCode, TerminalType }, 0);
                        name = lstAlarmType[alarmid];
                        if (!string.IsNullOrEmpty(name))
                            lstAlarmName.Add(alarmid, name);
                    }
                    if (((AlarmFlaglow & 0x10) >> 4) == 1)
                    {
                        alarmid = BitConverter.ToInt32(new byte[] { 0x10, 0x00, FunCode, TerminalType }, 0);
                        name = lstAlarmType[alarmid];
                        if (!string.IsNullOrEmpty(name))
                            lstAlarmName.Add(alarmid, name);
                    }
                    if (((AlarmFlaglow & 0x20) >> 5) == 1)
                    {
                        alarmid = BitConverter.ToInt32(new byte[] { 0x20, 0x00, FunCode, TerminalType }, 0);
                        name = lstAlarmType[alarmid];
                        if (!string.IsNullOrEmpty(name))
                            lstAlarmName.Add(alarmid, name);
                    }
                    if (((AlarmFlaglow & 0x40) >> 6) == 1)
                    {
                        alarmid = BitConverter.ToInt32(new byte[] { 0x40, 0x00, FunCode, TerminalType }, 0);
                        name = lstAlarmType[alarmid];
                        if (!string.IsNullOrEmpty(name))
                            lstAlarmName.Add(alarmid, name);
                    }
                    if (((AlarmFlaglow & 0x80) >> 7) == 1)
                    {
                        alarmid = BitConverter.ToInt32(new byte[] { 0x80, 0x00, FunCode, TerminalType }, 0);
                        name = lstAlarmType[alarmid];
                        if (!string.IsNullOrEmpty(name))
                            lstAlarmName.Add(alarmid, name);
                    }


                    if ((AlarmFlaghigh & 0x01) == 1)
                    {
                        alarmid = BitConverter.ToInt32(new byte[] { 0x00, 0x01, FunCode, TerminalType }, 0);
                        name = lstAlarmType[alarmid];
                        if (!string.IsNullOrEmpty(name))
                            lstAlarmName.Add(alarmid, name);
                    }
                    if (((AlarmFlaghigh & 0x02) >> 1) == 1)
                    {
                        alarmid = BitConverter.ToInt32(new byte[] { 0x00, 0x02, FunCode, TerminalType }, 0);
                        name = lstAlarmType[alarmid];
                        if (!string.IsNullOrEmpty(name))
                            lstAlarmName.Add(alarmid, name);
                    }
                    if (((AlarmFlaghigh & 0x04) >> 2) == 1)
                    {
                        alarmid = BitConverter.ToInt32(new byte[] { 0x00, 0x04, FunCode, TerminalType }, 0);
                        name = lstAlarmType[alarmid];
                        if (!string.IsNullOrEmpty(name))
                            lstAlarmName.Add(alarmid, name);
                    }
                    if (((AlarmFlaghigh & 0x08) >> 3) == 1)
                    {
                        alarmid = BitConverter.ToInt32(new byte[] { 0x00, 0x08, FunCode, TerminalType }, 0);
                        name = lstAlarmType[alarmid];
                        if (!string.IsNullOrEmpty(name))
                            lstAlarmName.Add(alarmid, name);
                    }
                    if (((AlarmFlaghigh & 0x10) >> 4) == 1)
                    {
                        alarmid = BitConverter.ToInt32(new byte[] { 0x00, 0x10, FunCode, TerminalType }, 0);
                        name = lstAlarmType[alarmid];
                        if (!string.IsNullOrEmpty(name))
                            lstAlarmName.Add(alarmid, name);
                    }
                    if (((AlarmFlaghigh & 0x20) >> 5) == 1)
                    {
                        alarmid = BitConverter.ToInt32(new byte[] { 0x00, 0x20, FunCode, TerminalType }, 0);
                        name = lstAlarmType[alarmid];
                        if (!string.IsNullOrEmpty(name))
                            lstAlarmName.Add(alarmid, name);
                    }
                    if (((AlarmFlaghigh & 0x40) >> 6) == 1)
                    {
                        alarmid = BitConverter.ToInt32(new byte[] { 0x00, 0x40, FunCode, TerminalType }, 0);
                        name = lstAlarmType[alarmid];
                        if (!string.IsNullOrEmpty(name))
                            lstAlarmName.Add(alarmid, name);
                    }
                    if (((AlarmFlaghigh & 0x80) >> 7) == 1)
                    {
                        alarmid = BitConverter.ToInt32(new byte[] { 0x00, 0x80, FunCode, TerminalType }, 0);
                        name = lstAlarmType[alarmid];
                        if (!string.IsNullOrEmpty(name))
                            lstAlarmName.Add(alarmid, name);
                    }
                }
                catch { }
            }

            return lstAlarmName;
        }

        /// <summary>
        /// 通过传入的警告标志，返回对应的警告类型（源于AlarmType）名称用于显示
        /// </summary>
        /// <param name="AlarmFlag"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetAlarmName(Dictionary<int, string> lstAlarmType,byte TerminalType, byte FunCode, byte AlarmFlag)
        {
            Dictionary<int, string> lstAlarmName = new Dictionary<int, string>();
            if (lstAlarmType != null && lstAlarmType.Count > 0)
            {
                int alarmid = 0;
                string name = "";

                try {
                    if ((AlarmFlag & 0x01) == 1)
                    {
                        alarmid = BitConverter.ToInt32(new byte[] { 0x01, 0x00, FunCode, TerminalType }, 0);
                        name = lstAlarmType[alarmid];
                        if (!string.IsNullOrEmpty(name))
                            lstAlarmName.Add(alarmid, name);
                    }
                    if (((AlarmFlag & 0x02) >> 1) == 1)
                    {
                        alarmid = BitConverter.ToInt32(new byte[] { 0x02, 0x00, FunCode, TerminalType }, 0);
                        name = lstAlarmType[alarmid];
                        if (!string.IsNullOrEmpty(name))
                            lstAlarmName.Add(alarmid, name);
                    }
                    if (((AlarmFlag & 0x04) >> 2) == 1)
                    {
                        alarmid = BitConverter.ToInt32(new byte[] { 0x04, 0x00, FunCode, TerminalType }, 0);
                        name = lstAlarmType[alarmid];
                        if (!string.IsNullOrEmpty(name))
                            lstAlarmName.Add(alarmid, name);
                    }
                    if (((AlarmFlag & 0x08) >> 3) == 1)
                    {
                        alarmid = BitConverter.ToInt32(new byte[] { 0x08, 0x00, FunCode, TerminalType }, 0);
                        name = lstAlarmType[alarmid];
                        if (!string.IsNullOrEmpty(name))
                            lstAlarmName.Add(alarmid, name);
                    }
                    if (((AlarmFlag & 0x10) >> 4) == 1)
                    {
                        alarmid = BitConverter.ToInt32(new byte[] { 0x10, 0x00, FunCode, TerminalType }, 0);
                        name = lstAlarmType[alarmid];
                        if (!string.IsNullOrEmpty(name))
                            lstAlarmName.Add(alarmid, name);
                    }
                    if (((AlarmFlag & 0x20) >> 5) == 1)
                    {
                        alarmid = BitConverter.ToInt32(new byte[] { 0x20, 0x00, FunCode, TerminalType }, 0);
                        name = lstAlarmType[alarmid];
                        if (!string.IsNullOrEmpty(name))
                            lstAlarmName.Add(alarmid, name);
                    }
                    if (((AlarmFlag & 0x40) >> 6) == 1)
                    {
                        alarmid = BitConverter.ToInt32(new byte[] { 0x40, 0x00, FunCode, TerminalType }, 0);
                        name = lstAlarmType[alarmid];
                        if (!string.IsNullOrEmpty(name))
                            lstAlarmName.Add(alarmid, name);
                    }
                    if (((AlarmFlag & 0x80) >> 7) == 1)
                    {
                        alarmid = BitConverter.ToInt32(new byte[] { 0x80, 0x00, FunCode, TerminalType }, 0);
                        name = lstAlarmType[alarmid];
                        if (!string.IsNullOrEmpty(name))
                            lstAlarmName.Add(alarmid, name);
                    }
                }
                catch
                {

                }
            }

            return lstAlarmName;
        }

    }
}
