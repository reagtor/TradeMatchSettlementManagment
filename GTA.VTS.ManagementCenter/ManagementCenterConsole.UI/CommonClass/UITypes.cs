using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagementCenterConsole.UI.CommonClass
{
    /// <summary>
    /// 类型枚举类
    /// 作者：程序员;熊晓凌
    /// 日期：2008-11-19
    /// </summary>
    public class UITypes
    {
        ///// <summary>
        ///// 角色类型
        ///// </summary>
        //public enum RoleTypeEnum
        //{ 
        //    /// <summary>
        //    /// 超级管理员
        //    /// </summary>
        //    Admin=1,
        //    /// <summary>
        //    /// 管理员
        //    /// </summary>
        //    Manager=2,
        //    /// <summary>
        //    /// 交易员
        //    /// </summary>
        //    Transaction=3
        //}

        ///// <summary>
        ///// 证件类型
        ///// </summary>
        //public enum CertificateStyleEnum
        //{
        //    /// <summary>
        //    /// 身份证
        //    /// </summary>
        //    StatusCard=1,
        //    /// <summary>
        //    /// 学生证
        //    /// </summary>
        //    StudentCard=2,
        //    /// <summary>
        //    /// 军官证
        //    /// </summary>
        //    ServicemanCard=3,
        //    /// <summary>
        //    /// 护照
        //    /// </summary>
        //    Passport=4
        //}

        #region 操作窗体类型枚举

        /// <summary>
        /// 操作窗体类型枚举
        /// </summary>
        public enum EditTypeEnum
        {
            /// <summary>
            /// 显示添加UI
            /// </summary>
            AddUI = 1,
            /// <summary>
            /// 显示修改UI
            /// </summary>
            UpdateUI = 2
        }

        #endregion

        #region 范围值窗体类型
        /// <summary>
        /// 范围值窗体类型
        /// </summary>
        public enum UITypeEnum
        {
            /// <summary>
            /// 现货交易规则_最小变动价位_范围_值UI
            /// </summary>
            XHMinChangePriceValueFieldRangeUI=1,
            /// <summary>
            /// 现货交易费用中的手续费范围值UI
            /// </summary>
            XHSpotRangeCostPoundageFieldRangeUI=2

        }
        #endregion

    }
}