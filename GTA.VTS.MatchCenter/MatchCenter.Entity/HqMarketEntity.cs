using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatchCenter.Entity
{
    /// <summary>
    /// 撮合中心行情实体类
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// </summary>
    public class HqMarketEntity
    {
        private decimal lastVolume;
        private decimal upPrice;
        private decimal lowerPrice;
        private decimal lastPrice;
        private decimal buyFirstPrice;
        private decimal buyFirstVolume;
        private decimal buySecondPrice;
        private decimal buySecondVolume;
        private decimal buyThirdPrice;
        private decimal buyThirdVolume;
        private decimal buyFourthPrice;
        private decimal buyFourthVolume;
        private decimal buyFivePrice;
        private decimal buyFiveVolume;
        private decimal sellFirstPrice;
        private decimal sellFirstVolume;
        private decimal sellSecondVolume;
        private decimal sellSecondPrice;
        private decimal sellThirdPrice;
        private decimal sellThirdVolume;
        private decimal sellFourthPrice;
        private decimal sellFourthVolume;
        private decimal sellFivePrice;
        private decimal sellFiveVolume;
        private bool isBuyFirst;
        private bool isBuySecond;
        private bool isBuyThird;
        private bool isBuyFourth;
        private bool isBuyFive;
        private bool isSellFirst;
        private bool isSellSecond;
        private bool isSellThird;
        private bool isSellFourth;
        private bool isSellFive;

        /// <summary>
        /// 买一是否已经作了市场存量
        /// </summary>
        public bool IsBuyFirst
        {
            get
            {
                return isBuyFirst;
            }
            set
            {
                isBuyFirst = value;
            }
        }

        /// <summary>
        /// 买二是否已经做了市场存量
        /// </summary>
        public bool IsBuySecond
        {
            get
            {
                return isBuySecond;
            }
            set
            {
                isBuySecond = value;
            }
        }

        /// <summary>
        /// 买三是否已经作了市场存量
        /// </summary>
        public bool IsBuyThird
        {
            get
            {
                return isBuyThird;
            }
            set
            {
                isBuyThird = value;
            }
        }

        /// <summary>
        /// 买四是否已经做了市场存量
        /// </summary>
        public bool IsBuyFourth
        {
            get
            {
                return isBuyFourth;
            }
            set
            {
                isBuyFourth = value;
            }
        }

        /// <summary>
        /// 买五是否已经做了市场存量
        /// </summary>
        public bool IsBuyFive
        {
            get
            {
                return isBuyFive;
            }
            set
            {
                isBuyFive = value;
            }
        }

        /// <summary>
        /// 卖一是否已经做了市场存量
        /// </summary>
        public bool IsSellFirst
        {
            get
            {
                return isSellFirst;
            }
            set
            {
                isSellFirst = value;
            }
        }

        /// <summary>
        /// 卖二是否已经做了市场存量
        /// </summary>
        public bool IsSellSecond
        {
            get
            {
                return isSellSecond;
            }
            set
            {
                isSellSecond = value;
            }
        }

        /// <summary>
        /// 卖三是否已经做了市场存量
        /// </summary>
        public bool IsSellThird
        {
            get
            {
                return isSellThird;
            }
            set
            {
                isSellThird = value;
            }
        }

        /// <summary>
        /// 卖四是否已经做了市场存量
        /// </summary>
        public bool IsSellFourth
        {
            get
            {
                return isSellFourth;
            }
            set
            {
                isSellFourth = value;
            }
        }

        /// <summary>
        /// 卖五是否已经做了市场存量
        /// </summary>
        public bool IsSellFive
        {
            get
            {
                return isSellFive;
            }
            set
            {
                isSellFive = value;
            }
        }

        /// <summary>
        /// 卖一价
        /// </summary>
        public decimal SellFirstPrice
        {
            get
            {
                return sellFirstPrice;
            }
            set
            {
                sellFirstPrice = value;
            }
        }

        /// <summary>
        /// 卖一量
        /// </summary>
        public decimal SellFirstVolume
        {
            get
            {
                return sellFirstVolume;
            }
            set
            {
                sellFirstVolume = value;
            }
        }

        /// <summary>
        /// 卖二价
        /// </summary>
        public decimal SellSecondPrice
        {
            get
            {
                return sellSecondPrice;
            }
            set
            {
                sellSecondPrice = value;
            }
        }

        /// <summary>
        /// 卖二量
        /// </summary>
        public decimal SellSecondVolume
        {
            get
            {
                return sellSecondVolume;
            }
            set
            {
                sellSecondVolume = value;
            }
        }

        /// <summary>
        /// 卖三价
        /// </summary>
        public decimal SellThirdPrice
        {
            get
            {
                return sellThirdPrice;
            }
            set
            {
                sellThirdPrice = value;
            }
        }

        /// <summary>
        /// 卖三量
        /// </summary>
        public decimal SellThirdVolume
        {
            get
            {
                return sellThirdVolume;
            }
            set
            {
                sellThirdVolume = value;
            }
        }

        /// <summary>
        /// 买四价
        /// </summary>
        public decimal SellFourthPrice
        {
            get
            {
                return sellFourthPrice;
            }
            set
            {
                sellFourthPrice = value;
            }
        }

        /// <summary>
        /// 卖四量
        /// </summary>
        public decimal SellFourthVolume
        {
            get
            {
                return sellFourthVolume;
            }
            set
            {
                sellFourthVolume = value;
            }
        }

        /// <summary>
        /// 卖五价
        /// </summary>
        public decimal SellFivePrice
        {
            get
            {
                return sellFivePrice;
            }
            set
            {
                sellFivePrice = value;
            }
        }

        /// <summary>
        /// 卖五量
        /// </summary>
        public decimal SellFiveVolume
        {
            get
            {
                return sellFiveVolume;
            }
            set
            {
                sellFiveVolume = value;
            }
        }

        /// <summary>
        /// 买一价
        /// </summary>
        public decimal BuyFirstPrice
        {
            get
            {
                return buyFirstPrice;
            }
            set
            {
                buyFirstPrice = value;
            }
        }

        /// <summary>
        /// 买一量
        /// </summary>
        public decimal BuyFirstVolume
        {
            get
            {
                return buyFirstVolume;
            }
            set
            {
                buyFirstVolume = value;
            }
        }

        /// <summary>
        /// 买二价
        /// </summary>
        public decimal BuySecondPrice
        {
            get
            {
                return buySecondPrice;
            }
            set
            {
                buySecondPrice = value;
            }
        }

        /// <summary>
        /// 买二量
        /// </summary>
        public decimal BuySecondVolume
        {
            get
            {
                return buySecondVolume;
            }
            set
            {
                buySecondVolume = value;
            }
        }

        /// <summary>
        /// 买三价
        /// </summary>
        public decimal BuyThirdPrice
        {
            get
            {
                return buyThirdPrice;
            }
            set
            {
                buyThirdPrice = value;
            }
        }

        /// <summary>
        /// 买三量
        /// </summary>
        public decimal BuyThirdVolume
        {
            get
            {
                return buyThirdVolume;
            }
            set
            {
                buyThirdVolume = value;
            }
        }

        /// <summary>
        /// 买四价
        /// </summary>
        public decimal BuyFourthPrice
        {
            get
            {
                return buyFourthPrice;
            }
            set
            {
                buyFourthPrice = value;
            }
        }

        /// <summary>
        /// 买四量
        /// </summary>
        public decimal BuyFourthVolume
        {
            get
            {
                return buyFourthVolume;
            }
            set
            {
                buyFourthVolume = value;
            }
        }

        /// <summary>
        /// 买五价
        /// </summary>
        public decimal BuyFivePrice
        {
            get
            {
                return buyFivePrice;
            }
            set
            {
                buyFivePrice = value;
            }

        }
        /// <summary>
        /// 买五量
        /// </summary>
        public decimal BuyFiveVolume
        {
            get
            {
                return buyFiveVolume;
            }
            set
            {
                buyFiveVolume = value;
            }
        }

        /// <summary>
        /// 成交量
        /// </summary>
        public decimal LastVolume
        {
            get
            {
                return lastVolume;
            }
            set
            {
                lastVolume = value;
            }
        }

        /// <summary>
        /// 成交价
        /// </summary>
        public decimal LastPrice
        {
            get
            {
                return lastPrice;
            }
            set
            {
                lastPrice = value;
            }
        }


        /// <summary>
        /// 涨停价
        /// </summary>
        public decimal UpPrice
        {
            get
            {
                return upPrice;
            }
            set
            {
                upPrice = value;
            }
        }

        /// <summary>
        /// 跌停价格
        /// </summary>
        public decimal LowerPrice
        {
            get
            {
                return lowerPrice;
            }
            set
            {
                lowerPrice = value;
            }
        }


    }
}
