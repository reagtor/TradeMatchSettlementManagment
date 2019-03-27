using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ReckoningCapabilityTest.TransactionManageService;

namespace ReckoningCapabilityTest
{
    public partial class TransManageTestUI : Form
    {
        public TransManageTestUI()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 测试开户方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddUser_Click(object sender, EventArgs e)
        {
            TransactionManageClient transactionManageClient = new TransactionManageClient();
            string mess = string.Empty;
            List<AccountEntity> accountEntities;
            UM_UserInfo umUserInfo = new UM_UserInfo();
            InitFund initFund = new InitFund();
            initFund.HK = 1;
            initFund.RMB = 1;
            initFund.US = 1;
            umUserInfo.UserName = "aa";
            umUserInfo.Password = "aa";
            UM_UserInfo userInfo = transactionManageClient.AddTransactionFP(out accountEntities, out mess, "admin", "pwhWQFzLWcw=", umUserInfo,
                                                        initFund);

            UM_UserInfo uu = userInfo;
            List<AccountEntity> accountEntitiesList = accountEntities;
            MessageBox.Show("A");
        }

        /// <summary>
        /// 测试追加资金
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddFund_Click(object sender, EventArgs e)
        {
            TransactionManageClient transactionManageClient = new TransactionManageClient();
            string mess = string.Empty;
            UM_FundAddInfo umFundAddInfo = new UM_FundAddInfo();
            umFundAddInfo.UserID = 29;
            umFundAddInfo.HKNumber = 100;
            umFundAddInfo.USNumber = 100;
            umFundAddInfo.RMBNumber = 100;
            bool reust = transactionManageClient.AddFundFP(out mess, umFundAddInfo);
            if (reust)
            {
                MessageBox.Show("追加资金成功!");
            }
        }

        /// <summary>
        /// 查询交易员资金测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryMoney_Click(object sender, EventArgs e)
        {
            TransactionManageClient transactionManageClient = new TransactionManageClient();
            string mess = string.Empty;
            
            List<TradersAccountCapitalInfo> tradersAccountCapitalInfos=new List<TradersAccountCapitalInfo>();
            tradersAccountCapitalInfos=transactionManageClient.AdminFindTraderCapitalAccountInfoByIDFP(out mess, string.Empty, string.Empty, "29");
            if (tradersAccountCapitalInfos.Count > 0)
            {
                MessageBox.Show("查询成功!");
            }
        }

        /// <summary>
        /// 测试转账
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTranMoney_Click(object sender, EventArgs e)
        {
            TransactionManageClient transactionManageClient = new TransactionManageClient();
                        string mess = string.Empty;
            bool reust=transactionManageClient.ConvertFreeTransferEntityFP(out mess, 29, 1, 2, 1, 1);
            if (reust)
            {
                MessageBox.Show("追加资金成功!");
            }
        }
    }
}
