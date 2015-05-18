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
        TerminalDataDAL terDal = new TerminalDataDAL();

        public List<TerminalConfigEntity> GetAllPreTerminals()
        {
            lock (ConstValue.obj)
            {
                string SQL = @"SELECT DISTINCT Ter.TerminalID,Ter.TerminalName,Ter.[Address],Ter.Remark,PreUpperLimit,PreLowLimit,PreSlopeUpLimit,PreSlopeLowLimit,EnablePreAlarm,EnableSlopeAlarm FROM Terminal Ter, [PreTerConfig] Config 
                WHERE Ter.TerminalID=Config.TerminalID AND Ter.SyncState<>-1 AND Ter.TerminalType='" +(int)TerType.PreTer+"' ORDER BY TerminalName";
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
            string SQL = "SELECT COUNT(1) FROM PreTerConfig WHERE TerminalID='" + TerminalID + "' AND SyncState='" + SyncState + "'";

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
                string SQL_Config = @"INSERT INTO PreTerConfig(TerminalID,PreUpperLimit,PreLowLimit,PreSlopeUpLimit,PreSlopeLowLimit,EnablePreAlarm,EnableSlopeAlarm) VALUES(
                @TerminalID,@PreUpperLimit,@PreLowLimit,@PreSlopeUpLimit,@PreSlopeLowLimit,@EnablePreAlarm,@EnableSlopeAlarm)";
                SQLiteParameter[] parms = new SQLiteParameter[]{
                new SQLiteParameter("@TerminalID",DbType.Int32),
                new SQLiteParameter("@PreUpperLimit",DbType.Decimal),
                new SQLiteParameter("@PreLowLimit",DbType.Single),
                new SQLiteParameter("@PreSlopeUpLimit",DbType.Single),
                new SQLiteParameter("@PreSlopeLowLimit",DbType.Single),

                new SQLiteParameter("@EnablePreAlarm",DbType.Int32),
                new SQLiteParameter("@EnableSlopeAlarm",DbType.Int32),

            };
                parms[0].Value = entity.TerminalID;
                parms[1].Value = entity.PreUpLimit;
                parms[2].Value = entity.PreLowLimit;
                parms[3].Value = entity.PreSlopeUpLimit;
                parms[4].Value = entity.PreSlopeLowLimit;

                parms[5].Value = entity.EnablePreAlarm ? 1 : 0;
                parms[6].Value = entity.EnableSlopeAlarm ? 1 : 0;

                SQLiteHelper.ExecuteNonQuery(SQL_Config, parms);

                terDal.SaveTerInfo(entity.TerminalID, entity.TerminalName, entity.TerminalAddr, entity.Remark, TerType.PreTer, null, false);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Modify(TerminalConfigEntity entity)
        {
            if (DeletePreTer(entity.TerminalID.ToString()))
            {
                Insert(entity);
            }
            return true;
        }

        public bool DeletePreTer(string TerminalID)
        {
            DeletePreTerConfig(TerminalID);

            terDal.DeleteTer(TerType.PreTer, TerminalID);

            return true;
        }

        public bool DeletePreTerConfig(string TerminalID)
        {
            string SQL = "";
            if (IsExist(TerminalID, 1))
            {
                SQL = "DELETE FROM PreTerConfig WHERE SyncState=-1 AND TerminalID='" + TerminalID + "'";
            }
            else
            {
                SQL = "UPDATE PreTerConfig SET SyncState=-1 WHERE TerminalID='" + TerminalID + "'";
            }

            SQLiteHelper.ExecuteNonQuery(SQL, null);
            return true;
        }

        public bool ExistUniversalConfig(string terId)
        {
            string SQL = "SELECT COUNT(1) FROM UniversalTerWayConfig WHERE TerminalType='" + (int)TerType.UniversalTer + "' AND TerminalID='" + terId + "' AND SyncState<>-1";
            object obj_count = SQLiteHelper.ExecuteScalar(SQL, null);
            if (obj_count != null)
            {
                return Convert.ToInt32(obj_count) > 0 ? true : false;
            }
            return false;
        }
    }
}
