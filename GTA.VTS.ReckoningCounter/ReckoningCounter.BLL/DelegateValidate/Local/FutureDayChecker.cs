#region Using Namespace

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Threading;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.BLL.ScheduleManagement;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.Entity;
using ReckoningCounter.MemoryData;
using ReckoningCounter.Model;
using Timer = System.Timers.Timer;
using ReckoningCounter.Entity.AccountManagementAndFindEntity;

#endregion

namespace ReckoningCounter.BLL.DelegateValidate.Local
{
    /// <summary>
    /// 期货每日开盘检查类
    /// 作者：宋涛 周建月
    /// 日期：2008-11-25
    /// Desc:增加了商品期货开盘检查处理
    /// Update by: 董鹏
    /// Update Date:2010-02-4
    /// Update by: 李健华
    /// Update Date:2010-03-02
    /// Desc.:修改平台类型以及平仓相关的逻辑。
    /// </summary>
    public class FutureDayChecker
    {
        /// <summary>
        /// 每日开盘检查处理
        /// </summary>
        public void Process()
        {
            ProcessHold();

            ProcessCapital();
        }

        /// <summary>
        /// 执行持仓检查
        /// </summary>
        private void ProcessHold()
        {
            Timer timer = new Timer();
            timer.Interval = 1 * 60 * 1000;
            timer.Elapsed += delegate
                                 {
                                     if (MemoryDataManager.Status != MemoryDataManagerStatus.Open)
                                         return;

                                     Thread thread = new Thread(CheckHold);
                                     thread.Start();
                                     timer.Enabled = false;
                                 };
            timer.Enabled = true;
        }

        /// <summary>
        /// 持仓检查
        /// </summary>
        private void CheckHold()
        {
            #region new 李健华 2010-06-08
            //不处理缓存直接从数据库中获取处理
            //VTTraders traders = VTTradersFactory.GetFutureTraders();

            //if (!traders.IsInitializeSuccess)
            //{
            //    return;
            //}

            LogHelper.WriteInfo("FutureDayChecker.Process开始每日开盘期货持仓检查处理");

            //获取所有期货持仓账号类型
            List<BD_AccountTypeInfo> futureAccountTypes = AccountManager.Instance.GetAccoutTypeByACTID((int)Types.AccountAttributionType.FuturesCapital);

            if (Utils.IsNullOrEmpty(futureAccountTypes))
            {
                return;
            }
            try
            {
                //有股指期货和商品期货，或者以后还有别的品种
                foreach (var item in futureAccountTypes)
                {
                    List<AccountPair> userAccountPair = VTTradersFactory.InitializeAccountPair(item);
                    if (Utils.IsNullOrEmpty(userAccountPair))
                    {
                        continue;
                    }
                    foreach (var accountPair in userAccountPair)
                    {
                        UserAccountChecker checker = new UserAccountChecker(accountPair.CapitalAccount, accountPair.HoldAccount);
                        checker.ProcessHold();
                    }
                }

                //更新数据库中的每日检查日期
                //StatusTableChecker.UpdateFutureDayCheckDate(tm);

                //tm.Commit();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                //tm.Rollback();
                LogHelper.WriteInfo("FutureDayChecker.Process每日开盘期货检查:持仓处理失败！");
            }
            #endregion

            #region old code update by李健华 2010-06-08
            //VTTraders traders = VTTradersFactory.GetFutureTraders();

            //if (!traders.IsInitializeSuccess)
            //{
            //    return;
            //}

            //LogHelper.WriteInfo("FutureDayChecker.Process开始每日开盘期货持仓检查处理");

            //try
            //{
            //    foreach (var vtTrader in traders.TraderList)
            //    {
            //        foreach (var accountPair in vtTrader.AccountPairList)
            //        {
            //            UserAccountChecker checker = new UserAccountChecker(accountPair.CapitalAccount, accountPair.HoldAccount);
            //            checker.ProcessHold();
            //        }
            //    }

            //    //更新数据库中的每日检查日期
            //    //StatusTableChecker.UpdateFutureDayCheckDate(tm);

            //    //tm.Commit();
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.WriteError(ex.Message, ex);
            //    //tm.Rollback();
            //    LogHelper.WriteInfo("FutureDayChecker.Process每日开盘期货检查:持仓处理失败！");
            //}
            #endregion
        }


        /// <summary>
        /// 执行资金检查
        /// </summary>
        private void ProcessCapital()
        {
            //开盘30分钟计时后 才对期货资金表进行检查
            int min = 30;
            //Timer mCheckFundTimer = new Timer(CheckCapitalTable, null, 1000*60*min, -1);

            string key = "daycheck";
            string value = ConfigurationManager.AppSettings[key];
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    min = int.Parse(value.Trim());
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }

            Timer timer = new Timer();
            timer.Interval = min * 60 * 1000;
            timer.Elapsed += delegate
                                 {
                                     Thread thread = new Thread(CheckCapitalTable);
                                     thread.Start();
                                     timer.Enabled = false;
                                 };

            timer.Enabled = true;
        }

