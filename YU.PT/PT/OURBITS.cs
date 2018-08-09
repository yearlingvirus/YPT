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

namespace YU.PT
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
                return YUEnums.PTEnum.OurBits;
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
                string tagTitle = string.Empty;
                var tagNodes = subNode.SelectNodes("./div[contains(concat(' ', normalize-space(@class), ' '), ' tag ')]");
                if (tagNodes != null && tagNodes.Any())
                {
                    foreach (var tagNode in tagNodes)
                    {
                        if (!tagNode.InnerText.IsNullOrEmptyOrWhiteSpace())
                            tagTitle += string.Format("[{0}]", tagNode.InnerText);
                    }
                }

                torrent.Subtitle = string.Format("{0} {1}", tagTitle, HttpUtility.HtmlDecode(subNode.NextSibling.InnerText));
            }
            return !torrent.Subtitle.IsNullOrEmptyOrWhiteSpace();
        }


        public override string Sign(bool isAuto = false)
        {
            string signMsg = string.Empty;
            if (!VerifySign(ref signMsg))
                return signMsg;

            string htmlResult = HttpUtils.GetDataGetHtml(Site.SignUrl, _cookie);
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
    }
}
