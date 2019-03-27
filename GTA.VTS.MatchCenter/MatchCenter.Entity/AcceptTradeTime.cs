using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatchCenter.Entity
{
    /// <summary>
    /// Title:接收交易时间实体
    /// Create by:李健华
    /// Create Date:2009-10-16
    /// </summary>
    public class AcceptTradeTime
    {
        #region 撮合中心接收开始和结束撮合时间
        /// <summary>
        /// 撮合中心结束接收撮合时间
        /// </summary>
        private DateTime _acceptEndTime;
        /// <summary>
        /// 撮合中心接收撮合时间
        /// </summary>
        private DateTime _acceptStartTime;
        /// <summary>
        /// 开始接收委托时间
        /// </summary>
        public DateTime AcceptStartTime
        {
            get { return _acceptStartTime; }
            set { _acceptStartTime = value; }
        }
        /// <summary>
        /// 终止接收委托时间
        /// </summary>
        public DateTime AcceptEndTime
        {
            get { return _acceptEndTime; }
            set { _acceptEndTime = value; }
        }
        #endregion
    }
}
