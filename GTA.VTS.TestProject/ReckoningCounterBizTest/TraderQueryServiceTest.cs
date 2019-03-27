using ReckoningCounter.BLL.AccountManagementAndFind.InterfaceBLL.AccountManagementAndFindService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GTA.VTS.Common.CommonObject;
using ReckoningCounter.Entity.AccountManagementAndFindEntity;
using System.Collections.Generic;
using System;
using ReckoningCounter.Entity.Model.QueryFilter;
using ReckoningCounter.Entity.Model;
using ReckoningCounter.Model;
using ReckoningCounter.Entity;

namespace ReckoningCounterBizTest
{


    /// <summary>
    ///这是 TraderQueryServiceTest 的测试类，旨在
    ///包含所有 TraderQueryServiceTest 单元测试
    ///</summary>
    [TestClass()]
    public class TraderQueryServiceTest
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
        ///SpotCapitalTradeAmountFind 的测试
        ///</summary>
        [TestMethod()]
        public void SpotCapitalTradeAmountFindTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string userId = string.Empty; // TODO: 初始化为适当的值
            string userPassword = string.Empty; // TODO: 初始化为适当的值
            string stockCode = string.Empty; // TODO: 初始化为适当的值
            int actual;
            int expected = 1000000;
            actual = target.SpotCapitalTradeAmountFind(userId, userPassword, stockCode);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///QueryXH_CapitalAccountTableByUserIDAndPwd 的测试
        ///</summary>
        [TestMethod()]
        public void QueryXH_CapitalAccountTableByUserIDAndPwdTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string userID = "27"; // TODO: 初始化为适当的值
            string pwd = "888888"; // TODO: 初始化为适当的值
            int accountType = 0;
            QueryType.QueryCurrencyType currencyType = new QueryType.QueryCurrencyType(); // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<XH_CapitalAccountTableInfo> expected = null; // TODO: 初始化为适当的值
            List<XH_CapitalAccountTableInfo> actual;
            actual = target.QueryXH_CapitalAccountTableByUserIDAndPwd(userID, pwd, accountType, currencyType, out errorMsg);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///QueryXH_CapitalAccountTableByUserID 的测试
        ///</summary>
        [TestMethod()]
        public void QueryXH_CapitalAccountTableByUserIDTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string userID = "27"; // TODO: 初始化为适当的值 
            int accountType = 0;
            QueryType.QueryCurrencyType currencyType = new QueryType.QueryCurrencyType(); // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<XH_CapitalAccountTableInfo> expected = null; // TODO: 初始化为适当的值
            List<XH_CapitalAccountTableInfo> actual;
            actual = target.QueryXH_CapitalAccountTableByUserID(userID, accountType, currencyType, out errorMsg);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///QueryXH_CapitalAccountTableByAccount 的测试
        ///</summary>
        [TestMethod()]
        public void QueryXH_CapitalAccountTableByAccountTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string xh_Cap_Account = "010000015402"; // TODO: 初始化为适当的值
            //QueryType.QueryCurrencyType currencyType = new QueryType.QueryCurrencyType(); // TODO: 初始化为适当的值
            QueryType.QueryCurrencyType currencyType = QueryType.QueryCurrencyType.RMB; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<XH_CapitalAccountTableInfo> expected = null; // TODO: 初始化为适当的值
            List<XH_CapitalAccountTableInfo> actual;
            actual = target.QueryXH_CapitalAccountTableByAccount(xh_Cap_Account, currencyType, out errorMsg);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///QueryXH_CapitalAccountFreezeTableByEntrustNo 的测试
        ///</summary>
        [TestMethod()]
        public void QueryXH_CapitalAccountFreezeTableByEntrustNoTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string entrustNo = "0906171705555931"; // TODO: 初始化为适当的值
            QueryType.QueryFreezeType freezeType = new QueryType.QueryFreezeType(); // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<XH_CapitalAccountFreezeTableInfo> expected = null; // TODO: 初始化为适当的值
            List<XH_CapitalAccountFreezeTableInfo> actual;
            actual = target.QueryXH_CapitalAccountFreezeTableByEntrustNo(entrustNo, freezeType, out errorMsg);

            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///QueryXH_AccountHoldTableByUserIDAndPwd 的测试
        ///</summary>
        [TestMethod()]
        public void QueryXH_AccountHoldTableByUserIDAndPwdTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string userID = "27"; // TODO: 初始化为适当的值
            string pwd = "888888"; // TODO: 初始化为适当的值
            int accountType = 0;
            QueryType.QueryCurrencyType currencyType = QueryType.QueryCurrencyType.RMB; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<XH_AccountHoldTableInfo> expected = null; // TODO: 初始化为适当的值
            List<XH_AccountHoldTableInfo> actual;
            actual = target.QueryXH_AccountHoldTableByUserIDAndPwd(userID, pwd, accountType, currencyType, out errorMsg);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///QueryXH_AccountHoldTableByUserID 的测试
        ///</summary>
        [TestMethod()]
        public void QueryXH_AccountHoldTableByUserIDTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string userID = "25"; // TODO: 初始化为适当的值
            int accountType = 0;
            QueryType.QueryCurrencyType currencyType = QueryType.QueryCurrencyType.RMB; // TODO: 初始化为适当的值
            //QueryType.QueryCurrencyType currencyType = QueryType.QueryCurrencyType.HK;
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<XH_AccountHoldTableInfo> expected = null; // TODO: 初始化为适当的值
            List<XH_AccountHoldTableInfo> actual;
            actual = target.QueryXH_AccountHoldTableByUserID(userID, accountType, currencyType, out errorMsg);

            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///QueryXH_AccountHoldTableByAccount 的测试
        ///</summary>
        [TestMethod()]
        public void QueryXH_AccountHoldTableByAccountTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string xh_Cap_Account = "010000001303"; // TODO: 初始化为适当的值
            QueryType.QueryCurrencyType currencyType = new QueryType.QueryCurrencyType(); // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<XH_AccountHoldTableInfo> expected = null; // TODO: 初始化为适当的值
            List<XH_AccountHoldTableInfo> actual;
            actual = target.QueryXH_AccountHoldTableByAccount(xh_Cap_Account, currencyType, out errorMsg);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///QueryXH_AcccountHoldFreezeTableByEntrustNo 的测试
        ///</summary>
        [TestMethod()]
        public void QueryXH_AcccountHoldFreezeTableByEntrustNoTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string entrustNo = "0906171332413751"; // TODO: 初始化为适当的值
            QueryType.QueryFreezeType freezeType = new QueryType.QueryFreezeType(); // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<XH_AcccountHoldFreezeTableInfo> expected = null; // TODO: 初始化为适当的值
            List<XH_AcccountHoldFreezeTableInfo> actual;
            actual = target.QueryXH_AcccountHoldFreezeTableByEntrustNo(entrustNo, freezeType, out errorMsg);

            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///QueryUA_BankAccountByUserID 的测试
        ///</summary>
        [TestMethod()]
        public void QueryUA_BankAccountByUserIDTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string userId = "27"; // TODO: 初始化为适当的值     
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<UA_BankAccountTableInfo> expected = null; // TODO: 初始化为适当的值
            List<UA_BankAccountTableInfo> actual;
            actual = target.QueryUA_BankAccountByUserID(userId, out errorMsg);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///QueryUA_BankAccountByBankAccount 的测试
        ///</summary>
        [TestMethod()]
        public void QueryUA_BankAccountByBankAccountTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string bankAccount = "010000005301"; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<UA_BankAccountTableInfo> expected = null; // TODO: 初始化为适当的值
            List<UA_BankAccountTableInfo> actual;
            actual = target.QueryUA_BankAccountByBankAccount(bankAccount, out errorMsg);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///QueryQH_HoldAccountTableByUserIDAndPwd 的测试
        ///</summary>
        [TestMethod()]
        public void QueryQH_HoldAccountTableByUserIDAndPwdTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string userID = "23"; // TODO: 初始化为适当的值
            string pwd = "888888"; // TODO: 初始化为适当的值
            int accountType = 0;
            QueryType.QueryCurrencyType currencyType = new QueryType.QueryCurrencyType(); // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<QH_HoldAccountTableInfo> expected = null; // TODO: 初始化为适当的值
            List<QH_HoldAccountTableInfo> actual;
            actual = target.QueryQH_HoldAccountTableByUserIDAndPwd(userID, pwd, accountType, currencyType, out errorMsg);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///QueryQH_HoldAccountTableByUserID 的测试
        ///</summary>
        [TestMethod()]
        public void QueryQH_HoldAccountTableByUserIDTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string userID = "25"; // TODO: 初始化为适当的值
            int accountType = 0;
            QueryType.QueryCurrencyType currencyType = new QueryType.QueryCurrencyType(); // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<QH_HoldAccountTableInfo> expected = null; // TODO: 初始化为适当的值
            List<QH_HoldAccountTableInfo> actual;
            actual = target.QueryQH_HoldAccountTableByUserID(userID, accountType, currencyType, out errorMsg);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///QueryQH_HoldAccountTableByAccount 的测试
        ///</summary>
        [TestMethod()]
        public void QueryQH_HoldAccountTableByAccountTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string xh_Cap_Account = "010000002307"; // TODO: 初始化为适当的值
            QueryType.QueryCurrencyType currencyType = QueryType.QueryCurrencyType.HK; // TODO: 初始化为适当的值
            //QueryType.QueryCurrencyType currencyType = QueryType.QueryCurrencyType.RMB; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<QH_HoldAccountTableInfo> expected = null; // TODO: 初始化为适当的值
            List<QH_HoldAccountTableInfo> actual;
            actual = target.QueryQH_HoldAccountTableByAccount(xh_Cap_Account, currencyType, out errorMsg);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///QueryQH_HoldAccountFreezeTableByEntrustNo 的测试
        ///</summary>
        [TestMethod()]
        public void QueryQH_HoldAccountFreezeTableByEntrustNoTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string entrustNo = "0905191222537961"; // TODO: 初始化为适当的值
            //QueryType.QueryFreezeType freezeType =   QueryType.QueryFreezeType.ALL; // TODO: 初始化为适当的值
            QueryType.QueryFreezeType freezeType = QueryType.QueryFreezeType.TodayHoldingCloseFreeze;
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<QH_HoldAccountFreezeTableInfo> expected = null; // TODO: 初始化为适当的值
            List<QH_HoldAccountFreezeTableInfo> actual;
            actual = target.QueryQH_HoldAccountFreezeTableByEntrustNo(entrustNo, freezeType, out errorMsg);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///QueryQH_CapitalAccountTableByUserIDAndPwd 的测试
        ///</summary>
        [TestMethod()]
        public void QueryQH_CapitalAccountTableByUserIDAndPwdTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string userID = "26"; // TODO: 初始化为适当的值
            string pwd = "888888"; // TODO: 初始化为适当的值
            int accountType = 0;
            // QueryType.QueryCurrencyType currencyType = QueryType.QueryCurrencyType.ALL; // TODO: 初始化为适当的值
            QueryType.QueryCurrencyType currencyType = QueryType.QueryCurrencyType.RMB;
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<QH_CapitalAccountTableInfo> actual;
            List<QH_CapitalAccountTableInfo> expected = null;
            actual = target.QueryQH_CapitalAccountTableByUserIDAndPwd(userID, pwd, accountType, currencyType, out errorMsg);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///QueryQH_CapitalAccountTableByUserID 的测试
        ///</summary>
        [TestMethod()]
        public void QueryQH_CapitalAccountTableByUserIDTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string userID = "19"; // TODO: 初始化为适当的值
            int accountType = 0;
            //QueryType.QueryCurrencyType currencyType = new QueryType.QueryCurrencyType(); // TODO: 初始化为适当的值
            QueryType.QueryCurrencyType currencyType = QueryType.QueryCurrencyType.RMB;
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<QH_CapitalAccountTableInfo> expected = null; // TODO: 初始化为适当的值
            List<QH_CapitalAccountTableInfo> actual;
            actual = target.QueryQH_CapitalAccountTableByUserID(userID, accountType, currencyType, out errorMsg);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///QueryQH_CapitalAccountTableByAccount 的测试
        ///</summary>
        [TestMethod()]
        public void QueryQH_CapitalAccountTableByAccountTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string qh_Cap_Account = "010000009904"; // TODO: 初始化为适当的值
            QueryType.QueryCurrencyType currencyType = new QueryType.QueryCurrencyType(); // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<QH_CapitalAccountTableInfo> expected = null; // TODO: 初始化为适当的值
            List<QH_CapitalAccountTableInfo> actual;
            actual = target.QueryQH_CapitalAccountTableByAccount(qh_Cap_Account, currencyType, out errorMsg);

            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///QueryQH_CapitalAccountFreezeTableByEntrustNo 的测试
        ///</summary>
        [TestMethod()]
        public void QueryQH_CapitalAccountFreezeTableByEntrustNoTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            //string entrustNo = "0906120409427961"; // TODO: 初始化为适当的值
            string entrustNo = "0905041112459211"; // TODO: 初始化为适当的值

            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            QueryType.QueryFreezeType freezeType = QueryType.QueryFreezeType.ALL;
            List<QH_CapitalAccountFreezeTableInfo> expected = null; // TODO: 初始化为适当的值
            List<QH_CapitalAccountFreezeTableInfo> actual;
            actual = target.QueryQH_CapitalAccountFreezeTableByEntrustNo(entrustNo, freezeType, out errorMsg);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///QueryAssetSummary 的测试
        ///</summary>
        [TestMethod()]
        public void QueryAssetSummaryTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string traderId = "23"; // TODO: 初始化为适当的值
            string password = "888888"; // TODO: 初始化为适当的值
            string outMessage = string.Empty; // TODO: 初始化为适当的值
            string outMessageExpected = string.Empty; // TODO: 初始化为适当的值
            List<AssetSummaryEntity> expected = null; // TODO: 初始化为适当的值
            List<AssetSummaryEntity> actual;
            actual = target.QueryAssetSummary(traderId, password, out outMessage);
            Assert.AreEqual(outMessageExpected, outMessage);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PaginQueryUA_CapitalFlowTableByFilter 的测试
        ///</summary>
        [TestMethod()]
        public void PaginQueryUA_CapitalFlowTableByFilterTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string userId = "23"; // TODO: 初始化为适当的值
            string pwd = "888888"; // TODO: 初始化为适当的值
            int accountType = 0; // TODO: 初始化为适当的值
            QueryUA_CapitalFlowFilter filter = new QueryUA_CapitalFlowFilter(); // TODO: 初始化为适当的值
            filter.CurrencyType = QueryType.QueryCurrencyType.ALL;
            //filter.CurrencyType = QueryType.QueryCurrencyType.RMB; 
            // filter.FromCapitalAccount = "010000002304";
            //filter.ToCapitalAccount = "010000002307";
            filter.StartTime = new DateTime(2009, 03, 05);
            filter.EndTime = new DateTime(2009, 03, 06);

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
            actual = target.PaginQueryUA_CapitalFlowTableByFilter(userId, pwd, accountType, filter, pageInfo, out total, out errorMsg);

