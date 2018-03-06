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
        public string Url { get; set; }

        public string Name { get; set; }

        public string LoginUrl { get; set; }

        public string SignUrl { get; set; }

        public string SearchUrl { get; set; }

        public string InfoUrl { get; set; }

        public bool IsEableSecurityQuestion { get; set; }

        public bool IsEnableVerificationCode { get; set; }

        public bool isEnableTwo_StepVerification { get; set; }

        private Dictionary<YUEnums.PersonInfoMap, string[]> _personInfoMaps;

        public Dictionary<YUEnums.PersonInfoMap, string[]> PersonInfoMaps
        {
            get
            {
                if (_personInfoMaps == null)
                    _personInfoMaps = GetDefaultPersonInfoMaps();
                return _personInfoMaps;
            }
        }

        private Dictionary<YUEnums.TorrentMap, string[]> _torrentMaps;

        public Dictionary<YUEnums.TorrentMap, string[]> TorrentMaps
        {
            get
            {
                if (_torrentMaps == null)
                    _torrentMaps = GetDefaultTorrentMaps();
                return _torrentMaps;
            }
        }

        public YUEnums.PTEnum Id { get; set; }


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
                SearchUrl = "https://totheglory.im/browse.php",
            },
            new PTSite() {
                Url = "https://tp.m-team.cc",
                Name = "MTEAM",
                Id = YUEnums.PTEnum.MTEAM,
                isEnableTwo_StepVerification = true,
                LoginUrl =  "https://tp.m-team.cc/takelogin.php",
                InfoUrl = "https://tp.m-team.cc/userdetails.php?id={0}",
                SearchUrl = "https://tp.m-team.cc/torrents.php",

            },
            new PTSite() {
                Url = "https://chdbits.co",
                Name = "CHDBITS",
                Id = YUEnums.PTEnum.CHDBITS,
                LoginUrl =  "https://chdbits.co/takelogin.php",
                InfoUrl = "https://chdbits.co/userdetails.php?id={0}",
                SearchUrl = "https://chdbits.co/torrents.php"
            },
            new PTSite() {
                Url = "https://ourbits.club",
                Name = "OURBITS",
                Id = YUEnums.PTEnum.OURBITS,
                LoginUrl =  "https://ourbits.club/takelogin.php",
                InfoUrl = "https://ourbits.club/userdetails.php?id={0}",
                SignUrl = "https://ourbits.club/attendance.php",
                SearchUrl = "https://ourbits.club/torrents.php",
            },
            new PTSite() {
                Url = "https://pt.keepfrds.com",
                Name = "KEEPFRDS",
                Id = YUEnums.PTEnum.KEEPFRDS,
                IsEnableVerificationCode = true,
                LoginUrl =  "https://pt.keepfrds.com/takelogin.php",
                InfoUrl = "https://pt.keepfrds.com/userdetails.php?id={0}",
                SearchUrl = "https://pt.keepfrds.com/torrents.php"
            },
            new PTSite() {
                Url = "http://pt.btschool.net",
                Name = "BTSCHOOL",
                Id = YUEnums.PTEnum.BTSCHOOL,
                IsEnableVerificationCode = true,
                LoginUrl =  "http://pt.btschool.net/takelogin.php",
                InfoUrl = "http://pt.btschool.net/userdetails.php?id={0}",
                SignUrl = "http://pt.btschool.net/index.php?action=addbonus",
                SearchUrl = "http://pt.btschool.net/torrents.php",
            },
            new PTSite() {
                Url = "https://pt.gztown.net",
                Name = "GZTOWN",
                Id = YUEnums.PTEnum.GZTOWN,
                IsEnableVerificationCode = true,
                LoginUrl =  "https://pt.gztown.net/takelogin.php",
                InfoUrl = "https://pt.gztown.net/userdetails.php?id={0}",
                SignUrl = "https://pt.gztown.net/attendance.php",
                SearchUrl = "https://pt.gztown.net/torrents.php",
            },
            new PTSite() {
                Url = "https://pt.upxin.net",
                Name = "HDU",
                Id = YUEnums.PTEnum.HDU,
                IsEnableVerificationCode = true,
                LoginUrl =  "https://pt.upxin.net/takelogin.php",
                InfoUrl = "https://pt.upxin.net/userdetails.php?id={0}",
                SignUrl = "https://pt.upxin.net/added.php",
                SearchUrl = "https://pt.upxin.net/torrents.php",
            },
            new PTSite() {
                Url = "https://nanyangpt.com/",
                Name = "NY",
                Id = YUEnums.PTEnum.NY,
                IsEnableVerificationCode = true,
                LoginUrl =  "https://nanyangpt.com//takelogin.php",
                InfoUrl = "https://nanyangpt.com//userdetails.php?id={0}",
                SignUrl = "",
                SearchUrl = "https://nanyangpt.com//torrents.php",
            },
        };


        private Dictionary<YUEnums.TorrentMap, string[]> GetDefaultTorrentMaps()
        {
            return new Dictionary<YUEnums.TorrentMap, string[]>()
            {
                { YUEnums.TorrentMap.ResourceType, new string[] { "类型", "类别" , "類型", "Cat.","Type" } },
                { YUEnums.TorrentMap.Detail, new string[] {"标题", "標題", "Name", "名称"} },
                { YUEnums.TorrentMap.PromotionType, new string[] {"标题", "標題", "Name", "名称"} },
                { YUEnums.TorrentMap.TimeAlive,  new string[] { "time", "存活时间", "存活時間", "TTL" } },
                { YUEnums.TorrentMap.Size,  new string[] { "size", "大小" } },
                { YUEnums.TorrentMap.SeederNumber,  new string[] { "seeders", "做种", "种子数", "種子數"} },
                { YUEnums.TorrentMap.LeecherNumber, new string[] { "leechers", "下载", "下載數"} },
                { YUEnums.TorrentMap.SnatchedNumber,  new string[] { "snatched", "完成", "完成數" } },
                { YUEnums.TorrentMap.UpLoader,  new string[] { "发布者", "發佈者", "上传者"} },
            };
        }

        private Dictionary<YUEnums.PersonInfoMap, string[]> GetDefaultPersonInfoMaps()
        {
            return new Dictionary<YUEnums.PersonInfoMap, string[]>()
            {
                { YUEnums.PersonInfoMap.Bonus, new string[] {"魔力值", "魔力豆", "积分", "Karma Points", "Bonus", "포인트" }},
                { YUEnums.PersonInfoMap.RegisterDate, new string[] { "Join date", "加入日期", "注册日期", "가입 날짜" } },
                { YUEnums.PersonInfoMap.ShareRate, new string[] { "Transfers", "传输", "傳送", "分享率", "Ratio" }},
                { YUEnums.PersonInfoMap.DownSize, new string[] { "Transfers", "传输", "傳送", "下载量", "Downloaded", "다운로드" }},
                { YUEnums.PersonInfoMap.UpSize, new string[] { "Transfers", "传输", "傳送", "上传量", "Uploaded", "업로드" }},
                { YUEnums.PersonInfoMap.SeedRate, new string[] { "Torrenting Time", "BT时间", "BT時間", "做种/下载时间比率", "Seed/Leech time ratio", "시딩/리칭 시간 비율" }},
                { YUEnums.PersonInfoMap.SeedTimes, new string[] { "做种率", "Torrenting Time", "BT时间", "BT時間", "做种/下载时间比率", "Seed/Leech time ratio", "시딩/리칭 시간 비율" }},
                { YUEnums.PersonInfoMap.DownTimes, new string[] { "做种率", "Torrenting Time", "BT时间", "BT時間", "做种/下载时间比率", "Seed/Leech time ratio", "시딩/리칭 시간 비율" }},
                { YUEnums.PersonInfoMap.SeedNumber, new string[] { "做种率", "Torrenting Time", "BT时间", "BT時間", "做种/下载时间比率", "Seed/Leech time ratio", "시딩/리칭 시간 비율" }},
                { YUEnums.PersonInfoMap.Rank, new string[] { "Class", "等级", "等級", "등급" }}
            };
        }

        #endregion
    }
}
