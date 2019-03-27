using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity = ManagementCenter.Model;
using System.ServiceModel;

namespace ManagementCenter.BLL.WCFService_Out.interfases
{
    /// <summary>
    /// 描述：期货交易规则对外接口
    /// 作者：刘书伟
    /// 日期：2008-11-20
    /// </summary>
    [ServiceContract]
    public interface IFuturesTradeRules
    {
        /// <summary>
        /// 获取所有的合约交割月份
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.QH_AgreementDeliveryMonth> GetALLAgreementDeliveryMonth();

        /// <summary>
        /// 根据品种标识返回合约交割月份
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        [OperationContract]
        List<Entity.QH_AgreementDeliveryMonth> GetAgreementDeliveryMonthByBreedClassID(int breedClassID);


        /// <summary>
        /// 获取所有的期货_品种_交割月份
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.QH_CFPositionMonth> GetAllCFPositionMonth();


        /// <summary>
        /// 获取所有的委托指令类型
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.QH_ConsignInstructionType> GetAllConsignInstructionType();

        /// <summary>
        /// 获取所有的交易规则委托量
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.QH_ConsignQuantum> GetAllConsignQuantum();

        /// <summary>
        /// 根据交易规则委托量标识返回交易规则委托量
        /// </summary>
        /// <param name="consignQuantumID">交易规则委托量标识</param>
        /// <returns></returns>
        [OperationContract]
        Entity.QH_ConsignQuantum GetConsignQuantumByConsignQuantumID(int consignQuantumID);

        /// <summary>
        /// 获取所有的品种_期货_交易费用
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.QH_FutureCosts> GetAllFutureCosts();

        /// <summary>
        /// 根据品种标识返回品种_期货_交易费用
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        [OperationContract]
        Entity.QH_FutureCosts GetFutureCostsByBreedClassID(int breedClassID);

        /// <summary>
        /// 获取所有的期货_品种_交易规则
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.QH_FuturesTradeRules> GetAllFuturesTradeRules();


        /// <summary>
        /// 根据品种标识返回期货_品种_交易规则
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        [OperationContract]
        Entity.QH_FuturesTradeRules GetFuturesTradeRulesByBreedClassID(int breedClassID);

        /// <summary>
        /// 获取所有的涨跌停板幅度类型
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.QH_HighLowStopScopeType> GetAllHighLowStopScopeType();

        /// <summary>
        /// 获取所有的最后交易日
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.QH_LastTradingDay> GetAllLastTradingDay();

        /// <summary>
        /// 根据最后交易日标识返回最后交易日
        /// </summary>
        /// <param name="lastTradingDayID">最后交易日标识</param>
        /// <returns></returns>
        [OperationContract]
        Entity.QH_LastTradingDay GetLastTradingDayByLastTradingDayID(int lastTradingDayID);

        /// <summary>
        /// 获取所有的最后交易日类型
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.QH_LastTradingDayType> GetAllLastTradingDayType();

        /// <summary>
        /// 获取所有的月份
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.QH_Month> GetAllMonth();


        /// <summary>
        /// 获取所有的品种_股指期货_保证金
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.QH_SIFBail> GetAllSIFBail();

        /// <summary>
        /// 根据品种标识返回品种_股指期货_保证金
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        [OperationContract]
        Entity.QH_SIFBail GetSIFBailByBreedClassID(int breedClassID);

        /// <summary>
        /// 获取所有的股指期货持仓限制
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.QH_SIFPosition> GetAllSIFPosition();

        /// <summary>
        /// 根据品种标识返回股指期货持仓限制
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        [OperationContract]
        Entity.QH_SIFPosition GetSIFPositionByBreedClassID(int breedClassID);

        /// <summary>
        /// 获取所有的单笔委托量
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.QH_SingleRequestQuantity> GetAllSingleRequestQuantity();

        /// <summary>
        /// 根据交易规则委托量标识返回单笔委托量
        /// </summary>
        /// <param name="consignQuantumID">交易规则委托量标识</param>
        /// <returns></returns>
        [OperationContract]
        List<Entity.QH_SingleRequestQuantity> GetSingleRequestQuantityByConsignQuantumID(int consignQuantumID);

        /// <summary>
        /// 获取所有的商品期货_保证金比例
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.QH_CFBailScaleValue> GetAllCFBailScaleValue();

        /// <summary>
        /// 根据品种标识返回商品期货_保证金比例
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        [OperationContract]
        List<Entity.QH_CFBailScaleValue> GetCFBailScaleValueByBreedClassID(int breedClassID);

        /// <summary>
        /// 根据商品期货-保证金比例标识返回商品期货_保证金比例
        /// </summary>
        /// <param name="cFBailScaleValueID">商品期货-保证金比例标识</param>
        /// <returns></returns>
        [OperationContract]
        Entity.QH_CFBailScaleValue GetCFBailScaleValueByCFBailScaleValueID(int cFBailScaleValueID);

        /// <summary>
        /// 获取所有的持仓和保证金控制类型
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.QH_PositionBailType> GetAllPositionBailType();

        /// <summary>
        /// 获取所有的期货_持仓限制
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.QH_PositionLimitValue> GetAllQHPositionLimitValue();

        /// <summary>
        /// 根据品种标识返回期货_持仓限制
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        [OperationContract]
        List<Entity.QH_PositionLimitValue> GetPositionLimitValueByBreedClassID(int breedClassID);

        /// <summary>
        /// 根据期货-持仓限制标识返回期货_持仓限制
        /// </summary>
        /// <param name="positionLimitValueID">期货-持仓限制标识</param>
        /// <returns></returns>
        [OperationContract]
        Entity.QH_PositionLimitValue GetPositionLimitValueByPositionLimitValueID(int positionLimitValueID);

        /// <summary>
        /// 获取所有的商品期货_持仓取值类型
        /// </summary>
        /// <returns>持仓取值类型</returns>
        [OperationContract]
        List<Entity.QH_PositionValueType> GetAllPositionValueType();
    }
}