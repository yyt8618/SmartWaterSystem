using System.Net.Sockets;
using System.Collections.Generic;
using System;
using Entity;

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
            set { _clientSocket = value; ActiveTime(); }
        }

        private ConstValue.DEV_TYPE _devType;
        /// <summary>
        /// 设备类型
        /// </summary>
        public Entity.ConstValue.DEV_TYPE DevType
        {
            get { return _devType; }
            set { _devType = value; }
        }

        private short _id = -1;   //默认为-1，SL651协议时使用默认值
        /// <summary>
        /// 终端编号
        /// </summary>
        public short TerId
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 终端地址A5
        /// </summary>
        public byte A5 { get; set; }
        /// <summary>
        /// 终端地址A4
        /// </summary>
        public byte A4 { get; set; }
        /// <summary>
        /// 终端地址A3
        /// </summary>
        public byte A3 { get; set; }
        /// <summary>
        /// 终端地址A2
        /// </summary>
        public byte A2 { get; set; }
        /// <summary>
        /// 终端地址A1
        /// </summary>
        public byte A1 { get; set; }

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
        
        /// <summary>
        /// 多包数据
        /// </summary>
        public List<byte> multiData { get; set; }

        private DateTime _ModifyTime = DateTime.Now;
        /// <summary>
        /// 对象修改时间，用于超过时间销毁
        /// </summary>
        public DateTime ModifyTime
        {
            get { return _ModifyTime; }
        }

        /// <summary>
        /// 激活时间,延长当前对象生命周期
        /// </summary>
        public void ActiveTime()
        {
            _ModifyTime = DateTime.Now;
        }
    }

    public class SendPackageEntity
    {
        private int _tableId =-99;
        /// <summary>
        /// 表ID -99:SL651； P68:-1:校时数据,-2:下送命令帧,>-1:数据库中获取的命令帧ID
        /// </summary>
        public int TableId
        {
            get { return _tableId; }
            set { _tableId = value; }
        }

        private int _sendCount = 0;
        /// <summary>
        /// 发送计数
        /// </summary>
        public int SendCount
        {
            get { return _sendCount; }
            set { _sendCount = value; }
        }

        private int _SendedFlag = 0;
        /// <summary>
        /// 发送标志 0:未发送,1:已发送
        /// </summary>
        public int SendedFlag
        {
            get { return _SendedFlag; }
            set { _SendedFlag = value; }
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

        //private Package _sendPackage;
        /// <summary>
        /// 待发送的包
        /// </summary>
        public Package SendPackage;

        private Package651 _sendPackage651;
        /// <summary>
        /// 待发送的包651
        /// </summary>
        public Package651 SendPackage651
        {
            get { return _sendPackage651; }
            set { _sendPackage651 = value; }
        }

        public SendPackageEntity()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TableId">表ID -99:SL651； P68:-1:校时数据,-2:下送命令帧,>-1:数据库中获取的命令帧ID</param>
        /// <param name="package"></param>
        public SendPackageEntity(PackFromType FromType,Package package)
        {
            TableId = (int)FromType;
            this.SendPackage = package;
        }

        public SendPackageEntity(Package651 package)
        {
            TableId = -99;
            this._sendPackage651 = package;
        }
    }
}
