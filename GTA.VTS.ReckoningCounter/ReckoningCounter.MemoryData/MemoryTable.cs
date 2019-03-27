#region Using Namespace

using System;
using System.Data.Common;
using System.Threading;
using GTA.VTS.Common.CommonUtility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.MemoryData.Interface;

#endregion

namespace ReckoningCounter.MemoryData
{
    /// <summary>
    /// 内存表基类
    /// 作者：宋涛
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TBase"></typeparam>
    /// <typeparam name="TChange"></typeparam>
    public abstract class MemoryTable<TKey, TBase, TChange>
    {
        /// <summary>
        /// 内存基础表
        /// </summary>
        protected TBase data;
        /// <summary>
        /// 是否变化
        /// </summary>
        protected bool hasChange;

        /// <summary>
        /// 内存数据持久化器
        /// </summary>
        protected IMemoryPersister<TKey, TBase, TChange> persister;

        /// <summary>
        /// 内存表锁
        /// </summary>
        protected ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data"></param>
        /// <param name="persister"></param>
        public MemoryTable(TBase data, IMemoryPersister<TKey, TBase, TChange> persister)
        {
            this.data = data;
            this.persister = persister;
        }

        /// <summary>
        /// 获取内存基础表数据
        /// </summary>
        public TBase Data
        {
            get
            {
                rwLock.EnterReadLock();
                try
                {
                    return data;
                }
                finally
                {
                    rwLock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// 是否增加过变化数据
        /// </summary>
        public bool HasChange
        {
            get { return hasChange; }
        }

        /// <summary>
        /// 增加一条变化数据
        /// </summary>
        /// <param name="change"></param>
        /// <returns>是否成功</returns>
        protected bool AddChange(TChange change)
        {
            //先插入到数据库变化表
            try
            {
                persister.PersistChange(change);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);

                //插入失败，直接返回
                return false;
            }

            //插入数据库成功后，再同步到内存表中
            SyncToMemory(change);

            return true;
        }

        /// <summary>
        /// 增加一条变化数据（带事务）,如果执行成功，但是事务失败，那么要回滚内存变化
        /// </summary>
        /// <param name="change"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns>是否成功</returns>
        protected bool AddChange(TChange change, Database db, DbTransaction transaction)
        {
            //先插入到数据库变化表
            try
            {
                persister.PersistChangeWithTransaction(change, db, transaction);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);

                //插入失败，直接返回
                return false;
            }

            //插入数据库成功后，再同步到内存表中
            ModifyMemoryData(change);
            hasChange = true;

            return true;
        }

        /// <summary>
        /// 先要检查再添加（保证检查时是单线程只有一个在跑）,如果执行成功，但是事务失败，
        /// 那么需要回滚内存
        /// </summary>
        /// <param name="func"></param>
        /// <param name="change"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        protected bool CheckAndAddChange(Func<TBase, TChange, bool> func, TChange change, Database db, DbTransaction transaction)
        {
            bool isSuccess = false;
            rwLock.EnterWriteLock();
            try
            {
                isSuccess = func(data, change);
                if(isSuccess)
                {
                    isSuccess = AddChange(change, db, transaction);
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }

            return isSuccess;
        }

        /// <summary>
        /// 增加一条变化数据
        /// </summary>
        /// <param name="change"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        protected void AddChangeToDB(TChange change, Database db, DbTransaction transaction)
        {
            //插入到数据库变化表
            persister.PersistChangeWithTransaction(change, db, transaction);
        }

        /// <summary>
        /// 同步内存表
        /// </summary>
        /// <param name="change"></param>
        protected void AddChangeToMemory(TChange change)
        {
            SyncToMemory(change);
        }

        /// <summary>
        /// 插入数据库成功后，再同步到内存表中
        /// </summary>
        /// <param name="change"></param>
        protected void SyncToMemory(TChange change)
        {
            rwLock.EnterWriteLock();
            try
            {
                ModifyMemoryData(change);
                hasChange = true;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 当CheckAndAddChange和AddChange（带事务的）方法所在的事务执行失败后，
        /// 要调用此方法进行内存回滚（因为上面两个方法已经把内存修改了）
        /// </summary>
        /// <param name="change"></param>
        public void RollBackMemory(TChange change)
        {
            TChange roll = GetRollbackChangeObject(change);

            SyncToMemory(roll);
        }

        /// <summary>
        /// 根据change对象获取一个相反的回滚对象（即每个字段都取反）
        /// </summary>
        /// <param name="change"></param>
        /// <returns></returns>
        protected abstract TChange GetRollbackChangeObject(TChange change);

        /// <summary>
        /// 修改变化数据到内存中
        /// </summary>
        /// <param name="change"></param>
        protected abstract void ModifyMemoryData(TChange change);
    }
}