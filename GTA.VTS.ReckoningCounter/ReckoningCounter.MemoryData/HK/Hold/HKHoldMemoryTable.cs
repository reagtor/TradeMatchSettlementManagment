#region Using Namespace

using System;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.MemoryData.Interface;

#endregion

namespace ReckoningCounter.MemoryData.HK.Hold
{
    /// <summary>
    /// 港股持仓内存表
    /// 作者：宋涛
    /// </summary>
    public class HKHoldMemoryTable : FlowMemoryTable<int, HK_AccountHoldInfo, HK_AccountHoldInfo_Delta>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data">港股持仓实体</param>
        /// <param name="persister">持久化接口</param>
        public HKHoldMemoryTable(HK_AccountHoldInfo data,
                                 IMemoryPersister<int, HK_AccountHoldInfo, HK_AccountHoldInfo_Delta> persister)
            : base(data, persister)
        {
        }

        #region Overrides of MemoryTable<int,HK_AccountHoldInfo,HK_AccountHoldInfo>

        /// <summary>
        /// 获取回滚实体
        /// </summary>
        /// <param name="change">变化量实体</param>
        /// <returns></returns>
        protected override HK_AccountHoldInfo_Delta GetRollbackChangeObject(HK_AccountHoldInfo_Delta change)
        {
            HK_AccountHoldInfo_Delta roll = new HK_AccountHoldInfo_Delta();
            roll.AccountHoldLogoId = change.AccountHoldLogoId;
            roll.AvailableAmountDelta = -change.AvailableAmountDelta;
            roll.Data = change.Data;
            roll.FreezeAmountDelta = -change.FreezeAmountDelta;

            return roll;
        }

        /// <summary>
        /// 更新内存表
        /// </summary>
        /// <param name="change">尺寸变化对象</param>
        protected override void ModifyMemoryData(HK_AccountHoldInfo_Delta change)
        {
            data.AvailableAmount += change.AvailableAmountDelta;
            data.FreezeAmount += change.FreezeAmountDelta;
        }

        #endregion

        /// <summary>
        /// 先要检查再添加
        /// </summary>
        /// <param name="func"></param>
        /// <param name="deltaInfo"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public bool CheckAndAddDelta(Func<HK_AccountHoldInfo, HK_AccountHoldInfo_Delta, bool> func,
                                     HK_AccountHoldInfo_Delta deltaInfo, Database db, DbTransaction transaction)
        {
            bool isSuccess = CheckAndAddChange(func, deltaInfo, db, transaction);

            return isSuccess;
        }


        /// <summary>
        /// 先提交数据到数据库，成功后要调用AddDeltaToMemory方法
        /// </summary>
        /// <param name="deltaInfo"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        public void AddDeltaToDB(HK_AccountHoldInfo_Delta deltaInfo, Database db, DbTransaction transaction)
        {
            //MemoryLog.WriteXHCapitalInfo(deltaInfo);

            AddChangeToDB(deltaInfo, db, transaction);
        }

        /// <summary>
        /// 提交数据到内存
        /// </summary>
        /// <param name="deltaInfo"></param>
        public void AddDeltaToMemory(HK_AccountHoldInfo_Delta deltaInfo)
        {
            AddChangeToMemory(deltaInfo);
        }

        #region Overrides of FlowMemoryTable<int,HK_AccountHoldInfo,HK_AccountHoldInfo>

        //protected override HK_AccountHoldInfo_Delta CreateChangeDataFromBase(HK_AccountHoldInfo hold)
        //{
        //    HK_AccountHoldInfo_Delta change = new HK_AccountHoldInfo_Delta();
        //    change.Data = hold;
        //    change.AccountHoldLogoId = hold.AccountHoldLogoId;
        //    change.AvailableAmountDelta = hold.AvailableAmount;
        //    change.FreezeAmountDelta = hold.FreezeAmount;

        //    return change;
        //}

        /// <summary>
        /// 复制持仓实体对象
        /// </summary>
        /// <param name="baseData"></param>
        /// <returns></returns>
        protected override HK_AccountHoldInfo GetCloneBase(HK_AccountHoldInfo baseData)
        {
            HK_AccountHoldInfo newData = new HK_AccountHoldInfo();
            newData.AccountHoldLogoID = baseData.AccountHoldLogoID;
            newData.AvailableAmount = baseData.AvailableAmount;
            newData.BreakevenPrice = baseData.BreakevenPrice;
            newData.Code = baseData.Code;
            newData.CostPrice = baseData.CostPrice;
            newData.CurrencyTypeID = baseData.CurrencyTypeID;
            newData.FreezeAmount = baseData.FreezeAmount;
            newData.HoldAveragePrice = baseData.HoldAveragePrice;
            newData.UserAccountDistributeLogo = baseData.UserAccountDistributeLogo;

            return newData;
        }

        #endregion
    }
}