using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatchCenter.DAL.SpotTradingDevolveService;
using MatchCenter.DAL.DevolveVerifyCommonService;
using MatchCenter.DAL.FuturesDevolveService;
//增加港股引用
using MatchCenter.DAL.HKTradingRulesService;
using MatchCenter.BLL.Common;
using GTA.VTS.Common.CommonUtility;
using GTA.VTS.Common.CommonObject;

namespace MatchCenter.BLL.ManagementCenter
{
    /// <summary>
    /// 公共数据缓存类
    /// Create BY：李健华
    /// Create Date：2009-08-18
    /// 添加根据期货品种类型缓存代码
    /// UPdate BY：李健华
    /// Update Date：2010-05-15
    /// </summary>
    public sealed class CommonDataCacheProxy : GetAllCommonDataFromManagerCenter
    {
        #region 单一实例进入模式
        /// <summary>
        /// 定义内部静态变量实现单一实例进入模式
        /// </summary>
        private static CommonDataCacheProxy _instance = new CommonDataCacheProxy();
        /// <summary>
        /// 单一进一返回唯一实例
        /// </summary>
        public static CommonDataCacheProxy Instanse
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        #region 定义缓存数据
        /// <summary>
        /// 撮合中心商品代码 
        /// </summary>
        private CacheUtil<string, CM_Commodity> commotity;
        /// <summary>
        /// 根据期货品种类型编存商品代码 
        /// </summary>
        private CacheUtil<int, List<CM_Commodity>> commotityQHByBreedClassID;
        /// <summary>
        /// 交易所列表
        /// </summary>
        private CacheUtil<int, CM_BourseType> bourseTypes;
        /// <summary>
        /// 商品类型
        /// </summary>
        private CacheUtil<int, CM_BreedClass> breedClasss;
        /// <summary>
        /// 商品类别
        /// </summary>
        private CacheUtil<int, CM_BreedClassType> breedClassTypes;
        /// <summary>
        /// 商品熔断相关设置
        /// </summary>
        private CacheUtil<string, CM_CommodityFuse> commodityFuses;
        ///// <summary>
        ///// 交易上下限值
        ///// </summary>
        //private CacheUtil<int, CM_FieldRange> fieldRanages;
        /// <summary>
        /// 熔断参数实体
        /// </summary>
        private CacheUtil<int, CM_FuseTimesection> fuseTimeSections;
        ///// <summary>
        ///// 现货交易规则_最小变动价位_范围_值
        ///// </summary>
        //private CacheUtil<string, XH_MinChangePriceValue> minChangePriceValues;
        /// <summary>
        /// 期货上下限（涨跌幅）类型
        /// </summary>
        private CacheUtil<int, QH_HighLowStopScopeType> qh_HighLowStopScopeType;
        /// <summary>
        /// 期货交易规则
        /// </summary>
        private CacheUtil<int, QH_FuturesTradeRules> qh_FuturesTradeRules;
        /// <summary>
        /// 撮合中心实体--这里只是缓存当前系统配置文件配置的IP和端品的一个撮合中心
        /// </summary>
        private CacheUtil<int, RC_MatchCenter> matchCenter;
        /// <summary>
        /// 撮合中心撮合机
        /// </summary>
        private CacheUtil<int, RC_MatchMachine> matchMachine;
        /// <summary>
        /// 撮合机所分配的交易商品
        /// </summary>
        private CacheUtil<string, RC_TradeCommodityAssign> tradeCommodityAssign;
        /// <summary>
        /// 现货交易规则
        /// </summary>
        private CacheUtil<int, XH_SpotTradeRules> spotTradeRules;
        /// <summary>
        /// 交易所类型交易时间--改由以下以交易所ID为主键
        /// </summary>
        private CacheUtil<int, List<CM_TradeTime>> tradeTimes;
        /// <summary>
        /// 现货有效申报类型
        /// </summary>
        private CacheUtil<int, XH_ValidDeclareType> validDeclareTypes;
        /// <summary>
        /// 现货有效申报类型值
        /// </summary>
        private CacheUtil<int, XH_ValidDeclareValue> validDeclareValues;
        /// <summary>
        /// 现货上下限（涨跌幅）类型
        /// </summary>
        private CacheUtil<int, XH_SpotHighLowControlType> xh_SpotHighLowControlType;
        /// <summary>
        /// 现货上下限（涨跌幅）类型值
        /// </summary>
        private CacheUtil<int, XH_SpotHighLowValue> xh_SpotHighLowValues;
        /// <summary>
        /// 增发商品(股票)列表
        /// </summary>
        private CacheUtil<string, ZFInfo> zFCommodity;
        /// <summary>
        /// 非交易日列表 根据交易所编号+@+日期组成key
        /// </summary>
        private CacheUtil<string, CM_NotTradeDate> cm_notTradeDate;

