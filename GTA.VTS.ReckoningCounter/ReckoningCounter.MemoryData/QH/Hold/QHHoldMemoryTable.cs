#region Using Namespace

using System;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.MemoryData.Interface;
using ReckoningCounter.Model;

#endregion

namespace ReckoningCounter.MemoryData.QH.Hold
{
    /// <summary>
    /// 期货持仓内存表
    /// 作者：宋涛
    /// </summary>
    public class QHHoldMemoryTable : FlowMemoryTable<int, QH_HoldAccountTableInfo, QH_HoldAccountTableInfo_Delta>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data"></param>
        /// <param name="persister"></param>
        public QHHoldMemoryTable(QH_HoldAccountTableInfo data,
                                 IMemoryPersister<int, QH_HoldAccountTableInfo, QH_HoldAccountTableInfo_Delta> persister)
            : base(data, persister)
        {
        }

        #region Overrides of MemoryTable<int,QH_HoldAccountTableInfo,QH_HoldAccountTableInfo>

        /// <summary>
        /// 获取持仓回滚实体
        /// </summary>
        /// <param name="change"></param>
        /// <returns></returns>
        protected override QH_HoldAccountTableInfo_Delta GetRollbackChangeObject(QH_HoldAccountTableInfo_Delta change)
        {
            QH_HoldAccountTableInfo_Delta roll = new QH_HoldAccountTableInfo_Delta();
            roll.AccountHoldLogoId = change.AccountHoldLogoId;
            roll.Data = change.Data;
            roll.HistoryFreezeAmountDelta = change.HistoryFreezeAmountDelta;
            roll.HistoryHoldAmountDelta = change.TodayHoldAmountDelta;
            roll.TodayFreezeAmountDelta = change.TodayFreezeAmountDelta;
            roll.TodayHoldAmountDelta = change.TodayHoldAmountDelta;

            return roll;
        }

        /// <summary>
        /// 修改内存持仓
        /// </summary>
        /// <param name="change"></param>
        protected override void ModifyMemoryData(QH_HoldAccountTableInfo_Delta change)
        {
            data.HistoryFreezeAmount += change.HistoryFreezeAmountDelta;
            data.HistoryHoldAmount += change.HistoryHoldAmountDelta;

            data.TodayFreezeAmount += change.TodayFreezeAmountDelta;
            data.TodayHoldAmount += change.TodayHoldAmountDelta;

            data.Margin += change.MarginDelta;
        }

        #endregion

        /// <summary>
        /// 先提交数据到数据库，成功后要调用AddDeltaToMemory方法
        /// </summary>
        /// <param name="deltaInfo"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        public void AddDeltaToDB(QH_HoldAccountTableInfo_Delta deltaInfo, Database db, DbTransaction transaction)
        {
            //MemoryLog.WriteXHCapitalInfo(deltaInfo);

            AddChangeToDB(deltaInfo, db, transaction);
        }

        /// <summary>
        /// 提交数据到内存
        /// </summary>
        /// <param name="deltaInfo"></param>
        public void AddDeltaToMemory(QH_HoldAccountTableInfo_Delta deltaInfo)
        {
            AddChangeToMemory(deltaInfo);
        }

        /// <summary>
        /// 先检查再加变化量
        /// </summary>
        /// <param name="func"></param>
        /// <param name="deltaInfo"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public bool CheckAndAddDelta(Func<QH_HoldAccountTableInfo, QH_HoldAccountTableInfo_Delta, bool> func, QH_HoldAccountTableInfo_Delta deltaInfo, Database db, DbTransaction transaction)
        {
            bool isSuccess = CheckAndAddChange(func, deltaInfo, db, transaction);

            return isSuccess;
        }

        #region Overrides of FlowMemoryTable<int,QH_HoldAccountTableInfo,QH_HoldAccountTableInfo>

        //protected override QH_HoldAccountTableInfo_Delta CreateChangeDataFromBase(QH_HoldAccountTableInfo hold)
        //{
        //    QH_HoldAccountTableInfo_Delta change = new QH_HoldAccountTableInfo_Delta();
        //    change.Data = hold;
        //    change.AccountHoldLogoId = hold.AccountHoldLogoId;
        //    change.TodayHoldAmountDelta = hold.TodayHoldAmount;
        //    change.TodayFreezeAmountDelta = hold.TodayFreezeAmount;
        //    change.HistoryHoldAmountDelta = hold.HistoryHoldAmount;
        //    change.HistoryFreezeAmountDelta = hold.HistoryFreezeAmount;
        //    change.MarginDelta = hold.Margin;

        //    return change;
        //}

        /// <summary>
        /// 复制资金表
        /// </summary>
        /// <param name="baseData"></param>
        /// <returns></returns>
        protected override QH_HoldAccountTableInfo GetCloneBase(QH_HoldAccountTableInfo baseData)
        {
            QH_HoldAccountTableInfo newData = new QH_HoldAccountTableInfo();
            newData.AccountHoldLogoId = baseData.AccountHoldLogoId;
            newData.BreakevenPrice = baseData.BreakevenPrice;
            newData.BuySellTypeId = baseData.BuySellTypeId;
            newData.Contract = baseData.Contract;
            newData.CostPrice = baseData.CostPrice;
            newData.HistoryFreezeAmount = baseData.HistoryFreezeAmount;
            newData.HistoryHoldAmount = baseData.HistoryHoldAmount;
            newData.HoldAveragePrice = baseData.HoldAveragePrice;
            newData.Margin = baseData.Margin;
            newData.OpenAveragePrice = baseData.OpenAveragePrice;
            newData.ProfitLoss = baseData.ProfitLoss;
            newData.TodayFreezeAmount = baseData.TodayFreezeAmount;
            newData.TodayHoldAmount = baseData.TodayHoldAmount;
            newData.TodayHoldAveragePrice = baseData.TodayHoldAveragePrice;
            newData.TradeCurrencyType = baseData.TradeCurrencyType;
            newData.UserAccountDistributeLogo = baseData.UserAccountDistributeLogo;

            return newData;
        }

        #endregion
    }
}