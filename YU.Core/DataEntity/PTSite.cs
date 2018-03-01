using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool isEnableTwo_StepVerification { get; set; }

        public YUEnums.PTEnum Id { get; set; }

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
                SearchUrl = "https://totheglory.im/browse.php"
            },
            new PTSite() {
                Url = "https://tp.m-team.cc",
                Name = "MTEAM",
                Id = YUEnums.PTEnum.MTEAM,
                isEnableTwo_StepVerification = true,
                LoginUrl =  "https://tp.m-team.cc/takelogin.php",
                InfoUrl = "https://tp.m-team.cc/userdetails.php?id={0}",
                SearchUrl = "https://tp.m-team.cc/torrents.php"
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
                SearchUrl = "https://ourbits.club/torrents.php"
            },
            new PTSite() {
                Url = "https://pt.keepfrds.com",
                Name = "KEEPFRDS",
                Id = YUEnums.PTEnum.KEEPFRDS,
                LoginUrl =  "https://pt.keepfrds.com/takelogin.php",
                InfoUrl = "https://pt.keepfrds.com/userdetails.php?id={0}",
                SearchUrl = "https://pt.keepfrds.com/torrents.php"
            }
        };
    }
}
