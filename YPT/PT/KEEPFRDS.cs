using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using YPT.Forms;
using YU.Core;
using YU.Core.DataEntity;
using YU.Core.Event;
using YU.Core.Utils;

namespace YPT.PT
{
    public class KEEPFRDS : AbstractPT
    {
        public KEEPFRDS(PTUser user)
            : base(user)
        {

        }

        protected override YUEnums.PTEnum SiteId
        {
            get
            {
                return YUEnums.PTEnum.KEEPFRDS;
            }
        }

        protected override HtmlNodeCollection GetTorrentNodes(HtmlDocument htmlDocument)
        {
            return htmlDocument.DocumentNode.SelectNodes("//table[contains(concat(' ', normalize-space(@class), ' '), ' torrents ')]/form/tr");
        }

        protected override bool SetTorrentSubTitle(HtmlNode node, PTTorrent torrent)
        {
            var subNode = node.SelectSingleNode(".//td[1]/br");
            if (subNode != null && subNode.NextSibling != null)
                torrent.Subtitle = HttpUtility.HtmlDecode(subNode.NextSibling.InnerText);
            return false;
        }

        public override string Login()
        {
            var result = HttpUtils.PostData(Site.Url, "", _cookie);
            var htmlResult = result.Item1;
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlResult);//加载HTML字符串，如果是文件可以用htmlDocument.Load方法加载

            //这里用Cookie登录，有可能之前已经登录成功了，不会再次出现验证码
            if (htmlResult.Contains(User.UserName))
            {
                User.Id = GetUserId(htmlResult);
                return "登录成功。";
            }
            else
            {
                HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"nav_block\"]/form[2]/table/tr[3]/td[2]/img");//跟Xpath一样
                string checkCodeKey = string.Empty;
                string checkCodeHash = string.Empty;

                if (node != null)
                {
                    string imgUrl = HttpUtility.HtmlDecode(node.Attributes["src"].Value);
                    if (imgUrl.IsNullOrEmptyOrWhiteSpace())
                        return "无法获取到验证码，登录失败，请稍后重试。";

                    imgUrl = "https://pt.keepfrds.com/" + imgUrl;
                    OnVerificationCodeEventArgs args = new OnVerificationCodeEventArgs();
                    args.VerificationCodeUrl = imgUrl;
                    args.Site = Site;
                    checkCodeKey = OnVerificationCode(args);
                    checkCodeHash = imgUrl.UrlSearchKey("imagehash");

                    if (checkCodeKey.IsNullOrEmptyOrWhiteSpace() || checkCodeHash.IsNullOrEmptyOrWhiteSpace())
                        return "无法获取到验证码，登录失败，请稍后重试。";

                    string postData = string.Format("username={0}&password={1}&imagestring={2}&imagehash={3}&ssl=yes&trackerssl=yes", User.UserName, User.PassWord, checkCodeKey, checkCodeHash);
                    result = HttpUtils.PostData(Site.LoginUrl, postData, _cookie);
                    htmlResult = result.Item1;
                    if (htmlResult.Contains(User.UserName))
                    {
                        User.Id = GetUserId(htmlResult);
                        _cookie = result.Item2.CookieContainer;
                        SetLocalCookie(_cookie);
                        return "登录成功。";
                    }
                    else
                    {
                        htmlDocument = new HtmlDocument();
                        htmlDocument.LoadHtml(htmlResult);//加载HTML字符串，如果是文件可以用htmlDocument.Load方法加载
                        node = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"nav_block\"]/table/tr/td/table/tr/td");//跟Xpath一样
                        string errMsg = htmlResult;
                        if (node != null)
                            errMsg = node.InnerText;
                        return string.Format("登录失败，失败原因：{0}", errMsg);
                    }
                }
                else
                {
                    return string.Format("登录失败，失败原因：无法获取验证码，请重试。");
                }
            }
        }

        public override string Sign()
        {
            return "我也想签到，可是站点不支持。";
        }
    }
}
