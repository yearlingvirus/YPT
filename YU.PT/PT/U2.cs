using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using YU.Core;
using YU.Core.DataEntity;
using YU.Core.Utils;
using System.Net;
using YU.Core.Event;
using Newtonsoft.Json;

namespace YU.PT
{
    public class U2 : AbstractPT
    {
        public U2(PTUser user) : base(user)
        {

        }

        protected override YUEnums.PTEnum SiteId
        {
            get
            {
                return YUEnums.PTEnum.U2;
            }
        }


        public override string Login()
        {
            Tuple<string, HttpWebRequest, HttpWebResponse> result = null;
            if (_cookie != null && _cookie.Count > 0)
            {
                result = HttpUtils.GetData(Site.Url, _cookie);
                if (HttpUtils.IsErrorRequest(result.Item1))
                    return result.Item1;
                if (IsLoginSuccess(result.Item3))
                {
                    UpdateUserWhileChange(result.Item1, User);
                    return "登录成功。";
                }
            }

            result = DoLoginWhenEnableVerificationCode(result);
            if (HttpUtils.IsErrorRequest(result.Item1))
                return result.Item1;

            if (result.Item2 != null)
            {
                var o = JsonConvert.DeserializeObject<Dictionary<string, object>>(result.Item1);
                if (o.ContainsKey("status") && o["status"].TryPareValue<string>().EqualIgnoreCase("redirect"))
                {
                    string url = o["redirect"].TryPareValue<string>();
                    result = HttpUtils.GetData(url, result.Item2.CookieContainer);
                    string htmlResult = result.Item1;
                    if (IsLoginSuccess(result.Item3))
                    {
                        _cookie = result.Item2.CookieContainer;
                        UpdateUserWhileChange(result.Item1, User);
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
                        return errMsg;
                    }
                }
                else
                {
                    return string.Format("登录失败，失败原因：{0}", result.Item1);
                }
            }
            else
                return string.Format("登录失败，失败原因：{0}", result.Item1);
        }

        protected override Tuple<string, HttpWebRequest, HttpWebResponse> DoLoginWhenEnableVerificationCode(Tuple<string, HttpWebRequest, HttpWebResponse> cookieResult, bool isAutoOrc = true)
        {
            if (cookieResult == null)
                cookieResult = HttpUtils.GetData(Site.Url, _cookie);

            _cookie = cookieResult != null && cookieResult.Item2 != null ? cookieResult.Item2.CookieContainer : _cookie;
            Random ran = new Random();
            string imgUrl = UrlUtils.CombileUrl(Site.Url, string.Format("captcha.php?sid={0}", ran.NextDouble()));
            OnVerificationCodeEventArgs args = new OnVerificationCodeEventArgs();
            args.VerificationCodeUrl = imgUrl;
            args.Cookie = _cookie;
            args.Site = Site;


            string checkCodeKey = OnVerificationCode(args);
            if (checkCodeKey.IsNullOrEmptyOrWhiteSpace())
                return new Tuple<string, HttpWebRequest, HttpWebResponse>("无法获取到验证码，登录失败，请稍后重试。", null, null);


            //if (Site.IsLoginByMail)
            //U2是否有使用用户名登录的接口?
            string postData = string.Format("login_type=email&login_ajax=1&username={0}&password={1}&captcha={2}&ssl=yes", HttpUtility.UrlEncode(User.Mail), HttpUtility.UrlEncode(User.PassWord), checkCodeKey);
            return HttpUtils.PostData(Site.LoginUrl, postData, _cookie);
        }

