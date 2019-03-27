using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagementCenter.DAL.AccountManageService;
using ManagementCenter.Model;
using ManagementCenter.Model.QH;
using ManagementCenter.BLL.ServiceIn;

namespace ManagementCenter.BLL.QH
{
    /// <summary>
    /// Desc: 结算价管理业务类
    /// Create By: 董鹏
    /// Create Date: 2010-03-10
    /// </summary>
    public class QH_TodaySettlementPriceBLL
    {
        /// <summary>
        /// 手动提交清算
        /// </summary>
        /// <param name="list">结算价列表</param>
        /// <param name="errorMsg">异常信息</param>
        /// <returns></returns>
        public bool DoManualReckoning(List<QH_TodaySettlementPriceInfoEx> list, out string errorMsg)
        {
            errorMsg = "结算价列表为Null！";
            if (list == null)
            {
                return false;
            }

            List<QH_TodaySettlementPriceInfo> listInfo = new List<QH_TodaySettlementPriceInfo>();
            foreach (QH_TodaySettlementPriceInfoEx item in list)
            {
                QH_TodaySettlementPriceInfo info = new QH_TodaySettlementPriceInfo();
                info.CommodityCode = item.CommodityCode;
                info.TradingDate = item.TradingDate;
                info.SettlementPrice = item.SettlementPrice;
                listInfo.Add(info);
            }

            List<CT_Counter> listcounter = StaticDalClass.CounterDAL.GetListArray(string.Empty);
            bool rtn = true;
            foreach (CT_Counter T in listcounter)
            {
                bool b = AccountManageServiceProxy.GetInstance().DoManualReckoning(T, listInfo, out errorMsg);
                if (!b)
                {
                    rtn = false;
                }
            }
            return rtn;
        }

        /// <summary>
        /// 获取当前所有持仓中要提供当日结算价清算的代码
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public List<QH_TodaySettlementPriceInfoEx> GetReckoningHoldCodeList(out string errMsg)
        {
            errMsg = string.Empty;
            string msg = string.Empty;
            Dictionary<string, QH_TodaySettlementPriceInfoEx> dict = new Dictionary<string, QH_TodaySettlementPriceInfoEx>();
            List<QH_TodaySettlementPriceInfoEx> list = null;
            List<CT_Counter> listcounter = StaticDalClass.CounterDAL.GetListArray(string.Empty);
            foreach (CT_Counter T in listcounter)
            {
                List<QH_TodaySettlementPriceInfo> holdCodeList = AccountManageServiceProxy.GetInstance().GetReckoningHoldCodeList(T, out errMsg);
                if (!string.IsNullOrEmpty(errMsg))
                {
                    msg = errMsg;
                }
                if (holdCodeList != null)
                {
                    if (list == null)
                    {
                        list = new List<QH_TodaySettlementPriceInfoEx>();
                    }
                    foreach (QH_TodaySettlementPriceInfo item in holdCodeList)
                    {
                        //消除重复的代码
                        if (!dict.ContainsKey(item.CommodityCode))
                        {
                            QH_TodaySettlementPriceInfoEx info = new QH_TodaySettlementPriceInfoEx();
                            info.CommodityCode = item.CommodityCode;
                            info.TradingDate = item.TradingDate;
                            info.SettlementPrice = item.SettlementPrice;
                            dict.Add(item.CommodityCode, info);
                        }
                    }
                }
            }
            foreach (KeyValuePair<string, QH_TodaySettlementPriceInfoEx> item in dict)
            {
                list.Add(item.Value);
            }
            errMsg = msg;
            return list;
        }

        /// <summary>
        /// 查看指定的时间柜台是否完成清算
        /// </summary>
        /// <param name="doneDate">指定日期时间</param>
        /// <returns></returns>
        public bool IsReckoningDone(DateTime doneDate)
        {
            List<CT_Counter> listcounter = StaticDalClass.CounterDAL.GetListArray(string.Empty);
            bool rtn = true;
            foreach (CT_Counter T in listcounter)
            {
                bool b = AccountManageServiceProxy.GetInstance().IsReckoningDone(T, doneDate);
                if (!b)
                {
                    rtn = false;
                    break;
                }
            }
            return rtn;
        }

    }
}
