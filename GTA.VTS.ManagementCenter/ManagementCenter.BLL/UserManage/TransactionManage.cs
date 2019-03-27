using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data;
using System.Transactions;
using GTA.VTS.Common.CommonUtility;
using ManagementCenter.BLL.ServiceIn;
using ManagementCenter.DAL;
using ManagementCenter.DAL.AccountManageService;
using ManagementCenter.Model;
using ManagementCenter.Model.CommonClass;
using GTA.VTS.Common.CommonObject;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Types = ManagementCenter.Model.CommonClass.Types;


namespace ManagementCenter.BLL.UserManage
{
    /// <summary>
    /// 描述：交易员信息管理类
    /// 错误编码范围:0200-0299
    /// 作者：熊晓凌
    /// 日期：2008-11-20
    /// 修改：刘书伟
    /// 日期：2010-05-06
    /// 描述：添加：管理员查询根据交易员查询交易员银行资金账户相关信息
    /// </summary>
    public class TransactionManage
    {
        #region 添加交易员

        /// <summary>
        /// 添加交易员
        /// </summary>
        /// <param name="dt">权限列表</param>
        /// <param name="UserInfo">交易员实体</param>
        /// <param name="initfund">初始资金实体</param>
        /// <param name="mess"></param>
        /// <returns></returns>
        public bool AddTransaction(DataTable dt, UM_UserInfo UserInfo, InitFund initfund, out string mess)
        {
            try
            {
                List<AccountEntity> l_AccountEntity = new List<AccountEntity>();

                string BackAccount = string.Empty;
                int UserID;

                Database db = DatabaseFactory.CreateDatabase();
                DbConnection Conn = db.CreateConnection();
                if (Conn.State != ConnectionState.Open) Conn.Open();
                DbTransaction Tran = Conn.BeginTransaction();

                try
                {
                    //添加用户基本信息表
                    UserID = StaticDalClass.UserInfoDAL.Add(UserInfo, Tran, db);
                    if (UserID < 1)
                    {
                        mess = "GL-0200:添加用户基本信息失败"; //写调试信息
                        LogHelper.WriteDebug(mess);
                        return false;
                    }
                    //添加用户交易品种权限表
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        UM_DealerTradeBreedClass DealerTradeBreedClass = new UM_DealerTradeBreedClass();
                        DealerTradeBreedClass.UserID = UserID;
                        DealerTradeBreedClass.BreedClassID = int.Parse(dt.Rows[i]["BreedClassID"].ToString());
                        StaticDalClass.DealerTradeBreedClassDAL.Add(DealerTradeBreedClass, Tran, db);
                    }
                    //分配帐号列表
                    List<UM_AccountType> AccountType = StaticDalClass.AccountTypeDAL.GetListArray(string.Empty);
                    if (AccountType == null || AccountType.Count == 0)
                    {
                        mess = "GL-0201:获取帐号类型列表失败"; //写调试信息
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
                        mess = "GL-0202：初始化资金失败！"; //写调试信息
                        LogHelper.WriteDebug(mess);
                        return false;
                    }
                    Tran.Commit();
                }
                catch (Exception ex)
                {
                    Tran.Rollback();
                    string errCode = "GL-0203";
                    string errMsg = "添加交易员失败！";
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

                //调用柜台开户方法
                string s;
                try
                {
                    CT_Counter T = StaticDalClass.CounterDAL.GetModel((int)UserInfo.CouterID);

                    if (
                        !ServiceIn.AccountManageServiceProxy.GetInstance().SingleTraderOpenAccount(T, l_AccountEntity, out s))
                    {
                        mess = "GL-0204:调用柜台开户方法SingleTraderOpenAccount失败!" + s; //写调试信息
                        LogHelper.WriteDebug(mess);

                        DelTransaction(UserID, out s);
                        return false;
                    }
                }
                catch (VTException ex)
                {
                    mess = ex.ToString();
                    LogHelper.WriteError(ex.ToString(), ex.InnerException);
                    DelTransaction(UserID, out s);
                    //写错误信息
                    return false;
                }
                mess = "GL-0232:开户成功！";
                return true;
            }
            catch (Exception Ex)
            {
                string errCode = "GL-0205";
                string errMsg = "数据库连接失败！";
                VTException vte = new VTException(errCode, errMsg, Ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                mess = vte.ToString();
                return false;
            }
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
                string errCode = "GL-0206";
                string errMsg = "SaveFund()方法失败,帐号:" + BackAccount;
                VTException vte = new VTException(errCode, errMsg, Ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return false;
            }
            return true;
        }

        #endregion

        #region 修改交易员

        /// <summary>
        /// 修改交易员
        /// </summary>
        /// <param name="dtAdd">新增加的权限表</param>
        /// <param name="dtDel">要删除的权限表</param>
        /// <param name="UserInfo">用户信息</param>
        /// <param name="UpdatePwd">需要更新的密码</param>
        /// <param name="mess"></param>
        /// <returns></returns>
        public bool UpdateTransaction(DataTable dtAdd, DataTable dtDel, UM_UserInfo UserInfo, bool UpdatePwd,
                                      out string mess)
        {
            //调用柜台服务
            if (UpdatePwd)
            {
                try
                {
                    CT_Counter T = StaticDalClass.CounterDAL.GetModel((int)UserInfo.CouterID);
                    if (T == null)
                    {
                        mess = "GL-0207:交易员获取相应柜台失败"; //写调试信息
                        LogHelper.WriteDebug(mess);

                        return false;
                    }
                    if (!ServiceIn.AccountManageServiceProxy.GetInstance().UpdateUserPassword(T, UserInfo.UserID,
                                                                                              UserInfo.Password, out mess))
                    {
                        mess = "GL-0208:调用柜台修改交易员方法UpdateUserPassword()失败!" + mess; //写调试信息
                        LogHelper.WriteDebug(mess);

                        return false;
                    }
                }
                catch (VTException Ex)
                {
                    LogHelper.WriteError(Ex.ToString(), Ex.InnerException);
                    mess = Ex.ToString();
                    //写错误日志
                    return false;
                }
            }

            Database db = DatabaseFactory.CreateDatabase();
            DbConnection Conn = db.CreateConnection();
            if (Conn.State != ConnectionState.Open) Conn.Open();
            DbTransaction Tran = Conn.BeginTransaction();
            try
            {
                StaticDalClass.UserInfoDAL.Update(UserInfo, Tran, db);

                for (int i = 0; i < dtAdd.Rows.Count; i++)
                {
                    UM_DealerTradeBreedClass DealerTradeBreedClass = new UM_DealerTradeBreedClass();
                    DealerTradeBreedClass.UserID = UserInfo.UserID;
                    DealerTradeBreedClass.BreedClassID = int.Parse(dtAdd.Rows[i]["BreedClassID"].ToString());
                    StaticDalClass.DealerTradeBreedClassDAL.Add(DealerTradeBreedClass, Tran, db);
                }
                for (int i = 0; i < dtDel.Rows.Count; i++)
                {
                    StaticDalClass.DealerTradeBreedClassDAL.Delete(
                        int.Parse(dtDel.Rows[i]["DealerTradeBreedClassID"].ToString()), Tran, db);
                }
                Tran.Commit();
                mess = "GL-0209:修改成功！";
                return true;
            }
            catch (Exception Ex)
            {
                Tran.Rollback();
                string errCode = "GL-0210";
                string errMsg = "修改交易员失败！";
                VTException vte = new VTException(errCode, errMsg, Ex);
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
            //更新本地状态  只要状态更新成功，如果本次删除失败会通过定时服务再次去删除
            //try
            //{
            //    StaticDalClass.UserInfoDAL.UpdateDelState(UserID, null, null);
            //}
            //catch (Exception Ex)
            //{
            //    string errCode = "GL-0211";
            //    string errMsg = "交易员更新为删除状态失败";
            //    VTException vte = new VTException(errCode, errMsg, Ex);
            //    mess = vte.ToString();
            //    return false;
            //}
            //调用柜台销户方法
            try
            {
                CT_Counter T = GetCounterByUserID(UserID);
                if (T == null)
                {
                    mess = "GL-0212:交易员获取相应柜台失败"; //写调试信息
                    LogHelper.WriteDebug(mess);
                    return false;
                }
                if (!AccountManageServiceProxy.GetInstance().DeleteSingleTraderAccount(T, UserID.ToString(), out mess))
                {
                    mess = "GL-0213:调用柜台柜台销户方法DeleteSingleTraderAccount失败!" + mess; //写调试信息
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
                    mess = "GL-0214:交易员删除成功！";
                    return true;
                }
                catch (Exception ex)
                {
                    Tran.Rollback();
                    string errCode = "GL-0215";
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
                string errCode = "GL-0216";
                string errMsg = "连接数据库失败！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                mess = vte.ToString();
                return false;
            }
        }

        #endregion

        #region 获取柜台实体
        /// <summary>
        /// 根据用户ID得到柜台实体
        /// </summary>
        /// <param name="UserID">用户ID</param>
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
                string errCode = "GL-0217";
                string errMsg = "根据用户ID得到柜台实体GetCounterByUserID()方法异常！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据账户ID获取柜台信息
        /// </summary>
        /// <param name="DealerAccoutID">账户ID</param>
        /// <returns></returns>
        public CT_Counter GetCounterByAccountID(string DealerAccoutID)
        {
            try
            {
                UM_DealerAccount DealerAccount = StaticDalClass.DealerAccountDAL.GetModel(DealerAccoutID);
                return GetCounterByUserID(DealerAccount.UserID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-0218";
                string errMsg = "根据帐号得到柜台实体GetCounterByAccountID()方法异常！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        #endregion

        #region 追加资金
        /// <summary>
        /// 追加资金
        /// </summary>
        /// <param name="model">追加资金实体</param>
        /// <param name="mess"></param>
        /// <returns></returns>
        public bool AddFund(ManagementCenter.Model.UM_FundAddInfo model, out string mess)
        {
            mess = string.Empty;
            UM_UserInfo user = StaticDalClass.UserInfoDAL.GetModel((int)model.UserID);
            if (user == null || user.RoleID != (int)ManagementCenter.Model.CommonClass.Types.RoleTypeEnum.Transaction)
            {
                mess = "GL-0219:交易员编号不存在！"; //写调试信息
                LogHelper.WriteDebug(mess);
                return false;
            }

            CT_Counter T = GetCounterByUserID((int)model.UserID);
            if (T == null)
            {
                mess = "GL-0220:交易员对应的柜台不存在！"; //写调试信息
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
                mess = "GL-0221:交易员对应的银行帐号不存在！"; //写调试信息
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
                    mess = "GL-0222:调用柜台追加资金方法AddCapital()失败!" + mess; //写调试信
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
                string errCode = "GL-0223";
                string errMsg = "追加资金失败！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                mess = vte.ToString();
                return false;
            }
            return true;
        }

        #endregion

        #region 冻结帐号
        /// <summary>
        /// 冻结帐号
        /// </summary>
        /// <param name="DealerAccount">帐号表</param>
        /// <param name="FreezeReason">资金冻结表</param>
        /// <param name="mess"></param>
        /// <returns></returns>
        public bool FreezeAccount(UM_DealerAccount DealerAccount, UM_FreezeReason FreezeReason, out string mess)
        {
            mess = string.Empty;
            CT_Counter T = GetCounterByUserID((int)DealerAccount.UserID);
            if (T == null)
            {
                mess = "GL-0224:交易员对应的柜台不存在！"; //写调试信息
                LogHelper.WriteDebug(mess);
                return false;
            }
            try
            {
                List<FindAccountEntity> l = new List<FindAccountEntity>();
                FindAccountEntity FindAccountEntity = new FindAccountEntity();
                FindAccountEntity.AccountType = (int)DealerAccount.AccountTypeID;
                FindAccountEntity.UserID = DealerAccount.UserID.ToString();
                l.Add(FindAccountEntity);
                if (!ServiceIn.AccountManageServiceProxy.GetInstance().FreezeAccount(T, l, out mess))
                {
                    mess = "GL-0225:调用柜台冻结帐号方法FreezeAccount()失败!" + mess; //写调试信息
                    LogHelper.WriteDebug(mess);
                    return false;
                }
            }
            catch (VTException Ex)
            {
                LogHelper.WriteError(Ex.ToString(), Ex.InnerException);
                mess = Ex.ToString();
                //写错误日志
                return false;
            }
            try
            {
                StaticDalClass.DealerAccountDAL.Update(DealerAccount);
                StaticDalClass.FreezeReasonDAL.Add(FreezeReason);
            }
            catch (Exception ex)
            {
                string errCode = "GL-0226";
                string errMsg = "冻结帐号方法FreezeAccount()异常!" + ex.Message;
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                mess = vte.ToString();
                return false;
            }
            return true;
        }

        #endregion

        #region 解冻帐号
        /// <summary>
        /// 解冻帐号
        /// </summary>
        /// <param name="DealerAccount">帐号表</param>
        /// <param name="ThawReason">解冻原因</param>
        /// <param name="mess"></param>
        /// <returns></returns>
        public bool ThawAccount(UM_DealerAccount DealerAccount, UM_ThawReason ThawReason, out string mess)
        {
            mess = string.Empty;
            CT_Counter T = GetCounterByUserID((int)DealerAccount.UserID);
            List<FindAccountEntity> l = new List<FindAccountEntity>();
            if (T == null)
            {
                mess = "GL-0227:交易员对应的柜台不存在！"; //写调试信息
                LogHelper.WriteDebug(mess);
                return false;
            }
            try
            {
                FindAccountEntity FindAccountEntity = new FindAccountEntity();
                FindAccountEntity.AccountType = (int)DealerAccount.AccountTypeID;
                FindAccountEntity.UserID = DealerAccount.UserID.ToString();
                l.Add(FindAccountEntity);
                if (!ServiceIn.AccountManageServiceProxy.GetInstance().ThawAccount(T, l, out mess))
                {
                    mess = "GL-0228:调用柜台解冻帐号方法ThawAccount()失败!" + mess; //写调试信息
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
                StaticDalClass.DealerAccountDAL.Update(DealerAccount);
                StaticDalClass.ThawReasonDAL.Add(ThawReason);
            }
            catch (Exception ex)
            {
                ServiceIn.AccountManageServiceProxy.GetInstance().FreezeAccount(T, l, out mess);
                string errCode = "GL-0229";
                string errMsg = "冻结帐号方法FreezeAccount()异常!" + ex.Message;
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                mess = vte.ToString();
                return false;
            }
            return true;
        }

        #endregion

        #region 自动解冻

        /// <summary>
        /// 自动解冻
        /// </summary>
        public void AutoUnFreeze()
        {
            try
            {
                //每天凌晨一点启动服务
                UM_DealerAccount DealerAccount;

                DateTime dt1 = DateTime.Parse(System.DateTime.Now.ToShortDateString());
                DateTime dt2 = DateTime.Parse(System.DateTime.Now.AddDays(1).ToShortDateString());
                int IsAuto = (int)Types.IsAutoUnFreezeEnum.Auto;
                List<UM_FreezeReason> list_FreezeReason =
                    StaticDalClass.FreezeReasonDAL.GetListArray(
                        string.Format("ThawReasonTime>='{0}' AND ThawReasonTime<='{1}' AND IsAuto={2}", dt1, dt2, IsAuto));

                if (list_FreezeReason != null && list_FreezeReason.Count > 0)
                {
                    foreach (UM_FreezeReason FreezeReason in list_FreezeReason)
                    {
                        DealerAccount = StaticDalClass.DealerAccountDAL.GetModel(FreezeReason.DealerAccoutID);
                        if (DealerAccount != null)
                        {
                            DealerAccount.IsDo = true;

                            //调用柜台服务
                            try
                            {
                                List<FindAccountEntity> l = new List<FindAccountEntity>();
                                FindAccountEntity Entity = new FindAccountEntity();
                                Entity.AccountType = (int)DealerAccount.AccountTypeID;
                                Entity.UserID = DealerAccount.UserID.ToString();
                                l.Add(Entity);
                                string s;
                                if (
                                    AccountManageServiceProxy.GetInstance().ThawAccount(
                                        GetCounterByUserID(DealerAccount.UserID), l, out s))
                                {
                                    //本地修改
                                    StaticDalClass.DealerAccountDAL.Update(DealerAccount);
                                }
                                else
                                {
                                    string mess = "GL-0230:调用柜台解冻帐号方法ThawAccount()失败!" + s; //写调试信息
                                }
                            }
                            catch (VTException ex)
                            {
                                //写错误日志
                                string mess = ex.ToString();
                            }
                        }
                    }
                }
                else
                {
                    LogHelper.WriteDebug("资金冻结表数据为空");
                }

            }
            catch (Exception ex)
            {
                string errCode = "GL-0231";
                string errMsg = "调用AutoUnFreeze()方法异常!";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return;
            }
        }

        #endregion

        #region 管理员查询根据交易员查询交易员各资金账户相关信息
        /// <summary>
        /// 管理员查询根据交易员查询交易员各资金账户相关信息
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="adminPassword">管理员密码</param>
        /// <param name="traderId">交易员ID</param>
        /// <param name="CouterID">柜台ID</param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public DataTable AdminFindTraderCapitalAccountInfoByID(string adminId, string adminPassword, string traderId, int CouterID, out string strErrorMessage)
        {
            //调用柜台管理员查询根据交易员查询交易员各资金账户相关信息方法
            // string s;
            try
            {
                CT_Counter T = StaticDalClass.CounterDAL.GetModel(CouterID);
                List<TradersAccountCapitalInfo> tradersAccountCList =
                    ServiceIn.AccountManageServiceProxy.GetInstance().AdminFindTraderCapitalAccountInfoByID(T, adminId,
                                                                                                            adminPassword,
                                                                                                            traderId,
                                                                                                            out
                                                                                                                strErrorMessage);

                if (tradersAccountCList.Count > 0)
                {
                    //return tradersAccountCList;
                    return ToDataTable(tradersAccountCList);
                }
                else
                {
                    strErrorMessage = "GL-0233:调用柜台开户方法AdminFindTraderCapitalAccountInfoByID失败!";// +s; //写调试信息
                    LogHelper.WriteDebug(strErrorMessage);

                }
            }
            catch (VTException ex)
            {
                strErrorMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex.InnerException);
                return null;
            }
            return null;
        }
        #endregion

        #region 将集合类转换成DataTable
        /// <summary>
        /// 将集合类转换成DataTable
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public DataTable ToDataTable(List<TradersAccountCapitalInfo> list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
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
        public bool TwoAccountsFreeTransferFunds(FreeTransferEntity freeTransfer, TypesCurrencyType currencyType, int couterID,
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
                        //outMessage = "GL-:调用柜台自由转帐（同币种）方法TwoAccountsFreeTransferFunds失败!";// +s; //写调试信息
                        LogHelper.WriteDebug(outMessage);
                    return false;
                }
                return true;
            }
            catch (VTException ex)
            {
                outMessage = "GL-0234:调用柜台自由转帐（同币种）方法TwoAccountsFreeTransferFunds失败!";
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
        /// <param name="traderID">交易员ID</param>
        /// <param name="FromCapitalAccount">转出资金账户类型</param>
        /// <param name="ToCapitalAccount">转入资金账户类型</param>
        /// <param name="TransferAmount">转账数量</param>
        /// <param name="couterID">柜台ID</param>
        /// <param name="currencyType">当前币种类型</param>
        /// <param name="outMessage">输出参数</param>
        /// <returns>返回true或false</returns>
        public bool ConvertFreeTransferEntity(int traderID, int FromCapitalAccount, int ToCapitalAccount, decimal TransferAmount, int currencyType, int couterID, out string outMessage)
        {
            try
            {
                FreeTransferEntity freeTransferEntity = new FreeTransferEntity();
                freeTransferEntity.TraderID = traderID.ToString();
                freeTransferEntity.FromCapitalAccountType = FromCapitalAccount;
                freeTransferEntity.ToCapitalAccountType = ToCapitalAccount;
                freeTransferEntity.TransferAmount = TransferAmount;
                TypesCurrencyType _currencyType = (TypesCurrencyType)currencyType;
                bool _result = TwoAccountsFreeTransferFunds(freeTransferEntity, _currencyType, couterID, out outMessage);
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

        #region 管理员查询根据交易员查询交易员银行资金账户相关信息
        /// <summary>
        /// 管理员查询根据交易员查询交易员各银行资金账户相关信息
        /// </summary>
        /// <param name="adminId">管理员ID(可传空值：因柜台没验证)</param>
        /// <param name="adminPassword">管理员密码(可传空值：因柜台没验证)</param>
        /// <param name="traderID">交易员ID</param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <returns>返回交易员的资金明细</returns>
        public UM_FundAddInfo AdminFindTraderBankCapitalAccountInfoByID(string adminId, string adminPassword, string traderID, out string strErrorMessage)
        {
            //调用柜台管理员查询根据交易员查询交易员各资金账户相关信息方法
            List<TradersAccountCapitalInfo> tradersAccountCList = new List<TradersAccountCapitalInfo>();
            UM_FundAddInfo uM_FundAddInfo = new UM_FundAddInfo();//追加资金明细实体
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
                    return null;
                }
                //返回银行账户里的可用资金
                foreach (TradersAccountCapitalInfo traderACInfo in tradersAccountCList)
                {
                    if (traderACInfo.AccountType == (int)GTA.VTS.Common.CommonObject.Types.AccountType.BankAccount)
                    {
                        switch (traderACInfo.CurrencyType)
                        {
                            case (int)GTA.VTS.Common.CommonObject.Types.CurrencyType.RMB:
                                uM_FundAddInfo.RMBNumber = traderACInfo.AvailableCapital;
                                break;
                            case (int)GTA.VTS.Common.CommonObject.Types.CurrencyType.HK:
                                uM_FundAddInfo.HKNumber = traderACInfo.AvailableCapital;
                                break;
                            case (int)GTA.VTS.Common.CommonObject.Types.CurrencyType.US:
                                uM_FundAddInfo.USNumber = traderACInfo.AvailableCapital;
                                break;
                            default:
                                break;


                        }
                    }
                }

            }
            catch (VTException ex)
            {
                strErrorMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex.InnerException);
                return null;
            }
            return uM_FundAddInfo;
        }
        #endregion

    }

}