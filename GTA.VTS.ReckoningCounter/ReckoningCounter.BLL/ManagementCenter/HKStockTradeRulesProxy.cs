using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTA.VTS.Common.CommonObject;
using ReckoningCounter.DAL.HKTradingRulesService;

namespace ReckoningCounter.BLL.ManagementCenter
{
    /// <summary>
    /// 对港股交易规则对外接口的缓存包装，错误编码8100-8199
    /// 作者：李健华
    /// 日期：2009-10-21
    /// </summary>
    public class HKStockTradeRulesProxy
    {
        #region IHKStockTradeRules Fields

        private static HKStockTradeRulesProxy instance = new HKStockTradeRulesProxy();

        public static HKStockTradeRulesProxy GetInstance()
        {
            return instance;
        }
        #endregion

        #region 定义

        /// <summary>
        /// 港股商品代码缓存
        /// </summary>
        private WCFCacheObject<string, HK_Commodity> hkCommodityObj;
        /// <summary>
        /// 港股交易费用
        /// </summary>
        private WCFCacheObject<int, HK_SpotCosts> spotCostsObj;
        /// <summary>
        /// 港股交易规则
        /// </summary>
        private WCFCacheObject<int, HK_SpotTradeRules> spotTradeRulesObj;
        /// <summary>
        /// 港股最小变动价格字段港围
        /// </summary>
        private WCFCacheObjectWithGetAll<int, HK_MinPriceFieldRange> minPriceFieldRangeObj;


        #endregion

        private HKStockTradeRulesProxy()
        {
            hkCommodityObj = new WCFCacheObject<string, HK_Commodity>(GetAllHKCommodityFromWCF, GetHKCommodityByCommodityCodeFromWCF, Val => Val.HKCommodityCode);

            spotCostsObj = new WCFCacheObject<int, HK_SpotCosts>(GetAllSpotCostsFromWCF, GetSpotCostsByBreedClassIDFromWCF, val => val.BreedClassID);

            spotTradeRulesObj = new WCFCacheObject<int, HK_SpotTradeRules>(GetAllSpotTradeRulesFromWCF, GetSpotTradeRulesByBreedClassIDFromWCF, val => val.BreedClassID);

            minPriceFieldRangeObj = new WCFCacheObjectWithGetAll<int, HK_MinPriceFieldRange>(GetAllHKMinPriceFieldRangeFromWCF, val => val.FieldRangeID);

        }

        /// <summary>
        /// 进行预加载
        /// </summary>
        public void Initialize()
        {
            GetAllHKCommodity();
            GetAllSpotCosts();
            GetAllSpotTradeRules();
            GetAllHKMinPriceFieldRange();

        }
        /// <summary>
        /// 重置（清空）
        /// </summary>
        public void Reset()
        {
            hkCommodityObj.Reset();
            spotCostsObj.Reset();
            spotTradeRulesObj.Reset();
            minPriceFieldRangeObj.Reset();
        }

        private HKTradeRulesClient GetClient()
        {
            HKTradeRulesClient client;
            try
            {
                client = new HKTradeRulesClient();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8100";
                string errMsg = "无法获取管理中心提供的服务[IHKTradeRules]。";
                throw new VTException(errCode, errMsg, ex);
            }

            return client;
        }

        #region ISportTradeRules

