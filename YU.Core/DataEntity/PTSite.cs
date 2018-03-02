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

        private Dictionary<YUEnums.PersonInfoMap, int> _personInfoMaps;

        public Dictionary<YUEnums.PersonInfoMap, int> PersonInfoMaps
        {
            get
            {
                if (_personInfoMaps == null)
                    _personInfoMaps = GetDefaultPersonInfoMaps();
                return _personInfoMaps;
            }
        }

        private Dictionary<YUEnums.TorrentMap, int> _torrentMaps;

        public Dictionary<YUEnums.TorrentMap, int> TorrentMaps
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
                _torrentMaps = new Dictionary<YUEnums.TorrentMap, int>()
                {
                    { YUEnums.TorrentMap.ResourceType, 0},
                    { YUEnums.TorrentMap.Detail, 1},
                    { YUEnums.TorrentMap.PromotionType, 1},
                    { YUEnums.TorrentMap.TimeAlive, 4},
                    { YUEnums.TorrentMap.Size, 6},
                    { YUEnums.TorrentMap.SeederNumber, 8},
                    { YUEnums.TorrentMap.LeecherNumber, 8},
                    { YUEnums.TorrentMap.SnatchedNumber, 7},
                    { YUEnums.TorrentMap.UpLoader, 9},
                },
                _personInfoMaps =  new Dictionary<YUEnums.PersonInfoMap, int>()
                {
                    { YUEnums.PersonInfoMap.RegisterDate, 0},
                    { YUEnums.PersonInfoMap.ShareRate, 5},
                    { YUEnums.PersonInfoMap.UpSize, 3},
                    { YUEnums.PersonInfoMap.DownSize, 4},
                    { YUEnums.PersonInfoMap.SeedRate, 6},
                    { YUEnums.PersonInfoMap.SeedTimes, 6},
                    { YUEnums.PersonInfoMap.DownTimes, 6},
                    { YUEnums.PersonInfoMap.SeedNumber, 6},
                    { YUEnums.PersonInfoMap.Rank, 8},
                    { YUEnums.PersonInfoMap.Bonus, 10},
                },
            },
            new PTSite() {
                Url = "https://tp.m-team.cc",
                Name = "MTEAM",
                Id = YUEnums.PTEnum.MTEAM,
                isEnableTwo_StepVerification = true,
                LoginUrl =  "https://tp.m-team.cc/takelogin.php",
                InfoUrl = "https://tp.m-team.cc/userdetails.php?id={0}",
                SearchUrl = "https://tp.m-team.cc/torrents.php",
                _torrentMaps = new Dictionary<YUEnums.TorrentMap, int>()
                {
                    { YUEnums.TorrentMap.ResourceType, 0},
                    { YUEnums.TorrentMap.Detail, 1},
                    { YUEnums.TorrentMap.PromotionType, 1},
                    { YUEnums.TorrentMap.TimeAlive, 3},
                    { YUEnums.TorrentMap.Size, 4},
                    { YUEnums.TorrentMap.SeederNumber, 5},
                    { YUEnums.TorrentMap.LeecherNumber, 6},
                    { YUEnums.TorrentMap.SnatchedNumber, 7},
                    { YUEnums.TorrentMap.UpLoader, 9},
                },
                _personInfoMaps = new Dictionary<YUEnums.PersonInfoMap, int>()
                {
                    { YUEnums.PersonInfoMap.RegisterDate, 1},
                    { YUEnums.PersonInfoMap.ShareRate, 5},
                    { YUEnums.PersonInfoMap.UpSize, 5},
                    { YUEnums.PersonInfoMap.DownSize, 5},
                    { YUEnums.PersonInfoMap.SeedRate, 7},
                    { YUEnums.PersonInfoMap.SeedTimes, 7},
                    { YUEnums.PersonInfoMap.DownTimes, 7},
                    { YUEnums.PersonInfoMap.SeedNumber, 7},
                    { YUEnums.PersonInfoMap.Rank, 10},
                    { YUEnums.PersonInfoMap.Bonus, 13},
                },
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
                _torrentMaps = new Dictionary<YUEnums.TorrentMap, int>()
                {
                    { YUEnums.TorrentMap.ResourceType, 0},
                    { YUEnums.TorrentMap.Detail, 1},
                    { YUEnums.TorrentMap.PromotionType, 1},
                    { YUEnums.TorrentMap.TimeAlive, 3},
                    { YUEnums.TorrentMap.Size, 4},
                    { YUEnums.TorrentMap.SeederNumber, 5},
                    { YUEnums.TorrentMap.LeecherNumber, 6},
                    { YUEnums.TorrentMap.SnatchedNumber, 7},
                    { YUEnums.TorrentMap.UpLoader, 9},
                },
                _personInfoMaps = new Dictionary<YUEnums.PersonInfoMap, int>()
                {
                    { YUEnums.PersonInfoMap.RegisterDate, 2},
                    { YUEnums.PersonInfoMap.ShareRate, 7},
                    { YUEnums.PersonInfoMap.UpSize, 7},
                    { YUEnums.PersonInfoMap.DownSize, 7},
                    { YUEnums.PersonInfoMap.SeedRate, 8},
                    { YUEnums.PersonInfoMap.SeedTimes, 8},
                    { YUEnums.PersonInfoMap.DownTimes, 8},
                    { YUEnums.PersonInfoMap.SeedNumber, 8},
                    { YUEnums.PersonInfoMap.Rank, 10},
                    { YUEnums.PersonInfoMap.Bonus, 13},
                },
            },
            new PTSite() {
                Url = "https://pt.keepfrds.com",
                Name = "KEEPFRDS",
                Id = YUEnums.PTEnum.KEEPFRDS,
                IsEnableVerificationCode = true,
                LoginUrl =  "https://pt.keepfrds.com/takelogin.php",
                InfoUrl = "https://pt.keepfrds.com/userdetails.php?id={0}",
                SearchUrl = "https://pt.keepfrds.com/torrents.php"
            }
        };


        private Dictionary<YUEnums.TorrentMap, int> GetDefaultTorrentMaps()
        {
            return new Dictionary<YUEnums.TorrentMap, int>()
            {
                { YUEnums.TorrentMap.ResourceType, 0},
                { YUEnums.TorrentMap.Detail, 1},
                { YUEnums.TorrentMap.PromotionType, 1},
                { YUEnums.TorrentMap.TimeAlive, 3},
                { YUEnums.TorrentMap.Size, 4},
                { YUEnums.TorrentMap.SeederNumber, 5},
                { YUEnums.TorrentMap.LeecherNumber, 6},
                { YUEnums.TorrentMap.SnatchedNumber, 7},
                { YUEnums.TorrentMap.UpLoader, 8},
            };
        }

        private Dictionary<YUEnums.PersonInfoMap, int> GetDefaultPersonInfoMaps()
        {
            return new Dictionary<YUEnums.PersonInfoMap, int>()
            {
                { YUEnums.PersonInfoMap.RegisterDate, 2},
                { YUEnums.PersonInfoMap.ShareRate, 6},
                { YUEnums.PersonInfoMap.UpSize, 6},
                { YUEnums.PersonInfoMap.DownSize, 6},
                { YUEnums.PersonInfoMap.SeedRate, 7},
                { YUEnums.PersonInfoMap.SeedTimes, 7},
                { YUEnums.PersonInfoMap.DownTimes, 7},
                { YUEnums.PersonInfoMap.SeedNumber, 7},
                { YUEnums.PersonInfoMap.Rank, 9},
                { YUEnums.PersonInfoMap.Bonus, 12},
            };
        }

        #endregion
    }
}
