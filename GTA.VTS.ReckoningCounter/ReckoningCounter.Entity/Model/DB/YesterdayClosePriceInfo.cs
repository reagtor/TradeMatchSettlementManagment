using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReckoningCounter.Entity
{
    /// <summary>
    /// Title:昨日收盘价
    /// Desc.:本实体只要是为了过滤管理中心中的数据而设置的实体，不用缓存多余的数据
    /// Create by:李健华
    /// Create date:2009-11-28
    /// </summary>
    public class YesterdayClosePriceInfo
    {
        /// <summary>
        /// 商品代码标识
        /// </summary>
        public string Code
        {
            get;
            set;
        }

        /// <summary>
        /// 昨日收盘价
        /// </summary>
        public decimal ClosePrice
        {
            get;
            set;
        }
    }
}
