using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class ConstValue
    {
        public static string MSMQPathToUI = "{0}\\private$\\GcGPRSToUI";
        public static string MSMQServiceName = "GCGPRSService";
        public static string MSMQPathToService = "{0}\\private$\\GcGPRSToService";

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
            Msg_Socket,   //消息类型:Socket消息
            Msg_HTTP,     //消息类型:HTTP消息
            Msg_Public,   //消息类型:公共消息,消息类型用于FrmConsole页面判断哪些消息需要显示,Msg_Public全部显示,Msg_Socket和Msg_HTTP通过勾选判断
            Cmd_Online,  //是否在线命令
            Data_OnLineState, //招测终端在线状态
            Get_OnLineState,  //获取终端在线状态
            Cmd_CallData,      //招测数据
            SQL_Syncing,       //正在同步SQL数据
            SL651_Cmd,         //SL651协议命令
            Get_SL651_AllowOnlineFlag,  //获取SL651是否允许在线标志
            Set_SL651_AllowOnlineFlag,  //设置SL651是否允许在线标志
            Get_SL651_WaitSendCmd,      //获取SL651待发送命令列表
            Del_SL651_WaitSendCmd       //删除SL651待发送命令
        }

        /// <summary>
        /// 设备类型
        /// </summary>
        public enum DEV_TYPE
        {
            /// <summary>
            /// 数据采集终端
            /// </summary>
            Data_CTRL = 0x00,
            /// <summary>
            /// 压力控制器
            /// </summary>
            PRESS_CTRL = 0x01,
            /// <summary>
            /// 通用终端
            /// </summary>
            UNIVERSAL_CTRL = 0x02,
            /// <summary>
            /// 便携式压力控制终端
            /// </summary>
            MOBELE_PRESSURE = 0x03,
            /// <summary>
            /// 噪音记录仪
            /// </summary>
            NOISE_LOG = 0x04,
            /// <summary>
            /// 数据远传控制器
            /// </summary>
            NOISE_CTRL = 0x05,
            /// <summary>
            /// 巡视仪
            /// </summary>
            NOISE_TOUR = 0x06,
            /// <summary>
            /// 水质终端
            /// </summary>
            OLWQ_CTRL= 0x07,
            /// <summary>
            /// 消防栓
            /// </summary>
            HYDRANT_CTRL = 0x08
        }

    }
}
