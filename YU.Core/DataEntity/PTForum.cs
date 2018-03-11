using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YU.Core.DataEntity
{
    /// <summary>
    /// PT版块
    /// </summary>
    [Serializable]
    public class PTForum
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public YUEnums.PTEnum SiteId { get; set; }

        /// <summary>
        /// 版块名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 搜索URL
        /// </summary>
        public string SearchUrl { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 可见性
        /// </summary>
        public bool Visable { get; set; }

    }
}
