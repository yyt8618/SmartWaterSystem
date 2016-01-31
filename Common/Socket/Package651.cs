using System;
using System.Collections.Generic;
using Common;

namespace Common
{
    [Serializable]
    public struct Package651
    {
        #region 原始帧
        /// <summary>
        /// 开始码 2
        /// </summary>
        public byte[] Start
        {
            get { return PackageDefine.BeginByte651; }
        }

        #region 设备编码
        /// <summary>
        /// 中心站地址（指以省/流域机构为单位，为县、市级以上分中心分配的中心站地址）
        /// </summary>
        public byte CenterAddr { get; set; }
        /// <summary>
        /// 终端地址A5
        /// </summary>
        public byte A5 { get; set; }
        /// <summary>
        /// 终端地址A4
        /// </summary>
        public byte A4 { get; set; }
        /// <summary>
        /// 终端地址A3
        /// </summary>
        public byte A3 { get; set; }
        /// <summary>
        /// 终端地址A2
        /// </summary>
        public byte A2 { get; set; }
        /// <summary>
        /// 终端地址A1
        /// </summary>
        public byte A1 { get; set; }
        /// <summary>
        /// 密码 2
        /// </summary>
        public byte[] PWD { get; set; }
        #endregion

        #region 控制码
        /// <summary>
        /// 功能码
        /// </summary>
        public byte FUNCODE { get; set; }
        #endregion

        /// <summary>
        /// 是否为上下标识(true:上行,false:下行)
        /// </summary>
        public bool IsUpload { get; set; }

        #region 数据长度
        /// <summary>
        /// 报文长度 高位
        /// </summary>
        public byte L0 { get; set; }
        /// <summary>
        /// 报文长度 低位
        /// </summary>
        public byte L1 { get; set; }
        #endregion

        /// <summary>
        /// 报文起始符
        /// </summary>
        public byte CStart { get; set; }

        /// <summary>
        /// 流水号
        /// </summary>
        public byte[] SNum { get; set; }

        /// <summary>
        /// 发报时间
        /// </summary>
        public byte[] dt { get; set; }

        /// <summary>
        /// 地址标识符
        /// </summary>
        public byte[] AddrFlag { get; set; }

        /// <summary>
        /// 遥测站分类码 1
        /// </summary>
        public byte Classific { get; set; }

        /// <summary>
        /// 观测时间标识符 2
        /// </summary>
        public byte[] ObservationFlag { get; set; }

        /// <summary>
        /// 观测时间 5
        /// </summary>
        public byte[] ObservationTime { get; set; }

        #region 数据域(要素参数信息)
        private byte[] data;
        /// <summary>
        /// 数据域
        /// </summary>
        public byte[] Data
        {
            get { return data; }
            set { data = value; }
        }
        #endregion

        #region 帧信息校验码
        /// <summary>
        /// 帧信息校验码  2
        /// </summary>
        public byte[] CS { get; set; }

        #endregion

        /// <summary>
        /// 结束码（0x03H,0x17H;05H,06H,04H,1BH） 1
        /// </summary>
        public byte End { get; set; }

        #endregion

        #region 扩展属性/方法
        /// <summary>
        /// 当前帧数据域 数据长度 十进制
        /// </summary>
        public int DataLength
        {
            get
            {
                return BitConverter.ToInt16(new byte[] { L0, L1 }, 0);
            }
            set
            {
                byte[] temp = BitConverter.GetBytes(value);
                L0 = temp[0];
                L1 = temp[1];
            }
        }

        /// <summary>
        /// 帧长度(下行帧)
        /// </summary>
        public int PackageLength
        {
            get
            {
                return 32 + DataLength;
            }
        }

