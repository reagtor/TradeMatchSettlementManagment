using ReckoningCounter.BLL.AccountManagementAndFind.InterfaceBLL.AccountManagementAndFindService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GTA.VTS.Common.CommonObject;
using ReckoningCounter.Entity.Model;
using ReckoningCounter.Model;
using System.Collections.Generic;
using System;
using ReckoningCounter.Entity.AccountManagementAndFindEntity;
using ReckoningCounter.Entity;
using ReckoningCounter.Entity.Model.QueryFilter;

namespace ReckoningCounterBizTest
{


    /// <summary>
    ///这是 TraderFindServiceTest 的测试类，旨在
    ///包含所有 TraderFindServiceTest 单元测试
    ///</summary>
    [TestClass()]
    public class TraderFindServiceTest
    {


        private TestContext testContextInstance; 

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
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

        #region 附加测试属性
        // 
        //编写测试时，还可使用以下属性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///PaginQueryUA_CapitalFlowTableByFilter 的测试
        ///</summary>
        [TestMethod()]
        public void PaginQueryUA_CapitalFlowTableByFilterTest()
        {
            TraderFindService target = new TraderFindService(); // TODO: 初始化为适当的值
            string userId = "4"; // TODO: 初始化为适当的值
            string pwd = string.Empty; // TODO: 初始化为适当的值
            Types.AccountAttributionType accountType = Types.AccountAttributionType.BankAccount; // TODO: 初始化为适当的值
            QueryUA_CapitalFlowFilter filter = new QueryUA_CapitalFlowFilter(); // TODO: 初始化为适当的值
            filter.StartTime = new DateTime(2009, 02, 3);
            filter.EndTime = DateTime.Now;

            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = true;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;
            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<UA_CapitalFlowTableInfo> expected = null; // TODO: 初始化为适当的值
            List<UA_CapitalFlowTableInfo> actual;
            //actual = target.PaginQueryUA_CapitalFlowTableByFilter(userId, pwd, accountType, filter, pageInfo, out total, out errorMsg);
            //Assert.AreEqual(totalExpected, total);
            //Assert.AreEqual(errorMsgExpected, errorMsg);
            //Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///BankCapitalAccountTransferFlowFind2 的测试
        ///</summary>
        [TestMethod()]
        public void BankCapitalAccountTransferFlowFind2Test()
        {
            TraderFindService target = new TraderFindService(); // TODO: 初始化为适当的值
            string userId = string.Empty; // TODO: 初始化为适当的值
            int AccountType = 0; // TODO: 初始化为适当的值
            string userPassword = string.Empty; // TODO: 初始化为适当的值
            DateTime startTime = new DateTime(); // TODO: 初始化为适当的值
            DateTime endTime = new DateTime(); // TODO: 初始化为适当的值
            int start = 0; // TODO: 初始化为适当的值
            int pageLength = 0; // TODO: 初始化为适当的值
            int count = 0; // TODO: 初始化为适当的值
            int countExpected = 0; // TODO: 初始化为适当的值
            string strErrorMessage = string.Empty; // TODO: 初始化为适当的值
            string strErrorMessageExpected = string.Empty; // TODO: 初始化为适当的值
            List<UA_CapitalFlowTableInfo> expected = null; // TODO: 初始化为适当的值
            List<UA_CapitalFlowTableInfo> actual;
            //actual = target.BankCapitalAccountTransferFlowFind2(userId, AccountType, userPassword, startTime, endTime, start, pageLength, out count, out strErrorMessage);
            Assert.AreEqual(countExpected, count);
            Assert.AreEqual(strErrorMessageExpected, strErrorMessage);
            //Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///BankCapitalFind2 的测试
        ///</summary>
        [TestMethod()]
        public void BankCapitalFind2Test()
        {
            TraderFindService target = new TraderFindService(); // TODO: 初始化为适当的值
            string userId = string.Empty; // TODO: 初始化为适当的值
            string outMessage = string.Empty; // TODO: 初始化为适当的值
            string outMessageExpected = string.Empty; // TODO: 初始化为适当的值
            List<UA_BankAccountTableInfo> expected = null; // TODO: 初始化为适当的值
            List<UA_BankAccountTableInfo> actual;
            //actual = target.BankCapitalFind2(userId, out outMessage);
            Assert.AreEqual(outMessageExpected, outMessage);
           // Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///FuturesCapitalFind2 的测试
        ///</summary>
        [TestMethod()]
        public void FuturesCapitalFind2Test()
        {
            TraderFindService target = new TraderFindService(); // TODO: 初始化为适当的值
            string userId = string.Empty; // TODO: 初始化为适当的值
            int AccountType = 0; // TODO: 初始化为适当的值
            Types.CurrencyType currencyType = new Types.CurrencyType(); // TODO: 初始化为适当的值
            string userPassword = string.Empty; // TODO: 初始化为适当的值
            string strErrorMessage = string.Empty; // TODO: 初始化为适当的值
            string strErrorMessageExpected = string.Empty; // TODO: 初始化为适当的值
            FuturesCapitalEntity expected = null; // TODO: 初始化为适当的值
            FuturesCapitalEntity actual;
            //actual = target.FuturesCapitalFind2(userId, AccountType, currencyType, userPassword, ref strErrorMessage);
            Assert.AreEqual(strErrorMessageExpected, strErrorMessage);
            //Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryQH_TodayTradeByFilterAndUserIDPwd 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryQH_TodayTradeByFilterAndUserIDPwdTest()
        {
            TraderFindService target = new TraderFindService(); // TODO: 初始化为适当的值
            string userID = "4"; // TODO: 初始化为适当的值
            string pwd = "XIVqM2FELUw="; // TODO: 初始化为适当的值
            FuturesTradeConditionFindEntity filter = null; // TODO: 初始化为适当的值

            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = true;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;

            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<QH_TodayTradeTableInfo> expected = null; // TODO: 初始化为适当的值
            List<QH_TodayTradeTableInfo> actual;
            //actual = target.PagingQueryQH_TodayTradeByFilterAndUserIDPwd(userID, pwd, filter, pageInfo, out total, out errorMsg);
            //Assert.AreEqual(totalExpected, total);
            //Assert.AreEqual(errorMsgExpected, errorMsg);
            //Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryQH_TodayTradeByFilterAndUserIDPwd 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryQH_HistoryTradeByFilterAndUserIDPwdTest()
        {
            TraderFindService target = new TraderFindService(); // TODO: 初始化为适当的值
            string userID = "4"; // TODO: 初始化为适当的值
            string pwd = "XIVqM2FELUw="; // TODO: 初始化为适当的值
            FuturesTradeConditionFindEntity filter = new FuturesTradeConditionFindEntity(); // TODO: 初始化为适当的值
            filter.BuySellDirection = 1;
            filter.CurrencyTypeId = 2;
            filter.OpenCloseDirection = 2;

            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = true;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;

            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<QH_HistoryTradeTableInfo> expected = null; // TODO: 初始化为适当的值
            List<QH_HistoryTradeTableInfo> actual;
            //actual = target.PagingQueryQH_HistoryTradeByFilterAndUserIDPwd(userID, pwd, filter, pageInfo, out total, out errorMsg);
            //Assert.AreEqual(totalExpected, total);
            //Assert.AreEqual(errorMsgExpected, errorMsg);
            //Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryXH_TodayTradeByFilterAndUserIDPwd 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryXH_TodayTradeByFilterAndUserIDPwdTest()
        {
            TraderFindService target = new TraderFindService(); // TODO: 初始化为适当的值
            string userID = "4"; // TODO: 初始化为适当的值
            string pwd = "XIVqM2FELUw="; // TODO: 初始化为适当的值
            SpotTradeConditionFindEntity filter = null; // TODO: 初始化为适当的值

            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = true;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;

            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<XH_TodayTradeTableInfo> expected = null; // TODO: 初始化为适当的值
            List<XH_TodayTradeTableInfo> actual;
            //actual = target.PagingQueryXH_TodayTradeByFilterAndUserIDPwd(userID, pwd, filter, pageInfo, out total, out errorMsg);
            //Assert.AreEqual(totalExpected, total);
            //Assert.AreEqual(errorMsgExpected, errorMsg);
            //Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryXH_HistoryTradeByFilterAndUserIDPwd 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryXH_HistoryTradeByFilterAndUserIDPwdTest()
        {
            TraderFindService target = new TraderFindService(); // TODO: 初始化为适当的值
            string userID = "4"; // TODO: 初始化为适当的值
            string pwd = "XIVqM2FELUw="; // TODO: 初始化为适当的值
            SpotTradeConditionFindEntity filter = new SpotTradeConditionFindEntity(); // TODO: 初始化为适当的值
            filter.BelongToMarket = 32;
            filter.BuySellDirection = 1;
            filter.CurrencyType = 2;
            filter.StartTime = DateTime.Now.AddDays(-30);
            filter.EndTime = DateTime.Now;
            filter.TradeType = 2;


            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = true;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;

            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<XH_HistoryTradeTableInfo> expected = null; // TODO: 初始化为适当的值
            List<XH_HistoryTradeTableInfo> actual;
            //actual = target.PagingQueryXH_HistoryTradeByFilterAndUserIDPwd(userID, pwd, filter, pageInfo, out total, out errorMsg);
            //Assert.AreEqual(totalExpected, total);
            //Assert.AreEqual(errorMsgExpected, errorMsg);
            //Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryQH_TodayEntrustInfoByFilterAndUserIDPwd 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryQH_TodayEntrustInfoByFilterAndUserIDPwdTest()
        {
            TraderFindService target = new TraderFindService(); // TODO: 初始化为适当的值
            string userID = "4"; // TODO: 初始化为适当的值
            string pwd = "XIVqM2FELUw="; // TODO: 初始化为适当的值
            FuturesEntrustConditionFindEntity filter = new FuturesEntrustConditionFindEntity(); // TODO: 初始化为适当的值
            filter.BelongToMarket = 32;
            filter.BuySellDirection = 1;
            filter.CurrencyTypeId = 2;
            filter.StartTime = DateTime.Now.AddDays(-30);
            filter.EndTime = DateTime.Now;
            filter.CanBeWithdrawnLogo = 1;


            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = true;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;
            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<QH_TodayEntrustTableInfo> expected = null; // TODO: 初始化为适当的值
            List<QH_TodayEntrustTableInfo> actual;
            //actual = target.PagingQueryQH_TodayEntrustInfoByFilterAndUserIDPwd(userID, pwd, filter, pageInfo, out total, out errorMsg);
            //Assert.AreEqual(totalExpected, total);
            //Assert.AreEqual(errorMsgExpected, errorMsg);
            //Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryQH_HistoryEntrustInfoByFilterAndUserIDPwd 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryQH_HistoryEntrustInfoByFilterAndUserIDPwdTest()
        {
            TraderFindService target = new TraderFindService(); // TODO: 初始化为适当的值
            string userID = "4"; // TODO: 初始化为适当的值
            string pwd = "XIVqM2FELUw="; // TODO: 初始化为适当的值
            FuturesEntrustConditionFindEntity filter = new FuturesEntrustConditionFindEntity(); // TODO: 初始化为适当的值
            filter.BelongToMarket = 32;
            filter.BuySellDirection = 1;
            filter.CurrencyTypeId = 2;
            filter.StartTime = DateTime.Now.AddDays(-30);
            filter.EndTime = DateTime.Now;
            filter.CanBeWithdrawnLogo = 1;


            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = true;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;
            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<QH_HistoryEntrustTableInfo> expected = null; // TODO: 初始化为适当的值
            List<QH_HistoryEntrustTableInfo> actual;
            //actual = target.PagingQueryQH_HistoryEntrustInfoByFilterAndUserIDPwd(userID, pwd, filter, pageInfo, out total, out errorMsg);
            //Assert.AreEqual(totalExpected, total);
            //Assert.AreEqual(errorMsgExpected, errorMsg);
            //Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryXH_TodayEntrustInfoByFilterAndUserIDPwd 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryXH_TodayEntrustInfoByFilterAndUserIDPwdTest()
        {
            TraderFindService target = new TraderFindService(); // TODO: 初始化为适当的值
            string userID = "4"; // TODO: 初始化为适当的值
            string pwd = "XIVqM2FELUw="; // TODO: 初始化为适当的值
            SpotEntrustConditionFindEntity filter = new SpotEntrustConditionFindEntity(); // TODO: 初始化为适当的值
            filter.BelongToMarket = 32;
            filter.BuySellDirection = 1;
            filter.CurrencyType = 2;
            filter.StartTime = DateTime.Now.AddDays(-30);
            filter.EndTime = DateTime.Now;
            filter.CanBeWithdrawnLogo = 1;
            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = true;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;
            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<XH_TodayEntrustTableInfo> expected = null; // TODO: 初始化为适当的值
            List<XH_TodayEntrustTableInfo> actual;
            //actual = target.PagingQueryXH_TodayEntrustInfoByFilterAndUserIDPwd(userID, pwd, filter, pageInfo, out total, out errorMsg);
            //Assert.AreEqual(totalExpected, total);
            //Assert.AreEqual(errorMsgExpected, errorMsg);
            //Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryXH_HistoryEntrustInfoByFilterAndUserIDPwd 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryXH_HistoryEntrustInfoByFilterAndUserIDPwdTest()
        {
            TraderFindService target = new TraderFindService(); // TODO: 初始化为适当的值
            string userID = "4"; // TODO: 初始化为适当的值
            string pwd = "XIVqM2FELUw="; // TODO: 初始化为适当的值
            SpotEntrustConditionFindEntity filter = new SpotEntrustConditionFindEntity(); // TODO: 初始化为适当的值
            filter.BelongToMarket = 32;
            filter.BuySellDirection = 1;
            filter.CurrencyType = 2;
            filter.StartTime = DateTime.Now.AddDays(-30);
            filter.EndTime = DateTime.Now;
            filter.CanBeWithdrawnLogo = 1;
            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = true;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;
            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<XH_HistoryEntrustTableInfo> expected = null; // TODO: 初始化为适当的值
            List<XH_HistoryEntrustTableInfo> actual;
            //actual = target.PagingQueryXH_HistoryEntrustInfoByFilterAndUserIDPwd(userID, pwd, filter, pageInfo, out total, out errorMsg);
            //Assert.AreEqual(totalExpected, total);
            //Assert.AreEqual(errorMsgExpected, errorMsg);
            //Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///QueryQH_CapitalAccountFreezeTableByEntrustNo 的测试
        ///</summary>
        [TestMethod()]
        public void QueryQH_CapitalAccountFreezeTableByEntrustNoTest()
        {
            TraderFindService target = new TraderFindService(); // TODO: 初始化为适当的值
            string entrustNo = "0906120409427961"; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            QueryType.QueryFreezeType freezeType = QueryType.QueryFreezeType.ALL;
            List<QH_CapitalAccountFreezeTableInfo> expected = null; // TODO: 初始化为适当的值
            List<QH_CapitalAccountFreezeTableInfo> actual;
            //actual = target.QueryQH_CapitalAccountFreezeTableByEntrustNo(entrustNo, freezeType, out errorMsg);
            //Assert.AreEqual(errorMsgExpected, errorMsg);
            //Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryQH_CapitalAccountFreezeTableByUserID 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryQH_CapitalAccountFreezeTableByUserIDTest()
        {
            TraderFindService target = new TraderFindService(); // TODO: 初始化为适当的值
            string userID = "4"; // TODO: 初始化为适当的值
            Nullable<DateTime> startTime = new Nullable<DateTime>(); // TODO: 初始化为适当的值
            Nullable<DateTime> endTime = new Nullable<DateTime>(); // TODO: 初始化为适当的值
            QueryType.QueryCurrencyType type = QueryType.QueryCurrencyType.ALL; // TODO: 初始化为适当的值

            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = false;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;

            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<QH_CapitalAccountFreezeTableInfo> expected = null; // TODO: 初始化为适当的值
            List<QH_CapitalAccountFreezeTableInfo> actual;
            //actual = target.PagingQueryQH_CapitalAccountFreezeTableByUserID(userID, startTime, endTime, type, QueryType.QueryFreezeType.ALL, pageInfo, out total, out errorMsg);
            //Assert.AreEqual(totalExpected, total);
            //Assert.AreEqual(errorMsgExpected, errorMsg);
            //Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryQH_CapitalAccountFreezeTableByAccount 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryQH_CapitalAccountFreezeTableByAccountTest()
        {
            TraderFindService target = new TraderFindService(); // TODO: 初始化为适当的值
            string account = "010000000406"; // TODO: 初始化为适当的值
            DateTime startTime = DateTime.Parse("2009-06-12 16:09:42.860"); // TODO: 初始化为适当的值
            DateTime endTime = DateTime.Parse("2009-06-13 16:09:42.860");  // TODO: 初始化为适当的值
            QueryType.QueryCurrencyType type = QueryType.QueryCurrencyType.ALL; // TODO: 初始化为适当的值

            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = true;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;

            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            QueryType.QueryFreezeType freezeType = QueryType.QueryFreezeType.ALL;
            List<QH_CapitalAccountFreezeTableInfo> expected = null; // TODO: 初始化为适当的值
            List<QH_CapitalAccountFreezeTableInfo> actual;
            //actual = target.PagingQueryQH_CapitalAccountFreezeTableByAccount(account, startTime, endTime, type, freezeType, pageInfo, out total, out errorMsg);
            //Assert.AreEqual(totalExpected, total);
            //Assert.AreEqual(errorMsgExpected, errorMsg);
            //Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
