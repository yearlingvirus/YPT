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
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*;q=0.8";
                req.Method = "POST";
                req.UserAgent = YUConst.HTTP_CHROME_UA;
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
                req.UserAgent = YUConst.HTTP_CHROME_UA;
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
                    req.Headers.Add(HttpRequestHeader.Cookie, YUUtils.GetCookieFromContainer(cookie, uri));
                    //req.CookieContainer = cookie;
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
                httpWebRequest.UserAgent = YUConst.HTTP_CHROME_UA;
                //对发送的数据不使用缓存
                httpWebRequest.AllowWriteStreamBuffering = false;
                httpWebRequest.Timeout = 10000;
                httpWebRequest.ServicePoint.Expect100Continue = false;

                if (cookie == null || cookie.Count == 0)
                {
                    httpWebRequest.CookieContainer = new CookieContainer();
                    cookie = httpWebRequest.CookieContainer;
                }
                else
                {
                    httpWebRequest.Headers.Add(HttpRequestHeader.Cookie, YUUtils.GetCookieFromContainer(cookie, new Uri(url)));
                    //httpWebRequest.CookieContainer = cookie;
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

        /// <summary>
        /// HttpWebRequest 通过get
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static Tuple<string, HttpWebRequest, HttpWebResponse> GetData(string url, CookieContainer cookie)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Method = "GET";
                httpWebRequest.UserAgent = YUConst.HTTP_CHROME_UA;
                //对发送的数据不使用缓存
                httpWebRequest.AllowWriteStreamBuffering = false;
                httpWebRequest.Timeout = 10000;
                httpWebRequest.ServicePoint.Expect100Continue = false;

                if (cookie == null || cookie.Count == 0)
                {
                    httpWebRequest.CookieContainer = new CookieContainer();
                    cookie = httpWebRequest.CookieContainer;
                }
                else
                {
                    httpWebRequest.Headers.Add(HttpRequestHeader.Cookie, YUUtils.GetCookieFromContainer(cookie, new Uri(url)));
                    //httpWebRequest.CookieContainer = cookie;
                }

                HttpWebResponse webRespon = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream webStream = webRespon.GetResponseStream();
                if (webStream == null)
                {
                    return new Tuple<string, HttpWebRequest, HttpWebResponse>("网络错误(Network error)：" + new ArgumentNullException("webStream").Message, httpWebRequest, webRespon);
                }
                StreamReader streamReader = new StreamReader(webStream, Encoding.UTF8);
                string responseContent = streamReader.ReadToEnd();

                webRespon.Close();
                streamReader.Close();

                return new Tuple<string, HttpWebRequest, HttpWebResponse>(responseContent, httpWebRequest, webRespon);
            }
            catch (Exception ex)
            {
                string msg = "网络错误(Network error)：" + ex.GetInnerExceptionMessage();
                Logger.Error(msg, ex);
                return new Tuple<string, HttpWebRequest, HttpWebResponse>(msg, null, null);
            }
        }

    }
}