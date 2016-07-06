using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace SmartWaterSystem
{
    public class JSONSerialize
    {
        public static T JsonDeserialize<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }


        public static string JsonSerialize<T>(object obj)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();

            ser.WriteObject(ms, obj);
            ms.Position = 0;
            byte[] bs = new byte[1024];
            StringBuilder sb = new StringBuilder();
            int count = -1;
            while ((count = ms.Read(bs, 0, 1024)) > 0)
            {
                sb.Append(Encoding.UTF8.GetString(bs, 0, count));
            }

            ms.Flush();
            ms.Close();
            ms.Dispose();

            if (sb.Length > 0)
                return sb.ToString();
            else
                return "";
        }

        public static string JsonSerialize_Newtonsoft(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T JsonDeserialize_Newtonsoft<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

    }
}