        /// <summary>
        /// 资金检查
        /// </summary>
        private void CheckCapitalTable()
        {

            #region new 李健华 2010-06-08
            //VTTraders traders = VTTradersFactory.GetFutureTraders();

            //if (!traders.IsInitializeSuccess)
            //    return;

            LogHelper.WriteInfo("FutureDayChecker.Process开始每日开盘期货资金检查处理");

            //var tm = TransactionFactory.GetTransactionManager();
            //tm.BeginTransaction();
            ReckoningTransaction reckoningTransaction = new ReckoningTransaction();
            Database database = DatabaseFactory.CreateDatabase();
            DbConnection connection = database.CreateConnection();

            try
            {
                connection.Open();
                reckoningTransaction.Database = database;
                DbTransaction transaction = connection.BeginTransaction();
                reckoningTransaction.Transaction = transaction;

                //获取所有期货持仓账号类型
                List<BD_AccountTypeInfo> futureAccountTypes = AccountManager.Instance.GetAccoutTypeByACTID((int)Types.AccountAttributionType.FuturesCapital);

                if (Utils.IsNullOrEmpty(futureAccountTypes))
                {
                    return;
                }
                foreach (var item in futureAccountTypes)
                {
                    List<AccountPair> userAccountPair = VTTradersFactory.InitializeAccountPair(item);
                    if (Utils.IsNullOrEmpty(userAccountPair))
                    {
                        continue;
                    }
                    foreach (var accountPair in userAccountPair)
                    {
                        UserAccountChecker checker = new UserAccountChecker(accountPair.CapitalAccount, accountPair.HoldAccount);
                        checker.ProcessCapital();
                    }
                }

                //更新数据库中的每日检查日期
                StatusTableChecker.UpdateFutureDayCheckDate(reckoningTransaction);

                reckoningTransaction.Transaction.Commit();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                reckoningTransaction.Transaction.Rollback();
                LogHelper.WriteInfo("FutureDayChecker.Process每日开盘期货检查:资金处理失败！");
            }
            finally
            {
                //这里为了安全起见，自行执行一次关闭数据连接
                //if (reckoningTransaction != null && reckoningTransaction.Transaction != null)
                //{
                //    reckoningTransaction.Transaction.Dispose();
                //}
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

            LogHelper.WriteInfo("FutureDayChecker.Process结束每日开盘期货检查处理");
            #endregion

            #region old code update by李健华 2010-06-08
            //VTTraders traders = VTTradersFactory.GetFutureTraders();

            //if (!traders.IsInitializeSuccess)
            //    return;

            //LogHelper.WriteInfo("FutureDayChecker.Process开始每日开盘期货资金检查处理");

            ////var tm = TransactionFactory.GetTransactionManager();
            ////tm.BeginTransaction();
            //ReckoningTransaction reckoningTransaction = new ReckoningTransaction();
            //Database database = DatabaseFactory.CreateDatabase();
            //DbConnection connection = database.CreateConnection();

            //try
            //{
            //    connection.Open();
            //    reckoningTransaction.Database = database;
            //    DbTransaction transaction = connection.BeginTransaction();
            //    reckoningTransaction.Transaction = transaction;
            //    foreach (var vtTrader in traders.TraderList)
            //    {
            //        foreach (var accountPair in vtTrader.AccountPairList)
            //        {
            //            UserAccountChecker checker = new UserAccountChecker(accountPair.CapitalAccount, accountPair.HoldAccount);
            //            checker.ProcessCapital();
            //        }
            //    }

            //    //更新数据库中的每日检查日期
            //    StatusTableChecker.UpdateFutureDayCheckDate(reckoningTransaction);

            //    reckoningTransaction.Transaction.Commit();
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.WriteError(ex.Message, ex);
            //    reckoningTransaction.Transaction.Rollback();
            //    LogHelper.WriteInfo("FutureDayChecker.Process每日开盘期货检查:资金处理失败！");
            //}
            //finally
            //{
            //    //这里为了安全起见，自行执行一次关闭数据连接
            //    //if (reckoningTransaction != null && reckoningTransaction.Transaction != null)
            //    //{
            //    //    reckoningTransaction.Transaction.Dispose();
            //    //}
            //    if (connection != null && connection.State != ConnectionState.Closed)
            //    {
            //        connection.Close();
            //    }
            //}

            //LogHelper.WriteInfo("FutureDayChecker.Process结束每日开盘期货检查处理");
            #endregion
        }

        /// <summary>
        /// 获取用户指定类型的账户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="accountTypeID"></param>
        /// <returns></returns>
        public static List<UA_UserAccountAllocationTableInfo> GetAccountList(UA_UserBasicInformationTableInfo user, int accountTypeID)
        {
            string where = string.Format("UserID = '{0}' AND AccountTypeLogo= '{1}'",
                                         user.UserID, accountTypeID);
            UA_UserAccountAllocationTableDal ua_UserAccountAllocationTableDal = new UA_UserAccountAllocationTableDal();
            return ua_UserAccountAllocationTableDal.GetListArray(where);
        }

        #region 废弃代码

        /*
        /// <summary>
        /// 每日开盘检查处理
        /// </summary>
        private void Process_Old()
        {
            //【期货资金类型】(AccountTypeClass账户类型分类)
            Types.AccountAttributionType actID = Types.AccountAttributionType.FuturesCapital;

            //账户类型AccountType 获取对应【期货资金类型】的帐户类型：1.商品期货资金帐号 2.股指期货资金帐号
            TList<BdAccountType> accountTypes = DataRepository.BdAccountTypeProvider.GetByAtcId((int) actID);
            if (Utils.IsNullOrEmpty(accountTypes))
                return;

            TList<UaUserBasicInformationTable> users = DataRepository.UaUserBasicInformationTableProvider.GetAll();
            if (Utils.IsNullOrEmpty(users))
                return;

            foreach (UaUserBasicInformationTable user in users)
            {
                foreach (BdAccountType accountType in accountTypes)
                {
                    ProcessAccount(user, accountType);
                }
            }
        }

        /// <summary>
        /// 对单个交易员进行校验
        /// </summary>
        /// <param name="user">交易员</param>
        /// <param name="accountType">资金账户类型</param>
        private void ProcessAccount(UaUserBasicInformationTable user, BdAccountType accountType)
        {
            TList<UaUserAccountAllocationTable> fundAccountList = GetAccountList(user,
                                                                                 accountType.AccountTypeLogo);

            if (Utils.IsNullOrEmpty(fundAccountList))
                return;

            //对应资金账户类型的只能有一个资金交易账户
            UaUserAccountAllocationTable fundAccount = fundAccountList[0];

            //查找对应的持仓交易账户
            int? relationID = accountType.RelationAccountId;
            if (!relationID.HasValue)
                return;
            int holdAccountTypeID = relationID.Value;

            TList<UaUserAccountAllocationTable> HoldAccountList = GetAccountList(user, holdAccountTypeID);
            if (Utils.IsNullOrEmpty(HoldAccountList))
                return;

            //对应持仓账户类型的只能有一个持仓交易账户
            UaUserAccountAllocationTable holdAccount = HoldAccountList[0];

            UserAccountChecker checker = new UserAccountChecker(fundAccount, holdAccount);
            checker.Process();
        }
        */

        #endregion
    }

