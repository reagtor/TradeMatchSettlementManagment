using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatchCenter.Entity;
using MatchCenter.Entity.HK;

namespace MatchCenter.BLL.MatchRules
{
    /// <summary>
    /// Title:接收到的委托排序规则泛型类
    /// Create By:李健华
    /// Create Date:2009-09-16
    /// Desc.:增加了商品期货相关处理
    /// Update By:董鹏
    /// Update Date:2010-01-27
    /// </summary>
    public class EntrustOrderDataSort<T> where T : new()
    {
        /// <summary>
        /// 期货买比较排序器,比较排序规则：高价在先，同等价时间先到在前。
        /// </summary>
        /// <param name="dataX">期货委托单</param>
        /// <param name="dataY">期货委托单</param>
        /// <returns></returns>
        public int CompareSortBuy(T dataX, T dataY)
        {
            // FutureDataOrderEntity

            if (typeof(T) == typeof(FutureDataOrderEntity))
            {
                #region 股指期货比较排序
                FutureDataOrderEntity itemX = dataX as FutureDataOrderEntity;
                FutureDataOrderEntity itemY = dataY as FutureDataOrderEntity;
                if (itemX.OrderPrice > itemY.OrderPrice)
                {
                    return 1;
                }
                else if (itemX.OrderPrice == itemY.OrderPrice)
                {
                    if (itemX.ReachTime >= itemY.ReachTime)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;

                }
                #endregion
            }
            else if (typeof(T) == typeof(StockDataOrderEntity))
            {
                #region 现货比较排序
                StockDataOrderEntity itemX = dataX as StockDataOrderEntity;
                StockDataOrderEntity itemY = dataY as StockDataOrderEntity;
                if (itemX.OrderPrice > itemY.OrderPrice)
                {
                    return 1;
                }
                else if (itemX.OrderPrice == itemY.OrderPrice)
                {
                    if (itemX.ReachTime >= itemY.ReachTime)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;

                }
                #endregion
            }
            else if (typeof(T) == typeof(HKEntrustOrderInfo))
            {
                #region 港股比较排序
                HKEntrustOrderInfo itemX = dataX as HKEntrustOrderInfo;
                HKEntrustOrderInfo itemY = dataY as HKEntrustOrderInfo;
                if (itemX.OrderPrice > itemY.OrderPrice)
                {
                    return 1;
                }
                else if (itemX.OrderPrice == itemY.OrderPrice)
                {
                    if (itemX.ReceiveTime >= itemY.ReceiveTime)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;

                }
                #endregion
            }
            else if (typeof(T) == typeof(CommoditiesDataOrderEntity))
            {
                #region 商品期货比较排序
                CommoditiesDataOrderEntity itemX = dataX as CommoditiesDataOrderEntity;
                CommoditiesDataOrderEntity itemY = dataY as CommoditiesDataOrderEntity;
                if (itemX.OrderPrice > itemY.OrderPrice)
                {
                    return 1;
                }
                else if (itemX.OrderPrice == itemY.OrderPrice)
                {
                    if (itemX.ReachTime >= itemY.ReachTime)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;

                }
                #endregion
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 卖比较排序器,比较排序规则：低价在先，同等价时间先到在前。
        /// </summary>
        /// <param name="dataX">期货委托单</param>
        /// <param name="dataY">期货委托单</param>
        /// <returns></returns>
        public int CompareSortSell(T dataX, T dataY)
        {
            if (typeof(T) == typeof(FutureDataOrderEntity))
            {
                #region 股指期货比较排序
                FutureDataOrderEntity itemX = dataX as FutureDataOrderEntity;
                FutureDataOrderEntity itemY = dataY as FutureDataOrderEntity;
                if (itemX.OrderPrice < itemY.OrderPrice)
                {
                    return 1;
                }
                else if (itemX.OrderPrice == itemY.OrderPrice)
                {
                    if (itemX.ReachTime <= itemY.ReachTime)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
                 #endregion
            }
            else if (typeof(T) == typeof(StockDataOrderEntity))
            {
                #region 现货比较排序
                StockDataOrderEntity itemX = dataX as StockDataOrderEntity;
                StockDataOrderEntity itemY = dataY as StockDataOrderEntity;
                if (itemX.OrderPrice < itemY.OrderPrice)
                {
                    return 1;
                }
                else if (itemX.OrderPrice == itemY.OrderPrice)
                {
                    if (itemX.ReachTime <= itemY.ReachTime)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
                #endregion
            }
            else if (typeof(T) == typeof(HKEntrustOrderInfo))
            {
                #region 港股比较排序
                HKEntrustOrderInfo itemX = dataX as HKEntrustOrderInfo;
                HKEntrustOrderInfo itemY = dataY as HKEntrustOrderInfo;
                if (itemX.OrderPrice < itemY.OrderPrice)
                {
                    return 1;
                }
                else if (itemX.OrderPrice == itemY.OrderPrice)
                {
                    if (itemX.ReceiveTime <= itemY.ReceiveTime)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
                #endregion
            }
            else if (typeof(T) == typeof(CommoditiesDataOrderEntity))
            {
                #region 商品期货比较排序
                CommoditiesDataOrderEntity itemX = dataX as CommoditiesDataOrderEntity;
                CommoditiesDataOrderEntity itemY = dataY as CommoditiesDataOrderEntity;
                if (itemX.OrderPrice < itemY.OrderPrice)
                {
                    return 1;
                }
                else if (itemX.OrderPrice == itemY.OrderPrice)
                {
                    if (itemX.ReachTime <= itemY.ReachTime)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
                #endregion
            }
            else
            {
                return -1;
            }
        }

    }
}
