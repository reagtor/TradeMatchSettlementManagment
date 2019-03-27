using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.Entity;
using ReckoningCounter.Entity.AccountManagementAndFindEntity;
using ReckoningCounter.Model;
using ReckoningCounter.DAL.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.DAL.Data.HK;
using ReckoningCounter.Entity.Model.HK;

namespace ReckoningCounter.DAL.AccountManagementAndFindDAL
{
    /// <summary>
    /// 账户管理
    /// 主要：包括用户开户基本信息添加，交易员账号开户添加相关账户列表，初始化相关资金账户银行账户等表的信息
    ///       追加资金
    /// 作者：熊晓凌
    /// 日期：2009-2-18
    /// Update BY:李健华
    /// Update Date:2009-07-20
    /// Desc.:修改之前的数据DAL操作以及相关逻辑
    /// </summary>
    public class TraderAccountManagement
    {
        #region 添加交易员基本信息表

        /// <summary>
        /// 添加交易员基本信息表
        /// </summary>
        /// <param name="account">账户实体对象</param>
        /// <param name="outMessage">输出信息</param>
        /// <param name="db">数据操作基类对象</param>
        /// <param name="tm">操作开启事务，如果为null即不开启</param>
        public bool AddUserToUserBasicInformation(AccountEntity account, out string outMessage, Database db, DbTransaction tm)
        {
            outMessage = string.Empty;
            if (account == null)
            {
                outMessage = "帐户对象为空！";
                return false;
            }
            try
            {
                //判断该交易员ID是否已经存在
                //var result = DataRepository.UaUserBasicInformationTableProvider.Find(tm, string.Format("UserID='{0}'", account.TraderID));
                UA_UserBasicInformationTableDal dal = new UA_UserBasicInformationTableDal();

                // if (result == null || result.Count > 0) 
                if (dal.Exists(account.TraderID))
                {
                    outMessage = "该交易员ID已经存在！";
                    return false;
                }
                //插入交易员信息到“用户基本信息表”中
                //var _UserBasicInformation = new SqlUaUserBasicInformationTableProvider(TransactionFactory.RC_ConnectionString, true, "");
                UA_UserBasicInformationTableInfo model = new UA_UserBasicInformationTableInfo();
                model.UserID = account.TraderID;
                model.Password = account.TraderPassWord;
                model.RoleNumber = account.RoleNumber;
                dal.Add(model, db, tm);
                return true;
            }
            catch (Exception ex)
            {
                outMessage = ex.Message.ToString();
                LogHelper.WriteError(outMessage, ex);
                return false;
            }
        }

        #endregion

        #region  添加交易员帐户列表

        /// <summary>
        /// 写交易员帐户列表
        /// </summary>
        /// <param name="account">账户实体对象</param>
        /// <param name="outMessage">输出信息</param>
        /// <param name="db"></param>
        /// <param name="tm">事务对象，如果为null不开启事务</param>
        public bool AddTraderAccountToUserAccountAllocation(AccountEntity account, out string outMessage,
                                                            Database db, DbTransaction tm)
        {
            outMessage = string.Empty;
            if (account == null)
            {
                outMessage = "帐户对象为空！";
                return false;
            }
            try
            {
                //先判断该交易账号是否已经存在
                //var result = DataRepository.UaUserAccountAllocationTableProvider.Find(tm, string.Format("UserAccountDistributeLogo='{0}'", account.Account));
                UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();

                //if (result == null || result.Count > 0)
                if (dal.Exists(account.Account))
                {
                    outMessage = "交易账号已经存在！" + account.Account;
                    return false;
                }
                //var _UserAccountAllocation = new SqlUaUserAccountAllocationTableProvider(TransactionFactory.RC_ConnectionString, true, "");
                UA_UserAccountAllocationTableInfo model = new UA_UserAccountAllocationTableInfo();
                model.UserAccountDistributeLogo = account.Account;
                model.UserID = account.TraderID;
                model.WhetherAvailable = true;
                model.AccountTypeLogo = Convert.ToInt32(account.AccountType);
                dal.Add(model, db, tm);
                return true;
            }
            catch (Exception ex)
            {
                outMessage = ex.Message.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
                return false;
            }
        }

        #endregion

        #region  初始化各资金帐户入口（开户）