    /// <summary>
    /// 对应一个资金账户的检查
    /// </summary>
    public class UserAccountChecker
    {
        private readonly UA_UserAccountAllocationTableInfo capitalAccount;
        private readonly UA_UserAccountAllocationTableInfo holdAccount;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="capitalAccount"></param>
        /// <param name="holdAccount"></param>
        public UserAccountChecker(UA_UserAccountAllocationTableInfo capitalAccount, UA_UserAccountAllocationTableInfo holdAccount)
        {
            this.capitalAccount = capitalAccount;
            this.holdAccount = holdAccount;
        }

        //public void Process()
        //{
        //    if (!fundAccount.WhetherAvailable.HasValue)
        //        return;

        //    if (!fundAccount.WhetherAvailable.Value)
        //        return;

        //    //开盘30分钟计时后 才对期货资金表进行检查
        //    int min = 3;
        //    Timer mCheckFundTimer = new Timer(CheckCapitalTable, null, 1000*60*min, -1);
        //    //CheckCapitalTable(new object());

        //    //检查持仓表
        //    CheckHoldTable();
        //}

        /// <summary>
        /// 执行资金检查
        /// </summary>
        public void ProcessCapital()
        {
            //if (!capitalAccount.WhetherAvailable)
            //    return;

            if (!capitalAccount.WhetherAvailable)
                return;

            CheckCapitalTable(null);
        }

        /// <summary>
        /// 执行持仓检查
        /// </summary>
        public void ProcessHold()
        {
            //if (!capitalAccount.WhetherAvailable)
            //    return;

            if (!holdAccount.WhetherAvailable)
                return;

            //检查持仓表
            CheckHoldTable();
        }

        /// <summary>
        /// 检查资金表，可用资金小于0的走资金强行平仓流程
        /// </summary>
        private void CheckCapitalTable(object state)
        {
            //LogHelper.WriteDebug("FutureDayChecker.CheckFundTable开始检查资金表");
            QH_CapitalAccountTableDal dal = new QH_CapitalAccountTableDal();
            //TList<QhCapitalAccountTable> list =
            //  DataRepository.QhCapitalAccountTableProvider.GetByUserAccountDistributeLogo(
            //      capitalAccount.UserAccountDistributeLogo);
            string where = string.Format("UserAccountDistributeLogo = '{0}' ", capitalAccount.UserAccountDistributeLogo);
            List<QH_CapitalAccountTableInfo> list = dal.GetListArray(where);

            if (Utils.IsNullOrEmpty(list))
                return;

            foreach (QH_CapitalAccountTableInfo accountTable in list)
            {
                //if (accountTable.AvailableCapital<0.00m)
                //    continue;

                List<QH_HoldAccountTableInfo> listCloseContract;
                OrderAccepter orderAccepter = OrderAccepterService.Service;

                if (accountTable.AvailableCapital < 0)
                {
                    if (accountTable.TradeCurrencyType < 0.00m)
                        continue;

                    List<QH_HoldAccountTableInfo> list2 = GetHoldAccountTableList(holdAccount.UserAccountDistributeLogo, accountTable.TradeCurrencyType);
                    if (Utils.IsNullOrEmpty(list2))
                        continue;

                    listCloseContract = FindWillCLosedContract(list2);

                    foreach (var holdTable in listCloseContract)
                    {
                        //如果不是交易日，不进行强行平仓 add by 董鹏 2010-05-05
                        if (!CommonParaProxy.GetInstance().IsTradeDate(holdTable.Contract))
                        {
                            continue;
                        }

                        //CloseStockIndexContract(orderAccepter, holdTable, 0, false);
                        #region 按账户类型进行商品期货、股指期货平仓 add by 董鹏 2010-02-03
                        if (holdAccount.AccountTypeLogo == (int)Types.AccountType.CommodityFuturesHoldCode)
                        {
                            //此处原来只有HistoryHoldAmount，造成未清算成功的持仓平不掉，因此加上TodayHoldAmount；
                            //而TodayFreezeAmount清算是否成功都没有影响 -- update by 董鹏 2010-03-29
                            //平历史
                            CloseCommoditiesContract(orderAccepter, holdTable, 0, (float)holdTable.HistoryHoldAmount, Types.QHForcedCloseType.CapitalCheck, ReckoningCounter.Entity.Contants.Types.FutureOpenCloseType.ClosePosition);
                            //平今
                            CloseCommoditiesContract(orderAccepter, holdTable, 0, (float)holdTable.TodayHoldAmount, Types.QHForcedCloseType.CapitalCheck, ReckoningCounter.Entity.Contants.Types.FutureOpenCloseType.CloseTodayPosition);
                        }
                        if (holdAccount.AccountTypeLogo == (int)Types.AccountType.StockFuturesHoldCode)
                        {
                            CloseStockIndexContract(orderAccepter, holdTable, (float)holdTable.HoldAveragePrice, false);
                        }
                        #endregion
                    }
                }
            }

            //LogHelper.WriteDebug("FutureDayChecker.CheckFundTable结束检查资金表");
        }

