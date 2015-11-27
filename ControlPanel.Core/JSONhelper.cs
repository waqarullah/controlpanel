using System;
using System.IO;
using System.Runtime.Serialization.Json;


namespace ControlPanel.Core
{
    public class JSONhelper
    {

        public static string GetString(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var ser = new DataContractJsonSerializer(obj.GetType());
                ser.WriteObject(ms, obj);
                ser = null;
                ms.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(ms))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        public static object GetObject(Stream stream, object obj)
        {
            var ser = new DataContractJsonSerializer(obj.GetType());
            obj = ser.ReadObject(stream);
            stream.Close();
            stream.Dispose();
            stream = null;
            ser = null;
            return obj;
        }

        public static R GetObject<R>(Stream stream)
        {
            var ser = new DataContractJsonSerializer(typeof(R));
            object obj = ser.ReadObject(stream);
            if (obj != null)
            {
                stream.Close();
                stream.Dispose();
                stream = null;
                ser = null;
                return (R)obj;
            }
            else
            {
                stream.Close();
                stream.Dispose();
                stream = null;
                ser = null;
                return default(R);
            }

        }

        public static object GetObject(string json, object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                StringReader sr = new StringReader(json);
                byte[] byteArray = System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(json);
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(byteArray);
                ms.Position = 0;
                var ser = new DataContractJsonSerializer(obj.GetType());
                obj = ser.ReadObject(memoryStream);
                return obj;
            }
        }

        public static T GetObject<T>(string json)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                StringReader sr = new StringReader(json);
                byte[] byteArray = System.Text.Encoding.GetEncoding("utf-8").GetBytes(json);
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(byteArray);
                ms.Position = 0;
                //stream.Position = 0;
                var ser = new DataContractJsonSerializer(typeof(T));
                T t = (T)ser.ReadObject(memoryStream);
                return t;
            }
        }

    }
}