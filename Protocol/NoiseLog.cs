using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoiseAnalysisSystem;
using System.Threading;
using System.Threading.Tasks;

namespace Protocol
{
    /// <summary>
    /// 噪音记录仪
    /// </summary>
    public class NoiseLog : Noise
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
            package.DevType = DEV_TYPE.NOISE_LOG;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_LOG_COMMAND.WRITE_TIME;
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
        /// <param name="id"></param>
        /// <param name="sfpl">收发频率</param>
        /// <param name="wxsl">无线速率</param>
        /// <param name="fsgl">发射功率</param>
        /// <param name="cksl">串口速率</param>
        /// <param name="hxsj">唤醒时间</param>
        /// <returns></returns>
        public bool WriteWireless(short id, int sfpl, int wxsl, int fsgl, int cksl, int hxsj)
        {
            Package package = new Package();
            package.DevType = DEV_TYPE.NOISE_LOG;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_LOG_COMMAND.WRITE_WIRELESS;
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
        /// 串口设置记录仪采集数据的时间段（每天连续采集2个小时） 默认值为2:00～4:00
        /// </summary>
        /// <param name="id"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public bool WriteStartEndTime(short id, int start = 2, int end = 4)
        {
            Package package = new Package();
            package.DevType = DEV_TYPE.NOISE_LOG;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_LOG_COMMAND.WRITE_START_END_TIME;
            package.DataLength = 2;
            byte[] data = new byte[package.DataLength];
            data[0] = (byte)start;
            data[1] = (byte)end;
            package.Data = data;
            package.CS = package.CreateCS();
            return Write(package);
        }

        /// <summary>
        /// 串口设置记录仪采集时间间隔
        /// </summary>
        /// <param name="id">记录仪ID</param>
        /// <param name="interval">时间间隔(范围:4≤时间间隔≤120,默认值12) </param>
        /// <returns></returns>
        public bool WriteInterval(short id, int interval = 12)
        {
            Package package = new Package();
            package.DevType = DEV_TYPE.NOISE_LOG;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_LOG_COMMAND.WRITE_INTERVAL;
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = (byte)interval;
            package.Data = data;
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
            package.DevType = DEV_TYPE.NOISE_LOG;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_LOG_COMMAND.WRITE_REMOTE_SWITCH;
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
            package.DevType = DEV_TYPE.NOISE_LOG;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_LOG_COMMAND.WRITE_REMOTE_SEND_TIME;
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
        /// 读取记录仪时间 数据顺序为：时、分、秒
        /// </summary>
        /// <param name="id">帧ID为记录仪的ID</param>
        /// <returns>数据顺序为：时、分、秒</returns>
        public byte[] ReadTime(short id)
        {
            Package package = new Package();
            package.DevType = DEV_TYPE.NOISE_LOG;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_LOG_COMMAND.READ_TIME;
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
        /// 串口读取记录仪无线通讯的【收发频率＋无线速率＋发射功率＋串口速率＋唤醒时间】
        /// </summary>
        /// <param name="id">帧ID为记录仪的ID</param>
        /// <returns>【收发频率＋无线速率＋发射功率＋串口速率＋唤醒时间】</returns>
        public int[] ReadWireless(short id)
        {
            Package package = new Package();
            package.DevType = DEV_TYPE.NOISE_LOG;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_LOG_COMMAND.READ_WIRELESS;
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
        /// 串口读取记录仪采集数据的时间（每天连续采集2个小时）
        /// </summary>
        /// <param name="id">帧ID为记录仪的ID</param>
        /// <returns></returns>
        public byte[] ReadStartEndTime(short id)
        {
            Package package = new Package();
            package.DevType = DEV_TYPE.NOISE_LOG;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_LOG_COMMAND.READ_START_END_TIME;
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
                if (result.Data.Length != 2)
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
        /// 串口读取记录仪采集时间间隔
        /// </summary>
        /// <param name="id">帧ID为记录仪的ID</param>
        /// <returns></returns>
        public int ReadInterval(short id)
        {
            Package package = new Package();
            package.DevType = DEV_TYPE.NOISE_LOG;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_LOG_COMMAND.READ_INTERVAL;
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
        /// 串口读取记录仪远传功能
        /// </summary>
        /// <param name="id">帧ID为记录仪的ID</param>
        /// <returns></returns>
        public bool ReadRemote(short id)
        {
            Package package = new Package();
            package.DevType = DEV_TYPE.NOISE_LOG;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_LOG_COMMAND.READ_REMOTE;
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
        /// 串口读取记录仪远传发送时间（远传发送频率：1次/天）
        /// </summary>
        /// <param name="id">帧ID为记录仪的ID</param>
        /// <returns></returns>
        public byte ReadRemoteSendTime(short id)
        {
            Package package = new Package();
            package.DevType = DEV_TYPE.NOISE_LOG;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_LOG_COMMAND.READ_REMOTE_SEND_TIME;
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
        /// 串口读取记录仪的ID(如:1)（此条帧ID为记录仪广播ID：0x04000000） 
        /// </summary>
        /// <returns></returns>
        public short ReadNoiseLogID()
        {
            Package package = new Package();
            package.DevType = DEV_TYPE.NOISE_LOG;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_LOG_COMMAND.READ_NOISE_LOG_ID;
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
                return result.DevID;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /// <summary>
        /// 串口读取记录仪的完整编号 (如：0x04000001)
        /// </summary>
        /// <returns></returns>
        public int ReadNoiseLogFullID()
        {
            Package package = new Package();
            package.DevType = DEV_TYPE.NOISE_LOG;
            //package.DevID = 0x04000000;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_LOG_COMMAND.READ_NOISE_LOG_ID;
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
                return result.ID;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region 控制
        /// <summary>
        /// 　串口发送控制记录仪【启动/停止】命令(帧ID为记录仪的ID号)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool CtrlStartOrStop(short id, bool value)
        {
            Package package = new Package();
            package.DevType = DEV_TYPE.NOISE_LOG;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_LOG_COMMAND.CTRL_START_OR_STOP;
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = (byte)(value ? 0x1 : 0x0);
            package.Data = data;
            package.CS = package.CreateCS();
            return Write(package);
        }

        ReadDataEventArgs data = null;
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public short[] Read(short id, int timeOut = 30)
        {
            try
            {
                if (AppConfigHelper.GetAppSettingValue("TimeOut") != null)
                {
                    timeOut = Convert.ToInt32(AppConfigHelper.GetAppSettingValue("TimeOut"));
                }
                return serialPortUtil.ReadData(id, timeOut);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 异步方式获取记录仪数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callBack">回调函数</param>
        /// <param name="stateObject">异步状态信息</param>
        /// <param name="timeOut">超时 默认30秒</param>
        /// <returns></returns>
        public IAsyncResult BeginRead(short id, AsyncCallback callBack, int timeOut = 30)
        {
            try
            {
                AsyncLoadDataEventHandler read = new AsyncLoadDataEventHandler(Read);
                return read.BeginInvoke(id, timeOut, callBack, read);
            }
            catch (Exception e)
            {
                // Hide inside method invoking stack  
                throw e;
            }
        }
        /// <summary>
        /// 等待挂起的异步读取完成
        /// </summary>
        /// <param name="asyncResult">对所等待挂起的异步请求的引用</param>
        /// <returns></returns>
        public short[] EndRead(IAsyncResult asyncResult)
        {
            if (asyncResult == null)
            {
                throw new ArgumentNullException("asyncResult");
            }
            WaitHandle handle = asyncResult.AsyncWaitHandle;
            if (handle != null)
            {
                try
                {
                    handle.WaitOne();
                }
                finally
                {
                    handle.Close();
                }
            }

            AsyncLoadDataEventHandler read = asyncResult.AsyncState as AsyncLoadDataEventHandler;
            short[] data = read.EndInvoke(asyncResult);
            if (data == null)
            {
                throw new TimeoutException("获取数据超时");
            }
            return data;
        }


        public void ReadData(short id)
        {

            Package package = new Package();
            package.DevType = DEV_TYPE.NOISE_LOG;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.DevID = id;

            package.C1 = (byte)NOISE_LOG_COMMAND.CTRL_START_READ;
            package.DataLength = 0;
            package.CS = package.CreateCS();

            serialPortUtil.SendData(package.ToArray());
        }

        #endregion
    }



    public delegate short[] AsyncLoadDataEventHandler(short id, int timeOut);



}
