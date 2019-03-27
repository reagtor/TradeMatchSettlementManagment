using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ReckoningCapabilityTest.DoOrderS8090;

namespace ReckoningCapabilityTest
{
    public partial class StockXdTestUI : Form
    {
        public StockXdTestUI()
        {
            InitializeComponent();
        }

        /// <summary>
        /// //声明一个委托
        /// </summary>
        /// <param name="msg"></param>
        public delegate void UpdateMsg(string msg);

        /// <summary>
        /// 现货下单请求实体( 同1秒钟1000个用户下一笔单调用的方法用)
        /// </summary>
        public StockOrderRequest stockOrderRequest = new StockOrderRequest();

        /// <summary>
        /// 声明执行现货下单的委托
        /// </summary>
        /// <param name="stockOrderRequest"></param>
        public delegate void StockXdOrder(StockOrderRequest stockOrderRequest);

        /// <summary>
        /// 现货下单信息list
        /// </summary>
        private List<StockOrderRequest> stockOrderRequestList = new List<StockOrderRequest>();

        /// <summary>
        /// 下单总秒数
        /// </summary>
        private int  allTimeSeconds =0;

        /// <summary>
        /// 柜台服务
        /// </summary>
        //private DoOrderClient doOrderClient = new DoOrderClient();

        /// <summary>
        /// 用来存放现货每一笔单的时间
        /// </summary>
        private List<int> addTimeList=new List<int>();

        #region  100个现货帐户(备份)
        //100个现货帐户
        //string[] xAccountList = { 
        //                         "010000004402", "010000004502", "010000004602", "010000004702", "010000004802", "010000004902" 
        //                        ,"010000005002","010000005102","010000005202","010000005302","010000005402","010000005502","010000005602",
        //                        "010000005702","010000005802","010000005902","010000006002","010000006102","010000006202","010000006302",
        //                        "010000006402","010000006502","010000006602","010000006702","010000006802","010000006902","010000007002",
        //                        "010000007102","010000007202","010000007302","010000007402","010000007502","010000007602","010000007702",
        //                        "010000007802","010000007902","010000008102","010000008202","010000008302","010000008502","010000008602",
        //                        "010000008702","010000008802","010000008902","010000009002","010000009102","010000009202","010000009302",
        //                        "010000009402","010000009502","010000009602","010000009702","010000009802","010000009902","010000010002",
        //                        "010000010102","010000010202","010000010302","010000010402","010000010502","010000010602","010000010702",
        //                        "010000010802","010000010902","010000011002","010000011102","010000011202","010000011302","010000011402",
        //                        "010000011502","010000011602","010000011702","010000011802","010000011902","010000012002","010000012102",
        //                        "010000012202","010000012302","010000012402","010000012502","010000012602","010000012702","010000012802",
        //                        "010000012902","010000013002","010000013102","010000013202","010000013302","010000013402","010000013502",
        //                        "010000013602","010000013702","010000013802","010000013902","010000014002","010000014102","010000014202",
        //                        "010000014302","010000014402","010000014502"
        //                        };

        #endregion


        /// <summary>
        /// 初始化现货下单数据(1个帐户下单调用的方法)
        /// </summary>
        private void InitXhOrder()
        {
            stockOrderRequest.BuySell = DoOrderS8090.TypesTransactionDirection.Buying;
            stockOrderRequest.Code = "000005";// "000001";// "031005";
            stockOrderRequest.ChannelID = "0";
            stockOrderRequest.FundAccountId ="010000004402";// "010000002402";
            //stockOrderRequest.ExtensionData = // stockIndexFuturesOrderRequest.OpenCloseType("Buying");
            stockOrderRequest.OrderAmount = 100;
            stockOrderRequest.OrderPrice = 3.5f;
            stockOrderRequest.OrderUnitType = DoOrderS8090.TypesUnitType.Thigh;
            stockOrderRequest.OrderWay = DoOrderS8090.TypesOrderPriceType.OPTLimited;
            //stockOrderRequest.TraderId = "44";// "24";
            stockOrderRequest.TraderPassword = "888888";
            stockOrderRequest.PortfoliosId = "0";
        }


