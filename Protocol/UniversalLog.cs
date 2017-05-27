using System;
using System.Collections.Generic;
using Common;
using System.Data;
using Entity;

namespace Protocol
{
    public class UniversalLog : RWData
    {
        public bool Reset(ConstValue.DEV_TYPE devtype,short Id,byte type)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.RESET;
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = type;
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetTime(ConstValue.DEV_TYPE devtype, short Id, DateTime dt)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.SET_TIME;
            byte[] data = new byte[6];
            data[0] = (byte)(dt.Year-2000);
            data[1] = (byte)dt.Month;
            data[2] = (byte)dt.Day;
            data[3] = (byte)dt.Hour;
            data[4] = (byte)dt.Minute;
            data[5] = (byte)dt.Second;
            package.DataLength = data.Length;
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool EnableCollect(ConstValue.DEV_TYPE devtype, short Id)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.EnableCollect;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool EnableAlarm(ConstValue.DEV_TYPE devtype, short Id,bool Enable)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.EnableAlarm;
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];  
            data[0] = Enable ? (byte)0x00 : (byte)0x01;  //1--取消     0--不取消
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool CalibartionSimulate1(ConstValue.DEV_TYPE devtype, short Id)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.CalibartionSimualte1;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool CalibartionSimulate2(ConstValue.DEV_TYPE devtype, short Id)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.CalibartionSimualte2;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetID(ConstValue.DEV_TYPE devtype, short Id)
        {
            Package package = new Package();
            package.DevType = devtype;
            //package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.SET_ID;
            byte[] data = BitConverter.GetBytes((int)Id);
            Array.Reverse(data);
            data[0] = (byte)(devtype);
            package.DataLength = data.Length;
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool Set(ConstValue.DEV_TYPE devtype, short Id, byte funcode, byte[] data)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = funcode;

            package.DataLength = data.Length;
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool Set(ConstValue.DEV_TYPE devtype, short Id, byte funcode, byte data)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = funcode;

            package.DataLength = 1;
            package.Data = new byte[] { data };
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetIP(ConstValue.DEV_TYPE devtype, short id, int[] ips)
        {
            string ip = "";
            for (int i = 0; i < ips.Length; i++)
            {
                ip += ips[i].ToString().PadLeft(3, '0') + ".";
            }
            if (!string.IsNullOrEmpty(ip))
                ip = ip.Substring(0, ip.Length - 1);
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.SET_IP;
            package.DataLength = 15;
            byte[] data = new byte[package.DataLength];
            for (int i = 0; i < ip.Length; i++)
            {
                data[i] = (byte)ip[i];
            }
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetCollectConfig(ConstValue.DEV_TYPE devtype, short Id, byte config)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.SET_COLLECTCONFIG;
            byte[] data = new byte[1];
            data[0] = config;
            package.DataLength = data.Length;
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        /// <summary>
        /// 设置限值
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="limit"></param>
        /// <param name="range">量程</param>
        /// <param name="flagtype"></param>
        /// <param name="alarmtype"></param>
        public bool SetLimit(ConstValue.DEV_TYPE devtype, short Id, double limit, double range, UniversalFlagType flagtype, UniversalAlarmType alarmtype)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            byte[] data = new byte[3];
            byte flag = 0x00;
            switch (flagtype)
            {
                case UniversalFlagType.Pressure1:
                    if (alarmtype == UniversalAlarmType.UpAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.SET_PREUPLIMIT;
                    else if (alarmtype == UniversalAlarmType.LowAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.SET_PRELOWLIMIT;
                    else if (alarmtype == UniversalAlarmType.SlopUpAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.SET_PRESLOPUPLIMIT;
                    else if (alarmtype == UniversalAlarmType.SlopLowAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.SET_PRESLOPLOWLIMIT;
                    flag = 0x01;
                    data = new byte[3];
                    Array.Copy(BitConverter.GetBytes((short)(limit * 1000)), data, 2);
                    Array.Reverse(data);
                    data[0] = flag;
                    break;
                case UniversalFlagType.Pressure2:
                    if (alarmtype == UniversalAlarmType.UpAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.SET_PREUPLIMIT;
                    else if (alarmtype == UniversalAlarmType.LowAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.SET_PRELOWLIMIT;
                    else if (alarmtype == UniversalAlarmType.SlopUpAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.SET_PRESLOPUPLIMIT;
                    else if (alarmtype == UniversalAlarmType.SlopLowAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.SET_PRESLOPLOWLIMIT;
                    flag = 0x02;
                    data = new byte[3];
                    Array.Copy(BitConverter.GetBytes((short)(limit * 1000)), data, 2);
                    Array.Reverse(data);
                    data[0] = flag;
                    break;
                case UniversalFlagType.Simulate1:
                    if (alarmtype == UniversalAlarmType.UpAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.SET_SIMUPLIMIT;
                    else if (alarmtype == UniversalAlarmType.LowAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.SET_SIMLOWLIMIT;
                    else if (alarmtype == UniversalAlarmType.SlopUpAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.SET_SIMSLOPUPLIMIT;
                    else if (alarmtype == UniversalAlarmType.SlopLowAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.SET_SIMSLOPLOWLIMIT;
                    flag = 0x01;
                    data = new byte[3];
                    Array.Copy(BitConverter.GetBytes((short)Math.Round(ConstValue.UniversalSimRatio * limit / range)), data, 2);
                    Array.Reverse(data);
                    data[0] = flag;
                    break;
                case UniversalFlagType.Simulate2:
                    if (alarmtype == UniversalAlarmType.UpAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.SET_SIMUPLIMIT;
                    else if (alarmtype == UniversalAlarmType.LowAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.SET_SIMLOWLIMIT;
                    else if (alarmtype == UniversalAlarmType.SlopUpAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.SET_SIMSLOPUPLIMIT;
                    else if (alarmtype == UniversalAlarmType.SlopLowAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.SET_SIMSLOPLOWLIMIT;
                    flag = 0x02;
                    data = new byte[3];
                    Array.Copy(BitConverter.GetBytes((short)Math.Round(ConstValue.UniversalSimRatio * limit / range)), data, 2);
                    Array.Reverse(data);
                    data[0] = flag;
                    break;
                case UniversalFlagType.Flow:
                    if (alarmtype == UniversalAlarmType.UpAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.SET_FLOWUPLIMIT;
                    else if (alarmtype == UniversalAlarmType.LowAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.SET_FLOWLOWLIMIT;
                    else if (alarmtype == UniversalAlarmType.SlopUpAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.SET_FLOWSLOPUPLIMIT;
                    else if (alarmtype == UniversalAlarmType.SlopLowAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.SET_FLOWSLOPLOWLIMIT;
                    flag = 0x01;
                    data = new byte[5];
                    Array.Copy(BitConverter.GetBytes((int)(limit * 1000)), data, 4);
                    Array.Reverse(data);
                    data[0] = flag;
                    break;
                case UniversalFlagType.Level:
                    if (alarmtype == UniversalAlarmType.UpAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.SET_LEVELUPLIMIT;
                    else if (alarmtype == UniversalAlarmType.LowAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.SET_LEVELLOWLIMIT;
                    //else if (alarmtype == UniversalAlarmType.SlopUpAlarm)
                    //    package.C1 = (byte)UNIVERSAL_COMMAND.SET_FLOWSLOPUPLIMIT;
                    //else if (alarmtype == UniversalAlarmType.SlopLowAlarm)
                    //    package.C1 = (byte)UNIVERSAL_COMMAND.SET_FLOWSLOPLOWLIMIT;
                    flag = 0x01;
                    data = new byte[2];
                    Array.Copy(BitConverter.GetBytes((short)(limit * 1000)), data, 2);
                    Array.Reverse(data);
                    //data[0] = flag;   //分体式液位没有flag
                    break;
            }
            package.DataLength = data.Length;

            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }
        public bool SetAlarmEnable(ConstValue.DEV_TYPE devtype, short Id, bool enable, UniversalFlagType flagtype, UniversalAlarmType alarmtype)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            if (alarmtype == UniversalAlarmType.UpAlarm)
                package.C1 = (byte)UNIVERSAL_COMMAND.SET_UPENABLE;
            else if (alarmtype == UniversalAlarmType.LowAlarm)
                package.C1 = (byte)UNIVERSAL_COMMAND.SET_LOWENABLE;
            else if (alarmtype == UniversalAlarmType.SlopUpAlarm)
                package.C1 = (byte)UNIVERSAL_COMMAND.SET_SLOPUPENABLE;
            else if (alarmtype == UniversalAlarmType.SlopLowAlarm)
                package.C1 = (byte)UNIVERSAL_COMMAND.SET_SLOPLOWENABLE;
            byte flag = 0x00;
            switch (flagtype)
            {
                case UniversalFlagType.Pressure1:
                    flag = 0x01;
                    break;
                case UniversalFlagType.Pressure2:
                    flag = 0x02;
                    break;
                case UniversalFlagType.Simulate1:
                    flag = 0x03;
                    break;
                case UniversalFlagType.Simulate2:
                    flag = 0x04;
                    break;
                case UniversalFlagType.Flow:
                    flag = 0x05;
                    break;
                case UniversalFlagType.Level:
                    flag = 0x06;
                    break;
            }
            byte[] data = new byte[2];
            data[0] = flag;
            data[1] = (byte)(enable ? 0x01 : 0x00);
            package.DataLength = data.Length;
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetRange(ConstValue.DEV_TYPE devtype, short Id, double range, UniversalFlagType flagtype)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            byte flag = 0x00;
            byte[] data = new byte[3];
            switch (flagtype)
            {
                case UniversalFlagType.Pressure1:
                    package.C1 = (byte)UNIVERSAL_COMMAND.SET_PRERANGE;
                    flag = 0x01;
                    data = new byte[3];
                    Array.Copy(BitConverter.GetBytes((short)(range * 1000)), data, 2);
                    Array.Reverse(data);
                    data[0] = flag;
                    break;
                case UniversalFlagType.Pressure2:
                    package.C1 = (byte)UNIVERSAL_COMMAND.SET_PRERANGE;
                    flag = 0x02;
                    data = new byte[3];
                    Array.Copy(BitConverter.GetBytes((short)(range * 1000)), data, 2);
                    Array.Reverse(data);
                    data[0] = flag;
                    break;
                case UniversalFlagType.Simulate1:
                    package.C1 = (byte)UNIVERSAL_COMMAND.SET_SIMRANGE;
                    flag = 0x01;
                    data = new byte[5];
                    short round = (short)range; //整数部分
                    short deci = (short)Math.Round((range - round) * 1000);   //小数部分*1000
                    Array.Copy(BitConverter.GetBytes(deci), data, 2);
                    Array.Copy(BitConverter.GetBytes(round), 0, data, 2, 2);
                    Array.Reverse(data);
                    data[0] = flag;
                    break;
                case UniversalFlagType.Simulate2:
                    package.C1 = (byte)UNIVERSAL_COMMAND.SET_SIMRANGE;
                    flag = 0x02;
                    data = new byte[5];
                    round = (short)range; //整数部分
                    deci = (short)Math.Round((range - round) * 1000);   //小数部分*1000
                    Array.Copy(BitConverter.GetBytes(deci), data, 2);
                    Array.Copy(BitConverter.GetBytes(round), 0, data, 2, 2);
                    Array.Reverse(data);
                    data[0] = flag;
                    break;
                case UniversalFlagType.Level:
                    package.C1 = (byte)UNIVERSAL_COMMAND.SET_LEVELRANGE;
                    flag = 0x01;
                    data = new byte[2];
                    Array.Copy(BitConverter.GetBytes((short)(range * 1000)), data, 2);
                    Array.Reverse(data);
                    //data[0] = flag;   //分体式液位没有flag
                    break;
                    //case UniversalFlagType.Flow:  //无流量
                    //    flag = 0x01;
                    //    break;
            }
            package.DataLength = data.Length;
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetPreOffsetBase(ConstValue.DEV_TYPE devtype, short Id, double offset, UniversalFlagType flagtype)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.SET_PREOFFSET;
            byte flag = 0x00;
            byte[] data = new byte[3];
            switch (flagtype)       //偏移值只有压力和分体式液位有，模拟量和流量没有
            {
                case UniversalFlagType.Pressure1:
                    flag = 0x01;
                    Array.Copy(BitConverter.GetBytes((short)(offset * 1000)), data, 2);
                    Array.Reverse(data);
                    data[0] = flag;
                    break;
                case UniversalFlagType.Pressure2:
                    flag = 0x02;
                    Array.Copy(BitConverter.GetBytes((short)(offset * 1000)), data, 2);
                    Array.Reverse(data);
                    data[0] = flag;
                    break;
                case UniversalFlagType.Level:
                    package.C1 = (byte)UNIVERSAL_COMMAND.SET_LEVELBASE;  //分体式液位基值
                    data = new byte[2];
                    Array.Copy(BitConverter.GetBytes((short)(offset * 1000)), data, 2);
                    Array.Reverse(data);
                    //flag = 0x01;   //分体式液位没有flag
                    break;
            }
            package.DataLength = data.Length;
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetCallEnable(ConstValue.DEV_TYPE devtype, short Id, bool Enable)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.SET_CALLENABLE;
            byte[] data = new byte[1];
            data[0] = Enable ? (byte)0x01 : (byte)0x00; //1-使能招测 0 - 关闭招测
            package.DataLength = data.Length;
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetSimulateInterval(ConstValue.DEV_TYPE devtype, short Id, DataTable dt)
        {
            Package package = GetSimulateIntervalPackage(devtype, Id, dt);

            return Write(package);
        }

        public Package GetSimulateIntervalPackage(ConstValue.DEV_TYPE devtype, short Id, DataTable dt)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.SET_SIMINTERVAL;
            List<byte> lstdata = new List<byte>();
            byte[] data;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["starttime"] != DBNull.Value && dr["sendtime"] != DBNull.Value && dr["collecttime1"] != DBNull.Value && dr["collecttime2"] != DBNull.Value)
                {
                    lstdata.Add(Convert.ToByte(dr["starttime"]));
                    data = BitConverter.GetBytes(Convert.ToInt16(dr["sendtime"]));
                    lstdata.Add(data[1]);
                    lstdata.Add(data[0]);
                    data = BitConverter.GetBytes(Convert.ToInt16(dr["collecttime1"]));
                    lstdata.Add(data[1]);
                    lstdata.Add(data[0]);
                    data = BitConverter.GetBytes(Convert.ToInt16(dr["collecttime2"]));
                    lstdata.Add(data[1]);
                    lstdata.Add(data[0]);
                }
            }

            data = lstdata.ToArray();
            package.DataLength = data.Length;
            package.Data = data;
            package.CS = package.CreateCS();

            return package;
        }

        public bool SetPluseInterval(ConstValue.DEV_TYPE devtype, short Id, DataTable dt)
        {
            Package package = GetPluseIntervalPackage(devtype, Id, dt);

            return Write(package);
        }

        public Package GetPluseIntervalPackage(ConstValue.DEV_TYPE devtype, short Id, DataTable dt)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.SET_PLUSEINTERVAL;
            List<byte> lstdata = new List<byte>();
            byte[] data;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["starttime"] != DBNull.Value && dr["sendtime"] != DBNull.Value && dr["precollecttime"] != DBNull.Value && dr["plusecollecttime"] != DBNull.Value)
                {
                    lstdata.Add(Convert.ToByte(dr["starttime"]));
                    data = BitConverter.GetBytes(Convert.ToInt16(dr["sendtime"]));
                    lstdata.Add(data[1]);
                    lstdata.Add(data[0]);
                    data = BitConverter.GetBytes(Convert.ToInt16(dr["precollecttime"]));
                    lstdata.Add(data[1]);
                    lstdata.Add(data[0]);
                    data = BitConverter.GetBytes(Convert.ToInt16(dr["plusecollecttime"]));
                    lstdata.Add(data[1]);
                    lstdata.Add(data[0]);
                }
            }

            data = lstdata.ToArray();
            package.DataLength = data.Length;
            package.Data = data;
            package.CS = package.CreateCS();

            return package;
        }

        public bool SetRS485Interval(ConstValue.DEV_TYPE devtype, short Id, DataTable dt)
        {
            Package package = GetRS485IntervalPackage(devtype, Id, dt);

            return Write(package);
        }

        public Package GetRS485IntervalPackage(ConstValue.DEV_TYPE devtype, short Id, DataTable dt)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.SET_485INTERVAL;
            List<byte> lstdata = new List<byte>();
            byte[] data;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["starttime"] != DBNull.Value && dr["sendtime"] != DBNull.Value && dr["collecttime"] != DBNull.Value)
                {
                    lstdata.Add(Convert.ToByte(dr["starttime"]));
                    data = BitConverter.GetBytes(Convert.ToInt16(dr["sendtime"]));
                    lstdata.Add(data[1]);
                    lstdata.Add(data[0]);
                    data = BitConverter.GetBytes(Convert.ToInt16(dr["collecttime"]));
                    lstdata.Add(data[1]);
                    lstdata.Add(data[0]);
                }
            }

            data = lstdata.ToArray();
            package.DataLength = data.Length;
            package.Data = data;
            package.CS = package.CreateCS();

            return package;
        }

        public bool SetModbusProtocol(ConstValue.DEV_TYPE devtype, short Id, DataTable dt)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.SET_MODBUSPROTOCOL;
            List<byte> lstdata = new List<byte>();
            byte[] data;
            int validrow = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                if (dr["baud"] != DBNull.Value && dr["ID"] != DBNull.Value && dr["funcode"] != DBNull.Value &&
                    dr["regbeginaddr"] != DBNull.Value && dr["regcount"] != DBNull.Value)
                {
                    int baud = Convert.ToInt32(dr["baud"]);
                    if (baud == 1200)
                        lstdata.Add(0x00);
                    else if (baud == 2400)
                        lstdata.Add(0x01);
                    else if (baud == 4800)
                        lstdata.Add(0x02);
                    else if (baud == 9600)
                        lstdata.Add(0x03);

                    string id = dr["ID"].ToString().Trim();
                    if (id.StartsWith("0x") && id.Length > 2)
                        lstdata.Add(Convert.ToByte(id.Substring(2), 16));
                    else
                        lstdata.Add(Convert.ToByte(dr["ID"]));

                    string funcode = dr["funcode"].ToString().Trim();
                    if (funcode.StartsWith("0x") && funcode.Length > 2)
                        lstdata.Add(Convert.ToByte(funcode.Substring(2), 16));
                    else
                        lstdata.Add(Convert.ToByte(dr["funcode"]));

                    string regbeginaddr = dr["regbeginaddr"].ToString().Trim();
                    if (regbeginaddr.StartsWith("0x") && regbeginaddr.Length > 2)
                    {
                        data = BitConverter.GetBytes(Convert.ToInt16(regbeginaddr.Substring(2), 16));
                        lstdata.Add(data[1]);
                        lstdata.Add(data[0]);
                    }
                    else
                    {
                        data = BitConverter.GetBytes(Convert.ToInt16(dr["regbeginaddr"]));
                        lstdata.Add(data[1]);
                        lstdata.Add(data[0]);
                    }

                    string regcount = dr["regcount"].ToString().Trim();
                    if (regcount.StartsWith("0x") && regcount.Length > 2)
                    {
                        data = BitConverter.GetBytes(Convert.ToInt16(regcount.Substring(2), 16));
                        lstdata.Add(data[1]);
                        lstdata.Add(data[0]);
                    }
                    else
                    {
                        data = BitConverter.GetBytes(Convert.ToInt16(dr["regcount"]));
                        lstdata.Add(data[1]);
                        lstdata.Add(data[0]);
                    }
                    validrow++;
                }
            }
            for (; validrow < 4; validrow++)
            {
                lstdata.AddRange(new byte[] { 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });  //补齐行
            }

            data = lstdata.ToArray();
            package.DataLength = data.Length;
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        /// <summary>
        /// 设置第?路脉冲基准数
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="value">基准数</param>
        /// <param name="waytype">1:第一路脉冲,2:第二路脉冲,3:第三路脉冲,4:第四路脉冲</param>
        /// <returns></returns>
        public bool SetPluseBasic(ConstValue.DEV_TYPE devtype, short Id, UInt32 value, int waytype)
        {
            Package package = GetPluseBasicPackage(devtype, Id, value, waytype);

            return Write(package);
        }

        public short ReadId(ConstValue.DEV_TYPE devtype)
        {
            Package package = new Package();
            package.DevType = devtype;
            //package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_ID;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package);
            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            if (result.Data.Length != 4)
            {
                throw new Exception("数据损坏");
            }
            return result.DevID;

        }

        public DateTime ReadTime(ConstValue.DEV_TYPE devtype, short Id)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_TIME;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package);
            if (RWType == RWFunType.GPRS)
                return DateTime.Now;

            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            if (result.Data.Length != 6)
            {
                throw new Exception("数据损坏");
            }
            return new DateTime(Convert.ToInt32(result.Data[0])+2000, Convert.ToInt32(result.Data[1]), Convert.ToInt32(result.Data[2]),
                Convert.ToInt32(result.Data[3]), Convert.ToInt32(result.Data[4]), Convert.ToInt32(result.Data[5]));
        }

        public string ReadVer(ConstValue.DEV_TYPE devtype, short Id)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_VER;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package);
            if (RWType == RWFunType.GPRS)
                return "";

            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            if (result.Data.Length <5)
            {
                throw new Exception("数据损坏");
            }
            return System.Text.Encoding.Default.GetString(result.Data);
        }

        public string ReadFieldStrength(ConstValue.DEV_TYPE devtype, short Id)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_FIELDSTRENGTH;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package, 35, 1);
            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            if (result.Data.Length != 3)
            {
                throw new Exception("数据损坏");
            } 
            return "场强:"+result.Data[0]+",电压:"+ ((float)BitConverter.ToInt16(new byte[] { result.Data[2], result.Data[1] }, 0)) / 1000; 
        }

        public string ReadCellPhone(ConstValue.DEV_TYPE devtype, short Id)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_CELLPHONE;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package);
            if (RWType == RWFunType.GPRS)
                return "";
            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            if (result.Data.Length != 11)
            {
                throw new Exception("数据损坏");
            }
            string number = "";
            for (int i = 0; i < result.DataLength; i++)
            {
                number += (char)result.Data[i];
            }
            return number;
        }

        public bool ReadModbusExeFlag(ConstValue.DEV_TYPE devtype, short Id)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_MODBUSEXEFLAG;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package);
            if (RWType == RWFunType.GPRS)
                return true;

            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            if (result.Data.Length != 1)
            {
                throw new Exception("数据损坏");
            }
            return (Convert.ToInt32(result.Data[0]) == 1) ? true : false;
        }

        public int Read485Baud(ConstValue.DEV_TYPE devtype, short Id)
        {
            Package package = new Package();
            package.DevType =devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_485BAUD;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package);
            if (RWType == RWFunType.GPRS)
                return 0;
            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            if (result.Data.Length != 1)
            {
                throw new Exception("数据损坏");
            }
            return Convert.ToInt32(result.Data[0]);
        }

        public int ReadNetworkType(ConstValue.DEV_TYPE devtype, short Id)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_NETWORKTYPE;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package);
            if (RWType == RWFunType.GPRS)
                return 0;
            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            if (result.Data.Length != 1)
            {
                throw new Exception("数据损坏");
            }
            return Convert.ToInt32(result.Data[0]);
        }

