using Common;
using Entity;
using System;
using System.Text;

namespace BLL
{
    public class GPRSCmdMSg
    {
        public string GetPackageDesc(Package pack)
        {
            StringBuilder Msg = new StringBuilder();
            try {
                EnumHelper enumHelper = new EnumHelper();
                string TerName = enumHelper.GetEnumDescription(pack.DevType);
                Msg.Append(TerName + "[" + pack.DevID + "],");
                UNIVERSAL_COMMAND funcode = (UNIVERSAL_COMMAND)pack.C1;
                string funcodeName = enumHelper.GetEnumDescription(funcode);
                Msg.Append(funcodeName);

                if(pack.DataLength==0)
                    return Msg.ToString();
                string DataDesc = "";   //数据部分描述
                switch (funcode)
                {
                    case UNIVERSAL_COMMAND.RESET:       //无数据部分描述
                    case UNIVERSAL_COMMAND.EnableCollect:
                    case UNIVERSAL_COMMAND.CalibartionSimualte1:
                    case UNIVERSAL_COMMAND.CalibartionSimualte2:
                    case UNIVERSAL_COMMAND.READ_ID:
                    case UNIVERSAL_COMMAND.SET_ID:
                    case UNIVERSAL_COMMAND.SET_CALLENABLE:
                        break;
                    case UNIVERSAL_COMMAND.READ_TIME:
                    case UNIVERSAL_COMMAND.SET_TIME:
                        DataDesc =(Convert.ToInt32(pack.Data[0]) + 2000) +"/"+ Convert.ToInt32(pack.Data[1])+"/"+Convert.ToInt32(pack.Data[2])+" "+
                Convert.ToInt32(pack.Data[3]) + ":" + Convert.ToInt32(pack.Data[4]) + ":" + Convert.ToInt32(pack.Data[5]);
                        break;
                    case UNIVERSAL_COMMAND.READ_CELLPHONE:
                    case UNIVERSAL_COMMAND.SET_CELLPHONE:
                        string number = "";
                        for (int i = 0; i < pack.DataLength; i++)
                        {
                            number += (char)pack.Data[i];
                        }
                        DataDesc = number;
                        break;
                    case UNIVERSAL_COMMAND.READ_IP:
                    case UNIVERSAL_COMMAND.SET_IP:
                        DataDesc = Encoding.Default.GetString(pack.Data);
                        break;
                    case UNIVERSAL_COMMAND.READ_PORT:
                    case UNIVERSAL_COMMAND.SET_PORT:
                        DataDesc = "";
                        for (int i = 0; i < pack.Data.Length; i++)
                        {
                            if ((char)pack.Data[i] != '\0')
                                DataDesc += (char)pack.Data[i];
                        }
                        break;
                    case UNIVERSAL_COMMAND.READ_HEARTINTERVAL:
                    case UNIVERSAL_COMMAND.SET_HEART:
                        DataDesc = BitConverter.ToInt16(new byte[] { pack.Data[1], pack.Data[0] }, 0).ToString()+"s";
                        break;
                    case UNIVERSAL_COMMAND.READ_VOLINTERVAL:
                    case UNIVERSAL_COMMAND.SET_VOLINTERVAL:
                        DataDesc = BitConverter.ToInt16(new byte[] { pack.Data[1], pack.Data[0] }, 0).ToString()+"min";
                        break;
                    case UNIVERSAL_COMMAND.READ_SMSINTERVAL:
                    case UNIVERSAL_COMMAND.SET_SMSINTERVAL:
                        DataDesc = BitConverter.ToInt16(new byte[] { pack.Data[1], pack.Data[0] }, 0).ToString() + "min";
                        break;
                    case UNIVERSAL_COMMAND.READ_NETWORKTYPE:
                    case UNIVERSAL_COMMAND.SET_NETWORKTYPE:
                        if (pack.Data[0] == 0x00)
                            DataDesc = "不连网";
                        else if (pack.Data[0] == 0x01)
                            DataDesc = "低功耗模式";
                        else if (pack.Data[0] == 0x02)
                            DataDesc = "实时在线";
                        break;
                    case UNIVERSAL_COMMAND.READ_COMTYPE:
                    case UNIVERSAL_COMMAND.SET_COMTYPE:
                        if (pack.Data[0] == 0x00)  //0-移动GSM，1-GPRS方式 2-CDMA方式  3-电信GSM
                            DataDesc = "移动GSM";
                        else if (pack.Data[0] == 0x01)
                            DataDesc = "GPRS方式";
                        else if (pack.Data[0] == 0x02)
                            DataDesc = "CDMA方式";
                        else if (pack.Data[0] == 0x03)
                            DataDesc = "电信GSM";
                        break;
                    case UNIVERSAL_COMMAND.READ_485BAUD:
                    case UNIVERSAL_COMMAND.SET_485BAUD:
                        DataDesc = Convert.ToInt32(pack.Data[0]).ToString();
                        if (pack.Data[0] == 0x00)
                            DataDesc = "1200";
                        else if (pack.Data[0] == 0x01)
                            DataDesc = "2400";
                        else if (pack.Data[0] == 0x02)
                            DataDesc = "4800";
                        else if (pack.Data[0] == 0x03)
                            DataDesc = "9600";
                        break;
                    case UNIVERSAL_COMMAND.READ_MODBUSEXEFLAG:
                    case UNIVERSAL_COMMAND.SET_MODBUSEXEFLAG:
                        DataDesc = (Convert.ToInt32(pack.Data[0]) == 1) ? "执行" : "不执行";
                        break;
                    case UNIVERSAL_COMMAND.READ_VOLLOWER:
                    case UNIVERSAL_COMMAND.SET_VOLLOWER:
                        DataDesc = Convert.ToInt16(pack.Data[0]).ToString()+"%";
                        break;
                    case UNIVERSAL_COMMAND.READ_PLUSEUNIT:
                    case UNIVERSAL_COMMAND.SET_PLUSEUNIT:
                        DataDesc = Convert.ToInt16(pack.Data[0]).ToString();
                        break;
                    case UNIVERSAL_COMMAND.READ_ALARMLEN:
                    case UNIVERSAL_COMMAND.SET_ALARMLEN:
                        DataDesc = BitConverter.ToInt16(new byte[] { pack.Data[1], pack.Data[0] }, 0).ToString()+"次";
                        break;
                    case UNIVERSAL_COMMAND.READ_COLLECTCONFIG:
                    case UNIVERSAL_COMMAND.SET_COLLECTCONFIG:
                        if ((pack.Data[0] & 0x01) == 0x01)
                            DataDesc = "数字量压力;";
                        if ((pack.Data[0] & 0x02) == 0x02)
                            DataDesc = "脉冲量;";
                        if ((pack.Data[0] & 0x04) == 0x04)
                            DataDesc = "第1路模拟量;";
                        if ((pack.Data[0] & 0x08) == 0x08)
                            DataDesc = "第2路模拟量;";
                        if ((pack.Data[0] & 0x10) == 0x10)
                            DataDesc = "RS495;";
                        DataDesc = "";
                        break;
                    case UNIVERSAL_COMMAND.READ_PREUPLIMIT:
                    case UNIVERSAL_COMMAND.READ_PRELOWLIMIT:
                    case UNIVERSAL_COMMAND.READ_PRESLOPUPLIMIT:
                    case UNIVERSAL_COMMAND.READ_PRESLOPLOWLIMIT:
                    case UNIVERSAL_COMMAND.SET_PREUPLIMIT:
                    case UNIVERSAL_COMMAND.SET_PRELOWLIMIT:
                    case UNIVERSAL_COMMAND.SET_PRESLOPUPLIMIT:
                    case UNIVERSAL_COMMAND.SET_PRESLOPLOWLIMIT:
                        DataDesc = (((double)BitConverter.ToInt16(new byte[] { pack.Data[2], pack.Data[1] }, 0)) / 1000).ToString("f2");
                        break;
                    case UNIVERSAL_COMMAND.READ_SIMUPLIMIT:
                    case UNIVERSAL_COMMAND.READ_SIMLOWLIMIT:
                    case UNIVERSAL_COMMAND.READ_SIMSLOPUPLIMIT:
                    case UNIVERSAL_COMMAND.READ_SIMSLOPLOWLIMIT:
                    case UNIVERSAL_COMMAND.SET_SIMUPLIMIT:
                    case UNIVERSAL_COMMAND.SET_SIMLOWLIMIT:
                    case UNIVERSAL_COMMAND.SET_SIMSLOPUPLIMIT:
                    case UNIVERSAL_COMMAND.SET_SIMSLOPLOWLIMIT:
                        double range = 0;
                        range += BitConverter.ToInt16(new byte[] { pack.Data[2], pack.Data[1] }, 0);    //整数部分
                        range += ((double)BitConverter.ToInt16(new byte[] { pack.Data[4], pack.Data[3] }, 0)) / 1000;    //小数部分
                        DataDesc = (((double)(BitConverter.ToInt16(new byte[] { pack.Data[6], pack.Data[5] }, 0))) * range / (ConstValue.UniversalSimRatio)).ToString("f2");
                        break;
                    case UNIVERSAL_COMMAND.READ_FLOWUPLIMIT:
                    case UNIVERSAL_COMMAND.READ_FLOWLOWLIMIT:
                    case UNIVERSAL_COMMAND.READ_FLOWSLOPUPLIMIT:
                    case UNIVERSAL_COMMAND.READ_FLOWSLOPLOWLIMIT:
                    case UNIVERSAL_COMMAND.SET_FLOWUPLIMIT:
                    case UNIVERSAL_COMMAND.SET_FLOWLOWLIMIT:
                    case UNIVERSAL_COMMAND.SET_FLOWSLOPUPLIMIT:
                    case UNIVERSAL_COMMAND.SET_FLOWSLOPLOWLIMIT:
                        DataDesc = (((double)BitConverter.ToInt32(new byte[] { pack.Data[4], pack.Data[3], pack.Data[2], pack.Data[1] }, 0)) / 1000).ToString("f2");
                        break;
                    case UNIVERSAL_COMMAND.READ_UPENABLE:
                    case UNIVERSAL_COMMAND.READ_LOWENABLE:
                    case UNIVERSAL_COMMAND.READ_SLOPUPENABLE:
                    case UNIVERSAL_COMMAND.READ_SLOPLOWENABLE:
                    case UNIVERSAL_COMMAND.SET_UPENABLE:
                    case UNIVERSAL_COMMAND.SET_LOWENABLE:
                    case UNIVERSAL_COMMAND.SET_SLOPUPENABLE:
                    case UNIVERSAL_COMMAND.SET_SLOPLOWENABLE:
                        DataDesc = (pack.Data[1] == 0x01) ? "报警开启" : "报警关闭";
                        break;
                    case UNIVERSAL_COMMAND.READ_PRERANGE:
                    case UNIVERSAL_COMMAND.SET_PRERANGE:
                        DataDesc = (((double)(BitConverter.ToInt16(new byte[] { pack.Data[2], pack.Data[1] }, 0))) / 1000).ToString("f2");
                        break;
                    case UNIVERSAL_COMMAND.READ_SIMRANGE:
                    case UNIVERSAL_COMMAND.SET_SIMRANGE:
                        range = 0;
                        range += BitConverter.ToInt16(new byte[] { pack.Data[2], pack.Data[1] }, 0);    //整数部分
                        range += ((double)BitConverter.ToInt16(new byte[] { pack.Data[4], pack.Data[3] }, 0))/1000;    //小数部分
                        DataDesc = range.ToString();
                        break;
                    case UNIVERSAL_COMMAND.READ_PREOFFSET:
                    case UNIVERSAL_COMMAND.SET_PREOFFSET:
                        DataDesc = (((double)BitConverter.ToInt16(new byte[] { pack.Data[2], pack.Data[1] }, 0)) / 1000).ToString();
                        break;
                    case UNIVERSAL_COMMAND.READ_SIMINTERVAL:
                    case UNIVERSAL_COMMAND.SET_SIMINTERVAL:
                        DataDesc = "\r\n起始时间\t发送间隔\t采集间隔1\t采集间隔2\r\n";
                        for (int i = 0; i < pack.DataLength; i += 7)
                        {
                            DataDesc += Convert.ToInt32(pack.Data[i])
                            + "\t\t" + BitConverter.ToInt16(new byte[] { pack.Data[i + 2], pack.Data[i + 1] }, 0)
                            + "\t\t" + BitConverter.ToInt16(new byte[] { pack.Data[i + 4], pack.Data[i + 3] }, 0)
                            + "\t\t" + BitConverter.ToInt16(new byte[] { pack.Data[i + 6], pack.Data[i + 5] }, 0) + "\r\n";
                        }
                        break;
                    case UNIVERSAL_COMMAND.READ_PLUSEINTERVAL:
                    case UNIVERSAL_COMMAND.SET_PLUSEINTERVAL:
                        DataDesc = "\r\n起始时间\t发送间隔\t压力采集间隔\t脉冲采集间隔\r\n";
                        for (int i = 0; i < pack.DataLength; i += 7)
                        {
                            DataDesc +=  Convert.ToInt32(pack.Data[i]) 
                            + "\t\t" + BitConverter.ToInt16(new byte[] { pack.Data[i + 2], pack.Data[i + 1] }, 0)
                            + "\t\t" + BitConverter.ToInt16(new byte[] { pack.Data[i + 4], pack.Data[i + 3] }, 0)
                            + "\t\t" + BitConverter.ToInt16(new byte[] { pack.Data[i + 6], pack.Data[i + 5] }, 0) + "\r\n";
                        }
                        break;
                    case UNIVERSAL_COMMAND.READ_485INTERVAL:
                    case UNIVERSAL_COMMAND.SET_485INTERVAL:
                        DataDesc = "\r\n起始时间\t发送间隔\t采集间隔\r\n";
                        for (int i = 0; i < pack.DataLength; i += 7)
                        {
                            DataDesc += Convert.ToInt32(pack.Data[i]) 
                            + "\t\t" + BitConverter.ToInt16(new byte[] { pack.Data[i + 2], pack.Data[i + 1] }, 0)
                            + "\t\t" + BitConverter.ToInt16(new byte[] { pack.Data[i + 4], pack.Data[i + 3] }, 0)
                            + "\t\t" + BitConverter.ToInt16(new byte[] { pack.Data[i + 6], pack.Data[i + 5] }, 0) + "\r\n";
                        }
                        break;
                    case UNIVERSAL_COMMAND.READ_MODBUSPROTOCOL:
                    case UNIVERSAL_COMMAND.SET_MODBUSPROTOCOL:
                        DataDesc = "\r\n波特率\tID\t功能码\t寄存器起始地址\t寄存器数量\r\n";
                        for (int i = 0; i < pack.DataLength; i += 7)
                        {
                            int baud = Convert.ToInt32(pack.Data[i]);
                            if (baud == 0)
                                baud = 1200;
                            else if (baud == 1)
                                baud = 2400;
                            else if (baud == 2)
                                baud = 4800;
                            else if (baud == 3)
                                baud = 9600;
                            DataDesc +=  baud
                             + "\t" + Convert.ToInt32(pack.Data[i + 1]) + "\t" + "0x" + String.Format("{0:X2}", pack.Data[i + 2])
                             + "\t" + "0x" + String.Format("{0:X2}", pack.Data[i + 4]) + String.Format("{0:X2}", pack.Data[i + 3])
                             + "\t" + BitConverter.ToInt16(new byte[] { pack.Data[i + 6], pack.Data[i + 5] }, 0) + "\r\n";
                        }
                        break;
                    case UNIVERSAL_COMMAND.SET_PLUSEBASIC1:
                    case UNIVERSAL_COMMAND.SET_PLUSEBASIC2:
                    case UNIVERSAL_COMMAND.SET_PLUSEBASIC3:
                    case UNIVERSAL_COMMAND.SET_PLUSEBASIC4:
                        DataDesc =BitConverter.ToInt32(new byte[] { pack.Data[3], pack.Data[2], pack.Data[1], pack.Data[0] }, 0).ToString();
                        break;
                    case UNIVERSAL_COMMAND.READ_VER:
                        DataDesc = Encoding.Default.GetString(pack.Data);
                        break;
                    case UNIVERSAL_COMMAND.READ_FIELDSTRENGTH:
                        DataDesc = "场强:" + pack.Data[0] + ",电压:" + ((float)BitConverter.ToInt16(new byte[] { pack.Data[2], pack.Data[1] }, 0)) / 1000; ;
                        break;
                    default:
                        break;
                }

                if (!string.IsNullOrEmpty(DataDesc))
                    Msg.Append("," + DataDesc);
                return Msg.ToString() ;
            }catch(Exception ex)
            {
                return "";
            }
        }

    }
}
