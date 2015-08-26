using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Entity;
using System.Data.SQLite;

namespace DAL
{
    public class HydrantDAL
    {
        public List<HydrantEntity> SelectAll()
        {
            List<HydrantEntity> lstHydrant = null;
            string SQL = "SELECT DISTINCT HydrantID,Addr,longitude,latitude FROM Hydrant";
            using (SQLiteDataReader reader=SQLiteHelper.ExecuteReader(SQL, null))
            {
                lstHydrant = new List<HydrantEntity>();
                while (reader.Read())
                {
                    HydrantEntity entity = new HydrantEntity();
                    entity.HydrantID = reader["HydrantID"].ToString();
                    entity.Addr = reader["Addr"].ToString();
                    entity.Longtitude = reader["longitude"].ToString();
                    entity.Latitude = reader["latitude"].ToString();
                    lstHydrant.Add(entity);
                }
            }
            return lstHydrant;
        }
        public void Insert(string id, string addr, string longitude, string latitude)
        {
            string SQL = string.Format("INSERT INTO Hydrant(HydrantID,Addr,longitude,latitude) VALUES('{0}','{1}','{2}','{3}')",
                id,addr,longitude,latitude);

            SQLiteHelper.ExecuteNonQuery(SQL,null);
        }

        public void Delete(string id)
        {
            string SQL = "DELETE FROM Hydrant WHERE HydrantID='"+id.Trim()+"'";

            SQLiteHelper.ExecuteNonQuery(SQL, null);
        }

        public void modifyCoordinate(string id, string longitude, string latitude)
        {
            string SQL = string.Format("UPDATE Hydrant SET longitude='{0}',latitude='{1}' WHERE HydrantID='{2}'", longitude, latitude, id.Trim());

            SQLiteHelper.ExecuteNonQuery(SQL, null);
        }
    }
}
