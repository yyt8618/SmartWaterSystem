using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace SmartWaterSystem
{
    public static class MD5Encrypt
    {

        /// <summary>
        /// MD5二次加密 会员相关加密，如密码(密码加密)
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>加密后结果</returns>
        public static string Encrypt(string str)
        {
            string one = MD5(str);
            return MD5(one + one.Substring(2, 8));
        }


        /// <summary>
        /// MD5加密 数据通信、接口对接时使用
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string MD5(string str)
        {
            byte[] b = Encoding.UTF8.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');

            return ret;
        }




        /// <summary>
        /// 指定字节流编码计算MD5哈希值，可解决不同系统中文编码差异的问题。
        /// </summary>
        /// <param name="source">要进行哈希的字符串</param>
        /// <param name="bytesEncoding">获取字符串字节流的编码，如果是中文，不同系统之间务必请使用相同编码</param>
        /// <returns>32位大写MD5哈希值</returns>
        public static string ComputeMD5(string source, Encoding bytesEncoding)
        {
            byte[] sourceBytes = bytesEncoding.GetBytes(source);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] hashedBytes = md5.ComputeHash(sourceBytes);

            StringBuilder buffer = new StringBuilder(hashedBytes.Length);
            foreach (byte item in hashedBytes)
            {
                buffer.AppendFormat("{0:X2}", item);
            }

            return buffer.ToString();
        }


        #region Md5加密方法

        public static string EncryptMD5(string instr)
        {
            string result;
            try
            {
                byte[] toByte = Encoding.Default.GetBytes(instr);
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                toByte = md5.ComputeHash(toByte);
                result = BitConverter.ToString(toByte).ToLower().Replace("-", "");
            }
            catch
            {
                result = "";
            }
            return result;
        }

        #endregion


        /// <summary>
        /// 语音支付 MD5
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>string</returns>
        public static string md5_Angelpay(string str)
        {
            if (str == null)
            {
                return null;
            }
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            System.Text.Encoding e = System.Text.Encoding.GetEncoding("UTF-8");
            byte[] fromData = e.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                string strHex = targetData[i].ToString("X2").ToLower();
                byte2String += strHex;
            }
            return byte2String;
        }


    }
}