        /// <summary>
        /// 期货的所有最后交易日数据列表
        /// </summary>
        private CacheUtil<int, QH_LastTradingDay> qh_LastTradingDay;

        #region 港股

        /// <summary>
        /// 港股最小变动价位
        /// </summary>
        private CacheUtil<int, HK_MinPriceFieldRange> hk_MinPriceFiled;

        /// <summary>
        /// 港股交易规则
        /// </summary>
        private CacheUtil<int, HK_SpotTradeRules> hk_spotTradeRules;
        /// <summary>
        /// 港股代码列表
        /// </summary>
        private CacheUtil<string, HK_Commodity> hk_commodity;
        ///// <summary>
        ///// 港股交易费用
        ///// </summary>
        //private CacheUtil<int, HK_SpotCosts> hkSpotCosts;
        #endregion

        #endregion

        #region 构造函数开始初始化内部变量
        /// <summary>
        /// 构造函数开始初始化内部变量
        /// </summary>
        public CommonDataCacheProxy()
        {
            commotity = new CacheUtil<string, CM_Commodity>();
            commotityQHByBreedClassID = new CacheUtil<int, List<CM_Commodity>>();
            bourseTypes = new CacheUtil<int, CM_BourseType>();
            breedClasss = new CacheUtil<int, CM_BreedClass>();
            breedClassTypes = new CacheUtil<int, CM_BreedClassType>();
            commodityFuses = new CacheUtil<string, CM_CommodityFuse>();
            //fieldRanages = new CacheUtil<int, CM_FieldRange>();
            fuseTimeSections = new CacheUtil<int, CM_FuseTimesection>();
            //minChangePriceValues = new CacheUtil<string, XH_MinChangePriceValue>();
            qh_HighLowStopScopeType = new CacheUtil<int, QH_HighLowStopScopeType>();
            qh_FuturesTradeRules = new CacheUtil<int, QH_FuturesTradeRules>();
            matchMachine = new CacheUtil<int, RC_MatchMachine>();
            matchCenter = new CacheUtil<int, RC_MatchCenter>();
            tradeCommodityAssign = new CacheUtil<string, RC_TradeCommodityAssign>();
            spotTradeRules = new CacheUtil<int, XH_SpotTradeRules>();
            tradeTimes = new CacheUtil<int, List<CM_TradeTime>>();
            validDeclareTypes = new CacheUtil<int, XH_ValidDeclareType>();
            validDeclareValues = new CacheUtil<int, XH_ValidDeclareValue>();
            xh_SpotHighLowControlType = new CacheUtil<int, XH_SpotHighLowControlType>();
            xh_SpotHighLowValues = new CacheUtil<int, XH_SpotHighLowValue>();
            zFCommodity = new CacheUtil<string, ZFInfo>();
            cm_notTradeDate = new CacheUtil<string, CM_NotTradeDate>();
            qh_LastTradingDay = new CacheUtil<int, QH_LastTradingDay>();
            //港股初始化
            hk_MinPriceFiled = new CacheUtil<int, HK_MinPriceFieldRange>();
            hk_spotTradeRules = new CacheUtil<int, HK_SpotTradeRules>();
            hk_commodity = new CacheUtil<string, HK_Commodity>();
            //hkSpotCosts = new CacheUtil<int, HK_SpotCosts>();
        }
        #endregion

