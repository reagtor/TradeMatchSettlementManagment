using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ReckoningCounter.BLL.Common
{
    /// <summary>
    /// 配置文件帮助类
    /// </summary>
    public class ServerConfig
    {
        private static readonly string TraderIDString = "TraderID";

        public static string TraderID
        {
            get
            {
                return ConfigurationManager.AppSettings[TraderIDString];
            }
            set
            {
                ModifyConfig(TraderIDString, value);
                Refresh();
            }
        }

        private static readonly string XHString = "XHCapitalAccount";

        public static string XHCapitalAccount
        {
            get
            {
                return ConfigurationManager.AppSettings[XHString];
            }
            set
            {
                ModifyConfig(XHString, value);
                Refresh();
            }
        }

        private static readonly string HKString = "HKCapitalAccount";

        public static string HKCapitalAccount
        {
            get
            {
                return ConfigurationManager.AppSettings[HKString];
            }
            set
            {
                ModifyConfig(HKString, value);
                Refresh();
            }
        }

        private static readonly string GZQHString = "GZQHCapitalAccount";

        public static string GZQHCapitalAccount
        {
            get
            {
                return ConfigurationManager.AppSettings[GZQHString];
            }
            set
            {
                ModifyConfig(GZQHString, value);
                Refresh();
            }
        }

        private static readonly string SPQHString = "SPQHCapitalAccount";

        public static string SPQHCapitalAccount
        {
            get
            {
                return ConfigurationManager.AppSettings[SPQHString];
            }
            set
            {
                ModifyConfig(SPQHString, value);
                Refresh();
            }
        }

        private static readonly string ChannelString = "ChannelID";

        public static string ChannelID
        {
            get
            {
                return ConfigurationManager.AppSettings[ChannelString];
            }
            set
            {
                ModifyConfig(ChannelString, value);
                Refresh();
            }
        }

        /// <summary>
        /// 对某个AppSetting的值进行修改
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        private static void ModifyConfig(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (config.AppSettings.Settings[key] == null)
            {
                config.AppSettings.Settings.Add(key, value);
                config.Save(ConfigurationSaveMode.Full);
            }
            else
            {
                config.AppSettings.Settings.Remove(key);
                config.AppSettings.Settings.Add(key, value);
                config.Save(ConfigurationSaveMode.Full);
            }
        }

        public static void Refresh()
        {
            ConfigurationManager.RefreshSection("appSettings");
            //ConfigurationManager.RefreshSection("connectionStrings");
        }
    }

}
