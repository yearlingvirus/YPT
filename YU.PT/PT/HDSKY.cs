using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using YU.Core;
using YU.Core.DataEntity;
using YU.Core.Event;
using YU.Core.Log;
using YU.Core.Utils;

namespace YU.PT
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

        protected override bool SetTorrentSubTitle(HtmlNode node, PTTorrent torrent)
        {
            bool result = base.SetTorrentSubTitle(node, torrent);
            if (!torrent.Subtitle.IsNullOrEmptyOrWhiteSpace())
                torrent.Subtitle = torrent.Subtitle.Replace("[优惠剩余时间：", "");
            return result;
        }


        protected override Tuple<string, HttpWebRequest, HttpWebResponse> DoLoginPostWithOutCookie(Tuple<string, HttpWebRequest, HttpWebResponse> cookieResult)
        {
            return DoLoginPostWithOutCookie(cookieResult);
        }

        private Tuple<string, HttpWebRequest, HttpWebResponse> DoLoginPostWithOutCookie(Tuple<string, HttpWebRequest, HttpWebResponse> cookieResult, bool isRetry = true)
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
                    checkCodeKey = GetVerificationCode(imgUrl, isRetry);
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

            postData = string.Format(postData, User.UserName, User.PassWord, otpCode, checkCodeKey, checkCodeHash);

            var result = HttpUtils.PostData(Site.LoginUrl, postData, _cookie);
            if (HttpUtils.IsErrorRequest(result.Item1))
                return result;

            //如果登录失败且不是二次尝试的，则重新登录。
            if (!isRetry && !IsLoginSuccess(result.Item3))
            {
                Logger.Info(string.Format("{0} 登录没有成功，识别到的验证码为{1}。", Site.Name, checkCodeKey));
                return DoLoginPostWithOutCookie(cookieResult, false);
            }
            else
                return result;

        }

        private string GetVerificationCode(string imgUrl, bool isAutoOrc = true)
        {
            string checkCodeKey = string.Empty;
            Bitmap bmp = null;
            if (isAutoOrc)
            {
                try
                {
                    bmp = ImageUtils.GetOrcImage((Bitmap)ImageUtils.ImageFromWebTest(imgUrl, _cookie));
                    if (bmp != null)
                    {
                        var orcResults = BaiDuApiUtil.WebImage(bmp);
                        if (orcResults.Any())
                        {
                            checkCodeKey = orcResults.FirstOrDefault();
                            string regEx = @"[^a-z0-9]";
                            checkCodeKey = Regex.Replace(checkCodeKey, regEx, "", RegexOptions.IgnoreCase);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(string.Format("{0} 验证码识别异常。异常原因：{1}", Site.Name, ex.GetInnerExceptionMessage()), ex);
                }
            }
            if (!isAutoOrc || checkCodeKey.Length < 6)
            {
                //目前Frds模板的验证码都是6位
                OnVerificationCodeEventArgs args = new OnVerificationCodeEventArgs();
                args.VerificationCodeUrl = imgUrl;
                args.Site = Site;
                checkCodeKey = OnVerificationCode(args);
            }
            return checkCodeKey;
        }
    }
}
