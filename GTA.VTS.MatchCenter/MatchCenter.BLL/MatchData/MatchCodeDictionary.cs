using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RealTime.Server.SModelData.HqData;
using Amib.Threading;
using CommonRealtimeMarket;
using GTA.VTS.Common.CommonUtility;
using MatchCenter.BLL.RealTime;
using System.Threading;

namespace MatchCenter.BLL.MatchData
{
    /// <summary>
    /// Title:撮合代码事件行情驱动撮合事件缓存类
    /// Create by:李健华
    /// Create Date:2009-09-14
    /// </summary>
    public class MatchCodeDictionary
    {
        /// <summary>
        /// 股指期货撮合代码行情驱动撮合事件缓存列表
        /// </summary>
        public static Dictionary<string, EventHandler<FutDataChangeEventArg>> funtureMatchCodeDic = new Dictionary<string, EventHandler<FutDataChangeEventArg>>();
        /// <summary>
        /// 商品期货撮合代码行情驱动撮合事件缓存列表
        /// </summary>
        public static Dictionary<string, EventHandler<MercantileFutDataChangeEventArg>> spQHMatchCodeDic = new Dictionary<string, EventHandler<MercantileFutDataChangeEventArg>>();
        
        /// <summary>
        ///  现货撮合代码行情驱动撮合事件缓存列表
        /// </summary>
        public static Dictionary<string, EventHandler<StockHqDataChangeEventArg>> stockMatchCodeDic = new Dictionary<string, EventHandler<StockHqDataChangeEventArg>>();

        /// <summary>
        ///  港股撮合代码行情驱动撮合事件缓存列表
        /// </summary>
        public static Dictionary<string, EventHandler<HKStockDataChangeEventArg>> hkStockMatchCodeDic = new Dictionary<string, EventHandler<HKStockDataChangeEventArg>>();

        #region 在程序活动的代码列表，这里是为了把所推过来的行情再进一步过滤而设置的,当每一次下单时都对些列表判断添加
        /// <summary>
        /// 读写锁
        /// 用于管理资源访问的锁定状态，可实现多线程读取或进行独占式写入访问。
        /// </summary>
        private static ReaderWriterLockSlim rwLockHK = new ReaderWriterLockSlim();
        /// <summary>
        /// 读写锁
        /// 用于管理资源访问的锁定状态，可实现多线程读取或进行独占式写入访问。
        /// </summary>
        private static ReaderWriterLockSlim rwLockXH = new ReaderWriterLockSlim();
        /// <summary>
        /// 读写锁
        /// 用于管理资源访问的锁定状态，可实现多线程读取或进行独占式写入访问。
        /// </summary>
        private static ReaderWriterLockSlim rwLockQH = new ReaderWriterLockSlim();
        /// <summary>
        /// 读写锁
        /// 用于管理资源访问的锁定状态，可实现多线程读取或进行独占式写入访问。
        /// </summary>
        private static ReaderWriterLockSlim rwLockSPQH = new ReaderWriterLockSlim();
        /// <summary>
        /// 现货已下单也即在程序中有活动的代码列表
        /// </summary>
        public static Dictionary<string, string> xh_ActivityOrderDic = new Dictionary<string, string>();
        /// <summary>
        /// 股指期货已下单也即在程序中有活动的代码列表
        /// </summary>
        public static Dictionary<string, string> qh_ActivityOrderDic = new Dictionary<string, string>();
        /// <summary>
        /// 商品期货已下单也即在程序中有活动的代码列表
        /// </summary>
        public static Dictionary<string, string> spqh_ActivityOrderDic = new Dictionary<string, string>();
        /// <summary>
        /// 港股已下单也即在程序中有活动的代码列表
        /// </summary>
        public static Dictionary<string, string> hk_ActivityOrderDic = new Dictionary<string, string>();
        
        /// <summary>
        /// 添加港股代码下单列表代码
        /// </summary>
        /// <param name="code"></param>
        public static void AddHK_ActivityOrderDic(string code)
        {
            rwLockHK.EnterWriteLock();
            try
            {
                if (!hk_ActivityOrderDic.ContainsKey(code))
                {
                    hk_ActivityOrderDic.Add(code, code);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("添加港股代码下单列表表过滤行情异常" + code, ex);
            }
            finally
            {
                rwLockHK.ExitWriteLock();
            }

        }
        /// <summary>
        /// 添加现货代码下单列表代码
        /// </summary>
        /// <param name="code"></param>
        public static void AddXH_ActivityOrderDic(string code)
        {
            rwLockXH.EnterWriteLock();
            try
            {
                if (!xh_ActivityOrderDic.ContainsKey(code))
                {
                    xh_ActivityOrderDic.Add(code, code);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("添加现货代码下单列表表过滤行情异常" + code, ex);
            }
            finally
            {
                rwLockXH.ExitWriteLock();
            }

        }
        /// <summary>
        /// 添加脂指期货代码下单列表代码
        /// </summary>
        /// <param name="code"></param>
        public static void AddQH_ActivityOrderDic(string code)
        {
            rwLockQH.EnterWriteLock();
            try
            {
                if (!qh_ActivityOrderDic.ContainsKey(code))
                {
                    qh_ActivityOrderDic.Add(code, code);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("添加股指期货代码下单列表表过滤行情异常" + code, ex);
            }
            finally
            {
                rwLockQH.ExitWriteLock();
            }

        }
        /// <summary>
        /// 添加商品期货代码下单列表代码
        /// </summary>
        /// <param name="code"></param>
        public static void AddSPQH_ActivityOrderDic(string code)
        {
            rwLockSPQH.EnterWriteLock();
            try
            {
                if (!spqh_ActivityOrderDic.ContainsKey(code))
                {
                    spqh_ActivityOrderDic.Add(code, code);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("添加商品期货代码下单列表表过滤行情异常" + code, ex);
            }
            finally
            {
                rwLockSPQH.ExitWriteLock();
            }

        }
        #endregion
    }
}
