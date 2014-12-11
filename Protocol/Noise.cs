using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoiseAnalysisSystem;
using System.IO.Ports;

namespace Protocol
{
    public abstract class Noise
    {

        #region 事件定义

        public readonly SerialPortUtil serialPortUtil = SerialPortUtil.GetInstance();

        /// <summary>
        /// 完整协议的记录处理事件
        /// </summary>
        public event PackageReceivedEventHandler PackageReceived
        {
            add
            {
                serialPortUtil.PackageReceived += value;
            }
            remove
            {
                serialPortUtil.PackageReceived -= value;
            }
        }
        /// <summary>
        /// 串口错误事件
        /// </summary>
        public event SerialErrorReceivedEventHandler Error
        {
            add
            {
                serialPortUtil.Error += value;
            }
            remove
            {

                serialPortUtil.Error -= value;
            }
        }

        /// <summary>
        /// 状态监控日志
        /// </summary>
        public event AppendBufLogEventHandler AppendBufLog
        {
            add
            {
                serialPortUtil.AppendBufLog += value;
            }
            remove
            {
                serialPortUtil.AppendBufLog -= value;
            }
        }

        /// <summary>
        /// 读取记录仪数据
        /// </summary>
        public event DataCompletedEventHandler Reading
        {
            add
            {
                serialPortUtil.DataCompleted += value;
            }
            remove
            {
                serialPortUtil.DataCompleted -= value;
            }
        }


        // 定义一个事件来提示界面工作的进度
        public event ReadDataChangedEventHandler ValueChanged
        {
            add
            {
                serialPortUtil.ValueChanged += value;
            }
            remove
            {
                serialPortUtil.ValueChanged -= value;
            }
        }

        #endregion



        public Noise()
        {

        }

        public Package Read(Package package)
        {
            try
            {
                if (!serialPortUtil.IsOpen)
                {
                    throw new Exception("串口未打开");
                }
                return serialPortUtil.SendPackage(package);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public bool TryRead(Package package, out Package result)
        {
            result = default(Package);
            try
            {
                result = Read(package);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// 发送 噪音记录仪 设置命令帧
        /// </summary>
        /// <param name="pParam"></param>
        public bool Write(Package package)
        {
            try
            {
                return Read(package).IsSuccess;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
