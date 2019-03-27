using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ManagementCenter.DAL;
using ManagementCenter.DAL.AccountManageService;
using ManagementCenter.Model;

namespace ManagementCenter.BLL.ServiceIn
{
    /// <summary>
    /// 描述:帐户管理代理类
    /// 错误编码范围:8100-8130
    /// 作者：熊晓凌                       
    /// 日期：2008-11-20           
    /// 修改：刘书伟
    /// 修改日期:2009-11-02
    /// 修改：叶振东
    /// 修改日期：2009-12-23
    /// </summary>
    public class AccountManageServiceProxy
    {
        /// <summary>
        /// 帐户管理代理变量
        /// </summary>
        private static AccountManageServiceProxy instance;

        /// <summary>
        /// 单例
        /// </summary>
        /// <returns></returns>
        public static AccountManageServiceProxy GetInstance()
        {
            if (instance == null)
                instance = new AccountManageServiceProxy();
            return instance;
        }

        /// <summary>
        /// 清算柜台资金账户管理配置名称
        /// </summary>
        private static string endpiontConfigurationName = "NetTcpBinding_IAccountAndCapitalManagement";

        #region 清算柜台

        /// <summary>
        /// 根据IP，端口和服务名称获取清算柜台资金账户管理服务
        /// </summary>
        /// <param name="ip">柜台服务IP</param>
        /// <param name="port">柜台服务端口</param>
        /// <param name="name">柜台服务名称</param>
        /// <returns>返回清算柜台资金账户管理服务连接</returns>
        public AccountAndCapitalManagementClient GetClient(string ip, int port, string name)
        {
            AccountAndCapitalManagementClient client;
            try
            {
                EndpointAddress tcpAddress = new EndpointAddress(string.Format("net.tcp://{0}:{1}/{2}", ip, port, name));
                client = new AccountAndCapitalManagementClient(endpiontConfigurationName, tcpAddress);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8100";
                string errMsg = "无法获取清算柜台提供的服务[IAccountAndCapitalManagement],IP为：" + ip;
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                throw vte;
            }
            return client;
        }

        /// <summary>
        /// 根据柜台实体获取清算柜台资金账户管理服务
        /// </summary>
        /// <param name="CT">柜台实体</param>
        /// <returns>返回清算柜台资金账户管理服务连接</returns>
        private AccountAndCapitalManagementClient GetClient(CT_Counter CT)
        {
            AccountAndCapitalManagementClient client;
            try
            {
                EndpointAddress tcpAddress =
                    new EndpointAddress(string.Format("net.tcp://{0}:{1}/{2}", CT.IP, CT.AccountServicePort, CT.AccountServiceName));
                client = new AccountAndCapitalManagementClient(endpiontConfigurationName, tcpAddress);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8110";
                string errMsg = "无法获取清算柜台提供的服务[IAccountAndCapitalManagement],IP为：" + CT.IP;
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                throw vte;
            }
            return client;
        }

        #endregion

