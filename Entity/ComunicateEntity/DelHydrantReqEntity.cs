using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class DelHydrantReqEntity
    {
        public string action = "delhydrant";

        private string _hydrantId;
        /// <summary>
        /// 消防栓编号
        /// </summary>
        public string HydrantID
        {
            get { return _hydrantId; }
            set { _hydrantId = value; }
        }
    }
}
