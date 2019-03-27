using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReckoningCounter.Model
{
    /// <summary>
    /// Title: 现货持仓明细实体类
    /// Desc: 现货持仓明细实体类XH_AccountHoldTable_Flow 。(属性说明自动提取数据库字段的描述信息)
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    [Serializable]
    public class XH_AccountHoldTable_FlowInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public XH_AccountHoldTable_FlowInfo()
        { }
        #region Model
        private int _id;
        private int _accountholdlogoid;
        private decimal _availableamount;
        private decimal _freezeamount;
        private decimal _costprice;
        private decimal _breakevenprice;
        private decimal _holdaverageprice;
        private DateTime _flowtime;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int AccountHoldLogoId
        {
            set { _accountholdlogoid = value; }
            get { return _accountholdlogoid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal AvailableAmount
        {
            set { _availableamount = value; }
            get { return _availableamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal FreezeAmount
        {
            set { _freezeamount = value; }
            get { return _freezeamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal CostPrice
        {
            set { _costprice = value; }
            get { return _costprice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal BreakevenPrice
        {
            set { _breakevenprice = value; }
            get { return _breakevenprice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal HoldAveragePrice
        {
            set { _holdaverageprice = value; }
            get { return _holdaverageprice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime FlowTime
        {
            set { _flowtime = value; }
            get { return _flowtime; }
        }
        #endregion Model

    }
}
