#region Using Namespace

using System;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.DAL.MatchCenterOrderDealRpt;
using ReckoningCounter.Model;
using GTA.VTS.Common.CommonUtility;
using System.Collections.Generic;
using ReckoningCounter.DAL.CustomDataAccess;
using ReckoningCounter.BLL.DelegateValidate.Cost;
using ReckoningCounter.Entity.Contants;
using ReckoningCounter.MemoryData.QH.Hold;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.MemoryData;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

#endregion

namespace ReckoningCounter.BLL.Reckoning.Logic.SPQH
{
    /// <summary>
    /// 商品期货清算处理单元，错误编码2600-2699
    /// Create By:李健华
    /// Create date:2010-01-26
    /// </summary>
    public class SPQHReckonUnit : ReckonUnitBase<QH_TodayEntrustTableInfo, CommoditiesDealBackEntity, QH_TodayTradeTableInfo>
    {
        /// <summary>
        /// 是否已经完成当前委托清算（成交+撤单）
        /// </summary>
        protected bool hasDoneDeal;

        protected Types.FutureOpenCloseType OpenCloseType { get; set; }

        #region Overrides of ReckonUnitBase<QhTodayEntrustTable,CommoditiesDealBackEntity>

        /// <summary>
        /// 内部接收成交回报处理方法
        /// </summary>
        /// <param name="message">成交回报或者撤单回报</param>
        protected override bool InternalInsertMessage(object message)
        {
            if (hasDoneDeal)
            {
                LogHelper.WriteDebug("SPQHReckonUnit.InternalInsertMessage:委托已经完成，回报作废" + message);
                return false;
            }

            string id = string.Empty;
            string orderId = string.Empty;
            string desc = string.Empty;
            decimal dealAmount = 0;
            if (message is CommoditiesDealBackEntity)
            {
                CommoditiesDealBackEntity ido = (CommoditiesDealBackEntity)message;
                id = ido.Id;
                orderId = ido.OrderNo;
                desc = ido.DescInfo();
                dealAmount = ido.DealAmount;
            }
            else if (message is CancelOrderEntity)
            {
                CancelOrderEntity coe = (CancelOrderEntity)message;
                id = coe.Id;
                orderId = coe.OrderNo;
                desc = coe.DescInfo();
                dealAmount = coe.OrderVolume;
            }

            if (id == string.Empty)
            {
                LogHelper.WriteDebug("SPQHReckonUnit.InternalInsertMessage: 空ID");
                return false;
            }

            if (HasAddId(id))
            {
                LogHelper.WriteDebug("SPQHReckonUnit.InternalInsertMessage: 已添加ID" + id);
                return false;
            }

            string strMessage = string.Empty;
            bool isSuccess = InitializeOrderCache(orderId, ref strMessage);

            if (!isSuccess)
            {
                LogHelper.WriteInfo(strMessage);
            }

            if (HasAddReckoned(id))
            {
                LogHelper.WriteDebug("SPQHReckonUnit.InternalInsertMessage: 已处理ID" + id);
                return false;
            }

            AddID(id);

            string entrusNum = "[EntrustNumber=" + EntrustNumber + "]";
            LogHelper.WriteDebug("SPQHReckonUnit.InternalInsertMessage" + desc + entrusNum);

            //添加到内部的缓存列表
            if (message is CommoditiesDealBackEntity)
            {
                CommoditiesDealBackEntity ido = (CommoditiesDealBackEntity)message;
                dealBackList.Add(ido);
            }
            else if (message is CancelOrderEntity)
            {
                CancelOrderEntity coe = (CancelOrderEntity)message;
                cancelBackList.Add(coe);
            }

            if ((dealAmount + TradeAmount + CancelAmount) == EntrustAmount)
                return true;

            return false;
        }

        /// <summary>
        /// 清算通知接收方法,类似于原来的MessageQueue_QueueItemProcessEvent方法 
        /// </summary>
        protected override void ReckonCommitCheck()
        {
            ProcessDealCommit();
            ProcessCancelCommit();
        }

        /// <summary>
        /// 从数据库加载已清算数据
        /// </summary>
        protected override void LoadReckonedIDList()
        {
            var trades = QHDataAccess.GetTodayTradeListByEntrustNumber(EntrustNumber);
            if (trades == null)
                return;

            foreach (QH_TodayTradeTableInfo trade in trades)
            {
                AddReckonedID(trade.TradeNumber.Trim());

                if (trade.TradeTypeId == (int)Types.DealRptType.DRTDealed)
                {
                    string format = "[新TradeAmount{0}=旧TradeAmount{1}+以前成交量{2}]";
                    string msg = string.Format(format, TradeAmount + trade.TradeAmount, TradeAmount,
                                               trade.TradeAmount);
                    LogHelper.WriteDebug("GZQHReckonUnit.LoadReckonedIDList" + msg);
                    TradeAmount += trade.TradeAmount;
                }

                if (trade.TradeTypeId == (int)Types.DealRptType.DRTCanceled)
                {
                    string format = "[新CancelAmount{0}=旧CancelAmount{1}+以前成交量{2}]";
                    string msg = string.Format(format, CancelAmount + trade.TradeAmount, CancelAmount,
                                               trade.TradeAmount);
                    LogHelper.WriteDebug("GZQHReckonUnit.LoadReckonedIDList" + msg);
                    CancelAmount += trade.TradeAmount;
                }
            }

            HasLoadReckonedID = true;
        }

        protected override CounterCache GetCounterCache()
        {
            return QHCounterCache.Instance;
        }

        protected override void GetCurrencyType()
        {
            var currOjb = MCService.FuturesTradeRules.GetCurrencyTypeByCommodityCode(Code);
            if (currOjb == null)
            {
                return;
            }

            CurrencyType = currOjb.CurrencyTypeID;
        }

        protected override void GetAccountID()
        {
            CapitalAccountId = MemoryDataManager.QHCapitalMemoryList.GetCapitalAccountLogo(CapitalAccount, CurrencyType);

            //获取持仓账户需要取相反的买卖方向，因为委托买时，对应的持仓方向是卖
            var buySellType = BuySellType == GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying
                                  ? GTA.VTS.Common.CommonObject.Types.TransactionDirection.Selling
                                  : GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying;

            HoldingAccountId = MemoryDataManager.QHHoldMemoryList.GetAccountHoldLogoId(HoldingAccount, Code,
                                                                                       (int)buySellType,
                                                                                       CurrencyType);
        }

        #endregion

        #region 撤单清算

        /// <summary>
        /// 是否可以进行撤单清算
        /// </summary>
        /// <returns></returns>
        private bool CanProcessCancelCommit(out CancelOrderEntity coe)
        {
            string entrustNumString = "[EntrustNumber=" + EntrustNumber + "]";

            var list = cancelBackList.GetAllAndClear();
            if (Utils.IsNullOrEmpty(list))
            {
                coe = null;
                return false;
            }

            #region 是否已经清算完毕

            var hasDealedAmount = TradeAmount + CancelAmount;

            if (hasDealedAmount == EntrustAmount)
            {
                string msg = "CanProcessCancelCommit：当前委托已经清算完毕，撤单回报无效，清除！" + entrustNumString;
                LogHelper.WriteDebug(msg);

                //表示不需要清算了
                coe = null;
                return false;
            }
            #endregion

            #region 是否有内部撤单
            //收盘后，会被插入一个内部撤单对象，此时直接返回内部撤单对象，
            //如果还有外部撤单对象，那么做废 
            foreach (var cancelOrderEntity in list)
            {
                if (cancelOrderEntity is CancelOrderEntityEx)
                {
                    CancelOrderEntityEx coeEx = cancelOrderEntity as CancelOrderEntityEx;
                    if (coeEx.IsInternalCancelOrder)
                    {
                        coe = coeEx;
                        return true;
                    }
                }
            }
            #endregion

            if (list.Count > 1)
            {
                LogHelper.WriteDebug("GZQHReckonUnit.ProcessCancelCommit: 收到多个撤单回报，只处理第一个，其他的抛弃！" + entrustNumString);
            }

            coe = list[0];

            //如果是错误价格的撤单，马上执行
            if (coe.OrderVolume == -1)
                return true;

            //如果TradeAmount+CancelAmount+coe.Amount!=EntrustAmount,
            //那么认为虽然收到了撤单回报，但是还有部分成交回报还没回来，那么
            //先不进行撤单处理，等所有的成交回报都清算完了，最后再进行撤单处理
            decimal allAmount = TradeAmount + CancelAmount + coe.OrderVolume;
            if (allAmount < EntrustAmount)
            {
                string format2 =
                    "GZQHReckonUnit.ProcessCancelCommit尚有部分成交回报未收到或未处理[EntrustAmount={0},TradeAmount={1},CancelAmount={2},CancelBackAmount={3}]";
                string info = string.Format(format2, EntrustAmount, TradeAmount, CancelAmount, coe.OrderVolume);
                //LogHelper.WriteInfo(info);

                //还有成交回报没有回来，继续放入撤单列表等待下次撤单处理
                cancelBackList.Add(coe);

                return false;
            }

            return true;
        }

        private void ProcessCancelCommit()
        {
            CancelOrderEntity coe;
            if (!CanProcessCancelCommit(out coe))
            {
                return;
            }

            string strMessage = "";
            QH_TodayEntrustTableInfo tet;
            List<QH_TodayTradeTableInfo> trades;
            bool isSuccess = false;

            //是否是废单
            //bool isBadOrder = false;

            CrashManager.GetInstance().AddReckoningID(coe.Id);

            //错误价格的撤单
            if (coe.OrderVolume == -1)
            {
                //isBadOrder = true;
                isSuccess = InstantReckon_CancelOrder(coe, out tet, out trades, true, ref strMessage);

                ////没有成功就马上再做一次
                //if (!isSuccess)
                //    isSuccess = InstantReckon_CancelOrder(coe, out tet, out trades, true);
            }
            else
            {
                //正常撤单
                isSuccess = InstantReckon_CancelOrder(coe, out tet, out trades, false, ref strMessage);

                ////没有成功就马上再做一次
                //if (!isSuccess)
                //    isSuccess = InstantReckon_CancelOrder(coe, out tet, out trades, false);
            }

            if (!isSuccess)
            {
                //如果清算没有成功，那么从保存的回报列表中删掉，下次CrashManger可以再次发送此回报过来
                LogHelper.WriteInfo("#####################GZQHReckonUnit.ProcessCancelCommit撤单清算失败，CancelOrderEntity[ID=" + coe.Id +
                                    "],Message=" + strMessage);
                RemoveID(coe.Id);
            }

            if (!string.IsNullOrEmpty(strMessage))
                QHDataAccess.UpdateEntrustOrderMessage(EntrustNumber, strMessage);

            CrashManager.GetInstance().RemoveReckoningID(coe.Id);

            if (isSuccess)
            {
                ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo> cancelEndObject =
                    new ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo>();
                cancelEndObject.TradeID = TradeID;
                cancelEndObject.EntrustTable = tet;
                cancelEndObject.TradeTableList = trades;
                cancelEndObject.IsSuccess = true;
                cancelEndObject.Message = strMessage;

                if (tet != null && trades != null)
                    OnEndCancelEvent(cancelEndObject);
            }

            //检查是否已经全部清算完
            if ((TradeAmount + CancelAmount) == EntrustAmount)
            {
                hasDoneDeal = true;
            }
        }

