using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace YU.Core.Utils
{
    public class YUUtils
    {
        /// <summary>
        /// ParseB
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static double ParseB(string sizeString)
        {
            sizeString = sizeString.Trim();
            if (sizeString.IsNullOrEmptyOrWhiteSpace())
                return 0;
            if (sizeString.Length <= 2)
                return 0;
            string[] units = new string[] { "KB", "MB", "GB", "TB", "PB" };
            for (int i = 0; i < units.Length; i++)
            {
                if (sizeString.Contains(units[i]))
                {
                    var index = sizeString.LastIndexOf(units[i]);
                    var size = sizeString.Substring(0, index).Trim().TryPareValue<double>();
                    if (size == 0)
                        return 0;
                    else
                        return size * Math.Pow(1024, i + 1);
                }
            }
            throw new InvalidCastException(sizeString + "类型转换失败");
        }

        /// <summary>
        /// 转换毫秒
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public static double ParseMilliSecond(string dateString)
        {
            //119天06:00:53
            //&nbsp;&nbsp;[做种时间: 852 天 19 小时 2 分钟 , 下载时间: 5 天 17 小时 7 分钟 ]
            //[Seed time: 853 d 16 h 32 m , Leech time: 5 d 17 h 7 m ]
            double dateValue = 0;
            if (dateString.IsNullOrEmptyOrWhiteSpace())
                return 0;
            dateString = dateString.Replace("天", "d");
            dateString = dateString.Replace("小时", "h");
            dateString = dateString.Replace("分钟", "m");
            dateString = dateString.Replace("days", "d");
            char[] spiltChars = new char[] { 'd', 'h', 'm', ':', ' ' };
            string[] arr = dateString.Split(spiltChars, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length >= 3)
            {
                dateValue += arr[0].TryPareValue<int>() * 24L * 60L * 60L * 1000L;
                dateValue += arr[1].TryPareValue<int>() * 60L * 60L * 1000L;
                dateValue += arr[2].TryPareValue<int>() * 60L * 1000L;
                if (arr.Length > 3)
                    dateValue += arr[3].TryPareValue<int>() * 1000L;
            }
            return dateValue;
        }

        /// <summary>  
        /// 转换方法  
        /// </summary>  
        /// <param name="size">字节值</param>  
        /// <returns></returns>  
        public static string ConvertBytes(double size)
        {
            string[] units = new string[] { "B", "KB", "MB", "GB", "TB", "PB" };
            double mod = 1024.0;
            int i = 0;
            while (size >= mod)
            {
                size /= mod;
                i++;
            }
            return Math.Round(size) + units[i];
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="container"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetCookieFromContainer(CookieContainer container,Uri uri)
        {
            List<string> cookieStr = new List<string>();
            CookieCollection cookies = container.GetCookies(uri);
            if (cookies != null && cookies.Count > 0)
            {
                foreach (Cookie item in cookies)
                {
                    cookieStr.Add(string.Format("{0}={1}", item.Name, item.Value));
                }
            }
            return string.Join("; ", cookieStr);
        }
    }
}
