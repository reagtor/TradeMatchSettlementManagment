using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ManagementCenter.Model.XH
{
    /// <summary>
    ///描述：交易规则_明细(包含XH_SpotHighLowControlType,XH_SpotHighLowValue,XH_ValidDeclareType,XH_ValidDeclareValue的字段) 实体类XHSpotTradeRulesDetail 。
    ///作者：刘书伟
    ///日期:2009-03-12
    /// </summary>
    [DataContract]
    public class XHSpotTradeRulesDetail
    {
        #region Model

        //涨跌幅类型及涨跌幅取值字段
        private int _highlowtypeid;
        private int _breedclasshighlowid;
        private decimal? _fundYestclosepricescale;
        private decimal? _righthighlowscale;
        private decimal? _normalvalue;
        private decimal? _stvalue;
        //有效申报类型及有效申报取字段
        private int _breedClassValidID;
        private int _validDeclareTypeID;
        private decimal? _upperlimit;
        private decimal? _lowerlimit;
        private int? _ismarketnewday;

        /// <summary>
        /// 涨跌幅类型标识
        /// </summary>
        [DataMember]
        public int HighLowTypeID
        {
            set { _highlowtypeid = value; }
            get { return _highlowtypeid; }
        }


        /// <summary>
        /// 基金昨日收盘价的上下百分比例
        /// </summary>
        [DataMember]
        public decimal? FundYestClosePriceScale
        {
            set { _fundYestclosepricescale = value; }
            get { return _fundYestclosepricescale; }
        }

        /// <summary>
        /// 权证涨跌幅比例
        /// </summary>
        [DataMember]
        public decimal? RightHighLowScale
        {
            set { _righthighlowscale = value; }
            get { return _righthighlowscale; }
        }

        /// <summary>
        /// 正常企业
        /// </summary>
        [DataMember]
        public decimal? NormalValue
        {
            set { _normalvalue = value; }
            get { return _normalvalue; }
        }

        /// <summary>
        /// ST企业
        /// </summary>
        [DataMember]
        public decimal? StValue
        {
            set { _stvalue = value; }
            get { return _stvalue; }
        }

        /// <summary>
        /// 品种涨跌幅标识
        /// </summary>
        [DataMember]
        public int BreedClassHighLowID
        {
            set { _breedclasshighlowid = value; }
            get { return _breedclasshighlowid; }
        }


        /// <summary>
        /// 有效申报类型标识
        /// </summary>
        [DataMember]
        public int ValidDeclareTypeID
        {
            set { _validDeclareTypeID = value; }
            get { return _validDeclareTypeID; }
        }

        /// <summary>
        /// 品种有效申报标识
        /// </summary>
        [DataMember]
        public int BreedClassValidID
        {
            set { _breedClassValidID = value; }
            get { return _breedClassValidID; }
        }


        /// <summary>
        /// 上限
        /// </summary>
        [DataMember]
        public decimal? UpperLimit
        {
            set { _upperlimit = value; }
            get { return _upperlimit; }
        }

        /// <summary>
        /// 下限
        /// </summary>
        [DataMember]
        public decimal? LowerLimit
        {
            set { _lowerlimit = value; }
            get { return _lowerlimit; }
        }

        /// <summary>
        /// 是否是上市首日
        /// </summary>
        [DataMember]
        public int? IsMarketNewDay
        {
            set { _ismarketnewday = value; }
            get { return _ismarketnewday; }
        }

        #endregion Model
    }
}