using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YU.PT;
using YU.Core;
using YU.Core.DataEntity;
using YU.Core.Log;
using YU.Core.Utils;

namespace YPT
{
    /// <summary>
    /// 全局静态类
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// 全局配置
        /// </summary>
        public static Config Config { get; set; }

        public static List<PTSite> Sites { get { return _sites; } }

        private static List<PTSite> _sites;

        public static List<PTUser> Users { get; set; }

        public static void Init()
        {
            //YU.WebUI.App.Start(4004);
            InitSites();
            Config = new Config();
            string dbName = ConfigUtil.GetConfigValue(YUConst.SETTING_SQLLITEDB);
            string dbPassWord = ConfigUtil.GetConfigValue(YUConst.SETTING_SQLLITEDBPASSWORD);
            if (dbName.IsNullOrEmptyOrWhiteSpace())
                throw new Exception("配置数据库名称不能为空。");
            else
            {
                AppService.InitConn(dbName, dbPassWord);
                if (!File.Exists(ConfigUtil.GetConfigValue(YUConst.SETTING_SQLLITEDB)))
                {
                    AppService.InitDB(dbName);
                    AppService.InitTable();
                    Users = new List<PTUser>();
                }
                else
                {
                    AppService.InitTable();
                    Users = AppService.GetAllUsers(Sites);
                }

                Config.SignTime = AppService.GetConfig(YUConst.CONFIG_SIGN_TIME, new DateTime(1970, 1, 1, 0, 0, 0));
                Config.IsAutoSign = AppService.GetConfig(YUConst.CONFIG_SIGN_AUTO, true);
                Config.IsEnablePostFileName = AppService.GetConfig(YUConst.CONFIG_ENABLEPOSTFILENAME, true);
                Config.IsSyncTiming = AppService.GetConfig(YUConst.CONFIG_SYNC_AUTO, true);
                Config.IsFirstOpen = AppService.GetConfig(YUConst.CONFIG_ISFIRSTOPEN, true);
                Config.IsMiniWhenClose = AppService.GetConfig(YUConst.CONFIG_ISMINIWHENCLOSE, false);
                Config.IsPostSiteOrder = AppService.GetConfig(YUConst.CONFIG_SEARCH_POSTSITEORDER, false);
                Config.IsIngoreTop = AppService.GetConfig(YUConst.CONFIG_SEARCH_INGORETOP, false);
                Config.IsLastSort = AppService.GetConfig(YUConst.CONFIG_SEARCH_ISLASTSORT, false);
                Config.SearchTimeSpan = AppService.GetConfig(YUConst.CONFIG_SEARCH_TIMESPAN, 300M);
            }
        }

        public static void InitSites()
        {
            _sites = ObjectUtils.CreateCopy<List<PTSite>>(PTSite.Sites.ToList());
            if (File.Exists(PTSiteConst.EXTEND_SITES))
            {
                string siteJson = File.ReadAllText(PTSiteConst.EXTEND_SITES);
                try
                {
                    var extendSites = JsonConvert.DeserializeObject<List<PTSite>>(siteJson);
                    if (extendSites != null && extendSites.Count > 0)
                    {
                        //如果扩展站点中是预置站点中的内容，则删除预置站点，以扩展站点为准。
                        _sites.RemoveAll(x => extendSites.Select(g => g.Id).Contains(x.Id));
                        _sites.RemoveAll(x => extendSites.Select(g => g.Name).Contains(x.Name));
                        foreach (var extendSite in extendSites)
                        {
                            _sites.Add(extendSite);
                        }
                    }
                }
                catch
                {
                    Logger.Info(string.Format("序列化扩展站点失败 ，请检查[{0}]文件。", PTSiteConst.EXTEND_SITES));
                }
            }
            else
            {
                if (!Directory.Exists(Path.GetDirectoryName(PTSiteConst.EXTEND_SITES)))
                    Directory.CreateDirectory(Path.GetDirectoryName(PTSiteConst.EXTEND_SITES));
                File.WriteAllText(PTSiteConst.EXTEND_SITES, PTSiteConst.EXTEND_SITES_COMMENT, Encoding.UTF8);
            }
            _sites = _sites.OrderBy(x => x.Order).ToList();
        }

     

    }
}
