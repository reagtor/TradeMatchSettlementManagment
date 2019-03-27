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
    /// 期货撤单内部校验，错误码范围1600-1699
    /// 作者：宋涛
    /// 日期：2008-11-24
    /// </summary>
    public class LocalFutureCacelValidater
    {
        private string orderNo;

        private QH_TodayEntrustTableInfo qhTodayEntrustTable;

        public LocalFutureCacelValidater(string orderNo)
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
                int state = qhTodayEntrustTable.OrderStatusId;
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
            QH_TodayEntrustTableDal dal = new QH_TodayEntrustTableDal();
            //qhTodayEntrustTable = DataRepository.QhTodayEntrustTableProvider.GetByEntrustNumber(this.orderNo);
            qhTodayEntrustTable = dal.GetModel(this.orderNo);
            if (qhTodayEntrustTable != null)
            {
                result = true;
            }
            else
            {
                string errCode = "GT-1600";
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
                string errCode = "GT-1601";
                errMsg = "委托不可撤。";
                errMsg = errCode + ":" + errMsg;
                LogHelper.WriteInfo(errMsg);
            }

            return result;
        }
    }
}