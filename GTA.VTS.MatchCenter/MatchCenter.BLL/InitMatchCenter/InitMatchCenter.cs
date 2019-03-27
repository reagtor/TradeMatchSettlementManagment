using MatchCenter.BLL.ManagementCenter;
using MatchCenter.DAL.DevolveVerifyCommonService;
using MatchCenter.Entity;
using System.Collections.Generic;
using System;
using GTA.VTS.Common.CommonUtility;
using MatchCenter.BLL.Common;
using GTA.VTS.Common.CommonObject;
using MatchCenter.DAL;
using MatchCenter.BLL.RealTime;
using MatchCenter.BLL.MatchData;
using MatchCenter.BLL.Match;
using MatchCenter.DAL.HK;
using MatchCenter.Entity.HK;
namespace MatchCenter.BLL
{
    /// <summary>
    /// 初始化撮合中心类
    /// Create BY：李健华
    /// Create Date：2009-08-18
    /// Desc.：添加商品期货相关的内容
    /// Update By: 董鹏
    /// Update Date:2010-01-22
    /// </summary>
    public class InitMatchCenter //: IniitMatchCenterWork
    {

        #region 初始化撮合中心类单一进入模式
        /// <summary>
        /// 撮合中心对象
        /// </summary>
        private static InitMatchCenter instanse;
        /// <summary>
        /// 初始化撮合中心类单一进入模式
        /// </summary>
        public static InitMatchCenter Instanse
        {
            get
            {
                //撮合中心实体不能为空
                if (instanse == null)
                {
                    instanse = new InitMatchCenter();
                }
                return instanse;
            }
        }
        #endregion

        #region 启动初始化撮合中心
        /// <summary>
        /// 启动初始化撮合中心
        /// </summary>
        public void InitMatchStart()
        {
            try
            {
                AppConfig.IsAppStartInitialize = true;
                InitMatchCenterParmter();
                InitDevice();
                RealtimeMarketService.InitRealTimeStart();
                AppConfig.IsAppStartInitialize = false;
                AppConfig.IsOpenMarket = true;
            }
            catch (Exception ex)
            {
                AppConfig.IsAppStartInitialize = false;
                AppConfig.IsOpenMarket = false;

                LogHelper.WriteError("CH-0006:撮合中心开启受阻", ex);
                throw ex;
            }
        }
        #endregion

        #region 撮合中心开始初始化数据，主要是初始化管理中心的缓存数据及撮合中心要用的相关数据
        /// <summary>
        /// 撮合中心开始初始化数据，主要是初始化管理中心的缓存数据及撮合中心要用的相关数据
        /// 开始分配相关的撮合机在这里初始化开始组装分配 
        /// </summary>
        private void InitMatchCenterParmter()
        {
            if (!CommonDataCacheProxy.Instanse.TestConnManagerCenterSuccess())
            {
                return;
            }

            //CommonDataCacheProxy.Instanse.Initialize();
            CommonDataCacheProxy.Instanse.Reset();
            InitializeFuseEntity();
        }
        #endregion

        #region 初始化熔断实体把列表数据组装成新的实体
        /// <summary>
        /// 初始化熔断实体把列表数据组装成新的实体
        /// </summary>
        private void InitializeFuseEntity()
        {
            //ShowMessage.Instanse.ShowFormTitleMessage("正在初始化熔断");
            List<CM_CommodityFuse> list = CommonDataCacheProxy.Instanse.GetCacheCommodityFuse();
            if (Utils.IsNullOrEmpty(list))
                return;
            MatchCenterManager.Instance.FuseHanderEntityList.Clear();

            foreach (var fuse in list)
            {
                FuseHanderEntity fuseHanderEntity = new FuseHanderEntity();
                fuseHanderEntity.IsFuse = false;
                fuseHanderEntity.PriorFuse = false;
                fuseHanderEntity.StockCode = fuse.CommodityCode;
                fuseHanderEntity.FuseCount = 0;
                if (!MatchCenterManager.Instance.FuseHanderEntityList.ContainsKey(fuse.CommodityCode))
                {
                    MatchCenterManager.Instance.FuseHanderEntityList.Add(fuse.CommodityCode, fuseHanderEntity);
                }
            }
            //ShowMessage.Instanse.ShowFormTitleMessage("初始化熔断(完)");
        }
        #endregion