        #region 向内部缓存数据添加数据初始化
        /// <summary>
        /// 向内部缓存数据添加数据初始化
        /// 程序启动时对些方法调用一次
        /// </summary>
        public void Initialize()
        {
            try
            {
                ShowMessage.Instanse.ShowFormTitleMessage("正在获取管理中心数据初始化内部参数");

                commotity.Fill(key => key.CommodityCode, GetAllCommodity());
                bourseTypes.Fill(key => key.BourseTypeID, GetAllBourseType());
                breedClasss.Fill(key => key.BreedClassID, GetAllBreedClass());
                breedClassTypes.Fill(key => key.BreedClassTypeID, GetAllBreedClassType());
                //缓存根据期货品种类型缓存相关代码
                privateInitCommotityByBreedClassID();
                //===========

                commodityFuses.Fill(key => key.CommodityCode, GetAllCommodityFuse());
                //fieldRanages.Fill(key => key.FieldRangeID, GetAllFieldRange());
                fuseTimeSections.Fill(key => key.TimesectionID, GetAllFuseTimesection());
                //minChangePriceValues.Fill(key => key.BreedClassID.ToString() + "@" + key.FieldRangeID.ToString(), GetAllMinChangePriceValue());
                qh_HighLowStopScopeType.Fill(key => key.HighLowStopScopeID, GetAllQH_HighLowStopScopeType());
                qh_FuturesTradeRules.Fill(key => key.BreedClassID, GetAllQH_FutureTradeRules());
                RC_MatchCenter model = GetMatchCenterByAddress();
                int matchCenterID = model.MatchCenterID;
                matchCenter.Add(matchCenterID, model);
                matchMachine.Fill(key => key.MatchMachineID, GetAllMatchMachineByMatchCenterID(matchCenterID));
                tradeCommodityAssign.Fill(key => key.CommodityCode, GetAllTradeCommodityAssign());
                spotTradeRules.Fill(key => key.BreedClassID, GetAllSpotTradeRules());
                //tradeTimes.Fill(key => key.TradeTimeID, GetAllTradeTime());
                //改由以下以交易所ID为主键 
                privateInitTradeTime();
                validDeclareTypes.Fill(key => key.BreedClassValidID, GetAllValidDeclareType());
                validDeclareValues.Fill(key => key.ValidDeclareValueID, GetAllValidDeclareValue());
                xh_SpotHighLowControlType.Fill(key => key.BreedClassHighLowID, GetAllSpotHighLowControlType());
                xh_SpotHighLowValues.Fill(key => key.HightLowValueID, GetAllSpotHighLowValue());
                zFCommodity.Fill(key => key.stkcd.ToString() + "@" + key.paydt.ToString(), GetAllZFCommodity());
                //获取所有非交易日的日期
                cm_notTradeDate.Fill(key => key.BourseTypeID.Value + "@" + key.NotTradeDay.Value.ToString("yyyy-MM-dd"), GetAllCMNotTradeDate());
                qh_LastTradingDay.Fill(key => key.LastTradingDayID, GetAllLastTradingDay());
                #region 港股部分

                hk_MinPriceFiled.Fill(key => key.FieldRangeID, GetHKMinChangePriceFieldRange());
                hk_spotTradeRules.Fill(key => key.BreedClassID, GetHKSpotTradeRules());
                hk_commodity.Fill(key => key.HKCommodityCode, GetAllHKCommodity());
                //hkSpotCosts.Fill(key => key.BreedClassID, get());
                #endregion
                ShowMessage.Instanse.ShowFormTitleMessage("获取管理中心数据初始化内部参数(完)");
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-0002:初始化缓存数据异常0001", ex);
                ShowMessage.Instanse.ShowFormTitleMessage("获取管理中心数据初始化内部参数异常");
                throw ex;
            }
        }
        #endregion

        #region 初始货内部重组方法
        /// <summary>
        /// 内部方法初始交易时间
        /// 初始交易时间要先初始化交易所，
        /// 因为这里是取交易所ID来作主键，如果交易所没有交易时间，这里不添加记录
        /// </summary>
        private void privateInitTradeTime()
        {
            List<CM_BourseType> btList = bourseTypes.GetAll();
            List<CM_TradeTime> ttList = GetAllTradeTime();
            tradeTimes.Reset();//重新初始化先清空再ADD
            if (Utils.IsNullOrEmpty(btList) || Utils.IsNullOrEmpty(ttList))
            {
                return;
            }
            foreach (var item in btList)
            {
                var cacheList = new List<CM_TradeTime>();
                foreach (var model in ttList)
                {
                    if ((int)model.BourseTypeID == item.BourseTypeID)
                    {
                        cacheList.Add(model);
                    }
                }
                if (!Utils.IsNullOrEmpty(cacheList))
                {
                    tradeTimes.Add(item.BourseTypeID, cacheList);
                }
            }

        }

