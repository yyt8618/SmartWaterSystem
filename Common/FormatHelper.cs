using System;
using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// 字符串格式化
    /// </summary>
    public static class FormatHelper
    {
        #region 格式化字符串
        /// <summary>
        /// 将字符数组装换成字符串
        /// </summary>
        /// <param name="array">数组</param>
        /// <param name="startIndex">开始位置</param>
        /// <param name="count">长度</param>
        /// <returns>拼接字符串</returns>
        public static string GetStringByArray(string[] array, int startIndex, int count)
        {
            int index = array.Length;

            if (startIndex < 0 || startIndex > index)
            {
                return string.Empty;
            }

            int endIndex = startIndex + count;
            if (endIndex < 0 || endIndex > index)
            {
                return string.Empty;
            }

            if (startIndex > endIndex)
            {
                return string.Empty;
            }

            string result = string.Empty;
            result = string.Join(" ", array, startIndex, count);
            return result;
        }

        /// <summary>
        /// 将BCDto日期
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeByBCD(string now)
        {
            string[] tempNow = now.TrimEnd(' ').Split(' ');

            string dt = string.Format("{6}{0}-{1}-{2} {3}:{4}:{5}", tempNow[0], tempNow[1], tempNow[2], tempNow[3], tempNow[4], tempNow[5], DateTime.Now.Year.ToString().Substring(0, 2));

            DateTime result;

            bool done = DateTime.TryParse(dt, out result);
            if (done)
            {
                return result;
            }
            return DateTime.Now;
        }

        /// <summary>
        /// 返回格式化的日期时间字符串
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public static string GetBCDDateTime(DateTime current)
        {
            string result = current.ToString("yy MM dd HH mm ss");
            return result;
        }

        /// <summary>
        /// 将字符串转换成对应的数值/100
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal GetValueByString(string value)
        {
            string tempValue = value.Replace(" ", "");
            return decimal.Divide(decimal.Parse(tempValue), 100);
        }

        /// <summary>
        /// 返回对应的整型字符串
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string BinaryToIntString(int i)
        {
            string result = string.Empty;
            if (i < 0 || i > 999999)
            {
                return "00 00 00";
            }
            else if (i < 100)
            {
                return string.Format("00 00 {0}", i.ToString("D2"));
            }
            else if (i >= 100 && i < 1000)
            {
                return string.Format("00 0{0} {1}", i.ToString().Substring(0, 1), i.ToString().Substring(1, 2));
            }
            else if (i >= 1000 && i < 10000)
            {
                return string.Format("00 {0} {1}", i.ToString().Substring(0, 2), i.ToString().Substring(2, 2));
            }
            else if (i >= 10000 && i < 100000)
            {
                return string.Format("0{0} {1} {2}", i.ToString().Substring(0, 1), i.ToString().Substring(1, 2), i.ToString().Substring(3, 2));
            }
            else if (i >= 100000 && i < 1000000)
            {
                return string.Format("{0} {1} {2}", i.ToString().Substring(0, 2), i.ToString().Substring(2, 2), i.ToString().Substring(4, 2));
            }
            return "00 00";
        }

        /// <summary>
        /// 整形格式化为两位小数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string IntFormatString(int value)
        {
            return value.ToString("D2");
        }


        /// <summary>
        /// 检查数据开头是否是68或者7E协议头，如果不是去掉，直到有正确协议头
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static List<byte> CheckHead(List<byte> datas)
        {
            if (datas.Count > 0)
            {
                if (datas[0] == PackageDefine.BeginByte || datas[0] == PackageDefine.BeginByte651[0])
                    return datas;
                else
                {
                    datas.RemoveAt(0);
                    return CheckHead(datas);
                }
            }
            else
                return datas;
        }
        #endregion
    }
}