        /// <summary>
        /// 初始化现货下单数据(多个帐户下单调用的方法 (10,100个帐户))
        /// </summary>
        private void InitXhOrder(string xAccount)
        {
            StockOrderRequest stockOrderRequest = new StockOrderRequest();
            stockOrderRequest.BuySell = DoOrderS8090.TypesTransactionDirection.Buying;
            stockOrderRequest.Code = "000005";// "000001";// "031005";
            stockOrderRequest.ChannelID = "0";
            stockOrderRequest.FundAccountId = xAccount; //"010000004402";// "010000002402";
            //stockOrderRequest.ExtensionData = // stockIndexFuturesOrderRequest.OpenCloseType("Buying");
            stockOrderRequest.OrderAmount = 100;
            stockOrderRequest.OrderPrice = 3.5f;
            stockOrderRequest.OrderUnitType = DoOrderS8090.TypesUnitType.Thigh;
            stockOrderRequest.OrderWay = DoOrderS8090.TypesOrderPriceType.OPTLimited;
            //stockOrderRequest.TraderId = "44";// "24";
            stockOrderRequest.TraderPassword = "888888";
            stockOrderRequest.PortfoliosId = "0";
            stockOrderRequestList.Add(stockOrderRequest);
        }

        /// <summary>
        /// 现货下单测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXHOrderService_Click(object sender, EventArgs e)
        {
            //下单请求时间
            labXhXDTime.Text = DateTime.Now.ToString();

            #region 同一秒一个帐户下1000笔单的测试
            ////同一秒一个帐户下1000笔单的测试
            //this.InitXhOrder();
            //int count = 1000;

            //Action action = new Action(XhOrderTest);
            //for (int i = 0; i < count; i++)
            //{
            //    action.BeginInvoke(null, null);
            //}
            #endregion

            #region 同一秒10帐户下1000笔单的测试
            ////同一秒一个10帐户下1000笔单的测试

            //10个现货帐户
            //string[] xAccountList = { 
            //                         "010000004402", "010000004502", "010000004602", "010000004702", "010000004802", "010000004902" 
            //                        ,"010000005002","010000005102","010000005202","010000005302"
            //                        };
            #endregion

            #region 同一秒100帐户下1000笔单的测试  //100个现货帐户
            string[] xAccountList = { 
                                 "010000004402", "010000004502", "010000004602", "010000004702", "010000004802", "010000004902" 
                                ,"010000005002","010000005102","010000005202","010000005302","010000005402","010000005502","010000005602",
                                "010000005702","010000005802","010000005902","010000006002","010000006102","010000006202","010000006302",
                                "010000006402","010000006502","010000006602","010000006702","010000006802","010000006902","010000007002",
                                "010000007102","010000007202","010000007302","010000007402","010000007502","010000007602","010000007702",
                                "010000007802","010000007902","010000008102","010000008202","010000008302","010000008502","010000008602",
                                "010000008702","010000008802","010000008902","010000009002","010000009102","010000009202","010000009302",
                                "010000009402","010000009502","010000009602","010000009702","010000009802","010000009902","010000010002",
                                "010000010102","010000010202","010000010302","010000010402","010000010502","010000010602","010000010702",
                                "010000010802","010000010902","010000011002","010000011102","010000011202","010000011302","010000011402",
                                "010000011502","010000011602","010000011702","010000011802","010000011902","010000012002","010000012102",
                                "010000012202","010000012302","010000012402","010000012502","010000012602","010000012702","010000012802",
                                "010000012902","010000013002","010000013102","010000013202","010000013302","010000013402","010000013502",
                                "010000013602","010000013702","010000013802","010000013902","010000014002","010000014102","010000014202",
                                "010000014302","010000014402","010000014502"
                                };

            #endregion

            //初始化现货下单数据
            string _xAccount = string.Empty;
            for (int i = 0; i < xAccountList.Length; i++)
            {
                _xAccount = xAccountList[i];
                this.InitXhOrder(_xAccount);
            }
            allTimeSeconds = 0;

            //循环多上帐户并执行现货下单
            for (int i = 0; i < stockOrderRequestList.Count; i++)
            {
                int count = 10;// 100;//1000;
                StockOrderRequest stockOrderRequest = stockOrderRequestList[i];
                StockXdOrder stockXdOrder = new StockXdOrder(XhOrderTest);
                for (int j = 0; j < count; j++)
                {
                    stockXdOrder.BeginInvoke(stockOrderRequest, null, null);
                }
            }
            
        }

