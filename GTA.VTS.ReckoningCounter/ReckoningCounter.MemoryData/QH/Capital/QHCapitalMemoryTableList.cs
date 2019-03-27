#region Using Namespace

using System;
using System.Collections.Generic;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.MemoryData.Interface;
using ReckoningCounter.Model;

#endregion

namespace ReckoningCounter.MemoryData.QH.Capital
{
    /// <summary>
    /// 期货资金内存表列表
    /// 作者：宋涛
    /// </summary>
    public class QHCapitalMemoryTableList :
        MemoryTableList<int, QH_CapitalAccountTableInfo, QH_CapitalAccountTable_DeltaInfo, QHCapitalMemoryTable>
    {
        private SyncCache<string, int> capitalIDCache = new SyncCache<string, int>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="persister"></param>
        public QHCapitalMemoryTableList(
            IMemoryPersister<int, QH_CapitalAccountTableInfo, QH_CapitalAccountTable_DeltaInfo> persister)
            : base(persister)
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="list"></param>
        protected override void InternalIntialize(List<QH_CapitalAccountTableInfo> list)
        {
            base.InternalIntialize(list);

            foreach (var capital in list)
            {
                string key = GetKey(capital.UserAccountDistributeLogo, capital.TradeCurrencyType);
                int val = capital.CapitalAccountLogoId;
                capitalIDCache.Add(key, val);
            }
        }

        /// <summary>
        /// 或者资金表ID
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
                QH_CapitalAccountTableInfo model = persister.GetBaseFromDBByWhere(key);
                if (model != null)
                {
                    isAdd = AddQHCapitalTableToMemory(model);
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

        private string GetKey(string capitalAccount, int tradeCurrencyType)
        {
            return capitalAccount + "@" + tradeCurrencyType;
        }

        /// <summary>
        /// 根据资金账户和币种或者资金内存表
        /// </summary>
        /// <param name="capitalAccount"></param>
        /// <param name="tradeCurrencyType"></param>
        /// <returns></returns>
        public QHCapitalMemoryTable GetByCapitalAccountAndCurrencyType(string capitalAccount, int tradeCurrencyType)
        {
            int capitalAccountLogo = GetCapitalAccountLogo(capitalAccount, tradeCurrencyType);
            if (capitalAccountLogo == -1)
                return null;

            return GetByCapitalAccountLogo(capitalAccountLogo);
        }

        /// <summary>
        /// 根据capitalAccountLogo获取期货资金内存表
        /// </summary>
        /// <param name="capitalAccountLogo"></param>
        /// <returns></returns>
        public QHCapitalMemoryTable GetByCapitalAccountLogo(int capitalAccountLogo)
        {
            //return Get(capitalAccountLogo);
            #region add 2010-06-07 李健华
            QHCapitalMemoryTable item = Get(capitalAccountLogo);
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

        private bool CanAddCapitalID(QH_CapitalAccountTableInfo capital)
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
        private bool AddCapitalID(QH_CapitalAccountTableInfo capital)
        {
            string key = GetKey(capital.UserAccountDistributeLogo, capital.TradeCurrencyType);

            int val = capital.CapitalAccountLogoId;

            if (capitalIDCache.Contains(key))
                return false;

            capitalIDCache.Add(key, val);

            return true;
        }

        /// <summary>
        /// 新增一个资金账户到数据库，并且保存到内存中(数据库不存在对于持仓)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool AddQHCapitalTable(QH_CapitalAccountTableInfo data)
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
                Add(data.CapitalAccountLogoId, memoryTable);

                canAdd = AddCapitalID(data);
                if (!canAdd)
                {
                    persister.DeleteBaseTable(data);
                    Remove(data.CapitalAccountLogoId);
                }
            }

            return canAdd;
        }

        /// <summary>
        /// 新增一个资金账户到内存中(数据库已经存在对于持仓)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool AddQHCapitalTableToMemory(QH_CapitalAccountTableInfo data)
        {
            var memoryTable = GetMemoryTableFromBaseTable(data);
            bool isCanAdd = Add(data.CapitalAccountLogoId, memoryTable);

            if (isCanAdd)
            {
                AddCapitalID(data);
            }

            return isCanAdd;
        }

        #region Overrides of DeltaMemoryTableList<int,QH_CapitalAccountTableInfo,QH_CapitalAccountTable_DeltaInfo,XHCapitalMemoryTable>

        /// <summary>
        /// 获取资金内存表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override QHCapitalMemoryTable GetMemoryTableFromBaseTable(QH_CapitalAccountTableInfo data)
        {
            QHCapitalMemoryTable memoryTable = null;
            if (data != null)
            {
                memoryTable = new QHCapitalMemoryTable(data, persister);
            }

            return memoryTable;
        }

        /// <summary>
        /// 获取资金账号
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override int GetBaseTableKey(QH_CapitalAccountTableInfo data)
        {
            return data.CapitalAccountLogoId;
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