using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using System.ComponentModel.DataAnnotations;
using YU.Core.Utils;

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
        public bool IsHR { get; set; }

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
            entity.PromotionType = this.PromotionType;
            entity.ResourceType = this.ResourceType;
            entity.SeederNumber = this.SeederNumber;
            entity.SiteId = this.SiteId;
            entity.Size = this.Size;
            entity.RealSize = YUUtils.ParseB(this.Size);
            entity.SnatchedNumber = this.SnatchedNumber;
            entity.UpLoader = this.UpLoader;
            entity.UpLoadTime = this.UpLoadTime;
            entity.IsHR = this.IsHR;
            if (YUConst.PromotionImgDict.ContainsKey(entity.PromotionType) && !YUConst.PromotionImgDict[entity.PromotionType].IsNullOrEmptyOrWhiteSpace())
                entity.image = FormUtils.GetImage(YUConst.PromotionImgDict[entity.PromotionType]);

            StringBuilder sb = new StringBuilder();
            if (!this.Title.IsNullOrEmptyOrWhiteSpace())
                sb.AppendLine(this.Title);
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
