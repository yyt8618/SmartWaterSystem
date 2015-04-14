using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
namespace Protocol
{
    /// <summary>
    /// 噪音巡视仪
    /// </summary>
    public class NoiseTour : SerialPortRW
    {
        #region 设置
        /// <summary>
        /// 发送 噪音巡视仪 设置命令 帧ID为巡视仪的ID
        /// </summary>
        /// <param name="Pl">收发频率</param>
        /// <param name="speed">无线速率</param>
        /// <param name="comSpeed">串口速率</param>
        public bool NoiseTourWrite(int Pl, int speed, int comSpeed)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_TOUR;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_TOUR_COMMAND.SETTING;

            package.DataLength = 5;
            package.Data = new byte[package.DataLength];
            //数据域数据 收发频率（3byte）+无线速率（1byte）+串口速率（1byte）
            int t = Pl;
            for (int i = 2; i >= 0; i++)
            {
                byte d = (byte)(t % 16);
                package.Data[i] = d;
                t = t / 16;
            }

            package.Data[3] = (byte)speed;
            package.Data[4] = (byte)comSpeed;

            return Write(package);
        }

        #endregion
        #region 读取
        /// <summary>
        /// 串口读取巡视仪【收发频率＋无线速率+串口速率】
        /// </summary>
        /// <param name="id">帧ID为巡视仪的ID</param>
        /// <returns></returns>
        public int[] ReadWireless(short id)
        {

            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_TOUR;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_TOUR_COMMAND.READ_WIRELESS;
            package.DataLength = 0;
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
                if (result.Data.Length != 5)
                {
                    throw new Exception("数据损坏");
                }
                int[] input = new int[]{
                    BitConverter.ToInt32(new byte[]{result.Data[0],result.Data[1],result.Data[2],0},0),
                    result.Data[3],result.Data[4]
                };
                return input;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 串口读取巡视仪的ID（此条帧ID为巡视仪广播ID：0x06000000）
        /// </summary>
        /// <returns></returns>
        public short ReadNoiseTourID()
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_TOUR;
            //package.DevID = 0x06000000;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_TOUR_COMMAND.READ_NOISE_TOUR_ID;
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
                    throw new Exception("数据错误");
                }
                return result.DevID;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int ReadNoiseTourFullID()
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_TOUR;
            //package.DevID = 0x06000000;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_TOUR_COMMAND.READ_NOISE_TOUR_ID;
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
                    throw new Exception("数据错误");
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
