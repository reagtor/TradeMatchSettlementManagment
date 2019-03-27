#region Using Namespace

using System;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.MemoryData.Interface;
using ReckoningCounter.MemoryData.Util;

#endregion

namespace ReckoningCounter.MemoryData.HK.Capital
{
    /// <summary>
    /// 港股资金内存表
    /// 作者：宋涛
    /// </summary>
    public class HKCapitalMemoryTable :
        MemoryTable<int, HK_CapitalAccountInfo, HK_CapitalAccount_DeltaInfo>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data">港股资金账户实体</param>
        /// <param name="persister">持久化接口</param>
        public HKCapitalMemoryTable(HK_CapitalAccountInfo data,
                                    IMemoryPersister<int, HK_CapitalAccountInfo, HK_CapitalAccount_DeltaInfo>
                                        persister)
            : base(data, persister)
        {
        }

        /// <summary>
        /// 获取资金变化回滚实体
        /// </summary>
        /// <param name="change"></param>
        /// <returns></returns>
        protected override HK_CapitalAccount_DeltaInfo GetRollbackChangeObject(HK_CapitalAccount_DeltaInfo change)
        {
            HK_CapitalAccount_DeltaInfo roll = new HK_CapitalAccount_DeltaInfo();
            roll.AvailableCapitalDelta = -change.AvailableCapitalDelta;
            roll.CapitalAccountLogo = change.CapitalAccountLogo;
            roll.DeltaTime = change.DeltaTime;
            roll.FreezeCapitalTotalDelta = -change.FreezeCapitalTotalDelta;
            roll.HasDoneProfitLossTotalDelta = -change.HasDoneProfitLossTotalDelta;
            roll.ID = change.ID;
            roll.TodayOutInCapital = -change.TodayOutInCapital;

            return roll;
        }

        /// <summary>
        /// 修改变化数据到内存中
        /// </summary>
        /// <param name="delta"></param>
        protected override void ModifyMemoryData(HK_CapitalAccount_DeltaInfo delta)
        {
            data.AvailableCapital += delta.AvailableCapitalDelta;
            data.FreezeCapitalTotal += delta.FreezeCapitalTotalDelta;
            data.HasDoneProfitLossTotal += delta.HasDoneProfitLossTotalDelta;
            data.TodayOutInCapital += delta.TodayOutInCapital;
            data.CapitalBalance = data.AvailableCapital + data.FreezeCapitalTotal;
        }

        /// <summary>
        /// 对现货资金表添加一条增量数据,同时提交数据到数据库和内存
        /// </summary>
        /// <param name="deltaInfo"></param>
        /// <returns></returns>
        public bool AddDelta(HK_CapitalAccount_DeltaInfo deltaInfo)
        {
            deltaInfo.DeltaTime = DateTime.Now;

            MemoryLog.WriteHKCapitalInfo(deltaInfo);

            return AddChange(deltaInfo);
        }

        /// <summary>
        /// 先提交数据到数据库，成功后要调用AddDeltaToMemory方法
        /// </summary>
        /// <param name="deltaInfo"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        public void AddDeltaToDB(HK_CapitalAccount_DeltaInfo deltaInfo, Database db, DbTransaction transaction)
        {
            deltaInfo.DeltaTime = DateTime.Now;

            MemoryLog.WriteHKCapitalInfo(deltaInfo);

            AddChangeToDB(deltaInfo, db, transaction);
        }

        /// <summary>
        /// 提交数据到内存
        /// </summary>
        /// <param name="deltaInfo">资金变化实体</param>
        public void AddDeltaToMemory(HK_CapitalAccount_DeltaInfo deltaInfo)
        {
            AddChangeToMemory(deltaInfo);
        }

        /// <summary>
        /// 先要检查再添加
        /// </summary>
        /// <param name="func"></param>
        /// <param name="deltaInfo"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public bool CheckAndAddDelta(Func<HK_CapitalAccountInfo, HK_CapitalAccount_DeltaInfo, bool> func,
                                     HK_CapitalAccount_DeltaInfo deltaInfo, Database db, DbTransaction transaction)
        {
            deltaInfo.DeltaTime = DateTime.Now;

            bool isSuccess = CheckAndAddChange(func, deltaInfo, db, transaction);
            if (isSuccess)
            {
                MemoryLog.WriteHKCapitalInfo(deltaInfo);
            }

            return isSuccess;
        }

        #region Overrides of FlowMemoryTable<int,HK_CapitalAccountInfo,XH_CapitalAccountTable_DeltaInfo>

        //protected override HK_CapitalAccountInfo GetCloneBase(HK_CapitalAccountInfo baseData)
        //{
        //    HK_CapitalAccountInfo newData = new HK_CapitalAccountInfo();
        //    newData.AvailableCapital = baseData.AvailableCapital;
        //    newData.BalanceOfTheDay = baseData.BalanceOfTheDay;
        //    newData.CapitalAccountLogo = baseData.CapitalAccountLogo;
        //    newData.CapitalBalance = baseData.CapitalBalance;
        //    newData.FreezeCapitalTotal = baseData.FreezeCapitalTotal;
        //    newData.HasDoneProfitLossTotal = baseData.HasDoneProfitLossTotal;
        //    newData.TodayOutInCapital = baseData.TodayOutInCapital;
        //    newData.TradeCurrencyType = baseData.TradeCurrencyType;
        //    newData.UserAccountDistributeLogo = baseData.UserAccountDistributeLogo;

        //    return newData;
        //}

        #endregion
    }
}