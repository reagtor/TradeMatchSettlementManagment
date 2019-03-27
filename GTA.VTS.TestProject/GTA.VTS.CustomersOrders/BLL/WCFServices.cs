#region Using Namespace

using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Timers;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;

using GTA.VTS.CustomersOrders.DoAccountManager;
using GTA.VTS.CustomersOrders.DoDealRptService;
using GTA.VTS.CustomersOrders.DoOrderService;
using GTA.VTS.CustomersOrders.HKCommonQuery;
using GTA.VTS.CustomersOrders.DoCommonQuery;
using GTA.VTS.CustomersOrders.TransactionManageService;
using AccountAndCapitalManagementClient = GTA.VTS.CustomersOrders.DoAccountManager.AccountAndCapitalManagementClient;
using AccountFindResultEntity = GTA.VTS.CustomersOrders.DoAccountManager.AccountFindResultEntity;
using AddCapitalEntity = GTA.VTS.CustomersOrders.DoAccountManager.AddCapitalEntity;
using FreeTransferEntity = GTA.VTS.CustomersOrders.DoAccountManager.FreeTransferEntity;
using HighLowRangeValue = GTA.VTS.CustomersOrders.DoAccountManager.HighLowRangeValue;
using PagingInfo = GTA.VTS.CustomersOrders.DoCommonQuery.PagingInfo;
using TypesOrderPriceType = GTA.VTS.CustomersOrders.DoAccountManager.TypesOrderPriceType;


#endregion

