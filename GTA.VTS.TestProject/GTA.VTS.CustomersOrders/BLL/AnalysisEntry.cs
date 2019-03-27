using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA.VTS.CustomersOrders.BLL
{
    /// <summary>
    /// 统计分析相关实体(本实体都设置为字符串是为了后面方法操作)
    /// </summary>
    public class AnalysisEntry
    {
        /// <summary>
        /// 总量
        /// </summary>
        public string Count { get; set; }
        /// <summary>
        /// 品种类型
        /// </summary>
        public string BreedClassName { get; set; }
        /// <summary>
        /// 已成  10
        /// </summary>
        public string Dealed { get; set; }
        /// <summary>
        /// 部成   9
        /// </summary>
        public string PartDealed { get; set; }
        /// <summary>
        /// 废单   6
        /// </summary>
        public string Canceled { get; set; }
        /// <summary>
        /// 已撤   7
        /// </summary>
        public string Removed { get; set; }
        /// <summary>
        ///  已报待撤  4
        /// </summary>
        public string RequiredRemoveSoon { get; set; }
        /// <summary>
        ///  部撤   8 
        /// </summary>
        public string PartRemoved { get; set; }
        /// <summary>
        ///  已报  5
        /// </summary>
        public string IsRequired { get; set; }
        /// <summary>
        ///  部成待撤 11
        /// </summary>
        public string PartDealRemoveSoon { get; set; }
        /// <summary>
        /// 待报  3
        /// </summary>
        public string RequiredSoon { get; set; }
        /// <summary>
        ///  未报 2
        /// </summary>
        public string UnRequired { get; set; }
        /// <summary>
        ///   无状态 1
        /// </summary>
        public string None { get; set; }

        /// <summary>
        /// 盯市盈亏(已实现盈亏)
        /// </summary>
        public string MarketProfitLoss { get; set; }
        /// <summary>
        /// 浮动盈亏
        /// </summary>
        public string FloatProfitLoss { get; set; }

       

    }



}