        public bool InstantReckon_CancelOrder(CancelOrderEntity rde,
                                              out QH_TodayEntrustTableInfo tet, out List<QH_TodayTradeTableInfo> trades,
                                              bool isErrorPriceCancelRpt, ref string strMessage)
        {
            #region 初始化参数
            tet = null;
            trades = null;

            if (!InitializeOrderCache(rde.OrderNo, ref strMessage))
            {
                LogHelper.WriteInfo(strMessage);
                return false;
            }

            string entrustNumString = "[EntrustNumber=" + EntrustNumber + "]";
            LogHelper.WriteDebug(
                "------xxxxxx------开始进行商品期货撤单GZQHReckonUnit.InstantReckon_CancelOrder" +
                rde.DescInfo() + entrustNumString);

            //现货当日委托回报
            tet = QHDataAccess.GetEntrustTable(EntrustNumber);
            if (tet == null)
            {
                strMessage = "GT-2600:[商品期货撤单清算]无法获取委托对象，委托单号=" + EntrustNumber;
                LogHelper.WriteDebug(strMessage);
                return false;
            }

            //取代码对应品种的交易币种
            if (CurrencyType == -1)
                CurrencyType = MCService.SpotTradeRules.GetCurrencyTypeByCommodityCode(tet.ContractCode).CurrencyTypeID;

            var hasDealedAmount = TradeAmount + CancelAmount;

            if (hasDealedAmount == EntrustAmount)
            {
                string msg = "撤单回报错误：当前委托已经清算完毕，撤单回报无效！" + entrustNumString;
                LogHelper.WriteDebug(msg);

                //将此问题id加入到已清算id列表中
                AddReckonedID(rde.Id);

                //清除数据库中保存的成交回报
                CrashManager.GetInstance().DeleteEntity(rde.Id);

                //表示清算还是成功了，这笔单在故障恢复时不需要重新发送
                return true;
            }

            if (rde.OrderVolume != -1)
            {
                if ((hasDealedAmount + rde.OrderVolume) > EntrustAmount)
                {
                    string msg = "撤单回报错误：撤单回报撤单数量+已清算的数量>当前委托量！" + entrustNumString;
                    LogHelper.WriteDebug(msg);

                    //将此问题id加入到已清算id列表中
                    AddReckonedID(rde.Id);

                    //清除数据库中保存的成交回报
                    CrashManager.GetInstance().DeleteEntity(rde.Id);

                    //表示清算还是成功了，这笔单在故障恢复时不需要重新发送
                    return true;
                }
            }

            #endregion

            #region 撤单过程

            //撤单（外部）InstantReckon_CancelOrder——因为1.1的结构修改，外部撤单与内部撤单逻辑基本一致(6除外)
            //1.资金冻结处理
            //把资金冻结记录里的金额和费用全部还给资金表，删除冻结记录(实际上只清零，不删除，盘后统一删）
            //与1.0的区别：在本次修改中，确保撤单回报是在最后进行处理，即如果撤单先回来，还有成交没有回来的话，那么不再像
            //1.0那样，谁先到就处理谁，这种处理逻辑会导致冻结资金处理复杂化。而现在的修改，在清算检查的时候，确保撤单回报
            //放到最后处理，如果后面还有成交回报没有回来，那么等待，直到所有的成交回报都收到并清算后才进行撤单清算，这样对
            //冻结资金的处理就简单化了，因为是最后一次操作，所以不再需要计算还要留多少冻结资金在冻结表里等后面的成交回报做
            //清算处理，而是简单的把冻结记录清零。
            //2.资金处理
            //把从资金冻结记录还回来的金额和费用加到可用资金，并减去总冻结资金
            //3.持仓冻结处理(开仓不处理）
            //把持仓冻结记录中的冻结量还给持仓表，删除冻结记录(实际上只清零，不删除，盘后统一删）
            //4.持仓处理（开仓不处理）
            //把从持仓冻结记录还回来的持仓量加到可用持仓，并减去总冻结持仓
            //5.委托处理
            //    更新委托状态，成交量，撤单量以及委托消息(OrderMessage)
            //6.生成一条成交记录（撤单类型）

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
                string msg = "[商品期货撤单清算]资金冻结记录不存在." + entrustNumString;
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

            #region 2.资金预处理，把从资金冻结记录还回来的金额和费用加到可用资金，并减去总冻结资金

            OpenCloseType = (Types.FutureOpenCloseType)tet.OpenCloseTypeId;

            decimal delta = preFreezeCapital + preFreezeCost;

            if (delta == 0)
                return true;

            var capMemory = MemoryDataManager.QHCapitalMemoryList.GetByCapitalAccountLogo(CapitalAccountId);
            if (capMemory == null)
            {
                strMessage = "GT-2601:[商品期货撤单清算]资金帐户不存在:" + CapitalAccount;

                return false;
            }

            QH_CapitalAccountTable_DeltaInfo capitalDelta = new QH_CapitalAccountTable_DeltaInfo();
            capitalDelta.CapitalAccountLogoId = capMemory.Data.CapitalAccountLogoId;
            capitalDelta.AvailableCapitalDelta += delta;
            capitalDelta.FreezeCapitalTotalDelta -= delta;

            #endregion

            decimal preFreezeHoldAmount = 0m;
            QH_HoldAccountTableInfo_Delta holdDelta = new QH_HoldAccountTableInfo_Delta();
            QHHoldMemoryTable holdMemory = null;
            if (OpenCloseType != Types.FutureOpenCloseType.OpenPosition)
            {
                #region 3.持仓冻结处理(开仓不计算）,获取持仓冻结记录中的冻结量和冻结id

                var hold = QHDataAccess.GetHoldAccountFreeze(EntrustNumber, Types.FreezeType.DelegateFreeze);
                if (hold == null)
                {
                    string msg = "[商品期货撤单清算]持仓冻结不存在";
                    LogHelper.WriteInfo(msg);
                    //持仓冻结不存在，一样运行撤单，当作持仓冻结量为0
                }
                else
                {
                    preFreezeHoldAmount = hold.FreezeAmount;
                    holdFreezeLogoId = hold.HoldFreezeLogoId;
                }

                #region 检查撤单量

                decimal cancelOrderVolume = 0;
                if (isErrorPriceCancelRpt)
                {
                    cancelOrderVolume = EntrustAmount;
                }
                else
                {
                    cancelOrderVolume = rde.OrderVolume;
                }

                if (cancelOrderVolume != preFreezeHoldAmount)
                {
                    string format = "商品期货撤单清算-回报冻结量与数据库冻结量不相等[EntrustNumber={0},CancelOrderVolume={1},FreezeHoldAmount={2}]";
                    string desc = string.Format(format, EntrustNumber, cancelOrderVolume, preFreezeHoldAmount);
                    preFreezeHoldAmount = cancelOrderVolume;
                    LogHelper.WriteDebug(desc);
                }

                #endregion

                #endregion

                //4.持仓预处理（开仓不计算），把从持仓冻结记录还回来的持仓量加到可用持仓，并减去总冻结持仓

                //if (preFreezeHoldAmount == 0)
                //    return true;

                try
                {
                    holdMemory = MemoryDataManager.QHHoldMemoryList.GetByAccountHoldLogoId(HoldingAccountId);
                    if (holdMemory == null)
                    {
                        //平仓时获取持仓账户需要取相反的买卖方向，因为委托买时，对应的持仓方向是卖
                        //获取持仓账户需要取相反的买卖方向，因为委托买时，对应的持仓方向是卖
                        var buySellType = BuySellType == GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying
                                              ? GTA.VTS.Common.CommonObject.Types.TransactionDirection.Selling
                                              : GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying;

                        holdMemory = QHCommonLogic.GetHoldMemoryTable(HoldingAccount, Code, CurrencyType, (int)buySellType);
                    }

                    if (holdMemory == null)
                    {
                        strMessage = "GT-2602:[商品期货撤单清算]持仓账户不存在:" + HoldingAccount;
                        LogHelper.WriteDebug(strMessage);
                        return false;
                    }

                    var holdData = holdMemory.Data;
                    holdDelta.Data = holdData;
                    holdDelta.AccountHoldLogoId = holdData.AccountHoldLogoId;

                    if (OpenCloseType ==
                        Types.FutureOpenCloseType.CloseTodayPosition)
                    {
                        //平今
                        holdDelta.TodayHoldAmountDelta += preFreezeHoldAmount;
                        holdDelta.TodayFreezeAmountDelta -= preFreezeHoldAmount;
                    }

                    if (OpenCloseType ==
                        Types.FutureOpenCloseType.ClosePosition)
                    {
                        //平历史
                        holdDelta.HistoryHoldAmountDelta += preFreezeHoldAmount;
                        holdDelta.HistoryFreezeAmountDelta -= preFreezeHoldAmount;
                    }
                }
                catch (Exception ex)
                {
                    strMessage = "GT-2603:[商品期货撤单清算]持仓处理异常:" + ex;
                    LogHelper.WriteDebug(strMessage);
                    LogHelper.WriteError(ex.Message, ex);
                }

            }

            tet = QHDataAccess.GetEntrustTable(EntrustNumber);
            //update 李健华 2009-12-29  这里如果是内部撤单把状态改变了到后面的
            // SetOrderState(tet);就会造成收市下单再撤单也即未报的前台主动撤单时变为废单状态，而应为已撤
            //QHCommonLogic.ProcessCancelOrderStatus(tet);
            //===========

            //tet.CancelAmount = tet.EntrustAmount - tet.TradeAmount;
            //tet.OrderMessage = strErrorMessage;

            tet.CancelAmount = (int)rde.OrderVolume;
            SetOrderState(tet);
            if (isErrorPriceCancelRpt)
            {
                tet.CancelAmount = (int)EntrustAmount;
                tet.OrderMessage = rde.Message;
            }

            //只要有错误信息，就写入委托
            if (!string.IsNullOrEmpty(rde.Message))
            {
                tet.OrderMessage = rde.Message;
            }

            //Clear资金和持仓冻结记录(没有删除)
            QH_TodayTradeTableInfo trade = null;

            #region 数据库提交动作

            bool isSuccess = false;
            var tet2 = tet;
            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                using (DbConnection connection = db.CreateConnection())
                {
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        ReckoningTransaction tm = new ReckoningTransaction();
                        tm.Database = db;
                        tm.Transaction = transaction;

                        QHDataAccess.ClearCapitalFreeze(capitalFreezeLogoId, db,
                                                                                         transaction);
                        QHDataAccess.ClearHoldFreeze(holdFreezeLogoId, db, transaction);

                        ReckoningTransaction rt = new ReckoningTransaction();
                        rt.Database = db;
                        rt.Transaction = transaction;

                        QHDataAccess.UpdateEntrustTable(tet2, rt);

                        bool isInternalCancelOrder = false;
                        if (rde is CancelOrderEntityEx)
                        {
                            var rde2 = (CancelOrderEntityEx)rde;
                            isInternalCancelOrder = rde2.IsInternalCancelOrder;
                        }

                        trade = QHCommonLogic.BuildQHCancelRpt(tet2, rde, rt, isInternalCancelOrder);


                        //清算完成后清除数据库中保存的成交回报
                        CrashManager.GetInstance().DeleteEntity(rde.Id, rt);

                        //资金处理
                        if (delta != 0)
                        {
                            capMemory.AddDeltaToDB(capitalDelta, db, transaction);
                        }

                        //持仓处理
                        if (OpenCloseType != Types.FutureOpenCloseType.OpenPosition)
                        {
                            if (preFreezeHoldAmount != 0)
                            {
                                holdMemory.AddDeltaToDB(holdDelta, db, transaction);
                            }
                        }


                        transaction.Commit();
                        isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        strMessage = "GT-2604:[商品期货撤单清算]数据库提交失败";

                        LogHelper.WriteError(ex.Message, ex);
                        isSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                strMessage = "GT-2604:[商品期货撤单清算]数据库提交失败";

            }

            if (isSuccess)
            {
                if (delta != 0)
                {
                    capMemory.AddDeltaToMemory(capitalDelta);
                }

                if (OpenCloseType != Types.FutureOpenCloseType.OpenPosition)
                {
                    if (preFreezeHoldAmount != 0)
                    {
                        holdMemory.AddDeltaToMemory(holdDelta);

                        holdMemory.ReadAndWrite(h =>
                        {
                            if (h.TodayFreezeAmount < 0)
                            {
                                h.TodayFreezeAmount = 0;
                            }

                            if (h.HistoryFreezeAmount < 0)
                            {
                                h.HistoryFreezeAmount = 0;
                            }

                            return h;
                        });
                    }
                }
            }
            else
            {

                return false;
            }

            #endregion


            //清算完成后将此id加入到已清算id列表中
            AddReckonedID(rde.Id);

            if (rde.OrderVolume == -1)
            {
                CancelAmount = EntrustAmount;
                strMessage = rde.Message;
            }
            else
                CancelAmount += rde.OrderVolume;

            trades = new List<QH_TodayTradeTableInfo>();
            if (trade != null)
                trades.Add(trade);

            return true;

            #endregion
        }

        #endregion

        #region 成交清算

