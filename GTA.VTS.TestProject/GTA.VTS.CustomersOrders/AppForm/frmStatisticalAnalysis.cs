using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTA.VTS.CustomersOrders.BLL;
using GTA.VTS.CustomersOrders.DoCommonQuery;
using GTA.VTS.CustomersOrders.HKCommonQuery;
using System.Threading;
using GTA.VTS.Common.CommonUtility;
using GTA.VTS.CustomersOrders.DoOrderService;
using System.Globalization;
using Amib.Threading;

namespace GTA.VTS.CustomersOrders.AppForm
{
    /// <summary>
    /// 当日交易统计分析窗体
    /// </summary>
    public partial class frmStatisticalAnalysis : MdiFormBase
    {
        List<AnalysisEntry> todaylist = new List<AnalysisEntry>();
        List<AnalysisEntry> Historylist = new List<AnalysisEntry>();

        protected ReaderWriterLockSlim listLock = new ReaderWriterLockSlim();


        Dictionary<string, AnalysisEntry> detaillList = new Dictionary<string, AnalysisEntry>();
        protected ReaderWriterLockSlim dicLock = new ReaderWriterLockSlim();

        private Dictionary<string, Thread> listThread = new Dictionary<string, Thread>();
        /// <summary>
        /// 线程池
        /// </summary>
        private SmartThreadPool smartPool = new SmartThreadPool { MaxThreads = 200, MinThreads = 25 };

        public frmStatisticalAnalysis()
        {
            InitializeComponent();
            LocalhostResourcesFormText();
        }

        /// <summary>
        /// 窗体下载初始化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTodayStatisticalAnalysis_Load(object sender, EventArgs e)
        {
            this.dgvHistoryTradeAnals.AutoGenerateColumns = false;
            this.dgvTodayTradeAnals.AutoGenerateColumns = false;
            this.dgvCodeListAnalysis.AutoGenerateColumns = false;
            smartPool.Start();

        }

