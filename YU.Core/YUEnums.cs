using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace YU.Core
{
    public class YUEnums
    {
        /// <summary>
        /// PT
        /// </summary>
        public enum PTEnum
        {
            TTG = 1,
            MTeam = 2,
            HDSky = 3,
            HDChina = 4,
            CHDBits = 5,
            CMCT = 6,
            OurBits = 7,
            KeepFrds = 8,
            U2 = 9,
            HDHome = 10,
            OpenCD = 11,
            GZTown = 12,
            BTSchool = 13,
            HDU = 14,
            NYPT = 15,
            HDStreet = 16,
            HDCity = 17,
            JoyHD = 18,
            HDRoute = 19,
            IPT = 20,
            HDT = 21,
            CCFBits = 22,
        }

        public enum FormatterType
        {
            BinaryFormatter = 0,
            NetDataContract = 1
        }


        /// <summary>
        /// 种子映射
        /// </summary>
        public enum TorrentMap
        {

            /// <summary>
            /// 详情
            /// </summary>
            Detail,

            /// <summary>
            /// 资源类型
            /// </summary>
            ResourceType,

            /// <summary>
            /// 促销类型
            /// </summary>
            PromotionType,

            /// <summary>
            /// 大小
            /// </summary>
            Size,

            /// <summary>
            /// 存活时长
            /// </summary>
            TimeAlive,

            /// <summary>
            /// 做种人数
            /// </summary>
            SeederNumber,

            /// <summary>
            /// 下载人数
            /// </summary>
            LeecherNumber,

            /// <summary>
            /// 完成人数
            /// </summary>
            SnatchedNumber,

            /// <summary>
            /// 发布者
            /// </summary>
            UpLoader,
        }

        /// <summary>
        /// 促销方式
        /// </summary>
        public enum PromotionType
        {
            /// <summary>
            /// 全部
            /// </summary>
            [Description("全部")]
            ALL,

            /// <summary>
            /// 普通
            /// </summary>
            [Description("NORMAL")]
            NORMAL,

            [Description("OTHER")]
            OTHER,

            /// <summary>
            /// 两倍上传
            /// </summary>
            [Description("2X")]
            TWOUP,

            /// <summary>
            /// 30%下载
            /// </summary>
            [Description("30%")]
            THIRTYPERDOWN,

            /// <summary>
            /// 50%下载
            /// </summary>
            [Description("50%")]
            HALFDOWN,

            /// <summary>
            /// 2X50%
            /// </summary>
            [Description("2X50%")]
            HALF2X,

            /// <summary>
            /// 免费
            /// </summary>
            [Description("FREE")]
            FREE,

            /// <summary>
            /// 2XFREE
            /// </summary>
            [Description("2XFREE")]
            FREE2UP,

        }


        /// <summary>
        /// HR方式
        /// </summary>
        public enum HRType
        {
            /// <summary>
            /// HR
            /// </summary>
            [Description("HR")]
            HR,

            /// <summary>
            /// NONE
            /// </summary>
            [Description("NONE")]
            NONE,
        }

        /// <summary>
        /// 种子Alive
        /// </summary>
        public enum AliveType
        {
            /// <summary>
            /// 全部
            /// </summary>
            [Description("全部")]
            ALL = 0,

            /// <summary>
            /// 活种
            /// </summary>
            [Description("活种")]
            Alive = 1,

            /// <summary>
            /// 断种
            /// </summary>
            [Description("断种")]
            Dead = 2,
        }

        /// <summary>
        /// 种子Fav类型
        /// </summary>
        public enum FavType
        {
            /// <summary>
            /// 全部
            /// </summary>
            [Description("全部")]
            ALL = 0,

            /// <summary>
            /// 已收藏
            /// </summary>
            [Description("已收藏")]
            Fav = 1,

            /// <summary>
            /// 未收藏
            /// </summary>
            [Description("未收藏")]
            None = 2,
        }


        /// <summary>
        /// 促销显示
        /// </summary>
        public enum PromotionDisplay
        {
            /// <summary>
            /// 高亮
            /// </summary>
            [Description("高亮显示")]
            HIGHLIGHT = 1,

            /// <summary>
            /// 添加标记
            /// </summary>
            [Description("添加标记")]
            MARK = 2,

            /// <summary>
            /// 添加图标
            /// </summary>
            [Description("添加图标")]
            LOGO = 3,

            /// <summary>
            /// 无标记
            /// /// </summary>
            [Description("无标记")]
            NONE = 4,
        }

        /// <summary>
        /// 排序类型
        /// </summary>

        public enum SortOrderType
        {
            /// <summary>
            /// 无
            /// </summary>
            None = 0,
            /// <summary>
            /// 正序
            /// </summary>
            Asc = 1,
            /// <summary>
            /// 倒序
            /// </summary>
            Desc = 2,
        }

        /// <summary>
        /// 用户映射
        /// </summary>
        public enum PersonInfoMap
        {
            /// <summary>
            /// 下载量
            /// </summary>
            DownSize = 0,

            /// <summary>
            /// 上传量
            /// </summary>
            UpSize = 1,

            /// <summary>
            /// 分享率
            /// </summary>
            ShareRate = 2,

            /// <summary>
            /// 保种数量
            SeedNumber = 3,

            /// <summary>
            /// 做种时间
            /// </summary>
            SeedTimes = 4,

            /// <summary>
            /// 下载时间
            /// </summary>
            DownTimes = 5,

            /// <summary>
            /// 做种率
            /// </summary>
            SeedRate = 6,

            /// <summary>
            /// 魔力值
            /// </summary>
            Bonus = 7,

            /// <summary>
            /// 注册日期
            /// </summary>
            RegisterDate = 8,

            /// <summary>
            /// 等级
            /// </summary>
            Rank = 9
        }
    }
}
