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
using ReckoningCounter.DAL.Data.HK;
using ReckoningCounter.Entity;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.MemoryData;
using ReckoningCounter.MemoryData.HK.Hold;

#endregion

namespace ReckoningCounter.BLL.DelegateAcceptOffer.OrderAccepter.Logic
{
    /// <summary>
    /// Title:港股下单预处理逻辑，错误编码2400-2429
    /// 
    /// </summary>
    public class HKAcceptLogic : AcceptLogic<HKOrderRequest, HK_TodayEntrustInfo, HK_TodayTradeInfo>
    {
        #region CancelOrderValidate 2400-2405

        protected override CounterCache GetCounterCache()
        {
            return HKCounterCache.Instance;
        }

        /// <summary>
        /// 撤单校验-检查委托单当前状态是否可撤
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        /// <param name="tet">委托实体</param>
        /// <param name="strMessage">错误信息</param>
        /// <returns>校验是否通过</returns>
        public override bool CancelOrderValidate(string entrustNumber, out HK_TodayEntrustInfo tet,
                                                 ref string strMessage)
        {
            var result = false;
            HK_TodayEntrustDal dal = new HK_TodayEntrustDal();
            tet = dal.GetModel(entrustNumber);
            if (tet != null)
            {
                if (tet.OrderStatusID == (int)Entity.Contants.Types.OrderStateType.DOSUnRequired ||
                    tet.OrderStatusID == (int)Entity.Contants.Types.OrderStateType.DOSRequiredSoon ||
                    tet.OrderStatusID == (int)Entity.Contants.Types.OrderStateType.DOSIsRequired ||
                    tet.OrderStatusID == (int)Entity.Contants.Types.OrderStateType.DOSPartDealed)
                {
                    result = true;
                }
                else
                {
                    strMessage = "GT-2400:[港股撤单校验]该委托状态的委托单不可撤.委托状态=" +
                                 Entity.Contants.Types.GetOrderStateMsg(tet.OrderStatusID);
                }
            }
            else
            {
                strMessage = "GT-2401:[港股撤单校验]委托单不存在.";
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
        public override bool InternalCancelOrder(HK_TodayEntrustInfo tet, string strMcErrorMessage)
        {
            #region 初始化参数

            //柜台委托单号
            EntrustNumber = tet.EntrustNumber;

            string errInfo = "[委托单号=" + EntrustNumber + ",Message=" + strMcErrorMessage + "]";
            LogHelper.WriteDebug(
                "------xxxxxx------开始进行港股内部撤单(撤单时委托状态为未报或者买卖报盘返回的委托单号无效)XHAcceptLogic.InternalCancelOrder" + errInfo);

            Code = tet.Code;
            CapitalAccount = tet.CapitalAccount; //资金帐户
            HoldingAccount = tet.HoldAccount; //持仓帐户
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

            tet.McOrderID = orderNo;
            //假的撮合委托单号也要写入委托信息中，以便后面好删除缓存
            //=====update 2009-11-09 李健华====
            //HKDataAccess.UpdateEntrustTable(tet);
            //这里只是更新撮合ID编号，以免更新了别的字段
            HKDataAccess.UpdateEntrustMcOrderID(EntrustNumber, orderNo);
            //=============
            LogHelper.WriteDebug("HKAcceptLogic.InternalCancelOrder添加委托单号映射到ConterCache,OrderNo=" + orderNo);
            //柜台缓存委托相关信息
            var item = new OrderCacheItem(tet.CapitalAccount, tet.HoldAccount, tet.EntrustNumber,
                                          (Types.TransactionDirection)Enum.Parse(typeof(Types.TransactionDirection),
                                          tet.BuySellTypeID.ToString()));
            item.EntrustAmount = tet.EntrustAmount;
            item.Code = tet.Code;
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

            ReckonCenter.Instace.AcceptCancelHKOrderRpt(coe);

            #endregion

            //内部撤单流程
            //1.资金冻结处理，把资金冻结记录里的金额和费用全部还给资金表，删除冻结记录(实际上清零，不删除，盘后统一删）
            //2.资金处理，把从资金冻结记录还回来的金额和费用加到可用资金，并减去总冻结资金
            //3.持仓冻结处理(买入不处理）,把持仓冻结记录中的冻结量还给持仓表，删除冻结记录(实际上清零，不删除，盘后统一删）
            //4.持仓处理（买入不处理），把从持仓冻结记录还回来的持仓量加到可用持仓，并减去总冻结持仓
            //5.委托处理，更新委托状态，成交量，撤单量以及委托消息(OrderMessage)

            return true;
        }

        #endregion

        #region PersistentOrder 2411-2439

        private string holdMessage = "";

        /// <summary>
        /// 检验并持久化柜台委托单
        /// </summary>
        /// <param name="stockorder">原始委托</param>
        /// <param name="strMessage">错误信息</param>
        /// <param name="outEntity">柜台委托</param>
        /// <returns>是否成功</returns>
        public override bool PersistentOrder(HKOrderRequest hkorder, ref HK_TodayEntrustInfo outEntity,
                                             ref string strMessage)
        {
            Request = hkorder;
            Code = hkorder.Code;
            bool result = PersistentOrder(ref outEntity, ref strMessage);

            return result;
        }

        /// <summary>
        /// 检验并持久化柜台委托单
        /// </summary>
        /// <param name="strMessage">错误消息</param>
        /// <param name="outEntity">柜台持久化后的对象</param>
        /// <returns>是否成功</returns>
        private bool PersistentOrder(ref HK_TodayEntrustInfo outEntity, ref string strMessage)
        {
            #region 初始化参数

            //取代码对应品种的交易币种
            if (!GetCurrencyType())
            {
                strMessage = "GT-2426:[港股委托持久化]无法获取交易币种";
                return false;
            }

            if (CurrencyType == -1)
            {
                strMessage = "GT-2426:[港股委托持久化]无法获取交易币种";
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
                strMessage = "GT-2427:[港股委托持久化]无法获取资金帐号ID";
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
                EntrustNumber = HKCommonLogic.BuildHKOrder(ref outEntity, Request, HoldingAccount, CapitalAccount,
                                                           CurrencyType, ref strMessage);
            }
            catch (Exception ex)
            {
                DeleteEntrust(EntrustNumber);
                LogHelper.WriteError(ex.Message, ex);
                strMessage = "GT-2425:[港股委托持久化]无法创建委托";
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

            var caMemory = MemoryDataManager.HKCapitalMemoryList.GetByCapitalAccountLogo(CapitalAccountId);

            if (caMemory == null)
            {
                strMessage = "GT-2411:[港股委托持久化]资金帐户不存在:" + CapitalAccount;

                return false;
            }

            if (Request.BuySell == Types.TransactionDirection.Buying)
            {
                //预成交总金额 = 预成交金额 + 预成交费用
                preDealSum = predealCapital + predealCost;
            }
            else
            {
                //预成交总金额 = 预成交费用(卖不需要加金额)
                preDealSum = predealCost;
            }

            HK_CapitalAccount_DeltaInfo capitalDelta = new HK_CapitalAccount_DeltaInfo();
            capitalDelta.CapitalAccountLogo = caMemory.Data.CapitalAccountLogo;
            capitalDelta.AvailableCapitalDelta = -preDealSum;
            capitalDelta.FreezeCapitalTotalDelta = preDealSum;

            #endregion

            #region 持仓预处理(买入不处理)

            decimal orderAmount = Convert.ToDecimal(Request.OrderAmount);
            HKHoldMemoryTable ahtMemory = null;
            HK_AccountHoldInfo_Delta holdDelta = null;
            if (Request.BuySell == Types.TransactionDirection.Selling)
            {
                ahtMemory = MemoryDataManager.HKHoldMemoryList.GetByAccountHoldLogoId(HoldingAccountId);
                if (ahtMemory == null)
                {
                    ahtMemory = HKCommonLogic.GetHoldMemoryTable(HoldingAccount, Code, CurrencyType);
                }

                if (ahtMemory == null)
                {
                    strMessage = "GT-2412:[港股委托持久化]持仓帐户不存在:" + HoldingAccount;
                    return false;
                }

                holdDelta = new HK_AccountHoldInfo_Delta();
                var holdData = ahtMemory.Data;
                holdDelta.AccountHoldLogoId = holdData.AccountHoldLogoID;
                holdDelta.AvailableAmountDelta = -orderAmount;
                holdDelta.FreezeAmountDelta = orderAmount;
                //holdDelta.Data = holdData;
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
                            strMessage = "GT-2418:[港股委托持久化]资金检查,无足够可用资金";
                            throw new CheckException("XHCapitalMemoryTable.CheckAddAddDelta失败");
                        }

                        //2.冻结资金处理，生成一条冻结记录
                        PO_BuildCapitalFreezeRecord(predealCapital, predealCost, Request.BuySell, tm);

                        //3.持仓冻结处理:只有卖出是才会有持仓处理
                        if (Request.BuySell ==
                            Types.TransactionDirection.Selling)
                        {
                            //持仓冻结处理，生成一条冻结记录
                            PO_BuildHoldFreezeRecord(tm);
                        }

                        //4.持仓处理
                        if (Request.BuySell == Types.TransactionDirection.Selling)
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
                            strMessage = "GT-2413:[港股委托持久化]持久化失败，无法提交到数据库";
                        isSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                strMessage = "GT-2414:[港股委托持久化]持久化失败";
                LogHelper.WriteError(ex.Message, ex);
            }

            //事务失败
            if (!isSuccess)
            {
                DeleteEntrust(EntrustNumber);

                if (isCapitalSuccess)
                {
                    //caMemory.RollBackMemory(capitalDelta);
                }

                if (Request.BuySell == Types.TransactionDirection.Selling)
                {
                    if (isHoldingSuccess)
                    {
                        //ahtMemory.RollBackMemory(holdDelta);
                    }
                }


                return false;
            }

            #endregion

            #endregion

            return true;
        }

        private bool HodingCheck(HK_AccountHoldInfo hold, HK_AccountHoldInfo_Delta change)
        {
            string strMessage = "";
            bool result = false;

            if (Request.BuySell == Types.TransactionDirection.Buying)
            {
                strMessage = "GT-2419:[港股委托持久化]买持仓检查,超过持仓限制";
                int position = 0;
                decimal freezeAmount = 0;

                position = Convert.ToInt32(hold.AvailableAmount);
                freezeAmount = hold.FreezeAmount;

                if (ValidateCenter.ValidateHKMinVolumeOfBusiness(Request, position, ref strMessage))
                {
                    //获取持仓限制
                    Decimal pLimit = MCService.GetPositionLimit(Request.Code, Types.BreedClassTypeEnum.HKStock).PositionValue;
                    //可用持仓+冻结量+委托量<持仓限制
                    result = pLimit >= position + freezeAmount + Convert.ToDecimal(Request.OrderAmount);
                }
            }
            else
            {
                strMessage = "GT-2420:[港股委托持久化]卖持仓检查,无持仓";

                if (hold != null)
                {
                    strMessage = "GT-2421:[港股委托持久化]卖持仓检查,无足够可用持仓";
                    int position = Convert.ToInt32(hold.AvailableAmount);
                    //持仓帐户是否存在判断
                    if (ValidateCenter.ValidateHKMinVolumeOfBusiness(Request, position, ref strMessage))
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

        private bool CapitalCheck(HK_CapitalAccountInfo capital, HK_CapitalAccount_DeltaInfo change)
        {
            //只有买时才检查
            if (Request.BuySell == Types.TransactionDirection.Buying)
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
                HKCostResult xhcr = null;
                //计价单位与交易单位倍数
                //update 李健华 2009-10-26
                //decimal unitMultiple = MCService.GetTradeUnitScale(Request.Code, Request.OrderUnitType);
                decimal unitMultiple = MCService.GetTradeUnitScale(Types.BreedClassTypeEnum.HKStock, Request.Code, Request.OrderUnitType);
                //=========
                //float orderPrice = 0;

                //成本计算器
                xhcr = MCService.ComputeHKCost(Request);
                //预成交总金额 委托量 * 计价单位与交易单位倍数 * 委托价
                predealCapital = Convert.ToDecimal(Request.OrderAmount) * unitMultiple *
                                 Convert.ToDecimal(Request.OrderPrice);
                //预成交费用
                predealCost = xhcr.CoseSum;
                result = true;

                #region 旧逻辑-港股无市价单，必须有价格
                /*
                //市价委托(TODO:待重写)
                if (Request.OrderWay == Types.HKPriceType.LO)
                {
                    decimal orderPriceD = (decimal) Request.OrderPrice;
                    var highLowRange = MCService.HLRangeProcessor.GetHighLowRangeValueByCommodityCode(Request.Code,
                                                                                                      orderPriceD);
                    if (highLowRange != null)
                    {
                        if (highLowRange.RangeType == Types.HighLowRangeType.HongKongPrice) //港股类型处理
                        {
                            var hkrv = highLowRange.HongKongRangeValue;
                            if (Request.BuySell == Types.TransactionDirection.Buying)
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
                        int orderAmount = Convert.ToInt32(Request.OrderAmount); //*Convert.ToDouble(unitMultiple));

                        //预成交总金额 委托量 * 计价单位与交易单位倍数 * 委托价
                        predealCapital = orderAmount*Convert.ToDecimal(orderPrice)*unitMultiple;
                        //预成交费用
                        xhcr = MCService.ComputeHKCost(Request.Code, orderPrice, orderAmount,
                                                       Request.OrderUnitType, Request.BuySell);
                        //预成交费用
                        predealCost = xhcr.CoseSum;
                        result = true;
                    }
                    else
                    {
                        strMessage = "GT-2415:[港股委托持久化]商品无涨跌幅设置";
                    }
                }
                else //限价委托计算( 委托价*委托量 + 费用)
                {
                    //成本计算器
                    xhcr = MCService.ComputeHKCost(Request);
                    //预成交总金额 委托量 * 计价单位与交易单位倍数 * 委托价
                    predealCapital = Convert.ToDecimal(Request.OrderAmount)*unitMultiple*
                                     Convert.ToDecimal(Request.OrderPrice);
                    //预成交费用
                    predealCost = xhcr.CoseSum;
                    result = true;
                }
                 * 
                 * */
                #endregion
            }
            catch (Exception ex)
            {
                if (ex is VTException)
                {
                    strMessage = ex.ToString();
                }
                else
                {
                    strMessage = "GT-2416:[港股委托持久化]成交金额及费用计算异常.";
                }

                LogHelper.WriteError(ex.Message, ex);
            }
            return result;
        }

        private bool PO_CapitalValidate(decimal preDealCost, decimal preDealCapital, ref string strMessage)
        {
            bool result = false;

            //获取港股对应币种资金帐户实体
            var catMemory = MemoryDataManager.HKCapitalMemoryList.GetByCapitalAccountLogo(CapitalAccountId);
            if (catMemory == null)
            {
                strMessage = "GT-2417:[港股委托持久化]资金检查,资金帐户不存在";
                return false;
            }

            var cat = catMemory.Data;

            //资金帐户是否存在判断
            if (cat != null)
            {
                if (Request.BuySell == Types.TransactionDirection.Buying)
                {
                    result = cat.AvailableCapital >= preDealCost + preDealCapital;
                }
                else
                {
                    result = cat.AvailableCapital + preDealCapital >= preDealCost;
                }

                strMessage = "GT-2418:[港股委托持久化]资金检查,无足够可用资金";
            }
            else
            {
                strMessage = "GT-2417:[港股委托持久化]资金检查,资金帐户不存在";
            }

            //成功时需要清空错误信息。
            if (result)
                strMessage = "";

            return result;
        }

        private bool PO_HoldValidate(ref string message)
        {
            if (Request.BuySell == Types.TransactionDirection.Buying)
            {
                return PO_HoldValidate_Buy(ref message);
            }

            return PO_HoldValidate_Sell(ref message);
        }

        private bool PO_HoldValidate_Buy(ref string strMessage)
        {
            bool result = false;
            strMessage = "GT-2419:[港股委托持久化]买持仓检查,超过持仓限制";

            int position = 0;
            decimal freezeAmount = 0;

            var ahtMemory = MemoryDataManager.HKHoldMemoryList.GetByAccountHoldLogoId(HoldingAccountId);
            if (ahtMemory == null)
            {
                ahtMemory = HKCommonLogic.GetHoldMemoryTable(HoldingAccount, Code, CurrencyType);
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
                        HoldingAccountId = aht.AccountHoldLogoID;
                    }
                    //======================
                    position = Convert.ToInt32(aht.AvailableAmount);
                    freezeAmount = aht.FreezeAmount;
                }
            }

            if (ValidateCenter.ValidateHKMinVolumeOfBusiness(Request, position, ref strMessage))
            {
                //获取持仓限制
                Decimal pLimit = MCService.GetPositionLimit(Request.Code, Types.BreedClassTypeEnum.HKStock).PositionValue;
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

            strMessage = "GT-2420:[港股委托持久化]卖持仓检查,无持仓";

            var ahtMemory = MemoryDataManager.HKHoldMemoryList.GetByHoldAccountAndCurrencyType(HoldingAccount, Code,
                                                                                               CurrencyType);

            if (ahtMemory == null)
            {
                ahtMemory = HKCommonLogic.GetHoldMemoryTable(HoldingAccount, Code, CurrencyType);
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
                HoldingAccountId = aht.AccountHoldLogoID;
            }
            //======================

            strMessage = "GT-2421:[港股委托持久化]卖持仓检查,无足够可用持仓";
            int position = Convert.ToInt32(aht.AvailableAmount);
            //持仓帐户是否存在判断
            if (ValidateCenter.ValidateHKMinVolumeOfBusiness(Request, position, ref strMessage))
            {
                //已经统一使用撮合单位了
                decimal orderAmount = Convert.ToDecimal(Request.OrderAmount); // *unitMultiple;
                result = aht.AvailableAmount >= orderAmount; //可用持仓＞＝委托量
            }
            if (result)
            {
                strMessage = "";
            }
            return result;
        }

        #endregion

        #region Persist功能方法

        private bool CheckAccount(out string strCapitalAccount, out string strHoldingAccount, out string strMessage)
        {
            //资金及持仓帐户状态
            bool bholdAccount, bCapitalAccount;

            this.counterCacher.GetHoldingAccountByTraderInfo(Request.TraderId, Request.Code, Types.BreedClassTypeEnum.HKStock,
                                                             out strHoldingAccount, out bholdAccount,
                                                             out strCapitalAccount, out bCapitalAccount);

            if (!bholdAccount)
            {
                strMessage = "GT-2422:[港股委托持久化]持仓帐户无交易权限";
                return false;
            }

            if (!bCapitalAccount)
            {
                strMessage = "GT-2423:[港股委托持久化]资金帐户无交易权限";
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
            CapitalAccountId = MemoryDataManager.HKCapitalMemoryList.GetCapitalAccountLogo(CapitalAccount, CurrencyType);
            HoldingAccountId = MemoryDataManager.HKHoldMemoryList.GetAccountHoldLogoId(HoldingAccount, Code,
                                                                                       CurrencyType);
        }

        /// <summary>
        /// 获取币种
        /// </summary>
        /// <returns></returns>
        private bool GetCurrencyType()
        {
            // var currOjb = MCService.SpotTradeRules.GetCurrencyTypeByCommodityCode(Code);
            //==update 2009-10-26 李健华
            var currOjb = MCService.CommonPara.GetCurrencyTypeByCommodityCode(Types.BreedClassTypeEnum.HKStock, Code);
            //===
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
            bool isSuccess = HKDataAccess.DeleteTodayEntrust(entrustNumber);
            if (!isSuccess)
            {
                RescueManager.Instance.Record_HK_DeleteTodayEntrust(entrustNumber);
            }
        }

        /// <summary>
        /// Persist检查失败时的消息处理
        /// </summary>
        /// <param name="strMessage"></param>
        private void PO_ValidateFailureProcess(ref string strMessage)
        {
            if (strMessage.IndexOf("GT") == -1)
                strMessage = "GT-2424:[港股委托持久化]持久化检查失败," + strMessage;

            LogHelper.WriteDebug(strMessage);
        }

        #endregion

        #region 持久化方法

        /// <summary>
        /// 港股持仓冻结处理
        /// </summary>
        /// <param name="tm"></param>
        /// <returns></returns>
        private int PO_BuildHoldFreezeRecord(ReckoningTransaction tm)
        {
            LogHelper.WriteDebug("港股持仓冻结处理XHSellOrderLogicFlow.PO_ProcessXhHoldingAccountFreeze");

            HK_AcccountHoldFreezeInfo hahf = new HK_AcccountHoldFreezeInfo();

            hahf.AccountHoldLogo = HoldingAccountId; //港股持仓帐户标识
            hahf.EntrustNumber = EntrustNumber; //委托单号
            hahf.FreezeTime = DateTime.Now; //冻结时间
            hahf.ThawTime = DateTime.Now; //解冻时间

            //decimal unitMultiple = MCService.GetTradeUnitScale(stockorder.Code, stockorder.OrderUnitType);
            decimal orderAmount = Convert.ToDecimal(Request.OrderAmount); // *unitMultiple;
            hahf.PrepareFreezeAmount = Convert.ToInt32(orderAmount); //冻结数量
            hahf.FreezeTypeID = (int)Entity.Contants.Types.FreezeType.DelegateFreeze; //冻结类型


            HK_AcccountHoldFreezeDal dal = new HK_AcccountHoldFreezeDal();
            try
            {
                return dal.Add(hahf, tm.Database, tm.Transaction);
            }
            catch (Exception ex)
            {
                string txt = "AccountHoldLogo={0},EntrustNumber={1},FreezeTime={2},ThawTime={3},PrepareFreezeAmount={4},FreezeTypeID={5}";
                txt = string.Format(txt, hahf.AccountHoldLogo, hahf.EntrustNumber, hahf.FreezeTime, hahf.ThawTime, hahf.PrepareFreezeAmount, hahf.FreezeTypeID);
                LogHelper.WriteDebug("港股持仓冻结处理实体内容" + txt);
                throw ex;
            }
        }

        private int PO_BuildCapitalFreezeRecord(decimal predealCapital, decimal predealCost,
                                                Types.TransactionDirection buySellType, ReckoningTransaction tm)
        {
            HK_CapitalAccountFreezeInfo caft = new HK_CapitalAccountFreezeInfo();

            caft.EntrustNumber = EntrustNumber; //委托单号
            //卖不产生冻结金额，只有费用
            if (buySellType == Types.TransactionDirection.Buying)
            {
                caft.FreezeCapitalAmount = predealCapital; //冻结 预成交金额
            }
            caft.FreezeCost = predealCost; //冻结 预成交费用
            caft.FreezeTime = DateTime.Now; //冻结时间
            caft.ThawTime = DateTime.Now; //解冻时间
            caft.FreezeTypeLogo = (int)Entity.Contants.Types.FreezeType.DelegateFreeze; //冻结类型
            caft.OweCosting = 0;
            caft.CapitalAccountLogo = CapitalAccountId;
            string format =
                "港股资金冻结处理HKBuyOrderLogicFlow.PO_BuildCapitalFreezeRecord[委托单号={0},冻结时间={1},解冻时间={2},冻结类型={3},预成交金额={4},预成交费用={5},资金账户ID={6}]";

            string desc = string.Format(format, caft.EntrustNumber, caft.FreezeTime, caft.ThawTime, caft.FreezeTypeLogo,
                                        caft.FreezeCapitalAmount, caft.FreezeCost, CapitalAccountId);
            LogHelper.WriteDebug(desc);

            HK_CapitalAccountFreezeDal dal = new HK_CapitalAccountFreezeDal();
            return dal.Add(caft, tm.Database, tm.Transaction);
        }

        #endregion

        #endregion
    }
}