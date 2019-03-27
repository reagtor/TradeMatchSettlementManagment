using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ManagementCenter.BLL;
using ManagementCenter.BLL.CommonTable;
using ManagementCenter.Model;
using Entity = ManagementCenter.Model;

namespace ManagementCenterConsole.UI.CommonClass
{
    /// <summary>
    ///描述：期货管理业务通用层
    ///作者：刘书伟
    ///日期:2008-12-30
    ///修改：叶振东
    ///日期：2010-01-20
    ///描述：添加通过交割月份标识和品种标识查询此交割月份是否存在
    /// </summary>
    public class FuturesManageCommon
    {
        //================================ (商品)期货_持仓限制 方法 ================================

        #region 根据期货-持仓限制标识返回实体

        /// <summary>
        /// 根据期货-持仓限制标识返回实体
        /// </summary>
        /// <param name="PositionLimitValueID">期货-持仓限制标识</param>
        /// <returns></returns>
        public static ManagementCenter.Model.QH_PositionLimitValue GetQHPositionLimitValueModel(int PositionLimitValueID)
        {
            QH_PositionLimitValueBLL qH_PositionLimitValueBLL = new QH_PositionLimitValueBLL();
            return qH_PositionLimitValueBLL.GetQHPositionLimitValueModel(PositionLimitValueID);
        }

        #endregion

        #region 获取所有(商品)期货_持仓限制

        /// <summary>
        ///获取所有(商品)期货_持仓限制 
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="DeliveryMonthTypeID">交割月份类型标识</param>
        /// <param name="PositionBailTypeID">持仓和保证金控制类型标识</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public static DataSet GetAllQHPositionLimitValue(string BreedClassName, int DeliveryMonthTypeID,
                                                         int PositionBailTypeID,
                                                         int pageNo, int pageSize,
                                                         out int rowCount)
        {
            QH_PositionLimitValueBLL qH_PositionLimitValueBLL = new QH_PositionLimitValueBLL();
            return qH_PositionLimitValueBLL.GetAllQHPositionLimitValue(BreedClassName, DeliveryMonthTypeID,
                                                                       PositionBailTypeID, pageNo, pageSize,
                                                                       out rowCount);
        }

        #endregion

        #region 添加(商品)期货_持仓限制

        /// <summary>
        /// 添加(商品)期货_持仓限制
        /// </summary>
        /// <param name="model">(商品)期货_持仓限制实体</param>
        /// <returns></returns>
        public static int AddQHPositionLimitValue(ManagementCenter.Model.QH_PositionLimitValue model)
        {
            QH_PositionLimitValueBLL qH_PositionLimitValueBLL = new QH_PositionLimitValueBLL();
            return qH_PositionLimitValueBLL.AddQHPositionLimitValue(model);
        }

        #endregion

        #region 修改(商品)期货_持仓限制

        /// <summary>
        /// 修改(商品)期货_持仓限制
        /// </summary>
        /// <param name="model">(商品)期货_持仓限制实体</param>
        /// <returns></returns>
        public static bool UpdateQHPositionLimitValue(ManagementCenter.Model.QH_PositionLimitValue model)
        {
            QH_PositionLimitValueBLL qH_PositionLimitValueBLL = new QH_PositionLimitValueBLL();
            return qH_PositionLimitValueBLL.UpdateQHPositionLimitValue(model);
        }

        #endregion

        #region  删除(商品)期货_持仓限制

        /// <summary>
        /// 删除(商品)期货_持仓限制
        /// </summary>
        /// <param name="PositionLimitValueID">(商品)期货_持仓限制ID</param>
        /// <returns></returns>
        public static bool DeleteQHPositionLimitValue(int PositionLimitValueID)
        {
            QH_PositionLimitValueBLL qH_PositionLimitValueBLL = new QH_PositionLimitValueBLL();
            return qH_PositionLimitValueBLL.DeleteQHPositionLimitValue(PositionLimitValueID);
        }

        #endregion

        //================================  商品期货_保证金比例 方法 ================================

        #region 添加商品期货_保证金比例

