using System;
using System.Collections.Generic;
using Common;
using System.Data;

namespace Protocol
{
    public class OLWQLog : SerialPortRW
    {
        public bool Reset(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.RESET;
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
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_TIME;
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

        public bool EnableCollect(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.EnableCollect;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public short ReadId()
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            //package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_ID;
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
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_TIME;
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
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_IP;
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
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_PORT;
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
            string tmp = "";
            for (int i = 0; i < 6; i++)
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

        public ushort ReadResidualClLowLimit(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_RESIDUALCLLOWLIMIT;
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
            if (result.Data.Length != 2)
            {
                throw new Exception("数据损坏");
            }
            return BitConverter.ToUInt16(new byte[]{ result.Data[1],result.Data[0]}, 0);
        }

        public ushort ReadResidualClZero(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_RESIDUALCLZERO;
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
            if (result.Data.Length != 2)
            {
                throw new Exception("数据损坏");
            }
            return BitConverter.ToUInt16(new byte[] { result.Data[1], result.Data[0] }, 0);
        }

        public ushort ReadResidualClStandValue(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_RESIDUALCLSTANDVALUE;
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
            if (result.Data.Length != 2)
            {
                throw new Exception("数据损坏");
            }
            return BitConverter.ToUInt16(new byte[] { result.Data[1], result.Data[0] }, 0);
        }

        public ushort ReadResidualClSensitivity(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_RESIDUALCLSENSITIVITY;
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
            if (result.Data.Length != 2)
            {
                throw new Exception("数据损坏");
            }
            return BitConverter.ToUInt16(new byte[] { result.Data[1], result.Data[0] }, 0);
        }

        public ushort ReadClearInterval(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_CLEARINTERVAL;
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
            if (result.Data.Length != 2)
            {
                throw new Exception("数据损坏");
            }
            return BitConverter.ToUInt16(new byte[] { result.Data[1], result.Data[0] }, 0);
        }
        public ushort ReadTempUpLimit(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_TEMPUPLIMIT;
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
            if (result.Data.Length != 2)
            {
                throw new Exception("数据损坏");
            }
            return BitConverter.ToUInt16(new byte[] { result.Data[1], result.Data[0] }, 0);
        }
        public ushort ReadTempLowLimit(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_TEMPLOWLIMIT;
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
            if (result.Data.Length != 2)
            {
                throw new Exception("数据损坏");
            }
            return BitConverter.ToUInt16(new byte[] { result.Data[1], result.Data[0] }, 0);
        }
        public ushort ReadTempAddtion(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_TEMPADDTION;
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
            return Convert.ToUInt16(result.Data[0]);
        }
        public ushort ReadPHUpLimit(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_PHUPLIMIT;
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
            return Convert.ToUInt16(result.Data[0]);
        }
        public ushort ReadPHLowLimit(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_PHLOWLIMIT;
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
            return Convert.ToUInt16(result.Data[0]);
        }
        public ushort ReadConductivityUpLimit(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_CONDUCTIVITYUPLIMIT;
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
            if (result.Data.Length != 2)
            {
                throw new Exception("数据损坏");
            }
            return BitConverter.ToUInt16(new byte[] { result.Data[1], result.Data[0] }, 0);
        }
        public ushort ReadConductivityLowLimit(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_CONDUCTIVITYLOWLIMIT;
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
            if (result.Data.Length != 2)
            {
                throw new Exception("数据损坏");
            }
            return BitConverter.ToUInt16(new byte[] { result.Data[1], result.Data[0] }, 0);
        }
        public ushort ReadTurbidityUpLimit(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_TURBIDITYUPLIMIT;
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
            if (result.Data.Length != 2)
            {
                throw new Exception("数据损坏");
            }
            return BitConverter.ToUInt16(new byte[] { result.Data[1], result.Data[0] }, 0);
        }
        public ushort ReadTurbidityLowLimit(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_TURBIDITYLOWLIMIT;
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
            if (result.Data.Length != 2)
            {
                throw new Exception("数据损坏");
            }
            return BitConverter.ToUInt16(new byte[] { result.Data[1], result.Data[0] }, 0);
        }
        public ushort ReadPowerSupplyType(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_POWERSUPPLYTYPE;
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
            return Convert.ToUInt16(result.Data[0]);
        }

        public byte ReadCollectConfig(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_COLLECTCONFIG;
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
            return result.Data[0];
        }

        public byte ReadCenterAddr(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_CENTERADDR;
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
            return result.Data[0];
        }

        public byte[] ReadTerAddr(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_TERADDR;
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
            if (result.Data.Length != 5)
            {
                throw new Exception("数据损坏");
            }
            return result.Data;
        }

        public byte[] ReadPassword(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_PASSWORD;
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
            if (result.Data.Length != 2)
            {
                throw new Exception("数据损坏");
            }
            return result.Data;
        }

        public ushort ReadWorkType(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_WORKTYPE;
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
            return Convert.ToUInt16(result.Data[0]);
        }

        public bool ReadGPRSSwitch(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_GPRSSWITCH;
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
            return Convert.ToUInt16(result.Data[0]) == 1 ? true : false;
        }

        public DataTable ReadResidualClInterval(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_RESIDUALCLINTERVAL;
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
            if ((result.Data.Length%5) !=0)
            {
                throw new Exception("数据损坏");
            }
            DataTable dt_rcl = new DataTable();
            dt_rcl.Columns.Add("starttime");
            dt_rcl.Columns.Add("collecttime");
            dt_rcl.Columns.Add("sendtime");
            for (int i = 0; i < result.DataLength; i+=5)
            {
                DataRow dr = dt_rcl.NewRow();
                dr["starttime"] = Convert.ToInt32(result.Data[i]);
                dr["sendtime"] = BitConverter.ToInt16(new byte[]{result.Data[i+2],result.Data[i+1]}, 0);
                dr["collecttime"] = BitConverter.ToInt16(new byte[] { result.Data[i + 4], result.Data[i + 3] }, 0);
                dt_rcl.Rows.Add(dr);
            }

            return dt_rcl;
        }
        public DataTable ReadTurbidityInterval(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_TURBIDITYINTERVAL;
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
            if ((result.Data.Length % 5) != 0)
            {
                throw new Exception("数据损坏");
            }
            DataTable dt_tur = new DataTable();
            dt_tur.Columns.Add("starttime");
            dt_tur.Columns.Add("collecttime");
            dt_tur.Columns.Add("sendtime");
            for (int i = 0; i < result.DataLength; i += 5)
            {
                DataRow dr = dt_tur.NewRow();
                dr["starttime"] = Convert.ToInt32(result.Data[i]);
                dr["sendtime"] = BitConverter.ToInt16(new byte[] { result.Data[i + 2], result.Data[i + 1] }, 0);
                dr["collecttime"] = BitConverter.ToInt16(new byte[] { result.Data[i + 4], result.Data[i + 3] }, 0);
                dt_tur.Rows.Add(dr);
            }

            return dt_tur;
        }

        public DataTable ReadPHInterval(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_PHINTERVAL;
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
            if ((result.Data.Length % 5) != 0)
            {
                throw new Exception("数据损坏");
            }
            DataTable dt_ph = new DataTable();
            dt_ph.Columns.Add("starttime");
            dt_ph.Columns.Add("collecttime");
            dt_ph.Columns.Add("sendtime");
            for (int i = 0; i < result.DataLength; i += 5)
            {
                DataRow dr = dt_ph.NewRow();
                dr["starttime"] = Convert.ToInt32(result.Data[i]);
                dr["sendtime"] = BitConverter.ToInt16(new byte[] { result.Data[i + 2], result.Data[i + 1] }, 0);
                dr["collecttime"] = BitConverter.ToInt16(new byte[] { result.Data[i + 4], result.Data[i + 3] }, 0);
                dt_ph.Rows.Add(dr);
            }

            return dt_ph;
        }

        public DataTable ReadConductivityInterval(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.READ_CONDUCTIVITY;
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
            if ((result.Data.Length % 5) != 0)
            {
                throw new Exception("数据损坏");
            }
            DataTable dt_cond = new DataTable();
            dt_cond.Columns.Add("starttime");
            dt_cond.Columns.Add("collecttime");
            dt_cond.Columns.Add("sendtime");
            for (int i = 0; i < result.DataLength; i += 5)
            {
                DataRow dr = dt_cond.NewRow();
                dr["starttime"] = Convert.ToInt32(result.Data[i]);
                dr["sendtime"] = BitConverter.ToInt16(new byte[] { result.Data[i + 2], result.Data[i + 1] }, 0);
                dr["collecttime"] = BitConverter.ToInt16(new byte[] { result.Data[i + 4], result.Data[i + 3] }, 0);
                dt_cond.Rows.Add(dr);
            }

            return dt_cond;
        }

        public DataTable ReadCallData(short Id, DataTable dt_config)
        {
            DataTable dt = new DataTable("CallDataTable");
            dt.Columns.Add("CallDataType");
            dt.Columns.Add("CallData");
            dt.Columns.Add("Unit");
            Package package = new Package();

            //if (calldataType.GetSim1)
            //{
                //package = GetCallDataPackage(Id, UNIVERSAL_COMMAND.CallData_Sim1);
                //Package result = Read(package);
                //if (!result.IsSuccess || result.Data == null)
                //{
                //    throw new Exception("获取模拟量1路失败");
                //}
                //if (result.Data.Length == 0)
                //{
                //    throw new Exception("模拟量1路无数据");
                //}
                //AnalysisSim(Id, result, dt_config, ref dt);
            //}

            return dt;
        }

        public bool SetID(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            //package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_ID;
            byte[] data = BitConverter.GetBytes((int)Id);
            List<byte> lstbyte = new List<byte>();
            for(int i = data.Length-1; i>= 0;i--)
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
                ip += ips[i].ToString().PadLeft(3,'0')+".";
            }
            if (!string.IsNullOrEmpty(ip))
                ip = ip.Substring(0, ip.Length - 1);
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_IP;
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
            string str_port = port.ToString().PadLeft(6,'0');
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_PORT;
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

        public bool SetCollectConfig(short Id, byte config)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_COLLECTCONFIG;
            byte[] data = new byte[1];
            data[0] = config;
            package.DataLength = data.Length;
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetResidualClInterval(short Id, DataTable dt)
        {
            Package package = GetResidualClIntervalPackage(Id, dt);

            return Write(package);
        }

        public Package GetResidualClIntervalPackage(short Id, DataTable dt)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_RESIDUALCLINTERVAL;
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

        public bool SetTurbidityInterval(short Id, DataTable dt)
        {
            Package package = GetTurbidityIntervalPackage(Id, dt);

            return Write(package);
        }

        public Package GetTurbidityIntervalPackage(short Id, DataTable dt)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_TURBIDITYINTERVAL;
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

        public bool SetPHInterval(short Id, DataTable dt)
        {
            Package package = GetPHIntervalPackage(Id, dt);

            return Write(package);
        }

        public Package GetPHIntervalPackage(short Id, DataTable dt)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_PH_INTERVAL;
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

        public bool SetConductivityInterval(short Id, DataTable dt)
        {
            Package package = GetConductivityIntervalPackage(Id, dt);

            return Write(package);
        }

        public Package GetConductivityIntervalPackage(short Id, DataTable dt)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_CONDUCTIVITYINTERVAL;
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

        public bool SetResidualClLowLimit(short Id, ushort value)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_RESIDUALCLLOWLIMIT;
            byte[] data = BitConverter.GetBytes(value);
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

        public bool SetResidualClZero(short Id, ushort value)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_RESIDUALCLZERO;
            byte[] data = BitConverter.GetBytes(value);
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

        public bool SetResidualClStandValue(short Id, ushort StandValue)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_RESIDUALCLSTANDVALUE;
            byte[] data = BitConverter.GetBytes(StandValue);
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

        public bool SetResidualClSensitivity(short Id, ushort value)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_RESIDUALCLSENSITIVITY;
            byte[] data = BitConverter.GetBytes(value);
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

        public bool SetTerAddr(short Id, byte A5,byte A4,byte A3,byte A2,byte A1)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_TERADDR;
            List<byte> lstbyte = new List<byte>();
            lstbyte.Add(A5);
            lstbyte.Add(A4);
            lstbyte.Add(A3);
            lstbyte.Add(A2);
            lstbyte.Add(A1);
            package.DataLength = 5;
            package.Data = lstbyte.ToArray();
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetCenterAddr(short Id, byte CenterAddr)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_CENTERADDR;
            package.DataLength = 1;
            package.Data = new byte[] { CenterAddr };
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetPwd(short Id, byte pwd1, byte pwd0)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_PWD;
            package.DataLength = 2;
            package.Data = new byte[] { pwd1, pwd0 };
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetWorkType(short Id, ushort WorkType)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_WORKTYPE;
            package.DataLength = 1;
            package.Data = new byte[] { Convert.ToByte(WorkType) };
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetGPRSSwitch(short Id, bool isOn)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_GPRSSWITCH;
            package.DataLength = 1;
            byte b = 0x01;
            if (isOn)
                b = 0x01;
            else
                b = 0x00;
            package.Data = new byte[] { b };
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetClearInterval(short Id, ushort value)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_CLEARINTERVAL;
            byte[] data = BitConverter.GetBytes(value);
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

        public bool SetTempUpLimit(short Id, ushort value)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_TEMPUPLIMIT;
            byte[] data = BitConverter.GetBytes(value);
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

        public bool SetTempLowLimit(short Id, ushort value)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_TEMPLOWLIMIT;
            byte[] data = BitConverter.GetBytes(value);
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

        public bool SetTempAddtion(short Id, ushort value)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_TEMPADDTION;
            byte[] data = BitConverter.GetBytes(value);
            package.DataLength = 1;
            package.Data = new byte[] { data[0] };
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetPHUpLimit(short Id, ushort value)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_PHUPLIMIT;
            byte[] data = BitConverter.GetBytes(value);
            package.DataLength = 1;
            package.Data = new byte[] { data[0] };
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetPHLowLimit(short Id, ushort value)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_PHLOWLIMIT;
            byte[] data = BitConverter.GetBytes(value);
            package.DataLength = 1;
            package.Data = new byte[] { data[0] };
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetConductivityUpLimit(short Id, ushort value)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_CONDUCTIVITYUPLIMIT;
            byte[] data = BitConverter.GetBytes(value);
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

        public bool SetConductivityLowLimit(short Id, ushort value)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_CONDUCTIVITYLOWLIMIT;
            byte[] data = BitConverter.GetBytes(value);
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

        public bool SetTurbidityUpLimit(short Id, ushort value)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_TURBIDITYUPLIMIT;
            byte[] data = BitConverter.GetBytes(value);
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

        public bool SetTurbidityLowLimit(short Id, ushort value)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_TURBIDITYLOWLIMIT;
            byte[] data = BitConverter.GetBytes(value);
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

        public bool SetPowerSupplyType(short Id, ushort value)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.OLWQ_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)OLWQ_COMMAND.SET_POWERSUPPLYTYPE;
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = (value == 1 ? (byte)0x01 : (byte)0x02);
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }


    }
}
