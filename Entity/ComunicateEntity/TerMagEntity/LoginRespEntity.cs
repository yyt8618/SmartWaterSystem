namespace Entity
{
    public class LoginRespEntity
    {
        public HttpRespCode code = HttpRespCode.Fail;
        public string msg = "";
        public string userid = "";
        /// <summary>
        /// 最大终端类型时间,用于判断终端类型基础数据是否需要更新
        /// </summary>
        public string maxtertypetime = "";
        /// <summary>
        /// 最大故障类型时间，用于判断故障类型基础数据是否需要更新
        /// </summary>
        public string maxbreakdowntime = "";
    }
}
