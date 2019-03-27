using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
    /// <summary>
    /// 行情数据实体
    /// Create By:李健华
    /// Create Date:2010-01-27
    /// </summary>
    [DataContract]
    public class MarketDataLevel
    {
        /// <summary>
        /// 行情代码
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 卖一价
        /// </summary>
        [DataMember]
        public decimal SellFirstPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 卖一量
        /// </summary>
        [DataMember]
        public decimal SellFirstVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 卖二价
        /// </summary>
        [DataMember]
        public decimal SellSecondPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 卖二量
        /// </summary>
        [DataMember]
        public decimal SellSecondVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 卖三价
        /// </summary>
        [DataMember]
        public decimal SellThirdPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 卖三量
        /// </summary>
        [DataMember]
        public decimal SellThirdVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 买四价
        /// </summary>
        [DataMember]
        public decimal SellFourthPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 卖四量
        /// </summary>
        [DataMember]
        public decimal SellFourthVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 卖五价
        /// </summary>
        [DataMember]
        public decimal SellFivePrice
        {
            get;
            set;
        }

        /// <summary>
        /// 卖五量
        /// </summary>
        [DataMember]
        public decimal SellFiveVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 买一价
        /// </summary>
        [DataMember]
        public decimal BuyFirstPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 买一量
        /// </summary>
        [DataMember]
        public decimal BuyFirstVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 买二价
        /// </summary>
        [DataMember]
        public decimal BuySecondPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 买二量
        /// </summary>
        [DataMember]
        public decimal BuySecondVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 买三价
        /// </summary>
        [DataMember]
        public decimal BuyThirdPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 买三量
        /// </summary>
        [DataMember]
        public decimal BuyThirdVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 买四价
        /// </summary>
        [DataMember]
        public decimal BuyFourthPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 买四量
        /// </summary>
        [DataMember]
        public decimal BuyFourthVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 买五价
        /// </summary>
        [DataMember]
        public decimal BuyFivePrice
        {
            get;
            set;

        }
        /// <summary>
        /// 买五量
        /// </summary>
        [DataMember]
        public decimal BuyFiveVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 成交量
        /// </summary>
        [DataMember]
        public decimal LastVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 成交价
        /// </summary>
        [DataMember]
        public decimal LastPrice
        {
            get;
            set;
        }


        /// <summary>
        /// 涨停价
        /// </summary>
        [DataMember]
        public decimal UpPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 跌停价格
        /// </summary>
        [DataMember]
        public decimal LowerPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 昨日收盘价
        /// </summary>
        [DataMember]
        public decimal YesterPrice
        {
            get;
            set;
        }
        /// <summary>
        /// 行情刷新时间
        /// </summary>
        [DataMember]
        public DateTime MarketRefreshTime
        {
            get;
            set;
        }
    }
}
