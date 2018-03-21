using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YU.Core;
using YU.Core.DataEntity;

namespace YU.PT
{
    /// <summary>
    /// 用户自行扩展的PT站点
    /// </summary>
    public class ExtendPT : AbstractPT
    {
        private int _siteId = 0;

        public ExtendPT(PTUser user, int siteId)
        {
            if (siteId == 0)
                throw new Exception("siteId cannot be zero");
            _siteId = siteId;

            if (user == null)
                throw new Exception("User is null");
            if (user.Site == null)
                throw new Exception("Site is null");
            if (user.Site.Id != SiteId)
                throw new Exception("SiteId not correct");
            _user = user;
            _site = user.Site;
            _cookie = GetLocalCookie();
        }

        protected override YUEnums.PTEnum SiteId
        {
            get
            {
                if (_siteId == 0)
                    throw new NotImplementedException();
                else
                    return (YUEnums.PTEnum)_siteId;
            }
        }

    }
}
