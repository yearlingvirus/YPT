using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YU.Core;
using YU.Core.DataEntity;
using YU.Core.Event;
using YU.Core.Log;
using YU.Core.Utils;
using System.Web;

namespace YPT.PT
{
    /// <summary>
    /// PT抽象类
    /// </summary>
    public abstract class AbstractPT : IPT
    {

        #region 字段

        /// <summary>
        /// 站点类型
        /// </summary>
        protected abstract YUEnums.PTEnum SiteId { get; }

        /// <summary>
        /// PT站点
        /// </summary>
        protected PTSite Site { get; set; }

        /// <summary>
        /// _cookie
        /// </summary>
        protected CookieContainer _cookie;

        protected static object syncObject = new object();

        public CookieContainer Cookie
        {
            get
            {
                lock (syncObject)
                {
                    if (_cookie == null || _cookie.Count <= 0)
                        _cookie = GetLocalCookie();
                }
                return _cookie;
            }
        }

        /// <summary>
        /// _cookie
        /// </summary>
        protected PTUser _user;


        /// <summary>
        /// 用户
        /// </summary>
        public PTUser User
        {
            get { return _user; }
        }


        /// <summary>
        /// 验证码委托
        /// </summary>
        /// <returns></returns>
        public delegate string OnVerificationCodeEventHandler(object sender, OnVerificationCodeEventArgs e);

        /// <summary>
        /// 验证码事件
        /// </summary>
        public event OnVerificationCodeEventHandler VerificationCode;

        /// <summary>
        /// 两步验证委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public delegate string OnTwoStepVerificationEventHandler(object sender, OnTwoStepVerificationEventArgs e);

        /// <summary>
        /// 两步验证事件
        /// </summary>
        public event OnTwoStepVerificationEventHandler TwoStepVerification;
        #endregion

        public AbstractPT()
        {

        }

        public AbstractPT(PTUser user)
        {
            Site = Global.Sites.Where(x => x.Id == SiteId).FirstOrDefault();
            _user = user;
            _cookie = GetLocalCookie();
        }

