using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using System.ComponentModel.DataAnnotations;
using YU.Core.Utils;
using System.Drawing;

namespace YU.Core.DataEntity
{
    [Serializable]
    public class PTTorrent
    {
        public PTTorrent()
        {
            DownUrl = string.Empty;
            LinkUrl = string.Empty;
            Title = string.Empty;
            Subtitle = string.Empty;
        }

        /// <summary>
        /// 站点
        /// </summary>
        public YUEnums.PTEnum SiteId { get; set; }

        /// <summary>
        /// 站点名称
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// 版块名称
        /// </summary>
        public string ForumName { get; set; }


        public string Id { get; set; }

        /// <summary>
        /// 种子详情页
        /// </summary>
        public string LinkUrl { get; set; }

        /// <summary>
        /// URL
        /// </summary>
        public string DownUrl { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 免费剩余时间
        /// </summary>
        public string FreeTime { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        public string ResourceType { get; set; }

        /// <summary>
        /// 促销类型
        /// </summary>
        public YUEnums.PromotionType PromotionType { get; set; }

        /// <summary>
        /// 大小
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime UpLoadTime { get; set; }

        /// <summary>
        /// 做种人数
        /// </summary>
        public int SeederNumber { get; set; }

        /// <summary>
        /// 下载人数
        /// </summary>
        public int LeecherNumber { get; set; }

        /// <summary>
        /// 完成人数
        /// </summary>
        public int SnatchedNumber { get; set; }

        /// <summary>
        /// 发布者
        /// </summary>
        public string UpLoader { get; set; }

        /// <summary>
        /// 是否启用H&R
        /// </summary>
        public YUEnums.HRType IsHR { get; set; }


        private static Dictionary<YUEnums.PromotionType, Image> PromotionImages = new Dictionary<YUEnums.PromotionType, Image>();

        private static Dictionary<YUEnums.HRType, Image> HRImages = new Dictionary<YUEnums.HRType, Image>();

        /// <summary>
        /// ->Grid实体
        /// </summary>
        /// <returns></returns>
        public PTTorrentGridEntity ToGridEntity()
        {
            PTTorrentGridEntity entity = new PTTorrentGridEntity();
            entity.Id = this.Id;
            entity.DownUrl = this.DownUrl;
            entity.LinkUrl = this.LinkUrl;
            entity.LeecherNumber = this.LeecherNumber;
            entity.ResourceType = this.ResourceType;
            entity.SeederNumber = this.SeederNumber;
            entity.SiteId = (int)this.SiteId;
            entity.SiteName = this.SiteName;
            entity.ForumName = this.ForumName;
            entity.Size_Display = this.Size;
            entity.Size = YUUtils.ParseB(this.Size);
            entity.SnatchedNumber = this.SnatchedNumber;
            entity.UpLoader = this.UpLoader;
            entity.UpLoadTime = this.UpLoadTime;
            entity.IsHR = this.IsHR;

            if (PTSiteConst.RESOURCE_HRIMG.ContainsKey(entity.IsHR) && !PTSiteConst.RESOURCE_HRIMG[entity.IsHR].IsNullOrEmptyOrWhiteSpace())
            {
                //缓存处理
                if (!HRImages.ContainsKey(entity.IsHR))
                {
                    var image = FormUtils.GetImage(PTSiteConst.RESOURCE_HRIMG[entity.IsHR]);
                    HRImages[entity.IsHR] = image;
                }
                entity.IsHR_Display = HRImages[entity.IsHR];
            }

            entity.PromotionType = this.PromotionType;
            if (PTSiteConst.RESOURCE_PROMOTIONIMG.ContainsKey(entity.PromotionType) && !PTSiteConst.RESOURCE_PROMOTIONIMG[entity.PromotionType].IsNullOrEmptyOrWhiteSpace())
            {
                //缓存处理
                if (!PromotionImages.ContainsKey(entity.PromotionType))
                {
                    var image = FormUtils.GetImage(PTSiteConst.RESOURCE_PROMOTIONIMG[entity.PromotionType]);
                    PromotionImages[entity.PromotionType] = image;
                }
                entity.PromotionType_Display = PromotionImages[entity.PromotionType];
            }


            StringBuilder sb = new StringBuilder();
            if (!this.Title.IsNullOrEmptyOrWhiteSpace())
            {
                if (!this.FreeTime.IsNullOrEmptyOrWhiteSpace())
                    sb.AppendLine(string.Format("{0} [{1}]", this.Title, this.FreeTime));
                else
                    sb.AppendLine(this.Title);
            }

            if (!this.Title.IsNullOrEmptyOrWhiteSpace())
                sb.AppendLine(this.Subtitle);

            string title = sb.ToString();
            int lastIndex = title.LastIndexOf("\r\n");
            if (lastIndex > -1)
                title = title.Substring(0, lastIndex);
            entity.Title = title;
            return entity;
        }
    }
}
