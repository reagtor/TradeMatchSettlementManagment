using System;
using System.Collections.Generic;
using System.Text;
using GTA.VTS.Common.CommonObject;
using System.ServiceModel;
using ReckoningCounter.BLL.DelegateValidate;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.Entity.AccountManagementAndFindEntity;
using ReckoningCounter.Entity.Model.QH;

namespace ReckoningCounter.BLL.AccountManagementAndFind.InterfaceBLL
{
    /// <summary>
    /// 作用：账户管理接口（包括：单个交易员开户、批量开户、单个交易员销户、批量销户、 冻结账户、解冻账户、查询账户、查询账户、修改密码、追加资金、自由转账）
    /// 作者：李科恒
    /// 日期：2008年11月24日
    /// Update by:李健华
    /// Update date:2009-12-23
    /// Desc.:添加个性化资金设置接口
    /// Update by:董鹏
    /// Update date:2009-12-23
    /// Desc.:添加执行试玩期后用户交易数据清空接口
    /// </summary>
    [ServiceContract]
    public interface IAccountAndCapitalManagement
    {
        /// <summary>
        /// 单个交易员开户
        /// </summary>
        /// <param name="accounts">账户实体对象</param>
        /// <param name="outMessage">输出参数</param>
        /// <returns></returns>
        [OperationContract]
        bool SingleTraderOpenAccount(List<AccountEntity> accounts, out string outMessage);

        /// <summary>
        /// 批量开户      
        /// </summary>
        /// <param name="accounts">账户实体对象</param>
        /// <param name="outMessage">输出参数</param>
        /// <returns></returns>
        [OperationContract]
        bool VolumeTraderOpenAccount(List<AccountEntity> accounts, out string outMessage);

        /// <summary>
        ///  单个交易员销户  
        /// </summary>
        /// <param name="userId">交易员ID</param>
        /// <param name="outMessage">输出参数</param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteSingleTraderAccount(string userId, out string outMessage);

        /// <summary>
        /// 批量销户
        /// </summary>
        /// <param name="userIDs">交易员ID字符串数组</param>
        /// <param name="outMessage">输出参数</param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteVolumeTraderAccount(string[] userIDs, out string outMessage);

        /// <summary>
        /// 冻结账户
        /// </summary>
        /// <param name="accounts">账户实体对象</param>
        /// <param name="outMessage">输出参数</param>
        /// <returns></returns>
        [OperationContract]
        bool FreezeAccount(List<FindAccountEntity> accounts, out string outMessage);

        /// <summary>
        /// 解冻账户
        /// </summary>
        /// <param name="accounts">账户实体对象</param>
        /// <param name="outMessage">输出参数</param>
        /// <returns></returns>
        [OperationContract]
        bool ThawAccount(List<FindAccountEntity> accounts, out string outMessage);

        /// <summary>
        /// 查询帐户
        /// </summary>
        /// <param name="password">交易员密码</param>
        /// <param name="outMessage">输出信息</param>
        /// <param name="traderId">交易员ID</param>
        /// <returns></returns>
        [OperationContract]
        List<AccountFindResultEntity> FindAccount(string traderId, string password, out string outMessage);

        /// <summary>
        ///  查询交易权限
        /// </summary>
        /// <param name="password">交易员密码</param>
        /// <param name="outMessage">输出信息</param>
        /// <param name="traderId">交易员ID</param>
        /// <returns></returns>
        [OperationContract]
        List<CM_BreedClass> FindTradePrivileges(string traderId, string password, out string outMessage);

        /// <summary>
        /// 交易员修改密码  
        /// </summary>
        /// <param name="userId">交易员ID</param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="outMessage">输出参数</param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateUserPassword(string userId, string oldPassword, string newPassword, out string outMessage);

        /// <summary>
        /// 管理员给交易员追加资金 
        /// </summary>
        /// <param name="addCapital">追加资金实体</param>
        /// <param name="outMessage">输出参数</param>
        /// <returns></returns>
        [OperationContract]
        bool AddCapital(AddCapitalEntity addCapital, out string outMessage);

        /// <summary>
        /// 交易员的几个资金账户之间两两自由转账（同币种）
        /// </summary>
        /// <param name="freeTransfer">自由转账实体对象</param>
        /// <param name="currencyType">币种</param>
        /// /// <param name="outMessage">输出参数</param>
        /// <returns></returns>
        [OperationContract]
        bool TwoAccountsFreeTransferFunds(FreeTransferEntity freeTransfer, Types.CurrencyType currencyType, out string outMessage);

        /// <summary>
        /// 检查通道
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string CheckChannel();

        /// <summary>
        /// 根据日期查询柜台清算是否已经完成
        /// </summary>
        /// <param name="doneDate">日期</param>
        /// <returns></returns>
        [OperationContract]
        bool IsReckoningDone(DateTime doneDate);


        /// <summary>
        /// 柜台清算是否正在清算
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool IsReckoning();

        /// <summary>
        /// 故障恢复清算，此方法只能由管理中心调用
        /// </summary>
        /// <param name="list">要提供的期货当日结算价列表</param>
        /// <param name="errorMsg">执行异常信息</param>
        /// <returns></returns>
        [OperationContract]
        bool FaultRecoveryReckoning(List<QH_TodaySettlementPriceInfo> list, out string errorMsg);

