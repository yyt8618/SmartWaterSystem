using System;
using Common;
using System.Threading;
using System.ServiceProcess;
using System.Messaging;
using Entity;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace SmartWaterSystem
{
    public class SocketEventArgs : EventArgs
    {
        private SocketEntity _msmqEntity;
        public SocketEntity msmqEntity
        {
            get { return _msmqEntity; }
            set { _msmqEntity = value; }
        }

        public SocketEventArgs(SocketEntity msmqEntity)
        {
            this._msmqEntity = msmqEntity;
        }
    }

    public class SocketStatusEventArgs:EventArgs
    {
        private bool _Connect = false;
        /// <summary>
        /// Socket连接状态
        /// </summary>
        public bool Connect
        {
            get { return _Connect; }
            set { _Connect = value; }
        }

        public SocketStatusEventArgs(bool isconnect)
        {
            _Connect = isconnect;
        }
    }

    public delegate void SocketHandler(object sender, SocketEventArgs e);
    public delegate void SocketConnHandle(object sender, SocketStatusEventArgs e);
    public class SocketManager
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("MSMQManager");
        public event SocketHandler SockMsgEvent;
        public event SocketConnHandle SocketConnEvent;
        string SmartWaterHeartBeat = SocketHelper.SocketMsgSplit+"heartbeat"+ SocketHelper.SocketMsgSplit;
        Socket socket = null;   //连接的socket对象
        //发送时，一条数据可能有截断的作为两次发送，将前面一包最后不是完整一条(MSMQEntity)的数据保存,并与下一包进行拼接
        public string msgpart = "";
        /// <summary>
        /// socket连接锁定对象
        /// </summary>
        private object obj_conncet = new object();
        
        public SocketManager()
        {
        }

        ~SocketManager()
        {
        }

        private void OnSockMsgEvent(SocketEventArgs e)
        {
            if (SockMsgEvent != null)
                SockMsgEvent(this, e);
        }

        private void OnSockConnEvent(SocketStatusEventArgs e)
        {
            if (SocketConnEvent != null)
                SocketConnEvent(this, e);
        }

        public bool Start()
        {
            Thread heartthread = new Thread(new ThreadStart(HeartPack));
            heartthread.IsBackground = true;
            heartthread.Start();
            
            //if (!Connect())
            //{
            //    return false;
            //}
            
            return true;
        }

        private void HeartPack()
        {
            byte[] data = Encoding.UTF8.GetBytes(SmartWaterHeartBeat);
            while (true)
            {
                Thread.Sleep(60 * 1000);
                
                try
                {
                    socket.Send(data);
                    OnSockConnEvent(new SocketStatusEventArgs(true));
                }
                catch
                {
                    int sendcount = 1;
                    OnSockConnEvent(new SocketStatusEventArgs(false));
                    Thread.Sleep(2 * 1000);
                    HeartPack(sendcount);
                }
            }
        }

        //正在断开连接的时候,不能去连接
        private bool disconnecting = false;
        private void HeartPack(int sendcount = 1)
        {
            byte[] data = Encoding.UTF8.GetBytes(SmartWaterHeartBeat);
            try
            {
                socket.Send(data);
                OnSockConnEvent(new SocketStatusEventArgs(true));
            }
            catch
            {
                OnSockConnEvent(new SocketStatusEventArgs(false));
                Thread.Sleep(5 * 1000);
                if (disconnecting)  //如果是正在断开连接,直接返回
                    return;
                if (sendcount > 3 )  //发送三次后重新连接
                {
                    Connect();
                    OnSockMsgEvent(new SocketEventArgs(new SocketEntity(Entity.ConstValue.MSMQTYPE.Msg_Err, DateTime.Now.ToString()+" 连接失败,正在重新连接！")));
                    return;
                }
                sendcount++;
                HeartPack(sendcount);
            }
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="showconnAddr">是否显示连接地址</param>
        /// <returns></returns>
        public bool Connect(bool showconnAddr = false)
        {
            try
            {
                lock(obj_conncet)
                {
                    if (showconnAddr)
                        OnSockMsgEvent(new SocketEventArgs(new SocketEntity(Entity.ConstValue.MSMQTYPE.Msg_Public, "开始Socket连接," + Settings.Instance.GetString(SettingKeys.GPRS_IP) + ":" + Settings.Instance.GetInt(SettingKeys.GPRS_PORT))));
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.ReceiveTimeout = 10 * 1000;
                    socket.SendTimeout = 10 * 1000;
                    IPAddress ipaddr = null;
                    if (IPAddress.TryParse(Settings.Instance.GetString(SettingKeys.GPRS_IP), out ipaddr))
                    {
                        socket.BeginConnect(ipaddr, Settings.Instance.GetInt(SettingKeys.GPRS_PORT), new AsyncCallback(ConnectCallback), socket);
                    }
                }
                return true;
            }
            catch(Exception ex) {
                OnSockConnEvent(new SocketStatusEventArgs(false));
                OnSockMsgEvent(new SocketEventArgs(new SocketEntity(Entity.ConstValue.MSMQTYPE.Msg_Err, "连接Socket失败！")));
                return false;
            }
        }

        public void ReConnect()
        {
            if (socket != null)
            {
                if (socket.Connected)  //如果连接过,判断地址是否一样
                {
                    //如果还是连接到一样的地址,则不需要去连接
                    if (((System.Net.IPEndPoint)socket.RemoteEndPoint).Address.ToString() == Settings.Instance.GetString(SettingKeys.GPRS_IP))
                        Connect(false);
                    else
                        DisConnect(socket);
                }
                else
                {
                    Connect(false);
                }
            }
            else
                Connect(true);
        }

        public void DisConnect(Socket sock)
        {
            try {
                sock.Shutdown(SocketShutdown.Both);
                Thread.Sleep(20);
                disconnecting = true;
                sock.Close();
                sock = null;
                OnSockMsgEvent(new SocketEventArgs(new SocketEntity(Entity.ConstValue.MSMQTYPE.Msg_Public, DateTime.Now.ToString() + "正在断开Socket重新连接,请稍候...")));
                Connect(true);
            }
            catch(Exception ex)
            {
                ;
            }
            finally
            {
                disconnecting = false;
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {   
                Socket client = (Socket)ar.AsyncState;
                client.EndConnect(ar);
                StateObject state = new StateObject();
                state.workSocket = socket;
                socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                OnSockConnEvent(new SocketStatusEventArgs(true));
                byte[] data = Encoding.UTF8.GetBytes(SmartWaterHeartBeat);
                socket.Send(data);
            }
            catch (Exception e)
            {
                OnSockConnEvent(new SocketStatusEventArgs(false));
                OnSockMsgEvent(new SocketEventArgs(new SocketEntity(Entity.ConstValue.MSMQTYPE.Msg_Err, "连接Socket失败！")));
            }
        }

        public void Stop()
        {
            
        }

        byte[] bsplit = Encoding.UTF8.GetBytes(SocketHelper.SocketMsgSplit); //分隔符
        private void ReceiveCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            try
            {
                if (!state.workSocket.Connected)
                    return;
                int REnd = state.workSocket.EndReceive(ar);
                if (REnd > 0)
                {
                    for (int i = 0; i < REnd; i++)
                    {
                        state.lstBuffer.Add(state.buffer[i]);
                    }

                    string[] strmsgs = SocketHelper.SplitSocketMsg(ref state.lstBuffer, ref state.msgpart);
                    foreach (string strtmp in strmsgs)
                    {
                        if (!string.IsNullOrEmpty(strtmp))
                        {
                            try
                            {
                                SocketEntity msmqEntity = JSONSerialize.JsonDeserialize_Newtonsoft<SocketEntity>(strtmp);
                                if (msmqEntity != null)
                                    OnSockMsgEvent(new SocketEventArgs(msmqEntity));
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    OnSockConnEvent(new SocketStatusEventArgs(true));
                }
                else
                {
                    //dispose();
                }
            }
            catch (SocketException sockex)
            {
                OnSockConnEvent(new SocketStatusEventArgs(false));
            }
            catch (Exception ex)
            {
                msgpart = "";
                OnSockMsgEvent(new SocketEventArgs(new SocketEntity(Entity.ConstValue.MSMQTYPE.Msg_Err, "解析Socket消息发生异常,ex:" + ex.Message)));
            }
            finally
            {
                try
                {
                    socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                }
                catch
                { }
            }
        }

        /// <summary>
        /// 发送到服务
        /// </summary>
        /// <param name="msg"></param>
        public void SendMessage(SocketEntity msmqEntity)
        {
            try
            {
                if (msmqEntity == null)
                    return;
                if(SocketHelper.IsSocketConnected_Poll(socket))
                {
                    string strmsg=JSONSerialize.JsonSerialize_Newtonsoft(msmqEntity);
                    byte[] data = Encoding.UTF8.GetBytes(SocketHelper.SocketMsgSplit + strmsg+ SocketHelper.SocketMsgSplit);
                    socket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), socket);
                    OnSockConnEvent(new SocketStatusEventArgs(true));
                }
                else
                {
                    Connect();
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("SendMessage", ex);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                handler.EndSend(ar);
            }
            catch (SocketException ex)
            { }
        }
        
        
    }
}