        protected override bool SetTorrentTitleAndLink(HtmlNode node, PTTorrent torrent)
        {
            var nodes = node.SelectNodes(".//td[contains(concat(' ', normalize-space(@class), ' '), ' embedded ')]//a");
            if (nodes != null && nodes.Count >= 2)
            {
                var titleNode = nodes[0];
                var downNode = nodes[1];
                if (titleNode != null && titleNode.Attributes.Contains("href"))
                {
                    var linkUrl = HttpUtility.HtmlDecode(titleNode.Attributes["href"].Value);
                    torrent.LinkUrl = string.Join("/", Site.Url, linkUrl);
                    torrent.Id = torrent.LinkUrl.UrlSearchKey("id");
                    torrent.Title = HttpUtility.HtmlDecode(titleNode.InnerText);
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


        protected override bool SetTorrentSubTitle(HtmlNode node, PTTorrent torrent)
        {
            var subNode = node.SelectSingleNode(".//span[contains(concat(' ', normalize-space(@class), ' '), ' tooltip ')]");
            if (subNode != null)
                torrent.Subtitle = subNode.InnerText;
            return !torrent.Subtitle.IsNullOrEmptyOrWhiteSpace();
        }


        protected override void SetTorrentFreeTime(HtmlNode node, PTTorrent torrent)
        {
            var freeNode = node.SelectSingleNode(".//td[contains(concat(' ', normalize-space(@class), ' '), ' embedded ')]//time");
            if (freeNode != null && !freeNode.InnerText.IsNullOrEmptyOrWhiteSpace())
                torrent.FreeTime = "剩余：" + HttpUtility.HtmlDecode(freeNode.InnerText.Replace("&shy;", ""));
        }


        protected override void SetTorrentOtherInfo(Dictionary<YUEnums.TorrentMap, int> torrentMaps, HtmlNodeCollection nodes, PTTorrent torrent)
        {
            //设置资源类型 
            torrent.ResourceType = nodes[torrentMaps[YUEnums.TorrentMap.ResourceType]].InnerText;

            HtmlNode timeNode = nodes[torrentMaps[YUEnums.TorrentMap.TimeAlive]].SelectSingleNode(".//time");
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
            torrent.UpLoader = "--";
        }

        protected override void PreSetPersonInfo(HtmlDocument htmlDocument, PTInfo info)
        {
            //做种数
            var node = htmlDocument.DocumentNode.SelectSingleNode("//img[contains(concat(' ', normalize-space(@alt), ' '), ' Torrents leeching ')]/preceding-sibling::a[1]");
            if (node != null)
                info.SeedNumber = node.InnerText.Trim().TryPareValue<string>();
        }


        protected override void SetPersonInfo(Dictionary<YUEnums.PersonInfoMap, int> infoMaps, HtmlNodeCollection nodes, PTInfo info)
        {

            #region Convert
            //注册日期
            var node = nodes[infoMaps[YUEnums.PersonInfoMap.RegisterDate]];
            if (node != null)
            {
                var childNode = node.SelectSingleNode(".//time");
                if (childNode != null)
                    info.RegisterDate = childNode.InnerText.TryPareValue<DateTime>();
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
                        info.UpSize = childNode.InnerText.Substring(index + 1).Trim().Replace("i", "");
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
                        info.DownSize = childNode.InnerText.Substring(index + 1).Trim().Replace("i", "");
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
                        info.SeedTimes = HttpUtility.HtmlDecode(childNode.InnerText.Substring(index + 1).Trim().Replace("&shy;", "").Replace("&nbsp;", ""));
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
                        info.DownTimes = HttpUtility.HtmlDecode(childNode.InnerText.Substring(index + 1).Trim().Replace("&shy;", "").Replace("&nbsp;", ""));
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
                var childNode = node.SelectSingleNode("./span[contains(concat(' ', normalize-space(@class), ' '), ' ucoin-notation ')]");
                if (childNode != null && childNode.Attributes.Contains("title"))
                    info.Bonus = childNode.Attributes["title"].Value.TryPareValue<double>();
            }

            info.LastSyncDate = DateTime.Now;
            info.UserId = User.UserId;
            info.SiteId = SiteId;
            info.SiteName = Site.Name;
            info.Name = User.UserName;

            #endregion
        }
    }
}
