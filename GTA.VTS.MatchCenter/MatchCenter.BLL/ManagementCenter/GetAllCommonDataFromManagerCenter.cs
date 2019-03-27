using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatchCenter.DAL.DevolveVerifyCommonService;
using GTA.VTS.Common.CommonUtility;
using MatchCenter.DAL.FuturesDevolveService;
using MatchCenter.DAL.SpotTradingDevolveService;
using MatchCenter.BLL.Common;
//增加港股引用
using MatchCenter.DAL.HKTradingRulesService;

namespace MatchCenter.BLL.ManagementCenter
{
    /// <summary>
    /// 从管理中心获取相关公共数据公共类
    /// 使用此类的方法获取管理中心的相关数据只能在程序启动或者程序初始化时使用，即缓存了相关所有数据在程序运行交易过程中是脱离管理中心的
    /// 所以在交易过程中获取相关数据只能从CommonDataManagerOperate和或者 CommonDataCacheProxy中获取
    /// Create BY：李健华
    /// Create Date：2009-08-18
    /// </summary>
    public class GetAllCommonDataFromManagerCenter
    {
        #region 1.获取所有商品
        /// <summary>
        /// 获取所有商品
        /// </summary>
        /// <returns></returns>
        protected List<CM_Commodity> GetAllCommodity()
        {
            try
            {
                using (CommonParaClient client = ManagementCenterDataAgent.Instanse.GetComonParaInstanse())
                {
                    return client.GetAllCommodity();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E006, ex);
                return null;
            }
        }
        #endregion

        #region 2.所有的交易所类型
        /// <summary>
        /// 所有的交易所类型
        /// </summary>
        /// <returns></returns>
        protected List<CM_BourseType> GetAllBourseType()
        {
            try
            {
                using (CommonParaClient client = ManagementCenterDataAgent.Instanse.GetComonParaInstanse())
                {
                    return client.GetAllBourseType();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E006, ex);
                return null;
            }
        }
        #endregion

        #region 3.获得所有商品的品种类型
        /// <summary>
        /// 获得所有商品的品种类型
        /// </summary>
        /// <returns></returns>
        protected List<CM_BreedClass> GetAllBreedClass()
        {
            try
            {
                using (CommonParaClient client = ManagementCenterDataAgent.Instanse.GetComonParaInstanse())
                {
                    return client.GetAllBreedClass();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E006, ex);
                return null;
            }
        }
        #endregion

        #region 4.获得所有商品的品种类别
        /// <summary>
        /// 获得所有商品的品种类别
        /// </summary>
        /// <returns></returns>
        protected List<CM_BreedClassType> GetAllBreedClassType()
        {
            try
            {
                using (CommonParaClient client = ManagementCenterDataAgent.Instanse.GetComonParaInstanse())
                {
                    return client.GetAllBreedClassType();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E006, ex);
                return null;
            }
        }
        #endregion

        #region 5.获取所有可交易商品_熔断
        /// <summary>
        /// 获取所有商品交易熔断规则
        /// </summary>
        /// <returns></returns>
        protected List<CM_CommodityFuse> GetAllCommodityFuse()
        {
            try
            {
                using (CommonParaClient client = ManagementCenterDataAgent.Instanse.GetComonParaInstanse())
                {
                    return client.GetAllCommodityFuse();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E006, ex);
                return null;
            }
        }
        #endregion

        #region 6.获取所有交易上下限值的字段_范围
        ///// <summary>
        ///// 获取所有交易上下限值的字段_范围
        ///// </summary>
        ///// <returns></returns>
        //protected  List<CM_FieldRange> GetAllFieldRange()
        //{
        //    try
        //    {
        //        using (CommonParaClient client = ManagementCenterDataAgent.Instanse.GetComonParaInstanse())
        //        {
        //            return client.GetAllFieldRange();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteError(GenerateInfo.CH_E006, ex);
        //        return null;
        //    }
        //}
        #endregion

