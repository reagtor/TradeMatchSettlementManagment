#region Using Namespace

using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.MemoryData.Interface;
using ReckoningCounter.Model;

#endregion

namespace ReckoningCounter.MemoryData.XH.Hold
{
    /// <summary>
    /// 现货内存持仓表数据库持久化器
    /// 作者：宋涛
    /// </summary>
    public class XHHoldMemoryDBPersister : IMemoryPersister<int, XH_AccountHoldTableInfo, XH_AccountHoldTableInfo_Delta>
    {
        #region Implementation of IMemoryPersister<int,XH_AccountHoldTableInfo,XH_AccountHoldTableInfo>

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
        public List<XH_AccountHoldTableInfo> GetAllBaseTable()
        {
            XH_AccountHoldTableDal dal = new XH_AccountHoldTableDal();
            return dal.GetAllListArray();
        }

        /// <summary>
        /// 获取指定Id持仓
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public XH_AccountHoldTableInfo GetBaseTable(int k)
        {
            XH_AccountHoldTableDal dal = new XH_AccountHoldTableDal();
            return dal.GetModel(k);
        }

        /// <summary>
        /// 插入持仓记录
        /// </summary>
        /// <param name="baseTable"></param>
        /// <returns></returns>
        public bool InsertBaseTable(XH_AccountHoldTableInfo baseTable)
        {
            XH_AccountHoldTableDal dal = new XH_AccountHoldTableDal();
            var hold = dal.GetXhAccountHoldTable(baseTable.UserAccountDistributeLogo, baseTable.Code,
                                                 baseTable.CurrencyTypeId);
            if (hold != null)
                return false;

            int id = dal.Add(baseTable);
            baseTable.AccountHoldLogoId = id;
            return true;
        }

        /// <summary>
        /// 删除持仓记录
        /// </summary>
        /// <param name="baseTable"></param>
        public void DeleteBaseTable(XH_AccountHoldTableInfo baseTable)
        {
            XH_AccountHoldTableDal dal = new XH_AccountHoldTableDal();
            dal.Delete(baseTable.AccountHoldLogoId);
        }

        /// <summary>
        /// 持久化持仓变化
        /// </summary>
        /// <param name="delta"></param>
        public void PersistChange(XH_AccountHoldTableInfo_Delta delta)
        {
            XH_AccountHoldTableDal dal = new XH_AccountHoldTableDal();
            dal.AddUpdate(delta);
        }

        /// <summary>
        /// 更新持仓
        /// </summary>
        /// <param name="baseTable"></param>
        public void PersistBase(XH_AccountHoldTableInfo baseTable)
        {
            XH_AccountHoldTableDal dal = new XH_AccountHoldTableDal();
            dal.Update(baseTable);
        }

        /// <summary>
        /// 带事务更新持仓
        /// </summary>
        /// <param name="baseTable"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        public void PersistBase(XH_AccountHoldTableInfo baseTable, Database db, DbTransaction transaction)
        {
            XH_AccountHoldTableDal dal = new XH_AccountHoldTableDal();
            dal.Update(baseTable, db, transaction);
        }

        /// <summary>
        /// 带事务插入
        /// </summary>
        /// <param name="baseTable"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        public void InsertBaseTableWithTransaction(XH_AccountHoldTableInfo baseTable, Database db, DbTransaction transaction)
        {
            //不实现
        }

        /// <summary>
        /// 带事务更新
        /// </summary>
        /// <param name="delta"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        public void PersistChangeWithTransaction(XH_AccountHoldTableInfo_Delta delta, Database db, DbTransaction transaction)
        {
            XH_AccountHoldTableDal dal = new XH_AccountHoldTableDal();
            dal.AddUpdate(delta, db, transaction);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="baseList"></param>
        public void Commit(List<XH_AccountHoldTableInfo> baseList)
        {
            //do nothing
        }
        /// <summary>
        /// 根据条件从数据库中获取相应的基础表数据
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public XH_AccountHoldTableInfo GetBaseFromDBByWhere(string where)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}