using Common;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class RectifyValueDAL
    {
        /// <summary>
        /// 获取全部的偏移值
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllRectifyValue()
        {
            string SQL = "SELECT [TerminalID],[TerminalType],[FunCode],[OffsetValue],[ModifyTime] FROM RectifyValue Order by TerminalID";
            return SQLHelper.ExecuteDataTable(SQL, null);
        }

        /// <summary>
        /// 保存所有的偏移量
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool SaveRectifyValue(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return true;

            DelAllValue();

            string SQL = "INSERT INTO RectifyValue(TerminalID,TerminalType,Funcode,OffsetValue,ModifyTime) VALUES(@id,@type,@funcode,@offsetvalue,@modifytime)";
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@id",SqlDbType.Int),
                new SqlParameter("@type",SqlDbType.Int),
                new SqlParameter("@funcode",SqlDbType.Int),
                new SqlParameter("@offsetvalue",SqlDbType.Float),
                new SqlParameter("@modifytime",SqlDbType.DateTime)
            };
            foreach(DataRow dr in dt.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    parms[0].Value = dr["TerminalID"];
                    parms[1].Value = dr["TerminalType"];
                    parms[2].Value = dr["Funcode"];
                    parms[3].Value = dr["OffsetValue"];
                    parms[4].Value = DateTime.Now;

                    SQLHelper.ExecuteNonQuery(SQL, parms);
                }
            }
            return true;
        }

        /// <summary>
        /// 删除所有的偏移值
        /// </summary>
        /// <returns></returns>
        private bool DelAllValue()
        {
            string SQL = "DELETE FROM RectifyValue";
            SQLHelper.ExecuteNonQuery(SQL, null);
            return true;
        }

    }
}
