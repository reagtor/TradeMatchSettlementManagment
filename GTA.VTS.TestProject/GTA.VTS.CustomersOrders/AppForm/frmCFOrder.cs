using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

using Amib.Threading;
using GTA.VTS.CustomersOrders.BLL;
using GTA.VTS.CustomersOrders.DoDealRptService;
using GTA.VTS.Common.CommonUtility;
using GTA.VTS.CustomersOrders.DoOrderService;
using GTA.VTS.CustomersOrders.DoAccountManager;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.CustomersOrders.DoCommonQuery;
using System.Threading;

namespace GTA.VTS.CustomersOrders.AppForm
{
    public partial class frmCFOrder : MdiFormBase, IOrderCallBackView<FutureDealOrderPushBack>
    {
        #region 变量

        /// <summary>
        /// 自动下单状态（是否正在自动下单）
        /// </summary>
        private bool isAutoOrder = false;


        /// <summary>
        /// WCF服务访问对象
        /// </summary>
        private WCFServices wcfLogic;

        /// <summary>
        /// 消息逻辑访问对象
        /// </summary>
        private SPQHMessageLogic spqhLogic;
        /// <summary>
        /// 读取Excel表格数据
        /// </summary>
        private OrderSQLHelper orderSql = new OrderSQLHelper();
        /// <summary>
        /// 线程池
        /// </summary>
        private SmartThreadPool smartPool = new SmartThreadPool { MaxThreads = 200, MinThreads = 25 };

        private int spqhDoOrderNum;
        private string title = "";

        private bool isProcessing;
        private bool isClearSPQH;
        private bool isReBindSPQHData;
        private int spqhIndex;

        private System.Timers.Timer spqhTimer = new System.Timers.Timer();
        private System.Timers.Timer timer = new System.Timers.Timer();
        /// <summary>
        /// 商品期货自动下单后自动撤单触发事件
        /// </summary>
        private System.Timers.Timer spAutoTime = null;

        /// <summary>
        /// 所有代码列表
        /// </summary>
        private List<string> AllCodeList = new List<string>();

        #endregion

        #region 构造函数

        public frmCFOrder()
        {
            InitializeComponent();
            OrderCallBack.CFView = this;
            spqhLogic = new SPQHMessageLogic();
            //自动撤单委托 事件
            spqhLogic.AutoEvent += new AutoCancelOrder(this.CancelSPQHOrder);

            wcfLogic = WCFServices.Instance;
            LocalhostResourcesFormText();
        }



        #endregion

