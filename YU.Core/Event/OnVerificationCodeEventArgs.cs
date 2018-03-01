using System;
using System.Collections.Generic;
using System.Linq;
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
        /// 站点
        /// </summary>
        public PTSite Site { get; set; }
    }
}
