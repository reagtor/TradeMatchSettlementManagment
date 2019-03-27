using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReckoningCounter.DAL.MatchCenterOrderDealRpt;

namespace ReckoningCounter.BLL.Reckoning.Logic
{
    /// <summary>
    /// 成交回报包裹类
    /// </summary>
    /// <typeparam name="TDealBack">原始成交回报</typeparam>
    /// <typeparam name="TCostResult">费用结果</typeparam>
    public class ReckonDealBackEx<TDealBack, TCostResult>
    {
        private TDealBack dealBack;
        private TCostResult costResult;
        private decimal dealCapital;
        private decimal dealCost;

        public ReckonDealBackEx(TDealBack dealBack)
        {
            this.dealBack = dealBack;
        }

        public TDealBack DealBack
        {
            get
            {
                return dealBack;
            }
        }

        public TCostResult CostResult
        {
            get
            {
                return costResult;
            }
            set
            {
                costResult = value;
            }
        }

        /// <summary>
        /// 成交金额
        /// </summary>
        public decimal DealCapital
        {
            get
            {
                return dealCapital;
            }
            set
            {
                dealCapital = value;
            }
        }

        /// <summary>
        /// 成交费用
        /// </summary>
        public decimal DealCost
        {
            get
            {
                return dealCost;
            }
            set
            {
                dealCost = value;
            }
        }
    }

    /// <summary>
    /// 撤单回报包裹类
    /// </summary>
    public class CancelOrderEntityEx : CancelOrderEntity
    {
        /// <summary>
        /// 是否是内部撤单
        /// </summary>
        public bool IsInternalCancelOrder = false;

        /// <summary>
        /// 是否是多个撤单组合而成
        /// </summary>
        public bool IsMultiCancelOrder;

        /// <summary>
        /// 如果是多个撤单组合而成，那么记录所有的撤单ID
        /// </summary>
        public List<string> IDList;
        /// <summary>
        /// 如果是多个撤单组合而成，那么记录所有的撤单ID对应的撤单量
        /// add 李健华 2009-11-07============
        /// </summary>
        public List<decimal> OrderVolumeList;
    }
}
