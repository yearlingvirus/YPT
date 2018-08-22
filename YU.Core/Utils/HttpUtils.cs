using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
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
            HttpWebRequest req = null;
            HttpWebResponse res = null;
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(postData);
                Uri uri = new Uri(url);
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                req = WebRequest.Create(uri) as HttpWebRequest;
                if (req == null)
                {
                    return new Tuple<string, HttpWebRequest, HttpWebResponse>("Network error:" + new ArgumentNullException("req").Message, req, null);
                }
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*;q=0.8";
                req.Method = "POST";
                req.UserAgent = YUConst.HTTP_CHROME_UA;
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

                res = req.GetResponse() as HttpWebResponse;
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
            finally
            {
                if (req != null)
                    req.Abort();//销毁关闭连接
                if (res != null)
                    res.Close();//销毁关闭响应
            }
        }

        public static string PostDataGetHtml(string url, string postData, CookieContainer cookie, bool isFormDataType = false)
        {
            HttpWebRequest req = null;
            HttpWebResponse res = null;
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(postData);
                Uri uri = new Uri(url);
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                req = WebRequest.Create(uri) as HttpWebRequest;
                if (req == null)
                {
                    return "Network error:" + new ArgumentNullException("req").Message;
                }
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*;q=0.8";
                req.Method = "POST";
                req.UserAgent = YUConst.HTTP_CHROME_UA;
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
                    req.Headers.Add(HttpRequestHeader.Cookie, YUUtils.GetCookieFromContainer(cookie, uri));
                    //req.CookieContainer = cookie;
                }

                Stream outStream = req.GetRequestStream();
                outStream.Write(data, 0, data.Length);
                outStream.Close();
                res = req.GetResponse() as HttpWebResponse;
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
            finally
            {
                if (req != null)
                    req.Abort();//销毁关闭连接
                if (res != null)
                    res.Close();//销毁关闭响应
            }
        }

        /// <summary>
        /// HttpWebRequest 通过get
        /// </summary>
        /// <param name="url">URI</param>
        /// <returns></returns>
        public static string GetDataGetHtml(string url, CookieContainer cookie)
        {
            HttpWebRequest req = null;
            HttpWebResponse res = null;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                req = (HttpWebRequest)WebRequest.Create(url);
                req.ContentType = "application/x-www-form-urlencoded";
                req.Method = "GET";
                req.UserAgent = YUConst.HTTP_CHROME_UA;
                //对发送的数据不使用缓存
                req.AllowWriteStreamBuffering = false;
                req.Timeout = 10000;
                req.ServicePoint.Expect100Continue = false;

                if (cookie == null || cookie.Count == 0)
                {
                    req.CookieContainer = new CookieContainer();
                    cookie = req.CookieContainer;
                }
                else
                {
                    req.Headers.Add(HttpRequestHeader.Cookie, YUUtils.GetCookieFromContainer(cookie, new Uri(url)));
                    //req.CookieContainer = cookie;
                }

                res = (HttpWebResponse)req.GetResponse();
                Stream webStream = res.GetResponseStream();
                if (webStream == null)
                {
                    return "网络错误(Network error)：" + new ArgumentNullException("webStream");
                }
                StreamReader streamReader = new StreamReader(webStream, Encoding.UTF8);
                string responseContent = streamReader.ReadToEnd();

                res.Close();
                streamReader.Close();

                return responseContent;
            }
            catch (Exception ex)
            {
                string msg = "网络错误(Network error)：" + ex.GetInnerExceptionMessage();
                Logger.Error(msg, ex);
                return msg;
            }
            finally
            {
                if (req != null)
                    req.Abort();//销毁关闭连接
                if (res != null)
                    res.Close();//销毁关闭响应
            }
        }

        /// <summary>
        /// HttpWebRequest 通过get
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static Tuple<string, HttpWebRequest, HttpWebResponse> GetData(string url, CookieContainer cookie)
        {
            HttpWebRequest req = null;
            HttpWebResponse res = null;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                req = (HttpWebRequest)WebRequest.Create(url);

                req.ContentType = "application/x-www-form-urlencoded";
                req.Method = "GET";
                req.UserAgent = YUConst.HTTP_CHROME_UA;
                //对发送的数据不使用缓存
                req.AllowWriteStreamBuffering = false;
                req.Timeout = 10000;
                req.ServicePoint.Expect100Continue = false;

                if (cookie == null || cookie.Count == 0)
                {
                    req.CookieContainer = new CookieContainer();
                    cookie = req.CookieContainer;
                }
                else
                {
                    //req.Headers.Add(HttpRequestHeader.Cookie, YUUtils.GetCookieFromContainer(cookie, new Uri(url)));
                    req.CookieContainer = cookie;
                }

                res = (HttpWebResponse)req.GetResponse();
                Stream webStream = res.GetResponseStream();
                if (webStream == null)
                {
                    return new Tuple<string, HttpWebRequest, HttpWebResponse>("网络错误(Network error)：" + new ArgumentNullException("webStream").Message, req, res);
                }
                StreamReader streamReader = new StreamReader(webStream, Encoding.UTF8);
                string responseContent = streamReader.ReadToEnd();

                res.Close();
                streamReader.Close();

                return new Tuple<string, HttpWebRequest, HttpWebResponse>(responseContent, req, res);
            }
            catch (Exception ex)
            {
                string msg = "网络错误(Network error)：" + ex.GetInnerExceptionMessage();
                Logger.Error(msg, ex);
                return new Tuple<string, HttpWebRequest, HttpWebResponse>(msg, null, null);
            }
            finally
            {
                if (req != null)
                    req.Abort();//销毁关闭连接
                if (res != null)
                    res.Close();//销毁关闭响应
            }
        }

        /// <summary>
        /// 是否异常请求
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool IsErrorRequest(string result)
        {
            return result.Contains("Network error");
        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {  // 总是接受  
            return true;
        }
    }
}