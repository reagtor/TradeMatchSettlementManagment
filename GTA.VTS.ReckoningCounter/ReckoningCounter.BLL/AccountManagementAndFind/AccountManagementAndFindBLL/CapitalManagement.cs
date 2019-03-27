using System;
using System.Collections.Generic;
using System.Text;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.DAL.AccountManagementAndFindDAL;
using ReckoningCounter.Entity;
using ReckoningCounter.DAL;
using GTA.VTS.Common.CommonObject;
using ReckoningCounter.Entity.AccountManagementAndFindEntity;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.Model;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using ReckoningCounter.MemoryData;
using ReckoningCounter.MemoryData.XH.Capital;
using ReckoningCounter.MemoryData.QH.Capital;
using ReckoningCounter.MemoryData.HK.Capital;
using ReckoningCounter.MemoryData.XH.Hold;
using ReckoningCounter.MemoryData.QH.Hold;
using ReckoningCounter.MemoryData.HK.Hold;
using ReckoningCounter.DAL.Data.HK;
using ReckoningCounter.Entity.Model.HK;

namespace ReckoningCounter.BLL.AccountManagementAndFind.AccountManagementAndFindBLL
{
    /// <summary>
    /// 作用：柜台资金管理（包括： 管理员给交易员追加资金、交易员的几个资金账户之间同币种的两两自由转账）
    /// 作者：李科恒
    /// 日期：2008-10-30
    /// Update By:李健华
    /// Update Date:2009-07-12
    /// Desc:修改之前的DaL操作以及相关的业务逻辑BUG
    /// Update by:董鹏
    /// Update date:2009-12-23
    /// Desc: 添加管理员个性化资金设置的方法，
    ///        添加管理员清空试玩数据的方法
    /// Update by:董鹏
    /// Update date:2010-02-02
    /// Desc: 修改个性化资金、清空试玩数据，增加了商品期货的实现。
    /// </summary>
    public class CapitalManagementBLL
    {

        # region (NEW)管理员给交易员追加资金
        /// <summary>
        /// 管理员给交易员追加资金 
        /// </summary>
        /// <param name="addCapital">追加资金实体</param>
        /// <param name="outMessage">追加异常信息</param>
        /// <returns></returns>
        public bool AddCapital(AddCapitalEntity addCapital, out string outMessage)
        {
            var _TraderAccountManagement = new TraderAccountManagement();
            return _TraderAccountManagement.AddCapital(addCapital, out outMessage);
        }
        # endregion 管理员给交易员追加资金

        # region (NEW)交易员的几个资金账户之间同币种两两自由转账
        /// <summary>
        /// 交易员的几个资金账户之间两两自由转账（同币种）
        /// Update BY:李健华
        /// Update Date:2009-07-23
        /// Desc.:为了从内存表中获取数据把DAL以前相关方法移动到本类中
        /// </summary>
        /// <param name="freeTransfer"></param>
        /// <param name="currencyType"></param>
        /// <param name="outMessage"></param>
        /// <returns></returns>
        public bool TwoAccountsFreeTransferFunds(FreeTransferEntity freeTransfer, Types.CurrencyType currencyType, out string outMessage)
        {
            outMessage = string.Empty;
            try
            {
                if (!CommonDataAgent.AccountTransferEntityChange(ref freeTransfer, ref outMessage))
                    return false;

                return AccountsFreeTransfer(freeTransfer, currencyType, out outMessage);
            }
            catch (Exception ex)
            {
                outMessage = ex.Message;
                LogHelper.WriteError(outMessage, ex);
                return false;
            }

        }

        # endregion 交易员的几个资金账户之间同币种两两自由转账

        #region  资金转帐 为了从内存表中获取数据把DAL以前相关方法移动到本类中 2009-07-23 李健华

        #region 资金转帐
        /// <summary>
        /// Title:资金转帐 
        ///  Create By:李健华
        ///  Create Date:2009-07-23
        /// Desc.:为了从内存表中获取数据
        /// * 把ReckoningCounter.DAL.AccountManagementAndFindDAL.TraderAccountManagement
        /// * 中相关方法移动到本类中 
        /// </summary>
        /// <param name="freeTransfer">自由转账实体</param>
        /// <param name="currencyType">币种</param>
        /// <param name="outMessage"></param>
        /// <returns></returns>
        public bool AccountsFreeTransfer(FreeTransferEntity freeTransfer, Types.CurrencyType currencyType, out string outMessage)
        {
            outMessage = "";

            #region 在前面已经有作判断 判断转出/转入资金账户是否为可用
            //try
            //{

            //    UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();

            //    FromAccount = dal.GetModel(freeTransfer.FromCapitalAccount);

            //    if (FromAccount.WhetherAvailable == false)
            //    {
            //        outMessage = "转出资金帐户被冻结，不能转帐";
            //        return false;
            //    }
            //    UA_UserAccountAllocationTableInfo ToAccount = dal.GetModel(freeTransfer.ToCapitalAccount);
            //    //UaUserAccountAllocationTable ToAccount = _UserAccountAllocation.GetByUserAccountDistributeLogo(tm, freeTransfer.ToCapitalAccount);
            //    if (ToAccount.WhetherAvailable == false)
            //    {
            //        outMessage = "转入资金帐户被冻结，不能转帐";
            //        return false;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    outMessage = "转账失败！失败原因为：" + ex.Message.ToString();
            //    LogHelper.WriteError(outMessage, ex);
            //    return false;
            //}
            #endregion

            Database db = DatabaseFactory.CreateDatabase();
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                DbTransaction trm = conn.BeginTransaction();
                try
                {
                    #region 转出帐号处理
                    if (!WriteCapitalTable((int)freeTransfer.FromCapitalAccountType, freeTransfer.FromCapitalAccount,
                                           (int)currencyType, db, trm, 1, freeTransfer.TransferAmount, out outMessage))
                    {
                        trm.Rollback();
                        return false;
                    }
                    #endregion

                    #region  转入帐号处理
                    if (!WriteCapitalTable((int)freeTransfer.ToCapitalAccountType, freeTransfer.ToCapitalAccount,
                                           (int)currencyType, db, trm, 2, freeTransfer.TransferAmount, out outMessage))
                    {
                        trm.Rollback();
                        return false;
                    }
                    #endregion

                    #region 资金流水表记录
                    UA_CapitalFlowTableDal _UaCapitalFlow = new UA_CapitalFlowTableDal();
                    //var _UaCapitalFlow = new SqlUaCapitalFlowTableProvider(TransactionFactory.RC_ConnectionString, true, "");
                    UA_CapitalFlowTableInfo _UaCapitalFlowTable = new UA_CapitalFlowTableInfo();
                    _UaCapitalFlowTable.TradeCurrencyType = (int)currencyType;
                    _UaCapitalFlowTable.FromCapitalAccount = freeTransfer.FromCapitalAccount;
                    _UaCapitalFlowTable.ToCapitalAccount = freeTransfer.ToCapitalAccount;
                    _UaCapitalFlowTable.TransferAmount = freeTransfer.TransferAmount;
                    _UaCapitalFlowTable.TransferTime = System.DateTime.Now;
                    _UaCapitalFlowTable.TransferTypeLogo = (int)ReckoningCounter.Entity.Contants.Types.TransferType.FreeTransfer;
                    _UaCapitalFlow.Add(_UaCapitalFlowTable, db, trm);
                    //outMessage = "记录资金流水失败";
                    //if (!_UaCapitalFlow.Insert(tm, _UaCapitalFlowTable))
                    //{
                    //    outMessage = "记录资金流水失败";
                    //    return false;
                    //} 
                    #endregion
                    trm.Commit();
                }
                catch (Exception ex)
                {
                    trm.Rollback();
                    outMessage = "转账失败！失败原因为：" + ex.Message.ToString();
                    LogHelper.WriteError(outMessage, ex);
                    return false;
                }
                finally
                {
                    trm.Dispose();
                }
            }
            #region 同步内存数据
            if (!UpdateMemoryCapital((int)freeTransfer.FromCapitalAccountType, freeTransfer.FromCapitalAccount, (int)currencyType, 1, freeTransfer.TransferAmount, out outMessage))
            {
                return false;
            }
            if (!UpdateMemoryCapital((int)freeTransfer.ToCapitalAccountType, freeTransfer.ToCapitalAccount, (int)currencyType, 2, freeTransfer.TransferAmount, out outMessage))
            {
                return false;
            }
            #endregion
            outMessage = "";
            return true;

        }
        #endregion

