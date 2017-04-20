using System.Collections.Generic;

namespace Entity
{
    public class TerMagInfoEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public int DevId { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        public ConstValue.DEV_TYPE DevType { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Addr { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Lng { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Lat { get; set; }
        /// <summary>
        /// 图片id
        /// </summary>
        public List<string> PicId { get; set; }
    }
}
