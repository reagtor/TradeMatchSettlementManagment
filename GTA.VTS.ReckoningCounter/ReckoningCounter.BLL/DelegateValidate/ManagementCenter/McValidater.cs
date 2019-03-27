#region Using Namespace

using System;
using GTA.VTS.Common.CommonObject;
using ReckoningCounter.BLL.Delegatevalidate.ManagementCenter;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.Entity;

#endregion

namespace ReckoningCounter.BLL.DelegateValidate.ManagementCenter
{
    /// <summary>
    /// 描述：根据交易商品品种类型调用现货或期货方法
    /// 作者：宋涛
    /// 日期：2008-11-20
    /// </summary>
    public class McValidater : IMcValidater, IMTradingRule
    {
        private static McValidater instance = new McValidater();
        private static object lockObject = new object();

        //private DealerTradeBreedRight _DealerTradeBreedRight;
        private FutureTradingRuleValidater _FutureTradingRuleValidater;
        private StockTradingRuleValidater _StockTradingRuleValidater;
        private HKStockTradingRuleValidater _HKStockTradingRuleValidater;
        private HKModifyOrderRuleContainer _HKModifyOrderRuleContainer;

        private McValidater()
        {
        }

        /// <summary>
        /// 股票规则验证器
        /// </summary>
        public StockTradingRuleValidater StockTradingRuleValidater
        {
            get
            {
                if (_StockTradingRuleValidater == null)
                    _StockTradingRuleValidater = new StockTradingRuleValidater();

                return _StockTradingRuleValidater;
            }
        }

        /// <summary>
        /// 期货规则验证器
        /// </summary>
        public FutureTradingRuleValidater FutureTradingRuleValidater
        {
            get
            {
                if (_FutureTradingRuleValidater == null)
                    _FutureTradingRuleValidater = new FutureTradingRuleValidater();

                return _FutureTradingRuleValidater;
            }
        }

        /// <summary>
        /// 港股规则验证器
        /// </summary>
        public HKStockTradingRuleValidater HKStockTradingRuleValidater
        {
            get
            {
                if (_HKStockTradingRuleValidater == null)
                    _HKStockTradingRuleValidater = new HKStockTradingRuleValidater();

                return _HKStockTradingRuleValidater;
            }
        }
        /// <summary>
        /// 港股改单规则验证器
        /// </summary>
        public HKModifyOrderRuleContainer HKModifyOrderRuleContainer
        {
            get
            {
                if (_HKModifyOrderRuleContainer == null)
                    _HKModifyOrderRuleContainer = new HKModifyOrderRuleContainer();

                return _HKModifyOrderRuleContainer;
            }
        }

        #region IMTradingRule 成员

        /// <summary>
        /// 根据商品代码，商品类型获取某一现货商品的费用计算公式
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>是否成功</returns>
        public StockRuleContainer GetStockRuleContainerByCode(string code)
        {
            int? breedClassID = MCService.CommonPara.GetBreedClassIdByCommodityCode(code);

            if (breedClassID.HasValue)
            {
                return this.StockTradingRuleValidater.GetContainer(breedClassID.Value);
            }

            return null;
        }

        /// <summary>
        /// 根据商品代码，商品类型获取某一期货商品的费用计算公式
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>是否成功</returns>
        public FutureRuleContainer GetFutureRuleContainerByCode(string code)
        {
            int? breedClassID = MCService.CommonPara.GetBreedClassIdByCommodityCode(code);

            if (breedClassID.HasValue)
            {
                return this.FutureTradingRuleValidater.GetContainer(breedClassID.Value);
            }

            return null;
        }

        /// <summary>
        /// 根据商品代码，商品类型获取某一港股商品的费用计算公式
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>是否成功</returns>
        public HKStockRuleContainer GetHKStockRuleContainerByCode(string code)
        {
            int? breedClassID = MCService.CommonPara.GetBreedClassIdByCommodityCode(code);

            if (breedClassID.HasValue)
            {
                return this.HKStockTradingRuleValidater.GetContainer(breedClassID.Value);
            }
            return null;
        }

        #endregion

        #region Implementation of IMcValidater

        #region  股票（现货）委托规则检验
        /// <summary>
        /// 股票（现货）委托规则检验
        /// </summary>
        /// <param name="request">股票委托对象</param>
        /// <param name="errMsg">返回错误检验信息</param>
        /// <returns>是否成功</returns>
        public bool ValidateStockTradeRule(StockOrderRequest request, ref string errMsg)
        {
            CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByCommodityCode(request.Code);

            bool result = false;

            if (breedClass != null)
            {
                int breedClassTypeID = breedClass.BreedClassTypeID.Value;
                int breedClassID = breedClass.BreedClassID;

                if (breedClassTypeID != (int)Types.BreedClassTypeEnum.Stock)
                    return false;

                result = StockTradingRuleValidater.Validate(request, ref errMsg,
                                                            Convert.ToInt32(breedClassID));
            }

            return result;
        }
        #endregion

