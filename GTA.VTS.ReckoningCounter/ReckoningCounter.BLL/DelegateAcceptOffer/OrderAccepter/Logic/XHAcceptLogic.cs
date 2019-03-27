#region Using Namespace

using System;
using System.Data.Common;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.DelegateValidate;
using ReckoningCounter.BLL.DelegateValidate.Cost;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.BLL.Reckoning.Logic;
using ReckoningCounter.DAL.CustomDataAccess;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.Entity;
using ReckoningCounter.MemoryData;
using ReckoningCounter.MemoryData.XH.Hold;
using ReckoningCounter.Model;
using Types = ReckoningCounter.Entity.Contants.Types;

#endregion

namespace ReckoningCounter.BLL.DelegateAcceptOffer.OrderAccepter.Logic
{
    /// <summary>
    /// 现货下单预处理逻辑，错误编码2200-2229
    /// </summary>
    public class XHAcceptLogic : AcceptLogic<StockOrderRequest, XH_TodayEntrustTableInfo, XH_TodayTradeTableInfo>
    {
        #region CancelOrderValidate 2200-2205

        protected override CounterCache GetCounterCache()
        {
            return XHCounterCache.Instance;
        }

        /// <summary>
        /// 撤单校验-检查委托单当前状态是否可撤
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        /// <param name="tet">委托实体</param>
        /// <param name="strMessage">错误信息</param>
        /// <returns>校验是否通过</returns>
        public override bool CancelOrderValidate(string entrustNumber, out XH_TodayEntrustTableInfo tet,
                                                 ref string strMessage)
        {
            var result = false;

            tet = XHDataAccess.GetTodayEntrustTable(entrustNumber);
            if (tet != null)
            {
                if (tet.OrderStatusId == (int)Types.OrderStateType.DOSUnRequired ||
                    tet.OrderStatusId == (int)Types.OrderStateType.DOSRequiredSoon ||
                    tet.OrderStatusId == (int)Types.OrderStateType.DOSIsRequired ||
                    tet.OrderStatusId == (int)Types.OrderStateType.DOSPartDealed)
                {
                    result = true;
                }
                else
                {
                    strMessage = "GT-2200:[现货撤单校验]该委托状态的委托单不可撤.委托状态=" +
                                 Types.GetOrderStateMsg(tet.OrderStatusId);
                }
            }
            else
            {
                strMessage = "GT-2201:[现货撤单校验]委托单不存在.";
            }

            return result;
        }

        #endregion

        #region 内部撤单（原DelegateOffer_CancelOrder）2206-2210

