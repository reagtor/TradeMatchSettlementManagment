using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Amib.Threading;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using GTA.VTS.CustomersOrders.BLL;
using GTA.VTS.CustomersOrders.DoAccountManager;
using GTA.VTS.CustomersOrders.DoDealRptService;
using GTA.VTS.CustomersOrders.DoOrderService;
using GTA.VTS.CustomersOrders.DoCommonQuery;
using TypesOrderPriceType = GTA.VTS.CustomersOrders.DoOrderService.TypesOrderPriceType;

namespace GTA.VTS.CustomersOrders.AppForm
{
    /// <summary>
    /// 作者：叶振东
    /// 时间：2010-03-02
    /// 描述：股指期货下单窗体                       
    /// </summary>
    public partial class frmSIOrder : MdiFormBase, IOrderCallBackView<FutureDealOrderPushBack>
    {
        #region 变量定义
        private string gzqhAccount = "";
        private int gzqhDoOrderNum;
        private bool isReBindGZQHData;
        private bool loadCodeSuccess;
        private bool isProcessing;
        private bool isClearGZQH;
        private SmartThreadPool smartPool = new SmartThreadPool { MaxThreads = 200, MinThreads = 25 };
        private string title = "";
        private string traderId = "";
        private WCFServices wcfLogic;
        private GZQHMessageLogic gzqhLogic;
        private OrderSQLHelper orderSql = new OrderSQLHelper();
        private System.Timers.Timer timer = new System.Timers.Timer();
        private System.Timers.Timer gzqhTimer = new System.Timers.Timer();
        /// <summary>
        /// 股指期货自动下单后自动撤单触发事件
        /// </summary>
        private System.Timers.Timer gzAutoTime = null;

        private int gzqhIndex;
        /// <summary>
        /// 自动下单状态（是否正在自动下单）
        /// </summary>
        private bool isAutoOrder = false;
        /// <summary>
        /// 所有代码列表
        /// </summary>
        private List<string> AllCodeList = new List<string>();
        #endregion

        #region 构造函数
        public frmSIOrder()
        {
            OrderCallBack.SIView = this;
            InitializeComponent();
            gzqhLogic = new GZQHMessageLogic();
            //自动撤单委托 事件
            gzqhLogic.AutoEvent += new AutoCancelOrder(this.CancelGZQHOrder);
            wcfLogic = WCFServices.Instance;
            LocalhostResourcesFormText();
        }
        #endregion 构造函数

        #region 初始化

