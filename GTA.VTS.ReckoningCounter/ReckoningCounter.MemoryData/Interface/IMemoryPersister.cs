#region Using Namespace

using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

#endregion

namespace ReckoningCounter.MemoryData.Interface
{
    /// <summary>
    /// 内存表持久化操作接口
    /// 此接口实现内存数据到持久化数据之间的互操作
    /// 此处以数据库作为持久化存储来进行注释说明
    /// 作者：宋涛
    /// </summary>
    /// <typeparam name="TKey">基础表主键</typeparam>
    /// <typeparam name="TBase">基础表</typeparam>
    /// <typeparam name="TChange">变化表（增量表或者流水表）</typeparam>
    public interface IMemoryPersister<TKey, TBase, TChange>
    {
        /// <summary>
        /// 如果上次非正常退出，那么先要到数据库同步变化表到基础表
        /// </summary>
        void SyncChangeToBase();

        /// <summary>
        /// 从数据库获取所有的基础表
        /// </summary>
        List<TBase> GetAllBaseTable();

        /// <summary>
        /// 从数据库获取某个基础表
        /// </summary>
        /// <param name="k">主键</param>
        /// <returns>基础表</returns>
        TBase GetBaseTable(TKey k);

        /// <summary>
        /// 新增一个基础表到数据库
        /// </summary>
        /// <param name="baseTable"></param>
        bool InsertBaseTable(TBase baseTable);

        /// <summary>
        /// 删除一个基础表
        /// </summary>
        /// <param name="baseTable"></param>
        void DeleteBaseTable(TBase baseTable);

        /// <summary>
        /// 每次修改内存数据，都需要持久化增量数据到数据库
        /// </summary>
        /// <param name="delta">增量数据</param>
        void PersistChange(TChange delta);

        /// <summary>
        /// 直接修改基础表数据
        /// </summary>
        /// <param name="baseTable">基础表</param>
        void PersistBase(TBase baseTable);

        /// <summary>
        /// 直接修改基础表数据(带事务）
        /// </summary>
        /// <param name="baseTable">基础表</param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        void PersistBase(TBase baseTable, Database db, DbTransaction transaction);

        /// <summary>
        /// 新增一个基础表到数据库(带事务）--目前暂时都不实现
        /// </summary>
        /// <param name="baseTable"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        void InsertBaseTableWithTransaction(TBase baseTable, Database db, DbTransaction transaction);

        /// <summary>
        /// 每次修改内存数据，都需要持久化增量数据到数据库(带事务）
        /// </summary>
        /// <param name="delta">增量数据</param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        void PersistChangeWithTransaction(TChange delta, Database db, DbTransaction transaction);

        /// <summary>
        /// 程序退出时需要提交内存数据到数据库
        /// </summary>
        void Commit(List<TBase> baseList);

        /// <summary>
        /// 根据条件从数据库中获取相应的基础表数据
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        TBase GetBaseFromDBByWhere(string where);
    }
}