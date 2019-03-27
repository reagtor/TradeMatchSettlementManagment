using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatchCenter.BLL.MatchRules;
using System.Threading;
using GTA.VTS.Common.CommonObject;
using MatchCenter.Entity;
using MatchCenter.Entity.HK;
using GTA.VTS.Common.CommonUtility;

namespace MatchCenter.BLL.MatchData
{
    ///// <summary>
    ///// Title:接收到要撮合成交的委托单缓存数据类
    ///// Create By:李健华
    ///// Crate Date:2009-09-16
    ///// </summary>
    //// public class EntrustOrderData<T>:IComparer<T> where T : new()
    //public class EntrustOrderData<T> where T : new()
    //{
    //    #region 定义
    //    /// <summary>
    //    /// 接收到要撮合成交的委托单缓存列表
    //    /// </summary>
    //    private List<T> entrustOrderList;

    //    /// <summary>
    //    /// 委托单的交易方法
    //    /// </summary>
    //    private Types.TransactionDirection transactionDirection = Types.TransactionDirection.Buying;
    //    /// <summary>
    //    /// 数据排序功能实现类
    //    /// </summary>
    //    private EntrustOrderDataSort<T> sortList = new EntrustOrderDataSort<T>();
    //    /// <summary>
    //    /// 读写锁
    //    /// 用于管理资源访问的锁定状态，可实现多线程读取或进行独占式写入访问。
    //    /// </summary>
    //    private ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();
    //    #endregion

    //    /// <summary>
    //    /// 构造函数
    //    /// </summary>
    //    /// <param name="direction"></param>
    //    public EntrustOrderData(Types.TransactionDirection direction)
    //    {
    //        transactionDirection = direction;
    //        entrustOrderList = new List<T>();
    //    }

    //    #region 添加、删除操作
    //    /// <summary>
    //    /// 撮合中心清除数据
    //    /// </summary>
    //    public void Clear()
    //    {
    //        rwLock.EnterWriteLock();
    //        try
    //        {
    //            if (entrustOrderList != null)
    //            {
    //                entrustOrderList.Clear();
    //            }
    //        }
    //        finally
    //        {
    //            rwLock.ExitWriteLock();
    //        }
    //    }

    //    /// <summary>
    //    /// 添加委托到数据存储区
    //    /// </summary>
    //    /// <param name="dataX">委托单</param>
    //    public void Add(T dataX)
    //    {
    //        if (entrustOrderList == null)
    //        {
    //            return;
    //        }
    //        rwLock.EnterWriteLock();
    //        try
    //        {
    //            entrustOrderList.Add(dataX);
    //            //entrustOrderList.Sort(Compare);
    //            if (transactionDirection == Types.TransactionDirection.Buying)
    //            {
    //                entrustOrderList.Sort(sortList.CompareSortBuy);
    //            }
    //            else if (transactionDirection == Types.TransactionDirection.Selling)
    //            {
    //                entrustOrderList.Sort(sortList.CompareSortSell);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.WriteError("添加委托数据存储区时异常", ex);
    //        }
    //        finally
    //        {
    //            rwLock.ExitWriteLock();
    //        }
    //    }

    //    /// <summary>
    //    /// 删除撮合委托实体
    //    /// </summary>
    //    /// <param name="dataX">委托</param>
    //    public bool Remove(T dataX)
    //    {

    //        if (entrustOrderList == null)
    //        {
    //            return false;
    //        }
    //        rwLock.EnterWriteLock();
    //        try
    //        {
    //            return entrustOrderList.Remove(dataX);
    //        }
    //        finally
    //        {
    //            rwLock.ExitWriteLock();
    //        }

    //    }

    //    /// <summary>
    //    /// 删除撮合实体
    //    /// </summary>
    //    /// <param name="orderNo">委托编号</param>
    //    /// <param name="cancount"></param>
    //    /// <returns></returns>
    //    public bool Remove(string orderNo, ref decimal cancount)
    //    {
    //        if (entrustOrderList == null)
    //        {
    //            return false;
    //        }

