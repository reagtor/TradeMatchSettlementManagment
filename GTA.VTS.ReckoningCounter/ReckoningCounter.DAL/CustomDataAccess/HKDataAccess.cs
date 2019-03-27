#region Using Namespace

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using GTA.VTS.Common.CommonUtility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.DAL.Data.HK;
using ReckoningCounter.Entity.Contants;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.Entity;

#endregion

namespace ReckoningCounter.DAL.CustomDataAccess
{
    /// <summary>
    /// 港股数据存取帮助类
    /// 作者：宋涛
    /// </summary>
    public class HKDataAccess
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
                    HK_AcccountHoldFreezeDal hDal = new HK_AcccountHoldFreezeDal();
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
       /// <param name="transaction">事务对象</param>
        /// <returns>是否成功</returns>
        public static bool DeleteHoldFreeze(int persistHoldFreezeId, Database db, DbTransaction transaction)
        {
            if (persistHoldFreezeId != -1)
            {
                try
                {
                    HK_AcccountHoldFreezeDal hDal = new HK_AcccountHoldFreezeDal();
                    hDal.DeleteRecord(persistHoldFreezeId, db, transaction);
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
                    HK_AcccountHoldFreezeDal hDal = new HK_AcccountHoldFreezeDal();
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
                    HK_CapitalAccountFreezeDal caDal = new HK_CapitalAccountFreezeDal();
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
        /// <param name="persistCapitalFreezeId">持仓冻结记录id</param>
        /// <param name="db">数据库对象</param>
        /// <param name="transaction">事务对象</param>
        /// <returns>是否成功</returns>
        public static bool DeleteCapitalFreeze(int persistCapitalFreezeId, Database db, DbTransaction transaction)
        {
            if (persistCapitalFreezeId != -1)
            {
                try
                {
                    HK_CapitalAccountFreezeDal caDal = new HK_CapitalAccountFreezeDal();

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
                    HK_CapitalAccountFreezeDal caDal = new HK_CapitalAccountFreezeDal();

                    caDal.Clear(persistCapitalFreezeId, db, transaction);
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
                HK_TodayEntrustDal dal = new HK_TodayEntrustDal();
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
        public static HK_TodayEntrustInfo GetTodayEntrustTable(string entrustNumber)
        {
            HK_TodayEntrustInfo tet = null;

            HK_TodayEntrustDal dal = new HK_TodayEntrustDal();
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
        public static HK_HistoryEntrustInfo GetHistoryEntrustTable(string entrustNumber)
        {
            HK_HistoryEntrustInfo tet = null;

            HK_HistoryEntrustDal dal = new HK_HistoryEntrustDal();
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
        public static HK_TodayEntrustInfo GetAllEntrustTable(string entrustNumber)
        {
            HK_TodayEntrustInfo tet = null;

            tet = GetTodayEntrustTable(entrustNumber);

            if (tet == null)
            {
                var tet2 = GetHistoryEntrustTable(entrustNumber);
                if (tet2 != null)
                {
                    tet = ConvertHistoryEntrustTable(tet2);
                }
            }

            return tet;
        }

        /// <summary>
        /// 根据撮合ID在今日委托中查询对应的委托编号再在改单记录中查询回推通道
        /// </summary>
        /// <param name="macID">撮合ID</param>
        /// <param name="entrustNumber">输出原委托编号</param>
        /// <returns>返回改单回推通道，原entrustNumber</returns>
        public static string GetModifyOrderChannelIDByMacID(string macID, out string entrustNumber)
        {
            entrustNumber = "";
            List<HK_TodayEntrustInfo> tet = null;
            HK_TodayEntrustDal dal = new HK_TodayEntrustDal();
            HK_ModifyOrderRequestDal mdal = new HK_ModifyOrderRequestDal();
            try
            {
                tet = dal.GetListArray(" McOrderID='" + macID + "'");
                if (tet != null && tet.Count > 0)
                {
                    string str = tet[0].EntrustNumber.Trim();
                    entrustNumber = str;
                    var list = mdal.GetListArray(" EntrustNubmer='" + str + "'");
                    if (list != null && tet.Count > 0)
                    {
                        return list[0].ChannelID;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);

            }
            return "";
        }

        private static HK_TodayEntrustInfo ConvertHistoryEntrustTable(HK_HistoryEntrustInfo tet)
        {
            HK_TodayEntrustInfo tet2 = new HK_TodayEntrustInfo();
            tet2.BuySellTypeID = tet.BuySellTypeID;
            //tet2.CallbackChannlID = tet.
            tet2.CancelAmount = tet.CancelAmount;
            //tet2.CancelLogo = tet.ca
            tet2.CapitalAccount = tet.CapitalAccount;
            tet2.CurrencyTypeID = tet.CurrencyTypeID;
            tet2.EntrustAmount = tet.EntrustMount;
            tet2.EntrustNumber = tet.EntrustNumber;
            tet2.EntrustPrice = tet.EntrustPrice;
            tet2.EntrustTime = tet.EntrustTime;
            tet2.HasDoneProfit = tet.HasDoneProfit;
            //tet2.IsMarketValue = tet.IsMarketValue;
            tet2.OrderPriceType = tet.OrderPriceType;
            tet2.McOrderID = tet.McOrderID;
            tet2.OfferTime = tet.OfferTime;
            tet2.OrderMessage = tet.OrderMessage;
            tet2.OrderStatusID = tet.OrderStatusID;
            tet2.PortfolioLogo = tet.PortfolioLogo;
            tet2.Code = tet.Code;
            tet2.HoldAccount = tet.HoldAccount;
            tet2.TradeAmount = tet.TradeAmount;
            tet2.TradeAveragePrice = tet.TradeAveragePrice;
            tet2.TradeUnitID = tet.TradeUnitID;

            tet2.IsModifyOrder = tet.IsModifyOrder;
            tet2.ModifyOrderNumber = tet.ModifyOrderNumber;

            return tet2;
        }

        /// <summary>
        /// 更新委托表
        /// </summary>
        /// <param name="tet">委托表</param>
        /// <returns>是否成功</returns>
        public static bool UpdateEntrustTable(HK_TodayEntrustInfo tet)
        {
            try
            {
                HK_TodayEntrustDal dal = new HK_TodayEntrustDal();
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
        public static bool UpdateEntrustTable(HK_TodayEntrustInfo tet, Database db, DbTransaction transaction)
        {
            try
            {
                HK_TodayEntrustDal dal = new HK_TodayEntrustDal();
                dal.Update(tet, db, transaction);
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
        public static HK_CapitalAccountFreezeInfo GetCapitalAccountFreeze(string entrustNumber, Types.FreezeType ft)
        {
            HK_CapitalAccountFreezeDal dal = new HK_CapitalAccountFreezeDal();
            string format = "EntrustNumber='{0}' AND FreezeTypeLogo={1}";
            string where = string.Format(format, entrustNumber, (int)ft);
            IList<HK_CapitalAccountFreezeInfo> list = null;

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
        public static HK_AcccountHoldFreezeInfo GetHoldAccountFreeze(string entrustNumber, Types.FreezeType ft)
        {
            HK_AcccountHoldFreezeDal dal = new HK_AcccountHoldFreezeDal();
            string format = "EntrustNumber='{0}' AND FreezeTypeID={1}";
            string where = string.Format(format, entrustNumber, (int)ft);

            IList<HK_AcccountHoldFreezeInfo> list = null;

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
        public static bool UpdateEntrustOrderMessage(string entrustNubmer, string orderMessage)
        {
            try
            {
                HK_TodayEntrustDal dal = new HK_TodayEntrustDal();
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
        /// 更新当前委托中的原始改单委托单号和IsModifyOrder是否为改单委托标识为True
        /// </summary>
        /// <param name="entrustNubmer">委托单号</param>
        /// <param name="originalNumber">原始委托单号</param>
        /// <returns></returns>
        public static bool UpdateEntrustModifyOrderNumber(string entrustNubmer, string originalNumber)
        {
            try
            {
                HK_TodayEntrustDal dal = new HK_TodayEntrustDal();
                dal.UpdateEntrustModifyOrderNumber(entrustNubmer, originalNumber);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 更新当前委托中的撮合编号
        /// </summary>
        /// <param name="entrustNubmer">委托单号</param>
        /// <param name="macID">撮合编号</param>
        /// <returns></returns>
        public static bool UpdateEntrustMcOrderID(string entrustNubmer, string macID)
        {
            try
            {
                HK_TodayEntrustDal dal = new HK_TodayEntrustDal();
                dal.UpdateEntrustMcOrderID(entrustNubmer, macID);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return false;
            }

            return true;
        }


        /// <summary>
        /// 记录港股改单操作
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool AddHKModifyOrderRequest(HKModifyOrderRequest model)
        {
            try
            {
                HK_ModifyOrderRequestDal dal = new HK_ModifyOrderRequestDal();
                dal.Add(model);

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);

                return false;
            }
        }

        /// <summary>
        /// 更新港股改单委托
        /// </summary>
        /// <param name="model"></param>
        public static void UpdateModifyOrderRequest(HKModifyOrderRequest model)
        {
            try
            {
                HK_ModifyOrderRequestDal dal = new HK_ModifyOrderRequestDal();
                dal.Update(model);

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 根据委托单号获取对应的成交列表
        /// </summary>
        /// <param name="entrustNumber"></param>
        /// <returns></returns>
        public static List<HK_TodayTradeInfo> GetTodayTradeListByEntrustNumber(string entrustNumber)
        {
            HK_TodayTradeDal dal = new HK_TodayTradeDal();
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
        /// 执行带事务的Sql语句
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="tm">事务对象</param>
        /// <returns></returns>
        public static bool HKCapitalAccountProcess(string strSql,
                                                   ReckoningTransaction tm)
        {
            bool result = false;

            try
            {
                if (tm != null)
                {
                    DbHelperSQL.ExecuteSql(strSql, tm);
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
        /// 更新港股持仓
        /// </summary>
        /// <param name="cAvaliable"></param>
        /// <param name="cFreeeze"></param>
        /// <param name="transactionManager"></param>
        /// <param name="holdAccountId"></param>
        /// <returns></returns>
        public static bool UpdateHKHoldAccount(decimal cAvaliable, decimal cFreeeze,
                                               ReckoningTransaction transactionManager, int holdAccountId)
        {
            int results = 0;

            string strSql = string.Empty;

            //var database = new SqlDatabase(TransactionFactory.RC_ConnectionString);
            var database = DataManager.GetDatabase();
            strSql = "UPDATE [HK_AccountHold] SET [AvailableAmount] = [AvailableAmount] + @AvailableHold"
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
        /// 更新港股持仓
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="others"></param>
        /// <param name="dealAmount">数量</param>
        /// <param name="strHoldAccount">持仓账号</param>
        /// <param name="strCode">代码</param>
        /// <param name="iCurrType">币种</param>
        /// <param name="transactionManager">事务对象</param>
        /// <returns></returns>
        public static bool UpdateHKHoldAccount_Sell(decimal scale,
                                                    decimal others,
                                                    decimal dealAmount,
                                                    string strHoldAccount,
                                                    string strCode,
                                                    int iCurrType, ReckoningTransaction transactionManager)
        {
            int results = 0;

            var database = DataManager.GetDatabase();

            //卖出不需要计算保本价
            string strSql = "UPDATE [HK_AccountHold]"
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


        /// <summary>
        /// 添加一条改单成功记录明细
        /// </summary>
        /// <param name="newEntrustNubmer">新委托单号</param>
        /// <param name="originalNumber">被改单委托单号</param>
        /// <param name="modifyType">改单类型(1未报，2量减，3量价变)</param>
        /// <returns></returns>
        public static bool AddModifyOrderSuccessDatils(string newEntrustNubmer, string originalNumber, int modifyType)
        {
            try
            {
                HK_ModifyOrderDetailsInfo model = new HK_ModifyOrderDetailsInfo();
                model.NewRequestNumber = newEntrustNubmer;
                model.OriginalRequestNumber = originalNumber;
                model.ModifyType = modifyType;
                model.CreateDate = DateTime.Now;
                HK_ModifyOrderDetailsDal dal = new HK_ModifyOrderDetailsDal();
                dal.Add(model);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return false;
            }

            return true;
        }
    }
}