        #region 窗体事件

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCFOrder_Load(object sender, EventArgs e)
        {
            #region 自动补全

            this.cbSPCode.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.cbSPCode.AutoCompleteSource = AutoCompleteSource.ListItems;
            AllCodeList = wcfLogic.GetAllCode(2);
            //如果没有数据还是用之前添加在Item里的数据
            if (AllCodeList != null && AllCodeList.Count > 0)
            {
                this.cbSPCode.DataSource = AllCodeList;
            }
            else
            {   //为了后面自动下单作准备使用
                AllCodeList = new List<string>();
                foreach (var item in cbSPCode.Items)
                {
                    AllCodeList.Add(item.ToString());
                }
            }
            #endregion 自动补全

            smartPool.Start();

            ////定时刷新资金账户信息
            timer.Interval = 1000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;

            //   txtSPQHTradeID.Text = ServerConfig.TraderID;
            //  txtSPQHCapital.Text = ServerConfig.SPQHCapitalAccount;

            cmbSPQHBuysell.SelectedIndex = 0;
            cmbSPQHUnit.SelectedIndex = 0;
            cmbSPQHOpenClose.SelectedIndex = 0;
            cbSPCode.SelectedIndex = 0;
            this.txtSPQHProtfolioLogo.Text = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            QuerySPQHCapital();
        }
        /// <summary>
        /// 刷新显示相关语方类型的标签
        /// </summary>
        private void LocalhostResourcesFormText()
        {
            this.Text = ResourceOperate.Instanse.GetResourceByKey("menuSPQHOrder");
            #region 自动下单多语言
            this.grbAutomaticorders.Text = ResourceOperate.Instanse.GetResourceByKey("Automaticorders");
            this.lbxhEndIndex.Text = ResourceOperate.Instanse.GetResourceByKey("EndIndx");
            this.lbTimeSpan.Text = ResourceOperate.Instanse.GetResourceByKey("TimeSpan");
            this.chbHoldAccount.Text = ResourceOperate.Instanse.GetResourceByKey("HoldAccounts");
            this.btnAutoOrder.Text = ResourceOperate.Instanse.GetResourceByKey("AutoOrder");
            this.toolTip1.SetToolTip(this.txtSPQHIndex, ResourceOperate.Instanse.GetResourceByKey("Index"));
            this.chkAutoSPQHCacle.Text = ResourceOperate.Instanse.GetResourceByKey("Automatic");
            this.toolTip1.SetToolTip(this.txtTimeSapn, ResourceOperate.Instanse.GetResourceByKey("TimeSapn"));
            #endregion

            #region 商品期货多语言显示
            this.lblSPQHContract.Text = ResourceOperate.Instanse.GetResourceByKey("lblCode");
            this.lblSPQHAmount.Text = ResourceOperate.Instanse.GetResourceByKey("Amount");
            //  this.lblSPQHTradeID.Text = ResourceOperate.Instanse.GetResourceByKey("lblTradeID");
            //  this.lblSPQHCapital.Text = ResourceOperate.Instanse.GetResourceByKey("lblCapital");
            this.lblSPQHPrice.Text = ResourceOperate.Instanse.GetResourceByKey("Price");
            // this.lblSPQHBatch.Text = ResourceOperate.Instanse.GetResourceByKey("lblBatch");
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
            #region 商品期货下单数据绑定显示
            for (int i = 0; i < this.dataGridViewSPQH.ColumnCount; i++)
            {
                string SPQHName = dataGridViewSPQH.Columns[i].HeaderText;
                dataGridViewSPQH.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(SPQHName);
            }
            #endregion 商品期货下单数据绑定显示3
        }