        #region 转帐资金表操作成功后更新内存表缓存数据
        /// <summary>
        /// 资金转帐操作成功后并列新内存相关数据
        /// Title:资金转帐同步内存缓存数据 
        ///  Create By:李健华
        ///  Create Date:2009-07-23
        /// Desc.:为了从内存表中获取数据
        /// </summary>
        /// <param name="CapitalAccountType"> 转出资金账户类型（注：1-银行帐号,2-证券资金帐户,3-证券持仓帐户,4-商品期货资金帐户,5-商品期货持仓帐户,6-股指期货资金帐号,7-股指期货持仓帐户,8-港股资金帐户,9-港股持仓帐户）</param>
        /// <param name="CapitalAccount">资金帐号</param>
        /// <param name="currencyType">币种类型</param>
        /// <param name="FromOrTo">转出OR转入 1为转出 2为转入</param>
        /// <param name="TransferAmount">转帐金额</param>
        /// <param name="outMessage">消息</param>
        /// <returns></returns>
        private bool UpdateMemoryCapital(int CapitalAccountType, string CapitalAccount, int currencyType, int FromOrTo, decimal TransferAmount, out string outMessage)
        {
            outMessage = "";
            string format = "资金帐号所属类型={0},资金账号={1},币种={2},转出或者转入={3},转账金额={4}";
            string msg = "";
            bool isUpdate = true;
            try
            {
                switch ((Types.AccountType)CapitalAccountType)
                {
                    case Types.AccountType.BankAccount:
                        break;
                    case Types.AccountType.StockSpotCapital:
                        #region case 2 --现货资金账户
                        #region 改由内存表取数据
                        var xhManager = MemoryDataManager.XHCapitalMemoryList;
                        if (xhManager == null)
                        {
                            outMessage = "缓存现货资金表还没有初始化数据！";
                            isUpdate = false;
                        }
                        var _XhFindMemoryData = xhManager.GetByCapitalAccountAndCurrencyType(CapitalAccount, currencyType);
                        if (_XhFindMemoryData == null)
                        {
                            outMessage = "无法从缓存表中获取现货资金表类型资金帐号";
                            isUpdate = false;
                        }
                        var _XhFindResult = _XhFindMemoryData.Data;

                        if (_XhFindResult == null)
                        {
                            outMessage = "现货资金表中不存在该类型资金帐号";
                            isUpdate = false;
                        }
                        if (FromOrTo == 1)
                        {
                            if (_XhFindResult.AvailableCapital < TransferAmount)
                            {
                                outMessage = "现货资金表可用资金不足";
                                isUpdate = false;
                            }
                        }
                        #endregion

                        #region 组装相关数据并提交到内存中更新
                        if (isUpdate)
                        {
                            XH_CapitalAccountTable_DeltaInfo xh_Account_deltInfo = new XH_CapitalAccountTable_DeltaInfo();
                            xh_Account_deltInfo.CapitalAccountLogo = _XhFindMemoryData.Data.CapitalAccountLogo;
                            xh_Account_deltInfo.DeltaTime = DateTime.Now;
                            xh_Account_deltInfo.FreezeCapitalTotalDelta = 0;
                            xh_Account_deltInfo.HasDoneProfitLossTotalDelta = 0;
                            if (FromOrTo == 1)
                            {
                                xh_Account_deltInfo.AvailableCapitalDelta = -TransferAmount;
                                xh_Account_deltInfo.TodayOutInCapital = -TransferAmount;
                            }
                            else
                            {
                                xh_Account_deltInfo.AvailableCapitalDelta = TransferAmount;
                                xh_Account_deltInfo.TodayOutInCapital = TransferAmount;
                            }
                            _XhFindMemoryData.AddDeltaToMemory(xh_Account_deltInfo);
                        }
                        #endregion
                        #endregion
                        break;
                    case Types.AccountType.StockSpotHoldCode:
                        break;

                    case Types.AccountType.CommodityFuturesHoldCode:
                        break;
                    case Types.AccountType.CommodityFuturesCapital:
                    case Types.AccountType.StockFuturesCapital:
                        #region case 6--期货资金账户

                        #region 改由内存表取数据
                        var qhManager = MemoryDataManager.QHCapitalMemoryList;
                        if (qhManager == null)
                        {
                            outMessage = "缓存期货资金表还没有初始化数据！";
                            isUpdate = false;
                        }
                        var _QHFindResultData = qhManager.GetByCapitalAccountAndCurrencyType(CapitalAccount, currencyType);
                        if (_QHFindResultData == null)
                        {
                            outMessage = "无法从缓存表中获取期货资金表类型资金帐号";
                            isUpdate = false;
                        }
                        var _QHFindResult = _QHFindResultData.Data;

                        if (_QHFindResult == null)
                        {
                            outMessage = "期货资金表中不存在该类型资金帐号";
                            isUpdate = false;
                        }
                        if (FromOrTo == 1)
                        {
                            if (_QHFindResult.AvailableCapital < TransferAmount)
                            {
                                outMessage = "期货资金表可用资金不足";
                                isUpdate = false;
                            }
                        }
                        #endregion

                        #region 组装相关数据并提交到内存中更新
                        if (isUpdate)
                        {
                            QH_CapitalAccountTable_DeltaInfo qh_Account_deltInfo = new QH_CapitalAccountTable_DeltaInfo();
                            qh_Account_deltInfo.CapitalAccountLogoId = _QHFindResultData.Data.CapitalAccountLogoId;
                            qh_Account_deltInfo.DeltaTime = DateTime.Now;
                            qh_Account_deltInfo.FreezeCapitalTotalDelta = 0;
                            qh_Account_deltInfo.CloseFloatProfitLossTotalDelta = 0;
                            qh_Account_deltInfo.CloseMarketProfitLossTotalDelta = 0;
                            qh_Account_deltInfo.MarginTotalDelta = 0;
                            if (FromOrTo == 1)
                            {
                                qh_Account_deltInfo.AvailableCapitalDelta = -TransferAmount;
                                qh_Account_deltInfo.TodayOutInCapitalDelta = -TransferAmount;
                            }
                            else
                            {
                                qh_Account_deltInfo.AvailableCapitalDelta = TransferAmount;
                                qh_Account_deltInfo.TodayOutInCapitalDelta = TransferAmount;
                            }
                            _QHFindResultData.AddDeltaToMemory(qh_Account_deltInfo);
                        }
                        #endregion
                        break;
                        #endregion
                    case Types.AccountType.StockFuturesHoldCode:
                        break;
                    case Types.AccountType.HKSpotCapital:
                        #region case 8--港股资金账户
                        #region 改由内存表取数据
                        var hkManager = MemoryDataManager.HKCapitalMemoryList;
                        if (hkManager == null)
                        {
                            outMessage = "缓存港股资金表还没有初始化数据！";
                            isUpdate = false;
                        }
                        var _HkFindMemoryData = hkManager.GetByCapitalAccountAndCurrencyType(CapitalAccount, currencyType);
                        if (_HkFindMemoryData == null)
                        {
                            outMessage = "无法从缓存表中获取港股资金表类型资金帐号";
                            isUpdate = false;
                        }
                        var _HkFindResult = _HkFindMemoryData.Data;
                        if (_HkFindResult == null)
                        {
                            outMessage = "港股资金表中不存在该类型资金帐号";
                            isUpdate = false;
                        }
                        if (FromOrTo == 1)
                        {
                            if (_HkFindResult.AvailableCapital < TransferAmount)
                            {
                                outMessage = "港股资金表可用资金不足";
                                isUpdate = false;
                            }
                        }
                        #endregion

                        #region 组装相关数据并提交到内存中更新
                        if (isUpdate)
                        {
                            HK_CapitalAccount_DeltaInfo hk_Account_deltInfo = new HK_CapitalAccount_DeltaInfo();
                            hk_Account_deltInfo.CapitalAccountLogo = _HkFindMemoryData.Data.CapitalAccountLogo;
                            hk_Account_deltInfo.DeltaTime = DateTime.Now;
                            hk_Account_deltInfo.FreezeCapitalTotalDelta = 0;
                            hk_Account_deltInfo.HasDoneProfitLossTotalDelta = 0;
                            if (FromOrTo == 1)
                            {
                                hk_Account_deltInfo.AvailableCapitalDelta = -TransferAmount;
                                hk_Account_deltInfo.TodayOutInCapital = -TransferAmount;
                            }
                            else
                            {
                                hk_Account_deltInfo.AvailableCapitalDelta = TransferAmount;
                                hk_Account_deltInfo.TodayOutInCapital = TransferAmount;
                            }
                            _HkFindMemoryData.AddDeltaToMemory(hk_Account_deltInfo);
                        }
                        #endregion
                        #endregion
                        break;
                    case Types.AccountType.HKSpotHoldCode:
                        break;
                    default:
                        break;
                }

                //switch (CapitalAccountType)
                //{
                //    case 2:

                //        break;
                //    case 6:

                //    case 5:
                //        break;
                //}
                if (!string.IsNullOrEmpty(outMessage))
                {
                    format += ",异常信息：{5}";
                    msg = string.Format(format, CapitalAccountType, CapitalAccount, currencyType, FromOrTo, TransferAmount, outMessage);
                }
                else
                {
                    msg = string.Format(format, CapitalAccountType, CapitalAccount, currencyType, FromOrTo, TransferAmount);

                }
                LogHelper.WriteInfo(msg);
                return true;
            }
            catch (Exception ex)
            {
                outMessage = ex.Message.ToString();
                format += ",异常信息：{5}";
                msg = string.Format(format, CapitalAccountType, CapitalAccount, currencyType, FromOrTo, TransferAmount, outMessage);
                LogHelper.WriteError(msg, ex);
                return false;
            }
        }
        #endregion

        #region 转帐资金表操作
        /// <summary>
        /// 转帐资金表操作
        /// Title:资金转帐 
        ///  Create By:李健华
        ///  Create Date:2009-07-23
        /// Desc.:为了从内存表中获取数据
        /// * 把ReckoningCounter.DAL.AccountManagementAndFindDAL.TraderAccountManagement
        /// * 中相关方法移动到本类中
        /// Update By:李健华
        /// Update Date:2009-10-21
        /// Desc.:因为港股资金账户表不与原来现货的同一表中所以这里为了不修改原来所传递的参数和逻辑
        ///       这里所以先通过账户查询判断是港股还是证卷（原现货）账号，再做转入转出
        /// </summary>
        /// <param name="CapitalAccountType">资金帐号所属类型</param>
        /// <param name="CapitalAccount">资金帐号</param>
        /// <param name="currencyType">币种类型</param>
        /// <param name="tm">事务</param>
        /// <param name="FromOrTo">转出OR转入 1为转出 2为转入</param>
        /// <param name="TransferAmount">转帐金额</param>
        /// <param name="outMessage">消息</param>
        /// <returns></returns>
        private bool WriteCapitalTable(int capitalAccountType, string capitalAccount, int currencyType, Database db, DbTransaction trm, int FromOrTo, decimal TransferAmount, out string outMessage)
        {
            try
            {
                var _BankAccount = new UA_BankAccountTableDal();

                var _XhCapitalAccount = new XH_CapitalAccountTableDal();

                var _QhCapitalAccount = new QH_CapitalAccountTableDal();

                var _HkCapitalAccount = new HK_CapitalAccountDal();

                #region old code
                //switch (capitalAccountType)
                //{
                //    case 1:

                //        break;
                //    case 3:

                //        break;
                //    case 5:

                //        break;
                //    default:
                //        outMessage = "转入或转出资金帐户类型不对";
                //        return false;
                //}
                #endregion

                switch ((Types.AccountType)capitalAccountType)
                {
                    case Types.AccountType.BankAccount:
                        #region case 1--银行账户
                        var _bkFindResult = _BankAccount.GetListArray(string.Format("UserAccountDistributeLogo='{0}' AND  TradeCurrencyTypeLogo='{1}'", capitalAccount, currencyType));
                        //_BankAccount.Find(tm, string.Format("UserAccountDistributeLogo='{0}' AND  TradeCurrencyTypeLogo='{1}'", CapitalAccount, currencyType));

                        if (_bkFindResult == null || _bkFindResult.Count < 1)
                        {
                            outMessage = "银行资金表中不存在资金帐号";
                            return false;
                        }
                        if (FromOrTo == 1)
                        {
                            if (_bkFindResult[0].AvailableCapital < TransferAmount)
                            {
                                outMessage = "银行资金表可用资金不足";
                                return false;
                            }
                        }
                        if (FromOrTo == 1)
                        {
                            _bkFindResult[0].AvailableCapital = _bkFindResult[0].AvailableCapital -
                                                                TransferAmount;
                            _bkFindResult[0].TodayOutInCapital = _bkFindResult[0].TodayOutInCapital -
                                                                 TransferAmount;
                        }
                        else
                        {
                            _bkFindResult[0].AvailableCapital = _bkFindResult[0].AvailableCapital +
                                                                TransferAmount;
                            _bkFindResult[0].TodayOutInCapital = _bkFindResult[0].TodayOutInCapital +
                                                                 TransferAmount;
                        }
                        _BankAccount.Update(_bkFindResult[0], db, trm);
                        //if (!_BankAccount.Update(_bkFindResult[0],db,trm))
                        //{
                        //    outMessage = "银行资金表更新失败";
                        //    return false;
                        //}
                        #endregion
                        break;
                    case Types.AccountType.StockSpotCapital:
                        #region case 2 --现货资金账户

                        #region 改由内存表取数据
                        var xhManager = MemoryDataManager.XHCapitalMemoryList;
                        if (xhManager == null)
                        {
                            outMessage = "缓存现货资金表还没有初始化数据！";
                            return false;
                        }
                        var _XhFindMemoryData = xhManager.GetByCapitalAccountAndCurrencyType(capitalAccount, currencyType);
                        if (_XhFindMemoryData == null)
                        {
                            outMessage = "无法从缓存表中获取现货资金表类型资金帐号";
                            return false;
                        }
                        var _XhFindResult = _XhFindMemoryData.Data;
                        #endregion
                        if (_XhFindResult == null)
                        {
                            outMessage = "现货资金表中不存在该类型资金帐号";
                            return false;
                        }
                        if (FromOrTo == 1)
                        {
                            if (_XhFindResult.AvailableCapital < TransferAmount)
                            {
                                outMessage = "现货资金表可用资金不足";
                                return false;
                            }
                        }
                        XH_CapitalAccountTable_DeltaInfo xh_Account_deltInfo = new XH_CapitalAccountTable_DeltaInfo();
                        xh_Account_deltInfo.CapitalAccountLogo = _XhFindMemoryData.Data.CapitalAccountLogo;
                        xh_Account_deltInfo.DeltaTime = DateTime.Now;
                        xh_Account_deltInfo.FreezeCapitalTotalDelta = 0;
                        xh_Account_deltInfo.HasDoneProfitLossTotalDelta = 0;
                        if (FromOrTo == 1)
                        {
                            xh_Account_deltInfo.AvailableCapitalDelta = -TransferAmount;
                            xh_Account_deltInfo.TodayOutInCapital = -TransferAmount;
                            _XhFindMemoryData.AddDeltaToDB(xh_Account_deltInfo, db, trm);
                        }
                        else
                        {
                            xh_Account_deltInfo.AvailableCapitalDelta = TransferAmount;
                            xh_Account_deltInfo.TodayOutInCapital = TransferAmount;
                            _XhFindMemoryData.AddDeltaToDB(xh_Account_deltInfo, db, trm);

                        }
                        #region 更新改由更新内存表一起
                        // _XhFindMemoryData.AddDelta(_XhFindResult.AvailableCapital, _XhFindResult.FreezeCapitalTotal, _XhFindResult.TodayOutInCapital, _XhFindResult.HasDoneProfitLossTotal);
                        //_XhCapitalAccount.Update(_XhFindResult, db, trm);
                        #endregion

                        #endregion
                        break;
                    case Types.AccountType.StockSpotHoldCode:
                        break;

                    case Types.AccountType.CommodityFuturesHoldCode:
                        break;
                    case Types.AccountType.CommodityFuturesCapital:
                    case Types.AccountType.StockFuturesCapital:
                        #region case 6--期货资金账户

                        #region 改由内存表取数据
                        var qhManager = MemoryDataManager.QHCapitalMemoryList;
                        if (qhManager == null)
                        {
                            outMessage = "缓存期货资金表还没有初始化数据！";
                            return false;
                        }
                        var _QHFindResultData = qhManager.GetByCapitalAccountAndCurrencyType(capitalAccount, currencyType);
                        if (_QHFindResultData == null)
                        {
                            outMessage = "无法从缓存表中获取现货资金表类型资金帐号";
                            return false;
                        }
                        var _QHFindResult = _QHFindResultData.Data;
                        #endregion
                        if (_QHFindResult == null)
                        {
                            outMessage = "期货资金表中不存在该类型资金帐号";
                            return false;
                        }
                        if (FromOrTo == 1)
                        {
                            if (_QHFindResult.AvailableCapital < TransferAmount)
                            {
                                outMessage = "期货资金表可用资金不足";
                                return false;
                            }
                        }
                        QH_CapitalAccountTable_DeltaInfo qh_Account_deltInfo = new QH_CapitalAccountTable_DeltaInfo();
                        qh_Account_deltInfo.CapitalAccountLogoId = _QHFindResultData.Data.CapitalAccountLogoId;
                        qh_Account_deltInfo.DeltaTime = DateTime.Now;
                        qh_Account_deltInfo.FreezeCapitalTotalDelta = 0;
                        qh_Account_deltInfo.CloseFloatProfitLossTotalDelta = 0;
                        qh_Account_deltInfo.CloseMarketProfitLossTotalDelta = 0;
                        qh_Account_deltInfo.MarginTotalDelta = 0;

                        if (FromOrTo == 1)
                        {
                            qh_Account_deltInfo.AvailableCapitalDelta = -TransferAmount;
                            qh_Account_deltInfo.TodayOutInCapitalDelta = -TransferAmount;
                            _QHFindResultData.AddDeltaToDB(qh_Account_deltInfo, db, trm);
                        }
                        else
                        {
                            qh_Account_deltInfo.AvailableCapitalDelta = TransferAmount;
                            qh_Account_deltInfo.TodayOutInCapitalDelta = TransferAmount;
                            _QHFindResultData.AddDeltaToDB(qh_Account_deltInfo, db, trm);
                        }
                        #endregion
                        break;
                    case Types.AccountType.StockFuturesHoldCode:
                        break;
                    case Types.AccountType.HKSpotCapital:
                        #region case 8 --现货资金账户

                        #region 改由内存表取数据
                        var hkManager = MemoryDataManager.HKCapitalMemoryList;
                        if (hkManager == null)
                        {
                            outMessage = "缓存港股资金表还没有初始化数据！";
                            return false;
                        }
                        var _HkFindMemoryData = hkManager.GetByCapitalAccountAndCurrencyType(capitalAccount, currencyType);
                        if (_HkFindMemoryData == null)
                        {
                            outMessage = "无法从缓存表中获取港股资金表类型资金帐号";
                            return false;
                        }
                        var _HkFindResult = _HkFindMemoryData.Data;
                        #endregion
                        if (_HkFindResult == null)
                        {
                            outMessage = "港股资金表中不存在该类型资金帐号";
                            return false;
                        }
                        if (FromOrTo == 1)
                        {
                            if (_HkFindResult.AvailableCapital < TransferAmount)
                            {
                                outMessage = "港股资金表可用资金不足";
                                return false;
                            }
                        }
                        HK_CapitalAccount_DeltaInfo hk_Account_deltInfo = new HK_CapitalAccount_DeltaInfo();
                        hk_Account_deltInfo.CapitalAccountLogo = _HkFindMemoryData.Data.CapitalAccountLogo;
                        hk_Account_deltInfo.DeltaTime = DateTime.Now;
                        hk_Account_deltInfo.FreezeCapitalTotalDelta = 0;
                        hk_Account_deltInfo.HasDoneProfitLossTotalDelta = 0;
                        if (FromOrTo == 1)
                        {
                            hk_Account_deltInfo.AvailableCapitalDelta = -TransferAmount;
                            hk_Account_deltInfo.TodayOutInCapital = -TransferAmount;
                            _HkFindMemoryData.AddDeltaToDB(hk_Account_deltInfo, db, trm);
                        }
                        else
                        {
                            hk_Account_deltInfo.AvailableCapitalDelta = TransferAmount;
                            hk_Account_deltInfo.TodayOutInCapital = TransferAmount;
                            _HkFindMemoryData.AddDeltaToDB(hk_Account_deltInfo, db, trm);

                        }
                        #endregion
                        break;
                    case Types.AccountType.HKSpotHoldCode:
                        break;
                    default:
                        outMessage = "转入或转出资金帐户类型不对";
                        return false;
                }
                outMessage = "资金表操作成功";
                return true;
            }
            catch (Exception ex)
            {
                outMessage = ex.Message.ToString();
                LogHelper.WriteError(outMessage, ex);
                return false;
            }
        }
        #endregion

