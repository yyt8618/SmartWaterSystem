using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Common;

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
                return Settings.Instance.GetString(SettingKeys.SerialPort) ?? "COM3";
            }
        }

        public static string BaudRateString
        {
            get
            {
                //return "9600";
                return Settings.Instance.GetString(SettingKeys.BaudRate) ?? "9600";
            }
        }

        public static System.IO.Ports.Parity Parity
        {
            get
            {
                switch (Settings.Instance.GetString(SettingKeys.Parity)) 
                {
                    case "NONE":
                        return System.IO.Ports.Parity.None;
                    case "ODD":
                        return System.IO.Ports.Parity.Odd;
                    case "EVEN":
                        return System.IO.Ports.Parity.Even;
                    case "MARK":
                        return System.IO.Ports.Parity.Mark;
                    case "SPACE":
                        return System.IO.Ports.Parity.Space;
                    default:
                        return System.IO.Ports.Parity.None;
                }
            }
        }

        public static string DataBitsString
        {
            get
            {
                return Settings.Instance.GetString(SettingKeys.DataBits) ?? "8";
            }
        }

        public static System.IO.Ports.StopBits StopBits
        {
            get
            {
                switch (Settings.Instance.GetString(SettingKeys.StopPos))
                {
                    case "NONE":
                        return System.IO.Ports.StopBits.None;
                    case "ONE":
                        return System.IO.Ports.StopBits.One;
                    case "TWO":
                        return System.IO.Ports.StopBits.Two;
                    case "OnePointFive":
                        return System.IO.Ports.StopBits.OnePointFive;
                    default:
                        return System.IO.Ports.StopBits.One;
                }
            }
        }
    }
}
