#region Using Namespace

using System;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.MemoryData.Interface;
using ReckoningCounter.MemoryData.Util;
using ReckoningCounter.Model;
using GTA.VTS.Common.CommonUtility;

#endregion

namespace ReckoningCounter.MemoryData.QH.Capital
{
    /// <summary>
    /// 期货资金内存表
    /// 作者：宋涛
    /// </summary>
    public class QHCapitalMemoryTable :
        MemoryTable<int, QH_CapitalAccountTableInfo, QH_CapitalAccountTable_DeltaInfo>
    {
        private int capitalAccountLogoId;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data"></param>
        /// <param name="persister"></param>
        public QHCapitalMemoryTable(QH_CapitalAccountTableInfo data,
                                    IMemoryPersister<int, QH_CapitalAccountTableInfo, QH_CapitalAccountTable_DeltaInfo>
                                        persister)
            : base(data, persister)
        {
            capitalAccountLogoId = data.CapitalAccountLogoId;
        }

        /// <summary>
        /// 获取变化量回滚实体
        /// </summary>
        /// <param name="change"></param>
        /// <returns></returns>
        protected override QH_CapitalAccountTable_DeltaInfo GetRollbackChangeObject(QH_CapitalAccountTable_DeltaInfo change)
        {
            QH_CapitalAccountTable_DeltaInfo roll = new QH_CapitalAccountTable_DeltaInfo();
            roll.AvailableCapitalDelta = -change.AvailableCapitalDelta;
            roll.CapitalAccountLogoId = change.CapitalAccountLogoId;
            roll.CloseFloatProfitLossTotalDelta = -change.CloseFloatProfitLossTotalDelta;
            roll.CloseMarketProfitLossTotalDelta = -change.CloseMarketProfitLossTotalDelta;
            roll.DeltaTime = change.DeltaTime;
            roll.FreezeCapitalTotalDelta = -change.FreezeCapitalTotalDelta;
            roll.ID = change.ID;
            roll.MarginTotalDelta = -change.MarginTotalDelta;
            roll.TodayOutInCapitalDelta = -change.TodayOutInCapitalDelta;

            return roll;
        }

        /// <summary>
        /// 修改变化数据到内存中
        /// </summary>
        /// <param name="delta"></param>
        protected override void ModifyMemoryData(QH_CapitalAccountTable_DeltaInfo delta)
        {

            string txtStart = "期货修改内存表数据{0}AvailableCapital={1}，CloseFloatProfitLossTotal={2},CloseMarketProfitLossTotal={3},FreezeCapitalTotal={4},MarginTotal={5},TodayOutInCapital={6}";
            LogHelper.WriteDebug(string.Format(txtStart, "修改前", data.AvailableCapital, data.CloseFloatProfitLossTotal, data.CloseMarketProfitLossTotal
                              , data.FreezeCapitalTotal, data.MarginTotal, data.TodayOutInCapital) + DateTime.Now);

            data.AvailableCapital += delta.AvailableCapitalDelta;
            data.CloseFloatProfitLossTotal += delta.CloseFloatProfitLossTotalDelta;
            data.CloseMarketProfitLossTotal += delta.CloseMarketProfitLossTotalDelta;
            data.FreezeCapitalTotal += delta.FreezeCapitalTotalDelta;
            data.MarginTotal += delta.MarginTotalDelta;
            data.TodayOutInCapital += delta.TodayOutInCapitalDelta;
            data.CapitalBalance = data.AvailableCapital + data.FreezeCapitalTotal;

            LogHelper.WriteDebug(string.Format(txtStart, "修改后", data.AvailableCapital, data.CloseFloatProfitLossTotal, data.CloseMarketProfitLossTotal
                              , data.FreezeCapitalTotal, data.MarginTotal, data.TodayOutInCapital) + DateTime.Now);

        }

        /// <summary>
        /// 对期货资金表添加一条增量数据
        /// </summary>
        /// <param name="deltaInfo"></param>
        /// <returns></returns>
        public bool AddDelta(QH_CapitalAccountTable_DeltaInfo deltaInfo)
        {
            deltaInfo.DeltaTime = DateTime.Now;

            MemoryLog.WriteQHCapitalInfo(deltaInfo);

            return AddChange(deltaInfo);
        }

        /// <summary>
        /// 先提交数据到数据库，成功后要调用AddDeltaToMemory方法
        /// </summary>
        /// <param name="deltaInfo"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        public void AddDeltaToDB(QH_CapitalAccountTable_DeltaInfo deltaInfo, Database db, DbTransaction transaction)
        {
            deltaInfo.DeltaTime = DateTime.Now;

            MemoryLog.WriteQHCapitalInfo(deltaInfo);

            AddChangeToDB(deltaInfo, db, transaction);
        }

        /// <summary>
        /// 提交数据到内存
        /// </summary>
        /// <param name="deltaInfo"></param>
        public void AddDeltaToMemory(QH_CapitalAccountTable_DeltaInfo deltaInfo)
        {
            AddChangeToMemory(deltaInfo);
        }

        /// <summary>
        /// 先检查在加变化量
        /// </summary>
        /// <param name="func"></param>
        /// <param name="deltaInfo"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public bool CheckAndAddDelta(Func<QH_CapitalAccountTableInfo, QH_CapitalAccountTable_DeltaInfo, bool> func, QH_CapitalAccountTable_DeltaInfo deltaInfo, Database db, DbTransaction transaction)
        {
            deltaInfo.DeltaTime = DateTime.Now;

            bool isSuccess = CheckAndAddChange(func, deltaInfo, db, transaction);
            if (isSuccess)
            {
                MemoryLog.WriteQHCapitalInfo(deltaInfo);
            }

            return isSuccess;
        }

        #region Overrides of FlowMemoryTable<int,QH_CapitalAccountTableInfo,QH_CapitalAccountTable_DeltaInfo>

        //protected override QH_CapitalAccountTableInfo GetCloneBase(QH_CapitalAccountTableInfo baseData)
        //{
        //    QH_CapitalAccountTableInfo newData = new QH_CapitalAccountTableInfo();
        //    newData.AvailableCapital = baseData.AvailableCapital;
        //    newData.BalanceOfTheDay = baseData.BalanceOfTheDay;
        //    newData.CapitalAccountLogoId = baseData.CapitalAccountLogoId;
        //    newData.CapitalBalance = baseData.CapitalBalance;
        //    newData.CloseFloatProfitLossTotal = baseData.CloseFloatProfitLossTotal;
        //    newData.CloseMarketProfitLossTotal = baseData.CloseMarketProfitLossTotal;
        //    newData.FreezeCapitalTotal = baseData.FreezeCapitalTotal;
        //    newData.MarginTotal = baseData.MarginTotal;
        //    newData.TodayOutInCapital = baseData.TodayOutInCapital;
        //    newData.TradeCurrencyType = baseData.TradeCurrencyType;
        //    newData.UserAccountDistributeLogo = baseData.UserAccountDistributeLogo;

        //    return newData;
        //}

        #endregion
    }
}