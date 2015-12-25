using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class HTTPEntity
    {
        //{params:body,timestamp:时间戳,digest："(MD5 (body +时间戳+Key) "}
        public string Params {get;set;}
        public string timestamp="";
        public string digest="";
    }
}
