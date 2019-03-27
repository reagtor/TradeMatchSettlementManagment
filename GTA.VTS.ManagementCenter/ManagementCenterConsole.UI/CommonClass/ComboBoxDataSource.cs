using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagementCenter.BLL;
using ManagementCenter.Model;
using ManagementCenter.Model.CommonClass;
using Types = GTA.VTS.Common.CommonObject.Types;

namespace ManagementCenterConsole.UI.CommonClass
{
    /// <summary>
    /// 描述：界面相关类型绑定
    /// 作者：熊晓凌   修改:刘书伟
    /// 日期：2008-11-19 修改日期:2009-11-03
    /// </summary>
    public class ComboBoxDataSource
    {
        /// <summary>
        /// 获取柜台UComboItem集合
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetAllCounterList()
        {
            UComboItem item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            try
            {
                CT_CounterBLL CounterBLL = new CT_CounterBLL();
                List<CT_Counter> l = CounterBLL.GetListArray(string.Empty);
                if (l == null) return null;
                foreach (CT_Counter ct in l)
                {
                    item = new UComboItem(ct.name, ct.CouterID);
                    listUComboItem.Add(item);
                }
            }
            catch
            {
                return null;
            }
            return listUComboItem;
        }

        /// <summary>
        /// 获取证件类型
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetCertificateStyleList()
        {
            UComboItem item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            item = new UComboItem("身份证", (int)ManagementCenter.Model.CommonClass.Types.CertificateStyleEnum.StatusCard);
            listUComboItem.Add(item);
            item = new UComboItem("学生证", (int)ManagementCenter.Model.CommonClass.Types.CertificateStyleEnum.StudentCard);
            listUComboItem.Add(item);
            item = new UComboItem("军官证",
                                  (int)ManagementCenter.Model.CommonClass.Types.CertificateStyleEnum.ServicemanCard);
            listUComboItem.Add(item);
            item = new UComboItem("护照", (int)ManagementCenter.Model.CommonClass.Types.CertificateStyleEnum.Passport);
            listUComboItem.Add(item);
            return listUComboItem;
        }

        /// <summary>
        /// 获取权限UComboItem集合
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetAllRightGroupList()
        {
            UComboItem item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            try
            {
                UM_ManagerGroupBLL ManagerGroupBLL = new UM_ManagerGroupBLL();
                List<UM_ManagerGroup> L_ManagerGroup = ManagerGroupBLL.GetListArray(string.Empty);
                if (L_ManagerGroup == null) return null;
                foreach (UM_ManagerGroup managergroup in L_ManagerGroup)
                {
                    item = new UComboItem(managergroup.ManagerGroupName, managergroup.ManagerGroupID);
                    listUComboItem.Add(item);
                }
            }
            catch
            {
                return null;
            }
            return listUComboItem;
        }

        /// <summary>
        /// 获取提示问题类型UComboItem集合
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetQuestionTypeList()
        {
            UComboItem item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            try
            {
                UM_QuestionTypeBLL QuestionTypeBLL = new UM_QuestionTypeBLL();
                List<UM_QuestionType> L_QuestionType = QuestionTypeBLL.GetListArray(string.Empty);
                if (L_QuestionType == null) return null;
                foreach (UM_QuestionType questiontype in L_QuestionType)
                {
                    item = new UComboItem(questiontype.Content, questiontype.QuestionID);
                    listUComboItem.Add(item);
                }
            }
            catch
            {
                return null;
            }
            return listUComboItem;
        }

        /// <summary>
        /// 获取交易所类型UComboItem集合
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBourseTypeList()
        {
            UComboItem item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            try
            {
                ManagementCenter.BLL.CM_BourseTypeBLL BourseTypeBLL = new CM_BourseTypeBLL();
                string strWhere = "  DELETESTATE IS NOT NULL AND DELETESTATE<>1 ";

                List<CM_BourseType> l = BourseTypeBLL.GetListArray(strWhere);//(string.Empty);
                if (l == null) return null;
                foreach (CM_BourseType BourseType in l)
                {
                    item = new UComboItem(BourseType.BourseTypeName, BourseType.BourseTypeID);
                    listUComboItem.Add(item);
                }
            }
            catch
            {
                return null;
            }
            return listUComboItem;
        }

        /// <summary>
        /// 获取撮合中心UComboItem集合
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetMatchCenterList()
        {
            UComboItem item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            try
            {
                ManagementCenter.BLL.RC_MatchCenterBLL MatchCenterBLL = new RC_MatchCenterBLL();
                List<RC_MatchCenter> l = MatchCenterBLL.GetListArray(string.Empty);
                if (l == null) return null;
                foreach (RC_MatchCenter MatchCenter in l)
                {
                    item = new UComboItem(MatchCenter.MatchCenterName, MatchCenter.MatchCenterID);
                    listUComboItem.Add(item);
                }
            }
            catch
            {
                return null;
            }
            return listUComboItem;
        }

