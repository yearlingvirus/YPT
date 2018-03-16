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
    public class BTSCHOOL : AbstractPT
    {
        public BTSCHOOL(PTUser user)
            : base(user)
        {

        }

        protected override YUEnums.PTEnum SiteId
        {
            get
            {
                return YUEnums.PTEnum.BTSchool;
            }
        }


        protected override void SetTorrentHR(HtmlNode node, PTTorrent torrent)
        {
            //BTSCHOOL 全站HR
            torrent.IsHR = YUEnums.HRType.HR;
        }

        public override string Sign()
        {
            if (_cookie != null && _cookie.Count > 0)
            {
                string htmlResult = HttpUtils.GetDataGetHtml(Site.SignUrl, _cookie);

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlResult);//加载HTML字符串，如果是文件可以用htmlDocument.Load方法加载

                //这个是每次第一次签到的
                HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"outer\"]//tr//a");
                if (node != null)
                    return node.InnerText.Contains("魔力值") ? node.InnerText : "无法获取签到信息，可能已经签到成功。";
                else
                    return "签到失败，请登录网站签到。";
            }
            else
            {
                return "无法获取Cookie信息，签到失败，请重新登录系统。";
            }
        }

    }
}