    //        rwLock.EnterWriteLock();
    //        try
    //        {
    //            if (typeof(T) == typeof(FutureDataOrderEntity))
    //            {
    //                #region  期货删除
    //                foreach (T entity in entrustOrderList)
    //                {
    //                    FutureDataOrderEntity item = entity as FutureDataOrderEntity;
    //                    if (item.OrderNo == orderNo)
    //                    {
    //                        cancount = item.OrderVolume;
    //                        return entrustOrderList.Remove(entity);
    //                    }
    //                }
    //                #endregion
    //            }
    //            else if (typeof(T) == typeof(StockDataOrderEntity))
    //            {
    //                #region  现货删除
    //                foreach (T entity in entrustOrderList)
    //                {
    //                    StockDataOrderEntity item = entity as StockDataOrderEntity;
    //                    if (item.OrderNo == orderNo)
    //                    {
    //                        cancount = item.OrderVolume;
    //                        return entrustOrderList.Remove(entity);
    //                    }
    //                }
    //                #endregion
    //            }
    //            else if (typeof(T) == typeof(HKEntrustOrderInfo))
    //            {
    //                #region  港股股删除
    //                foreach (T entity in entrustOrderList)
    //                {
    //                    HKEntrustOrderInfo item = entity as HKEntrustOrderInfo;
    //                    if (item.OrderNo == orderNo)
    //                    {
    //                        cancount = item.OrderVolume;
    //                        return entrustOrderList.Remove(entity);
    //                    }
    //                }
    //                #endregion
    //            }
    //        }
    //        finally
    //        {
    //            rwLock.ExitWriteLock();
    //        }

    //        return false;
    //    }

    //    #region 暂时不用
    //    ///// <summary>
    //    ///// 根据委托单号得到委托数量
    //    ///// </summary>
    //    ///// <param name="orderNo"></param>
    //    ///// <param name="orderCount"></param>
    //    ///// <returns></returns>
    //    //public bool Select(string orderNo, ref decimal orderCount)
    //    //{
    //    //    if (entrustOrderList == null)
    //    //    {
    //    //        return false;
    //    //    }

    //    //    rwLock.EnterWriteLock();
    //    //    try
    //    //    {
    //    //        if (typeof(T) == typeof(FutureDataOrderEntity))
    //    //        {
    //    //            #region  期货
    //    //            foreach (T entity in entrustOrderList)
    //    //            {
    //    //                FutureDataOrderEntity item = entity as FutureDataOrderEntity;
    //    //                if (item.OrderNo == orderNo)
    //    //                {
    //    //                    orderCount = item.OrderVolume;
    //    //                    return true;
    //    //                }
    //    //            }
    //    //            #endregion
    //    //        }
    //    //        else if (typeof(T) == typeof(StockDataOrderEntity))
    //    //        {
    //    //            #region  现货
    //    //            foreach (T entity in entrustOrderList)
    //    //            {
    //    //                StockDataOrderEntity item = entity as StockDataOrderEntity;
    //    //                if (item.OrderNo == orderNo)
    //    //                {
    //    //                    orderCount = item.OrderVolume;
    //    //                    return true;
    //    //                }
    //    //            }
    //    //            #endregion
    //    //        }
    //    //        else if (typeof(T) == typeof(HKEntrustOrderInfo))
    //    //        {
    //    //            #region  港股股
    //    //            foreach (T entity in entrustOrderList)
    //    //            {
    //    //                HKEntrustOrderInfo item = entity as HKEntrustOrderInfo;
    //    //                if (item.OrderNo == orderNo)
    //    //                {
    //    //                    orderCount = item.OrderVolume;
    //    //                    return true;
    //    //                }
    //    //            }
    //    //            #endregion
    //    //        }


