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
    ///描述：现货管理业务通用层
    ///作者：刘书伟
    ///日期:2008-12-11
    /// </summary>
    public class SpotManageCommon
    {
        #region 根据现货规则表中的品种ID获取品种名称

        /// <summary>
        /// 根据现货规则表中的品种ID获取品种名称
        /// </summary>
        /// <returns></returns>
        public static DataSet GetBreedClassNameByBreedClassID()
        {
            XH_SpotTradeRulesBLL xH_SpotTradeRulesBLL = new XH_SpotTradeRulesBLL();
            return xH_SpotTradeRulesBLL.GetBreedClassNameByBreedClassID();
        }

        #endregion

        //================================ 交易规则_交易方向_交易单位_交易量(最小交易单位)方法 ================================

        #region 添加交易规则_交易方向_交易单位_交易量(最小交易单位)

        /// <summary>
        /// 添加交易规则_交易方向_交易单位_交易量(最小交易单位)
        /// </summary>
        /// <param name="xHMinVolumeOfBusiness"></param>
        /// <returns></returns>
        public static int AddXHMinVolumeOfBusiness(Entity.XH_MinVolumeOfBusiness xHMinVolumeOfBusiness)
        {
            XH_MinVolumeOfBusinessBLL xH_MinVolumeOfBusinessBLL = new XH_MinVolumeOfBusinessBLL();
            return xH_MinVolumeOfBusinessBLL.Add(xHMinVolumeOfBusiness);
        }

        #endregion

        #region 获取所有交易规则_交易方向_交易单位_交易量(最小交易单位)

        /// <summary>
        /// 获取所有交易规则_交易方向_交易单位_交易量(最小交易单位)
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public static DataSet GetAllXHMinVolumeOfBusiness(string BreedClassName, int pageNo, int pageSize,
                                                          out int rowCount)
        {
            XH_MinVolumeOfBusinessBLL xH_MinVolumeOfBusinessBLL = new XH_MinVolumeOfBusinessBLL();
            return xH_MinVolumeOfBusinessBLL.GetAllXHMinVolumeOfBusiness(BreedClassName, pageNo, pageSize, out rowCount);
        }

        #endregion

        #region 根据交易规则_交易方向_交易单位_交易量(最小交易单位)ID，删除此数据

        /// <summary>
        /// 根据交易规则_交易方向_交易单位_交易量(最小交易单位)ID，删除此数据
        /// </summary>
        /// <param name="minVolumeOfBusinessID">根据交易规则_交易方向_交易单位_交易量(最小交易单位)ID</param>
        /// <returns></returns>
        public static bool DeleteXHMinVolumeOfBusByID(int minVolumeOfBusinessID)
        {
            XH_MinVolumeOfBusinessBLL xH_MinVolumeOfBusinessBLL = new XH_MinVolumeOfBusinessBLL();
            return xH_MinVolumeOfBusinessBLL.DeleteXHMinVolumeOfBusByID(minVolumeOfBusinessID);
        }

        #endregion

        #region 更新交易规则_交易方向_交易单位_交易量(最小交易单位)数据

        /// <summary>
        /// 更新交易规则_交易方向_交易单位_交易量(最小交易单位)数据
        /// </summary>
        /// <param name="model">交易规则_交易方向_交易单位_交易量(最小交易单位)实体</param>
        public static bool UpdateXHMinVolumeOfBus(ManagementCenter.Model.XH_MinVolumeOfBusiness model)
        {
            XH_MinVolumeOfBusinessBLL xH_MinVolumeOfBusinessBLL = new XH_MinVolumeOfBusinessBLL();
            return xH_MinVolumeOfBusinessBLL.UpdateXHMinVolumeOfBus(model);
        }

        #endregion

        #region 根据品种标识返回交易规则_交易方向_交易单位_交易量(最小交易单位)
        /// <summary>
        /// 根据品种标识返回交易规则_交易方向_交易单位_交易量(最小交易单位)
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        public static List<Entity.XH_MinVolumeOfBusiness> GetXHMinVolumeOfBusByBreedClassID(int breedClassID)
        {
            XH_MinVolumeOfBusinessBLL xH_MinVolumeOfBusinessBLL = new XH_MinVolumeOfBusinessBLL();
            return xH_MinVolumeOfBusinessBLL.GetListArray(string.Format("BreedClassID={0}", breedClassID));
        }
        #endregion

        #region 根据现货规则表和港股规则表中的品种标识获取品种名称
        /// <summary>
        /// 根据现货规则表和港股规则表中的品种标识获取品种名称
        /// </summary>
        /// <returns></returns>
        public static DataSet GetXHAndHKBreedClassNameByBreedClassID()
        {
            XH_MinVolumeOfBusinessBLL xH_MinVolumeOfBusinessBLL = new XH_MinVolumeOfBusinessBLL();
            return xH_MinVolumeOfBusinessBLL.GetXHAndHKBreedClassNameByBreedClassID();
        }
        #endregion

        //================================公共参数设置 方法 ================================

        #region 获取现货品种名称

        /// <summary>
        /// 获取现货品种名称
        /// </summary>
        /// <returns></returns>
        public static DataSet GetBreedClassName()
        {
            CM_BreedClassBLL cM_BreedClassBLL = new CM_BreedClassBLL();
            return cM_BreedClassBLL.GetBreedClassName();
        }

        #endregion

        //================================现货交易规则 方法 ================================

        #region 添加现货交易规则

        /// <summary>
        /// 添加现货交易规则
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool AddXHSpotTradeRules(ManagementCenter.Model.XH_SpotTradeRules model)
        {
            XH_SpotTradeRulesBLL xH_SpotTradeRulesBLL = new XH_SpotTradeRulesBLL();
            return xH_SpotTradeRulesBLL.Add(model);
        }

        #endregion

        //public static int R;

        /// <summary>
        /// 根据品种ID判断现货交易规则是否存在
        /// </summary>
        /// <param name="BreedClassID"></param>
        /// <returns></returns>
        public static bool ExistsSpotTradeRules(int BreedClassID)
        {
            XH_SpotTradeRulesBLL xH_SpotTradeRulesBLL = new XH_SpotTradeRulesBLL();
            return xH_SpotTradeRulesBLL.Exists(BreedClassID);
        }


        #region 根据品种有效申报标识删除有效申报取值和有效申报类型

        /// <summary>
        ///  根据品种有效申报标识删除有效申报取值和有效申报类型
        /// </summary>
        /// <param name="BreedClassValidID">品种有效申报标识</param>
        /// <returns></returns>
        public static bool DeleteValidDeclareValue(int BreedClassValidID)
        {
            XH_ValidDeclareTypeBLL xH_ValidDeclareTypeBLL = new XH_ValidDeclareTypeBLL();
            return xH_ValidDeclareTypeBLL.DeleteValidDeclareValue(BreedClassValidID);
        }

        #endregion

        #region 根据品种涨跌幅标识删除涨跌幅取值和涨跌幅类型(重载)

        /// <summary>
        /// 根据品种涨跌幅标识删除涨跌幅取值和涨跌幅类型
        /// </summary>
        /// <param name="BreedClassHighLowID">品种涨跌幅标识</param>
        /// <returns></returns>
        public static bool DeleteSpotHighLowValue(int BreedClassHighLowID)
        {
            XH_SpotHighLowControlTypeBLL xH_SpotHighLowControlTypeBLL = new XH_SpotHighLowControlTypeBLL();
            return xH_SpotHighLowControlTypeBLL.DeleteSpotHighLowValue(BreedClassHighLowID);
        }

        #endregion

        #region 根据品种标识,品种涨跌幅标识,品种有效申报标识,删除现货品种交易规则

        /// <summary>
        ///根据品种标识,品种涨跌幅标识,品种有效申报标识,删除现货品种交易规则 
        /// </summary>
        /// <param name="BreedClassID">品种标识</param>
        /// <param name="BreedClassHighLowID">品种涨跌幅标识</param>
        /// <param name="BreedClassValidID">品种有效申报标识</param>
        /// <returns></returns>
        public static bool DeleteSpotTradeRules(int BreedClassID, int BreedClassHighLowID, int BreedClassValidID)
        {
            XH_SpotTradeRulesBLL xH_SpotTradeRulesBLL = new XH_SpotTradeRulesBLL();
            return xH_SpotTradeRulesBLL.DeleteSpotTradeRules(BreedClassID, BreedClassHighLowID, BreedClassValidID);
        }

        #endregion

        #region 根据品种标识,品种涨跌幅标识,品种有效申报标识,删除现货品种交易规则(规则相关表全部删除)

        /// <summary>
        ///根据品种标识,品种涨跌幅标识,品种有效申报标识,删除现货品种交易规则(规则相关表全部删除) 
        /// </summary>
        /// <param name="BreedClassID">品种标识</param>
        ///// <param name="BreedClassHighLowID">品种涨跌幅标识</param>
        ///// <param name="BreedClassValidID">品种有效申报标识</param>
        /// <returns></returns>
        public static bool DeleteSpotTradeRulesAboutAll(int BreedClassID)
        {
            XH_SpotTradeRulesBLL xH_SpotTradeRulesBLL = new XH_SpotTradeRulesBLL();
            return xH_SpotTradeRulesBLL.DeleteSpotTradeRulesAboutAll(BreedClassID);
        }

        #endregion

        #region 获取所有现货交易规则

        /// <summary>
        /// 获取所有现货交易规则
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public static DataSet GetAllSpotTradeRules(string BreedClassName, int pageNo, int pageSize,
                                                   out int rowCount)
        {
            XH_SpotTradeRulesBLL xH_SpotTradeRulesBLL = new XH_SpotTradeRulesBLL();
            return xH_SpotTradeRulesBLL.GetAllSpotTradeRules(BreedClassName, pageNo, pageSize,
                                                             out rowCount);
        }

        #endregion

        #region 根据品种ID返回现货交易规则实体

        /// <summary>
        /// 根据品种ID返回现货交易规则实体
        /// </summary>
        /// <param name="BreedClassID"></param>
        /// <returns></returns>
        public static Entity.XH_SpotTradeRules GetModel(int BreedClassID)
        {
            XH_SpotTradeRulesBLL xH_SpotTradeRulesBLL = new XH_SpotTradeRulesBLL();
            return xH_SpotTradeRulesBLL.GetModel(BreedClassID);
        }

        #endregion

        #region 更新现货交易规则

        /// <summary>
        /// 更新现货交易规则
        /// </summary>
        /// <param name="model">现货交易规则实体</param>
        /// <returns></returns>
        public static bool UpdateSpotTradeRules(ManagementCenter.Model.XH_SpotTradeRules model)
        {
            XH_SpotTradeRulesBLL xH_SpotTradeRulesBLL = new XH_SpotTradeRulesBLL();
            return xH_SpotTradeRulesBLL.UpdateSpotTradeRules(model);
        }

        #endregion

        #region  根据品种涨跌幅标识得到一个对象实体

        /// <summary>
        /// 根据品种涨跌幅标识得到一个对象实体
        /// </summary>
        /// <param name="BreedClassHighLowID">品种涨跌幅标识</param>
        /// <returns></returns>
        public static Entity.XH_SpotHighLowValue GetModelByBCHighLowID(int BreedClassHighLowID)
        {
            XH_SpotHighLowValueBLL xH_SpotHighLowValueBLL = new XH_SpotHighLowValueBLL();
            return xH_SpotHighLowValueBLL.GetModelByBCHighLowID(BreedClassHighLowID);
        }

        #endregion

        #region 根据品种涨跌幅标识获取现货_品种_涨跌幅_控制类型实体

        /// <summary>
        /// 根据品种涨跌幅标识获取现货_品种_涨跌幅_控制类型实体
        /// </summary>
        /// <param name="BreedClassHighLowID"></param>
        /// <returns></returns>
        public static Entity.XH_SpotHighLowControlType GetModelSpotHighLowControlType(int BreedClassHighLowID)
        {
            XH_SpotHighLowControlTypeBLL xH_SpotHighLowControlTypeBLL = new XH_SpotHighLowControlTypeBLL();
            return xH_SpotHighLowControlTypeBLL.GetModel(BreedClassHighLowID);
        }

        #endregion

        #region 根据品种有效申报标识获取有效申报类型

        /// <summary>
        /// 根据品种有效申报标识获取有效申报类型
        /// </summary>
        /// <param name="BreedClassValidID">品种有效申报标识</param>
        /// <returns></returns>
        public static ManagementCenter.Model.XH_ValidDeclareType GetModelValidDeclareType(int BreedClassValidID)
        {
            try
            {
                XH_ValidDeclareTypeBLL xH_ValidDeclareTypeBLL = new XH_ValidDeclareTypeBLL();
                return xH_ValidDeclareTypeBLL.GetModelValidDeclareType(BreedClassValidID);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region 根据品种有效申报标识获取有效申报取值实体

        /// <summary>
        /// 根据品种有效申报标识获取有效申报取值实体
        /// </summary>
        /// <param name="BreedClassValidID">品种有效申报标识</param>
        /// <returns></returns>
        public static ManagementCenter.Model.XH_ValidDeclareValue GetModelValidDeclareValue(int BreedClassValidID)
        {
            XH_ValidDeclareTypeBLL xH_ValidDeclareTypeBLL = new XH_ValidDeclareTypeBLL();
            return xH_ValidDeclareTypeBLL.GetModelValidDeclareValue(BreedClassValidID);
        }

        #endregion

        #region 根据品种ID获取品种名称

        /// <summary>
        /// 根据品种ID获取品种名称
        /// </summary>
        /// <param name="breedClassID">品种ID</param>
        /// <returns></returns>
        public static string GetBreedClassNameByID(int breedClassID)
        {
            try
            {
                CM_BreedClassBLL cM_BreedClassBLL = new CM_BreedClassBLL();
                CM_BreedClass cM_BreedClass = cM_BreedClassBLL.GetModel(breedClassID);
                return cM_BreedClass.BreedClassName;
            }
            catch (Exception ex)
            {
                GTA.VTS.Common.CommonUtility.LogHelper.WriteError(ex.Message, ex);
                return null;
            }
        }

        #endregion

        #region 添加现货最小变动价位范围值

        /// <summary>
        /// 添加现货最小变动价位范围值
        /// </summary>
        /// <param name="xH_MinChangePriceValue">最小变动价位实体</param>
        /// <param name="cM_FieldRange">字段范围实体</param>
        /// <returns></returns>
        public static bool AddXHMinChangePriceValue(XH_MinChangePriceValue xH_MinChangePriceValue,
                                                    CM_FieldRange cM_FieldRange)
        {
            XH_MinChangePriceValueBLL xH_MinChangePriceValueBLL = new XH_MinChangePriceValueBLL();
            return xH_MinChangePriceValueBLL.AddXHMinChangePriceValue(xH_MinChangePriceValue, cM_FieldRange);
        }

        #endregion

        #region 根据品种ID获取交易规则_最小变动价位_范围_值

        /// <summary>
        /// 根据品种ID获取交易规则_最小变动价位_范围_值
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public static DataSet GetMinChangePriceFieldRangeByBreedClassID(int BreedClassID)
        {
            XH_MinChangePriceValueBLL xH_MinChangePriceValueBLL = new XH_MinChangePriceValueBLL();
            return xH_MinChangePriceValueBLL.GetMinChangePriceFieldRangeByBreedClassID(BreedClassID);
        }

        #endregion

        #region 根据品种ID，字段范围ID，删除最小变动价位_范围_值

        /// <summary>
        /// 根据品种ID，字段范围ID，删除最小变动价位_范围_值
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <param name="FieldRangeID">字段范围ID</param>
        /// <returns></returns>
        public static bool DeleteMinChangePriceValue(int BreedClassID, int FieldRangeID)
        {
            XH_MinChangePriceValueBLL xH_MinChangePriceValueBLL = new XH_MinChangePriceValueBLL();
            return xH_MinChangePriceValueBLL.DeleteMinChangePriceValue(BreedClassID, FieldRangeID);
        }

        #endregion

        #region 更新交易规则_最小变动价位_范围_值

        /// <summary>
        /// 更新交易规则_最小变动价位_范围_值
        /// </summary>
        /// <param name="xHMinChangePriceValue">交易规则_最小变动价位_范围_值实体</param>
        /// <param name="cMFieldRange">字段范围值实体</param>
        /// <returns></returns>
        public static bool UpdateMinChangePriceValue(CM_FieldRange cMFieldRange,
                                                     XH_MinChangePriceValue xHMinChangePriceValue)
        {
            XH_MinChangePriceValueBLL xH_MinChangePriceValueBLL = new XH_MinChangePriceValueBLL();
            return xH_MinChangePriceValueBLL.UpdateMinChangePriceValue(cMFieldRange, xHMinChangePriceValue);
        }

        #endregion


        #region 根据品种ID返回现货规则明细(此品种的涨跌幅和有效申报)数据

        /// <summary>
        /// 根据品种ID返回现货规则明细(此品种的涨跌幅和有效申报)数据
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public static DataSet GetSpotTradeRulesDetail(int BreedClassID)
        {
            XH_SpotTradeRulesBLL xH_SpotTradeRulesBLL = new XH_SpotTradeRulesBLL();
            return xH_SpotTradeRulesBLL.GetSpotTradeRulesDetail(BreedClassID);
        }
        #endregion

        #region 添加现货涨跌幅和有效申报
        /// <summary>
        /// 添加现货涨跌幅和有效申报
        /// </summary>
        /// <param name="xHSpotHighLowConType"></param>
        /// <param name="xHSpotHighLowValue"></param>
        /// <param name="xHValidDecType">有效申报类型实体类</param>
        /// <param name="xHValidDeclareValue">有效申报取值实体</param>
        /// <returns></returns>
        public static XH_AboutSpotHighLowEntity AddXHSpotHighLowAndValidDecl(XH_SpotHighLowControlType xHSpotHighLowConType,
                                         XH_SpotHighLowValue xHSpotHighLowValue, XH_ValidDeclareType xHValidDecType,
                                          XH_ValidDeclareValue xHValidDeclareValue)
        {
            XH_SpotHighLowControlTypeBLL xhSpotHighLowControlTypeBll = new XH_SpotHighLowControlTypeBLL();
            return xhSpotHighLowControlTypeBll.AddXHSpotHighLowAndValidDecl(xHSpotHighLowConType, xHSpotHighLowValue,
                                                                            xHValidDecType, xHValidDeclareValue);
        }
        #endregion

        #region 更新现货涨跌幅取值和涨跌幅类型及有效申报和有效申报类型
        /// <summary>
        /// 更新现货涨跌幅取值和涨跌幅类型及有效申报和有效申报类型
        /// </summary>
        /// <param name="xHSpotHighLowConType"></param>
        /// <param name="xHSpotHighLowValue"></param>
        /// <param name="xHValidDecType"></param>
        /// <param name="xHValidDeclareValue"></param>
        /// <returns></returns>
        public static bool UpdateXHSpotHighLowAndValidDecl(XH_SpotHighLowControlType xHSpotHighLowConType,
                                         XH_SpotHighLowValue xHSpotHighLowValue, XH_ValidDeclareType xHValidDecType,
                                          XH_ValidDeclareValue xHValidDeclareValue)
        {
            XH_SpotHighLowControlTypeBLL xhSpotHighLowControlTypeBll = new XH_SpotHighLowControlTypeBLL();
            return xhSpotHighLowControlTypeBll.UpdateXHSpotHighLowAndValidDecl(xHSpotHighLowConType, xHSpotHighLowValue,
                                                                               xHValidDecType, xHValidDeclareValue);
        }
        #endregion

        //================================现货交易费用 方法 ================================

        #region 获取所有现货交易费用

        /// <summary>
        /// 获取所有现货交易费用
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public static DataSet GetAllSpotCosts(int BreedClassID, string BreedClassName, int pageNo, int pageSize,
                                              out int rowCount)
        {
            XH_SpotCostsBLL xH_SpotCostsBLL = new XH_SpotCostsBLL();
            return xH_SpotCostsBLL.GetAllSpotCosts(BreedClassID, BreedClassName, pageNo, pageSize, out rowCount);
        }

        #endregion

        #region 根据现货交易费用表中的品种ID获取品种名称

        /// <summary>
        /// 根据现货交易费用表中的品种ID获取品种名称
        /// </summary>
        /// <returns></returns>
        public static DataSet GetSpotCostsBreedClassName()
        {
            XH_SpotCostsBLL xH_SpotCostsBLL = new XH_SpotCostsBLL();
            return xH_SpotCostsBLL.GetSpotCostsBreedClassName();
        }

        #endregion

        #region 根据品种ID获取现货交易费用

        /// <summary>
        /// 根据品种ID获取现货交易费用
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public static XH_SpotCosts GetXHSpotCostsModel(int BreedClassID)
        {
            XH_SpotCostsBLL xH_SpotCostsBLL = new XH_SpotCostsBLL();
            return xH_SpotCostsBLL.GetModel(BreedClassID);
        }

        #endregion

        #region 添加现货交易费用

        /// <summary>
        /// 添加现货交易费用
        /// </summary>
        /// <param name="model">现货交易费用实体</param>
        /// <returns></returns>
        public static bool AddSpotCosts(ManagementCenter.Model.XH_SpotCosts model)
        {
            XH_SpotCostsBLL xH_SpotCostsBLL = new XH_SpotCostsBLL();
            return xH_SpotCostsBLL.Add(model);
        }

        #endregion

        #region 更新现货交易费用

        /// <summary>
        /// 更新现货交易费用
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool UpdateSpotCosts(ManagementCenter.Model.XH_SpotCosts model)
        {
            XH_SpotCostsBLL xH_SpotCostsBLL = new XH_SpotCostsBLL();
            return xH_SpotCostsBLL.Update(model);
        }

        #endregion

        #region 根据品种ID，删除现货交易费用

        /// <summary>
        /// 根据品种ID，删除现货交易费用
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public static bool DeleteSpotCosts(int BreedClassID)
        {
            XH_SpotCostsBLL xH_SpotCostsBLL = new XH_SpotCostsBLL();
            return xH_SpotCostsBLL.DeleteSpotCosts(BreedClassID);
        }

        #endregion

        #region 判断品种_现货_交易费用中品种名称是否已经存在

        /// <summary>
        /// 判断品种_现货_交易费用中品种名称是否已经存在
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public static bool ExistsSpotCosts(int BreedClassID)
        {
            XH_SpotCostsBLL xH_SpotCostsBLL = new XH_SpotCostsBLL();
            return xH_SpotCostsBLL.Exists(BreedClassID);
        }

        #endregion

        //================================现货_交易商品品种_持仓限制 方法 ================================

        #region 获取所有现货_交易商品品种_持仓限制

        /// <summary>
        /// 获取所有现货_交易商品品种_持仓限制
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public static DataSet GetAllXHSpotPosition(string BreedClassName, int pageNo, int pageSize,
                                                   out int rowCount)
        {
            XH_SpotPositionBLL xHSpotPositionBLL = new XH_SpotPositionBLL();
            return xHSpotPositionBLL.GetAllXHSpotPosition(BreedClassName, pageNo, pageSize, out rowCount);
        }

        #endregion

        #region 根据现货交易商品品种_持仓限制表中的品种ID获取品种名称

        /// <summary>
        /// 根据现货交易商品品种_持仓限制表中的品种ID获取品种名称
        /// </summary>
        /// <returns></returns>
        public static DataSet GetSpotPositionBreedClassName()
        {
            XH_SpotPositionBLL xHSpotPositionBLL = new XH_SpotPositionBLL();
            return xHSpotPositionBLL.GetSpotPositionBreedClassName();
        }

        #endregion

        #region 添加现货交易商品品种_持仓限制

        /// <summary>
        ///  添加现货交易商品品种_持仓限制
        /// </summary>
        /// <param name="model">商品品种_持仓限制实体</param>
        /// <returns></returns>
        public static bool AddXHSpotPosition(ManagementCenter.Model.XH_SpotPosition model)
        {
            XH_SpotPositionBLL xHSpotPositionBLL = new XH_SpotPositionBLL();
            return xHSpotPositionBLL.Add(model);
        }

        #endregion

        #region 更新现货_交易商品品种_持仓限制

        /// <summary>
        /// 更新现货_交易商品品种_持仓限制
        /// </summary>
        /// <param name="model">现货_交易商品品种_持仓限制实体</param>
        /// <returns></returns>
        public static bool UpdateXHSpotPosition(ManagementCenter.Model.XH_SpotPosition model)
        {
            XH_SpotPositionBLL xHSpotPositionBLL = new XH_SpotPositionBLL();
            return xHSpotPositionBLL.Update(model);
        }

        #endregion

        #region  删除现货_交易商品品种_持仓限制

        /// <summary>
        /// 删除现货_交易商品品种_持仓限制
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public static bool DeleteXHSpotPosition(int BreedClassID)
        {
            XH_SpotPositionBLL xHSpotPositionBLL = new XH_SpotPositionBLL();
            return xHSpotPositionBLL.Delete(BreedClassID);
        }

        #endregion

        #region 根据品种ID，判断现货_交易商品品种_持仓限制记录是否已存在

        /// <summary>
        /// 根据品种ID，判断现货_交易商品品种_持仓限制记录是否已存在
        /// </summary>
        /// <param name="BreedClassID"></param>
        /// <returns></returns>
        public static bool ExistsXHSpotPosition(int BreedClassID)
        {
            XH_SpotPositionBLL xHSpotPositionBLL = new XH_SpotPositionBLL();
            return xHSpotPositionBLL.ExistsXHSpotPosition(BreedClassID);
        }

        #endregion

        //================================现货_品种_交易单位换算 方法 ================================

        #region 获取所有现货_品种_交易单位换算

        /// <summary>
        /// 获取所有现货_品种_交易单位换算
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public static DataSet GetAllCMUnitConversion(string BreedClassName, int pageNo, int pageSize,
                                                     out int rowCount)
        {
            CM_UnitConversionBLL cM_UnitConversionBLL = new CM_UnitConversionBLL();
            return cM_UnitConversionBLL.GetAllCMUnitConversion(BreedClassName, pageNo, pageSize, out rowCount);
        }

        #endregion

        #region 根据现货_品种_交易单位换算表中的品种ID获取品种名称

        /// <summary>
        /// 根据现货_品种_交易单位换算表中的品种ID获取品种名称
        /// </summary>
        /// <returns></returns>
        public static DataSet GetCMUnitConversionBreedClassName()
        {
            CM_UnitConversionBLL cM_UnitConversionBLL = new CM_UnitConversionBLL();
            return cM_UnitConversionBLL.GetCMUnitConversionBreedClassName();
        }

        #endregion

        #region 添加现货_品种_交易单位换算

        /// <summary>
        /// 添加现货_品种_交易单位换算
        /// </summary>
        /// <param name="model">现货_品种_交易单位换算实体</param>
        /// <returns></returns>
        public static int AddCMUnitConversion(ManagementCenter.Model.CM_UnitConversion model)
        {
            CM_UnitConversionBLL cM_UnitConversionBLL = new CM_UnitConversionBLL();
            return cM_UnitConversionBLL.Add(model);
        }

        #endregion

        #region 更新现货_品种_交易单位换算

        /// <summary>
        /// 更新现货_品种_交易单位换算
        /// </summary>
        /// <param name="model">现货_品种_交易单位换算实体</param>
        /// <returns></returns>
        public static bool UpdateCMUnitConversion(ManagementCenter.Model.CM_UnitConversion model)
        {
            CM_UnitConversionBLL cM_UnitConversionBLL = new CM_UnitConversionBLL();
            return cM_UnitConversionBLL.Update(model);
        }

        #endregion

        #region 根据现货_品种_交易单位换算ID删除现货_品种_交易单位换算

        /// <summary>
        /// 根据现货_品种_交易单位换算ID删除现货_品种_交易单位换算
        /// </summary>
        /// <param name="UnitConversionID">现货_品种_交易单位换算ID</param>
        /// <returns></returns>
        public static bool DeleteCMUnitConversion(int UnitConversionID)
        {
            CM_UnitConversionBLL cM_UnitConversionBLL = new CM_UnitConversionBLL();
            return cM_UnitConversionBLL.Delete(UnitConversionID);
        }

        #endregion

        #region 根据品种标识获取现货_品种_交易单位换算
        /// <summary>
        /// 根据品种标识获取现货_品种_交易单位换算
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        public static List<Entity.CM_UnitConversion> GetUnitConveByBreedClassID(int breedClassID)
        {
            CM_UnitConversionBLL cM_UnitConversion = new CM_UnitConversionBLL();
            return cM_UnitConversion.GetListArray(string.Format("BreedClassID={0}", breedClassID));
        }
        #endregion

    }
}