        #endregion

        #region 数据重新初始化 （清空试玩数据、个性化资金设置）

        #region 管理员个性化资金设置
        /// <summary>
        /// 个性化资金设置
        /// </summary>
        /// <param name="model">要设置的交易员资金实体</param>
        /// <param name="errMsg">操作异常信息</param>
        /// <returns>是否执行成功</returns>
        public bool PersonalizationCapital(CapitalPersonalization model, out string errMsg)
        {
            bool rtn = true;
            errMsg = string.Empty;
            string msg = "";
            try
            {
                //1、验证实体
                if (model == null)
                {
                    errMsg = "交易员资金实体为空！";
                    return false;
                }

                //2、验证交易员账户
                if (!CheckTradersAccount(model, out errMsg))
                {
                    return false;
                }

                //3、执行操作
                switch (model.PersonalType)
                {
                    case 0:
                        #region 所有类型
                        //银行账号
                        if (!PersonalizationCapitalTrans(Types.AccountType.BankAccount, model))
                        {
                            msg += "银行账户资金设置失败！";
                            rtn = false;
                        }
                        //现货
                        if (!PersonalizationCapitalTrans(Types.AccountType.StockSpotCapital, model))
                        {
                            msg += "现货账户资金设置失败！";
                            rtn = false;
                        }
                        //期货
                        if (!PersonalizationCapitalTrans(Types.AccountType.CommodityFuturesCapital, model))
                        {
                            msg += "商品期货账户资金设置失败！";
                            rtn = false;
                        }
                        if (!PersonalizationCapitalTrans(Types.AccountType.StockFuturesCapital, model))
                        {
                            msg += "股指期货账户资金设置失败！";
                            rtn = false;
                        }
                        //港股
                        if (!PersonalizationCapitalTrans(Types.AccountType.HKSpotCapital, model))
                        {
                            msg += "港股账户资金设置失败！";
                            rtn = false;
                        }
                        #endregion
                        break;
                    case 1:
                        #region 银行账号
                        if (!PersonalizationCapitalTrans(Types.AccountType.BankAccount, model))
                        {
                            msg += "银行账户资金设置失败！";
                            rtn = false;
                        }
                        #endregion
                        break;
                    case 2:
                        #region 现货
                        if (!PersonalizationCapitalTrans(Types.AccountType.StockSpotCapital, model))
                        {
                            msg += "现货账户资金设置失败！";
                            rtn = false;
                        }
                        #endregion
                        break;
                    case 3:
                        #region 期货
                        if (!PersonalizationCapitalTrans(Types.AccountType.CommodityFuturesCapital, model))
                        {
                            msg += "商品期货账户资金设置失败！";
                            rtn = false;
                        }
                        if (!PersonalizationCapitalTrans(Types.AccountType.StockFuturesCapital, model))
                        {
                            msg += "股指期货账户资金设置失败！";
                            rtn = false;
                        }
                        #endregion
                        break;
                    case 4:
                        #region 港股
                        if (!PersonalizationCapitalTrans(Types.AccountType.HKSpotCapital, model))
                        {
                            msg += "港股账户资金设置失败！";
                            rtn = false;
                        }
                        #endregion
                        break;
                    case 5:
                        #region 非银行类型
                        //现货
                        if (!PersonalizationCapitalTrans(Types.AccountType.StockSpotCapital, model))
                        {
                            msg += "现货账户资金设置失败！";
                            rtn = false;
                        }
                        //期货
                        if (!PersonalizationCapitalTrans(Types.AccountType.CommodityFuturesCapital, model))
                        {
                            msg += "商品期货账户资金设置失败！";
                            rtn = false;
                        }
                        if (!PersonalizationCapitalTrans(Types.AccountType.StockFuturesCapital, model))
                        {
                            msg += "股指期货账户资金设置失败！";
                            rtn = false;
                        }
                        //港股
                        if (!PersonalizationCapitalTrans(Types.AccountType.HKSpotCapital, model))
                        {
                            msg += "港股账户资金设置失败！";
                            rtn = false;
                        }
                        #endregion
                        break;
                }

                errMsg = msg;
                return rtn;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message.ToString();
                LogHelper.WriteError(errMsg, ex);
                return false;
            }
        }

