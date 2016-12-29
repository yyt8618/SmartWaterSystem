
using System.Collections.Generic;
using System;
using Entity;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract]
    public class SocketEntity
    {
        private string _msg = "";
        [DataMember(Order =1)]
        public string Msg
        {
            get { return _msg; }
            set { _msg = value; }
        }

        private int _MsgArgb=0;
        /// <summary>
        /// 用于显示消息颜色(0:不修改颜色)
        /// </summary>
        [DataMember(Order=2)]
        public int MsgArgb
        {
            get { return _MsgArgb; }
            set { _MsgArgb = value; }
        }

        private Entity.ConstValue.MSMQTYPE _type;
        /// <summary>
        /// 消息类型
        /// </summary>
        [DataMember(Order = 3)]
        public Entity.ConstValue.MSMQTYPE MsgType
        {
            get { return _type;}
            set { _type = value; }
        }

        private ColorType _ShowType;
        /// <summary>
        /// 显示类型,只用于MSMQTYPE是Msg开头的远传消息或者本地显示的串口之类的消息
        /// </summary>
        [DataMember(Order =4)]
        public ColorType ShowType
        {
            get { return _ShowType; }
            set { _ShowType = value; }
        }

        private short _DevId;
        /// <summary>
        /// 设备ID
        /// </summary>
        [DataMember(Order = 5)]
        public short DevId
        {
            get { return _DevId; }
            set { _DevId = value; }
        }

        private ConstValue.DEV_TYPE _DevType;
        /// <summary>
        /// 设备类型
        /// </summary>
        [DataMember(Order = 6)]
        public ConstValue.DEV_TYPE DevType
        {
            get { return _DevType; }
            set { _DevType = value; }
        }

        /// <summary>
        /// P68功能码
        /// </summary>
        [DataMember(Order = 7)]
        public byte P68Funcode { get; set; }

        /// <summary>
        /// 终端地址A5
        /// </summary>
        [DataMember(Order = 8)]
        public byte A5 { get; set; }
        /// <summary>
        /// 终端地址A4
        /// </summary>
        [DataMember(Order = 9)]
        public byte A4 { get; set; }
        /// <summary>
        /// 终端地址A3
        /// </summary>
        [DataMember(Order = 10)]
        public byte A3 { get; set; }
        /// <summary>
        /// 终端地址A2
        /// </summary>
        [DataMember(Order = 11)]
        public byte A2 { get; set; }
        /// <summary>
        /// 终端地址A1
        /// </summary>
        [DataMember(Order = 12)]
        public byte A1 { get; set; }

        /// <summary>
        /// SL651功能码
        /// </summary>
        [DataMember(Order = 13)]
        public byte SL651Funcode { get; set; }

        private bool _AllowOnLine = true;
        /// <summary>
        /// 是否允许在线
        /// </summary>
        [DataMember(Order = 14)]
        public bool AllowOnline
        {
            get { return _AllowOnLine; }
            set { _AllowOnLine = value; }
        }

        private List<OnLineTerEntity> _lstOnLine;
        /// <summary>
        /// 在线终端列表(用于返回给UI显示)
        /// </summary>
        [DataMember(Order = 15)]
        public List<OnLineTerEntity> lstOnLine
        {
            get { return _lstOnLine; }
            set { _lstOnLine = value; }
        }

        private CallDataTypeEntity _callDataType;
        /// <summary>
        /// 招测数据类型
        /// </summary>
        [DataMember(Order = 16)]
        public CallDataTypeEntity CallDataType
        {
            get { return _callDataType; }
            set { _callDataType = value; }
        }

        private Package[] _Packs;
        /// <summary>
        /// 68协议命令帧
        /// </summary>
        [DataMember(Order =17)]
        public Package[] Packs
        {
            get { return _Packs; }
            set { _Packs = value; }
        }

        private Package651 _pack651;
        /// <summary>
        /// SL651下送命令帧
        /// </summary>
        [DataMember(Order = 18)]
        public Package651 Pack651
        {
            get { return _pack651; }
            set { _pack651 = value; }
        }

        public SocketEntity()
        {
        }

        public SocketEntity(Entity.ConstValue.MSMQTYPE type, string msg)
        {
            this._type = type;
            this._msg = msg;
        }

        public SocketEntity(Entity.ConstValue.MSMQTYPE type,ColorType showType, string msg)
        {
            this._type = type;
            this._msg = msg;
            this._ShowType = showType;
        }
    }

    [DataContract]
    public class OnLineTerEntity
    {
        private ConstValue.DEV_TYPE _DevType;
        /// <summary>
        /// 设备类型
        /// </summary>
        [DataMember(Order = 0)]
        public ConstValue.DEV_TYPE DevType
        {
            get { return _DevType; }
            set { _DevType = value; }
        }

        private short _DevId;
        /// <summary>
        /// 设备ID
        /// </summary>
        [DataMember(Order = 1)]
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
