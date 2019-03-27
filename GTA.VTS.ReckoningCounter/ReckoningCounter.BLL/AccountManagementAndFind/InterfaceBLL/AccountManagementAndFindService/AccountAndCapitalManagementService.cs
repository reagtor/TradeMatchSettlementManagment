#region Using Namespace

using System;
using System.Collections.Generic;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.AccountManagementAndFind.AccountManagementAndFindBLL;
using ReckoningCounter.BLL.DelegateValidate;
using ReckoningCounter.BLL.DelegateValidate.Cost;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.BLL.ScheduleManagement;
using ReckoningCounter.DAL;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.DAL.FuturesDevolveService;
using ReckoningCounter.DAL.SpotTradingDevolveService;
using ReckoningCounter.Entity;
using ReckoningCounter.Entity.AccountManagementAndFindEntity;
using ReckoningCounter.Model;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.MemoryData;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.DAL.HKTradingRulesService;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.Entity.Model.QH;
using System.Threading;
using RealTime.Server.SModelData.HqData;

#endregion

namespace ReckoningCounter.BLL.AccountManagementAndFind.InterfaceBLL.AccountManagementAndFindService
{
    /// <summary>
    /// 作用：账户管理接口（包括：单个交易员开户、批量开户、单个交易员销户、批量销户、 冻结账户、解冻账户、查询账户、查询账户、修改密码、追加资金、自由转账）
    /// 作者：李健华
    /// 日期：2009年8月20日
    /// Update by:李健华
    /// Update date:2009-12-23
    /// Desc.:添加个性化资金设置接口
    /// Update by:董鹏
    /// Update date:2009-12-23
    /// Desc.:添加执行试玩期后用户交易数据清空接口
    /// </summary>
    public class AccountAndCapitalManagementService : IAccountAndCapitalManagement
    {
        # region 单个开户

        /// <summary>
        /// 单个开户
        /// </summary>
        /// <param name="accounts">帐户对象</param>
        /// <param name="outMessage">输出信息</param>
        /// <returns></returns>
        public bool SingleTraderOpenAccount(List<AccountEntity> accounts, out string outMessage)
        {
            var accountManagementBLL = new AccountManagementBLL();
            return accountManagementBLL.SingleTraderOpenAccount(accounts, out outMessage);
        }

        # endregion

        # region 批量开户

        /// <summary>
        /// 批量开户
        /// </summary>
        /// <param name="accounts">帐户对象列表</param>
        /// <param name="outMessage">输出信息</param>
        /// <returns></returns>
        public bool VolumeTraderOpenAccount(List<AccountEntity> accounts, out string outMessage)
        {
            var accountManagementBLL = new AccountManagementBLL();
            return accountManagementBLL.VolumeTraderOpenAccount(accounts, out outMessage);
        }

        # endregion 单个开户

        # region 单个销户

        /// <summary>
        /// 单个销户
        /// </summary>
        /// <param name="userId">交易员ID</param>
        /// <param name="outMessage">输出信息</param>
        /// <returns></returns>
        public bool DeleteSingleTraderAccount(string userId, out string outMessage)
        {
            var accountManagementBLL = new AccountManagementBLL();
            return accountManagementBLL.DeleteSingleTraderAccount(userId, out outMessage);
        }

        # endregion

        # region 批量销户

        /// <summary>
        /// 批量销户
        /// </summary>
        /// <param name="userIDs">交易员字符数组</param>
        /// <param name="outMessage">输出信息</param>
        /// <returns></returns>
        public bool DeleteVolumeTraderAccount(string[] userIDs, out string outMessage)
        {
            var accountManagementBLL = new AccountManagementBLL();
            return accountManagementBLL.DeleteVolumeTraderAccount(userIDs, out outMessage);
        }

        # endregion

        # region 冻结帐户

        /// <summary>
        /// 冻结帐户
        /// </summary>
        /// <param name="accounts">帐户实体</param>
        /// <param name="outMessage">输出信息</param>
        /// <returns></returns>
        public bool FreezeAccount(List<FindAccountEntity> accounts, out string outMessage)
        {
            var accountManagementBLL = new AccountManagementBLL();
            return accountManagementBLL.FreezeAccount(accounts, out outMessage);
        }

        # endregion

        # region 解冻帐户

        /// <summary>
        /// 解冻帐户
        /// </summary>
        /// <param name="accounts">帐户实体</param>
        /// <param name="outMessage">输出信息</param>
        /// <returns></returns>
        public bool ThawAccount(List<FindAccountEntity> accounts, out string outMessage)
        {
            var accountManagementBLL = new AccountManagementBLL();
            return accountManagementBLL.ThawAccount(accounts, out outMessage);
        }

        # endregion

        # region 查询帐户

        /// <summary>
        /// 查询帐户
        /// </summary>
        /// <param name="password">交易员密码</param>
        /// <param name="outMessage">输出信息</param>
        /// <param name="traderId">交易员ID</param>
        /// <returns></returns>
        public List<AccountFindResultEntity> FindAccount(string traderId, string password, out string outMessage)
        {
            var accountManagementBLL = new AccountManagementBLL();
            var findAccount = new FindAccountEntity();
            findAccount.UserID = traderId;
            findAccount.UserPassword = password;
            return accountManagementBLL.FindAccount(findAccount, out outMessage);
        }

        # endregion

        # region  查询交易权限

        /// <summary>
        ///  查询交易权限
        /// </summary>
        /// <param name="password">交易员密码</param>
        /// <param name="outMessage">输出信息</param>
        /// <param name="traderId">交易员ID</param>
        /// <returns></returns>
        public List<CM_BreedClass> FindTradePrivileges(string traderId, string password, out string outMessage)
        {
            var accountManagementBLL = new AccountManagementBLL();
            var findAccount = new FindAccountEntity();
            findAccount.UserID = traderId;
            findAccount.UserPassword = password;
            return accountManagementBLL.FindTradePrivileges(findAccount, out outMessage);
        }

