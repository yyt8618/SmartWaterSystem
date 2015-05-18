using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using Entity;
using Common;
using System.Data.SQLite;

namespace DAL
{
    public class TerminalDataDAL
    {
        public DataTable GetTerID_PointID(TerType type)
        {
            string SQL = "SELECT ID,TerminalID,TerminalName,Address,Remark,ModifyTime FROM Terminal WHERE TerminalType='" + (int)type + "'";
            return SQLHelper.ExecuteDataTable(SQL, null);
        }

        public bool TerminalExist(TerType type,string TerID)
        {
            string SQL = "SELECT COUNT(1) FROM Terminal WHERE TerminalType='" + (int)type + "' AND TerminalID='"+TerID+"'";
            object obj_count = SQLHelper.ExecuteScalar(SQL, null);
            if (obj_count != DBNull.Value && obj_count != null)
                return Convert.ToInt32(obj_count) > 0 ? true : false;

            return false;
        }

        #region GPRS数据操作
        public int InsertGPRSPreData(Queue<GPRSPreFrameDataEntity> datas)
        {
            lock (ConstValue.obj)
            {
                SqlTransaction trans = null;
                try
                {
                    trans = SQLHelper.GetTransaction();

                    string SQL_Frame = "INSERT INTO Frame(Dir,Frame,LogTime) VALUES(@dir,@frame,@logtime)";
                    SqlParameter[] parms_frame = new SqlParameter[]{
                new SqlParameter("@dir",SqlDbType.Int),
                new SqlParameter("@frame",SqlDbType.VarChar,2000),
                new SqlParameter("@logtime",SqlDbType.DateTime)
            };
                    SqlCommand command_frame = new SqlCommand();
                    command_frame.CommandText = SQL_Frame;
                    command_frame.Parameters.AddRange(parms_frame);
                    command_frame.CommandType = CommandType.Text;
                    command_frame.Connection = SQLHelper.Conn;
                    command_frame.Transaction = trans;

                    string SQL_PreData = "INSERT INTO Pressure_Real(TerminalID,PressValue,CollTime,UnloadTime,HistoryFlag) VALUES(@TerId,@prevalue,@coltime,@UploadTime,0)";
                    SqlParameter[] parms_predata = new SqlParameter[]{
                    new SqlParameter("@TerId",SqlDbType.Int),
                    new SqlParameter("@prevalue",SqlDbType.Decimal),
                    new SqlParameter("@coltime",SqlDbType.DateTime),
                    new SqlParameter("@UploadTime",SqlDbType.DateTime)
                };
                    SqlCommand command_predata = new SqlCommand();
                    command_predata.CommandText = SQL_PreData;
                    command_predata.Parameters.AddRange(parms_predata);
                    command_predata.CommandType = CommandType.Text;
                    command_predata.Connection = SQLHelper.Conn;
                    command_predata.Transaction = trans;

                    //string en_point_id = "";
                    while (datas.Count > 0)
                    {
                        GPRSPreFrameDataEntity entity = datas.Dequeue();
                        if (entity == null)
                        {
                            break;
                        }
                        parms_frame[0].Value = 1;
                        parms_frame[1].Value = entity.Frame;
                        parms_frame[2].Value = entity.ModifyTime;

                        command_frame.ExecuteNonQuery();

                        //en_point_id = "";
                        //foreach (DataRow dr in dt_PointID.Rows)
                        //{
                        //    if (dr["TerminalID"].ToString() == entity.TerId)
                        //    {
                        //        en_point_id = dr["ID"].ToString();
                        //        break;
                        //    }
                        //}

                        //if (!string.IsNullOrEmpty(en_point_id))
                        //{
                            for (int i = 0; i < entity.lstPreData.Count; i++)
                            {
                                parms_predata[0].Value = entity.TerId;
                                parms_predata[1].Value = entity.lstPreData[i].PreValue;
                                parms_predata[2].Value = entity.lstPreData[i].ColTime;
                                parms_predata[3].Value = entity.ModifyTime;

                                command_predata.ExecuteNonQuery();
                            }
                        //}
                    }
                    trans.Commit();

                    return 1;
                }
                catch (Exception ex)
                {
                    if (trans != null)
                        trans.Rollback();
                    throw ex;
                }
            }
        }

        public int InsertGPRSFlowData(Queue<GPRSFlowFrameDataEntity> datas)
        {
            lock (ConstValue.obj)
            {
                SqlTransaction trans = null;
                try
                {
                    //DataTable dt_PointID = GetTerID_PointID(TerType.PreTer);
                    //if ((dt_PointID == null) || (dt_PointID.Rows.Count == 0))
                    //{
                    //    return 0;
                    //}
                    trans = SQLHelper.GetTransaction();

                    string SQL_Frame = "INSERT INTO Frame(Dir,Frame,LogTime) VALUES(@dir,@frame,@logtime)";
                    SqlParameter[] parms_frame = new SqlParameter[]{
                new SqlParameter("@dir",SqlDbType.Int),
                new SqlParameter("@frame",SqlDbType.VarChar,2000),
                new SqlParameter("@logtime",SqlDbType.DateTime)
            };
                    SqlCommand command_frame = new SqlCommand();
                    command_frame.CommandText = SQL_Frame;
                    command_frame.Parameters.AddRange(parms_frame);
                    command_frame.CommandType = CommandType.Text;
                    command_frame.Connection = SQLHelper.Conn;
                    command_frame.Transaction = trans;

                    string SQL_PreData = "INSERT INTO Flow_Real(TerminalID,FlowValue,FlowInverted,FlowInstant,CollTime,UnloadTime,HistoryFlag) VALUES(@TerId,@flowvalue,@flowreverse,@flowinstant,@coltime,@UploadTime,0)";
                    SqlParameter[] parms_predata = new SqlParameter[]{
                    new SqlParameter("@TerId",SqlDbType.Int),
                    new SqlParameter("@flowvalue",SqlDbType.Decimal),
                    new SqlParameter("@flowreverse",SqlDbType.Decimal),
                    new SqlParameter("@flowinstant",SqlDbType.Decimal),
                    new SqlParameter("@coltime",SqlDbType.DateTime),
                    new SqlParameter("@UploadTime",SqlDbType.DateTime)
                };
                    SqlCommand command_predata = new SqlCommand();
                    command_predata.CommandText = SQL_PreData;
                    command_predata.Parameters.AddRange(parms_predata);
                    command_predata.CommandType = CommandType.Text;
                    command_predata.Connection = SQLHelper.Conn;
                    command_predata.Transaction = trans;

                    //string en_point_id = "";
                    while (datas.Count > 0)
                    {
                        GPRSFlowFrameDataEntity entity = datas.Dequeue();
                        parms_frame[0].Value = 1;
                        parms_frame[1].Value = entity.Frame;
                        parms_frame[2].Value = entity.ModifyTime;

                        command_frame.ExecuteNonQuery();

                        for (int i = 0; i < entity.lstFlowData.Count; i++)
                        {
                            parms_predata[0].Value = entity.TerId;
                            parms_predata[1].Value = entity.lstFlowData[i].Forward_FlowValue;
                            parms_predata[2].Value = entity.lstFlowData[i].Reverse_FlowValue;
                            parms_predata[3].Value = entity.lstFlowData[i].Instant_FlowValue;
                            parms_predata[4].Value = entity.lstFlowData[i].ColTime;
                            parms_predata[5].Value = entity.ModifyTime;

                            command_predata.ExecuteNonQuery();
                        }
                    }
                    trans.Commit();

                    return 1;
                }
                catch (Exception ex)
                {
                    if (trans != null)
                        trans.Rollback();
                    throw ex;
                }
            }
        }

