using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YU.Core;
using YU.Core.DataEntity;
using YU.Core.Event;

namespace YPT.PT
{
    /// <summary>
    /// PT
    /// </summary>
    public interface IPT
    {
        /// <summary>
        /// 登陆
        /// </summary>
        /// <returns></returns>
        string Login();

        /// <summary>
        /// 签到
        /// </summary>
        string Sign();

        /// <summary>
        /// 验证码事件
        /// </summary>
        /// <param name="e"></param>
        string OnVerificationCode(OnVerificationCodeEventArgs e);

        /// <summary>
        /// 搜索种子
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        List<PTTorrent> SearchTorrent(string searchKey, YUEnums.PromotionType promotionType, YUEnums.AliveType aliveType, YUEnums.FavType favType);

        /// <summary>
        /// 获取种子下载的文件名
        /// </summary>
        /// <param name="torrent"></param>
        /// <returns></returns>
        string GetTorrentDownFileName(PTTorrent torrent);

        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <returns></returns>
        PTInfo GetPersonInfo();
    }
}
