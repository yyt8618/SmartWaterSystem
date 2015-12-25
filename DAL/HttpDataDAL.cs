using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;
using Common;
using System.Data.SqlClient;

namespace DAL
{
    public class HttpDataDAL
    {
        public void GetGroupsInfo(out List<GroupsData> lstgroup,out List<TersData> lstter)
        {
            lstgroup = null;
            lstter = null;
            //获得组信息
            string SQL_grp = "SELECT GroupId,Name,Remark FROM EN_Group";
            using (SqlDataReader reader_grp = SQLHelper.ExecuteReader(SQL_grp, null))
            {
                lstgroup = new List<GroupsData>();
                while (reader_grp.Read())
                {
                    GroupsData grp = new GroupsData();
                    grp.groupid = reader_grp["GroupId"].ToString();
                    grp.groupname = reader_grp["Name"].ToString();
                    grp.remark = reader_grp["Remark"].ToString();

                    lstgroup.Add(grp);
                }
            }

            bool addungroup = false; //是否添加一个空分组
            string SQL_ter = "SELECT relate.GroupId,rec.RecorderId,rec.Remark,rec.GroupState FROM EN_NoiseRecorder as rec LEFT JOIN MP_GroupRecorder as relate ON rec.RecorderId = relate.RecorderId";
            using (SqlDataReader reader_ter = SQLHelper.ExecuteReader(SQL_ter, null))
            {
                lstter = new List<TersData>();
                while(reader_ter.Read())
                {
                    TersData ter = new TersData();
                    ter.groupid = reader_ter["GroupId"] != DBNull.Value ? reader_ter["GroupId"].ToString() : "";
                    ter.terid = reader_ter["RecorderId"] != DBNull.Value ? reader_ter["RecorderId"].ToString() : "";
                    ter.remark = reader_ter["Remark"] != DBNull.Value ? reader_ter["Remark"].ToString() : "";
                    ter.groupstate = reader_ter["GroupState"] != DBNull.Value ? Convert.ToInt32(reader_ter["GroupState"]) : 0;

                    lstter.Add(ter);

                    if (ter.groupid == "")
                        addungroup = true;
                }
            }

            if (addungroup)
            {
                GroupsData grp = new GroupsData();
                lstgroup.Add(grp);
            }
        }
    }
}
