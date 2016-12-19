
using System.ComponentModel;

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

    /// <summary>
    /// 通用终端报警标志类型
    /// </summary>
    public enum UniversalFlagType
    {
        /// <summary>
        /// 压力1
        /// </summary>
        Pressure1=0,
        /// <summary>
        /// 压力2
        /// </summary>
        Pressure2 = 1,
        /// <summary>
        /// 模拟量1
        /// </summary>
        Simulate1=2,
        /// <summary>
        /// 模拟量2
        /// </summary>
        Simulate2 = 3,
        /// <summary>
        /// 流量
        /// </summary>
        Flow=4,
    }

    public enum UniversalAlarmType
    {
        /// <summary>
        /// 上限报警
        /// </summary>
        UpAlarm,
        /// <summary>
        /// 下限报警
        /// </summary>
        LowAlarm,
        /// <summary>
        /// 斜率上限报警
        /// </summary>
        SlopUpAlarm,
        /// <summary>
        /// 斜率下限报警
        /// </summary>
        SlopLowAlarm,
    }

    /// <summary>
    /// 读写操作类型(用于界面操作)
    /// </summary>
    public enum RWFunType
    {
        /// <summary>
        /// 串口读写
        /// </summary>
        SerialPort,
        /// <summary>
        /// GPRS读写
        /// </summary>
        GPRS,
    }

    /// <summary>
    /// 颜色类型,用于FrmConsole显示,(>=40是远传终端)
    /// </summary>
    public enum ColorType
    {
        /// <summary>
        /// 报警
        /// </summary>
        [Description("报警")]
        Alarm =1,
        /// <summary>
        /// 公共信息
        /// </summary>
        [Description("公共信息")]
        Public =2,
        /// <summary>
        /// 错误
        /// </summary>
        [Description("错误")]
        Error =3,
        /// <summary>
        /// 未解析数据
        /// </summary>
        [Description("未解析数据")]
        UnResolve =4,
        /// <summary>
        /// 原始帧
        /// </summary>
        [Description("原始帧")]
        OriginalFrame = 5,
        /// <summary>
        /// 数据帧
        /// </summary>
        [Description("数据帧")]
        DataFrame =6,
        /// <summary>
        /// 手机
        /// </summary>
        [Description("手机")]
        CellPhone = 7,
        /// <summary>
        /// 串口
        /// </summary>
        [Description("串口")]
        SerialPort = 8,
        /// <summary>
        /// 数据库信息
        /// </summary>
        [Description("数据库")]
        DataBase = 9,
        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 10,
        /// <summary>
        /// 背景色
        /// </summary>
        [Description("背景色")]
        BackColor =11,


        /// <summary>
        /// 噪声终端
        /// </summary>
        [Description("噪声终端")]
        NoiseTer =40,
        /// <summary>
        /// 压力流量终端
        /// </summary>
        [Description("压力流量终端")]
        PreTer =41,
        /// <summary>
        /// 通用终端
        /// </summary>
        [Description("通用终端")]
        UniversalTer =42,
        /// <summary>
        /// 压力控制器(阀门开度控制)
        /// </summary>
        [Description("阀门开度控制")]
        PreCTL =43,
        /// <summary>
        /// 水质终端
        /// </summary>
        [Description("水质终端")]
        OLWQ =44,
        /// <summary>
        /// 消防栓
        /// </summary>
        [Description("消防栓")]
        Hydrant =45,
        /// <summary>
        /// 水厂数据
        /// </summary>
        [Description("水厂数据")]
        WaterWork =46,
        
    }
}
