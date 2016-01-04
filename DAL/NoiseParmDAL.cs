using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace DAL
{
    public class NoiseParmDAL
    {
        public string GetParm(string key)
        {
            string SQL = "SELECT [Value] FROM Noise_Parm WHERE Parm='" + key + "'";

            object obj=SQLHelper.ExecuteScalar(SQL, null);
            if (obj != null && obj != DBNull.Value)
            {
                return obj.ToString().Trim();
            }
            return "";
        }

        public void SetParm(string key, string value)
        {
            string SQL_Exist = "SELECT COUNT(1) FROM Noise_Parm WHERE Parm='" + key + "'";

            object obj_exist = SQLHelper.ExecuteScalar(SQL_Exist, null);
            bool exist = false;
            if (obj_exist != null && obj_exist != DBNull.Value)
            {
                if (Convert.ToInt32(obj_exist) > 0)
                    exist = true;
            }

            string SQL = "";
            if (exist)
                SQL = "UPDATE Noise_Parm SET [Value]='" + value + "' WHERE Parm='" + key + "'";
            else
                SQL = "INSERT INTO Noise_Parm (Parm,Value) VALUES('" + key + "','" + value + "')";

            SQLHelper.ExecuteNonQuery(SQL, null);
        }
    }
}
