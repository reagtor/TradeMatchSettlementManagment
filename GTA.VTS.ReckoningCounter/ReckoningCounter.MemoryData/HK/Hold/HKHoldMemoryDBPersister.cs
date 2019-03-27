#region Using Namespace

using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.MemoryData.Interface;

#endregion

namespace ReckoningCounter.MemoryData.HK.Hold
{
    /// <summary>
    /// 港股内存持仓表数据库持久化器
    /// 作者：宋涛
    /// </summary>
    public class HKHoldMemoryDBPersister : IMemoryPersister<int, HK_AccountHoldInfo, HK_AccountHoldInfo_Delta>
    {
        #region Implementation of IMemoryPersister<int,HK_AccountHoldInfo,HK_AccountHoldInfo>

        /// <summary>
        /// 同步数据库
        /// </summary>
        public void SyncChangeToBase()
        {
            //do nothing
        }

        /// <summary>
        /// 获取全部港股持仓实体
        /// </summary>
        /// <returns></returns>
        public List<HK_AccountHoldInfo> GetAllBaseTable()
        {
            HK_AccountHoldDal dal = new HK_AccountHoldDal();
            return dal.GetAllListArray();
        }

        /// <summary>
        /// 获取指定Id的公共持仓实体
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public HK_AccountHoldInfo GetBaseTable(int k)
        {
            HK_AccountHoldDal dal = new HK_AccountHoldDal();
            return dal.GetModel(k);
        }

        /// <summary>
        /// 新增港股持仓
        /// </summary>
        /// <param name="baseTable"></param>
        /// <returns></returns>
        public bool InsertBaseTable(HK_AccountHoldInfo baseTable)
        {
            HK_AccountHoldDal dal = new HK_AccountHoldDal();
            var hold = dal.GetHKAccountHoldInfo(baseTable.UserAccountDistributeLogo, baseTable.Code,
                                                baseTable.CurrencyTypeID);
            if (hold != null)
                return false;

            int id = dal.Add(baseTable);
            baseTable.AccountHoldLogoID = id;
            return true;
        }

        /// <summary>
        /// 删除持仓记录
        /// </summary>
        /// <param name="baseTable"></param>
        public void DeleteBaseTable(HK_AccountHoldInfo baseTable)
        {
            HK_AccountHoldDal dal = new HK_AccountHoldDal();
            dal.Delete(baseTable.AccountHoldLogoID);
        }

        /// <summary>
        /// 更新数据库改变量
        /// </summary>
        /// <param name="delta"></param>
        public void PersistChange(HK_AccountHoldInfo_Delta delta)
        {
            HK_AccountHoldDal dal = new HK_AccountHoldDal();
            dal.AddUpdate(delta);
        }

        /// <summary>
        /// 更新数据库持仓
        /// </summary>
        /// <param name="baseTable"></param>
        public void PersistBase(HK_AccountHoldInfo baseTable)
        {
            HK_AccountHoldDal dal = new HK_AccountHoldDal();
            dal.Update(baseTable);
        }

        /// <summary>
        /// 更新数据库持仓
        /// </summary>
        /// <param name="baseTable">港股持仓实体</param>
        /// <param name="db">数据库对象</param>
        /// <param name="transaction">事务对象</param>
        public void PersistBase(HK_AccountHoldInfo baseTable, Database db, DbTransaction transaction)
        {
            HK_AccountHoldDal dal = new HK_AccountHoldDal();
            dal.Update(baseTable, db, transaction);
        }

        /// <summary>
        /// 带事务插入数据库
        /// </summary>
        /// <param name="baseTable">港股持仓实体</param>
        /// <param name="db">数据库对象</param>
        /// <param name="transaction">事务对象</param>
        public void InsertBaseTableWithTransaction(HK_AccountHoldInfo baseTable, Database db, DbTransaction transaction)
        {
            //不实现
        }

        /// <summary>
        /// 带事务更新数据库
        /// </summary>
        /// <param name="delta">数据库变化对象</param>
        /// <param name="db">数据库对象</param>
        /// <param name="transaction">事务对象</param>
        public void PersistChangeWithTransaction(HK_AccountHoldInfo_Delta delta, Database db, DbTransaction transaction)
        {
            HK_AccountHoldDal dal = new HK_AccountHoldDal();
            dal.AddUpdate(delta, db, transaction);
        }

        /// <summary>
        /// 批量更新持仓
        /// </summary>
        /// <param name="baseList"></param>
        public void Commit(List<HK_AccountHoldInfo> baseList)
        {
            //do nothing
        }
        /// <summary>
        /// 根据条件从数据库中获取相应的基础表数据
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public HK_AccountHoldInfo GetBaseFromDBByWhere(string where)
        {
            throw new System.NotImplementedException();
        }

        #endregion


     

         
    }
}