        public int InsertGPRSUniversalData(Queue<GPRSUniversalFrameDataEntity> datas)
        {
            lock (ConstValue.obj)
            {
                SqlTransaction trans = null;
                try
                {
                    trans = SQLHelper.GetTransaction();

                    string SQL_Frame = "INSERT INTO Frame(Dir,Frame,LogTime) VALUES(@dir,@frame,@logtime)";
                    SqlParameter[] parms_frame = new SqlParameter[]{
                new SqlParameter("@dir",SqlDbType.Int),
                new SqlParameter("@frame",SqlDbType.VarChar,2000),
                new SqlParameter("@logtime",SqlDbType.DateTime)
            };
                    SqlCommand command_frame = new SqlCommand();
                    command_frame.CommandText = SQL_Frame;
                    command_frame.Parameters.AddRange(parms_frame);
                    command_frame.CommandType = CommandType.Text;
                    command_frame.Connection = SQLHelper.Conn;
                    command_frame.Transaction = trans;

                    string SQL_Data = @"INSERT INTO UniversalTerData([TerminalID],[DataValue],[Simulate1Zero],[Simulate2Zero],[CollTime],[UnloadTime],TypeTableID) 
                                            VALUES(@terId,@datavalue,@sim1zero,@sim2zero,@coltime,@UploadTime,@tableid)";
                    SqlParameter[] parms_data = new SqlParameter[]{
                    new SqlParameter("@terId",SqlDbType.Int),
                    new SqlParameter("@datavalue",SqlDbType.Decimal),
                    new SqlParameter("@sim1zero",SqlDbType.Decimal),
                    new SqlParameter("@sim2zero",SqlDbType.Decimal),
                    new SqlParameter("@coltime",SqlDbType.DateTime),

                    new SqlParameter("@UploadTime",SqlDbType.DateTime),
                    new SqlParameter("@tableid",SqlDbType.Int)
                };
                    SqlCommand command_predata = new SqlCommand();
                    command_predata.CommandText = SQL_Data;
                    command_predata.Parameters.AddRange(parms_data);
                    command_predata.CommandType = CommandType.Text;
                    command_predata.Connection = SQLHelper.Conn;
                    command_predata.Transaction = trans;

                    while (datas.Count > 0)
                    {
                        GPRSUniversalFrameDataEntity entity = datas.Dequeue();
                        parms_frame[0].Value = 1;
                        parms_frame[1].Value = entity.Frame;
                        parms_frame[2].Value = entity.ModifyTime;

                        command_frame.ExecuteNonQuery();

                        for (int i = 0; i < entity.lstData.Count; i++)
                        {
                            parms_data[0].Value = entity.TerId;
                            parms_data[1].Value = entity.lstData[i].DataValue;
                            parms_data[2].Value = entity.lstData[i].Sim1Zero;
                            parms_data[3].Value = entity.lstData[i].Sim2Zero;
                            parms_data[4].Value = entity.lstData[i].ColTime;

                            parms_data[5].Value = entity.ModifyTime;
                            parms_data[6].Value = entity.lstData[i].TypeTableID;

                            command_predata.ExecuteNonQuery();
                        }
                    }
                    trans.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    if (trans != null)
                        trans.Rollback();
                    throw ex;
                }
            }
        }

