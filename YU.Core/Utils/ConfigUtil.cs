using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using YU.Core.DataEntity;

namespace YU.Core.Utils
{
    /// <summary>
    /// Config工具类
    /// </summary>
    public static class ConfigUtil
    {
        /// <summary>
        /// 获取Config
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static Configuration GetConfig(string configName)
        {
            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            map.ExeConfigFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configName + ".config");
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            return config;
        }

        /// <summary>
        /// 删除Config中某个Key
        /// </summary>
        /// <param name="key"></param>

        public static void RemoveConfigValue(string key)
        {
            //增加的内容写在appSettings段下 <add key="RegCode" value="0"/>  
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] == null)
            {
                return;
            }
            else
            {
                config.AppSettings.Settings.Remove(key);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");//重新加载新的配置文件   
            }
        }

        /// <summary>  
        /// 写入值  
        /// </summary>  
        /// <param name="key"></param>  
        /// <param name="value"></param>  
        public static void SetConfigValue(string key, string value)
        {
            //增加的内容写在appSettings段下 <add key="RegCode" value="0"/>  
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] == null)
            {
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                config.AppSettings.Settings[key].Value = value;
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");//重新加载新的配置文件   
        }

        /// <summary>  
        /// 读取指定key的值  
        /// </summary>  
        /// <param name="key"></param>  
        /// <returns></returns>  
        public static string GetConfigValue(string key)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] == null)
                return "";
            else
                return config.AppSettings.Settings[key].Value;
        }

        ///// <summary>
        ///// 初始化Config
        ///// </summary>
        //public static Config InitConfig()
        //{
        //    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //    Config locConfig = new Config();
        //    if (config.AppSettings.Settings.Count > 0)
        //    {
        //        var properInfos = locConfig.GetType().GetProperties();
        //        if (properInfos != null && properInfos.Count() > 0)
        //        {
        //            foreach (KeyValueConfigurationElement setting in config.AppSettings.Settings)
        //            {
        //                //这里通过反射来初始化
        //                foreach (var properInfo in properInfos)
        //                {
        //                    if (setting.Key.EqualIgnoreCase(properInfo.Name))
        //                    {
        //                        try
        //                        {
        //                            properInfo.SetValue(locConfig, Convert.ChangeType(setting.Value, properInfo.PropertyType), null);
        //                        }
        //                        catch
        //                        {
        //                            //如果出现了异常，表明配置文件中配置项异常，这里将配置项重置为默认
        //                            //有时间可以做日志处理
        //                            SetConfigValue(setting.Key, properInfo.GetValue(locConfig, null).TryPareValue<string>());
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return locConfig;
        //}
    }
}
