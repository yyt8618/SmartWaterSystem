using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using Entity;
using Common;
using System.Collections.Concurrent;

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
        public int InsertGPRSPreData(ConcurrentQueue<GPRSPreFrameDataEntity> datas)
        {
            lock (ConstValue.obj)
            {
                //SqlTransaction trans = null;
                try
                {
                    //trans = SQLHelper.GetTransaction();
                    using (SqlCommand command_frame = new SqlCommand())
                    {
                        string SQL_Frame = "INSERT INTO Frame(Dir,Frame,LogTime) VALUES(@dir,@frame,@logtime)";
                        SqlParameter[] parms_frame = new SqlParameter[]{
                new SqlParameter("@dir",SqlDbType.Int),
                new SqlParameter("@frame",SqlDbType.VarChar,2000),
                new SqlParameter("@logtime",SqlDbType.DateTime)
            };
                        //SqlCommand command_frame = new SqlCommand();
                        command_frame.CommandText = SQL_Frame;
                        command_frame.Parameters.AddRange(parms_frame);
                        command_frame.CommandType = CommandType.Text;
                        command_frame.Connection = SQLHelper.Conn;
                        //command_frame.Transaction = trans;

                        using (SqlCommand command_predata = new SqlCommand())
                        {
                            string SQL_PreData = "INSERT INTO Pressure_Real(TerminalID,PressValue,CollTime,UnloadTime,HistoryFlag,Voltage,FieldStrength) VALUES(@TerId,@prevalue,@coltime,@UploadTime,0,@Voltage,@fieldstrength)";
                            SqlParameter[] parms_predata = new SqlParameter[]{
                    new SqlParameter("@TerId",SqlDbType.Int),
                    new SqlParameter("@prevalue",SqlDbType.Decimal),
                    new SqlParameter("@coltime",SqlDbType.DateTime),
                    new SqlParameter("@UploadTime",SqlDbType.DateTime),
                    new SqlParameter("@Voltage",SqlDbType.Decimal),
                    new SqlParameter("@fieldstrength",SqlDbType.SmallInt)
                };
                            //SqlCommand command_predata = new SqlCommand();
                            command_predata.CommandText = SQL_PreData;
                            command_predata.Parameters.AddRange(parms_predata);
                            command_predata.CommandType = CommandType.Text;
                            command_predata.Connection = SQLHelper.Conn;
                            //command_predata.Transaction = trans;

                            //string en_point_id = "";
                            while (datas.Count > 0)
                            {
                                GPRSPreFrameDataEntity entity = null;
                                try
                                {
                                    if (datas.TryDequeue(out entity))
                                    {
                                        parms_frame[0].Value = 1;
                                        parms_frame[1].Value = entity.Frame;
                                        parms_frame[2].Value = entity.ModifyTime;

                                        command_frame.ExecuteNonQuery();

                                        for (int i = 0; i < entity.lstPreData.Count; i++)
                                        {
                                            parms_predata[0].Value = entity.TerId;
                                            parms_predata[1].Value = entity.lstPreData[i].PreValue;
                                            parms_predata[2].Value = entity.lstPreData[i].ColTime;
                                            parms_predata[3].Value = entity.ModifyTime;
                                            parms_predata[4].Value = entity.lstPreData[i].Voltage;
                                            parms_predata[5].Value = entity.lstPreData[i].FieldStrength;

                                            command_predata.ExecuteNonQuery();
                                        }
                                        entity = null;
                                    }
                                }
                                catch (SqlException sqlex)
                                {
                                    throw sqlex;
                                }
                                catch (Exception iex)
                                {
                                    //if (entity != null)
                                    //    datas.Enqueue(entity);  //将移除队列的数据放回
                                    throw iex;
                                }
                            }
                        }
                        //trans.Commit();

                    }
                    return 1;
                }
                catch (Exception ex)
                {
                    //if (trans != null)
                    //    trans.Rollback();
                    throw ex;
                }
            }
        }

        public int InsertGPRSFlowData(ConcurrentQueue<GPRSFlowFrameDataEntity> datas)
        {
            lock (ConstValue.obj)
            {
                //SqlTransaction trans = null;
                try
                {
                    //trans = SQLHelper.GetTransaction();
                    using (SqlCommand command_frame = new SqlCommand())
                    {
                        string SQL_Frame = "INSERT INTO Frame(Dir,Frame,LogTime) VALUES(@dir,@frame,@logtime)";
                        SqlParameter[] parms_frame = new SqlParameter[]{
                new SqlParameter("@dir",SqlDbType.Int),
                new SqlParameter("@frame",SqlDbType.VarChar,2000),
                new SqlParameter("@logtime",SqlDbType.DateTime)
            };
                        //SqlCommand command_frame = new SqlCommand();
                        command_frame.CommandText = SQL_Frame;
                        command_frame.Parameters.AddRange(parms_frame);
                        command_frame.CommandType = CommandType.Text;
                        command_frame.Connection = SQLHelper.Conn;
                        //command_frame.Transaction = trans;

                        using (SqlCommand command_predata = new SqlCommand())
                        {
                            string SQL_PreData = "INSERT INTO Flow_Real(TerminalID,FlowValue,FlowInverted,FlowInstant,CollTime,UnloadTime,HistoryFlag,Voltage,FieldStrength) VALUES(@TerId,@flowvalue,@flowreverse,@flowinstant,@coltime,@UploadTime,0,@Voltage,@fieldstrength)";
                            SqlParameter[] parms_predata = new SqlParameter[]{
                    new SqlParameter("@TerId",SqlDbType.Int),
                    new SqlParameter("@flowvalue",SqlDbType.Decimal),
                    new SqlParameter("@flowreverse",SqlDbType.Decimal),
                    new SqlParameter("@flowinstant",SqlDbType.Decimal),
                    new SqlParameter("@coltime",SqlDbType.DateTime),
                    new SqlParameter("@UploadTime",SqlDbType.DateTime),
                    new SqlParameter("@Voltage",SqlDbType.Decimal),
                    new SqlParameter("@fieldstrength",SqlDbType.SmallInt)
                };
                            //SqlCommand command_predata = new SqlCommand();
                            command_predata.CommandText = SQL_PreData;
                            command_predata.Parameters.AddRange(parms_predata);
                            command_predata.CommandType = CommandType.Text;
                            command_predata.Connection = SQLHelper.Conn;
                            //command_predata.Transaction = trans;

                            //string en_point_id = "";
                            while (datas.Count > 0)
                            {
                                GPRSFlowFrameDataEntity entity = null;
                                try
                                {
                                    if (datas.TryDequeue(out entity))
                                    {
                                        if (!string.IsNullOrEmpty(entity.Frame))
                                        {
                                            parms_frame[0].Value = 1;
                                            parms_frame[1].Value = entity.Frame;
                                            parms_frame[2].Value = entity.ModifyTime;

                                            command_frame.ExecuteNonQuery();
                                        }

                                        for (int i = 0; i < entity.lstFlowData.Count; i++)
                                        {
                                            parms_predata[0].Value = entity.TerId;
                                            parms_predata[1].Value = entity.lstFlowData[i].Forward_FlowValue;
                                            parms_predata[2].Value = entity.lstFlowData[i].Reverse_FlowValue;
                                            parms_predata[3].Value = entity.lstFlowData[i].Instant_FlowValue;
                                            parms_predata[4].Value = entity.lstFlowData[i].ColTime;
                                            parms_predata[5].Value = entity.ModifyTime;
                                            parms_predata[6].Value = entity.lstFlowData[i].Voltage;
                                            parms_predata[7].Value = entity.lstFlowData[i].FieldStrength;

                                            command_predata.ExecuteNonQuery();
                                        }
                                        entity = null;
                                    }
                                }
                                catch (Exception iex)
                                {
                                    //if (entity != null)
                                    //    datas.Enqueue(entity);
                                    throw iex;
                                }
                            }
                        }
                        //trans.Commit();
                    }
                        return 1;
                }
                catch (Exception ex)
                {
                    //if (trans != null)
                    //    trans.Rollback();
                    throw ex;
                }
            }
        }

        public int InsertGPRSPrectrlData(ConcurrentQueue<GPRSPrectrlFrameDataEntity> datas)
        {
            lock (ConstValue.obj)
            {
                //SqlTransaction trans = null;
                try
                {
                    //trans = SQLHelper.GetTransaction();
                    using (SqlCommand command_frame = new SqlCommand())
                    {
                        string SQL_Frame = "INSERT INTO Frame(Dir,Frame,LogTime) VALUES(@dir,@frame,@logtime)";
                        SqlParameter[] parms_frame = new SqlParameter[]{
                new SqlParameter("@dir",SqlDbType.Int),
                new SqlParameter("@frame",SqlDbType.VarChar,2000),
                new SqlParameter("@logtime",SqlDbType.DateTime)
            };
                        //SqlCommand command_frame = new SqlCommand();
                        command_frame.CommandText = SQL_Frame;
                        command_frame.Parameters.AddRange(parms_frame);
                        command_frame.CommandType = CommandType.Text;
                        command_frame.Connection = SQLHelper.Conn;
                        //command_frame.Transaction = trans;

                        using (SqlCommand command_predata = new SqlCommand())
                        {
                            string SQL_PrectrlData = "INSERT INTO PreCtrl_Real(TerminalID,Entrance_pre,Outlet_pre,FlowValue,FlowInverted,FlowInstant,AlarmCode,AlarmDesc,CollTime,UnloadTime,Voltage,FieldStrength) VALUES(@TerId,@enprevalue,@outletprevalue,@flowvalue,@flowreverse,@flowinstant,@alarmcode,@alarmdesc,@coltime,@UploadTime,@Voltage,@fieldstrength)";
                            SqlParameter[] parms_predata = new SqlParameter[]{
                    new SqlParameter("@TerId",SqlDbType.Int),
                    new SqlParameter("@enprevalue",SqlDbType.Decimal),
                    new SqlParameter("@outletprevalue",SqlDbType.Decimal),
                    new SqlParameter("@flowvalue",SqlDbType.Decimal),
                    new SqlParameter("@flowreverse",SqlDbType.Decimal),

                    new SqlParameter("@flowinstant",SqlDbType.Decimal),
                     new SqlParameter("@alarmcode",SqlDbType.TinyInt),
                    new SqlParameter("@alarmdesc",SqlDbType.NVarChar),
                    new SqlParameter("@coltime",SqlDbType.DateTime),
                    new SqlParameter("@UploadTime",SqlDbType.DateTime),

                    new SqlParameter("@Voltage",SqlDbType.Decimal),
                    new SqlParameter("@fieldstrength",SqlDbType.SmallInt)
                };
                            //SqlCommand command_predata = new SqlCommand();
                            command_predata.CommandText = SQL_PrectrlData;
                            command_predata.Parameters.AddRange(parms_predata);
                            command_predata.CommandType = CommandType.Text;
                            command_predata.Connection = SQLHelper.Conn;
                            //command_predata.Transaction = trans;

                            //string en_point_id = "";
                            while (datas.Count > 0)
                            {
                                GPRSPrectrlFrameDataEntity entity = null;
                                try
                                {
                                    if (datas.TryDequeue(out entity))
                                    {
                                        if (!string.IsNullOrEmpty(entity.Frame))
                                        {
                                            parms_frame[0].Value = 1;
                                            parms_frame[1].Value = entity.Frame;
                                            parms_frame[2].Value = entity.ModifyTime;

                                            command_frame.ExecuteNonQuery();
                                        }

                                        for (int i = 0; i < entity.lstPrectrlData.Count; i++)
                                        {
                                            parms_predata[0].Value = entity.TerId;
                                            parms_predata[1].Value = entity.lstPrectrlData[i].Entrance_preValue;
                                            parms_predata[2].Value = entity.lstPrectrlData[i].Outlet_preValue;
                                            parms_predata[3].Value = entity.lstPrectrlData[i].Forward_FlowValue;
                                            parms_predata[4].Value = entity.lstPrectrlData[i].Reverse_FlowValue;

                                            parms_predata[5].Value = entity.lstPrectrlData[i].Instant_FlowValue;
                                            parms_predata[6].Value = entity.lstPrectrlData[i].AlarmCode;
                                            parms_predata[7].Value = entity.lstPrectrlData[i].AlarmDesc;
                                            parms_predata[8].Value = entity.lstPrectrlData[i].ColTime;
                                            parms_predata[9].Value = DateTime.Now;

                                            if (entity.lstPrectrlData[i].Voltage == -1)
                                                parms_predata[10].Value = DBNull.Value;
                                            else
                                                parms_predata[10].Value = entity.lstPrectrlData[i].Voltage;
                                            parms_predata[11].Value = entity.lstPrectrlData[i].FieldStrength;

                                            command_predata.ExecuteNonQuery();
                                        }
                                        entity = null;
                                    }
                                }
                                catch (Exception iex)
                                {
                                    //if (entity != null)
                                    //    datas.Enqueue(entity);
                                    throw iex;
                                }
                            }
                        }
                        //trans.Commit();
                    }
                    return 1;
                }
                catch (Exception ex)
                {
                    //if (trans != null)
                    //    trans.Rollback();
                    throw ex;
                }
            }
        }

        public int InsertGPRSUniversalData(ConcurrentQueue<GPRSUniversalFrameDataEntity> datas)
        {
            lock (ConstValue.obj)
            {
                //SqlTransaction trans = null;
                try
                {
                    //trans = SQLHelper.GetTransaction();
                    using (SqlCommand command_frame = new SqlCommand())
                    {
                        string SQL_Frame = "INSERT INTO Frame(Dir,Frame,LogTime) VALUES(@dir,@frame,@logtime)";
                        SqlParameter[] parms_frame = new SqlParameter[]{
                new SqlParameter("@dir",SqlDbType.Int),
                new SqlParameter("@frame",SqlDbType.VarChar,2000),
                new SqlParameter("@logtime",SqlDbType.DateTime)
            };
                        //SqlCommand command_frame = new SqlCommand();
                        command_frame.CommandText = SQL_Frame;
                        command_frame.Parameters.AddRange(parms_frame);
                        command_frame.CommandType = CommandType.Text;
                        command_frame.Connection = SQLHelper.Conn;
                        //command_frame.Transaction = trans;

                        using (SqlCommand command_predata = new SqlCommand())
                        {
                            string SQL_Data = @"INSERT INTO UniversalTerData([TerminalID],[DataValue],[Simulate1Zero],[Simulate2Zero],[CollTime],[UnloadTime],TypeTableID) 
                                            VALUES(@terId,@datavalue,@sim1zero,@sim2zero,@coltime,@UploadTime,@tableid)";
                            SqlParameter[] parms_data = new SqlParameter[]{
                    new SqlParameter("@terId",SqlDbType.Int),
                    new SqlParameter("@datavalue",SqlDbType.Float),
                    new SqlParameter("@sim1zero",SqlDbType.Float),
                    new SqlParameter("@sim2zero",SqlDbType.Float),
                    new SqlParameter("@coltime",SqlDbType.DateTime),

                    new SqlParameter("@UploadTime",SqlDbType.DateTime),
                    new SqlParameter("@tableid",SqlDbType.Int)
                };
                            //SqlCommand command_predata = new SqlCommand();
                            command_predata.CommandText = SQL_Data;
                            command_predata.Parameters.AddRange(parms_data);
                            command_predata.CommandType = CommandType.Text;
                            command_predata.Connection = SQLHelper.Conn;
                            //command_predata.Transaction = trans;

                            while (datas.Count > 0)
                            {
                                GPRSUniversalFrameDataEntity entity = null;
                                try
                                {
                                    if (datas.TryDequeue(out entity))
                                    {
                                        parms_frame[0].Value = 1;
                                        parms_frame[1].Value = entity.Frame;
                                        parms_frame[2].Value = entity.ModifyTime;

                                        command_frame.ExecuteNonQuery();

                                        for (int i = 0; i < entity.lstData.Count; i++)
                                        {
                                            parms_data[0].Value = entity.TerId;
                                            parms_data[1].Value = entity.lstData[i].DataValue.ToString("f2");
                                            parms_data[2].Value = entity.lstData[i].Sim1Zero;
                                            parms_data[3].Value = entity.lstData[i].Sim2Zero;
                                            parms_data[4].Value = entity.lstData[i].ColTime;

                                            parms_data[5].Value = entity.ModifyTime;
                                            parms_data[6].Value = entity.lstData[i].TypeTableID;

                                            command_predata.ExecuteNonQuery();
                                        }
                                        entity = null;
                                    }
                                }
                                catch (Exception iex)
                                {
                                    //if (entity != null)
                                    //    datas.Enqueue(entity);
                                    throw iex;
                                }
                            }
                        }
                        //trans.Commit();
                    }
                    return 1;
                }
                catch (Exception ex)
                {
                    //if (trans != null)
                    //    trans.Rollback();
                    throw ex;
                }
            }
        }

        public int InsertGPRSOLWQData(ConcurrentQueue<GPRSOLWQFrameDataEntity> datas)
        {
            lock (ConstValue.obj)
            {
                //SqlTransaction trans = null;
                
                try
                {
                    //trans = SQLHelper.GetTransaction();
                    using (SqlCommand command_frame = new SqlCommand())
                    {
                        string SQL_Frame = "INSERT INTO Frame(Dir,Frame,LogTime) VALUES(@dir,@frame,@logtime)";
                        SqlParameter[] parms_frame = new SqlParameter[]{
                new SqlParameter("@dir",SqlDbType.Int),
                new SqlParameter("@frame",SqlDbType.VarChar,2000),
                new SqlParameter("@logtime",SqlDbType.DateTime)
            };
                        //SqlCommand command_frame = new SqlCommand();
                        command_frame.CommandText = SQL_Frame;
                        command_frame.Parameters.AddRange(parms_frame);
                        command_frame.CommandType = CommandType.Text;
                        command_frame.Connection = SQLHelper.Conn;
                        //command_frame.Transaction = trans;

                        using (SqlCommand command_data = new SqlCommand())
                        {
                            string SQL_Data = "INSERT INTO OLWQ_Real(TerminalID,Turbidity,ResidualCl,PH,Conductivity,Temperature,CollTime,UnloadTime,ValueColumnName,Voltage) VALUES(@TerId,@Turbidity,@ResidualCl,@PH,@Cond,@Temp,@coltime,@UploadTime,@valuename,@Voltage)";
                            SqlParameter[] parms_predata = new SqlParameter[]{
                    new SqlParameter("@TerId",SqlDbType.Int),
                    new SqlParameter("@Turbidity",SqlDbType.Decimal),
                    new SqlParameter("@ResidualCl",SqlDbType.Decimal),
                    new SqlParameter("@PH",SqlDbType.Decimal),
                    new SqlParameter("@Cond",SqlDbType.Decimal),

                    new SqlParameter("@Temp",SqlDbType.Decimal),
                    new SqlParameter("@coltime",SqlDbType.DateTime),
                    new SqlParameter("@UploadTime",SqlDbType.DateTime),
                    new SqlParameter("@valuename",SqlDbType.NVarChar),
                    new SqlParameter("@Voltage",SqlDbType.Decimal)
                };
                            //SqlCommand command_data = new SqlCommand();
                            command_data.CommandText = SQL_Data;
                            command_data.Parameters.AddRange(parms_predata);
                            command_data.CommandType = CommandType.Text;
                            command_data.Connection = SQLHelper.Conn;
                            //command_data.Transaction = trans;

                            while (datas.Count > 0)
                            {
                                GPRSOLWQFrameDataEntity entity = null;
                                try
                                {
                                    if (datas.TryDequeue(out entity))
                                    {
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
                                            parms_predata[8].Value = entity.lstOLWQData[i].ValueColumnName;
                                            parms_predata[9].Value = entity.lstOLWQData[i].Voltage;

                                            command_data.ExecuteNonQuery();
                                        }
                                        entity = null;
                                    }
                                }
                                catch (Exception iex)
                                {
                                    //if (entity != null)
                                    //    datas.Enqueue(entity);
                                    throw iex;
                                }
                            }
                        }
                        //trans.Commit();
                    }
                    return 1;
                }
                catch (Exception ex)
                {
                    //if (trans != null)
                    //    trans.Rollback();
                    throw ex;
                }
            }
        }

        public int InsertGPRSHydrantData(ConcurrentQueue<GPRSHydrantFrameDataEntity> datas)
        {
            lock (ConstValue.obj)
            {
                //SqlTransaction trans = null;
                try
                {
                    //trans = SQLHelper.GetTransaction();
                    using (SqlCommand command_frame = new SqlCommand())
                    {
                        string SQL_Frame = "INSERT INTO Frame(Dir,Frame,LogTime) VALUES(@dir,@frame,@logtime)";
                        SqlParameter[] parms_frame = new SqlParameter[]{
                new SqlParameter("@dir",SqlDbType.Int),
                new SqlParameter("@frame",SqlDbType.VarChar,2000),
                new SqlParameter("@logtime",SqlDbType.DateTime)
            };
                        //SqlCommand command_frame = new SqlCommand();
                        command_frame.CommandText = SQL_Frame;
                        command_frame.Parameters.AddRange(parms_frame);
                        command_frame.CommandType = CommandType.Text;
                        command_frame.Connection = SQLHelper.Conn;
                        //command_frame.Transaction = trans;

                        using (SqlCommand command_predata = new SqlCommand())
                        {
                            string SQL_Data = "INSERT INTO Hydrant_Real(TerminalID,Operate,PressValue,OpenAngle,CollTime,UnloadTime) VALUES(@TerId,@opt,@prevalue,@openangle,@coltime,@UploadTime)";
                            SqlParameter[] parms_data = new SqlParameter[]{
                    new SqlParameter("@TerId",SqlDbType.Int),
                    new SqlParameter("@opt",SqlDbType.Int),
                    new SqlParameter("@prevalue",SqlDbType.Decimal),
                    new SqlParameter("@openangle",SqlDbType.Decimal),
                    new SqlParameter("@coltime",SqlDbType.DateTime),

                    new SqlParameter("@UploadTime",SqlDbType.DateTime)
                };
                            //SqlCommand command_predata = new SqlCommand();
                            command_predata.CommandText = SQL_Data;
                            command_predata.Parameters.AddRange(parms_data);
                            command_predata.CommandType = CommandType.Text;
                            command_predata.Connection = SQLHelper.Conn;
                            //command_predata.Transaction = trans;

                            while (datas.Count > 0)
                            {
                                GPRSHydrantFrameDataEntity entity = null;
                                try
                                {
                                    if (datas.TryDequeue(out entity))
                                    {
                                        parms_frame[0].Value = 1;
                                        parms_frame[1].Value = entity.Frame;
                                        parms_frame[2].Value = entity.ModifyTime;

                                        command_frame.ExecuteNonQuery();

                                        for (int i = 0; i < entity.lstHydrantData.Count; i++)
                                        {
                                            parms_data[0].Value = entity.TerId;
                                            parms_data[1].Value = entity.lstHydrantData[i].Operate;
                                            parms_data[2].Value = entity.lstHydrantData[i].PreValue;
                                            parms_data[3].Value = entity.lstHydrantData[i].OpenAngle;
                                            parms_data[4].Value = entity.lstHydrantData[i].ColTime;
                                            parms_data[5].Value = entity.ModifyTime;

                                            command_predata.ExecuteNonQuery();
                                        }
                                        entity = null;
                                    }
                                }
                                catch (Exception iex)
                                {
                                    //if (entity != null)
                                    //    datas.Enqueue(entity);
                                    throw iex;
                                }
                            }
                        }
                        //trans.Commit();
                    }
                    return 1;
                }
                catch (Exception ex)
                {
                    //if (trans != null)
                    //    trans.Rollback();
                    throw ex;
                }
            }
        }

        public int InsertWaterworkerData(ConcurrentQueue<GPRSWaterWorkerFrameDataEntity> datas)
        {
            lock (ConstValue.obj)
            {
                //SqlTransaction trans = null;
                
                try
                {
                    //trans = SQLHelper.GetTransaction();
                    using (SqlCommand command_frame = new SqlCommand())
                    {
                        string SQL_Frame = "INSERT INTO Frame(Dir,Frame,LogTime) VALUES(@dir,@frame,@logtime)";
                        SqlParameter[] parms_frame = new SqlParameter[]{
                new SqlParameter("@dir",SqlDbType.Int),
                new SqlParameter("@frame",SqlDbType.VarChar,2000),
                new SqlParameter("@logtime",SqlDbType.DateTime)
            };
                        //SqlCommand command_frame = new SqlCommand();
                        command_frame.CommandText = SQL_Frame;
                        command_frame.Parameters.AddRange(parms_frame);
                        command_frame.CommandType = CommandType.Text;
                        command_frame.Connection = SQLHelper.Conn;
                        //command_frame.Transaction = trans;
                        using (SqlCommand command_predata = new SqlCommand())
                        {
                            string SQL_Data = @"INSERT INTO Waterworker_Real([TrmlID],[TrmlName],[ActivePower_No1],[ReActivePower_No1],[ActivePower_No2],[ReActivePower_No2],[ActivePower_No3],[ReActivePower_No3],[ActivePower_No4],[ReActivePower_No4],[OutPressure],[LevelMeter],[FlowMeter_No1],[FlowMeter_No2],[SwitchStatu_No1],[SwitchStatu_No2],[SwitchStatu_No3],[SwitchStatu_No4],[Current1],[Current2],[Current3],[Current4],[Freq1],[Freq2],[Freq3],[Freq4],[CollTime],[UnloadTime]) 
                                VALUES(@TerId,@TerName,@activepower1,@reactivepower1,@activepower2,@reactivepower2,@activepower3,@reactivepower3,@activepower4,@reactivepower4,@prevalue,@liquidlevel,@flowvalue1,@flowvalue2,@switch1,@switch2,@switch3,@switch4,@current1,@current2,@current3,@current4,@freq1,@freq2,@freq3,@freq4,@colltime,@UploadTime)";
                            SqlParameter[] parms_data = new SqlParameter[]{
                    new SqlParameter("@TerId",SqlDbType.VarChar,50),
                    new SqlParameter("@TerName",SqlDbType.VarChar,50),
                    new SqlParameter("@activepower1",SqlDbType.Decimal),
                    new SqlParameter("@reactivepower1",SqlDbType.Decimal),
                    new SqlParameter("@activepower2",SqlDbType.Decimal),

                    new SqlParameter("@reactivepower2",SqlDbType.Decimal),
                    new SqlParameter("@activepower3",SqlDbType.Decimal),
                    new SqlParameter("@reactivepower3",SqlDbType.Decimal),
                    new SqlParameter("@activepower4",SqlDbType.Decimal),
                    new SqlParameter("@reactivepower4",SqlDbType.Decimal),

                    new SqlParameter("@prevalue",SqlDbType.Decimal),
                    new SqlParameter("@liquidlevel",SqlDbType.Decimal),
                    new SqlParameter("@flowvalue1",SqlDbType.Decimal),
                    new SqlParameter("@flowvalue2",SqlDbType.Decimal),
                    new SqlParameter("@switch1",SqlDbType.Bit),

                    new SqlParameter("@switch2",SqlDbType.Bit),
                    new SqlParameter("@switch3",SqlDbType.Bit),
                    new SqlParameter("@switch4",SqlDbType.Bit),
                    new SqlParameter("@current1",SqlDbType.Decimal),
                    new SqlParameter("@current2",SqlDbType.Decimal),

                    new SqlParameter("@current3",SqlDbType.Decimal),
                    new SqlParameter("@current4",SqlDbType.Decimal),
                    new SqlParameter("@freq1",SqlDbType.Decimal),
                    new SqlParameter("@freq2",SqlDbType.Decimal),
                    new SqlParameter("@freq3",SqlDbType.Decimal),

                    new SqlParameter("@freq4",SqlDbType.Decimal),
                    new SqlParameter("@colltime",SqlDbType.DateTime),
                    new SqlParameter("@UploadTime",SqlDbType.DateTime)
                };
                            //SqlCommand command_predata = new SqlCommand();
                            command_predata.CommandText = SQL_Data;
                            command_predata.Parameters.AddRange(parms_data);
                            command_predata.CommandType = CommandType.Text;
                            command_predata.Connection = SQLHelper.Conn;
                            //command_predata.Transaction = trans;

                            while (datas.Count > 0)
                            {
                                GPRSWaterWorkerFrameDataEntity entity = null;
                                try
                                {
                                    if (datas.TryDequeue(out entity))
                                    {
                                        parms_frame[0].Value = 1;
                                        parms_frame[1].Value = entity.Frame;
                                        parms_frame[2].Value = entity.ModifyTime;

                                        command_frame.ExecuteNonQuery();

                                        parms_data[0].Value = entity.TerId;
                                        parms_data[1].Value = "";
                                        if (entity.TerId == "1")  //名称写死 16-8-31
                                            parms_data[1].Value = "一水厂一泵房";
                                        else if (entity.TerId == "2")
                                            parms_data[1].Value = "一水厂二泵房";
                                        else if (entity.TerId == "3")
                                            parms_data[1].Value = "二水厂一泵房";
                                        else if (entity.TerId == "4")
                                            parms_data[1].Value = "二水厂二泵房";
                                        parms_data[2].Value = entity.Activenerge1;
                                        parms_data[3].Value = entity.Rectivenerge1;
                                        parms_data[4].Value = entity.Activenerge2;
                                        parms_data[5].Value = entity.Rectivenerge2;
                                        parms_data[6].Value = entity.Activenerge3;
                                        parms_data[7].Value = entity.Rectivenerge3;
                                        parms_data[8].Value = entity.Activenerge4;
                                        parms_data[9].Value = entity.Rectivenerge4;
                                        parms_data[10].Value = entity.Pressure;
                                        parms_data[11].Value = entity.LiquidLevel;
                                        parms_data[12].Value = entity.Flow1;
                                        parms_data[13].Value = entity.Flow2;
                                        parms_data[14].Value = entity.Switch1 ? 1 : 0;
                                        parms_data[15].Value = entity.Switch2 ? 1 : 0;
                                        parms_data[16].Value = entity.Switch3 ? 1 : 0;
                                        parms_data[17].Value = entity.Switch4 ? 1 : 0;

                                        if (entity.Current1 > -1)  //-1是无效值
                                            parms_data[18].Value = entity.Current1;
                                        else
                                            parms_data[18].Value = DBNull.Value;
                                        if (entity.Current2 > -1)
                                            parms_data[19].Value = entity.Current2;
                                        else
                                            parms_data[19].Value = DBNull.Value;
                                        if (entity.Current3 > -1)
                                            parms_data[20].Value = entity.Current3;
                                        else
                                            parms_data[20].Value = DBNull.Value;
                                        if (entity.Current4 > -1)
                                            parms_data[21].Value = entity.Current4;
                                        else
                                            parms_data[21].Value = DBNull.Value;

                                        if (entity.Freq1 > -1)   //-1是无效值
                                            parms_data[22].Value = entity.Freq1;
                                        else
                                            parms_data[22].Value = DBNull.Value;
                                        if (entity.Freq2 > -1)
                                            parms_data[23].Value = entity.Freq2;
                                        else
                                            parms_data[23].Value = DBNull.Value;
                                        if (entity.Freq3 > -1)
                                            parms_data[24].Value = entity.Freq3;
                                        else
                                            parms_data[24].Value = DBNull.Value;
                                        if (entity.Freq4 > -1)
                                            parms_data[25].Value = entity.Freq4;
                                        else
                                            parms_data[25].Value = DBNull.Value;

                                        parms_data[26].Value = entity.ModifyTime;
                                        parms_data[27].Value = entity.ModifyTime;

                                        command_predata.ExecuteNonQuery();
                                        entity = null;
                                    }
                                }
                                catch (Exception iex)
                                {
                                    //if (entity != null)
                                    //    datas.Enqueue(entity);
                                    throw iex;
                                }
                            }
                        }
                        //trans.Commit();
                    }
                    return 1;
                }
                catch (Exception ex)
                {
                    //if (trans != null)
                    //    trans.Rollback();

                    throw ex;
                }
            }
        }

        public int InsertAlarmData(ConcurrentQueue<GPRSAlarmFrameDataEntity> datas)
        {
            lock (ConstValue.obj)
            {
                //SqlTransaction trans = null;
                try
                {
                    //trans = SQLHelper.GetTransaction();
                    using (SqlCommand command_frame = new SqlCommand())
                    {
                        string SQL_Frame = "INSERT INTO Frame(Dir,Frame,LogTime) VALUES(@dir,@frame,@logtime)";
                        SqlParameter[] parms_frame = new SqlParameter[]{
                new SqlParameter("@dir",SqlDbType.Int),
                new SqlParameter("@frame",SqlDbType.VarChar,2000),
                new SqlParameter("@logtime",SqlDbType.DateTime)
            };
                        //SqlCommand command_frame = new SqlCommand();
                        command_frame.CommandText = SQL_Frame;
                        command_frame.Parameters.AddRange(parms_frame);
                        command_frame.CommandType = CommandType.Text;
                        command_frame.Connection = SQLHelper.Conn;
                        //command_frame.Transaction = trans;
                        using (SqlCommand command_predata = new SqlCommand())
                        {
                            string SQL_Data = @"INSERT INTO AlarmTable([TerminalId],[TerminalType],[AlarmId],[ModifyTime],Voltage,FieldStrength) 
                                VALUES(@TerId,@TerType,@AlarmId,@ModifyTime,@vol,@fieldstrength)";
                            SqlParameter[] parms_data = new SqlParameter[]{
                    new SqlParameter("@TerId",SqlDbType.Int),
                    new SqlParameter("@TerType",SqlDbType.Int),
                    new SqlParameter("@AlarmId",SqlDbType.Int),
                    new SqlParameter("@ModifyTime",SqlDbType.DateTime),
                    new SqlParameter("@vol",SqlDbType.Decimal),
                    new SqlParameter("@fieldstrength",SqlDbType.SmallInt)
                            };
                            //SqlCommand command_predata = new SqlCommand();
                            command_predata.CommandText = SQL_Data;
                            command_predata.Parameters.AddRange(parms_data);
                            command_predata.CommandType = CommandType.Text;
                            command_predata.Connection = SQLHelper.Conn;
                            //command_predata.Transaction = trans;

                            while (datas.Count > 0)
                            {
                                GPRSAlarmFrameDataEntity entity = null;
                                try
                                {
                                    if (datas.TryDequeue(out entity))
                                    {
                                        parms_frame[0].Value = 1;
                                        parms_frame[1].Value = entity.Frame;
                                        parms_frame[2].Value = entity.ModifyTime;

                                        command_frame.ExecuteNonQuery();

                                        for (int i = 0; i < entity.AlarmId.Count; i++)
                                        {
                                            parms_data[0].Value = entity.TerId;
                                            parms_data[1].Value = (int)(entity.TerminalType);
                                            parms_data[2].Value = entity.AlarmId[i];
                                            parms_data[3].Value = entity.ModifyTime;
                                            parms_data[4].Value = entity.Voltage;
                                            parms_data[5].Value = entity.FieldStrength;

                                            command_predata.ExecuteNonQuery();
                                        }
                                        entity = null;
                                    }
                                }
                                catch (Exception iex)
                                {
                                    //if (entity != null)
                                    //    datas.Enqueue(entity);
                                    throw iex;
                                }
                            }
                        }
                        //trans.Commit();
                    }
                    return 1;
                }
                catch (Exception ex)
                {
                    //if (trans != null)
                    //    trans.Rollback();

                    throw ex;
                }
            }
        }

        /// <summary>
        /// 获取需要下发的参数
        /// </summary>
        /// <returns></returns>
        public List<SendPackageEntity> GetGPRSParm()
        {
            lock (ConstValue.obj)
            {
                string SQL = "SELECT ID,DeviceId,DevTypeId,CtrlCode,FunCode,DataValue,DataLenth,SetDate FROM ParamToDev WHERE SendedFlag = 0 AND SetDate> DATEADD(hour,-48,getdate()) ORDER BY SetDate";  //48小时前的数据不再发送
                List<SendPackageEntity> lstCmd = null;
                using (SqlDataReader reader = SQLHelper.ExecuteReader(SQL, null))
                {
                    lstCmd = new List<SendPackageEntity>();
                    while (reader.Read())
                    {
                        SendPackageEntity cmd = new SendPackageEntity();
                        cmd.SendPackage = new Package();
                        
                        cmd.TableId = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : -1;
                        cmd.SendPackage.DevID = reader["DeviceId"] != DBNull.Value ? Convert.ToInt16(reader["DeviceId"]) : (short)-1;
                        cmd.SendPackage.DevType = (ConstValue.DEV_TYPE)(reader["DevTypeId"] != DBNull.Value ? Convert.ToInt32(reader["DevTypeId"]) : 0);
                        cmd.SendPackage.C0 = reader["CtrlCode"] != DBNull.Value ? Convert.ToByte(reader["CtrlCode"]) : (byte)0x00;
                        cmd.SendPackage.C1 = reader["FunCode"] != DBNull.Value ? Convert.ToByte(reader["FunCode"]) : (byte)0x00;
                        cmd.SendPackage.Data = reader["DataValue"] != DBNull.Value ? ConvertHelper.StringToByte(reader["DataValue"].ToString()) : null;
                        
                        cmd.SendPackage.DataLength = cmd.SendPackage.Data == null ? 0 : cmd.SendPackage.Data.Length;
                        //cmd.ModifyTime = reader["SetDate"] != DBNull.Value ? Convert.ToDateTime(reader["SetDate"]) : DateTime.Now;
                        //cmd.SendedFlag = 0;
                        
                        lstCmd.Add(cmd);
                    }
                }

                return lstCmd;
            }
        }

        /// <summary>
        /// 获取所有的报警类型
        /// </summary>
        public Dictionary<int, string> GetAlarmType()
        {
            lock(ConstValue.obj)
            {
                string SQL = "SELECT * FROM AlarmType";
                Dictionary<int, string> lstAlarm = null;
                using (SqlDataReader reader = SQLHelper.ExecuteReader(SQL, null))
                {
                    lstAlarm = new Dictionary<int, string>();
                    while(reader.Read())
                    {
                        lstAlarm.Add(Convert.ToInt32(reader["AlarmId"]), reader["AlarmName"].ToString());
                        
                        //AlarmTypeEntity alarm = new AlarmTypeEntity();
                        //alarm.TerminalType = Convert.ToByte(reader["TerminalType"]);
                        //alarm.FunCode = Convert.ToByte(reader["FunCode"]);
                        //alarm.AlarmFlag = BitConverter.GetBytes((Int16)(reader["AlarmFlag"]));
                        //alarm.AlarmId = Convert.ToInt32(reader["AlarmId"]);
                        //alarm.AlarmName = reader["AlarmName"].ToString();

                        //lstAlarm.Add(alarm);
                    }
                }
                return lstAlarm;
            }
        }

        public Dictionary<string, float> GetRectifyValue()
        {
            string SQL = "SELECT * FROM RectifyValue";
            Dictionary<string, float> lstOffset = new Dictionary<string, float>();
            using (SqlDataReader reader = SQLHelper.ExecuteReader(SQL, null))
            {
                while (reader.Read())
                {
                    lstOffset.Add(reader["TerminalID"].ToString().Trim() + reader["TerminalType"].ToString().PadLeft(2, '0').Trim() + reader["Funcode"].ToString().PadLeft(2, '0').Trim(), Convert.ToSingle(reader["OffsetValue"]));
                }
            }
            return lstOffset;
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

                    string SQL = "UPDATE ParamToDev SET SendedFlag=@flag, SendedCount=@sendcount WHERE ID=@id";
                    SqlParameter[] parms = new SqlParameter[]
                        {new SqlParameter("@flag",SqlDbType.Int),
                        new SqlParameter("@sendcount",SqlDbType.Int),
                        new SqlParameter("@id",SqlDbType.Int)
                        };
                        

                    SqlCommand command = new SqlCommand();
                    command.CommandText = SQL;
                    command.Parameters.AddRange(parms);
                    command.CommandType = CommandType.Text;
                    command.Connection = SQLHelper.Conn;
                    command.Transaction = trans;

                    for (int i = 0; i < ids.Count; i++)
                    {
                        parms[0].Value = ids[i].SendCount > 0 ? -1 : 1;  //发送次数大于0认为是发送超过限定次数失败
                        parms[1].Value = ids[i].SendCount;
                        parms[2].Value = ids[i].TableId;
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
            string SQL = "SELECT ID,TerminalID,TerminalName,Address,Remark,ModifyTime FROM Terminal WHERE TerminalType='" + (int)type + "' ORDER BY TerminalID";
            return SQLHelper.ExecuteDataTable(SQL, null);
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
            object obj = SQLHelper.ExecuteScalar(SQL, null);
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
            //string SQL_SELECT = "SELECT COUNT(1) FROM Terminal WHERE SyncState=1 AND TerminalType='" + (int)type + "' AND TerminalID='" + TerminalID + "'";
            //object obj_exist = SQLHelper.ExecuteScalar(SQL_SELECT, null);
            //bool exist = false;
            //if (obj_exist != null && obj_exist != DBNull.Value)
            //{
            //    exist = (Convert.ToInt32(obj_exist) > 0 ? true : false);
            //}
            //if (exist)
            SQL = "DELETE FROM Terminal WHERE TerminalType='" + (int)type + "' AND TerminalID='" + TerminalID + "'";
            SQLHelper.ExecuteNonQuery(SQL, null);
        }

        public int GetTerminalTableMaxId()
        {
            string SQL = "SELECT MAX(id) FROM Terminal";
            object obj = SQLHelper.ExecuteScalar(SQL, null);
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
            object obj = SQLHelper.ExecuteScalar(SQL, null);
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
            //SqlTransaction trans = null;
            try
            {
                //trans = SQLHelper.Conn.BeginTransaction();
                SqlCommand command = new SqlCommand();
                command.Connection = SQLHelper.Conn;
                //command.Transaction = trans;

                if (needmodify)
                {
                    command.CommandText = "DELETE FROM Terminal WHERE TerminalType='" + (int)terType + "' AND TerminalID='" + terminalid + "'";
                    command.ExecuteNonQuery();

                    //command.CommandText = "UPDATE Terminal SET SyncState=-1 WHERE TerminalType='" + (int)terType + "' AND TerminalID='" + terminalid + "'";
                    //command.ExecuteNonQuery();
                }

                command.CommandText = string.Format("INSERT INTO Terminal(ID,TerminalID,TerminalName,TerminalType,Address,Remark) VALUES('{0}','{1}','{2}','{3}','{4}','{5}')",
                                                         GetTerminalTableMaxId() + 1, terminalid, name.Trim(), (int)terType, addr.Trim(), remark.Trim());
                command.ExecuteNonQuery();

                //Update UniversalTerConfig Table

                //Update UniversalTerWayConfig Table
                if (lstPointID != null && lstPointID.Count > 0)
                {
                    //UniversalTerWayConfig ID TerminalID PointID
                    command.CommandText = "DELETE FROM UniversalTerWayConfig WHERE TerminalID='" + terminalid + "'";
                    command.ExecuteNonQuery();

                    //command.CommandText = "UPDATE UniversalTerWayConfig SET SyncState=-1 WHERE TerminalID='" + terminalid + "'";
                    //command.ExecuteNonQuery();

                    int configeMaxId = GetUniversalTerWayConfigTableMaxId();
                    foreach (UniversalWayTypeConfigEntity config in lstPointID)
                    {
                        configeMaxId++;
                        command.CommandText = string.Format("INSERT INTO UniversalTerWayConfig(ID,TerminalID,Sequence,PointID,TerminalType) VALUES('{0}','{1}','{2}','{3}','{4}')",
                            configeMaxId, terminalid, config.Sequence, config.PointID, (int)terType);
                        command.ExecuteNonQuery();
                    }
                }

                //trans.Commit();
                return 1;
            }
            catch (Exception ex)
            {
                //if (trans != null)
                //    trans.Rollback();
                return -1;
            }
        }

        public void DeleteUniversalWayTypeConfig(int PointID)
        {
            string SQL = "DELETE FROM UniversalTerWayConfig WHERE PointID='" + PointID + "'";
            SQLHelper.ExecuteNonQuery(SQL, null);
        }

        public void DeleteUniversalWayTypeConfig_TerID(int TerminalID,TerType terType)
        {
            string SQL_Ter = "SELECT Distinct PointID FROM UniversalTerWayConfig WHERE TerminalID='" + TerminalID + "' AND TerminalType='"+((int)terType).ToString()+"'";
            List<string> lstPoint = new List<string>();
            using (SqlDataReader reader = SQLHelper.ExecuteReader(SQL_Ter, null))
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
                    //string SQL_SELECT = "SELECT COUNT(1) FROM UniversalTerWayConfig WHERE SyncState=-1 AND PointID='" + pointid + "' AND TerminalID='" + TerminalID + "' AND TerminalType='" + ((int)terType).ToString() + "'";
                    //object obj_exist = SQLHelper.ExecuteScalar(SQL_SELECT, null);
                    //bool exist = false;
                    //if (obj_exist != null && obj_exist != DBNull.Value)
                    //{
                    //    exist = (Convert.ToInt32(obj_exist) > 0 ? true : false);
                    //}
                    //if (exist)
                        SQL = "DELETE FROM UniversalTerWayConfig WHERE PointID='" + pointid + "' AND TerminalID='" + TerminalID + "' AND TerminalType='" + ((int)terType).ToString() + "'";
                    //else
                    //    SQL = "UPDATE UniversalTerWayConfig SET SyncState=-1 WHERE PointID='" + pointid + "' AND TerminalID='" + TerminalID + "' AND TerminalType='" + ((int)terType).ToString() + "'";
                    SQLHelper.ExecuteNonQuery(SQL, null);
                }
            }
        }

        public List<UniversalWayTypeConfigEntity> GetUniversalWayTypeConfig(int TerminalID,TerType terType)
        {
            string SQL = "SELECT id,Sequence,PointID,ModifyTime FROM UniversalTerWayConfig WHERE TerminalID='" + TerminalID + "' AND TerminalType='" + ((int)terType).ToString() + "'";
            
            using (SqlDataReader reader = SQLHelper.ExecuteReader(SQL, null))
            {
                List<UniversalWayTypeConfigEntity> lstWayTypeConfig = new List<UniversalWayTypeConfigEntity>();
                while (reader.Read())
                {
                    UniversalWayTypeConfigEntity entity = new UniversalWayTypeConfigEntity();
                    entity.ID = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : -1;
                    entity.PointID = reader["PointID"] != DBNull.Value ? Convert.ToInt32(reader["PointID"]) : -1;
                    entity.Sequence = reader["Sequence"] != DBNull.Value ? Convert.ToInt32(reader["Sequence"]) : -1;
                    entity.TerminalID = TerminalID;
                    //entity.SyncState = reader["SyncState"] != DBNull.Value ? Convert.ToInt32(reader["SyncState"]) : -1;
                    entity.ModifyTime = reader["ModifyTime"] != DBNull.Value ? Convert.ToDateTime(reader["ModifyTime"]) : ConstValue.MinDateTime;

                    lstWayTypeConfig.Add(entity);
                }
                return lstWayTypeConfig;
            }
        }

        public DataTable GetUniversalDataConfig(TerType terType)
        {
            string SQL = @"SELECT DISTINCT Type.ID,Config.TerminalID,Config.Sequence,Type.[Level],Type.[ParentID],Type.[WayType],Type.[Name],Type.[Unit]
                        FROM [UniversalTerWayConfig] Config,[UniversalTerWayType] Type WHERE (Config.PointID=Type.ID OR Config.PointID=Type.ParentID) AND Config.TerminalType=Type.TerminalType AND Config.TerminalType='"+((int)terType).ToString()+"'";

            DataTable dt = SQLHelper.ExecuteDataTable(SQL, null);
            return dt;
        }

        public List<PreDetailDataEntity> GetPreDetail(string TerminalID, DateTime minTime, DateTime maxTime, int interval)
        {
            string SQL = @"SELECT PressValue,CollTime FROM Pressure_Real 
            WHERE (CollTime BETWEEN @mintime AND @maxtime) AND (DATEDIFF(minute,@mintime,CollTime) %@interval = 0) AND TerminalID=@TerId  ORDER BY CollTime";

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
        }

        public List<PreDetailDataEntity> GetFlowDetail(string TerminalID, DateTime minTime, DateTime maxTime, int interval, int datatype)
        {
            string SQL = @"SELECT FlowValue,FlowInverted,FlowInstant,CollTime FROM Flow_Real 
            WHERE (CollTime BETWEEN @mintime AND @maxtime) AND (DATEDIFF(minute,@mintime,CollTime) %@interval = 0) AND TerminalIDID=@TerId  ORDER BY CollTime";

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
        }

        public List<PrectrlDetailDataEntity> GetPrectrlDetail(string TerminalID, DateTime minTime, DateTime maxTime, int interval)
        {
            string SQL = @"SELECT entrance_pre,outlet_pre, FlowValue,FlowInverted,FlowInstant,CollTime FROM [PreCtrl_Real] 
            WHERE (CollTime BETWEEN @mintime AND @maxtime) AND (DATEDIFF(minute,@mintime,CollTime) %@interval = 0) AND TerminalID=@TerId  ORDER BY CollTime";

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
                List<PrectrlDetailDataEntity> lstData = new List<PrectrlDetailDataEntity>();
                while (reader.Read())
                {
                    PrectrlDetailDataEntity entity = new PrectrlDetailDataEntity();
                    //进口压力
                    entity.EntrancePreData = reader["entrance_pre"] != DBNull.Value ? Convert.ToDecimal(reader["entrance_pre"]) : 0;
                    //出口压力
                    entity.OutletPreData = reader["outlet_pre"] != DBNull.Value ? Convert.ToDecimal(reader["outlet_pre"]) : 0;
                    //正向流量
                    entity.ForwardFlow = reader["FlowValue"] != DBNull.Value ? Convert.ToDecimal(reader["FlowValue"]) : 0;
                    //反向流量
                    entity.ReverseFlow = reader["FlowInverted"] != DBNull.Value ? Convert.ToDecimal(reader["FlowInverted"]) : 0;
                    //瞬时流量
                    entity.InstantFlow = reader["FlowInstant"] != DBNull.Value ? Convert.ToDecimal(reader["FlowInstant"]) : 0;
                    entity.CollTime = reader["CollTime"] != DBNull.Value ? Convert.ToDateTime(reader["CollTime"]) : ConstValue.MinDateTime;

                    lstData.Add(entity);
                }
                return lstData;
            }
        }

        public List<UniversalDetailDataEntity> GetUniversalDetail(string TerminalID, int typeId, DateTime minTime, DateTime maxTime, int interval)
        {
            string SQL = @"SELECT [DataValue],CollTime FROM UniversalTerData 
    WHERE (CollTime BETWEEN @mintime AND @maxtime) AND (DATEDIFF(minute,@mintime,CollTime) %@interval = 0) AND TerminalID=@TerId AND TypeTableID=@typeId  ORDER BY CollTime";

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
        }

        public List<OLWQDetailDataEntity> GetOLWQDetail(string TerminalID, DateTime minTime, DateTime maxTime, int interval, int datatype)
        {
            string valuecolumnname = "";
            if (datatype == 0)  //浊度
                valuecolumnname = "Turbidity";
            else if (datatype == 1)  //余氯
                valuecolumnname = "ResidualCl";
            else if (datatype == 2)  //PH
                valuecolumnname = "PH";
            else if (datatype == 3)  //电导率
                valuecolumnname = "Conductivity";
            else if (datatype == 4)  //温度
                valuecolumnname = "Conductivity";
            string SQL = @"SELECT Turbidity,ResidualCl,PH,Conductivity,Temperature,CollTime FROM OLWQ_Real 
            WHERE (CollTime BETWEEN @mintime AND @maxtime) AND (DATEDIFF(minute,@mintime,CollTime) %@interval = 0) AND TerminalID=@TerId AND ValueColumnName=@valuename  ORDER BY CollTime";

            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter("@TerId",SqlDbType.Int),
                new SqlParameter("@mintime",SqlDbType.DateTime),
                new SqlParameter("@maxtime",SqlDbType.DateTime),
                new SqlParameter("@interval",SqlDbType.Int),
                new SqlParameter("@valuename",SqlDbType.NVarChar)
            };
            parms[0].Value = TerminalID;
            parms[1].Value = minTime;
            parms[2].Value = maxTime;
            parms[3].Value = interval;
            parms[4].Value = valuecolumnname;

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
        }

        public string GetTerminalName(string TerminalID, TerType tertype)
        {
            string SQL = "SELECT TerminalName FROM Terminal WHERE TerminalType= '" + (int)tertype + "' AND TerminalID='" + TerminalID.Trim() + "'";
            object obj_name = SQLHelper.ExecuteScalar(SQL, null);
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
        }

        public List<PrectrlDataEntity> GetPrectrlData(List<int> terminalids)
        {
            if (terminalids == null || terminalids.Count == 0)
                return null;
            string str_ids = "";
            foreach (int id in terminalids)
            {
                str_ids += "'" + id + "',";
            }
            if (str_ids.EndsWith(","))
                str_ids = str_ids.Substring(0, str_ids.Length - 1);

            //string SQL = string.Format(@"SELECT t.TerminalID,t.TerminalName,t.Address,f.entrance_pre,f.outlet_pre,f.FlowValue,f.FlowInverted,f.FlowInstant,f.CollTime,f.alarmcode,f.alarmdesc 
            //                            FROM [PreCtrl_Real] f,Terminal t WHERE 
            //                            f.TerminalID=t.TerminalID AND t.TerminalType = {0} AND t.TerminalID IN({1}) AND 
            //                            f.id = (SELECT TOP 1 id FROM PreCtrl_Real  
            //                            WHERE f.TerminalID = TerminalID order by UnloadTime DESC) order by TerminalID ASC, CollTime DESC", ((int)TerType.PressureCtrl).ToString(), str_ids);
            string SQL = string.Format(@"SELECT t.TerminalID,t.TerminalName,t.Address,f.entrance_pre,f.outlet_pre,f.FlowValue,f.FlowInverted,f.FlowInstant,f.CollTime,f.alarmcode,f.alarmdesc 
                                            FROM [PreCtrl_Real] f LEFT JOIN Terminal t ON f.TerminalID=t.TerminalID WHERE t.TerminalType = {0} AND f.ID IN
                                            (SELECT MAX(ID) FROM PreCtrl_Real WHERE TerminalID IN({1}) GROUP BY TerminalID) ORDER BY f.TerminalID", ((int)TerType.PressureCtrl).ToString(), str_ids);

            using (SqlDataReader reader = SQLHelper.ExecuteReader(SQL, null))
            {
                List<PrectrlDataEntity> lstData = new List<PrectrlDataEntity>();
                int terminalid;
                DateTime coltime;
                decimal entranceprevalue;
                decimal outletprevalue;
                decimal forwardflowvalue;
                decimal reverseflowvalue;
                decimal instantflowvalue;

                while (reader.Read())
                {
                    entranceprevalue = reader["entrance_pre"] != DBNull.Value ? Convert.ToDecimal(reader["entrance_pre"]) : 0;
                    outletprevalue = reader["outlet_pre"] != DBNull.Value ? Convert.ToDecimal(reader["outlet_pre"]) : 0;
                    forwardflowvalue = reader["FlowValue"] != DBNull.Value ? Convert.ToDecimal(reader["FlowValue"]) : 0;
                    reverseflowvalue = reader["FlowInverted"] != DBNull.Value ? Convert.ToDecimal(reader["FlowInverted"]) : 0;
                    instantflowvalue = reader["FlowInstant"] != DBNull.Value ? Convert.ToDecimal(reader["FlowInstant"]) : 0;
                    terminalid = reader["TerminalID"] != DBNull.Value ? Convert.ToInt32(reader["TerminalID"]) : 0;
                    coltime = reader["CollTime"] != DBNull.Value ? Convert.ToDateTime(reader["CollTime"]) : DateTime.Now;

                    PrectrlDataEntity entity = new PrectrlDataEntity();
                    entity.TerminalID = reader["TerminalID"] != DBNull.Value ? Convert.ToInt32(reader["TerminalID"]) : 0;
                    entity.TerminalName = reader["TerminalName"] != DBNull.Value ? reader["TerminalName"].ToString() : "";

                    entity.EntrancePreValue = entranceprevalue;
                    entity.OutletPreValue = outletprevalue;
                    entity.ForwardFlowValue = forwardflowvalue;
                    entity.ReverseFlowValue = reverseflowvalue;
                    entity.InstantFlowValue = instantflowvalue;
                    entity.AlarmCode = (byte)(reader["alarmcode"] != DBNull.Value ? (reader["alarmcode"]) : 0x00);
                    entity.AlarmDesc= reader["alarmdesc"] != DBNull.Value ? reader["alarmdesc"].ToString() : "";

                    entity.Addr = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : "";

                    lstData.Add(entity);

                }
                return lstData;
            }
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
                        if (valuetype.ToLower() == "conductivity")
                        {
                            dr_res[0]["Temperature"] = dr["Temperature"].ToString();
                        }
                        dr_res[0][valuetype] = dr[valuetype].ToString();
                    }
                }
            }

            return dt_res;
        }

        #region 清除历史数据
        //清除数据库中历史数据       
        public int ClearHistoryData(DateTime dt)
        {
            int clearcount = 0;  //清除数量
            clearcount += ClearHistoryData("AlarmTable", "ModifyTime", dt);           //报警数据
            clearcount += ClearHistoryData("DL_Noise_Real", "UnloadTime", dt);        //噪声数据
            clearcount += ClearHistoryData("DL_NoiseAnalyse", "UnloadTime", dt);      //噪声分析数据
            clearcount += ClearHistoryData("Flow_Real", "UnloadTime", dt);            //流量数据
            clearcount += ClearHistoryData("Frame", "LogTime", dt);                   //帧数据
            clearcount += ClearHistoryData("Hydrant_Real", "UnloadTime", dt);         //消防栓数据
            clearcount += ClearHistoryData("OLWQ_Real", "UnloadTime", dt);            //水质数据
            clearcount += ClearHistoryData("ParamToDev", "SetDate", dt);              //下送参数数据
            clearcount += ClearHistoryData("PreCtrl_Real", "UnloadTime", dt);         //压力控制器数据
            clearcount += ClearHistoryData("Pressure_Real", "UnloadTime", dt);        //压力数据
            clearcount += ClearHistoryData("UniversalTerData", "UnloadTime", dt);     //通用终端数据
            clearcount += ClearHistoryData("Waterworker_Real", "UnloadTime", dt);     //水厂数据

            return clearcount;
        }
        
        /// <summary>
        /// tablename
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <param name="dtcolname">时间字段名</param>
        /// <param name="dt">时间点</param>
        int ClearHistoryData(string tablename,string dtcolname,DateTime dt)  //DL_Noise_Real  DL_NoiseAnalyse
        {
            string SQL = "DELETE FROM " + tablename + " WHERE " + dtcolname + "<@mtime";
            SqlParameter[] parms = new SqlParameter[]
                {
                new SqlParameter("@mtime", SqlDbType.DateTime)
                };
            parms[0].Value = dt;

            return SQLHelper.ExecuteNonQuery(SQL, parms);
        }
        #endregion

    }
}
