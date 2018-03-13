using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using YU.Core.Log;

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
                if (sizeString.IndexOf(units[i], StringComparison.OrdinalIgnoreCase) > 0)
                {
                    var index = sizeString.LastIndexOf(units[i], StringComparison.OrdinalIgnoreCase);
                    var size = sizeString.Substring(0, index).Trim().TryPareValue<double>();
                    if (size == 0)
                        return 0;
                    else
                        return size * Math.Pow(1024, i + 1);
                }
            }

            units = new string[] { "KiB", "MiB", "GiB", "TiB", "PiB" };
            for (int i = 0; i < units.Length; i++)
            {
                if (sizeString.IndexOf(units[i], StringComparison.OrdinalIgnoreCase) > 0)
                {
                    var index = sizeString.LastIndexOf(units[i], StringComparison.OrdinalIgnoreCase);
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
        public static string GetCookieFromContainer(CookieContainer container, Uri uri)
        {
            if (container == null)
                return string.Empty;
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

        /// <summary>
        /// 获取CookieContainer
        /// </summary>
        /// <param name="cookieStr"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static CookieContainer GetContainerFromCookie(string cookieStr, Uri uri)
        {
            CookieContainer cc = new CookieContainer();
            Dictionary<string, string> AddCookies = new Dictionary<string, string>();
            if (!cookieStr.IsNullOrEmptyOrWhiteSpace())
            {
                var cookies = cookieStr.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (cookies != null && cookies.Length > 0)
                {
                    foreach (var cookieKvr in cookies)
                    {
                        try
                        {
                            string[] cookie = cookieKvr.Split('=');
                            if (cookie.Length == 2)
                            {
                                string name = cookie[0];
                                string value = cookie[1];
                                Cookie c = new Cookie();
                                c.Name = name;
                                c.Value = value;
                                c.Domain = uri.Host;
                                if (AddCookies.ContainsKey(name))
                                    continue;
                                else
                                {
                                    AddCookies.Add(name, value);
                                    cc.Add(c);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(string.Format("{0} 添加Cookie时出现异常。", uri.Host), ex);
                        }
                    }
                }
            }
            return cc;
        }

        /// <summary>
        /// 获取网站时间
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static DateTime GetWebSiteDateTime(string url)
        {
            WebRequest request = null;
            WebResponse response = null;
            WebHeaderCollection headerCollection = null;
            DateTime datetime = DateTime.Now;
            try
            {
                request = WebRequest.Create(url);
                request.Timeout = 3000;
                request.Credentials = CredentialCache.DefaultCredentials;
                response = request.GetResponse();
                headerCollection = response.Headers;
                foreach (var h in headerCollection.AllKeys)
                {
                    if (h == "Date")
                    {
                        datetime = headerCollection[h].TryPareValue<DateTime>();
                    }
                }
                return datetime;
            }
            catch (Exception)
            {
                return datetime;
            }
        }

        /// <summary>
        /// 获取版本
        /// </summary>
        /// <returns></returns>
        public static string GetVersion()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            var asmVersion = asm.GetName().Version;
            return string.Format("{0}.{1}.{2}", asmVersion.Major, asmVersion.Minor, asmVersion.Build);
        }

        public static void WriteCookiesContainerToDisk(string file, CookieContainer cc)
        {
            if (!Directory.Exists(Path.GetDirectoryName(file)))
                Directory.CreateDirectory(Path.GetDirectoryName(file));
            using (FileStream stream = File.Create(file))
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, cc);
                }
                catch (Exception e)
                {
                    Logger.Error(string.Format("Cookie写入文件失败，文件名[{0}]", file), e);
                }
            }
        }

        public static void WriteCookiesToDisk(string file, string cookies)
        {
            if (!Directory.Exists(Path.GetDirectoryName(file)))
                Directory.CreateDirectory(Path.GetDirectoryName(file));
            try
            {
                File.WriteAllText(file, cookies, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Logger.Error(string.Format("Cookie写入文件失败，文件名[{0}]", file), e);
            }
        }

        public static CookieContainer ReadCookiesContainerFromDisk(string file)
        {
            try
            {
                if (File.Exists(file))
                {
                    using (Stream stream = File.Open(file, FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        return (CookieContainer)formatter.Deserialize(stream);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Logger.Error(string.Format("从文件读取Cookie失败，文件名[{0}]", file), e);
                return null;
            }
        }

        public static string ReadCookiesFromDisk(string file)
        {
            if (!File.Exists(file))
                return null;
            try
            {
                Dictionary<string, string> AddCookies = new Dictionary<string, string>();
                List<string> result = new List<string>();
                string cookieStr = File.ReadAllText(file, Encoding.UTF8);
                if (!cookieStr.IsNullOrEmptyOrWhiteSpace())
                {
                    var cookies = cookieStr.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (cookies != null && cookies.Length > 0)
                    {
                        foreach (var cookieKvr in cookies)
                        {
                            string[] cookie = cookieKvr.Split('=');
                            if (cookie.Length == 2)
                            {
                                string name = cookie[0];
                                string value = cookie[1];
                                if (AddCookies.ContainsKey(name))
                                    continue;
                                else
                                {
                                    AddCookies.Add(name, value);
                                }
                            }
                        }
                    }
                    if (AddCookies.Count > 0)
                    {
                        foreach (var item in AddCookies)
                        {
                            result.Add(string.Format("{0}={1}", item.Key, item.Value));
                        }
                        return string.Join("; ", result);
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                Logger.Error(string.Format("从文件读取Cookie失败，文件名[{0}]", file), e);
                return null;
            }
        }
    }
}