        #region HK_Commodity
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IList<HK_Commodity> GetAllHKCommodityFromWCF()
        {
            try
            {
                using (HKTradeRulesClient client = GetClient())
                {
                    return client.GetAllHKCommodity();
                }
            }
            catch (Exception ex)
            {
                string errCode = "GT-8003";
                string errMsg = "无法从管理中心获取所有港股商品列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        private HK_Commodity GetHKCommodityByCommodityCodeFromWCF(string code)
        {
            try
            {
                using (HKTradeRulesClient client = GetClient())
                    return client.GetHKCommodityByHKCommodityCode(code);
            }
            catch (Exception ex)
            {
                string errCode = "GT-8004";
                string errMsg = "无法根据港股商品代码从管理中心获取指定港股商品。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        /// <summary>
        /// 获取所有的港股交易商品
        /// </summary>
        /// <returns>港股交易商品列表</returns>
        public IList<HK_Commodity> GetAllHKCommodity()
        {
            return hkCommodityObj.GetAll();
        }

        /// <summary>
        /// 获取所有港股的交易商品
        /// </summary>
        /// <param name="reLoad">是否从WCF重新加载</param>
        /// <returns>交易商品列表</returns>
        public IList<HK_Commodity> GetAllHKCommodity(bool reLoad)
        {
            return hkCommodityObj.GetAll(reLoad);
        }

        /// <summary>
        /// 根据港股商品代码获取港股商品实体
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>商品</returns>
        public HK_Commodity GetHKCommodityByCommodityCode(string code)
        {
            return hkCommodityObj.GetByKey(code);
        }

        /// <summary>
        /// 根据港股商品代码获取港股对应转换单位股的量
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>商品</returns>
        public int GetHKPerHandThighOrShareByCode(string code, out string errMsg)
        {
            errMsg = "";
            HK_Commodity hkcom = GetHKCommodityByCommodityCode(code);
            if (hkcom == null)
            {
                errMsg = "GT-8114:无法根据商品编码获取到相应的港股实体";
                return 0;
            }
            else
            {
                if (!hkcom.PerHandThighOrShare.HasValue)
                {
                    errMsg = "GT-8114:获取商品编码相应的港股实体无相关的转换值";
                    return 0;
                }
                return hkcom.PerHandThighOrShare.Value;
            }
        }

        /// <summary>
        /// 根据港股代码返回品种标识
        /// </summary>
        /// <param name="code">港股代码</param>
        /// <returns>商品</returns>
        public int? GetBreedClassIdByHKCommodityCode(string code)
        {
            HK_Commodity commdity = GetHKCommodityByCommodityCode(code);

            int? result = null;
            if (commdity != null)
                result = commdity.BreedClassID;

            return result;
        }
        #endregion

        #region HK_SpotCosts
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<HK_SpotCosts> GetAllSpotCosts()
        {
            return GetAllSpotCosts(false);
        }

        /// <summary>
        /// 获取所有的品种_港股_交易费用
        /// </summary>
        /// <param name="reLoad"></param>
        /// <returns></returns>
        public IList<HK_SpotCosts> GetAllSpotCosts(bool reLoad)
        {
            return spotCostsObj.GetAll(reLoad);
        }

        /// <summary>
        /// 从管理中心获取港股交易费用
        /// </summary>
        /// <returns></returns>
        private IList<HK_SpotCosts> GetAllSpotCostsFromWCF()
        {
            try
            {
                using (HKTradeRulesClient client = GetClient())
                    return client.GetAllHKSpotCosts();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8105";
                string errMsg = "无法从管理中心获取港股交易费用列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="breedClassID"></param>
        /// <returns></returns>
        private HK_SpotCosts GetSpotCostsByBreedClassIDFromWCF(int breedClassID)
        {
            try
            {
                using (HKTradeRulesClient client = GetClient())
                    return client.GetHKSpotCostsByBreedClassID(breedClassID);
            }
            catch (Exception ex)
            {
                string errCode = "GT-8106";
                string errMsg = "无法从管理中心获取港股交易费用。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="breedClassID"></param>
        /// <returns></returns>
        public HK_SpotCosts GetSpotCostsByBreedClassID(int breedClassID)
        {
            return spotCostsObj.GetByKey(breedClassID);
        }

        #endregion

        #region HK_SpotTradeRules
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<HK_SpotTradeRules> GetAllSpotTradeRules()
        {
            return GetAllSpotTradeRules(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reLoad"></param>
        /// <returns></returns>
        private IList<HK_SpotTradeRules> GetAllSpotTradeRules(bool reLoad)
        {
            return spotTradeRulesObj.GetAll(reLoad);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HK_SpotTradeRules GetSpotTradeRulesByBreedClassID(int id)
        {
            return spotTradeRulesObj.GetByKey(id);
        }

        /// <summary>
        /// 获取所有的港股_品种_交易规则
        /// </summary>
        /// <returns></returns>
        private IList<HK_SpotTradeRules> GetAllSpotTradeRulesFromWCF()
        {
            try
            {
                using (HKTradeRulesClient client = GetClient())
                    return client.GetAllHKSpotTradeRules();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8112";
                string errMsg = "无法从管理中心获取港股交易规则列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private HK_SpotTradeRules GetSpotTradeRulesByBreedClassIDFromWCF(int id)
        {
            try
            {
                using (HKTradeRulesClient client = GetClient())
                    return client.GetHKSpotTradeRulesByBreedClassID(id);
            }
            catch (Exception ex)
            {
                string errCode = "GT-8113";
                string errMsg = "无法从管理中心获取港股交易规则。";
                throw new VTException(errCode, errMsg, ex);
            }
        }
        /// <summary>
        /// 获取港股的交割制度
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="fund">资金交割制度</param>
        /// <param name="stock">股票交割制度</param>
        /// <param name="strMessage">错误信息</param>
        /// <returns>是否成功获取</returns>
        public bool GetHKDeliveryInstitution(string code, out int fund, out int stock, ref string strMessage)
        {
            bool result = false;
            string errCode = "GT-8114";
            string errMsg = "无法根据商品编码从管理中心获取对于的交割制度。";
            strMessage = errCode + ":" + errMsg;

            fund = -1;
            stock = -1;

            int? breedClassID = GetBreedClassIdByHKCommodityCode(code);
            if (breedClassID.HasValue)
            {
                HK_SpotTradeRules rules = GetSpotTradeRulesByBreedClassID(breedClassID.Value);
                if (rules != null)
                {
                    fund = rules.FundDeliveryInstitution.Value;
                    stock = rules.StockDeliveryInstitution.Value;
                    result = true;
                    strMessage = "";
                }
            }
            return result;
        }

        #endregion

        #region HK_MinPriceFieldRange
        /// <summary>
        /// 根据ID获取港股_最小价格变动范围值实体
        /// </summary>
        /// <returns></returns>
        public IList<HK_MinPriceFieldRange> GetAllHKMinPriceFieldRange()
        {
            return GetAllHKMinPriceFieldRange(false);
        }

        /// <summary>
        /// 获取港股_最小价格变动范围值实体
        /// </summary>
        /// <param name="reLoad"></param>
        /// <returns></returns>
        private IList<HK_MinPriceFieldRange> GetAllHKMinPriceFieldRange(bool reLoad)
        {
            return minPriceFieldRangeObj.GetAll(reLoad);
        }

        /// <summary>
        /// 根据ID获取港股_最小价格变动范围值实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HK_MinPriceFieldRange GetHKMinPriceFieldRangeByID(int id)
        {

            return minPriceFieldRangeObj.GetByKey(id);
        }

        /// <summary>
        /// 获取所有的港股_最小价格变动范围值
        /// </summary>
        /// <returns></returns>
        private IList<HK_MinPriceFieldRange> GetAllHKMinPriceFieldRangeFromWCF()
        {
            try
            {
                using (HKTradeRulesClient client = GetClient())
                    return client.GetAllHKMinPriceFieldRange();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8112";
                string errMsg = "无法从管理中心获取港股最小价格变动范围值。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        #endregion
        #endregion
    }
}
