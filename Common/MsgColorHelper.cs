using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SmartWaterSystem
{
    public class MsgColorHelper
    {
        /// <summary>
        /// 获取配置的颜色
        /// </summary>
        /// <returns></returns>
        public Hashtable GetColorConfig(string Filepath)
        {
            Hashtable ht = new Hashtable();
            try
            {
                if (File.Exists(Filepath))
                {
                    using (StreamReader reader = new StreamReader(Filepath, Encoding.UTF8))
                    {
                        do
                        {
                            string strrow = reader.ReadLine();
                            if (!string.IsNullOrEmpty(strrow))
                            {
                                string[] strcols = strrow.Split('\t');
                                if (strcols != null && strcols.Length == 2)
                                {
                                    if (System.Text.RegularExpressions.Regex.IsMatch(strcols[0], @"^\d{1,4}$"))
                                    {
                                        ht.Add(Convert.ToInt32(strcols[0]), strcols[1]);
                                    }
                                }
                            }
                        } while (!reader.EndOfStream);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ht;
        }
    }
}
