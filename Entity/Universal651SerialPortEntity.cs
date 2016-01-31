using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Universal651SerialPortEntity
    {
        #region 基本配置
        public bool IsOptA1_A5 = false;
        /// <summary>
        /// A1-A5(基本配置)
        /// </summary>
        public byte CA1 { get; set; }
        public byte CA2 { get; set; }
        public byte CA3 { get; set; }
        public byte CA4 { get; set; }
        public byte CA5 { get; set; }

        /// <summary>
        /// 中心地址
        /// </summary>
        public bool IsOptCenterAddr = false;
        public byte CCenterAddr { get; set; }

        /// <summary>
        /// 密码0-1
        /// </summary>
        public bool IsOptPwd = false;
        public byte CPwd0 { get; set; }
        public byte CPwd1 { get; set; }

        /// <summary>
        /// 主信道标志
        /// </summary>
        public bool IsOptCh = false;
        public int ChannelType { get; set; }
        public int Channel { get; set; }
        public string Ip1 { get; set; }
        public string Ip2 { get; set; }
        public string Ip3 { get; set; }
        public string Ip4 { get; set; }
        public int Port { get; set; }

        /// <summary>
        /// 备用信道标志
        /// </summary>
        public bool IsOptStandbyCh = false;
        public int StandByCh { get; set; }
        /// <summary>
        /// 备用信道电话号
        /// </summary>
        public string StandbyChTelnum { get; set; }

        /// <summary>
        /// 工作方式
        /// </summary>
        public bool IsOptWorkType = false;
        public int WorkType { get; set; }

        /// <summary>
        /// 采集要素
        /// </summary>
        public bool IsOptElements = false;
        public byte[] Elements { get; set; }

        /// <summary>
        /// 通信设备识别号
        /// </summary>
        public bool IsOptIdentifyNum = false;
        public int IdentifyNum { get; set; }
        /// <summary>
        /// 通信设备识别号
        /// </summary>
        public string telnum { get; set; }
        #endregion

        #region 运行配置
        /// <summary>
        /// 定时报时间间隔
        /// </summary>
        public bool IsOptPeriodInterval = false;
        public int PeriodInterval { get; set; }
        /// <summary>
        /// 加报时间间隔
        /// </summary>
        public bool IsOptAddInterval = false;
        public int AddInterval { get; set; }
        /// <summary>
        /// 降水量日起始时间
        /// </summary>
        public bool IsOptPrecipitationStartTime = false;
        public int PrecipitationStartTime { get; set; }
        /// <summary>
        /// 采样间隔
        /// </summary>
        public bool IsOptSamplingInterval = false;
        public int SamplingInterval { get; set; }
        /// <summary>
        /// 水位数据存储间隔
        /// </summary>
        public bool IsOptWaterLevelInterval = false;
        public int WaterLevelInterval { get; set; }
        /// <summary>
        /// 雨量计分辨率
        /// </summary>
        public bool IsOptRainFallPrecision = false;
        public double RainFallPrecision { get; set; }
        /// <summary>
        /// 水位计分辨率
        /// </summary>
        public bool IsOptWaterLevelGaugePrecision = false;
        public double WaterLevelGaugePrecision { get; set; }
        /// <summary>
        /// 雨量加报阀值
        /// </summary>
        public bool IsOptRainFallLimit = false;
        public int RainFallLimit { get; set; }
        /// <summary>
        /// 水位基值
        /// </summary>
        public bool IsOptWaterLevelBasic = false;
        public double WaterLevelBasic { get; set; }
        /// <summary>
        /// 水位修正基值
        /// </summary>
        public bool IsOptWaterLevelAmendLimit = false;
        public double WaterLevelAmendLimit { get; set; }
        /// <summary>
        /// 加报水位
        /// </summary>
        public bool IsOptAddtionWaterLevel = false;
        public double AddtionWaterLevel { get; set; }
        /// <summary>
        /// 加报水位以上加报阀值
        /// </summary>
        public bool IsOptAddtionWaterLevelUpLimit = false;
        public double AddtionWaterLevelUpLimit { get; set; }
        /// <summary>
        /// 加报水位以下加报阀值
        /// </summary>
        public bool IsOptAddtionWaterLevelLowLimit = false;
        public double AddtionWaterLevelLowLimit { get; set; }
        #endregion

        #region 按钮查询
        /// <summary>
        /// 查询时间
        /// </summary>
        public bool IsOptQueryTime = false;
        public string QueryTime { get; set; }

        /// <summary>
        /// 查询版本
        /// </summary>
        public bool IsOptQueryVer = false;
        public string Ver { get; set; }

        /// <summary>
        /// 查询实时数据
        /// </summary>
        public bool IsOptQueryCurData = false;
        public string QueryCurData { get; set; }

        /// <summary>
        /// 查询事件
        /// </summary>
        public bool IsOptQueryEvent = false;
        public string QueryEvent { get; set; }

        /// <summary>
        /// 查询状态和报警
        /// </summary>
        public bool IsOptQueryAlarm = false;
        public string QueryAlarm { get; set; }

        /// <summary>
        /// 定值控制命令
        /// </summary>
        public bool IsOptPreConstCtrl = false;
        public byte PreConstCtrl { get; set; }

        /// <summary>
        /// 查询人工置数
        /// </summary>
        public bool IsOptQueryManualSetParm = false;
        public string QueryManualSetParm { get; set; }

        /// <summary>
        /// 设置水位校准值
        /// </summary>
        public bool IsOptSetCalibration = false;
        public string SetCalibration { get; set; }

        /// <summary>
        /// 均匀时段报上传时间
        /// </summary>
        public bool IsOptTimeintervalReportTime = false;
        public int TimeintervalReportTime { get; set; }
        #endregion

    }
}
