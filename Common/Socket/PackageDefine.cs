using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class PackageDefine
    {
        /// <summary>
        /// 开始字符
        /// </summary>
        public static byte BeginByte = 0x68;
        
        public static byte BeginFrame = 0x68;
        /// <summary>
        /// 结束字符
        /// </summary>
        public static byte EndByte = 0x16;
        
        /// <summary>
        /// 帧最短长度
        /// </summary>
        public static int MinLenth = 12;

        #region 651协议定义
        /// <summary>
        /// 开始字符651
        /// </summary>
        public static byte[] BeginByte651 = new byte[] { 0x7E, 0x7E };
        /// <summary>
        /// 结束字符651
        /// </summary>
        public static byte EndByte651 = 0x03;
        /// <summary>
        /// 帧最短长度651
        /// </summary>
        public static int MinLenth651 = 25;
        /// <summary>
        /// 结束字符(有后续帧)
        /// </summary>
        public static byte EndByte_Continue = 0x17;
        /// <summary>
        /// 帧最短长度(多包发送)
        /// </summary>
        public static int MinLenth_Pack = 20;

        /// <summary>
        /// 报文起始符
        /// </summary>
        public static byte CStart = 0x02;

        /// <summary>
        /// 报文起始符(多包发送)
        /// </summary>
        public static byte CStart_Pack = 0x16;

        #region 标识符
        /// <summary>
        /// 地址标识符
        /// </summary>
        public static byte[] AddrFlag = new byte[] { 0xF1, 0xF1 };

        /// <summary>
        /// 中心站地址标识符
        /// </summary>
        public static byte[] CenterAddrFlag = new byte[] { 0x01, 0x20 };

        /// <summary>
        /// 密码标识符
        /// </summary>
        public static byte[] PwdFlag = new byte[] { 0x03, 0x10 };

        /// <summary>
        /// 中1主信道类型及地址
        /// </summary>
        public static byte[] Channel1Flag = new byte[] { 0x04, 0x45 };

        /// <summary>
        /// 中1备用信道类型及地址
        /// </summary>
        public static byte[] StandbyChannel1Flag = new byte[] { 0x05, 0x38 };

        /// <summary>
        /// 中2主信道类型及地址 --未使用
        /// </summary>
        public static byte[] Channel2Flag = new byte[] { 0x00, 0x00 };

        /// <summary>
        /// 中2备用信道类型及地址 --未使用
        /// </summary>
        public static byte[] StandbyChannel2Flag = new byte[] { 0x00, 0x00 };

        /// <summary>
        /// 中3主信道类型及地址 --未使用
        /// </summary>
        public static byte[] Channel3Flag = new byte[] { 0x00, 0x00 };

        /// <summary>
        /// 中3备用信道类型及地址 --未使用
        /// </summary>
        public static byte[] StandbyChannel3Flag = new byte[] { 0x00, 0x00 };

        /// <summary>
        /// 中4主信道类型及地址 --未使用
        /// </summary>
        public static byte[] Channel4Flag = new byte[] { 0x00, 0x00 };

        /// <summary>
        /// 中4备用信道类型及地址 --未使用
        /// </summary>
        public static byte[] StandbyChannel4Flag = new byte[] { 0x00, 0x00 };

        /// <summary>
        /// 工作方式标识符
        /// </summary>
        public static byte[] WorkTypeFlag = new byte[] { 0x0C, 0x08 };

        /// <summary>
        /// 通信设备识别号标识符
        /// </summary>
        public static byte[] IdentifyNumFlag = new byte[] { 0x0F, 0x60 };

        /// <summary>
        /// 采集要素标识符
        /// </summary>
        public static byte[] ElementsFlag = new byte[] { 0x0D, 0x40 };

        /// <summary>
        /// 卡识别号/主备信道标识符
        /// </summary>
        //public static byte[] TelNumFlag = new byte[] { 0x08, 0x08 };

        /// <summary>
        /// 观测时间标识符
        /// </summary>
        public static byte[] ObservationTimeFlag = new byte[] { 0xF0, 0xF0 };

        /// <summary>
        /// 雨量加报阀值标识符
        /// </summary>
        public static byte[] RainFallLimitFlag = new byte[] { 0x27, 0x08 };

        /// <summary>
        /// 时段降雨量标志符
        /// </summary>
        public static byte[] PeriodPrecipitationFlag = new byte[] { 0x22, 0x19 };

        /// <summary>
        /// 降雨量标志符
        /// </summary>
        public static byte[] PrecipitationFlag = new byte[] { 0x20, 0x19 };

        /// <summary>
        /// 日降雨量标志符
        /// </summary>
        public static byte[] DayPrecipitationFlag = new byte[] { 0x1F, 0x19 };

        /// <summary>
        /// 降雨历时标识符
        /// </summary>
        public static byte[] RainFallTookFlag = new byte[] { 0x15, 0x12 };

        /// <summary>
        /// 降水累计值标识符
        /// </summary>
        public static byte[] RainFallAddupFlag = new byte[] { 0x26, 0x19 };

        /// <summary>
        /// 状态及报警信息标识符
        /// </summary>
        public static byte[] AlarmFlag = new byte[] { 0x45, 0x20 };

        /// <summary>
        /// 电压标识符
        /// </summary>
        public static byte[] PowerFlag = new byte[] { 0x38, 0x12 };

        /// <summary>
        /// 瞬时水位标识符
        /// </summary>
        public static byte[] InstantWaterlevelFlag = new byte[] { 0x39, 0x23 };

        /// <summary>
        /// 1h内第5min时段降水量标识符
        /// </summary>
        public static byte[] Precipitation5MinFlag = new byte[] { 0xF4, 0x60 };

        /// <summary>
        /// 1h内5min间隔相对水位标识符
        /// </summary>
        public static byte[] Waterlevel5MinFlag = new byte[] { 0xF5, 0xC0 };

        /// <summary>
        /// 定时报时间间隔标识符
        /// </summary>
        public static byte[] PeriodIntervalFlag = new byte[] { 0x20, 0x18 };

        /// <summary>
        /// 加报时间间隔标识符
        /// </summary>
        public static byte[] AddIntervalFlag = new byte[] { 0x21, 0x08 };

        /// <summary>
        /// 降水量日起始时间标识符
        /// </summary>
        public static byte[] PrecipitationStartTimeFlag = new byte[] { 0x22, 0x08 };

        /// <summary>
        /// 采样间隔标识符
        /// </summary>
        public static byte[] SamplingFlag = new byte[] { 0x23, 0x10 };

        /// <summary>
        /// 水位数据存储间隔
        /// </summary>
        public static byte[] WaterLevelIntervalFlag = new byte[] { 0x24, 0x08 };

        /// <summary>
        /// 雨量计分辨率
        /// </summary>
        public static byte[] RainFallPrecisionFlag = new byte[] { 0x25, 0x09 };

        /// <summary>
        /// 水位计分辨率
        /// </summary>
        public static byte[] WaterLevelGaugePrecisionFlag = new byte[] { 0x26, 0x09 };

        /// <summary>
        /// 水位基值标识符
        /// </summary>
        public static byte[] WaterLevelBasicFlag = new byte[] { 0x28, 0x23 };

        /// <summary>
        /// 水位修正基值标识符
        /// </summary>
        public static byte[] WaterLevelAmendLimitFlag = new byte[] { 0x30, 0x1B };

        /// <summary>
        /// 加报水位标识符
        /// </summary>
        public static byte[] AddtionWaterLevelFlag = new byte[] { 0x38, 0x12 };

        /// <summary>
        /// 加报水位以上加报阀值标识符
        /// </summary>
        public static byte[] AddtionWaterLevelUpLimitFlag = new byte[] { 0x40, 0x12 };

        /// <summary>
        /// 加报水位以下加报阀值标识符
        /// </summary>
        public static byte[] AddtionWaterLevelLowLimitFlag = new byte[] { 0x41, 0x12 };

        /// <summary>
        /// 瞬时水温标识符
        /// </summary>
        public static byte[] InstantWaterTempFlag = new byte[] { 0x03, 0x11 };

        /// <summary>
        /// PH标识符
        /// </summary>
        public static byte[] PHFlag = new byte[] { 0x46, 0x12 };

        /// <summary>
        /// 电导率标识符
        /// </summary>
        public static byte[] ConductivityFlag = new byte[] { 0x48, 0x18 };

        /// <summary>
        /// 浊度标识符
        /// </summary>
        public static byte[] TurbidityFlag = new byte[] { 0x49, 0x10 };

        /// <summary>
        /// 1min时段降水量
        /// </summary>
        public static byte[] Precipitation1min = new byte[] { 0x21, 0x19 };

        /// <summary>
        /// 5min时段降水量
        /// </summary>
        public static byte[] Precipitation5min = new byte[] { 0x22, 0x19 };

        /// <summary>
        /// 10min时段降水量
        /// </summary>
        public static byte[] Precipitation10min = new byte[] { 0x23, 0x19 };

        /// <summary>
        /// 30min时段降水量
        /// </summary>
        public static byte[] Precipitation30min = new byte[] { 0x24, 0x19 };

        /// <summary>
        /// 1h时段降水量
        /// </summary>
        public static byte[] Precipitation1h = new byte[] { 0x1A, 0x19 };

        /// <summary>
        /// 2h时段降水量
        /// </summary>
        public static byte[] Precipitation2h = new byte[] { 0x1B, 0x19 };

        /// <summary>
        /// 3h时段降水量
        /// </summary>
        public static byte[] Precipitation3h = new byte[] { 0x1C, 0x19 };

        /// <summary>
        /// 6h时段降水量
        /// </summary>
        public static byte[] Precipitation6h = new byte[] { 0x1D, 0x19 };

        /// <summary>
        /// 12h时段降水量
        /// </summary>
        public static byte[] Precipitation12h = new byte[] { 0x1E, 0x19 };
        #endregion

        /// <summary>
        /// 询问。作为下行查询及控制命令帧报文结束符
        /// </summary>
        public static byte ENQ = 0x05;
        /// <summary>
        /// 肯定确认，继续发送。作为后续报文帧的”确认帧“报文结束符
        /// </summary>
        public static byte ACK = 0x06;
        /// <summary>
        /// 传输结束，退出，作为传输结束确认帧报文结束符，表示可以退出通讯
        /// </summary>
        public static byte EOT = 0x04;
        /// <summary>
        /// 传输结束，终端保持在线。在下行确认帧代替EOT作为报文结束符，要求终端在线。保持在线10分钟内若没有接受到中心站命令，终端退回原先设定的工作状态
        /// </summary>
        public static byte ESC = 0x1B;
        #endregion
    }
}
