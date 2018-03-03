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

        public override Tuple<string, HttpWebRequest, HttpWebResponse> DoLoginPostWithOutCookie(Tuple<string, HttpWebRequest, HttpWebResponse> cookieResult, string otpCode)
        {
            string htmlResult = string.Empty;
            //这里先看有没有前面是不是有过请求了，如果有的话，那么直接在这里获取验证码，如果没有，则自己获取。
            if (cookieResult != null && !cookieResult.Item1.IsNullOrEmptyOrWhiteSpace())
                htmlResult = cookieResult.Item1;
            else
                htmlResult = HttpUtils.GetData(Site.Url, _cookie).Item1;

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlResult);
            HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"nav_block\"]/form[2]/table/tr[3]/td[2]/img");
            string checkCodeKey = string.Empty;
            string checkCodeHash = string.Empty;
            if (node != null)
            {
                string imgUrl = HttpUtility.HtmlDecode(node.Attributes["src"].Value);
                if (imgUrl.IsNullOrEmptyOrWhiteSpace())
                    return new Tuple<string, HttpWebRequest, HttpWebResponse>("无法获取到验证码，登录失败，请稍后重试。", null, null);

                imgUrl = "https://pt.keepfrds.com/" + imgUrl;
                OnVerificationCodeEventArgs args = new OnVerificationCodeEventArgs();
                args.VerificationCodeUrl = imgUrl;
                args.Site = Site;
                checkCodeKey = OnVerificationCode(args);
                checkCodeHash = imgUrl.UrlSearchKey("imagehash");
                if (checkCodeKey.IsNullOrEmptyOrWhiteSpace() || checkCodeHash.IsNullOrEmptyOrWhiteSpace())
                    return new Tuple<string, HttpWebRequest, HttpWebResponse>("无法获取到验证码，登录失败，请稍后重试。", null, null);

                string postData = string.Format("username={0}&password={1}&imagestring={2}&imagehash={3}&ssl=yes&trackerssl=yes", User.UserName, User.PassWord, checkCodeKey, checkCodeHash);
                return HttpUtils.PostData(Site.LoginUrl, postData, _cookie);
            }
            else
            {
                return new Tuple<string, HttpWebRequest, HttpWebResponse>("无法获取到验证码，登录失败，请稍后重试。", null, null);
            }
        }

    }
}
