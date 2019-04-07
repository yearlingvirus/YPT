using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using YU.Core;
using YU.Core.DataEntity;
using YU.Core.Event;
using YU.Core.Utils;

namespace YU.PT
{
    public class KEEPFRDS : AbstractPT
    {
        public KEEPFRDS(PTUser user)
            : base(user)
        {

        }

        protected override YUEnums.PTEnum SiteId
        {
            get
            {
                return YUEnums.PTEnum.KeepFrds;
            }
        }

        protected override Tuple<string, HttpWebRequest, HttpWebResponse> DoLoginWhenEnableTwo_StepVerification(Tuple<string, HttpWebRequest, HttpWebResponse> cookieResult, string otpCode)
        {
            string postData = $"username={User.UserName}&password={User.PassWord}&g2fa_code={otpCode}";
            var result = HttpUtils.PostData(Site.LoginUrl, postData, _cookie);
            return result;
        }

        protected override void AfterSetPersonInfo(Dictionary<YUEnums.PersonInfoMap, int> infoMaps, HtmlNodeCollection nodes, PTInfo info)
        {
            //积分
            var node = nodes[infoMaps[YUEnums.PersonInfoMap.Bonus]];
            if (node != null)
            {
                node = node.SelectSingleNode(".//tr[2]/td/text()");
                string bonusString = node.InnerText;
                //移除空白之前的字符串
                bonusString = bonusString.Substring(bonusString.IndexOf(" "));
                //移除空格之后的字符串
                bonusString = bonusString.Substring(0, bonusString.IndexOf("&nbsp"));
                info.Bonus = bonusString.Trim().TryPareValue<double>();
            }
        }


    }
}