        /// <summary>
        /// 商品期货下单按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSPQHOrder_Click(object sender, EventArgs e)
        {
            try
            {
                #region 组装下单实体
                errPro.Clear();
                MercantileFuturesOrderRequest order = new MercantileFuturesOrderRequest();
                order.Code = cbSPCode.Text.Trim();
                order.BuySell = cmbSPQHBuysell.SelectedIndex == 0
                                    ? Types.TransactionDirection.Buying
                                    : Types.TransactionDirection.Selling;
                order.FundAccountId = ServerConfig.SPQHCapitalAccount; //"010000002306";
                order.OrderAmount = float.Parse(txtSPQHAmount.Text.Trim());

                #region 价格处理
                //if (isAutoOrder)
                //{
                //    //强制点击获取价格
                //    order.OrderPrice = float.Parse(Price().ToString());
                //    if (order.OrderPrice == 0)
                //    {
                //        return;
                //    }
                //}
                //else
                //{
                if (!string.IsNullOrEmpty(txtSPQHPrice.Text.Trim()))
                {
                    #region
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
                    #endregion
                }
                else
                {
                    errPro.Clear();
                    string GZPriceError = ResourceOperate.Instanse.GetResourceByKey("NullError");
                    errPro.SetError(txtSPQHPrice, "Price" + GZPriceError);
                    return;
                }
                //}


                #endregion 价格处理

                order.OrderUnitType = Utils.GetUnit(cmbSPQHUnit.Text.Trim());
                order.OrderWay = GTA.VTS.CustomersOrders.DoOrderService.TypesOrderPriceType.OPTLimited;
                // order.PortfoliosId = "p2";
                order.TraderId = ServerConfig.TraderID; //"23";
                order.TraderPassword = "";
                order.OpenCloseType = Utils.GetFutureOpenCloseType(cmbSPQHOpenClose.Text.Trim());
                order.PortfoliosId = this.txtSPQHProtfolioLogo.Text;
                int batch = 1;
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
                //this.txtSPQHProtfolioLogo.Text = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                #endregion 组装下单实体
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }



        /// <summary>
        /// 双击商品期货价格事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSPQHPrice_DoubleClick(object sender, EventArgs e)
        {
            Price();
        }

        /// <summary>
        /// 根据股票代码获取上下限和股票行情信息
        /// </summary>
        private decimal Price()
        {
            decimal price = 0;
            try
            {
                errPro.Clear();
                string errMsg = "";
                MarketDataLevel leave = wcfLogic.GetLastPricByCommodityCode(cbSPCode.Text.Trim(), 2, out errMsg);
                if (leave != null)
                {
                    price = leave.LastPrice;
                    //自动下单不作处理
                    //if (!isAutoOrder)
                    //{
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
                    //}
                }
                //if (!isAutoOrder)
                //{
                if (!string.IsNullOrEmpty(errMsg))
                {
                    errPro.SetError(txtSPQHPrice, errMsg);
                }
                txtSPQHPrice.Text = Utils.Round(price).ToString();
                SetSPHighLowValue();
                //}

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
            return price;
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
        /// 清空信息栏中信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox6.Items.Clear();
        }

        /// <summary>
        /// 清空表格数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (isClearSPQH)
                return;

            isClearSPQH = true;
            spqhLogic.ClearAll();
            //xHMessageLogicBindingSource.Clear();         
            isClearSPQH = false;

            ReBindSPQHData();
        }

        /// <summary>
        /// 列表颜色设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewSPQH_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

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
                spqhIndex = row.Index;

                int index = e.ColumnIndex;
                if (index != 0)
                    return;

                QHMessage message = row.DataBoundItem as QHMessage;
                if (message == null)
                {
                    continue;
                }

                string errMsg = "";
                if (!CancelSPQHOrder(message.EntrustNumber, ref errMsg))
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
        public bool CancelSPQHOrder(string entrustNumber, ref string errMsg)
        {
            errMsg = "";
            bool isSuccess = wcfLogic.CancelSPQHOrder(entrustNumber, ref errMsg);
            if (!isSuccess)
            {
                string msg = "股指期货委托[" + entrustNumber + "]撤单失败！" + errMsg;
                spqhLogic.UpdateMessage(entrustNumber, errMsg);
            }
            return isSuccess;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewSPQH_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SetGridNumber(this.dataGridViewSPQH, e);
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCFOrder_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer.Close();
            spqhTimer.Close();
            isAutoOrder = false;
            //关闭自动撤单事件
            if (spAutoTime != null)
            {
                spAutoTime.Enabled = false;
            }
        }

        /// <summary>
        /// 点击可用资金刷新资金事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSPQHACapital_MouseDown(object sender, MouseEventArgs e)
        {
            QuerySPQHCapital();
        }

        /// <summary>
        /// 双击委托单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewSPQH_DoubleClick(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridViewSPQH.SelectedRows)
            {
                QHMessage message = row.DataBoundItem as QHMessage;
                if (message == null)
                {
                    continue;
                }
                SPQHMessageLogic.FireEntrustSelectedEvent(message.EntrustNumber);
            }
        }

        #endregion

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

            WriteSPQHMsg(desc);

            if (isAutoOrder)
            {
                if (tet.OrderStatusId > 5 && tet.OrderStatusId != 9)
                {
                    spqhLogic.DelateAutoCanceOrder(tet.EntrustNumber);
                }
            }

            spqhLogic.ProcessPushBack(drmip);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 商品期货下单
        /// </summary>
        /// <param name="order"></param>
        private void DoSPQHOrder(MercantileFuturesOrderRequest order)
        {
            var res = wcfLogic.DoSPQHOrder(order);

            if (isAutoOrder)
            {
                if (res.IsSuccess)
                {
                    spqhLogic.AddAutoCanceOrder(res.OrderId, DateTime.Now);
                }
            }

            spqhLogic.ProcessDoOrder(res, order);

            spqhDoOrderNum++;

            //WriteTitle(spqhDoOrderNum);

            string format =
                "DoOrder[EntrustNumber={0},Code={1},EntrustAmount={2},EntrustPrice={3},BuySell={4},OpenClose={5},TraderId={6},OrderMessage={7},IsSuccess={8}] Time={9}";
            string desc = string.Format(format, res.OrderId, order.Code, order.OrderAmount, order.OrderPrice,
                                        order.BuySell, order.OpenCloseType, order.TraderId, res.OrderMessage,
                                        res.IsSuccess, DateTime.Now.ToLongTimeString());
            WriteSPQHMsg(desc);
            // LogHelper.WriteDebug(desc);
        }

