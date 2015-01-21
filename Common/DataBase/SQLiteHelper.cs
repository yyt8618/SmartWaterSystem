using System;
using System.IO;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Collections;
using System.Data;

namespace NoiseAnalysisSystem
{
    public class SQLiteHelper
    {
        private static NLog.Logger logger = NLog.LogManager.GetLogger("SQLiteHelper");

        public static readonly string DatabaseLocation = Application.StartupPath + "\\DataBase";

        public static readonly string FileName = "local.db";

        public static string ConnectionString
        {
            get
            {
                SQLiteConnectionStringBuilder connstr = new SQLiteConnectionStringBuilder();
                connstr.DataSource = Path.Combine(DatabaseLocation, FileName);
                //connstr.Password = "gcc123";

                return connstr.ToString();
            }
        }
        //= Path.Combine(DatabaseLocation, FileName);

        public static SQLiteConnection m_conn = null;

        private static object m_lockObject = new object();

        private static SQLiteCommand cmd = null;

        public static object LockObject
        {
            get { return m_lockObject; }
        }

        public static SQLiteConnection Conn
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
        public static int ExecuteNonQuery(string cmdText, params SQLiteParameter[] commandParameters)
        {
            lock (LockObject)
            {
                if (cmd == null)
                {
                    cmd = new SQLiteCommand();
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
        public static int ExecuteNonQuery(SQLiteConnection connection, string cmdText, params SQLiteParameter[] commandParameters)
        {
            lock (LockObject)
            {
                if (cmd == null)
                {
                    cmd = new SQLiteCommand();
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
        public static int ExecuteNonQuery(SQLiteTransaction trans, string cmdText, params SQLiteParameter[] commandParameters)
        {
            lock (LockObject)
            {
                if (cmd == null)
                {
                    cmd = new SQLiteCommand();
                }
                PrepareCommand(cmd, (SQLiteConnection)trans.Connection, trans, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }
        #endregion

        public static SQLiteDataReader ExecuteReader(string cmdText, params SQLiteParameter[] commandParameters)
        {
            lock (LockObject)
            {
                if (cmd == null)
                {
                    cmd = new SQLiteCommand();
                }
                PrepareCommand(cmd, null, null, cmdText, commandParameters);
                SQLiteDataReader rdr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return rdr;
            }
        }

        public static object ExecuteScalar(string cmdText, params SQLiteParameter[] commandParameters)
        {
            lock (LockObject)
            {
                if (cmd == null)
                {
                    cmd = new SQLiteCommand();
                }
                PrepareCommand(cmd, null, null, cmdText, commandParameters);
                object o = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return o;
            }
        }

        public static DataTable ExecuteDataTable(string cmdText, params SQLiteParameter[] commandParameters)
        {
            lock (LockObject)
            {
                if (cmd == null)
                {
                    cmd = new SQLiteCommand();
                }
                PrepareCommand(cmd, null, null, cmdText, commandParameters);
                SQLiteDataAdapter adp = new SQLiteDataAdapter();
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
        /// <param name="cmd">SQLiteCommand object</param>
        /// <param name="conn">SQLiteConnection object</param>
        /// <param name="trans">SqlCeTransaction object</param>
        /// <param name="cmdText">Command text, e.g. Select * from Products</param>
        /// <param name="cmdParms">SqlCeParameters to use in the command</param>
        private static void PrepareCommand(SQLiteCommand cmd, SQLiteConnection conn, SQLiteTransaction trans, string cmdText, SQLiteParameter[] cmdParms)
        {
            if (conn != null)
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
            }
            else
            {
                //if (!(cmd.Connection != null && cmd.Connection.State == ConnectionState.Open))
                //{
                OpenConnection();
                cmd.Connection = m_conn;
                //}
            }

            if (cmdParms != null)
            {
                cmd.Parameters.AddRange(cmdParms);
            }

            if (!string.IsNullOrEmpty(cmd.CommandText))
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
            if (m_conn == null)
            {
                m_conn = new SQLiteConnection(ConnectionString);
            }

            if (m_conn.State != ConnectionState.Open)
            {
                logger.Debug("OpenConnection()");
                m_conn.Open();
            }
        }

        public static void CloseConnection()
        {
            if (m_conn != null)
            {
                logger.Debug("OpenConnection() [close]");

                m_conn.Close();
                //m_conn.Dispose();
                //m_conn = null;
            }
            if (cmd != null)
            {
                cmd.Dispose();
                cmd = new SQLiteCommand();
                //cmd = null;
            }
        }

        public static SQLiteTransaction GetTransaction()
        {
            OpenConnection();
            logger.Debug("GetTransaction() [begin]");
            return m_conn.BeginTransaction();
        }

        #region CacheParameters
        /// <summary>
        /// add parameter array to the cache
        /// </summary>
        /// <param name="cacheKey">Key to the parameter cache</param>
        /// <param name="cmdParms">an array of SqlParamters to be cached</param>
        public static void CacheParameters(string cacheKey, params SQLiteParameter[] commandParameters)
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
        public static SQLiteParameter[] GetCachedParameters(string cacheKey)
        {
            SQLiteParameter[] cachedParms = (SQLiteParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            SQLiteParameter[] clonedParms = new SQLiteParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (SQLiteParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }
        #endregion
    }
}
