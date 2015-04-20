using System;
using System.Collections.Generic;
using Common;
using System.Data.SqlClient;
using Entity;
using System.Data;
using System.Data.SQLite;

namespace DAL
{
    public class TerminalConfigDAL
    {
        public List<TerminalConfigEntity> GetAllPreTerminals()
        {
            lock (ConstValue.obj)
            {
                string SQL = @"SELECT Ter.TerminalID,Ter.TerminalName,Ter.[Address],Ter.Remark,PreUpperLimit,PreLowLimit,PreSlopeUpLimit,PreSlopeLowLimit,EnablePreAlarm,EnableSlopeAlarm FROM Terminal Ter, [TerminalConfig] Config 
                WHERE Ter.TerminalID=Config.TerminalID AND Ter.SyncState<>-1 ORDER BY TerminalName";
                using (SQLiteDataReader reader = SQLiteHelper.ExecuteReader(SQL, null))
                {
                    List<TerminalConfigEntity> lstConfig = new List<TerminalConfigEntity>();
                    while (reader.Read())
                    {
                        TerminalConfigEntity entity = new TerminalConfigEntity();

                        entity.TerminalID = reader["TerminalID"] != DBNull.Value ? Convert.ToInt32(reader["TerminalID"]) : 0;
                        entity.TerminalName = reader["TerminalName"] != DBNull.Value ? reader["TerminalName"].ToString() : "";
                        entity.TerminalAddr = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : "";
                        entity.Remark = reader["Remark"] != DBNull.Value ? reader["Remark"].ToString() : "";

                        entity.EnablePreAlarm = reader["EnablePreAlarm"] != DBNull.Value ? (Convert.ToInt32(reader["EnablePreAlarm"]) == 1 ? true : false) : true;
                        entity.EnableSlopeAlarm = reader["EnableSlopeAlarm"] != DBNull.Value ? (Convert.ToInt32(reader["EnableSlopeAlarm"]) == 1 ? true : false) : true;

                        if (entity.EnablePreAlarm)
                        {
                            entity.PreLowLimit = reader["PreLowLimit"] != DBNull.Value ? Convert.ToDecimal(reader["PreLowLimit"]) : 0;
                            entity.PreUpLimit = reader["PreUpperLimit"] != DBNull.Value ? Convert.ToDecimal(reader["PreUpperLimit"]) : 0;
                        }
                        if (entity.EnableSlopeAlarm)
                        {
                            entity.PreSlopeLowLimit = reader["PreSlopeLowLimit"] != DBNull.Value ? Convert.ToDecimal(reader["PreSlopeLowLimit"]) : 0;
                            entity.PreSlopeUpLimit = reader["PreSlopeUpLimit"] != DBNull.Value ? Convert.ToDecimal(reader["PreSlopeUpLimit"]) : 0;
                        }

                        lstConfig.Add(entity);
                    }
                    return lstConfig;
                }
                return null;
            }
        }

        public bool IsExist(string TerminalID,int SyncState)
        {
            string SQL = "SELECT COUNT(1) FROM TerminalConfig WHERE TerminalID='" + TerminalID + "' AND SyncState='"+SyncState+"'";

            object obj_count = SQLiteHelper.ExecuteScalar(SQL);
            if (obj_count != null && obj_count != DBNull.Value)
            {
                return (Convert.ToInt32(obj_count) > 0) ? true : false;
            }
            return false;
        }

        public bool Insert(TerminalConfigEntity entity)
        {
            try
            {
                SqlCommand command_config = new SqlCommand();

                string SQL_Config = @"INSERT INTO TerminalConfig(TerminalID,PreUpperLimit,PreLowLimit,PreSlopeUpLimit,PreSlopeLowLimit,EnablePreAlarm,EnableSlopeAlarm) VALUES(
                @TerminalID,@PreUpperLimit,@PreLowLimit,@PreSlopeUpLimit,@PreSlopeLowLimit,@EnablePreAlarm,@EnableSlopeAlarm)";
                SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter("@TerminalID",SqlDbType.Int),
                new SqlParameter("@PreUpperLimit",SqlDbType.Decimal),
                new SqlParameter("@PreLowLimit",SqlDbType.Decimal),
                new SqlParameter("@PreSlopeUpLimit",SqlDbType.Decimal),
                new SqlParameter("@PreSlopeLowLimit",SqlDbType.Decimal),

                new SqlParameter("@EnablePreAlarm",SqlDbType.Int),
                new SqlParameter("@EnableSlopeAlarm",SqlDbType.Int),

            };
                parms[0].Value = entity.TerminalID;
                parms[1].Value = entity.PreUpLimit;
                parms[2].Value = entity.PreLowLimit;
                parms[3].Value = entity.PreSlopeUpLimit;
                parms[4].Value = entity.PreSlopeLowLimit;

                parms[5].Value = entity.EnablePreAlarm ? 1 : 0;
                parms[6].Value = entity.EnableSlopeAlarm ? 1 : 0;

                command_config.Connection = SQLHelper.Conn;
                command_config.CommandText = SQL_Config;
                command_config.CommandType = CommandType.Text;
                command_config.Parameters.AddRange(parms);

                command_config.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Modify(TerminalConfigEntity entity)
        {
            if (Delete(entity.TerminalID.ToString()))
            {
                Insert(entity);
            }
            return true;
        }

        public bool Delete(string TerminalID)
        {
            string SQL = "";
            if (IsExist(TerminalID, -1))
            {
                SQL = "DELETE FROM TerminalConfig WHERE SyncState=-1 AND TerminalID='" + TerminalID + "'";
            }
            else
            {
                SQL = "UPDATE TerminalConfig SET SyncState=-1 WHERE TerminalID='" + TerminalID + "'";
            }

            SQLHelper.ExecuteNonQuery(SQL, null);

            return true;
        }

    }
}
