#region Using Namespace

using System;
using System.Data.Common;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.DelegateValidate.Cost;
using ReckoningCounter.BLL.DelegateValidate.Local;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.BLL.Reckoning.Logic;
using ReckoningCounter.DAL.CustomDataAccess;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.Entity;
using ReckoningCounter.MemoryData;
using ReckoningCounter.MemoryData.QH.Hold;
using ReckoningCounter.Model;

#endregion

namespace ReckoningCounter.BLL.DelegateAcceptOffer.OrderAccepter.Logic
{
    /// <summary>
    /// 股指期货下单预处理逻辑,，错误编码2300-2333
    /// </summary>
    public class GZQHAcceptLogic :
        AcceptLogic<StockIndexFuturesOrderRequest, QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo>
    {
        ////是否是强制平仓的过期合约生成的委托
        //protected bool IsExpiredContract { get; set; }

        ////是否是期货开盘检查中的资金检查Order
        //protected bool IsCheckCapitalOrder { get; set; }

        //是否是盘前检查中的强行平仓Order
        protected bool IsForcedCloseOrder { get; set; }

        #region CancelOrderValidate 2300-2305

        protected override CounterCache GetCounterCache()
        {
            return QHCounterCache.Instance;
        }

        /// <summary>
        /// 撤单校验-检查委托单当前状态是否可撤
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        /// <param name="tet">委托实体</param>
        /// <param name="strMessage">错误信息</param>
        /// <returns>校验是否通过</returns>
        public override bool CancelOrderValidate(string entrustNumber, out QH_TodayEntrustTableInfo tet,
                                                 ref string strMessage)
        {
            var result = false;
            tet = QHDataAccess.GetEntrustTable(entrustNumber);
            if (tet != null)
            {
                if (tet.OrderStatusId == (int)Entity.Contants.Types.OrderStateType.DOSUnRequired ||
                    tet.OrderStatusId == (int)Entity.Contants.Types.OrderStateType.DOSRequiredSoon ||
                    tet.OrderStatusId == (int)Entity.Contants.Types.OrderStateType.DOSIsRequired ||
                    tet.OrderStatusId == (int)Entity.Contants.Types.OrderStateType.DOSPartDealed)
                {
                    result = true;
                }
                else
                {
                    //strMessage = "委托单状态不允许撤单"; 
                    strMessage = "GT-2300:[股指期货撤单校验]该委托状态的委托单不可撤.委托状态=" +
                                 Entity.Contants.Types.GetOrderStateMsg(tet.OrderStatusId);
                }
            }
            else
            {
                //strMessage = "委托单不存在"; 
                strMessage = "GT-2301:[股指期货撤单校验]委托单不存在.";
            }
            return result;
        }

        #endregion

        #region 内部撤单（原DelegateOffer_CancelOrder）2306-2310

        /// <summary>
        /// 内部撤单
        /// </summary>
        /// <param name="tet">委托实体</param>
        /// <param name="strMcErrorMessage">错误信息</param>
        /// <returns>是否成功</returns>
        public override bool InternalCancelOrder(QH_TodayEntrustTableInfo tet, string strMcErrorMessage)
        {
            //柜台委托单号
            EntrustNumber = tet.EntrustNumber;

            string errInfo = "[委托单号=" + EntrustNumber + ",Message=" + strMcErrorMessage + "]";
            LogHelper.WriteDebug(
                "------xxxxxx------开始进行股指期货内部撤单(撤单时委托状态为未报或者买卖报盘返回的委托单号无效)GZQHAcceptLogic.InternalCancelOrder" +
                errInfo);

            Code = tet.ContractCode;

            //资金帐户
            CapitalAccount = tet.CapitalAccount;
            //持仓帐户
            HoldingAccount = tet.TradeAccount;

            var buySellType = (Types.TransactionDirection)tet.BuySellTypeId;
            //GetAccountId(CapitalAccount, HoldingAccount, buySellType);

            //GetCurrencyType();

            var user = AccountManager.Instance.GetUserByAccount(tet.CapitalAccount);
            if (user != null)
            {
                TradeID = user.UserID;
            }
            //else
            //{
            //    TradeID = counterCacher.GetTraderIdByFundAccount(tet.CapitalAccount);
            //}

            //因为1.1结构修改，内部撤单和外部撤单逻辑已经一致，所以统一使用代码
            //内部撤单也作为一个“特殊”的外部撤单来处理，即柜台自己造一个“撤单回报”
            //发送给ReckonUnit来处理

            //假的撮合委托单号
            string orderNo = Guid.NewGuid().ToString();

            tet.McOrderId = orderNo;
            //假的撮合委托单号也要写入委托信息中，以便后面好删除缓存
            QHDataAccess.UpdateEntrustTable(tet);
            LogHelper.WriteDebug("GZQHAcceptLogic.InternalCancelOrder添加委托单号映射到ConterCache,OrderNo=" +
                                 orderNo);
            //柜台缓存委托相关信息
            var item = new OrderCacheItem(tet.CapitalAccount, tet.TradeAccount,
                                          tet.EntrustNumber,
                                          (
                                          Types.TransactionDirection)
                                          Enum.Parse(
                                              typeof(
                                                  Types.TransactionDirection),
                                              tet.BuySellTypeId.ToString
                                                  ()));
            item.EntrustAmount = tet.EntrustAmount;
            item.Code = tet.ContractCode;
            item.TraderId = TradeID;

            counterCacher.AddOrderMappingInfo(orderNo,
                                                      item);

            CancelOrderEntityEx coe = new CancelOrderEntityEx();
            coe.IsInternalCancelOrder = true;
            coe.OrderNo = orderNo;
            coe.IsSuccess = true;
            coe.OrderVolume = tet.EntrustAmount - tet.TradeAmount - tet.CancelAmount;
            coe.Id = Guid.NewGuid().ToString();
            coe.Message = strMcErrorMessage;

            ReckonCenter.Instace.AcceptCancelGZQHOrderRpt(coe);

            //内部撤单流程
            //1.资金冻结处理，把资金冻结记录里的金额和费用全部还给资金表，删除冻结记录(实际上清零，不删除，盘后统一删）
            //2.资金处理，把从资金冻结记录还回来的金额和费用加到可用资金，并减去总冻结资金
            //3.持仓冻结处理(开仓不处理）,把持仓冻结记录中的冻结量还给持仓表，删除冻结记录(实际上清零，不删除，盘后统一删）
            //4.持仓处理（开仓不处理），把从持仓冻结记录还回来的持仓量加到可用持仓，并减去总冻结持仓

            #region 废弃

            /*
            //实际的处理流程

            #region 1.资金冻结处理，获取冻结金额和冻结费用，并获取冻结记录的id

            //冻结金额
            decimal preFreezeCapital = 0;
            //冻结费用
            decimal preFreezeCost = 0;

            //持仓冻结的id
            int holdFreezeLogoId = -1;
            //资金冻结的id 
            int capitalFreezeLogoId = -1;

            var ca = QHDataAccess.GetCapitalAccountFreeze(EntrustNumber, Types.FreezeType.DelegateFreeze);
            if (ca == null)
            {
                string msg = "[股指期货内部撤单]资金冻结记录不存在." + entrustNum;
                LogHelper.WriteInfo(msg);
                //找不到资金冻结，一样允许撤单，当作冻结的资金全部为0
                //CancelOrderFailureProcess(EntrustNumber, 0, 0, 0, strErrorMessage);
                //return false;
            }
            else
            {
                preFreezeCapital = ca.FreezeAmount;
                preFreezeCost = ca.FreezeCost;
                capitalFreezeLogoId = ca.CapitalFreezeLogoId;
            }

            #endregion

            #region 2.资金处理，把从资金冻结记录还回来的金额和费用加到可用资金，并减去总冻结资金

            bool isSuccess = QHCommonLogic.DoCancelOrderCapitalProcess(preFreezeCapital,
                                                                       preFreezeCost, ref strErrorMessage,
                                                                       CapitalAccount, CapitalAccountId);

            if (!isSuccess)
            {
                QHCommonLogic.CancelOrderFailureProcess(EntrustNumber, 0, 0, 0, strErrorMessage, CapitalAccount,
                                                        CapitalAccountId, HoldingAccount, HoldingAccountId,
                                                        Request.OpenCloseType);
                return false;
            }

            #endregion

            decimal preFreezeHoldAmount = 0m;
            if (Request.OpenCloseType ==
                Types.FutureOpenCloseType.OpenPosition)
            {
                #region 3.持仓冻结处理(开仓不计算）,获取持仓冻结记录中的冻结量和冻结id

                var hold = QHDataAccess.GetHoldAccountFreeze(EntrustNumber, Types.FreezeType.DelegateFreeze);
                if (hold == null)
                {
                    string msg = "[股指期货内部撤单]持仓冻结不存在";
                    LogHelper.WriteInfo(msg);
                    //持仓冻结不存在，一样运行撤单，当作持仓冻结量为0
                    //CancelOrderFailureProcess(EntrustNumber, -preFreezeCapital, -preFreezeCost, 0, strErrorMessage);
                    //return false;
                }
                else
                {
                    preFreezeHoldAmount = hold.FreezeAmount;
                    holdFreezeLogoId = hold.HoldFreezeLogoId;
                }

                #endregion

                //4.持仓处理（开仓不计算），把从持仓冻结记录还回来的持仓量加到可用持仓，并减去总冻结持仓
                isSuccess = QHCommonLogic.DoXHReckonHoldProcess(preFreezeHoldAmount, ref strErrorMessage,
                                                                   HoldingAccount, HoldingAccountId,
                                                                   Request.OpenCloseType);
                if (!isSuccess)
                {
                    QHCommonLogic.CancelOrderFailureProcess(EntrustNumber, -preFreezeCapital, -preFreezeCost, 0,
                                                            strErrorMessage, CapitalAccount, CapitalAccountId,
                                                            HoldingAccount, HoldingAccountId, Request.OpenCloseType);
                    return false;
                }
            }

            tet = QHDataAccess.GetTodayEntrustTable(EntrustNumber);
            QHCommonLogic.ProcessCancelOrderStatus(tet);

            tet.CancelAmount = tet.EntrustAmount - tet.TradeAmount;
            tet.OrderMessage = strMcErrorMessage;

            //Clear资金和持仓冻结记录(没有删除)
            try
            {
                DataManager.ExecuteInTransaction((db, transaction) =>
                                                     {
                                                         QHDataAccess.ClearCapitalFreeze(capitalFreezeLogoId, db,
                                                                                         transaction);
                                                         QHDataAccess.ClearHoldFreeze(holdFreezeLogoId, db, transaction);

                                                         ReckoningTransaction rt = new ReckoningTransaction();
                                                         rt.Database = db;
                                                         rt.Transaction = transaction;
                                                         QHDataAccess.UpdateEntrustTable(tet, rt);
                                                     });
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);

                QHCommonLogic.CancelOrderFailureProcess(EntrustNumber, -preFreezeCapital, -preFreezeCost,
                                                        -preFreezeHoldAmount,
                                                        strErrorMessage, CapitalAccount, CapitalAccountId,
                                                        HoldingAccount, HoldingAccountId, Request.OpenCloseType);

                return false;
            }

            ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo> canCleObject =
                new ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo>();
            canCleObject.EntrustTable = tet;
            canCleObject.TradeID = TradeID;
            canCleObject.IsSuccess = true;
            canCleObject.TradeTableList = new List<QH_TodayTradeTableInfo>();

            OnEndCancelEvent(canCleObject);*/

            #endregion

            return true;
        }

        #endregion

        #region PersistentOrder 2311-2339

        private string capitalMesage = "";
        private string holdMessage = "";

        /// <summary>
        /// 检验并持久化柜台委托单
        /// </summary>
        /// <param name="stockorder">原始委托</param>
        /// <param name="strMessage">错误信息</param>
        /// <param name="outEntity">柜台委托</param>
        /// <returns>是否成功</returns>
        public override bool PersistentOrder(StockIndexFuturesOrderRequest stockorder,
                                             ref QH_TodayEntrustTableInfo outEntity, ref string strMessage)
        {
            Request = stockorder;
            Code = stockorder.Code;

            //执行第一次
            bool result = PersistentOrder(ref outEntity, ref strMessage);

            ////执行第二次
            //if (!result)
            //{
            //    LogHelper.WriteDebug("XHAcceptLogic.PersistentOrder执行第二次");
            //    result = PersistentOrder(ref outEntity, ref strMessage);
            //}

            ////执行第三次
            //if (!result)
            //{
            //    LogHelper.WriteDebug("XHAcceptLogic.PersistentOrder执行第三次");
            //    result = PersistentOrder(ref outEntity, ref strMessage);
            //}

            return result;
        }

        /// <summary>
        /// 检验并持久化柜台委托单
        /// </summary>
        /// <param name="strMessage">错误消息</param>
        /// <param name="outEntity">柜台持久化后的对象</param>
        /// <returns>是否成功</returns>
        private bool PersistentOrder(ref QH_TodayEntrustTableInfo outEntity, ref string strMessage)
        {
            #region 初始化参数

            //取代码对应品种的交易币种
            if (!GetCurrencyType())
            {
                strMessage = "GT-2334:[股指期货委托持久化]无法获取交易币种";
                return false;
            }

            if (CurrencyType == -1)
            {
                strMessage = "GT-2334:[股指期货委托持久化]无法获取交易币种";
                return false;
            }

            //预成交金额
            decimal predealCapital = 0;
            //预成交费用
            decimal predealCost = 0;

            //预成交总金额(根据买卖的不同，不一定=predealCapital+predealCost)
            decimal preDealSum = 0;


            //生成的资金冻结id
            int persistCapitalFreezeId = -1;
            //生成的持仓冻结id
            int persistHoldFreezeId = -1;

            //资金及持仓帐户
            string strHoldingAccount;
            string strCapitalAccount;

            //依据交易员及委托信息取对应资金及持仓帐户
            if (!CheckAccount(out strCapitalAccount, out strHoldingAccount, out strMessage))
                return false;

            GetAccountId(strCapitalAccount, strHoldingAccount, Request.BuySell);

            if (CapitalAccountId == -1)
            {
                strMessage = "GT-2335:[股指期货委托持久化]无法获取资金帐号ID";
                return false;
            }

            //冻结类型初始化
            //Entity.Contants.Types.FreezeType ft = Entity.Contants.Types.FreezeType.DelegateFreeze;

            //是否是强制平仓的过期合约生成的委托
            IsForcedCloseOrder = false;

            if (Request is StockIndexFuturesOrderRequest2)
            {
                IsForcedCloseOrder = ((StockIndexFuturesOrderRequest2)Request).IsForcedCloseOrder;
                //IsExpiredContract = ((MercantileFuturesOrderRequest2)Request).IsExpiredContract;
                //IsCheckCapitalOrder = ((MercantileFuturesOrderRequest2)Request).IsCapitalCheckContract;
            }

            #endregion

            #region 检查

            try
            {
                //计算预成交金额和预成交费用
                if (!this.PO_ComputePreCapital(ref strMessage, ref predealCapital, ref predealCost))
                {
                    PO_ValidateFailureProcess(ref strMessage);
                    return false;
                }

                if (predealCapital < 0)
                {
                    strMessage = "GT-2311:[股指期货委托持久化]无法计算交易金额，请检查行情是否有效";
                    PO_ValidateFailureProcess(ref strMessage);
                    return false;
                }

                if (predealCost < 0)
                    predealCost = 0;

                //四舍五入
                predealCapital = Utils.Round(predealCapital);
                predealCost = Utils.Round(predealCost);


                //1.资金检查
                if (
                    !this.PO_CapitalValidate(predealCost, predealCapital, ref strMessage))
                {
                    PO_ValidateFailureProcess(ref strMessage);
                    return false;
                }

                //2.持仓检查
                if (!PO_HoldValidate(ref strMessage))
                {
                    PO_ValidateFailureProcess(ref strMessage);
                    return false;
                }
            }
            catch (Exception ex)
            {
                strMessage = ex.Message;
                PO_ValidateFailureProcess(ref strMessage);
                return false;
            }

            #endregion

            #region 创建委托(处理失败时要删除此条委托）

            try
            {
                EntrustNumber = QHCommonLogic.BuildGZQHOrder(ref outEntity, Request, HoldingAccount, CapitalAccount,
                                                             CurrencyType,
                                                             ref strMessage);
            }
            catch (Exception ex)
            {
                DeleteEntrust(EntrustNumber);
                LogHelper.WriteError(ex.Message, ex);
                strMessage = "GT-2333:[期指期货委托持久化]无法创建委托";

                return false;
            }

            #endregion

            #region Persist流程

            //persistent流程
            //1.资金处理,可用资金减去预成交总金额，总冻结资金加上预成交总金额
            //2.冻结资金处理，生成一条冻结记录
            //3.持仓处理：开仓不做持仓处理，可用持仓减少委托量，总冻结持仓增加委托量
            //4.持仓冻结处理：开仓不做持仓冻结处理，生成一条冻结记录

            //实际处理：
            //2.4放在一个事务中进行，当成功后再进行1.3的处理
            bool isSuccess = false;

            #region 资金预处理

            var caMemory = MemoryDataManager.QHCapitalMemoryList.GetByCapitalAccountLogo(CapitalAccountId);

            if (caMemory == null)
            {
                strMessage = "GT-2312:[股指期货委托持久化]资金帐户不存在:" + CapitalAccount;

                return false;
            }
            //return caMemory.AddDelta(-preDealSum, preDealSum, 0, 0, 0, 0);

            if (Request.OpenCloseType == Entity.Contants.Types.FutureOpenCloseType.OpenPosition)
            {
                //预成交总金额 = 预成交金额 + 预成交费用
                preDealSum = predealCapital + predealCost;
            }
            else
            {
                //预成交总金额 = 预成交费用(平仓不需要加金额)
                preDealSum = predealCost;
            }

            QH_CapitalAccountTable_DeltaInfo capitalDelta = new QH_CapitalAccountTable_DeltaInfo();
            capitalDelta.CapitalAccountLogoId = caMemory.Data.CapitalAccountLogoId;
            capitalDelta.AvailableCapitalDelta = -preDealSum;
            capitalDelta.FreezeCapitalTotalDelta = preDealSum;

            #endregion

            #region 持仓预处理

            QHHoldMemoryTable ahtMemory = null;
            QH_HoldAccountTableInfo_Delta holdDelta = null;
            if (Request.OpenCloseType != Entity.Contants.Types.FutureOpenCloseType.OpenPosition)
            {
                ahtMemory = MemoryDataManager.QHHoldMemoryList.GetByAccountHoldLogoId(HoldingAccountId);
                if (ahtMemory == null)
                {
                    //平仓时获取持仓账户需要取相反的买卖方向，因为委托买时，对应的持仓方向是卖
                    var newbuySellType = Request.BuySell == Types.TransactionDirection.Buying
                                             ? Types.TransactionDirection.Selling
                                             : Types.TransactionDirection.Buying;

                    ahtMemory = QHCommonLogic.GetHoldMemoryTable(HoldingAccount, Code, CurrencyType,
                                                                 (int)newbuySellType);
                }

                if (ahtMemory == null)
                {
                    return false;
                }
                decimal orderAmount = Convert.ToDecimal(Request.OrderAmount);
                holdDelta = new QH_HoldAccountTableInfo_Delta();
                var holdData = ahtMemory.Data;
                holdDelta.AccountHoldLogoId = holdData.AccountHoldLogoId;
                holdDelta.Data = holdData;
                if (Request.OpenCloseType ==
                    Entity.Contants.Types.FutureOpenCloseType.CloseTodayPosition)
                {
                    holdDelta.TodayHoldAmountDelta -= orderAmount;
                    holdDelta.TodayFreezeAmountDelta += orderAmount;
                }

                if (Request.OpenCloseType == Entity.Contants.Types.FutureOpenCloseType.ClosePosition)
                {
                    holdDelta.HistoryHoldAmountDelta -= orderAmount;
                    holdDelta.HistoryFreezeAmountDelta += orderAmount;
                }
            }

            #endregion

            #region 数据库提交动作

            //#region 再次检查
            ////1.资金检查
            //if (
            //    !this.PO_CapitalValidate(predealCost, predealCapital, ref strMessage))
            //{
            //    PO_ValidateFailureProcess(ref strMessage);
            //    return false;
            //}

            ////2.持仓检查
            //if (!PO_HoldValidate(ref strMessage))
            //{
            //    PO_ValidateFailureProcess(ref strMessage);
            //    return false;
            //}

            //#endregion

            bool isCapitalSuccess = false;
            bool isHoldingSuccess = false;
            Database database = DatabaseFactory.CreateDatabase();
            try
            {
                using (DbConnection connection = database.CreateConnection())
                {
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        ReckoningTransaction tm = new ReckoningTransaction();
                        tm.Database = database;
                        tm.Transaction = transaction;
                        //1.资金处理
                        //首先提交资金到数据库
                        isCapitalSuccess = caMemory.CheckAndAddDelta(CapitalCheck, capitalDelta, database, transaction);
                        if (!isCapitalSuccess)
                        {
                            strMessage = capitalMesage;
                            capitalMesage = "";
                            throw new CheckException("QHCapitalMemoryTable.CheckAddAddDelta失败");
                        }

                        //2.冻结资金处理，生成一条冻结记录
                        persistCapitalFreezeId = PO_BuildCapitalFreezeRecord(predealCapital, predealCost, tm);

                        //3.持仓冻结处理:开仓不做股指期货持仓处理
                        if (Request.OpenCloseType != Entity.Contants.Types.FutureOpenCloseType.OpenPosition)
                        {
                            //持仓冻结处理，生成一条冻结记录
                            persistHoldFreezeId = PO_BuildHoldFreezeRecord(tm);
                        }

                        //4.持仓操作
                        //开仓不做股指期货持仓处理
                        if (Request.OpenCloseType != Entity.Contants.Types.FutureOpenCloseType.OpenPosition)
                        {
                            isHoldingSuccess = ahtMemory.CheckAndAddDelta(HodingCheck, holdDelta, tm.Database,
                                                                          tm.Transaction);
                            if (!isHoldingSuccess)
                            {
                                strMessage = holdMessage;
                                holdMessage = "";
                                throw new CheckException("QHHoldingMemoryTable.CheckAddAddDelta失败");
                            }
                        }

                        transaction.Commit();
                        isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogHelper.WriteError(ex.Message, ex);
                        if (!(ex is CheckException))
                            strMessage = "GT-2313:[股指期货委托持久化]持久化失败，无法提交到数据库";

                        isSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                strMessage = "GT-2314:[股指期货委托持久化]持久化失败";
            }

            //事务失败
            if (!isSuccess)
            {
                DeleteEntrust(EntrustNumber);

                if (isCapitalSuccess)
                {
                    caMemory.RollBackMemory(capitalDelta);
                }

                if (Request.OpenCloseType != Entity.Contants.Types.FutureOpenCloseType.OpenPosition)
                {
                    if (isHoldingSuccess)
                    {
                        ahtMemory.RollBackMemory(holdDelta);
                    }
                }


                return false;
            }

            //if (isSuccess)
            //{
            //    //当提交资金到数据库成功后，同步到内存
            //    caMemory.AddDeltaToMemory(capitalDelta);

            //    //开仓不做股指期货持仓处理
            //    if (Request.OpenCloseType != Types.FutureOpenCloseType.OpenPosition)
            //    {
            //        ahtMemory.AddDeltaToMemory(holdDelta);
            //    }
            //}
            //else
            //{
            //    DeleteEntrust(EntrustNumber);
            //    return false;
            //}

            #endregion

            #endregion

            return true;
        }

        private bool HodingCheck(QH_HoldAccountTableInfo hold, QH_HoldAccountTableInfo_Delta change)
        {
            bool result = false;
            string strMessage = "";

            if (Request.OpenCloseType ==
                Entity.Contants.Types.FutureOpenCloseType.OpenPosition)
            {
                strMessage = "GT-2325:[股指期货委托持久化]平仓检查,超过持仓限制";

                //获取持仓限制
                Decimal pLimit = MCService.GetPositionLimit(Request.Code).PositionValue;


                //可用持仓+冻结量+委托量<=持仓限制
                result = pLimit >=
                         hold.HistoryHoldAmount + hold.HistoryFreezeAmount + hold.TodayHoldAmount +
                         hold.TodayFreezeAmount
                         + Convert.ToDecimal(Request.OrderAmount);
            }

            if (Request.OpenCloseType ==
                Entity.Contants.Types.FutureOpenCloseType.CloseTodayPosition)
            {
                strMessage = "GT-2326:[股指期货委托持久化]平今日仓持仓检查,无足够可用持仓";

                if (hold.TodayHoldAmount <= 0)
                    result = false;

                result = hold.TodayHoldAmount + change.TodayHoldAmountDelta >= 0;
            }

            if (Request.OpenCloseType == Entity.Contants.Types.FutureOpenCloseType.ClosePosition)
            {
                strMessage = "GT-2328:[股指期货委托持久化]平历史仓持仓检查,无足够可用持仓";

                if (hold.HistoryHoldAmount <= 0)
                    result = false;

                result = hold.HistoryHoldAmount + change.HistoryHoldAmountDelta >= 0;
            }

            if (!result)
                holdMessage = strMessage;

            return result;
        }

        private bool CapitalCheck(QH_CapitalAccountTableInfo capital, QH_CapitalAccountTable_DeltaInfo change)
        {
            bool result = false;
            string strMessage = "";

            if (Request.OpenCloseType == Entity.Contants.Types.FutureOpenCloseType.OpenPosition)
            {
                strMessage = "GT-2319:[股指期货委托持久化]开仓资金检查,无足够可用资金";

                //只有开仓时才检查
                if (capital.AvailableCapital <= 0)
                    result = false;
                else
                    result = capital.AvailableCapital + change.AvailableCapitalDelta >= 0;
            }

            //平仓时只要成交额大于费用即可，前面已经检查过，此处不再检查
            if (Request.OpenCloseType == Entity.Contants.Types.FutureOpenCloseType.CloseTodayPosition)
            {
                strMessage = "GT-2321:[股指期货委托持久化]平今日仓资金检查,无足够可用资金";
                result = true;
            }

            if (Request.OpenCloseType == Entity.Contants.Types.FutureOpenCloseType.ClosePosition)
            {
                strMessage = "GT-2323:[股指期货委托持久化]平历史仓资金检查,无足够可用资金";
                result = true;
            }

            if (!result)
                capitalMesage = strMessage;

            return result;
        }

        #region 检查方法

        #region 计算预成交金额和预成交费用

        /// <summary>
        /// 计算预成交金额和预成交费用
        /// </summary>
        /// <param name="strMessage">错误信息</param>
        /// <param name="predealCapital">预成交金额</param>
        /// <param name="predealCost">预成交费用</param>
        /// <returns>计算是否成功</returns>
        private bool PO_ComputePreCapital(ref string strMessage, ref decimal predealCapital, ref decimal predealCost)
        {
            if (Request.OpenCloseType == Entity.Contants.Types.FutureOpenCloseType.OpenPosition)
            {
                return PO_ComputePreprocCapital_Open(Request, ref strMessage, ref predealCapital, ref predealCost);
            }

            if (Request.OpenCloseType == Entity.Contants.Types.FutureOpenCloseType.CloseTodayPosition)
            {
                return PO_ComputePreprocCapital_CloseToday(Request, ref strMessage, ref predealCapital, ref predealCost);
            }

            if (Request.OpenCloseType == Entity.Contants.Types.FutureOpenCloseType.ClosePosition)
            {
                return PO_ComputePreprocCapital_CloseHistory(Request, ref strMessage, ref predealCapital,
                                                             ref predealCost);
            }

            return true;
        }

        /// <summary>
        /// 计算开仓预成交金额和预成交费用
        /// </summary>
        /// <param name="order">原始委托</param>
        /// <param name="strMessage">错误信息</param>
        /// <param name="predealCapital">预成交金额</param>
        /// <param name="predealCost">预成交费用</param>
        /// <returns>计算是否成功</returns>
        private bool PO_ComputePreprocCapital_Open(StockIndexFuturesOrderRequest order, ref string strMessage,
                                                   ref decimal predealCapital, ref decimal predealCost)
        {
            bool result = false;
            predealCapital = 0;
            predealCost = 0;
            try
            {
                QHCostResult xhcr = null;
                float orderPrice = 0;
                //计价单位与交易单位倍数-期货是合约乘数300
                decimal unitMultiple = MCService.GetTradeUnitScale(order.Code, order.OrderUnitType);
                //保证金比例
                decimal futureBail = MCService.GetFutureBailScale(order.Code) / 100;
                //市价委托
                if (order.OrderWay == Entity.Contants.Types.OrderPriceType.OPTMarketPrice)
                {
                    decimal orderPriceD = (decimal)order.OrderPrice;
                    var highLowRange = MCService.HLRangeProcessor.GetHighLowRangeValueByCommodityCode(order.Code,
                                                                                                      orderPriceD);

                    if (highLowRange != null)
                    {
                        if (highLowRange.RangeType == Types.HighLowRangeType.HongKongPrice) //港股类型处理
                        {
                            var hkrv = highLowRange.HongKongRangeValue;
                            orderPrice = Convert.ToSingle(hkrv.BuyHighRangeValue);
                        }
                        else //其它类型处理
                        {
                            orderPrice = Convert.ToSingle(highLowRange.HighRangeValue);
                        }

                        if (orderPrice != 0)
                        {
                            int orderAmount = Convert.ToInt32(order.OrderAmount);
                            //成交金额 委托量 * 委托价 * 交易单位到计价单位倍数
                            decimal dealCapital = orderAmount * Convert.ToDecimal(orderPrice) * unitMultiple;
                            //保证金 成交金额 * 保证金比例
                            predealCapital = dealCapital * futureBail;
                            //预成交费用
                            xhcr = MCService.ComputeGZQHCost(order.Code, orderPrice, orderAmount,
                                                             order.OrderUnitType, order.OpenCloseType);
                            //预成交费用
                            predealCost = xhcr.Cosing;
                            result = true;
                        }
                        else
                        {
                            strMessage = "GT-2315:[股指期货委托持久化]开仓商品无涨跌幅设置";
                        }
                    }
                    else
                    {
                        strMessage = "GT-2315:[股指期货委托持久化]开仓商品无涨跌幅设置";
                    }
                }
                else //限价委托计算( 委托价*委托量 + 费用)
                {
                    int orderAmount = Convert.ToInt32(order.OrderAmount);
                    //成交金额 委托量 * 委托价 * 交易单位到计价单位倍数
                    decimal dealCapital = orderAmount * Convert.ToDecimal(order.OrderPrice) * unitMultiple;
                    //保证金 成交金额 * 保证金比例
                    predealCapital = dealCapital * futureBail;
                    //预成交费用
                    xhcr = MCService.ComputeGZQHCost(order.Code, order.OrderPrice, orderAmount,
                                                     order.OrderUnitType, order.OpenCloseType);
                    //预成交费用
                    predealCost = xhcr.Cosing;
                }
                result = true;
            }
            catch (Exception ex)
            {
                if (ex is VTException)
                {
                    strMessage = ex.ToString();
                }
                else
                {
                    strMessage = "GT-2316:[股指期货委托持久化]开仓成交金额及费用计算异常.";
                }
            }
            return result;
        }

        /// <summary>
        /// 计算平今预成交金额和预成交费用
        /// </summary>
        /// <param name="order">原始委托</param>
        /// <param name="strMessage">错误信息</param>
        /// <param name="predealCapital">预成交金额</param>
        /// <param name="predealCost">预成交费用</param>
        /// <returns>计算是否成功</returns>
        private bool PO_ComputePreprocCapital_CloseToday(StockIndexFuturesOrderRequest order, ref string strMessage,
                                                         ref decimal predealCapital, ref decimal predealCost)
        {
            bool result = false;
            predealCapital = 0;
            predealCost = 0;
            try
            {
                QHCostResult xhcr = null;
                //计价单位与交易单位倍数-期货是合约乘数300
                decimal unitMultiple = MCService.GetTradeUnitScale(order.Code, order.OrderUnitType);
                //保证金比例
                decimal futureBail = MCService.GetFutureBailScale(order.Code) / 100;
                float orderPrice = 0;

                //市价委托
                if (order.OrderWay == Entity.Contants.Types.OrderPriceType.OPTMarketPrice)
                {
                    decimal orderPriceD = (decimal)order.OrderPrice;
                    var highLowRange = MCService.HLRangeProcessor.GetHighLowRangeValueByCommodityCode(order.Code,
                                                                                                      orderPriceD);
                    if (highLowRange.RangeType == Types.HighLowRangeType.HongKongPrice) //港股类型处理
                    {
                        var hkrv = highLowRange.HongKongRangeValue;
                        orderPrice = Convert.ToSingle(hkrv.SellHighRangeValue);
                    }
                    else //其它类型处理
                    {
                        orderPrice = Convert.ToSingle(highLowRange.HighRangeValue);
                    }
                }
                else //限价委托计算( 委托价*委托量 + 费用)
                {
                    orderPrice = order.OrderPrice;
                }

                #region 预成交总金额

                //预成交总金额 委托量 * 计价单位与交易单位倍数 * 委托价
                int orderAmount = Convert.ToInt32(order.OrderAmount);
                //成交金额 委托量 * 委托价 * 交易单位到计价单位倍数
                decimal dealCapital = orderAmount * Convert.ToDecimal(orderPrice) * unitMultiple;
                //保证金 成交金额 * 保证金比例
                //预成交总金额 委托量 * 计价单位与交易单位倍数 * 委托价
                predealCapital = dealCapital * futureBail;

                #endregion

                #region 预成交费用

                //预成交费用
                xhcr = MCService.ComputeGZQHCost(order.Code, orderPrice, (int)order.OrderAmount,
                                               order.OrderUnitType, order.OpenCloseType);
                //预成交费用
                predealCost = xhcr.Cosing;

                #endregion

                result = true;
            }
            catch (Exception ex)
            {
                strMessage = "GT-2317:[股指期货委托持久化]平今日仓费用处理异常";
                LogHelper.WriteError(ex.Message, ex);
            }
            return result;
        }


        /// <summary>
        /// 计算平历史预成交金额和预成交费用
        /// </summary>
        /// <param name="order">原始委托</param>
        /// <param name="strMessage">错误信息</param>
        /// <param name="predealCapital">预成交金额</param>
        /// <param name="predealCost">预成交费用</param>
        /// <returns>计算是否成功</returns>
        private bool PO_ComputePreprocCapital_CloseHistory(StockIndexFuturesOrderRequest order,
                                                           ref string strMessage, ref decimal predealCapital,
                                                           ref decimal predealCost)
        {
            bool result = false;
            predealCapital = 0;
            predealCost = 0;
            try
            {
                QHCostResult xhcr = null;
                //计价单位与交易单位倍数-期货是合约乘数300
                decimal unitMultiple = MCService.GetTradeUnitScale(order.Code, order.OrderUnitType);
                //保证金比例
                decimal futureBail = MCService.GetFutureBailScale(order.Code) / 100;

                float orderPrice = 0;

                //市价委托
                if (order.OrderWay == Entity.Contants.Types.OrderPriceType.OPTMarketPrice)
                {
                    decimal orderPriceD = (decimal)order.OrderPrice;
                    var highLowRange = MCService.HLRangeProcessor.GetHighLowRangeValueByCommodityCode(order.Code,
                                                                                                      orderPriceD);
                    if (highLowRange.RangeType == Types.HighLowRangeType.HongKongPrice) //港股类型处理
                    {
                        var hkrv = highLowRange.HongKongRangeValue;
                        orderPrice = Convert.ToSingle(hkrv.SellHighRangeValue);
                    }
                    else //其它类型处理
                    {
                        orderPrice = Convert.ToSingle(highLowRange.HighRangeValue);
                    }

                    //预成交费用
                    xhcr = MCService.ComputeGZQHCost(order.Code, orderPrice, (int)order.OrderAmount,
                                                   order.OrderUnitType, order.OpenCloseType);
                    //预成交费用
                    predealCost = xhcr.Cosing;
                }
                else //限价委托计算( 委托价*委托量 + 费用)
                {
                    orderPrice = order.OrderPrice;
                    //成本计算器
                    xhcr = MCService.ComputeGZQHCost(order);

                    //预成交费用
                    predealCost = xhcr.Cosing;
                }

                //预成交总金额 委托量 * 计价单位与交易单位倍数 * 委托价
                int orderAmount = Convert.ToInt32(order.OrderAmount);
                //成交金额 委托量 * 委托价 * 交易单位到计价单位倍数
                decimal dealCapital = orderAmount * Convert.ToDecimal(orderPrice) * unitMultiple;
                //保证金 成交金额 * 保证金比例
                //预成交总金额 委托量 * 计价单位与交易单位倍数 * 委托价
                predealCapital = dealCapital * futureBail;

                result = true;
            }
            catch (Exception ex)
            {
                strMessage = "GT-2318:[股指期货委托持久化]平历史仓费用处理异常";

                LogHelper.WriteError(ex.Message, ex);
            }
            return result;
        }

        #endregion

        #region  资金检查

        private bool PO_CapitalValidate(decimal preDealCost, decimal preDealCapital, ref string strMessage)
        {
            if (Request.OpenCloseType == Entity.Contants.Types.FutureOpenCloseType.OpenPosition)
            {
                return PO_CapitalValidate_Open(preDealCost, preDealCapital, CapitalAccountId, ref strMessage);
            }

            if (Request.OpenCloseType == Entity.Contants.Types.FutureOpenCloseType.CloseTodayPosition)
            {
                return PO_CapitalValidate_CloseToday(preDealCost, preDealCapital, CapitalAccountId, ref strMessage);
            }

            if (Request.OpenCloseType == Entity.Contants.Types.FutureOpenCloseType.ClosePosition)
            {
                return PO_CapitalValidate_CloseHistory(preDealCost, preDealCapital, CapitalAccountId, ref strMessage);
            }

            return true;
        }

        private bool PO_CapitalValidate_Open(decimal preDealCost, decimal preDealCapital, int capitalAccountLogo,
                                             ref string strMessage)
        {
            bool result = false;
            strMessage = "GT-2319:[股指期货委托持久化]开仓资金检查,无足够可用资金";

            var catMemory = MemoryDataManager.QHCapitalMemoryList.GetByCapitalAccountLogo(capitalAccountLogo);

            //cat = QHDataAccess.GetQhCapitalAccountTable(tm, capitalAccount, iCurrType);
            //资金帐户是否存在判断
            if (catMemory != null)
            {
                var cat = catMemory.Data;
                result = cat.AvailableCapital >= preDealCost + preDealCapital;
            }
            else
            {
                strMessage = "GT-2320:[股指期货委托持久化]开仓资金检查,资金帐户不存在";
            }

            if (result)
                strMessage = "";

            return result;
        }

        private bool PO_CapitalValidate_CloseToday(decimal preDealCost, decimal preDealCapital, int capitalAccountLogo,
                                                   ref string strMessage)
        {
            bool result = false;
            strMessage = "GT-2321:[股指期货委托持久化]平今日仓资金检查,无足够可用资金";

            var catMemory = MemoryDataManager.QHCapitalMemoryList.GetByCapitalAccountLogo(capitalAccountLogo);

            //cat = QHDataAccess.GetQhCapitalAccountTable(tm, capitalAccount, iCurrType);
            //资金帐户是否存在判断
            if (catMemory != null)
            {
                var cat = catMemory.Data;

                //可用资金＞＝费用
                result = cat.AvailableCapital >= preDealCost;
                if (!result)
                {
                    result = preDealCapital >= preDealCost;
                }
            }
            else
            {
                strMessage = "GT-2322:[股指期货委托持久化]平今日仓资金检查,资金帐户不存在";
            }

            if (result)
                strMessage = "";

            return result;
        }

        private bool PO_CapitalValidate_CloseHistory(decimal preDealCost, decimal preDealCapital,
                                                     int capitalAccountLogo,
                                                     ref string strMessage)
        {
            bool result = false;
            strMessage = "GT-2323:[股指期货委托持久化]平历史仓资金检查,无足够可用资金";


            var catMemory = MemoryDataManager.QHCapitalMemoryList.GetByCapitalAccountLogo(capitalAccountLogo);

            //cat = QHDataAccess.GetQhCapitalAccountTable(tm, capitalAccount, iCurrType);
            //资金帐户是否存在判断
            if (catMemory != null)
            {
                var cat = catMemory.Data;

                //可用资金＞＝费用
                result = cat.AvailableCapital >= preDealCost;
                if (!result)
                {
                    result = preDealCapital >= preDealCost;
                }

                //强制平仓的过期合约生成的委托不检查资金
                //if (IsExpiredContract || IsCheckCapitalOrder)
                if (IsForcedCloseOrder)
                {
                    result = true;
                }
            }
            else
            {
                strMessage = "GT-2324:[股指期货委托持久化]平历史仓资金检查,资金帐户不存在";
            }

            if (result)
                strMessage = "";

            return result;
        }

        #endregion

        #region 持仓检查

        private bool PO_HoldValidate(ref string strMessage)
        {
            if (Request.OpenCloseType == Entity.Contants.Types.FutureOpenCloseType.OpenPosition)
            {
                return PO_HoldingAccountValidate_Open(Request, HoldingAccountId, ref strMessage);
            }

            if (Request.OpenCloseType == Entity.Contants.Types.FutureOpenCloseType.CloseTodayPosition)
            {
                return PO_HoldingAccountValidate_CloseToday(Request, HoldingAccountId, ref strMessage);
            }

            if (Request.OpenCloseType == Entity.Contants.Types.FutureOpenCloseType.ClosePosition)
            {
                return PO_HoldingAccountValidate_CloseHistory(Request, HoldingAccountId, ref strMessage);
            }

            return true;
        }

        protected bool PO_HoldingAccountValidate_Open(StockIndexFuturesOrderRequest stockorder, int holdingAccountId, ref string strMessage)
        {
            bool result = false;
            strMessage = "GT-2325:[股指期货委托持久化]开仓持仓检查,超过持仓限制";
            //获取股指期货对应代码持仓帐户实体，开仓时不能用传入的HoldingId，传入的是平仓使用的相反持仓
            //var ahtMemory = MemoryDataManager.QHHoldMemoryList.GetByAccountHoldLogoId(holdingAccountId);
            var ahtMemory = MemoryDataManager.QHHoldMemoryList.GetByHoldAccountAndCurrencyType(HoldingAccount, Code,
                                                                                               (int)Request.BuySell,
                                                                                               CurrencyType);
            if (ahtMemory == null)
            {
                ahtMemory = QHCommonLogic.GetHoldMemoryTable(HoldingAccount, Code, CurrencyType, (int)Request.BuySell);
            }


            //aht = this.GetQhHoldAccountTable(tm, strHoldingAccount, stockorder.Code, (int)stockorder.BuySell, iCurType);
            //持仓帐户是否存在判断
            if (ahtMemory != null)
            {
                var aht = ahtMemory.Data;
                if (aht != null)
                {
                    /*update 李健华 2009-12-11===========
                    *当程序没有加载持仓时第一次为-1，那么这里在内部从数据库获取持仓表的主键持仓账户ID时获取到了数据并加载到了内
                    *存表中，那么这里也应把持仓账户ID附值回这里,要不然在数据库提交时会有主外键的冲突,如果不设置数据库的主外键那么
                     *这里可以不用修改，因为后面清算的时候是以委托编号去查询相关的冻结记录
                    */
                    if (HoldingAccountId == -1)
                    {
                        HoldingAccountId = aht.AccountHoldLogoId;
                    }
                    //======================

                    //获取持仓限制
                    Decimal pLimit = MCService.GetPositionLimit(stockorder.Code).PositionValue;


                    //可用持仓+冻结量+委托量<=持仓限制
                    result = pLimit >=
                             aht.HistoryHoldAmount + aht.HistoryFreezeAmount + aht.TodayHoldAmount +
                             aht.TodayFreezeAmount
                             + Convert.ToDecimal(stockorder.OrderAmount);
                }
            }
            else
            {
                result = true;
            }

            if (result)
                strMessage = "";

            return result;
        }

        private bool PO_HoldingAccountValidate_CloseToday(StockIndexFuturesOrderRequest stockorder, int holdingAccountId,
                                                          ref string strMessage)
        {
            bool result = false;
            strMessage = "GT-2326:[股指期货委托持久化]平今日仓持仓检查,无足够可用持仓";
            //获取期货对应代码和买卖方向上的持仓帐户实体
            var ahtMemory = MemoryDataManager.QHHoldMemoryList.GetByAccountHoldLogoId(holdingAccountId);
            if (ahtMemory == null)
            {
                //平仓时获取持仓账户需要取相反的买卖方向，因为委托买时，对应的持仓方向是卖
                var newbuySellType = Request.BuySell == Types.TransactionDirection.Buying
                                         ? Types.TransactionDirection.Selling
                                         : Types.TransactionDirection.Buying;

                ahtMemory = QHCommonLogic.GetHoldMemoryTable(HoldingAccount, Code, CurrencyType, (int)newbuySellType);
            }


            //aht = this.GetQhHoldAccountTable(tm, strHoldingAccount, stockorder.Code, (int)stockorder.BuySell, iCurType);
            //持仓帐户是否存在判断
            if (ahtMemory != null)
            {
                var aht = ahtMemory.Data;
                if (aht != null)
                {
                    /*update 李健华 2009-12-11===========
                    *当程序没有加载持仓时第一次为-1，那么这里在内部从数据库获取持仓表的主键持仓账户ID时获取到了数据并加载到了内
                    *存表中，那么这里也应把持仓账户ID附值回这里,要不然在数据库提交时会有主外键的冲突,如果不设置数据库的主外键那么
                     *这里可以不用修改，因为后面清算的时候是以委托编号去查询相关的冻结记录
                    */
                    if (HoldingAccountId == -1)
                    {
                        HoldingAccountId = aht.AccountHoldLogoId;
                    }
                    //======================
                    //计价单位与交易单位倍数
                    //decimal unitMultiple = MCService.GetTradeUnitScale(stockorder.Code, stockorder.OrderUnitType);
                    decimal orderAmount = Convert.ToDecimal(stockorder.OrderAmount); // *unitMultiple;

                    // decimal orderAmount = Convert.ToDecimal(stockorder.OrderAmount);
                    result = aht.TodayHoldAmount >= orderAmount;
                }
            }
            else
            {
                strMessage = "GT-2327:[股指期货委托持久化]平今日仓持仓检查,无持仓";
                return false;
            }

            if (result)
                strMessage = "";

            return result;
        }

        private bool PO_HoldingAccountValidate_CloseHistory(StockIndexFuturesOrderRequest stockorder,
                                                            int holdingAccountId,
                                                            ref string strMessage)
        {
            bool result = false;
            strMessage = "GT-2328:[股指期货委托持久化]平历史仓持仓检查,无足够可用持仓";

            var buySellType = stockorder.BuySell == Types.TransactionDirection.Buying
                                  ? (int)Types.TransactionDirection.Selling
                                  : (int)Types.TransactionDirection.Buying;

            //获取期货对应代码和买卖方向上的持仓帐户实体
            var ahtMemory = MemoryDataManager.QHHoldMemoryList.GetByAccountHoldLogoId(holdingAccountId);
            if (ahtMemory == null)
            {
                //平仓时获取持仓账户需要取相反的买卖方向，因为委托买时，对应的持仓方向是卖
                var newbuySellType = Request.BuySell == Types.TransactionDirection.Buying
                                         ? Types.TransactionDirection.Selling
                                         : Types.TransactionDirection.Buying;

                ahtMemory = QHCommonLogic.GetHoldMemoryTable(HoldingAccount, Code, CurrencyType, (int)newbuySellType);
            }


            //aht = this.GetQhHoldAccountTable(tm, strHoldingAccount, stockorder.Code, (int)stockorder.BuySell, iCurType);
            //持仓帐户是否存在判断
            if (ahtMemory != null)
            {
                var aht = ahtMemory.Data;
                if (aht != null)
                {

                    /*update 李健华 2009-12-11===========
                    *当程序没有加载持仓时第一次为-1，那么这里在内部从数据库获取持仓表的主键持仓账户ID时获取到了数据并加载到了内
                    *存表中，那么这里也应把持仓账户ID附值回这里,要不然在数据库提交时会有主外键的冲突,如果不设置数据库的主外键那么
                     *这里可以不用修改，因为后面清算的时候是以委托编号去查询相关的冻结记录
                    */
                    if (HoldingAccountId == -1)
                    {
                        HoldingAccountId = aht.AccountHoldLogoId;
                    }
                    //======================
                    //计价单位与交易单位倍数
                    //decimal unitMultiple = MCService.GetTradeUnitScale(stockorder.Code, stockorder.OrderUnitType);
                    decimal orderAmount = Convert.ToDecimal(stockorder.OrderAmount); // *unitMultiple;

                    //decimal orderAmount = Convert.ToDecimal(stockorder.OrderAmount);
                    result = aht.HistoryHoldAmount >= orderAmount;
                }
            }
            else
            {
                strMessage = "GT-2329:[股指期货委托持久化]平历史仓持仓检查,无持仓";
                return false;
            }

            if (result)
                strMessage = "";

            return result;
        }

        #endregion

        #endregion

        #region Persist功能方法

        /// <summary>
        /// 获取账户id
        /// </summary>
        /// <param name="strCapitalAccount"></param>
        /// <param name="strHoldingAccount"></param>
        private void GetAccountId(string strCapitalAccount, string strHoldingAccount,
                                  Types.TransactionDirection buySellType)
        {
            CapitalAccount = strCapitalAccount;
            HoldingAccount = strHoldingAccount;

            CapitalAccountId = MemoryDataManager.QHCapitalMemoryList.GetCapitalAccountLogo(CapitalAccount, CurrencyType);

            //平仓时获取持仓账户需要取相反的买卖方向，因为委托买时，对应的持仓方向是卖
            var newbuySellType = buySellType == Types.TransactionDirection.Buying
                                     ? Types.TransactionDirection.Selling
                                     : Types.TransactionDirection.Buying;

            //开仓时不需要
            HoldingAccountId = MemoryDataManager.QHHoldMemoryList.GetAccountHoldLogoId(HoldingAccount, Code,
                                                                                       (int)newbuySellType,
                                                                                       CurrencyType);
        }

        /// <summary>
        /// 获取币种
        /// </summary>
        /// <returns></returns>
        private bool GetCurrencyType()
        {
            var currOjb = MCService.FuturesTradeRules.GetCurrencyTypeByCommodityCode(Code);
            if (currOjb == null)
            {
                return false;
            }

            CurrencyType = currOjb.CurrencyTypeID;
            return true;
        }

        private bool CheckAccount(out string strCapitalAccount, out string strHoldingAccount, out string strMessage)
        {
            //资金及持仓帐户状态
            bool bholdAccount, bCapitalAccount;

            this.counterCacher.GetHoldingAccountByTraderInfo(Request.TraderId, Request.Code,
                                                             out strHoldingAccount, out bholdAccount,
                                                             out strCapitalAccount, out bCapitalAccount);

            if (!bholdAccount)
            {
                strMessage = "GT-2305:[股指期货委托持久化]持仓帐户无交易权限";
                return false;
            }

            if (!bCapitalAccount)
            {
                strMessage = "GT-2306:[股指期货委托持久化]资金帐户无交易权限";
                return false;
            }

            strMessage = "";
            return true;
        }

        #endregion

        #region Persist失败处理方法

        /// <summary>
        /// Persist检查失败时的消息处理
        /// </summary>
        /// <param name="strMessage"></param>
        private void PO_ValidateFailureProcess(ref string strMessage)
        {
            if (strMessage.IndexOf("GT") == -1)
                strMessage = "GT-2332:[股指期货委托持久化]持久化检查失败," + strMessage;

            LogHelper.WriteDebug(strMessage);
        }

        private void DeleteEntrust(string entrustNumber)
        {
            //删除前面创建的柜台委托单实例
            bool isSuccess = QHDataAccess.DeleteTodayEntrust(entrustNumber);
            if (!isSuccess)
            {
                RescueManager.Instance.Record_QH_DeleteTodayEntrust(entrustNumber);
            }
        }

        #endregion

        #region 持久化方法

        /// <summary>
        /// 股指期货持仓冻结处理
        /// </summary>
        /// <param name="tm"></param>
        /// <returns></returns>
        private int PO_BuildHoldFreezeRecord(ReckoningTransaction tm)
        {
            LogHelper.WriteDebug("股指期货持仓冻结处理XHSellOrderLogicFlow.PO_ProcessXhHoldingAccountFreeze");

            var ahft = new QH_HoldAccountFreezeTableInfo();
            //期货持仓帐户标识
            ahft.AccountHoldLogo = HoldingAccountId;
            //委托单号
            ahft.EntrustNumber = EntrustNumber;
            //冻结时间
            ahft.FreezeTime = DateTime.Now;
            //解冻时间
            ahft.ThawTime = DateTime.Now;

            //冻结数量
            //计价单位与交易单位倍数
            //decimal unitMultiple = MCService.GetTradeUnitScale(order.Code, order.OrderUnitType);
            decimal orderAmount = Convert.ToDecimal(Request.OrderAmount); // *unitMultiple;
            ahft.FreezeAmount = (int)orderAmount;
            //冻结类型
            ahft.FreezeTypeLogo = (int)Entity.Contants.Types.FreezeType.DelegateFreeze;

            string format = "股指期货持仓冻结处理[委托单号={0},冻结总量={1},冻结时间={2},解冻时间={3},冻结类型={4},期货帐户持仓标识={5},]";
            string desc = string.Format(format, ahft.EntrustNumber, ahft.FreezeAmount, ahft.FreezeTime,
                                        ahft.ThawTime, ahft.FreezeTypeLogo, ahft.HoldFreezeLogoId);
            LogHelper.WriteDebug(desc);

            QH_HoldAccountFreezeTableDal dal = new QH_HoldAccountFreezeTableDal();
            return dal.Add(ahft, tm);
        }

        private int PO_BuildCapitalFreezeRecord(decimal predealCapital, decimal predealCost, ReckoningTransaction tm)
        {
            var caft = new QH_CapitalAccountFreezeTableInfo();

            //委托单号
            caft.EntrustNumber = EntrustNumber;

            //冻结 预成交金额,平仓时为0
            if (Request.OpenCloseType == Entity.Contants.Types.FutureOpenCloseType.OpenPosition)
            {
                caft.FreezeAmount = predealCapital;
            }
            else
            {
                caft.FreezeAmount = 0;
            }
            //冻结 预成交费用
            caft.FreezeCost = predealCost;
            //冻结时间
            caft.FreezeTime = DateTime.Now;
            //解冻时间
            caft.ThawTime = DateTime.Now;
            //冻结类型
            caft.FreezeTypeLogo = (int)Entity.Contants.Types.FreezeType.DelegateFreeze;

            caft.OweCosting = 0;

            caft.CapitalAccountLogo = CapitalAccountId;

            string format =
                "股指期货开仓资金冻结处理GZQHOpenOrderLogicFlow.PO_ProcessQhCapitalAccountFreeze[委托单号={0},冻结时间={1},解冻时间={2},冻结类型={3},预成交金额={4},预成交费用={5}]";

            string desc = string.Format(format, caft.EntrustNumber, caft.FreezeTime, caft.ThawTime, caft.FreezeTypeLogo,
                                        caft.FreezeAmount, caft.FreezeCost);
            LogHelper.WriteDebug(desc);

            QH_CapitalAccountFreezeTableDal dal = new QH_CapitalAccountFreezeTableDal();
            return dal.Add(caft, tm);
        }

        #endregion

        #endregion
    }
}