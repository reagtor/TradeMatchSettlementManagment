#region Using Namespace

using System;
using System.Collections.Generic;
using System.Data.Common;
using GTA.VTS.Common.CommonUtility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.delegateoffer;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.BLL.ScheduleManagement;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.DAL.Data.HK;
using ReckoningCounter.Entity.Contants;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.Model;
using ReckoningCounter.DAL.CustomDataAccess;
using System.Threading;
using ReckoningCounter.MemoryData;
using CommonObject = GTA.VTS.Common.CommonObject;

#endregion

namespace ReckoningCounter.BLL.MarketClose
{
    /// <summary>
    /// 盘后清算服务(解冻）
    /// 作者：宋涛
    /// 日期：2008-12-22
    /// </summary>
    public class ReckoningService
    {
        /// <summary>
        /// 进行现货盘后清算
        /// </summary>
        public static bool ProcessStock()
        {
            StockReckoning stockReckoning = new StockReckoning();
            return stockReckoning.Process();
        }

        /// <summary>
        /// 进行港股盘后清算
        /// </summary>
        public static bool ProcessHK()
        {
            HKReckoning hkReckoning = new HKReckoning();
            return hkReckoning.Process();
        }


        /// <summary>
        /// 进行期货盘后清算
        /// </summary>
        public static bool ProcessFuture()
        {
            FutureReckoning futureReckoning = new FutureReckoning();
            return futureReckoning.Process();
        }

        /// <summary>
        /// 今天是否是解冻日期前一天
        /// 因为是在盘后进行解冻动作，所以明天是解冻日期的话，今天盘后进行解冻动作
        /// </summary>
        /// <param name="date">解冻日期</param>
        /// <returns>是否</returns>
        public static bool IsUnFreezeDate(DateTime date)
        {
            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);

            DateTime unFreezeDate = date.AddDays(-1);
            unFreezeDate = new DateTime(unFreezeDate.Year, unFreezeDate.Month, unFreezeDate.Day, 0, 0, 0);

            //if (date.Year == now.Year && date.Month == now.Month && date.Day == now.Day)
            //    return true;
            TimeSpan ts = now - unFreezeDate;
            if (ts.TotalDays >= 0)
                return true;