        /// <summary>
        /// 刷新显示相关语方类型的标签
        /// </summary>
        private void LocalhostResourcesFormText()
        {
            this.Text = ResourceOperate.Instanse.GetResourceByKey("menuGZQHOrder");
            #region 自动下单多语言
            this.grbAutomaticorders.Text = ResourceOperate.Instanse.GetResourceByKey("Automaticorders");
            this.lbxhEndIndex.Text = ResourceOperate.Instanse.GetResourceByKey("EndIndx");
            this.lbTimeSpan.Text = ResourceOperate.Instanse.GetResourceByKey("TimeSpan");
            this.chbHoldAccount.Text = ResourceOperate.Instanse.GetResourceByKey("HoldAccounts");
            this.btnAutoOrder.Text = ResourceOperate.Instanse.GetResourceByKey("AutoOrder");
            this.toolTip1.SetToolTip(this.txtGZQHIndex, ResourceOperate.Instanse.GetResourceByKey("Index"));
            this.toolTip1.SetToolTip(this.txtTimeSapn, ResourceOperate.Instanse.GetResourceByKey("TimeSapn"));
            this.chkAutoGZQHCacle.Text = ResourceOperate.Instanse.GetResourceByKey("Automatic");
            #endregion
            #region 股指期货语言类型显示
            this.lblGZQHContract.Text = ResourceOperate.Instanse.GetResourceByKey("lblCode");
            this.lblGZQHAmount.Text = ResourceOperate.Instanse.GetResourceByKey("Amount");
            this.lblGZQHPrice.Text = ResourceOperate.Instanse.GetResourceByKey("Price");
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
            #region 股指期货下单数据绑定显示
            for (int i = 0; i < this.dataGridViewGZQH.ColumnCount; i++)
            {
                string GZQHName = dataGridViewGZQH.Columns[i].HeaderText;
                dataGridViewGZQH.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(GZQHName);
            }
            #endregion 股指期货下单数据绑定显示
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSIOrder_Load(object sender, EventArgs e)
        {
            #region 自动补全

            this.cbGZCode.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.cbGZCode.AutoCompleteSource = AutoCompleteSource.ListItems;
            AllCodeList = wcfLogic.GetAllCode(3);
            //如果没有数据还是用之前添加在Item里的数据
            if (AllCodeList != null && AllCodeList.Count > 0)
            {
                this.cbGZCode.DataSource = AllCodeList;
            }
            else
            {   //为了后面自动下单作准备使用
                AllCodeList = new List<string>();
                foreach (var item in cbGZCode.Items)
                {
                    AllCodeList.Add(item.ToString());
                }
            }
            #endregion 自动补全
            ////定时刷新资金账户信息
            timer.Interval = 1000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
            this.txtGZQHProtfolioLogo.Text = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            // LocalhostResourcesFormText();
            cbGZCode.SelectedIndex = 0;
            cbGZBuySell.SelectedIndex = 0;
            cbGZUnit.SelectedIndex = 0;
            cbGZOpenClose.SelectedIndex = 0;
            Start();
            title = this.Text;
            if (loadCodeSuccess)
            {
                //   this.txtCode.KeyUp += this.txtCode_KeyUp;
            }
            QueryGZQHCapital();
        }
        #region  定时刷新资金账户信息
        /// <summary>
        /// 定时刷新资金账户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (isProcessing)
                return;

            isProcessing = true;
            //QueryGZQHCapital();
            ReBindGZQHData();

            isProcessing = false;
        }
        #endregion 定时刷新资金账户信息
        /// <summary>
        /// 查询出股指期货资金并显示
        /// </summary>
        private void QueryGZQHCapital()
        {
            try
            {
                string capitalAccount = ServerConfig.GZQHCapitalAccount;
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
        /// 初始化基本信息
        /// </summary>
        public void Start()
        {
            smartPool.Start();
            wcfLogic.LoadTraderInfo();
            traderId = ServerConfig.TraderID;
            gzqhAccount = ServerConfig.GZQHCapitalAccount;
        }

        /// <summary>
        /// 重新绑定股指期货数据
        /// </summary>
        private void ReBindGZQHData()
        {
            if (!gzqhLogic.HasChanged)
                return;

            if (isReBindGZQHData)
                return;

            QueryGZQHCapital();

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
        /// 验证是否是合法的数字格式
        /// </summary>
        /// <param name="keyWords">需要验证的字符串</param>
        /// <returns></returns>
        public static bool DecimalTest(string keyWords)
        {
            return Regex.IsMatch(keyWords, @"^[0-9]+$");
        }

        /// <summary>
        /// 处理回报信息
        /// </summary>
        /// <param name="drsifi"></param>
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

        /// <summary>
        /// 回推信息处理
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
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

        #region 股指期货下单操作
        /// <summary>
        /// 双击股指期货价格事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtGZPrice_DoubleClick(object sender, EventArgs e)
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
                // decimal price = RealTimeMarketUtil.GetInstance().GetFutureLastTrade(cbGZCode.Text.Trim());
                string errMsg = "";

                MarketDataLevel leave = wcfLogic.GetLastPricByCommodityCode(cbGZCode.Text.Trim(), 3, out errMsg);
                if (leave != null)
                {
                    price = leave.LastPrice;
                    //if (!isAutoOrder)
                    //{
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
                    //}
                }
                //if (!isAutoOrder)
                //{
                if (!string.IsNullOrEmpty(errMsg))
                {
                    errPro.SetError(txtGZPrice, errMsg);
                }

                //}
                txtGZPrice.Text = Utils.Round(price).ToString();

                SetGZHighLowValue();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
            return price;
        }

        /// <summary>
        /// 股指期货价格上下限
        /// </summary>
        private void SetGZHighLowValue()
        {
            errPro.Clear();
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
                errPro.SetError(txtGZHigh, errMessage);
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

        /// <summary>
        /// 股指期货下单按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGZSendOrder_Click(object sender, EventArgs e)
        {
            try
            {
                #region 组装下单实体

                errPro.Clear();
                StockIndexFuturesOrderRequest order = new StockIndexFuturesOrderRequest();
                order.Code = cbGZCode.Text.Trim();
                order.BuySell = cbGZBuySell.SelectedIndex == 0
                                    ? Types.TransactionDirection.Buying
                                    : Types.TransactionDirection.Selling;
                order.FundAccountId = ServerConfig.GZQHCapitalAccount; //"010000002306";
                order.OrderAmount = float.Parse(txtGZAmount.Text.Trim());

                #region 是否市价委托

                if (!cbGZMarket.Checked)
                {
                    if (isAutoOrder)
                    {
                        //强制点击获取价格
                        order.OrderPrice = float.Parse(Price().ToString());
                    }
                    else
                    {
                        #region

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
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(this.txtGZHigh.Text) &&
                                    !string.IsNullOrEmpty(this.txtGZLow.Text))
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

                        #endregion
                    }
                }
                #endregion 是否市价委托

                order.OrderUnitType = Utils.GetUnit(cbGZUnit.Text.Trim());
                order.OrderWay = cbGZMarket.Checked
                                     ? TypesOrderPriceType.OPTMarketPrice
                                     : TypesOrderPriceType.OPTLimited;
                // order.PortfoliosId = "p2";
                order.TraderId = ServerConfig.TraderID; //"23";
                order.TraderPassword = "";
                order.OpenCloseType = Utils.GetFutureOpenCloseType(cbGZOpenClose.Text.Trim());
                order.PortfoliosId = this.txtGZQHProtfolioLogo.Text;
                int batch = 1;
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
                //this.txtGZQHProtfolioLogo.Text = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                #endregion 组装下单实体
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 股指期货下单
        /// </summary>
        /// <param name="order">股指期货下单请求</param>
        private void DoGZQHOrder(StockIndexFuturesOrderRequest order)
        {
            var res = wcfLogic.DoGZQHOrder(order);

            if (isAutoOrder)
            {
                if (res.IsSuccess)
                {
                    gzqhLogic.AddAutoCanceOrder(res.OrderId, DateTime.Now);
                }
            }

            gzqhLogic.ProcessDoOrder(res, order);

            gzqhDoOrderNum++;

            //WriteTitle(gzqhDoOrderNum);

            string format =
                "DoOrder[EntrustNumber={0},Code={1},EntrustAmount={2},EntrustPrice={3},BuySell={4},OpenClose={5},TraderId={6},OrderMessage={7},IsSuccess={8}] Time={9}";
            string desc = string.Format(format, res.OrderId, order.Code, order.OrderAmount, order.OrderPrice,
                                        order.BuySell, order.OpenCloseType, order.TraderId, res.OrderMessage,
                                        res.IsSuccess, DateTime.Now.ToLongTimeString());
            WriteGZQHMsg(desc);
            //  LogHelper.WriteDebug(desc);
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
        /// 期货的信息
        /// </summary>
        /// <param name="msg"></param>
        public void WriteGZQHMsg(string msg)
        {
            MessageDisplayHelper.Event(msg, listBox2);
        }

        /// <summary>
        /// 单击单元格时间
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
                {
                    continue;
                }

                string errMsg = "";
                if (!CancelGZQHOrder(message.EntrustNumber, ref errMsg))
                {
                    message.OrderMessage = errMsg;
                }
            }
        }

        /// <summary>
        /// 自动撤单
        /// </summary>
        /// <param name="entrustNumber">要撤单的委托单号</param>
        /// <param name="errMsg">撤单信息</param>
        public bool CancelGZQHOrder(string entrustNumber, ref string errMsg)
        {
            errMsg = "";
            bool isSuccess = wcfLogic.CancelGZQHOrder(entrustNumber, ref errMsg);
            if (!isSuccess)
            {
                string msg = "股指期货委托[" + entrustNumber + "]撤单失败！" + Environment.NewLine + errMsg;
                gzqhLogic.UpdateMessage(entrustNumber, errMsg);
            }
            return isSuccess;
        }

        /// <summary>
        /// 获取股指期货的最大委托量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtGZMax_DoubleClick(object sender, EventArgs e)
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
                txtGZMax.Text = "";
                var type = cbGZMarket.Checked
                               ? DoAccountManager.TypesOrderPriceType.OPTMarketPrice
                               : DoAccountManager.TypesOrderPriceType.OPTLimited;
                var traderId = ServerConfig.TraderID;
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
        /// 股指期货右击清空按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
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
        /// 清空股指期货回推信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSIOrder_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer.Close();
            gzqhTimer.Close();
            isAutoOrder = false;
            //关闭自动撤单事件
            if (gzAutoTime != null)
            {
                gzAutoTime.Enabled = false;
            }
        }

        /// <summary>
        /// 刷新资金事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtGZAvailableCapital_MouseDown(object sender, MouseEventArgs e)
        {
            QueryGZQHCapital();
        }

        /// <summary>
        /// 价格回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtGZPrice_KeyPress(object sender, KeyPressEventArgs e)
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
        private void txtGZMax_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                Max();
            }
        }

