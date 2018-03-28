using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YU.Core;
using YU.Core.DataEntity;
using YU.Core.Event;

namespace YU.PT
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
        /// <param name="isAuto"></param>
        /// <returns></returns>
        string Sign(bool isAuto = false);

        /// <summary>
        /// 验证码事件
        /// </summary>
        /// <param name="e"></param>
        string OnVerificationCode(OnVerificationCodeEventArgs e);

        /// <summary>
        /// 下载文件事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        string OnPrepareDownFile(OnPrepareDownFileEventArgs e);

        /// <summary>
        /// 两步验证事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        string OnTwoStepVerification(OnTwoStepVerificationEventArgs e);

        /// <summary>
        /// 搜索种子
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<PTTorrent> SearchTorrent(PTSearchArgs args);

        /// <summary>
        /// 下载种子
        /// </summary>
        /// <param name="torrent"></param>
        /// <param name="isOpen"></param>
        /// <param name="isPostFileName"></param>
        void DownTorrent(PTTorrent torrent, bool isOpen, bool isPostFileName);

        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <returns></returns>
        PTInfo GetPersonInfo();
    }
}
