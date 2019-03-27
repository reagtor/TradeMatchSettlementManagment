using ReckoningCounter.BLL.ManagementCenter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReckoningCounter.DAL.FuturesDevolveService;
using System.Collections.Generic;

namespace ReckoningCounterBizTest
{
    
    
    /// <summary>
    ///这是 FutureBailScaleServiceTest 的测试类，旨在
    ///包含所有 FutureBailScaleServiceTest 单元测试
    ///</summary>
    [TestClass()]
    public class FutureBailScaleServiceTest
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
        ///AssemblingFilterCFBail 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ReckoningCounter.BLL.dll")]
        public void AssemblingFilterCFBailTest()
        {
            IList<QH_CFBailScaleValue> values = new List<QH_CFBailScaleValue>();
            QH_CFBailScaleValue item1 = new QH_CFBailScaleValue();
            item1.BailScale = 30;
            item1.BreedClassID=66;
            item1.CFBailScaleValueID=40;
            item1.DeliveryMonthType=1;
            item1.Ends=31;
            item1.LowerLimitIfEquation=1;
            item1.PositionBailTypeID=3;
            item1.RelationScaleID=59;
            item1.Start=1;
            item1.UpperLimitIfEquation = 1;
            values.Add(item1);
            QH_CFBailScaleValue item2 = new QH_CFBailScaleValue();
            item2.BailScale = 40;
            item2.BreedClassID = 66;
            item2.CFBailScaleValueID = 59;
            item2.DeliveryMonthType = 1;
            item2.Ends = 2;
            item2.LowerLimitIfEquation = 1;
            item2.PositionBailTypeID = 3;
            item2.RelationScaleID = null;
            item2.Start =null;
            item2.UpperLimitIfEquation = 1;
            values.Add(item2);
            List<QH_CFBailScaleValue> expected = null; // TODO: 初始化为适当的值
            List<QH_CFBailScaleValue> actual;
            //actual = FutureBailScaleService_Accessor.AssemblingFilterCFBail(values);
            //actual = FutureBailScaleService_Accessor.AssemblingFilterCFBail(values);
            //Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
