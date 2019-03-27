using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ManagementCenter.Model.UserManage
{
    ///<summary> 
    /// 模块编号：
    /// 作用：个性化资金实体类
    /// 作者：叶振东
    /// 编写日期：2009-12-23
    /// 描述.:个性个设置资金类型中添加了单独针对银行的类型：1-银行。
    ///       将TradeID、PersonalType、SetCurrencyType修改为DataMember.
    /// </summary>
    [DataContract]
    public class PersonalizationCapital
    {
        /// <summary>
        /// 要个性化资金设置的交易员ID列表
        /// </summary>
        [DataMember]
        public List<string> TradeID { get; set; }

        /// <summary>
        /// 人民币的金额
        /// </summary>
        [DataMember]
        public Decimal RMBAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 港币的金额
        /// </summary>
        [DataMember]
        public Decimal HKAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 美元的金额
        /// </summary>
        [DataMember]
        public Decimal USAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 个性个设置资金类型(0-是所有包括银行、现货、期货、港股资金，1-银行，2-现货，3-期货，4-港股，5-银行资金外的所有）
        /// </summary>
        [DataMember]
        public int PersonalType { get; set; }

        /// <summary>
        /// 所有-0,人民币-1, 港币-2, 美元-3
        /// </summary>
        [DataMember]
        public int SetCurrencyType { get; set; }
    }
}