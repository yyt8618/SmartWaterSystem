
using System.Collections.Generic;
using System;
using Entity;
namespace Common
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

        /// <summary>
        /// SL651功能码
        /// </summary>
        public byte SL651Funcode { get; set; }

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

        private Package651 _pack651;
        /// <summary>
        /// SL651下送命令帧
        /// </summary>
        public Package651 Pack651
        {
            get { return _pack651; }
            set { _pack651 = value; }
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
