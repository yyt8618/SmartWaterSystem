using System;
using System.Collections.Generic;
using Entity;
using DAL;
using System.Data;
using System.Data.SqlClient;
using Common;
using System.Collections.Concurrent;

namespace BLL
{
    public class TerminalDataBLL
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("TerminalDataBLL");
        TerminalDataDAL dal = new TerminalDataDAL();

        public DataTable GetTerInfo(TerType type)
        {
            try
            {
                return dal.GetTerInfo(type);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetTerInfo", ex);
                return null;
            }
        }

        #region GPRS数据操作
        public int InsertGPRSPreData(ConcurrentQueue<GPRSPreFrameDataEntity> datas, out string msg)
        {
            msg = "";
            try
            {
                if ((datas == null) || (datas.Count == 0))
                    return 0;
                return dal.InsertGPRSPreData(datas);
            }
            catch (Exception ex)
            {
                logger.ErrorException("InsertGPRSPreData", ex);
                msg = "保存至数据库发生异常";
                return -1;
            }
        }

        public int InsertGPRSFlowData(ConcurrentQueue<GPRSFlowFrameDataEntity> datas, out string msg)
        {
            msg = "";
            try
            {
                if (datas.Count == 0)
                    return 0;
                return dal.InsertGPRSFlowData(datas);
            }
            catch (Exception ex)
            {
                logger.ErrorException("InsertGPRSFlowData", ex);
                msg = "保存至数据库发生异常";
                return -1;
            }
        }

        public int InsertGPRSPrectrlData(ConcurrentQueue<GPRSPrectrlFrameDataEntity> datas ,out string msg)
        {
            msg = "";
            try
            {
                if (datas.Count == 0)
                    return 0;
                return dal.InsertGPRSPrectrlData(datas);
            }
            catch (Exception ex)
            {
                logger.ErrorException("InsertGPRSPrectrlData", ex);
                msg = "保存至数据库发生异常";
                return -1;
            }
        }

        public int InsertGPRSUniversalData(ConcurrentQueue<GPRSUniversalFrameDataEntity> datas, out string msg)
        {
            msg = "";
            try
            {
                if (datas.Count == 0)
                    return 0;
                return dal.InsertGPRSUniversalData(datas);
            }
            catch (Exception ex)
            {
                logger.ErrorException("InsertGPRSUniversalData", ex);
                msg = "保存至数据库发生异常";
                return -1;
            }
        }

        public int InsertGPRSOLWQData(ConcurrentQueue<GPRSOLWQFrameDataEntity> datas, out string msg)
        {
            msg = "";
            try
            {
                if (datas.Count == 0)
                    return 0;
                return dal.InsertGPRSOLWQData(datas);
            }
            catch (Exception ex)
            {
                logger.ErrorException("InsertGPRSOLWQData", ex);
                msg = "保存至数据库发生异常";
                return -1;
            }
        }

        public int InsertGPRSHydrantData(ConcurrentQueue<GPRSHydrantFrameDataEntity> datas, out string msg)
        {
            msg = "";
            try
            {
                if (datas.Count == 0)
                    return 0;
                return dal.InsertGPRSHydrantData(datas);
            }
            catch (Exception ex)
            {
                logger.ErrorException("InsertGPRSHydrantData", ex);
                msg = "保存至数据库发生异常";
                return -1;
            }
        }

        public int InsertWaterworkerData(ConcurrentQueue<GPRSWaterWorkerFrameDataEntity> datas, out string msg)
        {
            msg = "";
            try
            {
                if (datas.Count == 0)
                    return 0;
                return dal.InsertWaterworkerData(datas);
            }
            catch (Exception ex)
            {
                logger.ErrorException("InsertWaterworkerData", ex);
                msg = "保存至数据库发生异常";
                return -1;
            }
        }

        public int InsertAlarmData(ConcurrentQueue<GPRSAlarmFrameDataEntity> datas, out string msg)
        {
            msg = "";
            try
            {
                if (datas.Count == 0)
                    return 0;
                return dal.InsertAlarmData(datas);
            }
            catch (Exception ex)
            {
                logger.ErrorException("InsertAlarmData", ex);
                msg = "保存至数据库发生异常";
                return -1;
            }
        }

