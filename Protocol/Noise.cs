using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace Protocol
{
    public abstract class Noise
    {

        #region 事件定义

        public readonly SerialPortUtil serialPortUtil = SerialPortUtil.GetInstance();


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
