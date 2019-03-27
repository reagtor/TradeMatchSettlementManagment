#region Using Namespace

using System;
using ReckoningCounter.Entity.Contants;
using ReckoningCounter.Model;
using ReckoningCounter.Entity.Model.HK;

#endregion

namespace ReckoningCounter.Entity.DelegateOffer
{
    /// <summary>
    /// Title:现货委托实体扩展
    /// Desc.:现货委托实体扩展
    /// Create by:宋涛
    /// Create date:2008-11-2
    /// </summary>
    public class XhTodayEntrustTableEx
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="original"></param>
        public XhTodayEntrustTableEx(XH_TodayEntrustTableInfo original)
        {
            OriginalEntity = original;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public XhTodayEntrustTableEx(StockOrderRequest request)
        {
            OriginalRequest = request;
        }

        /// <summary>
        /// 数据库存储的委托实体
        /// </summary>
        public XH_TodayEntrustTableInfo OriginalEntity { get; private set; }

        /// <summary>
        /// 原始委托
        /// </summary>
        public StockOrderRequest OriginalRequest { get; private set; }

        /// <summary>
        /// 交易员ID
        /// </summary>
        public string TraderId { get; set; }

        /// <summary>
        /// 是否是未开市缓存的单
        /// </summary>
        public bool IsCacheOrder { get; set; }

        /// <summary>
        /// 如果是未开市缓存的单，那么此属性代表是否已经唤醒发送过
        /// </summary>
        public bool HasSendCacheOrder { get; set; }

        /// <summary>
        /// 委托类型(限、市价)
        /// </summary>
        public Types.OrderPriceType PriceType
        {
            get
            {
                return OriginalEntity.IsMarketValue == true
                           ? Types.OrderPriceType.OPTMarketPrice
                           :
                               Types.OrderPriceType.OPTLimited;
            }
        }

        /// <summary>
        /// 转换显示的文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string desc = "";
            if (OriginalEntity != null)
            {

                string format =
                    "现货委托实体扩展XhTodayEntrustTableEx[股东代码={0},资金帐户={1},现货代码={2},买卖方向={3},委托价格={4} ,委托数量={5},委托时间={6},报盘时间={7},成交数量={8},成交均价={9},撤单数量={10},委托状态={11},EntrustNumber={12}]";

                int buySellTypeId = OriginalEntity.BuySellTypeId;
                decimal entrustPrice = OriginalEntity.EntrustPrice;
                int entrustAmount = OriginalEntity.EntrustAmount;
                int tradeAmount = OriginalEntity.TradeAmount;
                decimal tradeAveragePrice = OriginalEntity.TradeAveragePrice;
                int cancelAmount = OriginalEntity.CancelAmount;
                int orderStatusId = OriginalEntity.OrderStatusId;

                string offerTime = OriginalEntity.OfferTime.HasValue ? OriginalEntity.OfferTime.ToString() : "";
                string entrustTime = OriginalEntity.EntrustTime.ToString();

                string entrustNumber = OriginalEntity.EntrustNumber;
                desc = String.Format(format, OriginalEntity.StockAccount, OriginalEntity.CapitalAccount,
                                            OriginalEntity.SpotCode, buySellTypeId
                                            , entrustPrice, entrustAmount,
                                            entrustTime, offerTime
                                            , tradeAmount,
                                            tradeAveragePrice, cancelAmount,
                                            orderStatusId, entrustNumber);

            }
            else
            {
                desc = OriginalRequest.ToString();
            }
            return desc;
        }
    }

    /// <summary>
    /// Title:港股委托实体扩展
    /// Desc.:港股委托实体扩展
    /// Create by:宋涛
    /// Create date:2009-11-2
    /// </summary>
    public class HkTodayEntrustEx
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="original"></param>
        public HkTodayEntrustEx(HK_TodayEntrustInfo original)
        {
            OriginalEntity = original;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public HkTodayEntrustEx(HKOrderRequest request)
        {
            OriginalRequest = request;
        }

        /// <summary>
        /// 数据库存储的委托实体
        /// </summary>
        public HK_TodayEntrustInfo OriginalEntity { get; private set; }

        /// <summary>
        /// 原始委托实体
        /// </summary>
        public HKOrderRequest OriginalRequest { get; private set; }

        /// <summary>
        /// 交易员ID
        /// </summary>
        public string TraderId { get; set; }

        /// <summary>
        /// 是否是未开市缓存的单
        /// </summary>
        public bool IsCacheOrder { get; set; }

        /// <summary>
        /// 如果是未开市缓存的单，那么此属性代表是否已经唤醒发送过
        /// </summary>
        public bool HasSendCacheOrder { get; set; }

        ///// <summary>
        ///// 委托类型价格类型
        ///// </summary>
        //public CommonObject.Types.HKPriceType PriceType { get; set; }

        /// <summary>
        /// 转换显示的文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string desc = "";
            if (OriginalEntity != null)
            {

                string format =
                    "港股委托实体扩展HkTodayEntrustEx[股东代码={0},资金帐户={1},现货代码={2},买卖方向={3},委托价格={4} ,委托数量={5},委托时间={6},报盘时间={7},成交数量={8},成交均价={9},撤单数量={10},委托状态={11},EntrustNumber={12}]";

                int buySellTypeId = OriginalEntity.BuySellTypeID;
                decimal entrustPrice = OriginalEntity.EntrustPrice;
                int entrustAmount = OriginalEntity.EntrustAmount;
                int tradeAmount = OriginalEntity.TradeAmount;
                decimal tradeAveragePrice = OriginalEntity.TradeAveragePrice;
                int cancelAmount = OriginalEntity.CancelAmount;
                int orderStatusId = OriginalEntity.OrderStatusID;

                string offerTime = OriginalEntity.OfferTime.HasValue ? OriginalEntity.OfferTime.ToString() : "";
                string entrustTime = OriginalEntity.EntrustTime.ToString();

                string entrustNumber = OriginalEntity.EntrustNumber;
                desc = String.Format(format, OriginalEntity.HoldAccount, OriginalEntity.CapitalAccount,
                                            OriginalEntity.Code, buySellTypeId
                                            , entrustPrice, entrustAmount, entrustTime, offerTime
                                            , tradeAmount, tradeAveragePrice, cancelAmount,
                                            orderStatusId, entrustNumber);

            }
            else
            {
                desc = OriginalRequest.ToString();
            }
            return desc;
        }
    }

    /// <summary>
    /// Title:期货委托实体扩展
    /// Desc.:期货委托实体扩展
    /// Create by:宋涛
    /// Create date:2008-11-2
    /// </summary>
    public class QhTodayEntrustTableEx
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="original"></param>
        public QhTodayEntrustTableEx(QH_TodayEntrustTableInfo original)
        {
            OriginalEntity = original;
        }

        /// <summary>
        /// 数据库存储的委托实体
        /// </summary>
        public QH_TodayEntrustTableInfo OriginalEntity { get; private set; }

        /// <summary>
        /// 是否是未开市缓存的单
        /// </summary>
        public bool IsCacheOrder { get; set; }

        /// <summary>
        /// 如果是未开市缓存的单，那么此属性代表是否已经唤醒发送过
        /// </summary>
        public bool HasSendCacheOrder { get; set; }

        /// <summary>
        /// 是否是期货开盘前检查的委托单(包括资金，持仓限制检查)
        /// </summary>
        public bool IsOpenMarketCheckOrder { get; set; }
        /// <summary>
        /// 期货强行平仓类型(当是盘前检查时才有效)
        /// </summary>
        public GTA.VTS.Common.CommonObject.Types.QHForcedCloseType QHForcedCloseType { get; set; }
        /// <summary>
        /// 交易员ID
        /// </summary>
        public string TraderId { get; set; }

        /// <summary>
        /// 委托类型(限、市价)
        /// </summary>
        public Types.OrderPriceType PriceType
        {
            get
            {
                return OriginalEntity.IsMarketValue == true
                           ? Types.OrderPriceType.OPTMarketPrice
                           :
                               Types.OrderPriceType.OPTLimited;
            }
        }
    }
}