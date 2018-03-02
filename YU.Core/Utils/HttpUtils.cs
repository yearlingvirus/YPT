using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using YU.Core.Log;

namespace YU.Core.Utils
{
    /// <summary>
    /// Http访问帮助类
    /// </summary>
    public class HttpUtils
    {

        public static Tuple<string, HttpWebRequest, HttpWebResponse> PostData(string url, string postData, CookieContainer cookie, bool isFormDataType = false)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(postData);
                Uri uri = new Uri(url);
                HttpWebRequest req = WebRequest.Create(uri) as HttpWebRequest;
                if (req == null)
                {
                    return new Tuple<string, HttpWebRequest, HttpWebResponse>("Network error:" + new ArgumentNullException("httpWebRequest").Message, req, null);
                }
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                req.Method = "POST";
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.84 Safari/537.36";
                req.KeepAlive = true;
                if (isFormDataType)
                {
                    req.ContentType = string.Format("multipart/form-data; boundary=----{0}", YUConst.POST_BOUNDARY);
                }
                else
                {
                    req.ContentType = "application/x-www-form-urlencoded";
                }
                req.ContentLength = data.Length;
                req.Timeout = 10000;
                req.AllowAutoRedirect = true;
                req.Referer = url;

                if (cookie == null || cookie.Count == 0)
                {
                    req.CookieContainer = new CookieContainer();
                }
                else
                {
                    req.CookieContainer = cookie;
                }

                Stream outStream = req.GetRequestStream();
                outStream.Write(data, 0, data.Length);
                outStream.Close();

