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
       

        public Service1()
        {
            InitializeComponent();
            this.CanStop = true;
        }

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


        void HttpService_HTTPMessageEvent(HTTPMsgEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Msg))
            {
                SocketEntity mEntity = new SocketEntity();
                mEntity.MsgType = ConstValue.MSMQTYPE.Msg_HTTP;
                mEntity.Msg = e.Msg;
                GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(mEntity));
            }
        }
    }
}
