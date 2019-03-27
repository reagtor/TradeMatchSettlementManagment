using System.Net.Mime;
using GTA.VTS.Common.CommonObject;
using CommonRealtimeMarket;
using CommonRealtimeMarket.factory;
using GTA.VTS.Common.CommonUtility;
using GTAMarketSocket;
using ReckoningCounter.BLL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReckoningCounter.Entity;
//using RealTime.Server.Handler;
using RealTime.Common.CommonClass;

namespace ReckoningCounterBizTest
{
    
    
    /// <summary>
    ///This is a test class for OrderAccepterTest and is intended
    ///to contain all OrderAccepterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OrderAccepterTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for OrderAccepter Constructor
        ///</summary>
        [TestMethod()]
        public void OrderAccepterConstructorTest()
        {
            OrderAccepter target = new OrderAccepter();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for CancelSPQHOrder
        ///</summary>
        [TestMethod()]
        public void CancelMercantileFuturesOrderTest()
        {
            OrderAccepter target = new OrderAccepter(); // TODO: Initialize to an appropriate value
            string OrderId = string.Empty; // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            string messageExpected = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            int errCode = 0;
            actual = target.CancelMercantileFuturesOrder(OrderId, ref message, out errCode);
            Assert.AreEqual(messageExpected, message);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CancelGZQHOrder
        ///</summary>
        [TestMethod()]
        public void CancelStockIndexFuturesOrderTest()
        {
            OrderAccepter target = new OrderAccepter(); // TODO: Initialize to an appropriate value
            string OrderId = string.Empty; // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            string messageExpected = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            int errCode = 0;
            actual = target.CancelStockIndexFuturesOrder(OrderId, ref message,out errCode);
            Assert.AreEqual(messageExpected, message);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CancelStockOrder
        ///</summary>
        [TestMethod()]
        public void CancelStockOrderTest()
        {
            OrderAccepter target = new OrderAccepter(); // TODO: Initialize to an appropriate value
            string OrderId = string.Empty; // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            string messageExpected = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            //actual = target.CancelStockOrder(OrderId, ref message);
            //Assert.AreEqual(messageExpected, message);
            //Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DoMercantileFuturesOrder
        ///</summary>
        [TestMethod()]
        public void DoMercantileFuturesOrderTest()
        {
            OrderAccepter target = new OrderAccepter(); // TODO: Initialize to an appropriate value
            MercantileFuturesOrderRequest futuresorder = null; // TODO: Initialize to an appropriate value
            OrderResponse expected = null; // TODO: Initialize to an appropriate value
            OrderResponse actual;
            actual = target.DoMercantileFuturesOrder(futuresorder);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DoStockIndexFuturesOrder
        ///</summary>
        [TestMethod()]
        public void DoStockIndexFuturesOrderTest()
        {
            OrderAccepter target = new OrderAccepter(); // TODO: Initialize to an appropriate value
            StockIndexFuturesOrderRequest futuresorder = null; // TODO: Initialize to an appropriate value
            OrderResponse expected = null; // TODO: Initialize to an appropriate value
            OrderResponse actual;
            actual = target.DoStockIndexFuturesOrder(futuresorder);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DoStockOrder
        ///</summary>
        [TestMethod()]
        public void DoStockOrderTest()
        {
            InitRealtimeMarketComponent();
            OrderAccepter target = new OrderAccepter(); // TODO: Initialize to an appropriate value
            StockOrderRequest stockorder = BuildStockOrderRequest(); // TODO: Initialize to an appropriate value
            OrderResponse expected = null; // TODO: Initialize to an appropriate value
            OrderResponse actual;
            actual = target.DoStockOrder(stockorder);
            Assert.IsNotNull(actual.OrderId);
        }

        StockOrderRequest BuildStockOrderRequest()
        {
            var result = new StockOrderRequest();
            //通道回送通道ID
            result.ChannelID = "A";
            //买卖类型
            result.BuySell = GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying;
            //代码
            result.Code = "000001";
            //资金帐户
            result.FundAccountId ="XH0001";
            //交易员密码
            result.TraderPassword = "000";
            //委托量
            result.OrderAmount = 100;
            //委托价
            result.OrderPrice = 44.58F;
            //交易单位股
            result.OrderUnitType = Types.UnitType.Thigh;
            //限价委托/市价委托
            result.OrderWay = ReckoningCounter.Entity.Contants.Types.OrderPriceType.OPTLimited;
            //投组关系
            result.PortfoliosId = "1";

            return result;
        }


        #region == 行情初始化 ==


        /// <summary>
        /// 瑞尔格特下单服务
        /// </summary>
        private GTASocket SocketService = null;
        /// <summary>
        /// 初始化行情组件
        /// </summary>
        void InitRealtimeMarketComponent()
        {
            //登陆处理
            GTASocketSingletonForRealTime.StartLogin("rtuser", "11", ShowConnectMessage);
            //数据接受注册事件
            this.FEvent += Event;
            SocketService = GTASocketSingletonForRealTime.SocketService;
            GTASocketSingletonForRealTime.SocketStateChanged += GTASocketSingleton_SocketStateChanged;
          //  SocketService.AddEventDelegate(this.FEvent);
            GTASocketSingletonForRealTime.SocketStatusHandle(SocketStatusChange);
            IRealtimeMarketService rms = RealtimeMarketServiceFactory.GetService();

        }

        /// <summary>
        /// Socket状态改变触发事件
        /// </summary>
        /// <param name="_e"></param>
        private delegate void DelegateSocketStatusChange(SocketServiceStatusEventArg _e);

        /// <summary>
        /// 状态改变触发事件
        /// </summary>
        /// <param name="e"></param>
        private void SocketStatusChange(SocketServiceStatusEventArg e)
        {
           


        }

        /// <summary>
        /// 状态处理（暂不使用)
        /// </summary>
        /// <param name="state"></param>
        void GTASocketSingleton_SocketStateChanged(EnumSocketResetState state)
        {

        }

        /// <summary>
        /// 显示登陆信息
        /// </summary>
        /// <param name="e"></param>
        private void ShowConnectMessage(SocketServiceStatusEventArg e)
        {
           
        }


        /// <summary>
        /// 消息打印事件
        /// </summary>
        public GTAMarketSocket.OnEventDelegate FEvent;


        /// <summary>
        /// 清除消息
        /// </summary>
        public void ClearEchoInfo()
        {
         
        }

        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="eventMessage"></param>
        public void Event(string eventMessage)
        {
           // MessageDisplayHelper.Event(eventMessage, lstMessages);
        }

        #endregion

        ///2009-04-09新加的测试方法
        /// <summary>
        ///DoStockIndexFuturesOrder 的测试
        ///</summary>
        [TestMethod()]
        public void DoStockIndexFuturesOrderTest1()
        {
            OrderAccepter target = new OrderAccepter(); // TODO: 初始化为适当的值
            StockIndexFuturesOrderRequest futuresorder = null; // TODO: 初始化为适当的值
            futuresorder = new StockIndexFuturesOrderRequest();
            //赋值
            futuresorder.BuySell = Types.TransactionDirection.Buying;
            futuresorder.Code = "IF0904";
            futuresorder.ChannelID = "0";
            futuresorder.FundAccountId = "010000002406";
            futuresorder.OpenCloseType = ReckoningCounter.Entity.Contants.Types.FutureOpenCloseType.OpenPosition;
            futuresorder.OrderAmount = 1;
            futuresorder.OrderPrice = 2505;
            futuresorder.OrderUnitType = GTA.VTS.Common.CommonObject.Types.UnitType.Hand;
            futuresorder.OrderWay = ReckoningCounter.Entity.Contants.Types.OrderPriceType.OPTLimited;
            futuresorder.TraderId = "24";
            futuresorder.TraderPassword = "888888";
            futuresorder.PortfoliosId = "0";

            OrderResponse expected = new OrderResponse();  //null; // TODO: 初始化为适当的值
            expected.OrderId = "123456789";//仅测试
            OrderResponse actual;
            actual = target.DoStockIndexFuturesOrder(futuresorder);
            Assert.AreNotEqual(expected, actual);
            // Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
