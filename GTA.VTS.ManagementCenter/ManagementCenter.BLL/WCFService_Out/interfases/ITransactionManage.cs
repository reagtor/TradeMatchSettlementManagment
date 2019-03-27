using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ManagementCenter.DAL.AccountManageService;
using ManagementCenter.Model;
using ManagementCenter.Model.CommonClass;
using ManagementCenter.Model.UserManage;
using Entity = ManagementCenter.Model;

namespace ManagementCenter.BLL.WCFService_Out.interfases
{
    ///<summary> 
    /// 模块编号：
    /// 作用：WCF对外提供的接口-交易员管理
    /// 作者：熊哓凌                 
    /// 编写日期：2008-11-17        
    /// 修改：叶振东
    /// 修改日期：2009-12-23
    /// Update By:李健华
    /// Update Date:2009-12-28
    /// Desc.:修改代码的接口实现定义和修改相关的注释规范
    /// 
    /// 修改：刘书伟
    /// 修改日期：2010-01-07
    /// 描述:添加提供给金融平台的开户方法和转账方法
    /// </summary> 
    [ServiceContract]
    public interface ITransactionManage
    {
        #region  交易员管理提供给前台的服务
        /// <summary>
        /// 添加交易员
        /// </summary>
        /// <param name="Number">开户数目</param>
        /// <param name="initFund"></param>
        ///  <param name="MLoginName">管理员登陆名称</param>
        /// <param name="ManagerPWd">管理员密码</param>
        /// <param name="message">返回的信息</param>
        /// <returns></returns>
        [OperationContract]
        List<Entity.UM_UserInfo> BatchAddTransaction(int Number, InitFund initFund, string MLoginName, string ManagerPWd, out string message);

        /// <summary>
        /// 添加交易员
        /// </summary>
        /// <param name="tUserInfo">用户实体集</param>
        /// <param name="initFund">美元</param>
        /// <param name="MLoginName">管理员登陆名称</param>
        /// <param name="ManagerPWd">管理员密码</param>
        /// <param name="message">返回的信息</param>
        /// <returns></returns>
        [OperationContract]
        List<Entity.UM_UserInfo> BatchAddTransactionList(List<Entity.UM_UserInfo> tUserInfo, InitFund initFund, string MLoginName, string ManagerPWd, out string message);


        /// <summary>
        /// 修改交易员
        /// </summary>
        /// <param name="UserID">交易员ID</param>
        /// <param name="Password">密码</param>
        /// <param name="MLoginName">管理员登陆名称</param>
        /// <param name="ManagerPWd">管理员密码</param>
        /// <param name="message">返回的信息</param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateTransaction(int UserID, string Password, string MLoginName, string ManagerPWd, out string message);


        /// <summary>
        /// 修改交易员
        /// </summary>
        /// <param name="userInfo">交易员实体</param>
        /// <param name="MLoginName">管理员登陆名称</param>
        /// <param name="ManagerPWd">管理员密码</param>
        /// <param name="message">返回的信息</param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateTransactionAll(Entity.UM_UserInfo userInfo, string MLoginName, string ManagerPWd, out string message);


        /// <summary>
        /// 删除交易员
        /// </summary>
        /// <param name="userInfo">交易员实体集</param>
        /// <param name="MLoginName">管理员登陆名称</param>
        /// <param name="ManagerPWd">管理员密码</param>
        /// <param name="message">返回的信息</param>
        /// <returns></returns>
        [OperationContract]
        bool BatchDelectTransaction(List<Entity.UM_UserInfo> userInfo, string MLoginName, string ManagerPWd, out string message);

        #endregion

        ///<summary>
        ///交易员验证
        ///</summary>
        ///<param name="UserID">用户ID</param>
        ///<param name="PassWord">密码</param>
        /// <param name="message">返回的信息</param>
        /// <returns></returns>
        [OperationContract]
        Entity.CT_Counter TransactionConfirm(int UserID, string PassWord, out string message);

