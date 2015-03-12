using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Protocol;
using System.Windows.Forms;
using Entity;
using Common;

namespace NoiseAnalysisSystem
{
    public class GlobalValue
    {
        internal static int Time = Convert.ToInt32(AppConfigHelper.GetAppSettingValue("Time"));   // 采集时长
        internal static SerialPortUtil portUtil = SerialPortUtil.GetInstance();                   // 串口操作对象
        internal static NoiseLog log = new NoiseLog();                                            // 记录仪串口操作对象
        internal static List<NoiseRecorderGroup> groupList = new List<NoiseRecorderGroup>();      // 分组对象集合
        internal static List<NoiseRecorder> recorderList = new List<NoiseRecorder>();             // 记录仪对象集合
        internal static List<DistanceController> controllerList = new List<DistanceController>(); // 远传控制器对象集合
        internal static string TestPath = Application.StartupPath + @"\Data\";
        internal static List<int> reReadIdList = new List<int>();                                // 需要重新读取的记录仪ID集合
        internal static string Text = "自来水管道噪声分析系统";


        public static SQLSyncManager SQLSyncMgr = new SQLSyncManager();
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
