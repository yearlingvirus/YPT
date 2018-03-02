using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YU.Core;
using YU.Core.DataEntity;
using YU.Core.Utils;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using HtmlAgilityPack;
using System.Web;

namespace YPT.PT
{
    public class OURBITS : AbstractPT
    {
        public OURBITS(PTUser user)
            : base(user)
        {

        }

        protected override YUEnums.PTEnum SiteId
        {
            get
            {
                return YUEnums.PTEnum.OURBITS;
            }
        }


        protected override bool SetTorrentSubTitle(HtmlNode node, PTTorrent torrent)
        {
            //判断有没有官方的注解如[国语][中字]等
            var subNode = node.SelectSingleNode(".//td[1]/div[not(contains(concat(' ', normalize-space(@class), ' '), ' progressBar '))]");
            if (subNode == null)
            {
                subNode = node.SelectSingleNode(".//td[1]/br");
                if (subNode != null && subNode.NextSibling != null)
                    torrent.Subtitle = HttpUtility.HtmlDecode(subNode.NextSibling.InnerText);
            }
            else if (subNode.NextSibling != null)
            {
                torrent.Subtitle = string.Format("{0} {1}", subNode.InnerText, HttpUtility.HtmlDecode(subNode.NextSibling.InnerText));
                return true;
            }
            return false;
        }


        public override string Sign()
        {
            if (_cookie != null)
            {
                string htmlResult = HttpUtils.PostDataGetHtml(Site.SignUrl, "", _cookie);

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlResult);//加载HTML字符串，如果是文件可以用htmlDocument.Load方法加载

                //这个是每次第一次签到的
                HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"outer\"]/table/tr/td/table/tr/td/p");//跟Xpath一样，轻松的定位到相应节点下
                if (node == null)
                    //这个是已经签到过的
                    node = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"outer\"]/table/tr/td/table/tr/td");
                if (node != null)
                    return node.InnerText;
                else
                    return htmlResult;
            }
            else
            {
                return "无法获取Cookie信息，签到失败，请重新登录系统。";
            }
        }
    }
}
