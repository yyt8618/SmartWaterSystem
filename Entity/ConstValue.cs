using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class ConstValue
    {
        public static DateTime MinDateTime = new DateTime(2015, 1, 1, 0, 0, 0);

        public static object obj = new object();

        public static string GetUniversalCollectTypeName(UniversalCollectType type){
            if (type == UniversalCollectType.Simulate)
                return "模拟";
            else if (type == UniversalCollectType.Pluse)
                return "脉冲";
            else
                return "RS485";
        }
    }
}
