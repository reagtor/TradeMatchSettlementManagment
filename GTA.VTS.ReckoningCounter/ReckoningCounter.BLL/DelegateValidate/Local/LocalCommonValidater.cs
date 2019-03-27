#region Using Namespace

using System.Collections.Generic;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.DAL;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.Entity;
using ReckoningCounter.Model;

#endregion

namespace ReckoningCounter.BLL.DelegateValidate.Local
{
    /// <summary>
    /// 内部校验的基础功能类，错误码范围1900-1999
    /// 作者：宋涛
    /// 日期：2008-11-24
    /// </summary>
    public class LocalCommonValidater
    {
        protected int accountFundTypeClassId;
        protected int accountFundTypeId;

        protected int accountHoldTypeClassId;
        protected int accountHoldTypeId;

        protected CM_BreedClass breedClass;

        /// <summary>
        /// 资金账户类型
        /// </summary>
        public Types.AccountAttributionType AccountFundTypeId
        {
            get { return (Types.AccountAttributionType)accountFundTypeId; }
        }


        /// <summary>
        /// 资金账户类型分类
        /// </summary>
        public int AccountFundTypeClassId
        {
            get { return accountFundTypeClassId; }
        }

        /// <summary>
        /// 持仓账户类型
        /// </summary>
        public Types.AccountAttributionType AccountHoldTypeId
        {
            get { return (Types.AccountAttributionType)accountHoldTypeId; }
        }

        /// <summary>
        /// 持仓账户类型分类
        /// </summary>
        public int AccountHoldTypeClassId
        {
            get { return accountHoldTypeClassId; }
        }

        /// <summary>
        /// 进行初始化，进行账户类型和账户类型分类的获取
        /// </summary>
        /// <param name="_traderId">交易员账户id</param>
        /// <param name="_code">商品代码</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        protected bool Initialize(string _traderId, string _code, out string errMsg)
        {
            #region old code
            //errMsg = "";
            //string errCode = "";

            //int? atidFund = null;
            //int? atidHold = null;

            //try
            //{
            //    GetAccountTypeID(_code, out breedClass, out atidFund, out atidHold);
            //}
            //catch (VTException vt)
            //{
            //    LogHelper.WriteDebug(vt.ToString());
            //    errCode = vt.Code;
            //    errMsg = vt.ToString();

            //    return false;
            //}

            //#region 资金

            //if (!atidFund.HasValue)
            //{
            //    errMsg = "无法获取交易员资金账户类型。";
            //    errCode = "GT-1900";
            //    errMsg = errCode + ":" + errMsg;

            //    LogHelper.WriteInfo(errMsg);
            //    return false;
            //}

            //accountFundTypeId = atidFund.Value;

            //int? atcFundid = GetATCID(accountFundTypeId);

            //if (!atcFundid.HasValue)
            //{
            //    errCode = "GT-1901";
            //    errMsg = "无法获取交易员资金账户类型分类。";
            //    errMsg = errCode + ":" + errMsg;
            //    LogHelper.WriteInfo(errMsg);

            //    return false;
            //}

            //accountFundTypeClassId = atcFundid.Value;

            //#endregion

            //#region 持仓

            //if (!atidHold.HasValue)
            //{
            //    errMsg = "无法获取交易员持仓账户类型。";
            //    errCode = "GT-1902";
            //    errMsg = errCode + ":" + errMsg;

            //    LogHelper.WriteInfo(errMsg);
            //    return false;
            //}

            //accountHoldTypeId = atidHold.Value;

            //int? atcHoldid = GetATCID(accountHoldTypeId);

            //if (!atcHoldid.HasValue)
            //{
            //    errCode = "GT-1903";
            //    errMsg = "无法获取交易员资金账户类型分类。";
            //    errMsg = errCode + ":" + errMsg;
            //    LogHelper.WriteInfo(errMsg);

            //    return false;
            //}

            //accountHoldTypeClassId = atidHold.Value;

            //#endregion

            //return true;
            #endregion
            return Initialize(Types.BreedClassTypeEnum.Stock, _traderId, _code, out   errMsg);
        }

