using Common;
using Entity;
using SmartWaterSystem;
using System;
using System.Collections.Generic;
using System.Data;
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
            string SQL = "select info.*,pic.PicName from TerManagerInfo info left join TerMagPic pic on info.Id = pic.TerMagId and Usefor = 1";
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

        /// <summary>
        /// 查询终端信息
        /// </summary>
        public TerMagInfoEntity QueryTerMagInfo(ConstValue.DEV_TYPE devtype,int DevId,string netpathhead ="")
        {
            string SQL = string.Format("select info.*,pic.PicName from TerManagerInfo info left join TerMagPic pic on info.Id = pic.TerMagId and Usefor = 1 where info.TerType='{0}' AND info.TerId='{1}'", (int)devtype, DevId);
            using (SqlDataReader reader = SQLHelper.ExecuteReader(SQL, null))
            {
                TerMagInfoEntity terinfo = null;
                while (reader.Read())
                {
                    if(terinfo == null)
                    {
                        /* 如果不存在就新增TerMagInfoEntity，如果存在就添加图片 */
                        terinfo = new TerMagInfoEntity();
                        terinfo.Id = Convert.ToInt32(reader["id"]);
                        terinfo.DevId = Convert.ToInt32(reader["TerId"]);
                        terinfo.DevType = (ConstValue.DEV_TYPE)Convert.ToInt32(reader["TerType"]);
                        terinfo.Addr = reader["Addr"] != DBNull.Value ? reader["Addr"].ToString().Trim() : "";
                        terinfo.Remark = reader["Remark"] != DBNull.Value ? reader["Remark"].ToString().Trim() : "";
                        terinfo.Lng = Convert.ToDouble(reader["longitude"]);
                        terinfo.Lat = Convert.ToDouble(reader["latitude"]);
                        terinfo.PicId = new List<string>();
                        terinfo.PicId.Add(GetNetaddrByName(netpathhead, reader["PicName"].ToString().Trim()));
                    }
                    else
                    {
                        terinfo.PicId.Add(GetNetaddrByName(netpathhead, reader["PicName"].ToString().Trim()));
                    }
                }
                    
            }
            return null; 
        }

        public bool CheckPicExist(List<string> picIds)
        {
            string str_pic = "";
            foreach(string picid in picIds)
            {
                str_pic += "'" + picid + "',";
            }
            str_pic = str_pic.Substring(0, str_pic.Length - 1);
            string SQL = "SELECT COUNT(1) FROM TerMagPic WHERE PicName IN (" + str_pic + ")";
            object obj_exist = SQLHelper.ExecuteScalar(SQL, null);
            if(obj_exist!=null && obj_exist!=DBNull.Value)
            {
                return Convert.ToInt32(obj_exist) > 0 ? true : false;
            }
            return false;
        }

        public  bool InsertTerMagInfo(TerMagInfoEntity terinfo,string localtmppath, string localhead)
        {
            SqlTransaction trans = null;
            try
            {
                SqlParameter[] parmsinfo = new SqlParameter[]
                {
                    new SqlParameter("@terid",SqlDbType.Int),
                    new SqlParameter("@tertype",SqlDbType.SmallInt),
                    new SqlParameter("@addr",SqlDbType.NVarChar,200),
                    new SqlParameter("@remark",SqlDbType.NVarChar,300),
                    new SqlParameter("@lng",SqlDbType.Float),

                    new SqlParameter("@lat",SqlDbType.Float),
                    new SqlParameter("@modifytime",SqlDbType.DateTime),
                    new SqlParameter("@identify",SqlDbType.Int)
                };
                parmsinfo[7].Direction = ParameterDirection.Output;
                parmsinfo[0].Value = terinfo.DevId;
                parmsinfo[1].Value = terinfo.DevType;
                parmsinfo[2].Value = terinfo.Addr;
                parmsinfo[3].Value = terinfo.Remark;
                parmsinfo[4].Value = terinfo.Lng;
                parmsinfo[5].Value = terinfo.Lat;
                parmsinfo[6].Value = DateTime.Now;

                SqlConnection conn = SQLHelper.Conn;
                SQLHelper.OpenConnection();
                trans = conn.BeginTransaction();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandText = "INSERT TerManagerInfo(TerId,TerType,Addr,Remark,longitude,latitude,ModifyTime) VALUES(@terid,@tertype,@addr,@remark,@lng,@lat,@modifytime);select @identify=SCOPE_IDENTITY()";
                command.Parameters.Clear();
                command.Parameters.AddRange(parmsinfo);
                command.ExecuteNonQuery();
                long id = Convert.ToInt64(parmsinfo[7].Value);

                SqlParameter[] parmspic = new SqlParameter[]
                {
                    new SqlParameter("@magid",SqlDbType.Int),
                    new SqlParameter("@picname",SqlDbType.NVarChar,40),
                    new SqlParameter("modifytime",SqlDbType.DateTime)
                };
                parmspic[0].Value = id;
                command.CommandText = "ISNERT TerMagPic(TerMagId,PicName,Userfor,ModifyTime) VALUES(@magid,@picname,1,@modiftytime";
                command.Parameters.Clear();
                command.Parameters.AddRange(parmspic);

                foreach (string picname in terinfo.PicId)
                {
                    parmspic[1].Value = picname;
                    parmspic[2].Value = DateTime.Now;

                    command.ExecuteNonQuery();
                }

                PicHelper pichelper = new PicHelper();
                foreach(string picname in terinfo.PicId)
                {
                    pichelper.MoveFile(Path.Combine(localtmppath, picname), Path.Combine(localhead, picname));
                }

                trans.Commit();
                return true;
            }
            catch(Exception ex)
            {
                if(trans!=null)
                {
                    trans.Rollback();
                }
                throw ex;
            }
        }
        

    }
}
