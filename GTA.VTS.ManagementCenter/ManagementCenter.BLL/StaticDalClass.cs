using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagementCenter.DAL;

namespace ManagementCenter.BLL
{
    /// <summary>
    /// 描述：用户帐户信息静态实例管理类
    /// 作者：熊晓凌
    /// 日期：2008-11-20
    /// </summary>
    public class StaticDalClass
    {
        /// <summary>
        /// 用户管理
        /// </summary>
        private static UM_UserInfoDAL userInfoDAL = null;
        /// <summary>
        /// 用户管理
        /// </summary>
        public static UM_UserInfoDAL UserInfoDAL
        {
            get
            {
                if (userInfoDAL == null) userInfoDAL = new UM_UserInfoDAL();
                return userInfoDAL;
            }
        }

        /// <summary>
        /// 品种权限分配
        /// </summary>
        private static UM_DealerTradeBreedClassDAL dealerTradeBreedClassDAL = null;
        /// <summary>
        /// 品种权限分配
        /// </summary>
        public static UM_DealerTradeBreedClassDAL DealerTradeBreedClassDAL
        {
            get
            {
                if (dealerTradeBreedClassDAL == null) dealerTradeBreedClassDAL = new UM_DealerTradeBreedClassDAL();
                return dealerTradeBreedClassDAL;
            }
        }

        /// <summary>
        /// 品种维护
        /// </summary>
        private static CM_BreedClassDAL breedClassDAL = null;
        /// <summary>
        /// 品种维护
        /// </summary>
        public static CM_BreedClassDAL BreedClassDAL
        {
            get
            {
                if (breedClassDAL == null) 
                    breedClassDAL = new CM_BreedClassDAL();
                return breedClassDAL;
            }
        }

        /// <summary>
        /// 帐号类型
        /// </summary>
        private static UM_AccountTypeDAL accountTypeDAL = null;
        /// <summary>
        /// 帐号类型
        /// </summary>
        public static UM_AccountTypeDAL AccountTypeDAL
        {
            get
            {
                if (accountTypeDAL == null) accountTypeDAL = new UM_AccountTypeDAL();
                return accountTypeDAL;
            }
        }

        /// <summary>
        /// 帐号分配
        /// </summary>
        private static UM_DealerAccountDAL dealerAccountDAL = null;
        /// <summary>
        /// 帐号分配
        /// </summary>
        public static UM_DealerAccountDAL DealerAccountDAL
        {
            get
            {
                if (dealerAccountDAL == null) dealerAccountDAL = new UM_DealerAccountDAL();
                return dealerAccountDAL;
            }
        }

        /// <summary>
        /// 柜台列表
        /// </summary>
        private static CT_CounterDAL counterDAL = null;
        /// <summary>
        /// 柜台列表
        /// </summary>
        public static CT_CounterDAL CounterDAL
        {
            get
            {
                if (counterDAL == null) counterDAL = new CT_CounterDAL();
                return counterDAL;
            }
        }

        /// <summary>
        /// 初始资金
        /// </summary>
        private static UM_OriginationFundDAL originationFundDAL = null;
        /// <summary>
        /// 初始资金
        /// </summary>
        public static UM_OriginationFundDAL OriginationFundDAL
        {
            get
            {
                if (originationFundDAL == null) originationFundDAL = new UM_OriginationFundDAL();
                return originationFundDAL;
            }
        }

        /// <summary>
        /// 冻结
        /// </summary>
        private static UM_FreezeReasonDAL freezeReasonDAL = null;
        /// <summary>
        ///冻结 
        /// </summary>
        public static UM_FreezeReasonDAL FreezeReasonDAL
        {
            get
            {
                if (freezeReasonDAL == null) freezeReasonDAL = new UM_FreezeReasonDAL();
                return freezeReasonDAL;
            }
        }

        /// <summary>
        /// 解冻
        /// </summary>
        private static UM_ThawReasonDAL thawReasonDAL = null;
        /// <summary>
        /// 解冻
        /// </summary>
        public static UM_ThawReasonDAL ThawReasonDAL
        {
            get
            {
                if (thawReasonDAL == null) thawReasonDAL = new UM_ThawReasonDAL();
                return thawReasonDAL;
            }
        }

        /// <summary>
        /// 追加资金
        /// </summary>
        private static UM_FundAddInfoDAL fundAddInfoDAL = null;
        /// <summary>
        /// 追加资金
        /// </summary>
        public static UM_FundAddInfoDAL FundAddInfoDAL
        {
            get
            {
                if (fundAddInfoDAL == null) fundAddInfoDAL = new UM_FundAddInfoDAL();
                return fundAddInfoDAL;
            }
        }
    }
}