    //    //    }
    //    //    finally
    //    //    {
    //    //        rwLock.ExitWriteLock();
    //    //    }
    //    //    return false;

    //    //}
    //    #endregion

    //    /// <summary>
    //    /// 修改委托单中委托数量（撮合中只处理减量委托）
    //    /// 返回0表示改单失败包含实体为空和查询不到委托单等--不回报
    //    /// 返回1表示查询到委托单，但不附合减量改单规则 --回报
    //    /// 返回2表示查改单成功--回报
    //    /// </summary>
    //    /// <param name="orderNo">要改单的原委托单编号</param>
    //    /// <param name="modifynumer">改单委托量</param>
    //    /// <param name="nowOrderVolue">改单后当前委托量</param>
    //    /// <param name="canleCount">撤单量</param>
    //    /// <param name="errMsg">异常信息</param>
    //    /// <returns>返回0，1，2中其一</returns>
    //    public int Modify(string orderNo, decimal modifynumer, ref decimal nowOrderVolue, ref decimal canleCount, ref string errMsg)
    //    {

    //        if (entrustOrderList == null)
    //        {
    //            return 0;
    //        }
    //        rwLock.EnterWriteLock();

    //        errMsg = "";
    //        canleCount = 0;
    //        nowOrderVolue = 0;
    //        try
    //        {
    //            if (typeof(T) != typeof(HKEntrustOrderInfo))
    //            {
    //                return 0;
    //            }

    //            foreach (T entity in entrustOrderList)
    //            {
    //                HKEntrustOrderInfo item = entity as HKEntrustOrderInfo;
    //                if (item.OrderNo == orderNo)
    //                {
    //                    if (modifynumer >= item.OldVolume)
    //                    {
    //                        errMsg = "CH-0001:减量委托改单失败，改单量不可以大于或者等于原委托量";
    //                        canleCount = 0;
    //                        return 1;
    //                    }
    //                    //计算当前已经成交量=原委托量-当前剩余量;
    //                    decimal tradeVolume = item.OldVolume - item.OrderVolume;
    //                    //计算当前还可以委托量多少=当前改单委托量-当前已经成交量
    //                    decimal nowEntrusVolume = modifynumer - tradeVolume;
    //                    if (nowEntrusVolume <= 0)
    //                    {
    //                        errMsg = "CH-0001:减量委托改单失败，当前委托单的成交量已经大于或者等于改单委托量";
    //                        canleCount = 0;
    //                        return 1;
    //                    }
    //                    //当前撤单量=当前剩余量-当前还可以委托量
    //                    canleCount = item.OrderVolume - nowEntrusVolume;
    //                    nowOrderVolue = nowEntrusVolume;
    //                    item.OldVolume = modifynumer;
    //                    item.OrderVolume = nowEntrusVolume;
    //                    errMsg = "改单成功";
    //                    return 2;
    //                }
    //            }
    //        }
    //        finally
    //        {
    //            rwLock.ExitWriteLock();
    //        }
    //        return 0;
    //    }

    //    #endregion

    //    #region 返回获取所有委托
    //    /// <summary>
    //    /// 返回接收所有委托单实体 并重建委托缓存区
    //    /// </summary>
    //    /// <returns></returns>
    //    public List<T> GetAcceptEntitys()
    //    {
    //        List<T> matchOrderlist;
    //        rwLock.EnterReadLock();
    //        try
    //        {
    //            matchOrderlist = entrustOrderList;
    //            entrustOrderList = new List<T>();
    //        }
    //        finally
    //        {
    //            rwLock.ExitReadLock();
    //        }
    //        return matchOrderlist;
    //    }
    //    #endregion

