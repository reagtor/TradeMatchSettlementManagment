using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ManagementCenter.BLL;
using ManagementCenter.Model;
using ManagementCenter.Model.XH;
using Entity = ManagementCenter.Model;

namespace ManagementCenterConsole.UI.CommonClass
{
    /// <summary>
    ///描述：港股管理业务通用层
    ///作者：刘书伟
    ///日期:2009-10-24
    /// </summary>
    public class HKManageCommon
    {
        //================================港股交易商品代码管理 方法 ================================

        #region 获取所有港股交易商品
        /// <summary>
        /// 获取所有港股交易商品
        /// </summary>
        /// <param name="HKCommodityCode">港股商品代码</param>
        /// <param name="HKCommodityName">港股商品名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public static DataSet GetAllHKCommodity(string HKCommodityCode, string HKCommodityName, int pageNo,
                                         int pageSize,
                                         out int rowCount)
        {
            HK_CommodityBLL hK_CommodityBLL = new HK_CommodityBLL();
            return hK_CommodityBLL.GetAllHKCommodity(HKCommodityCode, HKCommodityName, pageNo, pageSize,
                                                     out rowCount);
        }

        #endregion

        #region 添加港股交易商品

        /// <summary>
        /// 添加港股交易商品
        /// </summary>
        /// <param name="model">港股交易商品实体</param>
        /// <returns></returns>
        public static bool AddHKCommodity(ManagementCenter.Model.HK_Commodity model)
        {
            HK_CommodityBLL hM_CommodityBLL = new HK_CommodityBLL();
            return hM_CommodityBLL.AddHKCommodity(model);
        }

        #endregion

        #region 更新港股交易商品
        /// <summary>
        /// 更新港股交易商品
        /// </summary>
        /// <param name="model">港股交易商品实体</param>
        /// <returns></returns>
        public static bool UpdateHKCommodity(ManagementCenter.Model.HK_Commodity model)
        {
            HK_CommodityBLL hM_CommodityBLL = new HK_CommodityBLL();
            return hM_CommodityBLL.UpdateHKCommodity(model);

        }
        #endregion

        #region 根据港股交易商品代码删除港股交易商品（相关表的记录同时删除）

        /// <summary>
        /// 根据港股交易商品代码删除港股交易商品（相关表的记录同时删除）
        /// </summary>
        /// <param name="HKCommodityCode">港股商品代码</param>
        /// <returns></returns>
        public static bool DeleteHKCommodity(string HKCommodityCode)
        {
            HK_CommodityBLL hM_CommodityBLL = new HK_CommodityBLL();
            return hM_CommodityBLL.DeleteHKCommodity(HKCommodityCode);

        }
        #endregion

        #region 判断港股交易商品代码是否已经存在

        /// <summary>
        /// 判断港股交易商品代码是否已经存在
        /// </summary>
        /// <param name="HKCommodityCode">港股商品代码</param>
        /// <returns></returns>
        public static bool IsExistHKCommodityCode(string HKCommodityCode)
        {
            HK_CommodityBLL hM_CommodityBLL = new HK_CommodityBLL();
            return hM_CommodityBLL.IsExistHKCommodityCode(HKCommodityCode);

        }
        #endregion

        #region 判断港股交易商品名称是否已经存在

        /// <summary>
        /// 判断港股交易商品名称是否已经存在
        /// </summary>
        /// <param name="HKCommodityName">港股商品名称</param>
        /// <returns></returns>
        public static bool IsExistHKCommodityName(string HKCommodityName)
        {
            HK_CommodityBLL hM_CommodityBLL = new HK_CommodityBLL();
            return hM_CommodityBLL.IsExistHKCommodityName(HKCommodityName);

        }
        #endregion

        //================================港股交易规则 方法 ================================


        #region 获取所有港股交易规则

        /// <summary>
        /// 获取所有港股交易规则
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public static DataSet GetAllHKSpotTradeRules(string BreedClassName, int pageNo, int pageSize,
                                            out int rowCount)
        {
            HK_SpotTradeRulesBLL hK_SpotTradeRulesBLL = new HK_SpotTradeRulesBLL();
            return hK_SpotTradeRulesBLL.GetAllHKSpotTradeRules(BreedClassName, pageNo, pageSize, out rowCount);
        }
        #endregion

        #region 根据港股规则表中的品种ID获取品种名称
        /// <summary>
        /// 根据港股规则表中的品种ID获取品种名称
        /// </summary>
        /// <returns></returns>
        public static DataSet GetHKBreedClassNameByBreedClassID()
        {
            HK_SpotTradeRulesBLL hK_SpotTradeRulesBLL = new HK_SpotTradeRulesBLL();
            return hK_SpotTradeRulesBLL.GetHKBreedClassNameByBreedClassID();
        }
        #endregion


        #region 添加港股交易规则
        /// <summary>
        ///添加港股交易规则 
        /// </summary>
        /// <param name="model">港股交易规则实体</param>
        /// <returns></returns>
        public static bool AddHKSpotTradeRules(ManagementCenter.Model.HK_SpotTradeRules model)
        {
            HK_SpotTradeRulesBLL hK_SpotTradeRulesBLL = new HK_SpotTradeRulesBLL();
            return hK_SpotTradeRulesBLL.AddHKSpotTradeRules(model);
        }
        #endregion

