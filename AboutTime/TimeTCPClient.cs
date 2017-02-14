using System;
using System.Net.Sockets;

namespace AboutTime
{
    public class TimeUDPClient
    {
        /// <summary>
        /// UDP客户端
        /// </summary>
        private UdpClient _NetWork = null;
        public UdpClient NetWork
        {
            get
            {
                return _NetWork;
            }
            set
            {
                _NetWork = value;
            }
        }


        /// <summary>
        /// 数据接收缓存区
        /// </summary>
        public byte[] buffer = new byte[2048];

        /// <summary>
        /// 断开客户端连接
        /// </summary>
        public void DisConnect()
        {
            try
            {
                if (_NetWork != null)// && _NetWork.Connected)
                {
                    //NetworkStream ns = _NetWork.GetStream();
                    //ns.Close();
                    _NetWork.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
