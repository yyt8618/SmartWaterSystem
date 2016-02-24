using System;
using System.Collections.Generic;
using Common;
using System.Data;

namespace Protocol
{
    public class PreLog : SerialPortRW
    {
        private Package GetPack(short Id, PreTer_COMMAND cmd)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.Data_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)PreTer_COMMAND.READ_TIME;
            return package;
        }

        public DateTime ReadTime(short Id)
        {
            Package package = GetPack(Id, PreTer_COMMAND.READ_TIME);
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
            return new DateTime(Convert.ToInt32(result.Data[0]) + 2000, Convert.ToInt32(result.Data[1]), Convert.ToInt32(result.Data[2]),
                Convert.ToInt32(result.Data[3]), Convert.ToInt32(result.Data[4]), Convert.ToInt32(result.Data[5]));
        }

        public DataTable ReadPreInterval(short Id)
        {
            Package package = GetPack(Id, PreTer_COMMAND.READ_PREINTERVAL);
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
            DataTable dt_rcl = new DataTable();
            dt_rcl.Columns.Add("starttime");
            dt_rcl.Columns.Add("collecttime");
            dt_rcl.Columns.Add("sendtime");
            for (int i = 0; i < result.DataLength; i += 5)
            {
                DataRow dr = dt_rcl.NewRow();
                dr["starttime"] = Convert.ToInt32(result.Data[i]);
                dr["sendtime"] = BitConverter.ToInt16(new byte[] { result.Data[i + 2], result.Data[i + 1] }, 0);
                dr["collecttime"] = BitConverter.ToInt16(new byte[] { result.Data[i + 4], result.Data[i + 3] }, 0);
                dt_rcl.Rows.Add(dr);
            }

            return dt_rcl;
        }

        public byte[] ReadOffset(short Id,int flag)
        {
            Package package = GetPack(Id, PreTer_COMMAND.READ_PREOFFSET);
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = Convert.ToByte(flag);
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
            if (result.Data.Length != 3)
            {
                throw new Exception("数据损坏");
            }
            return result.Data;
        }

        public DataTable ReadFlowInterval(short Id)
        {
            Package package = GetPack(Id, PreTer_COMMAND.READ_FLOWINTERVAL);
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
            DataTable dt_rcl = new DataTable();
            dt_rcl.Columns.Add("starttime");
            dt_rcl.Columns.Add("collecttime");
            dt_rcl.Columns.Add("sendtime");
            for (int i = 0; i < result.DataLength; i += 5)
            {
                DataRow dr = dt_rcl.NewRow();
                dr["starttime"] = Convert.ToInt32(result.Data[i]);
                dr["sendtime"] = BitConverter.ToInt16(new byte[] { result.Data[i + 2], result.Data[i + 1] }, 0);
                dr["collecttime"] = BitConverter.ToInt16(new byte[] { result.Data[i + 4], result.Data[i + 3] }, 0);
                dt_rcl.Rows.Add(dr);
            }

            return dt_rcl;
        }

        public byte[] ReadPreUpLimit(short Id, int flag)
        {
            Package package = GetPack(Id, PreTer_COMMAND.READ_PRE_UPPERLIMIT);
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            data[0] = Convert.ToByte(flag);
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
            if (result.Data.Length != 3)
            {
                throw new Exception("数据损坏");
            }
            return result.Data;
        }

        public byte[] ReadPreLowLimit(short Id, int flag)
        {
            Package package = GetPack(Id, PreTer_COMMAND.READ_PRE_LOWLIMIT);
            package.C1 = (byte)PreTer_COMMAND.READ_PRE_LOWLIMIT;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            data[0] = Convert.ToByte(flag);
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
            if (result.Data.Length != 3)
            {
                throw new Exception("数据损坏");
            }
            return result.Data;
        }

        public int ReadVoltageLimit(short Id, int flag)
        {
            Package package = GetPack(Id, PreTer_COMMAND.READ_BATTERY_LOWLIMIT);
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            data[0] = Convert.ToByte(flag);
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
            return Convert.ToInt32(result.Data);
        }

        public byte[] ReadSlopUpLimit(short Id, int flag)
        {
            Package package = GetPack(Id, PreTer_COMMAND.READ_SLOPE_UPPERLIMIT);
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            data[0] = Convert.ToByte(flag);
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
            if (result.Data.Length != 3)
            {
                throw new Exception("数据损坏");
            }
            return result.Data;
        }

        public byte[] ReadSlopLowLimit(short Id, int flag)
        {
            Package package = GetPack(Id, PreTer_COMMAND.READ_SLOPE_LOWLIMIT);
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            data[0] = Convert.ToByte(flag);
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
            if (result.Data.Length != 3)
            {
                throw new Exception("数据损坏");
            }
            return result.Data;
        }

        public ushort ReadHeartInterval(short Id, int flag)
        {
            Package package = GetPack(Id, PreTer_COMMAND.READ_HEARTINTERVAL);
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            data[0] = Convert.ToByte(flag);
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

        public byte ReadCollectConfig(short Id)
        {
            Package package = GetPack(Id, PreTer_COMMAND.READ_EnableCollect);
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

        public int ReadComType(short Id)
        {
            Package package = GetPack(Id, PreTer_COMMAND.READ_COMTYPE);
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
            return result.Data[0] == 0x00 ? 0 : 1 ;  //0:GSM,1:GPRS
        }

        /// <summary>
        /// 波特率
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>0:1200,1:2400,2:4800,3:9600</returns>
        public int ReadBaudrate(short Id)
        {
            Package package = GetPack(Id, PreTer_COMMAND.READ_BAUD);
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
            return Convert.ToInt32(result.Data[0]);  //0:1200,1:2400,2:4800,3:9600
        }

        public short ReadId()
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.Data_CTRL;
            //package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)PreTer_COMMAND.READ_ID;
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

        public int[] ReadIP(short Id)
        {
            Package package = GetPack(Id, PreTer_COMMAND.READ_IP);
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
                for (int j = i * 4; j < i * 4 + 3; j++)
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
            Package package = GetPack(Id, PreTer_COMMAND.READ_PORT);
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

        public string ReadCellPhone(short Id)
        {
            Package package = GetPack(Id, PreTer_COMMAND.READ_CELLPHONE);
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

        public bool ReadEnablePreUpLimit(short Id,int flag)
        {
            Package package = GetPack(Id, PreTer_COMMAND.READ_PREUPPERALARM_ENABLE);
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = Convert.ToByte(flag);
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
            return result.Data[1] == 0x00 ? false : true;
        }

        public bool ReadEnablePreLowLimit(short Id, int flag)
        {
            Package package = GetPack(Id, PreTer_COMMAND.READ_PRELOWALARM_ENABLE);
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = Convert.ToByte(flag);
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
            return result.Data[1] == 0x00 ? false : true;
        }

        public bool ReadEnableSlopUpLimit(short Id, int flag)
        {
            Package package = GetPack(Id, PreTer_COMMAND.READ_SLOPEUPPERALARM_ENABLE);
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = Convert.ToByte(flag);
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
            return result.Data[1] == 0x00 ? false : true;
        }

        public bool ReadEnableSlopLowLimit(short Id, int flag)
        {
            Package package = GetPack(Id, PreTer_COMMAND.READ_SLOPELOWALARM_ENABLE);
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = Convert.ToByte(flag);
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
            return result.Data[1] == 0x00 ? false : true;
        }

        public ushort ReadBatteryInterval(short Id, int flag)
        {
            Package package = GetPack(Id, PreTer_COMMAND.READ_BATTERY_INTERVAL);
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = Convert.ToByte(flag);
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

        public ushort ReadPreRange(short Id, int flag)
        {
            Package package = GetPack(Id, PreTer_COMMAND.READ_PRE_SPAN);
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = Convert.ToByte(flag);
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

        public bool SetEnableCallData(short Id,bool EnableCalldata)
        {
            Package package = GetPack(Id, PreTer_COMMAND.EnableCallData);
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = (EnableCalldata ? (byte)0x01 : (byte)0x00);
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public byte[] ReadCallData(short Id, int flag)
        {
            Package package = GetPack(Id, PreTer_COMMAND.READ_CALLDATA);
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = Convert.ToByte(flag);
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
            if (result.Data.Length != 34)
            {
                throw new Exception("数据损坏");
            }
            return result.Data;
        }

        public bool Reset(short Id)
        {
            Package package = GetPack(Id, PreTer_COMMAND.RESET);
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = (byte)2;
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool EnableCollect(short Id)
        {
            Package package = GetPack(Id, PreTer_COMMAND.EnableCollect);
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetTime(short Id, DateTime dt)
        {
            Package package = GetPack(Id, PreTer_COMMAND.SET_TIME);
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
            Package package = GetPack(Id, PreTer_COMMAND.WRITE_SETID);
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

        public bool SetIP(short Id, int[] ips)
        {
            string ip = "";
            for (int i = 0; i < ips.Length; i++)
            {
                ip += ips[i].ToString().PadLeft(3,'0')+".";
            }
            if (!string.IsNullOrEmpty(ip))
                ip = ip.Substring(0, ip.Length - 1);
            Package package = GetPack(Id, PreTer_COMMAND.WRITE_IP);
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
            Package package = GetPack(Id, PreTer_COMMAND.WRITE_PORT);
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
            Package package = GetPack(Id, PreTer_COMMAND.EnableCollect);
            byte[] data = new byte[1];
            data[0] = config;
            package.DataLength = data.Length;
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetTurbidityInterval(short Id, DataTable dt)
        {
            Package package = GetTurbidityIntervalPackage(Id, dt);

            return Write(package);
        }

        public Package GetTurbidityIntervalPackage(short Id, DataTable dt)
        {
            Package package = GetPack(Id, PreTer_COMMAND.EnableCollect);
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

    }
}
