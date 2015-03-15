using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using Entity;
using Common;

namespace DAL
{
    public class TerminalDataDAL
    {
        public DataTable GetTerID_PointID(TerType type)
        {
            string SQL = "SELECT ID,TerminalID,TerminalName FROM Terminal WHERE TerminalType='" + (int)type + "'";
            return SQLHelper.ExecuteDataTable(SQL, null);
        }
        #region GPRS数据操作
        public int InsertGPRSPreData(Queue<GPRSPreFrameDataEntity> datas)
        {
            lock (ConstValue.obj)
            {
                SqlTransaction trans = null;
                try
                {
                    DataTable dt_PointID= GetTerID_PointID(TerType.PreTer);
                    if ((dt_PointID == null)  || (dt_PointID.Rows.Count == 0))
                    {
                        return 0;
                    }

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

                    string SQL_PreData = "INSERT INTO Pressure_Real(Point_ID,PressValue,CollTime,UnloadTime,HistoryFlag) VALUES(@pointId,@prevalue,@coltime,@UploadTime,0)";
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
                        foreach (DataRow dr in dt_PointID.Rows)
                        {
                            if (dr["TerminalID"].ToString() == entity.TerId)
                            {
                                en_point_id = dr["ID"].ToString();
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
                    DataTable dt_PointID = GetTerID_PointID(TerType.PreTer);
                    if ((dt_PointID == null) || (dt_PointID.Rows.Count == 0))
                    {
                        return 0;
                    }

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

                    string SQL_PreData = "INSERT INTO Flow_Real(Point_ID,FlowValue,FlowInverted,FlowInstant,CollTime,UnloadTime,HistoryFlag) VALUES(@pointId,@flowvalue,@flowreverse,@flowinstant,@coltime,@UploadTime,0)";
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
                        foreach (DataRow dr in dt_PointID.Rows)
                        {
                            if (dr["TerminalID"].ToString() == entity.TerId)
                            {
                                en_point_id = dr["ID"].ToString();
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
    }
}
