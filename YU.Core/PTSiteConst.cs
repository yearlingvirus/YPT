using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YU.Core.DataEntity;

namespace YU.Core
{
    public class PTSiteConst
    {

        /// <summary>
        /// 促销字典
        /// </summary>
        public readonly static Dictionary<YUEnums.PromotionType, string[]> PromoptionDict = new Dictionary<YUEnums.PromotionType, string[]>()
        {
            {YUEnums.PromotionType.HALF2X, new string[] { "pro_50pctdown2up", "twouphalfdown", "twouphalfdown_bg" } },
            {YUEnums.PromotionType.HALFDOWN, new string[] { "pro_50pctdown", "halfdown", "ico_half" } },
            {YUEnums.PromotionType.FREE2UP, new string[] { "pro_free2up", "twoupfree", "twoupfree_bg" } },
            {YUEnums.PromotionType.FREE, new string[] {"pro_free", "free", "ico_free" } },
            {YUEnums.PromotionType.TWOUP, new string[] { "pro_2up", "twoup", "twoup_bg" } },
            {YUEnums.PromotionType.THIRTYPERDOWN, new string[] { "pro_30pctdown", "thirtypercent", "ico_30" } },
        };

        /// <summary>
        /// 促销图片资源
        /// </summary>
        public readonly static Dictionary<YUEnums.PromotionType, string> RESOURCE_PROMOTIONIMG = new Dictionary<YUEnums.PromotionType, string>()
        {
            { YUEnums.PromotionType.FREE, "Resource/icon/freeicon.gif"},
            { YUEnums.PromotionType.FREE2UP, "Resource/icon/2upfree.gif"},
            { YUEnums.PromotionType.HALF2X, "Resource/icon/2up50down.png"},
            { YUEnums.PromotionType.HALFDOWN, "Resource/icon/halfdown.gif"},
            //{ YUEnums.PromotionType.NORMAL, "Resource/icon/normal.png"},
            { YUEnums.PromotionType.THIRTYPERDOWN, "Resource/icon/ico_30.gif"},
            { YUEnums.PromotionType.TWOUP, "Resource/icon/2up.gif"},
        };

        /// <summary>
        /// 等级图片字典
        /// </summary>
        public readonly static Dictionary<string, string> CLASSIMGS = new Dictionary<string, string>()
        {
            { "Peasant", "Peasant"},
            { "User", "User"},
            { "Power", "Power User"},
            { "Elite", "Elite User"},
            { "Crazy", "Crazy User"},
            { "Insane", "Insane User"},
            { "Veteran", "Veteran User"},
            { "Extreme", "Extreme User"},
            { "Ultimate", "Ultimate User"},
            { "Nexus", "Nexus User"},
            { "VIP", "VIP"},
            { "Retiree", "Retiree"},
            { "Uploader", "Uploader"},
            { "Moderator", "Moderator"},
            { "Administrator", "Administrator"},
        };

        /// <summary>
        /// 扩展站点
        /// </summary>
        public const string EXTEND_SITES = "Extend/extendSite.json";

        /// <summary>
        /// Demo
        /// </summary>
        public const string EXTEND_SITES_SAMPLE = "Extend/sampleSite.json";

        /// <summary>
        /// 扩展站点注释
        /// </summary>
        public const string EXTEND_SITES_COMMENT = @"//此处可以配置扩展站点或者覆盖预置站点的信息。
//注意：扩展添加的站点Id请大于10000，以免与预置站点冲突
//添加为JSON格式，具体可以参考sampleSite.json
//如果种子和用户信息如果没有正常显示，请在PersonInfoMaps和TorrentMaps添加对应的行头或者列头名称。
//如果在请求站点排序中异常，请检查SearchColUrlMaps，查看站点对应列的Url参数，并回填到SearchColUrlMaps中。
//如果需要添加站点其他版块，请在Forums中添加，并将站点对应Url回填。
//如果正常显示，则不需要添加，这样程序升级后可以兼容更多的站点";
    }
}
