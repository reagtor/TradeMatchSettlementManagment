using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MatchCenter.Entity
{
    /// <summary>
    /// 撮合中心实体类
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// </summary>
    [Serializable]
    public class FuseHanderEntity
    {
        private string stockCode = string.Empty;

        private int fuseCount = 0;

        private bool isFuse = false;

        private bool priorFuse = false;

        private DateTime fuseTime;

        private DateTime startFuseTime;


        /// <summary>
        /// 启动熔断时间
        /// </summary>
        public DateTime StartFuseTime
        {
            get { return startFuseTime; }
            set { startFuseTime = value; }
        }

        /// <summary>
        /// 证卷代码
        /// </summary>
        public string StockCode
        {
            set { stockCode = value; }
            get { return stockCode; }
        }

        /// <summary>
        /// 熔断次数
        /// </summary>
        public int FuseCount
        {
            get { return fuseCount; }
            set { fuseCount = value;}
        }

        /// <summary>
        /// 是否产生熔断
        /// </summary>
        public bool IsFuse
        {
            get { return isFuse; }
            set { isFuse = value; }
        }

        /// <summary>
        /// 涨跌幅超过触发比例标志
        /// </summary>
        public bool PriorFuse
        {
            get { return priorFuse; }
            set { priorFuse = value; }
        }

        /// <summary>
        /// 涨跌幅度超过触发比例时间
        /// </summary>
        public DateTime FuseTime
        {
            set { fuseTime = value; }
            get { return fuseTime; }
        }
    }
}