        #region  7.获取所有熔断_时间段标识设置
        /// <summary>
        /// 获取所有熔断_时间段标识设置
        /// </summary>
        /// <returns></returns>
        protected List<CM_FuseTimesection> GetAllFuseTimesection()
        {
            try
            {
                using (CommonParaClient client = ManagementCenterDataAgent.Instanse.GetComonParaInstanse())
                {
                    return client.GetAllFuseTimesection();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E006, ex);
                return null;
            }
        }
        #endregion

        #region 8.获取所有现货的交易规则_最小变动价位_范围_值
        ///// <summary>
        ///// 获取所有现货的交易规则_最小变动价位_范围_值
        ///// </summary>
        ///// <returns></returns>
        //protected  List<XH_MinChangePriceValue> GetAllMinChangePriceValue()
        //{
        //    try
        //    {
        //        using (SpotTradeRulesClient client = ManagementCenterDataAgent.Instanse.GetSpotTradeRulesInstanse())
        //        {
        //            return client.GetAllMinChangePriceValue();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteError(GenerateInfo.CH_E006, ex); 
        //        return null;
        //    }
        //}
        #endregion

        #region 9.期货上下限（涨跌幅）类型
        /// <summary>
        /// 期货上下限（涨跌幅）类型
        /// </summary>
        /// <returns></returns>
        protected List<QH_HighLowStopScopeType> GetAllQH_HighLowStopScopeType()
        {
            try
            {
                using (FuturesTradeRulesClient client = ManagementCenterDataAgent.Instanse.GetFutureTradeRulesInstanse())
                {
                    return client.GetAllHighLowStopScopeType();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E006, ex);
                return null;
            }
        }
        #endregion

        #region 10.期货交易规则
        /// <summary>
        /// 期货交易规则
        /// </summary>
        /// <returns></returns>
        protected List<QH_FuturesTradeRules> GetAllQH_FutureTradeRules()
        {
            try
            {
                using (FuturesTradeRulesClient client = ManagementCenterDataAgent.Instanse.GetFutureTradeRulesInstanse())
                {
                    return client.GetAllFuturesTradeRules();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E006, ex);
                return null;
            }
        }
        #endregion

        #region 11.根据撮合中心ID获取所有撮合机
        /// <summary>
        /// 根据撮合中心ID获取所有撮合机
        /// </summary>
        /// <param name="centerID">撮合中心id</param>
        /// <returns></returns>
        protected List<RC_MatchMachine> GetAllMatchMachineByMatchCenterID(int centerID)
        {
            try
            {
                using (CommonParaClient client = ManagementCenterDataAgent.Instanse.GetComonParaInstanse())
                {
                    return client.GetAllMatchMachineByMatchCenter(centerID);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E006, ex);
                return null;
            }
        }
        #endregion

        #region 12.获取所有撮合机配置代码
        /// <summary>
        /// 获取所有撮合机配置代码
        /// </summary>
        /// <returns></returns>
        protected List<RC_TradeCommodityAssign> GetAllTradeCommodityAssign()
        {
            try
            {
                using (CommonParaClient client = ManagementCenterDataAgent.Instanse.GetComonParaInstanse())
                {
                    return client.GetAllTradeCommodityAssign();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E006, ex);
                return null;
            }
        }
        #endregion

        #region 13.获取现货交易规则
        /// <summary>
        /// 获取现货交易规则
        /// </summary>
        /// <returns></returns>
        protected List<XH_SpotTradeRules> GetAllSpotTradeRules()
        {
            try
            {
                using (SpotTradeRulesClient client = ManagementCenterDataAgent.Instanse.GetSpotTradeRulesInstanse())
                {
                    return client.GetAllSpotTradeRules();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E006, ex);
                return null;
            }
        }
        #endregion

        #region 14.获取所有交易所的交易时间
        /// <summary>
        /// 获取所有交易所的交易时间
        /// </summary>
        /// <returns></returns>
        protected List<CM_TradeTime> GetAllTradeTime()
        {
            try
            {
                using (CommonParaClient client = ManagementCenterDataAgent.Instanse.GetComonParaInstanse())
                {
                    return client.GetAllTradeTime();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E006, ex);
                return null;
            }
        }
        #endregion

