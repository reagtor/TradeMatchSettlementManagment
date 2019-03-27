using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagementCenter.Model.CommonClass
{
    /// <summary>
    /// 各种操作类型描述
    /// 作者：程序员：熊晓凌 修改：刘书伟
    /// 日期：2008-11-18    2009-10-23
    /// </summary>
    public class ButtonFunctionTypes
    {
        /// <summary>
        /// 按纽类型
        /// </summary>
        public enum TransactionLeftControlType
        {
            /// <summary>
            /// 追加资金管理
            /// </summary>
            AddFundManage,
            /// <summary>
            /// 交易员开销户管理
            /// </summary>
            AccountManage,
            /// <summary>
            /// 银行帐号冻结/解冻管理
            /// </summary>
            FreezeManage,

            /// <summary>
            /// 撮合中心管理
            /// </summary>
            CenterManage,

            /// <summary>
            /// 撮合机管理
            /// </summary>
            MachineManage,

            /// <summary>
            /// 转账管理
            /// </summary>
            TransferManageUI
        }

    }
}
