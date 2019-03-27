#region Using Namespace

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using Amib.Threading;
using GTA.VTS.Common.CommonObject;
using CommonRealtimeMarket;
using CommonRealtimeMarket.factory;
using GTA.VTS.Common.CommonUtility;
using GTAMarketSocket;
//using RealtimeMarket.factory;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.PushBackTest.DoDealRptService;
using ReckoningCounter.PushBackTest.DoOrderService;
using ReckoningCounter.PushBackTest.HKTraderFindService;
using ReckoningCounter.PushBackTest.TraderFindService;
using Timer = System.Timers.Timer;
using ReckoningCounter.PushBackTest.TraderQueryService;
using System.Text.RegularExpressions;
using RealTime.Common.CommonClass;
//using RealTime.Server.Handler;

#endregion

namespace ReckoningCounter.PushBackTest
{
    public partial class Form1 : Form
    {
        private string gzqhAccount = "";
        private int gzqhDoOrderNum;
        private int gzqhIndex;
        private GZQHMessageLogic gzqhLogic = new GZQHMessageLogic();
        private Timer gzqhTimer = new Timer();
        private string hkAccount = "";
        private int hkDoOrderNum;
        private int hkIndex;
        private HKMessageLogic hkLogic = new HKMessageLogic();
        private Timer hkTimer = new Timer();
        private bool isClearGZQH;
        private bool isClearHK;
        private bool isClearXH;
        private bool isProcessing;
        private bool isReBindGZQHData;
        private bool isReBindHKData;
        private bool isReBindXHData;
        private bool loadCodeSuccess;
        private SmartThreadPool smartPool = new SmartThreadPool { MaxThreads = 200, MinThreads = 25 };
        private string spqhAccount = "";
        private Timer timer = new Timer();
        private string title = "";
        private string traderId = "";
        internal WCFLogic wcfLogic;
        private string xhAccount = "";

        private int xhDoOrderNum;
        private int xhIndex;
        private XHMessageLogic xhLogic = new XHMessageLogic();

        private Timer xhTimer = new Timer();

        public Form1()
        {
            InitializeComponent();
        }

        public bool Initialize(ref string errMsg)
        {
            //1.启动行情服务
            try
            {
                //因点用CPU目前去掉使用柜台提供方法获取最新成交价
                // this.InitRealtimeMarketComponent();
            }
            catch (Exception ex)
            {
                string msg = "行情组件初始化失败！请检查配置文件。";
                string strErrorMsg = msg + Environment.NewLine + "错误信息如下：" + Environment.NewLine + ex.Message;
                //MessageBox.Show(strErrorMsg, "启动失败", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Environment.Exit(Environment.ExitCode);
                LogHelper.WriteDebug(strErrorMsg);
                errMsg = strErrorMsg;
                return false;
            }

            //2.连接WCF服务
            try
            {
                wcfLogic = new WCFLogic();

                bool isSuccess = wcfLogic.Initialize(txtChannelID.Text.Trim());
                if (!isSuccess)
                {
                    errMsg = "WCF initialize failure!";
                    LogHelper.WriteDebug(errMsg);
                    //MessageBox.Show("Register push back failure!");
                    return false;
                }

                string channleid = ServerConfig.ChannelID;
                string title = " [ChannelID: " + channleid + "]";
                StatusLable.Text = "Version:" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + "    " + StatusLable.Text;
                this.Text += title;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Error");
                errMsg = ex.Message;
                LogHelper.WriteError(ex.Message, ex);
                return false;
            }

            return true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lbCodes.Left = 72;
            lbCodes.Top = 40;

            this.loadCodeSuccess = CodeManager.Load();
            if (loadCodeSuccess)
                CodeManager.FillList(lbCodes);
            lbCodes.Visible = false;

            comboBuySell.SelectedIndex = 0;
            cbXHUnit.SelectedIndex = 0;
            txtCode.Focus();

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

            Start();

            title = this.Text;

            if (loadCodeSuccess)
                this.txtCode.KeyUp += this.txtCode_KeyUp;

            InitPageControls();
        }

        /// <summary>
        /// 初始化翻页控件
        /// </summary>
        private void InitPageControls()
        {
            int pageSize = 100;

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
        /// 验证是否是合法的小数点格式(可包含数字及小数点)
        /// </summary>
        /// <param name="keyWords">需要验证的字符串</param>
        /// <returns></returns>
        public static bool DecimalTest(string keyWords)
        {
            return Regex.IsMatch(keyWords, @"^[0-9.]+$");
        }
        public void Start()
        {
            smartPool.Start();

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
            txtCapital.Text = xhAccount;
            txtHKCapital.Text = hkAccount;
            txtGZCapital.Text = gzqhAccount;
        }

        public void StartTimer()
        {
            timer.Enabled = true;
        }

        public void StopTimer()
        {
            timer.Enabled = false;
        }

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

            isProcessing = false;
        }

        private string GetCode(ListBox listBox)
        {
            if (listBox.SelectedIndex < 0)
                return "";

            string value = listBox.SelectedItem.ToString();
            int i = value.IndexOf(' ');
            string code = value.Substring(0, i).Trim();

            return code;
        }


        private string GetValidKeyCode(string key)
        {
            string code = key;
            code = code.Substring(code.Length - 1, 1);

            return code.ToUpper();
        }

