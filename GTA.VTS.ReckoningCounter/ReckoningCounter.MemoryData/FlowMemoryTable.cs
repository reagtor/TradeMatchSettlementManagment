#region Using Namespace

using System;
using System.Data.Common;
using GTA.VTS.Common.CommonUtility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.MemoryData.Interface;

#endregion

namespace ReckoningCounter.MemoryData
{
    /// <summary>
    /// 流水内存表基类，操作流水表的内存
    /// 当某个表的记录无法累加时，如持仓表的成本价保本价之类，无法使用增量表记录
    /// 因为成本价保本价必须每次动态计算，这时适合使用流水表，即读时读内存中的值
    /// 写时更新到数据库，多读单写，保证写入序列的唯一性，避免数据库的死锁以及读
    /// 不一致的现象
    /// 作者：宋涛
    /// </summary>
    /// <typeparam name="TKey">key</typeparam>
    /// <typeparam name="TBase">基础表</typeparam>
    /// <typeparam name="TChange">流水表（基础表扩展）</typeparam>
    public abstract class FlowMemoryTable<TKey, TBase, TChange> : MemoryTable<TKey, TBase, TChange>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data">基础表</param>
        /// <param name="persister">内存持久化接口</param>
        protected FlowMemoryTable(TBase data, IMemoryPersister<TKey, TBase, TChange> persister) : base(data, persister)
        {
        }

        /// <summary>
        /// 当需要先读后写时，使用此方法
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public bool ReadAndWrite(Func<TBase, TBase> func)
        {
            rwLock.EnterWriteLock();
            try
            {
                var cloneData = GetCloneBase(data);
                var newData = func(cloneData);

                //TChange change = CreateChangeDataFromBase(newData);

                try
                {
                    //persister.PersistChange(change);
                    persister.PersistBase(newData);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);

                    return false;
                }

                data = newData;
                hasChange = true;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }

            return true;
        }

        /// <summary>
        /// 当需要先读后写时，使用此方法
        /// </summary>
        /// <param name="func"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public void ReadAndWriteInTrans(Func<TBase, TBase> func, Database db, DbTransaction transaction)
        {
            rwLock.EnterWriteLock();
            try
            {
                var cloneData = GetCloneBase(data);
                var newData = func(cloneData);

                //TChange change = CreateChangeDataFromBase(newData);
                bool isPersisSuccess = false;

                try
                {
                    //persister.PersistChangeWithTransaction(change, db, transaction);
                    persister.PersistBase(newData, db, transaction);
                    isPersisSuccess = true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                    isPersisSuccess = false;
                }

                if(isPersisSuccess)
                {
                    data = newData;
                    hasChange = true;
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 从基础表创建一个流水表
        /// </summary>
        /// <param name="baseData"></param>
        /// <returns></returns>
        //protected abstract TChange CreateChangeDataFromBase(TBase baseData);

        protected abstract TBase GetCloneBase(TBase baseData);
    }
}