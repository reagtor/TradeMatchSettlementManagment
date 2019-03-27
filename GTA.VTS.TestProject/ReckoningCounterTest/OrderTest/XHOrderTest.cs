using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounterTest.CounterOrderService;

namespace ReckoningCounterTest.OrderTest
{
    internal delegate void ProcessDoOrderHandler(int iOrderCount,DoOrderClient client,int selectIndex);
    public partial class XHOrderTest : Form
    {
        public XHOrderTest()
        {
            InitializeComponent();
        }

        private OrderDealRptClient rptClient = null;
        private InstanceContext ic = null;

        private const string ClientID = "TTTEST";

        private void XHOrderTest_Load(object sender, EventArgs e)
        {
            var tttt = new OrderCallbackProcess();
            tttt.Handler += new EventHandler(tttt_Handler);
            ic = new InstanceContext(tttt);
            rptClient = new OrderDealRptClient(ic, "NetTcpBinding_IOrderDealRpt",
                System.Configuration.ConfigurationSettings.AppSettings["serviceAddress"]);
            rptClient.Open();

            rptClient.RegisterChannel(ClientID);

            this.cbUnit.SelectedIndex = 1;
            this.cbfx.SelectedIndex = 1;
        }

        void tttt_Handler(object sender, EventArgs e)
        {
            MessageDisplayHelper.Event(sender.ToString(), this.listBox1);
        }

        private void cbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        StockOrderRequest BuildOrder(int selectIndex)
        {
            var result = new StockOrderRequest();
            result.Code = this.tbCode.Text;
            result.ChannelID = ClientID;
            result.OrderAmount = int.Parse(tbAmount.Text);
            result.BuySell = selectIndex == 0
                                 ? TypesTransactionDirection.Buying
                                 : TypesTransactionDirection.Selling;
            result.OrderUnitType = TypesUnitType.Hand;
                                     
            result.OrderPrice = double.Parse(this.tbPrice.Text);
            result.OrderWay = TypesOrderPriceType.OPTLimited;
            return result;
        }
        void ProcessDoOrder(int iOrderCount, DoOrderClient client, int selectIndex)
        {
            for (int i = 0; i < iOrderCount; i++)
            {
                OrderResponse or = client.DoStockOrder(BuildOrder(selectIndex));
                MessageDisplayHelper.Event( "OrderID:" + or.OrderId + " Message:" + or.OrderMessage,this.listBox1);
            }
        }

        private void btnDoOrder_Click(object sender, EventArgs e)
        {
            DoOrderClient doc = new DoOrderClient("NetTcpBinding_IDoOrder",System.Configuration.ConfigurationSettings.AppSettings["serviceAddress"]);

            ProcessDoOrderHandler doh = new ProcessDoOrderHandler(this.ProcessDoOrder);
            doh.BeginInvoke(int.Parse(textBox1.Text), doc,this.cbfx.SelectedIndex, null, null);
            
           
        }
    }
}
