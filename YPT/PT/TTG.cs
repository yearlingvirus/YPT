using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using YU.Core;
using YU.Core.DataEntity;
using YU.Core.Event;
using YU.Core.Log;
using YU.Core.Utils;

namespace YPT.PT
{
    public class TTG : AbstractPT
    {
        public TTG(PTUser user)
            : base(user)
        {

        }

        protected override YUEnums.PTEnum SiteId
        {
            get
            {
                return YUEnums.PTEnum.TTG;
            }
        }


        protected override Tuple<string, HttpWebRequest, HttpWebResponse> DoLoginPostWithOutCookie(Tuple<string, HttpWebRequest, HttpWebResponse> cookieResult)
        {
            //如果前面Cookie登录没有成功，则下面尝试没有Cookie的情况。
            string otpCode = string.Empty;

            StringBuilder sb = new StringBuilder();
            Dictionary<string, string> postDict = new Dictionary<string, string>();
            postDict.Add("username", User.UserName);
            postDict.Add("password", User.PassWord);
            postDict.Add("otp", "");

            //启用了二级验证
            if (Site.isEnableTwo_StepVerification && User.isEnableTwo_StepVerification)
            {
                OnTwoStepVerificationEventArgs e = new OnTwoStepVerificationEventArgs();
                e.Site = Site;
                otpCode = OnTwoStepVerification(e);
                postDict["otp"] = otpCode;
            }

            //启用了安全提问
            if (Site.IsEableSecurityQuestion)
            {
                if (!(User.SecurityQuestionOrder == -1 || User.SecuityAnswer.IsNullOrEmptyOrWhiteSpace()))
                {
                    postDict.Add("passan", User.SecuityAnswer);
                    postDict.Add("passid", User.SecurityQuestionOrder.TryPareValue<string>());
                }
                else
                {
                    postDict.Add("passan", "");
                    postDict.Add("passid", "0");
                }
            }

            postDict.Add("lang", "0");
            postDict.Add("rememberme", "yes");
            foreach (var item in postDict)
            {
                sb.AppendLine(string.Format("------{0}", YUConst.POST_BOUNDARY));
                sb.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0}\"", item.Key));
                sb.AppendLine("");
                sb.AppendLine(item.Value);
            }
            sb.AppendLine(string.Format("{0}--", YUConst.POST_BOUNDARY));

            string postData = sb.ToString();
            return HttpUtils.PostData(Site.LoginUrl, postData, _cookie, true);
        }



        public override string Sign()
        {
            if (_cookie != null && _cookie.Count > 0)
            {
                //TTG签到需要时间戳和TOKEN，所以这里需要用Cookie请求一下网页拿到
                string htmlResult = HttpUtils.PostDataGetHtml(Site.Url, "", _cookie);
                int signStart = htmlResult.IndexOf("signed_timestamp");
                if (signStart != -1 && htmlResult.Length > (signStart + 81))
                {
                    string signJson = htmlResult.Substring(signStart - 1, 82);
                    string signed_timestamp = string.Empty;
                    string signed_token = string.Empty;
                    if (signJson.IsNullOrEmptyOrWhiteSpace())
                        return "无法获取签到Token";
                    try
                    {
                        JObject signO = JsonConvert.DeserializeObject<JObject>(signJson);
                        signed_timestamp = signO["signed_timestamp"].TryPareValue<string>();
                        signed_token = signO["signed_token"].TryPareValue<string>();
                    }
                    catch (Exception ex)
                    {
                        string errMsg = string.Format("TTG 签到失败，无法获取到正确的TOKEN信息，失败原因：{0}", ex.GetInnerExceptionMessage());
                        Logger.Error(errMsg, ex);
                        return ex.GetInnerExceptionMessage();
                    }

                    string postData = string.Format("signed_timestamp={0}&signed_token={1}", signed_timestamp, signed_token);
                    htmlResult = HttpUtils.PostDataGetHtml(Site.SignUrl, postData, _cookie);
                    return htmlResult;
                }
                else
                {
                    return "无法获取签到Token";
                }
            }
            else
            {
                return "无法获取Cookie信息，签到失败，请重新登录系统。";
            }
        }

        protected override HtmlNodeCollection GetTorrentNodes(HtmlDocument htmlDocument)
        {
            return htmlDocument.DocumentNode.SelectNodes("//*[@id=\"torrent_table\"]/tr");
        }

        protected override bool SetTorrentTitleAndLink(HtmlNode node, PTTorrent torrent)
        {
            if (node.ParentNode != null && node.ParentNode.Attributes.Contains("id"))
                torrent.Id = node.ParentNode.GetAttributeValue("id", string.Empty);

            var linkUrlNode = node.SelectSingleNode("./div[contains(concat(' ', normalize-space(@class), ' '), ' name_left ')]/a");
            if (linkUrlNode != null && linkUrlNode.Attributes.Contains("href"))
            {
                var linkUrl = HttpUtility.HtmlDecode(linkUrlNode.Attributes["href"].Value);
                torrent.LinkUrl = string.Join("/", Site.Url, linkUrl);

                var titleNode = linkUrlNode.SelectSingleNode("./b/text()");
                if (titleNode != null)
                    torrent.Title = HttpUtility.HtmlDecode(titleNode.InnerText);
                else
                {
                    titleNode = linkUrlNode.SelectSingleNode("./b/font/text()");
                    if (titleNode != null)
                        torrent.Title = HttpUtility.HtmlDecode(titleNode.InnerText);
                }

                var subNode = linkUrlNode.SelectSingleNode("./b//span[last()]");
                if (subNode != null)
                {
                    string subTtile = subNode.InnerText;
                    if (subNode.NextSibling != null)
                    {
                        subTtile += subNode.NextSibling.InnerText;
                    }
                    torrent.Subtitle = HttpUtility.HtmlDecode(subTtile);
                }

            }

            var downUrlNode = node.SelectSingleNode("./div[contains(concat(' ', normalize-space(@class), ' '), ' name_right ')]//a");
            if (downUrlNode != null && downUrlNode.Attributes.Contains("href"))
                torrent.DownUrl  = UrlUtils.CombileUrl(Site.Url, HttpUtility.HtmlDecode(downUrlNode.GetAttributeValue("href", string.Empty)));

            if (torrent.Id.IsNullOrEmptyOrWhiteSpace() || torrent.DownUrl.IsNullOrEmptyOrWhiteSpace() || torrent.LinkUrl.IsNullOrEmptyOrWhiteSpace() || torrent.Title.IsNullOrEmptyOrWhiteSpace())
                return false;
            else
                return true;
        }

        protected override bool SetTorrentSubTitle(HtmlNode node, PTTorrent torrent)
        {
            //获取副标题在上面处理了，这里不再处理，以上面结果为准。
            if (torrent.Subtitle.IsNullOrEmptyOrWhiteSpace())
                return false;
            else
                return true;
        }

        protected override void SetTorrentOtherInfo(Dictionary<YUEnums.TorrentMap, int> torrentMaps, HtmlNodeCollection nodes, PTTorrent torrent)
        {
            //设置资源类型
            var imgNode = nodes[torrentMaps[YUEnums.TorrentMap.ResourceType]].SelectSingleNode(".//img");
            if (imgNode != null && imgNode.Attributes.Contains("alt"))
            {
                torrent.ResourceType = imgNode.Attributes["alt"].Value;
            }

            string uploadTimeStr = nodes[torrentMaps[YUEnums.TorrentMap.TimeAlive]].InnerText;
            if (!uploadTimeStr.IsNullOrEmptyOrWhiteSpace() && uploadTimeStr.Length >= 18)
            {
                string dateStr = uploadTimeStr.Substring(0, 10);
                string timeStr = uploadTimeStr.Substring(10);
                torrent.UpLoadTime = string.Format("{0} {1}", dateStr, timeStr).TryPareValue<DateTime>();
            }

            torrent.Size = nodes[torrentMaps[YUEnums.TorrentMap.Size]].InnerText;

            var seedNodes = nodes[torrentMaps[YUEnums.TorrentMap.SeederNumber]].SelectNodes("./b");
            if (seedNodes != null && seedNodes.Count > 0)
            {
                torrent.SeederNumber = seedNodes[0].InnerText.TryPareValue<int>();
                if (seedNodes.Count > 1)
                    torrent.LeecherNumber = seedNodes[1].InnerText.TryPareValue<int>();
            }

            var snatchNode = nodes[torrentMaps[YUEnums.TorrentMap.SnatchedNumber]].SelectSingleNode("./text()");
            if (snatchNode != null)
                torrent.SnatchedNumber = snatchNode.InnerText.TryPareValue<int>();


            torrent.UpLoader = nodes[torrentMaps[YUEnums.TorrentMap.UpLoader]].InnerText;
        }

        protected override string BuildSearchUrl(string searchKey, YUEnums.PromotionType promotionType = YUEnums.PromotionType.ALL, YUEnums.AliveType aliveType = YUEnums.AliveType.ALL, YUEnums.FavType favType = YUEnums.FavType.ALL)
        {
            //https://totheglory.im/browse.php?search_field=北京 +halfdown +30down +freeleech +incdead +onlydead +excl +hr&c=M
            //TTG看起来只支持5种情况的过滤，其他都认为是全部过滤。
            Dictionary<YUEnums.PromotionType, string> motionDict = new Dictionary<YUEnums.PromotionType, string>()
            {
                { YUEnums.PromotionType.FREE , " +freeleech"},
                { YUEnums.PromotionType.THIRTYPERDOWN , " +30down"},
                { YUEnums.PromotionType.HALFDOWN , " +halfdown"},
            };
            Dictionary<YUEnums.AliveType, string> aliveDict = new Dictionary<YUEnums.AliveType, string>()
            {
                { YUEnums.AliveType.ALL , " +incdead"},
                { YUEnums.AliveType.Dead , " +onlydead"},
            };

            string queryString = string.Format("?search_field={0}", Uri.EscapeDataString(searchKey));
            if (motionDict.ContainsKey(promotionType))
                queryString += Uri.EscapeDataString(motionDict[promotionType]);

            if (aliveDict.ContainsKey(aliveType))
                queryString += Uri.EscapeDataString(aliveDict[aliveType]);

            return Site.SearchUrl + queryString;
        }

        protected override Dictionary<YUEnums.TorrentMap, int> GetTorrentMaps(HtmlNodeCollection headNodes)
        {
            return new Dictionary<YUEnums.TorrentMap, int>()
            {
                { YUEnums.TorrentMap.ResourceType, 0},
                { YUEnums.TorrentMap.Detail, 1},
                { YUEnums.TorrentMap.PromotionType, 1},
                { YUEnums.TorrentMap.TimeAlive, 4},
                { YUEnums.TorrentMap.Size, 6},
                { YUEnums.TorrentMap.SeederNumber, 8},
                { YUEnums.TorrentMap.LeecherNumber, 8},
                { YUEnums.TorrentMap.SnatchedNumber, 7},
                { YUEnums.TorrentMap.UpLoader, 9},
            };
        }


        protected override int GetUserId(string htmlResult)
        {
            int id = 0;
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlResult);//加载HTML字符串，如果是文件可以用htmlDocument.Load方法加载
            HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode("//table/tr/td/span/b/a");//跟Xpath一样
            if (node != null)
            {
                var url = HttpUtility.HtmlDecode(node.Attributes["href"].Value);
                url = string.Join("/", Site.Url, url);
                id = url.UrlSearchKey("id").TryPareValue<int>();
            }
            return id;
        }


        public override PTInfo GetPersonInfo()
        {
            PTInfo info = new PTInfo();
            if (User.Id == 0)
            {
                string htmlResult = HttpUtils.GetDataGetHtml(Site.Url, _cookie);
                int id = GetUserId(htmlResult);
                if (id == 0)
                    throw new Exception(string.Format("{0} 无法获取用户Id，请尝试重新登录。", Site.Name));
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
                htmlDocument.LoadHtml(htmlResult);//加载HTML字符串，如果是文件可以用htmlDocument.Load方法加载


                HtmlNodeCollection headNodes =
                   htmlDocument.DocumentNode.SelectNodes("//table[contains(concat(' ', normalize-space(@class), ' '), ' main ')]//td[contains(concat(' ', normalize-space(@class), ' '), ' rowhead ')]");

                if (headNodes == null || headNodes.Count <= 0)
                    throw new Exception(string.Format("{0} 获取用户详细信息失败，请稍后重试。", Site.Name));

                //根据行头获取映射
                var infoMaps = GetInfoMaps(headNodes);

                HtmlNodeCollection nodes =
                    htmlDocument.DocumentNode.SelectNodes("//table[contains(concat(' ', normalize-space(@class), ' '), ' main ')]//td[contains(concat(' ', normalize-space(@class), ' '), ' embedded ')]//td[contains(concat(' ', normalize-space(@align), ' '), ' left ')]");//跟Xpath一样
                if (nodes == null || nodes.Count <= 0)
                    throw new Exception(string.Format("{0} 获取用户详细信息失败，请稍后重试。", Site.Name));
                else
                {
                    #region Convert
                    //注册日期
                    var node = nodes[infoMaps[YUEnums.PersonInfoMap.RegisterDate]];
                    if (node != null)
                    {
                        var childNode = node.SelectSingleNode("./text()");
                        if (childNode != null)
                            info.RegisterDate = childNode.InnerText.TryPareValue<DateTime>();
                    }

                    //分享率
                    node = nodes[infoMaps[YUEnums.PersonInfoMap.ShareRate]];
                    if (node != null)
                    {
                        var childNode = node.SelectSingleNode(".//font/text()");
                        if (childNode != null)
                            info.ShareRate = childNode.InnerText;
                    }

                    //上传量
                    node = nodes[infoMaps[YUEnums.PersonInfoMap.UpSize]];
                    if (node != null)
                    {
                        var childNode = node.SelectSingleNode("./text()");
                        if (childNode != null)
                            info.UpSize = childNode.InnerText;
                    }

                    //下载量
                    node = nodes[infoMaps[YUEnums.PersonInfoMap.DownSize]];
                    if (node != null)
                    {
                        var childNode = node.SelectSingleNode("./text()");
                        if (childNode != null)
                            info.DownSize = childNode.InnerText;
                    }

                    //做种率
                    node = nodes[infoMaps[YUEnums.PersonInfoMap.SeedRate]];
                    if (node != null)
                    {
                        var childNode = node.SelectSingleNode("./font/text()");
                        if (childNode != null)
                            info.SeedRate = childNode.InnerText;
                    }


                    //做种时间 下载时间
                    node = nodes[infoMaps[YUEnums.PersonInfoMap.DownTimes]];
                    if (node != null)
                    {
                        var childNode = node.SelectSingleNode("./text()[last()]");
                        if (childNode != null)
                        {
                            string[] arr = childNode.InnerText.Split('[', ']', ',', ':', '，');
                            info.DownTimes = arr[4];
                            info.SeedTimes = arr[2];
                        }
                    }

                    //等级
                    node = nodes[infoMaps[YUEnums.PersonInfoMap.Rank]];
                    if (node != null)
                    {
                        info.Rank = node.InnerText;
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

    }
}
