using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows.Forms;
using Amib.Threading;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using GTA.VTS.CustomersOrders.BLL;
using GTA.VTS.CustomersOrders.DoAccountManager;
using GTA.VTS.CustomersOrders.DoDealRptService;
using GTA.VTS.CustomersOrders.DoOrderService;
using GTA.VTS.CustomersOrders.HKCommonQuery;
using System.Threading;

namespace GTA.VTS.CustomersOrders.AppForm
{
    /// <summary>
    /// 作者：叶振东
    /// 时间：2010-03-02
    /// 描述：港股下单窗体
    /// </summary>
    public partial class frmHKOrder : MdiFormBase, IOrderCallBackView<HKDealOrderPushBack>, IOrderCallBackView<HKModifyOrderPushBack>
    {

        #region 变量定义

        private bool isAutoOrder = false;

        private string hkAccount = "";
        private int hkDoOrderNum;
        private bool isClearHK;
        private bool isProcessing;
        private bool isReBindHKData;
        private bool loadCodeSuccess;
        private SmartThreadPool smartPool = new SmartThreadPool { MaxThreads = 200, MinThreads = 25 };
        private int hkIndex;
        private string title = "";
        private string traderId = "";
        //internal WCFServices wcfLogic;
        private HKMessageLogic hkLogic = new HKMessageLogic();
        private OrderSQLHelper orderSql = new OrderSQLHelper();

        private System.Timers.Timer hkTimer = new System.Timers.Timer();
        private System.Timers.Timer timer = new System.Timers.Timer();
        /// <summary>
        /// 港股自动下单后自动撤单触发事件
        /// </summary>
        private System.Timers.Timer hkAutoTime = null;
        /// <summary>
        /// 所有代码列表
        /// </summary>
        private List<string> AllCodeList = new List<string>();
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public frmHKOrder()
        {
            InitializeComponent();
            OrderCallBack.HKView = this;
            OrderCallBack.HKModifyView = this;
            //wcfLogic = WCFServices.Instance;
            hkLogic = new HKMessageLogic();
            //自动撤单委托 事件
            hkLogic.AutoEvent += new AutoCancelOrder(this.CancelHKOrder);

            LocalhostResourcesFormText();
        }
        #endregion 构造函数

        #region 初始化

