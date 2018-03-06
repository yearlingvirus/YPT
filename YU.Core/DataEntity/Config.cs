using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YU.Core.DataEntity
{
    [Serializable]
    public class Config
    {
        /// <summary>
        /// 签到时间
        /// </summary>
        public DateTime SignTime { get; set; }

        /// <summary>
        /// 自动签到
        /// </summary>
        public bool IsAutoSign { get; set; }

        /// <summary>
        /// 准点同步信息
        /// </summary>
        public bool IsSyncTiming { get; set; }

        /// <summary>
        /// 请求服务器下载文件名
        /// </summary>
        public bool IsEnablePostFileName { get; set; }

        /// <summary>
        /// 关闭主面板时是否最小化到系统托盘
        /// </summary>
        public bool IsMiniWhenClose { get; set; }

        /// <summary>
        /// 首次使用程序
        /// </summary>
        public bool IsFirstOpen { get; set; }
    }
}
