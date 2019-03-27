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

namespace ReckoningCounter.MemoryData.QH.Capital
{
    /// <summary>
    /// 期货内存资金表数据库持久化器
    /// 作者：宋涛
    /// </summary>
    public class QHCapitalMemoryDBPersister :
        IMemoryPersister<int, QH_CapitalAccountTableInfo, QH_CapitalAccountTable_DeltaInfo>
    {
        #region Implementation of IMemoryPersister<QHCapitalMemoryTable,QH_CapitalAccountTable_DeltaInfo>

        /// <summary>
        /// 同步数据库
        /// </summary>
        public void SyncChangeToBase()
        {
            QH_CapitalAccountTable_DeltaDal deltaInfoDal = new QH_CapitalAccountTable_DeltaDal();
            var deltaList = deltaInfoDal.GetAllSum();

            if (deltaList == null)
                return;

            if (deltaList.Count == 0)
                return;

            try
            {
                bool isSuccess = false;
                QH_CapitalAccountTableDal dal = new QH_CapitalAccountTableDal();

                Database database = DatabaseFactory.CreateDatabase();
                LogHelper.WriteDebug("期货内存资金表数据库故障恢复开始==" + deltaList.Count);
                using (DbConnection connection = database.CreateConnection())
                {
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        foreach (var deltaInfo in deltaList)
                        {
                            var data = new QH_CapitalAccountTableInfo();
                            data.AvailableCapital = deltaInfo.AvailableCapitalDelta;
                            data.CapitalAccountLogoId = deltaInfo.CapitalAccountLogoId;
                            data.CloseFloatProfitLossTotal = deltaInfo.CloseFloatProfitLossTotalDelta;
                            data.CloseMarketProfitLossTotal = deltaInfo.CloseMarketProfitLossTotalDelta;
                            data.FreezeCapitalTotal = deltaInfo.FreezeCapitalTotalDelta;
                            data.MarginTotal = deltaInfo.MarginTotalDelta;
                            data.TodayOutInCapital = deltaInfo.TodayOutInCapitalDelta;

                            dal.AddUpdate(data, database, transaction);
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
                LogHelper.WriteDebug("期货内存资金表数据库故障恢复完成==");
                //同步后清除增量表数据
                if (isSuccess)
                {
                    QH_CapitalAccountTable_DeltaDal deltaDal = new QH_CapitalAccountTable_DeltaDal();
                    deltaDal.Truncate();
                }

                /*DataManager.ExecuteInTransaction((database, transaction) =>
                                                     {
                                                         foreach (var deltaInfo in deltaList)
                                                         {
                                                             var data = new QH_CapitalAccountTableInfo();
                                                             data.AvailableCapital = deltaInfo.AvailableCapitalDelta;
                                                             data.CloseFloatProfitLossTotal =
                                                                 deltaInfo.CloseFloatProfitLossTotalDelta;
                                                             data.CloseMarketProfitLossTotal =
                                                                 deltaInfo.CloseMarketProfitLossTotalDelta;
                                                             data.FreezeCapitalTotal = deltaInfo.FreezeCapitalTotalDelta;
                                                             data.MarginTotal = deltaInfo.MarginTotalDelta;
                                                             data.TodayOutInCapital = deltaInfo.TodayOutInCapitalDelta;

                                                             dal.AddUpdate(data, database, transaction);
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
        public List<QH_CapitalAccountTableInfo> GetAllBaseTable()
        {
            QH_CapitalAccountTableDal dal = new QH_CapitalAccountTableDal();
            List<QH_CapitalAccountTableInfo> list = dal.GetAllListArray();

            return list;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="baseTable"></param>
        /// <returns></returns>
        public bool InsertBaseTable(QH_CapitalAccountTableInfo baseTable)
        {
            //不实现
            return true;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="baseTable"></param>
        public void DeleteBaseTable(QH_CapitalAccountTableInfo baseTable)
        {
            //不实现
        }

        /// <summary>
        /// 持久化变化量
        /// </summary>
        /// <param name="delta"></param>
        public void PersistChange(QH_CapitalAccountTable_DeltaInfo delta)
        {
            QH_CapitalAccountTable_DeltaDal deltaInfoDal = new QH_CapitalAccountTable_DeltaDal();
            deltaInfoDal.Add(delta);
        }

        /// <summary>
        /// 持久化资金表
        /// </summary>
        /// <param name="baseTable"></param>
        public void PersistBase(QH_CapitalAccountTableInfo baseTable)
        {
            //资金表不实现
        }

        /// <summary>
        /// 带事务持久化资金表
        /// </summary>
        /// <param name="baseTable"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        public void PersistBase(QH_CapitalAccountTableInfo baseTable, Database db, DbTransaction transaction)
        {
            //资金表不实现
        }

        /// <summary>
        /// 带事务插入
        /// </summary>
        /// <param name="baseTable"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        public void InsertBaseTableWithTransaction(QH_CapitalAccountTableInfo baseTable, Database db, DbTransaction transaction)
        {
            //资金表不实现
        }

        /// <summary>
        /// 带事务更新
        /// </summary>
        /// <param name="delta"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        public void PersistChangeWithTransaction(QH_CapitalAccountTable_DeltaInfo delta, Database db, DbTransaction transaction)
        {
            QH_CapitalAccountTable_DeltaDal deltaInfoDal = new QH_CapitalAccountTable_DeltaDal();
            deltaInfoDal.Add(delta, db, transaction); ;
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="baseList"></param>
        public void Commit(List<QH_CapitalAccountTableInfo> baseList)
        {
            try
            {
                bool isSuccess = false;
                QH_CapitalAccountTableDal dal = new QH_CapitalAccountTableDal();

                Database database = DatabaseFactory.CreateDatabase();

                using (DbConnection connection = database.CreateConnection())
                {
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        foreach (var data in baseList)
                        {
                            dal.Update(data, database, transaction);
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
                    QH_CapitalAccountTable_DeltaDal deltaDal = new QH_CapitalAccountTable_DeltaDal();
                    deltaDal.Truncate();
                }

                /*DataManager.ExecuteInTransaction((database, transaction) =>
                                                     {
                                                         foreach (var data in baseList)
                                                         {
                                                             dal.Update(data, database, transaction);
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
        public QH_CapitalAccountTableInfo GetBaseTable(int k)
        {
            QH_CapitalAccountTableDal dal = new QH_CapitalAccountTableDal();
            var data = dal.GetModel(k);

            return data;
        }

        #endregion

        /// <summary>
        /// 清空变化量
        /// </summary>
        /// <param name="database"></param>
        /// <param name="transaction"></param>
        private void CleanDeltaTable(Database database, DbTransaction transaction)
        {
            QH_CapitalAccountTable_DeltaDal deltaInfoDal = new QH_CapitalAccountTable_DeltaDal();
            deltaInfoDal.Delete(database, transaction);
        }
        /// <summary>
        /// 根据条件从数据库中获取相应的基础表数据
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public QH_CapitalAccountTableInfo GetBaseFromDBByWhere(string where)
        {
            try
            {
                QH_CapitalAccountTableDal dal = new QH_CapitalAccountTableDal();
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
                LogHelper.WriteError("根据条件从数据库中获取相应的期货资金基础表数据异常:" + where + ex.Message, ex);
                return null;
            }

        }

    }
}