        /// <summary>
        /// 刷新显示相关语方类型的标签
        /// </summary>
        private void LocalhostResourcesFormText()
        {
            this.Text = ResourceOperate.Instanse.GetResourceByKey("menuHKOrder");
            #region 自动下单多语言
            this.grbAutomaticorders.Text = ResourceOperate.Instanse.GetResourceByKey("Automaticorders");
            this.lbxhEndIndex.Text = ResourceOperate.Instanse.GetResourceByKey("EndIndx");
            this.lbTimeSpan.Text = ResourceOperate.Instanse.GetResourceByKey("TimeSpan");
            this.chbHoldAccount.Text = ResourceOperate.Instanse.GetResourceByKey("HoldAccounts");
            this.btnAutoOrder.Text = ResourceOperate.Instanse.GetResourceByKey("AutoOrder");
            this.toolTip1.SetToolTip(this.txtHKIndex, ResourceOperate.Instanse.GetResourceByKey("Index"));
            this.toolTip1.SetToolTip(this.txtTimeSapn, ResourceOperate.Instanse.GetResourceByKey("TimeSapn"));
            this.chkAutoHKCacle.Text = ResourceOperate.Instanse.GetResourceByKey("Automatic");
            #endregion
            #region 港股语言类型显示
            this.lblHKCode.Text = ResourceOperate.Instanse.GetResourceByKey("lblCode");
            this.lblHKAmount.Text = ResourceOperate.Instanse.GetResourceByKey("Amount");
            this.lblHKPrice.Text = ResourceOperate.Instanse.GetResourceByKey("Price");
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
            #endregion 港股语言类型显示
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
            #region 港股下单数据绑定显示
            for (int i = 0; i < this.dagHK.ColumnCount; i++)
            {
                string HKName = dagHK.Columns[i].HeaderText;
                dagHK.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(HKName);
            }
            #endregion 港股下单数据绑定显示
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmHKOrder_Load(object sender, EventArgs e)
        {
            ////定时刷新资金账户信息
            timer.Interval = 2000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
            this.cbHKCode.SelectedIndex = 0;
            #region 自动补全
            this.cbHKCode.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.cbHKCode.AutoCompleteSource = AutoCompleteSource.ListItems;
            AllCodeList = WCFServices.Instance.GetAllCode(4);
            //如果没有数据还是用之前添加在Item里的数据
            if (AllCodeList != null && AllCodeList.Count > 0)
            {
                this.cbHKCode.DataSource = AllCodeList;
            }
            else
            {   //为了后面自动下单作准备使用
                AllCodeList = new List<string>();
                foreach (var item in cbHKCode.Items)
                {
                    AllCodeList.Add(item.ToString());
                }
            }
            #endregion 自动补全
            // LocalhostResourcesFormText();
            cbHKBuySell.SelectedIndex = 0;
            cbHKUnit.SelectedIndex = 0;
            cbHKPriceType.SelectedIndex = 0;
            Start();

            title = this.Text;
            this.txtHKProtfolioLogo.Text = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            if (loadCodeSuccess)
            {
                //   this.txtCode.KeyUp += this.txtCode_KeyUp;
            }

            QueryHKCapital();
        }

        /// <summary>
        /// 初始化基本信息
        /// </summary>
        public void Start()
        {
            smartPool.Start();

            //定时刷新资金账户信息
            hkTimer.Interval = 1000;
            hkTimer.Enabled = true;
            WCFServices.Instance.LoadTraderInfo();
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
        #region 定时刷新资金账户信息
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

            //QueryHKCapital();
            ReBindHKData();
            isProcessing = false;
        }
        #endregion 定时刷新资金账户信息
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

            QueryHKCapital();

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
        /// 港股资金账户查询
        /// </summary>
        private void QueryHKCapital()
        {
            try
            {
                string capitalAccount = ServerConfig.HKCapitalAccount;
                string msg = "";

                var cap = WCFServices.Instance.QueryHKCapital(capitalAccount, ref msg);

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

        #region 窗体事件
        /// <summary>
        /// 双击价格事件获取最新价格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtHKPrice_DoubleClick(object sender, EventArgs e)
        {
            Price();
        }
        /// <summary>
        /// 根据股票代码获取价格上下限和行情信息
        /// </summary>
        private decimal Price()
        {
            decimal price = 0;
            try
            {
                errPro.Clear();
                string errMsg = "";
                MarketDataLevel leave = WCFServices.Instance.GetLastPricByCommodityCode(cbHKCode.Text.Trim(), 4, out errMsg);
                if (leave != null)
                {
                    price = leave.LastPrice;

                    //自动下单不作处理
                    //if (!isAutoOrder)
                    //{
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
                    //}
                }
                //自动下单不作处理
                //if (!isAutoOrder)
                //{
                if (!string.IsNullOrEmpty(errMsg))
                {
                    errPro.SetError(txtHKPrice, errMsg);
                }
                txtHKPrice.Text = Utils.Round(price).ToString();

                SetHKHighLowValue();
                //}

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
            return price;
        }

        /// <summary>
        /// 设置港股价格上下限
        /// </summary>
        private void SetHKHighLowValue()
        {
            errPro.Clear();
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

            var highLowRange = WCFServices.Instance.GetHKHighLowRangeValueByCommodityCode(cbHKCode.Text.Trim(), price, priceType, tranType, out errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                errPro.SetError(txtHKPrice, errMsg);
            }
            if (highLowRange == null)
            {
                // MessageBox.Show("Can not get highlowrange object!");
                string errMessage = ResourceOperate.Instanse.GetResourceByKey("highlowrange");
                errPro.Clear();
                errPro.SetError(txtHKHigh, errMessage);
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
                // labHKHLType.Text = hkrv.HKValidPriceType.ToString();
            }
            else //其它类型处理
            {
                txtHKHigh.Text = Utils.Round(highLowRange.HighRangeValue).ToString();
                txtHKLow.Text = Utils.Round(highLowRange.LowRangeValue).ToString();
            }
        }

        /// <summary>
        /// 港股下单按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHKSendOrder_Click(object sender, EventArgs e)
        {
            try
            {
                #region 组装下单实体
                errPro.Clear();
                hkDoOrderNum = 0;
                HKOrderRequest order = new HKOrderRequest();
                //判断Code是否为空，如果为空则弹出错误提示框并退出
                order.Code = cbHKCode.Text.Trim();
                order.BuySell = cbHKBuySell.SelectedIndex == 0
                                    ? Types.TransactionDirection.Buying
                                    : Types.TransactionDirection.Selling;
                order.FundAccountId = ServerConfig.HKCapitalAccount;
                order.OrderAmount = float.Parse(txtHKAmount.Text.Trim());

                if (isAutoOrder)
                {
                    //强制点击获取价格
                    order.OrderPrice = float.Parse(Price().ToString());
                    if (order.OrderPrice == 0)
                    {
                        return;
                    }
                }
                else
                {
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
                }
                order.OrderUnitType = Utils.GetUnit(cbHKUnit.Text.Trim());
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

                //order.PortfoliosId = "p1";
                //判断TraderID是否正确
                order.TraderId = ServerConfig.TraderID;
                order.PortfoliosId = this.txtHKProtfolioLogo.Text;
                order.TraderPassword = "";
                int batch = 1;
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
                //this.txtHKProtfolioLogo.Text = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                #endregion 组装下单实体
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

        }

        /// <summary>
        /// 港股下单
        /// </summary>
        /// <param name="order">港股下单请求</param>
        private void DoHKOrder(HKOrderRequest order)
        {
            var res = WCFServices.Instance.DoHKOrder(order);
            if (isAutoOrder)
            {
                if (res.IsSuccess)
                {
                    hkLogic.AddAutoCanceOrder(res.OrderId, DateTime.Now);
                }
            }
            hkLogic.ProcessDoOrder(res, order);

            hkDoOrderNum++;

            //WriteTitle(hkDoOrderNum);

            string format =
                "DoHKOrder[EntrustNumber={0},Code={1},EntrustAmount={2},EntrustPrice={3},BuySell={4},TraderId={5},OrderMessage={6},IsSuccess={7}]  Time={8}";
            string desc = string.Format(format, res.OrderId, order.Code, order.OrderAmount, order.OrderPrice,
                                        order.BuySell, order.TraderId, res.OrderMessage, res.IsSuccess,
                                        DateTime.Now.ToLongTimeString());
            WriteHKMsg(desc);
            //LogHelper.WriteDebug(desc);
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
        /// 处理回推信息
        /// </summary>
        /// <param name="i"></param>
        private void WriteTitle(int i)
        {
            string msg = "[" + i + "]";
            this.Invoke(new MethodInvoker(() => { this.Text = this.title + msg; }));
        }

        /// <summary>
        /// 单击单元格触发
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
        /// 港股撤单委托下单
        /// </summary>
        private void DoHKCancelOrder(HKMessage message)
        {
            string errMsg = "";
            CancelHKOrder(message.EntrustNumber, ref errMsg);
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
                order.TraderId = ServerConfig.TraderID;
                order.OrderUnitType = form.ModifyUnitType;
                var res = WCFServices.Instance.ModifyHKOrder(order);

                string format =
                "DoHKModifyOrder[EntrustNumber={0},Code={1},EntrustAmount={2},EntrustPrice={3},TraderId={4},OrderMessage={5},IsSuccess={6}]  Time={7}";
                string desc = string.Format(format, res.OrderId, order.Code, order.OrderAmount, order.OrderPrice,
                                            order.TraderId, res.OrderMessage, res.IsSuccess,
                                            DateTime.Now.ToLongTimeString());
                WriteHKMsg(desc);
                //LogHelper.WriteDebug(desc);
                hkLogic.UpdateMessage(message.EntrustNumber, res.OrderMessage);
            }

            form.Close();
        }
        /// <summary>
        /// 自动撤单
        /// </summary>
        /// <param name="entrustNumber">要撤单的委托单号</param>
        /// <param name="errMsg">撤单信息</param>
        public bool CancelHKOrder(string entrustNumber, ref string errMsg)
        {
            errMsg = "";
            bool isSuccess = WCFServices.Instance.CancelHKOrder(entrustNumber, ref errMsg);
            if (!isSuccess)
            {
                string msg = "港股委托[" + entrustNumber + "]撤单失败！" + Environment.NewLine + errMsg;
                hkLogic.UpdateMessage(entrustNumber, errMsg);
            }
            return isSuccess;
        }

        /// <summary>
        /// 港股最大委托显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtHKMax_DoubleClick(object sender, EventArgs e)
        {
            Max();
        }
        /// <summary>
        /// 获取最大委托量
        /// </summary>
        private void Max()
        {
            try
            {
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

                var traderId = ServerConfig.TraderID;
                var code = cbHKCode.Text.Trim();

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
                var max = WCFServices.Instance.GetHKMaxCount(traderId, code, price, priceType, out errMsg);

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
        /// 港股右击清空按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
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

        /// <summary>
        /// 清空港股回推信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox5.Items.Clear();
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmHKOrder_FormClosed(object sender, FormClosedEventArgs e)
        {
            hkTimer.Close();
            timer.Close();
            isAutoOrder = false;
            //关闭自动撤单事件
            if (hkAutoTime != null)
            {
                hkAutoTime.Enabled = false;
            }
        }

        /// <summary>
        /// 价格回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtHKPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                Price();
            }
        }

        /// <summary>
        /// 最大委托量回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtHKMax_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                Max();
            }
        }
        /// <summary>
        /// 刷新资金事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtHKAvailable_MouseDown(object sender, MouseEventArgs e)
        {
            QueryHKCapital();
        }

        /// <summary>
        /// 双击委托单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dagHK_DoubleClick(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dagHK.SelectedRows)
            {
                HKMessage message = row.DataBoundItem as HKMessage;
                if (message == null)
                {
                    continue;
                }
                HKMessageLogic.FireEntrustSelectedEvent(message.EntrustNumber);
            }
        }

        #endregion 窗体事件

        #region IOrderCallBackView<StockDealOrderPushBack> 成员

        /// <summary>
        /// 处理成交回报
        /// </summary>
        /// <param name="drsip"></param>
        public void ProcessPushBack(HKDealOrderPushBack drsip)
        {
            var tet = drsip.HKOrderEntity;
            var deals = drsip.HKDealList;

            string format =
                "<--PushBack[EntrustNumber={0},Code={1},TradeAmount={2},CancelAmount={3},OrderStatusId={4},OrderMessage={5},DealsCount={6},IsModifyOrder={7},ModifyOrderNumber={8}] Time={9} ";
            string desc = string.Format(format, tet.EntrustNumber, tet.Code, tet.TradeAmount, tet.CancelAmount,
                                        Utils.GetOrderStateMsg(tet.OrderStatusId), tet.OrderMessage,
                                        deals.Count, tet.IsModifyOrder, tet.ModifyOrderNumber, DateTime.Now.ToLongTimeString());

            //LogHelper.WriteDebug(desc);


            //写委托记录
            WriteHKMsg(desc);
            //成交记录 
            // string dealsDesc = GetHKDealsDesc(deals);
            if (isAutoOrder)
            {
                if (tet.OrderStatusId > 5 && tet.OrderStatusId != 9)
                {
                    hkLogic.DelateAutoCanceOrder(tet.EntrustNumber);
                }
            }

            hkLogic.ProcessPushBack(drsip);
        }
        //    /// <summary>
        //    /// 处理回推信息
        //    /// </summary>
        //    /// <param name="deals"></param>
        //    /// <returns></returns>
        //    private string GetHKDealsDesc(List<HKPushDealEntity> deals)
        //    {
        //        StringBuilder sb = new StringBuilder("");
        //        if (deals == null || deals.Count <= 0)
        //        {
        //            return sb.ToString();
        //        }
        //        foreach (var deal in deals)
        //        {
        //            string format =
        //"<--HKTradeInfo-PushBack[TradeNumber={0},TradeAmount={1},TradePrice={2},TradeTypeId={3},TradeTime={4}] Time={5} ";
        //            string desc = string.Format(format, deal.TradeNumber, deal.TradeAmount, deal.TradePrice, deal.TradeTypeId, deal.TradeTime, DateTime.Now.ToLongTimeString());

        //            WriteHKMsg(desc);
        //        }
        //        return sb.ToString();
        //    }
        #endregion

        #region IOrderCallBackView<HKModifyOrderPushBack> 成员
        /// <summary>
        /// 港股改单回推
        /// </summary>
        /// <param name="back"></param>
        public void ProcessPushBack(HKModifyOrderPushBack back)
        {
            string format =
                 "<--ModifyBack[TraderID={0},IsSuccess={1},Message={2},OriginalNumber={3},NewNumber={4}]  Time={5}";
            string desc = string.Format(format, back.TradeID, back.IsSuccess, back.Message, back.OriginalRequestNumber,
                                        back.NewRequestNumber, DateTime.Now.ToLongTimeString());

            //LogHelper.WriteDebug(desc);

            WriteHKMsg(desc);

            hkLogic.ProcessModifyBack(back);
        }

        #endregion

        #region 港股批量下单处理方法
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
        /// <summary>
        /// 批量下单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHKBatchOrder_Click(object sender, EventArgs e)
        {
            try
            {
                #region 读取Excel表格中数据

                string Path = this.txtHKPath.Text;
                if (!string.IsNullOrEmpty(Path))
                {
                    DataSet myDataSet = new DataSet();
                    myDataSet = orderSql.dataSet(Path);
                    if (myDataSet != null)
                    {
                        try
                        {
                            string Code, BuySell, UnitType, cbMarketOrder, price, PortfoliosId;
                            int batch;
                            float OrderAmount;
                            for (int i = 0; i < myDataSet.Tables[0].Rows.Count; i++)
                            {

                                Code = myDataSet.Tables[0].Rows[i][0].ToString();
                                OrderAmount = float.Parse(myDataSet.Tables[0].Rows[i][1].ToString());
                                batch = int.Parse(myDataSet.Tables[0].Rows[i][2].ToString());
                                BuySell = myDataSet.Tables[0].Rows[i][3].ToString();
                                UnitType = myDataSet.Tables[0].Rows[i][4].ToString();
                                cbMarketOrder = myDataSet.Tables[0].Rows[i][5].ToString();
                                price = myDataSet.Tables[0].Rows[i][6].ToString();
                                PortfoliosId = myDataSet.Tables[0].Rows[i][7].ToString();
                                if (string.IsNullOrEmpty(PortfoliosId))
                                {
                                    PortfoliosId = "";
                                }
                                HKOrder(Code, OrderAmount, batch, BuySell, UnitType, cbMarketOrder, price, PortfoliosId);

                            }
                        }
                        catch (Exception ex)
                        {
                            errPro.Clear();
                            errPro.SetError(btnHKPath, "您选择的港股模板出现如下问题" + ex);
                            LogHelper.WriteError(ex.Message, ex);
                        }
                    }
                    else
                    {
                        errPro.Clear();
                        errPro.SetError(btnHKPath, "您选择的港股模板无法读取数据");
                    }
                }
                else
                {
                    errPro.Clear();
                    errPro.SetError(btnHKPath, "请先选择导入模板");
                }

                #endregion 读取Excel表格中数据
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }
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
        private void HKOrder(string Code, float OrderAmount, int batchs, string BuySell, string UnitType, string cbMarketOrder, string price, string PortfoliosId)
        {
            try
            {
                #region 获取表格中数据并添加到下单实体中
                errPro.Clear();
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
                    errPro.SetError(btnHKPath, "Code" + CodeError);
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
                    string Error = OrdreName + Code + BS + Exception;
                    errPro.SetError(btnHKPath, Error);
                }
                order.FundAccountId = ServerConfig.HKCapitalAccount;
                order.OrderAmount = OrderAmount;

                if (!string.IsNullOrEmpty(price))
                {
                    float pric;
                    if (float.TryParse(price, out pric))
                    {
                        order.OrderPrice = pric;
                    }
                    Types.HKPriceType priceType = new Types.HKPriceType();
                    ////LO限价盘
                    ////ELO增强限价盘
                    ////SLO特别限价盘
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
                    #region 获取上下限

                    //string high;
                    //string low;
                    //decimal prices = 0;
                    //string errMsg = "";
                    //bool isSuccess = decimal.TryParse(price, out prices);
                    //if (!isSuccess)
                    //{
                    //    MessageBox.Show(OrdreName + Code + OrderPrice + Exception);
                    //    return;
                    //}
                    ////获取价格上下限
                    //var highLowRange = WCFServices.Instance.GetHKHighLowRangeValueByCommodityCode(Code, prices, priceType, buySells,
                    //                                                                  out errMsg);
                    //if (!string.IsNullOrEmpty(errMsg))
                    //{
                    //    errPro.SetError(txtHKPrice, errMsg);
                    //}
                    //if (highLowRange == null)
                    //{
                    //    //MessageBox.Show("Can not get highlowrange object!");
                    //    string errMessage = ResourceOperate.Instanse.GetResourceByKey("highlowrange");
                    //    MessageBox.Show(OrdreName + Code + errMessage);
                    //    return;
                    //}
                    ////港股类型处理
                    //if (highLowRange.RangeType == Types.HighLowRangeType.HongKongPrice)
                    //{
                    //    var hkrv = highLowRange.HongKongRangeValue;
                    //    Types.TransactionDirection buySell = new Types.TransactionDirection();
                    //    if (BuySell.Equals("Buying"))
                    //    {
                    //        buySell = Types.TransactionDirection.Buying;
                    //    }
                    //    else if (BuySell.Equals("Selling"))
                    //    {
                    //        buySell = Types.TransactionDirection.Selling;
                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show(OrdreName + Code + BS + Exception);
                    //    }
                    //    if (buySell == Types.TransactionDirection.Buying)
                    //    {
                    //        high = Utils.Round(hkrv.BuyHighRangeValue).ToString();
                    //        low = Utils.Round(hkrv.BuyLowRangeValue).ToString();
                    //    }
                    //    else
                    //    {
                    //        high = Utils.Round(hkrv.SellHighRangeValue).ToString();
                    //        low = Utils.Round(hkrv.SellLowRangeValue).ToString();
                    //    }
                    //}
                    //else
                    //{
                    //    //其它类型处理
                    //    high = Utils.Round(highLowRange.HighRangeValue).ToString();
                    //    low = Utils.Round(highLowRange.LowRangeValue).ToString();
                    //}

                    #endregion 获取上下限
                    #region 价格处理

                    //bool SeesawPrice = ServerConfig.Price;
                    //if (SeesawPrice == true)
                    //{
                    //    order.OrderPrice = float.Parse(prices.ToString());
                    //    return;
                    //}
                    //else
                    //{
                    //    if (!string.IsNullOrEmpty(high) && !string.IsNullOrEmpty(low))
                    //    {
                    //        decimal highPrice = 0;
                    //        decimal lowPrice = 0;
                    //        if (decimal.TryParse(high, out highPrice) && decimal.TryParse(low, out lowPrice))
                    //        {
                    //            if (prices <= highPrice && prices >= lowPrice)
                    //            {
                    //                order.OrderPrice = float.Parse(prices.ToString());
                    //            }
                    //            else
                    //            {
                    //                string PriceErrors = ResourceOperate.Instanse.GetResourceByKey("PriceErrors");
                    //                MessageBox.Show(OrdreName + Code + PriceErrors);
                    //                return;
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        string PriceErrors = ResourceOperate.Instanse.GetResourceByKey("PriceErrors");
                    //        MessageBox.Show(OrdreName + Code + PriceErrors);
                    //        return;
                    //    }
                    //}

                    #endregion 价格处理
                }
                else
                {
                    errPro.Clear();
                    string HKPriceError = ResourceOperate.Instanse.GetResourceByKey("NullError");
                    errPro.SetError(btnHKPath, "Price" + HKPriceError);
                    return;
                }
                order.OrderUnitType = Utils.GetUnit(UnitType);
                //order.OrderWay = cbMarketOrder.Checked ? TypesOrderPriceType.OPTMarketPrice : TypesOrderPriceType.OPTLimited;
                order.PortfoliosId = PortfoliosId;
                //判断TraderID是否正确
                order.TraderId = ServerConfig.TraderID;
                order.TraderPassword = "";

                //int batch = int.Parse(txtHKBatch.Text.Trim());
                int batch = 1;
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

                #endregion 获取Excel表格中数据并添加到下单实体中
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }
        /// <summary>
        /// 弹出选择文件提示框
        /// </summary>
        /// <returns>选择的路径</returns>
        private string FileName()
        {
            errPro.Clear();
            openFileDialog1.Filter = "xls files   (*.xls)|*.xls";
            openFileDialog1.ShowDialog();
            return openFileDialog1.FileName;
        }
        #endregion  港股批量下单处理方法

        #region 自动下单
        /// <summary>
        /// 自动下单按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutoOrder_Click(object sender, EventArgs e)
        {
            if (isAutoOrder)
            {
                txtHKIndex.Enabled = true;
                //可用
                //cbHKCode.Enabled = true;
                isAutoOrder = false;
                //这里要实现多语言
                btnAutoOrder.Text = ResourceOperate.Instanse.GetResourceByKey("AutoOrder");
                //关闭自动撤单事件
                if (hkAutoTime != null)
                {
                    hkAutoTime.Enabled = false;
                }
            }
            else
            {
                //cbHKCode.SelectedIndex = 0;
                //不可用
                //cbHKCode.Enabled = false;
                txtHKIndex.Enabled = false;
                isAutoOrder = true;
                //这里要实现多语言
                btnAutoOrder.Text = ResourceOperate.Instanse.GetResourceByKey("AutoOrders");
                //自动撤单事件
                if (chkAutoHKCacle.Checked && hkAutoTime == null)
                {
                    hkAutoTime = new System.Timers.Timer();
                    hkAutoTime.Interval = 60 * 1000;
                    hkAutoTime.Elapsed += new System.Timers.ElapsedEventHandler(autoTime_Elapsed);
                }
                if (hkAutoTime != null)
                {
                    hkAutoTime.Enabled = chkAutoHKCacle.Checked;
                }

                #region 组装自动下单实体
                AutoDoOrder order = new AutoDoOrder();
                order.BuySell = this.cbHKBuySell.SelectedIndex;

                order.OrderPriceType = this.cbHKPriceType.SelectedIndex;
                order.OrderUnitType = cbHKUnit.Text.Trim();
                order.PortfoliosID = this.txtHKProtfolioLogo.Text;

                int amt = 0;
                int.TryParse(txtHKAmount.Text, out amt);
                order.OrderAmount = amt;
                order.IndexStart = cbHKCode.SelectedIndex;
                int end = 0;
                if (!int.TryParse(txtHKIndex.Text, out end))
                {
                    end = cbHKCode.Items.Count;
                }
                if (end > cbHKCode.Items.Count)
                {
                    end = cbHKCode.Items.Count;
                }

                //如果出现最后索引小于当前代码列表的索引时建最后索引和代码列表索引进行互换
                int y;
                if (end < order.IndexStart && end > 0)
                {
                    y = end;
                    end = order.IndexStart;
                    order.IndexStart = y;
                }
                order.IndexEnd = end;

                order.CodeCount = cbHKCode.Items.Count;
                if (chbHoldAccount.Checked)
                {
                    order.IsHoldAccount = true;
                }
                else
                {
                    order.IsHoldAccount = false;
                }
                int ts = 5;
                if (!int.TryParse(this.txtTimeSapn.Text, out ts))
                {
                    ts = 5 * 60;
                }


                order.DoOrderTimeSapn = ts;
                #endregion

                //开始异步多线程操作自动下单
                smartPool.QueueWorkItem(AutoOrderInvoker, order);
            }
        }

        /// <summary>
        /// 自动下组装相关实体
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="amount">下单总量</param>
        /// <param name="fundAccountID">资金账号</param>
        /// <param name="type">交易类型(买，卖）</param>
        /// <param name="unitType">委托单位（股，手）</param>
        /// <param name="tradeID">交易员ID</param>
        /// <param name="portfoliosID">投组标识</param>
        /// <param name="orderPriceType">报价类型</param>
        private void AutoHKOrder(string code, float amount, string fundAccountID, Types.TransactionDirection type, Types.UnitType unitType, string tradeID,
            string portfoliosID, Types.HKPriceType orderPriceType)
        {
            try
            {
                #region 组装下单实体
                if (amount < 0)
                {
                    return;
                }
                HKOrderRequest order = new HKOrderRequest();
                order.Code = code;
                order.BuySell = type;
                order.FundAccountId = fundAccountID;
                order.OrderAmount = amount;

                #region 价格处理

                string errMsg = "";
                MarketDataLevel leave = WCFServices.Instance.GetLastPricByCommodityCode(code, 4, out errMsg);
                if (leave != null)
                {
                    order.OrderPrice = float.Parse(leave.LastPrice.ToString());
                }
                //如果获取不到价格不下单
                if (order.OrderPrice == 0)
                {
                    return;
                }

                #endregion 价格处理

                order.OrderWay = orderPriceType;
                order.OrderUnitType = unitType;
                order.TraderId = tradeID;
                order.TraderPassword = "";
                order.PortfoliosId = portfoliosID;

                smartPool.QueueWorkItem(DoHKOrder, order);

                #endregion 组装下单实体
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        ///  自动撤单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void autoTime_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //间隔分钟撤单
            int minute = 3;
            if (!int.TryParse(txtHKCancleMin.Text, out minute))
            {
                minute = 3;
            }
            hkLogic.DisposeAutoCance(minute);
        }

        /// <summary>
        /// 异常自动下单操作方法
        /// <param name="index">开始索引列</param>
        /// <param name="order"></param>
        /// </summary>
        private void AutoOrderInvoker(AutoDoOrder order)
        {
            List<HK_AccountHoldInfo> holdList = null;


            string fundAccountID = ServerConfig.HKCapitalAccount;
            Types.TransactionDirection type = order.BuySell == 0
                               ? Types.TransactionDirection.Buying
                               : Types.TransactionDirection.Selling;
            //LO限价盘
            //ELO增强限价盘
            //SLO特别限价盘
            Types.HKPriceType orderWay = Types.HKPriceType.LO;
            switch (order.OrderPriceType)
            {
                case 0:
                    orderWay = Types.HKPriceType.LO;
                    break;
                case 1:
                    orderWay = Types.HKPriceType.ELO;
                    break;
                case 2:
                    orderWay = Types.HKPriceType.SLO;
                    break;
            }
            Types.UnitType unitType = Utils.GetUnit(order.OrderUnitType);
            string tradeID = ServerConfig.TraderID; ;
            string portfoliosID = order.PortfoliosID;
            //if (!decimal.TryParse(txtHKAmount.Text, out amt))
            //{
            //    amt = 0;
            //}
            decimal amt = order.OrderAmount;
            if (amt <= 0)
            {
                errPro.SetError(btnAutoOrder, "委托量不能为0");
                return;
            }

            do
            {
                #region 这里写死的时间判断，如果以后要修改再从管理中心获取添加服务
                DateTime nowTime = WCFServices.Instance.CheckDoAccoumtChannel();
                //7	2009-12-25 10:00:00.000	2009-12-25 12:30:00.000	4
                //8	2009-12-25 14:30:00.000	2009-12-25 16:00:00.000	4
                //如果不在交易开市时间不作自动下单
                if (!(nowTime >= new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 10, 0, 0) && nowTime <= new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 12, 30, 0))
                    && !(nowTime >= new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 14, 30, 0) && nowTime <= new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 16, 0, 0)))
                {
                    WriteHKMsg("不在交易时间内不能操作自动下单功能!");
                    btnAutoOrder_Click(this, null);
                    break;
                }
                #endregion
                if (order.IsHoldAccount)
                {
                    holdList = WCFServices.Instance.HKHold;
                    //当前下单量
                    decimal orderCount = amt;
                    foreach (var hold in holdList)
                    {
                        if (!isAutoOrder)
                        {
                            break;
                        }
                        //if (!decimal.TryParse(txtHKAmount.Text, out amt))
                        //{
                        //    amt = 0;
                        //}
                        //持仓不足
                        if (hold.AvailableAmount <= 0)
                        {
                            continue;
                        }
                        if (hold.AvailableAmount < amt)
                        {
                            orderCount = hold.AvailableAmount;
                        }
                        //this.Invoke(new MethodInvoker(() =>
                        //{
                        //    cbHKCode.Text = hold.Code;
                        //    cbHKBuySell.SelectedIndex = 1;
                        //    //强制点击
                        //    btnHKSendOrder_Click(this, null);
                        //}));
                        AutoHKOrder(hold.Code, (float)orderCount, fundAccountID, type, unitType, tradeID, portfoliosID, orderWay);

                    }
                }
                else
                {
                    if (order.IndexStart > AllCodeList.Count || order.IndexEnd > AllCodeList.Count)
                    {
                        errPro.SetError(btnAutoOrder, "索引选择不正确");
                        break;
                    }
                    for (int k = order.IndexStart; k < order.IndexEnd; k++)
                    {
                        if (!isAutoOrder)
                        {
                            break;
                        }

                        //this.Invoke(new MethodInvoker(() =>
                        //{
                        //    cbHKCode.SelectedIndex = k;
                        //    //强制点击
                        //    btnHKSendOrder_Click(this, null);
                        //}));
                        AutoHKOrder(AllCodeList[k], (float)amt, fundAccountID, type, unitType, tradeID, portfoliosID, orderWay);

                    }
                }
                int ts = order.DoOrderTimeSapn;
                //if (!int.TryParse(this.txtTimeSapn.Text, out ts))
                //{
                //    ts = 5 * 60;
                //}
                Thread.CurrentThread.Join(ts * 1000);
            } while (isAutoOrder);
        }
        /// <summary>
        /// 获取当前列表中当前选择的索引
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtHKIndex_DoubleClick(object sender, EventArgs e)
        {
            txtHKIndex.Text = cbHKCode.SelectedIndex.ToString();

        }
        /// <summary>
        /// 自动下单对持仓操作选择框方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbHoldAccount_CheckedChanged(object sender, EventArgs e)
        {
            //主要选择了当前对持仓进行自动下单操作，不象期货那样持仓会有卖空或者买空的存在，所以港股持仓操作只有卖（目前是这样的）
            if (chbHoldAccount.Checked)
            {
                cbHKBuySell.SelectedIndex = 1;
                cbHKBuySell.Enabled = false;//不可再用
            }
            else
            {
                cbHKBuySell.Enabled = true;
            }
        }
        #endregion



    }
}
