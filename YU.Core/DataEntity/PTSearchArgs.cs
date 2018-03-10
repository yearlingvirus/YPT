using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YU.Core.DataEntity
{
    /// <summary>
    /// 搜索参数
    /// </summary>
    public class PTSearchArgs
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string SearchKey { get; set; }

        /// <summary>
        /// 促销方式
        /// </summary>
        public YUEnums.PromotionType PromotionType { get; set; }

        /// <summary>
        /// 存活类型
        /// </summary>
        public YUEnums.AliveType AliveType { get; set; }

        /// <summary>
        /// 收藏类型
        /// </summary>
        public YUEnums.FavType FavType { get; set; }

        /// <summary>
        /// 忽略置顶
        /// </summary>
        public bool IsIngoreTop { get; set; }

        /// <summary>
        /// 是否请求站点排序
        /// </summary>
        public bool IsPostSiteOrder { get; set; }

        /// <summary>
        /// 排序列
        /// </summary>
        public KeyValuePair<string, YUEnums.SortOrderType> SortKvr { get; set; }
    }
}
