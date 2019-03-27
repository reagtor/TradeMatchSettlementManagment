#region Using Namespace

using System;

#endregion

namespace ReckoningCounter.Model
{
    /// <summary>
    /// Title: 现货资金账户明细记录实体类
    /// Desc: 现货资金账户明细记录实体类XH_CapitalAccountTable_DeltaInfo 。(属性说明自动提取数据库字段的描述信息)
    /// Create BY: 李健华
    /// Create Date: 2009-10-15
    /// </summary>
    [Serializable]
    public class XH_CapitalAccountTable_DeltaInfo
    {
        #region Model

        private int _id;
        private int _capitalaccountlogo;
        private decimal _availablecapitaldelta;
        private DateTime _deltatime;
        private decimal _freezecapitaltotaldelta;
        private decimal _todayoutincapital;
        private decimal _hasdoneprofitlosstotaldelta;

        /// <summary>
        /// ID
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }

        /// <summary>
        /// 现货资金账户表ID
        /// </summary>
        public int CapitalAccountLogo
        {
            set { _capitalaccountlogo = value; }
            get { return _capitalaccountlogo; }
        }

        /// <summary>
        /// 可用资金增量
        /// </summary>
        public decimal AvailableCapitalDelta
        {
            set { _availablecapitaldelta = value; }
            get { return _availablecapitaldelta; }
        }

        /// <summary>
        /// 冻结资金金额增量
        /// </summary>
        public decimal FreezeCapitalTotalDelta
        {
            set { _freezecapitaltotaldelta = value; }
            get { return _freezecapitaltotaldelta; }
        }

        /// <summary>
        /// 当日出入金增量
        /// </summary>
        public decimal TodayOutInCapital
        {
            set { _todayoutincapital = value; }
            get { return _todayoutincapital; }
        }

        /// <summary>
        /// 累计已实现盈亏增量
        /// </summary>
        public decimal HasDoneProfitLossTotalDelta
        {
            set { _hasdoneprofitlosstotaldelta = value; }
            get { return _hasdoneprofitlosstotaldelta; }
        }

        /// <summary>
        /// 写入日期
        /// </summary>
        public DateTime DeltaTime
        {
            set { _deltatime = value; }
            get { return _deltatime; }
        }

        #endregion Model
    }
}