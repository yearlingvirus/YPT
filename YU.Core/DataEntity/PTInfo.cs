using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YU.Core.Utils;

namespace YU.Core.DataEntity
{
    [Serializable]
    public class PTInfo
    {
        /// <summary>
        /// 站点
        /// </summary>
        public YUEnums.PTEnum SiteId { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [DefaultValue("")]
        public string Name { get; set; }

        /// <summary>
        /// 下载量
        /// </summary>
        [DefaultValue("")]
        public string DownSize { get; set; }

        /// <summary>
        /// 上传量
        /// </summary>
        [DefaultValue("")]
        public string UpSize { get; set; }

        /// <summary>
        /// 分享率
        /// </summary>
        [DefaultValue("")]
        public string ShareRate { get; set; }

        /// <summary>
        /// 保种数量
        /// </summary>
        [DefaultValue("")]
        public string SeedNumber { get; set; }

        /// <summary>
        /// 做种时间
        /// </summary>
        [DefaultValue("")]
        public string SeedTimes { get; set; }

        /// <summary>
        /// 下载时间
        /// </summary>
        [DefaultValue("")]
        public string DownTimes { get; set; }

        /// <summary>
        /// 做种率
        /// </summary>
        [DefaultValue("")]
        public string SeedRate { get; set; }

        /// <summary>
        /// 魔力值
        /// </summary>
        public double Bonus { get; set; }

        /// <summary>
        /// 注册日期
        /// </summary>
        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        [DefaultValue("")]
        public string Rank { get; set; }

        /// <summary>
        /// 上次同步时间
        /// </summary>
        public DateTime LastSyncDate { get; set; }

        public PTInfo()
        {
            foreach (var p in typeof(PTInfo).GetProperties())
            {
                object[] atts = p.GetCustomAttributes(typeof(DefaultValueAttribute), false);
                if (atts != null && atts.Length > 0)
                {
                    var att = atts[0] as DefaultValueAttribute;
                    p.SetValue(this, att.Value, null);
                }
            }
        }

        /// <summary>
        /// ->Grid实体
        /// </summary>
        /// <param name="torrnet"></param>
        /// <returns></returns>
        public PTInfoGridEntity ToGridEntity()
        {
            PTInfoGridEntity entity = new PTInfoGridEntity();
            entity.Id = this.Id;
            entity.Bonus = this.Bonus;
            entity.DownSize = this.DownSize;
            entity.DownTimes = this.DownTimes;
            entity.RealDownSize = YUUtils.ParseB(this.DownSize);
            entity.LastSyncDate = this.LastSyncDate;
            entity.Name = this.Name;

            string regEx = "[^a-zA-Z]";
            entity.Rank = Regex.Replace(this.Rank, regEx, "");
            entity.RealDownTimes = YUUtils.ParseMilliSecond(this.DownTimes);
            entity.RealSeedTimes = YUUtils.ParseMilliSecond(this.SeedTimes);
            entity.RegisterDate = this.RegisterDate;
            entity.SeedNumber = this.SeedNumber.Trim().TryPareValue<int>();
            entity.SeedRate = this.SeedRate.Trim().TryPareValue<double>();
            entity.SeedTimes = this.SeedTimes;
            entity.ShareRate = this.ShareRate.Trim().TryPareValue<double>();
            entity.SiteId = this.SiteId;
            entity.UpSize = this.UpSize;
            entity.RealUpSize = YUUtils.ParseB(this.UpSize);

            return entity;
        }

    }
}
