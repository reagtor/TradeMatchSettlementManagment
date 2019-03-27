using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using GTA.VTS.Common.CommonUtility;
using ManagementCenter.BLL.ServiceIn;
using ManagementCenter.DAL.AccountManageService;
using ManagementCenter.Model;
using ManagementCenter.DAL;
using ManagementCenter.Model.CommonClass;
using Microsoft.Practices.EnterpriseLibrary.Data;
using GTA.VTS.Common.CommonObject;
using ManagementCenter.Model.UserManage;
using Types = ManagementCenter.Model.CommonClass.Types;


namespace ManagementCenter.BLL.UserManage
{
    /// <summary>
    /// 描述: 提供给前台的交易员信息管理类
    /// 作者：程序员：熊晓凌  
    /// 日期：2008-11-20     
    /// 修改：叶振东 
    /// 修改日期：2009-12-23
    /// 修改：李健华
    /// 修改日期：2009-12-28
    /// Desc.:把放在供开服务的接口的相关私有方法移进本类中实现
    /// 
    /// 修改：刘书伟
    /// 修改日期：2010-01-07
    /// 描述:添加提供给金融平台的开户方法和转账方法
    /// </summary>
    public class Out_TransactionManage
    {
        #region 批量开户

        /// <summary>
        /// 添加交易员
        /// </summary>
        /// <param name="Number">开户个数</param>
        /// <param name="initFund">初始化资金</param>
        /// <param name="MLoginName">登陆名称</param>
        /// <param name="ManagerPWd">登陆密码</param>
        /// <param name="message">输出消息</param>
        /// <returns></returns>
        public List<UM_UserInfo> BatchAddTransaction(int Number, InitFund initFund, string MLoginName,
                                                     string ManagerPWd, out string message)
        {
            message = string.Empty;
            if (ManagerLoginConfirm(MLoginName, ManagerPWd) == null)
            {
                message = "GL-0029:管理员身份验证失败！";
                LogHelper.WriteDebug(message);
                return null;
            }
            if (Number < 1 || Number > 100)
            {
                message = "GL-0000:开户允许的数量范围为1～100";
                LogHelper.WriteDebug(message);
                return null;
            }
            List<UM_UserInfo> AllListUM_UserInfo = new List<UM_UserInfo>();
            List<AccountEntity> AllListAccountEntity = new List<AccountEntity>();
            int CounterID;
            try
            {
                CounterID = GetCounterID();
                if (CounterID == int.MaxValue)
                {
                    message = "GL-0001:当前没有可用柜台分配交易员";
                    LogHelper.WriteDebug(message);
                    return null;
                }
                //获取品种列表
                DataSet ds = StaticDalClass.BreedClassDAL.GetList(string.Empty);
                if (ds == null || ds.Tables[0].Rows.Count < 1)
                {
                    message = "GL-0009:获取品种列表失败";
                    LogHelper.WriteDebug(message);
                    return null;
                }
                UM_UserInfo UM_UserInfo = new UM_UserInfo();

                UM_UserInfo.CouterID = CounterID;
                UM_UserInfo.RoleID = (int)ManagementCenter.Model.CommonClass.Types.RoleTypeEnum.Transaction;
                UM_UserInfo.AddType = (int)ManagementCenter.Model.CommonClass.Types.AddTpyeEnum.FrontTaransaction;

                Database db = DatabaseFactory.CreateDatabase();
                DbConnection Conn = db.CreateConnection();
                if (Conn.State != ConnectionState.Open) Conn.Open();
                DbTransaction Tran = Conn.BeginTransaction();
                try
                {
                    for (int i = 1; i <= Number; i++)
                    {
                        List<AccountEntity> ListAccountEntity;
                        int UserInfoID;
                        //UM_UserInfo.CouterID = CounterID;
                        UM_UserInfo.Password = GetInitPassWord();
                        if (!AddTransaction(ds.Tables[0], UM_UserInfo, initFund, out ListAccountEntity, out UserInfoID,
                                            db,
                                            Tran))
                        {
                            message = "GL-0002:添加交易员失败！";
                            LogHelper.WriteDebug(message);
                            //写调试信息
                            return null;
                        }
                        UM_UserInfo.UserID = UserInfoID;
                        UM_UserInfo.Password = Model.CommonClass.UtilityClass.DesEncrypt(UM_UserInfo.Password, string.Empty);
                        UM_UserInfo UM_UserInfoCopy = new UM_UserInfo();
                        UtilityClass.CopyEntityToEntity(UM_UserInfo, UM_UserInfoCopy);
                        AllListUM_UserInfo.Add(UM_UserInfoCopy);
                        AllListAccountEntity.AddRange(ListAccountEntity);
                    }
                    Tran.Commit();
                }
                catch (Exception Ex)
                {
                    Tran.Rollback();
                    string errCode = "GL-0003";
                    string errMsg = "交易员帐号开设失败！";
                    VTException vte = new VTException(errCode, errMsg, Ex);
                    LogHelper.WriteError(vte.ToString(), vte.InnerException);
                    message = vte.ToString();
                    return null;
                }
                finally
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                    }
                }
            }
            catch (Exception Ex)
            {
                string errCode = "GL-0004";
                string errMsg = "开户初始化信息失败！";
                VTException vte = new VTException(errCode, errMsg, Ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                message = vte.ToString();
                return null;
            }

            //调用柜台开户接口
            try
            {
                CT_Counter T = StaticDalClass.CounterDAL.GetModel(CounterID);
                if (Number == 1)
                {
                    if (!ServiceIn.AccountManageServiceProxy.GetInstance().SingleTraderOpenAccount(T,
                                                                                                   AllListAccountEntity, out message))
                    {
                        message = "GL-0005:调用柜台开户方法SingleTraderOpenAccount()失败!" + message; //写调试信息
                        DelectTransactionLocal(AllListUM_UserInfo);
                        LogHelper.WriteDebug(message);
                        return null;
                    }
                }
                else
                {
                    if (!ServiceIn.AccountManageServiceProxy.GetInstance().VolumeTraderOpenAccount(T,
                                                                                                   AllListAccountEntity, out message))
                    {
                        message = "GL-0006:调用柜台开户方法VolumeTraderOpenAccount()失败!" + message; //写调试信息
                        DelectTransactionLocal(AllListUM_UserInfo);
                        LogHelper.WriteDebug(message);
                        return null;
                    }
                }
            }
            catch (VTException ex)
            {
                DelectTransactionLocal(AllListUM_UserInfo);
                message = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex.InnerException);
                return null;
            }
            //message = "GL-0007:开户成功！";
            return AllListUM_UserInfo;
        }

        #endregion

        #region 单个交易员处理

        /// <summary>
        /// 添加交易员
        /// </summary>
        /// <param name="dt">品种列表</param>
        /// <param name="UserInfo">用户基本信息</param>
        /// <param name="initfund">初始化资金</param>
        /// <param name="ListAccountEntity">帐户列表实体</param>
        /// <param name="UserInfoID">用户ID</param>
        /// <param name="db">Database</param>
        /// <param name="Tran">Transaction</param>
        /// <returns></returns>
        public bool AddTransaction(DataTable dt, UM_UserInfo UserInfo, InitFund initfund,
                                   out List<AccountEntity> ListAccountEntity, out int UserInfoID, Database db,
                                   DbTransaction Tran)
        {
            ListAccountEntity = new List<AccountEntity>();
            UserInfoID = 0;
            try
            {
                string BackAccount = string.Empty;
                //添加基本信息
                UserInfoID = StaticDalClass.UserInfoDAL.Add(UserInfo);
                if (UserInfoID < 1)
                {
                    string mess = "GL-0008:添加用户基本信息失败"; //写调试信息
                    LogHelper.WriteDebug(mess);
                    return false;
                }
                //添加品种权限
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    UM_DealerTradeBreedClass DealerTradeBreedClass = new UM_DealerTradeBreedClass();
                    DealerTradeBreedClass.UserID = UserInfoID;
                    DealerTradeBreedClass.BreedClassID = int.Parse(dt.Rows[i]["BreedClassID"].ToString());
                    StaticDalClass.DealerTradeBreedClassDAL.Add(DealerTradeBreedClass);
                }

                //分配帐号列表
                List<UM_AccountType> AccountType = StaticDalClass.AccountTypeDAL.GetListArray(string.Empty);
                if (AccountType == null || AccountType.Count == 0)
                {
                    string mess = "GL-0010:获取帐号类型列表失败"; //写调试信息
                    LogHelper.WriteDebug(mess);
                    return false;
                }
                //传加密后的密码给柜台开户
                string DesPassWord = UtilityClass.DesEncrypt(UserInfo.Password, string.Empty);

                foreach (UM_AccountType type in AccountType)
                {
                    UM_DealerAccount DealerAccount = new UM_DealerAccount();
                    DealerAccount.AccountTypeID = type.AccountTypeID;
                    DealerAccount.DealerAccoutID = ProductionAccount.FormationAccount((int)UserInfo.CouterID,
                                                                                      UserInfoID,
                                                                                      (int)
                                                                                      DealerAccount.AccountTypeID);
                    DealerAccount.IsDo = true;
                    DealerAccount.AccountAttributionType = type.AccountAttributionType;
                    DealerAccount.UserID = UserInfoID;
                    StaticDalClass.DealerAccountDAL.Add(DealerAccount, Tran, db);

                    //添加到调用柜台接口的参数类表
                    {
                        AccountEntity Element = new AccountEntity();
                        Element.Account = DealerAccount.DealerAccoutID;
                        Element.AccountAAttribution = (int)DealerAccount.AccountAttributionType;
                        Element.AccountType = (int)DealerAccount.AccountTypeID;
                        Element.CurrencyHK = initfund.HK;
                        Element.CurrencyRMB = initfund.RMB;
                        Element.CurrencyUS = initfund.US;
                        Element.RoleNumber = (int)GTA.VTS.Common.CommonObject.Types.UserId.Trader;
                        Element.TraderID = UserInfoID.ToString();
                        Element.TraderPassWord = DesPassWord;
                        ListAccountEntity.Add(Element);
                    }
                    if (type.AccountAttributionType ==
                        (int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.BankAccount)
                    {
                        BackAccount = DealerAccount.DealerAccoutID;
                    }
                }
                if (!SaveFund(initfund, Tran, db, BackAccount))
                {
                    string mess = "GL-0011:添加用户初始资金失败"; //写调试信息
                    LogHelper.WriteDebug(mess);
                    return false;
                }
            }
            catch (Exception Ex)
            {
                string errCode = "GL-0013";
                string errMsg = "添加交易员失败！";
                VTException vte = new VTException(errCode, errMsg, Ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return false;
            }
            return true;
        }

        #endregion

        #region 保存初始资金

        /// <summary>
        /// 保存初始资金
        /// </summary>
        /// <param name="initfund">初始化资金</param>
        /// <param name="Tran">Transaction</param>
        /// <param name="db">Database</param>
        /// <param name="BackAccount">银行帐号</param>
        /// <returns></returns>
        private bool SaveFund(InitFund initfund, DbTransaction Tran, Database db, string BackAccount)
        {
            try
            {
                if (initfund.HK != decimal.MaxValue)
                {
                    UM_OriginationFund OriginationFund = new UM_OriginationFund();
                    OriginationFund.DealerAccoutID = BackAccount;
                    OriginationFund.FundMoney = (decimal)initfund.HK;
                    OriginationFund.TransactionCurrencyTypeID = (int)GTA.VTS.Common.CommonObject.Types.CurrencyType.HK;
                    StaticDalClass.OriginationFundDAL.Add(OriginationFund, Tran, db);
                }
                if (initfund.RMB != decimal.MaxValue)
                {
                    UM_OriginationFund OriginationFund = new UM_OriginationFund();
                    OriginationFund.DealerAccoutID = BackAccount;
                    OriginationFund.FundMoney = (decimal)initfund.RMB;
                    OriginationFund.TransactionCurrencyTypeID = (int)GTA.VTS.Common.CommonObject.Types.CurrencyType.RMB;
                    StaticDalClass.OriginationFundDAL.Add(OriginationFund, Tran, db);
                }
                if (initfund.US != decimal.MaxValue)
                {
                    UM_OriginationFund OriginationFund = new UM_OriginationFund();
                    OriginationFund.DealerAccoutID = BackAccount;
                    OriginationFund.FundMoney = (decimal)initfund.US;
                    OriginationFund.TransactionCurrencyTypeID = (int)GTA.VTS.Common.CommonObject.Types.CurrencyType.US;
                    StaticDalClass.OriginationFundDAL.Add(OriginationFund, Tran, db);
                }
            }
            catch (Exception Ex)
            {
                string errCode = "GL-0014";
                string errMsg = "保存初始资金失败！帐号为：" + BackAccount;
                VTException vte = new VTException(errCode, errMsg, Ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return false;
            }
            return true;
        }

        #endregion

        #region 交易员验证
        /// <summary>
        /// 交易员验证
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="password">密码</param>
        /// <param name="mess">用户信息</param>
        /// <returns></returns>
        public CT_Counter LoginConfirmation(int id, string password, out string mess)
        {
            mess = string.Empty;
            try
            {
                UM_UserInfo UserInfo = StaticDalClass.UserInfoDAL.TranLogin(id, password);

                if (UserInfo == null)
                {
                    mess = "GL-0015:交易员验证失败！";
                    LogHelper.WriteDebug(mess);
                    return null;
                }

                CT_Counter Counter = StaticDalClass.CounterDAL.GetModel((int)UserInfo.CouterID);

                if (Counter == null)
                {
                    mess = "GL-0016:根据交易员找对应柜台失败！";
                    LogHelper.WriteDebug(mess);
                    return null;
                }
                //mess = "登陆成功！";
                return Counter;
            }
            catch (Exception ex)
            {
                string errCode = "GL-0017";
                string errMsg = "交易员验证失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                mess = vte.ToString();
                return null;
            }
        }
        #endregion

        #region 得到分配的柜台ID
        /// <summary>
        /// 得到柜台ID
        /// </summary>
        /// <returns></returns>
        private int GetCounterID()
        {
            float i = 1;
            int counterID = int.MaxValue;
            float Percent;
            try
            {
                List<CT_Counter> listcounter = StaticDalClass.CounterDAL.GetListArray(string.Empty);
                foreach (CT_Counter Counter in listcounter)
                {

                    DataSet ds = StaticDalClass.UserInfoDAL.GetList("CouterID=" + Counter.CouterID.ToString());
                    if (ds == null)
                    {
                        continue;
                    }
                    Percent = (float)((float)ds.Tables[0].Rows.Count / Counter.MaxValues);

                    if (Percent < i) //&& Counter.State == (int)ManagementCenter.Model.CommonClass.Types.StateEnum.ConnSuccess)
                    {
                        i = Percent;
                        counterID = Counter.CouterID;
                    }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-0018";
                string errMsg = "分配柜台失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
            }
            return counterID;
        }

        #endregion

        #region 得到随机密码

        /// <summary>
        /// 得到随机密码
        /// </summary>
        /// <returns></returns>
        private string GetInitPassWord()
        {
            Random random = new Random();
            int var = random.Next(0, 999999);
            return var.ToString();
        }

        #endregion

        #region 删除交易员

        /// <summary>
        ///  删除交易员
        /// </summary>
        /// <param name="listuserInfo">交易员实体</param>
        /// <param name="MLoginName">管理员登陆名称</param>
        /// <param name="ManagerPWd">管理员密码</param>
        /// <param name="message">返回的信息</param>
        /// <returns></returns>
        public bool BatchDelectTransaction(List<UM_UserInfo> listuserInfo, string MLoginName, string ManagerPWd,
                                           out string message)
        {
            message = string.Empty;
            if (ManagerLoginConfirm(MLoginName, ManagerPWd) == null)
            {
                message = "GL-0030:管理员身份验证失败！";
                LogHelper.WriteDebug(message);
                return false;
            }
            //更新用户状态
            if (!UpdateTranState(listuserInfo))
            {
                message = "GL-0019:更新交易员删除状态失败";
                LogHelper.WriteDebug(message);
                return false;
            }
            //调用柜台销户方法
            foreach (UM_UserInfo User in listuserInfo)
            {
                CT_Counter T = GetCounterByUserID(User.UserID);
                try
                {
                    if (!AccountManageServiceProxy.GetInstance().DeleteSingleTraderAccount(T, User.UserID.ToString(), out message))
                    {
                        message = "GL-0020:调用柜台柜台销户方法DeleteSingleTraderAccount失败,用户ID:" + User.UserID.ToString();
                        LogHelper.WriteDebug(message);
                        //写调试信息
                        continue;
                    }
                    //删除本地数据
                    DelectTransactionLocal(User.UserID);
                }
                catch (VTException Ex)
                {
                    //写日志
                    LogHelper.WriteError(Ex.ToString(), Ex.InnerException);
                    message = Ex.ToString();
                }
            }
            return true;
        }

        #endregion

        #region 本地数据删除
        /// <summary>
        /// 本地数据删除
        /// </summary>
        /// <param name="listuserInfo">用户信息列表</param>
        /// <returns></returns>
        private bool DelectTransactionLocal(List<UM_UserInfo> listuserInfo)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbConnection Conn = db.CreateConnection();
                if (Conn.State != ConnectionState.Open) Conn.Open();
                DbTransaction Tran = Conn.BeginTransaction();
                try
                {
                    foreach (UM_UserInfo info in listuserInfo)
                    {
                        int UserID = info.UserID;

                        StaticDalClass.ThawReasonDAL.DeleteByUserID(UserID, Tran, db);

                        StaticDalClass.FreezeReasonDAL.DeleteByUserID(UserID, Tran, db);

                        StaticDalClass.OriginationFundDAL.DeleteByUserID(UserID, Tran, db);

                        StaticDalClass.DealerAccountDAL.DeleteByUserID(UserID, Tran, db);

                        StaticDalClass.DealerTradeBreedClassDAL.DeleteByUserID(UserID, Tran, db);

                        StaticDalClass.UserInfoDAL.Delete(UserID, Tran, db);
                    }
                    Tran.Commit();
                }
                catch
                {
                    Tran.Rollback();
                    return false;
                }
                finally
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-0021";
                string errMsg = "本地用户数据删除失败！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 根据ID删除交易员
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        private bool DelectTransactionLocal(int UserID)
        {
            List<UM_UserInfo> L = new List<UM_UserInfo>();
            UM_UserInfo UserInfo = new UM_UserInfo();
            UserInfo.UserID = UserID;
            L.Add(UserInfo);
            return DelectTransactionLocal(L);
        }

        #endregion

        #region 更新用户状态
        /// <summary>
        /// 更新用户状态
        /// </summary>
        /// <param name="listuserInfo">用户信息列表</param>
        /// <returns></returns>
        public bool UpdateTranState(List<UM_UserInfo> listuserInfo)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbConnection Conn = db.CreateConnection();
                if (Conn.State != ConnectionState.Open) Conn.Open();
                DbTransaction Tran = Conn.BeginTransaction();
                try
                {
                    foreach (UM_UserInfo info in listuserInfo)
                    {
                        StaticDalClass.UserInfoDAL.UpdateDelState(info.UserID, Tran, db);
                    }
                    Tran.Commit();
                }
                catch (Exception ex)
                {
                    Tran.Rollback();
                    string errCode = "GL-0022";
                    string errMsg = "本地用户状态更新失败！";
                    VTException vte = new VTException(errCode, errMsg, ex);
                    LogHelper.WriteError(vte.ToString(), vte.InnerException);
                    return false;
                }
                finally
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-0023";
                string errMsg = "数据库连接失败！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                //写错误日志
                return false;
            }
            return true;
        }

        #endregion

        #region 获取柜台实体

        /// <summary>
        /// 根据用户ID得到柜台实体
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        private CT_Counter GetCounterByUserID(int UserID)
        {
            try
            {
                UM_UserInfo USER = StaticDalClass.UserInfoDAL.GetModel(UserID);
                CT_Counter CT = StaticDalClass.CounterDAL.GetModel((int)USER.CouterID);
                return CT;
            }
            catch (Exception ex)
            {
                string errCode = "GL-0024";
                string errMsg = "根据用户ID得到柜台实体GetCounterByUserID()方法异常！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据帐号得到柜台实体
        /// </summary>
        /// <param name="DealerAccoutID">帐号</param>
        /// <returns></returns>
        private CT_Counter GetCounterByAccountID(string DealerAccoutID)
        {
            try
            {
                UM_DealerAccount DealerAccount = StaticDalClass.DealerAccountDAL.GetModel(DealerAccoutID);
                return GetCounterByUserID(DealerAccount.UserID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-0025";
                string errMsg = "根据帐号得到柜台实体GetCounterByAccountID()方法异常！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        #endregion

        #region 定时启动服务删除没有删除成功的交易员
        /// <summary>
        /// 定时启动服务删除没有删除成功的交易员
        /// </summary>
        public void DelTran()
        {
            try
            {
                List<UM_UserInfo> listuserInfo = StaticDalClass.UserInfoDAL.GetListArray(string.Format(" AddType={0}",
                                                                          (int)ManagementCenter.Model.CommonClass.Types.
                                                                              AddTpyeEnum.FrontTarnDelState));

                string mess;

                if (listuserInfo == null || listuserInfo.Count < 1) return;
                foreach (UM_UserInfo User in listuserInfo)
                {
                    CT_Counter T = GetCounterByUserID(User.UserID);
                    if (T == null)
                    {
                        mess = "GL-0026:获取柜台失败";
                    }
                    try
                    {
                        string s;
                        if (!AccountManageServiceProxy.GetInstance().DeleteSingleTraderAccount(T, User.UserID.ToString(), out s))
                        {
                            mess = "GL-0027:调用柜台柜台销户方法DeleteSingleTraderAccount失败,用户ID:" + User.UserID.ToString();
                            LogHelper.WriteDebug(mess);
                            //写调试信息
                            continue;
                        }
                        else
                        {
                            //删除本地数据
                            DelectTransactionLocal(User.UserID);
                        }
                    }
                    catch (VTException Ex)
                    {
                        //写日志
                        mess = Ex.ToString();
                        LogHelper.WriteError(Ex.ToString(), Ex.InnerException);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message.ToString(), ex);
            }

        }
        #endregion

        #region 管理员验证

        /// <summary>
        /// 管理员验证
        /// </summary>
        /// <param name="LoginName">登陆名称</param>
        /// <param name="Password">登陆密码</param>
        /// <returns></returns>
        public UM_UserInfo ManagerLoginConfirm(string LoginName, string Password)
        {
            try
            {
                UM_UserInfo UserInfo = StaticDalClass.UserInfoDAL.ManagerLoginConfirm(LoginName, Password,
                                        (int)ManagementCenter.Model.CommonClass.Types.AddTpyeEnum.FrontManager);
                return UserInfo;
            }
            catch (Exception ex)
            {
                string errCode = "GL-0028";
                string errMsg = "前台接口的管理员帐号验证失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        #endregion

        #region 对管理员用户名和密码进行判断
        /// <summary>
        /// 对管理员用户名和密码进行判断
        /// </summary>
        /// <param name="LoginName">管理员用户名</param>
        /// <param name="PassWord">密码</param>
        /// <param name="message">返回错误信息</param>
        /// <returns></returns>
        public bool AdminLoginConfirmation(string LoginName, string PassWord, out string message)
        {
            message = string.Empty;
            try
            {
                UM_UserInfo UserInfo = StaticDalClass.UserInfoDAL.AdminLogin(LoginName, PassWord);

                if (UserInfo == null)
                {
                    message = "GL-0031:管理员验证失败！";
                    LogHelper.WriteDebug(message);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                string errCode = "GL-0032";
                string errMsg = "管理员验证失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                message = vte.ToString();
                return false;
            }
        }
        #endregion

        #region 查询交易员
        /// <summary>
        /// 查询交易员
        /// </summary>
        /// <param name="message">返回错误信息</param>
        /// <returns>获取交易员</returns>
        public List<ManagementCenter.Model.UM_UserInfo> GetAllUser(out string message)
        {
            message = string.Empty;
            try
            {
                List<UM_UserInfo> list = StaticDalClass.UserInfoDAL.GetAllUser();

                if (list == null)
                {
                    message = "GL-0032:查询交易员失败！";
                    LogHelper.WriteDebug(message);
                    return null;
                }
                return list;
            }
            catch (Exception ex)
            {
                string errCode = "GL-0033";
                string errMsg = "查询交易员失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                message = vte.ToString();
                return null;
            }
        }
        #endregion 查询交易员

        #region 对交易员列表进行所属柜台进行分组
        /// <summary>
        /// 对交易员列表进行所属柜台进行分组
        /// </summary>
        /// <param name="pca">个性化资金</param>
        /// <returns>分组后以柜台组合后的列表</returns>
        public Dictionary<string, CapitalPersonalization> CounterGrouping(PersonalizationCapital pca)
        {
            //PersonalizationCapital personalization = new PersonalizationCapital();
            Dictionary<string, CapitalPersonalization> list = new Dictionary<string, CapitalPersonalization>();
            //将获取的柜台信息进行分组(通过交易员ID查询出对应柜台。通过判断柜台是否存在，如果存在则将交易员ID添加到已经存在的数据中，否则创建出一条新的数据)
            for (int i = 0; i < pca.TradeID.Count; i++)
            {
                int UserId = int.Parse(pca.TradeID[i]);
                UM_UserInfo user = StaticDalClass.UserInfoDAL.GetModel(UserId);
                string CounterID = user.CouterID.ToString();
                if (list.ContainsKey(CounterID))
                {
                    list[CounterID].TradeID.Add(pca.TradeID[i]);
                }
                else
                {
                    CapitalPersonalization cap = new CapitalPersonalization();
                    cap.TradeID = new List<string>();
                    cap.USAmount = pca.USAmount;
                    cap.RMBAmount = pca.RMBAmount;
                    cap.HKAmount = pca.HKAmount;
                    cap.PersonalType = pca.PersonalType;
                    cap.SetCurrencyType = pca.SetCurrencyType;
                    cap.TradeID.Add(pca.TradeID[i]);
                    list.Add(CounterID, cap);
                }
            }
            return list;
        }
        #endregion

        //===============================================提供给金融工程平台的方法=====================================

        #region 金融工程平台管理员验证

        /// <summary>
        /// 金融工程平台管理员验证
        /// </summary>
        /// <param name="LoginName">登陆名称</param>
        /// <param name="Password">登陆密码</param>
        /// <returns></returns>
        public UM_UserInfo ManagerLoginConfirmFP(string LoginName, string Password)
        {
            try
            {
                UM_UserInfo UserInfo = StaticDalClass.UserInfoDAL.ManagerLoginConfirm(LoginName, Password,
                                         (int)ManagementCenter.Model.CommonClass.Types.AddTpyeEnum.BackManager);
                return UserInfo;
            }
            catch (Exception ex)
            {
                string errCode = "GL-0034";
                string errMsg = "提供给金融工程平台接口的管理员帐号验证失败!";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        #endregion

        #region 根据用户ID返回用户信息
        /// <summary>
        ///根据用户ID返回用户信息
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        public UM_UserInfo GetUMUserInfoModel(int UserID)
        {
            try
            {
                UM_UserInfoDAL uM_UserInfoDAL = new UM_UserInfoDAL();
                return uM_UserInfoDAL.GetModel(UserID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-0036";
                string errMsg = "根据用户ID返回用户信息失败!";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }
        #endregion

        #region 删除交易员根据ID

        /// <summary>
        /// 删除交易员根据ID
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="mess"></param>
        /// <returns></returns>
        public bool DelTransaction(int UserID, out string mess)
        {
            //调用柜台销户方法
            try
            {
                CT_Counter T = GetCounterByUserID(UserID);
                if (T == null)
                {
                    mess = "GL-0037:交易员获取相应柜台失败"; //写调试信息
                    LogHelper.WriteDebug(mess);
                    return false;
                }
                if (!AccountManageServiceProxy.GetInstance().DeleteSingleTraderAccount(T, UserID.ToString(), out mess))
                {
                    mess = "GL-0038:调用柜台柜台销户方法DeleteSingleTraderAccount失败!" + mess; //写调试信息
                    LogHelper.WriteDebug(mess);
                    return false;
                }
            }
            catch (VTException Ex)
            {
                mess = Ex.ToString();
                //写错误日志
                return false;
            }
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbConnection Conn = db.CreateConnection();
                if (Conn.State != ConnectionState.Open) Conn.Open();
                DbTransaction Tran = Conn.BeginTransaction();
                try
                {
                    StaticDalClass.ThawReasonDAL.DeleteByUserID(UserID, Tran, db);

                    StaticDalClass.FreezeReasonDAL.DeleteByUserID(UserID, Tran, db);

                    StaticDalClass.OriginationFundDAL.DeleteByUserID(UserID, Tran, db);

                    StaticDalClass.DealerAccountDAL.DeleteByUserID(UserID, Tran, db);

                    StaticDalClass.DealerTradeBreedClassDAL.DeleteByUserID(UserID, Tran, db);

                    StaticDalClass.UserInfoDAL.Delete(UserID, Tran, db);
                    Tran.Commit();
                    mess = "GL-0039:交易员删除成功！";
                    return true;
                }
                catch (Exception ex)
                {
                    Tran.Rollback();
                    string errCode = "GL-0040";
                    string errMsg = "删除交易员方法失败！";
                    VTException vte = new VTException(errCode, errMsg, ex);
                    LogHelper.WriteError(vte.ToString(), vte.InnerException);
                    mess = vte.ToString();
                    return false;
                }
                finally
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-0041";
                string errMsg = "连接数据库失败！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                mess = vte.ToString();
                return false;
            }
        }

        #endregion

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
        public UM_UserInfo AddTransactionFP(string MLoginName, string ManagerPWd, UM_UserInfo UserInfo, InitFund initfund, out List<AccountEntity> l_AccountEntity, out string message)
        {
            message = string.Empty;
            l_AccountEntity = new List<AccountEntity>();
            if (ManagerLoginConfirmFP(MLoginName, ManagerPWd) == null)
            {
                message = "GL-0042:管理员身份验证失败！";
                LogHelper.WriteDebug(message);
                return null;
            }
            int CounterID;
            try
            {
                CounterID = GetCounterID();
                if (CounterID == int.MaxValue)
                {
                    message = "GL-0043:当前没有可用柜台分配交易员";
                    LogHelper.WriteDebug(message);
                    return null;
                }
                //获取品种列表
                DataSet ds = StaticDalClass.BreedClassDAL.GetList(string.Empty);
                if (ds == null || ds.Tables[0].Rows.Count < 1)
                {
                    message = "GL-0044:获取品种列表失败";
                    LogHelper.WriteDebug(message);
                    return null;
                }

                //List<AccountEntity> l_AccountEntity = new List<AccountEntity>();

                string BackAccount = string.Empty;
                int UserID;

                Database db = DatabaseFactory.CreateDatabase();
                DbConnection Conn = db.CreateConnection();
                if (Conn.State != ConnectionState.Open) Conn.Open();
                DbTransaction Tran = Conn.BeginTransaction();

                try
                {
                    UserInfo.CouterID = CounterID;
                    UserInfo.RoleID = (int)ManagementCenter.Model.CommonClass.Types.RoleTypeEnum.Transaction;
                    UserInfo.AddType = (int)ManagementCenter.Model.CommonClass.Types.AddTpyeEnum.BackTaransaction;

                    //添加用户基本信息表
                    UserID = StaticDalClass.UserInfoDAL.Add(UserInfo, Tran, db);
                    if (UserID < 1)
                    {
                        message = "GL-0045:添加用户基本信息失败"; //写调试信息
                        LogHelper.WriteDebug(message);
                        return null;
                    }
                    //添加用户交易品种权限表
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        UM_DealerTradeBreedClass DealerTradeBreedClass = new UM_DealerTradeBreedClass();
                        DealerTradeBreedClass.UserID = UserID;
                        DealerTradeBreedClass.BreedClassID = int.Parse(ds.Tables[0].Rows[i]["BreedClassID"].ToString());
                        StaticDalClass.DealerTradeBreedClassDAL.Add(DealerTradeBreedClass, Tran, db);
                    }
                    //分配帐号列表
                    List<UM_AccountType> AccountType = StaticDalClass.AccountTypeDAL.GetListArray(string.Empty);
                    if (AccountType == null || AccountType.Count == 0)
                    {
                        message = "GL-0046:获取帐号类型列表失败"; //写调试信息
                        LogHelper.WriteDebug(message);
                        return null;
                    }
                    //传加密后的密码给柜台开户
                    string DesPassWord = UtilityClass.DesEncrypt(UserInfo.Password, string.Empty);

                    foreach (UM_AccountType type in AccountType)
                    {
                        UM_DealerAccount DealerAccount = new UM_DealerAccount();
                        DealerAccount.AccountTypeID = type.AccountTypeID;
                        DealerAccount.DealerAccoutID = ProductionAccount.FormationAccount((int)UserInfo.CouterID,
                                                                                          UserID,
                                                                                          (int)
                                                                                          DealerAccount.AccountTypeID);
                        DealerAccount.IsDo = true;
                        DealerAccount.AccountAttributionType = type.AccountAttributionType;
                        DealerAccount.UserID = UserID;
                        StaticDalClass.DealerAccountDAL.Add(DealerAccount, Tran, db);

                        //添加到调用柜台接口的参数类表
                        {
                            AccountEntity Element = new AccountEntity();
                            Element.Account = DealerAccount.DealerAccoutID;
                            Element.AccountAAttribution = (int)DealerAccount.AccountAttributionType;
                            Element.AccountType = (int)DealerAccount.AccountTypeID;
                            Element.CurrencyHK = initfund.HK;
                            Element.CurrencyRMB = initfund.RMB;
                            Element.CurrencyUS = initfund.US;
                            Element.RoleNumber = (int)GTA.VTS.Common.CommonObject.Types.UserId.Trader;
                            Element.TraderID = UserID.ToString();
                            Element.TraderPassWord = DesPassWord;
                            l_AccountEntity.Add(Element);
                        }
                        if (type.AccountAttributionType ==
                            (int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.BankAccount)
                        {
                            BackAccount = DealerAccount.DealerAccoutID;
                        }
                    }
                    //保存初始资金
                    if (!SaveFund(initfund, Tran, db, BackAccount))
                    {
                        message = "GL-0047：初始化资金失败！"; //写调试信息
                        LogHelper.WriteDebug(message);
                        return null;
                    }
                    Tran.Commit();
                }
                catch (Exception ex)
                {
                    Tran.Rollback();
                    string errCode = "GL-0048";
                    string errMsg = "添加交易员失败！";
                    VTException vte = new VTException(errCode, errMsg, ex);
                    LogHelper.WriteError(vte.ToString(), vte.InnerException);
                    message = vte.ToString();
                    return null;
                }
                finally
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                    }
                }

                //调用柜台开户方法
                string s;
                try
                {
                    CT_Counter T = StaticDalClass.CounterDAL.GetModel((int)UserInfo.CouterID);

                    if (
                        !ServiceIn.AccountManageServiceProxy.GetInstance().SingleTraderOpenAccount(T, l_AccountEntity, out s))
                    {
                        message = "GL-0049:调用柜台开户方法SingleTraderOpenAccount失败!" + s; //写调试信息
                        LogHelper.WriteDebug(message);

                        DelTransaction(UserID, out s);
                        return null;
                    }
                }
                catch (VTException ex)
                {
                    message = ex.ToString();
                    LogHelper.WriteError(ex.ToString(), ex.InnerException);
                    DelTransaction(UserID, out s);
                    //写错误信息
                    return null;
                }
                //message = "GL-0050:开户成功！";
                return null;
            }
            catch (Exception Ex)
            {
                string errCode = "GL-0051";
                string errMsg = "数据库连接失败！";
                VTException vte = new VTException(errCode, errMsg, Ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                message = vte.ToString();
                return null;
            }
        }

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
        //public DataTable AdminFindTraderCapitalAccountInfoByIDFP(string adminId, string adminPassword, string traderID, out string strErrorMessage)
        public List<TradersAccountCapitalInfo> AdminFindTraderCapitalAccountInfoByIDFP(string adminId, string adminPassword, string traderID, out string strErrorMessage)
        {
            //调用柜台管理员查询根据交易员查询交易员各资金账户相关信息方法
            List<TradersAccountCapitalInfo> tradersAccountCList = new List<TradersAccountCapitalInfo>();
            try
            {
                int couterID;
                UM_UserInfo uM_UserInfo = new UM_UserInfo();
                uM_UserInfo = GetUMUserInfoModel(Convert.ToInt32(traderID));
                if (uM_UserInfo == null)
                {
                    strErrorMessage = "用户ID不存在!";
                    LogHelper.WriteDebug(strErrorMessage);
                    return null;
                }
                couterID = Convert.ToInt32(uM_UserInfo.CouterID);
                if (string.IsNullOrEmpty(couterID.ToString()))
                {
                    strErrorMessage = "交易员ID（用户ID）的柜台不存在!";
                    LogHelper.WriteDebug(strErrorMessage);
                    return null;
                }
                CT_Counter T = StaticDalClass.CounterDAL.GetModel(couterID);
                tradersAccountCList =
                   ServiceIn.AccountManageServiceProxy.GetInstance().AdminFindTraderCapitalAccountInfoByID(T, adminId,
                                                                                                           adminPassword,
                                                                                                           traderID,
                                                                                                           out
                                                                                                                strErrorMessage);

                if (tradersAccountCList.Count == 0)
                {
                    strErrorMessage = "GL-0052:调用柜台开户方法AdminFindTraderCapitalAccountInfoByID失败!";// +s; //写调试信息
                    LogHelper.WriteDebug(strErrorMessage);

                }
            }
            catch (VTException ex)
            {
                strErrorMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex.InnerException);
                return null;
            }
            return tradersAccountCList;
        }
        #endregion

        #region 自由转帐（同币种）
        /// <summary>
        /// 自由转帐（同币种）
        /// </summary>
        /// <param name="freeTransfer"> 自由转账实体</param>
        /// <param name="currencyType">当前币种类型</param>
        /// <param name="couterID">柜台ID</param>
        /// <param name="outMessage">输出信息</param>
        /// <returns></returns>
        public bool TwoAccountsFreeTransferFundsFP(FreeTransferEntity freeTransfer, TypesCurrencyType currencyType, int couterID,
                                                 out string outMessage)
        {
            try
            {
                CT_Counter T = StaticDalClass.CounterDAL.GetModel(couterID);
                bool _Result =
                    ServiceIn.AccountManageServiceProxy.GetInstance().TwoAccountsFreeTransferFunds(T, freeTransfer,
                                                                                                   currencyType, out outMessage);


                if (_Result == false)
                {
                    if (!string.IsNullOrEmpty(outMessage))
                        LogHelper.WriteDebug(outMessage);
                    return false;
                }
                return true;
            }
            catch (VTException ex)
            {
                outMessage = "GL-0053:调用柜台自由转帐（同币种）方法TwoAccountsFreeTransferFunds失败!";
                LogHelper.WriteError(ex.ToString() + outMessage, ex.InnerException);
                //写错误信息
                return false;
            }
        }
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
        public bool ConvertFreeTransferEntityFP(int traderID, int FromCapitalAccountType, int ToCapitalAccountType, decimal TransferAmount, int currencyType, out string outMessage)
        {
            try
            {
                int couterID;
                UM_UserInfo uM_UserInfo = new UM_UserInfo();
                uM_UserInfo = GetUMUserInfoModel(traderID);
                if (uM_UserInfo == null)
                {
                    outMessage = "用户ID不存在!";
                    LogHelper.WriteDebug(outMessage);
                    return false;
                }
                couterID = Convert.ToInt32(uM_UserInfo.CouterID);
                if (string.IsNullOrEmpty(couterID.ToString()))
                {
                    outMessage = "交易员ID（用户ID）的柜台不存在!";
                    LogHelper.WriteDebug(outMessage);
                    return false;
                }
                FreeTransferEntity freeTransferEntity = new FreeTransferEntity();
                freeTransferEntity.TraderID = traderID.ToString();
                freeTransferEntity.FromCapitalAccountType = FromCapitalAccountType;
                freeTransferEntity.ToCapitalAccountType = ToCapitalAccountType;
                freeTransferEntity.TransferAmount = TransferAmount;
                TypesCurrencyType _currencyType = (TypesCurrencyType)currencyType;
                bool _result = TwoAccountsFreeTransferFundsFP(freeTransferEntity, _currencyType, couterID, out outMessage);
                if (_result)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                outMessage = "调用自由转帐（同币种)方法出错!";
                LogHelper.WriteError(ex.ToString(), ex.InnerException);
                return false;
            }
        }
        #endregion

        #region 追加资金
        /// <summary>
        /// 追加资金
        /// </summary>
        /// <param name="model">追加资金实体</param>
        /// <param name="mess">返回信息</param>
        /// <returns>返回是否成功</returns>
        public bool AddFundFP(ManagementCenter.Model.UM_FundAddInfo model, out string mess)
        {
            mess = string.Empty;
            UM_UserInfo user = StaticDalClass.UserInfoDAL.GetModel((int)model.UserID);
            if (user == null || user.RoleID != (int)ManagementCenter.Model.CommonClass.Types.RoleTypeEnum.Transaction)
            {
                mess = "GL-0054:交易员编号不存在！"; //写调试信息
                LogHelper.WriteDebug(mess);
                return false;
            }

            CT_Counter T = GetCounterByUserID((int)model.UserID);
            if (T == null)
            {
                mess = "GL-0055:交易员对应的柜台不存在！"; //写调试信息
                LogHelper.WriteDebug(mess);
                return false;
            }
            UM_DealerAccount DealerAccount = StaticDalClass.DealerAccountDAL.GetModelByUserIDAndType((int)model.UserID,
                                                                                                     (int)
                                                                                                     GTA.VTS.Common.CommonObject.Types.
                                                                                                         AccountAttributionType
                                                                                                         .
                                                                                                         BankAccount);
            if (DealerAccount == null)
            {
                mess = "GL-0056:交易员对应的银行帐号不存在！"; //写调试信息
                LogHelper.WriteDebug(mess);
                return false;
            }
            try
            {
                AddCapitalEntity AddCapitalEntity = new AddCapitalEntity();
                AddCapitalEntity.AddHKAmount = (decimal)model.HKNumber;
                AddCapitalEntity.AddRMBAmount = (decimal)model.RMBNumber;
                AddCapitalEntity.AddUSAmount = (decimal)model.USNumber;
                AddCapitalEntity.TraderID = model.UserID.ToString();
                AddCapitalEntity.BankCapitalAccount = DealerAccount.DealerAccoutID;
                if (!ServiceIn.AccountManageServiceProxy.GetInstance().AddCapital(T, AddCapitalEntity, out mess))
                {
                    mess = "GL-0057:调用柜台追加资金方法AddCapital()失败!" + mess; //写调试信
                    LogHelper.WriteDebug(mess);
                    return false;
                }
            }
            catch (VTException Ex)
            {
                mess = Ex.ToString();
                //写错误日志
                return false;
            }
            try
            {
                if (model.HKNumber == decimal.MaxValue) model.HKNumber = null;
                if (model.RMBNumber == decimal.MaxValue) model.RMBNumber = null;
                if (model.USNumber == decimal.MaxValue) model.USNumber = null;
                StaticDalClass.FundAddInfoDAL.Add(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-0058";
                string errMsg = "追加资金失败！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                mess = vte.ToString();
                return false;
            }
            return true;
        }

        #endregion

    }
}