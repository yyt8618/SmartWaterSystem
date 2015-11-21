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
            string SQL = @"SELECT h.HydrantID,h.Addr,h.longitude,h.latitude,d.Operate,d.PressValue,d.OpenAngle,d.IsAlarm
                        FROM Hydrant h LEFT JOIN 
                                 (SELECT a.TerminalID,a.Operate,a.PressValue,a.OpenAngle,a.IsAlarm FROM [Hydrant_Real] a,(SELECT TerminalID,MAX(colltime) 
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
                    entity.OptType = reader["Operate"] != DBNull.Value ? (HydrantOptType)Convert.ToInt32(reader["Operate"]) : HydrantOptType.Close;
                    entity.PressValue = reader["PressValue"] != DBNull.Value ? Convert.ToSingle(reader["PressValue"]) : 0f;
                    entity.OpenAngle = reader["OpenAngle"] != DBNull.Value ? Convert.ToInt32(reader["OpenAngle"]) : 0;
                    entity.IsAlarm = reader["IsAlarm"] != DBNull.Value ? (Convert.ToInt32(reader["IsAlarm"]) == 1 ? true : false) : false;
                    lstHydrant.Add(entity);
                }
            }
            return lstHydrant;
        }
        public void Insert(string id, string addr, string longitude, string latitude)
        {
            string SQL = string.Format("INSERT INTO Hydrant(HydrantID,Addr,longitude,latitude) VALUES('{0}','{1}','{2}','{3}')",
                id,addr,longitude,latitude);

            SQLHelper.ExecuteNonQuery(SQL, null);
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
                    entity.CollTime = reader["CollTime"] != DBNull.Value ? Convert.ToDateTime(reader["CollTime"]) : ConstValue.MinDateTime;

                    lstData.Add(entity);
                }
                return lstData;
            }
            return null;
        }
    }
}
