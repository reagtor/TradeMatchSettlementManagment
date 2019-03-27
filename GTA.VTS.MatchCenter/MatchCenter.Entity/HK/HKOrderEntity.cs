using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using GTA.VTS.Common.CommonObject;

namespace MatchCenter.Entity.HK
{
    /// <summary>
    /// 港股下单实体
    /// Create by:李健华
    /// Create date:2009-10-16
    /// </summary>
    [DataContract]
    public class HKOrderEntity
    {
        #region  港股股东代码 SholderCode 
        /// <summary>
        /// 港股股东代码
        /// </summary>
        [DataMember]
        public string SholderCode
        {
            get;
            set;
        }
        #endregion

        #region  通道号 ChannelNo 
        /// <summary>
        /// 通道号
        /// </summary>
        [DataMember]
        public string ChannelNo
        {
            set;
            get;
        }
        #endregion

        #region 港股证卷代码 StockCode
        /// <summary>
        /// 港股证卷代码
        /// </summary>
        [DataMember]
        public string Code
        {
            set;
            get;
        }
        #endregion

        #region  委托价格 OrderPrice
        /// <summary>
        /// 委托价格
        /// </summary>
        [DataMember]
        public decimal OrderPrice
        {
            set;
            get;
        }
        #endregion

        #region 委托数量 OrderVolume
        /// <summary>
        /// 委托数量
        /// </summary>
        [DataMember]
        public decimal OrderVolume
        {
            set;
            get;
        }

        #endregion

        #region 港股报盘价格类型 HKPriceType
        /// <summary>
        /// 港股报盘价格类型
        /// </summary>
        [DataMember]
        public Types.HKPriceType HKPriceType
        {
            get;
            set;
        }
        #endregion

        #region 买卖方向 TransactionDirection
        /// <summary>
        /// 买卖方向
        /// </summary>
        [DataMember]
        public Types.TransactionDirection TransactionDirection
        {
            get;
            set;
        }
        #endregion
    
    }
}
