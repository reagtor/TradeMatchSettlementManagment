using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ManagementCenter.BLL;
using ManagementCenter.Model;
using Entity = ManagementCenter.Model;


namespace ManagementCenterConsole.UI.CommonClass
{
    /// <summary>
    /// 描述：公共参数设置业务通用层
    /// 作者：刘书伟
    /// 日期:2008-12-26
    /// 修改：叶振东
    /// 时间：2010-04-07
    /// 描述：添加根据交易所类型和非交易日时间来查是否存在记录方法
    /// </summary>
    public class CommonParameterSetCommon
    {
        //================================ 交易所类型 ,交易所类型_交易时间,交易所类型_非交易日期方法 ================================

        #region 获取交易所类型名称

        /// <summary>
        /// 获取交易所类型名称
        /// </summary>
        /// <returns></returns>
        public static DataSet GetBourseTypeName()
        {
            CM_BourseTypeBLL cM_BourseTypeBLL = new CM_BourseTypeBLL();
            return cM_BourseTypeBLL.GetBourseTypeName();
        }

        #endregion

        #region 获取所有交易所类型

        /// <summary>
        /// 获取所有交易所类型
        /// </summary>
        /// <param name="BourseTypeName">交易所类型名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public static DataSet GetAllCMBourseType(string BourseTypeName, int pageNo, int pageSize,
                                                 out int rowCount)
        {
            CM_BourseTypeBLL cM_BourseTypeBLL = new CM_BourseTypeBLL();
            return cM_BourseTypeBLL.GetAllCMBourseType(BourseTypeName, pageNo, pageSize, out rowCount);
        }

        #endregion

        #region 添加交易所类型

        /// <summary>
        /// 添加交易所类型
        /// </summary>
        /// <param name="model">交易所类型实体</param>
        /// <returns></returns>
        public static int AddCMBourseType(ManagementCenter.Model.CM_BourseType model)
        {
            CM_BourseTypeBLL cM_BourseTypeBLL = new CM_BourseTypeBLL();
            return cM_BourseTypeBLL.AddCMBourseType(model);
        }

        #endregion

        #region 添加交易所类型和交易时间
        /// <summary>
        /// 添加交易所类型和交易时间
        /// </summary>
        /// <param name="cmBourseType">交易所类型实体</param>
        /// <param name="cmTradeTimeList">交易时间实体集合</param>
        /// <returns></returns>
        public static int AddCMBourseTypeAndTradeTime(CM_BourseType cmBourseType, List<CM_TradeTime> cmTradeTimeList)
        {
            CM_BourseTypeBLL cM_BourseTypeBLL = new CM_BourseTypeBLL();
            return cM_BourseTypeBLL.AddCMBourseTypeAndTradeTime(cmBourseType, cmTradeTimeList);
        }
        #endregion

        #region  更新交易所类型

        /// <summary>
        /// 更新交易所类型
        /// </summary>
        /// <param name="model">交易所类型实体</param>
        /// <returns></returns>
        public static bool UpdateCMBourseType(ManagementCenter.Model.CM_BourseType model)
        {
            CM_BourseTypeBLL cM_BourseTypeBLL = new CM_BourseTypeBLL();
            return cM_BourseTypeBLL.UpdateCMBourseType(model);
        }

        #endregion

        #region 删除交易所类型及相关联的表

        /// <summary>
        ///删除交易所类型及相关联的表
        /// </summary>
        /// <param name="BourseTypeID">交易所类型ID</param>
        public static bool DeleteCMBourseTypeAbout(int BourseTypeID)
        {
            CM_BourseTypeBLL cM_BourseTypeBLL = new CM_BourseTypeBLL();
            return cM_BourseTypeBLL.DeleteCMBourseTypeAbout(BourseTypeID);
        }

        #endregion

        #region 判断交易所名称是否已经存在

        /// <summary>
        /// 判断交易所名称是否已经存在
        /// </summary>
        /// <param name="BourseTypeName">交易所名称</param>
        /// <returns></returns>
        public static bool IsExistBourseTypeName(string BourseTypeName)
        {
            CM_BourseTypeBLL cM_BourseTypeBLL = new CM_BourseTypeBLL();
            return cM_BourseTypeBLL.IsExistBourseTypeName(BourseTypeName);
        }

        #endregion

        #region 根据交易所类型ID获取交易所类型实体

