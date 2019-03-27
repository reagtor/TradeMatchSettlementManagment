using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using MatchCenter.BLL.Common;
using MatchCenter.BLL.ManagementCenter;
using MatchCenter.BLL.PushBack;
using GTA.VTS.Common.CommonUtility;

namespace MatchCenter.BLL.MatchRules
{
    /// <summary>
    /// Title:开关市操作类
    /// Desc.:开关市操作类
    /// Create By:李健华
    /// Create Date:2009-11-19
    /// Update By:董鹏
    /// Update Date:2009-12-16
    /// Desc.:修改关市操作时间区间判断方法
    /// Update By:董鹏
    /// Update Date:2010-02-25
    /// Desc.:增加连接管理中心失败后，重新连接的功能
    /// </summary>
    public class MarketOpenClose
    {
        /// <summary>
        /// 开启定时检查开市初始化操作
        /// </summary>
        public static void StartOpenTimerInit()
        {
            Timer OpenMarketTimer = new Timer();
            OpenMarketTimer.Interval = 3 * RulesDefaultValue.DefaultInternal;//五分钟检查一次
            //OpenMarketTimer.Interval =  RulesDefaultValue.DefaultInternal;
            OpenMarketTimer.Elapsed -= MarketOpen;
            OpenMarketTimer.Elapsed += MarketOpen;
            OpenMarketTimer.Enabled = true;

            Timer CloseMarketTimer = new Timer();
            CloseMarketTimer.Interval = 9 * RulesDefaultValue.DefaultInternal;//十五分钟检查一次
            //CloseMarketTimer.Interval = RulesDefaultValue.DefaultInternal;
            CloseMarketTimer.Elapsed -= MarketClose;
            CloseMarketTimer.Elapsed += MarketClose;
            CloseMarketTimer.Enabled = true;
        }

        /// <summary>
        /// 定时开市操作，也即重新获取管理中心数据初始化内部数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void MarketOpen(object sender, ElapsedEventArgs args)
        {
            if (AppConfig.IsAppStartInitialize == true || AppConfig.IsOpenMarket == true)
            {
                return;
            }

            if (TradeTimeManager.Instanse.IsOpenMarketTime(DateTime.Now))
            {
                if (AppConfig.IsOpenMarket == false)
                {
                    try
                    {
                        LogHelper.WriteInfo("CH-0110:正在执行每日开市操作" + DateTime.Now);

                        ShowMessage.Instanse.ShowFormTitleMessage("==系统正在初始化中，请稍候……");
                        #region old
                        //if (!GetAllCommonDataFromManagerCenter.IsConnManagerCenterSuccess())
                        //{
                        //    ShowMessage.Instanse.ShowFormTitleMessage("==无法连接管理中心，初始货失败……");
                        //    return;
                        //}
                        #endregion

                        #region 增加连接管理中心失败后，重新连接的功能 modify by 董鹏 2010-02-25
                        int retryTimes = 0;
                        while (!CommonDataCacheProxy.Instanse.TestConnManagerCenterSuccess())
                        {
                            System.Threading.Thread.Sleep(5000);
                            retryTimes++;
                            ShowMessage.Instanse.ShowFormTitleMessage("==连接管理中心失败，正在尝试第" + retryTimes + "次重新连接……");
                        }
                        #endregion

                        InitMatchCenter.Instanse.InitMatchStart();

                        TradePushBackImpl.Instanse.InitDealPushBackFromDataBase();

                        ShowMessage.Instanse.ShowFormTitleMessage("==初始化完成……");
                        ShowMessage.Instanse.ShowFormTitleMessage(AppConfig.MatchCenterName);

                        LogHelper.WriteInfo("CH-0110:执行每日开市操作==完成" + DateTime.Now);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteError("CH-0029:启动每日开市前初始化异常", ex);
                    }
                }
            }
        }

        /// <summary>
        /// 定时关市操作目前只要是更新是否开市状态标识
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void MarketClose(object sender, ElapsedEventArgs args)
        {
            if (AppConfig.IsAppStartInitialize == true || AppConfig.IsOpenMarket == false)
            {
                return;
            }
            //old
            //if (TradeTimeManager.Instanse.IsCloseMarketTime(DateTime.Now))            

            //new Create by:董鹏 2009-12-16
            //只要不在时间段之内都执行关市
            if (!TradeTimeManager.Instanse.IsMarketTime(DateTime.Now))
            {
                if (AppConfig.IsOpenMarket == true)
                {
                    AppConfig.IsOpenMarket = false;
                    LogHelper.WriteInfo("CH-0010:已经执行闭市操作完成" + DateTime.Now);
                }
            }
        }
    }
}