        /// <summary>
        /// 内部撤单
        /// </summary>
        /// <param name="tet">委托实体</param>
        /// <param name="strMcErrorMessage">错误信息</param>
        /// <returns>是否成功</returns>
        public override bool InternalCancelOrder(XH_TodayEntrustTableInfo tet, string strMcErrorMessage)
        {
            #region 初始化参数

            //柜台委托单号
            EntrustNumber = tet.EntrustNumber;

            string errInfo = "[委托单号=" + EntrustNumber + ",Message=" + strMcErrorMessage + "]";
            LogHelper.WriteDebug(
                "------xxxxxx------开始进行现货内部撤单(撤单时委托状态为未报或者买卖报盘返回的委托单号无效)XHAcceptLogic.InternalCancelOrder" +
                errInfo);

            Code = tet.SpotCode;

            //资金帐户
            CapitalAccount = tet.CapitalAccount;
            //持仓帐户
            HoldingAccount = tet.StockAccount;

            //GetAccountId(CapitalAccount, HoldingAccount);

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
            XHDataAccess.UpdateEntrustTable(tet);
            LogHelper.WriteDebug("XHAcceptLogic.InternalCancelOrder添加委托单号映射到ConterCache,OrderNo=" +
                                 orderNo);
            //柜台缓存委托相关信息
            var item = new OrderCacheItem(tet.CapitalAccount, tet.StockAccount,
                                          tet.EntrustNumber,
                                          (
                                          GTA.VTS.Common.CommonObject.Types.TransactionDirection)
                                          Enum.Parse(
                                              typeof(
                                                  GTA.VTS.Common.CommonObject.Types.TransactionDirection),
                                              tet.BuySellTypeId.ToString
                                                  ()));
            item.EntrustAmount = tet.EntrustAmount;
            item.Code = tet.SpotCode;
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

            ReckonCenter.Instace.AcceptCancelXHOrderRpt(coe);

            #endregion

            //内部撤单流程
            //1.资金冻结处理，把资金冻结记录里的金额和费用全部还给资金表，删除冻结记录(实际上清零，不删除，盘后统一删）
            //2.资金处理，把从资金冻结记录还回来的金额和费用加到可用资金，并减去总冻结资金
            //3.持仓冻结处理(买入不处理）,把持仓冻结记录中的冻结量还给持仓表，删除冻结记录(实际上清零，不删除，盘后统一删）
            //4.持仓处理（买入不处理），把从持仓冻结记录还回来的持仓量加到可用持仓，并减去总冻结持仓
            //5.委托处理，更新委托状态，成交量，撤单量以及委托消息(OrderMessage)

            #region 已作废

            //实际的处理流程
            /*
            #region 1.资金冻结处理，获取冻结金额和冻结费用，并获取冻结记录的id

            //冻结金额
            decimal preFreezeCapital = 0;
            //冻结费用
            decimal preFreezeCost = 0;

            //持仓冻结的id
            int holdFreezeLogoId = -1;
            //资金冻结的id 
            int capitalFreezeLogoId = -1;

            var ca = XHDataAccess.GetCapitalAccountFreeze(EntrustNumber, Types.FreezeType.DelegateFreeze);
            if (ca == null)
            {
                string msg = "[现货内部撤单]资金冻结记录不存在." + entrustNum;
                LogHelper.WriteInfo(msg);
                //找不到资金冻结，一样允许撤单，当作冻结的资金全部为0
                //CancelOrderFailureProcess(EntrustNumber, 0, 0, 0, strErrorMessage);
                //return false;
            }
            else
            {
                preFreezeCapital = ca.FreezeCapitalAmount;
                preFreezeCost = ca.FreezeCost;
                capitalFreezeLogoId = ca.CapitalFreezeLogoId;
            }

            #endregion

            #region 2.资金处理，把从资金冻结记录还回来的金额和费用加到可用资金，并减去总冻结资金
            XH_CapitalAccountTable_DeltaInfo capitalDelta = new XH_CapitalAccountTable_DeltaInfo();
            XH_CapitalAccountTable_DeltaInfo capitalDelta2 = new XH_CapitalAccountTable_DeltaInfo();

            decimal delta = preFreezeCapital + preFreezeCost;

            if (delta != 0)
            {
                capitalDelta.AvailableCapitalDelta = delta;
                capitalDelta.FreezeCapitalTotalDelta = -delta;

                capitalDelta2.AvailableCapitalDelta = -delta;
                capitalDelta2.FreezeCapitalTotalDelta = delta;

                var capMemory = MemoryDataManager.XHCapitalMemoryList.GetByCapitalAccountLogo(CapitalAccountId);
                if (capMemory == null)
                {
                    strErrorMessage = "GT-2206:[现货内部撤单]资金帐户不存在:" + CapitalAccount;
                    return false;
                }

                bool isSuccess = capMemory.AddDelta(capitalDelta);

               
                if (!isSuccess)
                {
                   
                    return false;
                }
            }
            

            #endregion

            decimal preFreezeHoldAmount = 0m;
            if (Request.BuySell ==
                CommonObject.Types.TransactionDirection.Selling)
            {
                #region 3.持仓冻结处理(买入不计算）,获取持仓冻结记录中的冻结量和冻结id

                var hold = XHDataAccess.GetHoldAccountFreeze(EntrustNumber, Types.FreezeType.DelegateFreeze);
                if (hold == null)
                {
                    string msg = "[现货内部撤单]持仓冻结不存在";
                    LogHelper.WriteInfo(msg);
                    //持仓冻结不存在，一样运行撤单，当作持仓冻结量为0
                    //CancelOrderFailureProcess(EntrustNumber, -preFreezeCapital, -preFreezeCost, 0, strErrorMessage);
                    //return false;
                }
                else
                {
                    preFreezeHoldAmount = hold.PrepareFreezeAmount;
                    holdFreezeLogoId = hold.HoldFreezeLogoId;
                }

                #endregion

                //4.持仓处理（买入不计算），把从持仓冻结记录还回来的持仓量加到可用持仓，并减去总冻结持仓
                isSuccess = XHCommonLogic.DoXHReckonHoldProcess(preFreezeHoldAmount, ref strErrorMessage,
                                                                   HoldingAccount, HoldingAccountId);
                if (!isSuccess)
                {
                    XHCommonLogic.CancelOrderFailureProcess(EntrustNumber, -preFreezeCapital, -preFreezeCost, 0,
                                                            strErrorMessage, CapitalAccount, CapitalAccountId,
                                                            HoldingAccount, HoldingAccountId);
                    return false;
                }
            }


            //Clear资金和持仓冻结记录(没有删除)，更新委托信息
            tet = XHDataAccess.GetTodayEntrustTable(EntrustNumber);
            XHCommonLogic.ProcessCancelOrderStatus(tet);

            tet.CancelAmount = tet.EntrustAmount - tet.TradeAmount;
            tet.OrderMessage = strMcErrorMessage;

            try
            {
                DataManager.ExecuteInTransaction((db, transaction) =>
                                                     {
                                                         XHDataAccess.ClearCapitalFreeze(capitalFreezeLogoId, db,
                                                                                         transaction);
                                                         XHDataAccess.ClearHoldFreeze(holdFreezeLogoId, db, transaction);

                                                         XHDataAccess.UpdateEntrustTable(tet, db, transaction);
                                                     });
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);

                XHCommonLogic.CancelOrderFailureProcess(EntrustNumber, -preFreezeCapital, -preFreezeCost,
                                                        -preFreezeHoldAmount,
                                                        strErrorMessage, CapitalAccount, CapitalAccountId,
                                                        HoldingAccount, HoldingAccountId);

                return false;
            }

            ReckonEndObject<XH_TodayEntrustTableInfo, XH_TodayTradeTableInfo> canCleObject =
                new ReckonEndObject<XH_TodayEntrustTableInfo, XH_TodayTradeTableInfo>();
            canCleObject.EntrustTable = tet;
            canCleObject.TradeID = TradeID;
            canCleObject.IsSuccess = true;
            canCleObject.TradeTableList = new List<XH_TodayTradeTableInfo>();

            OnEndCancelEvent(canCleObject);
             * */

            #endregion

            return true;
        }