        /// <summary>
        /// 进行初始化，进行账户类型和账户类型分类的获取
        /// </summary>
        /// <param name="type">商品所属类别用来为了区分查询港股代码</param>
        /// <param name="_traderId">交易员账户id</param>
        /// <param name="_code">商品代码</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        protected bool Initialize(Types.BreedClassTypeEnum type, string _traderId, string _code, out string errMsg)
        {
            errMsg = "";
            string errCode = "";

            int? atidFund = null;
            int? atidHold = null;

            try
            {
                GetAccountTypeID(type, _code, out breedClass, out atidFund, out atidHold);
            }
            catch (VTException vt)
            {
                LogHelper.WriteDebug(vt.ToString());
                errCode = vt.Code;
                errMsg = vt.ToString();

                return false;
            }

            #region 资金

            if (!atidFund.HasValue)
            {
                errMsg = "无法获取交易员资金账户类型。";
                errCode = "GT-1900";
                errMsg = errCode + ":" + errMsg;

                LogHelper.WriteInfo(errMsg);
                return false;
            }

            accountFundTypeId = atidFund.Value;

            int? atcFundid = GetATCID(accountFundTypeId);

            if (!atcFundid.HasValue)
            {
                errCode = "GT-1901";
                errMsg = "无法获取交易员资金账户类型分类。";
                errMsg = errCode + ":" + errMsg;
                LogHelper.WriteInfo(errMsg);

                return false;
            }

            accountFundTypeClassId = atcFundid.Value;

            #endregion

            #region 持仓

            if (!atidHold.HasValue)
            {
                errMsg = "无法获取交易员持仓账户类型。";
                errCode = "GT-1902";
                errMsg = errCode + ":" + errMsg;

                LogHelper.WriteInfo(errMsg);
                return false;
            }

            accountHoldTypeId = atidHold.Value;

            int? atcHoldid = GetATCID(accountHoldTypeId);

            if (!atcHoldid.HasValue)
            {
                errCode = "GT-1903";
                errMsg = "无法获取交易员资金账户类型分类。";
                errMsg = errCode + ":" + errMsg;
                LogHelper.WriteInfo(errMsg);

                return false;
            }

            accountHoldTypeClassId = atidHold.Value;

            #endregion

            return true;
        }

