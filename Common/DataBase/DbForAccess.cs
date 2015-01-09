using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace NoiseAnalysisSystem
{
    /// <summary>
    /// 提供对Access数据库的访问
    /// </summary>
    public class DbForAccess
    {
        protected static OleDbConnection conn = new OleDbConnection();
        protected static OleDbCommand comm = new OleDbCommand();

        /// <summary>
        /// 打开数据库
        /// </summary>
        public static void OpenConnection()
        {
            if (conn.State == ConnectionState.Closed)
            {
                string dataSource = string.Empty;

                dataSource = Application.StartupPath + @"\DB\Noise.mdb"; 

                if (File.Exists(dataSource))
                {
                    conn.ConnectionString = @"Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + dataSource;//app.config文件里设定。
                    comm.Connection = conn;
                }
                else
                {
                    AppConfigHelper.GetAppSettingValue("DataSoure");
                    conn.ConnectionString = @"Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + dataSource;//app.config文件里设定。
                    comm.Connection = conn;
                }
                try
                {
                    conn.Open();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

        }

        /// <summary>
        /// 关闭数据库
        /// </summary>
        public static void CloseConnection()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                conn.Dispose();
                comm.Dispose();
            }
        }

        /// <summary>
        /// 执行sql语句，返回受影响的行数
        /// </summary>
        /// <param name="sqlstr">有效的Sql语句</param>
        public static int ExcuteSql(string sqlstr)
        {
            try
            {
                OpenConnection();
                comm.CommandType = CommandType.Text;
                comm.CommandText = sqlstr;
                return comm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            //finally
            //{ 
            //    CloseConnection(); 
            //}
        }

        /// <summary>
        /// 返回指定sql语句的第一行第一列结果
        /// </summary>
        /// <param name="sqlstr">有效的Sql语句</param>
        public static object GetFirstResult(string sqlstr)
        {
            object dr = null;
            try
            {
                OpenConnection();
                comm.CommandText = sqlstr;
                comm.CommandType = CommandType.Text;

                dr = comm.ExecuteScalar();
            }
            catch
            {
                try
                {
                    CloseConnection();
                }
                catch { }
            }
            return dr;
        }

        /// <summary>
        /// 返回指定sql语句的OleDbDataReader对象，使用时请注意关闭这个对象
        /// </summary>
        /// <param name="sqlstr">有效的Sql语句</param>
        public static OleDbDataReader GetDataReader(string sqlstr)
        {
            OleDbDataReader dr = null;
            try
            {
                OpenConnection();
                comm.CommandText = sqlstr;
                comm.CommandType = CommandType.Text;

                dr = comm.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                try
                {
                    dr.Close();
                    CloseConnection();
                }
                catch { }
            }
            return dr;
        }

        /// <summary>
        /// 返回指定sql语句的DataSet对象
        /// </summary>
        /// <param name="sqlstr">有效的Sql语句</param>
        public static DataSet GetDataSet(string sqlstr)
        {
            DataSet ds = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter();
            try
            {
                OpenConnection();
                comm.CommandType = CommandType.Text;
                comm.CommandText = sqlstr;
                da.SelectCommand = comm;
                da.Fill(ds);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            //finally
            //{
            //    CloseConnection();
            //}
            return ds;
        }

        /// <summary>
        /// 返回指定sql语句的DataTable对象
        /// </summary>
        /// <param name="sqlstr">有效的Sql语句</param>
        public static DataTable GetDataTable(string sqlstr)
        {
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter();
            try
            {
                OpenConnection();
                comm.CommandType = CommandType.Text;
                comm.CommandText = sqlstr;
                da.SelectCommand = comm;
                da.Fill(dt);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            //finally
            //{
            //    CloseConnection();
            //}
            return dt;
        }

        /// <summary>
        /// 返回指定sql语句的DataView对象
        /// </summary>
        /// <param name="sqlstr">有效的Sql语句</param>
        public static DataView GetDataView(string sqlstr)
        {
            OleDbDataAdapter da = new OleDbDataAdapter();
            DataView dv = new DataView();
            DataSet ds = new DataSet();
            try
            {
                OpenConnection();
                comm.CommandType = CommandType.Text;
                comm.CommandText = sqlstr;
                da.SelectCommand = comm;
                da.Fill(ds);
                dv = ds.Tables[0].DefaultView;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            //finally
            //{
            //    CloseConnection();
            //}
            return dv;
        }

    }
}