        /// <summary>
        /// 追加资金(此方法之前是没有实现的，这里实现只是用于验证管理员是否正确)
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="initFund">美元</param>
        /// <param name="MLoginName">管理员登陆名称</param>
        /// <param name="ManagerPWd">管理员密码</param>
        /// <param name="message">输出信息</param>
        /// <returns></returns>
        [OperationContract]
        bool AddFund(int UserID, InitFund initFund, string MLoginName, string ManagerPWd, out string message);

        #region Create 李健华 新增接口实现 2009-12-28
        /// <summary>
        /// 对管理员用户名和密码进行验证,并清空数据
        /// </summary>
        /// <param name="cap">个性化资金实体类</param>
        /// <param name="LoginName">管理员用户名</param>
        /// <param name="PassWord">密码</param>
        /// <param name="message">返回的信息</param>
        /// <returns>数据是否清空成功</returns>
        [OperationContract]
        bool ClearTrialData(PersonalizationCapital cap, string LoginName, string PassWord, out string message);

        /// <summary>
        /// 对管理员用户名和密码进行验证,并个性化设置操作
        /// </summary>
        /// <param name="cap">个性化资金实体类</param>
        /// <param name="LoginName">管理员用户名</param>
        /// <param name="PassWord">密码</param>
        /// <param name="message">返回的信息</param>
        /// <returns>格式化资金是否成功</returns>
        [OperationContract]
        bool PersonalizationCapital(PersonalizationCapital cap, string LoginName, string PassWord, out string message);
        #endregion

        //===============================================提供给金融工程平台的接口=====================================

        #region 金融工程平台调用的添加交易员

        /// <summary>
        /// 金融工程平台调用的添加交易员
        /// </summary>
        /// <param name="MLoginName">登陆名称（管理员角色）</param>
        /// <param name="ManagerPWd">登陆密码(管理员密码)</param>
        /// <param name="UserInfo">用户基本信息</param>
        /// <param name="initfund">初始化资金</param>
        /// <param name="l_AccountEntity">账户列表实体</param>
        /// <param name="message">输出信息</param>
        /// <returns>返回用户基本信息</returns>
        [OperationContract]
        UM_UserInfo AddTransactionFP(string MLoginName, string ManagerPWd, UM_UserInfo UserInfo, InitFund initfund,
                                     out List<AccountEntity> l_AccountEntity, out string message);
        #endregion

        #region 管理员查询根据交易员查询交易员各资金账户相关信息

        /// <summary>
        /// 管理员查询根据交易员查询交易员各资金账户相关信息
        /// </summary>
        /// <param name="adminId">管理员ID(可传空值：因柜台没验证)</param>
        /// <param name="adminPassword">管理员密码(可传空值：因柜台没验证)</param>
        /// <param name="traderID">交易员ID</param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <returns>返回交易员的资金明细</returns>
        [OperationContract]
        List<TradersAccountCapitalInfo> AdminFindTraderCapitalAccountInfoByIDFP(string adminId, string adminPassword, string traderID,
                                                               out string strErrorMessage);
        #endregion

        #region 调用自由转帐（同币种)方法

        /// <summary>
        /// 调用自由转帐（同币种)方法
        /// </summary>
        /// <param name="traderID">交易员ID(用户ID)</param>
        /// <param name="FromCapitalAccountType">转出资金账户类型</param>
        /// <param name="ToCapitalAccountType">转入资金账户类型</param>
        /// <param name="TransferAmount">转账数量</param>
        /// <param name="currencyType">币种类型</param>
        /// <param name="outMessage">输出消息</param>
        /// <returns>返回转账是否成功</returns>
        [OperationContract]
        bool ConvertFreeTransferEntityFP(int traderID, int FromCapitalAccountType, int ToCapitalAccountType,
                                       decimal TransferAmount, int currencyType, out string outMessage);

        #endregion

        #region 追加资金
        /// <summary>
        /// 追加资金
        /// </summary>
        /// <param name="model">追加资金实体</param>
        /// <param name="mess">返回信息</param>
        /// <returns>返回是否成功</returns>
        [OperationContract]
        bool AddFundFP(UM_FundAddInfo model, out string mess);
        #endregion

        #region  检查管理中心服务通道
        /// <summary>
        /// 检查管理中心服务通道
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string CheckChannel();
        #endregion

    }
}
