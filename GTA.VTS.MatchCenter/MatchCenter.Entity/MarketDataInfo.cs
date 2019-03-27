using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MatchCenter.Entity
{
    /// <summary>
    /// Desc: 行情数据实体
    /// Create By: 董鹏
    /// Create Date:2010-01-27
    /// </summary>
    public class MarketDataInfo
    {
        /// <summary>
        /// 行情代码
        /// </summary>
        public string Code { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 卖一价
        /// </summary>
        public decimal SellFirstPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 卖一量
        /// </summary>
        public decimal SellFirstVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 卖二价
        /// </summary>
        public decimal SellSecondPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 卖二量
        /// </summary>
        public decimal SellSecondVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 卖三价
        /// </summary>
        public decimal SellThirdPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 卖三量
        /// </summary>
        public decimal SellThirdVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 买四价
        /// </summary>
        public decimal SellFourthPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 卖四量
        /// </summary>
        public decimal SellFourthVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 卖五价
        /// </summary>
        public decimal SellFivePrice
        {
            get;
            set;
        }

        /// <summary>
        /// 卖五量
        /// </summary>
        public decimal SellFiveVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 买一价
        /// </summary>
        public decimal BuyFirstPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 买一量
        /// </summary>
        public decimal BuyFirstVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 买二价
        /// </summary>
        public decimal BuySecondPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 买二量
        /// </summary>
        public decimal BuySecondVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 买三价
        /// </summary>
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
        public decimal BuyFourthPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 买四量
        /// </summary>
        public decimal BuyFourthVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 买五价
        /// </summary>
        public decimal BuyFivePrice
        {
            get;
            set;

        }
        /// <summary>
        /// 买五量
        /// </summary>
        public decimal BuyFiveVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 成交量
        /// </summary>
        public decimal LastVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 成交价
        /// </summary>
        public decimal LastPrice
        {
            get;
            set;
        }


        /// <summary>
        /// 涨停价
        /// </summary>
        public decimal UpPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 跌停价格
        /// </summary>
        public decimal LowerPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 昨日收盘价
        /// </summary>
        public decimal YesterPrice
        {
            get;
            set;
        }
    }
}