        /// <summary>
        /// 根据商品类别缓存相关代码，目前这里只缓存期货类型下的相关类别,目前现货没有必要缓存
        /// 调用此方法要先初始货品种类型与类别再调用些方法
        /// </summary>
        private void privateInitCommotityByBreedClassID()
        {
            List<CM_BreedClass> list = breedClasss.GetAll();
            foreach (var item in list)
            {
                if (!item.BreedClassTypeID.HasValue)
                {
                    continue;
                }

                //目前只缓存股指期货或者是商品期货的代码
                if ((Types.BreedClassTypeEnum)item.BreedClassTypeID.Value == Types.BreedClassTypeEnum.CommodityFuture
                    || (Types.BreedClassTypeEnum)item.BreedClassTypeID.Value == Types.BreedClassTypeEnum.StockIndexFuture)
                {
                    List<CM_Commodity> model = new List<CM_Commodity>();
                    model = GetAllCommodityByBreedClass(item.BreedClassID);
                    if (model != null && model.Count > 0)
                    {
                        commotityQHByBreedClassID.Add(item.BreedClassID, model);
                    }
                }
            }
        }
        #endregion

        #region 重新初始化
        /// <summary>
        /// 重新初始化缓存数据，这里先是调用Remove清空所有后再从管理中心获取相关数据再Fill
        /// </summary>
        public void Reset()
        {
            Remove();
            Initialize();
        }
        #endregion

        #region 清空所有列表
        /// <summary>
        /// 清空所有列表
        /// </summary>
        public void Remove()
        {
            commotity.Reset();
            commotityQHByBreedClassID.Reset();

            bourseTypes.Reset();
            breedClasss.Reset();
            breedClassTypes.Reset();
            commodityFuses.Reset();
            //fieldRanages.Reset();
            fuseTimeSections.Reset();
            //minChangePriceValues.Reset();
            qh_HighLowStopScopeType.Reset();
            qh_FuturesTradeRules.Reset();

            matchCenter.Reset();
            matchMachine.Reset();
            tradeCommodityAssign.Reset();
            spotTradeRules.Reset();
            tradeTimes.Reset();
            validDeclareTypes.Reset();
            validDeclareValues.Reset();
            xh_SpotHighLowControlType.Reset();
            xh_SpotHighLowValues.Reset();
            zFCommodity.Reset();
            cm_notTradeDate.Reset();
            qh_LastTradingDay.Reset();
            //港股
            hk_MinPriceFiled.Reset();
            hk_spotTradeRules.Reset();
            hk_commodity.Reset();
        }
        #endregion

        #region 获取相关数据
        #region 1.获取缓存表中的熔断列表
        /// <summary>
        /// 获取缓存表中的熔断列表
        /// </summary>
        /// <returns></returns>
        public List<CM_CommodityFuse> GetCacheCommodityFuse()
        {
            return commodityFuses.GetAll();
        }
        #endregion

        #region 1.根据商品代码获取缓存表中的熔断实体
        /// <summary>
        /// 根据商品代码获取缓存表中的熔断实体
        /// </summary>
        /// <returns></returns>
        public CM_CommodityFuse GetCacheCommodityFuseByCode(string code)
        {
            return commodityFuses.GetByKey(code);
        }
        #endregion

        #region 2.根据商品Code获取商品信息实体
        /// <summary>
        /// 根据商品Code获取商品信息实体
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns></returns>
        public CM_Commodity GetCacheCommodityByCode(string code)
        {
            return commotity.GetByKey(code);
        }
        #endregion

        #region 3.根据商品代码获取是增发股实体
        /// <summary>
        /// 根据商品代码获取是增发股实体
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns></returns>
        public ZFInfo GetCacheZFInfoByCode(string code)
        {
            return zFCommodity.GetByKey(code);
        }
        #endregion

        #region 4.根据主键商品类别ID获取现货交易规则
        /// <summary>
        /// 根据主键商品类别ID获取现货交易规则
        /// </summary>
        /// <param name="breedClassID"></param>
        /// <returns></returns>
        public XH_SpotTradeRules GetCacheXH_SpotTradeRulesByKey(int breedClassID)
        {
            return spotTradeRules.GetByKey(breedClassID);
        }
        #endregion

        #region 5.根据ID获取有效申报类型
        /// <summary>
        /// 根据ID获取有效申报类型
        /// </summary>
        /// <param name="breedClassValidID">有效申报id即BreedClassValidID</param>
        /// <returns></returns>
        public XH_ValidDeclareType GetCacheXH_ValideDeclareTypeByID(int breedClassValidID)
        {
            return validDeclareTypes.GetByKey(breedClassValidID);
        }
        #endregion