namespace GTA.VTS.CustomersOrders.BLL
{
    /// <summary>
    /// Tilte;WCF服务访问类
    /// Desc: 提供所有对WCF服务的访问
    /// Create BY：董鹏
    /// Create date:2009-12-22
    /// 修改：叶振东
    /// 时间：2010-01-21
    /// 描述：添加资金流水查询
    /// </summary>
    public class WCFServices
    {
        #region WCF服务访问单例
        /// <summary>
        /// WCF服务访问对象
        /// </summary>
        private static WCFServices _instance;
        /// <summary>
        /// WCF服务访问单例
        /// </summary>
        public static WCFServices Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new WCFServices();
                return _instance;
            }
        }

        #endregion

        #region 变量定义

        /// <summary>
        /// 通道ID
        /// </summary>
        private string Channelid;

        /// <summary>
        /// 下单服务代理
        /// </summary>
        private DoOrderClient doOrderClient;
        /// <summary>
        /// 公共查询服务代理
        /// </summary>
        private TraderQueryClient traderQueryClient;
        /// <summary>
        /// 港股查询服务代理
        /// </summary>
        private HKTraderQueryClient hkTraderQueryClient;
        /// <summary>
        /// 回报服务代理
        /// </summary>
        private OrderDealRptClient rptClient;
        /// <summary>

        /// 账户资金管理服务代理
        /// </summary>
        private AccountAndCapitalManagementClient accountClient;

        /// <summary>
        /// 管理中心服务代理
        /// </summary>
        private TransactionManageClient transactionManageClient;

        /// <summary>
        /// 现货资金账户
        /// </summary>
        private string xhAccount = "";
        /// <summary>
        /// 股指期货资金账户
        /// </summary>
        private string gzqhAccount = "";
        /// <summary>
        /// 港股资金账户
        /// </summary>
        private string hkAccount = "";
        /// <summary>
        /// 商品期货资金账户
        /// </summary>
        private string spqhAccount = "";
        /// <summary>
        /// WCF服务是否连接成功
        /// </summary>
        public bool IsServiceOk = true;
        /// <summary>
        /// WCF服务是否手动断开
        /// </summary>
        public bool ServiceOk = true;
        /// <summary>
        /// 定时检查连接通道
        /// </summary>
        private Timer timer;

        /// <summary>
        /// 交易员ID
        /// </summary>
        private string traderId = "";
        /// <summary>
        /// 股指期货帐号类型
        /// </summary>
        private int GZQHaccountType = 6;

        /// <summary>
        /// 获取连接服务的IP地址
        /// </summary>
        public string DnsSafeHost;
        /// <summary>
        /// 商品期货的帐号类型
        /// </summary>
        private int SPQHaccountType = 4;
        private string ServiceErrorMsg = "无法连接柜台服务！请检查配置没有问题后重新连接！";

        #endregion

        #region 交易员账户信息变量赋值
        /// <summary>
        /// 交易员账户信息变量赋值
        /// </summary>
        public void LoadTraderInfo()
        {
            traderId = ServerConfig.TraderID;
            xhAccount = ServerConfig.XHCapitalAccount;
            hkAccount = ServerConfig.HKCapitalAccount;
            gzqhAccount = ServerConfig.GZQHCapitalAccount;
            spqhAccount = ServerConfig.SPQHCapitalAccount;
        }

        #endregion

        #region 信息显示接口
        /// <summary>
        /// wcf信息显示接口
        /// </summary>
        public IMessageView wcfMsgView
        {
            get;
            set;
        }

        #endregion

        #region 显示信息
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="msg">信息</param>
        public void WriteMsg(string msg)
        {
            //Program.mainForm.WriteWCFMsg(msg);

            if (wcfMsgView != null)
            {
                wcfMsgView.WriteMessage(msg);
            }
        }

        #endregion

        #region 服务

        /// <summary>
        /// 初始化wcf连接
        /// </summary>
        /// <param name="userID">交易员ID</param>
        /// <param name="_channelID">连接通道ID</param>
        /// <param name="errorMsg">返回错误消息</param>
        /// <returns>是否连接成功</returns>
        public bool Initialize(string userID, string _channelID, out string errorMsg)
        {
            bool result = false;
            errorMsg = "";
            try
            {
                // CustomBinding bing = new CustomBinding("normalBinding");
                NetTcpBinding bing = new NetTcpBinding("normalBinding");

                EndpointAddress endpointAddress = new EndpointAddress("net.tcp://" + ServerConfig.ReckoningIP + ":" + ServerConfig.DoOrderClientPort + "/" + ServerConfig.DoOrderServerName + "");
                doOrderClient = new DoOrderClient(bing, endpointAddress);
                ICommunicationObject co1 = doOrderClient;
                co1.Faulted += CO_Faulted;

                EndpointAddress endpointAddress1 = new EndpointAddress("net.tcp://" + ServerConfig.ReckoningIP + ":" + ServerConfig.TraderQueryClientPort + "/" + ServerConfig.TraderQueryServerName + "");
                traderQueryClient = new TraderQueryClient(bing, endpointAddress1);
                ICommunicationObject co2 = traderQueryClient;
                co2.Faulted += CO2_Faulted;


                EndpointAddress endpointAddress2 = new EndpointAddress("net.tcp://" + ServerConfig.ReckoningIP + ":" + ServerConfig.OrderDealRptClientPort + "/" + ServerConfig.OrderDealRptServerName + "");
                OrderCallBack callBack = new OrderCallBack();
                rptClient = new OrderDealRptClient(new InstanceContext(callBack), bing, endpointAddress2);
                ICommunicationObject co3 = rptClient;
                co3.Faulted += CO3_Faulted;


                EndpointAddress endpointAddress3 = new EndpointAddress("net.tcp://" + ServerConfig.ReckoningIP + ":" + ServerConfig.AccountAndCapitalManagementClientPort + "/" + ServerConfig.AccountAndCapitalManagementServerName + "");
                accountClient = new AccountAndCapitalManagementClient(bing, endpointAddress3);
                ICommunicationObject co4 = accountClient;
                co4.Faulted += co4_Faulted;


                EndpointAddress endpointAddress4 = new EndpointAddress("net.tcp://" + ServerConfig.ReckoningIP + ":" + ServerConfig.HKTraderQueryClientPort + "/" + ServerConfig.HKTraderQueryServerName + "");
                hkTraderQueryClient = new HKTraderQueryClient(bing, endpointAddress4);
                ICommunicationObject co5 = hkTraderQueryClient;
                co5.Faulted += co5_Faulted;

                EndpointAddress endpointAddress5 = new EndpointAddress("net.tcp://" + ServerConfig.ManagementIP + ":" + ServerConfig.TransactionManageClientPort + "/" + ServerConfig.TransactionManageServerName + "");
                transactionManageClient = new TransactionManageClient(bing, endpointAddress5);
                ICommunicationObject co6 = transactionManageClient;

                traderId = userID;

                IsServiceOk = true;
                if (string.IsNullOrEmpty(_channelID))
                {
                    //若通道号为空取mac地址为通道ID
                    Channelid = ServerConfig.ChannelID;

                    if (string.IsNullOrEmpty(Channelid))
                    {
                        Channelid = CommUtils.GetMacAddress();
                        Channelid = Channelid + "@" + traderId;
                        //ServerConfig.ChannelID = Channelid;
                    }

                    //这是为了处理之前已经有的通道号不是加上交易员ID的
                    if (Channelid.IndexOf('@') < 0)
                    {
                        Channelid += "@" + traderId;
                        // ServerConfig.ChannelID = Channelid;
                    }
                    else
                    {
                        //已经有了这个标志那判断交易员ID是否等于当前登录的
                        int k = Channelid.IndexOf('@');
                        string channelIDTradeID = Channelid.Substring(k);
                        if (channelIDTradeID != traderId)
                        {
                            Channelid = Channelid.Substring(0, k) + "@" + traderId;
                            //ServerConfig.ChannelID = Channelid;
                        }
                    }
                }
                else
                {
                    //这是一个格式没有这个标志这直接加上
                    if (_channelID.IndexOf('@') < 0)
                    {
                        _channelID += "@" + traderId;
                    }
                    //已经有了这个标志那判断交易员ID是否等于当前登录的
                    int k = _channelID.IndexOf('@');
                    string channelIDTradeID = _channelID.Substring(k);
                    if (channelIDTradeID != traderId)
                    {
                        _channelID = _channelID.Substring(0, k) + "@" + traderId;
                    }
                    Channelid = _channelID;
                    //ServerConfig.ChannelID = Channelid;
                }

                ServerConfig.ChannelID = Channelid;

                //注册通道
                result = rptClient.RegisterChannel(Channelid);
                DnsSafeHost = rptClient.ChannelFactory.Endpoint.Address.Uri.Host.ToString();
                if (result)
                {
                    string msg = "WCF Service [DoOrderService] is connected! " + DateTime.Now;
                    WriteMsg(msg);

                    string msg2 = "WCF Service [DoCommonQuery] is connected! " + DateTime.Now;
                    WriteMsg(msg2);

                    string msg3 = "WCF Service [DoDealRptService] is connected! " + DateTime.Now;
                    WriteMsg(msg3);

                    WriteMsg("");

                    //定时检查通道
                    //timer = new Timer();
                    //timer.Interval = 900 * 1000;
                    //timer.Elapsed += CheckRptChannel;
                    //timer.Enabled = true;
                }
                else
                {
                    IsServiceOk = false;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 关闭WCF连接
        /// </summary>
        public void ShutDown()
        {
            IsServiceOk = false;

            try
            {
                rptClient.UnRegisterChannel(Channelid);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            rptClient.DoClose();
            traderQueryClient.DoClose();
            doOrderClient.DoClose();
            accountClient.DoClose();

            string msg = "<————WCF Service [DoOrderService] is disconnected! " + DateTime.Now;
            WriteMsg(msg);

            string msg2 = "<————WCF Service [TradeFindService] is disconnected! " + DateTime.Now;
            WriteMsg(msg2);

            string msg3 = "<————WCF Service [DoDealRptService] is disconnected! " + DateTime.Now;
            WriteMsg(msg3);

            string msg4 = "<————WCF Service [DoAccountAndCapitalService] is disconnected! " + DateTime.Now;
            WriteMsg(msg4);

            WriteMsg("");
        }

        /// <summary>
        /// 检查通道
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckRptChannel(object sender, ElapsedEventArgs e)
        {
            try
            {
                string date = rptClient.CheckChannel();
            }
            catch (Exception ex)
            {
                timer.Enabled = false;
                LogHelper.WriteError(ex.Message, ex);
                ShutDown();
            }
        }

        /// <summary>
        /// 检查账号通通道主要是返回当前服务器的系统时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public DateTime CheckDoAccoumtChannel()
        {
            DateTime nowtime = DateTime.Now;
            try
            {

                string date = accountClient.CheckChannel();
                if (!DateTime.TryParse(date, out nowtime))
                {
                    nowtime = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
            return nowtime;
        }

        #region 服务连接失败信息
        private void co5_Faulted(object sender, EventArgs e)
        {
            IsServiceOk = false;

            string msg = "<————WCF Service [HKTraderQueryService] is disconnected! " + DateTime.Now;
            WriteMsg(msg);
        }

        private void co4_Faulted(object sender, EventArgs e)
        {
            IsServiceOk = false;

            string msg = "<————WCF Service [DoAccountManagerService] is disconnected! " + DateTime.Now;
            WriteMsg(msg);
        }

        private void CO3_Faulted(object sender, EventArgs e)
        {
            IsServiceOk = false;

            string msg = "<————WCF Service [DoDealRptService] is disconnected! " + DateTime.Now;
            WriteMsg(msg);
        }

        private void CO2_Faulted(object sender, EventArgs e)
        {
            IsServiceOk = false;

            string msg = "<————WCF Service [TradeQueryService] is disconnected! " + DateTime.Now;
            WriteMsg(msg);
        }

        private void CO_Faulted(object sender, EventArgs e)
        {
            IsServiceOk = false;

            string msg = "<————WCF Service [DoOrderService] is disconnected! " + DateTime.Now;
            WriteMsg(msg);
        }
        #endregion

        #endregion

        #region 管理中心服务接口
        /// <summary>
        /// 对管理员用户名和密码进行验证,并清空数据
        /// </summary>
        /// <param name="per">个性化资金实体类</param>
        /// <param name="LoginName">管理员用户名</param>
        /// <param name="PassWord">密码</param>
        /// <param name="message">返回的信息</param>
        /// <returns>数据是否清空成功</returns>
        public bool ClearTrialData(PersonalizationCapital per, string LoginName, string PassWord, out string message)
        {
            return transactionManageClient.ClearTrialData(out message, per, LoginName, PassWord);
        }
        /// <summary>
        /// 对Trade Id和Password进行验证
        /// </summary>
        /// <param name="UserID">Trade ID</param>
        /// <param name="PassWord">PassWord</param>
        /// <param name="message">错误信息</param>
        /// <returns>柜体信息</returns>
        public CT_Counter TransactionConfirm(int UserID, string PassWord, out string message)
        {
            return transactionManageClient.TransactionConfirm(out message, UserID, PassWord);
        }
        /// <summary>
        /// 对管理员用户名和密码进行验证,并个性化设置操作
        /// </summary>
        /// <param name="per">个性化资金实体类</param>
        /// <param name="LoginName">管理员用户名</param>
        /// <param name="PassWord">密码</param>
        /// <param name="message">返回的信息</param>
        /// <returns>格式化资金是否成功</returns>
        public bool PersonalizationCapital(PersonalizationCapital per, string LoginName, string PassWord, out string message)
        {
            return transactionManageClient.PersonalizationCapital(out message, per, LoginName, PassWord);
        }

        /// <summary>
        /// 追加资金(此方法之前是没有实现的，这里实现只是用于验证管理员是否正确,为了测试工具使用，如果日后要用再
        /// 作修改，因为这里也要验证管理员)
        /// </summary>
        /// <param name="LoginName">管理员登陆名称</param>
        /// <param name="PassWord">管理员密码</param>
        /// <param name="message">返回错误消息</param>
        /// <returns></returns>
        public bool AdminConfirmation(string LoginName, string PassWord, out string message)
        {
            int UserId = 0;
            InitFund intFund = new InitFund();
            return transactionManageClient.AddFund(out message, UserId, intFund, LoginName, PassWord);
        }
        #endregion 管理中心服务接口

        #region 获取基本信息

        /// <summary>
        /// 根据商品代码和委托价格获取上下限（涨跌幅值）
        /// </summary>
        /// <param name="code">现货期货商品代码（原代码表）</param>
        /// <param name="orderPrice">委托价格</param>
        /// <returns></returns>
        public HighLowRangeValue GetHighLowRangeValueByCommodityCode(string code, decimal orderPrice, out string errMsg)
        {
            try
            {
                errMsg = "";
                return accountClient.GetHighLowRangeValueByCommodityCode(code, orderPrice);
            }
            catch (Exception ex)
            {
                errMsg = "GT-0024：柜台连接失败,请检查柜台连接!";
                LogHelper.WriteError(ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// 根据商品代码和委托价格获取上下限（涨跌幅值）
        /// </summary>
        /// <param name="code">港股商品代码</param>
        /// <param name="orderPrice">委托价格</param>
        /// <param name="priceType">港股价格类型</param>
        /// <param name="tranType">交易方向</param>
        /// <returns></returns>
        public HighLowRangeValue GetHKHighLowRangeValueByCommodityCode(string code, decimal orderPrice, Types.HKPriceType priceType, Types.TransactionDirection tranType, out string errMsg)
        {
            try
            {
                errMsg = "";
                return accountClient.GetHKHighLowRangeValueByCommodityCode(code, orderPrice, priceType, tranType);
            }
            catch (Exception ex)
            {
                errMsg = "GT-0024：柜台连接失败,请检查柜台连接!";
                LogHelper.WriteError(ex.Message, ex);
                return null;
            }
        }
        /// <summary>
        /// 根据时间查询清算状态
        /// </summary>
        /// <param name="doneDate"></param>
        /// <returns></returns>
        public bool IsReckoningDone(DateTime doneDate)
        {
            bool x = accountClient.IsReckoningDone(doneDate);
            return x;
        }

        /// <summary>
        /// 是否正在清算
        /// </summary>
        /// <returns></returns>
        public bool IsReckoning()
        {
            bool x = accountClient.IsReckoning();
            return x;
        }
        /// <summary>
        /// 根据代码和代码类型获取最后成交价
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="breedClassType">所属商品类型（1-现货,2-商品期货,3-股指期货,4-港股)</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns>最新成交价</returns>
        public MarketDataLevel GetLastPricByCommodityCode(string code, int breedClassType, out string errMsg)
        {
            // decimal lastPrice = 0;
            MarketDataLevel level = new MarketDataLevel();
            try
            {
                level = accountClient.GetMarketDataInfoByCode(out errMsg, code, breedClassType);
                return level;


            }
            catch (Exception ex)
            {
                errMsg = "GT-0024：柜台连接失败,请检查柜台连接!";
                LogHelper.WriteError(ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// 转账方法
        /// </summary>
        /// <param name="freetranstre">帐号信息</param>
        /// <param name="currencyType">币种类型</param>
        /// <returns>转账是否成功</returns>
        public bool TwoAccountsFreeTransferFunds(FreeTransferEntity freetranstre, Types.CurrencyType currencyType, out string errMsg)
        {
            errMsg = "";
            bool x = accountClient.TwoAccountsFreeTransferFunds(out errMsg, freetranstre, currencyType);
            LogHelper.WriteInfo(errMsg);
            return x;
        }
        /// <summary>
        /// 获取资金帐号
        /// </summary>
        /// <param name="TradeID">交易员ID</param>
        /// <returns>转账是否成功</returns>
        public List<GTA.VTS.CustomersOrders.DoAccountManager.AccountFindResultEntity> FindAccount(string TradeID)
        {
            string errMsg = "";
            List<GTA.VTS.CustomersOrders.DoAccountManager.AccountFindResultEntity> list = new List<AccountFindResultEntity>();
            list = accountClient.FindAccount(out errMsg, TradeID, "");
            return list;
        }

        /// <summary>
        /// 追加资金
        /// </summary>
        /// <param name="addCapitalEntity">资金信息</param>
        /// <returns>追加资金是否成功</returns>
        public bool AddCapital(AddCapitalEntity addCapitalEntity, out string errMsg)
        {
            errMsg = "";
            // GTA.VTS.CustomersOrders.DoAccountManager.AddCapitalEntity addCapitalEntity=new AddCapitalEntity();
            bool x = accountClient.AddCapital(out errMsg, addCapitalEntity);
            LogHelper.WriteInfo(errMsg);
            return x;
        }
        #endregion

        #region XH

        #region 属性
        /// <summary>
        /// 现货资金
        /// </summary>
        public List<DoCommonQuery.XH_CapitalAccountTableInfo> XHCapital
        {
            get
            {
                string msg = "";

                var cap = QueryXHCapital(xhAccount, ref msg);
                List<DoCommonQuery.XH_CapitalAccountTableInfo> list = new List<DoCommonQuery.XH_CapitalAccountTableInfo>();
                if (cap != null)
                    list.Add(cap);

                return list;
            }
        }

        /// <summary>
        /// 现货持仓
        /// </summary>
        public List<DoCommonQuery.XH_AccountHoldTableInfo> XHHold
        {
            get
            {
                string msg = "";

                var list = QueryXHHold(xhAccount, ref msg);

                List<DoCommonQuery.XH_AccountHoldTableInfo> holds = new List<DoCommonQuery.XH_AccountHoldTableInfo>();

                if (list != null)
                {
                    foreach (var entity in list)
                    {
                        holds.Add(entity.HoldFindResult);
                    }
                }

                return holds;
            }
        }

        /// <summary>
        /// 现货当日委托
        /// </summary>
        public List<DoCommonQuery.XH_TodayEntrustTableInfo> XHTodayEntrust
        {
            get
            {
                string msg = "";

                return QueryXHTodayEntrust(xhAccount, ref msg);
            }
        }

        /// <summary>
        /// 现货当日成交
        /// </summary>
        public List<DoCommonQuery.XH_TodayTradeTableInfo> XHTodayTrade
        {
            get
            {
                string msg = "";

                return QueryXHTodayTrade(xhAccount, ref msg);
            }
        }

        #endregion

        #region 方法

        #region 下单
        /// <summary>
        /// 现货下单
        /// </summary>
        /// <param name="order">现货下单请求</param>
        /// <returns>委托返回信息</returns>
        public OrderResponse DoStockOrder(StockOrderRequest order)
        {
            if (!IsServiceOk)
                return new OrderResponse { OrderMessage = ServiceErrorMsg };

            order.ChannelID = Channelid;

            OrderResponse response = null;
            try
            {
                response = doOrderClient.DoStockOrder(order);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                response = new OrderResponse();
                response.IsSuccess = false;
                response.OrderMessage = ServiceErrorMsg;
                IsServiceOk = false;
            }

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entrustNumber"></param>
        /// <param name="channelID"></param>
        /// <param name="type">1现货，2期货</param>
        /// <returns></returns>
        public bool ChangeEntrustChannel(List<string> entrustNumber, string channelID, int type)
        {
            return rptClient.ChangeEntrustChannel(entrustNumber, channelID, type);
        }

        /// <summary>
        /// 现货撤单
        /// </summary>
        /// <param name="entrustNumber">委托编号</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns>是否成功</returns>
        public bool CancelStockOrder(string entrustNumber, ref string errMsg)
        {
            if (!IsServiceOk)
            {
                errMsg = ServiceErrorMsg;
                return false;
            }

            TypesOrderStateType statetype;
            return doOrderClient.CancelStockOrder(entrustNumber, ref errMsg, out statetype);
        }
        #endregion

        #region 查询
        /// <summary>
        /// 获取现货最大委托量
        /// </summary>
        /// <param name="traderId">交易员ID</param>
        /// <param name="code">商品代码</param>
        /// <param name="price">委托价格</param>
        /// <param name="orderPriceType">价格类型</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns>现货最大委托量</returns>
        public long GetXHMaxCount(string traderId, string code, decimal price, TypesOrderPriceType orderPriceType, out string errMsg)
        {
            return accountClient.GetSpotMaxOrderAmount(out errMsg, traderId, (float)price, code, orderPriceType);
        }

        /// <summary>
        /// 获取期货最大委托量
        /// </summary>
        /// <param name="traderId">交易员ID</param>
        /// <param name="code">商品代码</param>
        /// <param name="price">委托价格</param>
        /// <param name="orderPriceType">价格类型</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns>期货最大委托量</returns>
        public long GetQHMaxCount(string traderId, string code, decimal price, TypesOrderPriceType orderPriceType, out string errMsg)
        {
            return accountClient.GetFutureMaxOrderAmount(out errMsg, traderId, (float)price, code, orderPriceType);
        }
        /// <summary>
        /// 根据【现货资金账号】查询现货资金账号明细
        /// </summary>
        ///<param name="capitalAccount">现货资金账号</param>
        /// <param name="msg">异常信息</param>
        /// <returns>现货资金表实体</returns>
        public XH_CapitalAccountTableInfo QueryXHCapital(string capitalAccount, ref string msg)
        {
            if (!IsServiceOk)
            {
                return null;
            }
            var capital = traderQueryClient.QueryXH_CapitalAccountTableByAccount(out msg, capitalAccount, DoCommonQuery.QueryTypeQueryCurrencyType.RMB);
            if (capital != null && capital.Count > 0)
            {
                return capital[0];
            }
            return null;

        }

        /// <summary>
        /// 现货持仓查询
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>现货持仓实体</returns>
        public List<SpotHoldFindResultEntity> QueryXHHold(string capitalAccount, ref string strMessage)
        {
            if (!IsServiceOk)
                return null;

            int icount;
            var shcfe = new DoCommonQuery.SpotHoldConditionFindEntity();
            string UserId = ServerConfig.TraderID;
            string PassWord = ServerConfig.PassWord;
            var holds = traderQueryClient.PagingQuerySpotHold(out icount, UserId, 2, PassWord, null, 0, int.MaxValue, ref strMessage);
            return holds;
        }

        /// <summary>
        /// 现货当日委托查询
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>当日委托实体</returns>
        public List<XH_TodayEntrustTableInfo> QueryXHTodayEntrust(string capitalAccount, ref string strMessage)
        {
            if (!IsServiceOk)
                return new List<DoCommonQuery.XH_TodayEntrustTableInfo>();

            int icount;
            var secfe = new DoCommonQuery.SpotEntrustConditionFindEntity();
            secfe.EntrustNumber = CurrentQueryValue.QueryXHEntrustNO;
            string UserId = ServerConfig.TraderID;
            string PassWord = ServerConfig.PassWord;
            string message;
            GTA.VTS.CustomersOrders.DoCommonQuery.PagingInfo pagingInfo = new PagingInfo();
            pagingInfo.CurrentPage = 1;
            pagingInfo.PageLength = int.MaxValue;
            var tets = traderQueryClient.PagingQueryXH_TodayEntrustInfoByFilterAndUserIDPwd(out icount, out message, UserId, PassWord, 2, secfe, pagingInfo);
            return tets;
        }

        /// <summary>
        /// 现货当日委托查询
        /// </summary>
        /// <param name="icount">记录数</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="accountType">账号类型（注：这是账户类型不是类别，这里是有九种类型，而类别是五种）</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>历史委托实体列表</returns>
        public List<XH_TodayEntrustTableInfo> QueryXHTodayEntrust(out int total, out string errorMsg, string userID, string pwd, int accountType, SpotEntrustConditionFindEntity filter, PagingInfo pageInfo)
        {
            total = 0;
            errorMsg = "";
            if (!IsServiceOk)
            {
                return new List<DoCommonQuery.XH_TodayEntrustTableInfo>();
            }

            var tets = traderQueryClient.PagingQueryXH_TodayEntrustInfoByFilterAndUserIDPwd(out total, out errorMsg, userID, pwd, 2, filter, pageInfo);
            return tets;
        }



        /// <summary>
        /// 根据交易员ID查询出对应银行卡资金
        /// </summary>
        /// <returns></returns>
        public List<UA_BankAccountTableInfo> QueryUA_BankAccountByUserID()
        {
            try
            {
                string errMsg = "";
                string userId = ServerConfig.TraderID;
                return traderQueryClient.QueryUA_BankAccountByUserID(out errMsg, userId);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return null;
            }
        }
        /// <summary>
        /// 现货当日成交查询
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>现货当日成交实体</returns>
        public List<XH_TodayTradeTableInfo> QueryXHTodayTrade(string capitalAccount, ref string strMessage)
        {
            if (!IsServiceOk)
                return new List<DoCommonQuery.XH_TodayTradeTableInfo>();

            int icount;
            var secfe = new DoCommonQuery.SpotTradeConditionFindEntity();
            secfe.EntrustNumber = CurrentQueryValue.QueryXHTradeNO;
            string UserId = ServerConfig.TraderID;
            string PassWord = ServerConfig.PassWord;
            PagingInfo pagingInfo = new PagingInfo();
            pagingInfo.PageLength = int.MaxValue;
            pagingInfo.CurrentPage = 1;
            var trades = traderQueryClient.PagingQueryXH_TodayTradeByFilterAndUserIDPwd(out icount, out strMessage, UserId, PassWord, 2, secfe, pagingInfo);
            return trades;
        }

        /// <summary>
        /// 现货资金情况查询
        /// </summary>
        /// <param name="type">币种</param>
        /// <param name="msg">异常信息</param>
        /// <returns>现货资金实体</returns>
        public SpotCapitalEntity QueryXHTotalCapital(Types.CurrencyType type, ref string msg)
        {
            if (!IsServiceOk)
            {
                return null;
            }
            return traderQueryClient.PagingQuerySpotCapital(ServerConfig.TraderID, 2, type, ServerConfig.PassWord, ref msg);
        }

        /// <summary>
        /// 现货历史委托查询
        /// </summary>
        /// <param name="icount">记录总数</param>
        /// /// <param name="currentPage">当前页码</param>
        /// <param name="pageLength">页记录数</param>
        /// <param name="isCount">是否计算总记录数</param>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strMessage">异常信息</param>
        /// <param name="sDate">查询开始时间</param>
        /// <param name="eDate">查询结束时间</param>
        /// <returns>历史委托实体</returns>
        public List<XH_HistoryEntrustTableInfo> QueryXHHistoryEntrust(out int icount, int currentPage, int pageLength, bool isCount, string capitalAccount, ref string strMessage, DateTime? sDate, DateTime? eDate)
        {
            if (!IsServiceOk)
            {
                icount = 0;
                return new List<XH_HistoryEntrustTableInfo>();
            }
            PagingInfo pageInfo = new PagingInfo();
            pageInfo.CurrentPage = currentPage;
            pageInfo.PageLength = pageLength;
            pageInfo.IsCount = isCount;
            SpotEntrustConditionFindEntity findEt = new SpotEntrustConditionFindEntity();
            if (sDate.HasValue && eDate.HasValue)
            {
                findEt.StartTime = sDate.Value;
                findEt.EndTime = eDate.Value;
            }
            var tets = traderQueryClient.PagingQueryXH_HistoryEntrustInfoByFilterAndUserIDPwd(out icount, out strMessage, ServerConfig.TraderID, ServerConfig.PassWord, 2, findEt, pageInfo);
            return tets;
        }
        /// <summary>
        /// 现货历史委托查询
        /// </summary>
        /// <param name="icount">记录总数</param>
        /// /// <param name="currentPage">当前页码</param>
        /// <param name="pageLength">页记录数</param>
        /// <param name="isCount">是否计算总记录数</param>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strMessage">异常信息</param>
        /// <param name="sDate">查询开始时间</param>
        /// <param name="eDate">查询结束时间</param>
        /// <returns>历史委托实体</returns>
        public List<XH_HistoryEntrustTableInfo> QueryXHHistoryEntrust(out int icount, out string strMessage, SpotEntrustConditionFindEntity filter, PagingInfo pageInfo)
        {
            icount = 0;
            strMessage = "";
            if (!IsServiceOk)
            {
                return new List<XH_HistoryEntrustTableInfo>();
            }

            var tets = traderQueryClient.PagingQueryXH_HistoryEntrustInfoByFilterAndUserIDPwd(out icount, out strMessage, ServerConfig.TraderID, "", 2, filter, pageInfo);
            return tets;
        }

        /// <summary>
        /// 现货当日委托查询
        /// </summary>
        /// <param name="icount">记录总数</param>
        /// /// <param name="currentPage">当前页码</param>
        /// <param name="pageLength">页记录数</param>
        /// <param name="isCount">是否计算总记录数</param>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>历史委托实体</returns>
        public List<XH_TodayEntrustTableInfo> QueryXHTodayEntrust(out int icount, int currentPage, int pageLength, bool isCount, string capitalAccount, ref string strMessage)
        {
            icount = 0;
            if (!IsServiceOk)
                return new List<DoCommonQuery.XH_TodayEntrustTableInfo>();

            //int icount;
            var secfe = new DoCommonQuery.SpotEntrustConditionFindEntity();
            secfe.EntrustNumber = CurrentQueryValue.QueryXHEntrustNO;
            string UserId = ServerConfig.TraderID;
            string PassWord = ServerConfig.PassWord;
            string message;
            PagingInfo pageInfo = new PagingInfo();
            pageInfo.CurrentPage = currentPage;
            pageInfo.PageLength = pageLength;
            pageInfo.IsCount = true;
            var tets = traderQueryClient.PagingQueryXH_TodayEntrustInfoByFilterAndUserIDPwd(out icount, out message, UserId, PassWord, 2, secfe, pageInfo);
            return tets;
        }
        /// <summary>
        /// 现货历史成交查询
        /// </summary>
        /// <param name="icount">记录总数</param>
        /// /// <param name="currentPage">当前页码</param>
        /// <param name="pageLength">页记录数</param>
        /// <param name="isCount">是否计算总记录数</param>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>历史成交实体</returns>
        public List<XH_HistoryTradeTableInfo> QueryXHHistoryTrade(out int icount, int currentPage, int pageLength, bool isCount, string capitalAccount, ref string strMessage, DateTime? sDate, DateTime? eDate)
        {
            if (!IsServiceOk)
            {
                icount = 0;
                return new List<XH_HistoryTradeTableInfo>();
            }
            PagingInfo pageInfo = new PagingInfo();
            pageInfo.CurrentPage = currentPage;
            pageInfo.PageLength = pageLength;
            pageInfo.IsCount = isCount;
            SpotTradeConditionFindEntity findEt = new SpotTradeConditionFindEntity();
            if (sDate.HasValue && eDate.HasValue)
            {
                findEt.StartTime = sDate.Value;
                findEt.EndTime = eDate.Value;
            }
            var tets = traderQueryClient.PagingQueryXH_HistoryTradeByFilterAndUserIDPwd(out icount, out strMessage, ServerConfig.TraderID, ServerConfig.PassWord, 2, findEt, pageInfo);
            return tets;
        }

        /// <summary>
        /// 现货当日成交查询
        /// </summary>
        /// <param name="icount">记录总数</param>
        /// /// <param name="currentPage">当前页码</param>
        /// <param name="pageLength">页记录数</param>
        /// <param name="isCount">是否计算总记录数</param>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>历史成交实体</returns>
        public List<XH_TodayTradeTableInfo> QueryXHTodayTrade(out int icount, int currentPage, int pageLength, bool isCount, string capitalAccount, ref string strMessage)
        {
            if (!IsServiceOk)
            {
                icount = 0;
                return new List<XH_TodayTradeTableInfo>();
            }
            var secfe = new DoCommonQuery.SpotTradeConditionFindEntity();
            secfe.EntrustNumber = CurrentQueryValue.QueryXHTradeNO;
            string UserId = ServerConfig.TraderID;
            string PassWord = ServerConfig.PassWord;
            PagingInfo pageInfo = new PagingInfo();
            pageInfo.CurrentPage = currentPage;
            pageInfo.PageLength = pageLength;
            pageInfo.IsCount = true;
            var trades = traderQueryClient.PagingQueryXH_TodayTradeByFilterAndUserIDPwd(out icount, out strMessage, UserId, PassWord, 2, secfe, pageInfo);
            return trades;
        }

        /// <summary>
        /// 现货综合查询
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>查询数据汇总实体</returns>
        public List<HKMarketValue> QuerymarketValueXHHold(string code, ref string strMessage)
        {
            List<HKMarketValue> list = new List<HKMarketValue>();

            if (!IsServiceOk)
                return null;

            int icount;
            var shcfe = new DoCommonQuery.SpotHoldConditionFindEntity();
            if (!string.IsNullOrEmpty(code))
            {
                shcfe.SpotCode = code;
            }
            string UserId = ServerConfig.TraderID;
            string PassWord = ServerConfig.PassWord;
            var holds = traderQueryClient.PagingQuerySpotHold(out icount, UserId, 2, PassWord, shcfe, 0, int.MaxValue, ref strMessage);
            if (holds == null)
            {
                return list;
            }
            foreach (var item in holds)
            {
                HKMarketValue hkmare = new HKMarketValue();
                hkmare.BelongMarket = item.BelongMarket;
                hkmare.BreakevenPrice = item.HoldFindResult.BreakevenPrice;
                hkmare.Code = item.HoldFindResult.Code;
                hkmare.CostPrice = item.HoldFindResult.CostPrice;
                hkmare.CurrencyName = item.CurrencyName;
                hkmare.ErroNumber = item.ErroNumber;
                hkmare.ErroReason = item.ErroReason;
                hkmare.FloatProfitLoss = item.FloatProfitLoss;
                hkmare.HKName = item.SpotName;
                hkmare.HoldAveragePrice = item.HoldFindResult.HoldAveragePrice;
                hkmare.HoldSumAmount = item.HoldSumAmount;
                hkmare.MarketValue = item.MarketValue;
                hkmare.RealtimePrice = item.RealtimePrice;
                hkmare.TraderId = item.TraderId;
                hkmare.VarietyCategories = item.VarietyCategories;
                list.Add(hkmare);
            }
            return list;
        }

        #endregion
        #endregion

        #endregion

        #region HK

        #region 属性
        /// <summary>
        /// 港股资金信息
        /// </summary>
        public List<HK_CapitalAccountInfo> HKCapital
        {
            get
            {
                string msg = "";

                var cap = QueryHKCapital(hkAccount, ref msg);
                List<HK_CapitalAccountInfo> list = new List<HK_CapitalAccountInfo>();
                if (cap != null)
                    list.Add(cap);

                return list;
            }
        }

        /// <summary>
        /// 港股持仓信息
        /// </summary>
        public List<HK_AccountHoldInfo> HKHold
        {
            get
            {
                string msg = "";

                var list = QueryHK_AccountHoldByAccount("", hkAccount, ref msg);
                return list;
            }
        }

        /// <summary>
        /// 港股当日委托
        /// </summary>
        public List<HKCommonQuery.HK_TodayEntrustInfo> HKTodayEntrust
        {
            get
            {
                string msg = "";

                return QueryHKTodayEntrust(hkAccount, ref msg);
            }
        }

        /// <summary>
        /// 港股当日成交
        /// </summary>
        public List<HKCommonQuery.HK_TodayTradeInfo> HKTodayTrade
        {
            get
            {
                string msg = "";

                return QueryHKTodayTrade(hkAccount, ref msg);
            }
        }
        #endregion

        #region 方法

        #region 下单
        /// <summary>
        /// 港股下单
        /// </summary>
        /// <param name="order">港股下单请求</param>
        /// <returns>委托返回信息</returns>
        public OrderResponse DoHKOrder(HKOrderRequest order)
        {
            if (!IsServiceOk)
                return new OrderResponse { OrderMessage = ServiceErrorMsg };

            order.ChannelID = Channelid;

            OrderResponse response = null;
            try
            {
                response = doOrderClient.DoHKOrder(order);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                response = new OrderResponse();
                response.IsSuccess = false;
                response.OrderMessage = ServiceErrorMsg;
                IsServiceOk = false;
            }

            return response;
        }

        /// <summary>
        /// 港股改单
        /// </summary>
        /// <param name="order">港股改单请求</param>
        /// <returns>委托返回信息</returns>
        public OrderResponse ModifyHKOrder(HKModifyOrderRequest order)
        {
            if (!IsServiceOk)
                return new OrderResponse { OrderMessage = ServiceErrorMsg };

            order.ChannelID = Channelid;

            OrderResponse response = null;
            try
            {
                response = doOrderClient.DoHKModifyOrder(order);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                response = new OrderResponse();
                response.IsSuccess = false;
                response.OrderMessage = ServiceErrorMsg;
                IsServiceOk = false;
            }

            return response;
        }

        /// <summary>
        /// 港股撤单
        /// </summary>
        /// <param name="entrustNumber">委托编号</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns>是否成功</returns>
        public bool CancelHKOrder(string entrustNumber, ref string errMsg)
        {
            if (!IsServiceOk)
            {
                errMsg = ServiceErrorMsg;
                return false;
            }

            TypesOrderStateType statetype;
            return doOrderClient.CancelHKOrder(entrustNumber, ref errMsg, out statetype);
        }

        #endregion

        #region 查询

        /// <summary>
        /// 求港股最大委托量
        /// </summary>
        /// <param name="traderId">交易员ID</param>
        /// <param name="code">商品代码</param>
        /// <param name="price">委托价格</param>
        /// <param name="orderPriceType">港股价格类型</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns>港股最大委托量</returns>
        public long GetHKMaxCount(string traderId, string code, decimal price, Types.HKPriceType orderPriceType, out string errMsg)
        {
            return accountClient.GetHKMaxOrderAmount(out errMsg, traderId, (float)price, code, orderPriceType);
        }

        /// <summary>
        /// 港股资金明细查询
        /// </summary>
        /// <param name="type">币种</param>
        /// <param name="msg">异常信息</param>
        /// <returns>港股资金实体</returns>
        public HKCommonQuery.HKCapitalEntity QueryHKTotalCapital(Types.CurrencyType type, ref string msg)
        {
            if (!IsServiceOk)
            {
                return null;
            }
            var capital = hkTraderQueryClient.PagingQueryHKCapitalDetail(ServerConfig.TraderID, ServerConfig.PassWord, 8, type, ref msg);
            return capital;
        }

        /// <summary>
        /// 根据【港股资金账号】查询港股资金账号明细
        /// </summary>
        /// <param name="capitalAccount">港股资金账号</param>
        /// <param name="msg">异常信息</param>
        /// <returns>港股资金账户实体</returns>
        public HK_CapitalAccountInfo QueryHKCapital(string capitalAccount, ref string msg)
        {
            if (!IsServiceOk)
                return null;

            var capital = hkTraderQueryClient.QueryHK_CapitalAccountByAccount(out msg, capitalAccount, HKCommonQuery.QueryTypeQueryCurrencyType.HK);
            if (capital != null && capital.Count > 0)
            {
                return capital[0];
            }
            return null;


        }

        /// <summary>
        /// 根据【港股持仓账号和货币类型】查询港股持仓账号明细
        /// </summary>
        /// <param name="code"></param>
        /// <param name="hkAccount">港股持仓账号</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>港股持仓实体</returns>
        public List<HK_AccountHoldInfo> QueryHK_AccountHoldByAccount(string code, string hkAccount, ref string strMessage)
        {
            if (!IsServiceOk)
                return null;
            //特殊处理持仓账号
            string hkHoldAccount = hkAccount.Substring(0, hkAccount.Length - 1) + "9"; ;

            var holds = hkTraderQueryClient.QueryHK_AccountHoldByAccount(out strMessage, hkHoldAccount, HKCommonQuery.QueryTypeQueryCurrencyType.HK);

            return holds;
        }

        /// <summary>
        /// 港股持仓查询
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>港股持仓实体</returns>
        public List<HKHoldFindResultyEntity> QueryHKHold(string code, string capitalAccount, ref string strMessage)
        {
            if (!IsServiceOk)
                return null;

            int icount;
            var shcfe = new HKCommonQuery.HKHoldConditionFindEntity();
            if (!string.IsNullOrEmpty(code))
            {
                shcfe.HKCode = code;
            }
            var holds = hkTraderQueryClient.PagingQueryHKHold(out icount, ServerConfig.TraderID, ServerConfig.PassWord, 8, shcfe, 1, int.MaxValue, ref strMessage);
            return holds;
        }

        /// <summary>
        /// 港股持仓查询
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>港股持仓实体</returns>
        public List<HKHoldFindResultyEntity> QueryHKHold(string code, ref string strMessage)
        {
            return QueryHKHold(code, hkAccount, ref strMessage);
        }

        /// <summary>
        /// 港股汇总查询
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>港股查询数据汇总实体</returns>
        public List<HKMarketValue> QueryMarketValueHKHold(string code, ref string strMessage)
        {
            List<HKMarketValue> list = new List<HKMarketValue>();

            List<HKCommonQuery.HKHoldFindResultyEntity> holdMarket = QueryHKHold(code, hkAccount, ref strMessage);
            if (holdMarket == null)
            {
                return list;
            }
            foreach (var item in holdMarket)
            {
                HKMarketValue hkmare = new HKMarketValue();
                hkmare.BelongMarket = item.BelongMarket;
                hkmare.BreakevenPrice = item.HoldFindResult.BreakevenPrice;
                hkmare.Code = item.HoldFindResult.Code;
                hkmare.CostPrice = item.HoldFindResult.CostPrice;
                hkmare.CurrencyName = item.CurrencyName;
                hkmare.ErroNumber = item.ErroNumber;
                hkmare.ErroReason = item.ErroReason;
                hkmare.FloatProfitLoss = item.FloatProfitLoss;
                hkmare.HKName = item.HKName;
                hkmare.HoldAveragePrice = item.HoldFindResult.HoldAveragePrice;
                hkmare.HoldSumAmount = item.HoldSumAmount;
                hkmare.MarketValue = item.MarketValue;
                hkmare.RealtimePrice = item.RealtimePrice;
                hkmare.TraderId = item.TraderId;
                hkmare.VarietyCategories = item.VarietyCategories;
                list.Add(hkmare);
            }
            return list;
        }

        /// <summary>
        /// 港股当日委托查询
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>港股当日委托实体</returns>
        public List<HK_TodayEntrustInfo> QueryHKTodayEntrust(string capitalAccount, ref string strMessage)
        {
            if (!IsServiceOk)
            {
                return new List<HK_TodayEntrustInfo>();
            }
            List<HK_TodayEntrustInfo> tets = new List<HK_TodayEntrustInfo>();

            HKCommonQuery.PagingInfo pageInfo = new HKCommonQuery.PagingInfo();
            pageInfo.CurrentPage = 1;
            pageInfo.PageLength = int.MaxValue;

            int icount;
            var secfe = new HKEntrustConditionFindEntity();
            if (!string.IsNullOrEmpty(CurrentQueryValue.QueryHKEnNO))
            {
                tets = QueryHKTodayEntrust(capitalAccount, CurrentQueryValue.QueryHKEnNO, ref strMessage);
            }
            else
            {
                tets = hkTraderQueryClient.PagingQueryHK_TodayEntrustByFilterAndUserIDPwd(out icount, out strMessage, ServerConfig.TraderID, ServerConfig.PassWord, 8, secfe, pageInfo);
            }
            return tets;
        }

        /// <summary>
        /// 港股当日委托查询
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="entrustNumber">委托编号</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>港股当日委托实体</returns>
        public List<HKCommonQuery.HK_TodayEntrustInfo> QueryHKTodayEntrust(string capitalAccount, string entrustNumber, ref string strMessage)
        {
            if (!IsServiceOk)
                return new List<HKCommonQuery.HK_TodayEntrustInfo>();

            int icount;
            var secfe = new HKCommonQuery.HKEntrustConditionFindEntity();
            secfe.HKCapitalAccount = capitalAccount;
            secfe.EntrustNumber = entrustNumber;
            HKCommonQuery.PagingInfo pageInfo = new HKCommonQuery.PagingInfo();
            pageInfo.CurrentPage = 1;
            pageInfo.PageLength = int.MaxValue;
            var tets = hkTraderQueryClient.PagingQueryHK_TodayEntrustByFilterAndUserIDPwd(out icount, out strMessage, ServerConfig.TraderID, ServerConfig.PassWord, 8, secfe, pageInfo);
            return tets;
        }

        /// <summary>
        /// 港股当日委托查询
        /// </summary>
        /// <param name="icount">记录数</param>
        /// <param name="currentPage">页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>港股当日委托实体</returns>
        public List<HK_TodayEntrustInfo> QueryHKTodayEntrust(out int icount, string capitalAccount, string entrustNumber, int currentPage, int pageSize, out string strMessage)
        {
            icount = 0;
            strMessage = "";
            if (!IsServiceOk)
            {
                return new List<HK_TodayEntrustInfo>();
            }
            var secfe = new HKCommonQuery.HKEntrustConditionFindEntity();
            secfe.HKCapitalAccount = capitalAccount;
            secfe.EntrustNumber = entrustNumber;

            HKCommonQuery.PagingInfo pinfo = new HKCommonQuery.PagingInfo();
            pinfo.CurrentPage = currentPage;
            pinfo.PageLength = pageSize;
            pinfo.IsCount = true;
            // var tets = hkTraderQueryClient.PagingQueryHK_TodayEntrustByFilterAndUserIDPwd(out icount, out strMessage, ServerConfig.TraderID, ServerConfig.PassWord, 8, secfe, pinfo);
            var tets = QueryHKTodayEntrust(out icount, out strMessage, secfe, pinfo);
            return tets;
        }
        /// <summary>
        /// 港股当日委托查询
        /// </summary>
        /// <param name="icount">记录数</param>
        /// <param name="currentPage">页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>港股当日委托实体</returns>
        public List<HK_TodayEntrustInfo> QueryHKTodayEntrust(out int icount, out string strMessage, HKEntrustConditionFindEntity filter, HKCommonQuery.PagingInfo pageInfo)
        {
            icount = 0;
            strMessage = "";
            if (!IsServiceOk)
            {
                return new List<HK_TodayEntrustInfo>();
            }
            var tets = hkTraderQueryClient.PagingQueryHK_TodayEntrustByFilterAndUserIDPwd(out icount, out strMessage, ServerConfig.TraderID, "", 8, filter, pageInfo);
            return tets;
        }
        /// <summary>
        /// 港股当日成交查询
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>港股当日成交实体</returns>
        public List<HKCommonQuery.HK_TodayTradeInfo> QueryHKTodayTrade(string capitalAccount, ref string strMessage)
        {
            if (!IsServiceOk)
                return new List<HKCommonQuery.HK_TodayTradeInfo>();

            int icount;
            HKCommonQuery.HKTradeConditionFindEntity filter = new HKCommonQuery.HKTradeConditionFindEntity();
            HKCommonQuery.PagingInfo pageInfo = new GTA.VTS.CustomersOrders.HKCommonQuery.PagingInfo();
            pageInfo.PageLength = int.MaxValue;
            pageInfo.CurrentPage = 1;
            if (!string.IsNullOrEmpty(CurrentQueryValue.QueryHKTradeNO))
            {
                filter.EntrustNumber = CurrentQueryValue.QueryHKTradeNO;
            }
            else
            {
                filter = null;
            }
            var trades = hkTraderQueryClient.PagingQueryHK_TodayTradeByFilterAndUserIDPwd(out icount, out strMessage, ServerConfig.TraderID, ServerConfig.PassWord, 8, filter, pageInfo);
            return trades;
        }

        /// <summary>
        /// 港股当日成交查询
        /// </summary>
        /// <param name="icount"></param>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="entrustNumber">委托号</param>
        /// <param name="currentPage">页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>港股当日成交实体</returns>
        public List<HK_TodayTradeInfo> QueryHKTodayTrade(out int icount, string capitalAccount, string entrustNumber, int currentPage, int pageSize, out string strMessage)
        {
            icount = 0;
            strMessage = "";
            if (!IsServiceOk)
            {
                return new List<HK_TodayTradeInfo>();
            }
            var secfe = new HKCommonQuery.HKTradeConditionFindEntity();
            secfe.HKCapitalAccount = capitalAccount;
            secfe.EntrustNumber = entrustNumber;

            HKCommonQuery.PagingInfo pinfo = new HKCommonQuery.PagingInfo();
            pinfo.CurrentPage = currentPage;
            pinfo.PageLength = pageSize;
            pinfo.IsCount = true;
            var tets = hkTraderQueryClient.PagingQueryHK_TodayTradeByFilterAndUserIDPwd(out icount, out strMessage, ServerConfig.TraderID, ServerConfig.PassWord, 8, secfe, pinfo);
            return tets;
        }

        /// <summary>
        /// 港股历史委托查询
        /// </summary>
        /// <param name="entrustNumber">委托编号</param>
        /// <param name="startTime">查询起始时间</param>
        /// <param name="endTime">查询结束时间</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>港股历史委托实体</returns>
        public List<HKCommonQuery.HK_HistoryEntrustInfo> QueryHKHisotryEntrust(string entrustNumber, DateTime? startTime, DateTime? endTime, ref string strMessage)
        {

            if (!IsServiceOk)
                return new List<HKCommonQuery.HK_HistoryEntrustInfo>();
            List<HKCommonQuery.HK_HistoryEntrustInfo> tets = new List<HKCommonQuery.HK_HistoryEntrustInfo>();
            int icount;
            var secfe = new HKCommonQuery.HKEntrustConditionFindEntity();
            if (!string.IsNullOrEmpty(entrustNumber))
            {
                secfe.EntrustNumber = entrustNumber;
            }
            if (startTime.HasValue && endTime.HasValue)
            {
                secfe.StartTime = startTime.Value;
                secfe.EndTime = endTime.Value;
            }
            HKCommonQuery.PagingInfo pageInfo = new GTA.VTS.CustomersOrders.HKCommonQuery.PagingInfo();
            pageInfo.CurrentPage = 1;
            pageInfo.PageLength = int.MaxValue;
            tets = hkTraderQueryClient.PagingQueryHK_HistoryEntrustByFilterAndUserIDPwd(out icount, out strMessage, ServerConfig.TraderID, ServerConfig.PassWord, 8, secfe, pageInfo);
            return tets;
        }

        /// <summary>
        /// 港股历史委托查询
        /// </summary>
        /// <param name="entrustNumber">委托编号</param>
        /// <param name="startTime">查询起始时间</param>
        /// <param name="endTime">查询结束时间</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>港股历史委托实体</returns>
        public List<HKCommonQuery.HK_HistoryEntrustInfo> QueryHKHisotryEntrust(out int icount, out string strMessage, HKEntrustConditionFindEntity secfe, HKCommonQuery.PagingInfo pageInfo)
        {
            icount = 0;
            strMessage = "";
            if (!IsServiceOk)
            {
                return new List<HKCommonQuery.HK_HistoryEntrustInfo>();
            }

            var tets = hkTraderQueryClient.PagingQueryHK_HistoryEntrustByFilterAndUserIDPwd(out icount, out strMessage, ServerConfig.TraderID, "", 8, secfe, pageInfo);
            return tets;
        }


        public List<HK_HistoryEntrustInfo> QueryHKHisotryEntrust(out int icount, string capitalAccount, string entrustNumber, int currentPage, int pageSize, DateTime? startTime, DateTime? endTime, out string strMessage)
        {
            icount = 0;
            strMessage = "";
            if (!IsServiceOk)
            {
                return new List<HKCommonQuery.HK_HistoryEntrustInfo>();
            }
            List<HKCommonQuery.HK_HistoryEntrustInfo> tets = new List<HKCommonQuery.HK_HistoryEntrustInfo>();

            var secfe = new HKCommonQuery.HKEntrustConditionFindEntity();
            if (!string.IsNullOrEmpty(entrustNumber))
            {
                secfe.EntrustNumber = entrustNumber;
            }
            if (startTime.HasValue && endTime.HasValue)
            {
                secfe.StartTime = startTime.Value;
                secfe.EndTime = endTime.Value;
            }
            HKCommonQuery.PagingInfo pageInfo = new GTA.VTS.CustomersOrders.HKCommonQuery.PagingInfo();
            pageInfo.CurrentPage = currentPage;
            pageInfo.PageLength = pageSize;
            pageInfo.IsCount = true;
            tets = hkTraderQueryClient.PagingQueryHK_HistoryEntrustByFilterAndUserIDPwd(out icount, out strMessage, ServerConfig.TraderID, ServerConfig.PassWord, 8, secfe, pageInfo);
            return tets;
        }

        /// <summary>
        /// 港股历史成交查询
        /// </summary>
        /// <param name="entrustNumber">委托编号</param>
        /// <param name="startTime">查询起始时间</param>
        /// <param name="endTime">查询结束时间</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>港股历史成交实体</returns>
        public List<HKCommonQuery.HK_HistoryTradeInfo> QueryHKHisotryTrade(string entrustNumber, DateTime? startTime, DateTime? endTime, ref string strMessage)
        {
            if (!IsServiceOk)
                return new List<HKCommonQuery.HK_HistoryTradeInfo>();
            List<HKCommonQuery.HK_HistoryTradeInfo> tets = new List<HKCommonQuery.HK_HistoryTradeInfo>();
            int icount;
            var secfe = new HKCommonQuery.HKTradeConditionFindEntity();
            if (!string.IsNullOrEmpty(entrustNumber))
            {
                secfe.EntrustNumber = entrustNumber;
            }
            if (startTime.HasValue && endTime.HasValue)
            {
                secfe.StartTime = startTime.Value;
                secfe.EndTime = endTime.Value;
            }
            HKCommonQuery.PagingInfo pageInfo = new GTA.VTS.CustomersOrders.HKCommonQuery.PagingInfo();
            pageInfo.CurrentPage = 1;
            pageInfo.PageLength = int.MaxValue;
            tets = hkTraderQueryClient.PagingQueryHK_HistoryTradeByFilterAndUserIDPwd(out icount, out strMessage, ServerConfig.TraderID, ServerConfig.PassWord, 8, secfe, pageInfo);
            return tets;
        }

        /// <summary>
        /// 港股历史成交分页查询
        /// </summary>
        /// <param name="icount">记录总数</param>
        /// <param name="capitalAccount">资金账户</param>
        /// <param name="entrustNumber">委托号</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns></returns>
        public List<HK_HistoryTradeInfo> QueryHKHisotryTrade(out int icount, string capitalAccount, string entrustNumber, int currentPage, int pageSize, DateTime? startTime, DateTime? endTime, out string strMessage)
        {
            icount = 0;
            strMessage = "";
            if (!IsServiceOk)
            {
                return new List<HKCommonQuery.HK_HistoryTradeInfo>();
            }
            List<HKCommonQuery.HK_HistoryTradeInfo> tets = new List<HKCommonQuery.HK_HistoryTradeInfo>();

            var secfe = new HKCommonQuery.HKTradeConditionFindEntity();
            if (!string.IsNullOrEmpty(entrustNumber))
            {
                secfe.EntrustNumber = entrustNumber;
            }
            if (startTime.HasValue && endTime.HasValue)
            {
                secfe.StartTime = startTime.Value;
                secfe.EndTime = endTime.Value;
            }
            HKCommonQuery.PagingInfo pageInfo = new GTA.VTS.CustomersOrders.HKCommonQuery.PagingInfo();
            pageInfo.CurrentPage = currentPage;
            pageInfo.PageLength = pageSize;
            pageInfo.IsCount = true;
            tets = hkTraderQueryClient.PagingQueryHK_HistoryTradeByFilterAndUserIDPwd(out icount, out strMessage, ServerConfig.TraderID, ServerConfig.PassWord, 8, secfe, pageInfo);
            return tets;
        }

        /// <summary>
        /// 查询当日改单请求记录
        /// </summary>
        /// <param name="entrustNumber">委托单编号（被改单编号）</param>
        /// <param name="startTime">查询开始时间(如果为查询当日的可以为空)</param>
        /// <param name="endTime">查询结束时间(如果为查询当日的可以为空)</param>
        /// <param name="strMessage">异常信息</param>
        /// <param name="selectType">查询类型(0-查询所有即历史和当日，1-表时当日，2-表时历史)</param>
        /// <returns>港股改单历史表实体</returns>
        public List<HK_HistoryModifyOrderRequestInfo> QueryHKModifyOrderRequest(string entrustNumber, DateTime? startTime, DateTime? endTime, ref string strMessage, int selectType)
        {

            if (!IsServiceOk)
                return new List<HKCommonQuery.HK_HistoryModifyOrderRequestInfo>();
            HKCommonQuery.PagingInfo pageInfo = new HKCommonQuery.PagingInfo();
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = false;
            pageInfo.PageLength = int.MaxValue;

            List<HKCommonQuery.HK_HistoryModifyOrderRequestInfo> tets = new List<HKCommonQuery.HK_HistoryModifyOrderRequestInfo>();
            int icount;
            try
            {
                HKCommonQuery.HKTraderQueryClient cline = new HKCommonQuery.HKTraderQueryClient();

                tets = cline.PagingQueryHK_ModifyRequertByUserIDOrEntrustNo(out icount, out strMessage, traderId, entrustNumber, startTime, endTime, selectType, pageInfo);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
            return tets;
        }

        /// <summary>
        /// 查询持仓包括统计有盈亏
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="pwd">交易员密码</param>
        /// <returns>港股持仓查询汇总实体</returns>
        public List<HKHoldFindResultyEntity> QueryHKHoldMarketValue(string code, string pwd)
        {
            List<HKHoldFindResultyEntity> list = new List<HKHoldFindResultyEntity>();
            int count = 0;
            HKHoldConditionFindEntity findConndition = new HKHoldConditionFindEntity();
            findConndition.HKCode = code;
            string errMesg = "";
            list = hkTraderQueryClient.PagingQueryHKHold(out count, traderId, pwd, 9, findConndition, 0, 50, ref errMesg);
            return list;
        }

        #endregion

        #endregion

        #endregion

        #region QH

        #region 下单
        /// <summary>
        /// 股指期货撤单
        /// </summary>
        /// <param name="entrustNumber">委托编号</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns>是否成功</returns>
        public bool CancelGZQHOrder(string entrustNumber, ref string errMsg)
        {
            if (!IsServiceOk)
            {
                errMsg = ServiceErrorMsg;
                return false;
            }

            TypesOrderStateType statetype;
            return doOrderClient.CancelStockIndexFuturesOrder(entrustNumber, ref errMsg, out statetype);
        }

        /// <summary>
        /// 股指期货下单
        /// </summary>
        /// <param name="order">股指期货下单请求</param>
        /// <returns>委托返回信息</returns>
        public OrderResponse DoGZQHOrder(StockIndexFuturesOrderRequest order)
        {
            if (!IsServiceOk)
                return new OrderResponse { OrderMessage = ServiceErrorMsg };

            order.ChannelID = Channelid;

            OrderResponse response = null;
            try
            {
                response = doOrderClient.DoStockIndexFuturesOrder(order);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                response = new OrderResponse();
                response.IsSuccess = false;
                response.OrderMessage = ServiceErrorMsg;
                IsServiceOk = false;
            }

            return response;
        }
        /// <summary>
        /// 商品期货撤单
        /// </summary>
        /// <param name="entrustNumber">委托编号</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns>是否成功</returns>
        public bool CancelSPQHOrder(string entrustNumber, ref string errMsg)
        {
            if (!IsServiceOk)
            {
                errMsg = ServiceErrorMsg;
                return false;
            }

            TypesOrderStateType statetype;
            return doOrderClient.CancelMercantileFuturesOrder(entrustNumber, ref errMsg, out statetype);
        }
        /// <summary>
        /// 商品期货下单
        /// </summary>
        /// <param name="order">股指期货下单请求</param>
        /// <returns>委托返回信息</returns>
        public OrderResponse DoSPQHOrder(MercantileFuturesOrderRequest order)
        {
            if (!IsServiceOk)
                return new OrderResponse { OrderMessage = ServiceErrorMsg };

            order.ChannelID = Channelid;

            OrderResponse response = null;
            try
            {
                response = doOrderClient.DoMercantileFuturesOrder(order);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                response = new OrderResponse();
                response.IsSuccess = false;
                response.OrderMessage = ServiceErrorMsg;
                IsServiceOk = false;
            }

            return response;
        }
        #endregion

        #region 查询

        #region 属性
        /// <summary>
        /// 股指期货资金信息
        /// </summary>
        public List<QH_CapitalAccountTableInfo> GZQHCapital
        {
            get
            {
                string msg = "";

                var cap = QueryQHCapital(gzqhAccount, ref msg);
                List<DoCommonQuery.QH_CapitalAccountTableInfo> list = new List<DoCommonQuery.QH_CapitalAccountTableInfo>();
                if (cap != null)
                    list.Add(cap);

                return list;
            }
        }
        /// <summary>
        /// 商品期货资金信息
        /// </summary>
        public List<QH_CapitalAccountTableInfo> SPQHCapital
        {
            get
            {
                string msg = "";

                var cap = QueryQHCapital(spqhAccount, ref msg);
                List<DoCommonQuery.QH_CapitalAccountTableInfo> list = new List<DoCommonQuery.QH_CapitalAccountTableInfo>();
                if (cap != null)
                    list.Add(cap);

                return list;
            }
        }

        ///// <summary>
        ///// 期货历史成交
        ///// </summary>
        //public List<QH_HistoryTradeTableInfo> QHHistoryTrade
        //{
        //    get
        //    {
        //        string msg = "";
        //        int icount = 0;
        //        int currentPage = 0;
        //        int pageSize = 10;
        //        string strMessage = "";
        //        // return QueryQHHistoryTrade(gzqhAccount, ref msg);
        //        return QueryQHHistoryTrade(out icount, currentPage, pageSize, gzqhAccount, ref strMessage);
        //    }
        //}

        /// <summary>
        /// 股指期货持仓
        /// </summary>
        public List<DoCommonQuery.QH_HoldAccountTableInfo> GZQHHold
        {
            get
            {
                string msg = "";

                var list = QueryQHHold(gzqhAccount, GZQHaccountType, ref msg);

                List<DoCommonQuery.QH_HoldAccountTableInfo> holds = new List<DoCommonQuery.QH_HoldAccountTableInfo>();

                if (list != null)
                {
                    foreach (var entity in list)
                    {
                        holds.Add(entity.HoldFindResult);
                    }
                }

                return holds;
            }
        }
        /// <summary>
        /// 商品期货持仓
        /// </summary>
        public List<DoCommonQuery.QH_HoldAccountTableInfo> SPQHHold
        {
            get
            {
                string msg = "";

                var list = QueryQHHold(spqhAccount, SPQHaccountType, ref msg);

                List<DoCommonQuery.QH_HoldAccountTableInfo> holds = new List<DoCommonQuery.QH_HoldAccountTableInfo>();

                if (list != null)
                {
                    foreach (var entity in list)
                    {
                        holds.Add(entity.HoldFindResult);
                    }
                }

                return holds;
            }
        }
        /// <summary>
        /// 股指期货当日委托
        /// </summary>
        public List<DoCommonQuery.QH_TodayEntrustTableInfo> GZQHTodayEntrust
        {
            get
            {
                string msg = "";

                return QueryQHTodayEntrust(gzqhAccount, GZQHaccountType, ref msg);
            }
        }
        /// <summary>
        /// 商品期货当日委托
        /// </summary>
        public List<DoCommonQuery.QH_TodayEntrustTableInfo> SPQHTodayEntrust
        {
            get
            {
                string msg = "";

                return QueryQHTodayEntrust(gzqhAccount, SPQHaccountType, ref msg);
            }
        }
        /// <summary>
        /// 股指期货当日成交
        /// </summary>
        public List<DoCommonQuery.QH_TodayTradeTableInfo> GZQHTodayTrade
        {
            get
            {
                string msg = "";

                return QueryQHTodayTrade(gzqhAccount, GZQHaccountType, ref msg);
            }
        }
        /// <summary>
        /// 商品期货当日成交
        /// </summary>
        public List<DoCommonQuery.QH_TodayTradeTableInfo> SPQHTodayTrade
        {
            get
            {
                string msg = "";

                return QueryQHTodayTrade(spqhAccount, SPQHaccountType, ref msg);
            }
        }
        #endregion

        #region 方法

        /// <summary>
        /// 根据期货资金账号查询期货资金账号明细
        /// </summary>
        /// <param name="capitalAccount">期货资金账号</param>
        /// <param name="msg">异常信息</param>
        /// <returns>期货资金账户实体</returns>
        public QH_CapitalAccountTableInfo QueryQHCapital(string capitalAccount, ref string msg)
        {
            if (!IsServiceOk)
                return null;

            var capital = traderQueryClient.QueryQH_CapitalAccountTableByAccount(out msg, capitalAccount, DoCommonQuery.QueryTypeQueryCurrencyType.RMB);
            if (capital != null && capital.Count > 0)
            {
                return capital[0];
            }
            return null;
        }
        /// <summary>
        /// 期货持仓查询
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="accountType">账号类型（注：这是账户类型不是类别，这里是有九种类型，而类别是五种）</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>股指期货持仓实体</returns>
        public List<FuturesHoldFindResultEntity> QueryQHHold(string capitalAccount, int accountType, ref string strMessage)
        {
            if (!IsServiceOk)
            {
                return null;
            }
            int icount;
            var shcfe = new FuturesHoldConditionFindEntity();
            var holds = traderQueryClient.PagingQueryFuturesHold(out icount, ServerConfig.TraderID, accountType, ServerConfig.PassWord, shcfe, 1, int.MaxValue, ref strMessage);
            return holds;
        }
        /// <summary>
        /// 股指期货当日委托查询
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="accountType">账号类型（注：这是账户类型不是类别，这里是有九种类型，而类别是五种）</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>股指期货当日委托实体</returns>
        public List<DoCommonQuery.QH_TodayEntrustTableInfo> QueryQHTodayEntrust(string capitalAccount, int accountType, ref string strMessage)
        {
            if (!IsServiceOk)
                return new List<DoCommonQuery.QH_TodayEntrustTableInfo>();

            int icount;
            var secfe = new DoCommonQuery.FuturesEntrustConditionFindEntity();
            var pageInfo = new PagingInfo();
            pageInfo.CurrentPage = 1;
            pageInfo.PageLength = int.MaxValue;
            var tets = traderQueryClient.PagingQueryQH_TodayEntrustInfoByFilterAndUserIDPwd(out icount, out strMessage, ServerConfig.TraderID, ServerConfig.PassWord, accountType, secfe, pageInfo);
            return tets;
        }

        /// <summary>
        /// 股指期货当日委托查询
        /// </summary>
        /// <param name="accountType">账号类型（注：这是账户类型不是类别，这里是有九种类型，而类别是五种）</param>
        /// <param name="strMessage">异常信息</param>
        /// <param name="errorMsg"></param>
        /// <param name="filter"></param>
        /// <param name="pageInfo"></param>
        /// <param name="pwd">密码为空时后台不验证</param>
        /// <param name="total"></param>
        /// <param name="userID"></param>
        /// <returns>股指期货当日委托实体</returns>
        public List<DoCommonQuery.QH_TodayEntrustTableInfo> QueryQHTodayEntrust(out int total, out string errorMsg, string userID, string pwd, int accountType, FuturesEntrustConditionFindEntity filter, PagingInfo pageInfo)
        {
            total = 0;
            errorMsg = "";
            if (!IsServiceOk)
            {
                return new List<DoCommonQuery.QH_TodayEntrustTableInfo>();
            }

            var tets = traderQueryClient.PagingQueryQH_TodayEntrustInfoByFilterAndUserIDPwd(out total, out errorMsg, userID, pwd, accountType, filter, pageInfo);
            return tets;
        }
        /// <summary>
        /// 期货当日成交查询
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="accountType">账号类型（注：这是账户类型不是类别，这里是有九种类型，而类别是五种）</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>股指期货当日成交实体</returns>
        public List<DoCommonQuery.QH_TodayTradeTableInfo> QueryQHTodayTrade(string capitalAccount, int accountType, ref string strMessage)
        {
            if (!IsServiceOk)
            {
                return new List<DoCommonQuery.QH_TodayTradeTableInfo>();
            }
            int icount;
            var secfe = new DoCommonQuery.FuturesTradeConditionFindEntity();
            var pageInfo = new PagingInfo();
            pageInfo.CurrentPage = 1;
            pageInfo.PageLength = int.MaxValue;
            var trades = traderQueryClient.PagingQueryQH_TodayTradeByFilterAndUserIDPwd(out icount, out strMessage, ServerConfig.TraderID, ServerConfig.PassWord, accountType, secfe, pageInfo);
            return trades;
            // return QueryQHTodayTrade(out icount, capitalAccount, "", 1, int.MaxValue, out strMessage);
        }


        /// <summary>
        /// 股指期货综合查询
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="accountType"></param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>数据汇总实体</returns>
        public List<HKMarketValue> QueryMarketValueQHHold(string code, int accountType, ref string strMessage)
        {
            List<HKMarketValue> list = new List<HKMarketValue>();

            if (!IsServiceOk)
                return null;

            int icount;
            var shcfe = new DoCommonQuery.FuturesHoldConditionFindEntity();
            if (!string.IsNullOrEmpty(code))
            {
                shcfe.ContractCode = code;
            }
            var holds = traderQueryClient.PagingQueryFuturesHold(out icount, ServerConfig.TraderID, accountType, ServerConfig.PassWord, shcfe, 1, int.MaxValue, ref strMessage);
            if (holds == null)
            {
                return list;
            }
            foreach (var item in holds)
            {
                HKMarketValue hkmare = new HKMarketValue();
                hkmare.BelongMarket = item.BelongMarket;
                hkmare.BreakevenPrice = item.HoldFindResult.BreakevenPrice;
                hkmare.Code = item.HoldFindResult.Contract;
                hkmare.CostPrice = item.HoldFindResult.CostPrice;
                hkmare.CurrencyName = item.CurrencyName;
                hkmare.ErroNumber = item.ErroNumber;
                hkmare.ErroReason = item.ErroReason;
                hkmare.FloatProfitLoss = item.FloatProfitLoss;
                hkmare.HKName = item.ContractName;
                hkmare.HoldAveragePrice = item.HoldFindResult.HoldAveragePrice;
                hkmare.HoldSumAmount = item.HoldSumAmount;
                // 买方向的盯市盈亏=买方向的盯市盈亏=[持仓总量={0}*(当前价={1}-持仓均价={2})*交易单位倍数={3}]
                //sfre.MarketProfitLoss = computeTotal * (sfre.RealtimePrice - _QhAccountHold.HoldAveragePrice);
                //decimal holdSum = (decimal)item.FloatProfitLoss / (item.RealtimePrice - item.HoldFindResult.HoldAveragePrice);
                //if (holdSum == 0)
                //{
                if (item.UnitMultiple.HasValue)
                {
                    hkmare.MarketValue = item.HoldSumAmount * item.RealtimePrice * item.UnitMultiple.Value;//因后台期货持仓查询返回没有计算市值，所以这里按后台公式自行计算，这里默认300
                }
                else
                {
                    hkmare.ErroReason = "未获取到交易单位倍数，使用测试工具默认值300。";
                    hkmare.MarketValue = item.HoldSumAmount * item.RealtimePrice * 300;
                }
                //这里应该乘上数值倍数
                //}
                //else
                //{
                //  hkmare.MarketValue = holdSum * item.RealtimePrice;//因后台期货持仓查询返回没有计算市值，所以这里按后台公式自行计算
                //}
                hkmare.RealtimePrice = item.RealtimePrice;
                hkmare.MarketProfitLoss = item.MarketProfitLoss;
                hkmare.FloatProfitLoss = item.FloatProfitLoss;

                hkmare.TraderId = item.TraderId;
                hkmare.VarietyCategories = item.VarietyCategories;
                list.Add(hkmare);
            }
            return list;
        }

        /// <summary>
        /// 股指期货资金查询
        /// </summary>
        /// <param name="type">币种</param>
        /// <param name="accountType">账号类型（注：这是账户类型不是类别，这里是有九种类型，而类别是五种）</param>
        /// <param name="msg">异常信息</param>
        /// <returns>股指期货资金实体</returns>
        public DoCommonQuery.FuturesCapitalEntity QueryQHTotalCapital(Types.CurrencyType type, int accountType, ref string msg)
        {
            if (!IsServiceOk)
            {
                return null;
            }
            return traderQueryClient.PagingQueryFuturesCapital(ServerConfig.TraderID, accountType, type, ServerConfig.PassWord, ref msg);
        }

        /// <summary>
        /// 期货历史成交查询
        /// </summary>
        /// <param name="icount">记录数</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="accountType">账号类型（注：这是账户类型不是类别，这里是有九种类型，而类别是五种）</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>历史成交实体列表</returns>
        public List<QH_HistoryTradeTableInfo> QueryQHHistoryTrade(out int icount, int currentPage, int pageSize, string capitalAccount, int accountType, DateTime? sDate, DateTime? eDate, ref string strMessage)
        {
            icount = 0;
            if (!IsServiceOk)
            {
                return new List<QH_HistoryTradeTableInfo>();
            }
            var secfe = new DoCommonQuery.FuturesTradeConditionFindEntity();
            secfe.EntrustNumber = CurrentQueryValue.QueryQHEntrustNO;
            if (sDate.HasValue)
            {
                secfe.StartTime = sDate.Value;
            }
            if (eDate.HasValue)
            {
                secfe.EndTime = eDate.Value;
            }

            PagingInfo pinfo = new PagingInfo();
            pinfo.CurrentPage = currentPage;
            pinfo.PageLength = pageSize;
            pinfo.IsCount = true;
            var tets = traderQueryClient.PagingQueryQH_HistoryTradeByFilterAndUserIDPwd(out icount, out strMessage, ServerConfig.TraderID, ServerConfig.PassWord, accountType, secfe, pinfo);
            return tets;
        }


        /// <summary>
        /// 期货当日成交查询
        /// </summary>
        /// <param name="icount">记录数</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="accountType">账号类型（注：这是账户类型不是类别，这里是有九种类型，而类别是五种）</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>历史成交实体列表</returns>
        public List<QH_TodayTradeTableInfo> QueryGZQHTodayTrade(out int icount, int currentPage, int pageSize, string capitalAccount, int accountType, ref string strMessage)
        {
            icount = 0;
            if (!IsServiceOk)
            {
                return new List<QH_TodayTradeTableInfo>();
            }
            var secfe = new DoCommonQuery.FuturesTradeConditionFindEntity();
            secfe.EntrustNumber = CurrentQueryValue.QueryQHEntrustNO;

            PagingInfo pinfo = new PagingInfo();
            pinfo.CurrentPage = currentPage;
            pinfo.PageLength = pageSize;
            pinfo.IsCount = true;
            var trades = traderQueryClient.PagingQueryQH_TodayTradeByFilterAndUserIDPwd(out icount, out strMessage, ServerConfig.TraderID, ServerConfig.PassWord, accountType, secfe, pinfo);
            return trades;
        }


        /// <summary>
        /// 商品期货当日成交查询
        /// </summary>
        /// <param name="icount">记录数</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="accountType">账号类型（注：这是账户类型不是类别，这里是有九种类型，而类别是五种）</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>历史成交实体列表</returns>
        public List<QH_TodayTradeTableInfo> QueryQHTodayTrade(out int icount, int currentPage, int pageSize, string capitalAccount, int accountType, ref string strMessage)
        {
            icount = 0;
            if (!IsServiceOk)
            {
                return new List<DoCommonQuery.QH_TodayTradeTableInfo>();
            }
            var secfe = new DoCommonQuery.FuturesTradeConditionFindEntity();
            secfe.EntrustNumber = CurrentQueryValue.QueryQHEntrustNO;

            var pageInfo = new PagingInfo();
            pageInfo.CurrentPage = currentPage;
            pageInfo.PageLength = pageSize;
            pageInfo.IsCount = true;
            var trades = traderQueryClient.PagingQueryQH_TodayTradeByFilterAndUserIDPwd(out icount, out strMessage, ServerConfig.TraderID, ServerConfig.PassWord, accountType, secfe, pageInfo);
            return trades;
        }

        /// <summary>
        /// 期货历史委托查询
        /// </summary>
        /// <param name="icount">记录数</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="accountType">账号类型（注：这是账户类型不是类别，这里是有九种类型，而类别是五种）</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>历史委托实体列表</returns>
        public List<QH_HistoryEntrustTableInfo> QueryQHHistoryEntrust(out int icount, int currentPage, int pageSize, string capitalAccount, int accountType, DateTime? sDate, DateTime? eDate, ref string strMessage)
        {
            icount = 0;
            if (!IsServiceOk)
            {
                return new List<QH_HistoryEntrustTableInfo>();
            }
            var secfe = new DoCommonQuery.FuturesEntrustConditionFindEntity();
            secfe.EntrustNumber = CurrentQueryValue.QueryQHEntrustNO;
            if (sDate.HasValue)
            {
                secfe.StartTime = sDate.Value;
            }
            if (eDate.HasValue)
            {
                secfe.EndTime = eDate.Value;
            }

            PagingInfo pinfo = new PagingInfo();
            pinfo.CurrentPage = currentPage;
            pinfo.PageLength = pageSize;
            pinfo.IsCount = true;
            var tets = traderQueryClient.PagingQueryQH_HistoryEntrustInfoByFilterAndUserIDPwd(out icount, out strMessage, ServerConfig.TraderID, ServerConfig.PassWord, accountType, secfe, pinfo);
            return tets;
        }
        /// <summary>
        /// 期货历史委托查询
        /// </summary>
        /// <param name="icount">记录数</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="accountType">账号类型（注：这是账户类型不是类别，这里是有九种类型，而类别是五种）</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>历史委托实体列表</returns>
        public List<QH_HistoryEntrustTableInfo> QueryQHHistoryEntrust(out int icount, out string strMessage, string traderID, string pwd, int accountType, FuturesEntrustConditionFindEntity secfe, PagingInfo pinfo)
        {
            icount = 0;
            strMessage = "";
            if (!IsServiceOk)
            {
                return new List<QH_HistoryEntrustTableInfo>();
            }

            var tets = traderQueryClient.PagingQueryQH_HistoryEntrustInfoByFilterAndUserIDPwd(out icount, out strMessage, ServerConfig.TraderID, ServerConfig.PassWord, accountType, secfe, pinfo);
            return tets;
        }

        /// <summary>
        /// 期货当日委托查询
        /// </summary>
        /// <param name="icount">记录数</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="accountType">账号类型（注：这是账户类型不是类别，这里是有九种类型，而类别是五种）</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>历史委托实体列表</returns>
        public List<QH_TodayEntrustTableInfo> QueryQHTodayEntrust(out int icount, int currentPage, int pageSize, string capitalAccount, int accountType, ref string strMessage)
        {
            icount = 0;
            if (!IsServiceOk)
            {
                return new List<QH_TodayEntrustTableInfo>();
            }
            var secfe = new DoCommonQuery.FuturesEntrustConditionFindEntity();
            secfe.EntrustNumber = CurrentQueryValue.QueryQHEntrustNO;


            PagingInfo pageInfo = new PagingInfo();
            pageInfo.CurrentPage = currentPage;
            pageInfo.PageLength = pageSize;
            pageInfo.IsCount = true;
            var tets = traderQueryClient.PagingQueryQH_TodayEntrustInfoByFilterAndUserIDPwd(out icount, out strMessage, ServerConfig.TraderID, ServerConfig.PassWord, accountType, secfe, pageInfo);
            return tets;
        }


        /// <summary>
        /// 商品期货当日委托查询
        /// </summary>
        /// <param name="icount">记录数</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="accountType">账号类型（注：这是账户类型不是类别，这里是有九种类型，而类别是五种）</param>
        /// <param name="strMessage">异常信息</param>
        /// <returns>历史委托实体列表</returns>
        public List<DoCommonQuery.QH_TodayEntrustTableInfo> QuerySPQHHistoryEntrust(out int icount, int currentPage, int pageSize, string capitalAccount, int accountType, ref string strMessage)
        {
            icount = 0;
            if (!IsServiceOk)
            {
                return new List<DoCommonQuery.QH_TodayEntrustTableInfo>();
            }
            var secfe = new DoCommonQuery.FuturesEntrustConditionFindEntity();

            var pageInfo = new PagingInfo();
            pageInfo.CurrentPage = currentPage;
            pageInfo.PageLength = pageSize;
            pageInfo.IsCount = true;
            var tets = traderQueryClient.PagingQueryQH_TodayEntrustInfoByFilterAndUserIDPwd(out icount, out strMessage, ServerConfig.TraderID, ServerConfig.PassWord, accountType, secfe, pageInfo);
            return tets;
        }

        /// <summary>
        /// 查询商品期货相应盘后的资金清算流水
        /// </summary>
        /// <param name="type">货币类型</param>
        /// <param name="pwd">交易员密码</param>
        /// <param name="msg">异常信息</param>
        /// <returns>期货资金流水表实体</returns>
        public List<QH_TradeCapitalFlowDetailInfo> QuerySPQHCapitalFlowDetail(DoCommonQuery.QueryTypeQueryCurrencyType type, string pwd, out string msg)
        {
            msg = "";
            if (!IsServiceOk)
                return null;
            int total = 0;
            DoCommonQuery.PagingInfo pageInfo = new DoCommonQuery.PagingInfo();
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = false;
            pageInfo.PageLength = int.MaxValue;

            return traderQueryClient.PagingQuerySPQH_TradeCapitalFlowDetailByAccount(out total, out msg, traderId, pwd, null, null, type, pageInfo);
        }
        /// <summary>
        /// 查询股指期货相应盘后的资金清算流水
        /// </summary>
        /// <param name="type">货币类型</param>
        /// <param name="pwd">交易员密码</param>
        /// <param name="msg">异常信息</param>
        /// <returns>期货资金流水表实体</returns>
        public List<QH_TradeCapitalFlowDetailInfo> QueryQHCapitalFlowDetail(DoCommonQuery.QueryTypeQueryCurrencyType type, string pwd, out string msg)
        {
            msg = "";
            if (!IsServiceOk)
                return null;
            int total = 0;
            DoCommonQuery.PagingInfo pageInfo = new DoCommonQuery.PagingInfo();
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = false;
            pageInfo.PageLength = int.MaxValue;

            return traderQueryClient.PagingQueryQH_TradeCapitalFlowDetailByAccount(out total, out msg, traderId, pwd, null, null, type, pageInfo);
        }
        /// <summary>
        /// 资金流水查询
        /// </summary>
        /// <param name="msg">错误消息</param>
        /// <param name="CapitalFlowFilter">资金信息</param>
        /// <param name="account">帐号类型</param>
        /// <returns></returns>
        public List<UA_CapitalFlowTableInfo> QueryCapitalFlow(out string msg, QueryUA_CapitalFlowFilter CapitalFlowFilter, int account)
        {
            int total = 0;
            msg = "";
            DoCommonQuery.PagingInfo pageInfo = new DoCommonQuery.PagingInfo();
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = false;
            pageInfo.PageLength = int.MaxValue;
            string UserId = ServerConfig.TraderID;
            string pwd = ServerConfig.PassWord;

            return traderQueryClient.PaginQueryUA_CapitalFlowTableByFilter(out total, out msg, UserId, pwd, account, CapitalFlowFilter, pageInfo);
        }
        #endregion

        #endregion

        #endregion


        /// <summary>
        /// 根据品种类别获取相关所有柜台缓存的当前所有代码
        /// </summary>
        /// <param name="classID">品种类型 现货-1, 商品期货--2, 股指期货--3, 港股--4</param>
        /// <returns>返回相关的所有品种类型代码</returns>
        public List<string> GetAllCode(int classID)
        {
            List<string> list = new List<string>();
            try
            {

                //这里直接过滤掉过期代码，即直接传True，因为这里柜台内部处理过期代码也只是对期货代码有处理，所以这里直接true，如有别的必要再另外公开
                //此参数
                list = accountClient.GetAllCM_CommodityByBreedClassTypeID(classID, true);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
            return list;
        }



    }

    #region 记录当前查询委托编号
    /// <summary>
    /// Tilte;当前查询委托编号    
    /// Create BY：董鹏
    /// Create date:2009-12-22
    /// </summary>
    public class CurrentQueryValue
    {
        /// <summary>
        /// 港股查询今日委托的委托编号
        /// </summary>
        public static string QueryHKEnNO = "";
        /// <summary>
        /// 港股查今日成交的委托编号
        /// </summary>
        public static string QueryHKTradeNO = "";
        /// <summary>
        /// 港股查询历史委托的委托编号
        /// </summary>
        public static string QueryHKHistoryEnNO = "";
        /// <summary>
        /// 港股查历史成交的委托编号
        /// </summary>
        public static string QueryHKHistoryTradeNO = "";

        /// <summary>
        /// 现货查询今日委托的委托编号
        /// </summary>
        public static string QueryXHEntrustNO = "";
        /// <summary>
        /// 现货查今日成交的委托编号
        /// </summary>
        public static string QueryXHTradeNO = "";

        /// <summary>
        /// 期货查询委托编号
        /// </summary>
        public static string QueryQHEntrustNO = "";

    }

    #endregion
}