        public virtual string Login()
        {
            string postData = string.Format("username={0}&password={1}", User.UserName, User.PassWord);
            var result = HttpUtils.PostData(Site.LoginUrl, postData, _cookie);
            string htmlResult = result.Item1;
            if (!htmlResult.Contains("登录失败") && htmlResult.Contains(User.UserName) && (htmlResult.Contains("欢迎回来") || htmlResult.Contains("Welcome") || htmlResult.Contains("歡迎回來")))
            {
                User.Id = GetUserId(htmlResult);
                _cookie = result.Item2.CookieContainer;
                SetLocalCookie(_cookie);
                return "登录成功。";
            }
            else
            {
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlResult);//加载HTML字符串，如果是文件可以用htmlDocument.Load方法加载
                HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"nav_block\"]/table/tr/td/table/tr/td");//跟Xpath一样
                string errMsg = htmlResult;
                if (node != null)
                    errMsg = node.InnerText;
                return string.Format("登录失败，失败原因：{0}", errMsg);
            }
        }

        /// <summary>
        /// 获取用户Id
        /// </summary>
        /// <param name="htmlResult"></param>
        /// <returns></returns>
        protected virtual int GetUserId(string htmlResult)
        {
            int id = 0;
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlResult);//加载HTML字符串，如果是文件可以用htmlDocument.Load方法加载
            HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"info_block\"]/tr/td/table/tr/td//a");//跟Xpath一样
            if (node != null)
            {
                var url = HttpUtility.HtmlDecode(node.Attributes["href"].Value);
                url = string.Join("/", Site.Url, url);
                id = url.UrlSearchKey("id").TryPareValue<int>();
            }
            return id;
        }

        /// <summary>
        /// 签到
        /// </summary>
        public virtual string Sign() { return "签到尚未实现，革命仍需努力。"; }

        /// <summary>
        /// 触发验证码事件
        /// </summary>
        /// <param name="e"></param>
        public virtual string OnVerificationCode(OnVerificationCodeEventArgs e)
        {
            if (VerificationCode != null)
            {
                return VerificationCode.Invoke(this, e);
            }
            return string.Empty;
        }

        /// <summary>
        /// 触发两步验证事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public virtual string OnTwoStepVerification(OnTwoStepVerificationEventArgs e)
        {
            if (TwoStepVerification != null)
            {
                return TwoStepVerification.Invoke(this, e);
            }
            return string.Empty;
        }

        #region 种子处理

        public List<PTTorrent> SearchTorrent(string searchKey, YUEnums.PromotionType promotionType = YUEnums.PromotionType.ALL, YUEnums.AliveType aliveType = YUEnums.AliveType.ALL, YUEnums.FavType favType = YUEnums.FavType.ALL)
        {
            if (Site.SearchUrl.IsNullOrEmptyOrWhiteSpace())
            {
                //站点还没有支持搜索
                throw new Exception(string.Format("搜索种子错误，错误站点：{0}，关键字：{1}，错误原因：该站点尚未支持搜索。", Site.Name, searchKey));
            }

            if (_cookie == null || _cookie.Count <= 0)
            {
                throw new Exception(string.Format("搜索种子错误，错误站点：{0}，关键字：{1}，错误原因：未登录，请先登录。", Site.Name, searchKey));
            }

            string searchUrl = BuildSearchUrl(searchKey, promotionType, aliveType, favType);
            string htmlResult = HttpUtils.GetDataGetHtml(searchUrl, _cookie);

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlResult);//加载HTML字符串，如果是文件可以用htmlDocument.Load方法加载

            var trNodes = GetTorrentNodes(htmlDocument);
            //如果只有一个节点，那么应该是Table的标题，这里忽略，从第二个节点开始算。
            if (trNodes == null)
                throw new Exception(string.Format("{0}无法搜索到对应结果，请尝试更换其他关键字。", SiteId));
            else
            {
                List<PTTorrent> torrents = new List<PTTorrent>();
                for (int i = 1; i < trNodes.Count; i++)
                {
                    var trNode = trNodes[i];
                    var tdNodes = GetTorrentNodes(trNode);
                    //这里一般有10列，但是Frds为9列，所以这里以9列来处理
                    if (tdNodes == null || tdNodes.Count < 9)
                        continue;
                    else
                    {
                        PTTorrent torrent = new PTTorrent();
                        torrent.SiteId = SiteId;
                        torrent.SiteName = Site.Name;

                        //tdNodes[1]为种子信息
                        //种子链接和标题是最重要的，如果这里拿不到，直接跳过了
                        if (!SetTorrentTitleAndLink(tdNodes[Site.TorrentMaps[YUEnums.TorrentMap.Detail]], torrent))
                            continue;

                        //这里的副标题如果没有的话，是否需要跳过?暂时先保留把。
                        SetTorrentSubTitle(tdNodes[Site.TorrentMaps[YUEnums.TorrentMap.Detail]], torrent);

                        SetTorrentPromotionType(tdNodes[Site.TorrentMaps[YUEnums.TorrentMap.Detail]], torrent);

                        SetTorrentHR(tdNodes[Site.TorrentMaps[YUEnums.TorrentMap.Detail]], torrent);

                        SetTorrentOtherInfo(tdNodes, torrent);

                        torrents.Add(torrent);
                    }
                }
                return torrents;
            }
        }

        protected virtual string BuildSearchUrl(string searchKey, YUEnums.PromotionType promotionType = YUEnums.PromotionType.ALL, YUEnums.AliveType aliveType = YUEnums.AliveType.ALL, YUEnums.FavType favType = YUEnums.FavType.ALL)
        {
            //https://ourbits.club/torrents.php?incldead=2&spstate=2&inclbookmarked=1&search=&search_area=0&search_mode=0
            //incldead->活种/断种 | spstate->促销 | inclbookmarked->收藏 | search->关键字 | search_area->范围 | search_mode->匹配模式
            string queryString = string.Format("?incldead={0}&spstate={1}&inclbookmarked={2}&search={3}&search_area={4}&search_mode={5}", (int)aliveType, (int)promotionType, (int)favType, Uri.EscapeDataString(searchKey), 0, 0);
            return Site.SearchUrl + queryString;
        }

        /// <summary>
        /// 获取所有种子节点
        /// </summary>
        /// <param name="htmlDocument"></param>
        /// <returns></returns>
        protected virtual HtmlNodeCollection GetTorrentNodes(HtmlDocument htmlDocument)
        {
            return htmlDocument.DocumentNode.SelectNodes("//table[contains(concat(' ', normalize-space(@class), ' '), ' torrents ')]/tr");
        }

        /// <summary>
        /// 获取某行种子中信息节点
        /// </summary>
        /// <param name="trNode"></param>
        /// <returns></returns>
        protected virtual HtmlNodeCollection GetTorrentNodes(HtmlNode trNode)
        {
            return trNode.SelectNodes("./td");
        }


        /// <summary>
        /// 设置种子Id，链接和标题
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected virtual bool SetTorrentTitleAndLink(HtmlNode node, PTTorrent torrent)
        {
            var nodes = node.SelectNodes(".//td[contains(concat(' ', normalize-space(@class), ' '), ' embedded ')]//a");
            if (nodes != null && nodes.Count >= 2)
            {
                var titleNode = nodes[0];
                var downNode = nodes[1];
                if (titleNode != null && titleNode.Attributes.Contains("title") && titleNode.Attributes.Contains("href"))
                {
                    var linkUrl = HttpUtility.HtmlDecode(titleNode.Attributes["href"].Value);
                    torrent.LinkUrl = string.Join("/", Site.Url, linkUrl);
                    torrent.Id = torrent.LinkUrl.UrlSearchKey("id");
                    torrent.Title = titleNode.Attributes["title"].Value;
                }
                else
                    return false;

                if (downNode != null && downNode.Attributes.Contains("href"))
                {
                    torrent.DownUrl = string.Join("/", Site.Url, HttpUtility.HtmlDecode(downNode.Attributes["href"].Value));
                    if (!(torrent.DownUrl.Contains("download") && torrent.DownUrl.Contains(torrent.Id)))
                        torrent.DownUrl = string.Join("/", Site.Url, string.Format("download.php?id={0}", torrent.Id));
                }
                else
                    return false;

                return true;
            }
            return false;

        }


        /// <summary>
        /// 设置副标题
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected virtual bool SetTorrentSubTitle(HtmlNode node, PTTorrent torrent)
        {
            var subNode = node.SelectSingleNode(".//td/font[contains(concat(' ', normalize-space(@class), ' '), ' subtitle ')]");
            if (subNode != null)
            {
                torrent.Subtitle = HttpUtility.HtmlDecode(subNode.InnerText);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 设置HR信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="torrent"></param>
        protected virtual void SetTorrentHR(HtmlNode node, PTTorrent torrent)
        {
            string html = string.Empty;
            if (!torrent.Title.IsNullOrEmptyOrWhiteSpace())
                html = node.InnerHtml.Replace(torrent.Title, "");
            if (!torrent.Subtitle.IsNullOrEmptyOrWhiteSpace())
                html = node.InnerHtml.Replace(torrent.Subtitle, "");
            torrent.IsHR = false;

            if (html.Contains("hit_run"))
            {
                torrent.IsHR = true;
                return;
            }
        }


        /// <summary>
        /// 设置促销信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="torrent"></param>
        /// 
        protected virtual void SetTorrentPromotionType(HtmlNode node, PTTorrent torrent)
        {
            string html = string.Empty;
            if (!torrent.Title.IsNullOrEmptyOrWhiteSpace())
                html = node.InnerHtml.Replace(torrent.Title, "");
            if (!torrent.Subtitle.IsNullOrEmptyOrWhiteSpace())
                html = node.InnerHtml.Replace(torrent.Subtitle, "");
            torrent.PromotionType = YUEnums.PromotionType.NORMAL;

            foreach (var item in PTSiteConst.PromoptionDict)
            {
                foreach (var value in item.Value)
                {
                    if (html.Contains(value))
                    {
                        torrent.PromotionType = item.Key;
                        return;
                    }
                }
            }
        }


        /// <summary>
        /// 设置其他信息
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        protected virtual void SetTorrentOtherInfo(HtmlNodeCollection nodes, PTTorrent torrent)
        {
            //设置资源类型
            var imgNode = nodes[Site.TorrentMaps[YUEnums.TorrentMap.ResourceType]].SelectSingleNode(".//img");
            if (imgNode != null && imgNode.Attributes.Contains("alt"))
            {
                torrent.ResourceType = imgNode.Attributes["alt"].Value;
            }

            HtmlNode timeNode = nodes[Site.TorrentMaps[YUEnums.TorrentMap.TimeAlive]].SelectSingleNode(".//span");
            if (timeNode != null && timeNode.Attributes.Contains("title"))
                torrent.UpLoadTime = timeNode.Attributes["title"].Value.TryPareValue<DateTime>();
            else
            {
                string uploadTimeStr = nodes[Site.TorrentMaps[YUEnums.TorrentMap.TimeAlive]].InnerText;
                if (!uploadTimeStr.IsNullOrEmptyOrWhiteSpace() && uploadTimeStr.Length >= 18)
                {
                    string dateStr = uploadTimeStr.Substring(0, 10);
                    string timeStr = uploadTimeStr.Substring(10);
                    torrent.UpLoadTime = string.Format("{0} {1}", dateStr, timeStr).TryPareValue<DateTime>();
                }
            }
            torrent.Size = nodes[Site.TorrentMaps[YUEnums.TorrentMap.Size]].InnerText;
            torrent.SeederNumber = nodes[Site.TorrentMaps[YUEnums.TorrentMap.SeederNumber]].InnerText.TryPareValue<int>();
            torrent.LeecherNumber = nodes[Site.TorrentMaps[YUEnums.TorrentMap.LeecherNumber]].InnerText.TryPareValue<int>();
            torrent.SnatchedNumber = nodes[Site.TorrentMaps[YUEnums.TorrentMap.SnatchedNumber]].InnerText.TryPareValue<int>();
            torrent.UpLoader = nodes[Site.TorrentMaps[YUEnums.TorrentMap.UpLoader]].InnerText;
        }

        #endregion

        /// <summary>
        /// 从本地文件中读取Cookie
        /// </summary>
        /// <returns></returns>
        protected CookieContainer GetLocalCookie()
        {
            string cookiePath = GetCookieFilePath();
            return HttpUtils.ReadCookiesFromDisk(Site.Url, cookiePath);
        }

        /// <summary>
        /// 将Cookie写入本地文件
        /// </summary>
        /// <param name="_cookie"></param>
        protected void SetLocalCookie(CookieContainer _cookie)
        {
            string cookiePath = GetCookieFilePath();
            HttpUtils.WriteCookiesToDisk(cookiePath, _cookie);
        }

        public void DelLocalCookie()
        {
            string cookiePath = GetCookieFilePath();
            if (File.Exists(cookiePath))
                File.Delete(cookiePath);
        }

        public string GetCookieFilePath()
        {
            string cookiePath = Path.Combine(YUConst.PATH_LOCALCOOKIE, string.Format("{0}_{1}.cookie", Site.Name, User.UserName));
            return cookiePath;
        }

        /// <summary>
        /// 获取种子下载名称
        /// </summary>
        /// <param name="torrent"></param>
        /// <returns></returns>
        public string GetTorrentDownFileName(PTTorrent torrent)
        {
            if (torrent != null)
            {
                string fileName = string.Format("[{0}].{1}.{2}", SiteId, torrent.Title.Trim(), "torrent");
                if (Global.Config.IsEnablePostFileName)
                {
                    try
                    {
                        var url = torrent.DownUrl;

                        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        httpWebRequest.Method = "GET";
                        //这里用IE9的内核解析
                        httpWebRequest.UserAgent = "Mozilla / 5.0(compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident / 5.0)";
                        httpWebRequest.KeepAlive = true;
                        httpWebRequest.Timeout = 3000;
                        httpWebRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                        httpWebRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                        httpWebRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh");
                        httpWebRequest.CookieContainer = _cookie;

                        HttpWebResponse webRespon = (HttpWebResponse)httpWebRequest.GetResponse();

                        if (webRespon.Headers.AllKeys.Contains("Content-Disposition"))
                        {
                            string contentDis = webRespon.Headers.Get("Content-Disposition");
                            if (!contentDis.IsNullOrEmptyOrWhiteSpace())
                            {
                                string[] headers = contentDis.Split(';', '=');
                                if (headers.Length >= 3 && headers[2].Contains("torrent"))
                                {
                                    fileName = HttpUtility.UrlDecode(headers[2]).Replace("\"", "").Trim();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(string.Format("{0} 获取种子文件名称失败，种子标题：{1}", SiteId, torrent.Title), ex);
                    }
                }
                foreach (var car in Path.GetInvalidFileNameChars())
                {
                    if (fileName.Contains(car))
                        fileName = fileName.Replace(car, '_');
                }
                return fileName;
            }
            else
                throw new Exception("获取种子文件名称失败，失败原因：无法获取到种子信息。");
        }


        public virtual PTInfo GetPersonInfo()
        {
            PTInfo info = new PTInfo();
            if (User.Id == 0)
                throw new Exception(string.Format("{0} 无法获取用户Id，请尝试重新登录。", SiteId));
            else
            {
                string url = string.Format(Site.InfoUrl, User.Id);
                string htmlResult = HttpUtils.GetDataGetHtml(url, _cookie);

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlResult);//加载HTML字符串，如果是文件可以用htmlDocument.Load方法加载
                HtmlNodeCollection nodes = htmlDocument.DocumentNode.SelectNodes("//table[contains(concat(' ', normalize-space(@class), ' '), ' main ')]/tr/td/table/tr");//跟Xpath一样
                if (nodes == null || nodes.Count <= 0)
                    throw new Exception(string.Format("{0} 获取用户详细信息失败，请稍后重试。", SiteId));
                else
                {
                    #region Convert
                    //注册日期
                    var node = nodes[Site.PersonInfoMaps[YUEnums.PersonInfoMap.RegisterDate]];
                    if (node != null)
                    {
                        node = node.SelectSingleNode(".//span");
                        if (node != null && node.Attributes.Contains("title"))
                        {
                            info.RegisterDate = node.Attributes["title"].Value.TryPareValue<DateTime>();
                        }
                    }

                    //分享率
                    node = nodes[Site.PersonInfoMaps[YUEnums.PersonInfoMap.ShareRate]];
                    if (node != null)
                    {
                        var childNode = node.SelectSingleNode(".//td/font/text()");
                        if (childNode != null)
                            info.ShareRate = childNode.InnerText;
                    }

                    //上传量
                    node = nodes[Site.PersonInfoMaps[YUEnums.PersonInfoMap.UpSize]];
                    if (node != null)
                    {
                        var childNode = node.SelectSingleNode(".//td//tr[2]/td/text()[last()]");
                        if (childNode != null)
                        {
                            var index = childNode.InnerText.IndexOf(":");
                            if (index > -1)
                                info.UpSize = childNode.InnerText.Substring(index + 1).Trim();
                        }
                    }

                    //下载量
                    node = nodes[Site.PersonInfoMaps[YUEnums.PersonInfoMap.DownSize]];
                    if (node != null)
                    {
                        var childNode = node.SelectSingleNode(".//td//tr[2]/td[2]/text()[2]");
                        if (childNode != null)
                        {
                            var index = childNode.InnerText.IndexOf(":");
                            if (index > -1)
                                info.DownSize = childNode.InnerText.Substring(index + 1).Trim();
                        }
                    }

                    //做种率
                    node = nodes[Site.PersonInfoMaps[YUEnums.PersonInfoMap.SeedRate]];
                    if (node != null)
                    {
                        var childNode = node.SelectSingleNode(".//td/font/text()");
                        if (childNode != null)
                            info.SeedRate = childNode.InnerText;
                    }

                    //做种时间
                    node = nodes[Site.PersonInfoMaps[YUEnums.PersonInfoMap.SeedTimes]];
                    if (node != null)
                    {
                        var childNode = node.SelectSingleNode(".//td//tr[2]/td/text()[last()]");
                        if (childNode != null)
                        {
                            var index = childNode.InnerText.IndexOf(":");
                            if (index > -1)
                                info.SeedTimes = childNode.InnerText.Substring(index + 1).Trim();
                        }
                    }

                    //下载时间
                    node = nodes[Site.PersonInfoMaps[YUEnums.PersonInfoMap.DownTimes]];
                    if (node != null)
                    {
                        var childNode = node.SelectSingleNode(".//td//tr[2]/td[2]/text()[last()]");
                        if (childNode != null)
                        {
                            var index = childNode.InnerText.IndexOf(":");
                            if (index > -1)
                                info.DownTimes = childNode.InnerText.Substring(index + 1).Trim();
                        }
                    }

                    //等级
                    node = nodes[Site.PersonInfoMaps[YUEnums.PersonInfoMap.Rank]];
                    if (node != null)
                    {
                        var childNode = node.SelectSingleNode("./td/img");
                        if (childNode != null && childNode.Attributes.Contains("alt"))
                        {
                            info.Rank = childNode.Attributes["alt"].Value;
                        }
                    }

                    //积分
                    node = nodes[Site.PersonInfoMaps[YUEnums.PersonInfoMap.Bonus]];
                    if (node != null)
                    {
                        var childNode = node.SelectSingleNode(".//td[2]");
                        if (childNode != null)
                        {
                            info.Bonus = childNode.InnerText.TryPareValue<double>();
                        }
                    }

                    info.LastSyncDate = DateTime.Now;
                    info.Id = User.Id;
                    info.SiteId = SiteId;
                    info.SiteName = Site.Name;
                    info.Name = User.UserName;

                    #endregion

                }
                return info;
            }
        }

    }
}
