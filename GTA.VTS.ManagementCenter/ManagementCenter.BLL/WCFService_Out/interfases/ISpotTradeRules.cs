using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagementCenter.Model;
using Entity = ManagementCenter.Model;
using System.ServiceModel;

namespace ManagementCenter.BLL.WCFService_Out.interfases
{
    /// <summary>
    /// 描述：现货交易规则对外接口
    /// 作者：刘书伟
    /// 日期：2008-11-20
    /// </summary>
    [ServiceContract]
    public interface ISpotTradeRules
    {
       
        /// <summary>
        /// 获取所有的交易规则_交易方向_交易单位_交易量(最小交易单位)
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.XH_MinVolumeOfBusiness> GetAllMinVolumeOfBusiness();

        /// <summary>
        /// 根据品种标识返回交易规则_交易方向_交易单位_交易量(最小交易单位)
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        [OperationContract]
        List<Entity.XH_MinVolumeOfBusiness> GetMinVolumeOfBusinessByBreedClassID(int breedClassID);

        /// <summary>
        /// 获取所有的品种_现货_交易费用
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.XH_SpotCosts> GetAllSpotCosts();

        /// <summary>
        /// 根据品种标识返回品种_现货_交易费用
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        [OperationContract]
        Entity.XH_SpotCosts GetSpotCostsByBreedClassID(int breedClassID);

        /// <summary>
        /// 获取所有的现货_品种_涨跌幅_控制类型
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.XH_SpotHighLowControlType> GetAllSpotHighLowControlType();

        /// <summary>
        /// 获取所有的现货_交易商品品种_持仓限制
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.XH_SpotPosition> GetAllSpotPosition();

        /// <summary>
        /// 根据品种标识返回现货_交易商品品种_持仓限制
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        [OperationContract]
        Entity.XH_SpotPosition GetSpotPositionByBreedClassID(int breedClassID);

        /// <summary>
        /// 获取所有的品种_现货_交易费用_成交额_交易手续费
        /// </summary>
        /// <returns></returns>
        //[OperationContract]
        //List<Entity.XH_SpotRangeCost> GetAllSpotRangeCost();

        /// <summary>
        /// 根据品种标识返回品种_现货_交易费用_成交额_交易手续费
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        //[OperationContract]
        //List<Entity.XH_SpotRangeCost> GetSpotRangeCostByBreedClassID(int breedClassID);

        /// <summary>
        /// 获取所有的现货_品种_交易规则
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.XH_SpotTradeRules> GetAllSpotTradeRules();

        /// <summary>
        /// 根据品种标识返回现货_品种_交易规则
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        [OperationContract]
        Entity.XH_SpotTradeRules GetSpotTradeRulesByBreedClassID(int breedClassID);

        /// <summary>
        /// 获取所有的现货_品种_涨跌幅
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.XH_SpotHighLowValue> GetAllSpotHighLowValue();

        /// <summary>
        /// 根据涨跌幅标识返回现货_品种_涨跌幅
        /// </summary>
        /// <param name="hightLowValueID">涨跌幅取值标识</param>
        /// <returns></returns>
        [OperationContract]
        Entity.XH_SpotHighLowValue GetSpotHighLowValueByHightLowID(int hightLowValueID);

        /// <summary>
        /// 根据涨跌幅类型标识返回现货_品种_涨跌幅
        /// </summary>
        /// <param name="breedClassHighLowID">品种涨跌幅标识</param>
        /// <returns></returns>
        [OperationContract]
        List<Entity.XH_SpotHighLowValue> GetSpotHighLowValueByBreedClassHighLowID(int breedClassHighLowID);

        ///// <summary>
        ///// 获取所有的权证涨跌幅价格
        ///// </summary>
        ///// <returns></returns>
        //[OperationContract]
        //List<ManagementCenter.Model.XH_RightHightLowPrices> GetAllRightHightLowPrices();

        /// <summary>
        /// 根据涨跌幅标识获取权证涨跌幅价格
        /// </summary>
        /// <param name="hightLowID">涨跌幅标识</param>
        /// <returns></returns>
        [OperationContract]
        List<ManagementCenter.Model.XH_RightHightLowPrices> GetRightHightLowPricesByHightLowID(int hightLowID);

        /// <summary>
        /// 获取所有的有效申报类型
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.XH_ValidDeclareType> GetAllValidDeclareType();

        /// <summary>
        /// 根据品种有效申报标识获取有效申报类型
        /// </summary>
        /// <param name="breedClassValidID">品种有效申报标识</param>
        /// <returns></returns>
        [OperationContract]
        Entity.XH_ValidDeclareType GetValidDeclareTypeByBreedClassValidID(int breedClassValidID);

        /// <summary>
        /// 获取所有的有效申报取值
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.XH_ValidDeclareValue> GetAllValidDeclareValue();

        /// <summary>
        /// 根据有效申报取值标识获取有效申报取值
        /// </summary>
        /// <param name="validDeclareValueID">有效申报取值标识</param>
        /// <returns></returns>
        [OperationContract]
        Entity.XH_ValidDeclareValue GetValidDeclareValueByValidDeclareValueID(int validDeclareValueID);

        /// <summary>
        /// 根据品种有效申报标识获取有效申报取值
        /// </summary>
        /// <param name="breedClassValidID">品种有效申报标识</param>
        /// <returns></returns>
        [OperationContract]
        List<Entity.XH_ValidDeclareValue> GetValidDeclareValueByBreedClassValidID(int breedClassValidID);
    }
}