            Assert.AreEqual(totalExpected, total);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryXH_TodayTradeByFilterAndUserIDPwd 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryXH_TodayTradeByFilterAndUserIDPwdTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string userID = "23"; // TODO: 初始化为适当的值
            string pwd = "888888"; // TODO: 初始化为适当的值
            SpotTradeConditionFindEntity filter = new SpotTradeConditionFindEntity(); // TODO: 初始化为适当的值
            filter.BuySellDirection = 1;
            filter.CurrencyType = 1;

            filter.StartTime = new DateTime(2009, 01, 10);
            filter.EndTime = new DateTime(2009, 10, 10);

            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = true;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;
            int accountType = 0;
            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0;
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值

            List<XH_TodayTradeTableInfo> actual;
            List<XH_TodayTradeTableInfo> expected = null;
            actual = target.PagingQueryXH_TodayTradeByFilterAndUserIDPwd(userID, pwd, accountType, filter, pageInfo, out total, out errorMsg);
            Assert.AreEqual(totalExpected, total);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryXH_TodayEntrustInfoByFilterAndUserIDPwd 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryXH_TodayEntrustInfoByFilterAndUserIDPwdTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string userID = "27"; // TODO: 初始化为适当的值
            string pwd = "888888"; // TODO: 初始化为适当的值
            SpotEntrustConditionFindEntity filter = new SpotEntrustConditionFindEntity(); // TODO: 初始化为适当的值
            filter.BelongToMarket = 32;
            filter.BuySellDirection = 1;
            filter.CurrencyType = 2;
            filter.StartTime = DateTime.Now.AddDays(-30);
            // filter.StartTime = new DateTime(2009, 01, 10);
            filter.EndTime = DateTime.Now;
            filter.CanBeWithdrawnLogo = 1;
            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = true;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;
            int accountType = 0;
            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<XH_TodayEntrustTableInfo> expected = null; // TODO: 初始化为适当的值
            List<XH_TodayEntrustTableInfo> actual;
            actual = target.PagingQueryXH_TodayEntrustInfoByFilterAndUserIDPwd(userID, pwd, accountType, filter, pageInfo, out total, out errorMsg);
            Assert.AreEqual(totalExpected, total);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryXH_HistoryTradeByFilterAndUserIDPwd 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryXH_HistoryTradeByFilterAndUserIDPwdTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string userID = "23"; // TODO: 初始化为适当的值
            string pwd = "888888"; // TODO: 初始化为适当的值
            SpotTradeConditionFindEntity filter = new SpotTradeConditionFindEntity(); // TODO: 初始化为适当的值
            filter.BelongToMarket = 32;
            filter.BuySellDirection = 1;
            filter.CurrencyType = 1;
            filter.StartTime = new DateTime(2009, 01, 10);
            filter.EndTime = DateTime.Now;
            filter.TradeType = 2;


            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = true;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;
            int accountType = 0;
            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<XH_HistoryTradeTableInfo> expected = null; // TODO: 初始化为适当的值
            List<XH_HistoryTradeTableInfo> actual;
            actual = target.PagingQueryXH_HistoryTradeByFilterAndUserIDPwd(userID, pwd, accountType, filter, pageInfo, out total, out errorMsg);
            Assert.AreEqual(totalExpected, total);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryXH_HistoryEntrustInfoByFilterAndUserIDPwd 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryXH_HistoryEntrustInfoByFilterAndUserIDPwdTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string userID = "27"; // TODO: 初始化为适当的值
            string pwd = "888888"; // TODO: 初始化为适当的值
            SpotEntrustConditionFindEntity filter = new SpotEntrustConditionFindEntity(); // TODO: 初始化为适当的值
            filter.BelongToMarket = 32;
            filter.BuySellDirection = 1;
            filter.CurrencyType = 1;
            //filter.StartTime = DateTime.Now.AddDays(-30);
            filter.StartTime = new DateTime(2009, 01, 10);
            filter.EndTime = DateTime.Now;
            filter.CanBeWithdrawnLogo = 1;
            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = true;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;
            int accountType = 0;
            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<XH_HistoryEntrustTableInfo> expected = null; // TODO: 初始化为适当的值
            List<XH_HistoryEntrustTableInfo> actual;
            actual = target.PagingQueryXH_HistoryEntrustInfoByFilterAndUserIDPwd(userID, pwd, accountType, filter, pageInfo, out total, out errorMsg);
            Assert.AreEqual(totalExpected, total);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryXH_CapitalAccountFreezeTableByUserID 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryXH_CapitalAccountFreezeTableByUserIDTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string userID = "23";  // TODO: 初始化为适当的值
            Nullable<DateTime> startTime = new Nullable<DateTime>(); // TODO: 初始化为适当的值
            Nullable<DateTime> endTime = new Nullable<DateTime>(); // TODO: 初始化为适当的值
            QueryType.QueryCurrencyType currencyType = new QueryType.QueryCurrencyType(); // TODO: 初始化为适当的值
            QueryType.QueryFreezeType freezeType = new QueryType.QueryFreezeType(); // TODO: 初始化为适当的值
            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = true;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;
            int accountType = 0;
            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<XH_CapitalAccountFreezeTableInfo> expected = null; // TODO: 初始化为适当的值
            List<XH_CapitalAccountFreezeTableInfo> actual;
            actual = target.PagingQueryXH_CapitalAccountFreezeTableByUserID(userID, accountType, startTime, endTime, currencyType, freezeType, pageInfo, out total, out errorMsg);

