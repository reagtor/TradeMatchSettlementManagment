using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ReckoningCounter.Model
{
    /// <summary>
    /// Title: 期货资金流水表实体
    /// Desc: 实体类QH_TradeCapitalFlowDetailInfo 。(属性说明自动提取数据库字段的描述信息)
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    [DataContract]
    public class QH_TradeCapitalFlowDetailInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public QH_TradeCapitalFlowDetailInfo()
        { }

        private string _tradeid;
        private string _usercapitalaccount;
        private int _flowtypes;
        private decimal _margin;
        private decimal _tradeproceduresfee;
        private decimal _profitloss;
        private decimal _othercose;
        private decimal _flowtotal;
        private DateTime createDateTime;
        private int currencyType;

        /// <summary>
        /// 成交记录编号ID（如果是清算没有ID自动生成一个GUID）
        /// </summary>
        [DataMember]
        public string TradeID
        {
            set { _tradeid = value; }
            get { return _tradeid; }
        }
        /// <summary>
        /// 用户资金账号
        /// </summary>
        [DataMember]
        public string UserCapitalAccount
        {
            set { _usercapitalaccount = value; }
            get { return _usercapitalaccount; }
        }
        /// <summary>
        /// 流水类型 0-交易 ,1-结算
        /// </summary>
        [DataMember]
        public int FlowTypes
        {
            set { _flowtypes = value; }
            get { return _flowtypes; }
        }
        /// <summary>
        /// 保证金
        /// </summary>
        [DataMember]
        public decimal Margin
        {
            set { _margin = value; }
            get { return _margin; }
        }
        /// <summary>
        /// 交易费用
        /// </summary>
        [DataMember]
        public decimal TradeProceduresFee
        {
            set { _tradeproceduresfee = value; }
            get { return _tradeproceduresfee; }
        }
        /// <summary>
        /// 盈亏(流水类型为交易时是每笔成交平仓盯市盈亏，为结算时持仓盯市盈亏)
        /// </summary>
        [DataMember]
        public decimal ProfitLoss
        {
            set { _profitloss = value; }
            get { return _profitloss; }
        }
        /// <summary>
        /// 其他费用(用于预留可能用）
        /// </summary>
        [DataMember]
        public decimal OtherCose
        {
            set { _othercose = value; }
            get { return _othercose; }
        }
        /// <summary>
        /// 流水资金汇总
        /// </summary>
        [DataMember]
        public decimal FlowTotal
        {
            set { _flowtotal = value; }
            get { return _flowtotal; }
        }
        #region CreateDateTime 流水创建日期时间

        /// <summary>
        /// 流水创建日期时间
        /// </summary>
        [DataMember]
        public DateTime CreateDateTime
        {
            get
            {
                return createDateTime;
            }
            set
            {
                createDateTime = value;
            }
        }
        #endregion

        #region CurrencyType 流水货币类型
        /// <summary>
        /// 流水货币类型(外键BD_CurrencyType,1-RMB,2-HK,3-USA)
        /// </summary>
        [DataMember]
        public int CurrencyType
        {
            get
            {
                return currencyType;
            }
            set
            {
                currencyType = value;
            }
        }
        #endregion

    }
}
