using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTA.VTS.Common.CommonObject;
using Types = ReckoningCounter.Entity.Contants.Types;

namespace ReckoningCounter.BLL.Common
{
    /// <summary>
    /// 委托单检索缓存元素
    /// 作者：朱亮
    /// 日期：2008-11-25
    /// </summary>
    public class OrderCacheItem
    {
        /// <summary>
        /// 交易员ID
        /// </summary>
        public string TraderId { get; set; }

        /// <summary>
        /// 交易员资金帐户
        /// </summary>
        public string CapitalAccount { get; set; }


        /// <summary>
        /// 交易员持仓帐户
        /// </summary>
        public string HoldingAccount { get; set; }

        /// <summary>
        /// 柜台委托单号
        /// </summary>
        public string CounterOrderNo { get; set; }

        /// <summary>
        /// 委托数量
        /// </summary>
        public int EntrustAmount { get; set; }

        //交易代码
        public string Code { get; set; }
        /// <summary>
        /// 委托状态
        /// </summary>
        //   public Types.OrderStateType OrderState{ get; set; }

        /// <summary>
        /// 买卖类型
        /// </summary>
        public GTA.VTS.Common.CommonObject.Types.TransactionDirection BuySellType { get; set; }


        /// <summary>
        /// 开平方向
        /// </summary>
        public Types.FutureOpenCloseType OpenCloseType { get; set; }

        #region 仅用于期货
        /// <summary>
        /// 是否是期货开盘前检查的委托单(包括资金，持仓限制检查)
        /// </summary>
        public bool IsOpenMarketCheckOrder { get; set; }

        /// <summary>
        /// 期货强行平仓类型
        /// </summary>
        public GTA.VTS.Common.CommonObject.Types.QHForcedCloseType QHForcedCloseType { get; set; }
        #endregion

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="strCapitalAccount"></param>
        /// <param name="strHoldingAccount"></param>
        /// <param name="strCounterOrderNo"></param>
        /// <param name="buySellType"></param>
        /// <param name="openClose"></param>
        public OrderCacheItem(string strCapitalAccount, string strHoldingAccount, string strCounterOrderNo,
            GTA.VTS.Common.CommonObject.Types.TransactionDirection buySellType, Types.FutureOpenCloseType openClose)
        {
            CapitalAccount = strCapitalAccount;
            HoldingAccount = strHoldingAccount;
            CounterOrderNo = strCounterOrderNo;
            BuySellType = buySellType;
            OpenCloseType = openClose;
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="strCapitalAccount"></param>
        /// <param name="strHoldingAccount"></param>
        /// <param name="strCounterOrderNo"></param>
        /// <param name="buySellType"></param>

        public OrderCacheItem(string strCapitalAccount, string strHoldingAccount, string strCounterOrderNo,
            GTA.VTS.Common.CommonObject.Types.TransactionDirection buySellType)
        {
            CapitalAccount = strCapitalAccount;
            HoldingAccount = strHoldingAccount;
            CounterOrderNo = strCounterOrderNo;
            BuySellType = buySellType;
        }
    }
}
