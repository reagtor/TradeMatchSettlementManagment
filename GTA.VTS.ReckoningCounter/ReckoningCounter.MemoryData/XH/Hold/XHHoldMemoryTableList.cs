#region Using Namespace

using System;
using System.Collections.Generic;
 
using ReckoningCounter.MemoryData.Interface;
using ReckoningCounter.Model;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;

#endregion

namespace ReckoningCounter.MemoryData.XH.Hold
{
    /// <summary>
    /// 现货持仓内存表列表
    /// 作者：宋涛
    /// </summary>
    public class XHHoldMemoryTableList :
        MemoryTableList<int, XH_AccountHoldTableInfo, XH_AccountHoldTableInfo_Delta, XHHoldMemoryTable>
    {
        /// <summary>
        /// 持仓账户+代码+币种-->持仓id
        /// </summary>
        private SyncCache<string, int> holdIDCache = new SyncCache<string, int>();

        private SyncCache<string, SyncList<int>> holdAccountCache = new SyncCache<string, SyncList<int>>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="persister"></param>
        public XHHoldMemoryTableList(IMemoryPersister<int, XH_AccountHoldTableInfo, XH_AccountHoldTableInfo_Delta> persister)
            : base(persister)
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="list"></param>
        protected override void InternalIntialize(List<XH_AccountHoldTableInfo> list)
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
        private bool AddHoldID(XH_AccountHoldTableInfo hold)
        {
            string key = GetKey(hold.UserAccountDistributeLogo,hold.Code, hold.CurrencyTypeId);
            int val = hold.AccountHoldLogoId;

            if(holdIDCache.Contains(key))
                return false;

            holdIDCache.Add(key, val);

            string account = hold.UserAccountDistributeLogo;
            int id = hold.AccountHoldLogoId;
            if(holdAccountCache.Contains(account))
            {
                SyncList<int> idList = holdAccountCache.Get(account);
                idList.Add(id);
            }
            else
            {
                SyncList<int> idList = new SyncList<int>();
                idList.Add(id);
                holdAccountCache.Add(account,idList);
            }

            return true;
        }

        /// <summary>
        /// 内存中不包含对应的持仓
        /// </summary>
        /// <param name="hold"></param>
        /// <returns></returns>
        private bool CanAddHoldID(XH_AccountHoldTableInfo hold)
        {
            string key = GetKey(hold.UserAccountDistributeLogo, hold.Code, hold.CurrencyTypeId);

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
            if(holdAccountCache.Contains(holdAccount))
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
        public XHHoldMemoryTable GetByHoldAccountAndCurrencyType(string holdAccount, string code, int tradeCurrencyType)
        {
            int accountHoldLogoId = GetAccountHoldLogoId(holdAccount,code,tradeCurrencyType);
            if (accountHoldLogoId == -1)
            {
                string format = "HoldAccount={0},Code={1},CurrencyType={2}";
                string desc = string.Format("XHHoldMemoryTableList.GetByHoldAccountAndCurrencyType持仓ID=-1" + format, holdAccount, code, tradeCurrencyType);
                LogHelper.WriteDebug(desc);
                return null;
            }

            return GetByAccountHoldLogoId(accountHoldLogoId);
        }

        private string GetKey(string holdAccount,string code,int tradeCurrencyType)
        {
            return holdAccount + "_" + code + "@" + tradeCurrencyType;
        }

        /// <summary>
        /// 根据accountHoldLogoId获取现货持仓内存表
        /// </summary>
        /// <param name="accountHoldLogoId"></param>
        /// <returns></returns>
        public XHHoldMemoryTable GetByAccountHoldLogoId(int accountHoldLogoId)
        {
            return Get(accountHoldLogoId);
        }

        /// <summary>
        /// 新增一个持仓到数据库，并且保存到内存中(数据库不存在对于持仓)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool AddXHAccountHoldTable(XH_AccountHoldTableInfo data)
        {
            bool canAdd = CanAddHoldID(data);
            if(canAdd)
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
        public bool AddXHAccountHoldTableToMemory(XH_AccountHoldTableInfo data)
        {
            var memoryTable = GetMemoryTableFromBaseTable(data);
            bool isCanAdd = Add(data.AccountHoldLogoId, memoryTable);

            if(isCanAdd)
            {
                AddHoldID(data);
            }

            return isCanAdd;
        }

        #region Overrides of MemoryTableList<int,XH_AccountHoldTableInfo,XH_AccountHoldTableInfo,XHHoldMemoryTable>

        /// <summary>
        /// 获取持仓内存表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override XHHoldMemoryTable GetMemoryTableFromBaseTable(XH_AccountHoldTableInfo data)
        {
            XHHoldMemoryTable memoryTable = null;
            if(data != null)
            {
                memoryTable = new XHHoldMemoryTable(data, this.persister);
            }

            return memoryTable;
        }

        /// <summary>
        /// 获取持仓账号
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override int GetBaseTableKey(XH_AccountHoldTableInfo data)
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