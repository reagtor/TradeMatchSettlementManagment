#region Using Namespace

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using GTA.VTS.CustomersOrders.DoAccountManager;
using GTA.VTS.CustomersOrders.DoCommonQuery;
using GTA.VTS.CustomersOrders.HKCommonQuery;
using Timer = System.Timers.Timer;
using GTA.VTS.CustomersOrders.DoDealRptService;
using GTA.VTS.CustomersOrders.DoOrderService;
using GTA.VTS.CustomersOrders.TransactionManageService;
using Amib.Threading;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using GTA.VTS.CustomersOrders.BLL;
using System.Text.RegularExpressions;
using TypesOrderPriceType = GTA.VTS.CustomersOrders.DoOrderService.TypesOrderPriceType;
using System.Data.OleDb;
using System.Data;
using System.ComponentModel;

#endregion

namespace GTA.VTS.CustomersOrders.AppForm
{
    /// <summary>
    /// Tilte;测试工具主窗体
    /// Create BY：李健华
    /// Create date:2009-12-21
    /// </summary>
    public partial class MainForm : Form
    {
        #region 变量定义

        private string gzqhAccount = "";
        private int gzqhDoOrderNum;
        private int spqhDoOrderNum;
        private int gzqhIndex;


        private string hkAccount = "";
        private int hkDoOrderNum;
        private int hkIndex;

        private bool isClearGZQH;
        private bool isClearHK;
        private bool isClearXH;
        private bool isClearSPQH;
        private bool isProcessing;
        private bool isReBindGZQHData;
        private bool isReBindSPQHData;
        private bool isReBindHKData;
        private bool isReBindXHData;
        private bool loadCodeSuccess;
        private SmartThreadPool smartPool = new SmartThreadPool { MaxThreads = 200, MinThreads = 25 };
        private string spqhAccount = "";

        private string title = "";
        private string traderId = "";
        internal WCFServices wcfLogic;
        private string xhAccount = "";

        private int xhDoOrderNum;
        private int xhIndex;

