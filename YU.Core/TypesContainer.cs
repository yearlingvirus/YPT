using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YU.Core
{

    public class TypesContainer
    {
        private static ConcurrentDictionary<string, Type> typesDict = new ConcurrentDictionary<string, Type>();
        private static ConcurrentDictionary<Type, object> instancesDict = new ConcurrentDictionary<Type, object>();

      
        public static object GetOrRegisterSingletonInstance(string type)
        {
            var t = GetOrRegister(type);
            if (t != null)
                return instancesDict.GetOrAdd(t, Activator.CreateInstance(t));
            return null;
        }
       
        public static Type GetOrRegister(string type)
        {
            if (string.IsNullOrWhiteSpace(type)) return null;
            return typesDict.GetOrAdd(type, tp => Type.GetType(tp));
        }


        /// <summary>
        /// 创建实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="className"></param>
        /// <returns></returns>
        public static T CreateInstance<T>(string className, params object[] args)
        {
            Type type = TypesContainer.GetOrRegister(className);
            if (type == null)
            {
                throw new Exception(string.Format("{0} failed to  Create Instance.", className));
            }
            return (T)Activator.CreateInstance(type, args);
        }
    }
}
