#region Using Namespace

using System;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.MemoryData.Interface;
using ReckoningCounter.MemoryData.Util;
using ReckoningCounter.Model;

#endregion

namespace ReckoningCounter.MemoryData.XH.Hold
{
    /// <summary>
    /// 现货持仓内存表
    /// 作者：宋涛
    /// </summary>
    public class XHHoldMemoryTable : FlowMemoryTable<int, XH_AccountHoldTableInfo, XH_AccountHoldTableInfo_Delta>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data"></param>
        /// <param name="persister"></param>
        public XHHoldMemoryTable(XH_AccountHoldTableInfo data,
                                 IMemoryPersister<int, XH_AccountHoldTableInfo, XH_AccountHoldTableInfo_Delta> persister)
            : base(data, persister)
        {
        }

        #region Overrides of MemoryTable<int,XH_AccountHoldTableInfo,XH_AccountHoldTableInfo>

        /// <summary>
        /// 获取回滚实体
        /// </summary>
        /// <param name="change"></param>
        /// <returns></returns>
        protected override XH_AccountHoldTableInfo_Delta GetRollbackChangeObject(XH_AccountHoldTableInfo_Delta change)
        {
            XH_AccountHoldTableInfo_Delta roll = new XH_AccountHoldTableInfo_Delta();
            roll.AccountHoldLogoId = change.AccountHoldLogoId;
            roll.AvailableAmountDelta = -change.AvailableAmountDelta;
            roll.Data = change.Data;
            roll.FreezeAmountDelta = -change.FreezeAmountDelta;

            return roll;
        }

        /// <summary>
        /// 修改内存持仓
        /// </summary>
        /// <param name="change"></param>
        protected override void ModifyMemoryData(XH_AccountHoldTableInfo_Delta change)
        {
            data.AvailableAmount += change.AvailableAmountDelta;
            data.FreezeAmount += change.FreezeAmountDelta;
        }

        #endregion

        /// <summary>
        /// 先检查在加变化量
        /// </summary>
        /// <param name="func"></param>
        /// <param name="deltaInfo"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public bool CheckAndAddDelta(Func<XH_AccountHoldTableInfo, XH_AccountHoldTableInfo_Delta, bool> func, XH_AccountHoldTableInfo_Delta deltaInfo, Database db, DbTransaction transaction)
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
        public void AddDeltaToDB(XH_AccountHoldTableInfo_Delta deltaInfo, Database db, DbTransaction transaction)
        {
            //MemoryLog.WriteXHCapitalInfo(deltaInfo);

            AddChangeToDB(deltaInfo, db, transaction);
        }

        /// <summary>
        /// 提交数据到内存
        /// </summary>
        /// <param name="deltaInfo"></param>
        public void AddDeltaToMemory(XH_AccountHoldTableInfo_Delta deltaInfo)
        {
            AddChangeToMemory(deltaInfo);
        }

        #region Overrides of FlowMemoryTable<int,XH_AccountHoldTableInfo,XH_AccountHoldTableInfo>

        //protected override XH_AccountHoldTableInfo_Delta CreateChangeDataFromBase(XH_AccountHoldTableInfo hold)
        //{
        //    XH_AccountHoldTableInfo_Delta change = new XH_AccountHoldTableInfo_Delta();
        //    change.Data = hold;
        //    change.AccountHoldLogoId = hold.AccountHoldLogoId;
        //    change.AvailableAmountDelta = hold.AvailableAmount;
        //    change.FreezeAmountDelta = hold.FreezeAmount;
          
        //    return change;
        //}

        /// <summary>
        /// 复制持仓表
        /// </summary>
        /// <param name="baseData"></param>
        /// <returns></returns>
        protected override XH_AccountHoldTableInfo GetCloneBase(XH_AccountHoldTableInfo baseData)
        {
            XH_AccountHoldTableInfo newData = new XH_AccountHoldTableInfo();
            newData.AccountHoldLogoId = baseData.AccountHoldLogoId;
            newData.AvailableAmount = baseData.AvailableAmount;
            newData.BreakevenPrice = baseData.BreakevenPrice;
            newData.Code = baseData.Code;
            newData.CostPrice = baseData.CostPrice;
            newData.CurrencyTypeId = baseData.CurrencyTypeId;
            newData.FreezeAmount = baseData.FreezeAmount;
            newData.HoldAveragePrice = baseData.HoldAveragePrice;
            newData.UserAccountDistributeLogo = baseData.UserAccountDistributeLogo;

            return newData;
        }

        #endregion
    }

    
}