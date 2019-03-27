using System;
using System.Runtime.Serialization;
namespace ReckoningCounter.Model
{
    /// <summary>
    /// Title: 分红登记实体
    /// Desc: 分红登记实体XH_MelonCutRegisterTable 。(属性说明自动提取数据库字段的描述信息)
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    [DataContract]
    public class XH_MelonCutRegisterTableInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public XH_MelonCutRegisterTableInfo()
        { }
        #region Model
        private int _meloncutregisterid;
        private DateTime _registerdate;
        private DateTime _cutdate;
        private string _useraccountdistributelogo;
        private int _tradecurrencytype;
        private string _code;
        private decimal _registeramount;
        private int _cuttype;
        private int _currencytypeid;
        /// <summary>
        /// 分红登记表
        /// </summary>
        [DataMember]
        public int MelonCutRegisterID
        {
            set { _meloncutregisterid = value; }
            get { return _meloncutregisterid; }
        }
        /// <summary>
        /// 股权登记日(这来自于管理中心的StockMelonCash-->StockRightRegisterDate)
        /// </summary>
        [DataMember]
        public DateTime RegisterDate
        {
            set { _registerdate = value; }
            get { return _registerdate; }
        }
        /// <summary>
        /// 除权基准日(这来自于管理中心的StockMelonCash-->StockRightLogoutDatumDate)
        /// </summary>
        [DataMember]
        public DateTime CutDate
        {
            set { _cutdate = value; }
            get { return _cutdate; }
        }
        /// <summary>
        /// 用户持仓账号(外键XH_AccountHoldTable->UserAccountDistributeLogo)
        /// </summary>
        [DataMember]
        public string UserAccountDistributeLogo
        {
            set { _useraccountdistributelogo = value; }
            get { return _useraccountdistributelogo; }
        }
        /// <summary>
        /// 交易货币类型(来自于XH_AccountHoldTable->CurrencyTypeId)
        /// </summary>
        [DataMember]
        public int TradeCurrencyType
        {
            set { _tradecurrencytype = value; }
            get { return _tradecurrencytype; }
        }
        /// <summary>
        /// 股币商品编号(这来自于管理中心的StockMelonCash-->StockCode)
        /// </summary>
        [DataMember]
        public string Code
        {
            set { _code = value; }
            get { return _code; }
        }
        /// <summary>
        /// 登记分红总量(来自于当前持仓表中可用总量XH_AccountHoldTable->AvailableAmount)
        /// </summary>
        [DataMember]
        public decimal RegisterAmount
        {
            set { _registeramount = value; }
            get { return _registeramount; }
        }
        /// <summary>
        /// 分红类型(1-现金分红,2-股票分红)
        /// </summary>
        [DataMember]
        public int CutType
        {
            set { _cuttype = value; }
            get { return _cuttype; }
        }
        /// <summary>
        /// 当前分红货币类型(外键BD_CurrencyType)
        /// </summary>
        [DataMember]
        public int CurrencyTypeId
        {
            set { _currencytypeid = value; }
            get { return _currencytypeid; }
        }
        #endregion Model

    }
}

