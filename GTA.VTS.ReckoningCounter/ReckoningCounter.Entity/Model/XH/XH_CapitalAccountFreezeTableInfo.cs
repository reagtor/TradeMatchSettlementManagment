using System;
using System.Runtime.Serialization;
namespace ReckoningCounter.Model
{
    /// <summary>
    /// Title: 现货资金账户冻结实体类
    /// Desc: 现货资金账户冻结实体类XH_CapitalAccountFreezeTable 。(属性说明自动提取数据库字段的描述信息)
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    [DataContract]
    public class XH_CapitalAccountFreezeTableInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public XH_CapitalAccountFreezeTableInfo()
        { }
        #region CapitalFreezeLogoId 现货资金账号冻结明细ID(主键)
        private int capitalFreezeLogoId;
        /// <summary>
        /// 现货资金账号冻结明细ID(主键)
        /// </summary>
        [DataMember]
        public int CapitalFreezeLogoId
        {
            get
            {
                return capitalFreezeLogoId;
            }
            set
            {
                capitalFreezeLogoId = value;
            }
        }
        #endregion

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

        #region FreezeTypeLogo 冻结类型ID(外键BD_FreezeType)
        private int freezeTypeLogo;
        /// <summary>
        /// 冻结类型ID(外键BD_FreezeType)
        /// </summary>
        [DataMember]
        public int FreezeTypeLogo
        {
            get
            {
                return freezeTypeLogo;
            }
            set
            {
                freezeTypeLogo = value;
            }
        }
        #endregion

        #region EntrustNumber 现货委托单号
        private string entrustNumber;
        /// <summary>
        /// 现货委托单号
        /// </summary>
        [DataMember]
        public string EntrustNumber
        {
            get
            {
                return entrustNumber;
            }
            set
            {
                entrustNumber = value;
            }
        }
        #endregion

        #region FreezeCapitalAmount 冻结预成交金额
        private decimal freezeCapitalAmount;
        /// <summary>
        /// 冻结预成交金额
        /// </summary>
        [DataMember]
        public decimal FreezeCapitalAmount
        {
            get
            {
                return freezeCapitalAmount;
            }
            set
            {
                freezeCapitalAmount = value;
            }
        }
        #endregion

        #region FreezeCost 冻结预成交费用(冻结手续费用)
        private decimal freezeCost;
        /// <summary>
        /// 冻结预成交费用(冻结手续费用)
        /// </summary>
        [DataMember]
        public decimal FreezeCost
        {
            get
            {
                return freezeCost;
            }
            set
            {
                freezeCost = value;
            }
        }
        #endregion

        #region OweCosting 欠缴费用(交易费用 - 原始可用资金余额)
        private decimal oweCosting;
        /// <summary>
        /// 欠缴费用(交易费用 - 原始可用资金余额)
        /// </summary>
        [DataMember]
        public decimal OweCosting
        {
            get
            {
                return oweCosting;
            }
            set
            {
                oweCosting = value;
            }
        }
        #endregion

        #region ThawTime 解冻时间
        private DateTime? thawTime;
        /// <summary>
        /// 解冻时间
        /// </summary>
        [DataMember]
        public DateTime? ThawTime
        {
            get
            {
                return thawTime;
            }
            set
            {
                thawTime = value;
            }
        }
        #endregion

        #region FreezeTime 冻结时间
        private DateTime freezeTime;
        /// <summary>
        /// 冻结时间
        /// </summary>
        [DataMember]
        public DateTime FreezeTime
        {
            get
            {
                return freezeTime;
            }
            set
            {
                freezeTime = value;
            }
        }
        #endregion

    }

    /// <summary>
    /// Title: 现货资金账户冻结合计实体类
    /// Desc: 现货资金账户冻结合计实体类XH_CapitalAccountFreezeSum 。(属性说明自动提取数据库字段的描述信息)
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    public class XH_CapitalAccountFreezeSum
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

