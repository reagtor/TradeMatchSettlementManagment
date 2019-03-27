using ReckoningCounter.BLL.AccountManagementAndFind.InterfaceBLL.AccountManagementAndFindService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReckoningCounter.Entity.AccountManagementAndFindEntity;
using System.Collections.Generic;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.Entity.Contants;
using GTA.VTS.Common.CommonObject;
using System;

namespace ReckoningCounterBizTest
{


    /// <summary>
    ///这是 AccountAndCapitalManagementServiceTest 的测试类，旨在
    ///包含所有 AccountAndCapitalManagementServiceTest 单元测试
    ///</summary>
    [TestClass()]
    public class AccountAndCapitalManagementServiceTest
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
        ///VolumeTraderOpenAccount 的测试
        ///</summary>
        [TestMethod()]
        public void VolumeTraderOpenAccountTest()
        {
            AccountAndCapitalManagementService target = new AccountAndCapitalManagementService(); // TODO: 初始化为适当的值
            List<AccountEntity> accounts = null; // TODO: 初始化为适当的值
            string outMessage = string.Empty; // TODO: 初始化为适当的值
            string outMessageExpected = string.Empty; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.VolumeTraderOpenAccount(accounts, out outMessage);
            Assert.AreEqual(outMessageExpected, outMessage);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///UpdateUserPassword 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateUserPasswordTest()
        {
            AccountAndCapitalManagementService target = new AccountAndCapitalManagementService(); // TODO: 初始化为适当的值
            string userId = string.Empty; // TODO: 初始化为适当的值
            string oldPassword = string.Empty; // TODO: 初始化为适当的值
            string newPassword = string.Empty; // TODO: 初始化为适当的值
            string outMessage = string.Empty; // TODO: 初始化为适当的值
            string outMessageExpected = string.Empty; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.UpdateUserPassword(userId, oldPassword, newPassword, out outMessage);
            Assert.AreEqual(outMessageExpected, outMessage);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///TwoAccountsFreeTransferFunds 的测试
        ///</summary>
        [TestMethod()]
        public void TwoAccountsFreeTransferFundsTest()
        {
            AccountAndCapitalManagementService target = new AccountAndCapitalManagementService(); // TODO: 初始化为适当的值
            FreeTransferEntity freeTransfer = null; // TODO: 初始化为适当的值
            GTA.VTS.Common.CommonObject.Types.CurrencyType currencyType = new GTA.VTS.Common.CommonObject.Types.CurrencyType(); // TODO: 初始化为适当的值
            string outMessage = string.Empty; // TODO: 初始化为适当的值
            string outMessageExpected = string.Empty; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.TwoAccountsFreeTransferFunds(freeTransfer, currencyType, out outMessage);
            Assert.AreEqual(outMessageExpected, outMessage);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///ThawAccount 的测试
        ///</summary>
        [TestMethod()]
        public void ThawAccountTest()
        {
            AccountAndCapitalManagementService target = new AccountAndCapitalManagementService(); // TODO: 初始化为适当的值
            List<FindAccountEntity> accounts = null; // TODO: 初始化为适当的值
            string outMessage = string.Empty; // TODO: 初始化为适当的值
            string outMessageExpected = string.Empty; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.ThawAccount(accounts, out outMessage);
            Assert.AreEqual(outMessageExpected, outMessage);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///SingleTraderOpenAccount 的测试
        ///</summary>
        [TestMethod()]
        public void SingleTraderOpenAccountTest()
        {
            AccountAndCapitalManagementService target = new AccountAndCapitalManagementService(); // TODO: 初始化为适当的值
            List<AccountEntity> accounts = null; // TODO: 初始化为适当的值
            string outMessage = string.Empty; // TODO: 初始化为适当的值
            string outMessageExpected = string.Empty; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.SingleTraderOpenAccount(accounts, out outMessage);
            Assert.AreEqual(outMessageExpected, outMessage);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///IsReckoningDone 的测试
        ///</summary>
        [TestMethod()]
        public void IsReckoningDoneTest()
        {
            AccountAndCapitalManagementService target = new AccountAndCapitalManagementService(); // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.IsReckoningDone(DateTime.Now);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///IsReckoning 的测试
        ///</summary>
        [TestMethod()]
        public void IsReckoningTest()
        {
            AccountAndCapitalManagementService target = new AccountAndCapitalManagementService(); // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.IsReckoning();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetSpotMaxOrderAmount 的测试
        ///</summary>
        [TestMethod()]
        public void GetSpotMaxOrderAmountTest()
        {
            AccountAndCapitalManagementService target = new AccountAndCapitalManagementService(); // TODO: 初始化为适当的值
            string TraderId = string.Empty; // TODO: 初始化为适当的值
            float OrderPrice = 0F; // TODO: 初始化为适当的值
            string Code = string.Empty; // TODO: 初始化为适当的值
            string outMessage = string.Empty; // TODO: 初始化为适当的值
            string outMessageExpected = string.Empty; // TODO: 初始化为适当的值
            ReckoningCounter.Entity.Contants.Types.OrderPriceType orderPriceType = new ReckoningCounter.Entity.Contants.Types.OrderPriceType(); // TODO: 初始化为适当的值
            long expected = 0; // TODO: 初始化为适当的值
            long actual;
            actual = target.GetSpotMaxOrderAmount(TraderId, OrderPrice, Code, out outMessage, orderPriceType);
            Assert.AreEqual(outMessageExpected, outMessage);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetFutureMaxOrderAmount 的测试
        ///</summary>
        [TestMethod()]
        public void GetFutureMaxOrderAmountTest()
        {
            AccountAndCapitalManagementService target = new AccountAndCapitalManagementService(); // TODO: 初始化为适当的值
            string TraderId = string.Empty; // TODO: 初始化为适当的值
            float OrderPrice = 0F; // TODO: 初始化为适当的值
            string Code = string.Empty; // TODO: 初始化为适当的值
            string outMessage = string.Empty; // TODO: 初始化为适当的值
            string outMessageExpected = string.Empty; // TODO: 初始化为适当的值
            ReckoningCounter.Entity.Contants.Types.OrderPriceType orderPriceType = new ReckoningCounter.Entity.Contants.Types.OrderPriceType(); // TODO: 初始化为适当的值
            long expected = 0; // TODO: 初始化为适当的值
            long actual;
            actual = target.GetFutureMaxOrderAmount(TraderId, OrderPrice, Code, out outMessage, orderPriceType);
            Assert.AreEqual(outMessageExpected, outMessage);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///FreezeAccount 的测试
        ///</summary>
        [TestMethod()]
        public void FreezeAccountTest()
        {
            AccountAndCapitalManagementService target = new AccountAndCapitalManagementService(); // TODO: 初始化为适当的值
            List<FindAccountEntity> accounts = null; // TODO: 初始化为适当的值
            string outMessage = string.Empty; // TODO: 初始化为适当的值
            string outMessageExpected = string.Empty; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.FreezeAccount(accounts, out outMessage);
            Assert.AreEqual(outMessageExpected, outMessage);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///FindTradePrivileges 的测试
        ///</summary>
        [TestMethod()]
        public void FindTradePrivilegesTest()
        {
            AccountAndCapitalManagementService target = new AccountAndCapitalManagementService(); // TODO: 初始化为适当的值
            string traderId = string.Empty; // TODO: 初始化为适当的值
            string password = string.Empty; // TODO: 初始化为适当的值
            string outMessage = string.Empty; // TODO: 初始化为适当的值
            string outMessageExpected = string.Empty; // TODO: 初始化为适当的值
            List<CM_BreedClass> expected = null; // TODO: 初始化为适当的值
            List<CM_BreedClass> actual;
            actual = target.FindTradePrivileges(traderId, password, out outMessage);
            Assert.AreEqual(outMessageExpected, outMessage);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///FindAccount 的测试
        ///</summary>
        [TestMethod()]
        public void FindAccountTest()
        {
            AccountAndCapitalManagementService target = new AccountAndCapitalManagementService(); // TODO: 初始化为适当的值
            string traderId = string.Empty; // TODO: 初始化为适当的值
            string password = string.Empty; // TODO: 初始化为适当的值
            string outMessage = string.Empty; // TODO: 初始化为适当的值
            string outMessageExpected = string.Empty; // TODO: 初始化为适当的值
            List<AccountFindResultEntity> expected = null; // TODO: 初始化为适当的值
            List<AccountFindResultEntity> actual;
            actual = target.FindAccount(traderId, password, out outMessage);
            Assert.AreEqual(outMessageExpected, outMessage);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///DeleteVolumeTraderAccount 的测试
        ///</summary>
        [TestMethod()]
        public void DeleteVolumeTraderAccountTest()
        {
            AccountAndCapitalManagementService target = new AccountAndCapitalManagementService(); // TODO: 初始化为适当的值
            string[] userIDs = null; // TODO: 初始化为适当的值
            string outMessage = string.Empty; // TODO: 初始化为适当的值
            string outMessageExpected = string.Empty; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.DeleteVolumeTraderAccount(userIDs, out outMessage);
            Assert.AreEqual(outMessageExpected, outMessage);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///DeleteSingleTraderAccount 的测试
        ///</summary>
        [TestMethod()]
        public void DeleteSingleTraderAccountTest()
        {
            AccountAndCapitalManagementService target = new AccountAndCapitalManagementService(); // TODO: 初始化为适当的值
            string userId = string.Empty; // TODO: 初始化为适当的值
            string outMessage = string.Empty; // TODO: 初始化为适当的值
            string outMessageExpected = string.Empty; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.DeleteSingleTraderAccount(userId, out outMessage);
            Assert.AreEqual(outMessageExpected, outMessage);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///ConvertUnitType 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ReckoningCounter.BLL.dll")]
        public void ConvertUnitTypeTest()
        {
            // 为“Microsoft.VisualStudio.TestTools.TypesAndSymbols.Assembly”创建专用访问器失败
            Assert.Inconclusive("为“Microsoft.VisualStudio.TestTools.TypesAndSymbols.Assembly”创建专用访问器失败");
        }

        /// <summary>
        ///CheckChannel 的测试
        ///</summary>
        [TestMethod()]
        public void CheckChannelTest()
        {
            AccountAndCapitalManagementService target = new AccountAndCapitalManagementService(); // TODO: 初始化为适当的值
            string expected = string.Empty; // TODO: 初始化为适当的值
            string actual;
            actual = target.CheckChannel();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///AddCapital 的测试
        ///</summary>
        [TestMethod()]
        public void AddCapitalTest()
        {
            AccountAndCapitalManagementService target = new AccountAndCapitalManagementService(); // TODO: 初始化为适当的值
            AddCapitalEntity addCapital = null; // TODO: 初始化为适当的值
            string outMessage = string.Empty; // TODO: 初始化为适当的值
            string outMessageExpected = string.Empty; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.AddCapital(addCapital, out outMessage);
            Assert.AreEqual(outMessageExpected, outMessage);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///AccountAndCapitalManagementService 构造函数 的测试
        ///</summary>
        [TestMethod()]
        public void AccountAndCapitalManagementServiceConstructorTest()
        {
            AccountAndCapitalManagementService target = new AccountAndCapitalManagementService();
            Assert.Inconclusive("TODO: 实现用来验证目标的代码");
        }
    }
}
