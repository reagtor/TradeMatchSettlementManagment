#region Using Namespace

using System;
using System.Collections.Generic;
using System.Data.Common;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.DelegateValidate.Cost;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.DAL.CustomDataAccess;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.DAL.Data.HK;
using ReckoningCounter.DAL.MatchCenterOrderDealRpt;
using ReckoningCounter.Entity;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.MemoryData;
using ReckoningCounter.MemoryData.HK.Capital;
using ReckoningCounter.MemoryData.HK.Hold;
using ReckoningCounter.MemoryData.XH.Capital;
using ReckoningCounter.MemoryData.XH.Hold;
using ReckoningCounter.Model;
using ReckoningCounter.DAL.DevolveVerifyCommonService;

#endregion

namespace ReckoningCounter.BLL.Reckoning.Logic.HK
{
    /// <summary>
    /// 港股清算处理单元，错误编码2701-2790
    /// Create By:李健华
    /// Create date:2009-10-26
    /// </summary>
    public class HKReckonUnit : ReckonUnitBase<HK_TodayEntrustInfo, HKDealBackEntity, HK_TodayTradeInfo>
    {
        /// <summary>
        /// 是否已经完成当前委托清算（成交+撤单）
        /// </summary>
        protected bool hasDoneDeal;

        #region Overrides of ReckonUnitBase<HKTodayEntrustTable,HKDealBackEntity>