        #endregion

        #region PersistentOrder 2211-2239

        /// <summary>
        /// 检验并持久化柜台委托单
        /// </summary>
        /// <param name="stockorder">原始委托</param>
        /// <param name="strMessage">错误信息</param>
        /// <param name="outEntity">柜台委托</param>
        /// <returns>是否成功</returns>
        public override bool PersistentOrder(StockOrderRequest stockorder, ref XH_TodayEntrustTableInfo outEntity,
                                             ref string strMessage)
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
        private bool PersistentOrder(ref XH_TodayEntrustTableInfo outEntity, ref string strMessage)
        {
            #region 初始化参数

            //取代码对应品种的交易币种
            if (!GetCurrencyType())
            {
                strMessage = "GT-2226:[现货委托持久化]无法获取交易币种";
                return false;
            }

            if (CurrencyType == -1)
            {
                strMessage = "GT-2226:[现货委托持久化]无法获取交易币种";
                return false;
            }

            //预成交金额
            decimal predealCapital = 0;
            //预成交费用
            decimal predealCost = 0;

            //预成交总金额(根据买卖的不同，不一定=predealCapital+predealCost)
            decimal preDealSum = 0;

            //资金及持仓帐户
            string strHoldingAccount;
            string strCapitalAccount;

            //依据交易员及委托信息取对应资金及持仓帐户
            if (!CheckAccount(out strCapitalAccount, out strHoldingAccount, out strMessage))
                return false;

            GetAccountId(strCapitalAccount, strHoldingAccount);

            if (CapitalAccountId == -1)
            {
                strMessage = "GT-2227:[现货委托持久化]无法获取资金帐号ID";
                return false;
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
                EntrustNumber = XHCommonLogic.BuildXhOrder(ref outEntity, Request, HoldingAccount, CapitalAccount,
                                                           CurrencyType,
                                                           ref strMessage);
            }
            catch (Exception ex)
            {
                DeleteEntrust(EntrustNumber);
                LogHelper.WriteError(ex.Message, ex);
                strMessage = "GT-2225:[现货委托持久化]无法创建委托";
                LogHelper.WriteDebug(strMessage + ":" + outEntity);

                return false;
            }

            #endregion

            #region Persist流程

            //persistent流程
            //1.资金处理,可用资金减去预成交总金额，总冻结资金加上预成交总金额
            //2.冻结资金处理，生成一条冻结记录
            //3.持仓处理：只有卖出时才会有持仓处理，可用持仓减少委托量，总冻结持仓增加委托量
            //4.持仓冻结处理：只有卖出时才会有持仓冻结处理，生成一条冻结记录

            //实际处理：
            //2.4放在一个事务中进行，当成功后再进行1.3的处理
            bool isSuccess = false;

            #region 资金预处理


            var caMemory = MemoryDataManager.XHCapitalMemoryList.GetByCapitalAccountLogo(CapitalAccountId);

            if (caMemory == null)
            {
                strMessage = "GT-2211:[现货委托持久化]资金帐户不存在:" + CapitalAccount;

                return false;
            }

            if (Request.BuySell == GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying)
            {
                //预成交总金额 = 预成交金额 + 预成交费用
                preDealSum = predealCapital + predealCost;
            }
            else
            {
                //预成交总金额 = 预成交费用(卖不需要加金额)
                preDealSum = predealCost;
            }

            XH_CapitalAccountTable_DeltaInfo capitalDelta = new XH_CapitalAccountTable_DeltaInfo();
            capitalDelta.CapitalAccountLogo = caMemory.Data.CapitalAccountLogo;
            capitalDelta.AvailableCapitalDelta = -preDealSum;
            capitalDelta.FreezeCapitalTotalDelta = preDealSum;

            //return caMemory.AddDelta(-preDealCapitalAmount, preDealCapitalAmount, 0, 0);
            //return caMemory.AddDelta(capitalDelta);

            #endregion

            #region 持仓预处理(买入不处理)
            decimal orderAmount = Convert.ToDecimal(Request.OrderAmount);
            XHHoldMemoryTable ahtMemory = null;
            XH_AccountHoldTableInfo_Delta holdDelta = null;
            if (Request.BuySell == GTA.VTS.Common.CommonObject.Types.TransactionDirection.Selling)
            {
                ahtMemory = MemoryDataManager.XHHoldMemoryList.GetByAccountHoldLogoId(HoldingAccountId);
                if (ahtMemory == null)
                {
                    ahtMemory = XHCommonLogic.GetHoldMemoryTable(HoldingAccount, Code, CurrencyType);
                }

                if (ahtMemory == null)
                {
                    strMessage = "GT-2212:[现货委托持久化]持仓帐户不存在:" + HoldingAccount;
                    return false;
                }

                holdDelta = new XH_AccountHoldTableInfo_Delta();
                var holdData = ahtMemory.Data;
                holdDelta.AccountHoldLogoId = holdData.AccountHoldLogoId;
                holdDelta.AvailableAmountDelta = -orderAmount;
                holdDelta.FreezeAmountDelta = orderAmount;
                holdDelta.Data = holdData;
            }

            #endregion

            #region 数据库提交动作

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
                            strMessage = "GT-2218:[现货委托持久化]资金检查,无足够可用资金";
                            throw new CheckException("XHCapitalMemoryTable.CheckAddAddDelta失败");
                        }

