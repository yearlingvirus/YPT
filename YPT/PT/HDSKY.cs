using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using YU.Core;
using YU.Core.DataEntity;
using YU.Core.Event;
using YU.Core.Utils;

namespace YPT.PT
{
    public class HDSKY : AbstractPT
    {
        public HDSKY(PTUser user) : base(user)
        {
        }

        protected override YUEnums.PTEnum SiteId
        {
            get
            {
                return YUEnums.PTEnum.HDSky;
            }
        }

        protected override Tuple<string, HttpWebRequest, HttpWebResponse> DoLoginPostWithOutCookie(Tuple<string, HttpWebRequest, HttpWebResponse> cookieResult)
        {
            //如果前面Cookie登录没有成功，则下面尝试没有Cookie的情况。
            string postData = "username={0}&password={1}&oneCode={2}&imagestring={3}&imagehash={4}";
            if (new Uri(Site.LoginUrl).Scheme == "https")
                postData += string.Format("&ssl=yes&trackerssl=yes");

            string checkCodeKey = string.Empty;
            string checkCodeHash = string.Empty;
            string otpCode = string.Empty;

            if (Site.IsEnableVerificationCode)
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
                }
                else
                {
                    return new Tuple<string, HttpWebRequest, HttpWebResponse>("无法获取到验证码，登录失败，请稍后重试。", null, null);
                }
            }

            if (Site.isEnableTwo_StepVerification && User.isEnableTwo_StepVerification)
            {
                OnTwoStepVerificationEventArgs e = new OnTwoStepVerificationEventArgs();
                e.Site = Site;
                otpCode = OnTwoStepVerification(e);
            }

            //username=1&password=1&oneCode=1&imagestring=1&imagehash=ef2cc5be40c27a62760d0a3bd1565009
            postData = string.Format(postData, User.UserName, User.PassWord, otpCode, checkCodeKey, checkCodeHash);
            return HttpUtils.PostData(Site.LoginUrl, postData, _cookie);

        }
    }
}