        # endregion

        # region 修改密码

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId">交易员ID</param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword">新密码</param>
        /// <param name="outMessage">输出信息</param>
        /// <returns></returns>
        public bool UpdateUserPassword(string userId, string oldPassword, string newPassword, out string outMessage)
        {
            var accountManagementBLL = new AccountManagementBLL();
            return accountManagementBLL.UpdateUserPassword(userId, oldPassword, newPassword, out outMessage);
        }

        # endregion

        # region 追加资金

        /// <summary>
        /// 追加资金
        /// </summary>
        /// <param name="addCapital">追加资金实体</param>
        /// <param name="outMessage">输出信息</param>
        /// <returns></returns>
        public bool AddCapital(AddCapitalEntity addCapital, out string outMessage)
        {
            var capitalManagementBLL = new CapitalManagementBLL();
            return capitalManagementBLL.AddCapital(addCapital, out outMessage);
        }

        # endregion

        # region 自由转帐（同币种）

        /// <summary>
        /// 自由转帐（同币种）
        /// </summary>
        /// <param name="freeTransfer"></param>
        /// <param name="currencyType"></param>
        /// <param name="outMessage">输出信息</param>
        /// <returns></returns>
        public bool TwoAccountsFreeTransferFunds(FreeTransferEntity freeTransfer, Types.CurrencyType currencyType,
                                                 out string outMessage)
        {
            var capitalManagementBLL = new CapitalManagementBLL();
            return capitalManagementBLL.TwoAccountsFreeTransferFunds(freeTransfer, currencyType, out outMessage);
        }

        # endregion

        # region  检查通道

        /// <summary>
        /// 检查通道
        /// </summary>
        /// <returns></returns>
        public string CheckChannel()
        {
            return DateTime.Now.ToString();
        }