        /// <summary>
        /// 根据交易所类型ID获取交易所类型实体
        /// </summary>
        /// <param name="BourseTypeID"></param>
        /// <returns></returns>
        public static Entity.CM_BourseType GetCMBourseTypeModel(int BourseTypeID)
        {
            CM_BourseTypeBLL cM_BourseTypeBLL = new CM_BourseTypeBLL();
            return cM_BourseTypeBLL.GetModel(BourseTypeID);
        }

        #endregion

        #region 获取交易所类型的最大ID
        /// <summary>
        /// 获取交易所类型的最大ID
        /// </summary>
        public static int GetCMBourseTypeMaxId()
        {
            CM_BourseTypeBLL cM_BourseTypeBLL = new CM_BourseTypeBLL();
            return cM_BourseTypeBLL.GetCMBourseTypeMaxId();
        }
        #endregion

        #region 获取所有交易所类型_交易时间

        /// <summary>
        /// 获取所有交易所类型_交易时间
        /// </summary>
        /// <param name="BourseTypeName">交易所类型名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public static DataSet GetAllCMTradeTime(string BourseTypeName, int pageNo, int pageSize,
                                                out int rowCount)
        {
            CM_TradeTimeBLL cM_TradeTimeBLL = new CM_TradeTimeBLL();
            return cM_TradeTimeBLL.GetAllCMTradeTime(BourseTypeName, pageNo, pageSize, out rowCount);
        }

        #endregion

        #region  根据交易所类型_交易时间表中的交易所类型ID获取交易所类型名称

        /// <summary>
        /// 根据交易所类型_交易时间表中的交易所类型ID获取交易所类型名称
        /// </summary>
        /// <returns></returns>
        public static DataSet GetBourseTypeNameByBourseTypeID()
        {
            CM_TradeTimeBLL cM_TradeTimeBLL = new CM_TradeTimeBLL();
            return cM_TradeTimeBLL.GetBourseTypeNameByBourseTypeID();
        }

        #endregion

        #region 添加交易时间

        /// <summary>
        /// 添加交易时间
        /// </summary>
        /// <param name="model">交易所类型_交易时间实体</param>
        /// <returns></returns>
        public static int AddCMTradeTime(ManagementCenter.Model.CM_TradeTime model)
        {
            CM_TradeTimeBLL cM_TradeTimeBLL = new CM_TradeTimeBLL();
            return cM_TradeTimeBLL.AddCMTradeTime(model);
        }

        #endregion

        #region 更新交易时间

        /// <summary>
        /// 更新交易时间
        /// </summary>
        /// <param name="model">交易所类型_交易时间实体</param>
        /// <returns></returns>
        public static bool UpdateCMTradeTime(ManagementCenter.Model.CM_TradeTime model)
        {
            CM_TradeTimeBLL cM_TradeTimeBLL = new CM_TradeTimeBLL();
            return cM_TradeTimeBLL.UpdateCMTradeTime(model);
        }

        #endregion

        #region 根据交易时间ID，删除交易时间

        /// <summary>
        /// 根据交易时间ID，删除交易时间
        /// </summary>
        /// <param name="TradeTimeID">交易时间ID</param>
        /// <returns></returns>
        public static bool DeleteCMTradeTime(int TradeTimeID)
        {
            CM_TradeTimeBLL cM_TradeTimeBLL = new CM_TradeTimeBLL();
            return cM_TradeTimeBLL.DeleteCMTradeTime(TradeTimeID);
        }

        #endregion

        #region 根据交易所类型返回交易所时间

        /// <summary>
        /// 根据交易所类型返回交易所时间
        /// </summary>
        /// <param name="BourseTypeID">交易所类型</param>
        /// <returns></returns>
        public static DataSet GetTradeTimeByBourseTypeID(int BourseTypeID)
        {
            CM_TradeTimeBLL cM_TradeTimeBLL = new CM_TradeTimeBLL();
            return cM_TradeTimeBLL.GetTradeTimeByBourseTypeID(BourseTypeID);
        }

        #endregion

        #region 根据查询条件获取所有的交易所类型_交易时间（查询条件可为空）

        /// <summary>
        /// 根据查询条件获取所有的交易所类型_交易时间（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public static List<ManagementCenter.Model.CM_TradeTime> GetCMTradeTimeListArray(string strWhere)
        {
            CM_TradeTimeBLL cM_TradeTimeBLL = new CM_TradeTimeBLL();
            return cM_TradeTimeBLL.GetListArray(strWhere);
        }
        #endregion

        #region 根据交易时间ID判断是否已存在交易时间记录
        /// <summary>
        /// 根据交易时间ID判断是否已存在交易时间记录
        /// </summary>
        /// <param name="TradeTimeID">交易时间ID</param>
        /// <returns></returns>
        public static bool ExistsCMTradeTime(int TradeTimeID)
        {
            CM_TradeTimeBLL cM_TradeTimeBLL = new CM_TradeTimeBLL();
            return cM_TradeTimeBLL.ExistsCMTradeTime(TradeTimeID);
        }
        #endregion

        #region 根据交易所类型ID返回交易时间
        /// <summary>
        /// 根据交易所类型ID返回交易时间
        /// </summary>
        /// <param name="bourseTypeID">交易所类型ID</param>
        /// <returns></returns>
        public static DataSet GetTradeTimeAndBourseTypeList(int bourseTypeID)
        {
            CM_TradeTimeBLL cM_TradeTimeBLL = new CM_TradeTimeBLL();
            return cM_TradeTimeBLL.GetTradeTimeAndBourseTypeList(bourseTypeID);
        }
        #endregion

        #region 获取所有交易所类型_非交易日期

        /// <summary>
        /// 获取所有交易所类型_非交易日期
        /// </summary>
        /// <param name="BourseTypeName">交易所类型名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public static DataSet GetAllCMNotTradeDate(string BourseTypeName, int pageNo, int pageSize,
                                                   out int rowCount)
        {
            CM_NotTradeDateBLL cM_NotTradeDateBLL = new CM_NotTradeDateBLL();
            return cM_NotTradeDateBLL.GetAllCMNotTradeDate(BourseTypeName, pageNo, pageSize, out rowCount);
        }

        #endregion

        #region 添加非交易日

        /// <summary>
        /// 添加非交易日
        /// </summary>
        /// <param name="model">非交易日实体</param>
        /// <returns></returns>
        public static int AddCMNotTradeDate(ManagementCenter.Model.CM_NotTradeDate model)
        {
            CM_NotTradeDateBLL cM_NotTradeDateBLL = new CM_NotTradeDateBLL();
            return cM_NotTradeDateBLL.AddCMNotTradeDate(model);
        }

        #endregion

        /// <summary>
        ///  根据交易所类型和非交易日时间来查是否存在记录
        /// </summary>
        /// <param name="BourseTypeID">交易所类型</param>
        /// <param name="NotTradeDay">非交易日时间</param>
        /// <returns></returns>
        public static ManagementCenter.Model.CM_NotTradeDate GetNotTradeDate(int BourseTypeID, DateTime NotTradeDay)
        {
            CM_NotTradeDateBLL cM_NotTradeDateBLL = new CM_NotTradeDateBLL();
            return cM_NotTradeDateBLL.GetNotTradeDate(BourseTypeID,NotTradeDay);
        }

        #region 更新非交易日

        /// <summary>
        /// 更新非交易日
        /// </summary>
        /// <param name="model">非交易日实体</param>
        /// <returns></returns>
        public static bool UpdateCMNotTradeDate(ManagementCenter.Model.CM_NotTradeDate model)
        {
            CM_NotTradeDateBLL cM_NotTradeDateBLL = new CM_NotTradeDateBLL();
            return cM_NotTradeDateBLL.UpdateCMNotTradeDate(model);
        }

        #endregion

        #region 删除非交易日

        /// <summary>
        /// 删除非交易日
        /// </summary>
        /// <param name="NotTradeDateID">非交易日ID</param>
        /// <returns></returns>
        public static bool DeleteCMNotTradeDate(int NotTradeDateID)
        {
            CM_NotTradeDateBLL cM_NotTradeDateBLL = new CM_NotTradeDateBLL();
            return cM_NotTradeDateBLL.DeleteCMNotTradeDate(NotTradeDateID);
        }

        #endregion

        #region 根据交易所类型_非交易日期表中的交易所类型ID获取交易所类型名称

        /// <summary>
        ///根据交易所类型_非交易日期表中的交易所类型ID获取交易所类型名称
        /// </summary>
        /// <returns></returns>
        public static DataSet GetCMNotTradeDateBourseTypeName()
        {
            CM_NotTradeDateBLL cM_NotTradeDateBLL = new CM_NotTradeDateBLL();
            return cM_NotTradeDateBLL.GetCMNotTradeDateBourseTypeName();
        }

        #endregion

        //================================ 交易商品品种方法 ================================

        #region 添加交易商品品种

        /// <summary>
        /// 添加交易商品品种
        /// </summary>
        /// <param name="model">交易商品品种实体</param>
        /// <returns></returns>
        //public static int AddCMBreedClass(CM_BreedClass model)
        public static bool AddCMBreedClass(CM_BreedClass model)
        {
            CM_BreedClassBLL cM_BreedClassBLL = new CM_BreedClassBLL();
            return cM_BreedClassBLL.AddCMBreedClass(model);
        }

        #endregion

        #region 更新交易商品品种

        /// <summary>
        /// 更新交易商品品种
        /// </summary>
        /// <param name="model">交易商品品种实体</param>
        /// <returns></returns>
        public static bool UpdateCMBreedClass(CM_BreedClass model)
        {
            CM_BreedClassBLL cM_BreedClassBLL = new CM_BreedClassBLL();
            return cM_BreedClassBLL.UpdateCMBreedClass(model);
        }

        #endregion

        #region 删除交易商品品种

        /// <summary>
        /// 删除交易商品品种
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public static bool DeleteCMBreedClass(int BreedClassID)
        {
            CM_BreedClassBLL cM_BreedClassBLL = new CM_BreedClassBLL();
            return cM_BreedClassBLL.DeleteCMBreedClass(BreedClassID);
        }

        #endregion

        #region 删除品种时，则根据品种ID，删除所有相关联的表
        /// <summary>
        /// 删除品种时，则根据品种ID，删除所有相关联的表
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public static bool DeleteCMBreedClassALLAbout(int BreedClassID)
        {
            CM_BreedClassBLL cM_BreedClassBLL = new CM_BreedClassBLL();
            return cM_BreedClassBLL.DeleteCMBreedClassALLAbout(BreedClassID);

        }
        #endregion

        #region 获取所有交易商品品种

        /// <summary>
        /// 获取所有交易商品品种
        /// </summary>
        /// <param name="BreedClassTypeID">品种类型ID</param>
        ///  <param name="BourseTypeID">交易所类型ID</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public static DataSet GetAllCMBreedClass(int BreedClassTypeID, int BourseTypeID, int pageNo, int pageSize,
                                                 out int rowCount)
        {
            CM_BreedClassBLL cM_BreedClassBLL = new CM_BreedClassBLL();
            return cM_BreedClassBLL.GetAllCMBreedClass(BreedClassTypeID, BourseTypeID, pageNo, pageSize, out rowCount);
        }

        #endregion

        #region  根据交易商品品种表中的交易所类型ID获取交易所类型名称

        /// <summary>
        /// 根据交易商品品种表中的交易所类型ID获取交易所类型名称
        /// </summary>
        /// <returns></returns>
        public static DataSet GetCMBreedClassBourseTypeName()
        {
            CM_BreedClassBLL cM_BreedClassBLL = new CM_BreedClassBLL();
            return cM_BreedClassBLL.GetCMBreedClassBourseTypeName();
        }

        #endregion

        #region 获取所有品种名称

        /// <summary>
        /// 获取所有品种名称
        /// </summary>
        /// <returns></returns>
        public static DataSet GetAllBreedClassName()
        {
            CM_BreedClassBLL cM_BreedClassBLL = new CM_BreedClassBLL();
            return cM_BreedClassBLL.GetAllBreedClassName();
        }

        #endregion

        #region 获取品种类型是商品期货的品种名称

        /// <summary>
        /// 获取品种类型是商品期货的品种名称
        /// </summary>
        /// <returns></returns>
        public static DataSet GetSpQhTypeBreedClassName()
        {
            CM_BreedClassBLL cM_BreedClassBLL = new CM_BreedClassBLL();
            return cM_BreedClassBLL.GetSpQhTypeBreedClassName();
        }

        #endregion

        #region 获取品种类型是商品期货或股指期货的品种名称

        /// <summary>
        /// 获取品种类型是商品期货或股指期货的品种名称
        /// </summary>
        /// <returns></returns>
        public static DataSet GetQHFutureCostsBreedClassName()
        {
            CM_BreedClassBLL cM_BreedClassBLL = new CM_BreedClassBLL();
            return cM_BreedClassBLL.GetQHFutureCostsBreedClassName();
        }

        #endregion

        #region 获取品种类型是股指期货的品种名称

        /// <summary>
        /// 获取品种类型是股指期货的品种名称
        /// </summary>
        /// <returns></returns>
        public static DataSet GetQHSIFPositionAndBailBreedClassName()
        {
            CM_BreedClassBLL cM_BreedClassBLL = new CM_BreedClassBLL();
            return cM_BreedClassBLL.GetQHSIFPositionAndBailBreedClassName();
        }

        #endregion

        #region  判断品种名称是否已经存在

        /// <summary>
        /// 判断品种名称是否已经存在
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <returns></returns>
        public static bool IsExistBreedClassName(string BreedClassName)
        {
            CM_BreedClassBLL cM_BreedClassBLL = new CM_BreedClassBLL();
            return cM_BreedClassBLL.IsExistBreedClassName(BreedClassName);
        }

        #endregion

        #region 获取现货普通和港股品种名称

        /// <summary>
        /// 获取现货普通和港股品种名称
        /// </summary>
        /// <returns></returns>
        public static DataSet GetXHAndHKBreedClassName()
        {
            CM_BreedClassBLL cM_BreedClassBLL = new CM_BreedClassBLL();
            return cM_BreedClassBLL.GetXHAndHKBreedClassName();

        }
        #endregion

        #region 根据查询条件获取品种数据列表
        /// <summary>
        /// 根据查询条件获取品种数据列表
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        public static DataSet GetList(string strWhere)
        {
            CM_BreedClassBLL cM_BreedClassBLL = new CM_BreedClassBLL();
            return cM_BreedClassBLL.GetList(strWhere);
        }
        #endregion

        #region 获取交易商品品种表的最大ID
        /// <summary>
        ///获取交易商品品种表的最大ID 
        /// </summary>
        /// <returns></returns>
        public static int GetCMBreedClassMaxId()
        {
            CM_BreedClassBLL cM_BreedClassBLL = new CM_BreedClassBLL();
            return cM_BreedClassBLL.GetCMBreedClassMaxId();
        }
        #endregion

        #region 根据品种标识返回交易商品品种实体
        /// <summary>
        /// 根据品种标识返回交易商品品种实体
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        public static CM_BreedClass GetBreedClassByBClassID(int breedClassID)
        {
            CM_BreedClassBLL cM_BreedClassBLL = new CM_BreedClassBLL();
            return cM_BreedClassBLL.GetBreedClassByBClassID(breedClassID);
        }
        #endregion

        //================================ 交易商品 方法 ================================

        #region 获取所有交易商品

        /// <summary>
        /// 获取所有交易商品
        /// </summary>
        /// <param name="CommodityCode">商品代码</param>
        /// <param name="CommodityName">商品名称</param>
        ///  <param name="BreedClassID">品种ID</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public static DataSet GetAllCMCommodity(string CommodityCode, string CommodityName, int BreedClassID, int pageNo,
                                                int pageSize,
                                                out int rowCount)
        {
            CM_CommodityBLL cM_CommodityBLL = new CM_CommodityBLL();
            return cM_CommodityBLL.GetAllCMCommodity(CommodityCode, CommodityName, BreedClassID, pageNo, pageSize,
                                                     out rowCount);
        }

        #endregion

        #region 添加交易商品

        /// <summary>
        /// 添加交易商品
        /// </summary>
        /// <param name="model">交易商品实体</param>
        /// <returns></returns>
        public static bool AddCMCommodity(ManagementCenter.Model.CM_Commodity model)
        {
            CM_CommodityBLL cM_CommodityBLL = new CM_CommodityBLL();
            return cM_CommodityBLL.AddCMCommodity(model);
        }

        #endregion

        #region 更新交易商品

        /// <summary>
        /// 更新交易商品
        /// </summary>
        /// <param name="model">交易商品实体</param>
        /// <returns></returns>
        public static bool UpdateCMCommodity(ManagementCenter.Model.CM_Commodity model)
        {
            CM_CommodityBLL cM_CommodityBLL = new CM_CommodityBLL();
            return cM_CommodityBLL.UpdateCMCommodity(model);
        }

        #endregion

        #region 根据商品代码删除交易商品

        /// <summary>
        /// 根据商品代码删除交易商品
        /// </summary>
        /// <param name="CommodityCode">商品代码</param>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public static bool DeleteCMCommodity(string CommodityCode, int BreedClassID)
        {
            CM_CommodityBLL cM_CommodityBLL = new CM_CommodityBLL();
            return cM_CommodityBLL.DeleteCMCommodity(CommodityCode, BreedClassID);
        }

        #endregion

        #region 判断交易商品代码是否已经存在

        /// <summary>
        /// 判断交易商品代码是否已经存在
        /// </summary>
        /// <param name="CommodityCode">商品代码</param>
        /// <returns></returns>
        public static bool IsExistCommodityCode(string CommodityCode)
        {
            CM_CommodityBLL cM_CommodityBLL = new CM_CommodityBLL();
            return cM_CommodityBLL.IsExistCommodityCode(CommodityCode);
        }

        #endregion

        #region 判断交易商品名称是否已经存在

        /// <summary>
        /// 判断交易商品名称是否已经存在
        /// </summary>
        /// <param name="CommodityName">商品名称</param>
        /// <returns></returns>
        public static bool IsExistCommodityName(string CommodityName)
        {
            CM_CommodityBLL cM_CommodityBLL = new CM_CommodityBLL();
            return cM_CommodityBLL.IsExistCommodityName(CommodityName);
        }

        #endregion

        //================================ 可交易商品_熔断 方法 ================================

        #region 添加可交易商品_熔断

        /// <summary>
        /// 添加可交易商品_熔断
        /// </summary>
        /// <param name="model">可交易商品_熔断实体</param>
        /// <returns></returns>
        public static bool AddCMCommodityFuse(CM_CommodityFuse model)
        {
            CM_CommodityFuseBLL cM_CommodityFuseBLL = new CM_CommodityFuseBLL();
            return cM_CommodityFuseBLL.AddCMCommodityFuse(model);
        }

        #endregion

        #region 修改可交易商品_熔断

        /// <summary>
        /// 修改可交易商品_熔断
        /// </summary>
        /// <param name="model">可交易商品_熔断实体</param>
        /// <returns></returns>
        public static bool UpdateCMCommodityFuse(CM_CommodityFuse model)
        {
            CM_CommodityFuseBLL cM_CommodityFuseBLL = new CM_CommodityFuseBLL();
            return cM_CommodityFuseBLL.UpdateCMCommodityFuse(model);
        }

        #endregion

        #region 删除可交易商品_熔断

        /// <summary>
        /// 删除可交易商品_熔断
        /// </summary>
        /// <param name="CommodityCode">可交易商品_熔断实体</param>
        /// <returns></returns>
        public static bool DeleteCMCommodityFuse(string CommodityCode)
        {
            CM_CommodityFuseBLL cM_CommodityFuseBLL = new CM_CommodityFuseBLL();
            return cM_CommodityFuseBLL.DeleteCMCommodityFuse(CommodityCode);
        }

        #endregion

        #region 删除可交易商品_熔断(同时删除同一商品代码的熔断_时间段标识表中的记录)

        /// <summary>
        /// 删除可交易商品_熔断(同时删除同一商品代码的熔断_时间段标识表中的记录)
        /// </summary>
        /// <param name="CommodityCode">商品代码</param>
        /// <returns></returns>
        public static bool DeleteCMCommodityFuseAbout(string CommodityCode)
        {
            CM_CommodityFuseBLL cM_CommodityFuseBLL = new CM_CommodityFuseBLL();
            return cM_CommodityFuseBLL.DeleteCMCommodityFuseAbout(CommodityCode);
        }

        #endregion

        #region 获取所有可交易商品_熔断

        /// <summary>
        /// 获取所有可交易商品_熔断
        /// </summary>
        /// <param name="CommodityCode">商品代码</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public static DataSet GetAllCMCommodityFuse(string CommodityCode, int pageNo, int pageSize,
                                                    out int rowCount)
        {
            CM_CommodityFuseBLL cM_CommodityFuseBLL = new CM_CommodityFuseBLL();

            return cM_CommodityFuseBLL.GetAllCMCommodityFuse(CommodityCode, pageNo, pageSize, out rowCount);
        }

        #endregion

        #region 获取品种类型股指期货的商品代码

        /// <summary>
        /// 获取品种类型股指期货的商品代码
        /// </summary>
        /// <returns></returns>
        public static DataSet GetQHSIFCommodityCode()
        {
            CM_CommodityBLL cM_CommodityBLL = new CM_CommodityBLL();
            return cM_CommodityBLL.GetQHSIFCommodityCode();
        }

        #endregion

        #region  判断是否存在可交易商品_熔断记录

        /// <summary>
        /// 判断是否存在可交易商品_熔断记录
        /// </summary>
        /// <param name="CommodityCode">商品代码</param>
        /// <returns></returns>
        public static bool ExistsCommodityCode(string CommodityCode)
        {
            CM_CommodityFuseBLL cM_CommodityFuseBLL = new CM_CommodityFuseBLL();
            return cM_CommodityFuseBLL.ExistsCommodityCode(CommodityCode);
        }

        #endregion

        //================================ 熔断_时间段标识 方法 ================================

        #region 添加熔断_时间段标识

        /// <summary>
        /// 添加熔断_时间段标识
        /// </summary>
        /// <param name="model">熔断_时间段标识实体</param>
        /// <returns></returns>
        public static int AddCMFuseTimesection(ManagementCenter.Model.CM_FuseTimesection model)
        {
            CM_FuseTimesectionBLL cM_FuseTimesectionBLL = new CM_FuseTimesectionBLL();
            return cM_FuseTimesectionBLL.AddCMFuseTimesection(model);
        }

        #endregion

        #region 添加熔断_时间段标识 重载

        /// <summary>
        /// 添加熔断_时间段标识
        /// </summary>
        /// <param name="model">熔断_时间段标识实体</param>
        /// <param name="msg">返回错误结果提示信息</param>
        /// <returns></returns>
        public static int AddCMFuseTimesection(ManagementCenter.Model.CM_FuseTimesection model, ref string msg)
        {
            CM_FuseTimesectionBLL cM_FuseTimesectionBLL = new CM_FuseTimesectionBLL();
            return cM_FuseTimesectionBLL.AddCMFuseTimesection(model, ref msg);
        }

        #endregion

        #region 修改熔断_时间段标识

        /// <summary>
        /// 修改熔断_时间段标识
        /// </summary>
        /// <param name="model">熔断_时间段标识实体</param>
        /// <returns></returns>
        public static bool UpdateCMFuseTimesection(ManagementCenter.Model.CM_FuseTimesection model)
        {
            CM_FuseTimesectionBLL cM_FuseTimesectionBLL = new CM_FuseTimesectionBLL();
            return cM_FuseTimesectionBLL.UpdateCMFuseTimesection(model);
        }

        #endregion

        #region 修改熔断_时间段标识 重载

        /// <summary>
        /// 修改熔断_时间段标识 重载
        /// </summary>
        /// <param name="model">熔断_时间段标识实体</param>
        /// <param name="msg">返回错误结果提示信息</param>
        /// <returns></returns>
        public static bool UpdateCMFuseTimesection(ManagementCenter.Model.CM_FuseTimesection model, ref string msg)
        {
            CM_FuseTimesectionBLL cM_FuseTimesectionBLL = new CM_FuseTimesectionBLL();
            return cM_FuseTimesectionBLL.UpdateCMFuseTimesection(model, ref msg);
        }

        #endregion

        #region 删除熔断_时间段标识

        /// <summary>
        /// 删除熔断_时间段标识
        /// </summary>
        /// <param name="TimesectionID">熔断_时间段标识</param>
        /// <returns></returns>
        public static bool DeleteCMFuseTimesection(int TimesectionID)
        {
            CM_FuseTimesectionBLL cM_FuseTimesectionBLL = new CM_FuseTimesectionBLL();
            return cM_FuseTimesectionBLL.DeleteCMFuseTimesection(TimesectionID);
        }

        #endregion

        #region 根据商品代码获取所有熔断_时间段标识

        /// <summary>
        /// 根据商品代码获取熔断_时间段标识
        /// </summary>
        /// <param name="CommodityCode">商品代码</param>
        /// <returns></returns>
        public static DataSet GetCMFuseTimesectionByCommodityCode(string CommodityCode)
        {
            CM_FuseTimesectionBLL cM_FuseTimesectionBLL = new CM_FuseTimesectionBLL();
            return cM_FuseTimesectionBLL.GetCMFuseTimesectionByCommodityCode(CommodityCode);
        }

        #endregion
    }
}