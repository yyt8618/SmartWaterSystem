using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCGPRSService
{
    /// <summary>
    /// 肯特水表相关
    /// </summary>
    public class KERTFlow
    {
        public static GPRSFlowFrameDataEntity ProcessData(int dataindex,string tername,string id,string frame,byte[] data,int datalen,out bool bNeedCheckTime)
        {
            int alarmflag = 0;
            //报警标志
            alarmflag = BitConverter.ToInt16(new byte[] { data[0], data[1] }, 0);

            float volvalue = -1;  //电压,如果是没有这个电压值的,赋值为-1，保存至数据库时根据-1保存空
            if (datalen - (6 + 36) * dataindex - 3 == 2)  //最后余两个字节则认为是电压值
                volvalue = ((float)BitConverter.ToInt16(new byte[] { data[datalen - 1], data[datalen - 2] }, 0)) / 1000;

            GPRSFlowFrameDataEntity framedata = new GPRSFlowFrameDataEntity();
            framedata.TerId = id;
            framedata.ModifyTime = DateTime.Now;
            framedata.Frame = frame;

            bNeedCheckTime = false;

            int year = 0, month = 0, day = 0, hour = 0, minute = 0, sec = 0;
            double forward_flowvalue = 0, reverse_flowvalue = 0, instant_flowvalue = 0;
            for (int i = 0; i < dataindex; i++)
            {
                year = 2000 + Convert.ToInt16(data[i * 42 + 3]);
                month = Convert.ToInt16(data[i * 42 + 4]);
                day = Convert.ToInt16(data[i * 42 + 5]);
                hour = Convert.ToInt16(data[i * 42 + 6]);
                minute = Convert.ToInt16(data[i * 42 + 7]);
                sec = Convert.ToInt16(data[i * 42 + 8]);

                byte balarm = 0x00;  //水表报警
                string errmsg = "";
                if (KERTFlow.ProcessDataUnit(data, i * 42 + 3 + 6, 36, out forward_flowvalue, out reverse_flowvalue, out instant_flowvalue, out balarm, out errmsg))
                {
                    string str_alarm = "空";
                    if ((balarm & 0x10) == 0x10)
                    { str_alarm = "励磁报警"; }
                    else if ((balarm & 0x08) == 0x08)
                    { str_alarm = "空管报警"; }
                    else if ((balarm & 0x04) == 0x04)
                    { str_alarm = "流浪反向报警"; }
                    else if ((balarm & 0x10) == 0x10)
                    { str_alarm = "流量上限报警"; }
                    else if ((balarm & 0x10) == 0x10)
                    { str_alarm = "流量下限报警"; }
                    GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(string.Format("index({0})|[{1}]|报警标志({2})|采集时间({3})|正向流量值:{4}|反向流量值:{5}|瞬时流量值:{6}|报警:{7}|电压值:{8}V",
                       i, tername+id, alarmflag, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, forward_flowvalue.ToString("f4"), reverse_flowvalue.ToString("f4"), instant_flowvalue.ToString("f4"), str_alarm, volvalue)));

                    GPRSFlowDataEntity flowdata = new GPRSFlowDataEntity();
                    flowdata.Forward_FlowValue = forward_flowvalue;
                    flowdata.Reverse_FlowValue = reverse_flowvalue;
                    flowdata.Instant_FlowValue = instant_flowvalue;
                    flowdata.Voltage = volvalue;
                    try
                    {
                        flowdata.ColTime = new DateTime(year, month, day, hour, minute, sec);
                    }
                    catch { flowdata.ColTime = ConstValue.MinDateTime; }
                    bNeedCheckTime = GlobalValue.Instance.SocketMag.NeedCheckTime(flowdata.ColTime);
                    framedata.lstFlowData.Add(flowdata);

                }
                else
                {
                    GlobalValue.Instance.SocketMag.OnSendMsg(new SocketEventArgs(string.Format("[{0}]|报警标志({1})|采集时间({2})|错误:{3}",
                        tername+id, alarmflag, year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec, errmsg)));
                }
            }
            return framedata;
        }

        /// <summary>
        /// 处理肯特水表数据(不包含时间数据)
        /// </summary>
        /// <param name="data">水表数据</param>
        /// <param name="startindex">水表数据起始索引</param>
        /// <param name="datalen">水表数据长度</param>
        /// <param name="forwardflow">正向流量</param>
        /// <param name="reverseflow">反向流量</param>
        /// <param name="instantflow">瞬时流量</param>
        /// <returns></returns>
        public static bool ProcessDataUnit(byte[] data,int startindex,int datalen,out double forwardflow,out double reverseflow,out double instantflow,out byte alarm,out string errmsg)
        {
            alarm = 0x00;
            errmsg = "";
            #region 瞬时流量
            instantflow = BitConverter.ToSingle(new byte[] { data[startindex + 3], data[startindex + 2], data[startindex + 1], data[startindex + 0] }, 0); //瞬时流量
            /*
            *瞬时流量单位
            00：m3/s、  01：m3/min、02：m3/h、
            03：L/s、     04：L/min、    05：L/h、
            06：t/s、      07：t/min、    08：t/h、
            09：kg/s、   10：kg/min、  11：kg/h、
            12：ft3/h、  13：ft3/min、 14：ft3/s、
            15：g/s、     16：g/min、
            ( 注 ： 输入其他值无效 、默认值为 02 )"
            */
            int instantunit=BitConverter.ToInt16(new byte[] { data[startindex + 5], data[startindex + 4] }, 0);  //瞬时流量单位,全部转换成 m3/h
            switch(instantunit)
            {
                case 0:     //m3/s
                    instantflow *= 60 * 60;  //  ->m3/h
                    break;
                case 1:     //m3/min
                    instantflow *= 60;  //  ->m3/h
                    break;
                case 2:     //m3/h
                    break;
                case 3:     //L/s
                    instantflow *= 1/1000*(60*60);  //  ->m3/h
                    break;
                case 4:     //L/min
                    instantflow *= 1/1000 * (60);  //  ->m3/h
                    break;
                case 5:     //L/h
                    instantflow *= 1/1000;  //  ->m3/h
                    break;
                case 6:     //t/s
                    instantflow *= 60 * 60;  //  ->m3/h
                    break;
                case 7:     //t/min
                    instantflow *= 60;  //  ->m3/h
                    break;
                case 8:     //t/h
                    break;
                case 9:     //kg/s
                    instantflow *= 1/1000 * (60 * 60);  //  ->m3/h
                    break;
                case 10:     //kg/min
                    instantflow *= 1/1000 * (60);  //  ->m3/h
                    break;
                case 11:     //kg/h
                    instantflow *= 1/1000;  //  ->m3/h
                    break;
                case 12:     //ft3/h
                    instantflow *= 2.83168 * 0.02;  //  ->m3/h
                    break;
                case 13:     //ft3/min
                    instantflow *= 2.83168 * 0.02 * (60);  //  ->m3/h
                    break;
                case 14:     //ft3/s
                    instantflow *= 2.83168 * 0.02 * (60*60);  //  ->m3/h
                    break;
                case 15:     //g/s
                    instantflow *= 1/(1000 * 1000) * (60 * 60);  //  ->m3/h
                    break;
                case 16:     //g/min
                    instantflow *= 1/(1000 * 1000) * 60;  //  ->m3/h
                    break;
                default:
                    errmsg = "瞬时流量单位["+instantunit+"]不在预期范围内!";
                    instantflow = 0;
                    forwardflow = 0;
                    reverseflow = 0;
                    return false;
            }
            #endregion

            #region 正向流量
            //前向流量整数部分
            forwardflow = BitConverter.ToInt32(new byte[] { data[startindex + 9], data[startindex + 8], data[startindex + 7], data[startindex + 6] }, 0);
            //前向流量小数部分
            forwardflow += BitConverter.ToSingle(new byte[] { data[startindex + 13], data[startindex + 12], data[startindex + 11], data[startindex + 10] }, 0);
            //正向流量单位
            /*
            00：m3  、   04：L、     07：mL、
            08：t  、       12：kg、   15：g、
            16：in3、     17：ft3、
            ( 注 ： 输入其他值无效、默认值为 00 )"
            */
            int forwardunit = BitConverter.ToInt16(new byte[] { data[startindex + 15], data[startindex + 14] }, 0);  //瞬时流量单位,全部转换成 m3
            switch (forwardunit)
            {
                case 0:     //m3
                    break;
                case 4:     //L
                    forwardflow *= 1/1000;  //  ->m3
                    break;
                case 7:     //mL
                    forwardflow *= 1/(1000 * 1000);  //  ->m3
                    break;
                case 8:     //t
                    break;
                case 12:     //kg
                    forwardflow *= 1/1000;  //  ->m3
                    break;
                case 15:     //g
                    forwardflow *= 1/(1000*1000);  //  ->m3
                    break;
                case 16:     //in3   1in = 2.54cm=0.0254m
                    forwardflow *= 1/Math.Pow(2.54*100,3);  //  ->m3
                    break;
                case 17:     //ft3
                    forwardflow *= 2.83168 * 0.02;  //  ->m3
                    break;
                default:
                    errmsg = "正向流量单位[" + forwardunit + "]不在预期范围内!";
                    instantflow = 0;
                    forwardflow = 0;
                    reverseflow = 0;
                    return false;
            }
            #endregion

            #region 反向流量
            //反向流量整数部分
            reverseflow = BitConverter.ToInt32(new byte[] { data[startindex + 19], data[startindex + 18], data[startindex + 17], data[startindex + 16] }, 0);
            //反向流量小数部分
            reverseflow += BitConverter.ToSingle(new byte[] { data[startindex + 23], data[startindex + 22], data[startindex + 21], data[startindex + 20] }, 0);
            //反向流量单位
            /*
            00：m3  、   04：L、     07：mL、
            08：t  、       12：kg、   15：g、
            16：in3、     17：ft3、
            ( 注 ： 输入其他值无效、默认值为 00 )"
            */
            int reverseunit = BitConverter.ToInt16(new byte[] { data[startindex + 25], data[startindex + 24] }, 0);  //瞬时流量单位,全部转换成 m3
            switch (reverseunit)
            {
                case 0:     //m3
                    break;
                case 4:     //L
                    reverseflow *= 1 / 1000;  //  ->m3
                    break;
                case 7:     //mL
                    reverseflow *= 1 / (1000 * 1000);  //  ->m3
                    break;
                case 8:     //t
                    break;
                case 12:     //kg
                    reverseflow *= 1 / 1000;  //  ->m3
                    break;
                case 15:     //g
                    reverseflow *= 1 / (1000 * 1000);  //  ->m3
                    break;
                case 16:     //in3   1in = 2.54cm=0.0254m
                    reverseflow *= 1 / Math.Pow(2.54 * 100, 3);  //  ->m3
                    break;
                case 17:     //ft3
                    reverseflow *= 2.83168 * 0.02;  //  ->m3
                    break;
                default:
                    errmsg = "反向流量单位[" + reverseunit + "]不在预期范围内!";
                    instantflow = 0;
                    forwardflow = 0;
                    reverseflow = 0;
                    return false;
            }
            #endregion

            //报警
            if (BitConverter.ToInt16(new byte[] { data[startindex + 27], data[startindex + 26] }, 0) == 1)  //励磁报警
                alarm |= 0x10;
            if (BitConverter.ToInt16(new byte[] { data[startindex + 29], data[startindex + 28] }, 0) == 1)  //空管报警
                alarm |= 0x08;
            if (BitConverter.ToInt16(new byte[] { data[startindex + 31], data[startindex + 30] }, 0) == 1)  //流量反向报警
                alarm |= 0x04;
            if (BitConverter.ToInt16(new byte[] { data[startindex + 33], data[startindex + 32] }, 0) == 1)  //流量上限报警
                alarm |= 0x02;
            if (BitConverter.ToInt16(new byte[] { data[startindex + 35], data[startindex + 34] }, 0) == 1)  //流量下限报警
                alarm |= 0x02;
            return true;
        }
        

    }
}
