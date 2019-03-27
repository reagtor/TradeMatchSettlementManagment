#region Using Namespace

using System;
using System.Collections.Generic;
using System.Data.Common;
using GTA.VTS.Common.CommonUtility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.DAL.Data.HK;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.MemoryData.Interface;

#endregion

namespace ReckoningCounter.MemoryData.HK.Capital
{
    /// <summary>
    /// 港股内存资金表数据库持久化器
    /// 作者：宋涛
    /// </summary>
    public class HKCapitalMemoryDBPersister :
        IMemoryPersister<int, HK_CapitalAccountInfo, HK_CapitalAccount_DeltaInfo>
    {
        #region Implementation of IMemoryPersister<HKCapitalMemoryTable,HK_CapitalAccount_DeltaInfo>

        /// <summary>
        /// 同步更新数据库
        /// </summary>
        public void SyncChangeToBase()
        {
            HK_CapitalAccount_DeltaDal deltaInfoDal = new HK_CapitalAccount_DeltaDal();
            var deltaList = deltaInfoDal.GetAllSum();

            if (deltaList == null)
                return;

            if (deltaList.Count == 0)
                return;

            try
            {
                bool isSuccess = false;
                Database database = DatabaseFactory.CreateDatabase();
                HK_CapitalAccountDal dal = new HK_CapitalAccountDal();

                using (DbConnection connection = database.CreateConnection())
                {
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        foreach (var deltaInfo in deltaList)
                        {
                            dal.AddUpdate(deltaInfo.CapitalAccountLogo,
                                          deltaInfo.AvailableCapitalDelta,
                                          deltaInfo.FreezeCapitalTotalDelta,
                                          deltaInfo.TodayOutInCapital,
                                          deltaInfo.HasDoneProfitLossTotalDelta,
                                          database, transaction);
                        }

                        transaction.Commit();
                        isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogHelper.WriteError(ex.Message, ex);
                    }
                }

                //同步后清除增量表数据
                if (isSuccess)
                {
                    deltaInfoDal.Truncate();
                }

                /*DataManager.ExecuteInTransaction((database, transaction) =>
                                                     {
                                                         foreach (var deltaInfo in deltaList)
                                                         {
                                                             dal.AddUpdate(deltaInfo.CapitalAccountLogo,
                                                                           deltaInfo.AvailableCapitalDelta,
                                                                           deltaInfo.FreezeCapitalTotalDelta,
                                                                           deltaInfo.TodayOutInCapital,
                                                                           deltaInfo.HasDoneProfitLossTotalDelta,
                                                                           database, transaction);
                                                         }

                                                         //同步后清除增量表数据
                                                         CleanDeltaTable(database, transaction);
                                                         deltaList = null;
                                                     });*/
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取所有资金实体
        /// </summary>
        /// <returns></returns>
        public List<HK_CapitalAccountInfo> GetAllBaseTable()
        {
            HK_CapitalAccountDal dal = new HK_CapitalAccountDal();
            List<HK_CapitalAccountInfo> list = dal.GetAllListArray();

            return list;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="baseTable"></param>
        /// <returns></returns>
        public bool InsertBaseTable(HK_CapitalAccountInfo baseTable)
        {
            //不实现
            return true;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="baseTable"></param>
        public void DeleteBaseTable(HK_CapitalAccountInfo baseTable)
        {
            //不实现
        }

        /// <summary>
        /// 持久化
        /// </summary>
        /// <param name="baseTable"></param>
        public void PersistBase(HK_CapitalAccountInfo baseTable)
        {
            //资金表不实现
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseTable"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        public void PersistBase(HK_CapitalAccountInfo baseTable, Database db, DbTransaction transaction)
        {
            //资金表不实现
        }

        /// <summary>
        /// 带事务插入
        /// </summary>
        /// <param name="baseTable"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        public void InsertBaseTableWithTransaction(HK_CapitalAccountInfo baseTable, Database db,
                                                   DbTransaction transaction)
        {
            //资金表不实现
        }

        /// <summary>
        /// 批量更新资金账户事务
        /// </summary>
        /// <param name="baseList">港股资金账户实体</param>
        public void Commit(List<HK_CapitalAccountInfo> baseList)
        {
            try
            {
                bool isSuccess = false;
                HK_CapitalAccountDal dal = new HK_CapitalAccountDal();

                Database database = DatabaseFactory.CreateDatabase();

                using (DbConnection connection = database.CreateConnection())
                {
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        foreach (var data in baseList)
                        {
                            dal.Update(data.CapitalAccountLogo,
                                       data.AvailableCapital,
                                       data.FreezeCapitalTotal,
                                       data.TodayOutInCapital,
                                       data.HasDoneProfitLossTotal, database,
                                       transaction);
                        }

                        transaction.Commit();
                        isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogHelper.WriteError(ex.Message, ex);
                    }
                }

                //提交后清除增量表数据
                if (isSuccess)
                {
                    HK_CapitalAccount_DeltaDal deltaInfoDal = new HK_CapitalAccount_DeltaDal();
                    deltaInfoDal.Truncate();
                }

                /*DataManager.ExecuteInTransaction((database, transaction) =>
                                                     {
                                                         foreach (var data in baseList)
                                                         {
                                                             dal.Update(data.CapitalAccountLogo,
                                                                        data.AvailableCapital,
                                                                        data.FreezeCapitalTotal,
                                                                        data.TodayOutInCapital,
                                                                        data.HasDoneProfitLossTotal, database,
                                                                        transaction);
                                                         }

                                                         //提交后清除增量表数据
                                                         CleanDeltaTable(database, transaction);
                                                     });*/
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取指定id的港股资金账户
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public HK_CapitalAccountInfo GetBaseTable(int k)
        {
            HK_CapitalAccountDal dal = new HK_CapitalAccountDal();
            var data = dal.GetModel(k);

            return data;
        }

        /// <summary>
        /// 持久化资金变化
        /// </summary>
        /// <param name="delta">资金变化实体</param>
        public void PersistChange(HK_CapitalAccount_DeltaInfo delta)
        {
            HK_CapitalAccount_DeltaDal deltaInfoDal = new HK_CapitalAccount_DeltaDal();
            deltaInfoDal.Add(delta);
        }

        /// <summary>
        /// 带事务持久化资金变化
        /// </summary>
        /// <param name="delta">资金变化实体</param>
        /// <param name="db">数据库对象</param>
        /// <param name="transaction">事务对象</param>
        public void PersistChangeWithTransaction(HK_CapitalAccount_DeltaInfo delta, Database db,
                                                 DbTransaction transaction)
        {
            HK_CapitalAccount_DeltaDal deltaInfoDal = new HK_CapitalAccount_DeltaDal();
            deltaInfoDal.Add(delta, db, transaction);
        }
        /// <summary>
        /// 清除资金变化记录
        /// </summary>
        /// <param name="database">数据库对象</param>
        /// <param name="transaction">事务对象</param>
        private void CleanDeltaTable(Database database, DbTransaction transaction)
        {
            HK_CapitalAccount_DeltaDal deltaInfoDal = new HK_CapitalAccount_DeltaDal();
            deltaInfoDal.Delete(database, transaction);
        }
        /// <summary>
        /// 根据条件从数据库中获取相应的基础表数据
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public HK_CapitalAccountInfo GetBaseFromDBByWhere(string where)
        {
            try
            {
                HK_CapitalAccountDal dal = new HK_CapitalAccountDal();
                string sqlWhere = "";

                if (where.Split('@').Length > 1)
                {
                    sqlWhere = string.Format("UserAccountDistributeLogo='{0}'  and  TradeCurrencyType='{1}' ", where.Split('@')[0], where.Split('@')[1]);
                }
                else
                {
                    sqlWhere = string.Format("UserAccountDistributeLogo='{0}' ", where.Split('@')[0]);
                }
                var data = dal.GetListArray(sqlWhere);
                if (data != null && data.Count > 0)
                {
                    return data[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("根据条件从数据库中获取相应的港股资金基础表数据异常:" + where + ex.Message, ex);
                return null;
            }
        }
        #endregion



    }
}