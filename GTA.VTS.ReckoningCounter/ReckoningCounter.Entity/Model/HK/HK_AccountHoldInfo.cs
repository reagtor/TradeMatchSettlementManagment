using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReckoningCounter.Entity.Model.HK
{
    /// <summary>
    /// 港股持仓实体类HK_AccountHoldInfo 。(属性说明自动提取数据库字段的描述信息)
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    //[Serializable]
    public class HK_AccountHoldInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HK_AccountHoldInfo()
        { }
        #region Model
        private int _accountholdid;
        private string _userAccountDistributeLogo;
        private int _currencytypeid;
        private string _code;
        private decimal _availableamount;
        private decimal _freezeamount;
        private decimal _costprice;
        private decimal _breakevenprice;
        private decimal _holdaverageprice;
        /// <summary>
        /// 
        /// </summary>
        public int AccountHoldLogoID
        {
            set { _accountholdid = value; }
            get { return _accountholdid; }
        }
        /// <summary>
        /// 港股持仓账号(外键UA_UserAccountAllocationTable)
        /// </summary>
        public string UserAccountDistributeLogo
        {
            set { _userAccountDistributeLogo = value; }
            get { return _userAccountDistributeLogo; }
        }
        /// <summary>
        /// 当前交易货币类型(外键BD_CurrencyType)
        /// </summary>
        public int CurrencyTypeID
        {
            set { _currencytypeid = value; }
            get { return _currencytypeid; }
        }
        /// <summary>
        /// 港股证券商品编码(这与管理中心CM_Commodity的ID对应)
        /// </summary>
        public string Code
        {
            set { _code = value; }
            get { return _code; }
        }
        /// <summary>
        /// 持仓商品可用总量
        /// </summary>
        public decimal AvailableAmount
        {
            set { _availableamount = value; }
            get { return _availableamount; }
        }
        /// <summary>
        /// 持仓商品冻结总量
        /// </summary>
        public decimal FreezeAmount
        {
            set { _freezeamount = value; }
            get { return _freezeamount; }
        }
        /// <summary>
        /// 成本价格
        /// </summary>
        public decimal CostPrice
        {
            set { _costprice = value; }
            get { return _costprice; }
        }
        /// <summary>
        /// 保本价格
        /// </summary>
        public decimal BreakevenPrice
        {
            set { _breakevenprice = value; }
            get { return _breakevenprice; }
        }
        /// <summary>
        /// 持仓均价
        /// </summary>
        public decimal HoldAveragePrice
        {
            set { _holdaverageprice = value; }
            get { return _holdaverageprice; }
        }
        #endregion Model

    }

    /// <summary>
    /// 持仓的变化对象
    /// </summary>
    public class HK_AccountHoldInfo_Delta
    {
        /// <summary>
        /// 港股持仓实体
        /// </summary>
        public HK_AccountHoldInfo Data;

        /// <summary>
        /// 持仓账户Id
        /// </summary>
        public int AccountHoldLogoId;

        /// <summary>
        /// 可以持仓变化量
        /// </summary>
        public decimal AvailableAmountDelta;

        /// <summary>
        /// 冻结变化量
        /// </summary>
        public decimal FreezeAmountDelta;
    }
}