        /// <summary>
        /// 获取商品期货的上下限
        /// </summary>
        private void SetSPHighLowValue()
        {
            decimal price = 0;
            string errMsg = "";
            if (!string.IsNullOrEmpty(txtSPQHPrice.Text))
            {
                bool isSuccess = decimal.TryParse(txtSPQHPrice.Text.Trim(), out price);
                if (!isSuccess)
                {
                    errPro.Clear();
                    string errMessage = ResourceOperate.Instanse.GetResourceByKey("PleaseError");
                    errPro.SetError(txtSPQHPrice, errMessage);
                    return;
                }
            }

            var highLowRange = wcfLogic.GetHighLowRangeValueByCommodityCode(cbSPCode.Text.Trim(), price, out errMsg);

            if (highLowRange == null)
            {
                string errMessage = ResourceOperate.Instanse.GetResourceByKey("highlowrange");
                errPro.SetError(txtSPQHHigh, errMessage);
                return;
            }
            if (!string.IsNullOrEmpty(errMsg))
            {
                errPro.SetError(txtSPQHPrice, errMsg);
            }
            if (highLowRange.RangeType == Types.HighLowRangeType.HongKongPrice) //港股类型处理
            {
                var hkrv = highLowRange.HongKongRangeValue;

                var buySell = cmbSPQHBuysell.SelectedIndex == 0
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
        /// 处理回推信息
        /// </summary>
        /// <param name="i"></param>
        private void WriteTitle(int i)
        {
            string msg = "[" + i + "]";
            this.Invoke(new MethodInvoker(() => { this.Text = this.title + msg; }));
        }

        /// <summary>
        /// 商品期货的信息
        /// </summary>
        /// <param name="msg"></param>
        private void WriteSPQHMsg(string msg)
        {
            MessageDisplayHelper.Event(msg, listBox6);
        }
        #region 定时刷新资金账户信息
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

            //QuerySPQHCapital();
            ReBindSPQHData();

            isProcessing = false;
        }
        #endregion 定时刷新资金账户信息
        /// <summary>
        /// 查询出商品期货资金并显示
        /// </summary>
        private void QuerySPQHCapital()
        {
            try
            {
                string capitalAccount = ServerConfig.SPQHCapitalAccount;
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
                LogHelper.WriteError(ex.Message, ex);
            }
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

            QuerySPQHCapital();

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
        /// 获取商品期货的最大委托量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSPQHMax_DoubleClick(object sender, EventArgs e)
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
                txtSPQHMax.Text = "";
                var type = DoAccountManager.TypesOrderPriceType.OPTLimited;
                var traderId = ServerConfig.TraderID;
                var code = cbSPCode.Text.Trim();

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
                    errPro.SetError(txtSPQHMax, errMsg);
                    return;
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

        /// <summary>
        /// 价格回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSPQHPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                Price();
            }
        }

