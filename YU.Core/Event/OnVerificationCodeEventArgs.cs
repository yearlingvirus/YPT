using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YU.Core.DataEntity;

namespace YU.Core.Event
{
    [Serializable]
    public class OnVerificationCodeEventArgs : EventArgs
    {
        /// <summary>
        /// 验证码链接
        /// </summary>
        public string VerificationCodeUrl { get; set; }

        /// <summary>
        /// 某些站点需要用Cookie来匹配
        /// </summary>
        public CookieContainer Cookie { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        public PTSite Site { get; set; }
    }
}
