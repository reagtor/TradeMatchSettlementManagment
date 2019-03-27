#region Using Namespace

using System.Collections.Generic;
using System.ServiceModel;

#endregion

namespace ReckoningCounter.BLL
{
    /// <summary>
    /// 成交回报服务接口
    /// </summary>
    [ServiceContract(CallbackContract = typeof(IDoOrderCallback))]
    public interface IOrderDealRpt
    {
        /// <summary>
        /// 注册通道
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool RegisterChannel(string clientId);

        /// <summary>
        /// 修改委托对应的通道id
        /// </summary>
        /// <param name="entrustNumberList">委托单号列表</param>
        /// <param name="newClientId">新通道id</param>
        /// <param name="numberType">编号类型1--现货，2-期货</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool ChangeEntrustChannel(List<string> entrustNumberList, string newClientId, int numberType);

        /// <summary>
        /// 注销通道
        /// </summary>
        /// <param name="clientId"></param>
        [OperationContract]
        bool UnRegisterChannel(string clientId);

        /// <summary>
        /// 检查通道
        /// </summary>
        [OperationContract]
        string CheckChannel();
    }
}