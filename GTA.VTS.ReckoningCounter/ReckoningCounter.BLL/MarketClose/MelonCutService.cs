#region Using Namespace

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using GTA.VTS.Common.CommonUtility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.BLL.ScheduleManagement;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.Entity.Contants;
using ReckoningCounter.MemoryData;
using ReckoningCounter.Model;
using ReckoningCounter.BLL.Reckoning.Logic;

#endregion

namespace ReckoningCounter.BLL.MarketClose
{
    /// <summary>
    /// 分红处理类
    /// 作者：宋涛
    /// 日期：2008-12-20
    /// Update By:李健华
    /// Update Date:2010-06-11
    /// Desc.:修改分红相关需求设置在开市前进行分红
    /// </summary>
    public class MelonCutService
    {

        /// <summary>
        /// 分红登记表中的记录过期天数
        /// </summary>
        private const int expireDays = 30;

        /// <summary>
        /// 进行分红处理
        /// </summary>
        public static bool Process()
        {
            try
            {


                //1.清除分红表中过期的数据（分红日期已超过一个月的全部删除）
                bool isSuccess = ClearHistoryData();
                if (!isSuccess)
                {
                    LogHelper.WriteInfo("MeloncutService清除分红表中过期的数据失败！");
                    return false;
                }

                //2.处理现金分红
                isSuccess = ProcessCStockMelonCash();
                if (!isSuccess)
                {
                    LogHelper.WriteInfo("MeloncutService处理现金分红失败！");
                    return false;
                }

                //3.处理股票分红
                isSuccess = ProcessCStockMelonStock();
                if (!isSuccess)
                {
                    LogHelper.WriteInfo("MeloncutService处理股票分红失败！");
                    return false;
                }

                //4.更新数据库中的分红日期
                StatusTableChecker.UpdateMelonCutDate();

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
            return true;
        }

        /// <summary>
        /// 登录分红记录(每天清算的时候记录)
        /// </summary>
        /// <returns></returns>
        public static bool RegisterMeloncutTable()
        {
            //今天已经登记过
            if (StatusTableChecker.HasDoneRegistMelonCutDate(DateTime.Now))
            {
                return true;
            }
            //1.如果今日有需要登记的就进行登记
            IList<CM_StockMelonCash> list = MCService.CommonPara.GetAllStockMelonCash();
            bool isSuccess = true;
            if (!Utils.IsNullOrEmpty(list))
            {
                isSuccess = RegisterCashList(list);
            }

            if (isSuccess)
            {
                IList<CM_StockMelonStock> items = MCService.CommonPara.GetAllStockMelonStock();
                if (!Utils.IsNullOrEmpty(items))
                {
                    isSuccess = RegisterStockList(items);
                }
            }
            if (isSuccess)
            {
                StatusTableChecker.UpdateRegistMelonCutDate(null);
            }
            return isSuccess;
        }

        #region 清除过期数据

        /// <summary>
        /// 清除今日过期的股票分红登记信息(即分红日期已经过去N天的记录)
        /// </summary>
        private static bool ClearHistoryData()
        {
            bool result = false;
            LogHelper.WriteInfo("MelonCutService.ClearHistoryData清除今日过期的股票分红登记信息(即分红日期已经过去N天的记录)");

            string tabName = "XH_MelonCutRegisterTable";
            string sql = string.Format("delete from {0} where DATEDIFF(DAY,CutDate,GETDATE())>{1}", tabName,
                                       expireDays);

            // TransactionManager tm = TransactionFactory.GetTransactionManager();
            // tm.BeginTransaction();

            try
            {
                // DataRepository.Provider.ExecuteNonQuery(tm, CommandType.Text, sql);
                // tm.Commit();
                DbHelperSQL.ExecuteSql(sql);
                result = true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("", ex);
                //tm.Rollback();
            }

            return result;
        }

        #endregion

        #region 功能函数

        /// <summary>
        /// 是否是今天 
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>是否是今天</returns>
        private static bool CheckDate(DateTime? date)
        {
            if (!date.HasValue)
                return false;

            ////根据要求提前一天
            //DateTime now = DateTime.Now.AddDays(1);
            //update 2010-06-11
            DateTime now = DateTime.Now;

            DateTime aDate = date.Value;
            if (aDate.Year == now.Year && aDate.Month == now.Month && aDate.Day == now.Day)
                return true;

            return false;
        }

        /// <summary>
        /// 是否是今天登记
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>是否是今天登记</returns>
        private static bool CheckRegDate(DateTime? date)
        {
            if (!date.HasValue)
                return false;

            DateTime now = DateTime.Now;

            DateTime aDate = date.Value;
            if (aDate.Year == now.Year && aDate.Month == now.Month && aDate.Day == now.Day)
                return true;

            return false;
        }

        /// <summary>
        /// 是否已经过期
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>是否已经过期</returns>
        private static bool IsExpire(DateTime? date)
        {
            if (!date.HasValue)
                return false;


            DateTime now = DateTime.Now;
            DateTime aDate = date.Value;

            TimeSpan ts = now - aDate;
            if (ts.Days > expireDays)
                return true;

            return false;
        }

        /// <summary>
        /// 对某个股票进行登记
        /// </summary>
        /// <param name="tables">登记列表</param>
        private static void SaveRegisterTables(List<XH_MelonCutRegisterTableInfo> tables)
        {
            if (Utils.IsNullOrEmpty(tables))
                return;

            //TransactionManager tm = TransactionFactory.GetTransactionManager();
            //tm.BeginTransaction();

            try
            {
                //DataRepository.XhMelonCutRegisterTableProvider.BulkInsert(tables);
                //foreach (var table in tables)
                //{
                //    DataRepository.XhMelonCutRegisterTableProvider.Insert(tm, table);
                //}
                //tm.Commit();
                DbHelperSQL.Add(tables);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("对某个股票进行分红登记异常", ex);
                //tm.Rollback();
            }
        }

        /// <summary>
        /// 根据股票代码获取全部的持仓
        /// </summary>
        /// <param name="code">股票代码</param>
        /// <returns>返回对应的持仓</returns>
        private static List<XH_AccountHoldTableInfo> GetAccountHoldListByCode(string code)
        {
            if (String.IsNullOrEmpty(code))
                return null;

            string where = string.Format("Code = '{0}'", code);
            XH_AccountHoldTableDal dal = new XH_AccountHoldTableDal();
            return dal.GetListArray(where);
            // return DataRepository.XhAccountHoldTableProvider.Find(where);
        }

        private static List<XH_MelonCutRegisterTableInfo> GetMeloncutRegisterList(string code, DateTime regDate,
                                                                                  DateTime cutDate,
                                                                                  Types.MeloncutType cutType)
        {
            if (String.IsNullOrEmpty(code))
                return null;
            XH_MelonCutRegisterTableDal dal = new XH_MelonCutRegisterTableDal();

            string where = string.Format("Code = '{0}' AND RegisterDate = '{1}' AND CutDate ='{2}' AND CutType ='{3}'",
                                         code, regDate, cutDate, (int)cutType);
            return dal.GetListArray(where);
            // return DataRepository.XhMelonCutRegisterTableProvider.Find(where);
        }

        /// <summary>
        /// 获取资金账号信息，这里从内存中获取，如果没有会内部从数据库中获取（但这里会保证当前内存表是已经开启）
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="currencyType"></param>
        /// <returns></returns>
        private static List<XH_CapitalAccountTableInfo> GetCaptitalAccountList(string userID, int currencyType)
        {
            if (String.IsNullOrEmpty(userID))
                return null;

            #region update by:李健华 date: 2009-07-22 修改从内存表中获取数据

            //===old==
            //string where = string.Format("UserAccountDistributeLogo = '{0}' AND TradeCurrencyType = '{1}'",
            //                             userID, currencyType);
            //XH_CapitalAccountTableDal dal = new XH_CapitalAccountTableDal();
            //return dal.GetListArray(where);
            //====end==
            List<XH_CapitalAccountTableInfo> list = new List<XH_CapitalAccountTableInfo>();
            list.Add(MemoryDataManager.XHCapitalMemoryList.GetByCapitalAccountAndCurrencyType(userID, currencyType).Data);
            return list;

            #endregion

            //return DataRepository.XhCapitalAccountTableProvider.Find(where);
        }

        private static List<XH_AccountHoldTableInfo> GetAccountHoldList(string userID, int currencyType, string code)
        {
            if (String.IsNullOrEmpty(userID))
                return null;

            string where = string.Format(
                "UserAccountDistributeLogo = '{0}' AND CurrencyTypeId = '{1}' AND Code = '{2}'",
                userID, currencyType, code);
            XH_AccountHoldTableDal dal = new XH_AccountHoldTableDal();
            return dal.GetListArray(where);
            //return DataRepository.XhAccountHoldTableProvider.Find(where);
        }


        #endregion

        #region 现金分红转账

        /// <summary>
        /// 处理现金分红（分红转账 ）
        /// </summary>
        private static bool ProcessCStockMelonCash()
        {
            XH_MelonCutRegisterTableDal dal = new XH_MelonCutRegisterTableDal();

            string where = string.Format(" CutType ='{0}'", (int)Types.MeloncutType.Cash);
            List<XH_MelonCutRegisterTableInfo> list = dal.GetListArray(where);

            //选出今日需要分红的股票分红信息
            var q = from c in list
                    where
                        CheckDate(c.CutDate) && c.RegisterAmount > 0 && !string.IsNullOrEmpty(c.Code)
                    select c;
            bool issuss = true;
            if (q != null)
            {
                //2.如果今日有需要现金分红的就进行分红
                issuss = ProcessCashCut(q.ToList<XH_MelonCutRegisterTableInfo>());

            }

            return issuss;
        }

        /// <summary>
        /// 进行现金分红登记
        /// </summary>
        /// <param name="list">现金分红信息列表</param>
        private static bool RegisterCashList(IList<CM_StockMelonCash> list)
        {
            try
            {
                //选出今日需要登记的股票现金分红信息
                var q = from c in list
                        where
                            CheckRegDate(c.StockRightRegisterDate) && c.StockRightLogoutDatumDate.HasValue &&
                            c.RatioOfCashDividend.HasValue
                        select c;

                //处理每一个股票的现金分红
                foreach (CM_StockMelonCash cash in q)
                {
                    //获取所有有此股票的持仓记录
                    List<XH_AccountHoldTableInfo> holdTables = GetAccountHoldListByCode(cash.CommodityCode);
                    if (Utils.IsNullOrEmpty(holdTables))
                        continue;

                    List<XH_MelonCutRegisterTableInfo> tables = new List<XH_MelonCutRegisterTableInfo>();

                    foreach (XH_AccountHoldTableInfo holdTable in holdTables)
                    {
                        XH_MelonCutRegisterTableInfo table = new XH_MelonCutRegisterTableInfo();
                        table.RegisterDate = cash.StockRightRegisterDate.Value;
                        table.CutDate = cash.StockRightLogoutDatumDate.Value;
                        table.Code = cash.CommodityCode;
                        table.CutType = (int)Types.MeloncutType.Cash;

                        table.UserAccountDistributeLogo = holdTable.UserAccountDistributeLogo;
                        table.TradeCurrencyType = holdTable.CurrencyTypeId;
                        //目前这个没有什么用，因为数据库已经把该字段设置为外键不能为null，所以这里直接以持仓的交易货币类型来代替如果日后有用再修改
                        table.CurrencyTypeId = holdTable.CurrencyTypeId;
                        table.RegisterAmount = holdTable.AvailableAmount * ((decimal)cash.RatioOfCashDividend.Value);//这里直接存储计算后数据后面不再用到这个比例;

                        tables.Add(table);
                    }

                    //记录到登记表中
                    SaveRegisterTables(tables);

                    string format = "MelonCutService.RegisterCashList进行现金分红登记[商品代码={0},登记的持有者总数={1}]";
                    string desc = string.Format(format, cash.CommodityCode, holdTables.Count);

                    LogHelper.WriteDebug(desc);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("分红登记异常" + ex.Message, ex);
                return false;
            }

            return true;
        }

        ///// <summary>
        ///// 进行现金分红
        ///// </summary>
        ///// <param name="list">分红列表</param>
        //private static bool CutCashList(IList<CM_StockMelonCash> list)
        //{
        //    //选出今日需要分红的股票分红信息
        //    var q = from c in list
        //            where
        //                CheckDate(c.StockRightLogoutDatumDate) && c.StockRightRegisterDate.HasValue &&
        //                c.RatioOfCashDividend.HasValue
        //            select c;

        //    foreach (CM_StockMelonCash cash in q)
        //    {
        //        List<XH_MelonCutRegisterTableInfo> tables = GetMeloncutRegisterList(cash.CommodityCode,
        //                                                                            cash.StockRightRegisterDate.Value,
        //                                                                            cash.StockRightLogoutDatumDate.Value,
        //                                                                            Types.MeloncutType.Cash);
        //        ProcessCashCut(cash, tables);
        //    }

        //    return true;
        //}

        /// <summary>
        /// 对某一个股票进行现金分红
        /// </summary>
        /// <param name="cash">分红信息</param>
        /// <param name="tables">登记列表</param>
        //private static void ProcessCashCut(CM_StockMelonCash cash, List<XH_MelonCutRegisterTableInfo> tables)
        private static bool ProcessCashCut(IList<XH_MelonCutRegisterTableInfo> tables)
        {
            bool isSuss = true;
            if (Utils.IsNullOrEmpty(tables))
            {
                return isSuss;
            }

            //TransactionManager tm = TransactionFactory.GetTransactionManager();
            //tm.BeginTransaction();
            ReckoningTransaction reckoningTransaction = new ReckoningTransaction();
            Database database = DatabaseFactory.CreateDatabase();
            DbConnection connection = database.CreateConnection();
            List<XH_CapitalAccountTableInfo> list = new List<XH_CapitalAccountTableInfo>();
            try
            {
                connection.Open();
                reckoningTransaction.Database = database;
                DbTransaction transaction = connection.BeginTransaction();
                reckoningTransaction.Transaction = transaction;

                isSuss = UpdateCashCut(reckoningTransaction, tables);
                reckoningTransaction.Transaction.Commit();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("", ex);
                reckoningTransaction.Transaction.Rollback();
                isSuss = false;
            }
            finally
            {
                //这里为了安全起见，自行执行一次关闭数据连接
                if (reckoningTransaction != null && reckoningTransaction.Transaction != null)
                {
                    reckoningTransaction.Transaction.Dispose();
                }
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

            return isSuss;
        }

        /// <summary>
        /// 更新分红记录
        /// </summary>
        /// <param name="tm">TransactionManager</param>
        /// <param name="cash">分红信息</param>
        /// <param name="tables">登记列表</param>
        private static bool UpdateCashCut(ReckoningTransaction tm, IList<XH_MelonCutRegisterTableInfo> tables)
        {
            bool issuss = true;
            List<XH_CapitalAccountTableInfo> capitalList = new List<XH_CapitalAccountTableInfo>();
            List<UA_CapitalFlowTableInfo> flowList = new List<UA_CapitalFlowTableInfo>();

            XH_CapitalAccountTableDal dal = new XH_CapitalAccountTableDal();

            try
            {
                foreach (XH_MelonCutRegisterTableInfo registerTable in tables)
                {
                    //分红的现金额
                    decimal num = registerTable.RegisterAmount;

                    UA_UserAccountAllocationTableInfo capitalAccount = Utils.GetCapitalAccountByHoldAccount(registerTable.UserAccountDistributeLogo, registerTable.Code);

                    XH_CapitalAccountTableInfo accountTable = dal.GetXHCapitalAccount(capitalAccount.UserAccountDistributeLogo, registerTable.TradeCurrencyType);

                    if (accountTable == null)
                    {
                        continue;
                    }


                    accountTable.TodayOutInCapital += num;


                    accountTable.AvailableCapital += num;

                    capitalList.Add(accountTable);


                    //更新资金流水表
                    UA_CapitalFlowTableInfo flowTable = GetFlowTable(registerTable, capitalAccount.UserAccountDistributeLogo, num);
                    flowList.Add(flowTable);


                    string format = "MelonCutService.UpdateCashCut进行现金分红[商品代码={0},持仓帐号={1},分红金额={2}]";
                    string desc = string.Format(format, registerTable.Code, registerTable.UserAccountDistributeLogo, num);
                    LogHelper.WriteDebug(desc);
                }

                XH_CapitalAccountTableDal xh_AccountTableDal = new XH_CapitalAccountTableDal();

                if (capitalList.Count > 0)
                {
                    foreach (var accountTableInfo in capitalList)
                    {
                        xh_AccountTableDal.Update(accountTableInfo, tm.Database, tm.Transaction);
                    }
                }
                if (flowList.Count > 0)
                {
                    UA_CapitalFlowTableDal flowTableDal = new UA_CapitalFlowTableDal();
                    foreach (var historyTradeTableInfo in flowList)
                    {
                        flowTableDal.Add(historyTradeTableInfo, tm.Database, tm.Transaction);
                    }
                }
                if (tables.Count > 0)
                {
                    XH_MelonCutRegisterTableDal melonCutRegisterTableDal = new XH_MelonCutRegisterTableDal();
                    foreach (var registerTableInfo in tables)
                    {
                        melonCutRegisterTableDal.Delete(registerTableInfo, tm.Database, tm.Transaction);
                    }
                }
            }
            catch (Exception ex)
            {
                issuss = false;
                LogHelper.WriteError("分红出现问题 ", ex);
            }
            return issuss;
        }

        /// <summary>
        /// 创建资金流水表的记录
        /// </summary>
        /// <param name="registerTable">登记表实体</param>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="num">分红金额</param>
        /// <returns>资金流水表实体</returns>
        private static UA_CapitalFlowTableInfo GetFlowTable(XH_MelonCutRegisterTableInfo registerTable, string capitalAccount, decimal num)
        {
            UA_CapitalFlowTableInfo flowTable = new UA_CapitalFlowTableInfo();
            flowTable.TradeCurrencyType = registerTable.TradeCurrencyType;
            flowTable.FromCapitalAccount = string.Empty; //因为数据库不能为null所以这里以空的字符串填充
            //flowTable.ToCapitalAccount = registerTable.UserAccountDistributeLogo;
            flowTable.ToCapitalAccount = capitalAccount;
            flowTable.TransferAmount = num;
            flowTable.TransferTime = DateTime.Now;
            flowTable.TransferTypeLogo = (int)Types.TransferType.DividendTransfer;
            return flowTable;
        }

        #endregion

        #region 分红过户

        /// <summary>
        /// 处理股票分红（分红过户）
        /// </summary>
        private static bool ProcessCStockMelonStock()
        {
            XH_MelonCutRegisterTableDal dal = new XH_MelonCutRegisterTableDal();

            string where = string.Format(" CutType ='{0}'", (int)Types.MeloncutType.Stock);
            List<XH_MelonCutRegisterTableInfo> list = dal.GetListArray(where);

            //选出今日需要分红的股票分红信息
            var q = from c in list
                    where
                        CheckDate(c.CutDate) && c.RegisterAmount > 0 && !string.IsNullOrEmpty(c.Code)
                    select c;
            bool issum = true;
            if (q != null)
            {
                //2.如果今日有需要现金分红的就进行分红
                issum = ProcessStockCut(q.ToList<XH_MelonCutRegisterTableInfo>());
            }

            return issum;

        }

        /// <summary>
        /// 进行股票分红登记
        /// </summary>
        /// <param name="list">分红信息列表</param>
        private static bool RegisterStockList(IList<CM_StockMelonStock> list)
        {
            try
            {
                //选出今日需要登记的股票分红信息
                var q = from c in list
                        where
                            CheckRegDate(c.StockRightRegisterDate) && c.StockRightLogoutDatumDate.HasValue &&
                            c.SentStockRatio.HasValue
                        select c;

                //处理每一个股票的分红
                foreach (CM_StockMelonStock cash in q)
                {
                    //获取所有有此股票的持仓记录
                    List<XH_AccountHoldTableInfo> holdTables = GetAccountHoldListByCode(cash.CommodityCode);

                    if (Utils.IsNullOrEmpty(holdTables))
                        continue;

                    List<XH_MelonCutRegisterTableInfo> tables = new List<XH_MelonCutRegisterTableInfo>();

                    foreach (XH_AccountHoldTableInfo holdTable in holdTables)
                    {
                        XH_MelonCutRegisterTableInfo table = new XH_MelonCutRegisterTableInfo();
                        table.RegisterDate = cash.StockRightRegisterDate.Value;
                        table.CutDate = cash.StockRightLogoutDatumDate.Value;
                        table.Code = cash.CommodityCode;
                        //====update 李健华 2010-01-15 这里是股票分红
                        //table.CutType = (int) Types.MeloncutType.Cash;
                        table.CutType = (int)Types.MeloncutType.Stock;
                        //==========

                        table.UserAccountDistributeLogo = holdTable.UserAccountDistributeLogo;
                        table.TradeCurrencyType = holdTable.CurrencyTypeId;
                        //目前这个没有什么用，因为数据库已经把该字段设置为外键不能为null，所以这里直接以持仓的交易货币类型来代替如果日后有用再修改
                        table.CurrencyTypeId = holdTable.CurrencyTypeId;
                        table.RegisterAmount = holdTable.AvailableAmount * ((decimal)cash.SentStockRatio.Value);

                        tables.Add(table);
                    }

                    //记录到登记表中
                    SaveRegisterTables(tables);

                    string format = "MelonCutService.RegisterStockList进行股票分红登记[商品代码={0},登记的持有者总数={1}]";
                    string desc = string.Format(format, cash.CommodityCode, holdTables.Count);
                    LogHelper.WriteDebug(desc);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("股票分红登记异常" + ex.Message, ex);
                return false;
            }

            return true;
        }

        ///// <summary>
        ///// 进行股票分红
        ///// </summary>
        ///// <param name="list">分红列表</param>
        //private static void CutStockList(IList<CM_StockMelonStock> list)
        //{
        //    //选出今日需要分红的股票分红信息
        //    var q = from c in list
        //            where
        //                CheckDate(c.StockRightLogoutDatumDate) && c.StockRightRegisterDate.HasValue &&
        //                c.SentStockRatio.HasValue
        //            select c;

        //    foreach (CM_StockMelonStock cash in q)
        //    {
        //        //update 李健华 2010-1-15 这里是股票分红 不是现金分红Types.MeloncutType.Cash);
        //        List<XH_MelonCutRegisterTableInfo> tables = GetMeloncutRegisterList(cash.CommodityCode,
        //                                                                            cash.StockRightRegisterDate.Value,
        //                                                                            cash.StockRightLogoutDatumDate.Value,
        //                                                                            Types.MeloncutType.Stock);
        //        //==============================

        //        ProcessStockCut(cash, tables);
        //    }
        //}

        /// <summary>
        /// 对某一个股票进行分红(过户)
        /// </summary>
        /// <param name="cash">分红信息</param>
        /// <param name="tables">登记列表</param>
        private static bool ProcessStockCut(List<XH_MelonCutRegisterTableInfo> tables)
        {

            bool isSuss = true;
            if (Utils.IsNullOrEmpty(tables))
            {
                return isSuss;
            }

            //TransactionManager tm = TransactionFactory.GetTransactionManager();
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
                isSuss = UpdateStockCut(reckoningTransaction, tables);
                reckoningTransaction.Transaction.Commit();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("", ex);
                reckoningTransaction.Transaction.Rollback();
                isSuss = false;

            }
            return isSuss;
        }

        /// <summary>
        /// 更新过户记录
        /// </summary>
        /// <param name="tm">TransactionManager</param>
        /// <param name="cash">分红信息</param>
        /// <param name="tables">登记列表</param>
        private static bool UpdateStockCut(ReckoningTransaction tm, List<XH_MelonCutRegisterTableInfo> tables)
        {
            #region old code 李健华 2010-06-11 改为因为在每天开市时处理那么生成为当日委托我成交
            //List<XH_AccountHoldTableInfo> accountHoldList = new List<XH_AccountHoldTableInfo>();
            //List<XH_HistoryTradeTableInfo> historyTradeList = new List<XH_HistoryTradeTableInfo>();

            ////add 2010-1-15 因为数据库表有的成交记录是根据委托编号有外键关系，所以这里要生成委托记录
            //List<XH_HistoryEntrustTableInfo> historyEntrustList = new List<XH_HistoryEntrustTableInfo>();
            ////========

            //foreach (XH_MelonCutRegisterTableInfo registerTable in tables)
            //{
            //    //更新持仓表
            //    List<XH_AccountHoldTableInfo> accountHoldTables = GetAccountHoldList(
            //        registerTable.UserAccountDistributeLogo,
            //        registerTable.TradeCurrencyType, registerTable.Code);
            //    if (Utils.IsNullOrEmpty(accountHoldTables))
            //        continue;

            //    //过户的量
            //    decimal num = registerTable.RegisterAmount;
            //    //四舍五入
            //    num = Math.Round(num);
            //    int intNum = decimal.ToInt32(num);

            //    XH_AccountHoldTableInfo accountTable = accountHoldTables[0];
            //    SetAccountTable(accountTable, intNum);

            //    accountHoldList.Add(accountTable);

            //    //更新委托记录表
            //    XH_HistoryEntrustTableInfo historyEntrustTable = GetHistoryEntrustTable(registerTable, intNum);
            //    historyEntrustList.Add(historyEntrustTable);
            //    //更新历史成交表
            //    XH_HistoryTradeTableInfo historyTradeTable = GetHistoryTradeTable(registerTable, intNum);
            //    historyTradeTable.EntrustNumber = historyEntrustTable.EntrustNumber;
            //    historyTradeTable.CapitalAccount = historyEntrustTable.CapitalAccount;
            //    historyTradeList.Add(historyTradeTable);

            //    string format = "MelonCutService.UpdateStockCut进行股票分红[商品代码={0},持仓帐号={1},分红金额={2}]";
            //    string desc = string.Format(format, registerTable.Code, registerTable.UserAccountDistributeLogo, num);
            //    LogHelper.WriteDebug(desc);
            //}


            //XH_AccountHoldTableDal xh_AccountHoldTableDal = new XH_AccountHoldTableDal();
            //if (accountHoldList.Count > 0)
            //{
            //    foreach (var holdTableInfo in accountHoldList)
            //    {
            //        xh_AccountHoldTableDal.Update(holdTableInfo, tm.Database, tm.Transaction);
            //    }
            //}

            ////先添加委托记录
            //XH_HistoryEntrustTableDal xh_HistoryEntrustTableDal = new XH_HistoryEntrustTableDal();
            //if (historyEntrustList.Count > 0)
            //{
            //    foreach (XH_HistoryEntrustTableInfo item in historyEntrustList)
            //    {

            //        xh_HistoryEntrustTableDal.Add(item, tm.Database, tm.Transaction);
            //    }
            //}
            ////添加成交记录
            ////DataRepository.XhAccountHoldTableProvider.Update(tm, accountHoldList);

            ////DataRepository.XhHistoryTradeTableProvider.BulkInsert(historyTradeList);
            //XH_HistoryTradeTableDal xh_HistoryTradeTableDal = new XH_HistoryTradeTableDal();
            //if (historyTradeList.Count > 0)
            //{
            //    foreach (XH_HistoryTradeTableInfo historyTradeTable in historyTradeList)
            //    {
            //        //DataRepository.XhHistoryTradeTableProvider.Insert(tm, historyTradeTable);
            //        xh_HistoryTradeTableDal.Add(historyTradeTable, tm.Database, tm.Transaction);
            //    }
            //}


            ////删除对应的登记记录
            ////DataRepository.XhMelonCutRegisterTableProvider.Delete(tm, tables);
            //XH_MelonCutRegisterTableDal xh_MelonCutRegisterTableDal = new XH_MelonCutRegisterTableDal();
            //if (tables.Count > 0)
            //{
            //    foreach (var data in tables)
            //    {
            //        xh_MelonCutRegisterTableDal.Delete(data, tm.Database, tm.Transaction);
            //    }
            //}
            #endregion

            #region new code 李健华 2010-06-11 改为因为在每天开市时处理那么生成为当日委托我成交
            bool issuc = true;
            try
            {
                List<XH_AccountHoldTableInfo> accountHoldList = new List<XH_AccountHoldTableInfo>();
                List<XH_TodayTradeTableInfo> todayTradeList = new List<XH_TodayTradeTableInfo>();

                //add 2010-1-15 因为数据库表有的成交记录是根据委托编号有外键关系，所以这里要生成委托记录
                List<XH_TodayEntrustTableInfo> todayEntrustList = new List<XH_TodayEntrustTableInfo>();
                UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();

                //========

                foreach (XH_MelonCutRegisterTableInfo registerTable in tables)
                {
                    //更新持仓表
                    List<XH_AccountHoldTableInfo> accountHoldTables = GetAccountHoldList(registerTable.UserAccountDistributeLogo, registerTable.TradeCurrencyType, registerTable.Code);
                    if (Utils.IsNullOrEmpty(accountHoldTables))
                    {
                        continue;
                    }

                    //过户的量
                    decimal num = registerTable.RegisterAmount;
                    //四舍五入
                    num = Math.Round(num);
                    int intNum = decimal.ToInt32(num);

                    XH_AccountHoldTableInfo accountTable = accountHoldTables[0];
                    SetAccountTable(accountTable, intNum);

                    accountHoldList.Add(accountTable);

                    //更新委托记录表
                    XH_TodayEntrustTableInfo todayEntrustTable = GetHistoryEntrustTable(registerTable, intNum);
                    todayEntrustList.Add(todayEntrustTable);
                    //更新历史成交表
                    XH_TodayTradeTableInfo todayTradeTable = GetTodayTradeTable(registerTable, intNum);
                    todayTradeTable.EntrustNumber = todayEntrustTable.EntrustNumber;
                    todayTradeTable.CapitalAccount = todayEntrustTable.CapitalAccount;
                    todayTradeList.Add(todayTradeTable);
                    UA_UserAccountAllocationTableInfo userModel = dal.GetModel(registerTable.UserAccountDistributeLogo);
                    #region 回推相关成交记录信息
                    ReckonEndObject<XH_TodayEntrustTableInfo, XH_TodayTradeTableInfo> reckonEndObject = new ReckonEndObject<XH_TodayEntrustTableInfo, XH_TodayTradeTableInfo>();
                    reckonEndObject.IsSuccess = true;
                    reckonEndObject.EntrustTable = todayEntrustTable;
                    List<XH_TodayTradeTableInfo> tradeModels = new List<XH_TodayTradeTableInfo>();
                    tradeModels.Add(todayTradeTable);
                    reckonEndObject.TradeTableList = tradeModels;
                    if (userModel != null)
                    {
                        reckonEndObject.TradeID = userModel.UserID;
                    }
                    reckonEndObject.Message = "分红委托成交";

                    CounterOrderService.Instance.AcceptStockDealOrder(reckonEndObject);
                    #endregion

                    string format = "MelonCutService.UpdateStockCut进行股票分红[商品代码={0},持仓帐号={1},分红金额={2}]";
                    string desc = string.Format(format, registerTable.Code, registerTable.UserAccountDistributeLogo, num);
                    LogHelper.WriteDebug(desc);
                }


                XH_AccountHoldTableDal xh_AccountHoldTableDal = new XH_AccountHoldTableDal();
                if (accountHoldList.Count > 0)
                {
                    foreach (var holdTableInfo in accountHoldList)
                    {
                        xh_AccountHoldTableDal.Update(holdTableInfo, tm.Database, tm.Transaction);
                    }
                }

                //先添加委托记录
                XH_TodayEntrustTableDal xh_HistoryEntrustTableDal = new XH_TodayEntrustTableDal();
                if (todayEntrustList.Count > 0)
                {
                    foreach (XH_TodayEntrustTableInfo item in todayEntrustList)
                    {

                        xh_HistoryEntrustTableDal.Add(item, tm.Database, tm.Transaction);
                    }
                }
                //添加成交记录
                //DataRepository.XhAccountHoldTableProvider.Update(tm, accountHoldList);

                //DataRepository.XhHistoryTradeTableProvider.BulkInsert(historyTradeList);
                XH_TodayTradeTableDal xh_todayTradeTableDal = new XH_TodayTradeTableDal();
                if (todayTradeList.Count > 0)
                {
                    foreach (XH_TodayTradeTableInfo todayTradeTable in todayTradeList)
                    {
                        //DataRepository.XhHistoryTradeTableProvider.Insert(tm, historyTradeTable);
                        xh_todayTradeTableDal.Add(todayTradeTable, tm);
                    }
                }


                //删除对应的登记记录
                //DataRepository.XhMelonCutRegisterTableProvider.Delete(tm, tables);
                XH_MelonCutRegisterTableDal xh_MelonCutRegisterTableDal = new XH_MelonCutRegisterTableDal();
                if (tables.Count > 0)
                {
                    foreach (var data in tables)
                    {
                        xh_MelonCutRegisterTableDal.Delete(data, tm.Database, tm.Transaction);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("过户分红异常" + ex.Message, ex);
                issuc = false;
            }
            return issuc;
            #endregion
        }

        /// <summary>
        /// 更新资金表记录
        /// </summary>
        /// <param name="cash">分红信息</param>
        /// <param name="accountTable">要更新的资金表实体</param>
        /// <param name="intNum">分红金额</param>
        private static void SetAccountTable(XH_AccountHoldTableInfo accountTable, int intNum)
        {
            //if (accountTable.AvailableAmount < 0.00m)
            //    accountTable.AvailableAmount = 0;
            //原有的量
            decimal availableAmount = accountTable.AvailableAmount;

            //if (accountTable.CostPrice < 0.00m)
            //    accountTable.CostPrice = 0;
            //原来的成本价
            decimal costPrice = accountTable.CostPrice;
            //持仓均价=（持仓均价*持仓量+买入成交量*买入价）/(持仓量+买入成交量)
            decimal holdPrice = accountTable.HoldAveragePrice;
            accountTable.HoldAveragePrice = (decimal)(holdPrice * availableAmount + intNum * 0) / (availableAmount + intNum);

            decimal newCostprice = availableAmount * costPrice / (availableAmount + intNum);

            //新的量
            accountTable.AvailableAmount = availableAmount + intNum;
            //新的成本价
            accountTable.CostPrice = newCostprice;

            decimal newBreakevenPrice = MCService.GetHoldPrice(accountTable.Code, accountTable.CostPrice, accountTable.AvailableAmount);
            //新的保本价
            accountTable.BreakevenPrice = newBreakevenPrice;
        }

        /// <summary>
        /// 创建一个今日成交记录
        /// </summary>
        /// <param name="registerTable">登记表实体</param>
        /// <param name="intNum">分红过户量</param>里
        /// <returns>今日成交实体</returns>
        private static XH_TodayTradeTableInfo GetTodayTradeTable(XH_MelonCutRegisterTableInfo registerTable, int intNum)
        {
            XH_TodayTradeTableInfo todayTradeTable = new XH_TodayTradeTableInfo();

            todayTradeTable.TradeNumber = Guid.NewGuid().ToString();
            todayTradeTable.BuySellTypeId = (int)GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying;
            todayTradeTable.StockAccount = registerTable.UserAccountDistributeLogo;
            todayTradeTable.TradeTypeId = (int)Types.DealRptType.DRTTransfer;
            todayTradeTable.CurrencyTypeId = registerTable.TradeCurrencyType;
            todayTradeTable.TradeUnitId = MCService.GetPriceUnit(registerTable.Code);
            todayTradeTable.TradeAmount = intNum;
            todayTradeTable.TradeTime = DateTime.Now;

            CM_BourseType bourseType = MCService.CommonPara.GetBourseTypeByCommodityCode(registerTable.Code);
            if (bourseType != null)
            {
            }

            CM_Commodity commodity = MCService.CommonPara.GetCommodityByCommodityCode(registerTable.Code);
            todayTradeTable.SpotCode = registerTable.Code;
            if (commodity != null)
            {
            }

            CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByCommodityCode(registerTable.Code);
            if (breedClass != null)
            {
            }

            return todayTradeTable;
        }

        /// <summary>
        /// 创建一个今日委托记录
        /// </summary>
        /// <param name="registerTable">登记表实体</param>
        /// <param name="intNum">分红过户量</param>里
        /// <returns>今日委托实体</returns>
        private static XH_TodayEntrustTableInfo GetHistoryEntrustTable(XH_MelonCutRegisterTableInfo registerTable, int intNum)
        {
            XH_TodayEntrustTableInfo todayEntrustTable = new XH_TodayEntrustTableInfo();

            todayEntrustTable.EntrustNumber = XHCommonLogic.BuildXHOrderNo();
            todayEntrustTable.CurrencyTypeId = registerTable.TradeCurrencyType;
            todayEntrustTable.TradeUnitId = MCService.GetPriceUnit(registerTable.Code);
            todayEntrustTable.EntrustAmount = intNum;
            todayEntrustTable.EntrustPrice = 0;
            todayEntrustTable.EntrustTime = DateTime.Now;
            todayEntrustTable.OfferTime = DateTime.Now;

            todayEntrustTable.BuySellTypeId = (int)GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying;
            todayEntrustTable.OrderStatusId = (int)Types.OrderStateType.DOSDealed;
            todayEntrustTable.StockAccount = registerTable.UserAccountDistributeLogo;//持仓账号，在分红记录中都是持仓账号
            //====通过持仓账号查询相关联的资金账号
            UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
            UA_UserAccountAllocationTableInfo uaUser = dal.GetUserHoldAccountByUserCapitalAccount(registerTable.UserAccountDistributeLogo);
            todayEntrustTable.CapitalAccount = uaUser.UserAccountDistributeLogo;
            //=====        
            todayEntrustTable.PortfolioLogo = "";
            todayEntrustTable.SpotCode = registerTable.Code;
            todayEntrustTable.TradeAmount = intNum;
            todayEntrustTable.TradeAveragePrice = 0;
            todayEntrustTable.CancelAmount = 0;
            todayEntrustTable.IsMarketValue = false;
            todayEntrustTable.OrderMessage = "股票分红生成委托记录";
            todayEntrustTable.McOrderId = Guid.NewGuid().ToString();

            return todayEntrustTable;
        }

        #endregion
    }
}