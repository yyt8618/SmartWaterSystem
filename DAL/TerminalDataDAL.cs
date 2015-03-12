using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using Entity;
using Common;

namespace DAL
{
    public class TerminalDataDAL
    {
        public List<PreDataEntity> GetPreData(List<string> terminalIds)
        {
            if(terminalIds==null || terminalIds.Count ==0)
                return null;
            lock (ConstValue.obj)
            {
                string str_ids = "";
                foreach (string id in terminalIds)
                {
                    str_ids += "'" + id + "',";
                }
                if (str_ids.EndsWith(","))
                    str_ids = str_ids.Substring(0, str_ids.Length - 1); ;
                //MT_CollDeviceType_ID=1 是压力终端
                string SQL = @"SELECT A.TrmlID,A.Config_Name,B.PressValue,B.CollTime,B.UnloadTime,C.TerminalName FROM EN_TrmlCollRelate A,DL_Pressure_Real B,TerminalConfig C 
                        WHERE A.MT_CollDeviceType_ID=1 AND A.EN_CollPoint_ID=B.EN_CollPoint_ID AND A.TrmlID IN(" + str_ids + ") ORDER BY TrmlID,UnloadTime DESC";

                using (SqlDataReader reader = SQLHelper.ExecuteReader(SQL, null))
                {
                    List<PreDataEntity> lstData = new List<PreDataEntity>();
                    while (reader.Read())
                    {
                        PreDataEntity entity = new PreDataEntity();

                        entity.TerminalID = reader["TrmlID"] != DBNull.Value ? Convert.ToInt32(reader["TrmlID"]) : 0;
                        entity.ConfigName = reader["Config_Name"] != DBNull.Value ? reader["Config_Name"].ToString() : "";
                        entity.PressueValue = reader["PressValue"] != DBNull.Value ? Convert.ToDecimal(reader["PressValue"]) : 0;
                        entity.CollTime = reader["CollTime"] != DBNull.Value ? Convert.ToDateTime(reader["CollTime"]) : DateTime.Now;
                        entity.UploadTime = reader["UnloadTime"] != DBNull.Value ? Convert.ToDateTime(reader["UnloadTime"]) : DateTime.Now;

                        lstData.Add(entity);
                    }
                    return lstData;
                }
                return null;
            }
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
            string SQL = string.Format(@"SELECT r.TrmlID,r.Config_Name,v.PressValue,v.CollTime,c.TerminalName,c.PreLowLimit,c.PreUpperLimit,c.PreSlopeLowLimit,c.PreSlopeUpLimit,c.Address,c.EnablePreAlarm,c.EnableSlopeAlarm FROM DL_Pressure_Real v,EN_TrmlCollRelate r,TerminalConfig c WHERE 
                                            v.EN_CollPoint_ID=r.EN_CollPoint_ID AND R.MT_CollDeviceType_ID=1 AND r.TrmlID=c.TerminalID AND c.TerminalID IN({0}) AND 
                                            v.CollTime IN (SELECT TOP 2 CollTime FROM DL_Pressure_Real 
                                            WHERE v.EN_CollPoint_ID = EN_CollPoint_ID ORDER BY EN_CollPoint_ID,CollTime DESC)", str_ids);

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
                    terminalid = reader["TrmlID"] != DBNull.Value ? Convert.ToInt32(reader["TrmlID"]) : 0;
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
                        entity.TerminalID = reader["TrmlID"] != DBNull.Value ? Convert.ToInt32(reader["TrmlID"]) : 0;
                        entity.ConfigName = reader["Config_Name"] != DBNull.Value ? reader["Config_Name"].ToString() : "";
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

        public List<PreDetailDataEntity> GetDetail(string TerminalID, DateTime minTime, DateTime maxTime,int interval)
        {
            string SQL = @"SELECT PressValue,CollTime FROM DL_Pressure_Real 
            WHERE CollTime BETWEEN @mintime AND @maxtime AND DATEDIFF(minute,@mintime,CollTime) %@interval = 0 AND EN_CollPoint_ID IN (SELECT EN_CollPoint_ID FROM  EN_TrmlCollRelate WHERE  TrmlID=@TerId)  ORDER BY CollTime";

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

        #region GPRS数据操作
        public int InsertGPRSPreData(Queue<GPRSPreFrameDataEntity> datas)
        {
            lock (ConstValue.obj)
            {
                SqlTransaction trans = null;
                try
                {
                    //获取所有终端的En_Point_ID 从EN_TrmlCollRelate表
                    TerminalConfigDAL configdal = new TerminalConfigDAL();
                    Hashtable terId_point_Id = configdal.GetTerID_PointID();
                    if (terId_point_Id == null)
                    {
                        return 0;
                    }

                    trans = SQLHelper.GetTransaction();

                    string SQL_Frame = "INSERT INTO DL_Frame(Dir,Frame,LogTime) VALUES(@dir,@frame,@logtime)";
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

                    string SQL_PreData = "INSERT INTO DL_Pressure_Real(EN_CollPoint_ID,PressValue,CollTime,UnloadTime,HistoryFlag) VALUES(@pointId,@prevalue,@coltime,@UploadTime,0)";
                    SqlParameter[] parms_predata = new SqlParameter[]{
                    new SqlParameter("@pointId",SqlDbType.Int),
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

                    string en_point_id = "";
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

                        en_point_id = "";
                        foreach (DictionaryEntry entry in terId_point_Id)
                        {
                            if (entry.Key.ToString() == entity.TerId)
                            {
                                en_point_id = entry.Value.ToString();
                                break;
                            }
                        }
                        if (!string.IsNullOrEmpty(en_point_id))
                        {
                            for (int i = 0; i < entity.lstPreData.Count; i++)
                            {
                                parms_predata[0].Value = en_point_id;
                                parms_predata[1].Value = entity.lstPreData[i].PreValue;
                                parms_predata[2].Value = entity.lstPreData[i].ColTime;
                                parms_predata[3].Value = entity.ModifyTime;

                                command_predata.ExecuteNonQuery();
                            }
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

        public int InsertGPRSFlowData(Queue<GPRSFlowFrameDataEntity> datas)
        {
            lock (ConstValue.obj)
            {
                SqlTransaction trans = null;
                try
                {
                    //获取所有终端的En_Point_ID 从EN_TrmlCollRelate表
                    TerminalConfigDAL configdal = new TerminalConfigDAL();
                    Hashtable terId_point_Id = configdal.GetTerID_PointID();
                    if (terId_point_Id == null)
                    {
                        return 0;
                    }

                    trans = SQLHelper.GetTransaction();

                    string SQL_Frame = "INSERT INTO DL_Frame(Dir,Frame,LogTime) VALUES(@dir,@frame,@logtime)";
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

                    string SQL_PreData = "INSERT INTO DL_Flow_Real(EN_CollPoint_ID,FlowValue,FlowInverted,FlowInstant,CollTime,UnloadTime,HistoryFlag) VALUES(@pointId,@flowvalue,@flowreverse,@flowinstant,@coltime,@UploadTime,0)";
                    SqlParameter[] parms_predata = new SqlParameter[]{
                    new SqlParameter("@pointId",SqlDbType.Int),
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

                    string en_point_id = "";
                    while (datas.Count > 0)
                    {
                        GPRSFlowFrameDataEntity entity = datas.Dequeue();
                        parms_frame[0].Value = 1;
                        parms_frame[1].Value = entity.Frame;
                        parms_frame[2].Value = entity.ModifyTime;

                        command_frame.ExecuteNonQuery();

                        en_point_id = "";
                        foreach (DictionaryEntry entry in terId_point_Id)
                        {
                            if (entry.Key.ToString() == entity.TerId)
                            {
                                en_point_id = entry.Value.ToString();
                                break;
                            }
                        }
                        if (!string.IsNullOrEmpty(en_point_id))
                        {
                            for (int i = 0; i < entity.lstFlowData.Count; i++)
                            {
                                parms_predata[0].Value = en_point_id;
                                parms_predata[1].Value = entity.lstFlowData[i].Forward_FlowValue;
                                parms_predata[2].Value = entity.lstFlowData[i].Reverse_FlowValue;
                                parms_predata[3].Value = entity.lstFlowData[i].Instant_FlowValue;
                                parms_predata[4].Value = entity.lstFlowData[i].ColTime;
                                parms_predata[5].Value = entity.ModifyTime;

                                command_predata.ExecuteNonQuery();
                            }
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
                string SQL = "SELECT ID,DeviceId,DevTypeId,CtrlCode,FunCode,DataValue,DataLenth,SetDate FROM DL_ParamToDev WHERE SendedFlag = 0";
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

                    string SQL = "UPDATE DL_ParamToDev SET SendedFlag=1 WHERE ID=@id";
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
    }
}
