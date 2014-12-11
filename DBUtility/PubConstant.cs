using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using NoiseAnalysisSystem;

namespace Utility
{
    public class PubConstant
    {
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["ConnectionString"];
            }
        }

        /// <summary>
        /// 得到web.config里配置项的数据库连接字符串。
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string GetConnectionString(string configName)
        {
            return ConfigurationManager.AppSettings[configName];
        }

        public static string PortNameString
        {
            get
            {
                //return "COM3";
                return AppConfigHelper.GetAppSettingValue("SerialPort") ?? "COM3";
            }
        }

        public static string BaudRateString
        {
            get
            {
                //return "9600";
                return AppConfigHelper.GetAppSettingValue("BaudRate") ?? "9600";
            }
        }

        public static string DataBitsString
        {
            get
            {
                return "8";
            }
        }

        public static string StopBitsString
        {
            get
            {
                return "1";
            }
        }
    }
}
