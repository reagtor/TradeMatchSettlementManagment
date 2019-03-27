using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReckoningCounter.Entity.Model.HK
{
    /// <summary>
    /// 港股历史成交实体类HK_HistoryTradeInfo 。(属性说明自动提取数据库字段的描述信息)
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    //[Serializable]
    public class HK_HistoryTradeInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HK_HistoryTradeInfo()
        { }
        #region Model
        private string _tradenumber;
        private string _entrustnumber;
        private decimal _entrustprice;
        private decimal _tradeprice;
        private int _tradeamount;
        private decimal _stamptax;
        private decimal _commission;
        private string _code;
        private decimal _transferaccountfee;
        private decimal _tradeproceduresfee;
        private decimal _monitoringfee;
        private decimal _tradingsystemusefee;
        private decimal _clearingfee;
        private string _holdaccount;
        private string _capitalaccount;
        private int _tradetypeid;
        private int _tradeunitid;
        private int _buyselltypeid;
        private int _currencytypeid;
        private DateTime _tradetime;
        /// <summary>
        /// 港股成交单号主键
        /// </summary>
        public string TradeNumber
        {
            set { _tradenumber = value; }
            get { return _tradenumber; }
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
        /// 委托单号
        /// </summary>
        public string EntrustNumber
        {
            set { _entrustnumber = value; }
            get { return _entrustnumber; }
        }
        /// <summary>
        /// 委托价格
        /// </summary>
        public decimal EntrustPrice
        {
            set { _entrustprice = value; }
            get { return _entrustprice; }
        }
        /// <summary>
        /// 成交价格
        /// </summary>
        public decimal TradePrice
        {
            set { _tradeprice = value; }
            get { return _tradeprice; }
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
        /// 印花税
        /// </summary>
        public decimal StampTax
        {
            set { _stamptax = value; }
            get { return _stamptax; }
        }
        /// <summary>
        /// 佣金
        /// </summary>
        public decimal Commission
        {
            set { _commission = value; }
            get { return _commission; }
        }
        /// <summary>
        /// 成交商品编号(这与管理中心的CM_Commodity对应)
        /// </summary>
        public string Code
        {
            set { _code = value; }
            get { return _code; }
        }
        /// <summary>
        /// 过户费
        /// </summary>
        public decimal TransferAccountFee
        {
            set { _transferaccountfee = value; }
            get { return _transferaccountfee; }
        }
        /// <summary>
        /// 交易手续费
        /// </summary>
        public decimal TradeProceduresFee
        {
            set { _tradeproceduresfee = value; }
            get { return _tradeproceduresfee; }
        }
        /// <summary>
        /// 监管费
        /// </summary>
        public decimal MonitoringFee
        {
            set { _monitoringfee = value; }
            get { return _monitoringfee; }
        }
        /// <summary>
        /// 交易系统使用费
        /// </summary>
        public decimal TradingSystemUseFee
        {
            set { _tradingsystemusefee = value; }
            get { return _tradingsystemusefee; }
        }
        /// <summary>
        /// 结算费
        /// </summary>
        public decimal ClearingFee
        {
            set { _clearingfee = value; }
            get { return _clearingfee; }
        }
        /// <summary>
        /// 用于成交的持仓账户(即港股持仓帐户--即或证券股东代码/港股股东代码))(外键UA_UserAccountAllocationTable)
        /// </summary>
        public string HoldAccount
        {
            set { _holdaccount = value; }
            get { return _holdaccount; }
        }
        /// <summary>
        /// 港股成交资金帐户(即证券资金帐户/港股资金帐户)
        /// </summary>
        public string CapitalAccount
        {
            set { _capitalaccount = value; }
            get { return _capitalaccount; }
        }
        /// <summary>
        /// 成交类型ID(外键BD_TradeType)
        /// </summary>
        public int TradeTypeId
        {
            set { _tradetypeid = value; }
            get { return _tradetypeid; }
        }
        /// <summary>
        /// 委托单交易单位ID(外键BD_TradeUnit)
        /// </summary>
        public int TradeUnitId
        {
            set { _tradeunitid = value; }
            get { return _tradeunitid; }
        }
        /// <summary>
        /// 卖买类型(外键BD_BuySellType)
        /// </summary>
        public int BuySellTypeId
        {
            set { _buyselltypeid = value; }
            get { return _buyselltypeid; }
        }
        /// <summary>
        /// 委托单交易货币类型ID(外键BD_CurrencyType)
        /// </summary>
        public int CurrencyTypeId
        {
            set { _currencytypeid = value; }
            get { return _currencytypeid; }
        }
        /// <summary>
        /// 成交时间
        /// </summary>
        public DateTime TradeTime
        {
            set { _tradetime = value; }
            get { return _tradetime; }
        }
        #endregion Model

    }
}
