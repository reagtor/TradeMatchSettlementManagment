#region Using Namespace

using ReckoningCounter.Entity;

#endregion

namespace ReckoningCounter.BLL.DelegateValidate.Local
{
    /// <summary>
    /// 期货平仓买卖委托内部校验，错误码范围1400-1499
    /// 作者：宋涛
    /// 日期：2008-11-24
    /// </summary>
    public static class LocalFutureCloseValidater
    {
        /// <summary>
        /// 检测期货资金表是否正常，因为具体的开平仓由调用方外部校验，所以不再区分开平仓
        /// 统一由LocalFutureOpenValidater进行校验
        /// </summary>
        /// <param name="request"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool CheckFuncTable(MercantileFuturesOrderRequest request, ref string errMsg)
        {
            errMsg = "";
            return false;
        }

        /// <summary>
        /// 检测合约是否存在
        /// </summary>
        /// <param name="request"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool CheckContractExist(MercantileFuturesOrderRequest request, ref string errMsg)
        {
            errMsg = "";
            return false;
        }

        /// <summary>
        /// 检测是否停牌
        /// </summary>
        /// <param name="request"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool CheckStopTrading(MercantileFuturesOrderRequest request, ref string errMsg)
        {
            errMsg = "";
            return false;
        }

        /// <summary>
        /// 检测是否存在对应的持仓
        /// </summary>
        /// <param name="request"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool CheckPostionExist(MercantileFuturesOrderRequest request, ref string errMsg)
        {
            errMsg = "";
            return false;
        }

        /// <summary>
        /// 检测平仓买卖方向是否与持仓方向相反
        /// </summary>
        /// <param name="request"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool CheckCloseDirection(MercantileFuturesOrderRequest request, ref string errMsg)
        {
            errMsg = "";
            return false;
        }

        /// <summary>
        /// 检测:平仓量<=持仓量
        /// </summary>
        /// <param name="request"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool CheckPositionNum(MercantileFuturesOrderRequest request, ref string errMsg)
        {
            errMsg = "";
            return false;
        }

        /// <summary>
        /// 检测：手续费<=可用资金
        /// </summary>
        /// <param name="request"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool CheckTradeMoney(MercantileFuturesOrderRequest request, ref string errMsg)
        {
            errMsg = "";
            return false;
        }
    }
}