        private GZQHMessageLogic gzqhLogic = new GZQHMessageLogic();
        private SPQHMessageLogic spqhLogic = new SPQHMessageLogic();
        private HKMessageLogic hkLogic = new HKMessageLogic();
        private XHMessageLogic xhLogic = new XHMessageLogic();
        private OrderSQLHelper orderSql = new OrderSQLHelper();
        private Timer timer = new Timer();
        private Timer gzqhTimer = new Timer();
        private Timer hkTimer = new Timer();
        private Timer xhTimer = new Timer();
        private Timer spqhTimer = new Timer();
        /// <summary>
        /// 列表每页记录数
        /// </summary>
        private int pageSize = 20;

        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

        }

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化WCF连接
        /// </summary>
        /// <param name="tradeID">交易员ID</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns>是否成功</returns>
        public bool Initialize(string tradeID, ref string errMsg)
        {

            try
            {
                //wcfLogic = new WCFServices();
                wcfLogic = WCFServices.Instance;
                string errorMsg = "";
                bool isSuccess = wcfLogic.Initialize(traderId, txtChannelID.Text.Trim(), out errorMsg);
                if (!isSuccess)
                {

                    errMsg = ResourceOperate.Instanse.GetResourceByKey("WCF");
                    LogHelper.WriteDebug(errMsg);
                    return false;
                }

                string channleid = ServerConfig.ChannelID;
                string title = " [ChannelID: " + channleid + "]";
                StatusLable.Text = "Version:" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + "    " + StatusLable.Text;
                this.Text += title;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                LogHelper.WriteError(ex.Message, ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 刷新显示相关语方类型的标签
        /// </summary>
        private void LocalhostResourcesFormText()
        {
            #region 多语言显示
            #region 现货语言类型显示
            this.lblCode.Text = ResourceOperate.Instanse.GetResourceByKey("lblCode");
            this.lblAmount.Text = ResourceOperate.Instanse.GetResourceByKey("Amount");
            this.lblTradeID.Text = ResourceOperate.Instanse.GetResourceByKey("lblTradeID");
            this.lblCapital.Text = ResourceOperate.Instanse.GetResourceByKey("lblCapital");
            this.lblPrice.Text = ResourceOperate.Instanse.GetResourceByKey("Price");
            this.lblBatch.Text = ResourceOperate.Instanse.GetResourceByKey("lblBatch");
            this.lblHigh.Text = ResourceOperate.Instanse.GetResourceByKey("lblHigh");
            this.lblLow.Text = ResourceOperate.Instanse.GetResourceByKey("lblLow");
            this.lblBuySell.Text = ResourceOperate.Instanse.GetResourceByKey("lblBuySell");
            this.lblUnit.Text = ResourceOperate.Instanse.GetResourceByKey("lblUnit");
            this.cbMarketOrder.Text = ResourceOperate.Instanse.GetResourceByKey("cbMarketOrder");
            this.lblMax.Text = ResourceOperate.Instanse.GetResourceByKey("lblMax");
            this.lblACapital.Text = ResourceOperate.Instanse.GetResourceByKey("lblACapital");
            this.lblFCapital.Text = ResourceOperate.Instanse.GetResourceByKey("lblFCapital");
            this.lblBDay.Text = ResourceOperate.Instanse.GetResourceByKey("lblBDay");
            this.lblHLossTotal.Text = ResourceOperate.Instanse.GetResourceByKey("lblHLossTotal");
            this.lblTCapital.Text = ResourceOperate.Instanse.GetResourceByKey("lblTCapital");
            this.btnDoOrder.Text = ResourceOperate.Instanse.GetResourceByKey("DoOrder");

            this.gpbDoOrder.Text = ResourceOperate.Instanse.GetResourceByKey("gpbDoOrder");
            this.gpgPushBack.Text = ResourceOperate.Instanse.GetResourceByKey("gpgPushBack");
            this.tabPageXH.Text = ResourceOperate.Instanse.GetResourceByKey("tabPageXH");
            this.tabPageHK.Text = ResourceOperate.Instanse.GetResourceByKey("tabPageHK");
            this.tabPageGZQH.Text = ResourceOperate.Instanse.GetResourceByKey("tabPageGZQH");
            this.tabPageQuery.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.tabPageConnection.Text = ResourceOperate.Instanse.GetResourceByKey("tabPageConnection");
            this.tabPageAdminTest.Text = ResourceOperate.Instanse.GetResourceByKey("tabPageAdminTest");
            this.tabPageGrid.Text = ResourceOperate.Instanse.GetResourceByKey("tabPageGrid");
            this.tabPageList.Text = ResourceOperate.Instanse.GetResourceByKey("tabPageList");
            this.tabPageSPQH.Text = ResourceOperate.Instanse.GetResourceByKey("tabPageSPQH");
            this.Text = ResourceOperate.Instanse.GetResourceByKey("UserFront");
            this.lblPath.Text = ResourceOperate.Instanse.GetResourceByKey("Path");
            this.btnBatchSendOrder.Text = ResourceOperate.Instanse.GetResourceByKey("BatchOrder");
            this.lblXHTime.Text = ResourceOperate.Instanse.GetResourceByKey("Time");
            #region 现货行情信息显示
            this.lblXHBuyFirstPrice.Text = ResourceOperate.Instanse.GetResourceByKey("FirstPrice");
            this.lblXHBuySecondPrice.Text = ResourceOperate.Instanse.GetResourceByKey("SecondPrice");
            this.lblXHBuyThirdPrice.Text = ResourceOperate.Instanse.GetResourceByKey("ThirdPrice");
            this.lblXHBuyFourthPrice.Text = ResourceOperate.Instanse.GetResourceByKey("FourthPrice");
            this.lblXHBuyFivePrice.Text = ResourceOperate.Instanse.GetResourceByKey("FivePrice");
            this.lblXHSellFirstPrice.Text = ResourceOperate.Instanse.GetResourceByKey("FirstPrice");
            this.lblXHSellSecondPrice.Text = ResourceOperate.Instanse.GetResourceByKey("SecondPrice");
            this.lblXHSellThirdPrice.Text = ResourceOperate.Instanse.GetResourceByKey("ThirdPrice");
            this.lblXHSellFourthPrice.Text = ResourceOperate.Instanse.GetResourceByKey("FourthPrice");
            this.lblXHSellFivePrice.Text = ResourceOperate.Instanse.GetResourceByKey("FivePrice");
            this.lblXHSell.Text = ResourceOperate.Instanse.GetResourceByKey("Sell");
            this.lblXHBuy.Text = ResourceOperate.Instanse.GetResourceByKey("Buy");
            this.lblXHLastPrice.Text = ResourceOperate.Instanse.GetResourceByKey("LastPrice");
            this.lblXHLastVolume.Text = ResourceOperate.Instanse.GetResourceByKey("LastVolume");
            this.lblXHLowerPrice.Text = ResourceOperate.Instanse.GetResourceByKey("LowerPrice");
            this.lblXHUpPrice.Text = ResourceOperate.Instanse.GetResourceByKey("UpPrice");
            this.lblXHYesterPrice.Text = ResourceOperate.Instanse.GetResourceByKey("YesterPrice");
            this.lblXHName.Text = ResourceOperate.Instanse.GetResourceByKey("Name");
            this.gpXHmarket.Text = ResourceOperate.Instanse.GetResourceByKey("market");
            #endregion 现货行情信息显示
            #endregion 现货语言类型显示
            #region 港股语言类型显示
            this.lblHKCode.Text = ResourceOperate.Instanse.GetResourceByKey("lblCode");
            this.lblHKAmount.Text = ResourceOperate.Instanse.GetResourceByKey("Amount");
            this.lblHKTradeID.Text = ResourceOperate.Instanse.GetResourceByKey("lblTradeID");
            this.lblHKCapital.Text = ResourceOperate.Instanse.GetResourceByKey("lblCapital");
            this.lblHKPrice.Text = ResourceOperate.Instanse.GetResourceByKey("Price");
            this.lblHKBatch.Text = ResourceOperate.Instanse.GetResourceByKey("lblBatch");
            this.lblHKHigh.Text = ResourceOperate.Instanse.GetResourceByKey("lblHigh");
            this.lblHKLow.Text = ResourceOperate.Instanse.GetResourceByKey("lblLow");
            this.lblHKBuySell.Text = ResourceOperate.Instanse.GetResourceByKey("lblBuySell");
            this.lblHKUnit.Text = ResourceOperate.Instanse.GetResourceByKey("lblUnit");
            this.lblHKMax.Text = ResourceOperate.Instanse.GetResourceByKey("lblMax");
            this.lblHKACapital.Text = ResourceOperate.Instanse.GetResourceByKey("lblACapital");
            this.lblHKFCapital.Text = ResourceOperate.Instanse.GetResourceByKey("lblFCapital");
            this.lblHKBDay.Text = ResourceOperate.Instanse.GetResourceByKey("lblBDay");
            this.lblHKHLossTotal.Text = ResourceOperate.Instanse.GetResourceByKey("lblHLossTotal");
            this.lblHKTCapital.Text = ResourceOperate.Instanse.GetResourceByKey("lblTCapital");
            this.btnHKSendOrder.Text = ResourceOperate.Instanse.GetResourceByKey("DoOrder");
            this.gpgHKDoOrder.Text = ResourceOperate.Instanse.GetResourceByKey("gpbDoOrder");
            this.gpgHKPushBack.Text = ResourceOperate.Instanse.GetResourceByKey("gpgPushBack");
            this.tabPageHKGrid.Text = ResourceOperate.Instanse.GetResourceByKey("tabPageGrid");
            this.tabPageHKList.Text = ResourceOperate.Instanse.GetResourceByKey("tabPageList");
            this.lblHKPriceType.Text = ResourceOperate.Instanse.GetResourceByKey("lblHKPriceType");
            this.lblHKPath.Text = ResourceOperate.Instanse.GetResourceByKey("Path");
            this.btnHKBatchOrder.Text = ResourceOperate.Instanse.GetResourceByKey("BatchOrder");
            this.lblHKTime.Text = ResourceOperate.Instanse.GetResourceByKey("Time");
            #region 港股行情信息显示
            this.lblHKBuyFirstVolume.Text = ResourceOperate.Instanse.GetResourceByKey("FirstPrice");
            this.lblHKBuySecondVolume.Text = ResourceOperate.Instanse.GetResourceByKey("SecondPrice");
            this.lblHKBuyThirdVolume.Text = ResourceOperate.Instanse.GetResourceByKey("ThirdPrice");
            this.lblHKBuyFourthVolume.Text = ResourceOperate.Instanse.GetResourceByKey("FourthPrice");
            this.lblHKBuyFiveVolume.Text = ResourceOperate.Instanse.GetResourceByKey("FivePrice");
            this.lblHKSellFirstPrice.Text = ResourceOperate.Instanse.GetResourceByKey("FirstPrice");
            this.lblHKSellSecondPrice.Text = ResourceOperate.Instanse.GetResourceByKey("SecondPrice");
            this.lblHKSellThirdPrice.Text = ResourceOperate.Instanse.GetResourceByKey("ThirdPrice");
            this.lblHKSellFourthPrice.Text = ResourceOperate.Instanse.GetResourceByKey("FourthPrice");
            this.lblHKSellFivePrice.Text = ResourceOperate.Instanse.GetResourceByKey("FivePrice");
            this.lblHKSell.Text = ResourceOperate.Instanse.GetResourceByKey("Sell");
            this.lblHKBuy.Text = ResourceOperate.Instanse.GetResourceByKey("Buy");
            this.lblHKLastPrice.Text = ResourceOperate.Instanse.GetResourceByKey("LastPrice");
            this.lblHKLastVolume.Text = ResourceOperate.Instanse.GetResourceByKey("LastVolume");
            this.lblHKLowerPrice.Text = ResourceOperate.Instanse.GetResourceByKey("LowerPrice");
            this.lblHKUpPrice.Text = ResourceOperate.Instanse.GetResourceByKey("UpPrice");
            this.lblHKYesterPrice.Text = ResourceOperate.Instanse.GetResourceByKey("YesterPrice");
            this.lblHKName.Text = ResourceOperate.Instanse.GetResourceByKey("Name");
            this.gpHKmarket.Text = ResourceOperate.Instanse.GetResourceByKey("market");
            #endregion 港股行情信息显示
            #endregion 港股语言类型显示
            #region 股指期货语言类型显示
            this.lblGZQHContract.Text = ResourceOperate.Instanse.GetResourceByKey("lblCode");
            this.lblGZQHAmount.Text = ResourceOperate.Instanse.GetResourceByKey("Amount");
            this.lblGZQHTradeID.Text = ResourceOperate.Instanse.GetResourceByKey("lblTradeID");
            this.lblGZQHCapital.Text = ResourceOperate.Instanse.GetResourceByKey("lblCapital");
            this.lblGZQHPrice.Text = ResourceOperate.Instanse.GetResourceByKey("Price");
            this.lblGZQHBatch.Text = ResourceOperate.Instanse.GetResourceByKey("lblBatch");
            this.lblGZQHHigh.Text = ResourceOperate.Instanse.GetResourceByKey("lblHigh");
            this.lblGZQHLow.Text = ResourceOperate.Instanse.GetResourceByKey("lblLow");
            this.lblGZQHBuySell.Text = ResourceOperate.Instanse.GetResourceByKey("lblBuySell");
            this.lblGZQHUnit.Text = ResourceOperate.Instanse.GetResourceByKey("lblUnit");
            this.lblGZQHMax.Text = ResourceOperate.Instanse.GetResourceByKey("lblMax");
            this.lblGZQHACapital.Text = ResourceOperate.Instanse.GetResourceByKey("lblACapital");
            this.lblGZQHFCapital.Text = ResourceOperate.Instanse.GetResourceByKey("lblFCapital");
            this.lblGZQHBDay.Text = ResourceOperate.Instanse.GetResourceByKey("lblBDay");
            this.lblGZQHTCapital.Text = ResourceOperate.Instanse.GetResourceByKey("lblTCapital");
            this.btnGZSendOrder.Text = ResourceOperate.Instanse.GetResourceByKey("DoOrder");
            this.gpgGZQHDoOrder.Text = ResourceOperate.Instanse.GetResourceByKey("gpbDoOrder");
            this.gpgGZQHPushBack.Text = ResourceOperate.Instanse.GetResourceByKey("gpgPushBack");
            this.tabPageGZQHGrid.Text = ResourceOperate.Instanse.GetResourceByKey("tabPageGrid");
            this.tabPageGZQHList.Text = ResourceOperate.Instanse.GetResourceByKey("tabPageList");
            this.cbGZMarket.Text = ResourceOperate.Instanse.GetResourceByKey("cbMarketOrder");
            this.lblGZQHOpenClose.Text = ResourceOperate.Instanse.GetResourceByKey("lblGZQHOpenClose");
            this.lblGZQHMTotal.Text = ResourceOperate.Instanse.GetResourceByKey("lblGZQHMTotal");
            this.lblGZQHPath.Text = ResourceOperate.Instanse.GetResourceByKey("Path");
            this.btnGZQHBatchOrder.Text = ResourceOperate.Instanse.GetResourceByKey("BatchOrder");
            this.lblGZQHTime.Text = ResourceOperate.Instanse.GetResourceByKey("Time");
            #region 现货行情信息显示
            this.lblGZQHBuyFirstPrice.Text = ResourceOperate.Instanse.GetResourceByKey("FirstPrice");
            this.lblGZQHBuySecondPrice.Text = ResourceOperate.Instanse.GetResourceByKey("SecondPrice");
            this.lblGZQHBuyThirdPrice.Text = ResourceOperate.Instanse.GetResourceByKey("ThirdPrice");
            this.lblGZQHBuyFourthPrice.Text = ResourceOperate.Instanse.GetResourceByKey("FourthPrice");
            this.lblGZQHBuyFivePrice.Text = ResourceOperate.Instanse.GetResourceByKey("FivePrice");
            this.lblGZQHSellFirstPrice.Text = ResourceOperate.Instanse.GetResourceByKey("FirstPrice");
            this.lblGZQHSellSecondPrice.Text = ResourceOperate.Instanse.GetResourceByKey("SecondPrice");
            this.lblGZQHSellThirdPrice.Text = ResourceOperate.Instanse.GetResourceByKey("ThirdPrice");
            this.lblGZQHSellFourthPrice.Text = ResourceOperate.Instanse.GetResourceByKey("FourthPrice");
            this.lblGZQHSellFivePrice.Text = ResourceOperate.Instanse.GetResourceByKey("FivePrice");
            this.lblGZQHSell.Text = ResourceOperate.Instanse.GetResourceByKey("Sell");
            this.lblGZQHBuy.Text = ResourceOperate.Instanse.GetResourceByKey("Buy");
            this.lblGZQHLastPrice.Text = ResourceOperate.Instanse.GetResourceByKey("LastPrice");
            this.lblGZQHLastVolume.Text = ResourceOperate.Instanse.GetResourceByKey("LastVolume");
            this.lblGZQHLowerPrice.Text = ResourceOperate.Instanse.GetResourceByKey("LowerPrice");
            this.lblGZQHUpPrice.Text = ResourceOperate.Instanse.GetResourceByKey("UpPrice");
            this.lblGZQHYesterPrice.Text = ResourceOperate.Instanse.GetResourceByKey("YesterPrice");
            this.lblGZQHName.Text = ResourceOperate.Instanse.GetResourceByKey("Name");
            this.gpGZQHmarket.Text = ResourceOperate.Instanse.GetResourceByKey("market");
            #endregion 现货行情信息显示
            #endregion 股指期货语言类型显示
            #region 商品期货多语言显示
            this.lblSPQHContract.Text = ResourceOperate.Instanse.GetResourceByKey("lblCode");
            this.lblSPQHAmount.Text = ResourceOperate.Instanse.GetResourceByKey("Amount");
            this.lblSPQHTradeID.Text = ResourceOperate.Instanse.GetResourceByKey("lblTradeID");
            this.lblSPQHCapital.Text = ResourceOperate.Instanse.GetResourceByKey("lblCapital");
            this.lblSPQHPrice.Text = ResourceOperate.Instanse.GetResourceByKey("Price");
            this.lblSPQHBatch.Text = ResourceOperate.Instanse.GetResourceByKey("lblBatch");
            this.lblSPQHHigh.Text = ResourceOperate.Instanse.GetResourceByKey("lblHigh");
            this.lblSPQHLow.Text = ResourceOperate.Instanse.GetResourceByKey("lblLow");
            this.lblSPQHBugSell.Text = ResourceOperate.Instanse.GetResourceByKey("lblBuySell");
            this.lblSPQHUnit.Text = ResourceOperate.Instanse.GetResourceByKey("lblUnit");
            this.lblSPQHMax.Text = ResourceOperate.Instanse.GetResourceByKey("lblMax");
            this.lblSPQHACapital.Text = ResourceOperate.Instanse.GetResourceByKey("lblACapital");
            this.lblSPQHFCapital.Text = ResourceOperate.Instanse.GetResourceByKey("lblFCapital");
            this.lblSPQHBDay.Text = ResourceOperate.Instanse.GetResourceByKey("lblBDay");
            this.lblSPQHToday.Text = ResourceOperate.Instanse.GetResourceByKey("lblTCapital");
            this.btnSPQHOrder.Text = ResourceOperate.Instanse.GetResourceByKey("DoOrder");
            this.gpgSPQHDoOrder.Text = ResourceOperate.Instanse.GetResourceByKey("gpbDoOrder");
            this.gpgSPQHPushBack.Text = ResourceOperate.Instanse.GetResourceByKey("gpgPushBack");
            this.tabPageSPQHGrid.Text = ResourceOperate.Instanse.GetResourceByKey("tabPageGrid");
            this.tabPageSPQHList.Text = ResourceOperate.Instanse.GetResourceByKey("tabPageList");

            this.lblSPQHOpenClose.Text = ResourceOperate.Instanse.GetResourceByKey("lblGZQHOpenClose");
            this.lblSPQHMTotal.Text = ResourceOperate.Instanse.GetResourceByKey("lblGZQHMTotal");
            this.lblSPQHPath.Text = ResourceOperate.Instanse.GetResourceByKey("Path");
            this.btnSPQHBatchOrder.Text = ResourceOperate.Instanse.GetResourceByKey("BatchOrder");
            this.lblSPQHTime.Text = ResourceOperate.Instanse.GetResourceByKey("Time");
            #region 现货行情信息显示
            this.lblSPQHBuyFirstPrice.Text = ResourceOperate.Instanse.GetResourceByKey("FirstPrice");
            this.lblSPQHBuySecondPrice.Text = ResourceOperate.Instanse.GetResourceByKey("SecondPrice");
            this.lblSPQHBuyThirdPrice.Text = ResourceOperate.Instanse.GetResourceByKey("ThirdPrice");
            this.lblSPQHBuyFourthPrice.Text = ResourceOperate.Instanse.GetResourceByKey("FourthPrice");
            this.lblSPQHBuyFivePrice.Text = ResourceOperate.Instanse.GetResourceByKey("FivePrice");
            this.lblSPQHSellFirstPrice.Text = ResourceOperate.Instanse.GetResourceByKey("FirstPrice");
            this.lblSPQHSellSecondPrice.Text = ResourceOperate.Instanse.GetResourceByKey("SecondPrice");
            this.lblSPQHSellThirdPrice.Text = ResourceOperate.Instanse.GetResourceByKey("ThirdPrice");
            this.lblSPQHSellFourthPrice.Text = ResourceOperate.Instanse.GetResourceByKey("FourthPrice");
            this.lblSPQHSellFivePrice.Text = ResourceOperate.Instanse.GetResourceByKey("FivePrice");
            this.lblSPQHSell.Text = ResourceOperate.Instanse.GetResourceByKey("Sell");
            this.lblSPQHBuy.Text = ResourceOperate.Instanse.GetResourceByKey("Buy");
            this.lblSPQHLastPrice.Text = ResourceOperate.Instanse.GetResourceByKey("LastPrice");
            this.lblSPQHLastVolume.Text = ResourceOperate.Instanse.GetResourceByKey("LastVolume");
            this.lblSPQHLowerPrice.Text = ResourceOperate.Instanse.GetResourceByKey("LowerPrice");
            this.lblSPQHUpPrice.Text = ResourceOperate.Instanse.GetResourceByKey("UpPrice");
            this.lblSPQHYesterPrice.Text = ResourceOperate.Instanse.GetResourceByKey("YesterPrice");
            this.lblSPQHName.Text = ResourceOperate.Instanse.GetResourceByKey("Name");
            this.gpSPQHmarket.Text = ResourceOperate.Instanse.GetResourceByKey("market");
            #endregion 现货行情信息显示
            #endregion 商品期货多语言显示
            #region 试玩测试界面
            this.lblTestTradeID.Text = ResourceOperate.Instanse.GetResourceByKey("lblTestTradeID");
            this.lblTestCapitalType.Text = ResourceOperate.Instanse.GetResourceByKey("lblTestCapitalType");
            this.lblTestCapitalCurrency.Text = ResourceOperate.Instanse.GetResourceByKey("lblTestCapitalCurrency");
            this.lblTestCapitalAmount.Text = ResourceOperate.Instanse.GetResourceByKey("lblTestCapitalAmount");
            this.lblTestDesc.Text = ResourceOperate.Instanse.GetResourceByKey("lblTestDesc");
            this.gpgTest.Text = ResourceOperate.Instanse.GetResourceByKey("gpgTest");
            this.butCleat.Text = ResourceOperate.Instanse.GetResourceByKey("butCleat");
            this.butPersonalization.Text = ResourceOperate.Instanse.GetResourceByKey("butPersonalization");
            #endregion 试玩测试界面
            #region 柜台连接
            this.btnConnect.Text = ResourceOperate.Instanse.GetResourceByKey("btnConnect");
            this.btnRegisterChannle.Text = ResourceOperate.Instanse.GetResourceByKey("btnRegisterChannle");
            this.btnDisConnect.Text = ResourceOperate.Instanse.GetResourceByKey("btnDisConnect");
            this.lblChannel.Text = ResourceOperate.Instanse.GetResourceByKey("lblChannel");
            this.lblEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("lblEntrust");
            this.cbXH.Text = ResourceOperate.Instanse.GetResourceByKey("cbXH");
            #endregion 柜台连接
            #region 查询
            this.tabPageQueryXH.Text = ResourceOperate.Instanse.GetResourceByKey("tabPageXH");
            this.tabPageQueryHK.Text = ResourceOperate.Instanse.GetResourceByKey("tabPageHK");
            this.tabPageQueryGZQH.Text = ResourceOperate.Instanse.GetResourceByKey("tabPageGZQH");
            this.tabPageCapitalFlow.Text = ResourceOperate.Instanse.GetResourceByKey("tabPageCapitalFlow");
            this.tabPageSPQH.Text = ResourceOperate.Instanse.GetResourceByKey("tabPageSPQH");
            this.tabPageQuerySPQH.Text = ResourceOperate.Instanse.GetResourceByKey("tabPageSPQH");
            #region 现货查询
            this.lblXHCode.Text = ResourceOperate.Instanse.GetResourceByKey("lblCode");
            this.lblXHEntrustNumber.Text = ResourceOperate.Instanse.GetResourceByKey("lblXHEntrustNumber");
            this.lblXHEntrustNumbers.Text = ResourceOperate.Instanse.GetResourceByKey("lblXHEntrustNumber");
            this.gpgQueryXHCapital.Text = ResourceOperate.Instanse.GetResourceByKey("gpgCapital");
            this.btnQueryXHCapital.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryXHTodayTrade.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnXHMarketValue.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnXHTotalCapital.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryXHTodayEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryXHHold.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.tabPageXHHold.Text = ResourceOperate.Instanse.GetResourceByKey("Hold");
            this.tabPageXHEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("Entrust");
            this.tabPageXHTrade.Text = ResourceOperate.Instanse.GetResourceByKey("Trade");
            this.tabPageXHMarket.Text = ResourceOperate.Instanse.GetResourceByKey("MarkeValue");
            this.tabPageXHTotal.Text = ResourceOperate.Instanse.GetResourceByKey("TotalCapital");
            this.tabPageXHHistoryEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("HistoryEntrust");
            this.tabPageXHHistoryTrade.Text = ResourceOperate.Instanse.GetResourceByKey("HistoryTrade");
            this.btnXHHistoryEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnXHHistoryTrade.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.lblXHstart.Text = ResourceOperate.Instanse.GetResourceByKey("start");
            this.lblXHend.Text = ResourceOperate.Instanse.GetResourceByKey("end");
            this.chkXHDateTime_HistoryEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("QueryTime");
            this.lblXHTradestart.Text = ResourceOperate.Instanse.GetResourceByKey("start");
            this.lblXHTradeend.Text = ResourceOperate.Instanse.GetResourceByKey("end");
            this.chkXHDateTime_HistoryTrade.Text = ResourceOperate.Instanse.GetResourceByKey("QueryTime");
            #endregion 现货查询
            #region 港股查询
            this.gpgQueryHKCapital.Text = ResourceOperate.Instanse.GetResourceByKey("gpgCapital");
            this.btnQueryHKCapital.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryHKHold.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryHKTodayEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryHKTodayTrade.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryHK_HistoryEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQHKHistoryTrade.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnModifyQuery.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryMarketValue.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnTotalCapital.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.tabPageHKHold.Text = ResourceOperate.Instanse.GetResourceByKey("Hold");
            this.tabPageHKTodayEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("Entrust");
            this.tabPageHKTodayTrade.Text = ResourceOperate.Instanse.GetResourceByKey("Trade");
            this.tabPageHKMarketValue.Text = ResourceOperate.Instanse.GetResourceByKey("MarkeValue");
            this.tabPageHKTotalCapital.Text = ResourceOperate.Instanse.GetResourceByKey("TotalCapital");
            this.tabPageHKHistoryEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("HistoryEntrust");
            this.tabPageHKHistoryTrade.Text = ResourceOperate.Instanse.GetResourceByKey("HistoryTrade");
            this.tabPageHKModifyOrder.Text = ResourceOperate.Instanse.GetResourceByKey("ModifyOrder");
            #region 港股详细信息
            this.lblHKEntrustNumber.Text = ResourceOperate.Instanse.GetResourceByKey("lblXHEntrustNumber");
            this.lblHKEntrustNumber1.Text = ResourceOperate.Instanse.GetResourceByKey("lblXHEntrustNumber");
            this.lblHKEntrustNumber3.Text = ResourceOperate.Instanse.GetResourceByKey("lblXHEntrustNumber");
            this.lblHKEntrustNumber4.Text = ResourceOperate.Instanse.GetResourceByKey("lblXHEntrustNumber");
            this.lblHKEntrustNumber5.Text = ResourceOperate.Instanse.GetResourceByKey("lblXHEntrustNumber");
            this.lblstart.Text = ResourceOperate.Instanse.GetResourceByKey("start");
            this.lblstart1.Text = ResourceOperate.Instanse.GetResourceByKey("start");
            this.lblstart3.Text = ResourceOperate.Instanse.GetResourceByKey("start");
            this.lblend.Text = ResourceOperate.Instanse.GetResourceByKey("end");
            this.lblend1.Text = ResourceOperate.Instanse.GetResourceByKey("end");
            this.lblend3.Text = ResourceOperate.Instanse.GetResourceByKey("end");
            this.chkQueryHKHisTrade.Text = ResourceOperate.Instanse.GetResourceByKey("QueryTime");
            this.chkModifyTime.Text = ResourceOperate.Instanse.GetResourceByKey("QueryTime");
            this.chkHKDateTime.Text = ResourceOperate.Instanse.GetResourceByKey("QueryTime");
            this.lblDo1.Text = ResourceOperate.Instanse.GetResourceByKey("Do");
            this.lblDo4.Text = ResourceOperate.Instanse.GetResourceByKey("Do");
            this.lblDo5.Text = ResourceOperate.Instanse.GetResourceByKey("Do");
            this.lblHKDo.Text = ResourceOperate.Instanse.GetResourceByKey("Do");
            this.lblHKCodes.Text = ResourceOperate.Instanse.GetResourceByKey("lblCode");
            #endregion 港股详细信息
            #endregion 港股查询
            #region 股指期货查询
            this.tabPageQHHistoryEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("HistoryEntrust");
            this.gpgQueryGZQHCapital.Text = ResourceOperate.Instanse.GetResourceByKey("gpgCapital");
            this.btnQueryGZCapital.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryGZHold.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryGZTodayEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryGZTodayTrade.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQHQueryTotalCapital.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryQHFlow.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.button6.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.button3.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryMarketValu.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.tabPageQHHold.Text = ResourceOperate.Instanse.GetResourceByKey("Hold");
            this.tabPageQHTodayEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("Entrust");
            this.tabPageQHTodayTrade.Text = ResourceOperate.Instanse.GetResourceByKey("Trade");
            this.tabPageQHMarketValue.Text = ResourceOperate.Instanse.GetResourceByKey("MarkeValue");
            this.tabPageQHTotalCapital.Text = ResourceOperate.Instanse.GetResourceByKey("TotalCapital");
            this.tabPageQHFlowDetail.Text = ResourceOperate.Instanse.GetResourceByKey("FlowDetail");
            this.tabPageQHHistoryTrade.Text = ResourceOperate.Instanse.GetResourceByKey("HistoryTrade");
            this.lblQHCode.Text = ResourceOperate.Instanse.GetResourceByKey("lblCode");
            this.lblQHPassWord.Text = ResourceOperate.Instanse.GetResourceByKey("PassWords");
            #endregion 股指期货查询
            #region 资金流水查询
            this.butCapitalFlowQuery.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.grpQueryTerm.Text = ResourceOperate.Instanse.GetResourceByKey("grpQueryTerm");
            this.lblaccountType.Text = ResourceOperate.Instanse.GetResourceByKey("lblaccountType");
            this.lblCurrencyType.Text = ResourceOperate.Instanse.GetResourceByKey("lblCurrencyType");
            this.lblCapitalFlowType.Text = ResourceOperate.Instanse.GetResourceByKey("lblCapitalFlowType");
            this.lblCapitalAmount.Text = ResourceOperate.Instanse.GetResourceByKey("lblCapitalAmount");
            this.lblStartTime.Text = ResourceOperate.Instanse.GetResourceByKey("lblStartTime");
            this.lblEndTime.Text = ResourceOperate.Instanse.GetResourceByKey("lblEndTime");
            this.grpQueryresult.Text = ResourceOperate.Instanse.GetResourceByKey("grpQueryresult");
            #endregion 资金流水查询
            #region 商品期货查询
            this.tabPageSPQHHistoryEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("HistoryEntrust");
            this.gpQuerySPQHCapital.Text = ResourceOperate.Instanse.GetResourceByKey("gpgCapital");
            this.btnQuerySPHold.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQuerySPQHEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.bntQuerySPQHTrade.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.bntQuerySPQHMarket.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.bntQuerySPQHCapital.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.bntQueryFlowDetail.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.bntQuerySPQHHistoryTrade.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.bntQuerySPQHHistoryEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQuerySPCapital.Text = ResourceOperate.Instanse.GetResourceByKey("Query");

            this.tabPageSPQHHold.Text = ResourceOperate.Instanse.GetResourceByKey("Hold");
            this.tabPageSPQHEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("Entrust");
            this.tabPageSPQHTrade.Text = ResourceOperate.Instanse.GetResourceByKey("Trade");
            this.tabPageSPQHMarket.Text = ResourceOperate.Instanse.GetResourceByKey("MarkeValue");
            this.tabPageSPQHTotal.Text = ResourceOperate.Instanse.GetResourceByKey("TotalCapital");
            this.tabPageSPQHFlowDetail.Text = ResourceOperate.Instanse.GetResourceByKey("FlowDetail");
            this.tabPageSPQHHistoryTrade.Text = ResourceOperate.Instanse.GetResourceByKey("HistoryTrade");

            this.lblSPQHCode.Text = ResourceOperate.Instanse.GetResourceByKey("lblCode");
            this.lblSPQHPassWord.Text = ResourceOperate.Instanse.GetResourceByKey("PassWords");
            #endregion 商品期货查询
            #endregion 查询
            #region dataGridView数据绑定
            #region 现货下单数据绑定显示
            for (int i = 0; i < this.dagXH.ColumnCount; i++)
            {
                string XHName = dagXH.Columns[i].HeaderText;
                dagXH.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(XHName);
            }
            #endregion 现货下单数据绑定显示
            #region 港股下单数据绑定显示
            for (int i = 0; i < this.dagHK.ColumnCount; i++)
            {
                string HKName = dagHK.Columns[i].HeaderText;
                dagHK.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(HKName);
            }
            #endregion 港股下单数据绑定显示
            #region 股指期货下单数据绑定显示
            for (int i = 0; i < this.dataGridViewGZQH.ColumnCount; i++)
            {
                string GZQHName = dataGridViewGZQH.Columns[i].HeaderText;
                dataGridViewGZQH.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(GZQHName);
            }
            #endregion 股指期货下单数据绑定显示
            #region 商品期货下单数据绑定显示
            for (int i = 0; i < this.dataGridViewSPQH.ColumnCount; i++)
            {
                string SPQHName = dataGridViewSPQH.Columns[i].HeaderText;
                dataGridViewSPQH.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(SPQHName);
            }
            #endregion 商品期货下单数据绑定显示3
            #region 现货多语言
            #region 现货 资金dataGridView绑定
            for (int i = 0; i < this.dgXHCapital.ColumnCount; i++)
            {
                string XHCapitalName = dgXHCapital.Columns[i].HeaderText;
                dgXHCapital.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(XHCapitalName);
            }
            #endregion 现货 资金dataGridView绑定dgHKCapital
            #region 现货 持仓DataGridView绑定
            for (int i = 0; i < this.daXHHold.ColumnCount; i++)
            {
                string XHHoldName = daXHHold.Columns[i].HeaderText;
                daXHHold.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(XHHoldName);
            }
            #endregion 现货 持仓DataGridView绑定
            #region 现货 当日委托DataGridView绑定
            for (int i = 0; i < this.daXHTodayEntrust.ColumnCount; i++)
            {
                string XHTodayEntrustName = daXHTodayEntrust.Columns[i].HeaderText;
                daXHTodayEntrust.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(XHTodayEntrustName);
            }
            #endregion 现货 当日委托DataGridView绑定
            #region 现货 当日成交DataGridView绑定
            for (int i = 0; i < this.daXHTodayTrade.ColumnCount; i++)
            {
                string XHTodayTradeName = daXHTodayTrade.Columns[i].HeaderText;
                daXHTodayTrade.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(XHTodayTradeName);
            }
            #endregion 现货 当日成交DataGridView绑定
            #region 现货 市值DataGridView绑定
            for (int i = 0; i < this.dgXHMarketValue.ColumnCount; i++)
            {
                string XHMarketValueName = dgXHMarketValue.Columns[i].HeaderText;
                dgXHMarketValue.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(XHMarketValueName);
            }
            #endregion 现货 市值成交DataGridView绑定
            #region 现货 资金汇总DataGridView绑定
            for (int i = 0; i < this.dgvXHTotalCapital.ColumnCount; i++)
            {
                string XHTotalCapitalName = dgvXHTotalCapital.Columns[i].HeaderText;
                dgvXHTotalCapital.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(XHTotalCapitalName);
            }
            #endregion 现货 资金汇总DataGridView绑定
            #region 现货 历史委托DataGridView绑定
            for (int i = 0; i < this.daXHHistoryEntrust.ColumnCount; i++)
            {
                string XHHistoryEntrustName = daXHHistoryEntrust.Columns[i].HeaderText;
                daXHHistoryEntrust.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(XHHistoryEntrustName);
            }
            #endregion 现货 历史委托DataGridView绑定
            #region 现货 历史成交DataGridView绑定
            for (int i = 0; i < this.daXHHistoryTrade.ColumnCount; i++)
            {
                string XHHistoryTradeName = daXHHistoryTrade.Columns[i].HeaderText;
                daXHHistoryTrade.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(XHHistoryTradeName);
            }
            #endregion 现货 历史成交DataGridView绑定
            #endregion 现货多语言
            #region 港股多语言
            #region 港股 资金dataGridView绑定
            for (int i = 0; i < this.dgHKCapital.ColumnCount; i++)
            {
                string KCapitalName = dgHKCapital.Columns[i].HeaderText;
                dgHKCapital.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(KCapitalName);
            }
            #endregion 港股 资金dataGridView绑定dgHKCapital
            #region 港股 持仓DataGridView绑定
            for (int i = 0; i < this.dgHKHold.ColumnCount; i++)
            {
                string HKHoldName = dgHKHold.Columns[i].HeaderText;
                dgHKHold.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(HKHoldName);
            }
            #endregion 港股 持仓DataGridView绑定
            #region 现货 当日委托DataGridView绑定
            for (int i = 0; i < this.dgHKTodayEntrust.ColumnCount; i++)
            {
                string HKTodayEntrustName = dgHKTodayEntrust.Columns[i].HeaderText;
                dgHKTodayEntrust.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(HKTodayEntrustName);
            }
            #endregion 现货 当日委托DataGridView绑定
            #region 现货 当日成交DataGridView绑定
            for (int i = 0; i < this.dgHKTodayTrade.ColumnCount; i++)
            {
                string HKTodayTradeName = dgHKTodayTrade.Columns[i].HeaderText;
                dgHKTodayTrade.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(HKTodayTradeName);
            }
            #endregion 现货 当日成交DataGridView绑定
            #region 港股 市值DataGridView绑定
            for (int i = 0; i < this.dgvMarketValue.ColumnCount; i++)
            {
                string HKMarketValueValueName = dgvMarketValue.Columns[i].HeaderText;
                dgvMarketValue.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(HKMarketValueValueName);
            }
            #endregion 港股 市值成交DataGridView绑定
            #region 现货 资金汇总DataGridView绑定
            for (int i = 0; i < this.dgvTotalCapital.ColumnCount; i++)
            {
                string HKTotalCapitalName = dgvTotalCapital.Columns[i].HeaderText;
                dgvTotalCapital.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(HKTotalCapitalName);
            }
            #endregion 现货 资金汇总DataGridView绑定
            #region 现货 历史委托DataGridView绑定
            for (int i = 0; i < this.dgvHK_historyEntrust.ColumnCount; i++)
            {
                string HKHistoryEntrustName = dgvHK_historyEntrust.Columns[i].HeaderText;
                dgvHK_historyEntrust.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(HKHistoryEntrustName);
            }
            #endregion 现货 历史委托DataGridView绑定
            #region 现货 历史成交DataGridView绑定
            for (int i = 0; i < this.dgvHK_HistoryTrade.ColumnCount; i++)
            {
                string HKHistoryTradeName = dgvHK_HistoryTrade.Columns[i].HeaderText;
                dgvHK_HistoryTrade.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(HKHistoryTradeName);
            }
            #endregion 现货 历史成交DataGridView绑定
            #endregion 港股多语言
            #region 股指期货多语言
            #region 股指期货 资金dataGridView绑定
            for (int i = 0; i < this.dgQHCapital.ColumnCount; i++)
            {
                string QHCapitalName = dgQHCapital.Columns[i].HeaderText;
                dgQHCapital.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(QHCapitalName);
            }
            #endregion 股指期货 资金dataGridView绑定
            #region 股指期货 持仓dataGridView绑定
            for (int i = 0; i < this.dgvGZQHHold.ColumnCount; i++)
            {
                string GZQHHoldName = dgvGZQHHold.Columns[i].HeaderText;
                dgvGZQHHold.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(GZQHHoldName);
            }
            #endregion 股指期货 持仓dataGridView绑定
            #region 股指期货 当日委托dataGridView绑定
            for (int i = 0; i < this.dgvGZQHToday.ColumnCount; i++)
            {
                string GZQHTodayName = dgvGZQHToday.Columns[i].HeaderText;
                dgvGZQHToday.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(GZQHTodayName);
            }
            #endregion 股指期货 当日委托dataGridView绑定
            #region 股指期货 当日成交dataGridView绑定
            for (int i = 0; i < this.dagGZQHTodayTrade.ColumnCount; i++)
            {
                string GZQHTodayTradeName = dagGZQHTodayTrade.Columns[i].HeaderText;
                dagGZQHTodayTrade.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(GZQHTodayTradeName);
            }
            #endregion 股指期货 当日成交dataGridView绑定
            #region 股指期货 市值DataGridView绑定
            for (int i = 0; i < this.dgQHMarketValue.ColumnCount; i++)
            {
                string GZQHMarketValueValueName = dgQHMarketValue.Columns[i].HeaderText;
                dgQHMarketValue.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(GZQHMarketValueValueName);
            }
            #endregion 股指期货 市值成交DataGridView绑定
            #region 股指期货 资金汇总DataGridView绑定
            for (int i = 0; i < this.dgvQHTotalCapital.ColumnCount; i++)
            {
                string GZQHTotalCapitalName = dgvQHTotalCapital.Columns[i].HeaderText;
                dgvQHTotalCapital.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(GZQHTotalCapitalName);
            }
            #endregion 股指期货 资金汇总DataGridView绑定
            #region 股指期货 历史委托DataGridView绑定
            for (int i = 0; i < this.dagGZQHHistoryEntrust.ColumnCount; i++)
            {
                string GZQHHistoryEntrustName = dagGZQHHistoryEntrust.Columns[i].HeaderText;
                dagGZQHHistoryEntrust.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(GZQHHistoryEntrustName);
            }
            #endregion 股指期货 历史委托DataGridView绑定
            #region 股指期货 历史成交DataGridView绑定
            for (int i = 0; i < this.dagGZQHHistoryTrade.ColumnCount; i++)
            {
                string GZQHHistoryTradeName = dagGZQHHistoryTrade.Columns[i].HeaderText;
                dagGZQHHistoryTrade.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(GZQHHistoryTradeName);
            }
            #endregion 股指期货 历史成交DataGridView绑定
            #region 股指期货 资金流水DataGridView绑定
            for (int i = 0; i < this.dagGZQHFlowDetail.ColumnCount; i++)
            {
                string GZQHFlowDetailName = dagGZQHFlowDetail.Columns[i].HeaderText;
                dagGZQHFlowDetail.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(GZQHFlowDetailName);
            }
            #endregion 股指期货 资金流水DataGridView绑定
            #endregion 股指期货多语言
            #region 商品期货多语言
            #region 商品期货 资金dataGridView绑定
            for (int i = 0; i < this.dagSPQHCapital.ColumnCount; i++)
            {
                string SPQHCapitalName = dagSPQHCapital.Columns[i].HeaderText;
                dagSPQHCapital.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(SPQHCapitalName);
            }
            #endregion 商品期货 资金dataGridView绑定
            #region 股指期货 持仓dataGridView绑定
            for (int i = 0; i < this.dagSPQHHold.ColumnCount; i++)
            {
                string SPQHHoldName = dagSPQHHold.Columns[i].HeaderText;
                dagSPQHHold.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(SPQHHoldName);
            }
            #endregion 股指期货 持仓dataGridView绑定
            #region 股指期货 当日委托dataGridView绑定
            for (int i = 0; i < this.dagSPQHEntrust.ColumnCount; i++)
            {
                string SPQHTodayName = dagSPQHEntrust.Columns[i].HeaderText;
                dagSPQHEntrust.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(SPQHTodayName);
            }
            #endregion 股指期货 当日委托dataGridView绑定
            #region 股指期货 当日成交dataGridView绑定
            for (int i = 0; i < this.dagSPQHQueryTrade.ColumnCount; i++)
            {
                string SPQHTodayTradeName = dagSPQHQueryTrade.Columns[i].HeaderText;
                dagSPQHQueryTrade.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(SPQHTodayTradeName);
            }
            #endregion 股指期货 当日成交dataGridView绑定
            #region 商品期货 市值DataGridView绑定
            for (int i = 0; i < this.dgvQuerySPQHMarket.ColumnCount; i++)
            {
                string SPQHMarketValueValueName = dgvQuerySPQHMarket.Columns[i].HeaderText;
                dgvQuerySPQHMarket.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(SPQHMarketValueValueName);
            }
            #endregion 商品期货 市值成交DataGridView绑定
            #region 商品期货 资金汇总DataGridView绑定
            for (int i = 0; i < this.dagQuerySPQHCapital.ColumnCount; i++)
            {
                string SPQHTotalCapitalName = dagQuerySPQHCapital.Columns[i].HeaderText;
                dagQuerySPQHCapital.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(SPQHTotalCapitalName);
            }
            #endregion 商品期货 资金汇总DataGridView绑定
            #region 商品期货 历史委托DataGridView绑定
            for (int i = 0; i < this.dagSPQHHistoryEntrust.ColumnCount; i++)
            {
                string SPQHHistoryEntrustName = dagSPQHHistoryEntrust.Columns[i].HeaderText;
                dagSPQHHistoryEntrust.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(SPQHHistoryEntrustName);
            }
            #endregion 商品期货 历史委托DataGridView绑定
            #region 商品期货 历史成交DataGridView绑定
            for (int i = 0; i < this.dagSPQHHistoryTrade.ColumnCount; i++)
            {
                string SPQHHistoryTradeName = dagSPQHHistoryTrade.Columns[i].HeaderText;
                dagSPQHHistoryTrade.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(SPQHHistoryTradeName);
            }
            #endregion 商品期货 历史成交DataGridView绑定
            #region 商品期货 资金流水DataGridView绑定
            for (int i = 0; i < this.dagSPQHFlowDetail.ColumnCount; i++)
            {
                string SPQHFlowDetailName = dagSPQHFlowDetail.Columns[i].HeaderText;
                dagSPQHFlowDetail.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(SPQHFlowDetailName);
            }
            #endregion 商品期货 资金流水DataGridView绑定
            #endregion 商品期货多语言
            #region 资金流水查询
            for (int i = 0; i < this.dagCapitalFlow.ColumnCount; i++)
            {
                string CapitalFlowName = dagCapitalFlow.Columns[i].HeaderText;
                dagCapitalFlow.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(CapitalFlowName);
            }
            #endregion 资金流水查询
            #endregion dataGridView数据绑定
            #endregion 多语言显示
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            #region 自动补全
            this.cbXHCode.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.cbXHCode.AutoCompleteSource = AutoCompleteSource.ListItems;

            this.cbHKCode.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.cbHKCode.AutoCompleteSource = AutoCompleteSource.ListItems;

            this.cbGZCode.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.cbGZCode.AutoCompleteSource = AutoCompleteSource.ListItems;

            this.cbSPCode.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.cbSPCode.AutoCompleteSource = AutoCompleteSource.ListItems;
            #endregion 自动补全

            this.txtPwd.Text = ServerConfig.PassWord;
            this.txtSPQHPwd.Text = ServerConfig.PassWord;
            LocalhostResourcesFormText();
            string LoginName = ServerConfig.TraderID;
            string passWord = ServerConfig.PassWord;
            string message;
            //将TraderId和passWord通过管理中心服务进行验证
            bool result = wcfLogic.AdminConfirmation(LoginName, passWord, out message);

            //如果验证通过则说明登陆的是管理员，下单界面禁用
            if (result)
            {
                tabControl2.TabPages.Remove(tabPageXH);
                tabControl2.TabPages.Remove(tabPageHK);
                tabControl2.TabPages.Remove(tabPageGZQH);
                tabControl2.TabPages.Remove(tabPageSPQH);
                tabControl2.TabPages.Remove(tabPageQuery);
                tabControl2.TabPages.Remove(tabPageConnection);
                tabControl2.TabPages.Remove(tabPageCapitalFlow);
            }
            //验证不通过则是交易员，试玩界面禁用
            else
            {
                tabControl2.TabPages.Remove(tabPageAdminTest);
            }
            //lbCodes.Left = 72;
            //lbCodes.Top = 40;

            //this.loadCodeSuccess = CodeManager.Load();
            //if (loadCodeSuccess)
            //{
            //    CodeManager.FillList(lbCodes);
            //}
            lbCodes.Visible = false;

            comboBuySell.SelectedIndex = 0;
            cbXHUnit.SelectedIndex = 0;
            // txtCode.Focus();

            cbGZCode.SelectedIndex = 0;
            cbGZBuySell.SelectedIndex = 0;
            cbGZUnit.SelectedIndex = 0;
            cbGZOpenClose.SelectedIndex = 0;

            cbHKBuySell.SelectedIndex = 0;
            cbHKUnit.SelectedIndex = 0;
            cbHKPriceType.SelectedIndex = 0;

            cmbQueryType.SelectedIndex = 1;
            cmbCurrencyType.SelectedIndex = 1;
            cmbXHCurenyType.SelectedIndex = 0;
            cmbQHCureny.SelectedIndex = 0;
            cmbQHFlowCury.SelectedIndex = 0;

            cmbSPQHBuysell.SelectedIndex = 0;
            cmbSPQHUnit.SelectedIndex = 0;
            cmbSPQHOpenClose.SelectedIndex = 0;

            cmbSPQHCureny.SelectedIndex = 0;
            cmbSPQHFlowCury.SelectedIndex = 0;
            cmbaccountType.SelectedIndex = 0;
            cmbCurrType.SelectedIndex = 0;
            cmbTransferType.SelectedIndex = 3;
            cbSPCode.SelectedIndex = 0;
            cbHKCode.SelectedIndex = 0;
            cbXHCode.SelectedIndex = 0;
            Start();

            title = this.Text;

            if (loadCodeSuccess)
            {
                //   this.txtCode.KeyUp += this.txtCode_KeyUp;
            }

            InitPageControls();


        }

        /// <summary>
        /// 初始化基本信息
        /// </summary>
        public void Start()
        {
            smartPool.Start();

            //定时刷新资金账户信息
            timer.Interval = 1000;
            timer.Elapsed += timer_Elapsed;
            timer.Enabled = true;

            wcfLogic.LoadTraderInfo();

            traderId = ServerConfig.TraderID;
            xhAccount = ServerConfig.XHCapitalAccount;
            hkAccount = ServerConfig.HKCapitalAccount;
            gzqhAccount = ServerConfig.GZQHCapitalAccount;
            spqhAccount = ServerConfig.SPQHCapitalAccount;

            txtTradeID.Text = traderId;
            txtHKTraderID.Text = traderId;
            txtGZTraderID.Text = traderId;
            txtSPQHTradeID.Text = traderId;
            txtCapital.Text = xhAccount;
            txtHKCapital.Text = hkAccount;
            txtGZCapital.Text = gzqhAccount;
            txtSPQHCapital.Text = spqhAccount;
        }

        /// <summary>
        /// 验证是否是合法的数字格式
        /// </summary>
        /// <param name="keyWords">需要验证的字符串</param>
        /// <returns></returns>
        public static bool DecimalTest(string keyWords)
        {
            return Regex.IsMatch(keyWords, @"^[0-9]+$");
        }

        /// <summary>
        /// 初始化翻页控件
        /// </summary>
        private void InitPageControls()
        {
            pageControlXH_HistoryTrade.PageSize = pageSize;
            pageControlXH_HistoryTrade.CurrentPage = 1;
            pageControlXH_HistoryTrade.RecordsCount = 1;
            pageControlXH_HistoryTrade.OnPageChanged += new EventHandler(pageControlXH_HistoryTrade_OnPageChanged);
            pageControlXH_HistoryTrade.Visible = false;

            pageControlXH_HistoryEntrust.PageSize = pageSize;
            pageControlXH_HistoryEntrust.CurrentPage = 1;
            pageControlXH_HistoryEntrust.RecordsCount = 1;
            pageControlXH_HistoryEntrust.OnPageChanged += new EventHandler(pageControlXH_HistoryEntrust_OnPageChanged);
            pageControlXH_HistoryEntrust.Visible = false;

            pageControlGZQH_HistoryTrade.PageSize = pageSize;
            pageControlGZQH_HistoryTrade.CurrentPage = 1;
            pageControlGZQH_HistoryTrade.RecordsCount = 1;
            pageControlGZQH_HistoryTrade.OnPageChanged += new EventHandler(pageControlGZQH_HistoryTrade_OnPageChanged);
            pageControlGZQH_HistoryTrade.Visible = false;

            pageControlGZQH_HistoryEntrust.PageSize = pageSize;
            pageControlGZQH_HistoryEntrust.CurrentPage = 1;
            pageControlGZQH_HistoryEntrust.RecordsCount = 1;
            pageControlGZQH_HistoryEntrust.OnPageChanged += new EventHandler(pageControlGZQH_HistoryEntrust_OnPageChanged);
            pageControlGZQH_HistoryEntrust.Visible = false;

            pageControlSPQH_HistoryEntrust.PageSize = pageSize;
            pageControlSPQH_HistoryEntrust.CurrentPage = 1;
            pageControlSPQH_HistoryEntrust.RecordsCount = 1;
            pageControlSPQH_HistoryEntrust.OnPageChanged += new EventHandler(pageControlSPQH_HistoryEntrust_OnPageChanged);
            pageControlSPQH_HistoryEntrust.Visible = false;

            pageControlSPQH_HistoryTrade.PageSize = pageSize;
            pageControlSPQH_HistoryTrade.CurrentPage = 1;
            pageControlSPQH_HistoryTrade.RecordsCount = 1;
            pageControlSPQH_HistoryTrade.OnPageChanged += new EventHandler(pageControlSPQH_HistoryTrade_OnPageChanged_1);
            pageControlSPQH_HistoryTrade.Visible = false;

        }

        /// <summary>
        /// 定时刷新资金账户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (isProcessing)
                return;

            isProcessing = true;
            QueryXHCapital();
            ReBindXHData();

            QueryHKCapital();
            ReBindHKData();

            QueryGZQHCapital();
            ReBindGZQHData();

            QuerySPQHCapital();
            ReBindSPQHData();

            isProcessing = false;
        }

        #endregion

        #region 现货

        #region 窗体事件
        /// <summary>
        /// 下单按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDoOrder_Click(object sender, EventArgs e)
        {
            errPro.Clear();
            error.Clear();
            xhDoOrderNum = 0;

            #region 组装现货下单请求实体
            StockOrderRequest order = new StockOrderRequest();
            //判断Code是否为空，如果为空则弹出错误提示框并退出
            //if (!string.IsNullOrEmpty(this.txtCode.Text))
            //{
            //    order.Code = txtCode.Text.Trim();
            //}
            //else
            //{
            //    errPro.Clear();
            //    string CodeError = ResourceOperate.Instanse.GetResourceByKey("NullError");
            //    errPro.SetError(txtCode, "Code" + CodeError);
            //    return;
            //}
            order.Code = cbXHCode.SelectedItem.ToString();
            order.BuySell = comboBuySell.SelectedIndex == 0 ? Types.TransactionDirection.Buying : Types.TransactionDirection.Selling;
            //对用户输入的Capital进行判断，判断是否为登陆时输入的现货资金帐户.
            string Capital = ServerConfig.XHCapitalAccount;
            if (Capital.Equals(this.txtCapital.Text))
            {
                order.FundAccountId = txtCapital.Text.Trim(); //"010000002302";
            }
            else
            {
                errPro.Clear();
                string CapitalError = ResourceOperate.Instanse.GetResourceByKey("AccountError");
                errPro.SetError(txtCapital, CapitalError);
                return;
            }
            order.OrderAmount = float.Parse(txtAmount.Text.Trim());

            if (!cbMarketOrder.Checked)
            {
                if (!string.IsNullOrEmpty(txtPrice.Text.Trim()))
                {
                    //判断Price是否为零，如果为零则弹出错误提示框并退出
                    float price = 0;
                    if (float.TryParse(this.txtPrice.Text, out price))
                    {
                        if (price == 0)
                        {
                            errPro.Clear();
                            string PricezeroError = ResourceOperate.Instanse.GetResourceByKey("zero");
                            errPro.SetError(txtPrice, "Price" + PricezeroError);
                            return;
                        }
                    }
                    else
                    {
                        errPro.Clear();
                        string PriceDataError = ResourceOperate.Instanse.GetResourceByKey("Dataillegal");
                        errPro.SetError(txtPrice, "Price" + PriceDataError);
                        return;
                    }
                    #region 通过读取配置文件是否判断判断价格是否在上下限之间，来进行判断价格
                    bool SeesawPrice = ServerConfig.Price;
                    //获取配置文件，并进行判断是否进行上下限比较
                    if (SeesawPrice == true)
                    {
                        order.OrderPrice = float.Parse(txtPrice.Text.Trim());
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.txtXHHigh.Text) && !string.IsNullOrEmpty(this.txtXHLow.Text))
                    {
                        float high = float.Parse(this.txtXHHigh.Text);
                        float low = float.Parse(this.txtXHLow.Text);
                        if (price <= high && price >= low)
                        {
                            order.OrderPrice = float.Parse(txtPrice.Text.Trim());
                        }
                        else
                        {
                            string PriceErrors = ResourceOperate.Instanse.GetResourceByKey("PriceErrors");
                            errPro.SetError(txtPrice, PriceErrors);
                            return;
                        }
                    }
                    else
                    {
                        string PriceErrors = ResourceOperate.Instanse.GetResourceByKey("PriceErrors");
                        errPro.SetError(txtPrice, PriceErrors);
                        return;
                    }
                    #endregion  通过读取配置文件是否判断判断价格是否在上下限之间，来进行判断价格
                }
                else
                {
                    errPro.Clear();
                    string PriceError = ResourceOperate.Instanse.GetResourceByKey("NullError");
                    errPro.SetError(txtPrice, "Price" + PriceError);
                    return;
                }
            }
            order.OrderUnitType = Utils.GetUnit(cbXHUnit.Text.Trim());
            order.OrderWay = cbMarketOrder.Checked ? TypesOrderPriceType.OPTMarketPrice : TypesOrderPriceType.OPTLimited;
            order.PortfoliosId = "p1";
            //判断TraderID是否正确
            string id = ServerConfig.TraderID;
            if (id.Equals(this.txtTradeID.Text))
            {
                order.TraderId = txtTradeID.Text.Trim(); //"23";
            }
            else
            {
                errPro.Clear();
                string TraderIDError = ResourceOperate.Instanse.GetResourceByKey("TraderIDError");
                errPro.SetError(txtTradeID, TraderIDError);
                return;
            }
            order.TraderPassword = "";
            #endregion

            //批量下单数
            int batch = int.Parse(txtBatch.Text.Trim());

            for (int i = 0; i < batch; i++)
            {
                //异步执行下单
                smartPool.QueueWorkItem(DoXHOrder, order);
            }
            if (batch >= 50)
            {
                int scale = batch / 50;
                btnDoOrder.Enabled = false;
                xhTimer.Interval = 1000 * scale;
                xhTimer.Elapsed += delegate
                {
                    this.Invoke(new MethodInvoker(() => { btnDoOrder.Enabled = true; }));
                    xhTimer.Enabled = false;
                };
                xhTimer.Enabled = true;
            }
        }
        /// <summary>
        /// 清空信息栏中信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        /// <summary>
        /// 现货资金查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryXHCapital_Click(object sender, EventArgs e)
        {
            this.dgXHCapital.DataSource = new SortableBindingList<XH_CapitalAccountTableInfo>(wcfLogic.XHCapital);
        }

        /// <summary>
        /// 现货持仓查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryXHHold_Click(object sender, EventArgs e)
        {
            this.daXHHold.DataSource = new SortableBindingList<XH_AccountHoldTableInfo>(wcfLogic.XHHold);
        }

        /// <summary>
        /// 现货当日委托查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryXHTodayEntrust_Click(object sender, EventArgs e)
        {
            CurrentQueryValue.QueryXHEntrustNO = txtQueryXHNumber.Text;
            this.daXHTodayEntrust.DataSource = new SortableBindingList<XH_TodayEntrustTableInfo>(wcfLogic.XHTodayEntrust);
        }

        /// <summary>
        /// 现货当日成交查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryXHTodayTrade_Click(object sender, EventArgs e)
        {
            CurrentQueryValue.QueryXHTradeNO = txtQueryXHTradeNo.Text;
            this.daXHTodayTrade.DataSource = new SortableBindingList<XH_TodayTradeTableInfo>(wcfLogic.XHTodayTrade);
        }

        /// <summary>
        /// 价格上下限输入框双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtXHHigh_DoubleClick(object sender, EventArgs e)
        {
            SetXHHighLowValue();
        }

        /// <summary>
        /// 现货综合查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXHMarketValue_Click(object sender, EventArgs e)
        {
            //dgXHMarketValue.AutoGenerateColumns = false;
            string mess = "";
            dgXHMarketValue.DataSource = wcfLogic.QuerymarketValueXHHold(txtXHMarketValue.Text.Trim(), ref mess);

        }

        /// <summary>
        /// 现货资金查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXHTotalCapital_Click(object sender, EventArgs e)
        {
            string msg = "";
            List<SpotCapitalEntity> list = new List<SpotCapitalEntity>();
            // DoCommonQuery.TypesCurrencyType type =
            int x = cmbXHCurenyType.SelectedIndex;
            SpotCapitalEntity entry = new SpotCapitalEntity();

            if (x == 0)
            {
                entry = wcfLogic.QueryXHTotalCapital(Types.CurrencyType.RMB, ref msg);
            }
            else if (x == 1)
            {
                entry = wcfLogic.QueryXHTotalCapital(Types.CurrencyType.HK, ref msg);
            }
            else if (x == 2)
            {
                entry = wcfLogic.QueryXHTotalCapital(Types.CurrencyType.US, ref msg);
            }
            if (entry != null)
            {
                list.Add(entry);
            }
            dgvXHTotalCapital.DataSource = list;
        }

        /// <summary>
        /// 列表颜色设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            SetGridViewColor(this.dagXH);
        }

