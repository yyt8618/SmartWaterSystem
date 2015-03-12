using System;

namespace Common
{
    /// <summary>
    /// 校验码计算
    /// </summary>
    public static class CheckHelper
    {
        #region 校验码计算的方法
        /// <summary>
        /// 返回协议验证码，默认命令字的开始位置为第二位
        /// </summary>
        /// <param name="hex">协议串/param>
        /// <returns>16进制字符串表示的异或计算结果</returns>
        private static string HexXorToString(string hex)
        {
            string[] hexTemp = hex.Split(' ');
            int k = 0;
            for (int i = 0; i < hexTemp.Length; i++)
            {
                int j = Convert.ToInt32(hexTemp[i], 16);

                if (i == 1)
                {
                    k = j;
                }
                else if (i > 1)
                {
                    k = k ^ j;
                }

            }
            string hexResult = string.Format("{0:X}", k % 256);//保证得到一个字节长度的值
            if (hexResult.Length == 1)
            {
                hexResult = string.Format("0{0}", hexResult);
            }
            return hexResult;
        }

        /// <summary>
        /// 返回协议验证码
        /// </summary>
        /// <param name="hex">协议串</param>
        /// <param name="cmdIndex">命令字的开始位置</param>
        /// <returns>验证码</returns>
        private static string HexXorToString(string hex, int cmdIndex)
        {
            string[] hexTemp = hex.Split(' ');
            int k = 0;
            for (int i = cmdIndex; i < hexTemp.Length; i++)
            {
                int j = Convert.ToInt32(hexTemp[i], 16);

                if (i == cmdIndex)
                {
                    k = j;
                }
                else if (i > cmdIndex)
                {
                    k = k ^ j;
                }

            }
            string hexResult = string.Format("{0:X}", k % 256);//保证得到一个字节长度的值
            if (hexResult.Length == 1)
            {
                hexResult = string.Format("0{0}", hexResult);
            }
            return hexResult;
        }

        /// <summary>
        /// 16进制相加的值
        /// </summary>
        /// <param name="hex1"></param>
        /// <param name="hex2"></param>
        /// <returns></returns>
        private static string HexAddToString(string hex1, string hex2)
        {
            int h1 = Convert.ToInt32(hex1, 16);
            int h2 = Convert.ToInt32(hex2, 16);
            int sum = (h1 + h2) % 256;//保证得到一个字节长度的值
            string hexResult = string.Format("{0:X}", sum);

            if (hexResult.Length == 1)
            {
                hexResult = string.Format("0{0}", hexResult);
            }

            return hexResult;
        }

        /// <summary>
        /// 返回带验证码的协议命令
        /// </summary>
        /// <param name="hex">协议串</param>
        /// <returns>带验证码的协议命</returns>
        public static string GetSendProtocol(string hex)
        {
            string hexTemp = string.Empty;
            hexTemp = HexXorToString(hex);

            string hexResult = string.Empty;
            string hexTemp1 = HexAddToString(hexTemp, "37");
            string hexTemp2 = HexAddToString(hexTemp, "D5");
            string hexTemp3 = HexAddToString(hexTemp, "9E");
            hexResult = string.Format("{0} {1} {2} {3}", hex, hexTemp1, hexTemp2, hexTemp3);

            return hexResult;
        }

        /// <summary>
        /// 返回带验证码的协议命令
        /// </summary>
        /// <param name="hex">协议串</param>
        /// <param name="cmdIndex">命令字的开始位置</param>
        /// <returns>带验证码的协议命</returns>
        public static string GetSendProtocol(string hex, int cmdIndex)
        {
            string hexTemp = string.Empty;
            hexTemp = HexXorToString(hex, cmdIndex);

            string hexResult = string.Empty;
            string hexTemp1 = HexAddToString(hexTemp, "37");
            string hexTemp2 = HexAddToString(hexTemp, "D5");
            string hexTemp3 = HexAddToString(hexTemp, "9E");
            hexResult = string.Format("{0} {1} {2} {3}", hex, hexTemp1, hexTemp2, hexTemp3);

            return hexResult;
        }

        /// <summary>
        /// 得到校验码
        /// </summary>
        /// <param name="message">16进制ASCII</param>
        /// <returns></returns>
        private static string GetChecksum(string message)
        {
            char[] chars = message.ToCharArray();
            ushort c;
            ushort total = 0;

            for (int i = 0; i < chars.Length; i++)
            {
                total += (ushort)chars[i];
            }

            c = (ushort)(0 - total);
            string hex = Convert.ToString(c, 16);
            return hex.ToUpper();
        }
        /// <summary>
        /// 检查校验码是否相等
        /// </summary>
        /// <param name="message">16进制ASCII</param>
        /// <param name="checksum">16进制校验码</param>
        /// <returns>是否相等</returns>
        private static bool CalculateChecksum(string message, string checksum)
        {
            char[] chars = message.ToCharArray();
            ushort c = Convert.ToUInt16(checksum, 16);
            ushort total = 0;

            for (int i = 0; i < chars.Length; i++)
            {
                total += (ushort)chars[i];
            }

            if ((ushort)(c + total) == 0) return true;
            return false;
        }
        #endregion
    }
}