        /// <summary>
        /// 进行持仓检查，对已经过期的合约按昨日结算价进行强制平仓
        /// </summary>
        private void CheckHoldTable()
        {
            //LogHelper.WriteDebug("FutureDayChecker.CheckHoldTable开始检查持仓表");

            OrderAccepter orderAccepter = OrderAccepterService.Service;
            QH_HoldAccountTableDal dal = new QH_HoldAccountTableDal();
            string where = string.Format("UserAccountDistributeLogo = '{0}'", holdAccount.UserAccountDistributeLogo);
            List<QH_HoldAccountTableInfo> listCheck = dal.GetListArray(where);
            // DataRepository.QhHoldAccountTableProvider.GetByUserAccountDistributeLogo(
            //     holdAccount.UserAccountDistributeLogo);

            if (Utils.IsNullOrEmpty(listCheck))
            {
                return;
            }

            decimal price = 0;
            List<QH_HoldAccountTableInfo> list = new List<QH_HoldAccountTableInfo>();
            //这里是要分开处理，已经是过期的合约前面就已经强行平仓掉就没有后面的持仓限制和最小单位整数倍的判断
            foreach (var holdTable in listCheck)
            {
                //如果不是交易日，不进行强行平仓 add by 董鹏 2010-05-05
                if (!CommonParaProxy.GetInstance().IsTradeDate(holdTable.Contract))
                {
                    continue;
                }
                if (MCService.IsExpireLastedTradeDate(holdTable.Contract))
                {
                    //在最后交易日 进行平仓
                    //string msg = "";
                    //bool canGetPrice = MCService.GetFutureYesterdayPreSettlementPrice(holdTable.Contract, out price,
                    //                                                                  ref msg);
                    //if (!canGetPrice)
                    //{
                    //    string format = "FutureDayChecker.CheckHoldTable无法获取合约{0}的昨日收盘结算价,错误信息:{1}";
                    //    string msg2 = string.Format(format, holdTable.Contract, msg);
                    //    LogHelper.WriteDebug(msg2);
                    //    continue;
                    //}

                    //每天清算后，持仓均价就是结算价
                    price = holdTable.HoldAveragePrice;

                    #region 按账户类型进行商品期货、股指期货平仓 add by 董鹏 2010-02-03
                    UA_UserAccountAllocationTableDal accDal = new UA_UserAccountAllocationTableDal();
                    var acc = accDal.GetModel(holdTable.UserAccountDistributeLogo);
                    if (acc.AccountTypeLogo == (int)Types.AccountType.CommodityFuturesHoldCode)
                    {
                        //此处原来只有HistoryHoldAmount，造成未清算成功的持仓平不掉，因此加上TodayHoldAmount；
                        //而TodayFreezeAmount清算是否成功都没有影响 -- update by 董鹏 2010-03-29
                        //CloseCommoditiesContract(orderAccepter, holdTable, (float)price, (float)(holdTable.HistoryHoldAmount + holdTable.TodayHoldAmount), Types.QHForcedCloseType.Expired);
                        //平历史
                        CloseCommoditiesContract(orderAccepter, holdTable, (float)price, (float)holdTable.HistoryHoldAmount, Types.QHForcedCloseType.Expired, ReckoningCounter.Entity.Contants.Types.FutureOpenCloseType.ClosePosition);
                        //平今
                        CloseCommoditiesContract(orderAccepter, holdTable, (float)price, (float)holdTable.TodayHoldAmount, Types.QHForcedCloseType.Expired, ReckoningCounter.Entity.Contants.Types.FutureOpenCloseType.CloseTodayPosition);
                    }
                    if (acc.AccountTypeLogo == (int)Types.AccountType.StockFuturesHoldCode)
                    {
                        CloseStockIndexContract(orderAccepter, holdTable, (float)price, true);
                    }
                    #endregion
                    //CloseStockIndexContract(orderAccepter, holdTable, (float)price, true);
                }
                else
                {
                    list.Add(holdTable);
                }
            }


            #region 商品期货进行持仓限制、最小交割单位整数倍检验，并平仓超出量 add by 董鹏 2010-02-04
            if (holdAccount.AccountTypeLogo == (int)Types.AccountType.CommodityFuturesHoldCode)
            {
                LogHelper.WriteDebug("---->商品期货持仓限制、最小小交割单位整数倍验证，UserAccountDistributeLogo=" + holdAccount.UserAccountDistributeLogo);
                List<QH_HoldAccountTableInfo> listCloseContract;

                price = 0;

                //记录超出持仓限制的量：<持仓记录主键,超出量>
                Dictionary<int, decimal> dicAmount;
                // 超过最大持仓限制
                listCloseContract = FindWillClosedContractOverMaxHoldLimit(list, out dicAmount);
                foreach (var holdTable in listCloseContract)
                {
                    CloseCommoditiesContract(orderAccepter, holdTable, (float)price, (float)dicAmount[holdTable.AccountHoldLogoId], Types.QHForcedCloseType.OverHoldLimit, ReckoningCounter.Entity.Contants.Types.FutureOpenCloseType.ClosePosition);
                    //进行了持仓限制平仓的不再进行最小交割单位验证，下次持仓检查的时候才进行。
                    list.Remove(holdTable);
                }
                //超出最小交割量整数倍
                listCloseContract = FindWillClosedContractNotModMinUnitLimit(list, out dicAmount);
                foreach (var holdTable in listCloseContract)
                {
                    CloseCommoditiesContract(orderAccepter, holdTable, (float)price, (float)dicAmount[holdTable.AccountHoldLogoId], Types.QHForcedCloseType.NotModMinUnit, ReckoningCounter.Entity.Contants.Types.FutureOpenCloseType.ClosePosition);
                }
            }
            #endregion
            //LogHelper.WriteDebug("FutureDayChecker.CheckHoldTable结束检查持仓表");
        }

