using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using GTA.VTS.Common.CommonUtility;

namespace GTA.VTS.CustomersOrders.BLL
{
    /// <summary>    
    /// Tilte;配置文件帮助类
    /// Desc: 提供对配置文件的访问和操作
    /// Create BY：李健华        
    /// Create date:2009-12-21          
    /// 修改：叶振东
    /// 时间：2009-12-22
    /// 内容：添加密码保存和价格是否与上下限进行比较判断
    /// </summary>
    public class ServerConfig
    {
        private static readonly string TraderIDString = "TraderID";

        /// <summary>
        /// 交易员ID
        /// </summary>
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

        private static readonly string UriString = "Uri";
        /// <summary>
        /// 服务器连接地址
        /// </summary>
        public static string Uri
        {
            get
            {
                return ConfigurationManager.AppSettings[UriString];
            }
            set
            {
                ModifyConfig(UriString, value);
                Refresh();
            }
        }

        private static readonly string PassWordString = "PassWord";

        /// <summary>
        /// 交易员密码
        /// </summary>
        public static string PassWord
        {
            get
            {
                return ConfigurationManager.AppSettings[PassWordString];
            }
            set
            {
                ModifyConfig(PassWordString, value);
                Refresh();
            }
        }

        private static readonly string XHString = "XHCapitalAccount";

        /// <summary>
        /// 现货资金账户
        /// </summary>
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

        /// <summary>
        /// 港股资金账户
        /// </summary>
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

        /// <summary>
        /// 股指期货资金账户
        /// </summary>
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

        /// <summary>
        /// 商品期货资金账户
        /// </summary>
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

        /// <summary>
        /// 通道ID
        /// </summary>
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

        private static readonly string PriceString = "Price";

        /// <summary>
        /// 是否不对委托价格进行上下限进行判断（如果配置文件进行判断且为True不判断）
        /// </summary>
        public static bool Price
        {
            get
            {
                bool result = false;
                string key = PriceString;
                try
                {
                    string str = ConfigurationManager.AppSettings[key];
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (bool.TryParse(str, out result))
                        {
                            return result;
                        }
                        else
                        {
                            return false;
                        }
                    }

                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
                return result;
            }
        }

        #region 管理中心服务IP地址
        private static readonly string ManagementIPString = "ManagementIP";

        /// <summary>
        /// 管理中心服务IP地址
        /// </summary>
        public static string ManagementIP
        {
            get
            {
                return ConfigurationManager.AppSettings[ManagementIPString];
            }
            set
            {
                ModifyConfig(ManagementIPString, value);
                Refresh();
            }
        }
        #endregion 管理中心服务IP地址

        #region 管理中心服务
        private static readonly string TransactionManageServerNameString = "TransactionManageServerName";

        /// <summary>
        /// 管理中心服务名称
        /// </summary>
        public static string TransactionManageServerName
        {
            get
            {
                return ConfigurationManager.AppSettings[TransactionManageServerNameString];
            }
            set
            {
                ModifyConfig(TransactionManageServerNameString, value);
                Refresh();
            }
        }

        private static readonly string TransactionManageServerNamePortString = "TransactionManageClientPort";

        /// <summary>
        /// 管理中心服务端口
        /// </summary>
        public static string TransactionManageClientPort
        {
            get
            {
                return ConfigurationManager.AppSettings[TransactionManageServerNamePortString];
            }
            set
            {
                ModifyConfig(TransactionManageServerNamePortString, value);
                Refresh();
            }
        }
        #endregion 管理中心服务

        #region 下单服务
        private static readonly string DoOrderServerNameString = "DoOrderServerName";

        /// <summary>
        /// 下单服务名称
        /// </summary>
        public static string DoOrderServerName
        {
            get
            {
                return ConfigurationManager.AppSettings[DoOrderServerNameString];
            }
            set
            {
                ModifyConfig(DoOrderServerNameString, value);
                Refresh();
            }
        }

        private static readonly string DoOrderClientPortString = "DoOrderClientPort";

        /// <summary>
        /// 下单服务端口
        /// </summary>
        public static string DoOrderClientPort
        {
            get
            {
                return ConfigurationManager.AppSettings[DoOrderClientPortString];
            }
            set
            {
                ModifyConfig(DoOrderClientPortString, value);
                Refresh();
            }
        }
        #endregion 下单服务

        #region 公共查询服务
        private static readonly string TraderQueryServerNameString = "TraderQueryServerName";

        /// <summary>
        /// 公共查询服务名称
        /// </summary>
        public static string TraderQueryServerName
        {
            get
            {
                return ConfigurationManager.AppSettings[TraderQueryServerNameString];
            }
            set
            {
                ModifyConfig(TraderQueryServerNameString, value);
                Refresh();
            }
        }
        private static readonly string TraderQueryClientPortString = "TraderQueryClientPort";

        /// <summary>
        /// 公共查询服务端口
        /// </summary>
        public static string TraderQueryClientPort
        {
            get
            {
                return ConfigurationManager.AppSettings[TraderQueryClientPortString];
            }
            set
            {
                ModifyConfig(TraderQueryClientPortString, value);
                Refresh();
            }
        }
        #endregion

        #region 港股查询服务端口
        private static readonly string HKTraderQueryServerNameString = "HKTraderQueryServerName";

