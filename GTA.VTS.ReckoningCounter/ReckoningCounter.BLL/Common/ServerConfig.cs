using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using GTA.VTS.Common.CommonUtility;

namespace ReckoningCounter.BLL.Common
{
    /// <summary>
    /// 配置文件帮助累
    /// </summary>
    public class ServerConfig
    {
        private static readonly string CounterIDString = "CounterID";

        public static string CounterID
        {
            get
            {
                return ConfigurationManager.AppSettings[CounterIDString];
            }
            set
            {
                ModifyConfig(CounterIDString, value);
                Refresh();
            }
        }

        /// <summary>
        /// 初始化是是否加载所有持仓数据到内存中，默认为加载
        /// </summary>
        public static bool IsLoadAllData
        {
            get
            {
                bool result = true;
                string key = "loadAllData";
                try
                {
                    string str = ConfigurationManager.AppSettings[key];
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (str.Trim() == "2")
                            result = false;
                    }

                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }

                return result;
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
            ConfigurationManager.RefreshSection("connectionStrings");
        }
    }

}
