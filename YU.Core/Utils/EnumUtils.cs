using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace YU.Core.Utils
{
    /// <summary>
    /// 枚举帮助类
    /// </summary>
    public static class EnumUtils
    {
        /// <summary>
        /// 获取枚举描述特性值
        /// </summary>
        /// <param name="_enum">枚举值</param>
        /// <returns>枚举值的描述</returns>
        private static string GetEnumDesc(this Object _enum)
        {
            Type type = _enum.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue必须是一个枚举值", "_enum");
            }

            //使用反射获取该枚举的成员信息
            MemberInfo[] memberInfo = type.GetMember(_enum.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //返回枚举值得描述信息
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //如果没有描述特性的值，返回该枚举值得字符串形式
            return _enum.ToString();
        }

        /// <summary>
        /// 通过Key获取枚举描述特性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescByKey<T>(int key)
        {
            string value = GetValueByKey<T>(key);
            return GetEnumDescByValue<T>(value);
        }

        /// <summary>
        /// 通过Key得到Value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValueByKey<T>(int key)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T必须是一个枚举值", typeof(T).ToString());
            try
            {
                T obj = (T)Enum.Parse(typeof(T), key.ToString());
                var isEnum = Enum.IsDefined(typeof(T), obj) | obj.ToString().Contains(",");
                if (isEnum)
                {
                    return obj.ToString();
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static int GetKeyByValue<T>(string value)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T必须是一个枚举值", typeof(T).ToString());
            try
            {
                object obj = Enum.Parse(typeof(T), value);
                var isEnum = Enum.IsDefined(typeof(T), obj) | obj.ToString().Contains(",");
                if (isEnum)
                {
                    return (int)obj;
                }
                return -1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 通过Value获取枚举描述特性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescByValue<T>(string value)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T必须是一个枚举值", typeof(T).ToString());
            try
            {
                object obj = Enum.Parse(typeof(T), value);
                return GetEnumDesc(obj);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取枚举所有keyValue值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<string, string> GetEnumAllKeyValues<T>()
        {
            Type tType = typeof(T);
            if (!tType.IsEnum)
                throw new ArgumentException("EnumerationValue必须是一个枚举值", typeof(T).ToString());
            var result = new Dictionary<string, string>();
            string[] arr = Enum.GetNames(tType);
            for (int i = 0; i < arr.Length; i++)
            {
                result[Enum.Format(tType, Enum.Parse(tType, arr[i]), "d")] = arr[i];
            }
            return result;
        }

        /// <summary>
        /// 获取枚举所有keyDesc值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<int, string> GetEnumAllKeyDescs<T>()
        {
            Type tType = typeof(T);
            if (!tType.IsEnum)
                throw new ArgumentException("EnumerationValue必须是一个枚举值", typeof(T).ToString());


            var result = new Dictionary<int, string>();
            string[] arr = Enum.GetNames(tType);
            for (int i = 0; i < arr.Length; i++)
            {
                result[Enum.Format(tType, Enum.Parse(tType, arr[i]), "d").TryPareValue<int>()] = GetEnumDescByValue<T>(arr[i]);
            }
            return result;
        }

        /// <summary>
        /// 获取枚举所有ValueDesc值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<string, string> GetEnumAllValueDescs<T>()
        {
            Type tType = typeof(T);
            if (!tType.IsEnum)
                throw new ArgumentException("EnumerationValue必须是一个枚举值", typeof(T).ToString());


            var result = new Dictionary<string, string>();
            string[] arr = Enum.GetNames(tType);
            for (int i = 0; i < arr.Length; i++)
            {
                result[arr[i]] = GetEnumDescByValue<T>(arr[i]);
            }
            return result;
        }


        /// <summary>
        /// 是否包含Key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsContainKey<T>(int key)
        {
            object obj = Enum.Parse(typeof(T), key.ToString());
            var isEnum = Enum.IsDefined(typeof(T), obj) | obj.ToString().Contains(",");
            return isEnum;
        }

        /// <summary>
        /// 是否包含Key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsContainValue<T>(string value)
        {
            var key = GetKeyByValue<T>(value);
            return IsContainKey<T>(key);
        }

        /// <summary>
        /// 是否包含Key或Value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsContain<T1, T2>(T2 value)
        {
            Type t = typeof(T2);
            if (typeof(string) == t)
            {
                return IsContainValue<T1>(value as string);
            }
            else if (typeof(Int32) == t)
            {
                return IsContainKey<T1>(int.Parse(value.ToString()));
            }
            return false;
        }
    }

}
