using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using YU.Core;
using YU.Core.DataEntity;
using YU.Core.Event;
using YU.Core.Utils;

namespace YU.PT
{
    public class CMCT : AbstractPT
    {
        public CMCT(PTUser user)
            : base(user)
        {

        }

        protected override YUEnums.PTEnum SiteId
        {
            get
            {
                return YUEnums.PTEnum.CMCT;
            }
        }

        protected override void PreSetPersonInfo(HtmlDocument htmlDocument, PTInfo info)
        {
            //魔力值
            var node = htmlDocument.DocumentNode.SelectSingleNode("//a[contains(concat(' ', normalize-space(@href), ' '), ' mybonus.php ')]");
            if (node != null)
            {
                string regEx = @"[^.0-9]";
                string bonusString = Regex.Replace(node.InnerText, regEx, "", RegexOptions.IgnoreCase);
                info.Bonus = bonusString.Trim().TryPareValue<double>();
            }

            //分享率
            node = htmlDocument.DocumentNode.SelectSingleNode("//font[contains(concat(' ', normalize-space(@class), ' '), ' color_uploaded ')]");
            if (node != null && node.PreviousSibling != null)
                info.ShareRate = node.PreviousSibling.InnerText.Trim().TryPareValue<string>();

            //上传量
            node = htmlDocument.DocumentNode.SelectSingleNode("//font[contains(concat(' ', normalize-space(@class), ' '), ' color_downloaded ')]");
            if (node != null && node.PreviousSibling != null)
                info.UpSize = node.PreviousSibling.InnerText.Trim().TryPareValue<string>();

            //下载量
            node = htmlDocument.DocumentNode.SelectSingleNode("//font[contains(concat(' ', normalize-space(@class), ' '), ' color_active ')]");
            if (node != null && node.PreviousSibling != null)
                info.DownSize = node.PreviousSibling.InnerText.Trim().TryPareValue<string>();

            //做种数
            node = htmlDocument.DocumentNode.SelectSingleNode("//img[contains(concat(' ', normalize-space(@alt), ' '), ' Torrents leeching ')]");
            if (node != null && node.PreviousSibling != null)
                info.SeedNumber = node.PreviousSibling.InnerText.Trim().TryPareValue<string>();
        }

        /// <summary>
        /// 设置PersonInfo
        /// </summary>
        /// <param name="infoMaps"></param>
        /// <param name="nodes"></param>
        /// <param name="info"></param>
        protected override void SetPersonInfo(Dictionary<YUEnums.PersonInfoMap, int> infoMaps, HtmlNodeCollection nodes, PTInfo info)
        {
            #region Convert
            //注册日期
            var node = nodes[infoMaps[YUEnums.PersonInfoMap.RegisterDate]];
            if (node != null)
            {
                node = node.SelectSingleNode(".//span");
                if (node != null && node.Attributes.Contains("title"))
                {
                    info.RegisterDate = node.Attributes["title"].Value.TryPareValue<DateTime>();
                }
            }


            //做种率
            node = nodes[infoMaps[YUEnums.PersonInfoMap.SeedRate]];
            if (node != null)
            {
                var childNode = node.SelectSingleNode(".//td/font/text()");
                if (childNode != null)
                    info.SeedRate = childNode.InnerText;
            }

            //做种时间
            node = nodes[infoMaps[YUEnums.PersonInfoMap.SeedTimes]];
            if (node != null)
            {
                var childNode = node.SelectSingleNode(".//tr[2]/td/text()[last()]");
                if (childNode != null)
                {
                    var index = childNode.InnerText.IndexOf(":");
                    if (index > -1)
                        info.SeedTimes = childNode.InnerText.Substring(index + 1).Trim();
                }
            }

            //下载时间
            node = nodes[infoMaps[YUEnums.PersonInfoMap.DownTimes]];
            if (node != null)
            {
                var childNode = node.SelectSingleNode(".//tr[2]/td[2]/text()[last()]");
                if (childNode != null)
                {
                    var index = childNode.InnerText.IndexOf(":");
                    if (index > -1)
                        info.DownTimes = childNode.InnerText.Substring(index + 1).Trim();
                }
            }

            //等级
            node = nodes[infoMaps[YUEnums.PersonInfoMap.Rank]];
            if (node != null)
            {
                var childNode = node.SelectSingleNode("./img");
                if (childNode != null && childNode.Attributes.Contains("src"))
                {
                    string srcImg = childNode.Attributes["src"].Value;
                    foreach (var item in PTSiteConst.CLASSIMGS)
                    {
                        if (srcImg.IndexOf(item.Key, StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            info.Rank = item.Value;
                            break;
                        }
                    }
                    if (info.Rank.IsNullOrEmptyOrWhiteSpace() && childNode.Attributes.Contains("alt"))
                    {
                        info.Rank = childNode.Attributes["alt"].Value;
                    }
                }
            }


            info.LastSyncDate = DateTime.Now;
            info.UserId = User.UserId;
            info.SiteId = SiteId;
            info.SiteName = Site.Name;
            info.Name = User.UserName;

            #endregion
        }
    }
}
