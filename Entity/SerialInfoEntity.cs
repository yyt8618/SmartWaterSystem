
namespace Entity
{
    public class SerialInfoEntity
    {
        private string _serialFullName = string.Empty;
        /// <summary>
        /// 获取串口全名(用于显示串口完整信息)
        /// </summary>
        public string SerialFullName
        {
            get { return _serialFullName; }
            set { _serialFullName = value; }
        }

        private string _serialName = string.Empty;
        /// <summary>
        /// 获取串口名称(COM?)
        /// </summary>
        public string SerialName
        {
            get { return _serialName; }
            set { _serialName = value; }
        }
    }
}