        #region 5.获取有效申报类型值列表
        /// <summary>
        /// 获取有效申报类型值列表
        /// </summary>
        /// <returns></returns>
        public List<XH_ValidDeclareValue> GetCacheValidDeclareValues()
        {
            return validDeclareValues.GetAll();
        }
        #endregion

        #region 6.根据ID获取现货上下限（涨跌幅）类型
        /// <summary>
        /// 根据ID获取现货上下限（涨跌幅）类型
        /// </summary>
        /// <param name="breedClassHighLowID">涨跌幅类型id即breedClassHighLowID</param>
        /// <returns></returns>
        public XH_SpotHighLowControlType GetCacheXH_SpotHighLowControlTypeByKey(int breedClassHighLowID)
        {
            return xh_SpotHighLowControlType.GetByKey(breedClassHighLowID);
        }
        #endregion

        #region 7.获取现货上下限（涨跌幅）类型值列表
        /// <summary>
        /// 获取现货上下限（涨跌幅）类型值列表
        /// </summary>
        /// <returns></returns>
        public List<XH_SpotHighLowValue> GetCacheXH_SpotHighLowValue()
        {
            return xh_SpotHighLowValues.GetAll();
        }
        #endregion

        #region 8.获取交易规则上下限范围值列表
        ///// <summary>
        ///// 8.获取交易规则上下限范围值列表
        ///// </summary>
        ///// <returns></returns>
        //public List<CM_FieldRange> GetCacheCM_FileldRange()
        //{
        //    return fieldRanages.GetAll();
        //}
        #endregion

        #region 9.根据品种ID和范围ID获取现货交易规则_最小变动价位_范围_值
        ///// <summary>
        ///// 根据主键获取现货交易规则_最小变动价位_范围_值
        ///// </summary>
        ///// <param name="breedClassID">品种id</param>
        ///// <param name="fieldRangeID">范围id</param>
        ///// <returns></returns>
        //public XH_MinChangePriceValue GetCacheMinChangePriceValueByKey(int breedClassID, int fieldRangeID)
        //{
        //    return minChangePriceValues.GetByKey(breedClassID.ToString() + "@" + fieldRangeID.ToString());
        //}
        #endregion

        #region 10.根据商品类别ID获取商品类型实体
        /// <summary>
        /// 根据商品类别ID获取商品类型实体
        /// </summary>
        /// <param name="breedClassID"></param>
        /// <returns></returns>
        public CM_BreedClass GetCacheCM_BreedClassByKey(int breedClassID)
        {
            return breedClasss.GetByKey(breedClassID);
        }
        #endregion

        #region 11.获取撮合机所分配的交易商品列表
        ///<summary>
        /// 获取撮合机所分配的交易商品列表
        /// </summary>
        public List<RC_TradeCommodityAssign> GetCacheTradeCommodityAssign()
        {
            return tradeCommodityAssign.GetAll();
        }
        #endregion

        #region 12.根据主键（交易所ID）ID获取交易所实体
        /// <summary>
        /// 根据主键（交易所ID）ID获取交易所实体
        /// </summary>
        /// <param name="bourseTypeID">主键</param>
        /// <returns></returns>
        public CM_BourseType GetCacheCM_BourseTypeByKey(int bourseTypeID)
        {
            return bourseTypes.GetByKey(bourseTypeID);
        }
        #endregion

        #region 13.根据交易所ID(即主键)获取此交易所的所有交易时间
        /// <summary>
        /// 根据交易所ID(即主键)获取此交易所的所有交易时间
        /// 因为这里缓存时是以交易所ID来缓存的
        /// </summary>
        /// <returns></returns>
        public List<CM_TradeTime> GetCacheCM_TradeTimeByBourseID(int bourseID)
        {
            return tradeTimes.GetByKey(bourseID);
        }
        #endregion

        #region 13.获取所有撮合机列表
        /// <summary>
        /// 获取所有撮合机列表--此列表数据在初始化时只是初始化本撮合中心下的所有撮合机并不是包含数据库中的所有撮合机
        /// </summary>
        /// <returns></returns>
        public List<RC_MatchMachine> GetCacheRC_MatchMachine()
        {
            return matchMachine.GetAll();
        }
        #endregion

        #region 14.根据主键商品类别ID获取期货交易规则
        /// <summary>
        /// 根据主键商品类别ID获取期货交易规则
        /// </summary>
        /// <param name="breedClassID"></param>
        /// <returns></returns>
        public QH_FuturesTradeRules GetCacheQH_FuturesTradeRulesByKey(int breedClassID)
        {
            return qh_FuturesTradeRules.GetByKey(breedClassID);
        }
        #endregion

