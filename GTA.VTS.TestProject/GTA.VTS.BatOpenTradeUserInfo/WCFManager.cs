using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTA.VTS.BatOpenTradeUserInfo.HKTradingRulesService;
using GTA.VTS.BatOpenTradeUserInfo.TransactionManagerService;
using MatchCenter.DAL;
using GTA.VTS.BatOpenTradeUserInfo.CommonParaService;
using GTA.VTS.BatOpenTradeUserInfo.SpotTradeRulesService;
using System.Configuration;
using System.Windows.Forms;
using System.Threading;

namespace GTA.VTS.BatOpenTradeUserInfo
{
    /// <summary>
    /// 功能管理类
    /// </summary>
    public class WCFManager
    {
        /// <summary>
        /// 线程数
        /// </summary>
        private static int threadcount = 0;
        /// <summary>
        /// 显示状态信息label
        /// </summary>
        public static Label lableMsg;
        /// <summary>
        /// 按
        /// </summary>
        public static Button btnOpen;
        /// <summary>
        /// 时间记录
        /// </summary>
        public static DateTime time;
        /// <summary>
        /// 锁对象
        /// </summary>
        public static object obj = new object();
        /// <summary>
        /// 添加线程数
        /// </summary>
        public static void AddThread()
        {
            lock (obj)
            {
                threadcount += 1;
            }
        }
        /// <summary>
        /// 减少线程数
        /// </summary>
        public static void SubThread()
        {
            lock (obj)
            {
                threadcount -= 1;
            }

        }
        /// <summary>
        /// 返回总线程数
        /// </summary>
        /// <returns></returns>
        public static int CountThread()
        {

            lock (obj)
            {
                int k = 0;
                k = threadcount;
                return k;
            }
        }
        /// <summary>
        /// 获取连接管理中心开户WCF连接实例
        /// </summary>
        /// <returns></returns>
        public static TransactionManageClient GetManageClient()
        {
            TransactionManageClient client = null;
            try
            {
                client = new TransactionManageClient();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return client;
        }
        /// <summary>
        /// 获取连接管理中心开户WCF连接实例
        /// </summary>
        /// <returns></returns>
        public static CommonParaClient GetCommonParaClient()
        {
            CommonParaClient client = null;
            try
            {
                client = new CommonParaClient();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return client;
        }
        /// <summary>
        /// 获取现货交易规则
        /// </summary>
        /// <returns></returns>
        public static SpotTradeRulesClient GetSpotTradeRulesClient()
        {
            SpotTradeRulesClient client = null;
            try
            {
                client = new SpotTradeRulesClient();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return client;
        }

        /// <summary>
        /// 获取港股交易规则
        /// </summary>
        /// <returns></returns>
        public static HKTradeRulesClient GetHKTradeRulesClient()
        {
            HKTradeRulesClient hkTradeRulesClient = null;
            try
            {
                hkTradeRulesClient = new HKTradeRulesClient();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return hkTradeRulesClient;
        }
        /// <summary>
        /// 把用户修改为后台交易员和密码默认为888888
        /// </summary>
        /// <param name="useridStr"></param>
        /// <returns></returns>
        public static bool UpdateAddType(string useridStr)
        {
            try
            {
                string config = ConfigurationManager.AppSettings["ManagerConnection"].ToString();
                string strSql = "update  [UM_UserInfo] set Password='XIVqM2FELUw=',AddType=1 where  UserID in ({0})";
                SqlHelper.ExecuteNoneQuery(config, string.Format(strSql, useridStr));

                strSql = " UPDATE [dbo].[UA_UserBasicInformationTable]  SET [Password] = 'XIVqM2FELUw='   UserID in ({0})";
                SqlHelper.ExecuteSql(string.Format(strSql, useridStr));

                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 开始批量开户并处理初始化资金
        /// </summary>
        /// <param name="capital">初始化资金</param>
        /// <param name="amount">批开户数</param>
        /// <returns></returns>
        public static bool StartIni(decimal capital, int amount, bool isInitHold)
        {
            time = DateTime.Now;
            string message = "";
            //WCFManager.GetAllCM_Commodity();
            //WCFManager.GetAllHKCommodity();
            List<UM_UserInfo> list = new List<UM_UserInfo>();
            using (TransactionManageClient client = GetManageClient())
            {
                lableMsg.Text = "正在调用管理中心开始开户请稍后.......";

                #region 管理中心开户
                InitFund inifund = new InitFund();
                inifund.HK = capital;
                inifund.RMB = capital;
                inifund.US = capital;
                //list = client.BatchAddTransaction(out message, amount, inifund, "FrontManager", "fzjQ37ynndk=");
                //只开后台交易员用户(因验证前台管理员身份)
                list = client.BatchAddTransaction(out message, amount, inifund, "FrontManager", "fzjQ37ynndk=");
                if (!string.IsNullOrEmpty(message))
                {
                    lableMsg.Text = message; //"调用管理中心开始开户完成.......";
                }
                else
                {
                    lableMsg.Text = "调用管理中心开始开户完成.......";
                }
            }
                #endregion
            string userIDsStr = "";
            StringBuilder userIDs = new StringBuilder("");
            #region  更新柜台资金可用资金
            lableMsg.Text = "开始调用柜台数据库初始化可用资金.......";
            foreach (var item in list)
            {
                userIDs.Append(" ,  '" + item.UserID.ToString() + "'");
                WCFManager.InsertCapitalAccountInfo(capital, item.UserID.ToString());
            }

            lableMsg.Text = "柜台初始化可用资金完成.......";
            #endregion
            #region 更新所有用户添加类型和初始密码为888888
            userIDsStr = userIDs.ToString();
            lableMsg.Text = "更新管理中心添加交易员类型更为后台交易员和把密码初始化为888888并更新柜台交易员密码.......";
            if (!string.IsNullOrEmpty(userIDsStr))
            {
                userIDsStr = userIDsStr.Substring(userIDsStr.IndexOf(',') + 1);
                WCFManager.UpdateAddType(userIDsStr);
            }

            lableMsg.Text = "完成密码更新.......";

            #endregion

            #region  初始化持仓表
            //if (isInitHold)
            //{
            //    lableMsg.Text = "正在调用柜台初始化持仓量(线程开始开启分发).......";
            //    foreach (var item in list)
            //    {
            //        Thread th = new Thread(delegate() { WCFManager.InsertHoldAccount(item.UserID.ToString()); });
            //        th.Start();
            //        WCFManager.AddThread();
            //        th.Join(100);

            //        //WCFManager.InsertHoldAccount(item.UserID.ToString());
            //    }
            //    lableMsg.Text = "正在调用柜台初始化持仓量(线程开启完)各线程正在执行初始中请稍后.......";
            //}
            #endregion
            return true;
        }

        public static bool InsertCapitalAccountInfo(decimal AvailableCapital, string userid)
        {
            try
            {


                StringBuilder strSql = new StringBuilder();
                //strSql.Append(" declare @useraccount varchar(20)");
                //strSql.AppendFormat(" set @useraccount=(select UserAccountDistributeLogo from [UA_UserAccountAllocationTable]   where userid='{0}' and accounttypeLogo=2)", userid);
                //strSql.Append(" insert into XH_CapitalAccountTable(AvailableCapital,UserAccountDistributeLogo,");
                //strSql.Append("BalanceOfTheDay,TodayOutInCapital,FreezeCapitalTotal,");
                //strSql.Append("TradeCurrencyType,HasDoneProfitLossTotal ");
                //for (int i = 1; i < 4; i++)
                //{
                //    strSql.AppendFormat(") values ({0},@useraccount, 0, 0, 0,{1},0)", AvailableCapital, i);
                //    SqlHelper.ExecuteSql(strSql.ToString());
                //}
                //现货
                strSql.Append(" declare @useraccount varchar(20)");
                strSql.AppendFormat(" set @useraccount=(select UserAccountDistributeLogo from [UA_UserAccountAllocationTable]   where userid='{0}' and accounttypeLogo=2)", userid);

                strSql.AppendFormat(" Update XH_CapitalAccountTable set  AvailableCapital={0} where UserAccountDistributeLogo=@useraccount and ", AvailableCapital);
                strSql.Append(" TradeCurrencyType={0}");
                //期货(股指期货)
                strSql.AppendFormat(" set @useraccount=(select UserAccountDistributeLogo from [UA_UserAccountAllocationTable]   where userid='{0}' and accounttypeLogo=6)", userid);
                strSql.AppendFormat(" Update QH_CapitalAccountTable set  AvailableCapital={0} where UserAccountDistributeLogo=@useraccount and ", AvailableCapital);
                strSql.Append(" TradeCurrencyType={1}");
                //期货(商品期货)
                strSql.AppendFormat(" set @useraccount=(select UserAccountDistributeLogo from [UA_UserAccountAllocationTable]   where userid='{0}' and accounttypeLogo=4)", userid);
                strSql.AppendFormat(" Update QH_CapitalAccountTable set  AvailableCapital={0} where UserAccountDistributeLogo=@useraccount and ", AvailableCapital);
                strSql.Append(" TradeCurrencyType={1}");

                //港股
                strSql.AppendFormat(" set @useraccount=(select UserAccountDistributeLogo from [UA_UserAccountAllocationTable]   where userid='{0}' and accounttypeLogo=8)", userid);
                strSql.AppendFormat(" Update HK_CapitalAccount set  AvailableCapital={0} where UserAccountDistributeLogo=@useraccount and ", AvailableCapital);
                strSql.Append(" TradeCurrencyType={2}");

                for (int i = 1; i < 4; i++)
                {
                    SqlHelper.ExecuteSql(string.Format(strSql.ToString(), i, i, i));
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
        /// <summary>
        /// 现货代码列表
        /// </summary>
        public static List<CM_Commodity> list = new List<CM_Commodity>();
        /// <summary>
        ///现货费用
        /// </summary>
        public static List<XH_SpotCosts> spotList = new List<XH_SpotCosts>();

        /// <summary>
        /// 港股代码列表
        /// </summary>
        public static List<HK_Commodity> hKCommodityList = new List<HK_Commodity>();

        /// <summary>
        /// 港股费用
        /// </summary>
        public static List<HK_SpotCosts> hKSpotCostsList = new List<HK_SpotCosts>();

        /// <summary>
        /// 获取所有的现货代码
        /// </summary>
        public static void GetAllCM_Commodity()
        {
            list.Clear();
            spotList.Clear();
            int[] breeid = { 7, 8, 21, 22 };
            using (CommonParaClient client = GetCommonParaClient())
            {
                foreach (var k in breeid)
                {
                    List<CM_Commodity> item = new List<CM_Commodity>();
                    item = client.GetCommodityByBreedClassID(k);

                    list.AddRange(item);
                }
            }
            using (SpotTradeRulesClient client = GetSpotTradeRulesClient())
            {
                spotList = client.GetAllSpotCosts();
            }
        }

        public static int GetCurrencyType(int breeid)
        {
            int type = 0;
            foreach (var item in spotList)
            {
                if (item.BreedClassID == breeid)
                {
                    type = (int)item.CurrencyTypeID;
                    break;
                }
            }
            return type;
        }

        /// <summary>
        /// 获取所有的港股代码
        /// </summary>
        public static void GetAllHKCommodity()
        {
            hKCommodityList.Clear();
            hKCommodityList.Clear();
            //int[] breeid = { 44 };
            //int breeid = 44;//港股品种ID
            using (HKTradeRulesClient client = GetHKTradeRulesClient())
            {
                List<HK_Commodity> item = new List<HK_Commodity>();
                item = client.GetAllHKCommodity();
                hKCommodityList.AddRange(item);
            }
            using (HKTradeRulesClient client = GetHKTradeRulesClient())
            {
                hKSpotCostsList = client.GetAllHKSpotCosts();
            }
        }

        /// <summary>
        /// 根据ID返回港股品种币种类型
        /// </summary>
        /// <param name="breeid">品种ID</param>
        /// <returns></returns>
        public static int GetHKCurrencyType(int breeid)
        {
            int type = 0;
            foreach (var item in hKSpotCostsList)
            {
                if (item.BreedClassID == breeid)
                {
                    type = (int)item.CurrencyTypeID;
                    break;
                }
            }
            return type;
        }

        /// <summary>
        /// 根据用户ID初始化持仓
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        public static bool InsertHoldAccount(string userid)
        {
            lableMsg.Text = "正在初始化用户" + userid + "持仓量(线程开始).......";
            string sqlAccount = "select UserAccountDistributeLogo from [UA_UserAccountAllocationTable]   where userid='" + userid + "' and accounttypeLogo=3";
            string account = (string)SqlHelper.ExecuteScalar(sqlAccount);
            foreach (var item in list)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into XH_AccountHoldTable(");
                strSql.Append("UserAccountDistributeLogo,CurrencyTypeId,Code,AvailableAmount,FreezeAmount,CostPrice,BreakevenPrice,HoldAveragePrice)");
                strSql.Append(" values ('{0}',{1},'{2}',{3},0,0,0,0)");
                int currencyType = GetCurrencyType((int)item.BreedClassID);
                try
                {
                    long value = (long)item.turnovervolume / 3;
                    if (value > 100000000)
                        value = long.Parse(value.ToString().Substring(0, 8));
                    string execSql = string.Format(strSql.ToString(), account, currencyType, item.CommodityCode, value);
                    SqlHelper.ExecuteSql(execSql);
                    lableMsg.Text = "初始化用户" + userid + "的" + item.CommodityCode + "持仓量完成.......";
                }
                catch
                {

                }
            }
            //港股
            string sqlAccount1 = "select UserAccountDistributeLogo from [UA_UserAccountAllocationTable]   where userid='" + userid + "' and accounttypeLogo=9";
            string account1 = (string)SqlHelper.ExecuteScalar(sqlAccount1);
            foreach (var item in hKCommodityList)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into HK_AccountHold(");
                strSql.Append("UserAccountDistributeLogo,CurrencyTypeId,Code,AvailableAmount,FreezeAmount,CostPrice,BreakevenPrice,HoldAveragePrice)");
                strSql.Append(" values ('{0}',{1},'{2}',{3},0,0,0,0)");
                int currencyType = 2;//港股币种类型是2 //GetHKCurrencyType((int)item.BreedClassID);
                try
                {
                    long value = (long)item.turnovervolume / 3;
                    if (value > 100000000)
                        value = long.Parse(value.ToString().Substring(0, 8));
                    string execSql = string.Format(strSql.ToString(), account1, currencyType, item.HKCommodityCode, value);
                    SqlHelper.ExecuteSql(execSql);
                    lableMsg.Text = "初始化用户" + userid + "的" + item.HKCommodityCode + "持仓量完成.......";
                }
                catch
                {

                }
            }

            SubThread();
            if (CountThread() <= 0)
            {
                TimeSpan span = DateTime.Now - time;
                lableMsg.Text = "全部已经初始化完,用时：" + span.TotalSeconds + "秒";
                btnOpen.Enabled = true;
            }
            return true;
        }

        public static void testcurrcy()
        {
            CommonParaClient newobj = new CommonParaClient();
            CM_CurrencyBreedClassType tt = newobj.GetCurrencyByBreedClassID(7);
 
        }

        public static void ShowMesg(string msg)
        {
            Thread th = new Thread(delegate() { lableMsg.Text = msg; });
            th.Start();
        }
        //删除记录脚本
        //        --delete  dbo.xh_AccountHoldtable where UserAccountDistributeLogo<>'010000000403'
        //--delete  dbo.qh_capitalAccountTable where UserAccountDistributeLogo<>'010000000406'
        //--delete  dbo.[HK_CapitalAccount] where UserAccountDistributeLogo<>'010000000408'
        //--delete  dbo.xh_capitalAccountTable where UserAccountDistributeLogo<>'010000000402'
        //--delete UA_BankAccountTable where UserAccountDistributeLogo<>'010000000401'
        //--delete dbo.UA_UserAccountAllocationTable where UserID<>4 and UserID<>5
        //--  delete dbo.UA_UserBasicInformationTable where UserID<>4 and UserID<>5
    }
}
