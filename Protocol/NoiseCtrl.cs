using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Protocol
{
    /// <summary>
    /// 远传控制器
    /// </summary>
    public class NoiseCtrl : SerialPortRW
    {
        #region 设置

        /// <summary>
        /// 设置时间
        /// </summary>
        /// <param name="id"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool WriteTime(short id, DateTime time)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_CTRL;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_CTRL_COMMAND.WRITE_TIME;
            byte[] data = new byte[3];
            data[0] = (byte)time.Hour;
            data[1] = (byte)time.Minute;
            data[2] = (byte)time.Second;
            package.DataLength = data.Length;
            package.Data = data;
            package.CS = package.CreateCS();
            return Write(package);
        }

        /// <summary>
        /// 设置通讯连接
        /// </summary>
        /// <param name="id">远传控制器ID</param>
        /// <param name="sfpl">收发频率</param>
        /// <param name="wxsl">无线速率</param>
        /// <param name="fsgl">发射功率</param>
        /// <param name="cksl">串口速率</param>
        /// <param name="hxsj">唤醒时间</param>
        /// <returns></returns>
        public bool WriteWireless(short id, int sfpl, int wxsl, int fsgl, int cksl, int hxsj)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_CTRL;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_CTRL_COMMAND.WRITE_WIRELESS;
            package.DataLength = 7;
            byte[] data = new byte[package.DataLength];
            //收发频率
            string strSfpl = Convert.ToString(sfpl, 16).PadLeft(6, '0');
            byte[] tSfpl = ConvertHelper.HexStringToByteArray(strSfpl);
            if (tSfpl.Length != 3)
            {
                throw new Exception("收发频率溢出");
            }
            for (int i = 0; i < tSfpl.Length - 1; i++)
            {
                data[i] = tSfpl[i];
            }
            //无线速率
            string strwxsl = Convert.ToString(wxsl, 16).PadLeft(2, '0');
            byte[] twxsl = ConvertHelper.HexStringToByteArray(strwxsl);
            if (twxsl.Length != 1)
            {
                throw new Exception("无线速率溢出");
            }
            data[3] = twxsl[0];

            //发射功率
            string strfsgl = Convert.ToString(fsgl, 16).PadLeft(2, '0');
            byte[] tfsgl = ConvertHelper.HexStringToByteArray(strfsgl);
            if (tfsgl.Length != 1)
            {
                throw new Exception("发射功率溢出");
            }
            data[4] = tfsgl[0];

            //串口速率
            string strcksl = Convert.ToString(cksl, 16).PadLeft(2, '0');
            byte[] tcksl = ConvertHelper.HexStringToByteArray(strcksl);
            if (tcksl.Length != 1)
            {
                throw new Exception("串口速率溢出");
            }
            data[5] = tcksl[0];

            //唤醒时间
            string strhxsj = Convert.ToString(hxsj, 16).PadLeft(2, '0');
            byte[] thxsj = ConvertHelper.HexStringToByteArray(strhxsj);
            if (thxsj.Length != 1)
            {
                throw new Exception("唤醒时间溢出");
            }
            data[6] = thxsj[0];

            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }
        /// <summary>
        /// 设置远传控制器的串口与GPRS模块的通讯波特率
        /// </summary>
        /// <param name="id">远传控制器的ID</param>
        /// <param name="baudrate">串口速率 (默认值9600)</param>
        /// <returns></returns>
        public bool WriteGPRSBaurate(short id, int baudrate = 9600)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_CTRL;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_CTRL_COMMAND.WRITE_GPRS_BAUDRATE;
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = (byte)baudrate;
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }
        /// <summary>
        /// 串口设置远传控制器IP
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ip">IP 例如192.168.010.125</param>
        /// <returns></returns>
        public bool Write_IP(short id, string ip)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_CTRL;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_CTRL_COMMAND.WRITE_IP;
            package.DataLength = 15;
            byte[] data = new byte[package.DataLength];
            if (ip.Length != 15)
            {
                throw new Exception("ip长度溢出");
            }
            for (int i = 0; i < ip.Length; i++)
            {
                data[i] = (byte)ip[i];
            }
            package.Data = data;
            package.CS = package.CreateCS();

            return Write(package);
        }
        /// <summary>
        /// 串口设置远传控制器端口号
        /// </summary>
        /// <param name="id"></param>
        /// <param name="portName">端口号</param>
        /// <returns></returns>
        public bool WritePortName(short id, string portName)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_CTRL;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_CTRL_COMMAND.WRITE_PORT;
            package.DataLength = 4;
            byte[] data = new byte[package.DataLength];
            if (portName.Length > 4)
            {
                throw new Exception("串口号长度溢出");
            }
            string str = portName.PadLeft(4, ' ');

            for (int i = 0; i < str.Length; i++)
            {
                data[i] = ((byte)str[i]);
            }
            package.Data = data;
            package.CS = package.CreateCS();
            return Write(package);
        }
        /// <summary>
        /// 设置远传控制器设备与记录仪设备对应的ID号(1对1：一个远传控制器对应一个记录仪)
        /// </summary>
        /// <param name="ctrlID">控制器ID</param>
        /// <param name="logID">记录仪ID</param>
        /// <returns></returns>
        public bool WriteCtrlToNoiseLogID(short ctrlID, List<int> logID)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_CTRL;
            package.DevID = ctrlID;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_CTRL_COMMAND.WRITE_CTRL_NOISE_LOG_ID;

            List<byte> lstbyte = new List<byte>();
            for(int i=0;i<logID.Count;i++)
            {
                byte[] tmp = BitConverter.GetBytes(logID[i]);
                tmp[3] = (byte)Entity.ConstValue.DEV_TYPE.NOISE_LOG;      //设备类型
                lstbyte.AddRange(tmp);
            }
            //package.DataLength = 4;
            byte[] data = lstbyte.ToArray();
            //data =BitConverter.GetBytes((int)logID);

            Array.Reverse(data);
            package.DataLength = data.Length;
            package.Data = data;
            package.CS = package.CreateCS();
            return Write(package);
        }

        /// <summary>
        /// 设置远传控制器设备ID号
        /// </summary>
        /// <param name="curctrlID">当前控制器ID</param>
        /// <param name="newctrlID">设置的控制器ID</param>
        /// <returns></returns>
        public bool WriteNoiseCtrlID(short curctrlID,short newctrlID)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_CTRL;
            package.DevID = curctrlID;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_CTRL_COMMAND.WRITE_NOISE_CTRL_ID;

            byte[] data = BitConverter.GetBytes((int)newctrlID);
            List<byte> lstbyte = new List<byte>();
            for (int i = data.Length - 1; i >= 0; i--)
            {
                lstbyte.Add(data[i]);
            }
            lstbyte[0] = (byte)Entity.ConstValue.DEV_TYPE.NOISE_CTRL;  //设置设备类型
            package.DataLength = data.Length;
            package.Data = lstbyte.ToArray();

            package.CS = package.CreateCS();
            return Write(package);
        }
        /// <summary>
        /// 串口设置记录仪远传功能
        /// </summary>
        /// <param name="id"></param>
        /// <param name="IsOpen">是否打开 默认关</param>
        /// <returns></returns>
        public bool WriteRemoteSwitch(short id, bool IsOpen)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_CTRL;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_CTRL_COMMAND.WRITE_REMOTE_SWITCH;
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = (byte)(IsOpen ? 1 : 0);
            package.Data = data;
            package.CS = package.CreateCS();
            return Write(package);
        }

        /// <summary>
        /// 串口设置记录仪远传发送时间（远传发送频率：1次/天）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="time">发送时间(发送时间不能设置在记录仪采集数据的时间段内)</param>
        /// <returns></returns>
        public bool WriteRemoteSendTime(short id, int time)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_CTRL;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_CTRL_COMMAND.WRITE_REMOTE_SEND_TIME;
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = (byte)time;
            package.Data = data;
            package.CS = package.CreateCS();
            return Write(package);
        }
        #endregion

        #region 读取
        /// <summary>
        /// 读取时间 数据顺序为：时、分、秒
        /// </summary>
        /// <param name="id">帧ID为记录仪的ID</param>
        /// <returns>数据顺序为：时、分、秒</returns>
        public byte[] ReadTime(short id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_CTRL;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_CTRL_COMMAND.READ_TIME;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            try
            {
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
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 串口读取远传功能
        /// </summary>
        /// <param name="id">帧ID为记录仪的ID</param>
        /// <returns></returns>
        public bool ReadRemote(short id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_CTRL;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_CTRL_COMMAND.READ_REMOTE; 
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            try
            {
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
                return result.Data[0] == 1;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /// <summary>
        /// 串口读取远传发送时间（远传发送频率：1次/天）
        /// </summary>
        /// <param name="id">帧ID为记录仪的ID</param>
        /// <returns></returns>
        public byte ReadRemoteSendTime(short id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_CTRL;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_CTRL_COMMAND.READ_REMOTE_SEND_TIME; 
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            try
            {
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
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /// <summary>
        /// 串口读取远传控制器无线通讯的【收发频率＋无线速率＋发射功率＋串口速率＋唤醒时间】
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int[] ReadWireless(short id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_CTRL;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_CTRL_COMMAND.READ_WIRELESS;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            try
            {
                Package result = Read(package);
                if (!result.IsSuccess || result.Data == null)
                {
                    throw new Exception("获取失败");
                }
                if (result.Data.Length == 0)
                {
                    throw new Exception("无数据");
                }
                if (result.Data.Length != 7)
                {
                    throw new Exception("数据损坏");
                }
                int[] input = new int[]{
                    BitConverter.ToInt32(new byte[]{result.Data[0],result.Data[1],result.Data[2],0},0),
                    result.Data[3],result.Data[4],result.Data[5],result.Data[6]
                };
                return input;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 串口读取远传控制器的串口与GPRS模块的通讯波特率
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int ReadGPRSBaudrate(short id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_CTRL;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_CTRL_COMMAND.READ_GPRS_BAUDRATE;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            try
            {
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
            catch (Exception ex)
            {

                throw ex;
            }

        }
        /// <summary>
        /// 串口读取远传控制器IP
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string ReadIP(short id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_CTRL;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_CTRL_COMMAND.READ_IP;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            try
            {
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

                StringBuilder sb = new StringBuilder();
                foreach (var item in result.Data)
                {
                    sb.Append((char)item);
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 串口读取远传控制器端口号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string ReadPort(short id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_CTRL;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_CTRL_COMMAND.READ_PORT;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            try
            {
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

                StringBuilder sb = new StringBuilder();
                foreach (var item in result.Data)
                {
                    sb.Append((char)item);
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 串口读取远传控制器设备与记录仪设备对应的ID号
        /// </summary>
        /// <returns></returns>
        public List<int> ReadCtrlNoisLogID(short id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_CTRL;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_CTRL_COMMAND.READ_CTRL_NOISE_LOG_ID;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            try
            {
                Package result = Read(package);
                if (!result.IsSuccess || result.Data == null)
                {
                    throw new Exception("获取失败");
                }
                if ((result.Data.Length % 4) != 0)
                {
                    throw new Exception("数据损坏");
                }
                List<int> lstId = new List<int>();
                if (result.Data.Length == 0)
                {
                    //throw new Exception("无数据");
                    return lstId;
                }
                //Array.Reverse(result.Data);
                
                for(int i=0;i< result.Data.Length/4;i++)
                {
                    lstId.Add(BitConverter.ToInt32(new byte[] { result.Data[i*4 + 3], result.Data[i*4 + 2], result.Data[i*4 + 1], 0x00 }, 0));
                }
                //result.Data[3] = 0x00;  //将记录仪类型去掉
                //return BitConverter.ToInt32(result.Data, 0);
                return lstId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 串口读取远程控制器的ID
        /// </summary>
        /// <returns></returns>
        public short ReadNoiseCtrlID()
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_CTRL;
            //package.DevID = 0x05000000;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_CTRL_COMMAND.READ_NOISE_CTRL_ID;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            try
            {
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
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 串口读取远程控制器的ID
        /// </summary>
        /// <returns></returns>
        public int ReadNoiseCtrlFullID()
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_CTRL;
            //package.DevID = 0x05000000;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_CTRL_COMMAND.READ_NOISE_CTRL_ID;
            package.DataLength = 0;
            byte[] data = new byte[package.DataLength];
            package.Data = data;
            package.CS = package.CreateCS();

            try
            {
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
                return result.ID;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        #endregion
    }
}
