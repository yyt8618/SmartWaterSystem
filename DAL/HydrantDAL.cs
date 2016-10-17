using System.Collections.Generic;
using Common;
using Entity;
using System.Data.SqlClient;
using System;
using System.Data;

namespace DAL
{
    public class HydrantDAL
    {
        public List<HydrantEntity> SelectAll()
        {
            List<HydrantEntity> lstHydrant = null;
            string SQL = @"SELECT h.HydrantID,h.Addr,h.longitude,h.latitude,d.Operate,d.PressValue,d.OpenAngle,d.IsAlarm,d.CollTime,h.Remark 
                        FROM Hydrant h LEFT JOIN 
                                 (SELECT a.TerminalID,a.Operate,a.PressValue,a.OpenAngle,a.IsAlarm,a.CollTime FROM [Hydrant_Real] a,(SELECT TerminalID,MAX(colltime) 
                                AS colltime FROM Hydrant_Real GROUP BY TerminalID) b WHERE
                                a.TerminalID=b.TerminalID AND a.CollTime = b.colltime) d 
                        ON h.HydrantID=d.TerminalID";
            using (SqlDataReader reader = SQLHelper.ExecuteReader(SQL, null))
            {
                lstHydrant = new List<HydrantEntity>();
                while (reader.Read())
                {
                    HydrantEntity entity = new HydrantEntity();
                    entity.HydrantID = reader["HydrantID"].ToString();
                    entity.Addr = reader["Addr"].ToString();
                    entity.Longtitude = reader["longitude"].ToString();
                    entity.Latitude = reader["latitude"].ToString();

                    if (reader["Operate"] != DBNull.Value)      //有数据
                    {
                        entity.HaveData = true;
                        entity.OptType = reader["Operate"] != DBNull.Value ? (HydrantOptType)Convert.ToInt32(reader["Operate"]) : HydrantOptType.Close;
                        entity.PressValue = reader["PressValue"] != DBNull.Value ? Convert.ToSingle(reader["PressValue"]) : 0f;
                        entity.OpenAngle = reader["OpenAngle"] != DBNull.Value ? Convert.ToInt32(reader["OpenAngle"]) : 0;
                        entity.IsAlarm = reader["IsAlarm"] != DBNull.Value ? (Convert.ToInt32(reader["IsAlarm"]) == 1 ? true : false) : false;
                        entity.Remark = reader["Remark"] != DBNull.Value ? reader["Remark"].ToString() : "";
                        entity.CollTime = reader["CollTime"] != DBNull.Value ? Convert.ToDateTime(reader["CollTime"]).ToString("yyyy-MM-dd HH:mm") : DateTime.MinValue.ToString("yyyy-MM-dd HH:mm");
                    }
                    else
                    {
                        entity.HaveData = false;
                        entity.OptType = HydrantOptType.Close;
                        entity.PressValue = 0;
                        entity.OpenAngle = 0;
                        entity.IsAlarm = false;
                        entity.Remark = "";
                        entity.CollTime = DateTime.MinValue.ToString("yyyy-MM-dd HH:mm");
                    }
                    lstHydrant.Add(entity);
                }
            }
            return lstHydrant;
        }
        public void Insert(string id, string addr, string longitude, string latitude,string remark)
        {
            string SQL = string.Format("INSERT INTO Hydrant(HydrantID,Addr,longitude,latitude,Remark) VALUES('{0}','{1}','{2}','{3}','{4}')",
                id, addr, longitude, latitude, remark);

            SQLHelper.ExecuteNonQuery(SQL, null);
        }

        public void Update(string id,string addr,string longitude,string latitude,string remark)
        {
            string SQL = string.Format("UPDATE Hydrant set Addr='{0}',longitude='{1}',latitude='{2}',Remark='{3}' WHERE HydrantID='{4}'",
                addr, longitude, latitude, remark, id);

            SQLHelper.ExecuteNonQuery(SQL, null);
        }

        /// <summary>
        /// 消防栓是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool HydrantExist(string id)
        {
            string SQL = "SELECT COUNT(1) FROM Hydrant WHERE HydrantID='" + id + "'";

            object obj_count=SQLHelper.ExecuteScalar(SQL, null);
            if(obj_count!=null && obj_count!=DBNull.Value)
            {
                if (Convert.ToInt32(obj_count) > 0)
                    return true;
            }
            return false;
        }

        public void Delete(string id)
        {
            string SQL = "DELETE FROM Hydrant WHERE HydrantID='"+id.Trim()+"'";

            SQLHelper.ExecuteNonQuery(SQL, null);
        }

        public void UnAlarm(string id)
        {
            string SQL = "UPDATE Hydrant_Real SET IsAlarm=0 WHERE TerminalID="+id.Trim()+" AND CollTime=(SELECT MAX(colltime) AS colltime FROM Hydrant_Real WHERE TerminalID="+id.Trim()+")";

            SQLHelper.ExecuteNonQuery(SQL, null);
        }

        public void modifyCoordinate(string id, string longitude, string latitude)
        {
            string SQL = string.Format("UPDATE Hydrant SET longitude='{0}',latitude='{1}' WHERE HydrantID='{2}'", longitude, latitude, id.Trim());

            SQLHelper.ExecuteNonQuery(SQL, null);
        }

        public List<HydrantEntity> GetHydrantDetail(string HydrantID, int opt, DateTime minTime, DateTime maxTime, int interval)
        {
            string SQL = @"SELECT Operate,PressValue,OpenAngle,CollTime FROM Hydrant_Real 
  WHERE CollTime BETWEEN @mintime AND @maxtime AND DATEDIFF(minute,@mintime,CollTime) %@interval = 0 AND TerminalID=@TerId ";
            if (opt != -1)
                SQL += "AND Operate="+opt;
            SQL+="ORDER BY CollTime";

            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter("@TerId",SqlDbType.Int),
                new SqlParameter("@mintime",SqlDbType.DateTime),
                new SqlParameter("@maxtime",SqlDbType.DateTime),
                new SqlParameter("@interval",SqlDbType.Int)
            };

            parms[0].Value = HydrantID;
            parms[1].Value = minTime;
            parms[2].Value = maxTime;
            parms[3].Value = interval;

            using (SqlDataReader reader = SQLHelper.ExecuteReader(SQL, parms))
            {
                List<HydrantEntity> lstData = new List<HydrantEntity>();
                while (reader.Read())
                {
                    HydrantEntity entity = new HydrantEntity();
                    entity.HydrantID = HydrantID;
                    entity.OptType = (HydrantOptType)Convert.ToInt32(reader["Operate"]);
                    entity.PressValue = reader["PressValue"] != DBNull.Value ? Convert.ToSingle(reader["PressValue"]) : 0f;
                    entity.OpenAngle = reader["OpenAngle"] != DBNull.Value ? Convert.ToInt32(reader["OpenAngle"]) : 0;
                    entity.CollTime = reader["CollTime"] != DBNull.Value ? Convert.ToDateTime(reader["CollTime"]).ToString("yyyy-MM-dd HH:mm") : ConstValue.MinDateTime.ToString("yyyy-MM-dd HH:mm");

                    lstData.Add(entity);
                }
                return lstData;
            }
            return null;
        }
    }
}
