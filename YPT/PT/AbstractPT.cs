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

        /// <summary>
        /// 准备下载文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public delegate string OnPrepareDownFileEventHandler(object sender, OnPrepareDownFileEventArgs e);

        /// <summary>
        /// 准备下载文件事件
        /// </summary>
        public event OnPrepareDownFileEventHandler PrepareDownFile;
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

        #region 登录

        public string Login()
        {
            Tuple<string, HttpWebRequest, HttpWebResponse> result = null;
            if (_cookie != null && _cookie.Count > 0)
            {
                result = HttpUtils.GetData(Site.Url, _cookie);
                if (IsLoginSuccess(result.Item1))
                {
                    User.Id = GetUserId(result.Item1);
                    return "登录成功。";
                }
            }

            result = DoLoginPostWithOutCookie(result);
            string htmlResult = result.Item1;
            if (IsLoginSuccess(htmlResult))
            {
                _cookie = result.Item2.CookieContainer;
                User.Id = GetUserId(result.Item1);
                SetLocalCookie(_cookie);
                return "登录成功。";
            }
            else
            {
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlResult);//加载HTML字符串，如果是文件可以用htmlDocument.Load方法加载
                HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode("//table/tr/td/table/tr/td");//跟Xpath一样
                string errMsg = htmlResult;
                if (node != null)
                    errMsg = node.InnerText;
                return string.Format("登录失败，失败原因：{0}", errMsg);
            }
        }

        /// <summary>
        /// LoginPost->实际的登录操作
        /// </summary>
        /// <param name="cookieResult">如果存在Cookie时，请求主页的返回结果，如果不存在Cookie，则为空</param>
        /// <param name="otpCode">二级验证Code</param>
        /// <returns></returns>
        protected virtual Tuple<string, HttpWebRequest, HttpWebResponse> DoLoginPostWithOutCookie(Tuple<string, HttpWebRequest, HttpWebResponse> cookieResult)
        {
            //如果前面Cookie登录没有成功，则下面尝试没有Cookie的情况。
            string otpCode = string.Empty;
            if (Site.isEnableTwo_StepVerification && User.isEnableTwo_StepVerification)
            {
                OnTwoStepVerificationEventArgs e = new OnTwoStepVerificationEventArgs();
                e.Site = Site;
                otpCode = OnTwoStepVerification(e);
                return DoLoginWhenEnableTwo_StepVerification(cookieResult, otpCode);
            }
            else if (Site.IsEnableVerificationCode)
            {
                return DoLoginWhenEnableVerificationCode(cookieResult);
            }
            else
            {
                string postData = string.Format("username={0}&password={1}", User.UserName, User.PassWord);
                return HttpUtils.PostData(Site.LoginUrl, postData, _cookie);
            }
        }

        /// <summary>
        /// 二级验证登录（这里以馒头为模板）
        /// </summary>
        /// <param name="cookieResult"></param>
        /// <param name="otpCode"></param>
        /// <returns></returns>
        protected virtual Tuple<string, HttpWebRequest, HttpWebResponse> DoLoginWhenEnableTwo_StepVerification(Tuple<string, HttpWebRequest, HttpWebResponse> cookieResult, string otpCode)
        {
            string postData = string.Format("username={0}&password={1}", User.UserName, User.PassWord);
            var result = HttpUtils.PostData(Site.LoginUrl, postData, _cookie);

            //如果返回的页面是二级验证页面，则继续请求
            if (result.Item3 != null && result.Item3.ResponseUri.OriginalString.Contains("verify"))
            {
                if (otpCode.IsNullOrEmptyOrWhiteSpace())
                {
                    throw new Exception("无法获取到正确的二级验证码，请重新尝试。");
                }
                else
                {
                    postData = string.Format("otp={0}", otpCode);
                    _cookie = result.Item2.CookieContainer;
                    return result = HttpUtils.PostData(result.Item3.ResponseUri.OriginalString, postData, _cookie);
                }
            }
            else
                return result;
        }

        /// <summary>
        /// 验证码登录（这里以FRDS为模板）
        /// </summary>
        /// <param name="cookieResult"></param>
        /// <param name="otpCode"></param>
        /// <returns></returns>
        protected virtual Tuple<string, HttpWebRequest, HttpWebResponse> DoLoginWhenEnableVerificationCode(Tuple<string, HttpWebRequest, HttpWebResponse> cookieResult)
        {
            string htmlResult = string.Empty;
            //这里先看有没有前面是不是有过请求了，如果有的话，那么直接在这里获取验证码，如果没有，则自己获取。
            if (cookieResult != null && !cookieResult.Item1.IsNullOrEmptyOrWhiteSpace())
                htmlResult = cookieResult.Item1;
            else
                htmlResult = HttpUtils.GetData(Site.Url, _cookie).Item1;

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlResult);
            HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode(".//table//tr/td/img");
            string checkCodeKey = string.Empty;
            string checkCodeHash = string.Empty;
            if (node != null)
            {
                string imgUrl = HttpUtility.HtmlDecode(node.Attributes["src"].Value);
                if (imgUrl.IsNullOrEmptyOrWhiteSpace())
                    return new Tuple<string, HttpWebRequest, HttpWebResponse>("无法获取到验证码，登录失败，请稍后重试。", null, null);

                imgUrl = UrlUtils.CombileUrl(Site.Url, imgUrl);
                OnVerificationCodeEventArgs args = new OnVerificationCodeEventArgs();
                args.VerificationCodeUrl = imgUrl;
                args.Site = Site;
                checkCodeKey = OnVerificationCode(args);
                checkCodeHash = imgUrl.UrlSearchKey("imagehash");
                if (checkCodeKey.IsNullOrEmptyOrWhiteSpace() || checkCodeHash.IsNullOrEmptyOrWhiteSpace())
                    return new Tuple<string, HttpWebRequest, HttpWebResponse>("无法获取到验证码，登录失败，请稍后重试。", null, null);

                string postData = string.Format("username={0}&password={1}&imagestring={2}&imagehash={3}", User.UserName, User.PassWord, checkCodeKey, checkCodeHash);
                if (new Uri(Site.LoginUrl).Scheme == "https")
                    postData += string.Format("&ssl=yes&trackerssl=yes");

                //这里兼容某些站会有XSS的情况。
                var xssNode = htmlDocument.DocumentNode.SelectSingleNode("//form/input[contains(concat(' ', normalize-space(@name), ' '), ' xss ')]");
                if (xssNode != null && xssNode.Attributes.Contains("value"))
                {
                    string xssValue = xssNode.Attributes["value"].Value;
                    if (!xssValue.IsNullOrEmptyOrWhiteSpace())
                        postData = string.Format("xss={0}&", xssValue) + postData;
                }

                return HttpUtils.PostData(Site.LoginUrl, postData, _cookie);
            }
            else
            {
                return new Tuple<string, HttpWebRequest, HttpWebResponse>("无法获取到验证码，登录失败，请稍后重试。", null, null);
            }
        }

        protected bool IsLoginSuccess(string htmlResult)
        {
            if (!htmlResult.Contains("登录失败") && htmlResult.Contains(User.UserName) && (htmlResult.Contains("欢迎回来") || htmlResult.Contains("Welcome") || htmlResult.Contains("歡迎回來")))
                return true;
            else
                return false;
        }

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

        #endregion

        /// <summary>
        /// 获取用户ID
        /// </summary>
        /// <param name="htmlResult"></param>
        /// <returns></returns>
        protected virtual int GetUserId(string htmlResult)
        {
            int id = 0;
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.OptionOutputAsXml = false;
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
        public virtual string Sign()
        {
            if (Site.SignUrl.IsNullOrEmptyOrWhiteSpace())
                return "该站点并没有签到，无能为力啊。";
            return "签到尚未实现，革命仍需努力。";
        }

        #region 种子

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
            if (trNodes == null || trNodes.Count <= 1)
                throw new Exception(string.Format("{0}无法搜索到对应结果，请尝试更换其他关键字。", Site.Name));
            else
            {
                List<PTTorrent> torrents = new List<PTTorrent>();

                var torrentMaps = GetTorrentMaps(trNodes[0].SelectNodes(".//td[contains(concat(' ', normalize-space(@class), ' '), ' colhead ')]"));

                for (int i = 1; i < trNodes.Count; i++)
                {
                    var trNode = trNodes[i];
                    var tdNodes = GetTorrentNodes(trNode);
                    if (tdNodes == null || tdNodes.Count < 8)
                        continue;
                    else
                    {
                        PTTorrent torrent = new PTTorrent();
                        torrent.SiteId = SiteId;
                        torrent.SiteName = Site.Name;

                        //tdNodes[1]为种子信息
                        //种子链接和标题是最重要的，如果这里拿不到，直接跳过了
                        if (!SetTorrentTitleAndLink(tdNodes[torrentMaps[YUEnums.TorrentMap.Detail]], torrent))
                            continue;

                        //这里的副标题如果没有的话，是否需要跳过?暂时先保留把。
                        SetTorrentSubTitle(tdNodes[torrentMaps[YUEnums.TorrentMap.Detail]], torrent);

                        SetTorrentPromotionType(tdNodes[torrentMaps[YUEnums.TorrentMap.Detail]], torrent);

                        if (torrent.PromotionType == YUEnums.PromotionType.FREE || torrent.PromotionType == YUEnums.PromotionType.FREE2UP)
                            SetTorrentFreeTime(tdNodes[torrentMaps[YUEnums.TorrentMap.Detail]], torrent);

                        SetTorrentHR(tdNodes[torrentMaps[YUEnums.TorrentMap.Detail]], torrent);

                        SetTorrentOtherInfo(torrentMaps, tdNodes, torrent);

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
            return htmlDocument.DocumentNode.SelectNodes("//table[contains(concat(' ', normalize-space(@class), ' '), ' torrents ')]//tr");
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
                    torrent.Title = HttpUtility.HtmlDecode(titleNode.Attributes["title"].Value);
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
            }
            if (torrent.Subtitle.IsNullOrEmptyOrWhiteSpace())
            {
                subNode = node.SelectSingleNode(".//td[1]/br");
                if (subNode != null && subNode.NextSibling != null)
                    torrent.Subtitle = HttpUtility.HtmlDecode(subNode.NextSibling.InnerText);
            }
            return !torrent.Subtitle.IsNullOrEmptyOrWhiteSpace();
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

            if (html.Contains("hit_run") || html.Contains("hitandrun"))
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

        protected virtual void SetTorrentFreeTime(HtmlNode node, PTTorrent torrent)
        {
            var freeNode = node.SelectSingleNode(".//td[contains(concat(' ', normalize-space(@class), ' '), ' embedded ')]//span[not(@class)][last()]");

            if (freeNode != null && !freeNode.InnerText.IsNullOrEmptyOrWhiteSpace())
            {
                torrent.FreeTime = "剩余：" + freeNode.InnerText;
            }
            else
            {
                freeNode = node.SelectSingleNode(".//td[contains(concat(' ', normalize-space(@class), ' '), ' embedded ')]//*[contains(normalize-space(@class), 'free')]  ");
                if (freeNode != null && !freeNode.OuterHtml.IsNullOrEmptyOrWhiteSpace())
                {
                    string html = HttpUtility.HtmlDecode(freeNode.OuterHtml);
                    var index = html.LastIndexOf("<span");
                    var lastIndex = html.LastIndexOf("</span>");
                    if (index > 0 && lastIndex > 0)
                    {
                        string span = html.Substring(index, lastIndex - index);
                        HtmlDocument htmlDocument = new HtmlDocument();
                        htmlDocument.LoadHtml(span);
                        if (htmlDocument.DocumentNode != null && htmlDocument.DocumentNode.FirstChild != null)
                            torrent.FreeTime = "剩余：" + htmlDocument.DocumentNode.FirstChild.InnerText;
                    }
                }
            }
        }


        /// <summary>
        /// 设置其他信息
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        protected virtual void SetTorrentOtherInfo(Dictionary<YUEnums.TorrentMap, int> torrentMaps, HtmlNodeCollection nodes, PTTorrent torrent)
        {
            //设置资源类型 
            var imgNode = nodes[torrentMaps[YUEnums.TorrentMap.ResourceType]].SelectSingleNode(".//img");
            if (imgNode != null && imgNode.Attributes.Contains("alt"))
            {
                torrent.ResourceType = imgNode.Attributes["alt"].Value;
            }

            HtmlNode timeNode = nodes[torrentMaps[YUEnums.TorrentMap.TimeAlive]].SelectSingleNode(".//span");
            if (timeNode != null && timeNode.Attributes.Contains("title"))
                torrent.UpLoadTime = timeNode.Attributes["title"].Value.TryPareValue<DateTime>();
            else
            {
                string uploadTimeStr = nodes[torrentMaps[YUEnums.TorrentMap.TimeAlive]].InnerText;
                if (!uploadTimeStr.IsNullOrEmptyOrWhiteSpace() && uploadTimeStr.Length >= 18)
                {
                    string dateStr = uploadTimeStr.Substring(0, 10);
                    string timeStr = uploadTimeStr.Substring(10);
                    torrent.UpLoadTime = string.Format("{0} {1}", dateStr, timeStr).TryPareValue<DateTime>();
                }
            }
            torrent.Size = nodes[torrentMaps[YUEnums.TorrentMap.Size]].InnerText;
            torrent.SeederNumber = nodes[torrentMaps[YUEnums.TorrentMap.SeederNumber]].InnerText.TryPareValue<int>();
            torrent.LeecherNumber = nodes[torrentMaps[YUEnums.TorrentMap.LeecherNumber]].InnerText.TryPareValue<int>();
            torrent.SnatchedNumber = nodes[torrentMaps[YUEnums.TorrentMap.SnatchedNumber]].InnerText.TryPareValue<int>();
            torrent.UpLoader = nodes[torrentMaps[YUEnums.TorrentMap.UpLoader]].InnerText;
        }


        /// <summary>
        /// 根据行头获取用户信息列映射
        /// </summary>
        /// <param name="headNodes"></param>
        /// <returns></returns>
        protected virtual Dictionary<YUEnums.TorrentMap, int> GetTorrentMaps(HtmlNodeCollection headNodes)
        {
            try
            {
                Dictionary<YUEnums.TorrentMap, int> torrentMaps = new Dictionary<YUEnums.TorrentMap, int>();

                //这里保证torrentMaps包含所有值。
                foreach (var map in Site.TorrentMaps)
                {
                    torrentMaps.Add(map.Key, 0);
                }

                if (headNodes != null && headNodes.Count > 0)
                {
                    for (int i = 0; i < headNodes.Count; i++)
                    {
                        var headNode = headNodes[i];
                        foreach (var map in Site.TorrentMaps)
                        {
                            foreach (var item in map.Value)
                            {
                                if (headNode.InnerHtml.Contains(item))
                                    torrentMaps[map.Key] = i;
                            }
                        }
                    }
                }
                return torrentMaps;
            }
            catch (Exception ex)
            {
                Logger.Error("获取种子列映射失败，请检查配置文件。", ex);
                throw new Exception("获取种子列映射失败，请检查配置文件。");
            }
        }

        #endregion

        /// <summary>
        /// 从本地文件中读取Cookie
        /// </summary>
        /// <returns></returns>
        protected CookieContainer GetLocalCookie()
        {
            string cookiePath = GetCookieFilePath();
            string cookie = YUUtils.ReadCookiesFromDisk(cookiePath);
            return YUUtils.GetContainerFromCookie(cookie, new Uri(Site.Url));
        }

        /// <summary>
        /// 将Cookie写入本地文件
        /// </summary>
        /// <param name="_cookie"></param>
        protected void SetLocalCookie(CookieContainer _cookie)
        {
            string cookiePath = GetCookieFilePath();
            YUUtils.WriteCookiesToDisk(cookiePath, YUUtils.GetCookieFromContainer(_cookie, new Uri(Site.Url)));
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

        #region 下载

        public void DownTorrent(PTTorrent torrent, bool isOpen, bool isPostFileName)
        {
            if (torrent != null)
            {
                string filefullPath = string.Empty;
                string fileName = string.Format("[{0}].{1}.{2}", SiteId, torrent.Title.Trim(), "torrent");
                try
                {
                    var url = torrent.DownUrl;

                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebRequest.Method = "POST";
                    //使用用IE9下载（解决中文乱码问题）
                    httpWebRequest.UserAgent = IsUseIEDownload() ? YUConst.HTTP_IE9_UA : YUConst.HTTP_CHROME_UA;
                    httpWebRequest.Timeout = 10000;
                    httpWebRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*;q=0.8";
                    httpWebRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                    httpWebRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh");
                    httpWebRequest.Headers.Add(HttpRequestHeader.Cookie, YUUtils.GetCookieFromContainer(_cookie, new Uri(url)));
                    httpWebRequest.AllowAutoRedirect = false;
                    HttpWebResponse webRespon = (HttpWebResponse)httpWebRequest.GetResponse();

                    if (!webRespon.GetResponseHeader("Location").IsNullOrEmptyOrWhiteSpace())
                        throw new Exception("下载种子失败，也许是二次验证等原因导致，请尝试关闭。");

                    //如果需要请求服务器文件名的话
                    if (isPostFileName)
                    {
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
                        else
                        {
                            Uri uri = new Uri(url);
                            if (uri.Segments != null && uri.Segments.Length > 0)
                            {
                                if (uri.Segments.LastOrDefault().Contains(".torrent"))
                                    fileName = HttpUtility.UrlDecode(uri.Segments.LastOrDefault());
                            }
                        }
                    }

                    foreach (var car in Path.GetInvalidFileNameChars())
                    {
                        if (fileName.Contains(car))
                            fileName = fileName.Replace(car, '_');
                    }

                    Stream webStream = webRespon.GetResponseStream();
                    if (webStream == null)
                    {
                        throw new ArgumentNullException("网络错误(Network error)");
                    }

                    filefullPath = GetDownFileFullPath(fileName);
                    if (filefullPath.IsNullOrEmptyOrWhiteSpace())
                        return;

                    if (File.Exists(filefullPath))
                        File.Delete(filefullPath);

                    string fileDirectory = System.IO.Path.GetDirectoryName(filefullPath);

                    if (!Directory.Exists(fileDirectory))
                        Directory.CreateDirectory(fileDirectory);

                    int size = 1024;
                    FileStream fs = new FileStream(filefullPath, FileMode.Create);
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
                        System.Diagnostics.Process.Start(filefullPath);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("文件[{0}]下载失败，失败原因：{1}", filefullPath, ex.GetInnerExceptionMessage()), ex);
                }
            }
            else
                throw new Exception("下载种子失败，失败原因：无法获取到种子信息。");
        }

        protected virtual bool IsUseIEDownload()
        {
            return true;
        }

        /// <summary>
        /// 获取用户输入的文件路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetDownFileFullPath(string fileName)
        {
            OnPrepareDownFileEventArgs e = new OnPrepareDownFileEventArgs();
            e.FileName = fileName;
            return OnPrepareDownFile(e);
        }

        /// <summary>
        /// 触发下载文件事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public virtual string OnPrepareDownFile(OnPrepareDownFileEventArgs e)
        {
            if (PrepareDownFile != null)
            {
                return PrepareDownFile.Invoke(this, e);
            }
            return string.Empty;
        }

        #endregion

        public virtual PTInfo GetPersonInfo()
        {
            PTInfo info = new PTInfo();
            if (User.Id == 0)
            {
                string htmlResult = HttpUtils.GetDataGetHtml(Site.Url, _cookie);
                int id = GetUserId(htmlResult);
                if (id == 0)
                    throw new Exception(string.Format("{0} 无法获取用户ID，请尝试重新登录。", Site.Name));
                else
                {
                    User.Id = id;
                    return GetPersonInfo();
                }
            }
            else
            {
                string url = string.Format(Site.InfoUrl, User.Id);
                info.Url = url;
                string htmlResult = HttpUtils.GetDataGetHtml(url, _cookie);

                HtmlDocument htmlDocument = new HtmlDocument();
                //某些站点的HTML可能不规范，导致获取信息失败，这里OptionAutoCloseOnEnd设为True
                htmlDocument.OptionAutoCloseOnEnd = true;
                htmlDocument.LoadHtml(htmlResult);//加载HTML字符串，如果是文件可以用htmlDocument.Load方法加载

                HtmlNodeCollection headNodes =
                   htmlDocument.DocumentNode.SelectNodes("//table[contains(concat(' ', normalize-space(@class), ' '), ' main ')]//td[contains(concat(' ', normalize-space(@align), ' '), ' right ')]");



                if (headNodes == null || headNodes.Count <= 0)
                    throw new Exception(string.Format("{0} 获取用户详细信息失败，请稍后重试。", Site.Name));

                //根据行头获取映射
                var infoMaps = GetInfoMaps(headNodes);

                HtmlNodeCollection nodes =
                    htmlDocument.DocumentNode.SelectNodes("//table[contains(concat(' ', normalize-space(@class), ' '), ' main ')]//td[contains(concat(' ', normalize-space(@align), ' '), ' left ')]");
                if (nodes == null || nodes.Count <= 0)
                    throw new Exception(string.Format("{0} 获取用户详细信息失败，请稍后重试。", Site.Name));
                else
                {
                    #region Convert
                    //注册日期
                    var node = nodes[infoMaps[YUEnums.PersonInfoMap.RegisterDate]];
                    if (node != null)
                    {
                        node = node.SelectSingleNode(".//span");
                        if (node != null && node.Attributes.Contains("title"))
                        {
                            info.RegisterDate = node.Attributes["title"].Value.TryPareValue<DateTime>();
                        }
                    }

                    //分享率
                    node = nodes[infoMaps[YUEnums.PersonInfoMap.ShareRate]];
                    if (node != null)
                    {
                        var childNode = node.SelectSingleNode(".//td/font/text()");
                        if (childNode != null)
                            info.ShareRate = childNode.InnerText;
                    }

                    //上传量
                    node = nodes[infoMaps[YUEnums.PersonInfoMap.UpSize]];
                    if (node != null)
                    {
                        var childNode = node.SelectSingleNode(".//tr[2]/td/text()[last()]");
                        if (childNode != null)
                        {
                            var index = childNode.InnerText.IndexOf(":");
                            if (index > -1)
                                info.UpSize = childNode.InnerText.Substring(index + 1).Trim();
                        }
                    }

                    //下载量
                    node = nodes[infoMaps[YUEnums.PersonInfoMap.DownSize]];
                    if (node != null)
                    {
                        var childNode = node.SelectSingleNode(".//tr[2]/td[2]/text()[2]");
                        if (childNode != null)
                        {
                            var index = childNode.InnerText.IndexOf(":");
                            if (index > -1)
                                info.DownSize = childNode.InnerText.Substring(index + 1).Trim();
                        }
                    }

                    //做种率
                    node = nodes[infoMaps[YUEnums.PersonInfoMap.SeedRate]];
                    if (node != null)
                    {
                        var childNode = node.SelectSingleNode(".//td/font/text()");
                        if (childNode != null)
                            info.SeedRate = childNode.InnerText;
                    }

                    //做种时间
                    node = nodes[infoMaps[YUEnums.PersonInfoMap.SeedTimes]];
                    if (node != null)
                    {
                        var childNode = node.SelectSingleNode(".//tr[2]/td/text()[last()]");
                        if (childNode != null)
                        {
                            var index = childNode.InnerText.IndexOf(":");
                            if (index > -1)
                                info.SeedTimes = childNode.InnerText.Substring(index + 1).Trim();
                        }
                    }

                    //下载时间
                    node = nodes[infoMaps[YUEnums.PersonInfoMap.DownTimes]];
                    if (node != null)
                    {
                        var childNode = node.SelectSingleNode(".//tr[2]/td[2]/text()[last()]");
                        if (childNode != null)
                        {
                            var index = childNode.InnerText.IndexOf(":");
                            if (index > -1)
                                info.DownTimes = childNode.InnerText.Substring(index + 1).Trim();
                        }
                    }

                    //等级
                    node = nodes[infoMaps[YUEnums.PersonInfoMap.Rank]];
                    if (node != null)
                    {
                        var childNode = node.SelectSingleNode("./img");
                        if (childNode != null && childNode.Attributes.Contains("src"))
                        {
                            string srcImg = childNode.Attributes["src"].Value;
                            foreach (var item in PTSiteConst.CLASSIMGS)
                            {
                                if (srcImg.IndexOf(item.Key, StringComparison.OrdinalIgnoreCase) > 0)
                                {
                                    info.Rank = item.Value;
                                    break;
                                }
                            }
                            if (info.Rank.IsNullOrEmptyOrWhiteSpace() && childNode.Attributes.Contains("alt"))
                            {
                                info.Rank = childNode.Attributes["alt"].Value;
                            }
                        }
                    }

                    //积分
                    node = nodes[infoMaps[YUEnums.PersonInfoMap.Bonus]];
                    if (node != null)
                    {
                        info.Bonus = node.InnerText.TryPareValue<double>();
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

        /// <summary>
        /// 根据行头获取用户信息列映射
        /// </summary>
        /// <param name="headNodes"></param>
        /// <returns></returns>
        protected virtual Dictionary<YUEnums.PersonInfoMap, int> GetInfoMaps(HtmlNodeCollection headNodes)
        {
            try
            {
                Dictionary<YUEnums.PersonInfoMap, int> infoMaps = new Dictionary<YUEnums.PersonInfoMap, int>();

                //这里保证infoMaps包含所有值。
                foreach (var map in Site.PersonInfoMaps)
                {
                    infoMaps.Add(map.Key, 0);
                }

                if (headNodes != null && headNodes.Count > 0)
                {
                    for (int i = 0; i < headNodes.Count; i++)
                    {
                        var headNode = headNodes[i];
                        foreach (var map in Site.PersonInfoMaps)
                        {
                            if (map.Value.Contains(headNode.InnerText.Trim()))
                            {
                                infoMaps[map.Key] = i;
                            }
                        }
                    }
                }
                return infoMaps;
            }
            catch (Exception ex)
            {
                Logger.Error("获取用户行映射失败，请检查配置文件。", ex);
                throw new Exception("获取用户行映射失败，请检查配置文件。");
            }
        }

    }
}
