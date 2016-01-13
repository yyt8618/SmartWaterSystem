using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// 进制转换
    /// </summary>
    public static class ConvertHelper
    {
        #region 进制转换
        /// <summary>
        /// 整形转化成字节
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static byte IntToByte(int i)
        {
            return Convert.ToByte(i % 0x100);
        }

        /// <summary>
        /// 16进制转化成BCD
        /// </summary>
        /// <param name="b">byte</param>
        /// <returns>返回BCD码</returns>
        public static byte HexToBCD(byte b)
        {
            int r1 = b % 0x10;     // b = 0x12 -> r1 = 0x02
            int r2 = b / 0x10;     // b = 0x12 -> r2 = 0x01
            if (r1 > 9)             //r1 = 0x0A -> r1 = 0x00
                r1 = 0;
            if (r2 > 9)             //r2 = 0x0A -> r2 = 0x00
                r2 = 0;
            return Convert.ToByte(r1 + 10 * r2);    //0x12 -> 12
        }

        public static byte[] StrToBCD(string str)
        {
            if (str.Length % 2 != 0)
            {
                str = str.PadLeft(str.Length + 1, '0');
            }
            List<byte> lstByte = new List<byte>();
            for (int i = 0; i < str.Length; i += 2)
            {
                lstByte.Add(HexToBCD(Convert.ToByte(str.Substring(i, 2))));
            }
            return lstByte.ToArray();
        }

        /// <summary>
        /// 16进制转化成BCD
        /// </summary>
        /// <param name="i">int</param>
        /// <returns>返回BCD码</returns>
        public static byte HexToBCD(int i)
        {
            return HexToBCD(IntToByte(i));
        }

        /// <summary>
        /// Double转换为压缩BCD
        /// </summary>
        /// <param name="d">0-100内的双精度浮点数字</param>
        /// <returns>返回压缩BCD码</returns>
        public static string DoubleToBCD(double d)
        {
            string[] strs = d.ToString("F2").Split('.');
            string temp1 = int.Parse(strs[0]).ToString("D2");
            string temp2 = int.Parse(strs[1]).ToString("D2");
            return string.Format("{0} {1}", temp1, temp2);
        }

        /// <summary>
        /// long转换为BCD
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static long ToBCD(long val)
        {
            long res = 0;
            int bit = 0;
            while (val >= 10)
            {
                res |= (val % 10 << bit);
                val /= 10;
                bit += 4;
            }
            res |= val << bit;
            return res;
        }

        /// <summary>
        /// BCD转换为long
        /// </summary>
        /// <param name="vals"></param>
        /// <returns></returns>
        public static long FromBCD(long vals)
        {
            long c = 1;
            long b = 0;
            while (vals > 0)
            {
                b += ((vals & 0xf) * c);
                c *= 10;
                vals >>= 4;
            }
            return b;
        }

        /// <summary>
        /// BCD转化成16进制
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static byte BCDToHex(byte b)
        {
            int r2 = b % 100;      // b = 12 -> r2 = 12        //123 -> r2 = 23
            int r1 = r2 % 10;      //r2 = 12 -> r1 = 2     
            r2 = r2 / 10;           //r2 = 12 -> r2 =1
            return Convert.ToByte(r1 + 0x10 * r2);
        }

        /// <summary>
        /// BCD转化成16进制
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static byte BCDToHex(int i)
        {
            return BCDToHex(IntToByte(i));
        }

        /// <summary>
        /// btye转化成16进制字符
        /// </summary>
        /// <param name="b">byte</param>
        /// <returns></returns>
        public static string ToHexString(byte b)
        {
            return Convert.ToString(b, 16);
        }

        /// <summary>
        /// int转化成16进制字符
        /// </summary>
        /// <param name="num">int</param>
        /// <returns></returns>
        public static string ToHexString(int num)
        {
            return Convert.ToString(num, 16);
        }

        /// <summary>
        /// 16进制字符串转换成字节数组
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
            {
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            }
            return buffer;
        }

        /// <summary> 
        /// 字节数组转换成16进制字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
            {
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            }
            return sb.ToString().Trim().ToUpper();
        }

        /// <summary>
        /// ASCII转16进制
        /// </summary>
        /// <param name="ascii">ASCII</param>
        /// <returns></returns>
        public static string ASCIIToHex(string ascii)
        {
            char[] cs = ascii.ToCharArray();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < cs.Length; i++)
            {
                string hex = ((int)cs[i]).ToString("X");
                sb.AppendFormat("{0} ", hex);
            }
            return sb.ToString().TrimEnd(' ');
        }

        /// <summary>
        /// HEX to ASCII
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static string HexToASCII(string hex)
        {
            if (hex.Length % 2 == 0)
            {
                int iValue;
                byte[] bs;
                string sValue = string.Empty;
                string result = string.Empty;
                int length = hex.Length / 2;

                for (int i = 0; i < length; i++)
                {
                    iValue = Convert.ToInt32(hex.Substring(i * 2, 2), 16); // 16进制->10进制
                    bs = System.BitConverter.GetBytes(iValue); //int->byte[]
                    sValue = System.Text.Encoding.ASCII.GetString(bs, 0, 1);  //byte[]-> ASCII
                    result += char.Parse(sValue).ToString();
                }
                return result.PadLeft(length, '0');
            }
            return string.Empty;
        }

        /// <summary>
        /// ASCII To Float
        /// </summary>
        /// <param name="ascii"></param>
        /// <returns></returns>
        public static float ASCIIToFloat(string ascii)
        {
            if (ascii.Length == 8)
            {
                byte[] arr = new byte[4];
                for (int i = 0; i < 4; i++)
                {

                    arr[i] = Convert.ToByte(ascii.Substring((3 - i) * 2, 2), 16);
                }
                float f = BitConverter.ToSingle(arr, 0);
                return f;
            }
            return 0f;
        }

        /// <summary>
        /// Hex to Float
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static float HexToFloat(string hex)
        {
            string ascii = HexToASCII(hex);
            float f = ASCIIToFloat(ascii);
            return f;
        }
        #endregion


        public static string ByteToString(byte[] lstByte, int len)
        {
            string StringOut = "";
            for (int i = 0; i < len; i++)
            {
                StringOut = StringOut + String.Format("{0:X2} ", lstByte[i]);
            }
            return StringOut;
        }

        public static byte[] StringToByte(string InString)
        {
            InString = InString.Trim();
            List<Byte> lstBt = new List<byte>();
            for (int i = 0; i <= InString.Length - 1; i += 2)
            {
                if (((i+1) <= InString.Length - 1) && InString[i] == ' ')
                {
                    i++;
                }
                lstBt.Add(Convert.ToByte(("0x" + InString[i] + InString[i + 1]), 16));
            }
            return lstBt.ToArray();
        }
    }
}