        /// <summary>
        /// 获取帐号类型列表
        /// </summary>
        /// <param name="i">1：为资金帐号 2：持仓帐号</param>
        /// <returns></returns>
        public static List<UComboItem> GetAccountTypeList(int i)
        {
            UComboItem item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            try
            {
                ManagementCenter.BLL.UM_AccountTypeBLL AccountTypeBLL = new UM_AccountTypeBLL();
                List<UM_AccountType> l = AccountTypeBLL.GetListArray(string.Empty);
                if (l == null) return null;
                foreach (UM_AccountType AccountType in l)
                {
                    if (i == 1)
                    {
                        if ((int)AccountType.AccountAttributionType ==
                            (int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.SpotCapital ||
                            (int)AccountType.AccountAttributionType ==
                            (int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.FuturesCapital)
                        {
                            item = new UComboItem(AccountType.AccountName, AccountType.AccountTypeID);
                            listUComboItem.Add(item);
                        }

                    }
                    else if (i == 2)
                    {
                        if ((int)AccountType.AccountAttributionType ==
                            (int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.FuturesHold ||
                            (int)AccountType.AccountAttributionType ==
                            (int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.SpotHold)
                        {
                            item = new UComboItem(AccountType.AccountName, AccountType.AccountTypeID);
                            listUComboItem.Add(item);
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
            return listUComboItem;
        }
        #region 新添加的方法 作者:刘书伟 日期:2009-11-03

        #region 获取所有帐号类型列表
        /// <summary>
        /// 获取所有帐号类型列表
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetAccountTypeList()
        {
            UComboItem item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            try
            {
                ManagementCenter.BLL.UM_AccountTypeBLL AccountTypeBLL = new UM_AccountTypeBLL();
                List<UM_AccountType> l = AccountTypeBLL.GetListArray(string.Empty);
                if (l == null) return null;
                foreach (UM_AccountType accountType in l)
                {
                    if ((int)accountType.AccountAttributionType ==
                            (int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.SpotCapital ||
                            (int)accountType.AccountAttributionType ==
                            (int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.FuturesCapital ||
                             (int)accountType.AccountAttributionType ==
                            (int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.BankAccount)
                    {
                        item = new UComboItem(accountType.AccountName, accountType.AccountTypeID);
                        listUComboItem.Add(item);
                    }
                }
            }
            catch
            {
                return null;
            }
            return listUComboItem;
        }
        #endregion

        #region 根据账户类型ID,返回不包含此账户类型ID的资金账户
        /// <summary>
        /// 根据账户类型ID,返回不包含此账户类型ID的资金账户
        /// </summary>
        /// <param name="AccountTypeID">账户类型ID</param>
        /// <returns></returns>
        public static List<UComboItem> GetAccountTypeListByAccountTypeID(int AccountTypeID)
        {
            UComboItem item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            try
            {
                ManagementCenter.BLL.UM_AccountTypeBLL accountTypeBLL = new UM_AccountTypeBLL();
                List<UM_AccountType> l = accountTypeBLL.GetListArray(string.Format("AccountTypeID<>{0}", AccountTypeID));
                if (l == null)
                {
                    return null;
                }
                foreach (UM_AccountType accountType in l)
                {
                    if ((int)accountType.AccountAttributionType ==
                        (int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.SpotCapital ||
                        (int)accountType.AccountAttributionType ==
                        (int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.FuturesCapital ||
                             (int)accountType.AccountAttributionType ==
                            (int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.BankAccount)
                    {
                        item = new UComboItem(accountType.AccountName, accountType.AccountTypeID);
                        listUComboItem.Add(item);
                    }

                }
            }
            catch
            {
                return null;
            }
            return listUComboItem;
        }
        #endregion

        #region 获取币种类型列表
        /// <summary>
        /// 获取币种类型列表
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetCurrencyTypeList()
        {
            UComboItem item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            try
            {
                CM_CurrencyTypeBLL cM_CurrencyTypeBLL = new CM_CurrencyTypeBLL();
                List<CM_CurrencyType> l = cM_CurrencyTypeBLL.GetListArray(string.Empty);
                if (l == null)
                {
                    return null;
                }
                foreach (CM_CurrencyType _cMCurrencyType in l)
                {
                    item = new UComboItem(_cMCurrencyType.CurrencyName, _cMCurrencyType.CurrencyTypeID);
                    listUComboItem.Add(item);
                }
            }
            catch
            {
                return null;
            }
            return listUComboItem;
        }
        #endregion

        #endregion
    }
}