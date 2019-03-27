using System;
using System.Collections.Generic;
using System.Text;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.DAL.AccountManagementAndFindDAL;
using ReckoningCounter.Entity;
using ReckoningCounter.DAL;
using GTA.VTS.Common.CommonObject;
using ReckoningCounter.Entity.AccountManagementAndFindEntity;
using ReckoningCounter.Model;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.Entity.Model;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.Entity.Model.QueryFilter;

namespace ReckoningCounter.BLL.AccountManagementAndFind.AccountManagementAndFindBLL
{

    /// <summary>
    /// 作用：柜台委托和成交查询（包括： 现货委托查询、现货成交查询、期货委托查询、期货成交查询）
    /// 作者：李科恒
    /// 日期：2008-12-06
    /// Update by:李健华
    /// Update Date:2009-10-19
    /// Desc.:把未有实现的方法或者无用的方法删除掉
    /// </summary>
    public class EntrustAndTradeFindBLL
    {

        # region (NEW)现货当日委托查询过滤条件
        /// <summary>
        /// 现货当日委托查询过滤条件
        /// </summary>
        /// <param name="_findCoditions">过滤条件实体对象</param>
        /// <returns></returns>
        string BuildXHOrderQueryWhere(SpotEntrustConditionFindEntity _findCoditions)
        {
            string result = string.Empty;

            # region 委托单号（当只根据委托单号查询时）
            //-1.委托单号（当只根据委托单号查询时）
            if (!string.IsNullOrEmpty(_findCoditions.EntrustNumber)) //当有委托单号时
            {
                result += string.Format("EntrustNumber='{0}'", _findCoditions.EntrustNumber);
            }
            else //当没委托单号时
            {
                # region  0.资金账号
                if (!string.IsNullOrEmpty(_findCoditions.SpotCapitalAccount))
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("CapitalAccount='{0}'", _findCoditions.SpotCapitalAccount);
                    }
                    else
                    {
                        result += string.Format(" AND CapitalAccount='{0}'", _findCoditions.SpotCapitalAccount);
                    }
                }
                # endregion

                # region  1.买卖方向

