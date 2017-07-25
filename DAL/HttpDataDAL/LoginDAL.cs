using Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class LoginDAL
    {
        public bool Login(string userid,string pwd)
        {
            string SQL = "SELECT COUNT(1) FROM EN_User WHERE EN_User_ID='"+userid+"' AND EN_User_Password='"+pwd+"'";
            object obj_count = SQLHelper.ExecuteScalar(SQL, null);
            if(obj_count!=DBNull.Value && obj_count!=null)
            {
                return Convert.ToInt32(obj_count) > 0 ? true : false;
            }

            return false;
        }
    }
}
