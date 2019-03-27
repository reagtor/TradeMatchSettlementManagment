#region Using Namespace

using System;
using System.Collections.Generic;
using System.Data.Common;
using GTA.VTS.Common.CommonUtility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.Entity.Contants;
using ReckoningCounter.Model;
using ReckoningCounter.DAL.Data.QH;

#endregion

namespace ReckoningCounter.DAL.CustomDataAccess
{
    /// <summary>
    /// 期货数据访问类
    /// </summary>
    public class QHDataAccess
    {
        /// <summary>
        /// 删除持仓冻结记录
        /// </summary>
        /// <param name="holdFreezeLogoId">持仓冻结记录id</param>
        /// <returns>是否成功</returns>
        public static bool DeleteHoldFreeze(int holdFreezeLogoId)
        {
            if (holdFreezeLogoId != -1)
            {
                try
                {
                    QH_HoldAccountFreezeTableDal hDal = new QH_HoldAccountFreezeTableDal();
                    hDal.Delete(holdFreezeLogoId);
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
        /// <param name="holdFreezeLogoId"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static bool ClearHoldFreeze(int holdFreezeLogoId, Database db, DbTransaction transaction)
        {
            if (holdFreezeLogoId != -1)
            {
                try
                {
                    QH_HoldAccountFreezeTableDal hDal = new QH_HoldAccountFreezeTableDal();
                    hDal.Clear(holdFreezeLogoId, db, transaction);
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
        /// <param name="capitalFreezeLogoId">资金冻结记录id</param>
        /// <returns>是否成功 </returns>
        public static bool DeleteCapitalFreeze(int capitalFreezeLogoId)
        {
            if (capitalFreezeLogoId != -1)
            {
                try
                {
                    QH_CapitalAccountFreezeTableDal caDal = new QH_CapitalAccountFreezeTableDal();
                    caDal.Delete(capitalFreezeLogoId);
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
        /// <param name="capitalFreezeLogoId"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static bool ClearCapitalFreeze(int capitalFreezeLogoId, Database db, DbTransaction transaction)
        {
            if (capitalFreezeLogoId != -1)
            {
                try
                {
                    QH_CapitalAccountFreezeTableDal caDal = new QH_CapitalAccountFreezeTableDal();

                    caDal.Clear(capitalFreezeLogoId, db, transaction);
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
                QH_TodayEntrustTableDal dal = new QH_TodayEntrustTableDal();
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
        public static QH_TodayEntrustTableInfo GetEntrustTable(string entrustNumber)
        {
            QH_TodayEntrustTableInfo tet = null;

            QH_TodayEntrustTableDal dal = new QH_TodayEntrustTableDal();
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
        /// 更新委托表
        /// </summary>
        /// <param name="tet">委托表</param>
        /// <returns>是否成功</returns>
        public static bool UpdateEntrustTable(QH_TodayEntrustTableInfo tet)
        {
            try
            {
                QH_TodayEntrustTableDal dal = new QH_TodayEntrustTableDal();
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
        /// 更新委托表
        /// </summary>
        /// <param name="tet">委托表</param>
        /// <param name="rt">事务包装类</param>
        /// <returns>是否成功</returns>
        public static bool UpdateEntrustTable(QH_TodayEntrustTableInfo tet, ReckoningTransaction rt)
        {
            try
            {
                QH_TodayEntrustTableDal dal = new QH_TodayEntrustTableDal();
                dal.Update(tet, rt);
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
        public static List<QH_TodayTradeTableInfo> GetTodayTradeListByEntrustNumber(string entrustNumber)
        {
            QH_TodayTradeTableDal dal = new QH_TodayTradeTableDal();
            string format = "EntrustNumber='{0}'";
            string where = string.Format(format, entrustNumber);

            return dal.GetListArray(where);
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
                QH_TodayEntrustTableDal dal = new QH_TodayEntrustTableDal();
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
        /// 获取期货对应委托单的资金冻结对象
        /// </summary>
        /// <param name="enTrustNumber">委托单号</param>
        /// <param name="ft">冻结类型</param>
        /// <returns>委托单的资金冻结实体</returns>
        public static QH_CapitalAccountFreezeTableInfo GetCapitalAccountFreeze(string enTrustNumber, Types.FreezeType ft)
        {
            QH_CapitalAccountFreezeTableDal dal = new QH_CapitalAccountFreezeTableDal();
            string format = "EntrustNumber='{0}' AND FreezeTypeLogo={1}";
            string where = string.Format(format, enTrustNumber, (int)ft);
            IList<QH_CapitalAccountFreezeTableInfo> list = null;

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
        /// 获取期货对应委托单的持仓冻结对象
        /// </summary>
        /// <param name="enTrustNumber">委托单号</param>
        /// <param name="ft">冻结类型</param>
        /// <returns>委托单的持仓冻结实体</returns>
        public static QH_HoldAccountFreezeTableInfo GetHoldAccountFreeze(string enTrustNumber, Types.FreezeType ft)
        {
            QH_HoldAccountFreezeTableDal dal = new QH_HoldAccountFreezeTableDal();
            string format = "EntrustNumber='{0}' AND FreezeTypeLogo={1}";
            string where = string.Format(format, enTrustNumber, (int)ft);

            IList<QH_HoldAccountFreezeTableInfo> list = null;

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
        /// 执行带事务的sql语句
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="tm">事务对象</param>
        /// <returns>是否成功</returns>
        public static bool QHCapitalAccountProcess(string strSql, ReckoningTransaction tm)
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

        #region 增加期货资金流水
        /// <summary>
        /// 增加期货成交资金流水记录
        /// </summary>
        /// <param name="tet"></param>
        /// <param name="rt"></param>
        /// <returns></returns>
        public static bool AddQH_CapitalFlow(QH_TradeCapitalFlowDetailInfo tet, ReckoningTransaction rt)
        {
            try
            {
                QH_TradeCapitalFlowDetailDal dal = new QH_TradeCapitalFlowDetailDal();
                tet.CreateDateTime = DateTime.Now;
                dal.Add(tet, rt);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 增加期货成交资金流水记录
        /// </summary>
        /// <param name="tet">期货资金流水实体</param>
        /// <returns></returns>
        public static bool AddQH_CapitalFlow(QH_TradeCapitalFlowDetailInfo tet)
        {
            try
            {
                QH_TradeCapitalFlowDetailDal dal = new QH_TradeCapitalFlowDetailDal();
                tet.CreateDateTime = DateTime.Now;
                dal.Add(tet);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 增加期货成交资金流水记录
        /// </summary>
        /// <param name="flowType">流水类型 0-交易 ,1-结算</param>
        /// <param name="margin">保证金</param>
        /// <param name="otherCose">其他费用(用于预留可能用）</param>
        /// <param name="profitLosss">盯市盈亏(流水类型为交易时是每笔成交平仓盯市盈亏，为结算时持仓盯市盈亏)</param>
        /// <param name="tradeID">成交记录编号ID（如果是清算没有ID自动生成一个GUID）</param>
        /// <param name="tradeProceduresFee">交易费用</param>
        /// <param name="userAccount">用户资金账号</param>
        /// <param name="currencyType">币种</param>
        /// <returns></returns>
        public static bool AddQH_CapitalFlow(int flowType, decimal margin, decimal otherCose, decimal profitLosss,
            string tradeID, decimal tradeProceduresFee, string userAccount,int currencyType)
        {
            try
            {
                QH_TradeCapitalFlowDetailInfo detailInfo = new QH_TradeCapitalFlowDetailInfo();
                detailInfo.FlowTypes = flowType;
                detailInfo.Margin = margin;
                detailInfo.OtherCose = otherCose;
                detailInfo.ProfitLoss = profitLosss;
                if (string.IsNullOrEmpty(tradeID))
                {
                    detailInfo.TradeID = Guid.NewGuid().ToString();
                }
                else
                {
                    detailInfo.TradeID = tradeID;
                }
                detailInfo.TradeProceduresFee = tradeProceduresFee;
                detailInfo.CurrencyType = currencyType;
                detailInfo.UserCapitalAccount = userAccount;
                detailInfo.CreateDateTime = DateTime.Now;
                return AddQH_CapitalFlow(detailInfo);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return false;
            }

        }
        #endregion
    }
}