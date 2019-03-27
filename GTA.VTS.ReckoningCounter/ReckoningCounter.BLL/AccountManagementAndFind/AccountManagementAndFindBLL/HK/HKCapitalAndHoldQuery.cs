using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReckoningCounter.Model;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.Entity.AccountManagementAndFindEntity.HK;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.Entity.Model.QueryFilter;
using ReckoningCounter.DAL.Data.HK;
using GTA.VTS.Common.CommonObject;
//using CommonRealtimeMarket.entity;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.MemoryData;
using ReckoningCounter.MemoryData.XH.Hold;
using ReckoningCounter.Entity.Model;
using ReckoningCounter.MemoryData.HK.Hold;
using ReckoningCounter.MemoryData.HK.Capital;
using ReckoningCounter.DAL.HKTradingRulesService;
using RealTime.Server.SModelData.HqData;

namespace ReckoningCounter.BLL.AccountManagementAndFind.AccountManagementAndFindBLL.HK
{
    /// <summary>
    /// Title:港股资金账户查询与持仓表明细相关查询类
    /// Create BY：李健华
    /// Create Date:2009-10-19
    /// </summary>
    public class HKCapitalAndHoldQuery
    {

        # region  通过港股资金账号和币种获得该资金账号下的该币种的持仓市值
        /// <summary>
        ///  通过港股资金账号和币种获得该资金账号下的该币种的持仓市值
        ///  Update Date:2009-07-15
        ///  Update By:李健华
        ///  Desc.:修改操作数据层方法和相关实体,并增加out总浮动盈亏（未实现盈亏）统计参数
        /// </summary>
        /// <param name="hk_Cap_Acc">港股资金账户</param>
        /// <param name="currencyType">查询交易货币类型</param>
        /// <param name="floatProfitLoss">总浮动盈亏</param>
        /// <returns>返回所有持仓总市值</returns>
        decimal GetMarketValueByHKCapitalAccount(string hk_Cap_Acc, Types.CurrencyType currencyType, out decimal floatProfitLoss)
        {
            #region ==变量定义==
            decimal result = 0;
            floatProfitLoss = 0;
            #endregion

            #region 从缓存中根据港股资金账户获取关联的持仓账户信息
            UA_UserAccountAllocationTableInfo userInfo = AccountManager.Instance.GetHoldAccountByCapitalAccount(hk_Cap_Acc);
            #endregion

            if (userInfo != null)
            {
                result = GetMarketValueByHK_HoldAccount(userInfo.UserAccountDistributeLogo, currencyType, out floatProfitLoss);
            }

            return result;
        }
        /// <summary>
        /// 通过港股持仓账号和币种获得该港股持仓账号下的该币种的港股持仓市值和总浮动盈亏
        /// Create by:李健华
        /// Create Date:2009-07-15
        /// </summary>
        /// <param name="holdAccount">港股持仓账号</param>
        /// <param name="currencyType">交易货币类型</param>
        /// <param name="floatProfitLoss">总浮动盈亏</param>
        /// <returns></returns>
        decimal GetMarketValueByHK_HoldAccount(string holdAccount, Types.CurrencyType currencyType, out decimal floatProfitLoss)
        {
            decimal marketValue = 0;
            floatProfitLoss = 0;
            string errorMsg = "";
            #region 获取当前持仓账户下所有持仓
            List<HK_AccountHoldInfo> list = QueryHK_AccountHoldByHoldAccount(holdAccount, (QueryType.QueryCurrencyType)((int)currencyType), out errorMsg);
            #endregion

            #region 编历所有持仓信息统计
            foreach (HK_AccountHoldInfo item in list)
            {
                string codeStr = item.Code;
                //通过行情服务器获取当前港股行情
                HKStock vhe = CommonDataAgent.RealtimeService.GetHKStockData(codeStr);

                #region 根据商品代码获取撮合（即交易单位）单位转换成计价单位的倍数
                //根据商品代码获取搓合单位
                Types.UnitType utMatch = MCService.GetMatchUnitType(codeStr, Types.BreedClassTypeEnum.HKStock);
                //根据搓合单位转换成计价单位获取得转换的倍数
                decimal unitMultiple = MCService.GetTradeUnitScale(Types.BreedClassTypeEnum.HKStock, codeStr, utMatch);
                #endregion

                # region 获取持仓总量并赋值
                decimal amount = item.AvailableAmount + item.FreezeAmount;
                //把撮合（即交易）单位持仓总量转换为计价单位的持仓总量，因为之前存储到数据库中的与持仓量有关
                //的都是交易单位量，价格相关的都是计价单位。
                amount = amount * unitMultiple;
                #endregion

                # region 获取当前价并赋值 如果获取不到行情数据把当前价以持仓均价来代替计算
                decimal realtimePrice = 0.00M;
                if (vhe != null)
                {
                    if (vhe.Lasttrade == 0)
                    {
                        errorMsg = "【港股资金统计】获取该港股代码的行情最新成交价为0,当前记录使用持仓均价计算";
                        LogHelper.WriteDebug("持仓账号:" + holdAccount + " 代码：" + codeStr + errorMsg);
                        realtimePrice = item.HoldAveragePrice;
                    }
                    else
                    {
                        realtimePrice = Convert.ToDecimal(vhe.Lasttrade);
                    }
                }
                else
                {
                    errorMsg = "【港股资金统计】未获取到该港股代码的行情,当前记录使用持仓均价计算.";
                    LogHelper.WriteDebug("持仓账号:" + holdAccount + " 代码：" + codeStr + errorMsg);
                    realtimePrice = item.HoldAveragePrice;

                }

                #endregion

                #region 统计总浮动盈亏
                //浮动盈亏=持仓总量*（当前价-持仓均价）
                floatProfitLoss += amount * (realtimePrice - item.HoldAveragePrice);
                #endregion

                #region 总市值
                marketValue += amount * realtimePrice;
                #endregion

            }
            #endregion

            return marketValue;
        }
        # endregion

