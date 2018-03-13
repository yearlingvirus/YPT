using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using YU.Core;
using YU.Core.DataEntity;
using System.Web;
using YU.Core.Utils;
using System.IO;

namespace YPT.PT
{
    public class NY : AbstractPT
    {
        public NY(PTUser user) : base(user)
        {
        }

        protected override YUEnums.PTEnum SiteId
        {
            get
            {
                return YUEnums.PTEnum.NYPT;
            }
        }

        protected override HtmlNodeCollection GetTorrentNodes(HtmlDocument htmlDocument)
        {
            return htmlDocument.DocumentNode.SelectNodes("//*[@id=\"form_torrent\"]//tr");
        }


        protected override PTUser UpdateUserWhileChange(string htmlResult, PTUser user)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.OptionOutputAsXml = false;
            htmlDocument.LoadHtml(htmlResult);//加载HTML字符串，如果是文件可以用htmlDocument.Load方法加载
            HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"userlink\"]//a");//跟Xpath一样
            if (node != null)
            {
                var url = HttpUtility.HtmlDecode(node.Attributes["href"].Value);
                url = string.Join("/", Site.Url, url);
                user.UserId = url.UrlSearchKey("id").TryPareValue<int>();
                if (user.UserName != node.InnerText)
                {
                    //当用户输入的用户名与获取到的用户名不一致时，取获取的用户名为准。
                    //修改用户名时，同时还要处理本地的Cookie文件
                    string cookiePath = GetCookieFilePath();
                    if (File.Exists(cookiePath))
                    {
                        user.UserName = node.InnerText;
                        string newCookiePath = GetCookieFilePath();
                        File.Move(cookiePath, newCookiePath);
                    }
                }
            }
            return user;
        }
    }
}