    //    #region IComparer<T> 成员 废弃不用，这样写也可以
    //    ///// <summary>
    //    ///// 排序委托单数据
    //    ///// </summary>
    //    ///// <param name="dataX"></param>
    //    ///// <param name="dataY"></param>
    //    ///// <returns></returns>
    //    //public int Compare(T dataX, T dataY)
    //    //{
    //    //    if (transactionDirection == Types.TransactionDirection.Buying)
    //    //    {
    //    //        if (typeof(T) == typeof(FutureDataOrderEntity))
    //    //        {
    //    //            #region 期货比较排序
    //    //            FutureDataOrderEntity itemX = dataX as FutureDataOrderEntity;
    //    //            FutureDataOrderEntity itemY = dataY as FutureDataOrderEntity;
    //    //            if (itemX.OrderPrice > itemY.OrderPrice)
    //    //            {
    //    //                return 1;
    //    //            }
    //    //            else if (itemX.OrderPrice == itemY.OrderPrice)
    //    //            {
    //    //                if (itemX.ReachTime >= itemY.ReachTime)
    //    //                {
    //    //                    return 1;
    //    //                }
    //    //                else
    //    //                {
    //    //                    return -1;
    //    //                }
    //    //            }
    //    //            else
    //    //            {
    //    //                return -1;

    //    //            }
    //    //            #endregion
    //    //        }
    //    //        else if (typeof(T) == typeof(StockDataOrderEntity))
    //    //        {
    //    //            #region 现货比较排序
    //    //            StockDataOrderEntity itemX = dataX as StockDataOrderEntity;
    //    //            StockDataOrderEntity itemY = dataY as StockDataOrderEntity;
    //    //            if (itemX.OrderPrice > itemY.OrderPrice)
    //    //            {
    //    //                return 1;
    //    //            }
    //    //            else if (itemX.OrderPrice == itemY.OrderPrice)
    //    //            {
    //    //                if (itemX.ReachTime >= itemY.ReachTime)
    //    //                {
    //    //                    return 1;
    //    //                }
    //    //                else
    //    //                {
    //    //                    return -1;
    //    //                }
    //    //            }
    //    //            else
    //    //            {
    //    //                return -1;

    //    //            }
    //    //            #endregion
    //    //        }
    //    //        else
    //    //        {
    //    //            return -1;
    //    //        }
    //    //    }
    //    //    else if (transactionDirection == Types.TransactionDirection.Selling)
    //    //    {

    //    //        if (typeof(T) == typeof(FutureDataOrderEntity))
    //    //        {
    //    //            #region 期货比较排序
    //    //            FutureDataOrderEntity itemX = dataX as FutureDataOrderEntity;
    //    //            FutureDataOrderEntity itemY = dataY as FutureDataOrderEntity;
    //    //            if (itemX.OrderPrice < itemY.OrderPrice)
    //    //            {
    //    //                return 1;
    //    //            }
    //    //            else if (itemX.OrderPrice == itemY.OrderPrice)
    //    //            {
    //    //                if (itemX.ReachTime <= itemY.ReachTime)
    //    //                {
    //    //                    return 1;
    //    //                }
    //    //                else
    //    //                {
    //    //                    return -1;
    //    //                }
    //    //            }
    //    //            else
    //    //            {
    //    //                return -1;
    //    //            }
    //    //            #endregion
    //    //        }
    //    //        else if (typeof(T) == typeof(StockDataOrderEntity))
    //    //        {
    //    //            #region 现货比较排序
    //    //            StockDataOrderEntity itemX = dataX as StockDataOrderEntity;
    //    //            StockDataOrderEntity itemY = dataY as StockDataOrderEntity;
    //    //            if (itemX.OrderPrice < itemY.OrderPrice)
    //    //            {
    //    //                return 1;
    //    //            }
    //    //            else if (itemX.OrderPrice == itemY.OrderPrice)
    //    //            {
    //    //                if (itemX.ReachTime <= itemY.ReachTime)
    //    //                {
    //    //                    return 1;
    //    //                }
    //    //                else
    //    //                {
    //    //                    return -1;
    //    //                }
    //    //            }
    //    //            else
    //    //            {
    //    //                return -1;
    //    //            }
    //    //            #endregion
    //    //        }
    //    //        else
    //    //        {
    //    //            return -1;
    //    //        }
    //    //    }
    //    //    return -1;
    //    //}

