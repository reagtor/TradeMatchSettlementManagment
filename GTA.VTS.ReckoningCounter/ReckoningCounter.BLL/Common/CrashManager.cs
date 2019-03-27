#region Using Namespace

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Timers;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.delegateoffer;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.DAL.MatchCenterOrderDealRpt;
using ReckoningCounter.Model;
using Timer=System.Timers.Timer;

#endregion

namespace ReckoningCounter.BLL.Common
{
    /// <summary>
    /// 故障恢复管理类
    /// 作者：宋涛
    /// 日期：2009-02-13
    /// </summary>
    public class CrashManager
    {
        private static CrashManager instance = new CrashManager();
        private string countID;

        private IList<string> hasSendSet = new List<string>();
        private ReaderWriterLockSlim hasSendSetLock = new ReaderWriterLockSlim();

        private IList<string> idList = new List<string>();
        private ReaderWriterLockSlim idListLock = new ReaderWriterLockSlim();

        private bool isChecking;
        private List<string> isReckoningSet = new List<string>();
        private ReaderWriterLockSlim isReckoningSetLock = new ReaderWriterLockSlim();

        private Timer timer;

        private CrashManager()
        {
            CheckUnReckonedDeal();
        }

        public string CountID
        {
            get
            {
                if (string.IsNullOrEmpty(countID))
                    countID = ServerConfig.CounterID;
                    //countID = ConfigurationManager.AppSettings[Utils.CounterID];

                if(countID.Trim().Length == 0)
                    countID = ServerConfig.CounterID;
                    //countID = ConfigurationManager.AppSettings[Utils.CounterID];

                if(countID.Trim().Length == 0)
                    countID = OrderOfferCenter.COUNT_CLIENT_ID;

                return countID;
            }
        }

        public static CrashManager GetInstance()
        {
            return instance;
        }

        private void SaveEntity(BD_UnReckonedDealTableInfo table)
        {
            //陈武民修改 修改时间 2009年07月09日
            table.CounterID = CountID;
            BD_UnReckonedDealTableDal dal = new BD_UnReckonedDealTableDal();
            //TransactionManager tm = TransactionFactory.GetTransactionManager();
            //tm.BeginTransaction();

            try
            {
                dal.Add(table);
               // DataRepository.BdUnReckonedDealTableProvider.Insert(tm, table);
                //tm.Commit();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("$-------$CrashManager.SaveEntity成交回报保存失败！", ex);
                //tm.Rollback();
            }
        }

        #region Delete

