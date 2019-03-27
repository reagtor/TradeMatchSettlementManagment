using System;
using System.Runtime.Serialization;
namespace ReckoningCounter.Model
{
    /// <summary>
    /// Title: 资金转账实体
    /// Desc: 资金转账实体实体类UA_CapitalFlowTable 。(属性说明自动提取数据库字段的描述信息)
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    [DataContract]
    public class UA_CapitalFlowTableInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UA_CapitalFlowTableInfo()
        { }
        #region Model
        private int _capitalflowlogo;
        private string _fromcapitalaccount;
        private string _tocapitalaccount;
        private decimal _transferamount;
        private DateTime _transfertime;
        private int _tradecurrencytype;
        private int _transfertypelogo;
        /// <summary>
        /// 转账明细主键ID
        /// </summary>
        [DataMember]
        public int CapitalFlowLogo
        {
            set { _capitalflowlogo = value; }
            get { return _capitalflowlogo; }
        }
        /// <summary>
        /// 转出资金账号
        /// </summary>
        [DataMember]
        public string FromCapitalAccount
        {
            set { _fromcapitalaccount = value; }
            get { return _fromcapitalaccount; }
        }
        /// <summary>
        /// 转入金账号
        /// </summary>
        [DataMember]
        public string ToCapitalAccount
        {
            set { _tocapitalaccount = value; }
            get { return _tocapitalaccount; }
        }
        /// <summary>
        /// 转账总金额
        /// </summary>
        [DataMember]
        public decimal TransferAmount
        {
            set { _transferamount = value; }
            get { return _transferamount; }
        }
        /// <summary>
        /// 转账时间
        /// </summary>
        [DataMember]
        public DateTime TransferTime
        {
            set { _transfertime = value; }
            get { return _transfertime; }
        }
        /// <summary>
        /// 转账货币类型ID(外键BD_CurrencyType)
        /// </summary>
        [DataMember]
        public int TradeCurrencyType
        {
            set { _tradecurrencytype = value; }
            get { return _tradecurrencytype; }
        }
        /// <summary>
        /// 转账类型ID(外键BD_TransferTypeTable)
        /// </summary>
        [DataMember]
        public int TransferTypeLogo
        {
            set { _transfertypelogo = value; }
            get { return _transfertypelogo; }
        }
        #endregion Model

    }
}

