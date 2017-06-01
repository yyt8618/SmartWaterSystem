using BLL;
using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GCGPRSService
{
    public class Protol68
    {
        public void ProcData(StateObject state, Package pack, string str_frame, out bool bNeedCheckTime)
        {
            bNeedCheckTime = false;
            float volvalue = -1;  //电压,如果是没有这个电压值的,赋值为-1，保存至数据库时根据-1保存空
            Int16 field_strength = -1; //场强(0-31,99表示没信号)
            object RectifyResult = null;//纠偏结果,null表示运算失败或者异常
            string CalcExpress = "";    //运算表达式
            if (pack.ID3 == (byte)ConstValue.DEV_TYPE.Data_CTRL || pack.ID3 == (byte)ConstValue.DEV_TYPE.UNIVERSAL_CTRL)
            {
                string TerName = pack.ID3 == (byte)Entity.ConstValue.DEV_TYPE.Data_CTRL ? "压力流量终端" : "通用终端";
                #region 压力终端
                bool addtion_voldata = false;   //是否在数据段最后增加了两个字节的电压数据
                bool addtion_strength = false;  //是否在数据段最后增加了两个字节的电压数据和一个字节的场强数据
                if (pack.C1 == (byte)GPRS_READ.READ_PREDATA)  //从站向主站发送压力采集数据
                {
                    int dataindex = (pack.DataLength - 2 - 1) % 8;
                    if (dataindex != 0)
                    {
                        if (dataindex == 2)
                        {
                            dataindex = (pack.DataLength - 2 - 1 - 2) / 8;      //带电压
                            addtion_voldata = true;
                        }
                        else if (dataindex == 3)
                        {
                            dataindex = (pack.DataLength - 2 - 1 - 3) / 8;      //带电压和场强
                            addtion_strength = true;
                        }
                        else
                        {
                            throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(2+1+8*n)或(2+1+8*n+2)规则");  //GPRS远程压力终端在数据段最后增加两个字节的电压数据
                        }
                    }
                    else
                        dataindex = (pack.DataLength - 2 - 1) / 8;

                    StringBuilder str_alarm = new StringBuilder();
                    int preFlag = 0;

                    /****************************************宿州校准压力值********************************************/
                    //double[] RectifyValue = new double[] {  //修偏数组
                    //    -0.009, 0, -0.03, 0.013, -0.029, -0.029, 0, 0, 0, -0.011,
                    //    -0.008, -0.026, -0.009, -0.006, -0.009, -0.021, 0, -0.01, 0, -0.01,
                    //    -0.007, -0.019, -0.021, -0.04, -0.01, -0.007, -0.014, -0.013, 0, -0.023 };
                    /**************************************************************************************************/

                    preFlag = Convert.ToInt16(pack.Data[2]);

                    GPRSPreFrameDataEntity framedata = new GPRSPreFrameDataEntity();
                    framedata.TerId = pack.ID.ToString();
                    framedata.ModifyTime = DateTime.Now;
                    framedata.Frame = str_frame;

                    int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                    float pressuevalue = 0;

                    if (addtion_voldata)
                        volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 1], pack.Data[pack.DataLength - 2] }, 0)) / 1000;
                    if (addtion_strength)
                    {
                        volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 2], pack.Data[pack.DataLength - 3] }, 0)) / 1000;
                        field_strength = (Int16)pack.Data[pack.DataLength - 1];
                    }
                    for (int i = 0; i < dataindex; i++)
                    {
                        year = 2000 + Convert.ToInt16(pack.Data[i * 8 + 3]);
                        month = Convert.ToInt16(pack.Data[i * 8 + 4]);
                        day = Convert.ToInt16(pack.Data[i * 8 + 5]);
                        hour = Convert.ToInt16(pack.Data[i * 8 + 6]);
                        minute = Convert.ToInt16(pack.Data[i * 8 + 7]);
                        sec = Convert.ToInt16(pack.Data[i * 8 + 8]);

                        pressuevalue = BitConverter.ToInt16(new byte[] { pack.Data[i * 8 + 10], pack.Data[i * 8 + 9] }, 0);
                        RectifyResult = RectifyCalc(pack, out CalcExpress, pressuevalue.ToString());
                        if (RectifyResult != null)  //运算失败
                        {
                            CalcExpress = "(计算式:" + CalcExpress + ")";
                            pressuevalue = Convert.ToSingle(RectifyResult);
                        }
                        else
                        {
                            CalcExpress = "";
                            pressuevalue = pressuevalue / 1000;
                        }
                        GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.PreTer, string.Format("index({0})|{1}[{2}]|压力标志({3})|采集时间({4})|压力值:{5}MPa{6}|电压值:{7}V|信号强度:{8}",
                            i, TerName, pack.ID, preFlag, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, pressuevalue, CalcExpress, volvalue, field_strength)));

                        GPRSPreDataEntity data = new GPRSPreDataEntity();
                        data.PreValue = pressuevalue;
                        data.Voltage = volvalue;
                        data.FieldStrength = field_strength;
                        try
                        {
                            data.ColTime = new DateTime(year, month, day, hour, minute, sec);
                        }
                        catch { data.ColTime = ConstValue.MinDateTime; }
                        bNeedCheckTime = GlobalValue.Instance.SocketMag.NeedCheckTime(data.ColTime);
                        framedata.lstPreData.Add(data);
                    }

                    Dictionary<int, string> dictalarms = AlarmProc.GetAlarmName(GlobalValue.Instance.lstAlarmType, pack.ID3, pack.C1, pack.Data[1], pack.Data[0]);
                    if (dictalarms != null && dictalarms.Count > 0)
                    {
                        GPRSAlarmFrameDataEntity alarmframedata = new GPRSAlarmFrameDataEntity();
                        alarmframedata.Frame = str_frame;
                        alarmframedata.TerId = pack.DevID.ToString();
                        alarmframedata.TerminalType = TerType.PreTer;
                        try
                        {
                            alarmframedata.ModifyTime = framedata.ModifyTime;  //报警时间使用统一时间,以便确定最后一次是正常还是报警的判断// new DateTime(year, month, day, hour, minute, sec);
                        }
                        catch { alarmframedata.ModifyTime = ConstValue.MinDateTime; }

                        alarmframedata.AlarmId = new List<int>();
                        foreach (var de in dictalarms)
                        {
                            alarmframedata.AlarmId.Add(de.Key);
                            str_alarm.Append(de.Value + " ");

                        }
                        GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.Alarm, string.Format("{0}[{1}] {2}",
                            TerName, pack.ID, str_alarm)));

                        GlobalValue.Instance.GPRS_AlarmFrameData.Enqueue(alarmframedata);
                        GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertAlarm);
                    }

                    GlobalValue.Instance.GPRS_PreFrameData.Enqueue(framedata);  //通知存储线程处理
                    GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertPreValue);
                }
                else if (pack.C1 == (byte)GPRS_READ.READ_FLOWDATA)  //从站向主站发送流量采集数据
                {
                    bool isBCD = false;  //数据是否为BCD码
                    int dataindex = (pack.DataLength - 2 - 1) % 18;
                    if (dataindex != 0)
                    {
                        if (dataindex == 2)   //有带电压值
                        {
                            addtion_voldata = true;
                            dataindex = 0;
                        }
                        else if (dataindex == 3)
                        {
                            addtion_strength = true;
                            dataindex = 0;
                        }
                        else
                        {
                            dataindex = (pack.DataLength - 2 - 1) % 10;
                            isBCD = true;
                            if (dataindex == 2)     //有带电压值
                            {
                                addtion_voldata = true;
                                dataindex = 0;
                            }
                            else if (dataindex == 3)
                            {
                                addtion_strength = true;
                                dataindex = 0;
                            }
                        }

                    }
                    if (dataindex != 0)
                    {
                        throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(2+1+(18/10)*n)或(2+1+(18/10)*n+2)规则");
                    }

                    if (isBCD)
                    {
                        if (addtion_voldata)
                            dataindex = (pack.DataLength - 2 - 1 - 2) / 10;
                        else if (addtion_strength)
                            dataindex = (pack.DataLength - 2 - 1 - 3) / 10;
                        else
                            dataindex = (pack.DataLength - 2 - 1) / 10;
                    }
                    else
                    {
                        if (addtion_voldata)
                            dataindex = (pack.DataLength - 2 - 1 - 2) / 18;
                        else if (addtion_strength)
                            dataindex = (pack.DataLength - 2 - 1 - 3) / 18;
                        else
                            dataindex = (pack.DataLength - 2 - 1) / 18;
                    }

                    int alarmflag = 0;
                    int flowFlag = 0;

                    //报警标志
                    alarmflag = BitConverter.ToInt16(new byte[] { pack.Data[0], pack.Data[1] }, 0);
                    flowFlag = Convert.ToInt16(pack.Data[2]);
                    if (addtion_voldata)
                        volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 1], pack.Data[pack.DataLength - 2] }, 0)) / 1000;
                    else if (addtion_strength)
                    {
                        volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 2], pack.Data[pack.DataLength - 3] }, 0)) / 1000;
                        field_strength = (Int16)pack.Data[pack.DataLength - 1];
                    }

                    GPRSFlowFrameDataEntity framedata = new GPRSFlowFrameDataEntity();
                    framedata.TerId = pack.ID.ToString();
                    framedata.ModifyTime = DateTime.Now;
                    framedata.Frame = str_frame;

                    int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                    double forward_flowvalue = 0, reverse_flowvalue = 0, instant_flowvalue = 0;
                    for (int i = 0; i < dataindex; i++)
                    {
                        year = 2000 + Convert.ToInt16(pack.Data[i * 18 + 3]);
                        month = Convert.ToInt16(pack.Data[i * 18 + 4]);
                        day = Convert.ToInt16(pack.Data[i * 18 + 5]);
                        hour = Convert.ToInt16(pack.Data[i * 18 + 6]);
                        minute = Convert.ToInt16(pack.Data[i * 18 + 7]);
                        sec = Convert.ToInt16(pack.Data[i * 18 + 8]);

                        if (!isBCD)
                        {
                            //前向流量
                            forward_flowvalue = BitConverter.ToSingle(new byte[] { pack.Data[i * 18 + 12], pack.Data[i * 18 + 11], pack.Data[i * 18 + 10], pack.Data[i * 18 + 9] }, 0);
                            //反向流量
                            reverse_flowvalue = BitConverter.ToSingle(new byte[] { pack.Data[i * 18 + 16], pack.Data[i * 18 + 15], pack.Data[i * 18 + 14], pack.Data[i * 18 + 13] }, 0);
                            //瞬时流量
                            instant_flowvalue = BitConverter.ToSingle(new byte[] { pack.Data[i * 18 + 20], pack.Data[i * 18 + 19], pack.Data[i * 18 + 18], pack.Data[i * 18 + 17] }, 0);

                            GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.PreTer, string.Format("index({0})|{1}[{2}]|报警标志({3})|流量标志({4})|采集时间({5})|正向流量值:{6}|反向流量值:{7}|瞬时流量值:{8}|电压值:{9}V|信号强度:{10}",
                                i, TerName, pack.ID, alarmflag, flowFlag, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, forward_flowvalue, reverse_flowvalue, instant_flowvalue, volvalue, field_strength)));
                        }
                        else
                        {
                            string flowvalue = String.Format("{0:X2}", pack.Data[i * 18 + 12]) + String.Format("{0:X2}", pack.Data[i * 18 + 11]) + String.Format("{0:X2}", pack.Data[i * 18 + 10]) + String.Format("{0:X2}", pack.Data[i * 18 + 9]);
                            forward_flowvalue = Convert.ToDouble(flowvalue) / 100;
                            GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.PreTer, string.Format("index({0})|{1}[{2}]|报警标志({3})|流量标志({4})|采集时间({5})|日累计流量值:{6}|电压值:{7}V|信号强度:{8}",
                                i, TerName, pack.ID, alarmflag, flowFlag, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, forward_flowvalue, volvalue, field_strength)));
                        }

                        GPRSFlowDataEntity data = new GPRSFlowDataEntity();
                        data.Forward_FlowValue = forward_flowvalue;
                        data.Reverse_FlowValue = reverse_flowvalue;
                        data.Instant_FlowValue = instant_flowvalue;
                        data.Voltage = volvalue;
                        data.FieldStrength = field_strength;
                        try
                        {
                            data.ColTime = new DateTime(year, month, day, hour, minute, sec);
                        }
                        catch { data.ColTime = ConstValue.MinDateTime; }
                        bNeedCheckTime = GlobalValue.Instance.SocketMag.NeedCheckTime(data.ColTime);
                        framedata.lstFlowData.Add(data);
                    }
                    GlobalValue.Instance.GPRS_FlowFrameData.Enqueue(framedata);  //通知存储线程处理
                    GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertFlowValue);
                }
                else if (pack.C1 == (byte)GPRS_READ.READ_ALARMINFO)  //从站向主站发送设备报警信息
                {
                    if (pack.DataLength != 7 && pack.DataLength != 9 && pack.DataLength != 11)   //pack.DataLength == 9 带电压值;pack.DataLength == 11 带电压值和信号强度且报警标志为2个字节(旧的为1个字节)
                    {
                        throw new ArgumentException(DateTime.Now.ToString() + " " + "帧数据长度[" + pack.DataLength + "]不符合7、9、11位规则");
                    }

                    StringBuilder str_alarm = new StringBuilder();

                    if (pack.DataLength == 9)   //pack.DataLength == 9 带电压值
                    {
                        volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 1], pack.Data[pack.DataLength - 2] }, 0)) / 1000;
                    }
                    else if (pack.DataLength == 11)
                    {
                        volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 2], pack.Data[pack.DataLength - 3] }, 0)) / 1000;
                        field_strength = (Int16)pack.Data[pack.DataLength - 1];
                    }

                    int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                    year = 2000 + Convert.ToInt16(pack.Data[1]);
                    month = Convert.ToInt16(pack.Data[2]);
                    day = Convert.ToInt16(pack.Data[3]);
                    hour = Convert.ToInt16(pack.Data[4]);
                    minute = Convert.ToInt16(pack.Data[5]);
                    sec = Convert.ToInt16(pack.Data[6]);


                    Dictionary<int, string> dictalarms = null;
                    if (pack.DataLength == 11)      //pack.DataLength == 11 报警标志为2个字节(旧的为1个字节)
                    {
                        dictalarms = AlarmProc.GetAlarmName(GlobalValue.Instance.lstAlarmType, pack.ID3, pack.C1, pack.Data[1], pack.Data[0]);

                        year = 2000 + Convert.ToInt16(pack.Data[2]);
                        month = Convert.ToInt16(pack.Data[3]);
                        day = Convert.ToInt16(pack.Data[4]);
                        hour = Convert.ToInt16(pack.Data[5]);
                        minute = Convert.ToInt16(pack.Data[6]);
                        sec = Convert.ToInt16(pack.Data[7]);
                    }
                    else
                    {
                        dictalarms = AlarmProc.GetAlarmName(GlobalValue.Instance.lstAlarmType, pack.ID3, pack.C1, pack.Data[0]);

                        year = 2000 + Convert.ToInt16(pack.Data[1]);
                        month = Convert.ToInt16(pack.Data[2]);
                        day = Convert.ToInt16(pack.Data[3]);
                        hour = Convert.ToInt16(pack.Data[4]);
                        minute = Convert.ToInt16(pack.Data[5]);
                        sec = Convert.ToInt16(pack.Data[6]);
                    }
                    if (month == 0)
                        month = 1;
                    if (day == 0)
                        day = 1;

                    if (dictalarms != null && dictalarms.Count > 0)
                    {
                        GPRSAlarmFrameDataEntity alarmframedata = new GPRSAlarmFrameDataEntity();
                        alarmframedata.Frame = str_frame;
                        alarmframedata.TerId = pack.DevID.ToString();
                        alarmframedata.TerminalType = TerType.PreTer;
                        alarmframedata.ModifyTime = new DateTime(year, month, day, hour, minute, sec);
                        alarmframedata.AlarmId = new List<int>();
                        foreach (var de in dictalarms)
                        {
                            alarmframedata.AlarmId.Add(de.Key);
                            str_alarm.Append(de.Value + " ");
                        }
                        alarmframedata.Voltage = volvalue;
                        alarmframedata.FieldStrength = field_strength;

                        GlobalValue.Instance.GPRS_AlarmFrameData.Enqueue(alarmframedata);
                        GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertAlarm);

                        GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.Alarm, string.Format("{0}[{1}]{2}|时间({3})|电压值:{4}V|信号强度:{5}",
                         TerName, pack.ID, str_alarm, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, volvalue, field_strength)));
                    }
                    else
                    {
                        GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.PreTer, string.Format("{0}[{1}]{2}|时间({3})|电压值:{4}V|信号强度:{5}",
                             TerName, pack.ID, str_alarm, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, volvalue, field_strength)));
                    }
                    bNeedCheckTime = GlobalValue.Instance.SocketMag.NeedCheckTime(new DateTime(year, month, day, hour, minute, sec));
                }
                else if (pack.ID3 == (byte)Entity.ConstValue.DEV_TYPE.Data_CTRL && pack.C1 == (byte)GPRS_READ.READ_PREFLOWDATA) //从站向主站发送流量采集数据(压力流量终端)
                {
                    #region 上海肯特(KENT)水表
                    if (pack.Data[2] == 0x02)   //上海肯特(KENT)水表
                    {
                        int dataindex = (pack.DataLength - 2 - 2 - 1) % (6 + 36);  //两字节报警,1字节厂家类型,
                        if (dataindex != 0)
                        {
                            dataindex = (pack.DataLength - 2 - 2 - 2) % (6 + 36);  //两字节报警,1字节厂家类型,一个字节信号强度
                            if (dataindex != 0)
                                throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(2+1+(6+36)*n)规则");
                            dataindex = (pack.DataLength - 2 - 2 - 2) / (6 + 36);
                        }
                        else
                            dataindex = (pack.DataLength - 2 - 2 - 1) / (6 + 36);

                        GPRSFlowFrameDataEntity framedata = new KERTFlow().ProcessData(dataindex, TerType.PreTer, "压力流量终端", pack.ID.ToString(), str_frame, pack.Data, pack.DataLength, out bNeedCheckTime);

                        GlobalValue.Instance.GPRS_FlowFrameData.Enqueue(framedata);  //通知存储线程处理
                        GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertFlowValue);
                    }
                    else
                    {
                        GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.UnResolve, "压力流量终端[" + pack.ID + "]错误未知水表类型!"));
                    }
                    #endregion
                }
                #endregion

                #region 心跳
                else if (pack.C1 == (byte)GPRS_READ.READ_UNIVERSAL_HEART)
                {
                    GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.Other, TerName + "[" + pack.DevID + "]心跳!"));
                }
                #endregion

                #region 通用终端
                else if (pack.C1 == (byte)GPRS_READ.READ_UNIVERSAL_SIM)  //接受通用终端发送的模拟数据(包含招测数据)
                {
                    #region 通用终端模拟数据
                    GPRSUniversalFrameDataEntity framedata = new GPRSUniversalFrameDataEntity();
                    framedata.TerId = pack.DevID.ToString();
                    framedata.ModifyTime = DateTime.Now;
                    framedata.Frame = str_frame;

                    int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                    double datavalue = 0;

                    //报警
                    ProcAlarm(pack.DevID, TerType.UniversalTer, pack.DevType, pack.C1, pack.Data[1], pack.Data[0], str_frame, DateTime.Now);

                    DataRow[] dr_TerminalDataConfig = null;
                    if (GlobalValue.Instance.UniversalDataConfig != null && GlobalValue.Instance.UniversalDataConfig.Rows.Count > 0)
                    {
                        dr_TerminalDataConfig = GlobalValue.Instance.UniversalDataConfig.Select("TerminalID='" + pack.DevID + "' AND Sequence='" + pack.Data[2] + "'"); //WayType
                    }
                    if (dr_TerminalDataConfig != null && dr_TerminalDataConfig.Length > 0)
                    {
                        int loopdatalen = 6 + 8;  //循环部分数据宽度 = 时间(6)+量程（4byte）+校准值（2byte）+模拟数据（2byte）
                        int dataindex = (pack.DataLength - 2 - 1 - 3) % loopdatalen;  //电压和场强
                        if (dataindex != 0)
                            throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(2+1+" + loopdatalen + "*n+3)规则");

                        //电压和场强
                        volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 2], pack.Data[pack.DataLength - 3] }, 0)) / 1000;
                        field_strength = (Int16)pack.Data[pack.DataLength - 1];

                        dataindex = (pack.DataLength - 2 - 1) / loopdatalen;
                        for (int i = 0; i < dataindex; i++)
                        {
                            year = 2000 + Convert.ToInt16(pack.Data[i * loopdatalen + 3]);
                            month = Convert.ToInt16(pack.Data[i * loopdatalen + 4]);
                            day = Convert.ToInt16(pack.Data[i * loopdatalen + 5]);
                            hour = Convert.ToInt16(pack.Data[i * loopdatalen + 6]);
                            minute = Convert.ToInt16(pack.Data[i * loopdatalen + 7]);
                            sec = Convert.ToInt16(pack.Data[i * loopdatalen + 8]);

                            double range = 0;   //量程
                            range += BitConverter.ToInt16(new byte[] { pack.Data[i * loopdatalen + 10], pack.Data[i * loopdatalen + 9] }, 0);    //整数部分
                            range += ((double)BitConverter.ToInt16(new byte[] { pack.Data[i * loopdatalen + 12], pack.Data[i * loopdatalen + 11] }, 0)) / 1000;    //小数部分
                            short calibration = BitConverter.ToInt16(new byte[] { pack.Data[i * loopdatalen + 14], pack.Data[i * loopdatalen + 13] }, 0);
                            datavalue = BitConverter.ToInt16(new byte[] { pack.Data[i * loopdatalen + 16], pack.Data[i * loopdatalen + 15] }, 0);

                            RectifyResult = RectifyCalc(pack, pack.Data[2], out CalcExpress, range, calibration, datavalue);
                            if (RectifyResult != null)  //运算失败
                            {
                                CalcExpress = "(计算式:" + CalcExpress + ")";
                                datavalue = Convert.ToSingle(RectifyResult);
                            }
                            else
                            {
                                //(模拟数据-校准值)*量程/系数
                                CalcExpress = "";
                                datavalue = ((double)(datavalue - calibration)) * range / (ConstValue.UniversalSimRatio);
                            }
                            GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.UniversalTer, string.Format("index({0})|通用终端[{1}]模拟{2}路|校准值({3})|采集时间({4})|{5}:{6}{7}{8}|电压值:{9}V|信号强度:{10}",
                                i, pack.DevID, pack.Data[2], calibration, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec,
                                dr_TerminalDataConfig[0]["Name"].ToString().Trim(), datavalue, dr_TerminalDataConfig[0]["Unit"].ToString().Trim(), CalcExpress, volvalue, field_strength)));

                            GPRSUniversalDataEntity data = new GPRSUniversalDataEntity();
                            data.DataValue = datavalue;
                            data.Sim1Zero = calibration;
                            data.TypeTableID = Convert.ToInt32(dr_TerminalDataConfig[0]["ID"]);
                            try
                            {
                                data.ColTime = new DateTime(year, month, day, hour, minute, sec);
                            }
                            catch { data.ColTime = ConstValue.MinDateTime; }
                            bNeedCheckTime = GlobalValue.Instance.SocketMag.NeedCheckTime(data.ColTime);
                            framedata.lstData.Add(data);
                        }
                        GlobalValue.Instance.GPRS_UniversalFrameData.Enqueue(framedata);  //通知存储线程处理
                        GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertUniversalValue);
                    }
                    else
                    {
                        GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.UnResolve, "通用终端[" + pack.DevID + "]未配置数据帧解析规则,数据未能解析！"));
                    }
                    #endregion
                }
                else if ((pack.C1 == (byte)GPRS_READ.READ_UNIVERSAL_PLUSE))  //接受通用终端发送的脉冲数据
                {
                    #region 通用终端脉冲
                    GPRSUniversalFrameDataEntity framedata = new GPRSUniversalFrameDataEntity();
                    framedata.TerId = pack.DevID.ToString();
                    framedata.ModifyTime = DateTime.Now;
                    framedata.Frame = str_frame;

                    int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                    double datavalue = 0;

                    //报警
                    ProcAlarm(pack.DevID, TerType.UniversalTer, pack.DevType, pack.C1, pack.Data[1], pack.Data[0], str_frame, DateTime.Now);

                    DataRow[] dr_TerminalDataConfig = null;
                    if (GlobalValue.Instance.UniversalDataConfig != null && GlobalValue.Instance.UniversalDataConfig.Rows.Count > 0)
                    {
                        dr_TerminalDataConfig = GlobalValue.Instance.UniversalDataConfig.Select("TerminalID='" + pack.DevID + "' AND Sequence IN ('4','5','6','7')", "Sequence"); //WayType
                    }
                    if (dr_TerminalDataConfig != null && dr_TerminalDataConfig.Length > 0)
                    {
                        int waycount = dr_TerminalDataConfig.Length;
                        float PluseUnit = 1;       //脉冲计数单位 脉冲单位0代表0.01、1代表0.1、2代表0.2    3代表0.5、4代表1、5代表10、6代表100
                        string[] Names = new string[waycount];
                        string[] Units = new string[waycount];
                        int[] config_ids = new int[waycount];

                        for (int i = 0; i < waycount; i++)
                        {
                            Names[i] = dr_TerminalDataConfig[i]["Name"] != DBNull.Value ? dr_TerminalDataConfig[i]["Name"].ToString().Trim() : "";
                            Units[i] = dr_TerminalDataConfig[i]["Unit"] != DBNull.Value ? dr_TerminalDataConfig[i]["Unit"].ToString().Trim() : "";
                            config_ids[i] = dr_TerminalDataConfig[i]["ID"] != DBNull.Value ? Convert.ToInt32(dr_TerminalDataConfig[i]["ID"]) : 0;
                        }

                        int loopdatalen = 6 + 1 + 4 * 4;  //循环部分数据宽度 = 时间(6)+脉冲计数单位+固定4路*4(每路长度)
                        int dataindex = (pack.DataLength - 2 - 1 - 3) % loopdatalen;
                        if (dataindex != 0)
                            throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(2+1+" + loopdatalen + "*n+3)规则");
                        //电压和场强
                        volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 2], pack.Data[pack.DataLength - 3] }, 0)) / 1000;
                        field_strength = (Int16)pack.Data[pack.DataLength - 1];

                        dataindex = (pack.DataLength - 2 - 1) / loopdatalen;
                        for (int i = 0; i < dataindex; i++)
                        {
                            year = 2000 + Convert.ToInt16(pack.Data[i * loopdatalen + 3]);
                            month = Convert.ToInt16(pack.Data[i * loopdatalen + 4]);
                            day = Convert.ToInt16(pack.Data[i * loopdatalen + 5]);
                            hour = Convert.ToInt16(pack.Data[i * loopdatalen + 6]);
                            minute = Convert.ToInt16(pack.Data[i * loopdatalen + 7]);
                            sec = Convert.ToInt16(pack.Data[i * loopdatalen + 8]);
                            
                            int freindex = 0;
                            for (int j = 0; j < waycount; j++)
                            {
                                datavalue = BitConverter.ToInt32(new byte[] { pack.Data[i * loopdatalen + 13 + freindex], pack.Data[i * loopdatalen + 12 + freindex], pack.Data[i * loopdatalen + 11 + freindex], pack.Data[i * loopdatalen + 10 + freindex] }, 0);
                                freindex += 4;
                                
                                RectifyResult = RectifyCalc(pack, pack.Data[2] - 1, out CalcExpress, pack.Data[i * loopdatalen + 9], datavalue);
                                if (RectifyResult != null)  //运算失败
                                {
                                    CalcExpress = "(计算式:" + CalcExpress + ")";
                                    datavalue = Convert.ToSingle(RectifyResult);
                                }
                                else
                                {
                                    CalcExpress = "";
                                    switch (pack.Data[i * loopdatalen + 9])       //脉冲单位0代表0.01、1代表0.1、2代表0.2    3代表0.5、4代表1、5代表10、6代表100
                                    {
                                        case 0:
                                            PluseUnit = 0.01f;
                                            break;
                                        case 1:
                                            PluseUnit = 0.1f;
                                            break;
                                        case 2:
                                            PluseUnit = 0.2f;
                                            break;
                                        case 3:
                                            PluseUnit = 0.5f;
                                            break;
                                        case 4:
                                            PluseUnit = 1f;
                                            break;
                                        case 5:
                                            PluseUnit = 10f;
                                            break;
                                        case 6:
                                            PluseUnit = 100f;
                                            break;
                                    }
                                    datavalue = PluseUnit * datavalue;  //脉冲计数*单位脉冲值
                                }
                                GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.UniversalTer, string.Format("index({0})|通用终端[{1}]脉冲{2}路|采集时间({3})|{4}:{5}{6}{7}|电压值:{8}V|信号强度:{9}",
                                    i, pack.DevID, j + 1, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, Names[j], datavalue, Units[j], CalcExpress, volvalue, field_strength)));

                                GPRSUniversalDataEntity data = new GPRSUniversalDataEntity();
                                data.DataValue = datavalue;
                                data.TypeTableID = Convert.ToInt32(config_ids[j]);
                                try
                                {
                                    data.ColTime = new DateTime(year, month, day, hour, minute, sec);
                                }
                                catch { data.ColTime = ConstValue.MinDateTime; }
                                bNeedCheckTime = GlobalValue.Instance.SocketMag.NeedCheckTime(data.ColTime);
                                framedata.lstData.Add(data);
                            }
                        }
                        GlobalValue.Instance.GPRS_UniversalFrameData.Enqueue(framedata);  //通知存储线程处理
                        GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertUniversalValue);
                    }
                    else
                    {
                        GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.UnResolve, "通用终端[" + pack.DevID + "]未配置数据帧解析规则,数据未能解析！"));
                    }
                    #endregion
                }
                else if (pack.C1 == (byte)GPRS_READ.READ_UNVERSAL_RS485)  //接受通用终端发送的RS485 数据
                {
                    #region 通用终端RS485 数据
                    /*N=报警标志(2byte)+ 流量标志(1byte)+(长度(1byte)+时间1(6byte) + 采集的未解析流量数据(nbyte))*m = 2 + 1 + (1+6+n)*m，
                    n为流量数据字节数。当流量标识为 >= 2时，为RS485 / 流量，16进制数，2 - 9代表485的路数1 - 8；当流量标识为1时，为脉冲流量，数据为32位浮点数
                    */
                    GPRSUniversalFrameDataEntity framedata = new GPRSUniversalFrameDataEntity();
                    framedata.TerId = pack.DevID.ToString();
                    framedata.ModifyTime = DateTime.Now;
                    framedata.Frame = str_frame;

                    int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                    double datavalue = 0;

                    //报警
                    ProcAlarm(pack.DevID, TerType.UniversalTer, pack.DevType, pack.C1, pack.Data[1], pack.Data[0], str_frame, DateTime.Now);

                    //电压和场强
                    volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 2], pack.Data[pack.DataLength - 3] }, 0)) / 1000;
                    field_strength = (Int16)pack.Data[pack.DataLength - 1];

                    DataRow[] dr_TerminalDataConfig = null;
                    DataRow[] dr_DataConfig_Child = null;

                    if (GlobalValue.Instance.UniversalDataConfig != null && GlobalValue.Instance.UniversalDataConfig.Rows.Count > 0)
                    {
                        dr_TerminalDataConfig = GlobalValue.Instance.UniversalDataConfig.Select("TerminalID='" + pack.DevID + "' AND Sequence='" + (pack.Data[2] - 1) + "'"); //WayType
                        if (dr_TerminalDataConfig != null && dr_TerminalDataConfig.Length > 0)
                        {
                            dr_DataConfig_Child = GlobalValue.Instance.UniversalDataConfig.Select("ParentID='" + dr_TerminalDataConfig[0]["ID"].ToString().Trim() + "'", "Sequence");
                            if (dr_DataConfig_Child != null && dr_DataConfig_Child.Length > 0)
                            {
                                dr_TerminalDataConfig[0] = dr_DataConfig_Child[0];  //有子节点配置时，使用子节点配置
                            }
                        }
                    }
                    if (dr_TerminalDataConfig != null && dr_TerminalDataConfig.Length > 0)
                    {
                        string Names = "";
                        string Units = "";
                        int config_ids = -1;
                        Names = dr_TerminalDataConfig[0]["Name"] != DBNull.Value ? dr_TerminalDataConfig[0]["Name"].ToString().Trim() : "";
                        Units = dr_TerminalDataConfig[0]["Unit"] != DBNull.Value ? dr_TerminalDataConfig[0]["Unit"].ToString().Trim() : "";
                        config_ids = dr_TerminalDataConfig[0]["ID"] != DBNull.Value ? Convert.ToInt32(dr_TerminalDataConfig[0]["ID"]) : 0;

                        int partindex = 3;
                        int partlen = pack.Data[3]; //部分数据长度
                        do
                        {
                            year = 2000 + Convert.ToInt16(pack.Data[partindex]);
                            month = Convert.ToInt16(pack.Data[partindex + 1]);
                            day = Convert.ToInt16(pack.Data[partindex + 2]);
                            hour = Convert.ToInt16(pack.Data[partindex + 3]);
                            minute = Convert.ToInt16(pack.Data[partindex + 4]);
                            sec = Convert.ToInt16(pack.Data[partindex + 5]);

                            if (partlen == 2)
                                datavalue = BitConverter.ToInt16(new byte[] { pack.Data[partindex + 7], pack.Data[partindex + 6] }, 0);
                            else if (partlen == 4)
                                datavalue = BitConverter.ToInt32(new byte[] { pack.Data[partindex + 9], pack.Data[partindex + 8], pack.Data[partindex + 7], pack.Data[partindex + 6] }, 0);

                            RectifyResult = RectifyCalc(pack, pack.Data[2] - 1, out CalcExpress, datavalue.ToString());
                            if (RectifyResult != null)  //运算失败
                            {
                                CalcExpress = "(计算式:" + CalcExpress + ")";
                                datavalue = Convert.ToSingle(RectifyResult);
                            }
                            else
                                CalcExpress = "";
                            
                            GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.UniversalTer, string.Format("通用终端[{0}]RS485 {1}路|采集时间({2})|{3}:{4}{5}{6}|电压值:{7}V|信号强度:{8}",
                                        pack.DevID, (pack.Data[2] - 1), year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, Names, datavalue, Units, CalcExpress, volvalue, field_strength)));

                            GPRSUniversalDataEntity data = new GPRSUniversalDataEntity();
                            data.DataValue = datavalue;
                            data.TypeTableID = Convert.ToInt32(config_ids);
                            try
                            {
                                data.ColTime = new DateTime(year, month, day, hour, minute, sec);
                            }
                            catch { data.ColTime = ConstValue.MinDateTime; }
                            bNeedCheckTime = GlobalValue.Instance.SocketMag.NeedCheckTime(data.ColTime);
                            framedata.lstData.Add(data);

                            partindex += 1 + 6 + partlen;
                            partlen = pack.Data[partindex];
                        } while ((partindex + 1 + 6 + partlen) <= pack.DataLength);
                        GlobalValue.Instance.GPRS_UniversalFrameData.Enqueue(framedata);  //通知存储线程处理
                        GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertUniversalValue);

                    }
                    else
                    {
                        GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.UnResolve, "通用终端[" + pack.DevID + "]未配置数据帧解析规则,数据未能解析！"));
                    }
                    #endregion
                }
                #endregion

            }
            else if (pack.ID3 == (byte)Entity.ConstValue.DEV_TYPE.PRESS_CTRL)
            {
                #region 压力控制器
                if (pack.C1 == (byte)PRECTRL_COMMAND.READ_DATA)  //从站向主站发送采集数据
                {
                    bool addtion_strength = false;
                    int dataindex = (pack.DataLength) % 24;
                    if (dataindex != 0)
                    {
                        if (dataindex == 3)
                        {
                            addtion_strength = true;
                            dataindex = (pack.DataLength - 3) / 24;
                        }
                        else
                            throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(24*n/24*n+3)规则");  //GPRS远程压力终端在数据段最后增加两个字节的电压数据
                    }
                    else
                        dataindex = (pack.DataLength) / 24;

                    string parmalarm = "", alarm = "";

                    //报警
                    /*
                     参数报警标识说明：相应的位为“1”，则表示有相应报警，如下所示，
                    A0-进口压力上限报警
                    A1-进口压力下限报警
                    A2-压力调整超时报警
                    A3～A7-备用
                      设备报警标识说明：相应的位为“1”，则表示有相应报警，如下所示，
                    A0-电池报警
                    A1-进口压力传感器报警
                    A2-出口压力传感器报警
                    A3-流量器报警
                    A4～A7-备用
                     */

                    GPRSPrectrlFrameDataEntity framedata = new GPRSPrectrlFrameDataEntity();
                    framedata.TerId = pack.DevID.ToString();
                    framedata.ModifyTime = DateTime.Now;
                    framedata.Frame = str_frame;

                    if (addtion_strength)
                    {
                        volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 2], pack.Data[pack.DataLength - 3] }, 0)) / 1000;
                        field_strength = (Int16)pack.Data[pack.DataLength - 1];
                    }

                    for (int i = 0; i < dataindex; i++)
                    {
                        parmalarm = "";
                        alarm = "";
                        if ((pack.Data[i * 24 + 22] & 0x01) == 1)  //进口压力上限报警
                            parmalarm += "进口压力上限报警";
                        else if (((pack.Data[i * 24 + 22] & 0x02) >> 1) == 1)   //进口压力下限报警
                            parmalarm += "进口压力下限报警";
                        else if (((pack.Data[i * 24 + 22] & 0x04) >> 2) == 1)   //压力调整超时报警
                            parmalarm += "压力调整超时报警";

                        if ((pack.Data[i * 24 + 23] & 0x01) == 1)  //电池报警
                            alarm += "电池报警";
                        else if (((pack.Data[i * 24 + 23] & 0x02) >> 1) == 1)   //进口压力传感器报警
                            alarm += "进口压力传感器报警";
                        else if (((pack.Data[i * 24 + 23] & 0x04) >> 2) == 1)  //出口压力传感器报警
                            alarm += "出口压力传感器报警";
                        else if (((pack.Data[i * 24 + 23] & 0x08) >> 3) == 1)  //流量器报警
                            alarm += "流量器报警";

                        //发送时间(6bytes)+进口压力(2bytes)+出口压力(2bytes)+正向累积流量 (4bytes)+反向累积流量(4bytes)+瞬时流量(4bytes)+参数报警标识(1byte)+设备报警标识(1byte)
                        int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                        year = 2000 + Convert.ToInt16(pack.Data[i * 24]);
                        month = Convert.ToInt16(pack.Data[i * 24 + 1]);
                        day = Convert.ToInt16(pack.Data[i * 24 + 2]);
                        hour = Convert.ToInt16(pack.Data[i * 24 + 3]);
                        minute = Convert.ToInt16(pack.Data[i * 24 + 4]);
                        sec = Convert.ToInt16(pack.Data[i * 24 + 5]);

                        //进口压力
                        float entrance_prevalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[i * 24 + 7], pack.Data[i * 24 + 6] }, 0)) / 1000;
                        //出口压力
                        float outlet_prevalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[i * 24 + 9], pack.Data[i * 24 + 8] }, 0)) / 1000;
                        //前向流量
                        float forward_flowvalue = BitConverter.ToSingle(new byte[] { pack.Data[i * 24 + 13], pack.Data[i * 24 + 12], pack.Data[i * 24 + 11], pack.Data[i * 24 + 10] }, 0);
                        //反向流量
                        float reverse_flowvalue = BitConverter.ToSingle(new byte[] { pack.Data[i * 24 + 17], pack.Data[i * 24 + 16], pack.Data[i * 24 + 15], pack.Data[i * 24 + 14] }, 0);
                        //瞬时流量
                        float instant_flowvalue = BitConverter.ToSingle(new byte[] { pack.Data[i * 24 + 21], pack.Data[i * 24 + 20], pack.Data[i * 24 + 19], pack.Data[i * 24 + 18] }, 0);

                        if (!string.IsNullOrEmpty(parmalarm) || !string.IsNullOrEmpty(alarm))
                            GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.Alarm, string.Format("index({0})|压力控制器[{1}]|参数报警({2})|设备报警({3})|采集时间({4})|进口压力:{5}MPa|出口压力:{6}MPa|正向流量值:{7}|反向流量值:{8}|瞬时流量值:{9}|电压值:{10}V|信号强度:{11}",
                                    i, pack.DevID, parmalarm, alarm, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, entrance_prevalue, outlet_prevalue, forward_flowvalue, reverse_flowvalue, instant_flowvalue, volvalue, field_strength)));
                        else
                            GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.PreCTL, string.Format("index({0})|压力控制器[{1}]|参数报警({2})|设备报警({3})|采集时间({4})|进口压力:{5}MPa|出口压力:{6}MPa|正向流量值:{7}|反向流量值:{8}|瞬时流量值:{9}|电压值:{10}V|信号强度:{11}",
                                    i, pack.DevID, parmalarm, alarm, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, entrance_prevalue, outlet_prevalue, forward_flowvalue, reverse_flowvalue, instant_flowvalue, volvalue, field_strength)));

                        GPRSPrectrlDataEntity data = new GPRSPrectrlDataEntity();
                        data.Entrance_preValue = entrance_prevalue;
                        data.Outlet_preValue = outlet_prevalue;
                        data.Forward_FlowValue = forward_flowvalue;
                        data.Reverse_FlowValue = reverse_flowvalue;
                        data.Instant_FlowValue = instant_flowvalue;
                        byte balarm = (byte)((pack.Data[i * 24 + 22]) << 4);  //balarm存放pack.Data[22]的低四位到高四位位置,存放pack.Data[23]的低四位到低四位位置
                        balarm |= 0x0f;
                        balarm = (byte)(balarm & (pack.Data[i * 24 + 23] | 0xF0));

                        data.AlarmCode = balarm;
                        data.AlarmDesc = parmalarm;
                        if (!string.IsNullOrEmpty(data.AlarmDesc))
                            data.AlarmDesc += ",";
                        data.AlarmDesc += alarm;   //存放两个报警信息
                        data.Voltage = volvalue;
                        data.FieldStrength = field_strength;
                        try
                        {
                            data.ColTime = new DateTime(year, month, day, hour, minute, sec);
                        }
                        catch { data.ColTime = ConstValue.MinDateTime; }
                        bNeedCheckTime = GlobalValue.Instance.SocketMag.NeedCheckTime(data.ColTime);
                        framedata.lstPrectrlData.Add(data);
                    }

                    GlobalValue.Instance.GPRS_PrectrlFrameData.Enqueue(framedata);  //通知存储线程处理
                    GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertPrectrlValue);
                }
                #endregion
            }
            else if (pack.ID3 == (byte)Entity.ConstValue.DEV_TYPE.OLWQ_CTRL)
            {
                #region 水质终端
                bool addtion_voldata = false;   //是否在数据段最后增加了两个字节的电压数据
                if ((pack.C1 == (byte)GPRS_READ.READ_TURBIDITY) || (pack.C1 == (byte)GPRS_READ.READ_RESIDUALCL) ||
                    (pack.C1 == (byte)GPRS_READ.READ_PH) || (pack.C1 == (byte)GPRS_READ.READ_TEMP))  //从站向主站发送水质采集数据
                {
                    int dataindex = (pack.DataLength - 2 - 1) % 8;
                    if (dataindex != 0)
                    {
                        if (dataindex == 2)
                        {
                            dataindex = (pack.DataLength - 2 - 1 - 2) / 8;
                            addtion_voldata = true;
                        }
                        else
                        {
                            throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(2+1+8*n)或(2+1+8*n+2)规则");  //最后增加两个字节的电压数据
                        }
                    }
                    else
                        dataindex = (pack.DataLength - 2 - 1) / 8;

                    GPRSOLWQFrameDataEntity framedata = new GPRSOLWQFrameDataEntity();
                    framedata.TerId = pack.ID.ToString();
                    framedata.ModifyTime = DateTime.Now;
                    framedata.Frame = str_frame;

                    int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                    float value = 0;
                    string name = "";
                    string unit = "";
                    string valuecolumnname = "";
                    if (pack.C1 == (byte)GPRS_READ.READ_TURBIDITY)
                    {
                        name = "浊度";
                        unit = "NTU";
                        valuecolumnname = "Turbidity";
                    }
                    else if (pack.C1 == (byte)GPRS_READ.READ_RESIDUALCL)
                    {
                        name = "余氯";
                        unit = "PPM";
                        valuecolumnname = "ResidualCl";
                    }
                    else if (pack.C1 == (byte)GPRS_READ.READ_PH)
                    {
                        name = "PH";
                        unit = "ph";
                        valuecolumnname = "PH";
                    }
                    else if (pack.C1 == (byte)GPRS_READ.READ_TEMP)
                    {
                        name = "温度";
                        unit = "℃";
                        valuecolumnname = "Temperature";
                    }

                    if (addtion_voldata)  //电压
                        volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 1], pack.Data[pack.DataLength - 2] }, 0)) / 1000;

                    for (int i = 0; i < dataindex; i++)
                    {
                        year = 2000 + Convert.ToInt16(pack.Data[i * 8 + 3]);
                        month = Convert.ToInt16(pack.Data[i * 8 + 4]);
                        day = Convert.ToInt16(pack.Data[i * 8 + 5]);
                        hour = Convert.ToInt16(pack.Data[i * 8 + 6]);
                        minute = Convert.ToInt16(pack.Data[i * 8 + 7]);
                        sec = Convert.ToInt16(pack.Data[i * 8 + 8]);

                        value = (float)BitConverter.ToInt16(new byte[] { pack.Data[i * 8 + 10], pack.Data[i * 8 + 9] }, 0);
                        GPRSOLWQDataEntity data = new GPRSOLWQDataEntity();
                        data.ValueColumnName = valuecolumnname;
                        if (pack.C1 == (byte)GPRS_READ.READ_TURBIDITY)
                        {
                            value = value / 100;
                            data.Turbidity = value;
                        }
                        else if (pack.C1 == (byte)GPRS_READ.READ_RESIDUALCL)
                        {
                            data.ResidualCl = value / 1000;
                        }
                        else if (pack.C1 == (byte)GPRS_READ.READ_PH)
                        {
                            data.PH = value / 100;
                        }
                        else if (pack.C1 == (byte)GPRS_READ.READ_TEMP)
                        {
                            data.Temperature = value;
                        }

                        GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.OLWQ, string.Format("index({0})|水质终端[{1}]|采集时间({2})|{3}值:{4}{5}|电压值:{6}V",
                            dataindex, pack.ID, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, name, value, unit, volvalue)));
                        data.Voltage = volvalue;
                        try
                        {
                            data.ColTime = new DateTime(year, month, day, hour, minute, sec);
                        }
                        catch { data.ColTime = ConstValue.MinDateTime; }
                        bNeedCheckTime = GlobalValue.Instance.SocketMag.NeedCheckTime(data.ColTime);
                        framedata.lstOLWQData.Add(data);
                    }

                    GlobalValue.Instance.GPRS_OLWQFrameData.Enqueue(framedata);  //通知存储线程处理
                    GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertOLWQValue);
                }
                else if (pack.C1 == (byte)GPRS_READ.READ_CONDUCTIVITY)  //从站向主站发送电导率采集数据
                {
                    int dataindex = (pack.DataLength - 2 - 1) % 12;
                    if (dataindex != 0)
                    {
                        if (dataindex == 2)
                        {
                            dataindex = (pack.DataLength - 2 - 1 - 2) / 12;
                            addtion_voldata = true;
                        }
                        else
                        {
                            throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(2+1+10*n)或(2+1+10*n+2)规则");  //最后增加两个字节的电压数据
                        }
                    }
                    else
                        dataindex = (pack.DataLength - 2 - 1) / 12;

                    GPRSOLWQFrameDataEntity framedata = new GPRSOLWQFrameDataEntity();
                    framedata.TerId = pack.ID.ToString();
                    framedata.ModifyTime = DateTime.Now;
                    framedata.Frame = str_frame;

                    int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                    float Condvalue = 0;
                    float Tempvalue = 0;

                    if (addtion_voldata)  //电压
                        volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 1], pack.Data[pack.DataLength - 2] }, 0)) / 1000;

                    for (int i = 0; i < dataindex; i++)
                    {
                        year = 2000 + Convert.ToInt16(pack.Data[i * 12 + 3]);
                        month = Convert.ToInt16(pack.Data[i * 12 + 4]);
                        day = Convert.ToInt16(pack.Data[i * 12 + 5]);
                        hour = Convert.ToInt16(pack.Data[i * 12 + 6]);
                        minute = Convert.ToInt16(pack.Data[i * 12 + 7]);
                        sec = Convert.ToInt16(pack.Data[i * 12 + 8]);

                        Condvalue = ((float)BitConverter.ToInt32(new byte[] { pack.Data[i * 12 + 12], pack.Data[i * 12 + 11], pack.Data[i * 12 + 10], pack.Data[i * 12 + 9] }, 0)) / 100;
                        Tempvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[i * 12 + 14], pack.Data[i * 12 + 13] }, 0)) / 10;

                        GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.OLWQ, string.Format("index({0})|水质终端[{1}]|采集时间({2})|电导率值:{3}us/cm,温度:{4}℃|电压值:{5}V",
                            i, pack.ID, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, Condvalue.ToString("f2"), Tempvalue.ToString("f1"), volvalue)));

                        GPRSOLWQDataEntity data = new GPRSOLWQDataEntity();
                        data.Conductivity = Condvalue;
                        data.Temperature = Tempvalue;
                        data.ValueColumnName = "Conductivity";
                        data.Voltage = volvalue;
                        try
                        {
                            data.ColTime = new DateTime(year, month, day, hour, minute, sec);
                        }
                        catch { data.ColTime = ConstValue.MinDateTime; }
                        bNeedCheckTime = GlobalValue.Instance.SocketMag.NeedCheckTime(data.ColTime);
                        framedata.lstOLWQData.Add(data);
                    }

                    GlobalValue.Instance.GPRS_OLWQFrameData.Enqueue(framedata);  //通知存储线程处理
                    GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertOLWQValue);
                }
                else if (pack.C1 == (byte)GPRS_READ.READ_OLWQFLOW) //从站向主站发送流量采集数据(水质终端)
                {
                    #region 上海肯特(KENT)水表
                    if (pack.Data[2] == 0x02)   //上海肯特(KENT)水表
                    {
                        int dataindex = (pack.DataLength - 2 - 2 - 1) % (6 + 36);  //两字节报警,1字节厂家类型,
                        if (dataindex != 0)
                        {
                            throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(2+1+(6+36)*n)规则");
                        }
                        dataindex = (pack.DataLength - 2 - 2 - 1) / (6 + 36);

                        GPRSFlowFrameDataEntity framedata = new KERTFlow().ProcessData(dataindex, TerType.OLWQTer, "水质终端", pack.ID.ToString(), str_frame, pack.Data, pack.DataLength, out bNeedCheckTime);

                        GlobalValue.Instance.GPRS_FlowFrameData.Enqueue(framedata);  //通知存储线程处理
                        GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertFlowValue);
                    }
                    else
                    {
                        GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.UnResolve, "水质终端[" + pack.ID + "]错误未知水表类型!"));
                    }
                    #endregion
                }
                else if (pack.C1 == (byte)GPRS_READ.READ_OLWQALARM)  //从站向主站发送设备报警信息(水质终端)
                {
                    if (pack.DataLength != 7 && pack.DataLength != 9)   //pack.DataLength == 9 带电压值
                    {
                        throw new ArgumentException(DateTime.Now.ToString() + " " + "帧数据长度[" + pack.DataLength + "]不符合(2+1+18*n)或(2+1+18*n+2)规则");
                    }

                    string alarm = "";
                    //报警
                    /*
                     * A0—电池低压报警。
                     * A1—浊度报警。
                     * A2—余氯报警。
                     * A3—流量报警。
                     * A4—PH报警。
                     * A5—电导率报警。
                     * A6～A7—备用
                     */

                    if ((pack.Data[0] & 0x01) == 1)  //电池低压报警
                        alarm += "电池低压报警";
                    else if (((pack.Data[0] & 0x02) >> 1) == 1)   //浊度报警
                        alarm += "浊度报警";
                    else if (((pack.Data[0] & 0x04) >> 2) == 1)   //余氯报警
                        alarm += "余氯报警";
                    else if (((pack.Data[0] & 0x08) >> 3) == 1)  //流量报警
                        alarm += "流量报警";
                    else if (((pack.Data[0] & 0x10) >> 4) == 1)  //PH报警
                        alarm += "PH报警";
                    else if (((pack.Data[0] & 0x20) >> 5) == 1)  //电导率报警
                        alarm += "电导率报警";

                    int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                    year = 2000 + Convert.ToInt16(pack.Data[1]);
                    month = Convert.ToInt16(pack.Data[2]);
                    day = Convert.ToInt16(pack.Data[3]);
                    hour = Convert.ToInt16(pack.Data[4]);
                    minute = Convert.ToInt16(pack.Data[5]);
                    sec = Convert.ToInt16(pack.Data[6]);

                    if (pack.DataLength == 9)   //pack.DataLength == 9 带电压值
                    {
                        volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 1], pack.Data[pack.DataLength - 2] }, 0)) / 1000;
                    }
                    if (month == 0)
                        month = 1;
                    if (day == 0)
                        day = 1;
                    bNeedCheckTime = GlobalValue.Instance.SocketMag.NeedCheckTime(new DateTime(year, month, day, hour, minute, sec));
                    if (!string.IsNullOrEmpty(alarm))
                        GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.Alarm, string.Format("水质终端[{0}]{1}|时间({2})|电压值:{3}V",
                             pack.ID, alarm, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, volvalue)));
                    else
                        GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.OLWQ, string.Format("水质终端[{0}]{1}|时间({2})|电压值:{3}V",
                             pack.ID, alarm, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, volvalue)));
                }
                #endregion
            }
            else if (pack.ID3 == (byte)Entity.ConstValue.DEV_TYPE.HYDRANT_CTRL)
            {
                #region 消防栓
                GPRSHydrantFrameDataEntity framedata = new GPRSHydrantFrameDataEntity();
                framedata.TerId = pack.DevID.ToString();
                framedata.ModifyTime = DateTime.Now;
                framedata.Frame = str_frame;

                int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
                year = 2000 + Convert.ToInt16(pack.Data[0]);
                month = Convert.ToInt16(pack.Data[1]);
                day = Convert.ToInt16(pack.Data[2]);
                hour = Convert.ToInt16(pack.Data[3]);
                minute = Convert.ToInt16(pack.Data[4]);
                sec = Convert.ToInt16(pack.Data[5]);

                GPRSHydrantDataEntity data = new GPRSHydrantDataEntity();
                try
                {
                    data.ColTime = new DateTime(year, month, day, hour, minute, sec);
                }
                catch { data.ColTime = ConstValue.MinDateTime; }
                bNeedCheckTime = GlobalValue.Instance.SocketMag.NeedCheckTime(data.ColTime);

                if (pack.C1 == (byte)GPRS_READ.READ_HYDRANT_OPEN)
                {
                    int openangle = Convert.ToInt16(pack.Data[6]);
                    float prevalue = (float)BitConverter.ToInt16(new byte[] { pack.Data[8], pack.Data[7] }, 0);
                    RectifyResult = RectifyCalc(pack,out CalcExpress, prevalue.ToString());
                    if (RectifyResult != null)  //运算失败
                    {
                        CalcExpress = "(计算式:" + CalcExpress + ")";
                        prevalue = Convert.ToSingle(RectifyResult);
                    }
                    else {
                        CalcExpress = "";
                        prevalue = prevalue / 1000;
                    }
                    GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.Hydrant, string.Format("消防栓[{0}]被打开|时间({1})|开度:{2},压力:{3}MPa{4}",
                            pack.DevID, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, openangle, prevalue.ToString("f3"), CalcExpress)));
                    data.Operate = HydrantOptType.Open;
                    data.PreValue = prevalue;
                    data.OpenAngle = openangle;
                }
                else if (pack.C1 == (byte)GPRS_READ.READ_HYDRANT_CLOSE)
                {
                    GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.Hydrant, string.Format("消防栓[{0}]被关闭|时间({1})",
                               pack.DevID, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec)));
                    data.Operate = HydrantOptType.Close;
                }
                else if (pack.C1 == (byte)GPRS_READ.READ_HYDRANT_OPENANGLE)
                {
                    int openangle = Convert.ToInt16(pack.Data[6]);
                    GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.Hydrant, string.Format("消防栓[{0}]开度|时间({1})|开度:{2}",
                            pack.DevID, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, openangle)));
                    data.OpenAngle = openangle;
                    data.Operate = HydrantOptType.OpenAngle;
                }
                else if (pack.C1 == (byte)GPRS_READ.READ_HYDRANT_IMPACT)
                {
                    GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.Hydrant, string.Format("消防栓[{0}]被撞击|时间({1})",
                               pack.DevID, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec)));
                    data.Operate = HydrantOptType.Impact;
                }
                else if (pack.C1 == (byte)GPRS_READ.READ_HYDRANT_KNOCKOVER)
                {
                    GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.Hydrant, string.Format("消防栓[{0}]被撞倒|时间({1})",
                               pack.DevID, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec)));
                    data.Operate = HydrantOptType.KnockOver;
                }
                else if (pack.C1 == (byte)GPRS_READ.READ_HYDRANT_TIMEGPRS)       //定时远传
                {
                    //年、月、日、时、分、秒、开启/关闭、开度、被撞、撞倒、压力高位、压力低位
                    string strstate = "", stropenangle = "-", strprevalue = "未配置";
                    if (pack.Data[6] == 0x00)
                    {
                        data.Operate = HydrantOptType.Close;
                        strstate = "关闭";
                    }
                    if (pack.Data[6] == 0x01)
                    {
                        data.Operate = HydrantOptType.Open;
                        int openangle = Convert.ToInt16(pack.Data[7]);
                        data.OpenAngle = openangle;
                        strstate = "打开";
                        stropenangle = openangle.ToString();
                    }
                    if (pack.Data[8] == 0x01)
                    {
                        data.Operate = HydrantOptType.Impact;
                        strstate = "撞击";
                    }
                    if (pack.Data[9] == 0x01)
                    {
                        data.Operate = HydrantOptType.KnockOver;
                        strstate = "撞倒";
                    }
                    if (pack.Data[10] == 0x01)       //压力标志,0x01:表示有配置压力
                    {
                        float prevalue = (float)BitConverter.ToInt16(new byte[] { pack.Data[12], pack.Data[11] }, 0) / 1000;
                        data.PreValue = prevalue;
                        strprevalue = prevalue.ToString() + "MPa";
                    }
                    GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.Hydrant, string.Format("消防栓[{0}]定时报|时间({1})|状态:{2}|开度:{3}|压力:{4}",
                            pack.DevID, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, strstate, stropenangle, strprevalue)));
                }

                framedata.lstHydrantData.Add(data);
                GlobalValue.Instance.GPRS_HydrantFrameData.Enqueue(framedata);  //通知存储线程处理
                GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertHydrantValue);
                #endregion
            }
            else if (pack.ID3 == (byte)Entity.ConstValue.DEV_TYPE.NOISE_CTRL)
            {
                #region 噪声数据远传控制器
                if (pack.C1 == (byte)GPRS_READ.READ_NOISEDATA)  //从站向主站发送噪声采集数据
                {
                    int dataindex = (pack.DataLength - 3) % 2;
                    if (dataindex != 0)
                        throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合(2*n+3)规则");  //GPRS远程压力终端在数据段最后增加两个字节的电压数据
                    else
                        dataindex = (pack.DataLength - 3) / 2;
                    GPRSNoiseFrameDataEntity framedata = new GPRSNoiseFrameDataEntity();
                    framedata.TerId = pack.ID.ToString();
                    framedata.ModifyTime = DateTime.Now;
                    framedata.Frame = str_frame;

                    GPRSHydrantDataEntity data = new GPRSHydrantDataEntity();
                    bNeedCheckTime = false;
                    volvalue = ((float)BitConverter.ToInt16(new byte[] { pack.Data[pack.DataLength - 2], pack.Data[pack.DataLength - 3] }, 0)) / 1000;
                    field_strength = (Int16)pack.Data[pack.DataLength - 1];

                    //记录仪ID（4byte）＋启动值（2byte）＋总帧数（1byte）＋帧号（1byte）＋ 数据（128byte）＋ 电压（2byte）
                    int logId = BitConverter.ToInt32(new byte[] { pack.Data[3], pack.Data[2], pack.Data[1], 0x00 }, 0);  //记录仪ID
                    int standvalue = BitConverter.ToInt16(new byte[] { pack.Data[5], pack.Data[4] }, 0);      //启动值
                    int sumpackcount = Convert.ToInt32(pack.Data[6]);
                    int curpackindex = Convert.ToInt32(pack.Data[7]);

                    if (curpackindex == 1)
                        state.lstBuffer.Clear();   //收到第一包清空缓存
                    bool needprocess = true;  //是否处理当前包
                    if (curpackindex > 1 && (state.NoisePackIndex == curpackindex)) //如果当前包和上一包序号一样则不处理
                    {
                        needprocess = false;
                    }
                    if (needprocess)
                    {
                        state.NoisePackIndex = curpackindex;   //记录当前收到包的序号
                        if (sumpackcount != curpackindex && !pack.IsFinal)
                        {
                            for (int i = 8; i < pack.DataLength - 3; i++)  //多包时，当前不是最后一包时缓存数据至state.lstBuffer中
                            {
                                state.lstBuffer.Add(pack.Data[i]);
                            }
                        }
                        else
                        {
                            List<byte> lstbytes = new List<byte>();
                            if (curpackindex > 1)  //多包时获取缓存的数据拼接成完整的数据
                            {
                                lstbytes.AddRange(state.lstBuffer);
                                state.lstBuffer.Clear();
                            }
                            for (int i = 8; i < pack.DataLength - 3; i++)  //添加当前包数据
                            {
                                lstbytes.Add(pack.Data[i]);
                            }
                            UpLoadNoiseDataEntity noisedataentity = new UpLoadNoiseDataEntity();
                            noisedataentity.TerId = logId.ToString();
                            noisedataentity.GroupId = "";
                            //启动值
                            noisedataentity.cali = standvalue;
                            noisedataentity.ColTime = DateTime.Now.ToString();
                            for (int i = 0; i + 1 < lstbytes.Count; i += 2)
                            {
                                noisedataentity.Data += BitConverter.ToInt16(new byte[] { lstbytes[i + 1], lstbytes[i] }, 0) + ",";
                            }
                            if (noisedataentity.Data.EndsWith(","))
                                noisedataentity.Data = noisedataentity.Data.Substring(0, noisedataentity.Data.Length - 1);
                            framedata.NoiseData = noisedataentity;

                            bNeedCheckTime = true;  //每天传一次,一天校时一次,不适用NeedCheckTime方法校时
                        }
                        string strcurnoisedata = "";  //当前包的数据,用于显示
                        for (int i = 8; i + 1 < pack.DataLength - 3; i += 2)
                        {
                            strcurnoisedata += BitConverter.ToInt16(new byte[] { pack.Data[i + 1], pack.Data[i] }, 0) + ",";
                        }
                        if (strcurnoisedata.EndsWith(","))
                            strcurnoisedata = strcurnoisedata.Substring(0, strcurnoisedata.Length - 1);
                        GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.NoiseTer, string.Format("噪声远传控制器[{0}]|记录仪[{1}]|启动值[{2}]|总包数:{3}、当前第{4}包|噪声数据:{5}|电压值:{6}V|信号强度:{7}",
                               pack.DevID, logId, standvalue, sumpackcount, curpackindex, strcurnoisedata, volvalue, field_strength)));

                        GlobalValue.Instance.GPRS_NoiseFrameData.Enqueue(framedata);  //通知存储线程处理
                        GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertNoiseValue);
                    }
                }
                #endregion
            }
            else if (pack.ID3 == (byte)Entity.ConstValue.DEV_TYPE.WATER_WORKS)
            {
                #region 水厂数据
                if (pack.C1 == (byte)GPRS_READ.READ_WATERWORKSDATA)  //水厂PLC采集数据
                {
                    if (pack.DataLength != 50 && pack.DataLength != (50 + 4 * 4 + 4 * 2))  //电流4*4(byte)+频率4*2(byte)
                    {
                        throw new ArgumentException(DateTime.Now.ToString() + " 帧数据长度[" + pack.DataLength + "]不符合固定长度(50/74byte)规则");
                    }

                    GPRSWaterWorkerFrameDataEntity framedata = new GPRSWaterWorkerFrameDataEntity();
                    framedata.TerId = pack.DevID.ToString();
                    framedata.ModifyTime = DateTime.Now;
                    framedata.Frame = str_frame;

                    int i = 0;
                    string stractivenerge1 = String.Format("{0:X2}", pack.Data[i + 0]) + String.Format("{0:X2}", pack.Data[i + 1]) + String.Format("{0:X2}", pack.Data[i + 2]) + String.Format("{0:X2}", pack.Data[i + 3]);
                    framedata.Activenerge1 = Convert.ToSingle(stractivenerge1) / 100;         //1#有功电量
                    string strreactivenerge1 = String.Format("{0:X2}", pack.Data[i + 4]) + String.Format("{0:X2}", pack.Data[i + 5]) + String.Format("{0:X2}", pack.Data[i + 6]) + String.Format("{0:X2}", pack.Data[i + 7]);
                    framedata.Rectivenerge1 = Convert.ToSingle(strreactivenerge1) / 100;         //1#无功电量
                    string stractivenerge2 = String.Format("{0:X2}", pack.Data[i + 8]) + String.Format("{0:X2}", pack.Data[i + 9]) + String.Format("{0:X2}", pack.Data[i + 10]) + String.Format("{0:X2}", pack.Data[i + 11]);
                    framedata.Activenerge2 = Convert.ToSingle(stractivenerge2) / 100;         //2#有功电量
                    string strreactivenerge2 = String.Format("{0:X2}", pack.Data[i + 12]) + String.Format("{0:X2}", pack.Data[i + 13]) + String.Format("{0:X2}", pack.Data[i + 14]) + String.Format("{0:X2}", pack.Data[i + 15]);
                    framedata.Rectivenerge2 = Convert.ToSingle(strreactivenerge2) / 100;         //2#无功电量
                    string stractivenerge3 = String.Format("{0:X2}", pack.Data[i + 16]) + String.Format("{0:X2}", pack.Data[i + 17]) + String.Format("{0:X2}", pack.Data[i + 18]) + String.Format("{0:X2}", pack.Data[i + 19]);
                    framedata.Activenerge3 = Convert.ToSingle(stractivenerge3) / 100;         //3#有功电量
                    string strreactivenerge3 = String.Format("{0:X2}", pack.Data[i + 20]) + String.Format("{0:X2}", pack.Data[i + 21]) + String.Format("{0:X2}", pack.Data[i + 22]) + String.Format("{0:X2}", pack.Data[i + 23]);
                    framedata.Rectivenerge3 = Convert.ToSingle(strreactivenerge3) / 100;         //3#无功电量
                    string stractivenerge4 = String.Format("{0:X2}", pack.Data[i + 24]) + String.Format("{0:X2}", pack.Data[i + 25]) + String.Format("{0:X2}", pack.Data[i + 26]) + String.Format("{0:X2}", pack.Data[i + 27]);
                    framedata.Activenerge4 = Convert.ToSingle(stractivenerge4) / 100;         //4#有功电量
                    string strreactivenerge4 = String.Format("{0:X2}", pack.Data[i + 28]) + String.Format("{0:X2}", pack.Data[i + 29]) + String.Format("{0:X2}", pack.Data[i + 30]) + String.Format("{0:X2}", pack.Data[i + 31]);
                    framedata.Rectivenerge4 = Convert.ToSingle(strreactivenerge4) / 100;         //4#无功电量

                    framedata.Pressure = (double)BitConverter.ToInt16(new byte[] { pack.Data[i + 33], pack.Data[i + 32] }, 0) / 1000;         //出口压力
                    framedata.LiquidLevel = BitConverter.ToSingle(new byte[] { pack.Data[i + 37], pack.Data[i + 36], pack.Data[i + 35], pack.Data[i + 34] }, 0);         //液位
                    framedata.Flow1 = BitConverter.ToInt32(new byte[] { pack.Data[i + 41], pack.Data[i + 40], pack.Data[i + 39], pack.Data[i + 38] }, 0);         //流量1
                    framedata.Flow2 = BitConverter.ToInt32(new byte[] { pack.Data[i + 45], pack.Data[i + 44], pack.Data[i + 43], pack.Data[i + 42] }, 0);         //流量2
                                                                                                                                                                  //2个字节表示一个开关状态,第一个字节没有用  0x00 0x01表示开  0x00 0x00表示关
                    framedata.Switch1 = pack.Data[i + 46] > 0 ? true : false;            //开关状态1
                    framedata.Switch2 = pack.Data[i + 47] > 0 ? true : false;            //开关状态2
                    framedata.Switch3 = pack.Data[i + 48] > 0 ? true : false;            //开关状态3
                    framedata.Switch4 = pack.Data[i + 49] > 0 ? true : false;            //开关状态4

                    if (pack.DataLength == 50 + 4 * 4 + 4 * 2)
                    {
                        framedata.Current1 = BitConverter.ToSingle(new byte[] { pack.Data[i + 53], pack.Data[i + 52], pack.Data[i + 51], pack.Data[i + 50] }, 0);
                        framedata.Current2 = BitConverter.ToSingle(new byte[] { pack.Data[i + 57], pack.Data[i + 56], pack.Data[i + 55], pack.Data[i + 54] }, 0);
                        framedata.Current3 = BitConverter.ToSingle(new byte[] { pack.Data[i + 61], pack.Data[i + 60], pack.Data[i + 59], pack.Data[i + 58] }, 0);
                        framedata.Current4 = BitConverter.ToSingle(new byte[] { pack.Data[i + 65], pack.Data[i + 64], pack.Data[i + 63], pack.Data[i + 62] }, 0);

                        //framedata.Current1 = Convert.ToSingle(String.Format("{0:X2}", pack.Data[i + 50]) + String.Format("{0:X2}", pack.Data[i + 51])) / 100;
                        //framedata.Current2 = Convert.ToSingle(String.Format("{0:X2}", pack.Data[i + 52]) + String.Format("{0:X2}", pack.Data[i + 53])) / 100;
                        //framedata.Current3 = Convert.ToSingle(String.Format("{0:X2}", pack.Data[i + 54]) + String.Format("{0:X2}", pack.Data[i + 55])) / 100;
                        //framedata.Current4 = Convert.ToSingle(String.Format("{0:X2}", pack.Data[i + 56]) + String.Format("{0:X2}", pack.Data[i + 57])) / 100;

                        framedata.Freq1 = Convert.ToSingle(String.Format("{0:X2}", pack.Data[i + 66]) + String.Format("{0:X2}", pack.Data[i + 67])) / 100;   //1-4#频率
                        framedata.Freq2 = Convert.ToSingle(String.Format("{0:X2}", pack.Data[i + 68]) + String.Format("{0:X2}", pack.Data[i + 69])) / 100;
                        framedata.Freq3 = Convert.ToSingle(String.Format("{0:X2}", pack.Data[i + 70]) + String.Format("{0:X2}", pack.Data[i + 71])) / 100;
                        framedata.Freq4 = Convert.ToSingle(String.Format("{0:X2}", pack.Data[i + 72]) + String.Format("{0:X2}", pack.Data[i + 73])) / 100;
                    }

                    GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.WaterWork, string.Format("水厂数据[{0}]|1#有功电量:{1}|1#无功电量:{2}|2#有功电量:{3}|2#无功电量:{4}|3#有功电量:{5}|3#无功电量:{6}|4#有功电量:{7}|4#无功电量:{8}|出口压力:{9}|液位:{10}|流量1:{11}|流量2:{12}|开关状态1:{13}|开关状态2:{14}|开关状态3:{15}|开关状态4:{16}|1#电流:{17}|2#电流:{18}|3#电流:{19}|4#电流:{20}|1#频率:{21}|2#频率:{22}|3#频率:{23}|4#频率:{24}",
                        pack.DevID, framedata.Activenerge1, framedata.Rectivenerge1, framedata.Activenerge2, framedata.Rectivenerge2, framedata.Activenerge3, framedata.Rectivenerge3, framedata.Activenerge4, framedata.Rectivenerge4,
                         framedata.Pressure, framedata.LiquidLevel, framedata.Flow1, framedata.Flow2, framedata.Switch1 ? "开" : "关", framedata.Switch2 ? "开" : "关", framedata.Switch3 ? "开" : "关", framedata.Switch4 ? "开" : "关",
                        framedata.Current1, framedata.Current2, framedata.Current3, framedata.Current4, framedata.Freq1, framedata.Freq2, framedata.Freq3, framedata.Freq4)));

                    //bNeedCheckTime = NeedCheckTime(framedata.ColTime);
                    GlobalValue.Instance.GPRS_WaterworkerFrameData.Enqueue(framedata); //通知存储线程处理
                    GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertWaterworkerValue);
                }
                #endregion
            }
        }

        /// <summary>
        /// 处理报警
        /// </summary>
        /// <param name="DevId">设备Id</param>
        /// <param name="terType">设备类型</param>
        /// <param name="FunCode">功能码</param>
        /// <param name="AlarmFlaglow">报警标志低位</param>
        /// <param name="AlarmFlaghigh">报警标志高位</param>
        /// <param name="str_frame">报警帧</param>
        /// <param name="AlarmTime">报警时间</param>
        private void ProcAlarm(short DevId, TerType terType, ConstValue.DEV_TYPE devType, byte FunCode, byte AlarmFlaglow, byte AlarmFlaghigh, string str_frame, DateTime AlarmTime)
        {
            try
            {
                //报警
                StringBuilder str_alarm = new StringBuilder();
                Dictionary<int, string> dictalarms = AlarmProc.GetAlarmName(GlobalValue.Instance.lstAlarmType, (byte)devType, FunCode, AlarmFlaglow, AlarmFlaghigh);
                if (dictalarms != null && dictalarms.Count > 0)
                {
                    GPRSAlarmFrameDataEntity alarmframedata = new GPRSAlarmFrameDataEntity();
                    alarmframedata.Frame = str_frame;
                    alarmframedata.TerId = DevId.ToString();
                    alarmframedata.TerminalType = terType;
                    try
                    {
                        alarmframedata.ModifyTime = AlarmTime;
                    }
                    catch { alarmframedata.ModifyTime = ConstValue.MinDateTime; }

                    alarmframedata.AlarmId = new List<int>();
                    foreach (var de in dictalarms)
                    {
                        alarmframedata.AlarmId.Add(de.Key);
                        str_alarm.Append(de.Value + " ");

                    }
                    GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.Alarm, string.Format("{0}[{1}] {2}",
                       new EnumHelper().GetEnumDescription(terType), DevId, str_alarm)));

                    GlobalValue.Instance.GPRS_AlarmFrameData.Enqueue(alarmframedata);
                    GlobalValue.Instance.SocketSQLMag.Send(SQLType.InsertAlarm);
                }
            }
            catch
            {
                ;
            }
        }

        /// <summary>
        /// 通用终端招测
        /// </summary>
        /// <param name="pack"></param>
        public void UniversalTerCallData(Package pack)
        {
            TerminalDataBLL dataBll = new TerminalDataBLL();
            List<string> lstmsg = new List<string>();
            ColorType colortype = (pack.ID3 == (byte)Entity.ConstValue.DEV_TYPE.Data_CTRL) ? ColorType.PreTer : ColorType.UniversalTer;
            if (pack.C1 == (byte)UNIVERSAL_COMMAND.CallData_Pre1)
            {
                if (pack.Data == null || pack.Data.Length == 0)
                {
                    lstmsg.Add("招测压力无数据");
                }
                else
                    dataBll.AnalysisPre(pack.DevID, pack, GlobalValue.Instance.UniversalDataConfig, ref lstmsg, GlobalValue.Instance.lstAlarmType);

                foreach (string msgresult in lstmsg)
                {
                    GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(colortype, msgresult));
                }
                lstmsg.Clear();
            }
            else if (pack.C1 == (byte)UNIVERSAL_COMMAND.CallData_Sim1)
            {
                if (pack.Data == null || pack.Data.Length == 0)
                {
                    lstmsg.Add("招测模拟量1路无数据");
                }
                else
                    dataBll.AnalysisSim(pack.DevID, pack, GlobalValue.Instance.UniversalDataConfig, ref lstmsg, GlobalValue.Instance.lstAlarmType);

                foreach (string msgresult in lstmsg)
                {
                    GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(colortype, msgresult));
                }
                lstmsg.Clear();
            }
            else if (pack.C1 == (byte)UNIVERSAL_COMMAND.CallData_Sim2)
            {
                if (pack.Data == null || pack.Data.Length == 0)
                {
                    lstmsg.Add("招测模拟量2路无数据");
                }
                else
                    dataBll.AnalysisSim(pack.DevID, pack, GlobalValue.Instance.UniversalDataConfig, ref lstmsg, GlobalValue.Instance.lstAlarmType);

                foreach (string msgresult in lstmsg)
                {
                    GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(colortype, msgresult));
                }
                lstmsg.Clear();
            }
            else if (pack.C1 == (byte)UNIVERSAL_COMMAND.CallData_Pluse)
            {
                if (pack.Data == null || pack.Data.Length == 0)
                {
                    lstmsg.Add("招测脉冲无数据");
                }
                else
                    dataBll.AnalysisPluse(pack.DevID, pack, GlobalValue.Instance.UniversalDataConfig, ref lstmsg, GlobalValue.Instance.lstAlarmType);

                foreach (string msgresult in lstmsg)
                {
                    GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(colortype, msgresult));
                }
                lstmsg.Clear();
            }
            else if (pack.C1 == (byte)UNIVERSAL_COMMAND.CallData_RS4851)
            {
                if (pack.Data == null || pack.Data.Length == 0)
                {
                    lstmsg.Add("招测RS485 1路无数据");
                }
                else
                {
                    if (pack.Data.Length == 20)  //长度为20的作为流量解析
                        dataBll.AnalysisRS485Flow(pack.DevID, pack, GlobalValue.Instance.UniversalDataConfig, ref lstmsg, GlobalValue.Instance.lstAlarmType);
                    else
                        dataBll.AnalysisRS485(pack.DevID, pack, GlobalValue.Instance.UniversalDataConfig, ref lstmsg, GlobalValue.Instance.lstAlarmType);
                }

                foreach (string msgresult in lstmsg)
                {
                    GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(colortype, msgresult));
                }
                lstmsg.Clear();
            }
            else if (pack.C1 == (byte)UNIVERSAL_COMMAND.CallData_RS4852)
            {
                if (pack.Data == null || pack.Data.Length == 0)
                {
                    lstmsg.Add("招测RS485 2路无数据");
                }
                else
                    dataBll.AnalysisRS485(pack.DevID, pack, GlobalValue.Instance.UniversalDataConfig, ref lstmsg, GlobalValue.Instance.lstAlarmType);

                foreach (string msgresult in lstmsg)
                {
                    GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(colortype, msgresult));
                }
                lstmsg.Clear();
            }
            else if (pack.C1 == (byte)UNIVERSAL_COMMAND.CallData_RS4853)
            {
                if (pack.Data == null || pack.Data.Length == 0)
                {
                    lstmsg.Add("招测RS485 3路无数据");
                }
                else
                    dataBll.AnalysisRS485(pack.DevID, pack, GlobalValue.Instance.UniversalDataConfig, ref lstmsg, GlobalValue.Instance.lstAlarmType);

                foreach (string msgresult in lstmsg)
                {
                    GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(colortype, msgresult));
                }
                lstmsg.Clear();
            }
            else if (pack.C1 == (byte)UNIVERSAL_COMMAND.CallData_RS4854)
            {
                if (pack.Data == null || pack.Data.Length == 0)
                {
                    lstmsg.Add("招测RS485 4路无数据");
                }
                else
                    dataBll.AnalysisRS485(pack.DevID, pack, GlobalValue.Instance.UniversalDataConfig, ref lstmsg, GlobalValue.Instance.lstAlarmType);
            }

            foreach (string msgresult in lstmsg)
            {
                GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(colortype, msgresult));
            }
            lstmsg.Clear();

        }

        #region 运算函数
        /// <summary>
        /// 获取纠偏函数
        /// </summary>
        /// <param name="pack"></param>
        /// <returns></returns>
        public string GetRectifyFun(Package pack)
        {
            if (GlobalValue.Instance.lstRectifyFun.ContainsKey((pack.DevID).ToString() + ((int)pack.DevType).ToString().PadLeft(2, '0') + pack.C1.ToString().PadLeft(2, '0')))
            {
                return GlobalValue.Instance.lstRectifyFun[(pack.DevID).ToString() + ((int)pack.DevType).ToString().PadLeft(2, '0') + pack.C1.ToString().PadLeft(2, '0')];
            }
            return "";
        }
        /// <summary>
        /// 获取纠偏函数
        /// </summary>
        /// <param name="pack"></param>
        /// <param name="waytype">第几路</param>
        /// <returns></returns>
        public string GetRectifyFun(Package pack,int waytype)
        {
            if (GlobalValue.Instance.lstRectifyFun.ContainsKey((pack.DevID).ToString() + ((int)pack.DevType).ToString().PadLeft(2, '0') + pack.C1.ToString().PadLeft(2, '0') + waytype.ToString().PadLeft(2, '0')))
            {
                return GlobalValue.Instance.lstRectifyFun[(pack.DevID).ToString() + ((int)pack.DevType).ToString().PadLeft(2, '0') + pack.C1.ToString().PadLeft(2, '0')+waytype.ToString().PadLeft(2,'0')];
            }
            return "";
        }

        /// <summary>
        /// 纠偏运算,失败返回null
        /// </summary>
        /// <returns></returns>
        public object RectifyCalc(Package pack, out string CalcExpress, params object[] parms)
        {
            string TmpRectifyValue = "";
            CalcExpress = "";
            try
            {
                TmpRectifyValue = GetRectifyFun(pack);
                if (!string.IsNullOrEmpty(TmpRectifyValue))
                {
                    CalcExpress = string.Format(TmpRectifyValue, parms);
                    return SmartWaterSystem.JScriptMath.EvalExpress(CalcExpress);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                string strparm = "";
                for (int i = 0; i < parms.Length; i++)
                {
                    if (i == parms.Length - 1)
                        strparm += parms[i].ToString();
                    else
                        strparm += parms[i].ToString() + ",";
                }
                GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.Error, string.Format("纠偏运算错误,纠偏函数:{0},参数列表:{1}", TmpRectifyValue, strparm)));
                return null;
            }
        }

        /// <summary>
        /// 纠偏运算,失败返回null
        /// </summary>
        /// <param name="Waytype">第几路</param>
        /// <returns></returns>
        public object RectifyCalc(Package pack,int Waytype,out string CalcExpress, params object[] parms)
        {
            string TmpRectifyValue = "";
            CalcExpress = "";
            try
            {
                TmpRectifyValue = GetRectifyFun(pack, Waytype);
                if (!string.IsNullOrEmpty(TmpRectifyValue))
                {
                    CalcExpress = string.Format(TmpRectifyValue, parms);
                    return SmartWaterSystem.JScriptMath.EvalExpress(CalcExpress);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                string strparm = "";
                for(int i=0;i<parms.Length;i++)
                {
                    if (i == parms.Length - 1)
                        strparm += parms[i].ToString();
                    else
                        strparm += parms[i].ToString() + ",";
                }
                GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(ColorType.Error, string.Format("纠偏运算错误,纠偏函数:{0},第{1}路,参数列表:{2}", TmpRectifyValue, Waytype, strparm)));
                return null;
            }
        }
        #endregion

    }
}
