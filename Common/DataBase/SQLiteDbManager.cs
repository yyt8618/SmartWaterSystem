using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SQLite;
using System.Data;

namespace NoiseAnalysisSystem
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
            get { return File.Exists(SQLiteHelper.FileName); }
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

                SQLiteConnection.CreateFile(SQLiteHelper.ConnectionString);

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
                    return false;

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

            array.Add(CreateOrdersTable());
            array.Add(CreateScanDataTable());
            array.Add(CreateShipmentTable());
            array.Add(CreateExceptionTable());
            array.Add(CreateStationTable());
            array.Add(CreateUserProfileTable());
            array.Add(CreateDBVersionTable());
            array.Add(CreateDestinationTable);
            array.Add(CreatExceptionPackage);
            array.Add(CreateGPSInfo);
            array.Add(CreateRefuseOrder);
            array.Add(CreateStationDetailTable());
            array.Add(CreatePaymentRecordTable);
            array.Add(CreateNotifyMessageTable());
            array.Add(CreateDispatchTaskTable());
            array.Add(CreateOrderGoodsTable());
            array.Add(CreateDeliveryFee());
            array.Add(CreatePriceCurTable());//运费表
            array.Add(CreateBlackNameTable());//黑名单
            array.Add(CreateCustMonthTable());//月结客户
			//array.Add(CreateBillsSendTable());//面单发放（全峰
            array.Add(CreatePOSRecordTable());
            array.Add(CreatePwStatus());
            return array;
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        private List<string> GetCreateIndexSQL()
        {
            List<string> array = new List<string>();

            //创建索引
            array.Add(@"CREATE INDEX ix_tmsOrders_OwnUser on tmsOrders(OwnUser)");

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
            //if (!CheckIndexExists("tmsScanData", "idx_tmsScanData_ScanHawb"))
            //    array.Add(@"CREATE INDEX idx_tmsScanData_ScanHawb on tmsScanData(ScanHawb)");

            return array;
        }

        /// <summary>
        /// 获取新增表
        /// </summary>
        private List<string> GetUpdateTableSQL()
        {
            List<string> array = new List<string>();

            //if (!CheckTableExists("tmsPOSRecord"))
            //    array.Add(CreatePOSRecordTable());

            return array;
        }
        #endregion

        #region 创建数据库表脚本
        /// <summary>
        /// 订单表
        /// </summary>
        private string CreateOrdersTable()
        {
            return @"CREATE TABLE tmsOrders
                    (
                        [Id]                BIGINT          NOT NULL    IDENTITY(1,1)   PRIMARY KEY,
                        [OwnUser]           NVARCHAR(20)    NULL        DEFAULT '',
                        [OrderId]           NVARCHAR(20)    NULL        DEFAULT '',
                        [OrderType]         INT             NULL        DEFAULT 0,
                        [OrderTime]         Datetime        NULL        DEFAULT (GETDATE()),
                        [CustomerCode]      NVARCHAR(20)    NULL        DEFAULT '',
                        [CustomerName]      NVARCHAR(100)   NULL        DEFAULT '',
                        [CustomerContract]  NVARCHAR(50)    NULL        DEFAULT '',
                        [CustomerAddress]   NVARCHAR(200)   NULL        DEFAULT '',
                        [CustomerTel]       NVARCHAR(50)    NULL        DEFAULT '',
                        [Destination]       NVARCHAR(50)    NULL        DEFAULT '',
                        [OrderStatus]       INT             NULL        DEFAULT 0,
                        [ProcessTime]       Datetime        NULL        DEFAULT '',
                        [Comments]          NVARCHAR(100)   NULL        DEFAULT '',
                        [orderCategory]     INT             NULL        DEFAULT 2,           --1:淘宝件; 2:普通件,默认普通件
                        [ConsigneeName]     NVARCHAR(20)    NULL        DEFAULT '',
                        [ConCompanyName]    NVARCHAR(100)   NULL        DEFAULT '',
                        [ConsigneeTel]      NVARCHAR(50)    NULL        DEFAULT '',
                        [ConsigneeAddress]  NVARCHAR(200)   NULL        DEFAULT '',
                        [OrgId]             NVARCHAR(100)   NULL        DEFAULT '',
                        [GoodsWeight]       NUMERIC(20,2)   NULL        DEFAULT 0,
                        [Eappointment]      Datetime        NULL        DEFAULT '',
                        [DaiShouFee]        NUMERIC(20,2)   NULL        DEFAULT 0
                    )";
        }
