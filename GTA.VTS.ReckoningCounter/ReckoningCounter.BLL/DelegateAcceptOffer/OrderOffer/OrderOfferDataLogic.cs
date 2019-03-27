#region Using Namespace

using System;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.DAL.Data.HK;
using ReckoningCounter.Entity.Contants;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.Model;

#endregion

namespace ReckoningCounter.BLL.DelegateAcceptOffer.OrderOffer
{
    /// <summary>
    /// 报盘数据处理罗辑
    /// 作者：朱亮
    /// 日期：2008-11-25
    /// </summary>
    internal class OrderOfferDataLogic
    {
        #region 现货更新

        /// <summary>
        /// 更新现货委托单
        /// </summary>
        /// <param name="stockOrder">委托单</param>
        public static bool UpdateStockOrder(XH_TodayEntrustTableInfo stockOrder)
        {
            bool isSuccess = false;
            XH_TodayEntrustTableDal dal = new XH_TodayEntrustTableDal();
            try
            {
                // var sxtetp = new SqlXhTodayEntrustTableProvider(TransactionFactory.RC_ConnectionString, true,
                // string.Empty);
                //sxtetp.Update(stockOrder);
                dal.Update(stockOrder);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteDebug("OrderOfferDataLogic.UpdateStockOrder改使用企业库执行第1次" + ex.Message);
            }

            //使用企业库执行第1次
            if (!isSuccess)
            {
                try
                {
                    // XhTodayEntrustTableDao.Update(stockOrder);
                    dal.Update(stockOrder);
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteDebug("OrderOfferDataLogic.UpdateStockOrder使用企业库执行第2次" + ex.Message);
                }
            }

            //使用企业库执行第2次
            if (!isSuccess)
            {
                try
                {
                    // XhTodayEntrustTableDao.Update(stockOrder);
                    dal.Update(stockOrder);
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError("OrderOfferDataLogic.UpdateStockOrder彻底失败！", ex);
                }
            }

            return isSuccess;
        }