        #region 更新港股交易规则
        /// <summary>
        /// 更新港股交易规则
        /// </summary>
        /// <param name="model">港股交易规则实体</param>
        /// <returns></returns>
        public static bool UpdateHKSpotTradeRules(ManagementCenter.Model.HK_SpotTradeRules model)
        {
            HK_SpotTradeRulesBLL hK_SpotTradeRulesBLL = new HK_SpotTradeRulesBLL();
            return hK_SpotTradeRulesBLL.UpdateHKSpotTradeRules(model);
        }
        #endregion

        #region  根据品种ID删除港股交易规则(相关)
        /// <summary>
        /// 根据品种ID删除港股交易规则(相关)
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public static bool DeleteHKSpotTradeRulesAbout(int BreedClassID)
        {
            HK_SpotTradeRulesBLL hK_SpotTradeRulesBLL = new HK_SpotTradeRulesBLL();
            return hK_SpotTradeRulesBLL.DeleteHKSpotTradeRulesAbout(BreedClassID);
        }
        #endregion

        #region 根据品种ID判断港股交易规则是否存在记录
        /// <summary>
        /// 根据品种ID判断港股交易规则是否存在记录
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public static bool ExistsHKSpotTradeRules(int BreedClassID)
        {
            HK_SpotTradeRulesBLL hK_SpotTradeRulesBLL = new HK_SpotTradeRulesBLL();
            return hK_SpotTradeRulesBLL.ExistsHKSpotTradeRules(BreedClassID);
        }
        #endregion

        //================================港股交易费用 方法 ================================

        #region 获取所有港股交易费用

        /// <summary>
        /// 获取所有港股交易费用
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public static DataSet GetAllHKSpotCosts(string BreedClassName, int pageNo, int pageSize,
                                       out int rowCount)
        {
            HK_SpotCostsBLL hK_SpotCostsBLL = new HK_SpotCostsBLL();
            return hK_SpotCostsBLL.GetAllHKSpotCosts(BreedClassName, pageNo, pageSize, out  rowCount);
        }
        #endregion

        #region 根据港股交易费用表中的品种ID获取品种名称

        /// <summary>
        /// 根据港股交易费用表中的品种ID获取品种名称
        /// </summary>
        /// <returns></returns>
        public static DataSet GetHKSpotCostsBreedClassName()
        {
            HK_SpotCostsBLL hK_SpotCostsBLL = new HK_SpotCostsBLL();
            return hK_SpotCostsBLL.GetHKSpotCostsBreedClassName();
        }
        #endregion

        #region 根据品种ID得到港股交易费用对象实体
        /// <summary>
        /// 根据品种ID得到港股交易费用对象实体
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public static ManagementCenter.Model.HK_SpotCosts GetHKSpotCostsModel(int BreedClassID)
        {
            HK_SpotCostsBLL hK_SpotCostsBLL = new HK_SpotCostsBLL();
            return hK_SpotCostsBLL.GetModel(BreedClassID);
        }
        #endregion

        #region  添加港股交易费用
        /// <summary>
        /// 添加港股交易费用
        /// </summary>
        /// <param name="model">港股交易费用实体</param>
        /// <returns></returns>
        public static bool AddHKSpotCosts(ManagementCenter.Model.HK_SpotCosts model)
        {
            HK_SpotCostsBLL hK_SpotCostsBLL = new HK_SpotCostsBLL();
            return hK_SpotCostsBLL.AddHKSpotCosts(model);
        }
        #endregion

        #region 更新港股交易费用
        /// <summary>
        /// 更新港股交易费用
        /// </summary>
        /// <param name="model">港股交易费用实体</param>
        /// <returns></returns>
        public static bool UpdateHKSpotCosts(ManagementCenter.Model.HK_SpotCosts model)
        {
            HK_SpotCostsBLL hK_SpotCostsBLL = new HK_SpotCostsBLL();
            return hK_SpotCostsBLL.UpdateHKSpotCosts(model);
        }
        #endregion


        #region 根据品种ID，删除港股交易费用

        /// <summary>
        ///根据品种ID，删除港股交易费用
        /// </summary>
        public static bool DeleteHKSpotCosts(int BreedClassID)
        {
            HK_SpotCostsBLL hK_SpotCostsBLL = new HK_SpotCostsBLL();
            return hK_SpotCostsBLL.DeleteHKSpotCosts(BreedClassID);
        }
        #endregion

        #region 是否存在港股交易费用记录
        /// <summary>
        /// 是否存在港股交易费用记录
        /// </summary>
        public static bool ExistsHKSpotCosts(int BreedClassID)
        {
            HK_SpotCostsBLL hK_SpotCostsBLL = new HK_SpotCostsBLL();
            return hK_SpotCostsBLL.ExistsHKSpotCosts(BreedClassID);
        }
        #endregion

    }
}
