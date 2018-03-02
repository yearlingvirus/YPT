using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static YU.Core.YUEnums;

namespace YU.Core.Utils
{
    public static class ObjectUtils
    {

        /// <summary>
        /// 为对象创建一个拷贝
        /// </summary>
        /// <param name="obj">需要创建拷贝的对象</param>
        /// <returns>新建立的拷贝对象</returns>
        public static T CreateCopy<T>(object obj)
            where T : class
        {
            return  (T)ObjectUtils.CreateCopy(obj, YUEnums.FormatterType.BinaryFormatter);
        }

        /// <summary>
        /// 为对象创建一个拷贝
        /// </summary>
        /// <param name="obj">需要创建拷贝的对象</param>
        /// <param name="formatterType">格式类型</param>
        /// <returns>新建立的拷贝对象</returns>
        public static object CreateCopy(object obj, YUEnums.FormatterType formatterType)
        {
            if (obj == null)
            {
                return null;
            }
            object ret = null;
            using (MemoryStream stmSeriBuff = new MemoryStream())
            {
                IFormatter formatter = null;

                if (formatterType == FormatterType.NetDataContract)
                {
                    formatter = new NetDataContractSerializer();
                }
                else
                {
                    formatter = new BinaryFormatter();
                }
                //序列化.
                formatter.Serialize(stmSeriBuff, obj);
                stmSeriBuff.Position = 0;
                ret = formatter.Deserialize(stmSeriBuff);
                stmSeriBuff.Close();
                stmSeriBuff.Dispose();
                formatter = null;
            }
            return ret;
        }

        /// <summary>
        /// 将对象序列化为字节流
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns>序列化后的字节流</returns>
        public static byte[] GetObjectSream(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            byte[] ret = null;
            using (MemoryStream stmSeriBuff = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();

                //序列化.
                formatter.Serialize(stmSeriBuff, obj);
                ret = stmSeriBuff.ToArray();
                stmSeriBuff.Close();
                stmSeriBuff.Dispose();
            }
            return ret;
        }

        /// <summary>
        /// 将字节流反序列化为对象
        /// </summary>
        /// <param name="bytes">需要反序列化的字节流</param>
        /// <returns>反序列化后的对象</returns>
        public static object GetObject(byte[] bytes)
        {
            if (bytes == null)
            {
                return null;
            }
            object ret = null;
            using (MemoryStream stmSeriBuff = new MemoryStream(bytes))
            {
                IFormatter formatter = new BinaryFormatter();
                ret = formatter.Deserialize(stmSeriBuff);
                stmSeriBuff.Close();
                stmSeriBuff.Dispose();
            }
            return ret;
        }

        public static string Serializer(Type type, object obj)
        {
            string str = string.Empty;
            using (MemoryStream Stream = new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(type);
                xml.Serialize(Stream, obj);
                Stream.Position = 0;
                StreamReader sr = new StreamReader(Stream);
                str = sr.ReadToEnd();
            }
            return str;
        }

        public static object Deserialize(Type type, string xml)
        {
            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer xmldes = new XmlSerializer(type);
                return xmldes.Deserialize(sr);
            }
        }

        /// <summary>
        /// 通过反射浅复制
        /// </summary>
        public static object CloneByReflection(object obj)
        {
            object result = obj;
            if (obj != null)
            {
                Type type = obj.GetType();
                result = type.Assembly.CreateInstance(type.FullName);
                var properties = type.GetProperties();
                foreach (var property in properties)
                {
                    if (property.CanRead && property.CanWrite)
                    {
                        var pValue = property.GetValue(obj, null);
                        property.SetValue(result, pValue, null);
                    }
                }
            }

            return result;
        }
    }


}