        #region 根据港股资金账号和币种查询港股资金明细
        /// <summary>
        /// 根据港股资金账号和币种查询港股资金明细统计（包含持仓盈亏统计)
        /// </summary>
        /// <param name="hk_Cap_Acc">港股资金账号</param>
        /// <param name="currencyType">货币类型</param>
        /// <param name="userPassword">用户密码</param>
        /// <param name="strErrorMessage">异常信息</param>
        /// <returns></returns>
        public HKCapitalEntity QueryHKCapitalDetail(string hk_Cap_Acc, Types.CurrencyType currencyType, string userPassword, ref string strErrorMessage)
        {
            HKCapitalEntity result = null;
            try
            {
                List<HK_CapitalAccountInfo> capitalAccount = QueryHK_CapitalAccountByAccount(hk_Cap_Acc, (QueryType.QueryCurrencyType)((int)currencyType), out strErrorMessage);
                if (!Utils.IsNullOrEmpty(capitalAccount))
                {
                    HK_CapitalAccountInfo ca = capitalAccount[0];

                    #region 获取交易货币类型名称
                    CM_CurrencyType cmCurrencyType = MCService.CommonPara.GetCurrencyTypeByID(ca.TradeCurrencyType);
                    string cName = "";
                    if (cmCurrencyType != null)
                    {
                        cName = cmCurrencyType.CurrencyName;
                    }
                    #endregion

                    decimal notDoneProfitLossTotal = 0;//未实现盈亏
                    decimal marketValue = GetMarketValueByHKCapitalAccount(hk_Cap_Acc, currencyType, out notDoneProfitLossTotal);
                    result = new HKCapitalEntity(ca, marketValue, cName, notDoneProfitLossTotal);
                }
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
                LogHelper.WriteError("港股资金明细查询QueryHKCapitalDetail()异常：" + hk_Cap_Acc + "   CurrencyType:" + (int)currencyType, ex);
            }
            return result;
        }
        #endregion

