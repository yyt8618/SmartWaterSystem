using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    [Serializable]
    public struct Package
    {
        #region 原始帧

        /// <summary>
        /// 开始码
        /// </summary>
        public byte Start
        {
            get { return PackageDefine.BeginByte; }
        }

        #region 设备编码

        /// <summary>
        /// 终端ID 设备ID高位
        /// </summary>
        public byte ID3 { get; set; }
        /// <summary>
        /// 终端ID 设备ID低位
        /// </summary>
        public byte ID2 { get; set; }
        /// <summary>
        /// 流水号 高位
        /// </summary>
        public byte ID1 { get; set; }
        /// <summary>
        /// 流水号低位
        /// </summary>
        public byte ID0 { get; set; }

        #endregion

        #region 帧起始符
        public byte BeginFrame
        {
            get { return PackageDefine.BeginFrame; }
        }
        #endregion


        #region 控制码

        /// <summary>
        /// 控制码高位
        /// </summary>
        public byte C0 { get; set; }
        /// <summary>
        /// 控制码低位 功能码
        /// </summary>
        public byte C1 { get; set; }

        #endregion

        #region 数据长度
        /// <summary>
        /// 数据长度 高位
        /// </summary>
        public byte L0 { get; set; }
        /// <summary>
        /// 数据长度 低位
        /// </summary>
        public byte L1 { get; set; }
        #endregion

        #region 数据域
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
        /// 帧信息校验码 
        /// </summary>
        public byte CS { get; set; }

        #endregion

        /// <summary>
        /// 结束码
        /// </summary>
        public byte End
        {
            get { return PackageDefine.EndByte; }
        }

        #endregion

        #region 扩展属性/方法

        /// <summary>
        /// 设备类型
        /// </summary>
        public Entity.ConstValue.DEV_TYPE DevType
        {
            get
            {
                return (Entity.ConstValue.DEV_TYPE)ID3;
            }
            set
            {
                ID3 = (byte)value;
            }
        }

        /// <summary>
        /// 命令类型
        /// </summary>
        public CTRL_COMMAND_TYPE CommandType
        {
            get
            {
                return (CTRL_COMMAND_TYPE)(C0 >> 4);
            }
            set
            {
                string c0 = Convert.ToString(C0, 2).PadLeft(8, '0');
                string svalue = Convert.ToString((byte)value, 2).PadLeft(4, '0').Substring(0, 4) + c0.Substring(4, 4);
                C0 = Convert.ToByte(svalue, 2);
            }
        }

        /// <summary>
        /// 设备编号(无类型标识)
        /// </summary>
        public short DevID
        {
            get
            {
                return BitConverter.ToInt16(new byte[] { ID0, ID1 }, 0);
            }
            set
            {
                string str = Convert.ToString(value, 16).PadLeft(4, '0');
                if (str == null || str.Length == 0 || str.Length > 4)
                {
                    throw new Exception("长度溢出");
                }
                byte[] temp = ConvertHelper.HexStringToByteArray(str);
                ID1 = temp[0];
                ID0 = temp[1];
            }
        }

        /// <summary>
        /// 终端ID 十进制  设备类型+流水编号
        /// </summary>
        public int ID
        {
            //get
            //{
            //    return BitConverter.ToInt32(new byte[] { ID0, ID1, ID2, ID3 }, 0);
            //}
            //set
            //{
            //    string str = Convert.ToString(value, 16).PadLeft(8, '0');
            //    if (str == null || str.Length == 0 || str.Length > 8)
            //    {
            //        throw new Exception("ID长度溢出");
            //    }
            //    byte[] temp = ConvertHelper.HexStringToByteArray(str);
            //    ID3 = temp[0];
            //    ID2 = temp[1];
            //    ID1 = temp[2];
            //    ID0 = temp[3];
            //}
            get
            {
                int id = BitConverter.ToInt32(new byte[] { ID0, ID1, ID2, 0x00 }, 0);
                //类型作为id高位  这样是为了不同类型的终端都可以上传相同类型的数据(如:压力终端和水质终端都可以上传流量数据)的情况下，加入设备类型作为id区分
                //设备类型不直接转换而是单独乘100000是为了将ID值减小并且容易看懂
                id += Convert.ToInt32(ID3) * 100000;  
                return id;
            }
        }


        /// <summary>
        /// 当前帧数据域 数据长度 十进制
        /// </summary>
        public int DataLength
        {
            get
            {
                return BitConverter.ToInt16(new byte[] { L1, L0 }, 0);
            }
            set
            {
                string str = Convert.ToString(value, 16).PadLeft(4, '0');
                if (str == null || str.Length == 0 || str.Length > 4)
                {
                    throw new Exception("数据域长度溢出");
                }
                byte[] temp = ConvertHelper.HexStringToByteArray(str);
                L0 = temp[0];
                L1 = temp[1];
            }
        }


        #region 数据域扩展属性(分多帧发送/接收数据)

        /// <summary>
        /// 所有帧 总数据长度
        /// </summary>
        public int AllDataLength
        {
            get
            {
                if (data.Length < 3)
                {
                    return -1;
                }
                return BitConverter.ToInt16(new byte[] { data[1], data[0] }, 0);
            }
        }

        /// <summary>
        /// 当前数据帧 帧号 由1开始
        /// </summary>
        public int DataNum
        {
            get
            {
                if (data.Length < 3)
                {
                    return -1;
                }
                return data[2];
            }
        }
        #endregion

        /// <summary>
        /// 帧长度
        /// </summary>
        public int PackageLength
        {
            get
            {
                return 12 + DataLength;
            }
        }

        /// <summary>
        /// 是否收到从站应答0x10
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                string c0 = Convert.ToString(C0, 16);
                return c0[0] == '1';
            }
        }

        /// <summary>
        /// 是否最后帧
        /// </summary>
        /// <returns></returns>
        public bool IsFinal
        {
            get
            {
                string str = Convert.ToString(C0, 2).PadLeft(8, '0');
                return str[4] == '0';
            }
        }

        /// <summary>
        /// 转化为Byte[]数组
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            int len = PackageLength;
            byte[] data = new byte[len];
            data[0] = this.Start;
            data[1] = ID3;
            data[2] = ID2;
            data[3] = ID1;
            data[4] = ID0;
            data[5] = this.Start;
            data[6] = C0;
            data[7] = C1;
            data[8] = L0;
            data[9] = L1;
            for (int i = 10; i < len - 2; i++)
            {
                data[i] = this.Data[i - 10];
            }
            data[len - 2] = this.CS;
            data[len - 1] = this.End;
            return data;
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
        public byte CreateCS()
        {
            return Package.CreateCS(this);
        }

        #endregion

        #region 静态方法

        #region 重载运算符
        /// <summary>
        /// 重载+号
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Package operator +(Package x, Package y)
        {
            return Combine(x, y);
        }

        /// <summary>
        /// 重载==号
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator ==(Package x, Package y)
        {
            return x.ToString() == y.ToString();
        }
        /// <summary>
        /// 重载!=号
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator !=(Package x, Package y)
        {
            return x.ToString() != y.ToString();
        }

        #endregion

        /// <summary>
        /// 检查帧完整性
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public static bool CheckPackage(Package package, out string err)
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
        public static byte CreateCS(Package package)
        {

            byte code = 0x0;

            //int sum = 0;
            //sum = sum + PackageDefine.BeginByte;
            int sum = PackageDefine.BeginByte;
            sum = sum + package.ID3 + package.ID2 + package.ID1 + package.ID0;
            sum = sum + PackageDefine.BeginFrame;
            //sum = sum + PackageDefine.BeginByte;
            sum = sum + package.C0 + package.C1;
            sum = sum + package.L0 + package.L1;
            if (package.data != null)
            {
                foreach (var item in package.data)
                {
                    sum = sum + item;
                }
            }

            code = (byte)sum;
            return code;
        }

        public static bool IsEndByte(byte[] bytes, out string error)
        {
            error = "";
            if (bytes == null || bytes.Length == 0)
            {
                error = "参数不正确";
            }
            Package package = new Package();
            if (bytes == null || bytes.Length < 12)
            {
                error = "数据帧不完整";
            }

            int start = 0;

            for (int i = 0; i < bytes.Length - 5; i++)
            {
                if (bytes[start] == PackageDefine.BeginByte && bytes[start + 5] == PackageDefine.BeginByte)
                {
                    start = i;
                    break;
                }
            }

            if (bytes[start + 0] != PackageDefine.BeginByte)
            {
                error = "未找到帧头";
            }

            package.ID3 = bytes[start + 1];
            package.ID2 = bytes[start + 2];
            package.ID1 = bytes[start + 3];
            package.ID0 = bytes[start + 4];
            //package.DevID = id3 * 16 * 16 * 16 + id2 * 16 * 16 + id1 * 16 + id0;

            if (bytes[start + 5] != PackageDefine.BeginByte)
            {
                error = "数据帧不完整或已损坏";
            }

            //控制码
            package.C0 = bytes[start + 6];//控制码
            package.C1 = bytes[start + 7];//功能码

            //数据长度
            package.L0 = bytes[start + 8];
            package.L1 = bytes[start + 9];

            package.data = new byte[package.DataLength];
            if (bytes.Length - PackageDefine.MinLenth != package.DataLength)
            {
                error = "数据长度和数据不匹配";
            }
            for (int i = 0; i < package.DataLength; i++)
            {
                package.data[i] = bytes[start + 10 + i];
            }

            package.CS = bytes[bytes.Length - 2];//校验码

            if (bytes[bytes.Length - 1] != PackageDefine.EndByte)
            {
                error = "数据帧不完整或已损坏";
            }

            if (Package.CreateCS(package) != package.CS)
            {
                error = "数据帧校验失败";
            }

            return error == "";
        }

        /// <summary>
        /// 通过指定的十六进制字符串创建帧
        /// </summary>
        /// <param name="bytes">十六进制字符串 如 '16 1a 23 12 b1' </param>
        /// <param name="result">传出结果</param>
        /// <returns>是否成功</returns>
        public static bool TryParse(string hexString, out Package result)
        {
            try
            {
                return TryParse(ConvertHelper.HexStringToByteArray(hexString), out result);
            }
            catch (ArgumentException argex)
            {
                throw argex;
            }
            catch (Exception)
            {
                result = default(Package);
                return false;
            }
        }
        
        /// <summary>
        /// 通过指定的byte[]数组创建帧
        /// </summary>
        /// <param name="bytes">byte[]数组</param>
        /// <param name="result">传出结果</param>
        /// <returns>是否成功</returns>
        public static bool TryParse(byte[] bytes, out Package result)
        {
            try
            {
                result = FromBytes(bytes);
                return true;
            }
            catch (ArgumentException argex)
            {
                throw argex;
            }
            catch (Exception)
            {
                result = default(Package);
                return false;
            }
        }


        /// <summary>
        /// 通过指定的Byte[]数组创建帧
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Package FromBytes(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                throw new ArgumentException("数据为空");
            }
            Package package = new Package();
            if (bytes.Length < 12)
            {
                throw new ArgumentException("数据帧长度低于最短帧");
            }
            if (bytes[0] != PackageDefine.BeginByte)
            {
                throw new ArgumentException("未能在开始位找到帧头");
            }

            package.ID3 = bytes[1];
            package.ID2 = bytes[2];
            package.ID1 = bytes[3];
            package.ID0 = bytes[4];
            //package.DevID = id3 * 16 * 16 * 16 + id2 * 16 * 16 + id1 * 16 + id0;

            if (bytes[5] != PackageDefine.BeginByte)
            {
                throw new ArgumentException("数据帧不完整或已损坏");
            }


            //控制码
            package.C0 = bytes[6];//控制码
            package.C1 = bytes[7];//功能码

            //数据长度
            package.L0 = bytes[8];
            package.L1 = bytes[9];

            package.data = new byte[package.DataLength];
            if (bytes.Length - PackageDefine.MinLenth != package.DataLength)
            {
                throw new ArgumentException("数据长度和数据不匹配");
            }
            for (int i = 0; i < package.DataLength; i++)
            {
                package.data[i] = bytes[10 + i];
            }

            package.CS = bytes[bytes.Length - 2];//校验码


            if (bytes[bytes.Length - 1] != PackageDefine.EndByte)
            {
                throw new ArgumentException("数据帧不完整或已损坏");
            }

            //娄底终端问题，报警帧校验和错误，去掉检验,2016/11/29
            //if (Package.CreateCS(package) != package.CS)
            //{
            //    throw new ArgumentException("数据帧校验失败");
            //}

            return package;
        }

        /// <summary>
        /// 合并多个帧
        /// </summary>
        /// <param name="packages"></param>
        /// <returns></returns>
        public static Package Combine(params Package[] packages)
        {
            if (packages.Length == 0)
            {
                throw new ArgumentNullException("参数错误");
            }

            List<Package> list = packages.ToList();
            Package package = packages[0];
            List<byte> t = new List<byte>();

            if (list.Exists(obj => !obj.IsFinal))//数据帧分多帧情况
            {
                var q = from p in list
                        orderby p.DataNum
                        select p;
                int num = 1;
                foreach (var item in q)
                {
                    if (item.DataNum != num)
                    {
                        throw new Exception(string.Format("缺少第{0}帧", num));
                    }
                    for (int i = 3; i < item.DataLength; i++)
                    {
                        t.Add(item.Data[i]);
                    }
                    num++;
                }
                if (!list.Exists(obj => obj.IsFinal))
                {
                    throw new Exception("缺少结束帧");
                }


            }
            else//普通帧
            {
                foreach (var item in packages)
                {
                    if (!item.IsFinal)
                    {
                        foreach (var subItem in item.Data)
                        {
                            t.Add(subItem);
                        }
                    }
                }
            }
            package.data = t.ToArray();
            package.CS = package.CreateCS();
            return package;
        }

        #endregion

    }
}