        /// <summary>
        /// 单个交易员开户
        /// </summary>
        /// <param name="CT">柜台表</param>
        /// <param name="l_AccountEntity">帐户实体</param>
        /// <param name="mess">输出信息</param>
        /// <returns></returns>
        public bool SingleTraderOpenAccount(CT_Counter CT, List<AccountEntity> l_AccountEntity, out string mess)
        {
            try
            {
                using (AccountAndCapitalManagementClient client = GetClient(CT))
                {
                    //string mess;
                    return client.SingleTraderOpenAccount(out mess, l_AccountEntity);
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-8101";
                string errMsg = "调用清算柜台提供的SingleTraderOpenAccount()方法异常," + ex.Message;
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                throw vte;
            }
        }

        /// <summary>
        /// 批量开户
        /// </summary>
        /// <param name="CT">柜台表</param>
        /// <param name="l_AccountEntity">帐户信息</param>
        /// <param name="mess">输出信息</param>
        /// <returns></returns>
        public bool VolumeTraderOpenAccount(CT_Counter CT, List<AccountEntity> l_AccountEntity, out string mess)
        {
            try
            {
                using (AccountAndCapitalManagementClient client = GetClient(CT))
                {
                    //string mess;
                    return client.VolumeTraderOpenAccount(out mess, l_AccountEntity);
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-8102";
                string errMsg = "调用清算柜台提供的VolumeTraderOpenAccount()方法异常," + ex.Message;
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                throw vte;
            }
        }

        /// <summary>
        /// 单个销户
        /// </summary>
        /// <param name="CT">柜台表</param>
        /// <param name="UserID">用户ID</param>
        /// <param name="mess">输出信息</param>
        /// <returns></returns>
        public bool DeleteSingleTraderAccount(CT_Counter CT, string UserID, out string mess)
        {
            try
            {
                using (AccountAndCapitalManagementClient client = GetClient(CT))
                {
                    //string mess;
                    return client.DeleteSingleTraderAccount(out mess, UserID);
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-8103";
                string errMsg = "调用清算柜台提供的DeleteSingleTraderAccount()方法异常," + ex.Message;
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                throw vte;
            }
        }

        /// <summary>
        /// 批量销户
        /// </summary>
        /// <param name="CT">柜台表</param>
        /// <param name="Userids">用户ID</param>
        /// <param name="mess">输出信息</param>
        /// <returns></returns>
        public bool DeleteVolumeTraderAccount(CT_Counter CT, List<string> Userids, out string mess)
        {
            try
            {
                using (AccountAndCapitalManagementClient client = GetClient(CT))
                {
                    //string mess;
                    return client.DeleteVolumeTraderAccount(out mess, Userids);
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-8104";
                string errMsg = "调用清算柜台提供的DeleteVolumeTraderAccount()方法异常," + ex.Message;
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                throw vte;
            }
        }

        /// <summary>
        /// 冻结帐户
        /// </summary>
        /// <param name="CT">柜台表</param>
        /// <param name="l_AccountEntity">帐户实体</param>
        /// <param name="mess">输出信息</param>
        /// <returns></returns>
        public bool FreezeAccount(CT_Counter CT, List<FindAccountEntity> l_AccountEntity, out string mess)
        {
            try
            {
                using (AccountAndCapitalManagementClient client = GetClient(CT))
                {
                    //string mess;
                    return client.FreezeAccount(out mess, l_AccountEntity);
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-8105";
                string errMsg = "调用清算柜台提供的FreezeAccount()方法异常," + ex.Message;
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                throw vte;
            }
        }

        /// <summary>
        /// 解冻帐户
        /// </summary>
        /// <param name="CT">柜台表</param>
        /// <param name="l_AccountEntity">帐户实体</param>
        /// <param name="mess">输出信息</param>
        /// <returns></returns>
        public bool ThawAccount(CT_Counter CT, List<FindAccountEntity> l_AccountEntity, out string mess)
        {
            try
            {
                using (AccountAndCapitalManagementClient client = GetClient(CT))
                {
                    //string mess;
                    return client.ThawAccount(out mess, l_AccountEntity);
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-8106";
                string errMsg = "调用清算柜台提供的ThawAccount()方法异常," + ex.Message;
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                throw vte;
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="CT">柜台表</param>
        /// <param name="UserID">用户ID</param>
        /// <param name="PassWord">密码</param>
        /// <param name="mess">输出信息</param>
        /// <returns></returns>
        public bool UpdateUserPassword(CT_Counter CT, int UserID, string PassWord, out string mess)
        {
            try
            {
                using (AccountAndCapitalManagementClient client = GetClient(CT))
                {
                    // string mess;
                    UM_UserInfo user = StaticDalClass.UserInfoDAL.GetModel(UserID);
                    string oldPassword = user.Password;
                    return client.UpdateUserPassword(out mess, UserID.ToString(), oldPassword, PassWord);
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-8107";
                string errMsg = "调用清算柜台提供的UpdateUserPassword()方法异常," + ex.Message;
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                throw vte;
            }
        }

        /// <summary>
        /// 追加资金
        /// </summary>
        /// <param name="CT">柜台表</param>
        /// <param name="addCapitalEntity">添加资金实体</param>
        /// <param name="mess">输出信息</param>
        /// <returns></returns>
        public bool AddCapital(CT_Counter CT, AddCapitalEntity addCapitalEntity, out string mess)
        {
            try
            {
                using (AccountAndCapitalManagementClient client = GetClient(CT))
                {
                    //string mess;
                    return client.AddCapital(out mess, addCapitalEntity);
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-8108";
                string errMsg = "调用清算柜台提供的AddCapital()方法异常," + ex.Message;
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                throw vte;
            }
        }

        /// <summary>
        /// 测试服务连接方法
        /// </summary>
        /// <param name="ip">服务器IP</param>
        /// <param name="port">服务器端口</param>
        /// <param name="name">服务器名称</param>
        /// <returns></returns>
        public bool TestConnection(string ip, int port, string name)
        {
            bool falg = false;
            try
            {
                using (AccountAndCapitalManagementClient client = GetClient(ip, port, name))
                {
                    string mess = client.CheckChannel();
                    if (!string.IsNullOrEmpty(mess)) falg = true;
                    return falg;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-8109";
                string errMsg = "检测柜台连接失败！ip为" + ip;
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                throw vte;
            }
        }

        #region 管理员查询根据交易员查询交易员各资金账户相关信息
        /// <summary>
        /// 管理员查询根据交易员查询交易员各资金账户相关信息
        /// </summary>
        /// <param name="CT">柜台表</param>
        /// <param name="adminId">管理员ID</param>
        /// <param name="adminPassword">管理员密码</param>
        /// <param name="traderId">交易员ID</param>
        /// <param name="strErrorMessage">查询相关异常信息</param>
        /// <returns></returns>
        public List<TradersAccountCapitalInfo> AdminFindTraderCapitalAccountInfoByID(CT_Counter CT, string adminId, string adminPassword, string traderId, out string strErrorMessage)
        {
            try
            {
                using (AccountAndCapitalManagementClient client = GetClient(CT))
                {
                    //string mess;
                    return client.AdminFindTraderCapitalAccountInfo(out strErrorMessage, adminId, adminPassword, traderId);
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-8111";
                string errMsg = "调用清算柜台提供的AdminFindTraderCapitalAccountInfoByID()方法异常," + ex.Message;
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                throw vte;
            }
        }
        #endregion

        #region 自由转帐（同币种）
        /// <summary>
        /// 自由转帐（同币种）
        /// </summary>
        /// <param name="CT">柜台表</param>
        /// <param name="freeTransfer"> 自由转账实体</param>
        /// <param name="currencyType">当前币种类型</param>
        /// <param name="outMessage">输出信息</param>
        /// <returns></returns>
        public bool TwoAccountsFreeTransferFunds(CT_Counter CT, FreeTransferEntity freeTransfer, TypesCurrencyType currencyType,
                                                 out string outMessage)
        {
            try
            {
                using (AccountAndCapitalManagementClient client = GetClient(CT))
                {
                    //string mess;
                    return client.TwoAccountsFreeTransferFunds(out outMessage, freeTransfer, currencyType);
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-8112";
                string errMsg = "调用清算柜台提供的TwoAccountsFreeTransferFunds()方法异常," + ex.Message;
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                throw vte;
            }
        }
        #endregion

        /// <summary>
        /// 清空数据
        /// </summary>
        /// <param name="CT">柜台信息</param>
        /// <param name="cap">个性化资金实体类</param>
        /// <param name="mess">返回的错误信息</param>
        /// <returns>是否清空成功</returns>
        public bool ClearTrialData(CT_Counter CT, CapitalPersonalization cap, out string mess)
        {
            try
            {
                using (AccountAndCapitalManagementClient client = GetClient(CT))
                {
                    //string mess;
                    return client.AdminClearTrialData(out mess, cap);
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-8113";
                string errMsg = "调用清算柜台提供的AdminClearTrialData()方法异常," + ex.Message;
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                throw vte;
            }
        }

        /// <summary>
        /// 个性化资金
        /// </summary>
        /// <param name="CT">柜台信息</param>
        /// <param name="cap">个性化资金实体类</param>
        /// <param name="mess">返回的错误信息</param>
        /// <returns>是否个性化成功</returns>
        public bool PersonalizationCapital(CT_Counter CT, CapitalPersonalization cap, out string mess)
        {
            try
            {
                using (AccountAndCapitalManagementClient client = GetClient(CT))
                {
                    //string mess;
                    return client.AdminPersonalizationCapital(out mess, cap);
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-8114";
                string errMsg = "调用清算柜台提供的AdminPersonalizationCapital()方法异常," + ex.Message;
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                throw vte;
            }
        }

        /// <summary>
        /// 手动提交清算
        /// </summary>
        /// <param name="T">柜台信息</param>
        /// <param name="listInfo">结算价列表</param>
        /// <param name="errorMsg">异常信息</param>
        /// <returns></returns>
        public bool DoManualReckoning(CT_Counter T, List<QH_TodaySettlementPriceInfo> listInfo, out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                LogHelper.WriteDebug("=====调用清算柜台提供的FaultRecoveryReckoning()方法进行手动提交清算，柜台：" + T.CouterID);
                using (AccountAndCapitalManagementClient client = GetClient(T))
                {
                    return client.FaultRecoveryReckoning(out errorMsg, listInfo);
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-8115";
                string errMsg = "调用清算柜台提供的FaultRecoveryReckoning()方法异常," + ex.Message;
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                //throw vte;
            }
            return false;
        }

        /// <summary>
        /// 获取当前所有持仓中要提供当日结算价清算的代码
        /// </summary>
        /// <param name="T">柜台信息</param>
        /// <param name="errMsg">输出信息</param>
        /// <returns></returns>
        public List<QH_TodaySettlementPriceInfo> GetReckoningHoldCodeList(CT_Counter T, out string errMsg)
        {
            try
            {
                LogHelper.WriteDebug("=====获取当前所有持仓中要提供当日结算价清算的代码，柜台：" + T.CouterID);
                using (AccountAndCapitalManagementClient client = GetClient(T))
                {
                    return client.GetAllReckoningHoldCode(out errMsg);
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-8116";
                errMsg = "调用清算柜台提供的GetAllReckoningHoldCode()方法异常," + ex.Message;
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                //throw vte;
            }
            return null;
        }

        /// <summary>
        /// 查看指定的时间柜台是否完成清算
        /// </summary>
        /// <param name="T">柜台信息</param>
        /// <param name="doneDate">指定日期时间</param>
        /// <returns></returns>
        public bool IsReckoningDone(CT_Counter T, DateTime doneDate)
        {
            try
            {
                LogHelper.WriteDebug("=====查看指定的时间柜台是否完成清算，柜台：" + T.CouterID);
                using (AccountAndCapitalManagementClient client = GetClient(T))
                {
                    return client.IsReckoningDone(doneDate);
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-8116";
                string errMsg = "调用清算柜台提供的IsReckoningDone()方法异常," + ex.Message;
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                //throw vte;
                return false;
            }
        }
    }
}