        #region 15.获取熔断参数列表
        /// <summary>
        /// 获取熔断参数列表
        /// </summary>
        /// <returns></returns>
        public List<CM_FuseTimesection> GetCacheCM_FuseTimesection()
        {
            return fuseTimeSections.GetAll();
        }
        #endregion

        #region 16.根据交易所ID和非交易日期(即主键)获取此非交易日期
        /// <summary>
        /// 根据主键(交易所ID和非交易日期)获取此非交易日期
        /// 因为这里缓存时是以所ID+@+非交易日期来缓存的
        /// </summary>
        /// <returns></returns>
        public CM_NotTradeDate GetCacheNotTradeDateByKey(string key)
        {
            return cm_notTradeDate.GetByKey(key);
        }
        #endregion

        #region 17.港股-获取港股最小交易价位
        /// <summary>
        /// 获取港股最小交易价位
        /// </summary>
        /// <returns></returns>
        public List<HK_MinPriceFieldRange> GetAllHKMinChangePriceFieldRange()
        {
            return hk_MinPriceFiled.GetAll();
        }
        #endregion

        #region 18.港股-获取港股所有列表
        /// <summary>
        /// 获取所有港股实体
        /// </summary>
        /// <returns></returns>
        public List<HK_Commodity> GetCacheAllHKCommodity()
        {
            return hk_commodity.GetAll();
        }
        #endregion

        #region 19.港股-根据代码获取港股代码实体
        /// <summary>
        /// 根据商品Code获取港股信息实体
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns></returns>
        public HK_Commodity GetCacheHKCommodityByCode(string code)
        {
            return hk_commodity.GetByKey(code);
        }
        #endregion

        #region 20.获取所有交易时间
        /// <summary>
        /// 获取所有交易时间
        /// </summary>
        /// <returns></returns>
        public List<CM_TradeTime> GetCacheALLCM_TradeTime()
        {
            List<CM_TradeTime> list = new List<CM_TradeTime>();
            List<List<CM_TradeTime>> getall = tradeTimes.GetAll();
            foreach (var item in getall)
            {
                list.AddRange(item);
            }
            return list;
        }
        #endregion

        #region 21.根据品种类型获取所有商品代码列表(目前此列表只缓存了期货的代码)
        /// <summary>
        /// 根据品种类型获取所有商品代码列表(目前此列表只缓存了期货的代码)
        /// </summary>
        /// <param name="breedClassID">代码</param>
        /// <returns></returns>
        public List<CM_Commodity> GetCacheCommodityByBreedClassID(int breedClassID)
        {
            return commotityQHByBreedClassID.GetByKey(breedClassID); ;
        }
        #endregion

        #region 22.检查与管理中心连接是否成功
        /// <summary>
        /// 检查是否成功
        /// </summary>
        /// <returns></returns>
        public bool TestConnManagerCenterSuccess()
        {
            return IsConnManagerCenterSuccess();
        }

        #endregion

        #region 21.根据配置文件中IP和端口获取撮合中心
        /// <summary>
        /// 根据配置文件中IP和端口获取撮合中心
        /// </summary>
        /// <returns></returns>
        public RC_MatchCenter GetCacheMatchCenterByConfig()
        {

            List<RC_MatchCenter> list = matchCenter.GetAll();
            foreach (var item in list)
            {
                if (item.IP == AppConfig.GetConfigMatchCenterIP() && item.Port.Value == AppConfig.GetConfigMatchCenterPort())
                {
                    return item;
                }
            }
            return null;
        }
        #endregion

        #region 22.根据期货最后交易日记录ID获取最后交易日记录
        /// <summary>
        /// 获取最后交易日记录
        /// </summary>
        /// <returns></returns>
        public QH_LastTradingDay GetCacheQH_LastTradingDayByID(int id)
        {
            return qh_LastTradingDay.GetByKey(id);
        }
        #endregion

        #endregion

        #region 由于一些数据只是初始化时所用到，初始化完成后不用再保存在缓存中，所以进行清除
        /// <summary>
        /// 由于一些数据只是初始化时所用到，初始化完成后不用再保存在缓存中，所以进行清除
        /// 目前清除：撮合机所分配的交易商品列表tradeCommodityAssign
        /// </summary>
        public void ClearOther()
        {
            tradeCommodityAssign.Reset();
        }
        #endregion
    }
}
