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
        public int Id { get; set; }

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
        [GridView("上传量", true, 100)]
        public string UpSize { get; set; }

        /// <summary>
        /// 上传量
        /// </summary>
        [GridView("上传量", false, 100)]
        public double RealUpSize { get; set; }

        /// <summary>
        /// 下载量
        /// </summary>
        [GridView("下载量", true, 100)]
        public string DownSize { get; set; }

        /// <summary>
        /// 下载量
        /// </summary>
        [GridView("下载量", false, 100)]
        public double RealDownSize { get; set; }


        /// <summary>
        /// 分享率
        /// </summary>
        [GridView("分享率", true, 100)]
        public double ShareRate { get; set; }

        /// <summary>
        /// 保种数量
        /// </summary>
        [GridView("保种数量", false, 100)]
        public int SeedNumber { get; set; }


        /// <summary>
        /// 做种时间
        /// </summary>
        [GridView("做种时间", true, 100)]
        public string SeedTimes { get; set; }

        /// <summary>
        /// 做种时间
        /// </summary>
        [GridView("做种时间", false, 100)]
        public double RealSeedTimes { get; set; }

        /// <summary>
        /// 下载时间
        /// </summary>
        [GridView("下载时间", true, 100)]
        public string DownTimes { get; set; }

        /// <summary>
        /// 下载时间
        /// </summary>
        [GridView("下载时间", false, 100)]
        public double RealDownTimes { get; set; }

        /// <summary>
        /// 做种率
        /// </summary>
        [GridView("做种率", true, 100)]
        public double SeedRate { get; set; }

        /// <summary>
        /// 魔力值
        /// </summary>
        [GridView("魔力值", true, 100)]
        public double Bonus { get; set; }

        /// <summary>
        /// 注册日期
        /// </summary>
        [GridView("注册日期", true, 100)]
        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// 注册日期
        /// </summary>
        [GridView("注册日期(周)", true, 100)]
        public string RegisterWeek { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        [GridView("等级", true, 100)]
        public string Rank { get; set; }

        /// <summary>
        /// 上次同步时间
        /// </summary>
        [GridView("同步时间", true, 100)]
        public DateTime LastSyncDate { get; set; }
    }
}