        private void ReBindGZQHData()
        {
            if (!gzqhLogic.HasChanged)
                return;

            if (isReBindGZQHData)
                return;

            this.Invoke(new MethodInvoker(() =>
                                              {
                                                  isReBindGZQHData = true;
                                                  this.SuspendLayout();

                                                  SortableBindingList<QHMessage> list =
                                                      new SortableBindingList<QHMessage>(gzqhLogic.MessageList);
                                                  this.gZQHMessageLogicBindingSource.DataSource = list;
                                                  // gzqhLogic.MessageList;
                                                  gZQHMessageLogicBindingSource.ResetBindings(false);
                                                  gZQHMessageLogicBindingSource.Position = gzqhIndex;
                                                  this.ResumeLayout(false);
                                                  isReBindGZQHData = false;

                                                  gzqhLogic.HasChanged = false;
                                              }));
        }

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
                //LogHelper.WriteError(ex.Message, ex);
            }
        }

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

                                                  if (xhLogic.MessageList.Count > 0)
                                                  {
                                                      SortableBindingList<XHMessage> list =
                                                          new SortableBindingList<XHMessage>(xhLogic.MessageList);
                                                      this.xHMessageLogicBindingSource.DataSource = list;
                                                      // xhLogic.MessageList;
                                                      xHMessageLogicBindingSource.ResetBindings(false);
                                                      xHMessageLogicBindingSource.Position = xhIndex;
                                                  }

                                                  this.ResumeLayout(false);

                                                  xhLogic.HasChanged = false;

                                                  isReBindXHData = false;
                                              }));
        }

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
                                                  isReBindHKData = true;

                                                  this.SuspendLayout();

                                                  if (hkLogic.MessageList.Count > 0)
                                                  {
                                                      SortableBindingList<HKMessage> list =
                                                          new SortableBindingList<HKMessage>(hkLogic.MessageList);
                                                      this.hKMessageLogicBindingSource.DataSource = list;
                                                      // xhLogic.MessageList;
                                                      hKMessageLogicBindingSource.ResetBindings(false);
                                                      hKMessageLogicBindingSource.Position = hkIndex;
                                                  }

                                                  this.ResumeLayout(false);

                                                  hkLogic.HasChanged = false;

                                                  isReBindHKData = false;
                                              }));
        }

        private void btnDoOrder_Click(object sender, EventArgs e)
        {
            errPro.Clear();

            xhDoOrderNum = 0;
            StockOrderRequest order = new StockOrderRequest();
            //判断Code是否为空，如果为空则弹出错误提示框并退出
            if (!string.IsNullOrEmpty(this.txtCode.Text))
            {
                order.Code = txtCode.Text.Trim();
            }
            else
            {
                MessageBox.Show("Code不能为空！", "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            order.BuySell = comboBuySell.SelectedIndex == 0
                                ? Types.TransactionDirection.Buying
                                : Types.TransactionDirection.Selling;
            order.FundAccountId = txtCapital.Text.Trim(); //"010000002302";
            order.OrderAmount = float.Parse(txtAmount.Text.Trim());


            if (!cbMarketOrder.Checked)
            {
                //判断Price是否为零并且不为空，如果为零则弹出错误提示框并退出
                float price = 0;
                //if (DecimalTest(this.txtPrice.Text))
                //{
                if (!string.IsNullOrEmpty(txtPrice.Text.Trim()))
                {
                    if (float.TryParse(this.txtPrice.Text, out price))
                    {
                        if (price == 0)
                        {
                            MessageBox.Show("Price不能为零！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Price数据非法！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    order.OrderPrice = float.Parse(txtPrice.Text.Trim());
                    //}
                }
                else
                {
                    MessageBox.Show("Price不能为空！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            order.OrderUnitType = Utils.GetUnit(cbXHUnit.Text.Trim());
            order.OrderWay = cbMarketOrder.Checked ? TypesOrderPriceType.OPTMarketPrice : TypesOrderPriceType.OPTLimited;
            order.PortfoliosId = "p1";
            order.TraderId = txtTradeID.Text.Trim(); //"23";
            order.TraderPassword = "";

            int batch = int.Parse(txtBatch.Text.Trim());

            for (int i = 0; i < batch; i++)
            {
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

        //private Dictionary<string, HKOrderRequest> hkOrderDict = new Dictionary<string, HKOrderRequest>();
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

            //if (res.IsSuccess)
            //{
            //    hkOrderDict[res.OrderId] = order;
            //}
        }

        private void WriteTitle(int i)
        {
            string msg = "[" + i + "]";
            this.Invoke(new MethodInvoker(() => { this.Text = this.title + msg; }));
        }

        public void WriteXHMsg(string msg)
        {
            MessageDisplayHelper.Event(msg, listBox1);
        }

        public void WriteHKMsg(string msg)
        {
            if (string.IsNullOrEmpty(msg))
                return;
            MessageDisplayHelper.Event(msg, listBox5);
        }


        public void WriteGZQHMsg(string msg)
        {
            MessageDisplayHelper.Event(msg, listBox2);
        }

        public void WriteWCFMsg(string msg)
        {
            MessageDisplayHelper.Event(msg, listBox3);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Enabled = false;
            wcfLogic.ShutDown();
            smartPool.Shutdown();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void QueryXHCapital()
        {
            try
            {
                string capitalAccount = txtCapital.Text.Trim();
                string msg = "";

                var cap = wcfLogic.QueryXHCapital(capitalAccount, ref msg);

                if (cap == null)
                    return;

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
            }
        }

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
                //LogHelper.WriteError(ex.Message, ex);
            }
        }

        private void txtPrice_DoubleClick(object sender, EventArgs e)
        {
            errPro.Clear();
            //decimal price =  RealTimeMarketUtil.GetInstance().GetStockLastTrade(txtCode.Text.Trim());
            string errMsg = "";
            decimal price = wcfLogic.GetLastPricByCommodityCode(txtCode.Text.Trim(), 1, out errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                errPro.SetError(txtPrice, errMsg);
            }
            if (price != -1)
            {
                txtPrice.Text = price.ToString();
                SetXHHighLowValue();
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            SetGridViewColor(this.dataGridView1);
        }

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

        private void btnQueryXHCapital_Click(object sender, EventArgs e)
        {
            this.wCFLogicBindingSource.DataSource =
                new SortableBindingList<TraderQueryService.XH_CapitalAccountTableInfo>(wcfLogic.XHCapital);
            this.wCFLogicBindingSource.ResetBindings(false);
        }

        private void btnQueryXHHold_Click(object sender, EventArgs e)
        {
            this.wCFLogicBindingSource1.DataSource = new SortableBindingList<TraderFindService.XH_AccountHoldTableInfo>(wcfLogic.XHHold);
            this.wCFLogicBindingSource1.ResetBindings(false);
        }

        private void btnQueryXHTodayEntrust_Click(object sender, EventArgs e)
        {
            CurrentQueryValue.QueryXHTradeNO = txtQueryXHNumber.Text;
            this.wCFLogicBindingSource2.DataSource =
                new SortableBindingList<TraderFindService.XH_TodayEntrustTableInfo>(wcfLogic.XHTodayEntrust);
            this.wCFLogicBindingSource2.ResetBindings(false);
        }

        private void btnQueryXHTodayTrade_Click(object sender, EventArgs e)
        {
            CurrentQueryValue.QueryHKTradeNO = txtQueryXHTradeNo.Text;
            this.wCFLogicBindingSource3.DataSource =
                new SortableBindingList<TraderFindService.XH_TodayTradeTableInfo>(wcfLogic.XHTodayTrade);
            this.wCFLogicBindingSource3.ResetBindings(false);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
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

        private void btnGZSendOrder_Click(object sender, EventArgs e)
        {
            errPro.Clear();

            StockIndexFuturesOrderRequest order = new StockIndexFuturesOrderRequest();
            //判断Contract是否为空，如果为空则弹出错误提示框并退出
            if (!string.IsNullOrEmpty(cbGZCode.Text.Trim()))
            {
                order.Code = cbGZCode.Text.Trim();
            }
            else
            {
                MessageBox.Show("Code不能为空！", "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            order.BuySell = cbGZBuySell.SelectedIndex == 0
                                ? Types.TransactionDirection.Buying
                                : Types.TransactionDirection.Selling;
            order.FundAccountId = txtGZCapital.Text.Trim(); //"010000002306";
            order.OrderAmount = float.Parse(txtGZAmount.Text.Trim());

            if (!cbGZMarket.Checked)
            {
                //判断Price是否等于零并且不为空，如果为空则弹出错误提示框并退出
                float price = 0;
                //if (DecimalTest(this.txtGZPrice.Text))
                //{
                if (!string.IsNullOrEmpty(txtGZPrice.Text.Trim()))
                {
                    if (float.TryParse(this.txtGZPrice.Text, out price))
                    {
                        if (price == 0)
                        {
                            MessageBox.Show("Price不能为零！", "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Price数据非法！", "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    order.OrderPrice = float.Parse(txtGZPrice.Text.Trim());
                }
                else
                {
                    MessageBox.Show("Price不能为空！", "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
               // }
            }
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

        private void dataGridViewGZQH_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            SetGridViewColor(this.dataGridViewGZQH);
        }

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
                    //message.OrderMessage = msg;
                    LogHelper.WriteDebug(msg);

                    gzqhLogic.UpdateMessage(message.EntrustNumber, errMsg);
                }
            }
        }

        private void btnQueryGZCapital_Click(object sender, EventArgs e)
        {
            this.wCFLogicBindingSource4.DataSource =
                new SortableBindingList<TraderQueryService.QH_CapitalAccountTableInfo>(wcfLogic.GZQHCapital);
            this.wCFLogicBindingSource4.ResetBindings(false);
        }

        private void btnQueryGZHold_Click(object sender, EventArgs e)
        {
            this.wCFLogicBindingSource5.DataSource = new SortableBindingList<TraderFindService.QH_HoldAccountTableInfo>(wcfLogic.GZQHHold);
            this.wCFLogicBindingSource5.ResetBindings(false);
        }

        private void btnQueryGZTodayEntrust_Click(object sender, EventArgs e)
        {
            this.wCFLogicBindingSource6.DataSource =
                new SortableBindingList<TraderFindService.QH_TodayEntrustTableInfo>(wcfLogic.GZQHTodayEntrust);
            this.wCFLogicBindingSource6.ResetBindings(false);
        }

        private void btnQueryGZTodayTrade_Click(object sender, EventArgs e)
        {
            this.wCFLogicBindingSource7.DataSource =
                new SortableBindingList<TraderFindService.QH_TodayTradeTableInfo>(wcfLogic.GZQHTodayTrade);
            this.wCFLogicBindingSource7.ResetBindings(false);
        }

        private void txtGZPrice_DoubleClick(object sender, EventArgs e)
        {
            errPro.Clear();
            //decimal price = RealTimeMarketUtil.GetInstance().GetFutureLastTrade(cbGZCode.Text.Trim());
            string errMsg = "";
            decimal price = wcfLogic.GetLastPricByCommodityCode(cbGZCode.Text.Trim(), 3, out errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                errPro.SetError(txtGZPrice, errMsg);
            }

            if (price != -1)
            {
                txtGZPrice.Text = price.ToString();
                SetGZHighLowValue();
            }
        }

        private void clearAllFinalStateOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isClearXH)
                return;

            isClearXH = true;
            var list = xhLogic.ClearAllFinalStateOrder();
            //xHMessageLogicBindingSource.Clear();
            foreach (var message in list)
            {
                xHMessageLogicBindingSource.Remove(message);
            }

            isClearXH = false;
            ReBindXHData();
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isClearXH)
                return;

            isClearXH = true;
            xhLogic.ClearAll();
            xHMessageLogicBindingSource.Clear();
            isClearXH = false;
        }

        private void clearAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (isClearGZQH)
                return;

            isClearGZQH = true;
            gzqhLogic.ClearAll();
            gZQHMessageLogicBindingSource.Clear();
            isClearGZQH = false;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (wcfLogic.IsServiceOk)
                return;

            bool isSuccess = wcfLogic.Initialize(txtChannelID.Text.Trim());
            if (isSuccess)
            {
                //StartTimer();
            }
            else
            {
                string errMsg = "Start WCF Service failure!";
                MessageBox.Show(errMsg);
            }
        }

        private void btnDisConnect_Click(object sender, EventArgs e)
        {
            if (wcfLogic.IsServiceOk)
            {
                wcfLogic.ShutDown();
                //StopTimer();
            }
        }

        private void clearAllToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
        }

        private void clearToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SetGridNumber(this.dataGridView1, e);
        }

        private void SetGridNumber(DataGridView gridVidw, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(gridVidw.RowHeadersDefaultCellStyle.ForeColor);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(CultureInfo.CurrentUICulture),
                                  gridVidw.DefaultCellStyle.Font, b, e.RowBounds.Location.X + 20,
                                  e.RowBounds.Location.Y + 4);
        }

        private void dataGridViewGZQH_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SetGridNumber(dataGridViewGZQH, e);
        }

        private void btnRegisterChannle_Click(object sender, EventArgs e)
        {
            bool ok = false;
            try
            {
                List<string> list = new List<string>();
                foreach (var item in txtEnturstNo.Text.Split(','))
                {
                    list.Add(item);
                }
                ok = wcfLogic.ChangeEntrustChannel(list, txtChannelID.Text.Trim(), checkBox1.Checked ? 1 : 2);
            }
            catch
            {
            }
            if (ok)
                MessageBox.Show("更新成功!");
        }

        private void txtMax_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                txtXhMax.Text = "";
                var type = cbMarketOrder.Checked
                               ? DoAccountService.TypesOrderPriceType.OPTMarketPrice
                               : DoAccountService.TypesOrderPriceType.OPTLimited;
                var traderId = txtTradeID.Text.Trim();
                var code = txtCode.Text.Trim();

                decimal price = 0;
                if (!cbMarketOrder.Checked)
                {
                    if (string.IsNullOrEmpty(txtPrice.Text))
                    {
                        string msg = "Please input price!";
                        MessageBox.Show(msg);
                        return;
                    }
                    price = decimal.Parse(txtPrice.Text.Trim());
                }

                string errMsg = "";
                var max = wcfLogic.GetXHMaxCount(traderId, code, price, type, out errMsg);

                if (errMsg.Length > 0)
                {
                    //txtXhMax.Text = errMsg;
                    MessageBox.Show(errMsg);
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

        private void txtQHMax_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                txtGZMax.Text = "";
                var type = cbGZMarket.Checked
                               ? DoAccountService.TypesOrderPriceType.OPTMarketPrice
                               : DoAccountService.TypesOrderPriceType.OPTLimited;
                var traderId = txtGZTraderID.Text.Trim();
                var code = cbGZCode.Text.Trim();

                decimal price = 0;
                if (!cbGZMarket.Checked)
                {
                    if (string.IsNullOrEmpty(txtGZPrice.Text))
                    {
                        string msg = "Please input price!";
                        MessageBox.Show(msg);
                        return;
                    }

                    price = decimal.Parse(txtGZPrice.Text.Trim());
                }

                string errMsg = "";
                var max = wcfLogic.GetQHMaxCount(traderId, code, price, type, out errMsg);
                if (errMsg.Length > 0)
                {
                    MessageBox.Show(errMsg);
                    //txtGZMax.Text = errMsg;
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

        private void txtXHHigh_DoubleClick(object sender, EventArgs e)
        {
            SetXHHighLowValue();
        }

        private void SetXHHighLowValue()
        {
            decimal price = 0;

            if (!string.IsNullOrEmpty(txtPrice.Text))
            {
                bool isSuccess = decimal.TryParse(txtPrice.Text.Trim(), out price);
                if (!isSuccess)
                {
                    MessageBox.Show("Price is error!");
                    return;
                }
            }

            var highLowRange = wcfLogic.GetHighLowRangeValueByCommodityCode(txtCode.Text.Trim(), price);

            if (highLowRange == null)
            {
                MessageBox.Show("Can not get highlowrange object!");
                return;
            }

            if (highLowRange.RangeType == Types.HighLowRangeType.HongKongPrice) //港股类型处理
            {
                var hkrv = highLowRange.HongKongRangeValue;

                var buySell = comboBuySell.SelectedIndex == 0
                                  ? Types.TransactionDirection.Buying
                                  : Types.TransactionDirection.Selling;
                if (buySell == Types.TransactionDirection.Buying)
                {
                    txtXHHigh.Text = hkrv.BuyHighRangeValue.ToString();
                    txtXHLow.Text = hkrv.BuyLowRangeValue.ToString();
                }
                else
                {
                    txtXHHigh.Text = hkrv.SellHighRangeValue.ToString();
                    txtXHLow.Text = hkrv.SellLowRangeValue.ToString();
                }
            }
            else //其它类型处理
            {
                txtXHHigh.Text = highLowRange.HighRangeValue.ToString();
                txtXHLow.Text = highLowRange.LowRangeValue.ToString();
            }
        }

        private void txtGZHigh_DoubleClick(object sender, EventArgs e)
        {
            SetGZHighLowValue();
        }

        private void SetGZHighLowValue()
        {
            decimal price = 0;

            if (!string.IsNullOrEmpty(txtGZPrice.Text))
            {
                bool isSuccess = decimal.TryParse(txtGZPrice.Text.Trim(), out price);
                if (!isSuccess)
                {
                    MessageBox.Show("Price is error!");
                    return;
                }
            }

            var highLowRange = wcfLogic.GetHighLowRangeValueByCommodityCode(cbGZCode.Text.Trim(), price);

            if (highLowRange == null)
            {
                MessageBox.Show("Can not get highlowrange object!");
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
                    txtGZHigh.Text = hkrv.BuyHighRangeValue.ToString();
                    txtGZLow.Text = hkrv.BuyLowRangeValue.ToString();
                }
                else
                {
                    txtGZHigh.Text = hkrv.SellHighRangeValue.ToString();
                    txtGZLow.Text = hkrv.SellLowRangeValue.ToString();
                }
            }
            else //其它类型处理
            {
                txtGZHigh.Text = highLowRange.HighRangeValue.ToString();
                txtGZLow.Text = highLowRange.LowRangeValue.ToString();
            }
        }

        private void txtCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (!loadCodeSuccess)
                return;

            if (!CodeManager.Isvalidkey(e.KeyCode))
            {
                if (e.KeyCode != Keys.Back)
                    return;
            }

            txtCode.Text = txtCode.Text.ToUpper();
            lbCodes.Visible = true;
            CodeManager.FillList(lbCodes, txtCode.Text);
            if (lbCodes.Items.Count > 0)
            {
                lbCodes.SelectedIndex = 0;
                lbCodes.Focus();
            }
        }

        private void lbCodes_DoubleClick(object sender, EventArgs e)
        {
            txtCode.Text = GetCode(lbCodes);
            lbCodes.Visible = false;
        }

        private void lbCodes_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtCode.Text = GetCode(lbCodes);
                lbCodes.Visible = false;

                return;
            }

            if (e.KeyCode == Keys.Escape)
            {
                lbCodes.Visible = false;
                return;
            }

            if (e.KeyCode == Keys.Back)
            {
                if (txtCode.Text.Length <= 1)
                    txtCode.Text = "";
                else
                {
                    txtCode.Text = txtCode.Text.Substring(0, txtCode.Text.Length - 1);

                    CodeManager.FillList(lbCodes, txtCode.Text);
                    if (lbCodes.Items.Count > 0)
                        lbCodes.SelectedIndex = 0;
                }
            }

            if (CodeManager.Isvalidkey(e.KeyCode))
            {
                txtCode.Text += GetValidKeyCode(e.KeyCode.ToString());

                CodeManager.FillList(lbCodes, txtCode.Text);
                if (lbCodes.Items.Count > 0)
                    lbCodes.SelectedIndex = 0;
            }
        }

        private void lbCodes_Leave(object sender, EventArgs e)
        {
            lbCodes.Visible = false;
        }

        private void btnHKSendOrder_Click(object sender, EventArgs e)
        {
            errPro.Clear();

            hkDoOrderNum = 0;
            HKOrderRequest order = new HKOrderRequest();
            //判断Code是否为空，如果为空则弹出错误提示框并退出
            if (!string.IsNullOrEmpty(this.txtHKCode.Text))
            {
                order.Code = txtHKCode.Text.Trim();
            }
            else
            {
                MessageBox.Show("Code不能为空！", "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            order.BuySell = cbHKBuySell.SelectedIndex == 0
                                ? Types.TransactionDirection.Buying
                                : Types.TransactionDirection.Selling;
            order.FundAccountId = txtHKCapital.Text.Trim(); //"010000002302";
            order.OrderAmount = float.Parse(txtHKAmount.Text.Trim());

            if (!string.IsNullOrEmpty(txtHKPrice.Text.Trim()))
            {
                //判断Price是否等于零并且不为空，如果为空则弹出错误提示框并退出
                float price = 0;
                //if (DecimalTest(this.txtHKPrice.Text))
                //{
                if (float.TryParse(this.txtHKPrice.Text, out price))
                {
                    if (price == 0)
                    {
                        MessageBox.Show("Price不能为零！", "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Price数据非法！", "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                order.OrderPrice = float.Parse(txtHKPrice.Text.Trim());
          //  }
           
            }
            else
            {
                MessageBox.Show("Price不能为空！", "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            order.TraderId = txtHKTraderID.Text.Trim(); //"23";
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

        private void txtHKPrice_DoubleClick(object sender, EventArgs e)
        {
            errPro.Clear();
            //decimal price = 0 RealTimeMarketUtil.GetInstance().GetHKLastTrade(txtHKCode.Text.Trim());
            string errMsg = "";
            decimal price = wcfLogic.GetLastPricByCommodityCode(txtHKCode.Text.Trim(), 4, out errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                errPro.SetError(txtHKPrice, errMsg);
            }
            if (price != -1)
            {
                txtHKPrice.Text = price.ToString();

                SetHKHighLowValue();
            }


        }

        private void SetHKHighLowValue()
        {
            decimal price = 0;

            if (!string.IsNullOrEmpty(txtHKPrice.Text))
            {
                bool isSuccess = decimal.TryParse(txtHKPrice.Text.Trim(), out price);
                if (!isSuccess)
                {
                    MessageBox.Show("Price is error!");
                    return;
                }
            }
            else
            {
                string msg = "Please input price!";
                MessageBox.Show(msg);
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

            var highLowRange = wcfLogic.GetHKHighLowRangeValueByCommodityCode(txtHKCode.Text.Trim(), price, priceType,
                                                                              tranType);

            if (highLowRange == null)
            {
                MessageBox.Show("Can not get highlowrange object!");
                return;
            }

            if (highLowRange.RangeType == Types.HighLowRangeType.HongKongPrice) //港股类型处理
            {
                var hkrv = highLowRange.HongKongRangeValue;

                var buySell = comboBuySell.SelectedIndex == 0
                                  ? Types.TransactionDirection.Buying
                                  : Types.TransactionDirection.Selling;
                if (buySell == Types.TransactionDirection.Buying)
                {
                    txtHKHigh.Text = hkrv.BuyHighRangeValue.ToString();
                    txtHKLow.Text = hkrv.BuyLowRangeValue.ToString();
                }
                else
                {
                    txtHKHigh.Text = hkrv.SellHighRangeValue.ToString();
                    txtHKLow.Text = hkrv.SellLowRangeValue.ToString();
                }
                labHKHLType.Text = hkrv.HKValidPriceType.ToString();
            }
            else //其它类型处理
            {
                txtHKHigh.Text = highLowRange.HighRangeValue.ToString();
                txtHKLow.Text = highLowRange.LowRangeValue.ToString();
            }
        }

        private void btnQueryHKCapital_Click(object sender, EventArgs e)
        {
            this.wCFLogicBindingSource8.DataSource =
                new SortableBindingList<HKTraderQuerySevice.HK_CapitalAccountInfo>(wcfLogic.HKCapital);
            this.wCFLogicBindingSource8.ResetBindings(false);
        }

        private void btnQueryHKHold_Click(object sender, EventArgs e)
        {
            this.wCFLogicBindingSource9.DataSource = new SortableBindingList<HKTraderQuerySevice.HK_AccountHoldInfo>(wcfLogic.HKHold);
            this.wCFLogicBindingSource9.ResetBindings(false);
        }

        private void btnQueryHKTodayEntrust_Click(object sender, EventArgs e)
        {
            CurrentQueryValue.QueryHKEnNO = txtQueryHKEnNO.Text;
            this.wCFLogicBindingSource10.DataSource =
                new SortableBindingList<HK_TodayEntrustInfo>(wcfLogic.HKTodayEntrust);
            this.wCFLogicBindingSource10.ResetBindings(false);

        }

        private void btnQueryHKTodayTrade_Click(object sender, EventArgs e)
        {
            CurrentQueryValue.QueryHKTradeNO = txtQueryHKTradeNO.Text;
            this.wCFLogicBindingSource11.DataSource =
                new SortableBindingList<HK_TodayTradeInfo>(wcfLogic.HKTodayTrade);
            this.wCFLogicBindingSource11.ResetBindings(false);
        }

        private void txtHKMax_DoubleClick(object sender, EventArgs e)
        {
            try
            {
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
                var code = txtHKCode.Text.Trim();

                decimal price = 0;

                if (string.IsNullOrEmpty(txtHKPrice.Text))
                {
                    string msg = "Please input price!";
                    MessageBox.Show(msg);
                    return;
                }
                price = decimal.Parse(txtHKPrice.Text.Trim());


                string errMsg = "";
                var max = wcfLogic.GetHKMaxCount(traderId, code, price, priceType, out errMsg);

                if (errMsg.Length > 0)
                {
                    //txtXhMax.Text = errMsg;
                    MessageBox.Show(errMsg);
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



        private void txtHKHigh_DoubleClick(object sender, EventArgs e)
        {
            SetHKHighLowValue();
        }

        private void dataGridView10_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView10.SelectedRows)
            {
                hkIndex = row.Index;

                int index = e.ColumnIndex;

                HKMessage message = row.DataBoundItem as HKMessage;
                if (message == null)
                    continue;

                //txtQueryHKEnNO.Text = message.EntrustNumber;
                //txtQueryHKTradeNO.Text = message.EntrustNumber;

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

                //if (hkOrderDict.ContainsKey(message.EntrustNumber))
                //{
                //    var hkOrder = hkOrderDict[message.EntrustNumber];
                //    order.OrderUnitType = hkOrder.OrderUnitType;
                //}
                //else
                //{
                //    order.OrderUnitType = Types.UnitType.Thigh;
                //}

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

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (isClearHK)
                return;

            isClearHK = true;

            hkLogic.ClearAll();
            hKMessageLogicBindingSource.Clear();

            //hkOrderDict.Clear();
            isClearHK = false;
        }

        private void clearAllFinalStateOrderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (isClearHK)
                return;

            isClearHK = true;
            var list = hkLogic.ClearAllFinalStateOrder();
            //xHMessageLogicBindingSource.Clear();
            foreach (var message in list)
            {
                hKMessageLogicBindingSource.Remove(message);
            }

            isClearHK = false;
            ReBindHKData();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            listBox5.Items.Clear();
        }

        #region == 行情初始化 ==

        private ISocket _socketService;

        /// <summary>
        /// 消息打印事件
        /// </summary>
        public OnEventDelegate FEvent;

        private IRealtimeMarketService rms;

        /// <summary>
        /// 瑞尔格特下单服务
        /// </summary>
        private GTASocket SocketService;

        private void InitRealtimeMarketComponent()
        {
            int mode = Utils.GetRealTimeMode();

            if (mode == 1)
            {
                InitRealtimeMarketComponent1();
            }
            else if (mode == 2)
            {
                InitRealtimeMarketComponent2();
            }
        }

        /// <summary>
        /// 初始化行情组件1
        /// </summary>
        private void InitRealtimeMarketComponent1()
        {
            //登陆处理
            //string username = "rtuser";
            //string password = "11";
            string username = Utils.GetRealTimeUserName();
            string password = Utils.GetRealTimePassword();
            GTASocketSingletonForRealTime.StartLogin(username, password, ShowConnectMessage);
            //数据接受注册事件
            this.FEvent += Event;
            SocketService = GTASocketSingletonForRealTime.SocketService;
            GTASocketSingletonForRealTime.SocketStateChanged += GTASocketSingleton_SocketStateChanged;
            SocketService.AddEventDelegate(this.FEvent);
            GTASocketSingletonForRealTime.SocketStatusHandle(SocketStatusChange);
            IRealtimeMarketService rms = RealtimeMarketServiceFactory.GetService();
        }

        /// <summary>
        /// 初始化行情组件2
        /// </summary>
        private void InitRealtimeMarketComponent2()
        {
            ////登陆处理
            ////string username = "rtuser";
            ////string password = "11";
            //string username = Utils.GetRealTimeUserName();
            //string password = Utils.GetRealTimePassword();
            //this.rms = RealtimeMarketServiceFactory2.GetService();
            //rms.Login(username, password, ShowConnectMessage);
            //rms.InitializeService();

            ////数据接受注册事件
            //this.FEvent += Event;
            //_socketService = rms.SocketService;
            //_socketService.AddEventDelegate(this.FEvent);
            //rms.GetGTASingleton.SocketStatusHandle(SocketStatusChange);
        }

        /// <summary>
        /// 状态改变触发事件
        /// </summary>
        /// <param name="e"></param>
        private void SocketStatusChange(SocketServiceStatusEventArg e)
        {
            if (this.statusStrip1s.InvokeRequired)
            {
                statusStrip1s.Invoke(new DelegateSocketStatusChange(delegate { SocketStatusChange(e); }), e);
            }
            else
            {
                if (e.SocketStatus == SocketServiceStatus.SSSConnected || e.SocketStatus == SocketServiceStatus.SSSLogin)
                {
                    this.StatusText.Text = "已连接";

                    //this.MenuResetState("中断连接");
                }
                else if (e.SocketStatus == SocketServiceStatus.SSSDisConnectedNeedReconnect)
                {
                    this.StatusText.Text = "已断开,启动自动重连接...";
                    //this.MenuResetState("重新连接");
                }
                else if (e.SocketStatus == SocketServiceStatus.SSSDisConnected)
                {
                    this.StatusText.Text = "已断开";
                    //this.MenuResetState("重新连接");
                }
                else if (e.SocketStatus == SocketServiceStatus.SSSException)
                {
                    this.StatusText.Text = "连接异常";
                    //this.MenuResetState("重新连接");
                }
                else if (e.SocketStatus == SocketServiceStatus.SSSConnecting)
                {
                    this.StatusText.Text = e.Message; //"正在重新连接……";
                }
                else if (e.SocketStatus == SocketServiceStatus.SSSResetCycleIsEnding)
                {
                    int mode = Utils.GetRealTimeMode();
                    this.StatusText.Text = e.Message;
                    if (MessageBox.Show("连接中断，是否重新连接？", "系统信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) ==
                        DialogResult.Yes)
                    {
                        //if (mode == 1)
                        //{
                        GTASocketSingletonForRealTime.BeginAResetCycle();
                        //}
                        //else if (mode == 2)
                        //{
                        //    this.rms.GetGTASingleton.BeginAResetCycle();
                        //}
                    }
                    else
                    {
                        if (mode == 1)
                        {
                            SocketService.SocketStatus = GTASocketStatus.SSIsManualStoped;
                        }
                        else
                        {
                            _socketService.SocketStatus = GTASocketStatus.SSIsManualStoped;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 状态处理（暂不使用)
        /// </summary>
        /// <param name="state"></param>
        private void GTASocketSingleton_SocketStateChanged(EnumSocketResetState state)
        {
        }

        /// <summary>
        /// 显示登陆信息
        /// </summary>
        /// <param name="e"></param>
        private void ShowConnectMessage(SocketServiceStatusEventArg e)
        {
            if (this.InvokeRequired)
            {
                SocketServiceStatusHandler showConnectMessage = ShowConnectMessage;
                this.Invoke(showConnectMessage, new object[] { e });
            }
            else
            {
                Application.DoEvents();
                switch (e.SocketStatus)
                {
                    case SocketServiceStatus.SSSLogin:
                        //更新程序是否需要更新
                        this.statusStrip1s.Text = "检测更新程序是否需要更新。";
                        Application.DoEvents();
                        // UpdateUpdateSoftSelf();
                        //检测是否需要更新
                        this.statusStrip1s.Text = "异步启动检测客户端是否需要更新。";
                        Application.DoEvents();
                        this.statusStrip1s.Text = "已登陆行情服务器.";
                        Application.DoEvents();
                        this.DialogResult = DialogResult.OK;
                        break;
                    case SocketServiceStatus.SSSErrorUser:


                        MessageBox.Show(e.Message, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.statusStrip1s.Text = "登陆失败，请重新登陆！";
                        break;
                    case SocketServiceStatus.SSSException:
                        break;
                    case SocketServiceStatus.SSSResetCycleIsEnding:
                        this.statusStrip1s.Text = e.Message;

                        break;
                    case SocketServiceStatus.SSSDisConnected:
                        this.statusStrip1s.Text = e.Message;

                        break;
                    case SocketServiceStatus.SSSSocksProxyError:
                        this.statusStrip1s.Text = e.Message;

                        break;
                    default:
                        this.statusStrip1s.Text = e.Message;
                        Application.DoEvents();
                        break;
                }
            }
        }


        /// <summary>
        /// 清除消息
        /// </summary>
        public void ClearEchoInfo()
        {
            //lock (this.lstMessages)
            //{
            //    this.lstMessages.Items.Clear();
            //}
        }

        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="eventMessage"></param>
        public void Event(string eventMessage)
        {
            //MessageDisplayHelper.Event(eventMessage, lstMessages);
        }

        /// <summary>
        /// Socket状态改变触发事件
        /// </summary>
        /// <param name="_e"></param>
        private delegate void DelegateSocketStatusChange(SocketServiceStatusEventArg _e);

        #endregion

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
            //throw new NotImplementedException();
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
            //throw new NotImplementedException();
            return "";
        }

        #endregion

        private void dataGridView10_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            SetGridViewColor(this.dataGridView10);
        }

        private void dataGridView10_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SetGridNumber(this.dataGridView10, e);
        }

        /// <summary>
        /// 现货选择行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                HKMessage message = row.DataBoundItem as HKMessage;
                if (message == null)
                    continue;
                txtQueryXHNumber.Text = message.EntrustNumber;
                txtQueryXHTradeNo.Text = message.EntrustNumber;
            }
        }
        /// <summary>
        /// 港股选择行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView10_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView10.SelectedRows)
            {
                HKMessage message = row.DataBoundItem as HKMessage;
                if (message == null)
                    continue;
                txtQueryHKEnNO.Text = message.EntrustNumber;
                txtQueryHKTradeNO.Text = message.EntrustNumber;
            }

        }

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
            List<HKTraderQuerySevice.HK_HistoryModifyOrderRequestInfo> list = wcfLogic.QueryHKModifyOrderRequest(txtModifyEntrustNumber.Text, start, end, ref strMessage, selectType);
            if (list == null)
            {
                list = new List<HKTraderQuerySevice.HK_HistoryModifyOrderRequestInfo>();
            }

            dgvModifyList.DataSource = list;

            if (!string.IsNullOrEmpty(strMessage))
            {
                errPro.SetError(btnModifyQuery, strMessage);
            }
        }

        private void dgvModifyList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dgvModifyList.SelectedRows)
            {
                HKTraderQuerySevice.HK_HistoryModifyOrderRequestInfo message = row.DataBoundItem as HKTraderQuerySevice.HK_HistoryModifyOrderRequestInfo;
                if (message == null)
                    continue;
                //txtHKHistroyEnNo.Text = message.EntrustNumber;
                txtModifyEntrustNumber.Text = message.EntrustNubmer;
            }
        }

        private void btnQueryMarketValue_Click(object sender, EventArgs e)
        {
            dgvMarketValue.AutoGenerateColumns = false;
            string mess = "";
            //dgvMarketValue.DataSource = wcfLogic.QueryHKHoldMarketValue(txtMarketValuCode.Text.Trim());
            dgvMarketValue.DataSource = wcfLogic.QuerymarketValueHKHold(txtMarketValuCode.Text.Trim(), ref mess);

        }

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

        private void dgvHK_historyEntrust_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dgvHK_historyEntrust.SelectedRows)
            {
                HK_HistoryEntrustInfo message = row.DataBoundItem as HK_HistoryEntrustInfo;
                if (message == null)
                    continue;
                txtHKHistroyEnNo.Text = message.EntrustNumber;
                //txtHKHistroyTradeNo.Text = message.EntrustNumber;
            }
        }

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

        private void btnTotalCapital_Click(object sender, EventArgs e)
        {
            string msg = "";
            List<HKCapitalEntity> list = new List<HKCapitalEntity>();
            HKCapitalEntity entry = wcfLogic.QueryHKTotalCapital((Types.CurrencyType)cmbCurrencyType.SelectedIndex + 1, ref msg);
            if (entry != null)
            {
                list.Add(entry);
            }
            dgvTotalCapital.DataSource = list;

        }

        private void btnXHMarketValue_Click(object sender, EventArgs e)
        {
            dgXHMarketValue.AutoGenerateColumns = false;
            string mess = "";
            dgXHMarketValue.DataSource = wcfLogic.QuerymarketValueXHHold(txtXHMarketValue.Text.Trim(), ref mess);

        }

        private void btnXHTotalCapital_Click(object sender, EventArgs e)
        {
            string msg = "";
            List<TraderFindService.SpotCapitalEntity> list = new List<TraderFindService.SpotCapitalEntity>();
            TraderFindService.SpotCapitalEntity entry = wcfLogic.QueryXHTotalCapital((Types.CurrencyType)cmbXHCurenyType.SelectedIndex + 1, ref msg);
            if (entry != null)
            {
                list.Add(entry);
            }
            dgvXHTotalCapital.DataSource = list;
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
            dgQHMarketValue.DataSource = wcfLogic.QueryMarketValueQHHold(txtQHMarketValue.Text.Trim(), ref mess);

        }

        private void btnQHQueryTotalCapital_Click(object sender, EventArgs e)
        {
            dgvQHTotalCapital.AutoGenerateColumns = false;
            string msg = "";
            List<TraderFindService.FuturesCapitalEntity> list = new List<TraderFindService.FuturesCapitalEntity>();
            TraderFindService.FuturesCapitalEntity entry = wcfLogic.QueryQHTotalCapital((Types.CurrencyType)cmbQHCureny.SelectedIndex + 1, ref msg);
            if (entry != null)
            {
                list.Add(entry);
            }
            dgvQHTotalCapital.DataSource = list;

        }

        private void btnQueryQHFlow_Click(object sender, EventArgs e)
        {
            errPro.Clear();
            dgvQHFlowDetail.AutoGenerateColumns = false;
            string msg = "";
            List<QH_TradeCapitalFlowDetailInfo> list = new List<QH_TradeCapitalFlowDetailInfo>();
            List<QH_TradeCapitalFlowDetailInfo> entry = wcfLogic.QueryQHCapitalFlowDetail((TraderQueryService.QueryTypeQueryCurrencyType)cmbQHFlowCury.SelectedIndex, txtPwd.Text.Trim(), out msg);

            if (!string.IsNullOrEmpty(msg))
            {
                errPro.SetError(txtPwd, msg);
            }

            if (entry != null)
            {
                list = entry;
            }
            dgvQHFlowDetail.DataSource = list;
        }

        /// <summary>
        /// 绑定现货历史委托列表
        /// </summary>
        private void BindXHHistoryEntrustList()
        {
            int icount;

            DateTime? start = null;
            DateTime? end = null;

            if (checkBox4.Checked)
            {
                start = dtpDateXHStart.Value;
                end = dtpDateXHEnd.Value;
            }
            string strMessage = "";
            List<ReckoningCounter.PushBackTest.TraderFindService.XH_HistoryEntrustTableInfo> list = wcfLogic.QueryXHHisotryEntrust(out icount, pageControlXH_HistoryEntrust.CurrentPage, pageControlXH_HistoryEntrust.PageSize, txtXHHENumber.Text, start, end, txtXHCode.Text.Trim(), ref strMessage);
            pageControlXH_HistoryEntrust.RecordsCount = icount;
            if (list == null)
            {
                list = new List<ReckoningCounter.PushBackTest.TraderFindService.XH_HistoryEntrustTableInfo>();
            }
            SortableBindingList<ReckoningCounter.PushBackTest.TraderFindService.XH_HistoryEntrustTableInfo> sortList = null;
            if (list.Count != 0)
            {
                sortList = new SortableBindingList<ReckoningCounter.PushBackTest.TraderFindService.XH_HistoryEntrustTableInfo>(list);
            }

            dgXHHistoryEntrust.DataSource = sortList;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            errPro.Clear();
            //DateTime? start = null;
            //DateTime? end = null;          

            //if (checkBox4.Checked)
            //{
            //    start = dtpDateXHStart.Value;
            //    end = dtpDateXHEnd.Value;
            //}
            string strMessage = "";
            //List<ReckoningCounter.PushBackTest.TraderFindService.XH_HistoryEntrustTableInfo> list = wcfLogic.QueryXHHisotryEntrust(txtXHHENumber.Text, start, end,txtXHCode.Text.Trim(), ref strMessage);
            //if (list == null)
            //{
            //    list = new List<ReckoningCounter.PushBackTest.TraderFindService.XH_HistoryEntrustTableInfo>();
            //}
            //SortableBindingList<ReckoningCounter.PushBackTest.TraderFindService.XH_HistoryEntrustTableInfo> sortList = null;
            //if (list.Count != 0)
            //{
            //    sortList = new SortableBindingList<ReckoningCounter.PushBackTest.TraderFindService.XH_HistoryEntrustTableInfo>(list);
            //}

            //dgXHHistoryEntrust.DataSource = sortList;
            pageControlXH_HistoryEntrust.CurrentPage = 1;
            BindXHHistoryEntrustList();
            pageControlXH_HistoryEntrust.Visible = true;
            pageControlXH_HistoryEntrust.BindData();

            if (!string.IsNullOrEmpty(strMessage))
            {
                errPro.SetError(button4, strMessage);
            }
        }

        
        /// <summary>
        /// 绑定现货历史成交列表
        /// </summary>
        private void BindXHHistoryTradeList()
        {
            int icount;

            DateTime? start = null;
            DateTime? end = null;
            if (checkBox5.Checked)
            {
                start = dtpDateXHStart2.Value;
                end = dtpDateXHEnd2.Value;
            }
            string strMessage = "";
            List<ReckoningCounter.PushBackTest.TraderFindService.XH_HistoryTradeTableInfo> list = wcfLogic.QueryXHHisotryTrade(out icount, pageControlXH_HistoryTrade.CurrentPage, pageControlXH_HistoryTrade.PageSize, txtXHHTNumber.Text, start, end, txtXHCode2.Text.Trim(), ref strMessage);
            pageControlXH_HistoryTrade.RecordsCount = icount;
            if (list == null)
            {
                list = new List<ReckoningCounter.PushBackTest.TraderFindService.XH_HistoryTradeTableInfo>();
            }
            SortableBindingList<ReckoningCounter.PushBackTest.TraderFindService.XH_HistoryTradeTableInfo> sortList = null;
            if (list.Count != 0)
            {
                sortList = new SortableBindingList<ReckoningCounter.PushBackTest.TraderFindService.XH_HistoryTradeTableInfo>(list);
            }
            dgXHHistoryTrade.DataSource = sortList;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            errPro.Clear();
            //DateTime? start = null;
            //DateTime? end = null;
            //if (checkBox5.Checked)
            //{
            //    start = dtpDateXHStart2.Value;
            //    end = dtpDateXHEnd2.Value;
            //}
            string strMessage = "";
            //List<ReckoningCounter.PushBackTest.TraderFindService.XH_HistoryTradeTableInfo> list = wcfLogic.QueryXHHisotryTrade(txtXHHTNumber.Text, start, end, txtXHCode2.Text.Trim(), ref strMessage);
            //if (list == null)
            //{
            //    list = new List<ReckoningCounter.PushBackTest.TraderFindService.XH_HistoryTradeTableInfo>();
            //}
            //SortableBindingList<ReckoningCounter.PushBackTest.TraderFindService.XH_HistoryTradeTableInfo> sortList = null;
            //if (list.Count != 0)
            //{
            //    sortList = new SortableBindingList<ReckoningCounter.PushBackTest.TraderFindService.XH_HistoryTradeTableInfo>(list);
            //}
            //dgXHHistoryTrade.DataSource = sortList;
            pageControlXH_HistoryTrade.CurrentPage = 1;
            BindXHHistoryTradeList();
            pageControlXH_HistoryTrade.Visible = true;
            pageControlXH_HistoryTrade.BindData();

            if (!string.IsNullOrEmpty(strMessage))
            {
                errPro.SetError(button5, strMessage);
            }
        }



    }
}