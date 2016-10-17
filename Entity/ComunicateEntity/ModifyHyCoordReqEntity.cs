using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class ModifyHyCoordReqEntity
    {
        public string action = "modifyhydrantcoord";

        private string _hydrantId;
        /// <summary>
        /// 消防栓编号
        /// </summary>
        public string HydrantID
        {
            get { return _hydrantId; }
            set { _hydrantId = value; }
        }

        private string _longtitude;
        /// <summary>
        /// 经度
        /// </summary>
        public string Longtitude
        {
            get { return _longtitude; }
            set { _longtitude = value; }
        }

        private string _latitude;
        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }
    }
}