                if (_findCoditions.BuySellDirection != 0)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("BuySellTypeId='{0}'", _findCoditions.BuySellDirection);
                    }
                    else
                    {
                        result += string.Format(" AND BuySellTypeId='{0}'", _findCoditions.BuySellDirection);
                    }
                }

                # endregion

                # region 2.可撤标识
                if (_findCoditions.CanBeWithdrawnLogo != 0)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("CancelLogo='{0}'", _findCoditions.CanBeWithdrawnLogo);
                    }
                    else
                    {
                        result += string.Format(" AND CancelLogo='{0}'", _findCoditions.CanBeWithdrawnLogo);
                    }
                }
                # endregion

                # region 3.所属市场
                if (_findCoditions.BelongToMarket != 0)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("MarketTypeId='{0}'", _findCoditions.BelongToMarket);
                    }
                    else
                    {
                        result += string.Format(" AND MarketTypeId='{0}'", _findCoditions.BelongToMarket);
                    }
                }
                # endregion

                # region 4.委托状态
                if (_findCoditions.EntrustState != 0)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("OrderStatusId='{0}'", _findCoditions.EntrustState);
                    }
                    else
                    {
                        result += string.Format(" AND OrderStatusId='{0}'", _findCoditions.EntrustState);
                    }
                }
                # endregion

                # region 5.币种赋值
                if (_findCoditions.CurrencyType != 0)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("CurrencyTypeId='{0}'", _findCoditions.CurrencyType);
                    }
                    else
                    {
                        result += string.Format(" AND CurrencyTypeId='{0}'", _findCoditions.CurrencyType);
                    }
                }
                # endregion

                # region 6.现货代码
                if (!string.IsNullOrEmpty(_findCoditions.SpotCode))
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("SpotCode='{0}'", _findCoditions.SpotCode);
                    }
                    else
                    {
                        result += string.Format(" AND SpotCode='{0}'", _findCoditions.SpotCode);
                    }
                }
                # endregion

                # region 7.品种类别
                if (!string.IsNullOrEmpty(_findCoditions.VarietyType))
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("VarietyType='{0}'", _findCoditions.VarietyType);
                    }
                    else
                    {
                        result += string.Format(" AND VarietyType='{0}'", _findCoditions.VarietyType);
                    }
                }
                #endregion

                # region 8.查询时间为当日
                //if (string.IsNullOrEmpty(result))
                //{
                //    //查询开始时间为 ：上一个月的今天
                //    //_findCoditions.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddMonths(-1).ToShortDateString());

                //    //查询开始时间为：今天 00：00：00
                //    _findCoditions.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(0).ToShortDateString());

                //    //结束时间为：明天 00：00：00
                //    _findCoditions.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(1).ToShortDateString());

                //    result += string.Format("EntrustTime BETWEEN '{0}' AND '{1}'", _findCoditions.StartTime, _findCoditions.EndTime);
                //}
                //else
                //{
                //    //查询开始时间为：今天 00：00：00
                //    _findCoditions.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(0).ToShortDateString());

                //    //结束时间为：明天 00：00：00
                //    _findCoditions.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(1).ToShortDateString());

                //    result += string.Format(" AND EntrustTime BETWEEN '{0}' AND '{1}'", _findCoditions.StartTime, _findCoditions.EndTime);
                //}
                # endregion
            }
            # endregion
            return result;
        }
        # endregion 现货当日委托查询过滤条件

        # region (NEW)期货当日委托查询过滤条件
        /// <summary>
        /// 期货当日委托查询过滤条件
        /// </summary>
        /// <param name="_findCoditions">过滤条件实体对象</param>
        /// <returns></returns>
        string BuildQHOrderQueryWhere(FuturesEntrustConditionFindEntity _findCoditions)
        {
            string result = string.Empty;

            # region 委托单号（当只根据委托单号查询时）
            //-1.委托单号（当只根据委托单号查询时）
            if (!string.IsNullOrEmpty(_findCoditions.EntrustNumber)) //当有委托单号时
            {
                result += string.Format("EntrustNumber='{0}'", _findCoditions.EntrustNumber);
            }
            else //当没委托单号时
            {
                # region  0.资金账号
                if (!string.IsNullOrEmpty(_findCoditions.CapitalAccount))
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("CapitalAccount='{0}'", _findCoditions.CapitalAccount);
                    }
                    else
                    {
                        result += string.Format(" AND CapitalAccount='{0}'", _findCoditions.CapitalAccount);
                    }
                }
                # endregion

                # region  1.买卖方向

                if (_findCoditions.BuySellDirection != 0)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("BuySellId='{0}'", _findCoditions.BuySellDirection);
                    }
                    else
                    {
                        result += string.Format(" AND BuySellId='{0}'", _findCoditions.BuySellDirection);
                    }
                }

                # endregion

                # region 2.可撤标识
                if (_findCoditions.CanBeWithdrawnLogo != 0)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("CancelLogo='{0}'", _findCoditions.CanBeWithdrawnLogo);
                    }
                    else
                    {
                        result += string.Format(" AND CancelLogo='{0}'", _findCoditions.CanBeWithdrawnLogo);
                    }
                }
                # endregion

                # region 3.所属市场
                if (_findCoditions.BelongToMarket != 0)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("OwnershipMarketId='{0}'", _findCoditions.BelongToMarket);
                    }
                    else
                    {
                        result += string.Format(" AND OwnershipMarketId='{0}'", _findCoditions.BelongToMarket);
                    }
                }
                # endregion

                # region 4.委托状态
                if (_findCoditions.EntrustState != 0)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("OrderStatusId='{0}'", _findCoditions.EntrustState);
                    }
                    else
                    {
                        result += string.Format(" AND OrderStatusId='{0}'", _findCoditions.EntrustState);
                    }
                }
                # endregion

                # region 5.币种赋值
                if (_findCoditions.CurrencyTypeId != 0)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("CurrencyTypeId='{0}'", _findCoditions.CurrencyTypeId);
                    }
                    else
                    {
                        result += string.Format(" AND CurrencyTypeId='{0}'", _findCoditions.CurrencyTypeId);
                    }
                }
                # endregion

                # region 6.合约代码
                if (!string.IsNullOrEmpty(_findCoditions.ContractCode))
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("ContractCode='{0}'", _findCoditions.ContractCode);
                    }
                    else
                    {
                        result += string.Format(" AND ContractCode='{0}'", _findCoditions.ContractCode);
                    }
                }
                # endregion

                # region 7.品种类别
                if (_findCoditions.VarietyType != 0)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("VarietyTypeID='{0}'", _findCoditions.VarietyType);
                    }
                    else
                    {
                        result += string.Format(" AND VarietyTypeID='{0}'", _findCoditions.VarietyType);
                    }
                }
                #endregion

                # region 8.查询时间为当日
                //if (string.IsNullOrEmpty(result))
                //{
                //    //查询开始时间为 ：上一个月的今天
                //    //_findCoditions.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddMonths(-1).ToShortDateString());

                //    //查询开始时间为：当天 00:00:00
                //    _findCoditions.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(0).ToShortDateString());

                //    //结束时间为：明天 00：00：00
                //    _findCoditions.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(1).ToShortDateString());

                //    result += string.Format("EntrustTime BETWEEN '{0}' AND '{1}'", _findCoditions.StartTime, _findCoditions.EndTime);
                //}
                //else
                //{
                //    //查询开始时间为：当天 00:00:00
                //    _findCoditions.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(0).ToShortDateString());

                //    //结束时间为：明天 00：00：00
                //    _findCoditions.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(1).ToShortDateString());

                //    result += string.Format(" AND EntrustTime BETWEEN '{0}' AND '{1}'", _findCoditions.StartTime, _findCoditions.EndTime);
                //}
                # endregion
            }
            # endregion
            return result;
        }
        # endregion

        # region (NEW)现货当日成交查询过滤条件
        /// <summary>
        /// 现货当日成交查询过滤条件
        /// </summary>
        /// <param name="_findCoditions">过滤条件实体对象</param>
        /// <returns></returns>
        string BuildXHTradeQueryWhere(SpotTradeConditionFindEntity _findCoditions)
        {
            string result = string.Empty;

            # region 委托单号（判断有无委托单号）

            if (!string.IsNullOrEmpty(_findCoditions.EntrustNumber))  //委托单号（当只根据委托单号查询时）
            {
                result += string.Format("EntrustNumber='{0}'", _findCoditions.EntrustNumber);
            }
            else //当不带委托单号时
            {
                # region 0.资金帐户
                if (!string.IsNullOrEmpty(_findCoditions.SpotCapitalAccount))
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("CapitalAccount='{0}'", _findCoditions.SpotCapitalAccount);
                    }
                    else
                    {
                        result += string.Format(" AND CapitalAccount='{0}'", _findCoditions.SpotCapitalAccount);
                    }
                }
                # endregion

                # region  1.买卖方向

                if (_findCoditions.BuySellDirection != 0)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("BuySellTypeId='{0}'", _findCoditions.BuySellDirection);
                    }
                    else
                    {
                        result += string.Format(" AND BuySellTypeId='{0}'", _findCoditions.BuySellDirection);
                    }
                }

                # endregion

                # region 2.所属市场
                if (_findCoditions.BelongToMarket != 0)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("MarketTypeId='{0}'", _findCoditions.BelongToMarket);
                    }
                    else
                    {
                        result += string.Format(" AND MarketTypeId='{0}'", _findCoditions.BelongToMarket);
                    }
                }
                # endregion

                # region 3.成交类型
                if (_findCoditions.TradeType != 0)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("TradeTypeId='{0}'", _findCoditions.TradeType);
                    }
                    else
                    {
                        result += string.Format(" AND TradeTypeId='{0}'", _findCoditions.TradeType);
                    }
                }
                # endregion

                # region 4.币种赋值
                if (_findCoditions.CurrencyType != 0)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("CurrencyTypeId='{0}'", _findCoditions.CurrencyType);
                    }
                    else
                    {
                        result += string.Format(" AND CurrencyTypeId='{0}'", _findCoditions.CurrencyType);
                    }
                }
                # endregion

                # region 5.现货代码
                if (!string.IsNullOrEmpty(_findCoditions.SpotCode))
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("SpotCode='{0}'", _findCoditions.SpotCode);
                    }
                    else
                    {
                        result += string.Format(" AND SpotCode='{0}'", _findCoditions.SpotCode);
                    }
                }
                # endregion

                # region 6.品种类别
                if (!string.IsNullOrEmpty(_findCoditions.VarietyType))
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("VarietyType='{0}'", _findCoditions.VarietyType);
                    }
                    else
                    {
                        result += string.Format(" AND VarietyType='{0}'", _findCoditions.VarietyType);
                    }
                }
                #endregion

                # region 7.查询时间为当日
                //if (string.IsNullOrEmpty(result))
                //{
                //    //查询开始时间为 ：上一个月的今天
                //    //_findCoditions.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddMonths(-1).ToShortDateString());

                //    //查询开始时间为：当日零晨 00：00：00
                //    _findCoditions.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(0).ToShortDateString());

                //    //结束时间为：明天零晨 00：00：00
                //    _findCoditions.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(1).ToShortDateString());

                //    result += string.Format("TradeTime BETWEEN '{0}' AND '{1}'", _findCoditions.StartTime, _findCoditions.EndTime);
                //}
                //else
                //{
                //    //查询开始时间为：当日零晨 00：00：00
                //    _findCoditions.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(0).ToShortDateString());

                //    //结束时间为：明天零晨 00：00：00
                //    _findCoditions.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(1).ToShortDateString());

                //    result += string.Format(" AND TradeTime BETWEEN '{0}' AND '{1}'", _findCoditions.StartTime, _findCoditions.EndTime);
                //}
                # endregion
            }
            # endregion

            return result;
        }

        # endregion 现货查询过滤条件

        # region (NEW)期货当日成交查询过滤条件
        /// <summary>
        /// 期货当日成交查询过滤条件
        /// </summary>
        /// <param name="_findCoditions">过滤条件实体对象</param>
        /// <returns></returns>
        string BuildQHTradeQueryWhere(FuturesTradeConditionFindEntity _findCoditions)
        {
            string result = string.Empty;

            # region 委托单号（判断有无委托单号）

            if (!string.IsNullOrEmpty(_findCoditions.EntrustNumber))  //委托单号（当只根据委托单号查询时）
            {
                result += string.Format("EntrustNumber='{0}'", _findCoditions.EntrustNumber);
            }
            else //当不带委托单号时
            {
                # region 0.资金帐户
                if (!string.IsNullOrEmpty(_findCoditions.CapitalAccount))
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("CapitalAccount='{0}'", _findCoditions.CapitalAccount);
                    }
                    else
                    {
                        result += string.Format(" AND CapitalAccount='{0}'", _findCoditions.CapitalAccount);
                    }
                }
                # endregion

                # region  1.买卖方向

                if (_findCoditions.BuySellDirection != 0)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("BuySellId='{0}'", _findCoditions.BuySellDirection);
                    }
                    else
                    {
                        result += string.Format(" AND BuySellId='{0}'", _findCoditions.BuySellDirection);
                    }
                }

                # endregion

                # region 2.所属市场
                if (_findCoditions.BelongToMarket != 0)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("OwnershipMarket='{0}'", _findCoditions.BelongToMarket);
                    }
                    else
                    {
                        result += string.Format(" AND OwnershipMarket='{0}'", _findCoditions.BelongToMarket);
                    }
                }
                # endregion

                # region 3.成交类型
                if (_findCoditions.TradeType != 0)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("TradeTypeId='{0}'", _findCoditions.TradeType);
                    }
                    else
                    {
                        result += string.Format(" AND TradeTypeId='{0}'", _findCoditions.TradeType);
                    }
                }
                # endregion

                # region 4.币种赋值
                if (_findCoditions.CurrencyTypeId != 0)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("CurrencyTypeId='{0}'", _findCoditions.CurrencyTypeId);
                    }
                    else
                    {
                        result += string.Format(" AND CurrencyTypeId='{0}'", _findCoditions.CurrencyTypeId);
                    }
                }
                # endregion

                # region 5.合约代码
                if (!string.IsNullOrEmpty(_findCoditions.ContractCode))
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("ContractCode='{0}'", _findCoditions.ContractCode);
                    }
                    else
                    {
                        result += string.Format(" AND ContractCode='{0}'", _findCoditions.ContractCode);
                    }
                }
                # endregion

                # region 6.品种类别
                if (_findCoditions.VarietyType != 0)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += string.Format("VarietyTypeId='{0}'", _findCoditions.VarietyType);
                    }
                    else
                    {
                        result += string.Format(" AND VarietyTypeId='{0}'", _findCoditions.VarietyType);
                    }
                }
                #endregion

                # region 7.查询时间为当日
                //if (string.IsNullOrEmpty(result))
                //{
                //    //查询开始时间为 ：上一个月的今天
                //    //_findCoditions.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddMonths(-1).ToShortDateString());

                //    //查询开始时间为：当日零晨 00：00：00
                //    _findCoditions.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(0).ToShortDateString());

                //    //结束时间为：明天零晨 00：00：00
                //    _findCoditions.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(1).ToShortDateString());

                //    result += string.Format("TradeTime BETWEEN '{0}' AND '{1}'", _findCoditions.StartTime, _findCoditions.EndTime);
                //}
                //else
                //{
                //    //查询开始时间为：当日零晨 00：00：00
                //    _findCoditions.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(0).ToShortDateString());

                //    //结束时间为：明天零晨 00：00：00
                //    _findCoditions.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(1).ToShortDateString());

                //    result += string.Format(" AND TradeTime BETWEEN '{0}' AND '{1}'", _findCoditions.StartTime, _findCoditions.EndTime);
                //}
                # endregion
            }
            # endregion

            return result;
        }

        # endregion 现货查询过滤条件

        # region (NEW)现货历史委托查询过滤条件
        /// <summary>
        /// 现货历史委托查询过滤条件
        /// </summary>
        /// <param name="_findCoditions">过滤条件实体对象</param>
        /// <returns></returns>
        string BuildXhHistoryOrderQueryWhere(SpotEntrustConditionFindEntity _findCoditions)
        {
            #region old
            //string result = string.Empty;

            //# region  -1.委托单号（当只根据委托单号查询时）
            ////-1.委托单号（当只根据委托单号查询时）
            //if (!string.IsNullOrEmpty(_findCoditions.EntrustNumber))
            //{
            //    result += string.Format("EntrustNumber='{0}'", _findCoditions.EntrustNumber);
            //}
            //# endregion

            //# region  0.资金账号
            //if (!string.IsNullOrEmpty(_findCoditions.SpotCapitalAccount))
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("CapitalAccount='{0}'", _findCoditions.SpotCapitalAccount);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND CapitalAccount='{0}'", _findCoditions.SpotCapitalAccount);
            //    }
            //}
            //# endregion

            //# region  1.买卖方向

            //if (_findCoditions.BuySellDirection != 0)
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("BuySellTypeId='{0}'", _findCoditions.BuySellDirection);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND BuySellTypeId='{0}'", _findCoditions.BuySellDirection);
            //    }
            //}

            //# endregion

            //# region 3.所属市场
            //if (_findCoditions.BelongToMarket != 0)
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("MarketTypeId='{0}'", _findCoditions.BelongToMarket);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND MarketTypeId='{0}'", _findCoditions.BelongToMarket);
            //    }
            //}
            //# endregion

            //# region 4.委托状态
            //if (_findCoditions.EntrustState != 0)
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("OrderStatusId='{0}'", _findCoditions.EntrustState);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND OrderStatusId='{0}'", _findCoditions.EntrustState);
            //    }
            //}
            //# endregion

            //# region 5.币种赋值
            //if (_findCoditions.CurrencyType != 0)
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("CurrencyTypeId='{0}'", _findCoditions.CurrencyType);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND CurrencyTypeId='{0}'", _findCoditions.CurrencyType);
            //    }
            //}
            //# endregion

            //# region 6.现货代码
            //if (!string.IsNullOrEmpty(_findCoditions.SpotCode))
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("SpotCode='{0}'", _findCoditions.SpotCode);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND SpotCode='{0}'", _findCoditions.SpotCode);
            //    }
            //}
            //# endregion

            //# region 7.品种类别
            //if (!string.IsNullOrEmpty(_findCoditions.VarietyType))
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("VarietyType='{0}'", _findCoditions.VarietyType);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND VarietyType='{0}'", _findCoditions.VarietyType);
            //    }
            //}
            //#endregion

            //# region 8.查询开始时间和结束时间
            //if (_findCoditions.StartTime != null && _findCoditions.EndTime != null)//起始和结束时间均已赋值
            //{
            //    if (DateTime.Compare(_findCoditions.StartTime, _findCoditions.EndTime) <= 0) //起始时间小于结束时间（比如2008-05-06小于2008-05-28）
            //    {
            //        _findCoditions.EndTime = _findCoditions.EndTime.AddDays(1).Date;
            //        if (string.IsNullOrEmpty(result))
            //        {
            //            result += string.Format("EntrustTime >= '{0}' AND  EntrustTime< '{1}'", _findCoditions.StartTime, _findCoditions.EndTime);
            //        }
            //        else
            //        {
            //            result += string.Format(" AND (EntrustTime >= '{0}' AND  EntrustTime< '{1}')", _findCoditions.StartTime, _findCoditions.EndTime);
            //        }
            //    }
            //    else //起始时间大于或等于结束时间，就默认查出最近一个月的（从昨天开始往前推一个月）
            //    {
            //        //要得到上个月方法是：当月减去一个月
            //        //DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-1).ToShortDateString();

            //        //查询开始时间为 ：上一个月的今天
            //        _findCoditions.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddMonths(-1).ToShortDateString());

            //        //结束时间为：今天 00：00：00
            //        _findCoditions.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(0).ToShortDateString());

            //        if (string.IsNullOrEmpty(result))
            //        {
            //            result += string.Format(" EntrustTime >= '{0}' AND  EntrustTime< '{1}'", _findCoditions.StartTime, _findCoditions.EndTime);
            //        }
            //        else
            //        {
            //            result += string.Format(" AND (EntrustTime >= '{0}' AND  EntrustTime< '{1}')", _findCoditions.StartTime, _findCoditions.EndTime);
            //        }
            //    }
            //}
            //else //若起始时间或结束时间未赋值，就默认查出最近一个月的（从昨天开始往前推一个月）
            //{
            //    //C#中用DateTime取出日期时间.怎么才能转换为只有日期没有时间? 就是只要年月日,不要时间..要怎么做??
            //    //解决方法如下：
            //    //DateTime dt=new DateTime(); 
            //    //dt.ToString("yyyy-MM-dd");

            //    //查询开始时间为 ：上一个月的今天
            //    _findCoditions.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddMonths(-1).ToShortDateString());

            //    //结束时间为：今天 00：00：00
            //    _findCoditions.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(0).ToShortDateString());

            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("EntrustTime >= '{0}' AND  EntrustTime< '{1}'", _findCoditions.StartTime, _findCoditions.EndTime);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND (EntrustTime >= '{0}' AND  EntrustTime< '{1}')", _findCoditions.StartTime, _findCoditions.EndTime);
            //    }
            //}
            //#endregion

            //return result;
            #endregion

            StringBuilder sbFilter = new StringBuilder("1=1 ");

            # region  -1.委托单号（当只根据委托单号查询时）
            //-1.委托单号（当只根据委托单号查询时）
            if (!string.IsNullOrEmpty(_findCoditions.EntrustNumber))
            {
                sbFilter.AppendFormat(" AND EntrustNumber='{0}'", _findCoditions.EntrustNumber);
            }
            # endregion

            # region  0.资金账号
            if (!string.IsNullOrEmpty(_findCoditions.SpotCapitalAccount))
            {
                sbFilter.AppendFormat(" AND CapitalAccount='{0}'", _findCoditions.SpotCapitalAccount);
            }
            # endregion

            # region  1.买卖方向

            if (_findCoditions.BuySellDirection != 0)
            {
                sbFilter.AppendFormat(" AND BuySellTypeId='{0}'", _findCoditions.BuySellDirection);
            }

            # endregion

            # region 3.所属市场
            if (_findCoditions.BelongToMarket != 0)
            {
                sbFilter.AppendFormat(" AND MarketTypeId='{0}'", _findCoditions.BelongToMarket);
            }
            # endregion

            # region 4.委托状态
            if (_findCoditions.EntrustState != 0)
            {
                sbFilter.AppendFormat(" AND OrderStatusId='{0}'", _findCoditions.EntrustState);
            }
            # endregion

            # region 5.币种赋值
            if (_findCoditions.CurrencyType != 0)
            {
                sbFilter.AppendFormat(" AND CurrencyTypeId='{0}'", _findCoditions.CurrencyType);
            }
            # endregion

            # region 6.现货代码
            if (!string.IsNullOrEmpty(_findCoditions.SpotCode))
            {
                sbFilter.AppendFormat(" AND SpotCode='{0}'", _findCoditions.SpotCode);
            }
            # endregion

            # region 7.品种类别
            if (!string.IsNullOrEmpty(_findCoditions.VarietyType))
            {
                sbFilter.AppendFormat(" AND VarietyType='{0}'", _findCoditions.VarietyType);
            }
            #endregion

            # region 8.查询开始时间和结束时间

            sbFilter.AppendFormat(CommonDALBulidSQLScript.BuildWhereQueryBetwennTime(_findCoditions.StartTime, _findCoditions.EndTime, 30), "EntrustTime");

            #endregion

            return sbFilter.ToString();
        }

        # endregion 现货当日委托查询过滤条件

        # region (NEW)期货历史委托查询过滤条件
        /// <summary>
        /// 期货历史委托查询过滤条件
        /// </summary>
        /// <param name="_findCoditions">过滤条件实体对象</param>
        /// <returns></returns>
        string BuildQhHistoryOrderQueryWhere(FuturesEntrustConditionFindEntity _findCoditions)
        {
            #region old
            //string result = string.Empty;

            //# region  -1.委托单号（当只根据委托单号查询时）
            ////-1.委托单号（当只根据委托单号查询时）
            //if (!string.IsNullOrEmpty(_findCoditions.EntrustNumber))
            //{
            //    result += string.Format("EntrustNumber='{0}'", _findCoditions.EntrustNumber);
            //}
            //# endregion

            //# region  0.资金账号
            //if (!string.IsNullOrEmpty(_findCoditions.CapitalAccount))
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("CapitalAccount='{0}'", _findCoditions.CapitalAccount);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND CapitalAccount='{0}'", _findCoditions.CapitalAccount);
            //    }
            //}
            //# endregion

            //# region  1.买卖方向

            //if (_findCoditions.BuySellDirection != 0)
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("BuySellTypeId='{0}'", _findCoditions.BuySellDirection);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND BuySellTypeId='{0}'", _findCoditions.BuySellDirection);
            //    }
            //}

            //# endregion

            //# region 3.所属市场
            //if (_findCoditions.BelongToMarket != 0)
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("OwnershipMarketId='{0}'", _findCoditions.BelongToMarket);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND OwnershipMarketId='{0}'", _findCoditions.BelongToMarket);
            //    }
            //}
            //# endregion

            //# region 4.委托状态
            //if (_findCoditions.EntrustState != 0)
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("OrderStatusId='{0}'", _findCoditions.EntrustState);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND OrderStatusId='{0}'", _findCoditions.EntrustState);
            //    }
            //}
            //# endregion

            //# region 5.币种赋值
            //if (_findCoditions.CurrencyTypeId != 0)
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("CurrencyTypeId='{0}'", _findCoditions.CurrencyTypeId);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND CurrencyTypeId='{0}'", _findCoditions.CurrencyTypeId);
            //    }
            //}
            //# endregion

            //# region 6.合约代码
            //if (!string.IsNullOrEmpty(_findCoditions.ContractCode))
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("ContractCode='{0}'", _findCoditions.ContractCode);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND ContractCode='{0}'", _findCoditions.ContractCode);
            //    }
            //}
            //# endregion

            //# region 7.品种类别
            //if (_findCoditions.VarietyType != 0)
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("VarietypTypeId='{0}'", _findCoditions.VarietyType);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND VarietypTypeId='{0}'", _findCoditions.VarietyType);
            //    }
            //}
            //#endregion

            //# region 8.查询开始时间和结束时间
            //if (_findCoditions.StartTime != null && _findCoditions.EndTime != null)//起始和结束时间均已赋值
            //{
            //    if (DateTime.Compare(_findCoditions.StartTime, _findCoditions.EndTime) <= 0) //起始时间小于结束时间（比如2008-05-06小于2008-05-28）
            //    {
            //        _findCoditions.EndTime = _findCoditions.EndTime.AddDays(1).Date;
            //        if (string.IsNullOrEmpty(result))
            //        {
            //            result += string.Format("EntrustTime >= '{0}' AND  EntrustTime< '{1}'", _findCoditions.StartTime, _findCoditions.EndTime);
            //        }
            //        else
            //        {
            //            result += string.Format(" AND (EntrustTime >= '{0}' AND  EntrustTime< '{1}')", _findCoditions.StartTime, _findCoditions.EndTime);
            //        }
            //    }
            //    else //起始时间大于或等于结束时间，就默认查出最近一个月的（从昨天开始往前推一个月）
            //    {
            //        //要得到上个月方法是：当月减去一个月
            //        //DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-1).ToShortDateString();

            //        //查询开始时间为 ：上一个月的今天
            //        _findCoditions.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddMonths(-1).ToShortDateString());

            //        //结束时间为：今天 00：00：00
            //        _findCoditions.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(0).ToShortDateString());

            //        if (string.IsNullOrEmpty(result))
            //        {
            //            result += string.Format("EntrustTime >= '{0}' AND  EntrustTime< '{1}'", _findCoditions.StartTime, _findCoditions.EndTime);
            //        }
            //        else
            //        {
            //            result += string.Format(" AND (EntrustTime >= '{0}' AND  EntrustTime< '{1}')", _findCoditions.StartTime, _findCoditions.EndTime);
            //        }
            //    }
            //}
            //else //若起始时间或结束时间未赋值，就默认查出最近一个月的（从昨天开始往前推一个月）
            //{
            //    //C#中用DateTime取出日期时间.怎么才能转换为只有日期没有时间? 就是只要年月日,不要时间..要怎么做??
            //    //解决方法如下：
            //    //DateTime dt=new DateTime(); 
            //    //dt.ToString("yyyy-MM-dd");

            //    //查询开始时间为 ：上一个月的今天
            //    _findCoditions.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddMonths(-1).ToShortDateString());

            //    //结束时间为：今天 00：00：00
            //    _findCoditions.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(0).ToShortDateString());

            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("EntrustTime >= '{0}' AND  EntrustTime< '{1}'", _findCoditions.StartTime, _findCoditions.EndTime);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND (EntrustTime >= '{0}' AND  EntrustTime< '{1}')", _findCoditions.StartTime, _findCoditions.EndTime);
            //    }
            //}
            //#endregion

            //return result;
            #endregion

            StringBuilder sbFilter = new StringBuilder("1=1 ");

            # region  -1.委托单号（当只根据委托单号查询时）
            //-1.委托单号（当只根据委托单号查询时）
            if (!string.IsNullOrEmpty(_findCoditions.EntrustNumber))
            {
                sbFilter.AppendFormat(" AND EntrustNumber='{0}'", _findCoditions.EntrustNumber);
            }
            # endregion

            # region  0.资金账号
            if (!string.IsNullOrEmpty(_findCoditions.CapitalAccount))
            {
                sbFilter.AppendFormat(" AND CapitalAccount='{0}'", _findCoditions.CapitalAccount);
            }
            # endregion

            # region  1.买卖方向

            if (_findCoditions.BuySellDirection != 0)
            {
                sbFilter.AppendFormat(" AND BuySellTypeId='{0}'", _findCoditions.BuySellDirection);
            }

            # endregion

            # region 3.所属市场
            if (_findCoditions.BelongToMarket != 0)
            {
                sbFilter.AppendFormat(" AND OwnershipMarketId='{0}'", _findCoditions.BelongToMarket);
            }
            # endregion

            # region 4.委托状态
            if (_findCoditions.EntrustState != 0)
            {
                sbFilter.AppendFormat(" AND OrderStatusId='{0}'", _findCoditions.EntrustState);
            }
            # endregion

            # region 5.币种赋值
            if (_findCoditions.CurrencyTypeId != 0)
            {
                sbFilter.AppendFormat(" AND CurrencyTypeId='{0}'", _findCoditions.CurrencyTypeId);
            }
            # endregion

            # region 6.合约代码
            if (!string.IsNullOrEmpty(_findCoditions.ContractCode))
            {
                sbFilter.AppendFormat(" AND ContractCode='{0}'", _findCoditions.ContractCode);
            }
            # endregion

            # region 7.品种类别
            if (_findCoditions.VarietyType != 0)
            {
                sbFilter.AppendFormat(" AND VarietypTypeId='{0}'", _findCoditions.VarietyType);
            }
            #endregion

            # region 8.查询开始时间和结束时间

            sbFilter.AppendFormat(CommonDALBulidSQLScript.BuildWhereQueryBetwennTime(_findCoditions.StartTime, _findCoditions.EndTime, 30), "EntrustTime");

            #endregion

            return sbFilter.ToString();
        }

        # endregion 现货当日委托查询过滤条件

        # region (NEW)现货历史成交查询过滤条件
        /// <summary>
        /// 现货历史成交查询过滤条件
        /// </summary>
        /// <param name="_findCoditions">过滤条件实体对象</param>
        /// <returns></returns>
        string BuildXhHistoryTradeQueryWhere(SpotTradeConditionFindEntity _findCoditions)
        {
            #region old
            //string result = string.Empty;

            ////# region  -1.委托单号（当只根据委托单号查询时）
            //////-1.委托单号（当只根据委托单号查询时）
            ////if (!string.IsNullOrEmpty(_findCoditions.EntrustNumber))
            ////{
            ////    result += string.Format("EntrustNumber='{0}'", _findCoditions.EntrustNumber);
            ////}
            ////# endregion

            //# region 0.资金帐户
            //if (!string.IsNullOrEmpty(_findCoditions.SpotCapitalAccount))
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("CapitalAccount='{0}'", _findCoditions.SpotCapitalAccount);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND CapitalAccount='{0}'", _findCoditions.SpotCapitalAccount);
            //    }
            //}
            //# endregion

            //# region  1.买卖方向

            //if (_findCoditions.BuySellDirection != 0)
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("BuySellTypeId='{0}'", _findCoditions.BuySellDirection);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND BuySellTypeId='{0}'", _findCoditions.BuySellDirection);
            //    }
            //}

            //# endregion

            //# region 2.所属市场
            //if (_findCoditions.BelongToMarket != 0)
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("MarketTypeId='{0}'", _findCoditions.BelongToMarket);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND MarketTypeId='{0}'", _findCoditions.BelongToMarket);
            //    }
            //}
            //# endregion

            //# region 3.成交类型
            //if (_findCoditions.TradeType != 0)
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("TradeTypeId='{0}'", _findCoditions.TradeType);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND TradeTypeId='{0}'", _findCoditions.TradeType);
            //    }
            //}
            //# endregion

            //# region 4.币种赋值
            //if (_findCoditions.CurrencyType != 0)
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("CurrencyTypeId='{0}'", _findCoditions.CurrencyType);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND CurrencyTypeId='{0}'", _findCoditions.CurrencyType);
            //    }
            //}
            //# endregion

            //# region 5.现货代码
            //if (!string.IsNullOrEmpty(_findCoditions.SpotCode))
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("SpotCode='{0}'", _findCoditions.SpotCode);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND SpotCode='{0}'", _findCoditions.SpotCode);
            //    }
            //}
            //# endregion

            //# region 6.品种类别
            //if (!string.IsNullOrEmpty(_findCoditions.VarietyType))
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("VarietyType='{0}'", _findCoditions.VarietyType);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND VarietyType='{0}'", _findCoditions.VarietyType);
            //    }
            //}
            //#endregion

            //# region 7.查询开始时间和结束时间
            //if (_findCoditions.StartTime != null && _findCoditions.EndTime != null)//起始和结束时间均已赋值
            //{
            //    if (DateTime.Compare(_findCoditions.StartTime, _findCoditions.EndTime) <= 0) //起始时间小于结束时间（比如2008-05-06小于2008-05-28）
            //    {
            //        _findCoditions.EndTime = _findCoditions.EndTime.AddDays(1).Date;
            //        if (string.IsNullOrEmpty(result))
            //        {
            //            result += string.Format("TradeTime >= '{0}' AND  TradeTime< '{1}'", _findCoditions.StartTime, _findCoditions.EndTime);
            //        }
            //        else
            //        {
            //            result += string.Format(" AND (TradeTime >= '{0}' AND  TradeTime< '{1}')", _findCoditions.StartTime, _findCoditions.EndTime);
            //        }
            //    }
            //    else //起始时间大于或等于结束时间，就默认查出最近一个月的（从昨天开始往前推一个月）
            //    {
            //        //要得到上个月方法是：当月减去一个月
            //        //DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-1).ToShortDateString();

            //        //查询开始时间为 ：上一个月的今天
            //        _findCoditions.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddMonths(-1).ToShortDateString());

            //        //结束时间为：今天 00：00：00
            //        _findCoditions.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(0).ToShortDateString());

            //        if (string.IsNullOrEmpty(result))
            //        {
            //            result += string.Format("TradeTime >= '{0}' AND  TradeTime< '{1}'", _findCoditions.StartTime, _findCoditions.EndTime);
            //        }
            //        else
            //        {
            //            result += string.Format(" AND (TradeTime >= '{0}' AND  TradeTime< '{1}')", _findCoditions.StartTime, _findCoditions.EndTime);
            //        }
            //    }
            //}
            //else //若起始时间或结束时间未赋值，就默认查出最近一个月的（从昨天开始往前推一个月）
            //{
            //    //C#中用DateTime取出日期时间.怎么才能转换为只有日期没有时间? 就是只要年月日,不要时间..要怎么做??
            //    //解决方法如下：
            //    //DateTime dt=new DateTime(); 
            //    //dt.ToString("yyyy-MM-dd");

            //    //查询开始时间为 ：上一个月的今天
            //    _findCoditions.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddMonths(-1).ToShortDateString());

            //    //结束时间为：今天 00：00：00
            //    _findCoditions.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(0).ToShortDateString());

            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("TradeTime >= '{0}' AND  TradeTime< '{1}'", _findCoditions.StartTime, _findCoditions.EndTime);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND (TradeTime >= '{0}' AND  TradeTime< '{1}')", _findCoditions.StartTime, _findCoditions.EndTime);
            //    }
            //}
            //#endregion

            //return result;
            #endregion

            StringBuilder sbFilter = new StringBuilder("1=1 ");

            # region 0.资金帐户
            if (!string.IsNullOrEmpty(_findCoditions.SpotCapitalAccount))
            {
                sbFilter.AppendFormat(" AND CapitalAccount='{0}'", _findCoditions.SpotCapitalAccount);
            }
            # endregion

            # region  1.买卖方向

            if (_findCoditions.BuySellDirection != 0)
            {
                sbFilter.AppendFormat(" AND BuySellTypeId='{0}'", _findCoditions.BuySellDirection);
            }

            # endregion

            # region 2.所属市场
            if (_findCoditions.BelongToMarket != 0)
            {
                sbFilter.AppendFormat(" AND MarketTypeId='{0}'", _findCoditions.BelongToMarket);
            }
            # endregion

            # region 3.成交类型
            if (_findCoditions.TradeType != 0)
            {
                sbFilter.AppendFormat(" AND TradeTypeId='{0}'", _findCoditions.TradeType);
            }
            # endregion

            # region 4.币种赋值
            if (_findCoditions.CurrencyType != 0)
            {
                sbFilter.AppendFormat(" AND CurrencyTypeId='{0}'", _findCoditions.CurrencyType);
            }
            # endregion

            # region 5.现货代码
            if (!string.IsNullOrEmpty(_findCoditions.SpotCode))
            {
                sbFilter.AppendFormat(" AND SpotCode='{0}'", _findCoditions.SpotCode);
            }
            # endregion

            # region 6.品种类别
            if (!string.IsNullOrEmpty(_findCoditions.VarietyType))
            {
                sbFilter.AppendFormat(" AND VarietyType='{0}'", _findCoditions.VarietyType);
            }
            #endregion

            # region 7.查询开始时间和结束时间

            sbFilter.AppendFormat(CommonDALBulidSQLScript.BuildWhereQueryBetwennTime(_findCoditions.StartTime, _findCoditions.EndTime, 30), "TradeTime");

            #endregion

            return sbFilter.ToString();
        }
        # endregion

        # region (NEW)期货历史成交查询过滤条件
        /// <summary>
        /// 期货历史成交查询过滤条件
        /// </summary>
        /// <param name="_findCoditions">过滤条件实体对象</param>
        /// <returns></returns>
        string BuildQhHistoryTradeQueryWhere(FuturesTradeConditionFindEntity _findCoditions)
        {
            #region old
            //string result = string.Empty;

            ////# region 委托单号（判断有无委托单号）

            ////if (!string.IsNullOrEmpty(_findCoditions.EntrustNumber))  //委托单号（当只根据委托单号查询时）
            ////{
            ////    result += string.Format("EntrustNumber='{0}'", _findCoditions.EntrustNumber);
            ////}
            ////else //当不带委托单号时
            ////{
            ////}

            //# region 0.资金帐户
            //if (!string.IsNullOrEmpty(_findCoditions.CapitalAccount))
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("CapitalAccount='{0}'", _findCoditions.CapitalAccount);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND CapitalAccount='{0}'", _findCoditions.CapitalAccount);
            //    }
            //}
            //# endregion

            //# region  1.买卖方向

            //if (_findCoditions.BuySellDirection != 0)
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("BuySellId='{0}'", _findCoditions.BuySellDirection);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND BuySellId='{0}'", _findCoditions.BuySellDirection);
            //    }
            //}

            //# endregion

            //# region 2.所属市场
            //if (_findCoditions.BelongToMarket != 0)
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("OwnershipMarket='{0}'", _findCoditions.BelongToMarket);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND OwnershipMarket='{0}'", _findCoditions.BelongToMarket);
            //    }
            //}
            //# endregion

            //# region 3.成交类型
            //if (_findCoditions.TradeType != 0)
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("TradeTypeId='{0}'", _findCoditions.TradeType);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND TradeTypeId='{0}'", _findCoditions.TradeType);
            //    }
            //}
            //# endregion

            //# region 4.币种赋值
            //if (_findCoditions.CurrencyTypeId != 0)
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("CurrencyTypeId='{0}'", _findCoditions.CurrencyTypeId);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND CurrencyTypeId='{0}'", _findCoditions.CurrencyTypeId);
            //    }
            //}
            //# endregion

            //# region 5.合约代码
            //if (!string.IsNullOrEmpty(_findCoditions.ContractCode))
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("ContractCode='{0}'", _findCoditions.ContractCode);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND ContractCode='{0}'", _findCoditions.ContractCode);
            //    }
            //}
            //# endregion

            //# region 6.品种类别
            //if (_findCoditions.VarietyType != 0)
            //{
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("VarietyTypeId='{0}'", _findCoditions.VarietyType);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND VarietyTypeId='{0}'", _findCoditions.VarietyType);
            //    }
            //}
            //#endregion

            //# region 7.查询开始时间和结束时间
            //if (_findCoditions.StartTime != null && _findCoditions.EndTime != null)//起始和结束时间均已赋值
            //{
            //    if (DateTime.Compare(_findCoditions.StartTime, _findCoditions.EndTime) <= 0) //起始时间小于结束时间（比如2008-05-06小于2008-05-28）
            //    {
            //        _findCoditions.EndTime = _findCoditions.EndTime.AddDays(1).Date;
            //        if (string.IsNullOrEmpty(result))
            //        {
            //            result += string.Format("TradeTime >= '{0}' AND  TradeTime< '{1}'", _findCoditions.StartTime, _findCoditions.EndTime);
            //        }
            //        else
            //        {
            //            result += string.Format(" AND (TradeTime >= '{0}' AND  TradeTime< '{1}')", _findCoditions.StartTime, _findCoditions.EndTime);
            //        }
            //    }
            //    else //起始时间大于或等于结束时间，就默认查出最近一个月的（从昨天开始往前推一个月）
            //    {
            //        //要得到上个月方法是：当月减去一个月
            //        //DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-1).ToShortDateString();

            //        //查询开始时间为 ：上一个月的今天
            //        _findCoditions.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddMonths(-1).ToShortDateString());

            //        //结束时间为：今天 00：00：00
            //        _findCoditions.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(0).ToShortDateString());

            //        if (string.IsNullOrEmpty(result))
            //        {
            //            result += string.Format("TradeTime >= '{0}' AND  TradeTime< '{1}'", _findCoditions.StartTime, _findCoditions.EndTime);
            //        }
            //        else
            //        {
            //            result += string.Format(" AND (TradeTime >= '{0}' AND  TradeTime< '{1}')", _findCoditions.StartTime, _findCoditions.EndTime);
            //        }
            //    }
            //}
            //else //若起始时间或结束时间未赋值，就默认查出最近一个月的（从昨天开始往前推一个月）
            //{
            //    //C#中用DateTime取出日期时间.怎么才能转换为只有日期没有时间? 就是只要年月日,不要时间..要怎么做??
            //    //解决方法如下：
            //    //DateTime dt=new DateTime(); 
            //    //dt.ToString("yyyy-MM-dd");

            //    //查询开始时间为 ：上一个月的今天
            //    _findCoditions.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddMonths(-1).ToShortDateString());

            //    //结束时间为：今天 00：00：00
            //    _findCoditions.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(0).ToShortDateString());

            //    if (string.IsNullOrEmpty(result))
            //    {
            //        result += string.Format("TradeTime >= '{0}' AND  TradeTime< '{1}'", _findCoditions.StartTime, _findCoditions.EndTime);
            //    }
            //    else
            //    {
            //        result += string.Format(" AND (TradeTime >= '{0}' AND  TradeTime< '{1}')", _findCoditions.StartTime, _findCoditions.EndTime);
            //    }
            //}
            //#endregion

            //return result;
            #endregion

            StringBuilder sbFilter = new StringBuilder("1=1 ");

            # region 0.资金帐户
            if (!string.IsNullOrEmpty(_findCoditions.CapitalAccount))
            {
                sbFilter.AppendFormat(" AND CapitalAccount='{0}'", _findCoditions.CapitalAccount);
            }
            # endregion

            # region  1.买卖方向

            if (_findCoditions.BuySellDirection != 0)
            {
                sbFilter.AppendFormat(" AND BuySellId='{0}'", _findCoditions.BuySellDirection);
            }

            # endregion

            # region 2.所属市场
            if (_findCoditions.BelongToMarket != 0)
            {
                sbFilter.AppendFormat(" AND OwnershipMarket='{0}'", _findCoditions.BelongToMarket);
            }
            # endregion

            # region 3.成交类型
            if (_findCoditions.TradeType != 0)
            {
                sbFilter.AppendFormat(" AND TradeTypeId='{0}'", _findCoditions.TradeType);
            }
            # endregion

            # region 4.币种赋值
            if (_findCoditions.CurrencyTypeId != 0)
            {
                sbFilter.AppendFormat(" AND CurrencyTypeId='{0}'", _findCoditions.CurrencyTypeId);
            }
            # endregion

            # region 5.合约代码
            if (!string.IsNullOrEmpty(_findCoditions.ContractCode))
            {
                sbFilter.AppendFormat(" AND ContractCode='{0}'", _findCoditions.ContractCode);
            }
            # endregion

            # region 6.品种类别
            if (_findCoditions.VarietyType != 0)
            {
                sbFilter.AppendFormat(" AND VarietyTypeId='{0}'", _findCoditions.VarietyType);
            }
            #endregion

            # region 7.查询开始时间和结束时间

            sbFilter.AppendFormat(CommonDALBulidSQLScript.BuildWhereQueryBetwennTime(_findCoditions.StartTime, _findCoditions.EndTime, 30), "TradeTime");

            #endregion

            return sbFilter.ToString();
        }

        # endregion

        # region (NEW)现货当日委托查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// <summary>
        /// 现货当日委托查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strPassword">交易员密码</param>
        /// <param name="findCondition">查询条件</param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<XH_TodayEntrustTableInfo> SpotTodayEntrustFindByXhCapitalAccount(string capitalAccount, string strPassword,
            SpotEntrustConditionFindEntity findCondition, int start, int pageLength, out int count, ref string strErrorMessage)
        {
            List<XH_TodayEntrustTableInfo> list = new List<XH_TodayEntrustTableInfo>();
            count = 0;
            XH_TodayEntrustTableDal dal = new XH_TodayEntrustTableDal();
            try
            {
                #region 如果过滤条件为null只根据资金账号查询 ---2010-06-01 不能这样查询返回数据量太大
                //if (findCondition == null)
                //{
                //    count = 1;
                //    //list = DataRepository.XhTodayEntrustTableProvider.GetByCapitalAccount(capitalAccount);
                //    list = dal.GetListArray(" CapitalAccount='" + capitalAccount + "'");
                //    if (!Utils.IsNullOrEmpty(list))
                //    {
                //        count = list.Count;
                //    }
                //    return list;
                //}
                #endregion

                if (findCondition == null)
                {
                    findCondition = new SpotEntrustConditionFindEntity();
                }


                #region 如果过滤条件不为null只根据委托编号查询，因为委托编号是维一的
                if (!String.IsNullOrEmpty(findCondition.EntrustNumber))
                {
                    // XhTodayEntrustTable tet = GetTodayEntrustTable(findCondition.EntrustNumber, ref strErrorMessage);
                    XH_TodayEntrustTableInfo tet = dal.GetModel(findCondition.EntrustNumber.Trim());
                    if (tet != null)
                    {
                        list.Add(tet);
                        count = 1;
                    }
                    return list;
                }
                #endregion

                #region 以上两个条件都不满足根据条件和资金账号查询
                //将资金账号加入查询条件中
                findCondition.SpotCapitalAccount = capitalAccount;
                string whereCondition = BuildXHOrderQueryWhere(findCondition);

                #region 分页存储过程相关信息组装
                PagingProceduresInfo ppInfo = new PagingProceduresInfo();
                ppInfo.IsCount = true;
                ppInfo.PageNumber = start;
                ppInfo.PageSize = pageLength;
                ppInfo.Fields = " EntrustNumber,PortfolioLogo,EntrustPrice,EntrustAmount,SpotCode,TradeAmount,TradeAveragePrice,CancelAmount,CancelLogo,BuySellTypeId,StockAccount,CapitalAccount,OrderStatusId,IsMarketValue,OrderMessage,CurrencyTypeId,TradeUnitId,CallbackChannlId,McOrderId,HasDoneProfit,OfferTime,EntrustTime";

                ppInfo.PK = "EntrustNumber";
                ppInfo.Sort = " EntrustTime desc ";

                ppInfo.Tables = "XH_TodayEntrustTable";
                #endregion

                #region 过滤条件组装
                ppInfo.Filter = whereCondition;
                #endregion
                // result = DataRepository.XhTodayEntrustTableProvider.GetPaged(whereCondition, "EntrustTime ASC", start, pageLength, out count);
                CommonDALOperate<XH_TodayEntrustTableInfo> com = new CommonDALOperate<XH_TodayEntrustTableInfo>();
                list = com.PagingQueryProcedures(ppInfo, out count, dal.ReaderBind);

                #endregion
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
        }

        /// <summary>
        /// 如果不再使用方法
        /// </summary>
        /// <param name="entrustnumber"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        private static XH_TodayEntrustTableInfo GetTodayEntrustTable(string entrustnumber, ref string strErrorMessage)
        {
            XH_TodayEntrustTableInfo tet = null;
            XH_TodayEntrustTableDal dal = new XH_TodayEntrustTableDal();
            try
            {
                //tet = DAL.DAO.XhTodayEntrustTableDao.GetModel(entrustnumber);
                tet = dal.GetModel(entrustnumber);
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
                LogHelper.WriteError(ex.Message, ex);
            }

            if (tet == null)
            {
                try
                {
                    //  tet = DAL.DAO.XhTodayEntrustTableDao.GetModelWithNoLock(entrustnumber);
                    tet = dal.GetModelWithNoLock(" EntrustNumber ='" + entrustnumber + "'");

                }
                catch (Exception ex)
                {
                    strErrorMessage = ex.Message;
                    LogHelper.WriteError(ex.Message, ex);
                }
            }

            return tet;
        }

        # endregion 现货当日委托查询

        # region (NEW)期货当日委托查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// <summary>
        /// 期货当日委托查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strPassword">交易员密码</param>
        /// <param name="findCondition">查询条件</param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<QH_TodayEntrustTableInfo> FuturesTodayEntrustFindByQhCapitalAccount(string capitalAccount, string strPassword,
            FuturesEntrustConditionFindEntity findCondition, int start, int pageLength, out int count, ref string strErrorMessage)
        {
            List<QH_TodayEntrustTableInfo> list = new List<QH_TodayEntrustTableInfo>();
            count = 0;
            QH_TodayEntrustTableDal dal = new QH_TodayEntrustTableDal();
            try
            {
                #region 如果过滤条件为null只根据资金账号查询  ---2010-06-01 不能这样查询返回数据量太大
                //if (findCondition == null)
                //{
                //    list = dal.GetListArray(" CapitalAccount='" + capitalAccount + "'");
                //    if (!Utils.IsNullOrEmpty(list))
                //    {
                //        count = list.Count;
                //    }
                //    return list;
                //}
                #endregion

                if (findCondition == null)
                {
                    findCondition = new FuturesEntrustConditionFindEntity();
                }


                #region 如果过滤条件不为null只根据委托编号查询，因为委托编号是维一的
                if (!String.IsNullOrEmpty(findCondition.EntrustNumber))
                {
                    QH_TodayEntrustTableInfo tet = dal.GetModel(findCondition.EntrustNumber.Trim());
                    if (tet != null)
                    {
                        list.Add(tet);
                        count = 1;
                    }
                    return list;
                }
                #endregion

                #region 以上两个条件都不满足根据条件和资金账号查询
                //将资金账号加入查询条件中
                findCondition.CapitalAccount = capitalAccount;

                string whereCondition = BuildQHOrderQueryWhere(findCondition);
                #region 分页存储过程相关信息组装
                PagingProceduresInfo ppInfo = new PagingProceduresInfo();
                ppInfo.IsCount = true;
                ppInfo.PageNumber = start;
                ppInfo.PageSize = pageLength;
                ppInfo.Fields = " EntrustNumber,PortfolioLogo,EntrustPrice,EntrustAmount,TradeAmount,TradeAveragePrice,CancelAmount,CancelLogo,BuySellTypeId,CurrencyTypeId,OpenCloseTypeId,TradeUnitId,OrderStatusId,ContractCode,TradeAccount,CapitalAccount,IsMarketValue,OrderMessage,CallbackChannelId,McOrderId,CloseFloatProfitLoss,CloseMarketProfitLoss,OfferTime,EntrustTime";
                ppInfo.PK = "EntrustNumber";
                ppInfo.Sort = " EntrustTime desc ";
                ppInfo.Tables = "QH_TodayEntrustTable";
                #endregion

                #region 过滤条件组装
                ppInfo.Filter = whereCondition;
                #endregion
                // result = DataRepository.QhTodayEntrustTableProvider.GetPaged(whereCondition, "EntrustTime ASC", start, pageLength, out count);
                CommonDALOperate<QH_TodayEntrustTableInfo> com = new CommonDALOperate<QH_TodayEntrustTableInfo>();
                list = com.PagingQueryProcedures(ppInfo, out count, dal.ReaderBind);

                #endregion
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
        }
        # endregion 现货当日委托查询

        # region (NEW)现货当日成交查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// <summary>
        /// 现货当日成交查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strPassword">交易员密码</param>
        /// <param name="findCondition">查询条件实体对象</param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<XH_TodayTradeTableInfo> SpotTodayTradeFindByXhCapitalAccount(string capitalAccount, string strPassword,
            SpotTradeConditionFindEntity findCondition, int start, int pageLength, out int count, out string strErrorMessage)
        {
            List<XH_TodayTradeTableInfo> list = new List<XH_TodayTradeTableInfo>(); ;
            strErrorMessage = string.Empty;
            count = 0;
            XH_TodayTradeTableDal dal = new XH_TodayTradeTableDal();
            try
            {
                #region 如果过滤条件为null只根据资金账号查询  ---2010-06-01 不能这样查询返回数据量太大
                //if (findCondition == null)
                //{
                //    list = dal.GetListArray(" CapitalAccount='" + capitalAccount + "'");
                //    if (!Utils.IsNullOrEmpty(list))
                //    {
                //        count = list.Count;
                //    }
                //    //list = DataRepository.XH_TodayTradeTableInfoProvider.GetByCapitalAccount(capitalAccount);
                //    return list;
                //}
                #endregion
                if (findCondition == null)
                {
                    findCondition = new SpotTradeConditionFindEntity();
                }

                //将资金账号加入查询条件中
                findCondition.SpotCapitalAccount = capitalAccount;

                string whereCondition = BuildXHTradeQueryWhere(findCondition);

                #region 分页存储过程相关信息组装
                PagingProceduresInfo ppInfo = new PagingProceduresInfo();
                ppInfo.IsCount = true;
                ppInfo.PageNumber = start;
                ppInfo.PageSize = pageLength;
                ppInfo.Fields = "TradeNumber,EntrustNumber,PortfolioLogo,TradePrice,TradeAmount,EntrustPrice,StampTax,Commission,TransferAccountFee,TradeProceduresFee,MonitoringFee,TradingSystemUseFee,TradeCapitalAmount,ClearingFee,StockAccount,CapitalAccount,SpotCode,TradeTypeId,TradeUnitId,BuySellTypeId,CurrencyTypeId,TradeTime ";
                ppInfo.PK = "TradeNumber";
                ppInfo.Sort = " TradeTime desc ";
                ppInfo.Tables = "XH_TodayTradeTable";
                #endregion
                #region 过滤条件组装
                ppInfo.Filter = whereCondition;
                #endregion
                CommonDALOperate<XH_TodayTradeTableInfo> com = new CommonDALOperate<XH_TodayTradeTableInfo>();
                list = com.PagingQueryProcedures(ppInfo, out count, dal.ReaderBind);
                //result = DataRepository.XH_TodayTradeTableInfoProvider.GetPaged(whereCondition, "TradeTime ASC", start, pageLength, out count);
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
        }

        # endregion 现货当日成交查询

        # region (NEW)期货当日成交查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// <summary>
        /// 期货当日成交查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strPassword">交易员密码</param>
        /// <param name="findCondition">查询条件实体对象</param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<QH_TodayTradeTableInfo> FuturesTodayTradeFindByXhCapitalAccount(string capitalAccount, string strPassword,
            FuturesTradeConditionFindEntity findCondition, int start, int pageLength, out int count, out string strErrorMessage)
        {
            List<QH_TodayTradeTableInfo> list = new List<QH_TodayTradeTableInfo>();
            strErrorMessage = string.Empty;
            QH_TodayTradeTableDal dal = new QH_TodayTradeTableDal();
            count = 0;
            try
            {
                #region 如果过滤条件为null只根据资金账号查询  ---2010-06-01 不能这样查询返回数据量太大
                //if (findCondition == null)
                //{
                //    list = dal.GetListArray(" CapitalAccount='" + capitalAccount + "'");
                //    if (!Utils.IsNullOrEmpty(list))
                //    {
                //        count = list.Count;
                //    }
                //    //list = DataRepository.XH_TodayTradeTableInfoProvider.GetByCapitalAccount(capitalAccount);
                //    return list;
                //}
                #endregion

                if (findCondition == null)
                {
                    findCondition = new FuturesTradeConditionFindEntity();
                }


                ////将资金账号加入查询条件中
                findCondition.CapitalAccount = capitalAccount;

                string whereCondition = BuildQHTradeQueryWhere(findCondition);
                #region 分页存储过程相关信息组装
                PagingProceduresInfo ppInfo = new PagingProceduresInfo();
                ppInfo.IsCount = true;
                ppInfo.PageNumber = start;
                ppInfo.PageSize = pageLength;
                ppInfo.Fields = "TradeNumber,EntrustNumber,PortfolioLogo,TradePrice,EntrustPrice,TradeAmount,TradeProceduresFee,Margin,ContractCode,TradeAccount,CapitalAccount,BuySellTypeId,OpenCloseTypeId,TradeUnitId,TradeTypeId,CurrencyTypeId,TradeTime,MarketProfitLoss ";
                ppInfo.PK = "TradeNumber";
                ppInfo.Sort = " TradeTime desc ";
                ppInfo.Tables = "QH_TodayTradeTable";
                #endregion

                #region 过滤条件组装
                ppInfo.Filter = whereCondition;
                #endregion
                CommonDALOperate<QH_TodayTradeTableInfo> com = new CommonDALOperate<QH_TodayTradeTableInfo>();
                list = com.PagingQueryProcedures(ppInfo, out count, dal.ReaderBind);
                //result = DataRepository.QhTodayTradeTableProvider.GetPaged(whereCondition, "TradeTime ASC", start, pageLength, out count);
                //result = ReckoningCounter.DAL.AccountManagementAndFindDAL.SearchRecord.FuturesTodayTradeFind(whereCondition, start, pageLength, out count);
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
        }

        # endregion 现货当日成交查询

        # region (NEW)现货历史委托查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// <summary>
        /// 现货历史委托查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strPassword">交易员密码</param>
        /// <param name="findCondition">查询条件</param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<XH_HistoryEntrustTableInfo> SpotHistoryEntrustFindByXhCapitalAccount(string capitalAccount, string strPassword,
            SpotEntrustConditionFindEntity findCondition, int start, int pageLength, out int count, ref string strErrorMessage)
        {
            List<XH_HistoryEntrustTableInfo> list = new List<XH_HistoryEntrustTableInfo>();
            XH_HistoryEntrustTableDal dal = new XH_HistoryEntrustTableDal();
            count = 0;
            try
            {
                #region 如果过滤条件为null只根据资金账号查询 ---2010-06-01 不能这样查询返回数据量太大
                //if (findCondition == null)
                //{
                //    list = dal.GetListArray(" CapitalAccount='" + capitalAccount + "'");
                //    if (!Utils.IsNullOrEmpty(list))
                //    {
                //        count = list.Count;
                //    }
                //    return list;
                //}
                #endregion

                if (findCondition == null)
                {
                    findCondition = new SpotEntrustConditionFindEntity();
                }


                //将资金账号加入查询条件中
                findCondition.SpotCapitalAccount = capitalAccount;
                // 现货历史委托查询过滤条件
                //string BuildXhHistoryOrderQueryWhere(SpotEntrustConditionFindEntity _findCoditions)
                string whereCondition = BuildXhHistoryOrderQueryWhere(findCondition);

                LogHelper.WriteDebug("现货历史委托查询:" + whereCondition);
                #region 分页存储过程相关信息组装
                PagingProceduresInfo ppInfo = new PagingProceduresInfo();
                ppInfo.IsCount = true;
                ppInfo.PageNumber = start;
                ppInfo.PageSize = pageLength;
                ppInfo.Fields = " EntrustNumber,PortfolioLogo,EntrustPrice,EntrustMount,SpotCode,TradeAmount,TradeAveragePrice,IsMarketValue,CancelAmount,BuySellTypeId,StockAccount,CapitalAccount,CurrencyTypeId,TradeUnitId,OrderStatusId,OrderMessage,McOrderId,HasDoneProfit,OfferTime,EntrustTime  ";
                ppInfo.PK = "EntrustNumber";
                ppInfo.Sort = " EntrustTime desc ";
                ppInfo.Tables = "XH_HistoryEntrustTable";
                #endregion

                #region 过滤条件组装
                ppInfo.Filter = whereCondition;
                #endregion

                CommonDALOperate<XH_HistoryEntrustTableInfo> com = new CommonDALOperate<XH_HistoryEntrustTableInfo>();
                list = com.PagingQueryProcedures(ppInfo, out count, dal.ReaderBind);
                //result = DataRepository.XhTodayEntrustTableProvider.GetPaged(whereCondition, "EntrustTime DESC", start, pageLength, out count);
                //result = DataRepository.XhHistoryEntrustTableProvider.GetPaged(whereCondition, "EntrustTime ASC", start, pageLength, out count);
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
        }
        # endregion 现货当日委托查询

        # region (NEW)期货历史委托查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// <summary>
        /// 期货历史委托查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strPassword">交易员密码</param>
        /// <param name="findCondition">查询条件</param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<QH_HistoryEntrustTableInfo> FuturesHistoryEntrustFindByQhCapitalAccount(string capitalAccount, string strPassword,
            FuturesEntrustConditionFindEntity findCondition, int start, int pageLength, out int count, ref string strErrorMessage)
        {
            List<QH_HistoryEntrustTableInfo> list = new List<QH_HistoryEntrustTableInfo>();
            QH_HistoryEntrustTableDal dal = new QH_HistoryEntrustTableDal();
            count = 0;
            try
            {

                #region 如果过滤条件为null只根据资金账号查询 ---2010-06-01 不能这样查询返回数据量太大
                //if (findCondition == null)
                //{
                //    list = dal.GetListArray(" CapitalAccount='" + capitalAccount + "'");
                //    if (!Utils.IsNullOrEmpty(list))
                //    {
                //        count = list.Count;
                //    }
                //    return list;
                //}
                #endregion

                if (findCondition == null)
                {
                    findCondition = new FuturesEntrustConditionFindEntity();
                }

                ////将资金账号加入查询条件中
                findCondition.CapitalAccount = capitalAccount;

                // 现货历史委托查询过滤条件
                //string BuildXhHistoryOrderQueryWhere(SpotEntrustConditionFindEntity _findCoditions)
                string whereCondition = BuildQhHistoryOrderQueryWhere(findCondition);
                LogHelper.WriteDebug("期货历史委托查询:" + whereCondition);
                #region 分页存储过程相关信息组装
                PagingProceduresInfo ppInfo = new PagingProceduresInfo();
                ppInfo.IsCount = true;
                ppInfo.PageNumber = start;
                ppInfo.PageSize = pageLength;

                ppInfo.Fields = "EntrustNumber,PortfolioLogo,EntrustPrice,EntrustAmount,ContractCode,TradeAmount,TradeAveragePrice,CancelAmount,TradeAccount,CapitalAccount,BuySellTypeId,CurrencyTypeId,TradeUnitId,OpenCloseTypeId,OrderStatusId,IsMarketValue,OrderMessage,McOrderId,CloseFloatProfitLoss,CloseMarketProfitLoss,OfferTime,EntrustTime";
                ppInfo.PK = "EntrustNumber";
                ppInfo.Sort = " EntrustTime desc ";
                ppInfo.Tables = "QH_HistoryEntrustTable";
                #endregion

                #region 过滤条件组装
                ppInfo.Filter = whereCondition;
                #endregion
                CommonDALOperate<QH_HistoryEntrustTableInfo> com = new CommonDALOperate<QH_HistoryEntrustTableInfo>();
                list = com.PagingQueryProcedures(ppInfo, out count, dal.ReaderBind);
                //result = DataRepository.XhTodayEntrustTableProvider.GetPaged(whereCondition, "EntrustTime DESC", start, pageLength, out count);
                // result = DataRepository.QhHistoryEntrustTableProvider.GetPaged(whereCondition, "EntrustTime ASC", start, pageLength, out count);
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
        }
        # endregion 现货当日委托查询

        # region (NEW)现货历史成交查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// <summary>
        /// 现货历史成交查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strPassword">交易员密码</param>
        /// <param name="findCondition">查询条件实体对象</param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<XH_HistoryTradeTableInfo> SpotHistoryTradeFindByXhCapitalAccount(string capitalAccount, string strPassword,
            SpotTradeConditionFindEntity findCondition, int start, int pageLength, out int count, ref string strErrorMessage)
        {
            List<XH_HistoryTradeTableInfo> list = new List<XH_HistoryTradeTableInfo>();
            XH_HistoryTradeTableDal dal = new XH_HistoryTradeTableDal();
            count = 0;
            try
            {
                #region 如果过滤条件为null只根据资金账号查询---2010-06-01 不能这样查询返回数据量太大
                //if (findCondition == null)
                //{
                //    list = dal.GetListArray(" CapitalAccount='" + capitalAccount + "'");
                //    if (!Utils.IsNullOrEmpty(list))
                //    {
                //        count = list.Count;
                //    }
                //    return list;
                //}
                #endregion
                if (findCondition == null)
                {
                    findCondition = new SpotTradeConditionFindEntity();
                }

                //将资金账号加入查询条件中
                findCondition.SpotCapitalAccount = capitalAccount;
                // 现货历史成交查询过滤条件
                //string BuildXhHistoryTradeQueryWhere(SpotTradeConditionFindEntity _findCoditions)
                string whereCondition = BuildXhHistoryTradeQueryWhere(findCondition);

                LogHelper.WriteDebug("现货历史成交查询" + whereCondition);

                #region 分页存储过程相关信息组装
                PagingProceduresInfo ppInfo = new PagingProceduresInfo();
                ppInfo.IsCount = true;
                ppInfo.PageNumber = start;
                ppInfo.PageSize = pageLength;
                ppInfo.Fields = "  TradeNumber,EntrustNumber,EntrustPrice,TradePrice,PortfolioLogo,TradeAmount,StampTax,Commission,SpotCode,TransferAccountFee,TradeProceduresFee,MonitoringFee,TradingSystemUseFee,ClearingFee,StockAccount,CapitalAccount,TradeTypeId,TradeUnitId,BuySellTypeId,CurrencyTypeId,TradeTime ";
                ppInfo.PK = "TradeNumber";
                ppInfo.Sort = " TradeTime desc ";
                ppInfo.Tables = "XH_HistoryTradeTable";
                #endregion

                #region 过滤条件组装
                ppInfo.Filter = whereCondition;
                #endregion
                CommonDALOperate<XH_HistoryTradeTableInfo> com = new CommonDALOperate<XH_HistoryTradeTableInfo>();
                list = com.PagingQueryProcedures(ppInfo, out count, dal.ReaderBind);
                // result = DataRepository.XhHistoryTradeTableProvider.GetPaged(whereCondition, "TradeTime ASC", start, pageLength, out count);
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
        }

        # endregion 现货历史成交查询

        # region (NEW)期货历史成交查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// <summary>
        /// 期货历史成交查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strPassword">交易员密码</param>
        /// <param name="findCondition">查询条件实体对象</param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<QH_HistoryTradeTableInfo> FuturesHistoryTradeFindByQHCapitalAccount(string capitalAccount, string strPassword,
            FuturesTradeConditionFindEntity findCondition, int start, int pageLength, out int count, ref string strErrorMessage)
        {
            List<QH_HistoryTradeTableInfo> list = new List<QH_HistoryTradeTableInfo>();
            QH_HistoryTradeTableDal dal = new QH_HistoryTradeTableDal();
            count = 0;
            try
            {

                #region 如果过滤条件为null只根据资金账号查询--update 2010-06-01不能这样查询，返回数据太大
                //if (findCondition == null)
                //{
                //    list = dal.GetListArray(" CapitalAccount='" + capitalAccount + "'");
                //    if (!Utils.IsNullOrEmpty(list))
                //    {
                //        count = list.Count;
                //    }
                //    return list;
                //}
                #endregion
                //将资金账号加入查询条件中
                if (findCondition == null)
                {
                    findCondition = new FuturesTradeConditionFindEntity();
                }

                findCondition.CapitalAccount = capitalAccount;

                //期货历史成交查询过滤条件
                //string BuildQhHistoryTradeQueryWhere(FuturesTradeConditionFindEntity findCoditions)
                #region 分页存储过程相关信息组装
                PagingProceduresInfo ppInfo = new PagingProceduresInfo();
                ppInfo.IsCount = true;
                ppInfo.PageNumber = start;
                ppInfo.PageSize = pageLength;
                ppInfo.Fields = " TradeNumber,EntrustNumber,PortfolioLogo,TradePrice,EntrustPrice,TradeAmount,TradeProceduresFee,Margin,ContractCode,TradeAccount,CapitalAccount,BuySellTypeId,OpenCloseTypeId,TradeUnitId,TradeTypeId,CurrencyTypeId,TradeTime,MarketProfitLoss ";
                ppInfo.PK = "TradeNumber";
                ppInfo.Sort = " TradeTime desc ";
                ppInfo.Tables = "QH_HistoryTradeTable";
                #endregion

                #region 过滤条件组装
                string whereCondition = BuildQhHistoryTradeQueryWhere(findCondition);

                LogHelper.WriteDebug("期货历史成交查询:" + whereCondition);
                ppInfo.Filter = whereCondition;
                #endregion
                CommonDALOperate<QH_HistoryTradeTableInfo> com = new CommonDALOperate<QH_HistoryTradeTableInfo>();
                list = com.PagingQueryProcedures(ppInfo, out count, dal.ReaderBind);
                // result = ReckoningCounter.DAL.AccountManagementAndFindDAL.SearchRecord.FuturesHistoryTradeFind(whereCondition, start, pageLength, out count);
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
        }

        # endregion 现货历史成交查询

        #region create by :李健华 Date:2009-07-09

        #region 现货今日/历史【委托】分页查询

        #region 现货【今日】委托查询 根据用户和密码、过滤条件查询该用户所拥有的现货资金帐户今日现货委托信息

        /// <summary>
        ///  Title:根据用户和密码查询该用户所拥有的现货资金帐户今日期货委托信息
        ///  Des.:此方法因为一个用户（交易员）可能拥有多个现货资金账号(如：-商品现货资金帐号,-股指现货资金帐号)
        ///  所以返回的数据会回多个资金账号的数据
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码--如果密码为空时不验证密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤条件数据</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">返回数页数</param>
        /// <param name="errorMsg">异常信息</param>
        /// <returns>返回当日期货委托信息</returns>
        public List<XH_TodayEntrustTableInfo> PagingXH_TodayEntrustByFilter(string userID, string pwd, int accountType, SpotEntrustConditionFindEntity filter, PagingInfo pageInfo, out int total, out string errorMsg)
        {
            List<XH_TodayEntrustTableInfo> list = null;
            XH_TodayEntrustTableDal dal = new XH_TodayEntrustTableDal();
            errorMsg = "";
            total = 0;

            #region 密码不为空时先验证用户
            if (string.IsNullOrEmpty(userID))
            {
                errorMsg = "查询失败！失败原因为：交易员ID不能为空！";
                return list;
            }
            if (!string.IsNullOrEmpty(pwd))
            {
                UA_UserBasicInformationTableDal usDal = new UA_UserBasicInformationTableDal();
                if (!usDal.Exists(userID, pwd))
                {
                    errorMsg = "查询失败！失败原因为：交易员ID或密码输入错误 ！";
                    return list;
                }
            }
            #endregion

            if (filter != null && !string.IsNullOrEmpty(filter.EntrustNumber))  //委托单号（当只根据委托单号查询时）
            {
                #region 如果有委托单号直接查询唯一记录
                list = dal.GetListArray(" EntrustNumber='" + filter.EntrustNumber + "'");
                if (!Utils.IsNullOrEmpty(list))
                {
                    total = 1;
                }
                #endregion
            }
            else //当不带委托单号时
            {
                #region 如果分页信息为空返回异常
                if (pageInfo == null)
                {
                    errorMsg = "分页信息不能为空!";
                    return null;
                }
                #endregion

                #region 分页存储过程相关信息组装
                PagingProceduresInfo ppInfo = new PagingProceduresInfo();
                ppInfo.IsCount = pageInfo.IsCount;
                ppInfo.PageNumber = pageInfo.CurrentPage;
                ppInfo.PageSize = pageInfo.PageLength;
                ppInfo.Fields = " EntrustNumber,PortfolioLogo,EntrustPrice,EntrustAmount,SpotCode,TradeAmount,TradeAveragePrice,CancelAmount,CancelLogo,BuySellTypeId,StockAccount,CapitalAccount,OrderStatusId,IsMarketValue,OrderMessage,CurrencyTypeId,TradeUnitId,CallbackChannlId,McOrderId,HasDoneProfit,OfferTime,EntrustTime";

                ppInfo.PK = "EntrustNumber";
                if (pageInfo.Sort == 0)
                {
                    ppInfo.Sort = " EntrustTime asc ";
                }
                else
                {
                    ppInfo.Sort = " EntrustTime desc ";
                }
                ppInfo.Tables = "XH_TodayEntrustTable";
                #endregion

                #region 过滤条件组装
                ppInfo.Filter = BuildXHEntrustQueryWhere(filter, userID, accountType, true);
                #endregion

                #region 执行查询
                try
                {
                    CommonDALOperate<XH_TodayEntrustTableInfo> com = new CommonDALOperate<XH_TodayEntrustTableInfo>();
                    list = com.PagingQueryProcedures(ppInfo, out total, dal.ReaderBind);
                    //list = dal.PagingXH_TodayEntrustByFilter(ppInfo, out total);
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    LogHelper.WriteError(ex.ToString(), ex);
                }
                #endregion
            }
            return list;
        }

        #endregion

        #region 现货【历史】委托查询 根据用户和密码、过滤条件查询该用户所拥有的现货资金帐户历史现货委托信息

        /// <summary>
        ///  Title:根据用户和密码查询该用户所拥有的现货资金帐户历史期货委托信息
        ///  Des.:此方法因为一个用户（交易员）可能拥有多个现货资金账号(如：-商品现货资金帐号,-股指现货资金帐号)
        ///  所以返回的数据会回多个资金账号的数据
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码--如果密码为空时不验证密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤条件数据</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">返回数页数</param>
        /// <param name="errorMsg">异常信息</param>
        /// <returns>返回当日期货委托信息</returns>
        public List<XH_HistoryEntrustTableInfo> PagingXH_HistoryEntrustByFilter(string userID, string pwd, int accountType, SpotEntrustConditionFindEntity filter, PagingInfo pageInfo, out int total, out string errorMsg)
        {
            List<XH_HistoryEntrustTableInfo> list = null;
            XH_HistoryEntrustTableDal dal = new XH_HistoryEntrustTableDal();
            errorMsg = "";
            total = 0;

            #region 密码不为空时先验证用户
            if (string.IsNullOrEmpty(userID))
            {
                errorMsg = "查询失败！失败原因为：交易员ID不能为空！";
                return list;
            }
            if (!string.IsNullOrEmpty(pwd))
            {
                UA_UserBasicInformationTableDal usDal = new UA_UserBasicInformationTableDal();
                if (!usDal.Exists(userID, pwd))
                {
                    errorMsg = "查询失败！失败原因为：交易员ID或密码输入错误 ！";
                    return list;
                }
            }
            #endregion

            if (filter != null && !string.IsNullOrEmpty(filter.EntrustNumber))  //委托单号（当只根据委托单号查询时）
            {
                #region 如果有委托单号直接查询唯一记录
                list = dal.GetListArray(" EntrustNumber='" + filter.EntrustNumber + "'");
                if (!Utils.IsNullOrEmpty(list))
                {
                    total = 1;
                }
                #endregion
            }
            else //当不带委托单号时
            {
                #region 如果分页信息为空返回异常
                if (pageInfo == null)
                {
                    errorMsg = "分页信息不能为空!";
                    return null;
                }
                #endregion

                #region 分页存储过程相关信息组装
                PagingProceduresInfo ppInfo = new PagingProceduresInfo();
                ppInfo.IsCount = pageInfo.IsCount;
                ppInfo.PageNumber = pageInfo.CurrentPage;
                ppInfo.PageSize = pageInfo.PageLength;
                ppInfo.Fields = " EntrustNumber,PortfolioLogo,EntrustPrice,EntrustMount,SpotCode,TradeAmount,TradeAveragePrice,IsMarketValue,CancelAmount,BuySellTypeId,StockAccount,CapitalAccount,CurrencyTypeId,TradeUnitId,OrderStatusId,OrderMessage,McOrderId,HasDoneProfit,OfferTime,EntrustTime  ";

                ppInfo.PK = "EntrustNumber";
                if (pageInfo.Sort == 0)
                {
                    ppInfo.Sort = " EntrustTime asc ";
                }
                else
                {
                    ppInfo.Sort = " EntrustTime desc ";
                }
                ppInfo.Tables = "XH_HistoryEntrustTable";
                #endregion

                #region 过滤条件组装
                ppInfo.Filter = BuildXHEntrustQueryWhere(filter, userID, accountType, false);
                #endregion

                #region 执行查询
                try
                {
                    CommonDALOperate<XH_HistoryEntrustTableInfo> com = new CommonDALOperate<XH_HistoryEntrustTableInfo>();
                    list = com.PagingQueryProcedures(ppInfo, out total, dal.ReaderBind);
                    //list = dal.PagingXH_HistoryEntrustByFilter(ppInfo, out total);
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    LogHelper.WriteError(ex.ToString(), ex);
                }
                #endregion
            }
            return list;
        }

        #endregion

        #region 组装现货委托过滤条件
        /// <summary>
        /// 组装现货委托过滤条件
        /// </summary>
        /// <param name="filter">现货委托过滤条件实体</param>
        /// <param name="userID">用户ID</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="isToday">是否为查询今日委托</param>
        /// <returns></returns>
        string BuildXHEntrustQueryWhere(SpotEntrustConditionFindEntity filter, string userID, int accountType, bool isToday)
        {
            #region 过滤条件组装
            StringBuilder sbFilter = new StringBuilder("1=1 ");
            if (filter != null)
            {
                # region  0.资金账号
                if (!string.IsNullOrEmpty(filter.SpotCapitalAccount))
                {
                    sbFilter.AppendFormat(" AND CapitalAccount='{0}'", filter.SpotCapitalAccount.Trim());
                }
                else //不指定可能有多个账号
                {
                    sbFilter.Append(" And CapitalAccount in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable   ");
                    if (accountType == 0)
                    {
                        sbFilter.AppendFormat("  where accounttypelogo in (select accounttypelogo from BD_AccountType where atcid='{0}')  and userid='{1}')", (int)Types.AccountAttributionType.SpotCapital, userID);
                    }
                    else
                    {
                        sbFilter.AppendFormat("  where accounttypelogo='{0}'  and userid='{1}')", accountType, userID);
                    }
                }
                # endregion

                # region  1.买卖方向

                if (filter.BuySellDirection != 0)
                {
                    sbFilter.AppendFormat(" AND BuySellTypeId='{0}'", filter.BuySellDirection);
                }

                # endregion

                # region 2.可撤标识
                if (isToday) //历史已经没有这个字段
                {
                    if (filter.CanBeWithdrawnLogo != 0)
                    {
                        sbFilter.AppendFormat(" AND CancelLogo='{0}'", filter.CanBeWithdrawnLogo);
                    }
                }
                # endregion

                # region 3.委托状态
                if (filter.EntrustState != 0)
                {
                    sbFilter.AppendFormat(" AND OrderStatusId='{0}'", filter.EntrustState);
                }
                # endregion

                # region 4.币种赋值
                if (filter.CurrencyType != 0)
                {
                    sbFilter.AppendFormat(" AND CurrencyTypeId='{0}'", filter.CurrencyType);
                }
                # endregion

                # region 5.现货代码
                if (!string.IsNullOrEmpty(filter.SpotCode))
                {
                    sbFilter.AppendFormat(" AND SpotCode='{0}'", filter.SpotCode);
                }
                # endregion

                # region 6.查询开始时间和结束时间
                if (!isToday) //为当日成交就不分时间查询
                {
                    sbFilter.AppendFormat(CommonDALBulidSQLScript.BuildWhereQueryBetwennTime(filter.StartTime, filter.EndTime, 30), "EntrustTime");

                    #region old code
                    //if (filter.StartTime != null && filter.EndTime != null)//起始和结束时间均已赋值
                    //{
                    //    if (DateTime.Compare(filter.StartTime, filter.EndTime) < 0) //起始时间小于结束时间（比如2008-05-06小于2008-05-28）
                    //    {
                    //        sbFilter.AppendFormat(" AND (EntrustTime>= '{0}' AND  EntrustTime<'{1}')", filter.StartTime.ToString("yyyy-MM-dd"), filter.EndTime.AddDays(1).ToString("yyyy-MM-dd"));
                    //    }
                    //    else //起始时间大于或等于结束时间，就默认查出最近一个月的（从昨天开始往前推一个月）
                    //    {
                    //        //查询开始时间为 ：上一个月的今天
                    //        filter.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddMonths(-1).ToShortDateString());
                    //        //结束时间为：今天 00：00：00
                    //        filter.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(1).ToShortDateString());
                    //        sbFilter.AppendFormat(" AND (EntrustTime>= '{0}' AND  EntrustTime<'{1}')", filter.StartTime, filter.EndTime);
                    //    }
                    //}
                    //else //若起始时间或结束时间未赋值，就默认查出最近一个月的（从昨天开始往前推一个月）
                    //{
                    //    //查询开始时间为 ：上一个月的今天
                    //    filter.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddMonths(-1).ToShortDateString());
                    //    //结束时间为：今天 00：00：00
                    //    filter.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(1).ToShortDateString());
                    //    sbFilter.AppendFormat(" AND (EntrustTime>= '{0}' AND  EntrustTime<'{1}')", filter.StartTime, filter.EndTime);
                    //}
                    #endregion
                }
                #endregion

            }
            else
            {
                //条件为空直接查询用户所对应拥有的资金账户下的今日成交
                sbFilter.Append(" And CapitalAccount in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable   ");
                if (accountType == 0)
                {
                    sbFilter.AppendFormat("  where accounttypelogo in (select accounttypelogo from BD_AccountType where atcid='{0}')  and userid='{1}')", (int)Types.AccountAttributionType.SpotCapital, userID);
                }
                else
                {
                    sbFilter.AppendFormat("  where accounttypelogo='{0}'  and userid='{1}')", accountType, userID);
                }
            }
            #endregion
            return sbFilter.ToString();
        }
        #endregion

        #endregion

        #region 期货今日/历史委托分页查询

        #region 期货资金帐户今日委托信息分页查询--根据用户和密码查询该用户所拥有的期货资金帐户今日期货委托信息
        /// <summary>
        ///  Title:根据用户和密码查询该用户所拥有的期货资金帐户今日期货委托信息
        ///  Des.:此方法因为一个用户（交易员）可能拥有多个现货资金账号(如：-商品期货资金帐号,-股指期货资金帐号)
        ///  所以返回的数据会回多个资金账号的数据
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码--如果密码为空时不验证密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤条件数据</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">返回数页数</param>
        /// <param name="errorMsg">异常信息</param>
        /// <returns>返回当日期货委托信息</returns>
        public List<QH_TodayEntrustTableInfo> PagingQH_TodayEntrustByFilter(string userID, string pwd, int accountType, FuturesEntrustConditionFindEntity filter, PagingInfo pageInfo, out int total, out string errorMsg)
        {
            List<QH_TodayEntrustTableInfo> list = null;
            QH_TodayEntrustTableDal dal = new QH_TodayEntrustTableDal();
            errorMsg = "";
            total = 0;

            #region 密码不为空时先验证用户
            if (string.IsNullOrEmpty(userID))
            {
                errorMsg = "查询失败！失败原因为：交易员ID不能为空！";
                return list;
            }
            if (!string.IsNullOrEmpty(pwd))
            {
                UA_UserBasicInformationTableDal usDal = new UA_UserBasicInformationTableDal();
                if (!usDal.Exists(userID, pwd))
                {
                    errorMsg = "查询失败！失败原因为：交易员ID或密码输入错误 ！";
                    return list;
                }
            }
            #endregion

            if (filter != null && !string.IsNullOrEmpty(filter.EntrustNumber))  //委托单号（当只根据委托单号查询时）
            {
                #region 如果有委托单号直接查询唯一记录
                list = dal.GetListArray(" EntrustNumber='" + filter.EntrustNumber + "'");
                if (!Utils.IsNullOrEmpty(list))
                {
                    total = 1;
                }
                #endregion
            }
            else //当不带委托单号时
            {
                #region 如果分页信息为空返回异常
                if (pageInfo == null)
                {
                    errorMsg = "分页信息不能为空!";
                    return null;
                }
                #endregion

                #region 分页存储过程相关信息组装
                PagingProceduresInfo ppInfo = new PagingProceduresInfo();
                ppInfo.IsCount = pageInfo.IsCount;
                ppInfo.PageNumber = pageInfo.CurrentPage;
                ppInfo.PageSize = pageInfo.PageLength;
                ppInfo.Fields = " EntrustNumber,PortfolioLogo,EntrustPrice,EntrustAmount,TradeAmount,TradeAveragePrice,CancelAmount,CancelLogo,BuySellTypeId,CurrencyTypeId,OpenCloseTypeId,TradeUnitId,OrderStatusId,ContractCode,TradeAccount,CapitalAccount,IsMarketValue,OrderMessage,CallbackChannelId,McOrderId,CloseFloatProfitLoss,CloseMarketProfitLoss,OfferTime,EntrustTime";

                ppInfo.PK = "EntrustNumber";
                if (pageInfo.Sort == 0)
                {
                    ppInfo.Sort = " EntrustTime asc ";
                }
                else
                {
                    ppInfo.Sort = " EntrustTime desc ";
                }
                ppInfo.Tables = "QH_TodayEntrustTable";
                #endregion

                #region 过滤条件组装
                ppInfo.Filter = BuildQHEntrustQueryWhere(filter, userID, accountType, true);
                #endregion

                #region 执行查询
                try
                {
                    CommonDALOperate<QH_TodayEntrustTableInfo> com = new CommonDALOperate<QH_TodayEntrustTableInfo>();
                    list = com.PagingQueryProcedures(ppInfo, out total, dal.ReaderBind);
                    // list = dal.PagingQH_TodayEntrustByFilter(ppInfo, out total);
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    LogHelper.WriteError(ex.ToString(), ex);
                }
                #endregion
            }
            return list;
        }
        #endregion

        #region 期货资金帐户历史委托信息分页查询--根据用户和密码查询该用户所拥有的期货资金帐户历史期货委托信息
        /// <summary>
        ///  Title:根据用户和密码查询该用户所拥有的期货资金帐户今日期货委托信息
        ///  Des.:此方法因为一个用户（交易员）可能拥有多个现货资金账号(如：-商品期货资金帐号,-股指期货资金帐号)
        ///  所以返回的数据会回多个资金账号的数据
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码--如果密码为空时不验证密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤条件数据</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">返回数页数</param>
        /// <param name="errorMsg">异常信息</param>
        /// <returns>返回当日期货委托信息</returns>
        public List<QH_HistoryEntrustTableInfo> PagingQH_HistoryEntrustByFilter(string userID, string pwd, int accountType, FuturesEntrustConditionFindEntity filter, PagingInfo pageInfo, out int total, out string errorMsg)
        {
            List<QH_HistoryEntrustTableInfo> list = null;
            QH_HistoryEntrustTableDal dal = new QH_HistoryEntrustTableDal();
            errorMsg = "";
            total = 0;

            #region 密码不为空时先验证用户
            if (string.IsNullOrEmpty(userID))
            {
                errorMsg = "查询失败！失败原因为：交易员ID不能为空！";
                return list;
            }
            if (!string.IsNullOrEmpty(pwd))
            {
                UA_UserBasicInformationTableDal usDal = new UA_UserBasicInformationTableDal();
                if (!usDal.Exists(userID, pwd))
                {
                    errorMsg = "查询失败！失败原因为：交易员ID或密码输入错误 ！";
                    return list;
                }
            }
            #endregion

            if (filter != null && !string.IsNullOrEmpty(filter.EntrustNumber))  //委托单号（当只根据委托单号查询时）
            {
                #region 如果有委托单号直接查询唯一记录
                list = dal.GetListArray(" EntrustNumber='" + filter.EntrustNumber + "'");
                if (!Utils.IsNullOrEmpty(list))
                {
                    total = 1;
                }
                #endregion
            }
            else //当不带委托单号时
            {
                #region 如果分页信息为空返回异常
                if (pageInfo == null)
                {
                    errorMsg = "分页信息不能为空!";
                    return null;
                }
                #endregion

                #region 分页存储过程相关信息组装
                PagingProceduresInfo ppInfo = new PagingProceduresInfo();
                ppInfo.IsCount = pageInfo.IsCount;
                ppInfo.PageNumber = pageInfo.CurrentPage;
                ppInfo.PageSize = pageInfo.PageLength;

                ppInfo.Fields = "EntrustNumber,PortfolioLogo,EntrustPrice,EntrustAmount,ContractCode,TradeAmount,TradeAveragePrice,CancelAmount,TradeAccount,CapitalAccount,BuySellTypeId,CurrencyTypeId,TradeUnitId,OpenCloseTypeId,OrderStatusId,IsMarketValue,OrderMessage,McOrderId,CloseFloatProfitLoss,CloseMarketProfitLoss,OfferTime,EntrustTime";

                ppInfo.PK = "EntrustNumber";
                if (pageInfo.Sort == 0)
                {
                    ppInfo.Sort = " EntrustTime asc ";
                }
                else
                {
                    ppInfo.Sort = " EntrustTime desc ";
                }
                ppInfo.Tables = "QH_HistoryEntrustTable";
                #endregion

                #region 过滤条件组装
                ppInfo.Filter = BuildQHEntrustQueryWhere(filter, userID, accountType, false);
                #endregion

                #region 执行查询
                try
                {
                    CommonDALOperate<QH_HistoryEntrustTableInfo> com = new CommonDALOperate<QH_HistoryEntrustTableInfo>();
                    list = com.PagingQueryProcedures(ppInfo, out total, dal.ReaderBind);
                    //list = dal.PagingQH_HistoryEntrustByFilter(ppInfo, out total);
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    LogHelper.WriteError(ex.ToString(), ex);
                }
                #endregion
            }
            return list;
        }
        #endregion

        #region 组装期货委托过滤条件
        /// <summary>
        /// 组装期货委托过滤条件
        /// </summary>
        /// <param name="filter">期货委托过滤条件实体</param>
        /// <param name="userID">用户ID</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="isToday">是否为查询今日委托</param>
        /// <returns></returns>
        string BuildQHEntrustQueryWhere(FuturesEntrustConditionFindEntity filter, string userID, int accountType, bool isToday)
        {
            #region 过滤条件组装
            StringBuilder sbFilter = new StringBuilder("1=1 ");
            if (filter != null)
            {
                # region  0.资金账号
                if (!string.IsNullOrEmpty(filter.CapitalAccount))
                {
                    sbFilter.AppendFormat(" AND CapitalAccount='{0}'", filter.CapitalAccount.Trim());
                }
                else //不指定可能有多个账号
                {
                    sbFilter.Append(" And CapitalAccount in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable   ");
                    if (accountType == 0)
                    {
                        sbFilter.AppendFormat("  where accounttypelogo in (select accounttypelogo from BD_AccountType where atcid='{0}')  and userid='{1}')", (int)Types.AccountAttributionType.FuturesCapital, userID);
                    }
                    else
                    {
                        sbFilter.AppendFormat("  where accounttypelogo='{0}'  and userid='{1}')", accountType, userID);
                    }

                }
                # endregion

                # region  1.买卖方向

                if (filter.BuySellDirection != 0)
                {
                    sbFilter.AppendFormat(" AND BuySellTypeId='{0}'", filter.BuySellDirection);
                }

                # endregion

                # region 2.可撤标识
                if (isToday) //历史已经没有这个字段
                {
                    if (filter.CanBeWithdrawnLogo != 0)
                    {
                        sbFilter.AppendFormat(" AND CancelLogo='{0}'", filter.CanBeWithdrawnLogo);
                    }
                }
                # endregion

                # region 3.委托状态
                if (filter.EntrustState != 0)
                {
                    sbFilter.AppendFormat(" AND OrderStatusId='{0}'", filter.EntrustState);
                }
                # endregion

                # region 4.币种赋值
                if (filter.CurrencyTypeId != 0)
                {
                    sbFilter.AppendFormat(" AND CurrencyTypeId='{0}'", filter.CurrencyTypeId);
                }
                # endregion

                # region 5.合约代码
                if (!string.IsNullOrEmpty(filter.ContractCode))
                {
                    sbFilter.AppendFormat(" AND ContractCode='{0}'", filter.ContractCode);
                }
                # endregion

                # region 6.查询时间为当日
                if (!isToday) //为当日成交就不分时间查询
                {
                    sbFilter.AppendFormat(CommonDALBulidSQLScript.BuildWhereQueryBetwennTime(filter.StartTime, filter.EndTime, 30), "EntrustTime");

                    #region old code
                    //if (filter.StartTime != null && filter.EndTime != null)//起始和结束时间均已赋值
                    //{
                    //    if (DateTime.Compare(filter.StartTime, filter.EndTime) < 0) //起始时间小于结束时间（比如2008-05-06小于2008-05-28）
                    //    {
                    //        sbFilter.AppendFormat(" AND (EntrustTime>= '{0}' AND  EntrustTime<'{1}')", filter.StartTime.ToString("yyyy-MM-dd"), filter.EndTime.AddDays(1).ToString("yyyy-MM-dd"));
                    //    }
                    //    else //起始时间大于或等于结束时间，就默认查出最近一个月的（从昨天开始往前推一个月）
                    //    {
                    //        //查询开始时间为 ：上一个月的今天
                    //        filter.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddMonths(-1).ToShortDateString());
                    //        //结束时间为：今天 00：00：00
                    //        filter.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(1).ToShortDateString());
                    //        sbFilter.AppendFormat(" AND (EntrustTime>= '{0}' AND  EntrustTime<'{1}')", filter.StartTime, filter.EndTime);
                    //    }
                    //}
                    //else //若起始时间或结束时间未赋值，就默认查出最近一个月的（从昨天开始往前推一个月）
                    //{
                    //    //查询开始时间为 ：上一个月的今天
                    //    filter.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddMonths(-1).ToShortDateString());
                    //    //结束时间为：今天 00：00：00
                    //    filter.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(1).ToShortDateString());
                    //    sbFilter.AppendFormat(" AND (EntrustTime>= '{0}' AND  EntrustTime<'{1}')", filter.StartTime, filter.EndTime);
                    //}
                    #endregion
                }
                # endregion

            }
            else
            {
                //条件为空直接查询用户所对应拥有的资金账户下的今日成交
                sbFilter.Append(" And CapitalAccount in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable   ");
                if (accountType == 0)
                {
                    sbFilter.AppendFormat("  where accounttypelogo in (select accounttypelogo from BD_AccountType where atcid='{0}')  and userid='{1}')", (int)Types.AccountAttributionType.FuturesCapital, userID);
                }
                else
                {
                    sbFilter.AppendFormat("  where accounttypelogo='{0}'  and userid='{1}')", accountType, userID);
                }
            }
            #endregion
            return sbFilter.ToString();
        }
        #endregion

        #endregion

        #region 现货今日/历史成交分页查询

        #region 现货资金帐户历史现货成交信息分页查询--根据用户和密码查询该用户所拥有的现货资金帐户历史现货成交信息
        /// <summary>
        ///  Title:根据用户和密码查询该用户所拥有的现货资金帐户历史现货成交信息
        ///  Des.:此方法因为一个用户（交易员）可能拥有多个现货资金账号(如：4-商品现货资金帐号,6-股指现货资金帐号)
        ///  所以返回的数据会回多个资金账号的数据
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码--如果密码为空时不验证密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤条件数据</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">返回数页数</param>
        /// <param name="errorMsg">异常信息</param>
        /// <returns>返回当日现货成交信息</returns>
        public List<XH_HistoryTradeTableInfo> PagingXH_HistoryTradeByFilter(string userID, string pwd, int accountType, SpotTradeConditionFindEntity filter, PagingInfo pageInfo, out int total, out string errorMsg)
        {
            List<XH_HistoryTradeTableInfo> list = null;
            XH_HistoryTradeTableDal dal = new XH_HistoryTradeTableDal();
            errorMsg = "";
            total = 0;

            #region 密码不为空时先验证用户
            if (string.IsNullOrEmpty(userID))
            {
                errorMsg = "查询失败！失败原因为：交易员ID不能为空！";
                return list;
            }
            if (!string.IsNullOrEmpty(pwd))
            {
                UA_UserBasicInformationTableDal usDal = new UA_UserBasicInformationTableDal();
                if (!usDal.Exists(userID, pwd))
                {
                    errorMsg = "查询失败！失败原因为：交易员ID或密码输入错误 ！";
                    return list;
                }
            }
            #endregion

            //string strFilter = "";
            if (filter != null && !string.IsNullOrEmpty(filter.EntrustNumber))  //委托单号（当只根据委托单号查询时）
            {
                #region 如果有委托单号直接查询唯一记录
                list = dal.GetListArray(" EntrustNumber='" + filter.EntrustNumber + "'");
                if (!Utils.IsNullOrEmpty(list))
                {
                    total = 1;
                }
                #endregion
            }
            else //当不带委托单号时
            {
                #region 如果分页信息为空返回异常
                if (pageInfo == null)
                {
                    errorMsg = "分页信息不能为空!";
                    return null;
                }
                #endregion

                #region 分页存储过程相关信息组装
                PagingProceduresInfo ppInfo = new PagingProceduresInfo();
                ppInfo.IsCount = pageInfo.IsCount;
                ppInfo.PageNumber = pageInfo.CurrentPage;
                ppInfo.PageSize = pageInfo.PageLength;
                ppInfo.Fields = "  TradeNumber,EntrustNumber,EntrustPrice,TradePrice,PortfolioLogo,TradeAmount,StampTax,Commission,SpotCode,TransferAccountFee,TradeProceduresFee,MonitoringFee,TradingSystemUseFee,ClearingFee,StockAccount,CapitalAccount,TradeTypeId,TradeUnitId,BuySellTypeId,CurrencyTypeId,TradeTime ";

                ppInfo.PK = "TradeNumber";
                if (pageInfo.Sort == 0)
                {
                    ppInfo.Sort = " TradeTime asc ";
                }
                else
                {
                    ppInfo.Sort = " TradeTime desc ";
                }
                ppInfo.Tables = "XH_HistoryTradeTable";
                #endregion

                #region 过滤条件组装
                ppInfo.Filter = BuildXHTradeQueryWhere(filter, userID, accountType, false);
                #endregion

                #region 执行查询
                try
                {
                    CommonDALOperate<XH_HistoryTradeTableInfo> com = new CommonDALOperate<XH_HistoryTradeTableInfo>();
                    list = com.PagingQueryProcedures(ppInfo, out total, dal.ReaderBind);
                    // list = dal.PagingXH_HistoryTradeByFilter(ppInfo, out total);
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    LogHelper.WriteError(ex.ToString(), ex);
                }
                #endregion
            }
            return list;
        }
        #endregion

        #region 现货资金帐户当日现货成交信息分页查询--根据用户和密码查询该用户所拥有的现货资金帐户当日现货成交信息
        /// <summary>
        ///  Title:根据用户和密码查询该用户所拥有的现货资金帐户当日现货成交信息
        ///  Des.:此方法因为一个用户（交易员）可能拥有多个现货资金账号(如：4-商品现货资金帐号,6-股指现货资金帐号)
        ///  所以返回的数据会回多个资金账号的数据
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码--如果密码为空时不验证密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤条件数据</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">返回数页数</param>
        /// <param name="errorMsg">异常信息</param>
        /// <returns>返回当日现货成交信息</returns>
        public List<XH_TodayTradeTableInfo> PagingXH_TodayTradeByFilter(string userID, string pwd, int accountType, SpotTradeConditionFindEntity filter, PagingInfo pageInfo, out int total, out string errorMsg)
        {
            List<XH_TodayTradeTableInfo> list = null;
            XH_TodayTradeTableDal dal = new XH_TodayTradeTableDal();
            errorMsg = "";
            total = 0;

            #region 密码不为空时先验证用户
            if (string.IsNullOrEmpty(userID))
            {
                errorMsg = "查询失败！失败原因为：交易员ID不能为空！";
                return list;
            }
            if (!string.IsNullOrEmpty(pwd))
            {
                UA_UserBasicInformationTableDal usDal = new UA_UserBasicInformationTableDal();
                if (!usDal.Exists(userID, pwd))
                {
                    errorMsg = "查询失败！失败原因为：交易员ID或密码输入错误 ！";
                    return list;
                }
            }
            #endregion

            string strFilter = "";
            if (filter != null && !string.IsNullOrEmpty(filter.EntrustNumber))  //委托单号（当只根据委托单号查询时）
            {
                #region 如果有委托单号直接查询唯一记录
                strFilter += string.Format(" EntrustNumber='{0}'", filter.EntrustNumber);
                list = dal.GetListArray(strFilter);
                if (!Utils.IsNullOrEmpty(list))
                {
                    total = 1;
                }
                #endregion
            }
            else //当不带委托单号时
            {
                #region 如果分页信息为空返回异常
                if (pageInfo == null)
                {
                    errorMsg = "分页信息不能为空!";
                    return null;
                }
                #endregion

                #region 分页存储过程相关信息组装
                PagingProceduresInfo ppInfo = new PagingProceduresInfo();
                ppInfo.IsCount = pageInfo.IsCount;
                ppInfo.PageNumber = pageInfo.CurrentPage;
                ppInfo.PageSize = pageInfo.PageLength;
                ppInfo.Fields = "TradeNumber,EntrustNumber,PortfolioLogo,TradePrice,TradeAmount,EntrustPrice,StampTax,Commission,TransferAccountFee,TradeProceduresFee,MonitoringFee,TradingSystemUseFee,TradeCapitalAmount,ClearingFee,StockAccount,CapitalAccount,SpotCode,TradeTypeId,TradeUnitId,BuySellTypeId,CurrencyTypeId,TradeTime ";

                ppInfo.PK = "TradeNumber";
                if (pageInfo.Sort == 0)
                {
                    ppInfo.Sort = " TradeTime asc ";
                }
                else
                {
                    ppInfo.Sort = " TradeTime desc ";
                }
                ppInfo.Tables = "XH_TodayTradeTable";
                #endregion

                #region 过滤条件组装
                #region old code
                //StringBuilder sbFilter = new StringBuilder("1=1 ");
                //// strFilter = "1=1 ";
                //if (filter != null)
                //{
                //    # region 0.资金帐户
                //    if (!string.IsNullOrEmpty(filter.SpotCapitalAccount))
                //    {
                //        sbFilter.AppendFormat(" AND CapitalAccount='{0}'", filter.SpotCapitalAccount);
                //    }
                //    else //不指定可能有多个账号
                //    {
                //        sbFilter.Append(" And CapitalAccount in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable   ");
                //        sbFilter.AppendFormat("  where accounttypelogo in (select accounttypelogo from BD_AccountType where atcid=3)  and userid='{0}')", userID);
                //    }
                //    # endregion

                //    # region  1.买卖方向

                //    if (filter.BuySellDirection != 0)
                //    {
                //        sbFilter.AppendFormat(" AND BuySellTypeId='{0}'", filter.BuySellDirection);
                //    }

                //    # endregion

                //    # region 2.所属市场
                //    if (filter.BelongToMarket != 0)
                //    {
                //        sbFilter.AppendFormat(" AND MarketTypeId='{0}'", filter.BelongToMarket);
                //        // strFilter += string.Format(" AND OwnershipMarket='{0}'", filter.BelongToMarket);
                //    }
                //    # endregion

                //    # region 3.成交类型
                //    if (filter.TradeType != 0)
                //    {
                //        sbFilter.AppendFormat(" AND TradeTypeId='{0}'", filter.TradeType);
                //    }
                //    # endregion

                //    # region 4.币种赋值
                //    if (filter.CurrencyType != 0)
                //    {
                //        sbFilter.AppendFormat(" AND CurrencyTypeId='{0}'", filter.CurrencyType);
                //    }
                //    # endregion

                //    # region 5.合约代码
                //    if (!string.IsNullOrEmpty(filter.SpotCode))
                //    {
                //        sbFilter.AppendFormat(" AND SpotCode='{0}'", filter.SpotCode);
                //    }
                //    # endregion

                //    # region 6.品种类别
                //    if (filter.VarietyType != 0)
                //    {
                //        sbFilter.AppendFormat(" AND VarietytypeId='{0}'", filter.VarietyType);
                //    }
                //    #endregion
                //}
                //else
                //{
                //    //条件为空直接查询用户所对应拥有的资金账户下的今日成交
                //    sbFilter.Append(" And CapitalAccount in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable   ");
                //    sbFilter.AppendFormat("  where accounttypelogo in (select accounttypelogo from BD_AccountType where atcid=3)  and userid='{0}')", userID);
                //}
                #endregion

                ppInfo.Filter = BuildXHTradeQueryWhere(filter, userID, accountType, true);
                #endregion

                #region 执行查询
                try
                {
                    CommonDALOperate<XH_TodayTradeTableInfo> com = new CommonDALOperate<XH_TodayTradeTableInfo>();
                    list = com.PagingQueryProcedures(ppInfo, out total, dal.ReaderBind);
                    //list = dal.PagingXH_TodayTradeByFilter(ppInfo, out total);
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    LogHelper.WriteError(ex.ToString(), ex);
                }
                #endregion
            }
            return list;
        }
        #endregion

        #region 组装现货成交过滤条件
        /// <summary>
        /// 组装现货成交过滤条件
        /// </summary>
        /// <param name="filter">过滤条件实体</param>
        /// <param name="userID">用户ID</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="isToday">是否为当日查询</param>
        /// <returns></returns>
        string BuildXHTradeQueryWhere(SpotTradeConditionFindEntity filter, string userID, int accountType, bool isToday)
        {
            #region 过滤条件组装
            StringBuilder sbFilter = new StringBuilder("1=1 ");
            // strFilter = "1=1 ";
            if (filter != null)
            {
                # region 0.资金帐户
                if (!string.IsNullOrEmpty(filter.SpotCapitalAccount))
                {
                    sbFilter.AppendFormat(" AND CapitalAccount='{0}'", filter.SpotCapitalAccount);
                }
                else //不指定可能有多个账号
                {
                    sbFilter.Append(" And CapitalAccount in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable   ");
                    if (accountType == 0)
                    {
                        sbFilter.AppendFormat("  where accounttypelogo in (select accounttypelogo from BD_AccountType where  atcid='{0}')  and userid='{1}')", (int)Types.AccountAttributionType.SpotCapital, userID);
                    }
                    else
                    {
                        sbFilter.AppendFormat("  where accounttypelogo='{0}'  and userid='{1}')", accountType, userID);
                    }
                }
                # endregion

                # region  1.买卖方向

                if (filter.BuySellDirection != 0)
                {
                    sbFilter.AppendFormat(" AND BuySellTypeId='{0}'", filter.BuySellDirection);
                }
                # endregion

                # region 2.成交类型
                if (filter.TradeType != 0)
                {
                    sbFilter.AppendFormat(" AND TradeTypeId='{0}'", filter.TradeType);
                }
                # endregion

                # region 3.币种赋值
                if (filter.CurrencyType != 0)
                {
                    sbFilter.AppendFormat(" AND CurrencyTypeId='{0}'", filter.CurrencyType);
                }
                # endregion

                # region 4.合约代码
                if (!string.IsNullOrEmpty(filter.SpotCode))
                {
                    sbFilter.AppendFormat(" AND SpotCode='{0}'", filter.SpotCode);
                }
                # endregion

                # region 5.查询开始时间和结束时间
                if (!isToday) //为当日成交就不分时间查询
                {
                    sbFilter.AppendFormat(CommonDALBulidSQLScript.BuildWhereQueryBetwennTime(filter.StartTime, filter.EndTime, 30), "TradeTime");

                    #region old code
                    //if (filter.StartTime != null && filter.EndTime != null)//起始和结束时间均已赋值
                    //{
                    //    if (DateTime.Compare(filter.StartTime, filter.EndTime) < 0) //起始时间小于结束时间（比如2008-05-06小于2008-05-28）
                    //    {
                    //        sbFilter.AppendFormat(" AND (TradeTime>= '{0}' AND  TradeTime<'{1}')", filter.StartTime.ToString("yyyy-MM-dd"), filter.EndTime.AddDays(1).ToString("yyyy-MM-dd"));
                    //    }
                    //    else //起始时间大于或等于结束时间，就默认查出最近一个月的（从昨天开始往前推一个月）
                    //    {
                    //        //查询开始时间为 ：上一个月的今天
                    //        filter.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddMonths(-1).ToShortDateString());
                    //        //结束时间为：今天 00：00：00
                    //        filter.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(1).ToShortDateString());
                    //        sbFilter.AppendFormat(" AND (TradeTime>= '{0}' AND  TradeTime<'{1}')", filter.StartTime, filter.EndTime);
                    //    }
                    //}
                    //else //若起始时间或结束时间未赋值，就默认查出最近一个月的（从昨天开始往前推一个月）
                    //{
                    //    //查询开始时间为 ：上一个月的今天
                    //    filter.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddMonths(-1).ToShortDateString());
                    //    //结束时间为：今天 00：00：00
                    //    filter.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(1).ToShortDateString());
                    //    sbFilter.AppendFormat(" AND (TradeTime>= '{0}' AND  TradeTime<'{1}')", filter.StartTime, filter.EndTime);
                    //}
                    #endregion
                }
                #endregion

            }
            else
            {
                //条件为空直接查询用户所对应拥有的资金账户下的今日成交
                sbFilter.Append(" And CapitalAccount in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable   ");
                if (accountType == 0)
                {
                    sbFilter.AppendFormat("  where accounttypelogo in (select accounttypelogo from BD_AccountType where  atcid='{0}')  and userid='{1}')", (int)Types.AccountAttributionType.SpotCapital, userID);
                }
                else
                {
                    sbFilter.AppendFormat("  where accounttypelogo='{0}'  and userid='{1}')", accountType, userID);
                }
            }
            #endregion
            return sbFilter.ToString();
        }
        #endregion

        #endregion

        #region 期货今日/历史成交分页查询
        #region 期货资金帐户历史期货成交信息分页查询--根据用户和密码查询该用户所拥有的期货资金帐户历史期货成交信息
        /// <summary>
        ///  Title:根据用户和密码查询该用户所拥有的期货资金帐户历史期货成交信息
        ///  Des.:此方法因为一个用户（交易员）可能拥有多个期货资金账号(如：4-商品期货资金帐号,6-股指期货资金帐号)
        ///  所以返回的数据会回多个资金账号的数据
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码--如果密码为空时不验证密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤条件数据</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">返回数页数</param>
        /// <param name="errorMsg">异常信息</param>
        /// <returns>返回当日期货成交信息</returns>
        public List<QH_HistoryTradeTableInfo> PagingQH_HistoryTradeByFilter(string userID, string pwd, int accountType, FuturesTradeConditionFindEntity filter, PagingInfo pageInfo, out int total, out string errorMsg)
        {
            List<QH_HistoryTradeTableInfo> list = null;
            QH_HistoryTradeTableDal dal = new QH_HistoryTradeTableDal();
            errorMsg = "";
            total = 0;

            #region 密码不为空时先验证用户
            if (string.IsNullOrEmpty(userID))
            {
                errorMsg = "查询失败！失败原因为：交易员ID不能为空！";
                return list;
            }
            if (!string.IsNullOrEmpty(pwd))
            {
                UA_UserBasicInformationTableDal usDal = new UA_UserBasicInformationTableDal();
                if (!usDal.Exists(userID, pwd))
                {
                    errorMsg = "查询失败！失败原因为：交易员ID或密码输入错误 ！";
                    return list;
                }
            }
            #endregion

            if (filter != null && !string.IsNullOrEmpty(filter.EntrustNumber))  //委托单号（当只根据委托单号查询时）
            {
                #region 如果有委托单号直接查询唯一记录
                list = dal.GetListArray(" EntrustNumber='" + filter.EntrustNumber.Trim() + "'");
                if (!Utils.IsNullOrEmpty(list))
                {
                    total = 1;
                }
                #endregion
            }
            else //当不带委托单号时
            {
                #region 如果分页信息为空返回异常
                if (pageInfo == null)
                {
                    errorMsg = "分页信息不能为空!";
                    return null;
                }
                #endregion

                #region 分页存储过程相关信息组装
                PagingProceduresInfo ppInfo = new PagingProceduresInfo();
                ppInfo.IsCount = pageInfo.IsCount;
                ppInfo.PageNumber = pageInfo.CurrentPage;
                ppInfo.PageSize = pageInfo.PageLength;
                ppInfo.Fields = "  TradeNumber,EntrustNumber,PortfolioLogo,TradePrice,EntrustPrice,TradeAmount,TradeProceduresFee,Margin,ContractCode,TradeAccount,CapitalAccount,BuySellTypeId,OpenCloseTypeId,TradeUnitId,TradeTypeId,CurrencyTypeId,TradeTime,MarketProfitLoss  ";

                ppInfo.PK = "TradeNumber";
                if (pageInfo.Sort == 0)
                {
                    ppInfo.Sort = " TradeTime asc ";
                }
                else
                {
                    ppInfo.Sort = " TradeTime desc ";
                }
                ppInfo.Tables = "QH_HistoryTradeTable";
                #endregion

                #region old code
                //StringBuilder sbFilter = new StringBuilder("1=1 ");
                //// strFilter = "1=1 ";
                //if (filter != null)
                //{
                //    # region 0.资金帐户
                //    if (!string.IsNullOrEmpty(filter.CapitalAccount))
                //    {
                //        sbFilter.AppendFormat(" AND CapitalAccount='{0}'", filter.CapitalAccount);
                //        //strFilter += string.Format(" AND CapitalAccount='{0}'", filter.CapitalAccount);
                //    }
                //    else //不指定可能有多个账号
                //    {
                //        sbFilter.Append(" And CapitalAccount in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable   ");
                //        sbFilter.AppendFormat("  where accounttypelogo in (select accounttypelogo from BD_AccountType where atcid=3)  and userid='{0}')", userID);
                //        //strFilter += " And CapitalAccount in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable   ";
                //        //strFilter += string.Format("  where accounttypelogo in (select accounttypelogo from BD_AccountType where atcid=3)  and userid='{0}'", userID);
                //    }
                //    # endregion

                //    # region  1.买卖方向

                //    if (filter.BuySellDirection != 0)
                //    {
                //        sbFilter.AppendFormat(" AND BuySellTypeId='{0}'", filter.BuySellDirection);
                //        // strFilter += string.Format(" AND BuySellTypeId='{0}'", filter.BuySellDirection);
                //    }

                //    # endregion

                //    # region 2.所属市场
                //    if (filter.BelongToMarket != 0)
                //    {
                //        sbFilter.AppendFormat(" AND OwnershipMarketId='{0}'", filter.BelongToMarket);
                //        //strFilter += string.Format(" AND OwnershipMarketId='{0}'", filter.BelongToMarket);
                //    }
                //    # endregion

                //    # region 3.成交类型
                //    if (filter.TradeType != 0)
                //    {
                //        sbFilter.AppendFormat(" AND TradeTypeId='{0}'", filter.TradeType);
                //        //strFilter += string.Format(" AND TradeTypeId='{0}'", filter.TradeType);
                //    }
                //    # endregion

                //    # region 4.币种赋值
                //    if (filter.CurrencyTypeId != 0)
                //    {
                //        sbFilter.AppendFormat(" AND CurrencyTypeId='{0}'", filter.CurrencyTypeId);
                //        //strFilter += string.Format(" AND CurrencyTypeId='{0}'", filter.CurrencyTypeId);
                //    }
                //    # endregion

                //    # region 5.合约代码
                //    if (!string.IsNullOrEmpty(filter.ContractCode))
                //    {
                //        sbFilter.AppendFormat(" AND ContractCode='{0}'", filter.ContractCode);
                //        // strFilter += string.Format(" AND ContractCode='{0}'", filter.ContractCode);

                //    }
                //    # endregion

                //    # region 6.品种类别
                //    if (filter.VarietyType != 0)
                //    {
                //        sbFilter.AppendFormat(" AND VarietypTypeId='{0}'", filter.VarietyType);
                //        //  strFilter += string.Format(" AND VarietypTypeId='{0}'", filter.VarietyType);

                //    }
                //    #endregion

                //    # region 7.查询开始时间和结束时间
                //    if (filter.StartTime != null && filter.EndTime != null)//起始和结束时间均已赋值
                //    {
                //        if (DateTime.Compare(filter.StartTime, filter.EndTime) < 0) //起始时间小于结束时间（比如2008-05-06小于2008-05-28）
                //        {
                //            sbFilter.AppendFormat(" AND (TradeTime>= '{0}' AND  TradeTime<'{1}')", filter.StartTime.ToString("yyyy-MM-dd"), filter.EndTime.AddDays(1).ToString("yyyy-MM-dd"));
                //            //strFilter += string.Format(" AND (TradeTime>= '{0}' AND  TradeTime<'{1}')", filter.StartTime.ToString("yyyy-MM-dd"), filter.EndTime.AddDays(1).ToString("yyyy-MM-dd"));
                //        }
                //        else //起始时间大于或等于结束时间，就默认查出最近一个月的（从昨天开始往前推一个月）
                //        {
                //            //查询开始时间为 ：上一个月的今天
                //            filter.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddMonths(-1).ToShortDateString());
                //            //结束时间为：今天 00：00：00
                //            filter.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(1).ToShortDateString());
                //            sbFilter.AppendFormat(" AND (TradeTime>= '{0}' AND  TradeTime<'{1}')", filter.StartTime, filter.EndTime);
                //            //strFilter += string.Format(" AND (TradeTime>= '{0}' AND  TradeTime<'{1}') ", filter.StartTime, filter.EndTime);

                //        }
                //    }
                //    else //若起始时间或结束时间未赋值，就默认查出最近一个月的（从昨天开始往前推一个月）
                //    {
                //        //查询开始时间为 ：上一个月的今天
                //        filter.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddMonths(-1).ToShortDateString());
                //        //结束时间为：今天 00：00：00
                //        filter.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(1).ToShortDateString());
                //        sbFilter.AppendFormat(" AND (TradeTime>= '{0}' AND  TradeTime<'{1}')", filter.StartTime, filter.EndTime);
                //        // strFilter += string.Format(" AND (TradeTime>= '{0}' AND  TradeTime<'{1}') ", filter.StartTime, filter.EndTime);
                //    }
                //    #endregion
                //}
                //else
                //{
                //    //条件为空直接查询用户所对应拥有的资金账户下的今日成交
                //    sbFilter.Append(" And CapitalAccount in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable   ");
                //    sbFilter.AppendFormat("  where accounttypelogo in (select accounttypelogo from BD_AccountType where atcid=3)  and userid='{0}')", userID);
                //    //strFilter += " And CapitalAccount in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable   ";
                //    //strFilter += string.Format("  where accounttypelogo in (select accounttypelogo from BD_AccountType where atcid=3)  and userid='{0}'", userID);
                //}
                #endregion

                #region 过滤条件组装
                ppInfo.Filter = BuildQHTradeQueryWhere(filter, userID, accountType, false);
                #endregion

                #region 执行查询
                try
                {
                    CommonDALOperate<QH_HistoryTradeTableInfo> com = new CommonDALOperate<QH_HistoryTradeTableInfo>();
                    list = com.PagingQueryProcedures(ppInfo, out total, dal.ReaderBind);
                    //list = dal.PagingQH_HistoryTradeByFilter(ppInfo, out total);
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    LogHelper.WriteError(ex.ToString(), ex);
                }
                #endregion
            }
            return list;
        }
        #endregion

        #region 期货资金帐户当日期货成交信息分页查询--根据用户和密码查询该用户所拥有的期货资金帐户当日期货成交信息
        /// <summary>
        ///  Title:根据用户和密码查询该用户所拥有的期货资金帐户当日期货成交信息
        ///  Des.:此方法因为一个用户（交易员）可能拥有多个期货资金账号(如：4-商品期货资金帐号,6-股指期货资金帐号)
        ///  所以返回的数据会回多个资金账号的数据
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码--如果密码为空时不验证密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤条件数据</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">返回数页数</param>
        /// <param name="errorMsg">异常信息</param>
        /// <returns>返回当日期货成交信息</returns>
        public List<QH_TodayTradeTableInfo> PagingQH_TodayTradeByFilter(string userID, string pwd, int accountType, FuturesTradeConditionFindEntity filter, PagingInfo pageInfo, out int total, out string errorMsg)
        {
            List<QH_TodayTradeTableInfo> list = null;
            QH_TodayTradeTableDal dal = new QH_TodayTradeTableDal();
            errorMsg = "";
            total = 0;

            #region 密码不为空时先验证用户
            if (string.IsNullOrEmpty(userID))
            {
                errorMsg = "查询失败！失败原因为：交易员ID不能为空！";
                return list;
            }
            if (!string.IsNullOrEmpty(pwd))
            {
                UA_UserBasicInformationTableDal usDal = new UA_UserBasicInformationTableDal();
                if (!usDal.Exists(userID, pwd))
                {
                    errorMsg = "查询失败！失败原因为：交易员ID或密码输入错误 ！";
                    return list;
                }
            }
            #endregion

            //string strFilter = "";
            if (filter != null && !string.IsNullOrEmpty(filter.EntrustNumber))  //委托单号（当只根据委托单号查询时）
            {
                #region 如果有委托单号直接查询唯一记录
                list = dal.GetListArray(" EntrustNumber='" + filter.EntrustNumber.Trim() + "'");
                if (!Utils.IsNullOrEmpty(list))
                {
                    total = 1;
                }
                #endregion
            }
            else //当不带委托单号时
            {
                #region 如果分页信息为空返回异常
                if (pageInfo == null)
                {
                    errorMsg = "分页信息不能为空!";
                    return null;
                }
                #endregion

                #region 分页存储过程相关信息组装
                PagingProceduresInfo ppInfo = new PagingProceduresInfo();
                ppInfo.IsCount = pageInfo.IsCount;
                ppInfo.PageNumber = pageInfo.CurrentPage;
                ppInfo.PageSize = pageInfo.PageLength;
                ppInfo.Fields = "TradeNumber,EntrustNumber,PortfolioLogo,TradePrice,EntrustPrice,TradeAmount,TradeProceduresFee,Margin,ContractCode,TradeAccount,CapitalAccount,BuySellTypeId,OpenCloseTypeId,TradeUnitId,TradeTypeId,CurrencyTypeId,TradeTime,MarketProfitLoss ";

                ppInfo.PK = "TradeNumber";
                if (pageInfo.Sort == 0)
                {
                    ppInfo.Sort = " TradeTime asc ";
                }
                else
                {
                    ppInfo.Sort = " TradeTime desc ";
                }
                ppInfo.Tables = "QH_TodayTradeTable";
                #endregion

                #region old code
                ///// <summary>
                ///// 
                //StringBuilder sbFilter = new StringBuilder("1=1 ");
                //// strFilter = "1=1 ";
                //if (filter != null)
                //{
                //    # region 0.资金帐户
                //    if (!string.IsNullOrEmpty(filter.CapitalAccount))
                //    {
                //        sbFilter.AppendFormat(" AND CapitalAccount='{0}'", filter.CapitalAccount);
                //        //strFilter += string.Format(" AND CapitalAccount='{0}'", filter.CapitalAccount);
                //    }
                //    else //不指定可能有多个账号
                //    {
                //        sbFilter.Append(" And CapitalAccount in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable   ");
                //        sbFilter.AppendFormat("  where accounttypelogo in (select accounttypelogo from BD_AccountType where atcid=3)  and userid='{0}')", userID);
                //        //strFilter += " And CapitalAccount in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable   ";
                //        //strFilter += string.Format("  where accounttypelogo in (select accounttypelogo from BD_AccountType where atcid=3)  and userid='{0}'", userID);
                //    }
                //    # endregion

                //    # region  1.买卖方向

                //    if (filter.BuySellDirection != 0)
                //    {
                //        sbFilter.AppendFormat(" AND BuySellId='{0}'", filter.BuySellDirection);
                //        //strFilter += string.Format(" AND BuySellId='{0}'", filter.BuySellDirection);
                //    }

                //    # endregion

                //    # region 2.所属市场
                //    if (filter.BelongToMarket != 0)
                //    {
                //        sbFilter.AppendFormat(" AND OwnershipMarket='{0}'", filter.BelongToMarket);
                //        // strFilter += string.Format(" AND OwnershipMarket='{0}'", filter.BelongToMarket);
                //    }
                //    # endregion

                //    # region 3.成交类型
                //    if (filter.TradeType != 0)
                //    {
                //        sbFilter.AppendFormat(" AND TradeTypeId='{0}'", filter.TradeType);
                //        // strFilter += string.Format(" AND TradeTypeId='{0}'", filter.TradeType);
                //    }
                //    # endregion

                //    # region 4.币种赋值
                //    if (filter.CurrencyTypeId != 0)
                //    {
                //        sbFilter.AppendFormat(" AND CurrencyTypeId='{0}'", filter.CurrencyTypeId);
                //        // strFilter += string.Format(" AND CurrencyTypeId='{0}'", filter.CurrencyTypeId);
                //    }
                //    # endregion

                //    # region 5.合约代码
                //    if (!string.IsNullOrEmpty(filter.ContractCode))
                //    {
                //        sbFilter.AppendFormat(" AND ContractCode='{0}'", filter.ContractCode);
                //        //strFilter += string.Format(" AND ContractCode='{0}'", filter.ContractCode);

                //    }
                //    # endregion

                //    # region 6.品种类别
                //    if (filter.VarietyType != 0)
                //    {
                //        sbFilter.AppendFormat(" AND VarietyTypeId='{0}'", filter.VarietyType);
                //        //strFilter += string.Format(" AND VarietyTypeId='{0}'", filter.VarietyType);
                //    }
                //    #endregion
                //}
                //else
                //{
                //    //条件为空直接查询用户所对应拥有的资金账户下的今日成交
                //    sbFilter.Append(" And CapitalAccount in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable   ");
                //    sbFilter.AppendFormat("  where accounttypelogo in (select accounttypelogo from BD_AccountType where atcid=3)  and userid='{0}')", userID);
                //    //strFilter += " And CapitalAccount in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable   ";
                //    //strFilter += string.Format("  where accounttypelogo in (select accounttypelogo from BD_AccountType where atcid=3)  and userid='{0}'", userID);

                //}
                #endregion

                #region 过滤条件组装
                ppInfo.Filter = BuildQHTradeQueryWhere(filter, userID, accountType, true);
                #endregion

                #region 执行查询
                try
                {
                    CommonDALOperate<QH_TodayTradeTableInfo> com = new CommonDALOperate<QH_TodayTradeTableInfo>();
                    list = com.PagingQueryProcedures(ppInfo, out total, dal.ReaderBind);
                    //list = dal.PagingQH_TodayTradeByFilter(ppInfo, out total);
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    LogHelper.WriteError(ex.ToString(), ex);
                }
                #endregion

            }
            return list;
        }
        #endregion

        #region 组装期货成交过滤条件
        /// <summary>
        /// 组装期货成交过滤条件
        /// </summary>
        /// <param name="filter">期货过滤条件实体</param>
        /// <param name="userID">用户ID</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的期货资金账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="isToday">是否为今日查询</param>
        /// <returns></returns>
        string BuildQHTradeQueryWhere(FuturesTradeConditionFindEntity filter, string userID, int accountType, bool isToday)
        {
            #region 过滤条件组装
            StringBuilder sbFilter = new StringBuilder("1=1 ");
            if (filter != null)
            {
                # region 0.资金帐户
                if (!string.IsNullOrEmpty(filter.CapitalAccount))
                {
                    sbFilter.AppendFormat(" AND CapitalAccount='{0}'", filter.CapitalAccount);
                }
                else //不指定可能有多个账号
                {
                    sbFilter.Append(" And CapitalAccount in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable   ");
                    if (accountType == 0)
                    {
                        sbFilter.AppendFormat("  where accounttypelogo in (select accounttypelogo from BD_AccountType where  atcid='{0}')  and userid='{1}')", (int)Types.AccountAttributionType.FuturesCapital, userID);
                    }
                    else
                    {
                        sbFilter.AppendFormat("  where accounttypelogo='{0}'  and userid='{1}')", accountType, userID);
                    }
                }
                # endregion

                # region  1.买卖方向

                if (filter.BuySellDirection != 0)
                {
                    sbFilter.AppendFormat(" AND BuySellTypeId='{0}'", filter.BuySellDirection);
                }

                # endregion

                # region 2.成交类型
                if (filter.TradeType != 0)
                {
                    sbFilter.AppendFormat(" AND TradeTypeId='{0}'", filter.TradeType);
                }
                # endregion

                # region 3.币种赋值
                if (filter.CurrencyTypeId != 0)
                {
                    sbFilter.AppendFormat(" AND CurrencyTypeId='{0}'", filter.CurrencyTypeId);
                }
                # endregion

                # region 4.合约代码
                if (!string.IsNullOrEmpty(filter.ContractCode))
                {
                    sbFilter.AppendFormat(" AND ContractCode='{0}'", filter.ContractCode);
                }
                # endregion

                #region 5.开平方向
                if (filter.OpenCloseDirection != 0)
                {
                    sbFilter.AppendFormat(" AND OpenCloseTypeId='{0}'", filter.OpenCloseDirection);
                }
                #endregion

                # region 6.查询开始时间和结束时间
                if (!isToday)
                {
                    sbFilter.AppendFormat(CommonDALBulidSQLScript.BuildWhereQueryBetwennTime(filter.StartTime, filter.EndTime, 30), "TradeTime");

                    #region old code
                    //if (filter.StartTime != null && filter.EndTime != null)//起始和结束时间均已赋值
                    //{
                    //    if (DateTime.Compare(filter.StartTime, filter.EndTime) < 0) //起始时间小于结束时间（比如2008-05-06小于2008-05-28）
                    //    {
                    //        sbFilter.AppendFormat(" AND (TradeTime>= '{0}' AND  TradeTime<'{1}')", filter.StartTime.ToString("yyyy-MM-dd"), filter.EndTime.AddDays(1).ToString("yyyy-MM-dd"));
                    //    }
                    //    else //起始时间大于或等于结束时间，就默认查出最近一个月的（从昨天开始往前推一个月）
                    //    {
                    //        //查询开始时间为 ：上一个月的今天
                    //        filter.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddMonths(-1).ToShortDateString());
                    //        //结束时间为：今天 00：00：00
                    //        filter.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(1).ToShortDateString());
                    //        sbFilter.AppendFormat(" AND (TradeTime>= '{0}' AND  TradeTime<'{1}')", filter.StartTime, filter.EndTime);

                    //    }
                    //}
                    //else //若起始时间或结束时间未赋值，就默认查出最近一个月的（从昨天开始往前推一个月）
                    //{
                    //    //查询开始时间为 ：上一个月的今天
                    //    filter.StartTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddMonths(-1).ToShortDateString());
                    //    //结束时间为：今天 00：00：00
                    //    filter.EndTime = Convert.ToDateTime(DateTime.Parse(DateTime.Today.ToString()).AddDays(1).ToShortDateString());
                    //    sbFilter.AppendFormat(" AND (TradeTime>= '{0}' AND  TradeTime<'{1}')", filter.StartTime, filter.EndTime);
                    //}
                    #endregion
                }
                #endregion
            }
            else
            {
                //条件为空直接查询用户所对应拥有的资金账户下的今日成交
                sbFilter.Append(" And CapitalAccount in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable   ");

                if (accountType == 0)
                {
                    sbFilter.AppendFormat("  where accounttypelogo in (select accounttypelogo from BD_AccountType where  atcid='{0}')  and userid='{1}')", (int)Types.AccountAttributionType.FuturesCapital, userID);
                }
                else
                {
                    sbFilter.AppendFormat("  where accounttypelogo='{0}'  and userid='{1}')", accountType, userID);
                }
            }
            #endregion

            return sbFilter.ToString();
        }
        #endregion
        #endregion

        #region 构建查询时间段字符串
        ////这个方法已经移到了CommonDALOperate中去
        ///// <summary>
        ///// Title:构建查询时间段条件语句
        ///// Desc.:如果两者中有一个时间没有附值(或者开始时间大于结束时间),即以今天为结束时间向前查询N天
        ///// 返回 AND ({0}>= '2009-05-07' AND  {0} &lt; '2009-05-15')
        ///// </summary>
        ///// <param name="startTime">开始时间</param>
        ///// <param name="endTime">结束时间</param>
        ///// <param name="day">向前查询多少天</param>
        ///// <returns>返回 AND ({0}>= '2009-05-07' AND  {0} <![CDATA[<]]> '2009-05-15')</returns>
        //string BuildWhereQueryBetwennTime(DateTime? startTime, DateTime? endTime, int day)
        //{
        //    string start = DateTime.Today.AddDays(-day).ToShortDateString(); ;
        //    string end = DateTime.Today.AddDays(1).ToShortDateString();
        //    if (startTime != null && endTime != null)//起始和结束时间均已赋值
        //    {
        //        //起始时间小于结束时间（比如2008-05-06小于2008-05-28）
        //        if (DateTime.Compare(startTime.Value, endTime.Value) < 0)
        //        {
        //            start = startTime.Value.ToString("yyyy-MM-dd");
        //            end = endTime.Value.ToString("yyyy-MM-dd");
        //        }
        //        else if (DateTime.Compare(startTime.Value, endTime.Value) == 0)
        //        {
        //            //起始时间等于结束时间
        //            start = startTime.Value.ToString("yyyy-MM-dd");
        //            end = endTime.Value.AddDays(1).ToString("yyyy-MM-dd");
        //        }
        //    }

        //    return " AND ( {0}>= '" + start + "' AND  {0}<'" + end + "')";
        //}
        #endregion

        #endregion

    }
}