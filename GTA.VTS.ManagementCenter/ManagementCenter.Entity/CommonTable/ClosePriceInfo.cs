using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ManagementCenter.Model
{
    /// <summary>
    ///描述:现货收盘价表 实体类ClosePriceInfo
    ///作者：刘书伟
    ///日期:2009-11-27
    /// </summary>
    [DataContract]
    public class ClosePriceInfo
    {
        public ClosePriceInfo()
        {
        }

        #region Model

        private string _StockCode;
        private decimal _ClosePrice;
        private DateTime _ClosePriceDate;
        private int _BreedClassTypeID;

        /// <summary>
        /// 股票代码
        /// </summary>
        [DataMember]
        public string StockCode
        {
            set { _StockCode = value; }
            get { return _StockCode; }
        }

        /// <summary>
        /// 收盘价
        /// </summary>
        [DataMember]
        public decimal ClosePrice
        {
            set { _ClosePrice = value; }
            get { return _ClosePrice; }
        }

        /// <summary>
        /// 收盘价取值日期
        /// </summary>
        [DataMember]
        public DateTime ClosePriceDate
        {
            set { _ClosePriceDate = value; }
            get { return _ClosePriceDate; }
        }

        /// <summary>
        /// 品种类型标识
        /// </summary>
        [DataMember]
        public int BreedClassTypeID
        {
            set { _BreedClassTypeID = value; }
            get { return _BreedClassTypeID; }
        }
        #endregion

    }
}
