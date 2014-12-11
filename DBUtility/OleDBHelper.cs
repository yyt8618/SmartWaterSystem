using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.OleDb;
using System.Configuration;

namespace Utility
{
    /// <summary>
    /// 数据访问基础类(基于OleDb)
    /// 可以用户可以修改满足自己项目的需要。
    /// </summary>
    public abstract class OleDBHelper
    {
       
        private static string ConnStr = null;
        public static string conString
        {
            get
            {
                if (ConnStr == null)
                {
                    ConnStr = ConfigurationManager.AppSettings["AccessconString"];
                    //ConnStr = ConnStr.Replace("#webpath#", System.Web.HttpContext.Current.Server.MapPath("~/"));
                }
                return ConnStr;
            }
        }

        /// <summary>
        /// 执行增删改
        /// </summary>
        /// <param name="sql">有效的insert update delete语句</param>
        /// <param name="param">参数集合</param>
        /// <returns>影响的行数</returns>
        public static int ExecuteNonQuery(string sql, OleDbParameter[] param)
        {
            using (OleDbConnection con = new OleDbConnection(conString))
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand(sql, con);
                if (param != null)
                    cmd.Parameters.AddRange(param);
                int i = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                con.Close();
                return i;
            }
        }

        /// <summary>
        /// 执行查询 返回DataTable
        /// </summary>
        /// <param name="sql">有效的select语句</param>
        /// <returns>返回DataTable</returns>
        public static DataTable ExecuteDataQuery(string sql)
        {
            using (OleDbConnection con = new OleDbConnection(conString))
            {
                con.Open();
                OleDbDataAdapter adap = new OleDbDataAdapter(sql, con);
                DataTable tbl = new DataTable();
                adap.Fill(tbl);
                con.Close();
                return tbl;
            }
        }

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="sql">有效的select语句</param>
        /// <returns>返回OleDbDataReader</returns>
        public static OleDbDataReader ExecuteReader(string sql)
        {
            OleDbConnection con = new OleDbConnection(conString);
            con.Open();
            OleDbCommand cmd = new OleDbCommand(sql, con);
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="sql">有效的select语句</param>
        /// <param name="param">参数集合</param>
        /// <returns>返回OleDbDataReader</returns>
        public static OleDbDataReader ExecuteReader(string sql, OleDbParameter[] param)
        {
            OleDbConnection con = new OleDbConnection(conString);
            con.Open();
            OleDbCommand cmd = new OleDbCommand(sql, con);
            cmd.Parameters.AddRange(param);
            OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return reader;
        }

        /// <summary>
        /// 执行查询 返回结果集中第一行第一列的值，忽略其他行列。
        /// </summary>
        /// <param name="sql">有效的select语句</param>
        /// <returns>返回结果集中第一行第一列的值，忽略其他行列。</returns>
        public static object ExecuteScalar(string sql)
        {
            using (OleDbConnection con = new OleDbConnection(conString))
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand(sql, con);
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的OleDbParameter[]）</param>
        public static void ExecuteSqlTran(Hashtable SQLStringList)
        {
            using (OleDbConnection conn = new OleDbConnection(conString))
            {
                conn.Open();
                using (OleDbTransaction trans = conn.BeginTransaction())
                {
                    OleDbCommand cmd = new OleDbCommand();
                    try
                    {
                        //循环
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            OleDbParameter[] cmdParms = (OleDbParameter[])myDE.Value;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                            trans.Commit();
                        }
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        
        private static void PrepareCommand(OleDbCommand cmd, OleDbConnection conn, OleDbTransaction trans, string cmdText, OleDbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (OleDbParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

    }
}