        /// <summary>
        /// 平仓股指期货合约
        /// </summary>
        /// <param name="orderAccepter"></param>
        /// <param name="holdTable">持仓合约</param>
        /// <param name="price">价格</param>
        /// <param name="isExpiredContract">是否是过期的合约需要平仓</param>
        private void CloseStockIndexContract(OrderAccepter orderAccepter, QH_HoldAccountTableInfo holdTable, float price, bool isExpiredContract)
        {
            if (holdTable == null)
                return;

            //if (holdTable.HistoryHoldAmount<0.00m)
            //    return;

            if (holdTable.HistoryHoldAmount == 0)
                return;

            StockIndexFuturesOrderRequest request = null;

            if (isExpiredContract)
            {
                request = new StockIndexFuturesOrderRequest2 { IsForcedCloseOrder = true, QHForcedCloseType = Types.QHForcedCloseType.Expired };
            }
            else
            {
                request = new StockIndexFuturesOrderRequest2 { IsForcedCloseOrder = true, QHForcedCloseType = Types.QHForcedCloseType.CapitalCheck };
            }

            var buySellType = holdTable.BuySellTypeId == (int)Types.TransactionDirection.Buying
                                  ? Types.TransactionDirection.Selling
                                  : Types.TransactionDirection.Buying;

            request.BuySell = buySellType;
            request.Code = holdTable.Contract;
            request.FundAccountId = capitalAccount.UserAccountDistributeLogo;
            request.OpenCloseType = Entity.Contants.Types.FutureOpenCloseType.ClosePosition;

            request.OrderAmount = (float)holdTable.HistoryHoldAmount;

            request.OrderPrice = price;
            request.OrderUnitType = Types.UnitType.Hand;
            request.OrderWay = price == 0
                                   ? Entity.Contants.Types.OrderPriceType.OPTMarketPrice
                                   : Entity.Contants.Types.OrderPriceType.OPTLimited;

            string type = isExpiredContract ? "持仓检查平仓" : "资金检查平仓";
            string format =
                "FutureDayChecker开盘持仓检查强制平仓[UserAccountDistributeLogo={0},AccountHoldLogoId={1}，Code={2}, Price={3}, 平仓类型={4}]-委托信息：" +
                request;
            string msg = string.Format(format, holdTable.UserAccountDistributeLogo, holdTable.AccountHoldLogoId,
                                       holdTable.Contract, price, type);
            LogHelper.WriteDebug(msg + holdTable);
            UA_UserAccountAllocationTableDal ua_UserAccountAllocationTableDal = new UA_UserAccountAllocationTableDal();
            //设置为其所属的交易员
            var userAccountAllocationTable =
                ua_UserAccountAllocationTableDal.GetModel(
                    capitalAccount.UserAccountDistributeLogo);
            if (userAccountAllocationTable == null)
            {
                string msg2 = "开盘检查强行平仓失败！无法获取资金账户信息，ID=" + capitalAccount.UserAccountDistributeLogo;
                LogHelper.WriteInfo(msg2);
                return;
            }
            UA_UserBasicInformationTableDal ua_UserBasicInformationTableDal = new UA_UserBasicInformationTableDal();
            var user =
                ua_UserBasicInformationTableDal.GetModel(userAccountAllocationTable.UserID);
            if (user == null)
            {
                string msg3 = "开盘检查强行平仓失败！无法获取交易员信息，UserID=" + userAccountAllocationTable.UserID;
                LogHelper.WriteInfo(msg3);
                return;
            }

            request.TraderId = user.UserID;
            request.TraderPassword = user.Password;

            orderAccepter.DoStockIndexFuturesOrder(request);
        }