        /// <summary>
        /// 多个帐户下单调用的方法(10,100个帐户)
        /// </summary>
        /// <param name="stockOrderRequest"></param>
        private void XhOrderTest(StockOrderRequest stockOrderRequest)
        {
            //DoOrderClient doOrderClient = new DoOrderClient();
            string xhOrderID = string.Empty;
            //下单
            if (stockOrderRequestList.Count != 0)
            {
                DoOrderClient doOrderClient = new DoOrderClient();
                DateTime startTime = DateTime.Now;

                xhOrderID = doOrderClient.DoStockOrder(stockOrderRequest).OrderId;

                DateTime endTime = DateTime.Now;
                TimeSpan timeSpan = endTime - startTime;
                int span = timeSpan.Milliseconds;
                //allTimeSeconds += span;

                //把每笔单的时间添加到list中
                addTimeList.Add(span);

                if (!string.IsNullOrEmpty(xhOrderID))
                {
                    UpdateVaule(xhOrderID); //调用委托方法
                }
            }
        }

        /// <summary>
        ///  同1秒钟1个帐户同下1000笔单调用的方法
        /// </summary>
        private void XhOrderTest()
        {
            //DoOrderClient doOrderClient = new DoOrderClient();
             string xhOrderID = string.Empty;
            //下1000笔单
            if (stockOrderRequest != null)
            {
                DoOrderClient doOrderClient = new DoOrderClient();
                DateTime startTime = DateTime.Now;

                xhOrderID = doOrderClient.DoStockOrder(stockOrderRequest).OrderId;

                DateTime endTime = DateTime.Now;
                TimeSpan timeSpan = endTime - startTime;
                int span = timeSpan.Milliseconds;
               // allTimeSeconds += span;
                //把毫秒换算成秒
                //int allTimeS = span/1000;
                //把每笔单的时间添加到list中
                addTimeList.Add(span);

                if (!string.IsNullOrEmpty(xhOrderID))
                {
                    UpdateVaule(xhOrderID); //调用委托方法
                }
            }
        }

        /// <summary>
        /// 把委托单号显示在listBox上
        /// </summary>
        /// <param name="p"></param>
        private void UpdateVaule(string p)//委托方法
        {
            if (listBox1.InvokeRequired)//如果跨线程调用
            {
                this.listBox1.Invoke(new UpdateMsg(UpdateVaule), new object[] { p });//Invoke委托方法 

            }
            else
            {

                //if (listBox1.Items.Count >= 30)
                //{
                //    this.listBox1.Items.RemoveAt(29);
                //}
                this.listBox1.Items.Add(p + " 时间：" + DateTime.Now);
            }
            
        }

        /// <summary>
        /// 显示现货下单平均时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisXhXdAgeTime_Click(object sender, EventArgs e)
        {
            int timeS = 0;
            if (addTimeList.Count > 0)
            {
                for (int i = 0; i < addTimeList.Count; i++)
                {
                    timeS += addTimeList[i];
                }
                
                //把毫秒换算成秒
                float allTimeS = timeS/1000;

                //显示总秒数
                labAllTimeS.Text = allTimeS.ToString();

                //算平均时间
                labAgeTime.Text = (1000/allTimeS).ToString();
                addTimeList.Clear();
            }

           
        }



    }
}
