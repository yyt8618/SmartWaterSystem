
namespace Entity
{
    /// <summary>
    /// 版本类型
    /// </summary>
    public enum VersionType
    {
        /// <summary>
        /// 数据库
        /// </summary>
        DataBase,
        /// <summary>
        /// 基础数据
        /// </summary>
        BaseData
    }

    /// <summary>
    /// 终端类型
    /// </summary>
    public enum TerType
    {
        /// <summary>
        /// 压力终端
        /// </summary>
        PreTer = 1,
        /// <summary>
        /// 流量终端
        /// </summary>
        FlowTer = 2,
        /// <summary>
        /// 通用终端
        /// </summary>
        UniversalTer = 3
    }

    public enum UniversalCollectType
    {
        /// <summary>
        /// 采集类型（模拟类型）
        /// </summary>
        Simulate = 0,
        /// <summary>
        /// 采集类型（脉冲类型）
        /// </summary>
        Pluse =1,
        /// <summary>
        /// 采集类型RS485
        /// </summary>
        RS485 = 2
    }
}
