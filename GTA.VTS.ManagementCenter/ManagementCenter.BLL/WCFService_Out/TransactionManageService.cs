using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using GTA.VTS.Common.CommonUtility;
using ManagementCenter.BLL.CommonTable;
using ManagementCenter.BLL.ServiceIn;
using ManagementCenter.BLL.UserManage;
using ManagementCenter.BLL.WCFService_Out.interfases;
using System.ServiceModel;
using ManagementCenter.DAL.AccountManageService;
using ManagementCenter.Model;
using ManagementCenter.Model.CommonClass;
using ManagementCenter.Model.UserManage;
using Entity = ManagementCenter.Model;

namespace ManagementCenter.BLL.WCFService_Out
{
    ///<summary> 
    /// 模块编号：
    /// 作用：交易员管理模块,用WCF实现对外公布的ITransactionManage接口
    /// 作者：熊晓凌                   
    /// 编写日期：2008-11-17           
    /// 修改：叶振东
    /// 修改日期：2009-12-23
    /// Update By:李健华
    /// Update Date:2009-12-28
    /// Desc.:修改代码的接口实现定义
    ///
    /// 修改：刘书伟
    /// 修改日期：2010-01-07
    /// 描述:添加提供给金融平台的开户方法和转账方法
    /// Desc: 将定时执行的时间判断放到StartUpdateService里，避免程序一启动就执行更新操作
    /// Update By: 董鹏
    /// Update Date: 2010-01-22
    ///</summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class TransactionManageService : ITransactionManage
    {
        #region 交易员管理提供给前台的服务
        /// <summary>
        /// 添加交易员
        /// </summary>
        /// <param name="Number">数目</param>
        /// <param name="initFund"></param>
        /// <param name="MLoginName">管理员登陆名称</param>
        /// <param name="ManagerPWd">管理员密码</param>
        /// <param name="message">返回的信息</param>
        /// <returns></returns>
        public List<Entity.UM_UserInfo> BatchAddTransaction(int Number, InitFund initFund, string MLoginName, string ManagerPWd, out string message)
        {
            UserManage.Out_TransactionManage tm = new Out_TransactionManage();
            return tm.BatchAddTransaction(Number, initFund, MLoginName, ManagerPWd, out message);
        }

        /// <summary>
        /// 添加交易员
        /// </summary>
        /// <param name="tUserInfo">交易员列表</param>
        /// <param name="initFund"></param>
        /// <param name="MLoginName">管理员登陆名称</param>
        /// <param name="ManagerPWd">管理员密码</param>
        /// <param name="message">返回的信息</param>
        /// <returns></returns>
        public List<Entity.UM_UserInfo> BatchAddTransactionList(List<Entity.UM_UserInfo> tUserInfo, InitFund initFund, string MLoginName, string ManagerPWd, out string message)
        {
            message = string.Empty;
            return null;
        }

        /// <summary>
        /// 更新交易员
        /// </summary>
        /// <param name="UserID">交易员ID</param>
        /// <param name="Password">密码</param>
        /// <param name="MLoginName">管理员登陆名称</param>
        /// <param name="ManagerPWd">管理员密码</param>
        /// <param name="message">返回的信息</param>
        /// <returns></returns>
        public bool UpdateTransaction(int UserID, string Password, string MLoginName, string ManagerPWd, out string message)
        {
            message = string.Empty;
            return false;
        }

        /// <summary>
        /// 更新交易员
        /// </summary>
        /// <param name="userInfo">交易员实体</param>
        /// <param name="MLoginName">管理员登陆名称</param>
        /// <param name="ManagerPWd">管理员密码</param>
        /// <param name="message">返回的信息</param>
        /// <returns></returns>
        public bool UpdateTransactionAll(Entity.UM_UserInfo userInfo, string MLoginName, string ManagerPWd, out string message)
        {
            message = string.Empty;
            return false;
        }

        /// <summary>
        ///  删除交易员
        /// </summary>
        /// <param name="userInfo">交易员实体</param>
        /// <param name="MLoginName">管理员登陆名称</param>
        /// <param name="ManagerPWd">管理员密码</param>
        /// <param name="message">返回的信息</param>
        /// <returns></returns>
        public bool BatchDelectTransaction(List<Entity.UM_UserInfo> userInfo, string MLoginName, string ManagerPWd, out string message)
        {
            UserManage.Out_TransactionManage tm = new Out_TransactionManage();
            return tm.BatchDelectTransaction(userInfo, MLoginName, ManagerPWd, out message);
        }


        /// <summary>
        /// 交易员验证
        /// </summary>
        /// <param name="UserID">交易员ID</param>
        /// <param name="PassWord">交易员密码</param>
        /// <param name="message">返回的信息</param>
        /// <returns></returns>
        public Entity.CT_Counter TransactionConfirm(int UserID, string PassWord, out string message)
        {
            UserManage.Out_TransactionManage tm = new Out_TransactionManage();
            return tm.LoginConfirmation(UserID, PassWord, out message);
        }

        /// <summary>
        /// 追加资金(此方法之前是没有实现的，这里实现只是用于验证管理员是否正确,为了测试工具使用，如果日后要用再
        /// 作修改，因为这里也要验证管理员)
        /// </summary>
        /// <param name="UserID">交易员ID</param>
        /// <param name="initFund"></param>
        /// <param name="MLoginName">管理员登陆名称</param>
        /// <param name="ManagerPWd">管理员密码</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool AddFund(int UserID, InitFund initFund, string MLoginName, string ManagerPWd, out string message)
        {
            message = string.Empty;
            Out_TransactionManage outTran = new Out_TransactionManage();
            bool login = outTran.AdminLoginConfirmation(MLoginName, ManagerPWd, out message);
            return login;
        }
        #endregion

        //===============================================提供给金融工程平台的方法=====================================
        #region 提供给金融工程平台的方法

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
        /// <returns></returns>
        public UM_UserInfo AddTransactionFP(string MLoginName, string ManagerPWd, UM_UserInfo UserInfo, InitFund initfund, out List<AccountEntity> l_AccountEntity, out string message)
        {
            UserManage.Out_TransactionManage tm = new Out_TransactionManage();
            return tm.AddTransactionFP(MLoginName, ManagerPWd, UserInfo, initfund, out l_AccountEntity, out message);
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
        public List<TradersAccountCapitalInfo> AdminFindTraderCapitalAccountInfoByIDFP(string adminId, string adminPassword, string traderID, out string strErrorMessage)
        {
            UserManage.Out_TransactionManage tm = new Out_TransactionManage();
            return tm.AdminFindTraderCapitalAccountInfoByIDFP(adminId, adminPassword, traderID, out strErrorMessage);

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
            UserManage.Out_TransactionManage tm = new Out_TransactionManage();
            return tm.ConvertFreeTransferEntityFP(traderID, FromCapitalAccountType, ToCapitalAccountType, TransferAmount,
                                                currencyType, out outMessage);
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
            UserManage.Out_TransactionManage tm = new Out_TransactionManage();
            return tm.AddFundFP(model, out mess);
        }
        #endregion

        #region  检查管理中心服务通道
        /// <summary>
        /// 检查管理中心服务通道
        /// </summary>
        /// <returns></returns>
        public string CheckChannel()
        {
            return DateTime.Now.ToString();
        }
        #endregion

        #endregion
        /// <summary>
        /// 实例化时间控件 
        /// </summary>
        public System.Timers.Timer t = new System.Timers.Timer();
        /// <summary>
        /// 定义线程变量
        /// </summary>
        public System.Threading.Thread schedulerThread;

        /// <summary>
        /// 启动服务更新
        /// </summary>
        public void Start()
        {
            t.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);
            t.Interval = 3600 * 1000;
            t.Enabled = true;
            t.Start();
        }

        /// <summary>
        /// 服务更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                t.Enabled = false;

                this.StartUpdateService();

                t.Enabled = true;
            }
            catch (Exception ex)
            {
                t.Enabled = true;
                //写异常日志
                LogHelper.WriteError(ex.Message.ToString(), ex);
            }
        }

        /// <summary>
        /// Desc: 判断当前时间时候定时执行时间
        /// Create By: 董鹏
        /// Create Date: 2010-01-22
        /// </summary>
        /// <returns></returns>
        private bool IsUpdateTime()
        {
            int UpdateCodeTime;
            if (!int.TryParse(System.Configuration.ConfigurationManager.AppSettings["UpdateCodeTime"].ToString(), out UpdateCodeTime))
            {
                UpdateCodeTime = 23;
            }

            if (System.DateTime.Now.Hour == UpdateCodeTime)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 每次当服务启动时更新数据
        /// </summary>
        public void StartUpdateService()
        {
            if (IsUpdateTime())
            {
                CommonTable.CommodityCodeUpdate CommodityCodeUpdate = new CommodityCodeUpdate();

                //把StockInfo表和HKStockInfo表中新增的代码添加到交易商品_撮合机_分配表中
                LogHelper.WriteDebug("开始调用新增代码添加到撮合机的方法：XHCodeAutoRCTradeCommodityAssign()" + DateTime.Now);
                CommodityCodeUpdate.XHCodeAutoRCTradeCommodityAssign();
                LogHelper.WriteDebug("结束调用新增代码添加到撮合机的方法：XHCodeAutoRCTradeCommodityAssign()" + DateTime.Now);

                //从StockInfo表中更新新增和修改的现货代码
                LogHelper.WriteDebug("开始调用更新新增和修改的现货代码的方法：CodeUpdata()" + DateTime.Now);
                CommodityCodeUpdate.CodeUpdata();
                LogHelper.WriteDebug("结束调用更新新增和修改的现货代码的方法：CodeUpdata()" + DateTime.Now);

                //从HKStockInfo表中更新新增和修改的港股交易商品代码更新
                LogHelper.WriteDebug("开始调用更新新增和修改港股代码的方法：HKCodeUpdata()" + DateTime.Now);
                CommodityCodeUpdate.HKCodeUpdata();
                LogHelper.WriteDebug("结束调用更新新增和修改港股代码的方法：HKCodeUpdata()" + DateTime.Now);

                //期货代码更新
                LogHelper.WriteDebug("开始调用期货代码更新的方法：CheckCodeIsExpired()" + DateTime.Now);
                CommodityCodeUpdate.CheckCodeIsExpired();
                LogHelper.WriteDebug("结束调用期货代码更新的方法：CheckCodeIsExpired()" + DateTime.Now);

                //自动解冻
                LogHelper.WriteDebug("开始调用自动解冻的方法：AutoUnFreeze()" + DateTime.Now);
                ManagementCenter.BLL.UserManage.TransactionManage TransactionManage = new TransactionManage();
                TransactionManage.AutoUnFreeze();
                LogHelper.WriteDebug("结束调用自动解冻的方法：AutoUnFreeze()" + DateTime.Now);

                //删除标志状态为FrontTarnDelState=5的交易员
                LogHelper.WriteDebug("开始调用删除标志状态为FrontTarnDelState=5的交易员的方法：DelTran()" + DateTime.Now);

                ManagementCenter.BLL.UserManage.Out_TransactionManage outTransactionManage = new Out_TransactionManage();
                outTransactionManage.DelTran();
                LogHelper.WriteDebug("结束调用删除标志状态为FrontTarnDelState=5的交易员的方法：DelTran()" + DateTime.Now);

                //更新非交易日期
                int Currentyear = System.DateTime.Now.Year;
                int nyear = System.DateTime.Now.AddDays(1).Year;
                if (Currentyear != nyear)
                {
                    LogHelper.WriteDebug("开始调用更新非交易日期的方法：InitNotTrandingDay()" + DateTime.Now);
                    ManagementCenter.BLL.CommonTable.QHCodeInit.InitNotTrandingDay(nyear);
                    LogHelper.WriteDebug("结束更新非交易日期的方法：InitNotTrandingDay()" + DateTime.Now);

                }
            }
        }

        #region Create 李健华 update接口实现 2009-12-28

        /// <summary>
        /// 对管理员用户名和密码进行验证,并清空数据
        /// </summary>
        /// <param name="pca">个性化资金实体类</param>
        /// <param name="LoginName">管理员用户名</param>
        /// <param name="PassWord">密码</param>
        /// <param name="message">返回的信息</param>
        /// <returns>数据是否清空成功</returns>
        public bool ClearTrialData(PersonalizationCapital pca, string LoginName, string PassWord, out string message)
        {
            //验证管理员用户名和密码是否通过验证
            bool lastly = true;
            Out_TransactionManage outTran = new Out_TransactionManage();
            bool login = outTran.AdminLoginConfirmation(LoginName, PassWord, out message);
            if (login == true)
            {
                #region 对获取的数据通过柜台进行分组，并根据分组后柜台调用柜台服务接口
                Dictionary<string, CapitalPersonalization> list = outTran.CounterGrouping(pca);
                //循环list中键值，并分别通过键值获取柜台信息和通过键值获取个性化资金信息再调用柜台的清空数据接口
                foreach (string key in list.Keys)
                {
                    string CID = key;
                    int CounterID = int.Parse(CID);
                    CT_Counter T = StaticDalClass.CounterDAL.GetModel((int)CounterID);
                    //调用柜台的清空数据接口
                    bool state = AccountManageServiceProxy.GetInstance().ClearTrialData(T, list[key], out message);
                    if (state == false)
                    {
                        lastly = false;
                    }
                }
                return lastly;
                #endregion 对获取的数据通过柜台进行分组，并根据分组后柜台调用柜台服务接口
            }
            return false;
        }
        /// <summary>
        /// 对管理员用户名和密码进行验证,并个性化设置操作
        /// </summary>
        /// <param name="pec">个性化资金实体类</param>
        /// <param name="LoginName">管理员用户名</param>
        /// <param name="PassWord">密码</param>
        /// <param name="message">返回的信息</param>
        /// <returns>格式化资金是否成功</returns>
        public bool PersonalizationCapital(PersonalizationCapital pec, string LoginName, string PassWord, out string message)
        {
            //验证管理员用户名和密码是否通过验证
            bool lastly = true;
            Out_TransactionManage outTran = new Out_TransactionManage();
            bool login = outTran.AdminLoginConfirmation(LoginName, PassWord, out message);
            if (login == true)
            {
                #region 对获取的数据通过柜台进行分组，并根据分组后柜台调用柜台服务接口
                Dictionary<string, CapitalPersonalization> list = outTran.CounterGrouping(pec);
                //循环list中键值，并分别通过键值获取柜台信息和通过键值获取个性化资金信息再调用柜台的个性化资金接口
                foreach (string key in list.Keys)
                {
                    string CID = key;
                    int CounterID = int.Parse(CID);
                    CT_Counter T = StaticDalClass.CounterDAL.GetModel((int)CounterID); ;
                    //调用柜台的个性化资金接口
                    bool state = AccountManageServiceProxy.GetInstance().PersonalizationCapital(T, list[key], out message);
                    if (state == false)
                    {
                        lastly = false;
                    }
                }
                return lastly;
                #endregion 对获取的数据通过柜台进行分组，并根据分组后柜台调用柜台服务接口
            }
            return false;
        }

        #endregion
    }
}
