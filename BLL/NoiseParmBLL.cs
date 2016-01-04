using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;

namespace BLL
{
    public class NoiseParmBLL
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("NoiseParmBLL");
        NoiseParmDAL dal = new NoiseParmDAL();

        public string GetParm(string key)
        {
            try
            {
                return dal.GetParm(key);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetParm", ex);
                return "";
            }
        }

        public double GetParmD(string key)
        {
            try
            {
                 string strparm = dal.GetParm(key);
                 if (string.IsNullOrEmpty(strparm))
                 {
                     return 0;
                 }
                 else
                 {
                     return Convert.ToDouble(strparm);
                 }
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetParm", ex);
                return 0;
            }
        }

        public int GetParmI(string key)
        {
            try
            {
                string strparm = dal.GetParm(key);
                if (string.IsNullOrEmpty(strparm))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(strparm);
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetParm", ex);
                return 0;
            }
        }

        public bool SetParm(string key, string value)
        {
            try
            {
                dal.SetParm(key, value);
                return true;
            }
            catch (Exception ex)
            {
                logger.ErrorException("SetParm", ex);
                return false;
            }
        }
    }
}