        /// <summary>
        /// 商品期货强行平仓
        /// Create by:董鹏
        /// Create Date:2010-02-04
        /// </summary>
        /// <param name="orderAccepter">委托接收对象</param>
        /// <param name="holdTable">持仓实体</param>
        /// <param name="price">委托价格</param>
        /// <param name="amount">委托数量</param>
        /// <param name="isExpiredContract">是否是过期的合约需要平仓</param>
        private void CloseCommoditiesContract(OrderAccepter orderAccepter, QH_HoldAccountTableInfo holdTable, float price, float amount, Types.QHForcedCloseType closeType, Entity.Contants.Types.FutureOpenCloseType closeType2)
        {
            if (holdTable == null)
            {
                return;
            }
            if (holdTable.HistoryHoldAmount == 0 && closeType2 == ReckoningCounter.Entity.Contants.Types.FutureOpenCloseType.ClosePosition)
            {
                return;
            }
            if (holdTable.TodayHoldAmount == 0 && closeType2 == ReckoningCounter.Entity.Contants.Types.FutureOpenCloseType.CloseTodayPosition)
            {
                return;
            }

            MercantileFuturesOrderRequest2 request = null;

            request = new MercantileFuturesOrderRequest2();
            string type = "";
            switch (closeType)
            {
                case Types.QHForcedCloseType.Expired:
                    // request.IsExpiredContract = true;
                    type = "过期合约持仓检查平仓";
                    break;
                case Types.QHForcedCloseType.CapitalCheck:
                    //request.IsCapitalCheckContract = true;
                    type = "资金检查平仓";
                    break;
                case Types.QHForcedCloseType.OverHoldLimit:
                    //request.IsOverHoldLimitContract = true;
                    type = "持仓限制检查平仓";
                    break;
                case Types.QHForcedCloseType.NotModMinUnit:
                    //request.IsNotModMinUnitContract = true;
                    type = "最小交割单位整数倍持仓检查平仓";
                    break;
            }

            request.QHForcedCloseType = closeType;//盘前检查强行平仓类型
            request.IsForcedCloseOrder = true;//是否盘前检查强行平仓

            //{
            //    request = new MercantileFuturesOrderRequest2 { IsExpiredContract = true, IsCapitalCheckContract = false };
            //}
            //else
            //{
            //    request = new MercantileFuturesOrderRequest2 { IsExpiredContract = false, IsCapitalCheckContract = true };
            //}

            var buySellType = holdTable.BuySellTypeId == (int)Types.TransactionDirection.Buying
                                  ? Types.TransactionDirection.Selling
                                  : Types.TransactionDirection.Buying;

            request.BuySell = buySellType;
            request.Code = holdTable.Contract;
            request.FundAccountId = capitalAccount.UserAccountDistributeLogo;
            request.OpenCloseType = closeType2;

            request.OrderAmount = amount;//(float)holdTable.HistoryHoldAmount;

            //和刘丹确认过，不是过期合约平仓或者价格为0时，取当日的涨停价或跌停价报盘，买平用涨停价，卖平用跌停价，add by 董鹏 2010-02-23
            //2010-04-02 和刘丹、苏婷再次讨论，定为先取行情最新价，若取不到在使用涨跌停价 update by 董鹏 2010-04-02
            if (closeType != Types.QHForcedCloseType.Expired || price == 0)
            {
                string errMsg;
                int errcount = 0;

                //获取行情最新成交价
                MarketDataLevel marketData = null;
                while (marketData == null)
                {
                    marketData = RealTimeMarketUtil.GetInstance().GetLastPriceByCode(holdTable.Contract, (int)Types.BreedClassTypeEnum.CommodityFuture, out errMsg);
                    errcount++;
                    if (errcount > 10)
                    {
                        LogHelper.WriteDebug("期货强行平仓，无法获取到行情最新成交价。CloseCommoditiesContract");
                        break;
                    }
                    if (marketData == null)
                    {
                        Thread.Sleep(10000);
                    }
                }
                if (marketData != null && marketData.LastPrice != 0)
                {
                    price = (float)marketData.LastPrice;
                }
                else
                {
                    //取不到行情成交价，取涨跌停板价
                    HighLowRangeValue hlValue = null;

                    while (hlValue == null)
                    {
                        hlValue = MCService.HLRangeProcessor.GetHighLowRangeValueByCommodityCode(holdTable.Contract, 0);
                        errcount++;
                        if (errcount > 10)
                        {
                            LogHelper.WriteDebug("期货强行平仓，无法获取到涨跌停板价格。CloseCommoditiesContract");
                            return;
                        }
                        if (hlValue == null)
                        {
                            Thread.Sleep(10000);
                        }
                    }
                    price = holdTable.BuySellTypeId == (int)Types.TransactionDirection.Buying
                                      ? (float)hlValue.HighRangeValue
                                      : (float)hlValue.LowRangeValue;
                }
            }
            request.OrderPrice = price;
            request.OrderUnitType = Types.UnitType.Hand;
            request.OrderWay = Entity.Contants.Types.OrderPriceType.OPTLimited;

            string format = "FutureDayChecker开盘持仓检查强制平仓[UserAccountDistributeLogo={0},AccountHoldLogoId={1}，Code={2}, Price={3}, 平仓类型={4}]-委托信息：" + request;
            string msg = string.Format(format, holdTable.UserAccountDistributeLogo, holdTable.AccountHoldLogoId, holdTable.Contract, price, type);
            LogHelper.WriteDebug(msg + holdTable);

            UA_UserAccountAllocationTableDal ua_UserAccountAllocationTableDal = new UA_UserAccountAllocationTableDal();
            //设置为其所属的交易员
            var userAccountAllocationTable = ua_UserAccountAllocationTableDal.GetModel(capitalAccount.UserAccountDistributeLogo);
            if (userAccountAllocationTable == null)
            {
                string msg2 = "开盘检查强行平仓失败！无法获取资金账户信息，ID=" + capitalAccount.UserAccountDistributeLogo;
                LogHelper.WriteInfo(msg2);
                return;
            }
            UA_UserBasicInformationTableDal ua_UserBasicInformationTableDal = new UA_UserBasicInformationTableDal();
            var user = ua_UserBasicInformationTableDal.GetModel(userAccountAllocationTable.UserID);
            if (user == null)
            {
                string msg3 = "开盘检查强行平仓失败！无法获取交易员信息，UserID=" + userAccountAllocationTable.UserID;
                LogHelper.WriteInfo(msg3);
                return;
            }

            request.TraderId = user.UserID;
            request.TraderPassword = user.Password;

            orderAccepter.DoMercantileFuturesOrder(request);
        }

