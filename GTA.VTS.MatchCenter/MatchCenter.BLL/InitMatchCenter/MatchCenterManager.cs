using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Timers;
using System.Windows.Forms;
using GTA.VTS.Common.CommonUtility;
using MatchCenter.BLL.PushBack;
using MatchCenter.DAL;
using MatchCenter.Entity;
using Timer = System.Timers.Timer;
using MatchCenter.DAL.DevolveVerifyCommonService;
using MatchCenter.DAL.SpotTradingDevolveService;
using MatchCenter.DAL.FuturesDevolveService;
using MatchCenter.BLL.ManagementCenter;
using MatchCenter.Entity.HK;
using MatchCenter.DAL.HK;

namespace MatchCenter.BLL
{
    /// <summary>
    /// 撮合中心管理类
    /// Create BY：李健华
    /// Create Date：2008-05-19
    /// Desc.：添加港股相关的方法公开接口定义
    /// Update By:李健华
    /// Update Date:2009-10-19
    ///  Desc：增加港股实现方法
    /// Update By:李健华
    /// Update Date:2009-10-22
    /// Desc.：添加商品期货相关的内容
    /// Update By: 董鹏
    /// Update Date:2010-01-22
    /// Desc.：撤单增加原委托单号验证
    /// Update By: 董鹏
    /// Update Date:2010-01-27
    /// </summary>
    public class MatchCenterManager
    {
        #region 撮合中心单例
        /// <summary>
        /// 撮合管理中心对象
        /// </summary>
        private static MatchCenterManager _instance;
        /// <summary>
        /// 撮合中心单例
        /// </summary>
        public static MatchCenterManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MatchCenterManager();
                return _instance;
            }
        }
        #endregion

        //撮合中心默认的数量
        private static int DeFaultCount = 10;

        #region 缓冲区定义

        /// <summary>
        /// 现货委托撤单回报缓冲区
        /// </summary>
        public Dictionary<string, Queue<CancelOrderEntity>> CancelBackXHEntitys = new Dictionary<string, Queue<CancelOrderEntity>>();

        #region  add by 董鹏 2010-04-30
        /// <summary>
        /// 港股委托撤单回报缓冲区
        /// </summary>
        public Dictionary<string, Queue<CancelOrderEntity>> CancelBackHKEntitys = new Dictionary<string, Queue<CancelOrderEntity>>();

        /// <summary>
        /// 股指期货委托撤单回报缓冲区
        /// </summary>
        public Dictionary<string, Queue<CancelOrderEntity>> CancelBackSIEntitys = new Dictionary<string, Queue<CancelOrderEntity>>();

        /// <summary>
        /// 商品期货委托撤单回报缓冲区
        /// </summary>
        public Dictionary<string, Queue<CancelOrderEntity>> CancelBackCFEntitys = new Dictionary<string, Queue<CancelOrderEntity>>();

        #endregion
        /// <summary>
        /// 现货委托成交回报缓冲区
        /// </summary>
        public Dictionary<string, Queue<StockDealEntity>> DealBackEntitys = new Dictionary<string, Queue<StockDealEntity>>();
        /// <summary>
        /// 委托改单回报缓冲区
        /// </summary>
        public Dictionary<string, Queue<HKModifyBackEntity>> ModifyBackEntitys = new Dictionary<string, Queue<HKModifyBackEntity>>();

        /// <summary>
        /// 港股委托成交回报缓冲区
        /// </summary>
        public Dictionary<string, Queue<HKDealBackEntity>> DealBackHKEntitys = new Dictionary<string, Queue<HKDealBackEntity>>();
        /// <summary>
        /// 股指期货成交回报缓冲区
        /// </summary>
        public Dictionary<string, Queue<FutureDealBackEntity>> DealFutureBackEntitys = new Dictionary<string, Queue<FutureDealBackEntity>>();

        /// <summary>
        /// 商品期货成交回报缓冲区
        /// Create by 董鹏 2010-01-22
        /// </summary>
        public Dictionary<string, Queue<CommoditiesDealBackEntity>> DealCommoditiesBackEntitys = new Dictionary<string, Queue<CommoditiesDealBackEntity>>();

        /// <summary>
        /// 熔断处理处理缓冲区
        /// </summary>
        public Dictionary<string, FuseHanderEntity> FuseHanderEntityList = new Dictionary<string, FuseHanderEntity>();
        /// <summary>
        /// 提供对服务方法的执行上下文的访问权限缓存区
        /// </summary>
        public Dictionary<string, OperationContext> OperationContexts = new Dictionary<string, OperationContext>();
        /// <summary>
        /// 撮合中心委托回报服务
        /// </summary>
        public DobackService matchCenterBackService = new DobackService(DeFaultCount);
        /// <summary>
        /// 撮合机分配置管理
        /// </summary>
        public IDictionary<string, MatchDevice> matchDevices = new Dictionary<string, MatchDevice>();

        #endregion

        #region 显示信息控件定义
        ///// <summary>
        ///// 显示撮合信息listbox
        ///// </summary>
        //public ListBox ListMessage;
        ///// <summary>
        ///// 显示报盘信息listbox
        ///// </summary>
        //public ListBox ListWork;
        #endregion

        #region 构造涵数
        /// <summary>
        /// 构造涵数
        /// </summary>
        public MatchCenterManager()
        {
            Timer ClearTimer = new Timer();
            ClearTimer.Interval = RulesDefaultValue.DefaultInternal * 24;//40分钟执行一次
            ClearTimer.Elapsed += OnTimerElapsed;
            ClearTimer.Enabled = true;
        }
        #endregion

        #region 事件Timer间隔操作事件，用于删除数据库中不必要数据
        /// <summary>
        /// 事件Timer间隔操作事件，用于删除数据库中不必要数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTimerElapsed(object sender, ElapsedEventArgs args)
        {
            LogHelper.WriteDebug("正在执行删除数据库中不必要数据");
            //现货
            StockDataOrderDataAccess.Delete();
            DealOrderDataAccess.Delete();
            //股指期货
            FutureDataOrderDataAccess.Delete();
            FutureDealOrderDataAccess.Delete();
            //港股
            HKEntrustOrderDal.Delete();
            HKDealOrderDal.DeleteDeal();

            //商品期货 add by 董鹏 2010-01-27
            CommoditiesDataOrderAccess.Delete();
            CommoditiesDealOrderAccess.Delete();

            LogHelper.WriteDebug("执行删除数据库中不必要数据操作完成");
        }
        #endregion

        #region 下单撤单操作
        /// <summary>
        /// 现货下单
        /// </summary>
        /// <param name="model">现货实体</param>
        /// <returns></returns>
        public ResultDataEntity DoStockOrder(StockDataOrderEntity model)
        {
            var result = new ResultDataEntity();
            string message;
            LogHelper.WriteDebug("撮合中心管理器现货下单方法：");
            if (model == null || string.IsNullOrEmpty((model.StockCode)))
            {
                message = "CH-0010:委托单不能为空或者股票代码不能为空；";
                result.Message = message;
                LogHelper.WriteDebug(message);
                return result;
            }
            //撮合中心撮合机不能为空
            if (matchDevices != null)
            {
                if (matchDevices.ContainsKey(model.StockCode.Trim()))
                {
                    return matchDevices[model.StockCode.Trim()].DoStockOrder(model);
                }
                message = "CH-0011:管理中心撮合单元没有分配或撮合中心没有分配此品种类型撮合；";
                LogHelper.WriteDebug(message);
            }
            else
            {
                message = "CH-0012:撮合中心撮合机不存在；";
                LogHelper.WriteDebug(message);
            }
            result.Message = message;
            return result;
        }

        /// <summary>
        /// 现货撤单
        /// </summary>
        /// <param name="model">委托单</param>
        /// <returns></returns>
        public CancelResultEntity CancelOrder(CancelEntity model)
        {
            //撮合中心委托不能为空
            if (model == null || string.IsNullOrEmpty(model.StockCode) || string.IsNullOrEmpty(model.OldOrderNo))
            {
                return null;
            }
            if (matchDevices != null)
            {
                if (matchDevices.ContainsKey(model.StockCode.Trim()))
                {
                    return matchDevices[model.StockCode.Trim()].CancelOrder(model);
                }
            }
            return null;
        }

        /// <summary>
        /// 股指期货撤单
        /// </summary>
        /// <param name="model">委托单</param>
        /// <returns></returns>
        public CancelResultEntity CancelFutureOrder(CancelEntity model)
        {
            //委托不能为空
            if (model == null || string.IsNullOrEmpty(model.StockCode) || string.IsNullOrEmpty(model.OldOrderNo))
            {
                return null;
            }
            if (matchDevices != null)
            {
                if (matchDevices.ContainsKey(model.StockCode.Trim()))
                {
                    return matchDevices[model.StockCode.Trim()].CancelFutureOrder(model);
                }
            }
            return null;
        }

        /// <summary>
        /// 港股撤单
        /// </summary>
        /// <param name="model">委托单</param>
        /// <returns></returns>
        public CancelResultEntity CancelHKOrder(CancelEntity model)
        {
            //撮合中心委托不能为空
            if (model == null || string.IsNullOrEmpty(model.StockCode) || string.IsNullOrEmpty(model.OldOrderNo))
            {
                return null;
            }
            if (matchDevices != null)
            {
                if (matchDevices.ContainsKey(model.StockCode.Trim()))
                {
                    return matchDevices[model.StockCode.Trim()].CancelHKOrder(model);
                }
            }
            return null;
        }

        /// <summary>
        /// 商品期货撤单
        /// </summary>
        /// <param name="model">委托单</param>
        /// <returns></returns>
        public CancelResultEntity CancelCommoditiesOrder(CancelEntity model)
        {
            //委托不能为空
            if (model == null || string.IsNullOrEmpty(model.StockCode) || string.IsNullOrEmpty(model.OldOrderNo))
            {
                return null;
            }
            if (matchDevices != null)
            {
                if (matchDevices.ContainsKey(model.StockCode.Trim()))
                {
                    return matchDevices[model.StockCode.Trim()].CancelCommoditiesOrder(model);
                }
            }
            return null;
        }

        /// <summary>
        /// 股指期货下单
        /// </summary>
        /// <param name="model">委托</param>
        /// <returns></returns>
        public ResultDataEntity DoFutureOrder(FutureDataOrderEntity model)
        {
            var result = new ResultDataEntity();
            string message;
            LogHelper.WriteDebug("撮合中心管理器股指期货下单方法：");
            if (model == null || string.IsNullOrEmpty((model.StockCode)))
            {
                message = "CH-0010:委托单不能为空或者期货代码不能为空；";
                result.Message = message;
                LogHelper.WriteDebug(message);
                return result;
            }
            //撮合中心撮合机不能为空
            if (matchDevices != null)
            {
                if (matchDevices.ContainsKey(model.StockCode.Trim()))
                {
                    return matchDevices[model.StockCode.Trim()].DoFutureOrder(model);
                }
                message = "CH-0011:管理中心撮合单元没有分配或撮合中心没有分配此品种类型撮合；";
                LogHelper.WriteDebug(message);
            }
            else
            {
                message = "CH-0012:撮合中心撮合机不存在；";
                LogHelper.WriteDebug(message);
            }
            result.Message = message;
            return result;
        }

        /// <summary>
        /// 商品期货下单
        /// Create by: 董鹏
        /// Create Date: 2010-01-22
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultDataEntity DoCommoditiesOrder(CommoditiesDataOrderEntity model)
        {
            var result = new ResultDataEntity();
            string message;
            LogHelper.WriteDebug("撮合中心管理器商品期货下单方法：");
            if (model == null || string.IsNullOrEmpty((model.StockCode)))
            {
                message = "CH-0010:委托单不能为空或者期货代码不能为空；";
                result.Message = message;
                LogHelper.WriteDebug(message);
                return result;
            }
            //撮合中心撮合机不能为空
            if (matchDevices != null)
            {
                if (matchDevices.ContainsKey(model.StockCode.Trim()))
                {
                    return matchDevices[model.StockCode.Trim()].DoCommoditiesOrder(model);
                }
                message = "CH-0011:管理中心撮合单元没有分配或撮合中心没有分配此品种类型撮合；";
                LogHelper.WriteDebug(message);
            }
            else
            {
                message = "CH-0012:撮合中心撮合机不存在；";
                LogHelper.WriteDebug(message);
            }
            result.Message = message;
            return result;
        }

        /// <summary>
        /// 港股下单
        /// </summary>
        /// <param name="model">港股委托实体</param>
        /// <returns></returns>
        public ResultDataEntity DoHKStockOrder(HKEntrustOrderInfo model)
        {
            var result = new ResultDataEntity();
            string message;
            LogHelper.WriteDebug("撮合中心管理器港股下单方法：");
            if (model == null || string.IsNullOrEmpty((model.HKSecuritiesCode)))
            {
                message = "CH-0010:委托单不能为空或者港股代码不能为空；";
                result.Message = message;
                LogHelper.WriteDebug(message);
                return result;
            }
            if (matchDevices != null)
            {
                if (matchDevices.ContainsKey(model.HKSecuritiesCode.Trim()))
                {
                    return matchDevices[model.HKSecuritiesCode.Trim()].DoHKStockOrder(model);
                }
                message = "CH-0011:管理中心撮合单元没有分配或撮合中心没有分配此品种类型撮合；";
                LogHelper.WriteDebug(message);
            }
            else
            {
                message = "CH-0012:撮合中心撮合机不存在；";
                LogHelper.WriteDebug(message);
            }
            result.Message = message;
            return result;
        }

        #endregion

        #region 港股改单
        /// <summary>
        /// 港股改单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public HKModifyResultEntity ModifyHKOrder(HKModifyEntity model)
        {
            var result = new HKModifyResultEntity();
            string message;
            result.OrderNo = model.OldOrderNo;
            LogHelper.WriteDebug("撮合中心管理器港股下单方法：");
            try
            {
                if (model == null || string.IsNullOrEmpty(model.StockCode))
                {
                    message = "CH-0018:委托港股改单不能为空或者港股代码不能为空；";
                    result.Message = message;
                    result.IsSuccess = false;
                    LogHelper.WriteDebug(message);
                    return result;
                }
                if (matchDevices != null)
                {
                    if (matchDevices.ContainsKey(model.StockCode.Trim()))
                    {
                        return matchDevices[model.StockCode.Trim()].ModifyHKOrder(model);
                    }
                    message = "CH-0018:管理中心撮合单元没有分配或撮合中心没有分配此品种类型撮合；";
                    LogHelper.WriteDebug(message);
                }
                else
                {
                    message = "CH-0018:港股改单撮合中心撮合机不存在；";
                    LogHelper.WriteDebug(message);
                }
            }
            catch (Exception ex)
            {
                message = "CH-0018:港股改单异常";
                LogHelper.WriteError(message, ex);
            }
            result.IsSuccess = false;
            result.Message = message;
            return result;
        }


        #endregion

        #region 保存所有撤单队列
        /// <summary>
        /// 此方法为程序在关闭时调用
        /// </summary>
        public void SaveAllCancleData()
        {
            try
            {
                //目前因为初始化的问题，所以这里使用以交易所ID来遍历保存，代修改了
                //初始化的问题后再修改此方法的使用
                List<string> list = new List<string>();
                foreach (var item in this.matchDevices)
                {
                    MatchDevice dev = item.Value;
                    if (dev != null)
                    {
                        if (list.Contains(dev.bourseTypeID.ToString()))
                        {
                            continue;
                        }
                        dev.SaveAllCancel();
                        list.Add(dev.bourseTypeID.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("保存撤单队列数据异常", ex);
            }
        }
        #endregion

    }
}