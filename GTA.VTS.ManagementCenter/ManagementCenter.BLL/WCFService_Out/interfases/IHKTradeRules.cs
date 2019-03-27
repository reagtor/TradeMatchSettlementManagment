using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity = ManagementCenter.Model;
using System.ServiceModel;

namespace ManagementCenter.BLL.WCFService_Out.interfases
{
    /// <summary>
    /// 描述：港股交易规则对外接口
    /// 作者：刘书伟
    /// 日期：2009-10-22
    /// </summary>
    [ServiceContract]
    public interface IHKTradeRules
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
        /// 获取所有的港股交易商品
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.HK_Commodity> GetAllHKCommodity();

        /// <summary>
        /// 根据港股商品代码返回港股交易商品
        /// </summary>
        /// <param name="hkcommodityCode">商品代码</param>
        /// <returns></returns>
        [OperationContract]
        Entity.HK_Commodity GetHKCommodityByHKCommodityCode(string hkcommodityCode);

        /// <summary>
        /// 根据品种标识返回港股交易商品
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        [OperationContract]
        List<Entity.HK_Commodity> GetHKCommodityByBreedClassID(int breedClassID);

        /// <summary>
        /// 获取新股上市的港股商品代码
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.HK_Commodity> GetNewHKCommodity();

        /// <summary>
        /// 获取所有的港股_交易费用
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.HK_SpotCosts> GetAllHKSpotCosts();

        /// <summary>
        /// 根据品种标识返回港股_交易费用
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        [OperationContract]
        Entity.HK_SpotCosts GetHKSpotCostsByBreedClassID(int breedClassID);

        /// <summary>
        /// 获取所有的港股_品种_交易规则
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.HK_SpotTradeRules> GetAllHKSpotTradeRules();

        /// <summary>
        /// 根据品种标识返回港股_品种_交易规则
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        [OperationContract]
        Entity.HK_SpotTradeRules GetHKSpotTradeRulesByBreedClassID(int breedClassID);

        /// <summary>
        /// 获取所有的港股交易规则_最小变动价位范围值
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.HK_MinPriceFieldRange> GetAllHKMinPriceFieldRange();

        #region 前台接口
        /// <summary>
        /// 提供前台获取港股商品代码列表
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.OnstageHK_Commodity> GetHKCommodityListArray();

        /// <summary>
        /// 提供前台获取港股行业列表
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ManagementCenter.Model.HKProfessionInfo> GetListHKProfessionInfoArray();

        #endregion

    }
}