    //    #endregion
    //}


    #region 代用修改 另一版本
    /// <summary>
    /// Title:接收到要撮合成交的委托单缓存数据类
    /// Create By:李健华
    /// Crate Date:2009-09-16
    /// Modifty By:李健华
    /// Modify Date:2010-01-08
    /// Desc.:修改缓存列表的方式，为了提高后面获取撮合时的性能，不再遍历重组数据,队列的排序方式是按需求是以每个人排队
    /// Desc.: 增加了商品期货相关内容
    /// Update By:董鹏
    /// Update Date:2010-01-22
    /// </summary>
    public class EntrustOrderData<T> where T : new()
    {
        #region 定义
        /// <summary>
        /// 接收到要撮合成交的委托单缓存列表以股东代码即交易员Code主主键
        /// </summary>
        private Dictionary<string, List<T>> entrustOrderList;

        /// <summary>
        /// 委托单的交易方法
        /// </summary>
        private Types.TransactionDirection transactionDirection = Types.TransactionDirection.Buying;
        /// <summary>
        /// 数据排序功能实现类
        /// </summary>
        private EntrustOrderDataSort<T> sortList = new EntrustOrderDataSort<T>();
        /// <summary>
        /// 读写锁
        /// 用于管理资源访问的锁定状态，可实现多线程读取或进行独占式写入访问。
        /// </summary>
        private ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="direction"></param>
        public EntrustOrderData(Types.TransactionDirection direction)
        {
            transactionDirection = direction;
            entrustOrderList = new Dictionary<string, List<T>>();
        }