            Assert.AreEqual(totalExpected, total);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryXH_CapitalAccountFreezeTableByAccount 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryXH_CapitalAccountFreezeTableByAccountTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string account = "010000000402"; // TODO: 初始化为适当的值
            Nullable<DateTime> startTime = new Nullable<DateTime>(); // TODO: 初始化为适当的值
            startTime = new DateTime(2009, 1, 01);

            Nullable<DateTime> endTime = new Nullable<DateTime>(); // TODO: 初始化为适当的值
            endTime = new DateTime(2009, 8, 01);
            QueryType.QueryCurrencyType currencyType = new QueryType.QueryCurrencyType(); // TODO: 初始化为适当的值
            QueryType.QueryFreezeType freezeType = new QueryType.QueryFreezeType(); // TODO: 初始化为适当的值
            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = true;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;
            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<XH_CapitalAccountFreezeTableInfo> expected = null; // TODO: 初始化为适当的值
            List<XH_CapitalAccountFreezeTableInfo> actual;
            actual = target.PagingQueryXH_CapitalAccountFreezeTableByAccount(account, startTime, endTime, currencyType, freezeType, pageInfo, out total, out errorMsg);
            Assert.AreEqual(totalExpected, total);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryXH_AcccountHoldFreezeTableByUserID 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryXH_AcccountHoldFreezeTableByUserIDTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string userID = "4"; // TODO: 初始化为适当的值

