using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using YU.Core;
using YU.Core.DataEntity;
using YU.Core.Event;
using YU.Core.Utils;

namespace YPT.PT
{
    public class MTEAM : AbstractPT
    {

        public MTEAM(PTUser user)
            : base(user)
        {

        }

        protected override YUEnums.PTEnum SiteId
        {
            get
            {
                return YUEnums.PTEnum.MTEAM;
            }
        }

        public override Tuple<string, HttpWebRequest, HttpWebResponse> DoLoginPostWithOutCookie(Tuple<string, HttpWebRequest, HttpWebResponse> cookieResult, string otpCode)
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



        protected override bool SetTorrentSubTitle(HtmlNode node, PTTorrent torrent)
        {
            var subNode = node.SelectSingleNode(".//td[2]/br");
            if (subNode != null && subNode.NextSibling != null)
                torrent.Subtitle = HttpUtility.HtmlDecode(subNode.NextSibling.InnerText);
            return false;
        }
    }
}
