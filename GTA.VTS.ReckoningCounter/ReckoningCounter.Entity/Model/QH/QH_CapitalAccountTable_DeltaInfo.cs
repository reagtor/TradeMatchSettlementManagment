using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ReckoningCounter.Model
{
    /// <summary>
    /// Title: 期货资金账户明细记录实体类
    /// Desc: 期货资金账户明细记录实体类QH_CapitalAccountTable_Delta 。(属性说明自动提取数据库字段的描述信息)
    /// Create BY: 李健华
    /// Create Date: 2009-10-15
    /// </summary>
    [DataContract]
    public class QH_CapitalAccountTable_DeltaInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public QH_CapitalAccountTable_DeltaInfo()
        { }
        #region Model
        private int _id;
        private int _capitalaccountlogoid;
        private decimal _availablecapitaldelta;
        private decimal _freezecapitaltotaldelta;
        private decimal _todayoutincapitaldelta;
        private decimal _margintotaldelta;
        private decimal _closefloatprofitlosstotaldelta;
        private decimal _closemarketprofitlosstotaldelta;
        private DateTime _deltatime;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 期货资金账户ID(主键)
        /// </summary>
        [DataMember]
        public int CapitalAccountLogoId
        {
            set { _capitalaccountlogoid = value; }
            get { return _capitalaccountlogoid; }
        }
        /// <summary>
        /// 可用资金增量
        /// </summary>
        [DataMember]
        public decimal AvailableCapitalDelta
        {
            set { _availablecapitaldelta = value; }
            get { return _availablecapitaldelta; }
        }
        /// <summary>
        /// 今天冻结资金总额增量
        /// </summary>
        [DataMember]
        public decimal FreezeCapitalTotalDelta
        {
            set { _freezecapitaltotaldelta = value; }
            get { return _freezecapitaltotaldelta; }
        }
        /// <summary>
        /// 今天收入/支出资金总额增量
        /// </summary>
        [DataMember]
        public decimal TodayOutInCapitalDelta
        {
            set { _todayoutincapitaldelta = value; }
            get { return _todayoutincapitaldelta; }
        }
        /// <summary>
        /// 保证金总额增量
        /// </summary>
        [DataMember]
        public decimal MarginTotalDelta
        {
            set { _margintotaldelta = value; }
            get { return _margintotaldelta; }
        }
        /// <summary>
        /// 总平仓盈亏（浮)增量
        /// </summary>
        [DataMember]
        public decimal CloseFloatProfitLossTotalDelta
        {
            set { _closefloatprofitlosstotaldelta = value; }
            get { return _closefloatprofitlosstotaldelta; }
        }
        /// <summary>
        /// 总平仓盈亏（盯）增量
        /// </summary>
        [DataMember]
        public decimal CloseMarketProfitLossTotalDelta
        {
            set { _closemarketprofitlosstotaldelta = value; }
            get { return _closemarketprofitlosstotaldelta; }
        }

        /// <summary>
        /// 写入日期
        /// </summary>
        [DataMember]
        public DateTime DeltaTime
        {
            set { _deltatime = value; }
            get { return _deltatime; }
        }
        #endregion Model

    }
}
