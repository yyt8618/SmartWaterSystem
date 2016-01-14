using System;
using System.ServiceProcess;
using System.Threading;
using System.Messaging;
using Common;
using Entity;
using System.Diagnostics;
using System.Reflection;
using System.IO;

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
            if (string.IsNullOrEmpty(Settings.Instance.GetString(SettingKeys.DBString)))
            {
                SendMessage(DateTime.Now.ToString() + " 数据库连接未配置,请配置后重启服务!\r\n",ConstValue.MSMQTYPE.Msg_Public);
                logger.Info(DateTime.Now.ToString() + " 数据库连接未配置,请配置后重启服务!");
                this.OnStop();
                return;
            }

            SQLHelper.ConnectionString = Settings.Instance.GetString(SettingKeys.DBString);

            GlobalValue.Instance.SocketSQLMag.Send(SQLType.GetSendParm); //获得上传参数
            GlobalValue.Instance.SocketSQLMag.Send(SQLType.GetUniversalConfig); //获取解析帧的配置数据

            GlobalValue.Instance.SocketMag.cmdEvent += new cmdEventHandle(socketService_cmdEvent);
            GlobalValue.Instance.SocketMag.T_Listening();

            GlobalValue.Instance.HttpService.HTTPMessageEvent += new HTTPReceiveMessage(HttpService_HTTPMessageEvent);
            GlobalValue.Instance.HttpService.Start();

            if (CreateMSMQPath())
            {
                t_msmq_receive = new Thread(new ThreadStart(MSMQReceiveThread));
                t_msmq_receive.Start();
            }

            if (Settings.Instance.GetBool(SettingKeys.ServiceMonitorEnable))
            {
                try
                {
                    ProcessStartInfo monitorInfo = new ProcessStartInfo();
                    monitorInfo.FileName = "GCGPRSServiceMonitor.exe";
                    monitorInfo.WorkingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    Process.Start(monitorInfo);
                    SendMessage(DateTime.Now.ToString() + " 启用服务监控成功!", ConstValue.MSMQTYPE.Msg_Public);
                }
                catch (Exception ex)
                {
                    logger.ErrorException("启用服务监控失败[OnStart]", ex);
                    SendMessage(DateTime.Now.ToString() + " 启用服务监控失败!", ConstValue.MSMQTYPE.Msg_Public);
                }
            }
            else
            {
                SendMessage(DateTime.Now.ToString() + " 配置不启用服务监控!", ConstValue.MSMQTYPE.Msg_Public);
            }
        }


        private void MSMQReceiveThread()  //MSMQ接受线程,用于招测命令接收
        {
            try
            {
                //if (MessageQueue.Exists(string.Format(ConstValue.MSMQPathToService, ".")))
                //{
                //    MessageQueue.Delete(ConstValue.MSMQPathToService);
                //}
                while (true)
                {
                    t_msmq_receive.Join(2000);
                    ServiceController serviceController1 = ServiceManager.GetService(ConstValue.MSMQServiceName);
                    if (serviceController1 != null)
                    {
                        MessageQueue MQueue;
                        //if (MessageQueue.Exists(string.Format(ConstValue.MSMQPathToService, ".")))
                        //{
                            MQueue = new MessageQueue(string.Format(ConstValue.MSMQPathToService, "."));
                        //}
                        //else
                        //{
                        //    MQueue = MessageQueue.Create(string.Format(ConstValue.MSMQPathToService, "."));
                        //    MQueue.SetPermissions("Administrators", MessageQueueAccessRights.FullControl);
                        //    MQueue.Label = "GCGprsMSMQ";
                        //}

                        //一次读取全部消息,但是不去除读过的消息
                        System.Messaging.Message[] Msg = MQueue.GetAllMessages();
                        //删除所有消息
                        MQueue.Purge();

                        foreach (System.Messaging.Message m in Msg)
                        {
                            m.Formatter = new BinaryMessageFormatter();
                            try
                            {
                                MSMQEntity msmqMsg = (MSMQEntity)m.Body;
                                if (msmqMsg != null)
                                {
                                    if (msmqMsg.MsgType == ConstValue.MSMQTYPE.Cmd_Online)
                                        GlobalValue.Instance.SocketMag.SetOnLineClient(msmqMsg.DevType, msmqMsg.DevId, msmqMsg.AllowOnline);
                                    else if (msmqMsg.MsgType == ConstValue.MSMQTYPE.Get_OnLineState)
                                        GlobalValue.Instance.SocketMag.GetTerminalOnLineState();
                                    else if (msmqMsg.MsgType == ConstValue.MSMQTYPE.Cmd_CallData && msmqMsg.CallDataType != null)
                                        GlobalValue.Instance.SocketMag.ClientCallData(msmqMsg.DevType, msmqMsg.DevId, msmqMsg.CallDataType);
                                    else if (msmqMsg.MsgType == ConstValue.MSMQTYPE.SQL_Syncing)
                                        GlobalValue.Instance.SocketMag.SetSQL_SyncingStatus();
                                    else if (msmqMsg.MsgType == ConstValue.MSMQTYPE.SL651_Cmd)
                                    {
                                        if (msmqMsg.Pack651 != null)
                                            GlobalValue.Instance.SocketMag.Send651Cmd(msmqMsg.Pack651);
                                        GlobalValue.Instance.SocketMag.GetSL651WaitSendCmd();  //发送完毕获取待发送SL651命令列表
                                    }
                                    else if (msmqMsg.MsgType == ConstValue.MSMQTYPE.Set_SL651_AllowOnlineFlag)
                                    {
                                        GlobalValue.Instance.SocketMag.SetSL651AllowOnLineFlag(msmqMsg.Msg.ToLower() == "true");
                                        GlobalValue.Instance.SocketMag.GetSL651AllowOnLineFlag();
                                    }
                                    else if (msmqMsg.MsgType == ConstValue.MSMQTYPE.Get_SL651_AllowOnlineFlag)
                                        GlobalValue.Instance.SocketMag.GetSL651AllowOnLineFlag();
                                    else if (msmqMsg.MsgType == ConstValue.MSMQTYPE.Get_SL651_WaitSendCmd)
                                    {
                                        GlobalValue.Instance.SocketMag.GetSL651WaitSendCmd();
                                    }
                                    else if (msmqMsg.MsgType == ConstValue.MSMQTYPE.Del_SL651_WaitSendCmd)
                                    {
                                        //del
                                        GlobalValue.Instance.SocketMag.DelSL651WaitSendCmd(msmqMsg.A1, msmqMsg.A2, msmqMsg.A3, msmqMsg.A4, msmqMsg.A5, msmqMsg.SL651Funcode);
                                        GlobalValue.Instance.SocketMag.GetSL651WaitSendCmd();
                                    }

                                    //Other
                                }
                            }
                            catch (Exception ex)
                            {
                                logger.ErrorException("解析数据异常", ex);
                                MSMQEntity msmqMsg = new MSMQEntity();
                                msmqMsg.MsgType = ConstValue.MSMQTYPE.Msg_Public;
                                msmqMsg.Msg = "解析JSON数据异常(MSMQ)";
                                SendMessage(msmqMsg);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("MSMQReceiveThread", ex);
                //SendMessage("创建接受消息队列失败!");
            }
        }

        protected override void OnStop()
        {
            if (t_msmq_receive != null)
                t_msmq_receive.Abort();

            GlobalValue.Instance.SocketSQLMag.Stop();

            GlobalValue.Instance.SocketMag.cmdEvent -= new cmdEventHandle(socketService_cmdEvent);
            GlobalValue.Instance.SocketMag.Close();

            GlobalValue.Instance.HttpService.HTTPMessageEvent -= new HTTPReceiveMessage(HttpService_HTTPMessageEvent);
            GlobalValue.Instance.HttpService.Stop();
        }

        void socketService_cmdEvent(object sender, SocketEventArgs e)
        {
            if (e.JsonMsg!=null)
            {
                SendMessage(e.JsonMsg);
            }
        }


        void HttpService_HTTPMessageEvent(HTTPMsgEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Msg))
            {
                SendMessage(e.Msg, ConstValue.MSMQTYPE.Msg_HTTP);
            }
        }

        bool CreateMSMQPath()
        {
            try
            {
                MessageQueue MQueue;
                if (!MessageQueue.Exists(string.Format(ConstValue.MSMQPathToService, ".")))
                {
                    MQueue = MessageQueue.Create(string.Format(ConstValue.MSMQPathToService, "."));
                    MQueue.SetPermissions("Everyone", MessageQueueAccessRights.FullControl);
                    MQueue.SetPermissions("ANONYMOUS LOGON", MessageQueueAccessRights.FullControl);
                    MQueue.Label = "GCGprsMSMQ";
                }

                if (!MessageQueue.Exists(string.Format(ConstValue.MSMQPathToUI, ".")))
                {
                    MQueue = MessageQueue.Create(string.Format(ConstValue.MSMQPathToUI, "."));
                    MQueue.SetPermissions("Everyone", MessageQueueAccessRights.FullControl);    //MQueue.SetPermissions("Administrators", MessageQueueAccessRights.FullControl);
                    MQueue.SetPermissions("ANONYMOUS LOGON", MessageQueueAccessRights.FullControl);
                    MQueue.Label = "GCGprsMSMQ";
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("CreateMSMQPath", ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 发送到UI
        /// </summary>
        /// <param name="msg"></param>
        void SendMessage(MSMQEntity msg)
        {
            try
            {
                MessageQueue MQueue;
                //if (!MessageQueue.Exists(string.Format(ConstValue.MSMQPathToUI,".")))
                //{
                //    return;
                //}

                MQueue = new MessageQueue(string.Format(ConstValue.MSMQPathToUI,"."));

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

        /// <summary>
        /// 发送到UI
        /// </summary>
        /// <param name="msg"></param>
        void SendMessage(string msg,ConstValue.MSMQTYPE type)
        {
            try
            {
                MessageQueue MQueue;
                //if (!MessageQueue.Exists(string.Format(ConstValue.MSMQPathToUI,".")))
                //{
                //    return;
                //}

                MQueue = new MessageQueue(string.Format(ConstValue.MSMQPathToUI,"."));

                MSMQEntity mEntity = new MSMQEntity();
                mEntity.MsgType = type;
                mEntity.Msg = msg;

                Message Msg = new Message();
                Msg.Body = mEntity;
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
