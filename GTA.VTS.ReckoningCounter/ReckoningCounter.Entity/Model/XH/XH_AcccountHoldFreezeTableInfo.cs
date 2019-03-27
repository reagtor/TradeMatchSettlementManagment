using System;
using System.Runtime.Serialization;
namespace ReckoningCounter.Model
{
    /// <summary>
    /// Title: 现货持仓账号的所持仓冻结明细实体类
    /// Desc: XH_AcccountHoldFreezeTable现货持仓账号的所持仓冻结明细 。(属性说明自动提取数据库字段的描述信息)
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    [DataContract]
    public class XH_AcccountHoldFreezeTableInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public XH_AcccountHoldFreezeTableInfo()
        { }
        #region HoldFreezeLogoId 现货持仓账号冻结记录主键
        private int holdFreezeLogoId;
        /// <summary>
        /// 现货持仓账号冻结记录主键
        /// </summary>
        [DataMember]
        public int HoldFreezeLogoId
        {
            get
            {
                return holdFreezeLogoId;
            }
            set
            {
                holdFreezeLogoId = value;
            }
        }
        #endregion

        #region EntrustNumber 委托单号
        private string entrustNumber;
        /// <summary>
        /// 委托单号
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

        #region PrepareFreezeAmount 准备冻结总量
        private int prepareFreezeAmount;
        /// <summary>
        /// 准备冻结总量
        /// </summary>
        [DataMember]
        public int PrepareFreezeAmount
        {
            get
            {
                return prepareFreezeAmount;
            }
            set
            {
                prepareFreezeAmount = value;
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

        #region AccountHoldLogo 所拥有持仓ID(XH_AccountHoldTable)，这是对所持仓的冻结
        private int accountHoldLogo;
        /// <summary>
        /// 所拥有持仓ID(XH_AccountHoldTable)，这是对所持仓的冻结
        /// </summary>
        [DataMember]
        public int AccountHoldLogo
        {
            get
            {
                return accountHoldLogo;
            }
            set
            {
                accountHoldLogo = value;
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
}