        public int InsertGPRSOLWQData(Queue<GPRSOLWQFrameDataEntity> datas)
        {
            lock (ConstValue.obj)
            {
                SqlTransaction trans = null;
                try
                {
                    trans = SQLHelper.GetTransaction();

                    string SQL_Frame = "INSERT INTO Frame(Dir,Frame,LogTime) VALUES(@dir,@frame,@logtime)";
                    SqlParameter[] parms_frame = new SqlParameter[]{
                new SqlParameter("@dir",SqlDbType.Int),
                new SqlParameter("@frame",SqlDbType.VarChar,2000),
                new SqlParameter("@logtime",SqlDbType.DateTime)
            };
                    SqlCommand command_frame = new SqlCommand();
                    command_frame.CommandText = SQL_Frame;
                    command_frame.Parameters.AddRange(parms_frame);
                    command_frame.CommandType = CommandType.Text;
                    command_frame.Connection = SQLHelper.Conn;
                    command_frame.Transaction = trans;

                    string SQL_Data = "INSERT INTO OLWQ_Real(TerminalID,Turbidity,ResidualCl,PH,Conductivity,Temperature,CollTime,UnloadTime) VALUES(@TerId,@Turbidity,@ResidualCl,@PH,@Cond,@Temp,@coltime,@UploadTime)";
                    SqlParameter[] parms_predata = new SqlParameter[]{
                    new SqlParameter("@TerId",SqlDbType.Int),
                    new SqlParameter("@Turbidity",SqlDbType.Decimal),
                    new SqlParameter("@ResidualCl",SqlDbType.Decimal),
                    new SqlParameter("@PH",SqlDbType.Decimal),
                    new SqlParameter("@Cond",SqlDbType.Decimal),
                    new SqlParameter("@Temp",SqlDbType.Decimal),
                    new SqlParameter("@coltime",SqlDbType.DateTime),
                    new SqlParameter("@UploadTime",SqlDbType.DateTime)
                };
                    SqlCommand command_data = new SqlCommand();
                    command_data.CommandText = SQL_Data;
                    command_data.Parameters.AddRange(parms_predata);
                    command_data.CommandType = CommandType.Text;
                    command_data.Connection = SQLHelper.Conn;
                    command_data.Transaction = trans;

                    while (datas.Count > 0)
                    {
                        GPRSOLWQFrameDataEntity entity = datas.Dequeue();
                        parms_frame[0].Value = 1;
                        parms_frame[1].Value = entity.Frame;
                        parms_frame[2].Value = entity.ModifyTime;

                        command_frame.ExecuteNonQuery();

                        for (int i = 0; i < entity.lstOLWQData.Count; i++)
                        {
                            parms_predata[0].Value = entity.TerId;
                            parms_predata[1].Value = entity.lstOLWQData[i].Turbidity;
                            parms_predata[2].Value = entity.lstOLWQData[i].ResidualCl;
                            parms_predata[3].Value = entity.lstOLWQData[i].PH;
                            parms_predata[4].Value = entity.lstOLWQData[i].Conductivity;
                            parms_predata[5].Value = entity.lstOLWQData[i].Temperature;
                            parms_predata[6].Value = entity.lstOLWQData[i].ColTime;
                            parms_predata[7].Value = entity.ModifyTime;

                            command_data.ExecuteNonQuery();
                        }
                    }
                    trans.Commit();

                    return 1;
                }
                catch (Exception ex)
                {
                    if (trans != null)
                        trans.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 获取需要下发的参数
        /// </summary>
        /// <returns></returns>
        public List<GPRSCmdEntity> GetGPRSParm()
        {
            lock (ConstValue.obj)
            {
                //SELECT * FROM DL_ParamToDev WHERE ID IN (SELECT MAX(ID) FROM DL_ParamToDev WHERE SendedFlag=0 GROUP BY DeviceId)
                string SQL = "SELECT ID,DeviceId,DevTypeId,CtrlCode,FunCode,DataValue,DataLenth,SetDate FROM ParamToDev WHERE SendedFlag = 0";
                List<GPRSCmdEntity> lstCmd = null;
                using (SqlDataReader reader = SQLHelper.ExecuteReader(SQL, null))
                {
                    lstCmd = new List<GPRSCmdEntity>();
                    while (reader.Read())
                    {
                        GPRSCmdEntity cmd = new GPRSCmdEntity();

                        cmd.TableId = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : -1;
                        cmd.DeviceId = reader["DeviceId"] != DBNull.Value ? Convert.ToInt32(reader["DeviceId"]) : -1;
                        cmd.DevTypeId = reader["DevTypeId"] != DBNull.Value ? Convert.ToInt32(reader["DevTypeId"]) : -1;
                        cmd.CtrlCode = reader["CtrlCode"] != DBNull.Value ? Convert.ToInt32(reader["CtrlCode"]) : -1;
                        cmd.FunCode = reader["FunCode"] != DBNull.Value ? Convert.ToInt32(reader["FunCode"]) : -1;
                        cmd.Data = reader["DataValue"] != DBNull.Value ? reader["DataValue"].ToString() : "";

                        cmd.DataLen = reader["DataLenth"] != DBNull.Value ? Convert.ToInt32(reader["DataLenth"]) : -1;
                        cmd.ModifyTime = reader["SetDate"] != DBNull.Value ? Convert.ToDateTime(reader["SetDate"]) : DateTime.Now;
                        cmd.SendedFlag = 0;

                        lstCmd.Add(cmd);
                    }
                }

                return lstCmd;
            }
        }

        public int UpdateGPRSParmFlag(List<GPRSCmdFlag> ids)
        {
            lock (ConstValue.obj)
            {
                SqlTransaction trans = null;
                try
                {
                    if ((ids == null) || (ids.Count == 0))
                    {
                        return 0;
                    }

                    trans = SQLHelper.GetTransaction();

                    string SQL = "UPDATE ParamToDev SET SendedFlag=1 WHERE ID=@id";
                    SqlParameter parm =  new SqlParameter("@id",SqlDbType.Int);

                    SqlCommand command = new SqlCommand();
                    command.CommandText = SQL;
                    command.Parameters.Add(parm);
                    command.CommandType = CommandType.Text;
                    command.Connection = SQLHelper.Conn;
                    command.Transaction = trans;

                    for (int i = 0; i<ids.Count; i++)
                    {
                        parm.Value = ids[i].TableId;

                        command.ExecuteNonQuery();
                    }
                    trans.Commit();

                    return 1;
                }
                catch (Exception ex)
                {
                    if (trans != null)
                        trans.Rollback();
                    throw ex;
                }
            }
        }
        #endregion

        public DataTable GetTerInfo(TerType type)
        {
            string SQL = "SELECT ID,TerminalID,TerminalName,Address,Remark,ModifyTime FROM Terminal WHERE SyncState<>-1 AND TerminalType='" + (int)type + "' ORDER BY TerminalID";
            return SQLiteHelper.ExecuteDataTable(SQL, null);
        }

        /// <summary>
        /// 查找指定类型的终端是否存在,-1:查找发生异常,0:不存在,1:存在
        /// </summary>
        /// <param name="type"></param>
        /// <param name="TerminalID"></param>
        /// <returns></returns>
        public int GetTerExist(TerType type, int TerminalID)
        {
            string SQL = "SELECT COUNT(1) FROM Terminal WHERE TerminalType='" + (int)type + "' AND TerminalID='"+TerminalID+"'";
            object obj = SQLiteHelper.ExecuteScalar(SQL, null);
            if (obj != null && obj != DBNull.Value)
            {
                return Convert.ToInt32(obj) > 0 ? 1 : 0;
            }
            else
            {
                return 0;
            }
        }

        public void DeleteTer(TerType type, string TerminalID)
        {
            string SQL = "";
            string SQL_SELECT = "SELECT COUNT(1) FROM Terminal WHERE SyncState=1 AND TerminalType='" + (int)type + "' AND TerminalID='" + TerminalID + "'";
            object obj_exist = SQLiteHelper.ExecuteScalar(SQL_SELECT, null);
            bool exist = false;
            if (obj_exist != null && obj_exist != DBNull.Value)
            {
                exist = (Convert.ToInt32(obj_exist) > 0 ? true : false);
            }
            if (exist)
                SQL = "DELETE FROM Terminal WHERE SyncState=1 AND TerminalType='" + (int)type + "' AND TerminalID='" + TerminalID + "'";
            else
                SQL = "UPDATE Terminal SET SyncState=-1 WHERE TerminalType='" + (int)type + "' AND TerminalID='" + TerminalID + "'";
            SQLiteHelper.ExecuteNonQuery(SQL, null);
        }

        public int GetTerminalTableMaxId()
        {
            string SQL = "SELECT MAX(id) FROM Terminal";
            object obj = SQLiteHelper.ExecuteScalar(SQL, null);
            if (obj != null && obj != DBNull.Value)
            {
                return Convert.ToInt32(obj);
            }
            else
            {
                return 0;
            }
        }


        public int GetUniversalTerWayConfigTableMaxId()
        {
            string SQL = "SELECT MAX(id) FROM UniversalTerWayConfig";
            object obj = SQLiteHelper.ExecuteScalar(SQL, null);
            if (obj != null && obj != DBNull.Value)
            {
                return Convert.ToInt32(obj);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 保存终端配置
        /// </summary>
        /// <returns></returns>
        public int SaveTerInfo(int terminalid, string name, string addr, string remark,TerType terType, List<UniversalWayTypeConfigEntity> lstPointID,bool needmodify = true)
        {
            SQLiteTransaction trans = null;
            try
            {
                trans = SQLiteHelper.GetTransaction();
                SQLiteCommand command = new SQLiteCommand();
                command.Connection = SQLiteHelper.Conn;
                command.Transaction = trans;

                if (needmodify)
                {
                    command.CommandText = "DELETE FROM Terminal WHERE SyncState=1 AND TerminalType='" + (int)terType + "' AND TerminalID='" + terminalid + "'";
                    command.ExecuteNonQuery();

                    command.CommandText = "UPDATE Terminal SET SyncState=-1 WHERE TerminalType='" + (int)terType + "' AND TerminalID='" + terminalid + "'";
                    command.ExecuteNonQuery();
                }

                command.CommandText = string.Format("INSERT INTO Terminal(ID,TerminalID,TerminalName,TerminalType,Address,Remark) VALUES('{0}','{1}','{2}','{3}','{4}','{5}')",
                                                         GetTerminalTableMaxId() + 1, terminalid, name.Trim(), (int)terType, addr.Trim(), remark.Trim());
                command.ExecuteNonQuery();

                //Update UniversalTerConfig Table

                //Update UniversalTerWayConfig Table
                if (lstPointID != null && lstPointID.Count > 0)
                {
                    //UniversalTerWayConfig ID TerminalID PointID
                    command.CommandText = "DELETE FROM UniversalTerWayConfig WHERE SyncState<>0 AND TerminalID='" + terminalid + "'";
                    command.ExecuteNonQuery();

                    command.CommandText = "UPDATE UniversalTerWayConfig SET SyncState=-1 WHERE TerminalID='" + terminalid + "'";
                    command.ExecuteNonQuery();

                    int configeMaxId = GetUniversalTerWayConfigTableMaxId();
                    foreach (UniversalWayTypeConfigEntity config in lstPointID)
                    {
                        configeMaxId++;
                        command.CommandText = string.Format("INSERT INTO UniversalTerWayConfig(ID,TerminalID,Sequence,PointID,TerminalType) VALUES('{0}','{1}','{2}','{3}','{4}')",
                            configeMaxId, terminalid, config.Sequence, config.PointID, (int)terType);
                        command.ExecuteNonQuery();
                    }
                }

                trans.Commit();
                return 1;
            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();
                return -1;
            }
        }

        public void DeleteUniversalWayTypeConfig(int PointID)
        {
            string SQL = "UPDATE UniversalTerWayConfig SET SyncState=-1 WHERE PointID='" + PointID + "'";
            SQLiteHelper.ExecuteNonQuery(SQL, null);
        }

        public void DeleteUniversalWayTypeConfig_TerID(int TerminalID,TerType terType)
        {
            string SQL_Ter = "SELECT Distinct PointID FROM UniversalTerWayConfig WHERE TerminalID='" + TerminalID + "' AND TerminalType='"+((int)terType).ToString()+"'";
            List<string> lstPoint = new List<string>();
            using (SQLiteDataReader reader = SQLiteHelper.ExecuteReader(SQL_Ter, null))
            {
                while (reader.Read())
                {
                    lstPoint.Add(reader["PointID"].ToString());
                }
            }
            if (lstPoint != null && lstPoint.Count > 0)
            {
                foreach (string pointid in lstPoint)
                {
                    string SQL = "";
                    string SQL_SELECT = "SELECT COUNT(1) FROM UniversalTerWayConfig WHERE SyncState=-1 AND PointID='" + pointid + "' AND TerminalID='" + TerminalID + "' AND TerminalType='" + ((int)terType).ToString() + "'";
                    object obj_exist = SQLiteHelper.ExecuteScalar(SQL_SELECT, null);
                    bool exist = false;
                    if (obj_exist != null && obj_exist != DBNull.Value)
                    {
                        exist = (Convert.ToInt32(obj_exist) > 0 ? true : false);
                    }
                    if (exist)
                        SQL = "DELETE FROM UniversalTerWayConfig WHERE PointID='" + pointid + "' AND TerminalID='" + TerminalID + "' AND TerminalType='" + ((int)terType).ToString() + "'";
                    else
                        SQL = "UPDATE UniversalTerWayConfig SET SyncState=-1 WHERE PointID='" + pointid + "' AND TerminalID='" + TerminalID + "' AND TerminalType='" + ((int)terType).ToString() + "'";
                    SQLiteHelper.ExecuteNonQuery(SQL, null);
                }
            }
        }

        public List<UniversalWayTypeConfigEntity> GetUniversalWayTypeConfig(int TerminalID,TerType terType)
        {
            string SQL = "SELECT id,Sequence,PointID,SyncState,ModifyTime FROM UniversalTerWayConfig WHERE TerminalID='" + TerminalID + "' AND SyncState!=-1 AND TerminalType='" + ((int)terType).ToString() + "'";
            
            using (SQLiteDataReader reader = SQLiteHelper.ExecuteReader(SQL, null))
            {
                List<UniversalWayTypeConfigEntity> lstWayTypeConfig = new List<UniversalWayTypeConfigEntity>();
                while (reader.Read())
                {
                    UniversalWayTypeConfigEntity entity = new UniversalWayTypeConfigEntity();
                    entity.ID = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : -1;
                    entity.PointID = reader["PointID"] != DBNull.Value ? Convert.ToInt32(reader["PointID"]) : -1;
                    entity.Sequence = reader["Sequence"] != DBNull.Value ? Convert.ToInt32(reader["Sequence"]) : -1;
                    entity.TerminalID = TerminalID;
                    entity.SyncState = reader["SyncState"] != DBNull.Value ? Convert.ToInt32(reader["SyncState"]) : -1;
                    entity.ModifyTime = reader["ModifyTime"] != DBNull.Value ? Convert.ToDateTime(reader["ModifyTime"]) : ConstValue.MinDateTime;

                    lstWayTypeConfig.Add(entity);
                }
                return lstWayTypeConfig;
            }
            return null;
        }

        public DataTable GetUniversalDataConfig(TerType terType)
        {
            string SQL = @"SELECT DISTINCT Type.ID,Config.TerminalID,Config.Sequence,Type.[Level],Type.[ParentID],Type.[WayType],Type.[Name],Type.[MaxMeasureRange],Type.[MaxMeasureRangeFlag],Type.[FrameWidth],Type.[Precision],Type.[Unit]
                        FROM [UniversalTerWayConfig] Config,[UniversalTerWayType] Type WHERE Config.PointID=Type.ID OR Config.PointID=Type.ParentID AND Config.TerminalType=Type.TerminalType AND Config.TerminalType='"+((int)terType).ToString()+"'";

            DataTable dt = SQLHelper.ExecuteDataTable(SQL, null);
            return dt;
        }

        public List<PreDetailDataEntity> GetPreDetail(string TerminalID, DateTime minTime, DateTime maxTime, int interval)
        {
            string SQL = @"SELECT PressValue,CollTime FROM Pressure_Real 
            WHERE CollTime BETWEEN @mintime AND @maxtime AND DATEDIFF(minute,@mintime,CollTime) %@interval = 0 AND TerminalID=@TerId  ORDER BY CollTime";

            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter("@TerId",SqlDbType.Int),
                new SqlParameter("@mintime",SqlDbType.DateTime),
                new SqlParameter("@maxtime",SqlDbType.DateTime),
                new SqlParameter("@interval",SqlDbType.Int)
            };

            parms[0].Value = TerminalID;
            parms[1].Value = minTime;
            parms[2].Value = maxTime;
            parms[3].Value = interval;

            using (SqlDataReader reader = SQLHelper.ExecuteReader(SQL, parms))
            {
                List<PreDetailDataEntity> lstData = new List<PreDetailDataEntity>();
                while (reader.Read())
                {
                    PreDetailDataEntity entity = new PreDetailDataEntity();
                    entity.PreData = reader["PressValue"] != DBNull.Value ? Convert.ToDecimal(reader["PressValue"]) : 0;
                    entity.CollTime = reader["CollTime"] != DBNull.Value ? Convert.ToDateTime(reader["CollTime"]) : ConstValue.MinDateTime;

                    lstData.Add(entity);
                }
                return lstData;
            }
            return null;
        }

        public List<PreDetailDataEntity> GetFlowDetail(string TerminalID, DateTime minTime, DateTime maxTime, int interval, int datatype)
        {
            string SQL = @"SELECT FlowValue,FlowInverted,FlowInstant,CollTime FROM Flow_Real 
            WHERE CollTime BETWEEN @mintime AND @maxtime AND DATEDIFF(minute,@mintime,CollTime) %@interval = 0 AND TerminalIDID=@TerId  ORDER BY CollTime";

            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter("@TerId",SqlDbType.Int),
                new SqlParameter("@mintime",SqlDbType.DateTime),
                new SqlParameter("@maxtime",SqlDbType.DateTime),
                new SqlParameter("@interval",SqlDbType.Int)
            };

            parms[0].Value = TerminalID;
            parms[1].Value = minTime;
            parms[2].Value = maxTime;
            parms[3].Value = interval;

            using (SqlDataReader reader = SQLHelper.ExecuteReader(SQL, parms))
            {
                List<PreDetailDataEntity> lstData = new List<PreDetailDataEntity>();
                while (reader.Read())
                {
                    PreDetailDataEntity entity = new PreDetailDataEntity();
                    if (datatype == 1)  //正向流量
                        entity.PreData = reader["FlowValue"] != DBNull.Value ? Convert.ToDecimal(reader["FlowValue"]) : 0;
                    else if (datatype == 2)  //反向流量
                        entity.PreData = reader["FlowInverted"] != DBNull.Value ? Convert.ToDecimal(reader["FlowInverted"]) : 0;
                    else if (datatype == 3)  //瞬时流量
                        entity.PreData = reader["FlowInstant"] != DBNull.Value ? Convert.ToDecimal(reader["FlowInstant"]) : 0;
                    entity.CollTime = reader["CollTime"] != DBNull.Value ? Convert.ToDateTime(reader["CollTime"]) : ConstValue.MinDateTime;

                    lstData.Add(entity);
                }
                return lstData;
            }
            return null;
        }

        public List<UniversalDetailDataEntity> GetUniversalDetail(string TerminalID, int typeId, DateTime minTime, DateTime maxTime, int interval)
        {
            string SQL = @"SELECT [DataValue],CollTime FROM UniversalTerData 
  WHERE CollTime BETWEEN @mintime AND @maxtime AND DATEDIFF(minute,@mintime,CollTime) %@interval = 0 AND TerminalID=@TerId AND TypeTableID=@typeId  ORDER BY CollTime";

            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter("@TerId",SqlDbType.Int),
                new SqlParameter("@typeId",SqlDbType.Int),
                new SqlParameter("@mintime",SqlDbType.DateTime),
                new SqlParameter("@maxtime",SqlDbType.DateTime),
                new SqlParameter("@interval",SqlDbType.Int)
            };

            parms[0].Value = TerminalID;
            parms[1].Value = typeId;
            parms[2].Value = minTime;
            parms[3].Value = maxTime;
            parms[4].Value = interval;

            using (SqlDataReader reader = SQLHelper.ExecuteReader(SQL, parms))
            {
                List<UniversalDetailDataEntity> lstData = new List<UniversalDetailDataEntity>();
                while (reader.Read())
                {
                    UniversalDetailDataEntity entity = new UniversalDetailDataEntity();

                    entity.Data = reader["DataValue"] != DBNull.Value ? Convert.ToDecimal(reader["DataValue"]) : 0;
                    entity.CollTime = reader["CollTime"] != DBNull.Value ? Convert.ToDateTime(reader["CollTime"]) : ConstValue.MinDateTime;

                    lstData.Add(entity);
                }
                return lstData;
            }
            return null;
        }

        public List<OLWQDetailDataEntity> GetOLWQDetail(string TerminalID, DateTime minTime, DateTime maxTime, int interval, int datatype)
        {
            string SQL = @"SELECT Turbidity,ResidualCl,PH,Conductivity,Temperature,CollTime FROM OLWQ_Real 
            WHERE CollTime BETWEEN @mintime AND @maxtime AND DATEDIFF(minute,@mintime,CollTime) %@interval = 0 AND TerminalIDID=@TerId  ORDER BY CollTime";

            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter("@TerId",SqlDbType.Int),
                new SqlParameter("@mintime",SqlDbType.DateTime),
                new SqlParameter("@maxtime",SqlDbType.DateTime),
                new SqlParameter("@interval",SqlDbType.Int)
            };
            parms[0].Value = TerminalID;
            parms[1].Value = minTime;
            parms[2].Value = maxTime;
            parms[3].Value = interval;

            using (SqlDataReader reader = SQLHelper.ExecuteReader(SQL, parms))
            {
                List<OLWQDetailDataEntity> lstData = new List<OLWQDetailDataEntity>();
                while (reader.Read())
                {
                    OLWQDetailDataEntity entity = new OLWQDetailDataEntity();
                    if (datatype == 0)  //浊度
                        entity.Value = reader["Turbidity"] != DBNull.Value ? Convert.ToDecimal(reader["Turbidity"]) : 0;
                    else if (datatype == 1)  //余氯
                        entity.Value = reader["ResidualCl"] != DBNull.Value ? Convert.ToDecimal(reader["ResidualCl"]) : 0;
                    else if (datatype == 2)  //PH
                        entity.Value = reader["PH"] != DBNull.Value ? Convert.ToDecimal(reader["PH"]) : 0;
                    else if (datatype == 3)  //电导率
                        entity.Value = reader["Conductivity"] != DBNull.Value ? Convert.ToDecimal(reader["Conductivity"]) : 0;
                    else if (datatype == 4)  //温度
                        entity.Value = reader["Temperature"] != DBNull.Value ? Convert.ToDecimal(reader["Temperature"]) : 0;
                    entity.CollTime = reader["CollTime"] != DBNull.Value ? Convert.ToDateTime(reader["CollTime"]) : ConstValue.MinDateTime;

                    lstData.Add(entity);
                }
                return lstData;
            }
            return null;
        }

        public string GetTerminalName(string TerminalID, TerType tertype)
        {
            string SQL = "SELECT TerminalName FROM Terminal WHERE TerminalType= '" + (int)tertype + "' AND TerminalID='" + TerminalID.Trim() + "'";
            object obj_name = SQLiteHelper.ExecuteScalar(SQL, null);
            if (obj_name != null && obj_name != DBNull.Value)
            {
                return obj_name.ToString().Trim();
            }
            return "";
        }

        public void InsertDevGPRSParm(int devId, int DevTypeId, int ctrlCode, int Funcode, string DataValue)
        {
            string SQL_Insert = @"INSERT INTO ParamToDev(DeviceId,DevTypeId,CtrlCode,FunCode,DataValue,DataLenth,SetDate,SendedFlag) VALUES(
                                @DeviceId,@DevTypeId,@CtrlCode,@FunCode,@DataValue,@DataLenth,@SetDate,@SendedFlag)";
            SqlParameter[] parms = new SqlParameter[]{
                    new SqlParameter("DeviceId",SqlDbType.Int),
                    new SqlParameter("DevTypeId",SqlDbType.Int),
                    new SqlParameter("CtrlCode",SqlDbType.Int),
                    new SqlParameter("FunCode",SqlDbType.Int),
                    new SqlParameter("DataValue",SqlDbType.VarChar,512),

                    new SqlParameter("DataLenth",SqlDbType.Int),
                    new SqlParameter("SetDate",SqlDbType.DateTime),
                    new SqlParameter("SendedFlag",SqlDbType.Int)
                };

            parms[0].Value = devId;
            parms[1].Value = DevTypeId;
            parms[2].Value = ctrlCode;
            parms[3].Value = Funcode;
            parms[4].Value = DataValue;

            parms[5].Value = DataValue.Length;
            parms[6].Value = DateTime.Now;
            parms[7].Value = 0;

            SQLHelper.ExecuteNonQuery(SQL_Insert, parms);
        }

