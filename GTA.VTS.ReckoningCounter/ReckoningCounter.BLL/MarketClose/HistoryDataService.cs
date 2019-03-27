#region Using Namespace

using System;
using System.Data.SqlClient;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.ScheduleManagement;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.Model;

#endregion

namespace ReckoningCounter.BLL.MarketClose
{
    /// <summary>
    /// 历史数据服务
    /// 作者：宋涛
    /// 日期：2008-12-20
    /// </summary>
    public class HistoryDataService
    {
        public static bool IsProcessStock = false;
        public static bool IsProcessFuture = false;

        #region 现货

        /// <summary>
        /// 进行现货盘后历史数据处理
        /// </summary>
        public static void ProcessStock()
        {
            if(IsProcessStock)
                return;

            try
            {
                //日期的更新放在存储过程里面
                DbHelperSQL.BeginRunProcedure("[RC_XH_HistoryDataService]", ProcessStockCallBack);
                IsProcessStock = true;

                //DataManager.ExecuteInTransaction(tm =>
                //                                     {
                //                                         //1.处理当日委托表（收盘后需要将当日委托移到历史委托中，未完全成交的委托需进行删除）
                //                                         //ProcessStockTodayEntrust(tm);

                //                                         //2.处理当日成交表（收盘后需要将当日成交移到历史成交表中）
                //                                         //ProcessStockTodayTrade(tm);              


                //                                         //3.处理资金表（收盘后需要将当日出入金清空,上日结存需等于可用资金）
                //                                         //ProcessStockTodayOutInCapital(tm);

                //                                         //4.处理银行表（收盘后需要将当日出入金清空,上日结存需等于可用资金）
                //                                         //ProcessBankTodayOutInCapital(tm);
                //                                         //DataRepository.Provider.ExecuteNonQuery(tm, "[RC_XH_HistoryDataService]");
                //                                         DbHelperSQL.RunProcedure("[RC_XH_HistoryDataService]", commandTimeOut, tm);


                //                                         //更新数据库中现货盘后历史数据处理日期
                //                                         StatusTableChecker.UpdateStockHistoryDataProcessDate(tm);
                //                                     });
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("", ex);
            }
        }

        private static void ProcessStockCallBack(IAsyncResult iResult)
        {
            //更新数据库中期货盘后历史数据处理日期
            //StatusTableChecker.UpdateStockHistoryDataProcessDate();

            SqlConnection conn = iResult.AsyncState as SqlConnection;

            if (conn != null)
            {
                try
                {
                    conn.Close();
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }

            IsProcessStock = false;
        }

        /// <summary>
        /// 清除未成交表
        /// </summary>
        public static void ProcessUnReckonedTable()
        {
            string sql = "delete from BD_UnReckonedDealTable";

            try
            {
                DataManager.ExecuteInTransaction(tm => { DbHelperSQL.ExecuteSql(sql); });
                //DataRepository.Provider.ExecuteNonQuery(tm, CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("", ex);
            }
        }

        #endregion

        #region 港股

        /// <summary>
        /// 进行港股盘后历史数据处理
        /// </summary>
        public static void ProcessHK()
        {
            if (IsProcessStock)
                return;

            try
            {
                //TODO：需要添加存储过程
                //日期的更新放在存储过程里面
                DbHelperSQL.BeginRunProcedure("[RC_HK_HistoryDataService]", ProcessHKCallBack);
                IsProcessStock = true;

                //DataManager.ExecuteInTransaction(tm =>
                //                                     {
                //                                         //1.处理当日委托表（收盘后需要将当日委托移到历史委托中，未完全成交的委托需进行删除）
                //                                         //ProcessStockTodayEntrust(tm);

                //                                         //2.处理当日成交表（收盘后需要将当日成交移到历史成交表中）
                //                                         //ProcessStockTodayTrade(tm);              


                //                                         //3.处理资金表（收盘后需要将当日出入金清空,上日结存需等于可用资金）
                //                                         //ProcessStockTodayOutInCapital(tm);

                //                                         //4.处理银行表（收盘后需要将当日出入金清空,上日结存需等于可用资金）
                //                                         //ProcessBankTodayOutInCapital(tm);
                //                                         //DataRepository.Provider.ExecuteNonQuery(tm, "[RC_XH_HistoryDataService]");
                //                                         DbHelperSQL.RunProcedure("[RC_XH_HistoryDataService]", commandTimeOut, tm);


                //                                         //更新数据库中现货盘后历史数据处理日期
                //                                         StatusTableChecker.UpdateStockHistoryDataProcessDate(tm);
                //                                     });
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("", ex);
            }
        }

        private static void ProcessHKCallBack(IAsyncResult iResult)
        {
            //更新数据库中期货盘后历史数据处理日期
            //StatusTableChecker.UpdateStockHistoryDataProcessDate();

            SqlConnection conn = iResult.AsyncState as SqlConnection;

            if (conn != null)
            {
                try
                {
                    conn.Close();
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }

            IsProcessStock = false;
        }

        #endregion

        #region 期货

        /// <summary>
        /// 进行期货盘后历史数据处理
        /// </summary>
        public static void ProcessFuture()
        {
            if(IsProcessFuture)
                return;

            try
            {
                DbHelperSQL.BeginRunProcedure("[RC_QH_HistoryDataService]", ProcessFutureCallBack);
                IsProcessFuture = true;

                //DataManager.ExecuteInTransaction(tm =>
                //                                     {
                //                                         //1.处理当日委托表（收盘后需要将当日委托移到历史委托中，未完全成交的委托需进行删除）
                //                                         //ProcessFutureTodayEntrust(tm);

                //                                         //2.处理当日成交表（收盘后需要将当日成交移到历史成交表中）
                //                                         //ProcessFutureTodayTrade(tm);

                //                                         //3.处理资金表（收盘后需要将当日出入金清空,上日结存需等于可用资金）
                //                                         //ProcessFutureTodayOutInCapital(tm);

                //                                         //DataRepository.Provider.ExecuteNonQuery(tm, "[RC_QH_HistoryDataService]");
                //                                         DbHelperSQL.RunProcedure("[RC_QH_HistoryDataService]", commandTimeOut, tm);

                //                                         //4.处理期货持仓表（收盘后需要将今日开仓变为历史开仓）
                //                                         //ProcessFutureHold(tm);

                //                                         //更新数据库中期货盘后历史数据处理日期
                //                                         StatusTableChecker.UpdateFutureHistoryDataProcessDate(tm);
                //                                     });
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("", ex);
            }
        }

        private static void ProcessFutureCallBack(IAsyncResult iResult)
        {
            //更新数据库中期货盘后历史数据处理日期
            //StatusTableChecker.UpdateFutureHistoryDataProcessDate(tm);
            SqlConnection conn = iResult.AsyncState as SqlConnection;

            if(conn != null)
            {
                try
                {
                    conn.Close();
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                   
                }
            }

            IsProcessFuture = false;
        }

        #endregion
    }
}