        /// <summary>
        /// 双击委托单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewGZQH_DoubleClick(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridViewGZQH.SelectedRows)
            {
                QHMessage message = row.DataBoundItem as QHMessage;
                if (message == null)
                {
                    continue;
                }
                GZQHMessageLogic.FireEntrustSelectedEvent(message.EntrustNumber);
            }
        }

        #endregion  股指期货下单操作

        #region IOrderCallBackView<FutureDealOrderPushBack> 成员

        /// <summary>
        /// 商品期货回报信息
        /// </summary>
        /// <param name="drmip"></param>
        public void ProcessPushBack(FutureDealOrderPushBack drmip)
        {
            var tet = drmip.StockIndexFuturesOrde;
            var deals = drmip.FutureDealList;

            string format =
                "<--PushBack[EntrustNumber={0},Code={1},TradeAmount={2},CancelAmount={3},OrderStatusId={4},OrderMessage={5},DealsCount={6}]  Time={7}";
            string desc = string.Format(format, tet.EntrustNumber, tet.ContractCode, tet.TradeAmount, tet.CancelAmount,
                                        Utils.GetOrderStateMsg(tet.OrderStatusId), "",
                                        deals.Count, DateTime.Now.ToLongTimeString());

            // LogHelper.WriteDebug(desc);

            //string dealsDesc = GetGZQHDealsDesc(deals);

            WriteGZQHMsg(desc);
            if (isAutoOrder)
            {
                if (tet.OrderStatusId > 5 && tet.OrderStatusId != 9)
                {
                    gzqhLogic.DelateAutoCanceOrder(tet.EntrustNumber);
                }
            }
            gzqhLogic.ProcessPushBack(drmip);
        }

