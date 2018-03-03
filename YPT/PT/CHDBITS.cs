using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YU.Core;
using YU.Core.DataEntity;
using YU.Core.Utils;

namespace YPT.PT
{
    public class CHDBITS : AbstractPT
    {
        public CHDBITS(PTUser user)
            : base(user)
        {

        }

        protected override YUEnums.PTEnum SiteId
        {
            get
            {
                return YUEnums.PTEnum.CHDBITS;
            }
        }

        public override string Sign()
        {
            return "暂未支持该站点，请登录网页端签到。";
        }

        protected override void SetTorrentPromotionType(HtmlNode node, PTTorrent torrent)
        {
            string html = string.Empty;
            //由于ChdBits tr行的Class bg是错误的，这里当做不可信处理，移除tr的html.
            node = node.SelectSingleNode("./table/tr");
            if (!torrent.Title.IsNullOrEmptyOrWhiteSpace())
                html = node.InnerHtml.Replace(torrent.Title, "");
            if (!torrent.Subtitle.IsNullOrEmptyOrWhiteSpace())
                html = node.InnerHtml.Replace(torrent.Subtitle, "");
            torrent.PromotionType = YUEnums.PromotionType.NORMAL;

            foreach (var item in PTSiteConst.PromoptionDict)
            {
                foreach (var value in item.Value)
                {
                    //ChdBits所有bg Class都不可信。
                    if (html.Contains(value) && !value.Contains("bg"))
                    {
                        torrent.PromotionType = item.Key;
                        return;
                    }
                }
            }
        }

    }
}
