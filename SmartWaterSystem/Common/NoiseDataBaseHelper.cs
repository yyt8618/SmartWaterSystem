using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using Entity;
using Common;
using System.Data.SqlClient;

namespace SmartWaterSystem
{
    /// <summary>
    /// 提供对噪声系统的数据访问
    /// </summary>
    internal class NoiseDataBaseHelper
    {
        /// <summary>
        /// 从数据库中获取记录仪列表
        /// </summary>
        public static List<NoiseRecorder> GetRecorders()
        {
            try
            {
                List<NoiseRecorder> recList = new List<NoiseRecorder>();
                string sql = string.Empty;

                sql = "SELECT * FROM EN_NoiseRecorder";
                DataTable dtRecorder = SQLHelper.ExecuteDataTable(sql);

                if (dtRecorder.Rows.Count == 0)
                {
					return recList;//throw new Exception("记录仪数据为空。");
                }

                for (int i = 0; i < dtRecorder.Rows.Count; i++)
                {
                    NoiseRecorder rec = new NoiseRecorder();
                    rec.ID = Convert.ToInt32(dtRecorder.Rows[i]["RecorderId"]);
                    rec.Remark = dtRecorder.Rows[i]["Remark"].ToString();
                    rec.AddDate = Convert.ToDateTime(dtRecorder.Rows[i]["AddDate"]);
                    rec.GroupState = Convert.ToInt32(dtRecorder.Rows[i]["GroupState"]);
                    rec.Longtitude = dtRecorder.Rows[i]["longitude"] != DBNull.Value ? dtRecorder.Rows[i]["longitude"].ToString() : "";
                    rec.Latitude = dtRecorder.Rows[i]["latitude"] != DBNull.Value ? dtRecorder.Rows[i]["latitude"].ToString() : "";

                    sql = "SELECT * FROM MT_RecorderSet WHERE RecorderId = " + rec.ID.ToString();
                    DataTable recSet = SQLHelper.ExecuteDataTable(sql);
                    rec.RecordTime = Convert.ToInt32(recSet.Rows[0]["RecordTime"]);
                    //rec.CommunicationTime = Convert.ToInt32(recSet.Rows[0]["CommunicationTime"]);
                    rec.PickSpan = Convert.ToInt32(recSet.Rows[0]["PickSpan"]);
                    rec.Power = Convert.ToInt32(recSet.Rows[0]["StartEnd_Power"]);
                    rec.ControlerPower = Convert.ToInt32(recSet.Rows[0]["Controler_Power"]);
					rec.Power = Convert.ToInt32(recSet.Rows[0]["StartEnd_Power"]);
                    rec.LeakValue = Convert.ToInt32(recSet.Rows[0]["LeakValue"]);

                    sql = "SELECT GroupId,RecorderId,leakValue,FrequencyValue,OriginalData,CollTime,UnloadTime,HistoryFlag FROM DL_Noise_Real WHERE RecorderId = " + rec.ID + " ORDER BY CollTime DESC";
                    //DataTable dt_test = SQLiteHelper.ExecuteDataTable(sql, null);
                    using (SqlDataReader reader = SQLHelper.ExecuteReader(sql, null))
                    {
                        if (reader.Read())
                        {
                            rec.Data = new NoiseData();
                            rec.Data.GroupID = Convert.ToInt32(reader["GroupId"]);
                            rec.Data.ReadTime = Convert.ToDateTime(reader["CollTime"]);
                            rec.Data.UploadTime = Convert.ToDateTime(reader["UnloadTime"]);
                            rec.Data.UploadFlag = Convert.ToInt32(reader["HistoryFlag"]); 

                            string[] strAmp = reader["LeakValue"].ToString().Split(',');
                            double[] amp = new double[strAmp.Length];
                            for (int j = 0; j < strAmp.Length ; j++)  //&& strAmp.Length > 1
                            {
                                if (!string.IsNullOrEmpty(strAmp[j]))
                                    amp[j] = Convert.ToDouble(strAmp[j]);
                            }
                            rec.Data.Amplitude = amp;

                            string[] strFrq = reader["FrequencyValue"].ToString().Split(',');
                            double[] frq = new double[strFrq.Length];
                            for (int j = 0; j < strFrq.Length && strFrq.Length > 1; j++)
                            {
                                if (!string.IsNullOrEmpty(strFrq[j]))
                                    frq[j] = Convert.ToDouble(strFrq[j]);
                            }
                            rec.Data.Frequency = frq;

                            string[] strDa = reader["OriginalData"].ToString().Split(',');
                            short[] da = new short[strDa.Length];
                            for (int j = 0; j < strDa.Length; j++)
                            {
                                if (strDa[j] != "")
                                    da[j] = Convert.ToInt16(strDa[j]);
                            }
                            rec.Data.OriginalData = da;
                        }
                    }

                    sql = "SELECT GroupId,RecorderId,MinLeakValue,MinFrequencyValue,IsLeak,ESA,CollTime,UnloadTime,HistoryFlag,EnergyValue,LeakProbability FROM DL_NoiseAnalyse WHERE RecorderId = " + rec.ID + " ORDER BY CollTime DESC";
                    using (SqlDataReader reader = SQLHelper.ExecuteReader(sql, null))
                    {
                        if (reader.Read())
                        {
                            rec.Result = new NoiseResult();
                            rec.Result.GroupID = Convert.ToInt32(reader["GroupId"]);
                            rec.Result.RecorderID = rec.ID;
                            rec.Result.IsLeak = Convert.ToInt32(reader["IsLeak"]);
                            rec.Result.ReadTime = Convert.ToDateTime(reader["CollTime"]);
                            rec.Result.UploadTime = Convert.ToDateTime(reader["UnloadTime"]);
                            rec.Result.LeakAmplitude = Convert.ToDouble(reader["MinLeakValue"]);
                            rec.Result.LeakFrequency = Convert.ToDouble(reader["MinFrequencyValue"]);
                            rec.Result.EnergyValue = Convert.ToDouble(reader["EnergyValue"]);
                            rec.Result.LeakProbability = Convert.ToDouble(reader["LeakProbability"]);
                            //rec.Result.UploadFlag = (int)reSet.Rows[0]["HistoryFlag"];
                        }
                    }
                    
                    sql = @"SELECT GroupId FROM MP_GroupRecorder WHERE RecorderId = " + rec.ID.ToString();
                    object gID = SQLHelper.ExecuteScalar(sql);
                    if (gID == null)
                        rec.GroupID = 0;
                    else
                        rec.GroupID = Convert.ToInt32(gID);

                    recList.Add(rec);
                }

                return recList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 从数据库中获取记录仪分组列表
        /// </summary>
        public static List<NoiseRecorderGroup> GetGroups()
        {
            try
            {
                List<NoiseRecorder> recList = GetRecorders();
                List<NoiseRecorderGroup> recGroupList = new List<NoiseRecorderGroup>();
                string sql = string.Empty;

                sql = "SELECT * FROM EN_Group";
                DataTable dtGroup = SQLHelper.ExecuteDataTable(sql);

                if (dtGroup.Rows.Count == 0)
                {
					return recGroupList;//throw new Exception("分组数据为空。");
                }

                for (int i = 0; i < dtGroup.Rows.Count; i++)
                {
                    NoiseRecorderGroup gp = new NoiseRecorderGroup();
                    //gp.RecorderList = new List<NoiseRecorder>();
                    gp.ID = Convert.ToInt32(dtGroup.Rows[i]["GroupId"]);
                    gp.Name = dtGroup.Rows[i]["Name"].ToString();
                    gp.Remark = dtGroup.Rows[i]["Remark"].ToString();

					gp.RecorderList = (from item in recList.AsEnumerable()
							   where item.GroupID == gp.ID
							   select item).ToList();

					//sql = "SELECT RecorderId FROM MP_GroupRecorder WHERE GroupId = " + group.ID.ToString();
					//DataTable dtGroupRecorder = DbForAccess.GetDataTable(sql);
					//for (int j = 0; j < recList.Count; j++)
					//{
					//    for (int k = 0; k < dtGroupRecorder.Rows.Count; k++)
					//    {
					//        int recID = (int)dtGroupRecorder.Rows[k]["RecorderId"];
					//        if (recID == recList[j].ID)
					//            gp.RecorderList.Add(recList[j]);
					//    }
					//}
                    recGroupList.Add(gp);
                }

                return recGroupList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取指定分组下的所有记录仪
        /// </summary>
        /// <param name="groupID">分组编号</param>
        public static List<NoiseRecorder> GetRecordersByGroupId(int groupID)
        {
            try
            {
                List<NoiseRecorder> recGroupList = new List<NoiseRecorder>();
                List<NoiseRecorder> recAllList = GetRecorders();
                string sql = string.Empty;

				recGroupList = (from item in recAllList.AsEnumerable()
								where item.GroupID == groupID
								   select item).ToList();
                
                return recGroupList;
            }
			catch (Exception ex)
			{
				throw ex;
				//return -1;
			}
        }

        /// <summary>
        /// 从数据库中获取远传控制器列表
        /// </summary>
        /// <returns></returns>
        public static List<DistanceController> GetController()
        {
            List<DistanceController> controler = new List<DistanceController>();
            string sql = string.Empty;

            sql = "SELECT * FROM EN_DistanceControl";
            DataTable dtCon = SQLHelper.ExecuteDataTable(sql);
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                DistanceController con = new DistanceController();
                con.ID = (int)dtCon.Rows[i]["ControlId"];
                con.RecordID = (int)dtCon.Rows[i]["RecorderId"];
                con.Port = (int)dtCon.Rows[i]["Port"];
                con.SendTime = (int)dtCon.Rows[i]["SendTime"];
                con.IPAdress = dtCon.Rows[i]["IPAdress"].ToString();
                controler.Add(con);
            }

            return controler;
        }

        /// <summary>
        /// 更新记录仪
        /// </summary>
        /// <param name="rec">记录仪对象</param>
        public static int UpdateRecorder(NoiseRecorder rec)
        {
            try
            {
                string sql = string.Empty;
                int query = 0;
                sql = string.Format(@"UPDATE EN_NoiseRecorder SET Remark = '{0}',GroupState = {1},Longitude='{2}',Latitude='{3}' WHERE RecorderId = {4}", rec.Remark, rec.GroupState, rec.Longtitude, rec.Latitude, rec.ID);
                query = SQLHelper.ExecuteNonQuery(sql);

                sql = string.Format(
                @"UPDATE MT_RecorderSet 
                SET RecordTime = {0},PickSpan = {1},Controler_Power = {2},StartEnd_Power = {3},LeakValue = {4} WHERE RecorderId = {5}",
                rec.RecordTime, rec.PickSpan, rec.ControlerPower, rec.Power, rec.LeakValue, rec.ID);
                query = SQLHelper.ExecuteNonQuery(sql);

                return query;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public static int DeleteStandData(int GroupID, int RecorderID)
        {
            try
            {
                string SQL = string.Empty;
                SQL = string.Format(@"DELETE FROM ST_Noise_StandData WHERE GroupID='{0}' AND RecorderID='{1}'", GroupID, RecorderID);
                SQLHelper.ExecuteNonQuery(SQL);
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public static int DeleteStandData(int RecorderID)
        {
            try
            {
                string SQL = string.Empty;
                SQL = string.Format(@"DELETE FROM ST_Noise_StandData WHERE RecorderID='{0}'", RecorderID);
                SQLHelper.ExecuteNonQuery(SQL);
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        /// <summary>
        /// 保存启动时获取的32个数据(用于漏点确定)
        /// </summary>
        /// <param name="rec"></param>
        /// <returns></returns>
        public static int SaveStandData(int GroupID, int RecorderID, int StandValue)
        {
            try
            {
                if (DeleteStandData(GroupID, RecorderID) == -1)
                {
                    return -1;
                }
                string SQL = string.Format(@"INSERT INTO ST_Noise_StandData(GroupID,RecorderID,Data) VALUES('{0}','{1}','{2}')",
                      GroupID, RecorderID, StandValue);
                SQLHelper.ExecuteNonQuery(SQL);

                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        /// <summary>
        /// 保存启动时获取的32个数据(用于漏点确定)
        /// </summary>
        /// <param name="rec"></param>
        /// <returns></returns>
        public static int SaveStandData(int RecorderID, int StandValue)
        {
            try
            {
                if (DeleteStandData(RecorderID) == -1)
                {
                    return -1;
                }
                string SQL = string.Format(@"INSERT INTO ST_Noise_StandData(GroupID,RecorderID,Data) SELECT GroupId,RecorderId,{0} FROM MP_GroupRecorder WHERE RecorderId={1}",
                      StandValue, RecorderID);
                SQLHelper.ExecuteNonQuery(SQL);

                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        /// <summary>
        ///  获取记录仪的标准数据
        /// </summary>
        /// <param name="GroupID"></param>
        /// <param name="RecorderID"></param>
        /// <returns></returns>
        public static short[] GetStandData(int GroupID, int RecorderID)
        {
            try
            {
                string SQL = string.Format("SELECT Data FROM ST_Noise_StandData WHERE GroupID='{0}' AND RecorderID='{1}'", GroupID, RecorderID);

                string str_data = string.Empty;
                DataTable dt = SQLHelper.ExecuteDataTable(SQL);
                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                {
                    str_data = dt.Rows[0]["Data"] != DBNull.Value ? dt.Rows[0]["Data"].ToString() : "";
                }
                if (!string.IsNullOrEmpty(str_data))
                {
                    List<short> lstData = new List<short>();
                    string[] str_datas = str_data.Split(',');
                    if (str_datas != null && str_datas.Length == 32)
                    {
                        foreach (string tmp in str_datas)
                        {
                            if (Regex.IsMatch(tmp, @"^\d+$"))
                                lstData.Add(Convert.ToInt16(tmp));
                        }
                    }
                    return lstData.ToArray();
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 更新记录仪分组
        /// </summary>
        /// <param name="group">分组对象</param>
        public static int UpdateGroup(NoiseRecorderGroup group)
        {
            try
            {
                string sql = string.Empty;
                int query = 0;
                sql = string.Format(@"UPDATE EN_Group SET Name = '{0}', Remark = '{1}' WHERE GroupId = {2}", group.Name, group.Remark, group.ID);
                query = SQLHelper.ExecuteNonQuery(sql);

                return query;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// 更新远传控制器
        /// </summary>
        /// <returns></returns>
        public static int UpdateControler(DistanceController ctrl)
        {
            try
            {
                string sql = string.Empty;
                int query = 0;

                sql = "SELECT * FROM EN_DistanceControl WHERE ControlId = " + ctrl.ID.ToString();
                DataTable dt = SQLHelper.ExecuteDataTable(sql);
                if (dt.Rows.Count != 0) // 不为0 表示存在该控制器的记录，更新即可
                {
                    sql = string.Format(@"UPDATE EN_DistanceControl SET RecorderId = {0}, IPAdress = '{1}', Port = {2}, SendTime = {3} WHERE ControlId = {4}",
                        ctrl.RecordID, ctrl.IPAdress, ctrl.Port, ctrl.SendTime, ctrl.ID);
                    query = SQLHelper.ExecuteNonQuery(sql);
                }
                else if (dt.Rows.Count == 0)
                {
                    query = AddControler(ctrl);
                }

                return query;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// 添加记录仪到数据库
        /// </summary>
        /// <param name="rec">记录仪对象</param>
        public static int AddRecorder(NoiseRecorder rec)
        {
            try
            {
                string sql = string.Empty;
                int query = 0;
                sql = string.Format(@"INSERT INTO EN_NoiseRecorder(RecorderId,AddDate,Remark,GroupState,longitude,latitude) VALUES(@RecorderId,@AddDate,@Remark,@GroupState,@lng,@lat)",
                    rec.ID, rec.AddDate, rec.Remark, rec.GroupState);
                SqlParameter[] parms = new SqlParameter[]{
                    new SqlParameter("@RecorderId",DbType.String),
                    new SqlParameter("@AddDate",DbType.DateTime),
                    new SqlParameter("@Remark",DbType.String),
                    new SqlParameter("@GroupState",DbType.Int32),
                    new SqlParameter("@lng",SqlDbType.Float),
                    new SqlParameter("@lat",SqlDbType.Float)
                };
                parms[0].Value = rec.ID;
                parms[1].Value = rec.AddDate;
                parms[2].Value = rec.Remark;
                parms[3].Value = rec.GroupState;
                if (rec.Longtitude == null)
                    parms[4].Value = DBNull.Value;
                else
                    parms[4].Value = rec.Longtitude;
                if (rec.Latitude == null)
                    parms[5].Value = DBNull.Value;
                else
                    parms[5].Value = rec.Latitude;

                query = SQLHelper.ExecuteNonQuery(sql, parms);

                sql = string.Format(@"INSERT INTO MT_RecorderSet(RecorderId, RecordTime, PickSpan, Controler_Power, StartEnd_Power, LeakValue) 
                VALUES({0},{1},{2},{3},{4},{5})",
                    rec.ID, rec.RecordTime, rec.PickSpan, rec.ControlerPower, rec.Power, rec.LeakValue);
                query = SQLHelper.ExecuteNonQuery(sql);
                return query;
            }
            catch (Exception ex)
            {
				throw ex;
                //return -1;
            }
        }

        /// <summary>
        /// 添加分组到数据库
        /// </summary>
        /// <param name="rec">分组对象</param>
        public static int AddGroup(NoiseRecorderGroup group)
        {
            try
            {
                string sql = string.Empty;
                int query = 0;
                sql = string.Format(@"INSERT INTO EN_Group(GroupId,Name, Remark) VALUES('{0}','{1}','{2}')",
                    group.ID,group.Name, group.Remark);
                query = SQLHelper.ExecuteNonQuery(sql);

                return query;
            }
			catch (Exception ex)
			{
				throw ex;
				//return -1;
			}
        }

        /// <summary>
        /// 获得一个组ID，该ID应该足够大，使之不与记录仪ID重复,否则会导致树形列表出现重复ID项
        /// </summary>
        /// <returns></returns>
        public static int GetGroupID()
        {
            Random rd = new Random();
            int rdint=rd.Next(50000, int.MaxValue - 1);
            if (IsExistGroupID(rdint))
            {
                return GetGroupID();
            }
            else 
                return rdint;
        }

        public static bool IsExistGroupID(int groupId)
        {
            string SQL = "SELECT COUNT(1) FROM EN_Group WHERE GroupID='" + groupId + "'";
            object objcount = SQLHelper.ExecuteScalar(SQL, null);
            if (objcount != null)
                return Convert.ToInt32(objcount) == 1 ? true : false;
            return false;
        }

        /// <summary>
        /// 添加远传控制器到数据库
        /// </summary>
        public static int AddControler(DistanceController ctrl)
        {
            try
            {
                string sql = string.Empty;
                int query = 0;
                sql = string.Format(@"INSERT INTO EN_DistanceControl(ControlId, RecorderId, IPAdress, Port, SendTime) VALUES({0},{1},'{2}',{3},{4})",
                    ctrl.ID, ctrl.RecordID, ctrl.IPAdress, ctrl.Port, ctrl.SendTime);
                query = SQLHelper.ExecuteNonQuery(sql);
                return query;
            }
			catch (Exception ex)
			{
				throw ex;
				//return -1;
			}
        }

        /// <summary>
        /// 添加记录仪-分组关系对照信息到数据库
        /// </summary>
        /// <param name="recID">记录仪编号</param>
        /// <param name="groupID">分组编号</param>
        public static int AddRecorderGroupRelation(int recID, int groupID)
        {
            try
            {
                string sql = string.Empty;
                int query = 0;
                sql = string.Format(@"INSERT INTO MP_GroupRecorder(RecorderId, GroupId) VALUES({0},{1})",
                    recID, groupID);
                query = SQLHelper.ExecuteNonQuery(sql);
                return query;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// 添加噪声分析结果到数据库
        /// </summary>
        /// <param name="result">噪声分析结果对象</param>
        public static int AddNoiseResult(NoiseResult result)
        {
            try
            {
                string sql = string.Empty;
                int query = 0;
                sql = string.Format(@"INSERT INTO DL_NoiseAnalyse(GroupId, RecorderId, MinLeakValue, MinFrequencyValue, UnloadTime, IsLeak, ESA, HistoryFlag, CollTime, EnergyValue, LeakProbability) 
                    VALUES({0},{1},{2},{3},'{4}',{5},{6},{7},'{8}',{9}, {10})",
                    result.GroupID, result.RecorderID, result.LeakAmplitude, result.LeakFrequency, result.UploadTime.ToString("yyyy/MM/dd HH:mm:ss").Replace('-', '/'), result.IsLeak.ToString(), result.ESA, result.UploadFlag, result.ReadTime.ToString("yyyy/MM/dd HH:mm:ss").Replace('-', '/'), result.EnergyValue.ToString("f4"), result.LeakProbability.ToString("f4"));
                query = SQLHelper.ExecuteNonQuery(sql);
                return query;
            }
			catch (Exception ex)
			{
				throw ex;
				//return -1;
			}
        }

        /// <summary>
        /// 添加噪声数据到数据库
        /// </summary>
        /// <param name="data">噪声数据对象</param>
        public static int AddNoiseData(NoiseData data)
        {
            try
            {
                string sql = string.Empty;
                int query = 0;

                string strAmp = string.Empty;
                for (int i = 0; i < data.Amplitude.Length; i++)
                {
                    if (i == data.Amplitude.Length -1)
                        strAmp += data.Amplitude[i];
                    else
                        strAmp += data.Amplitude[i] + ",";
                }
                string strFrq = string.Empty;
                for (int i = 0; i < data.Frequency.Length; i++)
                {
                    if (i == data.Frequency.Length - 1)
                        strFrq += data.Frequency[i];
                    else
                        strFrq += data.Frequency[i] + ",";
                }

                string strDa = string.Empty;
                for (int i = 0; i < data.OriginalData.Length; i++)
                {
                    if (i == data.OriginalData.Length - 1)
                        strDa += data.OriginalData[i];
                    else
                        strDa += data.OriginalData[i] + ",";
                }

                sql = string.Format(@"INSERT INTO DL_Noise_Real(GroupId, RecorderId, LeakValue, FrequencyValue, OriginalData, CollTime, UnloadTime, HistoryFlag) 
                    VALUES({0},{1},'{2}','{3}','{4}','{5}','{6}',{7})",
                    data.GroupID, data.RecorderID, strAmp, strFrq, strDa, data.ReadTime, data.UploadTime, data.UploadFlag);
                query = SQLHelper.ExecuteNonQuery(sql);
                return query;
            }
			catch (Exception ex)
			{
				throw ex;
				//return -1;
			}
        }

        /// <summary>
        /// 从数据库中删除记录仪
        /// </summary>
        /// <param name="recID">记录仪编号</param>
        public static int DeleteRecorder(int recID)
        {
            try
            {
                string sql = string.Empty;
                int query = 0;
                sql = string.Format(@"DELETE FROM MT_RecorderSet WHERE RecorderId = {0}", recID);
                query = SQLHelper.ExecuteNonQuery(sql);

                sql = string.Format(@"DELETE FROM EN_DistanceControl WHERE RecorderId = {0}", recID);
                query = SQLHelper.ExecuteNonQuery(sql);

                DeleteRelationByRecoderId(recID);
				DeleteDataByRecorderId(recID);
				DeleteResultByRecorderId(recID);

                sql = string.Format(@"DELETE FROM EN_NoiseRecorder WHERE RecorderId = {0}", recID);
                query = SQLHelper.ExecuteNonQuery(sql);

                return query;
            }
			catch (Exception ex)
			{
				throw ex;
				//return -1;
			}
        }

        /// <summary>
        /// 从数据库中删除分组
        /// </summary>
        /// <param name="groupID">分组编号</param>
        public static int DeleteGroup(int groupID)
        {
            try
            {
                List<NoiseRecorder> recList = GetRecordersByGroupId(groupID);
                for (int i = 0; i < recList.Count; i++)
                {
                    recList[i].GroupState = 0;
                    UpdateRecorder(recList[i]);
                }

                DeleteRelationByGroupId(groupID);
				DeleteDataByGroupId(groupID);
				DeleteResultByGroupId(groupID);

                string sql = string.Empty;
                int query = 0;
                sql = string.Format(@"DELETE FROM EN_Group WHERE GroupId = {0}", groupID);
                query = SQLHelper.ExecuteNonQuery(sql);

                return query;
            }
			catch (Exception ex)
			{
				throw ex;
				//return -1;
			}
        }

        /// <summary>
        /// 从数据库中删除远传控制器
        /// </summary>
        /// <param name="conID">分组编号</param>
        public static int DeleteControler(int conID)
        {
            try
            {
                string sql = string.Empty;
                int query = 0;
                sql = string.Format(@"DELETE FROM EN_DistanceControl WHERE ControlId = {0}", conID);
                query = SQLHelper.ExecuteNonQuery(sql);

                return query;
            }
			catch (Exception ex)
			{
				throw ex;
				//return -1;
			}
        }

        /// <summary>
        /// 从数据库中删除指定记录仪编号的分组对应关系
        /// </summary>
        /// <param name="recID">记录仪编号</param>
        public static int DeleteRelationByRecoderId(int recID)
        {
            try
            {
                string sql = string.Empty;
                int query = 0;
                sql = string.Format(@"DELETE FROM MP_GroupRecorder WHERE RecorderId = {0}", recID);
                query = SQLHelper.ExecuteNonQuery(sql);
                return query;
            }
			catch (Exception ex)
			{
				throw ex;
				//return -1;
			}
        }

        /// <summary>
        /// 从数据库中删除指定分组编号的记录仪对应关系
        /// </summary>
        /// <param name="groupID">分组编号</param>
        public static int DeleteRelationByGroupId(int groupID)
        {
            try
            {
                string sql = string.Empty;
                int query = 0;
                sql = string.Format(@"DELETE FROM MP_GroupRecorder WHERE GroupId = {0}", groupID);
                query = SQLHelper.ExecuteNonQuery(sql);
                return query;
            }
			catch (Exception ex)
			{
				throw ex;
				//return -1;
			}
        }

        /// <summary>
        /// 从数据库中删除指定分组编号的分组下的指定记录仪编号对应关系
        /// </summary>
        /// <param name="recID">记录仪编号</param>
        /// <param name="groupID">分组编号</param>
        public static int DeleteOneRelation(int recID, int groupID)
        {
            try
            {
                string sql = string.Empty;
                int query = 0;
                sql = string.Format(@"DELETE FROM MP_GroupRecorder WHERE GroupId = {0} AND RecorderId = {1}", groupID, recID);
                query = SQLHelper.ExecuteNonQuery(sql);
                return query;
            }
			catch (Exception ex)
			{
				throw ex;
				//return -1;
			}
        }

        /// <summary>
        /// 从数据库中删除指定分组编号的噪声数据
        /// </summary>
        /// <param name="recID">分组编号</param>
        public static int DeleteDataByGroupId(int groupID)
        {
            try
            {
                string sql = string.Empty;
                int query = 0;
                sql = string.Format(@"DELETE FROM DL_Noise_Real WHERE GroupId = {0}", groupID);
                query = SQLHelper.ExecuteNonQuery(sql);
                return query;
            }
			catch (Exception ex)
			{
				throw ex;
				//return -1;
			}
        }

        /// <summary>
        /// 从数据库中删除指定记录仪编号的噪声数据
        /// </summary>
        /// <param name="groupID">记录仪编号</param>
        public static int DeleteDataByRecorderId(int recID)
        {
            try
            {
                string sql = string.Empty;
                int query = 0;
                sql = string.Format(@"DELETE FROM DL_Noise_Real WHERE RecorderId = {0}", recID);
                query = SQLHelper.ExecuteNonQuery(sql);
                return query;
            }
			catch (Exception ex)
			{
				throw ex;
				//return -1;
			}
        }

        /// <summary>
        /// 从数据库中删除指定分组编号的噪声结果数据
        /// </summary>
        /// <param name="groupID">分组编号</param>
        public static int DeleteResultByGroupId(int groupID)
        {
            try
            {
                string sql = string.Empty;
                int query = 0;
                sql = string.Format(@"DELETE FROM DL_NoiseAnalyse WHERE GroupId = {0}", groupID);
                query = SQLHelper.ExecuteNonQuery(sql);
                return query;
            }
			catch (Exception ex)
			{
				throw ex;
				//return -1;
			}
        }

        /// <summary>
        /// 从数据库中删除指定记录仪编号的噪声结果数据
        /// </summary>
        /// <param name="groupID">记录仪编号</param>
		public static int DeleteResultByRecorderId(int recID)
        {
            try
            {
                string sql = string.Empty;
                int query = 0;
                sql = string.Format(@"DELETE FROM DL_NoiseAnalyse WHERE RecorderId = {0}", recID);
                query = SQLHelper.ExecuteNonQuery(sql);
                return query;
            }
			catch (Exception ex)
			{
				throw ex;
				//return -1;
			}
        }

    }
}