                        //2.冻结资金处理，生成一条冻结记录
                        PO_BuildCapitalFreezeRecord(predealCapital, predealCost, Request.BuySell, tm);

                        //3.持仓冻结处理:只有卖出是才会有持仓处理
                        if (Request.BuySell ==
                            GTA.VTS.Common.CommonObject.Types.TransactionDirection.Selling)
                        {
                            //持仓冻结处理，生成一条冻结记录
                            PO_BuildHoldFreezeRecord(tm);
                        }

                        //4.持仓处理
                        if (Request.BuySell == GTA.VTS.Common.CommonObject.Types.TransactionDirection.Selling)
                        {
                            isHoldingSuccess = ahtMemory.CheckAndAddDelta(HodingCheck, holdDelta, tm.Database, tm.Transaction);
                            if (!isHoldingSuccess)
                            {
                                strMessage = holdMessage;
                                holdMessage = "";
                                throw new CheckException("XHHoldingMemoryTable.CheckAddAddDelta失败");
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
                            strMessage = "GT-2213:[现货委托持久化]持久化失败，无法提交到数据库";
                        isSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                strMessage = "GT-2214:[现货委托持久化]持久化失败";
                LogHelper.WriteError(ex.Message, ex);
            }

            //事务失败
            if (!isSuccess)
            {
                DeleteEntrust(EntrustNumber);

                if (isCapitalSuccess)
                {
                    caMemory.RollBackMemory(capitalDelta);
                }

                if (Request.BuySell == GTA.VTS.Common.CommonObject.Types.TransactionDirection.Selling)
                {
                    if (isHoldingSuccess)
                    {
                        ahtMemory.RollBackMemory(holdDelta);
                    }
                }


                return false;
            }

            //if(isSuccess)
            //{
            //    //当提交资金、持仓 到数据库成功后，同步到内存
            //    caMemory.AddDeltaToMemory(capitalDelta);
            //    if (Request.BuySell == CommonObject.Types.TransactionDirection.Selling)
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

        private string holdMessage = "";
        private bool HodingCheck(XH_AccountHoldTableInfo hold, XH_AccountHoldTableInfo_Delta change)
        {
            string strMessage = "";
            bool result = false;

            if (Request.BuySell == GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying)
            {
                strMessage = "GT-2219:[现货委托持久化]买持仓检查,超过持仓限制";
                int position = 0;
                decimal freezeAmount = 0;

                position = Convert.ToInt32(hold.AvailableAmount);
                freezeAmount = hold.FreezeAmount;

                if (ValidateCenter.ValidateStockMinVolumeOfBusiness(Request, position, ref strMessage))
                {
                    //获取持仓限制
                    Decimal pLimit = MCService.GetPositionLimit(Request.Code).PositionValue;
                    //可用持仓+冻结量+委托量<持仓限制
                    result = pLimit >= position + freezeAmount +
                                       Convert.ToDecimal(Request.OrderAmount);
                }
            }
            else
            {
                strMessage = "GT-2220:[现货委托持久化]卖持仓检查,无持仓";

                if (hold != null)
                {
                    strMessage = "GT-2221:[现货委托持久化]卖持仓检查,无足够可用持仓";
                    int position = Convert.ToInt32(hold.AvailableAmount);
                    //持仓帐户是否存在判断
                    if (ValidateCenter.ValidateStockMinVolumeOfBusiness(Request, position, ref strMessage))
                    {
                        //已经统一使用撮合单位了
                        decimal orderAmount = Convert.ToDecimal(Request.OrderAmount); // *unitMultiple;
                        //可用持仓＞＝委托量
                        result = hold.AvailableAmount >= orderAmount;
                    }
                }
            }

            if (result)
                strMessage = "";
            else
            {
                holdMessage = strMessage;
            }

            return result;
        }

        private bool CapitalCheck(XH_CapitalAccountTableInfo capital, XH_CapitalAccountTable_DeltaInfo change)
        {
            //只有买时才检查
            if (Request.BuySell == GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying)
            {
                if (capital.AvailableCapital <= 0)
                    return false;

                return capital.AvailableCapital + change.AvailableCapitalDelta >= 0;
            }

            //卖时只要成交额大于费用即可，前面已经检查过，此处不再检查
            return true;
        }

        #region 检查方法

        /// <summary>
        /// 计算预成交金额和预成交费用
        /// </summary>
        /// <param name="strMessage">错误信息</param>
        /// <param name="predealCapital">预成交金额</param>
        /// <param name="predealCost">预成交费用</param>
        /// <returns>计算是否成功</returns>
        private bool PO_ComputePreCapital(ref string strMessage, ref decimal predealCapital, ref decimal predealCost)
        {
            bool result = false;
            predealCapital = 0;
            predealCost = 0;

            //如果量为0，直接返回0
            if (Request.OrderAmount == 0)
                return true;

            try
            {
                XHCostResult xhcr = null;
                //计价单位与交易单位倍数
                decimal unitMultiple = MCService.GetTradeUnitScale(Request.Code, Request.OrderUnitType);
                float orderPrice = 0;

                //市价委托
                if (Request.OrderWay == Types.OrderPriceType.OPTMarketPrice)
                {
                    decimal orderPriceD = (decimal)Request.OrderPrice;
                    var highLowRange = MCService.HLRangeProcessor.GetHighLowRangeValueByCommodityCode(Request.Code,
                                                                                                      orderPriceD);
                    if (highLowRange != null)
                    {
                        if (highLowRange.RangeType == GTA.VTS.Common.CommonObject.Types.HighLowRangeType.HongKongPrice) //港股类型处理
                        {
                            var hkrv = highLowRange.HongKongRangeValue;
                            if (Request.BuySell == GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying)
                            {
                                orderPrice = Convert.ToSingle(hkrv.BuyHighRangeValue);
                            }
                            else
                            {
                                orderPrice = Convert.ToSingle(hkrv.SellHighRangeValue);
                            }
                        }
                        else //其它类型处理
                        {
                            orderPrice = Convert.ToSingle(highLowRange.HighRangeValue);
                        }

                        //以计价单位计算的委托量
                        int orderAmount = Convert.ToInt32(Request.OrderAmount);//*Convert.ToDouble(unitMultiple));

                        //预成交总金额 委托量 * 计价单位与交易单位倍数 * 委托价
                        predealCapital = orderAmount * Convert.ToDecimal(orderPrice) * unitMultiple;
                        //预成交费用
                        xhcr = MCService.ComputeXHCost(Request.Code, orderPrice, orderAmount,
                                                       Request.OrderUnitType, Request.BuySell);
                        //预成交费用
                        predealCost = xhcr.CoseSum;
                        result = true;
                    }
                    else
                    {
                        strMessage = "GT-2215:[现货委托持久化]商品无涨跌幅设置";
                    }
                }
                else //限价委托计算( 委托价*委托量 + 费用)
                {
                    //成本计算器
                    xhcr = MCService.ComputeXHCost(Request);
                    //预成交总金额 委托量 * 计价单位与交易单位倍数 * 委托价
                    predealCapital = Convert.ToDecimal(Request.OrderAmount) * unitMultiple *
                                     Convert.ToDecimal(Request.OrderPrice);
                    //预成交费用
                    predealCost = xhcr.CoseSum;
                    result = true;
                }
            }
            catch (Exception ex)
            {
                if (ex is VTException)
                {
                    strMessage = ex.ToString();
                }
                else
                {
                    strMessage = "GT-2216:[现货委托持久化]成交金额及费用计算异常.";
                }

                LogHelper.WriteError(ex.Message, ex);
            }
            return result;
        }

        private bool PO_CapitalValidate(decimal preDealCost, decimal preDealCapital, ref string strMessage)
        {
            bool result = false;

            //获取现货对应币种资金帐户实体
            var catMemory =
                MemoryDataManager.XHCapitalMemoryList.GetByCapitalAccountLogo(CapitalAccountId);
            if (catMemory == null)
            {
                strMessage = "GT-2217:[现货委托持久化]资金检查,资金帐户不存在";
                return false;
            }

            var cat = catMemory.Data;

            //资金帐户是否存在判断
            if (cat != null)
            {
                if (Request.BuySell == GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying)
                {
                    result = cat.AvailableCapital >= preDealCost + preDealCapital;
                }
                else
                {
                    result = cat.AvailableCapital + preDealCapital >= preDealCost;
                }

                strMessage = "GT-2218:[现货委托持久化]资金检查,无足够可用资金";
            }
            else
            {
                strMessage = "GT-2217:[现货委托持久化]资金检查,资金帐户不存在";
            }

            //成功时需要清空错误信息。
            if (result)
                strMessage = "";

            return result;
        }

        private bool PO_HoldValidate(ref string message)
        {
            if (Request.BuySell == GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying)
            {
                return PO_HoldValidate_Buy(ref message);
            }

            return PO_HoldValidate_Sell(ref message);
        }

        private bool PO_HoldValidate_Buy(ref string strMessage)
        {
            bool result = false;
            strMessage = "GT-2219:[现货委托持久化]买持仓检查,超过持仓限制";

            int position = 0;
            decimal freezeAmount = 0;


            var ahtMemory = MemoryDataManager.XHHoldMemoryList.GetByAccountHoldLogoId(HoldingAccountId);
            if (ahtMemory == null)
            {
                ahtMemory = XHCommonLogic.GetHoldMemoryTable(HoldingAccount, Code, CurrencyType);
            }

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
                    position = Convert.ToInt32(aht.AvailableAmount);
                    freezeAmount = aht.FreezeAmount;
                }
            }

