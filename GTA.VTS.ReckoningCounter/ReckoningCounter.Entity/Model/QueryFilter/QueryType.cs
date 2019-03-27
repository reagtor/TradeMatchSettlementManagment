using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTA.VTS.Common.CommonObject;
using System.Runtime.Serialization;

namespace ReckoningCounter.Entity.Model.QueryFilter
{
    /// <summary>
    /// Title: 数据查询过滤条件枚举类型
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    public class QueryType
    {
        #region 1.货币查询类型 public enum QueryCurrencyType
        /// <summary>
        /// 1.货币查询类型
        /// </summary>
        [DataContract]
        public enum QueryCurrencyType : int
        {
            /// <summary>
            /// 所有
            /// </summary>
            [EnumMember]
            ALL = 0,
            /// <summary>
            /// 人民币
            /// </summary>
            [EnumMember]
            RMB = 1,
            /// <summary>
            /// 港币
            /// </summary>
            [EnumMember]
            HK = 2,
            /// <summary>
            /// 美元
            /// </summary>
            [EnumMember]
            US = 3

        }
        #endregion

        #region 2.冻结类型 public enum QueryFreezeType
        /// <summary>
        /// 2.冻结类型
        /// </summary>
        [DataContract]
        public enum QueryFreezeType : int
        {
            /// <summary>
            /// 默认查询所有
            /// </summary>
            [EnumMember]
            ALL = 0,
            /// <summary>
            /// 委托冻结
            /// </summary>
            [EnumMember]
            DelegateFreeze = 1,

            /// <summary>
            /// 清算冻结
            /// </summary>
            [EnumMember]
            ReckoningFreeze = 2,

            /// <summary>
            /// 今日开仓平仓冻结
            /// </summary>
            [EnumMember]
            TodayHoldingCloseFreeze = 3,

            /// <summary>
            /// 历史开仓平仓冻结
            /// </summary>
            [EnumMember]
            HistoryHoldingCloseFreeze = 4,
        }
        #endregion

        #region 3.此枚举设置为内部查询自行使用，为了方便管理 public enum QueryWhereType
        /// <summary>
        /// 3.此枚举设置为内部查询自行使用，为了方便管理
        /// </summary>
        public enum QueryWhereType : int
        {
            /// <summary>
            /// 默认
            /// </summary>
            Default = 0,
            /// <summary>
            /// 根据用户ID
            /// </summary>
            ByUserID = 1,
            /// <summary>
            /// 根据用户ID，密码
            /// </summary>
            ByUserAndPwd = 2,
            /// <summary>
            /// 根据账号
            /// </summary>
            ByAccount = 3


        }
        #endregion
    }
}
