using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatchCenter.Entity;
using MatchCenter.Entity.HK;

namespace MatchCenter.BLL
{
    ///// <summary>
    ///// 撮合现货委托单委托方法
    ///// </summary>
    ///// <param name="model"></param>
    //public delegate void MatchStockDelegate(StockDataOrderEntity model);
    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="data"></param>
    //public delegate void MatchSockMarkDelegate(HqMarketEntity data);
    /// <summary>
    /// 撤现货单委托方法
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public delegate int CacelStockOrderDelegate(CancelEntity model);

    /// <summary>
    /// 港股改单委托方法
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public delegate int ModifyHKOrderDelegate(HKModifyEntity model);
    ///// <summary>
    ///// 撮合期货委托单委托方法
    ///// </summary>
    ///// <param name="model"></param>
    //public delegate void MatchFutureOrderDelegate(FutureDataOrderEntity model);
    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="data"></param>
    //public delegate void MatchSockFutureMarkDelegate(FutureMarketEntity data);
    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="model"></param>
    //public delegate void MatchStockFutureDelegate(FutureFuseEntity model);

    /// <summary>
    /// 撮合规则委托方法
    /// </summary>
    /// <param name="data"></param>
    public delegate void MatchResultDelegate(ResultDataEntity data);
    ///// <summary>
    ///// 对撤委托单委托方法
    ///// </summary>
    ///// <param name="data"></param>
    //public delegate void CancelOrderDelegate(CancelOrderEntity data);

    /// <summary>
    /// 现货成交回报委托方法
    /// </summary>
    /// <param name="model">现货成交回报实体</param>
    public delegate void DoBackStockDealDeletate(StockDealBackEntity model);
    /// <summary>
    /// 期货成交回报委托方法
    /// </summary>
    /// <param name="model">期货成交回报实体</param>
    public delegate void DoBackFutureDealDeletate(FutureDealBackEntity model);

    /// <summary>
    /// 期货成交回报委托方法
    /// </summary>
    /// <param name="model">期货成交回报实体</param>
    public delegate void DoBackCommoditiesStockDealDeletate(CommoditiesDealBackEntity model);
    
    /// <summary>
    /// 港股成交回报委托方法
    /// </summary>
    /// <param name="model">期货成交回报实体</param>
    public delegate void DoBackHKDealDeletate(HKDealBackEntity model);
    ///// <summary>
    ///// 期货委托回报委托方法
    ///// </summary>
    ///// <param name="model"></param>
    //public delegate void DobackFutureDealDeletate(FutureDealBackEntity model);
    ///// <summary>
    ///// 现货成交回报委托方法
    ///// </summary>
    ///// <param name="model"></param>
    //public delegate void DobackStockDealDeletate(StockDealEntity model);
    ///// <summary>
    ///// 消息显示委托方法
    ///// </summary>
    ///// <param name="mode"></param>
    //public delegate void ShowMatchMessageDeletate(StockDealMessage mode);
    ///// <summary>
    ///// 撤单消息委托方法
    ///// </summary>
    ///// <param name="mode"></param>
    //public delegate void ShowRejuctMessageDeletate(StockDataOrderEntity mode);
    ///// <summary>
    ///// 显示消息委托方法
    ///// </summary>
    ///// <param name="model"></param>
    //public delegate void ShowFutureMessageDeletete(FutureDataOrderEntity model);
    ///// <summary>
    ///// 股指期货委托回报委托方法
    ///// </summary>
    ///// <param name="model"></param>
    //public delegate void DoBackCommoditiesDealDeletate(CommoditiesDealBackEntity model);
    ///// <summary>
    ///// 期货委托委托方法
    ///// </summary>
    ///// <param name="datalist"></param>
    //public delegate void MatchStockFutureDelegate(List<FutureDataOrderEntity> datalist);

}
