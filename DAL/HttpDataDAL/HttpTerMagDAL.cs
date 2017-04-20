using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class HttpTerMagDAL
    {
        public List<TerMagInfoEntity> GetAllTer()
        {
            List<TerMagInfoEntity> lstTerInfo = null;
            string SQL = "SELECT * FROM [TerManagerInfo]";
            using (SqlDataReader reader = SQLHelper.ExecuteReader(SQL, null))
            {
                lstTerInfo = new List<TerMagInfoEntity>();
                while(reader.Read())
                {
                    TerMagInfoEntity terinfo = new TerMagInfoEntity();
                    terinfo.Id = Convert.ToInt32(reader["Id"]);
                    terinfo.DevId = Convert.ToInt32(reader["TerId"]);
                    terinfo.DevType = (ConstValue.DEV_TYPE)Convert.ToInt32(reader["TerType"]);
                    terinfo.Addr = reader["Addr"] != DBNull.Value ? reader["Addr"].ToString().Trim() : "";
                    terinfo.Remark = reader["Remark"] != DBNull.Value ? reader["Remark"].ToString().Trim() : "";
                    terinfo.Lng = Convert.ToDouble(reader["longitude"]);
                    terinfo.Lat = Convert.ToDouble(reader["latitude"]);
                    terinfo.PicId = new List<string>();
                    lstTerInfo.Add(terinfo);
                }
            }
            return lstTerInfo;
        }

        public void DelTer(int Id)
        {
            //需要删除图片
            string SQL = "DELETE FROM [TerManagerInfo] WHERE Id='"+Id+"'";
            SQLHelper.ExecuteNonQuery(SQL, null);
        }


    }
}
