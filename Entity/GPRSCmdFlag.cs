
namespace Entity
{
    public class GPRSCmdFlag
    {
        private int _tableId = -1;
        /// <summary>
        /// ParamToDev.ID
        /// </summary>
        public int TableId
        {
            get { return _tableId; }
            set { _tableId = value; }
        }

        private int _SendCount = 0;
        /// <summary>
        /// 发送次数,用于更新到数据库
        /// </summary>
        public int SendCount
        {
            get { return _SendCount; }
            set { _SendCount = value; }
        }
    }
}