        /// <summary>
        /// 港股查询服务名称
        /// </summary>
        public static string HKTraderQueryServerName
        {
            get
            {
                return ConfigurationManager.AppSettings[HKTraderQueryServerNameString];
            }
            set
            {
                ModifyConfig(HKTraderQueryServerNameString, value);
                Refresh();
            }
        }
        private static readonly string HKTraderQueryClientPortString = "HKTraderQueryClientPort";

        /// <summary>
        /// 港股查询服务端口
        /// </summary>
        public static string HKTraderQueryClientPort
        {
            get
            {
                return ConfigurationManager.AppSettings[HKTraderQueryClientPortString];
            }
            set
            {
                ModifyConfig(HKTraderQueryClientPortString, value);
                Refresh();
            }
        }
        #endregion 港股查询服务端口

        #region 回报服务名称
        private static readonly string OrderDealRptServerNameString = "OrderDealRptServerName";

        /// <summary>
        /// 回报服务名称
        /// </summary>
        public static string OrderDealRptServerName
        {
            get
            {
                return ConfigurationManager.AppSettings[OrderDealRptServerNameString];
            }
            set
            {
                ModifyConfig(OrderDealRptServerNameString, value);
                Refresh();
            }
        }
        private static readonly string OrderDealRptClientPortString = "OrderDealRptClientPort";

        /// <summary>
        /// 回报服务端口
        /// </summary>
        public static string OrderDealRptClientPort
        {
            get
            {
                return ConfigurationManager.AppSettings[OrderDealRptClientPortString];
            }
            set
            {
                ModifyConfig(OrderDealRptClientPortString, value);
                Refresh();
            }
        }
        #endregion 回报服务名称

        #region 账户资金管理服务
        private static readonly string AccountAndCapitalManagementServerNameString = "AccountAndCapitalManagementServerName";

        /// <summary>
        /// 账户资金管理服务名称
        /// </summary>
        public static string AccountAndCapitalManagementServerName
        {
            get
            {
                return ConfigurationManager.AppSettings[AccountAndCapitalManagementServerNameString];
            }
            set
            {
                ModifyConfig(AccountAndCapitalManagementServerNameString, value);
                Refresh();
            }
        }
        private static readonly string AccountAndCapitalManagementClientPortString = "AccountAndCapitalManagementClientPort";

        /// <summary>
        /// 账户资金管理服务端口
        /// </summary>
        public static string AccountAndCapitalManagementClientPort
        {
            get
            {
                return ConfigurationManager.AppSettings[AccountAndCapitalManagementClientPortString];
            }
            set
            {
                ModifyConfig(AccountAndCapitalManagementClientPortString, value);
                Refresh();
            }
        }
        #endregion 账户资金管理服务

        #region 柜台地址
        /// <summary>
        /// 柜台地址
        /// </summary>
        private static readonly string ReckoningIPString = "ReckoningIP";

        public static string ReckoningIP
        {
            get
            {
                return ConfigurationManager.AppSettings[ReckoningIPString];
            }
            set
            {
                ModifyConfig(ReckoningIPString, value);
                Refresh();
            }
        }
        #endregion 柜台地址
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

        /// <summary>
        /// 刷新
        /// </summary>
        public static void Refresh()
        {
            ConfigurationManager.RefreshSection("appSettings");
            //ConfigurationManager.RefreshSection("connectionStrings");
        }

        ///// <summary>
        ///// 获取程序更新版的原文件数路径
        ///// </summary>
        //public static string APPFilePath
        //{
        //    get
        //    {
        //        return ConfigurationManager.AppSettings["APPFilePath"];
        //    }

        //}
        ///// <summary>
        ///// 程序启动的方式1-客户端，2-服务端--，不存在以客户端启动
        ///// </summary>
        //public static int AppMode
        //{
        //    get
        //    {
        //        int result = 1;
        //        try
        //        {
        //            string str = ConfigurationManager.AppSettings["AppMode"];
        //            if (!string.IsNullOrEmpty(str))
        //            {
        //                if (int.TryParse(str, out result))
        //                {
        //                    return result;
        //                }
        //                else
        //                {
        //                    return 1;
        //                }
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            LogHelper.WriteError(ex.Message, ex);
        //        }
        //        return result;
        //    }
        //}
        ///// <summary>
        ///// 更新程序开启的服务器端口
        ///// </summary>
        //public static int UpdatePort
        //{
        //     get
        //    {
        //        int result = 9999;
        //        try
        //        {
        //            string str =    ConfigurationManager.AppSettings["UpdatePort"];
        //            if (!string.IsNullOrEmpty(str))
        //            {
        //                if (int.TryParse(str, out result))
        //                {
        //                    return result;
        //                }
        //                else
        //                {
        //                    return 9999;
        //                }
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            LogHelper.WriteError(ex.Message, ex);
        //        }
        //        return result;
        //    }

        //}

        /// <summary>
        /// 查询结果每页记录数
        /// </summary>
        public static int QueryPageSize
        {
            get
            {
                int size;
                //int max;
                if (!int.TryParse(ConfigurationManager.AppSettings["QueryPageSize"], out size))
                {
                    size = 20;
                }
                //if (!int.TryParse(ConfigurationManager.AppSettings["QueryPageMaxSize"], out max))
                //{
                //    max = 100;
                //}
                if (size > 500)
                {
                    size = 500;
                }
                if (size < 1)
                {
                    size = 1;
                }
                return size;
            }
            set
            {
                ModifyConfig("QueryPageSize", value.ToString());
                Refresh();
            }
        }
    }
}