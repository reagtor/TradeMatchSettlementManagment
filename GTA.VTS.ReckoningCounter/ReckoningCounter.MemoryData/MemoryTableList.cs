#region Using Namespace

using System;
using System.Collections.Generic;
using System.Configuration;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.MemoryData.Interface;


#endregion

namespace ReckoningCounter.MemoryData
{
    /// <summary>
    /// 内存表管理器
    /// 作者：宋涛
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TChange"></typeparam>
    /// <typeparam name="TMemoryTable"></typeparam>
    /// <typeparam name="TBase"></typeparam>
    public abstract class MemoryTableList<TKey, TBase, TChange, TMemoryTable>
        where TMemoryTable : MemoryTable<TKey, TBase, TChange>
    {
        /// <summary>
        /// 内部集合
        /// </summary>
        private SyncCache2<TKey, TMemoryTable> cache = new SyncCache2<TKey, TMemoryTable>();

        /// <summary>
        /// 内存数据持久化器
        /// </summary>
        protected IMemoryPersister<TKey, TBase, TChange> persister;

        internal MemoryTableList(IMemoryPersister<TKey, TBase, TChange> persister)
        {
            this.persister = persister;
        }

        /// <summary>
        /// 添加一个内存表
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="table">table</param>
        internal bool Add(TKey key, TMemoryTable table)
        {
            return cache.Add(key, table);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected bool Remove(TKey key)
        {
            return cache.Delete(key);
        }

        /// <summary>
        /// 从内部缓存获取一个内存表
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>table</returns>
        internal TMemoryTable Get(TKey key)
        {
            GetByKey<TKey, TMemoryTable> getByKey = GetMemoryTableByKey;
            TMemoryTable memoryTable = cache.GetWithAdd(key, getByKey);

            return memoryTable;
        }

        /// <summary>
        /// 从内部缓存获取全部的内存表
        /// </summary>
        /// <returns>全部的内存表</returns>
        public ICollection<TMemoryTable> GetAll()
        {
            return cache.GetAll();
        }

        /// <summary>
        /// 从持久化存储获取一个内存表
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private TMemoryTable GetMemoryTableByKey(TKey key)
        {
            var data = persister.GetBaseTable(key);

            return GetMemoryTableFromBaseTable(data);
        }

        /// <summary>
        /// 从基础表创建内存表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected abstract TMemoryTable GetMemoryTableFromBaseTable(TBase data);

        /// <summary>
        /// 获取基础表的主键
        /// </summary>
        /// <param name="data">基础表</param>
        /// <returns>主键</returns>
        protected abstract TKey GetBaseTableKey(TBase data);

        #region begin-end 管理器的开始、结束方法

        /// <summary>
        /// 初始化管理器（加载所有的表）
        /// </summary>
        public void Initialize(bool isNormalExit)
        {
            if (!isNormalExit)
            {
                //如果上次非正常退出，那么先要到数据库同步变化表（增量或流水）到基础表
                persister.SyncChangeToBase();
            }

            if(!CanLoadAllData())
                return;

            //加载所有的基础表
            List<TBase> list = persister.GetAllBaseTable();

            foreach (var data in list)
            {
                TMemoryTable memoryTable = GetMemoryTableFromBaseTable(data);
                TKey key = GetBaseTableKey(data);
                Add(key, memoryTable);
            }

            InternalIntialize(list);
        }

        /// <summary>
        /// 留给子类进行自定义的初始化动作
        /// </summary>
        /// <param name="bases"></param>
        protected virtual void InternalIntialize(List<TBase> bases)
        {
            
        }

        /// <summary>
        /// 程序退出时需要提交内存数据到数据库
        /// </summary>
        public void Commit()
        {
            var collection = cache.GetAll();

            var dataList = new List<TBase>();
            foreach (var memoryTable in collection)
            {
                if (!memoryTable.HasChange)
                    continue;

                var data = memoryTable.Data;
                dataList.Add(data);
            }

            if (dataList.Count > 0)
                persister.Commit(dataList);
        }

        #endregion

        /// <summary>
        /// 初始化是否加载全部数据到内存中，默认全部加载
        /// </summary>
        protected virtual bool CanLoadAllData()
        {
            return true;
        }

        /// <summary>
        /// 获取加载配置信息
        /// </summary>
        /// <returns></returns>
        protected bool GetLoadConfig()
        {
            bool result = true;
            string key = "loadAllData";
            try
            {
                string str = ConfigurationManager.AppSettings[key];
                if (!string.IsNullOrEmpty(str))
                {
                    if (str.Trim() == "2")
                        result = false;
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return result;
        }
    }
}