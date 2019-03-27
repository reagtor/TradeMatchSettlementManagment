#region Using Namespace

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using GTA.VTS.Common.CommonUtility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.Entity.Contants;
using ReckoningCounter.Model;

#endregion

namespace ReckoningCounter.DAL.CustomDataAccess
{
    /// <summary>
    /// 现货数据存取帮助类
    /// 作者：宋涛
    /// </summary>
    public class XHDataAccess
    {
        /// <summary>
        /// 删除资金冻结记录
        /// </summary>
        /// <param name="persistHoldFreezeId">资金冻结记录id</param>
        /// <returns>是否成功</returns>
        public static bool DeleteHoldFreeze(int persistHoldFreezeId)
        {
            if (persistHoldFreezeId != -1)
            {
                try
                {
                    XH_AcccountHoldFreezeTableDal hDal = new XH_AcccountHoldFreezeTableDal();
                    hDal.Delete(persistHoldFreezeId);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 删除资金冻结记录
        /// </summary>
        /// <param name="persistHoldFreezeId">资金冻结记录id</param>
        /// <param name="db">数据库对象</param>
        /// <param name="transaction">事务</param>
        /// <returns>是否成功</returns>
        public static bool DeleteHoldFreeze(int persistHoldFreezeId, Database db, DbTransaction transaction)
        {
            if (persistHoldFreezeId != -1)
            {
                try
                {
                    XH_AcccountHoldFreezeTableDal hDal = new XH_AcccountHoldFreezeTableDal();
                    hDal.Delete(persistHoldFreezeId,db,transaction);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 清除持仓冻结
        /// </summary>
        /// <param name="persistHoldFreezeId"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static bool ClearHoldFreeze(int persistHoldFreezeId, Database db, DbTransaction transaction)
        {
            if (persistHoldFreezeId != -1)
            {
                try
                {
                    XH_AcccountHoldFreezeTableDal hDal = new XH_AcccountHoldFreezeTableDal();
                    hDal.Clear(persistHoldFreezeId, db, transaction);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 删除持仓冻结记录
        /// </summary>
        /// <param name="persistCapitalFreezeId">持仓冻结记录id</param>
        /// <returns>是否成功 </returns>
        public static bool DeleteCapitalFreeze(int persistCapitalFreezeId)
        {
            if (persistCapitalFreezeId != -1)
            {
                try
                {
                    XH_CapitalAccountFreezeTableDal caDal = new XH_CapitalAccountFreezeTableDal();
                    caDal.Delete(persistCapitalFreezeId);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                    return false;
                }
            }

            return true;
        }
       
        /// <summary>
        /// 删除持仓冻结记录
        /// </summary>
        /// <param name="persistCapitalFreezeId">冻结记录id</param>
        /// <param name="db">数据库对象</param>
        /// <param name="transaction">事务对象</param>
        /// <returns></returns>
        public static bool DeleteCapitalFreeze(int persistCapitalFreezeId, Database db, DbTransaction transaction)
        {
            if (persistCapitalFreezeId != -1)
            {
                try
                {
                    XH_CapitalAccountFreezeTableDal caDal = new XH_CapitalAccountFreezeTableDal();

                    ReckoningTransaction tm = new ReckoningTransaction();
                    tm.Database = db;
                    tm.Transaction = transaction;
                    caDal.Delete(persistCapitalFreezeId, tm);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 清除资金冻结
        /// </summary>
        /// <param name="persistCapitalFreezeId"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static bool ClearCapitalFreeze(int persistCapitalFreezeId, Database db, DbTransaction transaction)
        {
            if (persistCapitalFreezeId != -1)
            {
                try
                {
                    XH_CapitalAccountFreezeTableDal caDal = new XH_CapitalAccountFreezeTableDal();

                    caDal.Clear(persistCapitalFreezeId, db,transaction);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 删除当日委托
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        public static bool DeleteTodayEntrust(string entrustNumber)
        {
            try
            {
                XH_TodayEntrustTableDal dal = new XH_TodayEntrustTableDal();
                dal.Delete(entrustNumber);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 根据委托单号获取今日委托
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        /// <returns>今日委托</returns>
        public static XH_TodayEntrustTableInfo GetTodayEntrustTable(string entrustNumber)
        {
            XH_TodayEntrustTableInfo tet = null;

            XH_TodayEntrustTableDal dal = new XH_TodayEntrustTableDal();
            try
            {
                tet = dal.GetModel(entrustNumber);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return tet;
        }

        /// <summary>
        /// 根据委托单号获取历史委托
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        /// <returns>今日委托</returns>
        public static XH_HistoryEntrustTableInfo GetHistoryEntrustTable(string entrustNumber)
        {
            XH_HistoryEntrustTableInfo tet = null;

            XH_HistoryEntrustTableDal dal = new XH_HistoryEntrustTableDal();
            try
            {
                tet = dal.GetModel(entrustNumber);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return tet;
        }

        /// <summary>
        /// 根据委托单号获取委托，先在当日中查找，找不到再到历史中查找，转换成当日委托实体
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        /// <returns>今日委托</returns>
        public static XH_TodayEntrustTableInfo GetAllEntrustTable(string entrustNumber)
        {
            XH_TodayEntrustTableInfo tet = null;

            tet = GetTodayEntrustTable(entrustNumber);

            if(tet == null)
            {
                var tet2 = GetHistoryEntrustTable(entrustNumber);
                if(tet2 != null)
                {
                    tet = ConvertHistoryEntrustTable(tet2);
                }
            }

            return tet;
        }

        private static XH_TodayEntrustTableInfo ConvertHistoryEntrustTable(XH_HistoryEntrustTableInfo tet)
        {
            XH_TodayEntrustTableInfo tet2 = new XH_TodayEntrustTableInfo();
            tet2.BuySellTypeId = tet.BuySellTypeId;
            //tet2.CallbackChannlId = tet.
            tet2.CancelAmount = tet.CancelAmount;
            //tet2.CancelLogo = tet.ca
            tet2.CapitalAccount = tet.CapitalAccount;
            tet2.CurrencyTypeId = tet.CurrencyTypeId;
            tet2.EntrustAmount = tet.EntrustMount;
            tet2.EntrustNumber = tet.EntrustNumber;
            tet2.EntrustPrice = tet.EntrustPrice;
            tet2.EntrustTime = tet.EntrustTime;
            tet2.HasDoneProfit = tet.HasDoneProfit;
            tet2.IsMarketValue = tet.IsMarketValue;
            tet2.McOrderId = tet.McOrderId;
            tet2.OfferTime = tet.OfferTime;
            tet2.OrderMessage = tet.OrderMessage;
            tet2.OrderStatusId = tet.OrderStatusId;
            tet2.PortfolioLogo = tet.PortfolioLogo;
            tet2.SpotCode = tet.SpotCode;
            tet2.StockAccount = tet.StockAccount;
            tet2.TradeAmount = tet.TradeAmount;
            tet2.TradeAveragePrice = tet.TradeAveragePrice;
            tet2.TradeUnitId = tet.TradeUnitId;

            return tet2;
        }

        /// <summary>
        /// 更新委托表
        /// </summary>
        /// <param name="tet">委托表</param>
        /// <returns>是否成功</returns>
        public static bool UpdateEntrustTable(XH_TodayEntrustTableInfo tet)
        {
            try
            {
                XH_TodayEntrustTableDal dal = new XH_TodayEntrustTableDal();
                dal.Update(tet);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 更新委托表（带事务）
        /// </summary>
        /// <param name="tet">委托表</param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns>是否成功</returns>
        public static bool UpdateEntrustTable(XH_TodayEntrustTableInfo tet, Database db, DbTransaction transaction)
        {
            try
            {
                XH_TodayEntrustTableDal dal = new XH_TodayEntrustTableDal();
                dal.Update(tet,db,transaction);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取现货对应委托单的资金冻结对象
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        /// <param name="ft">冻结类型</param>
        /// <returns>委托单的资金冻结实体</returns>
        public static XH_CapitalAccountFreezeTableInfo GetCapitalAccountFreeze(string entrustNumber, Types.FreezeType ft)
        {
            XH_CapitalAccountFreezeTableDal dal = new XH_CapitalAccountFreezeTableDal();
            string format = "EntrustNumber='{0}' AND FreezeTypeLogo={1}";
            string where = string.Format(format, entrustNumber, (int) ft);
            IList<XH_CapitalAccountFreezeTableInfo> list = null;

            try
            {
                list = dal.GetListArray(where);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            if (list == null)
                return null;

            if (list.Count == 0)
                return null;

            return list[0];
        }


        /// <summary>
        /// 获取现货对应委托单的持仓冻结对象
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        /// <param name="ft">冻结类型</param>
        /// <returns>委托单的持仓冻结实体</returns>
        public static XH_AcccountHoldFreezeTableInfo GetHoldAccountFreeze(string entrustNumber, Types.FreezeType ft)
        {
            XH_AcccountHoldFreezeTableDal dal = new XH_AcccountHoldFreezeTableDal();
            string format = "EntrustNumber='{0}' AND FreezeTypeLogo={1}";
            string where = string.Format(format, entrustNumber, (int) ft);

            IList<XH_AcccountHoldFreezeTableInfo> list = null;

            try
            {
                list = dal.GetListArray(where);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            if (list == null)
                return null;

            if (list.Count == 0)
                return null;

            return list[0];
        }

        /// <summary>
        /// 更新委托信息
        /// </summary>
        /// <param name="entrustNubmer">委托单号</param>
        /// <param name="orderMessage">信息</param>
        /// <returns></returns>
        public static bool UpdateEntrustOrderMessage(string entrustNubmer,string orderMessage)
        {
            try
            {
                XH_TodayEntrustTableDal dal = new XH_TodayEntrustTableDal();
                dal.UpdateOrderMessage(entrustNubmer, orderMessage);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 根据委托单号获取对应的成交列表
        /// </summary>
        /// <param name="entrustNumber"></param>
        /// <returns></returns>
        public static List<XH_TodayTradeTableInfo> GetTodayTradeListByEntrustNumber(string entrustNumber)
        {
            XH_TodayTradeTableDal dal = new XH_TodayTradeTableDal();
            string format = "EntrustNumber='{0}'";
            string where = string.Format(format, entrustNumber);

            return dal.GetListArray(where);
        }

        /// <summary>
        ///获取数据库访问命令对象
        /// </summary>
        /// <param name="database"></param>
        /// <param name="strSql"></param>
        /// <returns></returns>
        private static DbCommand GetDBCommand(Database database, string strSql)
        {
            DbCommand command =
                database.GetSqlStringCommand(strSql);
            //command.CommandTimeout = DataRepository.Provider.DefaultCommandTimeout;
            return command;
        }

        /// <summary>
        /// 执行带事务sql语句
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="tm">事务对象</param>
        /// <returns></returns>
        public static bool XHCapitalAccountProcess(string strSql,
                                                   ReckoningTransaction tm)
        {
            bool result = false;

            try
            {
                if (tm != null)
                {
                    DbHelperSQL.ExecuteSql(strSql,tm);
                }
                else
                {
                    DbHelperSQL.ExecuteSql(strSql);
                }

                result = true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return result;
        }


        /// <summary>
        /// 更新现货持仓
        /// </summary>
        /// <param name="cAvaliable"></param>
        /// <param name="cFreeeze"></param>
        /// <param name="transactionManager"></param>
        /// <param name="holdAccountId"></param>
        /// <returns></returns>
        public static bool UpdateXHHoldAccount(decimal cAvaliable, decimal cFreeeze,
                                               ReckoningTransaction transactionManager, int holdAccountId)
        {
            int results = 0;

            string strSql = string.Empty;

            //var database = new SqlDatabase(TransactionFactory.RC_ConnectionString);
            var database = DataManager.GetDatabase();
            strSql = "UPDATE [XH_AccountHoldTable] SET [AvailableAmount] = [AvailableAmount] + @AvailableHold"
                     + ",[FreezeAmount] = [FreezeAmount] + @FreezeHold"
                     + "WHERE [AccountHoldLogoId] = @holdAccountId";


            DbCommand commandWrapper = GetDBCommand(database, strSql);

            database.AddInParameter(commandWrapper, "@AvailableHold", DbType.Decimal, cAvaliable);
            database.AddInParameter(commandWrapper, "@holdAccountId", DbType.Int32, holdAccountId);
            database.AddInParameter(commandWrapper, "@FreezeHold", DbType.Decimal, cFreeeze);

            if (transactionManager != null)
            {
                results = DbHelperSQL.ExecuteCountSql(strSql, transactionManager);
            }
            else
            {
                results = DbHelperSQL.ExecuteCountSql(strSql);
            }

            return Convert.ToBoolean(results);
        }

        /// <summary>
        /// 更新现货持仓
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="others"></param>
        /// <param name="dealAmount"></param>
        /// <param name="strHoldAccount"></param>
        /// <param name="strCode"></param>
        /// <param name="iCurrType"></param>
        /// <param name="transactionManager"></param>
        /// <returns></returns>
        public static bool UpdateXHHoldAccount_Sell(decimal scale,
                                                    decimal others,
                                                    decimal dealAmount,
                                                    string strHoldAccount,
                                                    string strCode,
                                                    int iCurrType, ReckoningTransaction transactionManager)
        {
            int results = 0;

            var database = DataManager.GetDatabase();

            //卖出不需要计算保本价
            string strSql = "UPDATE [XH_AccountHoldTable]"
                            + " SET [FreezeAmount] = [FreezeAmount] + @dealAmount"
                            //+ ",[BreakevenPrice] = ([CostPrice] + @others)/(1 - @scale)"
                            +
                            "WHERE UserAccountDistributeLogo = @strHoldAccount AND Code = @strCode AND CurrencyTypeId=@iCurrType";

            DbCommand commandWrapper = GetDBCommand(database, strSql);

            //database.AddInParameter(commandWrapper, "@scale", DbType.Decimal, scale);
            //database.AddInParameter(commandWrapper, "@others", DbType.Decimal, others);
            database.AddInParameter(commandWrapper, "@dealAmount", DbType.Decimal, dealAmount);
            database.AddInParameter(commandWrapper, "@strHoldAccount", DbType.String, strHoldAccount);
            database.AddInParameter(commandWrapper, "@strCode", DbType.String, strCode);
            database.AddInParameter(commandWrapper, "@iCurrType", DbType.Int32, iCurrType);
            //database.AddOutParameter(commandWrapper, "@iHoldAccountId", DbType.Int32, 4);
            if (transactionManager != null)
            {
                results = DbHelperSQL.ExecuteCountSql(strSql, transactionManager);
            }
            else
            {
                results = DbHelperSQL.ExecuteCountSql(strSql);
            }

            return Convert.ToBoolean(results);
        }
    }
}