        public void DeleteEntity(string id)
        {
            if (string.IsNullOrEmpty(id))
                return;
            //陈武民修改 修改时间 2009年07月09日
           // TransactionManager tm = TransactionFactory.GetTransactionManager();
            //tm.BeginTransaction();

            try
            {
               // DataRepository.BdUnReckonedDealTableProvider.Delete(tm, id);
               // tm.Commit();
                BD_UnReckonedDealTableDal dal = new BD_UnReckonedDealTableDal();
                dal.Delete(id);
                LogHelper.WriteDebug("$-------$CrashManger.DeleteEntity成交回报删除成功！ID=" + id);

                //lock (((ICollection) hasSendSet).SyncRoot)
                //    hasSendSet.Remove(id);

                hasSendSetLock.EnterWriteLock();
                try
                {
                    hasSendSet.Remove(id);
                }
                finally
                {
                    hasSendSetLock.ExitWriteLock();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("$-------$CrashManager.SaveEntity成交回报删除失败！ID=" + id, ex);
                //tm.Rollback();
            }
        }

        public void DeleteEntity(string id, ReckoningTransaction trans)
        {
            if (string.IsNullOrEmpty(id))
                return;
            //陈武民修改 修改时间 2009年07月09日
            // TransactionManager tm = TransactionFactory.GetTransactionManager();
            //tm.BeginTransaction();

            try
            {
                // DataRepository.BdUnReckonedDealTableProvider.Delete(tm, id);
                // tm.Commit();
                BD_UnReckonedDealTableDal dal = new BD_UnReckonedDealTableDal();
                dal.Delete(id, trans.Database,trans.Transaction);
                LogHelper.WriteDebug("$-------$CrashManger.DeleteEntity成交回报删除成功！ID=" + id);

                //lock (((ICollection) hasSendSet).SyncRoot)
                //    hasSendSet.Remove(id);

                hasSendSetLock.EnterWriteLock();
                try
                {
                    hasSendSet.Remove(id);
                }
                finally
                {
                    hasSendSetLock.ExitWriteLock();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("$-------$CrashManager.SaveEntity成交回报删除失败！ID=" + id, ex);
                //tm.Rollback();
            }
        }

        public void DeleteEntityList(List<string> ids, ReckoningTransaction trans)
        {
            if (Utils.IsNullOrEmpty(ids))
                return;
            //陈武民修改 修改时间 2009年07月09日
            // TransactionManager tm = TransactionFactory.GetTransactionManager();
            //tm.BeginTransaction();

            try
            {
                // DataRepository.BdUnReckonedDealTableProvider.Delete(tm, id);
                // tm.Commit();
                BD_UnReckonedDealTableDal dal = new BD_UnReckonedDealTableDal();

                
                

                //lock (((ICollection) hasSendSet).SyncRoot)
                //    hasSendSet.Remove(id);

                hasSendSetLock.EnterWriteLock();
                try
                {
                    foreach (var id in ids)
                    {
                        dal.Delete(id, trans.Database, trans.Transaction);
                        hasSendSet.Remove(id);
                        LogHelper.WriteDebug("$-------$CrashManger.DeleteEntity成交回报删除成功！ID=" + id);
                    }
                }
                finally
                {
                    hasSendSetLock.ExitWriteLock();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("$-------$CrashManager.SaveEntity成交回报删除失败！",ex);
                //tm.Rollback();
            }
        }

        #endregion

        #region Check

        public void AddReckoningID(string id)
        {
            //lock (((ICollection) isReckoningSet).SyncRoot)
            //    isReckoningSet.Add(id);

            isReckoningSetLock.EnterWriteLock();
            try
            {
                isReckoningSet.Add(id);
            }
            finally
            {
                isReckoningSetLock.ExitWriteLock();
            }
        }

        public void AddReckoningIDList(List<string> ids)
        {
            isReckoningSetLock.EnterWriteLock();
            try
            {
                foreach (var id in ids)
                {
                    isReckoningSet.Add(id);
                }
            }
            finally
            {
                isReckoningSetLock.ExitWriteLock();
            }
        }

        public void RemoveReckoningID(string id)
        {
            //lock (((ICollection) isReckoningSet).SyncRoot)
            //    isReckoningSet.Remove(id);

            isReckoningSetLock.EnterWriteLock();
            try
            {
                isReckoningSet.Remove(id);
            }
            finally
            {
                isReckoningSetLock.ExitWriteLock();
            }
        }

        public void RemoveReckoningIDList(List<string> ids)
        {
            //lock (((ICollection) isReckoningSet).SyncRoot)
            //    isReckoningSet.Remove(id);

            isReckoningSetLock.EnterWriteLock();
            try
            {
                foreach (var id in ids)
                {
                    isReckoningSet.Remove(id);
                }
            }
            finally
            {
                isReckoningSetLock.ExitWriteLock();
            }
        }

        public bool IsReckoning(string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            //if (isReckoningSet.Contains(id))
            //    return true;

            bool result = false;
            isReckoningSetLock.EnterReadLock();
            try
            {
                if (isReckoningSet.Contains(id))
                    result = true;
            }
            finally
            {
                isReckoningSetLock.ExitReadLock();
            }

            return result;
        }

        private void CheckUnReckonedDeal()
        {
            timer = new Timer();
            this.timer.Interval = 3*60*1000;
            this.timer.Elapsed += timer_Elapsed;
            this.timer.Enabled = true;
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (isChecking)
                return;

            isChecking = true;
            //LogHelper.WriteDebug("CrashManager.CheckUnReckonedDeal");
            //陈武民修改 2009年7月9日
            string where = string.Format("CounterId = '{0}' ", CountID);
            BD_UnReckonedDealTableDal dal = new BD_UnReckonedDealTableDal();
            //var tables = DataRepository.BdUnReckonedDealTableProvider.Find(where);
            var tables = dal.GetListArray(where);
            int i = 0;
            foreach (BD_UnReckonedDealTableInfo table in tables)
            {
                if (IsReckoning(table.id))
                    continue;

                if (string.IsNullOrEmpty(table.CounterID))
                    continue;

                //if (!hasSendSet.Contains(table.Id))
                //{
                //    i++;
                //    ProcessTable(table);

                //    lock (((ICollection) hasSendSet).SyncRoot)
                //        hasSendSet.Add(table.Id);
                //}

                hasSendSetLock.EnterUpgradeableReadLock();
                try
                {
                    if (!hasSendSet.Contains(table.id))
                    {
                        i++;
                        ProcessTable(table);

                        hasSendSetLock.EnterWriteLock();
                        try
                        {
                            hasSendSet.Add(table.id);
                        }
                        finally
                        {
                            hasSendSetLock.ExitWriteLock();
                        }
                    }
                }
                finally
                {
                    hasSendSetLock.ExitUpgradeableReadLock();
                }
            }

            if (i > 0)
            {
                string format = "CrashManager.Timer进行故障恢复，当前数据库中未清算记录有{0}条,重新发送的记录有{1}条.";
                string desc = string.Format(format, tables.Count, i);
                LogHelper.WriteDebug(desc);
            }

            isChecking = false;
        }

        private void ProcessTable(BD_UnReckonedDealTableInfo table)
        {
            switch (table.EntityType)
            {
                case (int) DealBackEntityType.XH:
                    ProcessXHTable(table);
                    break;
                case (int)DealBackEntityType.HK:
                    ProcessHKTable(table);
                    break;
                case (int) DealBackEntityType.QH:
                    ProcessQHTable(table);
                    break;
                case (int) DealBackEntityType.GZXH:
                    ProcessGZQHTable(table);
                    break;
                case (int) DealBackEntityType.XHResult:
                case (int) DealBackEntityType.QHResult:
                case (int) DealBackEntityType.GZQHResult:
                case (int) DealBackEntityType.HKResult:
                    ProcessResultTable(table, table.EntityType);
                    break;
                case (int) DealBackEntityType.XHCancel:
                case (int) DealBackEntityType.QHCancel:
                case (int) DealBackEntityType.GZQHCancel:
                case (int) DealBackEntityType.HKCancel:
                    ProcessCancelTable(table, table.EntityType);
                    break;
            }
        }

        private void ProcessXHTable(BD_UnReckonedDealTableInfo table)
        {
            StockDealBackEntity entity = new StockDealBackEntity();
            entity.Id = table.id;
            entity.OrderNo = table.OrderNo;
            entity.DealPrice = table.Price.Value;
            entity.DealAmount = table.Amount.Value;
            entity.DealTime = table.Time.Value;

            LogHelper.WriteInfo("$-------$CrashManager.ProcessXHTable重新发送现货成交回报" + GetXHDesc(entity));
            ReckonCenter.Instace.AcceptXHDealOrder(entity);
        }

        private void ProcessHKTable(BD_UnReckonedDealTableInfo table)
        {
            HKDealBackEntity entity = new HKDealBackEntity();
            entity.ID = table.id;
            entity.OrderNo = table.OrderNo;
            entity.DealPrice = table.Price.Value;
            entity.DealAmount = table.Amount.Value;
            entity.DealTime = table.Time.Value;

            LogHelper.WriteInfo("$-------$CrashManager.ProcessHKTable重新发送现货成交回报" + GetHKDesc(entity));
            ReckonCenter.Instace.AcceptHKDealOrder(entity);
        }

        private void ProcessQHTable(BD_UnReckonedDealTableInfo table)
        {
            CommoditiesDealBackEntity entity = new CommoditiesDealBackEntity();
            entity.Id = table.id;
            entity.OrderNo = table.OrderNo;
            entity.DealPrice = table.Price.Value;
            entity.DealAmount = table.Amount.Value;
            entity.DealTime = table.Time.Value;

            LogHelper.WriteInfo("$-------$CrashManager.ProcessXHTable重新发送期货成交回报" + GetQHDesc(entity));

            ReckonCenter.Instace.AcceptSPQHDealOrder(entity);
        }

        private void ProcessGZQHTable(BD_UnReckonedDealTableInfo table)
        {
            FutureDealBackEntity entity = new FutureDealBackEntity();
            entity.Id = table.id;
            entity.OrderNo = table.OrderNo;
            entity.DealPrice = table.Price.Value;
            entity.DealAmount = table.Amount.Value;
            entity.DealTime = table.Time.Value;

            LogHelper.WriteInfo("$-------$CrashManager.ProcessXHTable重新发送股指期货成交回报" + GetGZQHDesc(entity));

            ReckonCenter.Instace.AcceptGZQHDealOrder(entity);
        }

        private void ProcessResultTable(BD_UnReckonedDealTableInfo table, int type)
        {
            ResultDataEntity entity = new ResultDataEntity();
            entity.Id = table.id;
            entity.OrderNo = table.OrderNo;
            entity.Message = table.Message;

            switch (type)
            {
                case (int) DealBackEntityType.XHResult:
                    LogHelper.WriteInfo("$-------$CrashManger.ProcessResultTable重新发送现货ResultDataEntity" +
                                         GetResultDesc(entity));
                    ReckonCenter.Instace.AcceptXHErrorOrderRpt(entity);
                    break;
                case (int)DealBackEntityType.HKResult:
                    LogHelper.WriteInfo("$-------$CrashManger.ProcessResultTable重新发送现货ResultDataEntity" +
                                         GetResultDesc(entity));
                    ReckonCenter.Instace.AcceptHKErrorOrderRpt(entity);
                    break;
                case (int) DealBackEntityType.QHResult:
                    LogHelper.WriteInfo("$-------$CrashManger.ProcessResultTable重新发送期货ResultDataEntity" +
                                         GetResultDesc(entity));

                   ReckonCenter.Instace.AcceptSPQHErrorOrderRpt(entity);
                    break;
                case (int) DealBackEntityType.GZQHResult:
                    LogHelper.WriteInfo("$-------$CrashManger.ProcessResultTable重新发送股指期货ResultDataEntity" +
                                         GetResultDesc(entity));

                    ReckonCenter.Instace.AcceptGZQHErrorOrderRpt(entity);
                    break;
            }
        }

        private void ProcessCancelTable(BD_UnReckonedDealTableInfo table, int type)
        {
            CancelOrderEntity entity = new CancelOrderEntity();
            entity.Id = table.id;
            entity.OrderNo = table.OrderNo;
            entity.OrderVolume = table.Amount.Value;
            entity.Message = table.Message;
            entity.IsSuccess = table.IsSuccess;

            switch (type)
            {
                case (int) DealBackEntityType.XHCancel:
                    LogHelper.WriteInfo("$-------$CrashManger.ProcessCancelTable重新发送现货CancelOrderEntity" +
                                         GetCancelDesc(entity));
                    ReckonCenter.Instace.AcceptCancelXHOrderRpt(entity);
                    break;
                case (int)DealBackEntityType.HKCancel:
                    LogHelper.WriteInfo("$-------$CrashManger.ProcessCancelTable重新发送现货CancelOrderEntity" +
                                         GetCancelDesc(entity));
                    ReckonCenter.Instace.AcceptCancelHKOrderRpt(entity);
                    break;
                case (int) DealBackEntityType.QHCancel:
                    LogHelper.WriteInfo("$-------$CrashManger.ProcessCancelTable重新发送期货CancelOrderEntity" +
                                         GetCancelDesc(entity));

                    ReckonCenter.Instace.AcceptCancelSPQHOrderRpt(entity);
                    break;
                case (int) DealBackEntityType.GZQHCancel:
                    LogHelper.WriteInfo("$-------$CrashManger.ProcessCancelTable重新发送股指期货CancelOrderEntity" +
                                         GetCancelDesc(entity));

                    ReckonCenter.Instace.AcceptCancelGZQHOrderRpt(entity);
                    break;
            }
        }

        #endregion

        #region 成交回报

        private void AddID(string id)
        {
            if (string.IsNullOrEmpty(id))
                return;

            //lock (((ICollection) idList).SyncRoot)
            //    idList.Add(id);

            idListLock.EnterWriteLock();
            try
            {
                idList.Add(id);
            }
            finally
            {
                idListLock.ExitWriteLock();
            }
        }

        private bool HasAddId(string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            bool result = false;

            idListLock.EnterReadLock();
            try
            {
                if (idList.Contains(id))
                {
                    LogHelper.WriteInfo("CrashManger.HasAddID从撮合返回的回报ID重复,之前已发送过相同ID的回报，ID=" + id);
                    result = true;
                }
            }
            finally
            {
                idListLock.ExitReadLock();
            }

            return result;
        }

        /// <summary>
        /// 保存现货成交回报
        /// </summary>
        /// <param name="entity">成交回报</param>
        public bool InsertXHDealBackEntity(StockDealBackEntity entity)
        {
            if (HasAddId(entity.Id))
            {
                LogHelper.WriteInfo("CrashManager.InsertXHDealBackEntity从撮合收到重复的ID=" + entity.Id);
                return false;
            }

            BD_UnReckonedDealTableInfo table = new BD_UnReckonedDealTableInfo();
            table.id = entity.Id;
            table.OrderNo = entity.OrderNo;
            table.Price = entity.DealPrice;
            table.Amount = (int) entity.DealAmount;
            table.Time = entity.DealTime;

            table.EntityType = (int) DealBackEntityType.XH;

            string desc = GetXHDesc(entity);
            LogHelper.WriteDebug("$-------$CrashManager.InsertStockDealBackEntity" + desc);

            SaveEntity(table);

            //entity.Id = table.Id.ToString();
            AddID(entity.Id);
            return true;
        }

        /// <summary>
        /// 保存港股成交回报
        /// </summary>
        /// <param name="entity">成交回报</param>
        public bool InsertHKDealBackEntity(HKDealBackEntity entity)
        {
            if (HasAddId(entity.ID))
            {
                LogHelper.WriteInfo("CrashManager.InsertHKDealBackEntity从撮合收到重复的ID=" + entity.ID);
                return false;
            }

            BD_UnReckonedDealTableInfo table = new BD_UnReckonedDealTableInfo();
            table.id = entity.ID;
            table.OrderNo = entity.OrderNo;
            table.Price = entity.DealPrice;
            table.Amount = (int)entity.DealAmount;
            table.Time = entity.DealTime;

            table.EntityType = (int)DealBackEntityType.XH;

            string desc = GetHKDesc(entity);
            LogHelper.WriteDebug("$-------$CrashManager.InsertHKDealBackEntity" + desc);

            SaveEntity(table);

            //entity.Id = table.Id.ToString();
            AddID(entity.ID);
            return true;
        }

        public string GetXHDesc(StockDealBackEntity entity)
        {
            string format = "现货成交回报[OrderNo={0},DealPrice={1},DealAmount={2},DealTime={3}, ID={4}, CounterID={5}]";
            return string.Format(format, entity.OrderNo, entity.DealPrice, entity.DealAmount, entity.DealTime, entity.Id,
                                 CountID);
        }

        public string GetHKDesc(HKDealBackEntity entity)
        {
            string format = "港股成交回报[OrderNo={0},DealPrice={1},DealAmount={2},DealTime={3}, ID={4}, CounterID={5}]";
            return string.Format(format, entity.OrderNo, entity.DealPrice, entity.DealAmount, entity.DealTime, entity.ID,
                                 CountID);
        }

        /// <summary>
        /// 保存期货成交回报
        /// </summary>
        /// <param name="entity">成交回报</param>
        public bool InsertQHDealBackEntity(CommoditiesDealBackEntity entity)
        {
            if (HasAddId(entity.Id))
                return false;

            BD_UnReckonedDealTableInfo table = new BD_UnReckonedDealTableInfo();
            table.id = entity.Id;
            table.OrderNo = entity.OrderNo;
            table.Price = entity.DealPrice;
            table.Amount = (int) entity.DealAmount;
            table.Time = entity.DealTime;

            table.EntityType = (int) DealBackEntityType.QH;

            string desc = GetQHDesc(entity);
            LogHelper.WriteDebug("$-------$CrashManager.InsertQHDealBackEntity" + desc);

            SaveEntity(table);

            //entity.Id = table.Id.ToString();
            AddID(entity.Id);
            return true;
        }

        public string GetQHDesc(CommoditiesDealBackEntity entity)
        {
            string format = "期货成交回报[OrderNo={0},DealPrice={1},DealAmount={2},DealTime={3}, ID={4}, CounterID={5}]";
            return string.Format(format, entity.OrderNo, entity.DealPrice, entity.DealAmount, entity.DealTime, entity.Id,
                                 CountID);
        }

        /// <summary>
        /// 保存股指期货成交回报
        /// </summary>
        /// <param name="entity">成交回报</param>
        public bool InsertGZQHDealBackEntity(FutureDealBackEntity entity)
        {
            if (HasAddId(entity.Id))
                return false;

            BD_UnReckonedDealTableInfo table = new BD_UnReckonedDealTableInfo();
            table.id = entity.Id;
            table.OrderNo = entity.OrderNo;
            table.Price = entity.DealPrice;
            table.Amount = (int) entity.DealAmount;
            table.Time = entity.DealTime;

            table.EntityType = (int) DealBackEntityType.GZXH;

            string desc = GetGZQHDesc(entity);
            LogHelper.WriteDebug("$-------$CrashManager.InsertGZQHDealBackEntity" + desc);

            SaveEntity(table);

            //entity.Id = table.Id.ToString();
            AddID(entity.Id);
            return true;
        }

        public string GetGZQHDesc(FutureDealBackEntity entity)
        {
            string format = "股指期货成交回报[OrderNo={0},DealPrice={1},DealAmount={2},DealTime={3}, ID={4}, CounterID={5}]";
            return string.Format(format, entity.OrderNo, entity.DealPrice, entity.DealAmount, entity.DealTime, entity.Id,
                                 CountID);
        }

        #endregion

        #region ResultDataEntity

        public bool InsertXHResultDealBackEntity(ResultDataEntity entity)
        {
            return InsertResultDealBackEntity(entity, DealBackEntityType.XHResult);
        }

        public bool InsertHKResultDealBackEntity(ResultDataEntity entity)
        {
            return InsertResultDealBackEntity(entity, DealBackEntityType.HKResult);
        }

        public bool InsertQHResultDealBackEntity(ResultDataEntity entity)
        {
            return InsertResultDealBackEntity(entity, DealBackEntityType.QHResult);
        }

        public bool InsertGZQHResultDealBackEntity(ResultDataEntity entity)
        {
            return InsertResultDealBackEntity(entity, DealBackEntityType.GZQHResult);
        }

        /// <summary>
        /// 保存错误成交回报
        /// </summary>
        /// <param name="entity">成交回报</param>
        /// <param name="type"></param>
        private bool InsertResultDealBackEntity(ResultDataEntity entity, DealBackEntityType type)
        {
            if (HasAddId(entity.Id))
                return false;

            BD_UnReckonedDealTableInfo table = new BD_UnReckonedDealTableInfo();
            table.id = entity.Id;
            table.OrderNo = entity.OrderNo;
            table.Message = entity.Message;
            //table.Price = entity.DealPrice;
            //table.Amount = (int)entity.DealAmount;
            //table.Time = entity.DealTime;

            table.EntityType = (int) type;

            string desc = GetResultDesc(entity);
            LogHelper.WriteDebug("$-------$CrashManager.InsertResultDealBackEntity" + desc);

            SaveEntity(table);

            //entity.Id = table.Id.ToString();
            AddID(entity.Id);
            return true;
        }

        public string GetResultDesc(ResultDataEntity entity)
        {
            string format = "错误成交回报[OrderNo={0},Message={1}, ID={2}, CounterID={3}]";
            return string.Format(format, entity.OrderNo, entity.Message, entity.Id, CountID);
        }

        #endregion

        #region CancelOrderEntity

        public bool InsertXhCancelDealBackEntity(CancelOrderEntity entity)
        {
            return InsertCancelDealBackEntity(entity, DealBackEntityType.XHCancel);
        }

        public bool InsertHkCancelDealBackEntity(CancelOrderEntity entity)
        {
            return InsertCancelDealBackEntity(entity, DealBackEntityType.HKCancel);
        }

        public bool InsertQhCancelDealBackEntity(CancelOrderEntity entity)
        {
            return InsertCancelDealBackEntity(entity, DealBackEntityType.QHCancel);
        }

        public bool InsertGZQHCancelDealBackEntity(CancelOrderEntity entity)
        {
            return InsertCancelDealBackEntity(entity, DealBackEntityType.GZQHCancel);
        }

        /// <summary>
        /// 保存撤单回报
        /// </summary>
        /// <param name="entity">撤单回报</param>
        private bool InsertCancelDealBackEntity(CancelOrderEntity entity, DealBackEntityType type)
        {
            if (HasAddId(entity.Id))
                return false;

            BD_UnReckonedDealTableInfo table = new BD_UnReckonedDealTableInfo();
            table.id = entity.Id;
            table.OrderNo = entity.OrderNo;
            table.Message = entity.Message;
            //table.Price = entity.DealPrice;
            table.Amount = (int) entity.OrderVolume;
            table.IsSuccess = entity.IsSuccess;

            table.EntityType = (int) type;

            string desc = GetCancelDesc(entity);
            LogHelper.WriteDebug("$-------$CrashManager.InsertCancelDealBackEntity" + desc);

            SaveEntity(table);

            //entity.Id = table.Id.ToString();
            AddID(entity.Id);
            return true;
        }

        public string GetCancelDesc(CancelOrderEntity entity)
        {
            string format = "撤单回报[OrderNo={0},OrderVolume={1},Message={2},IsSuccess={3}, ID={4}, CounterID={5}]";
            return string.Format(format, entity.OrderNo, entity.OrderVolume, entity.Message, entity.IsSuccess, entity.Id,
                                 CountID);
        }

        #endregion
    }

    /// <summary>
    /// 撮合回报类型
    /// </summary>
    public enum DealBackEntityType
    {
        /// <summary>
        /// StockDealBackEntity
        /// </summary>
        XH = 1,

        /// <summary>
        /// CommoditiesDealBackEntity
        /// </summary>
        QH = 2,

        /// <summary>
        /// FutureDealBackEntity
        /// </summary>
        GZXH = 3,

        /// <summary>
        /// XHResultDataEntity
        /// </summary>
        XHResult = 4,

        /// <summary>
        /// QHResultDataEntity
        /// </summary>
        QHResult = 5,

        /// <summary>
        /// GZQHResultDataEntity
        /// </summary>
        GZQHResult = 6,

        /// <summary>
        /// XHCancelOrderEntity
        /// </summary>
        XHCancel = 7,

        /// <summary>
        /// QHCancelOrderEntity
        /// </summary>
        QHCancel = 8,

        /// <summary>
        /// GZQHCancelOrderEntity
        /// </summary>
        GZQHCancel = 9,

        HK = 10,

        HKResult = 11,

        HKCancel = 12,

        HKModify = 13

    }
}