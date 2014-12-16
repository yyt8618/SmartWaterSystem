using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;

namespace NoiseAnalysisSystem
{
    /// <summary>
    /// 提供对App.Config配置文件的操作
    /// </summary>
    public class AppConfigHelper
    {
        /// <summary>
        /// 返回appSettings指定key的value值
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static string GetAppSettingValue(string strKey)
        {
            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key == strKey)
                {
                    return ConfigurationManager.AppSettings[strKey];
                }
            }
            return "";
        }

        /// <summary>
        /// 设置appSetting的指定key的value值
        /// </summary>
        /// <param name="newKey">键</param>
        /// <param name="newValue">值</param>
        public static void SetAppSettingValue(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[key].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            // 修改App.config文件    
            XmlDocument xDoc = new XmlDocument();
            // 获取可执行文件的路径和名称            
            xDoc.Load(System.Windows.Forms.Application.ExecutablePath + ".config");
            XmlNode xNode;
            XmlNodeList xElem1;
            xNode = xDoc.SelectSingleNode("//appSettings");

            xElem1 = xNode.SelectNodes("//add");
            for (int i = 0; i < xElem1.Count; i++)
            {
                if (xElem1[i].Attributes["key"].Value == key)
                {
                    xElem1[i].Attributes["value"].Value = value;
                    break;
                }
            }

            xDoc.Save(System.Windows.Forms.Application.ExecutablePath + ".config");
        }
    }
}
