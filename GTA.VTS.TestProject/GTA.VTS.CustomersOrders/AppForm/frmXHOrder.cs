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
    /// <summary>
    /// Desc: 现货下单窗体
    /// Create by: 董鹏
    /// Create Date: 2010-03-01
    /// </summary>
    public partial class frmXHOrder : MdiFormBase, IOrderCallBackView<StockDealOrderPushBack>
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
        /// 读取Excel表格数据
        /// </summary>
        private OrderSQLHelper orderSql = new OrderSQLHelper();
        /// <summary>
        /// 现货消息逻辑访问对象
        /// </summary>
        private XHMessageLogic xhLogic;

        /// <summary>
        /// 线程池
        /// </summary>
        private SmartThreadPool smartPool = new SmartThreadPool { MaxThreads = 200, MinThreads = 25 };

        private int xhDoOrderNum;
        private string title = "";

        private bool isProcessing;
        private bool isClearXH;
        private bool isReBindXHData;
        private int xhIndex;

        private System.Timers.Timer xhTimer = new System.Timers.Timer();
        private System.Timers.Timer timer = new System.Timers.Timer();
        /// <summary>
        /// 现货自动下单后自动撤单触发事件
        /// </summary>
        private System.Timers.Timer xhAutoTime = null;

        /// <summary>
        /// 所有代码列表
        /// </summary>
        private List<string> AllCodeList = new List<string>();

        #endregion

        #region 构造函数

        public frmXHOrder()
        {
            InitializeComponent();
            OrderCallBack.XHView = this;
            xhLogic = new XHMessageLogic();
            //自动撤单委托 事件
            xhLogic.AutoEvent += new AutoCancelOrder(this.CancelXHOrder);
            wcfLogic = WCFServices.Instance;
            LocalhostResourcesFormText();
        }

        #endregion

        //private string GetXHDealsDesc(List<StockPushDealEntity> deals)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    string line = "\n";
        //    foreach (var deal in deals)
        //    {
        //        sb.Append(line);
        //        sb.Append("     ");
        //    }
        //    return sb.ToString();
        //}

        #region 窗体事件

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmXHOrder_Load(object sender, EventArgs e)
        {

            #region  自动补全
            this.cbXHCode.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.cbXHCode.AutoCompleteSource = AutoCompleteSource.ListItems;
            AllCodeList = wcfLogic.GetAllCode(1);
            //如果没有数据还是用之前添加在Item里的数据
            if (AllCodeList != null && AllCodeList.Count > 0)
            {
                this.cbXHCode.DataSource = AllCodeList;
            }
            else
            {
                //为了后面自动下单作准备使用
                AllCodeList = new List<string>();
                foreach (var item in cbXHCode.Items)
                {
                    AllCodeList.Add(item.ToString());
                }
            }

            #endregion
            smartPool.Start();

            //定时刷新账户信息
            timer.Interval = 1000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
            this.txtProtfolioLogo.Text = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            // txtTradeID.Text = ServerConfig.TraderID;
            // txtCapital.Text = ServerConfig.XHCapitalAccount;

            comboBuySell.SelectedIndex = 0;
            cbXHUnit.SelectedIndex = 0;
            cbXHCode.SelectedIndex = 0;

            QueryXHCapital();
        }
        /// <summary>
        /// 刷新显示相关语方类型的标签
        /// </summary>
        private void LocalhostResourcesFormText()
        {
            this.Text = ResourceOperate.Instanse.GetResourceByKey("menuXHOrder");
            #region 自动下单多语言
            this.grbAutomaticorders.Text = ResourceOperate.Instanse.GetResourceByKey("Automaticorders");
            this.lbxhEndIndex.Text = ResourceOperate.Instanse.GetResourceByKey("EndIndx");
            this.lbTimeSpan.Text = ResourceOperate.Instanse.GetResourceByKey("TimeSpan");
            this.chbHoldAccount.Text = ResourceOperate.Instanse.GetResourceByKey("HoldAccounts");
            this.btnAutoOrder.Text = ResourceOperate.Instanse.GetResourceByKey("AutoOrder");
            this.toolTip1.SetToolTip(this.txtXHIndex, ResourceOperate.Instanse.GetResourceByKey("Index"));
            this.toolTip1.SetToolTip(this.txtTimeSapn, ResourceOperate.Instanse.GetResourceByKey("TimeSapn"));
            this.chkAutoXHCacle.Text = ResourceOperate.Instanse.GetResourceByKey("Automatic");
            #endregion
            #region 现货语言类型显示
            this.lblCode.Text = ResourceOperate.Instanse.GetResourceByKey("lblCode");
            this.lblAmount.Text = ResourceOperate.Instanse.GetResourceByKey("Amount");
            //  this.lblTradeID.Text = ResourceOperate.Instanse.GetResourceByKey("lblTradeID");
            // this.lblCapital.Text = ResourceOperate.Instanse.GetResourceByKey("lblCapital");
            this.lblPrice.Text = ResourceOperate.Instanse.GetResourceByKey("Price");
            //     this.lblBatch.Text = ResourceOperate.Instanse.GetResourceByKey("lblBatch");
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

            this.tabPageGrid.Text = ResourceOperate.Instanse.GetResourceByKey("tabPageGrid");
            this.tabPageList.Text = ResourceOperate.Instanse.GetResourceByKey("tabPageList");
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
            #region 现货下单数据绑定显示
            for (int i = 0; i < this.dagXH.ColumnCount; i++)
            {
                string XHName = dagXH.Columns[i].HeaderText;
                dagXH.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(XHName);
            }
            #endregion 现货下单数据绑定显示
        }

        /// <summary>
        /// 下单按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDoOrder_Click(object sender, EventArgs e)
        {
            try
            {

                #region 组装下单实体

                errPro.Clear();
                xhDoOrderNum = 0;

                #region 组装现货下单请求实体

                StockOrderRequest order = new StockOrderRequest();
                order.Code = cbXHCode.Text.Trim();
                order.BuySell = comboBuySell.SelectedIndex == 0
                                    ? Types.TransactionDirection.Buying
                                    : Types.TransactionDirection.Selling;
                order.FundAccountId = ServerConfig.XHCapitalAccount;
                order.OrderAmount = float.Parse(txtAmount.Text.Trim());

                if (!cbMarketOrder.Checked)
                {
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
                        }
                        else
                        {
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
                //}
                order.OrderUnitType = Utils.GetUnit(cbXHUnit.Text.Trim());
                order.OrderWay = cbMarketOrder.Checked
                                     ? GTA.VTS.CustomersOrders.DoOrderService.TypesOrderPriceType.OPTMarketPrice
                                     : GTA.VTS.CustomersOrders.DoOrderService.TypesOrderPriceType.OPTLimited;
                //order.PortfoliosId = "p1";
                //判断TraderID是否正确
                order.TraderId = ServerConfig.TraderID;
                order.TraderPassword = "";
                order.PortfoliosId = this.txtProtfolioLogo.Text;
                #endregion

                //int batch = 1;

                //for (int i = 0; i < batch; i++)
                //{
                //异步执行下单
                smartPool.QueueWorkItem(DoXHOrder, order);
                //}
                //if (batch >= 50)
                //{
                //    int scale = batch / 50;
                //    btnDoOrder.Enabled = false;
                //    xhTimer.Interval = 1000 * scale;
                //    xhTimer.Elapsed += delegate
                //                           {
                //                               this.Invoke(new MethodInvoker(() => { btnDoOrder.Enabled = true; }));
                //                               xhTimer.Enabled = false;
                //                           };
                //    xhTimer.Enabled = true;
                //}
                //this.txtProtfolioLogo.Text = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                #endregion 组装下单实
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 价格输入框双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPrice_DoubleClick(object sender, MouseEventArgs e)
        {
            Price();
        }
        /// <summary>
        /// 根据股票代码和股票的上下限和行情信息
        /// </summary>
        private decimal Price()
        {
            decimal price = 0;
            try
            {
                errPro.Clear();
                string errMsg = "";

                MarketDataLevel leave = wcfLogic.GetLastPricByCommodityCode(cbXHCode.Text.Trim(), 1, out errMsg);
                if (leave != null)
                {
                    price = leave.LastPrice;

                    //自动下单不作处理
                    //if (!isAutoOrder)
                    //{
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
                    //}
                }
                ////自动下单不作处理
                //if (!isAutoOrder)
                //{
                if (!string.IsNullOrEmpty(errMsg))
                {
                    errPro.SetError(txtPrice, errMsg);
                } 

                this.txtPrice.Text = Utils.Round(price).ToString();
                //获取价格上下限 
                SetXHHighLowValue();
                //}
                

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
            return price;
        }

        /// <summary>
        /// 价格上下限输入框双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtXHHigh_DoubleClick_1(object sender, EventArgs e)
        {
            SetXHHighLowValue();
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
        /// 列表颜色设置 (不再调用，直接设置属性即可)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dagXH_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            SetGridViewColor(this.dagXH);
        }

        /// <summary>
        /// 列表行单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dagXH_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dagXH.SelectedRows)
            {
                xhIndex = row.Index;

                int index = e.ColumnIndex;
                if (index != 0)
                    return;

                XHMessage message = row.DataBoundItem as XHMessage;
                if (message == null)
                {
                    continue;
                }
                string errMsg = "";
                if (!CancelXHOrder(message.EntrustNumber, ref errMsg))
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
        public bool CancelXHOrder(string entrustNumber, ref string errMsg)
        {
            errMsg = "";
            //现货委托撤单
            bool isSuccess = wcfLogic.CancelStockOrder(entrustNumber, ref errMsg);
            if (!isSuccess)
            {
                string msg = "现货委托[" + entrustNumber + "]撤单失败！" + Environment.NewLine + errMsg;
                xhLogic.UpdateMessage(entrustNumber, errMsg);
            }
            return isSuccess;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dagXH_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SetGridNumber(this.dagXH, e);
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
        /// 获取现货的最大委托量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtXhMax_DoubleClick(object sender, EventArgs e)
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
                txtXhMax.Text = "";
                var type = cbMarketOrder.Checked
                               ? DoAccountManager.TypesOrderPriceType.OPTMarketPrice
                               : DoAccountManager.TypesOrderPriceType.OPTLimited;
                //var traderId = txtTradeID.Text.Trim();
                var traderId = ServerConfig.TraderID;
                var code = cbXHCode.Text.Trim();

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

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmXHOrder_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer.Close();
            xhTimer.Close();
            isAutoOrder = false;
            //关闭自动撤单事件
            if (xhAutoTime != null)
            {
                xhAutoTime.Enabled = false;
            }
        }

        /// <summary>
        /// 可用资金获取焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAvailableCapital_MouseDown(object sender, MouseEventArgs e)
        {
            QueryXHCapital();
        }

        /// <summary>
        /// 使用回车键获取股票价格和上下限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                Price();
            }
        }

        /// <summary>
        /// 在最大委托量使用回车按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtXhMax_KeyPress(object sender, KeyPressEventArgs e)
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
        private void dagXH_DoubleClick(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dagXH.SelectedRows)
            {
                XHMessage message = row.DataBoundItem as XHMessage;
                if (message == null)
                {
                    continue;
                }
                XHMessageLogic.FireEntrustSelectedEvent(message.EntrustNumber);
            }
        }

        ///// <summary>
        ///// 标签页标题颜色绘画事件
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void tabPushBack_DrawItem(object sender, DrawItemEventArgs e)
        //{
        //    Graphics g = e.Graphics;
        //    g.FillRectangle(new SolidBrush(SystemColors.GradientActiveCaption), 0, 0, this.tabXHPushBack.ItemSize.Width, this.tabXHPushBack.ItemSize.Height);
        //    g.FillRectangle(new SolidBrush(SystemColors.GradientActiveCaption), this.tabXHPushBack.ItemSize.Width, 0, this.tabXHPushBack.ItemSize.Width * 2, this.tabXHPushBack.ItemSize.Height);

        //    g.DrawString(ResourceOperate.Instanse.GetResourceByKey("tabPageGrid"), this.Font, new SolidBrush(Color.Black), 5, 5);
        //    g.DrawString(ResourceOperate.Instanse.GetResourceByKey("tabPageList"), this.Font, new SolidBrush(Color.Black), 5 + this.tabXHPushBack.ItemSize.Width, 5);
        //}
        #endregion

        #region 私有方法

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
            var highLowRange = wcfLogic.GetHighLowRangeValueByCommodityCode(cbXHCode.Text.Trim(), price, out errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                errPro.SetError(txtPrice, errMsg);
            }
            if (highLowRange == null)
            {
                //MessageBox.Show("Can not get highlowrange object!");
                string errMessage = ResourceOperate.Instanse.GetResourceByKey("highlowrange");
                errPro.Clear();
                errPro.SetError(txtXHHigh, errMessage);
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
        /// 现货下单
        /// </summary>
        /// <param name="order">现货委托申请</param>
        private void DoXHOrder(StockOrderRequest order)
        {
            var res = wcfLogic.DoStockOrder(order);


            if (isAutoOrder)
            {
                if (res.IsSuccess)
                {
                    xhLogic.AddAutoCanceOrder(res.OrderId, DateTime.Now);
                }
            }


            xhLogic.ProcessDoOrder(res, order);

            xhDoOrderNum++;

            //WriteTitle(xhDoOrderNum);

            string format =
                "DoOrder[EntrustNumber={0},Code={1},EntrustAmount={2},EntrustPrice={3},BuySell={4},TraderId={5},OrderMessage={6},IsSuccess={7}]  Time={8}";
            string desc = string.Format(format, res.OrderId, order.Code, order.OrderAmount, order.OrderPrice,
                                        order.BuySell, order.TraderId, res.OrderMessage, res.IsSuccess,
                                        DateTime.Now.ToLongTimeString());
            WriteXHMsg(desc);

            //LogHelper.WriteDebug(desc);
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

            ReBindXHData();

            isProcessing = false;
        }
        #endregion 定时刷新资金账户信息
        /// <summary>
        /// 现货资金账户信息查询
        /// </summary>
        private void QueryXHCapital()
        {
            try
            {
                //  string capitalAccount = txtCapital.Text.Trim();
                string capitalAccount = ServerConfig.XHCapitalAccount;
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

            QueryXHCapital();

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
        /// 现货信息
        /// </summary>
        /// <param name="msg"></param>
        private void WriteXHMsg(string msg)
        {
            MessageDisplayHelper.Event(msg, listBox1);
        }

        #endregion

        #region IOrderCallBackView<StockDealOrderPushBack> 成员

        /// <summary>
        /// 处理成交回报
        /// </summary>
        /// <param name="drsip"></param>
        public void ProcessPushBack(StockDealOrderPushBack drsip)
        {
            var tet = drsip.StockOrderEntity;
            var deals = drsip.StockDealList;

            string format =
                "<--PushBack[EntrustNumber={0},Code={1},TradeAmount={2},CancelAmount={3},OrderStatusId={4},OrderMessage={5},DealsCount={6}]  Time={7}";
            string desc = string.Format(format, tet.EntrustNumber, tet.SpotCode, tet.TradeAmount, tet.CancelAmount,
                                        Utils.GetOrderStateMsg(tet.OrderStatusId), tet.OrderMessage,
                                        deals.Count, DateTime.Now.ToLongTimeString());
            // LogHelper.WriteDebug(desc);

            //   string dealsDesc = GetXHDealsDesc(deals);

            WriteXHMsg(desc);

            if (isAutoOrder)
            {
                if (tet.OrderStatusId > 5 && tet.OrderStatusId != 9)
                {
                    xhLogic.DelateAutoCanceOrder(tet.EntrustNumber);
                }
            }


            xhLogic.ProcessPushBack(drsip);
        }

        #endregion

        #region 批量下单
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
        /// <summary>
        /// 批量下单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBatchSendOrder_Click(object sender, EventArgs e)
        {
            try
            {
                #region 获取Excel表格中数据

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

                            string Code, BuySell, UnitType, price, PortfoliosId;
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
                                price = myDataSet.Tables[0].Rows[i][6].ToString();
                                PortfoliosId = myDataSet.Tables[0].Rows[i][7].ToString();
                                if (string.IsNullOrEmpty(PortfoliosId))
                                {
                                    PortfoliosId = "";
                                }
                                Order(Code, OrderAmount, batch, BuySell, UnitType, cbMarketOrder, price, PortfoliosId);
                            }

                            #endregion 获取Excel表格中数据
                        }
                        catch (Exception ex)
                        {
                            string MesageError = ResourceOperate.Instanse.GetResourceByKey("MesageError");
                            errPro.Clear();
                            errPro.SetError(btnPath, MesageError);
                            LogHelper.WriteError(ex.Message, ex);
                        }
                    }
                    else
                    {
                        string MesageData = ResourceOperate.Instanse.GetResourceByKey("MesageData");
                        errPro.Clear();
                        errPro.SetError(btnPath, MesageData);
                    }
                }
                else
                {
                    errPro.Clear();
                    string Mesage = ResourceOperate.Instanse.GetResourceByKey("Mesage");
                    errPro.SetError(btnPath, Mesage);
                }

                #endregion 获取Excel表格中数据
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
        private void Order(string Code, float OrderAmount, int batchs, string BuySell, string UnitType, bool cbMarketOrder, string price, string PortfoliosId)
        {
            try
            {
                #region 获取Excel数据并组装下单实体

                errPro.Clear();
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
                    errPro.Clear();
                    errPro.SetError(btnPath, OrdreName + Code + "Code" + CodeError);
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
                    errPro.Clear();
                    errPro.SetError(btnPath, OrdreName + Code + BS + Exception);

                }
                //对用户输入的Capital进行判断，判断是否为登陆时输入的现货资金帐户.
                order.FundAccountId = ServerConfig.XHCapitalAccount;
                order.OrderAmount = OrderAmount;

                #region 判断是否为市价委托，来判断价格

                if (cbMarketOrder == false)
                {
                    if (price != null && !price.Equals("0"))
                    {
                        float pric;
                        if (float.TryParse(price, out pric))
                        {
                            order.OrderPrice = pric;
                        }
                        #region 获取价格上下限

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
                        //var highLowRange = wcfLogic.GetHighLowRangeValueByCommodityCode(Code, prices, out errMsg);
                        //if (!string.IsNullOrEmpty(errMsg))
                        //{
                        //    errPro.SetError(txtPrice, errMsg);
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

                        #endregion 获取价格上下限

                        #region 价格判断

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

                        #endregion 价格判断
                    }
                    else
                    {
                        errPro.Clear();
                        errPro.SetError(btnPath, OrdreName + Code + OrderPrice + Exception);
                    }
                }

                #endregion 判断是否为市价委托，来判断价格

                order.OrderUnitType = Utils.GetUnit(UnitType);
                if (cbMarketOrder == true)
                {
                    order.OrderWay = GTA.VTS.CustomersOrders.DoOrderService.TypesOrderPriceType.OPTMarketPrice;
                }
                else if (cbMarketOrder == false)
                {
                    order.OrderWay = GTA.VTS.CustomersOrders.DoOrderService.TypesOrderPriceType.OPTLimited;
                }
                else
                {
                    errPro.Clear();
                    errPro.SetError(btnPath, OrdreName + Code + IsMarketValue + Exception);
                }
                order.PortfoliosId = PortfoliosId;
                //判断TraderID是否正确
                order.TraderId = ServerConfig.TraderID;
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
        #endregion 批量下单

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
                txtXHIndex.Enabled = true;
                //可用
                //cbXHCode.Enabled = true;
                isAutoOrder = false;
                //这里要实现多语言
                btnAutoOrder.Text = ResourceOperate.Instanse.GetResourceByKey("AutoOrder");
                //关闭自动撤单事件
                if (xhAutoTime != null)
                {
                    xhAutoTime.Enabled = false;
                }
            }
            else
            {
                //不可用
                //cbXHCode.Enabled = false;
                txtXHIndex.Enabled = false;
                isAutoOrder = true;
                //这里要实现多语言
                btnAutoOrder.Text = ResourceOperate.Instanse.GetResourceByKey("AutoOrders");

                //自动撤单事件
                if (chkAutoXHCacle.Checked && xhAutoTime == null)
                {
                    xhAutoTime = new System.Timers.Timer();
                    xhAutoTime.Interval = 60 * 1000;
                    xhAutoTime.Elapsed += new System.Timers.ElapsedEventHandler(autoTime_Elapsed);
                }

                if (xhAutoTime != null)
                {
                    xhAutoTime.Enabled = chkAutoXHCacle.Checked;

                }

                #region 组装自动下单实体
                AutoDoOrder order = new AutoDoOrder();
                order.BuySell = this.comboBuySell.SelectedIndex;

                order.OrderPriceType = this.cbMarketOrder.Checked ? 0 : 1;
                order.OrderUnitType = cbXHUnit.Text.Trim();
                order.PortfoliosID = this.txtProtfolioLogo.Text;

                int amt = 0;
                int.TryParse(txtAmount.Text, out amt);
                order.OrderAmount = amt;
                order.IndexStart = cbXHCode.SelectedIndex;
                int end = 0;
                if (!int.TryParse(txtXHIndex.Text, out end))
                {
                    end = cbXHCode.Items.Count;
                }
                if (end > cbXHCode.Items.Count)
                {
                    end = cbXHCode.Items.Count;
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

                order.CodeCount = cbXHCode.Items.Count;
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
        /// <param name="orderPriceType">是否为现价还限价</param>
        private void AutoXHOrder(string code, float amount, string fundAccountID, Types.TransactionDirection type, Types.UnitType unitType, string tradeID,
            string portfoliosID, DoOrderService.TypesOrderPriceType orderPriceType)
        {
            try
            {
                #region 组装下单实体
                if (amount < 0)
                {
                    return;
                }
                StockOrderRequest order = new StockOrderRequest();
                order.Code = code;
                order.BuySell = type;
                order.FundAccountId = fundAccountID;
                order.OrderAmount = amount;

                #region 价格处理
                if (orderPriceType != DoOrderService.TypesOrderPriceType.OPTMarketPrice)
                {
                    string errMsg = "";
                    MarketDataLevel leave = wcfLogic.GetLastPricByCommodityCode(code, 1, out errMsg);
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

                order.OrderWay = orderPriceType;
                order.OrderUnitType = unitType;
                order.TraderId = tradeID;
                order.TraderPassword = "";
                order.PortfoliosId = portfoliosID;

                smartPool.QueueWorkItem(DoXHOrder, order);

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
            if (!int.TryParse(txtXHCancleMin.Text, out minute))
            {
                minute = 3;
            }
            xhLogic.DisposeAutoCance(minute);
        }



        /// <summary>
        /// 异步自动下单操作方法
        /// <param name="order"></param>
        /// </summary>
        private void AutoOrderInvoker(AutoDoOrder order)
        {
            List<XH_AccountHoldTableInfo> holdList = null;
            decimal amt;

            string fundAccountID = ServerConfig.XHCapitalAccount;
            Types.TransactionDirection type = order.BuySell == 0
                               ? Types.TransactionDirection.Buying
                               : Types.TransactionDirection.Selling;
            DoOrderService.TypesOrderPriceType orderWay = order.OrderPriceType == 0
                                                          ? DoOrderService.TypesOrderPriceType.OPTMarketPrice
                                                          : DoOrderService.TypesOrderPriceType.OPTLimited;
            Types.UnitType unitType = Utils.GetUnit(order.OrderUnitType);
            string tradeID = ServerConfig.TraderID; ;
            string portfoliosID = order.PortfoliosID;
            //if (!decimal.TryParse(txtAmount.Text, out amt))
            //{
            //    amt = 0;
            //}
            amt = order.OrderAmount;
            if (amt <= 0)
            {
                errPro.SetError(btnAutoOrder, "委托量不能为0");
                return;
            }
            do
            {
                #region 这里写死的时间判断，如果以后要修改再从管理中心获取添加服务
                DateTime nowTime = wcfLogic.CheckDoAccoumtChannel();
                //2010-02-23 09:30:00.000	2010-02-23 11:30:00.000	1
                //2010-02-23 13:00:00.000	2010-02-23 15:00:00.000	1
                //2009-12-25 09:30:00.000	2009-12-25 11:30:00.000	2
                //2009-12-25 13:00:00.000	2009-12-25 15:00:00.000	2
                //2009-12-25 09:15:00.000	2009-12-25 11:30:00.000	3
                //2009-12-25 13:00:00.000	2009-12-25 15:15:00.000	3
                //如果不在交易开市时间不作自动下单
                if (!(nowTime >= new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 9, 30, 0) && nowTime <= new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 11, 30, 0))
                    && !(nowTime >= new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 13, 0, 0) && nowTime <= new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 15, 15, 0)))
                {
                    WriteXHMsg("不在交易时间内不能操作自动下单功能!");
                    btnAutoOrder_Click(this, null);
                    break;
                }
                #endregion
                if (order.IsHoldAccount)
                {
                    holdList = wcfLogic.XHHold;
                    //当前下单量
                    decimal orderCount = amt;
                    foreach (var hold in holdList)
                    {
                        if (!isAutoOrder)
                        {
                            break;
                        }
                        //if (!decimal.TryParse(txtAmount.Text, out amt))
                        //{
                        //amt = 0;
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
                        //    cbXHCode.Text = hold.Code;
                        //    comboBuySell.SelectedIndex = 1;
                        //    //强制点击
                        //    btnDoOrder_Click(this, null);
                        //}));
                        AutoXHOrder(hold.Code, (float)orderCount, fundAccountID, type, unitType, tradeID, portfoliosID, orderWay);

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
                        //    this.cbXHCode.SelectedIndex = k;
                        //    //强制点击
                        //    btnDoOrder_Click(this, null);
                        //}));
                        AutoXHOrder(AllCodeList[k], (float)amt, fundAccountID, type, unitType, tradeID, portfoliosID, orderWay);

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
        /// 获取当前列表中当前选择的索引
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtXHIndex_DoubleClick(object sender, EventArgs e)
        {
            txtXHIndex.Text = cbXHCode.SelectedIndex.ToString();

        }

        /// <summary>
        /// 自动下单对持仓操作选择框方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbHoldAccount_CheckedChanged(object sender, EventArgs e)
        {
            //主要选择了当前对持仓进行自动下单操作，那么现货只有卖，不象期货那样持仓会有卖空或者买空的存在，所以现货持仓操作只有卖
            if (chbHoldAccount.Checked)
            {
                comboBuySell.SelectedIndex = 1;
                comboBuySell.Enabled = false;//不可再用
            }
            else
            {
                comboBuySell.Enabled = true;
            }

        }


        #endregion

    }
}