        /// <summary>
        /// 初试化各资金帐户
        /// </summary>
        /// <param name="account">帐户实体</param>
        /// <param name="outMessage"></param>
        /// <param name="db">操作数据句柄</param>
        /// <param name="tm">操作开启事务，如果为null即不开启</param>
        /// <returns></returns>
        public bool AccountInformationInsertToRelevantAccount(AccountEntity account, out string outMessage,
                                                               Database db, DbTransaction tm)
        {
            outMessage = string.Empty;
            try
            {
                #region Old 因港股不也现货账户一起所以不能这样去判断初始化
                ////账号所属类型:1 为现货资金账号，2为现货持仓账号，3为期货资金账号，4为期货持仓账号，5为银行账号
                //int AccountAttribution = Convert.ToInt32(account.AccountAAttribution);
                //switch (AccountAttribution)
                //{
                //    case (int)CommonObject.Types.AccountAttributionType.SpotCapital:
                //        //初始化现货资金账户
                //        InitializationXHCapitalAccount(account, out outMessage, db, tm);
                //        break;

                //    case (int)CommonObject.Types.AccountAttributionType.SpotHold:
                //        break;

                //    case (int)CommonObject.Types.AccountAttributionType.FuturesCapital:
                //        //初始化期货资金账户
                //        InitializationQHCapitalAccount(account, out outMessage, db, tm);
                //        break;

                //    case (int)CommonObject.Types.AccountAttributionType.FuturesHold:
                //        break;

                //    case (int)CommonObject.Types.AccountAttributionType.BankAccount:
                //        //初始化银行资金账户
                //        InitializationBankCapitalAccount(account, out outMessage, db, tm);
                //        break;
                //}
                #endregion

                switch ((Types.AccountType)account.AccountType)
                {
                    case Types.AccountType.BankAccount:
                        //初始化银行资金账户
                        InitializationBankCapitalAccount(account, out outMessage, db, tm);
                        break;
                    case Types.AccountType.StockSpotCapital:
                        //初始化现货资金账户
                        InitializationXHCapitalAccount(account, out outMessage, db, tm);
                        break;
                    case Types.AccountType.StockSpotHoldCode:
                        break;
                    case Types.AccountType.CommodityFuturesCapital:
                        //初始化期货资金账户(商品期货）
                        InitializationQHCapitalAccount(account, out outMessage, db, tm);
                        break;
                    case Types.AccountType.CommodityFuturesHoldCode:
                        break;
                    case Types.AccountType.StockFuturesCapital:
                        //初始化期货资金账户（股指期货）
                        InitializationQHCapitalAccount(account, out outMessage, db, tm);
                        break;
                    case Types.AccountType.StockFuturesHoldCode:
                        break;
                    case Types.AccountType.HKSpotCapital:
                        //初始化港股资金账户
                        InitializationHKCapitalAccount(account, out outMessage, db, tm);
                        break;
                    case Types.AccountType.HKSpotHoldCode:
                        break;
                    default:
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                outMessage = ex.Message.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
                return false;
            }
        }

        #endregion

        #region 初始化银行资金账户

        /// <summary>
        /// 初始化银行资金账户
        /// </summary>
        /// <param name="account">账户实体对象</param>
        /// <param name="outMessage">返回信息</param>
        /// <param name="db">数据库对象</param>
        /// <param name="tm">事务对象</param>
        public void InitializationBankCapitalAccount(AccountEntity account, out string outMessage, Database db, DbTransaction tm)
        {
            outMessage = string.Empty;
            //account.
            if (account == null)
            {
                outMessage = "银行资金账户对象为空！";
                return;
            }
            decimal _CurrencyAmount;
            //var _BKCapitalAccount = new SqlUaBankAccountTableProvider(TransactionFactory.RC_ConnectionString, true, "");
            UA_BankAccountTableDal dal = new UA_BankAccountTableDal();
            UA_BankAccountTableInfo model = new UA_BankAccountTableInfo();
            model.UserAccountDistributeLogo = account.Account;
            model.TodayOutInCapital = 0;
            model.FreezeCapital = 0;

            //人民币
            if (account.CurrencyRMB == decimal.MaxValue || account.CurrencyRMB == decimal.MinValue)
            {
                _CurrencyAmount = 0;
            }
            else
            {
                _CurrencyAmount = account.CurrencyRMB;
            }
            model.CapitalRemainAmount = _CurrencyAmount;
            model.BalanceOfTheDay = _CurrencyAmount;
            model.AvailableCapital = _CurrencyAmount;
            model.TradeCurrencyTypeLogo = (int)GTA.VTS.Common.CommonObject.Types.CurrencyType.RMB;
            dal.Add(model, db, tm);

            //港元
            if (account.CurrencyHK == decimal.MaxValue || account.CurrencyHK == decimal.MinValue)
            {
                _CurrencyAmount = 0;
            }
            else
            {
                _CurrencyAmount = account.CurrencyHK;
            }
            model.CapitalRemainAmount = _CurrencyAmount;
            model.BalanceOfTheDay = _CurrencyAmount;
            model.AvailableCapital = _CurrencyAmount;
            model.TradeCurrencyTypeLogo = (int)GTA.VTS.Common.CommonObject.Types.CurrencyType.HK;
            dal.Add(model, db, tm);

            //美元
            if (account.CurrencyUS == decimal.MaxValue || account.CurrencyUS == decimal.MinValue)
            {
                _CurrencyAmount = 0;
            }
            else
            {
                _CurrencyAmount = account.CurrencyUS;
            }
            model.CapitalRemainAmount = _CurrencyAmount;
            model.BalanceOfTheDay = _CurrencyAmount;
            model.AvailableCapital = _CurrencyAmount;
            model.TradeCurrencyTypeLogo = (int)GTA.VTS.Common.CommonObject.Types.CurrencyType.US;
            dal.Add(model, db, tm);
        }

        #endregion

        #region  初始化现货资金账户

        /// <summary>
        /// 初始化现货资金账户
        /// </summary>
        /// <param name="account">账户实体对象</param>
        /// <param name="outMessage"></param>
        /// <param name="db"></param>
        /// <param name="tm">开启事务对象，如果为null不开启事务</param>
        public void InitializationXHCapitalAccount(AccountEntity account, out string outMessage, Database db, DbTransaction tm)
        {
            outMessage = string.Empty;
            if (account == null)
            {
                outMessage = "现货资金账户对象为空！";
                return;
            }
            //var _XhCapitalAccount = new SqlXhCapitalAccountTableProvider(TransactionFactory.RC_ConnectionString, true, "");
            XH_CapitalAccountTableDal dal = new XH_CapitalAccountTableDal();
            XH_CapitalAccountTableInfo model = new XH_CapitalAccountTableInfo();
            model.AvailableCapital = 0;
            model.BalanceOfTheDay = 0;
            model.FreezeCapitalTotal = 0;
            model.TodayOutInCapital = 0;
            model.UserAccountDistributeLogo = account.Account;
            model.CapitalBalance = 0;
            model.TradeCurrencyType = (int)GTA.VTS.Common.CommonObject.Types.CurrencyType.RMB;
            dal.Add(model, db, tm);
            model.TradeCurrencyType = (int)GTA.VTS.Common.CommonObject.Types.CurrencyType.HK;
            dal.Add(model, db, tm);
            model.TradeCurrencyType = (int)GTA.VTS.Common.CommonObject.Types.CurrencyType.US;
            dal.Add(model, db, tm);
        }

        #endregion

        #region 初始化期货资金账户

        /// <summary>
        /// 初始化期货资金账户
        /// </summary>
        /// <param name="account">账户实体对象</param>
        /// <param name="outMessage">返回信息</param>
        /// <param name="db">数据库对象</param>
        /// <param name="tm">事务对象</param>
        public void InitializationQHCapitalAccount(AccountEntity account, out string outMessage, Database db, DbTransaction tm)
        {
            outMessage = string.Empty;
            if (account == null)
            {
                outMessage = "期货资金账户对象为空！";
                return;
            }
            //var _QhCapitalAccount = new SqlQhCapitalAccountTableProvider(TransactionFactory.RC_ConnectionString, true, "");
            QH_CapitalAccountTableDal dal = new QH_CapitalAccountTableDal();
            QH_CapitalAccountTableInfo model = new QH_CapitalAccountTableInfo();
            model.AvailableCapital = 0;
            model.BalanceOfTheDay = 0;
            model.CapitalBalance = 0;
            model.FreezeCapitalTotal = 0;
            model.TodayOutInCapital = 0;
            model.UserAccountDistributeLogo = account.Account;
            model.TradeCurrencyType = (int)GTA.VTS.Common.CommonObject.Types.CurrencyType.RMB;
            dal.Add(model, db, tm);
            model.TradeCurrencyType = (int)GTA.VTS.Common.CommonObject.Types.CurrencyType.HK;
            dal.Add(model, db, tm);
            model.TradeCurrencyType = (int)GTA.VTS.Common.CommonObject.Types.CurrencyType.US;
            dal.Add(model, db, tm);
        }

        #endregion

        #region  初始化港股资金账户

        /// <summary>
        /// 初始化港股资金账户
        /// </summary>
        /// <param name="account">账户实体对象</param>
        /// <param name="outMessage"></param>
        /// <param name="db"></param>
        /// <param name="tm">开启事务对象，如果为null不开启事务</param>
        public void InitializationHKCapitalAccount(AccountEntity account, out string outMessage, Database db, DbTransaction tm)
        {
            outMessage = string.Empty;
            if (account == null)
            {
                outMessage = "现货资金账户对象为空！";
                return;
            }

            HK_CapitalAccountDal dal = new HK_CapitalAccountDal();
            HK_CapitalAccountInfo model = new HK_CapitalAccountInfo();
            model.AvailableCapital = 0;
            model.BalanceOfTheDay = 0;
            model.FreezeCapitalTotal = 0;
            model.TodayOutInCapital = 0;
            model.UserAccountDistributeLogo = account.Account;
            model.CapitalBalance = 0;
            model.TradeCurrencyType = (int)GTA.VTS.Common.CommonObject.Types.CurrencyType.RMB;
            dal.Add(model, db, tm);
            model.TradeCurrencyType = (int)GTA.VTS.Common.CommonObject.Types.CurrencyType.HK;
            dal.Add(model, db, tm);
            model.TradeCurrencyType = (int)GTA.VTS.Common.CommonObject.Types.CurrencyType.US;
            dal.Add(model, db, tm);
        }

        #endregion

        #region 追加资金
        /// <summary>
        /// 追加资金
        /// </summary>
        /// <param name="addCapital"></param>
        /// <param name="outMessage"></param>
        /// <returns></returns>
        public bool AddCapital(AddCapitalEntity addCapital, out string outMessage)
        {
            outMessage = string.Empty;
            //TransactionManager tm = TransactionFactory.GetTransactionManager();
            //tm.BeginTransaction();
            try
            {
                if (addCapital == null) return false;
                UA_UserAccountAllocationTableDal userAccountDal = new UA_UserAccountAllocationTableDal();
                // new SqlUaUserAccountAllocationTableProvider(TransactionFactory.RC_ConnectionString, true, "");
                string findCodition = string.Format(" UserID='{0}' AND AccountTypeLogo='{1}'", addCapital.TraderID, 1);
                List<UA_UserAccountAllocationTableInfo> list = userAccountDal.GetListArray(findCodition);
                if (list == null || list.Count < 1)
                {
                    outMessage = "该交易员的银行帐号不存在！";
                    return false;
                }
                if (list[0].UserAccountDistributeLogo != addCapital.BankCapitalAccount)
                {
                    outMessage = "银行帐号不正确！";
                    return false;
                }

                UA_BankAccountTableDal _BankAccount = new UA_BankAccountTableDal();
                //new SqlUaBankAccountTableProvider(TransactionFactory.RC_ConnectionString, true, "");
                UA_CapitalFlowTableDal _UaCapitalFlow = new UA_CapitalFlowTableDal();
                //new SqlUaCapitalFlowTableProvider(TransactionFactory.RC_ConnectionString, true, "");
                Database db = DatabaseFactory.CreateDatabase();
                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();
                    DbTransaction trm = conn.BeginTransaction();

                    try
                    {
                        if (addCapital.AddRMBAmount > 0 && addCapital.AddRMBAmount != decimal.MaxValue)
                        {
                            #region 增加人民币
                            var _record = _BankAccount.GetModel((int)Types.CurrencyType.RMB, addCapital.BankCapitalAccount);
                            if (_record == null)
                            {
                                outMessage = "币种为人民币的银行帐号不存在！";
                                return false;
                            }
                            #region old code
                            ////_record.CapitalRemainAmount = _record.CapitalRemainAmount + addCapital.AddRMBAmount;
                            ////_record.AvailableCapital += addCapital.AddRMBAmount;
                            ////_record.TodayOutInCapital += addCapital.AddRMBAmount;
                            ////if (!_BankAccount.Update(tm, _record))
                            ////{
                            ////    outMessage = "更新人民币银行帐号失败！";
                            ////    return false;
                            ////}
                            //string sql =
                            //    string.Format(
                            //        "Update UA_BankAccountTable set CapitalRemainAmount=CapitalRemainAmount+{0},AvailableCapital=AvailableCapital+{0},TodayOutInCapital=TodayOutInCapital+{0} where TradeCurrencyTypeLogo={1} AND UserAccountDistributeLogo='{2}'",
                            //        addCapital.AddRMBAmount, (int)Types.CurrencyType.RMB, addCapital.BankCapitalAccount);
                            //DataRepository.Provider.ExecuteNonQuery(tm, CommandType.Text, sql);
                            #endregion
                            _BankAccount.AddCapital(addCapital.AddRMBAmount, addCapital.BankCapitalAccount, Types.CurrencyType.RMB, db, trm);

                            var _UaCapitalFlowTable = new UA_CapitalFlowTableInfo();
                            _UaCapitalFlowTable.TradeCurrencyType = (int)Types.CurrencyType.RMB;
                            _UaCapitalFlowTable.FromCapitalAccount = string.Empty;
                            _UaCapitalFlowTable.ToCapitalAccount = addCapital.BankCapitalAccount;
                            _UaCapitalFlowTable.TransferAmount = addCapital.AddRMBAmount;
                            _UaCapitalFlowTable.TransferTime = System.DateTime.Now;
                            _UaCapitalFlowTable.TransferTypeLogo =
                                (int)ReckoningCounter.Entity.Contants.Types.TransferType.AddCapital;
                            _UaCapitalFlow.Add(_UaCapitalFlowTable, db, trm);
                            //if (!_UaCapitalFlow.Insert(tm, _UaCapitalFlowTable))
                            //{
                            //outMessage = "记录资金流水失败";
                            //return false;
                            //}
                            #endregion
                        }
                        if (addCapital.AddHKAmount > 0 && addCapital.AddHKAmount != decimal.MaxValue)
                        {
                            #region 增加港币
                            var _record = _BankAccount.GetModel((int)Types.CurrencyType.HK, addCapital.BankCapitalAccount);
                            //var _record =  _BankAccount.GetByTradeCurrencyTypeLogoUserAccountDistributeLogo((int)Types.CurrencyType.HK,
                            //                                                                     addCapital.BankCapitalAccount);
                            if (_record == null)
                            {
                                outMessage = "币种为港元的银行帐号不存在！";
                                return false;
                            }
                            #region old code
                            ////_record.CapitalRemainAmount += addCapital.AddHKAmount;
                            ////_record.AvailableCapital += addCapital.AddHKAmount;
                            ////_record.TodayOutInCapital += addCapital.AddHKAmount;
                            ////if (!_BankAccount.Update(tm, _record))
                            ////{
                            ////    outMessage = "更新港元银行帐号失败！";
                            ////    return false;
                            ////}
                            //string sql =
                            //   string.Format(
                            //       "Update UA_BankAccountTable set CapitalRemainAmount=CapitalRemainAmount+{0},AvailableCapital=AvailableCapital+{0},TodayOutInCapital=TodayOutInCapital+{0} where TradeCurrencyTypeLogo={1} AND UserAccountDistributeLogo='{2}'",
                            //       addCapital.AddHKAmount, (int)Types.CurrencyType.HK, addCapital.BankCapitalAccount);
                            //DataRepository.Provider.ExecuteNonQuery(tm, CommandType.Text, sql);
                            #endregion
                            _BankAccount.AddCapital(addCapital.AddHKAmount, addCapital.BankCapitalAccount, Types.CurrencyType.HK, db, trm);

                            var _UaCapitalFlowTable = new UA_CapitalFlowTableInfo();
                            _UaCapitalFlowTable.TradeCurrencyType = (int)Types.CurrencyType.HK;
                            _UaCapitalFlowTable.FromCapitalAccount = string.Empty;
                            _UaCapitalFlowTable.ToCapitalAccount = addCapital.BankCapitalAccount;
                            _UaCapitalFlowTable.TransferAmount = addCapital.AddHKAmount;
                            _UaCapitalFlowTable.TransferTime = System.DateTime.Now;
                            _UaCapitalFlowTable.TransferTypeLogo =
                                (int)ReckoningCounter.Entity.Contants.Types.TransferType.AddCapital;
                            _UaCapitalFlow.Add(_UaCapitalFlowTable, db, trm);
                            //if (!_UaCapitalFlow.Insert(tm, _UaCapitalFlowTable))
                            //{
                            //    outMessage = "记录资金流水失败";
                            //    return false;
                            //}
                            #endregion
                        }
                        if (addCapital.AddUSAmount > 0 && addCapital.AddUSAmount != decimal.MaxValue)
                        {
                            #region 增加美元
                            //var _record =
                            //    _BankAccount.GetByTradeCurrencyTypeLogoUserAccountDistributeLogo((int)Types.CurrencyType.US,
                            //                                                                     addCapital.BankCapitalAccount);
                            var _record = _BankAccount.GetModel((int)Types.CurrencyType.US, addCapital.BankCapitalAccount);

                            if (_record == null)
                            {
                                outMessage = "币种为美元的银行帐号不存在！";
                                return false;
                            }
                            //_record.CapitalRemainAmount += addCapital.AddUSAmount;
                            //_record.AvailableCapital += addCapital.AddUSAmount;
                            //_record.TodayOutInCapital += addCapital.AddUSAmount;
                            //if (!_BankAccount.Update(tm, _record))
                            //{
                            //    outMessage = "更新美元银行帐号失败！";
                            //    return false;
                            //}

                            //string sql =
                            //  string.Format(
                            //      "Update UA_BankAccountTable set CapitalRemainAmount=CapitalRemainAmount+{0},AvailableCapital=AvailableCapital+{0},TodayOutInCapital=TodayOutInCapital+{0} where TradeCurrencyTypeLogo={1} AND UserAccountDistributeLogo='{2}'",
                            //      addCapital.AddUSAmount, (int)Types.CurrencyType.US, addCapital.BankCapitalAccount);
                            //DataRepository.Provider.ExecuteNonQuery(tm, CommandType.Text, sql);
                            _BankAccount.AddCapital(addCapital.AddUSAmount, addCapital.BankCapitalAccount, Types.CurrencyType.US, db, trm);

                            var _UaCapitalFlowTable = new UA_CapitalFlowTableInfo();
                            _UaCapitalFlowTable.TradeCurrencyType = (int)Types.CurrencyType.US;
                            _UaCapitalFlowTable.FromCapitalAccount = string.Empty;
                            _UaCapitalFlowTable.ToCapitalAccount = addCapital.BankCapitalAccount;
                            _UaCapitalFlowTable.TransferAmount = addCapital.AddUSAmount;
                            _UaCapitalFlowTable.TransferTime = System.DateTime.Now;
                            _UaCapitalFlowTable.TransferTypeLogo =
                                (int)ReckoningCounter.Entity.Contants.Types.TransferType.AddCapital;
                            _UaCapitalFlow.Add(_UaCapitalFlowTable, db, trm);

                            //if (!_UaCapitalFlow.Insert(tm, _UaCapitalFlowTable))
                            //{
                            //    outMessage = "记录资金流水失败";
                            //    return false;
                            //}
                            #endregion
                        }
                        trm.Commit();
                    }
                    catch (Exception ex)
                    {
                        trm.Rollback();
                        outMessage = ex.Message.ToString();
                        LogHelper.WriteError(outMessage, ex);
                        return false;
                    }
                    finally
                    {
                        trm.Dispose();
                    }
                }
                outMessage = "追加资金成功！";
                return true;
            }
            catch (Exception ex)
            {
                outMessage = ex.Message.ToString();
                LogHelper.WriteError(outMessage, ex);
                return false;
            }
        }
        #endregion
      
    }
}