        private void ProcessDealCommit()
        {
            var list = dealBackList.GetAllAndClear();
            if (Utils.IsNullOrEmpty(list))
                return;

            string format = "商品期货成交清算[EntrustNumber={0},DealBackCount={1}";
            string info = string.Format(format, EntrustNumber, list.Count);
            LogHelper.WriteInfo(info);


            string strMessage = "";
            QH_TodayEntrustTableInfo tet;
            List<QH_TodayTradeTableInfo> tradeList;

            bool isSuccess = false;

            List<string> ids = GetIdList(list);

            CrashManager.GetInstance().AddReckoningIDList(ids);

            isSuccess = InstantReckon_Deal(list, ref strMessage, out tet, out tradeList);

            if (!string.IsNullOrEmpty(strMessage))
                QHDataAccess.UpdateEntrustOrderMessage(EntrustNumber, strMessage);

            //失败，那么进行故障恢复流程
            if (!isSuccess)
            {
                //如果清算没有成功，那么从保存的回报列表中删掉，下次CrashManger可以再次发送此回报过来
                LogHelper.WriteInfo("#####################SPQHReckonUnit.ProcessDealCommit成交清算失败:" + strMessage);
                //"，StockDealBackEntity[ID=" + ido.Id +"]");
                RemoveIDList(ids);
            }
            CrashManager.GetInstance().RemoveReckoningIDList(ids);

            if (isSuccess)
            {
                ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo> reckonEndObject =
                    new ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo>();
                reckonEndObject.IsSuccess = true;
                reckonEndObject.EntrustTable = tet;
                reckonEndObject.TradeTableList = tradeList;
                reckonEndObject.TradeID = TradeID;
                reckonEndObject.Message = "";

                if (tet != null && tradeList != null)
                    OnEndReckoningEvent(reckonEndObject);
            }

            //检查是否已经全部清算完
            if ((TradeAmount + CancelAmount) == EntrustAmount)
            {
                hasDoneDeal = true;
            }

            if (hasDoneDeal)
            {
                bool isCheckSuccess = LastCheckFreezeMoney(EntrustNumber, CapitalAccountId);
                if (!isCheckSuccess)
                {
                    RescueManager.Instance.Record_GZQH_LastCheckFreezeMoney(EntrustNumber, CapitalAccountId);
                }

                LastCheckMargin(CapitalAccountId, HoldingAccountId);
            }
            else
            {
                if (isSuccess)
                {
                    CheckFreezeMoney();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idoList"></param>
        /// <param name="strMessage"></param>
        /// <param name="tet"></param>
        /// <param name="tradeList"></param>
        /// <returns></returns>
        public bool InstantReckon_Deal(List<CommoditiesDealBackEntity> idoList, ref string strMessage,
                                       out QH_TodayEntrustTableInfo tet, out List<QH_TodayTradeTableInfo> tradeList)
        {
            #region ==初始化变量 ==

            //bool result = false;
            strMessage = string.Empty;
            tet = null;
            tradeList = null;
            List<string> ids = GetIdList(idoList);

            string entrustNum = "[EntrustNumber=" + EntrustNumber + "]";

            string format = "即时清算SPQHReckonUnit.InstantReckon_Deal[资金帐户={0},持仓帐户={1},柜台委托单号={2}]";
            string msg = string.Format(format, CapitalAccount, HoldingAccount, EntrustNumber);
            LogHelper.WriteDebug(msg);

            var hasDealedAmount = TradeAmount + CancelAmount;
            if (hasDealedAmount == EntrustAmount)
            {
                msg = "即时清算SPQHReckonUnit.InstantReckon_Deal-成交回报错误：当前委托已经清算完毕，成交回报无效！EntrustNumber=" + EntrustNumber;
                LogHelper.WriteDebug(msg);

                //将此问题idList加入到已清算id列表中
                AddReckonedIDList(ids);

                //清除数据库中保存的成交回报
                foreach (var id in ids)
                {
                    CrashManager.GetInstance().DeleteEntity(id);
                }

                //表示清算还是成功了，这笔单在故障恢复时不需要重新发送
                return true;
            }

            List<CommoditiesDealBackEntityEx> idoexList;
            SPQHDealSum dealSum;
            bool isSuccess = ComputeSPQHDealBacks(idoList, out idoexList, out dealSum, ref strMessage);
            if (!isSuccess)
            {
                strMessage = "ComputeSPQHDealBacks失败！";
                LogHelper.WriteDebug(strMessage);
                return false;
            }

            if ((hasDealedAmount + dealSum.AmountSum) > EntrustAmount)
            {
                msg = "即时清算SPQHReckonUnit.InstantReckon_Deal-成交回报错误：成交回报成交数量{0}+已清算的数量{1}>当前委托量{2}！EntrustNumber=" +
                      EntrustNumber;

                LogHelper.WriteDebug(msg);
                //将此问题id加入到已清算id列表中
                AddReckonedIDList(ids);

                //清除数据库中保存的成交回报
                foreach (var id in ids)
                {
                    CrashManager.GetInstance().DeleteEntity(id);
                }

                //表示清算还是成功了，这笔单在故障恢复时不需要重新发送
                return true;
            }

            //取代码对应品种的交易币种
            if (CurrencyType == -1)
            {
                CurrencyType = MCService.FuturesTradeRules.GetCurrencyTypeByCommodityCode(Code).CurrencyTypeID;
            }

            //交割制度获获取(此方法没有用到数据库)
            if (CaptitalTradingRule == -1 || HoldingTradingRule == -1)
            {
                int captitalTradingRule;
                int holdingTradingRule;
                if (!MCService.FuturesTradeRules.GetDeliveryInstitution(Code, out captitalTradingRule,
                                                                        out holdingTradingRule, ref strMessage))
                {
                    QHDataAccess.UpdateEntrustOrderMessage(entrustNum, strMessage);
                    LogHelper.WriteDebug("UpdateEntrustOrderMessage失败！");
                    return false;
                }

                CaptitalTradingRule = captitalTradingRule;
                HoldingTradingRule = holdingTradingRule;
            }

            //现货当日委托回报
            tet = QHDataAccess.GetEntrustTable(EntrustNumber);
            if (tet == null)
            {
                strMessage = "GT-2605:[商品期货成交清算]无法获取委托对象，委托单号=" + EntrustNumber;
                LogHelper.WriteDebug(strMessage);
                return false;
            }

            OpenCloseType = (Types.FutureOpenCloseType)tet.OpenCloseTypeId;

            #endregion

            if (OpenCloseType == Types.FutureOpenCloseType.OpenPosition)
            {
                return SPQHOpen_InstantReckon_Deal(idoexList, dealSum, ref strMessage, out tet, out tradeList);
            }
            else
            {
                return SPQHClose_InstantReckon_Deal(idoexList, dealSum, ref strMessage, out tet, out tradeList);
            }
        }

        /// <summary>
        /// 最后一次清算时，检查持仓为空时是否还剩有保证金
        /// </summary>
        /// <param name="capitalAccountId"></param>
        /// <param name="holdingAccountId"></param>
        private void LastCheckMargin(int capitalAccountId, int holdingAccountId)
        {
            if (capitalAccountId == -1)
                return;

            if (holdingAccountId == -1)
                return;

            var holdMem = MemoryDataManager.QHHoldMemoryList.GetByAccountHoldLogoId(holdingAccountId);
            if (holdMem == null)
            {
                //平仓时获取持仓账户需要取相反的买卖方向，因为委托买时，对应的持仓方向是卖
                //获取持仓账户需要取相反的买卖方向，因为委托买时，对应的持仓方向是卖
                var buySellType = BuySellType == GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying
                                      ? GTA.VTS.Common.CommonObject.Types.TransactionDirection.Selling
                                      : GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying;

                holdMem = QHCommonLogic.GetHoldMemoryTable(HoldingAccount, Code, CurrencyType, (int)buySellType);
            }

            if (holdMem == null)
                return;

            var capMem = MemoryDataManager.QHCapitalMemoryList.GetByCapitalAccountLogo(capitalAccountId);
            if (capMem == null)
                return;

            var hold = holdMem.Data;
            var holdSum = hold.TodayHoldAmount + hold.TodayFreezeAmount + hold.HistoryHoldAmount +
                          hold.HistoryFreezeAmount;

            if (holdSum > 0)
                return;

            if (hold.Margin == 0)
                return;

            decimal margin = 0;

            holdMem.ReadAndWrite(h =>
            {
                margin = h.Margin;
                h.Margin = 0;
                return h;
            });

            QH_CapitalAccountTable_DeltaInfo capitalDelta = new QH_CapitalAccountTable_DeltaInfo();
            capitalDelta.CapitalAccountLogoId = capitalAccountId;
            capitalDelta.MarginTotalDelta = margin;

            string format = "SPQHReckonUnit.LastCheckMargin[CapitalAccountId={0},HoldingAccountId={1},Margin={2}]";
            string desc = string.Format(format, capitalAccountId, holdingAccountId, margin);
            LogHelper.WriteDebug(desc);

            capMem.AddDelta(capitalDelta);
        }

        /// <summary>
        /// 最后一次清算时，要把冻结记录清零
        /// </summary>
        public static bool LastCheckFreezeMoney(string entrustNumber, int capitalAccountId)
        {
            if (string.IsNullOrEmpty(entrustNumber))
                return true;

            if (capitalAccountId == -1)
                return true;

            //当最后一次清算时，要把冻结记录清零，多出的钱要还到资金表中，即“多了要退”并且总冻结资金要减去这部分钱;

            var caf = QHDataAccess.GetCapitalAccountFreeze(entrustNumber,
                                                           Types.FreezeType.DelegateFreeze);

            if (caf == null)
                return false;

            //需要还给资金表中的钱
            decimal needAddMoney = 0;
            if (caf.FreezeAmount != 0)
            {
                needAddMoney = caf.FreezeAmount;
            }

            if (caf.FreezeCost != 0)
            {
                needAddMoney += caf.FreezeCost;
            }

            if (caf.FreezeCost == 0 && caf.FreezeAmount == 0)
                return true;

            QH_CapitalAccountTable_DeltaInfo delta = new QH_CapitalAccountTable_DeltaInfo();
            delta.CapitalAccountLogoId = capitalAccountId;
            delta.AvailableCapitalDelta = needAddMoney;
            delta.FreezeCapitalTotalDelta = -needAddMoney;

            var capMemory = MemoryDataManager.QHCapitalMemoryList.GetByCapitalAccountLogo(capitalAccountId);
            if (capMemory == null)
            {
                return false;
            }

            bool isSuccess = false;
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
                        //1.清除资金冻结记录
                        QHDataAccess.ClearCapitalFreeze(caf.CapitalFreezeLogoId, database, transaction);

                        //2.修改资金表
                        capMemory.AddDeltaToDB(delta, database, transaction);

                        transaction.Commit();
                        isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogHelper.WriteError(ex.Message, ex);

                        isSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            if (isSuccess)
            {
                capMemory.AddDeltaToMemory(delta);
            }

            return isSuccess;
        }

        /// <summary>
        /// 检查当前委托的资金冻结情况，是否有为负的
        /// </summary>
        private void CheckFreezeMoney()
        {
            if (string.IsNullOrEmpty(EntrustNumber))
                return;

            if (CapitalAccountId == -1)
                return;

            var caf = QHDataAccess.GetCapitalAccountFreeze(EntrustNumber, Types.FreezeType.DelegateFreeze);

            //需要从资金表中扣去的钱
            decimal needRemoveMoney = 0;
            if (caf != null)
            {
                if (caf.FreezeAmount < 0)
                {
                    needRemoveMoney = -caf.FreezeAmount;
                    caf.FreezeAmount = 0;
                }

                if (caf.FreezeCost < 0)
                {
                    needRemoveMoney += (-caf.FreezeCost);
                    caf.FreezeCost = 0;
                }
            }

            QH_CapitalAccountTable_DeltaInfo delta = new QH_CapitalAccountTable_DeltaInfo();
            delta.CapitalAccountLogoId = CapitalAccountId;
            delta.AvailableCapitalDelta = -needRemoveMoney;
            delta.FreezeCapitalTotalDelta = needRemoveMoney;

            var capMemory = MemoryDataManager.QHCapitalMemoryList.GetByCapitalAccountLogo(CapitalAccountId);
            if (capMemory == null)
            {
                return;
            }

            bool isSuccess = capMemory.AddDelta(delta);
            if (!isSuccess)
            {
                isSuccess = capMemory.AddDelta(delta);
            }

            if (!isSuccess)
            {

            }
        }

        #endregion

        #region 开仓清算
        private bool SPQHOpen_InstantReckon_Deal(List<CommoditiesDealBackEntityEx> idoExList, SPQHDealSum dealSum,
                                                ref string strMessage, out QH_TodayEntrustTableInfo tet,
                                                out List<QH_TodayTradeTableInfo> tradeList)
        {
            //开仓
            //1.资金及冻结处理
            //(1) 根据成交金额和费用，从资金冻结记录减去相应的值，并从资金表总冻结资金中减去;
            //(2) 当冻结记录里钱不够扣时，直接从资金表可用资金中扣，即“少了要补”；
            //(3) 当最后一次清算时，要把冻结记录清零，多出的钱要还到资金表中，即“多了要退”，并且总冻结资金要减去这部分钱;
            //(4) 资金表的保证金总额要加上成交金额
            //2.持仓及冻结处理
            //    (1) 根据持仓交割规则：
            //    a.T+0: 将成交量直接加入持仓表今日持仓字段，更新成本价、保本价，开仓均价、持仓均价等等，不牵涉持仓冻结表。
            //    b.T+N: 生成一条持仓冻结记录，更新持仓表的成本价、保本价，开仓均价、持仓均价等等，更新持仓表的历史总冻结量。
            //    (2) 持仓表的保证金要加上成交金额
            //3.委托表处理
            //    更新各种状态（成交量，状态等）
            //4.生成一条成交记录

            tet = null;
            tradeList = new List<QH_TodayTradeTableInfo>();

            //总成交金额（包括capital和cost）
            decimal dealMoneySum = dealSum.CapitalSum + dealSum.CostSum;

            #region 1.资金表、资金冻结表预处理

            //先检查资金冻结表中的冻结金额和费用是否够扣
            QH_CapitalAccountFreezeTableInfo caf;
            if (!GetCapitalFreezeTable(out caf))
            {
                strMessage = "GT-2699:[商品期货成交清算]无法获取资金冻结记录.";

                return false;
            }

            var capMemory = MemoryDataManager.QHCapitalMemoryList.GetByCapitalAccountLogo(CapitalAccountId);
            if (capMemory == null)
            {
                strMessage = "GT-2606:[商品期货成交清算]资金帐户不存在:" + CapitalAccount;

                return false;
            }

            QH_CapitalAccountTable_DeltaInfo capitalDelta = SPQHOpen_InstantReckon_PreCapitalProcess(dealSum, caf, dealMoneySum);
            capitalDelta.CapitalAccountLogoId = capMemory.Data.CapitalAccountLogoId;
            //资金表不再首先提交，等待后面事务成功后再与持仓一起提交
            //capMemory.AddDelta(capitalDelta);

            #endregion

            #region 2.持仓表预处理

            bool isSuccess = false;
            QHHoldMemoryTable holdMemory = SPQHOpen_InstantReckon_PreHoldingProcess(ref isSuccess, ref strMessage);

            if (!isSuccess)
            {
                strMessage = "GT-2698:[商品期货成交清算]无法获取持仓记录.";

                return false;
            }

            if (holdMemory == null)
            {
                strMessage = "GT-2698:[商品期货成交清算]无法获取持仓记录.";

                return false;
            }

            #endregion

            #region 3.资金冻结和持仓冻结处理

            //处理资金冻结和持仓冻结,以及委托信息
            QH_HoldAccountFreezeTableInfo holdFreeze = null;
            bool isFirstHold = false;
            if (HoldingTradingRule != 0)
            {
                holdFreeze = SPQHOpen_InstantReckon_HoldingFreezeProcess(dealSum, ref isFirstHold);
            }

            tet = QHDataAccess.GetEntrustTable(EntrustNumber);
            var tet2 = tet;
            SPQHOpen_InstantReckon_EntrustProcess(dealSum, tet2);

            var tradeList2 = tradeList;

            #endregion

            #region 数据库提交操作

            isSuccess = false;
            List<string> ids = GetIDList2(idoExList);
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

                        //1.更新资金冻结
                        QH_CapitalAccountFreezeTableDal cafDal =
                            new QH_CapitalAccountFreezeTableDal();
                        cafDal.Update(caf, tm);

                        //2.更新持仓冻结(T+N时)——T+0时已经直接加入到持仓了，不需要做持仓冻结
                        if (HoldingTradingRule != 0)
                        {
                            QH_HoldAccountFreezeTableDal holdDal = new QH_HoldAccountFreezeTableDal();
                            if (isFirstHold)
                            {
                                holdDal.Add(holdFreeze, tm);
                            }
                            else
                            {
                                holdDal.Update(holdFreeze, tm);
                            }
                        }

                        //3.更新委托表
                        QHDataAccess.UpdateEntrustTable(tet2, tm);


                        //4.生成成交记录
                        BuildTradeList(tet2, tm, idoExList, tradeList2);

                        //5.资金表操作
                        capMemory.AddDeltaToDB(capitalDelta, tm.Database, tm.Transaction);

                        //5.清算完成后清除数据库中保存的成交回报
                        CrashManager.GetInstance().DeleteEntityList(ids, tm);

                        transaction.Commit();
                        isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogHelper.WriteError(ex.Message, ex);
                        strMessage = "GT-2607:[商品期货成交清算]清算提交失败！委托单号" + EntrustNumber;

                        isSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            if (isSuccess)
            {
                //当提交资金到数据库成功后，同步到内存
                capMemory.AddDeltaToMemory(capitalDelta);
            }
            else
            {
                LogHelper.WriteDebug("清算提交失败!");
                return false;
            }


            #endregion

            #region 4.持仓表真正提交处理

            //之所以放到最后执行，是因为开仓时持仓表的操作太多，难以回滚，所以放到最后，保证必须执行）
            isSuccess = SPQHOpen_InstantReckon_HoldingProcess(holdMemory, dealSum, HoldingTradingRule);

            if (!isSuccess)
            {
                for (int i = 0; i < 3; i++)
                {
                    //不成功，再执行一次
                    isSuccess = SPQHOpen_InstantReckon_HoldingProcess(holdMemory, dealSum, HoldingTradingRule);

                    if (isSuccess)
                        break;
                }
            }

            if (!isSuccess)
            {
                //一定要执行，记录下来等下次执行
                RescueManager.Instance.Record_SPQHOpen_InstantReckon_HoldingProcess(HoldingAccountId, dealSum, HoldingTradingRule);
            }

            #endregion

            //清算完成后将此id加入到已清算id列表中
            AddReckonedIDList(ids);
            strMessage = string.Empty;


            if (tet.EntrustAmount < (TradeAmount + dealSum.AmountSum) || tet.EntrustAmount < TradeAmount)
            {
                LogHelper.WriteInfo("SPQHReckonUnit.InstantReckon_Deal2清算成交量错误");
            }
            else
            {
                TradeAmount += dealSum.AmountSum;
            }

            return true;
        }

        /// <summary>
        /// 开仓清算-委托表处理
        /// </summary>
        /// <param name="dealSum"></param>
        /// <param name="tet2"></param>
        private void SPQHOpen_InstantReckon_EntrustProcess(SPQHDealSum dealSum, QH_TodayEntrustTableInfo tet2)
        {
            //成交均价
            tet2.TradeAveragePrice =
                (tet2.TradeAveragePrice * tet2.TradeAmount +
                 dealSum.CapitalSumNoScale) /
                (tet2.TradeAmount + dealSum.AmountSum);

            //成交量
            tet2.TradeAmount =
                Convert.ToInt32(tet2.TradeAmount + dealSum.AmountSum);

            SetOrderState(tet2);

            tet2.OrderMessage = "";
        }

        /// <summary>
        /// 资金表预处理(包括对资金冻结表的预处理)
        /// </summary>
        /// <param name="dealSum"></param>
        /// <param name="caf"></param>
        /// <param name="dealMoneySum"></param>
        /// <returns></returns>
        private QH_CapitalAccountTable_DeltaInfo SPQHOpen_InstantReckon_PreCapitalProcess(SPQHDealSum dealSum, QH_CapitalAccountFreezeTableInfo caf, decimal dealMoneySum)
        {
            //注意：为了简化处理，在清算期间不再进行冻结资金的补偿计算及多退少补的计算，统一在本次清算结束后
            //对资金冻结表进行检查校验，所以在清算内部，直接在资金冻结表上减去成交金额和成交费用
            //资金冻结表操作
            caf.FreezeAmount -= dealSum.CapitalSum;
            caf.FreezeCost -= dealSum.CostSum;

            QH_CapitalAccountTable_DeltaInfo capitalDelta = new QH_CapitalAccountTable_DeltaInfo();
            //资金表操作1：资金表的总冻结金额减去本次成交的金额和费用和（因为资金冻结里减去了这么多钱）
            capitalDelta.FreezeCapitalTotalDelta = -dealMoneySum;
            //资金表操作2：保证金总额要加上成交金额
            capitalDelta.MarginTotalDelta = dealSum.CapitalSum;
            capitalDelta.CapitalAccountLogoId = CapitalAccountId;
            return capitalDelta;
        }

        /// <summary>
        /// 持仓表预处理-开仓
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="strMessage"></param>
        /// <returns></returns>
        private QHHoldMemoryTable SPQHOpen_InstantReckon_PreHoldingProcess(ref bool isSuccess, ref string strMessage)
        {
            QHHoldMemoryTable holdMemory = null;
            try
            {
                holdMemory = MemoryDataManager.QHHoldMemoryList.GetByHoldAccountAndCurrencyType(HoldingAccount, Code,
                                                                                                (int)BuySellType,
                                                                                                CurrencyType);
                //如果持仓为空，那么先从数据库加载，看是不是内存表没有加载
                if (holdMemory == null)
                {
                    QH_HoldAccountTableInfo hold = GetHoldFromDB(HoldingAccount, Code, (int)BuySellType, CurrencyType);
                    //如果数据库有，那么直接加载到内存表中
                    if (hold != null)
                    {
                        string format = "SPQHOpen_InstantReckon_PreHoldingProcess数据库存在持仓，直接加载到内存表中[Code={0},BuySell={1},HoldAccount={2}]";
                        string desc = string.Format(format, Code, BuySellType, HoldingAccount);
                        LogHelper.WriteDebug(desc);

                        isSuccess = MemoryDataManager.QHHoldMemoryList.AddQHHoldAccountTableToMemeory(hold);

                        if (!isSuccess)
                        {
                            isSuccess = MemoryDataManager.QHHoldMemoryList.AddQHHoldAccountTableToMemeory(hold);
                        }

                        if (!isSuccess)
                        {
                            desc += "-加载失败，已存在";
                            LogHelper.WriteDebug(desc);
                        }
                    }
                    else
                    {
                        //如果数据库也没有，那么代表第一次买入，新建一个空的持仓，并加入到数据库和内存表中
                        string format = "SPQHOpen_InstantReckon_PreHoldingProcess数据库不存在持仓，新建一个空的持仓，并加入到数据库和内存表中[Code={0},BuySell={1},HoldAccount={2}]";
                        string desc = string.Format(format, Code, BuySellType, HoldingAccount);
                        LogHelper.WriteDebug(desc);

                        hold = new QH_HoldAccountTableInfo();
                        hold.UserAccountDistributeLogo = HoldingAccount;
                        hold.Contract = Code;
                        hold.HistoryHoldAmount = 0;
                        hold.HistoryFreezeAmount = 0;
                        hold.TodayHoldAmount = 0;
                        hold.TodayFreezeAmount = 0;
                        hold.TradeCurrencyType = CurrencyType;
                        hold.BuySellTypeId = (int)BuySellType;

                        isSuccess = MemoryDataManager.QHHoldMemoryList.AddQHHoldAccountTable(hold);

                        if (!isSuccess)
                        {
                            isSuccess = MemoryDataManager.QHHoldMemoryList.AddQHHoldAccountTable(hold);
                        }

                        if (!isSuccess)
                        {
                            desc += "-新建失败，已存在";
                            LogHelper.WriteDebug(desc);
                        }
                    }


                    holdMemory = MemoryDataManager.QHHoldMemoryList.GetByHoldAccountAndCurrencyType(HoldingAccount, Code,
                                                                                                    (int)BuySellType,
                                                                                                    CurrencyType);

                    if (holdMemory == null)
                    {
                        int holdId = MemoryDataManager.QHHoldMemoryList.GetAccountHoldLogoId(HoldingAccount, Code,
                                                                                             (int)BuySellType, CurrencyType);

                        if (holdId != -1)
                            holdMemory = MemoryDataManager.QHHoldMemoryList.GetByAccountHoldLogoId(holdId);
                    }

                    if (holdMemory == null)
                    {
                        LogHelper.WriteDebug("SPQHOpen_InstantReckon_PreHoldingProcess无法获取持仓记录");
                        isSuccess = false;
                        return null;
                    }
                }

                HoldingAccountId = holdMemory.Data.AccountHoldLogoId;

                isSuccess = true;
            }
            catch (Exception ex)
            {
                strMessage = "GT-2608:[商品期货成交清算]持仓处理异常:" + ex;
                LogHelper.WriteError(ex.Message, ex);
            }

            return holdMemory;
        }

        private QH_HoldAccountTableInfo GetHoldFromDB(string holdingAccount, string code, int buySellType, int currencyType)
        {
            QH_HoldAccountTableInfo hold = new QH_HoldAccountTableInfo();
            QH_HoldAccountTableDal dal = new QH_HoldAccountTableDal();

            hold = dal.GetQhAccountHoldTable(holdingAccount, code, currencyType, buySellType);

            return hold;
        }

        /// <summary>
        /// 开仓清算-持仓冻结处理
        /// </summary>
        /// <param name="dealSum"></param>
        /// <param name="isFirstHold"></param>
        /// <returns></returns>
        private QH_HoldAccountFreezeTableInfo SPQHOpen_InstantReckon_HoldingFreezeProcess(SPQHDealSum dealSum, ref bool isFirstHold)
        {
            QH_HoldAccountFreezeTableInfo holdFreeze;
            holdFreeze = QHDataAccess.GetHoldAccountFreeze(EntrustNumber,
                                                           Types.FreezeType.DelegateFreeze);
            if (holdFreeze == null)
            {
                holdFreeze = new QH_HoldAccountFreezeTableInfo();
                //冻结总量
                holdFreeze.FreezeAmount = Convert.ToInt32(dealSum.AmountSum);
                //冻结时间
                holdFreeze.FreezeTime = DateTime.Now;
                //解冻时间
                holdFreeze.ThawTime = DateTime.Now.AddDays(HoldingTradingRule);
                //委托单号
                holdFreeze.EntrustNumber = EntrustNumber;
                //冻结类型
                holdFreeze.FreezeTypeLogo = (int)Types.FreezeType.ReckoningFreeze;
                //现货帐户持仓标识
                holdFreeze.AccountHoldLogo = HoldingAccountId;
                isFirstHold = true;
            }
            else
            {
                //之前存在冻结
                //冻结总量
                holdFreeze.FreezeAmount += Convert.ToInt32(dealSum.AmountSum);
            }
            return holdFreeze;
        }

        /// <summary>
        /// 商品期货开仓-持仓操作
        /// </summary>
        /// <param name="holdMemory"></param>
        /// <param name="dealSum"></param>
        /// <param name="holdingTradingRule"></param>
        private static bool SPQHOpen_InstantReckon_HoldingProcess(QHHoldMemoryTable holdMemory, SPQHDealSum dealSum, int holdingTradingRule)
        {
            bool isSuccess = holdMemory.ReadAndWrite(hold =>
            {
                LogHelper.WriteDebug("--->商品期货开仓-持仓操作：code=" + hold.Contract + ",UserAccountDistributeLogo=" + hold.UserAccountDistributeLogo);
                decimal holdAmount = hold.HistoryHoldAmount +
                                     hold.HistoryFreezeAmount +
                                     hold.TodayHoldAmount +
                                     hold.TodayFreezeAmount;

                LogHelper.WriteDebug("计算总持仓量 = hold.HistoryHoldAmount + hold.HistoryFreezeAmount + hold.TodayHoldAmount + hold.TodayFreezeAmount = " + hold.HistoryHoldAmount + " + " + hold.HistoryFreezeAmount + " + " + hold.TodayHoldAmount + " + " + hold.TodayFreezeAmount + " = " + holdAmount);

                decimal todayAmount = hold.TodayHoldAmount +
                                      hold.TodayFreezeAmount;

                LogHelper.WriteDebug("计算当日持仓量 = hold.TodayHoldAmount + hold.TodayFreezeAmount =  " + hold.TodayHoldAmount + " + " + hold.TodayFreezeAmount + " = " + todayAmount);

                //买= ((总持仓量*成本价+ (当前成交价*当前成交量 + 当前费用))/(总持仓量+当前成交量)) 
                //卖= ((总持仓量*成本价+ (当前成交价*当前成交量 - 当前费用))/(总持仓量+当前成交量)) 
                decimal costPrice = 0;
                if (hold.BuySellTypeId == (int)GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying)
                {
                    costPrice = (holdAmount * hold.CostPrice +
                                     dealSum.CapitalSumNoScale +
                                     dealSum.CostSum) /
                                    (holdAmount + dealSum.AmountSum);
                    LogHelper.WriteDebug("计算成本价(买) =(holdAmount * hold.CostPrice + dealSum.CapitalSumNoScale + dealSum.CostSum) / (holdAmount + dealSum.AmountSum) = (" + holdAmount + " * " + hold.CostPrice + " + " + dealSum.CapitalSumNoScale + " + " + dealSum.CostSum + ") / (" + holdAmount + " + " + dealSum.AmountSum + ")=" + costPrice);
                }
                else
                {
                    costPrice = (holdAmount * hold.CostPrice +
                                     dealSum.CapitalSumNoScale -
                                     dealSum.CostSum) /
                                    (holdAmount + dealSum.AmountSum);
                    LogHelper.WriteDebug("计算成本价(卖) =(holdAmount * hold.CostPrice + dealSum.CapitalSumNoScale - dealSum.CostSum) / (holdAmount + dealSum.AmountSum) = (" + holdAmount + " * " + hold.CostPrice + " + " + dealSum.CapitalSumNoScale + " - " + dealSum.CostSum + ") / (" + holdAmount + " + " + dealSum.AmountSum + ")=" + costPrice);
                }
                //四舍五入
                costPrice = Utils.Round(costPrice);


                string code = hold.Contract;
                decimal holdPrice = MCService.GetHoldPrice(code, costPrice, (holdAmount + dealSum.AmountSum), hold.BuySellTypeId);
                //四舍五入
                holdPrice = Utils.Round(holdPrice);
                hold.CostPrice = costPrice;
                hold.BreakevenPrice = holdPrice;

                //今日开仓均价  == (今开仓量 * 开仓均价+当前成交价*当前成交量)/(今开仓量+当前成交量)
                hold.TodayHoldAveragePrice = (hold.TodayHoldAveragePrice * todayAmount + dealSum.CapitalSumNoScale) / (todayAmount + dealSum.AmountSum);
                hold.TodayHoldAveragePrice =
                    Utils.Round(hold.TodayHoldAveragePrice);

                //持仓均价=（持仓均价*总持仓量+买入成交量*买入价）/(总持仓量+买入成交量)
                hold.HoldAveragePrice = (hold.HoldAveragePrice * holdAmount +
                                         dealSum.CapitalSumNoScale) /
                                        (holdAmount + dealSum.AmountSum);
                hold.HoldAveragePrice = Utils.Round(hold.HoldAveragePrice);

                LogHelper.WriteDebug("计算持仓均价 = (hold.HoldAveragePrice * holdAmount + dealSum.CapitalSumNoScale) / (holdAmount + dealSum.AmountSum) = (" + hold.HoldAveragePrice + " * " + holdAmount + " + " + dealSum.CapitalSumNoScale + ") / (" + holdAmount + " + " + dealSum.AmountSum + ") = " + hold.HoldAveragePrice);

                //期货持仓表中当前持仓均价在盘后清算后，会被修改为当日结算价，现在需要新增一个开仓均价，
                //与当前持仓均价的计算公式一样，唯一的区别是盘后不进行修改，保持不变。
                //开仓均价
                hold.OpenAveragePrice = (hold.OpenAveragePrice * holdAmount +
                                         dealSum.CapitalSumNoScale) /
                                        (holdAmount + dealSum.AmountSum);
                hold.OpenAveragePrice = Utils.Round(hold.OpenAveragePrice);

                LogHelper.WriteDebug("计算开仓均价 = (hold.OpenAveragePrice * holdAmount + dealSum.CapitalSumNoScale) / (holdAmount + dealSum.AmountSum) = (" + hold.OpenAveragePrice + " * " + holdAmount + " + " + dealSum.CapitalSumNoScale + ") / (" + holdAmount + " + " + dealSum.AmountSum + ") = " + hold.OpenAveragePrice);

                //持仓表操作1
                //T+0
                if (holdingTradingRule == 0)
                {
                    //a.T+0: 将成交量直接加入持仓表今日持仓字段，更新成本价、保本价，开仓均价、持仓均价等等，不牵涉持仓冻结表。
                    hold.TodayHoldAmount += dealSum.AmountSum;
                }
                else
                {
                    //b.T+N: 生成一条持仓冻结记录，更新持仓表的成本价、保本价，开仓均价、持仓均价等等，更新持仓表的历史总冻结量。
                    hold.HistoryFreezeAmount += dealSum.AmountSum;
                }

                //持仓表操作2：持仓表的保证金要加上成交金额
                hold.Margin += dealSum.CapitalSum;
                return hold;
            });
            return isSuccess;
        }

        /// <summary>
        /// 供RescueManager恢复期货开仓持仓操作时调用
        /// </summary>
        /// <param name="holdingAccoutnId"></param>
        /// <param name="dealSum"></param>
        /// <returns></returns>
        public static bool DoSPQHOpen_HoldingRescue(int holdingAccoutnId, SPQHDealSum dealSum, int holdingTradingRule)
        {
            QHHoldMemoryTable holdMemory = MemoryDataManager.QHHoldMemoryList.GetByAccountHoldLogoId(holdingAccoutnId);
            if (holdMemory == null)
            {
                holdMemory = QHCommonLogic.GetHoldMemoryTable(holdingAccoutnId);
            }

            if (holdMemory == null)
            {
                return true;
            }

            return SPQHOpen_InstantReckon_HoldingProcess(holdMemory, dealSum, holdingTradingRule);
        }

        #endregion

        #region 平仓清算

        private bool SPQHClose_InstantReckon_Deal(List<CommoditiesDealBackEntityEx> idoExList, SPQHDealSum dealSum,
                                                       ref string strMessage, out QH_TodayEntrustTableInfo tet,
                                                       out List<QH_TodayTradeTableInfo> tradeList)
        {
            //平今/平历史
            //1.资金及冻结处理【资金表操作1】
            //(1) 根据费用，从资金冻结记录减去相应的值，并从资金表总冻结资金中减去;
            //(2) 当冻结记录里钱不够扣时，直接从资金表可用资金中扣，即“少了要补”；
            //(3) 当最后一次清算时，要把冻结记录清零，多出的钱要还到资金表中，即“多了要退”并且总冻结资金要减去这部分钱;
            //2.持仓及冻结处理【持仓表操作1】
            //    将成交量从持仓表的今日总冻结量(平今)/历史总冻结量(平历史)上减去，并且还要从持仓冻结中减去。
            //3.平仓盈亏及保证金处理
            //	资金表总保证金减去计算出的保证金【资金表操作2】
            //	持仓表的保证金减去计算出的保证金【持仓表操作2】
            //	根据资金交割规则：【资金表操作3】
            //a.T+0: 平仓盈亏和保证金直接加入资金表可用资金；
            //        b.T+N:生成一条平仓盈亏和保证金之和的资金冻结记录，并且在资金表总冻结金额中加上。
            //4.委托及盯市盈亏，浮动盈亏处理
            //    (1)更新各种状态（成交量，状态，盯市盈亏，浮动盈亏等）；
            //    (2)汇总到资金表中的总盯市盈亏和总浮动盈亏中。【资金表操作4】
            //5.生成一条成交记录

            tet = null;
            tradeList = new List<QH_TodayTradeTableInfo>();

            #region 1.资金表操作1

            //先检查资金冻结表中的冻结金额和费用是否够扣

            QH_CapitalAccountFreezeTableInfo capitalFreeze;
            if (!GetCapitalFreezeTable(out capitalFreeze))
            {
                strMessage = "GT-2699:[商品期货成交清算]无法获取资金冻结记录.";
                return false;
            }

            var capMemory = MemoryDataManager.QHCapitalMemoryList.GetByCapitalAccountLogo(CapitalAccountId);
            if (capMemory == null)
            {
                strMessage = "GT-2609:[商品期货成交清算]资金帐户不存在:" + CapitalAccount;
                LogHelper.WriteDebug(strMessage);
                return false;
            }

            //资金表操作1
            QH_CapitalAccountTable_DeltaInfo capitalDelta = SPQHClose_InstantReckon_PreCapitalProcess(capitalFreeze, dealSum);
            capitalDelta.CapitalAccountLogoId = capMemory.Data.CapitalAccountLogoId;

            #endregion

            #region 2.持仓表预处理

            bool isSuccess = false;
            QHHoldMemoryTable holdMemory = null;
            holdMemory = SPQHClose_InstantReckon_PreHoldingProcess(ref isSuccess, ref strMessage);

            if (!isSuccess)
            {
                strMessage = "GT-2698:[商品期货成交清算]无法获取持仓记录.";

                return false;
            }

            if (holdMemory == null)
            {
                strMessage = "GT-2698:[商品期货成交清算]无法获取持仓记录.";

                return false;
            }

            QH_HoldAccountTableInfo_Delta holdDelta = new QH_HoldAccountTableInfo_Delta();
            var holdData = holdMemory.Data;
            holdDelta.Data = holdData;
            holdDelta.AccountHoldLogoId = holdData.AccountHoldLogoId;
            #endregion

            #region 3.计算平仓盈亏和保证金

            decimal profitLossSum = 0;
            decimal marginSum = 0;
            var holdAccountTable = holdMemory.Data;

            isSuccess = GetAllSPQH_ProfitLossAndMargin(idoExList, holdAccountTable, out profitLossSum, out marginSum, ref strMessage);
            if (!isSuccess)
            {
                strMessage = "GT-2697:[商品期货成交清算]平仓盈亏与保证金计算失败：" + strMessage;

                return false;
            }

            //资金表操作2
            capitalDelta.MarginTotalDelta = -marginSum;


            SPQHClose_InstantReckon_HoldingProcess(holdDelta, dealSum, marginSum);

            #endregion

            #region 4.计算浮动盈亏和盯市盈亏
            //总浮动盈亏
            decimal floatProfitLossSum = 0;
            //总盯市盈亏
            decimal marketProfitLossSum = 0;
            isSuccess = GetAllSPQH_FloatAndMarketProfitLoss(idoExList, holdAccountTable, out floatProfitLossSum,
                                                            out marketProfitLossSum, ref strMessage);

            if (!isSuccess)
            {
                strMessage = "GT-2696:[商品期货成交清算]浮动盈亏和盯市盈亏计算失败：" + strMessage;

                return false;
            }

            //4.资金表操作4
            capitalDelta.CloseFloatProfitLossTotalDelta += floatProfitLossSum;
            capitalDelta.CloseMarketProfitLossTotalDelta += marketProfitLossSum;
            #endregion

            #region 5.持仓冻结处理

            //处理资金冻结和持仓冻结,以及委托信息
            QH_HoldAccountFreezeTableInfo holdFreeze = null;

            holdFreeze = QHDataAccess.GetHoldAccountFreeze(EntrustNumber,
                                                           Types.FreezeType.DelegateFreeze);
            if (holdFreeze == null)
            {
                strMessage = "GT-2695:[商品期货成交清算]无法获取持仓冻结记录.";
                LogHelper.WriteInfo(strMessage);
                //持仓冻结不存在，一样运行撤单，即不再处理持仓冻结表，只对持仓表的冻结做处理
                //return false;
            }
            else
            {
                holdFreeze.FreezeAmount -= Convert.ToInt32(dealSum.AmountSum);
                if (holdFreeze.FreezeAmount < 0)
                    holdFreeze.FreezeAmount = 0;
            }

            #endregion

            #region 委托表状态修改
            tet = QHDataAccess.GetEntrustTable(EntrustNumber);
            var tet2 = tet;
            SPQHClose_InstantReckon_EntrustProcess(dealSum, tet2, floatProfitLossSum, marketProfitLossSum);

            #endregion

            #region 数据库提交处理
            var tradeList2 = tradeList;
            isSuccess = false;

            List<string> ids = GetIDList2(idoExList);

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

                        //1.更新资金冻结
                        QH_CapitalAccountFreezeTableDal cafDal =
                            new QH_CapitalAccountFreezeTableDal();
                        cafDal.Update(capitalFreeze, tm);

                        //资金表操作3
                        SPQHClose_InstantReckon_CapitalAndFreezeProcess(tet2, profitLossSum, marginSum, tm, capitalDelta);

                        //2.更新持仓冻结
                        if (holdFreeze != null)
                        {
                            QH_HoldAccountFreezeTableDal holdDal = new QH_HoldAccountFreezeTableDal();
                            holdDal.Update(holdFreeze, tm);
                        }

                        //3.更新委托表
                        QHDataAccess.UpdateEntrustTable(tet2, tm);


                        //4.资金表操作
                        capMemory.AddDeltaToDB(capitalDelta, tm.Database, tm.Transaction);

                        //5.持仓表操作
                        holdMemory.AddDeltaToDB(holdDelta, tm.Database, tm.Transaction);

                        //6.生成成交记录
                        BuildTradeList(tet2, tm, idoExList, tradeList2);

                        //7.清算完成后清除数据库中保存的成交回报
                        CrashManager.GetInstance().DeleteEntityList(ids, tm);

                        transaction.Commit();
                        isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogHelper.WriteError(ex.Message, ex);
                        strMessage = "GT-2607:[商品期货成交清算]清算提交失败！委托单号" + EntrustNumber;
                        isSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            if (isSuccess)
            {
                //当提交资金、持仓到数据库成功后，同步到内存
                capMemory.AddDeltaToMemory(capitalDelta);

                holdMemory.AddDeltaToMemory(holdDelta);
                holdMemory.ReadAndWrite(h =>
                {
                    if (h.TodayFreezeAmount < 0)
                    {
                        h.TodayFreezeAmount = 0;
                    }

                    if (h.HistoryFreezeAmount < 0)
                    {
                        h.HistoryFreezeAmount = 0;
                    }

                    return h;
                });
            }
            else
            {
                LogHelper.WriteDebug("清算提交失败!");

                return false;
            }

            #endregion

            //清算完成后将此id加入到已清算id列表中
            AddReckonedIDList(ids);
            strMessage = string.Empty;


            if (tet.EntrustAmount < (TradeAmount + dealSum.AmountSum) || tet.EntrustAmount < TradeAmount)
            {
                LogHelper.WriteInfo("XHOrderLogicFlow.InstantReckon_Deal2清算成交量错误");
            }
            else
            {
                TradeAmount += dealSum.AmountSum;
            }

            return true;
        }


        /// <summary>
        /// 平仓-持仓处理
        /// </summary>
        /// <param name="holdDelta"></param>
        /// <param name="dealSum"></param>
        /// <param name="marginSum"></param>
        private void SPQHClose_InstantReckon_HoldingProcess(QH_HoldAccountTableInfo_Delta holdDelta, SPQHDealSum dealSum, decimal marginSum)
        {
            //持仓表操作1.冻结量减去成交量
            //平今
            if (OpenCloseType == Types.FutureOpenCloseType.CloseTodayPosition)
            {
                holdDelta.TodayFreezeAmountDelta -= dealSum.AmountSum;
            }

            //平历史
            if (OpenCloseType == Types.FutureOpenCloseType.ClosePosition)
            {
                holdDelta.HistoryFreezeAmountDelta -= dealSum.AmountSum;
            }

            //持仓表操作2.保证金减去计算出来的 保证金
            holdDelta.MarginDelta -= marginSum;
        }

        /// <summary>
        /// 平仓清算-资金及冻结操作
        /// </summary>
        /// <param name="tet2"></param>
        /// <param name="profitLossSum"></param>
        /// <param name="marginSum"></param>
        /// <param name="tm"></param>
        /// <param name="capitalDelta"></param>
        private void SPQHClose_InstantReckon_CapitalAndFreezeProcess(QH_TodayEntrustTableInfo tet2, decimal profitLossSum, decimal marginSum, ReckoningTransaction tm, QH_CapitalAccountTable_DeltaInfo capitalDelta)
        {
            //资金冻结操作3
            if (CaptitalTradingRule != 0)
            {
                //T+N时生成一条平仓盈亏和保证金之和的资金冻结记录，并且在资金表总冻结金额中加上
                QH_CapitalAccountFreezeTableDal cafDal =
                    new QH_CapitalAccountFreezeTableDal();

                //当前成交金额冻结
                var newCaft = new QH_CapitalAccountFreezeTableInfo();
                //委托单号
                newCaft.EntrustNumber = tet2.EntrustNumber;
                //冻结 预成交金额
                newCaft.FreezeAmount = profitLossSum + marginSum;
                //冻结 预成交费用
                newCaft.FreezeCost = 0;
                //冻结时间
                newCaft.FreezeTime = DateTime.Now;
                //解冻时间
                newCaft.ThawTime = DateTime.Now.AddDays(CaptitalTradingRule);
                //冻结类型
                newCaft.FreezeTypeLogo =
                    (int)Entity.Contants.Types.FreezeType.ReckoningFreeze;

                newCaft.CapitalAccountLogo = CapitalAccountId;

                cafDal.Add(newCaft, tm);

                capitalDelta.FreezeCapitalTotalDelta += profitLossSum + marginSum;
            }
            else
            {
                //资金表操作3
                //T+0时平仓盈亏和保证金直接加入资金表可用资金
                capitalDelta.AvailableCapitalDelta += profitLossSum + marginSum;
            }
        }

        /// <summary>
        /// 平仓清算-委托表处理
        /// </summary>
        /// <param name="dealSum"></param>
        /// <param name="tet2"></param>
        /// <param name="floatProfitLossSum"></param>
        /// <param name="marketProfitLossSum"></param>
        private void SPQHClose_InstantReckon_EntrustProcess(SPQHDealSum dealSum, QH_TodayEntrustTableInfo tet2, decimal floatProfitLossSum, decimal marketProfitLossSum)
        {
            //成交均价
            tet2.TradeAveragePrice =
                (tet2.TradeAveragePrice * tet2.TradeAmount +
                 dealSum.CapitalSumNoScale) /
                (tet2.TradeAmount + dealSum.AmountSum);

            //成交量
            tet2.TradeAmount =
                Convert.ToInt32(tet2.TradeAmount + dealSum.AmountSum);

            tet2.CloseFloatProfitLoss += floatProfitLossSum;
            tet2.CloseMarketProfitLoss += marketProfitLossSum;

            SetOrderState(tet2);

            tet2.OrderMessage = "";
        }

        /// <summary>
        /// 平仓清算-资金表及资金冻结表预处理
        /// </summary>
        /// <param name="capitalFreeze"></param>
        /// <param name="dealSum"></param>
        /// <returns></returns>
        private QH_CapitalAccountTable_DeltaInfo SPQHClose_InstantReckon_PreCapitalProcess(QH_CapitalAccountFreezeTableInfo capitalFreeze, SPQHDealSum dealSum)
        {
            //注意：为了简化处理，在清算期间不再进行冻结资金的补偿计算及多退少补的计算，统一在本次清算结束后
            //对资金冻结表进行检查校验，所以在清算内部，直接在资金冻结表上减去成交金额和成交费用
            //caf.FreezeCapitalAmount -= 0; 卖不会冻结资金
            capitalFreeze.FreezeCost -= dealSum.CostSum;


            //T+0:资金表的总冻结金额减去本次成交的费用和（因为资金冻结里减去了这么多钱）
            //需要更新的资金变化对象
            QH_CapitalAccountTable_DeltaInfo capitalDelta = new QH_CapitalAccountTable_DeltaInfo();
            capitalDelta.CapitalAccountLogoId = CapitalAccountId;
            //资金表操作1
            capitalDelta.FreezeCapitalTotalDelta = -dealSum.CostSum;

            //if (CaptitalTradingRule == 0)
            //{
            //    //资金表不马上执行更新，在后面统一更新
            //    //capitalDelta.AvailableCapitalDelta = dealSum.CapitalSum;
            //    capitalDelta.FreezeCapitalTotalDelta = -dealSum.CostSum;
            //}
            //else
            //{
            //    //T+N:生成资金冻结记录，资金表的总冻结金额要【-成交费用】，注意：不加成交金额，因为期货平仓
            //    //只冻结了费用，成交金额由平仓盈亏计算，作为平仓盈亏加入可用资金，不在这里处理
            //    //capMemory.AddDelta(0, dealSum.CapitalSum - dealSum.CostSum, 0, 0, 0, 0);
            //    capitalDelta.FreezeCapitalTotalDelta = - dealSum.CostSum;
            //}
            return capitalDelta;
        }

        /// <summary>
        /// 持仓表预处理-平仓
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="strMessage"></param>
        /// <returns></returns>
        private QHHoldMemoryTable SPQHClose_InstantReckon_PreHoldingProcess(ref bool isSuccess, ref string strMessage)
        {
            QHHoldMemoryTable holdMemory = null;
            isSuccess = false;
            var buySellType = BuySellType == GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying
                                      ? (int)GTA.VTS.Common.CommonObject.Types.TransactionDirection.Selling
                                      : (int)GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying;

            try
            {
                holdMemory = MemoryDataManager.QHHoldMemoryList.GetByHoldAccountAndCurrencyType(HoldingAccount, Code,
                                                                                                buySellType,
                                                                                                CurrencyType);
                if (holdMemory != null)
                {
                    isSuccess = true;
                    HoldingAccountId = holdMemory.Data.AccountHoldLogoId;

                    return holdMemory;
                }

                //如果持仓为空，那么看能否从数据库加载
                QH_HoldAccountTableInfo hold = GetHoldFromDB(HoldingAccount, Code, buySellType, CurrencyType);
                if (hold == null)
                {
                    return null;
                }

                isSuccess = MemoryDataManager.QHHoldMemoryList.AddQHHoldAccountTableToMemeory(hold);
                if (!isSuccess)
                {
                    return null;
                }

                holdMemory =
                    MemoryDataManager.QHHoldMemoryList.GetByHoldAccountAndCurrencyType(HoldingAccount, Code,
                                                                                       buySellType,
                                                                                       CurrencyType);

                if (holdMemory == null)
                {
                    isSuccess = false;
                    return null;
                }

                HoldingAccountId = holdMemory.Data.AccountHoldLogoId;

            }
            catch (Exception ex)
            {
                strMessage = "GT-2608:[商品期货成交清算]持仓处理异常:" + ex;
                LogHelper.WriteError(ex.Message, ex);
            }

            return holdMemory;
        }

        #endregion

        #region  获取浮动和盯市盈亏
        private bool GetAllSPQH_FloatAndMarketProfitLoss(List<CommoditiesDealBackEntityEx> idoexList, QH_HoldAccountTableInfo holdAccountTable, out decimal floatProfitLossSum, out decimal marketProfitLossSum, ref string strMessage)
        {
            bool isSuccess = false;
            floatProfitLossSum = 0;
            marketProfitLossSum = 0;

            foreach (var entityEx in idoexList)
            {
                var ido = entityEx.DealBack;
                decimal floatProfitLoss = 0;
                decimal marketProfitLoss = 0;

                if (OpenCloseType == Types.FutureOpenCloseType.CloseTodayPosition)
                {
                    isSuccess = GetSPQHCloseToday_FloatMarketProfitLoss(holdAccountTable, ido, ref strMessage, out floatProfitLoss, out marketProfitLoss);

                }

                //平历史
                if (OpenCloseType == Types.FutureOpenCloseType.ClosePosition)
                {
                    isSuccess = GetSPQHCloseHistory_FloatMarketProfitLoss(holdAccountTable, ido, ref strMessage, out floatProfitLoss, out marketProfitLoss);
                }

                if (!isSuccess)
                {
                    return false;
                }
                //2009-12-03 add 李健华 增加每笔盯市盈亏和浮动盈亏
                entityEx.FloatProfitLoss = floatProfitLoss;
                entityEx.MarketProfitLoss = marketProfitLoss;
                //==========

                floatProfitLossSum += floatProfitLoss;
                marketProfitLossSum += marketProfitLoss;
            }

            //四舍五入
            floatProfitLossSum = Utils.Round(floatProfitLossSum);
            marketProfitLossSum = Utils.Round(marketProfitLossSum);

            return isSuccess;
        }

        private bool GetSPQHCloseToday_FloatMarketProfitLoss(QH_HoldAccountTableInfo holdAccountTable, CommoditiesDealBackEntity ido, ref string strMessage, out decimal floatProfitLoss, out decimal marketProfitLoss)
        {
            floatProfitLoss = 0;
            marketProfitLoss = 0;

            //期货合约乘数300
            decimal scale = MCService.GetFutureTradeUntiScale(Code);

            if (holdAccountTable.BuySellTypeId == (int)GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying)
            {
                //	平仓盈亏（浮）：
                //	平今日买入持仓时，
                //	平仓盈亏=（平仓卖出价-今日开仓买入均价）*平仓卖出数量*合约乘数
                floatProfitLoss = (ido.DealPrice - holdAccountTable.TodayHoldAveragePrice) * ido.DealAmount * scale;
                string format1 = "浮动盈亏-平今日买入持仓[EntrustNumber={0},（平仓卖出价{1}-今日开仓买入均价{2}）*平仓卖出数量{3}*合约乘数{4}]";
                string desc1 = string.Format(format1, EntrustNumber, ido.DealPrice, holdAccountTable.TodayHoldAveragePrice,
                                            ido.DealAmount, scale);
                LogHelper.WriteInfo(desc1);

                //	平仓盈亏（盯）：（等同于《修改柜台需求2009.6.3.doc》的平仓盈亏）
                //	平今日买入持仓时，
                //	平仓盈亏=（平仓卖出价-今日开仓买入均价）*平仓卖出数量*合约乘数
                marketProfitLoss = (ido.DealPrice - holdAccountTable.TodayHoldAveragePrice) * ido.DealAmount * scale;
                string format2 = "盯市盈亏-平今日买入持仓[EntrustNumber={0},（平仓卖出价{1}-今日开仓买入均价{2}）*平仓卖出数量{3}*合约乘数{4}]";
                string desc2 = string.Format(format2, EntrustNumber, ido.DealPrice, holdAccountTable.TodayHoldAveragePrice,
                                            ido.DealAmount, scale);
                LogHelper.WriteInfo(desc2);
            }
            else
            {
                //	平仓盈亏（浮）：
                //	平今日卖出持仓时，
                //	平仓盈亏=（今日开仓卖出均价-平仓买入价）*平仓买入数量*合约乘数
                floatProfitLoss = (holdAccountTable.TodayHoldAveragePrice - ido.DealPrice) * ido.DealAmount * scale;
                string format3 = "浮动盈亏-平今日卖出持仓[EntrustNumber={0},（今日开仓卖出均价{1}-平仓买入价{2}）*平仓买入数量{3}*合约乘数{4}]";
                string desc3 = string.Format(format3, EntrustNumber, holdAccountTable.TodayHoldAveragePrice, ido.DealPrice,
                                            ido.DealAmount, scale);
                LogHelper.WriteInfo(desc3);

                //	平仓盈亏（盯）：（等同于《修改柜台需求2009.6.3.doc》的平仓盈亏）
                //	平今日卖出持仓时，
                //	平仓盈亏=（今日开仓卖出均价-平仓买入价）*平仓买入数量*合约乘数
                marketProfitLoss = (holdAccountTable.TodayHoldAveragePrice - ido.DealPrice) * ido.DealAmount * scale;
                string format4 = "盯市盈亏-平今日卖出持仓[EntrustNumber={0},（今日开仓卖出均价{1}-平仓买入价{2}）*平仓买入数量{3}*合约乘数{4}]";
                string desc4 = string.Format(format4, EntrustNumber, holdAccountTable.TodayHoldAveragePrice, ido.DealPrice,
                                            ido.DealAmount, scale);
                LogHelper.WriteInfo(desc4);

            }

            floatProfitLoss = Utils.Round(floatProfitLoss);
            marketProfitLoss = Utils.Round(marketProfitLoss);

            return true;
        }

        private bool GetSPQHCloseHistory_FloatMarketProfitLoss(QH_HoldAccountTableInfo holdAccountTable, CommoditiesDealBackEntity ido, ref string strMessage, out decimal floatProfitLoss, out decimal marketProfitLoss)
        {
            floatProfitLoss = 0;
            marketProfitLoss = 0;

            //期货合约乘数300
            decimal scale = MCService.GetFutureTradeUntiScale(Code);

            decimal holdSum = holdAccountTable.TodayHoldAmount + holdAccountTable.TodayFreezeAmount +
                              holdAccountTable.HistoryFreezeAmount + holdAccountTable.HistoryHoldAmount;
            decimal todaySum = holdAccountTable.TodayHoldAmount + holdAccountTable.TodayFreezeAmount;

            //如果今日开仓均价为0，那么代表今日没有开仓，也不应该今日冻结
            //但是因为盘后清算可能不成功，昨天的“今日冻结”没有转移到历史冻结中
            //那么会造成historyPrice计算有误，所以当今日开仓均价为0时，
            //也设置todaySum=0
            if (holdAccountTable.TodayHoldAveragePrice == 0)
            {
                todaySum = 0;
            }

            #region 获取昨日结算价
            //IRealtimeMarketService service = RealtimeMarketServiceFactory.GetService();
            //VTFutData futData = service.GetFutData(code);

            ////参考价(昨日结算价)
            decimal refPrice = 0;

            bool canGetPrice = MCService.GetFutureYesterdayPreSettlementPrice(Code, out refPrice, ref strMessage);

            bool canGetHoldPrice = false;
            decimal holdPrice = holdAccountTable.HoldAveragePrice;
            if (holdPrice <= 0)
            {
                string format = "合约{0}的持仓均价非法！";
                string msg = string.Format(format, holdAccountTable.Contract);
                LogHelper.WriteInfo(msg);
            }
            else
            {
                canGetHoldPrice = true;
            }

            //如果不能获取昨日结算价，那么取持仓均价
            if (!canGetPrice)
            {
                string format = "无法获取合约{0}的昨日结算价！";
                string msg = string.Format(format, holdAccountTable.Contract);
                LogHelper.WriteInfo(msg);

                if (!canGetHoldPrice)
                {
                    string format1 = "无法获取合约{0}的持仓均价！";
                    string msg1 = string.Format(format1, holdAccountTable.Contract);
                    LogHelper.WriteInfo(msg1);

                    return false;
                }

                refPrice = holdPrice;
            }

            if (refPrice <= 0)
            {
                string format1 = "无法获取合约{0}的昨日结算价！以持仓均价替代{1}";
                string msg1 = string.Format(format1, holdAccountTable.Contract, holdPrice);
                LogHelper.WriteInfo(msg1);

                refPrice = holdPrice;
            }

            #endregion

            decimal yesterdayPrice = refPrice;
            if (yesterdayPrice <= 0)
            {
                string format1 = "无法获取合约{0}的昨日结算价！平仓盈亏将会计算错误！YesterdayPrice={1}";
                string msg1 = string.Format(format1, holdAccountTable.Contract, yesterdayPrice);
                LogHelper.WriteInfo(msg1);
            }

            if (holdAccountTable.BuySellTypeId == (int)GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying)
            {
                //	平仓盈亏（浮）
                //	平历史买入持仓时，
                //	平仓盈亏=（平仓卖出价-历史买入持仓的开仓均价）*平仓卖出数量*合约乘数
                //	历史买入持仓的开仓均价=（开仓均价*总买入持仓量-今日开仓买入均价*今日开仓量）/（总买入持仓量-今日开仓买入量）
                decimal historyBuyPrice = (holdAccountTable.OpenAveragePrice * holdSum -
                                           holdAccountTable.TodayHoldAveragePrice * todaySum) / (holdSum - todaySum);
                string format01 = "历史买入持仓的开仓均价=（开仓均价OpenAveragePrice{0}*总买入持仓量{1}-今日开仓买入均价TodayHoldAveragePrice{2}*今日开仓量{3}）/（总买入持仓量{1}-今日开仓量{3}）";
                string desc0 = string.Format(format01, holdAccountTable.OpenAveragePrice, holdSum, holdAccountTable.TodayHoldAveragePrice, todaySum);
                LogHelper.WriteInfo(desc0);

                floatProfitLoss = (ido.DealPrice - historyBuyPrice) * ido.DealAmount * scale;
                string format1 = "浮动盈亏-平历史买入持仓[EntrustNumber={0},（平仓卖出价{1}-历史买入持仓的开仓均价{2}）*平仓卖出数量{3}*合约乘数{4}]";
                string desc1 = string.Format(format1, EntrustNumber, ido.DealPrice, historyBuyPrice,
                                            ido.DealAmount, scale);
                LogHelper.WriteInfo(desc1);

                //	平仓盈亏（盯）
                //	平历史买入持仓时，
                //	平仓盈亏=（平仓卖出价-昨日结算价）*平仓卖出数量*合约乘数
                marketProfitLoss = (ido.DealPrice - yesterdayPrice) * ido.DealAmount * scale;
                string format2 = "盯市盈亏-平历史买入持仓[EntrustNumber={0},（平仓卖出价{1}-昨日结算价{2}）*平仓卖出数量{3}*合约乘数{4}]";
                string desc2 = string.Format(format2, EntrustNumber, ido.DealPrice, yesterdayPrice,
                                            ido.DealAmount, scale);
                LogHelper.WriteInfo(desc2);

            }
            else
            {
                //	平仓盈亏（浮）
                //	平历史卖出持仓时，
                //	平仓盈亏=（历史卖出持仓的开仓均价-平仓买入价）*平仓买入数量*合约乘数
                //	历史卖出持仓的开仓均价=（开仓均价*总卖出持仓量-今日开仓卖出均价*今日开仓量）/（总卖出持仓量-今日开仓卖出量）
                decimal historySellPrice = (holdAccountTable.OpenAveragePrice * holdSum -
                                          holdAccountTable.TodayHoldAveragePrice * todaySum) / (holdSum - todaySum);
                string format02 = "历史卖出持仓的开仓均价=（开仓均价{0}*总卖出持仓量{1}-今日开仓卖出均价{2}*今日开仓量{3}）/（总卖出持仓量{1}-今日开仓量{3}）";
                string desc02 = string.Format(format02, holdAccountTable.OpenAveragePrice, holdSum, holdAccountTable.TodayHoldAveragePrice, todaySum);
                LogHelper.WriteInfo(desc02);

                floatProfitLoss = (historySellPrice - ido.DealPrice) * ido.DealAmount * scale;
                string format3 = "浮动盈亏-平历史卖出持仓[EntrustNumber={0},（历史卖出持仓的开仓均价{1}-平仓买入价{2}）*平仓买入数量{3}*合约乘数{4}]";
                string desc3 = string.Format(format3, EntrustNumber, historySellPrice, ido.DealPrice,
                                            ido.DealAmount, scale);
                LogHelper.WriteInfo(desc3);

                //	平仓盈亏（盯）
                //	平历史卖出持仓时，
                //	平仓盈亏=（昨日结算价-平仓买入价）*平仓买入数量*合约乘数
                marketProfitLoss = (yesterdayPrice - ido.DealPrice) * ido.DealAmount * scale;
                string format4 = "盯市盈亏-平历史卖出持仓[EntrustNumber={0},（昨日结算价{1}-平仓买入价{2}）*平仓买入数量{3}*合约乘数{4}]";
                string desc4 = string.Format(format4, EntrustNumber, yesterdayPrice, ido.DealPrice,
                                            ido.DealAmount, scale);
                LogHelper.WriteInfo(desc4);
            }

            //四舍五入
            floatProfitLoss = Utils.Round(floatProfitLoss);
            marketProfitLoss = Utils.Round(marketProfitLoss);

            return true;
        }

        #endregion

        #region 获取平仓盈亏和保证金

        private bool GetAllSPQH_ProfitLossAndMargin(List<CommoditiesDealBackEntityEx> idoexList, QH_HoldAccountTableInfo holdAccountTable, out decimal profitLossSum, out decimal marginSum, ref string strMessage)
        {
            profitLossSum = 0;
            marginSum = 0;

            bool isSuccess = false;
            //QHHoldMemoryTable holdMemory = GZQHOpen_InstantReckon_PreHoldingProcess(ref isSuccess, ref strMessage);

            //if (!isSuccess)
            //{
            //    strMessage = "无法获取对应的开仓记录";
            //    return false;
            //}
            //var holdAccountTable = holdMemory.Data;

            foreach (var entityEx in idoexList)
            {
                var ido = entityEx.DealBack;
                decimal profitLoss = 0;
                decimal margin = 0;

                if (OpenCloseType == Types.FutureOpenCloseType.CloseTodayPosition)
                {
                    isSuccess = GetSPQHCloseToday_CapitalProfitAndLoss(holdAccountTable, ido, ref strMessage, out profitLoss,
                                                        out margin);

                }

                //平历史
                if (OpenCloseType == Types.FutureOpenCloseType.ClosePosition)
                {
                    isSuccess = GetSPQHCloseHistory_CapitalProfitAndLoss(holdAccountTable, ido, ref strMessage, out profitLoss,
                                                       out margin);
                }

                if (!isSuccess)
                {
                    return false;
                }
                //2009-12-03 add 李健华 增加每笔保证金
                entityEx.DealCapital = margin;
                //==========

                profitLossSum += profitLoss;
                marginSum += margin;
            }

            //四舍五入
            profitLossSum = Utils.Round(profitLossSum);
            marginSum = Utils.Round(marginSum);

            return true;
        }

        /// <summary>
        /// 获取平今时的平仓盈亏和保证金
        /// </summary>
        /// <param name="holdAccountTable"></param>
        /// <param name="ido"></param>
        /// <param name="strMessage"></param>
        /// <param name="profitLoss"></param>
        /// <param name="margin"></param>
        /// <returns></returns>
        private bool GetSPQHCloseToday_CapitalProfitAndLoss(QH_HoldAccountTableInfo holdAccountTable, CommoditiesDealBackEntity ido, ref string strMessage, out decimal profitLoss, out decimal margin)
        {
            profitLoss = 0;
            margin = 0;
            #region 获取平仓盈亏

            //bool isSuccess = false;
            //QHHoldMemoryTable holdMemory = GZQHOpen_InstantReckon_PreHoldingProcess(ref isSuccess, ref strMessage);

            //if (!isSuccess)
            //{
            //    strMessage = "无法获取对应的开仓记录";
            //    return false;
            //}
            //var holdAccountTable = holdMemory.Data;

            //获取今日开仓均价
            decimal todayHoldAveragePrice = holdAccountTable.TodayHoldAveragePrice;

            //参考价（卖-平多：今日开仓买入价 买-平空：今日开仓卖出价）
            decimal refPrice = todayHoldAveragePrice;
            //本次委托成交价格
            decimal dealPrice = ido.DealPrice;
            //本次委托成交数量
            decimal amount = ido.DealAmount;


            //期货合约乘数300
            decimal scale = MCService.GetFutureTradeUntiScale(Code);

            if (BuySellType == GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying)
            {
                //平空：平仓盈亏=（今日开仓卖出价-平仓买入价）*平仓量*乘数
                profitLoss = (refPrice - dealPrice) * amount * scale;

                string format1 = "期货盘中平今资金清算1[ID={0},账户={1},合约={2},平仓盈亏{3}=（今日开仓卖出价{4}-平仓买入价{5}）*平仓量{6}*乘数{7},买卖方向={8}]";
                string desc1 = string.Format(format1, holdAccountTable.AccountHoldLogoId,
                                             holdAccountTable.UserAccountDistributeLogo, Code, profitLoss,
                                             refPrice, dealPrice, amount, scale, BuySellType);
                LogHelper.WriteDebug(desc1);
            }
            else
            {
                //平多：平仓盈亏=（平仓卖出价-今日开仓买入出价）*平仓量*乘数
                profitLoss = (dealPrice - refPrice) * amount * scale;

                string format2 =
                    "期货盘中平今资金清算1[ID={0},账户={1},合约={2},平仓盈亏{3}=（平仓卖出价{4}-今日开仓买入出价{5}）*平仓量{6}*乘数{7},买卖方向={8}]";
                string desc2 = string.Format(format2, holdAccountTable.AccountHoldLogoId,
                                             holdAccountTable.UserAccountDistributeLogo, Code, profitLoss,
                                             dealPrice, refPrice, amount, scale, BuySellType);
                LogHelper.WriteDebug(desc2);
            }

            //四舍五入
            profitLoss = Utils.Round(profitLoss);

            #endregion

            #region 计算保证金

            //保证金比例
            decimal futureBail = MCService.GetFutureBailScale(Code) / 100;
            margin = amount * refPrice * scale * futureBail;
            margin = Utils.Round(margin);

            #endregion

            return true;
        }

        /// <summary>
        /// 获取平历史时的平仓盈亏和保证金
        /// </summary>
        /// <param name="ido"></param>
        /// <param name="strMessage"></param>
        /// <param name="profitLoss"></param>
        /// <param name="margin"></param>
        /// <returns></returns>
        private bool GetSPQHCloseHistory_CapitalProfitAndLoss(QH_HoldAccountTableInfo holdAccountTable, CommoditiesDealBackEntity ido, ref string strMessage, out decimal profitLoss, out decimal margin)
        {
            profitLoss = 0;
            margin = 0;


            //bool isSuccess = false;
            //QHHoldMemoryTable holdMemory = GZQHOpen_InstantReckon_PreHoldingProcess(ref isSuccess, ref strMessage);

            //if (!isSuccess)
            //{
            //    strMessage = "无法获取对应的开仓记录";
            //    return false;
            //}
            //var holdAccountTable = holdMemory.Data;

            //获取今日开仓均价
            decimal todayHoldAveragePrice = holdAccountTable.TodayHoldAveragePrice;

            #region 获取昨日结算价

            ////参考价(昨日结算价)
            decimal refPrice = 0;

            bool canGetPrice = MCService.GetFutureYesterdayPreSettlementPrice(Code, out refPrice, ref strMessage);

            bool canGetHoldPrice = false;
            decimal holdPrice = holdAccountTable.HoldAveragePrice;
            if (holdPrice <= 0)
            {
                string format = "合约{0}的持仓均价非法！";
                string msg = string.Format(format, holdAccountTable.Contract);
                LogHelper.WriteInfo(msg);
            }
            else
            {
                canGetHoldPrice = true;
            }

            //如果不能获取昨日结算价，那么取持仓均价
            if (!canGetPrice)
            {
                string format = "无法获取合约{0}的昨日结算价！";
                string msg = string.Format(format, holdAccountTable.Contract);
                LogHelper.WriteInfo(msg);

                if (!canGetHoldPrice)
                {
                    string format1 = "无法获取合约{0}的持仓均价！";
                    string msg1 = string.Format(format1, holdAccountTable.Contract);
                    LogHelper.WriteInfo(msg1);

                    return false;
                }

                refPrice = holdPrice;
            }

            if (refPrice <= 0)
            {
                string format1 = "无法获取合约{0}的昨日结算价！以持仓均价替代{1}";
                string msg1 = string.Format(format1, holdAccountTable.Contract, holdPrice);
                LogHelper.WriteInfo(msg1);

                refPrice = holdPrice;
            }

            #endregion

            //本次委托成交价格
            decimal dealPrice = ido.DealPrice;
            //本次委托成交数量
            decimal amount = ido.DealAmount;

            //期货合约乘数300
            decimal scale = MCService.GetFutureTradeUntiScale(Code);

            if (refPrice <= 0)
            {
                string format1 = "无法获取合约{0}的昨日结算价！平仓盈亏将会计算错误！YesterdayPrice={1}";
                string msg1 = string.Format(format1, holdAccountTable.Contract, refPrice);
                LogHelper.WriteInfo(msg1);
            }

            if (BuySellType == GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying)
            {
                //平空：平仓盈亏=（昨日结算价-平仓买入价）*平仓量*乘数
                profitLoss = (refPrice - dealPrice) * amount * scale;

                string format1 = "期货盘中平历史资金清算1[ID={0},账户={1},合约={2},平仓盈亏{3}=（昨日结算价{4}-平仓买入价{5}）*平仓量{6}*乘数{7}]";
                string desc1 = string.Format(format1, holdAccountTable.AccountHoldLogoId,
                                             holdAccountTable.UserAccountDistributeLogo, Code, profitLoss,
                                             refPrice, dealPrice, amount, scale);
                LogHelper.WriteDebug(desc1);
            }
            else
            {
                //平多：平仓盈亏=（平仓卖出价-昨日结算价）*平仓量*乘数
                profitLoss = (dealPrice - refPrice) * amount * scale;

                string format2 = "期货盘中平历史资金清算1[ID={0},账户={1},合约={2},平仓盈亏{3}=（平仓卖出价{4}-昨日结算价{5}）*平仓量{6}*乘数{7}]";
                string desc2 = string.Format(format2, holdAccountTable.AccountHoldLogoId,
                                             holdAccountTable.UserAccountDistributeLogo, Code, profitLoss,
                                             dealPrice, refPrice, amount, scale);
                LogHelper.WriteDebug(desc2);
            }

            //四舍五入
            profitLoss = Utils.Round(profitLoss);

            #region 计算保证金

            //保证金比例
            decimal futureBail = MCService.GetFutureBailScale(Code) / 100;
            margin = amount * refPrice * scale * futureBail;
            //四舍五入
            margin = Utils.Round(margin);

            #endregion


            return true;
        }

        #endregion

        #region 功能方法

        /// <summary>
        /// 生成成交记录
        /// </summary>
        /// <param name="tet2"></param>
        /// <param name="tm"></param>
        /// <param name="idoExList"></param>
        /// <param name="tradeList2"></param>
        private void BuildTradeList(QH_TodayEntrustTableInfo tet2, ReckoningTransaction tm, List<CommoditiesDealBackEntityEx> idoExList, List<QH_TodayTradeTableInfo> tradeList2)
        {
            CommoditiesDealBackEntity ido = null;
            QHCostResult costResult = null;
            FutureDealBackEntity gzDeal = null;
            foreach (var idoEx in idoExList)
            {
                ido = idoEx.DealBack;
                costResult = idoEx.CostResult;

                // 为了能使用之前的方法把成交回报实体转换成商品期货成交实体
                gzDeal = QHCommonLogic.SPQHDealEntryConversionGZQHDealEntry(ido);

                //转换成交类型
                Types.DealRptType dealRptType = Types.DealRptType.DRTDealed;
                if (IsCheckForcedCloseOrder)
                {
                    switch (QHForcedCloseType)
                    {
                        case GTA.VTS.Common.CommonObject.Types.QHForcedCloseType.Expired:
                            dealRptType = Types.DealRptType.DRTTradeDated;
                            break;
                        case GTA.VTS.Common.CommonObject.Types.QHForcedCloseType.CapitalCheck:
                            dealRptType = Types.DealRptType.DRTMargin;
                            break;
                        case GTA.VTS.Common.CommonObject.Types.QHForcedCloseType.OverHoldLimit:
                            dealRptType = Types.DealRptType.DRTViolateLimit;
                            break;
                        case GTA.VTS.Common.CommonObject.Types.QHForcedCloseType.NotModMinUnit:
                            dealRptType = Types.DealRptType.DRTViolateLimit;
                            break;
                    }

                }

                var trade = QHCommonLogic.BuildGZQHDealRpt(tet2, gzDeal, costResult, idoEx.DealCapital, idoEx.MarketProfitLoss, tm, dealRptType);
                if (trade != null)
                {
                    tradeList2.Add(trade);
                }
            }
        }


        //设置委托状态
        private void SetOrderState(QH_TodayEntrustTableInfo tet)
        {
            if (tet.OrderStatusId == (int)Types.OrderStateType.DOSCanceled)
                return;

            //错误价格的废单
            if (tet.CancelAmount == -1)
            {
                tet.OrderStatusId = (int)Types.OrderStateType.DOSCanceled;
                return;
            }

            //委托量==成交量+撤单量 全部成交
            if (tet.EntrustAmount == (tet.TradeAmount + tet.CancelAmount))
            {
                hasDoneDeal = true;
                //如果撤单成功的量大于0
                if (tet.CancelAmount > 0)
                {
                    //如果撤单成功的量等于委托的量，那么代表全撤,否则是部撤
                    if (tet.EntrustAmount == tet.CancelAmount)
                    {
                        tet.OrderStatusId = (int)Types.OrderStateType.DOSRemoved;
                    }
                    else
                    {
                        tet.OrderStatusId = (int)Types.OrderStateType.DOSPartRemoved;
                    }
                }
                //否则是没有发生撤单，已成   
                else
                {
                    tet.OrderStatusId = (int)Types.OrderStateType.DOSDealed;
                }
            }
            //委托量>成交量+撤单量 部分成交
            else
            {
                //如果还在成交的过程中，收到撤单的命令，那么就保持部成待撤状态，直到最后变成已成或已撤
                if (tet.OrderStatusId == (int)Types.OrderStateType.DOSPartDealRemoveSoon)
                    return;

                //否则不管有没有撤单，均统一设置为部成（因为撮合可能先返回撤单，所以此处状态不好设置）
                if (tet.TradeAmount > 0)
                    tet.OrderStatusId = (int)Types.OrderStateType.DOSPartDealed;
            }
        }

        private bool GetCapitalFreezeTable(out QH_CapitalAccountFreezeTableInfo caf)
        {
            caf = QHDataAccess.GetCapitalAccountFreeze(EntrustNumber,
                                                       Types.FreezeType.DelegateFreeze);
            if (caf == null)
            {
                //那么认为冻结的金额和费用都已经为0，插入一条空的冻结记录以供后面使用
                try
                {
                    string txt = string.Format("期货插入冻结空记录CapitalAccountId={0},EntrustNumber={1}", CapitalAccountId, EntrustNumber);
                    LogHelper.WriteDebug(txt);
                    QHCommonLogic.InsertNullCapitalFreeze(CapitalAccountId, EntrustNumber);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                    return false;
                }

                caf = QHDataAccess.GetCapitalAccountFreeze(EntrustNumber,
                                                           Types.FreezeType.DelegateFreeze);
            }
            return true;
        }



        /// <summary>
        /// 获取成交回报列表所有的ID
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<string> GetIdList(List<CommoditiesDealBackEntity> list)
        {
            var ids = new List<string>();
            foreach (var ido in list)
            {
                ids.Add(ido.Id);
            }
            return ids;
        }

        private List<string> GetIDList2(List<CommoditiesDealBackEntityEx> list)
        {
            List<string> ids = new List<string>();
            foreach (var entityEx in list)
            {
                ids.Add(entityEx.DealBack.Id);
            }

            return ids;
        }

        /// <summary>
        /// 计算商品期货成交回报汇总信息
        /// </summary>
        /// <returns>商品期货成交回报汇总对象</returns>
        private bool ComputeSPQHDealBacks(List<CommoditiesDealBackEntity> idos, out List<CommoditiesDealBackEntityEx> idoexs, out SPQHDealSum dealSum, ref string strMessage)
        {
            idoexs = new List<CommoditiesDealBackEntityEx>();
            dealSum = new SPQHDealSum();
            foreach (var ido in idos)
            {
                CommoditiesDealBackEntityEx ex = new CommoditiesDealBackEntityEx(ido);
                QHCostResult costResult = null;
                decimal dealCapital = 0;
                decimal dealCost = 0;
                bool isSuccess = ComputePreprocCapital(ido, Code, OpenCloseType, ref strMessage, ref dealCapital, ref dealCost, out costResult);

                if (!isSuccess)
                    return false;

                decimal dealCapitalNoScale = ido.DealAmount * ido.DealPrice;
                //四舍五入
                dealCapitalNoScale = Utils.Round(dealCapitalNoScale);

                ex.CostResult = costResult;
                ex.DealCapital = dealCapital;
                ex.DealCost = dealCost;
                ex.DealCapitalNoScale = dealCapitalNoScale;

                dealSum.AmountSum += ido.DealAmount;
                dealSum.CapitalSum += dealCapital;
                dealSum.CostSum += dealCost;
                dealSum.CapitalSumNoScale += dealCapitalNoScale;

                idoexs.Add(ex);
            }

            return true;
        }

        /// <summary>
        /// 计算成交记录费用
        /// </summary>
        /// <param name="ido"></param>
        /// <param name="code"></param>
        /// <param name="openCloseType"></param>
        /// <param name="strMessage"></param>
        /// <param name="dealCapital"></param>
        /// <param name="dealCost"></param>
        /// <param name="qhcr"></param>
        /// <returns></returns>
        private bool ComputePreprocCapital(CommoditiesDealBackEntity ido, string code, Types.FutureOpenCloseType openCloseType, ref string strMessage, ref decimal dealCapital,
                                           ref decimal dealCost, out QHCostResult qhcr)
        {
            bool result = false;
            qhcr = null;
            try
            {
                int iut = MCService.GetPriceUnit(code);
                //计价单位与交易单位倍数-期货是合约乘数300
                decimal unitMultiple = MCService.GetTradeUnitScale(code, (GTA.VTS.Common.CommonObject.Types.UnitType)Enum.Parse(typeof(GTA.VTS.Common.CommonObject.Types.UnitType), iut.ToString()));
                //保证金比例
                decimal futureBail = MCService.GetFutureBailScale(code) / 100;

                int dealamount = Convert.ToInt32(ido.DealAmount);
                //成交金额 委托量 * 委托价 * 交易单位到计价单位倍数
                dealCapital = dealamount * Convert.ToDecimal(ido.DealPrice) * unitMultiple;

                //保证金 成交金额 * 保证金比例
                dealCapital = dealCapital * futureBail;
                //四舍五入
                dealCapital = Utils.Round(dealCapital);

                //预成交费用
                qhcr = MCService.ComputeSPQHCost(code, Convert.ToSingle(ido.DealPrice), dealamount,
                                                 (GTA.VTS.Common.CommonObject.Types.UnitType)
                                                 Enum.Parse(typeof(GTA.VTS.Common.CommonObject.Types.UnitType), iut.ToString()),
                                                 openCloseType);
                //预成交费用
                dealCost = qhcr.Cosing;

                result = true;
            }
            catch (Exception ex)
            {
                strMessage = "GT-2610:[商品期货成交清算]开仓委托成交清算成交金额及费用计算异常.";
                LogHelper.WriteError(ex.Message, ex);
            }
            return result;
        }

        #endregion
    }

    /// <summary>
    /// 商品期货成交回报扩展类
    /// </summary>
    public class CommoditiesDealBackEntityEx : ReckonDealBackEx<CommoditiesDealBackEntity, QHCostResult>
    {
        public CommoditiesDealBackEntityEx(CommoditiesDealBackEntity dealBack)
            : base(dealBack)
        {
        }
        /// <summary>
        /// 成交金额（不包括费用）
        /// </summary>
        public decimal DealCapitalNoScale;

        /// <summary>
        /// 每笔的浮动盈亏
        /// </summary>
        public decimal FloatProfitLoss;
        /// <summary>
        /// 每笔的盯市盈亏
        /// </summary>
        public decimal MarketProfitLoss;

    }
}