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
        public string Fid { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        public YUEnums.PTEnum SiteId { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        [DefaultValue("")]
        public string SiteName { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// URL
        /// </summary>
        public string Url { get; set; }

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
                    p.SetValue(this, att.Value);
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
            string defaultValue = "--";

            PTInfoGridEntity entity = new PTInfoGridEntity();
            entity.Id = this.UserId == 0 ? defaultValue : this.UserId.TryPareValue<string>();
            entity.Url = this.Url;
            entity.Bonus = this.Bonus;
            entity.Bonus_Display = GetPropertyDisplay(this.Bonus);

            entity.DownSize_Display = GetPropertyDisplay(this.DownSize);
            entity.DownSize = YUUtils.ParseB(this.DownSize);
            entity.UpSize_Display = GetPropertyDisplay(this.UpSize);
            entity.UpSize = YUUtils.ParseB(this.UpSize);

            entity.DownTimes = YUUtils.ParseMilliSecond(this.DownTimes);
            entity.DownTimes_Display = entity.DownTimes <= 0 ? defaultValue : YUUtils.RetainNDecimal(TimeSpan.FromMilliseconds(entity.DownTimes).TotalDays);

            entity.SeedTimes = YUUtils.ParseMilliSecond(this.SeedTimes);
            entity.SeedTimes_Display = entity.SeedTimes <= 0 ? defaultValue : YUUtils.RetainNDecimal(TimeSpan.FromMilliseconds(entity.SeedTimes).TotalDays);

            entity.LastSyncDate = this.LastSyncDate;
            entity.LastSyncDate_Display = GetPropertyDisplay(this.LastSyncDate);

            entity.RegisterDate = this.RegisterDate;
            entity.RegisterDate_Display = GetPropertyDisplay(this.RegisterDate);

            int registerDays = DateTime.Now.Subtract(this.RegisterDate).Duration().Days;
            entity.RegisterWeek = this.RegisterDate;
            if (this.RegisterDate == DateTime.MinValue)
                entity.RegisterWeek_Display = defaultValue;
            else
                entity.RegisterWeek_Display = registerDays == 0 ? "0天" : string.Format("{0}周", registerDays / 7) + ((registerDays % 7) > 0 ? string.Format("{0}天", registerDays % 7) : string.Empty);

            string regEx = "/^[A-Za-z\\s]+$/";
            entity.Rank = Regex.Replace(this.Rank, regEx, "");
            //如果过滤之后为空字符串，则直接用站点的。
            if (entity.Rank.IsNullOrEmptyOrWhiteSpace())
                entity.Rank = this.Rank.IsNullOrEmptyOrWhiteSpace() ? defaultValue : this.Rank;

            entity.SeedNumber = GetPropertyValue<int>(this.SeedNumber);
            entity.SeedNumber_Display = GetPropertyDisplay(this.SeedNumber);

            entity.SeedRate = GetPropertyValue<double>(this.SeedRate);
            entity.SeedRate_Display = GetPropertyDisplay(this.SeedRate);

            if (entity.DownSize == 0 && entity.UpSize == 0)
            {
                entity.ShareRate = 0D;
                entity.ShareRate_Display = defaultValue;
            }
            else if (entity.UpSize > 0 && entity.DownSize == 0)
            {
                entity.ShareRate = double.MaxValue;
                entity.ShareRate_Display = "inf";
            }
            else
            {
                entity.ShareRate = GetPropertyValue<double>(this.ShareRate);
                entity.ShareRate_Display = GetPropertyDisplay(this.ShareRate);
            }

            entity.Name = GetPropertyDisplay(this.Name);
            entity.SiteId = (int)this.SiteId;
            entity.SiteName = GetPropertyDisplay(this.SiteName);

            return entity;
        }

        public string GetPropertyDisplay<T>(T propertyValue, string defaultValue = "--")
        {
            Type t = typeof(T);
            if (t == typeof(int))
                return propertyValue.TryPareValue<int>() == 0 ? defaultValue : propertyValue.TryPareValue<string>();
            else if (t == typeof(float))
                return propertyValue.TryPareValue<float>() == 0F ? defaultValue : propertyValue.TryPareValue<string>();
            else if (t == typeof(double))
                return propertyValue.TryPareValue<double>() == 0D ? defaultValue : propertyValue.TryPareValue<string>();
            else if (t == typeof(long))
                return propertyValue.TryPareValue<long>() == 0L ? defaultValue : propertyValue.TryPareValue<string>();
            else if (t == typeof(decimal))
                return propertyValue.TryPareValue<decimal>() == 0M ? defaultValue : propertyValue.TryPareValue<string>();
            else if (t == typeof(string))
                return propertyValue.TryPareValue<string>().IsNullOrEmptyOrWhiteSpace() ? defaultValue : propertyValue.TryPareValue<string>().Trim();
            else if (t == typeof(DateTime))
                return propertyValue.TryPareValue<DateTime>() == DateTime.MinValue ? defaultValue : propertyValue.TryPareValue<string>();
            return defaultValue;
        }

        public T GetPropertyValue<T>(string propertyValue)
        {
            T defaultValue = default(T);
            if (propertyValue.IsNullOrEmptyOrWhiteSpace())
                return defaultValue;
            else
                return propertyValue.Trim().TryPareValue<T>();
        }

    }
}