        /// <summary>
        /// 添加商品期货_保证金比例
        /// </summary>
        /// <param name="model">商品期货_保证金比例实体</param>
        /// <returns></returns>
        public static int AddQHCFBailScaleValue(ManagementCenter.Model.QH_CFBailScaleValue model)
        {
            QH_CFBailScaleValueBLL qH_CFBailScaleValueBLL = new QH_CFBailScaleValueBLL();
            return qH_CFBailScaleValueBLL.AddQHCFBailScaleValue(model);
        }

        /// <summary>
        /// 添加商品期货_保证金比例
        /// </summary>
        /// <param name="model">商品期货_保证金比例实体</param>
        /// <param name="model2">商品期货_保证金比例实体</param>
        /// <returns></returns>
        public static int AddQHCFBailScaleValue(QH_CFBailScaleValue model, QH_CFBailScaleValue model2)
        {
            QH_CFBailScaleValueBLL qH_CFBailScaleValueBLL = new QH_CFBailScaleValueBLL();
            return qH_CFBailScaleValueBLL.AddQHCFBailScaleValue(model, model2);
        }

        #endregion

        #region 更新商品期货_保证金比例

        /// <summary>
        /// 更新商品期货_保证金比例
        /// </summary>
        /// <param name="model">商品期货_保证金比例实体</param>
        /// <returns></returns>
        public static bool UpdateQHCFBailScaleValue(ManagementCenter.Model.QH_CFBailScaleValue model)
        {
            QH_CFBailScaleValueBLL qH_CFBailScaleValueBLL = new QH_CFBailScaleValueBLL();
            return qH_CFBailScaleValueBLL.UpdateQHCFBailScaleValue(model);
        }

        /// <summary>
        /// 更新商品期货_保证金比例
        /// </summary>
        /// <param name="model">商品期货_保证金比例实体</param>
        /// <param name="model2">商品期货_保证金比例实体</param>
        /// <returns></returns>
        public static bool UpdateQHCFBailScaleValue(QH_CFBailScaleValue model, QH_CFBailScaleValue model2)
        {
            QH_CFBailScaleValueBLL qH_CFBailScaleValueBLL = new QH_CFBailScaleValueBLL();
            return qH_CFBailScaleValueBLL.UpdateQHCFBailScaleValue(model, model2);
        }

        #endregion

        #region 删除商品期货_保证金比例

        /// <summary>
        /// 删除商品期货_保证金比例
        /// </summary>
        /// <param name="CFBailScaleValueID">商品期货_保证金比例ID</param>
        /// <returns></returns>
        public static bool DeleteQHCFBailScaleValue(int CFBailScaleValueID)
        {
            QH_CFBailScaleValueBLL qH_CFBailScaleValueBLL = new QH_CFBailScaleValueBLL();
            return qH_CFBailScaleValueBLL.DeleteQHCFBailScaleValue(CFBailScaleValueID);
        }

        #endregion

        #region 根据商品期货_保证金比例ID获取商品期货_保证金比例对象实体

        /// <summary>
        /// 根据商品期货_保证金比例ID获取商品期货_保证金比例对象实体
        /// </summary>
        /// <param name="CFBailScaleValueID">商品期货_保证金比例ID</param>
        /// <returns></returns>
        public static ManagementCenter.Model.QH_CFBailScaleValue GetQHCFBailScaleValueModel(int CFBailScaleValueID)
        {
            QH_CFBailScaleValueBLL qH_CFBailScaleValueBLL = new QH_CFBailScaleValueBLL();
            return qH_CFBailScaleValueBLL.GetQHCFBailScaleValueModel(CFBailScaleValueID);
        }

        #endregion

        #region 获取所有商品期货_保证金比例

