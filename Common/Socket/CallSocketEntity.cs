using System.Net.Sockets;
using System.Collections.Generic;

namespace Common
{
    public class CallSocketEntity
    {
        private Socket _clientSocket;
        /// <summary>
        /// 终端连接socket对象
        /// </summary>
        public Socket ClientSocket
        {
            get { return _clientSocket; }
            set { _clientSocket = value; }
        }

        private Entity.ConstValue.DEV_TYPE _devType;
        /// <summary>
        /// 设备类型
        /// </summary>
        public Entity.ConstValue.DEV_TYPE DevType
        {
            get { return _devType; }
            set { _devType = value; }
        }

        private short _id;
        /// <summary>
        /// 终端编号
        /// </summary>
        public short TerId
        {
            get { return _id; }
            set { _id = value; }
        }

        private List<Package> _lstWaitSendCmd = new List<Package>();
        /// <summary>
        /// 待发送数据列表
        /// </summary>
        public List<Package> lstWaitSendCmd
        {
            get { return _lstWaitSendCmd; }
            set { _lstWaitSendCmd = value; }
        }
    }
}