        /// <summary>
        /// 更新现货委托单状态
        /// </summary>
        /// <param name="stockOrder">委托单</param>
        public static void UpdateStockOrderStatus(XH_TodayEntrustTableInfo stockOrder)
        {
            try
            {
                string format = "update xh_todayentrusttable set orderstatusid={0} where entrustnumber='{1}'";
                string sql = string.Format(format, stockOrder.OrderStatusId, stockOrder.EntrustNumber);
                DbHelperSQL.ExecuteSql(sql);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新现货委托单状态，撤单专用，当前状态为最终状态时不更新
        /// </summary>
        /// <param name="stockOrder">委托单</param>
        public static void UpdateStockOrderStatus_Cancel(XH_TodayEntrustTableInfo stockOrder)
        {
            //如果状态时废单，部撤，已撤，已成，那么代表是最终状态
            int a = (int)Types.OrderStateType.DOSCanceled;
            int b = (int)Types.OrderStateType.DOSPartRemoved;
            int c = (int)Types.OrderStateType.DOSRemoved;
            int d = (int)Types.OrderStateType.DOSDealed;

            //如果状态是已报待撤、部成待撤，那么也不需要更新状态
            int e = (int)Types.OrderStateType.DOSRequiredRemoveSoon;
            int f = (int)Types.OrderStateType.DOSPartDealRemoveSoon;

            try
            {
                string format = "update xh_todayentrusttable set orderstatusid={0} where entrustnumber='{1}'";
                format += " and OrderStatusId<>{2} and OrderStatusId<>{3} and OrderStatusId<>{4} and OrderStatusId<>{5}";
                format += " and OrderStatusId<>{6} and OrderStatusId<>{7}";

                string sql = string.Format(format, stockOrder.OrderStatusId, stockOrder.EntrustNumber, a, b, c, d, e, f);
                DbHelperSQL.ExecuteSql(sql);

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新现货委托单信息
        /// </summary>
        /// <param name="stockOrder">委托单</param>
        public static void UpdateStockOrderMessage(XH_TodayEntrustTableInfo stockOrder)
        {
            try
            {
                string msg = stockOrder.OrderMessage;
                if (msg.Length > 50)
                    msg = msg.Substring(0, 49);

                string format = "update xh_todayentrusttable set OrderMessage='{0}' where entrustnumber='{1}'";
                string sql = string.Format(format, msg, stockOrder.EntrustNumber);
                DbHelperSQL.ExecuteSql(sql);
                // DataRepository.Provider.ExecuteNonQuery(CommandType.Text, sql);

                //var sxtetp = new SqlXhTodayEntrustTableProvider(TransactionFactory.RC_ConnectionString, true,
                //                                                string.Empty);
                //sxtetp.Update(stockOrder);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新现货委托单状态和信息
        /// </summary>
        /// <param name="stockOrder">委托单</param>
        public static void UpdateStockOrderStatusAndMessage(XH_TodayEntrustTableInfo stockOrder)
        {
            try
            {
                string msg = stockOrder.OrderMessage;
                if (msg.Length > 50)
                    msg = msg.Substring(0, 49);

                string format =
                    "update xh_todayentrusttable set orderstatusid={0}, OrderMessage='{1}' where entrustnumber='{2}'";
                string sql = string.Format(format, stockOrder.OrderStatusId, msg,
                                           stockOrder.EntrustNumber);

                //DataRepository.Provider.ExecuteNonQuery(CommandType.Text, sql);
                DbHelperSQL.ExecuteSql(sql);
                //var sxtetp = new SqlXhTodayEntrustTableProvider(TransactionFactory.RC_ConnectionString, true,
                //                                                string.Empty);
                //sxtetp.Update(stockOrder);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        #endregion

        #region 期货更新

        /// <summary>
        /// 更新期货委托单
        /// </summary>
        /// <param name="stockOrder">委托单</param>
        public static void UpdateFutureOrder(QH_TodayEntrustTableInfo stockOrder)
        {
            try
            {
                //var sxtetp = new SqlQhTodayEntrustTableProvider(TransactionFactory.RC_ConnectionString, true,
                //string.Empty);
                // sxtetp.Update(stockOrder);
                QH_TodayEntrustTableDal dal = new QH_TodayEntrustTableDal();
                dal.Update(stockOrder);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新期货委托单状态
        /// </summary>
        /// <param name="order">委托单</param>
        public static void UpdateFutureOrderStatus(QH_TodayEntrustTableInfo order)
        {
            try
            {
                string format = "update qh_todayentrusttable set orderstatusid={0} where entrustnumber='{1}'";
                string sql = string.Format(format, order.OrderStatusId, order.EntrustNumber);

                //DataRepository.Provider.ExecuteNonQuery(CommandType.Text, sql);
                DbHelperSQL.ExecuteSql(sql);
                //var sxtetp = new SqlXhTodayEntrustTableProvider(TransactionFactory.RC_ConnectionString, true,
                //                                                string.Empty);
                //sxtetp.Update(order);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新期货委托单状态，撤单专用，当前状态为最终状态时不更新
        /// </summary>
        /// <param name="order">委托单</param>
        public static void UpdateFutureOrderStatus_Cancel(QH_TodayEntrustTableInfo order)
        {
            //如果状态时废单，部撤，已撤，已成，那么代表是最终状态
            int a = (int)Types.OrderStateType.DOSCanceled;
            int b = (int)Types.OrderStateType.DOSPartRemoved;
            int c = (int)Types.OrderStateType.DOSRemoved;
            int d = (int)Types.OrderStateType.DOSDealed;

            //如果状态是已报待撤、部成待撤，那么也不需要更新状态
            int e = (int)Types.OrderStateType.DOSRequiredRemoveSoon;
            int f = (int)Types.OrderStateType.DOSPartDealRemoveSoon;

            try
            {
                string format = "update qh_todayentrusttable set orderstatusid={0} where entrustnumber='{1}'";
                format += " and OrderStatusId<>{2} and OrderStatusId<>{3} and OrderStatusId<>{4} and OrderStatusId<>{5}";
                format += " and OrderStatusId<>{6} and OrderStatusId<>{7}";

                string sql = string.Format(format, order.OrderStatusId, order.EntrustNumber, a, b, c, d, e, f);
                DbHelperSQL.ExecuteSql(sql);

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }


        /// <summary>
        /// 更新期货委托单信息
        /// </summary>
        /// <param name="order">委托单</param>
        public static void UpdateFutureOrderMessage(QH_TodayEntrustTableInfo order)
        {
            try
            {
                string msg = order.OrderMessage;
                if (msg.Length > 50)
                    msg = msg.Substring(0, 49);

                string format = "update qh_todayentrusttable set OrderMessage='{0}' where entrustnumber='{1}'";
                string sql = string.Format(format, msg, order.EntrustNumber);

                //DataRepository.Provider.ExecuteNonQuery(CommandType.Text, sql);
                DbHelperSQL.ExecuteSql(sql);
                //var sxtetp = new SqlXhTodayEntrustTableProvider(TransactionFactory.RC_ConnectionString, true,
                //                                                string.Empty);
                //sxtetp.Update(order);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新期货委托单状态和信息
        /// </summary>
        /// <param name="order">委托单</param>
        public static void UpdateFutureOrderStatusAndMessage(XH_TodayEntrustTableInfo order)
        {
            try
            {
                string msg = order.OrderMessage;
                if (msg.Length > 50)
                    msg = msg.Substring(0, 49);

                string format =
                    "update qh_todayentrusttable set orderstatusid={0}, OrderMessage='{1}' where entrustnumber='{2}'";
                string sql = string.Format(format, order.OrderStatusId, msg,
                                           order.EntrustNumber);

                //DataRepository.Provider.ExecuteNonQuery(CommandType.Text, sql);
                DbHelperSQL.ExecuteSql(sql);
                //var sxtetp = new SqlXhTodayEntrustTableProvider(TransactionFactory.RC_ConnectionString, true,
                //                                                string.Empty);
                //sxtetp.Update(order);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        #endregion

        //public static string GetTraderIdByFundAccount(string strFundAccountId)
        //{
        //    string result = string.Empty;
        //    UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();

        //    if (!string.IsNullOrEmpty(strFundAccountId))
        //    {
        //        //var userAccounts =
        //        // DataRepository.UaUserAccountAllocationTableProvider.GetByUserAccountDistributeLogo(strFundAccountId);
        //        var userAccount = AccountManager.Instance.GetUserByAccount(strFundAccountId);
        //        if (userAccount != null)
        //        {
        //            result = userAccount.UserID;
        //        }
        //        else
        //        {
        //            userAccount = dal.GetModel(strFundAccountId);
        //            if (userAccount != null)
        //                result = userAccount.UserID;
        //        }
        //    }

        //    return result;
        //}

        #region 港股更新

        /// <summary>
        /// 更新港股委托单
        /// </summary>
        /// <param name="stockOrder">委托单</param>
        public static bool UpdateHKOrder(HK_TodayEntrustInfo stockOrder)
        {
            bool isSuccess = false;
            HK_TodayEntrustDal dal = new HK_TodayEntrustDal();
            try
            {
                // var sxtetp = new SqlXhTodayEntrustTableProvider(TransactionFactory.RC_ConnectionString, true,
                // string.Empty);
                //sxtetp.Update(stockOrder);
                //===update 李健华 2009-11-08 此更新报盘时更，所以使用更新不包括是否为改单这两个字段
                //dal.Update(stockOrder);
                dal.UpdateNoIsModifyOrder(stockOrder);
                //=======
                isSuccess = true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteDebug("OrderOfferDataLogic.UpdateHKOrder改使用企业库执行第1次" + ex.Message);
            }

            //使用企业库执行第1次
            if (!isSuccess)
            {
                try
                {
                    // XhTodayEntrustTableDao.Update(stockOrder);
                    dal.Update(stockOrder);
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteDebug("OrderOfferDataLogic.UpdateHKOrder使用企业库执行第2次" + ex.Message);
                }
            }

            //使用企业库执行第2次
            if (!isSuccess)
            {
                try
                {
                    // XhTodayEntrustTableDao.Update(stockOrder);
                    dal.Update(stockOrder);
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError("OrderOfferDataLogic.UpdateHKOrder彻底失败！", ex);
                }
            }

            return isSuccess;
        }

        /// <summary>
        /// 更新港股委托单状态
        /// </summary>
        /// <param name="stockOrder">委托单</param>
        public static void UpdateHKOrderStatus(HK_TodayEntrustInfo stockOrder)
        {
            try
            {
                string format = "update HK_TodayEntrust set orderstatusid={0} where entrustnumber='{1}'";
                string sql = string.Format(format, stockOrder.OrderStatusID, stockOrder.EntrustNumber);
                DbHelperSQL.ExecuteSql(sql);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新港股委托单状态，撤单专用，当前状态为最终状态时不更新
        /// </summary>
        /// <param name="stockOrder">委托单</param>
        public static void UpdateHKOrderStatus_Cancel(HK_TodayEntrustInfo stockOrder)
        {
            //如果状态时废单，部撤，已撤，已成，那么代表是最终状态
            int a = (int)Types.OrderStateType.DOSCanceled;
            int b = (int)Types.OrderStateType.DOSPartRemoved;
            int c = (int)Types.OrderStateType.DOSRemoved;
            int d = (int)Types.OrderStateType.DOSDealed;

            //如果状态是已报待撤、部成待撤，那么也不需要更新状态
            int e = (int)Types.OrderStateType.DOSRequiredRemoveSoon;
            int f = (int)Types.OrderStateType.DOSPartDealRemoveSoon;

            try
            {
                string format = "UPDATE HK_TodayEntrust set orderstatusid={0} where entrustnumber='{1}'";
                format += " and OrderStatusId<>{2} and OrderStatusId<>{3} and OrderStatusId<>{4} and OrderStatusId<>{5}";
                format += " and OrderStatusId<>{6} and OrderStatusId<>{7}";

                string sql = string.Format(format, stockOrder.OrderStatusID, stockOrder.EntrustNumber, a, b, c, d, e, f);
                DbHelperSQL.ExecuteSql(sql);

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新港股委托单信息
        /// </summary>
        /// <param name="stockOrder">委托单</param>
        public static void UpdateHKOrderMessage(HK_TodayEntrustInfo stockOrder)
        {
            try
            {
                string msg = stockOrder.OrderMessage;
                if (msg.Length > 50)
                    msg = msg.Substring(0, 49);

                string format = "update HK_TodayEntrust set OrderMessage='{0}' where entrustnumber='{1}'";
                string sql = string.Format(format, msg, stockOrder.EntrustNumber);
                DbHelperSQL.ExecuteSql(sql);
                // DataRepository.Provider.ExecuteNonQuery(CommandType.Text, sql);

                //var sxtetp = new SqlXhTodayEntrustTableProvider(TransactionFactory.RC_ConnectionString, true,
                //                                                string.Empty);
                //sxtetp.Update(stockOrder);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新港股委托单状态和信息
        /// </summary>
        /// <param name="stockOrder">委托单</param>
        public static void UpdateHKOrderStatusAndMessage(HK_TodayEntrustInfo stockOrder)
        {
            try
            {
                string msg = stockOrder.OrderMessage;
                if (msg.Length > 50)
                    msg = msg.Substring(0, 49);

                string format =
                    "update HK_TodayEntrust set orderstatusid={0}, OrderMessage='{1}' where entrustnumber='{2}'";
                string sql = string.Format(format, stockOrder.OrderStatusID, msg,
                                           stockOrder.EntrustNumber);

                //DataRepository.Provider.ExecuteNonQuery(CommandType.Text, sql);
                DbHelperSQL.ExecuteSql(sql);
                //var sxtetp = new SqlXhTodayEntrustTableProvider(TransactionFactory.RC_ConnectionString, true,
                //                                                string.Empty);
                //sxtetp.Update(stockOrder);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        #endregion
    }
}