            if (ValidateCenter.ValidateStockMinVolumeOfBusiness(Request, position, ref strMessage))
            {
                //获取持仓限制
                Decimal pLimit = MCService.GetPositionLimit(Request.Code).PositionValue;
                //可用持仓+冻结量+委托量<持仓限制
                result = pLimit >= position + freezeAmount +
                                   Convert.ToDecimal(Request.OrderAmount);
            }

            //成功时需要清空错误信息。
            if (result)
                strMessage = "";

            return result;
        }

        private bool PO_HoldValidate_Sell(ref string strMessage)
        {
            bool result = false;

            strMessage = "GT-2220:[现货委托持久化]卖持仓检查,无持仓";

            var ahtMemory = MemoryDataManager.XHHoldMemoryList.GetByHoldAccountAndCurrencyType(HoldingAccount, Code,
                                                                                               CurrencyType);

            if (ahtMemory == null)
            {
                ahtMemory = XHCommonLogic.GetHoldMemoryTable(HoldingAccount, Code, CurrencyType);
            }

            if (ahtMemory == null)
            {
                return false;
            }

            var aht = ahtMemory.Data;
            if (aht == null)
            {
                return false;
            }

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

            strMessage = "GT-2221:[现货委托持久化]卖持仓检查,无足够可用持仓";
            int position = Convert.ToInt32(aht.AvailableAmount);
            //持仓帐户是否存在判断
            if (ValidateCenter.ValidateStockMinVolumeOfBusiness(Request, position, ref strMessage))
            {
                //已经统一使用撮合单位了
                decimal orderAmount = Convert.ToDecimal(Request.OrderAmount); // *unitMultiple;
                //可用持仓＞＝委托量
                result = aht.AvailableAmount >= orderAmount;
            }

            if (result)
                strMessage = "";

            return result;
        }