        public List<PreDataEntity> GetPreDataTop2(List<int> terminalids)
        {
            if (terminalids == null || terminalids.Count == 0)
                return null;
            string str_ids = "";
            foreach (int id in terminalids)
            {
                str_ids += "'" + id + "',";
            }
            if (str_ids.EndsWith(","))
                str_ids = str_ids.Substring(0, str_ids.Length - 1); ;
            //MT_CollDeviceType_ID=1 是压力终端
            string SQL = string.Format(@"SELECT t.TerminalID,t.TerminalName,t.Address,v.PressValue,v.CollTime,c.PreLowLimit,c.PreUpperLimit,c.PreSlopeLowLimit,c.PreSlopeUpLimit,c.EnablePreAlarm,c.EnableSlopeAlarm FROM Pressure_Real v,PreTerConfig c,Terminal t WHERE 
                                            v.TerminalID=c.TerminalID AND t.TerminalType = {0} AND c.TerminalID=t.TerminalID AND t.TerminalID IN({1}) AND 
                                            v.CollTime IN (SELECT TOP 2 CollTime FROM Pressure_Real 
                                            WHERE v.TerminalID = TerminalID ORDER BY TerminalID,CollTime DESC)", ((int)TerType.PreTer).ToString(),str_ids);

            using (SqlDataReader reader = SQLHelper.ExecuteReader(SQL, null))
            {
                List<PreDataEntity> lstData = new List<PreDataEntity>();
                int index = -1;
                bool exist;
                int terminalid;
                DateTime coltime;
                decimal prevalue;

                while (reader.Read())
                {
                    prevalue = reader["PressValue"] != DBNull.Value ? Convert.ToDecimal(reader["PressValue"]) : 0;
                    terminalid = reader["TerminalID"] != DBNull.Value ? Convert.ToInt32(reader["TerminalID"]) : 0;
                    coltime = reader["CollTime"] != DBNull.Value ? Convert.ToDateTime(reader["CollTime"]) : DateTime.Now;
                    exist = false;
                    for (index = 0; index < lstData.Count; index++)
                    {
                        if (lstData[index].TerminalID == terminalid)
                        {
                            exist = true;
                            break;
                        }
                    }

                    if (exist)
                    {
                        if (lstData[index].NewestCollTime != ConstValue.MinDateTime)
                        {
                            if (coltime < lstData[index].NewestCollTime)
                            {
                                lstData[index].PreValueLastbutone = prevalue;
                                lstData[index].CollTimeLastbutone = coltime;
                            }
                            else
                            {
                                DateTime tmpdt = lstData[index].NewestCollTime;
                                decimal tmppredata = lstData[index].NewestPressueValue;

                                lstData[index].NewestCollTime = coltime;
                                lstData[index].NewestPressueValue = prevalue;

                                lstData[index].CollTimeLastbutone = tmpdt;
                                lstData[index].PreValueLastbutone = tmppredata;
                            }
                        }
                        else
                        {
                            lstData[index].NewestCollTime = coltime;
                            lstData[index].NewestPressueValue = prevalue;
                        }
                    }
                    else
                    {
                        PreDataEntity entity = new PreDataEntity();
                        entity.TerminalID = reader["TerminalID"] != DBNull.Value ? Convert.ToInt32(reader["TerminalID"]) : 0;
                        entity.TerminalName = reader["TerminalName"] != DBNull.Value ? reader["TerminalName"].ToString() : "";

                        entity.EnablePreAlarm = reader["EnablePreAlarm"] != DBNull.Value ? (Convert.ToInt32(reader["EnablePreAlarm"]) == 1 ? true : false) : true;
                        entity.EnableSlopeAlarm = reader["EnableSlopeAlarm"] != DBNull.Value ? (Convert.ToInt32(reader["EnableSlopeAlarm"]) == 1 ? true : false) : true;

                        if (entity.EnablePreAlarm)
                        {
                            entity.PreLowLimit = reader["PreLowLimit"] != DBNull.Value ? Convert.ToDecimal(reader["PreLowLimit"]) : 0;
                            entity.PreUpLimit = reader["PreUpperLimit"] != DBNull.Value ? Convert.ToDecimal(reader["PreUpperLimit"]) : 0;
                        }
                        if (entity.EnableSlopeAlarm)
                        {
                            entity.PreSlopeLowLimit = reader["PreSlopeLowLimit"] != DBNull.Value ? Convert.ToDecimal(reader["PreSlopeLowLimit"]) : 0;
                            entity.PreSlopeUpLimit = reader["PreSlopeUpLimit"] != DBNull.Value ? Convert.ToDecimal(reader["PreSlopeUpLimit"]) : 0;
                        }

                        entity.NewestCollTime = coltime;
                        entity.NewestPressueValue = prevalue;

                        entity.Addr = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : "";

                        lstData.Add(entity);
                    }

                }
                return lstData;
            }
            return null;
        }

        public List<FlowDataEntity> GetFlowDataTop2(List<int> terminalids)
        {
            if (terminalids == null || terminalids.Count == 0)
                return null;
            string str_ids = "";
            foreach (int id in terminalids)
            {
                str_ids += "'" + id + "',";
            }
            if (str_ids.EndsWith(","))
                str_ids = str_ids.Substring(0, str_ids.Length - 1); ;
            //MT_CollDeviceType_ID=1 是压力终端
            string SQL = string.Format(@"SELECT t.TerminalID,t.TerminalName,t.Address,f.FlowValue,f.FlowInverted,f.FlowInstant,f.CollTime,c.PreLowLimit,c.PreUpperLimit,c.PreSlopeLowLimit,c.PreSlopeUpLimit,c.EnablePreAlarm,c.EnableSlopeAlarm 
                                        FROM Flow_Real f,PreTerConfig c,Terminal t WHERE 
                                        f.TerminalID=c.TerminalID AND t.TerminalType = {0} AND c.TerminalID=t.TerminalID AND c.TerminalID IN({1}) AND 
                                        f.CollTime IN (SELECT TOP 2 CollTime FROM Flow_Real  
                                        WHERE f.TerminalID = TerminalID ORDER BY TerminalID,CollTime DESC)", ((int)TerType.PreTer).ToString(),str_ids);

            using (SqlDataReader reader = SQLHelper.ExecuteReader(SQL, null))
            {
                List<FlowDataEntity> lstData = new List<FlowDataEntity>();
                int index = -1;
                bool exist;
                int terminalid;
                DateTime coltime;
                decimal forwardflowvalue;
                decimal reverseflowvalue;
                decimal instantflowvalue;

                while (reader.Read())
                {
                    forwardflowvalue = reader["FlowValue"] != DBNull.Value ? Convert.ToDecimal(reader["FlowValue"]) : 0;
                    reverseflowvalue = reader["FlowInverted"] != DBNull.Value ? Convert.ToDecimal(reader["FlowInverted"]) : 0;
                    instantflowvalue = reader["FlowInstant"] != DBNull.Value ? Convert.ToDecimal(reader["FlowInstant"]) : 0;
                    terminalid = reader["TerminalID"] != DBNull.Value ? Convert.ToInt32(reader["TerminalID"]) : 0;
                    coltime = reader["CollTime"] != DBNull.Value ? Convert.ToDateTime(reader["CollTime"]) : DateTime.Now;
                    exist = false;
                    for (index = 0; index < lstData.Count; index++)
                    {
                        if (lstData[index].TerminalID == terminalid)
                        {
                            exist = true;
                            break;
                        }
                    }

                    if (exist)
                    {
                        if (lstData[index].NewestCollTime != ConstValue.MinDateTime)
                        {
                            if (coltime < lstData[index].NewestCollTime)
                            {
                                lstData[index].ForwardFlowLastbutone = forwardflowvalue;
                                lstData[index].ReverseFlowLastbutone = reverseflowvalue;
                                lstData[index].InstantFlowLastbutone = instantflowvalue;
                                lstData[index].CollTimeLastbutone = coltime;
                            }
                            else
                            {
                                DateTime tmpdt = lstData[index].NewestCollTime;
                                decimal tmpforwardflowdata = lstData[index].NewestForwardFlowValue;
                                decimal tmpreverseflowdata = lstData[index].NewestReverseFlowValue;
                                decimal tmpinstantflowdata = lstData[index].NewestInstantFlowValue;

                                lstData[index].NewestCollTime = coltime;
                                lstData[index].NewestForwardFlowValue = forwardflowvalue;
                                lstData[index].NewestReverseFlowValue = reverseflowvalue;
                                lstData[index].NewestInstantFlowValue = instantflowvalue;

                                lstData[index].CollTimeLastbutone = tmpdt;
                                lstData[index].ForwardFlowLastbutone = tmpforwardflowdata;
                                lstData[index].ReverseFlowLastbutone = tmpreverseflowdata;
                                lstData[index].InstantFlowLastbutone = tmpinstantflowdata;
                            }
                        }
                        else
                        {
                            lstData[index].NewestCollTime = coltime;
                            lstData[index].NewestForwardFlowValue = forwardflowvalue;
                            lstData[index].NewestReverseFlowValue = reverseflowvalue;
                            lstData[index].NewestInstantFlowValue = instantflowvalue;
                        }
                    }
                    else
                    {
                        FlowDataEntity entity = new FlowDataEntity();
                        entity.TerminalID = reader["TerminalID"] != DBNull.Value ? Convert.ToInt32(reader["TerminalID"]) : 0;
                        //entity.ConfigName = reader["Config_Name"] != DBNull.Value ? reader["Config_Name"].ToString() : "";
                        entity.TerminalName = reader["TerminalName"] != DBNull.Value ? reader["TerminalName"].ToString() : "";

                        entity.NewestCollTime = coltime;
                        entity.NewestForwardFlowValue = forwardflowvalue;
                        entity.NewestReverseFlowValue = reverseflowvalue;
                        entity.NewestInstantFlowValue = instantflowvalue;

                        entity.Addr = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : "";

                        lstData.Add(entity);
                    }

                }
                return lstData;
            }
            return null;
        }

        public DataTable GetOLWQData(List<string> terminalids)
        {
            if (terminalids == null || terminalids.Count == 0)
                return null;
            string str_ids = "";
            foreach (string id in terminalids)
            {
                str_ids += "'" + id + "',";
            }
            if (str_ids.EndsWith(","))
                str_ids = str_ids.Substring(0, str_ids.Length - 1);
            string SQL = string.Format(@"SELECT TerminalID,Turbidity,ResidualCl,PH,Conductivity,Temperature,ValueColumnName FROM OLWQ_Real WHERE ID IN(SELECT MAX(ID) FROM OLWQ_Real WHERE TerminalID IN({0}) GROUP BY ValueColumnName)", str_ids);
            DataTable dt = SQLHelper.ExecuteDataTable(SQL, null);
            string SQL_Ter = string.Format("SELECT DISTINCT TerminalID,TerminalName,'无' Turbidity,'无' ResidualCl,'无' PH,'无' Conductivity,'无' Temperature FROM Terminal WHERE TerminalID IN({0}) AND TerminalType={1}", str_ids,((int)TerType.OLWQTer).ToString());
            DataTable dt_res = SQLHelper.ExecuteDataTable(SQL_Ter, null);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string terminalid = dr["TerminalID"].ToString().Trim();
                    string valuetype = dr["ValueColumnName"].ToString().Trim();
                    DataRow[] dr_res = dt_res.Select("TerminalID='" + terminalid + "'");
                    if (dr_res != null && dr_res.Length > 0)
                    {
                        dr_res[0][valuetype] = dr[valuetype].ToString();
                    }
                }
            }

            return dt_res;
        }

    }
}
