using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace Common
{
    public class SQLHelper
    {
        static NLog.Logger logger = NLog.LogManager.GetLogger("SQLHelper");
        
        public static string ConnectionString = "";

        public static SqlConnection m_conn = null;

        private static object m_lockObject = new object();

        private static SqlCommand cmd = null;

        public static object LockObject 
        {
            get { return m_lockObject; }
        }

        public static SqlConnection Conn
        {
            get
            {
                OpenConnection();
                return m_conn;
            }
        }

        // Hashtable to store cached parameters
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        #region ExecuteNonQuery
        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(string cmdText, params SqlParameter[] commandParameters)
        {
            lock (LockObject)
            {
                if (cmd == null)
                {
                    cmd = new SqlCommand();
                }

                PrepareCommand(cmd, null, null, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against an existing database connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(SqlConnection connection, string cmdText, params SqlParameter[] commandParameters)
        {
            lock (LockObject)
            {
                if (cmd == null)
                {
                    cmd = new SqlCommand();
                }
                PrepareCommand(cmd, connection, null, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) using an existing SQL Transaction 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="trans">an existing sql transaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(SqlTransaction trans, string cmdText, params SqlParameter[] commandParameters)
        {
            lock (LockObject)
            {
                if (cmd == null)
                {
                    cmd = new SqlCommand();
                }
                PrepareCommand(cmd, (SqlConnection)trans.Connection, trans, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }
        #endregion

        public static SqlDataReader ExecuteReader(string cmdText, params SqlParameter[] commandParameters)
        {
            lock (LockObject)
            {
                if (cmd == null)
                {
                    cmd = new SqlCommand();
                }
                PrepareCommand(cmd, null, null, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return rdr;
            }
        }

        public static object ExecuteScalar(string cmdText, params SqlParameter[] commandParameters)
        {
            lock (LockObject)
            {
                if (cmd == null)
                {
                    cmd = new SqlCommand();
                }
                PrepareCommand(cmd, null, null, cmdText, commandParameters);
                object o = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return o;
            }
        }

        public static DataTable ExecuteDataTable(string cmdText, params SqlParameter[] commandParameters)
        {
            lock (LockObject)
            {
                if (cmd == null)
                {
                    cmd = new SqlCommand();
                }
                PrepareCommand(cmd, null, null, cmdText, commandParameters);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataTable tb = new DataTable();
                adp.Fill(tb);
                cmd.Parameters.Clear();
                return tb;
            }
        }
       
        #region PrepareCommand
        /// <summary>
        /// Prepare a command for execution
        /// </summary>
        /// <param name="cmd">SqlCommand object</param>
        /// <param name="conn">SqlConnection object</param>
        /// <param name="trans">SqlTransaction object</param>
        /// <param name="cmdText">Command text, e.g. Select * from Products</param>
        /// <param name="cmdParms">SqlParameters to use in the command</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn != null && !string.IsNullOrEmpty(conn.ConnectionString))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
            }
            else 
            {
                OpenConnection();
                cmd.Connection = m_conn;
            }

            if (cmdParms != null)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddRange(cmdParms);
            }

            if (cmd.CommandText.Equals(cmdText))
            {
                return;
            }

            cmd.CommandText = cmdText;
            //logger.Debug("PrepareCommand() CommandText " + cmdText);

            if (trans != null) 
            {
                //logger.Debug("PrepareCommand() Transaction");
                cmd.Transaction = trans;
            }
        }
        #endregion

        public static void OpenConnection()
        {
            if (m_conn == null || string.IsNullOrEmpty(m_conn.ConnectionString)) 
            {
                m_conn = new SqlConnection(ConnectionString);
            }

            if (m_conn.State != ConnectionState.Open) 
            {
                m_conn.Open();
            }
        }

        public static void CloseConnection() 
        {
            if (m_conn != null)
            {
                m_conn.Close();
            }
                
        }

        public static SqlTransaction GetTransaction() 
        {
            OpenConnection();
            return m_conn.BeginTransaction();
        }

        #region CacheParameters
        /// <summary>
        /// add parameter array to the cache
        /// </summary>
        /// <param name="cacheKey">Key to the parameter cache</param>
        /// <param name="cmdParms">an array of SqlParamters to be cached</param>
        public static void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }
        #endregion

        #region GetCachedParameters
        /// <summary>
        /// Retrieve cached parameters
        /// </summary>
        /// <param name="cacheKey">key used to lookup parameters</param>
        /// <returns>Cached SqlParamters array</returns>
        public static SqlParameter[] GetCachedParameters(string cacheKey)
        {
            SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }
        #endregion

        public static bool TryConn(string conn)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(conn);
                connection.Open();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
    }
}
