using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Protocol;
using System.Windows.Forms;
using Entity;
using Common;
using System.Data;

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
        internal static string Text = "IGC DataLog系列产品应用软件";
        internal static string SocketConfigFilePath= Application.StartupPath + @"\SocketAddr.txt";  //SOCKET配置文件地址

        #region 系统主界面
        private static FrmSystem _mainForm;
        public static FrmSystem MainForm
        {
            get { return _mainForm; }
            set { _mainForm = value; }
        }
        #endregion

        public static SocketManager SocketMgr = new SocketManager();
        public static SerialPortManager SerialPortMgr = new SerialPortManager();
        public static UniversalLog Universallog = new UniversalLog();                            //通用终端串口操作对象
        public static OLWQLog OLWQlog = new OLWQLog();                                          //水质终端串口操作对象
        public static HydrantLog Hydrantlog = new HydrantLog();                                 //消防栓串口操作对象

        private static NoiseSerialPortOptEntity _noiseserialportOptEntity = null;
        /// <summary>
        /// 噪声记录仪串口操作数据
        /// </summary>
        public static NoiseSerialPortOptEntity NoiseSerialPortOptData
        {
            get { return _noiseserialportOptEntity; }
            set { _noiseserialportOptEntity = value; }
        }

        private static UniversalSerialPortOptEntity _UniSerialPortOptData;
        public static UniversalSerialPortOptEntity UniSerialPortOptData
        {
            get { return _UniSerialPortOptData; }
            set { _UniSerialPortOptData = value; }
        }

        private static PreSerialPortOptEntity _PreSerialPortOptData;
        public static PreSerialPortOptEntity PreSerialPortOptData
        {
            get { return _PreSerialPortOptData; }
            set { _PreSerialPortOptData = value; }
        }

        private static Package651 _SerialPort651OptData;
        /// <summary>
        /// 通用终端651协议串口操作数据
        /// </summary>
        public static Package651 SerialPort651OptData
        {
            get { return _SerialPort651OptData; }
            set { _SerialPort651OptData = value; }
        }

        private static CallDataTypeEntity _SerialPortCallDataType;
        /// <summary>
        /// 串口招测类型
        /// </summary>
        public static CallDataTypeEntity SerialPortCallDataType
        {
            get { return _SerialPortCallDataType; }
            set { _SerialPortCallDataType = value; }
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