        #endregion

        #region Persist功能方法

        private bool CheckAccount(out string strCapitalAccount, out string strHoldingAccount, out string strMessage)
        {
            //资金及持仓帐户状态
            bool bholdAccount, bCapitalAccount;

            counterCacher.GetHoldingAccountByTraderInfo(Request.TraderId, Request.Code,
                                                             out strHoldingAccount, out bholdAccount,
                                                             out strCapitalAccount, out bCapitalAccount);

            if (!bholdAccount)
            {
                strMessage = "GT-2222:[现货委托持久化]持仓帐户无交易权限";
                return false;
            }

            if (!bCapitalAccount)
            {
                strMessage = "GT-2223:[现货委托持久化]资金帐户无交易权限";
                return false;
            }

            strMessage = "";
            return true;
        }

        /// <summary>
        /// 获取账户id
        /// </summary>
        /// <param name="strCapitalAccount"></param>
        /// <param name="strHoldingAccount"></param>
        private void GetAccountId(string strCapitalAccount, string strHoldingAccount)
        {
            CapitalAccount = strCapitalAccount;
            HoldingAccount = strHoldingAccount;

            CapitalAccountId = MemoryDataManager.XHCapitalMemoryList.GetCapitalAccountLogo(CapitalAccount, CurrencyType);
            HoldingAccountId = MemoryDataManager.XHHoldMemoryList.GetAccountHoldLogoId(HoldingAccount, Code,
                                                                                       CurrencyType);
        }

