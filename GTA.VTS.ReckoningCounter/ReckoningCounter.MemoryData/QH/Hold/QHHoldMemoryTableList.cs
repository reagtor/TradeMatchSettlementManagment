#region Using Namespace

using System;
using System.Collections.Generic;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.MemoryData.Interface;
using ReckoningCounter.Model;

#endregion

namespace ReckoningCounter.MemoryData.QH.Hold
{
    /// <summary>
    /// 期货持仓内存表列表
    /// 作者：宋涛
    /// </summary>
    public class QHHoldMemoryTableList :
        MemoryTableList<int, QH_HoldAccountTableInfo, QH_HoldAccountTableInfo_Delta, QHHoldMemoryTable>
    {
        private SyncCache<string, int> holdIDCache = new SyncCache<string, int>();
        private SyncCache<string, SyncList<int>> holdAccountCache = new SyncCache<string, SyncList<int>>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="persister"></param>
        public QHHoldMemoryTableList(IMemoryPersister<int, QH_HoldAccountTableInfo, QH_HoldAccountTableInfo_Delta> persister)
            : base(persister)
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="list"></param>
        protected override void InternalIntialize(List<QH_HoldAccountTableInfo> list)
        {
            base.InternalIntialize(list);

            foreach (var hold in list)
            {
                AddHoldID(hold);
            }
        }

        /// <summary>
        /// 增加持仓到缓存
        /// </summary>
        /// <param name="hold"></param>
        /// <returns></returns>
        private bool AddHoldID(QH_HoldAccountTableInfo hold)
        {
            string key = GetKey(hold.UserAccountDistributeLogo, hold.Contract,hold.BuySellTypeId, hold.TradeCurrencyType);
            int val = hold.AccountHoldLogoId;

            if(holdIDCache.Contains(key))
                return false;

            holdIDCache.Add(key, val);

            string account = hold.UserAccountDistributeLogo;
            int id = hold.AccountHoldLogoId;
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

        /// <summary>
        /// 检查持仓记录是否未添加
        /// </summary>
        /// <param name="hold"></param>
        /// <returns></returns>
        private bool CanAddHoldID(QH_HoldAccountTableInfo hold)
        {
            string key = GetKey(hold.UserAccountDistributeLogo, hold.Contract, hold.BuySellTypeId,
                                hold.TradeCurrencyType);

            if (holdIDCache.Contains(key))
                return false;

            return true;
        }

        /// <summary>
        /// 获取持仓表ID
        /// </summary>
        /// <param name="holdAccount">持仓账户</param>
        /// <param name="code">合约代码</param>
        /// <param name="buySellTypeId">买卖方向</param>
        /// <param name="tradeCurrencyType">币种</param>
        /// <returns></returns>
        public int GetAccountHoldLogoId(string holdAccount, string code, int buySellTypeId, int tradeCurrencyType)
        {
            string key = GetKey(holdAccount, code, buySellTypeId,tradeCurrencyType);

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
        /// 新增一个持仓到数据库，并且保存到内存中(数据库不存在对于持仓)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool AddQHHoldAccountTable(QH_HoldAccountTableInfo data)
        {
            bool canAdd = CanAddHoldID(data);
            if(canAdd)
            {
                try
                {
                    bool isSuccess = persister.InsertBaseTable(data);
                    if(!isSuccess)
                        return false;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                    return false;
                }

                var memoryTable = GetMemoryTableFromBaseTable(data);
                Add(data.AccountHoldLogoId, memoryTable);
                
                canAdd = AddHoldID(data);
                if(!canAdd)
                {
                    persister.DeleteBaseTable(data);
                    Remove(data.AccountHoldLogoId);
                }

            }

            return canAdd;
            
        }

        /// <summary>
        /// 新增一个持仓到内存中(数据库已经存在对于持仓)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool AddQHHoldAccountTableToMemeory(QH_HoldAccountTableInfo data)
        {
            var memoryTable = GetMemoryTableFromBaseTable(data);
            bool isCanAdd = Add(data.AccountHoldLogoId, memoryTable);

            if(isCanAdd)
            {
                AddHoldID(data);
            }

            return isCanAdd;
        }

        /// <summary>
        /// 根据持仓账户、代码、买卖方向和币种返回持仓内存表
        /// </summary>
        /// <param name="holdAccount"></param>
        /// <param name="code"></param>
        /// <param name="buySellTypeId"></param>
        /// <param name="tradeCurrencyType"></param>
        /// <returns></returns>
        public QHHoldMemoryTable GetByHoldAccountAndCurrencyType(string holdAccount, string code, int buySellTypeId, int tradeCurrencyType)
        {
            int capitalAccountLogo = GetAccountHoldLogoId(holdAccount, code, buySellTypeId, tradeCurrencyType);
            if (capitalAccountLogo == -1)
                return null;

            return GetByAccountHoldLogoId(capitalAccountLogo);
        }

        private string GetKey(string holdAccount, string code, int buySellTypeId, int tradeCurrencyType)
        {
            return holdAccount + "_" + code + "_" + buySellTypeId + "@" + tradeCurrencyType;
        }

        /// <summary>
        /// 根据accountHoldLogoId获取期货持仓内存表
        /// </summary>
        /// <param name="accountHoldLogoId"></param>
        /// <returns></returns>
        public QHHoldMemoryTable GetByAccountHoldLogoId(int accountHoldLogoId)
        {
            return Get(accountHoldLogoId);
        }

        #region Overrides of MemoryTableList<int,QH_HoldAccountTableInfo,QH_HoldAccountTableInfo,XHHoldMemoryTable>

        /// <summary>
        /// 获取资金内存表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override QHHoldMemoryTable GetMemoryTableFromBaseTable(QH_HoldAccountTableInfo data)
        {
            QHHoldMemoryTable memoryTable = null;

            if(data != null)
            {
                memoryTable = new QHHoldMemoryTable(data, this.persister);
            }

            return memoryTable;
        }

        /// <summary>
        /// 获取资金账号
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override int GetBaseTableKey(QH_HoldAccountTableInfo data)
        {
            return data.AccountHoldLogoId;
        }

        #endregion

        /// <summary>
        /// 获取加载配置信息
        /// </summary>
        /// <returns></returns>
        protected override bool CanLoadAllData()
        {
            return GetLoadConfig();
        }
    }
}