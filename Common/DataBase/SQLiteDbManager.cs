using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SQLite;
using System.Data;

namespace Common
{
    public class SQLiteDbManager
    {
        private NLog.Logger logger = NLog.LogManager.GetLogger("DatabaseManager");
        
        
        #region 最新数据库版本
        private string m_lastestDBVersion = "V1.0";
        /// <summary>
        /// 最新数据库版本
        /// </summary>
        public string LastestDBVersion
        {
            get { return m_lastestDBVersion; }
        }
        #endregion

        #region 数据库是否存在
        /// <summary>
        /// 数据库是否存在
        /// </summary>
        public bool Exists
        {
            get 
            {
                return File.Exists(Path.Combine(SQLiteHelper.DatabaseLocation, SQLiteHelper.FileName)); 
            }
        }
        #endregion

        #region 创建数据库文件
        /// <summary>
        /// 创建数据库文件
        /// </summary>
        /// <returns></returns>
        public bool CreateDatabase()
        {
            try
            {
                // first, delete existing database
                SQLiteHelper.CloseConnection();
                DeleteDatabase();

                // create new database
                if (!Directory.Exists(SQLiteHelper.DatabaseLocation))
                {
                    Directory.CreateDirectory(SQLiteHelper.DatabaseLocation);
                }

                SQLiteConnection.CreateFile(Path.Combine(SQLiteHelper.DatabaseLocation,SQLiteHelper.FileName));

                return true;
            }
            catch (System.Exception ex)
            {
                logger.ErrorException("CreateDatabase()", ex);
                return false;
            }
        }
        #endregion

        #region 删除已经存在的数据库
        /// <summary>
        /// 删除已经存在的数据库
        /// </summary>
        /// <returns></returns>
        public void DeleteDatabase()
        {
            if (new SQLiteDbManager().Exists)
            {
                SQLiteHelper.CloseConnection();
                File.Delete(SQLiteHelper.FileName);
            }
        }
        #endregion

        #region 创建数据表 & 更新数据表 & 创建索引
        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <returns></returns>
        public bool CreateDataTable()
        {
            SQLiteTransaction trans = null;
            try
            {
                List<string> cmdTextArray = this.GetCreateTable();
                if (cmdTextArray == null || cmdTextArray.Count <= 0)
                {
                    return false;
                }
                
                trans = SQLiteHelper.GetTransaction();

                foreach (string s in cmdTextArray)
                {
                    SQLiteHelper.ExecuteNonQuery(trans, s, null);
                }
                trans.Commit();
                return true;
            }
            catch (SQLiteException ex)
            {
                if (trans!=null)
                {
                    trans.Rollback();
                }
                logger.ErrorException("CreateDataTable()", ex);
            }
            return false;
        }

        /// <summary>
        /// 更新数据库
        /// </summary>
        public bool CreateIndex()
        {
            SQLiteTransaction trans = null;
            try
            {
                List<string> cmdTextArray = GetCreateIndexSQL();
                if (cmdTextArray == null || cmdTextArray.Count <= 0)
                    return true;

                trans = SQLiteHelper.GetTransaction();

                foreach (string s in cmdTextArray)
                    SQLiteHelper.ExecuteNonQuery(trans, s, null);

                trans.Commit();
                return true;
            }
            catch (SQLiteException ex)
            {
                if (trans != null)
                    trans.Rollback();

                logger.ErrorException("CreateIndex().", ex);
                return false;
            }
        }

        /// <summary>
        /// 更新表
        /// </summary>
        public bool UpdateDataTable()
        {
            SQLiteTransaction trans = null;
            try
            {
                List<string> cmdTextArray = GetUpdateTableSQL();
                if (cmdTextArray == null || cmdTextArray.Count <= 0)
                    return true;

                trans = SQLiteHelper.GetTransaction();

                foreach (string s in cmdTextArray)
                    SQLiteHelper.ExecuteNonQuery(trans, s, null);

                trans.Commit();
                return true;
            }
            catch (SQLiteException ex)
            {
                if (trans != null)
                    trans.Rollback();

                logger.ErrorException("UpdateDataTable().", ex);
                return false;
            }
        }

