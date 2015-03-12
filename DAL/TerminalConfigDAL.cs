using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using Entity;
using Common;

namespace DAL
{
    public class TerminalConfigDAL
    {
        public List<TerminalConfigEntity> GetAllTerminals()
        {
            lock (ConstValue.obj)
            {
                string SQL = "SELECT TerminalID,TerminalName,[Address],Remark,PreUpperLimit,PreLowLimit,PreSlopeUpLimit,PreSlopeLowLimit,EnablePreAlarm,EnableSlopeAlarm FROM [TerminalConfig] ORDER BY TerminalName";
                using (SqlDataReader reader = SQLHelper.ExecuteReader(SQL, null))
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

        public bool IsExist(string TerminalID)
        {
            string SQL = "SELECT COUNT(1) FROM TerminalConfig WHERE TerminalID='" + TerminalID + "'";

            object obj_count =SQLHelper.ExecuteScalar(SQL);
            if (obj_count != null && obj_count != DBNull.Value)
            {
                return (Convert.ToInt32(obj_count) > 0) ? true : false;
            }
            return false;
        }

        public TerminalConfigEntity GetTerminal(string TerminalID)
        {
            if(string.IsNullOrEmpty(TerminalID))
                return null;
            string SQL = "SELECT TerminalName,[Address],Remark,PreUpperLimit,PreLowLimit,PreSlopeUpLimit,PreSlopeLowLimit FROM [TerminalConfig] WHERE TerminalID='"+TerminalID+"'";
            using (SqlDataReader reader = SQLHelper.ExecuteReader(SQL, null))
            {
                TerminalConfigEntity entity = null;
                if (reader.Read())
                {
                    entity = new TerminalConfigEntity();

                    entity.TerminalID = Convert.ToInt32(TerminalID);
                    entity.TerminalName = reader["TerminalName"] != DBNull.Value ? reader["TerminalName"].ToString() : "";
                    entity.TerminalAddr = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : "";
                    entity.Remark = reader["Remark"] != DBNull.Value ? reader["Remark"].ToString() : "";
                    entity.PreLowLimit = reader["PreLowLimit"] != DBNull.Value ? Convert.ToDecimal(reader["PreLowLimit"]) : 0;

                    entity.PreUpLimit = reader["PreUpperLimit"] != DBNull.Value ? Convert.ToDecimal(reader["PreUpperLimit"]) : 0;
                    entity.PreSlopeLowLimit = reader["PreSlopeLowLimit"] != DBNull.Value ? Convert.ToDecimal(reader["PreSlopeLowLimit"]) : 0;
                    entity.PreSlopeUpLimit = reader["PreSlopeUpLimit"] != DBNull.Value ? Convert.ToDecimal(reader["PreSlopeUpLimit"]) : 0;
                }
                return entity;
            }
            return null;
        }

        /// <summary>
        /// 获取随机的EndPoint值,且不与EN_TrmlCollRelate->EN_CollPoint_ID重复
        /// </summary>
        private int GetRandCollectEndpoint()
        {
            Random r = new Random();
            int endpoint=r.Next(1, 1000000);
            if (IsExistEndPoint(endpoint))
            {
                return GetRandCollectEndpoint();
            }
            else
                return endpoint;
        }

        private bool IsExistEndPoint(int endpoint)
        {
            try
            {
                string SQL = "SELECT COUNT(1) FROM EN_TrmlCollRelate WHERE EN_CollPoint_ID='" + endpoint + "'";
                object obj = SQLHelper.ExecuteScalar(SQL, null);
                if (obj != null && obj != DBNull.Value)
                {
                    return Convert.ToInt32(obj) > 0 ? true : false;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsExistTrmlID(int TerminalId,SqlTransaction trans)
        {
            try
            {
                string SQL = "SELECT COUNT(1) FROM EN_TrmlCollRelate WHERE TrmlID='" + TerminalId + "'";
                SqlCommand command = new SqlCommand();
                command.Transaction = trans;
                command.CommandType = CommandType.Text;
                command.CommandText = SQL;
                command.Connection = SQLHelper.Conn;

                object obj = command.ExecuteScalar();
                if (obj != null && obj != DBNull.Value)
                {
                    return Convert.ToInt32(obj) > 0 ? true : false;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Hashtable GetTerID_PointID()
        {
            string SQL = "SELECT DISTINCT TrmlID,EN_CollPoint_ID FROM EN_TrmlCollRelate";
            using( SqlDataReader reader=SQLHelper.ExecuteReader(SQL,null))
            {
                Hashtable data = new Hashtable();
                while (reader.Read())
                {
                    data.Add(reader["TrmlID"].ToString(), reader["EN_CollPoint_ID"].ToString());
                }
                return data;
            }
            return null;
        }

        public bool Insert(TerminalConfigEntity entity)
        {
            //GetRandCollectEndpoint
            SqlTransaction trans = null;
            try
            {
                int EndPoint = GetRandCollectEndpoint();
                trans = SQLHelper.Conn.BeginTransaction();

                SqlCommand command_config = new SqlCommand();

                string SQL_Config = @"INSERT INTO TerminalConfig(TerminalID,TerminalName,[Address],Remark,PreUpperLimit,PreLowLimit,PreSlopeUpLimit,PreSlopeLowLimit,EnablePreAlarm,EnableSlopeAlarm) VALUES(
                @TerminalID,@TerminalName,@Address,@Remark,@PreUpperLimit,@PreLowLimit,@PreSlopeUpLimit,@PreSlopeLowLimit,@EnablePreAlarm,@EnableSlopeAlarm)";
                SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter("@TerminalID",SqlDbType.Int),
                new SqlParameter("@TerminalName",SqlDbType.NVarChar),
                new SqlParameter("@Address",SqlDbType.NVarChar),
                new SqlParameter("@Remark",SqlDbType.NVarChar),
                new SqlParameter("@PreUpperLimit",SqlDbType.Decimal),

                new SqlParameter("@PreLowLimit",SqlDbType.Decimal),
                new SqlParameter("@PreSlopeUpLimit",SqlDbType.Decimal),
                new SqlParameter("@PreSlopeLowLimit",SqlDbType.Decimal),
                new SqlParameter("@EnablePreAlarm",SqlDbType.Int),
                new SqlParameter("@EnableSlopeAlarm",SqlDbType.Int),

            };
                parms[0].Value = entity.TerminalID;
                parms[1].Value = entity.TerminalName;
                parms[2].Value = entity.TerminalAddr;
                parms[3].Value = entity.Remark;
                parms[4].Value = entity.PreUpLimit;

                parms[5].Value = entity.PreLowLimit;
                parms[6].Value = entity.PreSlopeUpLimit;
                parms[7].Value = entity.PreSlopeLowLimit;
                parms[8].Value = entity.EnablePreAlarm ? 1 : 0;
                parms[9].Value = entity.EnableSlopeAlarm ? 1 : 0;

                command_config.Connection = SQLHelper.Conn;
                command_config.CommandText = SQL_Config;
                command_config.CommandType = CommandType.Text;
                command_config.Parameters.AddRange(parms);
                command_config.Transaction = trans;

                command_config.ExecuteNonQuery();

                if (!IsExistTrmlID(entity.TerminalID,trans))
                {
                    string SQL_Relate = string.Format(@"INSERT INTO EN_TrmlCollRelate(TrmlID,Config_Value,Config_Name,MT_CollDeviceType_ID,En_CollPoint_ID) 
                        VALUES('{0}',1,'',1,'{1}')", entity.TerminalID, EndPoint);
                    SqlCommand command_relate = new SqlCommand();
                    command_relate.Connection = SQLHelper.Conn;
                    command_relate.CommandType = CommandType.Text;
                    command_relate.Transaction = trans;
                    command_relate.CommandText = SQL_Relate;

                    command_relate.ExecuteNonQuery();
                }

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

        public bool Modify(TerminalConfigEntity entity)
        {
            string SQL = @"UPDATE TerminalConfig SET TerminalName=@TerminalName,
                        [Address]=@Address,Remark=@Remark,EnablePreAlarm=@EnablePreAlarm,EnableSlopeAlarm=@EnableSlopeAlarm,
                        PreUpperLimit=@PreUpperLimit,PreLowLimit=@PreLowLimit,
                        PreSlopeUpLimit=@PreSlopeUpLimit,PreSlopeLowLimit=@PreSlopeLowLimit WHERE TerminalID=@TerminalID";
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter("@TerminalID",SqlDbType.Int),
                new SqlParameter("@TerminalName",SqlDbType.NVarChar),
                new SqlParameter("@Address",SqlDbType.NVarChar),
                new SqlParameter("@Remark",SqlDbType.NVarChar),
                new SqlParameter("@EnablePreAlarm",SqlDbType.Int),

                new SqlParameter("@EnableSlopeAlarm",SqlDbType.Int),
                new SqlParameter("@PreUpperLimit",SqlDbType.Decimal),
                new SqlParameter("@PreLowLimit",SqlDbType.Decimal),
                new SqlParameter("@PreSlopeUpLimit",SqlDbType.Decimal),
                new SqlParameter("@PreSlopeLowLimit",SqlDbType.Decimal)
            };

            parms[0].Value = entity.TerminalID;
            parms[1].Value = entity.TerminalName;
            parms[2].Value = entity.TerminalAddr;
            parms[3].Value = entity.Remark;
            parms[4].Value = entity.EnablePreAlarm ? 1 : 0;

            parms[5].Value = entity.EnableSlopeAlarm ? 1 : 0;
            parms[6].Value = entity.PreUpLimit;
            parms[7].Value = entity.PreLowLimit;
            parms[8].Value = entity.PreSlopeUpLimit;
            parms[9].Value = entity.PreSlopeLowLimit;

            SQLHelper.ExecuteNonQuery(SQL, parms);
            return true;
        }

        public bool Delete(string TerminalID)
        {
            string SQL = "DELETE FROM TerminalConfig WHERE TerminalID='" + TerminalID + "'";

            SQLHelper.ExecuteNonQuery(SQL, null);

            return true;
        }

        public List<string> GetAddresses()
        {
            string SQL = "SELECT DISTINCT Address FROM TerminalConfig";

            using (SqlDataReader reader = SQLHelper.ExecuteReader(SQL, null))
            {
                List<string> lstAddress = new List<string>();
                string addr = "";
                while (reader.Read())
                {
                    addr = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : "";
                    lstAddress.Add(addr);
                }
                return lstAddress;
            }
            return null;
        }

    }
}
