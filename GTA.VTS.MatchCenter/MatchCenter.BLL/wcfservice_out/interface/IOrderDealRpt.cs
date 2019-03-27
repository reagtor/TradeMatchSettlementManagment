using System.ServiceModel;

namespace MatchCenter.BLL.interfaces
{
    /// <summary>
    /// 撮合中心成交回报接口
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// </summary>
    [ServiceContract(CallbackContract = typeof(IDoOrderCallback))]
    public interface IOrderDealRpt
    {
        /// <summary>
        /// 获取通道号
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        void RegisterChannel(string channelId);

        /// <summary>
        /// 移除通道号
        /// </summary>
        /// <param name="channelId"></param>
        [OperationContract]
        void UnRegisterChannel(string channelId);

        /// <summary>
        /// 检查通道
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string CheckChannel();

    }
}