        /// <summary>
        /// 根据日期查询柜台清算是否已经完成
        /// </summary>
        /// <param name="doneDate">日期</param>
        /// <returns></returns>
        public bool IsReckoningDone(DateTime doneDate)
        {
            if (StatusTableChecker.HasDoneFutureReckoning(doneDate) && StatusTableChecker.HasDoneStockReckoning(doneDate) && StatusTableChecker.HasDoneHKReckoning(doneDate) &&
                StatusTableChecker.HasDoneStockHistoryDataProcess(doneDate) && StatusTableChecker.HasDoneFutureHistoryDataProcess(doneDate) && StatusTableChecker.HasDoneHKHistoryDataProcess(doneDate))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 是否正在清算
        /// </summary>
        /// <returns></returns>
        public bool IsReckoning()
        {
            if (ScheduleManager.IsStockReckoning || ScheduleManager.IsFutureReckoning || ScheduleManager.IsHKReckoning)
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// 故障恢复清算，此方法只能由管理中心调用
        /// </summary>
        /// <param name="list">要提供的期货当日结算价列表</param>
        /// <param name="errorMsg">执行异常信息</param>
        /// <returns></returns>
        public bool FaultRecoveryReckoning(List<QH_TodaySettlementPriceInfo> list, out string errorMsg)
        {

            errorMsg = "清算指今已经提交，请稍后....！";
            //1.如果正在清算返回不可接收本次故障恢复清算的提交,判断提交的数据是否是要做故障恢复清算的数据
            //2.清空当前期货今日结算价表中所有数据
            //3.根据提交的数据插入保存相关的今日结算价数据
            //4.提交开始清算指令
            LogHelper.WriteDebug(errorMsg);
            try
            {
                //if (Utils.IsNullOrEmpty(list))
                //{
                //    errorMsg = "提交的数据不能为空,清算失败";
                //    return false;
                //}

                if (ScheduleManager.IsFaultRecoveryFutureReckoning)
                {
                    errorMsg = "期货故障恢复清算请在进行，请稍后再试!";
                    return false;
                }

                //再次验证提交的数据是否是要执行清算的数据 
                DateTime ReckoningDateTime;
                bool isReckoning = false;
                LogHelper.WriteDebug("正在执行判断是否可以执行清算故障恢复！");
                //当前发送过来的日期
                int currentSendDate = int.Parse(DateTime.Now.AddDays(-1).Date.ToString("yyyyMMdd"));
                if (!Utils.IsNullOrEmpty(list))
                {
                    currentSendDate = list[0].TradingDate;
                }
                isReckoning = StatusTableChecker.IsFutureReckoningFaultRecovery(out ReckoningDateTime, out errorMsg);
                if (isReckoning)
                {
                    if (int.Parse(ReckoningDateTime.ToString("yyyyMMdd")) != currentSendDate)
                    {
                        errorMsg = "提交的清算今日价格不是所要提交的数据,清算失败";
                        return false;
                    }
                }
                else
                {
                    //errorMsg = "不能执行故障恢复清算操作,记录中没有清算失败的记录!";
                    return false;
                }
                LogHelper.WriteDebug("正在保存所有清算价格数据！");

                //1.
                //2.
                QH_TodaySettlementPriceDal dal = new QH_TodaySettlementPriceDal();
                dal.Delete();
                //3.

                foreach (var item in list)
                {
                    LogHelper.WriteDebug("用户手动提交的结算价：合约代码=" + item.CommodityCode + "，交易日期=" + item.TradingDate + "，今日结算价=" + item.SettlementPrice);
                    dal.Add(item);
                    //recktime = item.TradingDate.ToString();

                }
                //if (string.IsNullOrEmpty(recktime))
                //{
                //    recktime = DateTime.Now.AddDays(-1).Date.ToString("yyyyMMdd");
                //}
                //4.
                //转换成日期 
                string recktime = currentSendDate.ToString();
                recktime = recktime.Substring(0, 4) + "-" + recktime.Substring(4, 2) + "-" + recktime.Substring(6, 2);
                recktime = DateTime.Parse(recktime).ToShortDateString();
                //ScheduleManager.IsStartSuccess = false;//不用延时  //这里不可以不使用延时，因为有时记录太多，所以要使用延时
                Thread th = new Thread(delegate() { FutureOpenCloseProcess.DoFaultRecoveryClose(recktime); });
                th.Start();
                LogHelper.WriteDebug("已经提交故障恢复清算指令！");

            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }

            return true;

        }
        # endregion

        #region 管理员查询根据交易员查询交易员各资金账户相关信息
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
        public List<TradersAccountCapitalInfo> AdminFindTraderCapitalAccountInfo(string adminId, string adminPassword, string traderId, out string strErrorMessage)
        {
            AdministratorFindTraderBLL bll = new AdministratorFindTraderBLL();
            return bll.AdminFindTraderCapitalAccountInfoByID(adminId, adminPassword, traderId, out strErrorMessage);
        }
        #endregion

        #region  == 求现货最大委托量 ==

        /// <summary>
        /// 将单位转换为撮合单位，并返回比例
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="oriUnitType">原始单位</param>
        /// <param name="scale">比例</param>
        /// <param name="matchUnitType">撮合单位</param>
        /// <returns></returns>
        private bool ConvertUnitType(string code, GTA.VTS.Common.CommonObject.Types.UnitType oriUnitType, out decimal scale, out GTA.VTS.Common.CommonObject.Types.UnitType matchUnitType)
        {
            bool result = false;
            try
            {
                //获取撮合单位（行情单位）
                matchUnitType = MCService.GetMatchUnitType(code);
                if (matchUnitType == oriUnitType)
                {
                    scale = 1;
                    return true;
                }

                var breedClass = MCService.CommonPara.GetBreedClassIdByCommodityCode(code);
                scale = MCService.CommonPara.GetUnitConversionByDetailUnits(breedClass.Value, (int)oriUnitType,
                                                                                (int)matchUnitType);

                result = true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);

                scale = 1;
                matchUnitType = oriUnitType;
            }
            return result;
        }


        /// <summary>
        /// 求最大委托量
        /// </summary>
        /// <param name="TraderId">交易员ID</param>
        /// <param name="OrderPrice">委托价格</param>
        /// <param name="Code">商品代码</param>
        /// <param name="outMessage"></param>
        /// <param name="orderPriceType"></param>
        /// <returns></returns>
        public long GetSpotMaxOrderAmount(string TraderId, float OrderPrice, string Code, out string outMessage,
                                          Entity.Contants.Types.OrderPriceType orderPriceType)
        {
            long getSpotMaxOrderAmount = 0;//计算出的现货最大委托量
            string format = "求现货最大委托量[TradeID={0},OrderPrice={1},Code={2},OrderPriceType={3}]-错误信息：";
            string format1 = "求现货最大委托量[TradeID={0},OrderPrice={1},Code={2},OrderPriceType={3},getSpotOrderAmount={4}]-现货最大委托量相关信息：";
            string desc = string.Format(format, TraderId, OrderPrice, Code, orderPriceType);

            outMessage = string.Empty;
            try
            {
                #region 参数判断
                if (TraderId == string.Empty || OrderPrice == float.MaxValue || OrderPrice < 0 || Code == string.Empty)
                {
                    outMessage = "参数有误";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }
                #endregion

                #region 市价委托计算委托价格
                if (orderPriceType == Entity.Contants.Types.OrderPriceType.OPTMarketPrice)
                {
                    decimal orderPriceD = (decimal)OrderPrice;
                    var highLowRange = MCService.HLRangeProcessor.GetHighLowRangeValueByCommodityCode(Code,
                                                                                                      orderPriceD);
                    if (highLowRange != null)
                    {
                        if (highLowRange.RangeType == Types.HighLowRangeType.HongKongPrice) //港股类型处理
                        {
                            var hkrv = highLowRange.HongKongRangeValue;
                            OrderPrice = Convert.ToSingle(hkrv.BuyHighRangeValue);
                        }
                        else //其它类型处理
                        {
                            OrderPrice = Convert.ToSingle(highLowRange.HighRangeValue);
                        }
                    }
                    else
                    {
                        outMessage = "获取涨贴幅范围失败！";
                        LogHelper.WriteDebug(desc + outMessage);
                        return 0;
                    }
                }
                #endregion

                #region 获取商品类别 及商品交易账号类型
                CM_Commodity commodity = MCService.CommonPara.GetCommodityByCommodityCode(Code);
                if (commodity == null)
                {
                    outMessage = "获取商品失败！";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }

                CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByBreedClassID(commodity.BreedClassID.Value);
                if (breedClass == null)
                {
                    outMessage = "获取商品类别失败！";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }

                if (!breedClass.AccountTypeIDFund.HasValue)
                {
                    outMessage = "获取商品类别的AccountTypeIDFund失败！";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }

                int accountTypeIDFunc = (int)breedClass.AccountTypeIDFund;
                #endregion

                #region 获取用户数据

                #region 从缓存中获取用户数据
                UA_UserAccountAllocationTableInfo UserAccountAllocationTable = AccountManager.Instance.GetAccountByUserIDAndAccountType(TraderId, accountTypeIDFunc);
                #endregion

                #region 从数据库中获取数据
                //UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
                ////List<UA_UserAccountAllocationTableInfo> accountList = DataRepository.UaUserAccountAllocationTableProvider.Find(string.Format("UserID={0} AND AccountTypeLogo={1}", TraderId, accountTypeIDFunc));
                //List<UA_UserAccountAllocationTableInfo> UserAccountAllocationTable = dal.GetListArray(string.Format(" UserID={0} AND AccountTypeLogo={1}", TraderId, accountTypeIDFunc));
                #endregion

                if (UserAccountAllocationTable == null)
                {
                    outMessage = "获取现货资金帐号失败";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }

                string account = UserAccountAllocationTable.UserAccountDistributeLogo;
                #endregion

                #region 获取商品交易货币类型用交易规则
                int currencyTypeID =
                    MCService.SpotTradeRules.GetCurrencyTypeByBreedClassID(breedClass.BreedClassID).
                        CurrencyTypeID;

                XH_SpotTradeRules rules =
                    MCService.SpotTradeRules.GetSpotTradeRulesByBreedClassID(breedClass.BreedClassID);
                if (rules == null)
                {
                    outMessage = "获取现货交易规则失败";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }

                Types.UnitType unit = (Types.UnitType)rules.PriceUnit;
                #endregion

                #region 获取用户当前商品交易货币类型资金账信息
                XH_CapitalAccountTableInfo xhCapitalAccount = MemoryDataManager.XHCapitalMemoryList.GetByCapitalAccountAndCurrencyType(account, currencyTypeID).Data;
                //DataRepository.XhCapitalAccountTableProvider.Find(string.Format("TradeCurrencyType='{0}' AND UserAccountDistributeLogo='{1}'", currencyTypeID, account));
                if (xhCapitalAccount == null)
                {
                    outMessage = "获取现货资金帐号实体失败";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }

                if (xhCapitalAccount.AvailableCapital <= 0)
                {
                    outMessage = "可用资金为零";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }
                #endregion

                float OrderAmount = (float)xhCapitalAccount.AvailableCapital / OrderPrice;

                XHCostResult xhCostResult = MCService.ComputeXHCost(Code, OrderPrice, (int)OrderAmount, unit,
                                                                    Types.TransactionDirection.Buying);
                if (xhCostResult == null)
                {
                    outMessage = "无法计算交易费用";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }
                decimal scale;
                GTA.VTS.Common.CommonObject.Types.UnitType matchUnitType;
                bool canConvert = ConvertUnitType(Code, unit, out scale, out matchUnitType);
                if (!canConvert)
                {
                    outMessage = "无法进行行情成交单位换算";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }
                OrderAmount = (float)(xhCapitalAccount.AvailableCapital - xhCostResult.CoseSum) / OrderPrice;
                OrderAmount = OrderAmount * (float)scale;
                //return ((long)OrderAmount / 100) * 100;
                getSpotMaxOrderAmount = (long)OrderAmount;
                string descSpotMaxOrderAmount = string.Format(format1, TraderId, OrderPrice, Code, orderPriceType, getSpotMaxOrderAmount);
                LogHelper.WriteDebug(descSpotMaxOrderAmount);
                return getSpotMaxOrderAmount;
            }
            catch (Exception ex)
            {
                outMessage = desc + "求现货最大委托量失败,原因为：" + ex.Message;
                LogHelper.WriteError(outMessage, ex);
                return 0;
            }
        }

        #endregion

        #region 求期货最大委托量
        /// <summary>
        /// 求期货最大委托量(股指期货和商品期货都用此方法)
        /// </summary>
        /// <param name="TraderId">交易员ID</param>
        /// <param name="OrderPrice">委托价格</param>
        /// <param name="Code">代码</param>
        /// <param name="outMessage">返回信息</param>
        /// <param name="orderPriceType">价格类型</param>
        /// <returns></returns>
        public long GetFutureMaxOrderAmount(string TraderId, float OrderPrice, string Code, out string outMessage,
                                            Entity.Contants.Types.OrderPriceType orderPriceType)
        {
            outMessage = string.Empty;
            long getFutureMaxOrderAmount = 0;//计算出的期货最大委托量
            string format = "求期货最大委托量[TradeID={0},OrderPrice={1},Code={2},OrderPriceType={3}]-错误信息：";
            string format1 = "求期货最大委托量[TradeID={0},OrderPrice={1},Code={2},OrderPriceType={3},getFutureMaxOrderAmount={4}]-期货最大委托量相关信息：";
            string desc = string.Format(format, TraderId, OrderPrice, Code, orderPriceType);

            //可以购买的数量
            float OrderAmount = 0; //
            try
            {
                #region 参数判断
                if (TraderId == string.Empty || OrderPrice == float.MaxValue || OrderPrice < 0 || Code == string.Empty)
                {
                    outMessage = "参数有误";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }
                #endregion

                #region 市价委托计算委托价格
                if (orderPriceType == Entity.Contants.Types.OrderPriceType.OPTMarketPrice)
                {
                    decimal orderPriceD = (decimal)OrderPrice;
                    var highLowRange = MCService.HLRangeProcessor.GetHighLowRangeValueByCommodityCode(Code,
                                                                                                      orderPriceD);
                    if (highLowRange != null)
                    {
                        if (highLowRange.RangeType == Types.HighLowRangeType.HongKongPrice) //港股类型处理
                        {
                            var hkrv = highLowRange.HongKongRangeValue;
                            OrderPrice = Convert.ToSingle(hkrv.BuyHighRangeValue);
                        }
                        else //其它类型处理
                        {
                            OrderPrice = Convert.ToSingle(highLowRange.HighRangeValue);
                        }
                    }
                    else
                    {
                        outMessage = "获取涨贴幅范围失败！";
                        LogHelper.WriteDebug(desc + outMessage);
                        return 0;
                    }
                }
                #endregion

                #region 获取商品类别 及商品交易账号类型
                CM_Commodity commodity = MCService.CommonPara.GetCommodityByCommodityCode(Code);
                if (commodity == null)
                {
                    outMessage = "获取商品失败！";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }

                CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByBreedClassID(commodity.BreedClassID.Value);
                if (breedClass == null)
                {
                    outMessage = "获取商品类别失败！";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }

                if (!breedClass.AccountTypeIDFund.HasValue)
                {
                    outMessage = "获取商品类别的AccountTypeIDFund失败！";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }

                int accountTypeIDFunc = (int)breedClass.AccountTypeIDFund;
                #endregion

                //TList<UA_UserAccountAllocationTable> UserAccountAllocationTable = DataRepository.UaUserAccountAllocationTableProvider.Find(string.Format("UserID={0} AND AccountTypeLogo={1}", TraderId, accountTypeIDFunc));
                #region 获取用户数据

                #region 从缓存中获取用户数据
                UA_UserAccountAllocationTableInfo UserAccountAllocationTable = AccountManager.Instance.GetAccountByUserIDAndAccountType(TraderId, accountTypeIDFunc);
                #endregion

                #region 从数据库中获取数据
                //UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
                //List<UA_UserAccountAllocationTableInfo> UserAccountAllocationTable = dal.GetListArray(string.Format(" UserID={0} AND AccountTypeLogo={1}", TraderId, accountTypeIDFunc));
                #endregion

                if (UserAccountAllocationTable == null)
                {
                    outMessage = "获取期货资金帐号失败";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }
                string account = UserAccountAllocationTable.UserAccountDistributeLogo;
                #endregion

                #region 获取商品交易货币类型用交易规则
                int currencyTypeID =
                    MCService.FuturesTradeRules.GetCurrencyTypeByBreedClassID(breedClass.BreedClassID).
                        CurrencyTypeID;

                QH_FuturesTradeRules qhFuturesTradeRules =
                    MCService.FuturesTradeRules.GetFuturesTradeRulesByBreedClassID(breedClass.BreedClassID);
                if (qhFuturesTradeRules == null)
                {
                    outMessage = "获取期货交易规则失败";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }

                Types.UnitType unit = (Types.UnitType)qhFuturesTradeRules.PriceUnit;
                #endregion

                //TList<QhCapitalAccountTable> qhCapitalAccountTables = DataRepository.QhCapitalAccountTableProvider.Find(string.Format("TradeCurrencyType='{0}' AND UserAccountDistributeLogo='{1}'", currencyTypeID, account));

                #region 获取用户当前商品交易货币类型资金账信息
                QH_CapitalAccountTableInfo qhCapitalAccount = MemoryDataManager.QHCapitalMemoryList.GetByCapitalAccountAndCurrencyType(account, currencyTypeID).Data;

                if (qhCapitalAccount == null)
                {
                    outMessage = "获取期货资金帐号实体失败";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }

                if (qhCapitalAccount.AvailableCapital <= 0)
                {
                    outMessage = "可用资金为零";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }
                #endregion

                #region 计算相关费用
                //计价单位与交易单位倍数
                decimal unitMultiple = MCService.GetTradeUnitScale(Code, (Types.UnitType)Enum.Parse(typeof(Types.UnitType), unit.ToString()));

                //保证金比例
                decimal futureBail = MCService.GetFutureBailScale(Code) / 100;

                //当前需要支付的费用=当前委托价格*计价单位与交易单位倍数*保证金比例
                decimal curPayMent = Convert.ToDecimal(OrderPrice) * unitMultiple * futureBail;
                //可以购买的数量=总资金/当前需要支付的费用
                OrderAmount = (float)qhCapitalAccount.AvailableCapital / ((float)curPayMent);
                QHCostResult qhCostResult = MCService.ComputeGZQHCost(Code, OrderPrice, (int)OrderAmount, unit,
                                                                      Entity.Contants.Types.FutureOpenCloseType.
                                                                          OpenPosition);
                if (qhCostResult == null)
                {
                    outMessage = "无法计算交易费用";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }

                //最大委托量=(总资金-此委托需要的总费用)/当前需要支付的费用
                OrderAmount = (float)(qhCapitalAccount.AvailableCapital - qhCostResult.Cosing) / ((float)curPayMent);
                #endregion

                //根据最大委托量ID，返回单笔委托量集合
                IList<QH_SingleRequestQuantity> singleRequestQuantityList =
                    MCService.FuturesTradeRules.GetSingleRequestQuantityByConsignQuantumID(
                        (int)qhFuturesTradeRules.ConsignQuantumID);
                if (singleRequestQuantityList == null)
                {
                    outMessage = "无法获取单笔委托量记录";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }

                QH_SingleRequestQuantity limit_SingleRequestQuantity = null;
                QH_SingleRequestQuantity market_SingleRequestQuantity = null;

                foreach (QH_SingleRequestQuantity singleRequestQuantity in singleRequestQuantityList)
                {
                    if (singleRequestQuantity.ConsignInstructionTypeID.HasValue)
                    {
                        int val = singleRequestQuantity.ConsignInstructionTypeID.Value;
                        if (val == (int)Entity.Contants.Types.OrderPriceType.OPTLimited)
                            limit_SingleRequestQuantity = singleRequestQuantity;
                        else if (val == (int)Entity.Contants.Types.OrderPriceType.OPTMarketPrice)
                            market_SingleRequestQuantity = singleRequestQuantity;
                    }
                }

                //限价委托
                if (orderPriceType == Entity.Contants.Types.OrderPriceType.OPTLimited)
                {
                    if (limit_SingleRequestQuantity == null)
                        return (long)OrderAmount;
                    if (!limit_SingleRequestQuantity.MaxConsignQuanturm.HasValue)
                        return (long)OrderAmount;
                    var maxLimitAmount = limit_SingleRequestQuantity.MaxConsignQuanturm.Value;
                    if (OrderAmount > maxLimitAmount) //计算出的最大委托量>管理中心设置的最大委托量时，则返回管理中心设置的最大委托量
                        return maxLimitAmount;
                }
                else if (orderPriceType == Entity.Contants.Types.OrderPriceType.OPTMarketPrice) //市价委托
                {
                    if (market_SingleRequestQuantity == null)
                        return (long)OrderAmount;
                    if (!market_SingleRequestQuantity.MaxConsignQuanturm.HasValue)
                        return (long)OrderAmount;
                    var maxMarketAmount = market_SingleRequestQuantity.MaxConsignQuanturm.Value;
                    if (OrderAmount > maxMarketAmount) //计算出的最大委托量>管理中心设置的最大委托量时，则返回管理中心设置的最大委托量
                        return maxMarketAmount;
                }
                getFutureMaxOrderAmount = (long)OrderAmount;
                string descFutureMaxOrderAmount = string.Format(format1, TraderId, OrderPrice, Code, orderPriceType, getFutureMaxOrderAmount);
                LogHelper.WriteDebug(descFutureMaxOrderAmount);
                return getFutureMaxOrderAmount;
            }
            catch (Exception ex)
            {
                outMessage = desc + "求期货最大委托量失败,原因为：" + ex.Message;
                //LogHelper.WriteDebug(desc + outMessage);
                LogHelper.WriteError(outMessage, ex);
                return 0;
            }
        }
        #endregion

        #region 根据商品代码和委托价格获取上下限（涨跌幅值）
        /// <summary>
        /// 根据商品代码和委托价格获取上下限（涨跌幅值）
        /// </summary>
        /// <param name="code">现货期货商品代码（原代码表）</param>
        /// <param name="orderPrice">委托价格</param>
        /// <returns></returns>
        public HighLowRangeValue GetHighLowRangeValueByCommodityCode(string code, decimal orderPrice)
        {
            HighLowRangeValue hl = null;
            try
            {
                hl = MCService.HLRangeProcessor.GetHighLowRangeValueByCommodityCode(code, orderPrice);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return hl;
        }
        #endregion


        #region  == 求港股最大委托量 ==

        /// <summary>
        /// 港股将单位转换为撮合单位，并返回比例
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="oriUnitType">原始单位</param>
        /// <param name="scale">比例</param>
        /// <param name="matchUnitType">撮合单位</param>
        /// <returns></returns>
        private bool ConvertHKUnitType(string code, GTA.VTS.Common.CommonObject.Types.UnitType oriUnitType, out decimal scale, out GTA.VTS.Common.CommonObject.Types.UnitType matchUnitType)
        {
            bool result = false;
            try
            {
                //获取撮合单位（行情单位）
                matchUnitType = MCService.GetMatchUnitType(code, Types.BreedClassTypeEnum.HKStock);
                if (matchUnitType == oriUnitType)
                {
                    scale = 1;
                    return true;
                }
                scale = MCService.CommonPara.GetHKUnitConversionByDetailUnits(code, oriUnitType, matchUnitType);

                result = true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                scale = 1;
                matchUnitType = oriUnitType;
            }
            return result;
        }
        /// <summary>
        /// 求港股最大委托量
        /// </summary>
        /// <param name="TraderId">交易员ID</param>
        /// <param name="OrderPrice">委托价格</param>
        /// <param name="Code">商品代码</param>
        /// <param name="outMessage">返回信息</param>
        /// <param name="orderPriceType">港股价格类型(限价盘,增强限价盘,特别限价盘)</param>
        /// <returns></returns>
        public long GetHKMaxOrderAmount(string TraderId, float OrderPrice, string Code, out string outMessage, Types.HKPriceType orderPriceType)
        {
            long getHKMaxOrderAmount = 0;//计算出的现货最大委托量
            string format = "求港股最大委托量[TradeID={0},OrderPrice={1},Code={2},OrderPriceType={3}]-错误信息：";
            string format1 = "求港股最大委托量[TradeID={0},OrderPrice={1},Code={2},OrderPriceType={3},getSpotOrderAmount={4}]-港股最大委托量相关信息：";
            string desc = string.Format(format, TraderId, OrderPrice, Code, orderPriceType);

            outMessage = string.Empty;
            //（代修改）
            try
            {
                #region 参数判断
                if (TraderId == string.Empty || OrderPrice == float.MaxValue || OrderPrice < 0 || Code == string.Empty)
                {
                    outMessage = "参数有误";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }
                #endregion

                #region 委托计算委托价格
                switch (orderPriceType)
                {
                    case Types.HKPriceType.LO:
                        break;
                    case Types.HKPriceType.ELO:
                        break;
                    case Types.HKPriceType.SLO:
                        break;
                    default:
                        break;
                }
                #endregion

                #region 获取商品类别 及商品交易账号类型
                HK_Commodity commodity = MCService.HKTradeRulesProxy.GetHKCommodityByCommodityCode(Code);
                if (commodity == null)
                {
                    outMessage = "获取港股商品失败！";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }

                CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByBreedClassID(commodity.BreedClassID.Value);
                if (breedClass == null)
                {
                    outMessage = "获取商品类别失败！";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }

                if (!breedClass.AccountTypeIDFund.HasValue)
                {
                    outMessage = "获取商品类别的AccountTypeIDFund失败！";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }

                int accountTypeIDFunc = (int)breedClass.AccountTypeIDFund;
                #endregion

                #region 获取用户数据

                #region 从缓存中获取用户数据
                UA_UserAccountAllocationTableInfo UserAccountAllocationTable = AccountManager.Instance.GetAccountByUserIDAndAccountType(TraderId, accountTypeIDFunc);
                #endregion

                if (UserAccountAllocationTable == null)
                {
                    outMessage = "获取港股资金帐号失败";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }

                string account = UserAccountAllocationTable.UserAccountDistributeLogo;
                #endregion

                #region 获取商品交易货币类型用交易规则
                CM_CurrencyType cuType = MCService.CommonPara.GetCurrencyTypeByBreedClassID(breedClass.BreedClassID);
                if (cuType == null)
                {
                    outMessage = "根据BreedClassID无法获取交易费用所属货币类型";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }
                int currencyTypeID = cuType.CurrencyTypeID;

                HK_SpotTradeRules rules = MCService.HKTradeRulesProxy.GetSpotTradeRulesByBreedClassID(breedClass.BreedClassID);
                if (rules == null)
                {
                    outMessage = "获取港股交易规则失败";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }

                Types.UnitType unit = (Types.UnitType)rules.PriceUnit;
                #endregion

                #region 获取用户当前商品交易货币类型资金账信息
                HK_CapitalAccountInfo hkCapitalAccount = MemoryDataManager.HKCapitalMemoryList.GetByCapitalAccountAndCurrencyType(account, currencyTypeID).Data;
                if (hkCapitalAccount == null)
                {
                    outMessage = "获取港股资金帐号实体失败";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }

                if (hkCapitalAccount.AvailableCapital <= 0)
                {
                    outMessage = "可用资金为零";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }
                #endregion

                float OrderAmount = (float)hkCapitalAccount.AvailableCapital / OrderPrice;

                HKCostResult hkCostResult = MCService.ComputeHKCost(Code, OrderPrice, (int)OrderAmount, unit,
                                                                    Types.TransactionDirection.Buying);
                if (hkCostResult == null)
                {
                    outMessage = "无法计算交易费用";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }
                decimal scale;
                GTA.VTS.Common.CommonObject.Types.UnitType matchUnitType;
                bool canConvert = ConvertHKUnitType(Code, unit, out scale, out matchUnitType);
                if (!canConvert)
                {
                    outMessage = "无法进行行情成交单位换算";
                    LogHelper.WriteDebug(desc + outMessage);
                    return 0;
                }
                OrderAmount = (float)(hkCapitalAccount.AvailableCapital - hkCostResult.CoseSum) / OrderPrice;
                OrderAmount = OrderAmount * (float)scale;
                getHKMaxOrderAmount = (long)OrderAmount;
                string descSpotMaxOrderAmount = string.Format(format1, TraderId, OrderPrice, Code, orderPriceType, getHKMaxOrderAmount);
                LogHelper.WriteDebug(descSpotMaxOrderAmount);
                return getHKMaxOrderAmount;
            }
            catch (Exception ex)
            {
                outMessage = desc + "求港股最大委托量失败,原因为：" + ex.Message;
                LogHelper.WriteError(outMessage, ex);
                return 0;
            }

        }

        #endregion

        #region 根据港股商品代码和委托价格获取上下限（涨跌幅值）
        /// <summary>
        /// 根据商品代码和委托价格获取上下限（涨跌幅值）
        /// </summary>
        /// <param name="code">港股商品代码</param>
        /// <param name="orderPrice">委托价格</param>
        /// <param name="priceType">港股价格类型(限价盘,增强限价盘,特别限价盘)</param>
        /// <param name="tranType">交易方向</param>
        /// <returns></returns>
        public HighLowRangeValue GetHKHighLowRangeValueByCommodityCode(string code, decimal orderPrice, Types.HKPriceType priceType, Types.TransactionDirection tranType)
        {
            HighLowRangeValue hl = null;
            try
            {
                hl = MCService.HLRangeProcessor.GetHKStockHighLowRangeValueByCommodityCode(code, orderPrice, priceType, tranType);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return hl;
        }
        #endregion


        #region 根据代码和代码品种类型查询当前行情

        /// <summary>
        /// Title:根据代码和代码品种类型查询当前行情
        /// Desc.:因目前行情件同时加载多个服务点用CPU使用率，所以为了开启此方法用于内部测试启动测试端不用加载
        ///        行情组件接口，而提供此方法获取当前行情
        /// Create By:李健华
        /// Create Date:2009-11-08
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="breedClassType">所属商品类型（1-现货,2-商品期货,3-股指期货,4-港股)</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns></returns>
        public MarketDataLevel GetMarketDataInfoByCode(string code, int breedClassType, out string errMsg)
        {
            return RealTimeMarketUtil.GetInstance().GetLastPriceByCode(code, breedClassType, out errMsg);
        }

        #endregion

        #region 根据品种类别获取相关所有柜台缓存的当前所有代码
        /// <summary>
        /// 根据品种类别获取相关所有柜台缓存的当前所有代码
        /// </summary>
        /// <param name="classTypeID">品种类型 现货-1, 商品期货--2, 股指期货--3, 港股--4</param>
        /// <param name="isRemoveExpired">是否排除期货过期代码</param>
        /// <returns>返回相关的所有品种类型代码</returns>
        public List<string> GetAllCM_CommodityByBreedClassTypeID(int classTypeID, bool isRemoveExpired)
        {
            List<string> codeList = new List<string>();
            IList<CM_BreedClass> bcList = MCService.CommonPara.GetAllBreedClass();
            try
            {

                switch ((Types.BreedClassTypeEnum)classTypeID)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        #region 现货
                        List<int> stock = new List<int>();
                        foreach (var item in bcList)
                        {
                            if (item.BreedClassTypeID.Value == (int)Types.BreedClassTypeEnum.Stock)
                            {
                                stock.Add(item.BreedClassID);
                            }
                        }
                        IList<CM_Commodity> list = MCService.CommonPara.GetAllCommodity();
                        foreach (var item in list)
                        {
                            if (stock.Contains(item.BreedClassID.Value))
                            {
                                codeList.Add(item.CommodityCode);
                            }
                        }
                        #endregion
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        #region 商品期货
                        List<int> commodityFuture = new List<int>();
                        foreach (var item in bcList)
                        {
                            if (item.BreedClassTypeID.Value == (int)Types.BreedClassTypeEnum.CommodityFuture)
                            {
                                commodityFuture.Add(item.BreedClassID);
                            }
                        }
                        IList<CM_Commodity> listf = MCService.CommonPara.GetAllCommodity();
                        foreach (var item in listf)
                        {
                            if (commodityFuture.Contains(item.BreedClassID.Value))
                            {
                                if (!isRemoveExpired || item.IsExpired != 1)
                                {
                                    codeList.Add(item.CommodityCode);
                                }
                            }
                        }
                        #endregion
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        #region 股指期货
                        List<int> stockIndexFuture = new List<int>();
                        foreach (var item in bcList)
                        {
                            if (item.BreedClassTypeID.Value == (int)Types.BreedClassTypeEnum.StockIndexFuture)
                            {
                                stockIndexFuture.Add(item.BreedClassID);
                            }
                        }
                        IList<CM_Commodity> listfs = MCService.CommonPara.GetAllCommodity();
                        foreach (var item in listfs)
                        {
                            if (stockIndexFuture.Contains(item.BreedClassID.Value))
                            {
                                if (!isRemoveExpired || item.IsExpired != 1)
                                {
                                    codeList.Add(item.CommodityCode);
                                }
                            }
                        }
                        #endregion
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        #region 港股
                        List<int> hkStock = new List<int>();
                        foreach (var item in bcList)
                        {
                            if (item.BreedClassTypeID.Value == (int)Types.BreedClassTypeEnum.HKStock)
                            {
                                hkStock.Add(item.BreedClassID);
                            }
                        }
                        IList<HK_Commodity> listhk = MCService.HKTradeRulesProxy.GetAllHKCommodity();
                        foreach (var item in listhk)
                        {
                            if (hkStock.Contains(item.BreedClassID.Value))
                            {
                                codeList.Add(item.HKCommodityCode);
                            }
                        }
                        #endregion
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteError("获取柜台内部缓存的代码异常", ex);
            }
            return codeList;
        }
        #endregion

        #region 管理员设置对资金个性化设置操作
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
        /// <param name="admin">管理员ID</param>
        /// <param name="pwd">管理员密码</param>
        /// <param name="errMsg">操作异常信息</param>
        /// <returns></returns>
        public bool AdminPersonalizationCapital(CapitalPersonalization model, out string errMsg)
        {

            if (model.TradeID.Count > 20)
            {
                errMsg = "选择的交易员数量过多，请控制在20个以内！";
                return false;
            }
            LogHelper.WriteDebug("管理员设置对资金个性化设置操作");
            var capitalManagementBLL = new CapitalManagementBLL();
            return capitalManagementBLL.PersonalizationCapital(model, out errMsg);
        }
        #endregion

        #region 管理员执行试玩期后用户交易数据清空操作

        /// <summary>
        /// Title:管理员执行试玩期后用户交易数据清空操作
        /// Desc.:管理员执行试玩期后用户交易数据清空操作
        /// Create by:董鹏
        /// Create Date:2009-12-23
        /// </summary>
        /// <param name="model">要设置的交易员资金实体</param>
        /// <param name="errMsg">操作异常信息</param>
        /// <returns>是否执行成功</returns>
        public bool AdminClearTrialData(CapitalPersonalization model, out string errMsg)
        {
            //if (model.TradeID.Count > 20)
            //{
            //    errMsg = "选择的交易员数量过多，请控制在20个以内！";
            //    return false;
            //}
            var capitalManagementBLL = new CapitalManagementBLL();
            return capitalManagementBLL.ClearTrialData(model, out errMsg);
        }

        #endregion

        #region  获取当前所有持仓中要提供当日结算价清算的代码
        /// <summary>
        /// 获取当前所有持仓中要提供当日结算价清算的代码
        /// 如果返回为null的话即不用做故障恢复，但如果为list.count=0或者>0即要做故障恢复
        /// </summary>
        /// <param name="errMsg">查询返回异常或者提示信息</param>
        /// <returns></returns>
        public List<QH_TodaySettlementPriceInfo> GetAllReckoningHoldCode(out string errMsg)
        {
            errMsg = "";
            try
            {
                if (ScheduleManager.IsFaultRecoveryFutureReckoning)
                {
                    errMsg = "系统请在执行故障恢复清算，请稍后再查询!";
                    return null;
                }
                DateTime ReckoningDateTime;
                bool isReckoning = false;
                isReckoning = StatusTableChecker.IsFutureReckoningFaultRecovery(out ReckoningDateTime, out errMsg);

                if (!isReckoning)
                {
                    return null;
                }

                List<QH_TodaySettlementPriceInfo> list = new List<QH_TodaySettlementPriceInfo>();
                QH_HoldAccountTableDal dal = new QH_HoldAccountTableDal();
                List<QH_HoldAccountTableInfo> models = dal.GetAllListArray();
                decimal price = 0;
                foreach (var item in models)
                {

                    QH_TodaySettlementPriceInfo tsp = new QH_TodaySettlementPriceInfo();
                    int? breedtype = MCService.CommonPara.GetBreedClassTypeEnumByCommodityCode(item.Contract);
                    price = 0;
                    if (breedtype.HasValue)
                    {
                        switch ((Types.BreedClassTypeEnum)breedtype)
                        {
                            case Types.BreedClassTypeEnum.Stock:
                                break;
                            case Types.BreedClassTypeEnum.CommodityFuture:
                                MerFutData cfdata = RealTimeMarketUtil.GetRealMarketService().GetMercantileFutData(item.Contract);
                                if (cfdata != null)
                                {
                                    if (ReckoningDateTime.Date == DateTime.Now.Date)
                                    {
                                        price = (decimal)cfdata.ClearPrice;
                                    }
                                    else
                                    {
                                        price = (decimal)cfdata.PreClearPrice;
                                    }
                                }
                                break;
                            case Types.BreedClassTypeEnum.StockIndexFuture:
                                FutData qhdata = RealTimeMarketUtil.GetRealMarketService().GetFutData(item.Contract);
                                if (qhdata != null)
                                {

                                    if (ReckoningDateTime.Date == DateTime.Now.Date)
                                    {
                                        price = (decimal)qhdata.SettlementPrice;
                                    }
                                    else
                                    {
                                        price = (decimal)qhdata.PreSettlementPrice;
                                    }

                                }

                                break;
                            case Types.BreedClassTypeEnum.HKStock:
                                break;
                            default:
                                break;
                        }

                    }
                    tsp.CommodityCode = item.Contract;
                    tsp.SettlementPrice = price;
                    tsp.TradingDate = int.Parse(ReckoningDateTime.ToString("yyyyMMdd"));
                    list.Add(tsp);
                }
                return list;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return null;
            }
        }
        #endregion

    }
}