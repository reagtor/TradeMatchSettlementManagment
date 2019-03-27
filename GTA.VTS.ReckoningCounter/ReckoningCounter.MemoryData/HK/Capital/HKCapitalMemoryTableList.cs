#region Using Namespace

using System;
using System.Collections.Generic;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.MemoryData.Interface;

#endregion

namespace ReckoningCounter.MemoryData.HK.Capital
{
    /// <summary>
    /// 港股资金内存表列表
    /// 作者：宋涛
    /// Update by:李健华
    /// Update Date:2010-06-07
    /// Desc.:修改资金表缓存不处理开始就缓存，在开市后使用到的用户再处理
    /// </summary>
    public class HKCapitalMemoryTableList :
        MemoryTableList<int, HK_CapitalAccountInfo, HK_CapitalAccount_DeltaInfo, HKCapitalMemoryTable>
    {
        private SyncCache<string, int> capitalIDCache = new SyncCache<string, int>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="persister"></param>
        public HKCapitalMemoryTableList(
            IMemoryPersister<int, HK_CapitalAccountInfo, HK_CapitalAccount_DeltaInfo> persister)
            : base(persister)
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="list"></param>
        protected override void InternalIntialize(List<HK_CapitalAccountInfo> list)
        {
            base.InternalIntialize(list);

            foreach (var capital in list)
            {
                string key = GetKey(capital.UserAccountDistributeLogo, capital.TradeCurrencyType);
                int val = capital.CapitalAccountLogo;
                capitalIDCache.Add(key, val);
            }
        }

        /// <summary>
        /// 获取资金表ID
        /// </summary>
        /// <param name="capitalAccount"></param>
        /// <param name="tradeCurrencyType"></param>
        /// <returns></returns>
        public int GetCapitalAccountLogo(string capitalAccount, int tradeCurrencyType)
        {
            string key = GetKey(capitalAccount, tradeCurrencyType);

            if (capitalIDCache.Contains(key))
            {
                return capitalIDCache.Get(key);
            }
            #region add 2010-06-07 李健华
            else
            {
                bool isAdd = false;
                //从数据库中获取再添加（同步到内存中）
                HK_CapitalAccountInfo model = persister.GetBaseFromDBByWhere(key);
                if (model != null)
                {
                    isAdd = AddCapitalTableToMemory(model);
                }

                //不管添加是否成功都获取一次，因为有可能在添加的过程中有线程冲突
                //if (isAdd)
                //{
                if (capitalIDCache.Contains(key))
                {
                    return capitalIDCache.Get(key);
                }
                //}
            }
            #endregion

            return -1;
        }

        /// <summary>
        /// 根据资金账户和币种返回资金内存表
        /// </summary>
        /// <param name="capitalAccount"></param>
        /// <param name="tradeCurrencyType"></param>
        /// <returns></returns>
        public HKCapitalMemoryTable GetByCapitalAccountAndCurrencyType(string capitalAccount, int tradeCurrencyType)
        {
            int capitalAccountLogo = GetCapitalAccountLogo(capitalAccount, tradeCurrencyType);
            if (capitalAccountLogo == -1)
                return null;

            return GetByCapitalAccountLogo(capitalAccountLogo);
        }

        private string GetKey(string capitalAccount, int tradeCurrencyType)
        {
            return capitalAccount + "@" + tradeCurrencyType;
        }

        /// <summary>
        /// 根据capitalAccountLogo获取现货资金内存表
        /// </summary>
        /// <param name="capitalAccountLogo"></param>
        /// <returns></returns>
        public HKCapitalMemoryTable GetByCapitalAccountLogo(int capitalAccountLogo)
        {
            // return Get(capitalAccountLogo);
            #region add 2010-06-07 李健华
            HKCapitalMemoryTable item = Get(capitalAccountLogo);
            if (item != null && item.Data != null)
            {
                //防止不全部加载时先调用此方法时无法处理添加相应在的影射
                if (CanAddCapitalID(item.Data))
                {
                    AddCapitalID(item.Data);
                }
            }
            return item;
            #endregion

        }

        private bool CanAddCapitalID(HK_CapitalAccountInfo capital)
        {
            string key = GetKey(capital.UserAccountDistributeLogo, capital.TradeCurrencyType);

            if (capitalIDCache.Contains(key))
                return false;

            return true;
        }

        /// <summary>
        /// 添加一个资金id映射
        /// </summary>
        /// <param name="capital"></param>
        private bool AddCapitalID(HK_CapitalAccountInfo capital)
        {
            string key = GetKey(capital.UserAccountDistributeLogo, capital.TradeCurrencyType);

            int val = capital.CapitalAccountLogo;

            if (capitalIDCache.Contains(key))
                return false;

            capitalIDCache.Add(key, val);

            return true;
        }

        /// <summary>
        /// 新增一个资金账户到数据库，并且保存到内存中(数据库不存在对应资金账户)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool AddCapitalTable(HK_CapitalAccountInfo data)
        {
            bool canAdd = CanAddCapitalID(data);
            if (canAdd)
            {
                try
                {
                    persister.InsertBaseTable(data);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                    return false;
                }

                var memoryTable = GetMemoryTableFromBaseTable(data);
                Add(data.CapitalAccountLogo, memoryTable);

                canAdd = AddCapitalID(data);
                if (!canAdd)
                {
                    persister.DeleteBaseTable(data);
                    Remove(data.CapitalAccountLogo);
                }
            }

            return canAdd;
        }

        /// <summary>
        /// 新增一个资金账户到内存中(数据库已经存在对于持仓)
        /// </summary>
        /// <param name="data">港股资金实体</param>
        /// <returns></returns>
        public bool AddCapitalTableToMemory(HK_CapitalAccountInfo data)
        {
            var memoryTable = GetMemoryTableFromBaseTable(data);
            bool isCanAdd = Add(data.CapitalAccountLogo, memoryTable);


            if (isCanAdd)
            {
                AddCapitalID(data);
            }
            else
            {
 
            }
            

            return isCanAdd;
        }

        #region Overrides of DeltaMemoryTableList<int,HK_CapitalAccountInfo,XH_CapitalAccountTable_DeltaInfo,HKCapitalMemoryTable>

        /// <summary>
        /// 获取资金账户对应内存表
        /// </summary>
        /// <param name="data">资金账户实体</param>
        /// <returns></returns>
        protected override HKCapitalMemoryTable GetMemoryTableFromBaseTable(HK_CapitalAccountInfo data)
        {
            HKCapitalMemoryTable memoryTable = null;
            if (data != null)
            {
                memoryTable = new HKCapitalMemoryTable(data, persister);
            }

            return memoryTable;
        }

        /// <summary>
        /// 获取资金账户实体对应账号
        /// </summary>
        /// <param name="data">资金账户实体</param>
        /// <returns></returns>
        protected override int GetBaseTableKey(HK_CapitalAccountInfo data)
        {
            return data.CapitalAccountLogo;
        }

        /// <summary>
        /// 获取加载信息
        /// </summary>
        /// <returns></returns>
        protected override bool CanLoadAllData()
        {
            return GetLoadConfig();
        }
        #endregion
    }
}