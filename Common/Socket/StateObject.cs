﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Common
{
    public class StateObject
    {
        // Client socket.     
        public Socket workSocket = null;
        // Size of receive buffer.     
        public const int BufferSize = 4096;
        // Receive buffer.     
        public byte[] buffer = new byte[BufferSize];
        // Receive buffer. 仅用于SmartWater连接使用
        public List<byte> lstBuffer = new List<byte>();
        /// <summary>
        /// 部分未处理数据缓存(仅用于SmartWater连接使用)
        /// </summary>
        public string msgpart = "";
        // Received data string.     
        public List<Package> lstPack = new List<Package>();

        public void AppendBuffer(Package pack)
        {
            lstPack.Add(pack);
        }
    }
}