        #region 15获取所有有效申报类型
        /// <summary>
        /// 获取所有有效申报类型
        /// </summary>
        /// <returns></returns>
        protected List<XH_ValidDeclareType> GetAllValidDeclareType()
        {
            try
            {
                using (SpotTradeRulesClient client = ManagementCenterDataAgent.Instanse.GetSpotTradeRulesInstanse())
                {
                    return client.GetAllValidDeclareType();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E006, ex);
                return null;
            }
        }
        #endregion

        #region 16.获取所有的有效申报值
        /// <summary>
        /// 获取所有的有效申报值
        /// </summary>
        /// <returns></returns>
        protected List<XH_ValidDeclareValue> GetAllValidDeclareValue()
        {
            try
            {
                using (SpotTradeRulesClient client = ManagementCenterDataAgent.Instanse.GetSpotTradeRulesInstanse())
                {
                    return client.GetAllValidDeclareValue();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E006, ex);
                return null;
            }
        }
        #endregion

        #region 17.得到获取所有的现货_品种_涨跌幅_控制类型
        /// <summary>
        /// 得到获取所有的现货_品种_涨跌幅_控制类型
        /// </summary>
        /// <returns></returns>
        protected List<XH_SpotHighLowControlType> GetAllSpotHighLowControlType()
        {
            try
            {
                using (SpotTradeRulesClient client = ManagementCenterDataAgent.Instanse.GetSpotTradeRulesInstanse())
                {
                    return client.GetAllSpotHighLowControlType();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E006, ex);
                return null;
            }

        }
        #endregion

        #region 18.获取现货的所有涨跌幅值
        /// <summary>
        /// 获取现货的所有涨跌幅值
        /// </summary>
        /// <returns></returns>
        protected List<XH_SpotHighLowValue> GetAllSpotHighLowValue()
        {
            try
            {

                using (SpotTradeRulesClient client = ManagementCenterDataAgent.Instanse.GetSpotTradeRulesInstanse())
                {
                    return client.GetAllSpotHighLowValue();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E006, ex);
                return null;
            }
        }
        #endregion

        #region 19.获取增发上市的商品代码
        /// <summary>
        /// 获取增发上市的商品代码
        /// </summary>
        /// <returns></returns>
        protected List<ZFInfo> GetAllZFCommodity()
        {
            try
            {
                using (CommonParaClient client = ManagementCenterDataAgent.Instanse.GetComonParaInstanse())
                {
                    return client.GetZFCommodity();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E006, ex);
                return null;
            }
        }
        #endregion

        #region 20.根据IP和端口获取撮合中心实体
        /// <summary>
        /// 根据IP和端口获取撮合中心实体
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口</param>
        /// <returns></returns>
        protected RC_MatchCenter GetMatchCenterByAddress(string ip, int port)
        {
            try
            {
                using (CommonParaClient client = ManagementCenterDataAgent.Instanse.GetComonParaInstanse())
                {
                    return client.GetMatchCenterByIpAndPort(ip, port);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E006, ex);
                return null;
            }
        }
        #endregion

        #region 21.根据配置文件中IP和端口获取撮合中心
        /// <summary>
        /// 根据配置文件中IP和端口获取撮合中心
        /// </summary>
        /// <returns></returns>
        protected RC_MatchCenter GetMatchCenterByAddress()
        {
            return GetMatchCenterByAddress(AppConfig.GetConfigMatchCenterIP(), AppConfig.GetConfigMatchCenterPort());
        }
        #endregion

        #region 22.根据IP和端口获取撮合中心的主键ID
        /// <summary>
        /// 根据配置文件中IP和端口获取撮合中心主键ID，如果从管理中心获取数据有异常或者返回的为null时
        /// 这里返回的ID为0
        /// </summary>
        /// <returns></returns>
        protected int GetMatchCenterIDByAddress()
        {
            int id = 0;
            RC_MatchCenter model = GetMatchCenterByAddress(AppConfig.GetConfigMatchCenterIP(), AppConfig.GetConfigMatchCenterPort());
            if (model != null)
            {
                id = model.MatchCenterID;
            }
            return id;
        }
        #endregion

