using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.Entity.Contants;
using ReckoningCounter.MemoryData;
using ReckoningCounter.MemoryData.XH.Capital;
using ReckoningCounter.MemoryData.XH.Hold;
using ReckoningCounter.Model;

namespace ReckoningCounter.MemoryData.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test0();
            //Test1();
            //Test11();
            //Test111();
            //Test2();
            //Test3();
            //Test4();

            //TestHold1();
            //TestHold11();
            //TestHold2();
            //TestHold3();
            //TestHold4();

            //TestQHCapital1();

            //TestQHHold1();
            //TestQHHold2();


            //Test1T();
            //Test2T();

            //TestAccountManager();

            //Test22();
            Test222();
        }

        private static void ExitConsole()
        {
            Console.Write("输入任意字符退出……");
            Console.ReadKey(true);
        }

        private static void Test0()
        {
            Console.WriteLine("测试故障恢复");
            MemoryDataManager.Start();
            MemoryDataManager.End();
            ExitConsole();
        }

        /// <summary>
        /// 一个资金id多次插入
        /// </summary>
        private static void Test1()
        {
            MemoryDataManager.Start();

            var table = MemoryDataManager.XHCapitalMemoryList.GetByCapitalAccountLogo(2);

            Console.WriteLine("开始Test1");
            //table.AddDelta(1, 1, 1, 1);

            //table.AddDelta(2, 2, 2, 2);

            //table.AddDelta(3, 3, 3, 3);

            ExitConsole();
            MemoryDataManager.End();
        }

        /// <summary>
        /// 一个资金id多次插入
        /// </summary>
        private static void Test1T()
        {
            MemoryDataManager.Start();

            var table = MemoryDataManager.XHCapitalMemoryList.GetByCapitalAccountLogo(2);
            var table1 = MemoryDataManager.XHHoldMemoryList.GetByAccountHoldLogoId(4);

            Console.WriteLine("开始Test1T");
            XH_CapitalAccountTable_DeltaInfo delta = new XH_CapitalAccountTable_DeltaInfo();
            delta.AvailableCapitalDelta = 1;

            XH_AccountHoldTableInfo_Delta holdDelta = new XH_AccountHoldTableInfo_Delta();
            var data = table1.Data;
            holdDelta.Data = data;
            holdDelta.AccountHoldLogoId = data.AccountHoldLogoId;
            holdDelta.AvailableAmountDelta = 1;
            holdDelta.FreezeAmountDelta = 1;

            Database database = DatabaseFactory.CreateDatabase();

            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                bool isSuccess = false;
                try
                {
                    //table.AddDeltaToDB(delta, database, transaction);

                    //MakeException();

                    //table.AddDeltaToDB(delta, database, transaction);
                    //table.AddDeltaToDB(delta, database, transaction);
                  
                    //table1.AddDeltaToDB(holdDelta,database,transaction);
                    transaction.Commit();
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    isSuccess = false;
                    Console.WriteLine("Exception:" + ex.Message);
                }

                if(isSuccess)
                {
                    //table.AddDeltaToMemory(delta);
                    //table.AddDeltaToMemory(delta);
                    //table.AddDeltaToMemory(delta);

                    //table1.AddDeltaToMemory(holdDelta);
                }
            }
            Console.WriteLine(table.Data.AvailableCapital);
            Console.WriteLine(table1.Data.AvailableAmount);


            ExitConsole();
            MemoryDataManager.End();
        }

        /// <summary>
        /// 一个资金id多次插入
        /// </summary>
        private static void Test2T()
        {
            MemoryDataManager.Start();

            var table = MemoryDataManager.QHCapitalMemoryList.GetByCapitalAccountLogo(1);
            var table1 = MemoryDataManager.QHHoldMemoryList.GetByAccountHoldLogoId(1);

            Console.WriteLine("开始Test1T");
            QH_CapitalAccountTable_DeltaInfo delta = new QH_CapitalAccountTable_DeltaInfo();
            delta.AvailableCapitalDelta = 1;

            QH_HoldAccountTableInfo_Delta holdDelta = new QH_HoldAccountTableInfo_Delta();
            var data = table1.Data;
            holdDelta.Data = data;
            holdDelta.AccountHoldLogoId = data.AccountHoldLogoId;
            holdDelta.TodayHoldAmountDelta += 1;

            Database database = DatabaseFactory.CreateDatabase();

            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                bool isSuccess = false;
                try
                {
                    //table.AddDeltaToDB(delta, database, transaction);

                    //MakeException();

                    //table.AddDeltaToDB(delta, database, transaction);
                    //table.AddDeltaToDB(delta, database, transaction);

                    //table1.AddDeltaToDB(holdDelta, database, transaction);
                    transaction.Commit();
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    isSuccess = false;
                    Console.WriteLine("Exception:" + ex.Message);
                }

                if (isSuccess)
                {
                    //table.AddDeltaToMemory(delta);
                    //table.AddDeltaToMemory(delta);
                    //table.AddDeltaToMemory(delta);

                    //table1.AddDeltaToMemory(holdDelta);
                }
            }
            Console.WriteLine(table.Data.AvailableCapital);
            Console.WriteLine(table1.Data.TodayHoldAmount);


            ExitConsole();
            MemoryDataManager.End();
        }

        private static void MakeException()
        {
            decimal a = 3;
            decimal b = 0;
            decimal c = a/b;
        }

        private static void Test111()
        {
            MemoryDataManager.Start();

            var table = MemoryDataManager.XHCapitalMemoryList.GetByCapitalAccountAndCurrencyType("010000000402",2);
            Console.WriteLine(table.Data.FreezeCapitalTotal);

            ExitConsole();
            MemoryDataManager.End();
        }

        public static void TestQHCapital1()
        {
            MemoryDataManager.Start();
            var table = MemoryDataManager.QHCapitalMemoryList.GetByCapitalAccountLogo(2);
            //table.AddDelta(1, 2, 3, 4, 5, 6);

            ExitConsole();
            MemoryDataManager.End();
        }


        /// <summary>
        /// 直接插入数据库
        /// </summary>
        private static void Test11()
        {

            for (int i = 0; i < 1000; i++)
            {
                Thread t = new Thread(DoAction);
                t.Start(i);
            }

            ExitConsole();
        }

        public static void DoAction(object obj)
        {
            int i = (int) obj;
            XH_CapitalAccountTableDal dal = new XH_CapitalAccountTableDal();

            DataManager.ExecuteInTransaction((database, transaction) =>
            {
                dal.AddUpdate(2, 1, 1, 1, 1, database, transaction);
                Console.WriteLine("执行：" + i);
            });
        }

        /// <summary>
        /// 一个资金id多线程插入
        /// </summary>
        private static void Test2()
        {
            MemoryDataManager.Start();

            var table = MemoryDataManager.XHCapitalMemoryList.GetByCapitalAccountLogo(2);

            Console.WriteLine("开始Test2");

            int sum = 0;
            for (int i = 0; i < 1000; i++)
            {
                sum += (i + 1);
            }

            RunTableThread(table);

            Console.WriteLine("总值：{0}", sum);
            ExitConsole();
            MemoryDataManager.End();
        }

        private static void RunTableThread(object tableObj)
        {
            XHCapitalMemoryTable table = tableObj as XHCapitalMemoryTable;
            if(table == null)
                return;

            for (int i = 0; i < 1000; i++)
            {
                Thread t = new Thread(Target1);
                TableObj<XHCapitalMemoryTable> obj = new TableObj<XHCapitalMemoryTable>();
                obj.Table = table;
                obj.val = i + 1;
                t.Start(obj);
            }

            Console.WriteLine("插入结束！");
        }

        public static void Target1(object obj)
        {
            TableObj<XHCapitalMemoryTable> tableObj = obj as TableObj<XHCapitalMemoryTable>;
            if(tableObj == null)
                return;

            //ShowMessage(0, tableObj.val, 0);
            int val = tableObj.val;
            //tableObj.Table.AddDelta(val, val, val, val);
        }

        /// <summary>
        /// 一个资金id多线程插入(带检查)
        /// </summary>
        public static void Test22()
        {
            MemoryDataManager.Start();

            var table = MemoryDataManager.XHCapitalMemoryList.GetByCapitalAccountLogo(1756);
            for (int i = 0; i < 2; i++)
            {
                Thread t = new Thread(Target3);
                TableObj<XHCapitalMemoryTable> obj = new TableObj<XHCapitalMemoryTable>();
                obj.Table = table;
                obj.val = i + 1;
                t.Start(obj);
            }

            ExitConsole();
            MemoryDataManager.End();
        }

        public static void Target2(object obj)
        {
            TableObj<XHCapitalMemoryTable> tableObj = obj as TableObj<XHCapitalMemoryTable>;
            if (tableObj == null)
                return;

            //ShowMessage(0, tableObj.val, 0);
            int val = tableObj.val;
            var table = tableObj.Table;//.AddDelta(val, val, val, val);

            XH_CapitalAccountTable_DeltaInfo delta = new XH_CapitalAccountTable_DeltaInfo();
            delta.AvailableCapitalDelta = -val;

            if(table.Data.AvailableCapital - val > 0)
                table.AddDelta(delta);
            else
            {
                Console.WriteLine("-");
            }
        }

        public static void Target3(object obj)
        {
            TableObj<XHCapitalMemoryTable> tableObj = obj as TableObj<XHCapitalMemoryTable>;
            if (tableObj == null)
                return;

            //ShowMessage(0, tableObj.val, 0);
            int val = tableObj.val;
            var table = tableObj.Table;//.AddDelta(val, val, val, val);

            XH_CapitalAccountTable_DeltaInfo delta = new XH_CapitalAccountTable_DeltaInfo();
            delta.AvailableCapitalDelta = -val;

            bool isSuccess = false;
            bool isCapitalSuccess = false;

            Database database = DatabaseFactory.CreateDatabase();

            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    isCapitalSuccess = table.CheckAndAddDelta(func, delta, database, transaction);
                    if (!isCapitalSuccess)
                    {
                        Console.WriteLine("----------capitalfailure");
                        throw new Exception();
                    }

                    if(val/2!= 0)
                    {
                        throw new Exception();
                    }

                    transaction.Commit();
                    isSuccess = true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    isSuccess = false;
                    Console.WriteLine("事务失败！");
                }
            }

           if(!isSuccess)
           {
               if(isCapitalSuccess)
               {
                   table.RollBackMemory(delta);
                   Console.WriteLine("rollback data=" +table.Data.AvailableCapital);
               }
           }
        }

        private static bool func(XH_CapitalAccountTableInfo baseData, XH_CapitalAccountTable_DeltaInfo delta)
        {
            Console.WriteLine("base:" + baseData.AvailableCapital + " delta:" + delta.AvailableCapitalDelta);
            if(baseData.AvailableCapital < 0)
                return false;

            return baseData.AvailableCapital + delta.AvailableCapitalDelta >= 0;
        }

        /// <summary>
        /// 一个持仓id多线程插入(带检查)
        /// </summary>
        public static void Test222()
        {
            MemoryDataManager.Start();

            XHHoldMemoryTable table = MemoryDataManager.XHHoldMemoryList.GetByAccountHoldLogoId(1375);
            for (int i = 0; i < 5; i++)
            {
                Thread t = new Thread(Target33);
                TableObj<XHHoldMemoryTable> obj = new TableObj<XHHoldMemoryTable>();
                obj.Table = table;
                obj.val = 200;
                t.Start(obj);
            }

            ExitConsole();
            MemoryDataManager.End();
        }

        public static void Target33(object obj)
        {
            TableObj<XHHoldMemoryTable> tableObj = obj as TableObj<XHHoldMemoryTable>;
            if (tableObj == null)
                return;

            //ShowMessage(0, tableObj.val, 0);
            int val = tableObj.val;
            var table = tableObj.Table;//.AddDelta(val, val, val, val);
            var data = table.Data;

            XH_AccountHoldTableInfo_Delta delta = new XH_AccountHoldTableInfo_Delta();
            delta.AccountHoldLogoId = data.AccountHoldLogoId;
            delta.AvailableAmountDelta = -val;
            delta.Data = data;

            bool isSuccess = false;
            bool isCapitalSuccess = false;

            Database database = DatabaseFactory.CreateDatabase();

            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    isCapitalSuccess = table.CheckAndAddDelta(func2, delta, database, transaction);
                    if (!isCapitalSuccess)
                    {
                        Console.WriteLine("----------holdfailure");
                        Console.WriteLine("holdfailure data=" + table.Data.AvailableAmount);

                        throw new Exception();
                    }

                    transaction.Commit();
                    isSuccess = true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    isSuccess = false;
                    Console.WriteLine("事务失败！");
                }
            }

            if (!isSuccess)
            {
                if (isCapitalSuccess)
                {
                    table.RollBackMemory(delta);
                    Console.WriteLine("rollback data=" + table.Data.AvailableAmount);
                }
            }
        }

        private static bool func2(XH_AccountHoldTableInfo hold, XH_AccountHoldTableInfo_Delta change)
        {
            if(hold.AvailableAmount <=0)
                return false;

            if(hold.AvailableAmount + change.AvailableAmountDelta <0)
                return false;

            return true;
        }


        /// <summary>
        /// 多个资金id多次插入
        /// </summary>
        private static void Test3()
        {
            MemoryDataManager.Start();
            var table1 = MemoryDataManager.XHCapitalMemoryList.GetByCapitalAccountLogo(1);
            var table2 = MemoryDataManager.XHCapitalMemoryList.GetByCapitalAccountLogo(2);
            var table3 = MemoryDataManager.XHCapitalMemoryList.GetByCapitalAccountLogo(3);

            Console.WriteLine("开始Test3");
            //table1.AddDelta(1, 1, 1, 1);
            //table1.AddDelta(-1, -1, -1, -1);
            //table1.AddDelta(1, 1, 1, 1);

            //table2.AddDelta(2, 2, 2, 2);
            //table2.AddDelta(-2, -2, -2, -2);
            //table2.AddDelta(2, 2, 2, 2);

            //table3.AddDelta(3, 3, 3, 3);
            //table3.AddDelta(-3, -3, -3, -3);
            //table3.AddDelta(3, 3, 3, 3);

            ExitConsole();
            MemoryDataManager.End();
        }

        /// <summary>
        /// 多个资金id多线程插入
        /// </summary>
        public static void Test4()
        {
            Console.WriteLine("开始Test4");

            int sum = 0;
            for (int i = 0; i < 1000; i++)
            {
                sum += (i + 1);
            }

            MemoryDataManager.Start();
            var table1 = MemoryDataManager.XHCapitalMemoryList.GetByCapitalAccountLogo(1);
            var table2 = MemoryDataManager.XHCapitalMemoryList.GetByCapitalAccountLogo(2);
            var table3 = MemoryDataManager.XHCapitalMemoryList.GetByCapitalAccountLogo(3);

            Thread t1 = new Thread(RunTableThread);
            t1.Start(table1);

            Thread t2 = new Thread(RunTableThread);
            t2.Start(table2);

            Thread t3 = new Thread(RunTableThread);
            t3.Start(table3);

            Console.WriteLine("总值：{0}", sum);
            ExitConsole();
            MemoryDataManager.End();
        }

        public static void TestHold1()
        {
            Console.WriteLine("开始TestHold1");
            MemoryDataManager.Start();
            var table1 = MemoryDataManager.XHHoldMemoryList.GetByAccountHoldLogoId(1);
            var hold = table1.Data;

            ExitConsole();
            MemoryDataManager.End();
        }

        public static void TestQHHold1()
        {
            Console.WriteLine("开始TestQHHold1");
            MemoryDataManager.Start();
            var table1 = MemoryDataManager.QHHoldMemoryList.GetByAccountHoldLogoId(1);
            var hold = table1.Data;
            Console.WriteLine(hold.Contract);
            ExitConsole();
            MemoryDataManager.End();
        }

        public static void TestHold11()
        {
           Console.WriteLine("开始TestHold11");
            MemoryDataManager.Start();
            var table1 = MemoryDataManager.XHHoldMemoryList.GetByHoldAccountAndCurrencyType("010000000403", "000002", 2);
            var hold = table1.Data;
            Console.WriteLine(hold.AvailableAmount);

            ExitConsole();
            MemoryDataManager.End(); 
        }

        public static void TestHold2()
        {
            Console.WriteLine("开始TestHold2");
            MemoryDataManager.Start();
            var table1 = MemoryDataManager.XHHoldMemoryList.GetByAccountHoldLogoId(1);

            table1.ReadAndWrite(hold =>
                                    {
                                        hold.AvailableAmount += 1;
                                        hold.BreakevenPrice += 1;
                                        hold.CostPrice += 1;
                                        hold.FreezeAmount += 1;
                                        hold.HoldAveragePrice += 1;

                                        return hold;
                                    });

            ExitConsole();
            MemoryDataManager.End();
        }

        public static void TestQHHold2()
        {
            Console.WriteLine("开始TestQHHold2");
            MemoryDataManager.Start();
            var table1 = MemoryDataManager.QHHoldMemoryList.GetByAccountHoldLogoId(1);

            table1.ReadAndWrite(hold =>
            {
                hold.CostPrice = 1;

                return hold;
            });

            ExitConsole();
            MemoryDataManager.End();
        }

        public static void TestHold3()
        {
            Console.WriteLine("开始TestHold3");
            MemoryDataManager.Start();

            var table1 = MemoryDataManager.XHHoldMemoryList.GetByAccountHoldLogoId(1);
            Thread t = new Thread(RunHoldTableThread);
            TableObj<XHHoldMemoryTable> obj1 = new TableObj<XHHoldMemoryTable>();
            obj1.Table = table1;
            t.Start(obj1);

            ExitConsole();
            MemoryDataManager.End();
        }


        public static void TestHold4()
        {
            Console.WriteLine("开始TestHold4");
            MemoryDataManager.Start();

            var table1 = MemoryDataManager.XHHoldMemoryList.GetByAccountHoldLogoId(1);
            TableObj<XHHoldMemoryTable> obj1 = new TableObj<XHHoldMemoryTable>();
            obj1.Table = table1;
            Thread t = new Thread(RunHoldTableThread);
            t.Start(obj1);

            var table2 = MemoryDataManager.XHHoldMemoryList.GetByAccountHoldLogoId(2);
            TableObj<XHHoldMemoryTable> obj2 = new TableObj<XHHoldMemoryTable>();
            obj2.Table = table2;
            Thread t2 = new Thread(RunHoldTableThread);
            t2.Start(obj2);

            ExitConsole();
            MemoryDataManager.End();
        }

        private static void RunHoldTableThread(object tobj)
        {
            var tableObj = tobj as TableObj<XHHoldMemoryTable>;
            var table = tableObj.Table;
            
            if (table == null)
                return;

            for (int i = 0; i < 1000; i++)
            {
                Thread t = new Thread(DoHoldAction);
                TableObj<XHHoldMemoryTable> obj = new TableObj<XHHoldMemoryTable>();
                obj.Table = table;
                obj.val = i + 1;
                t.Start(obj);

                var data = table.Data;
                Console.WriteLine(data.AccountHoldLogoId + "Read" +  + (i+1) + ":" + data.AvailableAmount);
            }

            Console.WriteLine("插入结束！");
        }

        private static void DoHoldAction(object obj)
        {
            TableObj<XHHoldMemoryTable> tableObj = obj as TableObj<XHHoldMemoryTable>;
            var table1 = tableObj.Table;
            int i = tableObj.val;

            table1.ReadAndWrite(hold =>
            {
                hold.AvailableAmount += i;
                hold.BreakevenPrice += i;
                hold.CostPrice += i;
                hold.FreezeAmount += i;
                hold.HoldAveragePrice += i;

                Console.WriteLine(hold.AccountHoldLogoId + "Write" + i);

                return hold;
            });
        }

        private static void TestAccountManager()
        {
            //AccountManager accountManager = AccountManager.Instance;

            //accountManager.Reset();

            //string userID = "23";

            //var list1 = accountManager.GetUserAllAccounts(userID);

            //string capAccount = "010000002302";
            //string holdAccount = "010000002303";

            //var user1 = accountManager.GetUserByAccount(capAccount);
            //var user2 = accountManager.GetUserByAccount(holdAccount);

            //var user3 = accountManager.GetCapitalAccountByHoldAccount(holdAccount);
            //var user33 = accountManager.GetCapitalAccountByHoldAccount(capAccount);

            //var user4 = accountManager.GetHoldAccountByCapitalAccount(capAccount);
            //var user44 = accountManager.GetHoldAccountByCapitalAccount(holdAccount);

            //var userList= accountManager.GetAccountByUserIDAndAccountTypeClass(userID,
            //                                                                     CommonObject.Types.
            //                                                                         AccountAttributionType.SpotHold);
        }
    }

    public class TableObj<TMemoryTable>
    {
        public TMemoryTable Table;
        public int val;
    }
}
