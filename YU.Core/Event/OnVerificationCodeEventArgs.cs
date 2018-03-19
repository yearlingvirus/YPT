using System;
using System.Collections.Generic;
using System.Drawing;
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

        /// <summary>
        /// 图像（如果传入此参数，则忽略URL）
        /// </summary>
       public Image Image { get; set; }

        /// <summary>
        /// 默认验证阿妈
        /// </summary>
        public string Code { get; set; }
    }
}
