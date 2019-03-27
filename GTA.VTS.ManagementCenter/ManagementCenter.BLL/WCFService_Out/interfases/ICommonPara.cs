using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity = ManagementCenter.Model;
using System.ServiceModel;

namespace ManagementCenter.BLL.WCFService_Out.interfases
{
    /// <summary>
    /// 对外公共参数(用户管理，现货，期货)接口
    /// 作者：刘书伟
    /// 日期：2008-11-20  修改日期：2009-10-22
    /// </summary>
    [ServiceContract]
    public interface ICommonPara
    {
        /// <summary>
        /// 获取所有的交易所类型
        /// </summary>
        /// <returns>交易所类型列表</returns>
        [OperationContract]
        List<Entity.CM_BourseType> GetAllBourseType();

        /// <summary>
        /// 根据交易所类型标识返回交易所类型
        /// </summary>
        /// <param name="bourseTypeID">交易所类型标识</param>
        /// <returns>交易所类型</returns>
        [OperationContract]
        Entity.CM_BourseType GetBourseTypeByBourseTypeID(int bourseTypeID);

        /// <summary>
        /// 获取所有的交易商品品种
        /// </summary>
        /// <returns>商品品种列表</returns>
        [OperationContract]
        List<Entity.CM_BreedClass> GetAllBreedClass();

        /// <summary>
        /// 获取所有交易商品品种类型
        /// </summary>
        /// <returns>商品品种类型列表</returns>
        [OperationContract]
        List<Entity.CM_BreedClassType> GetAllBreedClassType();

        /// <summary>
        /// 获取所有的交易商品
        /// </summary>
        /// <returns>商品列表</returns>
        [OperationContract]
        List<Entity.CM_Commodity> GetAllCommodity();

        /// <summary>
        /// 根据商品代码返回交易商品
        /// </summary>
        /// <param name="commodityCode">商品代码</param>
        /// <returns>商品表</returns>
        [OperationContract]
        Entity.CM_Commodity GetCommodityByCommodityCode(string commodityCode);

        /// <summary>
        /// 根据品种标识返回交易商品
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns>商品列表</returns>
        [OperationContract]
        List<Entity.CM_Commodity> GetCommodityByBreedClassID(int breedClassID);

        /// <summary>
        /// 根据品种标识返回交易商品品种
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns>商品品种</returns>
        [OperationContract]
        Entity.CM_BreedClass GetBreedClassByBreedClassID(int breedClassID);

        /// <summary>
        /// 根据交易所类型标识返回交易商品品种
        /// </summary>
        /// <param name="bourseTypeID">交易所类型标识</param>
        /// <returns>商品品种列表</returns>
        [OperationContract]
        List<Entity.CM_BreedClass> GetBreedClassByBourseTypeID(int bourseTypeID);

        /// <summary>
        /// 获取所有的可交易商品_熔断
        /// </summary>
        /// <returns>可交易商品_熔断列表</returns>
        [OperationContract]
        List<Entity.CM_CommodityFuse> GetAllCommodityFuse();

        /// <summary>
        /// 根据商品代码返回可交易商品_熔断
        /// </summary>
        /// <param name="commodityCode">商品代码</param>
        /// <returns>可交易商品_熔断</returns>
        [OperationContract]
        Entity.CM_CommodityFuse GetCommodityFuseByCommodityCode(string commodityCode);

        /// <summary>
        /// 获取所有的熔断_时间段标识
        /// </summary>
        /// <returns>熔断_时间段标识列表</returns>
        [OperationContract]
        List<ManagementCenter.Model.CM_FuseTimesection> GetAllFuseTimesection();


        /// <summary>
        /// 根据商品代码返回熔断_时间段标识
        /// </summary>
        /// <param name="commodityCode">商品代码</param>
        /// <returns>熔断_时间段标识列表</returns>
        [OperationContract]
        List<ManagementCenter.Model.CM_FuseTimesection> GetFuseTimesectionByCommodityCode(string commodityCode);

        /// <summary>
        /// 获取所有的交易货币类型
        /// </summary>
        /// <returns>交易货币类型列表</returns>
        [OperationContract]
        List<Entity.CM_CurrencyType> GetAllCurrencyType();

