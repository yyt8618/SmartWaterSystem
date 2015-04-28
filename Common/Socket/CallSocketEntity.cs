using System.Net.Sockets;
using System.Collections.Generic;
using System;

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

        private bool _AllowOnLine = false;
        /// <summary>
        /// 是否允许在线
        /// </summary>
        public bool AllowOnLine
        {
            get { return _AllowOnLine; }
            set { _AllowOnLine = value; }
        }

        private List<SendPackageEntity> _lstWaitSendCmd = new List<SendPackageEntity>();
        /// <summary>
        /// 待发送数据列表
        /// </summary>
        public List<SendPackageEntity> lstWaitSendCmd
        {
            get { return _lstWaitSendCmd; }
            set { _lstWaitSendCmd = value; }
        }
    }

    public class SendPackageEntity
    {
        private int _sendCount = 3;
        /// <summary>
        /// 发送计数
        /// </summary>
        public int SendCount
        {
            get { return _sendCount; }
            set { _sendCount = value; }
        }

        private DateTime _sendTime = DateTime.Now;
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime
        {
            get { return _sendTime; }
            set { _sendTime = value; }
        }

        private Package _sendPackage;
        /// <summary>
        /// 待发送的包
        /// </summary>
        public Package SendPackage
        {
            get { return _sendPackage; }
            set { _sendPackage = value; }
        }

        public SendPackageEntity()
        {
        }

        public SendPackageEntity(Package package)
        {
            this._sendPackage = package;
        }
    }
}
