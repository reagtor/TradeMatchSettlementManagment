#region Using Namespace

using System;
using System.Collections.Generic;
using System.Data.Common;
using GTA.VTS.Common.CommonUtility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.MemoryData.Interface;
using ReckoningCounter.Model;

#endregion

namespace ReckoningCounter.MemoryData.XH.Capital
{
    /// <summary>
    /// 现货内存资金表数据库持久化器
    /// 作者：宋涛
    /// </summary>
    public class XHCapitalMemoryDBPersister :
        IMemoryPersister<int, XH_CapitalAccountTableInfo, XH_CapitalAccountTable_DeltaInfo>
    {
        #region Implementation of IMemoryPersister<XHCapitalMemoryTable,XH_CapitalAccountTable_DeltaInfo>

        /// <summary>
        /// 构造函数
        /// </summary>
        public void SyncChangeToBase()
        {
            XH_CapitalAccountTable_DeltaInfoDal deltaInfoDal = new XH_CapitalAccountTable_DeltaInfoDal();
            var deltaList = deltaInfoDal.GetAllSum();

            if (deltaList == null)
                return;

            if (deltaList.Count == 0)
                return;

            try
            {
                bool isSuccess = false;
                Database database = DatabaseFactory.CreateDatabase();
                XH_CapitalAccountTableDal dal = new XH_CapitalAccountTableDal();

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
        /// 获取所有资金内存表
        /// </summary>
        /// <returns></returns>
        public List<XH_CapitalAccountTableInfo> GetAllBaseTable()
        {
            XH_CapitalAccountTableDal dal = new XH_CapitalAccountTableDal();
            List<XH_CapitalAccountTableInfo> list = dal.GetAllListArray();

            return list;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="baseTable"></param>
        /// <returns></returns>
        public bool InsertBaseTable(XH_CapitalAccountTableInfo baseTable)
        {
            //不实现
            return true;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="baseTable"></param>
        public void DeleteBaseTable(XH_CapitalAccountTableInfo baseTable)
        {
            //不实现
        }

        /// <summary>
        /// 持久化变化量
        /// </summary>
        /// <param name="delta"></param>
        public void PersistChange(XH_CapitalAccountTable_DeltaInfo delta)
        {
            XH_CapitalAccountTable_DeltaInfoDal deltaInfoDal = new XH_CapitalAccountTable_DeltaInfoDal();
            deltaInfoDal.Add(delta);
        }

        /// <summary>
        /// 持久化资金表
        /// </summary>
        /// <param name="baseTable"></param>
        public void PersistBase(XH_CapitalAccountTableInfo baseTable)
        {
            //资金表不实现
        }

        /// <summary>
        /// 带事务持久化资金表
        /// </summary>
        /// <param name="baseTable"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        public void PersistBase(XH_CapitalAccountTableInfo baseTable, Database db, DbTransaction transaction)
        {
            //资金表不实现
        }

        /// <summary>
        /// 带事务插入
        /// </summary>
        /// <param name="baseTable"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        public void InsertBaseTableWithTransaction(XH_CapitalAccountTableInfo baseTable, Database db,
                                                   DbTransaction transaction)
        {
            //资金表不实现
        }

        /// <summary>
        /// 带事务更新
        /// </summary>
        /// <param name="delta"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        public void PersistChangeWithTransaction(XH_CapitalAccountTable_DeltaInfo delta, Database db,
                                                 DbTransaction transaction)
        {
            XH_CapitalAccountTable_DeltaInfoDal deltaInfoDal = new XH_CapitalAccountTable_DeltaInfoDal();
            deltaInfoDal.Add(delta, db, transaction);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="baseList"></param>
        public void Commit(List<XH_CapitalAccountTableInfo> baseList)
        {
            try
            {
                bool isSuccess = false;
                XH_CapitalAccountTableDal dal = new XH_CapitalAccountTableDal();

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
                    XH_CapitalAccountTable_DeltaInfoDal deltaInfoDal = new XH_CapitalAccountTable_DeltaInfoDal();
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
        /// 获取指定Id的资金表
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public XH_CapitalAccountTableInfo GetBaseTable(int k)
        {
            XH_CapitalAccountTableDal dal = new XH_CapitalAccountTableDal();
            var data = dal.GetModel(k);

            return data;
        }

        /// <summary>
        /// 清空资金变化
        /// </summary>
        /// <param name="database"></param>
        /// <param name="transaction"></param>
        private void CleanDeltaTable(Database database, DbTransaction transaction)
        {
            XH_CapitalAccountTable_DeltaInfoDal deltaInfoDal = new XH_CapitalAccountTable_DeltaInfoDal();
            deltaInfoDal.Delete(database, transaction);
        }
        /// <summary>
        /// 根据条件从数据库中获取相应的基础表数据
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public XH_CapitalAccountTableInfo GetBaseFromDBByWhere(string where)
        {
            try
            {

                XH_CapitalAccountTableDal dal = new XH_CapitalAccountTableDal();
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
                LogHelper.WriteError("根据条件从数据库中获取相应的现货资金基础表数据异常:" + where + ex.Message, ex);
                return null;
            }
        }
        #endregion

    }
}