        /// <summary>
        /// 获取所有的股票分红记录_现金
        /// </summary>
        /// <returns>股票分红记录_现金</returns>
        [OperationContract]
        List<Entity.CM_StockMelonCash> GetAllStockMelonCash();

        /// <summary>
        /// 获取所有的股票分红记
        /// 录_股票
        /// </summary>
        /// <returns>股票分红记录_股票</returns>
        [OperationContract]
        List<Entity.CM_StockMelonStock> GetAllStockMelonStock();

    

        /// <summary>
        /// 获取所有的交易所类型_交易时间
        /// </summary>
        /// <returns>交易所交易时间</returns>
        [OperationContract]
        List<Entity.CM_TradeTime> GetAllTradeTime();

        /// <summary>
        /// 根据交易所类型标识返回交易时间
        /// </summary>
        /// <param name="bourseTypeID">交易所类型</param>
        /// <returns>交易所交易时间</returns>
        [OperationContract]
        List<Entity.CM_TradeTime> GetTradeTimeByBourseTypeID(int bourseTypeID);

        /// <summary>
        /// 获取所有的交易方向
        /// </summary>
        /// <returns>交易方向</returns>
        [OperationContract]
        List<Entity.CM_TradeWay> GetAllTradeWay();

        /// <summary>
        /// 获取所有的现货_品种_交易单位换算
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.CM_UnitConversion> GetAllUnitConversion();

        /// <summary>
        /// 根据品种标识获取现货_品种_交易单位换算
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns>现货_品种_交易单位换算表</returns>
        [OperationContract]
        List<Entity.CM_UnitConversion> GetUnitConversionByBreedClassID(int breedClassID);

        /// <summary>
        /// 获取所有的单位
        /// </summary>
        /// <returns>单位</returns>
        [OperationContract]
        List<Entity.CM_Units> GetAllUnits();

        /// <summary>
        /// 获取所有的交易规则_取值类型
        /// </summary>
        /// <returns>交易规则_取值类型</returns>
        [OperationContract]
        List<Entity.CM_ValueType> GetAllValueType();

        #region

        /// <summary>
        /// 获取柜台列表
        /// </summary>
        /// <returns>柜台列表</returns>
        [OperationContract]
        List<Entity.CT_Counter> GetAllCounter();

        /// <summary>
        /// 获取撮合中心列表
        /// </summary>
        /// <returns>撮合中心列表</returns>
        [OperationContract]
        List<Entity.RC_MatchCenter> GetAllMatchCenter();

        /// <summary>
        /// 根据IP和端口获取撮合中心
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="port">端口</param>
        /// <returns>撮合中心</returns>
        [OperationContract]
        Entity.RC_MatchCenter GetMatchCenterByIpAndPort(string ip, int port);
        /// <summary>
        /// 获取撮合机列表
        /// </summary>
        /// <returns>撮合机列表</returns>
        [OperationContract]
        List<Entity.RC_MatchMachine> GetAllMatchMachine();

        /// <summary>
        /// 根据撮合中心获取撮合机列表
        /// </summary>
        /// <returns>撮合机列表</returns>
        [OperationContract]
        List<Entity.RC_MatchMachine> GetAllMatchMachineByMatchCenter(int MatchCenterID);
        /// <summary>
        /// 根据撮合机ID获取撮合机
        /// </summary>
        /// <returns>撮合机</returns>
        [OperationContract]
        Entity.RC_MatchMachine GetMatchMachine(int MatchMachineID);

        /// <summary>
        /// 获取撮合机—代码分配列表
        /// </summary>
        /// <returns>撮合机代码分配列表</returns>
        [OperationContract]
        List<Entity.RC_TradeCommodityAssign> GetAllTradeCommodityAssign();

        /// <summary>
        /// 获取一个撮合机分配的代码列表
        /// </summary>
        /// <returns>撮合机代码分配列表</returns>
        [OperationContract]
        List<Entity.RC_TradeCommodityAssign> GetCommodityAssignByMachineID(int MatchMachineID);

        /// <summary>
        /// 获取代码所属的撮合机代码分配列表
        /// </summary>
        /// <returns>撮合机代码分配列表</returns>
        [OperationContract]
        Entity.RC_TradeCommodityAssign GetTradeCommodityAssign(string CommodityCode);