        /// <summary>
        /// 获取币种
        /// </summary>
        /// <returns></returns>
        private bool GetCurrencyType()
        {
            var currOjb = MCService.SpotTradeRules.GetCurrencyTypeByCommodityCode(Code);
            if (currOjb == null)
            {
                return false;
            }

            CurrencyType = currOjb.CurrencyTypeID;
            return true;
        }

        #endregion

        #region Persist失败处理方法

        private void DeleteEntrust(string entrustNumber)
        {
            if (string.IsNullOrEmpty(entrustNumber))
                return;

            //删除前面创建的柜台委托单实例
            bool isSuccess = XHDataAccess.DeleteTodayEntrust(entrustNumber);
            if (!isSuccess)
            {
                RescueManager.Instance.Record_XH_DeleteTodayEntrust(entrustNumber);
            }
        }

        /// <summary>
        /// Persist检查失败时的消息处理
        /// </summary>
        /// <param name="strMessage"></param>
        private void PO_ValidateFailureProcess(ref string strMessage)
        {
            if (strMessage.IndexOf("GT") == -1)
                strMessage = "GT-2224:[现货委托持久化]持久化检查失败," + strMessage;

            LogHelper.WriteDebug(strMessage);
        }

        #endregion

        #region 持久化方法

        /// <summary>
        /// 现货持仓冻结处理
        /// </summary>
        /// <param name="tm"></param>
        /// <returns></returns>
        private int PO_BuildHoldFreezeRecord(ReckoningTransaction tm)
        {
            LogHelper.WriteDebug("现货持仓冻结处理XHSellOrderLogicFlow.PO_ProcessXhHoldingAccountFreeze");

            var ahft = new XH_AcccountHoldFreezeTableInfo();
            //现货持仓帐户标识
            ahft.AccountHoldLogo = HoldingAccountId;
            //委托单号
            ahft.EntrustNumber = EntrustNumber;
            //冻结时间
            ahft.FreezeTime = DateTime.Now;
            //解冻时间
            ahft.ThawTime = DateTime.Now;
            //冻结数量
            //decimal unitMultiple = MCService.GetTradeUnitScale(stockorder.Code, stockorder.OrderUnitType);
            decimal orderAmount = Convert.ToDecimal(Request.OrderAmount); // *unitMultiple;
            ahft.PrepareFreezeAmount = Convert.ToInt32(orderAmount);
            //冻结类型
            ahft.FreezeTypeLogo = (int)Types.FreezeType.DelegateFreeze;

            //string format = "现货持仓冻结处理[委托单号={0},冻结总量={1},冻结时间={2},解冻时间={3},冻结类型={4},现货帐户持仓标识={5},]";
            //string desc = string.Format(format, ahft.EntrustNumber, ahft.PrepareFreezeAmount, ahft.FreezeTime,
            //                            ahft.ThawTime, ahft.FreezeTypeLogo, ahft.HoldFreezeLogoId);
            //LogHelper.WriteDebug(desc);

            XH_AcccountHoldFreezeTableDal dal = new XH_AcccountHoldFreezeTableDal();
            return dal.Add(ahft, tm.Database, tm.Transaction);
        }

