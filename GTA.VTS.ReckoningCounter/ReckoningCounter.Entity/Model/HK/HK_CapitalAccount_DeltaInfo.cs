using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReckoningCounter.Entity.Model.HK
{
    /// <summary>
    /// Title: 港股资金账户明细记录实体类
    /// Desc: 港股资金账户明细记录实体类HK_CapitalAccount_DeltaInfo 。(属性说明自动提取数据库字段的描述信息)
    /// Create BY: 李健华
    /// Create Date: 2009-10-15
    /// </summary>
    [Serializable]
    public class HK_CapitalAccount_DeltaInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HK_CapitalAccount_DeltaInfo()
        { }
        #region Model
        private int _id;
        private int _capitalaccountlogo;
        private decimal _availablecapitaldelta;
        private decimal _freezecapitaltotaldelta;
        private decimal _todayoutincapital;
        private decimal _hasdoneprofitlosstotaldelta;
        private DateTime _deltatime;
        /// <summary>
        /// 港股资金账户明细记录表ID(主键)
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 用户港股资金账号ID(外键UA_UserAccountAllocationTable)
        /// </summary>
        public int CapitalAccountLogo
        {
            set { _capitalaccountlogo = value; }
            get { return _capitalaccountlogo; }
        }
        /// <summary>
        /// 可用资金
        /// </summary>
        public decimal AvailableCapitalDelta
        {
            set { _availablecapitaldelta = value; }
            get { return _availablecapitaldelta; }
        }
        /// <summary>
        /// 冻结资金金额
        /// </summary>
        public decimal FreezeCapitalTotalDelta
        {
            set { _freezecapitaltotaldelta = value; }
            get { return _freezecapitaltotaldelta; }
        }
        /// <summary>
        /// 今天收入/支出资金总额
        /// </summary>
        public decimal TodayOutInCapital
        {
            set { _todayoutincapital = value; }
            get { return _todayoutincapital; }
        }
        /// <summary>
        /// 累计已实现盈亏
        /// </summary>
        public decimal HasDoneProfitLossTotalDelta
        {
            set { _hasdoneprofitlosstotaldelta = value; }
            get { return _hasdoneprofitlosstotaldelta; }
        }
        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime DeltaTime
        {
            set { _deltatime = value; }
            get { return _deltatime; }
        }
        #endregion Model

    }
}
