using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using YU.Core.DataEntity;

namespace YU.Core.Utils
{
    /// <summary>
    /// 扩展类
    /// </summary>
    public static class ExtendUtil
    {
        /// <summary>
        /// 获取URL中的Search值
        /// </summary>
        /// <param name="pageURL"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public static string UrlSearchKey(this string pageURL, string searchKey)
        {
            Uri uri = new Uri(pageURL);
            string queryString = uri.Query;
            NameValueCollection col = UrlUtils.GetQueryString(queryString);
            if (col.AllKeys.Contains(searchKey))
                return searchKey = col[searchKey];
            else
                return string.Empty;
        }

        /// <summary>
        /// 指示指定的字符串是 null、空还是仅由空白字符组成。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmptyOrWhiteSpace(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 使用序号排序规则并忽略被比较字符串的大小写，对字符串进行比较。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool EqualIgnoreCase(this string str, string value)
        {
            return string.Equals(str, value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T TryPareValue<T>(this object o, T defaultValue = default(T))
        {
            if (o == null)
                return defaultValue;
            try
            {
                return (T)Convert.ChangeType(o, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 数值转换
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static int ParseNumber(this string o)
        {
            if (o == null)
                return 0;
            try
            {
                return int.Parse(Convert.ToString(o), NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取内部异常提示信息
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetInnerExceptionMessage(this Exception ex)
        {
            if (ex != null)
            {
                if (ex.InnerException != null)
                {
                    return GetInnerExceptionMessage(ex.InnerException);
                }
                else
                {
                    return ex.Message;
                }
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
