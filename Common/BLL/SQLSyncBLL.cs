using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoiseAnalysisSystem.BLL
{
    public class SQLSyncBLL
    {
        //同步终端信息
        public bool SyncTerInfo()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //同步模拟类型信息
        public bool SyncSimulateType()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //同步RS485类型信息
        public bool SyncRS485Type()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //同步脉冲类型信息
        public bool SyncPluseType()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