                var res = req.GetResponse() as HttpWebResponse;
                if (res == null)
                {
                    return new Tuple<string, HttpWebRequest, HttpWebResponse>("Network error:" + new ArgumentNullException("HttpWebResponse").Message, req, null);
                }
                Stream inStream = res.GetResponseStream();
                var sr = new StreamReader(inStream, Encoding.UTF8);
                string htmlResult = sr.ReadToEnd();
                return new Tuple<string, HttpWebRequest, HttpWebResponse>(htmlResult, req, res);
            }
            catch (Exception ex)
            {
                string msg = "网络错误(Network error)：" + ex.GetInnerExceptionMessage();
                Logger.Error(msg, ex);
                return new Tuple<string, HttpWebRequest, HttpWebResponse>(msg, null, null);
            }
        }

        public static string PostDataGetHtml(string url, string postData, CookieContainer cookie, bool isFormDataType = false)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(postData);
                Uri uri = new Uri(url);
                HttpWebRequest req = WebRequest.Create(uri) as HttpWebRequest;
                if (req == null)
                {
                    return "Network error:" + new ArgumentNullException("httpWebRequest").Message;
                }
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*;q=0.8";
                req.Method = "POST";
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.84 Safari/537.36";
                req.KeepAlive = true;
                if (isFormDataType)
                {
                    req.ContentType = string.Format("multipart/form-data; boundary=----{0}", YUConst.POST_BOUNDARY);
                }
                else
                {
                    req.ContentType = "application/x-www-form-urlencoded";
                }
                req.ContentLength = data.Length;
                req.Timeout = 10000;
                req.AllowAutoRedirect = true;
                req.Referer = url;

                if (cookie == null || cookie.Count == 0)
                {
                    req.CookieContainer = new CookieContainer();
                }
                else
                {
                    req.CookieContainer = cookie;
                }

                Stream outStream = req.GetRequestStream();
                outStream.Write(data, 0, data.Length);
                outStream.Close();

                var res = req.GetResponse() as HttpWebResponse;
                if (res == null)
                {
                    return "Network error:" + new ArgumentNullException("HttpWebResponse").Message;
                }
                Stream inStream = res.GetResponseStream();
                var sr = new StreamReader(inStream, Encoding.UTF8);
                string htmlResult = sr.ReadToEnd();
                return htmlResult;
            }
            catch (Exception ex)
            {
                string msg = "网络错误(Network error)：" + ex.GetInnerExceptionMessage();
                Logger.Error(msg, ex);
                return msg;
            }
        }

        /// <summary>
        /// HttpWebRequest 通过get
        /// </summary>
        /// <param name="url">URI</param>
        /// <returns></returns>
        public static string GetDataGetHtml(string url, CookieContainer cookie)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Method = "GET";
                httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.84 Safari/537.36";
                //对发送的数据不使用缓存
                httpWebRequest.AllowWriteStreamBuffering = false;
                httpWebRequest.Timeout = 10000;
                httpWebRequest.ServicePoint.Expect100Continue = false;

                if (cookie.Count == 0)
                {
                    httpWebRequest.CookieContainer = new CookieContainer();
                    cookie = httpWebRequest.CookieContainer;
                }
                else
                {
                    httpWebRequest.CookieContainer = cookie;
                }

                HttpWebResponse webRespon = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream webStream = webRespon.GetResponseStream();
                if (webStream == null)
                {
                    return "网络错误(Network error)：" + new ArgumentNullException("webStream");
                }
                StreamReader streamReader = new StreamReader(webStream, Encoding.UTF8);
                string responseContent = streamReader.ReadToEnd();

                webRespon.Close();
                streamReader.Close();

                return responseContent;
            }
            catch (Exception ex)
            {
                string msg = "网络错误(Network error)：" + ex.GetInnerExceptionMessage();
                Logger.Error(msg, ex);
                return msg;
            }
        }

        public static List<Cookie> GetAllCookies(CookieContainer cc)
        {
            List<Cookie> lstCookies = new List<Cookie>();

            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.Instance, null, cc, new object[] { });

            foreach (object pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField
                    | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie c in colCookies) lstCookies.Add(c);
            }

            return lstCookies;
        }

        public static void WriteCookiesToDisk(string file, CookieContainer cc)
        {
            if (!Directory.Exists(Path.GetDirectoryName(file)))
                Directory.CreateDirectory(Path.GetDirectoryName(file));
            //using (FileStream stream = File.Create(file))
            //{
            //    try
            //    {
            //        BinaryFormatter formatter = new BinaryFormatter();
            //        formatter.Serialize(stream, cc);
            //    }
            //    catch (Exception e)
            //    {
            //        Logger.Error(string.Format("Cookie写入文件失败，文件名[{0}]", file), e);
            //    }
            //}
            try
            {
                StringBuilder sb = new StringBuilder();
                List<Cookie> cooklist = GetAllCookies(cc);
                if (cooklist != null && cooklist.Count > 0)
                {
                    foreach (var cookie in cooklist)
                    {
                        sb.AppendLine(string.Format("{0}={1}; ", cookie.Name, cookie.Value));
                    }
                    File.WriteAllText(file, sb.ToString(), Encoding.UTF8);
                }

            }
            catch (Exception e)
            {
                Logger.Error(string.Format("Cookie写入文件失败，文件名[{0}]", file), e);
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

        public static CookieContainer ReadCookiesFromDisk(string file)
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

        public static CookieContainer ReadCookiesFromDisk(string url, string file)
        {
            try
            {
                Uri uri = new Uri(url);
                CookieContainer cc = new CookieContainer();
                string cookieStr = File.ReadAllText(file, Encoding.UTF8);
                if (!cookieStr.IsNullOrEmptyOrWhiteSpace())
                {
                    string[] cookieArr = cookieStr.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (cookieArr != null && cookieArr.Length > 0)
                    {
                        foreach (var cookieKvr in cookieArr)
                        {
                            string[] cookie = cookieKvr.Split('=');
                            if (cookie.Length == 2)
                            {
                                Cookie c = new Cookie();
                                c.Name = cookie[0].Trim();
                                c.Value = cookie[1].Trim();
                                c.Domain = uri.Host;
                                c.Expires = DateTime.Now.AddYears(1);
                                cc.Add(c);
                            }

                        }
                    }
                }
                return cc;
            }
            catch (Exception e)
            {
                Logger.Error(string.Format("从文件读取Cookie失败，文件名[{0}]", file), e);
                return null;
            }
        }

        /// <summary>
        /// Http下载文件
        /// </summary>
        /// <param name="uri">下载地址</param>
        /// <param name="filefullpath">存放完整路径（含文件名）</param>
        /// <param name="size">每次多的大小</param>
        /// <returns>下载操作是否成功</returns>
        public static bool DownLoadFiles(string url, string filefullpath, int size = 1024, CookieContainer cookie = null, bool isOpen = false)
        {
            try
            {
                if (File.Exists(filefullpath))
                {
                    try
                    {
                        File.Delete(filefullpath);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(string.Format("文件[{0}]下载失败，失败原因：{1}", filefullpath, ex.GetInnerExceptionMessage()), ex);
                        return false;
                    }
                }
                string fileDirectory = System.IO.Path.GetDirectoryName(filefullpath);

                if (!Directory.Exists(fileDirectory))
                {
                    Directory.CreateDirectory(fileDirectory);
                }

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                //httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Method = "GET";
                httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.84 Safari/537.36";
                httpWebRequest.KeepAlive = true;
                httpWebRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*;q=0.8";
                httpWebRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                httpWebRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh");
                httpWebRequest.CookieContainer = cookie;
                httpWebRequest.Timeout = 10000;
                HttpWebResponse webRespon = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream webStream = webRespon.GetResponseStream();
                if (webStream == null)
                {
                    throw new ArgumentNullException("网络错误(Network error)");
                }

                FileStream fs = new FileStream(filefullpath, FileMode.Create);
                byte[] buffer = new byte[size];
                int length = webStream.Read(buffer, 0, buffer.Length);
                while (length > 0)
                {
                    fs.Write(buffer, 0, length);
                    buffer = new byte[size];
                    length = webStream.Read(buffer, 0, buffer.Length);
                }
                fs.Close();
                webRespon.Close();

                if (isOpen)
                    System.Diagnostics.Process.Start(filefullpath);
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("文件[{0}]下载失败，失败原因：{1}", filefullpath, ex.GetInnerExceptionMessage(), ex));
            }
        }

    }
}