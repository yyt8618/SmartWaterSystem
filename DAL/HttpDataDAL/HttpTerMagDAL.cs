using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL
{
    public class HttpTerMagDAL
    {
        public List<TerMagInfoEntity> GetAllTer(string netpathhead)
        {
            List<TerMagInfoEntity> lstTerInfo = null;
            string SQL = "select info.*,pic.PicName from TerManagerInfo info left join TerMagPic pic on info.TerId = pic.TerMagId and info.TerType = pic.TerType and Usefor = 1";
            using (SqlDataReader reader = SQLHelper.ExecuteReader(SQL, null))
            {
                lstTerInfo = new List<TerMagInfoEntity>();
                while(reader.Read())
                {
                    bool isExist = false;
                    int index = 0;
                    int id = Convert.ToInt32(reader["Id"]); 
                    for (index = 0; index < lstTerInfo.Count; index++)
                    {
                        if (lstTerInfo[index].Id == id)
                        {
                            isExist = true;
                            break;
                        }
                    }
                    /* 如果不存在就新增TerMagInfoEntity，如果存在就添加图片 */
                    if (!isExist)
                    {
                        TerMagInfoEntity terinfo = new TerMagInfoEntity();
                        terinfo.Id = id;
                        terinfo.DevId = Convert.ToInt32(reader["TerId"]);
                        terinfo.DevType = (ConstValue.DEV_TYPE)Convert.ToInt32(reader["TerType"]);
                        terinfo.Addr = reader["Addr"] != DBNull.Value ? reader["Addr"].ToString().Trim() : "";
                        terinfo.Remark = reader["Remark"] != DBNull.Value ? reader["Remark"].ToString().Trim() : "";
                        terinfo.Lng = Convert.ToDouble(reader["longitude"]);
                        terinfo.Lat = Convert.ToDouble(reader["latitude"]);
                        terinfo.PicId = new List<string>();
                        terinfo.PicId.Add(GetNetaddrByName(netpathhead,reader["PicName"].ToString().Trim()));
                        lstTerInfo.Add(terinfo);
                    }
                    else
                    {
                        lstTerInfo[index].PicId.Add(GetNetaddrByName(netpathhead, reader["PicName"].ToString().Trim()));
                    }
                }
                
            }
            return lstTerInfo;
        }

        /// <summary>
        /// 通过文件名，获取网络地址
        /// </summary>
        /// <param name="netpathhead">网络地址头</param>
        /// <param name="picname">文件名</param>
        /// <returns></returns>
        string GetNetaddrByName(string netpathhead,string picname)
        {
            return Path.Combine(netpathhead, picname);
        }

        public void DelTer(int Id)
        {
            //需要删除图片
            string SQL = "DELETE FROM [TerManagerInfo] WHERE Id='"+Id+"'";
            SQLHelper.ExecuteNonQuery(SQL, null);
        }


    }
}
