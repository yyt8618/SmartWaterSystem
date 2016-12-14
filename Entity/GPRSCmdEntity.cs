using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class GPRSCmdEntity
    {
        private int _tableId;
        /// <summary>
        /// 表ID(-1:校时数据,-2:下送命令帧,>-1:数据库中获取的命令帧ID
        /// </summary>
        public int TableId
        {
            get { return _tableId; }
            set { _tableId = value; }
        }
             
        private int _deviceId;
        /// <summary>
        /// 设备ID
        /// </summary>
        public int DeviceId
        {
            get { return _deviceId; }
            set { _deviceId = value; }
        }

        private int _DevTypeId;
        /// <summary>
        /// 设备类型Id
        /// </summary>
        public int DevTypeId
        {
            get { return _DevTypeId; }
            set { _DevTypeId = value; }
        }

        private int _ctrlCode;
        public int CtrlCode
        {
            get { return _ctrlCode; }
            set { _ctrlCode = value; }
        }

        private int _funCode;
        public int FunCode
        {
            get { return _funCode; }
            set { _funCode = value; }
        }

        private string _Data;
        /// <summary>
        /// 数据
        /// </summary>
        public string Data
        {
            get { return _Data; }
            set { _Data = value; }
        }

        private int _dataLen;
        /// <summary>
        /// 数据长度
        /// </summary>
        public int DataLen
        {
            get { return _dataLen; }
            set { _dataLen = value; }
        }

        private DateTime _ModifyTime = DateTime.Now;
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get { return _ModifyTime; }
            set { _ModifyTime = value; }
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

        private int _SendedCount = 0;
        /// <summary>
        /// 发送次数,达到一定次数,则不再发送该帧
        /// </summary>
        public int SendedCount
        {
            get { return _SendedCount; }
            set { _SendedCount = value; }
        }

    }
}
