using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace SmartWaterSystem
{
    public class SocketHelper
    {
        public static byte[] SocketByteSplit = new byte[]{ 0x40, 0x40, 0x40 };// Encoding.UTF8.GetBytes(SocketMsgSplit);
        public static string SocketMsgSplit = "@@@";


        public static bool IsSocketConnected_Poll(System.Net.Sockets.Socket socket)
        {
            #region remarks
            /* As zendar wrote, it is nice to use the Socket.Poll and Socket.Available, but you need to take into conside                ration
             * that the socket might not have been initialized in the first place.
             * This is the last (I believe) piece of information and it is supplied by the Socket.Connected property.
             * The revised version of the method would looks something like this:
             * from：http://stackoverflow.com/questions/2661764/how-to-check-if-a-socket-is-connected-disconnected-in-c */
            #endregion

            #region 过程
            if (socket == null)
                return false;
            return !((socket.Poll(1000, SelectMode.SelectRead) && (socket.Available == 0)) || !socket.Connected);
            #endregion
        }

        /// <summary>
        /// 拆分Socket消息
        /// </summary>
        /// <param name="lstBuffer">接收到的数据</param>
        /// <param name="msgpart">上次部分不完整数据缓存</param>
        /// <returns></returns>
        public static string[] SplitSocketMsg(ref List<byte> lstBuffer,ref string msgpart)
        {
            string strmsg = "";
            int firstsplitindex = -1;  //第一个分隔符位置
            int tmpindex = -1;
            int lastsplitindex = -1;   //最后一个分隔符位置
            for (int i = SocketHelper.SocketByteSplit.Length - 1; i < lstBuffer.Count; i++)
            {
                bool iscontain = true;  //是否包含分隔符标记
                for (int j = 0; j < SocketHelper.SocketByteSplit.Length; j++)  //判断是否包含分隔符
                {
                    if (lstBuffer[i - (SocketHelper.SocketByteSplit.Length - 1) + j] != SocketHelper.SocketByteSplit[j])
                    {
                        iscontain = false;
                        break;
                    }
                }
                if (iscontain)
                {
                    if (firstsplitindex == -1)
                        firstsplitindex = i;
                    else
                    {
                        tmpindex = lastsplitindex;
                        lastsplitindex = i;
                    }
                }
            }
            if ((lastsplitindex >= SocketHelper.SocketByteSplit.Length * 2) && (tmpindex == lastsplitindex - 1))
            {
                //检查是否有连在一起的分隔符 如:"@@@@@@",如果有则取倒数第二个分隔符的位置
                bool iscontain = true;
                for (int j = 0; j < SocketHelper.SocketByteSplit.Length; j++)
                {
                    if (lstBuffer[lastsplitindex - (SocketHelper.SocketByteSplit.Length * 2 - 1) + j] != SocketHelper.SocketByteSplit[j])
                    {
                        iscontain = false;
                        break;
                    }
                }
                if (iscontain)
                    lastsplitindex = lastsplitindex - SocketHelper.SocketByteSplit.Length;
            }

            if (lastsplitindex < (SocketHelper.SocketByteSplit.Length * 2 - 1))
            {
                return null;
            }
            strmsg = Encoding.UTF8.GetString(lstBuffer.ToArray(), firstsplitindex - (SocketHelper.SocketByteSplit.Length - 1), lastsplitindex - firstsplitindex + SocketHelper.SocketByteSplit.Length);
            lstBuffer.RemoveRange(0, lastsplitindex + 1);

            if (!string.IsNullOrEmpty(strmsg))
            {
                if (!string.IsNullOrEmpty(msgpart) && !strmsg.StartsWith(SocketHelper.SocketMsgSplit))  //如果分包了则拼接成完整一包
                {
                    strmsg = msgpart + strmsg;
                }
                msgpart = "";
                if (!strmsg.EndsWith(SocketHelper.SocketMsgSplit))  //如果不是以@@@结尾，说明该数据分多包发送的,需要把结尾数据保存至msgpart中，在下一包收到后拼接成完成一包
                {
                    int index = strmsg.LastIndexOf(SocketHelper.SocketMsgSplit);
                    if (index > 0)
                    {
                        msgpart = strmsg.Substring(index + 3);
                        strmsg = strmsg.Substring(0, index);
                    }
                }

                string[] strmsgs = strmsg.Split(new string[] { SocketHelper.SocketMsgSplit }, StringSplitOptions.RemoveEmptyEntries);
                return strmsgs;
            }
            return null;
        }
    }
}