        public double ReadLimit(ConstValue.DEV_TYPE devtype, short Id, UniversalFlagType flagtype, UniversalAlarmType alarmtype)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            byte flag = 0x00;
            switch(flagtype)
            {
                case UniversalFlagType.Pressure1:
                    if (alarmtype == UniversalAlarmType.UpAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.READ_PREUPLIMIT;
                    else if (alarmtype == UniversalAlarmType.LowAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.READ_PRELOWLIMIT;
                    else if (alarmtype == UniversalAlarmType.SlopUpAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.READ_PRESLOPUPLIMIT;
                    else if (alarmtype == UniversalAlarmType.SlopLowAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.READ_PRESLOPLOWLIMIT;
                    flag = 0x01;
                    break;
                case UniversalFlagType.Pressure2:
                    if (alarmtype == UniversalAlarmType.UpAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.READ_PREUPLIMIT;
                    else if (alarmtype == UniversalAlarmType.LowAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.READ_PRELOWLIMIT;
                    else if (alarmtype == UniversalAlarmType.SlopUpAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.READ_PRESLOPUPLIMIT;
                    else if (alarmtype == UniversalAlarmType.SlopLowAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.READ_PRESLOPLOWLIMIT;
                    flag = 0x02;
                    break;
                case UniversalFlagType.Simulate1:
                    if(alarmtype == UniversalAlarmType.UpAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.READ_SIMUPLIMIT;
                    else if (alarmtype == UniversalAlarmType.LowAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.READ_SIMLOWLIMIT;
                    else if (alarmtype == UniversalAlarmType.SlopUpAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.READ_SIMSLOPUPLIMIT;
                    else if (alarmtype == UniversalAlarmType.SlopLowAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.READ_SIMSLOPLOWLIMIT;
                    flag = 0x01;
                    break;
                case UniversalFlagType.Simulate2:
                    if (alarmtype == UniversalAlarmType.UpAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.READ_SIMUPLIMIT;
                    else if (alarmtype == UniversalAlarmType.LowAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.READ_SIMLOWLIMIT;
                    else if (alarmtype == UniversalAlarmType.SlopUpAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.READ_SIMSLOPUPLIMIT;
                    else if (alarmtype == UniversalAlarmType.SlopLowAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.READ_SIMSLOPLOWLIMIT;
                    flag = 0x02;
                    break;
                case UniversalFlagType.Flow:
                    if (alarmtype == UniversalAlarmType.UpAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.READ_FLOWUPLIMIT;
                    else if (alarmtype == UniversalAlarmType.LowAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.READ_FLOWLOWLIMIT;
                    else if (alarmtype == UniversalAlarmType.SlopUpAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.READ_FLOWSLOPUPLIMIT;
                    else if (alarmtype == UniversalAlarmType.SlopLowAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.READ_FLOWSLOPLOWLIMIT;
                    flag = 0x01;
                    break;
                case UniversalFlagType.Level:
                    if (alarmtype == UniversalAlarmType.UpAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.READ_LEVELUPLIMIT;
                    else if (alarmtype == UniversalAlarmType.LowAlarm)
                        package.C1 = (byte)UNIVERSAL_COMMAND.READ_LEVELLOWLIMIT;
                    //else if (alarmtype == UniversalAlarmType.SlopUpAlarm)
                    //    package.C1 = (byte)UNIVERSAL_COMMAND.READ_LEVELSLOPUPLIMIT;
                    //else if (alarmtype == UniversalAlarmType.SlopLowAlarm)
                    //    package.C1 = (byte)UNIVERSAL_COMMAND.READ_LEVELSLOPLOWLIMIT;
                    flag = 0x01;
                    break;
            }
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = flag;
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package);
            if (RWType == RWFunType.GPRS)
                return 0;
            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            switch (flagtype)
            {
                case UniversalFlagType.Pressure1:
                case UniversalFlagType.Pressure2:
                    if (result.Data.Length != 3)
                    {
                        throw new Exception("数据损坏");
                    }
                    return ((double)BitConverter.ToInt16(new byte[] { result.Data[2], result.Data[1] }, 0))/1000;
                case UniversalFlagType.Simulate1:
                case UniversalFlagType.Simulate2:
                    if (result.Data.Length != 7)
                    {
                        throw new Exception("数据损坏");
                    }
                    double range = 0;
                    range += BitConverter.ToInt16(new byte[] { result.Data[2], result.Data[1] }, 0);    //整数部分
                    range += ((double)BitConverter.ToInt16(new byte[] { result.Data[4], result.Data[3] }, 0)) / 1000;    //小数部分
                    return ((double)(BitConverter.ToInt16(new byte[] { result.Data[6], result.Data[5] }, 0))) * range / (ConstValue.UniversalSimRatio);
                case UniversalFlagType.Flow:
                    if (result.Data.Length != 5)
                    {
                        throw new Exception("数据损坏");
                    }
                    return ((double)BitConverter.ToInt32(new byte[] { result.Data[4], result.Data[3], result.Data[2], result.Data[1] }, 0))/1000;
                case UniversalFlagType.Level:
                    if (result.Data.Length != 2)
                    {
                        throw new Exception("数据损坏");
                    }
                    return ((double)BitConverter.ToInt16(new byte[] { result.Data[1], result.Data[0] }, 0)) / 1000;
                default:
                    return 0;
            }
        }
        
        public bool ReadAlarmEnable(ConstValue.DEV_TYPE devtype, short Id, UniversalFlagType flagtype, UniversalAlarmType alarmtype)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            if (alarmtype == UniversalAlarmType.UpAlarm)
                package.C1 = (byte)UNIVERSAL_COMMAND.READ_UPENABLE;
            else if (alarmtype == UniversalAlarmType.LowAlarm)
                package.C1 = (byte)UNIVERSAL_COMMAND.READ_LOWENABLE;
            else if (alarmtype == UniversalAlarmType.SlopUpAlarm)
                package.C1 = (byte)UNIVERSAL_COMMAND.READ_SLOPUPENABLE;
            else if (alarmtype == UniversalAlarmType.SlopLowAlarm)
                package.C1 = (byte)UNIVERSAL_COMMAND.READ_SLOPLOWENABLE;
            byte flag = 0x00;
            switch (flagtype)
            {
                case UniversalFlagType.Pressure1:
                    flag = 0x01;
                    break;
                case UniversalFlagType.Pressure2:
                    flag = 0x02;
                    break;
                case UniversalFlagType.Simulate1:
                    flag = 0x03;
                    break;
                case UniversalFlagType.Simulate2:
                    flag = 0x04;
                    break;
                case UniversalFlagType.Flow:
                    flag = 0x05;
                    break;
                case UniversalFlagType.Level:
                    flag = 0x06;
                    break;
            }
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = flag;
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package);
            if (RWType == RWFunType.GPRS)
                return true;

            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            if (result.Data.Length != 2)
            {
                throw new Exception("数据损坏");
            }
            return (result.Data[1] == 0x01) ? true : false;
        }

        public double ReadRange(ConstValue.DEV_TYPE devtype, short Id, UniversalFlagType flagtype)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            byte flag = 0x00;
            switch (flagtype)
            {
                case UniversalFlagType.Pressure1:
                    package.C1 = (byte)UNIVERSAL_COMMAND.READ_PRERANGE;
                    flag = 0x01;
                    break;
                case UniversalFlagType.Pressure2:
                    package.C1 = (byte)UNIVERSAL_COMMAND.READ_PRERANGE;
                    flag = 0x02;
                    break;
                case UniversalFlagType.Simulate1:
                    package.C1 = (byte)UNIVERSAL_COMMAND.READ_SIMRANGE;
                    flag = 0x01;
                    break;
                case UniversalFlagType.Simulate2:
                    package.C1 = (byte)UNIVERSAL_COMMAND.READ_SIMRANGE;
                    flag = 0x02;
                    break;
                case UniversalFlagType.Level:
                    package.C1 = (byte)UNIVERSAL_COMMAND.READ_LEVELRANGE;
                    flag = 0x01;        //分体式液位不需要这个，有也不影响
                    break;
                    //case UniversalFlagType.Flow:  //无流量
                    //    flag = 0x01;
                    //    break;
            }
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = flag;
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package);
            if (RWType == RWFunType.GPRS)
                return 0;
            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            if (flagtype == UniversalFlagType.Pressure1 || flagtype == UniversalFlagType.Pressure2)   //压力、分体式液位
            {
                if (result.Data.Length != 3)
                {
                    throw new Exception("数据损坏");
                }
                return ((double)(BitConverter.ToInt16(new byte[] { result.Data[2], result.Data[1] }, 0))) /1000;
            }
            else if (flagtype == UniversalFlagType.Level)   //分体式液位
            {
                if (result.Data.Length != 2)
                {
                    throw new Exception("数据损坏");
                }
                return ((double)(BitConverter.ToInt16(new byte[] { result.Data[1], result.Data[0] }, 0))) / 1000;
            }
            else     //模拟量
            {
                if (result.Data.Length != 5)
                {
                    throw new Exception("数据损坏");
                }
                double range = 0;
                range += BitConverter.ToInt16(new byte[] { result.Data[2], result.Data[1] }, 0);    //整数部分
                range += ((double)BitConverter.ToInt16(new byte[] { result.Data[4], result.Data[3] }, 0))/1000;    //小数部分
                return range;
            }
        }

        public double ReadPreOffsetBase(ConstValue.DEV_TYPE devtype, short Id, UniversalFlagType flagtype)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_PREOFFSET;
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            byte flag = 0x00;
            if (flagtype == UniversalFlagType.Pressure1)
                flag = 0x01;
            else if (flagtype == UniversalFlagType.Pressure2)
                flag = 0x02;
            else if (flagtype == UniversalFlagType.Level)
            {
                //flag = 0x01;   //分体式液位没有flag
                package.C1 = (byte)UNIVERSAL_COMMAND.READ_LEVELBASE;  //读取分体式液位基值
            }
            data[0] = flag;
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package);
            if (RWType == RWFunType.GPRS)
                return 0;

            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            if (flagtype != UniversalFlagType.Level)
            {
                if (result.Data.Length != 3)
                {
                    throw new Exception("数据损坏");
                }
                return ((double)BitConverter.ToInt16(new byte[] { result.Data[2], result.Data[1] }, 0)) / 1000;
            }
            else
            {
                if (result.Data.Length != 2)
                {
                    throw new Exception("数据损坏");
                }
                return ((double)BitConverter.ToInt16(new byte[] { result.Data[1], result.Data[0] }, 0)) / 1000;
            }
            
        }

        public int ReadComType(ConstValue.DEV_TYPE devtype, short Id)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_COMTYPE;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package);
            if (RWType == RWFunType.GPRS)
                return 0;
            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            if (result.Data.Length != 1)
            {
                throw new Exception("数据损坏");
            }
            return Convert.ToInt32(result.Data[0]);
        }

        public int[] ReadIP(ConstValue.DEV_TYPE devtype, short Id)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_IP;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package);
            if (RWType == RWFunType.GPRS)
                return null;
            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            if (result.Data.Length != 15)
            {
                throw new Exception("数据损坏");
            }
            int[] ip = new int[4];
            for (int i = 0; i < 4; i++)
            {
                string tmp = "";
                for (int j = i * 4; j < i*4 + 3; j++)
                {
                    if ((char)result.Data[j] != '\0')
                        tmp += (char)result.Data[j];
                }
                if (string.IsNullOrEmpty(tmp))
                {
                    tmp = "0";
                }
                ip[i] = Convert.ToInt32(tmp);
            }

            return ip;
        }

        public int ReadPort(ConstValue.DEV_TYPE devtype, short Id)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_PORT;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package);
            if (RWType == RWFunType.GPRS)
                return 0;

            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            if (result.Data.Length != 4 && result.Data.Length != 5)
            {
                throw new Exception("数据损坏");
            }
            string tmp = "";
            for (int i = 0; i < result.Data.Length; i++)
            {
                if ((char)result.Data[i] != '\0')
                    tmp += (char)result.Data[i];
            }
            if (string.IsNullOrEmpty(tmp))
            {
                tmp = "0";
            }
            return Convert.ToInt32(tmp);
        }

        public byte ReadCollectConfig(ConstValue.DEV_TYPE devtype, short Id)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_COLLECTCONFIG;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package);
            if (RWType == RWFunType.GPRS)
                return 0;

            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            if (result.Data.Length != 1)
            {
                throw new Exception("数据损坏");
            }
            return result.Data[0];
        }

        public int ReadHeartInterval(ConstValue.DEV_TYPE devtype, short Id)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_HEARTINTERVAL;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package);
            if (RWType == RWFunType.GPRS)
                return 0;

            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            if (result.Data.Length != 2)
            {
                throw new Exception("数据损坏");
            }
            return BitConverter.ToInt16(new byte[] { result.Data[1], result.Data[0] }, 0);
        }

        public int ReadVolInterval(ConstValue.DEV_TYPE devtype, short Id)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_VOLINTERVAL;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package);
            if (RWType == RWFunType.GPRS)
                return 0;

            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            if (result.Data.Length != 2)
            {
                throw new Exception("数据损坏");
            }
            return BitConverter.ToInt16(new byte[] { result.Data[1], result.Data[0] }, 0);
        }

        public short ReadVolLower(ConstValue.DEV_TYPE devtype, short Id)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_VOLLOWER;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package);
            if (RWType == RWFunType.GPRS)
                return 0;
            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            if (result.Data.Length != 1)
            {
                throw new Exception("数据损坏");
            }
            return Convert.ToInt16(result.Data[0]);
        }

        public short ReadSMSInterval(ConstValue.DEV_TYPE devtype, short Id)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_SMSINTERVAL;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package);
            if (RWType == RWFunType.GPRS)
                return 0;

            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            if (result.Data.Length != 2)
            {
                throw new Exception("数据损坏");
            }
            return BitConverter.ToInt16(new byte[] { result.Data[1], result.Data[0] }, 0);
        }
        public short ReadPluseUnit(ConstValue.DEV_TYPE devtype, short Id)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_PLUSEUNIT;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package);
            if (RWType == RWFunType.GPRS)
                return 0;

            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            if (result.Data.Length != 1)
            {
                throw new Exception("数据损坏");
            }
            return Convert.ToInt16(result.Data[0]);
        }

        public short ReadAlarmLen(ConstValue.DEV_TYPE devtype, short Id)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_ALARMLEN;
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package);
            if (RWType == RWFunType.GPRS)
                return 0;

            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            if (result.Data.Length != 1)
            {
                throw new Exception("数据损坏");
            }
            return Convert.ToInt16(result.Data[0]);
        }

        public DataTable ReadSimualteInterval(ConstValue.DEV_TYPE devtype, short Id)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_SIMINTERVAL;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package);
            if (RWType == RWFunType.GPRS)
                return null;

            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            if ((result.Data.Length%7) !=0)
            {
                throw new Exception("数据损坏");
            }
            DataTable dt_simulate = new DataTable();
            dt_simulate.Columns.Add("starttime");
            dt_simulate.Columns.Add("collecttime1");
            dt_simulate.Columns.Add("collecttime2");
            dt_simulate.Columns.Add("sendtime");
            for (int i = 0; i < result.DataLength; i+=7)
            {
                DataRow dr = dt_simulate.NewRow();
                dr["starttime"] = Convert.ToInt32(result.Data[i]);
                dr["sendtime"] = BitConverter.ToInt16(new byte[]{result.Data[i+2],result.Data[i+1]}, 0);
                dr["collecttime1"] = BitConverter.ToInt16(new byte[] { result.Data[i + 4], result.Data[i + 3] }, 0);
                dr["collecttime2"] = BitConverter.ToInt16(new byte[] { result.Data[i + 6], result.Data[i + 5] }, 0);
                dt_simulate.Rows.Add(dr);
            }
            
            return dt_simulate;
        }
        public DataTable ReadPluseInterval(ConstValue.DEV_TYPE devtype, short Id)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_PLUSEINTERVAL;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package);
            if (RWType == RWFunType.GPRS)
                return null;

            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            if ((result.Data.Length % 7) != 0)
            {
                throw new Exception("数据损坏");
            }
            DataTable dt_pluse = new DataTable();
            dt_pluse.Columns.Add("starttime");
            dt_pluse.Columns.Add("precollecttime");
            dt_pluse.Columns.Add("plusecollecttime");
            dt_pluse.Columns.Add("sendtime");
            for (int i = 0; i < result.DataLength; i += 7)
            {
                DataRow dr = dt_pluse.NewRow();
                dr["starttime"] = Convert.ToInt32(result.Data[i]);
                dr["sendtime"] = BitConverter.ToInt16(new byte[] { result.Data[i + 2], result.Data[i + 1] }, 0);
                dr["precollecttime"] = BitConverter.ToInt16(new byte[] { result.Data[i + 4], result.Data[i + 3] }, 0);
                dr["plusecollecttime"] = BitConverter.ToInt16(new byte[] { result.Data[i + 6], result.Data[i + 5] }, 0);
                dt_pluse.Rows.Add(dr);
            }

            return dt_pluse;
        }

        public DataTable ReadRS485Interval(ConstValue.DEV_TYPE devtype, short Id)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_485INTERVAL;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package);
            if (RWType == RWFunType.GPRS)
                return null;

            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            if ((result.Data.Length % 5) != 0)
            {
                throw new Exception("数据损坏");
            }
            DataTable dt_rs485 = new DataTable();
            dt_rs485.Columns.Add("starttime");
            dt_rs485.Columns.Add("collecttime");
            dt_rs485.Columns.Add("sendtime");
            for (int i = 0; i < result.DataLength; i += 5)
            {
                DataRow dr = dt_rs485.NewRow();
                dr["starttime"] = Convert.ToInt32(result.Data[i]);
                dr["sendtime"] = BitConverter.ToInt16(new byte[] { result.Data[i + 2], result.Data[i + 1] }, 0);
                dr["collecttime"] = BitConverter.ToInt16(new byte[] { result.Data[i + 4], result.Data[i + 3] }, 0);
                dt_rs485.Rows.Add(dr);
            }

            return dt_rs485;
        }

        public DataTable ReadModbusProtocol(ConstValue.DEV_TYPE devtype, short Id)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_MODBUSPROTOCOL;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            Package result = Read(package);
            if (RWType == RWFunType.GPRS)
                return null;

            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception("获取失败");
            }
            if (result.Data.Length == 0)
            {
                throw new Exception("无数据");
            }
            if ((result.Data.Length % 7) != 0)
            {
                throw new Exception("数据损坏");
            }
            DataTable dt_485protocol = new DataTable();
            dt_485protocol.Columns.Add("baud");
            dt_485protocol.Columns.Add("ID");
            dt_485protocol.Columns.Add("funcode");
            dt_485protocol.Columns.Add("regbeginaddr");
            dt_485protocol.Columns.Add("regcount");
            for (int i = 0; i < result.DataLength; i += 7)
            {
                DataRow dr = dt_485protocol.NewRow();
                int baud = Convert.ToInt32(result.Data[i]);
                if (baud == 0)
                    dr["baud"] = 1200;
                else if (baud == 1)
                    dr["baud"] = 2400;
                else if (baud == 2)
                    dr["baud"] = 4800;
                else if (baud == 3)
                    dr["baud"] = 9600;
                dr["ID"] = Convert.ToInt32(result.Data[i+1]);
                dr["funcode"] = "0x"+String.Format("{0:X2}", result.Data[i + 2]);
                dr["regbeginaddr"] = "0x" + String.Format("{0:X2}", result.Data[i + 4]) + String.Format("{0:X2}", result.Data[i + 3]);//BitConverter.ToInt16(new byte[] { result.Data[i + 4], result.Data[i + 3] }, 0);
                dr["regcount"] = BitConverter.ToInt16(new byte[] { result.Data[i + 6], result.Data[i + 5] }, 0);
                dt_485protocol.Rows.Add(dr);
            }

            return dt_485protocol;
        }
        
        public Package GetCallDataPackage(short Id,UNIVERSAL_COMMAND commandtype,int timeout=4,int times=2)
        {
            Package package = new Package();
            package.DevType = ConstValue.DEV_TYPE.UNIVERSAL_CTRL; ;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)commandtype;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            return Read(package,timeout,times);
        }

        /// <summary>
        /// 获得第?路脉冲基准数数据包
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="value">基准数</param>
        /// <param name="waytype">1:第一路脉冲,2:第二路脉冲,3:第三路脉冲,4:第四路脉冲</param>
        /// <returns></returns>
        public Package GetPluseBasicPackage(ConstValue.DEV_TYPE devtype, short Id, UInt32 value, int waytype)
        {
            Package package = new Package();
            package.DevType = devtype;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            if (waytype == 1)
                package.C1 = (byte)UNIVERSAL_COMMAND.SET_PLUSEBASIC1;
            else if (waytype == 2)
                package.C1 = (byte)UNIVERSAL_COMMAND.SET_PLUSEBASIC2;
            else if (waytype == 3)
                package.C1 = (byte)UNIVERSAL_COMMAND.SET_PLUSEBASIC3;
            else if (waytype == 4)
                package.C1 = (byte)UNIVERSAL_COMMAND.SET_PLUSEBASIC4;
            byte[] data = BitConverter.GetBytes(value);
            List<byte> lstdata = new List<byte>();
            if (data.Length == 4)
            {
                lstdata.Add(data[3]);
                lstdata.Add(data[2]);
                lstdata.Add(data[1]);
                lstdata.Add(data[0]);
            }
            package.DataLength = data.Length;
            package.Data = lstdata.ToArray();
            package.CS = package.CreateCS();

            return package;
        }

    }
}