        /// <summary>
        /// 查找欲平仓的合约
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<QH_HoldAccountTableInfo> FindWillCLosedContract(List<QH_HoldAccountTableInfo> list)
        {
            List<QH_HoldAccountTableInfo> result = new List<QH_HoldAccountTableInfo>();
            QH_HoldAccountTableInfo temp = null;
            if (list.Count == 1)
            {
                result.Add(list[0]);
                return result;
            }
            // SortedList<

            for (int i = 0; i < list.Count - 1; i++) //降序排序
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (list[i].HistoryHoldAmount < list[j].HistoryHoldAmount)
                    {
                        temp = list[j];
                        list[j] = list[i];
                        list[i] = temp;
                    }
                }
            }
            Dictionary<string, List<QH_HoldAccountTableInfo>> _dictMaxQhAccountTable = null;
            if (list[0].HistoryHoldAmount == list[1].HistoryHoldAmount) //不存在最大的合约
            {
                _dictMaxQhAccountTable = new Dictionary<string, List<QH_HoldAccountTableInfo>>();
                List<QH_HoldAccountTableInfo> temp1 = null;
                temp1 = new List<QH_HoldAccountTableInfo>();
                temp1.Add(list[0]);
                _dictMaxQhAccountTable.Add(list[0].Contract, temp1);
                for (int m = 0; m < list.Count - 1; m++)
                {
                    if (list[m].HistoryHoldAmount == list[m + 1].HistoryHoldAmount)
                    {
                        if (!_dictMaxQhAccountTable.TryGetValue(list[m + 1].Contract, out temp1))
                        {
                            temp1 = new List<QH_HoldAccountTableInfo>();
                            temp1.Add(list[m + 1]);
                            _dictMaxQhAccountTable.Add(list[m + 1].Contract, temp1);
                        }
                        else
                            _dictMaxQhAccountTable[list[m + 1].Contract].Add(list[m + 1]);
                    }
                    else
                        break;
                }
                //比较静头寸亏损最大的合约
                List<string> contractArry = new List<string>(_dictMaxQhAccountTable.Keys);

                decimal kProfitloss = 0;
                decimal nProfitloss = 0;
                string temp2 = "";
                for (int k = 0; k < contractArry.Count - 1; k++)
                {
                    for (int n = k + 1; n < contractArry.Count; n++)
                    {
                        kProfitloss = getProfitLoss(_dictMaxQhAccountTable[contractArry[k]]);
                        nProfitloss = getProfitLoss(_dictMaxQhAccountTable[contractArry[n]]);
                        if (kProfitloss < nProfitloss)
                        {
                            temp2 = contractArry[n];
                            contractArry[n] = contractArry[k];
                            contractArry[k] = temp2;
                        }
                    }
                }

                return _dictMaxQhAccountTable[contractArry[0]];
            }
            else
            {
                result.Add(list[0]);
                return result;
            }