        /// <summary>
        /// 更新表结构
        /// </summary>
        public bool UpdateFeilds()
        {
            SQLiteTransaction trans = null;
            try
            {
                List<string> cmdTextArray = GetUpdateFieldsSQL();
                if (cmdTextArray == null || cmdTextArray.Count <= 0)
                    return true;

                trans = SQLiteHelper.GetTransaction();

                foreach (string s in cmdTextArray)
                    SQLiteHelper.ExecuteNonQuery(trans, s, null);

                trans.Commit();
                return true;
            }
            catch (SQLiteException ex)
            {
                if (trans != null)
                    trans.Rollback();

                logger.ErrorException("UpdateFeilds().", ex);
                return false;
            }
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        public bool UpdateIndex()
        {
            SQLiteTransaction trans = null;
            try
            {
                List<string> cmdTextArray = GetUpdateIndexSQL();
                if (cmdTextArray == null || cmdTextArray.Count <= 0)
                    return true;

                trans = SQLiteHelper.GetTransaction();

                foreach (string s in cmdTextArray)
                    SQLiteHelper.ExecuteNonQuery(trans, s, null);

                trans.Commit();
                return true;
            }
            catch (SQLiteException ex)
            {
                if (trans != null)
                    trans.Rollback();

                logger.ErrorException("UpdateIndex().", ex);
                return false;
            }
        }

        /// <summary>
        /// 数据库升级
        /// </summary>
        /// <returns></returns>
        public bool UpgradeDB()
        {
            if (!UpdateDataTable())
                return false;

            if (!UpdateFeilds())
                return false;

            if (!UpdateIndex())
                return false;

            return true;
        }

        /// <summary>
        /// 获取数据库建表SQL
        /// </summary>
        /// <returns></returns>
        private List<string> GetCreateTable()
        {
            List<string> array = new List<string>();

            array.Add(CreateTableNoiseReal());
            array.Add(CreateTableNoiseAnalyse());
            array.Add(CreateTableRemoteCTL());
            array.Add(CreateTableGroup());
            array.Add(CreateTableNoiseRecord());
            array.Add(CreateTableGroupRecorder());
            array.Add(CreateTableRecorderSet());
            array.Add(CreateTableStandData());
            //array.Add(CreateTableUniversalTerConfig());
            array.Add(CreateTablePreTerConfig());
            array.Add(CreateTableGPRSTerminal());
            array.Add(CreateTableUniversalTerWayType());
            array.Add(CreateTableUniversalTerWayConfig());

            array.Add(CreateDBVersionTable());
            return array;
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        private List<string> GetCreateIndexSQL()
        {
            List<string> array = new List<string>();

            //创建索引
            //array.Add(@"CREATE INDEX ix_tmsOrders_OwnUser on tmsOrders(OwnUser)");
            //添加数据库版本
            array.Add(@"INSERT INTO tmsVersion(VersionType, VersionValue) VALUES ('DataBase', '" + LastestDBVersion + "')");

            return array;
        }

        /// <summary>
        /// 获取更新表结构SQL
        /// </summary>
        private List<string> GetUpdateFieldsSQL()
        {
            List<string> array = new List<string>();

            //if (!CheckColumnExists("tmsException", "BigClassName"))
            //    array.Add(@"ALTER TABLE tmsException ADD BigClassName NVARCHAR(40) NULL DEFAULT ''");

            return array;
        }

        /// <summary>
        /// 获取更新索引
        /// </summary>
        private List<string> GetUpdateIndexSQL()
        {
            List<string> array = new List<string>();

            //添加运单索引
            //if (!CheckIndexExists("", ""))
            //    array.Add(@"CREATE INDEX idx_tmsSc on tmsSc(Hawb)");

            return array;
        }

        /// <summary>
        /// 获取新增表
        /// </summary>
        private List<string> GetUpdateTableSQL()
        {
            List<string> array = new List<string>();

            //if (!CheckTableExists(""))
            //    array.Add(());

            return array;
        }
        #endregion

        #region 创建数据库表脚本
        /// <summary>
        /// 数据库版本管理表
        /// </summary>
        private string CreateDBVersionTable()
        {
            return @"CREATE TABLE tmsVersion
                    (
                        [VersionType]       NVARCHAR(10)    NOT NULL    PRIMARY KEY,
                        [VersionValue]      NVARCHAR(30)    NOT NULL    DEFAULT ''
                    )";
        }

        /// <summary>
        /// DL_NoiseReal
        /// </summary>
        /// <returns></returns>
        private string CreateTableNoiseReal()
        {
            return @"CREATE TABLE DL_Noise_Real
                     (
                        [ID]             INTEGER PRIMARY KEY         AUTOINCREMENT,
                        [GroupId]        NVARCHAR(15)    NULL        DEFAULT 0,          --组ID
                        [RecorderId]     INT             NULL        DEFAULT 0,          --记录仪ID
                        [LeakValue]      NVARCHAR(300)   NULL        DEFAULT '',         --
                        [FrequencyValue] NVARCHAR(300)   NULL        DEFAULT '',         --
                        [OriginalData]   NVARCHAR(200)   NULL        DEFAULT '',         --
                        --[MaxAmp]         NVARCHAR(200)   NULL        DEFAULT '',         --分析后的每组最大幅度值(%)
                        --[MaxFrq]         NVARCHAR(200)   NULL        DEFAULT '',         --分析后的每组最大频率值(HZ),与MaxAmp一一对应
                        [CollTime]       NVARCHAR(25)    NULL        DEFAULT (datetime('now', 'localtime')),  --时间
                        [UnloadTime]     NVARCHAR(25)    NULL        DEFAULT (datetime('now', 'localtime')),  --时间
                        [HistoryFlag]    INT             NULL        DEFAULT 0           --
                     )";
        }

        /// <summary>
        /// DL_NoiseAnalyse
        /// </summary>
        /// <returns></returns>
        private string CreateTableNoiseAnalyse()
        {
            return @"CREATE TABLE DL_NoiseAnalyse
                     (
                        [ID]             INTEGER PRIMARY KEY         AUTOINCREMENT,
                        [GroupId]        NVARCHAR(15)    NULL        DEFAULT 0,          --组ID
                        [RecorderId]     INT             NULL        DEFAULT 0,          --记录仪ID
                        [MinLeakValue]   NVARCHAR(300)   NULL        DEFAULT '',         --
                        [MinFrequencyValue] NVARCHAR(300)   NULL        DEFAULT '',      --
                        [IsLeak]         INT             NULL        DEFAULT 0,
                        [EnergyValue]    NVARCHAR(15)    NULL        DEFAULT 0,          --能量强度
                        [LeakProbability]  NVARCHAR(15)    NULL        DEFAULT 0,          --漏水概率
                        [ESA]            INT             NULL        DEFAULT 0,
                        [CollTime]       NVARCHAR(25)    NULL        DEFAULT (datetime('now', 'localtime')),  --时间
                        [UnloadTime]     NVARCHAR(25)    NULL        DEFAULT (datetime('now', 'localtime')),  --时间
                        [HistoryFlag]    INT             NULL        DEFAULT 0           --
                     )";
        }

        /// <summary>
        /// EN_DistanceControl
        /// </summary>
        /// <returns></returns>
        private string CreateTableRemoteCTL()
        {
            return @"CREATE TABLE EN_DistanceControl
                     (
                        [ID]             INTEGER PRIMARY KEY         AUTOINCREMENT,
                        [ControlId]      NVARCHAR(15)    NULL        DEFAULT 0,          --组ID
                        [RecorderId]     INT             NULL        DEFAULT 0,          --记录仪ID
                        [IP]             NVARCHAR(20)    NULL        DEFAULT '',         --
                        [Port]           INT             NULL        DEFAULT 0,
                        [SendTime]       INT             NULL        DEFAULT 0
                     )";
        }

        /// <summary>
        /// En_Group
        /// </summary>
        /// <returns></returns>
        private string CreateTableGroup()
        {
            return @"CREATE TABLE En_Group
                     (
                        [GroupId]        INTEGER PRIMARY KEY         NOT NULL,           --组ID
                        [Name]           NVARCHAR(30)    NULL        DEFAULT '',         --
                        [Remark]         NVARCHAR(100)   NULL        DEFAULT '',         --备注
                        [ModifyTime]     NVARCHAR(25)    NULL        DEFAULT (datetime('now', 'localtime'))
                     )";
        }

        /// <summary>
        /// En_NoiseRecorder
        /// </summary>
        /// <returns></returns>
        private string CreateTableNoiseRecord()
        {
            return @"CREATE TABLE En_NoiseRecorder
                     (
                        [ID]             INTEGER PRIMARY KEY         AUTOINCREMENT,
                        [RecorderId]     NVARCHAR(15)    NULL        DEFAULT 0,          --组ID
                        [AddDate]        NVARCHAR(25)    NULL        DEFAULT (datetime('now', 'localtime')),
                        [Remark]         NVARCHAR(100)   NULL        DEFAULT '',         --备注
                        [GroupState]     INT             NULL        DEFAULT 0
                     )";
        }

        /// <summary>
        /// MP_GroupRecorder
        /// </summary>
        /// <returns></returns>
        private string CreateTableGroupRecorder()
        {
            return @"CREATE TABLE MP_GroupRecorder
                     (
                        [ID]             INTEGER PRIMARY KEY         AUTOINCREMENT,
                        [GroupId]        NVARCHAR(15)    NULL        DEFAULT 0,          --组ID
                        [RecorderId]     INT             NULL        DEFAULT 0,
                        [ModifyTime]     NVARCHAR(25)    NULL        DEFAULT (datetime('now', 'localtime'))
                     )";
        }

        /// <summary>
        /// MT_RecorderSet
        /// </summary>
        /// <returns></returns>
        private string CreateTableRecorderSet()
        {
            return @"CREATE TABLE MT_RecorderSet
                     (
                        [ID]             INTEGER PRIMARY KEY         AUTOINCREMENT,
                        [RecorderId]     INT             NULL        DEFAULT 0,
                        [RecordTime]     INT             NULL        DEFAULT 0,
                        [PickSpan]       INT             NULL        DEFAULT 0,
                        [Controler_Power] INT             NULL        DEFAULT 0,
                        [StartEnd_Power]  INT             NULL        DEFAULT 0,
                        [LeakValue]       INT             NULL        DEFAULT 0, 
                        [CommunicationTime] INT             NULL        DEFAULT 0
                     )";
        }

        /// <summary>
        /// ST_Noise_StandData
        /// </summary>
        /// <returns></returns>
        private string CreateTableStandData()
        {
            return @"CREATE TABLE ST_Noise_StandData
                     (
                        [ID]             INTEGER PRIMARY KEY         AUTOINCREMENT,
                        [GroupId]        NVARCHAR(15)    NULL        DEFAULT 0,          --组ID
                        [RecorderId]     INT             NULL        DEFAULT 0,
                        [data]           NVARCHAR(300)   NULL        DEFAULT '',
                        [CreateTime]     NVARCHAR(25)    NULL        DEFAULT (datetime('now', 'localtime'))
                     )";
        }

        //终端表
        private string CreateTableGPRSTerminal()
        {
            return @"CREATE TABLE [Terminal]
                    (
	                    [ID]                [INT]           NOT NULL,
	                    [TerminalID]		[INT] NOT NULL,								--终端ID
	                    [TerminalName]		NVARCHAR(200)   NOT NULL,					--终端名称
                        [TerminalType]      [INT]           NOT NULL,                   --终端类型 
	                    [Address]			NVARCHAR(200)	NOT NULL DEFAULT '',		--地址
	                    [Remark]			NVARCHAR(200)	NULL DEFAULT '',		    --备注
                        [SyncState]         INT             NULL        DEFAULT 1 ,     --0:已同步,1:新增未同步,-1:删除未同步
	                    [ModifyTime]		NVARCHAR(25)	NOT NULL DEFAULT (datetime('now', 'localtime'))	--修改时间
                    )";
        }

        //压力终端配置
        private string CreateTablePreTerConfig()
        {
            return @"CREATE TABLE [PreTerConfig]
                    (
	                    [ID]                INTEGER PRIMARY KEY         AUTOINCREMENT,
	                    [TerminalID]		[INT] NOT NULL,								--终端ID
	                    [EnablePreAlarm]	[INT]			NOT NULL DEFAULT 1,			--是否启用压力报警 0:不启用,1:启用
	                    [EnableSlopeAlarm]	[INT]			NOT NULL DEFAULT 1,			--是否启用斜率报警 0:不启用,1:启用
	                    [PreUpperLimit]		NUMERIC(8, 5)	NOT NULL DEFAULT 0,			--压力上限值
	                    [PreLowLimit]		NUMERIC(8, 5)	NOT NULL DEFAULT 0,			--压力下限值
	                    [PreSlopeUpLimit]   NUMERIC(8, 5)   NOT NULL DEFAULT 0,			--压力斜率上限值
	                    [PreSlopeLowLimit]  NUMERIC(8, 5)   NOT NULL DEFAULT 0,			--压力斜率下限值
                        [SyncState]         INT             NULL     DEFAULT 1 ,        --0:已同步,1:新增未同步,-1:删除未同步
	                    [ModifyTime]		NVARCHAR(25)    NOT NULL DEFAULT (datetime('now', 'localtime'))	--修改时间
                    )";
        }

//        private string CreateTableUniversalTerConfig()
//        {
//            return @"CREATE TABLE [UniversalTerConfig]
//                    (
//	                    [ID]                INTEGER PRIMARY KEY         AUTOINCREMENT,
//	                    [TerminalID]		[INT] NOT NULL,								--终端ID
//	                    [ModifyTime]		[DATETIME]		NOT NULL DEFAULT (datetime('now', 'localtime'))	--修改时间
//                    )";
//        }

        /// <summary>
        /// 第几路采集类型(通用终端)
        /// </summary>
        /// <returns></returns>
        private string CreateTableUniversalTerWayType()
        {
            return @"CREATE TABLE [UniversalTerWayType]
                    (
	                    [ID]                [INT]           NOT NULL,
                        [TerminalType]      [INT]           NOT NULL,                           --终端类型 ,通用终端和在线水质
	                    [Level]             [INT]           NOT NULL,                           --从1开始
	                    [ParentID]          [INT]           NOT NULL    DEFAULT -1,
                        [WayType]           [INT]           NOT NULL,
	                    [Name]              NVARCHAR(30)    NOT NULL,
	                    [MaxMeasureRange]   NUMERIC(18, 2)  NOT NULL,
	                    [MaxMeasureRangeFlag] NUMERIC(18, 2) NOT NULL,
                        [FrameWidth]        [INT]           NOT NULL    DEFAULT 2,              --帧中数据宽度(字节)
                        [Sequence]          [INT]           NOT NULL,                           --帧中顺序,一级节点为0,子节点从1开始
	                    [Precision]         [INT]           NULL,
	                    [Unit]              NVARCHAR(20)    NULL,
                        [SyncState]         INT             NULL        DEFAULT 1,              --0:已同步,1:新增未同步,-1:删除未同步
	                    [ModifyTime]        NVARCHAR(25)    NOT NULL    DEFAULT (datetime('now', 'localtime'))
                    )";
        }
        /// <summary>
        /// 采集类型配置(通用终端)
        /// </summary>
        /// <returns></returns>
        private string CreateTableUniversalTerWayConfig()
        {
            return @"CREATE TABLE [UniversalTerWayConfig]
                    (
	                    [ID]                [INT]           NOT NULL,
                        [TerminalID]		[INT]           NOT NULL,								--终端ID
                        [TerminalType]      [INT]           NOT NULL,                               --终端类型 ,通用终端和在线水质
                        [Sequence]          [INT]           NOT NULL,                               --对应顺序序号
	                    [PointID]           [INT]           NOT NULL,
                        [SyncState]         [INT]           NULL        DEFAULT 1,                  --0:已同步,1:新增未同步,-1:删除未同步
	                    [ModifyTime]        NVARCHAR(25)    NOT NULL    DEFAULT (datetime('now', 'localtime'))
                    )";
        }
        #endregion
            
        #region 重建数据库
        /// <summary>
        /// 重建数据库
        /// </summary>
        public bool ResetDatabase()
        {
            try
            {
                if (!CreateDatabase())
                {
                    DeleteDatabase();
                    return false;
                }
                if (!CreateDataTable())
                {
                    DeleteDatabase();
                    return false;
                }
                if (!CreateIndex())
                {
                    DeleteDatabase();
                    return false;
                }
                System.Threading.Thread.Sleep(5 * 1000);    //等待5秒，等待数据写入数据库
                return true;
            }
            catch (System.Exception ex)
            {
                logger.Error("ResetDatabase()." + ex.Message);
                return false;
            }
        }
        #endregion

        #region 检查指定表，指定字段是否存在
        /// <summary>
        /// 检查指定表是否存在
        /// </summary>
        /// <param name="TableName">表名</param>
        private bool CheckTableExists(string TableName)
        {
            string strSQL = @"SELECT COUNT(1) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TABLE_NAME";
            
            SQLiteParameter parm = new SQLiteParameter("@TABLE_NAME", DbType.String, 50);
            parm.Value = TableName;

            object obj = SQLiteHelper.ExecuteScalar(strSQL, parm);
            if (obj == null)
                return false;
            else
                return Convert.ToInt32(obj) == 0 ? false : true;
        }

		/// <summary>
		/// 检查指定表的指定列的长度
		/// </summary>
		/// <param name="TableName">表名</param>
		/// <param name="ColumnName">列名</param>
		/// <param name="EqualsLength">对比长度</param>
		/// <returns></returns>
		private bool CheckColumnLength(string TableName, string ColumnName, int EqualsLength)
		{
			string strSQL = @"SELECT COUNT(1) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME =@TABLE_NAME AND COLUMN_NAME=@COLUMN_NAME and character_maximum_length=@LENGTH ";

			SQLiteParameter[] parm = new SQLiteParameter[] { new SQLiteParameter("@TABLE_NAME", DbType.String, 50), new SQLiteParameter("@COLUMN_NAME", DbType.String, 50),new SQLiteParameter("@LENGTH",DbType.Int32) };
			parm[0].Value = TableName;
			parm[1].Value = ColumnName;
			parm[2].Value = EqualsLength;

			object obj = SQLiteHelper.ExecuteScalar(strSQL, parm);
			if (obj == null)
				return false;
			else
				return Convert.ToInt32(obj) == 0 ? false : true;
		}

		/// <summary>
		/// 检查指定表的指定列是否存在
		/// </summary>
		/// <param name="TableName">表名</param>
		/// <param name="ColumnName">列名</param>
		private bool CheckColumnExists(string TableName, string ColumnName)
		{
			string strSQL = @"SELECT COUNT(1) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TABLE_NAME AND COLUMN_NAME = @COLUMN_NAME";

            SQLiteParameter[] parm = new SQLiteParameter[] { new SQLiteParameter("@TABLE_NAME", DbType.String, 50), new SQLiteParameter("@COLUMN_NAME", DbType.String, 50) };
			parm[0].Value = TableName;
			parm[1].Value = ColumnName;

			object obj = SQLiteHelper.ExecuteScalar(strSQL, parm);
			if (obj == null)
				return false;
			else
				return Convert.ToInt32(obj) == 0 ? false : true;
		}

        /// <summary>
        /// 检查指定表的指定索引是否存在
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="IndexName">索引名</param>
        private bool CheckIndexExists(string TableName, string IndexName)
        {
            string strSQL = @"SELECT COUNT(1) FROM INFORMATION_SCHEMA.INDEXES WHERE TABLE_NAME = @TABLE_NAME AND INDEX_NAME = @INDEX_NAME";

            SQLiteParameter[] parm = new SQLiteParameter[] { new SQLiteParameter("@TABLE_NAME", DbType.String, 50), new SQLiteParameter("@INDEX_NAME", DbType.String, 50) };
            parm[0].Value = TableName;
            parm[1].Value = IndexName;

            object obj = SQLiteHelper.ExecuteScalar(strSQL, parm);
            if (obj == null)
                return false;
            else
                return Convert.ToInt32(obj) == 0 ? false : true;
        }
        #endregion
    }
}