        private int PO_BuildCapitalFreezeRecord(decimal predealCapital, decimal predealCost, GTA.VTS.Common.CommonObject.Types.TransactionDirection buySellType, ReckoningTransaction tm)
        {
            var caft = new XH_CapitalAccountFreezeTableInfo();
            ;
            //委托单号
            caft.EntrustNumber = EntrustNumber;

            //卖不产生冻结金额，只有费用
            if (buySellType == GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying)
            {
                //冻结 预成交金额
                caft.FreezeCapitalAmount = predealCapital;
            }

            //冻结 预成交费用
            caft.FreezeCost = predealCost;
            //冻结时间
            caft.FreezeTime = DateTime.Now;
            //解冻时间
            caft.ThawTime = DateTime.Now;
            //冻结类型
            caft.FreezeTypeLogo = (int)Types.FreezeType.DelegateFreeze;

            caft.OweCosting = 0;

            caft.CapitalAccountLogo = CapitalAccountId;

            string format =
                "现货资金冻结处理XHBuyOrderLogicFlow.PO_BuildCapitalFreezeRecord[委托单号={0},冻结时间={1},解冻时间={2},冻结类型={3},预成交金额={4},预成交费用={5},资金账户ID={6}]";

            string desc = string.Format(format, caft.EntrustNumber, caft.FreezeTime, caft.ThawTime, caft.FreezeTypeLogo,
                                        caft.FreezeCapitalAmount, caft.FreezeCost, CapitalAccountId);
            LogHelper.WriteDebug(desc);

            XH_CapitalAccountFreezeTableDal dal = new XH_CapitalAccountFreezeTableDal();
            return dal.Add(caft, tm.Database, tm.Transaction);
        }

        #endregion

        #endregion
    }
}