        /// <summary>
        /// 执行个性化资金事务
        /// </summary>
        /// <param name="accountType">账号类型</param>
        /// <param name="model">要设置的交易员资金实体</param>
        /// <returns>是否执行成功</returns>
        private bool PersonalizationCapitalTrans(Types.AccountType accountType, CapitalPersonalization model)
        {
            UA_BankAccountTableDal bankAccount;
            XH_CapitalAccountTableDal xhAccount;
            HK_CapitalAccountDal hkAccount;
            QH_CapitalAccountTableDal qhAccount;

            UA_UserAccountAllocationTableDal userAccountDal = new UA_UserAccountAllocationTableDal();
            List<UA_UserAccountAllocationTableInfo> list;
            string findCodition = "";

            List<CapitalAccountTransInfoEntity> listRollback = new List<CapitalAccountTransInfoEntity>();

            Database db = DatabaseFactory.CreateDatabase();
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                DbTransaction trm = conn.BeginTransaction();

                try
                {
                    foreach (string tradeid in model.TradeID)
                    {
                        //现货、期货、港股需要更新内存数据
                        switch (accountType)
                        {
                            case Types.AccountType.BankAccount:
                                #region 银行
                                bankAccount = new UA_BankAccountTableDal();
                                //查询交易员的银行账号
                                findCodition = string.Format(" UserID='{0}' AND AccountTypeLogo=({1})", tradeid, (int)Types.AccountType.BankAccount);
                                list = userAccountDal.GetListArray(findCodition);
                                foreach (UA_UserAccountAllocationTableInfo acc in list)
                                {
                                    switch (model.SetCurrencyType)
                                    {
                                        case 0:
                                            //所有币种           
                                            bankAccount.PersonalizationCapital(model.RMBAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.RMB, db, trm);
                                            bankAccount.PersonalizationCapital(model.HKAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.HK, db, trm);
                                            bankAccount.PersonalizationCapital(model.USAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.US, db, trm);
                                            break;
                                        case 1:
                                            //人民币
                                            bankAccount.PersonalizationCapital(model.RMBAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.RMB, db, trm);
                                            break;
                                        case 2:
                                            //港币
                                            bankAccount.PersonalizationCapital(model.HKAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.HK, db, trm);
                                            break;
                                        case 3:
                                            //美元
                                            bankAccount.PersonalizationCapital(model.USAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.US, db, trm);
                                            break;
                                    }
                                }
                                #endregion
                                break;
                            case Types.AccountType.StockSpotCapital:
                                #region 现货
                                xhAccount = new XH_CapitalAccountTableDal();
                                //查询交易员的现货账号
                                findCodition = string.Format(" UserID='{0}' AND AccountTypeLogo=({1})", tradeid, (int)Types.AccountType.StockSpotCapital);
                                list = userAccountDal.GetListArray(findCodition);
                                foreach (UA_UserAccountAllocationTableInfo acc in list)
                                {
                                    switch (model.SetCurrencyType)
                                    {
                                        case 0:
                                            //所有币种           
                                            UpdateXHMemoryData(acc.UserAccountDistributeLogo, Types.CurrencyType.RMB, model.RMBAmount, ref listRollback);
                                            xhAccount.PersonalizationCapital(model.RMBAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.RMB, db, trm);

                                            UpdateXHMemoryData(acc.UserAccountDistributeLogo, Types.CurrencyType.HK, model.HKAmount, ref listRollback);
                                            xhAccount.PersonalizationCapital(model.HKAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.HK, db, trm);

                                            UpdateXHMemoryData(acc.UserAccountDistributeLogo, Types.CurrencyType.US, model.USAmount, ref listRollback);
                                            xhAccount.PersonalizationCapital(model.USAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.US, db, trm);
                                            break;
                                        case 1:
                                            //人民币
                                            UpdateXHMemoryData(acc.UserAccountDistributeLogo, Types.CurrencyType.RMB, model.RMBAmount, ref listRollback);
                                            xhAccount.PersonalizationCapital(model.RMBAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.RMB, db, trm);
                                            break;
                                        case 2:
                                            //港币
                                            UpdateXHMemoryData(acc.UserAccountDistributeLogo, Types.CurrencyType.HK, model.HKAmount, ref listRollback);
                                            xhAccount.PersonalizationCapital(model.HKAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.HK, db, trm);
                                            break;
                                        case 3:
                                            //美元
                                            UpdateXHMemoryData(acc.UserAccountDistributeLogo, Types.CurrencyType.US, model.USAmount, ref listRollback);
                                            xhAccount.PersonalizationCapital(model.USAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.US, db, trm);
                                            break;
                                    }
                                }
                                #endregion
                                break;
                            case Types.AccountType.CommodityFuturesCapital:
                                #region 商品期货
                                qhAccount = new QH_CapitalAccountTableDal();
                                //查询交易员的股指期货账号
                                findCodition = string.Format(" UserID='{0}' AND AccountTypeLogo=({1})", tradeid, (int)Types.AccountType.CommodityFuturesCapital);
                                list = userAccountDal.GetListArray(findCodition);
                                foreach (UA_UserAccountAllocationTableInfo acc in list)
                                {
                                    switch (model.SetCurrencyType)
                                    {
                                        case 0:
                                            //所有币种          
                                            UpdateQHMemoryData(acc.UserAccountDistributeLogo, Types.CurrencyType.RMB, model.RMBAmount, ref listRollback);
                                            qhAccount.PersonalizationCapital(model.RMBAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.RMB, db, trm);
                                            UpdateQHMemoryData(acc.UserAccountDistributeLogo, Types.CurrencyType.HK, model.HKAmount, ref listRollback);
                                            qhAccount.PersonalizationCapital(model.HKAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.HK, db, trm);
                                            UpdateQHMemoryData(acc.UserAccountDistributeLogo, Types.CurrencyType.US, model.USAmount, ref listRollback);
                                            qhAccount.PersonalizationCapital(model.USAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.US, db, trm);
                                            break;
                                        case 1:
                                            //人民币
                                            UpdateQHMemoryData(acc.UserAccountDistributeLogo, Types.CurrencyType.RMB, model.RMBAmount, ref listRollback);
                                            qhAccount.PersonalizationCapital(model.RMBAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.RMB, db, trm);
                                            break;
                                        case 2:
                                            //港币
                                            UpdateQHMemoryData(acc.UserAccountDistributeLogo, Types.CurrencyType.HK, model.HKAmount, ref listRollback);
                                            qhAccount.PersonalizationCapital(model.HKAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.HK, db, trm);
                                            break;
                                        case 3:
                                            //美元
                                            UpdateQHMemoryData(acc.UserAccountDistributeLogo, Types.CurrencyType.US, model.USAmount, ref listRollback);
                                            qhAccount.PersonalizationCapital(model.USAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.US, db, trm);
                                            break;
                                    }
                                }
                                #endregion
                                break;
                            case Types.AccountType.StockFuturesCapital:
                                #region 股指期货
                                qhAccount = new QH_CapitalAccountTableDal();
                                //查询交易员的股指期货账号
                                findCodition = string.Format(" UserID='{0}' AND AccountTypeLogo=({1})", tradeid,(int)Types.AccountType.StockFuturesCapital);
                                list = userAccountDal.GetListArray(findCodition);
                                foreach (UA_UserAccountAllocationTableInfo acc in list)
                                {
                                    switch (model.SetCurrencyType)
                                    {
                                        case 0:
                                            //所有币种          
                                            UpdateQHMemoryData(acc.UserAccountDistributeLogo, Types.CurrencyType.RMB, model.RMBAmount, ref listRollback);
                                            qhAccount.PersonalizationCapital(model.RMBAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.RMB, db, trm);
                                            UpdateQHMemoryData(acc.UserAccountDistributeLogo, Types.CurrencyType.HK, model.HKAmount, ref listRollback);
                                            qhAccount.PersonalizationCapital(model.HKAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.HK, db, trm);
                                            UpdateQHMemoryData(acc.UserAccountDistributeLogo, Types.CurrencyType.US, model.USAmount, ref listRollback);
                                            qhAccount.PersonalizationCapital(model.USAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.US, db, trm);
                                            break;
                                        case 1:
                                            //人民币
                                            UpdateQHMemoryData(acc.UserAccountDistributeLogo, Types.CurrencyType.RMB, model.RMBAmount, ref listRollback);
                                            qhAccount.PersonalizationCapital(model.RMBAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.RMB, db, trm);
                                            break;
                                        case 2:
                                            //港币
                                            UpdateQHMemoryData(acc.UserAccountDistributeLogo, Types.CurrencyType.HK, model.HKAmount, ref listRollback);
                                            qhAccount.PersonalizationCapital(model.HKAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.HK, db, trm);
                                            break;
                                        case 3:
                                            //美元
                                            UpdateQHMemoryData(acc.UserAccountDistributeLogo, Types.CurrencyType.US, model.USAmount, ref listRollback);
                                            qhAccount.PersonalizationCapital(model.USAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.US, db, trm);
                                            break;
                                    }
                                }
                                #endregion
                                break;
                            case Types.AccountType.HKSpotCapital:
                                #region 港股
                                hkAccount = new HK_CapitalAccountDal();
                                //查询交易员的港股账号
                                findCodition = string.Format(" UserID='{0}' AND AccountTypeLogo=({1})", tradeid, (int)Types.AccountType.HKSpotCapital);
                                list = userAccountDal.GetListArray(findCodition);
                                foreach (UA_UserAccountAllocationTableInfo acc in list)
                                {
                                    switch (model.SetCurrencyType)
                                    {
                                        case 0:
                                            //所有币种           
                                            UpdateHKMemoryData(acc.UserAccountDistributeLogo, Types.CurrencyType.RMB, model.RMBAmount, ref listRollback);
                                            hkAccount.PersonalizationCapital(model.RMBAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.RMB, db, trm);
                                            UpdateHKMemoryData(acc.UserAccountDistributeLogo, Types.CurrencyType.HK, model.HKAmount, ref listRollback);
                                            hkAccount.PersonalizationCapital(model.HKAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.HK, db, trm);
                                            UpdateHKMemoryData(acc.UserAccountDistributeLogo, Types.CurrencyType.US, model.USAmount, ref listRollback);
                                            hkAccount.PersonalizationCapital(model.USAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.US, db, trm);
                                            break;
                                        case 1:
                                            //人民币
                                            UpdateHKMemoryData(acc.UserAccountDistributeLogo, Types.CurrencyType.RMB, model.RMBAmount, ref listRollback);
                                            hkAccount.PersonalizationCapital(model.RMBAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.RMB, db, trm);
                                            break;
                                        case 2:
                                            //港币
                                            UpdateHKMemoryData(acc.UserAccountDistributeLogo, Types.CurrencyType.HK, model.HKAmount, ref listRollback);
                                            hkAccount.PersonalizationCapital(model.HKAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.HK, db, trm);
                                            break;
                                        case 3:
                                            //美元
                                            UpdateHKMemoryData(acc.UserAccountDistributeLogo, Types.CurrencyType.US, model.USAmount, ref listRollback);
                                            hkAccount.PersonalizationCapital(model.USAmount, acc.UserAccountDistributeLogo, Types.CurrencyType.US, db, trm);
                                            break;
                                    }
                                }
                                #endregion
                                break;
                        }
                    }
                    trm.Commit();
                }
                catch (Exception ex)
                {
                    trm.Rollback();
                    //回滚内存数据
                    RollbackCapitalMemoryData(ref listRollback);
                    LogHelper.WriteError(ex.Message.ToString(), ex);
                    return false;
                }
                finally
                {
                    trm.Dispose();
                }
            }
            return true;
        }


        /// <summary>
        /// 更新现货内存表资金数据
        /// </summary>
        /// <param name="account">资金账号</param>
        /// <param name="currencyType">币种</param>
        /// <param name="amount">金额</param>
        /// <param name="listRollback">记录回滚信息的列表</param>
        private void UpdateXHMemoryData(string account, Types.CurrencyType currencyType, decimal amount, ref  List<CapitalAccountTransInfoEntity> listRollback)
        {
            var xhManager = MemoryDataManager.XHCapitalMemoryList;
            var xhFindMemoryData = xhManager.GetByCapitalAccountAndCurrencyType(account, (int)currencyType);
            var xhFindResult = xhFindMemoryData.Data;

            XH_CapitalAccountTable_DeltaInfo delta = new XH_CapitalAccountTable_DeltaInfo();
            delta.AvailableCapitalDelta = amount - xhFindResult.AvailableCapital;

            xhFindMemoryData.AddDeltaToMemory(delta);

            CapitalAccountTransInfoEntity cap = new CapitalAccountTransInfoEntity();
            cap.accountType = Types.AccountType.StockSpotCapital;
            cap.account = account;
            cap.currencyType = currencyType;
            cap.AvailableCapitalDelta = delta.AvailableCapitalDelta;
            listRollback.Add(cap);
        }

        /// <summary>
        /// 更新港股内存表资金数据
        /// </summary>
        /// <param name="account">资金账号</param>
        /// <param name="currencyType">币种</param>
        /// <param name="amount">金额</param>
        /// <param name="listRollback">记录回滚信息的列表</param>
        private void UpdateHKMemoryData(string account, Types.CurrencyType currencyType, decimal amount, ref  List<CapitalAccountTransInfoEntity> listRollback)
        {
            var hkManager = MemoryDataManager.HKCapitalMemoryList;
            var hkFindMemoryData = hkManager.GetByCapitalAccountAndCurrencyType(account, (int)currencyType);
            var hkFindResult = hkFindMemoryData.Data;

            HK_CapitalAccount_DeltaInfo delta = new HK_CapitalAccount_DeltaInfo();
            delta.AvailableCapitalDelta = amount - hkFindResult.AvailableCapital;

            hkFindMemoryData.AddDeltaToMemory(delta);

            CapitalAccountTransInfoEntity cap = new CapitalAccountTransInfoEntity();
            cap.accountType = Types.AccountType.HKSpotCapital;
            cap.account = account;
            cap.currencyType = currencyType;
            cap.AvailableCapitalDelta = delta.AvailableCapitalDelta;
            listRollback.Add(cap);
        }

        /// <summary>
        /// 更新期货内存表资金数据
        /// </summary>
        /// <param name="account">资金账号</param>
        /// <param name="currencyType">币种</param>
        /// <param name="amount">金额</param>
        /// <param name="listRollback">记录回滚信息的列表</param>
        private void UpdateQHMemoryData(string account, Types.CurrencyType currencyType, decimal amount, ref  List<CapitalAccountTransInfoEntity> listRollback)
        {
            var qhManager = MemoryDataManager.QHCapitalMemoryList;
            var qhFindMemoryData = qhManager.GetByCapitalAccountAndCurrencyType(account, (int)currencyType);
            var qhFindResult = qhFindMemoryData.Data;

            QH_CapitalAccountTable_DeltaInfo delta = new QH_CapitalAccountTable_DeltaInfo();
            delta.AvailableCapitalDelta = amount - qhFindResult.AvailableCapital;

            qhFindMemoryData.AddDeltaToMemory(delta);

            CapitalAccountTransInfoEntity cap = new CapitalAccountTransInfoEntity();
            cap.accountType = Types.AccountType.StockFuturesCapital;
            cap.account = account;
            cap.currencyType = currencyType;
            cap.AvailableCapitalDelta = delta.AvailableCapitalDelta;
            listRollback.Add(cap);
        }

        /// <summary>
        /// 回滚资金内存数据
        /// </summary>
        /// <param name="listRollback">记录回滚信息的列表</param>
        private void RollbackCapitalMemoryData(ref List<CapitalAccountTransInfoEntity> listRollback)
        {
            var xhManager = MemoryDataManager.XHCapitalMemoryList;
            var hkManager = MemoryDataManager.HKCapitalMemoryList;
            var qhManager = MemoryDataManager.QHCapitalMemoryList;

            XHCapitalMemoryTable xhFindMemoryData;
            HKCapitalMemoryTable hkFindMemoryData;
            QHCapitalMemoryTable qhFindMemoryData;

            XH_CapitalAccountTable_DeltaInfo xhDelta;
            HK_CapitalAccount_DeltaInfo hkDelta;
            QH_CapitalAccountTable_DeltaInfo qhDelta;

            foreach (CapitalAccountTransInfoEntity cap in listRollback)
            {
                switch (cap.accountType)
                {
                    case Types.AccountType.StockSpotCapital:
                        xhFindMemoryData = xhManager.GetByCapitalAccountAndCurrencyType(cap.account, (int)cap.currencyType);
                        xhDelta = new XH_CapitalAccountTable_DeltaInfo();
                        xhDelta.AvailableCapitalDelta = cap.AvailableCapitalDelta;
                        xhDelta.FreezeCapitalTotalDelta = cap.FreezeCapitalTotalDelta;
                        xhDelta.TodayOutInCapital = cap.TodayOutInCapital;
                        xhDelta.HasDoneProfitLossTotalDelta = cap.HasDoneProfitLossTotalDelta;
                        xhFindMemoryData.RollBackMemory(xhDelta);
                        break;
                    case Types.AccountType.CommodityFuturesCapital:
                        break;
                    case Types.AccountType.StockFuturesCapital:
                        qhFindMemoryData = qhManager.GetByCapitalAccountAndCurrencyType(cap.account, (int)cap.currencyType);
                        qhDelta = new QH_CapitalAccountTable_DeltaInfo();
                        qhDelta.AvailableCapitalDelta = cap.AvailableCapitalDelta;
                        qhDelta.AvailableCapitalDelta = cap.AvailableCapitalDelta;
                        qhDelta.FreezeCapitalTotalDelta = cap.FreezeCapitalTotalDelta;
                        qhDelta.TodayOutInCapitalDelta = cap.TodayOutInCapitalDelta;
                        qhDelta.MarginTotalDelta = cap.MarginTotalDelta;
                        qhDelta.CloseFloatProfitLossTotalDelta = cap.CloseFloatProfitLossTotalDelta;
                        qhDelta.CloseMarketProfitLossTotalDelta = cap.CloseMarketProfitLossTotalDelta;
                        qhFindMemoryData.RollBackMemory(qhDelta);
                        break;
                    case Types.AccountType.HKSpotCapital:
                        hkFindMemoryData = hkManager.GetByCapitalAccountAndCurrencyType(cap.account, (int)cap.currencyType);
                        hkDelta = new HK_CapitalAccount_DeltaInfo();
                        hkDelta.AvailableCapitalDelta = cap.AvailableCapitalDelta;
                        hkDelta.FreezeCapitalTotalDelta = cap.FreezeCapitalTotalDelta;
                        hkDelta.TodayOutInCapital = cap.TodayOutInCapital;
                        hkDelta.HasDoneProfitLossTotalDelta = cap.HasDoneProfitLossTotalDelta;
                        hkFindMemoryData.RollBackMemory(hkDelta);
                        break;
                }
            }
        }

        /// <summary>
        /// 回滚持仓内存数据
        /// </summary>
        /// <param name="listRollback">记录回滚信息的列表</param>
        private void RollbackHoldMemoryData(ref List<HoldAccountTransInfoEntity> listRollback)
        {
            var xhManager = MemoryDataManager.XHHoldMemoryList;
            var hkManager = MemoryDataManager.HKHoldMemoryList;
            var qhManager = MemoryDataManager.QHHoldMemoryList;

            XHHoldMemoryTable xhFindMemoryData;
            HKHoldMemoryTable hkFindMemoryData;
            QHHoldMemoryTable qhFindMemoryData;

            XH_AccountHoldTableInfo_Delta xhDelta;
            HK_AccountHoldInfo_Delta hkDelta;
            QH_HoldAccountTableInfo_Delta qhDelta;

            foreach (HoldAccountTransInfoEntity cap in listRollback)
            {
                switch (cap.accountType)
                {
                    case Types.AccountType.StockSpotHoldCode:
                        xhFindMemoryData = xhManager.GetByHoldAccountAndCurrencyType(cap.account, "", (int)cap.currencyType);
                        xhDelta = new XH_AccountHoldTableInfo_Delta();
                        xhDelta.AvailableAmountDelta = cap.AvailableAmountDelta;
                        xhDelta.FreezeAmountDelta = cap.FreezeAmountDelta;
                        xhDelta.Data = cap.xhData;
                        xhFindMemoryData.RollBackMemory(xhDelta);
                        break;
                    case Types.AccountType.CommodityFuturesHoldCode:
                        break;
                    case Types.AccountType.StockFuturesHoldCode:
                        qhFindMemoryData = qhManager.GetByHoldAccountAndCurrencyType(cap.account, "", 0, (int)cap.currencyType);
                        qhDelta = new QH_HoldAccountTableInfo_Delta();
                        qhDelta.HistoryFreezeAmountDelta = cap.HistoryFreezeAmountDelta;
                        qhDelta.HistoryHoldAmountDelta = cap.HistoryHoldAmountDelta;
                        qhDelta.MarginDelta = cap.MarginDelta;
                        qhDelta.TodayFreezeAmountDelta = cap.TodayFreezeAmountDelta;
                        qhDelta.TodayHoldAmountDelta = cap.TodayHoldAmountDelta;
                        qhDelta.Data = cap.qhData;
                        qhFindMemoryData.RollBackMemory(qhDelta);
                        break;
                    case Types.AccountType.HKSpotHoldCode:
                        hkFindMemoryData = hkManager.GetByHoldAccountAndCurrencyType(cap.account, "", (int)cap.currencyType);
                        hkDelta = new HK_AccountHoldInfo_Delta();
                        hkDelta.AvailableAmountDelta = cap.AvailableAmountDelta;
                        hkDelta.FreezeAmountDelta = cap.FreezeAmountDelta;                        
                        hkFindMemoryData.RollBackMemory(hkDelta);
                        break;
                }
            }
        }

        #endregion

        #region 管理员清空试玩数据
        /// <summary>
        /// 清空试玩数据
        /// </summary>
        /// <param name="model">要设置的交易员资金实体</param>
        /// <param name="errMsg">操作异常信息</param>
        /// <returns>是否执行成功</returns>
        public bool ClearTrialData(CapitalPersonalization model, out string errMsg)
        {
            bool rtn = true;
            errMsg = string.Empty;
            string msg = "";
            try
            {
                //1、验证实体
                if (model == null)
                {
                    errMsg = "交易员资金实体为空！";
                    LogHelper.WriteDebug(errMsg);
                    return false;
                }

                //2、验证交易员账户
                if (!CheckTradersAccount(model, out errMsg))
                {
                    LogHelper.WriteDebug(errMsg);
                    return false;
                }

                //3、执行清空操作
                switch (model.PersonalType)
                {
                    case 0:
                        LogHelper.WriteDebug("开始执行清空所有类型账号操作...");
                        #region 所有类型
                        //银行账号
                        if (!ClearTrialDataTrans(Types.AccountType.BankAccount, model))
                        {
                            msg += "";
                            rtn = false;
                        }
                        //现货
                        if (!ClearTrialDataTrans(Types.AccountType.StockSpotCapital, model))
                        {
                            msg += "现货数据重新初始化失败！";
                            rtn = false;
                        }
                        //期货
                        if (!ClearTrialDataTrans(Types.AccountType.CommodityFuturesCapital, model))
                        {
                            msg += "商品期货数据重新初始化失败！";
                            rtn = false;
                        }
                        if (!ClearTrialDataTrans(Types.AccountType.StockFuturesCapital, model))
                        {
                            msg += "股指期货数据重新初始化失败！";
                            rtn = false;
                        }
                        //港股
                        if (!ClearTrialDataTrans(Types.AccountType.HKSpotCapital, model))
                        {
                            msg += "港股数据重新初始化失败！";
                            rtn = false;
                        }
                        #endregion
                        break;
                    case 1:
                        LogHelper.WriteDebug("开始执行清空银行账号操作...");
                        #region 银行账号
                        //银行账号
                        if (!ClearTrialDataTrans(Types.AccountType.BankAccount, model))
                        {
                            msg += "";
                            rtn = false;
                        }
                        #endregion
                        break;
                    case 2:
                        LogHelper.WriteDebug("开始执行清空现货账号操作...");
                        #region 现货
                        if (!ClearTrialDataTrans(Types.AccountType.StockSpotCapital, model))
                        {
                            msg += "现货数据重新初始化失败！";
                            rtn = false;
                        }
                        #endregion
                        break;
                    case 3:
                        LogHelper.WriteDebug("开始执行清空期货账号操作...");
                        #region 期货
                        if (!ClearTrialDataTrans(Types.AccountType.CommodityFuturesCapital, model))
                        {
                            msg += "商品期货数据重新初始化失败！";
                            rtn = false;
                        }
                        if (!ClearTrialDataTrans(Types.AccountType.StockFuturesCapital, model))
                        {
                            msg += "股指期货数据重新初始化失败！";
                            rtn = false;
                        }
                        #endregion
                        break;
                    case 4:
                        LogHelper.WriteDebug("开始执行清空港股账号操作...");
                        #region 港股
                        if (!ClearTrialDataTrans(Types.AccountType.HKSpotCapital, model))
                        {
                            msg += "港股数据重新初始化失败！";
                            rtn = false;
                        }
                        #endregion
                        break;
                    case 5:
                        LogHelper.WriteDebug("开始执行清空所非银行类型账号操作...");
                        #region 非银行类型
                        //现货
                        if (!ClearTrialDataTrans(Types.AccountType.StockSpotCapital, model))
                        {
                            msg += "现货数据重新初始化失败！";
                            rtn = false;
                        }
                        //期货
                        if (!ClearTrialDataTrans(Types.AccountType.CommodityFuturesCapital, model))
                        {
                            msg += "商品期货数据重新初始化失败！";
                            rtn = false;
                        }
                        if (!ClearTrialDataTrans(Types.AccountType.StockFuturesCapital, model))
                        {
                            msg += "股指期货数据重新初始化失败！";
                            rtn = false;
                        }
                        //港股
                        if (!ClearTrialDataTrans(Types.AccountType.HKSpotCapital, model))
                        {
                            msg += "港股数据重新初始化失败！";
                            rtn = false;
                        }
                        #endregion
                        break;
                }
                string msg2 = "";
                //当数据清除成功时执行初始化资金
                if (rtn)
                {
                    //4、初始化资金(仅银行账户)
                    model.PersonalType = 1;
                    if (!PersonalizationCapital(model, out msg2))
                    {
                        msg += "初始化资金失败！";
                        rtn = false;
                    }
                }
                errMsg = msg + msg2;
                return rtn;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message.ToString();
                LogHelper.WriteError(errMsg, ex);
                return false;
            }
        }

        /// <summary>
        /// 执行清空试玩数据事务
        /// </summary>
        /// <param name="accountType">账号类型</param>
        /// <param name="model">要设置的交易员资金实体</param>
        /// <returns>是否执行成功</returns>
        private bool ClearTrialDataTrans(Types.AccountType accountType, CapitalPersonalization model)
        {
            switch (accountType)
            {
                case Types.AccountType.BankAccount:
                    LogHelper.WriteDebug("开始执行清空银行账号操作...");
                    return ClearTrialDataTransBK(model);
                case Types.AccountType.StockSpotCapital:
                    LogHelper.WriteDebug("开始执行清空现货账号操作...");
                    return ClearTrialDataTransXH(model);
                case Types.AccountType.CommodityFuturesCapital:
                    LogHelper.WriteDebug("开始执行清空商品期货账号操作...");
                    return ClearTrialDataTransSPQH(model);
                case Types.AccountType.StockFuturesCapital:
                    LogHelper.WriteDebug("开始执行清空股指期货账号操作...");
                    return ClearTrialDataTransGZQH(model);
                case Types.AccountType.HKSpotCapital:
                    LogHelper.WriteDebug("开始执行清空港股账号操作...");
                    return ClearTrialDataTransHK(model);
            }
            return true;
        }

        /// <summary>
        /// 执行银行账户类型清空试玩数据事务
        /// </summary>
        /// <param name="model">要设置的交易员资金实体</param>
        /// <returns>是否执行成功</returns>
        private bool ClearTrialDataTransBK(CapitalPersonalization model)
        {
            StringBuilder sb = new StringBuilder();
            UA_UserAccountAllocationTableDal userAccountDal = new UA_UserAccountAllocationTableDal();
            //List<UA_UserAccountAllocationTableInfo> list;

            Database db = DatabaseFactory.CreateDatabase();
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                DbTransaction trm = conn.BeginTransaction();

                ReckoningTransaction tm = new ReckoningTransaction();
                tm.Database = db;
                tm.Transaction = trm;
                try
                {
                    foreach (string tradeid in model.TradeID)
                    {
                        string findCodition = string.Format(" (select [UserAccountDistributeLogo] from dbo.UA_UserAccountAllocationTable where userID={0})", tradeid);

                        sb.Append("delete UA_CapitalFlowTable where FromCapitalAccount in " + findCodition + ";");
                        sb.Append("delete UA_CapitalFlowTable where ToCapitalAccount in " + findCodition + ";");

                        sb.Append("update UA_BankAccountTable set CapitalRemainAmount=0,BalanceOfTheDay=0,TodayOutInCapital=0,FreezeCapital=0,AvailableCapital=0 ");
                        sb.Append("where UserAccountDistributeLogo in " + findCodition + ";");

                    }
                    DbHelperSQL.ExecuteCountSql(sb.ToString(), tm);
                    trm.Commit();
                }
                catch (Exception ex)
                {
                    trm.Rollback();
                    LogHelper.WriteError(ex.Message, ex);
                    return false;
                }
                finally
                {
                    trm.Dispose();
                }
            }
            return true;
        }

        /// <summary>
        /// 执行现货账户类型清空试玩数据事务
        /// </summary>
        /// <param name="model">要设置的交易员资金实体</param>
        /// <returns>是否执行成功</returns>
        private bool ClearTrialDataTransXH(CapitalPersonalization model)
        {
            StringBuilder sb = new StringBuilder();
            UA_UserAccountAllocationTableDal userAccountDal = new UA_UserAccountAllocationTableDal();
            List<UA_UserAccountAllocationTableInfo> list;

            List<CapitalAccountTransInfoEntity> listRollback = new List<CapitalAccountTransInfoEntity>();
            List<HoldAccountTransInfoEntity> listRollback2 = new List<HoldAccountTransInfoEntity>();

            Database db = DatabaseFactory.CreateDatabase();
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                DbTransaction trm = conn.BeginTransaction();

                ReckoningTransaction tm = new ReckoningTransaction();
                tm.Database = db;
                tm.Transaction = trm;
                try
                {
                    foreach (string tradeid in model.TradeID)
                    {
                        //获取交易员所有现货账号
                        string findCodition = "";
                        findCodition = string.Format(" UserID='{0}' AND AccountTypeLogo in ({1},{2})", tradeid, (int)Types.AccountType.StockSpotCapital, (int)Types.AccountType.StockSpotHoldCode);
                        list = userAccountDal.GetListArray(findCodition);
                        foreach (UA_UserAccountAllocationTableInfo acc in list)
                        {
                            sb.Append("delete a from XH_CapitalAccountTable_Delta a ");
                            sb.Append("inner join XH_CapitalAccountTable b on a.CapitalAccountLogo=b.CapitalAccountLogo ");
                            sb.Append("where b.UserAccountDistributeLogo='" + acc.UserAccountDistributeLogo + "';");

                            sb.Append("delete a from XH_CapitalAccountFreezeTable a ");
                            sb.Append("inner join XH_CapitalAccountTable b on a.CapitalAccountLogo=b.CapitalAccountLogo ");
                            sb.Append("where b.UserAccountDistributeLogo='" + acc.UserAccountDistributeLogo + "';");

                            sb.Append("delete a from XH_AcccountHoldFreezeTable a ");
                            sb.Append("inner join XH_AccountHoldTable b on a.AccountHoldLogo=b.AccountHoldLogoId ");
                            sb.Append("where b.UserAccountDistributeLogo='" + acc.UserAccountDistributeLogo + "';");

                            sb.Append("delete XH_AccountHoldTable where UserAccountDistributeLogo='" + acc.UserAccountDistributeLogo + "';");
                            sb.Append("delete XH_MelonCutRegisterTable where UserAccountDistributeLogo='" + acc.UserAccountDistributeLogo + "';");
                            sb.Append("delete XH_TodayTradeTable where CapitalAccount='" + acc.UserAccountDistributeLogo + "';");
                            sb.Append("delete XH_TodayEntrustTable where CapitalAccount='" + acc.UserAccountDistributeLogo + "';");
                            sb.Append("delete XH_HistoryTradeTable where CapitalAccount='" + acc.UserAccountDistributeLogo + "';");
                            sb.Append("delete XH_HistoryEntrustTable where CapitalAccount='" + acc.UserAccountDistributeLogo + "';");

                            sb.Append("update XH_CapitalAccountTable set AvailableCapital=0,BalanceOfTheDay=0,TodayOutInCapital=0,FreezeCapitalTotal=0,HasDoneProfitLossTotal=0 ");
                            sb.Append("where UserAccountDistributeLogo='" + acc.UserAccountDistributeLogo + "';");

                            //更新内存表
                            if (acc.AccountTypeLogo == (int)Types.AccountType.StockSpotCapital)
                            {
                                ClearXHCapitalMemoryData(acc.UserAccountDistributeLogo, ref listRollback);
                            }
                            if (acc.AccountTypeLogo == (int)Types.AccountType.StockSpotHoldCode)
                            {
                                ClearXHHoldMemoryData(acc.UserAccountDistributeLogo, ref listRollback2);
                            }
                        }
                    }
                    DbHelperSQL.ExecuteCountSql(sb.ToString(), tm);
                    trm.Commit();
                }
                catch (Exception ex)
                {
                    trm.Rollback();
                    //回滚内存表
                    RollbackCapitalMemoryData(ref listRollback);
                    RollbackHoldMemoryData(ref listRollback2);
                    LogHelper.WriteError(ex.Message, ex);
                    return false;
                }
                finally
                {
                    trm.Dispose();
                }
            }
            return true;
        }

        /// <summary>
        /// 执行商品期货账户类型清空试玩数据事务
        /// </summary>
        /// <param name="model">要设置的交易员资金实体</param>
        /// <returns>是否执行成功</returns>
        private bool ClearTrialDataTransSPQH(CapitalPersonalization model)
        {
            StringBuilder sb = new StringBuilder();
            UA_UserAccountAllocationTableDal userAccountDal = new UA_UserAccountAllocationTableDal();
            List<UA_UserAccountAllocationTableInfo> list;

            List<CapitalAccountTransInfoEntity> listRollback = new List<CapitalAccountTransInfoEntity>();
            List<HoldAccountTransInfoEntity> listRollback2 = new List<HoldAccountTransInfoEntity>();

            Database db = DatabaseFactory.CreateDatabase();
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                DbTransaction trm = conn.BeginTransaction();

                ReckoningTransaction tm = new ReckoningTransaction();
                tm.Database = db;
                tm.Transaction = trm;
                try
                {
                    foreach (string tradeid in model.TradeID)
                    {
                        //获取交易员所有股指期货账号
                        string findCodition = "";
                        findCodition = string.Format(" UserID='{0}' AND AccountTypeLogo in ({1},{2})", tradeid, (int)Types.AccountType.CommodityFuturesCapital, (int)Types.AccountType.CommodityFuturesHoldCode);
                        list = userAccountDal.GetListArray(findCodition);
                        foreach (UA_UserAccountAllocationTableInfo acc in list)
                        {
                            sb.Append("delete QH_TradeCapitalFlowDetail where UserCapitalAccount='" + acc.UserAccountDistributeLogo + "';");

                            sb.Append("delete a from QH_CapitalAccountFreezeTable a ");
                            sb.Append("inner join QH_CapitalAccountTable b on a.CapitalAccountLogo=b.CapitalAccountLogoId ");
                            sb.Append("where b.UserAccountDistributeLogo='" + acc.UserAccountDistributeLogo + "';");

                            sb.Append("delete a from QH_CapitalAccountTable_Delta a ");
                            sb.Append("inner join QH_CapitalAccountTable b on a.CapitalAccountLogoId=b.CapitalAccountLogoId ");
                            sb.Append("where b.UserAccountDistributeLogo='" + acc.UserAccountDistributeLogo + "';");

                            sb.Append("delete a from QH_HoldAccountFreezeTable a ");
                            sb.Append("inner join QH_HoldAccountTable b on a.FreezeTypeLogo=b.AccountHoldLogoId ");
                            sb.Append("where b.UserAccountDistributeLogo='" + acc.UserAccountDistributeLogo + "';");

                            sb.Append("delete QH_HoldAccountTable where UserAccountDistributeLogo='" + acc.UserAccountDistributeLogo + "';");
                            sb.Append("delete QH_TodayTradeTable where CapitalAccount='" + acc.UserAccountDistributeLogo + "';");
                            sb.Append("delete QH_TodayEntrustTable where CapitalAccount='" + acc.UserAccountDistributeLogo + "';");
                            sb.Append("delete QH_HistoryTradeTable where CapitalAccount='" + acc.UserAccountDistributeLogo + "';");
                            sb.Append("delete QH_HistoryEntrustTable where CapitalAccount='" + acc.UserAccountDistributeLogo + "';");

                            sb.Append("update QH_CapitalAccountTable set BalanceOfTheDay=0,TodayOutInCapital=0,AvailableCapital=0,FreezeCapitalTotal=0,MarginTotal=0,CloseFloatProfitLossTotal=0,CloseMarketProfitLossTotal=0 ");
                            sb.Append("where UserAccountDistributeLogo='" + acc.UserAccountDistributeLogo + "'");

                            //更新内存表
                            if (acc.AccountTypeLogo == (int)Types.AccountType.CommodityFuturesCapital)
                            {
                                ClearQHCapitalMemoryData(acc.UserAccountDistributeLogo, ref listRollback);
                            }
                            if (acc.AccountTypeLogo == (int)Types.AccountType.CommodityFuturesHoldCode)
                            {
                                ClearQHHoldMemoryData(acc.UserAccountDistributeLogo, ref listRollback2);
                            }
                        }
                    }
                    DbHelperSQL.ExecuteCountSql(sb.ToString(), tm);
                    trm.Commit();
                }
                catch (Exception ex)
                {
                    trm.Rollback();
                    //回滚内存表
                    RollbackCapitalMemoryData(ref listRollback);
                    RollbackHoldMemoryData(ref listRollback2);
                    LogHelper.WriteError(ex.Message, ex);
                    return false;
                }
                finally
                {
                    trm.Dispose();
                }
            }
            return true;
        }

        /// <summary>
        /// 执行股指期货账户类型清空试玩数据事务
        /// </summary>
        /// <param name="model">要设置的交易员资金实体</param>
        /// <returns>是否执行成功</returns>
        private bool ClearTrialDataTransGZQH(CapitalPersonalization model)
        {
            StringBuilder sb = new StringBuilder();
            UA_UserAccountAllocationTableDal userAccountDal = new UA_UserAccountAllocationTableDal();
            List<UA_UserAccountAllocationTableInfo> list;

            List<CapitalAccountTransInfoEntity> listRollback = new List<CapitalAccountTransInfoEntity>();
            List<HoldAccountTransInfoEntity> listRollback2 = new List<HoldAccountTransInfoEntity>();

            Database db = DatabaseFactory.CreateDatabase();
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                DbTransaction trm = conn.BeginTransaction();

                ReckoningTransaction tm = new ReckoningTransaction();
                tm.Database = db;
                tm.Transaction = trm;
                try
                {
                    foreach (string tradeid in model.TradeID)
                    {
                        //获取交易员所有股指期货账号
                        string findCodition = "";
                        findCodition = string.Format(" UserID='{0}' AND AccountTypeLogo in ({1},{2})", tradeid, (int)Types.AccountType.StockFuturesCapital, (int)Types.AccountType.StockFuturesHoldCode);
                        list = userAccountDal.GetListArray(findCodition);
                        foreach (UA_UserAccountAllocationTableInfo acc in list)
                        {
                            sb.Append("delete QH_TradeCapitalFlowDetail where UserCapitalAccount='" + acc.UserAccountDistributeLogo + "';");

                            sb.Append("delete a from QH_CapitalAccountFreezeTable a ");
                            sb.Append("inner join QH_CapitalAccountTable b on a.CapitalAccountLogo=b.CapitalAccountLogoId ");
                            sb.Append("where b.UserAccountDistributeLogo='" + acc.UserAccountDistributeLogo + "';");

                            sb.Append("delete a from QH_CapitalAccountTable_Delta a ");
                            sb.Append("inner join QH_CapitalAccountTable b on a.CapitalAccountLogoId=b.CapitalAccountLogoId ");
                            sb.Append("where b.UserAccountDistributeLogo='" + acc.UserAccountDistributeLogo + "';");

                            sb.Append("delete a from QH_HoldAccountFreezeTable a ");
                            sb.Append("inner join QH_HoldAccountTable b on a.FreezeTypeLogo=b.AccountHoldLogoId ");
                            sb.Append("where b.UserAccountDistributeLogo='" + acc.UserAccountDistributeLogo + "';");

                            sb.Append("delete QH_HoldAccountTable where UserAccountDistributeLogo='" + acc.UserAccountDistributeLogo + "';");
                            sb.Append("delete QH_TodayTradeTable where CapitalAccount='" + acc.UserAccountDistributeLogo + "';");
                            sb.Append("delete QH_TodayEntrustTable where CapitalAccount='" + acc.UserAccountDistributeLogo + "';");
                            sb.Append("delete QH_HistoryTradeTable where CapitalAccount='" + acc.UserAccountDistributeLogo + "';");
                            sb.Append("delete QH_HistoryEntrustTable where CapitalAccount='" + acc.UserAccountDistributeLogo + "';");

                            sb.Append("update QH_CapitalAccountTable set BalanceOfTheDay=0,TodayOutInCapital=0,AvailableCapital=0,FreezeCapitalTotal=0,MarginTotal=0,CloseFloatProfitLossTotal=0,CloseMarketProfitLossTotal=0 ");
                            sb.Append("where UserAccountDistributeLogo='" + acc.UserAccountDistributeLogo + "'");

                            //更新内存表
                            if (acc.AccountTypeLogo == (int)Types.AccountType.StockFuturesCapital)
                            {
                                ClearQHCapitalMemoryData(acc.UserAccountDistributeLogo, ref listRollback);
                            }
                            if (acc.AccountTypeLogo == (int)Types.AccountType.StockFuturesHoldCode)
                            {
                                ClearQHHoldMemoryData(acc.UserAccountDistributeLogo, ref listRollback2);
                            }
                        }
                    }
                    DbHelperSQL.ExecuteCountSql(sb.ToString(), tm);
                    trm.Commit();
                }
                catch (Exception ex)
                {
                    trm.Rollback();
                    //回滚内存表
                    RollbackCapitalMemoryData(ref listRollback);
                    RollbackHoldMemoryData(ref listRollback2);
                    LogHelper.WriteError(ex.Message, ex);
                    return false;
                }
                finally
                {
                    trm.Dispose();
                }
            }
            return true;
        }

        /// <summary>
        /// 执行港股账户类型清空试玩数据事务
        /// </summary>
        /// <param name="model">要设置的交易员资金实体</param>
        /// <returns>是否执行成功</returns>
        private bool ClearTrialDataTransHK(CapitalPersonalization model)
        {
            StringBuilder sb = new StringBuilder();
            UA_UserAccountAllocationTableDal userAccountDal = new UA_UserAccountAllocationTableDal();
            List<UA_UserAccountAllocationTableInfo> list;

            List<CapitalAccountTransInfoEntity> listRollback = new List<CapitalAccountTransInfoEntity>();
            List<HoldAccountTransInfoEntity> listRollback2 = new List<HoldAccountTransInfoEntity>();

            Database db = DatabaseFactory.CreateDatabase();
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                DbTransaction trm = conn.BeginTransaction();

                ReckoningTransaction tm = new ReckoningTransaction();
                tm.Database = db;
                tm.Transaction = trm;
                try
                {
                    foreach (string tradeid in model.TradeID)
                    {
                        //获取交易员所有港股账号
                        string findCodition = "";
                        findCodition = string.Format(" UserID='{0}' AND AccountTypeLogo in ({1},{2})", tradeid, (int)Types.AccountType.HKSpotCapital, (int)Types.AccountType.HKSpotHoldCode);
                        list = userAccountDal.GetListArray(findCodition);
                        foreach (UA_UserAccountAllocationTableInfo acc in list)
                        {
                            sb.Append("delete a from HK_CapitalAccount_Delta a ");
                            sb.Append("inner join HK_CapitalAccount b on a.CapitalAccountLogo=b.CapitalAccountLogo ");
                            sb.Append("where b.UserAccountDistributeLogo='" + acc.UserAccountDistributeLogo + "';");

                            sb.Append("delete a from HK_CapitalAccountFreeze a ");
                            sb.Append("inner join HK_CapitalAccount b on a.CapitalAccountLogo=b.CapitalAccountLogo ");
                            sb.Append("where b.UserAccountDistributeLogo='" + acc.UserAccountDistributeLogo + "';");

                            sb.Append("delete a from HK_AcccountHoldFreeze a ");
                            sb.Append("inner join HK_AccountHold b on a.AccountHoldLogo=b.AccountHoldLogoID ");
                            sb.Append("where b.UserAccountDistributeLogo='" + acc.UserAccountDistributeLogo + "';");

                            sb.Append("delete HK_AccountHold where UserAccountDistributeLogo='" + acc.UserAccountDistributeLogo + "';");
                            //sb.Append("delete HK_ModifyOrderDetails ");
                            //sb.Append("where NewRequestNumber in ");
                            //sb.Append("(select EntrustNumber from ");
                            //sb.Append("(select EntrustNumber,HoldAccount,CapitalAccount from HK_TodayEntrust ");
                            //sb.Append("union ");
                            //sb.Append("select EntrustNumber,HoldAccount,CapitalAccount from HK_HistoryEntrust) t ");
                            //sb.Append("where t.HoldAccount='" + acc.UserAccountDistributeLogo + "' or CapitalAccount='" + acc.UserAccountDistributeLogo + "') ");
                            sb.Append("delete HK_ModifyOrderRequest where FundAccountId='" + acc.UserAccountDistributeLogo + "';");
                            sb.Append("delete HK_HistoryModifyOrderRequest where FundAccountId='" + acc.UserAccountDistributeLogo + "';");
                            sb.Append("delete HK_TodayTrade where CapitalAccount='" + acc.UserAccountDistributeLogo + "';");
                            sb.Append("delete HK_TodayEntrust where CapitalAccount='" + acc.UserAccountDistributeLogo + "';");
                            sb.Append("delete HK_HistoryTrade where CapitalAccount='" + acc.UserAccountDistributeLogo + "';");
                            sb.Append("delete HK_HistoryEntrust where CapitalAccount='" + acc.UserAccountDistributeLogo + "';");

                            sb.Append("update HK_CapitalAccount set AvailableCapital=0,BalanceOfTheDay=0,TodayOutInCapital=0,FreezeCapitalTotal=0,HasDoneProfitLossTotal=0 ");
                            sb.Append("where UserAccountDistributeLogo='" + acc.UserAccountDistributeLogo + "';");

                            //更新内存表
                            if (acc.AccountTypeLogo == (int)Types.AccountType.HKSpotCapital)
                            {
                                ClearHKCapitalMemoryData(acc.UserAccountDistributeLogo, ref listRollback);
                            }
                            if (acc.AccountTypeLogo == (int)Types.AccountType.HKSpotHoldCode)
                            {
                                ClearHKHoldMemoryData(acc.UserAccountDistributeLogo, ref listRollback2);
                            }
                        }
                    }
                    DbHelperSQL.ExecuteCountSql(sb.ToString(), tm);
                    trm.Commit();
                }
                catch (Exception ex)
                {
                    trm.Rollback();
                    //回滚内存表
                    RollbackCapitalMemoryData(ref listRollback);
                    RollbackHoldMemoryData(ref listRollback2);
                    LogHelper.WriteError(ex.Message, ex);
                    return false;
                }
                finally
                {
                    trm.Dispose();
                }
            }
            return true;
        }

        /// <summary>
        /// 现货资金内存表清零
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="listRollback">记录回滚信息的列表</param>
        private void ClearXHCapitalMemoryData(string account, ref  List<CapitalAccountTransInfoEntity> listRollback)
        {
            XHCapitalMemoryTableList xhManager = MemoryDataManager.XHCapitalMemoryList;

            XHCapitalMemoryTable xhFindMemoryData;
            XH_CapitalAccountTableInfo xhFindResult;

            for (int i = 1; i <= 3; i++)
            {
                xhFindMemoryData = xhManager.GetByCapitalAccountAndCurrencyType(account, i);
                if (xhFindMemoryData != null)
                {
                    xhFindResult = xhFindMemoryData.Data;

                    XH_CapitalAccountTable_DeltaInfo delta = new XH_CapitalAccountTable_DeltaInfo();
                    delta.AvailableCapitalDelta = -xhFindResult.AvailableCapital;
                    delta.FreezeCapitalTotalDelta = -xhFindResult.FreezeCapitalTotal;
                    delta.TodayOutInCapital = -xhFindResult.TodayOutInCapital;
                    delta.HasDoneProfitLossTotalDelta = -xhFindResult.HasDoneProfitLossTotal;

                    xhFindMemoryData.AddDeltaToMemory(delta);

                    CapitalAccountTransInfoEntity cap = new CapitalAccountTransInfoEntity();
                    cap.accountType = Types.AccountType.StockSpotCapital;
                    cap.account = account;
                    switch (i)
                    {
                        case 1:
                            cap.currencyType = Types.CurrencyType.RMB;
                            break;
                        case 2:
                            cap.currencyType = Types.CurrencyType.HK;
                            break;
                        case 3:
                            cap.currencyType = Types.CurrencyType.US;
                            break;
                    }
                    cap.AvailableCapitalDelta = delta.AvailableCapitalDelta;
                    cap.FreezeCapitalTotalDelta = delta.FreezeCapitalTotalDelta;
                    cap.TodayOutInCapital = delta.TodayOutInCapital;
                    cap.HasDoneProfitLossTotalDelta = delta.HasDoneProfitLossTotalDelta;
                    listRollback.Add(cap);
                }
            }
        }

        /// <summary>
        /// 现货持仓内存表清零
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="listRollback">记录回滚信息的列表</param>
        private void ClearXHHoldMemoryData(string account, ref  List<HoldAccountTransInfoEntity> listRollback)
        {
            XHHoldMemoryTableList xhManager = MemoryDataManager.XHHoldMemoryList;


            XHHoldMemoryTable xhFindMemoryData;
            XH_AccountHoldTableInfo xhFindResult;

            for (int i = 1; i <= 3; i++)
            {
                IList<int> accIDList = xhManager.GetAccountHoldLogoID(account);
                if (accIDList != null)
                {
                    foreach (int id in accIDList)
                    {
                        xhFindMemoryData = xhManager.GetByAccountHoldLogoId(id);
                        if (xhFindMemoryData != null)
                        {
                            xhFindResult = xhFindMemoryData.Data;

                            XH_AccountHoldTableInfo_Delta delta = new XH_AccountHoldTableInfo_Delta();
                            delta.AvailableAmountDelta = -xhFindResult.AvailableAmount;
                            delta.FreezeAmountDelta = -xhFindResult.FreezeAmount;
                            delta.Data = new XH_AccountHoldTableInfo();
                            delta.Data.AccountHoldLogoId = xhFindResult.AccountHoldLogoId;
                            delta.Data.AvailableAmount = -xhFindResult.AvailableAmount;
                            delta.Data.BreakevenPrice = -xhFindResult.BreakevenPrice;
                            delta.Data.Code = xhFindResult.Code;
                            delta.Data.CostPrice = -xhFindResult.CostPrice;
                            delta.Data.CurrencyTypeId = i;
                            delta.Data.FreezeAmount = -xhFindResult.FreezeAmount;
                            delta.Data.HoldAveragePrice = -xhFindResult.HoldAveragePrice;
                            delta.Data.UserAccountDistributeLogo = xhFindResult.UserAccountDistributeLogo;

                            xhFindMemoryData.AddDeltaToMemory(delta);

                            HoldAccountTransInfoEntity hold = new HoldAccountTransInfoEntity();
                            hold.accountType = Types.AccountType.StockSpotHoldCode;
                            hold.account = account;
                            switch (i)
                            {
                                case 1:
                                    hold.currencyType = Types.CurrencyType.RMB;
                                    break;
                                case 2:
                                    hold.currencyType = Types.CurrencyType.HK;
                                    break;
                                case 3:
                                    hold.currencyType = Types.CurrencyType.US;
                                    break;
                            }
                            hold.AvailableAmountDelta = delta.AvailableAmountDelta;
                            hold.FreezeAmountDelta = delta.FreezeAmountDelta;
                            hold.xhData = delta.Data;
                            listRollback.Add(hold);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 股指期货资金内存表清零
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="listRollback">记录回滚信息的列表</param>
        private void ClearQHCapitalMemoryData(string account, ref  List<CapitalAccountTransInfoEntity> listRollback)
        {
            QHCapitalMemoryTableList qhManager = MemoryDataManager.QHCapitalMemoryList;
            QHCapitalMemoryTable qhFindMemoryData;
            QH_CapitalAccountTableInfo qhFindResult;

            for (int i = 1; i <= 3; i++)
            {
                qhFindMemoryData = qhManager.GetByCapitalAccountAndCurrencyType(account, i);
                if (qhFindMemoryData != null)
                {
                    qhFindResult = qhFindMemoryData.Data;

                    QH_CapitalAccountTable_DeltaInfo delta = new QH_CapitalAccountTable_DeltaInfo();
                    delta.AvailableCapitalDelta = -qhFindResult.AvailableCapital;
                    delta.FreezeCapitalTotalDelta = -qhFindResult.FreezeCapitalTotal;
                    delta.TodayOutInCapitalDelta = -qhFindResult.TodayOutInCapital;
                    delta.MarginTotalDelta = -qhFindResult.MarginTotal;
                    delta.CloseFloatProfitLossTotalDelta = -qhFindResult.CloseFloatProfitLossTotal;
                    delta.CloseMarketProfitLossTotalDelta = -qhFindResult.CloseMarketProfitLossTotal;

                    qhFindMemoryData.AddDeltaToMemory(delta);

                    CapitalAccountTransInfoEntity cap = new CapitalAccountTransInfoEntity();
                    cap.accountType = Types.AccountType.StockFuturesCapital;
                    cap.account = account;
                    switch (i)
                    {
                        case 1:
                            cap.currencyType = Types.CurrencyType.RMB;
                            break;
                        case 2:
                            cap.currencyType = Types.CurrencyType.HK;
                            break;
                        case 3:
                            cap.currencyType = Types.CurrencyType.US;
                            break;
                    }
                    cap.AvailableCapitalDelta = delta.AvailableCapitalDelta;
                    cap.FreezeCapitalTotalDelta = delta.FreezeCapitalTotalDelta;
                    cap.TodayOutInCapitalDelta = delta.TodayOutInCapitalDelta;
                    cap.MarginTotalDelta = delta.MarginTotalDelta;
                    cap.CloseFloatProfitLossTotalDelta = delta.CloseFloatProfitLossTotalDelta;
                    cap.CloseMarketProfitLossTotalDelta = delta.CloseMarketProfitLossTotalDelta;

                    listRollback.Add(cap);
                }
            }
        }

        /// <summary>
        /// 股指期货持仓内存表清零
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="listRollback">记录回滚信息的列表</param>
        private void ClearQHHoldMemoryData(string account, ref  List<HoldAccountTransInfoEntity> listRollback)
        {
            QHHoldMemoryTableList qhManager = MemoryDataManager.QHHoldMemoryList;
            QHHoldMemoryTable qhFindMemoryData;
            QH_HoldAccountTableInfo qhFindResult;

            for (int i = 1; i <= 3; i++)
            {
                IList<int> accIDList = qhManager.GetAccountHoldLogoID(account);
                if (accIDList != null)
                {
                    foreach (int id in accIDList)
                    {
                        qhFindMemoryData = qhManager.GetByAccountHoldLogoId(id);
                        if (qhFindMemoryData != null)
                        {
                            qhFindResult = qhFindMemoryData.Data;

                            QH_HoldAccountTableInfo_Delta delta = new QH_HoldAccountTableInfo_Delta();
                            delta.HistoryFreezeAmountDelta = -qhFindResult.HistoryFreezeAmount;
                            delta.HistoryHoldAmountDelta = -qhFindResult.HistoryHoldAmount;
                            delta.MarginDelta = -qhFindResult.Margin;
                            delta.TodayFreezeAmountDelta = -qhFindResult.TodayFreezeAmount;
                            delta.TodayHoldAmountDelta = -qhFindResult.TodayHoldAmount;
                            delta.Data = new QH_HoldAccountTableInfo();
                            delta.Data.AccountHoldLogoId = qhFindResult.AccountHoldLogoId;
                            delta.Data.BreakevenPrice = -qhFindResult.BreakevenPrice;
                            delta.Data.BuySellTypeId = qhFindResult.BuySellTypeId;
                            delta.Data.Contract = qhFindResult.Contract;
                            delta.Data.CostPrice = -qhFindResult.CostPrice;
                            delta.Data.HistoryFreezeAmount = -qhFindResult.HistoryFreezeAmount;
                            delta.Data.HistoryHoldAmount = -qhFindResult.HistoryHoldAmount;
                            delta.Data.HoldAveragePrice = -qhFindResult.HoldAveragePrice;
                            delta.Data.Margin = -qhFindResult.Margin;
                            delta.Data.OpenAveragePrice = -qhFindResult.OpenAveragePrice;
                            delta.Data.ProfitLoss = -qhFindResult.ProfitLoss;
                            delta.Data.TodayFreezeAmount = -qhFindResult.TodayFreezeAmount;
                            delta.Data.TodayHoldAmount = -qhFindResult.TodayHoldAmount;
                            delta.Data.TodayHoldAveragePrice = -qhFindResult.TodayHoldAveragePrice;
                            delta.Data.TradeCurrencyType = qhFindResult.TradeCurrencyType;
                            delta.Data.UserAccountDistributeLogo = qhFindResult.UserAccountDistributeLogo;

                            qhFindMemoryData.AddDeltaToMemory(delta);

                            HoldAccountTransInfoEntity cap = new HoldAccountTransInfoEntity();
                            cap.accountType = Types.AccountType.StockFuturesHoldCode;
                            cap.account = account;
                            switch (i)
                            {
                                case 1:
                                    cap.currencyType = Types.CurrencyType.RMB;
                                    break;
                                case 2:
                                    cap.currencyType = Types.CurrencyType.HK;
                                    break;
                                case 3:
                                    cap.currencyType = Types.CurrencyType.US;
                                    break;
                            }
                            cap.HistoryFreezeAmountDelta = delta.HistoryFreezeAmountDelta;
                            cap.HistoryHoldAmountDelta = delta.HistoryHoldAmountDelta;
                            cap.MarginDelta = delta.MarginDelta;
                            cap.TodayFreezeAmountDelta = delta.TodayFreezeAmountDelta;
                            cap.TodayHoldAmountDelta = delta.TodayHoldAmountDelta;
                            cap.qhData = delta.Data;
                            listRollback.Add(cap);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 港股资金内存表清零
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="listRollback">记录回滚信息的列表</param>
        private void ClearHKCapitalMemoryData(string account, ref  List<CapitalAccountTransInfoEntity> listRollback)
        {
            HKCapitalMemoryTableList hkManager = MemoryDataManager.HKCapitalMemoryList;
            HKCapitalMemoryTable hkFindMemoryData;
            HK_CapitalAccountInfo hkFindResult;

            for (int i = 1; i <= 3; i++)
            {
                hkFindMemoryData = hkManager.GetByCapitalAccountAndCurrencyType(account, i);
                if (hkFindMemoryData != null)
                {
                    hkFindResult = hkFindMemoryData.Data;

                    HK_CapitalAccount_DeltaInfo delta = new HK_CapitalAccount_DeltaInfo();
                    delta.AvailableCapitalDelta = -hkFindResult.AvailableCapital;
                    delta.FreezeCapitalTotalDelta = -hkFindResult.FreezeCapitalTotal;
                    delta.TodayOutInCapital = -hkFindResult.TodayOutInCapital;
                    delta.HasDoneProfitLossTotalDelta = -hkFindResult.HasDoneProfitLossTotal;

                    hkFindMemoryData.AddDeltaToMemory(delta);

                    CapitalAccountTransInfoEntity cap = new CapitalAccountTransInfoEntity();
                    cap.accountType = Types.AccountType.HKSpotCapital;
                    cap.account = account;
                    switch (i)
                    {
                        case 1:
                            cap.currencyType = Types.CurrencyType.RMB;
                            break;
                        case 2:
                            cap.currencyType = Types.CurrencyType.HK;
                            break;
                        case 3:
                            cap.currencyType = Types.CurrencyType.US;
                            break;
                    }
                    cap.AvailableCapitalDelta = delta.AvailableCapitalDelta;
                    cap.FreezeCapitalTotalDelta = delta.FreezeCapitalTotalDelta;
                    cap.TodayOutInCapital = delta.TodayOutInCapital;
                    cap.HasDoneProfitLossTotalDelta = delta.HasDoneProfitLossTotalDelta;
                    listRollback.Add(cap);
                }
            }
        }

        /// <summary>
        /// 港股持仓内存表清零
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="listRollback">记录回滚信息的列表</param>
        private void ClearHKHoldMemoryData(string account, ref  List<HoldAccountTransInfoEntity> listRollback)
        {
            HKHoldMemoryTableList hkManager = MemoryDataManager.HKHoldMemoryList;
            HKHoldMemoryTable hkFindMemoryData;
            HK_AccountHoldInfo hkFindResult;

            for (int i = 1; i <= 3; i++)
            {
                IList<int> accIDList = hkManager.GetAccountHoldLogoID(account);
                if (accIDList != null)
                {
                    foreach (int id in accIDList)
                    {
                        hkFindMemoryData = hkManager.GetByAccountHoldLogoId(id);
                        if (hkFindMemoryData != null)
                        {
                            hkFindResult = hkFindMemoryData.Data;

                            HK_AccountHoldInfo_Delta delta = new HK_AccountHoldInfo_Delta();
                            delta.AvailableAmountDelta = -hkFindResult.AvailableAmount;
                            delta.FreezeAmountDelta = -hkFindResult.FreezeAmount;
                            delta.Data = new HK_AccountHoldInfo();
                            delta.Data.AccountHoldLogoID = hkFindResult.AccountHoldLogoID;
                            delta.Data.AvailableAmount = -hkFindResult.AvailableAmount;
                            delta.Data.BreakevenPrice = -hkFindResult.BreakevenPrice;
                            delta.Data.Code = hkFindResult.Code;
                            delta.Data.CostPrice = -hkFindResult.CostPrice;
                            delta.Data.CurrencyTypeID = hkFindResult.CurrencyTypeID;
                            delta.Data.FreezeAmount = -hkFindResult.FreezeAmount;
                            delta.Data.HoldAveragePrice = -hkFindResult.HoldAveragePrice;
                            delta.Data.UserAccountDistributeLogo = hkFindResult.UserAccountDistributeLogo;

                            hkFindMemoryData.AddDeltaToMemory(delta);

                            HoldAccountTransInfoEntity cap = new HoldAccountTransInfoEntity();
                            cap.accountType = Types.AccountType.HKSpotCapital;
                            cap.account = account;
                            switch (i)
                            {
                                case 1:
                                    cap.currencyType = Types.CurrencyType.RMB;
                                    break;
                                case 2:
                                    cap.currencyType = Types.CurrencyType.HK;
                                    break;
                                case 3:
                                    cap.currencyType = Types.CurrencyType.US;
                                    break;
                            }
                            cap.AvailableAmountDelta = delta.AvailableAmountDelta;
                            cap.FreezeAmountDelta = delta.FreezeAmountDelta;
                            cap.hkData = delta.Data;
                            listRollback.Add(cap);
                        }
                    }
                }
            }
        }

        #endregion

        #region 验证交易员账户
        /// <summary>
        /// 验证交易员账户
        /// </summary>
        /// <param name="model">要设置的交易员资金实体</param>
        /// <param name="errMsg">操作异常信息</param>
        /// <returns>验证是否通过</returns>
        private bool CheckTradersAccount(CapitalPersonalization model, out string errMsg)
        {
            errMsg = string.Empty;
            string accountTypeLogo = "";
            string accountTypeName = "";
            switch (model.PersonalType)
            {
                case 0:
                    accountTypeLogo = "1,2,4,6,8";
                    accountTypeName = "银行，现货，期货，港股";
                    break;
                case 1:
                    accountTypeLogo = "1";
                    accountTypeName = "银行";
                    break;
                case 2:
                    accountTypeLogo = "2";
                    accountTypeName = "现货";
                    break;
                case 3:
                    accountTypeLogo = "4,6";
                    accountTypeName = "期货";
                    break;
                case 4:
                    accountTypeLogo = "8";
                    accountTypeName = "港股";
                    break;
                case 5:
                    accountTypeLogo = "2,4,6,8";
                    accountTypeName = "现货，期货，港股";
                    break;
            }
            UA_UserAccountAllocationTableDal userAccountDal = new UA_UserAccountAllocationTableDal();
            List<UA_UserAccountAllocationTableInfo> list;
            string findCodition = "";
            foreach (string tradeid in model.TradeID)
            {
                findCodition = string.Format(" UserID='{0}' AND AccountTypeLogo in ({1})", tradeid, accountTypeLogo);
                list = userAccountDal.GetListArray(findCodition);
                if (list == null || list.Count < 1)
                {
                    errMsg = string.Format("交易员{0}的{1}帐号不存在！", tradeid, accountTypeName);
                    return false;
                }
            }
            return true;
        }

        #endregion

        #endregion
    }

}