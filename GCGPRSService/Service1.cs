using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Messaging;
using Common;

namespace GCGPRSService
{
    public partial class Service1 : ServiceBase
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("Service1");
        string QueuePath = ".\\private$\\GcGPRS";
        public Service1()
        {
            InitializeComponent();
            this.CanStop = true;
           
        }

        protected override void OnStart(string[] args)
        {
            if (string.IsNullOrEmpty(Settings.Instance.GetString(SettingKeys.DBString)))
            {
                SendMessage(DateTime.Now.ToString() + " 数据库连接未配置,请配置后重启服务!\r\n");
                logger.Info(DateTime.Now.ToString() + " 数据库连接未配置,请配置后重启服务!");
                this.OnStop();
                return;
            }
            SQLHelper.ConnectionString = Settings.Instance.GetString(SettingKeys.DBString);

            GlobalValue.Instance.SocketSQLMag.Send(SQLType.GetSendParm); //获得上传参数
            GlobalValue.Instance.SocketMag.cmdEvent += new cmdEventHandle(socketService_cmdEvent);
            GlobalValue.Instance.SocketMag.T_Listening();
        }

        protected override void OnStop()
        {
            GlobalValue.Instance.SocketSQLMag.Stop();

            GlobalValue.Instance.SocketMag.cmdEvent -= new cmdEventHandle(socketService_cmdEvent);
            GlobalValue.Instance.SocketMag.Close();
        }

        void socketService_cmdEvent(object sender, SocketEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Msg))
            {
                SendMessage(e.Msg);
            }
        }

        void SendMessage(string msg)
        {
            try
            {
                MessageQueue MQueue;

                if (!MessageQueue.Exists(QueuePath))
                {
                    return;
                }

                MQueue = new MessageQueue(QueuePath);

                Message Msg = new Message();
                Msg.Body = msg;
                Msg.Formatter = new BinaryMessageFormatter();
                MQueue.Send(Msg);
            }
            catch (Exception ex)
            {
                logger.ErrorException("SendMessage", ex);
            }
        }
    }
}
