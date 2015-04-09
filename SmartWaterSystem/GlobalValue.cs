using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Protocol;
using System.Windows.Forms;
using Entity;
using Common;

namespace SmartWaterSystem
{
    public class GlobalValue
    {
        internal static int Time = Settings.Instance.GetInt(SettingKeys.Time);                    // 采集时长
        internal static SerialPortUtil portUtil = SerialPortUtil.GetInstance();                   // 串口操作对象
        internal static NoiseLog Noiselog = new NoiseLog();                                            // 记录仪串口操作对象
        internal static List<NoiseRecorderGroup> groupList = new List<NoiseRecorderGroup>();      // 分组对象集合
        internal static List<NoiseRecorder> recorderList = new List<NoiseRecorder>();             // 记录仪对象集合
        internal static List<DistanceController> controllerList = new List<DistanceController>(); // 远传控制器对象集合
        internal static string TestPath = Application.StartupPath + @"\Data\";
        internal static List<int> reReadIdList = new List<int>();                                // 需要重新读取的记录仪ID集合
        internal static string Text = "自来水管道分析系统";


        public static SQLSyncManager SQLSyncMgr = new SQLSyncManager();
        public static SerialPortManager SerialPortMgr = new SerialPortManager();
        public static UniversalLog Universallog = new UniversalLog();                            //通用终端串口操作对象

        private static NoiseSerialPortOptEntity _noiseserialportOptEntity = null;
        /// <summary>
        /// 噪声记录仪串口操作数据
        /// </summary>
        public static NoiseSerialPortOptEntity NoiseSerialPortOptData
        {
            get { return _noiseserialportOptEntity; }
            set { _noiseserialportOptEntity = value; }
        }

        private static UniversalSerialPortOptEntity _UniversalSerialPortOptData;
        public static UniversalSerialPortOptEntity UniversalSerialPortOptData
        {
            get { return _UniversalSerialPortOptData; }
            set { _UniversalSerialPortOptData = value; }
        }

        /// <summary>
        /// 清除文本框文本
        /// </summary>
        /// <param name="c">指定控件</param>
        public static void ClearText(Control c)
        {
            foreach (var item in c.Controls)
            {
                if (item is TextBox)
                {
                    (item as TextBox).Clear();
                }
                else if (item is DevExpress.XtraEditors.MemoEdit)
                {
                    (item as DevExpress.XtraEditors.MemoEdit).EditValue = "";
                }
                else if (item is DevExpress.XtraEditors.TextEdit)
                {
                    (item as DevExpress.XtraEditors.TextEdit).EditValue = "";
                }
            }
        }
    }
}