            //return null;
        }

        /// <summary>
        /// Desc: 获取超过最大持仓量的记录
        /// Create by: 董鹏
        /// Create Date: 2010-02-03
        /// </summary>
        /// <param name="list">商品期货持仓实体列表</param>
        /// <param name="amtList">超量列表</param>
        /// <returns></returns>
        private List<QH_HoldAccountTableInfo> FindWillClosedContractOverMaxHoldLimit(List<QH_HoldAccountTableInfo> list, out Dictionary<int, decimal> dicAmount)
        {
            dicAmount = new Dictionary<int, decimal>();
            List<QH_HoldAccountTableInfo> listReturn = new List<QH_HoldAccountTableInfo>();
            LogHelper.WriteDebug("---->商品期货持仓限制验证");
            foreach (QH_HoldAccountTableInfo item in list)
            {
                //获取持仓限制
                try
                {
                    PositionLimitValueInfo posInfo = MCService.GetPositionLimit(item.Contract);
                    LogHelper.WriteDebug("-------->获取持仓限制，HistoryHoldAmount=" + item.HistoryHoldAmount + ",TodayHoldAmount=" + item.TodayHoldAmount + ",pLimit=" + posInfo.PositionValue + ",IsNoComputer=" + posInfo.IsNoComputer +
                        ",IsMinMultiple=" + posInfo.IsMinMultiple + ",MinMultipleValue=" + posInfo.MinMultipleValue);
                    Decimal pLimit = posInfo.PositionValue;
                    if (pLimit < 0)
                    {
                        LogHelper.WriteDebug("-------->商品期货持仓限制验证，持仓限制小于0，不处理，pLimit=" + pLimit);
                        continue;
                    }
                    //持仓大于持仓限制
                    if (item.HistoryHoldAmount > pLimit)
                    {
                        LogHelper.WriteDebug("-------->持仓大于持仓限制，UserAccountDistributeLogo=" + item.UserAccountDistributeLogo + ",Contract=" +
                            item.Contract + ",HistoryHoldAmount=" + item.HistoryHoldAmount + ",TodayHoldAmount=" + item.TodayHoldAmount + ",pLimit=" + pLimit + ",HistoryHoldAmount - pLimit=" + (item.HistoryHoldAmount - pLimit));
                        listReturn.Add(item);
                        dicAmount.Add(item.AccountHoldLogoId, item.HistoryHoldAmount - pLimit);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }

            return listReturn;
        }

        /// <summary>
        /// Desc: 获取超过最小交割量整数倍的记录
        /// Create by: 董鹏
        /// Create Date: 2010-02-03
        /// </summary>
        /// <param name="list">商品期货持仓实体列表</param>
        /// <param name="amtList">超量列表</param>
        /// <returns></returns>
        private List<QH_HoldAccountTableInfo> FindWillClosedContractNotModMinUnitLimit(List<QH_HoldAccountTableInfo> list, out Dictionary<int, decimal> dicAmount)
        {
            dicAmount = new Dictionary<int, decimal>();
            List<QH_HoldAccountTableInfo> listReturn = new List<QH_HoldAccountTableInfo>();
            LogHelper.WriteDebug("---->商品期货最小交割量整数倍限制验证");
            foreach (QH_HoldAccountTableInfo item in list)
            {
                //获取持仓限制
                try
                {
                    PositionLimitValueInfo posInfo = MCService.GetPositionLimit(item.Contract);
                    LogHelper.WriteDebug("-------->获取持仓限制，HistoryHoldAmount=" + item.HistoryHoldAmount + ",TodayHoldAmount=" + item.TodayHoldAmount + ",pLimit=" + posInfo.PositionValue + ",IsNoComputer=" + posInfo.IsNoComputer +
                        ",IsMinMultiple=" + posInfo.IsMinMultiple + ",MinMultipleValue=" + posInfo.MinMultipleValue);
                    if (posInfo.IsMinMultiple)
                    {
                        if (item.HistoryHoldAmount % posInfo.MinMultipleValue > 0)
                        {
                            LogHelper.WriteDebug("-------->持仓超过最小交割量整数倍限制，UserAccountDistributeLogo=" + item.UserAccountDistributeLogo + ",Contract=" +
                            item.Contract + ",HistoryHoldAmount=" + item.HistoryHoldAmount + ",TodayHoldAmount=" + item.TodayHoldAmount + ",MinMultipleValue=" +
                            posInfo.MinMultipleValue + "HistoryHoldAmount % pLimit=" + (item.HistoryHoldAmount % posInfo.MinMultipleValue));
                            listReturn.Add(item);
                            dicAmount.Add(item.AccountHoldLogoId, (item.HistoryHoldAmount % posInfo.MinMultipleValue));
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }
            return listReturn;
        }

        /// <summary>
        /// 获取持仓总盈亏
        /// </summary>
        /// <param name="listHold">持仓账户表</param>
        /// <returns></returns>
        private decimal getProfitLoss(List<QH_HoldAccountTableInfo> listHold)
        {
            decimal kProfitloss = 0;
            foreach (var _holdEntity in listHold)
            {
                kProfitloss += _holdEntity.ProfitLoss;
            }
            return kProfitloss;
        }

        /// <summary>
        /// 获取指定Id、币种的持仓账户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currencyType"></param>
        /// <returns></returns>
        private List<QH_HoldAccountTableInfo> GetHoldAccountTableList(string id, int? currencyType)
        {
            QH_HoldAccountTableDal dal = new QH_HoldAccountTableDal();
            string where = string.Format("UserAccountDistributeLogo = '{0}' AND TradeCurrencyType= '{1}'",
                                         id, currencyType);
            // return DataRepository.QhHoldAccountTableProvider.Find(where);
            return dal.GetListArray(where);
        }
    }

    /// <summary>
    /// 股指期货下单契约扩展
    /// </summary>
    public class StockIndexFuturesOrderRequest2 : StockIndexFuturesOrderRequest
    {
        ///// <summary>
        ///// 是否是过期的合约生成的委托
        ///// </summary>
        //public bool IsExpiredContract { get; set; }

        ///// <summary>
        ///// 是否是资金检查需要平仓的委托
        ///// </summary>
        //public bool IsCapitalCheckContract { get; set; }

        /// <summary>
        /// 是否是盘前检查强行平仓委托
        /// </summary>
        public bool IsForcedCloseOrder { get; set; }
        /// <summary>
        /// 期货强行平仓类型
        /// </summary>
        public Types.QHForcedCloseType QHForcedCloseType { get; set; }
    }
    /// <summary>
    /// 商品期货扩展类
    /// </summary>
    public class MercantileFuturesOrderRequest2 : MercantileFuturesOrderRequest
    {
        ///// <summary>
        ///// 是否是过期的合约生成的委托
        ///// </summary>
        //public bool IsExpiredContract { get; set; }

        ///// <summary>
        ///// 是否是资金检查需要平仓的委托
        ///// </summary>
        //public bool IsCapitalCheckContract { get; set; }
        ///// <summary>
        ///// 是否是超过持仓限制需要平仓的委托
        ///// </summary>
        //public bool IsOverHoldLimitContract { get; set; }
        ///// <summary>
        ///// 是否是不为最小交割单位整数倍需要平仓的委托
        ///// </summary>
        //public bool IsNotModMinUnitContract { get; set; }

        /// <summary>
        /// 是否是盘前检查强行平仓委托
        /// </summary>
        public bool IsForcedCloseOrder { get; set; }
        /// <summary>
        /// 期货强行平仓类型
        /// </summary>
        public Types.QHForcedCloseType QHForcedCloseType { get; set; }
    }


}