        /// <summary>
        /// 根据用户得到某一商品代码的最大委托量
        /// </summary>
        /// <param name="TraderId">交易员ID</param>
        /// <param name="OrderPrice">委托价格</param>
        /// <param name="Code">代码</param>
        /// <param name="outMessage"></param>
        /// <param name="orderPriceType"></param>
        /// <returns></returns>
        [OperationContract]
        long GetSpotMaxOrderAmount(string TraderId, float OrderPrice, string Code, out string outMessage, Entity.Contants.Types.OrderPriceType orderPriceType);

        /// <summary>
        /// 根据用户得到某一商品代码的最大委托量（股指期货和商品期货都用此方法）
        /// </summary>
        /// <param name="TraderId">交易员ID</param>
        /// <param name="OrderPrice">委托价格</param>
        /// <param name="Code">代码</param>
        /// <param name="outMessage">返回信息</param>
        /// <param name="orderPriceType">价格类型</param>
        /// <returns></returns>
        [OperationContract]
        long GetFutureMaxOrderAmount(string TraderId, float OrderPrice, string Code, out string outMessage, Entity.Contants.Types.OrderPriceType orderPriceType);

        /// <summary>
        /// 根据商品代码和委托价格获取涨跌幅
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="orderPrice">委托价格</param>
        /// <returns>涨跌幅对象</returns>
        [OperationContract]
        HighLowRangeValue GetHighLowRangeValueByCommodityCode(string code, decimal orderPrice);

        /// <summary>
        /// 根据用户得到某一港股商品代码的最大委托量
        /// </summary>
        /// <param name="TraderId">交易员ID</param>
        /// <param name="OrderPrice">委托价格</param>
        /// <param name="Code">代码</param>
        /// <param name="outMessage">返回信息</param>
        /// <param name="orderPriceType">港股价格类型(限价盘,增强限价盘,特别限价盘)</param>
        /// <returns></returns>
        [OperationContract]
        long GetHKMaxOrderAmount(string TraderId, float OrderPrice, string Code, out string outMessage, Types.HKPriceType orderPriceType);

        /// <summary>
        /// 根据商品代码和委托价格获取上下限（涨跌幅值）
        /// </summary>
        /// <param name="code">港股商品代码</param>
        /// <param name="orderPrice">委托价格</param>
        /// <param name="priceType">港股价格类型(限价盘,增强限价盘,特别限价盘)</param>
        /// <param name="tranType">交易方向</param>
        /// <returns></returns>
        [OperationContract]
        HighLowRangeValue GetHKHighLowRangeValueByCommodityCode(string code, decimal orderPrice, Types.HKPriceType priceType, Types.TransactionDirection tranType);

        /// <summary>
        /// Title:管理员查询根据交易员查询交易员各资金账户相关信息
        /// Create By:李健华
        /// Create Date:2009-11-02
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="adminPassword">管理员密码</param>
        /// <param name="traderId">交易员ID</param>
        /// <param name="strErrorMessage">查询相关异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<TradersAccountCapitalInfo> AdminFindTraderCapitalAccountInfo(string adminId, string adminPassword, string traderId, out string strErrorMessage);
        /// <summary>
        /// Title:根据代码和代码品种类型查询当前行情
        /// Desc.:因目前行情件同时加载多个服务点用CPU使用率，所以为了开启此方法用于内部测试启动测试端不用加载
        ///        行情组件接口，而提供此方法获取行情
        /// Create By:李健华
        /// Create Date:2009-11-08
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="breedClassType">所属商品类型（1-现货,2-商品期货,3-股指期货,4-港股)</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns>最新成交价</returns>
        [OperationContract]
        MarketDataLevel GetMarketDataInfoByCode(string code, int breedClassType, out string errMsg);

        /// <summary>
        /// 根据品种类别获取相关所有柜台缓存的当前所有代码
        /// </summary>
        /// <param name="classTypeID">品种类型</param>
        /// <param name="isRemoveExpired">是否排除期货过期代码</param>
        /// <returns>返回相关的所有品种类型代码</returns>
        [OperationContract]
        List<string> GetAllCM_CommodityByBreedClassTypeID(int classTypeID, bool isRemoveExpired);

        /// <summary>
        /// Title:管理员设置对资金个性化设置操作
        /// Desc.:管理员设置对资金个性化设置操作
        /// Create by:李健华
        /// Create Date:2009-12-23
        /// Update by:董鹏
        /// Update date:2009-12-23
        /// Desc.:去掉了管理员ID和密码string admin, string pwd，验证由管理中心进行
        /// </summary>
        /// <param name="model">要设置的交易员资金实体</param>
        /// <param name="errMsg">操作异常信息</param>
        /// <returns></returns>
        [OperationContract]
        bool AdminPersonalizationCapital(CapitalPersonalization model, out string errMsg);

        /// <summary>
        /// Title:管理员执行试玩期后用户交易数据清空操作
        /// Desc.:管理员执行试玩期后用户交易数据清空操作
        /// Create by:董鹏
        /// Create Date:2009-12-23
        /// </summary>
        /// <param name="model">要设置的交易员资金实体</param>
        /// <param name="errMsg">操作异常信息</param>
        /// <returns>是否执行成功</returns>
        [OperationContract]
        bool AdminClearTrialData(CapitalPersonalization model, out string errMsg);

        /// <summary>
        /// 获取当前所有持仓中要提供当日结算价清算的代码
        /// 如果返回为null的话即不用做故障恢复，但如果为list.count=0或者>0即要做故障恢复
        /// </summary>
        /// <param name="errMsg">查询返回异常或者提示信息</param>
        /// <returns></returns>
        [OperationContract]
        List<QH_TodaySettlementPriceInfo> GetAllReckoningHoldCode(out string errMsg);

    }
}