        /// <summary>
        /// 内部接收成交回报处理方法
        /// </summary>
        /// <param name="message">成交回报或者撤单回报</param>
        protected override bool InternalInsertMessage(object message)
        {
            if (hasDoneDeal)
            {
                LogHelper.WriteDebug("HKReckonUnit.InternalInsertMessage:委托已经完成，回报作废" + message);
                return false;
            }

            string id = string.Empty;
            string orderID = string.Empty;
            string desc = string.Empty;
            decimal dealAmount = 0;
            if (message is HKDealBackEntity)
            {
                HKDealBackEntity ido = (HKDealBackEntity)message;
                id = ido.ID;
                orderID = ido.OrderNo;
                desc = ido.DescInfo();
                dealAmount = ido.DealAmount;
            }
            else if (message is CancelOrderEntity)
            {
                CancelOrderEntity coe = (CancelOrderEntity)message;
                id = coe.Id;
                orderID = coe.OrderNo;
                desc = coe.DescInfo();
                dealAmount = coe.OrderVolume;
            }

            if (id == string.Empty)
            {
                LogHelper.WriteDebug("HKReckonUnit.InternalInsertMessage: 空ID");
                return false;
            }

            if (HasAddId(id))
            {
                LogHelper.WriteDebug("HKReckonUnit.InternalInsertMessage: 已添加ID" + id);
                return false;
            }

            string strMessage = string.Empty;
            bool isSuccess = InitializeOrderCache(orderID, ref strMessage);

            if (!isSuccess)
            {
                LogHelper.WriteInfo(strMessage);
            }

            if (HasAddReckoned(id))
            {
                LogHelper.WriteDebug("HKReckonUnit.InternalInsertMessage: 已处理ID" + id);
                return false;
            }

            AddID(id);

            string entrusNum = "[EntrustNumber=" + EntrustNumber + "]";
            LogHelper.WriteDebug("HKReckonUnit.InternalInsertMessage" + desc + entrusNum);

            //添加到内部的缓存列表
            if (message is HKDealBackEntity)
            {
                HKDealBackEntity ido = (HKDealBackEntity)message;
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
            var trades = HKDataAccess.GetTodayTradeListByEntrustNumber(EntrustNumber);
            if (trades == null)
                return;

            foreach (HK_TodayTradeInfo trade in trades)
            {
                AddReckonedID(trade.TradeNumber.Trim());

                if (trade.TradeTypeId == (int)Entity.Contants.Types.DealRptType.DRTDealed)
                {
                    string format = "[新TradeAmount{0}=旧TradeAmount{1}+以前成交量{2}]";
                    string msg = string.Format(format, TradeAmount + trade.TradeAmount, TradeAmount,
                                               trade.TradeAmount);
                    LogHelper.WriteDebug("HKReckonUnit.LoadReckonedIDList" + msg);
                    TradeAmount += trade.TradeAmount;
                }

                if (trade.TradeTypeId == (int)Entity.Contants.Types.DealRptType.DRTCanceled)
                {
                    string format = "[新CancelAmount{0}=旧CancelAmount{1}+以前成交量{2}]";
                    string msg = string.Format(format, CancelAmount + trade.TradeAmount, CancelAmount,
                                               trade.TradeAmount);
                    LogHelper.WriteDebug("HKReckonUnit.LoadReckonedIDList" + msg);
                    CancelAmount += trade.TradeAmount;
                }
            }

            HasLoadReckonedID = true;
        }

        protected override CounterCache GetCounterCache()
        {
            return HKCounterCache.Instance;
        }

        protected override void GetCurrencyType()
        {
            //===Update Date:2009-10-26 李健华
            //var currOjb = MCService.SpotTradeRules.GetCurrencyTypeByCommodityCode(Code);
            var currOjb = MCService.CommonPara.GetCurrencyTypeByCommodityCode(Types.BreedClassTypeEnum.HKStock, Code);
            //=========
            if (currOjb == null)
            {
                return;
            }

            CurrencyType = currOjb.CurrencyTypeID;
        }

        protected override void GetAccountID()
        {
            CapitalAccountId = MemoryDataManager.HKCapitalMemoryList.GetCapitalAccountLogo(CapitalAccount, CurrencyType);

            HoldingAccountId = MemoryDataManager.HKHoldMemoryList.GetAccountHoldLogoId(HoldingAccount, Code, CurrencyType);
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

            //因为可能有Type2改单，会导致多个撤单出现
            //if (list.Count > 1)
            //{
            //    LogHelper.WriteDebug("HKReckonUnit.ProcessCancelCommit: 收到多个撤单回报，只处理第一个，其他的抛弃！");
            //}

            coe = list[0];

            //如果是错误价格的撤单，马上执行
            if (coe.OrderVolume == -1)
                return true;

            decimal cancelSum = 0;
            foreach (var cancelOrderEntity in list)
            {
                cancelSum += cancelOrderEntity.OrderVolume;
            }

            //如果TradeAmount+CancelAmount+cancelSum!=EntrustAmount,
            //那么认为虽然收到了撤单回报，但是还有部分成交回报还没回来，那么
            //先不进行撤单处理，等所有的成交回报都清算完了，最后再进行撤单处理
            decimal allAmount = TradeAmount + CancelAmount + cancelSum;
            if (allAmount < EntrustAmount)
            {
                string format2 =
                    "HKReckonUnit.ProcessCancelCommit尚有部分成交回报未收到或未处理[EntrustAmount={0},TradeAmount={1},CancelAmount={2},CancelBackAmount={3}]";
                string info = string.Format(format2, EntrustAmount, TradeAmount, CancelAmount, coe.OrderVolume);
                //LogHelper.WriteInfo(info);

                //还有成交回报没有回来，继续放入撤单列表等待下次撤单处理
                foreach (var cancelOrderEntity in list)
                {
                    cancelBackList.Add(cancelOrderEntity);
                }

                return false;
            }

            //如果是Type2的改单，那么将所有的撤单整合成一个撤单处理
            if (list.Count > 1)
            {
                var coe2 = new CancelOrderEntityEx();
                coe2.IsMultiCancelOrder = true;
                coe2.ChannelNo = list[0].ChannelNo;
                coe2.IsSuccess = list[0].IsSuccess;
                coe2.Message = "";
                coe2.OrderNo = list[0].OrderNo;
                coe2.IDList = new List<string>();
                //add 李健华 2009-11-07============
                coe2.OrderVolumeList = new List<decimal>();
                //======
                string cancelDesc = "";
                string cancelDescFormat = "(ID={0},Volume={1})";

                foreach (var cancelOrderEntity in list)
                {
                    coe2.OrderVolume += cancelOrderEntity.OrderVolume;
                    coe2.IDList.Add(cancelOrderEntity.Id);
                    //add 李健华 2009-11-07============
                    coe2.OrderVolumeList.Add(cancelOrderEntity.OrderVolume);
                    //=======
                    cancelDesc += string.Format(cancelDescFormat, cancelOrderEntity.Id, cancelOrderEntity.OrderVolume);
                }

                string format = "HKReckonUnit.CanProcessCancelCommit类型2改单，将所有的撤单整合成一个撤单处理[EntrustNumber={0}][{1}]";
                string desc = string.Format(format, EntrustNumber, cancelDesc);
                LogHelper.WriteDebug(desc);

                coe = coe2;
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
            HK_TodayEntrustInfo tet;
            List<HK_TodayTradeInfo> trades;
            bool isSuccess = false;

            //是否是废单
            //bool isBadOrder = false;

            CrashManager.GetInstance().AddReckoningID(coe.Id);

            //错误价格的撤单
            if (coe.OrderVolume == -1)
            {
                isSuccess = InstantReckon_CancelOrder(coe, out tet, out trades, true, ref strMessage);

                ////没有成功就马上再做一次
                //if (!isSuccess)
                //    isSuccess = InstantReckon_CancelOrder(coe, out tet, out trades, false);
            }
            else
            {
                //正常撤单
                isSuccess = InstantReckon_CancelOrder(coe, out tet, out trades, false, ref strMessage);

                ////没有成功就马上再做一次
                //if (!isSuccess)
                //    isSuccess = InstantReckon_CancelOrder(coe, out tet, out trades, true);
            }

            bool isMultiCancelOrder = false;
            CancelOrderEntityEx coeEx = null;
            if (coe is CancelOrderEntityEx)
            {
                coeEx = coe as CancelOrderEntityEx;

                if (coeEx.IsMultiCancelOrder)
                    isMultiCancelOrder = true;
            }

            if (!isSuccess)
            {
                //如果清算没有成功，那么从保存的回报列表中删掉，下次CrashManger可以再次发送此回报过来
                LogHelper.WriteInfo(
                    "#####################HKReckonUnit.ProcessCancelCommit撤单清算失败，CancelOrderEntity[ID=" + coe.Id +
                    "], Message=" + strMessage);

                if (isMultiCancelOrder)
                {
                    foreach (var id in coeEx.IDList)
                    {
                        RemoveID(id);
                    }
                }
                else
                {
                    RemoveID(coe.Id);
                }
            }

            if (!string.IsNullOrEmpty(strMessage))
                HKDataAccess.UpdateEntrustOrderMessage(EntrustNumber, strMessage);

            if (isMultiCancelOrder)
            {
                foreach (var id in coeEx.IDList)
                {
                    CrashManager.GetInstance().RemoveReckoningID(id);
                }
            }
            else
            {
                CrashManager.GetInstance().RemoveReckoningID(coe.Id);
            }

            if (isSuccess)
            {
                ReckonEndObject<HK_TodayEntrustInfo, HK_TodayTradeInfo> cancelEndObject =
                    new ReckonEndObject<HK_TodayEntrustInfo, HK_TodayTradeInfo>();
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

            if (hasDoneDeal)
            {
                bool isCheckSuccess = LastCheckFreezeMoney(EntrustNumber, CapitalAccountId);
                if (!isCheckSuccess)
                {
                    RescueManager.Instance.Record_HK_LastCheckFreezeMoney(EntrustNumber, CapitalAccountId);
                }

                //如果已经全部清算完，那么验证一下是否是Type2的改单，如果是的话，那么要修改当前委托的各种相关信息
                HKModifyOrderRequest hkModifyRequest1;
                if (ModifyOrderProcessor.Instance.IsExistType2Request(EntrustNumber, out hkModifyRequest1))
                {
                    ModifyOrderProcessor.Instance.ProcessType2ModifyOrder(EntrustNumber);
                }
                //update 2009-11-07 李健华 
                //这里可能是先改单交易量减少，然后再增加交易量，所以不能否则判断,两个列表都检查
                //else
                //======
                //{
                //如果不是Type2的改单，那么再验证一下是否是Type3的改单，如果是的话，那么要生成新的委托，
                //并修改当前委托的各种相关信息
                HKModifyOrderRequest hkModifyRequest;
                if (ModifyOrderProcessor.Instance.IsExistType3Request(EntrustNumber, out hkModifyRequest))
                {
                    ModifyOrderProcessor.Instance.ProcessType3NewOrder(hkModifyRequest);
                }
                // }
            }
        }

        /// <summary>
        /// 撤单清算
        /// </summary>
        /// <param name="rde">撤单回报</param>
        /// <param name="tet">委托</param>
        /// <param name="trades">输出的成交列表</param>
        /// <param name="isErrorPriceCancelRpt">是否是错误价格的撤单</param>
        /// <param name="strMessage">错误信息</param>
        /// <returns>是否成功</returns>
        public bool InstantReckon_CancelOrder(CancelOrderEntity rde,
                                              out HK_TodayEntrustInfo tet, out List<HK_TodayTradeInfo> trades,
                                              bool isErrorPriceCancelRpt, ref string strMessage)
        {
            #region 初始化变量

            tet = null;
            trades = null;

            if (!InitializeOrderCache(rde.OrderNo, ref strMessage))
            {
                LogHelper.WriteInfo(strMessage);
                return false;
            }

            string entrustNumString = "[EntrustNumber=" + EntrustNumber + "]";

            string canDesc = "";
            CancelOrderEntityEx rdeEx = null;
            bool isMultiCancelOrder = false;
            if (rde is CancelOrderEntityEx)
            {
                rdeEx = rde as CancelOrderEntityEx;

                if (rdeEx.IsMultiCancelOrder)
                {
                    isMultiCancelOrder = true;
                    canDesc = "——类型2撤单";
                }

                if (rdeEx.IsInternalCancelOrder)
                {
                    canDesc = "——内部撤单";
                }
            }

            LogHelper.WriteDebug(
                "------xxxxxx------开始进行港股外部撤单(撮合返回撤单回报[正常或者有价格问题的])HKReckonUnit.InstantReckon_CancelOrder2" +
                rde.DescInfo() + entrustNumString + canDesc);



            //港股当日委托回报
            //tet = HKDataAccess.GetTodayEntrustTable(EntrustNumber);
            //因为某些异常导致清算失败，但是当日委托被转入历史委托，以后再清算会找不到委托，所以现在改为
            //在当日找不到后到历史中找
            tet = HKDataAccess.GetAllEntrustTable(EntrustNumber);
            if (tet == null)
            {
                strMessage = "GT-2701:[港股撤单清算]无法获取委托对象，委托单号=" + EntrustNumber;
                LogHelper.WriteDebug(strMessage);
                return false;
            }

            //取代码对应品种的交易币种
            if (CurrencyType == -1)
            {
                //===Update Date:2009-10-26 李健华
                // CurrencyType = MCService.SpotTradeRules.GetCurrencyTypeByCommodityCode(tet.Code).CurrencyTypeID;
                CurrencyType = MCService.CommonPara.GetCurrencyTypeByCommodityCode(Types.BreedClassTypeEnum.HKStock, tet.Code).CurrencyTypeID;
                //==============
            }

            var hasDealedAmount = TradeAmount + CancelAmount;

            if (hasDealedAmount == EntrustAmount)
            {
                string msg = "撤单回报错误：当前委托已经清算完毕，撤单回报无效！" + entrustNumString;
                LogHelper.WriteDebug(msg);

                //将此问题id加入到已清算id列表中
                if (isMultiCancelOrder)
                {
                    foreach (var id in rdeEx.IDList)
                    {
                        AddReckonedID(id);
                        //清除数据库中保存的成交回报
                        CrashManager.GetInstance().DeleteEntity(id);
                    }
                }
                else
                {
                    AddReckonedID(rde.Id);
                    //清除数据库中保存的成交回报
                    CrashManager.GetInstance().DeleteEntity(rde.Id);
                }

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
                    if (isMultiCancelOrder)
                    {
                        foreach (var id in rdeEx.IDList)
                        {
                            AddReckonedID(id);
                            //清除数据库中保存的成交回报
                            CrashManager.GetInstance().DeleteEntity(id);
                        }
                    }
                    else
                    {
                        AddReckonedID(rde.Id);
                        //清除数据库中保存的成交回报
                        CrashManager.GetInstance().DeleteEntity(rde.Id);
                    }

                    //表示清算还是成功了，这笔单在故障恢复时不需要重新发送
                    return true;
                }
            }

            bool isSuccess = false;

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

            //3.持仓冻结处理(买入不处理）
            //把持仓冻结记录中的冻结量还给持仓表，删除冻结记录(实际上只清零，不删除，盘后统一删）

            //4.持仓处理（买入不处理）
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

            var ca = HKDataAccess.GetCapitalAccountFreeze(EntrustNumber, Entity.Contants.Types.FreezeType.DelegateFreeze);
            if (ca == null)
            {
                string msg = "[港股撤单清算]资金冻结记录不存在." + entrustNumString;
                LogHelper.WriteInfo(msg);
                //找不到资金冻结，一样允许撤单，当作冻结的资金全部为0
                //InternalCancelOrderFailureProcess(EntrustNumber, 0, 0, 0, strErrorMessage);
                //return false;
            }
            else
            {
                preFreezeCapital = ca.FreezeCapitalAmount;
                preFreezeCost = ca.FreezeCost;
                capitalFreezeLogoId = ca.CapitalFreezeLogoId;
            }

            #endregion

            #region 2.资金预处理，把从资金冻结记录还回来的金额和费用加到可用资金，并减去总冻结资金（放后面提交）

            HK_CapitalAccount_DeltaInfo capitalDelta = new HK_CapitalAccount_DeltaInfo();
            capitalDelta.CapitalAccountLogo = CapitalAccountId;
            decimal delta = preFreezeCapital + preFreezeCost;

            HKCapitalMemoryTable capMemory = null;
            if (delta != 0)
            {
                capitalDelta.AvailableCapitalDelta = delta;
                capitalDelta.FreezeCapitalTotalDelta = -delta;

                capMemory = MemoryDataManager.HKCapitalMemoryList.GetByCapitalAccountLogo(CapitalAccountId);
                if (capMemory == null)
                {
                    strMessage = "GT-2702:[港股撤单清算]资金帐户不存在:" + CapitalAccount;
                    LogHelper.WriteDebug(strMessage);
                    return false;
                }
            }

            #endregion

            decimal preFreezeHoldAmount = 0;
            HK_AccountHoldInfo_Delta holdDelta = new HK_AccountHoldInfo_Delta();
            HKHoldMemoryTable holdMemory = null;
            if (tet.BuySellTypeID == (int)Types.TransactionDirection.Selling)
            {
                #region 3.持仓冻结处理(买入不计算）,获取持仓冻结记录中的冻结量和冻结id

                var hold = HKDataAccess.GetHoldAccountFreeze(EntrustNumber,
                                                             Entity.Contants.Types.FreezeType.DelegateFreeze);
                if (hold == null)
                {
                    string msg = "[港股撤单清算]持仓冻结不存在";
                    LogHelper.WriteInfo(msg);
                    //持仓冻结不存在，一样运行撤单，当作持仓冻结量为0
                    //InternalCancelOrderFailureProcess(EntrustNumber, -preFreezeCapital, -preFreezeCost, 0, strErrorMessage);
                    //return false;
                }
                else
                {
                    preFreezeHoldAmount = hold.PrepareFreezeAmount;
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
                    string format =
                        "港股撤单清算-回报冻结量与数据库冻结量不相等[EntrustNumber={0},CancelOrderVolume={1},FreezeHoldAmount={2}]";
                    string desc = string.Format(format, EntrustNumber, cancelOrderVolume, preFreezeHoldAmount);
                    preFreezeHoldAmount = cancelOrderVolume;
                    LogHelper.WriteDebug(desc);
                }

                #endregion

                holdMemory = MemoryDataManager.HKHoldMemoryList.GetByAccountHoldLogoId(HoldingAccountId);
                if (holdMemory == null)
                {
                    holdMemory = HKCommonLogic.GetHoldMemoryTable(HoldingAccount, Code, CurrencyType);
                }

                if (holdMemory == null)
                {
                    strMessage = "GT-2703:[港股撤单清算]持仓账户不存在:" + HoldingAccount;
                    return false;
                }

                var holdData = holdMemory.Data;
                holdDelta.Data = holdData;
                holdDelta.AccountHoldLogoId = holdData.AccountHoldLogoID;
                holdDelta.AvailableAmountDelta += preFreezeHoldAmount;
                holdDelta.FreezeAmountDelta -= preFreezeHoldAmount;

                #endregion
            }

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

            HK_TodayTradeInfo trade = null;

            List<HK_TodayTradeInfo> multi_Trade = new List<HK_TodayTradeInfo>();

            #region 数据库提交动作

            isSuccess = false;
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

                        HKDataAccess.ClearCapitalFreeze(capitalFreezeLogoId, db, transaction);

                        if (tet.BuySellTypeID == (int)Types.TransactionDirection.Selling)
                        {
                            HKDataAccess.ClearHoldFreeze(holdFreezeLogoId, db, transaction);
                        }

                        HKDataAccess.UpdateEntrustTable(tet2, db, transaction);

                        //撤单也要加入当日成交表中
                        ReckoningTransaction rt = new ReckoningTransaction();
                        rt.Database = db;
                        rt.Transaction = transaction;

                        bool isInternalCancelOrder = false;
                        if (rde is CancelOrderEntityEx)
                        {
                            var rde2 = (CancelOrderEntityEx)rde;
                            isInternalCancelOrder = rde2.IsInternalCancelOrder;
                        }
                        #region update by:李健华 update date:2009-11-6
                        #region update=================old code=========
                        //update=================old code=========
                        //HKCommonLogic.BuildHKCancelRpt(tet2, rde, rt, out trade, isInternalCancelOrder);

                        //if (isMultiCancelOrder)
                        //{
                        //    foreach (var id in rdeEx.IDList)
                        //    {
                        //        //清算完成后清除数据库中保存的成交回报
                        //        CrashManager.GetInstance().DeleteEntity(id, rt);
                        //    }
                        //}
                        //else
                        //{
                        //    //清算完成后清除数据库中保存的成交回报
                        //    CrashManager.GetInstance().DeleteEntity(rde.Id, rt);
                        //}
                        //update=================old code=========
                        #endregion

                        if (isMultiCancelOrder)
                        {
                            for (int i = 0; i < rdeEx.IDList.Count; i++)
                            {
                                CancelOrderEntity newRde = new CancelOrderEntity();
                                newRde = rdeEx;
                                newRde.Id = rdeEx.IDList[i]; //因为多个撤单组合ID已经不为基类中的
                                newRde.OrderVolume = rdeEx.OrderVolumeList[i];//这里如果要分多笔记录那要记录之前的撤单量
                                HK_TodayTradeInfo mtrade = null;

                                //生成成交记录存库并输入出成交回报数据
                                HKCommonLogic.BuildHKCancelRpt(tet2, newRde, rt, out mtrade, isInternalCancelOrder);

                                if (mtrade != null)
                                {
                                    multi_Trade.Add(mtrade);
                                }

                                //清算完成后清除数据库中保存的成交回报
                                CrashManager.GetInstance().DeleteEntity(rdeEx.IDList[i], rt);
                            }
                        }
                        else
                        {
                            //生成成交记录存库并输入出成交回报数据
                            HKCommonLogic.BuildHKCancelRpt(tet2, rde, rt, out trade, isInternalCancelOrder);
                            //清算完成后清除数据库中保存的成交回报
                            CrashManager.GetInstance().DeleteEntity(rde.Id, rt);
                        }
                        #endregion
                        //资金操作
                        if (delta != 0)
                        {
                            capMemory.AddDeltaToDB(capitalDelta, db, transaction);
                        }

                        //持仓操作
                        if (tet.BuySellTypeID == (int)Types.TransactionDirection.Selling)
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
                        LogHelper.WriteError(ex.Message, ex);
                        strMessage = "GT-2704:[港股撤单清算]提交到数据库失败";

                        isSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                strMessage = "GT-2704:[港股撤单清算]提交到数据库失败";
            }

            if (isSuccess)
            {
                if (delta != 0)
                {
                    capMemory.AddDeltaToMemory(capitalDelta);
                }

                if (tet.BuySellTypeID == (int)Types.TransactionDirection.Selling)
                {
                    if (preFreezeHoldAmount != 0)
                    {
                        holdMemory.AddDeltaToMemory(holdDelta);

                        holdMemory.ReadAndWrite(h =>
                                                    {
                                                        if (h.FreezeAmount < 0)
                                                        {
                                                            h.FreezeAmount = 0;
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
            if (isMultiCancelOrder)
            {
                foreach (var id in rdeEx.IDList)
                {
                    AddReckonedID(id);
                }
            }
            else
            {
                AddReckonedID(rde.Id);
            }

            if (rde.OrderVolume == -1)
            {
                CancelAmount = EntrustAmount;
                strMessage = rde.Message;
            }
            else
            {
                CancelAmount += rde.OrderVolume;
            }


            trades = new List<HK_TodayTradeInfo>();
            if (isMultiCancelOrder)
            {
                trades = multi_Trade;
            }
            else
            {
                if (trade != null)
                {
                    trades.Add(trade);
                }
            }

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

            string format = "港股成交清算[EntrustNumber={0},DealBackCount={1}";
            string info = string.Format(format, EntrustNumber, list.Count);
            LogHelper.WriteInfo(info);

            string strMessage = "";
            HK_TodayEntrustInfo tet;
            List<HK_TodayTradeInfo> tradeList;

            bool isSuccess = false;

            List<string> ids = GetIdList(list);

            CrashManager.GetInstance().AddReckoningIDList(ids);

            isSuccess = InstantReckon_Deal(list, ref strMessage, out tet, out tradeList);

            if (!string.IsNullOrEmpty(strMessage))
                HKDataAccess.UpdateEntrustOrderMessage(EntrustNumber, strMessage);

            //失败，那么进行故障恢复流程
            if (!isSuccess)
            {
                //如果清算没有成功，那么从保存的回报列表中删掉，下次CrashManger可以再次发送此回报过来
                LogHelper.WriteInfo("#####################HKReckonUnit.ProcessDealCommit成交清算失败:" + strMessage);
                //"，HKDealBackEntity[ID=" + ido.Id +"]");
                RemoveIDList(ids);
            }
            CrashManager.GetInstance().RemoveReckoningIDList(ids);

            if (isSuccess)
            {
                ReckonEndObject<HK_TodayEntrustInfo, HK_TodayTradeInfo> reckonEndObject =
                    new ReckonEndObject<HK_TodayEntrustInfo, HK_TodayTradeInfo>();
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
                    RescueManager.Instance.Record_HK_LastCheckFreezeMoney(EntrustNumber, CapitalAccountId);
                }
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
        /// 最后一次清算时，要把冻结记录清零
        /// </summary>
        public static bool LastCheckFreezeMoney(string entrustNumber, int capitalAccountId)
        {
            if (string.IsNullOrEmpty(entrustNumber))
                return true;

            if (capitalAccountId == -1)
                return true;

            //当最后一次清算时，要把冻结记录清零，多出的钱要还到资金表中，即“多了要退”并且总冻结资金要减去这部分钱;

            var caf = HKDataAccess.GetCapitalAccountFreeze(entrustNumber,
                                                           Entity.Contants.Types.FreezeType.DelegateFreeze);

            if (caf == null)
                return false;

            //需要还给资金表中的钱
            decimal needAddMoney = 0;
            if (caf.FreezeCapitalAmount != 0)
            {
                needAddMoney = caf.FreezeCapitalAmount;
            }

            if (caf.FreezeCost != 0)
            {
                needAddMoney += caf.FreezeCost;
            }

            if (caf.FreezeCost == 0 && caf.FreezeCapitalAmount == 0)
                return true;

            HK_CapitalAccount_DeltaInfo delta = new HK_CapitalAccount_DeltaInfo();
            delta.CapitalAccountLogo = capitalAccountId;
            delta.AvailableCapitalDelta = needAddMoney;
            delta.FreezeCapitalTotalDelta = -needAddMoney;

            var capMemory = MemoryDataManager.HKCapitalMemoryList.GetByCapitalAccountLogo(capitalAccountId);
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
                        HKDataAccess.ClearCapitalFreeze(caf.CapitalFreezeLogoId, database, transaction);

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

            var caf = HKDataAccess.GetCapitalAccountFreeze(EntrustNumber,
                                                           Entity.Contants.Types.FreezeType.DelegateFreeze);

            //需要从资金表中扣去的钱
            decimal needRemoveMoney = 0;
            if (caf != null)
            {
                if (caf.FreezeCapitalAmount < 0)
                {
                    needRemoveMoney = -caf.FreezeCapitalAmount;
                    caf.FreezeCapitalAmount = 0;
                }

                if (caf.FreezeCost < 0)
                {
                    needRemoveMoney += (-caf.FreezeCost);
                    caf.FreezeCost = 0;
                }
            }

            var capMemory = MemoryDataManager.HKCapitalMemoryList.GetByCapitalAccountLogo(CapitalAccountId);
            if (capMemory == null)
            {
                return;
            }

            HK_CapitalAccount_DeltaInfo delta = new HK_CapitalAccount_DeltaInfo();
            delta.CapitalAccountLogo = CapitalAccountId;
            delta.AvailableCapitalDelta = -needRemoveMoney;
            delta.FreezeCapitalTotalDelta = needRemoveMoney;

            bool isSuccess = capMemory.AddDelta(delta);
            if (!isSuccess)
            {
                isSuccess = capMemory.AddDelta(delta);
            }

            if (!isSuccess)
            {
            }
        }


        private bool ComputePreprocCapital(HKDealBackEntity ido, string code, Types.TransactionDirection buySellType,
                                           ref string strMessage, ref decimal dealCapital,
                                           ref decimal dealCost, out HKCostResult xhcr)
        {
            bool result = false;
            xhcr = null;

            try
            {
                //搓合单位
                Types.UnitType utMatch = MCService.GetMatchUnitType(code, Types.BreedClassTypeEnum.HKStock);
                //搓合单位 -> 计价单位 倍数
                decimal unitMultiple = MCService.GetTradeUnitScale(Types.BreedClassTypeEnum.HKStock, code, utMatch);
                //当前成交费用对象
                xhcr = MCService.ComputeHKCost(code, Decimal.ToSingle(ido.DealPrice),
                                               decimal.ToInt32(ido.DealAmount), utMatch, buySellType);
                //成交金额
                dealCapital = ido.DealAmount * unitMultiple * ido.DealPrice;
                //四舍五入
                dealCapital = Utils.Round(dealCapital);

                //成交费用
                dealCost = xhcr.CoseSum;
                //四舍五入
                dealCost = Utils.Round(dealCost);

                result = true;
            }
            catch (Exception ex)
            {
                strMessage = "GT-2705:[港股成交清算]买委托成交清算成交金额及费用计算异常.";
                LogHelper.WriteError(ex.Message, ex);
            }
            return result;
        }

        public bool InstantReckon_Deal(List<HKDealBackEntity> idoList, ref string strMessage,
                                       out HK_TodayEntrustInfo tet, out List<HK_TodayTradeInfo> tradeList)
        {
            #region ==初始化变量 ==

            //bool result = false;
            strMessage = string.Empty;
            tet = null;
            tradeList = null;
            List<string> ids = GetIdList(idoList);

            string entrustNum = "[EntrustNumber=" + EntrustNumber + "]";

            string format = "即时清算HKRecikonUnit.InstantReckon_Deal[资金帐户={0},持仓帐户={1},柜台委托单号={2}]";
            string msg = string.Format(format, CapitalAccount, HoldingAccount, EntrustNumber);
            LogHelper.WriteDebug(msg);

            var hasDealedAmount = TradeAmount + CancelAmount;
            if (hasDealedAmount == EntrustAmount)
            {
                msg = "即时清算HKRecikonUnit.InstantReckon_Deal-成交回报错误：当前委托已经清算完毕，成交回报无效！EntrustNumber=" +
                      EntrustNumber;
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

            List<HKDealBackEntityEx> idoexList;
            HKDealSum dealSum;
            bool isSuccess = ComputeHKDealBacks(idoList, out idoexList, out dealSum, ref strMessage);
            if (!isSuccess)
            {
                strMessage = "GT-2723:[港股成交清算]成交回报汇总失败.";
                return false;
            }

            if ((hasDealedAmount + dealSum.AmountSum) > EntrustAmount)
            {
                msg = "即时清算HKRecikonUnit.InstantReckon_Deal-成交回报错误：成交回报成交数量{0}+已清算的数量{1}>当前委托量{2}！EntrustNumber=" +
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
                //===Update Date:2009-10-26 李健华
                //   CurrencyType = MCService.SpotTradeRules.GetCurrencyTypeByCommodityCode(Code).CurrencyTypeID;
                CM_CurrencyType cm_cur = MCService.CommonPara.GetCurrencyTypeByCommodityCode(Types.BreedClassTypeEnum.HKStock, Code);
                if (cm_cur == null)
                {
                    strMessage = "GT-2723:[港股成交清算]根据代码获取交易货币类型失败.";
                    return false;
                }
                CurrencyType = cm_cur.CurrencyTypeID;
                //================
            }


            //交割制度获获取(此方法没有用到数据库)
            if (CaptitalTradingRule == -1 || HoldingTradingRule == -1)
            {
                int captitalTradingRule;
                int holdingTradingRule;

                #region update by 李健华 2009-10-29
                //if (!MCService.SpotTradeRules.GetDeliveryInstitution(Code, out captitalTradingRule,
                //                                                     out holdingTradingRule, ref strMessage))
                //{
                //    strMessage = "GT-2706:[港股成交清算]无法获取交割制度.";

                //    HKDataAccess.UpdateEntrustOrderMessage(entrustNum, strMessage);
                //    return false;
                //}
                if (!MCService.HKTradeRulesProxy.GetHKDeliveryInstitution(Code, out captitalTradingRule,
                                                     out holdingTradingRule, ref strMessage))
                {
                    strMessage = "GT-2706:[港股成交清算]无法获取交割制度.";
                    HKDataAccess.UpdateEntrustOrderMessage(entrustNum, strMessage);
                    return false;
                }
                #endregion

                CaptitalTradingRule = captitalTradingRule;
                HoldingTradingRule = holdingTradingRule;
            }

            //港股当日委托回报
            tet = HKDataAccess.GetTodayEntrustTable(EntrustNumber);
            if (tet == null)
            {
                strMessage = "GT-2707:[港股成交清算]无法获取委托对象，委托单号=" + EntrustNumber;
                LogHelper.WriteDebug(strMessage);
                return false;
            }

            #endregion

            if (BuySellType == Types.TransactionDirection.Buying)
            {
                return HKBuy_InstantReckon_Deal(idoexList, dealSum, ref strMessage, out tet, out tradeList);
            }

            return HKSell_InstantReckon_Deal(idoexList, dealSum, ref strMessage, out tet, out tradeList);
        }

        #endregion

        #region 港股买清算处理

        /// <summary>
        /// 港股买清算提交入口
        /// </summary>
        /// <param name="idoExList">扩展成交回报列表</param>
        /// <param name="dealSum">港股成交回报汇总</param>
        /// <param name="strMessage">错误信息</param>
        /// <param name="tet">委托</param>
        /// <param name="tradeList">成交列表</param>
        /// <returns>清算是否成功</returns>
        private bool HKBuy_InstantReckon_Deal(List<HKDealBackEntityEx> idoExList, HKDealSum dealSum,
                                              ref string strMessage, out HK_TodayEntrustInfo tet,
                                              out List<HK_TodayTradeInfo> tradeList)
        {
            //买清算：
            //1.资金及冻结处理
            //(1) 根据成交金额和费用，从资金冻结记录减去相应的值，并从资金表总冻结资金中减去;
            //(2) 当冻结记录里钱不够扣时，直接从资金表扣，即“少了要补”；
            //(3) 当最后一次成交清算时，要把冻结记录清零，多出的钱要还到资金表中，并且总冻结资金要减去这部分钱;
            //2.持仓及冻结处理
            //    根据持仓交割规则：
            //    a.T+0: 将成交量加入持仓表，更新成本价、保本价，计算持仓均价等等，不牵涉持仓冻结表。
            //    b.T+N:将成交量加入到持仓冻结表，更新持仓表的成本价、保本价，计算持仓均价等等，更新持仓表的总冻结量。
            //3.委托表处理
            //    更新各种状态（成交量，状态等）


            tet = null;
            tradeList = new List<HK_TodayTradeInfo>();

            //总成交金额（包括capital和cost）
            decimal dealMoneySum = dealSum.CapitalSum + dealSum.CostSum;

            #region 1.资金表预处理

            //先检查资金冻结表中的冻结金额和费用是否够扣

            HK_CapitalAccountFreezeInfo caf;
            if (!GetCapitalFreezeTable(out caf))
            {
                strMessage = "GT-2724:[港股成交清算]无法获取资金冻结记录.";

                return false;
            }

            #region 资金表与资金冻结补偿逻辑——为了简化处理，不再进行补偿，统一改为清算结束后进行检查补偿

            /*
            decimal freezeCapital = caf.FreezeCapitalAmount;
            decimal freezeCost = caf.FreezeCost;

            //需要补的资金
            decimal removeCapital = 0;

            //需要补的费用
            decimal removeCost = 0;

            //需要减去的冻结资金
            decimal removeFreeze = 0;

            freezeCapital -= dealSum.CapitalSum;
            if (freezeCapital < 0)
            {
                removeCapital = -freezeCapital;
                removeFreeze += caf.FreezeCapitalAmount;
                caf.FreezeCapitalAmount = 0;
            }
            else
            {
                removeFreeze += dealSum.CapitalSum;
                caf.FreezeCapitalAmount -= dealSum.CapitalSum;
            }


            freezeCost -= dealSum.CostSum;
            if (freezeCost < 0)
            {
                removeCost = -freezeCost;
                removeFreeze += caf.FreezeCost;
                caf.FreezeCost = 0;
            }
            else
            {
                removeFreeze += dealSum.CostSum;
                caf.FreezeCost -= dealSum.CostSum;
            }

            //需要补的总金额
            decimal removeTotal = removeCapital + removeCost;

            //需要退的钱
            decimal tuiAll = 0;
            if (isLastReckon)
            {
                //需要退的金额
                decimal tuiCapital = caf.FreezeCapitalAmount;
                caf.FreezeCapitalAmount = 0;

                //需要退的费用
                decimal tuiCost = caf.FreezeCost;
                caf.FreezeCost = 0;

                tuiAll = tuiCapital + tuiCost;
            }
            */

            #endregion

            var capMemory = MemoryDataManager.HKCapitalMemoryList.GetByCapitalAccountLogo(CapitalAccountId);
            if (capMemory == null)
            {
                strMessage = "GT-2708:[港股成交清算]资金帐户不存在:" + CapitalAccount;

                return false;
            }

            HK_CapitalAccount_DeltaInfo capitalDelta = HKBuy_InstantReckon_PreCapitalProcess(dealSum, caf,
                                                                                                  dealMoneySum);
            capitalDelta.CapitalAccountLogo = capMemory.Data.CapitalAccountLogo;
            //资金表不再首先提交，等待后面事务成功后再与持仓一起提交
            //capMemory.AddDelta(capitalDelta);

            #endregion

            #region 2.持仓表预处理

            bool isSuccess = false;

            HKHoldMemoryTable holdMemory = null;
            holdMemory = HKBuy_InstantReckon_PreHoldingProcess(ref isSuccess, ref strMessage);

            if (!isSuccess)
            {
                //再获取一次
                holdMemory = HKBuy_InstantReckon_PreHoldingProcess(ref isSuccess, ref strMessage);
            }

            if (!isSuccess)
            {
                strMessage = "GT-2790:[港股成交清算]无法获取持仓记录.";
                LogHelper.WriteDebug("XHBuy_InstantReckon_Deal_!isSuccess " + strMessage);
                return false;
            }

            if (holdMemory == null)
            {
                strMessage = "GT-2790:[港股成交清算]无法获取持仓记录.";
                LogHelper.WriteDebug("XHBuy_InstantReckon_Deal_holdMemory == null " + strMessage);

                return false;
            }

            #endregion

            //处理资金冻结和持仓冻结,以及委托信息
            HK_AcccountHoldFreezeInfo holdFreeze = null;
            bool isFirstHold = false;
            if (HoldingTradingRule != 0)
            {
                holdFreeze = HKBuy_InstantReckon_HoldingFreezeProcess(dealSum, ref isFirstHold);
            }

            tet = HKDataAccess.GetTodayEntrustTable(EntrustNumber);
            var tet2 = tet;

            HKBuy_InstantReckon_EntrustProcess(dealSum, tet2);

            var tradeList2 = tradeList;

            List<string> ids = GetIDList2(idoExList);

            #region 数据库提交操作

            isSuccess = false;
            Database database = DatabaseFactory.CreateDatabase();
            try
            {
                using (DbConnection connection = database.CreateConnection())
                {
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();
                    ReckoningTransaction tm = new ReckoningTransaction();
                    tm.Database = database;
                    tm.Transaction = transaction;
                    try
                    {
                        //1.更新资金冻结
                        HK_CapitalAccountFreezeDal cafDal =
                            new HK_CapitalAccountFreezeDal();
                        cafDal.Update(caf, tm);

                        //2.更新持仓冻结
                        if (HoldingTradingRule != 0)
                        {
                            HK_AcccountHoldFreezeDal holdDal =
                                new HK_AcccountHoldFreezeDal();
                            if (isFirstHold)
                                holdDal.Add(holdFreeze, tm.Database, tm.Transaction);
                            else
                                holdDal.Update(holdFreeze, tm.Database, tm.Transaction);
                        }

                        //3.更新委托表
                        HKDataAccess.UpdateEntrustTable(tet2, tm.Database,
                                                        tm.Transaction);
                        //成交时间
                        //tet2.OfferTime = ido.DealTime;

                        //4.生成成交记录
                        BuildTradeList(tet2, tm, idoExList, tradeList2);

                        //5.清算完成后清除数据库中保存的成交回报
                        CrashManager.GetInstance().DeleteEntityList(ids, tm);

                        //6.更新资金表
                        capMemory.AddDeltaToDB(capitalDelta, tm.Database, tm.Transaction);

                        transaction.Commit();
                        isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogHelper.WriteError(ex.Message, ex);
                        strMessage = "GT-2709:[港股成交清算]数据库提交失败";

                        isSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                strMessage = "GT-2709:[港股成交清算]数据库提交失败";
            }

            if (isSuccess)
            {
                capMemory.AddDeltaToMemory(capitalDelta);
            }
            else
            {
                return false;
            }

            #endregion

            #region 4.持仓表真正提交处理

            //之所以放到最后执行，是因为买入时持仓表的操作太多，难以回滚，所以放到最后，保证必须执行）

            isSuccess = HKBuy_InstantReckon_HoldingProcess(holdMemory, dealSum, HoldingTradingRule);

            if (!isSuccess)
            {
                //不成功连续执行3次
                for (int i = 0; i < 3; i++)
                {
                    //不成功，再执行一次
                    isSuccess = HKBuy_InstantReckon_HoldingProcess(holdMemory, dealSum, HoldingTradingRule);

                    if (isSuccess)
                        break;
                }
            }

            if (!isSuccess)
            {
                //一定要执行，记录下来等下次执行
                RescueManager.Instance.Record_HKBuy_InstantReckon_HoldingProcess(HoldingAccountId, dealSum,
                                                                                 HoldingTradingRule);
            }

            #endregion

            #region 资金表真正提交处理(作废)

            //isSuccess = capMemory.AddDelta(capitalDelta);
            //if (!isSuccess)
            //{
            //    //不成功连续执行3次
            //    for (int i = 0; i < 3; i++)
            //    {
            //        //不成功，再执行一次
            //        isSuccess = capMemory.AddDelta(capitalDelta);

            //        if (isSuccess)
            //            break;
            //    }
            //}

            //if (!isSuccess)
            //{
            //    RescueManager.Instance.DoXHReckonCapitalFailureProcess(capitalDelta);
            //}

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
        /// 买清算-委托表处理
        /// </summary>
        /// <param name="dealSum"></param>
        /// <param name="tet2"></param>
        private void HKBuy_InstantReckon_EntrustProcess(HKDealSum dealSum, HK_TodayEntrustInfo tet2)
        {
            //搓合单位
            Types.UnitType utMatch = MCService.GetMatchUnitType(tet2.Code, Types.BreedClassTypeEnum.HKStock);
            //搓合单位 -> 计价单位 倍数
            decimal unitMultiple = MCService.GetTradeUnitScale(Types.BreedClassTypeEnum.HKStock, tet2.Code, utMatch);

            //成交均价（价格是报价单位，数量是撮合单位，计算金额时需要统一，即把撮合单位转为报价单位）
            decimal tradeAveragePrice =
                (tet2.TradeAveragePrice * tet2.TradeAmount * unitMultiple +
                 dealSum.CapitalSum) / (tet2.TradeAmount * unitMultiple + dealSum.AmountSum * unitMultiple);

            tradeAveragePrice = Utils.Round(tradeAveragePrice);

            tet2.TradeAveragePrice = tradeAveragePrice;
            //成交量
            tet2.TradeAmount =
                Convert.ToInt32(tet2.TradeAmount + dealSum.AmountSum);

            SetOrderState(tet2);

            tet2.OrderMessage = "";
        }

        /// <summary>
        /// 买清算-持仓冻结处理
        /// </summary>
        /// <param name="dealSum"></param>
        /// <param name="isFirstHold"></param>
        /// <returns></returns>
        private HK_AcccountHoldFreezeInfo HKBuy_InstantReckon_HoldingFreezeProcess(HKDealSum dealSum,
                                                                                        ref bool isFirstHold)
        {
            HK_AcccountHoldFreezeInfo holdFreeze;
            holdFreeze = HKDataAccess.GetHoldAccountFreeze(EntrustNumber,
                                                           Entity.Contants.Types.FreezeType.DelegateFreeze);
            if (holdFreeze == null)
            {
                holdFreeze = new HK_AcccountHoldFreezeInfo();
                //冻结总量
                holdFreeze.PrepareFreezeAmount = Convert.ToInt32(dealSum.AmountSum);
                //冻结时间
                holdFreeze.FreezeTime = DateTime.Now;
                //解冻时间
                holdFreeze.ThawTime = DateTime.Now.AddDays(HoldingTradingRule);
                //委托单号
                holdFreeze.EntrustNumber = EntrustNumber;
                //冻结类型
                holdFreeze.FreezeTypeID = (int)Entity.Contants.Types.FreezeType.ReckoningFreeze;
                //港股帐户持仓标识
                holdFreeze.AccountHoldLogo = HoldingAccountId;
                isFirstHold = true;
            }
            else
            {
                //之前存在冻结
                //冻结总量
                holdFreeze.PrepareFreezeAmount += Convert.ToInt32(dealSum.AmountSum);
            }
            return holdFreeze;
        }

        /// <summary>
        /// 买清算-资金表及资金冻结表预处理
        /// </summary>
        /// <param name="dealSum"></param>
        /// <param name="caf"></param>
        /// <param name="dealMoneySum"></param>
        private HK_CapitalAccount_DeltaInfo HKBuy_InstantReckon_PreCapitalProcess(HKDealSum dealSum,
                                                                                       HK_CapitalAccountFreezeInfo
                                                                                           caf, decimal dealMoneySum)
        {
            //注意：为了简化处理，在清算期间不再进行冻结资金的补偿计算及多退少补的计算，统一在本次清算结束后
            //对资金冻结表进行检查校验，所以在清算内部，直接在资金冻结表上减去成交金额和成交费用
            caf.FreezeCapitalAmount -= dealSum.CapitalSum;
            caf.FreezeCost -= dealSum.CostSum;


            //******************任何地方执行失败，都需要回滚的操作1********************
            //资金表的总冻结金额减去本次成交的金额和费用和（因为资金冻结里减去了这么多钱）
            HK_CapitalAccount_DeltaInfo capitalDelta = new HK_CapitalAccount_DeltaInfo();
            capitalDelta.FreezeCapitalTotalDelta = -dealMoneySum;
            capitalDelta.CapitalAccountLogo = CapitalAccountId;

            //资金表不马上执行更新，在后面统一更新
            //capMemory.AddDelta(capitalDelta);

            return capitalDelta;
        }

        /// <summary>
        /// 买-持仓表预处理
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="strMessage"></param>
        /// <returns></returns>
        private HKHoldMemoryTable HKBuy_InstantReckon_PreHoldingProcess(ref bool isSuccess, ref string strMessage)
        {
            HKHoldMemoryTable holdMemory = null;
            try
            {
                holdMemory = MemoryDataManager.HKHoldMemoryList.GetByHoldAccountAndCurrencyType(HoldingAccount, Code,
                                                                                                CurrencyType);
                //holdMemory = MemoryDataManager.HKHoldMemoryList.GetByAccountHoldLogoId(HoldingAccountId);
                //如果持仓为空，那么先从数据库加载，看是不是内存表没有加载
                if (holdMemory == null)
                {
                    HK_AccountHoldInfo hold = GetHoldFromDB(HoldingAccount, Code, CurrencyType);
                    //如果数据库有，那么直接加载到内存表中
                    if (hold != null)
                    {
                        string format =
                            "XHBuy_InstantReckon_PreHoldingProcess数据库存在持仓，直接加载到内存表中[Code={0},HoldAccount={1}]";
                        string desc = string.Format(format, Code, HoldingAccount);
                        LogHelper.WriteDebug(desc);

                        isSuccess = MemoryDataManager.HKHoldMemoryList.AddAccountHoldTableToMemory(hold);

                        if (!isSuccess)
                        {
                            isSuccess = MemoryDataManager.HKHoldMemoryList.AddAccountHoldTableToMemory(hold);
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
                        string format =
                            "XHBuy_InstantReckon_PreHoldingProcess数据库不存在持仓，新建一个空的持仓，并加入到数据库和内存表中[Code={0},HoldAccount={1}]";
                        string desc = string.Format(format, Code, HoldingAccount);
                        LogHelper.WriteDebug(desc);

                        hold = new HK_AccountHoldInfo();

                        hold.UserAccountDistributeLogo = HoldingAccount;
                        hold.Code = Code;
                        hold.AvailableAmount = 0;
                        hold.FreezeAmount = 0;
                        hold.CurrencyTypeID = CurrencyType;

                        isSuccess = MemoryDataManager.HKHoldMemoryList.AddAccountHoldTable(hold);

                        if (!isSuccess)
                        {
                            isSuccess = MemoryDataManager.HKHoldMemoryList.AddAccountHoldTable(hold);
                        }

                        if (!isSuccess)
                        {
                            desc += "-新建失败，已存在";
                            LogHelper.WriteDebug(desc);
                        }
                    }

                    holdMemory = MemoryDataManager.HKHoldMemoryList.GetByHoldAccountAndCurrencyType(HoldingAccount, Code,
                                                                                                    CurrencyType);

                    if (holdMemory == null)
                    {
                        int holdId = MemoryDataManager.HKHoldMemoryList.GetAccountHoldLogoId(HoldingAccount, Code,
                                                                                             CurrencyType);
                        if (holdId != -1)
                            holdMemory = MemoryDataManager.HKHoldMemoryList.GetByAccountHoldLogoId(holdId);
                    }

                    if (holdMemory == null)
                    {
                        hold = GetHoldFromDB(HoldingAccount, Code, CurrencyType);
                        isSuccess = MemoryDataManager.HKHoldMemoryList.AddAccountHoldTableToMemory(hold);
                    }

                    holdMemory = MemoryDataManager.HKHoldMemoryList.GetByHoldAccountAndCurrencyType(HoldingAccount, Code,
                                                                                                    CurrencyType);


                    if (holdMemory == null)
                    {
                        strMessage = "GT-2725:[港股成交清算]无法获取持仓记录.";
                        LogHelper.WriteDebug("HKBuy_InstantReckon_PreHoldingProcess" + strMessage);
                        isSuccess = false;
                        return null;
                    }
                }

                HoldingAccountId = holdMemory.Data.AccountHoldLogoID;
                isSuccess = true;
            }
            catch (Exception ex)
            {
                strMessage = "GT-2720:[港股成交清算]持仓处理异常:" + ex;
                LogHelper.WriteError(ex.Message, ex);
            }

            return holdMemory;
        }

        /// <summary>
        /// 从数据库获取持仓信息
        /// </summary>
        /// <param name="holdingAccount"></param>
        /// <param name="code"></param>
        /// <param name="currencyType"></param>
        /// <returns></returns>
        private HK_AccountHoldInfo GetHoldFromDB(string holdingAccount, string code, int currencyType)
        {
            HK_AccountHoldInfo hold = null;

            HK_AccountHoldDal dal = new HK_AccountHoldDal();
            hold = dal.GetHKAccountHoldInfo(holdingAccount, code, currencyType);

            return hold;
        }


        /// <summary>
        /// 买清算-持仓处理
        /// </summary>
        /// <param name="holdMemory"></param>
        /// <param name="dealSum"></param>
        /// <param name="holdingTradingRule"></param>
        private static bool HKBuy_InstantReckon_HoldingProcess(HKHoldMemoryTable holdMemory, HKDealSum dealSum,
                                                               int holdingTradingRule)
        {
            //0813:经过讨论，单位转换按如下标准进行：
            //1.价格相关：以计价单位为准，如成交价，均价，成本价，保本价等等
            //2.持仓相关：以交易单位为准，如成交量，持仓量等等
            //如下委托：
            //债券   委托量=1 委托价=103  成交量=1 成交价103
            //其中委托量、成交量的单位是手，委托价、成交价的单位是张，1手=10张
            //总金额=委托量×委托价 因为量/价的单位不匹配，所以需要进行转换
            //总金额=(1×10)×103=1030，均价= 1030/(1×10)=103，即每张103元
            //保存到数据库中的字段，也是按上述规则保存，即量/价各有不同的单位

            //return Internal_XHBuy_InstantReckon_HoldingProcess(holdMemory, dealSum, holdingTradingRule);

            string code = holdMemory.Data.Code;
            //搓合单位
            Types.UnitType utMatch = MCService.GetMatchUnitType(code, Types.BreedClassTypeEnum.HKStock);
            //搓合单位 -> 计价单位 倍数
            decimal unitMultiple = MCService.GetTradeUnitScale(Types.BreedClassTypeEnum.HKStock, code, utMatch);

            bool isSuccess = holdMemory.ReadAndWrite(hold =>
                                                         {
                                                             decimal holdAmount = hold.AvailableAmount +
                                                                                  hold.FreezeAmount;
                                                             decimal costPrice = (holdAmount * hold.CostPrice * unitMultiple +
                                                                                  dealSum.CapitalSum + dealSum.CostSum) /
                                                                                 (holdAmount * unitMultiple +
                                                                                  dealSum.AmountSum * unitMultiple);
                                                             //四舍五入
                                                             costPrice = Utils.Round(costPrice);

                                                             decimal holdPrice = MCService.GetHoldPrice(code, costPrice,
                                                                                                        (holdAmount *
                                                                                                         unitMultiple +
                                                                                                         dealSum.
                                                                                                             AmountSum *
                                                                                                         unitMultiple));
                                                             //四舍五入
                                                             holdPrice = Utils.Round(holdPrice);

                                                             hold.CostPrice = costPrice;
                                                             hold.BreakevenPrice = holdPrice;

                                                             //持仓均价=（持仓均价*持仓量+买入成交量*买入价）/(持仓量+买入成交量)
                                                             decimal holdAveragePrice = (hold.HoldAveragePrice *
                                                                                         holdAmount * unitMultiple +
                                                                                         dealSum.CapitalSum) /
                                                                                        (holdAmount * unitMultiple +
                                                                                         dealSum.AmountSum * unitMultiple);
                                                             //四舍五入
                                                             holdAveragePrice = Utils.Round(holdAveragePrice);
                                                             hold.HoldAveragePrice = holdAveragePrice;

                                                             //T+0
                                                             if (holdingTradingRule == 0)
                                                             {
                                                                 hold.AvailableAmount += dealSum.AmountSum;
                                                             }
                                                             else
                                                             {
                                                                 hold.FreezeAmount += dealSum.AmountSum;
                                                             }

                                                             return hold;
                                                         });
            return isSuccess;
        }

        /// <summary>
        /// 供RescueManager恢复现场买持仓操作时调用
        /// </summary>
        /// <param name="holdingAccoutnId"></param>
        /// <param name="dealSum"></param>
        /// <returns></returns>
        public static bool DoHKBuy_HoldingRescue(int holdingAccoutnId, HKDealSum dealSum, int holdingTradingRule)
        {
            HKHoldMemoryTable holdMemory = MemoryDataManager.HKHoldMemoryList.GetByAccountHoldLogoId(holdingAccoutnId);
            if (holdMemory == null)
            {
                holdMemory = HKCommonLogic.GetHoldMemoryTable(holdingAccoutnId);
            }

            if (holdMemory == null)
            {
                return true;
            }

            return HKBuy_InstantReckon_HoldingProcess(holdMemory, dealSum, holdingTradingRule);
        }

        #endregion

        #region 港股卖清算处理

        /// <summary>
        /// 港股卖清算提交入口
        /// </summary>
        /// <param name="idoExList">扩展成交回报列表</param>
        /// <param name="dealSum">港股成交回报汇总</param>
        /// <param name="strMessage">错误信息</param>
        /// <param name="tet">委托</param>
        /// <param name="tradeList">成交列表</param>
        /// <returns>清算是否成功</returns>
        private bool HKSell_InstantReckon_Deal(List<HKDealBackEntityEx> idoExList, HKDealSum dealSum,
                                               ref string strMessage, out HK_TodayEntrustInfo tet,
                                               out List<HK_TodayTradeInfo> tradeList)
        {
            //卖清算：
            //1.资金及冻结处理
            //(1) 根据成交金额和费用，从资金冻结记录减去相应的值，并从资金表总冻结资金中减去;
            //(2) 当冻结记录里钱不够扣时，直接从资金表可用资金中扣，即“少了要补”；
            //(3) 当最后一次清算时，要把冻结记录清零，多出的钱要还到资金表中，即“多了要退”并且总冻结资金要减去这部分钱;
            //    (4)根据资金交割规则：
            //a.T+0: 成交金额加入资金表可用资金；
            //        b.T+N:生成一条资金冻结记录，并且在资金表总冻结记录中加上。
            //    (5)将本次清算的已实现盈亏加入到资金表累计已实现盈亏中。
            //2.持仓及冻结处理
            //    将成交量从持仓表的总冻结量上减去，并且还要从持仓冻结中减去。
            //3.委托表处理
            //    (1)更新各种状态（成交量，状态等）；
            //    (2)将本次清算的已实现盈亏加入到已实现盈亏中


            tet = null;
            tradeList = new List<HK_TodayTradeInfo>();

            #region 1.资金表预处理

            //先检查资金冻结表中的冻结金额和费用是否够扣
            HK_CapitalAccountFreezeInfo caf;
            if (!GetCapitalFreezeTable(out caf))
            {
                strMessage = "GT-2724:[港股成交清算]无法获取资金冻结记录.";

                return false;
            }

            var capMemory = MemoryDataManager.HKCapitalMemoryList.GetByCapitalAccountLogo(CapitalAccountId);
            if (capMemory == null)
            {
                strMessage = "GT-2721:[港股成交清算]资金帐户不存在:" + CapitalAccount;

                return false;
            }

            //注意：为了简化处理，在清算期间不再进行冻结资金的补偿计算及多退少补的计算，统一在本次清算结束后
            //对资金冻结表进行检查校验，所以在清算内部，直接在资金冻结表上减去成交金额和成交费用
            HK_CapitalAccount_DeltaInfo capitalDelta = HKSell_InstantReckon_PreCapitalProcess(caf, dealSum);
            capitalDelta.CapitalAccountLogo = capMemory.Data.CapitalAccountLogo;

            #endregion

            #region 2.持仓表预处理

            bool isSuccess = false;
            HKHoldMemoryTable holdMemory = null;
            holdMemory = HKSell_InstantReckon_PreHoldingProcess(holdMemory, ref isSuccess, ref strMessage);

            if (!isSuccess)
            {
                strMessage = "GT-2790:[港股成交清算]无法获取持仓记录.";
                LogHelper.WriteDebug("XHSell_InstantReckon_Deal_!isSuccess " + strMessage);

                return false;
            }

            if (holdMemory == null)
            {
                strMessage = "GT-2790:[港股成交清算]无法获取持仓记录.";
                LogHelper.WriteDebug("XHSell_InstantReckon_Deal_holdMemory == null " + strMessage);

                return false;
            }

            decimal holdAveragePrice = holdMemory.Data.HoldAveragePrice;

            HK_AccountHoldInfo_Delta holdDelta = new HK_AccountHoldInfo_Delta();
            var holdData = holdMemory.Data;

            string holdFormat =
                "XHSell_InstantReckon_Deal获取持仓均价[AccountHoldLogoId={0},UserAccountDistributeLogo={1},Code={2},HoldAveragePrice={3}]";
            string holdDesc = string.Format(holdFormat, holdData.AccountHoldLogoID, holdData.UserAccountDistributeLogo,
                                            holdData.Code,
                                            holdData.HoldAveragePrice);
            LogHelper.WriteInfo(holdDesc);

            holdDelta.AccountHoldLogoId = holdData.AccountHoldLogoID;
            holdDelta.FreezeAmountDelta -= dealSum.AmountSum;

            #endregion

            #region 3.资金冻结和持仓冻结处理

            //处理资金冻结和持仓冻结,以及委托信息
            HK_AcccountHoldFreezeInfo holdFreeze = null;

            holdFreeze = HKDataAccess.GetHoldAccountFreeze(EntrustNumber,
                                                           Entity.Contants.Types.FreezeType.DelegateFreeze);
            if (holdFreeze == null)
            {
                strMessage = "GT-2789:[港股成交清算]无法获取持仓冻结记录.";
                LogHelper.WriteInfo(strMessage);
                //持仓冻结不存在，一样运行撤单，即不再处理持仓冻结表，只对持仓表的冻结做处理
                //return false;
            }
            else
            {
                holdFreeze.PrepareFreezeAmount -= Convert.ToInt32(dealSum.AmountSum);
                if (holdFreeze.PrepareFreezeAmount < 0)
                {
                    holdFreeze.PrepareFreezeAmount = 0;
                }
            }


            tet = HKDataAccess.GetTodayEntrustTable(EntrustNumber);
            var tet2 = tet;

            decimal hasDoneProfit = HKSell_InstantReckon_EntrustProcess(dealSum, tet2, holdAveragePrice);

            //在港股资金表中增加累计已实现盈亏的字段
            capitalDelta.HasDoneProfitLossTotalDelta = hasDoneProfit;

            var tradeList2 = tradeList;
            List<string> ids = GetIDList2(idoExList);

            #endregion

            #region 4.数据库提交动作

            isSuccess = false;
            Database database = DatabaseFactory.CreateDatabase();
            try
            {
                using (DbConnection connection = database.CreateConnection())
                {
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();
                    ReckoningTransaction tm = new ReckoningTransaction();
                    tm.Database = database;
                    tm.Transaction = transaction;
                    try
                    {
                        //非T+0时新增资金冻结
                        HKSell_InstantReckon_CapitalFreezeProcess(tet2, dealSum, tm);

                        //1.更新资金冻结
                        HK_CapitalAccountFreezeDal cafDal =
                            new HK_CapitalAccountFreezeDal();
                        cafDal.Update(caf, tm);

                        //2.更新持仓冻结
                        if (holdFreeze != null)
                        {
                            HK_AcccountHoldFreezeDal holdDal = new HK_AcccountHoldFreezeDal();
                            holdDal.Update(holdFreeze, tm.Database, tm.Transaction);
                        }

                        //3.更新委托表
                        HKDataAccess.UpdateEntrustTable(tet2, tm.Database,
                                                        tm.Transaction);

                        //4.生成成交记录
                        BuildTradeList(tet2, tm, idoExList, tradeList2);

                        //5.资金表操作
                        capMemory.AddDeltaToDB(capitalDelta, tm.Database, tm.Transaction);

                        //6.持仓表操作
                        holdMemory.AddDeltaToDB(holdDelta, tm.Database, tm.Transaction);

                        //7.清算完成后清除数据库中保存的成交回报
                        CrashManager.GetInstance().DeleteEntityList(ids, tm);

                        transaction.Commit();
                        isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogHelper.WriteError(ex.Message, ex);
                        strMessage = "GT-2722:[港股成交清算]数据库提交失败";

                        isSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                strMessage = "GT-2722:[港股成交清算]数据库提交失败";
            }

            if (isSuccess)
            {
                capMemory.AddDeltaToMemory(capitalDelta);
                holdMemory.AddDeltaToMemory(holdDelta);
                holdMemory.ReadAndWrite(h =>
                                            {
                                                if (h.FreezeAmount < 0)
                                                {
                                                    h.FreezeAmount = 0;
                                                }

                                                return h;
                                            });
            }
            else
            {
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
        /// 卖清算-资金冻结处理
        /// </summary>
        /// <param name="tet2"></param>
        /// <param name="dealSum"></param>
        /// <param name="tm"></param>
        private void HKSell_InstantReckon_CapitalFreezeProcess(HK_TodayEntrustInfo tet2, HKDealSum dealSum,
                                                               ReckoningTransaction tm)
        {
            //1.非T+0时新增资金冻结
            if (CaptitalTradingRule != 0)
            {
                HK_CapitalAccountFreezeDal cafDal =
                    new HK_CapitalAccountFreezeDal();

                //当前成交金额冻结
                var newCaft = new HK_CapitalAccountFreezeInfo();
                //委托单号
                newCaft.EntrustNumber = tet2.EntrustNumber;
                //冻结 预成交金额
                newCaft.FreezeCapitalAmount = dealSum.CapitalSum;
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

                cafDal.AddRecord(newCaft, tm.Database, tm.Transaction);
            }
        }

        /// <summary>
        /// 卖清算-委托处理
        /// </summary>
        /// <param name="dealSum"></param>
        /// <param name="tet2"></param>
        /// <param name="holdAveragePrice"></param>
        /// <returns></returns>
        private decimal HKSell_InstantReckon_EntrustProcess(HKDealSum dealSum, HK_TodayEntrustInfo tet2,
                                                            decimal holdAveragePrice)
        {
            //搓合单位
            Types.UnitType utMatch = MCService.GetMatchUnitType(tet2.Code, Types.BreedClassTypeEnum.HKStock);
            //搓合单位 -> 计价单位 倍数
            decimal unitMultiple = MCService.GetTradeUnitScale(Types.BreedClassTypeEnum.HKStock, tet2.Code, utMatch);

            //成交均价（价格是报价单位，数量是撮合单位，计算金额时需要统一，即把撮合单位转为报价单位）
            decimal tradeAveragePrice =
                (tet2.TradeAveragePrice * tet2.TradeAmount * unitMultiple +
                 dealSum.CapitalSum) /
                (tet2.TradeAmount * unitMultiple + dealSum.AmountSum * unitMultiple);
            //四舍五入
            tradeAveragePrice = Utils.Round(tradeAveragePrice);
            tet2.TradeAveragePrice = tradeAveragePrice;

            //成交量
            tet2.TradeAmount =
                Convert.ToInt32(tet2.TradeAmount + dealSum.AmountSum);

            //已实现盈亏=（卖出价-持仓均价）*卖出量
            decimal hasDoneProfit = dealSum.CapitalSum -
                                    holdAveragePrice * dealSum.AmountSum * unitMultiple;
            //四舍五入
            hasDoneProfit = Utils.Round(hasDoneProfit);

            string format =
                "XHSell_InstantReckon_EntrustProcess港股已实现盈亏[EntrustNumber={0},Code={1}][已实现盈亏{2}=成交金额{3}-持仓均价{4}*成交量{5}即({6})]";
            string desc = string.Format(format, tet2.EntrustNumber, tet2.Code, hasDoneProfit, dealSum.CapitalSum,
                                        holdAveragePrice, dealSum.AmountSum * unitMultiple, holdAveragePrice * dealSum.AmountSum * unitMultiple);
            LogHelper.WriteInfo(desc);

            tet2.HasDoneProfit += hasDoneProfit;


            SetOrderState(tet2);
            tet2.OrderMessage = "";

            return hasDoneProfit;
        }

        /// <summary>
        /// 卖清算-资金表及资金冻结表预处理
        /// </summary>
        /// <param name="caf"></param>
        /// <param name="dealSum"></param>
        /// <returns></returns>
        private HK_CapitalAccount_DeltaInfo HKSell_InstantReckon_PreCapitalProcess(
            HK_CapitalAccountFreezeInfo caf, HKDealSum dealSum)
        {
            //caf.FreezeCapitalAmount -= 0; 卖不会冻结资金
            caf.FreezeCost -= dealSum.CostSum;

            //T+0:资金表的总冻结金额减去本次成交的费用和（因为资金冻结里减去了这么多钱）,可用资金加上本次成交金额
            HK_CapitalAccount_DeltaInfo capitalDelta = new HK_CapitalAccount_DeltaInfo();
            capitalDelta.CapitalAccountLogo = CapitalAccountId;
            if (CaptitalTradingRule == 0)
            {
                capitalDelta.AvailableCapitalDelta = dealSum.CapitalSum;
                capitalDelta.FreezeCapitalTotalDelta = -dealSum.CostSum;
                //capMemory.AddDelta(dealSum.CapitalSum, -dealSum.CostSum, 0, 0);
            }
            else
            {
                //T+N:生成资金冻结记录，资金表的总冻结金额要【+成交金额，-成交费用】
                capitalDelta.FreezeCapitalTotalDelta = dealSum.CapitalSum - dealSum.CostSum;
                //capMemory.AddDelta(0, dealSum.CapitalSum - dealSum.CostSum, 0, 0);
            }

            return capitalDelta;
        }

        /// <summary>
        /// 卖-持仓表预处理
        /// </summary>
        /// <param name="holdMemory"></param>
        /// <param name="isSuccess"></param>
        /// <param name="strMessage"></param>
        /// <returns></returns>
        private HKHoldMemoryTable HKSell_InstantReckon_PreHoldingProcess(HKHoldMemoryTable holdMemory,
                                                                         ref bool isSuccess,
                                                                         ref string strMessage)
        {
            try
            {
                //holdMemory = MemoryDataManager.HKHoldMemoryList.GetByHoldAccountAndCurrencyType(HoldingAccount, Code,
                //                                                                                CurrencyType);
                holdMemory = MemoryDataManager.HKHoldMemoryList.GetByAccountHoldLogoId(HoldingAccountId);
                //如果持仓为空，那么先从数据库加载，看是不是内存表没有加载
                if (holdMemory == null)
                {
                    HK_AccountHoldInfo hold = GetHoldFromDB(HoldingAccount, Code, CurrencyType);
                    //如果数据库有，那么直接加载到内存表中
                    if (hold != null)
                    {
                        isSuccess = MemoryDataManager.HKHoldMemoryList.AddAccountHoldTableToMemory(hold);
                    }
                    else
                    {
                        //如果数据库也没有，那么代表无持仓
                        return null;
                    }

                    holdMemory = MemoryDataManager.HKHoldMemoryList.GetByHoldAccountAndCurrencyType(HoldingAccount, Code,
                                                                                                    CurrencyType);

                    if (holdMemory == null)
                    {
                        strMessage = "GT-2725:[港股成交清算]无法获取持仓记录.";
                        LogHelper.WriteDebug("HKSell_InstantReckon_PreHoldingProcess-" + strMessage);
                        isSuccess = false;
                        return null;
                    }
                }

                HoldingAccountId = holdMemory.Data.AccountHoldLogoID;
                isSuccess = true;
            }
            catch (Exception ex)
            {
                strMessage = "GT-2720:[港股成交清算]持仓处理异常:" + ex;
                LogHelper.WriteError("HKSell_InstantReckon_PreHoldingProcess-" + ex.Message, ex);
            }

            return holdMemory;
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
        private void BuildTradeList(HK_TodayEntrustInfo tet2, ReckoningTransaction tm,
                                    List<HKDealBackEntityEx> idoExList, List<HK_TodayTradeInfo> tradeList2)
        {
            foreach (var idoEx in idoExList)
            {
                var ido = idoEx.DealBack;
                var costResult = idoEx.CostResult;

                var trade = HKCommonLogic.BuildHKDealRpt(tet2, ido,
                                                         costResult,
                                                         idoEx.DealCapital,
                                                         tm);
                if (trade != null)
                    tradeList2.Add(trade);
            }
        }

        private bool GetCapitalFreezeTable(out HK_CapitalAccountFreezeInfo caf)
        {
            caf = HKDataAccess.GetCapitalAccountFreeze(EntrustNumber,
                                                       Entity.Contants.Types.FreezeType.DelegateFreeze);
            if (caf == null)
            {
                //那么认为冻结的金额和费用都已经为0，插入一条空的冻结记录以供后面使用
                try
                {
                    HKCommonLogic.InsertNullCapitalFreeze(CapitalAccountId, EntrustNumber);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                    return false;
                }

                caf = HKDataAccess.GetCapitalAccountFreeze(EntrustNumber,
                                                           Entity.Contants.Types.FreezeType.DelegateFreeze);
            }
            return true;
        }

        /// <summary>
        /// 获取成交回报列表所有的ID
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<string> GetIdList(List<HKDealBackEntity> list)
        {
            var ids = new List<string>();
            foreach (var ido in list)
            {
                ids.Add(ido.ID);
            }
            return ids;
        }

        /// <summary>
        /// 计算港股成交回报汇总信息
        /// </summary>
        /// <returns>港股成交回报汇总对象</returns>
        private bool ComputeHKDealBacks(List<HKDealBackEntity> idos, out List<HKDealBackEntityEx> idoexs,
                                        out HKDealSum dealSum, ref string strMessage)
        {
            idoexs = new List<HKDealBackEntityEx>();
            dealSum = new HKDealSum();
            foreach (var ido in idos)
            {
                HKDealBackEntityEx ex = new HKDealBackEntityEx(ido);
                HKCostResult costResult = null;
                decimal dealCapital = 0;
                decimal dealCost = 0;
                bool isSuccess = ComputePreprocCapital(ido, Code, BuySellType, ref strMessage, ref dealCapital,
                                                       ref dealCost, out costResult);

                if (!isSuccess)
                    return false;

                ex.CostResult = costResult;
                ex.DealCapital = dealCapital;
                ex.DealCost = dealCost;

                dealSum.AmountSum += ido.DealAmount;
                dealSum.CapitalSum += dealCapital;
                dealSum.CostSum += dealCost;

                idoexs.Add(ex);
            }

            return true;
        }

        //设置委托状态
        private void SetOrderState(HK_TodayEntrustInfo tet)
        {
            if (tet.OrderStatusID == (int)Entity.Contants.Types.OrderStateType.DOSCanceled)
                return;

            //错误价格的废单
            if (tet.CancelAmount == -1)
            {
                tet.OrderStatusID = (int)Entity.Contants.Types.OrderStateType.DOSCanceled;
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
                        tet.OrderStatusID = (int)Entity.Contants.Types.OrderStateType.DOSRemoved;
                    }
                    else
                    {
                        tet.OrderStatusID = (int)Entity.Contants.Types.OrderStateType.DOSPartRemoved;
                    }
                }
                //否则是没有发生撤单，已成   
                else
                {
                    tet.OrderStatusID = (int)Entity.Contants.Types.OrderStateType.DOSDealed;
                }
            }
            //委托量>成交量+撤单量 部分成交
            else
            {
                //如果还在成交的过程中，收到撤单的命令，那么就保持部成待撤状态，直到最后变成已成或已撤
                if (tet.OrderStatusID == (int)Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon)
                    return;

                //否则不管有没有撤单，均统一设置为部成（因为撮合可能先返回撤单，所以此处状态不好设置）
                if (tet.TradeAmount > 0)
                    tet.OrderStatusID = (int)Entity.Contants.Types.OrderStateType.DOSPartDealed;
            }
        }

        private List<string> GetIDList2(List<HKDealBackEntityEx> list)
        {
            List<string> ids = new List<string>();
            foreach (var entityEx in list)
            {
                ids.Add(entityEx.DealBack.ID);
            }

            return ids;
        }

        private HK_AccountHoldInfo GetCloneHold(HK_AccountHoldInfo info)
        {
            HK_AccountHoldInfo newInfo = new HK_AccountHoldInfo();
            newInfo.AccountHoldLogoID = info.AccountHoldLogoID;
            newInfo.AvailableAmount = info.AvailableAmount;
            newInfo.BreakevenPrice = info.BreakevenPrice;
            newInfo.Code = info.Code;
            newInfo.CostPrice = info.CostPrice;
            newInfo.CurrencyTypeID = info.CurrencyTypeID;
            newInfo.FreezeAmount = info.FreezeAmount;
            newInfo.HoldAveragePrice = info.HoldAveragePrice;
            newInfo.UserAccountDistributeLogo = info.UserAccountDistributeLogo;

            return newInfo;
        }

        #endregion
    }

    /// <summary>
    /// 港股成交回报扩展类
    /// </summary>
    public class HKDealBackEntityEx : ReckonDealBackEx<HKDealBackEntity, HKCostResult>
    {
        public HKDealBackEntityEx(HKDealBackEntity dealBack)
            : base(dealBack)
        {
        }
    }
}