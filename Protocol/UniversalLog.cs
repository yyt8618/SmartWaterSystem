using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Data;

namespace Protocol
{
    public class UniversalLog : SerialPortRW
    {
        public bool Reset(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.RESET;
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
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
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

        public bool EnableCollect(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.EnableCollect;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public short ReadId()
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
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

        public DateTime ReadTime(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_TIME;
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

        public string ReadCellPhone(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_CELLPHONE;
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

        public bool ReadModbusExeFlag(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_MODBUSEXEFLAG;
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
            return (Convert.ToInt32(result.Data[0]) == 1) ? true : false;
        }

        public int ReadBaud(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_BAUD;
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
            return Convert.ToInt32(result.Data);
        }

        public int ReadComType(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_COMTYPE;
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

        public int[] ReadIP(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_IP;
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
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_PORT;
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

        public byte ReadCollectConfig(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_COLLECTCONFIG;
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

        public DataTable ReadSimualteInterval(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_SIMINTERVAL;
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
        public DataTable ReadPluseInterval(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_PLUSEINTERVAL;
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
            DataTable dt_pluse = new DataTable();
            dt_pluse.Columns.Add("starttime");
            dt_pluse.Columns.Add("collecttime");
            dt_pluse.Columns.Add("sendtime");
            for (int i = 0; i < result.DataLength; i += 5)
            {
                DataRow dr = dt_pluse.NewRow();
                dr["starttime"] = Convert.ToInt32(result.Data[i]);
                dr["sendtime"] = BitConverter.ToInt16(new byte[] { result.Data[i + 2], result.Data[i + 1] }, 0);
                dr["collecttime"] = BitConverter.ToInt16(new byte[] { result.Data[i + 4], result.Data[i + 3] }, 0);
                dt_pluse.Rows.Add(dr);
            }

            return dt_pluse;
        }

        public DataTable ReadRS485Interval(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_485INTERVAL;
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

        public DataTable ReadModbusProtocol(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.READ_MODBUSPROTOCOL;
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

        public bool CalibartionSimulate1(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.CalibartionSimualte1;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool CalibartionSimulate2(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.CalibartionSimualte2;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetID(short Id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
            //package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.SET_ID;
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

        public bool SetCellPhone(short Id,string phonenum)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.SET_ID;
            byte[] data = BitConverter.GetBytes((int)Id);
            package.DataLength = data.Length;
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetModbusExeFlag(short Id, bool flag)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.SET_MODBUSEXEFLAG;
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = (flag ? (byte)0x01 : (byte)0x00);
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }

        public bool SetComType(short Id, int flag)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.SET_COMTYPE;
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = (byte)flag;
            package.Data = data;
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
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.SET_IP;
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
            string str_port = port.ToString();
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.SET_PORT;
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
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
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

        public bool SetSimulateInterval(short Id, DataTable dt)
        {
            Package package = GetSimulateIntervalPackage(Id, dt);

            return Write(package);
        }

        public Package GetSimulateIntervalPackage(short Id, DataTable dt)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
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

        public bool SetPluseInterval(short Id, DataTable dt)
        {
            Package package = GetPluseIntervalPackage(Id, dt);

            return Write(package);
        }

        public Package GetPluseIntervalPackage(short Id, DataTable dt)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.SET_PLUSEINTERVAL;
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

        public bool SetRS485Interval(short Id, DataTable dt)
        {
            Package package = GetRS485IntervalPackage(Id, dt);

            return Write(package);
        }

        public Package GetRS485IntervalPackage(short Id, DataTable dt)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
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

        public bool SetModbusProtocol(short Id, DataTable dt)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
            package.DevID = Id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)UNIVERSAL_COMMAND.SET_MODBUSPROTOCOL;
            List<byte> lstdata = new List<byte>();
            byte[] data;
            int i=0;
            for(;i<dt.Rows.Count;i++)
            {
                DataRow dr = dt.Rows[i];
                if (dr["baud"] != DBNull.Value && dr["ID"] != DBNull.Value && dr["funcode"] != DBNull.Value&&
                    dr["regbeginaddr"] != DBNull.Value && dr["regcount"] != DBNull.Value )
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
                    if (id.StartsWith("0x")&&id.Length>2)
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
                }
            }
            for (; i < 8; i++)
            {
                lstdata.Add(0x03);
                lstdata.AddRange(new byte[] { 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });  //补齐8行
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
        public bool SetPluseBasic(short Id,UInt32 value,int waytype)
        {
            Package package = GetPluseBasicPackage(Id,value,waytype);

            return Write(package);
        }

        /// <summary>
        /// 获得第?路脉冲基准数数据包
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="value">基准数</param>
        /// <param name="waytype">1:第一路脉冲,2:第二路脉冲,3:第三路脉冲,4:第四路脉冲</param>
        /// <returns></returns>
        public Package GetPluseBasicPackage(short Id, UInt32 value, int waytype)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.UNIVERSAL_CTRL;
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
