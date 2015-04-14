using System;
using System.ServiceProcess;
using System.Threading;
using System.Messaging;
using Common;
using Entity;
using Newtonsoft.Json;

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
                SendMessage(DateTime.Now.ToString() + " 数据库连接未配置,请配置后重启服务!\r\n");
                logger.Info(DateTime.Now.ToString() + " 数据库连接未配置,请配置后重启服务!");
                this.OnStop();
                return;
            }
            SQLHelper.ConnectionString = Settings.Instance.GetString(SettingKeys.DBString);

            GlobalValue.Instance.SocketSQLMag.Send(SQLType.GetSendParm); //获得上传参数
            GlobalValue.Instance.SocketSQLMag.Send(SQLType.GetUniversalConfig); //获取解析帧的配置数据

            GlobalValue.Instance.SocketMag.cmdEvent += new cmdEventHandle(socketService_cmdEvent);
            GlobalValue.Instance.SocketMag.T_Listening();

            t_msmq_receive = new Thread(new ThreadStart(MSMQReceiveThread));
            t_msmq_receive.Start();
        }

        private void MSMQReceiveThread()  //MSMQ接受线程,用于招测命令接收
        {
            try
            {
                if (MessageQueue.Exists(ConstValue.MSMQPathToService))
                {
                    MessageQueue.Delete(ConstValue.MSMQPathToService);
                }
                while (true)
                {
                    t_msmq_receive.Join(2000);
                    ServiceController serviceController1 = ServiceManager.GetService(ConstValue.MSMQServiceName);
                    if (serviceController1 != null)
                    {
                        MessageQueue MQueue;
                        if (MessageQueue.Exists(ConstValue.MSMQPathToService))
                        {
                            MQueue = new MessageQueue(ConstValue.MSMQPathToService);
                        }
                        else
                        {
                            MQueue = MessageQueue.Create(ConstValue.MSMQPathToService);
                            MQueue.SetPermissions("Administrators", MessageQueueAccessRights.FullControl);
                            MQueue.Label = "GCGprsMSMQ";
                        }

                        //一次读取全部消息,但是不去除读过的消息
                        System.Messaging.Message[] Msg = MQueue.GetAllMessages();
                        //删除所有消息
                        MQueue.Purge();

                        foreach (System.Messaging.Message m in Msg)
                        {
                            m.Formatter = new BinaryMessageFormatter();
                            try
                            {
                                MSMQEntity msmqMsg = (MSMQEntity)JsonConvert.DeserializeObject(m.Body.ToString(), typeof(MSMQEntity));
                                if (msmqMsg != null)
                                {
                                    if (msmqMsg.MsgType == ConstValue.MSMQTYPE.Cmd_Online)
                                        GlobalValue.Instance.SocketMag.SetOnLineClient(msmqMsg.DevType, msmqMsg.DevId, msmqMsg.AllowOnline);
                                    else if (msmqMsg.MsgType == ConstValue.MSMQTYPE.Get_OnLineState)
                                        GlobalValue.Instance.SocketMag.GetTerminalOnLineState();
                                    //Other
                                }
                            }
                            catch (Exception ex)
                            {
                                logger.ErrorException("解析数据异常", ex);
                                MSMQEntity msmqMsg = new MSMQEntity();
                                msmqMsg.MsgType = ConstValue.MSMQTYPE.Message;
                                msmqMsg.Msg = "解析JSON数据异常(MSMQ)";
                                SendMessage(JsonConvert.SerializeObject(msmqMsg));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("MSMQReceiveThread", ex);
                SendMessage("创建接受消息队列失败!");
            }
        }

        protected override void OnStop()
        {
            if (t_msmq_receive != null)
                t_msmq_receive.Abort();

            GlobalValue.Instance.SocketSQLMag.Stop();

            GlobalValue.Instance.SocketMag.cmdEvent -= new cmdEventHandle(socketService_cmdEvent);
            GlobalValue.Instance.SocketMag.Close();
        }

        void socketService_cmdEvent(object sender, SocketEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.JsonMsg))
            {
                SendMessage(e.JsonMsg);
            }
        }

        /// <summary>
        /// 发送到UI
        /// </summary>
        /// <param name="msg"></param>
        void SendMessage(string msg)
        {
            try
            {
                MessageQueue MQueue;
                if (!MessageQueue.Exists(ConstValue.MSMQPathToUI))
                {
                    return;
                }

                MQueue = new MessageQueue(ConstValue.MSMQPathToUI);

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
