using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ReckoningCounter.Entity.Model.HK
{
    /// <summary>
    /// 港股历史委托单实体类HK_HistoryEntrustInfo 。(属性说明自动提取数据库字段的描述信息)
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    //[Serializable]
    public class HK_HistoryEntrustInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HK_HistoryEntrustInfo()
        { }
        #region Model
        private string _entrustnumber;
        private decimal _entrustprice;
        private int _entrustmount;
        private string _hkcode;
        private int _tradeamount;
        private decimal _tradeaverageprice;
        private int _orderPriceType;
        private int _cancelamount;
        private int _buyselltypeid;
        private string _hktradeaccount;
        private string _hkcapitalaccount;
        private int _currencytypeid;
        private int _tradeunitid;
        private int _orderstatusid;
        private string _ordermessage;
        private string _mcorderid;
        private decimal _hasdoneprofit;
        private DateTime? _offertime;
        private DateTime _entrusttime;
        private bool _ismodifyorder;
        private string _modifyordernumber;
        /// <summary>
        /// 港股委托单号(主键)
        /// </summary>
        public string EntrustNumber
        {
            set { _entrustnumber = value; }
            get { return _entrustnumber; }
        }
        #region PortfolioLogo 投组标识
        private string portfolioLogo;
        /// <summary>
        /// 投组标识
        /// </summary>
        public string PortfolioLogo
        {
            get
            {
                return portfolioLogo;
            }
            set
            {
                portfolioLogo = value;
            }
        }
        #endregion
        /// <summary>
        /// 委托价格
        /// </summary>
        public decimal EntrustPrice
        {
            set { _entrustprice = value; }
            get { return _entrustprice; }
        }
        /// <summary>
        /// 委托数量
        /// </summary>
        public int EntrustMount
        {
            set { _entrustmount = value; }
            get { return _entrustmount; }
        }
        /// <summary>
        /// 委托港股商品ID(编码)
        /// </summary>
        public string Code
        {
            set { _hkcode = value; }
            get { return _hkcode; }
        }
        /// <summary>
        /// 成交总量
        /// </summary>
        public int TradeAmount
        {
            set { _tradeamount = value; }
            get { return _tradeamount; }
        }
        /// <summary>
        /// 委托成交平均价格
        /// </summary>
        public decimal TradeAveragePrice
        {
            set { _tradeaverageprice = value; }
            get { return _tradeaverageprice; }
        }
        /// <summary>
        /// 委托报价类型( 0,限价盘 1,增强限价盘 , 2特别限价盘)
        /// </summary>
        public int OrderPriceType
        {
            set { _orderPriceType = value; }
            get { return _orderPriceType; }
        }
        /// <summary>
        /// 撤单总量
        /// </summary>
        public int CancelAmount
        {
            set { _cancelamount = value; }
            get { return _cancelamount; }
        }
        /// <summary>
        /// 卖买类型(外键BD_BuySellType)
        /// </summary>
        public int BuySellTypeID
        {
            set { _buyselltypeid = value; }
            get { return _buyselltypeid; }
        }
        /// <summary>
        /// 用于委托的持仓账户(即港股持仓帐户--即或证券股东代码/港股股东代码))(外键UA_UserAccountAllocationTable)
        /// </summary>
        public string HoldAccount
        {
            set { _hktradeaccount = value; }
            get { return _hktradeaccount; }
        }
        /// <summary>
        /// 委托港股资金帐户(即证券资金帐户/港股资金帐户)(外键UA_UserAccountAllocationTable)
        /// </summary>
        public string CapitalAccount
        {
            set { _hkcapitalaccount = value; }
            get { return _hkcapitalaccount; }
        }
        /// <summary>
        /// 委托单交易货币类型ID(外键BD_CurrencyType)
        /// </summary>
        public int CurrencyTypeID
        {
            set { _currencytypeid = value; }
            get { return _currencytypeid; }
        }
        /// <summary>
        /// 委托单交易单位ID(外键BD_TradeUnit)
        /// </summary>
        public int TradeUnitID
        {
            set { _tradeunitid = value; }
            get { return _tradeunitid; }
        }
        /// <summary>
        /// 委托单状态(外键DB_OrderStatus)
        /// </summary>
        public int OrderStatusID
        {
            set { _orderstatusid = value; }
            get { return _orderstatusid; }
        }
        /// <summary>
        /// 委托单信息
        /// </summary>
        public string OrderMessage
        {
            set { _ordermessage = value; }
            get { return _ordermessage; }
        }
        /// <summary>
        /// 委托单的MC机器随机编码单号
        /// </summary>
        public string McOrderID
        {
            set { _mcorderid = value; }
            get { return _mcorderid; }
        }
        /// <summary>
        /// 已实现盈亏
        /// </summary>
        public decimal HasDoneProfit
        {
            set { _hasdoneprofit = value; }
            get { return _hasdoneprofit; }
        }
        /// <summary>
        /// 委托报盘时间
        /// </summary>
        public DateTime? OfferTime
        {
            set { _offertime = value; }
            get { return _offertime; }
        }
        /// <summary>
        /// 港股委托时间
        /// </summary>
        public DateTime EntrustTime
        {
            set { _entrusttime = value; }
            get { return _entrusttime; }
        }
        /// <summary>
        /// 是否是改单委托(默认0不是，1是)
        /// </summary>
        public bool IsModifyOrder
        {
            set { _ismodifyorder = value; }
            get { return _ismodifyorder; }
        }
        /// <summary>
        /// 改单的原委托编号
        /// </summary>
        public string ModifyOrderNumber
        {
            set { _modifyordernumber = value; }
            get { return _modifyordernumber; }
        }
        #endregion Model

    }
}
