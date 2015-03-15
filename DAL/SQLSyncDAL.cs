using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Entity;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace DAL
{
    public class SQLSyncDAL
    {
        public DateTime GetSQLModifyTime(string TableName)
        {
            string SQL = "SELECT MAX(ModifyTime) FROM " + TableName;
            object obj = SQLHelper.ExecuteScalar(SQL, null);
            if ((obj == null) || (obj == DBNull.Value))
            {
                return ConstValue.MinDateTime;
            }
            else
            {
                return Convert.ToDateTime(obj);
            }
        }

        public DateTime GetSQLiteModifyTime(string TableName)
        {
            string SQL = "SELECT MAX(ModifyTime) FROM " + TableName;
            object obj = SQLiteHelper.ExecuteScalar(SQL, null);
            if ((obj == null) || (obj == DBNull.Value))
            {
                return ConstValue.MinDateTime;
            }
            else
            {
                return Convert.ToDateTime(obj);
            }
        }

        //同步终端信息
        public void SyncTerInfo()
        {
            //同步Terminal Table
            DateTime date_sql = GetSQLModifyTime("Terminal");
            DateTime date_sqlite = GetSQLiteModifyTime("Terminal");
            if (date_sql > date_sqlite)  //sql比sqlite新
            {
                UpdateSQLite_Terminal(date_sql);
            }
            else if (date_sqlite > date_sql)
            {
                UpdateSQL_Terminal(date_sqlite);
            }

            //同步 PreTerConfig Table
            date_sql = GetSQLModifyTime("PreTerConfig");
            date_sqlite = GetSQLiteModifyTime("PreTerConfig");
            if (date_sql > date_sqlite)  //sql比sqlite新
            {
                UpdateSQLite_PreTerConfig(date_sql);
            }
            else if (date_sqlite > date_sql)
            {
                UpdateSQL_PreTerConfig(date_sqlite);
            }

            //同步UniversalTerConfig Table  待用
            //date_sql = GetSQLModifyTime("UniversalTerConfig");
            //date_sqlite = GetSQLiteModifyTime("UniversalTerConfig");
            //if (date_sql > date_sqlite)  //sql比sqlite新
            //{
            //    UpdateSQLite_UniversalTerConfig(date_sql);
            //}
            //else if (date_sqlite > date_sql)
            //{
            //    UpdateSQL_UniversalTerConfig(date_sqlite);
            //}

        }
        #region 同步Terminal Table
        public void UpdateSQL_Terminal(DateTime mindate)
        {
            string str_SQLite = "SELECT TerminalID,TerminalName,Address,Remark FROM Terminal WHERE ModifyTime>@date";
            SQLiteParameter param = new SQLiteParameter("@date", DbType.DateTime);
            param.Value = mindate;
            DataTable dt_sqlite = SQLiteHelper.ExecuteDataTable(str_SQLite, param);
            if ((dt_sqlite != null) && dt_sqlite.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_sqlite.Rows)
                {
                    string str_SQLExist = "SELECT ID FROM Terminal WHERE TerminalID = '" + dr["TerminalID"].ToString() + "' AND TerminaleType='"+dr["TerminaleType"].ToString()+"'";
                    object obj_exist = SQLHelper.ExecuteScalar(str_SQLExist, null);
                    if (obj_exist != null && obj_exist != DBNull.Value)  //modify
                    {
                        string str_Update = string.Format("UPDATE Terminal SET TerminalName='{0}',Address='{1}',Remark='{2}' WHERE ID = '{3}'",
                            dr["TerminalName"].ToString(), dr["Address"].ToString(), dr["Remark"].ToString(),obj_exist.ToString());
                        SQLHelper.ExecuteNonQuery(str_Update, null);

                    }
                    else  //insert
                    {
                        string str_Insert = string.Format("INSERT INTO Terminal(TerminalName,Address,Remark) VALUES('{0}','{1}','{2}')",
                            dr["TerminalID"].ToString(),dr["TerminalName"].ToString(), dr["Address"].ToString(), dr["Remark"].ToString());
                        SQLHelper.ExecuteNonQuery(str_Insert, null);
                    }
                }
            }
        }
        public void UpdateSQLite_Terminal(DateTime mindate)
        {
            string str_SQLite = "SELECT TerminalID,TerminalName,Address,Remark FROM Terminal WHERE ModifyTime>@date";
            SqlParameter param = new SqlParameter("@date", DbType.DateTime);
            param.Value = mindate;
            DataTable dt_sql = SQLHelper.ExecuteDataTable(str_SQLite, param);
            if ((dt_sql != null) && dt_sql.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_sql.Rows)
                {
                    string str_SQLiteExist = "SELECT ID FROM Terminal WHERE TerminalID = '" + dr["TerminalID"].ToString() + "' AND TerminaleType='" + dr["TerminaleType"].ToString() + "'";
                    object obj_exist = SQLiteHelper.ExecuteScalar(str_SQLiteExist, null);
                    if (obj_exist != null && obj_exist != DBNull.Value)  //modify
                    {
                        string str_Update = string.Format("UPDATE Terminal SET TerminalName='{0}',Address='{1}',Remark='{2}' WHERE ID = '{3}'",
                            dr["TerminalName"].ToString(), dr["Address"].ToString(), dr["Remark"].ToString(), obj_exist.ToString());
                        SQLiteHelper.ExecuteNonQuery(str_Update, null);

                    }
                    else  //insert
                    {
                        string str_Insert = string.Format("INSERT INTO Terminal(TerminalName,Address,Remark) VALUES('{0}','{1}','{2}')",
                            dr["TerminalID"].ToString(),dr["TerminalName"].ToString(), dr["Address"].ToString(), dr["Remark"].ToString(), obj_exist.ToString());
                        SQLiteHelper.ExecuteNonQuery(str_Insert, null);
                    }
                }
            }
        }
        #endregion

        #region 同步PreTerConfig Table
        public void UpdateSQL_PreTerConfig(DateTime mindate)
        {
            string str_SQLite = "SELECT TerminalID,EnablePreAlarm,EnableSlopeAlarm,PreUpperLimit,PreLowLimit,PreSlopeUpLimit,PreSlopeLowLimit FROM PreTerConfig WHERE ModifyTime>@date";
            SQLiteParameter param = new SQLiteParameter("@date", DbType.DateTime);
            param.Value = mindate;
            DataTable dt_sql = SQLiteHelper.ExecuteDataTable(str_SQLite, param);
            if ((dt_sql != null) && dt_sql.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_sql.Rows)
                {
                    string str_SQLiteExist = "SELECT ID FROM PreTerConfig WHERE TerminalID = '" + dr["TerminalID"].ToString() + "'";
                    object obj_exist = SQLiteHelper.ExecuteScalar(str_SQLiteExist, null);
                    if (obj_exist != null && obj_exist != DBNull.Value)  //modify
                    {
                        string str_Update = string.Format("UPDATE PreTerConfig SET EnablePreAlarm='{0}',EnableSlopeAlarm='{1}',PreUpperLimit='{2}',PreLowLimit='{3}',PreSlopeUpLimit='{4}',PreSlopeLowLimit='{5}' WHERE ID = '{6}'",
                            dr["EnablePreAlarm"].ToString(), dr["EnableSlopeAlarm"].ToString(), dr["PreUpperLimit"].ToString(), dr["PreLowLimit"].ToString(), dr["PreSlopeUpLimit"].ToString(), dr["PreSlopeLowLimit"].ToString(), obj_exist.ToString());
                        SQLHelper.ExecuteNonQuery(str_Update, null);

                    }
                    else  //insert
                    {
                        string str_Insert = string.Format("INSERT INTO PreTerConfig(EnablePreAlarm,EnableSlopeAlarm,PreUpperLimit,PreLowLimit,PreSlopeUpLimit,PreSlopeLowLimit) VALUES('{0}','{1}','{2}','{3}','{4}','{5}')",
                            dr["TerminalID"].ToString(),dr["EnablePreAlarm"].ToString(), dr["EnableSlopeAlarm"].ToString(), dr["PreUpperLimit"].ToString(), dr["PreLowLimit"].ToString(), dr["PreSlopeUpLimit"].ToString(), dr["PreSlopeLowLimit"].ToString());
                        SQLHelper.ExecuteNonQuery(str_Insert, null);
                    }
                }
            }
        }
        public void UpdateSQLite_PreTerConfig(DateTime mindate)
        {
            string str_SQLite = "SELECT TerminalID,EnablePreAlarm,EnableSlopeAlarm,PreUpperLimit,PreLowLimit,PreSlopeUpLimit,PreSlopeLowLimit FROM PreTerConfig WHERE ModifyTime>@date";
            SqlParameter param = new SqlParameter("@date", DbType.DateTime);
            param.Value = mindate;
            DataTable dt_sql = SQLHelper.ExecuteDataTable(str_SQLite, param);
            if ((dt_sql != null) && dt_sql.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_sql.Rows)
                {
                    string str_SQLiteExist = "SELECT ID FROM PreTerConfig WHERE TerminalID = '" + dr["TerminalID"].ToString() + "'";
                    object obj_exist = SQLHelper.ExecuteScalar(str_SQLiteExist, null);
                    if (obj_exist != null && obj_exist != DBNull.Value)  //modify
                    {
                        string str_Update = string.Format("UPDATE PreTerConfig SET EnablePreAlarm='{0}',EnableSlopeAlarm='{1}',PreUpperLimit='{2}',PreLowLimit='{3}',PreSlopeUpLimit='{4}',PreSlopeLowLimit='{5}' WHERE ID = '{6}'",
                            dr["EnablePreAlarm"].ToString(), dr["EnableSlopeAlarm"].ToString(), dr["PreUpperLimit"].ToString(), dr["PreLowLimit"].ToString(), dr["PreSlopeUpLimit"].ToString(), dr["PreSlopeLowLimit"].ToString(), obj_exist.ToString());
                        SQLiteHelper.ExecuteNonQuery(str_Update, null);

                    }
                    else  //insert
                    {
                        string str_Insert = string.Format("INSERT INTO PreTerConfig(EnablePreAlarm,EnableSlopeAlarm,PreUpperLimit,PreLowLimit,PreSlopeUpLimit,PreSlopeLowLimit) VALUES('{0}','{1}','{2}','{3}','{4}','{5}')",
                            dr["TerminalID"].ToString(),dr["EnablePreAlarm"].ToString(), dr["EnableSlopeAlarm"].ToString(), dr["PreUpperLimit"].ToString(), dr["PreLowLimit"].ToString(), dr["PreSlopeUpLimit"].ToString(), dr["PreSlopeLowLimit"].ToString());
                        SQLiteHelper.ExecuteNonQuery(str_Insert, null);
                    }
                }
            }
        }
        #endregion

        #region 同步UniversalTerConfig Table
        public void UpdateSQL_UniversalTerConfig(DateTime mindate)
        {
            string str_SQLite = "SELECT TerminalID FROM UniversalTerConfig WHERE ModifyTime>@date";
            SQLiteParameter param = new SQLiteParameter("@date", DbType.DateTime);
            param.Value = mindate;
            DataTable dt_sql = SQLiteHelper.ExecuteDataTable(str_SQLite, param);
            if ((dt_sql != null) && dt_sql.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_sql.Rows)
                {
                    string str_SQLiteExist = "SELECT ID FROM UniversalTerConfig WHERE TerminalID = '" + dr["TerminalID"].ToString() + "'";
                    object obj_exist = SQLiteHelper.ExecuteScalar(str_SQLiteExist, null);
                    if (obj_exist != null && obj_exist != DBNull.Value)  //modify
                    {
                        string str_Update = string.Format("UPDATE UniversalTerConfig SET EnablePreAlarm='{0}',EnableSlopeAlarm='{1}',PreUpperLimit='{2}',PreLowLimit='{3}',PreSlopeUpLimit='{4}',PreSlopeLowLimit='{5}' WHERE ID = '{6}'",
                            dr["EnablePreAlarm"].ToString(), dr["EnableSlopeAlarm"].ToString(), dr["PreUpperLimit"].ToString(), dr["PreLowLimit"].ToString(), dr["PreSlopeUpLimit"].ToString(), dr["PreSlopeLowLimit"].ToString(), obj_exist.ToString());
                        SQLHelper.ExecuteNonQuery(str_Update, null);

                    }
                    else  //insert
                    {
                        string str_Insert = string.Format("INSERT INTO UniversalTerConfig(TerminalID,EnablePreAlarm,EnableSlopeAlarm,PreUpperLimit,PreLowLimit,PreSlopeUpLimit,PreSlopeLowLimit) VALUES('{0}','{1}','{2}','{3}','{4}','{5}')",
                            dr["TerminalID"].ToString(),dr["EnablePreAlarm"].ToString(), dr["EnableSlopeAlarm"].ToString(), dr["PreUpperLimit"].ToString(), dr["PreLowLimit"].ToString(), dr["PreSlopeUpLimit"].ToString(), dr["PreSlopeLowLimit"].ToString());
                        SQLHelper.ExecuteNonQuery(str_Insert, null);
                    }
                }
            }
        }
        public void UpdateSQLite_UniversalTerConfig(DateTime mindate)
        {
            string str_SQLite = "SELECT TerminalID FROM UniversalTerConfig WHERE ModifyTime>@date";
            SqlParameter param = new SqlParameter("@date", DbType.DateTime);
            param.Value = mindate;
            DataTable dt_sql = SQLHelper.ExecuteDataTable(str_SQLite, param);
            if ((dt_sql != null) && dt_sql.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_sql.Rows)
                {
                    string str_SQLiteExist = "SELECT ID FROM UniversalTerConfig WHERE TerminalID = '" + dr["TerminalID"].ToString() + "'";
                    object obj_exist = SQLiteHelper.ExecuteScalar(str_SQLiteExist, null);
                    if (obj_exist != null && obj_exist != DBNull.Value)  //modify
                    {
                        string str_Update = string.Format("UPDATE UniversalTerConfig SET EnablePreAlarm='{0}',EnableSlopeAlarm='{1}',PreUpperLimit='{2}',PreLowLimit='{3}',PreSlopeUpLimit='{4}',PreSlopeLowLimit='{5}' WHERE ID = '{6}'",
                            dr["EnablePreAlarm"].ToString(), dr["EnableSlopeAlarm"].ToString(), dr["PreUpperLimit"].ToString(), dr["PreLowLimit"].ToString(), dr["PreSlopeUpLimit"].ToString(), dr["PreSlopeLowLimit"].ToString(), obj_exist.ToString());
                        SQLiteHelper.ExecuteNonQuery(str_Update, null);

                    }
                    else  //insert
                    {
                        string str_Insert = string.Format("INSERT INTO UniversalTerConfig(TerminalID,EnablePreAlarm,EnableSlopeAlarm,PreUpperLimit,PreLowLimit,PreSlopeUpLimit,PreSlopeLowLimit) VALUES('{0}','{1}','{2}','{3}','{4}','{5}')",
                            dr["TerminalID"].ToString(),dr["EnablePreAlarm"].ToString(), dr["EnableSlopeAlarm"].ToString(), dr["PreUpperLimit"].ToString(), dr["PreLowLimit"].ToString(), dr["PreSlopeUpLimit"].ToString(), dr["PreSlopeLowLimit"].ToString());
                        SQLiteHelper.ExecuteNonQuery(str_Insert, null);
                    }
                }
            }
        }
        #endregion

        #region 同步UniversalTerWayConfig Table
        public void UpdateSQL_UniversalTerWayConfig(DateTime mindate)
        {
            string str_SQLite = "SELECT TerminalID,PointID FROM UniversalTerWayConfig WHERE ModifyTime>@date";
            SQLiteParameter param = new SQLiteParameter("@date", DbType.DateTime);
            param.Value = mindate;
            DataTable dt_sql = SQLiteHelper.ExecuteDataTable(str_SQLite, param);
            if ((dt_sql != null) && dt_sql.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_sql.Rows)
                {
                    string str_SQLiteExist = "SELECT ID FROM UniversalTerWayConfig WHERE TerminalID = '" + dr["TerminalID"].ToString() + "'";
                    object obj_exist = SQLiteHelper.ExecuteScalar(str_SQLiteExist, null);
                    if (obj_exist != null && obj_exist != DBNull.Value)  //modify
                    {
                        string str_Update = string.Format("UPDATE UniversalTerWayConfig SET PointID='{0}' WHERE ID = '{1}'",
                            dr["PointID"].ToString(), obj_exist.ToString());
                        SQLHelper.ExecuteNonQuery(str_Update, null);
                    }
                    else  //insert
                    {
                        string str_Insert = string.Format("INSERT INTO UniversalTerWayConfig(TerminalID,PointID) VALUES('{0}','{1}')",
                            dr["TerminalID"].ToString(), dr["PointID"].ToString());
                        SQLHelper.ExecuteNonQuery(str_Insert, null);
                    }
                }
            }
        }
        public void UpdateSQLite_UniversalTerWayConfig(DateTime mindate)
        {
            string str_SQLite = "SELECT TerminalID,PointID FROM UniversalTerWayConfig WHERE ModifyTime>@date";
            SqlParameter param = new SqlParameter("@date", DbType.DateTime);
            param.Value = mindate;
            DataTable dt_sql = SQLHelper.ExecuteDataTable(str_SQLite, param);
            if ((dt_sql != null) && dt_sql.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_sql.Rows)
                {
                    string str_SQLiteExist = "SELECT ID FROM UniversalTerWayConfig WHERE TerminalID = '" + dr["TerminalID"].ToString() + "'";
                    object obj_exist = SQLiteHelper.ExecuteScalar(str_SQLiteExist, null);
                    if (obj_exist != null && obj_exist != DBNull.Value)  //modify
                    {
                        string str_Update = string.Format("UPDATE UniversalTerWayConfig SET PointID='{0}' WHERE ID = '{1}'",
                            dr["PointID"].ToString(), obj_exist.ToString());
                        SQLiteHelper.ExecuteNonQuery(str_Update, null);
                    }
                    else  //insert
                    {
                        string str_Insert = string.Format("INSERT INTO UniversalTerWayConfig(TerminalID,PointID) VALUES('{0}','{1}')",
                            dr["TerminalID"].ToString(), dr["PointID"].ToString());
                        SQLiteHelper.ExecuteNonQuery(str_Insert, null);
                    }
                }
            }
        }
        #endregion

        #region 同步UniversalTerWayType Table
        public void Update_UniversalTerWayType()
        {
            SqlTransaction trans = null;
            try
            {
                //获得服务器上所有ID
                List<int> lst_sql_ids = new List<int>();
                string str_SQL_Exist = "SELECT ID FROM UniversalTerWayType";
                SqlDataReader sql_reader = SQLHelper.ExecuteReader(str_SQL_Exist, null);
                while (sql_reader.Read())
                {
                    lst_sql_ids.Add(Convert.ToInt32(sql_reader[0]));
                }
                //得到本地已存在且同步过的ID
                string str_SQLite_Exist = "SELECT ID FROM UniversalTerWayType WHERE SyncState=0";
                SQLiteDataReader sqlite_reader= SQLiteHelper.ExecuteReader(str_SQLite_Exist, null);
                int id;
                while (sqlite_reader.Read())
                {
                    id = Convert.ToInt32(sqlite_reader[0]);
                    if (lst_sql_ids.Contains(id))
                    {
                        lst_sql_ids.Remove(id);
                    }
                }
                //删除本地有但是服务器上不存在的数据(注:本地已同步过)
                if (lst_sql_ids.Count > 0)
                {
                    string str_del_ids = "";
                    foreach(int tmp_id in lst_sql_ids)
                    {
                        str_del_ids+="'"+tmp_id+"',";
                    }
                    if (str_del_ids.EndsWith(","))
                        str_del_ids = str_del_ids.Substring(0, str_del_ids.Length - 1);
                    string str_SQLite_Del = "DELETE FROM UniversalTerWayType WHERE ID IN (" + str_del_ids + ")";
                    SQLiteHelper.ExecuteNonQuery(str_SQLite_Del, null);
                }

                //本地新增数据更新到服务器
                string str_SQLite_Addtion = "SELECT ID,Level,ParentID,WayType,Name,MaxMeasureRange,MaxMeasureRangeFlag,Precision,Unit FROM UniversalTerWayType WHERE SyncState=1";
                DataTable dt_sql = SQLiteHelper.ExecuteDataTable(str_SQLite_Addtion, null);
                if ((dt_sql != null) && dt_sql.Rows.Count > 0)
                {
                    //id以SQL数据库中为准，如果离线数据库(SQLite)中有过更新时，先更新到SQL，并将最大ID加一作为当前ID，并将SQLite上的ID更新为SQL上一样的ID
                    string str_GetSQLMaxID = "SELECT MAX(ID) FROM UniversalTerWayType";
                    object obj_maxid = SQLHelper.ExecuteScalar(str_GetSQLMaxID, null);
                    long maxid = (obj_maxid != null && obj_maxid != DBNull.Value) ? Convert.ToInt64(obj_maxid) : 0;
                    long parentid;

                    trans=SQLHelper.GetTransaction();
                    SqlCommand command = new SqlCommand();
                    command.CommandText = "INSERT INTO UniversalTerWayType(ID,Level,ParentID,WayType,Name,MaxMeasureRange,MaxMeasureRangeFlag,Precision,Unit) VALUES(@id,@level,@parentid,@waytype,@name,@maxrange,@maxrangeflag,@precision,@unit)";
                    command.CommandType = CommandType.Text;
                    command.Connection = SQLHelper.Conn;
                    command.Transaction = trans;

                    SqlParameter[] parms = new SqlParameter[]{
                        new SqlParameter("@id",SqlDbType.BigInt),
                        new SqlParameter("@level",SqlDbType.Int),
                        new SqlParameter("@parentid",SqlDbType.Int),
                        new SqlParameter("@waytype",SqlDbType.Int),
                        new SqlParameter("@name",SqlDbType.NChar,30),

                        new SqlParameter("@maxrange",SqlDbType.Decimal),
                        new SqlParameter("@maxrangeflag",SqlDbType.Decimal),
                        new SqlParameter("@precision",SqlDbType.Int),
                        new SqlParameter("@unit",SqlDbType.NChar,20)
                    };
                    command.Parameters.AddRange(parms);                            

                    DataRow[] Parent_drs = dt_sql.Select("Level ='1'");
                    if (Parent_drs != null && Parent_drs.Length > 0)
                    {
                        foreach (DataRow p_dr in Parent_drs)
                        {
                            maxid++;
                            parentid = maxid;
                            parms[0].Value = maxid;
                            parms[1].Value = 1;
                            parms[2].Value = null;
                            parms[3].Value = p_dr["WayType"];
                            parms[4].Value = p_dr["Name"];

                            parms[5].Value = p_dr["MaxMeasureRange"];
                            parms[6].Value = p_dr["MaxMeasureRangeFlag"];
                            parms[7].Value = p_dr["Precision"];
                            parms[8].Value = p_dr["Unit"];
                            command.ExecuteNonQuery();

                            DataRow[] Child_drs = dt_sql.Select("Level!='1' AND ParentID='" + p_dr["ID"].ToString() + "'");
                            if (Child_drs != null && Child_drs.Length > 0)
                            {
                                foreach (DataRow c_dr in Child_drs)
                                {
                                    maxid++;
                                    parms[0].Value = maxid;
                                    parms[1].Value = 2;
                                    parms[2].Value = parentid;
                                    parms[3].Value = p_dr["WayType"];
                                    parms[4].Value = p_dr["Name"];

                                    parms[5].Value = p_dr["MaxMeasureRange"];
                                    parms[6].Value = p_dr["MaxMeasureRangeFlag"];
                                    parms[7].Value = p_dr["Precision"];
                                    parms[8].Value = p_dr["Unit"];
                                    command.ExecuteNonQuery();
                                    //dt_sql.Rows.Remove(c_dr);
                                }
                            }
                            //dt_sql.Rows.Remove(p_dr);
                        }
                    }
                    //将新增过的数据ID更新，将Flag更新


                    //从服务器上获取本地不存在的

                    //UpdateSQL
                    //先删除后插入，不使用update,使其id这样更新也不同，方便同步时区分
                }  
            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();
            }
        }
        #endregion

    }
}