        /// <summary>
        /// 获取代码所属的撮合机
        /// </summary>
        /// <returns>撮合机</returns>
        [OperationContract]
        Entity.RC_MatchMachine GetMatchMachinebyCommodity(string CommodityCode);

        /// <summary>
        /// 获取所有帐户类型
        /// </summary>
        /// <returns>帐户类型</returns>
        [OperationContract]
        List<Entity.UM_AccountType> GetALLAccountType();

        /// <summary>
        /// 根据交易所类型标识返回非交易日期
        /// </summary>
        /// <param name="bourseTypeID">交易所类型标识</param>
        /// <returns>非交易日期</returns>
        [OperationContract]
        List<ManagementCenter.Model.CM_NotTradeDate> GetNotTradeDateByBourseTypeID(int bourseTypeID);

        /// <summary>
        /// 根据交易所类型和日期判断是否为交易日
        /// </summary>
        /// <param name="bourseTypeID">交易所类型标识</param>
        /// <param name="dt">日期</param>
        /// <returns>是否为交易日</returns>
        [OperationContract]
        bool JudgeIsTradeDateByBourseTypeID(int bourseTypeID, DateTime dt);

        /// <summary>
        /// 返回所有的非交易日期
        /// </summary>
        /// <returns>非交易日期</returns>
        [OperationContract]
        List<ManagementCenter.Model.CM_NotTradeDate> GetAllNotTradeDate();

        #endregion

        /// <summary>
        /// 获取新股上市的商品
        /// </summary>
        /// <returns>商品表</returns>
        [OperationContract]
        List<Entity.CM_Commodity> GetNewCommodity();

        /// <summary>
        /// 获取增发上市的商品代码
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entity.ZFInfo> GetZFCommodity();


        #region 前台接口
        /// <summary>
        /// 提供前台获取商品代码列表
        /// </summary>
        /// <returns>商品代码列表</returns>
        [OperationContract]
        List<Entity.CommonTable.OnstageCommodity> GetCommodityListArray();

        /// <summary>
        /// 提供前台根据品种获取币种
        /// </summary>
        /// <returns>币种</returns>
        [OperationContract]
        Entity.CM_CurrencyBreedClassType GetCurrencyByBreedClassID(int BreedClassID);

        /// <summary>
        ///获取所有品种与币种关系列表
        /// </summary>
        /// <returns>品种与币种关系列表</returns>
        [OperationContract]
        List<Entity.CM_CurrencyBreedClassType> GetListCurrencyBreedClass();

        /// <summary>
        /// 获取行业列表
        /// </summary>
        /// <returns>行业列表</returns>
        [OperationContract]
        List<ManagementCenter.Model.Profession> GetListProfessionArray();

        #region add by 董鹏 2010-05-19

        /// <summary>
        /// 获取指定日期区间内(除权基准日)所有的股票分红记录_现金
        /// （开始或结束日期为空则取今天的日期）
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>股票分红记录_现金</returns>
        [OperationContract]
        List<Entity.CM_StockMelonCash> GetStockMelonCashByDateRange(DateTime? startDate, DateTime? endDate);

        /// <summary>
        /// 获取指定日期区间内(除权基准日)所有的股票分红记录_股票
        /// （开始或结束日期为空则取今天的日期）
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>股票分红记录_股票</returns>
        [OperationContract]
        List<Entity.CM_StockMelonStock> GetStockMelonStockByDateRange(DateTime? startDate, DateTime? endDate);

        #endregion

        #endregion

        /// <summary>
        /// 根据交易员获取拥有的交易品种权限
        /// </summary>
        /// <returns>交易品种权限</returns>
        [OperationContract]
        List<Entity.UM_DealerTradeBreedClass> TransactionRightTable(int UserID);

        /// <summary>
        /// 获取所有的股票收盘价
        /// </summary>
        /// <returns>现货收盘价</returns>
        [OperationContract]
        List<Entity.ClosePriceInfo> GetAllClosePriceInfo();

        /// <summary>
        /// 根据品种类型标识获取股票收盘价
        /// </summary>
        /// <param name="BreedClassTypeID">品种类型标识</param>
        /// <returns>现货收盘价</returns>
        [OperationContract]
        List<Entity.ClosePriceInfo> GetAllClosePriceInfoByBreedClassTypeID(int BreedClassTypeID);

    }
}