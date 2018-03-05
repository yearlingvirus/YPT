using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YU.Core;
using YU.Core.DataEntity;
using YU.Core.Utils;

namespace YPT.PT
{
    public class HDU : AbstractPT
    {
        public HDU(PTUser user)
            : base(user)
        {

        }

        protected override YUEnums.PTEnum SiteId
        {
            get
            {
                return YUEnums.PTEnum.HDU;
            }
        }

        public override string Sign()
        {
            if (_cookie != null && _cookie.Count > 0)
            {
                string postData = "action=qiandao";
                string htmlResult = HttpUtils.PostDataGetHtml(Site.SignUrl, postData, _cookie);
                return htmlResult;
            }
            else
            {
                return "无法获取Cookie信息，签到失败，请重新登录系统。";
            }
        }
    }
}