            Nullable<DateTime> startTime = new Nullable<DateTime>(); // TODO: 初始化为适当的值
            startTime = new DateTime(2009, 1, 01);
            Nullable<DateTime> endTime = new Nullable<DateTime>(); // TODO: 初始化为适当的值
            endTime = DateTime.Now;
            QueryType.QueryCurrencyType currencyType = new QueryType.QueryCurrencyType(); // TODO: 初始化为适当的值
            QueryType.QueryFreezeType freezeType = new QueryType.QueryFreezeType(); // TODO: 初始化为适当的值
            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = true;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;
            int accountType = 0;
            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<XH_AcccountHoldFreezeTableInfo> expected = null; // TODO: 初始化为适当的值
            List<XH_AcccountHoldFreezeTableInfo> actual;
            actual = target.PagingQueryXH_AcccountHoldFreezeTableByUserID(userID, accountType, startTime, endTime, currencyType, freezeType, pageInfo, out total, out errorMsg);
            Assert.AreEqual(totalExpected, total);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryXH_AcccountHoldFreezeTableByAccount 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryXH_AcccountHoldFreezeTableByAccountTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string holdAccount = "010000000403"; // TODO: 初始化为适当的值
            Nullable<DateTime> startTime = new Nullable<DateTime>(); // TODO: 初始化为适当的值
            startTime = new DateTime(2009, 1, 01);
            Nullable<DateTime> endTime = new Nullable<DateTime>(); // TODO: 初始化为适当的值
            endTime = DateTime.Now;
            QueryType.QueryCurrencyType currencyType = new QueryType.QueryCurrencyType(); // TODO: 初始化为适当的值
            QueryType.QueryFreezeType freezeType = QueryType.QueryFreezeType.ReckoningFreeze; // TODO: 初始化为适当的值
            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = true;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;
            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<XH_AcccountHoldFreezeTableInfo> expected = null; // TODO: 初始化为适当的值
            List<XH_AcccountHoldFreezeTableInfo> actual;
            actual = target.PagingQueryXH_AcccountHoldFreezeTableByAccount(holdAccount, startTime, endTime, currencyType, freezeType, pageInfo, out total, out errorMsg);
            Assert.AreEqual(totalExpected, total);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQuerySpotHold 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQuerySpotHoldTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string userId = "23"; // TODO: 初始化为适当的值
            string userPassword = "888888"; // TODO: 初始化为适当的值
            int accountType = 3; // TODO: 初始化为适当的值
            SpotHoldConditionFindEntity findCondition = null; // TODO: 初始化为适当的值
            int start = 1; // TODO: 初始化为适当的值
            int pageLength = 12; // TODO: 初始化为适当的值
            int count = 0; // TODO: 初始化为适当的值
            int countExpected = 0; // TODO: 初始化为适当的值
            string strErrorMessage = string.Empty; // TODO: 初始化为适当的值
            string strErrorMessageExpected = string.Empty; // TODO: 初始化为适当的值
            List<SpotHoldFindResultEntity> expected = null; // TODO: 初始化为适当的值
            List<SpotHoldFindResultEntity> actual;
            actual = target.PagingQuerySpotHold(userId, accountType, userPassword, findCondition, start, pageLength, out count, ref strErrorMessage);
            Assert.AreEqual(countExpected, count);
            Assert.AreEqual(strErrorMessageExpected, strErrorMessage);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQuerySpotCapital 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQuerySpotCapitalTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string userId = "23"; // TODO: 初始化为适当的值
            int accountType = 2; // TODO: 初始化为适当的值
            //Types.CurrencyType currencyType = Types.CurrencyType.RMB; // TODO: 初始化为适当的值
            Types.CurrencyType currencyType = Types.CurrencyType.HK; // TODO: 初始化为适当的值
            string userPassword = "888888"; // TODO: 初始化为适当的值
            string strErrorMessage = string.Empty; // TODO: 初始化为适当的值
            string strErrorMessageExpected = string.Empty; // TODO: 初始化为适当的值
            SpotCapitalEntity expected = null; // TODO: 初始化为适当的值
            SpotCapitalEntity actual;
            actual = target.PagingQuerySpotCapital(userId, accountType, currencyType, userPassword, ref strErrorMessage);
            Assert.AreEqual(strErrorMessageExpected, strErrorMessage);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryQH_TodayTradeByFilterAndUserIDPwd 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryQH_TodayTradeByFilterAndUserIDPwdTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string userID = "27"; // TODO: 初始化为适当的值
            string pwd = "888888"; // TODO: 初始化为适当的值
            FuturesTradeConditionFindEntity filter = null; // TODO: 初始化为适当的值

            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = true;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;
            int accountType = 0;
            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<QH_TodayTradeTableInfo> expected = null; // TODO: 初始化为适当的值
            List<QH_TodayTradeTableInfo> actual;
            actual = target.PagingQueryQH_TodayTradeByFilterAndUserIDPwd(userID, pwd, accountType, filter, pageInfo, out total, out errorMsg);
            Assert.AreEqual(totalExpected, total);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryQH_TodayEntrustInfoByFilterAndUserIDPwd 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryQH_TodayEntrustInfoByFilterAndUserIDPwdTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string userID = "23"; // TODO: 初始化为适当的值
            string pwd = "888888"; // TODO: 初始化为适当的值
            FuturesEntrustConditionFindEntity filter = new FuturesEntrustConditionFindEntity(); // TODO: 初始化为适当的值
            filter.BelongToMarket = 32;
            filter.BuySellDirection = 1;
            filter.CurrencyTypeId = 1;
            filter.StartTime = new DateTime(2009, 01, 10);
            filter.EndTime = DateTime.Now;
            filter.CanBeWithdrawnLogo = 1;


            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = true;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;
            int accountType = 0;
            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0;
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<QH_TodayEntrustTableInfo> actual;
            List<QH_TodayEntrustTableInfo> expected = null;
            actual = target.PagingQueryQH_TodayEntrustInfoByFilterAndUserIDPwd(userID, pwd, accountType, filter, pageInfo, out total, out errorMsg);
            Assert.AreEqual(totalExpected, total);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryQH_HoldAccountFreezeTableByUserID 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryQH_HoldAccountFreezeTableByUserIDTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            //string userID = "23"; // TODO: 初始化为适当的值
            string userID = "25"; // TODO: 初始化为适当的值
            Nullable<DateTime> startTime = new Nullable<DateTime>(); // TODO: 初始化为适当的值
            startTime = new DateTime(2009, 1, 01);
            Nullable<DateTime> endTime = new Nullable<DateTime>(); // TODO: 初始化为适当的值
            endTime = new DateTime(2009, 8, 01);
            QueryType.QueryCurrencyType currencyType = new QueryType.QueryCurrencyType(); // TODO: 初始化为适当的值
            QueryType.QueryFreezeType freezeType = new QueryType.QueryFreezeType(); // TODO: 初始化为适当的值
            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = true;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;
            int accountType = 0;
            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<QH_HoldAccountFreezeTableInfo> expected = null; // TODO: 初始化为适当的值
            List<QH_HoldAccountFreezeTableInfo> actual;
            actual = target.PagingQueryQH_HoldAccountFreezeTableByUserID(userID, accountType, startTime, endTime, currencyType, freezeType, pageInfo, out total, out errorMsg);
            Assert.AreEqual(totalExpected, total);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryQH_HoldAccountFreezeTableByAccount 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryQH_HoldAccountFreezeTableByAccountTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            //string holdAccount = "010000002507"; // TODO: 初始化为适当的值
            string holdAccount = "010000002307"; // TODO: 初始化为适当的值
            Nullable<DateTime> startTime = new Nullable<DateTime>(); // TODO: 初始化为适当的值
            startTime = new DateTime(2009, 1, 01);
            Nullable<DateTime> endTime = new Nullable<DateTime>(); // TODO: 初始化为适当的值
            endTime = new DateTime(2009, 8, 01);
            QueryType.QueryCurrencyType currencyType = new QueryType.QueryCurrencyType(); // TODO: 初始化为适当的值
            QueryType.QueryFreezeType freezeType = new QueryType.QueryFreezeType(); // TODO: 初始化为适当的值
            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = true;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;
            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<QH_HoldAccountFreezeTableInfo> expected = null; // TODO: 初始化为适当的值
            List<QH_HoldAccountFreezeTableInfo> actual;
            actual = target.PagingQueryQH_HoldAccountFreezeTableByAccount(holdAccount, startTime, endTime, currencyType, freezeType, pageInfo, out total, out errorMsg);
            Assert.AreEqual(totalExpected, total);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryQH_HistoryTradeByFilterAndUserIDPwd 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryQH_HistoryTradeByFilterAndUserIDPwdTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string userID = "23"; // TODO: 初始化为适当的值
            string pwd = "888888"; // TODO: 初始化为适当的值
            FuturesTradeConditionFindEntity filter = new FuturesTradeConditionFindEntity(); // TODO: 初始化为适当的值
            filter.BuySellDirection = 1;
            filter.CurrencyTypeId = 1;
            filter.OpenCloseDirection = 2;
            filter.StartTime = new DateTime(2009, 01, 10);
            filter.EndTime = new DateTime(2009, 10, 10);
            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = true;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 1;
            int accountType = 0;
            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0;
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<QH_HistoryTradeTableInfo> actual;
            List<QH_HistoryTradeTableInfo> expected = null;
            actual = target.PagingQueryQH_HistoryTradeByFilterAndUserIDPwd(userID, pwd, accountType, filter, pageInfo, out total, out errorMsg);
            Assert.AreEqual(totalExpected, total);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryQH_HistoryEntrustInfoByFilterAndUserIDPwd 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryQH_HistoryEntrustInfoByFilterAndUserIDPwdTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string userID = "27"; // TODO: 初始化为适当的值
            string pwd = "888888"; // TODO: 初始化为适当的值
            FuturesEntrustConditionFindEntity filter = new FuturesEntrustConditionFindEntity(); // TODO: 初始化为适当的值
            filter.BelongToMarket = 32;
            filter.BuySellDirection = 1;
            filter.CurrencyTypeId = 1;
            filter.StartTime = new DateTime(2009, 01, 10);
            filter.EndTime = DateTime.Now;
            filter.CanBeWithdrawnLogo = 1;


            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = true;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;
            int accountType = 0;
            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            List<QH_HistoryEntrustTableInfo> expected = null; // TODO: 初始化为适当的值
            List<QH_HistoryEntrustTableInfo> actual;
            actual = target.PagingQueryQH_HistoryEntrustInfoByFilterAndUserIDPwd(userID, pwd, accountType, filter, pageInfo, out total, out errorMsg);
            Assert.AreEqual(totalExpected, total);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryQH_CapitalAccountFreezeTableByUserID 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryQH_CapitalAccountFreezeTableByUserIDTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            //string userID = "27"; // TODO: 初始化为适当的值
            string userID = "qq"; // TODO: 初始化为适当的值
            //Nullable<DateTime> startTime = new Nullable<DateTime>(); // TODO: 初始化为适当的值
            //Nullable<DateTime> endTime = new Nullable<DateTime>(); // TODO: 初始化为适当的值
            //开始或者结束时间为null就只查询当前的前一个月的时间内数据
            DateTime startTime = new DateTime(2009, 01, 10);
            DateTime endTime = DateTime.Now;