        #region 期货委托规则检验
        /// <summary>
        /// 期货委托规则检验
        /// </summary>
        /// <param name="request">期货委托对象</param>
        /// <param name="errMsg">返回错误检验信息</param>
        /// <returns>是否成功</returns>
        public bool ValidateFutureTradeRule(MercantileFuturesOrderRequest request, ref string errMsg)
        {
            CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByCommodityCode(request.Code);

            bool result = false;

            if (breedClass != null)
            {
                int breedClassTypeID = breedClass.BreedClassTypeID.Value;
                int breedClassID = breedClass.BreedClassID;

                //if (breedClassTypeID != (int) Types.BreedClassTypeEnum.CommodityFuture)
                if (breedClassTypeID != (int)Types.BreedClassTypeEnum.CommodityFuture && breedClassTypeID != (int)Types.BreedClassTypeEnum.StockIndexFuture)
                    return false;

                result = FutureTradingRuleValidater.Validate(request, breedClassID, ref errMsg);
            }

            return result;
        }
        #endregion

        #region 股指期货委托规则检验
        /// <summary>
        /// 股指期货委托规则检验
        /// </summary>
        /// <param name="request">股指期货委托对象</param>
        /// <param name="errMsg">返回错误检验信息</param>
        /// <returns>是否成功</returns>
        public bool ValidateStockIndexFutureTradeRule(StockIndexFuturesOrderRequest request, ref string errMsg)
        {
            //目前股指期货的校验使用期货的校验
            MercantileFuturesOrderRequest futuresOrderRequest = MCService.GetFuturesOrderRequest(request);


            return ValidateFutureTradeRule(futuresOrderRequest, ref errMsg);
        }
        #endregion

        #region 港股委托规则检验
        /// <summary>
        /// 港股委托规则检验
        /// </summary>
        /// <param name="request">股票委托对象</param>
        /// <param name="errMsg">返回错误检验信息</param>
        /// <returns>是否成功</returns>
        public bool ValidateHKStockTradeRule(HKOrderRequest request, ref string errMsg)
        {
            CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByCommodityCode(request.Code, Types.BreedClassTypeEnum.HKStock);

            bool result = false;

            if (breedClass != null)
            {
                if (!breedClass.BreedClassTypeID.HasValue || breedClass.BreedClassTypeID.Value != (int)Types.BreedClassTypeEnum.HKStock)
                {
                    errMsg = "当前代码不为港股类别--" + breedClass.BreedClassTypeID.Value;
                    return false;
                }
                result = HKStockTradingRuleValidater.Validate(request, ref errMsg, Convert.ToInt32(breedClass.BreedClassID));
            }
            return result;
        }
        #endregion

        #region 港股改单委托规则检验
        /// <summary>
        /// 港股改单委托规则检验
        /// </summary>
        /// <param name="request">改单请求实体</param>
        /// <param name="entrusInfo">原委托实体对象</param>
        /// <param name="errMsg">返回错误检验信息</param>
        /// <returns>是否成功</returns>
        public bool ValidateHKModifyOrderRule(HKModifyOrderRequest request,  ReckoningCounter.Entity.Model.HK.HK_TodayEntrustInfo entrusInfo, ref string errMsg)
        {
            CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByCommodityCode(request.Code, Types.BreedClassTypeEnum.HKStock);

            bool result = false;

            if (breedClass != null)
            {
                if (!breedClass.BreedClassTypeID.HasValue || breedClass.BreedClassTypeID.Value != (int)Types.BreedClassTypeEnum.HKStock)
                {
                    errMsg = "当前代码不为港股类别--" + breedClass.BreedClassTypeID.Value;
                    return false;
                }
                result = HKModifyOrderRuleContainer.Validate(request, entrusInfo,breedClass.BreedClassID, ref errMsg);
            }
            return result;
        }
        #endregion


        #endregion

        /// <summary>
        /// 交易员权限验证器
        /// </summary>
        //public DealerTradeBreedRight DealerTradeBreedRight
        //{
        //    get
        //    {
        //        if (_DealerTradeBreedRight == null)
        //            _DealerTradeBreedRight = new DealerTradeBreedRight();

        //        return _DealerTradeBreedRight;
        //    }
        //}

        public static McValidater GetInstance()
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new McValidater();
                    }
                }
            }

            return instance;
        }


    }
}