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

        protected override bool IsUseIEDownload()
        {
            return false;
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
