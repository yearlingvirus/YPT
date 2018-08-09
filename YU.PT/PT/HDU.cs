using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YU.Core;
using YU.Core.DataEntity;
using YU.Core.Utils;

namespace YU.PT
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

        public override string Sign(bool isAuto = false)
        {
            string signMsg = string.Empty;
            if (!VerifySign(ref signMsg))
                return signMsg;

            string postData = "action=qiandao";
            string htmlResult = HttpUtils.PostDataGetHtml(Site.SignUrl, postData, _cookie);
            return htmlResult;
        }
    }
}
