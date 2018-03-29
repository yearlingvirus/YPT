using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
    public class OPENCD : AbstractPT
    {
        public OPENCD(PTUser user) : base(user)
        {
        }

        protected override YUEnums.PTEnum SiteId
        {
            get
            {
                return YUEnums.PTEnum.OpenCD;
            }
        }

        public override string Sign(bool isAuto = false)
        {
            if (_cookie != null && _cookie.Count > 0)
                return Sign(isAuto, true, 1);
            else
                return "无法获取Cookie信息，签到失败，请重新登录系统。";
        }

        private string Sign(bool isAuto, bool isAutoOrc, int count)
        {
            var result = HttpUtils.GetData(Site.SignUrl, _cookie);
            if (HttpUtils.IsErrorRequest(result.Item1))
                return result.Item1;

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(result.Item1);

            var imgNode = htmlDocument.DocumentNode.SelectSingleNode("//img");
            if (imgNode != null && imgNode.Attributes.Contains("src"))
            {
                string imgUrl = HttpUtility.HtmlDecode(imgNode.Attributes["src"].Value);
                if (imgUrl.IsNullOrEmptyOrWhiteSpace())
                    return "无法获取到签到验证码。";

                imgUrl = UrlUtils.CombileUrl(Site.Url, imgUrl);

                string checkCodeHash = imgUrl.UrlSearchKey("imagehash");
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
                if (!isAutoOrc)
                {
                    OnVerificationCodeEventArgs args = new OnVerificationCodeEventArgs();
                    args.VerificationCodeUrl = imgUrl;
                    args.Site = Site;
                    checkCodeKey = OnVerificationCode(args);
                }

                string postData = string.Format("imagehash={0}&imagestring={1}", checkCodeHash, checkCodeKey);
                result = HttpUtils.PostData(Site.SignUrl + "?cmd=signin", postData, _cookie);
                if (HttpUtils.IsErrorRequest(result.Item1))
                    return result.Item1;

                var o = JsonConvert.DeserializeObject(result.Item1) as JObject;
                string message = o["msg"].TryPareValue<string>();
                if (o["state"].TryPareValue<bool>())
                {
                    string signDay = o["signindays"].TryPareValue<string>();
                    string bonus = o["integral"].TryPareValue<string>();
                    return string.Format("签到成功，连续天数：{0}，积分：{1}", signDay, bonus);
                }
                else
                {
                    if (message.IsNullOrEmptyOrWhiteSpace())
                        return string.Format("签到失败，失败原因：{0}", "你可能已经签过到了。");
                }

                if (!isAuto && count <= 1)
                {
                    //如果签到失败且之前是自动签到
                    Logger.Info(string.Format("{0} 签到失败，识别到的验证码为{1}。", Site.Name, checkCodeKey));
                    return Sign(isAuto, false, ++count);
                }
                else if (isAuto && count <= 2)
                    return Sign(isAuto, true, ++count);
                else
                    return string.Format("签到失败，失败原因：{0}", message);


            }
            else
            {
                return "签到失败，获取签到验证码失败。";
            }
        }

        protected override void SetTorrentHR(HtmlNode node, PTTorrent torrent)
        {
            torrent.IsHR = YUEnums.HRType.HR;
        }


        protected override void PreSetPersonInfo(HtmlDocument htmlDocument, PTInfo info)
        {
            //做种数
            string regex = "[^.0-9]";
            var node = htmlDocument.DocumentNode.SelectSingleNode("//img[contains(concat(' ', normalize-space(@alt), ' '), ' Torrents seeding ')]");
            if (node != null && node.NextSibling != null)
                info.SeedNumber = Regex.Replace(node.NextSibling.InnerText.Trim(), regex, "", RegexOptions.IgnoreCase);
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

            //分享率
            node = nodes[infoMaps[YUEnums.PersonInfoMap.ShareRate]];
            if (node != null)
            {
                var childNode = node.SelectSingleNode(".//td/font/text()");
                if (childNode != null)
                    info.ShareRate = childNode.InnerText;
            }

            //上传量
            node = nodes[infoMaps[YUEnums.PersonInfoMap.UpSize]];
            if (node != null)
            {
                var childNode = node.SelectSingleNode(".//tr[2]/td/text()[last()]");
                if (childNode != null)
                {
                    var index = childNode.InnerText.IndexOf(":");
                    if (index > -1)
                    {
                        string sizeString = childNode.InnerText.Substring(index + 1).Trim();
                        index = sizeString.IndexOf("(");
                        if (index > -1)
                            info.UpSize = sizeString.Substring(0, index).Trim();
                    }

                }
            }

            //下载量
            node = nodes[infoMaps[YUEnums.PersonInfoMap.DownSize]];
            if (node != null)
            {
                var childNode = node.SelectSingleNode(".//tr[2]/td[2]/text()[2]");
                if (childNode != null)
                {
                    var index = childNode.InnerText.IndexOf(":");
                    if (index > -1)
                    {
                        string sizeString = childNode.InnerText.Substring(index + 1).Trim();
                        index = sizeString.IndexOf("(");
                        if (index > -1)
                            info.DownSize = sizeString.Substring(0, index).Trim();
                    }
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

            //积分
            node = nodes[infoMaps[YUEnums.PersonInfoMap.Bonus]];
            if (node != null)
            {
                info.Bonus = node.InnerText.TryPareValue<double>();
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
