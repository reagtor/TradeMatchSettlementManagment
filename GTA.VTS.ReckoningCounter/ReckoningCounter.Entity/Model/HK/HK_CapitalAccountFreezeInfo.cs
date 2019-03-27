using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ReckoningCounter.Entity.Model.HK
{
    /// <summary>
    /// 港股资金账户冻结实体类HK_CapitalAccountFreezeInfo 。(属性说明自动提取数据库字段的描述信息)
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    //[Serializable]
    public class HK_CapitalAccountFreezeInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HK_CapitalAccountFreezeInfo()
        { }
        #region Model
        private int _capitalFreezeLogoId;
        private int _capitalaccountid;
        private int _freezetypeid;
        private string _entrustnumber;
        private decimal _freezecapitalamount;
        private decimal _freezecost;
        private decimal _owecosting;
        private DateTime? _thawtime;
        private DateTime _freezetime;
        /// <summary>
        /// 港股资金账号冻结明细ID(主键)
        /// </summary>
        public int CapitalFreezeLogoId
        {
            set { _capitalFreezeLogoId = value; }
            get { return _capitalFreezeLogoId; }
        }
        /// <summary>
        /// 港股资金账号表的ID(外键HK_CapitalAccount)
        /// </summary>
        public int CapitalAccountLogo
        {
            set { _capitalaccountid = value; }
            get { return _capitalaccountid; }
        }
        /// <summary>
        /// 冻结类型ID(外键BD_FreezeType)
        /// </summary>
        public int FreezeTypeLogo
        {
            set { _freezetypeid = value; }
            get { return _freezetypeid; }
        }
        /// <summary>
        /// 港股委托单号
        /// </summary>
        public string EntrustNumber
        {
            set { _entrustnumber = value; }
            get { return _entrustnumber; }
        }
        /// <summary>
        /// 冻结预成交金额
        /// </summary>
        public decimal FreezeCapitalAmount
        {
            set { _freezecapitalamount = value; }
            get { return _freezecapitalamount; }
        }
        /// <summary>
        /// 冻结预成交费用(冻结手续费用)
        /// </summary>
        public decimal FreezeCost
        {
            set { _freezecost = value; }
            get { return _freezecost; }
        }
        /// <summary>
        /// 欠缴费用(交易费用 - 原始可用资金余额)
        /// </summary>
        public decimal OweCosting
        {
            set { _owecosting = value; }
            get { return _owecosting; }
        }
        /// <summary>
        /// 解冻时间
        /// </summary>
        public DateTime? ThawTime
        {
            set { _thawtime = value; }
            get { return _thawtime; }
        }
        /// <summary>
        /// 冻结时间
        /// </summary>
        public DateTime FreezeTime
        {
            set { _freezetime = value; }
            get { return _freezetime; }
        }
        #endregion Model

    }
    /// <summary>
    /// 港股资金账户冻结统计实体类HK_CapitalAccountFreezeSum 
    /// Create BY：李健华
    /// Create Date：2009-10-20
    /// </summary>
    public class HK_CapitalAccountFreezeSum
    {
        #region CapitalAccountLogo 现货资金账号表的ID(外键QH_CapitalAccountTable)
        private int capitalAccountLogo;
        /// <summary>
        /// 现货资金账号表的ID(外键QH_CapitalAccountTable)
        /// </summary>
        [DataMember]
        public int CapitalAccountLogo
        {
            get
            {
                return capitalAccountLogo;
            }
            set
            {
                capitalAccountLogo = value;
            }
        }
        #endregion

        #region FreezeCapitalSum 冻结总金额
        private decimal freezeCapitalSum;
        /// <summary>
        /// 冻结预成交金额
        /// </summary>
        [DataMember]
        public decimal FreezeCapitalSum
        {
            get
            {
                return freezeCapitalSum;
            }
            set
            {
                freezeCapitalSum = value;
            }
        }
        #endregion
    }
}
