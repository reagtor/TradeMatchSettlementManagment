#region Using Namespace

using System;
using System.Collections.Generic;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.MemoryData.Interface;

#endregion

namespace ReckoningCounter.MemoryData.HK.Hold
{
    /// <summary>
    /// 港股持仓内存表列表
    /// 作者：宋涛
    /// </summary>
    public class HKHoldMemoryTableList :
        MemoryTableList<int, HK_AccountHoldInfo, HK_AccountHoldInfo_Delta, HKHoldMemoryTable>
    {
        private SyncCache<string, SyncList<int>> holdAccountCache = new SyncCache<string, SyncList<int>>();

        /// <summary>
        /// 持仓账户+代码+币种-->持仓id
        /// </summary>
        private SyncCache<string, int> holdIDCache = new SyncCache<string, int>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="persister"></param>
        public HKHoldMemoryTableList(IMemoryPersister<int, HK_AccountHoldInfo, HK_AccountHoldInfo_Delta> persister)
            : base(persister)
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="list"></param>
        protected override void InternalIntialize(List<HK_AccountHoldInfo> list)
        {
            base.InternalIntialize(list);

            foreach (var hold in list)
            {
                AddHoldID(hold);
            }
        }

        /// <summary>
        /// 添加一个持仓id映射
        /// </summary>
        /// <param name="hold"></param>
        private bool AddHoldID(HK_AccountHoldInfo hold)
        {
            string key = GetKey(hold.UserAccountDistributeLogo, hold.Code, hold.CurrencyTypeID);
            int val = hold.AccountHoldLogoID;

            if (holdIDCache.Contains(key))
                return false;

            holdIDCache.Add(key, val);

            string account = hold.UserAccountDistributeLogo;
            int id = hold.AccountHoldLogoID;
            if (holdAccountCache.Contains(account))
            {
                SyncList<int> idList = holdAccountCache.Get(account);
                idList.Add(id);
            }
            else
            {
                SyncList<int> idList = new SyncList<int>();
                idList.Add(id);
                holdAccountCache.Add(account, idList);
            }

            return true;
        }

        private bool CanAddHoldID(HK_AccountHoldInfo hold)
        {
            string key = GetKey(hold.UserAccountDistributeLogo, hold.Code, hold.CurrencyTypeID);

            if (holdIDCache.Contains(key))
                return false;

            return true;
        }

        /// <summary>
        /// 获取持仓表ID
        /// </summary>
        /// <param name="holdAccount"></param>
        /// <param name="code"></param>
        /// <param name="tradeCurrencyType"></param>
        /// <returns></returns>
        public int GetAccountHoldLogoId(string holdAccount, string code, int tradeCurrencyType)
        {
            string key = GetKey(holdAccount, code, tradeCurrencyType);

            if (holdIDCache.Contains(key))
                return holdIDCache.Get(key);

            return -1;
        }

        /// <summary>
        /// 获取持仓表ID
        /// </summary>
        /// <param name="holdAccount">持仓账户</param>
        /// <returns>持仓账户所有的持仓id</returns>
        public IList<int> GetAccountHoldLogoID(string holdAccount)
        {
            IList<int> list = null;
            if (holdAccountCache.Contains(holdAccount))
            {
                var slist = holdAccountCache.Get(holdAccount);
                list = slist.GetAll();
            }

            return list;
        }

        /// <summary>
        /// 根据持仓账户、代码和币种返回持仓内存表
        /// </summary>
        /// <param name="holdAccount"></param>
        /// <param name="code"></param>
        /// <param name="tradeCurrencyType"></param>
        /// <returns></returns>
        public HKHoldMemoryTable GetByHoldAccountAndCurrencyType(string holdAccount, string code, int tradeCurrencyType)
        {
            int accountHoldLogoId = GetAccountHoldLogoId(holdAccount, code, tradeCurrencyType);
            if (accountHoldLogoId == -1)
            {
                string format = "HoldAccount={0},Code={1},CurrencyType={2}";
                string desc = string.Format("HKHoldMemoryTableList.GetByHoldAccountAndCurrencyType持仓ID=-1" + format,
                                            holdAccount, code, tradeCurrencyType);
                LogHelper.WriteDebug(desc);
                return null;
            }

            return GetByAccountHoldLogoId(accountHoldLogoId);
        }

        private string GetKey(string holdAccount, string code, int tradeCurrencyType)
        {
            return holdAccount + "_" + code + "@" + tradeCurrencyType;
        }

        /// <summary>
        /// 根据accountHoldLogoId获取现货持仓内存表
        /// </summary>
        /// <param name="accountHoldLogoId"></param>
        /// <returns></returns>
        public HKHoldMemoryTable GetByAccountHoldLogoId(int accountHoldLogoId)
        {
            return Get(accountHoldLogoId);
        }

        /// <summary>
        /// 新增一个持仓到数据库，并且保存到内存中(数据库不存在对于持仓)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool AddAccountHoldTable(HK_AccountHoldInfo data)
        {
            bool canAdd = CanAddHoldID(data);
            if (canAdd)
            {
                try
                {
                    bool isSuccess = persister.InsertBaseTable(data);
                    if (!isSuccess)
                        return false;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                    return false;
                }

                var memoryTable = GetMemoryTableFromBaseTable(data);
                Add(data.AccountHoldLogoID, memoryTable);

                canAdd = AddHoldID(data);
                if (!canAdd)
                {
                    persister.DeleteBaseTable(data);
                    Remove(data.AccountHoldLogoID);
                }
            }

            return canAdd;
        }

        /// <summary>
        /// 新增一个持仓到内存中(数据库已经存在对于持仓)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool AddAccountHoldTableToMemory(HK_AccountHoldInfo data)
        {
            var memoryTable = GetMemoryTableFromBaseTable(data);
            bool isCanAdd = Add(data.AccountHoldLogoID, memoryTable);

            if (isCanAdd)
            {
                AddHoldID(data);
            }

            return isCanAdd;
        }

        /// <summary>
        /// 获取加载信息
        /// </summary>
        /// <returns></returns>
        protected override bool CanLoadAllData()
        {
            return GetLoadConfig();
        }

        #region Overrides of MemoryTableList<int,HK_AccountHoldInfo,HK_AccountHoldInfo,HKHoldMemoryTable>

        /// <summary>
        /// 获取持仓内存表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override HKHoldMemoryTable GetMemoryTableFromBaseTable(HK_AccountHoldInfo data)
        {
            HKHoldMemoryTable memoryTable = null;
            if (data != null)
            {
                memoryTable = new HKHoldMemoryTable(data, this.persister);
            }

            return memoryTable;
        }

        /// <summary>
        /// 获取持仓账号
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override int GetBaseTableKey(HK_AccountHoldInfo data)
        {
            return data.AccountHoldLogoID;
        }

        #endregion
    }
}