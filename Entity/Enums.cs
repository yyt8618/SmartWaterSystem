
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
        /// 压力流量终端
        /// </summary>
        PreTer = 1,
        /// <summary>
        /// 流量终端(作废)
        /// </summary>
        FlowTer = 2,
        /// <summary>
        /// 在线水质
        /// </summary>
        OLWQTer=3,
        /// <summary>
        /// 通用终端
        /// </summary>
        UniversalTer = 4,
        /// <summary>
        /// 压力控制器
        /// </summary>
        PressureCtrl = 5,

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

    public enum HydrantOptType
    {
        /// <summary>
        /// 消防栓被打开
        /// </summary>
        Open = 1,
        /// <summary>
        /// 消防栓被关闭
        /// </summary>
        Close = 2,
        /// <summary>
        /// 消防栓开度
        /// </summary>
        OpenAngle = 3,
        /// <summary>
        /// 消防栓被撞击
        /// </summary>
        Impact = 4,
        /// <summary>
        /// 消防栓被撞倒
        /// </summary>
        KnockOver = 5
    }
}