        public int InsertGPRSNoiseData(ConcurrentQueue<GPRSNoiseFrameDataEntity> datas, out string msg)
        {
            msg = "";
            try
            {
                if (datas.Count == 0)
                    return 0;
                lock (ConstValue.obj)
                {
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

                    List<UpLoadNoiseDataEntity> lstNoiseData = new List<UpLoadNoiseDataEntity>();
                    while (datas.Count > 0)
                    {
                        GPRSNoiseFrameDataEntity entity = null;
                        try
                        {
                            if (datas.TryDequeue(out entity))
                            {
                                parms_frame[0].Value = 1;
                                parms_frame[1].Value = entity.Frame;
                                parms_frame[2].Value = entity.ModifyTime;
                                if (entity.NoiseData != null)
                                    lstNoiseData.Add(entity.NoiseData);
                                command_frame.ExecuteNonQuery();
                                entity = null;
                            }
                        }
                        catch (Exception iex)
                        {
                            if (entity != null)
                                datas.Enqueue(entity);
                            throw iex;
                        }
                    }

                    if (lstNoiseData != null && lstNoiseData.Count > 0)
                    {
                        HttpDataBLL httpdata = new HttpDataBLL();
                        HTTPRespEntity resp = httpdata.UploadGroups(lstNoiseData);   //处理噪声数据
                        if (resp.code == 1)
                            return 1;
                        else
                        {
                            msg = resp.msg;
                            return resp.code;
                        }
                    }
                    else
                        return 1;
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("InsertGPRSNoiseData", ex);
                msg = "保存至数据库发生异常";
                return -1;
            }
        }

        public List<SendPackageEntity> GetGPRSParm()
        {
            try
            {
                return dal.GetGPRSParm();
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetGPRSParm()", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的报警类型
        /// </summary>
        public Dictionary<int, string> GetAlarmType()
        {
            try
            {
                return dal.GetAlarmType();
            }
            catch(Exception ex)
            {
                logger.ErrorException("GetAlarmType()", ex);
                return null;
            }
        }

        public int UpdateGPRSParmFlag(List<GPRSCmdFlag> ids)
        {
            try
            {
                return dal.UpdateGPRSParmFlag(ids);
            }
            catch (Exception ex)
            {
                logger.ErrorException("UpdateGPRSParmFlag()", ex);
                return -1;
            }
        }
        #endregion

        /// <summary>
        /// 保存通用终端配置 -1:保存异常,1:保存成功
        /// </summary>
        public int SaveTerInfo(int terminalid, string name, string addr, string remark, TerType terType, List<UniversalWayTypeConfigEntity> lstPointID)
        {
            try
            {
                return dal.SaveTerInfo(terminalid, name, addr, remark,terType, lstPointID);
            }
            catch (Exception ex)
            {
                logger.ErrorException("SaveTerInfo", ex);
                return -1;
            }
        }

        public int DeleteUniversalWayTypeConfig(int PointID)
        {
            try
            {
                dal.DeleteUniversalWayTypeConfig(PointID);
                return 1;
            }
            catch (Exception ex)
            {
                logger.ErrorException("DeleteUniversalWayTypeConfig", ex);
                return -1;
            }
        }

        public int DeleteUniversalWayTypeConfig_TerID(int TerminalID,TerType terType)
        {
            try
            {
                dal.DeleteUniversalWayTypeConfig_TerID(TerminalID, terType);
                return 1;
            }
            catch (Exception ex)
            {
                logger.ErrorException("DeleteUniversalWayTypeConfig_TerID", ex);
                return -1;
            }
        }

        /// <summary>
        /// 获取对应终端的采集配置
        /// </summary>
        /// <param name="TerminalID"></param>
        /// <returns></returns>
        public List<UniversalWayTypeConfigEntity> GetUniversalWayTypeConfig(int TerminalID, TerType terType)
        {
            try
            {
                return dal.GetUniversalWayTypeConfig(TerminalID, terType);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetUniversalWayTypeConfig", ex);
                return null;
            }
        }

        public int DeleteTer(TerType type, string TerminalID)
        {
            try
            {
                dal.DeleteTer(type, TerminalID);
                return 1;
            }
            catch (Exception ex)
            {
                logger.ErrorException("DeleteTer", ex);
                return -1;
            }
        }

        public DataTable GetUniversalDataConfig(TerType terType)
        {
            try
            {
                return dal.GetUniversalDataConfig(terType);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetUniversalDataConfig()", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取指定时间范围的终端压力数据
        /// </summary>
        public List<PreDetailDataEntity> GetPreDetail(string TerminalID, DateTime minTime, DateTime maxTime, int interval)
        {
            try
            {
                return dal.GetPreDetail(TerminalID, minTime, maxTime, interval);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetPreDetail", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取指定时间范围的终端流量数据
        /// </summary>
        public List<PreDetailDataEntity> GetFlowDetail(string TerminalID, DateTime minTime, DateTime maxTime, int interval, int datatype)
        {
            try
            {
                return dal.GetFlowDetail(TerminalID, minTime, maxTime, interval, datatype);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetFlowDetail", ex);
                return null;
            }
        }

        public List<PrectrlDetailDataEntity> GetPrectrlDetail(string TerminalID, DateTime minTime, DateTime maxTime, int interval)
        {
            try
            {
                return dal.GetPrectrlDetail(TerminalID, minTime, maxTime, interval);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetPrectrlDetail", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取指定时间范围的终端压力数据
        /// </summary>
        public List<UniversalDetailDataEntity> GetUniversalDetail(string TerminalID, int typeId, DateTime minTime, DateTime maxTime, int interval)
        {
            try
            {
                return dal.GetUniversalDetail(TerminalID, typeId, minTime, maxTime, interval);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetUniversalDetail", ex);
                return null;
            }
        }

        public List<OLWQDetailDataEntity> GetOLWQDetail(string TerminalID, DateTime minTime, DateTime maxTime, int interval, int datatype)
        {
            try
            {
                return dal.GetOLWQDetail(TerminalID, minTime, maxTime, interval, datatype);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetOLWQDetail", ex);
                return null;
            }
        }

        public string GetTerminalName(string TerminalID, TerType tertype)
        {
            try
            {
                return dal.GetTerminalName(TerminalID, tertype);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetTerminalName", ex);
                return "";
            }
        }

        public bool InsertDevGPRSParm(int devId, int DevTypeId, int ctrlCode, int Funcode, string DataValue)
        {
            try
            {
                dal.InsertDevGPRSParm(devId, DevTypeId, ctrlCode, Funcode, DataValue);
                return true;
            }
            catch (Exception ex)
            {
                logger.ErrorException("InsertDevGPRSParm", ex);
                return false;
            }
        }

        /// <summary>
        /// 获取压力数据，包括最新和次新数据
        /// </summary>
        public List<PreDataEntity> GetPreDataTop2(List<int> terminalids)
        {
            try
            {
                return dal.GetPreDataTop2(terminalids);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetPreDataTop2", ex);
                return null;
            }
        }

        public List<FlowDataEntity> GetFlowDataTop2(List<int> terminalids)
        {
            try
            {
                return dal.GetFlowDataTop2(terminalids);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetFlowDataTop2", ex);
                return null;
            }
        }

        public List<PrectrlDataEntity> GetPrectrlData(List<int> terminalids)
        {
            try
            {
                return dal.GetPrectrlData(terminalids);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetPrectrlData", ex);
                return null;
            }
        }

        public DataTable GetOLWQData(List<string> terminalids)
        {
            try
            {
                return dal.GetOLWQData(terminalids);
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetOLWQData", ex);
                return null;
            }
        }

        #region 招测数据处理
        public void AnalysisPre(short Id, Package pack, DataTable dt_config, ref List<string> lstMsg, Dictionary<int, string> lstAlarmType)
        {
            bool addtion_strength = false;  //是否在数据段最后增加了两个字节的电压数据和一个字节的场强数据
            float volvalue = 0;
            Int16 field_strength = 0;
            //招测数据由报警标志(2byte)+招测时间(6byte)+压力1(2byte)=10byte组成
            if (pack.DataLength != 2 + 6 + 2 && pack.DataLength != 2 + 6 + 2 + 3)
            {
                lstMsg.Add("压力帧数据长度[" + pack.DataLength + "]不符合[报警标志(2byte)+招测时间(6byte)+压力1(2byte)]规则");
                return;
            }
            if (pack.DataLength == 2 + 6 + 2 + 3)
            {
                volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 2], pack.Data[pack.DataLength - 3] }, 0)) / 1000;
                field_strength = (Int16)pack.Data[pack.DataLength - 1];
                addtion_strength = true;
            }
            Dictionary<int, string> dictalarms = AlarmProc.GetAlarmName(lstAlarmType, pack.ID3, pack.C1, pack.Data[1], pack.Data[0]);
            if (dictalarms != null && dictalarms.Count > 0)
            {
                foreach (var de in dictalarms)
                {
                    lstMsg.Add(de.Value + " ");
                }
            }

            int year, month, day, hour, minute, sec;
            year = 2000 + Convert.ToInt16(pack.Data[2]);
            month = Convert.ToInt16(pack.Data[3]);
            day = Convert.ToInt16(pack.Data[4]);
            hour = Convert.ToInt16(pack.Data[5]);
            minute = Convert.ToInt16(pack.Data[6]);
            sec = Convert.ToInt16(pack.Data[7]);

            double value = BitConverter.ToInt16(new byte[] { pack.Data[9], pack.Data[8] }, 0);
            if (addtion_strength)
                lstMsg.Add("招测到压力值:" + value.ToString("F2") + "Mpa,电压值:" + volvalue + "V,信号强度:" + field_strength + ", 时间:" + year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec);
            else
                lstMsg.Add("招测到压力值:" + value.ToString("F2") + "Mpa,时间:" + year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec);
        }

        public void AnalysisSim(short Id, Package pack, DataTable dt_config, ref List<string> lstMsg, Dictionary<int, string> lstAlarmType)
        {
            bool addtion_strength = false;  //是否在数据段最后增加了两个字节的电压数据和一个字节的场强数据
            float volvalue = 0;
            Int16 field_strength = 0;
            //招测数据由报警标志(2byte)+招测时间(6byte)+量程(4byte)+校准(2byte)+模拟值(2byte)=16byte组成
            if (pack.DataLength != 2 + 6 + 4 + 2 + 2 && pack.DataLength != 2 + 6 + 4 + 2 + 2 + 3)
            {
                lstMsg.Add("模拟帧数据长度[" + pack.DataLength + "]不符合[报警标志(2byte)+招测时间(6byte)+量程(4byte)+校准(2byte)+模拟值(2byte)]规则");
                return;
            }
            if (pack.DataLength == 2 + 6 + 4 + 2 + 2)
            {
                volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 2], pack.Data[pack.DataLength - 3] }, 0)) / 1000;
                field_strength = (Int16)pack.Data[pack.DataLength - 1];
                addtion_strength = true;
            }
            string sequence = "";
            if (pack.C1 == (byte)UNIVERSAL_COMMAND.CallData_Sim1)
            {
                sequence = "1";
            }
            else if (pack.C1 == (byte)UNIVERSAL_COMMAND.CallData_Sim2)
            {
                sequence = "2";
            }
            //int calibration = BitConverter.ToInt16(new byte[] { pack.Data[7], pack.Data[6] }, 0);

            //float datavalue = 0;

            DataRow[] dr_TerminalDataConfig = null;
            dr_TerminalDataConfig = dt_config.Select("TerminalID='" + Id + "' AND Sequence='" + sequence + "'"); //WayType
            if (dr_TerminalDataConfig != null && dr_TerminalDataConfig.Length > 0)
            {
                //float MaxMeasureRange = dr_TerminalDataConfig[0]["MaxMeasureRange"] != DBNull.Value ? Convert.ToSingle(dr_TerminalDataConfig[0]["MaxMeasureRange"]) : 0;
                //float MaxMeasureRangeFlag = dr_TerminalDataConfig[0]["MaxMeasureRangeFlag"] != DBNull.Value ? Convert.ToSingle(dr_TerminalDataConfig[0]["MaxMeasureRangeFlag"]) : 0;
                //int datawidth = dr_TerminalDataConfig[0]["FrameWidth"] != DBNull.Value ? Convert.ToInt16(dr_TerminalDataConfig[0]["FrameWidth"]) : 0;
                //int precision = dr_TerminalDataConfig[0]["precision"] != DBNull.Value ? Convert.ToInt32(dr_TerminalDataConfig[0]["precision"]) : 0;
                string name = dr_TerminalDataConfig[0]["Name"] != DBNull.Value ? dr_TerminalDataConfig[0]["Name"].ToString().Trim() : "";
                string unit = dr_TerminalDataConfig[0]["Unit"] != DBNull.Value ? dr_TerminalDataConfig[0]["Unit"].ToString().Trim() : "";
                //if (MaxMeasureRangeFlag > 0 && datawidth > 0)
                //{
                //int loopdatalen = 2 + 6 + 4 + 2 + 2;  //循环部分数据宽度 = 时间(6)+配置长度
                //if (pack.C1 == (byte)UNIVERSAL_COMMAND.CallData_Sim1 || pack.C1 == (byte)UNIVERSAL_COMMAND.CallData_Sim2)
                //loopdatalen = 2 + 6 + 4 + 2 + 2;
                //int dataindex = (pack.DataLength) % loopdatalen;
                //if (dataindex != 0)
                //    throw new Exception("招测模拟数据帧长度[" + pack.DataLength + "]不符合长度["+loopdatalen+"]规则");
                //报警标志(2byte)+时间（6byte）+量程（4byte）+校准值（2byte）+模拟数据（2byte）
                Dictionary<int, string> dictalarms = AlarmProc.GetAlarmName(lstAlarmType, pack.ID3, pack.C1, pack.Data[1], pack.Data[0]);
                if (dictalarms != null && dictalarms.Count > 0)
                {
                    foreach (var de in dictalarms)
                    {
                        lstMsg.Add(de.Value + " ");
                    }
                }

                //dataindex = (pack.DataLength) / loopdatalen;
                //for (int i = 0; i < dataindex; i++)
                //{
                int year, month, day, hour, minute, sec;
                year = 2000 + Convert.ToInt16(pack.Data[2]);
                month = Convert.ToInt16(pack.Data[3]);
                day = Convert.ToInt16(pack.Data[4]);
                hour = Convert.ToInt16(pack.Data[5]);
                minute = Convert.ToInt16(pack.Data[6]);
                sec = Convert.ToInt16(pack.Data[7]);

                double range = 0;   //量程
                range += BitConverter.ToInt16(new byte[] { pack.Data[9], pack.Data[8] }, 0);    //整数部分
                range += ((double)BitConverter.ToInt16(new byte[] { pack.Data[11], pack.Data[10] }, 0)) / 1000;    //小数部分
                //(模拟数据-校准值)*量程/系数
                double value = ((double)(BitConverter.ToInt16(new byte[] { pack.Data[15], pack.Data[14] }, 0) - BitConverter.ToInt16(new byte[] { pack.Data[13], pack.Data[12] }, 0))) * range / (ConstValue.UniversalSimRatio);
                if (value < 0)
                    value = 0;
                if (addtion_strength)
                    lstMsg.Add("招测到模拟量" + sequence + "值:" + value.ToString("F2") + unit + ",电压值:" + volvalue + "V,信号强度:" + field_strength + ", 时间: " + year + " - " + month + " - " + day + " " + hour + ":" + minute + ":" + sec);
                else
                    lstMsg.Add("招测到模拟量" + sequence + "值:" + value.ToString("F2") + unit + ", 时间: " + year + " - " + month + " - " + day + " " + hour + ":" + minute + ":" + sec);

                //if (datawidth == 2)
                //datavalue = BitConverter.ToInt16(new byte[] { pack.Data[i * 8 + 9], pack.Data[i * 8 + 8] }, 0);
                //else if (datawidth == 4)
                //    datavalue = BitConverter.ToSingle(new byte[] { pack.Data[i * 8 + 11], pack.Data[i * 8 + 10], pack.Data[i * 8 + 9], pack.Data[i * 8 + 8] }, 0);

                //datavalue = (MaxMeasureRange / MaxMeasureRangeFlag) * (datavalue - calibration);  //根据设置和校准值计算
                //datavalue = Convert.ToSingle(datavalue.ToString("F" + precision));  //精度调整
                //if (datavalue < 0)
                //datavalue = 0;

                //DataRow dr = dt.NewRow();
                //dr["CallDataType"] = name.Trim();
                //dr["CallData"] = datavalue;
                //dr["Unit"] = dr_TerminalDataConfig[0]["Unit"].ToString().Trim();
                //dt.Rows.Add(dr);
                //}
                //}
                //else
                //{
                //    throw new Exception("通用终端[" + Id + "]数据帧解析规则配置错误,数据未能解析！");
                //}
            }
            else
            {
                lstMsg.Add("终端[" + Id + "]招测模拟数据未配置数据帧解析规则,数据未能解析！");
            }
        }

        public void AnalysisPluse(short Id, Package pack, DataTable dt_config, ref List<string> lstMsg, Dictionary<int, string> lstAlarmType)
        {
            float volvalue = -1;
            Int16 field_strength = -1;
            //时间(6byte)+脉冲单位(1byte)+数据(16byte)
            if (pack.DataLength != 6 + 1 + 16 && pack.DataLength!= 6 + 1 + 16+3)
            {
                lstMsg.Add("脉冲帧数据长度[" + pack.DataLength + "]不符合[报警标志时间(6byte)+脉冲单位(1byte)+数据(16byte)]规则");
                return;
            }
            if (pack.DataLength == 6 + 1 + 16)
            {
                volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 2], pack.Data[pack.DataLength - 3] }, 0)) / 1000;
                field_strength = (Int16)pack.Data[pack.DataLength - 1];
            }
            DataRow[] dr_TerminalDataConfig = null;
            dr_TerminalDataConfig = dt_config.Select("TerminalID='" + Id + "' AND Sequence IN ('4','5','6','7')", "Sequence"); //WayType
            if (dr_TerminalDataConfig != null && dr_TerminalDataConfig.Length > 0)
            {
                int waycount = dr_TerminalDataConfig.Length;
                float PluseUnits = 0;
                string[] Names = new string[waycount];
                string[] Units = new string[waycount];
                int[] config_ids = new int[waycount];

                switch (pack.Data[6])       //脉冲单位0代表0.01、1代表0.1、2代表0.2    3代表0.5、4代表1、5代表10、6代表100
                {
                    case 0:
                        PluseUnits = 0.01f;
                        break;
                    case 1:
                        PluseUnits = 0.1f;
                        break;
                    case 2:
                        PluseUnits = 0.2f;
                        break;
                    case 3:
                        PluseUnits = 0.5f;
                        break;
                    case 4:
                        PluseUnits = 1f;
                        break;
                    case 5:
                        PluseUnits = 10f;
                        break;
                    case 6:
                        PluseUnits = 100f;
                        break;
                }

                int year, month, day, hour, minute, sec;
                year = 2000 + Convert.ToInt16(pack.Data[0]);
                month = Convert.ToInt16(pack.Data[1]);
                day = Convert.ToInt16(pack.Data[2]);
                hour = Convert.ToInt16(pack.Data[3]);
                minute = Convert.ToInt16(pack.Data[4]);
                sec = Convert.ToInt16(pack.Data[5]);
                for (int i = 0; i < waycount; i++)
                {
                    Names[i] = dr_TerminalDataConfig[i]["Name"] != DBNull.Value ? dr_TerminalDataConfig[i]["Name"].ToString().Trim() : "";
                    Units[i] = dr_TerminalDataConfig[i]["Unit"] != DBNull.Value ? dr_TerminalDataConfig[i]["Unit"].ToString().Trim() : "";
                    //config_ids[i] = dr_TerminalDataConfig[i]["ID"] != DBNull.Value ? Convert.ToInt32(dr_TerminalDataConfig[i]["ID"]) : 0;
                }
                //时间（6byte）+脉冲单位（1byte）+4路脉冲个数（16byte）脉冲数据＝脉冲个数* 脉冲单位代表的数值 脉冲单位0代表0.01、1代表0.1、2代表0.2    3代表0.5、4代表1、5代表10、6代表100

                string strmsg = string.Format("招测到脉冲数据,脉冲单位:{0},{1}:{2}{3},{4}:{5}{6},{7}:{8}{9},{10}:{11}{12},电压值:{13},信号强度:{14},时间:{15}-{16}-{17} {18}:{19}:{20}",
                PluseUnits,
                waycount > 0 ? Names[0] : "第一路脉冲数据",
                BitConverter.ToInt32(new byte[] { pack.Data[10], pack.Data[9], pack.Data[8], pack.Data[7] }, 0) * PluseUnits,
                waycount > 0 ? Units[0] : "",

                waycount > 1 ? Names[1] : "第二路脉冲数据",
                BitConverter.ToInt32(new byte[] { pack.Data[14], pack.Data[13], pack.Data[12], pack.Data[11] }, 0) * PluseUnits,
                waycount > 1 ? Units[1] : "",

                waycount > 2 ? Names[2] : "第三路脉冲数据",
                BitConverter.ToInt32(new byte[] { pack.Data[18], pack.Data[17], pack.Data[16], pack.Data[15] }, 0) * PluseUnits,
                waycount > 2 ? Units[2] : "",

                waycount > 3 ? Names[3] : "第四路脉冲数据",
                BitConverter.ToInt32(new byte[] { pack.Data[22], pack.Data[21], pack.Data[20], pack.Data[19] }, 0) * PluseUnits,
                waycount > 3 ? Units[3] : "",
                volvalue,field_strength,
                year, month, day, hour, minute, sec
                );
                lstMsg.Add(strmsg);

            }
            else
            {
                lstMsg.Add("终端[" + Id + "]招测脉冲数据未配置数据帧解析规则,数据未能解析！");
            }
        }
        //485流量
        public void AnalysisRS485Flow(short Id, Package pack, DataTable dt_config, ref List<string> lstMsg, Dictionary<int, string> lstAlarmType)
        {
            bool addtion_strength = false;  //是否在数据段最后增加了两个字节的电压数据和一个字节的场强数据
            float volvalue = 0;
            Int16 field_strength = 0;
            //报警标志(2byte)+时间（6byte）+瞬时流量(4byte)+正向累积流量(4byte)+反向累积流量(4byte)
            if (pack.DataLength != 2 + 6 + 4 + 4 + 4 && pack.DataLength != 2 + 6 + 4 + 4 + 4 +3)
            {
                lstMsg.Add("485帧数据长度[" + pack.DataLength + "]不符合[报警标志(2byte)+时间(6byte)+瞬时流量(4byte)+正向累积流量(4byte)+反向累积流量(4byte)]规则");
                return;
            }
            if (pack.DataLength == 2 + 6 + 4 + 4 + 4 +3)
            {
                volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 2], pack.Data[pack.DataLength - 3] }, 0)) / 1000;
                field_strength = (Int16)pack.Data[pack.DataLength - 1];
                addtion_strength = true;
            }
            Dictionary<int, string> dictalarms = AlarmProc.GetAlarmName(lstAlarmType, pack.ID3, pack.C1, pack.Data[1], pack.Data[0]);
            if (dictalarms != null && dictalarms.Count > 0)
            {
                foreach (var de in dictalarms)
                {
                    lstMsg.Add(de.Value + " ");
                }
            }

            int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
            year = 2000 + Convert.ToInt16(pack.Data[2]);
            month = Convert.ToInt16(pack.Data[3]);
            day = Convert.ToInt16(pack.Data[4]);
            hour = Convert.ToInt16(pack.Data[5]);
            minute = Convert.ToInt16(pack.Data[6]);
            sec = Convert.ToInt16(pack.Data[7]);

            float instantflow = BitConverter.ToSingle(new byte[] { pack.Data[11], pack.Data[10], pack.Data[9], pack.Data[8] }, 0);  //瞬时流量
            float forwardflow = BitConverter.ToSingle(new byte[] { pack.Data[15], pack.Data[14], pack.Data[13], pack.Data[12] }, 0);    //正向流量
            float reverseflow = BitConverter.ToSingle(new byte[] { pack.Data[19], pack.Data[18], pack.Data[17], pack.Data[16] }, 0);    //反向流量
            if (addtion_strength)
                lstMsg.Add("招测到RS485 1路瞬时流量:" + instantflow + ",正向流量:" + forwardflow + ",反向流量:" + reverseflow + ", 电压值: " + volvalue + "V, 信号强度: " + field_strength + ", 时间:" + year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec);
            else
                lstMsg.Add("招测到RS485 1路瞬时流量:" + instantflow + ",正向流量:" + forwardflow + ",反向流量:" + reverseflow + ", 时间:" + year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec);
        }

        public void AnalysisRS485(short Id, Package pack, DataTable dt_config, ref List<string> lstMsg, Dictionary<int, string> lstAlarmType)
        {
            float volvalue = 0;
            Int16 field_strength = 0;
            //报警标志(2byte)+时间（6byte）+ 数据(?byte)
            if (pack.DataLength < 2 + 6 +3)
            {
                lstMsg.Add("485帧数据长度[" + pack.DataLength + "]不符合[报警标志(2byte)+时间(6byte)+数据]规则");
                return;
            }
            else if (pack.DataLength == 2+6)
            {
                lstMsg.Add("485帧数据为空");
                return;
            }
            volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 2], pack.Data[pack.DataLength - 3] }, 0)) / 1000;
            field_strength = (Int16)pack.Data[pack.DataLength - 1];

            string sequence = "";
            string name = "";
            if (pack.C1 == (byte)UNIVERSAL_COMMAND.CallData_RS4851)
            {
                sequence = "9"; name = "RS485 1路";
            }
            else if (pack.C1 == (byte)UNIVERSAL_COMMAND.CallData_RS4852)
            {
                sequence = "10"; name = "RS485 2路";
            }
            else if (pack.C1 == (byte)UNIVERSAL_COMMAND.CallData_RS4853)
            {
                sequence = "11"; name = "RS485 3路";
            }
            else if (pack.C1 == (byte)UNIVERSAL_COMMAND.CallData_RS4854)
            {
                sequence = "12"; name = "RS485 4路";
            }
            //报警标志(2byte) + 时间（6byte）+485采集数据数据
            int calibration = BitConverter.ToInt16(new byte[] { pack.Data[1], pack.Data[0] }, 0);

            Dictionary<int, string> dictalarms = AlarmProc.GetAlarmName(lstAlarmType, pack.ID3, pack.C1, pack.Data[1], pack.Data[0]);
            if (dictalarms != null && dictalarms.Count > 0)
            {
                foreach (var de in dictalarms)
                {
                    lstMsg.Add(de.Value + " ");
                }
            }

            int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
            year = 2000 + Convert.ToInt16(pack.Data[2]);
            month = Convert.ToInt16(pack.Data[3]);
            day = Convert.ToInt16(pack.Data[4]);
            hour = Convert.ToInt16(pack.Data[5]);
            minute = Convert.ToInt16(pack.Data[6]);
            sec = Convert.ToInt16(pack.Data[7]);

            byte[] datas = new byte[pack.DataLength - 8];
            Array.Copy(pack.Data, 8, datas, 0, datas.Length);
            string str_datas = ConvertHelper.ByteToString(datas, datas.Length -3);

            lstMsg.Add("招测到" + name + "数据:" + str_datas + ", 电压值: " + volvalue + "V, 信号强度: " + field_strength + ",时间:" + year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec);
            /*
            float datavalue = 0;

            DataRow[] dr_TerminalDataConfig = null;
            DataRow[] dr_DataConfig_Child = null;
            bool ConfigHaveChild = false;

            dr_TerminalDataConfig = dt_config.Select("TerminalID='" + Id + "' AND Sequence='" + sequence + "'"); //WayType
            if (dr_TerminalDataConfig != null && dr_TerminalDataConfig.Length > 0)
            {
                dr_DataConfig_Child = dt_config.Select("ParentID='" + dr_TerminalDataConfig[0]["ID"].ToString().Trim() + "'", "Sequence");
                if (dr_DataConfig_Child != null && dr_DataConfig_Child.Length > 0)
                {
                    ConfigHaveChild = true;
                    dr_TerminalDataConfig = dr_DataConfig_Child;  //有子节点配置时，使用子节点配置
                }
            }
            if (dr_TerminalDataConfig != null && dr_TerminalDataConfig.Length > 0)
            {
                int waycount = dr_TerminalDataConfig.Length;
                //float[] MaxMeasureRanges = new float[waycount];
                //float[] MaxMeasureRangeFlags = new float[waycount];
                //int[] DataWidths = new int[waycount];
                //int[] Precisions = new int[waycount];
                string[] Names = new string[waycount];
                string[] Units = new string[waycount];
                int[] config_ids = new int[waycount];

                int topdatawidth = 0;
                for (int i = 0; i < waycount; i++)
                {
                    //MaxMeasureRanges[i] = dr_TerminalDataConfig[i]["MaxMeasureRange"] != DBNull.Value ? Convert.ToSingle(dr_TerminalDataConfig[i]["MaxMeasureRange"]) : 0;
                    //MaxMeasureRangeFlags[i] = dr_TerminalDataConfig[0]["MaxMeasureRangeFlag"] != DBNull.Value ? Convert.ToSingle(dr_TerminalDataConfig[0]["MaxMeasureRangeFlag"]) : 0;
                    //DataWidths[i] = dr_TerminalDataConfig[i]["FrameWidth"] != DBNull.Value ? Convert.ToInt16(dr_TerminalDataConfig[i]["FrameWidth"]) : 0;
                    //Precisions[i] = dr_TerminalDataConfig[i]["precision"] != DBNull.Value ? Convert.ToInt32(dr_TerminalDataConfig[i]["precision"]) : 0;
                    Names[i] = dr_TerminalDataConfig[i]["Name"] != DBNull.Value ? dr_TerminalDataConfig[i]["Name"].ToString().Trim() : "";
                    Units[i] = dr_TerminalDataConfig[i]["Unit"] != DBNull.Value ? dr_TerminalDataConfig[i]["Unit"].ToString().Trim() : "";
                    config_ids[i] = dr_TerminalDataConfig[i]["ID"] != DBNull.Value ? Convert.ToInt32(dr_TerminalDataConfig[i]["ID"]) : 0;
                    //topdatawidth += DataWidths[i];
                }

                if (topdatawidth > 0)
                {
                    int loopdatalen = 6 + topdatawidth;  //循环部分数据宽度
                    int dataindex = (pack.DataLength) % loopdatalen;
                    if (dataindex != 0)
                        throw new Exception(name+"帧数据长度[" + pack.DataLength + "]不符合" + loopdatalen + "*n规则");
                    dataindex = (pack.DataLength) / loopdatalen;
                    for (int i = 0; i < dataindex; i++)
                    {
                        year = 2000 + Convert.ToInt16(pack.Data[i * loopdatalen]);
                        month = Convert.ToInt16(pack.Data[i * loopdatalen + 1]);
                        day = Convert.ToInt16(pack.Data[i * loopdatalen + 2]);
                        hour = Convert.ToInt16(pack.Data[i * loopdatalen + 3]);
                        minute = Convert.ToInt16(pack.Data[i * loopdatalen + 4]);
                        sec = Convert.ToInt16(pack.Data[i * loopdatalen + 5]);

                        int freindex = 0;
                        for (int j = 0; j < waycount; j++)
                        {
                            //if (DataWidths[j] == 2)
                            //{
                            //    datavalue = BitConverter.ToInt16(new byte[] { pack.Data[i * loopdatalen + 7 + freindex], pack.Data[i * loopdatalen + 6 + freindex] }, 0);
                            //    freindex += 2;
                            //}
                            //else if (DataWidths[j] == 4)
                            //{
                            //    datavalue = BitConverter.ToInt32(new byte[] { pack.Data[i * loopdatalen + 9 + freindex], pack.Data[i * loopdatalen + 8 + freindex], pack.Data[i * loopdatalen + 7 + freindex], pack.Data[i * loopdatalen + 6 + freindex] }, 0);
                            //    freindex += 4;
                            //}

                            //datavalue = MaxMeasureRanges[j] * datavalue;  //系数
                            datavalue = Convert.ToSingle(datavalue.ToString("F2"));

                        }
                    }
                }
                else
                {
                    lstMsg.Add("通用终端[" + Id + "]数据帧解析规则配置错误,数据未能解析！");
                }
            }
            else
            {
                lstMsg.Add("通用终端[" + Id + "]未配置数据帧解析规则,数据未能解析！");
            }
            */
        }
        #endregion

    }
}
