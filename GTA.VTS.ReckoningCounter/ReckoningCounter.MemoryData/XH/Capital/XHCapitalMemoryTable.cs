#region Using Namespace

using System;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.MemoryData.Interface;
using ReckoningCounter.MemoryData.Util;
using ReckoningCounter.Model;

#endregion

namespace ReckoningCounter.MemoryData.XH.Capital
{
    /// <summary>
    /// 现货资金内存表
    /// 作者：宋涛
    /// </summary>
    public class XHCapitalMemoryTable :
        MemoryTable<int, XH_CapitalAccountTableInfo, XH_CapitalAccountTable_DeltaInfo>
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data"></param>
        /// <param name="persister"></param>
        public XHCapitalMemoryTable(XH_CapitalAccountTableInfo data,
                                    IMemoryPersister<int, XH_CapitalAccountTableInfo, XH_CapitalAccountTable_DeltaInfo>
                                        persister)
            : base(data, persister)
        {
        }

        /// <summary>
        /// 获取回滚实体
        /// </summary>
        /// <param name="change"></param>
        /// <returns></returns>
        protected override XH_CapitalAccountTable_DeltaInfo GetRollbackChangeObject(XH_CapitalAccountTable_DeltaInfo change)
        {
            XH_CapitalAccountTable_DeltaInfo roll = new XH_CapitalAccountTable_DeltaInfo();
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
        protected override void ModifyMemoryData(XH_CapitalAccountTable_DeltaInfo delta)
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
        public bool AddDelta(XH_CapitalAccountTable_DeltaInfo deltaInfo)
        {
            deltaInfo.DeltaTime = DateTime.Now;

            MemoryLog.WriteXHCapitalInfo(deltaInfo);

            return AddChange(deltaInfo); 
        }

        /// <summary>
        /// 先提交数据到数据库，成功后要调用AddDeltaToMemory方法
        /// </summary>
        /// <param name="deltaInfo"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        public void AddDeltaToDB(XH_CapitalAccountTable_DeltaInfo deltaInfo, Database db, DbTransaction transaction)
        {
            deltaInfo.DeltaTime = DateTime.Now;

            MemoryLog.WriteXHCapitalInfo(deltaInfo);

            AddChangeToDB(deltaInfo, db, transaction);
        }

        /// <summary>
        /// 提交数据到内存
        /// </summary>
        /// <param name="deltaInfo"></param>
        public void AddDeltaToMemory(XH_CapitalAccountTable_DeltaInfo deltaInfo)
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
        public bool CheckAndAddDelta(Func<XH_CapitalAccountTableInfo,XH_CapitalAccountTable_DeltaInfo, bool> func, XH_CapitalAccountTable_DeltaInfo deltaInfo, Database db, DbTransaction transaction)
        {
            deltaInfo.DeltaTime = DateTime.Now;

            bool isSuccess = CheckAndAddChange(func, deltaInfo, db, transaction);
            if(isSuccess)
            {
                MemoryLog.WriteXHCapitalInfo(deltaInfo);
            }

            return isSuccess;
        }

        #region Overrides of FlowMemoryTable<int,XH_CapitalAccountTableInfo,XH_CapitalAccountTable_DeltaInfo>

        //protected override XH_CapitalAccountTableInfo GetCloneBase(XH_CapitalAccountTableInfo baseData)
        //{
        //    XH_CapitalAccountTableInfo newData = new XH_CapitalAccountTableInfo();
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