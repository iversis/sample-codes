using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace D365.Shared
{
    public static class JsonSerializer
    {
        public static string SerialiseToJson<T>(T data)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                //write the object into a memory stream
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                ser.WriteObject(stream, data);

                //convert to string
                byte[] raw = stream.ToArray();
                var json = Encoding.UTF8.GetString(raw, 0, raw.Length);

                return json;
            }
        }

        public static T DeserialiseFromJson<T>(string json)
        {
            try
            {
                //convert the string to bytes
                byte[] byteArray = Encoding.UTF8.GetBytes(json);
                using (MemoryStream stream = new MemoryStream(byteArray))
                {
                    //read the bytes and convert to object.
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                    stream.Position = 0;

                    return (T)serializer.ReadObject(stream);
                }
            }
            catch
            {
                return default;
            }
        }
    }
}
