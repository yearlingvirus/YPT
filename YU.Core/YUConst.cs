using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YU.Core
{
    public class YUConst
    {

        /// <summary>
        /// 设置节-SQLLiteDB
        /// </summary>
        public const string SETTING_SQLLITEDB = "SQLLiteDB";

        /// <summary>
        /// 设置节-SQLLiteDBPassWord
        /// </summary>
        public const string SETTING_SQLLITEDBPASSWORD = "SQLLiteDBPassWord";

        /// <summary>
        /// 日志Logger
        /// </summary>
        public const string LOGGER = "application-log";

        /// <summary>
        /// 聚合搜索历史选择的站点
        /// </summary>
        public const string CONFIG_SEARCHSITEHISTORY = "DAA6124E-D8AC-4800-98E7-F2E87C379D61";

        /// <summary>
        /// 签到时间
        /// </summary>
        public const string CONFIG_SIGN_TIME = "3FC62527-90BE-4EF2-8AA4-628B330FD50B";

        /// <summary>
        /// 自动签到
        /// </summary>
        public const string CONFIG_SIGN_AUTO = "EE8FAC27-6CC3-48B7-A55E-31C91824193D";

        /// <summary>
        /// 定时同步个人信息
        /// </summary>
        public const string CONFIG_SYNC_AUTO = "971D162A-74EC-4094-92DF-BA4EAB94B965";

        /// <summary>
        /// 定时搜索间隔
        /// </summary>
        public const string CONFIG_SEARCH_TIMESPAN = "FACE5546-DC33-4BF8-87F8-86A98BDE3113";

        /// <summary>
        /// 请求站点排序
        /// </summary>
        public const string CONFIG_SEARCH_POSTSITEORDER = "875AEF39-F14A-4A92-8E89-B0E21FDED3B1";

        /// <summary>
        /// 记住上次排序
        /// </summary>
        public const string CONFIG_SEARCH_ISLASTSORT = "DF247257-BDBF-45F4-BD20-5FA0AF29ADCF";

        /// <summary>
        /// 忽略置顶
        /// </summary>
        public const string CONFIG_SEARCH_INGORETOP = "9CB38C60-122A-4419-8932-60B1D3D2814F";

        /// <summary>
        /// 请求服务器下载文件名
        /// </summary>
        public const string CONFIG_ENABLEPOSTFILENAME = "77D20A93-0794-41EC-ADD4-5B56EDF30259";


        /// <summary>
        /// 关闭主面板时是否最小化到系统托盘
        /// </summary>
        public const string CONFIG_ISMINIWHENCLOSE = "000D4880-9A06-4D1F-9536-A0C11D2C9B1F";

        /// <summary>
        /// 首次使用程序
        /// </summary>
        public const string CONFIG_ISFIRSTOPEN = "C23135A8-A5E7-45C8-A61A-BEDBB0444C94";

        /// <summary>
        /// POST_BOUNDARY
        /// </summary>
        public const string POST_BOUNDARY = "WebKitFormBoundarySl3FiMIKHkGajqA8";

        /// <summary>
        /// 本地存储Cookie的路径
        /// </summary>
        public const string PATH_LOCALCOOKIE = "COOKIE";

        /// <summary>
        /// Chrome
        /// </summary>
        public const string HTTP_CHROME_UA = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.186 Safari/537.36";

        /// <summary>
        /// IE9
        /// </summary>
        public const string HTTP_IE9_UA = "Mozilla/5.0(compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident / 5.0)";

    }
}