        /// <summary>
        /// 当日统计分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryAnalysis_Click(object sender, EventArgs e)
        {
            try
            {
                this.btnTodayQueryAnalysis.Enabled = false;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                todaylist.Clear();
                QueryTodayAnalysis();
                this.dgvTodayTradeAnals.DataSource = todaylist;
                this.dgvTodayTradeAnals.Refresh();
                this.btnTodayQueryAnalysis.Enabled = true;
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 刷新显示相关语方类型的标签
        /// </summary>
        private void LocalhostResourcesFormText()
        {
            this.Text = ResourceOperate.Instanse.GetResourceByKey("tsmAnalysi");
            this.btnQueryHistoryAnalysis.Text = ResourceOperate.Instanse.GetResourceByKey("QueryHistoryAnalysis");
            this.gbTodayTradeAnalysis.Text = ResourceOperate.Instanse.GetResourceByKey("gbTodayTradeAnalysis");
            this.gbHistoryTradeAnalysis.Text = ResourceOperate.Instanse.GetResourceByKey("gbHistoryTradeAnalysis");
            this.btnTodayQueryAnalysis.Text = ResourceOperate.Instanse.GetResourceByKey("TodayQueryAnalysis");
            this.labStart.Text = ResourceOperate.Instanse.GetResourceByKey("start");
            this.labEnd.Text = ResourceOperate.Instanse.GetResourceByKey("end");
            #region 今日委托成交统计
            for (int i = 0; i < this.dgvTodayTradeAnals.ColumnCount; i++)
            {
                string Name = dgvTodayTradeAnals.Columns[i].Name;
                dgvTodayTradeAnals.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(Name);
            }
            #endregion 今日委托成交统计

            #region 历史交易统计统计
            for (int i = 0; i < this.dgvHistoryTradeAnals.ColumnCount; i++)
            {
                string Name = dgvHistoryTradeAnals.Columns[i].Name;
                dgvHistoryTradeAnals.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(Name);
            }
            for (int i = 0; i < this.dgvCodeListAnalysis.ColumnCount; i++)
            {
                string Name = dgvCodeListAnalysis.Columns[i].Name;
                dgvCodeListAnalysis.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(Name);
            }
            #endregion 历史交易统计统计
        }

        /// <summary>
        /// 分析商品期货
        /// </summary>
        private void QueryTodayAnalysis()
        {

            int total = 0;
            string errorMsg = "";

            #region 分页信息
            DoCommonQuery.PagingInfo pageInfo = new DoCommonQuery.PagingInfo();
            //当第一页时要返回总记录数，到了第二页就不应再返回总记录数，这样可以提高查询速度
            pageInfo.IsCount = true;
            //这里我们只是为了要记录总数，这里设置页数设置小一些，序列也快一些返回
            pageInfo.PageLength = 2;
            //第一页开始以1开始
            pageInfo.CurrentPage = 1;
            #endregion

            #region 商品期货
            AnalysisEntry modelSPQH = new AnalysisEntry();
            modelSPQH.BreedClassName = "商品期货";

            FuturesEntrustConditionFindEntity spQHfilter = new FuturesEntrustConditionFindEntity();
            spQHfilter.CapitalAccount = ServerConfig.SPQHCapitalAccount;
            List<QH_TodayEntrustTableInfo> todaySPQHItems = WCFServices.Instance.QueryQHTodayEntrust(out total, out errorMsg, ServerConfig.TraderID, "", 6, spQHfilter, pageInfo);
            modelSPQH.Count = total.ToString();
            for (int i = 1; i < 12; i++)
            {
                total = 0;
                pageInfo.CurrentPage = 1;
                spQHfilter.EntrustState = i;

                todaySPQHItems = WCFServices.Instance.QueryQHTodayEntrust(out total, out errorMsg, ServerConfig.TraderID, "", 6, spQHfilter, pageInfo);
                SetAnalysisEntry(modelSPQH, i, total, int.Parse(modelSPQH.Count));
            }
            todaylist.Add(modelSPQH);
            #endregion

            #region 股指期货
            AnalysisEntry modelGZQH = new AnalysisEntry();
            modelGZQH.BreedClassName = "股指期货";

            FuturesEntrustConditionFindEntity gzQHfilter = new FuturesEntrustConditionFindEntity();
            gzQHfilter.CapitalAccount = ServerConfig.GZQHCapitalAccount;
            List<QH_TodayEntrustTableInfo> todayGZQHItems = WCFServices.Instance.QueryQHTodayEntrust(out total, out errorMsg, ServerConfig.TraderID, "", 4, gzQHfilter, pageInfo);
            modelGZQH.Count = total.ToString();
            for (int i = 1; i < 12; i++)
            {
                total = 0;

                pageInfo.CurrentPage = 1;
                gzQHfilter.EntrustState = i;
                todayGZQHItems = WCFServices.Instance.QueryQHTodayEntrust(out total, out errorMsg, ServerConfig.TraderID, "", 4, gzQHfilter, pageInfo);
                SetAnalysisEntry(modelGZQH, i, total, int.Parse(modelGZQH.Count));
            }
            todaylist.Add(modelGZQH);
            #endregion

            #region 现货
            AnalysisEntry modelXH = new AnalysisEntry();
            modelXH.BreedClassName = "现货";

            SpotEntrustConditionFindEntity xhFilter = new SpotEntrustConditionFindEntity();
            xhFilter.SpotCapitalAccount = ServerConfig.XHCapitalAccount;
            List<XH_TodayEntrustTableInfo> todayXHItems = WCFServices.Instance.QueryXHTodayEntrust(out total, out errorMsg, ServerConfig.TraderID, "", 2, xhFilter, pageInfo);
            modelXH.Count = total.ToString();
            for (int i = 1; i < 12; i++)
            {
                total = 0;
                pageInfo.CurrentPage = 1;
                xhFilter.EntrustState = i;
                todayXHItems = WCFServices.Instance.QueryXHTodayEntrust(out total, out errorMsg, ServerConfig.TraderID, "", 2, xhFilter, pageInfo);
                SetAnalysisEntry(modelXH, i, total, int.Parse(modelXH.Count));
            }
            todaylist.Add(modelXH);
            #endregion

            #region 港股

            HKCommonQuery.PagingInfo hkpageInfo = new HKCommonQuery.PagingInfo();
            //当第一页时要返回总记录数，到了第二页就不应再返回总记录数，这样可以提高查询速度
            hkpageInfo.IsCount = true;
            //这里我们只是为了要记录总数，这里设置页数设置小一些，序列也快一些返回
            hkpageInfo.PageLength = 2;
            //第一页开始以1开始
            hkpageInfo.CurrentPage = 1;

            AnalysisEntry modelHK = new AnalysisEntry();
            modelHK.BreedClassName = "港股";

            HKEntrustConditionFindEntity hkFilter = new HKEntrustConditionFindEntity();
            hkFilter.HKCapitalAccount = ServerConfig.HKCapitalAccount;
            List<HK_TodayEntrustInfo> todayHKItems = WCFServices.Instance.QueryHKTodayEntrust(out total, out errorMsg, hkFilter, hkpageInfo);
            modelHK.Count = total.ToString();
            for (int i = 1; i < 12; i++)
            {
                total = 0;
                hkpageInfo.CurrentPage = 1;
                hkFilter.EntrustState = i;
                todayHKItems = WCFServices.Instance.QueryHKTodayEntrust(out total, out errorMsg, hkFilter, hkpageInfo);
                SetAnalysisEntry(modelHK, i, total, int.Parse(modelHK.Count));
            }
            todaylist.Add(modelHK);
            #endregion

        }


        /// <summary>
        /// 根据类型设置相关实体内容
        /// </summary>
        /// <param name="model"></param>
        /// <param name="orderType"></param>
        /// <param name="items">类型总量</param>
        /// <param name="sum">总数</param>
        private void SetAnalysisEntry(AnalysisEntry model, int orderType, int items, int sum)
        {
            double point = 0.000;
            if (sum != 0)
            {
                point = (double)items / sum;
            }
            string txt = items.ToString() + "笔>>" + (point * 100).ToString("0.000") + "%";
            switch (orderType)
            {
                case 1:
                    model.None = txt;
                    break;
                case 2:
                    model.UnRequired = txt;
                    break;
                case 3:
                    model.RequiredSoon = txt;
                    break;
                case 4:
                    model.RequiredRemoveSoon = txt;
                    break;
                case 5:
                    model.IsRequired = txt;
                    break;
                case 6:
                    model.Canceled = txt;
                    break;
                case 7:
                    model.Removed = txt;
                    break;
                case 8:
                    model.PartRemoved = txt;
                    break;
                case 9:
                    model.PartDealed = txt;
                    break;
                case 10:
                    model.Dealed = txt;
                    break;
                case 11:
                    model.PartDealRemoveSoon = txt;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 历史统计分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryHistoryAnalysis_Click(object sender, EventArgs e)
        {
            try
            {
                this.btnQueryHistoryAnalysis.Enabled = false;
                this.dtpEndDate.Enabled = false;
                this.dtpStartDate.Enabled = false;
                //this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                ClearHistoryList();
                for (int k = 1; k < 5; k++)
                {
                    Thread th = new Thread(delegate() { QueryHistoryAnalysis(k); });
                    th.Start();
                    if (listThread.ContainsKey(k.ToString()))
                    {
                        try
                        {
                            listThread[k.ToString()].Abort();
                            listThread.Remove(k.ToString());
                        }
                        catch (Exception ex)
                        {
                            LogHelper.WriteError(ex.Message, ex);
                        }
                    }
                    listThread.Add(k.ToString(), th);

                    //这是为了防止K发生变化时线程还没有开启
                    Thread.CurrentThread.Join(300);
                }
                //System.Timers.Timer time = new System.Timers.Timer();
                //time.Interval = 2 * 1000;
                //time.Elapsed += delegate
                //{
                //    this.Invoke(new MethodInvoker(() =>
                //        {
                //            this.dgvHistoryTradeAnals.DataSource = null;
                //            this.dgvHistoryTradeAnals.DataSource = Historylist;
                //            this.btnQueryHistoryAnalysis.Enabled = true;
                //        }));
                //};
                //time.Enabled = true;
                this.dgvHistoryTradeAnals.DataSource = null;
                this.dgvCodeListAnalysis.DataSource = null;
                //this.dgvHistoryTradeAnals.Refresh();
                //this.btnQueryHistoryAnalysis.Enabled = true;
                //this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 分析商品期货
        /// <param name="type">类型（为应用于多程操作）</param>
        /// </summary>
        private void QueryHistoryAnalysis(int type)
        {
            int total = 0;
            string errorMsg = "";

            #region 分页信息
            DoCommonQuery.PagingInfo pageInfo = new DoCommonQuery.PagingInfo();
            //当第一页时要返回总记录数，到了第二页就不应再返回总记录数，这样可以提高查询速度
            pageInfo.IsCount = true;
            //这里我们只是为了要记录总数，这里设置页数设置小一些，序列也快一些返回
            pageInfo.PageLength = 2;
            //第一页开始以1开始
            pageInfo.CurrentPage = 1;
            #endregion

            switch (type)
            {
                case 1:
                    #region 商品期货
                    string txtSPQHMesg = "正在查询商品期货";
                    LogHelper.WriteDebug("正在查询商品期货...");

                    AnalysisEntry modelSPQH = new AnalysisEntry();
                    modelSPQH.BreedClassName = "商品期货";

                    FuturesEntrustConditionFindEntity spQHFilter = new FuturesEntrustConditionFindEntity();
                    spQHFilter.CapitalAccount = ServerConfig.SPQHCapitalAccount;
                    spQHFilter.StartTime = dtpStartDate.Value.Date;
                    spQHFilter.EndTime = dtpEndDate.Value.Date;
                    List<QH_HistoryEntrustTableInfo> todaySPQHItems = WCFServices.Instance.QueryQHHistoryEntrust(out total, out errorMsg, ServerConfig.TraderID, "", 6, spQHFilter, pageInfo);
                    modelSPQH.Count = total.ToString();

                    double spQHFloatProfitLoss = 0.000, spQHMarketProfitLoss = 0.000;
                    for (int i = 6; i < 12; i++)
                    {
                        if (i == 9 || i == 11)
                        {
                            continue;
                        }
                        total = 0;
                        pageInfo.CurrentPage = 1;
                        pageInfo.IsCount = true;
                        spQHFilter.EntrustState = i;

                        #region 先统计本状态页数
                        todaySPQHItems = WCFServices.Instance.QueryQHHistoryEntrust(out total, out errorMsg, ServerConfig.TraderID, "", 6, spQHFilter, pageInfo);
                        SetAnalysisEntry(modelSPQH, i, total, int.Parse(modelSPQH.Count));
                        #endregion

                        todaySPQHItems = new List<QH_HistoryEntrustTableInfo>();

                        if (total > 0)
                        {
                            pageInfo.IsCount = false;//为了提速度
                            pageInfo.PageLength = 500;
                            //只有一页数据
                            if (total <= pageInfo.PageLength)
                            {
                                todaySPQHItems = WCFServices.Instance.QueryQHHistoryEntrust(out total, out errorMsg, ServerConfig.TraderID, "", 6, spQHFilter, pageInfo);
                            }
                            else
                            {
                                #region 多页数据循环查询
                                int pageSize = total / pageInfo.PageLength;
                                if (total % pageInfo.PageLength != 0)
                                {
                                    pageSize += 1;
                                }

                                for (int k = 1; k <= pageSize; k++)
                                {
                                    // this.Invoke(new MethodInvoker(() => { this.labMessage.Text = txtSPQHMesg + "状态:" + i + "第" + k + "/" + pageSize + "页"; }));
                                    smartPool.QueueWorkItem(WirteQueryMessge, txtSPQHMesg + "状态:" + i + "第" + k + "/" + pageSize + "页");

                                    pageInfo.CurrentPage = k;
                                    List<QH_HistoryEntrustTableInfo> listSPQH = WCFServices.Instance.QueryQHHistoryEntrust(out total, out errorMsg, ServerConfig.TraderID, "", 6, spQHFilter, pageInfo);
                                    if (listSPQH != null && listSPQH.Count > 0)
                                    {
                                        todaySPQHItems.AddRange(listSPQH);
                                    }
                                }
                                #endregion
                            }

                            #region 本次状态的统计
                            if (todaySPQHItems != null)
                            {
                                #region 后面还要用来统计不能过滤
                                //todaySPQHItems = todaySPQHItems.FindAll(
                                //    new Predicate<QH_HistoryEntrustTableInfo>((QH_HistoryEntrustTableInfo model) =>
                                //    {
                                //        if (model.CloseMarketProfitLoss != 0 || model.CloseFloatProfitLoss != 0)
                                //        {
                                //            return true;
                                //        }
                                //        else
                                //        {
                                //            return false;
                                //        }
                                //    }));
                                #endregion
                                foreach (var item in todaySPQHItems)
                                {
                                    AnalysisEntry model = new AnalysisEntry();
                                    //为了后面的转型为null异常处理
                                    model.Canceled = "0";
                                    model.PartRemoved = "0";

                                    model.BreedClassName = item.ContractCode + "@" + item.OpenCloseTypeId;
                                    switch ((TypesFutureOpenCloseType)item.OpenCloseTypeId)
                                    {
                                        case TypesFutureOpenCloseType.OpenPosition:
                                            model.None = "开仓";
                                            break;
                                        case TypesFutureOpenCloseType.ClosePosition:
                                            model.None = "平历史";
                                            break;
                                        case TypesFutureOpenCloseType.CloseTodayPosition:
                                            model.None = "平今";
                                            break;
                                    }
                                    switch ((GTA.VTS.Common.CommonObject.Types.TransactionDirection)item.BuySellTypeId)
                                    {
                                        case GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying:
                                            model.PartRemoved = item.EntrustAmount.ToString();//买笔数
                                            break;
                                        case GTA.VTS.Common.CommonObject.Types.TransactionDirection.Selling:
                                            model.Canceled = item.EntrustAmount.ToString();   //卖笔数
                                            break;
                                    }

                                    model.Dealed = item.TradeAmount.ToString();//成交笔数
                                    model.Removed = item.CancelAmount.ToString();//撤单笔数
                                    model.Count = item.EntrustAmount.ToString();        //总笔数

                                    model.FloatProfitLoss = item.CloseFloatProfitLoss.ToString(); ;//浮动盈亏
                                    model.MarketProfitLoss = item.CloseMarketProfitLoss.ToString(); ;//盯市盈亏
                                    model.RequiredSoon = "1";//标识品种类型
                                    AddOrUpdate(model);

                                    spQHFloatProfitLoss += double.Parse(item.CloseFloatProfitLoss.ToString());
                                    spQHMarketProfitLoss += double.Parse(item.CloseMarketProfitLoss.ToString());
                                }
                            }
                            #endregion
                        }
                    }
                    modelSPQH.MarketProfitLoss = spQHMarketProfitLoss.ToString();
                    modelSPQH.FloatProfitLoss = spQHFloatProfitLoss.ToString();
                    //用完后即清算
                    todaySPQHItems.Clear();
                    AddHistoryList(modelSPQH);
                    LogHelper.WriteDebug("结束查询商品期货...");
                    #endregion
                    break;
                case 2:
                    #region 股指期货
                    string txtGZQHMesg = "正在查询股指期货";
                    LogHelper.WriteDebug("正在查询股指期货...");
                    AnalysisEntry modelGZQH = new AnalysisEntry();
                    modelGZQH.BreedClassName = "股指期货";

                    FuturesEntrustConditionFindEntity gzQHFilter = new FuturesEntrustConditionFindEntity();
                    gzQHFilter.CapitalAccount = ServerConfig.GZQHCapitalAccount;
                    gzQHFilter.StartTime = dtpStartDate.Value.Date;
                    gzQHFilter.EndTime = dtpEndDate.Value.Date;
                    List<QH_HistoryEntrustTableInfo> todayGZQHItems = WCFServices.Instance.QueryQHHistoryEntrust(out total, out errorMsg, ServerConfig.TraderID, "", 4, gzQHFilter, pageInfo);
                    modelGZQH.Count = total.ToString();

                    double gzQHFloatProfitLoss = 0.000, gzQHMarketProfitLoss = 0.000;
                    for (int i = 6; i < 12; i++)
                    {
                        if (i == 9 || i == 11)
                        {
                            continue;
                        }
                        total = 0;

                        pageInfo.CurrentPage = 1;
                        pageInfo.IsCount = true;
                        gzQHFilter.EntrustState = i;
                        todayGZQHItems = WCFServices.Instance.QueryQHHistoryEntrust(out total, out errorMsg, ServerConfig.TraderID, "", 4, gzQHFilter, pageInfo);
                        SetAnalysisEntry(modelGZQH, i, total, int.Parse(modelGZQH.Count));

                        todayGZQHItems = new List<QH_HistoryEntrustTableInfo>();
                        if (total > 0)
                        {
                            pageInfo.IsCount = false;//为了提速度
                            pageInfo.PageLength = 500;
                            //只有一页数据
                            if (total <= pageInfo.PageLength)
                            {
                                todayGZQHItems = WCFServices.Instance.QueryQHHistoryEntrust(out total, out errorMsg, ServerConfig.TraderID, "", 4, gzQHFilter, pageInfo);
                            }
                            else
                            {
                                #region 多页数据循环查询
                                int pageSize = total / pageInfo.PageLength;
                                if (total % pageInfo.PageLength != 0)
                                {
                                    pageSize += 1;
                                }

                                for (int k = 1; k <= pageSize; k++)
                                {
                                    //this.Invoke(new MethodInvoker(() => { this.labMessage.Text = txtGZQHMesg + "状态:" + i + "第" + k + "/" + pageSize + "页"; }));
                                    smartPool.QueueWorkItem(WirteQueryMessge, txtGZQHMesg + "状态:" + i + "第" + k + "/" + pageSize + "页");

                                    pageInfo.CurrentPage = k;
                                    pageInfo.IsCount = false;//为了提速度

                                    List<QH_HistoryEntrustTableInfo> listGZQH = WCFServices.Instance.QueryQHHistoryEntrust(out total, out errorMsg, ServerConfig.TraderID, "", 4, gzQHFilter, pageInfo);
                                    if (listGZQH != null && listGZQH.Count > 0)
                                    {
                                        todayGZQHItems.AddRange(listGZQH);
                                    }
                                }
                                #endregion
                            }

                            #region 本次状态的统计
                            if (todayGZQHItems != null)
                            {
                                #region 不要过滤后面统考要用
                                //todayGZQHItems = todayGZQHItems.FindAll(
                                //    new Predicate<QH_HistoryEntrustTableInfo>((QH_HistoryEntrustTableInfo model) =>
                                //    {
                                //        if (model.CloseMarketProfitLoss != 0 || model.CloseFloatProfitLoss != 0)
                                //        {
                                //            return true;
                                //        }
                                //        else
                                //        {
                                //            return false;
                                //        }
                                //    }));
                                #endregion

                                foreach (var item in todayGZQHItems)
                                {
                                    AnalysisEntry model = new AnalysisEntry();
                                    //为了后面的转型为null异常处理
                                    model.Canceled = "0";
                                    model.PartRemoved = "0";

                                    model.BreedClassName = item.ContractCode + "@" + item.OpenCloseTypeId;
                                    switch ((TypesFutureOpenCloseType)item.OpenCloseTypeId)
                                    {
                                        case TypesFutureOpenCloseType.OpenPosition:
                                            model.None = "开仓";
                                            break;
                                        case TypesFutureOpenCloseType.ClosePosition:
                                            model.None = "平历史";
                                            break;
                                        case TypesFutureOpenCloseType.CloseTodayPosition:
                                            model.None = "平今";
                                            break;
                                    }
                                    switch ((GTA.VTS.Common.CommonObject.Types.TransactionDirection)item.BuySellTypeId)
                                    {
                                        case GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying:
                                            model.PartRemoved = item.EntrustAmount.ToString();//买笔数
                                            break;
                                        case GTA.VTS.Common.CommonObject.Types.TransactionDirection.Selling:
                                            model.Canceled = item.EntrustAmount.ToString();   //卖笔数
                                            break;
                                    }

                                    model.Dealed = item.TradeAmount.ToString();//成交笔数
                                    model.Removed = item.CancelAmount.ToString();//撤单笔数
                                    model.Count = item.EntrustAmount.ToString();        //总笔数

                                    model.FloatProfitLoss = item.CloseFloatProfitLoss.ToString(); ;//浮动盈亏
                                    model.MarketProfitLoss = item.CloseMarketProfitLoss.ToString(); ;//盯市盈亏
                                    model.RequiredSoon = "2";//标识品种类型
                                    AddOrUpdate(model);


                                    gzQHFloatProfitLoss += double.Parse(item.CloseFloatProfitLoss.ToString());
                                    gzQHMarketProfitLoss += double.Parse(item.CloseMarketProfitLoss.ToString());
                                }
                            }
                            #endregion
                        }
                    }
                    modelGZQH.MarketProfitLoss = gzQHMarketProfitLoss.ToString();
                    modelGZQH.FloatProfitLoss = gzQHFloatProfitLoss.ToString();
                    //用完后即清算
                    todayGZQHItems.Clear();
                    AddHistoryList(modelGZQH);
                    LogHelper.WriteDebug("End查询股指期货...");
                    #endregion
                    break;
                case 3:
                    #region 现货
                    string txtXHMessage = "正在查询现货";
                    LogHelper.WriteDebug("正在查询现货...");
                    AnalysisEntry modelXH = new AnalysisEntry();
                    modelXH.BreedClassName = "现货";

                    SpotEntrustConditionFindEntity xhFilter = new SpotEntrustConditionFindEntity();
                    xhFilter.SpotCapitalAccount = ServerConfig.XHCapitalAccount;
                    xhFilter.StartTime = dtpStartDate.Value.Date;
                    xhFilter.EndTime = dtpEndDate.Value.Date;

                    List<XH_HistoryEntrustTableInfo> todayXHItems = WCFServices.Instance.QueryXHHistoryEntrust(out total, out errorMsg, xhFilter, pageInfo);
                    modelXH.Count = total.ToString();

                    double xhHasDoneProfitLoss = 0.000;

                    for (int i = 6; i < 12; i++)
                    {
                        if (i == 9 || i == 11)
                        {
                            continue;
                        }
                        total = 0;
                        pageInfo.CurrentPage = 1;
                        pageInfo.IsCount = true;
                        xhFilter.EntrustState = i;
                        todayXHItems = WCFServices.Instance.QueryXHHistoryEntrust(out total, out errorMsg, xhFilter, pageInfo);
                        SetAnalysisEntry(modelXH, i, total, int.Parse(modelXH.Count));

                        todayXHItems = new List<XH_HistoryEntrustTableInfo>();
                        if (total > 0)
                        {
                            pageInfo.IsCount = false;//为了提速度
                            pageInfo.PageLength = 500;
                            //只有一页数据
                            if (total <= pageInfo.PageLength)
                            {
                                todayXHItems = WCFServices.Instance.QueryXHHistoryEntrust(out total, out errorMsg, xhFilter, pageInfo);
                            }
                            else
                            {
                                #region 多页数据循环查询
                                int pageSize = total / pageInfo.PageLength;
                                if (total % pageInfo.PageLength != 0)
                                {
                                    pageSize += 1;
                                }

                                for (int k = 1; k <= pageSize; k++)
                                {
                                    //  this.Invoke(new MethodInvoker(() => { this.labMessage.Text = txtXHMessage + "状态:" + i + "第" + k + "/" + pageSize + "页"; }));
                                    smartPool.QueueWorkItem(WirteQueryMessge, txtXHMessage + "状态:" + i + "第" + k + "/" + pageSize + "页");
                                    pageInfo.CurrentPage = k;
                                    pageInfo.IsCount = false;//为了提速度

                                    List<XH_HistoryEntrustTableInfo> listXH = WCFServices.Instance.QueryXHHistoryEntrust(out total, out errorMsg, xhFilter, pageInfo);
                                    if (listXH != null && listXH.Count > 0)
                                    {
                                        todayXHItems.AddRange(listXH);
                                    }
                                }
                                #endregion
                            }

                            #region 本次状态的统计
                            if (todayXHItems != null)
                            {
                                // todayXHItems = todayXHItems.FindAll(FindHasDone);

                                #region 不要过滤后面统考要用
                                //todayXHItems = todayXHItems.FindAll(new Predicate<XH_HistoryEntrustTableInfo>((XH_HistoryEntrustTableInfo model) => { if (model.HasDoneProfit != 0) { return true; } else { return false; } }));
                                #endregion

                                foreach (var item in todayXHItems)
                                {
                                    AnalysisEntry model = new AnalysisEntry();
                                    //为了后面的转型为null异常处理
                                    model.Canceled = "0";
                                    model.PartRemoved = "0";

                                    model.BreedClassName = item.SpotCode + "@";
                                    model.None = "";

                                    switch ((GTA.VTS.Common.CommonObject.Types.TransactionDirection)item.BuySellTypeId)
                                    {
                                        case GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying:
                                            model.PartRemoved = item.EntrustMount.ToString();//买笔数
                                            break;
                                        case GTA.VTS.Common.CommonObject.Types.TransactionDirection.Selling:
                                            model.Canceled = item.EntrustMount.ToString();   //卖笔数
                                            break;
                                    }

                                    model.Dealed = item.TradeAmount.ToString();//成交笔数
                                    model.Removed = item.CancelAmount.ToString();//撤单笔数
                                    model.Count = item.EntrustMount.ToString();        //总笔数

                                    model.FloatProfitLoss = item.HasDoneProfit.ToString(); ;//浮动盈亏
                                    model.MarketProfitLoss = "0";//盯市盈亏
                                    model.RequiredSoon = "3";//标识品种类型
                                    AddOrUpdate(model);

                                    xhHasDoneProfitLoss += double.Parse(item.HasDoneProfit.ToString());
                                }
                            }
                            #endregion
                        }

                    }
                    modelXH.FloatProfitLoss = xhHasDoneProfitLoss.ToString();
                    //用完后即清算
                    todayXHItems.Clear();
                    AddHistoryList(modelXH);
                    LogHelper.WriteDebug("End查询现货...");
                    #endregion
                    break;
                case 4:
                    #region 港股
                    string txtHKMessage = "开始查询港股";
                    LogHelper.WriteDebug("开始查询港股...");

                    HKCommonQuery.PagingInfo hkpageInfo = new HKCommonQuery.PagingInfo();
                    //当第一页时要返回总记录数，到了第二页就不应再返回总记录数，这样可以提高查询速度
                    hkpageInfo.IsCount = true;
                    //这里我们只是为了要记录总数，这里设置页数设置小一些，序列也快一些返回
                    hkpageInfo.PageLength = 2;
                    //第一页开始以1开始
                    hkpageInfo.CurrentPage = 1;

                    AnalysisEntry modelHK = new AnalysisEntry();
                    modelHK.BreedClassName = "港股";

                    HKEntrustConditionFindEntity hkFilter = new HKEntrustConditionFindEntity();
                    hkFilter.HKCapitalAccount = ServerConfig.HKCapitalAccount;
                    hkFilter.StartTime = dtpStartDate.Value.Date;
                    hkFilter.EndTime = dtpEndDate.Value.Date;

                    List<HK_HistoryEntrustInfo> todayHKItems = WCFServices.Instance.QueryHKHisotryEntrust(out total, out errorMsg, hkFilter, hkpageInfo);
                    modelHK.Count = total.ToString();


                    double hkHasDoneProfitLoss = 0.000;

                    for (int i = 6; i < 12; i++)
                    {
                        if (i == 9 || i == 11)
                        {
                            continue;
                        }
                        total = 0;
                        hkpageInfo.CurrentPage = 1;
                        hkpageInfo.IsCount = true;
                        hkFilter.EntrustState = i;
                        todayHKItems = WCFServices.Instance.QueryHKHisotryEntrust(out total, out errorMsg, hkFilter, hkpageInfo);
                        SetAnalysisEntry(modelHK, i, total, int.Parse(modelHK.Count));


                        todayHKItems = new List<HK_HistoryEntrustInfo>();
                        if (total > 0)
                        {
                            hkpageInfo.IsCount = false;//为了提速度
                            hkpageInfo.PageLength = 500;
                            //只有一页数据
                            if (total <= hkpageInfo.PageLength)
                            {
                                todayHKItems = WCFServices.Instance.QueryHKHisotryEntrust(out total, out errorMsg, hkFilter, hkpageInfo);
                            }
                            else
                            {
                                #region 多页数据循环查询
                                int pageSize = total / hkpageInfo.PageLength;
                                if (total % hkpageInfo.PageLength != 0)
                                {
                                    pageSize += 1;
                                }

                                for (int k = 1; k <= pageSize; k++)
                                {
                                    smartPool.QueueWorkItem(WirteQueryMessge, txtHKMessage + "状态:" + i + "第" + k + "/" + pageSize + "页");

                                    hkpageInfo.CurrentPage = k;
                                    hkpageInfo.IsCount = false;//为了提速度

                                    List<HK_HistoryEntrustInfo> listHK = WCFServices.Instance.QueryHKHisotryEntrust(out total, out errorMsg, hkFilter, hkpageInfo);
                                    if (listHK != null && listHK.Count > 0)
                                    {
                                        todayHKItems.AddRange(listHK);
                                    }
                                }
                                #endregion
                            }

                            #region 本次状态的统计
                            if (todayHKItems != null)
                            {
                                #region 不要过滤后面统考要用
                                //todayHKItems = todayHKItems.FindAll(new Predicate<HK_HistoryEntrustInfo>((HK_HistoryEntrustInfo model) => { if (model.HasDoneProfit != 0) { return true; } else { return false; } }));
                                #endregion

                                foreach (var item in todayHKItems)
                                {
                                    AnalysisEntry model = new AnalysisEntry();
                                    //为了后面的转型为null异常处理
                                    model.Canceled = "0";
                                    model.PartRemoved = "0";

                                    model.BreedClassName = item.Code + "@";
                                    model.None = "";

                                    switch ((GTA.VTS.Common.CommonObject.Types.TransactionDirection)item.BuySellTypeID)
                                    {
                                        case GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying:
                                            model.PartRemoved = item.EntrustMount.ToString();//买笔数
                                            break;
                                        case GTA.VTS.Common.CommonObject.Types.TransactionDirection.Selling:
                                            model.Canceled = item.EntrustMount.ToString();   //卖笔数
                                            break;
                                    }

                                    model.Dealed = item.TradeAmount.ToString();//成交笔数
                                    model.Removed = item.CancelAmount.ToString();//撤单笔数
                                    model.Count = item.EntrustMount.ToString();        //总笔数

                                    model.FloatProfitLoss = item.HasDoneProfit.ToString(); ;//浮动盈亏
                                    model.MarketProfitLoss = "0";//盯市盈亏
                                    model.RequiredSoon = "4";//标识品种类型
                                    AddOrUpdate(model);

                                    hkHasDoneProfitLoss += double.Parse(item.HasDoneProfit.ToString());
                                }
                            }
                            #endregion
                        }
                    }
                    modelHK.FloatProfitLoss = hkHasDoneProfitLoss.ToString();
                    //用完后即清算
                    todayHKItems.Clear();
                    AddHistoryList(modelHK);
                    LogHelper.WriteDebug("End查询港股...");

                    #endregion
                    break;
            }

        }


        /// <summary>
        /// 写查询信息
        /// </summary>
        /// <param name="txt"></param>
        private void WirteQueryMessge(string txt)
        {
            this.Invoke(new MethodInvoker(() => { this.labMessage.Text = txt; }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool FindHasDone(XH_HistoryEntrustTableInfo info)
        {

            if (info.HasDoneProfit > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        /// <summary>
        /// 对分析历史列表添加数据
        /// </summary>
        public void AddHistoryList(AnalysisEntry model)
        {
            listLock.EnterWriteLock();
            bool isBind = false;
            try
            {
                Historylist.Add(model);
                LogHelper.WriteDebug(model.BreedClassName + model.Count + "当前记录" + Historylist.Count);
                if (Historylist.Count >= 4)
                {
                    isBind = true;
                }
            }
            finally
            {
                listLock.ExitWriteLock();
            }
            if (isBind)
            {
                LogHelper.WriteDebug("开始");
                this.Invoke(new MethodInvoker(() =>
                {
                    this.dgvHistoryTradeAnals.DataSource = null;
                    this.dgvHistoryTradeAnals.DataSource = Historylist;

                    this.dgvCodeListAnalysis.DataSource = null;
                    SortableBindingList<AnalysisEntry> detailList = new SortableBindingList<AnalysisEntry>(detaillList.Values.ToList<AnalysisEntry>());
                    this.dgvCodeListAnalysis.DataSource = detailList;

                    this.btnQueryHistoryAnalysis.Enabled = true;
                    this.dtpEndDate.Enabled = true;
                    this.dtpStartDate.Enabled = true;
                }));
            }
        }
        /// <summary>
        /// 对分析历史列表清空数据
        /// </summary>
        public void ClearHistoryList()
        {
            listLock.EnterWriteLock();
            try
            {
                Historylist.Clear();
                detaillList.Clear();
            }
            finally
            {
                listLock.ExitWriteLock();
            }

        }

        /// <summary>
        /// 没有就添加(BreedClassNmae期货加上开平类型）
        /// </summary>
        /// <param name="item"></param>
        private void AddOrUpdate(AnalysisEntry item)
        {
            //考滤开始的时候已经分开每个品种一个线程跑，而每个线程跑的代码不一样，而每个品种只有一个线程在跑，所以这里不再读写锁，
            //如果加上就会变慢了就等于前面不开线程一样了
            //dicLock.EnterWriteLock();

            try
            {
                string key = item.BreedClassName;
                item.BreedClassName = item.BreedClassName.Substring(0, item.BreedClassName.IndexOf("@"));
                if (!detaillList.ContainsKey(key))
                {
                    detaillList.Add(key, item);
                }
                else
                {
                    var model = detaillList[key];

                    model.Canceled = (float.Parse(model.Canceled) + float.Parse(item.Canceled)).ToString();   //卖笔数
                    model.PartRemoved = (float.Parse(model.PartRemoved) + float.Parse(item.PartRemoved)).ToString();//买笔数

                    model.Dealed = (float.Parse(model.Dealed) + float.Parse(item.Dealed)).ToString("00.00");//成交笔数
                    model.Removed = (float.Parse(model.Removed) + float.Parse(item.Removed)).ToString("00.00");//撤单笔数

                    model.Count = (float.Parse(model.Count) + float.Parse(item.Count)).ToString("00.00");        //总笔数

                    model.FloatProfitLoss = (float.Parse(model.FloatProfitLoss) + float.Parse(item.FloatProfitLoss)).ToString("00.00");//浮动盈亏
                    model.MarketProfitLoss = (float.Parse(model.MarketProfitLoss) + float.Parse(item.MarketProfitLoss)).ToString("00.00");//盯市盈亏
                    //model.IsRequired=item.m   //类型
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
            finally
            {
                //dicLock.ExitWriteLock();
            }


        }
        /// <summary>
        /// 窗体正在关闭把线程关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmStatisticalAnalysis_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (listThread != null)
            {
                try
                {
                    foreach (var item in listThread)
                    {
                        item.Value.Abort();
                    }
                    smartPool.Shutdown();
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }
        }



        /// <summary>
        /// 加上行号   
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCodeListAnalysis_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(dgvCodeListAnalysis.RowHeadersDefaultCellStyle.ForeColor);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(CultureInfo.CurrentUICulture),
                                  dgvCodeListAnalysis.DefaultCellStyle.Font, b, e.RowBounds.Location.X + 20,
                                  e.RowBounds.Location.Y + 4);
        }

        /// <summary>
        ///  双击绑定相关明细列表数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvHistoryTradeAnals_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            this.dgvCodeListAnalysis.DataSource = null;
            SortableBindingList<AnalysisEntry> bindItems = null;
            List<AnalysisEntry> bindList = new List<AnalysisEntry>();
            foreach (DataGridViewRow row in this.dgvHistoryTradeAnals.SelectedRows)
            {

                AnalysisEntry message = row.DataBoundItem as AnalysisEntry;
                string seletStr = "";
                switch (message.BreedClassName)
                {
                    case "商品期货":

                        //todayHKItems = todayHKItems.FindAll(
                        //   new Predicate<HK_HistoryEntrustInfo>((HK_HistoryEntrustInfo model) => { 
                        //        if (model.HasDoneProfit != 0) {
                        //            return true;
                        //        } else { return false; } }));
                        //public delegate TResult Func<T, TResult>(T arg);
                        //public delegate bool Predicate<T>(T obj);
                        //detaillList.se(new Func<KeyValuePair<string,AnalysisEntry>, List<AnalysisEntry>>(
                        //       () =>
                        //        {
                        //            List<AnalysisEntry> items = new List<AnalysisEntry>();
                        //            return items;
                        //        }));
                        seletStr = "1";

                        //  IEnumerable<int> squares = Enumerable.Range(1, 10).Select(x => x * x);
                        break;
                    case "股指期货":
                        seletStr = "2";
                        break;
                    case "现货":

                        seletStr = "3";
                        break;
                    case "港股":
                        seletStr = "4";
                        break;

                }


                bindList = detaillList.Values.ToList<AnalysisEntry>().FindAll(
                               new Predicate<AnalysisEntry>((AnalysisEntry model) =>
                               {
                                   if (model.RequiredSoon == seletStr)
                                   {
                                       return true;

                                   }
                                   else
                                   {
                                       return false;
                                   }

                               }));
                bindItems = new SortableBindingList<AnalysisEntry>(bindList);
                this.dgvCodeListAnalysis.DataSource = bindItems;

            }
        }


    }
}