            return false;
        }
    }

    /// <summary>
    /// 现货清算类
    /// </summary>
    public class StockReckoning
    {
        #region old code 李健华 2010-06-08
        //protected VTTraders vtTraders;

        //public StockReckoning()
        //{
        //    VTTradersFactory.Reset();
        //    vtTraders = VTTradersFactory.GetStockTraders();
        //}

        ///// <summary>
        ///// 进行盘后清算
        ///// </summary>
        //public bool Process()
        //{
        //    if (!vtTraders.IsInitializeSuccess)
        //    {
        //        LogHelper.WriteDebug("VTTraders没有初始化，无法进行清算！");
        //        return false;
        //    }

        //    bool result = false;
        //    LogHelper.WriteInfo("开始进行现货盘后清算");

        //    //未报、待报的委托状态改为废单,已报待撤、已报、部成、部成待撤的走撤单流程
        //    ProcessTodayEntrustStatus();

        //    //=============
        //    //add 李健华 因为盘后清算可能做内部撤单比后面的慢所以会产生冻结资无法即时解冻
        //    //所以这里做了撤单后做延时一分钟
        //    if (ScheduleManager.IsStartSuccess)
        //    {
        //        LogHelper.WriteInfo("现货进行盘后清算延时一分钟操作请稍后....");
        //        Thread.CurrentThread.Join(60000);
        //    }

        //    //add 李健华   2009-12-16
        //    //先关闭内存管理器，提交资金到数据库，因为后面的清算都是从数据库中清算的.
        //    MemoryDataManager.End();
        //    //=====================

        //    bool hasFailure = false;

        //    for (int i = 0; i < vtTraders.TraderList.Count; i++)
        //    {
        //        var trader = vtTraders.TraderList[i];
        //        string traderFormat = "现货盘后清算进度————当前交易员[UserID={0}]";
        //        string traderMsg = string.Format(traderFormat, trader.Trader.UserID);
        //        LogHelper.WriteDebug(traderMsg);

        //        for (int j = 0; j < trader.AccountPairList.Count; j++)
        //        {
        //            var accountPair = trader.AccountPairList[j];

        //            //如果账户类型是港股，那么不处理
        //            if (accountPair.CapitalAccount.AccountTypeLogo == (int)CommonObject.Types.AccountType.HKSpotCapital)
        //                continue;

        //            string processFormat = "现货盘后清算进度————当前/总交易员数量：[{0}/{1}] 当前/当前交易员总帐户数量：[{2}/{3}]";
        //            string processMsg = string.Format(processFormat, i + 1, vtTraders.TraderList.Count, j + 1,
        //                                              trader.AccountPairList.Count);
        //            LogHelper.WriteDebug(processMsg);

        //            //var accountPair = trader.AccountPairList[j];

        //            bool isSuccess = DoReckoning(accountPair.CapitalAccount, accountPair.HoldAccount);

        //            if (!isSuccess)
        //                hasFailure = true;
        //        }
        //    }

        //    if (!hasFailure)
        //    {
        //        try
        //        {
        //            CheckAllFreezeMoney();
        //            CheckAllFreezeHold();

        //            CheckAllFreezeMoney2();
        //            CheckAllFreezeHold2();

        //            StatusTableChecker.UpdateStockReckoningDate(null);
        //            LogHelper.WriteInfo("现货盘后清算成功！");
        //            result = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            LogHelper.WriteError(ex.Message, ex);
        //        }

        //        //ReckoningTransaction tm = new ReckoningTransaction();
        //        //Database database = DatabaseFactory.CreateDatabase();
        //        //using (DbConnection connection = database.CreateConnection())
        //        //{
        //        //    try
        //        //    {
        //        //        connection.Open();
        //        //        tm.Database = database;
        //        //        DbTransaction transaction = connection.BeginTransaction();
        //        //        tm.Transaction = transaction;
        //        //        StatusTableChecker.UpdateStockReckoningDate(tm);

        //        //        tm.Transaction.Commit();
        //        //        LogHelper.WriteInfo("现货盘后清算成功！");
        //        //        result = true;
        //        //    }
        //        //    catch (Exception ex)
        //        //    {
        //        //        LogHelper.WriteError(ex.Message, ex);
        //        //        tm.Transaction.Rollback();
        //        //    }
        //        //}
        //    }
        //    //这里后面还有分红处理不能在这里就加载内存表
        //    ////清算完毕后再重新加载内存资金表内容,为后面其他清算做预委托下单的处理同样操作内存表的数据
        //    ////内存表管理
        //    //MemoryDataManager.Start();
        //    return result;
        //}
        #endregion

        #region new 李健华 2010-06-08
        /// <summary>
        /// 所有资金账号与持仓账号配对对
        /// </summary>
        protected List<AccountPair> userAccountPair;
        /// <summary>
        /// 构造函数(内部带有初始化处理)
        /// </summary>
        public StockReckoning()
        {
            userAccountPair = new List<AccountPair>();

            //获取所有现货资金账号类型
            List<BD_AccountTypeInfo> accountTypes = AccountManager.Instance.GetAccoutTypeByACTID((int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.SpotCapital);

            if (Utils.IsNullOrEmpty(accountTypes))
            {
                return;
            }
            foreach (var item in accountTypes)
            {
                //过虑掉不是现货的数据（如港股）
                if (item.AccountTypeLogo != (int)GTA.VTS.Common.CommonObject.Types.AccountType.StockSpotCapital)
                {
                    continue;
                }
                List<AccountPair> list = VTTradersFactory.InitializeAccountPair(item);
                if (!Utils.IsNullOrEmpty(list))
                {
                    userAccountPair.AddRange(list);
                }
            }
        }

        /// <summary>
        /// 进行盘后清算
        /// </summary>
        public bool Process()
        {
            if (Utils.IsNullOrEmpty(userAccountPair))
            {
                LogHelper.WriteDebug("VTTraders没有初始化，无法进行清算！");
                return false;
            }

            bool result = false;
            LogHelper.WriteInfo("开始进行现货盘后清算");

            //未报、待报的委托状态改为废单,已报待撤、已报、部成、部成待撤的走撤单流程
            ProcessTodayEntrustStatus();

            //=============
            //add 李健华 因为盘后清算可能做内部撤单比后面的慢所以会产生冻结资无法即时解冻
            //所以这里做了撤单后做延时一分钟
            if (ScheduleManager.IsStartSuccess)
            {
                LogHelper.WriteInfo("现货进行盘后清算延时一分钟操作请稍后....");
                Thread.CurrentThread.Join(60000);
            }

            //add 李健华   2009-12-16
            //先关闭内存管理器，提交资金到数据库，因为后面的清算都是从数据库中清算的.
            MemoryDataManager.End();
            //=====================

            bool hasFailure = false;


            for (int j = 0; j < userAccountPair.Count; j++)
            {
                var accountPair = userAccountPair[j];

                string traderFormat = "现货盘后清算进度————当前交易员[UserID={0}]";
                string traderMsg = string.Format(traderFormat, accountPair.CapitalAccount.UserID);
                LogHelper.WriteDebug(traderMsg);

                //如果账户类型是港股，那么不处理
                if (accountPair.CapitalAccount.AccountTypeLogo == (int)CommonObject.Types.AccountType.HKSpotCapital)
                {
                    continue;
                }

                string processFormat = "现货盘后清算进度————当前/总交易员数量：[{0}/{1}] 当前/当前交易员总帐户数量：[{2}/{3}]";
                string processMsg = string.Format(processFormat, j + 1, userAccountPair.Count, 1, 1);
                LogHelper.WriteDebug(processMsg);

                bool isSuccess = DoReckoning(accountPair.CapitalAccount, accountPair.HoldAccount);

                if (!isSuccess)
                    hasFailure = true;
            }


            if (!hasFailure)
            {
                try
                {
                    CheckAllFreezeMoney();
                    CheckAllFreezeHold();

                    CheckAllFreezeMoney2();
                    CheckAllFreezeHold2();

                    StatusTableChecker.UpdateStockReckoningDate(null);
                    LogHelper.WriteInfo("现货盘后清算成功！");
                    result = true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }

                //ReckoningTransaction tm = new ReckoningTransaction();
                //Database database = DatabaseFactory.CreateDatabase();
                //using (DbConnection connection = database.CreateConnection())
                //{
                //    try
                //    {
                //        connection.Open();
                //        tm.Database = database;
                //        DbTransaction transaction = connection.BeginTransaction();
                //        tm.Transaction = transaction;
                //        StatusTableChecker.UpdateStockReckoningDate(tm);

                //        tm.Transaction.Commit();
                //        LogHelper.WriteInfo("现货盘后清算成功！");
                //        result = true;
                //    }
                //    catch (Exception ex)
                //    {
                //        LogHelper.WriteError(ex.Message, ex);
                //        tm.Transaction.Rollback();
                //    }
                //}
            }
            //这里后面还有分红处理不能在这里就加载内存表
            ////清算完毕后再重新加载内存资金表内容,为后面其他清算做预委托下单的处理同样操作内存表的数据
            ////内存表管理
            //MemoryDataManager.Start();
            return result;
        }
        #endregion


        /// <summary>
        /// 检查冻结记录对应的委托是否已经是最终状态，如果是，那么这条冻结记录要解冻
        /// </summary>
        private void CheckAllFreezeMoney2()
        {
            XH_CapitalAccountFreezeTableDal dal = new XH_CapitalAccountFreezeTableDal();
            var list = dal.GetAllFreezeMoney();

            XH_CapitalAccountTableDal capitalDal = new XH_CapitalAccountTableDal();
            foreach (var freezeSum in list)
            {
                if (freezeSum.FreezeCapitalSum == 0)
                    continue;

                try
                {
                    var capitalAccountTable = capitalDal.GetModel(freezeSum.CapitalAccountLogo);
                    ProcessCapitalFreezeMoney2(capitalAccountTable);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// 删除所有没有对应持仓的冻结
        /// </summary>
        private void CheckAllFreezeHold2()
        {
            XH_AccountHoldTableDal holdDal = new XH_AccountHoldTableDal();
            XH_AcccountHoldFreezeTableDal freezeDal = new XH_AcccountHoldFreezeTableDal();

            var freezeList = freezeDal.GetAllListArray();
            foreach (var freezeTableInfo in freezeList)
            {
                int accountHoldId = freezeTableInfo.AccountHoldLogo;

                if (!holdDal.Exists(accountHoldId))
                    freezeDal.Delete(freezeTableInfo.HoldFreezeLogoId);
            }
        }

        private void ProcessCapitalFreezeMoney2(XH_CapitalAccountTableInfo capitalTable)
        {
            int capitalAccountLogo = capitalTable.CapitalAccountLogo;
            XH_CapitalAccountFreezeTableDal dal = new XH_CapitalAccountFreezeTableDal();
            string where = string.Format("CapitalAccountLogo = '{0}' AND freezetypelogo={1}", capitalAccountLogo,
                                         (int)Types.FreezeType.DelegateFreeze);
            var freezeList = dal.GetListArray(where);
            if (Utils.IsNullOrEmpty(freezeList))
                return;

            var freezeList2 = new List<XH_CapitalAccountFreezeTableInfo>();
            decimal allSum = 0;
            foreach (var freezeTable in freezeList)
            {
                decimal famount = freezeTable.FreezeCapitalAmount;

                decimal fcost = freezeTable.FreezeCost;

                decimal sum = fcost + famount;

                if (sum == 0)
                    continue;

                bool isDone = HasDoneEntrust(freezeTable.EntrustNumber);
                if (isDone)
                {
                    freezeList2.Add(freezeTable);
                    allSum += sum;
                }
            }

            if (Utils.IsNullOrEmpty(freezeList2))
            {
                UpdateCapitalTable(capitalTable, freezeList2, allSum);
            }
        }

        private void UpdateCapitalTable(XH_CapitalAccountTableInfo capitalTable,
                                        List<XH_CapitalAccountFreezeTableInfo> freezeTables,
                                        decimal freezeSum)
        {
            capitalTable.AvailableCapital += freezeSum;
            capitalTable.FreezeCapitalTotal -= freezeSum;

            XH_CapitalAccountFreezeTableDal capitalDal = new XH_CapitalAccountFreezeTableDal();
            XH_CapitalAccountTableDal xhCapitalAccountTableDal = new XH_CapitalAccountTableDal();

            DataManager.ExecuteInTransaction(tm =>
                                                 {
                                                     foreach (var freezeTable in freezeTables)
                                                     {
                                                         capitalDal.Clear(freezeTable.CapitalFreezeLogoId, tm.Database,
                                                                          tm.Transaction);
                                                     }

                                                     xhCapitalAccountTableDal.Update(capitalTable, tm.Database,
                                                                                     tm.Transaction);

                                                     string format2 =
                                                         "******************StockReckoning.UpdateCapitalTable修正现货冻结资金[剩余冻结资金={0}]";
                                                     string desc = string.Format(format2, freezeSum);
                                                     LogHelper.WriteDebug(desc);
                                                 });
        }

        private bool HasDoneEntrust(string entrustNumber)
        {
            XH_TodayEntrustTableDal xhTodayEntrustTableDal = new XH_TodayEntrustTableDal();
            var tet = xhTodayEntrustTableDal.GetModel(entrustNumber);

            //如果找不到，那么这笔冻结记录更应该解冻
            if (tet == null)
            {
                return true;
            }

            int state = tet.OrderStatusId;
            if (state < 0)
                return false;

            //如果状态时废单，部撤，已撤，已成，那么代表是最终状态
            if (state == (int)Types.OrderStateType.DOSCanceled || state == (int)Types.OrderStateType.DOSPartRemoved
                || state == (int)Types.OrderStateType.DOSRemoved || state == (int)Types.OrderStateType.DOSDealed)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 检查持仓的总冻结是否等于所有冻结记录的总和
        /// </summary>
        private void CheckAllFreezeHold()
        {
            XH_AccountHoldTableDal dal = new XH_AccountHoldTableDal();
            var list = dal.GetAllListArray();
            foreach (var hold in list)
            {
                try
                {
                    int accountHoldId = hold.AccountHoldLogoId;

                    XH_AcccountHoldFreezeTableDal freezeTableDal = new XH_AcccountHoldFreezeTableDal();
                    int sum = freezeTableDal.GetAllFreezeAmount(accountHoldId);

                    if (hold.FreezeAmount != sum)
                    {
                        string format =
                            "ReckoningService.StockReckoning.CheckAllFreezeHold[AccountHoldLogoId={0},FreezeAmount={1},TrueSum={2}]";
                        string desc = string.Format(format, accountHoldId, hold.FreezeAmount, sum);
                        LogHelper.WriteDebug(desc);

                        hold.FreezeAmount = sum;
                        dal.Update(hold);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }
        }


        /// <summary>
        /// 检查总冻结资金是否等于所有冻结记录的总和
        /// </summary>
        private void CheckAllFreezeMoney()
        {
            XH_CapitalAccountFreezeTableDal dal = new XH_CapitalAccountFreezeTableDal();
            var list = dal.GetAllFreezeMoney();
            foreach (var freezeSum in list)
            {
                try
                {
                    InternalCheckAllFreezeMoney(freezeSum);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }

            /*
            try
            {
                XH_CapitalAccountTableDal xhCapitalAccountTableDal = new XH_CapitalAccountTableDal();
                var capitals = xhCapitalAccountTableDal.GetListArray(string.Empty);
                foreach (XH_CapitalAccountTableInfo capitalAccountTable in capitals)
                {
                    ProcessCapitalFreezeMoney(capitalAccountTable);
                }

                //LogHelper.WriteInfo("现货盘后冻结资金清算成功！");
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }*/
        }

        private void InternalCheckAllFreezeMoney(XH_CapitalAccountFreezeSum freezeSum)
        {
            int capitalAccountId = freezeSum.CapitalAccountLogo;
            decimal sum = freezeSum.FreezeCapitalSum;

            XH_CapitalAccountTableDal dal = new XH_CapitalAccountTableDal();

            var capital = dal.GetModel(capitalAccountId);

            if (capital.FreezeCapitalTotal != sum)
            {
                decimal oldSum = capital.FreezeCapitalTotal;

                capital.FreezeCapitalTotal = sum;
                dal.Update(capital);

                string format2 = "StockReckoning.CheckAllFreezeMoney修正现货总冻结资金[初始总冻结资金={0},实际总冻结资金={1},资金账户ID={2}]";
                string desc = string.Format(format2, oldSum, sum, capitalAccountId);
                LogHelper.WriteDebug(desc);
            }
        }

        private void ProcessCapitalFreezeMoney(XH_CapitalAccountTableInfo capitalTable)
        {
            int capitalAccountLogo = capitalTable.CapitalAccountLogo;

            string format1 = "select sum(freezecapitalamount) + sum(freezecost) from xh_capitalAccountfreezetable "
                             + "where capitalaccountlogo={0} and freezetypelogo={1}";
            string sql1 = string.Format(format1, capitalAccountLogo, (int)Types.FreezeType.DelegateFreeze);

            object obj = DbHelperSQL.ExecuteSqlScalar(sql1);
            //DataRepository.Provider.ExecuteScalar(CommandType.Text, sql1);
            if (obj == null)
                return;

            int sum = 0;
            bool isSuccess = int.TryParse(obj.ToString().Trim(), out sum);
            if (!isSuccess)
                return;
            if (capitalTable.FreezeCapitalTotal == sum)
                return;

            string strSql =
                string.Format(
                    "UPDATE [XH_CapitalAccountTable] SET [FreezeCapitalTotal] = {0} "
                    + "WHERE CapitalAccountLogo = {1}", sum, capitalAccountLogo);

            //修正为实际的总和
            //capitalTable.FreezeCapitalTotal = sum;

            // ReckoningTransaction tm = TransactionFactory.GetTransactionManager();

            try
            {
                //tm.BeginTransaction();
                //XHDataAccess.XHCapitalAccountProcess(strSql, tm);
                //DataRepository.XhCapitalAccountTableProvider.Update(tm, capitalTable);
                // tm.Commit();
                DbHelperSQL.ExecuteSql(strSql);

                string format2 = "ReckoningService.ProcessCapitalFreezeMoney修正现货总冻结资金[初始总冻结资金={0},实际总冻结资金={1}]";
                string desc = string.Format(format2, capitalTable.FreezeCapitalTotal, sum);
                LogHelper.WriteDebug(desc);
            }
            catch (Exception ex)
            {
                // tm.Rollback();
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 1.检查是否有【委托量=成交量+撤单量】但是状态没有被修改的
        /// 2.未报、待报的委托状态改为废单,已报待撤、已报、部成、部成待撤的走撤单流程
        /// </summary>
        private static void ProcessTodayEntrustStatus()
        {
            //1.检查是否有【委托量=成交量+撤单量】但是状态没有被修改的
            CheckTodayDealStatus();

            //2.未报、待报的委托状态改为废单,已报待撤、已报、部成、部成待撤的走撤单流程
            //TODO:未报、待报的委托状态改为废单,已报待撤、已报、部成、部成待撤的走撤单流程
            /*/// 未报
            [EnumMember] DOSUnRequired = 2,
            /// 待报
            [EnumMember] DOSRequiredSoon = 3,
            /// 已报待撤
            [EnumMember] DOSRequiredRemoveSoon = 4,
            /// 已报
            [EnumMember] DOSIsRequired = 5,
            /// 废单
            [EnumMember] DOSCanceled = 6,
            /// 部成
            [EnumMember] DOSPartDealed = 9,
            /// 部成待撤
            [EnumMember] DOSPartDealRemoveSoon = 11,
            /// 部撤
            //[EnumMember] DOSPartRemoved = 8,
             */
            int DOSUnRequired = (int)Types.OrderStateType.DOSUnRequired;
            int DOSRequiredSoon = (int)Types.OrderStateType.DOSRequiredSoon;
            int DOSRequiredRemoveSoon = (int)Types.OrderStateType.DOSRequiredRemoveSoon;
            int DOSIsRequired = (int)Types.OrderStateType.DOSIsRequired;
            //int DOSCanceled = (int)Types.OrderStateType.DOSCanceled;
            int DOSPartDealed = (int)Types.OrderStateType.DOSPartDealed;
            int DOSPartDealRemoveSoon = (int)Types.OrderStateType.DOSPartDealRemoveSoon;
            int DOSPartRemoved = (int)Types.OrderStateType.DOSPartRemoved;

            //未报,待报也要走内部撤单流程
            ProcessOrderStatus(DOSUnRequired);
            ProcessOrderStatus(DOSRequiredSoon);

            //string format1 =
            //    "update xh_todayentrusttable set orderstatusid={0} where orderstatusid={1} or orderstatusid={2}";
            //string sql1 = string.Format(format1, DOSCanceled, DOSUnRequired, DOSRequiredSoon);

            //// ReckoningTransaction tm = TransactionFactory.GetTransactionManager();
            //// tm.BeginTransaction();

            //try
            //{
            //    DbHelperSQL.ExecuteSql(sql1);
            //    // DataRepository.Provider.ExecuteNonQuery(tm, CommandType.Text, sql1);
            //    //tm.Commit();
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.WriteError("", ex);
            //    // tm.Rollback();
            //}

            //已报待撤、已报、部成，部成待撤,部撤,走内部撤单流程(还有已成，已撤2种状态走的是正常清算流程)

            ProcessOrderStatus(DOSRequiredRemoveSoon);
            ProcessOrderStatus(DOSIsRequired);
            ProcessOrderStatus(DOSPartDealed);
            ProcessOrderStatus(DOSPartDealRemoveSoon);
            ProcessOrderStatus(DOSPartRemoved);
            //string format2 = "OrderStatusId = '{0}' OR OrderStatusId = '{1}' OR OrderStatusId = '{2}' OR OrderStatusId = '{3}'";
            //string where = string.Format(format2, DOSRequiredRemoveSoon, DOSIsRequired, DOSPartDealed, DOSPartDealRemoveSoon);

            //var list = DataRepository.XhTodayEntrustTableProvider.Find(where);
            //if (list == null)
            //    return;

            //string message = "盘后撤单，委托作废";
            //foreach (XhTodayEntrustTable tet in list)
            //{
            //    OrderOfferCenter.Instance.IntelnalCancelXHOrder(tet, message);
            //}
        }

        //检查是否有【委托量=成交量+撤单量】但是状态没有被修改的
        private static void CheckTodayDealStatus()
        {
            string where =
                "EntrustAmount=(TradeAmount+CancelAmount) and not (OrderStatusId=6 or OrderStatusId=7 or OrderStatusId=8 or OrderStatusId=10)";

            List<XH_TodayEntrustTableInfo> list = null;

            try
            {
                XH_TodayEntrustTableDal dal = new XH_TodayEntrustTableDal();
                //list = XhTodayEntrustTableDao.GetListArray(where);
                list = dal.GetListArray(where);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            if (Utils.IsNullOrEmpty(list))
                return;

            List<XH_TodayEntrustTableInfo> list2 = new List<XH_TodayEntrustTableInfo>();

            foreach (XH_TodayEntrustTableInfo table in list)
            {
                if (table.EntrustAmount == table.TradeAmount)
                    table.OrderStatusId = (int)Types.OrderStateType.DOSDealed;
                else if (table.EntrustAmount == table.CancelAmount)
                    table.OrderStatusId = (int)Types.OrderStateType.DOSRemoved;
                else
                    table.OrderStatusId = (int)Types.OrderStateType.DOSPartRemoved;

                list2.Add(table);
            }

            //ReckoningTransaction tm = TransactionFactory.GetTransactionManager();
            //tm.BeginTransaction();
            ReckoningTransaction tm = new ReckoningTransaction();
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                try
                {
                    //DataRepository.XhTodayEntrustTableProvider.Update(tm, list2);
                    //tm.Commit();
                    connection.Open();
                    tm.Database = database;
                    DbTransaction transaction = connection.BeginTransaction();
                    tm.Transaction = transaction;
                    XH_TodayEntrustTableDal xh_TodayEntrustTableDal = new XH_TodayEntrustTableDal();
                    foreach (XH_TodayEntrustTableInfo table in list2)
                    {
                        xh_TodayEntrustTableDal.Update(table, tm.Database,
                                                       tm.Transaction);
                    }
                    tm.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError("", ex);
                    tm.Transaction.Rollback();
                }
            }
        }

        private static void ProcessOrderStatus(int status)
        {
            string format = "OrderStatusId={0}";
            string where = string.Format(format, status);

            List<XH_TodayEntrustTableInfo> list = null;
            XH_TodayEntrustTableDal xhTodayEntrustTableDal = new XH_TodayEntrustTableDal();
            bool findSuccess = false;
            try
            {
                list = xhTodayEntrustTableDal.GetListArray(where);
                findSuccess = true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            if (!findSuccess)
            {
                try
                {
                    list = xhTodayEntrustTableDal.GetListArrayWithNoLock(where);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }

            if (Utils.IsNullOrEmpty(list))
                return;

            string format2 = "现货盘后撤单，委托作废[OrderStatusID={0},Count={1}]";
            string message = string.Format(format2, status, list.Count);
            LogHelper.WriteDebug(message);

            List<XH_TodayEntrustTableInfo> unDoneList = new List<XH_TodayEntrustTableInfo>();
            foreach (XH_TodayEntrustTableInfo tet in list)
            {
                bool isSuccess = OrderOfferCenter.Instance.IntelnalCancelXHOrder(tet, message);
                if (!isSuccess)
                    unDoneList.Add(tet);
            }

            //上次未成功执行的再执行一次
            foreach (XH_TodayEntrustTableInfo table in unDoneList)
            {
                bool isSuccess = OrderOfferCenter.Instance.IntelnalCancelXHOrder(table, message);
                if (!isSuccess)
                {
                    LogHelper.WriteInfo("ReckoningService.ProcessOrderStatus现货盘后撤单失败，委托单号=" + table.EntrustNumber);
                }
            }
        }

        /// <summary>
        /// 对每一个用户下的每一对资金和持仓进行清算
        /// </summary>
        /// <param name="capitalAccount">资金账户</param>
        /// <param name="holdAccount">持仓账户</param>
        private bool DoReckoning(UA_UserAccountAllocationTableInfo capitalAccount,
                                 UA_UserAccountAllocationTableInfo holdAccount)
        {
            //bool result = false;
            string format = "现货盘后清算DoReckoning[资金账户={0},持仓账户={1}]";
            string msg = string.Format(format, capitalAccount.UserAccountDistributeLogo,
                                       holdAccount.UserAccountDistributeLogo);
            LogHelper.WriteDebug(msg);

            ProcessCapital(capitalAccount);
            ProcessHold(holdAccount);

            //ReckoningTransaction tm = TransactionFactory.GetTransactionManager();
            //tm.BeginTransaction();

            //try
            //{
            //    ProcessCapital(tm, capitalAccount);

            //    tm.Commit();
            //    result = true;
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.WriteError("", ex);
            //    tm.Rollback();
            //}

            //if (result)
            //    ProcessHold(holdAccount);

            return true;
        }

        #region 清算资金

        /// <summary>
        /// 清算资金
        /// </summary>
        /// <param name="capitalAccount"></param>
        private void ProcessCapital(UA_UserAccountAllocationTableInfo capitalAccount)
        {
            XH_CapitalAccountTableDal dal = new XH_CapitalAccountTableDal();

            string where = string.Format("UserAccountDistributeLogo = '{0}' ", capitalAccount.UserAccountDistributeLogo);
            List<XH_CapitalAccountTableInfo> list = dal.GetListArray(where);
            // DataRepository.XhCapitalAccountTableProvider.GetByUserAccountDistributeLogo(
            //    capitalAccount.UserAccountDistributeLogo);

            if (Utils.IsNullOrEmpty(list))
                return;

            //每一个资金账户有多个币种的资金表,每个币种一条记录
            foreach (var capitalAccountTable in list)
            {
                //如果当前资金记录没有冻结量，那么不处理
                //if (capitalAccountTable.FreezeCapitalTotal <=0.00m)
                //    continue;

                if (capitalAccountTable.FreezeCapitalTotal == 0)
                    continue;

                ProcessCapitalByCurrency(capitalAccountTable);
                //ReckoningTransaction tm = TransactionFactory.GetTransactionManager();
                //tm.BeginTransaction();
                //try
                //{
                //    ProcessCapitalByCurrency(tm, capitalAccountTable);
                //    tm.Commit();
                //}
                //catch (Exception ex)
                //{
                //    LogHelper.WriteError("清算资金出现问题 ", ex);
                //    tm.Rollback();
                //    throw ex;
                //    //return false;
                //}
            }
        }


        /// <summary>
        /// 按币种对资金进行清算
        /// </summary>
        /// <param name="tm"></param>
        /// <param name="capitalAccountTable"></param>
        private void ProcessCapitalByCurrency(ReckoningTransaction tm, XH_CapitalAccountTableInfo capitalAccountTable)
        {
            XH_CapitalAccountFreezeTableDal dal = new XH_CapitalAccountFreezeTableDal();

            string where = string.Format("CapitalAccountLogo = '{0}' ", capitalAccountTable.CapitalAccountLogo);
            List<XH_CapitalAccountFreezeTableInfo> freezeTables = dal.GetListArray(where);
            //DataRepository.XhCapitalAccountFreezeTableProvider.GetByCapitalAccountLogo(
            //   capitalAccountTable.CapitalAccountLogo);

            if (Utils.IsNullOrEmpty(freezeTables))
                return;

            decimal freezeMoney = 0;

            //需要更新的冻结表（从所有的冻结表中选出）
            List<XH_CapitalAccountFreezeTableInfo> updateFreezeTables = new List<XH_CapitalAccountFreezeTableInfo>();
            foreach (var freezeTable in freezeTables)
            {
                int freezeTypeLogo = freezeTable.FreezeTypeLogo;
                //if (freezeTypeLogo<0.00m)
                //    continue;

                switch (freezeTypeLogo)
                {
                    //委托冻结，直接解冻
                    case (int)Types.FreezeType.DelegateFreeze:
                        freezeMoney += GetUnFreezeMoney(freezeTable);
                        updateFreezeTables.Add(freezeTable);
                        break;

                    //清算冻结，需要判断解冻日期
                    case (int)Types.FreezeType.ReckoningFreeze:


                        DateTime thawTime = (DateTime)freezeTable.ThawTime;
                        if (ReckoningService.IsUnFreezeDate(thawTime))
                        {
                            freezeMoney += GetUnFreezeMoney(freezeTable);
                            updateFreezeTables.Add(freezeTable);
                        }

                        break;
                }
            }

            ///更新资金冻结表(直接删除解冻的记录-冻结金额和冻结费用都已经为0）
            if (updateFreezeTables.Count > 0)
            {
                //DataRepository.XhCapitalAccountFreezeTableProvider.Update(tm, updateFreezeTables);
                //dal.Delete();
                // DataRepository.XhCapitalAccountFreezeTableProvider.Delete(tm, updateFreezeTables);
                foreach (var freezeTable in updateFreezeTables)
                {
                    dal.Delete(freezeTable.CapitalFreezeLogoId, tm);
                }
            }


            ///更新资金表
            //if (capitalAccountTable.AvailableCapital <=0.00m)
            //    capitalAccountTable.AvailableCapital = 0;

            capitalAccountTable.AvailableCapital += freezeMoney;

            //if (capitalAccountTable.FreezeCapitalTotal <=0.00m)
            //    capitalAccountTable.FreezeCapitalTotal = 0;
            capitalAccountTable.FreezeCapitalTotal -= freezeMoney;
            if (capitalAccountTable.FreezeCapitalTotal < 0)
                capitalAccountTable.FreezeCapitalTotal = 0;

            capitalAccountTable.CapitalBalance = capitalAccountTable.AvailableCapital +
                                                 capitalAccountTable.FreezeCapitalTotal;

            // DataRepository.XhCapitalAccountTableProvider.Update(tm, capitalAccountTable);
            XH_CapitalAccountTableDal xhCapitalAccountTableDal = new XH_CapitalAccountTableDal();
            xhCapitalAccountTableDal.Update(capitalAccountTable, tm.Database, tm.Transaction);
        }

        /// <summary>
        /// 按币种对资金进行清算
        /// </summary>
        /// <param name="capitalAccountTable"></param>
        private void ProcessCapitalByCurrency(XH_CapitalAccountTableInfo capitalAccountTable)
        {
            XH_CapitalAccountFreezeTableDal dal = new XH_CapitalAccountFreezeTableDal();
            string where = string.Format("CapitalAccountLogo = '{0}' ", capitalAccountTable.CapitalAccountLogo);
            List<XH_CapitalAccountFreezeTableInfo> freezeTables = dal.GetListArray(where);
            // DataRepository.XhCapitalAccountFreezeTableProvider.GetByCapitalAccountLogo(
            //    capitalAccountTable.CapitalAccountLogo);

            if (Utils.IsNullOrEmpty(freezeTables))
                return;

            decimal freezeMoney = 0;

            //需要更新的冻结表（从所有的冻结表中选出）
            List<XH_CapitalAccountFreezeTableInfo> updateFreezeTables = new List<XH_CapitalAccountFreezeTableInfo>();
            foreach (var freezeTable in freezeTables)
            {
                int freezeTypeLogo = freezeTable.FreezeTypeLogo;
                //if (freezeTypeLogo<0.00m)
                //    continue;

                switch (freezeTypeLogo)
                {
                    //委托冻结，直接解冻
                    case (int)Types.FreezeType.DelegateFreeze:
                        freezeMoney += GetUnFreezeMoney(freezeTable);
                        updateFreezeTables.Add(freezeTable);
                        break;

                    //清算冻结，需要判断解冻日期
                    case (int)Types.FreezeType.ReckoningFreeze:


                        DateTime thawTime = (DateTime)freezeTable.ThawTime;
                        if (ReckoningService.IsUnFreezeDate(thawTime))
                        {
                            freezeMoney += GetUnFreezeMoney(freezeTable);
                            updateFreezeTables.Add(freezeTable);
                        }

                        break;
                }
            }

            #region 进行更新

            //ReckoningTransaction tm = TransactionFactory.GetTransactionManager();
            //tm.BeginTransaction();
            ReckoningTransaction tm = new ReckoningTransaction();
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                try
                {
                    connection.Open();
                    tm.Database = database;
                    DbTransaction transaction = connection.BeginTransaction();
                    tm.Transaction = transaction;
                    ///更新资金冻结表(直接删除解冻的记录-冻结金额和冻结费用都已经为0）
                    if (updateFreezeTables.Count > 0)
                    {
                        //DataRepository.XhCapitalAccountFreezeTableProvider.Update(tm, updateFreezeTables);
                        XH_CapitalAccountFreezeTableDal xhCapitalAccountFreezeTableDal =
                            new XH_CapitalAccountFreezeTableDal();
                        foreach (var freezeTable in updateFreezeTables)
                        {
                            xhCapitalAccountFreezeTableDal.Delete(freezeTable.CapitalFreezeLogoId, tm);
                        }

                        //DataRepository.XhCapitalAccountFreezeTableProvider.Delete(tm, updateFreezeTables);
                    }


                    ///更新资金表
                    //if (capitalAccountTable.AvailableCapital<0.00m)
                    //    capitalAccountTable.AvailableCapital = 0;

                    capitalAccountTable.AvailableCapital += freezeMoney;

                    //if (capitalAccountTable.FreezeCapitalTotal<=0.00m)
                    //    capitalAccountTable.FreezeCapitalTotal = 0;
                    capitalAccountTable.FreezeCapitalTotal -= freezeMoney;
                    if (capitalAccountTable.FreezeCapitalTotal < 0)
                        capitalAccountTable.FreezeCapitalTotal = 0;

                    capitalAccountTable.CapitalBalance = capitalAccountTable.AvailableCapital +
                                                         capitalAccountTable.FreezeCapitalTotal;
                    XH_CapitalAccountTableDal xhCapitalAccountTableDal = new XH_CapitalAccountTableDal();
                    xhCapitalAccountTableDal.Update(capitalAccountTable, tm.Database,
                                                    tm.Transaction);
                    //DataRepository.XhCapitalAccountTableProvider.Update(tm, capitalAccountTable);
                    //tm.Commit();
                    tm.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                    //tm.Rollback();
                    tm.Transaction.Rollback();
                }
            }

            #endregion
        }

        private static decimal GetUnFreezeMoney(XH_CapitalAccountFreezeTableInfo freezeTable)
        {
            decimal result = 0;

            //if (freezeTable.FreezeCapitalAmount <=0.00m)
            //    freezeTable.FreezeCapitalAmount = 0;
            //if (freezeTable.FreezeCost<=0.00m)
            //    freezeTable.FreezeCost = 0;

            result = freezeTable.FreezeCapitalAmount + freezeTable.FreezeCost;

            //获取值后赋零
            freezeTable.FreezeCapitalAmount = 0;
            freezeTable.FreezeCost = 0;

            return result;
        }

        #endregion

        #region 清算持仓

        /// <summary>
        /// 清算持仓
        /// </summary>
        /// <param name="tm"></param>
        /// <param name="holdAccount"></param>
        protected void ProcessHold(ReckoningTransaction tm, UA_UserAccountAllocationTableInfo holdAccount)
        {
            XH_AccountHoldTableDal xhAccountHoldTableDal = new XH_AccountHoldTableDal();
            string where = string.Format("UserAccountDistributeLogo = '{0}'", holdAccount.UserAccountDistributeLogo);
            List<XH_AccountHoldTableInfo> list = xhAccountHoldTableDal.GetListArray(where);
            //DataRepository.XhAccountHoldTableProvider.GetByUserAccountDistributeLogo(
            //    holdAccount.UserAccountDistributeLogo);

            if (Utils.IsNullOrEmpty(list))
                return;

            for (int i = 0; i < list.Count; i++)
            {
                XH_AccountHoldTableInfo accountHoldTable = list[i];
                //如果当前持仓记录没有冻结量，那么不处理
                //if (accountHoldTable.FreezeAmount <=0.00m)
                //    continue;

                if (accountHoldTable.FreezeAmount == 0)
                    continue;

                //ReckoningTransaction tm2 = TransactionFactory.GetTransactionManager();
                //tm2.BeginTransaction();
                ReckoningTransaction reckoningTransaction = new ReckoningTransaction();
                Database database = DatabaseFactory.CreateDatabase();
                using (DbConnection connection = database.CreateConnection())
                {
                    try
                    {
                        connection.Open();
                        reckoningTransaction.Database = database;
                        DbTransaction transaction = connection.BeginTransaction();
                        reckoningTransaction.Transaction = transaction;
                        ProcessHoldByCode(reckoningTransaction, accountHoldTable);
                        //tm2.Commit();
                        reckoningTransaction.Transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteError("清算持仓出现问题 ", ex);
                        reckoningTransaction.Transaction.Rollback();
                        //tm2.Rollback();
                        throw ex;
                        //return false;
                    }
                }
                //tm2.Commit();
            }

            //IDictionary<string, List<XH_AccountHoldTableInfo>> stockCurrencyList =
            //    new Dictionary<string, List<XH_AccountHoldTableInfo>>();


            ////每一个持仓账户有多个股票的持仓记录, 每个股票只有一条记录
            //foreach (var accountHoldTable in list)
            //{
            //    string code = accountHoldTable.Code;

            //    if (stockCurrencyList.ContainsKey(code))
            //    {
            //        stockCurrencyList[code] = new List<XH_AccountHoldTableInfo>();
            //    }

            //    stockCurrencyList[code].Add(accountHoldTable);
            //}

            ////对每一个股票进行处理
            //foreach (var key in stockCurrencyList.Keys)
            //{
            //    var codeList = stockCurrencyList[key];

            //    //每一个股票，对应每个币种有一条记录
            //    foreach (XH_AccountHoldTableInfo holdTableByCodeAndCurrency in codeList)
            //    {
            //        ProcessHoldByCode(tm, holdTableByCodeAndCurrency);
            //    }
            //}
        }

        /// <summary>
        /// 清算持仓
        /// </summary>
        /// <param name="holdAccount"></param>
        protected void ProcessHold(UA_UserAccountAllocationTableInfo holdAccount)
        {
            XH_AccountHoldTableDal xhAccountHoldTableDal = new XH_AccountHoldTableDal();
            string where = string.Format("UserAccountDistributeLogo = '{0}'", holdAccount.UserAccountDistributeLogo);
            List<XH_AccountHoldTableInfo> list =
                xhAccountHoldTableDal.GetListArray(where);

            if (Utils.IsNullOrEmpty(list))
                return;

            for (int i = 0; i < list.Count; i++)
            {
                XH_AccountHoldTableInfo accountHoldTable = list[i];
                //如果当前持仓记录没有冻结量，那么不处理
                //if (accountHoldTable.FreezeAmount<0)
                //    continue;

                if (accountHoldTable.FreezeAmount == 0)
                    continue;

                ProcessHoldByCode(accountHoldTable);
            }
        }


        /// <summary>
        /// 按股票对持仓进行清算
        /// </summary>
        /// <param name="tm"></param>
        /// <param name="holdAccountTable"></param>
        private void ProcessHoldByCode(ReckoningTransaction tm, XH_AccountHoldTableInfo holdAccountTable)
        {
            string where = string.Format("AccountHoldLogo = '{0}'", holdAccountTable.AccountHoldLogoId);
            XH_AcccountHoldFreezeTableDal xhAcccountHoldFreezeTableDal = new XH_AcccountHoldFreezeTableDal();
            List<XH_AcccountHoldFreezeTableInfo> freezeTables = xhAcccountHoldFreezeTableDal.GetListArray(where);
            //DataRepository.XhAcccountHoldFreezeTableProvider.GetByAccountHoldLogo(
            //    holdAccountTable.AccountHoldLogoId);

            if (Utils.IsNullOrEmpty(freezeTables))
                return;

            int freezeHold = 0;
            List<XH_AcccountHoldFreezeTableInfo> updateFreezeTables = new List<XH_AcccountHoldFreezeTableInfo>();
            foreach (var freezeTable in freezeTables)
            {
                int freezeTypeLogo = freezeTable.FreezeTypeLogo;
                //if (freezeTypeLogo<0.00m)
                //    continue;

                //if (freezeTable.PrepareFreezeAmount <=0.00m)
                //    continue;

                if (freezeTable.PrepareFreezeAmount == 0)
                    continue;

                switch (freezeTypeLogo)
                {
                    case (int)Types.FreezeType.DelegateFreeze:
                        //if (freezeTable.PrepareFreezeAmount<=0.00m)
                        //    freezeTable.PrepareFreezeAmount = 0;

                        freezeHold += freezeTable.PrepareFreezeAmount;

                        updateFreezeTables.Add(freezeTable);
                        break;

                    //清算冻结，需要判断解冻日期
                    case (int)Types.FreezeType.ReckoningFreeze:


                        DateTime thawTime = (DateTime)freezeTable.ThawTime;
                        if (ReckoningService.IsUnFreezeDate(thawTime))
                        {
                            //if (freezeTable.PrepareFreezeAmount<=0.00m)
                            //    freezeTable.PrepareFreezeAmount = 0;

                            freezeHold += freezeTable.PrepareFreezeAmount;

                            updateFreezeTables.Add(freezeTable);
                        }

                        break;
                }
            }

            ///更新持仓冻结表
            if (updateFreezeTables.Count > 0)
            {
                //DataRepository.XhAcccountHoldFreezeTableProvider.Update(tm, updateFreezeTables);
                //DataRepository.XhAcccountHoldFreezeTableProvider.Delete(tm, updateFreezeTables);
                //XH_AcccountHoldFreezeTableDal xh_AcccountHoldFreezeTableDal = new XH_AcccountHoldFreezeTableDal();
                foreach (var table in updateFreezeTables)
                {
                    xhAcccountHoldFreezeTableDal.Update(table, tm.Database, tm.Transaction);
                }
            }

            ///更新持仓表
            //if (holdAccountTable.AvailableAmount<=0.00m)
            //    holdAccountTable.AvailableAmount = 0;

            holdAccountTable.AvailableAmount += freezeHold;

            //if (holdAccountTable.FreezeAmount<=0.00m)
            //    holdAccountTable.FreezeAmount = 0;
            holdAccountTable.FreezeAmount -= freezeHold;
            if (holdAccountTable.FreezeAmount < 0)
                holdAccountTable.FreezeAmount = 0;
            XH_AccountHoldTableDal xhAccountHoldTableDal = new XH_AccountHoldTableDal();
            xhAccountHoldTableDal.Update(holdAccountTable, tm.Database, tm.Transaction);
            // DataRepository.XhAccountHoldTableProvider.Update(tm, holdAccountTable);
        }

        /// <summary>
        /// 按股票对持仓进行清算
        /// </summary>
        /// <param name="holdAccountTable"></param>
        private void ProcessHoldByCode(XH_AccountHoldTableInfo holdAccountTable)
        {
            XH_AcccountHoldFreezeTableDal xhAcccountHoldFreezeTableDal = new XH_AcccountHoldFreezeTableDal();
            string where = string.Format("AccountHoldLogo = '{0}'", holdAccountTable.AccountHoldLogoId);
            List<XH_AcccountHoldFreezeTableInfo> freezeTables = xhAcccountHoldFreezeTableDal.GetListArray(where);

            if (Utils.IsNullOrEmpty(freezeTables))
                return;

            int freezeHold = 0;
            List<XH_AcccountHoldFreezeTableInfo> updateFreezeTables = new List<XH_AcccountHoldFreezeTableInfo>();
            foreach (var freezeTable in freezeTables)
            {
                int freezeTypeLogo = freezeTable.FreezeTypeLogo;
                //if (freezeTypeLogo<0.00m)
                //    continue;

                //if (freezeTable.PrepareFreezeAmount<0.00m)
                //    continue;

                if (freezeTable.PrepareFreezeAmount == 0)
                    continue;

                switch (freezeTypeLogo)
                {
                    case (int)Types.FreezeType.DelegateFreeze:


                        //if (freezeTable.PrepareFreezeAmount<0.00m)
                        //    freezeTable.PrepareFreezeAmount = 0;

                        freezeHold += freezeTable.PrepareFreezeAmount;

                        updateFreezeTables.Add(freezeTable);
                        break;

                    //清算冻结，需要判断解冻日期
                    case (int)Types.FreezeType.ReckoningFreeze:


                        DateTime thawTime = (DateTime)freezeTable.ThawTime;
                        if (ReckoningService.IsUnFreezeDate(thawTime))
                        {
                            //if (freezeTable.PrepareFreezeAmount<0.00m)
                            //    freezeTable.PrepareFreezeAmount = 0;

                            freezeHold += freezeTable.PrepareFreezeAmount;

                            updateFreezeTables.Add(freezeTable);
                        }

                        break;
                }
            }

            #region 进行更新

            xhAcccountHoldFreezeTableDal = new XH_AcccountHoldFreezeTableDal();
            XH_AccountHoldTableDal xhAccountHoldTableDal = new XH_AccountHoldTableDal();

            try
            {
                DataManager.ExecuteInTransaction(rt =>
                                                     {
                                                         ///更新持仓冻结表
                                                         if (updateFreezeTables.Count > 0)
                                                         {
                                                             //DataRepository.XhAcccountHoldFreezeTableProvider.Update(tm, updateFreezeTables);
                                                             foreach (var info in updateFreezeTables)
                                                             {
                                                                 xhAcccountHoldFreezeTableDal.DeleteRecord(
                                                                     info.HoldFreezeLogoId, rt.Database, rt.Transaction);
                                                             }
                                                         }

                                                         ///更新持仓表
                                                         //if (holdAccountTable.AvailableAmount<0.00m)
                                                         //    holdAccountTable.AvailableAmount = 0;

                                                         holdAccountTable.AvailableAmount += freezeHold;

                                                         //if (holdAccountTable.FreezeAmount<0.00m)
                                                         //    holdAccountTable.FreezeAmount = 0;
                                                         holdAccountTable.FreezeAmount -= freezeHold;
                                                         //if (holdAccountTable.FreezeAmount < 0)
                                                         //    holdAccountTable.FreezeAmount = 0;
                                                         //XH_AccountHoldTableDal xh_AccountHoldTableDal = new XH_AccountHoldTableDal();
                                                         xhAccountHoldTableDal.UpdateRecord(holdAccountTable,
                                                                                            rt.Database, rt.Transaction);
                                                     });
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }


            //ReckoningTransaction tm = new ReckoningTransaction();
            //Database database = DatabaseFactory.CreateDatabase();
            //DbConnection connection = database.CreateConnection();
            //connection.Open();
            //tm.Database = database;
            //DbTransaction transaction = connection.BeginTransaction();
            //tm.Transaction = transaction;

            //xh_AcccountHoldFreezeTableDal = new XH_AcccountHoldFreezeTableDal();
            //try
            //{
            //    ///更新持仓冻结表
            //    if (updateFreezeTables.Count > 0)
            //    {
            //        //DataRepository.XhAcccountHoldFreezeTableProvider.Update(tm, updateFreezeTables);
            //        foreach (var info in updateFreezeTables)
            //        {
            //            xh_AcccountHoldFreezeTableDal.DeleteRecord(info.HoldFreezeLogoId, tm.Database, tm.Transaction);
            //        }

            //    }

            //    ///更新持仓表
            //    //if (holdAccountTable.AvailableAmount<0.00m)
            //    //    holdAccountTable.AvailableAmount = 0;

            //    holdAccountTable.AvailableAmount += freezeHold;

            //    //if (holdAccountTable.FreezeAmount<0.00m)
            //    //    holdAccountTable.FreezeAmount = 0;
            //    holdAccountTable.FreezeAmount -= freezeHold;
            //    //if (holdAccountTable.FreezeAmount < 0)
            //    //    holdAccountTable.FreezeAmount = 0;
            //    XH_AccountHoldTableDal xh_AccountHoldTableDal = new XH_AccountHoldTableDal();
            //    xh_AccountHoldTableDal.Update(holdAccountTable, tm.Database, tm.Transaction);
            //    tm.Transaction.Commit();
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.WriteError(ex.Message, ex);
            //    tm.Transaction.Rollback();
            //}

            #endregion
        }

        #endregion
    }

    /// <summary>
    /// 港股清算类
    /// </summary>
    public class HKReckoning
    {
        #region old code 李健华 2010-06-08
        //protected VTTraders vtTraders;

        //public HKReckoning()
        //{
        //    VTTradersFactory.Reset();
        //    vtTraders = VTTradersFactory.GetStockTraders();
        //}

        ///// <summary>
        ///// 进行盘后清算
        ///// </summary>
        //public bool Process()
        //{
        //    if (!vtTraders.IsInitializeSuccess)
        //    {
        //        LogHelper.WriteDebug("VTTraders没有初始化，无法进行清算！");
        //        return false;
        //    }

        //    bool result = false;
        //    LogHelper.WriteInfo("开始进行港股盘后清算");

        //    //未报、待报的委托状态改为废单,已报待撤、已报、部成、部成待撤的走撤单流程
        //    ProcessTodayEntrustStatus();

        //    //=============
        //    //add 李健华 因为盘后清算可能做内部撤单比后面的慢所以会产生冻结资无法即时解冻
        //    //所以这里做了撤单后做延时一分钟
        //    if (ScheduleManager.IsStartSuccess)
        //    {
        //        LogHelper.WriteInfo("港股进行盘后清算延时一分钟操作请稍后....");
        //        Thread.CurrentThread.Join(60000);
        //    }
        //    //==============

        //    //add 李健华   2009-12-16
        //    //先关闭内存管理器，提交资金到数据库，因为后面的清算都是从数据库中清算的.
        //    MemoryDataManager.End();
        //    //=====================

        //    bool hasFailure = false;

        //    for (int i = 0; i < vtTraders.TraderList.Count; i++)
        //    {
        //        var trader = vtTraders.TraderList[i];
        //        string traderFormat = "港股盘后清算进度————当前交易员[UserID={0}]";
        //        string traderMsg = string.Format(traderFormat, trader.Trader.UserID);
        //        LogHelper.WriteDebug(traderMsg);

        //        for (int j = 0; j < trader.AccountPairList.Count; j++)
        //        {
        //            var accountPair = trader.AccountPairList[j];

        //            //如果账户类型不是港股，那么不处理
        //            if (accountPair.CapitalAccount.AccountTypeLogo != (int)CommonObject.Types.AccountType.HKSpotCapital)
        //                continue;

        //            string processFormat = "港股盘后清算进度————当前/总交易员数量：[{0}/{1}] 当前/当前交易员总帐户数量：[{2}/{3}]";
        //            string processMsg = string.Format(processFormat, i + 1, vtTraders.TraderList.Count, j + 1,
        //                                              trader.AccountPairList.Count);
        //            LogHelper.WriteDebug(processMsg);

        //            bool isSuccess = DoReckoning(accountPair.CapitalAccount, accountPair.HoldAccount);

        //            if (!isSuccess)
        //                hasFailure = true;
        //        }
        //    }

        //    if (!hasFailure)
        //    {
        //        try
        //        {
        //            CheckAllFreezeMoney();
        //            CheckAllFreezeHold();

        //            CheckAllFreezeMoney2();
        //            CheckAllFreezeHold2();

        //            StatusTableChecker.UpdateHKReckoningDate(null);
        //            LogHelper.WriteInfo("港股盘后清算成功！");
        //            result = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            LogHelper.WriteError(ex.Message, ex);
        //        }

        //        //ReckoningTransaction tm = new ReckoningTransaction();
        //        //Database database = DatabaseFactory.CreateDatabase();
        //        //using (DbConnection connection = database.CreateConnection())
        //        //{
        //        //    try
        //        //    {
        //        //        connection.Open();
        //        //        tm.Database = database;
        //        //        DbTransaction transaction = connection.BeginTransaction();
        //        //        tm.Transaction = transaction;
        //        //        StatusTableChecker.UpdateStockReckoningDate(tm);

        //        //        tm.Transaction.Commit();
        //        //        LogHelper.WriteInfo("现货盘后清算成功！");
        //        //        result = true;
        //        //    }
        //        //    catch (Exception ex)
        //        //    {
        //        //        LogHelper.WriteError(ex.Message, ex);
        //        //        tm.Transaction.Rollback();
        //        //    }
        //        //}
        //    }
        //    //这里后面还有分红处理不能在这里就加载内存表
        //    ////清算完毕后再重新加载内存资金表内容,为后面其他清算做预委托下单的处理同样操作内存表的数据
        //    ////内存表管理
        //    //MemoryDataManager.Start();
        //    return result;

        //}
        #endregion

        #region new  李健华 2010-06-08
        /// <summary>
        /// 资金账户配对实体
        /// </summary>
        protected List<AccountPair> userAccountPair;

        /// <summary>
        /// 构造函数(内部带有初始化处理)
        /// </summary>
        public HKReckoning()
        {
            userAccountPair = new List<AccountPair>();
            //VTTradersFactory.Reset();
            //vtTraders = VTTradersFactory.GetStockTraders();
            //获取所有现货资金账号类型
            List<BD_AccountTypeInfo> accountTypes = AccountManager.Instance.GetAccoutTypeByACTID((int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.SpotCapital);

            if (Utils.IsNullOrEmpty(accountTypes))
            {
                return;
            }
            foreach (var item in accountTypes)
            {
                //过虑掉不是港股的现货
                if (item.AccountTypeLogo != (int)GTA.VTS.Common.CommonObject.Types.AccountType.HKSpotCapital)
                {
                    continue;
                }
                List<AccountPair> list = VTTradersFactory.InitializeAccountPair(item);
                if (!Utils.IsNullOrEmpty(list))
                {
                    userAccountPair.AddRange(list);
                }
            }
        }

        /// <summary>
        /// 进行盘后清算
        /// </summary>
        public bool Process()
        {
            if (Utils.IsNullOrEmpty(userAccountPair))
            {
                LogHelper.WriteDebug("VTTraders没有初始化，无法进行清算！");
                return false;
            }

            bool result = false;
            LogHelper.WriteInfo("开始进行港股盘后清算");

            //未报、待报的委托状态改为废单,已报待撤、已报、部成、部成待撤的走撤单流程
            ProcessTodayEntrustStatus();

            //=============
            //add 李健华 因为盘后清算可能做内部撤单比后面的慢所以会产生冻结资无法即时解冻
            //所以这里做了撤单后做延时一分钟
            if (ScheduleManager.IsStartSuccess)
            {
                LogHelper.WriteInfo("港股进行盘后清算延时一分钟操作请稍后....");
                Thread.CurrentThread.Join(60000);
            }
            //==============

            //add 李健华   2009-12-16
            //先关闭内存管理器，提交资金到数据库，因为后面的清算都是从数据库中清算的.
            MemoryDataManager.End();
            //=====================

            bool hasFailure = false;



            for (int j = 0; j < userAccountPair.Count; j++)
            {
                var accountPair = userAccountPair[j];

                string traderFormat = "港股盘后清算进度————当前交易员[UserID={0}]—当前/总交易员数量：[{1}/{2}] 当前/当前交易员总帐户数量：[{3}/{4}]";
                string traderMsg = string.Format(traderFormat, accountPair.CapitalAccount.UserID, j + 1, userAccountPair.Count, 1, 1);
                LogHelper.WriteDebug(traderMsg);

                //这里其实不用再处理，初始化的时候已经处理了
                //如果账户类型不是港股，那么不处理
                if (accountPair.CapitalAccount.AccountTypeLogo != (int)CommonObject.Types.AccountType.HKSpotCapital)
                {
                    continue;
                }

                //string processFormat = "港股盘后清算进度————当前/总交易员数量：[{0}/{1}] 当前/当前交易员总帐户数量：[{2}/{3}]";
                //string processMsg = string.Format(processFormat, j + 1, userAccountPair.Count, 1, 1);
                //LogHelper.WriteDebug(processMsg);

                bool isSuccess = DoReckoning(accountPair.CapitalAccount, accountPair.HoldAccount);

                if (!isSuccess)
                    hasFailure = true;
            }


            if (!hasFailure)
            {
                try
                {
                    CheckAllFreezeMoney();
                    CheckAllFreezeHold();

                    CheckAllFreezeMoney2();
                    CheckAllFreezeHold2();

                    StatusTableChecker.UpdateHKReckoningDate(null);
                    LogHelper.WriteInfo("港股盘后清算成功！");
                    result = true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }

                //ReckoningTransaction tm = new ReckoningTransaction();
                //Database database = DatabaseFactory.CreateDatabase();
                //using (DbConnection connection = database.CreateConnection())
                //{
                //    try
                //    {
                //        connection.Open();
                //        tm.Database = database;
                //        DbTransaction transaction = connection.BeginTransaction();
                //        tm.Transaction = transaction;
                //        StatusTableChecker.UpdateStockReckoningDate(tm);

                //        tm.Transaction.Commit();
                //        LogHelper.WriteInfo("现货盘后清算成功！");
                //        result = true;
                //    }
                //    catch (Exception ex)
                //    {
                //        LogHelper.WriteError(ex.Message, ex);
                //        tm.Transaction.Rollback();
                //    }
                //}
            }
            //这里后面还有分红处理不能在这里就加载内存表
            ////清算完毕后再重新加载内存资金表内容,为后面其他清算做预委托下单的处理同样操作内存表的数据
            ////内存表管理
            //MemoryDataManager.Start();
            return result;

        }
        #endregion


        /// <summary>
        /// 检查冻结记录对应的委托是否已经是最终状态，如果是，那么这条冻结记录要解冻
        /// </summary>
        private void CheckAllFreezeMoney2()
        {
            HK_CapitalAccountFreezeDal dal = new HK_CapitalAccountFreezeDal();
            var list = dal.GetAllFreezeMoney();

            HK_CapitalAccountDal capitalDal = new HK_CapitalAccountDal();
            foreach (var freezeSum in list)
            {
                if (freezeSum.FreezeCapitalSum == 0)
                    continue;

                try
                {
                    var capitalAccountTable = capitalDal.GetModel(freezeSum.CapitalAccountLogo);
                    ProcessCapitalFreezeMoney2(capitalAccountTable);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// 删除所有没有对应持仓的冻结
        /// </summary>
        private void CheckAllFreezeHold2()
        {
            HK_AccountHoldDal holdDal = new HK_AccountHoldDal();
            HK_AcccountHoldFreezeDal freezeDal = new HK_AcccountHoldFreezeDal();

            var freezeList = freezeDal.GetAllListArray();
            foreach (var freezeTableInfo in freezeList)
            {
                int accountHoldId = freezeTableInfo.AccountHoldLogo;

                if (!holdDal.Exists(accountHoldId))
                    freezeDal.Delete(freezeTableInfo.HoldFreezeLogoId);
            }
        }

        private void ProcessCapitalFreezeMoney2(HK_CapitalAccountInfo capitalTable)
        {
            int capitalAccountLogo = capitalTable.CapitalAccountLogo;
            HK_CapitalAccountFreezeDal dal = new HK_CapitalAccountFreezeDal();
            string where = string.Format("CapitalAccountLogo = '{0}' AND freezetypelogo={1}", capitalAccountLogo,
                                         (int)Types.FreezeType.DelegateFreeze);
            var freezeList = dal.GetListArray(where);
            if (Utils.IsNullOrEmpty(freezeList))
                return;

            var freezeList2 = new List<HK_CapitalAccountFreezeInfo>();
            decimal allSum = 0;
            foreach (var freezeTable in freezeList)
            {
                decimal famount = freezeTable.FreezeCapitalAmount;

                decimal fcost = freezeTable.FreezeCost;

                decimal sum = fcost + famount;

                if (sum == 0)
                    continue;

                bool isDone = HasDoneEntrust(freezeTable.EntrustNumber);
                if (isDone)
                {
                    freezeList2.Add(freezeTable);
                    allSum += sum;
                }
            }

            if (Utils.IsNullOrEmpty(freezeList2))
            {
                UpdateCapitalTable(capitalTable, freezeList2, allSum);
            }
        }

        private void UpdateCapitalTable(HK_CapitalAccountInfo capitalTable,
                                        List<HK_CapitalAccountFreezeInfo> freezeTables,
                                        decimal freezeSum)
        {
            capitalTable.AvailableCapital += freezeSum;
            capitalTable.FreezeCapitalTotal -= freezeSum;

            HK_CapitalAccountFreezeDal capitalDal = new HK_CapitalAccountFreezeDal();
            HK_CapitalAccountDal xhCapitalAccountTableDal = new HK_CapitalAccountDal();

            DataManager.ExecuteInTransaction(tm =>
                                                 {
                                                     foreach (var freezeTable in freezeTables)
                                                     {
                                                         capitalDal.Clear(freezeTable.CapitalFreezeLogoId, tm.Database,
                                                                          tm.Transaction);
                                                     }

                                                     xhCapitalAccountTableDal.Update(capitalTable, tm.Database,
                                                                                     tm.Transaction);

                                                     string format2 =
                                                         "******************HKReckoning.UpdateCapitalTable修正港股冻结资金[剩余冻结资金={0}]";
                                                     string desc = string.Format(format2, freezeSum);
                                                     LogHelper.WriteDebug(desc);
                                                 });
        }

        private bool HasDoneEntrust(string entrustNumber)
        {
            HK_TodayEntrustDal xhTodayEntrustTableDal = new HK_TodayEntrustDal();
            var tet = xhTodayEntrustTableDal.GetModel(entrustNumber);

            //如果找不到，那么这笔冻结记录更应该解冻
            if (tet == null)
            {
                return true;
            }

            int state = tet.OrderStatusID;
            if (state < 0)
                return false;

            //如果状态时废单，部撤，已撤，已成，那么代表是最终状态
            if (state == (int)Types.OrderStateType.DOSCanceled || state == (int)Types.OrderStateType.DOSPartRemoved
                || state == (int)Types.OrderStateType.DOSRemoved || state == (int)Types.OrderStateType.DOSDealed)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 检查持仓的总冻结是否等于所有冻结记录的总和
        /// </summary>
        private void CheckAllFreezeHold()
        {
            HK_AccountHoldDal dal = new HK_AccountHoldDal();
            var list = dal.GetAllListArray();
            foreach (var hold in list)
            {
                try
                {
                    int accountHoldId = hold.AccountHoldLogoID;

                    HK_AcccountHoldFreezeDal freezeTableDal = new HK_AcccountHoldFreezeDal();
                    int sum = freezeTableDal.GetAllFreezeAmount(accountHoldId);

                    if (hold.FreezeAmount != sum)
                    {
                        string format =
                            "ReckoningService.HKReckoning.CheckAllFreezeHold[AccountHoldLogoId={0},FreezeAmount={1},TrueSum={2}]";
                        string desc = string.Format(format, accountHoldId, hold.FreezeAmount, sum);
                        LogHelper.WriteDebug(desc);

                        hold.FreezeAmount = sum;
                        dal.Update(hold);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }
        }


        /// <summary>
        /// 检查总冻结资金是否等于所有冻结记录的总和
        /// </summary>
        private void CheckAllFreezeMoney()
        {
            HK_CapitalAccountFreezeDal dal = new HK_CapitalAccountFreezeDal();
            var list = dal.GetAllFreezeMoney();
            foreach (var freezeSum in list)
            {
                try
                {
                    InternalCheckAllFreezeMoney(freezeSum);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }

            /*
            try
            {
                XH_CapitalAccountTableDal xhCapitalAccountTableDal = new XH_CapitalAccountTableDal();
                var capitals = xhCapitalAccountTableDal.GetListArray(string.Empty);
                foreach (XH_CapitalAccountTableInfo capitalAccountTable in capitals)
                {
                    ProcessCapitalFreezeMoney(capitalAccountTable);
                }

                //LogHelper.WriteInfo("现货盘后冻结资金清算成功！");
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }*/
        }

        private void InternalCheckAllFreezeMoney(HK_CapitalAccountFreezeSum freezeSum)
        {
            int capitalAccountId = freezeSum.CapitalAccountLogo;
            decimal sum = freezeSum.FreezeCapitalSum;

            HK_CapitalAccountDal dal = new HK_CapitalAccountDal();

            var capital = dal.GetModel(capitalAccountId);

            if (capital.FreezeCapitalTotal != sum)
            {
                decimal oldSum = capital.FreezeCapitalTotal;

                capital.FreezeCapitalTotal = sum;
                dal.Update(capital);

                string format2 = "HKReckoning.CheckAllFreezeMoney修正港股总冻结资金[初始总冻结资金={0},实际总冻结资金={1},资金账户ID={2}]";
                string desc = string.Format(format2, oldSum, sum, capitalAccountId);
                LogHelper.WriteDebug(desc);
            }
        }

        private void ProcessCapitalFreezeMoney(HK_CapitalAccountInfo capitalTable)
        {
            int capitalAccountLogo = capitalTable.CapitalAccountLogo;

            string format1 = "select sum(freezecapitalamount) + sum(freezecost) from hk_capitalAccountfreeze "
                             + "where capitalaccountlogo={0} and freezetypelogo={1}";
            string sql1 = string.Format(format1, capitalAccountLogo, (int)Types.FreezeType.DelegateFreeze);

            object obj = DbHelperSQL.ExecuteSqlScalar(sql1);
            //DataRepository.Provider.ExecuteScalar(CommandType.Text, sql1);
            if (obj == null)
                return;

            int sum = 0;
            bool isSuccess = int.TryParse(obj.ToString().Trim(), out sum);
            if (!isSuccess)
                return;
            if (capitalTable.FreezeCapitalTotal == sum)
                return;

            string strSql =
                string.Format(
                    "UPDATE [HK_CapitalAccount] SET [FreezeCapitalTotal] = {0} "
                    + "WHERE CapitalAccountLogo = {1}", sum, capitalAccountLogo);

            //修正为实际的总和
            //capitalTable.FreezeCapitalTotal = sum;

            // ReckoningTransaction tm = TransactionFactory.GetTransactionManager();

            try
            {
                //tm.BeginTransaction();
                //XHDataAccess.XHCapitalAccountProcess(strSql, tm);
                //DataRepository.XhCapitalAccountTableProvider.Update(tm, capitalTable);
                // tm.Commit();
                DbHelperSQL.ExecuteSql(strSql);

                string format2 = "ReckoningService.ProcessCapitalFreezeMoney修正港股总冻结资金[初始总冻结资金={0},实际总冻结资金={1}]";
                string desc = string.Format(format2, capitalTable.FreezeCapitalTotal, sum);
                LogHelper.WriteDebug(desc);
            }
            catch (Exception ex)
            {
                // tm.Rollback();
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 1.检查是否有【委托量=成交量+撤单量】但是状态没有被修改的
        /// 2.未报、待报的委托状态改为废单,已报待撤、已报、部成、部成待撤的走撤单流程
        /// </summary>
        private static void ProcessTodayEntrustStatus()
        {
            //1.检查是否有【委托量=成交量+撤单量】但是状态没有被修改的
            CheckTodayDealStatus();

            //2.未报、待报的委托状态改为废单,已报待撤、已报、部成、部成待撤的走撤单流程
            //TODO:未报、待报的委托状态改为废单,已报待撤、已报、部成、部成待撤的走撤单流程
            /*/// 未报
            [EnumMember] DOSUnRequired = 2,
            /// 待报
            [EnumMember] DOSRequiredSoon = 3,
            /// 已报待撤
            [EnumMember] DOSRequiredRemoveSoon = 4,
            /// 已报
            [EnumMember] DOSIsRequired = 5,
            /// 废单
            [EnumMember] DOSCanceled = 6,
            /// 部成
            [EnumMember] DOSPartDealed = 9,
            /// 部成待撤
            [EnumMember] DOSPartDealRemoveSoon = 11,
            /// 部撤
            //[EnumMember] DOSPartRemoved = 8,
             */
            int DOSUnRequired = (int)Types.OrderStateType.DOSUnRequired;
            int DOSRequiredSoon = (int)Types.OrderStateType.DOSRequiredSoon;
            int DOSRequiredRemoveSoon = (int)Types.OrderStateType.DOSRequiredRemoveSoon;
            int DOSIsRequired = (int)Types.OrderStateType.DOSIsRequired;
            //int DOSCanceled = (int)Types.OrderStateType.DOSCanceled;
            int DOSPartDealed = (int)Types.OrderStateType.DOSPartDealed;
            int DOSPartDealRemoveSoon = (int)Types.OrderStateType.DOSPartDealRemoveSoon;
            int DOSPartRemoved = (int)Types.OrderStateType.DOSPartRemoved;

            //未报,待报
            //string format1 =
            //    "update hk_todayentrust set orderstatusid={0} where orderstatusid={1} or orderstatusid={2}";
            //string sql1 = string.Format(format1, DOSCanceled, DOSUnRequired, DOSRequiredSoon);

            //// ReckoningTransaction tm = TransactionFactory.GetTransactionManager();
            //// tm.BeginTransaction();

            //try
            //{
            //    DbHelperSQL.ExecuteSql(sql1);
            //    // DataRepository.Provider.ExecuteNonQuery(tm, CommandType.Text, sql1);
            //    //tm.Commit();
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.WriteError("", ex);
            //    // tm.Rollback();
            //}

            //未报,待报也要走内部撤单流程
            ProcessOrderStatus(DOSUnRequired);
            ProcessOrderStatus(DOSRequiredSoon);

            //已报待撤、已报、部成，部成待撤,部撤,走内部撤单流程(还有已成，已撤2种状态走的是正常清算流程)

            ProcessOrderStatus(DOSRequiredRemoveSoon);
            ProcessOrderStatus(DOSIsRequired);
            ProcessOrderStatus(DOSPartDealed);
            ProcessOrderStatus(DOSPartDealRemoveSoon);
            ProcessOrderStatus(DOSPartRemoved);
            //string format2 = "OrderStatusId = '{0}' OR OrderStatusId = '{1}' OR OrderStatusId = '{2}' OR OrderStatusId = '{3}'";
            //string where = string.Format(format2, DOSRequiredRemoveSoon, DOSIsRequired, DOSPartDealed, DOSPartDealRemoveSoon);

            //var list = DataRepository.XhTodayEntrustTableProvider.Find(where);
            //if (list == null)
            //    return;

            //string message = "盘后撤单，委托作废";
            //foreach (XhTodayEntrustTable tet in list)
            //{
            //    OrderOfferCenter.Instance.IntelnalCancelXHOrder(tet, message);
            //}
        }

        //检查是否有【委托量=成交量+撤单量】但是状态没有被修改的
        private static void CheckTodayDealStatus()
        {
            string where =
                "EntrustAmount=(TradeAmount+CancelAmount) and not (OrderStatusId=6 or OrderStatusId=7 or OrderStatusId=8 or OrderStatusId=10)";

            List<HK_TodayEntrustInfo> list = null;

            try
            {
                HK_TodayEntrustDal dal = new HK_TodayEntrustDal();
                //list = XhTodayEntrustTableDao.GetListArray(where);
                list = dal.GetListArray(where);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            if (Utils.IsNullOrEmpty(list))
                return;

            List<HK_TodayEntrustInfo> list2 = new List<HK_TodayEntrustInfo>();

            foreach (HK_TodayEntrustInfo table in list)
            {
                if (table.EntrustAmount == table.TradeAmount)
                    table.OrderStatusID = (int)Types.OrderStateType.DOSDealed;
                else if (table.EntrustAmount == table.CancelAmount)
                    table.OrderStatusID = (int)Types.OrderStateType.DOSRemoved;
                else
                    table.OrderStatusID = (int)Types.OrderStateType.DOSPartRemoved;

                list2.Add(table);
            }

            //ReckoningTransaction tm = TransactionFactory.GetTransactionManager();
            //tm.BeginTransaction();
            ReckoningTransaction tm = new ReckoningTransaction();
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                try
                {
                    //DataRepository.XhTodayEntrustTableProvider.Update(tm, list2);
                    //tm.Commit();
                    connection.Open();
                    tm.Database = database;
                    DbTransaction transaction = connection.BeginTransaction();
                    tm.Transaction = transaction;
                    HK_TodayEntrustDal xh_TodayEntrustTableDal = new HK_TodayEntrustDal();
                    foreach (HK_TodayEntrustInfo table in list2)
                    {
                        xh_TodayEntrustTableDal.Update(table, tm.Database,
                                                       tm.Transaction);
                    }
                    tm.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError("", ex);
                    tm.Transaction.Rollback();
                }
            }
        }

        private static void ProcessOrderStatus(int status)
        {
            string format = "OrderStatusId={0}";
            string where = string.Format(format, status);

            List<HK_TodayEntrustInfo> list = null;
            HK_TodayEntrustDal xhTodayEntrustTableDal = new HK_TodayEntrustDal();
            bool findSuccess = false;
            try
            {
                list = xhTodayEntrustTableDal.GetListArray(where);
                findSuccess = true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            if (!findSuccess)
            {
                try
                {
                    list = xhTodayEntrustTableDal.GetListArrayWithNoLock(where);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }

            if (Utils.IsNullOrEmpty(list))
                return;

            string format2 = "港股盘后撤单，委托作废[OrderStatusID={0},Count={1}]";
            string message = string.Format(format2, status, list.Count);
            LogHelper.WriteDebug(message);

            List<HK_TodayEntrustInfo> unDoneList = new List<HK_TodayEntrustInfo>();
            foreach (HK_TodayEntrustInfo tet in list)
            {
                bool isSuccess = OrderOfferCenter.Instance.IntelnalCancelHKOrder(tet, message);
                if (!isSuccess)
                    unDoneList.Add(tet);
            }

            //上次未成功执行的再执行一次
            foreach (HK_TodayEntrustInfo table in unDoneList)
            {
                bool isSuccess = OrderOfferCenter.Instance.IntelnalCancelHKOrder(table, message);
                if (!isSuccess)
                {
                    LogHelper.WriteInfo("ReckoningService.ProcessOrderStatus港股盘后撤单失败，委托单号=" + table.EntrustNumber);
                }
            }
        }

        /// <summary>
        /// 对每一个用户下的每一对资金和持仓进行清算
        /// </summary>
        /// <param name="capitalAccount">资金账户</param>
        /// <param name="holdAccount">持仓账户</param>
        private bool DoReckoning(UA_UserAccountAllocationTableInfo capitalAccount, UA_UserAccountAllocationTableInfo holdAccount)
        {
            //bool result = false;
            string format = "港股盘后清算DoReckoning[资金账户={0},持仓账户={1}]";
            string msg = string.Format(format, capitalAccount.UserAccountDistributeLogo, holdAccount.UserAccountDistributeLogo);
            LogHelper.WriteDebug(msg);

            ProcessCapital(capitalAccount);
            ProcessHold(holdAccount);

            //ReckoningTransaction tm = TransactionFactory.GetTransactionManager();
            //tm.BeginTransaction();

            //try
            //{
            //    ProcessCapital(tm, capitalAccount);

            //    tm.Commit();
            //    result = true;
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.WriteError("", ex);
            //    tm.Rollback();
            //}

            //if (result)
            //    ProcessHold(holdAccount);

            return true;
        }

        #region 清算资金

        /// <summary>
        /// 清算资金
        /// </summary>
        /// <param name="capitalAccount"></param>
        private void ProcessCapital(UA_UserAccountAllocationTableInfo capitalAccount)
        {
            HK_CapitalAccountDal dal = new HK_CapitalAccountDal();

            string where = string.Format("UserAccountDistributeLogo = '{0}' ", capitalAccount.UserAccountDistributeLogo);
            List<HK_CapitalAccountInfo> list = dal.GetListArray(where);
            // DataRepository.XhCapitalAccountTableProvider.GetByUserAccountDistributeLogo(
            //    capitalAccount.UserAccountDistributeLogo);

            if (Utils.IsNullOrEmpty(list))
                return;

            //每一个资金账户有多个币种的资金表,每个币种一条记录
            foreach (var capitalAccountTable in list)
            {
                //如果当前资金记录没有冻结量，那么不处理
                //if (capitalAccountTable.FreezeCapitalTotal <=0.00m)
                //    continue;

                if (capitalAccountTable.FreezeCapitalTotal == 0)
                    continue;

                ProcessCapitalByCurrency(capitalAccountTable);
                //ReckoningTransaction tm = TransactionFactory.GetTransactionManager();
                //tm.BeginTransaction();
                //try
                //{
                //    ProcessCapitalByCurrency(tm, capitalAccountTable);
                //    tm.Commit();
                //}
                //catch (Exception ex)
                //{
                //    LogHelper.WriteError("清算资金出现问题 ", ex);
                //    tm.Rollback();
                //    throw ex;
                //    //return false;
                //}
            }
        }


        /// <summary>
        /// 按币种对资金进行清算
        /// </summary>
        /// <param name="tm"></param>
        /// <param name="capitalAccountTable"></param>
        private void ProcessCapitalByCurrency(ReckoningTransaction tm, HK_CapitalAccountInfo capitalAccountTable)
        {
            HK_CapitalAccountFreezeDal dal = new HK_CapitalAccountFreezeDal();

            string where = string.Format("CapitalAccountLogo = '{0}' ", capitalAccountTable.CapitalAccountLogo);
            List<HK_CapitalAccountFreezeInfo> freezeTables = dal.GetListArray(where);
            //DataRepository.XhCapitalAccountFreezeTableProvider.GetByCapitalAccountLogo(
            //   capitalAccountTable.CapitalAccountLogo);

            if (Utils.IsNullOrEmpty(freezeTables))
                return;

            decimal freezeMoney = 0;

            //需要更新的冻结表（从所有的冻结表中选出）
            List<HK_CapitalAccountFreezeInfo> updateFreezeTables = new List<HK_CapitalAccountFreezeInfo>();
            foreach (var freezeTable in freezeTables)
            {
                int freezeTypeLogo = freezeTable.FreezeTypeLogo;
                //if (freezeTypeLogo<0.00m)
                //    continue;

                switch (freezeTypeLogo)
                {
                    //委托冻结，直接解冻
                    case (int)Types.FreezeType.DelegateFreeze:
                        freezeMoney += GetUnFreezeMoney(freezeTable);
                        updateFreezeTables.Add(freezeTable);
                        break;

                    //清算冻结，需要判断解冻日期
                    case (int)Types.FreezeType.ReckoningFreeze:


                        DateTime thawTime = (DateTime)freezeTable.ThawTime;
                        if (ReckoningService.IsUnFreezeDate(thawTime))
                        {
                            freezeMoney += GetUnFreezeMoney(freezeTable);
                            updateFreezeTables.Add(freezeTable);
                        }

                        break;
                }
            }

            ///更新资金冻结表(直接删除解冻的记录-冻结金额和冻结费用都已经为0）
            if (updateFreezeTables.Count > 0)
            {
                //DataRepository.XhCapitalAccountFreezeTableProvider.Update(tm, updateFreezeTables);
                //dal.Delete();
                // DataRepository.XhCapitalAccountFreezeTableProvider.Delete(tm, updateFreezeTables);
                foreach (var freezeTable in updateFreezeTables)
                {
                    dal.Delete(freezeTable.CapitalFreezeLogoId, tm);
                }
            }


            ///更新资金表
            //if (capitalAccountTable.AvailableCapital <=0.00m)
            //    capitalAccountTable.AvailableCapital = 0;

            capitalAccountTable.AvailableCapital += freezeMoney;

            //if (capitalAccountTable.FreezeCapitalTotal <=0.00m)
            //    capitalAccountTable.FreezeCapitalTotal = 0;
            capitalAccountTable.FreezeCapitalTotal -= freezeMoney;
            if (capitalAccountTable.FreezeCapitalTotal < 0)
                capitalAccountTable.FreezeCapitalTotal = 0;

            capitalAccountTable.CapitalBalance = capitalAccountTable.AvailableCapital +
                                                 capitalAccountTable.FreezeCapitalTotal;

            // DataRepository.XhCapitalAccountTableProvider.Update(tm, capitalAccountTable);
            HK_CapitalAccountDal xhCapitalAccountTableDal = new HK_CapitalAccountDal();
            xhCapitalAccountTableDal.Update(capitalAccountTable, tm.Database, tm.Transaction);
        }

        /// <summary>
        /// 按币种对资金进行清算
        /// </summary>
        /// <param name="capitalAccountTable"></param>
        private void ProcessCapitalByCurrency(HK_CapitalAccountInfo capitalAccountTable)
        {
            HK_CapitalAccountFreezeDal dal = new HK_CapitalAccountFreezeDal();
            string where = string.Format("CapitalAccountLogo = '{0}' ", capitalAccountTable.CapitalAccountLogo);
            List<HK_CapitalAccountFreezeInfo> freezeTables = dal.GetListArray(where);
            // DataRepository.XhCapitalAccountFreezeTableProvider.GetByCapitalAccountLogo(
            //    capitalAccountTable.CapitalAccountLogo);

            if (Utils.IsNullOrEmpty(freezeTables))
                return;

            decimal freezeMoney = 0;

            //需要更新的冻结表（从所有的冻结表中选出）
            List<HK_CapitalAccountFreezeInfo> updateFreezeTables = new List<HK_CapitalAccountFreezeInfo>();
            foreach (var freezeTable in freezeTables)
            {
                int freezeTypeLogo = freezeTable.FreezeTypeLogo;
                //if (freezeTypeLogo<0.00m)
                //    continue;

                switch (freezeTypeLogo)
                {
                    //委托冻结，直接解冻
                    case (int)Types.FreezeType.DelegateFreeze:
                        freezeMoney += GetUnFreezeMoney(freezeTable);
                        updateFreezeTables.Add(freezeTable);
                        break;

                    //清算冻结，需要判断解冻日期
                    case (int)Types.FreezeType.ReckoningFreeze:


                        DateTime thawTime = (DateTime)freezeTable.ThawTime;
                        if (ReckoningService.IsUnFreezeDate(thawTime))
                        {
                            freezeMoney += GetUnFreezeMoney(freezeTable);
                            updateFreezeTables.Add(freezeTable);
                        }

                        break;
                }
            }

            #region 进行更新

            //ReckoningTransaction tm = TransactionFactory.GetTransactionManager();
            //tm.BeginTransaction();
            ReckoningTransaction tm = new ReckoningTransaction();
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                try
                {
                    connection.Open();
                    tm.Database = database;
                    DbTransaction transaction = connection.BeginTransaction();
                    tm.Transaction = transaction;
                    ///更新资金冻结表(直接删除解冻的记录-冻结金额和冻结费用都已经为0）
                    if (updateFreezeTables.Count > 0)
                    {
                        //DataRepository.XhCapitalAccountFreezeTableProvider.Update(tm, updateFreezeTables);
                        HK_CapitalAccountFreezeDal xhCapitalAccountFreezeTableDal =
                            new HK_CapitalAccountFreezeDal();
                        foreach (var freezeTable in updateFreezeTables)
                        {
                            xhCapitalAccountFreezeTableDal.Delete(freezeTable.CapitalFreezeLogoId, tm);
                        }

                        //DataRepository.XhCapitalAccountFreezeTableProvider.Delete(tm, updateFreezeTables);
                    }


                    ///更新资金表
                    //if (capitalAccountTable.AvailableCapital<0.00m)
                    //    capitalAccountTable.AvailableCapital = 0;

                    capitalAccountTable.AvailableCapital += freezeMoney;

                    //if (capitalAccountTable.FreezeCapitalTotal<=0.00m)
                    //    capitalAccountTable.FreezeCapitalTotal = 0;
                    capitalAccountTable.FreezeCapitalTotal -= freezeMoney;
                    if (capitalAccountTable.FreezeCapitalTotal < 0)
                        capitalAccountTable.FreezeCapitalTotal = 0;

                    capitalAccountTable.CapitalBalance = capitalAccountTable.AvailableCapital +
                                                         capitalAccountTable.FreezeCapitalTotal;
                    HK_CapitalAccountDal xhCapitalAccountTableDal = new HK_CapitalAccountDal();
                    xhCapitalAccountTableDal.Update(capitalAccountTable, tm.Database,
                                                    tm.Transaction);
                    //DataRepository.XhCapitalAccountTableProvider.Update(tm, capitalAccountTable);
                    //tm.Commit();
                    tm.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                    //tm.Rollback();
                    tm.Transaction.Rollback();
                }
            }

            #endregion
        }

        private static decimal GetUnFreezeMoney(HK_CapitalAccountFreezeInfo freezeTable)
        {
            decimal result = 0;

            //if (freezeTable.FreezeCapitalAmount <=0.00m)
            //    freezeTable.FreezeCapitalAmount = 0;
            //if (freezeTable.FreezeCost<=0.00m)
            //    freezeTable.FreezeCost = 0;

            result = freezeTable.FreezeCapitalAmount + freezeTable.FreezeCost;

            //获取值后赋零
            freezeTable.FreezeCapitalAmount = 0;
            freezeTable.FreezeCost = 0;

            return result;
        }

        #endregion

        #region 清算持仓

        /// <summary>
        /// 清算持仓
        /// </summary>
        /// <param name="tm"></param>
        /// <param name="holdAccount"></param>
        protected void ProcessHold(ReckoningTransaction tm, UA_UserAccountAllocationTableInfo holdAccount)
        {
            HK_AccountHoldDal xhAccountHoldTableDal = new HK_AccountHoldDal();
            string where = string.Format("UserAccountDistributeLogo = '{0}'", holdAccount.UserAccountDistributeLogo);
            List<HK_AccountHoldInfo> list = xhAccountHoldTableDal.GetListArray(where);
            //DataRepository.XhAccountHoldTableProvider.GetByUserAccountDistributeLogo(
            //    holdAccount.UserAccountDistributeLogo);

            if (Utils.IsNullOrEmpty(list))
                return;

            for (int i = 0; i < list.Count; i++)
            {
                HK_AccountHoldInfo accountHoldTable = list[i];
                //如果当前持仓记录没有冻结量，那么不处理
                //if (accountHoldTable.FreezeAmount <=0.00m)
                //    continue;

                if (accountHoldTable.FreezeAmount == 0)
                    continue;

                //ReckoningTransaction tm2 = TransactionFactory.GetTransactionManager();
                //tm2.BeginTransaction();
                ReckoningTransaction reckoningTransaction = new ReckoningTransaction();
                Database database = DatabaseFactory.CreateDatabase();
                using (DbConnection connection = database.CreateConnection())
                {
                    try
                    {
                        connection.Open();
                        reckoningTransaction.Database = database;
                        DbTransaction transaction = connection.BeginTransaction();
                        reckoningTransaction.Transaction = transaction;
                        ProcessHoldByCode(reckoningTransaction, accountHoldTable);
                        //tm2.Commit();
                        reckoningTransaction.Transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteError("清算持仓出现问题 ", ex);
                        reckoningTransaction.Transaction.Rollback();
                        //tm2.Rollback();
                        throw ex;
                        //return false;
                    }
                }
                //tm2.Commit();
            }

            //IDictionary<string, List<XH_AccountHoldTableInfo>> stockCurrencyList =
            //    new Dictionary<string, List<XH_AccountHoldTableInfo>>();


            ////每一个持仓账户有多个股票的持仓记录, 每个股票只有一条记录
            //foreach (var accountHoldTable in list)
            //{
            //    string code = accountHoldTable.Code;

            //    if (stockCurrencyList.ContainsKey(code))
            //    {
            //        stockCurrencyList[code] = new List<XH_AccountHoldTableInfo>();
            //    }

            //    stockCurrencyList[code].Add(accountHoldTable);
            //}

            ////对每一个股票进行处理
            //foreach (var key in stockCurrencyList.Keys)
            //{
            //    var codeList = stockCurrencyList[key];

            //    //每一个股票，对应每个币种有一条记录
            //    foreach (XH_AccountHoldTableInfo holdTableByCodeAndCurrency in codeList)
            //    {
            //        ProcessHoldByCode(tm, holdTableByCodeAndCurrency);
            //    }
            //}
        }

        /// <summary>
        /// 清算持仓
        /// </summary>
        /// <param name="holdAccount"></param>
        protected void ProcessHold(UA_UserAccountAllocationTableInfo holdAccount)
        {
            HK_AccountHoldDal xhAccountHoldTableDal = new HK_AccountHoldDal();
            string where = string.Format("UserAccountDistributeLogo = '{0}'", holdAccount.UserAccountDistributeLogo);
            List<HK_AccountHoldInfo> list =
                xhAccountHoldTableDal.GetListArray(where);

            if (Utils.IsNullOrEmpty(list))
                return;

            for (int i = 0; i < list.Count; i++)
            {
                HK_AccountHoldInfo accountHoldTable = list[i];
                //如果当前持仓记录没有冻结量，那么不处理
                //if (accountHoldTable.FreezeAmount<0)
                //    continue;

                if (accountHoldTable.FreezeAmount == 0)
                    continue;

                ProcessHoldByCode(accountHoldTable);
            }
        }


        /// <summary>
        /// 按股票对持仓进行清算
        /// </summary>
        /// <param name="tm"></param>
        /// <param name="holdAccountTable"></param>
        private void ProcessHoldByCode(ReckoningTransaction tm, HK_AccountHoldInfo holdAccountTable)
        {
            string where = string.Format("AccountHoldLogo = '{0}'", holdAccountTable.AccountHoldLogoID);
            HK_AcccountHoldFreezeDal xhAcccountHoldFreezeTableDal = new HK_AcccountHoldFreezeDal();
            List<HK_AcccountHoldFreezeInfo> freezeTables = xhAcccountHoldFreezeTableDal.GetListArray(where);
            //DataRepository.XhAcccountHoldFreezeTableProvider.GetByAccountHoldLogo(
            //    holdAccountTable.AccountHoldLogoId);

            if (Utils.IsNullOrEmpty(freezeTables))
                return;

            int freezeHold = 0;
            List<HK_AcccountHoldFreezeInfo> updateFreezeTables = new List<HK_AcccountHoldFreezeInfo>();
            foreach (var freezeTable in freezeTables)
            {
                int freezeTypeLogo = freezeTable.FreezeTypeID;
                //if (freezeTypeLogo<0.00m)
                //    continue;

                //if (freezeTable.PrepareFreezeAmount <=0.00m)
                //    continue;

                if (freezeTable.PrepareFreezeAmount == 0)
                    continue;

                switch (freezeTypeLogo)
                {
                    case (int)Types.FreezeType.DelegateFreeze:
                        //if (freezeTable.PrepareFreezeAmount<=0.00m)
                        //    freezeTable.PrepareFreezeAmount = 0;

                        freezeHold += freezeTable.PrepareFreezeAmount;

                        updateFreezeTables.Add(freezeTable);
                        break;

                    //清算冻结，需要判断解冻日期
                    case (int)Types.FreezeType.ReckoningFreeze:


                        DateTime thawTime = (DateTime)freezeTable.ThawTime;
                        if (ReckoningService.IsUnFreezeDate(thawTime))
                        {
                            //if (freezeTable.PrepareFreezeAmount<=0.00m)
                            //    freezeTable.PrepareFreezeAmount = 0;

                            freezeHold += freezeTable.PrepareFreezeAmount;

                            updateFreezeTables.Add(freezeTable);
                        }

                        break;
                }
            }

            ///更新持仓冻结表
            if (updateFreezeTables.Count > 0)
            {
                //DataRepository.XhAcccountHoldFreezeTableProvider.Update(tm, updateFreezeTables);
                //DataRepository.XhAcccountHoldFreezeTableProvider.Delete(tm, updateFreezeTables);
                //XH_AcccountHoldFreezeTableDal xh_AcccountHoldFreezeTableDal = new XH_AcccountHoldFreezeTableDal();
                foreach (var table in updateFreezeTables)
                {
                    xhAcccountHoldFreezeTableDal.Update(table, tm.Database, tm.Transaction);
                }
            }

            ///更新持仓表
            //if (holdAccountTable.AvailableAmount<=0.00m)
            //    holdAccountTable.AvailableAmount = 0;

            holdAccountTable.AvailableAmount += freezeHold;

            //if (holdAccountTable.FreezeAmount<=0.00m)
            //    holdAccountTable.FreezeAmount = 0;
            holdAccountTable.FreezeAmount -= freezeHold;
            if (holdAccountTable.FreezeAmount < 0)
                holdAccountTable.FreezeAmount = 0;
            HK_AccountHoldDal xhAccountHoldTableDal = new HK_AccountHoldDal();
            xhAccountHoldTableDal.Update(holdAccountTable, tm.Database, tm.Transaction);
            // DataRepository.XhAccountHoldTableProvider.Update(tm, holdAccountTable);
        }

        /// <summary>
        /// 按股票对持仓进行清算
        /// </summary>
        /// <param name="holdAccountTable"></param>
        private void ProcessHoldByCode(HK_AccountHoldInfo holdAccountTable)
        {
            HK_AcccountHoldFreezeDal xhAcccountHoldFreezeTableDal = new HK_AcccountHoldFreezeDal();
            string where = string.Format("AccountHoldLogo = '{0}'", holdAccountTable.AccountHoldLogoID);
            List<HK_AcccountHoldFreezeInfo> freezeTables = xhAcccountHoldFreezeTableDal.GetListArray(where);

            if (Utils.IsNullOrEmpty(freezeTables))
                return;

            int freezeHold = 0;
            List<HK_AcccountHoldFreezeInfo> updateFreezeTables = new List<HK_AcccountHoldFreezeInfo>();
            foreach (var freezeTable in freezeTables)
            {
                int freezeTypeLogo = freezeTable.FreezeTypeID;
                //if (freezeTypeLogo<0.00m)
                //    continue;

                //if (freezeTable.PrepareFreezeAmount<0.00m)
                //    continue;

                if (freezeTable.PrepareFreezeAmount == 0)
                    continue;

                switch (freezeTypeLogo)
                {
                    case (int)Types.FreezeType.DelegateFreeze:


                        //if (freezeTable.PrepareFreezeAmount<0.00m)
                        //    freezeTable.PrepareFreezeAmount = 0;

                        freezeHold += freezeTable.PrepareFreezeAmount;

                        updateFreezeTables.Add(freezeTable);
                        break;

                    //清算冻结，需要判断解冻日期
                    case (int)Types.FreezeType.ReckoningFreeze:


                        DateTime thawTime = (DateTime)freezeTable.ThawTime;
                        if (ReckoningService.IsUnFreezeDate(thawTime))
                        {
                            //if (freezeTable.PrepareFreezeAmount<0.00m)
                            //    freezeTable.PrepareFreezeAmount = 0;

                            freezeHold += freezeTable.PrepareFreezeAmount;

                            updateFreezeTables.Add(freezeTable);
                        }

                        break;
                }
            }

            #region 进行更新

            xhAcccountHoldFreezeTableDal = new HK_AcccountHoldFreezeDal();
            HK_AccountHoldDal xhAccountHoldTableDal = new HK_AccountHoldDal();

            try
            {
                DataManager.ExecuteInTransaction(rt =>
                                                     {
                                                         ///更新持仓冻结表
                                                         if (updateFreezeTables.Count > 0)
                                                         {
                                                             //DataRepository.XhAcccountHoldFreezeTableProvider.Update(tm, updateFreezeTables);
                                                             foreach (var info in updateFreezeTables)
                                                             {
                                                                 xhAcccountHoldFreezeTableDal.DeleteRecord(
                                                                     info.HoldFreezeLogoId, rt.Database, rt.Transaction);
                                                             }
                                                         }

                                                         ///更新持仓表
                                                         //if (holdAccountTable.AvailableAmount<0.00m)
                                                         //    holdAccountTable.AvailableAmount = 0;

                                                         holdAccountTable.AvailableAmount += freezeHold;

                                                         //if (holdAccountTable.FreezeAmount<0.00m)
                                                         //    holdAccountTable.FreezeAmount = 0;
                                                         holdAccountTable.FreezeAmount -= freezeHold;
                                                         //if (holdAccountTable.FreezeAmount < 0)
                                                         //    holdAccountTable.FreezeAmount = 0;
                                                         //XH_AccountHoldTableDal xh_AccountHoldTableDal = new XH_AccountHoldTableDal();
                                                         xhAccountHoldTableDal.UpdateRecord(holdAccountTable,
                                                                                            rt.Database, rt.Transaction);
                                                     });
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }


            //ReckoningTransaction tm = new ReckoningTransaction();
            //Database database = DatabaseFactory.CreateDatabase();
            //DbConnection connection = database.CreateConnection();
            //connection.Open();
            //tm.Database = database;
            //DbTransaction transaction = connection.BeginTransaction();
            //tm.Transaction = transaction;

            //xh_AcccountHoldFreezeTableDal = new XH_AcccountHoldFreezeTableDal();
            //try
            //{
            //    ///更新持仓冻结表
            //    if (updateFreezeTables.Count > 0)
            //    {
            //        //DataRepository.XhAcccountHoldFreezeTableProvider.Update(tm, updateFreezeTables);
            //        foreach (var info in updateFreezeTables)
            //        {
            //            xh_AcccountHoldFreezeTableDal.DeleteRecord(info.HoldFreezeLogoId, tm.Database, tm.Transaction);
            //        }

            //    }

            //    ///更新持仓表
            //    //if (holdAccountTable.AvailableAmount<0.00m)
            //    //    holdAccountTable.AvailableAmount = 0;

            //    holdAccountTable.AvailableAmount += freezeHold;

            //    //if (holdAccountTable.FreezeAmount<0.00m)
            //    //    holdAccountTable.FreezeAmount = 0;
            //    holdAccountTable.FreezeAmount -= freezeHold;
            //    //if (holdAccountTable.FreezeAmount < 0)
            //    //    holdAccountTable.FreezeAmount = 0;
            //    XH_AccountHoldTableDal xh_AccountHoldTableDal = new XH_AccountHoldTableDal();
            //    xh_AccountHoldTableDal.Update(holdAccountTable, tm.Database, tm.Transaction);
            //    tm.Transaction.Commit();
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.WriteError(ex.Message, ex);
            //    tm.Transaction.Rollback();
            //}

            #endregion
        }

        #endregion
    }

    /// <summary>
    /// 期货清算类
    /// </summary>
    public class FutureReckoning
    {
        private IDictionary<int, QHCapitalHoldPair> dict = new Dictionary<int, QHCapitalHoldPair>();

        #region old code 李健华 2010-06-09
        //protected VTTraders vtTraders;

        //public FutureReckoning()
        //{
        //    vtTraders = VTTradersFactory.GetFutureTraders();
        //}


        ///// <summary>
        ///// 进行期货盘后清算
        ///// </summary>
        //public bool Process()
        //{
        //    if (!vtTraders.IsInitializeSuccess)
        //    {
        //        LogHelper.WriteDebug("VTTraders没有初始化，无法进行清算！");
        //        return false;
        //    }

        //    bool result = false;
        //    LogHelper.WriteInfo("进行期货盘后清算");

        //    //未报、待报的委托状态改为废单,已报待撤、已报、部成、部成待撤的走撤单流程
        //    ProcessTodayFutureEntrustStatus();

        //    //foreach (var trader in vtTraders.TraderList)
        //    //{
        //    //    foreach (var accountPair in trader.AccountPairList)
        //    //    {
        //    //        DoReckoning(accountPair.CapitalAccount, accountPair.HoldAccount);
        //    //    }
        //    //}
        //    //=============
        //    //add 李健华 因为盘后清算可能做内部撤单比后面的慢所以会产生冻结资无法即时解冻
        //    //所以这里做了撤单后做延时一分钟
        //    if (ScheduleManager.IsStartSuccess)
        //    {
        //        LogHelper.WriteInfo("期货进行盘后清算延时一分钟操作请稍后....");
        //        Thread.CurrentThread.Join(60000);
        //    }

        //    //==============
        //    //add 李健华   2009-12-16
        //    //先关闭内存管理器，提交资金到数据库，因为后面的清算都是从数据库中清算的.
        //    MemoryDataManager.End();
        //    //=====================

        //    bool hasFailure = false;

        //    for (int i = 0; i < vtTraders.TraderList.Count; i++)
        //    {
        //        var trader = vtTraders.TraderList[i];
        //        string traderFormat = "期货盘后清算进度————当前交易员[UserID={0}]";
        //        string traderMsg = string.Format(traderFormat, trader.Trader.UserID);
        //        LogHelper.WriteDebug(traderMsg);

        //        for (int j = 0; j < trader.AccountPairList.Count; j++)
        //        {
        //            string processFormat = "期货盘后清算进度————当前/总交易员数量：[{0}/{1}] 当前/当前交易员总帐户数量：[{2}/{3}]";
        //            string processMsg = string.Format(processFormat, i + 1, vtTraders.TraderList.Count, j + 1,
        //                                              trader.AccountPairList.Count);
        //            LogHelper.WriteDebug(processMsg);

        //            var accountPair = trader.AccountPairList[j];

        //            bool isSuccess = DoReckoning(accountPair.CapitalAccount, accountPair.HoldAccount);

        //            if (!isSuccess)
        //                hasFailure = true;
        //        }
        //    }

        //    if (!hasFailure)
        //    {
        //        try
        //        {
        //            CheckAllFreezeMoney();
        //            CheckAllMargin();
        //            CheckAllMargin2();

        //            CheckAllFreezeMoney2();
        //            CheckAllFreezeHold2();

        //            StatusTableChecker.UpdateFutureReckoningDate(null);
        //            LogHelper.WriteInfo("期货盘后清算成功！");

        //            result = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            LogHelper.WriteError(ex.Message, ex);
        //        }

        //        //ReckoningTransaction tm = new ReckoningTransaction();
        //        //Database database = DatabaseFactory.CreateDatabase();
        //        //using (DbConnection connection = database.CreateConnection())
        //        //{
        //        //    try
        //        //    {
        //        //        connection.Open();
        //        //        tm.Database = database;
        //        //        DbTransaction transaction = connection.BeginTransaction();
        //        //        tm.Transaction = transaction;
        //        //        StatusTableChecker.UpdateFutureReckoningDate(tm);
        //        //        LogHelper.WriteInfo("期货盘后清算成功！");
        //        //        tm.Transaction.Commit();
        //        //        result = true;
        //        //    }
        //        //    catch (Exception ex)
        //        //    {
        //        //        LogHelper.WriteError("", ex);
        //        //        tm.Transaction.Rollback();
        //        //    }
        //        //}
        //    }
        //    //清算完毕后再重新加载内存资金表内容,为后面其他清算做预委托下单的处理同样操作内存表的数据
        //    //内存表管理
        //    MemoryDataManager.Start();
        //    return result;
        //}
        #endregion

        #region new  code 李健华 2010-06-09
        /// <summary>
        /// 所有资金账号与持仓账号配对对
        /// </summary>
        protected List<AccountPair> userAccountPair;


        /// <summary>
        /// 构造函数(内部带有初始化处理)
        /// </summary>
        public FutureReckoning()
        {
            userAccountPair = new List<AccountPair>();

            //获取所有期货资金账号类型
            List<BD_AccountTypeInfo> accountTypes = AccountManager.Instance.GetAccoutTypeByACTID((int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.FuturesCapital);

            if (Utils.IsNullOrEmpty(accountTypes))
            {
                return;
            }
            foreach (var item in accountTypes)
            {
                //过虑掉不是期的其他
                if (item.AccountTypeLogo != (int)GTA.VTS.Common.CommonObject.Types.AccountType.StockFuturesCapital
                    && item.AccountTypeLogo != (int)GTA.VTS.Common.CommonObject.Types.AccountType.CommodityFuturesCapital)
                {
                    continue;
                }

                List<AccountPair> list = VTTradersFactory.InitializeAccountPair(item);
                if (!Utils.IsNullOrEmpty(list))
                {
                    userAccountPair.AddRange(list);
                }
            }
        }


        /// <summary>
        /// 进行期货盘后清算
        /// </summary>
        public bool Process()
        {
            if (Utils.IsNullOrEmpty(userAccountPair))
            {
                LogHelper.WriteDebug("VTTraders没有初始化，无法进行清算！");
                return false;
            }

            bool result = false;
            LogHelper.WriteInfo("进行期货盘后清算");

            //未报、待报的委托状态改为废单,已报待撤、已报、部成、部成待撤的走撤单流程
            ProcessTodayFutureEntrustStatus();

            //foreach (var trader in vtTraders.TraderList)
            //{
            //    foreach (var accountPair in trader.AccountPairList)
            //    {
            //        DoReckoning(accountPair.CapitalAccount, accountPair.HoldAccount);
            //    }
            //}
            //=============
            //add 李健华 因为盘后清算可能做内部撤单比后面的慢所以会产生冻结资无法即时解冻
            //所以这里做了撤单后做延时一分钟
            if (ScheduleManager.IsStartSuccess)
            {
                LogHelper.WriteInfo("期货进行盘后清算延时一分钟操作请稍后....");
                Thread.CurrentThread.Join(60000);
            }

            //==============
            //add 李健华   2009-12-16
            //先关闭内存管理器，提交资金到数据库，因为后面的清算都是从数据库中清算的.
            MemoryDataManager.End();
            //=====================

            bool hasFailure = false;


            for (int j = 0; j < userAccountPair.Count; j++)
            {
                var accountPair = userAccountPair[j];

                string traderFormat = "期货盘后清算进度————当前交易员[UserID={0}]-当前/总交易员数量：[{1}/{2}] 当前/当前交易员总帐户数量：[{3}/{4}]";
                string traderMsg = string.Format(traderFormat, accountPair.CapitalAccount.UserID, j + 1, userAccountPair.Count, 1, 1);
                LogHelper.WriteDebug(traderMsg);

                bool isSuccess = DoReckoning(accountPair.CapitalAccount, accountPair.HoldAccount);

                if (!isSuccess)
                    hasFailure = true;
            }

            if (!hasFailure)
            {
                try
                {
                    LogHelper.WriteInfo("【正在】开始检查总冻结资金是否等于所有冻结记录的总和");
                    CheckAllFreezeMoney();
                    LogHelper.WriteInfo("【完成】检查总冻结资金是否等于所有冻结记录的总和");

                    LogHelper.WriteInfo("【正在】检查资金表中的保证金总值是否等于所有持仓的保证金累加");
                    CheckAllMargin();
                    LogHelper.WriteInfo("【完成】资金表中的保证金总值是否等于所有持仓的保证金累加");

                    LogHelper.WriteInfo("【正在】检查持仓都为0的是否还有保证金");
                    CheckAllMargin2();
                    LogHelper.WriteInfo("【完成】检查持仓都为0的是否还有保证金");

                    LogHelper.WriteInfo("【正在】检查所有冻结是否最终状态以持仓冻结");
                    CheckAllFreezeMoney2();
                    
                    CheckAllFreezeHold2();
                    LogHelper.WriteInfo("【完成】检查所有冻结是否最终状态以持仓冻结");

                    StatusTableChecker.UpdateFutureReckoningDate(null);
                    LogHelper.WriteInfo("期货盘后清算成功！");

                    result = true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }

                //ReckoningTransaction tm = new ReckoningTransaction();
                //Database database = DatabaseFactory.CreateDatabase();
                //using (DbConnection connection = database.CreateConnection())
                //{
                //    try
                //    {
                //        connection.Open();
                //        tm.Database = database;
                //        DbTransaction transaction = connection.BeginTransaction();
                //        tm.Transaction = transaction;
                //        StatusTableChecker.UpdateFutureReckoningDate(tm);
                //        LogHelper.WriteInfo("期货盘后清算成功！");
                //        tm.Transaction.Commit();
                //        result = true;
                //    }
                //    catch (Exception ex)
                //    {
                //        LogHelper.WriteError("", ex);
                //        tm.Transaction.Rollback();
                //    }
                //}
            }
            //清算完毕后再重新加载内存资金表内容,为后面其他清算做预委托下单的处理同样操作内存表的数据
            //内存表管理
            MemoryDataManager.Start();
            return result;
        }
        #endregion

        /// <summary>
        /// 检查冻结记录对应的委托是否已经是最终状态，如果是，那么这条冻结记录要解冻
        /// </summary>
        private void CheckAllFreezeMoney2()
        {
            QH_CapitalAccountFreezeTableDal dal = new QH_CapitalAccountFreezeTableDal();
            var list = dal.GetAllFreezeMoney();

            QH_CapitalAccountTableDal capitalDal = new QH_CapitalAccountTableDal();
            foreach (var freezeSum in list)
            {
                if (freezeSum.FreezeCapitalSum == 0)
                    continue;

                try
                {
                    var capitalAccountTable = capitalDal.GetModel(freezeSum.CapitalAccountLogo);
                    ProcessCapitalFreezeMoney2(capitalAccountTable);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// 删除所有没有对应持仓的冻结
        /// </summary>
        private void CheckAllFreezeHold2()
        {
            QH_HoldAccountTableDal holdDal = new QH_HoldAccountTableDal();
            QH_HoldAccountFreezeTableDal freezeDal = new QH_HoldAccountFreezeTableDal();

            var freezeList = freezeDal.GetAllListArray();
            foreach (var freezeTableInfo in freezeList)
            {
                int accountHoldId = freezeTableInfo.AccountHoldLogo;

                if (!holdDal.Exists(accountHoldId))
                    freezeDal.Delete(freezeTableInfo.HoldFreezeLogoId);
            }
        }

        private void ProcessCapitalFreezeMoney2(QH_CapitalAccountTableInfo capitalTable)
        {
            int capitalAccountLogo = capitalTable.CapitalAccountLogoId;
            QH_CapitalAccountFreezeTableDal dal = new QH_CapitalAccountFreezeTableDal();
            string where = string.Format("CapitalAccountLogo = '{0}' AND freezetypelogo={1}", capitalAccountLogo,
                                         (int)Types.FreezeType.DelegateFreeze);
            var freezeList = dal.GetListArray(where);
            if (Utils.IsNullOrEmpty(freezeList))
                return;

            var freezeList2 = new List<QH_CapitalAccountFreezeTableInfo>();
            decimal allSum = 0;
            foreach (var freezeTable in freezeList)
            {
                decimal famount = freezeTable.FreezeAmount;

                decimal fcost = freezeTable.FreezeCost;

                decimal sum = fcost + famount;

                if (sum == 0)
                    continue;

                bool isDone = HasDoneEntrust(freezeTable.EntrustNumber);
                if (isDone)
                {
                    freezeList2.Add(freezeTable);
                    allSum += sum;
                }
            }

            if (Utils.IsNullOrEmpty(freezeList2))
            {
                UpdateCapitalTable(capitalTable, freezeList2, allSum);
            }
        }

        private void UpdateCapitalTable(QH_CapitalAccountTableInfo capitalTable,
                                        List<QH_CapitalAccountFreezeTableInfo> freezeTables,
                                        decimal freezeSum)
        {
            capitalTable.AvailableCapital += freezeSum;
            capitalTable.FreezeCapitalTotal -= freezeSum;

            QH_CapitalAccountFreezeTableDal capitalDal = new QH_CapitalAccountFreezeTableDal();
            QH_CapitalAccountTableDal qhCapitalAccountTableDal = new QH_CapitalAccountTableDal();

            DataManager.ExecuteInTransaction(tm =>
                                                 {
                                                     foreach (var freezeTable in freezeTables)
                                                     {
                                                         capitalDal.Clear(freezeTable.CapitalFreezeLogoId, tm.Database,
                                                                          tm.Transaction);
                                                     }

                                                     qhCapitalAccountTableDal.Update(capitalTable, tm.Database,
                                                                                     tm.Transaction);

                                                     string format2 =
                                                         "******************FutureReckoning.UpdateCapitalTable修正期货冻结资金[剩余冻结资金={0}]";
                                                     string desc = string.Format(format2, freezeSum);
                                                     LogHelper.WriteDebug(desc);
                                                 });

            //这里不放在上面，是因为这是新增的需求，就算这记录不成功也不影响之前的相关操作
            //2009-12-2 李健华add 添加期货资金流水表记录========start===
            if (freezeSum != 0)
            {
                QHDataAccess.AddQH_CapitalFlow(1, 0, freezeSum, 0, "", 0, capitalTable.UserAccountDistributeLogo, capitalTable.TradeCurrencyType);
            }
            //=============end===============

        }

        private bool HasDoneEntrust(string entrustNumber)
        {
            QH_TodayEntrustTableDal qhTodayEntrustTableDal = new QH_TodayEntrustTableDal();
            var tet = qhTodayEntrustTableDal.GetModel(entrustNumber);

            //如果找不到，那么这笔冻结记录更应该解冻
            if (tet == null)
            {
                return true;
            }

            int state = tet.OrderStatusId;
            if (state < 0)
                return false;

            //如果状态时废单，部撤，已撤，已成，那么代表是最终状态
            if (state == (int)Types.OrderStateType.DOSCanceled || state == (int)Types.OrderStateType.DOSPartRemoved
                || state == (int)Types.OrderStateType.DOSRemoved || state == (int)Types.OrderStateType.DOSDealed)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 检查持仓的总冻结是否等于所有冻结记录的总和
        /// </summary>
        private void CheckAllFreezeHold()
        {
            //QH_HoldAccountTableDal dal = new QH_HoldAccountTableDal();
            //var list = dal.GetAllListArray();
            //foreach (var hold in list)
            //{
            //    try
            //    {
            //        int accountHoldId = hold.AccountHoldLogoId;

            //        QH_HoldAccountFreezeTableDal freezeTableDal = new QH_HoldAccountFreezeTableDal();
            //        int sum = freezeTableDal.GetAllFreezeAmount(accountHoldId);

            //        if (hold.FreezeAmount != sum)
            //        {
            //            string format = "ReckoningService.StockReckoning.CheckAllFreezeHold[AccountHoldLogoId={0},FreezeAmount={1},TrueSum={2}]";
            //            string desc = string.Format(format, accountHoldId, hold.FreezeAmount, sum);
            //            LogHelper.WriteDebug(desc);

            //            hold.FreezeAmount = sum;
            //            dal.Update(hold);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        LogHelper.WriteError(ex.Message, ex);
            //    }
            //}
        }

        /// <summary>
        /// 检查资金表中的保证金总值是否等于所有持仓的保证金累加
        /// </summary>
        private void CheckAllMargin()
        {
            QH_CapitalAccountTableDal capitalDal = new QH_CapitalAccountTableDal();
            var list = capitalDal.GetAllListArray();

            QH_HoldAccountTableDal holdDal = new QH_HoldAccountTableDal();

            UA_UserAccountAllocationTableDal userDal = new UA_UserAccountAllocationTableDal();

            foreach (var capital in list)
            {
                decimal marginTotal = capital.MarginTotal;
                int currency = capital.TradeCurrencyType;
                #region update 2010-06-10 李健华
                //这里是盘后清算，不应在缓存中查询，直接在数据库中查询
                //var holdAccount = AccountManager.Instance.GetHoldAccountByCapitalAccount(capital.UserAccountDistributeLogo);
                var holdAccount = userDal.GetUserHoldAccountByUserCapitalAccount(capital.UserAccountDistributeLogo);
                if (holdAccount != null)
                {
                    continue;
                }
                #endregion
                decimal marginSum = holdDal.GetMarginSum(holdAccount.UserAccountDistributeLogo, currency);

                if (marginTotal != marginSum)
                {
                    capital.MarginTotal = marginSum;
                    capitalDal.Update(capital);

                    //这里不放在上面，是因为这是新增的需求，就算这记录不成功也不影响之前的相关操作
                    //2009-12-2 李健华add 添加期货资金流水表记录========start===
                    decimal marginDiffSum = capital.MarginTotal - marginSum;
                    if (marginDiffSum != 0)
                    {
                        QHDataAccess.AddQH_CapitalFlow(1, marginDiffSum, 0, 0, "", 0, capital.UserAccountDistributeLogo, capital.TradeCurrencyType);
                    }
                    //=============end===============
                }
            }
        }

        /// <summary>
        /// 检查持仓都为0的是否还有保证金
        /// </summary>
        private void CheckAllMargin2()
        {
            QH_CapitalAccountTableDal capitalDal = new QH_CapitalAccountTableDal();
            var list = capitalDal.GetAllListArray();

            QH_HoldAccountTableDal holdDal = new QH_HoldAccountTableDal();
            string format = "HistoryHoldAmount=0 and HistoryFreezeAmount=0 and TodayHoldAmount=0 and TodayFreezeAmount=0"
                            + " and TradeCurrencyType={0} and UserAccountDistributeLogo='{1}' and Margin<>0";

            UA_UserAccountAllocationTableDal userDal = new UA_UserAccountAllocationTableDal();

            foreach (var capital in list)
            {
                int currency = capital.TradeCurrencyType;

                #region update 2010-06-10 李健华
                //这里是盘后清算，不应在缓存中查询，直接在数据库中查询
                //  var holdAccount = AccountManager.Instance.GetHoldAccountByCapitalAccount(capital.UserAccountDistributeLogo);
                var holdAccount = userDal.GetUserHoldAccountByUserCapitalAccount(capital.UserAccountDistributeLogo);
                if (holdAccount != null)
                {
                    continue;
                }
                #endregion
              

                string where = string.Format(format, currency, holdAccount.UserAccountDistributeLogo);
                var holdList = holdDal.GetListArray(where);

                if (Utils.IsNullOrEmpty(holdList))
                {
                    continue;
                }

                try
                {
                    decimal marginDiffSum = 0;
                    DataManager.ExecuteInTransaction(tm =>
                                                         {
                                                             foreach (var hold in holdList)
                                                             {
                                                                 string format2 =
                                                                     "ReckoningService.FutureReckoning.CheckAllMargin2[HoldAccountId={0},Margin={1},CapitalId={2}]";
                                                                 string desc = string.Format(format2, hold.AccountHoldLogoId, hold.Margin, capital.CapitalAccountLogoId);
                                                                 LogHelper.WriteDebug(desc);

                                                                 marginDiffSum += hold.Margin;

                                                                 capital.AvailableCapital += hold.Margin;
                                                                 hold.Margin = 0;

                                                                 holdDal.Update(hold, tm);
                                                             }

                                                             capitalDal.Update(capital, tm.Database, tm.Transaction);
                                                         });

                    //这里不放在上面，是因为这是新增的需求，就算这记录不成功也不影响之前的相关操作
                    //2009-12-2 李健华add 添加期货资金流水表记录========start===
                    if (marginDiffSum != 0)
                    {
                        QHDataAccess.AddQH_CapitalFlow(1, marginDiffSum, 0, 0, "", 0, capital.UserAccountDistributeLogo, capital.TradeCurrencyType);
                    }//=============end===============

                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// 检查总冻结资金是否等于所有冻结记录的总和
        /// </summary>
        private void CheckAllFreezeMoney()
        {
            QH_CapitalAccountFreezeTableDal dal = new QH_CapitalAccountFreezeTableDal();
            var list = dal.GetAllFreezeMoney();
            foreach (var freezeSum in list)
            {
                try
                {
                    InternalCheckAllFreezeMoney(freezeSum);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }

            /*
            try
            {
                QH_CapitalAccountTableDal qhCapitalAccountTableDal = new QH_CapitalAccountTableDal();
                var capitals = qhCapitalAccountTableDal.GetListArray(string.Empty);
                foreach (QH_CapitalAccountTableInfo capitalAccountTable in capitals)
                {
                    ProcessCapitalFreezeMoney(capitalAccountTable);
                }

                //LogHelper.WriteInfo("现货盘后冻结资金清算成功！");
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }*/
        }

        private void InternalCheckAllFreezeMoney(QH_CapitalAccountFreezeSum freezeSum)
        {
            int capitalAccountId = freezeSum.CapitalAccountLogo;
            decimal sum = freezeSum.FreezeCapitalSum;

            QH_CapitalAccountTableDal dal = new QH_CapitalAccountTableDal();

            var capital = dal.GetModel(capitalAccountId);

            if (capital.FreezeCapitalTotal != sum)
            {
                decimal oldSum = capital.FreezeCapitalTotal;

                capital.FreezeCapitalTotal = sum;
                dal.Update(capital);

                string format2 =
                    "FutureReckoning.CheckAllFreezeMoney修正期货总冻结资金[初始总冻结资金={0},实际总冻结资金={1},资金账户ID={2}]";
                string desc = string.Format(format2, oldSum, sum, capitalAccountId);
                LogHelper.WriteDebug(desc);
                //decimal freezeMoney = 0;
                //freezeMoney = sum - oldSum;
                //if (freezeMoney != 0)
                //{
                //    QHDataAccess.AddQH_CapitalFlow(1, 0, freezeMoney, 0, "", 0, capital.UserAccountDistributeLogo, capital.TradeCurrencyType);
                //}

            }
        }

        private void ProcessCapitalFreezeMoney(QH_CapitalAccountTableInfo capitalTable)
        {
            int capitalAccountLogo = capitalTable.CapitalAccountLogoId;

            string format1 = "select sum(FreezeAmount) + sum(freezecost) from QH_CapitalAccountFreezeTable "
                             + "where CapitalAccountLogo={0} and FreezeTypeLogo={1}";
            string sql1 = string.Format(format1, capitalAccountLogo, (int)Types.FreezeType.DelegateFreeze);

            object obj = DbHelperSQL.ExecuteSqlScalar(sql1);
            if (obj == null)
                return;

            int sum = 0;
            bool isSuccess = int.TryParse(obj.ToString().Trim(), out sum);
            if (!isSuccess)
                return;
            if (capitalTable.FreezeCapitalTotal == sum)
                return;

            string strSql =
                string.Format(
                    "UPDATE [QH_CapitalAccountTable] SET [FreezeCapitalTotal] = {0} "
                    + "WHERE CapitalAccountLogoId = {1}", sum, capitalAccountLogo);

            //修正为实际的总和
            //capitalTable.FreezeCapitalTotal = sum;

            try
            {
                //QH_CapitalAccountTableDal qhCapitalAccountTableDal = new QH_CapitalAccountTableDal();
                //qhCapitalAccountTableDal.Update(capitalTable);
                DbHelperSQL.ExecuteSql(strSql);

                string format2 = "FutureReckoning.ProcessCapitalFreezeMoney修正现货总冻结资金[初始总冻结资金={0},实际总冻结资金={1}]";
                string desc = string.Format(format2, capitalTable.FreezeCapitalTotal, sum);
                LogHelper.WriteDebug(desc);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 未报、待报的委托状态改为废单,已报待撤、已报、部成、部成待撤的走撤单流程
        /// </summary>
        private void ProcessTodayFutureEntrustStatus()
        {
            //1.检查是否有【委托量=成交量+撤单量】但是状态没有被修改的
            CheckTodayDealStatus();

            //2.未报、待报的委托状态改为废单,已报待撤、已报、部成、部成待撤的走撤单流程
            //TODO:未报、待报的委托状态改为废单,已报待撤、已报、部成、部成待撤的走撤单流程
            /*/// 未报
            [EnumMember] DOSUnRequired = 2,
            /// 待报
            [EnumMember] DOSRequiredSoon = 3,
            /// 已报待撤
            [EnumMember] DOSRequiredRemoveSoon = 4,
            /// 已报
            [EnumMember] DOSIsRequired = 5,
            /// 废单
            [EnumMember] DOSCanceled = 6,
            /// 部成
            [EnumMember] DOSPartDealed = 9,
            /// 部成待撤
            [EnumMember] DOSPartDealRemoveSoon = 11,
            /// 部撤
            //[EnumMember] DOSPartRemoved = 8,
             */
            int DOSUnRequired = (int)Types.OrderStateType.DOSUnRequired;
            int DOSRequiredSoon = (int)Types.OrderStateType.DOSRequiredSoon;
            int DOSRequiredRemoveSoon = (int)Types.OrderStateType.DOSRequiredRemoveSoon;
            int DOSIsRequired = (int)Types.OrderStateType.DOSIsRequired;
            //int DOSCanceled = (int)Types.OrderStateType.DOSCanceled;
            int DOSPartDealed = (int)Types.OrderStateType.DOSPartDealed;
            int DOSPartDealRemoveSoon = (int)Types.OrderStateType.DOSPartDealRemoveSoon;
            int DOSPartRemoved = (int)Types.OrderStateType.DOSPartRemoved;

            //未报,待报
            //string format1 =
            //    "update qh_todayentrusttable set orderstatusid={0} where orderstatusid={1} or orderstatusid={2}";
            //string sql1 = string.Format(format1, DOSCanceled, DOSUnRequired, DOSRequiredSoon);

            //ReckoningTransaction tm = new ReckoningTransaction();
            //Database database = DatabaseFactory.CreateDatabase();
            //using (DbConnection connection = database.CreateConnection())
            //{
            //    try
            //    {
            //        connection.Open();
            //        tm.Database = database;
            //        DbTransaction transaction = connection.BeginTransaction();
            //        tm.Transaction = transaction;
            //        DbHelperSQL.ExecuteSql(sql1, tm);
            //        tm.Transaction.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        LogHelper.WriteError("", ex);
            //        tm.Transaction.Rollback();
            //    }
            //}

            //未报,待报也要走内部撤单流程
            ProcessOrderStatus(DOSUnRequired);
            ProcessOrderStatus(DOSRequiredSoon);

            //已报待撤、已报、部成，部成待撤,部撤,走内部撤单流程(还有已成，已撤2种状态走的是正常清算流程)

            ProcessOrderStatus(DOSRequiredRemoveSoon);
            ProcessOrderStatus(DOSIsRequired);
            ProcessOrderStatus(DOSPartDealed);
            ProcessOrderStatus(DOSPartDealRemoveSoon);
            ProcessOrderStatus(DOSPartRemoved);
            //string format2 = "OrderStatusId = '{0}' OR OrderStatusId = '{1}' OR OrderStatusId = '{2}' OR OrderStatusId = '{3}'";
            //string where = string.Format(format2, DOSRequiredRemoveSoon, DOSIsRequired, DOSPartDealed, DOSPartDealRemoveSoon);

            //var list = DataRepository.XhTodayEntrustTableProvider.Find(where);
            //if (list == null)
            //    return;

            //string message = "盘后撤单，委托作废";
            //foreach (XhTodayEntrustTable tet in list)
            //{
            //    OrderOfferCenter.Instance.IntelnalCancelXHOrder(tet, message);
            //}
        }

        //检查是否有【委托量=成交量+撤单量】但是状态没有被修改的
        private static void CheckTodayDealStatus()
        {
            string where =
                "EntrustAmount=(TradeAmount+CancelAmount) and not (OrderStatusId=6 or OrderStatusId=7 or OrderStatusId=8 or OrderStatusId=10)";

            List<QH_TodayEntrustTableInfo> list = null;
            QH_TodayEntrustTableDal qhTodayEntrustTableDal = new QH_TodayEntrustTableDal();
            try
            {
                list = qhTodayEntrustTableDal.GetListArray(where);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            if (Utils.IsNullOrEmpty(list))
                return;

            List<QH_TodayEntrustTableInfo> list2 = new List<QH_TodayEntrustTableInfo>();

            foreach (QH_TodayEntrustTableInfo table in list)
            {
                if (table.EntrustAmount == table.TradeAmount)
                    table.OrderStatusId = (int)Types.OrderStateType.DOSDealed;
                else if (table.EntrustAmount == table.CancelAmount)
                    table.OrderStatusId = (int)Types.OrderStateType.DOSRemoved;
                else
                    table.OrderStatusId = (int)Types.OrderStateType.DOSPartRemoved;

                list2.Add(table);
            }

            ReckoningTransaction tm = new ReckoningTransaction();
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                try
                {
                    connection.Open();
                    tm.Database = database;
                    DbTransaction transaction = connection.BeginTransaction();
                    tm.Transaction = transaction;
                    foreach (var tableInfo in list2)
                    {
                        qhTodayEntrustTableDal.UpdateRecord(tableInfo, tm);
                    }

                    tm.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError("", ex);
                    tm.Transaction.Rollback();
                }
            }
        }

        private static void ProcessOrderStatus(int status)
        {
            string format = "OrderStatusId={0}";
            string where = string.Format(format, status);

            List<QH_TodayEntrustTableInfo> list = null;
            QH_TodayEntrustTableDal qhTodayEntrustTableDal = new QH_TodayEntrustTableDal();
            bool findSuccess = false;
            try
            {
                list = qhTodayEntrustTableDal.GetListArray(where);
                findSuccess = true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            if (!findSuccess)
            {
                try
                {
                    list = qhTodayEntrustTableDal.GetListArray(where);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }

            if (Utils.IsNullOrEmpty(list))
                return;

            string format2 = "期货盘后撤单，委托作废[OrderStatusID={0},Count={1}]";
            string message = string.Format(format2, status, list.Count);
            LogHelper.WriteDebug(message);

            IList<QH_TodayEntrustTableInfo> unDoneList = new List<QH_TodayEntrustTableInfo>();
            int? breedID;
            foreach (QH_TodayEntrustTableInfo tet in list)
            {
                breedID = MCService.CommonPara.GetBreedClassTypeEnumByCommodityCode(tet.ContractCode);
                bool isSuccess = false;
                if (breedID.HasValue)
                {
                    switch ((CommonObject.Types.BreedClassTypeEnum)breedID.Value)
                    {

                        case GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.StockIndexFuture:
                            //先走股指期货撤单
                            isSuccess = OrderOfferCenter.Instance.IntelnalCancelGZQHOrder(tet, message);
                            break;
                        case GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.CommodityFuture:
                            //不成功再走商品期货撤单
                            isSuccess = OrderOfferCenter.Instance.IntelnalCancelSPQHOrder(tet, message);
                            break;
                    }

                }
                if (!isSuccess)
                {
                    unDoneList.Add(tet);
                }

            }

            //上次未成功执行的再执行一次
            foreach (QH_TodayEntrustTableInfo table in unDoneList)
            {
                breedID = MCService.CommonPara.GetBreedClassTypeEnumByCommodityCode(table.ContractCode);
                bool isSuccess = false;
                if (breedID.HasValue)
                {
                    switch ((CommonObject.Types.BreedClassTypeEnum)breedID.Value)
                    {
                        case GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.StockIndexFuture:
                            isSuccess = OrderOfferCenter.Instance.IntelnalCancelGZQHOrder(table, message);
                            break;
                        case GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.CommodityFuture:
                            isSuccess = OrderOfferCenter.Instance.IntelnalCancelSPQHOrder(table, message);
                            break;
                    }
                }
                if (!isSuccess)
                {
                    LogHelper.WriteInfo("ReckoningService.ProcessOrderStatus期货盘后撤单失败，委托单号=" + table.EntrustNumber);
                }
            }
        }

        /// <summary>
        /// 对每一个用户下的每一对资金和持仓进行清算
        /// </summary>
        /// <param name="capitalAccount">资金账户</param>
        /// <param name="holdAccount">持仓账户</param>
        private bool DoReckoning(UA_UserAccountAllocationTableInfo capitalAccount, UA_UserAccountAllocationTableInfo holdAccount)
        {
            string format = "期货盘后清算DoReckoning[资金账户={0},持仓账户={1}]";
            string msg = string.Format(format, capitalAccount.UserAccountDistributeLogo, holdAccount.UserAccountDistributeLogo);
            LogHelper.WriteDebug(msg);

            //ReckoningTransaction tm = TransactionFactory.GetTransactionManager();
            //tm.BeginTransaction();

            //try
            //{
            ProcessCapital(capitalAccount);
            ProcessHold(holdAccount);
            ProcessTogether(capitalAccount, holdAccount);
            ProcessTodayToHistory(holdAccount);

            return true;

            //    tm.Commit();
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.WriteError("", ex);
            //    tm.Rollback();
            //    return false;
            //}
        }

        /// <summary>
        /// 当日开仓移入到历史开仓中
        /// </summary>
        /// <param name="holdAccount"></param>
        private void ProcessTodayToHistory(UA_UserAccountAllocationTableInfo holdAccount)
        {
            //List<QH_HoldAccountTableInfo> list =
            //   DataRepository.QhHoldAccountTableProvider.GetByUserAccountDistributeLogo(
            //       holdAccount.UserAccountDistributeLogo);

            //if (Utils.IsNullOrEmpty(list))
            //    return;

            //////每一个持仓账户有多个商品的持仓记录, 每个商品只有一条记录
            //foreach (var accountHoldTable in list)
            //{
            //    if(accountHoldTable.TodayHoldAmount<0.00m)
            //        continue;

            //    accountHoldTable.HistoryHoldAmount += accountHoldTable.TodayHoldAmount;
            //    accountHoldTable.TodayHoldAmount = 0;
            //}

            string format =
                "update qh_holdaccounttable set historyholdamount=historyholdamount+todayholdamount,"
                + "todayholdamount=0,HistoryFreezeAmount=HistoryFreezeAmount+TodayFreezeAmount,"
                + "TodayFreezeAmount=0,TodayHoldAveragePrice=0 where useraccountdistributelogo='{0}'";

            string sql = string.Format(format, holdAccount.UserAccountDistributeLogo);
            //LogHelper.WriteDebug("FutureReckoning.ProcessTodayToHistory[SQL=" + sql + "]");

            #region 进行更新

            ReckoningTransaction tm = new ReckoningTransaction();
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                try
                {
                    connection.Open();
                    tm.Database = database;
                    DbTransaction transaction = connection.BeginTransaction();
                    tm.Transaction = transaction;
                    DbHelperSQL.ExecuteSql(sql, tm);
                    tm.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                    tm.Transaction.Rollback();
                }
            }

            #endregion
        }

        #region 清算资金

        /// <summary>
        /// 清算资金
        /// </summary>
        /// <param name="capitalAccount"></param>
        private void ProcessCapital(UA_UserAccountAllocationTableInfo capitalAccount)
        {
            QH_CapitalAccountTableDal uaUserAccountAllocationTableDal = new QH_CapitalAccountTableDal();
            string where = string.Format("UserAccountDistributeLogo = '{0}'", capitalAccount.UserAccountDistributeLogo);
            List<QH_CapitalAccountTableInfo> list =
                uaUserAccountAllocationTableDal.GetListArray(where);

            if (Utils.IsNullOrEmpty(list))
                return;

            //每一个资金账户有多个币种的资金记录，每个币种一条
            foreach (var capitalAccountTable in list)
            {
                //如果当前资金记录没有冻结量，那么不处理
                //if (capitalAccountTable.FreezeCapitalTotal<0.00m)
                //    continue;

                if (capitalAccountTable.FreezeCapitalTotal == 0)
                    continue;

                ProcessCapitalByCurrency(capitalAccountTable);
            }
        }

        /// <summary>
        /// 按币种对资金进行清算
        /// </summary>
        /// <param name="tm"></param>
        /// <param name="capitalAccountTable"></param>
        private void ProcessCapitalByCurrency(ReckoningTransaction tm, QH_CapitalAccountTableInfo capitalAccountTable)
        {
            QH_CapitalAccountFreezeTableDal qhCapitalAccountFreezeTableDal = new QH_CapitalAccountFreezeTableDal();
            string where = string.Format("CapitalAccountLogo = '{0}'", capitalAccountTable.CapitalAccountLogoId);
            List<QH_CapitalAccountFreezeTableInfo> freezeTables =
                qhCapitalAccountFreezeTableDal.GetListArray(where);

            if (Utils.IsNullOrEmpty(freezeTables))
                return;

            decimal freezeMoney = 0;
            List<QH_CapitalAccountFreezeTableInfo> updateFreezeTables = new List<QH_CapitalAccountFreezeTableInfo>();
            foreach (var freezeTable in freezeTables)
            {
                int freezeTypeLogo = freezeTable.FreezeTypeLogo;
                //if (freezeTypeLogo<0.00m)
                //    continue;

                switch (freezeTypeLogo)
                {
                    case (int)Types.FreezeType.DelegateFreeze:

                        freezeMoney += GetUnFreezeMoney(freezeTable);
                        updateFreezeTables.Add(freezeTable);
                        break;

                    //清算冻结，需要判断解冻日期
                    case (int)Types.FreezeType.ReckoningFreeze:


                        DateTime thawTime = (DateTime)freezeTable.ThawTime;
                        if (ReckoningService.IsUnFreezeDate(thawTime))
                        {
                            freezeMoney += GetUnFreezeMoney(freezeTable);
                            updateFreezeTables.Add(freezeTable);
                        }

                        break;
                }
            }

            ///更新资金冻结表
            if (updateFreezeTables.Count > 0)
                foreach (var info in updateFreezeTables)
                {
                    qhCapitalAccountFreezeTableDal.DeleteRecord(info.CapitalFreezeLogoId, tm);
                }


            ///更新资金表
            //if (capitalAccountTable.AvailableCapital<0.00m)
            //    capitalAccountTable.AvailableCapital = 0;

            capitalAccountTable.AvailableCapital += freezeMoney;

            //if (capitalAccountTable.FreezeCapitalTotal<0.00m)
            //    capitalAccountTable.FreezeCapitalTotal = 0;
            capitalAccountTable.FreezeCapitalTotal -= freezeMoney;
            if (capitalAccountTable.FreezeCapitalTotal < 0)
                capitalAccountTable.FreezeCapitalTotal = 0;

            capitalAccountTable.CapitalBalance = capitalAccountTable.AvailableCapital +
                                                 capitalAccountTable.FreezeCapitalTotal;
            QH_CapitalAccountTableDal qhCapitalAccountTableDal = new QH_CapitalAccountTableDal();
            qhCapitalAccountTableDal.UpdateRecord(capitalAccountTable, tm.Database, tm.Transaction);
        }

        /// <summary>
        /// 按币种对资金进行清算
        /// </summary>
        /// <param name="capitalAccountTable"></param>
        private void ProcessCapitalByCurrency(QH_CapitalAccountTableInfo capitalAccountTable)
        {
            QH_CapitalAccountFreezeTableDal qhCapitalAccountFreezeTableDal = new QH_CapitalAccountFreezeTableDal();
            QH_CapitalAccountTableDal qhCapitalAccountTableDal = new QH_CapitalAccountTableDal();
            string where = string.Format("CapitalAccountLogo = '{0}'", capitalAccountTable.CapitalAccountLogoId);
            List<QH_CapitalAccountFreezeTableInfo> freezeTables =
                qhCapitalAccountFreezeTableDal.GetListArray(where);

            decimal freezeMoney = 0;
            bool isUpdate = false;
            if (Utils.IsNullOrEmpty(freezeTables))
            {
                #region 判断即冻结资金还不为0时然而冻结列表也没有数据时那么修改下这个冻结资金
                //update 李健华 2009-12-16为了不改变之前的逻辑，这里多加一个判断即冻结资金还不为0时
                //然而冻结列表也没有数据时那么修改下这个冻结资金
                //目前此部只对期货作修正，现货目前没有发现，所以这里不再做，如果日后发现再做
                //添加清算提交内存表数据和添加冻结列表为0时如果冻结资金还不为0时作修改资金操作
                if (capitalAccountTable.FreezeCapitalTotal != 0)
                {
                    ReckoningTransaction tm1 = new ReckoningTransaction();
                    Database database1 = DatabaseFactory.CreateDatabase();
                    using (DbConnection connection = database1.CreateConnection())
                    {
                        try
                        {
                            connection.Open();
                            tm1.Database = database1;
                            DbTransaction transaction = connection.BeginTransaction();
                            tm1.Transaction = transaction;

                            //更新资金表
                            freezeMoney = capitalAccountTable.FreezeCapitalTotal;
                            capitalAccountTable.AvailableCapital += freezeMoney;
                            capitalAccountTable.FreezeCapitalTotal = 0;

                            capitalAccountTable.CapitalBalance = capitalAccountTable.AvailableCapital +
                                                                 capitalAccountTable.FreezeCapitalTotal;

                            qhCapitalAccountTableDal.UpdateRecord(capitalAccountTable, tm1.Database, tm1.Transaction);
                            tm1.Transaction.Commit();
                            isUpdate = true;
                        }
                        catch (Exception ex)
                        {
                            LogHelper.WriteError(ex.Message, ex);
                            tm1.Transaction.Rollback();
                            isUpdate = false;
                        }
                    }
                    if (isUpdate)
                    {
                        if (freezeMoney != 0)
                        {
                            QHDataAccess.AddQH_CapitalFlow(1, 0, freezeMoney, 0, "", 0, capitalAccountTable.UserAccountDistributeLogo, capitalAccountTable.TradeCurrencyType);
                        }
                    }
                    string format2 = "FutureReckoning.ProcessCapitalByCurrency修正期货总冻结资金[初始总冻结资金={0},实际总冻结资金={1},资金账户ID={2}]";
                    string desc = string.Format(format2, freezeMoney, 0, capitalAccountTable.UserAccountDistributeLogo);
                    LogHelper.WriteDebug(desc);

                }
                #endregion
                return;
            }


            List<QH_CapitalAccountFreezeTableInfo> updateFreezeTables = new List<QH_CapitalAccountFreezeTableInfo>();
            foreach (var freezeTable in freezeTables)
            {
                int freezeTypeLogo = freezeTable.FreezeTypeLogo;
                //if (freezeTypeLogo<0.00m)
                //    continue;

                switch (freezeTypeLogo)
                {
                    case (int)Types.FreezeType.DelegateFreeze:

                        freezeMoney += GetUnFreezeMoney(freezeTable);
                        updateFreezeTables.Add(freezeTable);
                        break;

                    //清算冻结，需要判断解冻日期
                    case (int)Types.FreezeType.ReckoningFreeze:


                        DateTime thawTime = (DateTime)freezeTable.ThawTime;
                        if (ReckoningService.IsUnFreezeDate(thawTime))
                        {
                            freezeMoney += GetUnFreezeMoney(freezeTable);
                            updateFreezeTables.Add(freezeTable);
                        }

                        break;
                }
            }

            #region 进行更新

            ReckoningTransaction tm = new ReckoningTransaction();
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                try
                {
                    connection.Open();
                    tm.Database = database;
                    DbTransaction transaction = connection.BeginTransaction();
                    tm.Transaction = transaction;
                    ///更新资金冻结表
                    if (updateFreezeTables.Count > 0)
                        foreach (var info in updateFreezeTables)
                        {
                            qhCapitalAccountFreezeTableDal.DeleteRecord(info.CapitalFreezeLogoId, tm);
                        }


                    ///更新资金表
                    //if (capitalAccountTable.AvailableCapital<0.00m)
                    //    capitalAccountTable.AvailableCapital = 0;

                    capitalAccountTable.AvailableCapital += freezeMoney;

                    //if (capitalAccountTable.FreezeCapitalTotal<0.00m)
                    //    capitalAccountTable.FreezeCapitalTotal = 0;
                    capitalAccountTable.FreezeCapitalTotal -= freezeMoney;
                    if (capitalAccountTable.FreezeCapitalTotal < 0)
                        capitalAccountTable.FreezeCapitalTotal = 0;

                    capitalAccountTable.CapitalBalance = capitalAccountTable.AvailableCapital +
                                                         capitalAccountTable.FreezeCapitalTotal;

                    qhCapitalAccountTableDal.UpdateRecord(capitalAccountTable, tm.Database,
                                                          tm.Transaction);
                    tm.Transaction.Commit();
                    isUpdate = true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                    tm.Transaction.Rollback();
                    isUpdate = false;
                }
            }

            //这里不放在上面，是因为这是新增的需求，就算这记录不成功也不影响之前的相关操作
            //2009-12-2 李健华add 添加期货资金流水表记录========start===
            if (isUpdate)
            {
                //QH_TradeCapitalFlowDetailInfo detailInfo = new QH_TradeCapitalFlowDetailInfo();
                //detailInfo.FlowTypes = 1;
                //detailInfo.Margin = 0;
                //detailInfo.OtherCose = 0;
                //detailInfo.ProfitLoss = 0;
                //detailInfo.TradeID = Guid.NewGuid().ToString();
                //detailInfo.TradeProceduresFee = 0;
                //detailInfo.UserCapitalAccount = capitalAccountTable.UserAccountDistributeLogo;
                //QHDataAccess.AddQH_CapitalFlow(detailInfo);
                if (freezeMoney != 0)
                {
                    QHDataAccess.AddQH_CapitalFlow(1, 0, freezeMoney, 0, "", 0, capitalAccountTable.UserAccountDistributeLogo, capitalAccountTable.TradeCurrencyType);
                }
            }
            //=============end===============



            #endregion
        }


        private decimal GetUnFreezeMoney(QH_CapitalAccountFreezeTableInfo freezeTable)
        {
            decimal result = 0;

            //if (freezeTable.FreezeAmount<0.00m)
            //    freezeTable.FreezeAmount = 0;
            //if (freezeTable.FreezeCost<0.00m)
            //    freezeTable.FreezeCost = 0;

            result = freezeTable.FreezeAmount + freezeTable.FreezeCost;

            //获取值后赋零
            freezeTable.FreezeAmount = 0;
            freezeTable.FreezeCost = 0;

            return result;
        }

        #endregion

        #region 清算持仓

        /// <summary>
        /// 清算持仓
        /// </summary>
        /// <param name="holdAccount"></param>
        private void ProcessHold(UA_UserAccountAllocationTableInfo holdAccount)
        {
            string where = string.Format("UserAccountDistributeLogo = '{0}'", holdAccount.UserAccountDistributeLogo);
            QH_HoldAccountTableDal qhHoldAccountTableDal = new QH_HoldAccountTableDal();
            List<QH_HoldAccountTableInfo> list =
                qhHoldAccountTableDal.GetListArray(
                    where);

            if (Utils.IsNullOrEmpty(list))
                return;

            ////每一个持仓账户有多个商品的持仓记录, 每个商品只有一条记录
            foreach (var accountHoldTable in list)
            {
                ProcessHoldByCode(accountHoldTable);
            }
        }

        /// <summary>
        /// 按商品代码对持仓进行清算
        /// </summary>
        /// <param name="holdAccountTable"></param>
        private void ProcessHoldByCode(QH_HoldAccountTableInfo holdAccountTable)
        {
            //AccountHoldLogoId
            string where = string.Format("AccountHoldLogo = {0}", holdAccountTable.AccountHoldLogoId);
            QH_HoldAccountFreezeTableDal qhHoldAccountFreezeTableDal = new QH_HoldAccountFreezeTableDal();
            List<QH_HoldAccountFreezeTableInfo> freezeTables = qhHoldAccountFreezeTableDal.GetListArray(where);

            if (Utils.IsNullOrEmpty(freezeTables))
                return;
            int todayFreezeHold = 0;
            int historyFreezeHold = 0;

            List<QH_HoldAccountFreezeTableInfo> updateFreezeTables = new List<QH_HoldAccountFreezeTableInfo>();
            foreach (var freezeTable in freezeTables)
            {
                int freezeTypeLogo = freezeTable.FreezeTypeLogo;
                //if (freezeTypeLogo<0.00m)
                //    continue;

                switch (freezeTypeLogo)
                {
                    //今日开仓平仓冻结
                    case (int)Types.FreezeType.TodayHoldingCloseFreeze:
                        if (freezeTable.ThawTime == null)
                            continue;


                        DateTime thawTime1 = (DateTime)freezeTable.ThawTime;
                        if (ReckoningService.IsUnFreezeDate(thawTime1))
                        {
                            //if (freezeTable.FreezeAmount<0.00m)
                            //    freezeTable.FreezeAmount = 0;

                            todayFreezeHold += freezeTable.FreezeAmount;

                            updateFreezeTables.Add(freezeTable);
                        }
                        break;

                    //历史开仓平仓冻结
                    case (int)Types.FreezeType.HistoryHoldingCloseFreeze:
                        if (freezeTable.ThawTime == null)
                            continue;


                        DateTime thawTime2 = (DateTime)freezeTable.ThawTime;
                        if (ReckoningService.IsUnFreezeDate(thawTime2))
                        {
                            //if (freezeTable.FreezeAmount<0.00m)
                            //    freezeTable.FreezeAmount = 0;

                            historyFreezeHold += freezeTable.FreezeAmount;

                            updateFreezeTables.Add(freezeTable);
                        }

                        break;
                }
            }

            #region 进行更新

            try
            {
                DataManager.ExecuteInTransaction(rt =>
                                                     {
                                                         ///更新持仓冻结表
                                                         if (updateFreezeTables.Count > 0)
                                                         {
                                                             foreach (var uf in updateFreezeTables)
                                                             {
                                                                 qhHoldAccountFreezeTableDal.DeleteRecord(
                                                                     uf.HoldFreezeLogoId, rt);
                                                             }
                                                         }

                                                         ///更新持仓表
                                                         //if (holdAccountTable.TodayHoldAmount<0.00m)
                                                         //    holdAccountTable.TodayHoldAmount = 0;
                                                         holdAccountTable.TodayHoldAmount += todayFreezeHold;

                                                         //if (holdAccountTable.TodayFreezeAmount<0.00m)
                                                         //    holdAccountTable.TodayFreezeAmount = 0;
                                                         holdAccountTable.TodayFreezeAmount -= todayFreezeHold;
                                                         if (holdAccountTable.TodayFreezeAmount < 0)
                                                             holdAccountTable.TodayFreezeAmount = 0;

                                                         //if (holdAccountTable.HistoryHoldAmount<0.00m)
                                                         //    holdAccountTable.HistoryHoldAmount = 0;
                                                         holdAccountTable.HistoryHoldAmount += historyFreezeHold;

                                                         //if (holdAccountTable.HistoryFreezeAmount<0.00m)
                                                         //    holdAccountTable.HistoryFreezeAmount = 0;
                                                         holdAccountTable.HistoryFreezeAmount -= historyFreezeHold;
                                                         if (holdAccountTable.HistoryFreezeAmount < 0)
                                                             holdAccountTable.HistoryFreezeAmount = 0;

                                                         QH_HoldAccountTableDal qhHoldDal = new QH_HoldAccountTableDal();
                                                         qhHoldDal.Update(holdAccountTable, rt);
                                                     });
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            //ReckoningTransaction reckoningTransaction = new ReckoningTransaction();
            //Database database = DatabaseFactory.CreateDatabase();
            //DbConnection connection = database.CreateConnection();
            //try
            //{
            //    connection.Open();
            //    reckoningTransaction.Database = database;
            //    DbTransaction transaction = connection.BeginTransaction();
            //    reckoningTransaction.Transaction = transaction;
            //    ///更新持仓冻结表
            //    if (updateFreezeTables.Count > 0)
            //    {
            //        foreach (var uf in updateFreezeTables)
            //        {
            //            qhHoldAccountFreezeTableDal.DeleteRecord(uf.HoldFreezeLogoId, reckoningTransaction);
            //        }
            //    }

            //    ///更新持仓表
            //    //if (holdAccountTable.TodayHoldAmount<0.00m)
            //    //    holdAccountTable.TodayHoldAmount = 0;
            //    holdAccountTable.TodayHoldAmount += todayFreezeHold;

            //    //if (holdAccountTable.TodayFreezeAmount<0.00m)
            //    //    holdAccountTable.TodayFreezeAmount = 0;
            //    holdAccountTable.TodayFreezeAmount -= todayFreezeHold;
            //    if (holdAccountTable.TodayFreezeAmount < 0)
            //        holdAccountTable.TodayFreezeAmount = 0;

            //    //if (holdAccountTable.HistoryHoldAmount<0.00m)
            //    //    holdAccountTable.HistoryHoldAmount = 0;
            //    holdAccountTable.HistoryHoldAmount += historyFreezeHold;

            //    //if (holdAccountTable.HistoryFreezeAmount<0.00m)
            //    //    holdAccountTable.HistoryFreezeAmount = 0;
            //    holdAccountTable.HistoryFreezeAmount -= historyFreezeHold;
            //    if (holdAccountTable.HistoryFreezeAmount < 0)
            //        holdAccountTable.HistoryFreezeAmount = 0;

            //    QH_HoldAccountTableDal qhHoldDal = new QH_HoldAccountTableDal();
            //    qhHoldDal.Update(holdAccountTable, reckoningTransaction);
            //    reckoningTransaction.Transaction.Commit();
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.WriteError(ex.Message, ex);
            //    reckoningTransaction.Transaction.Rollback();
            //}

            #endregion
        }

        #endregion

        #region 期货结算资金和持仓

        /// <summary>
        /// 期货结算资金和持仓（价格计算与保证金判断）
        /// </summary>
        /// <param name="capitalAccount"></param>
        /// <param name="holdAccount"></param>
        private void ProcessTogether(UA_UserAccountAllocationTableInfo capitalAccount, UA_UserAccountAllocationTableInfo holdAccount)
        {
            ProcessCapitalHoldPairs(capitalAccount, holdAccount);

            if (dict.Count == 0)
                return;

            foreach (KeyValuePair<int, QHCapitalHoldPair> pair in dict)
            {
                ProcessCapitalAndHoldByCurrency(pair);
            }
        }

        protected void CheckHoldNullValue(QH_HoldAccountTableInfo aht)
        {
            if (aht == null)
                return;

            //if (aht.HistoryHoldAmount<0.00m)
            //    aht.HistoryHoldAmount = 0;

            //if (aht.HistoryFreezeAmount<0.00m)
            //    aht.HistoryFreezeAmount = 0;

            //if (aht.TodayHoldAmount<0.00m)
            //    aht.TodayHoldAmount = 0;

            //if (aht.TodayFreezeAmount<0.00m)
            //    aht.TodayFreezeAmount = 0;

            //if (aht.BreakevenPrice<0.00m)
            //    aht.BreakevenPrice = 0;

            //if (aht.CostPrice<0.00m)
            //    aht.CostPrice = 0;

            //if (aht.HoldAveragePrice<0.00m)
            //    aht.HoldAveragePrice = 0;

            //if (aht.Margin<0.00m)
            //    aht.Margin = 0;

            //if (aht.ProfitLoss<0.00m)
            //    aht.ProfitLoss = 0;

            //if (aht.TodayHoldAveragePrice<0.00m)
            //    aht.TodayHoldAveragePrice = 0;
        }

        /// <summary>
        /// 根据币种对资金和持仓进行结算
        /// </summary>
        /// <param name="pair"></param>
        private void ProcessCapitalAndHoldByCurrency(KeyValuePair<int, QHCapitalHoldPair> pair)
        {
            QH_CapitalAccountTableInfo capitalAccountTable = pair.Value.Capital;
            List<QH_HoldAccountTableInfo> holdAccountTables = pair.Value.Hold;

            if (holdAccountTables == null)
                return;
            decimal marginSum = 0; //保证金总额
            decimal profitLossSum = 0; //总的持仓盈亏
            decimal marginDiffSum = 0; //结算保证金差额

            //1 将持仓表中的 持仓均价 更新为 行情中 的 今收盘的结算价格
            foreach (var holdAccountTable in holdAccountTables)
            {
                CheckHoldNullValue(holdAccountTable);

                var contract = holdAccountTable.Contract;

                #region 今日收盘结算价

                decimal price = 0;
                string msg = "";

                //获取今日结算价
                bool canGetPrice = MCService.GetFutureTodayPreSettlementPrice(contract, out price, ref msg);
                if (!canGetPrice)
                {
                    string format = "FutureReckoning.ProcessCapitalAndHoldByCurrency无法获取合约{0}的今日收盘结算价";
                    string msg2 = string.Format(format, contract);
                    LogHelper.WriteDebug(msg2);

                    //update 李健华 2010-01-11 获取不到今日结算价就以持仓均价来计算不能不计算，要不然后面的总保证金就出现错误
                    //update 李健华 2010-02-02 返是修改回原来一样，获取不到今日结算价就不能不计算和清算本条记录，要不然后面的总保证金就出现错误
                    continue;
                    //price = holdAccountTable.HoldAveragePrice;
                    //===========

                }

                #endregion

                #region 参数

                //合约乘数
                decimal scale = MCService.GetFutureTradeUntiScale(contract);
                //保证金比例
                decimal futureBail = 0;
                //盘后清算，如果获取不到比例或者行情获取不到时候不作调整

                try
                {
                    futureBail = MCService.GetFutureBailScale(contract) / 100;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError("GT-9001:清算时获取保证金异常,保证不作调整,合约代码：" + contract + ex.Message, ex);
                }
                //持仓均价
                decimal averagePrice = holdAccountTable.HoldAveragePrice;

                #endregion

                #region 计算持仓盈亏

                decimal holdSum = holdAccountTable.TodayHoldAmount + holdAccountTable.HistoryHoldAmount
                                  + holdAccountTable.TodayFreezeAmount + holdAccountTable.HistoryFreezeAmount;
                //LogHelper.WriteDebug("HoldSum=" + holdAccountTable);
                //string formatHold =
                //    "FutureReckoning.ProcessCapitalAndHoldByCurrency获取总持仓[ID={0},HistoryHold={1},TodayHold={2}]";
                //string msgHold = string.Format(formatHold, holdAccountTable.UserAccountDistributeLogo,
                //                               holdAccountTable.HistoryHoldAmount, holdAccountTable.TodayHoldAmount);

                //LogHelper.WriteDebug(msgHold);

                if (holdSum > 0)
                {
                    decimal profitLoss = 0;
                    if (holdAccountTable.BuySellTypeId == (int)CommonObject.Types.TransactionDirection.Buying)
                    {
                        #region 买持仓

                        //持仓盈亏=（今日结算价-买入持仓均价）*总的持仓量*合约乘数
                        profitLoss = (price - averagePrice) * holdSum * scale;
                        string format1 =
                            "期货盘后资金清算1[持仓ID={0},账户={1},合约={2},持仓盈亏{3}=（今日结算价{4}-买入持仓均价{5}）*总的持仓量{6}*合约乘数{7}，买卖方向={8}]";
                        string desc1 = string.Format(format1, holdAccountTable.AccountHoldLogoId,
                                                     holdAccountTable.UserAccountDistributeLogo, contract, profitLoss,
                                                     price, averagePrice, holdSum, scale,
                                                     holdAccountTable.BuySellTypeId);
                        LogHelper.WriteDebug(desc1);

                        #endregion
                    }
                    else
                    {
                        #region 卖持仓

                        //持仓盈亏=（卖出持仓均价-今日结算价）*总的持仓量*合约乘数
                        profitLoss = (averagePrice - price) * holdSum * scale;
                        string format2 =
                            "期货盘后资金清算1[持仓ID={0},账户={1},合约={2},持仓盈亏{3}=（卖出持仓均价{4}-今日结算价{5}）*总的持仓量{6}*合约乘数{7}，买卖方向={8}]";
                        string desc2 = string.Format(format2, holdAccountTable.AccountHoldLogoId,
                                                     holdAccountTable.UserAccountDistributeLogo, contract, profitLoss,
                                                     averagePrice, price, holdSum, scale,
                                                     holdAccountTable.BuySellTypeId);
                        LogHelper.WriteDebug(desc2);

                        #endregion
                    }

                    profitLossSum += profitLoss;
                }

                #endregion

                #region 计算保证金差额

                //decimal marginDiff = averagePrice * holdSum * scale * futureBail;
                //为避免出错，差值直接赋为老保证金值 20090625
                decimal marginDiff = holdAccountTable.Margin;
                //还未减去保证金总额，在循环外面减
                marginDiffSum += marginDiff;

                decimal margin = marginDiff;
                //如果保证金比例没有获取得到即不作调整
                if (futureBail > 0)
                {
                    margin = price * holdSum * scale * futureBail;
                }

                holdAccountTable.Margin = margin;
                marginSum += margin;

                //decimal marginDiff = averagePrice*holdSum*scale*futureBail;

                #endregion

                #region 更新成本价，保本价和持仓均价

                if (holdSum != 0)
                {
                    LogHelper.WriteDebug("合约代码=" + holdAccountTable.Contract + "，账号=" + holdAccountTable.UserAccountDistributeLogo);
                    LogHelper.WriteDebug("计算成本价：holdAccountTable.CostPrice - (price - holdAccountTable.HoldAveragePrice) = " + holdAccountTable.CostPrice + " - (" + price + " - " + holdAccountTable.HoldAveragePrice + ") = " + (holdAccountTable.CostPrice - (price - holdAccountTable.HoldAveragePrice)));

                    //修改持仓表中的 成本价 字段=最近的成本价-（今日结算价-最近的持仓均价）
                    holdAccountTable.CostPrice = holdAccountTable.CostPrice -
                                                 (price - holdAccountTable.HoldAveragePrice);
                    //修改持仓表中的 保本价 字段=收盘调整后成本价格-平仓手续费/持仓量
                    /*QHCostResult qhCost = MCService.ComputeQHCost(holdAccountTable.Contract,
                                                                  (float)holdAccountTable.BreakevenPrice,
                                                                  (int)holdSum,
                                                                  (CommonObject.Types.UnitType)
                                                                  MCService.GetFutureTradeUnit(contract),
                                                                  Types.FutureOpenCloseType.
                                                                      ClosePosition);

                    //新的保本价

                    holdAccountTable.BreakevenPrice = holdAccountTable.CostPrice - qhCost.Cosing / holdSum;*/
                    var holdPrice = MCService.GetHoldPrice(holdAccountTable.Contract, holdAccountTable.CostPrice,
                                                           holdSum, holdAccountTable.BuySellTypeId);
                    holdAccountTable.BreakevenPrice = holdPrice;

                    //修改持仓表中的 持仓均价
                    string format = "期货盘后资金清算2修改持仓均价[AccountHoldLogoId={0},Contract={1},原持仓均价={2},新持仓均价(即结算价)={3}]";
                    string msgPrice = string.Format(format, holdAccountTable.AccountHoldLogoId,
                                                    holdAccountTable.Contract,
                                                    holdAccountTable.HoldAveragePrice, price);

                    holdAccountTable.HoldAveragePrice = price;

                    LogHelper.WriteDebug(msgPrice);
                }
                else
                {
                    holdAccountTable.CostPrice = 0;
                    holdAccountTable.BreakevenPrice = 0;
                    holdAccountTable.HoldAveragePrice = 0;
                }

                #endregion
            }

            //2 修改资金表中的 可用资金+=所有合约的持仓盈亏+结算保证差额
            marginDiffSum -= marginSum;
            capitalAccountTable.AvailableCapital += profitLossSum + marginDiffSum;
            string format3 = "期货盘后资金清算2[资金ID={0},账户={1},可用资金{2}+=所有合约的持仓盈亏{3}+结算保证差额{4}]";
            string desc3 = string.Format(format3, capitalAccountTable.CapitalAccountLogoId,
                                         capitalAccountTable.UserAccountDistributeLogo,
                                         capitalAccountTable.AvailableCapital, profitLossSum, marginDiffSum);
            LogHelper.WriteDebug(desc3);

            //修改资金表中的 资金余额=可用资金+冻结资金(计算字段，不需要再计算)
            //capitalAccountTable.CapitalBalance = (capitalAccountTable.AvailableCapital +
            //                                      capitalAccountTable.FreezeCapitalTotal);
            string format4 = "期货盘后资金清算3[资金ID={0},账户={1},上日总保证金={2},当前保证金={3}]";
            string desc4 = string.Format(format4, capitalAccountTable.CapitalAccountLogoId,
                                         capitalAccountTable.UserAccountDistributeLogo,
                                         capitalAccountTable.MarginTotal, marginSum);
            LogHelper.WriteDebug(desc4);
            capitalAccountTable.MarginTotal = marginSum;

            #region 进行更新
            bool isUpdate = false;
            ReckoningTransaction tm = new ReckoningTransaction();
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                try
                {
                    connection.Open();
                    tm.Database = database;
                    DbTransaction transaction = connection.BeginTransaction();
                    tm.Transaction = transaction;
                    QH_HoldAccountTableDal qhHoldAccountTableDal = new QH_HoldAccountTableDal();
                    QH_CapitalAccountTableDal qhCapitalAccountTableDal = new QH_CapitalAccountTableDal();
                    //更新到数据表中
                    //DataRepository.QhHoldAccountTableProvider.Update(tm, holdAccountTables);
                    foreach (var holdAccountTable in holdAccountTables)
                    {
                        qhHoldAccountTableDal.Update(holdAccountTable, tm);
                    }
                    qhCapitalAccountTableDal.UpdateRecord(capitalAccountTable, tm.Database,
                                                          tm.Transaction);
                    tm.Transaction.Commit();
                    isUpdate = true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                    tm.Transaction.Rollback();
                    isUpdate = false;
                }
            }
            //这里不放在上面，是因为这是新增的需求，就算这记录不成功也不影响之前的相关操作
            //2009-12-2 李健华add 添加期货资金流水表记录========start===
            if (isUpdate)
            {
                //保证金差额为0和盯市盈亏也为0即资金没有变动不用记录
                if (marginDiffSum != 0 || profitLossSum != 0)
                {
                    QHDataAccess.AddQH_CapitalFlow(1, marginDiffSum, 0, profitLossSum, "", 0, capitalAccountTable.UserAccountDistributeLogo, capitalAccountTable.TradeCurrencyType);
                }
            }
            //=============end===============
            #endregion
        }

        /// <summary>
        /// 根据币种进行资金和持仓的配对
        /// </summary>
        /// <param name="capitalAccount"></param>
        /// <param name="holdAccount"></param>
        private void ProcessCapitalHoldPairs(UA_UserAccountAllocationTableInfo capitalAccount, UA_UserAccountAllocationTableInfo holdAccount)
        {
            string where = string.Format("UserAccountDistributeLogo = '{0}'", capitalAccount.UserAccountDistributeLogo);
            QH_CapitalAccountTableDal qhCapitalAccountTableDal = new QH_CapitalAccountTableDal();
            List<QH_CapitalAccountTableInfo> list = qhCapitalAccountTableDal.GetListArray(where);

            if (Utils.IsNullOrEmpty(list))
            {
                return;
            }

            //每一个资金账户有多个币种的资金表
            foreach (var capital in list)
            {
                if (capital.TradeCurrencyType > 0)
                {
                    QHCapitalHoldPair pair = new QHCapitalHoldPair();
                    pair.Capital = capital;
                    pair.Hold = new List<QH_HoldAccountTableInfo>();
                    dict[capital.TradeCurrencyType] = pair;
                }
            }
            QH_HoldAccountTableDal qhHoldAccountTableDal = new QH_HoldAccountTableDal();
            where = string.Format("UserAccountDistributeLogo = '{0}'", holdAccount.UserAccountDistributeLogo);
            List<QH_HoldAccountTableInfo> list2 = qhHoldAccountTableDal.GetListArray(where);

            if (Utils.IsNullOrEmpty(list2))
                return;

            //每一个持仓账户有多个币种的资金表
            foreach (var hold in list2)
            {
                //if (hold.TradeCurrencyType<0.00m)
                //    continue;

                if (dict.ContainsKey(hold.TradeCurrencyType))
                {
                    QHCapitalHoldPair pair = dict[hold.TradeCurrencyType];

                    if (pair.Hold == null)
                    {
                        pair.Hold = new List<QH_HoldAccountTableInfo>();
                    }
                    pair.Hold.Add(hold);
                }
            }
        }

        #endregion
    }

    internal class QHCapitalHoldPair
    {
        public QH_CapitalAccountTableInfo Capital { get; set; }

        public List<QH_HoldAccountTableInfo> Hold { get; set; }
    }
}