using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;

namespace Protocol
{
    /// <summary>
    /// 噪音记录仪
    /// </summary>
    public class NoiseLog : SerialPortRW
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
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_LOG;
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
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_LOG;
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
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_LOG;
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
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_LOG;
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
        /// 设置启动值(标准值)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="StandValue"></param>
        /// <returns></returns>
        public bool WriteStandValue(short id, int StandValue)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_LOG;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_LOG_COMMAND.WRITE_NOISE_STANDVALUE;
            byte[] data = BitConverter.GetBytes((short)StandValue);
            Array.Reverse(data);
            package.DataLength = data.Length;
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
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_LOG;
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
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_LOG;
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
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_LOG;
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
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_LOG;
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
        /// 串口读取记录仪的ID(如:1)（此条帧ID为记录仪广播ID：0x04000000） 
        /// </summary>
        /// <returns></returns>
        public short ReadNoiseLogID()
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_LOG;
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
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_LOG;
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
        public bool CtrlStartOrStop(short id, bool value,out short[] originaldata)
        {
            originaldata = null;

            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_LOG;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_LOG_COMMAND.CTRL_START_OR_STOP;
            package.DataLength = 1;
            byte[] data = new byte[package.DataLength];
            data[0] = (byte)(value ? 0x1 : 0x0);
            package.Data = data;
            package.CS = package.CreateCS();
            if (false)  //value = true,启动时需要获取第一组32个原始数据用于漏水第一种方法判断的数据
            {
                ////清除记录仪数据
                //bool bclear =  ClearData(id);
                //if (!bclear)
                //{
                //    return false;
                //}

                //// 读取记录时间段
                //byte[] tt = ReadStartEndTime(id);
                //if (tt == null)
                //    return false;
                //int begintime = (int)tt[0];

                //DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, ((begintime - 1) < 0) ? 0 : (begintime - 1), 59, 50);
                //dt.AddHours(-1);
                //bool bmodifytime=WriteTime(id, dt);  //修改时间
                //if (!bmodifytime)
                //    return false;

                //bool bstart=Write(package);  //启动
                //if (!bstart)
                //    return false;

                //originaldata = Read(id, 30);
                //if (originaldata == null)
                //    return false;

                //bmodifytime=WriteTime(id, DateTime.Now);  //改回时间
                //if (!bmodifytime)
                //    return false;
                originaldata = serialPortUtil.ReadOrigityData(package, 10);

                return true;
            }
            else
                return Write(package);
        }



        /// <summary>
        /// 清除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ClearData(short id)
        {
            Package clear = new Package();
            clear.DevType = Entity.ConstValue.DEV_TYPE.NOISE_LOG;
            clear.DevID = id;
            clear.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            clear.C1 = (byte)NOISE_LOG_COMMAND.CTRL_CLEAR_FLASH;
            clear.DataLength = 0;
            clear.Data = null;
            clear.CS = clear.CreateCS();

            return Write(clear);
        }

        /// <summary>
        /// 串口读取记录仪的数据总数和启动值
        /// </summary>
        /// <returns></returns>
        public void ReadNoiseLogDataSum2Standvalue(short id,out short datasum,out short standvalue)
        {
            datasum = standvalue = 0;

            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_LOG;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_LOG_COMMAND.CTRL_START_READ;
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
                //数据总长度（2byte）+启动值（2byte）
                datasum = BitConverter.ToInt16(new byte[] { result.Data[1], result.Data[0] }, 0);
                standvalue = BitConverter.ToInt16(new byte[] { result.Data[3], result.Data[2] }, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ReadDataEventArgs data = null;
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public short[] ReadData(short id, int packindex, int timeOut = 30)
        {
            try
            {
                if (!string.IsNullOrEmpty(Settings.Instance.GetString(SettingKeys.TimeOut)))
                {
                    timeOut = Settings.Instance.GetInt(SettingKeys.TimeOut);
                }

                byte curpackindex = 0x01;  //从第1包开始读
                byte maxindex = BitConverter.GetBytes(packindex)[0];
                try
                {
                    Thread.Sleep(100);
                    return serialPortUtil.ReadNoiseLogData(id, maxindex,ref curpackindex, 5);  //1
                }
                catch (TimeoutException e1)
                {
                    try
                    {
                        Thread.Sleep(100);
                        return serialPortUtil.ReadNoiseLogData(id, maxindex, ref curpackindex, 5);  //2
                    }
                    catch (TimeoutException e2)
                    {
                        try
                        {
                            Thread.Sleep(100);
                            return serialPortUtil.ReadNoiseLogData(id, maxindex, ref curpackindex, 5);  //3
                        }
                        catch (TimeoutException e3)
                        {
                            try
                            {
                                Thread.Sleep(100);
                                return serialPortUtil.ReadNoiseLogData(id, maxindex, ref curpackindex, 5);  //4
                            }
                            catch (TimeoutException e4)
                            {
                                throw e4;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
        
        /// <summary>
        /// 读取噪声标准值(启动值)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int ReadNoiseStandValue(short id)
        {
            Package package = new Package();
            package.DevType = Entity.ConstValue.DEV_TYPE.NOISE_LOG;
            package.DevID = id;
            package.CommandType = CTRL_COMMAND_TYPE.REQUEST_BY_MASTER;
            package.C1 = (byte)NOISE_LOG_COMMAND.READ_NOISE_STANDVALUE;
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
                Array.Reverse(result.Data);
                return BitConverter.ToInt16(result.Data, 0);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion
    }



    public delegate short[] AsyncLoadDataEventHandler(short id, int timeOut);



}