        #endregion

        #region 股指期货批量下单操作
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
            try
            {
                #region 获取Excel表格中所有数据
                string Path = this.txtGZQHPath.Text;
                if (!string.IsNullOrEmpty(Path))
                {
                    DataSet myDataSet = new DataSet();
                    myDataSet = orderSql.dataSet(Path);
                    if (myDataSet != null)
                    {
                        try
                        {
                            string Code, BuySell, UnitType, OpenClose, price, PortfoliosId;
                            float OrderAmount;
                            int batch;
                            bool cbMarketOrder;
                            for (int i = 0; i < myDataSet.Tables[0].Rows.Count; i++)
                            {

                                Code = myDataSet.Tables[0].Rows[i][0].ToString();
                                OrderAmount = float.Parse(myDataSet.Tables[0].Rows[i][1].ToString());
                                batch = int.Parse(myDataSet.Tables[0].Rows[i][2].ToString());
                                BuySell = myDataSet.Tables[0].Rows[i][3].ToString();
                                UnitType = myDataSet.Tables[0].Rows[i][4].ToString();
                                cbMarketOrder = bool.Parse(myDataSet.Tables[0].Rows[i][5].ToString());
                                OpenClose = myDataSet.Tables[0].Rows[i][6].ToString();
                                price = myDataSet.Tables[0].Rows[i][7].ToString();
                                PortfoliosId = myDataSet.Tables[0].Rows[i][8].ToString();
                                if (string.IsNullOrEmpty(PortfoliosId))
                                {
                                    PortfoliosId = "";
                                }
                                GZQHOrder(Code, OrderAmount, batch, BuySell, UnitType, cbMarketOrder, OpenClose, price, PortfoliosId);

                            }
                        }
                        catch (Exception ex)
                        {
                            string MesageError = ResourceOperate.Instanse.GetResourceByKey("MesageError");
                            errPro.Clear();
                            errPro.SetError(btnGZQHPath, "Code" + MesageError);
                            LogHelper.WriteError(ex.Message, ex);
                        }
                    }
                    else
                    {
                        string MesageData = ResourceOperate.Instanse.GetResourceByKey("MesageData");
                        errPro.Clear();
                        errPro.SetError(btnGZQHPath, "Code" + MesageData);
                    }
                }
                else
                {
                    errPro.Clear();
                    string Mesage = ResourceOperate.Instanse.GetResourceByKey("Mesage");
                    errPro.SetError(btnGZQHPath, Mesage);
                }
                #endregion 获取Excel表格中所有数据
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
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
        private void GZQHOrder(string Code, float OrderAmount, int batch, string BuySell, string UnitType, bool cbMarketOrder, string OpenClose, string price, string PortfoliosId)
        {
            try
            {
                #region 将获取到的Excel表格中数据进行组装成下单实体

                string errMsg = "";
                string high;
                string low;
                errPro.Clear();
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
                    errPro.Clear();
                    errPro.SetError(btnGZQHPath, OrdreName + Code + BS + Exception);
                }
                order.FundAccountId = ServerConfig.GZQHCapitalAccount; //"010000002306";

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
                        float pric;
                        if (float.TryParse(price, out pric))
                        {
                            order.OrderPrice = pric;
                        }
                        #region 获取价格上下限

                        //var highLowRange = wcfLogic.GetHighLowRangeValueByCommodityCode(Code, prices, out errMsg);
                        //var highLowRange = wcfLogic.GetHighLowRangeValueByCommodityCode(Code, prices, out errMsg);
                        //if (!string.IsNullOrEmpty(errMsg))
                        //{
                        //    errPro.SetError(txtGZPrice, errMsg);
                        //}
                        //if (highLowRange == null)
                        //{
                        //    //MessageBox.Show("Can not get highlowrange object!");
                        //    string errMessage = ResourceOperate.Instanse.GetResourceByKey("highlowrange");
                        //    // string errors = "Can not get highlowrange object!";
                        //    error.Clear();
                        //    MessageBox.Show(OrdreName + errMessage);
                        //    return;
                        //}

                        //if (highLowRange.RangeType == Types.HighLowRangeType.HongKongPrice) //港股类型处理
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
                        //else //其它类型处理
                        //{
                        //    high = Utils.Round(highLowRange.HighRangeValue).ToString();
                        //    low = Utils.Round(highLowRange.LowRangeValue).ToString();
                        //}

                        #endregion 获取价格上下限

                        #region 价格判断

                        //bool SeesawPrice = ServerConfig.Price;
                        //float highs = 0;
                        //float lows = 0;
                        //float pric = 0;
                        //if (float.TryParse(high, out highs) && float.TryParse(low, out lows) &&
                        //    float.TryParse(price, out pric))
                        //{
                        //    if (SeesawPrice == true)
                        //    {
                        //        order.OrderPrice = pric;
                        //        return;
                        //    }
                        //    else
                        //    {
                        //        if (!string.IsNullOrEmpty(high) && !string.IsNullOrEmpty(low))
                        //        {

                        //            if (pric <= highs && pric >= lows)
                        //            {
                        //                order.OrderPrice = pric;
                        //            }
                        //            else
                        //            {
                        //                string PriceErrors = ResourceOperate.Instanse.GetResourceByKey("PriceErrors");
                        //                MessageBox.Show(OrdreName + PriceErrors);
                        //                return;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            string HighLowException =
                        //                ResourceOperate.Instanse.GetResourceByKey("HighLowException");
                        //            MessageBox.Show(HighLowException);
                        //            return;
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    string HighLow = ResourceOperate.Instanse.GetResourceByKey("HighLow");
                        //    MessageBox.Show(HighLow);
                        //    return;
                        //}

                        #endregion 价格判断
                    }
                    else
                    {
                        errPro.Clear();
                        string GZPriceError = ResourceOperate.Instanse.GetResourceByKey("NullError");
                        errPro.Clear();
                        errPro.SetError(btnGZQHPath, GZPriceError);
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

                order.PortfoliosId = PortfoliosId;
                order.TraderId = ServerConfig.TraderID; //"23";
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

                #endregion 将获取的Excel表格中数据进行组装成下单实体
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }
        #region 弹出选择文件提示框
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
        #endregion 弹出选择文件提示框
        #endregion 股指期货批量下单操作

        #region 自动下单
        /// <summary>
        /// 双击索引文本框事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtGZQHIndex_DoubleClick(object sender, EventArgs e)
        {
            txtGZQHIndex.Text = cbGZCode.SelectedIndex.ToString();
        }

        /// <summary>
        /// 自动下单按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutoOrder_Click(object sender, EventArgs e)
        {
            if (isAutoOrder)
            {
                txtGZQHIndex.Enabled = true;
                //cbGZCode.Enabled = true;
                isAutoOrder = false;
                btnAutoOrder.Text = ResourceOperate.Instanse.GetResourceByKey("AutoOrder");
                //关闭自动撤单事件
                if (gzAutoTime != null)
                {
                    gzAutoTime.Enabled = false;
                }
            }
            else
            {
                //不可用
                //cbGZCode.Enabled = false;
                txtGZQHIndex.Enabled = false;
                isAutoOrder = true;
                btnAutoOrder.Text = ResourceOperate.Instanse.GetResourceByKey("AutoOrders");
                //自动撤单事件
                if (chkAutoGZQHCacle.Checked && gzAutoTime == null)
                {
                    gzAutoTime = new System.Timers.Timer();
                    gzAutoTime.Interval = 60 * 1000;
                    gzAutoTime.Elapsed += new System.Timers.ElapsedEventHandler(autoTime_Elapsed);
                }
                if (gzAutoTime != null)
                {
                    gzAutoTime.Enabled = chkAutoGZQHCacle.Checked;
                }
                #region 组装自动下单实体
                AutoDoOrder order = new AutoDoOrder();
                order.BuySell = this.cbGZBuySell.SelectedIndex;

                order.OrderPriceType = this.cbGZMarket.Checked ? 0 : 1;
                order.OrderUnitType = cbGZUnit.Text.Trim();
                order.PortfoliosID = this.txtGZQHProtfolioLogo.Text;

                int amt = 1;
                if (!int.TryParse(txtGZAmount.Text, out amt))
                {
                    amt = 1;
                }
                order.OrderAmount = amt;
                order.IndexStart = cbGZCode.SelectedIndex;
                int end = 0;
                if (!int.TryParse(txtGZQHIndex.Text, out end))
                {
                    end = cbGZCode.Items.Count;
                }
                if (end > cbGZCode.Items.Count)
                {
                    end = cbGZCode.Items.Count;
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

                order.CodeCount = cbGZCode.Items.Count;

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
                order.FutureOpenCloseType = cbGZOpenClose.Text;

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
        /// <param name="openCloseType">开平仓</param>
        /// <param name="portfoliosID">投组标识</param>
        /// <param name="orderWay">委托现价限价</param>
        private void AutoGZQHOrder(string code, float amount, string fundAccountID, Types.TransactionDirection type, Types.UnitType unitType, string tradeID,
            TypesFutureOpenCloseType openCloseType, string portfoliosID, TypesOrderPriceType orderWay)
        {
            try
            {
                #region 组装下单实体
                if (amount < 0)
                {
                    return;
                }
                StockIndexFuturesOrderRequest order = new StockIndexFuturesOrderRequest();
                order.Code = code;
                order.BuySell = type;
                order.FundAccountId = fundAccountID;
                order.OrderAmount = amount;

                #region 价格处理
                if (TypesOrderPriceType.OPTMarketPrice != orderWay)
                {
                    string errMsg = "";
                    MarketDataLevel leave = wcfLogic.GetLastPricByCommodityCode(code, 3, out errMsg);
                    if (leave != null)
                    {
                        order.OrderPrice = float.Parse(leave.LastPrice.ToString());
                    }
                    //如果获取不到价格不下单
                    if (order.OrderPrice == 0)
                    {
                        return;
                    }

                }
                #endregion 价格处理

                order.OrderUnitType = unitType; //Utils.GetUnit(cmbSPQHUnit.Text.Trim());
                order.OrderWay = orderWay;
                order.TraderId = tradeID; //ServerConfig.TraderID; //"23";
                order.TraderPassword = "";
                order.OpenCloseType = openCloseType;// Utils.GetFutureOpenCloseType(cmbSPQHOpenClose.Text.Trim());
                order.PortfoliosId = portfoliosID;// this.txtSPQHProtfolioLogo.Text;

                smartPool.QueueWorkItem(DoGZQHOrder, order);

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
            if (!int.TryParse(txtGZQHCancleMin.Text, out minute))
            {
                minute = 3;
            }
            gzqhLogic.DisposeAutoCance(minute);
        }
        /// <summary>
        /// 异步自动下单操作方法
        /// <param name="index">开始索引列</param>
        /// <param name="order"></param>
        /// </summary>
        private void AutoOrderInvoker(AutoDoOrder order)
        {
            try
            {

                List<QH_HoldAccountTableInfo> holdList = null;
                //先把之前的填写的下单量记录下来
                int orderVoume = order.OrderAmount;


                string fundAccountID = ServerConfig.GZQHCapitalAccount;

                Types.TransactionDirection type = order.BuySell == 0
                                   ? Types.TransactionDirection.Buying
                                   : Types.TransactionDirection.Selling;
                Types.UnitType unitType = Utils.GetUnit(order.OrderUnitType.Trim()); ;
                string tradeID = ServerConfig.TraderID; ;
                TypesFutureOpenCloseType openCloseType = Utils.GetFutureOpenCloseType(order.FutureOpenCloseType.Trim());
                string portfoliosID = order.PortfoliosID;
                TypesOrderPriceType orderWay = order.OrderPriceType == 0
                                         ? TypesOrderPriceType.OPTMarketPrice
                                         : TypesOrderPriceType.OPTLimited;

                do
                {
                    #region 这里写死的时间判断，如果以后要修改再从管理中心获取添加服务
                    DateTime nowTime = wcfLogic.CheckDoAccoumtChannel();
                    //5	2009-12-25 09:15:00.000	2009-12-25 11:30:00.000	3
                    //6	2009-12-25 13:00:00.000	2009-12-25 15:15:00.000	3
                    //如果不在交易开市时间不作自动下单
                    if (!(nowTime >= new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 9, 15, 0) && nowTime <= new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 11, 30, 0))
                        && !(nowTime >= new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 13, 0, 0) && nowTime <= new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 15, 15, 0)))
                    {
                        WriteGZQHMsg("不在交易时间内不能操作自动下单功能!");
                        btnAutoOrder_Click(this, null);
                        break;
                    }
                    #endregion

                    if (order.IsHoldAccount)
                    {
                        holdList = wcfLogic.GZQHHold;
                        //当前下单量
                        decimal orderCount = orderVoume;
                        //选择记录之前选择的开平仓类型,这里的值要设置与cbGZOpenClose的下拉列表框的索引一致 0-开仓1，1--平仓（历史）2，2--平今3
                        openCloseType = TypesFutureOpenCloseType.ClosePosition;
                        ////买卖方向
                        //int buySell = 0;
                        foreach (var hold in holdList)
                        {

                            if (!isAutoOrder)
                            {
                                break;
                            }

                            #region 判断持仓及平仓类型（平今/历史）
                            //当今日持仓量小于0那么从历史中获取
                            if (hold.TodayHoldAmount <= 0)
                            {
                                //如果历史也小于0那么本次下单无效不下单
                                if (hold.HistoryHoldAmount <= 0)
                                {
                                    continue;
                                }
                                else//如果历史持仓不为0那么下历史持仓
                                {

                                    //如果历史小于填写的就直接
                                    if (hold.HistoryHoldAmount < orderVoume)
                                    {
                                        orderCount = hold.HistoryHoldAmount;
                                    }
                                    openCloseType = TypesFutureOpenCloseType.ClosePosition;
                                }

                            }
                            else
                            {
                                //如果当日持仓小于填写的就直接用当用持仓量来下单
                                if (hold.TodayHoldAmount < orderVoume)
                                {
                                    orderCount = hold.TodayHoldAmount;
                                }
                                openCloseType = TypesFutureOpenCloseType.CloseTodayPosition;

                            }
                            #endregion

                            #region 修改卖还是买
                            // Buying = 1买,    Selling = 2卖,
                            if (hold.BuySellTypeId == 1)
                            {
                                type = Types.TransactionDirection.Selling;
                            }
                            else
                            {
                                type = Types.TransactionDirection.Buying;
                            }
                            #endregion


                            //this.Invoke(new MethodInvoker(() =>
                            //{
                            //    cbGZCode.Text = hold.Contract;
                            //    txtGZAmount.Text = orderCount.ToString();
                            //    cbGZOpenClose.SelectedIndex = openCloseType;
                            //    cbGZBuySell.SelectedIndex = buySell;
                            //    //强制点击
                            //    btnGZSendOrder_Click(this, null);

                            //    if (orderCount != orderVoume)
                            //    {
                            //        txtGZAmount.Text = orderVoume.ToString();
                            //    }

                            //}));
                            AutoGZQHOrder(hold.Contract, (float)orderCount, fundAccountID, type, unitType, tradeID, openCloseType, portfoliosID, orderWay);

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
                            //    this.cbGZCode.SelectedIndex = k;
                            //    //强制点击
                            //    btnGZSendOrder_Click(this, null);
                            //}));
                            AutoGZQHOrder(AllCodeList[k], (float)orderVoume, fundAccountID, type, unitType, tradeID, openCloseType, portfoliosID, orderWay);

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
            catch (Exception ex)
            {
                LogHelper.WriteError("测试工具自动下单股指期货异常" + ex.Message, ex);
            }
        }

        /// <summary>
        /// 自动下单对持仓操作选择框方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbHoldAccount_CheckedChanged(object sender, EventArgs e)
        {
            cbGZBuySell.Enabled = !chbHoldAccount.Checked;
            cbGZOpenClose.Enabled = !chbHoldAccount.Checked;

        }
        #endregion

    }
}
