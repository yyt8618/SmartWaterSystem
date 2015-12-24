using System;
using Common;
using System.Threading;
using System.ServiceProcess;
using System.Messaging;
using Entity;

namespace SmartWaterSystem
{
    public class MSMQEventArgs : EventArgs
    {
        private MSMQEntity _msmqEntity;
        public MSMQEntity msmqEntity
        {
            get { return _msmqEntity; }
            set { _msmqEntity = value; }
        }

        public MSMQEventArgs(MSMQEntity msmqEntity)
        {
            this._msmqEntity = msmqEntity;
        }
    }

    public delegate void MSMQHandler(object sender, MSMQEventArgs e);
    public class MSMQManager
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("MSMQManager");
        public event MSMQHandler MSMQEvent;

        Thread t = null;
        public MSMQManager()
        {
        }

        ~MSMQManager()
        {
        }

        private void OnMSMQEvent(MSMQEventArgs e)
        {
            if (MSMQEvent != null)
                MSMQEvent(this, e);
        }

        public bool Start()
        {
            t = new Thread(new ThreadStart(MSMQReceiveThread));
            t.Start();
            return true;
        }

        public void Stop()
        {
            try
            {
                t.Abort();
            }
            catch { }
        }

        /// <summary>
        /// 发送到服务
        /// </summary>
        /// <param name="msg"></param>
        public void SendMessage(MSMQEntity msmqEntity)
        {
            try
            {
                if (msmqEntity == null)
                    return;
                MessageQueue MQueue;
                //if (!MessageQueue.Exists(ConstValue.MSMQPathToService))
                //{
                //    return;
                //}

                MQueue = new MessageQueue(string.Format("FormatName:Direct=TCP:"+ConstValue.MSMQPathToService,Settings.Instance.GetString(SettingKeys.MSMQIpAddr)));

                Message Msg = new Message();
                Msg.Body = msmqEntity;
                Msg.Formatter = new BinaryMessageFormatter();
                MQueue.Send(Msg);
            }
            catch (Exception ex)
            {
                logger.ErrorException("SendMessage", ex);
            }
        }

        public void GetServiceStopMsg()
        {
            showstopmsg = false;
        }

        bool showstopmsg = false;
        private void MSMQReceiveThread()
        {
            try
            {
                while (true)
                {
                    Thread.CurrentThread.Join(1000);
                    //ServiceController serviceController1 = ServiceManager.GetService(ConstValue.MSMQServiceName);

                    //if (serviceController1 != null)
                    //{
                    //    serviceController1.Refresh();

                    //    if (serviceController1.Status == ServiceControllerStatus.Stopped)
                    //    {
                    //        if (!showstopmsg)
                    //        {
                    //            OnMSMQEvent(new MSMQEventArgs(new MSMQEntity(Entity.ConstValue.MSMQTYPE.Message, "服务已停止!")));
                    //        }
                    //        showstopmsg = true;
                    //    }
                    //    else
                    //        showstopmsg = false;

                        MessageQueue MQueue;

                        //if (MessageQueue.Exists(ConstValue.MSMQPathToUI))
                        //{
                            MQueue = new MessageQueue(string.Format("FormatName:Direct=TCP:"+ConstValue.MSMQPathToUI,Settings.Instance.GetString(SettingKeys.MSMQIpAddr)));
                        //}
                        //else
                        //{
                        //    MQueue = MessageQueue.Create(ConstValue.MSMQPathToUI);
                        //    MQueue.SetPermissions("Administrators", MessageQueueAccessRights.FullControl);
                        //    MQueue.Label = "GCGprsMSMQ";
                        //}

                        //一次读取全部消息,但是不去除读过的消息
                            try
                            {
                                System.Messaging.Message[] Msg = MQueue.GetAllMessages();
                                //删除所有消息
                                MQueue.Purge();

                                if (Msg != null)
                                {
                                    int i = 0;
                                    if (Msg.Length > 1000)
                                        i = Msg.Length - 1000;
                                    for (; i < Msg.Length; i++)
                                    {
                                        Msg[i].Formatter = new BinaryMessageFormatter();
                                        MSMQEntity msmqEntity = (MSMQEntity)Msg[i].Body;
                                        OnMSMQEvent(new MSMQEventArgs(msmqEntity));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                OnMSMQEvent(new MSMQEventArgs(new MSMQEntity(Entity.ConstValue.MSMQTYPE.Message, ex.Message)));
                                logger.ErrorException("JSON解析错误,Location:MSMQManager.MSMQReceiveThread", ex);
                            }
                    //}
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetServiceStatus", ex);
                OnMSMQEvent(new MSMQEventArgs(new MSMQEntity(Entity.ConstValue.MSMQTYPE.Message, ex.Message)));
            }
        }

    }
}
