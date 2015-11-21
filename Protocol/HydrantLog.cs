using System;
using System.Collections.Generic;
using Common;
using System.Data;
using Entity;

namespace Protocol
{
    public class HydrantLog : SerialPortRW
    {
        public bool Reset(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.HYDRANT_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)HYDRANT_COMMAND.RESET;
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = (byte)2;
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetTime(short Id, DateTime dt)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.HYDRANT_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)HYDRANT_COMMAND.SET_TIME;
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

        public bool SetID(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.HYDRANT_CTRL;
            //package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)HYDRANT_COMMAND.SET_ID;
            byte[] data = BitConverter.GetBytes((int)Id);
            List<byte> lstbyte = new List<byte>();
            for (int i = data.Length - 1; i >= 0; i--)
            {
                lstbyte.Add(data[i]);
            }
            package.DataLength = data.Length;
            package.Data = lstbyte.ToArray();
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetIP(short id, int[] ips)
        {
            string ip = "";
            for (int i = 0; i < ips.Length; i++)
            {
                ip += ips[i].ToString().PadLeft(3, '0') + ".";
            }
            if (!string.IsNullOrEmpty(ip))
                ip = ip.Substring(0, ip.Length - 1);
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.HYDRANT_CTRL;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)HYDRANT_COMMAND.SET_IP;
            package.DataLength = 15;
            byte[] data = new byte[package.DataLength];
            //if (ip.Length != 15)
            //{
            //    throw new Exception("ip长度溢出");
            //}
            for (int i = 0; i < ip.Length; i++)
            {
                data[i] = (byte)ip[i];
            }
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetPort(short Id, int port)
        {
            string str_port = port.ToString().PadLeft(4, '0');
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.HYDRANT_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)HYDRANT_COMMAND.SET_PORT;
            byte[] data = new byte[str_port.Length];
            for (int i = 0; i < str_port.Length; i++)
            {
                data[i] = (byte)str_port[i];
            }
            package.DataLength = data.Length;
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetPreConfig(short Id,bool preconfig)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.HYDRANT_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)HYDRANT_COMMAND.SET_PRECONFIG;
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = (byte)(preconfig ? 1 : 0);
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public short ReadId()
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.HYDRANT_CTRL;
            //package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)HYDRANT_COMMAND.READ_ID;
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

        public DateTime ReadTime(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.HYDRANT_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)HYDRANT_COMMAND.READ_TIME;
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
            if (result.Data.Length != 6)
            {
                throw new Exception("数据损坏");
            }
            return new DateTime(Convert.ToInt32(result.Data[0])+2000, Convert.ToInt32(result.Data[1]), Convert.ToInt32(result.Data[2]),
                Convert.ToInt32(result.Data[3]), Convert.ToInt32(result.Data[4]), Convert.ToInt32(result.Data[5]));
        }

        public int[] ReadIP(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.HYDRANT_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)HYDRANT_COMMAND.READ_IP;
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

        public int ReadPort(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.HYDRANT_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)HYDRANT_COMMAND.READ_PORT;
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
            string tmp = "";
            for (int i = 0; i < 4; i++)
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

        public string ReadCellPhone(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.HYDRANT_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)HYDRANT_COMMAND.READ_TELNUM;
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

        public int ReadComType(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.HYDRANT_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)HYDRANT_COMMAND.READ_COMMTYPE;
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
            if (result.Data.Length != 1)
            {
                throw new Exception("数据损坏");
            }
            return Convert.ToInt32(result.Data[0]);
        }

        public bool ReadPreConfig(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.HYDRANT_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)HYDRANT_COMMAND.READ_PRECONFIG;
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
            if (result.Data.Length != 1)
            {
                throw new Exception("数据损坏");
            }
            return Convert.ToInt32(result.Data[0]) == 1 ? true : false;
        }

        public int ReadNumofturns(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.HYDRANT_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)HYDRANT_COMMAND.READ_NUMOFTURNS;
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
            if (result.Data.Length != 1)
            {
                throw new Exception("数据损坏");
            }
            return Convert.ToInt32(result.Data[0]);
        }

        public byte[] ReadHistory(short id,HydrantOptType opt)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.HYDRANT_CTRL;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;

            if (opt == HydrantOptType.Open)
                package.C1 = (byte)HYDRANT_COMMAND.READ_OPEN_HISTORY;
            else if (opt == HydrantOptType.Close)
                package.C1 = (byte)HYDRANT_COMMAND.READ_CLOSE_HISTORY;
            else if (opt == HydrantOptType.OpenAngle)
                package.C1 = (byte)HYDRANT_COMMAND.READ_OPENANGLE_HISTORY;
            else if (opt == HydrantOptType.Impact)
                package.C1 = (byte)HYDRANT_COMMAND.READ_IMPACT_HISTORY;
            else if (opt == HydrantOptType.KnockOver)
                package.C1 = (byte)HYDRANT_COMMAND.READ_KNOCKOVER_HISTORY;

            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            try
            {
                Package result = new Package();
                serialPortUtil.ReadHistoryData(package,opt, 10);
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
                return result.Data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool ReadEnableCollect(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.HYDRANT_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)HYDRANT_COMMAND.READ_ENABLE;
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
            if (result.Data.Length != 1)
            {
                throw new Exception("数据损坏");
            }
            int ienable = Convert.ToInt32(result.Data[0]);
            return ienable == 1 ? true : false;
        }

        public bool SetEnableCollect(short Id,bool enable)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.HYDRANT_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)HYDRANT_COMMAND.ENABLECOLLECT;
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = enable ? (byte)1 : (byte)0;
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

    }
}
