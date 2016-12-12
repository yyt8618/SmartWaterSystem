
using System;
namespace Entity
{
    [Serializable]
    public class CallDataTypeEntity
    {
        public bool GetPre = false;         //招测压力1
        public bool GetSim1 = false;        //招测模拟量一路
        public bool GetSim2 = false;        //招测模拟量二路
        public bool GetPluse = false;       //招测脉冲量
        public bool GetRS4851 = false;      //招测RS485 1路
        public bool GetRS4852 = false;      //招测RS485 2路
        public bool GetRS4853 = false;      //招测RS485 3路
        public bool GetRS4854 = false;      //招测RS485 4路
    }
}
