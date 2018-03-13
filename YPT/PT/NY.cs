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

        protected override HtmlNode GetUserNode(string htmlResult)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.OptionOutputAsXml = false;
            htmlDocument.LoadHtml(htmlResult);//加载HTML字符串，如果是文件可以用htmlDocument.Load方法加载
            HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"userlink\"]//a");//跟Xpath一样\
            return node;
        }


    }
}
