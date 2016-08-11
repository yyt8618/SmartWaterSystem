using System.Net.Sockets;
using System.Collections.Generic;
using System;

namespace Common
{
    public class SmartSocketEntity
    {
        public SmartSocketEntity(Socket socket,string IP, string ID)
        {
            this._clientSocket = socket;
            this._IP = IP;
            this._ID = ID;
        }

        private Socket _clientSocket;
        /// <summary>
        /// smart终端连接socket对象
        /// </summary>
        public Socket ClientSocket
        {
            get { return _clientSocket; }
            set { _clientSocket = value; }
        }

        private string _ID;
        /// <summary>
        /// smart终端的ID
        /// </summary>
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _IP;
        public string IP
        {
            get { return _IP; }
            set { _IP = value; }
        }
        
        private List<string> _MsgBuff = new List<string>();
        /// <summary>
        /// 消息缓存,如果发送的消息有失败的,缓存到这里
        /// </summary>
        public List<string> MsgBuff
        {
            get { return _MsgBuff; }
            set { _MsgBuff = value; }
        }
    }
}