        /// <summary>
        /// 转化为Byte[]数组(下行帧)
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray(bool CalcCS = false)
        {
            List<byte> lstByte = new List<byte>();
            lstByte.AddRange(this.Start);
            lstByte.Add(this.A5);
            lstByte.Add(this.A4);
            lstByte.Add(this.A3);
            lstByte.Add(this.A2);
            lstByte.Add(this.A1);
            lstByte.Add(this.CenterAddr);
            lstByte.Add(this.PWD[1]);
            lstByte.Add(this.PWD[0]);
            lstByte.Add(this.FUNCODE);
            if (this.IsUpload)
                lstByte.Add((byte)(this.L1 & 0x0F));
            else
                lstByte.Add((byte)(this.L1 | 0x80));

            lstByte.Add(this.L0);
            lstByte.Add(this.CStart);
            lstByte.Add(this.SNum[1]);
            lstByte.Add(this.SNum[0]);
            lstByte.AddRange(this.dt);
            lstByte.Add(this.AddrFlag[0]);
            lstByte.Add(this.AddrFlag[1]);
            lstByte.Add(this.A5);
            lstByte.Add(this.A4);
            lstByte.Add(this.A3);
            lstByte.Add(this.A2);
            lstByte.Add(this.A1);
            if (this.data != null)
                lstByte.AddRange(this.data);
            lstByte.Add(this.End);

            if (!CalcCS && this.CS != null)
                lstByte.AddRange(this.CS);
            return lstByte.ToArray();
        }

        /// <summary>
        /// 响应帧(无遥测站地址:地址标识符2+遥测站地址5 )
        /// </summary>
        /// <returns></returns>
        public byte[] ToResponseArray(bool CalcCS = false)
        {
            List<byte> lstByte = new List<byte>();
            lstByte.AddRange(this.Start);
            lstByte.Add(this.A5);
            lstByte.Add(this.A4);
            lstByte.Add(this.A3);
            lstByte.Add(this.A2);
            lstByte.Add(this.A1);
            lstByte.Add(this.CenterAddr);
            lstByte.Add(this.PWD[0]);
            lstByte.Add(this.PWD[1]);
            lstByte.Add(this.FUNCODE);
            if (this.IsUpload)
                lstByte.Add((byte)(this.L1 & 0x0F));
            else
                lstByte.Add((byte)(this.L1 | 0x80));

            lstByte.Add(this.L0);
            lstByte.Add(this.CStart);
            lstByte.Add(this.SNum[1]);
            lstByte.Add(this.SNum[0]);
            lstByte.AddRange(this.dt);
            if (this.data != null)
                lstByte.AddRange(this.data);
            lstByte.Add(this.End);

            if (!CalcCS && this.CS != null)
                lstByte.AddRange(this.CS);
            return lstByte.ToArray();
        }

        /// <summary>
        /// 比较两个Package651是否值相等
        /// </summary>
        /// <param name="pack"></param>
        /// <returns></returns>
        public bool Equals(Package651 pack)
        {
            if (this.A5 != pack.A5)
                return false;
            if (this.A4 != pack.A4)
                return false;
            if (this.A3 != pack.A3)
                return false;
            if (this.A2 != pack.A2)
                return false;
            if (this.A1 != pack.A1)
                return false;
            if (this.CenterAddr != pack.CenterAddr)
                return false;
            if (this.PWD[0] != pack.PWD[0])
                return false;
            if (this.PWD[1] != pack.PWD[1])
                return false;
            if (this.FUNCODE != pack.FUNCODE)
                return false;
            if (this.SNum[0] != pack.SNum[0])
                return false;
            if (this.SNum[1] != pack.SNum[1])
                return false;
            if (!PasswordEquals(this.data, pack.data))
                return false;

            return true;
        }

        /// 比较两个字节数组是否相等
        /// </summary>
        /// <param name="b1">byte数组1</param>
        /// <param name="b2">byte数组2</param>
        /// <returns>是否相等</returns>
        private bool PasswordEquals(byte[] b1, byte[] b2)
        {
            if (b1.Length != b2.Length) return false;
            if (b1 == null || b2 == null) return false;
            for (int i = 0; i < b1.Length; i++)
                if (b1[i] != b2[i])
                    return false;
            return true;
        }

        /// <summary>
        /// 转化为16字节字符串数组
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ConvertHelper.ByteArrayToHexString(ToArray());
        }