        #region 添加、删除操作
        /// <summary>
        /// 撮合中心清除数据
        /// </summary>
        public void Clear()
        {
            rwLock.EnterWriteLock();
            try
            {
                if (entrustOrderList != null)
                {
                    entrustOrderList.Clear();
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 添加委托到数据存储区
        /// </summary>
        /// <param name="dataX">委托单</param>
        public void Add(T dataX)
        {
            if (entrustOrderList == null)
            {
                return;
            }
            rwLock.EnterWriteLock();
            try
            {
                string sholderCode = "";
                if (typeof(T) == typeof(FutureDataOrderEntity))
                {
                    #region  股指期货添加
                    FutureDataOrderEntity qhitem = dataX as FutureDataOrderEntity;
                    sholderCode = qhitem.SholderCode.Trim();
                    #endregion
                }
                else if (typeof(T) == typeof(StockDataOrderEntity))
                {
                    #region  现货添加
                    StockDataOrderEntity xhitem = dataX as StockDataOrderEntity;
                    sholderCode = xhitem.SholderCode.Trim();
                    #endregion
                }
                else if (typeof(T) == typeof(HKEntrustOrderInfo))
                {
                    #region  港股添加
                    HKEntrustOrderInfo hkitem = dataX as HKEntrustOrderInfo;
                    sholderCode = hkitem.SholderCode.Trim();
                    #endregion
                }
                else if (typeof(T) == typeof(CommoditiesDataOrderEntity))
                {
                    #region  商品期货添加 add by 董鹏 2010-01-22
                    CommoditiesDataOrderEntity qhitem = dataX as CommoditiesDataOrderEntity;
                    sholderCode = qhitem.SholderCode.Trim();
                    #endregion
                }

                #region 添加保存队列处理
                if (entrustOrderList.ContainsKey(sholderCode.Trim()))
                {
                    List<T> queryList = entrustOrderList[sholderCode.Trim()];
                    if (queryList != null)
                    {
                        queryList.Add(dataX);
                        if (transactionDirection == Types.TransactionDirection.Buying)
                        {
                            queryList.Sort(sortList.CompareSortBuy);
                        }
                        else if (transactionDirection == Types.TransactionDirection.Selling)
                        {
                            queryList.Sort(sortList.CompareSortSell);
                        }
                    }
                    else
                    {
                        queryList = new List<T>();
                        queryList.Add(dataX);
                    }
                }
                else
                {
                    List<T> futureList = new List<T>();
                    futureList.Add(dataX);
                    entrustOrderList.Add(sholderCode.Trim(), futureList);
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("添加委托数据存储区时异常", ex);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 删除撮合委托实体
        /// </summary>
        /// <param name="dataX">委托</param>
        public bool Remove(T dataX)
        {

            if (entrustOrderList == null)
            {
                return false;
            }
            rwLock.EnterWriteLock();
            try
            {
                string sholderCode = "";
                if (typeof(T) == typeof(FutureDataOrderEntity))
                {
                    #region  股指期货添加
                    FutureDataOrderEntity qhitem = dataX as FutureDataOrderEntity;
                    sholderCode = qhitem.SholderCode.Trim();
                    #endregion
                }
                else if (typeof(T) == typeof(StockDataOrderEntity))
                {
                    #region  现货添加
                    StockDataOrderEntity xhitem = dataX as StockDataOrderEntity;
                    sholderCode = xhitem.SholderCode.Trim();
                    #endregion
                }
                else if (typeof(T) == typeof(HKEntrustOrderInfo))
                {
                    #region  港股添加
                    HKEntrustOrderInfo hkitem = dataX as HKEntrustOrderInfo;
                    sholderCode = hkitem.SholderCode.Trim();
                    #endregion
                }
                else if (typeof(T) == typeof(CommoditiesDataOrderEntity))
                {
                    #region  商品期货添加 add by 董鹏 2010-01-22
                    CommoditiesDataOrderEntity qhitem = dataX as CommoditiesDataOrderEntity;
                    sholderCode = qhitem.SholderCode.Trim();
                    #endregion
                }

                if (entrustOrderList.ContainsKey(sholderCode))
                {
                    bool remove = entrustOrderList[sholderCode].Remove(dataX);
                    if (entrustOrderList[sholderCode].Count <= 0)
                    {
                        entrustOrderList.Remove(sholderCode);
                    }
                    return remove;
                }

                return false;

            }
            finally
            {
                rwLock.ExitWriteLock();
            }

        }

        /// <summary>
        /// 删除撮合实体
        /// </summary>
        /// <param name="orderNo">委托编号</param>
        /// <param name="sholderCode">股东代码</param>
        /// <param name="cancount"></param>
        /// <returns></returns>
        public bool Remove(string orderNo, string sholderCode, ref decimal cancount)
        {
            if (entrustOrderList == null)
            {
                return false;
            }

            rwLock.EnterWriteLock();
            try
            {
                if (!entrustOrderList.ContainsKey(sholderCode))
                {
                    return false;
                }
                List<T> list = entrustOrderList[sholderCode];
                if (typeof(T) == typeof(FutureDataOrderEntity))
                {
                    #region  股指期货删除
                    foreach (T entity in list)
                    {
                        FutureDataOrderEntity item = entity as FutureDataOrderEntity;
                        if (item.OrderNo == orderNo)
                        {
                            cancount = item.OrderVolume;
                            return list.Remove(entity);

                        }
                    }
                    #endregion
                }
                else if (typeof(T) == typeof(StockDataOrderEntity))
                {
                    #region  现货删除
                    foreach (T entity in list)
                    {
                        StockDataOrderEntity item = entity as StockDataOrderEntity;
                        if (item.OrderNo == orderNo)
                        {
                            cancount = item.OrderVolume;
                            return list.Remove(entity);
                        }
                    }
                    #endregion
                }
                else if (typeof(T) == typeof(HKEntrustOrderInfo))
                {
                    #region  港股股删除
                    foreach (T entity in list)
                    {
                        HKEntrustOrderInfo item = entity as HKEntrustOrderInfo;
                        if (item.OrderNo == orderNo)
                        {
                            cancount = item.OrderVolume;
                            return list.Remove(entity);
                        }
                    }
                    #endregion
                }
                else if (typeof(T) == typeof(CommoditiesDataOrderEntity))
                {
                    #region  商品期货删除  add by 董鹏 2010-01-22
                    foreach (T entity in list)
                    {
                        CommoditiesDataOrderEntity item = entity as CommoditiesDataOrderEntity;
                        if (item.OrderNo == orderNo)
                        {
                            cancount = item.OrderVolume;
                            return list.Remove(entity);

                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("撮合撤单删除撮合实体异常", ex);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }

            return false;
        }



        /// <summary>
        /// 修改委托单中委托数量（撮合中只处理减量委托）
        /// 返回0表示改单失败包含实体为空和查询不到委托单等--不回报
        /// 返回1表示查询到委托单，但不附合减量改单规则 --回报
        /// 返回2表示查改单成功--回报
        /// </summary>
        /// <param name="orderNo">要改单的原委托单编号</param>
        /// <param name="sholderCode">股东代码</param>
        /// <param name="modifynumer">改单委托量</param>
        /// <param name="nowOrderVolue">改单后当前委托量</param>
        /// <param name="canleCount">撤单量</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns>返回0，1，2中其一</returns>
        public int Modify(string orderNo, string sholderCode, decimal modifynumer, ref decimal nowOrderVolue, ref decimal canleCount, ref string errMsg)
        {

            if (entrustOrderList == null)
            {
                return 0;
            }
            rwLock.EnterWriteLock();

            errMsg = "";
            canleCount = 0;
            nowOrderVolue = 0;
            try
            {
                if (typeof(T) != typeof(HKEntrustOrderInfo))
                {
                    return 0;
                }
                if (!entrustOrderList.ContainsKey(sholderCode))
                {
                    return 0;
                }
                List<T> list = entrustOrderList[sholderCode];
                foreach (T entity in list)
                {
                    HKEntrustOrderInfo item = entity as HKEntrustOrderInfo;
                    if (item.OrderNo == orderNo)
                    {
                        if (modifynumer >= item.OldVolume)
                        {
                            errMsg = "CH-0001:减量委托改单失败，改单量不可以大于或者等于原委托量";
                            canleCount = 0;
                            return 1;
                        }
                        //计算当前已经成交量=原委托量-当前剩余量;
                        decimal tradeVolume = item.OldVolume - item.OrderVolume;
                        //计算当前还可以委托量多少=当前改单委托量-当前已经成交量
                        decimal nowEntrusVolume = modifynumer - tradeVolume;
                        if (nowEntrusVolume <= 0)
                        {
                            errMsg = "CH-0001:减量委托改单失败，当前委托单的成交量已经大于或者等于改单委托量";
                            canleCount = 0;
                            return 1;
                        }
                        //当前撤单量=当前剩余量-当前还可以委托量
                        canleCount = item.OrderVolume - nowEntrusVolume;
                        nowOrderVolue = nowEntrusVolume;
                        item.OldVolume = modifynumer;
                        item.OrderVolume = nowEntrusVolume;
                        errMsg = "改单成功";
                        return 2;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("港股改单队列修改实体异常", ex);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
            return 0;
        }

        #endregion

        #region 返回获取所有委托
        /// <summary>
        /// 返回接收所有委托单实体 并重建委托缓存区
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<T>> GetAcceptEntitys()
        {
            Dictionary<string, List<T>> matchOrderlist;
            rwLock.EnterReadLock();
            try
            {
                matchOrderlist = entrustOrderList;
                entrustOrderList = new Dictionary<string, List<T>>();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
            return matchOrderlist;
        }
        #endregion


    }
    #endregion

}