        /// <summary>
        ///获取所有商品期货_保证金比例
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="DeliveryMonthTypeID">交割月份类型标识</param>
        /// <param name="PositionBailTypeID">持仓和保证金控制类型标识</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public static DataSet GetAllQHCFBailScaleValue(string BreedClassName, int DeliveryMonthTypeID,
                                                       int PositionBailTypeID,
                                                       int pageNo, int pageSize,
                                                       out int rowCount)
        {
            QH_CFBailScaleValueBLL qH_CFBailScaleValueBLL = new QH_CFBailScaleValueBLL();
            return qH_CFBailScaleValueBLL.GetAllQHCFBailScaleValue(BreedClassName, DeliveryMonthTypeID,
                                                                   PositionBailTypeID, pageNo, pageSize, out rowCount);
        }

        #endregion

        /// <summary>
        /// 添加商品期货_最低保证金比例
        /// </summary>
        /// <param name="model">商品期货_保证金比例实体</param>
        /// <returns></returns>
        public static bool AddQHCFMinScaleValue(QH_SIFBail model)
        {
            QH_CFBailScaleValueBLL qH_CFBailScaleValueBLL = new QH_CFBailScaleValueBLL();
            return qH_CFBailScaleValueBLL.AddQHCFMinScaleValue(model);
        }      

        //================================ 品种_期货_交易费用 方法 ================================

        #region 添加品种_期货_交易费用

        /// <summary>
        /// 添加品种_期货_交易费用
        /// </summary>
        /// <param name="model">品种_期货_交易费用实体</param>
        /// <returns></returns>
        public static bool AddQHFutureCosts(ManagementCenter.Model.QH_FutureCosts model)
        {
            QH_FutureCostsBLL qH_FutureCostsBLL = new QH_FutureCostsBLL();
            return qH_FutureCostsBLL.AddQHFutureCosts(model);
        }

        #endregion

        #region 修改品种_期货_交易费用

        /// <summary>
        /// 修改品种_期货_交易费用
        /// </summary>
        /// <param name="model">品种_期货_交易费用实体</param>
        /// <returns></returns>
        public static bool UpdateQHFutureCosts(ManagementCenter.Model.QH_FutureCosts model)
        {
            QH_FutureCostsBLL qH_FutureCostsBLL = new QH_FutureCostsBLL();
            return qH_FutureCostsBLL.UpdateQHFutureCosts(model);
        }

        #endregion

        #region 删除品种_期货_交易费用

        /// <summary>
        /// 删除品种_期货_交易费用
        /// </summary>
        /// <param name="BreedClassID">品种_期货_交易费用ID</param>
        /// <returns></returns>
        public static bool DeleteQHFutureCosts(int BreedClassID)
        {
            QH_FutureCostsBLL qH_FutureCostsBLL = new QH_FutureCostsBLL();
            return qH_FutureCostsBLL.DeleteQHFutureCosts(BreedClassID);
        }

        #endregion

        #region 获取所有品种_期货_交易费用

        /// <summary>
        ///获取所有品种_期货_交易费用
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public static DataSet GetAllQHFutureCosts(string BreedClassName,
                                                  int pageNo, int pageSize,
                                                  out int rowCount)
        {
            QH_FutureCostsBLL qH_FutureCostsBLL = new QH_FutureCostsBLL();
            return qH_FutureCostsBLL.GetAllQHFutureCosts(BreedClassName, pageNo, pageSize, out rowCount);
        }

        #endregion

        #region 根据品种ID判断此品种的期货交易费用是否已经存在

        /// <summary>
        /// 根据品种ID判断此品种的期货交易费用是否已经存在
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public static bool ExistsFutureCosts(int BreedClassID)
        {
            QH_FutureCostsBLL qH_FutureCostsBLL = new QH_FutureCostsBLL();
            return qH_FutureCostsBLL.Exists(BreedClassID);
        }

        #endregion

        //================================ (股指期货持仓限制和品种_股指期货_保证金) 方法 ================================

        #region 添加股指期货持仓限制和品种_股指期货_保证金数据

        /// <summary>
        /// 添加股指期货持仓限制和品种_股指期货_保证金数据
        /// </summary>
        /// <param name="qHSIFPosition">股指期货持仓限制实体</param>
        /// <param name="qHSIFBail">品种_股指期货_保证金实体</param>
        /// <returns></returns>
        public static bool AddQHSIFPositionAndQHSIFBail(ManagementCenter.Model.QH_SIFPosition qHSIFPosition,
                                                        ManagementCenter.Model.QH_SIFBail qHSIFBail)
        {
            QH_SIFPositionBLL qH_SIFPositionBLL = new QH_SIFPositionBLL();
            return qH_SIFPositionBLL.AddQHSIFPositionAndQHSIFBail(qHSIFPosition, qHSIFBail);
        }

        #endregion

        #region  修改股指期货持仓限制和品种_股指期货_保证金数据

        /// <summary>
        /// 修改股指期货持仓限制和品种_股指期货_保证金数据
        /// </summary>
        /// <param name="qHSIFPosition">股指期货持仓限制实体</param>
        /// <param name="qHSIFBail">品种_股指期货_保证金实体</param>
        /// <returns></returns>
        public static bool UpdateQHSIFPositionAndQHSIFBail(ManagementCenter.Model.QH_SIFPosition qHSIFPosition,
                                                           ManagementCenter.Model.QH_SIFBail qHSIFBail)
        {
            QH_SIFPositionBLL qH_SIFPositionBLL = new QH_SIFPositionBLL();
            return qH_SIFPositionBLL.UpdateQHSIFPositionAndQHSIFBail(qHSIFPosition, qHSIFBail);
        }

        #endregion

        #region 删除股指期货持仓限制和品种_股指期货_保证金数据

        /// <summary>
        /// 删除股指期货持仓限制和品种_股指期货_保证金数据
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public static bool DeleteQHSIFPositionAndQHSIFBail(int BreedClassID)
        {
            QH_SIFPositionBLL qH_SIFPositionBLL = new QH_SIFPositionBLL();
            return qH_SIFPositionBLL.DeleteQHSIFPositionAndQHSIFBail(BreedClassID);
        }

        #endregion

        #region  获取所有股指期货持仓限制和品种_股指期货_保证金数据

        /// <summary>
        ///  获取所有股指期货持仓限制和品种_股指期货_保证金数据
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public static DataSet GetAllQHSIFPositionAndQHSIFBail(string BreedClassName, int pageNo, int pageSize,
                                                              out int rowCount)
        {
            QH_SIFPositionBLL qH_SIFPositionBLL = new QH_SIFPositionBLL();
            return qH_SIFPositionBLL.GetAllQHSIFPositionAndQHSIFBail(BreedClassName, pageNo, pageSize, out rowCount);
        }

        #endregion

        #region 根据品种ID判断品种_股指期货_保证金是否已经存在

        /// <summary>
        /// 根据品种ID判断品种_股指期货_保证金是否已经存在
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public static bool ExistsSIFBail(int BreedClassID)
        {
            QH_SIFBailBLL qH_SIFBailBLL = new QH_SIFBailBLL();
            return qH_SIFBailBLL.Exists(BreedClassID);
        }

        #endregion

        #region  根据品种ID判断 股指期货持仓限制是否已经存在

        /// <summary>
        /// 根据品种ID判断 股指期货持仓限制是否已经存在
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public static bool ExistsSIFPosition(int BreedClassID)
        {
            QH_SIFPositionBLL qH_SIFPositionBLL = new QH_SIFPositionBLL();
            return qH_SIFPositionBLL.Exists(BreedClassID);
        }

        #endregion

        //================================ (期货_品种_交易规则) 方法 ================================

        #region  添加期货交易规则

        /// <summary>
        /// 添加期货交易规则
        /// </summary>
        /// <param name="model">期货_品种_交易规则实体</param>
        /// <returns></returns>
        public static bool AddFuturesTradeRules(ManagementCenter.Model.QH_FuturesTradeRules model)
        {
            QH_FuturesTradeRulesBLL qH_FuturesTradeRulesBLL = new QH_FuturesTradeRulesBLL();
            return qH_FuturesTradeRulesBLL.AddFuturesTradeRules(model);
        }

        #endregion

        #region 更新期货交易规则

        /// <summary>
        /// 更新期货交易规则
        /// </summary>
        /// <param name="model">期货_品种_交易规则实体</param>
        /// <returns></returns>
        public static bool UpdateFuturesTradeRules(ManagementCenter.Model.QH_FuturesTradeRules model)
        {
            QH_FuturesTradeRulesBLL qH_FuturesTradeRulesBLL = new QH_FuturesTradeRulesBLL();
            return qH_FuturesTradeRulesBLL.UpdateFuturesTradeRules(model);
        }

        #endregion

        #region 获取所有期货_品种_交易规则

        /// <summary>
        /// 获取所有期货_品种_交易规则
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public static DataSet GetAllFuturesTradeRules(string BreedClassName, int pageNo, int pageSize,
                                                      out int rowCount)
        {
            QH_FuturesTradeRulesBLL qH_FuturesTradeRulesBLL = new QH_FuturesTradeRulesBLL();
            return qH_FuturesTradeRulesBLL.GetAllFuturesTradeRules(BreedClassName, pageNo, pageSize, out rowCount);
        }

        #endregion

        #region 根据品种ID，获取期货交易规则对象实体

        /// <summary>
        /// 根据品种ID，获取期货交易规则对象实体
        /// </summary>
        /// <param name="BreedClassID"></param>
        /// <returns></returns>
        public static Entity.QH_FuturesTradeRules GetFuturesTradeRulesModel(int BreedClassID)
        {
            QH_FuturesTradeRulesBLL qH_FuturesTradeRulesBLL = new QH_FuturesTradeRulesBLL();
            return qH_FuturesTradeRulesBLL.GetFuturesTradeRulesModel(BreedClassID);
        }

        #endregion

        #region 根据品种ID，判断期货交易规则是否已存在

        /// <summary>
        /// 根据品种ID，判断期货交易规则是否已存在
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public static bool ExistsFuturesTradeRules(int BreedClassID)
        {
            QH_FuturesTradeRulesBLL qH_FuturesTradeRulesBLL = new QH_FuturesTradeRulesBLL();
            return qH_FuturesTradeRulesBLL.ExistsFuturesTradeRules(BreedClassID);
        }

        #endregion

        #region 根据品种标识,删除期货品种交易规则(规则相关表全部删除)

        /// <summary>
        ///根据品种标识,删除期货品种交易规则(规则相关表全部删除) 
        /// </summary>
        /// <param name="BreedClassID">品种标识</param>
        /// <returns></returns>
        public static bool DeleteFuturesTradeRulesAboutAll(int BreedClassID)
        {
            QH_FuturesTradeRulesBLL qH_FuturesTradeRulesBLL = new QH_FuturesTradeRulesBLL();
            return qH_FuturesTradeRulesBLL.DeleteFuturesTradeRulesAboutAll(BreedClassID);
        }

        #endregion

        #region 添加期货品种时初始化一个周期的和约代码
        /// <summary>
        /// 添加期货品种时初始化一个周期的和约代码
        /// </summary>
        /// <param name="BreedClassID"></param>
        public static void QHCommdityCodeInit(int BreedClassID)
         {
            QHCodeInit qHCodeInit=new QHCodeInit();
            qHCodeInit.QHCommdityCodeInit(BreedClassID);
         }
        #endregion

        //================================ (期货_品种_交易规则--最后交易日) 方法 ================================

        #region 添加最后交易日

        /// <summary>
        /// 添加最后交易日
        /// </summary>
        /// <param name="model">最后交易日实体</param>
        /// <returns></returns>
        public static int AddQHLastTradingDay(ManagementCenter.Model.QH_LastTradingDay model)
        {
            QH_LastTradingDayBLL qH_LastTradingDayBLL = new QH_LastTradingDayBLL();
            return qH_LastTradingDayBLL.AddQHLastTradingDay(model);
        }

        #endregion

        #region 更新最后交易日

        /// <summary>
        /// 更新最后交易日
        /// </summary>
        /// <param name="model">最后交易日实体</param>
        /// <returns></returns>
        public static bool UpdateQHLastTradingDay(ManagementCenter.Model.QH_LastTradingDay model)
        {
            QH_LastTradingDayBLL qH_LastTradingDayBLL = new QH_LastTradingDayBLL();
            return qH_LastTradingDayBLL.UpdateQHLastTradingDay(model);
        }

        #endregion

        #region 删除最后交易日

        /// <summary>
        /// 删除最后交易日
        /// </summary>
        /// <param name="LastTradingDayID">最后交易日ID</param>
        /// <returns></returns>
        public static bool DeleteQHLastTradingDay(int LastTradingDayID)
        {
            QH_LastTradingDayBLL qH_LastTradingDayBLL = new QH_LastTradingDayBLL();
            return qH_LastTradingDayBLL.DeleteQHLastTradingDay(LastTradingDayID);
        }

        #endregion

        #region 根据最后交易日ID，获取最后交易日实体

        /// <summary>
        /// 根据最后交易日ID，获取最后交易日实体
        /// </summary>
        /// <param name="LastTradingDayID">最后交易日ID</param>
        /// <returns></returns>
        public static Entity.QH_LastTradingDay GetQHLastTradingDayModel(int LastTradingDayID)
        {
            QH_LastTradingDayBLL qH_LastTradingDayBLL = new QH_LastTradingDayBLL();
            return qH_LastTradingDayBLL.GetQHLastTradingDayModel(LastTradingDayID);
        }

        #endregion

        //================================ (期货_品种_交易规则--交易规则委托量和单笔委托量) 方法 ================================

        #region 添加交易规则和单笔最大委托量

        /// <summary>
        /// 添加交易规则和单笔最大委托量
        /// </summary>
        /// <param name="qHConsignQuantum">交易规则实体</param>
        /// <param name="qHSingleRequestQuantityl">单笔最大委托量实体</param>
        /// <param name="qHSingleRequestQuantity2">单笔最大委托量实体</param>
        /// <returns></returns>
        public static int AddQHConsignQuantumAndSingle(QH_ConsignQuantum qHConsignQuantum,
                                                       QH_SingleRequestQuantity qHSingleRequestQuantityl,
                                                       QH_SingleRequestQuantity qHSingleRequestQuantity2)
        {
            QH_ConsignQuantumBLL qH_ConsignQuantumBLL = new QH_ConsignQuantumBLL();
            return qH_ConsignQuantumBLL.AddQHConsignQuantumAndSingle(qHConsignQuantum, qHSingleRequestQuantityl,
                                                                     qHSingleRequestQuantity2);
        }

        #endregion

        #region  更新交易规则和单笔最大委托量

        /// <summary>
        /// 更新交易规则和单笔最大委托量
        /// </summary>
        /// <param name="qHConsignQuantum">交易规则实体</param>
        /// <param name="qHSingleRequestQuantity1">单笔最大委托量实体</param>
        /// <param name="qHSingleRequestQuantity2">单笔最大委托量实体</param>
        /// <returns></returns>
        public static bool UpdateQHConsignQuantumAndSingle(QH_ConsignQuantum qHConsignQuantum,
                                                           QH_SingleRequestQuantity qHSingleRequestQuantity1,
                                                           QH_SingleRequestQuantity qHSingleRequestQuantity2)
        {
            QH_ConsignQuantumBLL qH_ConsignQuantumBLL = new QH_ConsignQuantumBLL();
            return qH_ConsignQuantumBLL.UpdateQHConsignQuantumAndSingle(qHConsignQuantum, qHSingleRequestQuantity1,
                                                                        qHSingleRequestQuantity2);
        }

        #endregion

        #region 根据交易规则委托量标识删除交易规则和单笔最大委托量

        /// <summary>
        /// 根据交易规则委托量标识删除交易规则和单笔最大委托量
        /// </summary>
        /// <param name="ConsignQuantumID">交易规则委托量ID</param>
        /// <returns></returns>
        public static bool DeleteQHConsignQuantumAndSingle(int ConsignQuantumID)
        {
            QH_ConsignQuantumBLL qH_ConsignQuantumBLL = new QH_ConsignQuantumBLL();
            return qH_ConsignQuantumBLL.DeleteQHConsignQuantumAndSingle(ConsignQuantumID);
        }

        #endregion

        #region 根据交易规则委托量ID，获取委托量实体

        /// <summary>
        /// 根据交易规则委托量ID，获取委托量实体
        /// </summary>
        /// <param name="ConsignQuantumID">交易规则委托量ID</param>
        /// <returns></returns>
        public static Entity.QH_ConsignQuantum GetQHConsignQuantumModel(int ConsignQuantumID)
        {
            QH_ConsignQuantumBLL qH_ConsignQuantumBLL = new QH_ConsignQuantumBLL();
            return qH_ConsignQuantumBLL.GetQHConsignQuantumModel(ConsignQuantumID);
        }

        #endregion

        #region 根据单笔委托量标识获取单笔委托量实体

        /// <summary>
        /// 根据单笔委托量标识获取单笔委托量实体
        /// </summary>
        /// <param name="SingleRequestQuantityID">单笔委托量标识</param>
        /// <returns></returns>
        public static Entity.QH_SingleRequestQuantity GetQHSingleRequestQuantityModel(int SingleRequestQuantityID)
        {
            QH_SingleRequestQuantityBLL qH_SingleRequestQuantityBLL = new QH_SingleRequestQuantityBLL();
            return qH_SingleRequestQuantityBLL.GetQHSingleRequestQuantityModel(SingleRequestQuantityID);
        }

        #endregion

        #region 根据交易规则委托量ID获取单笔委托量

        /// <summary>
        /// 根据交易规则委托量ID获取单笔委托量
        /// </summary>
        /// <param name="ConsignQuantumID">交易规则委托量ID</param>
        /// <returns></returns>
        public static Entity.QH_SingleRequestQuantity GetQHSingleRequestQuantityModelByConsignQuantumID(
            int ConsignQuantumID)
        {
            QH_SingleRequestQuantityBLL qH_SingleRequestQuantityBLL = new QH_SingleRequestQuantityBLL();
            return qH_SingleRequestQuantityBLL.GetQHSingleRequestQuantityModelByConsignQuantumID(ConsignQuantumID);
        }

        #endregion

        /// <summary>
        /// 根据交易规则委托量标识返回单笔委托量
        /// </summary>
        /// <param name="ConsignQuantumID">交易规则委托量标识</param>
        /// <returns></returns>
        public static List<Entity.QH_SingleRequestQuantity> GetQHSingleRQuantityListByConsignQuantumID(int ConsignQuantumID)
        {
            QH_SingleRequestQuantityBLL qH_SingleRequestQuantityBLL = new QH_SingleRequestQuantityBLL();
            return qH_SingleRequestQuantityBLL.GetListArray(string.Format("ConsignQuantumID={0}", ConsignQuantumID));
        }


        //================================ (期货_品种_交易规则--合约交割月份) 方法 ================================

        #region 更新交割月份(包括添加,删除)

        /// <summary>
        /// 更新交割月份(包括添加,删除)
        /// </summary>
        /// <param name="addMonthID">需要添加的月份ID</param>
        /// <param name="deleteMonthID">需要删除的月份ID</param>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public static bool UpdateQHAgreementDeliveryMonth(List<int> addMonthID, List<int> deleteMonthID,
                                                          int BreedClassID)
        {
            QH_AgreementDeliveryMonthBLL qH_AgreementDeliveryMonthBLL = new QH_AgreementDeliveryMonthBLL();
            return qH_AgreementDeliveryMonthBLL.UpdateQHAgreementDeliveryMonth(addMonthID, deleteMonthID, BreedClassID);
        }

        #endregion

        #region 根据品种标识返回合约交割月份

        /// <summary>
        /// 根据品种标识返回合约交割月份
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        public static List<Entity.QH_AgreementDeliveryMonth> GetQHAgreementDeliveryMonth(int breedClassID)
        {
            QH_AgreementDeliveryMonthBLL qH_AgreementDeliveryMonthBLL = new QH_AgreementDeliveryMonthBLL();
            return qH_AgreementDeliveryMonthBLL.GetQHAgreementDeliveryMonth(breedClassID);
        }

        #endregion

        #region 根据期货_品种_交易规则表中的品种标识获取品种名称

        /// <summary>
        /// 根据期货_品种_交易规则表中的品种标识获取品种名称
        /// </summary>
        /// <returns></returns>
        public static DataSet GetQHBreedClassNameByBreedClassID()
        {
            QH_FuturesTradeRulesBLL qH_FuturesTradeRulesBLL = new QH_FuturesTradeRulesBLL();
            return qH_FuturesTradeRulesBLL.GetQHBreedClassNameByBreedClassID();
        }

        #endregion

        #region 根据品种标识和月份标识查询合约交割月份

        /// <summary>
        /// 根据品种标识和月份标识返回合约交割月份
        /// </summary>
        /// <param name="BreedClassID">品种标识</param>
        /// <param name="monthid">月份标识</param>
        /// <returns>是否存在此交割月份</returns>
        public static QH_AgreementDeliveryMonth GetQHAgreementDeliveryBreedClassID(int BreedClassID, int monthid)
        {
            QH_AgreementDeliveryMonthBLL qH_AgreementDeliveryMonthBLL = new QH_AgreementDeliveryMonthBLL();
            return qH_AgreementDeliveryMonthBLL.GetQHAgreementDeliveryBreedClassID(BreedClassID, monthid);
        }
        #endregion 根据品种标识和月份标识查询合约交割月份


      
    }
}