        #region 根据【港股持仓账号】查询港股持仓账号明细
        /// <summary>
        /// 根据【港股持仓账号】查询港股持仓账号明细
        /// </summary>
        ///<param name="hk_hold_Account">港股持仓账号</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<HK_AccountHoldInfo> QueryHK_AccountHoldByHoldAccount(string hk_hold_Account, QueryType.QueryCurrencyType currencyType, out string errorMsg)
        {
            errorMsg = "";
            List<HK_AccountHoldInfo> list = new List<HK_AccountHoldInfo>();

            #region 从数据库中获取数据
            //try
            //{
            //    HK_AccountHoldDal dal = new HK_AccountHoldDal();
            //    list = dal.GetListByAccount(hk_hold_Account, currencyType);
            //}
            //catch (Exception ex)
            //{
            //    errorMsg = ex.Message;
            //    LogHelper.WriteError(ex.ToString(), ex);
            //}
            #endregion

            #region 从内存表中获取数据
            try
            {
                list = GetHK_AccountHoldListFromMemory(hk_hold_Account, currencyType, out errorMsg);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            #endregion
            return list;
        }
        #endregion

        #region 根据港股持仓账号和查询交易货币类型从内存表中获取港股持仓表数据列表
        /// <summary>
        /// 根据港股持仓账号和查询交易货币类型从内存表中获取港股持仓表数据列表
        /// </summary>
        /// <param name="hk_Hold_Account">港股持仓账号</param>
        /// <param name="currencyType">交易货币类型</param>
        /// <param name="errorMsg">查询异常</param>
        /// <returns></returns>
        List<HK_AccountHoldInfo> GetHK_AccountHoldListFromMemory(string hk_Hold_Account, QueryType.QueryCurrencyType currencyType, out string errorMsg)
        {
            errorMsg = "";
            List<HK_AccountHoldInfo> list = new List<HK_AccountHoldInfo>();
            if (!ServerConfig.IsLoadAllData)
            {
                #region 没有加载缓存持仓，从数据库中查询
                try
                {
                    //string messStr = "GetHK_AccountHoldListFromMemory===正在查询港股账号" + hk_Hold_Account + "的持仓,因为没有加载持仓表数据正在从数据库中查询，查询时间" + DateTime.Now.ToString();
                    //LogHelper.WriteInfo(messStr);
                    HK_AccountHoldDal dal = new HK_AccountHoldDal();
                    list = dal.GetListByAccount(hk_Hold_Account, currencyType);
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    LogHelper.WriteError(ex.ToString(), ex);
                }
                #endregion
            }
            else
            {
                #region 直接从缓存中查询
                var manager = MemoryDataManager.HKHoldMemoryList;
                if (manager == null)
                {
                    errorMsg = "还没有初始化缓存数据对象！";
                    return list;
                }
                IList<int> listID = manager.GetAccountHoldLogoID(hk_Hold_Account);
                if (Utils.IsNullOrEmpty(listID))
                {
                    return list;
                }
                if (currencyType == QueryType.QueryCurrencyType.ALL)
                {
                    foreach (int item in listID)
                    {
                        HKHoldMemoryTable table = manager.GetByAccountHoldLogoId(item);
                        if (table != null)
                        {
                            var info = table.Data;
                            list.Add(info);
                        }
                    }
                }
                else
                {
                    foreach (int item in listID)
                    {

                        HKHoldMemoryTable table = manager.GetByAccountHoldLogoId(item);
                        if (table != null)
                        {
                            var info = table.Data;
                            if (info.CurrencyTypeID == (int)currencyType)
                            {
                                list.Add(info);
                            }
                        }
                    }
                }
                #endregion
            }

            return list;
        }
        #endregion

        # region 港股持仓查询 （根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// <summary>
        /// 港股持仓查询 （根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        ///  Update Date:2009-10-19
        ///  Update By:李健华
        /// </summary>
        /// <param name="hk_cap_Acc">港股资金账号</param>
        /// <param name="pwd">交易员密码</param>
        /// <param name="filter">查询条件实体对象</param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public List<HKHoldFindResultyEntity> QueryHKHoldSummary(string hk_cap_Acc, string pwd, HKHoldConditionFindEntity filter, int start, int pageLength, out int count, ref string errMsg)
        {
            List<HK_AccountHoldInfo> tempt = null;
            List<HKHoldFindResultyEntity> result = new List<HKHoldFindResultyEntity>();
            count = 0;
            try
            {

                #region 根据港股资金账户获取关联的持仓账户信息
                UA_UserAccountAllocationTableInfo userInfo = AccountManager.Instance.GetHoldAccountByCapitalAccount(hk_cap_Acc);
                if (userInfo == null)
                {
                    errMsg = "查询不到相关账户信息";
                    return result;
                }
                #endregion

                #region 组装条件
                string whereCondition = "";
                if (filter != null)
                {
                    //将股东代码（持仓账号）加入查询条件中
                    filter.HKHoldAccount = userInfo.UserAccountDistributeLogo;
                    whereCondition = BuildHKHoldQueryWhere(filter);
                }
                else
                {
                    whereCondition = string.Format(" UserAccountDistributeLogo='{0}'", userInfo.UserAccountDistributeLogo);
                }
                #endregion

                #region 分页查询相关信息
                PagingProceduresInfo prcoInfo = new PagingProceduresInfo();
                if (start <= 1)
                {
                    prcoInfo.IsCount = false;
                }
                {
                    prcoInfo.IsCount = true;
                }
                prcoInfo.PageNumber = start;
                prcoInfo.PageSize = pageLength;
                prcoInfo.Fields = "   AccountHoldLogoID,UserAccountDistributeLogo,CurrencyTypeID,Code,AvailableAmount,FreezeAmount,CostPrice,BreakevenPrice ,HoldAveragePrice ";
                prcoInfo.PK = "AccountHoldLogoID";
                prcoInfo.Sort = " AvailableAmount asc ";
                prcoInfo.Tables = " HK_AccountHold ";

                #region 组装相关条件
                prcoInfo.Filter = whereCondition;
                #endregion


                #endregion

                HK_AccountHoldDal xh_AccHolDal = new HK_AccountHoldDal();
                CommonDALOperate<HK_AccountHoldInfo> com = new CommonDALOperate<HK_AccountHoldInfo>();
                tempt = com.PagingQueryProcedures(prcoInfo, out count, xh_AccHolDal.ReaderBind);


                foreach (HK_AccountHoldInfo _hkAccountHold in tempt)
                {
                    HKHoldFindResultyEntity hkfs = new HKHoldFindResultyEntity();
                    hkfs.HoldFindResult = _hkAccountHold;

                    #region 根据商品代码获取商品对象实体
                    HK_Commodity cm_Commodity = MCService.HKTradeRulesProxy.GetHKCommodityByCommodityCode(_hkAccountHold.Code);
                    #endregion

                    # region  获取品种类别并赋值

                    if (cm_Commodity != null)
                    {
                        if (filter != null && !string.IsNullOrEmpty(filter.VarietyCode))
                        {
                            if (cm_Commodity.BreedClassID.Value.ToString().Trim() != filter.VarietyCode.Trim())
                            {
                                continue;
                            }
                        }
                        hkfs.VarietyCategories = cm_Commodity.BreedClassID.Value.ToString();
                    }

                    # endregion

                    # region  获取所属市场并赋值
                    //所属市场
                    CM_BourseType cm_BourseType = MCService.CommonPara.GetBourseTypeByCommodityCode(_hkAccountHold.Code, Types.BreedClassTypeEnum.HKStock);
                    if (cm_BourseType != null)
                    {
                        hkfs.BelongMarket = cm_BourseType.BourseTypeName;
                    }
                    else
                    {
                        errMsg = "根据商品代码未获取到商品所属市场 ！";
                    }
                    # endregion

                    # region  获取港股名称并赋值
                    //港股名称
                    if (cm_Commodity != null)
                    {
                        hkfs.HKName = cm_Commodity.HKCommodityName;
                    }
                    else
                    {
                        errMsg = "根据商品代码未获取到港股名称 ！";
                    }
                    # endregion

                    # region 获取币种名称并赋值
                    string _currencyName = MCService.CommonPara.GetCurrencyTypeByID(_hkAccountHold.CurrencyTypeID).CurrencyName;
                    if (!string.IsNullOrEmpty(_currencyName))
                    {
                        hkfs.CurrencyName = _currencyName;
                    }
                    else
                    {
                        errMsg = "港股持仓表中的币种类型存储错误！";
                    }
                    # endregion

                    # region 获取持仓总量并赋值
                    hkfs.HoldSumAmount = Convert.ToInt32(_hkAccountHold.AvailableAmount + _hkAccountHold.FreezeAmount);
                    # endregion

                    # region 获取当前价并赋值
                    HKStock vhe = CommonDataAgent.RealtimeService.GetHKStockData(_hkAccountHold.Code);
                    if (vhe != null)
                    {
                        if (vhe.Lasttrade == 0)
                        {
                            errMsg = "【港股持仓统计】获取该港股代码的行情最新成交价为0,当前记录使用持仓均价计算.";
                            LogHelper.WriteDebug("持仓账号:" + _hkAccountHold.UserAccountDistributeLogo + " 代码：" + _hkAccountHold.Code + errMsg);
                            hkfs.RealtimePrice = _hkAccountHold.HoldAveragePrice;
                        }
                        else
                        {
                            hkfs.RealtimePrice = Convert.ToDecimal(vhe.Lasttrade);
                        }
                    }
                    else
                    {
                        errMsg = "【港股持仓统计】未获取到该港股代码的行情,当前记录使用持仓均价计算.";
                        LogHelper.WriteDebug("持仓账号:" + _hkAccountHold.UserAccountDistributeLogo + " 代码：" + _hkAccountHold.Code + errMsg);
                        hkfs.RealtimePrice = _hkAccountHold.HoldAveragePrice;

                    }
                    # endregion

                    #region 根据商品代码获取撮合（即交易单位）单位转换成计价单位的倍数
                    //根据商品代码获取搓合单位
                    Types.UnitType utMatch = MCService.GetMatchUnitType(_hkAccountHold.Code, Types.BreedClassTypeEnum.HKStock);
                    //根据搓合单位转换成计价单位获取得转换的倍数
                    decimal unitMultiple = MCService.GetTradeUnitScale(Types.BreedClassTypeEnum.HKStock, _hkAccountHold.Code, utMatch);
                    #endregion

                    # region 获取市值并赋值
                    //因为持仓记录的是撮合单位量（即交易量）所以要转换成计价单位量才正确
                    //sfre.MarketValue = sfre.HoldSumAmount * sfre.RealtimePrice;
                    hkfs.MarketValue = hkfs.HoldSumAmount * unitMultiple * hkfs.RealtimePrice;
                    # endregion

                    # region 获取浮动盈亏并赋值
                    //浮动盈亏=持仓总量*（当前价-持仓均价）
                    //因为持仓记录的是撮合单位量（即交易量）所以要转换成计价单位量才正确
                    //sfre.FloatProfitLoss = sfre.HoldSumAmount * (sfre.RealtimePrice - _XhAccountHold.HoldAveragePrice);
                    hkfs.FloatProfitLoss = hkfs.HoldSumAmount * unitMultiple * (hkfs.RealtimePrice - _hkAccountHold.HoldAveragePrice);
                    # endregion

                    # region 获取交易员ID并赋值
                    hkfs.TraderId = userInfo.UserID;
                    # endregion

                    # region 获取错误原因并赋值
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        hkfs.ErroReason = errMsg;
                    }
                    # endregion

                    # region 获取错误号并赋值（还未实现，暂时赋为空值）
                    hkfs.ErroNumber = string.Empty;
                    # endregion

                    result.Add(hkfs);
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                LogHelper.WriteError("ReckoningCounter.BLL.AccountManagementAndFind.HKCapitalAndHoldQuery--QueryHKHoldSummary()查询异常", ex);
            }
            return result;
        }

        # endregion 港股持仓查询

        # region 港股持仓查询过滤条件
        /// <summary>
        /// 港股持仓查询过滤条件
        ///  Create Date:2009-10-19
        ///  Create By:李健华
        /// </summary>
        /// <param name="_findConditions">过滤条件实体对象</param>
        /// <returns></returns>
        private string BuildHKHoldQueryWhere(HKHoldConditionFindEntity _findConditions)
        {
            string result = "1=1 ";

            # region  0.股东代码
            //0.股东代码
            if (!string.IsNullOrEmpty(_findConditions.HKHoldAccount))
            {
                result += string.Format(" And UserAccountDistributeLogo='{0}'", _findConditions.HKHoldAccount);
            }
            # endregion

            # region 2.币种赋值
            if (_findConditions.CurrencyType != 0)
            {
                result += string.Format(" AND CurrencyTypeID='{0}'", _findConditions.CurrencyType);
            }
            # endregion

            # region 3.港股代码
            if (!string.IsNullOrEmpty(_findConditions.HKCode))
            {
                result += string.Format(" AND Code='{0}'", _findConditions.HKCode);
            }
            # endregion
            return result;
        }
        # endregion 港股持仓查询过滤条件

        #region 根据【用户ID查询】用户所拥有的港股持仓账号明细
        /// <summary>
        /// 根据【用户ID查询】用户所拥有的public货持仓账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的港股持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="currencytype">要查询的货币类型</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<HK_AccountHoldInfo> QueryHK_AccountHoldByUserID(string userID, int accountType, QueryType.QueryCurrencyType currencytype, out string errorMsg)
        {
            errorMsg = "";
            List<HK_AccountHoldInfo> list = new List<HK_AccountHoldInfo>();

            #region 从数据库中取数据
            //try
            //{ 
            //    HK_AccountHoldDal dal = new HK_AccountHoldDal();
            //    list = dal.GetListByUserID(userID, type);
            //}
            //catch (Exception ex)
            //{
            //    errorMsg = ex.Message;
            //    LogHelper.WriteError(ex.ToString(), ex);
            //}
            #endregion
            try
            {
                #region 先通过用户ID取得用户的港股持仓账号
                List<UA_UserAccountAllocationTableInfo> userAccountInfo = new List<UA_UserAccountAllocationTableInfo>();

                #region 如果为0就查询类别下的所有可能有两个账号

                #region 从缓存中获取账号
                userAccountInfo = AccountManager.Instance.GetUserHoldAccountFormUserCache(userID, accountType, 1);
                #endregion

                #region 直接从数据库中获取
                //if (accountType == 0)
                //{
                //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountTypeClass(userID, Types.AccountAttributionType.SpotHold);
                //}
                //else
                //{
                //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountType(userID, accountType);
                //}
                #endregion
                #endregion
                #endregion

                #region 从内存表中获取数据
                foreach (UA_UserAccountAllocationTableInfo item in userAccountInfo)
                {
                    list.AddRange(GetHK_AccountHoldListFromMemory(item.UserAccountDistributeLogo, currencytype, out errorMsg));
                }
                #endregion
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;

        }
        #endregion

        #region 根据【用户ID和密码】查询用户所拥有的港股持仓账号明细
        /// <summary>
        /// 根据【用户ID和密码】查询用户所拥有的港股持仓账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的港股持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="currencytype">要查询的货币类型</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<HK_AccountHoldInfo> QueryHK_AccountHoldByUserIDAndPwd(string userID, string pwd, int accountType, QueryType.QueryCurrencyType currencytype, out string errorMsg)
        {
            errorMsg = "";
            List<HK_AccountHoldInfo> list = new List<HK_AccountHoldInfo>();

            #region 从数据库中获取数据
            //try
            //{
            //    HK_AccountHoldDal dal = new HK_AccountHoldDal();
            //    list = dal.GetListByUserIDAndPwd(userID, pwd, type);
            //}
            //catch (Exception ex)
            //{
            //    errorMsg = ex.Message;
            //    LogHelper.WriteError(ex.ToString(), ex);
            //}
            #endregion
            try
            {
                #region 检查用户密码

                //update 2009-11-25 李健华
                //var userDal = new UA_UserBasicInformationTableDal();
                //if (!userDal.Exists(userID, pwd))
                //{
                //    errorMsg = "用户密码不正确，或者没有此用户相关信息！";
                //    return list;
                //}
                //==============
                //====new===
                if (!AccountManager.Instance.ExistsBasicUserPWDByUserId(userID, pwd))
                {
                    errorMsg = "用户密码不正确，或者没有此用户相关信息！";
                    return list;
                }
                //==========end==========

                #endregion

                #region 先通过用户ID取得用户的港股持仓资金账号
                List<UA_UserAccountAllocationTableInfo> userAccountInfo = new List<UA_UserAccountAllocationTableInfo>();
                #region 如果为0就查询类别下的所有可能有两个账号
                #region 从缓存中获取账号
                userAccountInfo = AccountManager.Instance.GetUserHoldAccountFormUserCache(userID, accountType, 1);
                #endregion
                #endregion

                #region 直接从数据库中获取
                //if (accountType == 0)
                //{
                //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountTypeClass(userID, Types.AccountAttributionType.SpotHold);
                //}
                //else
                //{
                //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountType(userID, accountType);
                //}
                #endregion
                #endregion

                #region 从内存表中获取数据
                foreach (UA_UserAccountAllocationTableInfo item in userAccountInfo)
                {
                    list.AddRange(GetHK_AccountHoldListFromMemory(item.UserAccountDistributeLogo, currencytype, out errorMsg));
                }
                #endregion

            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }

            return list;
        }

        #endregion

        #region 根据【港股持仓账号】查询港股持仓账号明细
        /// <summary>
        /// 根据【港股持仓账号】查询港股持仓账号明细
        /// </summary>
        ///<param name="xh_hold_Account">港股持仓账号</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<HK_AccountHoldInfo> QueryHK_AccountHoldByHoldAccountAndCurr_Type(string hk_hold_Account, QueryType.QueryCurrencyType currencyType, out string errorMsg)
        {
            errorMsg = "";
            List<HK_AccountHoldInfo> list = new List<HK_AccountHoldInfo>();

            #region 从数据库中获取数据
            //try
            //{ 
            //    HK_AccountHoldDal dal = new HK_AccountHoldDal();
            //    list = dal.GetListByAccount(xh_Cap_Account, type);
            //}
            //catch (Exception ex)
            //{
            //    errorMsg = ex.Message;
            //    LogHelper.WriteError(ex.ToString(), ex);
            //}
            #endregion

            #region 从内存表中获取数据
            list = GetHK_AccountHoldListFromMemory(hk_hold_Account, currencyType, out errorMsg);
            #endregion
            return list;
        }
        #endregion

        #region 根据委托编号查询【港股持仓冻结表】明细
        /// <summary>
        /// 根据委托编号查询【港股持仓冻结表】明细
        /// </summary>
        /// <param name="entrustNo">委托编号</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public List<HK_AcccountHoldFreezeInfo> QueryHK_AcccountHoldFreezeByEntrustNo(string entrustNo, QueryType.QueryFreezeType freezeType, out string errorMsg)
        {
            errorMsg = "";
            List<HK_AcccountHoldFreezeInfo> list = null;
            if (string.IsNullOrEmpty(entrustNo))
            {
                errorMsg = "委托编号不能为空！";
                return list;
            }
            HK_AcccountHoldFreezeDal dal = new HK_AcccountHoldFreezeDal();
            try
            {
                StringBuilder sb = new StringBuilder("  EntrustNumber='" + entrustNo.Trim() + "'");
                //如果查询的冻结类型不是查询所有时加上条件
                if (freezeType != QueryType.QueryFreezeType.ALL)
                {
                    sb.AppendFormat("  AND FreezeTypeID='{0}'", (int)freezeType);
                }
                list = dal.GetListArray(sb.ToString());
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
        }
        #endregion

        #region 根据用户持仓账号和查询的交易的货币类型、查询时间段查询【港股持仓冻结表】明细信息
        /// <summary>
        /// Title：根据用户持仓账号和查询的交易的货币类型、查询时间段查询【港股持仓冻结表】明细信息
        /// Desc.：如果开始时间或者结束时间为null即返回当前时间向前一个月（30天）的时间段查询
        ///        如果开始时间大于结束时间即返回当前时间向前一个月（30天）的时间段查询
        /// </summary>
        /// <param name="account">持仓账号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<HK_AcccountHoldFreezeInfo> PagingQueryHK_AcccountHoldFreezeByHoldAccount(string holdAccount, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, QueryType.QueryFreezeType freezeType, PagingInfo pageInfo, out int total, out string errorMsg)
        {
            #region 初始货变量
            List<HK_AcccountHoldFreezeInfo> list = null;
            HK_AcccountHoldFreezeDal dal = new HK_AcccountHoldFreezeDal();
            errorMsg = "";
            total = 0;
            #endregion

            #region 如果持仓账号或者分页对象实体为空直接返错误传递参数提示 列表为null
            if (string.IsNullOrEmpty(holdAccount))
            {
                errorMsg = "持仓账号不能为空！";
                return list;
            }
            if (pageInfo == null)
            {
                errorMsg = "分页信息不能为空!";
                return null;
            }
            #endregion

            #region 分页查询相关信息
            PagingProceduresInfo prcoInfo = new PagingProceduresInfo();
            prcoInfo.IsCount = pageInfo.IsCount;
            prcoInfo.PageNumber = pageInfo.CurrentPage;
            prcoInfo.PageSize = pageInfo.PageLength;
            prcoInfo.Fields = "  f.HoldFreezeLogoId,f.EntrustNumber,f.prepareFreezeAmount,f.FreezeTypeID,f.AccountHoldLogo,f.ThawTime,f.FreezeTime ";
            prcoInfo.PK = "f.HoldFreezeLogoId";
            if (pageInfo.Sort == 0)
            {
                prcoInfo.Sort = " f.FreezeTime asc ";
            }
            else
            {
                prcoInfo.Sort = " f.FreezeTime desc ";
            }
            prcoInfo.Tables = " HK_AcccountHoldFreeze as f  inner join HK_AccountHold as a on a.AccountHoldLogoId=f.AccountHoldLogo";

            #region 组装相关条件
            StringBuilder sb = new StringBuilder();
            //1=1 and a.UserAccountDistributeLogo='010000000406' and a.TradeCurrencyType=1
            sb.Append(" a.UserAccountDistributeLogo='" + holdAccount + "'  ");

            if (currencyType != QueryType.QueryCurrencyType.ALL)
            {
                sb.Append(" and a.CurrencyTypeID='" + (int)currencyType + "'");
            }
            sb.AppendFormat(CommonDALBulidSQLScript.BuildWhereQueryBetwennTime(startTime, endTime, 30), "f.FreezeTime");
            //如果查询的冻结类型不是查询所有时加上条件
            if (freezeType != QueryType.QueryFreezeType.ALL)
            {
                sb.AppendFormat("  AND f.FreezeTypeID='{0}'", (int)freezeType);
            }
            #endregion

            prcoInfo.Filter = sb.ToString();
            #endregion

            #region 执行查询
            try
            {
                CommonDALOperate<HK_AcccountHoldFreezeInfo> com = new CommonDALOperate<HK_AcccountHoldFreezeInfo>();
                list = com.PagingQueryProcedures(prcoInfo, out total, dal.ReaderBind);
                //list = dal.PagingXH_AcccountHoldFreezeByFilter(prcoInfo, out pageTotal);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
            #endregion

        }

        #endregion

        #region 根据用户ID和查询的交易的货币类型、查询时间段查询【港股持仓冻结表】明细
        /// <summary>
        /// Title：根据用户ID和查询的交易的货币类型、查询时间段查询【港股持仓冻结表】明细信息
        /// Desc.：如果开始时间或者结束时间为null即返回当前时间向前一个月（30天）的时间段查询
        ///        如果开始时间大于结束时间即返回当前时间向前一个月（30天）的时间段查询
        /// </summary>
        /// <param name="userID">持仓账号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的港股持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="currencytype">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<HK_AcccountHoldFreezeInfo> PagingQueryHK_AcccountHoldFreezeByUserID(string userID, int accountType, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, QueryType.QueryFreezeType freezeType, PagingInfo pageInfo, out int total, out string errorMsg)
        {
            #region 初始货变量
            List<HK_AcccountHoldFreezeInfo> list = null;
            HK_AcccountHoldFreezeDal dal = new HK_AcccountHoldFreezeDal();
            errorMsg = "";
            total = 0;
            #endregion

            #region 如果用户ID或者分页对象实体为空直接返错误传递参数提示 列表为null
            if (string.IsNullOrEmpty(userID))
            {
                errorMsg = "用户ID不能为空！";
                return list;
            }
            //如果日后要加上用户和密码验证在此加上
            if (pageInfo == null)
            {
                errorMsg = "分页信息不能为空!";
                return null;
            }
            #endregion

            #region 分页查询相关信息
            PagingProceduresInfo prcoInfo = new PagingProceduresInfo();
            prcoInfo.IsCount = pageInfo.IsCount;
            prcoInfo.PageNumber = pageInfo.CurrentPage;
            prcoInfo.PageSize = pageInfo.PageLength;
            prcoInfo.Fields = "  f.HoldFreezeLogoId,f.EntrustNumber,f.prepareFreezeAmount,f.FreezeTypeID,f.AccountHoldLogo,f.ThawTime,f.FreezeTime ";
            prcoInfo.PK = "f.HoldFreezeLogoId";
            if (pageInfo.Sort == 0)
            {
                prcoInfo.Sort = " f.FreezeTime asc ";
            }
            else
            {
                prcoInfo.Sort = " f.FreezeTime desc ";
            }
            prcoInfo.Tables = "HK_AcccountHoldFreeze as f  inner join HK_AccountHold as a on a.AccountHoldLogoId=f.AccountHoldLogo";

            #region 组装相关条件
            StringBuilder sb = new StringBuilder();

            #region 从缓存中获取账号

            List<UA_UserAccountAllocationTableInfo> userAccountInfo = AccountManager.Instance.GetUserHoldAccountFormUserCache(userID, accountType, 1);
            #region 添加数据
            string userIDstr = "";
            foreach (UA_UserAccountAllocationTableInfo item in userAccountInfo)
            {
                userIDstr += ",   '" + item.UserAccountDistributeLogo + "'";
            }
            if (!string.IsNullOrEmpty(userIDstr))
            {
                userIDstr = userIDstr.Substring(userIDstr.IndexOf(',') + 1);
            }

            #endregion
            #endregion

            #region 直接从数据库中获取
            if (!string.IsNullOrEmpty(userIDstr))
            {
                if (userIDstr.Split(',').Length > 1)
                {
                    sb.AppendFormat("  a.UserAccountDistributeLogo  in ( {0} )", userIDstr);
                }
                else
                {
                    sb.AppendFormat("  a.UserAccountDistributeLogo = {0} ", userIDstr);
                }
            }
            else //如果在缓存中获取不到数据直接从数据库中获取数据
            {

                //找不到用户不在数据库中查找了,这里不再管了
                sb.Append("  a.UserAccountDistributeLogo=''  ");//这里加这个是为了后面的条件
                //if (accountType == 0)
                //{
                //    sb.Append("  a.UserAccountDistributeLogo  in ( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable  where accounttypelogo in ( select accounttypelogo from BD_AccountType where atcid='" + (int)CommonObject.Types.AccountAttributionType.SpotHold + "')  and userid='" + userID + "' )");
                //}
                //else
                //{
                //    sb.Append("  a.UserAccountDistributeLogo  in ( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable  where accounttypelogo='" + accountType + "' and userid='" + userID + "' )");
                //}
            }
            #endregion

            if (currencyType != QueryType.QueryCurrencyType.ALL)
            {
                sb.Append(" and a.CurrencyTypeID='" + (int)currencyType + "'");
            }
            sb.AppendFormat(CommonDALBulidSQLScript.BuildWhereQueryBetwennTime(startTime, endTime, 30), "f.FreezeTime");
            //如果查询的冻结类型不是查询所有时加上条件
            if (freezeType != QueryType.QueryFreezeType.ALL)
            {
                sb.AppendFormat("  AND f.FreezeTypeID='{0}'", (int)freezeType);
            }
            #endregion

            prcoInfo.Filter = sb.ToString();
            #endregion

            #region 执行查询
            try
            {
                CommonDALOperate<HK_AcccountHoldFreezeInfo> com = new CommonDALOperate<HK_AcccountHoldFreezeInfo>();
                list = com.PagingQueryProcedures(prcoInfo, out total, dal.ReaderBind);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
            #endregion
        }
        #endregion

        #region 根据【用户ID查询】用户所拥有的港股资金账号明细
        /// <summary>
        /// 根据【用户ID查询】用户所拥有的public货资金账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的港股持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<HK_CapitalAccountInfo> QueryHK_CapitalAccountByUserID(string userID, int accountType, QueryType.QueryCurrencyType currencyType, out string errorMsg)
        {
            errorMsg = "";
            List<HK_CapitalAccountInfo> list = new List<HK_CapitalAccountInfo>();
            #region 从数据库中获取数据
            //try
            //{
            //    XH_CapitalAccountTableDal dal = new XH_CapitalAccountTableDal();
            //    list = dal.GetListByUserID(userID, type);
            //}
            //catch (Exception ex)
            //{
            //    errorMsg = ex.Message;
            //    LogHelper.WriteError(ex.ToString(), ex);
            //}
            #endregion
            try
            {
                #region 先通过用户ID取得用户的港股资金账号
                //UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
                List<UA_UserAccountAllocationTableInfo> userAccountInfo = new List<UA_UserAccountAllocationTableInfo>();
                #region 从缓存中获取资金账号
                userAccountInfo = AccountManager.Instance.GetUserCapitalAccountFormUserCache(userID, accountType, 1);
                #endregion
                #region 从数据库中获取资金账号
                //if (accountType == 0)
                //{
                //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountTypeClass(userID, Types.AccountAttributionType.SpotCapital);
                //}
                //else
                //{
                //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountType(userID, accountType);
                //}
                #endregion
                #endregion
                #region 从内存表中获取数据
                foreach (UA_UserAccountAllocationTableInfo item in userAccountInfo)
                {
                    list.AddRange(GetHK_CapitalAccountListFromMemory(item.UserAccountDistributeLogo, currencyType, out errorMsg));
                }
                #endregion
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;

        }
        #endregion

        #region 根据【用户ID和密码】查询用户所拥有的港股资金账号明细
        /// <summary>
        /// 根据【用户ID和密码】查询用户所拥有的港股资金账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的港股持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<HK_CapitalAccountInfo> QueryHK_CapitalAccountByUserIDAndPwd(string userID, string pwd, int accountType, QueryType.QueryCurrencyType currencyType, out string errorMsg)
        {
            errorMsg = "";
            List<HK_CapitalAccountInfo> list = new List<HK_CapitalAccountInfo>();
            #region 从数据库中获取数据
            //try
            // { 
            //     XH_CapitalAccountTableDal dal = new XH_CapitalAccountTableDal();
            //     list = dal.GetListByUserIDAndPwd(userID, pwd, type);
            // }
            // catch (Exception ex)
            // {
            //     errorMsg = ex.Message;
            //     LogHelper.WriteError(ex.ToString(), ex);
            // }
            #endregion

            try
            {
                #region 检查用户密码
                //update 2009-11-25 李健华
                //var userDal = new UA_UserBasicInformationTableDal();
                //if (!userDal.Exists(userID, pwd))
                //{
                //    errorMsg = "用户密码不正确，或者没有此用户相关信息！";
                //    return list;
                //}
                //==============
                //====new===
                if (!AccountManager.Instance.ExistsBasicUserPWDByUserId(userID, pwd))
                {
                    errorMsg = "用户密码不正确，或者没有此用户相关信息！";
                    return list;
                }
                //==========end==========

                #endregion

                #region 先通过用户ID取得用户的港股资金账号
                //UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
                List<UA_UserAccountAllocationTableInfo> userAccountInfo = new List<UA_UserAccountAllocationTableInfo>();
                #region 从缓存中获取资金账号
                userAccountInfo = AccountManager.Instance.GetUserCapitalAccountFormUserCache(userID, accountType, 1);
                #endregion
                #region 从数据库中获取资金账号
                //if (accountType == 0)
                //{
                //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountTypeClass(userID, Types.AccountAttributionType.SpotCapital);
                //}
                //else
                //{
                //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountType(userID, accountType);
                //}
                #endregion
                #endregion

                #region 从内存表中获取数据

                foreach (UA_UserAccountAllocationTableInfo item in userAccountInfo)
                {
                    list.AddRange(GetHK_CapitalAccountListFromMemory(item.UserAccountDistributeLogo, currencyType, out errorMsg));
                }
                #endregion
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
        }

        #endregion

        #region 根据资金账号和查询交易货币类型从内存表中获取港股资金数据列表
        /// <summary>
        /// 根据资金账号和查询交易货币类型从内存表中获取数据列表
        /// </summary>
        /// <param name="hk_Cap_Account">资金账号</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errorMsg">查询异常</param>
        /// <returns></returns>
        List<HK_CapitalAccountInfo> GetHK_CapitalAccountListFromMemory(string hk_Cap_Account, QueryType.QueryCurrencyType currencyType, out string errorMsg)
        {
            errorMsg = "";
            List<HK_CapitalAccountInfo> list = new List<HK_CapitalAccountInfo>();
            var manager = MemoryDataManager.HKCapitalMemoryList;
            if (manager == null)
            {
                errorMsg = "还没有初始化缓存数据对象！";
                return list;
            }
            if (currencyType != QueryType.QueryCurrencyType.ALL)
            {

                HKCapitalMemoryTable table = manager.GetByCapitalAccountAndCurrencyType(hk_Cap_Account, (int)currencyType);
                if (table != null)
                {
                    var cap = table.Data;
                    list.Add(cap);
                }
            }
            else
            {
                for (int i = 1; i < 4; i++)
                {

                    HKCapitalMemoryTable table = manager.GetByCapitalAccountAndCurrencyType(hk_Cap_Account, i);
                    if (table != null)
                    {
                        var cap = table.Data;
                        list.Add(cap);
                    }
                }
            }
            return list;
        }
        #endregion

        #region 根据【港股资金账号】查询港股资金账号明细
        /// <summary>
        /// 根据【港股资金账号】查询港股资金账号明细
        /// </summary>
        ///<param name="hk_Cap_Acc">港股资金账号</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<HK_CapitalAccountInfo> QueryHK_CapitalAccountByAccount(string hk_Cap_Acc, QueryType.QueryCurrencyType currencyType, out string errorMsg)
        {
            errorMsg = "";
            List<HK_CapitalAccountInfo> list = new List<HK_CapitalAccountInfo>();

            #region 从数据库中获取数据
            //try
            //{
            //    HK_CapitalAccountDal dal = new HK_CapitalAccountDal();
            //    list = dal.GetListByAccount(hk_Cap_Acc, currencyType);
            //}
            //catch (Exception ex)
            //{
            //    errorMsg = ex.Message;
            //    LogHelper.WriteError(ex.ToString(), ex);
            //}
            #endregion

            #region 从内存表中获取数据
            try
            {
                list = GetHK_CapitalAccountListFromMemory(hk_Cap_Acc, currencyType, out errorMsg);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            #endregion
            return list;
        }
        #endregion

        #region 港股资金冻结明细查询

        #region 根据委托编号查询【港股资金冻结表】明细
        /// <summary>
        /// 根据委托编号查询【港股资金冻结表】明细
        /// </summary>
        /// <param name="entrustNo">委托编号</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public List<HK_CapitalAccountFreezeInfo> QueryHK_CapitalAccountFreezeByEntrustNo(string entrustNo, QueryType.QueryFreezeType freezeType, out string errorMsg)
        {
            errorMsg = "";
            List<HK_CapitalAccountFreezeInfo> list = null;
            if (string.IsNullOrEmpty(entrustNo))
            {
                errorMsg = "委托编号不能为空！";
                return list;
            }
            HK_CapitalAccountFreezeDal dal = new HK_CapitalAccountFreezeDal();
            try
            {
                StringBuilder sb = new StringBuilder("  EntrustNumber='" + entrustNo.Trim() + "'");
                //如果查询的冻结类型不是查询所有时加上条件
                if (freezeType != QueryType.QueryFreezeType.ALL)
                {
                    sb.AppendFormat("  AND FreezeTypeLogo='{0}'", (int)freezeType);
                }
                list = dal.GetListArray(sb.ToString());
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
        }
        #endregion

        #region 根据用户资金账号和查询的交易的货币类型、查询时间段查询【港股资金冻结表】明细信息
        /// <summary>
        /// Title：根据用户资金账号和查询的交易的货币类型、查询时间段查询【港股资金冻结表】明细信息
        /// Desc.：如果开始时间或者结束时间为null即返回当前时间向前一个月（30天）的时间段查询
        ///        如果开始时间大于结束时间即返回当前时间向前一个月（30天）的时间段查询
        /// </summary>
        /// <param name="account">资金账号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<HK_CapitalAccountFreezeInfo> PagingQueryHK_CapitalAccountFreezeByAccount(string account, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, QueryType.QueryFreezeType freezeType, PagingInfo pageInfo, out int total, out string errorMsg)
        {
            #region 初始货变量
            List<HK_CapitalAccountFreezeInfo> list = null;
            HK_CapitalAccountFreezeDal dal = new HK_CapitalAccountFreezeDal();
            errorMsg = "";
            total = 0;
            #endregion

            #region 如果资金账号或者分页对象实体为空直接返错误传递参数提示 列表为null
            if (string.IsNullOrEmpty(account))
            {
                errorMsg = "资金账号不能为空！";
                return list;
            }
            if (pageInfo == null)
            {
                errorMsg = "分页信息不能为空!";
                return null;
            }
            #endregion

            #region 分页查询相关信息
            PagingProceduresInfo prcoInfo = new PagingProceduresInfo();
            prcoInfo.IsCount = pageInfo.IsCount;
            prcoInfo.PageNumber = pageInfo.CurrentPage;
            prcoInfo.PageSize = pageInfo.PageLength;
            prcoInfo.Fields = " f.CapitalFreezeLogoId,f.CapitalAccountLogo,f.FreezeTypeLogo,f.EntrustNumber,f.FreezeCapitalAmount,f.FreezeCost,f.OweCosting,f.ThawTime,f.FreezeTime ";
            prcoInfo.PK = "f.CapitalFreezeLogoId";
            if (pageInfo.Sort == 0)
            {
                prcoInfo.Sort = " f.FreezeTime asc ";
            }
            else
            {
                prcoInfo.Sort = " f.FreezeTime desc ";
            }
            prcoInfo.Tables = "dbo.HK_CapitalAccountFreeze as f  inner join HK_CapitalAccount as a on a.CapitalAccountLogo=f.CapitalAccountLogo";

            #region 组装相关条件
            StringBuilder sb = new StringBuilder();
            //1=1 and a.UserAccountDistributeLogo='010000000406' and a.TradeCurrencyType=1
            sb.Append(" a.UserAccountDistributeLogo='" + account + "'  ");

            if (currencyType != QueryType.QueryCurrencyType.ALL)
            {
                sb.Append(" and a.TradeCurrencyType='" + (int)currencyType + "'");
            }
            sb.AppendFormat(CommonDALBulidSQLScript.BuildWhereQueryBetwennTime(startTime, endTime, 30), "f.FreezeTime");
            //如果查询的冻结类型不是查询所有时加上条件
            if (freezeType != QueryType.QueryFreezeType.ALL)
            {
                sb.AppendFormat("  AND f.FreezeTypeLogo='{0}'", (int)freezeType);
            }
            #endregion

            prcoInfo.Filter = sb.ToString();
            #endregion

            #region 执行查询
            try
            {
                CommonDALOperate<HK_CapitalAccountFreezeInfo> com = new CommonDALOperate<HK_CapitalAccountFreezeInfo>();
                list = com.PagingQueryProcedures(prcoInfo, out total, dal.ReaderBind);
                //list = dal.PagingXH_CapitalAccountFreezeByFilter(prcoInfo, out pageTotal);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
            #endregion

        }

        #endregion

        #region 根据用户ID和查询的交易的货币类型、查询时间段查询【港股资金冻结表】明细
        /// <summary>
        /// Title：根据用户ID和查询的交易的货币类型、查询时间段查询【港股资金冻结表】明细信息
        /// Desc.：如果开始时间或者结束时间为null即返回当前时间向前一个月（30天）的时间段查询
        ///        如果开始时间大于结束时间即返回当前时间向前一个月（30天）的时间段查询
        /// </summary>
        /// <param name="userID">资金账号</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的港股持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<HK_CapitalAccountFreezeInfo> PagingQueryHK_CapitalAccountFreezeByUserID(string userID, int accountType, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, QueryType.QueryFreezeType freezeType, PagingInfo pageInfo, out int total, out string errorMsg)
        {
            #region 初始货变量
            List<HK_CapitalAccountFreezeInfo> list = null;
            HK_CapitalAccountFreezeDal dal = new HK_CapitalAccountFreezeDal();
            errorMsg = "";
            total = 0;
            #endregion

            #region 如果用户ID或者分页对象实体为空直接返错误传递参数提示 列表为null
            if (string.IsNullOrEmpty(userID))
            {
                errorMsg = "用户ID不能为空！";
                return list;
            }
            //如果日后要加上用户和密码验证在此加上
            if (pageInfo == null)
            {
                errorMsg = "分页信息不能为空!";
                return null;
            }
            #endregion

            #region 分页查询相关信息
            PagingProceduresInfo prcoInfo = new PagingProceduresInfo();
            prcoInfo.IsCount = pageInfo.IsCount;
            prcoInfo.PageNumber = pageInfo.CurrentPage;
            prcoInfo.PageSize = pageInfo.PageLength;
            prcoInfo.Fields = " f.CapitalFreezeLogoId,f.CapitalAccountLogo,f.FreezeTypeLogo,f.EntrustNumber,f.FreezeCapitalAmount,f.FreezeCost,f.OweCosting,f.ThawTime,f.FreezeTime ";
            prcoInfo.PK = "f.CapitalFreezeLogoId";
            if (pageInfo.Sort == 0)
            {
                prcoInfo.Sort = " f.FreezeTime asc ";
            }
            else
            {
                prcoInfo.Sort = " f.FreezeTime desc ";
            }
            prcoInfo.Tables = "dbo.HK_CapitalAccountFreeze as f  inner join HK_CapitalAccount as a on a.CapitalAccountLogo=f.CapitalAccountLogo";


            #region 组装相关条件
            StringBuilder sb = new StringBuilder();
            #region 从缓存中获取资金账号
            List<UA_UserAccountAllocationTableInfo> userAccountInfo = AccountManager.Instance.GetUserCapitalAccountFormUserCache(userID, accountType, 1);
            #region 添加数据
            string userIDstr = "";
            foreach (UA_UserAccountAllocationTableInfo item in userAccountInfo)
            {
                userIDstr += ",   '" + item.UserAccountDistributeLogo + "'";
            }
            if (!string.IsNullOrEmpty(userIDstr))
            {
                userIDstr = userIDstr.Substring(userIDstr.IndexOf(',') + 1);
            }

            #endregion
            #endregion

            #region 直接从数据库中获取
            if (!string.IsNullOrEmpty(userIDstr))
            {
                if (userIDstr.Split(',').Length > 1)
                {
                    sb.AppendFormat("  a.UserAccountDistributeLogo  in ( {0} )", userIDstr);
                }
                else
                {
                    sb.AppendFormat("  a.UserAccountDistributeLogo ={0} ", userIDstr);
                }
            }
            else //如果在缓存中获取不到数据直接从数据库中获取数据
            {
                //这里不再查找数据库
                sb.Append("  a.UserAccountDistributeLogo ='' ");
                //if (accountType == 0)
                //{
                //    sb.Append("  a.UserAccountDistributeLogo  in ( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable  where accounttypelogo in ( select accounttypelogo from BD_AccountType where atcid='" + (int)CommonObject.Types.AccountAttributionType.SpotCapital + "')  and userid='" + userID + "' )");
                //}
                //else
                //{
                //    sb.Append("  a.UserAccountDistributeLogo  in ( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable  where accounttypelogo='" + accountType + "'  and userid='" + userID + "' )");
                //}
            }
            #endregion


            if (currencyType != QueryType.QueryCurrencyType.ALL)
            {
                sb.Append(" and a.TradeCurrencyType='" + (int)currencyType + "'");
            }
            sb.AppendFormat(CommonDALBulidSQLScript.BuildWhereQueryBetwennTime(startTime, endTime, 30), "f.FreezeTime");
            //如果查询的冻结类型不是查询所有时加上条件
            if (freezeType != QueryType.QueryFreezeType.ALL)
            {
                sb.AppendFormat("  AND f.FreezeTypeLogo='{0}'", (int)freezeType);
            }
            #endregion

            prcoInfo.Filter = sb.ToString();
            #endregion

            #region 执行查询
            try
            {
                CommonDALOperate<HK_CapitalAccountFreezeInfo> com = new CommonDALOperate<HK_CapitalAccountFreezeInfo>();
                list = com.PagingQueryProcedures(prcoInfo, out total, dal.ReaderBind);
                // list = dal.PagingXH_CapitalAccountFreezeByFilter(prcoInfo, out pageTotal);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
            #endregion
        }
        #endregion
        #endregion
    }
}
