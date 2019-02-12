using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using YU.Core;
using YU.Core.DataEntity;
using YU.Core.Event;
using YU.Core.Log;
using YU.Core.Utils;

namespace YU.PT
{

    public class HDCHINA : AbstractPT
    {
        public HDCHINA(PTUser user)
            : base(user)
        {

        }

        protected override YUEnums.PTEnum SiteId
        {
            get
            {
                return YUEnums.PTEnum.HDChina;
            }
        }

        /// <summary>
        /// 根据行头获取用户信息列映射
        /// </summary>
        /// <param name="headNodes"></param>
        /// <returns></returns>
        protected override Dictionary<YUEnums.TorrentMap, int> GetTorrentMaps(HtmlNodeCollection headNodes)
        {
            try
            {
                Dictionary<YUEnums.TorrentMap, int> torrentMaps = new Dictionary<YUEnums.TorrentMap, int>
                {
                    { YUEnums.TorrentMap.ResourceType, 0 },
                    { YUEnums.TorrentMap.Detail, 1 },
                    { YUEnums.TorrentMap.TimeAlive, 3 },
                    { YUEnums.TorrentMap.Size, 4 },
                    { YUEnums.TorrentMap.SeederNumber, 5 },
                    { YUEnums.TorrentMap.LeecherNumber, 6 },
                    { YUEnums.TorrentMap.SnatchedNumber, 7 },
                    { YUEnums.TorrentMap.UpLoader, 8 },
                    { YUEnums.TorrentMap.PromotionType, 1 }
                };
                return torrentMaps;
            }
            catch (Exception ex)
            {
                Logger.Error("获取种子列映射失败，请检查配置文件。", ex);
                throw new Exception("获取种子列映射失败，请检查配置文件。");
            }
        }

        protected override HtmlNode GetUserNode(string htmlResult)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.OptionOutputAsXml = false;
            htmlDocument.LoadHtml(htmlResult);//加载HTML字符串，如果是文件可以用htmlDocument.Load方法加载

            HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"site_header\"]/div[contains(concat(' ', normalize-space(@class), ' '), ' userinfo ')]/p/span/a");//跟Xpath一样
                                                                                                                                                                                   //这里不用User_Name判断，因为不同等级的User，Class也会不一样。
                                                                                                                                                                                   //HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode("//a[contains(concat(' ', normalize-space(@class), ' '), ' User_Name ')]");
            return node;
        }

        public override PTInfo GetPersonInfo()
        {

            PTInfo info = new PTInfo();

            if (_cookie == null || _cookie.Count <= 0)
                throw new Exception(string.Format("{0} 获取Cookie信息失败，请尝试重新登录。", Site.Name));

            if (User.UserId == 0)
            {
                string htmlResult = HttpUtils.GetDataGetHtml(Site.Url, _cookie);
                UpdateUserWhileChange(htmlResult, User);
                if (User.UserId == 0)
                    throw new Exception(string.Format("{0} 无法获取用户ID，请尝试重新登录。", Site.Name));
                else
                    return GetPersonInfo();
            }
            else
            {
                string url = string.Format(Site.InfoUrl, User.UserId);
                info.Url = url;
                string htmlResult = HttpUtils.GetDataGetHtml(url, _cookie);

                if (HttpUtils.IsErrorRequest(htmlResult))
                    throw new Exception(string.Format("{0} 获取用户详细信息失败，请稍后重试。原因：{1}", Site.Name, htmlResult));

                HtmlDocument htmlDocument = new HtmlDocument();
                //某些站点的HTML可能不规范，导致获取信息失败，这里OptionAutoCloseOnEnd设为True
                htmlDocument.OptionAutoCloseOnEnd = true;
                htmlDocument.LoadHtml(htmlResult);//加载HTML字符串，如果是文件可以用htmlDocument.Load方法加载

                PreSetPersonInfo(htmlDocument, info);

                HtmlNodeCollection headNodes = htmlDocument.DocumentNode.SelectNodes("//table[contains(concat(' ', normalize-space(@class), ' '), ' normal_tab ')]//td[contains(concat(' ', normalize-space(@class), ' '), ' rowhead ')]");

                if (headNodes == null || headNodes.Count <= 0)
                    throw new Exception(string.Format("{0} 获取用户详细信息失败，请稍后重试。", Site.Name));

                //根据行头获取映射
                var infoMaps = GetInfoMaps(headNodes);

                HtmlNodeCollection nodes = htmlDocument.DocumentNode.SelectNodes("//table[contains(concat(' ', normalize-space(@class), ' '), ' normal_tab ')]//td[contains(concat(' ', normalize-space(@class), ' '), ' rowfollow ')]");
                if (nodes == null || nodes.Count <= 0)
                    throw new Exception(string.Format("{0} 获取用户详细信息失败，请稍后重试。", Site.Name));
                else
                    SetPersonInfo(infoMaps, nodes, info);

                return info;

            }
        }

        protected override void PreSetPersonInfo(HtmlDocument htmlDocument, PTInfo info)
        {
            //做种数
            var node = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(concat(' ', normalize-space(@class), ' '), ' userinfo ')]//i[contains(concat(' ', normalize-space(@class), ' '), ' fa-arrow-up ')]");
            if (node != null && node.PreviousSibling != null)
                info.SeedNumber = node.NextSibling.InnerText.Trim().TryPareValue<string>();
        }

        /// <summary>
        /// 设置种子Id，链接和标题
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override bool SetTorrentTitleAndLink(HtmlNode node, PTTorrent torrent)
        {

            var titleNode = node.SelectSingleNode(".//td/h3/a");
            var downNode = node.SelectSingleNode(".//td[contains(concat(' ', normalize-space(@class), ' '), ' act ')]/a[1]");
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

        /// <summary>
        /// 设置副标题
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override bool SetTorrentSubTitle(HtmlNode node, PTTorrent torrent)
        {
            var subNode = node.SelectSingleNode(".//td/h4");
            if (subNode != null)
            {
                if (torrent.Subtitle.IsNullOrEmptyOrWhiteSpace())
                    torrent.Subtitle = HttpUtility.HtmlDecode(subNode.InnerText);
            }
            return !torrent.Subtitle.IsNullOrEmptyOrWhiteSpace();
        }

        /// <summary>
        /// 获取所有种子节点
        /// </summary>
        /// <param name="htmlDocument"></param>
        /// <returns></returns>
        protected override HtmlNodeCollection GetTorrentNodes(HtmlDocument htmlDocument)
        {
            return htmlDocument.DocumentNode.SelectNodes("//table[contains(concat(' ', normalize-space(@class), ' '), ' torrent_list ')]//tr");
        }

        public override string Sign(bool isAuto = false)
        {
            if (_cookie != null && _cookie.Count > 0)
            {
                var resultJson = JsonConvert.DeserializeObject<JObject>(HttpUtils.PostData(Site.SignUrl, "{}", _cookie).Item1);
                switch (resultJson.Value<string>("state"))
                {
                    case "success":
                        return $"签到成功，您已连续签到{resultJson.Value<string>("signindays")}天，本次增加魔力:{resultJson.Value<string>("integral")}。";
                    default:
                        if (!resultJson.Value<string>("msg").IsNullOrEmptyOrWhiteSpace())
                            return resultJson.Value<string>("msg");
                        else
                            return "签到失败";
                }
            }
            else
            {
                return "无法获取Cookie信息，签到失败，请重新登录系统。";
            }
        }

        public override string Login()
        {
            return "由于HDChina启用了人机验证码，程序无法处理，请自行录入Cookie使用。";
        }

    }
}
