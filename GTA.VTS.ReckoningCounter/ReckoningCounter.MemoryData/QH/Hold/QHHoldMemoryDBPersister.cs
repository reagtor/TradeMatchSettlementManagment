#region Using Namespace

using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.MemoryData.Interface;
using ReckoningCounter.Model;

#endregion

namespace ReckoningCounter.MemoryData.QH.Hold
{
    /// <summary>
    /// 期货货内存持仓表数据库持久化器
    /// 作者：宋涛
    /// </summary>
    public class QHHoldMemoryDBPersister : IMemoryPersister<int, QH_HoldAccountTableInfo, QH_HoldAccountTableInfo_Delta>
    {
        #region Implementation of IMemoryPersister<int,QH_HoldAccountTableInfo,QH_HoldAccountTableInfo>

        /// <summary>
        /// 同步数据库
        /// </summary>
        public void SyncChangeToBase()
        {
            //do nothing
        }

        /// <summary>
        /// 获取全部持仓
        /// </summary>
        /// <returns></returns>
        public List<QH_HoldAccountTableInfo> GetAllBaseTable()
        {
            QH_HoldAccountTableDal dal = new QH_HoldAccountTableDal();
            return dal.GetAllListArray();
        }

        /// <summary>
        /// 获取指定id持仓
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public QH_HoldAccountTableInfo GetBaseTable(int k)
        {
            QH_HoldAccountTableDal dal = new QH_HoldAccountTableDal();
            return dal.GetModel(k);
        }

        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="baseTable"></param>
        /// <returns></returns>
        public bool InsertBaseTable(QH_HoldAccountTableInfo baseTable)
        {
            QH_HoldAccountTableDal dal = new QH_HoldAccountTableDal();
            var hold = dal.GetQhAccountHoldTable(baseTable.UserAccountDistributeLogo, baseTable.Contract,
                                                 baseTable.TradeCurrencyType, baseTable.BuySellTypeId);
            if(hold != null)
                return false;

            int id = dal.Add(baseTable);
            baseTable.AccountHoldLogoId = id;
            return true;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="baseTable"></param>
        public void DeleteBaseTable(QH_HoldAccountTableInfo baseTable)
        {
            QH_HoldAccountTableDal dal = new QH_HoldAccountTableDal();
            dal.Delete(baseTable.AccountHoldLogoId);
        }

        /// <summary>
        /// 持久化变化量
        /// </summary>
        /// <param name="delta"></param>
        public void PersistChange(QH_HoldAccountTableInfo_Delta delta)
        {
            QH_HoldAccountTableDal dal = new QH_HoldAccountTableDal();
            dal.AddUpdate(delta);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="baseTable"></param>
        public void PersistBase(QH_HoldAccountTableInfo baseTable)
        {
            QH_HoldAccountTableDal dal = new QH_HoldAccountTableDal();
            dal.Update(baseTable);
        }

        /// <summary>
        /// 带事务更新
        /// </summary>
        /// <param name="baseTable"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        public void PersistBase(QH_HoldAccountTableInfo baseTable, Database db, DbTransaction transaction)
        {
            QH_HoldAccountTableDal dal = new QH_HoldAccountTableDal();
            ReckoningTransaction tm = new ReckoningTransaction();
            tm.Database = db;
            tm.Transaction = transaction;
            dal.Update(baseTable, tm);
        }

        /// <summary>
        /// 带事务插入
        /// </summary>
        /// <param name="baseTable"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        public void InsertBaseTableWithTransaction(QH_HoldAccountTableInfo baseTable, Database db, DbTransaction transaction)
        {
            //不实现
        }

        /// <summary>
        /// 带事务更新
        /// </summary>
        /// <param name="delta"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        public void PersistChangeWithTransaction(QH_HoldAccountTableInfo_Delta delta, Database db, DbTransaction transaction)
        {
            QH_HoldAccountTableDal dal = new QH_HoldAccountTableDal();
            ReckoningTransaction tm = new ReckoningTransaction();
            tm.Database = db;
            tm.Transaction = transaction;

            dal.AddUpdate(delta,tm);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="baseList"></param>
        public void Commit(List<QH_HoldAccountTableInfo> baseList)
        {
            //do nothing
        }
        /// <summary>
        /// 根据条件从数据库中获取相应的基础表数据
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
            public QH_HoldAccountTableInfo GetBaseFromDBByWhere(string where)
        {
            throw new NotImplementedException();
        }
        #endregion

       
    }
}