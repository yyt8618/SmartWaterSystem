using System;
using Entity;
using System.Data.SQLite;
using Common;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public class OLWQConfigDAL
    {
        public bool Insert(OLWQConfigEntity configData)
        {
            SqlTransaction trans = null;
            try
            {
                if (configData == null)
                    return true;

                trans = SQLHelper.GetTransaction();
                string SQL_Del = "DELETE FROM OLWQConfig WHERE TerminalID='" + configData.TerId + "'";
                SqlCommand cmd_Del = new SqlCommand();
                cmd_Del.CommandText = SQL_Del;
                cmd_Del.Connection = SQLHelper.Conn;
                cmd_Del.Transaction = trans;
                cmd_Del.ExecuteNonQuery();

                string SQL_Insert = @"INSERT INTO OLWQConfig(TerminalID,EnableTurbidityAlarm,EnableResidualClAlarm,EnablePHAlarm,
            EnableConductivityAlarm,TurbidityUpLimit,TurbidityLowLimit,ResidualClUpLimit,ResidualClLowLimit,PHUpLimit,PHLowLimit,
            ConductivityUpLimit,ConductivityLowLimit) VALUES(@terid,@enableTurbidity,@enableResidualCl,@enablePH,@enableConductivity,
            @turbidityUpLimit,@turbidityLowLimit,@residualUpLimit,@residualLowLimit,@phUpLimit,@phLowLimit,@conductivityUpLimit,@conductivityLowLimit)";
                SqlParameter[] parms = new SqlParameter[]{
                    new SqlParameter("@terid",DbType.Int32),
                    new SqlParameter("@enableTurbidity",DbType.Int32),
                    new SqlParameter("@enableResidualCl", DbType.Int32),
                    new SqlParameter("@enablePH",DbType.Int32),
                    new SqlParameter("@enableConductivity",DbType.Int32),
                    
                    new SqlParameter("@turbidityUpLimit", DbType.Decimal),
                    new SqlParameter("@turbidityLowLimit",DbType.Decimal),
                    new SqlParameter("@residualUpLimit",DbType.Decimal),
                    new SqlParameter("@residualLowLimit",DbType.Decimal),
                    new SqlParameter("@phUpLimit",DbType.Decimal),

                    new SqlParameter("@phLowLimit",DbType.Decimal),
                    new SqlParameter("@conductivityUpLimit",DbType.Decimal),
                    new SqlParameter("@conductivityLowLimit",DbType.Decimal)
                };
                parms[0].Value =configData.TerId; 
                parms[1].Value =configData.enableTurbidityAlarm;
                parms[2].Value =configData.enableResidualClAlarm;
                parms[3].Value =configData.enablePHAlarm;
                parms[4].Value =configData.enableConductivityAlarm;
                parms[5].Value =configData.TurbidityUpLimit;
                parms[6].Value =configData.TurbidityLowLimit;
                parms[7].Value =configData.ResidualClUpLimit;
                parms[8].Value =configData.ResidualClLowLimit;
                parms[9].Value =configData.PHUpLimit;
                parms[10].Value =configData.PHLowLimit;
                parms[11].Value =configData.ConductivityUpLimit;
                parms[12].Value =configData.ConductivityLowLimit;
                SqlCommand cmd_insert = new SqlCommand();
                cmd_insert.CommandText = SQL_Insert;
                cmd_insert.Connection = SQLHelper.Conn;
                cmd_insert.Transaction = trans;
                cmd_insert.Parameters.AddRange(parms);
                cmd_insert.ExecuteNonQuery();

                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();
                throw ex;
            }
        }

        public void Delete(string TerId)
        {
            string SQL_Del = "DELETE FROM OLWQConfig WHERE TerminalID='" + TerId + "'";
            SQLHelper.ExecuteNonQuery(SQL_Del, null);
        }

        public List<OLWQConfigEntity> Select(string where)
        {
            string SQL = @"SELECT TerminalID,EnableTurbidityAlarm,EnableResidualClAlarm,EnablePHAlarm,
            EnableConductivityAlarm,TurbidityUpLimit,TurbidityLowLimit,ResidualClUpLimit,ResidualClLowLimit,PHUpLimit,PHLowLimit,
            ConductivityUpLimit,ConductivityLowLimit FROM OLWQConfig ";
            if (!string.IsNullOrEmpty(where))
                SQL = SQL +"WHERE "+ where;
            using (SqlDataReader reader = SQLHelper.ExecuteReader(SQL, null))
            {
                List<OLWQConfigEntity> lst_Config = new List<OLWQConfigEntity>();
                while (reader.Read())
                {
                    OLWQConfigEntity config = new OLWQConfigEntity();
                    config.TerId = reader["TerminalID"] != DBNull.Value ? reader["TerminalID"].ToString().Trim() : "";
                    config.enableTurbidityAlarm = reader["EnableTurbidityAlarm"] != DBNull.Value ? (Convert.ToInt32(reader["EnableTurbidityAlarm"]) == 1 ? true : false) : false;
                    config.enableResidualClAlarm = reader["EnableResidualClAlarm"] != DBNull.Value ? (Convert.ToInt32(reader["EnableResidualClAlarm"]) == 1 ? true : false) : false;
                    config.enablePHAlarm = reader["EnablePHAlarm"] != DBNull.Value ? (Convert.ToInt32(reader["EnablePHAlarm"]) == 1 ? true : false) : false;
                    config.enableConductivityAlarm = reader["EnableConductivityAlarm"] != DBNull.Value ? (Convert.ToInt32(reader["EnableConductivityAlarm"]) == 1 ? true : false) : false;
                    config.TurbidityUpLimit = reader["TurbidityUpLimit"] != DBNull.Value ? Convert.ToSingle(reader["TurbidityUpLimit"]) : 0;
                    config.TurbidityLowLimit = reader["TurbidityLowLimit"] != DBNull.Value ? Convert.ToSingle(reader["TurbidityLowLimit"]) : 0;
                    config.ResidualClUpLimit = reader["ResidualClUpLimit"] != DBNull.Value ? Convert.ToSingle(reader["ResidualClUpLimit"]) : 0;
                    config.ResidualClLowLimit = reader["ResidualClLowLimit"] != DBNull.Value ? Convert.ToSingle(reader["ResidualClLowLimit"]) : 0;
                    config.PHUpLimit = reader["PHUpLimit"] != DBNull.Value ? Convert.ToSingle(reader["PHUpLimit"]) : 0;
                    config.PHLowLimit = reader["PHLowLimit"] != DBNull.Value ? Convert.ToSingle(reader["PHLowLimit"]) : 0;
                    config.ConductivityUpLimit = reader["ConductivityUpLimit"] != DBNull.Value ? Convert.ToSingle(reader["ConductivityUpLimit"]) : 0;
                    config.ConductivityLowLimit = reader["ConductivityLowLimit"] != DBNull.Value ? Convert.ToSingle(reader["ConductivityLowLimit"]) : 0;
                    
                    lst_Config.Add(config);
                }
                return lst_Config;
            }
        }


    }
}
