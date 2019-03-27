#region Using Namespace

using log4net;
using log4net.Config;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.Model;

#endregion

namespace ReckoningCounter.MemoryData.Util
{
    /// <summary>
    /// 内存表日志
    /// 作者：宋涛
    /// </summary>
    public class MemoryLog
    {
        /// <summary>
        /// 日志
        /// </summary>
        private static ILog capitalLog;

        /// <summary>
        /// 初始化
        /// </summary>
        static MemoryLog()
        {
            XmlConfigurator.Configure();
            if (capitalLog == null)
                capitalLog = LogManager.GetLogger("Capital");
        }

        /// <summary>
        /// 写现货资金增量调试信息
        /// </summary>
        public static void WriteXHCapitalInfo(XH_CapitalAccountTable_DeltaInfo deltaInfo)
        {
            string format = "现货capitalAccountLogo={0},可用资金增量={1},冻结资金增量={2},当日出入金增量={3},已实现盈亏增量={4}";

            string msg = string.Format(format, deltaInfo.CapitalAccountLogo, deltaInfo.AvailableCapitalDelta,
                                       deltaInfo.FreezeCapitalTotalDelta,
                                       deltaInfo.TodayOutInCapital, deltaInfo.HasDoneProfitLossTotalDelta);
            capitalLog.Debug(msg);
        }

        /// <summary>
        /// 写现货资金增量调试信息
        /// </summary>
        public static void WriteHKCapitalInfo(HK_CapitalAccount_DeltaInfo deltaInfo)
        {
            string format = "港股capitalAccountLogo={0},可用资金增量={1},冻结资金增量={2},当日出入金增量={3},已实现盈亏增量={4}";

            string msg = string.Format(format, deltaInfo.CapitalAccountLogo, deltaInfo.AvailableCapitalDelta,
                                       deltaInfo.FreezeCapitalTotalDelta,
                                       deltaInfo.TodayOutInCapital, deltaInfo.HasDoneProfitLossTotalDelta);
            capitalLog.Debug(msg);
        }

        /// <summary>
        /// 写期货资金增量调试信息
        /// </summary>
        public static void WriteQHCapitalInfo(QH_CapitalAccountTable_DeltaInfo deltaInfo)
        {
            string format =
                "期货capitalAccountLogoId={0},可用资金增量={1},冻结资金增量={2},总保证金增量={3},当日出入金增量={4},浮动平仓盈亏增量={5},盯市平仓盈亏增量={6}";

            string msg = string.Format(format, deltaInfo.CapitalAccountLogoId, deltaInfo.AvailableCapitalDelta,
                                       deltaInfo.FreezeCapitalTotalDelta, deltaInfo.MarginTotalDelta,
                                       deltaInfo.TodayOutInCapitalDelta, deltaInfo.CloseFloatProfitLossTotalDelta,
                                       deltaInfo.CloseMarketProfitLossTotalDelta);
            capitalLog.Debug(msg);
        }
    }
}