        /// <summary>
        /// 生成校验码
        /// </summary>
        /// <returns></returns>
        public byte[] CreateCS()
        {
            return Package651.CreateCS(this);
        }
        #endregion

        #region 静态方法

        #region 重载运算符
        /// <summary>
        /// 重载==号
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator ==(Package651 x, Package651 y)
        {
            return x.ToString() == y.ToString();
        }
        /// <summary>
        /// 重载!=号
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator !=(Package651 x, Package651 y)
        {
            return x.ToString() != y.ToString();
        }

        #endregion

        /// <summary>
        /// 检查帧完整性
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public static bool CheckPackage(Package651 package, out string err)
        {
            err = "";
            if (package.PackageLength < 12)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///  获得校验码
        /// </summary>
        /// <param name="package">帧</param>
        /// <returns></returns>
        public static byte[] CreateCS(Package651 package)
        {
            byte[] code;

            int sum = PackageDefine.BeginByte651[0] + PackageDefine.BeginByte651[1];
            sum = sum + package.CenterAddr;
            sum = sum + package.A1 + package.A2 + package.A3 + package.A4 + package.A5;
            sum = sum + package.PWD[0] + package.PWD[1];
            sum = sum + package.FUNCODE;
            sum = sum + (package.IsUpload ? 0x80 : 0x00) + package.L0 + package.L1;
            sum = sum + package.CStart;
            sum = sum + package.SNum[0] + package.SNum[1];
            sum = sum + package.dt[0] + package.dt[1] + package.dt[2] + package.dt[3] + package.dt[4];
            sum = sum + package.AddrFlag[0] + package.AddrFlag[1];
            sum = sum + package.A1 + package.A2 + package.A3 + package.A4 + package.A5;
            if (package.Classific != null)
                sum = sum + package.Classific;
            if (package.ObservationFlag != null)
                sum = sum + package.ObservationFlag[0] + package.ObservationFlag[1];
            if (package.ObservationTime != null)
                sum = sum + package.ObservationTime[0] + package.ObservationTime[1] + package.ObservationTime[2] + package.ObservationTime[3] + package.ObservationTime[4];

            if (package.data != null)
            {
                foreach (var item in package.data)
                {
                    sum = sum + item;
                }
            }

            code = BitConverter.GetBytes((short)sum);
            return code;
        }

        public static byte[] crc16(byte[] data, int datalen)
        {
            if (data.Length == 0)
                throw new Exception("调用CRC16校验算法,（低字节在前，高字节在后）时发生异常，异常信息：被校验的数组长度为0。");
            byte[] temdata = new byte[data.Length + 2];
            int xda, xdapoly;
            int i, j, xdabit;
            xda = 0xFFFF;
            xdapoly = 0xA001;
            for (i = 0; i < datalen; i++)
            {
                xda ^= data[i];
                for (j = 0; j < 8; j++)
                {
                    xdabit = (int)(xda & 0x01);
                    xda >>= 1;
                    if (xdabit == 1)
                        xda ^= xdapoly;
                }
            }
            temdata = new byte[2] { (byte)(xda >> 8), (byte)(xda & 0xFF) };
            return temdata;
        }

        /// <summary>
        /// 通过指定的byte[]数组创建帧
        /// </summary>
        /// <param name="bytes">byte[]数组</param>
        /// <param name="result">传出结果</param>
        /// <returns>是否成功</returns>
        public static bool TryParse(byte[] bytes, out Package651 result, out bool havesubsequent, out string subsequentmsg)
        {
            havesubsequent = false;
            subsequentmsg = "";
            try
            {
                result = FromBytes(bytes, out havesubsequent, out subsequentmsg);
                return true;
            }
            catch (ArgumentException argex)
            {
                throw argex;
            }
            catch (Exception)
            {
                result = default(Package651);
                return false;
            }
        }

        /// <summary>
        /// 通过指定的Byte[]数组创建帧
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Package651 FromBytes(byte[] bytes, out bool havesubsequent, out string subsequentmsg)
        {
            havesubsequent = false;
            subsequentmsg = "";
            if (bytes == null || bytes.Length == 0)
            {
                throw new ArgumentException("数据为空");
            }
            Package651 package = new Package651();
            if (bytes.Length < PackageDefine.MinLenth651)
            {
                throw new ArgumentException("数据帧长度低于最短帧");
            }
            if (bytes[0] != PackageDefine.BeginByte651[0] && bytes[1] != PackageDefine.BeginByte651[1])
            {
                throw new ArgumentException("未能在开始位找到帧头");
            }

            package.CenterAddr = bytes[2];
            package.A5 = bytes[3];
            package.A4 = bytes[4];
            package.A3 = bytes[5];
            package.A2 = bytes[6];
            package.A1 = bytes[7];

            package.PWD = new byte[2];
            package.PWD[1] = bytes[8];
            package.PWD[0] = bytes[9];
            package.FUNCODE = bytes[10];
            package.IsUpload = (Convert.ToInt16(bytes[11] >> 4) == 8) ? false : true;
            package.L1 = (byte)(bytes[11] & (byte)0x0f);
            package.L0 = bytes[12];

            package.CStart = bytes[13];

            package.SNum = new byte[2];

            package.dt = new byte[6];

            if (package.DataLength > 8)
            {
                if (package.CStart == PackageDefine.CStart)
                {
                    package.SNum[1] = bytes[14];
                    package.SNum[0] = bytes[15];
                    if (bytes.Length - PackageDefine.MinLenth651 != (package.DataLength - 8))
                    {
                        throw new ArgumentException("数据长度和数据不匹配");
                    }
                    package.dt[0] = bytes[16];
                    package.dt[1] = bytes[17];
                    package.dt[2] = bytes[18];
                    package.dt[3] = bytes[19];
                    package.dt[4] = bytes[20];
                    package.dt[5] = bytes[21];
                    package.data = new byte[package.DataLength - 8];
                    for (int i = 0; i < (package.DataLength - 8); i++)
                    {
                        package.data[i] = bytes[22 + i];
                    }
                }
                else if (package.CStart == PackageDefine.CStart_Pack)  //多包发送
                {
                    if (bytes.Length - PackageDefine.MinLenth_Pack != package.DataLength)
                    {
                        throw new ArgumentException("数据长度和数据不匹配");
                    }
                    package.dt[0] = bytes[19];
                    package.dt[1] = bytes[20];
                    package.dt[2] = bytes[21];
                    package.dt[3] = bytes[22];
                    package.dt[4] = bytes[23];
                    package.dt[5] = bytes[24];
                    byte b = (byte)((bytes[14] << 4) | (bytes[15] >> 4));
                    int pack_l1 = BitConverter.ToInt16(new byte[] { b, (byte)(bytes[14] >> 4) }, 0);
                    int pack_index = BitConverter.ToInt16(new byte[] { bytes[16], (byte)(bytes[15] & 0x0F) }, 0);
                    if (pack_l1 != pack_index)
                    {
                        havesubsequent = true;
                    }
                    subsequentmsg = "总包数:" + pack_l1 + "、当前第" + pack_index + "包";
                    package.data = new byte[package.DataLength];
                    for (int i = 0; i < (package.DataLength); i++)
                    {
                        package.data[i] = bytes[17 + i];
                    }
                }
            }

            package.CS = new byte[2];//校验码
            package.CS[0] = bytes[bytes.Length - 2];
            package.CS[1] = bytes[bytes.Length - 1];

            if ((bytes[bytes.Length - 3] != PackageDefine.EndByte651) && (bytes[bytes.Length - 3] != PackageDefine.EndByte_Continue))
            {
                throw new ArgumentException("数据帧不完整或已损坏");
            }

            byte[] crc_result = crc16(bytes, bytes.Length - 2);

            if (crc_result == package.CS)
            {
                throw new ArgumentException("数据帧校验失败");
            }

            return package;
        }

        #endregion

    }
}
