#region Using Namespace

using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.DAL;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.Entity;
using ReckoningCounter.Entity.Contants; 
using ReckoningCounter.Model;

#endregion

namespace ReckoningCounter.BLL.DelegateValidate.Local
{
    /// <summary>
    /// 证券撤单内部校验，错误码范围1500-1599
    /// 作者：宋涛
    /// 日期：2008-11-24
    /// </summary>
    public class LocalStockCacelValidater
    {
        private string orderNo;
        private XH_TodayEntrustTableInfo todayEntrustTable;

        public LocalStockCacelValidater(string orderNo)
        {
            this.orderNo = orderNo;
        }

        /// <summary>
        /// 进行撤单校验
        /// </summary>
        /// <param name="errMsg">错误信息</param>
        /// <returns>校验结果</returns>
        public bool Check(ref string errMsg)
        {
            bool result = CheckDelegateExist(ref errMsg);

            if (result)
            {
                int state =todayEntrustTable.OrderStatusId;
                Types.OrderStateType orderState = (Types.OrderStateType) state;
                result = CheckCanCancelDelegate(orderState, ref errMsg);
            }

            return result;
        }

        /// <summary>
        /// 检查是否存在对应的委托单号
        /// </summary>
        /// <param name="errMsg">错误信息</param>
        /// <returns>校验结果</returns>
        private bool CheckDelegateExist(ref string errMsg)
        {
            bool result = false;
            errMsg = "";

            try
            {
                XH_TodayEntrustTableDal dal = new XH_TodayEntrustTableDal();

                //todayEntrustTable = DataRepository.XhTodayEntrustTableProvider.GetByEntrustNumber(this.orderNo);
                todayEntrustTable = dal.GetModel(this.orderNo);
            }
            catch (System.Exception ex)
            {
                LogHelper.WriteError(ex.Message,ex);
            }

            if (todayEntrustTable != null)
            {
                result = true;
            }
            else
            {
                string errCode = "GT-1500";
                errMsg = "委托不存在。";
                errMsg = errCode + ":" + errMsg;
                LogHelper.WriteInfo(errMsg);
            }

            return result;
        }

        /// <summary>
        /// 检查是否可撤单
        /// </summary>
        /// <param name="orderState">委托状态</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>校验结果</returns>
        private bool CheckCanCancelDelegate(Types.OrderStateType orderState, ref string errMsg)
        {
            bool result = false;
            errMsg = "";

            if (orderState == Types.OrderStateType.DOSUnRequired || orderState == Types.OrderStateType.DOSRequiredSoon ||
                orderState == Types.OrderStateType.DOSIsRequired || orderState == Types.OrderStateType.DOSPartDealed)
            {
                result = true;
            }
            else
            {
                string errCode = "GT-1501";
                errMsg = "委托不可撤。";
                errMsg = errCode + ":" + errMsg;
                LogHelper.WriteInfo(errMsg);
            }

            return result;
        }
    }
}