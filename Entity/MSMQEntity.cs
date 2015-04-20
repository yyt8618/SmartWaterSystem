
using System.Collections.Generic;
using System;
namespace Entity
{
    [Serializable]
    public class MSMQEntity
    {
        private string _msg = "";
        public string Msg
        {
            get { return _msg; }
            set { _msg = value; }
        }

        private Entity.ConstValue.MSMQTYPE _type;
        /// <summary>
        /// 消息类型
        /// </summary>
        public Entity.ConstValue.MSMQTYPE MsgType
        {
            get { return _type; }
            set { _type = value; }
        }

        private ConstValue.DEV_TYPE _DevType;
        /// <summary>
        /// 设备类型
        /// </summary>
        public ConstValue.DEV_TYPE DevType
        {
            get { return _DevType; }
            set { _DevType = value; }
        }

        private short _DevId;
        /// <summary>
        /// 设备ID
        /// </summary>
        public short DevId
        {
            get { return _DevId; }
            set { _DevId = value; }
        }

        private bool _AllowOnLine = true;
        /// <summary>
        /// 是否允许在线
        /// </summary>
        public bool AllowOnline
        {
            get { return _AllowOnLine; }
            set { _AllowOnLine = value; }
        }

        private List<OnLineTerEntity> _lstOnLine;
        /// <summary>
        /// 在线终端列表(用于返回给UI显示)
        /// </summary>
        public List<OnLineTerEntity> lstOnLine
        {
            get { return _lstOnLine; }
            set { _lstOnLine = value; }
        }

        private CallDataTypeEntity _callDataType;
        /// <summary>
        /// 招测数据类型
        /// </summary>
        public CallDataTypeEntity CallDataType
        {
            get { return _callDataType; }
            set { _callDataType = value; }
        }

        public MSMQEntity()
        {
        }

        public MSMQEntity(Entity.ConstValue.MSMQTYPE type, string msg)
        {
            this._type = type;
            this._msg = msg;
        }
    }

    [Serializable]
    public class OnLineTerEntity
    {
        private ConstValue.DEV_TYPE _DevType;
        /// <summary>
        /// 设备类型
        /// </summary>
        public ConstValue.DEV_TYPE DevType
        {
            get { return _DevType; }
            set { _DevType = value; }
        }

        private short _DevId;
        /// <summary>
        /// 设备ID
        /// </summary>
        public short DevId
        {
            get { return _DevId; }
            set { _DevId = value; }
        }

        public OnLineTerEntity()
        {
        }

        public OnLineTerEntity(ConstValue.DEV_TYPE DevType, short DevId)
        {
            this._DevType = DevType;
            this._DevId = DevId;
        }
    }
}