            QueryType.QueryCurrencyType currencyType = QueryType.QueryCurrencyType.ALL; // TODO: 初始化为适当的值

            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = false;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;
            int accountType = 0;
            int total = 0; // TODO: 初始化为适当的值
            int totalExpected = 0; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            QueryType.QueryFreezeType freezeType = QueryType.QueryFreezeType.ALL;
            List<QH_CapitalAccountFreezeTableInfo> actual;
            List<QH_CapitalAccountFreezeTableInfo> expected = null;
            actual = target.PagingQueryQH_CapitalAccountFreezeTableByUserID(userID, accountType, startTime, endTime, currencyType, freezeType, pageInfo, out total, out errorMsg);
            Assert.AreEqual(totalExpected, total);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryQH_CapitalAccountFreezeTableByAccount 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryQH_CapitalAccountFreezeTableByAccountTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            //string account = "010000000406"; // TODO: 初始化为适当的值
            string account = "qhzjqq"; // TODO: 初始化为适当的值
            DateTime startTime = DateTime.Parse("2009-01-1 16:09:42.860"); // TODO: 初始化为适当的值
            DateTime endTime = DateTime.Parse("2009-06-13 16:09:42.860");  // TODO: 初始化为适当的值
            QueryType.QueryCurrencyType currencyType = QueryType.QueryCurrencyType.ALL; // TODO: 初始化为适当的值