        #region 初始化撮合机
        /// <summary>
        /// 初始化撮合机
        /// </summary>
        private void InitDevice()
        {
            string showmsg = "正在初始化行撮合机";
            LogHelper.WriteDebug(showmsg + DateTime.Now);
            ShowMessage.Instanse.ShowFormTitleMessage(showmsg);
            RC_MatchCenter center = CommonDataCacheProxy.Instanse.GetCacheMatchCenterByConfig();
            if (center != null)
            {
                AppConfig.MatchCenterName = "【" + center.MatchCenterName + "】";
            }

            List<RC_MatchMachine> matchMachines = CommonDataCacheProxy.Instanse.GetCacheRC_MatchMachine();
            string matchTypeID = AppConfig.GetConfigMatchBreedClassType();
            //撮合中心撮合机不能为空
            if (Utils.IsNullOrEmpty(matchMachines))
            {
                LogHelper.WriteInfo("撮合中心初始化撮合机分配代码时没有找到撮合机--撮合中心初始化不成功！" + DateTime.Now);
                return;
            }
            int k = 0;
            showmsg += "[" + matchMachines.Count + "]==";

            #region 先清空所有撮合管理器中内容
            //先清空所有撮合管理器中内容
            //MatchCenterManager.Instance.matchDevices.Clear();
            foreach (var item in MatchCenterManager.Instance.matchDevices)
            {
                MatchDevice dev = item.Value;
                if (dev != null)
                {
                    dev.ClearAllTimerEvent();
                }
            }
            MatchCenterManager.Instance.matchDevices.Clear();
            #endregion

            #region 清除所有下单过滤行情数据列表
            MatchCodeDictionary.hk_ActivityOrderDic.Clear();
            MatchCodeDictionary.xh_ActivityOrderDic.Clear();
            MatchCodeDictionary.qh_ActivityOrderDic.Clear();
            //add by 董鹏 2010-01-25
            MatchCodeDictionary.spqh_ActivityOrderDic.Clear();
            #endregion

            foreach (RC_MatchMachine machine in matchMachines)
            {
                #region 初始货所有撮合机器
                k += 1;
                ShowMessage.Instanse.ShowFormTitleMessage(showmsg + k + "号撮合机");

                CM_BourseType bourseType = CommonDataCacheProxy.Instanse.GetCacheCM_BourseTypeByKey((int)machine.BourseTypeID);

                MatchDevice device = new MatchDevice();
                //TradeTimeManager.Instanse.AcceptStartTime = (DateTime)bourseType.ReceivingConsignStartTime;
                //TradeTimeManager.Instanse.AcceptEndTime = (DateTime)bourseType.ReceivingConsignEndTime;
                //List<CM_TradeTime> tradeTimes = CommonDataCacheProxy.Instanse.GetCacheCM_TradeTimeByBourseID(bourseType.BourseTypeID);

                List<RC_TradeCommodityAssign> assigns = CommonDataManagerOperate.GetTradeCommodityAssignByMatchineID(machine.MatchMachineID);
                if (Utils.IsNullOrEmpty(assigns))
                {
                    continue;
                }
                //撮合中心遍历撮合单元 
                foreach (RC_TradeCommodityAssign assign in assigns)
                {
                    string iniCode = assign.CommodityCode;
                    #region 根据配置初始化要撮合的商品代码
                    CM_BreedClass breedClass = CommonDataManagerOperate.GetBreedClassByCommodityCode(iniCode, assign.CodeFormSource);

                    if (breedClass != null && breedClass.BreedClassTypeID.HasValue)
                    {
                        //根据配置初始化要撮合的商品代码
                        switch ((Types.BreedClassTypeEnum)breedClass.BreedClassTypeID)
                        {
                            case Types.BreedClassTypeEnum.Stock:
                                //根据配置是否初始化本商品撮合代码
                                if (matchTypeID.Substring(3, 1) == "0")
                                {
                                    continue;
                                }
                                #region 初始化现货撮合机
                                //ShowMessage.Instanse.ShowFormTitleMessage(showmsg + "正在初始现货代码[" + assign.CommodityCode + "]撮合机");
                                //if ((Types.BreedClassTypeEnum)breedClass.BreedClassTypeID == Types.BreedClassTypeEnum.Stock)
                                //{
                                var stockMatcher = new StockMatcher(iniCode);
                                stockMatcher.bourseTypeID = bourseType.BourseTypeID;
                                device.bourseTypeID = bourseType.BourseTypeID;
                                device.IniMatchDevice((Types.BreedClassTypeEnum)breedClass.BreedClassTypeID.Value);
                                //====获取相关代码中数据库的所有已下的委托 并添加到委托队列中
                                List<StockDataOrderEntity> xhEntity = StockDataOrderDataAccess.GetStockEntityList(iniCode);
                                if (!Utils.IsNullOrEmpty(xhEntity))
                                {
                                    foreach (var entity in xhEntity)
                                    {
                                        //市场存量故障恢复
                                        if (entity.MarketVolumeNo == entity.OrderNo && entity.MarkLeft > 0
                                            && entity.MatchState != Types.MatchCenterState.First
                                            && entity.IsMarketPrice == (int)Types.MarketPriceType.otherPrice)
                                        {
                                            string stockKey = entity.SholderCode + "@" + entity.OrderPrice + "@" + entity.MarketVolumeNo;
                                            if ((Types.TransactionDirection)entity.TransactionDirection == Types.TransactionDirection.Buying)
                                            {
                                                stockMatcher.buyMarketVolume.AddMarketVolume(stockKey, entity.MarkLeft);
                                            }
                                            else if ((Types.TransactionDirection)entity.TransactionDirection == Types.TransactionDirection.Selling)
                                            {
                                                stockMatcher.sellMarketVolume.AddMarketVolume(stockKey, entity.MarkLeft);
                                            }
                                        }
                                        //添加过滤行情列表数据
                                        MatchCodeDictionary.AddXH_ActivityOrderDic(entity.StockCode);
                                        stockMatcher.EntityBuffer.InsertQueueItem(entity);
                                    }
                                }
                                //==============================
                                if (device.StockMarkers.ContainsKey(iniCode))
                                {
                                    device.StockMarkers.Remove(iniCode);
                                }
                                device.StockMarkers.Add(iniCode, stockMatcher);
                                //}
                                #endregion
                                break;
                            case Types.BreedClassTypeEnum.CommodityFuture:
                                #region 初始化商品期货撮合机 add by 董鹏 2010-01-22

                                //根据配置是否初始化本商品撮合代码
                                if (matchTypeID.Substring(0, 1) == "0")
                                {
                                    continue;
                                }

                                var cfMatcher = new SPQHMatcher(iniCode);
                                cfMatcher.bourseTypeID = bourseType.BourseTypeID;
                                device.bourseTypeID = bourseType.BourseTypeID;
                                device.IniMatchDevice((Types.BreedClassTypeEnum)breedClass.BreedClassTypeID.Value);
                                //====获取相关代码中数据库的所有已下的委托 并添加到委托队列中
                                List<CommoditiesDataOrderEntity> cfModel = CommoditiesDataOrderAccess.GetFutureEntityList(iniCode);
                                if (!Utils.IsNullOrEmpty(cfModel))
                                {
                                    foreach (var entity in cfModel)
                                    {
                                        //市场存量故障恢复
                                        if (entity.MarketVolumeNo == entity.OrderNo && entity.MarkLeft > 0
                                            && entity.MatchState != Types.MatchCenterState.First
                                             && entity.IsMarketPrice == (int)Types.MarketPriceType.otherPrice)
                                        {
                                            string futKey = entity.SholderCode + "@" + entity.OrderPrice + "@" + entity.MarketVolumeNo;
                                            if ((Types.TransactionDirection)entity.TransactionDirection == Types.TransactionDirection.Buying)
                                            {
                                                cfMatcher.buyMarketVolume.AddMarketVolume(futKey, entity.MarkLeft);
                                            }
                                            else if ((Types.TransactionDirection)entity.TransactionDirection == Types.TransactionDirection.Selling)
                                            {
                                                cfMatcher.sellMarketVolume.AddMarketVolume(futKey, entity.MarkLeft);
                                            }
                                        }
                                        //添加过滤行情列表数据
                                        MatchCodeDictionary.AddSPQH_ActivityOrderDic(entity.StockCode);
                                        cfMatcher.EntityBuffer.InsertQueueItem(entity);
                                    }
                                }
                                //========================================//
                                if (device.CommoditiesMarkers.ContainsKey(iniCode))
                                {
                                    device.CommoditiesMarkers.Remove(iniCode);
                                }
                                device.CommoditiesMarkers.Add(iniCode, cfMatcher);
                                #endregion
                                break;
                            case Types.BreedClassTypeEnum.StockIndexFuture:

                                #region 初始化股指期货撮合机
                                //ShowMessage.Instanse.ShowFormTitleMessage(showmsg + "正在初始期货代码[" + assign.CommodityCode + "]撮合机");
                                //根据配置是否初始化本商品撮合代码
                                if (matchTypeID.Substring(2, 1) == "0")
                                {
                                    continue;
                                }
                                //if ((Types.BreedClassTypeEnum)breedClass.BreedClassTypeID == Types.BreedClassTypeEnum.StockIndexFuture)
                                //{
                                var futureMatcher = new FutureMatcher(iniCode);
                                futureMatcher.bourseTypeID = bourseType.BourseTypeID;
                                device.bourseTypeID = bourseType.BourseTypeID;
                                device.IniMatchDevice((Types.BreedClassTypeEnum)breedClass.BreedClassTypeID.Value);
                                //====获取相关代码中数据库的所有已下的委托 并添加到委托队列中
                                List<FutureDataOrderEntity> qhModel = FutureDataOrderDataAccess.GetFutureEntityList(iniCode);
                                if (!Utils.IsNullOrEmpty(qhModel))
                                {
                                    foreach (var entity in qhModel)
                                    {
                                        //市场存量故障恢复
                                        if (entity.MarketVolumeNo == entity.OrderNo && entity.MarkLeft > 0
                                            && entity.MatchState != Types.MatchCenterState.First
                                             && entity.IsMarketPrice == (int)Types.MarketPriceType.otherPrice)
                                        {
                                            string futKey = entity.SholderCode + "@" + entity.OrderPrice + "@" + entity.MarketVolumeNo;
                                            if ((Types.TransactionDirection)entity.TransactionDirection == Types.TransactionDirection.Buying)
                                            {
                                                futureMatcher.buyMarketVolume.AddMarketVolume(futKey, entity.MarkLeft);
                                            }
                                            else if ((Types.TransactionDirection)entity.TransactionDirection == Types.TransactionDirection.Selling)
                                            {
                                                futureMatcher.sellMarketVolume.AddMarketVolume(futKey, entity.MarkLeft);
                                            }
                                        }
                                        //添加过滤行情列表数据
                                        MatchCodeDictionary.AddQH_ActivityOrderDic(entity.StockCode);
                                        futureMatcher.EntityBuffer.InsertQueueItem(entity);
                                    }
                                }
                                //========================================//
                                if (device.FutureMarkers.ContainsKey(iniCode))
                                {
                                    device.FutureMarkers.Remove(iniCode);
                                }
                                device.FutureMarkers.Add(iniCode, futureMatcher);
                                //}
                                #endregion
                                break;
                            case Types.BreedClassTypeEnum.HKStock:
                                #region 初始化港股撮合机
                                //根据配置是否初始化本商品撮合代码
                                if (matchTypeID.Substring(1, 1) == "0")
                                {
                                    continue;
                                }
                                //ShowMessage.Instanse.ShowFormTitleMessage(showmsg + "正在初始港股代码[" + iniCode + "]撮合机");

                                //if ((Types.BreedClassTypeEnum)breedClass.BreedClassTypeID == Types.BreedClassTypeEnum.HKStock)
                                //{
                                // LogHelper.WriteInfo("正在初始化港股撮合机" + DateTime.Now);
                                var hkMatcher = new HKStockMatcher(iniCode);
                                hkMatcher.bourseTypeID = bourseType.BourseTypeID;
                                device.bourseTypeID = bourseType.BourseTypeID;
                                device.IniMatchDevice((Types.BreedClassTypeEnum)breedClass.BreedClassTypeID.Value);

                                //====获取相关代码中数据库的所有已下的委托 并添加到委托队列中 
                                List<HKEntrustOrderInfo> hkOrders = HKEntrustOrderDal.GetHKEntrustOrderList(iniCode);
                                if (!Utils.IsNullOrEmpty(hkOrders))
                                {
                                    foreach (var entity in hkOrders)
                                    {
                                        LogHelper.WriteDebug("程序启动==【港股】初始化重新添加委托到队列中" + entity.OrderNo + "当前委托量" + entity.OrderVolume + "原委托量:" + entity.OldVolume
                                            + "代码:" + entity.HKSecuritiesCode);
                                        //市场存量故障恢复
                                        if (entity.MarketVolumeNo == entity.OrderNo && entity.MarkLeft > 0 && entity.MatchState != Types.MatchCenterState.First
                                            && entity.OrderType != (int)Types.HKPriceType.SLO)
                                        {
                                            string hkKey = entity.SholderCode + "@" + entity.OrderPrice + "@" + entity.MarketVolumeNo;
                                            if ((Types.TransactionDirection)entity.TradeType == Types.TransactionDirection.Buying)
                                            {
                                                hkMatcher.buyMarketVolume.AddMarketVolume(hkKey, entity.MarkLeft);
                                            }
                                            else if ((Types.TransactionDirection)entity.TradeType == Types.TransactionDirection.Selling)
                                            {
                                                hkMatcher.sellMarketVolume.AddMarketVolume(hkKey, entity.MarkLeft);
                                            }
                                        }
                                        //添加过滤行情列表数据
                                        MatchCodeDictionary.AddHK_ActivityOrderDic(entity.HKSecuritiesCode);
                                        hkMatcher.orderCache.InsertQueueItem(entity);
                                    }
                                }
                                //=============================//
                                if (device.HKStockMarkers.ContainsKey(iniCode))
                                {
                                    device.HKStockMarkers.Remove(iniCode);
                                }
                                device.HKStockMarkers.Add(iniCode, hkMatcher);
                                //}
                                #endregion
                                break;
                            default:
                                break;
                        }
                        //以上加载并验证通过后再删除和加入
                        if (MatchCenterManager.Instance.matchDevices.ContainsKey(iniCode))
                        {
                            MatchCenterManager.Instance.matchDevices[iniCode] = null;

                            MatchCenterManager.Instance.matchDevices.Remove(iniCode);
                        }
                        MatchCenterManager.Instance.matchDevices.Add(iniCode, device);
                        // ShowMessage.Instanse.ShowFormTitleMessage(showmsg + "初始化代码[" + iniCode + "]撮合机(完)");
                    }
                    #endregion
                }
                #endregion
            }
            //删除完所有撤单数据,因为已经把数据都放入列表，已经不删除当再重启时又会重写一次到数据库
            //这样就会重复
            CancelOrderRecoveryDal.DeleteAll();
            //清除初始化管理中心的数据中没有再使用到的数据
            CommonDataCacheProxy.Instanse.ClearOther();

            LogHelper.WriteInfo("撮合中心初始化成功！" + DateTime.Now);
        }
        #endregion



    }
}