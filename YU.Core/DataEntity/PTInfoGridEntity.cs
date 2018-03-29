using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YU.Core.Attributes;

namespace YU.Core.DataEntity
{
    [Serializable]
    public class PTInfoGridEntity
    {
        /// <summary>
        /// 站点
        /// </summary>
        [GridView("站点", false, 100)]
        public int SiteId { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        [GridView("站点", true, 100)]
        public string SiteName { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        [GridView("ID", true, 100)]
        public string Id { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        [GridView("Url", false, 100)]
        public string Url { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [GridView("用户名", true, 100)]
        public string Name { get; set; }

        /// <summary>
        /// 上传量
        /// </summary>
        [GridView("上传量", true, 120)]
        public string UpSize_Display { get; set; }

        /// <summary>
        /// 上传量
        /// </summary>
        [GridView("上传量", false, 120)]
        public double UpSize { get; set; }

        /// <summary>
        /// 下载量
        /// </summary>
        [GridView("下载量", true, 120)]
        public string DownSize_Display { get; set; }

        /// <summary>
        /// 下载量
        /// </summary>
        [GridView("下载量", false, 120)]
        public double DownSize { get; set; }


        /// <summary>
        /// 分享率
        /// </summary>
        [GridView("分享率", false, 100)]
        public double ShareRate { get; set; }

        /// <summary>
        /// 分享率
        /// </summary>
        [GridView("分享率", true, 100)]
        public string ShareRate_Display { get; set; }

        /// <summary>
        /// 保种数量
        /// </summary>
        [GridView("保种", false, 80)]
        public int SeedNumber { get; set; }

        /// <summary>
        /// 保种数量
        /// </summary>
        [GridView("保种", true, 80)]
        public string SeedNumber_Display { get; set; }

        /// <summary>
        /// 做种时间
        /// </summary>
        [GridView("做种(天)", true , 100)]
        public string SeedTimes_Display { get; set; }

        /// <summary>
        /// 做种时间
        /// </summary>
        [GridView("做种时间", false, 100)]
        public double SeedTimes { get; set; }

        /// <summary>
        /// 下载时间
        /// </summary>
        [GridView("下载(天)", true, 100)]
        public string DownTimes_Display { get; set; }

        /// <summary>
        /// 下载时间
        /// </summary>
        [GridView("下载时间", false, 100)]
        public double DownTimes { get; set; }

        /// <summary>
        /// 做种率
        /// </summary>
        [GridView("做种率", false, 100)]
        public double SeedRate { get; set; }

        /// <summary>
        /// 做种率
        /// </summary>
        [GridView("做种率", true, 100)]
        public string SeedRate_Display { get; set; }

        /// <summary>
        /// 魔力值
        /// </summary>
        [GridView("魔力值", false, 100)]
        public double Bonus { get; set; }

        /// <summary>
        /// 魔力值
        /// </summary>
        [GridView("魔力值", true, 100)]
        public string Bonus_Display { get; set; }

        /// <summary>
        /// 注册日期
        /// </summary>
        [GridView("注册日期", false, 120)]
        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// 注册日期
        /// </summary>
        [GridView("注册日期", true, 120)]
        public string RegisterDate_Display { get; set; }

        /// <summary>
        /// 入站至今
        /// </summary>
        [GridView("入站至今", true, 100)]
        public string RegisterWeek_Display { get; set; }

        /// <summary>
        /// 入站至今
        /// </summary>
        [GridView("入站至今", false, 100)]
        public DateTime RegisterWeek { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        [GridView("等级", true, 100)]
        public string Rank { get; set; }

        /// <summary>
        /// 上次同步时间
        /// </summary>
        [GridView("同步时间", false, 120)]
        public DateTime LastSyncDate { get; set; }

        /// <summary>
        /// 上次同步时间
        /// </summary>
        [GridView("同步时间", true, 120)]
        public string LastSyncDate_Display { get; set; }
    }
}
