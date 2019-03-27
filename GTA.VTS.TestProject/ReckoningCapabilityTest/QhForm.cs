using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ReckoningCapabilityTest.DoAccountAndCapitalManagementService;
using ReckoningCapabilityTest.TraderFindService;

namespace ReckoningCapabilityTest
{
    public partial class QhForm : Form
    {
        public QhForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 期货最大委托量查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFuteMax_Click(object sender, EventArgs e)
        {
            DoAccountAndCapitalManagementService.AccountAndCapitalManagementClient accountAndCapitalManagementClient=new AccountAndCapitalManagementClient();
            string a = "1";
            //long maxAccount = accountAndCapitalManagementClient.GetFutureMaxOrderAmount(out a,"23", 600,"IF0909",DoAccountAndCapitalManagementService.TypesOrderPriceType.OPTMarketPrice);
            long maxAccount = accountAndCapitalManagementClient.GetFutureMaxOrderAmount(out a, "18", 600, "IF0910", DoAccountAndCapitalManagementService.TypesOrderPriceType.OPTMarketPrice);

            //accountAndCapitalManagementClient.GetFutureMaxOrderAmount()
            maxAccount.ToString();
            MessageBox.Show("maxAccount", "期货最大委托量");              
        }

        private void btnQhHold_Click(object sender, EventArgs e)
        {
            TraderFindService.TraderFindClient traderFindClient=new TraderFindClient();

            FuturesHoldConditionFindEntity findEntity=new FuturesHoldConditionFindEntity();
            //findEntity.CurrencyType = TraderFindService.;
            findEntity.ContractCode = "IF0909";
            string a = "0";
            int n = 0;
            List<FuturesHoldFindResultEntity> list = new List<FuturesHoldFindResultEntity>();
           list[0]=traderFindClient.FuturesHoldFind(out n,"010000002306", "888888", findEntity, 0, 0,ref a)[0];

           
        }

        /// <summary>
        /// 现货最大委托量查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXHMaxAccount_Click(object sender, EventArgs e)
        {
            DoAccountAndCapitalManagementService.AccountAndCapitalManagementClient accountAndCapitalManagementClient = new AccountAndCapitalManagementClient();
            string a = "1";
            long maxAccount = accountAndCapitalManagementClient.GetSpotMaxOrderAmount(out a, "249", 100, "019711", DoAccountAndCapitalManagementService.TypesOrderPriceType.OPTMarketPrice);
           // accountAndCapitalManagementClient.GetSpotMaxOrderAmount()
            maxAccount.ToString();
            MessageBox.Show("maxAccount", "现货最大委托量");    
        }

       
    }
}
