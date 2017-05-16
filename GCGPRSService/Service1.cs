using System;
using System.ServiceProcess;
using System.Threading;
using Common;
using Entity;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

namespace GCGPRSService
{
    public partial class Service1 : ServiceBase
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("Service1");
        Thread t_msmq_receive=null;
        public delegate void DumpThreadCallback();  //主线程Dump动作回调
        AutoResetEvent DumpEvent = new AutoResetEvent(false);

        public Service1()
        {
            InitializeComponent();
            this.CanStop = true;
        }

        //public void OnStart(string[] args)
        protected override void OnStart(string[] args)
        {
            GlobalValue.Instance.lstStartRecord.Clear();

            GlobalValue.Instance.lstStartRecord.Add("当前服务版本号:" + Assembly.GetExecutingAssembly().GetName().Version.ToString());

            if (string.IsNullOrEmpty(Settings.Instance.GetString(SettingKeys.DBString)))
            {
                GlobalValue.Instance.lstStartRecord.Add(DateTime.Now.ToString() + " 数据库连接未配置,请配置后重启服务!\r\n");
                logger.Info(DateTime.Now.ToString() + " 数据库连接未配置,请配置后重启服务!");
                this.OnStop();
                return;
            }

            SQLHelper.ConnectionString = Settings.Instance.GetString(SettingKeys.DBString);

            GlobalValue.Instance.SocketSQLMag.Send(SQLType.GetSendParm); //获得上传参数
            GlobalValue.Instance.SocketSQLMag.Send(SQLType.GetUniversalConfig); //获取解析帧的配置数据
            GlobalValue.Instance.SocketSQLMag.Send(SQLType.GetAlarmType);  //获取报警类型列表
            GlobalValue.Instance.SocketSQLMag.Send(SQLType.GetOffsetValue); //获取偏移值

            GlobalValue.Instance.SocketMag.Dumpcallback = DumpSet;
            GlobalValue.Instance.SocketMag.T_Listening();

            GlobalValue.Instance.HttpService.HTTPMessageEvent += new HTTPReceiveMessage(HttpService_HTTPMessageEvent);
            GlobalValue.Instance.HttpService.Start();
            
            if (Settings.Instance.GetBool(SettingKeys.ServiceMonitorEnable))
            {
                try
                {
                    ProcessStartInfo monitorInfo = new ProcessStartInfo();
                    monitorInfo.FileName = "GCGPRSServiceMonitor.exe";
                    monitorInfo.WorkingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    Process.Start(monitorInfo);
                    GlobalValue.Instance.lstStartRecord.Add(DateTime.Now.ToString() + " 启用服务监控成功!");
                }
                catch (Exception ex)
                {
                    logger.ErrorException("启用服务监控失败[OnStart]", ex);
                    GlobalValue.Instance.lstStartRecord.Add(DateTime.Now.ToString() + " 启用服务监控失败!");
                }
            }
            else
            {
                GlobalValue.Instance.lstStartRecord.Add(DateTime.Now.ToString() + " 配置不启用服务监控!");
            }

            while(true)
            {
                //DumpEvent.Reset();
                DumpEvent.WaitOne();
                CreateDump();
            }
        }

        protected override void OnStop()
        {
            if (t_msmq_receive != null)
                t_msmq_receive.Abort();

            GlobalValue.Instance.SocketSQLMag.Stop();
            
            GlobalValue.Instance.SocketMag.Close();

            GlobalValue.Instance.HttpService.HTTPMessageEvent -= new HTTPReceiveMessage(HttpService_HTTPMessageEvent);
            GlobalValue.Instance.HttpService.Stop();
        }

        private void DumpSet()
        {
            DumpEvent.Set();
        }

        private void CreateDump()
        {
            try
            {
                string dumpPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Dump\\";
                if (!Directory.Exists(dumpPath))
                {
                    Directory.CreateDirectory(dumpPath);
                }
                string dumpexepath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "procdump.exe");
                if (!File.Exists(dumpexepath))
                {

                    GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.Other, DateTime.Now.ToString() + " 不存在procdump.exe程序文件,退出生成转储文件!"));
                }
                else
                {
                    //Process current = Process.GetCurrentProcess();
                    //Process procdump = Process.Start(dumpexepath, " -ma " + current.Id + " " + dumpPath);
                    //Thread.Sleep(15 * 1000);
                    //procdump.WaitForExit();

                    string dumpFile = dumpPath + "\\MiniDmp" + DateTime.Now.ToString("yyMMddHHmm") + ".dmp";
                    if (File.Exists(dumpFile))
                        File.Delete(dumpFile);
                    using (FileStream fs = new FileStream(dumpFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Write))
                    {
                        SmartWaterSystem.MiniDump.Write(fs.SafeFileHandle, SmartWaterSystem.MiniDump.Option.WithFullMemory);
                    }
                    GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.Other, DateTime.Now.ToString() + " 生成dmp文件成功,位置:" + dumpPath));
                }
            }
            catch(Exception ex)
            {
                logger.ErrorException("CreateDump", ex);
                GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.Error, DateTime.Now.ToString() + " 生成dmp文件失败,ex:" + ex.Message));
            }
        }


        void HttpService_HTTPMessageEvent(HTTPMsgEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Msg))
            {
                GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.CellPhone,e.Msg));
            }
        }
    }
}
