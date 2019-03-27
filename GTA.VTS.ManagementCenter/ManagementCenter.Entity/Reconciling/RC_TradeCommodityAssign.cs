using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
	/// 描述：撮合机代码分配表 实体类RC_TradeCommodityAssign 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：熊晓凌 修改：刘书伟
    /// 日期：2008-11-18 修改日期：2009-10-22
    /// </summary>
    [DataContract]
	public class RC_TradeCommodityAssign
	{
		public RC_TradeCommodityAssign()
		{}
		#region Model
		private string _commoditycode;
		private int _matchmachineid;
        private int  codeformsource;

		/// <summary>
		/// 商品代码
		/// </summary>
        [DataMember]
		public string CommodityCode
		{
			set{ _commoditycode=value;}
			get{return _commoditycode;}
		}
		/// <summary>
		/// 撮合机编号
		/// </summary>
        [DataMember]
		public int MatchMachineID
		{
			set{ _matchmachineid=value;}
			get{return _matchmachineid;}
        }

        #region 2009.10.22 新增字段

        #endregion
        /// <summary>
        /// 代码来源那个表
        /// </summary>
        [DataMember]
        public int CodeFormSource
        {
            set { codeformsource = value; }
            get { return codeformsource; }
        }
        #endregion Model

    }
}

