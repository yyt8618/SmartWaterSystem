
namespace Entity
{
    public class GPRSCmdFlag
    {
        private int _tableId = -1;
        /// <summary>
        /// DL_ParamToDev.ID
        /// </summary>
        public int TableId
        {
            get { return _tableId; }
            set { _tableId = value; }
        }

        private int _index = -1;
        /// <summary>
        /// lstGprsCmd的索引
        /// </summary>
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
    }
}
