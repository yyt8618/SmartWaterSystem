using System;
using System.Collections.Generic;
using Common;
using Entity;

namespace Protocol
{
    public abstract class RWData
    {
        public RWFunType RWType = RWFunType.SerialPort;
        /// <summary>
        /// 命令帧
        /// </summary>
        public List<Package> lstCmdPack = new List<Package>(100);

        /// <summary>
        /// 清除缓存的命令帧(lstCmdPack)
        /// </summary>
        public void ClearCmdPack()
        {
            lstCmdPack = new List<Package>(100);
        }

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

        public Package Read(Package package,int timeout = 4,int times=2)
        {
            try
            {
                if (RWType == RWFunType.SerialPort)
                {
                    if (!serialPortUtil.IsOpen)
                    {
                        throw new Exception("串口未打开");
                    }
                    return serialPortUtil.SendPackage(package, timeout, times);
                }
                else if(RWType == RWFunType.GPRS)
                {
                    lstCmdPack.Add(package);
                }
                return new Package();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 发送 设置命令帧
        /// </summary>
        /// <param name="pParam"></param>
        public bool Write(Package package)
        {
            try
            {
                if (RWType == RWFunType.SerialPort)
                    return Read(package).IsSuccess;
                else if(RWType == RWFunType.GPRS)
                {
                    lstCmdPack.Add(package);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
