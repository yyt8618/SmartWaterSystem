using System;
using System.Data.SQLite;
using System.Data;

namespace NoiseAnalysisSystem
{
    public class DBVersion
    {
        private NLog.Logger logger = NLog.LogManager.GetLogger("Version");
        /// <summary>
        /// 获取版本
        /// </summary>
        public string GetVersion(string versionType)
        {
            try
            {
                string strSQL = "select VersionValue from tmsVersion where VersionType = @VersionType";
                SQLiteParameter[] parm = new SQLiteParameter[] { new SQLiteParameter("@VersionType", DbType.String, 10) };
                parm[0].Value = versionType;

                object objValue = SQLiteHelper.ExecuteScalar(strSQL, parm);
                return Convert.ToString(objValue);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetVersion", ex);
                return "";
            }
        }

        /// <summary>
        /// 更新版本
        /// </summary>
        public bool UpdateVersion(string versionType, string versionValue)
        {
            try
            {
                string strSQL = "update tmsVersion set VersionValue = @VersionValue where VersionType = @VersionType";
                SQLiteParameter[] parm = new SQLiteParameter[] 
            { 
                new SQLiteParameter ("@VersionValue", DbType.String, 30), 
                new SQLiteParameter ("@VersionType", DbType.String, 10) 
            };
                parm[0].Value = versionValue;
                parm[1].Value = versionType;

                SQLiteHelper.ExecuteNonQuery(strSQL, parm);

                return true;
            }
            catch (Exception ex)
            {
                logger.ErrorException("UpdateVersion", ex);
                return false;
            }
        }
    }
}