        #region 23.检查与管理中心连接是否成功
        /// <summary>
        /// 检查是否成功
        /// </summary>
        /// <returns></returns>
        protected bool IsConnManagerCenterSuccess()
        {
            try
            {
                using (CommonParaClient client = ManagementCenterDataAgent.Instanse.GetComonParaInstanse())
                {
                    List<RC_MatchCenter> list = client.GetAllMatchCenter();
                    if (Utils.IsNullOrEmpty(list))
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E006, ex);
                return false;
            }
            return true;
        }

        #endregion

        #region 24.根据交易所ID获取非交易日期
        /// <summary>
        /// 获取非交易日期
        /// </summary>
        /// <returns></returns>
        protected List<CM_NotTradeDate> GetAllCMNotTradeDate()
        {
            try
            {
                using (CommonParaClient client = ManagementCenterDataAgent.Instanse.GetComonParaInstanse())
                {
                    return client.GetAllNotTradeDate();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E006, ex);
                return null;
            }
        }
        #endregion

        #region 25.获取港股交易规则_最小变动价位_范围_值
        /// <summary>
        /// 25.获取港股交易规则_最小变动价位_范围_值
        /// </summary>
        /// <returns></returns>
        protected List<HK_MinPriceFieldRange> GetHKMinChangePriceFieldRange()
        {
            try
            {
                using (HKTradeRulesClient client = ManagementCenterDataAgent.Instanse.GetHKTradeRulesInstance())
                {
                    //List<HK_MinPriceFieldRange> ranges = client.GetAllHKMinPriceFieldRange();
                    return client.GetAllHKMinPriceFieldRange();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E006, ex);
                return null;
            }
        }


        #endregion

        #region 26.获取港股交易规则
        /// <summary>
        /// 获取港股交易规则
        /// </summary>
        /// <returns></returns>
        protected List<HK_SpotTradeRules> GetHKSpotTradeRules()
        {
            try
            {
                using (HKTradeRulesClient client = ManagementCenterDataAgent.Instanse.GetHKTradeRulesInstance())
                {
                    //List<HK_SpotTradeRules> r = client.GetAllHKSpotTradeRules();
                    return client.GetAllHKSpotTradeRules();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E006, ex);
                return null;
            }
        }
        #endregion

        #region 27.获取港股交易所有代码
        /// <summary>
        /// 获取港股交易所有代码
        /// </summary>
        /// <returns></returns>
        protected List<HK_Commodity> GetAllHKCommodity()
        {
            try
            {
                using (HKTradeRulesClient client = ManagementCenterDataAgent.Instanse.GetHKTradeRulesInstance())
                {
                    //List<HK_SpotTradeRules> r = client.GetAllHKSpotTradeRules();
                    //List<HK_Commodity> ok = client.GetAllHKCommodity();
                    return client.GetAllHKCommodity();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E006, ex);
                return null;
            }

        }

        #endregion

        #region 28.根据商品所属类型获取所有商品代码
        /// <summary>
        /// 根据商品所属类型获取所有商品代码
        /// </summary>
        /// <param name="breedClassID">商品所属类型ID</param>
        /// <returns></returns>
        protected List<CM_Commodity> GetAllCommodityByBreedClass(int breedClassID)
        {
            try
            {
                using (CommonParaClient client = ManagementCenterDataAgent.Instanse.GetComonParaInstanse())
                {
                    return client.GetCommodityByBreedClassID(breedClassID);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E006, ex);
                return null;
            }
        }
        #endregion


        #region 29.获取所有期货交易规则的最后交易日所有列表
        /// <summary>
        /// 获取所有期货交易规则的最后交易日所有列表
        /// </summary>
        /// <returns></returns>
        protected List<QH_LastTradingDay> GetAllLastTradingDay()
        {
            try
            {
                using (FuturesTradeRulesClient client = ManagementCenterDataAgent.Instanse.GetFutureTradeRulesInstanse())
                {
                    return client.GetAllLastTradingDay();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E006, ex);
                return null;
            }
        }
        #endregion


    }
}
