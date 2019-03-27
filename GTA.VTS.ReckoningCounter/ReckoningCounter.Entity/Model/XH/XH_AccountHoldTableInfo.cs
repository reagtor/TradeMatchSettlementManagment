using System;
using System.Runtime.Serialization;
namespace ReckoningCounter.Model
{
    /// <summary>
    /// Title: 现货持仓账户的所拥有持仓信息明细实体
    /// Desc: XH_AccountHoldTable-现货持仓账户的所拥有持仓信息明细实体。(属性说明自动提取数据库字段的描述信息)
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    [DataContract]
    public class XH_AccountHoldTableInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public XH_AccountHoldTableInfo()
        { }
        #region AccountHoldLogoId 持仓ID主键
        private int accountHoldLogoId;
        /// <summary>
        /// 持仓ID主键
        /// </summary>
        [DataMember]
        public int AccountHoldLogoId
        {
            get
            {
                return accountHoldLogoId;
            }
            set
            {
                accountHoldLogoId = value;
            }
        }
        #endregion

        #region UserAccountDistributeLogo 持仓账号(外键UA_UserAccountAllocationTable)
        private string userAccountDistributeLogo;
        /// <summary>
        /// 持仓账号(外键UA_UserAccountAllocationTable)
        /// </summary>
        [DataMember]
        public string UserAccountDistributeLogo
        {
            get
            {
                return userAccountDistributeLogo;
            }
            set
            {
                userAccountDistributeLogo = value;
            }
        }
        #endregion

        #region CurrencyTypeId 当前交易货币类型(外键BD_CurrencyType)
        private int currencyTypeId;
        /// <summary>
        /// 当前交易货币类型(外键BD_CurrencyType)
        /// </summary>
        [DataMember]
        public int CurrencyTypeId
        {
            get
            {
                return currencyTypeId;
            }
            set
            {
                currencyTypeId = value;
            }
        }
        #endregion

        #region Code 持仓商品编码(这与管理中心CM_Commodity的ID对应)
        private string code;
        /// <summary>
        /// 持仓商品编码(这与管理中心CM_Commodity的ID对应)
        /// </summary>
        [DataMember]
        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
            }
        }
        #endregion

        #region AvailableAmount 持仓商品可用总量
        private decimal availableAmount;
        /// <summary>
        /// 持仓商品可用总量
        /// </summary>
        [DataMember]
        public decimal AvailableAmount
        {
            get
            {
                return availableAmount;
            }
            set
            {
                availableAmount = value;
            }
        }
        #endregion

        #region FreezeAmount 持仓商品冻结总量
        private decimal freezeAmount;
        /// <summary>
        /// 持仓商品冻结总量
        /// </summary>
        [DataMember]
        public decimal FreezeAmount
        {
            get
            {
                return freezeAmount;
            }
            set
            {
                freezeAmount = value;
            }
        }
        #endregion

        #region CostPrice 成本价格
        private decimal costPrice;
        /// <summary>
        /// 成本价格
        /// </summary>
        [DataMember]
        public decimal CostPrice
        {
            get
            {
                return costPrice;
            }
            set
            {
                costPrice = value;
            }
        }
        #endregion

        #region BreakevenPrice 保本价格
        private decimal breakevenPrice;
        /// <summary>
        /// 保本价格
        /// </summary>
        [DataMember]
        public decimal BreakevenPrice
        {
            get
            {
                return breakevenPrice;
            }
            set
            {
                breakevenPrice = value;
            }
        }
        #endregion

        #region HoldAveragePrice 持仓均价
        private decimal holdAveragePrice;
        /// <summary>
        /// 持仓均价
        /// </summary>
        [DataMember]
        public decimal HoldAveragePrice
        {
            get
            {
                return holdAveragePrice;
            }
            set
            {
                holdAveragePrice = value;
            }
        }
        #endregion

    }

    /// <summary>
    /// 持仓的变化对象
    /// </summary>
    public class XH_AccountHoldTableInfo_Delta
    {
        /// <summary>
        /// 现货持仓实体
        /// </summary>
        public XH_AccountHoldTableInfo Data;

        /// <summary>
        /// 持仓账户Id
        /// </summary>
        public int AccountHoldLogoId;

        /// <summary>
        /// 可用持仓变化量
        /// </summary>
        public decimal AvailableAmountDelta;

        /// <summary>
        /// 冻结变化量
        /// </summary>
        public decimal FreezeAmountDelta;
    }
}