        /// <summary>
        /// 最大委托量委托事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSPQHMax_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                Max();
            }
        }
        #endregion

        #region 批量下单
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
        /// <summary>
        /// 商品期货批量下单按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSPQHBatchOrder_Click(object sender, EventArgs e)
        {
            try
            {
                #region 获取导入的Excel表格中数据

                string Path = this.txtSPQHPath.Text;
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
                            for (int i = 0; i < myDataSet.Tables[0].Rows.Count; i++)
                            {

                                Code = myDataSet.Tables[0].Rows[i][0].ToString();
                                OrderAmount = float.Parse(myDataSet.Tables[0].Rows[i][1].ToString());
                                batch = int.Parse(myDataSet.Tables[0].Rows[i][2].ToString());
                                BuySell = myDataSet.Tables[0].Rows[i][3].ToString();
                                UnitType = myDataSet.Tables[0].Rows[i][4].ToString();
                                OpenClose = myDataSet.Tables[0].Rows[i][5].ToString();
                                price = myDataSet.Tables[0].Rows[i][6].ToString();
                                PortfoliosId = myDataSet.Tables[0].Rows[i][7].ToString();
                                if (string.IsNullOrEmpty(PortfoliosId))
                                {
                                    PortfoliosId = "";
                                }
                                SPQHOrder(Code, OrderAmount, batch, BuySell, UnitType, OpenClose, price, PortfoliosId);

                            }
                        }
                        catch (Exception ex)
                        {
                            string MesageError = ResourceOperate.Instanse.GetResourceByKey("MesageError");
                            errPro.Clear();
                            errPro.SetError(btnSPQHPath, MesageError);
                            LogHelper.WriteError(ex.Message, ex);
                        }
                    }
                    else
                    {
                        string MesageData = ResourceOperate.Instanse.GetResourceByKey("MesageData");
                        errPro.Clear();
                        errPro.SetError(btnSPQHPath, MesageData);
                    }
                }
                else
                {
                    errPro.Clear();
                    string Mesage = ResourceOperate.Instanse.GetResourceByKey("Mesage");
                    errPro.SetError(btnSPQHPath, Mesage);
                }

                #endregion 获取导入的Excel表格中数据
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
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
        private void SPQHOrder(string Code, float OrderAmount, int batch, string BuySell, string UnitType, string OpenClose, string price, string PortfoliosId)
        {
            try
            {
                #region  获取Excel表格中数据并添加到下单实体中
                errPro.Clear();
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
                    errPro.SetError(btnSPQHPath, "Code" + CodeError);
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

                order.FundAccountId = ServerConfig.SPQHCapitalAccount; //"010000002306";
                order.OrderAmount = OrderAmount;
                if (!string.IsNullOrEmpty(price))
                {
                    int pric;
                    if (int.TryParse(price, out pric))
                    {
                        order.OrderPrice = pric;
                    }
                    #region 对价格进行判断
                    //判断Price是否等于零，如果为空则弹出错误提示框并退出
                    //decimal prices = 0;
                    //if (decimal.TryParse(price, out prices))
                    //{
                    //    if (prices == 0)
                    //    {
                    //        errPro.Clear();
                    //        string GZPricezeroError = ResourceOperate.Instanse.GetResourceByKey("zero");
                    //        errPro.SetError(txtSPQHPrice, "Price" + GZPricezeroError);
                    //        return;
                    //    }
                    //}
                    //else
                    //{
                    //    errPro.Clear();
                    //    string GZPriceDataError = ResourceOperate.Instanse.GetResourceByKey("Dataillegal");
                    //    errPro.SetError(txtSPQHPrice, "Price" + GZPriceDataError);
                    //    return;
                    //}
                    //#region 获取价格上下限
                    //var highLowRange = wcfLogic.GetHighLowRangeValueByCommodityCode(Code, prices, out errMsg);

                    //if (highLowRange == null)
                    //{
                    //    string errMessage = ResourceOperate.Instanse.GetResourceByKey("highlowrange");
                    //    // string errors = "Can not get highlowrange object!";
                    //    error.Clear();
                    //    error.SetError(txtSPQHHigh, errMessage);
                    //    //MessageBox.Show("Can not get highlowrange object!");
                    //    return;
                    //}
                    //if (!string.IsNullOrEmpty(errMsg))
                    //{
                    //    errPro.SetError(txtSPQHPrice, errMsg);
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
                    //#endregion 获取价格上下限
                    //#region 价格判断
                    //bool SeesawPrice = ServerConfig.Price;
                    //float highs = 0;
                    //float lows = 0;
                    //float pric = 0;
                    //if (float.TryParse(high, out highs) && float.TryParse(low, out lows) &&
                    //        float.TryParse(price, out pric))
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

                    //        }
                    //        else
                    //        {
                    //            string PriceErrors = ResourceOperate.Instanse.GetResourceByKey("PriceErrors");
                    //            errPro.SetError(txtSPQHPrice, PriceErrors);
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
                    //#endregion 价格判断
                    #endregion 对价格进行判断
                }
                else
                {
                    string HighLowException = ResourceOperate.Instanse.GetResourceByKey("HighLowException");
                    errPro.Clear();
                    errPro.SetError(btnSPQHPath, "Code" + HighLowException);
                    return;
                }
                //}
                order.OrderUnitType = Utils.GetUnit(UnitType);
                order.OrderWay = GTA.VTS.CustomersOrders.DoOrderService.TypesOrderPriceType.OPTLimited;
                order.PortfoliosId = PortfoliosId;
                order.TraderId = ServerConfig.TraderID; //"23";
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
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }
        #endregion

        #region 自动下单
        /// <summary>
        /// 双击索引文本框事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSPQHIndex_DoubleClick(object sender, EventArgs e)
        {
            txtSPQHIndex.Text = cbSPCode.SelectedIndex.ToString();
        }
        /// <summary>
        /// 点击自动下单按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutoOrder_Click(object sender, EventArgs e)
        {
            if (isAutoOrder)
            {
                txtSPQHIndex.Enabled = true;
                //cbSPCode.Enabled = true;
                isAutoOrder = false;
                btnAutoOrder.Text = ResourceOperate.Instanse.GetResourceByKey("AutoOrder");
                //关闭自动撤单事件
                if (spAutoTime != null)
                {
                    spAutoTime.Enabled = false;
                }
            }
            else
            {
                //不可用
                //cbSPCode.Enabled = false;
                txtSPQHIndex.Enabled = false;
                isAutoOrder = true;
                btnAutoOrder.Text = ResourceOperate.Instanse.GetResourceByKey("AutoOrders");

                //自动撤单事件
                if (chkAutoSPQHCacle.Checked && spAutoTime == null)
                {
                    spAutoTime = new System.Timers.Timer();
                    spAutoTime.Interval = 60 * 1000;
                    spAutoTime.Elapsed += new System.Timers.ElapsedEventHandler(autoTime_Elapsed);
                }
                if (spAutoTime != null)
                {
                    spAutoTime.Enabled = chkAutoSPQHCacle.Checked;
                }

                #region 组装自动下单实体
                AutoDoOrder order = new AutoDoOrder();
                order.BuySell = this.cmbSPQHBuysell.SelectedIndex;

                //order.OrderPriceType = this.cbMarketOrder.Checked ? 0 : 1;
                order.OrderUnitType = cmbSPQHUnit.Text.Trim();
                order.PortfoliosID = this.txtSPQHProtfolioLogo.Text;

                int amt = 1;
                if (!int.TryParse(txtSPQHAmount.Text, out amt))
                {
                    amt = 1;
                }
                order.OrderAmount = amt;
                order.IndexStart = cbSPCode.SelectedIndex;
                int end = 0;
                if (!int.TryParse(txtSPQHIndex.Text, out end))
                {
                    end = cbSPCode.Items.Count;
                }
                if (end > cbSPCode.Items.Count)
                {
                    end = cbSPCode.Items.Count;
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

                order.CodeCount = cbSPCode.Items.Count;

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
                order.FutureOpenCloseType = cmbSPQHOpenClose.Text;

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
        private void AutoSPQHOrder(string code, float amount, string fundAccountID, Types.TransactionDirection type, Types.UnitType unitType, string tradeID,
            TypesFutureOpenCloseType openCloseType, string portfoliosID)
        {
            try
            {
                #region 组装下单实体
                if (amount < 0)
                {
                    return;
                }
                MercantileFuturesOrderRequest order = new MercantileFuturesOrderRequest();
                order.Code = code;
                order.BuySell = type;
                order.FundAccountId = fundAccountID;
                order.OrderAmount = amount;

                #region 价格处理

                string errMsg = "";
                MarketDataLevel leave = wcfLogic.GetLastPricByCommodityCode(code, 2, out errMsg);
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

                order.OrderUnitType = unitType; //Utils.GetUnit(cmbSPQHUnit.Text.Trim());
                order.OrderWay = GTA.VTS.CustomersOrders.DoOrderService.TypesOrderPriceType.OPTLimited;
                // order.PortfoliosId = "p2";
                order.TraderId = tradeID; //ServerConfig.TraderID; //"23";
                order.TraderPassword = "";
                order.OpenCloseType = openCloseType;// Utils.GetFutureOpenCloseType(cmbSPQHOpenClose.Text.Trim());
                order.PortfoliosId = portfoliosID;// this.txtSPQHProtfolioLogo.Text;

                smartPool.QueueWorkItem(DoSPQHOrder, order);

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
            if (!int.TryParse(txtSPQHCancleMin.Text, out minute))
            {
                minute = 3;
            }
            spqhLogic.DisposeAutoCance(minute);
        }


        /// <summary>
        /// 异步自动下单操作方法
        /// <param name="index">开始索引列</param>
        /// </summary>
        private void AutoOrderInvoker(AutoDoOrder order)
        {
            List<QH_HoldAccountTableInfo> holdList = null;


            //先把之前的填写的下单量记录下来
            int orderVoume = order.OrderAmount;

            string fundAccountID = ServerConfig.SPQHCapitalAccount;

            Types.TransactionDirection type = order.BuySell == 0
                               ? Types.TransactionDirection.Buying
                               : Types.TransactionDirection.Selling;
            Types.UnitType unitType = Utils.GetUnit(order.OrderUnitType.Trim()); ;
            string tradeID = ServerConfig.TraderID; ;
            TypesFutureOpenCloseType openCloseType = Utils.GetFutureOpenCloseType(order.FutureOpenCloseType.Trim());
            string portfoliosID = order.PortfoliosID;

            do
            {
                #region 这里写死的时间判断，如果以后要修改再从管理中心获取添加服务
                DateTime nowTime = wcfLogic.CheckDoAccoumtChannel();
                //9	2010-01-25 09:00:00.000	2010-01-25 10:15:00.000	6
                //15	2010-01-25 10:30:00.000	2010-01-25 11:30:00.000	6
                //16	2010-01-25 13:30:00.000	2010-01-25 15:00:00.000	6
                //10	2010-01-25 09:00:00.000	2010-01-25 10:15:00.000	7
                //17	2010-01-25 10:30:00.000	2010-01-25 11:30:00.000	7
                //18	2010-01-25 13:30:00.000	2010-01-25 15:00:00.000	7
                //11	2010-01-25 09:00:00.000	2010-01-25 10:15:00.000	8
                //12	2010-01-25 10:30:00.000	2010-01-25 11:30:00.000	8
                //13	2010-01-25 13:30:00.000	2010-01-25 14:10:00.000	8
                //14	2010-01-25 14:20:00.000	2010-01-25 15:00:00.000	8
                //如果不在交易开市时间不作自动下单
                if (!(nowTime >= new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 9, 0, 0) && nowTime <= new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 11, 30, 0))
                    && !(nowTime >= new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 13, 30, 0) && nowTime <= new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 15, 0, 0)))
                {
                    WriteSPQHMsg("不在交易时间内不能操作自动下单功能!");
                    btnAutoOrder_Click(this, null);
                    break;
                }
                #endregion
                if (order.IsHoldAccount)
                {
                    holdList = wcfLogic.SPQHHold;
                    //当前下单量
                    decimal orderCount = orderVoume;
                    //选择记录之前选择的开平仓类型,这里的值要设置与cbGZOpenClose的下拉列表框的索引一致 0-开仓1，1--平仓（历史）2，2--平今3
                    openCloseType = TypesFutureOpenCloseType.ClosePosition;
                    ////买卖方向
                    //Types.TransactionDirection buySell = Types.TransactionDirection.Selling;

                    foreach (var hold in holdList)
                    {
                        if (!isAutoOrder)
                        {
                            break;
                        }
                        orderCount = orderVoume;

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
                        //cbSPCode.Text = hold.Contract;
                        //txtSPQHAmount.Text = orderCount.ToString();
                        //cmbSPQHOpenClose.SelectedIndex = openCloseType;
                        //cmbSPQHBuysell.SelectedIndex = buySell;
                        //强制点击
                        //btnSPQHOrder_Click(this, null);
                        AutoSPQHOrder(hold.Contract, (float)orderCount, fundAccountID, type, unitType, tradeID, openCloseType, portfoliosID);

                        //if (orderCount != orderVoume)
                        //{
                        //txtSPQHAmount.Text = orderVoume.ToString();

                        //}
                        //}));
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
                        //    this.cbSPCode.SelectedIndex = k;
                        //强制点击
                        //btnSPQHOrder_Click(this, null);
                        AutoSPQHOrder(AllCodeList[k], (float)orderVoume, fundAccountID, type, unitType, tradeID, openCloseType, portfoliosID);
                        //}));
                    }
                }
                int ts = order.DoOrderTimeSapn;
                //if (!int.TryParse(this.txtTimeSapn.Text, out ts))
                //{
                //    ts = 5;
                //}
                Thread.CurrentThread.Join(ts * 1000);
            } while (isAutoOrder);
        }

        /// <summary>
        /// 自动下单对持仓操作选择框方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbHoldAccount_CheckedChanged(object sender, EventArgs e)
        {
            cmbSPQHOpenClose.Enabled = !chbHoldAccount.Checked;
            cmbSPQHBuysell.Enabled = !chbHoldAccount.Checked;

        }
        #endregion

    }
}
