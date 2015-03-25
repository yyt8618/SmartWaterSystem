using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Common
{
    /// <summary>
    /// Contains setting key names.
    /// </summary>
    public class SettingKeys
    {
        public const string Time = "Time";
        public const string SerialPort = "SerialPort";
        public const string BaudRate = "BaudRate";
        public const string Max1 = "Max1";
        public const string Max2 = "Max2";
        public const string Min1 = "Min1";
        public const string Min2 = "Min2";
        public const string LeakHZ_Template = "LeakHZ_Template";
        public const string MaxStandardAMP = "MaxStandardAMP";
        public const string MinStandardAMP = "MinStandardAMP";
        public const string DCComponentLen = "DCComponentLen";
        public const string Calc = "Calc";
        public const string ComTime_Template = "ComTime_Template";
        public const string RecTime_Template = "RecTime_Template";
        public const string Span_Template = "Span_Template";
        public const string LeakValue_Template = "LeakValue_Template";
        public const string Power_Template = "Power_Template";
        public const string ControlPower_Template = "ControlPower_Template";
        public const string Port_Template = "Port_Template";
        public const string Adress_Template = "Adress_Template";
        public const string Skin = "Skin";
        public const string Parity = "Parity";
        public const string DataBits = "DataBits";
        public const string StopPos = "StopPos";
        public const string TimeOut = "TimeOut";
        public const string StopBits = "StopBits";
        public const string LeakVoice = "LeakVoice";
        public const string RecorderVoice = "RecorderVoice";
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public const string DBString = "DBString";
        public const string PreLowLimitColor = "PreLowLimitColor";
        public const string PreUpLimtColor = "PreUpLimtColor";
        public const string PreSlopeLowLimitColor = "PreSlopeLowLimitColor";
        public const string PreSlopeUpLimitColor = "PreSlopeUpLimitColor";
        public const string GPRS_IP = "GPRSIP";
        public const string GPRS_PORT = "GPRSPORT";
    }

    /// <summary>
    /// Default settings
    /// </summary>
    public class SettingDefaults
    {
        static public string[,] Values = 
		{
            {SettingKeys.DBString,""},
            {SettingKeys.PreLowLimitColor,""},
            {SettingKeys.PreUpLimtColor,""},
            {SettingKeys.PreSlopeLowLimitColor,""},
            {SettingKeys.PreSlopeUpLimitColor,""},
            {SettingKeys.GPRS_IP,""},
            {SettingKeys.GPRS_PORT,""}
        };
    }

    /// <summary>
	/// 读写PDA.Config
	/// </summary>
    public class Settings
    {
        public static NLog.Logger Logger = NLog.LogManager.GetLogger("Settings");

        #region 变量
        private static readonly Settings m_instance = new Settings(SettingDefaults.Values);

		private Hashtable m_list = new Hashtable();
        private string m_filePath = string.Empty;
        private bool m_autoWrite = true;
        private string[,] m_defaultValues; 
	    #endregion

        #region 属性
		/// <summary>
		/// Specifies if the settings file is updated whenever a value is set.
        /// If false, you need to call Write to update the underlying settings file.
		/// </summary>
        public bool AutoWrite
        {
            get { return m_autoWrite; }
            set { m_autoWrite = value; }
        }

        /// <summary>
		/// Full path to settings file.
		/// </summary>
        public string FilePath
        {
            get { return m_filePath; }
            set { m_filePath = value; }
        }
	    #endregion

        #region 构造函数
         /// <summary>
        /// Default constructor.
        /// </summary>
        private Settings()
        {
            // get full path to file
            m_filePath = GetFilePath();

            // populate list with settings from file
            Read();
        }

        /// <summary>
		/// Constructor. Pass in an array of default values.
		/// </summary>
        private Settings(string[,] defaultValues)
        {
            // store default values, use later when populate list
            m_defaultValues = defaultValues;

            // get full path to file
            m_filePath = GetFilePath();

            // populate list with settings from file
            Read();
        }
        #endregion

        #region 共用方法
        /// <summary>
        /// The entry point into this singleton
        /// </summary>
        public static Settings Instance
        {
            get { return m_instance; }
        }

        /// <summary>
		/// Set setting value. Update underlying file if AutoWrite is true.
        /// </summary>
        public void SetValue(string key, object value)
        {
            // update internal list
            if (value != null)
                m_list[key] = value.ToString();
            else
                m_list[key] = value;

            // update settings file
            if (m_autoWrite)
                Write();
        }

        /// <summary>
        /// Return specified settings as string.
        /// </summary>
        public string GetString(string key)
        {
            try
            {
                object result = m_list[key];
                if (result != null)
                    return result.ToString();

                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Return specified settings as datetime.
        /// </summary>
        public DateTime GetDateTime(string key)
        {
            try
            {
                object result = m_list[key];
                if (result != null && result.ToString().Length > 0)
                    return Convert.ToDateTime(result);

                return DateTime.MinValue;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Return specified settings as integer.
        /// </summary>
        public int GetInt(string key)
        {
            try
            {
                string result = GetString(key);
                return (result == String.Empty) ? 0 : Convert.ToInt32(result);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Return specified settings as boolean.
        /// </summary>
        public bool GetBool(string key)
        {
            try
            {
                string result = GetString(key);
                return (result == String.Empty) ? false : Convert.ToBoolean(result);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Read settings file.
        /// </summary>
        public void Read()
        {
            XmlTextReader reader = null;
            try
            {
                // first remove all items from list
                m_list.Clear();

                // next, populate list with default values
                for (int i = 0; i < m_defaultValues.GetLength(0); i++)
                    m_list[m_defaultValues[i, 0]] = m_defaultValues[i, 1];

                // open settings file
                reader = new XmlTextReader(m_filePath);

                // go through file and read the xml file and 
                // populate internal list with 'add' elements
                while (reader.Read())
                {
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "add"))
                        m_list[reader.GetAttribute("key")] = reader.GetAttribute("value");
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Settings.Read()", ex);
            }
            finally
            {
                if (reader.ReadState!=ReadState.Closed)
                    reader.Close();
            }
        }

        /// <summary>
        /// Write settings to file.
        /// </summary>
        public void Write()
        {
            try
            {
                // header elements
                FileInfo file = new FileInfo(m_filePath);
                file.Attributes = FileAttributes.Normal;
                StreamWriter writer = File.CreateText(m_filePath);
                writer.WriteLine("<configuration>");
                writer.WriteLine("\t<appSettings>");

                // go through internal list and create 'add' element for each item
                IDictionaryEnumerator enumerator = m_list.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    writer.WriteLine("\t\t<add key=\"{0}\" value=\"{1}\" />", enumerator.Key.ToString(), enumerator.Value);
                }

                // footer elements
                writer.WriteLine("\t</appSettings>");
                writer.WriteLine("</configuration>");
                writer.Close();
                file.Attributes=FileAttributes .ReadOnly ;
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Settings.Write()", ex);
            }
        }

        /// <summary>
        /// Restore defaults settings values
        /// </summary>
        public void RestoreDefaults()
        {
            try
            {
                File.Delete(FilePath);
                Read();
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Settings.RestoreDefaults()", ex);
            }
        }
	    #endregion
       
        #region 私有方法
        /// <summary>
        /// Return full path to settings file. Appends .config to the assembly name.
        /// </summary>
        private string GetFilePath()
        {
            //return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase) + Assembly.GetExecutingAssembly().GetName().Name+".Config";
            //return Assembly.GetExecutingAssembly().Location + ".Config";
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)+"\\App.Config";
        }
        #endregion
    }
}