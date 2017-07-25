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
        public long Login(string username, string pwd)
        {
            string SQL = "SELECT EN_User_ID FROM EN_User WHERE EN_User_Name='" + username + "' AND EN_User_Password='"+pwd+"'";
            object obj_id = SQLHelper.ExecuteScalar(SQL, null);
            if(obj_id!=DBNull.Value && obj_id!=null)
            {
                return Convert.ToInt64(obj_id);
            }

            return -1;
        }
    }
}