//        /// <summary>
//        /// 面单发放
//        /// </summary>
//        /// <returns></returns>
//        private string CreateBillsSendTable()
//        {
//            return @"CREATE TABLE tmsBillsSend
//                    (
//                        [Id]                BIGINT          NOT NULL    IDENTITY(1,1)   PRIMARY KEY,
//						[ScanBatch]         NVARCHAR(20)    NOT NULL    DEFAULT '',
//                        [StartId]           NVARCHAR(20)    NOT NULL    DEFAULT '',				--起始单号
//                        [EndId]             NVARCHAR(20)    NOT NULL    DEFAULT '',				--截止单号
//                        [IssueTime]         Datetime        NULL        DEFAULT (GETDATE()),	--发放时间
//                        [MaterialId]        NVARCHAR(20)    NULL        DEFAULT '',				--物料编号
//                        [RecipientsOutlets] NVARCHAR(20)    NULL        DEFAULT '',				--领用网点
//                        [Quantity]		    INT			    NULL        DEFAULT 0,				--数量
//                        [Operator]		    NVARCHAR(20)    NULL        DEFAULT '',				--经办人
//                        [agencyNetwork]     NVARCHAR(20)    NULL        DEFAULT '',				--经办网点
//                        [UploadStatus]      INT             NOT NULL    DEFAULT 2,          --上传状态,该处默认为2表示该上传状态无效(shipment表的状态有效)
//                        [UploadTime]        NVARCHAR(20)    NULL        DEFAULT (''),       --上传时间
//                        [UploadErrMsg]      NVARCHAR(100)   NULL        DEFAULT ''
//                    )";
//        }
        /// <summary>
        /// 运费表
        /// </summary>
        /// <returns></returns>
        private string CreatePriceCurTable()
        {
             return @"CREATE TABLE tmsPriceCur
                   (
                        [ID]                BIGINT          NOT NULL    PRIMARY KEY,                 --ID
                        [PRICE_ID]          BIGINT          NOT NULL               ,                  --价格ID
                        [SOURCE_ZONE_CODE]  NVARCHAR(30)    NOT NULL    DEFAULT '',                  --原寄地代码
                        [DEST_ZONE_CODE]    NVARCHAR(30)    NOT NULL    DEFAULT '',                  --目的地代码
                        [EXPRESS_TYPE_CODE] NVARCHAR(30)    NULL        DEFAULT '',                  --业务类型代码
                        [CARGO_TYPE_CODE]   NVARCHAR(30)    NULL        DEFAULT '',                  --快件内容代码
                        [LIMIT_TYPE_CODE]   NVARCHAR(30)    NULL        DEFAULT '',                  --时效类型代码
                        [ISTANCE_TYPE_CODE] NVARCHAR(30)    NULL        DEFAULT '',                  --区域类型代码
                        [MODIFY_TIME]       Datetime        NULL        DEFAULT '',                  --最后修改时间
                        [NEXT_WEIGHT_ID]    NUMERIC(20,2)   NULL        DEFAULT 0,                   --续重价格ID
                        [WEIGHT_ROUND_TYPE] NVARCHAR(10)    NULL        DEFAULT '',                  --重量进位类型编码
                        [PRICE_ROUND_TYPE]  NVARCHAR(10)    NULL        DEFAULT '',                  --价格进位类型编码
                        [START_WEIGHT_QTY]  NUMERIC(10,2)   NULL        DEFAULT 0,                   --起始重量
                        [MAX_WEIGHT_QTY]    NUMERIC(10,2)   NULL        DEFAULT 0,                   --最大重量
                        [WEIGHT_PRICE_QTY]  NUMERIC(10,2)   NULL        DEFAULT 0,                   --续重运费
                        [BASE_WEIGHT_QTY]   NUMERIC(10,2)   NULL        DEFAULT 0,                   --首重
                        [BASE_PRICE]        NUMERIC(10,2)   NULL        DEFAULT 0,                   --首重运费
                        [VALID_DATE]        Datetime        NULL        DEFAULT (GETDATE()),         --生效日期
                        [LastModifyUser]    NVARCHAR(20)    NULL        DEFAULT '',                  --上次修改者
                        [LastModifyDate]    BIGINT          NULL        DEFAULT 0                    --上次修改时间
                    )";
        }
        /// <summary>
        /// 黑名单
        /// </summary>
        /// <returns></returns>
        private string CreateBlackNameTable()
        {
            return @"CREATE TABLE tmsBlackName(
                        [ID]                BIGINT          NOT NULL    PRIMARY KEY,                 --ID
                        [DEPT_CODE]         NVARCHAR(12)    NULL        DEFAULT '',                  --分点部代码
                        [CUST_TYPE]         INT             NULL                  ,                  --黑名单类型
                        [CUST_NAME]         NVARCHAR(200)   NULL        DEFAULT '',                  --客户名称
                        [CUST_TEL]          NVARCHAR(16)    NULL        DEFAULT '',                  --客户联系电话
                        [COMP_ADDR]         NVARCHAR(200)   NULL        DEFAULT '',                  --公司地址
                        [MEMO]              NVARCHAR(200)   NULL        DEFAULT '',                  --备注
                        [LastModifyUser]    NVARCHAR(20)    NULL        DEFAULT '',                  --上次修改者
                        [LastModifyDate]    BIGINT          NULL        DEFAULT 0                    --上次修改时间
                    )";
        }
        /// <summary>
        /// 月结客户查询
        /// </summary>
        /// <returns></returns>
        private string CreateCustMonthTable()
        {
            return @"CREATE TABLE tmsCustMonth(
                        [ID]                BIGINT          NOT NULL    PRIMARY KEY,                    --ID
                        [CUST_CODE]         NVARCHAR(10)    NOT NULL    DEFAULT '',                     --月结账号
                        [DEPT_CODE]         NVARCHAR(10)    NULL        DEFAULT '',                     --月结账号所属分点部代码
                        [CUST_NAME]         NVARCHAR(500)   NULL        DEFAULT '',                     --客户简称
                        [VALID_DATE]        Datetime        NULL        DEFAULT (GETDATE()),            --生效日期
                        [LastModifyUser]    NVARCHAR(20)    NULL        DEFAULT '',                     --上次修改者
                        [LastModifyDate]    BIGINT          NULL        DEFAULT 0                       --上次修改时间
                    )";
        }
        /// <summary>
        /// 淘宝订单商品表
        /// </summary>
        /// <returns></returns>
        private string CreateOrderGoodsTable()
        {
            return @"CREATE TABLE tmsOrderGoods
                    (
                        [Id]                BIGINT          NOT NULL    IDENTITY(1,1)   PRIMARY KEY,
                        [OrderId]           NVARCHAR(20)    NULL        DEFAULT '',         --订单编号
                        [GoodsName]         NVARCHAR(100)   NULL        DEFAULT '',         --商品名称
                        [GoodsNum]          INT             NULL        DEFAULT 0,          --商品数量
                        [Weight]            NUMERIC(18,2)   NULL        DEFAULT 0,         --商品重量
                        [GoodsVolume]       NUMERIC(20,2)   NULL        DEFAULT 0          --商品体积
                    )";
        }

        /// <summary>
        /// 运单表
        /// </summary>
        private string CreateShipmentTable()
        {
            return @"CREATE TABLE tmsShipment
                    (
                        [Id]                BIGINT          NOT NULL    IDENTITY(1,1)   PRIMARY KEY,
                        [ScanBatch]         NVARCHAR(20)    NOT NULL    DEFAULT '',         --批次号
                        [OrderId]           NVARCHAR(20)    NULL        DEFAULT '',         --订单号
                        [CustomerCode]      NVARCHAR(20)    NULL        DEFAULT '',         --客户编号
                        [Hawb]              NVARCHAR(30)    NULL        DEFAULT '',         --运单号
                        [Piece]             INT             NULL        DEFAULT 0,          --件数
                        [Weight]            NUMERIC(18,2)   NULL        DEFAULT 0,          --重量
                        [ProductCode]       NVARCHAR(20)    NULL        DEFAULT '',         --产品编号
                        [GoodsType]         NVARCHAR(20)    NULL        DEFAULT '',         --物品类型
                        [InsuredValue]      NUMERIC(18,2)   NULL        DEFAULT 0,          --保价值
                        [InsuredFee]        NUMERIC(18,2)   NULL        DEFAULT 0,          --保价金额
                        [DaiShouFee]        NUMERIC(18,2)   NULL        DEFAULT 0,          --代收款
                        [DaoFuFee]          NUMERIC(18,2)   NULL        DEFAULT 0,          --到付款
                        [FreightFee]        NUMERIC(18,2)   NULL        DEFAULT 0,          --运费
                        [Destination]       NVARCHAR(20)    NULL        DEFAULT '',         --目的地编号
                        [ReturnBillFlag]    BIT             NULL        DEFAULT 'FALSE',    --有无回单
                        [ReturnBill]        NVARCHAR(20)    NULL        DEFAULT '',         --回单编号
                        [PaymentType]       NVARCHAR(20)    NULL        DEFAULT '',         --支付类型
                        [ServiceType]       NVARCHAR(20)    NULL        DEFAULT '',         --服务类型
                        [IsInsured]         BIT             NULL        DEFAULT(0),         --是否保价
                        [AcceptSMS]         BIT             NULL        DEFAULT(0),         --是否接收短信
                        [TelNo]             NVARCHAR(20)    NULL        DEFAULT '',         --电话号码
                        [IsCollectnCheck]   BIT             NULL        DEFAULT(0),         --是否代收支票 
                        [ProcessTime]       Datetime        NULL        DEFAULT (GETDATE()),--处理时间
                        [OwnUser]           NVARCHAR(20)    NULL        DEFAULT '',         --处理人
                        [UploadStatus]      INT             NOT NULL    DEFAULT 0,          --上传状态
                        [UploadTime]        NVARCHAR(20)    NULL        DEFAULT (''),       --上传时间
                        [UploadErrMsg]      NVARCHAR(100)   NULL        DEFAULT ''          --上传错误消息
                    )";
        }

        /// <summary>
        /// 扫描数据表
        /// </summary>
        private string CreateScanDataTable()
        {
            return @"CREATE TABLE tmsScanData
                    (
                        [Id]                BIGINT          NOT NULL    IDENTITY(1,1)   PRIMARY KEY,
                        [ScanBatch]         NVARCHAR(20)    NOT NULL    DEFAULT '',
                        [FromToStation]     NVARCHAR(20)    NULL        DEFAULT '',
                        [BusinessMan]       NVARCHAR(20)    NULL        DEFAULT '',
                        [ScanType]          INT             NOT NULL    DEFAULT 10,
                        [ScanHawb]          NVARCHAR(30)    NOT NULL    DEFAULT '',
                        [ScanCarcode]       NVARCHAR(20)    NOT NULL    DEFAULT '',        --车线码
                        [BagNumber]         NVARCHAR(20)    NULL        DEFAULT '',
                        [ScanUser]          NVARCHAR(20)    NOT NULL    DEFAULT '',
                        [ScanTime]          Datetime        NOT NULL    DEFAULT (GETDATE()),
                        [OperationTime]     Datetime        NULL        DEFAULT (GETDATE()),
                        [ShiftTimes]        NVARCHAR(10)    NULL        DEFAULT '',
                        [ScanStation]       NVARCHAR(20)    NULL        DEFAULT '',
                        [ExceptionCode]     NVARCHAR(20)    NULL        DEFAULT '',
                        [ExceptionMemo]     NVARCHAR(100)   NULL        DEFAULT '',
                        [SignatureType]     NVARCHAR(20)    NULL        DEFAULT '',
                        [CustomerSignature] NVARCHAR(50)    NULL        DEFAULT '',
                        [TransportType]     NVARCHAR(20)    NULL        DEFAULT '',
                        [SignPhotoPath]     NVARCHAR(200)   NULL        DEFAULT '',
                        [ToHawb]            NVARCHAR(20)    NULL        DEFAULT '',
                        [Weight]            NUMERIC(18,2)   NULL        DEFAULT 0,
                        [IsCalTranferFee]   INT             NULL        DEFAULT 0,
                        [UploadStatus]      INT             NOT NULL    DEFAULT 2,          --上传状态,该处默认为2表示该上传状态无效(shipment表的状态有效)
                        [UploadTime]        NVARCHAR(20)    NULL        DEFAULT (''),       --上传时间
                        [UploadErrMsg]      NVARCHAR(100)   NULL        DEFAULT '',
                        [SealNum]           NVARCHAR(20)    NULL        DEFAULT '',
                        [FromToStationBak]  NVARCHAR(20)    NULL        DEFAULT '',         --备用站点，目前只有民航到发扫描用到
                        [PosExcp]           NVARCHAR(50)    NULL        DEFAULT ''          --POS未刷卡原因
                    )";
        }

        /// <summary>
        /// 问题件原因表
        /// </summary>
        private string CreateExceptionTable()
        {
            return @"CREATE TABLE tmsException
                    (
                        [Id]                BIGINT          NOT NULL    PRIMARY KEY,
                        [ExceptionType]     NVARCHAR(20)    NOT NULL    DEFAULT '',
                        [ExceptionCode]     NVARCHAR(20)    NOT NULL    DEFAULT '',
                        [ExceptionDesc]     NVARCHAR(100)   NULL        DEFAULT '',
                        [LastModifyUser]    NVARCHAR(20)    NULL        DEFAULT '', 
                        [LastModifyDate]    BIGINT          NULL        DEFAULT 0,
                        [BigClassCode]      NVARCHAR(20)    NULL        DEFAULT '',         --大类Code
                        [BigClassName]      NVARCHAR(40)    NULL        DEFAULT ''          --大类Name
                    )";
        }

        /// <summary>
        /// 网点表
        /// </summary>
        private string CreateStationTable()
        {
            return @"CREATE TABLE tmsStation
                    (
                        [Id]                BIGINT          NOT NULL    PRIMARY KEY,
                        [StationCode]       NVARCHAR(20)    NOT NULL    DEFAULT '',
                        [StationName]       NVARCHAR(100)   NULL        DEFAULT '',
                        [LastModifyUser]    NVARCHAR(20)    NULL        DEFAULT '',
                        [LastModifyDate]    BIGINT          NULL        DEFAULT 0
                    )";
        }

        /// <summary>
        /// 网点明细表
        /// </summary>
        private string CreateStationDetailTable()
        {
            return @"CREATE TABLE tmsStationDetail
                    (
                      Id                    BIGINT          NOT NULL PRIMARY KEY,
                      StationCode           NVARCHAR(20)    NOT NULL    DEFAULT '',
                      StationName           NVARCHAR(100)   NULL        DEFAULT '',
                      OrgType               INT             NOT NULL DEFAULT(3),
                      OrgTypeDesc           NVARCHAR(20)    NOT NULL DEFAULT '',
                      HighOrg               NVARCHAR(20)    NULL DEFAULT(''),
                      Province              NVARCHAR(40)    NULL DEFAULT(''),
                      City                  NVARCHAR(40)    NULL DEFAULT(''),
                      DefaultCurrency       NVARCHAR(10)    NULL DEFAULT(''),
                      SuperFinanceCenter    NVARCHAR(20)    NULL DEFAULT(''),
                      Phone                 NVARCHAR(150)   NULL DEFAULT(''),
                      Principal             NVARCHAR(50)    NULL DEFAULT(''),
                      DefaultSendPlace      NVARCHAR(20)    NULL DEFAULT(''),
                      IsAllowToPayment      BIT             NULL DEFAULT(0),
                      AreaName              NVARCHAR(30)    NULL DEFAULT(''),
                      SalePhone             NVARCHAR(50)    NULL DEFAULT(''),
                      Fax                   NVARCHAR(50)    NULL DEFAULT(''),
                      DispatchRange         NTEXT           NULL,
                      NotDispatchRange      NTEXT           NULL,
                      IsAllowAgentMoney     BIT             NULL DEFAULT(0),
                      DispatchTimeLimit     NVARCHAR(1000)  NULL DEFAULT(''),
                      GoodsPaymentLimited   Decimal(10,2)   NULL DEFAULT(0),
                      ToPaymentLimited      Decimal(10,2)   NULL DEFAULT(0),
                      IsPreLimit            BIT             NULL DEFAULT(0),
                      GoodsBillFlag         BIT             NULL DEFAULT(0),
                      VipFlag               BIT             NULL DEFAULT(0),
                      MaxVipGP              Decimal(10,2)   NULL DEFAULT(0),
                      TaobaoGotFlag         BIT             NULL DEFAULT(0),
                      TaobaoDeliveryFlag    BIT             NULL DEFAULT(0),
                      LastModifyUser        NVARCHAR(20)    NULL DEFAULT(''),
                      LastModifyDate        BIGINT          NULL DEFAULT(0),
                      IsShow                BIT             NULL DEFAULT(1)
                    )";
        }

        /// <summary>
        /// 员工表
        /// </summary>
        private string CreateUserProfileTable()
        {
            return @"CREATE TABLE tmsUserProfile
                    (
                        [Id]                BIGINT          NOT NULL,
                        [UserId]            NVARCHAR(20)    NOT NULL    PRIMARY KEY,
                        [UserName]          NVARCHAR(40)    NULL        DEFAULT '',
                        [Password]          NVARCHAR(20)    NULL        DEFAULT '',
                        [StationCode]       NVARCHAR(20)    NULL        DEFAULT '',
                        [StationName]       NVARCHAR(50)    NULL        DEFAULT '',
                        [UserType]          INT             NULL        DEFAULT 3,
                        [AdminFlag]         BIT             NULL        DEFAULT 'FALSE',
                        [LastModifyUser]    NVARCHAR(20)    NULL        DEFAULT '',
                        [LastModifyDate]    BIGINT          NULL        DEFAULT 0
                    )";
        }

        /// <summary>
        /// 目的地表
        /// </summary>
        public string CreateDestinationTable
        {
            get
            {
                return @"CREATE TABLE tmsDestination
                        (                    
                           ID                  BIGINT               not null,      --标示ID              
                           SiteID              NVARCHAR(20)         not null,      --地区编号
                           SiteName            NVARCHAR(40)         not null,      --地区名称
                           StationCode         NVARCHAR(20)         not null,      --网点编号
                           StationName         NVARCHAR(100)        null DEFAULT '',--网点名称
                           [LastModifyUser]    NVARCHAR(20)         NULL    DEFAULT '',
                           [LastModifyDate]    BIGINT               NULL    DEFAULT 0,       
                           Constraint PK_Destination primary key  (ID)
                        )";
            }
        }

        /// <summary>
        /// 异常件
        /// </summary>
        public string CreatExceptionPackage
        {
            get
            {
                return @"CREATE TABLE tmsExceptionPackage
                        (
                            [Id]                BIGINT          NOT NULL    IDENTITY(1,1)   PRIMARY KEY,
                            ScanHawb           NVARCHAR(20)         NOT NULL,                   --运单号
                            ExceptionCode      NVARCHAR(20)         NOT NULL,                   --异常编码
                            ExceptionMemo      NVARCHAR(200)        NULL    DEFAULT ''          --异常备注
                        )";
            }
        }

        /// <summary>
        /// 拒收订单
        /// </summary>
        public string CreateRefuseOrder
        {
            get
            {
                return @"CREATE TABLE tmsRefuseOrder
                        (
                            [OrderId]           NVARCHAR(20)        NULL        DEFAULT '',             --订单号
                            [ExceptionMemo]     NVARCHAR(200)       NULL        DEFAULT '',             --异常备注
                            [OperationTime]     Datetime            NULL        DEFAULT (GETDATE()),    --操作时间
                            [OperationUser]     NVARCHAR(20)        NULL        DEFAULT '',             --操作人员
                            [UploadStatus]      INT                 NOT NULL    DEFAULT 0,              --上传状态
                            [UploadTime]        NVARCHAR(20)        NULL        DEFAULT (''),           --上传时间
                            [UploadErrMsg]      NVARCHAR(100)       NULL        DEFAULT ''              --上传错误消息
                        )";
            }
        }

        /// <summary>
        /// GPS信息
        /// </summary>
        public string CreateGPSInfo
        {
            get
            {
                return @"CREATE TABLE tmsGpsInfo
                        (
                            [ID]                BIGINT          NOT NULL    IDENTITY(1,1)   PRIMARY KEY,
                            [LocationTime]      NVARCHAR(20)    NOT NULL    DEFAULT '',
                            [LatitudeDegree]    INT             NULL        DEFAULT 0,
                            [LatitudeMinute]    REAL            NULL        DEFAULT 0,
                            [LongitudeDegree]   INT             NULL        DEFAULT 0,
                            [LongitudeMinute]   REAL            NULL        DEFAULT 0,
                            [Speed]             TINYINT         NULL        DEFAULT 0,
                            [Bearing]           SMALLINT        NULL        DEFAULT 0,
                            [Status]            TINYINT         NULL        DEFAULT 0,
                            [ACLineStatus]      TINYINT         NULL        DEFAULT 0,
                            [CreateTime]        DateTime        NULL        DEFAULT (GETDATE()),
                            [Upload]            TINYINT         NULL        DEFAULT 0
                        )";
            }
        }

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
        /// 签收财务表
        /// </summary>
        private string CreatePaymentRecordTable
        {
            get
            {
                return @"CREATE TABLE tmsPaymentRecord
                        (
                            [ID]                BIGINT          NOT NULL    IDENTITY(1,1)   PRIMARY KEY,
                            [Hawb]              NVARCHAR(20)    NULL        DEFAULT '',                     --运单号
                            [PayType]           INT             NOT NULL    DEFAULT 0,                      --支付类型,支票(1)，现金(2)，刷卡(3),到付转月结(4)
                            [FeeType]           INT             NOT NULL    DEFAULT 0,                      --代收货款(1)，到付(2)
                            [Fee]               NUMERIC(18,2)   NULL        DEFAULT 0,                      --费用
                            [PosCet]            NVARCHAR(100)   NULL        DEFAULT '',                     --POS凭证
                            [ScanType]          INT             NOT NULL    DEFAULT 15,                     --扫描类型，默认正常签收扫描（15）
                            [ScanUser]          NVARCHAR(20)    NOT NULL    DEFAULT '',
                            [ScanTime]          Datetime        NOT NULL    DEFAULT (GETDATE()),            --扫描时间
                            [UploadStatus]      INT             NOT NULL    DEFAULT 0,                      --上传状态
                            [UploadTime]        NVARCHAR(20)    NULL        DEFAULT (''),                   --上传时间
                            [UploadErrMsg]      NVARCHAR(100)   NULL        DEFAULT '' 
                        )";         
            }
        }

        /// <summary>
        /// pos运单处理结果报告表
        /// 交易状态:00-刷卡成功 01-刷卡部分成功 30-现金成功 31-现金部分成功 60-支票成功 20-拒收 40-异常 70-快递单撤销成功
        ///拒收或异常的原因码：21 拒收 22 地址错误 23 拒付全款 24 联系不上 25 内损 26 开箱验货 27 丢失 28 货款不符 29 要求无法满足
        ///30 更改付款方式 31 重复订购 32 未订购 33 天气原因 34 发票问题 35 配送延迟 36 其他原因 37 超出配送 38 范围取消配送 39 内缺 40 用户自提
        /// </summary>
        private string CreatePOSRecordTable()
        {
            return @"CREATE TABLE tmsPOSRecord
                        (
                            [ID]                BIGINT          NOT NULL    IDENTITY(1,1)   PRIMARY KEY,
                            [TerminalNo]        NVARCHAR(10)    NULL        DEFAULT '',                     --终端号
                            [TotalPayment]      NUMERIC(18,2)   NULL        DEFAULT 0,                      --交易金额
                            [OperateUser]       NVARCHAR(20)    NOT NULL    DEFAULT '',                     --工号
                            [Hawb]              NVARCHAR(20)    NOT NULL    DEFAULT '',                     --运单号
                            [CardNo]            NVARCHAR(20)    NOT NULL    DEFAULT '',                     --卡号
                            [OperateTime]       Datetime        NOT NULL    DEFAULT (GETDATE()),            --扫描时间
                            [TerminalFlow]      NVARCHAR(6)     NOT NULL    DEFAULT '',                     --终端流水号
                            [UnionFlow]         NVARCHAR(12)    NOT NULL    DEFAULT '',                     --银联流水号
                            [State]             NVARCHAR(2)     NOT NULL    DEFAULT '00',                   --交易状态
                            [ReasonCode]        NVARCHAR(2)     NOT NULL    DEFAULT '',                     --异常原因码
                            [UploadStatus]      INT             NOT NULL    DEFAULT 0,                      --上传状态
                            [UploadTime]        NVARCHAR(20)    NULL        DEFAULT (''),                   --上传时间
                            [IsDeposit]         INT             NOT NULL    DEFAULT 0,                      --是否为存款,0:代收;1:存款
                            [UploadErrMsg]      NVARCHAR(100)   NULL        DEFAULT '' 
                        )";
        }

        /// <summary>
        /// 消息通知表
        /// </summary>
        private string CreateNotifyMessageTable()
        {
            return @"CREATE TABLE tmsNotifyMessage
                     (
                        [ID]            BIGINT          NOT NULL    IDENTITY(1,1)   PRIMARY KEY,
                        [MsgID]         BIGINT          NULL        DEFAULT 0,
                        [MsgType]       INT             NULL        DEFAULT 0,
                        [MsgTitle]      NVARCHAR(100)   NULL        DEFAULT '',
                        [MsgContent]    NVARCHAR(3000)  NULL        DEFAULT '',
                        [MsgStatus]     INT             NULL        DEFAULT 0,
                        [ReceieveOrg]   NVARCHAR(20)    NULL        DEFAULT '',
                        [ReceieveUser]  NVARCHAR(20)    NULL        DEFAULT '',
                        [SendOrg]       NVARCHAR(20)    NULL        DEFAULT '',
                        [SendUser]      NVARCHAR(20)    NULL        DEFAULT '',
                        [SendTime]      DATETIME        NULL        DEFAULT GETDATE(),
                        [LastSyncTime]  BIGINT          NULL        DEFAULT 0,
                        [VarNo1]        NVARCHAR(100)   NULL        DEFAULT '',
                        [VarNo2]        NVARCHAR(100)   NULL        DEFAULT '',
                        [VarNo3]        NVARCHAR(100)   NULL        DEFAULT ''
                     )";
        }

        /// <summary>
        /// 派件任务表
        /// </summary>
        private string CreateDispatchTaskTable()
        {
            return @"CREATE TABLE tmsDispatchTask
                     (
                        [ID]            BIGINT          NOT NULL    IDENTITY(1,1)   PRIMARY KEY,
                        [TaskID]        BIGINT          NULL        DEFAULT 0,          --任务ID
                        [Hawb]          NVARCHAR(30)    NULL        DEFAULT '',         --运单号码
                        [ReceiveMan]    NVARCHAR(50)    NULL        DEFAULT '',         --收件人
                        [ReceiveAddr]   NVARCHAR(200)   NULL        DEFAULT '',         --收件地址
                        [ReceiveTel]    NVARCHAR(50)    NULL        DEFAULT '',         --收件电话
                        [TotalPiece]    INT             NULL        DEFAULT 0,          --总件数
                        [TotalWeight]   DECIMAL(12,2)   NULL        DEFAULT 0,          --总重量
                        [DaiShouFee]    DECIMAL(12,2)   NULL        DEFAULT 0,          --代收货款
                        [DaoFuFee]      DECIMAL(12,2)   NULL        DEFAULT 0,          --到付费
                        [DispatchMan]   NVARCHAR(20)    NULL        DEFAULT '',         --派送人员
                        [DispatchOrg]   NVARCHAR(20)    NULL        DEFAULT '',         --派送机构
                        [TaskStatus]    INT             NULL        DEFAULT 0           --任务状态(0:初始, 1:签收, 2:异常签收)
                     )";
        }

        /// <summary>
        /// 香港加收费用表
        /// </summary>
        /// <returns></returns>
        private string CreateDeliveryFee()
        {
            return @"CREATE TABLE tmsDeliveryFee
                    (
                        ID              BIGINT          NOT NULL,                       --标示ID     
                        Region          NVARCHAR(50)    NULL        DEFAULT '',         --区域名称
                        Company         NVARCHAR(40)    NULL        DEFAULT '',         --公司或仓库
                        Address         NVARCHAR(100)   NULL        DEFAULT '',         --地址
                        AddtionFee      DECIMAL(12,2)   NULL        DEFAULT 0,          --偏远费用
                        WareHouseFee    DECIMAL(12,2)   NULL        DEFAULT 0,          --入仓费用
                        Remark          NVARCHAR(256)   NULL        DEFAULT '',         --备注
                        [LastModifyDate]    BIGINT      NULL        DEFAULT 0,
                        Constraint PK_Destination primary key  (ID)     
                    )";
        }

        /// <summary>
        /// 电量记录表
        /// </summary>
        /// <returns></returns>
        private string CreatePwStatus()
        {
            return @"CREATE TABLE tmsPWStatus
                    (
                        [ID]                BIGINT          NOT NULL    IDENTITY(1,1)   PRIMARY KEY,   
                        StationCode         NVARCHAR(20)    NULL        DEFAULT '',             --站点编号
                        [SN]                NVARCHAR(20)    NULL        DEFAULT '',             --SN
                        DeviceType          NVARCHAR(20)    NULL        DEFAULT '',             --设备类型
                        [OperateTime]       DATETIME        NULL        DEFAULT GETDATE(),      --上传时间
                        PwPersent           INT             NULL        DEFAULT 0,              --电量(百分比)
                        IsCharge            INT             NULL        DEFAULT '',             --是否充电 0:未充电,1:充电
                        [UploadStatus]      INT             NOT NULL    DEFAULT 0,              --上传状态
                        [UploadTime]        DATETIME        NULL        DEFAULT GETDATE(),      --上传时间                     
                        [UploadErrMsg]      NVARCHAR(100)   NULL        DEFAULT ''             --错误消息   
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
                System.Threading.Thread.Sleep(30 * 1000);    //等待15秒，等待CE将数据写入数据库
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
