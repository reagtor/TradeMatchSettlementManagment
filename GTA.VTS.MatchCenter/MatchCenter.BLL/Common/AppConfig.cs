using System;
using System.Configuration;
using GTA.VTS.Common.CommonUtility;
using System.Collections.Generic;



namespace MatchCenter.BLL.Common
{
    /// <summary>
    /// 功能配置类
    /// Create BY：李健华
    /// Create Date：2009-08-18
    /// Desc: 增加模拟行情开关
    /// Update By: 董鹏
    /// Update Date: 2010-01-27
    /// </summary>
    public class AppConfig
    {
        #region 默认配置值
        /// <summary>
        /// 默认端口
        /// </summary>
        private readonly static int DefaultPort = 9958;
        /// <summary>
        /// 默认本机IP
        /// </summary>
        private readonly static string DefaultIP = "127.0.0.1";
        /// <summary>
        /// 默认时间
        /// </summary>
        private readonly static double DefaultHour = 0.5;
        /// <summary>
        /// 默认行情登录用户
        /// </summary>
        private readonly static string DefaultRealUserName = "rtuser";
        /// <summary>
        /// 默认行情登录密码
        /// </summary>
        private readonly static string DefaultRealPwd = "11";
        // private readonly static int DefaultManagePort = 8083;
        /// <summary>
        /// 默认开启的撮合类型
        /// </summary>
        private readonly static string DefaultMatchType = "1111";

        #endregion

        #region 从配置文件中获取相关配置

        #region 从配置文件中获取实时行情组件使用模式
        /// <summary>
        /// 从配置文件中获取实时行情组件使用模式
        /// 如果获取不到使用默认1-socket
        /// </summary>
        /// <returns>1.socket 2.fast</returns>
        public static int GetConfigRealTimeMode()
        {
            int realTimeMode = 1;
            //获取配置文件中的行情组件模式
            string realTime = ConfigurationManager.AppSettings["MatchrealTime"];
            if (string.IsNullOrEmpty(realTime) || !int.TryParse(realTime.Trim(), out realTimeMode))
            {
                realTimeMode = 1;
            }
            return realTimeMode;
        }
        #endregion

        #region 从配置文件中获取登录实时行情组件用户名
        /// <summary>
        /// 从配置文件中获取实时行情组件用户名
        /// </summary>
        /// <returns>实时行情组件用户名</returns>
        public static string GetConfigRealTimeUserName()
        {
            string userName = ConfigurationManager.AppSettings["ServerUserName"];
            if (string.IsNullOrEmpty(userName))
            {
                userName = DefaultRealUserName;
            }
            return userName;
        }
        #endregion

        #region 从配置文件中获取登录实时行情组件的密码
        /// <summary>
        /// 从配置文件中获取登录实时行情组件的密码
        /// </summary>
        /// <returns></returns>
        public static string GetConfigRealTimePassword()
        {
            string password = ConfigurationManager.AppSettings["ServerPassword"];
            if (string.IsNullOrEmpty(password))
            {
                password = DefaultRealPwd;
            }
            return password;
        }
        #endregion

        #region 从配置文件中获取撮合中心使用的端口
        /// <summary>
        /// 从配置文件中获取撮合中心使用的端口
        /// </summary>
        /// <returns></returns>
        public static int GetConfigMatchCenterPort()
        {
            int port = DefaultPort;
            string centerPort = ConfigurationSettings.AppSettings["CenterPort"];
            if (string.IsNullOrEmpty(centerPort) || !int.TryParse(centerPort, out port))
            {
                port = DefaultPort;
            }
            return port;
        }
        #endregion

        #region 从配置文件中获取撮合中心使用的IP地址
        /// <summary>
        /// 从配置文件中获取撮合中心使用的IP地址
        /// 如果没有配置则默认以本机(127.0.0.1)
        /// </summary>
        /// <returns></returns>
        public static string GetConfigMatchCenterIP()
        {
            string ip = DefaultIP;
            if (ConfigurationSettings.AppSettings["CenterIp"] != null)
            {
                ip = ConfigurationSettings.AppSettings["CenterIp"].Trim();
            }
            return ip;
        }
        #endregion