            PagingInfo pageInfo = new PagingInfo(); // TODO: 初始化为适当的值
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = true;
            pageInfo.PageLength = 12;
            pageInfo.Sort = 0;

            int total = 0; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            string errorMsg = string.Empty; // TODO: 初始化为适当的值
            string errorMsgExpected = string.Empty; // TODO: 初始化为适当的值
            QueryType.QueryFreezeType freezeType = QueryType.QueryFreezeType.ALL;
            List<QH_CapitalAccountFreezeTableInfo> actual;
            List<QH_CapitalAccountFreezeTableInfo> totalExpected = null;
            actual = target.PagingQueryQH_CapitalAccountFreezeTableByAccount(account, startTime, endTime, currencyType, freezeType, pageInfo, out total, out errorMsg);
            Assert.AreEqual(totalExpected, total);
            Assert.AreEqual(errorMsgExpected, errorMsg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryFuturesHold 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryFuturesHoldTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string userId = "23"; // TODO: 初始化为适当的值
            string userPassword = "888888"; // TODO: 初始化为适当的值
            int accountType = 7; // TODO: 初始化为适当的值
            //int accountType = 5; // TODO: 初始化为适当的值
            FuturesHoldConditionFindEntity findCondition = null; // TODO: 初始化为适当的值
            int start = 0; // TODO: 初始化为适当的值
            int pageLength = 12; // TODO: 初始化为适当的值
            int count = 0; // TODO: 初始化为适当的值
            int countExpected = 0; // TODO: 初始化为适当的值
            string strErrorMessage = string.Empty; // TODO: 初始化为适当的值
            string strErrorMessageExpected = string.Empty; // TODO: 初始化为适当的值
            List<FuturesHoldFindResultEntity> expected = null; // TODO: 初始化为适当的值
            List<FuturesHoldFindResultEntity> actual;
            actual = target.PagingQueryFuturesHold(userId, accountType, userPassword, findCondition, start, pageLength, out count, ref strErrorMessage);
            Assert.AreEqual(countExpected, count);
            Assert.AreEqual(strErrorMessageExpected, strErrorMessage);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///PagingQueryFuturesCapital 的测试
        ///</summary>
        [TestMethod()]
        public void PagingQueryFuturesCapitalTest()
        {
            TraderQueryService target = new TraderQueryService(); // TODO: 初始化为适当的值
            string userId = "23"; // TODO: 初始化为适当的值
            int accountType = 4; // TODO: 初始化为适当的值
            Types.CurrencyType currencyType = Types.CurrencyType.RMB; // TODO: 初始化为适当的值
            string userPassword = "888888"; // TODO: 初始化为适当的值
            string strErrorMessage = string.Empty; // TODO: 初始化为适当的值
            string strErrorMessageExpected = string.Empty; // TODO: 初始化为适当的值
            FuturesCapitalEntity expected = null; // TODO: 初始化为适当的值
            FuturesCapitalEntity actual;
            actual = target.PagingQueryFuturesCapital(userId, accountType, currencyType, userPassword, ref strErrorMessage);
            Assert.AreEqual(strErrorMessageExpected, strErrorMessage);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///TraderQueryService 构造函数 的测试
        ///</summary>
        [TestMethod()]
        public void TraderQueryServiceConstructorTest()
        {
            TraderQueryService target = new TraderQueryService();
            Assert.Inconclusive("TODO: 实现用来验证目标的代码");
        }
    }
}
