using System;
using System.Collections.Generic;
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
        
        #region 同步Terminal Table
        public void UpdateSQL_Terminal()
        {
            SqlTransaction trans = null;
            try
            {
                trans = SQLHelper.GetTransaction();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.Connection = SQLHelper.Conn;
                command.Transaction = trans;

                //本地新增数据更新到服务器
                string str_SQLite_Addtion = "SELECT ID,TerminalID,TerminalName,TerminalType,Address,Remark FROM Terminal WHERE SyncState=1";
                DataTable dt_sql = SQLiteHelper.ExecuteDataTable(str_SQLite_Addtion, null);
                if ((dt_sql != null) && dt_sql.Rows.Count > 0)
                {
                    //id以SQL数据库中为准，如果离线数据库(SQLite)中有过更新时，先更新到SQL，并将最大ID加一作为当前ID，并将SQLite上的ID更新为SQL上一样的ID
                    string str_GetSQLMaxID = "SELECT MAX(ID) FROM Terminal";
                    command.CommandText = str_GetSQLMaxID;
                    command.Parameters.Clear();
                    object obj_maxid = command.ExecuteScalar();
                    long maxid = (obj_maxid != null && obj_maxid != DBNull.Value) ? Convert.ToInt64(obj_maxid) : 0;

                    command.CommandText = "INSERT INTO Terminal(ID,TerminalID,TerminalName,TerminalType,Address,Remark) VALUES(@id,@TerminalID,@TerminalName,@TerminalType,@Address,@Remark)";
                    command.Parameters.Clear();
                    SqlParameter[] parms = new SqlParameter[]{
                        new SqlParameter("@id",SqlDbType.BigInt),
                        new SqlParameter("@TerminalID",SqlDbType.Int),
                        new SqlParameter("@TerminalName",SqlDbType.NChar,200),
                        new SqlParameter("@TerminalType",SqlDbType.Int),
                        new SqlParameter("@Address",SqlDbType.NChar,200),

                        new SqlParameter("@Remark",SqlDbType.NChar,200)
                    };
                    command.Parameters.AddRange(parms);

                    List<SyncIdsEntity> ht_insert = new List<SyncIdsEntity>();  //local server
                    foreach (DataRow dr in dt_sql.Rows)
                    {
                        maxid++;
                        parms[0].Value = maxid;
                        parms[1].Value = dr["TerminalID"];
                        parms[2].Value = dr["TerminalName"];
                        parms[3].Value = dr["TerminalType"];
                        parms[4].Value = dr["Address"];

                        parms[5].Value = dr["Remark"];

                        command.ExecuteNonQuery();
                        ht_insert.Add(new SyncIdsEntity(dr["ID"].ToString(), maxid));
                    }

                    //将新增过的数据ID更新，将Flag更新
                    string SQL_Update_Local = "UPDATE Terminal SET ID = @newid, SyncState=0 WHERE ID=@oldid";
                    SQLiteParameter[] parms_update_local = new SQLiteParameter[]{
                                new SQLiteParameter("@oldid",DbType.Int32),
                                new SQLiteParameter("@newid",DbType.Int32)
                            };
                    foreach (SyncIdsEntity identry in ht_insert)
                    {
                        parms_update_local[0].Value = identry.localid;
                        parms_update_local[1].Value = identry.serverid;
                        SQLiteHelper.ExecuteNonQuery(SQL_Update_Local, parms_update_local);
                    }
                }

                //本地删除更新至服务器
                string str_SQLite_Del = "SELECT DISTINCT ID FROM Terminal WHERE SyncState=-1";
                SQLiteDataReader read_del = SQLiteHelper.ExecuteReader(str_SQLite_Del, null);
                string str_sql_delid = "";
                while (read_del.Read())
                {
                    str_sql_delid += "'" + read_del["Id"].ToString() + "',";
                }
                if (!string.IsNullOrEmpty(str_sql_delid))
                {
                    str_sql_delid = str_sql_delid.Substring(0, str_sql_delid.Length - 1);
                    command.CommandText = "DELETE FROM Terminal WHERE ID in (" + str_sql_delid + ")";
                    command.Parameters.Clear();
                    command.ExecuteNonQuery();
                }
                read_del.Close();

                //获得服务器上所有ID
                List<int> lst_sql_ids = new List<int>();
                string str_SQL_Exist = "SELECT ID FROM Terminal";
                command.CommandText = str_SQL_Exist;
                command.Parameters.Clear();
                SqlDataReader sql_reader = command.ExecuteReader();
                while (sql_reader.Read())
                {
                    lst_sql_ids.Add(Convert.ToInt32(sql_reader[0]));
                }
                sql_reader.Close();
                //得到本地已存在且同步过的ID
                List<int> lst_sqlite = new List<int>();
                string str_SQLite_Exist = "SELECT DISTINCT ID FROM Terminal";  //WHERE SyncState=0
                SQLiteDataReader sqlite_reader = SQLiteHelper.ExecuteReader(str_SQLite_Exist, null);
                int id;
                while (sqlite_reader.Read())
                {
                    id = Convert.ToInt32(sqlite_reader[0]);
                    lst_sqlite.Add(id);
                }
                sqlite_reader.Close();
                foreach (int id_tmp in lst_sqlite)
                {
                    if (!lst_sql_ids.Contains(id_tmp))  //本地有，服务器上不存在，从本地删除掉
                    {
                        str_SQLite_Del = "DELETE FROM Terminal WHERE ID='" + id_tmp + "'";
                        SQLiteHelper.ExecuteNonQuery(str_SQLite_Del, null);
                    }
                }
                string str_SQLite_addtion_ids = "";
                foreach (int id_tmp in lst_sql_ids)
                {
                    if (!lst_sqlite.Contains(id_tmp)) //服务器有，本地没有，更新至本地
                    {
                        str_SQLite_addtion_ids += "'" + id_tmp + "',";
                    }
                }

                if (str_SQLite_addtion_ids.Length > 0)
                {
                    str_SQLite_addtion_ids = str_SQLite_addtion_ids.Substring(0, str_SQLite_addtion_ids.Length - 1);
                    command.Parameters.Clear();
                    command.CommandText = "SELECT ID,TerminalID,TerminalName,TerminalType,Address,Remark FROM Terminal WHERE ID IN(" + str_SQLite_addtion_ids + ")";
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    dt_sql = new DataTable();
                    adapter.Fill(dt_sql);
                    if (dt_sql != null && dt_sql.Rows.Count > 0)
                    {
                        string str_SQLite_Insert = @"INSERT INTO Terminal(ID,TerminalID,TerminalName,TerminalType,Address,Remark,SyncState) VALUES(
                                                        @ID,@TerminalID,@TerminalName,@TerminalType,@Address,@Remark,@SyncState)";
                        SQLiteParameter[] parms_sqlite = new SQLiteParameter[]{
                                new SQLiteParameter("@ID",DbType.Int32),
                                new SQLiteParameter("@TerminalID",DbType.Int32),
                                new SQLiteParameter("@TerminalName",DbType.String,200),
                                new SQLiteParameter("@TerminalType",DbType.Int32),
                                new SQLiteParameter("@Address",DbType.String,200),

                                new SQLiteParameter("@Remark",DbType.String,200),
                                new SQLiteParameter("@SyncState",DbType.Int32) };

                        foreach (DataRow dr in dt_sql.Rows)
                        {
                            parms_sqlite[0].Value = dr["ID"];
                            parms_sqlite[1].Value = dr["TerminalID"];
                            parms_sqlite[2].Value = dr["TerminalName"];
                            parms_sqlite[3].Value = dr["TerminalType"];
                            parms_sqlite[4].Value = dr["Address"];

                            parms_sqlite[5].Value = dr["Remark"];
                            parms_sqlite[6].Value = 0;

                            SQLiteHelper.ExecuteNonQuery(str_SQLite_Insert, parms_sqlite);
                        }
                    }
                }

                //UpdateSQL
                //先删除后插入，不使用update,使其id这样更新也不同，方便同步时区分 
                trans.Commit();
            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();
            }
        }
        #endregion

        #region 同步PreTerConfig Table
        public void UpdateSQL_PreTerConfig()
        {
        }
        #endregion

        #region 同步UniversalTerConfig Table
        public void UpdateSQL_UniversalTerConfig()
        {
            
        }
        #endregion

        #region 同步UniversalTerWayConfig Table
        public void UpdateSQL_UniversalTerWayConfig()
        {
            SqlTransaction trans = null;
            try
            {
                trans = SQLHelper.GetTransaction();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.Connection = SQLHelper.Conn;
                command.Transaction = trans;

                //本地新增数据更新到服务器
                string str_SQLite_Addtion = "SELECT ID,TerminalID,Sequence,PointID FROM UniversalTerWayConfig WHERE SyncState=1";
                DataTable dt_sql = SQLiteHelper.ExecuteDataTable(str_SQLite_Addtion, null);
                if ((dt_sql != null) && dt_sql.Rows.Count > 0)
                {
                    //id以SQL数据库中为准，如果离线数据库(SQLite)中有过更新时，先更新到SQL，并将最大ID加一作为当前ID，并将SQLite上的ID更新为SQL上一样的ID
                    string str_GetSQLMaxID = "SELECT MAX(ID) FROM UniversalTerWayConfig";
                    command.CommandText = str_GetSQLMaxID;
                    command.Parameters.Clear();
                    object obj_maxid = command.ExecuteScalar();
                    long maxid = (obj_maxid != null && obj_maxid != DBNull.Value) ? Convert.ToInt64(obj_maxid) : 0;

                    command.CommandText = "INSERT INTO UniversalTerWayConfig(ID,TerminalID,Sequence,PointID) VALUES(@id,@TerminalID,@Sequence,@PointID)";
                    command.Parameters.Clear();
                    SqlParameter[] parms = new SqlParameter[]{
                        new SqlParameter("@id",SqlDbType.BigInt),
                        new SqlParameter("@TerminalID",SqlDbType.Int),
                        new SqlParameter("@Sequence",SqlDbType.Int),
                        new SqlParameter("@PointID",SqlDbType.Int)
                    };
                    command.Parameters.AddRange(parms);

                    List<SyncIdsEntity> ht_insert = new List<SyncIdsEntity>();  //local server
                    foreach (DataRow dr in dt_sql.Rows)
                    {
                        maxid++;
                        parms[0].Value = maxid;
                        parms[1].Value = dr["TerminalID"];
                        parms[2].Value = dr["Sequence"];
                        parms[3].Value = dr["PointID"];

                        command.ExecuteNonQuery();
                        ht_insert.Add(new SyncIdsEntity(dr["ID"].ToString(), maxid));
                    }

                    //将新增过的数据ID更新，将Flag更新
                    string SQL_Update_Local = "UPDATE UniversalTerWayConfig SET ID = @newid, SyncState=0 WHERE ID=@oldid";
                    SQLiteParameter[] parms_update_local = new SQLiteParameter[]{
                                new SQLiteParameter("@oldid",DbType.Int32),
                                new SQLiteParameter("@newid",DbType.Int32)
                            };
                    foreach (SyncIdsEntity identry in ht_insert)
                    {
                        parms_update_local[0].Value = identry.localid;
                        parms_update_local[1].Value = identry.serverid;
                        SQLiteHelper.ExecuteNonQuery(SQL_Update_Local, parms_update_local);
                    }
                }

                //本地删除更新至服务器
                string str_SQLite_Del = "SELECT DISTINCT ID FROM UniversalTerWayConfig WHERE SyncState=-1";
                SQLiteDataReader read_del = SQLiteHelper.ExecuteReader(str_SQLite_Del, null);
                string str_sql_delid = "";
                while (read_del.Read())
                {
                    str_sql_delid += "'" + read_del["Id"].ToString() + "',";
                }
                if (!string.IsNullOrEmpty(str_sql_delid))
                {
                    str_sql_delid = str_sql_delid.Substring(0, str_sql_delid.Length - 1);
                    command.CommandText = "DELETE FROM UniversalTerWayConfig WHERE ID in (" + str_sql_delid + ")";
                    command.Parameters.Clear();
                    command.ExecuteNonQuery();
                }
                read_del.Close();

                //获得服务器上所有ID
                List<int> lst_sql_ids = new List<int>();
                string str_SQL_Exist = "SELECT ID FROM UniversalTerWayConfig";
                command.CommandText = str_SQL_Exist;
                command.Parameters.Clear();
                SqlDataReader sql_reader = command.ExecuteReader();
                while (sql_reader.Read())
                {
                    lst_sql_ids.Add(Convert.ToInt32(sql_reader[0]));
                }
                sql_reader.Close();
                //得到本地已存在且同步过的ID
                List<int> lst_sqlite = new List<int>();
                string str_SQLite_Exist = "SELECT DISTINCT ID FROM UniversalTerWayConfig";  //WHERE SyncState=0
                SQLiteDataReader sqlite_reader = SQLiteHelper.ExecuteReader(str_SQLite_Exist, null);
                int id;
                while (sqlite_reader.Read())
                {
                    id = Convert.ToInt32(sqlite_reader[0]);
                    lst_sqlite.Add(id);
                }
                sqlite_reader.Close();
                foreach (int id_tmp in lst_sqlite)
                {
                    if (!lst_sql_ids.Contains(id_tmp))  //本地有，服务器上不存在，从本地删除掉
                    {
                        str_SQLite_Del = "DELETE FROM UniversalTerWayConfig WHERE ID='" + id_tmp + "'";
                        SQLiteHelper.ExecuteNonQuery(str_SQLite_Del, null);
                    }
                }
                string str_SQLite_addtion_ids = "";
                foreach (int id_tmp in lst_sql_ids)
                {
                    if (!lst_sqlite.Contains(id_tmp)) //服务器有，本地没有，更新至本地
                    {
                        str_SQLite_addtion_ids += "'" + id_tmp + "',";
                    }
                }

                if (str_SQLite_addtion_ids.Length > 0)
                {
                    str_SQLite_addtion_ids = str_SQLite_addtion_ids.Substring(0, str_SQLite_addtion_ids.Length - 1);
                    command.Parameters.Clear();
                    command.CommandText = "SELECT ID,TerminalID,Sequence,PointID FROM UniversalTerWayConfig WHERE ID IN(" + str_SQLite_addtion_ids + ")";
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    dt_sql = new DataTable();
                    adapter.Fill(dt_sql);
                    if (dt_sql != null && dt_sql.Rows.Count > 0)
                    {
                        string str_SQLite_Insert = @"INSERT INTO UniversalTerWayConfig(ID,TerminalID,Sequence,PointID,SyncState) VALUES(
                                                        @ID,@TerminalID,@Sequence,@PointID,@SyncState)";
                        SQLiteParameter[] parms_sqlite = new SQLiteParameter[]{
                                new SQLiteParameter("@ID",DbType.Int32),
                                new SQLiteParameter("@TerminalID",DbType.Int32),
                                new SQLiteParameter("@Sequence",DbType.Int32),
                                new SQLiteParameter("@PointID",DbType.Int32),
                                new SQLiteParameter("@SyncState",DbType.Int32) };

                        foreach (DataRow dr in dt_sql.Rows)
                        {
                            parms_sqlite[0].Value = dr["ID"];
                            parms_sqlite[1].Value = dr["TerminalID"];
                            parms_sqlite[2].Value = dr["Sequence"];
                            parms_sqlite[3].Value = dr["PointID"];
                            parms_sqlite[4].Value = 0;

                            SQLiteHelper.ExecuteNonQuery(str_SQLite_Insert, parms_sqlite);
                        }
                    }
                }

                //UpdateSQL
                //先删除后插入，不使用update,使其id这样更新也不同，方便同步时区分 
                trans.Commit();
            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();
            }
        }
        #endregion

        #region 同步UniversalTerWayType Table
        public void Update_UniversalTerWayType()
        {
            SqlTransaction trans = null;
            try
            {
                trans = SQLHelper.GetTransaction();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.Connection = SQLHelper.Conn;
                command.Transaction = trans;

                //本地新增数据更新到服务器
                string str_SQLite_Addtion = "SELECT ID,Level,ParentID,WayType,Name,MaxMeasureRange,MaxMeasureRangeFlag,FrameWidth,Sequence,Precision,Unit FROM UniversalTerWayType WHERE SyncState=1";
                DataTable dt_sql = SQLiteHelper.ExecuteDataTable(str_SQLite_Addtion, null);
                if ((dt_sql != null) && dt_sql.Rows.Count > 0)
                {
                    //id以SQL数据库中为准，如果离线数据库(SQLite)中有过更新时，先更新到SQL，并将最大ID加一作为当前ID，并将SQLite上的ID更新为SQL上一样的ID
                    string str_GetSQLMaxID = "SELECT MAX(ID) FROM UniversalTerWayType";
                    command.CommandText = str_GetSQLMaxID;
                    command.Parameters.Clear();
                    object obj_maxid = command.ExecuteScalar();
                    long maxid = (obj_maxid != null && obj_maxid != DBNull.Value) ? Convert.ToInt64(obj_maxid) : 0;
                    long parentid;

                    command.CommandText = "INSERT INTO UniversalTerWayType(ID,Level,ParentID,WayType,Name,MaxMeasureRange,MaxMeasureRangeFlag,FrameWidth,Sequence,Precision,Unit) VALUES(@id,@level,@parentid,@waytype,@name,@maxrange,@maxrangeflag,@FrameWidth,@Sequence,@precision,@unit)";
                    command.Parameters.Clear();
                    SqlParameter[] parms = new SqlParameter[]{
                        new SqlParameter("@id",SqlDbType.BigInt),
                        new SqlParameter("@level",SqlDbType.Int),
                        new SqlParameter("@parentid",SqlDbType.Int),
                        new SqlParameter("@waytype",SqlDbType.Int),
                        new SqlParameter("@name",SqlDbType.NChar,30),

                        new SqlParameter("@maxrange",SqlDbType.Decimal),
                        new SqlParameter("@maxrangeflag",SqlDbType.Decimal),
                        new SqlParameter("@FrameWidth",SqlDbType.Int),
                        new SqlParameter("@Sequence",SqlDbType.Int),
                        new SqlParameter("@precision",SqlDbType.Int),

                        new SqlParameter("@unit",SqlDbType.NChar,20)
                    };
                    command.Parameters.AddRange(parms);

                    List<string> have_parent_child_ids = new List<string>(); //有父亲的子节点ID
                    List<SyncIdsEntity> ht_insert = new List<SyncIdsEntity>();  //local server
                    DataRow[] Parent_drs = dt_sql.Select("Level ='1'");
                    if (Parent_drs != null && Parent_drs.Length > 0)
                    {
                        foreach (DataRow p_dr in Parent_drs)
                        {
                            maxid++;
                            parentid = maxid;
                            parms[0].Value = maxid;
                            parms[1].Value = 1;
                            parms[2].Value = -1;
                            parms[3].Value = p_dr["WayType"];
                            parms[4].Value = p_dr["Name"];

                            parms[5].Value = p_dr["MaxMeasureRange"];
                            parms[6].Value = p_dr["MaxMeasureRangeFlag"];
                            parms[7].Value = p_dr["FrameWidth"];
                            parms[8].Value = p_dr["Sequence"];
                            parms[9].Value = p_dr["Precision"];

                            parms[10].Value = p_dr["Unit"];
                            command.ExecuteNonQuery();
                            ht_insert.Add(new SyncIdsEntity(p_dr["ID"].ToString(), maxid));

                            DataRow[] Child_drs = dt_sql.Select("Level<>'1' AND ParentID='" + p_dr["ID"].ToString() + "'");
                            if (Child_drs != null && Child_drs.Length > 0)
                            {
                                foreach (DataRow c_dr in Child_drs)
                                {
                                    maxid++;
                                    parms[0].Value = maxid;
                                    parms[1].Value = 2;
                                    parms[2].Value = parentid;
                                    parms[3].Value = c_dr["WayType"];
                                    parms[4].Value = c_dr["Name"];

                                    parms[5].Value = c_dr["MaxMeasureRange"];
                                    parms[6].Value = c_dr["MaxMeasureRangeFlag"];
                                    parms[7].Value = c_dr["FrameWidth"];
                                    parms[8].Value = c_dr["Sequence"];
                                    parms[9].Value = c_dr["Precision"];

                                    parms[10].Value = c_dr["Unit"];
                                    command.ExecuteNonQuery();
                                    ht_insert.Add(new SyncIdsEntity(c_dr["ID"].ToString(), maxid));
                                    have_parent_child_ids.Add(c_dr["ID"].ToString());
                                }
                            }
                        }
                    }

                    DataRow[] Child_drs_noparent;
                    if (have_parent_child_ids != null && have_parent_child_ids.Count > 0)
                    {
                        string str_tmp = "";
                        foreach (string child_id in have_parent_child_ids)
                        {
                            str_tmp += "'" + child_id + "',";
                        }
                        str_tmp = str_tmp.Substring(0, str_tmp.Length - 1);
                        Child_drs_noparent = dt_sql.Select("Level<>'1' AND ID NOT IN(" + str_tmp + ")");
                    }
                    else
                    {
                        Child_drs_noparent = dt_sql.Select("Level<>'1'");
                    }

                    if (Child_drs_noparent != null && Child_drs_noparent.Length > 0)
                    {
                        foreach (DataRow c_dr in Child_drs_noparent)
                        {
                            maxid++;
                            parms[0].Value = maxid;
                            parms[1].Value = 2;
                            parms[2].Value = c_dr["ParentID"];
                            parms[3].Value = c_dr["WayType"];
                            parms[4].Value = c_dr["Name"];

                            parms[5].Value = c_dr["MaxMeasureRange"];
                            parms[6].Value = c_dr["MaxMeasureRangeFlag"];
                            parms[7].Value = c_dr["FrameWidth"];
                            parms[8].Value = c_dr["Sequence"];
                            parms[9].Value = c_dr["Precision"];

                            parms[10].Value = c_dr["Unit"];
                            command.ExecuteNonQuery();
                            ht_insert.Add(new SyncIdsEntity(c_dr["ID"].ToString(), maxid));
                        }
                    }

                    //将新增过的数据ID更新，将Flag更新
                    string SQL_Update_Local = "UPDATE UniversalTerWayType SET ID = @newid, SyncState=0 WHERE ID=@oldid";
                    SQLiteParameter[] parms_update_local = new SQLiteParameter[]{
                                new SQLiteParameter("@oldid",DbType.Int32),
                                new SQLiteParameter("@newid",DbType.Int32)
                            };
                    foreach (SyncIdsEntity identry in ht_insert)
                    {
                        parms_update_local[0].Value = identry.localid;
                        parms_update_local[1].Value = identry.serverid;
                        SQLiteHelper.ExecuteNonQuery(SQL_Update_Local, parms_update_local);
                    }
                }

                //本地删除更新至服务器
                string str_SQLite_Del = "SELECT DISTINCT ID FROM UniversalTerWayType WHERE SyncState=-1";
                SQLiteDataReader read_del = SQLiteHelper.ExecuteReader(str_SQLite_Del, null);
                string str_sql_delid = "";
                while (read_del.Read())
                {
                    str_sql_delid += "'" + read_del["Id"].ToString() + "',";
                }
                if (!string.IsNullOrEmpty(str_sql_delid))
                {
                    str_sql_delid = str_sql_delid.Substring(0, str_sql_delid.Length - 1);
                    command.CommandText = "DELETE FROM UniversalTerWayType WHERE ID in (" + str_sql_delid + ")";
                    command.Parameters.Clear();
                    command.ExecuteNonQuery();
                }
                read_del.Close();

                //获得服务器上所有ID
                List<int> lst_sql_ids = new List<int>();
                string str_SQL_Exist = "SELECT ID FROM UniversalTerWayType";
                command.CommandText = str_SQL_Exist;
                command.Parameters.Clear();
                SqlDataReader sql_reader = command.ExecuteReader();
                while (sql_reader.Read())
                {
                    lst_sql_ids.Add(Convert.ToInt32(sql_reader[0]));
                }
                sql_reader.Close();
                //得到本地已存在且同步过的ID
                List<int> lst_sqlite = new List<int>();
                string str_SQLite_Exist = "SELECT DISTINCT ID FROM UniversalTerWayType";  //WHERE SyncState=0
                SQLiteDataReader sqlite_reader = SQLiteHelper.ExecuteReader(str_SQLite_Exist, null);
                int id;
                while (sqlite_reader.Read())
                {
                    id = Convert.ToInt32(sqlite_reader[0]);
                    lst_sqlite.Add(id);
                }
                sqlite_reader.Close();
                foreach (int id_tmp in lst_sqlite)
                {
                    if (!lst_sql_ids.Contains(id_tmp))  //本地有，服务器上不存在，从本地删除掉
                    {
                        str_SQLite_Del = "DELETE FROM UniversalTerWayType WHERE ID='" + id_tmp + "'";
                        SQLiteHelper.ExecuteNonQuery(str_SQLite_Del, null);
                    }
                }
                string str_SQLite_addtion_ids = "";
                foreach (int id_tmp in lst_sql_ids)
                {
                    if (!lst_sqlite.Contains(id_tmp)) //服务器有，本地没有，更新至本地
                    {
                        str_SQLite_addtion_ids += "'" + id_tmp + "',";
                    }
                }

                if (str_SQLite_addtion_ids.Length > 0)
                {
                    str_SQLite_addtion_ids = str_SQLite_addtion_ids.Substring(0, str_SQLite_addtion_ids.Length - 1);
                    command.Parameters.Clear();
                    command.CommandText = "SELECT ID,Level,ParentID,WayType,Name,MaxMeasureRange,MaxMeasureRangeFlag,FrameWidth,Sequence,Precision,Unit FROM UniversalTerWayType WHERE ID IN(" + str_SQLite_addtion_ids + ")";
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    dt_sql = new DataTable();
                    adapter.Fill(dt_sql);
                    if (dt_sql != null && dt_sql.Rows.Count > 0)
                    {
                        string str_SQLite_Insert = @"INSERT INTO UniversalTerWayType(ID,Level,ParentID,WayType,Name,MaxMeasureRange,MaxMeasureRangeFlag,FrameWidth,Sequence,Precision,Unit,SyncState) VALUES(
                                                        @ID,@Level,@ParentID,@WayType,@Name,@MaxMeasureRange,@MaxMeasureRangeFlag,@FrameWidth,@Sequence,@Precision,@Unit,@SyncState)";
                        SQLiteParameter[] parms_sqlite = new SQLiteParameter[]{
                                new SQLiteParameter("@ID",DbType.Int32),
                                new SQLiteParameter("@Level",DbType.Int32),
                                new SQLiteParameter("@ParentID",DbType.Int32),
                                new SQLiteParameter("@WayType",DbType.Int32),
                                new SQLiteParameter("@Name",DbType.String,30),

                                new SQLiteParameter("@MaxMeasureRange",DbType.Decimal),
                                new SQLiteParameter("@MaxMeasureRangeFlag",DbType.Decimal),
                                new SQLiteParameter("@FrameWidth",DbType.Int32),
                                new SQLiteParameter("@Sequence",DbType.Int32),
                                new SQLiteParameter("@Precision",DbType.Int32),
                                new SQLiteParameter("@Unit",DbType.String),
                                new SQLiteParameter("@SyncState",DbType.Int32) };

                        foreach (DataRow dr in dt_sql.Rows)
                        {
                            parms_sqlite[0].Value = dr["ID"];
                            parms_sqlite[1].Value = dr["Level"];
                            parms_sqlite[2].Value = dr["ParentID"];
                            parms_sqlite[3].Value = dr["WayType"];
                            parms_sqlite[4].Value = dr["Name"];

                            parms_sqlite[5].Value = dr["MaxMeasureRange"];
                            parms_sqlite[6].Value = dr["MaxMeasureRangeFlag"];
                            parms_sqlite[7].Value = dr["FrameWidth"];
                            parms_sqlite[8].Value = dr["Sequence"];
                            parms_sqlite[9].Value = dr["Precision"];
                            parms_sqlite[10].Value = dr["Unit"];
                            parms_sqlite[11].Value = 0;

                            SQLiteHelper.ExecuteNonQuery(str_SQLite_Insert, parms_sqlite);
                        }
                    }
                }

                //UpdateSQL
                //先删除后插入，不使用update,使其id这样更新也不同，方便同步时区分 

                trans.Commit();
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