        #region 从配置文件中获取清除时间
        /// <summary>
        /// 从配置文件中获取清除时间
        /// </summary>
        /// <returns></returns>
        public static double GetConfigClearTime()
        {
            double hour = DefaultHour;
            string clearTime = ConfigurationManager.AppSettings["ClearTime"];
            if (string.IsNullOrEmpty(clearTime) || !double.TryParse(clearTime, out hour))
            {
                hour = DefaultHour;
            }
            return DefaultHour;
        }
        #endregion

        #region 从配置文件中获取管理中心中心IP地址，默认为本机
        ///// <summary>
        ///// 从配置文件中获取管理中心中心IP地址，默认为本机
        ///// </summary>
        ///// <returns></returns>
        //public static string GetConfigManageCenterIP()
        //{
        //    string ip = DefaultIP;
        //    if (ConfigurationSettings.AppSettings["ManageCenterIp"] != null)
        //    {
        //        ip = ConfigurationSettings.AppSettings["ManageCenterIp"];
        //    }
        //    return ip;
        //}
        #endregion

        #region 从配置文件中获取管理中心中心端口，默认为8083
        ///// <summary>
        ///// 从配置文件中获取管理中心中心端口，默认为8083
        ///// </summary>
        ///// <returns></returns>
        //public static int GetConfigManagePort()
        //{
        //    int port = DefaultManagePort;
        //    string portStr = ConfigurationManager.AppSettings["ManageCenterPort"];
        //    if (string.IsNullOrEmpty(portStr) || !int.TryParse(portStr.Trim(), out port))
        //    {
        //        port = DefaultManagePort;
        //    }
        //    return port;
        //}
        #endregion

        #region 从配置文件中获取开市时间
        ///// <summary>
        ///// 从配置文件中获取开市时间,这里返回可能为null要注意检验
        ///// </summary>
        ///// <returns></returns>
        //public static string GetConfigBeginTime()
        //{
        //    return ConfigurationManager.AppSettings["BeginTime"];
        //}
        #endregion

        #region 从配置文件中获取开启撮合商品类型
        /// <summary>
        /// 开启撮合商品类型1111个位表示-现货,十位-股指期货,百位-港股，1或者非零表示开启,0表示不开启
        /// ，不配置或者长度大于或者小于四位此项表示全部开启
        /// </summary>
        /// <returns></returns>
        public static string GetConfigMatchBreedClassType()
        {
            string type = DefaultMatchType;
            string classType = ConfigurationSettings.AppSettings["MatchBreedClassType"];
            if (string.IsNullOrEmpty(classType) || classType.Length > 4 || classType.Length < 4)
            {
                type = DefaultMatchType;
            }
            else
            {
                type = classType;
            }
            int k = 1;
            if (!int.TryParse(classType, out k))
            {
                type = DefaultMatchType;
            }
            else
            {
                type = classType;
            }

            return type;
        }
        #endregion

        #region 是否使用模拟行情 add by 董鹏 2010-01-28
        /// <summary>
        /// 是否使用模拟行情
        /// </summary>
        /// <returns></returns>
        public static bool GetConfigMarketSimulate()
        {
            bool bSimulate = false;
            string simulate = System.Configuration.ConfigurationManager.AppSettings["MarketSimulate"];
            if (!string.IsNullOrEmpty(simulate))
            {
                bool.TryParse(simulate, out bSimulate);
            }
            return bSimulate;
        }
        #endregion

        #endregion

        #region 程序管理相关
        /// <summary>
        /// 程序是否正在启动开始初始化程序（也即正在初始化动作）
        /// </summary>
        public static bool IsAppStartInitialize=false;
        /// <summary>
        /// 撮合中心名称
        /// </summary>
        public static string MatchCenterName = "【一号撮合中心】";
        /// <summary>
        /// 是否已经执行开市动作
        /// </summary>
        public static bool IsOpenMarket = false;
 
        #endregion


    }

}