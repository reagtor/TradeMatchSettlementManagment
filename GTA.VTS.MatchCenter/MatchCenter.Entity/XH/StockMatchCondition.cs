using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatchCenter.Entity
{
    /// <summary>
    /// 撮合中心实体类
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// </summary>
    public class StockMatchCondition
    {
        private string _stockCode = string.Empty;
        private decimal _orderPrice;
        private decimal _marketCount;
        private decimal _marketPartial;
        private string _tableName = string.Empty;

        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName
        {
            get
            {
                return _tableName;
            }
            set
            {
                _tableName = value;
            }
        }

        /// <summary>
        /// 股票代码
        /// </summary>
        public string StockCode
        {
            get { return _stockCode; }
            set { _stockCode = value; }
        }

        /// <summary>
        /// 股票价格
        /// </summary>
        public decimal OrderPrice
        {
            get { return _orderPrice; }
            set { _orderPrice = value; }
        }

        /// <summary>
        /// 股票行情总量
        /// </summary>
        public decimal MarkCount
        {
            get { return _marketCount; }
            set { _marketCount = value; }
        }

        /// <summary>
        /// 市场参与度
        /// </summary>
        public decimal MarketPartial
        {
            get { return _marketPartial; }
            set { _marketPartial = value; }
        }
    }
}
