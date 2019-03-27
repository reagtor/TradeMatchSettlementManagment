using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ManagementCenter.Model.CommonClass
{
    /// <summary>
    /// 各种银行帐号类型描述
    /// 作者：程序员：熊晓凌
    /// 日期：2008-11-19
    /// </summary>
    public class Types
    {
        /// <summary>
        /// 9种帐户类型描述
        /// </summary>
        public enum AccountTypeEnum
        {
            /// <summary>
            /// 银行帐号
            /// </summary>
            BankAccount = 1,

            /// <summary>
            /// 证券资金帐号
            /// </summary>
            NegotiableSecuritiesAccount = 2,

            /// <summary>
            /// 证券股东代码
            /// </summary>
            NegotiableSecuritiesCode = 3,

            /// <summary>
            /// 商品期货资金帐号
            /// </summary>
            CommodityFutureAccount = 4,

            /// <summary>
            /// 商品期货交易编码
            /// </summary>
            CommodityFutureCode = 5,

            /// <summary>
            /// 股指期货资金帐号
            /// </summary>
            StockIndexAccount = 6,

            /// <summary>
            /// 股指期货交易编码
            /// </summary>
            StockIndexCode = 7,

            /// <summary>
            /// 港股资金帐户
            /// </summary>
            HongKongStockAccount = 8,

            /// <summary>
            /// 港股股东代码
            /// </summary>
            HongKongStockCode = 9
        }

        /// <summary>
        /// 角色类型
        /// </summary>
        public enum RoleTypeEnum
        {
            /// <summary>
            /// 超级管理员
            /// </summary>
            Admin = 1,
            /// <summary>
            /// 管理员
            /// </summary>
            Manager = 2,
            /// <summary>
            /// 交易员
            /// </summary>
            Transaction = 3
        }

        /// <summary>
        /// 证件类型
        /// </summary>
        public enum CertificateStyleEnum
        {
            /// <summary>
            /// 身份证
            /// </summary>
            StatusCard = 1,
            /// <summary>
            /// 学生证
            /// </summary>
            StudentCard = 2,
            /// <summary>
            /// 军官证
            /// </summary>
            ServicemanCard = 3,
            /// <summary>
            /// 护照
            /// </summary>
            Passport = 4
        }

        /// <summary>
        /// 冻结类型
        /// </summary>
        public enum IsAutoUnFreezeEnum
        {
            /// <summary>
            /// 自动
            /// </summary>
            Auto = 1,
            /// <summary>
            /// 非自动
            /// </summary>
            UnAuto = 0
        }

        /// <summary>
        /// 添加的管理员类型
        /// </summary>
        public enum AddTpyeEnum
        {
            /// <summary>
            /// 后台交易员
            /// </summary>
            BackTaransaction = 1,
            /// <summary>
            /// 前台交易员
            /// </summary>
            FrontTaransaction = 2,
            /// <summary>
            /// 后台管理员
            /// </summary>
            BackManager = 3,
            /// <summary>
            /// 前台管理员
            /// </summary>
            FrontManager = 4,

            /// <summary>
            /// 标志前台交易员删除状态
            /// </summary>
            FrontTarnDelState = 5

        }

        /// <summary>
        /// 连接状态
        /// </summary>
        public enum StateEnum
        {
            /// <summary>
            /// 连接成功
            /// </summary>
            ConnSuccess = 1,
            /// <summary>
            /// 连接失败
            /// </summary>
            ConnDefeat = 0
        }

        /// <summary>
        /// 控制台菜单项描述
        /// </summary>
        public enum MenuTypeEnum
        {
            /// <summary>
            /// 交易员管理
            /// </summary>
            AccountM_TransactionM = 1,
            /// <summary>
            /// 管理员管理
            /// </summary>
            AccountM_ManagerM = 2,
            /// <summary>
            /// 权限组管理
            /// </summary>
            AccountM_RightM = 3,
            /// <summary>
            /// 撮合中心配置
            /// </summary>
            MatchM_CenterM = 4,
            /// <summary>
            /// 简单配置向导
            /// </summary>
            MatchM_GuideM = 5,
            /// <summary>
            /// 清算柜台管理
            /// </summary>
            CounterM_ConfigM = 6,
            /// <summary>
            /// 品种管理
            /// </summary>
            MenuCommParaM_BreedClassM = 7,
            /// <summary>
            /// 代码管理
            /// </summary>
            MenuCommParaM_CommodityM = 8,
            /// <summary>
            /// 交易所管理
            /// </summary>
            MenuCommParaM_BourseM = 9,
            /// <summary>
            /// 结算价管理（期货）
            /// </summary>
            MenuFuturesM_TodaySettlementPriceM = 10,
            /// <summary>
            /// 现货管理
            /// </summary>
            MenuSpotM = 11,
            /// <summary>
            /// 港股管理
            /// </summary>
            MenuHKM = 12,
            /// <summary>
            /// 期货管理
            /// </summary>
            MenuFuturesM = 13

        }

        #region IsYesOrNo enum

        /// <summary>
        /// 判断是否(判断期货代码是否过期时用)
        /// </summary>
        public enum IsYesOrNo
        {
            /// <summary>
            /// 是
            /// </summary>
            Yes = 1,
            /// <summary>
            /// 否
            /// </summary>
            No = 2
        }

        #endregion
    }
    [DataContract]
    public class InitFund
    {
        //public int UserID = int.MaxValue;
        /// <summary>
        /// 人民币
        /// </summary>
        [DataMember]
        public decimal RMB = decimal.MaxValue;
        /// <summary>
        /// 美元
        /// </summary>
        [DataMember]
        public decimal US = decimal.MaxValue;
        /// <summary>
        /// 港币
        /// </summary>
        [DataMember]
        public decimal HK = decimal.MaxValue;
    }


}
