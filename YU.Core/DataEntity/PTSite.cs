using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YU.Core.Utils;

namespace YU.Core.DataEntity
{
    [Serializable]
    public class PTSite
    {

        /// <summary>
        /// 站点Id
        /// </summary>
        public YUEnums.PTEnum Id { get; set; }


        /// <summary>
        /// URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 站点名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 登录URL
        /// </summary>
        public string LoginUrl { get; set; }

        /// <summary>
        /// 签到URL
        /// </summary>
        public string SignUrl { get; set; }

        /// <summary>
        /// 个人信息URL
        /// </summary>
        public string InfoUrl { get; set; }

        /// <summary>
        /// 是否启用安全问题
        /// </summary>
        public bool IsEableSecurityQuestion { get; set; }

        /// <summary>
        /// 是否启用验证码
        /// </summary>
        public bool IsEnableVerificationCode { get; set; }

        /// <summary>
        /// 是否启用两步验证
        /// </summary>
        public bool isEnableTwo_StepVerification { get; set; }

        /// <summary>
        /// 是否通过邮箱登陆
        /// </summary>
        public bool IsLoginByMail { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 板块
        /// </summary>
        public List<PTForum> Forums { get; set; }

        /// <summary>
        /// 个人信息映射
        /// </summary>
        private Dictionary<YUEnums.PersonInfoMap, string[]> _personInfoMaps;

        /// <summary>
        /// 个人信息映射
        /// </summary>
        public Dictionary<YUEnums.PersonInfoMap, string[]> PersonInfoMaps
        {
            get
            {
                if (_personInfoMaps == null)
                    _personInfoMaps = GetDefaultPersonInfoMaps();
                return _personInfoMaps;
            }
        }

        /// <summary>
        /// 种子信息映射
        /// </summary>
        private Dictionary<YUEnums.TorrentMap, string[]> _torrentMaps;

        /// <summary>
        /// 种子信息映射
        /// </summary>
        public Dictionary<YUEnums.TorrentMap, string[]> TorrentMaps
        {
            get
            {
                if (_torrentMaps == null)
                    _torrentMaps = GetDefaultTorrentMaps();
                return _torrentMaps;
            }
        }

        /// <summary>
        /// 搜索排序URL映射
        /// </summary>
        private Dictionary<string, string> _searchColOrderUrlMaps;

        /// <summary>
        /// 搜索排序URL映射
        /// </summary>
        public Dictionary<string, string> SearchColOrderUrlMaps
        {
            get
            {
                if (_searchColOrderUrlMaps == null)
                    _searchColOrderUrlMaps = GetDefaultSearchOrderUrlMaps();
                return _searchColOrderUrlMaps;
            }
        }

        /// <summary>
        /// 促销搜索URL映射
        /// </summary>
        private Dictionary<YUEnums.PromotionType, string> _promotionSearchUrlMaps;

        /// <summary>
        /// 促销搜索URL映射
        /// </summary>
        public Dictionary<YUEnums.PromotionType, string> PromotionSearchUrlMaps
        {
            get
            {
                if (_promotionSearchUrlMaps == null)
                    _promotionSearchUrlMaps = GetDefaultPromotionSearchUrlMaps();
                return _promotionSearchUrlMaps;
            }
        }

        /// <summary>
        /// 获取默认种子映射
        /// </summary>
        /// <returns></returns>
        private Dictionary<YUEnums.TorrentMap, string[]> GetDefaultTorrentMaps()
        {
            return new Dictionary<YUEnums.TorrentMap, string[]>()
            {
                { YUEnums.TorrentMap.ResourceType, new string[] { "类型", "类别" , "類型", "分类", "Cat.","Type" } },
                { YUEnums.TorrentMap.Detail, new string[] {"标题", "標題", "Name", "名称"} },
                { YUEnums.TorrentMap.PromotionType, new string[] {"标题", "標題", "Name", "名称"} },
                { YUEnums.TorrentMap.TimeAlive,  new string[] { "time", "存活时间", "存活時間", "生存时间", "TTL" } },
                { YUEnums.TorrentMap.Size,  new string[] { "size", "大小" } },
                { YUEnums.TorrentMap.SeederNumber,  new string[] { "seeders", "做种", "种子数", "種子數"} },
                { YUEnums.TorrentMap.LeecherNumber, new string[] { "leechers", "下载", "下載數"} },
                { YUEnums.TorrentMap.SnatchedNumber,  new string[] { "snatched", "完成", "完成數" } },
                { YUEnums.TorrentMap.UpLoader,  new string[] { "发布者", "發佈者", "上传者"} },
            };
        }

        /// <summary>
        /// 获取默认个人信息映射
        /// </summary>
        /// <returns></returns>
        private Dictionary<YUEnums.PersonInfoMap, string[]> GetDefaultPersonInfoMaps()
        {
            return new Dictionary<YUEnums.PersonInfoMap, string[]>()
            {
                { YUEnums.PersonInfoMap.Bonus, new string[] {"魔力值", "魔力豆", "积分", "UCoin", "Karma Points", "Bonus", "포인트" }},
                { YUEnums.PersonInfoMap.RegisterDate, new string[] { "Join date", "加入日期", "注册日期", "가입 날짜" } },
                { YUEnums.PersonInfoMap.ShareRate, new string[] { "Transfers", "传输", "傳送", "分享率", "Ratio" }},
                { YUEnums.PersonInfoMap.DownSize, new string[] { "Transfers", "传输", "傳送", "下载量", "Downloaded", "다운로드" }},
                { YUEnums.PersonInfoMap.UpSize, new string[] { "Transfers", "传输", "傳送", "上传量", "Uploaded", "업로드" }},
                { YUEnums.PersonInfoMap.SeedRate, new string[] { "做种率", "Torrenting Time", "BT时间", "BT時間", "做种/下载时间比率", "Seed/Leech time ratio", "시딩/리칭 시간 비율" }},
                { YUEnums.PersonInfoMap.SeedTimes, new string[] { "做种率", "Torrenting Time", "BT时间", "BT時間", "做种/下载时间比率", "Seed/Leech time ratio", "시딩/리칭 시간 비율" }},
                { YUEnums.PersonInfoMap.DownTimes, new string[] { "做种率", "Torrenting Time", "BT时间", "BT時間", "做种/下载时间比率", "Seed/Leech time ratio", "시딩/리칭 시간 비율" }},
                { YUEnums.PersonInfoMap.SeedNumber, new string[] { "当前做种", "當前做種", "Currently Seeding" }},
                { YUEnums.PersonInfoMap.Rank, new string[] { "Class", "等级", "等級", "등급" }}
            };
        }

        /// <summary>
        /// 获取默认搜索排序映射
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetDefaultSearchOrderUrlMaps()
        {
            return new Dictionary<string, string>()
            {
                { "Title","1" },
                { "Size","5" },
                { "UpLoadTime","4" },
                { "SeederNumber","7" },
                { "LeecherNumber","8" },
                { "SnatchedNumber","6" },
                { "UpLoader","9" },
            };
        }

        /// <summary>
        /// 获取默认促销搜索URL映射
        /// </summary>
        /// <returns></returns>
        private Dictionary<YUEnums.PromotionType, string> GetDefaultPromotionSearchUrlMaps()
        {
            return new Dictionary<YUEnums.PromotionType, string>()
            {
                { YUEnums.PromotionType.ALL, "0" },
                { YUEnums.PromotionType.NORMAL, "1" },
                { YUEnums.PromotionType.FREE, "2" },
                { YUEnums.PromotionType.TWOUP, "3" },
                { YUEnums.PromotionType.FREE2UP, "4" },
                { YUEnums.PromotionType.HALFDOWN, "5" },
                { YUEnums.PromotionType.HALF2X, "6" },
                { YUEnums.PromotionType.THIRTYPERDOWN, "7" },
                { YUEnums.PromotionType.OTHER, "8" },
            };
        }

        #region 系统预置站点

        public readonly static List<PTSite> Sites = new List<PTSite>()
        {
            new PTSite() {
                Url = "https://totheglory.im",
                Name = "TTG",
                Id = YUEnums.PTEnum.TTG,
                IsEableSecurityQuestion = true,
                isEnableTwo_StepVerification = true,
                LoginUrl =  "https://totheglory.im/takelogin.php",
                SignUrl = "https://totheglory.im/signed.php",
                InfoUrl = "https://totheglory.im/userdetails.php?id={0}",
                Forums = new List<PTForum>()
                {
                    new PTForum() { SiteId = YUEnums.PTEnum.TTG, Name = "TTG", SearchUrl = "https://totheglory.im/browse.php", Visable = true, Order = 1 },
                },
                Order = (int)YUEnums.PTEnum.TTG,
            },
            new PTSite() {
                Url = "https://tp.m-team.cc",
                Name = "MTeam",
                Id = YUEnums.PTEnum.MTeam,
                isEnableTwo_StepVerification = true,
                LoginUrl =  "https://tp.m-team.cc/takelogin.php",
                InfoUrl = "https://tp.m-team.cc/userdetails.php?id={0}",
                Forums = new List<PTForum>()
                {
                    new PTForum() { SiteId = YUEnums.PTEnum.MTeam, Name = "MTeam", SearchUrl = "https://tp.m-team.cc/torrents.php", Visable = true, Order = 1 },
                    new PTForum() { SiteId = YUEnums.PTEnum.MTeam, Name = "MTeam_Adult", SearchUrl = "https://tp.m-team.cc/adult.php", Visable = false, Order = 2 },
                },
                Order = (int)YUEnums.PTEnum.MTeam,
            },
            new PTSite() {
                Url = "https://hdsky.me",
                Name = "HDSky",
                Id = YUEnums.PTEnum.HDSky,
                IsEnableVerificationCode = true,
                isEnableTwo_StepVerification = true,
                LoginUrl =  "https://hdsky.me/takelogin.php",
                InfoUrl = "https://hdsky.me/userdetails.php?id={0}",
                SignUrl = "https://hdsky.me/showup.php",
                Forums = new List<PTForum>()
                {
                    new PTForum() { SiteId = YUEnums.PTEnum.HDSky, Name = "HDSky", SearchUrl = "https://hdsky.me/torrents.php", Visable = true, Order = 1 },
                },
                Order = (int)YUEnums.PTEnum.HDSky,
            },
            new PTSite() {
                Url = "https://chdbits.co",
                Name = "CHDBits",
                Id = YUEnums.PTEnum.CHDBits,
                LoginUrl =  "https://chdbits.co/takelogin.php",
                InfoUrl = "https://chdbits.co/userdetails.php?id={0}",
                Forums = new List<PTForum>()
                {
                    new PTForum() { SiteId = YUEnums.PTEnum.CHDBits, Name = "CHDBits", SearchUrl = "https://chdbits.co/torrents.php", Visable = true, Order = 1 },
                },
                Order = (int)YUEnums.PTEnum.CHDBits,
            },
            new PTSite() {
                Url = "https://hdcmct.org",
                Name = "CMCT",
                Id = YUEnums.PTEnum.CMCT,
                IsEnableVerificationCode = true,
                LoginUrl =  "https://hdcmct.org/takelogin.php",
                InfoUrl = "https://hdcmct.org/userdetails.php?id={0}",
                Forums = new List<PTForum>()
                {
                    new PTForum() { SiteId = YUEnums.PTEnum.CMCT, Name = "CMCT", SearchUrl = "https://hdcmct.org/torrents.php", Visable = true, Order = 1 },
                },
                Order = (int)YUEnums.PTEnum.CMCT,
            },
            new PTSite() {
                Url = "https://ourbits.club",
                Name = "OurBits",
                Id = YUEnums.PTEnum.OurBits,
                LoginUrl =  "https://ourbits.club/takelogin.php",
                InfoUrl = "https://ourbits.club/userdetails.php?id={0}",
                SignUrl = "https://ourbits.club/attendance.php",
                Forums = new List<PTForum>()
                {
                    new PTForum() { SiteId = YUEnums.PTEnum.OurBits, Name = "OurBits", SearchUrl = "https://ourbits.club/torrents.php", Visable = true, Order = 1 },
                },
                Order = (int)YUEnums.PTEnum.OurBits,
            },
            new PTSite() {
                Url = "https://pt.keepfrds.com",
                Name = "KeepFrds",
                Id = YUEnums.PTEnum.KeepFrds,
                IsEnableVerificationCode = true,
                LoginUrl =  "https://pt.keepfrds.com/takelogin.php",
                InfoUrl = "https://pt.keepfrds.com/userdetails.php?id={0}",
                Forums = new List<PTForum>()
                {
                    new PTForum() { SiteId = YUEnums.PTEnum.KeepFrds, Name = "KeepFrds", SearchUrl = "https://pt.keepfrds.com/torrents.php", Visable = true, Order = 1 },
                },
                Order = (int)YUEnums.PTEnum.KeepFrds,
            },
            new PTSite() {
                Url = "https://u2.dmhy.org",
                Name = "U2",
                Id = YUEnums.PTEnum.U2,
                IsLoginByMail = true,
                IsEnableVerificationCode = true,
                LoginUrl =  "https://u2.dmhy.org/takelogin.php",
                InfoUrl = "https://u2.dmhy.org/userdetails.php?id={0}",
                Forums = new List<PTForum>()
                {
                    new PTForum() { SiteId = YUEnums.PTEnum.U2, Name = "U2", SearchUrl = "https://u2.dmhy.org/torrents.php", Visable = true, Order = 1 },
                },
                Order = (int)YUEnums.PTEnum.U2,
            },
            new PTSite() {
                Url = "https://hdhome.org/",
                Name = "HDHome",
                Id = YUEnums.PTEnum.HDHome,
                IsEnableVerificationCode = true,
                LoginUrl =  "https://hdhome.org/takelogin.php",
                InfoUrl = "https://hdhome.org/userdetails.php?id={0}",
                SignUrl = "https://hdhome.org/attendance.php",
                Forums = new List<PTForum>()
                {
                    new PTForum() { SiteId = YUEnums.PTEnum.HDHome, Name = "HDHome", SearchUrl = "https://hdhome.org/torrents.php", Visable = true, Order = 1 },
                },
                Order = (int)YUEnums.PTEnum.HDHome,
            },
            new PTSite() {
                Url = "https://open.cd/",
                Name = "OpenCD",
                Id = YUEnums.PTEnum.OpenCD,
                IsEnableVerificationCode = true,
                LoginUrl =  "https://open.cd/takelogin.php",
                InfoUrl = "https://open.cd/userdetails.php?id={0}",
                SignUrl = "https://open.cd/plugin_sign-in.php",
                Forums = new List<PTForum>()
                {
                    new PTForum() { SiteId = YUEnums.PTEnum.OpenCD, Name = "OpenCD", SearchUrl = "https://open.cd/torrents.php", Visable = true, Order = 1 },
                    new PTForum() { SiteId = YUEnums.PTEnum.OpenCD, Name = "OpenCD_Music", SearchUrl = "https://open.cd/torrents.php?boardid=1", Visable = false, Order = 2 },
                    new PTForum() { SiteId = YUEnums.PTEnum.OpenCD, Name = "OpenCD_Catch", SearchUrl = "https://open.cd/torrents.php?boardid=2", Visable = false, Order = 3 },
                },
                Order = (int)YUEnums.PTEnum.OpenCD,
            },
            new PTSite() {
                Url = "https://pt.gztown.net",
                Name = "GZTown",
                Id = YUEnums.PTEnum.GZTown,
                IsEnableVerificationCode = true,
                LoginUrl =  "https://pt.gztown.net/takelogin.php",
                InfoUrl = "https://pt.gztown.net/userdetails.php?id={0}",
                SignUrl = "https://pt.gztown.net/attendance.php",
                Forums = new List<PTForum>()
                {
                    new PTForum() { SiteId = YUEnums.PTEnum.GZTown, Name = "GZTown",  SearchUrl = "https://pt.gztown.net/torrents.php", Visable = true, Order = 1 },
                },
                Order = (int)YUEnums.PTEnum.GZTown,
            },
            new PTSite() {
                Url = "http://pt.btschool.net",
                Name = "BTSchool",
                Id = YUEnums.PTEnum.BTSchool,
                IsEnableVerificationCode = true,
                LoginUrl =  "http://pt.btschool.net/takelogin.php",
                InfoUrl = "http://pt.btschool.net/userdetails.php?id={0}",
                SignUrl = "http://pt.btschool.net/index.php?action=addbonus",
                Forums = new List<PTForum>()
                {
                    new PTForum() { SiteId = YUEnums.PTEnum.BTSchool, Name = "BTSchool", SearchUrl = "http://pt.btschool.net/torrents.php", Visable = true, Order = 1 },
                },
                Order = (int)YUEnums.PTEnum.BTSchool,
            },
            new PTSite() {
                Url = "https://pt.upxin.net",
                Name = "HDU",
                Id = YUEnums.PTEnum.HDU,
                IsEnableVerificationCode = true,
                LoginUrl =  "https://pt.upxin.net/takelogin.php",
                InfoUrl = "https://pt.upxin.net/userdetails.php?id={0}",
                SignUrl = "https://pt.upxin.net/added.php",
                Forums = new List<PTForum>()
                {
                    new PTForum() { SiteId = YUEnums.PTEnum.HDU, Name = "HDU", SearchUrl = "https://pt.upxin.net/torrents.php", Visable = true, Order = 1 },
                },
                Order = (int)YUEnums.PTEnum.HDU,
            },
            new PTSite() {
                Url = "https://nanyangpt.com/",
                Name = "NYPT",
                Id = YUEnums.PTEnum.NYPT,
                IsEnableVerificationCode = true,
                LoginUrl =  "https://nanyangpt.com/takelogin.php",
                InfoUrl = "https://nanyangpt.com/userdetails.php?id={0}",
                SignUrl = "",
                Forums = new List<PTForum>()
                {
                    new PTForum() { SiteId = YUEnums.PTEnum.NYPT, Name = "NYPT", SearchUrl = "https://nanyangpt.com/torrents.php", Visable = true, Order = 1 },
                },
                Order = (int)YUEnums.PTEnum.NYPT,
            },

        };


        #endregion
    }
}
