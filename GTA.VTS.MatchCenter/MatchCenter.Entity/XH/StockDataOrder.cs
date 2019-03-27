using System;

namespace MatchCenter.Entity
{
    /// <summary>
    /// 撮合中心委托实体类
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// </summary>
   public class StockDataOrder
    {
        #region Model
        private string _orderNo;
        private string _branchId;
        private DateTime _reachTime;
        private string _stockCode;
        private decimal _orderPrice;
        private decimal _orderVolume;
        private int _orderType;
        private string _orderMessage;

        /// <summary>
        /// 委托单号
        /// </summary>
        public string OrderNo
        {
            set { _orderNo = value; }
            get { return _orderNo; }
        }

        /// <summary>
        /// 柜台通道号
        /// </summary>
        public string BranchID
        {
            set { _branchId = value; }
            get { return _branchId; }
        }

        /// <summary>
        /// 委托到达时间
        /// </summary>
        public DateTime ReachTime
        {
            set { _reachTime = value; }
            get { return _reachTime; }
        }

        /// <summary>
        /// 证卷代码
        /// </summary>
        public string StockCode
        {
            set { _stockCode = value; }
            get { return _stockCode; }
        }
        /// <summary>
        /// 委托价格
        /// </summary>
        public decimal OrderPrice
        {
            set { _orderPrice = value; }
            get { return _orderPrice; }
        }
        /// <summary>
        /// 委托数量
        /// </summary>
        public decimal OrderVolume
        {
            set { _orderVolume = value; }
            get { return _orderVolume; }
        }
        /// <summary>
        /// 委托类型
        /// </summary>
        public int OrderType
        {
            set { _orderType = value; }
            get { return _orderType; }
        }
        /// <summary>
        /// 委托消息
        /// </summary>
        public string OrderMessage
        {
            set { _orderMessage = value; }
            get { return _orderMessage; }
        }
        #endregion Model
    }
}
