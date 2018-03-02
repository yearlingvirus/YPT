using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YU.Core;
using YU.Core.DataEntity;

namespace YPT.PT
{
    /// <summary>
    /// 用户自行扩展的PT站点
    /// </summary>
    public class ExtendPT : AbstractPT
    {
        private int _siteId = 0;

        public ExtendPT(PTUser user, int siteId)
        {
            _siteId = siteId;
            Site = Global.Sites.Where(x => x.Id == SiteId).FirstOrDefault();
            _user = user;
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
