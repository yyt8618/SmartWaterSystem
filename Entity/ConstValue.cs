using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Entity
{
    public class ConstValue
    {
        public static string MSMQServiceName = "GCGPRSService";

        //噪声参数 Noise_Parm Table
        public static string DCComponentLen = "DCComponentLen";
        public static string Max1 = "Max1";
        public static string Max2 = "Max2";
        public static string Min1 = "Min1";
        public static string Min2 = "Min2";
        public static string LeakHZ_Template = "LeakHZ_Template";
        public static string MaxStandardAMP = "MaxStandardAMP";
        public static string MinStandardAMP = "MinStandardAMP";
        public static string LeakValue_Template = "LeakValue_Template";
        public static double UniversalSimRatio = 3275;  //模拟量1、2报警值计算系数

        public static DateTime MinDateTime = new DateTime(2015, 1, 1, 0, 0, 0);

        public static object obj = new object();

        public static string GetUniversalCollectTypeName(UniversalCollectType type){
            if (type == UniversalCollectType.Simulate)
                return "模拟";
            else if (type == UniversalCollectType.Pluse)
                return "脉冲";
            else
                return "RS485";
        }

        /// <summary>
        /// 消息队列消息类型
        /// </summary>
        public enum MSMQTYPE
        {
            None,
            Msg_Socket,   //socket消息
            Cmd_Online,  //是否在线命令
            Data_OnLineState, //招测终端在线状态
            Get_OnLineState,  //获取终端在线状态
            Cmd_CallData,      //招测数据
            SQL_Syncing,       //正在同步SQL数据
            SL651_Cmd,         //SL651协议命令
            Get_SL651_AllowOnlineFlag,  //获取SL651是否允许在线标志
            Set_SL651_AllowOnlineFlag,  //设置SL651是否允许在线标志
            Get_SL651_WaitSendCmd,      //获取SL651待发送命令列表
            Del_SL651_WaitSendCmd,      //删除SL651待发送命令
            MiniDump,                   //获取Dump文件
            P68_Cmd,           //68协议命令
            Get_P68_WaitSendCmd,        //获取68协议待发送命令列表
            Del_P68_WaitSendCmd,        //删除68协议待发送命令
            GC,
        }

        /// <summary>
        /// 设备类型(用在设备协议)
        /// </summary>
        public enum DEV_TYPE
        {
            /// <summary>
            /// 压力流量终端
            /// </summary>
            [Description("压力流量终端")]
            Data_CTRL = 0x00,
            /// <summary>
            /// 压力控制器
            /// </summary>
            [Description("压力控制器")]
            PRESS_CTRL = 0x01,
            /// <summary>
            /// 通用终端
            /// </summary>
            [Description("通用终端")]
            UNIVERSAL_CTRL = 0x02,
            /// <summary>
            /// 便携式压力控制终端
            /// </summary>
            [Description("便携式压力终端")]
            MOBELE_PRESSURE = 0x03,
            /// <summary>
            /// 噪音记录仪
            /// </summary>
            [Description("噪音记录仪")]
            NOISE_LOG = 0x04,
            /// <summary>
            /// 数据远传控制器
            /// </summary>
            [Description("远传控制器")]
            NOISE_CTRL = 0x05,
            /// <summary>
            /// 巡视仪
            /// </summary>
            [Description("巡视仪")]
            NOISE_TOUR = 0x06,
            /// <summary>
            /// 水质终端
            /// </summary>
            [Description("水质终端")]
            OLWQ_CTRL = 0x07,
            /// <summary>
            /// 消防栓
            /// </summary>
            [Description("消防栓")]
            HYDRANT_CTRL = 0x08,
            /// <summary>
            /// 水厂
            /// </summary>
            [Description("水厂")]
            WATER_WORKS =0x10,
        }

    }
}
