using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class HTTPRespEntity
    {
        public HttpRespCode code = HttpRespCode.Fail;
        public string msg = "";
        public string data = "";
    }
}
