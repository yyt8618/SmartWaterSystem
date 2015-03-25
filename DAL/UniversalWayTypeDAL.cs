using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Entity;
using System.Data;
using System.Data.SQLite;

namespace DAL
{
    public class UniversalWayTypeDAL
    {
        public int GetMaxId()
        {
            string SQL = "SELECT MAX(id) FROM UniversalTerWayType";
            object obj = SQLiteHelper.ExecuteScalar(SQL, null);
            if (obj != null && obj != DBNull.Value)
            {
                return Convert.ToInt32(obj);
            }
            else
            {
                return 0;
            }
        }

        public int GetMaxSequence(int parentId)
        {
            string SQL = "SELECT MAX(Sequence) FROM UniversalTerWayType WHERE ParentId='"+parentId+"'";
            object obj = SQLiteHelper.ExecuteScalar(SQL, null);
            if (obj != null && obj != DBNull.Value)
            {
                return Convert.ToInt32(obj);
            }
            else
            {
                return 0;
            }
        }

        public int TypeExist(UniversalCollectType type, string name)
        {
            string SQL = "SELECT COUNT(1) FROM UniversalTerWayType WHERE WayType='" + (int)type + "' AND Name='" + name + "'";
            object obj = SQLiteHelper.ExecuteScalar(SQL, null);
            if (obj != null && obj != DBNull.Value)
            {
                return (Convert.ToInt32(obj) > 0) ? 1 : 0;
            }
            else
            {
                return 0;
            }
        }

        public int Insert(UniversalWayTypeEntity entity)
        {
            if (entity == null)
                return 0;

            string SQL = @"INSERT INTO 
                            UniversalTerWayType(ID,Level,ParentID,WayType,Name,FrameWidth,Sequence,MaxMeasureRange,MaxMeasureRangeFlag,Precision,Unit,SyncState,ModifyTime) VALUES(
                            @ID,@Level,@ParentID,@WayType,@Name,@FrameWidth,@Sequence,@MaxMeasureRange,@MaxMeasureRangeFlag,@Precision,@Unit,@SyncState,@ModifyTime)";
            SQLiteParameter[] parms = new SQLiteParameter[]{
                new SQLiteParameter("@ID",DbType.Int32),
                new SQLiteParameter("@Level",DbType.Int32),
                new SQLiteParameter("@ParentID",DbType.Int32),
                new SQLiteParameter("@WayType",DbType.Int32),
                new SQLiteParameter("@Name",DbType.String),

                new SQLiteParameter("@FrameWidth",DbType.Int32),
                new SQLiteParameter("@Sequence",DbType.Int32),
                new SQLiteParameter("@MaxMeasureRange",DbType.Single),
                new SQLiteParameter("@MaxMeasureRangeFlag",DbType.Single),
                new SQLiteParameter("@Precision",DbType.Int32),

                new SQLiteParameter("@Unit",DbType.String),
                new SQLiteParameter("@SyncState",DbType.Int32),
                new SQLiteParameter("@ModifyTime",DbType.DateTime)
            };
            parms[0].Value = entity.ID;
            parms[1].Value = entity.Level;
            parms[2].Value = entity.ParentID;
            parms[3].Value = (int)entity.WayType;
            parms[4].Value = entity.Name;

            parms[5].Value = entity.FrameWidth;
            parms[6].Value = entity.Sequence;
            parms[7].Value = entity.MaxMeasureRange;
            parms[8].Value = entity.ManMeasureRangeFlag;
            parms[9].Value = entity.Precision;

            parms[10].Value = entity.Unit;
            parms[11].Value = entity.SyncState;
            parms[12].Value = entity.ModifyTime;

            SQLiteHelper.ExecuteNonQuery(SQL, parms);
            return 1;
        }

        public int Delete(int id)
        {
            if (id < 0)
                return 0;
            string SQL = "DELETE FROM UniversalTerWayType WHERE ID='" + id + "'";
            SQLiteHelper.ExecuteNonQuery(SQL, null);
            return 1;
        }

        public List<UniversalWayTypeEntity> Select(string where)
        {
            string SQL = "SELECT ID,Level,ParentID,WayType,Name,FrameWidth,Sequence,MaxMeasureRange,MaxMeasureRangeFlag,Precision,Unit,SyncState,ModifyTime FROM UniversalTerWayType ";
            if (!string.IsNullOrEmpty(where))
                SQL +=  where;
            using (SQLiteDataReader reader = SQLiteHelper.ExecuteReader(SQL, null))
            {
                List<UniversalWayTypeEntity> lst = new List<UniversalWayTypeEntity>();
                while (reader.Read())
                {
                    UniversalWayTypeEntity entity = new UniversalWayTypeEntity();

                    entity.ID = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : -1;
                    entity.Level = reader["Level"] != DBNull.Value ? Convert.ToInt32(reader["Level"]) : 0;
                    entity.ParentID = reader["ParentID"] != DBNull.Value ? Convert.ToInt32(reader["ParentID"]) : 0;
                    entity.WayType = reader["WayType"] != DBNull.Value ? (UniversalCollectType)(Convert.ToInt32(reader["WayType"])) : UniversalCollectType.Simulate;
                    entity.Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : "";

                    entity.FrameWidth = reader["FrameWidth"] != DBNull.Value ? Convert.ToInt32(reader["FrameWidth"]) : 2;
                    entity.Sequence = reader["Sequence"] != DBNull.Value ? Convert.ToInt32(reader["Sequence"]) : 1;
                    entity.MaxMeasureRange = reader["MaxMeasureRange"] != DBNull.Value ? Convert.ToSingle(reader["MaxMeasureRange"]) : 0f;
                    entity.ManMeasureRangeFlag = reader["MaxMeasureRangeFlag"] != DBNull.Value ? Convert.ToSingle(reader["MaxMeasureRangeFlag"]) : 0f;
                    entity.Precision = reader["Precision"] != DBNull.Value ? Convert.ToInt32(reader["Precision"]) : 2;

                    entity.Unit = reader["Unit"] != DBNull.Value ? reader["Unit"].ToString() : "";
                    entity.SyncState = reader["SyncState"] != DBNull.Value ? Convert.ToInt32(reader["SyncState"]) : 0;
                    entity.ModifyTime = reader["ModifyTime"] != DBNull.Value ? Convert.ToDateTime(reader["ModifyTime"]) : ConstValue.MinDateTime;

                    lst.Add(entity);
                }
                return lst;
            }
            return null;
        }
        
    }
}
