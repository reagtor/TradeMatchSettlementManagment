using System;
using System.ServiceModel;
using MatchCenter.DAL.DevolveVerifyCommonService;
using MatchCenter.DAL.FuturesDevolveService;
using MatchCenter.DAL.SpotTradingDevolveService;
//增加港股引用
using MatchCenter.DAL.HKTradingRulesService;
using System.Configuration;
using MatchCenter.BLL.Common;

namespace MatchCenter.BLL.ManagementCenter
{
    /// <summary>
    /// 初始化撮合中心数据访问代理类
    /// 管理中心客户端创建连接服务对象
    /// Create BY：李健华
    /// Create Date：2009-08-18
    /// 增加港股信息
    /// Create By: 王伟
    /// Create Date:2009-10-22
    /// </summary>
    public class ManagementCenterDataAgent
    {

        #region 管理中心客户端创建连接服务对象单例
        private static ManagementCenterDataAgent instanse;
        /// <summary>
        /// 管理中心客户端创建连接服务对象单例
        /// </summary>
        public static ManagementCenterDataAgent Instanse
        {
            get
            {
                //撮合中心实体不能为空
                if (instanse == null)
                {
                    instanse = new ManagementCenterDataAgent();
                }
                return instanse;
            }
        }
        #endregion

        ///// <summary>
        ///// 获取管理中心公共数据连接客户端对象
        ///// </summary>
        //private static CommonParaClient commonClient;
        ///// <summary>
        ///// 获取管理中心【期货】交易规则连接客户端对象
        ///// </summary>
        //private static FuturesTradeRulesClient futureClient;
        ///// <summary>
        ///// 获取管理中心【现货】交易规则连接客户端对象
        ///// </summary>
        //private static SpotTradeRulesClient tradeRuleClient;

        /// <summary>
        /// 获取与管理中心公共数据连接客户端对象 
        /// 这里不捕捉异常，所以外部调用时要捕捉异常
        /// </summary>
        /// <returns></returns>
        public CommonParaClient GetComonParaInstanse()
        {
            CommonParaClient client = new CommonParaClient();
            return client;

            #region oldcode
            //get
            //{
            //if (commonClient == null)
            //{
            //    string strAddress = AppConfig.GetConfigManageCenterIP();
            //    string port = AppConfig.GetConfigManagePort().ToString();
            //    EndpointAddress orderAddress = new EndpointAddress("net.tcp://" + strAddress + ":" + port + "/WcfCommonalityProvider");
            //    commonClient = new CommonParaClient("NetTcpBinding_ICommonPara", orderAddress);
            //}
            //return commonClient;
            //}
            #endregion 
        }

        /// <summary>
        /// 获取与管理中心期货交易规则连接客户端对象
        /// </summary>
        /// <returns></returns>
        public FuturesTradeRulesClient GetFutureTradeRulesInstanse()
        {
            FuturesTradeRulesClient futureClient = new FuturesTradeRulesClient();
            return futureClient;
            #region oldcode
            //get
            //{
            //    if (futureClient == null)
            //    {
            //        string strAddress = AppConfig.GetConfigManageCenterIP();
            //        string port = AppConfig.GetConfigManagePort().ToString();
            //        EndpointAddress orderAddress = new EndpointAddress("net.tcp://" + strAddress + ":" + port + "/WcfFuturesProvider");
            //        futureClient = new FuturesTradeRulesClient("NetTcpBinding_IFuturesTradeRules",
            //                                                                     orderAddress);
            //    }
            //    return futureClient;
            //}
            #endregion
        }

        /// <summary>
        /// 获取与管理中心现货交易规则连接客户端对象
        /// </summary>
        /// <returns></returns>
        public SpotTradeRulesClient GetSpotTradeRulesInstanse()
        {
            SpotTradeRulesClient tradeRuleClient = new SpotTradeRulesClient();
            return tradeRuleClient;

            #region oldcode
            //get
            //{
            //    if (tradeRuleClient == null)
            //    {
            //        string strAddress = AppConfig.GetConfigManageCenterIP();
            //        string port = AppConfig.GetConfigManagePort().ToString();
            //        EndpointAddress orderAddress =
            //        new EndpointAddress("net.tcp://" + strAddress + ":" + port + "/WcfSpotTradingRulesProvider");
            //        tradeRuleClient = new SpotTradeRulesClient("NetTcpBinding_ISpotTradeRules", orderAddress);

            //    }
            //    return tradeRuleClient;

            //}
            #endregion
        }

        /// <summary>
        /// 获取与管理中心港股交易规则连接客户端对象
        /// </summary>
        /// <returns></returns>
        public HKTradeRulesClient GetHKTradeRulesInstance()
        {
            HKTradeRulesClient tradeRuleClient = new HKTradeRulesClient();
            return tradeRuleClient;        
        }



        ///// <summary>
        ///// 关闭wcf服务
        ///// </summary>
        //public void MatchClost()
        //{
        //    //服务对象不能为空
        //    if (commonClient != null && commonClient.State != CommunicationState.Closed)
        //    {
        //        commonClient.Close();
        //    }
        //    //服务对象不能为空
        //    if (futureClient != null && futureClient.State != CommunicationState.Closed)
        //    {
        //        futureClient.Close();
        //    }
        //    //服务对象不能为空
        //    if (tradeRuleClient != null && tradeRuleClient.State != CommunicationState.Closed)
        //    {
        //        tradeRuleClient.Close();
        //    }

        //}

        ///// <summary>
        ///// 清空服务
        ///// </summary>
        //public void ClearService()
        //{
        //    commonClient = null;
        //    futureClient = null;
        //    tradeRuleClient = null;
        //}


    }
}