        /// <summary>
        /// 列表行单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dagXH.SelectedRows)
            {
                xhIndex = row.Index;

                int index = e.ColumnIndex;
                if (index != 0)
                    return;

                XHMessage message = row.DataBoundItem as XHMessage;
                if (message == null)
                    continue;
                //txtQueryXHNumber.Text = CurrentQueryValue.QueryXHEntrustNO;
                //txtQueryXHTradeNo.Text = CurrentQueryValue.QueryXHTradeNO;

                string errMsg = "";
                //现货委托撤单
                bool isSuccess = wcfLogic.CancelStockOrder(message.EntrustNumber, ref errMsg);
                if (!isSuccess)
                {
                    string msg = "现货委托[" + message.EntrustNumber + "]撤单失败！" + Environment.NewLine + errMsg;
                    LogHelper.WriteDebug(msg);

                    //MessageBox.Show(msg,"撤单失败");
                    xhLogic.UpdateMessage(message.EntrustNumber, errMsg);
                }
            }
        }

        /// <summary>
        /// 价格输入框双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPrice_DoubleClick(object sender, EventArgs e)
        {
            errPro.Clear();
            error.Clear();
            string errMsg = "";
            decimal price = 0;
            MarketDataLevel leave = wcfLogic.GetLastPricByCommodityCode(cbXHCode.SelectedItem.ToString(), 1, out errMsg);
            if (leave != null)
            {
                price = leave.LastPrice;
                #region 卖价，卖量
                this.txtXHSellFirstPrice.Text = leave.SellFirstPrice.ToString();
                this.txtXHSellSecondPrice.Text = leave.SellSecondPrice.ToString();
                this.txtXHSellThirdPrice.Text = leave.SellThirdPrice.ToString();
                this.txtXHSellFourthPrice.Text = leave.SellFourthPrice.ToString();
                this.txtXHSellFivePrice.Text = leave.SellFivePrice.ToString();

                this.txtXHSellFirstVolume.Text = leave.SellFirstVolume.ToString();
                this.txtXHSellSecondVolume.Text = leave.SellSecondVolume.ToString();
                this.txtXHSellThirdVolume.Text = leave.SellThirdVolume.ToString();
                this.txtXHSellFourthVolume.Text = leave.SellFourthVolume.ToString();
                this.txtXHSellFiveVolume.Text = leave.SellFiveVolume.ToString();
                #endregion 卖价，卖量
                #region 买价，买量
                this.txtXHBuyFirstPrice.Text = leave.BuyFirstPrice.ToString();
                this.txtXHBuySecondPrice.Text = leave.BuySecondPrice.ToString();
                this.txtXHBuyThirdPrice.Text = leave.BuyThirdPrice.ToString();
                this.txtXHBuyFourthPrice.Text = leave.BuyFourthPrice.ToString();
                this.txtXHBuyFivePrice.Text = leave.BuyFivePrice.ToString();

                this.txtXHBuyFirstVolume.Text = leave.BuyFirstVolume.ToString();
                this.txtXHBuySecondVolume.Text = leave.BuySecondVolume.ToString();
                this.txtXHBuyThirdVolume.Text = leave.BuyThirdVolume.ToString();
                this.txtXHBuyFourthVolume.Text = leave.BuyFourthVolume.ToString();
                this.txtXHBuyFiveVolume.Text = leave.BuyFiveVolume.ToString();
                #endregion
                #region 商品期货信息
                this.txtXHName.Text = leave.Name.ToString();
                this.txtXHLastPrice.Text = leave.LastPrice.ToString();
                this.txtXHLastVolume.Text = leave.LastVolume.ToString();
                this.txtXHLowerPrice.Text = leave.LowerPrice.ToString();
                this.txtXHUpPrice.Text = leave.UpPrice.ToString();
                this.txtXHYesterPrice.Text = leave.YesterPrice.ToString();
                #endregion

                this.txtXHTime.Text = leave.MarketRefreshTime.ToString("hh:mm:ss");
            }
            if (!string.IsNullOrEmpty(errMsg))
            {
                errPro.SetError(txtPrice, errMsg);
            }
            txtPrice.Text = Utils.Round(price).ToString();
            SetXHHighLowValue();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SetGridNumber(this.dagXH, e);
        }

        /// <summary>
        /// 现货选择行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dagXH.SelectedRows)
            {
                HKMessage message = row.DataBoundItem as HKMessage;
                if (message == null)
                {
                    continue;
                }
                txtQueryXHNumber.Text = message.EntrustNumber;
                txtQueryXHTradeNo.Text = message.EntrustNumber;
            }
        }

        /// <summary>
        /// 获取现货的最大委托量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMax_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                error.Clear();
                errPro.Clear();
                txtXhMax.Text = "";
                var type = cbMarketOrder.Checked
                               ? DoAccountManager.TypesOrderPriceType.OPTMarketPrice
                               : DoAccountManager.TypesOrderPriceType.OPTLimited;
                var traderId = txtTradeID.Text.Trim();
                var code = cbXHCode.SelectedItem.ToString();

                decimal price = 0;
                if (!cbMarketOrder.Checked)
                {
                    if (string.IsNullOrEmpty(txtPrice.Text))
                    {
                        // string msg = "Please input price!";
                        // MessageBox.Show(msg);
                        string err = ResourceOperate.Instanse.GetResourceByKey("Please");
                        errPro.SetError(txtXhMax, err);
                        return;
                    }
                    price = decimal.Parse(txtPrice.Text.Trim());
                }

                string errMsg = "";
                var max = wcfLogic.GetXHMaxCount(traderId, code, price, type, out errMsg);

                if (errMsg.Length > 0)
                {
                    //txtXhMax.Text = errMsg;
                    // MessageBox.Show(errMsg);
                    errPro.SetError(txtXhMax, errMsg);
                }
                else
                {
                    txtXhMax.Text = max.ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }


        private void lbCodes_DoubleClick(object sender, EventArgs e)
        {
            //txtCode.Text = GetCode(lbCodes);
            //lbCodes.Visible = false;
        }

        private void lbCodes_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    txtCode.Text = GetCode(lbCodes);
            //    lbCodes.Visible = false;

            //    return;
            //}

            //if (e.KeyCode == Keys.Escape)
            //{
            //    lbCodes.Visible = false;
            //    return;
            //}

            //if (e.KeyCode == Keys.Back)
            //{
            //    if (txtCode.Text.Length <= 1)
            //        txtCode.Text = "";
            //    else
            //    {
            //        txtCode.Text = txtCode.Text.Substring(0, txtCode.Text.Length - 1);

            //        CodeManager.FillList(lbCodes, txtCode.Text);
            //        if (lbCodes.Items.Count > 0)
            //            lbCodes.SelectedIndex = 0;
            //    }
            //}

            //if (CodeManager.Isvalidkey(e.KeyCode))
            //{
            //    txtCode.Text += GetValidKeyCode(e.KeyCode.ToString());

            //    CodeManager.FillList(lbCodes, txtCode.Text);
            //    if (lbCodes.Items.Count > 0)
            //        lbCodes.SelectedIndex = 0;
            //}
        }

        private void lbCodes_Leave(object sender, EventArgs e)
        {
            //  lbCodes.Visible = false;
        }

        /// <summary>
        /// 现货历史成交查询翻页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pageControlXH_HistoryTrade_OnPageChanged(object sender, EventArgs e)
        {
            BindXHHistoryTradeList();
        }

        /// <summary>
        /// 现货历史委托查询翻页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pageControlXH_HistoryEntrust_OnPageChanged(object sender, EventArgs e)
        {
            BindXHHistoryEntrustList();
        }

        /// <summary>
        /// 现货历史委托查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXHHistoryEntrust_Click(object sender, EventArgs e)
        {
            BindXHHistoryEntrustList();
            pageControlXH_HistoryEntrust.Visible = true;
            pageControlXH_HistoryEntrust.BindData();
        }

        /// <summary>
        /// 现货历史成交查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXHHistoryTrade_Click(object sender, EventArgs e)
        {
            BindXHHistoryTradeList();
            pageControlXH_HistoryTrade.Visible = true;
            pageControlXH_HistoryTrade.BindData();
        }

        /// <summary>
        /// 绑定现货历史成交列表
        /// </summary>
        private void BindXHHistoryTradeList()
        {
            int icount;
            string msg = "";

            DateTime? sDate = null;
            DateTime? eDate = null;

            if (chkXHDateTime_HistoryTrade.Checked)
            {
                sDate = dtpXHStart_HistoryTrade.Value;
                eDate = dtpXHEnd_HistoryTrade.Value;
            }

            List<XH_HistoryTradeTableInfo> list = wcfLogic.QueryXHHistoryTrade(out icount, pageControlXH_HistoryTrade.CurrentPage, pageControlXH_HistoryTrade.PageSize, true, ServerConfig.XHCapitalAccount, ref msg, sDate, eDate);
            pageControlXH_HistoryTrade.RecordsCount = icount;
            daXHHistoryTrade.DataSource = list;
        }

        /// <summary>
        /// 绑定现货历史委托列表
        /// </summary>
        private void BindXHHistoryEntrustList()
        {
            int icount;
            string msg = "";

            DateTime? sDate = null;
            DateTime? eDate = null;
            if (chkXHDateTime_HistoryEntrust.Checked)
            {
                sDate = dtpXHStart_HistoryEntrust.Value;
                eDate = dtpXHEnd_HistoryEntrust.Value;
            }

            List<XH_HistoryEntrustTableInfo> list = wcfLogic.QueryXHHistoryEntrust(out icount, pageControlXH_HistoryEntrust.CurrentPage, pageControlXH_HistoryEntrust.PageSize, true, ServerConfig.XHCapitalAccount, ref msg, sDate, eDate);
            pageControlXH_HistoryEntrust.RecordsCount = icount;
            daXHHistoryEntrust.DataSource = list;
        }

        #endregion

        /// <summary>
        /// 现货下单
        /// </summary>
        /// <param name="order">现货委托申请</param>
        private void DoXHOrder(StockOrderRequest order)
        {
            var res = wcfLogic.DoStockOrder(order);

            xhDoOrderNum++;

            WriteTitle(xhDoOrderNum);

            string format =
                "DoOrder[EntrustNumber={0},Code={1},EntrustAmount={2},EntrustPrice={3},BuySell={4},TraderId={5},OrderMessage={6},IsSuccess={7}]  Time={8}";
            string desc = string.Format(format, res.OrderId, order.Code, order.OrderAmount, order.OrderPrice,
                                        order.BuySell, order.TraderId, res.OrderMessage, res.IsSuccess,
                                        DateTime.Now.ToLongTimeString());
            WriteXHMsg(desc);
            LogHelper.WriteDebug(desc);

            xhLogic.ProcessDoOrder(res, order);
        }

        /// <summary>
        /// 设置现货价格上下限
        /// </summary>
        private void SetXHHighLowValue()
        {
            decimal price = 0;
            string errMsg = "";
            if (!string.IsNullOrEmpty(txtPrice.Text))
            {
                bool isSuccess = decimal.TryParse(txtPrice.Text.Trim(), out price);
                if (!isSuccess)
                {
                    // MessageBox.Show("Price is error!");
                    string error = "Price is error!";
                    errPro.Clear();
                    errPro.SetError(txtPrice, error);
                    return;
                }
            }
            //获取价格上下限
            var highLowRange = wcfLogic.GetHighLowRangeValueByCommodityCode(cbXHCode.SelectedItem.ToString(), price, out errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                errPro.SetError(txtPrice, errMsg);
            }
            if (highLowRange == null)
            {
                //MessageBox.Show("Can not get highlowrange object!");
                string errMessage = ResourceOperate.Instanse.GetResourceByKey("highlowrange");
                error.Clear();
                error.SetError(txtXHHigh, errMessage);
                return;
            }
            //港股类型处理
            if (highLowRange.RangeType == Types.HighLowRangeType.HongKongPrice)
            {
                var hkrv = highLowRange.HongKongRangeValue;

                var buySell = comboBuySell.SelectedIndex == 0
                                  ? Types.TransactionDirection.Buying
                                  : Types.TransactionDirection.Selling;
                if (buySell == Types.TransactionDirection.Buying)
                {
                    txtXHHigh.Text = Utils.Round(hkrv.BuyHighRangeValue).ToString();
                    txtXHLow.Text = Utils.Round(hkrv.BuyLowRangeValue).ToString();
                }
                else
                {
                    txtXHHigh.Text = Utils.Round(hkrv.SellHighRangeValue).ToString();
                    txtXHLow.Text = Utils.Round(hkrv.SellLowRangeValue).ToString();
                }
            }
            else
            {
                //其它类型处理
                txtXHHigh.Text = Utils.Round(highLowRange.HighRangeValue).ToString();
                txtXHLow.Text = Utils.Round(highLowRange.LowRangeValue).ToString();
            }
        }

        /// <summary>
        /// 现货资金账户信息查询
        /// </summary>
        private void QueryXHCapital()
        {
            try
            {
                string capitalAccount = txtCapital.Text.Trim();
                string msg = "";

                var cap = wcfLogic.QueryXHCapital(capitalAccount, ref msg);

                if (cap == null)
                {
                    return;
                }
                this.Invoke(new MethodInvoker(() =>
                {
                    txtAvailableCapital.Text = cap.AvailableCapital.ToString();
                    txtFreezeCapital.Text = cap.FreezeCapitalTotal.ToString();
                    txtBalance.Text = cap.BalanceOfTheDay.ToString();
                    txtXHHasDone.Text = cap.HasDoneProfitLossTotal.ToString();
                    txtTodayOutIn.Text = cap.TodayOutInCapital.ToString();
                }));
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                //LogHelper.WriteError(ex.Message, ex);
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        #endregion

        #region 港股

        #region 窗体事件

        /// <summary>
        /// 港股下单按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHKSendOrder_Click(object sender, EventArgs e)
        {
            errPro.Clear();
            error.Clear();
            hkDoOrderNum = 0;
            HKOrderRequest order = new HKOrderRequest();
            //判断Code是否为空，如果为空则弹出错误提示框并退出
            order.Code = cbHKCode.SelectedItem.ToString();
            order.BuySell = cbHKBuySell.SelectedIndex == 0
                                ? Types.TransactionDirection.Buying
                                : Types.TransactionDirection.Selling;
            string Capital = ServerConfig.HKCapitalAccount;
            if (Capital.Equals(txtHKCapital.Text.Trim()))
            {
                order.FundAccountId = txtHKCapital.Text.Trim(); //"010000002302";
            }
            else
            {
                errPro.Clear();
                string CapitalError = ResourceOperate.Instanse.GetResourceByKey("AccountError");
                errPro.SetError(txtHKCapital, "Capita" + CapitalError);
                return;
            }
            order.OrderAmount = float.Parse(txtHKAmount.Text.Trim());

            if (!string.IsNullOrEmpty(txtHKPrice.Text.Trim()))
            {
                //判断Price是否等于零，如果为空则弹出错误提示框并退出
                float price = 0;
                if (float.TryParse(this.txtHKPrice.Text, out price))
                {
                    if (price == 0)
                    {
                        errPro.Clear();
                        string HKPricezeroError = ResourceOperate.Instanse.GetResourceByKey("zero");
                        errPro.SetError(txtHKPrice, "Price" + HKPricezeroError);
                        return;
                    }
                }
                else
                {
                    errPro.Clear();
                    string HKPriceDataError = ResourceOperate.Instanse.GetResourceByKey("Dataillegal");
                    errPro.SetError(txtHKPrice, "Price" + HKPriceDataError);
                    return;
                }
                #region 价格判断
                bool SeesawPrice = ServerConfig.Price;
                if (SeesawPrice == true)
                {
                    order.OrderPrice = float.Parse(txtHKPrice.Text.Trim());
                    return;
                }
                else
                {

                    if (!string.IsNullOrEmpty(this.txtHKHigh.Text) && !string.IsNullOrEmpty(this.txtHKLow.Text))
                    {
                        float high = float.Parse(this.txtHKHigh.Text);
                        float low = float.Parse(this.txtHKLow.Text);
                        if (price <= high && price >= low)
                        {
                            order.OrderPrice = float.Parse(txtHKPrice.Text.Trim());
                        }
                        else
                        {
                            string PriceErrors = ResourceOperate.Instanse.GetResourceByKey("PriceErrors");
                            errPro.SetError(txtHKPrice, PriceErrors);
                            return;
                        }
                    }
                    else
                    {
                        string PriceErrors = ResourceOperate.Instanse.GetResourceByKey("PriceErrors");
                        errPro.SetError(txtHKPrice, PriceErrors);
                        return;
                    }
                }
                #endregion 价格判断
            }
            else
            {
                errPro.Clear();
                string HKPriceError = ResourceOperate.Instanse.GetResourceByKey("NullError");
                errPro.SetError(txtHKPrice, "Price" + HKPriceError);
                return;
            }
            order.OrderUnitType = Utils.GetUnit(cbHKUnit.Text.Trim());
            //order.OrderWay = cbMarketOrder.Checked ? TypesOrderPriceType.OPTMarketPrice : TypesOrderPriceType.OPTLimited;

            //LO限价盘
            //ELO增强限价盘
            //SLO特别限价盘
            switch (cbHKPriceType.SelectedIndex)
            {
                case 0:
                    order.OrderWay = Types.HKPriceType.LO;
                    break;
                case 1:
                    order.OrderWay = Types.HKPriceType.ELO;
                    break;
                case 2:
                    order.OrderWay = Types.HKPriceType.SLO;
                    break;
            }

            order.PortfoliosId = "p1";
            //判断TraderID是否正确
            string TraderID = ServerConfig.TraderID;
            if (TraderID.Equals(this.txtHKTraderID.Text))
            {
                order.TraderId = txtHKTraderID.Text.Trim(); //"23";
            }
            else
            {
                errPro.Clear();
                string HKTraderIDError = ResourceOperate.Instanse.GetResourceByKey("TraderIDError");
                errPro.SetError(txtHKTraderID, HKTraderIDError);
                return;
            }
            order.TraderPassword = "";

            int batch = int.Parse(txtHKBatch.Text.Trim());

            for (int i = 0; i < batch; i++)
            {
                smartPool.QueueWorkItem(DoHKOrder, order);
            }

            if (batch >= 50)
            {
                int scale = batch / 50;
                btnHKSendOrder.Enabled = false;
                hkTimer.Interval = 1000 * scale;
                hkTimer.Elapsed += delegate
                {
                    this.Invoke(new MethodInvoker(() => { btnHKSendOrder.Enabled = true; }));
                    hkTimer.Enabled = false;
                };
                hkTimer.Enabled = true;
            }
        }

        /// <summary>
        /// 双击刚下价格文本框事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtHKPrice_DoubleClick(object sender, EventArgs e)
        {
            errPro.Clear();
            error.Clear();
            string errMsg = "";
            decimal price = 0;
            MarketDataLevel leave = wcfLogic.GetLastPricByCommodityCode(cbHKCode.SelectedItem.ToString(), 4, out errMsg);
            if (leave != null)
            {
                price = leave.LastPrice;
                #region 卖价，卖量
                this.txtHKSellFirstPrice.Text = leave.SellFirstPrice.ToString();
                this.txtHKSellSecondPrice.Text = leave.SellSecondPrice.ToString();
                this.txtHKSellThirdPrice.Text = leave.SellThirdPrice.ToString();
                this.txtHKSellFourthPrice.Text = leave.SellFourthPrice.ToString();
                this.txtHKSellFivePrice.Text = leave.SellFivePrice.ToString();

                this.txtHKSellFirstVolume.Text = leave.SellFirstVolume.ToString();
                this.txtHKSellSecondVolume.Text = leave.SellSecondVolume.ToString();
                this.txtHKSellThirdVolume.Text = leave.SellThirdVolume.ToString();
                this.txtHKSellFourthVolume.Text = leave.SellFourthVolume.ToString();
                this.txtHKSellFiveVolume.Text = leave.SellFiveVolume.ToString();
                #endregion 卖价，卖量
                #region 买价，买量
                this.txtHKBuyFirstPrice.Text = leave.BuyFirstPrice.ToString();
                this.txtHKBuySecondPrice.Text = leave.BuySecondPrice.ToString();
                this.txtHKBuyThirdPrice.Text = leave.BuyThirdPrice.ToString();
                this.txtHKBuyFourthPrice.Text = leave.BuyFourthPrice.ToString();
                this.txtHKBuyFivePrice.Text = leave.BuyFivePrice.ToString();

                this.txtHKBuyFirstVolume.Text = leave.BuyFirstVolume.ToString();
                this.txtHKBuySecondVolume.Text = leave.BuySecondVolume.ToString();
                this.txtHKBuyThirdVolume.Text = leave.BuyThirdVolume.ToString();
                this.txtHKBuyFourthVolume.Text = leave.BuyFourthVolume.ToString();
                this.txtHKBuyFiveVolume.Text = leave.BuyFiveVolume.ToString();
                #endregion
                #region 商品期货信息
                this.txtHKName.Text = leave.Name.ToString();
                this.txtHKLastPrice.Text = leave.LastPrice.ToString();
                this.txtHKLastVolume.Text = leave.LastVolume.ToString();
                this.txtHKLowerPrice.Text = leave.LowerPrice.ToString();
                this.txtHKUpPrice.Text = leave.UpPrice.ToString();
                this.txtHKYesterPrice.Text = leave.YesterPrice.ToString();
                #endregion
                //行情刷新时间
                this.txtHKTime.Text = leave.MarketRefreshTime.ToString("hh:mm:ss");
            }
            if (!string.IsNullOrEmpty(errMsg))
            {
                errPro.SetError(txtHKPrice, errMsg);
            }
            txtHKPrice.Text = Utils.Round(price).ToString();
            SetHKHighLowValue();
        }
        #region 港股查询

        /// <summary>
        /// 港股资金信息查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryHKCapital_Click(object sender, EventArgs e)
        {
            this.dgHKCapital.DataSource = new SortableBindingList<HK_CapitalAccountInfo>(wcfLogic.HKCapital);
        }
        /// <summary>
        /// 港股持仓信息查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryHKHold_Click(object sender, EventArgs e)
        {
            this.dgHKHold.DataSource = new SortableBindingList<HK_AccountHoldInfo>(wcfLogic.HKHold);
        }

        /// <summary>
        /// 港股当日委托查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryHKTodayEntrust_Click(object sender, EventArgs e)
        {
            CurrentQueryValue.QueryHKEnNO = txtQueryHKEnNO.Text;
            this.dgHKTodayEntrust.DataSource = new SortableBindingList<HK_TodayEntrustInfo>(wcfLogic.HKTodayEntrust);
        }

        /// <summary>
        /// 港股当日成交查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryHKTodayTrade_Click(object sender, EventArgs e)
        {
            CurrentQueryValue.QueryHKTradeNO = txtQueryHKTradeNO.Text;
            //this.wCFLogicBindingSource11.DataSource =
            //    new SortableBindingList<HK_TodayTradeInfo>(wcfLogic.HKTodayTrade);
            //this.wCFLogicBindingSource11.ResetBindings(false);

            this.dgHKTodayTrade.DataSource = new SortableBindingList<HK_TodayTradeInfo>(wcfLogic.HKTodayTrade);
        }

        /// <summary>
        /// 港股最大值显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtHKMax_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                error.Clear();
                errPro.Clear();
                txtHKMax.Text = "";
                Types.HKPriceType priceType = Types.HKPriceType.ELO;
                switch (cbHKPriceType.SelectedIndex)
                {
                    case 0:
                        priceType = Types.HKPriceType.LO;
                        break;
                    case 1:
                        priceType = Types.HKPriceType.ELO;
                        break;
                    case 2:
                        priceType = Types.HKPriceType.SLO;
                        break;
                }

                var traderId = txtHKTraderID.Text.Trim();
                var code = cbHKCode.SelectedItem.ToString();

                decimal price = 0;

                if (string.IsNullOrEmpty(txtHKPrice.Text))
                {
                    //string msg = "Please input price!";
                    //MessageBox.Show(msg);
                    string err = ResourceOperate.Instanse.GetResourceByKey("Please");
                    errPro.SetError(txtHKMax, err);
                    return;
                }
                price = decimal.Parse(txtHKPrice.Text.Trim());


                string errMsg = "";
                var max = wcfLogic.GetHKMaxCount(traderId, code, price, priceType, out errMsg);

                if (errMsg.Length > 0)
                {
                    //txtXhMax.Text = errMsg;
                    //MessageBox.Show(errMsg);
                    errPro.SetError(txtHKMax, errMsg);
                }
                else
                {
                    txtHKMax.Text = max.ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取港股最高价
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtHKHigh_DoubleClick(object sender, EventArgs e)
        {
            SetHKHighLowValue();
        }
        /// <summary>
        /// 双击港股最低价
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtHKLow_DoubleClick(object sender, EventArgs e)
        {
            SetHKHighLowValue();
        }
        /// <summary>
        /// 点击类表时显示列表中数据详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvHK_historyEntrust_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dgvHK_historyEntrust.SelectedRows)
            {
                HK_HistoryEntrustInfo message = row.DataBoundItem as HK_HistoryEntrustInfo;
                if (message == null)
                    continue;
                txtHKHistroyEnNo.Text = message.EntrustNumber;
                txtHKHistroyTradeNo.Text = message.EntrustNumber;
            }
        }
        /// <summary>
        /// 点击类表时显示列表中数据详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvHK_HistoryTrade_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dgvHK_HistoryTrade.SelectedRows)
            {
                HK_HistoryTradeInfo message = row.DataBoundItem as HK_HistoryTradeInfo;
                if (message == null)
                    continue;
                //txtHKHistroyEnNo.Text = message.EntrustNumber;
                txtHKHistroyTradeNo.Text = message.EntrustNumber;
            }

        }
        /// <summary>
        /// 点击列表中的列时进行对应的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dagHK_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dagHK.SelectedRows)
            {
                hkIndex = row.Index;

                int index = e.ColumnIndex;

                HKMessage message = row.DataBoundItem as HKMessage;
                if (message == null)
                    continue;

                txtQueryHKEnNO.Text = message.EntrustNumber;
                txtQueryHKTradeNO.Text = message.EntrustNumber;

                switch (index)
                {
                    case 0:
                        DoHKCancelOrder(message);
                        break;
                    case 1:
                        DoHKModifyOrder(message);
                        break;
                    default:
                        return;
                }
            }
        }

        /// <summary>
        /// 港股历史委托查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryHK_HistoryEntrust_Click(object sender, EventArgs e)
        {
            errPro.Clear();
            DateTime? start = null;
            DateTime? end = null;
            if (chkHKDateTime.Checked)
            {
                start = dtpDateHKStart.Value;
                end = dtpDateHKEnd.Value;
            }
            string strMessage = "";
            List<HK_HistoryEntrustInfo> list = wcfLogic.QueryHKHisotryEntrust(txtHKHistroyEnNo.Text, start, end, ref strMessage);
            if (list == null)
            {
                list = new List<HK_HistoryEntrustInfo>();
            }

            dgvHK_historyEntrust.DataSource = list;

            if (!string.IsNullOrEmpty(strMessage))
            {
                errPro.SetError(btnQueryHK_HistoryEntrust, strMessage);
            }
        }

        /// <summary>
        /// 港股历史成交查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQHKHistoryTrade_Click(object sender, EventArgs e)
        {
            errPro.Clear();
            DateTime? start = null;
            DateTime? end = null;
            if (chkQueryHKHisTrade.Checked)
            {
                start = dtpDateHKHisStart.Value;
                end = dtpDateHKHistEnd.Value;
            }
            string strMessage = "";
            List<HK_HistoryTradeInfo> list = wcfLogic.QueryHKHisotryTrade(txtHKHistroyTradeNo.Text, start, end, ref strMessage);
            if (list == null)
            {
                list = new List<HK_HistoryTradeInfo>();
            }

            dgvHK_HistoryTrade.DataSource = list;

            if (!string.IsNullOrEmpty(strMessage))
            {
                errPro.SetError(btnQHKHistoryTrade, strMessage);
            }
        }

        private void dagHK_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

        }

        private void dataGridView10_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SetGridNumber(this.dagHK, e);
        }

        /// <summary>
        /// 港股选择行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dagHK_SelectionChanged(object sender, EventArgs e)
        {

        }

        #endregion

        /// <summary>
        /// 港股下单
        /// </summary>
        /// <param name="order">港股下单请求</param>
        private void DoHKOrder(HKOrderRequest order)
        {
            var res = wcfLogic.DoHKOrder(order);

            hkDoOrderNum++;

            WriteTitle(hkDoOrderNum);

            string format =
                "DoHKOrder[EntrustNumber={0},Code={1},EntrustAmount={2},EntrustPrice={3},BuySell={4},TraderId={5},OrderMessage={6},IsSuccess={7}]  Time={8}";
            string desc = string.Format(format, res.OrderId, order.Code, order.OrderAmount, order.OrderPrice,
                                        order.BuySell, order.TraderId, res.OrderMessage, res.IsSuccess,
                                        DateTime.Now.ToLongTimeString());
            WriteHKMsg(desc);
            LogHelper.WriteDebug(desc);

            hkLogic.ProcessDoOrder(res, order);
        }

        /// <summary>
        /// 设置港股价格上下限
        /// </summary>
        private void SetHKHighLowValue()
        {
            errPro.Clear();
            error.Clear();
            decimal price = 0;
            string errMsg = "";
            if (!string.IsNullOrEmpty(txtHKPrice.Text))
            {
                bool isSuccess = decimal.TryParse(txtHKPrice.Text.Trim(), out price);
                if (!isSuccess)
                {
                    // MessageBox.Show("Price is error!");
                    errPro.Clear();
                    string errs = ResourceOperate.Instanse.GetResourceByKey("PleaseError");
                    errPro.SetError(txtHKPrice, errs);
                    return;
                }
            }
            else
            {
                errPro.Clear();
                string err = ResourceOperate.Instanse.GetResourceByKey("Please");
                errPro.SetError(txtHKPrice, err);
                return;
            }

            Types.TransactionDirection tranType;
            tranType = cbHKBuySell.SelectedIndex == 0
                           ? Types.TransactionDirection.Buying
                           : Types.TransactionDirection.Selling;
            Types.HKPriceType priceType = Types.HKPriceType.ELO;
            switch (cbHKPriceType.SelectedIndex)
            {
                case 0:
                    priceType = Types.HKPriceType.LO;
                    break;
                case 1:
                    priceType = Types.HKPriceType.ELO;
                    break;
                case 2:
                    priceType = Types.HKPriceType.SLO;
                    break;
            }

            var highLowRange = wcfLogic.GetHKHighLowRangeValueByCommodityCode(cbHKCode.SelectedItem.ToString(), price, priceType, tranType, out errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                errPro.SetError(txtHKPrice, errMsg);
            }
            if (highLowRange == null)
            {
                // MessageBox.Show("Can not get highlowrange object!");
                string errMessage = ResourceOperate.Instanse.GetResourceByKey("highlowrange");
                error.Clear();
                error.SetError(txtHKHigh, errMessage);
                return;
            }

            if (highLowRange.RangeType == Types.HighLowRangeType.HongKongPrice) //港股类型处理
            {
                var hkrv = highLowRange.HongKongRangeValue;

                var buySell = cbHKBuySell.SelectedIndex == 0
                                  ? Types.TransactionDirection.Buying
                                  : Types.TransactionDirection.Selling;
                if (buySell == Types.TransactionDirection.Buying)
                {
                    txtHKHigh.Text = Utils.Round(hkrv.BuyHighRangeValue).ToString();
                    txtHKLow.Text = Utils.Round(hkrv.BuyLowRangeValue).ToString();
                }
                else
                {
                    txtHKHigh.Text = Utils.Round(hkrv.SellHighRangeValue).ToString();
                    txtHKLow.Text = Utils.Round(hkrv.SellLowRangeValue).ToString();
                }
                labHKHLType.Text = hkrv.HKValidPriceType.ToString();
            }
            else //其它类型处理
            {
                txtHKHigh.Text = Utils.Round(highLowRange.HighRangeValue).ToString();
                txtHKLow.Text = Utils.Round(highLowRange.LowRangeValue).ToString();
            }
        }

        /// <summary>
        /// 港股改单委托下单
        /// </summary>
        private void DoHKModifyOrder(HKMessage message)
        {
            ModifyOrderForm form = new ModifyOrderForm(message);
            var result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                HKModifyOrderRequest order = new HKModifyOrderRequest();
                order.Code = message.Code;
                order.EntrustNubmer = message.EntrustNumber;
                order.FundAccountId = message.CapitalAccount;
                order.OrderAmount = form.ModifyAmount;
                order.OrderPrice = form.ModifyPrice;
                order.TraderId = txtHKTraderID.Text;
                order.OrderUnitType = form.ModifyUnitType;
                var res = wcfLogic.ModifyHKOrder(order);

                string format =
                "DoHKModifyOrder[EntrustNumber={0},Code={1},EntrustAmount={2},EntrustPrice={3},TraderId={4},OrderMessage={5},IsSuccess={6}]  Time={7}";
                string desc = string.Format(format, res.OrderId, order.Code, order.OrderAmount, order.OrderPrice,
                                            order.TraderId, res.OrderMessage, res.IsSuccess,
                                            DateTime.Now.ToLongTimeString());
                WriteHKMsg(desc);
                LogHelper.WriteDebug(desc);
                hkLogic.UpdateMessage(message.EntrustNumber, res.OrderMessage);
            }

            form.Close();
        }

        /// <summary>
        /// 港股撤单委托下单
        /// </summary>
        private void DoHKCancelOrder(HKMessage message)
        {
            string errMsg = "";
            bool isSuccess = wcfLogic.CancelHKOrder(message.EntrustNumber, ref errMsg);
            if (!isSuccess)
            {
                string msg = "港股委托[" + message.EntrustNumber + "]撤单失败！" + Environment.NewLine + errMsg;
                LogHelper.WriteDebug(msg);

                //MessageBox.Show(msg,"撤单失败");
                hkLogic.UpdateMessage(message.EntrustNumber, errMsg);
            }
        }

        /// <summary>
        /// 港股资金账户查询
        /// </summary>
        private void QueryHKCapital()
        {
            try
            {
                string capitalAccount = txtHKCapital.Text.Trim();
                string msg = "";

                var cap = wcfLogic.QueryHKCapital(capitalAccount, ref msg);

                if (cap == null)
                    return;

                this.Invoke(new MethodInvoker(() =>
                {
                    txtHKAvailable.Text = cap.AvailableCapital.ToString();
                    txtHKFreeze.Text = cap.FreezeCapitalTotal.ToString();
                    txtHKBalance.Text = cap.BalanceOfTheDay.ToString();
                    txtHKHasDone.Text = cap.HasDoneProfitLossTotal.ToString();
                    txtHKTodayOutIn.Text = cap.TodayOutInCapital.ToString();
                }));
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        #endregion

        #region 查询
        #endregion


        #endregion

        #region 期货

        #region 股指期货

        /// <summary>
        /// 股指期货下单按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGZSendOrder_Click(object sender, EventArgs e)
        {
            errPro.Clear();
            error.Clear();
            StockIndexFuturesOrderRequest order = new StockIndexFuturesOrderRequest();
            //判断Contract是否为空，如果为空则弹出错误提示框并退出
            if (!string.IsNullOrEmpty(cbGZCode.Text.Trim()))
            {
                order.Code = cbGZCode.Text.Trim();
            }
            else
            {
                errPro.Clear();
                string CodeError = ResourceOperate.Instanse.GetResourceByKey("NullError");
                errPro.SetError(cbGZCode, "Code" + CodeError);
                return;
            }
            order.BuySell = cbGZBuySell.SelectedIndex == 0
                                ? Types.TransactionDirection.Buying
                                : Types.TransactionDirection.Selling;
            order.FundAccountId = txtGZCapital.Text.Trim(); //"010000002306";
            order.OrderAmount = float.Parse(txtGZAmount.Text.Trim());
            #region 是否市价委托
            if (!cbGZMarket.Checked)
            {
                if (!string.IsNullOrEmpty(txtGZPrice.Text.Trim()))
                {
                    //判断Price是否等于零，如果为空则弹出错误提示框并退出
                    float price = 0;
                    //if (DecimalTest(this.txtGZPrice.Text))
                    //{
                    if (float.TryParse(this.txtGZPrice.Text, out price))
                    {
                        if (price == 0)
                        {
                            errPro.Clear();
                            string GZPricezeroError = ResourceOperate.Instanse.GetResourceByKey("zero");
                            errPro.SetError(txtGZPrice, "Price" + GZPricezeroError);
                            return;
                        }
                    }
                    else
                    {
                        errPro.Clear();
                        string GZPriceDataError = ResourceOperate.Instanse.GetResourceByKey("Dataillegal");
                        errPro.SetError(txtGZPrice, "Price" + GZPriceDataError);
                        return;
                    }
                    bool SeesawPrice = ServerConfig.Price;
                    if (SeesawPrice == true)
                    {
                        order.OrderPrice = float.Parse(txtGZPrice.Text.Trim());
                        return;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this.txtGZHigh.Text) && !string.IsNullOrEmpty(this.txtGZLow.Text))
                        {
                            float high = float.Parse(this.txtGZHigh.Text);
                            float low = float.Parse(this.txtGZLow.Text);
                            if (price <= high && price >= low)
                            {
                                order.OrderPrice = float.Parse(txtGZPrice.Text.Trim());
                            }
                            else
                            {
                                string PriceErrors = ResourceOperate.Instanse.GetResourceByKey("PriceErrors");
                                errPro.SetError(txtGZPrice, PriceErrors);
                                return;
                            }
                        }
                        else
                        {
                            string PriceErrors = ResourceOperate.Instanse.GetResourceByKey("PriceErrors");
                            errPro.SetError(txtGZPrice, PriceErrors);
                            return;
                        }
                    }
                }
                else
                {
                    errPro.Clear();
                    string GZPriceError = ResourceOperate.Instanse.GetResourceByKey("NullError");
                    errPro.SetError(txtGZPrice, "Price" + GZPriceError);
                    return;
                }
            }
            #endregion 是否市价委托
            order.OrderUnitType = Utils.GetUnit(cbGZUnit.Text.Trim());
            order.OrderWay = cbGZMarket.Checked ? TypesOrderPriceType.OPTMarketPrice : TypesOrderPriceType.OPTLimited;
            order.PortfoliosId = "p2";
            order.TraderId = txtGZTraderID.Text.Trim(); //"23";
            order.TraderPassword = "";
            order.OpenCloseType = Utils.GetFutureOpenCloseType(cbGZOpenClose.Text.Trim());

            int batch = int.Parse(txtGZBatch.Text.Trim());

            gzqhDoOrderNum = 0;

            for (int i = 0; i < batch; i++)
            {
                smartPool.QueueWorkItem(DoGZQHOrder, order);
            }

            if (batch >= 50)
            {
                int scale = batch / 50;
                btnGZSendOrder.Enabled = false;
                gzqhTimer.Interval = 1000 * scale;
                gzqhTimer.Elapsed += delegate
                {
                    this.Invoke(new MethodInvoker(() => { btnGZSendOrder.Enabled = true; }));
                    gzqhTimer.Enabled = false;
                };
                gzqhTimer.Enabled = true;
            }
        }

        /// <summary>
        /// 股指期货下单
        /// </summary>
        /// <param name="order">股指期货下单请求</param>
        private void DoGZQHOrder(StockIndexFuturesOrderRequest order)
        {
            var res = wcfLogic.DoGZQHOrder(order);

            gzqhDoOrderNum++;

            WriteTitle(gzqhDoOrderNum);

            string format =
                "DoOrder[EntrustNumber={0},Code={1},EntrustAmount={2},EntrustPrice={3},BuySell={4},OpenClose={5},TraderId={6},OrderMessage={7},IsSuccess={8}] Time={9}";
            string desc = string.Format(format, res.OrderId, order.Code, order.OrderAmount, order.OrderPrice,
                                        order.BuySell, order.OpenCloseType, order.TraderId, res.OrderMessage,
                                        res.IsSuccess, DateTime.Now.ToLongTimeString());
            WriteGZQHMsg(desc);
            LogHelper.WriteDebug(desc);

            gzqhLogic.ProcessDoOrder(res, order);
        }

        /// <summary>
        /// 商品期货下单
        /// </summary>
        /// <param name="order"></param>
        private void DoSPQHOrder(MercantileFuturesOrderRequest order)
        {
            var res = wcfLogic.DoSPQHOrder(order);

            spqhDoOrderNum++;

            WriteTitle(spqhDoOrderNum);

            string format =
                "DoOrder[EntrustNumber={0},Code={1},EntrustAmount={2},EntrustPrice={3},BuySell={4},OpenClose={5},TraderId={6},OrderMessage={7},IsSuccess={8}] Time={9}";
            string desc = string.Format(format, res.OrderId, order.Code, order.OrderAmount, order.OrderPrice,
                                        order.BuySell, order.OpenCloseType, order.TraderId, res.OrderMessage,
                                        res.IsSuccess, DateTime.Now.ToLongTimeString());
            WriteSPQHMsg(desc);
            LogHelper.WriteDebug(desc);

            spqhLogic.ProcessDoOrder(res, order);
        }

        private void dataGridViewGZQH_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            SetGridViewColor(this.dataGridViewGZQH);
        }
        /// <summary>
        /// 点击列表中的第一列是触发此事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewGZQH_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridViewGZQH.SelectedRows)
            {
                gzqhIndex = row.Index;

                int index = e.ColumnIndex;
                if (index != 0)
                    return;

                QHMessage message = row.DataBoundItem as QHMessage;
                if (message == null)
                    continue;

                string errMsg = "";
                bool isSuccess = wcfLogic.CancelGZQHOrder(message.EntrustNumber, ref errMsg);
                if (!isSuccess)
                {
                    string msg = "股指期货委托[" + message.EntrustNumber + "]撤单失败！" + Environment.NewLine + errMsg;
                    //MessageBox.Show(msg, "撤单失败");
                    message.OrderMessage = msg;
                    LogHelper.WriteDebug(msg);

                    gzqhLogic.UpdateMessage(message.EntrustNumber, errMsg);
                }
            }
        }
        /// <summary>
        /// 单击商品期货撤单按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewSPQH_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridViewSPQH.SelectedRows)
            {
                gzqhIndex = row.Index;

                int index = e.ColumnIndex;
                if (index != 0)
                    return;

                QHMessage message = row.DataBoundItem as QHMessage;
                if (message == null)
                    continue;

                string errMsg = "";
                bool isSuccess = wcfLogic.CancelSPQHOrder(message.EntrustNumber, ref errMsg);
                if (!isSuccess)
                {
                    string msg = "股指期货委托[" + message.EntrustNumber + "]撤单失败！" + Environment.NewLine + errMsg;
                    //MessageBox.Show(msg, "撤单失败");
                    message.OrderMessage = msg;
                    LogHelper.WriteDebug(msg);

                    spqhLogic.UpdateMessage(message.EntrustNumber, errMsg);
                }
            }
        }
        /// <summary>
        /// 股指期货资金信息查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryGZCapital_Click(object sender, EventArgs e)
        {
            this.dgQHCapital.DataSource = new SortableBindingList<QH_CapitalAccountTableInfo>(wcfLogic.GZQHCapital);
        }
        #region 股指期货查询功能
        /// <summary>
        ///  股指期货持仓查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryGZHold_Click(object sender, EventArgs e)
        {
            this.dgvGZQHHold.DataSource = new SortableBindingList<QH_HoldAccountTableInfo>(wcfLogic.GZQHHold);
        }
        /// <summary>
        /// 股指期货当日委托查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryGZTodayEntrust_Click(object sender, EventArgs e)
        {
            this.dgvGZQHToday.DataSource = new SortableBindingList<QH_TodayEntrustTableInfo>(wcfLogic.GZQHTodayEntrust);
        }
        /// <summary>
        /// 股指期货当日交易量查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryGZTodayTrade_Click(object sender, EventArgs e)
        {
            this.dagGZQHTodayTrade.DataSource = new SortableBindingList<QH_TodayTradeTableInfo>(wcfLogic.GZQHTodayTrade);
        }
        /// <summary>
        /// 双击股指期货价格事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtGZPrice_DoubleClick(object sender, EventArgs e)
        {
            errPro.Clear();
            error.Clear();
            // decimal price = RealTimeMarketUtil.GetInstance().GetFutureLastTrade(cbGZCode.Text.Trim());
            string errMsg = "";
            decimal price = 0;
            MarketDataLevel leave = wcfLogic.GetLastPricByCommodityCode(cbGZCode.Text.Trim(), 3, out errMsg);
            if (leave != null)
            {
                price = leave.LastPrice;
                #region 卖价，卖量
                this.txtGZQHSellFirstPrice.Text = leave.SellFirstPrice.ToString();
                this.txtGZQHSellSecondPrice.Text = leave.SellSecondPrice.ToString();
                this.txtGZQHSellThirdPrice.Text = leave.SellThirdPrice.ToString();
                this.txtGZQHSellFourthPrice.Text = leave.SellFourthPrice.ToString();
                this.txtGZQHSellFivePrice.Text = leave.SellFivePrice.ToString();

                this.txtGZQHSellFirstVolume.Text = leave.SellFirstVolume.ToString();
                this.txtGZQHSellSecondVolume.Text = leave.SellSecondVolume.ToString();
                this.txtGZQHSellThirdVolume.Text = leave.SellThirdVolume.ToString();
                this.txtGZQHSellFourthVolume.Text = leave.SellFourthVolume.ToString();
                this.txtGZQHSellFiveVolume.Text = leave.SellFiveVolume.ToString();
                #endregion 卖价，卖量
                #region 买价，买量
                this.txtGZQHBuyFirstPrice.Text = leave.BuyFirstPrice.ToString();
                this.txtGZQHBuySecondPrice.Text = leave.BuySecondPrice.ToString();
                this.txtGZQHBuyThirdPrice.Text = leave.BuyThirdPrice.ToString();
                this.txtGZQHBuyFourthPrice.Text = leave.BuyFourthPrice.ToString();
                this.txtGZQHBuyFivePrice.Text = leave.BuyFivePrice.ToString();

                this.txtGZQHBuyFirstVolume.Text = leave.BuyFirstVolume.ToString();
                this.txtGZQHBuySecondVolume.Text = leave.BuySecondVolume.ToString();
                this.txtGZQHBuyThirdVolume.Text = leave.BuyThirdVolume.ToString();
                this.txtGZQHBuyFourthVolume.Text = leave.BuyFourthVolume.ToString();
                this.txtGZQHBuyFiveVolume.Text = leave.BuyFiveVolume.ToString();
                #endregion
                #region 商品期货信息
                this.txtGZQHName.Text = leave.Name.ToString();
                this.txtGZQHLastPrice.Text = leave.LastPrice.ToString();
                this.txtGZQHLastVolume.Text = leave.LastVolume.ToString();
                this.txtGZQHLowerPrice.Text = leave.LowerPrice.ToString();
                this.txtGZQHUpPrice.Text = leave.UpPrice.ToString();
                this.txtGZQHYesterPrice.Text = leave.YesterPrice.ToString();
                #endregion

                this.txtGZQHTime.Text = leave.MarketRefreshTime.ToString("hh:mm:ss");
            }
            if (!string.IsNullOrEmpty(errMsg))
            {
                errPro.SetError(txtGZPrice, errMsg);
            }
            txtGZPrice.Text = Utils.Round(price).ToString();
            SetGZHighLowValue();
        }

        /// <summary>
        /// 股指期货价格上限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtGZHigh_DoubleClick(object sender, EventArgs e)
        {
            SetGZHighLowValue();
        }
        /// <summary>
        /// 股指期货价格下限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtGZLow_DoubleClick(object sender, EventArgs e)
        {
            SetGZHighLowValue();
        }
        /// <summary>
        /// 股指期货价格上下限
        /// </summary>
        private void SetGZHighLowValue()
        {
            error.Clear();
            decimal price = 0;
            string errMsg = "";
            if (!string.IsNullOrEmpty(txtGZPrice.Text))
            {
                bool isSuccess = decimal.TryParse(txtGZPrice.Text.Trim(), out price);
                if (!isSuccess)
                {
                    //MessageBox.Show("Price is error!");
                    //   string error = "Price is error!";
                    errPro.Clear();
                    errPro.SetError(txtGZPrice, "Price is error!");
                    return;
                }
            }

            var highLowRange = wcfLogic.GetHighLowRangeValueByCommodityCode(cbGZCode.Text.Trim(), price, out errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                errPro.SetError(txtGZPrice, errMsg);
            }
            if (highLowRange == null)
            {
                //MessageBox.Show("Can not get highlowrange object!");
                string errMessage = ResourceOperate.Instanse.GetResourceByKey("highlowrange");
                // string errors = "Can not get highlowrange object!";
                error.Clear();
                error.SetError(txtGZHigh, errMessage);
                return;
            }

            if (highLowRange.RangeType == Types.HighLowRangeType.HongKongPrice) //港股类型处理
            {
                var hkrv = highLowRange.HongKongRangeValue;

                var buySell = cbGZBuySell.SelectedIndex == 0
                                  ? Types.TransactionDirection.Buying
                                  : Types.TransactionDirection.Selling;
                if (buySell == Types.TransactionDirection.Buying)
                {
                    txtGZHigh.Text = Utils.Round(hkrv.BuyHighRangeValue).ToString();
                    txtGZLow.Text = Utils.Round(hkrv.BuyLowRangeValue).ToString();
                }
                else
                {
                    txtGZHigh.Text = Utils.Round(hkrv.SellHighRangeValue).ToString();
                    txtGZLow.Text = Utils.Round(hkrv.SellLowRangeValue).ToString();
                }
            }
            else //其它类型处理
            {
                txtGZHigh.Text = Utils.Round(highLowRange.HighRangeValue).ToString();
                txtGZLow.Text = Utils.Round(highLowRange.LowRangeValue).ToString();
            }
        }

        private void dataGridViewGZQH_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SetGridNumber(dataGridViewGZQH, e);
        }

        /// <summary>
        /// 期货历史成交查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            //CurrentQueryValue.QueryQHHistoryTradeNO = txtQueryQHTradeNo.Text;

            BindGZQHHistoryTradeList();
            pageControlGZQH_HistoryTrade.Visible = true;
            pageControlGZQH_HistoryTrade.BindData();
            // this.daQHHistoryTrade.DataSource = new SortableBindingList<QH_HistoryTradeTableInfo>(wcfLogic.QHHistoryTrade);
        }

        /// <summary>
        /// 期货历史委托查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click_1(object sender, EventArgs e)
        {
            BindGZQHHistoryEntrustList();
            pageControlGZQH_HistoryEntrust.Visible = true;
            pageControlGZQH_HistoryEntrust.BindData();
        }
        #endregion 股指期货查询功能
        /// <summary>
        /// 股指期货历史委托查询翻页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pageControlGZQH_HistoryEntrust_OnPageChanged(object sender, EventArgs e)
        {
            BindGZQHHistoryEntrustList();
        }
        /// <summary>
        /// 商品期货历史委托查询翻页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pageControlSPQH_HistoryEntrust_OnPageChanged(object sender, EventArgs e)
        {
            BindSPQHHistoryEntrustList();
        }
        /// <summary>
        /// 股指期货历史委托查询绑定列表
        /// </summary>
        private void BindGZQHHistoryEntrustList()
        {
            int icount;
            string msg = "";

            List<QH_HistoryEntrustTableInfo> list = wcfLogic.QueryQHHistoryEntrust(out icount, pageControlGZQH_HistoryEntrust.CurrentPage, pageControlGZQH_HistoryEntrust.PageSize, ServerConfig.GZQHCapitalAccount, 6, ref msg);
            pageControlGZQH_HistoryEntrust.RecordsCount = icount;
            dagGZQHHistoryEntrust.DataSource = list;
        }
        /// <summary>
        /// 商品期货历史委托查询绑定列表
        /// </summary>
        private void BindSPQHHistoryEntrustList()
        {
            int icount;
            string msg = "";

            List<QH_HistoryEntrustTableInfo> list = wcfLogic.QueryQHHistoryEntrust(out icount, pageControlSPQH_HistoryTrade.CurrentPage, pageControlSPQH_HistoryTrade.PageSize, ServerConfig.SPQHCapitalAccount, 4, ref msg);
            pageControlSPQH_HistoryTrade.RecordsCount = icount;
            dagSPQHHistoryEntrust.DataSource = list;
        }
        /// <summary>
        /// 股指期货历史成交查询翻页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <summary>
        private void pageControlGZQH_HistoryTrade_OnPageChanged(object sender, EventArgs e)
        {
            BindGZQHHistoryTradeList();
        }
        /// 商品期货历史成交查询翻页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <summary>
        private void pageControlSPQH_HistoryTrade_OnPageChanged_1(object sender, EventArgs e)
        {
            BindSPQHHistoryTradeList();
        }
        /// 股指期货历史成交查询绑定列表
        /// </summary>
        private void BindGZQHHistoryTradeList()
        {
            int icount;
            string msg = "";

            List<QH_HistoryTradeTableInfo> list = wcfLogic.QueryQHHistoryTrade(out icount, pageControlGZQH_HistoryTrade.CurrentPage, pageControlGZQH_HistoryTrade.PageSize, ServerConfig.GZQHCapitalAccount, 6, ref msg);
            pageControlGZQH_HistoryTrade.RecordsCount = icount;
            dagGZQHHistoryTrade.DataSource = list;
        }
        /// <summary>
        /// 商品期货历史成交查询绑定列表
        /// </summary>
        private void BindSPQHHistoryTradeList()
        {
            int icount;
            string msg = "";
            List<QH_HistoryTradeTableInfo> list = wcfLogic.QueryQHHistoryTrade(out icount, pageControlSPQH_HistoryTrade.CurrentPage, pageControlSPQH_HistoryTrade.PageSize, ServerConfig.SPQHCapitalAccount, 4, ref msg);
            pageControlSPQH_HistoryTrade.RecordsCount = icount;
            dagSPQHHistoryTrade.DataSource = list;
        }

        #endregion

        #region 商品期货

        /// <summary>
        /// 商品期货下单按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSPQHOrder_Click(object sender, EventArgs e)
        {
            errPro.Clear();
            error.Clear();
            MercantileFuturesOrderRequest order = new MercantileFuturesOrderRequest();
            //判断Contract是否为空，如果为空则弹出错误提示框并退出
            //if (!string.IsNullOrEmpty(txtSPQHCode.Text.Trim()))
            //{
            //    order.Code = txtSPQHCode.Text.Trim();
            //}
            //else
            //{
            //    errPro.Clear();
            //    string CodeError = ResourceOperate.Instanse.GetResourceByKey("NullError");
            //    errPro.SetError(txtSPQHCode, "Code" + CodeError);
            //    return;
            //}
            order.Code = cbSPCode.SelectedItem.ToString();
            order.BuySell = cmbSPQHBuysell.SelectedIndex == 0
                                ? Types.TransactionDirection.Buying
                                : Types.TransactionDirection.Selling;
            order.FundAccountId = txtSPQHCapital.Text.Trim(); //"010000002306";
            order.OrderAmount = float.Parse(txtSPQHAmount.Text.Trim());

            //if (!cbGZMarket.Checked)
            //{
            #region 价格处理
            if (!string.IsNullOrEmpty(txtSPQHPrice.Text.Trim()))
            {
                //判断Price是否等于零，如果为空则弹出错误提示框并退出
                float price = 0;
                if (float.TryParse(this.txtSPQHPrice.Text, out price))
                {
                    if (price == 0)
                    {
                        errPro.Clear();
                        string GZPricezeroError = ResourceOperate.Instanse.GetResourceByKey("zero");
                        errPro.SetError(txtSPQHPrice, "Price" + GZPricezeroError);
                        return;
                    }
                }
                else
                {
                    errPro.Clear();
                    string GZPriceDataError = ResourceOperate.Instanse.GetResourceByKey("Dataillegal");
                    errPro.SetError(txtSPQHPrice, "Price" + GZPriceDataError);
                    return;
                }
                bool SeesawPrice = ServerConfig.Price;
                if (SeesawPrice == true)
                {
                    order.OrderPrice = float.Parse(txtSPQHPrice.Text.Trim());
                    return;
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.txtSPQHHigh.Text) &&
                         !string.IsNullOrEmpty(this.txtSPQHLow.Text))
                    {
                        float high = float.Parse(this.txtSPQHHigh.Text);
                        float low = float.Parse(this.txtSPQHLow.Text);
                        if (price <= high && price >= low)
                        {
                            order.OrderPrice = float.Parse(txtSPQHPrice.Text.Trim());
                        }
                        else
                        {
                            string PriceErrors = ResourceOperate.Instanse.GetResourceByKey("PriceErrors");
                            errPro.SetError(txtSPQHPrice, PriceErrors);
                            return;
                        }
                    }
                    else
                    {
                        string PriceErrors = ResourceOperate.Instanse.GetResourceByKey("PriceErrors");
                        errPro.SetError(txtSPQHPrice, PriceErrors);
                        return;
                    }
                }
            }
            else
            {
                errPro.Clear();
                string GZPriceError = ResourceOperate.Instanse.GetResourceByKey("NullError");
                errPro.SetError(txtSPQHPrice, "Price" + GZPriceError);
                return;
            }
            #endregion 价格处理
            //}
            order.OrderUnitType = Utils.GetUnit(cmbSPQHUnit.Text.Trim());
            //    order.OrderWay = cbGZMarket.Checked ? TypesOrderPriceType.OPTMarketPrice : TypesOrderPriceType.OPTLimited;
            order.OrderWay = TypesOrderPriceType.OPTLimited;
            order.PortfoliosId = "p2";
            order.TraderId = txtSPQHTradeID.Text.Trim(); //"23";
            order.TraderPassword = "";
            order.OpenCloseType = Utils.GetFutureOpenCloseType(cmbSPQHOpenClose.Text.Trim());

            int batch = int.Parse(txtSPQHBatch.Text.Trim());

            spqhDoOrderNum = 0;

            for (int i = 0; i < batch; i++)
            {
                smartPool.QueueWorkItem(DoSPQHOrder, order);
            }

            if (batch >= 50)
            {
                int scale = batch / 50;
                btnSPQHOrder.Enabled = false;
                spqhTimer.Interval = 1000 * scale;
                spqhTimer.Elapsed += delegate
                {
                    this.Invoke(new MethodInvoker(() => { btnSPQHOrder.Enabled = true; }));
                    spqhTimer.Enabled = false;
                };
                spqhTimer.Enabled = true;
            }
        }
        /// <summary>
        /// 双击商品期货价格事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSPQHPrice_DoubleClick(object sender, EventArgs e)
        {
            errPro.Clear();
            error.Clear();
            string errMsg = "";
            decimal price = 0;
            MarketDataLevel leave = wcfLogic.GetLastPricByCommodityCode(cbSPCode.SelectedItem.ToString(), 2, out errMsg);
            if (leave != null)
            {
                price = leave.LastPrice;

                #region 卖价，卖量
                this.txtSPQHSellFirstPrice.Text = leave.SellFirstPrice.ToString();
                this.txtSPQHSellSecondPrice.Text = leave.SellSecondPrice.ToString();
                this.txtSPQHSellThirdPrice.Text = leave.SellThirdPrice.ToString();
                this.txtSPQHSellFourthPrice.Text = leave.SellFourthPrice.ToString();
                this.txtSPQHSellFivePrice.Text = leave.SellFivePrice.ToString();

                this.txtSPQHSellFirstVolume.Text = leave.SellFirstVolume.ToString();
                this.txtSPQHSellSecondVolume.Text = leave.SellSecondVolume.ToString();
                this.txtSPQHSellThirdVolume.Text = leave.SellThirdVolume.ToString();
                this.txtSPQHSellFourthVolume.Text = leave.SellFourthVolume.ToString();
                this.txtSPQHSellFiveVolume.Text = leave.SellFiveVolume.ToString();
                #endregion 卖价，卖量
                #region 买价，买量
                this.txtSPQHBuyFirstPrice.Text = leave.BuyFirstPrice.ToString();
                this.txtSPQHBuySecondPrice.Text = leave.BuySecondPrice.ToString();
                this.txtSPQHBuyThirdPrice.Text = leave.BuyThirdPrice.ToString();
                this.txtSPQHBuyFourthPrice.Text = leave.BuyFourthPrice.ToString();
                this.txtSPQHBuyFivePrice.Text = leave.BuyFivePrice.ToString();

                this.txtSPQHBuyFirstVolume.Text = leave.BuyFirstVolume.ToString();
                this.txtSPQHBuySecondVolume.Text = leave.BuySecondVolume.ToString();
                this.txtSPQHBuyThirdVolume.Text = leave.BuyThirdVolume.ToString();
                this.txtSPQHBuyFourthVolume.Text = leave.BuyFourthVolume.ToString();
                this.txtSPQHBuyFiveVolume.Text = leave.BuyFiveVolume.ToString();
                #endregion
                #region 商品期货信息
                this.txtSPQHName.Text = leave.Name.ToString();
                this.txtSPQHLastPrice.Text = leave.LastPrice.ToString();
                this.txtSPQHLastVolume.Text = leave.LastVolume.ToString();
                this.txtSPQHLowerPrice.Text = leave.LowerPrice.ToString();
                this.txtSPQHUpPrice.Text = leave.UpPrice.ToString();
                this.txtSPQHYesterPrice.Text = leave.YesterPrice.ToString();
                #endregion

                this.txtSPQHTime.Text = leave.MarketRefreshTime.ToString("hh:mm:ss");
            }
            if (!string.IsNullOrEmpty(errMsg))
            {
                errPro.SetError(txtSPQHPrice, errMsg);
            }
            txtSPQHPrice.Text = Utils.Round(price).ToString();
            SetSPHighLowValue();
        }
        /// <summary>
        /// 获取商品价格上限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSPQHHigh_DoubleClick(object sender, EventArgs e)
        {
            SetSPHighLowValue();
        }
        /// <summary>
        /// 获取商品价格下限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSPQHLow_DoubleClick(object sender, EventArgs e)
        {
            SetSPHighLowValue();
        }
        /// <summary>
        /// 获取商品期货的上下限
        /// </summary>
        private void SetSPHighLowValue()
        {
            decimal price = 0;
            error.Clear();
            string errMsg = "";
            if (!string.IsNullOrEmpty(txtSPQHPrice.Text))
            {
                bool isSuccess = decimal.TryParse(txtSPQHPrice.Text.Trim(), out price);
                if (!isSuccess)
                {
                    //  MessageBox.Show("Price is error!");
                    //  string error = "Price is error!";
                    errPro.Clear();
                    string errMessage = ResourceOperate.Instanse.GetResourceByKey("PleaseError");
                    errPro.SetError(txtSPQHPrice, errMessage);
                    return;
                }
            }

            var highLowRange = wcfLogic.GetHighLowRangeValueByCommodityCode(cbSPCode.SelectedItem.ToString(), price, out errMsg);

            if (highLowRange == null)
            {
                string errMessage = ResourceOperate.Instanse.GetResourceByKey("highlowrange");
                // string errors = "Can not get highlowrange object!";
                error.Clear();
                error.SetError(txtSPQHHigh, errMessage);
                //MessageBox.Show("Can not get highlowrange object!");
                return;
            }
            if (!string.IsNullOrEmpty(errMsg))
            {
                errPro.SetError(txtSPQHPrice, errMsg);
            }
            if (highLowRange.RangeType == Types.HighLowRangeType.HongKongPrice) //港股类型处理
            {
                var hkrv = highLowRange.HongKongRangeValue;

                var buySell = cbGZBuySell.SelectedIndex == 0
                                  ? Types.TransactionDirection.Buying
                                  : Types.TransactionDirection.Selling;
                if (buySell == Types.TransactionDirection.Buying)
                {
                    txtSPQHHigh.Text = Utils.Round(hkrv.BuyHighRangeValue).ToString();
                    txtSPQHLow.Text = Utils.Round(hkrv.BuyLowRangeValue).ToString();
                }
                else
                {
                    txtSPQHHigh.Text = Utils.Round(hkrv.SellHighRangeValue).ToString();
                    txtSPQHLow.Text = Utils.Round(hkrv.SellLowRangeValue).ToString();
                }
            }
            else //其它类型处理
            {
                txtSPQHHigh.Text = Utils.Round(highLowRange.HighRangeValue).ToString();
                txtSPQHLow.Text = Utils.Round(highLowRange.LowRangeValue).ToString();
            }
        }
        /// <summary>
        /// 商品期货资金查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuerySPCapital_Click(object sender, EventArgs e)
        {
            this.dagSPQHCapital.DataSource = new SortableBindingList<QH_CapitalAccountTableInfo>(wcfLogic.SPQHCapital);
        }
        /// <summary>
        /// 商品期货持仓查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuerySPHold_Click(object sender, EventArgs e)
        {
            this.dagSPQHHold.DataSource = new SortableBindingList<QH_HoldAccountTableInfo>(wcfLogic.SPQHHold);
        }
        /// <summary>
        /// 商品期货当日交易量查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntQuerySPQHTrade_Click(object sender, EventArgs e)
        {
            this.dagSPQHQueryTrade.DataSource = new SortableBindingList<QH_TodayTradeTableInfo>(wcfLogic.SPQHTodayTrade);
        }
        /// <summary>
        /// 商品期货当日委托查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuerySPQHEntrust_Click(object sender, EventArgs e)
        {
            this.dagSPQHEntrust.DataSource = new SortableBindingList<QH_TodayEntrustTableInfo>(wcfLogic.SPQHTodayEntrust);
        }

        /// <summary>
        /// 商品获取市价查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntQuerySPQHMarket_Click(object sender, EventArgs e)
        {
            dgvQuerySPQHMarket.AutoGenerateColumns = false;
            string mess = "";
            dgvQuerySPQHMarket.DataSource = wcfLogic.QueryMarketValueQHHold(txtQuerySPQHCode.Text.Trim(), 4, ref mess);
        }

        /// <summary>
        /// 商品期货当日金额查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntQuerySPQHCapital_Click(object sender, EventArgs e)
        {
            dagQuerySPQHCapital.AutoGenerateColumns = false;
            string msg = "";
            List<FuturesCapitalEntity> list = new List<FuturesCapitalEntity>();
            int x = cmbSPQHCureny.SelectedIndex;
            FuturesCapitalEntity entry = new FuturesCapitalEntity();

            if (x == 0)
            {
                entry = wcfLogic.QueryQHTotalCapital(Types.CurrencyType.RMB, 4, ref msg);
            }
            else if (x == 1)
            {
                entry = wcfLogic.QueryQHTotalCapital(Types.CurrencyType.HK, 4, ref msg);
            }
            else if (x == 2)
            {
                entry = wcfLogic.QueryQHTotalCapital(Types.CurrencyType.US, 4, ref msg);
            }
            //FuturesCapitalEntity entry = wcfLogic.QueryQHTotalCapital((Types.CurrencyType)cmbQHCureny.SelectedIndex + 1, ref msg);
            if (entry != null)
            {
                list.Add(entry);
            }
            dagQuerySPQHCapital.DataSource = list;
        }
        /// <summary>
        /// 商品期货盘后清算流水查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntQueryFlowDetail_Click(object sender, EventArgs e)
        {
            errPro.Clear();
            dagSPQHFlowDetail.AutoGenerateColumns = true;
            string msg = "";
            List<QH_TradeCapitalFlowDetailInfo> list = new List<QH_TradeCapitalFlowDetailInfo>();

            // List<QH_TradeCapitalFlowDetailInfo> entry = wcfLogic.QueryQHCapitalFlowDetail((QueryTypeQueryCurrencyType)cmbQHFlowCury.SelectedIndex, txtPwd.Text.Trim(), out msg);
            int x = cmbSPQHFlowCury.SelectedIndex;
            List<QH_TradeCapitalFlowDetailInfo> entry = new List<QH_TradeCapitalFlowDetailInfo>();
            if (x == 0)
            {
                entry = wcfLogic.QuerySPQHCapitalFlowDetail(DoCommonQuery.QueryTypeQueryCurrencyType.ALL, txtSPQHPwd.Text.Trim(), out msg);
            }
            else if (x == 1)
            {
                entry = wcfLogic.QuerySPQHCapitalFlowDetail(DoCommonQuery.QueryTypeQueryCurrencyType.RMB, txtSPQHPwd.Text.Trim(), out msg);
            }
            else if (x == 2)
            {
                entry = wcfLogic.QuerySPQHCapitalFlowDetail(DoCommonQuery.QueryTypeQueryCurrencyType.HK, txtSPQHPwd.Text.Trim(), out msg);
            }
            else if (x == 3)
            {
                entry = wcfLogic.QuerySPQHCapitalFlowDetail(DoCommonQuery.QueryTypeQueryCurrencyType.US, txtSPQHPwd.Text.Trim(), out msg);
            }
            if (!string.IsNullOrEmpty(msg))
            {
                errPro.SetError(txtSPQHPwd, msg);
            }

            if (entry != null)
            {
                list = entry;
            }
            dagSPQHFlowDetail.DataSource = list;
        }
        /// <summary>
        /// 商品期货历史成交查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntQuerySPQHHistoryTrade_Click(object sender, EventArgs e)
        {
            BindSPQHHistoryTradeList();
            pageControlSPQH_HistoryTrade.Visible = true;
            pageControlSPQH_HistoryTrade.BindData();
        }

        /// <summary>
        /// 商品期货历史委托查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntQuerySPQHHistoryEntrust_Click(object sender, EventArgs e)
        {
            BindSPQHHistoryEntrustList();
            pageControlSPQH_HistoryEntrust.Visible = true;
            pageControlSPQH_HistoryEntrust.BindData();
        }
        #endregion
        /// <summary>
        ///  股指期货当日金额查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQHQueryTotalCapital_Click(object sender, EventArgs e)
        {
            dgvQHTotalCapital.AutoGenerateColumns = false;
            string msg = "";
            List<FuturesCapitalEntity> list = new List<FuturesCapitalEntity>();
            int x = cmbQHCureny.SelectedIndex;
            FuturesCapitalEntity entry = new FuturesCapitalEntity();

            if (x == 0)
            {
                entry = wcfLogic.QueryQHTotalCapital(Types.CurrencyType.RMB, 6, ref msg);
            }
            else if (x == 1)
            {
                entry = wcfLogic.QueryQHTotalCapital(Types.CurrencyType.HK, 6, ref msg);
            }
            else if (x == 2)
            {
                entry = wcfLogic.QueryQHTotalCapital(Types.CurrencyType.US, 6, ref msg);
            }
            //FuturesCapitalEntity entry = wcfLogic.QueryQHTotalCapital((Types.CurrencyType)cmbQHCureny.SelectedIndex + 1, ref msg);
            if (entry != null)
            {
                list.Add(entry);
            }
            dgvQHTotalCapital.DataSource = list;

        }

        /// <summary>
        /// 查询期货相应盘后的资金清算流水
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryQHFlow_Click(object sender, EventArgs e)
        {
            errPro.Clear();
            dagGZQHFlowDetail.AutoGenerateColumns = false;
            string msg = "";
            List<QH_TradeCapitalFlowDetailInfo> list = new List<QH_TradeCapitalFlowDetailInfo>();

            // List<QH_TradeCapitalFlowDetailInfo> entry = wcfLogic.QueryQHCapitalFlowDetail((QueryTypeQueryCurrencyType)cmbQHFlowCury.SelectedIndex, txtPwd.Text.Trim(), out msg);
            int x = cmbQHFlowCury.SelectedIndex;
            List<QH_TradeCapitalFlowDetailInfo> entry = new List<QH_TradeCapitalFlowDetailInfo>();
            if (x == 0)
            {
                entry = wcfLogic.QueryQHCapitalFlowDetail(DoCommonQuery.QueryTypeQueryCurrencyType.ALL, txtPwd.Text.Trim(), out msg);
            }
            else if (x == 1)
            {
                entry = wcfLogic.QueryQHCapitalFlowDetail(DoCommonQuery.QueryTypeQueryCurrencyType.RMB, txtPwd.Text.Trim(), out msg);
            }
            else if (x == 2)
            {
                entry = wcfLogic.QueryQHCapitalFlowDetail(DoCommonQuery.QueryTypeQueryCurrencyType.HK, txtPwd.Text.Trim(), out msg);
            }
            else if (x == 3)
            {
                entry = wcfLogic.QueryQHCapitalFlowDetail(DoCommonQuery.QueryTypeQueryCurrencyType.US, txtPwd.Text.Trim(), out msg);
            }
            if (!string.IsNullOrEmpty(msg))
            {
                errPro.SetError(txtPwd, msg);
            }

            if (entry != null)
            {
                list = entry;
            }
            dagGZQHFlowDetail.DataSource = list;
        }

        /// <summary>
        /// 获取股指期货的最大委托量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtQHMax_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                error.Clear();
                errPro.Clear();
                txtGZMax.Text = "";
                var type = cbGZMarket.Checked
                               ? DoAccountManager.TypesOrderPriceType.OPTMarketPrice
                               : DoAccountManager.TypesOrderPriceType.OPTLimited;
                var traderId = txtGZTraderID.Text.Trim();
                var code = cbGZCode.Text.Trim();

                decimal price = 0;
                if (!cbGZMarket.Checked)
                {
                    if (string.IsNullOrEmpty(txtGZPrice.Text))
                    {
                        //string msg = "Please input price!";
                        //  MessageBox.Show(msg);
                        string err = ResourceOperate.Instanse.GetResourceByKey("Please");
                        errPro.SetError(txtGZMax, err);
                        return;
                    }

                    price = decimal.Parse(txtGZPrice.Text.Trim());
                }

                string errMsg = "";
                var max = wcfLogic.GetQHMaxCount(traderId, code, price, type, out errMsg);
                if (errMsg.Length > 0)
                {
                    // MessageBox.Show(errMsg);
                    //txtGZMax.Text = errMsg;
                    errPro.SetError(txtGZMax, errMsg);
                }
                else
                {
                    txtGZMax.Text = max.ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }
        /// <summary>
        /// 获取商品期货的最大委托量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSPQHMax_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                errPro.Clear();
                error.Clear();
                txtSPQHMax.Text = "";
                var type = DoAccountManager.TypesOrderPriceType.OPTLimited;
                //TypesFutureOpenCloseType
                //   var type = TypesOrderPriceType.OPTLimited;
                var traderId = txtSPQHTradeID.Text.Trim();
                var code = cbSPCode.SelectedItem.ToString();

                decimal price = 0;
                if (string.IsNullOrEmpty(txtSPQHPrice.Text))
                {
                    string err = ResourceOperate.Instanse.GetResourceByKey("Please");
                    errPro.SetError(txtSPQHMax, err);
                    return;
                }
                price = decimal.Parse(txtSPQHPrice.Text.Trim());
                string errMsg = "";
                var max = wcfLogic.GetQHMaxCount(traderId, code, price, type, out errMsg);
                if (errMsg.Length > 0)
                {
                    //MessageBox.Show(errMsg);
                    errPro.SetError(txtSPQHMax, errMsg);
                    return;
                    //txtGZMax.Text = errMsg;
                }
                else
                {
                    txtSPQHMax.Text = max.ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }
        #endregion

        #region 连接

        /// <summary>
        /// 开启Timer
        /// </summary>
        public void StartTimer()
        {
            timer.Enabled = true;
        }

        /// <summary>
        /// 关闭Timer
        /// </summary>
        public void StopTimer()
        {
            timer.Enabled = false;
        }

        /// <summary>
        /// 连接按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (wcfLogic.IsServiceOk)
                return;
            string errorMsg = "";
            bool isSuccess = wcfLogic.Initialize(ServerConfig.TraderID, txtChannelID.Text.Trim(), out errorMsg);
            if (isSuccess)
            {
                //StartTimer();
            }
            else
            {
                string errMsg = ResourceOperate.Instanse.GetResourceByKey("StartWCF");
                MessageBox.Show(errMsg);
            }
        }

        /// <summary>
        /// 关闭连接按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisConnect_Click(object sender, EventArgs e)
        {
            if (wcfLogic.IsServiceOk)
            {
                wcfLogic.ShutDown();
                //StopTimer();
            }
        }

        /// <summary>
        /// 注册通道按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegisterChannle_Click(object sender, EventArgs e)
        {
            bool ok = false;
            string Success = ResourceOperate.Instanse.GetResourceByKey("Success");
            try
            {
                List<string> list = new List<string>();
                foreach (var item in txtEnturstNo.Text.Split(','))
                {
                    list.Add(item);
                }
                ok = wcfLogic.ChangeEntrustChannel(list, txtChannelID.Text.Trim(), cbXH.Checked ? 1 : 2);
            }
            catch
            {
            }
            if (ok)
                MessageBox.Show(Success);
        }

        #endregion

        #region 其它
        /// <summary>
        /// 获取股票代码
        /// </summary>
        /// <param name="listBox"></param>
        /// <returns></returns>
        private string GetCode(ListBox listBox)
        {
            if (listBox.SelectedIndex < 0)
                return "";

            string value = listBox.SelectedItem.ToString();
            int i = value.IndexOf(' ');
            string code = value.Substring(0, i).Trim();

            return code;
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //private string GetValidKeyCode(string key)
        //{
        //    string code = key;
        //    code = code.Substring(code.Length - 1, 1);

        //    return code.ToUpper();
        //}
        /// <summary>
        /// 重新绑定股指期货数据
        /// </summary>
        private void ReBindGZQHData()
        {
            if (!gzqhLogic.HasChanged)
                return;

            if (isReBindGZQHData)
                return;

            this.Invoke(new MethodInvoker(() =>
            {
                SortableBindingList<QHMessage> list = new SortableBindingList<QHMessage>();
                isReBindGZQHData = true;
                this.SuspendLayout();
                if (gzqhLogic.MessageList.Count > 0)
                {
                    list = new SortableBindingList<QHMessage>(gzqhLogic.MessageList);
                    this.dataGridViewGZQH.DataSource = list;
                }
                else
                {
                    this.dataGridViewGZQH.DataSource = list;
                }

                this.ResumeLayout(false);
                isReBindGZQHData = false;

                gzqhLogic.HasChanged = false;
            }));
        }
        /// <summary>
        /// 重新绑定商品期货数据
        /// </summary>
        private void ReBindSPQHData()
        {
            if (!spqhLogic.HasChanged)
                return;

            if (isReBindSPQHData)
                return;

            this.Invoke(new MethodInvoker(() =>
            {
                SortableBindingList<QHMessage> list = new SortableBindingList<QHMessage>();
                isReBindSPQHData = true;
                this.SuspendLayout();
                if (spqhLogic.MessageList.Count > 0)
                {
                    list = new SortableBindingList<QHMessage>(spqhLogic.MessageList);
                    this.dataGridViewSPQH.DataSource = list;
                }
                else
                {
                    this.dataGridViewSPQH.DataSource = list;
                }

                this.ResumeLayout(false);
                isReBindSPQHData = false;

                spqhLogic.HasChanged = false;
            }));
        }
        /// <summary>
        /// 查询出股指期货资金并显示
        /// </summary>
        private void QueryGZQHCapital()
        {
            try
            {
                string capitalAccount = txtGZCapital.Text.Trim();
                string msg = "";

                var cap = wcfLogic.QueryQHCapital(capitalAccount, ref msg);

                if (cap == null)
                    return;

                this.Invoke(new MethodInvoker(() =>
                {
                    txtGZAvailableCapital.Text = cap.AvailableCapital.ToString();
                    txtGZFreezeCapital.Text = cap.FreezeCapitalTotal.ToString();
                    txtGZBalance.Text = cap.BalanceOfTheDay.ToString();
                    txtGZTodayOutIn.Text = cap.TodayOutInCapital.ToString();
                    txtGZMargin.Text = cap.MarginTotal.ToString();
                }));
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                LogHelper.WriteError(ex.Message, ex);
            }
        }
        /// <summary>
        /// 查询出商品期货资金并显示
        /// </summary>
        private void QuerySPQHCapital()
        {
            try
            {
                string capitalAccount = txtSPQHCapital.Text.Trim();
                string msg = "";

                var cap = wcfLogic.QueryQHCapital(capitalAccount, ref msg);

                if (cap == null)
                    return;

                this.Invoke(new MethodInvoker(() =>
                {
                    txtSPQHACapital.Text = cap.AvailableCapital.ToString();
                    txtSPQHFCapital.Text = cap.FreezeCapitalTotal.ToString();
                    txtSPQHbDay.Text = cap.BalanceOfTheDay.ToString();
                    txtSPQHToday.Text = cap.TodayOutInCapital.ToString();
                    txtSPQHMTotal.Text = cap.MarginTotal.ToString();
                }));
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                LogHelper.WriteError(ex.Message, ex);
            }
        }
        /// <summary>
        /// 重新绑定现货数据
        /// </summary>
        private void ReBindXHData()
        {

            if (isClearXH)
                return;

            if (isReBindXHData)
                return;

            if (!xhLogic.HasChanged)
                return;

            this.Invoke(new MethodInvoker(() =>
            {
                isReBindXHData = true;

                this.SuspendLayout();
                SortableBindingList<XHMessage> list = new SortableBindingList<XHMessage>();
                if (xhLogic.MessageList.Count > 0)
                {
                    //SortableBindingList<XHMessage> list = new SortableBindingList<XHMessage>(xhLogic.MessageList);
                    list = new SortableBindingList<XHMessage>(xhLogic.MessageList);
                    this.dagXH.DataSource = list;
                }
                else
                {
                    this.dagXH.DataSource = list;
                }

                this.ResumeLayout(false);

                xhLogic.HasChanged = false;

                isReBindXHData = false;
            }));
        }
        /// <summary>
        /// 重新绑定港股数据
        /// </summary>
        private void ReBindHKData()
        {
            if (isClearHK)
                return;

            if (isReBindHKData)
                return;

            if (!hkLogic.HasChanged)
                return;

            this.Invoke(new MethodInvoker(() =>
            {
                SortableBindingList<HKMessage> list = new SortableBindingList<HKMessage>();
                isReBindHKData = true;

                this.SuspendLayout();

                if (hkLogic.MessageList.Count > 0)
                {
                    list = new SortableBindingList<HKMessage>(hkLogic.MessageList);
                    this.dagHK.DataSource = list;
                }
                else
                {
                    this.dagHK.DataSource = list;
                }

                this.ResumeLayout(false);

                hkLogic.HasChanged = false;

                isReBindHKData = false;
            }));
        }
        /// <summary>
        /// 处理回推信息
        /// </summary>
        /// <param name="i"></param>
        private void WriteTitle(int i)
        {
            string msg = "[" + i + "]";
            this.Invoke(new MethodInvoker(() => { this.Text = this.title + msg; }));
        }
        /// <summary>
        /// 现货信息
        /// </summary>
        /// <param name="msg"></param>
        public void WriteXHMsg(string msg)
        {
            MessageDisplayHelper.Event(msg, listBox1);
        }
        /// <summary>
        /// 港股信息
        /// </summary>
        /// <param name="msg"></param>
        public void WriteHKMsg(string msg)
        {
            if (string.IsNullOrEmpty(msg))
                return;
            MessageDisplayHelper.Event(msg, listBox5);
        }
        /// <summary>
        /// 期货的信息
        /// </summary>
        /// <param name="msg"></param>
        public void WriteGZQHMsg(string msg)
        {
            MessageDisplayHelper.Event(msg, listBox2);
        }
        /// <summary>
        /// 商品期货的信息
        /// </summary>
        /// <param name="msg"></param>
        public void WriteSPQHMsg(string msg)
        {
            MessageDisplayHelper.Event(msg, listBox6);
        }

        /// <summary>
        /// 显示服务信息
        /// </summary>
        /// <param name="msg"></param>
        public void WriteWCFMsg(string msg)
        {
            MessageDisplayHelper.Event(msg, listBox3);
        }

        /// <summary>
        /// 隔行设置列表行颜色
        /// </summary>
        /// <param name="dataGridView"></param>
        private void SetGridViewColor(DataGridView dataGridView)
        {
            if (dataGridView.Rows.Count != 0)
            {
                for (int i = 0; i < dataGridView.Rows.Count; )
                {
                    dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.LightYellow;
                    i += 2;
                }
            }
        }

        #region 菜单事件

        private void clearAllFinalStateOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isClearXH)
                return;

            //isClearXH = true;
            //var list = xhLogic.ClearAllFinalStateOrder();
            ////xHMessageLogicBindingSource.Clear();
            //foreach (var message in list)
            //{
            //    xHMessageLogicBindingSource.Remove(message);
            //}

            //isClearXH = false;
            //ReBindXHData();
        }

        /// <summary>
        /// 现货右击清空按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isClearXH)
                return;

            isClearXH = true;
            xhLogic.ClearAll();
            //xHMessageLogicBindingSource.Clear();         
            isClearXH = false;

            ReBindXHData();
        }

        /// <summary>
        /// 股指期货右击清空按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (isClearGZQH)
                return;

            isClearGZQH = true;
            gzqhLogic.ClearAll();
            //gZQHMessageLogicBindingSource.Clear();
            isClearGZQH = false;

            ReBindGZQHData();
        }
        /// <summary>
        /// 商品期货右击清空按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearAllToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (isClearSPQH)
                return;

            isClearSPQH = true;
            spqhLogic.ClearAll();
            //gZQHMessageLogicBindingSource.Clear();
            isClearSPQH = false;

            ReBindSPQHData();
        }

        /// <summary>
        /// 清空连接信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearAllToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
        }

        /// <summary>
        /// 清空股指期货回推信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
        }
        /// <summary>
        /// 港股右击清空按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (isClearHK)
                return;

            isClearHK = true;

            hkLogic.ClearAll();
            //hKMessageLogicBindingSource.Clear();

            //hkOrderDict.Clear();
            isClearHK = false;
            ReBindHKData();
        }

        private void clearAllFinalStateOrderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (isClearHK)
                return;

            //isClearHK = true;
            //var list = hkLogic.ClearAllFinalStateOrder();
            //xHMessageLogicBindingSource.Clear();
            //foreach (var message in list)
            //{
            //    hKMessageLogicBindingSource.Remove(message);
            //}

            isClearHK = false;
            ReBindHKData();
        }

        /// <summary>
        /// 清空港股回推信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            listBox5.Items.Clear();
        }

        /// <summary>
        /// 清空商品期货回推信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cleasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox6.Items.Clear();
        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gridVidw"></param>
        /// <param name="e"></param>
        private void SetGridNumber(DataGridView gridVidw, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(gridVidw.RowHeadersDefaultCellStyle.ForeColor);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(CultureInfo.CurrentUICulture),
                                  gridVidw.DefaultCellStyle.Font, b, e.RowBounds.Location.X + 20,
                                  e.RowBounds.Location.Y + 4);
        }
        /// <summary>
        /// 释放现货股票代码文本框的时候触发时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCode_KeyUp(object sender, KeyEventArgs e)
        {
            //    if (!loadCodeSuccess)
            //        return;

            //    if (!CodeManager.Isvalidkey(e.KeyCode))
            //    {
            //        if (e.KeyCode != Keys.Back)
            //            return;
            //    }

            ////    txtCode.Text = txtCode.Text.ToUpper();
            //    lbCodes.Visible = true;
            //   // CodeManager.FillList(lbCodes, txtCode.Text);
            //    if (lbCodes.Items.Count > 0)
            //    {
            //        lbCodes.SelectedIndex = 0;
            //        lbCodes.Focus();
            //    }
        }
        /// <summary>
        ///  改单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifyQuery_Click(object sender, EventArgs e)
        {
            dgvModifyList.AutoGenerateColumns = false;
            errPro.Clear();
            DateTime? start = null;
            DateTime? end = null;
            if (chkModifyTime.Checked)
            {
                start = dtpModifyStart.Value;
                end = dtpModifyEnd.Value;
            }
            string strMessage = "";
            if (cmbQueryType.SelectedIndex == -1)
            {
                cmbQueryType.SelectedIndex = 0;
            }
            int selectType = cmbQueryType.SelectedIndex;
            List<HK_HistoryModifyOrderRequestInfo> list = wcfLogic.QueryHKModifyOrderRequest(txtModifyEntrustNumber.Text, start, end, ref strMessage, selectType);
            if (list == null)
            {
                list = new List<HK_HistoryModifyOrderRequestInfo>();
            }

            dgvModifyList.DataSource = list;

            if (!string.IsNullOrEmpty(strMessage))
            {
                errPro.SetError(btnModifyQuery, strMessage);
            }
        }
        /// <summary>
        /// 获取被双击的列的详细信息显示在文本框中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvModifyList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dgvModifyList.SelectedRows)
            {
                HK_HistoryModifyOrderRequestInfo message = row.DataBoundItem as HK_HistoryModifyOrderRequestInfo;
                if (message == null)
                    continue;
                //txtHKHistroyEnNo.Text = message.EntrustNumber;
                txtModifyEntrustNumber.Text = message.EntrustNubmer;
            }
        }
        /// <summary>
        /// 港股综合查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryMarketValue_Click(object sender, EventArgs e)
        {
            dgvMarketValue.AutoGenerateColumns = false;
            string password = ServerConfig.PassWord;
            string mess = "";
            dgvMarketValue.DataSource = wcfLogic.QueryHKHoldMarketValue(txtMarketValuCode.Text.Trim(), password);
            dgvMarketValue.DataSource = wcfLogic.QueryMarketValueHKHold(txtMarketValuCode.Text.Trim(), ref mess);

        }
        /// <summary>
        /// 双击港股综合信息列表是触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvMarketValue_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dgvMarketValue.SelectedRows)
            {
                HKMarketValue message = row.DataBoundItem as HKMarketValue;
                if (message == null)
                    continue;

                txtMarketValuCode.Text = message.Code;
            }

        }
        /// <summary>
        /// 现货资金查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTotalCapital_Click(object sender, EventArgs e)
        {
            string msg = "";
            List<HKCapitalEntity> list = new List<HKCapitalEntity>();
            int x = cmbCurrencyType.SelectedIndex;
            HKCapitalEntity entry = new HKCapitalEntity();

            if (x == 0)
            {
                entry = wcfLogic.QueryHKTotalCapital(Types.CurrencyType.RMB, ref msg);
            }
            else if (x == 1)
            {
                entry = wcfLogic.QueryHKTotalCapital(Types.CurrencyType.HK, ref msg);
            }
            else if (x == 2)
            {
                entry = wcfLogic.QueryHKTotalCapital(Types.CurrencyType.US, ref msg);
            }
            //   HKCapitalEntity entry = wcfLogic.QueryHKTotalCapital((Types.CurrencyType)cmbCurrencyType.SelectedIndex + 1, ref msg);
            if (entry != null)
            {
                list.Add(entry);
            }
            dgvTotalCapital.DataSource = list;

        }
        /// <summary>
        /// 期货查询市值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryMarketValu_Click(object sender, EventArgs e)
        {
            dgQHMarketValue.AutoGenerateColumns = false;
            string mess = "";
            dgQHMarketValue.DataSource = wcfLogic.QueryMarketValueQHHold(txtQHMarketValue.Text.Trim(), 6, ref mess);
        }
        #endregion 港股查询

        #region Process Push Back

        public void ProcessXHBack(StockDealOrderPushBack drsip)
        {
            var tet = drsip.StockOrderEntity;
            var deals = drsip.StockDealList;

            string format =
                "<--PushBack[EntrustNumber={0},Code={1},TradeAmount={2},CancelAmount={3},OrderStatusId={4},OrderMessage={5},DealsCount={6}]  Time={7}";
            string desc = string.Format(format, tet.EntrustNumber, tet.SpotCode, tet.TradeAmount, tet.CancelAmount,
                                        Utils.GetOrderStateMsg(tet.OrderStatusId), tet.OrderMessage,
                                        deals.Count, DateTime.Now.ToLongTimeString());
            LogHelper.WriteDebug(desc);

            string dealsDesc = GetXHDealsDesc(deals);

            WriteXHMsg(desc);

            xhLogic.ProcessPushBack(drsip);
        }

        private string GetXHDealsDesc(List<StockPushDealEntity> deals)
        {
            StringBuilder sb = new StringBuilder();
            string line = "\n";
            foreach (var deal in deals)
            {
                sb.Append(line);
                sb.Append("     ");
            }


            return sb.ToString();
        }

        public void ProcessHKBack(HKDealOrderPushBack drsip)
        {
            var tet = drsip.HKOrderEntity;
            var deals = drsip.HKDealList;

            string format =
                "<--PushBack[EntrustNumber={0},Code={1},TradeAmount={2},CancelAmount={3},OrderStatusId={4},OrderMessage={5},DealsCount={6},IsModifyOrder={7},ModifyOrderNumber={8}] Time={9} ";
            string desc = string.Format(format, tet.EntrustNumber, tet.Code, tet.TradeAmount, tet.CancelAmount,
                                        Utils.GetOrderStateMsg(tet.OrderStatusId), tet.OrderMessage,
                                        deals.Count, tet.IsModifyOrder, tet.ModifyOrderNumber, DateTime.Now.ToLongTimeString());

            LogHelper.WriteDebug(desc);


            //写委托记录
            WriteHKMsg(desc);
            //成交记录 
            string dealsDesc = GetHKDealsDesc(deals);


            hkLogic.ProcessPushBack(drsip);
        }

        private string GetHKDealsDesc(List<HKPushDealEntity> deals)
        {
            StringBuilder sb = new StringBuilder("");
            if (deals == null || deals.Count <= 0)
            {
                return sb.ToString();
            }
            foreach (var deal in deals)
            {
                string format =
    "<--HKTradeInfo-PushBack[TradeNumber={0},TradeAmount={1},TradePrice={2},TradeTypeId={3},TradeTime={4}] Time={5} ";
                string desc = string.Format(format, deal.TradeNumber, deal.TradeAmount, deal.TradePrice, deal.TradeTypeId, deal.TradeTime, DateTime.Now.ToLongTimeString());

                WriteHKMsg(desc);
            }
            return sb.ToString();
        }

        public void ProcessHKModifyOrderBack(HKModifyOrderPushBack back)
        {
            string format =
                "<--ModifyBack[TraderID={0},IsSuccess={1},Message={2},OriginalNumber={3},NewNumber={4}]  Time={5}";
            string desc = string.Format(format, back.TradeID, back.IsSuccess, back.Message, back.OriginalRequestNumber,
                                        back.NewRequestNumber, DateTime.Now.ToLongTimeString());

            LogHelper.WriteDebug(desc);

            WriteHKMsg(desc);

            hkLogic.ProcessModifyBack(back);
        }

        public void ProcessSPQHBack(FutureDealOrderPushBack drmip)
        {
            var tet = drmip.StockIndexFuturesOrde;
            var deals = drmip.FutureDealList;

            string format =
                "<--PushBack[EntrustNumber={0},Code={1},TradeAmount={2},CancelAmount={3},OrderStatusId={4},OrderMessage={5},DealsCount={6}]  Time={7}";
            string desc = string.Format(format, tet.EntrustNumber, tet.ContractCode, tet.TradeAmount, tet.CancelAmount,
                                        Utils.GetOrderStateMsg(tet.OrderStatusId), "",
                                        deals.Count, DateTime.Now.ToLongTimeString());

            LogHelper.WriteDebug(desc);

            string dealsDesc = GetGZQHDealsDesc(deals);

            WriteSPQHMsg(desc);

            spqhLogic.ProcessPushBack(drmip);
        }

        public void ProcessGZQHBack(FutureDealOrderPushBack drsifi)
        {
            var tet = drsifi.StockIndexFuturesOrde;
            var deals = drsifi.FutureDealList;

            string format =
                "<--PushBack[EntrustNumber={0},Code={1},TradeAmount={2},CancelAmount={3},OrderStatusId={4},OrderMessage={5},DealsCount={6}]  Time={7}";
            string desc = string.Format(format, tet.EntrustNumber, tet.ContractCode, tet.TradeAmount, tet.CancelAmount,
                                        Utils.GetOrderStateMsg(tet.OrderStatusId), "",
                                        deals.Count, DateTime.Now.ToLongTimeString());

            LogHelper.WriteDebug(desc);

            string dealsDesc = GetGZQHDealsDesc(deals);

            WriteGZQHMsg(desc);

            gzqhLogic.ProcessPushBack(drsifi);
        }

        private string GetGZQHDealsDesc(List<FuturePushDealEntity> entities)
        {
            StringBuilder sb = new StringBuilder();
            string line = "\n";
            foreach (var deal in entities)
            {
                sb.Append(line);
                sb.Append("     ");
            }


            return sb.ToString();
        }

        #endregion

        #region 窗体关闭事件
        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Enabled = false;
            //wcfLogic.ShutDown();
            smartPool.Shutdown();
        }

        #endregion

        #region 查询资金流水
        /// <summary>
        ///  查询资金流水
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butCapitalFlowQuery_Click(object sender, EventArgs e)
        {
            #region QueryUA_CapitalFlowFilter类的查询条件
            QueryUA_CapitalFlowFilter CapitalFlowFilter = new QueryUA_CapitalFlowFilter();
            //转账货币类型
            string currencyType = this.cmbCurrType.SelectedIndex.ToString();
            if (currencyType.Equals("0"))
            {
                CapitalFlowFilter.CurrencyType = GTA.VTS.CustomersOrders.DoCommonQuery.QueryTypeQueryCurrencyType.ALL;
            }
            else if (currencyType.Equals("1"))
            {
                CapitalFlowFilter.CurrencyType = GTA.VTS.CustomersOrders.DoCommonQuery.QueryTypeQueryCurrencyType.RMB;
            }
            else if (currencyType.Equals("2"))
            {
                CapitalFlowFilter.CurrencyType = GTA.VTS.CustomersOrders.DoCommonQuery.QueryTypeQueryCurrencyType.HK;
            }
            else if (currencyType.Equals("3"))
            {
                CapitalFlowFilter.CurrencyType = GTA.VTS.CustomersOrders.DoCommonQuery.QueryTypeQueryCurrencyType.US;
            }
            //转账金额
            string TransferAmount = this.txtTransferAmount.Text.ToString();
            decimal Amount = 0;
            if (!string.IsNullOrEmpty(TransferAmount))
            {
                try
                {
                    Amount = decimal.Parse(TransferAmount);
                    CapitalFlowFilter.CapitalAmount = Amount;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }
            //转账时间
            //this.dateTimePickerstartTime.Text = "";
            string startTime = this.dateTimePickerstartTime.Value.ToString("yyyy-MM-dd");
            string endTime = this.dateTimePickerendTime.Value.ToString("yyyy-MM-dd");
            if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime))
            {
                try
                {
                    CapitalFlowFilter.StartTime = DateTime.Parse(startTime);
                    CapitalFlowFilter.EndTime = DateTime.Parse(endTime);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }
            //转账类型
            string TransferType = this.cmbTransferType.SelectedIndex.ToString();
            if (TransferType.Equals("0"))
            {
                CapitalFlowFilter.CapitalFlowType = 1;
            }
            else if (TransferType.Equals("1"))
            {
                CapitalFlowFilter.CapitalFlowType = 2;
            }
            else if (TransferType.Equals("2"))
            {
                CapitalFlowFilter.CapitalFlowType = 3;
            }
            #endregion QueryUA_CapitalFlowFilter类的查询条件
            #region 帐号类型查询条件
            string accountType = this.cmbaccountType.SelectedIndex.ToString();
            int account = 0;
            if (accountType.Equals("0"))
            {
                account = 1;
            }
            else if (accountType.Equals("1"))
            {
                account = 2;
            }
            else if (accountType.Equals("2"))
            {
                account = 3;
            }
            else if (accountType.Equals("3"))
            {
                account = 4;
            }
            else if (accountType.Equals("4"))
            {
                account = 5;
            }
            else if (accountType.Equals("5"))
            {
                account = 6;
            }
            else if (accountType.Equals("6"))
            {
                account = 7;
            }
            else if (accountType.Equals("7"))
            {
                account = 8;
            }
            else if (accountType.Equals("8"))
            {
                account = 9;
            }
            else
            {
                account = 0;
            }
            #endregion 帐号类型查询条件
            string msg = "";
            this.dagCapitalFlow.DataSource = wcfLogic.QueryCapitalFlow(out msg, CapitalFlowFilter, account);
        }
        #endregion 查询资金流水

        #region 清空数据
        /// <summary>
        /// 清空数据按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butCleat_Click(object sender, EventArgs e)
        {
            errPro.Clear();
            List<string> list = new List<string>();
            string Prompted = ResourceOperate.Instanse.GetResourceByKey("Prompted");

            if (string.IsNullOrEmpty(this.txtUserId.Text))
            {
                errPro.Clear();
                string UserIdError = ResourceOperate.Instanse.GetResourceByKey("UserIdError");
                errPro.SetError(txtUserId, UserIdError);
                return;
            }
            else
            {
                foreach (var item in txtUserId.Text.Split(','))
                {
                    if (DecimalTest(item))
                    {
                        list.Add(item);
                    }
                }
                string CapitalType = this.cmbCapitalType.SelectedIndex.ToString();
                if (CapitalType.Equals("-1"))
                {
                    errPro.Clear();
                    string CapitalNull = ResourceOperate.Instanse.GetResourceByKey("CapitalNull");
                    errPro.SetError(cmbCapitalType, CapitalNull);
                    return;
                }
                string money = this.cmbmoney.SelectedIndex.ToString();
                if (money.Equals("-1"))
                {
                    errPro.Clear();
                    string CurrencyError = ResourceOperate.Instanse.GetResourceByKey("Currency");
                    errPro.SetError(cmbmoney, CurrencyError);
                    return;
                }
                decimal Capital;
                if (decimal.TryParse(this.txtCapitals.Text, out Capital))
                {
                    if (Capital < 0)
                    {
                        errPro.Clear();
                        string Capitalamount = ResourceOperate.Instanse.GetResourceByKey("Capitalamount");
                        errPro.SetError(txtCapitals, Capitalamount);
                        return;
                    }
                }
                else
                {
                    errPro.Clear();
                    string Capitalamountillegal = ResourceOperate.Instanse.GetResourceByKey("Capitalamountillegal");
                    errPro.SetError(txtCapitals, Capitalamountillegal);
                    return;
                }
                #region 因需要用户名和密码，所有使用默认的用户名和密码并对密码进行加密
                string LoginName = "Admin";
                string pwd = "admin";
                string password = Encrypt.DesEncrypt(pwd, string.Empty);
                #endregion 因需要用户名和密码，所有使用默认的用户名和密码并对密码进行加密
                GTA.VTS.CustomersOrders.TransactionManageService.PersonalizationCapital personalization = new PersonalizationCapital();
                personalization.TradeID = list;
                int CapitalCode = 0;
                if (CapitalType.Equals("0"))
                {
                    CapitalCode = 0;
                }
                else if (CapitalType.Equals("1"))
                {
                    CapitalCode = 1;
                }
                else if (CapitalType.Equals("2"))
                {
                    CapitalCode = 2;
                }
                else if (CapitalType.Equals("3"))
                {
                    CapitalCode = 3;
                }
                else if (CapitalType.Equals("4"))
                {
                    CapitalCode = 4;
                }
                else if (CapitalType.Equals("5"))
                {
                    CapitalCode = 5;
                }
                personalization.PersonalType = CapitalCode;

                if (money.Equals("0"))
                {
                    personalization.SetCurrencyType = 0;
                    personalization.RMBAmount = Capital;
                    personalization.HKAmount = Capital;
                    personalization.USAmount = Capital;
                }
                else if (money.Equals("1"))
                {
                    personalization.SetCurrencyType = 1;
                    personalization.RMBAmount = Capital;
                }
                else if (money.Equals("2"))
                {
                    personalization.SetCurrencyType = 2;
                    personalization.HKAmount = Capital;
                }
                else if (money.Equals("3"))
                {
                    personalization.SetCurrencyType = 3;
                    personalization.USAmount = Capital;
                }
                string message;
                // wcfLogic.GetHighLowRangeValueByCommodityCode("00001",50);
                //调用清空数据服务接口
                bool ClearTrialData = wcfLogic.ClearTrialData(personalization, LoginName, password, out message);
                if (ClearTrialData == true)
                {
                    string Emptysuccessfully = ResourceOperate.Instanse.GetResourceByKey("Emptysuccessfully");
                    MessageBox.Show(Emptysuccessfully, Prompted, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    string Emptyfailed = ResourceOperate.Instanse.GetResourceByKey("Emptyfailed");
                    MessageBox.Show(Emptyfailed + message, Prompted, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        #endregion 清空数据

        #region 个性化资金
        /// <summary>
        /// 个性化资金按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butPersonalization_Click(object sender, EventArgs e)
        {
            errPro.Clear();
            List<string> list = new List<string>();
            string Prompted = ResourceOperate.Instanse.GetResourceByKey("Prompted");
            if (string.IsNullOrEmpty(this.txtUserId.Text))
            {
                errPro.Clear();
                string UserIdError = ResourceOperate.Instanse.GetResourceByKey("UserIdError");
                errPro.SetError(txtUserId, UserIdError);
                return;
            }
            else
            {
                foreach (var item in txtUserId.Text.Split(','))
                {
                    if (DecimalTest(item))
                    {
                        list.Add(item);
                    }
                }
                string CapitalType = this.cmbCapitalType.SelectedIndex.ToString();
                if (CapitalType.Equals("-1"))
                {
                    errPro.Clear();
                    string CapitalNull = ResourceOperate.Instanse.GetResourceByKey("CapitalNull");
                    errPro.SetError(cmbCapitalType, CapitalNull);
                    return;
                }
                string money = this.cmbmoney.SelectedIndex.ToString();
                if (money.Equals("-1"))
                {
                    errPro.Clear();
                    string CurrencyError = ResourceOperate.Instanse.GetResourceByKey("Currency");
                    errPro.SetError(cmbmoney, CurrencyError);
                    return;
                }
                decimal Capital;
                if (decimal.TryParse(this.txtCapitals.Text, out Capital))
                {
                    if (Capital < 0)
                    {
                        errPro.Clear();
                        string Capitalamount = ResourceOperate.Instanse.GetResourceByKey("Capitalamount");
                        errPro.SetError(txtCapitals, Capitalamount);
                        return;
                    }
                }
                else
                {
                    errPro.Clear();
                    string Capitalamountillegal = ResourceOperate.Instanse.GetResourceByKey("Capitalamountillegal");
                    errPro.SetError(txtCapitals, Capitalamountillegal);
                    return;
                }
                #region 因需要用户名和密码，所有使用默认的用户名和密码并对密码进行加密
                string LoginName = "Admin";
                string pwd = "admin";
                string password = Encrypt.DesEncrypt(pwd, string.Empty);
                #endregion 因需要用户名和密码，所有使用默认的用户名和密码并对密码进行加密
                GTA.VTS.CustomersOrders.TransactionManageService.PersonalizationCapital personalization = new PersonalizationCapital();
                personalization.TradeID = list;
                int CapitalCode = 0;
                if (CapitalType.Equals("0"))
                {
                    CapitalCode = 0;
                }
                else if (CapitalType.Equals("1"))
                {
                    CapitalCode = 1;
                }
                else if (CapitalType.Equals("2"))
                {
                    CapitalCode = 2;
                }
                else if (CapitalType.Equals("3"))
                {
                    CapitalCode = 3;
                }
                else if (CapitalType.Equals("4"))
                {
                    CapitalCode = 4;
                }
                else if (CapitalType.Equals("5"))
                {
                    CapitalCode = 5;
                }
                personalization.PersonalType = CapitalCode;

                if (money.Equals("0"))
                {
                    personalization.SetCurrencyType = 0;
                    personalization.RMBAmount = Capital;
                    personalization.HKAmount = Capital;
                    personalization.USAmount = Capital;
                }
                else if (money.Equals("1"))
                {
                    personalization.SetCurrencyType = 1;
                    personalization.RMBAmount = Capital;
                }
                else if (money.Equals("2"))
                {
                    personalization.SetCurrencyType = 2;
                    personalization.HKAmount = Capital;
                }
                else if (money.Equals("3"))
                {
                    personalization.SetCurrencyType = 3;
                    personalization.USAmount = Capital;
                }
                string message;
                //调用个性
                bool PersonalizationCapital = wcfLogic.PersonalizationCapital(personalization, LoginName, password, out message);
                if (PersonalizationCapital == true)
                {
                    string Personalizedsuccessful = ResourceOperate.Instanse.GetResourceByKey("Personalizedsuccessful");
                    MessageBox.Show(Personalizedsuccessful, Prompted, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    string Personalizedfailure = ResourceOperate.Instanse.GetResourceByKey("Personalizedfailure");
                    MessageBox.Show(Personalizedfailure + message, Prompted, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion 个性化资金

        #region 现货批量下单
        #region 获取Excel表格路径
        /// <summary>
        /// 选择路径按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPath_Click(object sender, EventArgs e)
        {
            string path = FileName();
            this.txtPath.Text = path;
        }
        #endregion 获取Excel表格路径
        #region 读取选定好的Excel表格中的数据
        /// <summary>
        /// 批量下单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBatchSendOrder_Click(object sender, EventArgs e)
        {
            string Path = this.txtPath.Text;
            if (!string.IsNullOrEmpty(Path))
            {
                DataSet myDataSet = new DataSet();
                myDataSet = orderSql.dataSet(Path);
                if (myDataSet != null)
                {
                    try
                    {
                        #region 获取Excel表格中数据
                        for (int i = 0; i < myDataSet.Tables[0].Rows.Count; i++)
                        {

                            string Code = myDataSet.Tables[0].Rows[i][0].ToString();
                            float OrderAmount = float.Parse(myDataSet.Tables[0].Rows[i][1].ToString());
                            int batch = int.Parse(myDataSet.Tables[0].Rows[i][2].ToString());
                            string BuySell = myDataSet.Tables[0].Rows[i][3].ToString();
                            string UnitType = myDataSet.Tables[0].Rows[i][4].ToString();
                            bool cbMarketOrder = bool.Parse(myDataSet.Tables[0].Rows[i][5].ToString());
                            string price = myDataSet.Tables[0].Rows[i][6].ToString();
                            Order(Code, OrderAmount, batch, BuySell, UnitType, cbMarketOrder, price);
                        }
                        #endregion 获取Excel表格中数据
                    }
                    catch (Exception ex)
                    {
                        string MesageError = ResourceOperate.Instanse.GetResourceByKey("MesageError");
                        MessageBox.Show(MesageError + ex);
                        LogHelper.WriteError(ex.Message, ex);
                    }
                }
                else
                {
                    string MesageData = ResourceOperate.Instanse.GetResourceByKey("MesageData");
                    MessageBox.Show(MesageData);
                }
            }
            else
            {
                errPro.Clear();
                error.Clear();
                string Mesage = ResourceOperate.Instanse.GetResourceByKey("Mesage");
                errPro.SetError(btnPath, Mesage);
            }
        }
        #endregion  读取选定好的Excel表格中的数据
        #region 现货批量下单处理方法
        /// <summary>
        /// 批量下单处理方法
        /// </summary>
        /// <param name="Code">股票代码</param>
        /// <param name="OrderAmount">数量</param>
        /// <param name="batchs">批量数</param>
        /// <param name="BuySell">买卖方向</param>
        /// <param name="UnitType">单位</param>
        /// <param name="cbMarketOrder">是否市价下单</param>
        /// <param name="price">价格</param>
        private void Order(string Code, float OrderAmount, int batchs, string BuySell, string UnitType, bool cbMarketOrder, string price)
        {
            #region 获取Excel数据并组装下单实体
            errPro.Clear();
            error.Clear();
            xhDoOrderNum = 0;
            string OrdreName = ResourceOperate.Instanse.GetResourceByKey("OrdreName");
            string Exception = ResourceOperate.Instanse.GetResourceByKey("Exception");
            string OrderPrice = ResourceOperate.Instanse.GetResourceByKey("OrderPrice");
            string IsMarketValue = ResourceOperate.Instanse.GetResourceByKey("IsMarketValue");
            string BS = ResourceOperate.Instanse.GetResourceByKey("BuySell");
            #region 组装现货下单请求实体
            StockOrderRequest order = new StockOrderRequest();
            //判断Code是否为空，如果为空则弹出错误提示框并退出
            if (!string.IsNullOrEmpty(Code))
            {
                order.Code = Code;
            }
            else
            {
                string CodeError = ResourceOperate.Instanse.GetResourceByKey("NullError");
                MessageBox.Show(OrdreName + Code + "Code" + CodeError);
                return;
            }
            if (BuySell.Equals("Buying"))
            {
                order.BuySell = Types.TransactionDirection.Buying;
            }
            else if (BuySell.Equals("Selling"))
            {
                order.BuySell = Types.TransactionDirection.Selling;
            }
            else
            {
                MessageBox.Show(OrdreName + Code + BS + Exception);
            }
            //对用户输入的Capital进行判断，判断是否为登陆时输入的现货资金帐户.
            string Capital = ServerConfig.XHCapitalAccount;
            if (Capital.Equals(this.txtCapital.Text))
            {
                order.FundAccountId = txtCapital.Text.Trim(); //"010000002302";
            }
            else
            {
                string CapitalError = ResourceOperate.Instanse.GetResourceByKey("AccountError");
                MessageBox.Show(OrdreName + Code + " Code" + CapitalError);
                return;
            }
            order.OrderAmount = OrderAmount;
            #region 判断是否为市价委托，来判断价格
            if (cbMarketOrder == false)
            {
                if (price != null && !price.Equals("0"))
                {
                    #region 获取价格上下限

                    string high;
                    string low;
                    decimal prices = 0;

                    string errMsg = "";
                    bool isSuccess = decimal.TryParse(price, out prices);
                    if (!isSuccess)
                    {
                        MessageBox.Show(OrdreName + Code + OrderPrice + Exception);
                        return;
                    }

                    //获取价格上下限
                    var highLowRange = wcfLogic.GetHighLowRangeValueByCommodityCode(Code, prices, out errMsg);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        errPro.SetError(txtPrice, errMsg);
                    }
                    if (highLowRange == null)
                    {
                        //MessageBox.Show("Can not get highlowrange object!");
                        string errMessage = ResourceOperate.Instanse.GetResourceByKey("highlowrange");
                        MessageBox.Show(OrdreName + Code + errMessage);
                        return;
                    }
                    //港股类型处理
                    if (highLowRange.RangeType == Types.HighLowRangeType.HongKongPrice)
                    {
                        var hkrv = highLowRange.HongKongRangeValue;

                        Types.TransactionDirection buySell = new Types.TransactionDirection();
                        if (BuySell.Equals("Buying"))
                        {
                            buySell = Types.TransactionDirection.Buying;
                        }
                        else if (BuySell.Equals("Selling"))
                        {
                            buySell = Types.TransactionDirection.Selling;
                        }
                        else
                        {
                            MessageBox.Show(OrdreName + Code + BS + Exception);
                        }
                        if (buySell == Types.TransactionDirection.Buying)
                        {
                            high = Utils.Round(hkrv.BuyHighRangeValue).ToString();
                            low = Utils.Round(hkrv.BuyLowRangeValue).ToString();
                        }
                        else
                        {
                            high = Utils.Round(hkrv.SellHighRangeValue).ToString();
                            low = Utils.Round(hkrv.SellLowRangeValue).ToString();
                        }
                    }
                    else
                    {
                        //其它类型处理
                        high = Utils.Round(highLowRange.HighRangeValue).ToString();
                        low = Utils.Round(highLowRange.LowRangeValue).ToString();
                    }
                    #endregion 获取价格上下限
                    #region 价格判断
                    bool SeesawPrice = ServerConfig.Price;
                    if (SeesawPrice == true)
                    {
                        order.OrderPrice = float.Parse(prices.ToString());
                        return;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(high) && !string.IsNullOrEmpty(low))
                        {
                            decimal highPrice = 0;
                            decimal lowPrice = 0;
                            if (decimal.TryParse(high, out highPrice) && decimal.TryParse(low, out lowPrice))
                            {
                                if (prices <= highPrice && prices >= lowPrice)
                                {
                                    order.OrderPrice = float.Parse(prices.ToString());
                                }
                                else
                                {
                                    string PriceErrors = ResourceOperate.Instanse.GetResourceByKey("PriceErrors");
                                    MessageBox.Show(OrdreName + Code + PriceErrors);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            string PriceErrors = ResourceOperate.Instanse.GetResourceByKey("PriceErrors");
                            MessageBox.Show(OrdreName + Code + PriceErrors);
                            return;
                        }
                    }
                    #endregion 价格判断
                }
                else
                {
                    MessageBox.Show(OrdreName + Code + OrderPrice + Exception);
                }
            }
            #endregion 判断是否为市价委托，来判断价格
            order.OrderUnitType = Utils.GetUnit(UnitType);
            if (cbMarketOrder == true)
            {
                order.OrderWay = TypesOrderPriceType.OPTMarketPrice;
            }
            else if (cbMarketOrder == false)
            {
                order.OrderWay = TypesOrderPriceType.OPTLimited;
            }
            else
            {
                MessageBox.Show(OrdreName + Code + IsMarketValue + Exception);
            }
            order.PortfoliosId = "p1";
            //判断TraderID是否正确
            string id = ServerConfig.TraderID;
            if (id.Equals(this.txtTradeID.Text))
            {
                order.TraderId = txtTradeID.Text.Trim(); //"23";
            }
            else
            {
                string TraderIDError = ResourceOperate.Instanse.GetResourceByKey("TraderIDError");
                MessageBox.Show(TraderIDError);
                return;
            }
            order.TraderPassword = "";
            #endregion

            //批量下单数
            int batch = batchs;

            for (int i = 0; i < batch; i++)
            {
                //异步执行下单
                smartPool.QueueWorkItem(DoXHOrder, order);
            }
            if (batch >= 50)
            {
                int scale = batch / 50;
                btnDoOrder.Enabled = false;
                xhTimer.Interval = 1000 * scale;
                xhTimer.Elapsed += delegate
                {
                    this.Invoke(new MethodInvoker(() => { btnDoOrder.Enabled = true; }));
                    xhTimer.Enabled = false;
                };
                xhTimer.Enabled = true;
            }
            #endregion 获取Excel数据并组装下单实体
        }
        #endregion  现货批量下单处理方法
        #endregion 现货批量下单

        #region 港股批量下单
        #region 获取Excel表格路径
        /// <summary>
        /// 选择路径按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHKPath_Click(object sender, EventArgs e)
        {
            string path = FileName();
            this.txtHKPath.Text = path;
        }
        #endregion 获取Excel表格路径
        #region 读取选定好的Excel表格中的数据
        /// <summary>
        /// 批量下单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHKBatchOrder_Click(object sender, EventArgs e)
        {
            string Path = this.txtHKPath.Text;
            if (!string.IsNullOrEmpty(Path))
            {
                DataSet myDataSet = new DataSet();
                myDataSet = orderSql.dataSet(Path);
                if (myDataSet != null)
                {
                    try
                    {
                        for (int i = 0; i < myDataSet.Tables[0].Rows.Count; i++)
                        {

                            string Code = myDataSet.Tables[0].Rows[i][0].ToString();
                            float OrderAmount = float.Parse(myDataSet.Tables[0].Rows[i][1].ToString());
                            int batch = int.Parse(myDataSet.Tables[0].Rows[i][2].ToString());
                            string BuySell = myDataSet.Tables[0].Rows[i][3].ToString();
                            string UnitType = myDataSet.Tables[0].Rows[i][4].ToString();
                            string cbMarketOrder = myDataSet.Tables[0].Rows[i][5].ToString();
                            string price = myDataSet.Tables[0].Rows[i][6].ToString();
                            HKOrder(Code, OrderAmount, batch, BuySell, UnitType, cbMarketOrder, price);

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("您选择的港股模板出现如下问题" + ex);
                        LogHelper.WriteError(ex.Message, ex);
                    }
                }
                else
                {
                    MessageBox.Show("您选择的港股模板无法读取数据");
                }
            }
            else
            {
                errPro.Clear();
                error.Clear();
                errPro.SetError(btnHKPath, "请先选择导入模板");
            }
        }
        #endregion  读取选定好的Excel表格中的数据
        #region 港股批量下单处理方法
        /// <summary>
        /// 批量下单处理方法
        /// </summary>
        /// <param name="Code">股票代码</param>
        /// <param name="OrderAmount">数量</param>
        /// <param name="batchs">批量数</param>
        /// <param name="BuySell">买卖方向</param>
        /// <param name="UnitType">单位</param>
        /// <param name="cbMarketOrder">是否市价下单</param>
        /// <param name="price">价格</param>
        private void HKOrder(string Code, float OrderAmount, int batchs, string BuySell, string UnitType, string cbMarketOrder, string price)
        {
            errPro.Clear();
            error.Clear();
            string OrdreName = ResourceOperate.Instanse.GetResourceByKey("OrdreName");
            string Exception = ResourceOperate.Instanse.GetResourceByKey("Exception");
            string OrderPrice = ResourceOperate.Instanse.GetResourceByKey("OrderPrice");
            string BS = ResourceOperate.Instanse.GetResourceByKey("BuySell");
            hkDoOrderNum = 0;
            HKOrderRequest order = new HKOrderRequest();
            //判断Code是否为空，如果为空则弹出错误提示框并退出
            if (!string.IsNullOrEmpty(Code))
            {
                order.Code = Code;
            }

            else
            {
                errPro.Clear();
                string CodeError = ResourceOperate.Instanse.GetResourceByKey("NullError");
                errPro.SetError(cbHKCode, "Code" + CodeError);
                return;
            }
            Types.TransactionDirection buySells = new Types.TransactionDirection();
            if (BuySell.Equals("Buying"))
            {
                order.BuySell = Types.TransactionDirection.Buying;
                buySells = Types.TransactionDirection.Buying;
            }
            else if (BuySell.Equals("Selling"))
            {
                order.BuySell = Types.TransactionDirection.Selling;
                buySells = Types.TransactionDirection.Selling;
            }
            else
            {
                MessageBox.Show(OrdreName + Code + BS + Exception);
            }
            string Capital = ServerConfig.HKCapitalAccount;
            if (Capital.Equals(txtHKCapital.Text.Trim()))
            {
                order.FundAccountId = txtHKCapital.Text.Trim(); //"010000002302";
            }
            else
            {
                errPro.Clear();
                string CapitalError = ResourceOperate.Instanse.GetResourceByKey("AccountError");
                errPro.SetError(txtHKCapital, "Capita" + CapitalError);
                return;
            }
            order.OrderAmount = OrderAmount;

            if (!string.IsNullOrEmpty(price))
            {
                #region 获取上下限
                string high;
                string low;
                decimal prices = 0;
                string errMsg = "";
                bool isSuccess = decimal.TryParse(price, out prices);
                if (!isSuccess)
                {
                    MessageBox.Show(OrdreName + Code + OrderPrice + Exception);
                    return;
                }
                Types.HKPriceType priceType = new Types.HKPriceType();
                //LO限价盘
                //ELO增强限价盘
                //SLO特别限价盘
                if (cbMarketOrder.Equals("LO"))
                {
                    order.OrderWay = Types.HKPriceType.LO;
                    priceType = Types.HKPriceType.LO;
                }
                else if (cbMarketOrder.Equals("ELO"))
                {
                    order.OrderWay = Types.HKPriceType.ELO;
                    priceType = Types.HKPriceType.ELO;
                }
                else
                {

                    order.OrderWay = Types.HKPriceType.SLO;
                    priceType = Types.HKPriceType.SLO;
                }

                //获取价格上下限
                var highLowRange = wcfLogic.GetHKHighLowRangeValueByCommodityCode(Code, prices, priceType, buySells, out errMsg);
                if (!string.IsNullOrEmpty(errMsg))
                {
                    errPro.SetError(txtHKPrice, errMsg);
                }
                if (highLowRange == null)
                {
                    //MessageBox.Show("Can not get highlowrange object!");
                    string errMessage = ResourceOperate.Instanse.GetResourceByKey("highlowrange");
                    MessageBox.Show(OrdreName + Code + errMessage);
                    return;
                }
                //港股类型处理
                if (highLowRange.RangeType == Types.HighLowRangeType.HongKongPrice)
                {
                    var hkrv = highLowRange.HongKongRangeValue;

                    Types.TransactionDirection buySell = new Types.TransactionDirection();
                    if (BuySell.Equals("Buying"))
                    {
                        buySell = Types.TransactionDirection.Buying;
                    }
                    else if (BuySell.Equals("Selling"))
                    {
                        buySell = Types.TransactionDirection.Selling;
                    }
                    else
                    {
                        MessageBox.Show(OrdreName + Code + BS + Exception);
                    }
                    if (buySell == Types.TransactionDirection.Buying)
                    {
                        high = Utils.Round(hkrv.BuyHighRangeValue).ToString();
                        low = Utils.Round(hkrv.BuyLowRangeValue).ToString();
                    }
                    else
                    {
                        high = Utils.Round(hkrv.SellHighRangeValue).ToString();
                        low = Utils.Round(hkrv.SellLowRangeValue).ToString();
                    }
                }
                else
                {
                    //其它类型处理
                    high = Utils.Round(highLowRange.HighRangeValue).ToString();
                    low = Utils.Round(highLowRange.LowRangeValue).ToString();
                }
                #endregion 获取上下限
                #region 价格处理
                bool SeesawPrice = ServerConfig.Price;
                if (SeesawPrice == true)
                {
                    order.OrderPrice = float.Parse(prices.ToString());
                    return;
                }
                else
                {
                    if (!string.IsNullOrEmpty(high) && !string.IsNullOrEmpty(low))
                    {
                        decimal highPrice = 0;
                        decimal lowPrice = 0;
                        if (decimal.TryParse(high, out highPrice) && decimal.TryParse(low, out lowPrice))
                        {
                            if (prices <= highPrice && prices >= lowPrice)
                            {
                                order.OrderPrice = float.Parse(prices.ToString());
                            }
                            else
                            {
                                string PriceErrors = ResourceOperate.Instanse.GetResourceByKey("PriceErrors");
                                MessageBox.Show(OrdreName + Code + PriceErrors);
                                return;
                            }
                        }
                    }
                    else
                    {
                        string PriceErrors = ResourceOperate.Instanse.GetResourceByKey("PriceErrors");
                        MessageBox.Show(OrdreName + Code + PriceErrors);
                        return;
                    }
                }
                #endregion 价格处理
            }
            else
            {
                errPro.Clear();
                string HKPriceError = ResourceOperate.Instanse.GetResourceByKey("NullError");
                errPro.SetError(txtHKPrice, "Price" + HKPriceError);
                return;
            }
            order.OrderUnitType = Utils.GetUnit(UnitType);
            //order.OrderWay = cbMarketOrder.Checked ? TypesOrderPriceType.OPTMarketPrice : TypesOrderPriceType.OPTLimited;
            order.PortfoliosId = "p1";
            //判断TraderID是否正确
            string TraderID = ServerConfig.TraderID;
            if (TraderID.Equals(this.txtHKTraderID.Text))
            {
                order.TraderId = txtHKTraderID.Text.Trim(); //"23";
            }
            else
            {
                errPro.Clear();
                string HKTraderIDError = ResourceOperate.Instanse.GetResourceByKey("TraderIDError");
                errPro.SetError(txtHKTraderID, HKTraderIDError);
                return;
            }
            order.TraderPassword = "";

            int batch = int.Parse(txtHKBatch.Text.Trim());

            for (int i = 0; i < batch; i++)
            {
                smartPool.QueueWorkItem(DoHKOrder, order);
            }

            if (batchs >= 50)
            {
                int scale = batch / 50;
                btnHKSendOrder.Enabled = false;
                hkTimer.Interval = 1000 * scale;
                hkTimer.Elapsed += delegate
                {
                    this.Invoke(new MethodInvoker(() => { btnHKSendOrder.Enabled = true; }));
                    hkTimer.Enabled = false;
                };
                hkTimer.Enabled = true;
            }
        }
        #endregion  港股批量下单处理方法

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        #endregion 港股批量下单

        #region 股指期货批量下单
        /// <summary>
        /// 股指期货批量下单路径选择按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGZQHPath_Click(object sender, EventArgs e)
        {
            string path = FileName();
            this.txtGZQHPath.Text = path;
        }
        /// <summary>
        /// 股指期货批量下单按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGZQHBatchOrder_Click(object sender, EventArgs e)
        {
            string Path = this.txtGZQHPath.Text;
            if (!string.IsNullOrEmpty(Path))
            {
                DataSet myDataSet = new DataSet();
                myDataSet = orderSql.dataSet(Path);
                if (myDataSet != null)
                {
                    try
                    {
                        for (int i = 0; i < myDataSet.Tables[0].Rows.Count; i++)
                        {

                            string Code = myDataSet.Tables[0].Rows[i][0].ToString();
                            float OrderAmount = float.Parse(myDataSet.Tables[0].Rows[i][1].ToString());
                            int batch = int.Parse(myDataSet.Tables[0].Rows[i][2].ToString());
                            string BuySell = myDataSet.Tables[0].Rows[i][3].ToString();
                            string UnitType = myDataSet.Tables[0].Rows[i][4].ToString();
                            bool cbMarketOrder = bool.Parse(myDataSet.Tables[0].Rows[i][5].ToString());
                            string OpenClose = myDataSet.Tables[0].Rows[i][6].ToString();
                            string price = myDataSet.Tables[0].Rows[i][7].ToString();
                            GZQHOrder(Code, OrderAmount, batch, BuySell, UnitType, cbMarketOrder, OpenClose, price);

                        }
                    }
                    catch (Exception ex)
                    {
                        string MesageError = ResourceOperate.Instanse.GetResourceByKey("MesageError");
                        MessageBox.Show(MesageError + ex);
                        LogHelper.WriteError(ex.Message, ex);
                    }
                }
                else
                {
                    string MesageData = ResourceOperate.Instanse.GetResourceByKey("MesageData");
                    MessageBox.Show(MesageData);
                }
            }
            else
            {
                errPro.Clear();
                error.Clear();
                string Mesage = ResourceOperate.Instanse.GetResourceByKey("Mesage");
                errPro.SetError(btnGZQHPath, Mesage);
            }
        }

        /// <summary>
        /// 股指期货下单处理方法
        /// </summary>
        /// <param name="Code">代码</param>
        /// <param name="OrderAmount">数量</param>
        /// <param name="batch">批量数</param>
        /// <param name="BuySell">买卖方向</param>
        /// <param name="UnitType">单位类型</param>
        /// <param name="cbMarketOrder">委托类型</param>
        /// <param name="OpenClose">开平方向</param>
        /// <param name="price">价格</param>
        private void GZQHOrder(string Code, float OrderAmount, int batch, string BuySell, string UnitType, bool cbMarketOrder, string OpenClose, string price)
        {
            string errMsg = "";
            string high;
            string low;
            errPro.Clear();
            error.Clear();
            string OrdreName = ResourceOperate.Instanse.GetResourceByKey("OrdreName");
            string Exception = ResourceOperate.Instanse.GetResourceByKey("Exception");
            string OrderPrice = ResourceOperate.Instanse.GetResourceByKey("OrderPrice");
            string BS = ResourceOperate.Instanse.GetResourceByKey("BuySell");
            string IsMarketValue = ResourceOperate.Instanse.GetResourceByKey("IsMarketValue");
            StockIndexFuturesOrderRequest order = new StockIndexFuturesOrderRequest();
            //判断Contract是否为空，如果为空则弹出错误提示框并退出
            if (!string.IsNullOrEmpty(Code))
            {
                order.Code = Code;
            }
            else
            {
                errPro.Clear();
                string CodeError = ResourceOperate.Instanse.GetResourceByKey("NullError");
                errPro.SetError(cbGZCode, "Code" + CodeError);
                return;
            }
            if (BuySell.Equals("Buying"))
            {
                order.BuySell = Types.TransactionDirection.Buying;
                //  buySells = Types.TransactionDirection.Buying;
            }
            else if (BuySell.Equals("Selling"))
            {
                order.BuySell = Types.TransactionDirection.Selling;
                //   buySells = Types.TransactionDirection.Selling;
            }
            else
            {
                MessageBox.Show(OrdreName + Code + BS + Exception);
            }
            order.FundAccountId = txtGZCapital.Text.Trim(); //"010000002306";

            order.OrderAmount = OrderAmount;

            if (cbMarketOrder == false)
            {
                #region  市价委托
                if (!string.IsNullOrEmpty(price))
                {
                    //判断Price是否等于零，如果为空则弹出错误提示框并退出
                    decimal prices = 0;
                    //if (DecimalTest(this.txtGZPrice.Text))
                    //{
                    if (decimal.TryParse(price, out prices))
                    {
                        if (prices == 0)
                        {
                            errPro.Clear();
                            string GZPricezeroError = ResourceOperate.Instanse.GetResourceByKey("zero");
                            errPro.SetError(txtGZPrice, "Price" + GZPricezeroError);
                            return;
                        }
                    }
                    else
                    {
                        errPro.Clear();
                        string GZPriceDataError = ResourceOperate.Instanse.GetResourceByKey("Dataillegal");
                        errPro.SetError(txtGZPrice, "Price" + GZPriceDataError);
                        return;
                    }
                    #region 获取价格上下限
                    //var highLowRange = wcfLogic.GetHighLowRangeValueByCommodityCode(Code, prices, out errMsg);
                    var highLowRange = wcfLogic.GetHighLowRangeValueByCommodityCode(Code, prices, out errMsg);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        errPro.SetError(txtGZPrice, errMsg);
                    }
                    if (highLowRange == null)
                    {
                        //MessageBox.Show("Can not get highlowrange object!");
                        string errMessage = ResourceOperate.Instanse.GetResourceByKey("highlowrange");
                        // string errors = "Can not get highlowrange object!";
                        error.Clear();
                        MessageBox.Show(OrdreName + errMessage);
                        return;
                    }

                    if (highLowRange.RangeType == Types.HighLowRangeType.HongKongPrice) //港股类型处理
                    {
                        var hkrv = highLowRange.HongKongRangeValue;

                        Types.TransactionDirection buySell = new Types.TransactionDirection();
                        if (BuySell.Equals("Buying"))
                        {
                            buySell = Types.TransactionDirection.Buying;
                        }
                        else if (BuySell.Equals("Selling"))
                        {
                            buySell = Types.TransactionDirection.Selling;
                        }
                        else
                        {
                            MessageBox.Show(OrdreName + Code + BS + Exception);
                        }
                        if (buySell == Types.TransactionDirection.Buying)
                        {


                            high = Utils.Round(hkrv.BuyHighRangeValue).ToString();
                            low = Utils.Round(hkrv.BuyLowRangeValue).ToString();
                        }
                        else
                        {
                            high = Utils.Round(hkrv.SellHighRangeValue).ToString();
                            low = Utils.Round(hkrv.SellLowRangeValue).ToString();
                        }
                    }
                    else //其它类型处理
                    {
                        high = Utils.Round(highLowRange.HighRangeValue).ToString();
                        low = Utils.Round(highLowRange.LowRangeValue).ToString();
                    }
                    #endregion 获取价格上下限
                    #region 价格判断
                    bool SeesawPrice = ServerConfig.Price;
                    float highs = 0;
                    float lows = 0;
                    float pric = 0;
                    if (float.TryParse(high, out highs) && float.TryParse(low, out lows) &&
                        float.TryParse(price, out pric))
                    {
                        if (SeesawPrice == true)
                        {
                            order.OrderPrice = pric;
                            return;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(high) && !string.IsNullOrEmpty(low))
                            {

                                if (pric <= highs && pric >= lows)
                                {
                                    order.OrderPrice = pric;
                                }
                                else
                                {
                                    string PriceErrors = ResourceOperate.Instanse.GetResourceByKey("PriceErrors");
                                    MessageBox.Show(OrdreName + PriceErrors);
                                    return;
                                }
                            }
                            else
                            {
                                string HighLowException = ResourceOperate.Instanse.GetResourceByKey("HighLowException");
                                MessageBox.Show(HighLowException);
                                return;
                            }
                        }
                    }
                    else
                    {
                        string HighLow = ResourceOperate.Instanse.GetResourceByKey("HighLow");
                        MessageBox.Show(HighLow);
                        return;
                    }
                    #endregion 价格判断
                }
                else
                {
                    errPro.Clear();
                    string GZPriceError = ResourceOperate.Instanse.GetResourceByKey("NullError");
                    MessageBox.Show(OrdreName + "Price" + GZPriceError);
                    return;
                }
                #endregion  市价委托
            }
            order.OrderUnitType = Utils.GetUnit(UnitType);
            if (cbMarketOrder == true)
            {
                order.OrderWay = TypesOrderPriceType.OPTMarketPrice;
            }
            else if (cbMarketOrder == false)
            {
                order.OrderWay = TypesOrderPriceType.OPTLimited;
            }
            else
            {
                MessageBox.Show(OrdreName + Code + IsMarketValue + Exception);
            }

            order.PortfoliosId = "p2";
            order.TraderId = txtGZTraderID.Text.Trim(); //"23";
            order.TraderPassword = "";
            order.OpenCloseType = Utils.GetFutureOpenCloseType(OpenClose);

            gzqhDoOrderNum = 0;

            for (int i = 0; i < batch; i++)
            {
                smartPool.QueueWorkItem(DoGZQHOrder, order);
            }

            if (batch >= 50)
            {
                int scale = batch / 50;
                btnGZSendOrder.Enabled = false;
                gzqhTimer.Interval = 1000 * scale;
                gzqhTimer.Elapsed += delegate
                {
                    this.Invoke(new MethodInvoker(() => { btnGZSendOrder.Enabled = true; }));
                    gzqhTimer.Enabled = false;
                };
                gzqhTimer.Enabled = true;
            }
        }
        #endregion 股指期货批量下单

        #region 商品期货批量下单
        /// <summary>
        /// 商品期货批量下单选择路径按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSPQHPath_Click(object sender, EventArgs e)
        {
            string path = FileName();
            this.txtSPQHPath.Text = path;
        }
        /// <summary>
        /// 商品期货批量下单按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSPQHBatchOrder_Click(object sender, EventArgs e)
        {
            string Path = this.txtSPQHPath.Text;
            if (!string.IsNullOrEmpty(Path))
            {
                DataSet myDataSet = new DataSet();
                myDataSet = orderSql.dataSet(Path);
                if (myDataSet != null)
                {
                    try
                    {
                        for (int i = 0; i < myDataSet.Tables[0].Rows.Count; i++)
                        {

                            string Code = myDataSet.Tables[0].Rows[i][0].ToString();
                            float OrderAmount = float.Parse(myDataSet.Tables[0].Rows[i][1].ToString());
                            int batch = int.Parse(myDataSet.Tables[0].Rows[i][2].ToString());
                            string BuySell = myDataSet.Tables[0].Rows[i][3].ToString();
                            string UnitType = myDataSet.Tables[0].Rows[i][4].ToString();
                            string OpenClose = myDataSet.Tables[0].Rows[i][5].ToString();
                            string price = myDataSet.Tables[0].Rows[i][6].ToString();
                            SPQHOrder(Code, OrderAmount, batch, BuySell, UnitType, OpenClose, price);

                        }
                    }
                    catch (Exception ex)
                    {
                        string MesageError = ResourceOperate.Instanse.GetResourceByKey("MesageError");
                        MessageBox.Show(MesageError + ex);
                        LogHelper.WriteError(ex.Message, ex);
                    }
                }
                else
                {
                    string MesageData = ResourceOperate.Instanse.GetResourceByKey("MesageData");
                    MessageBox.Show(MesageData);
                }
            }
            else
            {
                errPro.Clear();
                error.Clear();
                string Mesage = ResourceOperate.Instanse.GetResourceByKey("Mesage");
                errPro.SetError(btnSPQHPath, Mesage);
            }
        }
        /// <summary>
        /// 商品期货批量下单处理方法
        /// </summary>
        /// <param name="Code">代码</param>
        /// <param name="OrderAmount">数量</param>
        /// <param name="batch">批量数</param>
        /// <param name="BuySell">买卖方向</param>
        /// <param name="UnitType">类型</param>
        /// <param name="OpenClose">开平方向</param>
        /// <param name="price">价格</param>
        private void SPQHOrder(string Code, float OrderAmount, int batch, string BuySell, string UnitType, string OpenClose, string price)
        {
            errPro.Clear();
            error.Clear();
            string errMsg = "";
            string high;
            string low;
            string OrdreName = ResourceOperate.Instanse.GetResourceByKey("OrdreName");
            string Exception = ResourceOperate.Instanse.GetResourceByKey("Exception");
            string OrderPrice = ResourceOperate.Instanse.GetResourceByKey("OrderPrice");
            string BS = ResourceOperate.Instanse.GetResourceByKey("BuySell");
            MercantileFuturesOrderRequest order = new MercantileFuturesOrderRequest();
            //判断Contract是否为空，如果为空则弹出错误提示框并退出
            if (!string.IsNullOrEmpty(Code))
            {
                order.Code = Code;
            }
            else
            {
                errPro.Clear();
                string CodeError = ResourceOperate.Instanse.GetResourceByKey("NullError");
                errPro.SetError(cbSPCode, "Code" + CodeError);
                return;
            }
            if (BuySell.Equals("Buying"))
            {
                order.BuySell = Types.TransactionDirection.Buying;
                //  buySells = Types.TransactionDirection.Buying;
            }
            else if (BuySell.Equals("Selling"))
            {
                order.BuySell = Types.TransactionDirection.Selling;
                //   buySells = Types.TransactionDirection.Selling;
            }
            else
            {
                MessageBox.Show(OrdreName + Code + BS + Exception);
            }

            order.FundAccountId = txtSPQHCapital.Text.Trim(); //"010000002306";
            order.OrderAmount = OrderAmount;

            //if (!cbGZMarket.Checked)
            //{
            if (!string.IsNullOrEmpty(price))
            {
                //判断Price是否等于零，如果为空则弹出错误提示框并退出
                decimal prices = 0;
                if (decimal.TryParse(price, out prices))
                {
                    if (prices == 0)
                    {
                        errPro.Clear();
                        string GZPricezeroError = ResourceOperate.Instanse.GetResourceByKey("zero");
                        errPro.SetError(txtSPQHPrice, "Price" + GZPricezeroError);
                        return;
                    }
                }
                else
                {
                    errPro.Clear();
                    string GZPriceDataError = ResourceOperate.Instanse.GetResourceByKey("Dataillegal");
                    errPro.SetError(txtSPQHPrice, "Price" + GZPriceDataError);
                    return;
                }
                #region 获取价格上下限
                var highLowRange = wcfLogic.GetHighLowRangeValueByCommodityCode(Code, prices, out errMsg);

                if (highLowRange == null)
                {
                    string errMessage = ResourceOperate.Instanse.GetResourceByKey("highlowrange");
                    // string errors = "Can not get highlowrange object!";
                    error.Clear();
                    error.SetError(txtSPQHHigh, errMessage);
                    //MessageBox.Show("Can not get highlowrange object!");
                    return;
                }
                if (!string.IsNullOrEmpty(errMsg))
                {
                    errPro.SetError(txtSPQHPrice, errMsg);
                }
                if (highLowRange.RangeType == Types.HighLowRangeType.HongKongPrice) //港股类型处理
                {
                    var hkrv = highLowRange.HongKongRangeValue;

                    Types.TransactionDirection buySell = new Types.TransactionDirection();
                    if (BuySell.Equals("Buying"))
                    {
                        buySell = Types.TransactionDirection.Buying;
                    }
                    else if (BuySell.Equals("Selling"))
                    {
                        buySell = Types.TransactionDirection.Selling;
                    }
                    else
                    {
                        MessageBox.Show(OrdreName + Code + BS + Exception);
                    }
                    if (buySell == Types.TransactionDirection.Buying)
                    {
                        high = Utils.Round(hkrv.BuyHighRangeValue).ToString();
                        low = Utils.Round(hkrv.BuyLowRangeValue).ToString();
                    }
                    else
                    {
                        high = Utils.Round(hkrv.SellHighRangeValue).ToString();
                        low = Utils.Round(hkrv.SellLowRangeValue).ToString();
                    }
                }
                else //其它类型处理
                {
                    high = Utils.Round(highLowRange.HighRangeValue).ToString();
                    low = Utils.Round(highLowRange.LowRangeValue).ToString();
                }
                #endregion 获取价格上下限
                #region 价格判断
                bool SeesawPrice = ServerConfig.Price;
                float highs = 0;
                float lows = 0;
                float pric = 0;
                if (float.TryParse(high, out highs) && float.TryParse(low, out lows) &&
                        float.TryParse(price, out pric))
                {
                    if (SeesawPrice == true)
                    {
                        order.OrderPrice = pric;
                        return;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(high) && !string.IsNullOrEmpty(low))
                        {
                            if (pric <= highs && pric >= lows)
                            {
                                order.OrderPrice = pric;
                            }

                        }
                        else
                        {
                            string PriceErrors = ResourceOperate.Instanse.GetResourceByKey("PriceErrors");
                            errPro.SetError(txtSPQHPrice, PriceErrors);
                            return;
                        }
                    }
                }
                else
                {
                    string HighLow = ResourceOperate.Instanse.GetResourceByKey("HighLow");
                    MessageBox.Show(HighLow);
                    return;
                }
                #endregion 价格判断
            }
            else
            {
                string HighLowException = ResourceOperate.Instanse.GetResourceByKey("HighLowException");
                MessageBox.Show(HighLowException);
                return;
            }
            //}
            order.OrderUnitType = Utils.GetUnit(UnitType);
            //    order.OrderWay = cbGZMarket.Checked ? TypesOrderPriceType.OPTMarketPrice : TypesOrderPriceType.OPTLimited;
            order.OrderWay = TypesOrderPriceType.OPTLimited;
            order.PortfoliosId = "p2";
            order.TraderId = txtSPQHTradeID.Text.Trim(); //"23";
            order.TraderPassword = "";
            order.OpenCloseType = Utils.GetFutureOpenCloseType(OpenClose);
            spqhDoOrderNum = 0;

            for (int i = 0; i < batch; i++)
            {
                smartPool.QueueWorkItem(DoSPQHOrder, order);
            }

            if (batch >= 50)
            {
                int scale = batch / 50;
                btnSPQHOrder.Enabled = false;
                spqhTimer.Interval = 1000 * scale;
                spqhTimer.Elapsed += delegate
                {
                    this.Invoke(new MethodInvoker(() => { btnSPQHOrder.Enabled = true; }));
                    spqhTimer.Enabled = false;
                };
                spqhTimer.Enabled = true;
            }
        }
        #endregion 商品期货批量下单

        #region 弹出选择文件提示框
        /// <summary>
        /// 弹出选择文件提示框
        /// </summary>
        /// <returns>选择的路径</returns>
        private string FileName()
        {
            error.Clear();
            errPro.Clear();
            openFileDialog1.Filter = "xls files   (*.xls)|*.xls";
            openFileDialog1.ShowDialog();
            return openFileDialog1.FileName;
        }
        #endregion 弹出选择文件提示框
    }
}