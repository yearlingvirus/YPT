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
        /// 站点配置
        /// </summary>
        public const string RESOURCE_SITES = "Resource/data/PTSite.json";

    }
}
