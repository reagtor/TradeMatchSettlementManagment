using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using GTA.VTS.Common.CommonObject;

namespace MatchCenter.Entity.HK
{
    /// <summary>
    /// 港股委托单实体类HKEntrustOrderInfo 。(属性说明自动提取数据库字段的描述信息)
    /// Create BY：李健华
    /// Create Date：2009-10-15   
    /// Desc.:添加字段和修改
    /// Update BY：李健华
    /// Update Date：2009-10-19
    /// Desc.:添加字段和修改(增加撮合状态)
    /// Update BY：王伟
    /// Update Date：2009-10-19
    /// </summary>
    [DataContract]
    public class HKEntrustOrderInfo
    {
        public HKEntrustOrderInfo()
        { }
        #region Model
        private string _sholderCode;
        private string _orderno;
        private string _branchid;
        private string _hksecuritiescode;
        private decimal _oldvolume;
        private DateTime _receivetime;
        private decimal _orderprice;
        private decimal _ordervolume;
        private int _ordertype;
        private int _tradetype;
        private string _ordermessage;
        private decimal markLeft;
        private string marketVolumeNo;
        private Types.MatchCenterState matchState = Types.MatchCenterState.First;

        /// <summary>
        /// 撮合状态
        /// </summary>
        [DataMember]
        public Types.MatchCenterState MatchState
        {
            get { return matchState; }
            set { matchState = value; }
        }

        #region  港股股东代码 SholderCode
        /// <summary>
        /// 港股股东代码
        /// </summary>
        [DataMember]
        public string SholderCode
        {
            get { return _sholderCode; }
            set { _sholderCode = value; }
        }
        #endregion

        /// <summary>
        /// 委托单标识
        /// </summary>
        [DataMember]
        public string OrderNo
        {
            set { _orderno = value; }
            get { return _orderno; }
        }
        /// <summary>
        /// 柜台标识
        /// </summary>
        [DataMember]
        public string BranchID
        {
            set { _branchid = value; }
            get { return _branchid; }
        }
        /// <summary>
        /// 委托港股代码
        /// </summary>
        [DataMember]
        public string HKSecuritiesCode
        {
            set { _hksecuritiescode = value; }
            get { return _hksecuritiescode; }
        }
        /// <summary>
        /// 原始委托量
        /// </summary>
        [DataMember]
        public decimal OldVolume
        {
            set { _oldvolume = value; }
            get { return _oldvolume; }
        }
        /// <summary>
        /// 接收委托标识
        /// </summary>
        [DataMember]
        public DateTime ReceiveTime
        {
            set { _receivetime = value; }
            get { return _receivetime; }
        }
        /// <summary>
        /// 委托价格
        /// </summary>
        [DataMember]
        public decimal OrderPrice
        {
            set { _orderprice = value; }
            get { return _orderprice; }
        }
        /// <summary>
        /// 委托量
        /// </summary>
        [DataMember]
        public decimal OrderVolume
        {
            set { _ordervolume = value; }
            get { return _ordervolume; }
        }
        /// <summary>
        /// 委托类型(LO = 1,-限价盘，ELO = 2-增强限价盘), SLO = 3-特别限价盘
        /// </summary>
        [DataMember]
        public int OrderType
        {
            set { _ordertype = value; }
            get { return _ordertype; }
        }
        /// <summary>
        /// 委托卖买方向（1,买，2卖)
        /// </summary>
        [DataMember]
        public int TradeType
        {
            set { _tradetype = value; }
            get { return _tradetype; }
        }
        /// <summary>
        /// 委托单信息
        /// </summary>
        [DataMember]
        public string OrderMessage
        {
            set { _ordermessage = value; }
            get { return _ordermessage; }
        }

        /// <summary>
        /// 市场存量关联委托单编号
        /// </summary>
        [DataMember]
        public string MarketVolumeNo
        {
            get
            {
                return marketVolumeNo;
            }
            set
            {
                marketVolumeNo = value;
            }
        }
        /// <summary>
        /// 市场存量
        /// </summary>
        [DataMember]
        public decimal MarkLeft
        {
            get
            {
                return markLeft;
            }
            set
            {
                markLeft = value;
            }
        }
        #endregion Model

    }
}