        /// <summary>
        /// 根据交易员ID，交易账户类型来获交易账户信息UserAccountAllocationTable
        /// </summary>
        /// <param name="traderId"></param>
        /// <param name="accountTypeID"></param>
        /// <returns></returns>
        protected static UA_UserAccountAllocationTableInfo GetUserAccountAllocationTable(string traderId, int accountTypeID)
        {
            UA_UserAccountAllocationTableInfo val = null;

            try
            {
                //UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
                // List<UA_UserAccountAllocationTableInfo> list =
                //         DataRepository.UaUserAccountAllocationTableProvider.GetByUserId(traderId);
                //string where = string.Format("UserID = '{0}' ", traderId);
                //List<UA_UserAccountAllocationTableInfo> list = dal.GetListArray(where);
                var list = AccountManager.Instance.GetUserAllAccounts(traderId);
                foreach (UA_UserAccountAllocationTableInfo accountAllocationTable in list)
                {
                    if (accountAllocationTable.AccountTypeLogo == accountTypeID)
                    {
                        if (accountAllocationTable.WhetherAvailable)
                        {
                            val = accountAllocationTable;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return val;
        }

        /// <summary>
        /// 根据商品代码获取交易账户类型（对应BD_AccountType表中的内容AccountTypeID）
        /// </summary>
        /// <param name="type">商品所属类别用来为了区分查询港股代码</param>
        /// <param name="commodityCode"></param>
        /// <param name="breedClass"></param>
        /// <param name="accountTypeIDFunc">资金帐号类型</param>
        /// <param name="accountTypeIDHold">持仓帐号类型</param>
        protected static void GetAccountTypeID(Types.BreedClassTypeEnum type, string commodityCode, out CM_BreedClass breedClass,
                                               out int? accountTypeIDFunc, out int? accountTypeIDHold)
        {
            breedClass = GetBreedClass(type, commodityCode);

            if (breedClass == null)
            {
                throw new VTException("GT-1905", "交易商品品种不存在");
            }


            //通过产品获取交易账户类型

            accountTypeIDFunc = breedClass.AccountTypeIDFund;
            accountTypeIDHold = breedClass.AccountTypeIDHold;
        }

        /// <summary>
        /// 根据交易账户类型获取账户类型分类 AccountTypeClassID
        /// </summary>
        /// <param name="accountTypeID"></param>
        /// <returns></returns>
        protected static int? InternalGetATCID(int accountTypeID)
        {
            var accountType = AccountManager.Instance.GetAccountType(accountTypeID);
            if(accountType != null)
            {
                return accountType.ATCId;
            }

            BD_AccountTypeDal bd_AccountTypeDal = new BD_AccountTypeDal();
            //BD_AccountTypeInfo accountType = null;
            try
            {
                accountType = bd_AccountTypeDal.GetModel(accountTypeID);
            }
            catch (System.Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return accountType == null ? null : accountType.ATCId;
        }

        private static SyncCache<int, int> atcidCache = new SyncCache<int, int>();
        protected static int? GetATCID(int accountTypeID)
        {
            int? result = null;

            int val;
            if (!atcidCache.TryGetValue(accountTypeID, out val))
            {
                result = InternalGetATCID(accountTypeID);
                if (result.HasValue)
                {
                    atcidCache.AddOrUpdate(accountTypeID, result.Value);
                }
            }
            else
            {
                result = val;
            }

            return result;
        }

        public static void Reset()
        {
            atcidCache.Reset();
        }

        /// <summary>
        /// 根据商品代码获取交易商品品种BreedClass
        /// </summary>
        /// <param name="type">商品所属类别用来为了区分查询港股代码</param>
        /// <param name="commodityCode"></param>
        /// <returns></returns>
        protected static CM_BreedClass GetBreedClass(Types.BreedClassTypeEnum type, string commodityCode)
        {
            #region  other
            //int breedClassID;
            //switch (type)
            //{
            //    case Types.BreedClassTypeEnum.Stock:
            //    case Types.BreedClassTypeEnum.CommodityFuture:
            //    case Types.BreedClassTypeEnum.StockIndexFuture:
            //        CM_Commodity commodity = MCService.CommonPara.GetCommodityByCommodityCode(commodityCode);
            //        if (commodity == null)
            //        {
            //            throw new VTException("GT-1904", "交易品种不存在");
            //        }
            //        else
            //        {
            //            breedClassID = commodity.BreedClassID.Value;
            //        }
            //        break;
            //    case Types.BreedClassTypeEnum.HKStock:
            //        //等待实现
            //        //CM_Commodity commodity = GetCommodityByCommodityCode(code);
            //        //if (commodity != null)
            //        //{
            //        //    breedClassID = commodity.BreedClassID.Value;
            //        //}
            //        break;
            //}

            //old code
            //CM_Commodity commodity = MCService.CommonPara.GetCommodityByCommodityCode(commodityCode);
            //if (commodity == null)
            //{
            //    throw new VTException("GT-1904", "交易品种不存在");
            //}
            //return MCService.CommonPara.GetBreedClassByBreedClassID(commodity.BreedClassID.Value);
            //===old code
            #endregion
            return MCService.CommonPara.GetBreedClassByCommodityCode(commodityCode, type);

        }
    }
}